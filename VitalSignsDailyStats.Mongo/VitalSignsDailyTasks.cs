using System;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VSFramework;
using LogUtilities;
using System.IO;
using System.Data.SqlClient;
using System.Globalization;
using System.Threading;
using static LogUtilities.LogUtils;
using RPRWyatt.VitalSigns.Services;
using Ionic.Zip;
using VSNext.Mongo.Entities;
using VSNext.Mongo.Repository;
using System.Collections.Generic;

namespace VitalSignsDailyStats
{
   
    public partial class VitalSignsDailyTasks : VSServices
    {
        private  UnitOfWork _unitOfWork;
       
       
        IRepository<DailyStatistics> dailyStatasticsRepository = _unitOfWork.Repository<DailyStatistics>();
        IRepository<SummaryStatistics> summaryStatasticsRepository =_unitOfWork.Repository<SummaryStatistics>();
        VSAdaptor objVsAdaptor = new VSAdaptor();
        string culture = "en-US";
        string cultureName = "CultureString";
        LogUtils.LogLevel logLevel;
        string logDest, appPath, auditText, htmlPath, statisticsPath, serversMdPath,
            dateFormat, productName ;
        string companyName = "JNIT Inc. dba RPR Wyatt";
        LogUtils.LogLevel myLogLevel;
        int builddNumber;
        string[] dominoDiskNames = new string[101];
        DateUtils.DateUtils objDateUtils = new DateUtils.DateUtils();
        string[] MicrosoftDiskNames = new string[101];
        bool timeToStop = false;
        public VitalSignsDailyTasks()
        {
            InitializeComponent();
            string connetionString = System.Configuration.ConfigurationManager.AppSettings["MongoConnectionString"];
            int? tenantId = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["TenantId"]);
            _unitOfWork = new UnitOfWork(connetionString, tenantId);
        }

        protected override void OnStart(string[] args)
        {
        }

        protected override void OnStop()
        {
            try
            {
                WriteAuditEntry(DateTime.Now.ToString() + " Shutting down....");
                timeToStop = true;
                RegistryHandler myRegistry = new RegistryHandler();
                myRegistry.WriteToRegistry("Daily Tasks End", DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString());


            }
            catch (Exception ex)
            {
            }
            base.OnStop();
        }

        protected override void ServiceOnStart(string[] args)
        {
            try
            {
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings[cultureName]))
                    culture = ConfigurationManager.AppSettings[cultureName];
                  
            RegistryHandler myRegistry = new RegistryHandler();
            
                    logLevel = myRegistry.ReadFromRegistry("Log Level") == null ? LogUtils.LogLevel.Verbose : (LogUtils.LogLevel)myRegistry.ReadFromRegistry("Log Level");


                appPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                appPath = string.IsNullOrEmpty(appPath) ? @"c:\" : appPath;
                dateFormat = objDateUtils.GetDateFormat();
               logDest = appPath + @"\Log_Files\Daily_Tasks_Log.txt";
              if (File.Exists(logDest))
                {
                    File.Move(logDest, appPath + @"\Log_Files\Daily_Tasks_Log_Bak.txt");
                    File.Delete(logDest);
                }
                myRegistry.WriteToRegistry("Daily Tasks Start", DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString());
                myRegistry.WriteToRegistry("Daily Tasks Build", builddNumber);
                // productName =string.IsNullOrEmpty myRegistry.ReadFromRegistry("ProductName");
                productName = Convert.ToString(myRegistry.ReadFromRegistry("ProductName"));
               if( string.IsNullOrEmpty(productName))
                    productName= "VitalSigns";

                WriteAuditEntry(DateTime.Now.ToString() + " VitalSigns Daily Tasks service is starting up.");
                WriteAuditEntry(DateTime.Now.ToString() + " VitalSigns Daily Tasks Build Number: " + builddNumber);
                WriteAuditEntry(DateTime.Now.ToString() + " Copyright " + companyName + "  " + DateTime.Now.Year + " - All rights reserved." + "\r\n" + "\r\n");
               // DailyBackup();
                bool isPrimaryNode = true;
                string sql = null;
                try
                {

                    if ((System.Configuration.ConfigurationManager.AppSettings["VSNodeName"] != null))
                    {
                        VSFramework.XMLOperation myConnectionString = new VSFramework.XMLOperation();

                        string nodeName = System.Configuration.ConfigurationManager.AppSettings["VSNodeName"].ToString();
                        sql = "SELECT IsPrimaryNode From Nodes WHERE Name='" + nodeName + "'";

                        DataTable dt = objVsAdaptor.FetchData(myConnectionString.GetDBConnectionString("VitalSigns"),sql);
                        if ((dt.Rows.Count > 0))
                        {
                            isPrimaryNode = Convert.ToBoolean(dt.Rows[0][0].ToString());
                        }

                    }

                }
                catch (Exception ex)
                {
                    WriteAuditEntry("Exception checking if primary node.  Error: " + ex.Message);
                }

                if (!isPrimaryNode)
                {
                    WriteAuditEntry("Daily Task is finished since it is not the Primary Node....");
                    this.Stop();
                    return;
                }

                try
                {
                    WriteAuditEntry("Building a list of all unique Domino disk drives, if any. ");
                    BuildDominoDriveList();
                }
                catch (Exception ex)
                {
                    WriteAuditEntry("Exception building Domino drives list ...." + ex.ToString());
                }

                try
                {
                    WriteAuditEntry("Building a list of all unique Microsoft server disk drives, if any. ");
                    BuildMicrosoftDriveList();
                }
                catch (Exception ex)
                {
                    WriteAuditEntry("Exception building Microsoft server disk drives list ...." + ex.ToString());
                }
                ConsolidateStatistics();
                    try
                {
                   // CleanUpObsoleteData();
                }
                catch (Exception ex)
                {
                    WriteAuditEntry("OOPS, error cleaning up old data...." + ex.ToString());
                }
                try
                {
                    //Durga VSPLUS 1874 6/26/2015
                    Shrinkdb("VitalSigns");
                    Shrinkdb("VSS_Statistics");
                }
                catch (Exception ex)
                {
                    WriteAuditEntry("OOPS, error  to Shrink Databases...." + ex.ToString());
                }
                //6/25/2015 NS added for VSPLUS-1226

                {
                    try
                    {
                        VSFramework.XMLOperation myConnectionString = new VSFramework.XMLOperation();
                        bool cleanupNow = false;
                       sql = "SELECT CASE WHEN DATEADD(Day,7,CONVERT(DateTime, ISNULL(svalue,DATEADD(Day,-7,GETDATE())), 120)) < GETDATE() " + "THEN 'true' ELSE 'false' END AS CleanupNow FROM Settings WHERE sname='CleanUpTablesDate'";

                        DataTable dt = objVsAdaptor.FetchData(myConnectionString.GetDBConnectionString("VitalSigns"), sql);
                        if ((dt.Rows.Count > 0))
                        {
                            cleanupNow = Convert.ToBoolean(dt.Rows[0][0].ToString());
                        }
                        if (cleanupNow)
                        {
                            WriteAuditEntry(DateTime.Now.ToString() + " Starting weekly cleanup.");
                            CleanupAnyTableWeekly();
                            //Kiran Dadireddy VSPLUS-2684
                            ShrinkDBLogOnWeeklyBasis();
                            sql = "UPDATE Settings SET svalue=GETDATE() WHERE sname='CleanUpTablesDate'";
                            objVsAdaptor.ExecuteNonQueryAny("vitalsigns", "vitalsigns", sql);
                            WriteAuditEntry(DateTime.Now.ToString() + " Updated the Settings table CleanUpTablesDate column.");
                        }
                    }
                    catch (Exception ex)
                    {
                        WriteAuditEntry("Error cleaning up weekly data...." + ex.ToString());
                    }

                    try
                    {
                        WriteAuditEntry("Starting the Log Statistics");
                        LogTableStatistics("Vitalsigns");
                        LogTableStatistics("VSS_Statistics");

                    }
                    catch (Exception ex)
                    {
                        WriteAuditEntry("OOPS, error printing the statistics database" + ex.ToString());
                    }
                    try
                    {
                        WriteAuditEntry("Starting update of local tables");
                        UpdateLocalTables();
                    }
                    catch (Exception ex)
                    {
                        WriteAuditEntry("OOPS, error updating local tables" + ex.ToString());
                    }
                    WriteAuditEntry("Daily Task is finished....");
                    this.Stop();

                }

            }
            catch (Exception ex)
            {
            }
        }

       
        private void WriteAuditEntry(string message, LogUtils.LogLevel logLevel=LogUtils.LogLevel.Normal)
        {
            //base.WriteAuditEntry(message, "Daily_Tasks_Log.txt", logLevel);
           // base.wr
            base.WriteAuditEntry(message, "Daily_Tasks_Log.txt", logLevel);
        }


        //public void DailyBackup()
        //{

        //    try
        //    {
                

        //        string[] LogFilesToBeRecreated = {
        //                                    "History.txt",
        //                                     "Daily_Tasks_Log.txt"
        //                                      };
        //        if (myLogLevel == LogUtils.LogLevel.Verbose)
        //        {
        //            WriteAuditEntry(DateTime.Now.ToString() + " ***** Starting Daily Log File Zip Up ******* ");
        //            WriteAuditEntry("\r\n");
        //        }

              
                
        //        if (!Directory.Exists(appPath + "\\Log_Files\\Backup\\"))
        //         {
        //         Directory.CreateDirectory(appPath + "\\Log_Files\\Backup\\");
        //         }
                
        //        try
        //        {
        //            WriteAuditEntry(DateTime.Now.ToString() + " Deleting any leftover *.txt files from the \\backup folder.");
        //            string[] fileArray = null;
        //            WriteAuditEntry(DateTime.Now.ToString() + " Cleaning up log files");
        //            fileArray = Directory.GetFiles(appPath + "\\Log_Files\\Backup\\", "*.txt");

        //            string myFile = null;
        //            foreach (string myFile_loopVariable in fileArray)
        //            {
        //                myFile = myFile_loopVariable;
        //                File.Delete(myFile);
        //                WriteAuditEntry(DateTime.Now.ToString() + " Deleting " + myFile);
        //            }
        //            fileArray = null;

        //            string[] ExchangeFolders = Directory.GetDirectories(appPath + "\\Log_Files\\Backup\\");
        //            string myFolder = null;
        //            foreach (string myFolder_loopVariable in ExchangeFolders)
        //            {
        //                myFolder = myFolder_loopVariable;
                        
        //               this.Computer.FileSystem.DeleteDirectory(myFolder, Microsoft.VisualBasic.FileIO.DeleteDirectoryOption.DeleteAllContents);
        //                WriteAuditEntry(DateTime.Now.ToString() + " Deleting " + myFolder);
        //            }
        //            ExchangeFolders = null;

        //        }
        //        catch (Exception ex)
        //        {
        //            WriteAuditEntry(DateTime.Now.ToString() + " Error cleaning up past log files " + ex.ToString());
        //        }

        //        WriteAuditEntry("\r\n" + "\r\n" + DateTime.Now.ToString() + "**********************************************");



        //        try
        //        {
        //            string[] fileArray = null;
        //            WriteAuditEntry(DateTime.Now.ToString() + " Moving the current log files to the backup folder");
        //            fileArray = Directory.GetFiles(appPath + "\\Log_Files\\", "*.txt");

        //            string myFile = null;
        //            foreach (string myFile_loopVariable in fileArray)
        //            {
        //                myFile = myFile_loopVariable;
        //                string dest = Path.Combine(appPath + "\\Log_Files\\backup\\", Path.GetFileName(myFile));
        //                WriteAuditEntry(DateTime.Now.ToString() + " Moving " + myFile + " to " + dest);
        //                File.Move(myFile, dest);
        //                if (Array.IndexOf(LogFilesToBeRecreated, Path.GetFileName(myFile)) > -1)
        //                {
        //                    File.Create(Path.GetFileName(myFile));
        //                }

        //            }
        //            fileArray = null;

        //            string[] ExchangeFolders = Directory.GetDirectories(appPath + "\\Log_Files\\");

        //            string folder = null;
        //            foreach (string folder_loopVariable in ExchangeFolders)
        //            {
        //                folder = folder_loopVariable;
        //                if ((Path.GetFileName(folder).ToLower() == "backup"))
        //                {
        //                    continue;
        //                }
        //                string destFolder = appPath + "\\Log_Files\\backup\\" + Path.GetFileName(folder) + "";
        //                if ((this.Computer.FileSystem.DirectoryExists(destFolder) == false))
        //                {
        //                   this.Computer.FileSystem.CreateDirectory(destFolder);
        //                }
        //                WriteAuditEntry(DateTime.Now.ToString() + " Moving folder " + folder + " to " + destFolder);
        //               this.Computer.FileSystem.MoveDirectory(folder, destFolder);


        //            }
        //            ExchangeFolders = null;

        //        }
        //        catch (Exception ex)
        //        {
        //            WriteAuditEntry(DateTime.Now.ToString() + " Error copying the files to backup: " + ex.ToString());
        //        }

        //        //Delete previous zip file
        //        try
        //        {
        //            string myzipfile = "";
        //            myzipfile = appPath + "\\Log_Files\\backup\\" + DateTime.Now.DayOfWeek.ToString() + ".zip";
        //            WriteAuditEntry(DateTime.Now.ToString() + " Deleting prior week's zip file, if present.");
        //            File.Delete(myzipfile);

        //        }
        //        catch (Exception ex)
        //        {
        //        }

        //        //Adds files to be zipped and zips them up
        //        try
        //        {
        //            WriteAuditEntry("\r\n" + "\r\n" + DateTime.Now.ToString() + "**********************************************");
        //            ZipFile myZip = new ZipFile();
        //            string[] zipFileArray = null;
        //            WriteAuditEntry(DateTime.Now.ToString() + " Creating new zip file");
        //            zipFileArray = Directory.GetFiles(appPath + "\\Log_Files\\backup\\", "*.txt");
        //            string myFile = null;
        //            try
        //            {
        //                foreach (string myFile_loopVariable in zipFileArray)
        //                {
        //                    myFile = myFile_loopVariable;
        //                    WriteAuditEntry(DateTime.Now.ToString() + " Zipping " + myFile);
        //                    myZip.AddFile(myFile, "");
        //                    // myZip.AddFile(myFile)
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                WriteAuditEntry(DateTime.Now.ToString() + " Error zipping: " + ex.ToString());
        //            }

        //            string[] zipFolderArray = null;
        //            zipFolderArray = Directory.GetDirectories(appPath + "\\Log_Files\\backup\\");
        //            string myFolder = null;
        //            try
        //            {
        //                foreach (string myFolder_loopVariable in zipFolderArray)
        //                {
        //                    myFolder = myFolder_loopVariable;
        //                    WriteAuditEntry(DateTime.Now.ToString() + " Zipping Folder " + myFolder);
        //                    myZip.AddDirectory(myFolder, Path.GetFileName(myFolder));
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                WriteAuditEntry(DateTime.Now.ToString() + " Error zipping folders: " + ex.ToString());
        //            }

        //            myZip.Save(appPath + "\\Log_Files\\backup\\" + DateTime.Now.DayOfWeek.ToString() + ".zip");
        //            WriteAuditEntry(DateTime.Now.ToString() + " The zip file is created as " + DateTime.Now.DayOfWeek.ToString() + ".zip");
        //        }
        //        catch (Exception ex)
        //        {
        //            WriteAuditEntry(DateTime.Now.ToString() + " Error creating zip file: ");
        //        }

        //        //Delete files in the back up folder after they have been zipped
        //        try
        //        {
        //            WriteAuditEntry(DateTime.Now.ToString() + " Deleting the backup log files. ");
        //            string[] fileArray = null;
        //            fileArray = Directory.GetFiles(appPath + "\\Log_Files\\Backup", "*.txt");

        //            string myFile = null;
        //            foreach (string myFile_loopVariable in fileArray)
        //            {
        //                myFile = myFile_loopVariable;
        //                WriteAuditEntry(DateTime.Now.ToString() + " Deleting " + myFile + "....");
        //                File.Delete(myFile);
        //            }

        //            string[] ExchangeFolders = Directory.GetDirectories(appPath + "\\Log_Files\\Backup\\");
        //            string myFolder = null;
        //            foreach (string myFolder_loopVariable in ExchangeFolders)
        //            {
        //                myFolder = myFolder_loopVariable;
        //                WriteAuditEntry(DateTime.Now.ToString() + " Deleting " + myFolder + "...");
        //               this.Computer.FileSystem.DeleteDirectory(myFolder, Microsoft.VisualBasic.FileIO.DeleteDirectoryOption.DeleteAllContents);
        //            }
        //            ExchangeFolders = null;

        //        }
        //        catch (Exception ex)
        //        {
        //            WriteAuditEntry(DateTime.Now.ToString() + " Error cleaning up the current log files " + ex.ToString());
        //        }




        //        if (myLogLevel == LogUtils.LogLevel.Verbose)
        //        {
        //            WriteAuditEntry(DateTime.Now.ToString() + " Finished Daily log file zip up.");
        //        }
        //        //1537'
        //        LoglevelToNormal();



        //        //............................................................


        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
          

        //}

        private void LoglevelToNormal()
        {
            myLogLevel = LogUtils.LogLevel.Normal;
            RegistryHandler myRegistry = new RegistryHandler();
            myRegistry.WriteToRegistry("Log Level", LogUtils.LogLevel.Normal);
            myRegistry.WriteToRegistry("Log Level VSAdapter", LogUtils.LogLevel.Normal);
        }
        public void BuildDominoDriveList()
        {
            WriteAuditEntry(DateTime.Now.ToString() + " Calculating all the unique drive names. ");
            string sql = "SELECT DISTINCT DiskName FROM DominoDiskSpace";
            System.Data.DataSet myDataSet = new System.Data.DataSet();
            System.Data.DataTable myTable = new System.Data.DataTable();
            myDataSet.Tables.Add(myTable);

            int myDriveCount = 0;

            try
            {
              
                objVsAdaptor.FillDatasetAny("VitalSigns", "statistics", sql, ref myDataSet, "MyTable");
                WriteAuditEntry(DateTime.Now.ToString() + " Filled the dataset ");
                myDriveCount = myDataSet.Tables["MyTable"].Rows.Count;
                WriteAuditEntry(DateTime.Now.ToString() + " The dataset has " + myDriveCount + " unique drive names.");
                // ERROR: Not supported in C#: ReDimStatement

            }
            catch (Exception ex)
            {
                WriteAuditEntry(DateTime.Now.ToString() + " Error creating Daily Summary " + ex.Message + "-- The failed Average command was " + sql);
            }

            int n = 0;
            System.Data.DataView myView = new System.Data.DataView(myDataSet.Tables["MyTable"]);
            DataRowView drv = null;

            foreach (DataRowView drv_loopVariable in myView)
            {
                drv = drv_loopVariable;
                try
                {
                    WriteAuditEntry(DateTime.Now.ToString() + " Found drive " + drv["DiskName"]);
                    dominoDiskNames[n] =Convert.ToString(drv["DiskName"]);
                    n += 1;

                }
                catch (Exception ex)
                {
                    WriteAuditEntry(DateTime.Now.ToString() + " Exception creating SQL: " + ex.ToString());
                }

            }

        }
        public void BuildMicrosoftDriveList()
        {
            WriteAuditEntry(DateTime.Now.ToString() + " Calculating all the unique drive names. ");
            string strSQL = "SELECT DISTINCT DiskName FROM DiskSpace";
            System.Data.DataSet myDataSet = new System.Data.DataSet();
            System.Data.DataTable myTable = new System.Data.DataTable();
            myDataSet.Tables.Add(myTable);

            int myDriveCount = 0;

            try
            {
                
                objVsAdaptor.FillDatasetAny("VitalSigns", "statistics", strSQL,ref myDataSet, "MyTable");
                WriteAuditEntry(DateTime.Now.ToString() + " Filled the dataset ");
                myDriveCount = myDataSet.Tables["MyTable"].Rows.Count;
                WriteAuditEntry(DateTime.Now.ToString() + " The dataset has " + myDriveCount + " unique drive names.");
              

            }
            catch (Exception ex)
            {
                WriteAuditEntry(DateTime.Now.ToString() + " Error creating Daily Summary " + ex.Message + "-- The failed Average command was " + strSQL);
            }

            int n = 0;
            System.Data.DataView myView = new System.Data.DataView(myDataSet.Tables["MyTable"]);
            DataRowView drv = null;

            foreach (DataRowView drv_loopVariable in myView)
            {
                drv = drv_loopVariable;
                try
                {
                    WriteAuditEntry(DateTime.Now.ToString() + " Found drive " + drv["DiskName"]);
                    MicrosoftDiskNames[n] = Convert.ToString(drv["DiskName"]);
                    n += 1;

                }
                catch (Exception ex)
                {
                    WriteAuditEntry(DateTime.Now.ToString() + " Exception creating SQL: " + ex.ToString());
                }

            }

        }

        public void ConsolidateStatistics()
        {

            int GoBackDays = 3;
           
            int n = 0;
            for (n = GoBackDays; n >= 1; n += -1)
            {
                WriteAuditEntry("\r\n" + "\r\n" + "*************************************  ---> Processing " + DateTime.Today.AddDays(-n).ToString());
              
                ProcessSpecificDate(DateTime.Today.AddDays(-n), "DATEADD(dd,-" + n.ToString() + ",GETDATE())");
            }

            try
            {
                
                CleanUpTravelerSummaryData();

            }
            catch (Exception ex)
            {
            }


            try
            {
               
                WriteAuditEntry(DateTime.Now.ToString() + " Consolidating Traveler Stats");
                ProcessStoredProcedures("OpenTimesDelta");
                ProcessStoredProcedures("CumulativeTimesMin");
                ProcessStoredProcedures("CumulativeTimesMax");
            }
            catch (Exception ex)
            {
                WriteAuditEntry("  Error calling stored procedure: " + ex.ToString());
            }


            WriteAuditEntry("\r\n" + "\r\n" + "***********************************************" + "\r\n" + "Finished!");


        }


        public void ProcessSpecificDate(System.DateTime SearchDate, string SearchDateSQL = "")
        {
            WriteAuditEntry(DateTime.Now.ToString() + " VitalSigns Daily Tasks service is consolidating statistics for " + SearchDate, LogLevel.Normal);

            VSAdaptor objVSAdaptor = new VSAdaptor();
           
            string strSQL = "";
            System.Data.DataSet myDataSet = new System.Data.DataSet();
            System.Data.DataTable myTable = new System.Data.DataTable();
            myTable.TableName = "DailyTasks";
            myDataSet.Tables.Add(myTable);


            string AlreadyProcessed = "";
           
            try
            {
                strSQL = "Select Result FROM ConsolidationResults WHERE CONVERT (DATE, ScanDate) = '" + FixDate(SearchDate) + "' ";
                WriteAuditEntry(DateTime.Now.ToString() + " --> " + strSQL);
                AlreadyProcessed =Convert.ToString( objVSAdaptor.ExecuteScalarAny("VSS_Statistics", "Stats", strSQL));
            }
            catch (Exception ex)
            {
                AlreadyProcessed = "False";
            }


            try
            {
                if (AlreadyProcessed == "Success")
                {
                    WriteAuditEntry(DateTime.Now.ToString() + " " + FixDate(SearchDate) + " has already been processed", LogLevel.Normal);
                    return;
                }
                else
                {
                    WriteAuditEntry(DateTime.Now.ToString() + " " + FixDate(SearchDate) + " has NOT already been processed", LogLevel.Normal);
                    objVSAdaptor.ExecuteNonQueryAny("VSS_Statistics", "Stats", "Insert INTO ConsolidationResults (ScanDate, Result) VALUES ('" + FixDate(SearchDate) + "', 'Success')");
                }

            }
            catch (Exception ex)
            {
            }

            try
            {
                ConsolidateDominoDiskStats(SearchDate);

            }
            catch (Exception ex)
            {
            }

            try
            {
                ConsolidateServerDiskStats(SearchDate);

            }
            catch (Exception ex)
            {
            }

            strSQL = "SELECT SourceTableName, SourceAggregation, SourceStatName, DestinationTableName, DestinationStatName, QueryType FROM DailyTasks";
            strSQL = strSQL + " Order By SourceStatName  DESC";

            WriteAuditEntry("\r\n" + strSQL + "\r\n", LogLevel.Verbose);


            try
            {
                objVSAdaptor.FillDatasetAny("VitalSigns", "vitalsigns", strSQL, ref myDataSet, "DailyTasks");
            }
            catch (Exception ex)
            {
                WriteAuditEntry("Exception = " + ex.ToString());
                WriteAuditEntry(DateTime.Now.ToString() + " Error Accessing the DailyTasks Table " + ex.Message + strSQL);
            }

            try
            {
                WriteAuditEntry("I found " + myDataSet.Tables["DailyTasks"].Rows.Count + " statistics to process.", LogLevel.Verbose);

            }
            catch (Exception ex)
            {
            }


            System.Data.DataView myView = new System.Data.DataView(myDataSet.Tables["DailyTasks"]);
            DataRowView drv = null;

            string strSQLSelect = "";
            string srcTable = null;
            string operation = null;
            string srcStat = null;
            string destTable = null;
            string destStat = null;
            string QueryType = null;
            // Dim value As Single = 0
            int rowCounter = 1;


            foreach (DataRowView drv_loopVariable in myView)
            {
                drv = drv_loopVariable;

                try
                {
                    System.Data.DataTable SummaryData = new System.Data.DataTable();

                    myDataSet.Tables.Add(SummaryData);
                    srcTable = Convert.ToString(drv["SourceTableName"]);
                    operation = Convert.ToString(drv["SourceAggregation"]);
                    srcStat = Convert.ToString(drv["SourceStatName"]);
                    destTable = Convert.ToString(drv["DestinationTableName"]);
                    destStat = Convert.ToString(drv["DestinationStatName"]);
                    QueryType = Convert.ToString(drv["QueryType"]);
                    try
                    {
                        WriteAuditEntry("\r\n" + DateTime.Now.ToString() + " Processing stat #" + rowCounter + ": " + srcStat, LogLevel.Verbose);
                        rowCounter += 1;
                    }
                    catch (Exception ex)
                    {
                    }

                    //Followed the SQL way of providing the date range
                    if ((!string.IsNullOrEmpty(srcTable) & !string.IsNullOrEmpty(operation) & !string.IsNullOrEmpty(srcStat) & !string.IsNullOrEmpty(destTable) & !string.IsNullOrEmpty(destStat) & QueryType == "1"))
                    {
                        RunQueryType1(SearchDate, srcStat, srcTable, operation, destTable, destStat, QueryType);
                    }
                    else if ((!string.IsNullOrEmpty(srcTable) & !string.IsNullOrEmpty(operation) & !string.IsNullOrEmpty(srcStat) & !string.IsNullOrEmpty(destTable) & !string.IsNullOrEmpty(destStat) & QueryType == "2"))
                    {
                        RunQueryType2(SearchDate, srcStat, srcTable, operation, destTable, destStat, QueryType);
                    }
                    else if ((!string.IsNullOrEmpty(srcTable) & !string.IsNullOrEmpty(operation) & !string.IsNullOrEmpty(srcStat) & !string.IsNullOrEmpty(destTable) & !string.IsNullOrEmpty(destStat) & QueryType == "3"))
                    {
                        RunQueryType3(SearchDate, srcStat, srcTable, operation, destTable, destStat, QueryType);
                    }
                }
                catch (Exception ex)
                {
                    WriteAuditEntry("Advancing to next row...", LogUtils.LogLevel.Verbose);

                }

            }

            try
            {
                //11/20/2015 NS modified for VSPLUS-2383
                ConsolidateExchangeDatabases(SearchDate, SearchDateSQL);

            }
            catch (Exception ex)
            {
            }

            try
            {
                //12/23/2015 WS modified for VSPLUS-1423
                ConsolidateExchangeMailboxData(SearchDate, SearchDateSQL);

            }
            catch (Exception ex)
            {
            }

            WriteAuditEntry("\r\n" + "\r\n" + "***********************************************" + "\r\n" + "Finished!");


        }



        public void RunQueryType3(System.DateTime SearchDate, string srcStat, string srcTable, string operation, string destTable, string destStat, string QueryType)
        {
            DataRowView drv = null;
            System.Data.DataSet myDataSet = new System.Data.DataSet();
            System.Data.DataTable myTable = new System.Data.DataTable();
            myTable.TableName = "DailyTasks";
            myDataSet.Tables.Add(myTable);
            List<DailyStatistics> dialyStataStics = new List<DailyStatistics>();

            string sql = "";
          
            int rowCounter = 1;
           
            SqlCommand sqlcmd = new SqlCommand();
            IFormatProvider USprovider = CultureInfo.CreateSpecificCulture(culture);
            IFormatProvider Europrovider = CultureInfo.CreateSpecificCulture("fr-FR");
            WriteAuditEntry(DateTime.Now.ToString() + " USprovider culture is " + culture, LogUtils.LogLevel.Verbose);

            try
            {

                sql = "SELECT DeviceType, ServerName, WeekNumber, MonthNumber, YearNumber, DayNumber," + operation + "(StatValue) AS value FROM " + "\r\n";
                sql = sql + "" + srcTable + " WHERE StatName = '" + srcStat + "' AND date >= DATEADD(day,DATEDIFF(day,0," + objVsAdaptor.DateFormat(FixDate(SearchDate)) + "),0)" + " ";
                sql = sql + "AND date < DATEADD(day,DATEDIFF(day,0," + objVsAdaptor.DateFormat(FixDate(SearchDate.AddDays(1))) + "),0) GROUP BY DeviceType, ServerName, WeekNumber, MonthNumber, YearNumber, DayNumber" + " ";
                
                WriteAuditEntry(sql, LogUtils.LogLevel.Verbose);


                try
                {
                    objVsAdaptor.FillDatasetAny("VSS_Statistics", "statistics", sql,ref  myDataSet, "SummaryData");
                }
                catch (Exception ex)
                {
                    WriteAuditEntry(DateTime.Now.ToString() + " Error Accessing the " + srcTable + " Table. " + ex.Message + sql);
                }

                System.Data.DataView SummaryView = new System.Data.DataView(myDataSet.Tables["SummaryData"]);
                DataRowView summaryDrv = null;

                string strSQLInsert = "";
                string serverName = "";
                string deviceName = "";
                string deviceType = "";
                int weekNumber = 0;
                int dayNumber = 0;
                int monthNumber = 0;
                int yearNumber = 0;
                float summaryValue = 0;
                string queryToLog = string.Empty;

                try
                {
                    if (SummaryView.Count > 0)
                    {
                        foreach (DataRowView summaryDrv_loopVariable in SummaryView)
                        {
                            summaryDrv = summaryDrv_loopVariable;
                            DailyStatistics dailyStats = new DailyStatistics();
                            try
                            {
                                dailyStats.DeviceType = Convert.ToString(summaryDrv["deviceType"]);
                                dailyStats.ServerName = Convert.ToString(summaryDrv["ServerName"]);
                                dailyStats.StatName = destStat;
                                dailyStats.StatValue = Convert.ToDouble(summaryDrv["value"]);

                            }
                            catch (Exception ex)
                            {
                                WriteAuditEntry(DateTime.Now.ToString() + " Error getting the values " + ex.Message);
                            }

                            if ((QueryType == "3"))
                            {
                               
                                try
                                {
                                    dialyStataStics.Add(dailyStats);


                                    WriteAuditEntry(DateTime.Now.ToString() + " SQL INSERT statement is " + queryToLog, LogUtils.LogLevel.Verbose);
                                }
                                catch (Exception ex)
                                {
                                    WriteAuditEntry(DateTime.Now.ToString() + " Exception creating INSERT statement: " + ex.ToString());
                                }
                            }
                           
                        }
                        if (dialyStataStics.Count > 0)
                            dailyStatasticsRepository.Insert(dialyStataStics);
                    }
                    else
                    {
                        WriteAuditEntry(DateTime.Now.ToString() + " This query did not produce any rows.", LogUtils.LogLevel.Verbose);
                    }
                }
                catch (Exception ex)
                {
                    WriteAuditEntry(DateTime.Now.ToString() + " Exception where I least expected it: " + ex.ToString());
                }

                try
                {
                    
                    myDataSet.Tables["SummaryData"].Clear();
                }
                catch (Exception ex)
                {
                    WriteAuditEntry("Could not Remove the table as no data in it.", LogUtils.LogLevel.Verbose);
                }

                WriteAuditEntry("Skipping this row as not all the required parameters were provided.", LogUtils.LogLevel.Verbose);
            }
            catch (Exception ex)
            {
                WriteAuditEntry(DateTime.Now.ToString() + " Exception creating SQL in DailyTasks: " + ex.ToString());
            }


        }


        public void RunQueryType1(System.DateTime SearchDate, string srcStat, string srcTable, string operation, string destTable, string destStat, string QueryType)
        {
            DataRowView drv = null;
            System.Data.DataSet myDataSet = new System.Data.DataSet();
            System.Data.DataTable myTable = new System.Data.DataTable();
            myTable.TableName = "DailyTasks";
            myDataSet.Tables.Add(myTable);
            List<DailyStatistics> dialyStataStics = new List<DailyStatistics>();
            string strSQLSelect = "";
          
            int rowCounter = 1;
          
            SqlCommand sqlcmd = new SqlCommand();
            IFormatProvider USprovider = CultureInfo.CreateSpecificCulture(culture);
            IFormatProvider Europrovider = CultureInfo.CreateSpecificCulture("fr-FR");
            WriteAuditEntry(DateTime.Now.ToString() + " USprovider sCultureString is " + culture, LogUtils.LogLevel.Verbose);

            try
            {
             
                if (srcTable == "MicrosoftDailyStats")
                {
                    strSQLSelect = "SELECT ServerName, ServerTypeId, WeekNumber, MonthNumber, YearNumber, DayNumber," + operation + "(StatValue) AS value FROM " + " ";
                    strSQLSelect = strSQLSelect + "" + srcTable + " WHERE StatName = '" + srcStat + "' AND date >= DATEADD(day,DATEDIFF(day,0," + objVsAdaptor.DateFormat(FixDate(SearchDate)) + "),0)" + " ";
                    strSQLSelect = strSQLSelect + "AND date < DATEADD(day,DATEDIFF(day,0," + objVsAdaptor.DateFormat(FixDate(SearchDate.AddDays(1))) + "),0) GROUP BY ServerName, ServerTypeId, WeekNumber, MonthNumber, YearNumber, DayNumber" + " ";

                }
                else
                {
                    strSQLSelect = "SELECT ServerName, WeekNumber, MonthNumber, YearNumber, DayNumber," + operation + "(StatValue) AS value FROM " + "\r\n";
                    strSQLSelect = strSQLSelect + "" + srcTable + " WHERE StatName = '" + srcStat + "' AND date >= DATEADD(day,DATEDIFF(day,0," + objVsAdaptor.DateFormat(FixDate(SearchDate)) + "),0)" + " ";
                    strSQLSelect = strSQLSelect + "AND date < DATEADD(day,DATEDIFF(day,0," + objVsAdaptor.DateFormat(FixDate(SearchDate.AddDays(1))) + "),0) GROUP BY ServerName, WeekNumber, MonthNumber, YearNumber, DayNumber" + " ";
                  
                }
                WriteAuditEntry(strSQLSelect, LogUtils.LogLevel.Verbose);

                try
                {
                    objVsAdaptor.FillDatasetAny("VSS_Statistics", "statistics", strSQLSelect, ref myDataSet, "SummaryData");
                }
                catch (Exception ex)
                {
                    WriteAuditEntry(DateTime.Now.ToString() + " Error Accessing the " + srcTable + " Table. " + ex.Message + strSQLSelect);
                }

                System.Data.DataView SummaryView = new System.Data.DataView(myDataSet.Tables["SummaryData"]);
                DataRowView consummaryDrv = null;

                string strSQLInsert = "";
                string serverName = "";
                string deviceName = "";
                int weekNumber = 0;
                int dayNumber = 0;
                int monthNumber = 0;
                int yearNumber = 0;
                double summaryValue = 0;
                string serverTypeId = "";
                string queryToLog = string.Empty;

                try
                {
                    if (SummaryView.Count > 0)
                    {
                        foreach (DataRowView summaryDrv in SummaryView)
                        {
                            DailyStatistics dailyStats = new DailyStatistics();
                          
                             try
                            {
                                dailyStats.DeviceType = Convert.ToString(summaryDrv["ServerTypeId"]);
                                dailyStats.ServerName = Convert.ToString(summaryDrv["ServerName"]);
                                dailyStats.StatName = destStat;
                                dailyStats.StatValue = Convert.ToDouble(summaryDrv["value"]);

                            }
                            catch (Exception ex)
                            {
                                WriteAuditEntry(DateTime.Now.ToString() + " Error getting the values " + ex.Message);
                            }

                            if ((QueryType == "1"))
                            {
                              

                            

                                if (srcTable == "MicrosoftDailyStats")
                                {
                                    dialyStataStics.Add(dailyStats);
                                }
                                else
                                {
                                    dialyStataStics.Add(dailyStats);
                                }
                                WriteAuditEntry(DateTime.Now.ToString() + " SQL command statement is " + queryToLog, LogUtils.LogLevel.Verbose);
                            }
                          
                        }
                        if (dialyStataStics.Count > 0)
                            dailyStatasticsRepository.Insert(dialyStataStics);
                    }
                    else
                    {
                        WriteAuditEntry(DateTime.Now.ToString() + " This query did not produce any rows.", LogUtils.LogLevel.Verbose);
                    }
                }
                catch (Exception ex)
                {
                    WriteAuditEntry(DateTime.Now.ToString() + " Exception where I least expected it: " + ex.ToString());
                }

                try
                {
                 
                    myDataSet.Tables["SummaryData"].Clear();
                }
                catch (Exception ex)
                {
                    WriteAuditEntry("Could not Remove the table as no data in it.", LogUtils.LogLevel.Verbose);
                }

                WriteAuditEntry("Skipping this row as not all the required parameters were provided.", LogUtils.LogLevel.Verbose);
            }
            catch (Exception ex)
            {
                WriteAuditEntry(DateTime.Now.ToString() + " Exception creating SQL in DailyTasks: " + ex.ToString());
            }


        }

        public void RunQueryType2(System.DateTime SearchDate, string srcStat, string srcTable, string operation, string destTable, string destStat, string QueryType)
        {
          
            System.Data.DataSet myDataSet = new System.Data.DataSet();
            System.Data.DataTable myTable = new System.Data.DataTable();
            myTable.TableName = "DailyTasks";
            myDataSet.Tables.Add(myTable);
            List<DailyStatistics> dialyStataStics = new List<DailyStatistics>();
            string strSQLSelect = "";
          
            //11/19/2015 NS added for VSPLUS-2383
            SqlCommand sqlcmd = new SqlCommand();
          
            IFormatProvider USprovider = CultureInfo.CreateSpecificCulture(culture);
            IFormatProvider Europrovider = CultureInfo.CreateSpecificCulture("fr-FR");
            WriteAuditEntry(DateTime.Now.ToString() + " USprovider sCultureString is " + culture, LogUtils.LogLevel.Verbose);
            try
            {
               
                strSQLSelect = "SELECT DeviceType, DeviceName, WeekNumber, MonthNumber, YearNumber, DayNumber," + operation + "(StatValue) AS value FROM " + "\r\n";
                strSQLSelect = strSQLSelect + "" + srcTable + " WHERE StatName = '" + srcStat + "' AND date >= DATEADD(day,DATEDIFF(day,0," + objVsAdaptor.DateFormat(FixDate(SearchDate)) + "),0)" + "\r\n";
                strSQLSelect = strSQLSelect + "AND date < DATEADD(day,DATEDIFF(day,0," + objVsAdaptor.DateFormat(FixDate(SearchDate.AddDays(1))) + "),0) GROUP BY DeviceType, DeviceName, WeekNumber, MonthNumber, YearNumber, DayNumber" + "\r\n";
               
                WriteAuditEntry(strSQLSelect, LogUtils.LogLevel.Verbose);

                try
                {
                    objVsAdaptor.FillDatasetAny("VSS_Statistics", "statistics", strSQLSelect,ref myDataSet, "SummaryData");
                }
                catch (Exception ex)
                {
                    WriteAuditEntry(DateTime.Now.ToString() + " Error Accessing the " + srcTable + " Table. " + ex.Message + strSQLSelect);
                }

                System.Data.DataView SummaryView = new System.Data.DataView(myDataSet.Tables["SummaryData"]);
                DataRowView summaryDrv = null;

                string strSQLInsert = "";
                
                string deviceName = "";
                string deviceType = "";
                int weekNumber = 0;
                int dayNumber = 0;
                int monthNumber = 0;
                int yearNumber = 0;
                float summaryValue = 0;
                string queryToLog = string.Empty;

                try
                {
                    if (SummaryView.Count > 0)
                    {
                        foreach (DataRowView summaryDrv_loopVariable in SummaryView)
                        {
                            summaryDrv = summaryDrv_loopVariable;
                            DailyStatistics dailyStats = new DailyStatistics();
                           
                             try
                            {
                                dailyStats.DeviceType = Convert.ToString(summaryDrv["deviceType"]);
                                dailyStats.ServerName = Convert.ToString(summaryDrv["ServerName"]);
                                dailyStats.StatName = destStat;
                                dailyStats.StatValue = Convert.ToDouble(summaryDrv["value"]);

                            }

                            catch (Exception ex)
                            {
                                WriteAuditEntry(DateTime.Now.ToString() + " Error getting the values " + ex.Message);
                            }

                            if ((QueryType == "2"))
                            {
                                try
                                {

                                    dialyStataStics.Add(dailyStats);

                                    WriteAuditEntry(DateTime.Now.ToString() + " SQL INSERT statement is " + queryToLog, LogUtils.LogLevel.Verbose);
                                }
                                catch (Exception ex)
                                {
                                    WriteAuditEntry(DateTime.Now.ToString() + " Exception creating INSERT statement: " + ex.ToString());
                                }
                            }

                           
                        }
                        if (dialyStataStics.Count > 0)
                            dailyStatasticsRepository.Insert(dialyStataStics);
                    }
                    else
                    {
                        WriteAuditEntry(DateTime.Now.ToString() + " This query did not produce any rows.", LogUtils.LogLevel.Verbose);
                    }
                }
                catch (Exception ex)
                {
                    WriteAuditEntry(DateTime.Now.ToString() + " Exception where I least expected it: " + ex.ToString());
                }

                try
                {
                   
                    myDataSet.Tables["SummaryData"].Clear();
                }
                catch (Exception ex)
                {
                    WriteAuditEntry("Could not Remove the table as no data in it.", LogUtils.LogLevel.Verbose);
                }

                WriteAuditEntry("Skipping this row as not all the required parameters were provided.",LogUtils.LogLevel.Verbose);
            }
            catch (Exception ex)
            {
                WriteAuditEntry(DateTime.Now.ToString() + " Exception creating SQL in DailyTasks: " + ex.ToString());
            }


        }

        public class Root
        {
            public OS[] OS { get; set; }
            public Device[] Device { get; set; }
            public Location[] Location { get; set; }
        }
        public class Location
        {
            public string Country { get; set; }
            public string State { get; set; }
        }

        public class OS
        {
            public string OSType { get; set; }
            public string TranslatedValue { get; set; }
            public string OSName { get; set; }
        }
        public class Device
        {
            public string DeviceType { get; set; }
            public string TranslatedValue { get; set; }
            public string OSName { get; set; }
        }
          //Kiran Dadireddy VSPLUS-2684
        private void ShrinkLog(string connection)
        {

            try
            {
                VSFramework.XMLOperation myAdapter = new VSFramework.XMLOperation();
                using (System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(myAdapter.GetDBConnectionString(connection)))
                {
                    try
                    {
                        con.Open();
                        SqlCommand command = new SqlCommand("DBCC SHRINKFILE(" + connection + "_Log,10)", con);
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                    }
                    finally
                    {
                        if (con.State == ConnectionState.Open)
                        {
                            con.Close();
                        }
                    }
                }

                WriteAuditEntry(DateTime.Now.ToString() + " Completed shrinking " + connection + " Log ");

            }
            catch (Exception ex)
            {
                WriteAuditEntry(DateTime.Now.ToString() + " Exception while Shrinking " + connection + " Log . \\n Exception :" + ex.Message);
            }
        }
         //Kiran Dadireddy VSPLUS-2684
        private void ShrinkDBLogOnWeeklyBasis()
        {
            WriteAuditEntry(DateTime.Now.ToString() + " Starting Shrinking VitalSigns Log ");
            ShrinkLog("VitalSigns");

            WriteAuditEntry(DateTime.Now.ToString() + " Starting Shrinking VSS_Statistics Log ");
            ShrinkLog("VSS_Statistics");
        }

        private void ConsolidateExchangeMailboxData(DateTime curDate , String searchDateStr = "")
        {
            SqlCommand sqlcmd = new SqlCommand();
            DataSet myDataSet = new DataSet();
            string sql;
            List<SummaryStatistics> summaryStataStics = new List<SummaryStatistics>();
          
            sql = "SELECT ServerName, StatName, SUM(StatValue) StatValue FROM MicrosoftDailyStats where StatName like 'Mailbox.%.%.%' " +
        "and DateAdd(day, DateDiff(day, 0, Date),0) = DateAdd(day, DateDiff(day, 0, " + searchDateStr + "),0) " +
        "GROUP BY StatName, ServerName";
            WriteAuditEntry("\r\n" + sql + "\r\n");

            try
            {

                objVsAdaptor.FillDatasetAny("VSS_Statistics", "vitalsigns", sql, ref myDataSet, "MyTable");
                
            }
            catch (Exception ex)
            {

                WriteAuditEntry(DateTime.Now.ToString() + " Error creating Exchange Mail Files collection " + ex.Message + "-- The failed command was " + sql);
            }

            System.Data.DataView myView = new System.Data.DataView(myDataSet.Tables["MyTable"]);
            DataRowView drv = null;

            string sqlInsert = "";
            string serverName = null;
            string statName = null;
            string statValue = null;
            int myWeekNumber = 0;
            string queryToLog = string.Empty;
            foreach (DataRowView drv_loopVariable in myView)
            {
                drv = drv_loopVariable;
                try
                {
                    SummaryStatistics summaryStats = new SummaryStatistics();
                   
                  // summaryStats.DeviceId = Convert.ToString(drv["ServerName"]);
                    summaryStats.StatName = Convert.ToString(drv["StatName"]);
                    summaryStats.StatValue = Convert.ToDouble(drv["StatValue"]);

                   summaryStataStics .Add(summaryStats);

                  

                }
                catch (Exception ex)
                {
                    WriteAuditEntry(DateTime.Now.ToString() + " Exception creating Mongo: " + ex.ToString());
                }

               
            }
            if (summaryStataStics.Count > 0)
                summaryStatasticsRepository.Insert(summaryStataStics);


        }

        private void ConsolidateExchangeDatabases(DateTime curDate, String searchDateStr )
        {
            //11/20/2015 NS added for VSPLUS-2383
            SqlCommand sqlcmd = new SqlCommand();
            DataSet myDataSet = new DataSet();
            string sql;
            List<SummaryStatistics> summaryStataStics = new List<SummaryStatistics>();
            sql = "SELECT ServerName, StatName, AVG(StatValue) StatValue FROM MicrosoftDailyStats where StatName like 'ExDatabaseSizeMb.%' " +
       "and DateAdd(day, DateDiff(day, 0, Date),0) = DateAdd(day, DateDiff(day, 0, " + searchDateStr + "),0) " +
       "GROUP BY StatName, ServerName";
            WriteAuditEntry("\r\n" + sql + "\r\n");
            try
            {
                
                objVsAdaptor.FillDatasetAny("VSS_Statistics", "vitalsigns", sql, ref myDataSet, "MyTable");
             
            }
            catch (Exception ex)
            {
                WriteAuditEntry(DateTime.Now.ToString() + " Error creating Exchange DB collection " + ex.Message + "-- The failed command was " + sql);
            }
            System.Data.DataView myView = new System.Data.DataView(myDataSet.Tables["MyTable"]);
            

            string sqlInsert = "";
            string serverName=string.Empty;
            string statName;
            string statValue;
            int myWeekNumber = 0;
            string queryToLog = string.Empty;
          //  DataRowView drv;
            foreach (DataRowView drv in myView)
            {
              
                try
                {
                    SummaryStatistics summaryStats = new SummaryStatistics();
                    summaryStats.StatName  =drv["StatName"].ToString();
                    summaryStats.StatValue  =Convert.ToDouble(drv["StatValue"].ToString());
                    //summaryStats.DeviceId=


                    summaryStataStics.Add(summaryStats);


                   

                }
                catch (Exception ex)
                {
                    WriteAuditEntry(DateTime.Now.ToString() + " Exception creating Mongo: " + ex.ToString());
                }

               
            }

            if (summaryStataStics.Count > 0)
                summaryStatasticsRepository.Insert(summaryStataStics);


        }
        public void UpdateLocalTables()
        {
           
            string timeToUpdate = "true1";
            string sql = "select case when DATEADD(day,7,convert(datetime, Svalue, 120)) < getdate() then 'true' else 'false' end as UpdateTables from Settings where Sname = 'LastTableUpdate'";

            DataSet ds = new DataSet();
            try
            {
                objVsAdaptor.FillDatasetAny("VitalSigns", "VitalSigns", sql, ref ds, "Settings");
                timeToUpdate = ds.Tables["Settings"].Rows[0][0].ToString();
                WriteAuditEntry(DateTime.Now.ToString() + " " + timeToUpdate);
            }
            catch (Exception ex)
            {
                timeToUpdate = "true";
            }

            if ((timeToUpdate.ToLower() == "false"))
            {
                WriteAuditEntry(DateTime.Now.ToString() + " It is not time to update the local tables.");
                return;
            }

            WriteAuditEntry(DateTime.Now.ToString() + " Updating local tables with new values from RPR's servers");

            System.Net.WebClient Web = new System.Net.WebClient();
            string response = "";
            string url = null;
            int counter = 0;


            url = "http://jnitinc.com/WebService/UpdateTables.php?newTable=true";
            response = Web.DownloadString(url);

            Root root = new Root();
            MemoryStream ms = new MemoryStream(Encoding.Unicode.GetBytes(response));
            System.Runtime.Serialization.Json.DataContractJsonSerializer serializer = new System.Runtime.Serialization.Json.DataContractJsonSerializer(root.GetType());
            root = (Root)serializer.ReadObject(ms);
            ms.Close();
            ms.Dispose();

           

            if ((root.Location.Length > 0))
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("Country");
                dt.Columns.Add("State");
                counter = 0;
                foreach (Location item in root.Location)
                {
                    DataRow row = dt.NewRow();
                    row["Country"] = item.Country.ToString();
                    row["State"] = item.State.ToString();
                    dt.Rows.Add(row);
                }

               
                try
                {
                    objVsAdaptor.ExecuteNonQueryAny("VitalSigns", "", "DELETE FROM ValidLocations");
                    SqlConnection con = objVsAdaptor.StartConnectionSQL("VitalSigns");

                    SqlBulkCopy blk = new SqlBulkCopy(con);
                    blk.DestinationTableName = "ValidLocations";
                    blk.WriteToServer(dt);

                    blk.Close();
                    objVsAdaptor.StopConnectionSQL(con);


                    
                }
                catch (Exception ex)
                {
                    WriteAuditEntry(DateTime.Now.ToString() + " Error executing SQL command " + ex.ToString());
                }

            }


            if ((root.Device.Length > 0))
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("DeviceType");
                dt.Columns.Add("TranslatedValue");
                dt.Columns.Add("OSName");
                foreach (Device item in root.Device)
                {
                    DataRow row = dt.NewRow();
                    row["DeviceType"] = item.DeviceType.ToString();
                    row["TranslatedValue"] = item.TranslatedValue.ToString();
                    row["OSName"] = item.OSName.ToString();
                    dt.Rows.Add(row);
                }

                
                try
                {
                    objVsAdaptor.ExecuteNonQueryAny("VitalSigns", "", "DELETE FROM DeviceTypeTranslation");

                    SqlConnection con = objVsAdaptor.StartConnectionSQL("VitalSigns");

                    SqlBulkCopy blk = new SqlBulkCopy(con);
                    blk.DestinationTableName = "DeviceTypeTranslation";
                    blk.WriteToServer(dt);

                    blk.Close();
                    objVsAdaptor.StopConnectionSQL(con);

                }
                catch (Exception ex)
                {
                    WriteAuditEntry(DateTime.Now.ToString() + " Error executing SQL command " + ex.ToString());
                }

            }



            if ((root.OS.Length > 0))
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("OSType");
                dt.Columns.Add("TranslatedValue");
                dt.Columns.Add("OSName");
                foreach (OS item in root.OS)
                {
                    DataRow row = dt.NewRow();
                    row["OSType"] = item.OSType.ToString();
                    row["TranslatedValue"] = item.TranslatedValue.ToString();
                    row["OSName"] = item.OSName.ToString();
                    dt.Rows.Add(row);
                }
               
                try
                {
                    objVsAdaptor.ExecuteNonQueryAny("VitalSigns", "", "DELETE FROM OSTypeTranslation");
                    SqlConnection con = objVsAdaptor.StartConnectionSQL("VitalSigns");

                    SqlBulkCopy blk = new SqlBulkCopy(con);
                    blk.DestinationTableName = "OSTypeTranslation";
                    blk.WriteToServer(dt);

                    blk.Close();
                    objVsAdaptor.StopConnectionSQL(con);
                }
                catch (Exception ex)
                {
                    WriteAuditEntry(DateTime.Now.ToString() + " Error executing SQL command " + ex.ToString());
                }
            }



            sql = "IF NOT EXISTS(SELECT * FROM Settings WHERE Sname = 'LastTableUpdate') ";
            sql += " INSERT INTO Settings (SName, SValue, SType) VALUES ('LastTableUpdate', getDate(),'System.String') ";
            sql += " ELSE UPDATE Settings SET SValue=getDate() WHERE SName='LastTableUpdate'";
            try
            {
                objVsAdaptor.ExecuteNonQueryAny("VitalSigns", "", sql);
            }
            catch (Exception ex)
            {
                WriteAuditEntry(DateTime.Now.ToString() + " Error executing SQL command " + ex.ToString());
            }


        }

        private int GetWeekNumber(DateTime dt)
        {
            int year = dt.Year;
            DateTime Dec29 = new DateTime(year, 12, 29);
            DateTime week1 = new DateTime();
          
            if ((dt >= new DateTime(year, 12, 29)))
            {
                week1 = GetWeekOneDate(year + 1);
                if ((dt < week1))
                    week1 = GetWeekOneDate(year);
            }
            else
            {
                week1 = GetWeekOneDate(year);
                if ((dt < week1))
                    week1 = GetWeekOneDate(-year);
            }

            return ((dt.Subtract(week1).Days / 7 + 1));
        }

        private DateTime GetWeekOneDate(int Year)
        {
            
            DateTime MyDate = new DateTime(Year, 1, 4);

            int DayNum =Convert.ToInt32(MyDate.DayOfWeek);


            if (DayNum == 0)
            {
                DayNum = 7;
            }

           
            return MyDate.AddDays(1 - DayNum);

        }

        public void ConsolidateServerDiskStats(System.DateTime SearchDate)
        {
            //11/20/2015 NS added for VSPLUS-2383
            SqlCommand sqlcmd = new SqlCommand();
            string sql = "";

            System.Data.DataSet myDataSet = new System.Data.DataSet();
            System.Data.DataTable myTable = new System.Data.DataTable();
            myDataSet.Tables.Add(myTable);
            List<SummaryStatistics> summaryStataStics = new List<SummaryStatistics>();
            sql = "Select ServerName, DiskName, DiskFree, ServerTypes.ID FROM [vitalsigns].[dbo].[DiskSpace] inner join [vitalsigns].[dbo].[ServerTypes] on ServerTypes.ServerType=DiskSpace.ServerType";

            WriteAuditEntry("\r\n" + sql + "\r\n");

            try
            {
              
                objVsAdaptor.FillDatasetAny("VSS_Statistics", "vitalsigns", sql, ref myDataSet, "MyTable");
                
            }
            catch (Exception ex)
            {
                WriteAuditEntry(DateTime.Now.ToString() + " Error creating Daily Disk Space Summary " + ex.Message + "-- The failed command was " + sql);
            }

            System.Data.DataView myView = new System.Data.DataView(myDataSet.Tables["MyTable"]);
            DataRowView drv = null;

            string strSQLInsert = "";
            string serverName = null;
            string myNumberString = null;
            string driveName = null;
            int myWeekNumber = 0;
            double myNumber = 0;
            string queryToLog = string.Empty;

            foreach (DataRowView drv_loopVariable in myView)
            {
                drv = drv_loopVariable;

                try
                {
                    SummaryStatistics summaryStats = new SummaryStatistics();
                    summaryStats.StatName = drv["StatName"].ToString();
                    summaryStats.StatValue =Convert.ToDouble(myNumberString);
                    //summaryStats.DeviceId=

                    summaryStataStics.Add(summaryStats);


                

                }
                catch (Exception ex)
                {
                    WriteAuditEntry(DateTime.Now.ToString() + " Exception creating Mongo: " + ex.ToString());
                }

               
            }
            if (summaryStataStics.Count > 0)
                summaryStatasticsRepository.Insert(summaryStataStics);

            WriteAuditEntry(DateTime.Now.ToString() + " Finished processing drives for all Microsoft servers.");
            Thread.Sleep(250);
        }

        public void DeleteDominoDailyStats()
        {
            //Durga  VSPLUS 2281

            string sql = null;
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            VSAdaptor vsobj = new VSAdaptor();
            int cleanupMonth = 0;
            SqlConnection con = new SqlConnection();
            try
            {
                try
                {
                    sql = "select * from Settings where sname='CleanupMonth'";

                    dt.TableName = "Settings";
                    ds.Tables.Add(dt);
                    vsobj.FillDatasetAny("VitalSigns", "vitalsigns", sql, ref ds, "Settings");


                    if (dt.Rows.Count > 0)
                    {
                        cleanupMonth = Convert.ToInt32(dt.Rows[0]["svalue"].ToString());
                        WriteAuditEntry(DateTime.Now.ToString() + " Sucess in getting CleanupMonth from settings table. " + cleanupMonth);
                    }
                }
                catch (Exception ex)
                {
                    WriteAuditEntry(DateTime.Now.ToString() + " Error in getting CleanupMonth from settings table. " + ex.Message);
                }

                VSFramework.XMLOperation myAdapter = new VSFramework.XMLOperation();
                con.ConnectionString = myAdapter.GetDBConnectionString("VSS_Statistics");
                con.Open();
                SqlDataAdapter da = default(SqlDataAdapter);
                da = new SqlDataAdapter("DominoDailyCleanup", con);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.Parameters.Add(new SqlParameter("@month", SqlDbType.VarChar, 50));
                da.SelectCommand.Parameters["@month"].Value = cleanupMonth;

                da.SelectCommand.ExecuteNonQuery();
                WriteAuditEntry(DateTime.Now.ToString() + " Sucess in  Deleting Obsolete Daily Statistics Records more than 6 months old ");
            }
            catch (Exception ex)
            {
                WriteAuditEntry(DateTime.Now.ToString() + " Error Deleting Obsolete Daily Statistics Records more than 6 months old " + ex.Message);
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                    con.Dispose();
                }
            }

        }

        public void GenerateSummaryforAllDomino(DateTime StatDate, string SrcStatName, string operation)
        {
           
            //11/20/2015 NS added for VSPLUS-2383
            SqlCommand sqlcmd = new SqlCommand();
            string sql = "";

            DateTime EndDate = StatDate.AddDays(1);
            System.Data.DataSet myDataSet = new System.Data.DataSet();
            System.Data.DataTable myTable = new System.Data.DataTable();
            myDataSet.Tables.Add(myTable);

            List<SummaryStatistics> summaryStataStics = new List<SummaryStatistics>();

            sql = "SELECT ServerName ,  JustDate , " + operation + "(StatValue)  AS Average " + "\r\n";
            sql += "FROM (SELECT ServerName,  CONVERT(DATE, [Date]) AS JustDate , StatValue, StatName " + "\r\n";
            sql += "FROM DominoDailyStats WHERE StatName = '" + SrcStatName + "' " +"\r\n";
            sql += " AND Date > " + objVsAdaptor.DateFormat(FixDate(StatDate)) + " AND Date < " + objVsAdaptor.DateFormat(FixDate(EndDate));

            sql += ") as t1  " + "\r\n";
            sql += "GROUP BY ServerName , JustDate " + "\r\n";
            sql += " ORDER BY  ServerName, JustDate DESC" + "\r\n";

            WriteAuditEntry("\r\n" + sql + "\r\n");

            try
            {
                
                objVsAdaptor.FillDatasetAny("VSS_Statistics", "statistics", sql, ref myDataSet, "MyTable");
              
            }
            catch (Exception ex)
            {
                WriteAuditEntry(DateTime.Now.ToString() + " Error creating Daily Summary " + ex.Message + "-- The failed Average command was " + sql);
            }

            System.Data.DataView myView = new System.Data.DataView(myDataSet.Tables["MyTable"]);
            DataRowView drv = null;

            string sqlInsert = "";
            string ServerName = null;
            string myNumberString = null;
            int MyWeekNumber = 0;
            double myNumber = 0;
            string QueryToLog = null;
            foreach (DataRowView drv_loopVariable in myView)
            {
                drv = drv_loopVariable;

                try
                {
                    ServerName =Convert.ToString(drv["ServerName"]);
               
                    myNumber =Convert.ToDouble(drv["Average"]);
                    myNumberString = myNumber.ToString("F2");
                 
                    MyWeekNumber = GetWeekNumber(StatDate);

                    SummaryStatistics summaryStats = new SummaryStatistics();
                    summaryStats.StatName = drv["StatName"].ToString();
                    summaryStats.StatValue = Convert.ToDouble(myNumberString);
                    //summaryStats.DeviceId=

                    summaryStataStics.Add(summaryStats);


                  

                }
                catch (Exception ex)
                {
                    WriteAuditEntry(DateTime.Now.ToString() + " Exception creating Mongo: " + ex.ToString());
                }

                
            }

            if (summaryStataStics.Count > 0)
                summaryStatasticsRepository.Insert(summaryStataStics);

            sql = "DELETE FROM DominoDailyStats WHERE  Date<" + objVsAdaptor.DateFormat(FixDate(StatDate.AddDays(-2))) + " AND StatName='" + SrcStatName + "'";

            try
            {
                objVsAdaptor.ExecuteNonQueryAny("VSS_Statistics", "statistics", sql);
            }
            catch (Exception ex)
            {
                WriteAuditEntry(DateTime.Now.ToString() + " Error Deleting Obsolete Daily Statistics Records " + ex.Message);
            }

            WriteAuditEntry(DateTime.Now.ToString() + " Finished processing SummaryforAllDomino for Stat" + SrcStatName + " Date = " + StatDate);
            Thread.Sleep(250);
        }
        public void ConsolidateDominoDiskStats(System.DateTime SearchDate)
        {
            string myDiskFree = "";
            for (int n = 0; n <= dominoDiskNames.GetUpperBound(0); n++)
            {
                myDiskFree = dominoDiskNames[n] + ".Free";
                GenerateSummaryforAllDomino(SearchDate, myDiskFree, "AVG");
            }

        }

        private string FixDate(DateTime dt)
        {
           
            return objDateUtils.FixDate(dt, dateFormat);
        }
        private void CleanupAnyTableWeekly()
        {
            System.Data.DataSet myDataSet = new System.Data.DataSet();
            DataTable dt = null;
            string sql = "";
          
            string cleanup = "";
            string whereClause = "";
            VSFramework.XMLOperation myConnectionString = new VSFramework.XMLOperation();

            try
            {

                sql = "SELECT ID,DBName,TableName,ISNULL(ParameterType,'') ParameterType,ISNULL(Parameter,'') Parameter,ISNULL(Condition,'') Condition,ISNULL(Value,'') Value FROM DailyCleanup";
                dt = objVsAdaptor.FetchData(myConnectionString.GetDBConnectionString("VitalSigns"), sql);
                if ((dt.Rows.Count > 0))
                {
                    for (int n = 0; n <= dt.Rows.Count - 1; n++)
                    {
                        if (dt.Rows[n]["ParameterType"] == "DateTime")
                        {
                            whereClause = " WHERE " + dt.Rows[n]["Parameter"] + dt.Rows[n]["Condition"] + "CONVERT(DateTime, " + dt.Rows[n]["Value"] + ", 120)";
                        }
                        else if (dt.Rows[n]["ParameterType"] == "String")
                        {
                            whereClause = " WHERE " + dt.Rows[n]["Parameter"] + dt.Rows[n]["Condition"] + "'" + dt.Rows[n]["Value"] + "'";
                        }
                        else if (dt.Rows[n]["ParameterType"] == "Number")
                        {
                            whereClause = " WHERE " + dt.Rows[n]["Parameter"] + dt.Rows[n]["Condition"] + dt.Rows[n]["Value"];
                        }
                        else
                        {
                            whereClause = "";
                        }
                        cleanup = "DELETE FROM " + dt.Rows[n]["DBName"] + ".dbo." + dt.Rows[n]["TableName"] + whereClause;
                        WriteAuditEntry(DateTime.Now.ToString() + " Cleanup SQL query " + cleanup);
                        try
                        {
                            objVsAdaptor.ExecuteNonQueryAny(Convert.ToString(dt.Rows[n]["DBName"]), Convert.ToString(dt.Rows[n]["DBName"]), cleanup);
                        }
                        catch (Exception ex)
                        {
                            WriteAuditEntry(DateTime.Now.ToString() + " Error executing SQL command " + cleanup + " - " + ex.ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                WriteAuditEntry(DateTime.Now.ToString() + " Error executing SQL command " + sql + " - " + ex.ToString());
            }

            try
            {
                //DailyTasks table processing - allows for the stat table cleanup where the servers/devices have been decommissioned
                sql = "SELECT DISTINCT SourceTablename,DestinationTableName FROM DailyTasks";
                dt = objVsAdaptor.FetchData(myConnectionString.GetDBConnectionString("VitalSigns"), sql);
                if ((dt.Rows.Count > 0))
                {
                    for (int n = 0; n <= dt.Rows.Count - 1; n++)
                    {
                        cleanup = "DELETE FROM [VSS_Statistics].dbo." + dt.Rows[n]["SourceTablename"] + " " + "WHERE ServerName NOT IN( " + "SELECT DISTINCT t1.ServerName from [VSS_Statistics].dbo." + dt.Rows[n]["SourceTablename"] + " t1 " + "INNER JOIN dbo.DeviceInventory t2 ON t1.ServerName=t2.Name) ";
                        WriteAuditEntry(DateTime.Now.ToString() + " Cleanup SQL query " + cleanup);
                        try
                        {
                            objVsAdaptor.ExecuteNonQueryAny("vitalsigns", "vitalsigns", cleanup);
                        }
                        catch (Exception ex)
                        {
                            WriteAuditEntry(DateTime.Now.ToString() + " Error executing SQL command " + cleanup + " - " + ex.ToString());
                        }
                        cleanup = "DELETE FROM [VSS_Statistics].dbo." + dt.Rows[n]["DestinationTableName"] + " " + "WHERE ServerName NOT IN( " + "SELECT DISTINCT t1.ServerName from [VSS_Statistics].dbo." + dt.Rows[n]["DestinationTableName"] + " t1 " + "INNER JOIN dbo.DeviceInventory t2 ON t1.ServerName=t2.Name) ";
                        WriteAuditEntry(DateTime.Now.ToString() + " Cleanup SQL query " + cleanup);
                        try
                        {
                            objVsAdaptor.ExecuteNonQueryAny("vitalsigns", "vitalsigns", cleanup);
                        }
                        catch (Exception ex)
                        {
                            WriteAuditEntry(DateTime.Now.ToString() + " Error executing SQL command " + cleanup + " - " + ex.ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                WriteAuditEntry(DateTime.Now.ToString() + " Error executing SQL command " + sql + " - " + ex.ToString());
            }

            try
            {
                GC.Collect();

            }
            catch (Exception ex)
            {
            }

        }

        private void CleanUpTravelerSummaryData()
        {
            WriteAuditEntry(DateTime.Now.ToString() + " Cleaning up TravelerStats for today in case the query has been run today already. ");

            DateTime SearchDate = default(DateTime);
            SearchDate =Convert.ToDateTime(FixDate(DateTime.Today.AddDays(-30)));
            string sql = "";
            try
            {
                sql = "delete FROM [vitalsigns].[dbo].[TravelerStats] WHERE [DateUpdated] <= CAST(GETDATE() AS DATE)";

            }
            catch (Exception ex)
            {
            }

            try
            {
                objVsAdaptor.ExecuteNonQueryAny("vitalsigns", "servers", sql);

            }
            catch (Exception ex)
            {
                WriteAuditEntry(DateTime.Now.ToString() + " Error executing SQL command " + ex.ToString());
            }

            try
            {
                GC.Collect();
            }
            catch (Exception ex)
            {
                //  Thread.Sleep(500)
            }

            WriteAuditEntry(DateTime.Now.ToString() + " Finished cleaning up TravelerStats ");

        }
          //Durga VSPLUS 1874 6/26/2015
        private void Shrinkdb(string connection)
        {
            VSFramework.XMLOperation myAdapter = new VSFramework.XMLOperation();
            using (System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(myAdapter.GetDBConnectionString(connection)))
            {
                try
                {
                    con.Open();
                    SqlCommand command = new SqlCommand("DBCC SHRINKDATABASE(0)", con);
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                }
                finally
                {
                    if (con.State == ConnectionState.Open)
                    {
                        con.Close();
                    }
                }
            }
        }
        public void LogTableStatistics(string dataBase)
        {
            WriteAuditEntry("Print Log table Statistics... ");
            VSAdaptor objVSAdaptor = new VSAdaptor();
          
            string sql = "";
            System.Data.DataSet myDataSet = new System.Data.DataSet();
            System.Data.DataTable myTable = new System.Data.DataTable();
            myTable.TableName = "TableData";
            myDataSet.Tables.Add(myTable);

            sql = "SELECT V.Name AS 'TableName' , SUM(B.rows) AS 'RowCount' FROM sys.objects V INNER JOIN sys.partitions B ON V.object_id = B.object_id WHERE V.type = 'U' GROUP BY V.schema_id, V.Name";

            WriteAuditEntry("\t" + sql + "\t");

            try
            {
                objVSAdaptor.FillDatasetAny(dataBase, dataBase, sql, ref myDataSet, "TableData");
            }
            catch (Exception ex)
            {
                WriteAuditEntry("Exception = " + ex.ToString());
                WriteAuditEntry(DateTime.Now.ToString() + " Error Accessing the DailyTasks Table " + ex.Message + sql);
            }

            System.Data.DataView myView = new System.Data.DataView(myDataSet.Tables["TableData"]);
            DataRowView drv = null;

           
            string tableName = null;
            string rowCount = null;

            WriteAuditEntry("VitalSigns Database table Statics are as below.. ");

            foreach (DataRowView drv_loopVariable in myView)
            {
                drv = drv_loopVariable;
                try
                {
                    tableName = Convert.ToString(drv["TableName"]);
                    rowCount = Convert.ToString(drv["RowCount"]);
                    WriteAuditEntry("Table Name = " + tableName + "\r\n" + "\r\n" + " Row Count = " + rowCount);
                }
                catch (Exception ex)
                {
                    WriteAuditEntry(DateTime.Now.ToString() + " Problem in processing Rows... " + ex.ToString());
                }
            }
            WriteAuditEntry("\r\n" + "\r\n" + "***********************************************" + "\r\n" + "Finished!");
        }
        private void CleanUpVSTables()
        {
            
            VSFramework.XMLOperation myConnectionString = new VSFramework.XMLOperation();
            string connectionString = myConnectionString.GetDBConnectionString("VitalSigns");
            using (System.Data.SqlClient.SqlConnection connection = new System.Data.SqlClient.SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("CleanUpData", connection);
                    command.CommandTimeout = 2000;
                    command.CommandType = CommandType.StoredProcedure;
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    WriteAuditEntry("\r\n" +  "\r\n" + "Error while executing the CleanUpData stored Procedure--" + ex.Message + "\r\n");
                }
                finally
                {
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                }
            }
        }
        private void CleanUpStatsTable(string strTables)
        {
           
            VSFramework.XMLOperation myConnectionString = new VSFramework.XMLOperation();
            string connectionString = myConnectionString.GetDBConnectionString("VSS_Statistics");
            using (System.Data.SqlClient.SqlConnection connection = new System.Data.SqlClient.SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("CleanUpObsoleteData", connection);
                    command.CommandTimeout = 2000;
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@DelimatedString", strTables);
                    command.ExecuteNonQuery();
                    WriteAuditEntry("\r\n" + "\r\n" + "Success while executing the CleanUpObsoleteData stored Procedure--" + "\r\n");
                }
                catch (Exception ex)
                {
                    WriteAuditEntry("\r\n" + "\r\n" + "Error while executing the CleanUpObsoleteData stored Procedure--" + ex.Message + "\r\n");
                }
                finally
                {
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }

                }
            }
        }
        public void CleanUpObsoleteData()
        {
            WriteAuditEntry("Cleaning up obsolete and processed data....");
            VSAdaptor objVSAdaptor = new VSAdaptor();
            
            string sql = "";
            System.Data.DataSet myDataSet = new System.Data.DataSet();
            System.Data.DataTable myTable = new System.Data.DataTable();
            //11/24/2015 NS added for VSPLUS-2383
            SqlCommand sqlcmd = new SqlCommand();
            myTable.TableName = "DailyTasks";
            myDataSet.Tables.Add(myTable);

            sql = "SELECT distinct SourceTableName FROM DailyTasks";

            WriteAuditEntry("\r\n" + sql + "\r\n");

            try
            {
                objVSAdaptor.FillDatasetAny("VitalSigns", "vitalsigns", sql,ref myDataSet, "DailyTasks");
            }
            catch (Exception ex)
            {
                WriteAuditEntry("Exception = " + ex.ToString());
                WriteAuditEntry(DateTime.Now.ToString() + " Error Accessing the DailyTasks Table " + ex.Message + sql);
            }
           
         
          
            DataTable sourceTables = myDataSet.Tables["DailyTasks"];
            object tables = string.Join(",", (from sourceTable in sourceTables.TableName select sourceTable).ToList());
            WriteAuditEntry("\r\n" + "\r\n" + "Tables Will be impacted : " + tables + "\r\n");
           Task taskStatsCleanUp = Task.Factory.StartNew(() => CleanUpStatsTable(Convert.ToString(tables)));
            taskStatsCleanUp.Wait();
            System.Threading.Tasks.Task taskVSStatsCleanUp = Task.Factory.StartNew(() => CleanUpVSTables());
            taskVSStatsCleanUp.Wait();

            WriteAuditEntry("\r\n" + "\r\n" + " * **********************************************" + "\r\n" + "Finished!");
        }
        public void ProcessStoredProcedures(string StatName)
        {
            SqlConnection con = new SqlConnection();

            try
            {
                VSFramework.XMLOperation myAdapter = new VSFramework.XMLOperation();
                con.ConnectionString = myAdapter.GetDBConnectionString("VSS_Statistics");
                con.Open();
                SqlDataAdapter da = default(SqlDataAdapter);
                da = new SqlDataAdapter("PopulateTravelerSummaryStats", con);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.Parameters.Add(new SqlParameter("@StatName", SqlDbType.VarChar, 50));
                da.SelectCommand.Parameters["@StatName"].Value = StatName;
                da.SelectCommand.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                WriteAuditEntry(DateTime.Now.ToString() + " Exception in ProcessStoredProcedures: " + ex.Message);
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                    con.Dispose();
                }
            }


        }
     

    }
}
