using System;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VSFramework;
using System.IO;
using System.Data.SqlClient;
using System.Globalization;
using System.Threading;
using Ionic.Zip;
using VSNext.Mongo.Entities;
using VSNext.Mongo.Repository;
using System.Collections.Generic;
using RPRWyatt.VitalSigns.Services;
using System.Linq.Expressions;
using MongoDB.Driver;
using LogUtilities;
using System.ServiceProcess;
//using System.Data.Objects;


namespace VitalSignsDailyStats
{

    public partial class VitalSignsDailyTasks :VSServices
       
    {
        private static UnitOfWork _unitOfWork;


        IRepository<DailyStatistics> dailyStatasticsRepository;
        IRepository<SummaryStatistics> summaryStatasticsRepository;
        IRepository<DailyTasks> dailyTasksRepository;
        IRepository<TravelerStats> travelerStatusSummaryRepository;
        IRepository<Status> statusRepository;
        IRepository<Nodes> nodesRepository;
        IRepository<TravelerStats> travelerStatsRepository;
        IRepository<TravelerStatusSummary> travelerSummaryStatsRepository;
        IRepository<StatusDetails> statusDeatilsRepository;
        IRepository<NameValue> nameValueRepository;

        IRepository<ValidLocation> validLocationsRepository;
        List<string> diskNames = new List<string>();
        VSAdaptor objVsAdaptor = new VSAdaptor();
        string culture = "en-US";
        string cultureName = "CultureString";
        LogUtils.LogLevel logLevel = LogUtils.LogLevel.Normal;
        string logDest, appPath, auditText, htmlPath, statisticsPath, serversMdPath,
            dateFormat, productName;
        string companyName = "JNIT Inc. dba RPR Wyatt";
        LogUtils.LogLevel myLogLevel;
        int builddNumber;
        string[] dominoDiskNames = new string[101];
        DateUtils.DateUtils objDateUtils = new DateUtils.DateUtils();
        string[] MicrosoftDiskNames = new string[101];
        bool timeToStop = false;

        public VitalSignsDailyTasks()
        {
            string connetionString = System.Configuration.ConfigurationManager.AppSettings["MongoConnectionString"];
            int? tenantId = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["TenantId"]);
            _unitOfWork = new UnitOfWork(connetionString, tenantId);
            InitializeComponent();

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

        public void ServiceStart()
        {
            ServiceOnStart();
        }
        protected override void ServiceOnStart(string[] args = null)
        {
            try
            {
                dailyStatasticsRepository = _unitOfWork.Repository<DailyStatistics>();
                summaryStatasticsRepository = _unitOfWork.Repository<SummaryStatistics>();
                dailyTasksRepository = _unitOfWork.Repository<DailyTasks>();
                statusRepository = _unitOfWork.Repository<Status>();
                nodesRepository = _unitOfWork.Repository<Nodes>();
                travelerStatsRepository = _unitOfWork.Repository<TravelerStats>();
                travelerSummaryStatsRepository = _unitOfWork.Repository<TravelerStatusSummary>();
                statusDeatilsRepository = _unitOfWork.Repository<StatusDetails>();
                nameValueRepository = _unitOfWork.Repository<NameValue>();

                validLocationsRepository = _unitOfWork.Repository<ValidLocation>();
              
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings[cultureName]))
                    culture = ConfigurationManager.AppSettings[cultureName];

                RegistryHandler myRegistry = new RegistryHandler();

                logLevel = myRegistry.ReadFromRegistry("Log Level") == null ? LogUtils.LogLevel.Verbose : (LogUtils.LogLevel)Convert.ToInt32(myRegistry.ReadFromRegistry("Log Level"));

                //logLevel = LogUtils.LogLevel.Verbose;
                appPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                appPath = string.IsNullOrEmpty(appPath) ? @"c:\" : appPath;
                try
                {
                    dateFormat = objDateUtils.GetDateFormat();
                }
                catch (Exception ex)
                {
                    WriteAuditEntry("Exception while handling DateFormate.  Error: " + ex.Message);
                }
                logDest = appPath + @"\Log_Files\Daily_Tasks_Log.txt";
                if (File.Exists(logDest))
                {
                    File.Move(logDest, appPath + @"\Log_Files\Daily_Tasks_Log_Bak.txt");
                    File.Delete(logDest);
                }
                //To DO 
                try
                {
                  //  myRegistry.WriteToRegistry("Daily Tasks Start", DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString());
                 //   myRegistry.WriteToRegistry("Daily Tasks Build", builddNumber);
                }
                catch (Exception ex)
                {

                    WriteAuditEntry("Error in WriteToRegistry.  Error: " + ex.Message);
                }
                try
                {
                    //productName = myRegistry.ReadFromRegistry("ProductName").ToString();
                    //if (productName=="")
                    //{
                    //    productName = "VitalSigns";

                    //}
                }
                catch (Exception ex)
                {
                    WriteAuditEntry("Error in getting ProductName.  Error: " + ex.Message);
                    productName = "VitalSigns";
                }
                WriteAuditEntry(DateTime.Now.ToString() + " VitalSigns Daily Tasks service is starting up.");
                WriteAuditEntry(DateTime.Now.ToString() + " VitalSigns Daily Tasks Build Number: " + builddNumber);
                WriteAuditEntry(DateTime.Now.ToString() + " Copyright " + companyName + "  " + DateTime.Now.Year + " - All rights reserved." + "\r\n" + "\r\n");
                try
                {
                    DailyBackup();
                }
                catch (Exception ex)
                {
                    WriteAuditEntry("Error in processing DailyBackup.  Error: " + ex.Message);

                }
                bool isPrimaryNode = true;
                string sql = null;
                try
                {

                    if ((ConfigurationManager.AppSettings["VSNodeName"] != null))
                    {
                        VSFramework.XMLOperation myConnectionString = new VSFramework.XMLOperation();

                        string nodeName = System.Configuration.ConfigurationManager.AppSettings["VSNodeName"].ToString();
                        Expression<Func<Nodes, bool>> expression = (p => p.Name == nodeName);
                        var result = nodesRepository.Find(expression).FirstOrDefault();
                    
                        if (result != null)
                        {
                            isPrimaryNode = result.IsPrimary;
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
                    WriteAuditEntry("Building a list of all disk drives, if any. ");
                    BuildDriveList();
                }
                catch (Exception ex)
                {
                    WriteAuditEntry("Exception building Domino drives list ...." + ex.ToString());
                }


                try
                {
                    ConsolidateStatistics();
                }

                catch (Exception ex)
                {
                    WriteAuditEntry("Exception in ConsolidateStatistics ...." + ex.ToString());
                }

                try
                {

                    Task cleanupObsoleteDatatask = Task.Factory.StartNew(() => CleanUpObsoleteData());

                    cleanupObsoleteDatatask.Wait();

                }
                catch (Exception ex)
                {
                    WriteAuditEntry("OOPS, error cleaning up old data...." + ex.ToString());
                }

                // To do
                {
                    try
                    {
                        VSFramework.XMLOperation myConnectionString = new VSFramework.XMLOperation();

                        Expression<Func<NameValue, bool>> expression = (p => p.Name == "CleanUpTablesDate");

                        var result = nameValueRepository.Find(expression).FirstOrDefault();

                        if (result==null)
                        {
                            NameValue nameValue = new NameValue { Name = "CleanUpTablesDate", Value = "" };
                            nameValueRepository.Insert(nameValue);
                        }
                        
                        var svalue = string.IsNullOrEmpty(result.Value) ? DateTime.Now.AddDays(-7) : Convert.ToDateTime(result.Value);

                      
                        if (svalue.AddDays(7) < DateTime.Now)
                        {
                            WriteAuditEntry(DateTime.Now.ToString() + " Starting weekly cleanup.");
                            //   CleanupAnyTableWeekly();
                            //Kiran Dadireddy VSPLUS-2684
                            //  ShrinkDBLogOnWeeklyBasis();

                            try
                            {
                               
                             
                                var filterDefination = Builders<VSNext.Mongo.Entities.NameValue>.Filter.Where(p => p.Name == "CleanUpTablesDate");
                                var updateDefinitaion = nameValueRepository.Updater.Set(p => p.Value, DateTime.Now.ToString());
                                var cleanupTables = nameValueRepository.Update(filterDefination, updateDefinitaion);
                            }
                            catch (Exception ex)
                            {
                                WriteAuditEntry("Error in updating CleanUpTablesDate in name_value collection...." + ex.ToString());
                            }
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
                        //TO DO
                       //  LogTableStatistics("Vitalsigns");
                        //LogTableStatistics("VSS_Statistics");

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
                WriteAuditEntry("OOPS, error in processing TravelerSummaryStats" + ex.ToString());
            }
        }
        public bool SaveNameValues(VSNext.Mongo.Entities.NameValue nameValues)
        {
            bool result = true;
            try
            {
                nameValueRepository = _unitOfWork.Repository<NameValue>();
               
                    if (nameValueRepository.Collection.AsQueryable().Where(x => x.Name.Equals(nameValues.Name)).Count() > 0)
                    {
                        var filterDefination = Builders<VSNext.Mongo.Entities.NameValue>.Filter.Where(p => p.Name == nameValues.Name);
                        var updateDefinitaion = nameValueRepository.Updater.Set(p => p.Value, nameValues.Value);
                        var results = nameValueRepository.Update(filterDefination, updateDefinitaion);
                    }
                    else
                    {
                        nameValueRepository.Insert(nameValues);
                    }
               
            }
            catch (Exception ex)
            {
                result = false;
            }
            return result;
        }

        private void WriteAuditEntry(string message, LogUtils.LogLevel logLevel = LogUtils.LogLevel.Normal)
        {
          
            WriteAuditEntry(message, "Daily_Tasks_Log.txt", logLevel);
        }


        public void DailyBackup()
        {

            try
            {

                appPath = @"C:\Program Files (x86)";
                string[] LogFilesToBeRecreated = {
                                            "History.txt",
                                             "Daily_Tasks_Log.txt"
                                              };
                if (myLogLevel == LogUtils.LogLevel.Verbose)
                {
                    WriteAuditEntry(DateTime.Now.ToString() + " ***** Starting Daily Log File Zip Up ******* ");
                    WriteAuditEntry("\r\n");
                }



                if (!Directory.Exists(appPath + "\\Log_Files\\Backup\\"))
                {
                    Directory.CreateDirectory(appPath + "\\Log_Files\\Backup\\");
                }

                try
                {
                    WriteAuditEntry(DateTime.Now.ToString() + " Deleting any leftover *.txt files from the \\backup folder.");
                    string[] fileArray = null;
                    WriteAuditEntry(DateTime.Now.ToString() + " Cleaning up log files");
                    fileArray = Directory.GetFiles(appPath + "\\Log_Files\\Backup\\", "*.txt");

                    string myFile = null;
                    foreach (string myFile_loopVariable in fileArray)
                    {
                        myFile = myFile_loopVariable;
                      
                        File.Delete(myFile);
                        WriteAuditEntry(DateTime.Now.ToString() + " Deleting " + myFile);
                    }
                    fileArray = null;

                    string[] ExchangeFolders = Directory.GetDirectories(appPath + "\\Log_Files\\Backup\\");
                    string myFolder = null;
                    foreach (string myFolder_loopVariable in ExchangeFolders)
                    {
                        myFolder = myFolder_loopVariable;

                        
                        Directory.Delete(myFolder, true);
                        WriteAuditEntry(DateTime.Now.ToString() + " Deleting " + myFolder);
                    }
                    ExchangeFolders = null;

                }
                catch (Exception ex)
                {
                    WriteAuditEntry(DateTime.Now.ToString() + " Error cleaning up past log files " + ex.ToString());
                }

                WriteAuditEntry("\r\n" + "\r\n" + DateTime.Now.ToString() + "**********************************************");



                try
                {
                    string[] fileArray = null;
                    WriteAuditEntry(DateTime.Now.ToString() + " Moving the current log files to the backup folder");
                    fileArray = Directory.GetFiles(appPath + "\\Log_Files\\", "*.txt");

                    string myFile = null;
                    foreach (string myFile_loopVariable in fileArray)
                    {
                        myFile = myFile_loopVariable;
                        string dest = Path.Combine(appPath + "\\Log_Files\\backup\\", Path.GetFileName(myFile));
                        WriteAuditEntry(DateTime.Now.ToString() + " Moving " + myFile + " to " + dest);
                        File.Move(myFile, dest);
                        if (Array.IndexOf(LogFilesToBeRecreated, Path.GetFileName(myFile)) > -1)
                        {
                            File.Create(Path.GetFileName(myFile));
                        }

                    }
                    fileArray = null;

                    string[] ExchangeFolders = Directory.GetDirectories(appPath + "\\Log_Files\\");

                    string folder = null;
                    foreach (string folder_loopVariable in ExchangeFolders)
                    {
                        folder = folder_loopVariable;
                        if ((Path.GetFileName(folder).ToLower() == "backup"))
                        {
                            continue;
                        }
                        string destFolder = appPath + "\\Log_Files\\backup\\" + Path.GetFileName(folder) + "";
                        if ((Directory.Exists(destFolder) == false))
                        {
                            Directory.Move(folder, destFolder);
                            Directory.CreateDirectory(destFolder);
                        }
                        else
                        {
                            
                            Directory.Delete(destFolder);
                            Directory.Move(folder, destFolder);
                        }
                       
                        WriteAuditEntry(DateTime.Now.ToString() + " Moving folder " + folder + " to " + destFolder);
                       


                    }

                    ExchangeFolders = null;

                }
                catch (Exception ex)
                {
                    WriteAuditEntry(DateTime.Now.ToString() + " Error copying the files to backup: " + ex.ToString());
                }

             
                try
                {
                    string myzipfile = "";
                    myzipfile = appPath + "\\Log_Files\\backup\\" + DateTime.Now.DayOfWeek.ToString() + ".zip";
                    WriteAuditEntry(DateTime.Now.ToString() + " Deleting prior week's zip file, if present.");
                    File.Delete(myzipfile);

                }
                catch (Exception ex)
                {
                }

                
                try
                {
                    WriteAuditEntry("\r\n" + "\r\n" + DateTime.Now.ToString() + "**********************************************");
                    ZipFile myZip = new ZipFile();
                    string[] zipFileArray = null;
                    WriteAuditEntry(DateTime.Now.ToString() + " Creating new zip file");
                    zipFileArray = Directory.GetFiles(appPath + "\\Log_Files\\backup\\", "*.txt");
                    string myFile = null;
                    try
                    {
                        foreach (string myFile_loopVariable in zipFileArray)
                        {
                            myFile = myFile_loopVariable;
                            WriteAuditEntry(DateTime.Now.ToString() + " Zipping " + myFile);
                            myZip.AddFile(myFile, "");
                          
                        }
                    }
                    catch (Exception ex)
                    {
                        WriteAuditEntry(DateTime.Now.ToString() + " Error zipping: " + ex.ToString());
                    }

                    string[] zipFolderArray = null;
                    zipFolderArray = Directory.GetDirectories(appPath + "\\Log_Files\\backup\\");
                    string myFolder = null;
                    try
                    {
                        foreach (string myFolder_loopVariable in zipFolderArray)
                        {
                            myFolder = myFolder_loopVariable;
                            WriteAuditEntry(DateTime.Now.ToString() + " Zipping Folder " + myFolder);
                            myZip.AddDirectory(myFolder, Path.GetFileName(myFolder));
                        }
                    }
                    catch (Exception ex)
                    {
                        WriteAuditEntry(DateTime.Now.ToString() + " Error zipping folders: " + ex.ToString());
                    }

                    myZip.Save(appPath + "\\Log_Files\\backup\\" + DateTime.Now.DayOfWeek.ToString() + ".zip");
                    WriteAuditEntry(DateTime.Now.ToString() + " The zip file is created as " + DateTime.Now.DayOfWeek.ToString() + ".zip");
                }
                catch (Exception ex)
                {
                    WriteAuditEntry(DateTime.Now.ToString() + " Error creating zip file: ");
                }

               
                try
                {
                    WriteAuditEntry(DateTime.Now.ToString() + " Deleting the backup log files. ");
                    string[] fileArray = null;
                    fileArray = Directory.GetFiles(appPath + "\\Log_Files\\Backup", "*.txt");

                    string myFile = null;
                    foreach (string myFile_loopVariable in fileArray)
                    {
                        myFile = myFile_loopVariable;
                        WriteAuditEntry(DateTime.Now.ToString() + " Deleting " + myFile + "....");
                        File.Delete(myFile);
                    }

                    string[] ExchangeFolders = Directory.GetDirectories(appPath + "\\Log_Files\\Backup\\");
                    string myFolder = null;
                    foreach (string myFolder_loopVariable in ExchangeFolders)
                    {
                        myFolder = myFolder_loopVariable;
                        WriteAuditEntry(DateTime.Now.ToString() + " Deleting " + myFolder + "...");
                       
                        Directory.Delete(myFolder, true);
                    }
                    ExchangeFolders = null;

                }
                catch (Exception ex)
                {
                    WriteAuditEntry(DateTime.Now.ToString() + " Error cleaning up the current log files " + ex.ToString());
                }




                if (myLogLevel == LogUtils.LogLevel.Verbose)
                {
                    WriteAuditEntry(DateTime.Now.ToString() + " Finished Daily log file zip up.");
                }
                //1537'
                LoglevelToNormal();



              


            }
            catch (Exception ex)
            {

                WriteAuditEntry(DateTime.Now.ToString() + " Error in processing DailyBackup"+ex);
            }


        }

        private void LoglevelToNormal()
        {
            myLogLevel = LogUtils.LogLevel.Normal;
            RegistryHandler myRegistry = new RegistryHandler();
            myRegistry.WriteToRegistry("Log Level", LogUtils.LogLevel.Normal);
            myRegistry.WriteToRegistry("Log Level VSAdapter", LogUtils.LogLevel.Normal);
        }
        public void BuildDriveList()
        {
            try
            {
                var disks = statusRepository.All().Select(x => x.Disks).ToList();
                foreach (List<DiskStatus> item in disks)
                {
                    if (item != null)
                    {
                        var diskNamesList = item.Select(x => x.DiskName).ToList();
                        foreach (string diskName in diskNamesList)
                        {
                            if (!diskNames.Contains(diskName))
                                diskNames.Add(diskName);

                        }

                    }

                }
            }
            catch (Exception ex)
            {

                WriteAuditEntry(DateTime.Now.ToString() + " Error cleaning up the current log files " + ex.ToString());
            }
         
         

        }
    

        public void ConsolidateStatistics()
        {
            try
            {

                int GoBackDays = 3;

                int n = 0;
                for (n = GoBackDays; n >= 1; n--)
                {
                    WriteAuditEntry("\r\n" + "\r\n" + "*************************************  ---> Processing " + DateTime.Today.AddDays(-n).ToString());

                    ProcessSpecificDate(DateTime.Today.AddDays(-n), "DATEADD(dd,-" + n.ToString() + ",GETDATE())");
                }

                try
                {
                    ProcessTravelerstats();
                    CleanUpTravelerSummaryData();

                }
                catch (Exception ex)
                {
                    WriteAuditEntry("OOPS, error in deleting TravelerSummaryStats" + ex.ToString());
                }

            }
            catch (Exception ex)
            {

                WriteAuditEntry("OOPS, error in deleting ConsolidateStatistics" + ex.ToString());
            }

          



        }

        public void ProcessTravelerstats()
        {

            try
            {
                List<TravelerStatusSummary> summaryList = new List<TravelerStatusSummary>();

                var result = travelerStatsRepository.Collection.Aggregate().Group(x => x.DeviceId, g => new { deviceId = g.Key }).ToList();
             
                foreach (var item in result)
                {
                    Expression<Func<TravelerStats, bool>> Expression = (p => p.MailServerName != "" && p.DateUpdated < DateTime.Now && p.DeviceId == item.deviceId);
                    var travelerStatData = travelerStatsRepository.Find(Expression).ToList();
                    summaryList.Add(new TravelerStatusSummary
                    {
                        StatName = "OpenTimesDelta",
                        MailServerName = travelerStatData.FirstOrDefault().MailServerName,
                        DateUpdated = travelerStatData.FirstOrDefault().DateUpdated,
                       
                        DeviceId = item.deviceId,
                        c_000_001 = Convert.ToInt32(travelerStatData.Where(x => (x.Interval == "000-001")).Average(s => s.Delta)),
                        c_001_002 = Convert.ToInt32(travelerStatData.Where(x => (x.Interval == "001-002")).Average(s => s.Delta)),
                        c_002_005 = Convert.ToInt32(travelerStatData.Where(x => (x.Interval == "002-005")).Average(s => s.Delta)),
                        c_005_010 = Convert.ToInt32(travelerStatData.Where(x => (x.Interval == "005-010")).Average(s => s.Delta)),
                        c_010_030 = Convert.ToInt32(travelerStatData.Where(x => (x.Interval == "010-030")).Average(s => s.Delta)),
                        c_030_060 = Convert.ToInt32(travelerStatData.Where(x => (x.Interval == "030-060")).Average(s => s.Delta)),
                        c_060_120 = Convert.ToInt32(travelerStatData.Where(x => (x.Interval == "060-120")).Average(s => s.Delta)),
                        c_120_INF = Convert.ToInt32(travelerStatData.Where(x => (x.Interval == "120-INF")).Average(s => s.Delta)),
                        TravelerServerName= travelerStatData.FirstOrDefault().TravelerServerName,

                    });
                    var min = travelerStatsRepository.All().Where(x => x.DateUpdated < DateTime.Now).Min(x => x.DateUpdated).ToString();

                    Expression<Func<TravelerStats, bool>> minExpression = (p => p.MailServerName != "" && p.DateUpdated == Convert.ToDateTime(min) && p.DeviceId == item.deviceId);
                    var travelerStatDataforMin = travelerStatsRepository.Find(minExpression).ToList();
                    if (travelerStatDataforMin.Count > 0)
                    {
                        summaryList.Add(new TravelerStatusSummary
                        {
                            StatName = "CumulativeTimesMin",
                            DeviceId = item.deviceId,
                            MailServerName = travelerStatDataforMin.FirstOrDefault().MailServerName,
                            DateUpdated = travelerStatDataforMin.FirstOrDefault().DateUpdated,
                            c_000_001 = Convert.ToInt32(travelerStatDataforMin.Where(x => (x.Interval == "000-001")).Sum(s => s.OpenTimes)),
                            c_001_002 = Convert.ToInt32(travelerStatDataforMin.Where(x => (x.Interval == "001-002")).Sum(s => s.OpenTimes)),
                            c_002_005 = Convert.ToInt32(travelerStatDataforMin.Where(x => (x.Interval == "002-005")).Sum(s => s.OpenTimes)),
                            c_005_010 = Convert.ToInt32(travelerStatDataforMin.Where(x => (x.Interval == "005-010")).Sum(s => s.OpenTimes)),
                            c_010_030 = Convert.ToInt32(travelerStatDataforMin.Where(x => (x.Interval == "010-030")).Sum(s => s.OpenTimes)),
                            c_030_060 = Convert.ToInt32(travelerStatDataforMin.Where(x => (x.Interval == "030-060")).Sum(s => s.OpenTimes)),
                            c_060_120 = Convert.ToInt32(travelerStatDataforMin.Where(x => (x.Interval == "060-120")).Sum(s => s.OpenTimes)),
                            c_120_INF = Convert.ToInt32(travelerStatDataforMin.Where(x => (x.Interval == "120-INF")).Sum(s => s.OpenTimes)),
                            TravelerServerName= travelerStatDataforMin.FirstOrDefault().TravelerServerName,

                        });
                    }
                    var max = travelerStatsRepository.All().Where(x => x.DateUpdated < DateTime.Now).Max(x => x.DateUpdated).ToString();

                    Expression<Func<TravelerStats, bool>> maxExpression = (p => p.MailServerName != "" && p.DateUpdated == Convert.ToDateTime(max) && p.DeviceId == item.deviceId);
                    var travelerStatDataforMax = travelerStatsRepository.Find(maxExpression).ToList();
                    if (travelerStatDataforMax.Count > 0)
                    {
                        summaryList.Add(new TravelerStatusSummary
                        {
                            StatName = "CumulativeTimesMax",
                            MailServerName = travelerStatDataforMax.FirstOrDefault().MailServerName,
                            DateUpdated = travelerStatDataforMax.FirstOrDefault().DateUpdated,
                            DeviceId = item.deviceId,
                            c_000_001 = Convert.ToInt32(travelerStatDataforMax.Where(x => (x.Interval == "000-001")).Sum(s => s.OpenTimes)),
                            c_001_002 = Convert.ToInt32(travelerStatDataforMax.Where(x => (x.Interval == "001-002")).Sum(s => s.OpenTimes)),
                            c_002_005 = Convert.ToInt32(travelerStatDataforMax.Where(x => (x.Interval == "002-005")).Sum(s => s.OpenTimes)),
                            c_005_010 = Convert.ToInt32(travelerStatDataforMax.Where(x => (x.Interval == "005-010")).Sum(s => s.OpenTimes)),
                            c_010_030 = Convert.ToInt32(travelerStatDataforMax.Where(x => (x.Interval == "010-030")).Sum(s => s.OpenTimes)),
                            c_030_060 = Convert.ToInt32(travelerStatDataforMax.Where(x => (x.Interval == "030-060")).Sum(s => s.OpenTimes)),
                            c_060_120 = Convert.ToInt32(travelerStatDataforMax.Where(x => (x.Interval == "060-120")).Sum(s => s.OpenTimes)),
                            c_120_INF = Convert.ToInt32(travelerStatDataforMax.Where(x => (x.Interval == "120-INF")).Sum(s => s.OpenTimes)),
                            TravelerServerName= travelerStatDataforMax.FirstOrDefault().TravelerServerName,

                        });
                    }
                }


                var travelersummary = summaryList.ToList();
                if (travelersummary.Count > 0)
                {
                    travelerSummaryStatsRepository.Insert(travelersummary);
                }

            }
            catch (Exception ex)
            {

                throw;
            }




        }
        public void ProcessSpecificDate(System.DateTime SearchDate, string SearchDateSQL = "")
        {
            try
            {
                WriteAuditEntry(DateTime.Now.ToString() + " VitalSigns Daily Tasks service is consolidating statistics for " + SearchDate, LogUtilities.LogUtils.LogLevel.Normal);

                try
                {
                    ConsolidateDiskStats(SearchDate);

                }
                catch (Exception ex)
                {
                    WriteAuditEntry("Exception in Processing ConsolidateDominoDiskStats" + ex.Message);
                }


                try
                {
                    List<DailyTasks> dailyTasks = dailyTasksRepository.All().ToList();

                    List<SummaryStatistics> summaryStatistics = new List<SummaryStatistics>();
                    foreach (DailyTasks dailyTask in dailyTasks)
                    {
                        string name = dailyTask.StatName;
                        SummaryStatistics summaryStatistic = new SummaryStatistics();
                        switch (dailyTask.AggregationType.ToUpper())
                        {

                            case "AVG":
                                var avgResult = dailyStatasticsRepository.Collection.Aggregate()
                               .Match(x => x.StatName == dailyTask.StatName && x.CreatedOn >= SearchDate && x.CreatedOn < SearchDate.AddDays(1))

                                   .Group(g => new { g.DeviceId, g.StatName, g.DeviceName,g.DeviceType }, g => new { key = g.Key, value = g.Average(s => s.StatValue) })

                                   .Project(x => new SummaryStatistics
                                   {
                                       DeviceId = x.key.DeviceId,
                                       StatName = x.key.StatName,
                                       StatValue = x.value,
                                       DeviceName = x.key.DeviceName,
                                       DeviceType=x.key.DeviceType
                                   //StatDate= SearchDate


                               }).ToList();
                                if (avgResult.Count > 0)
                                {
                                    foreach (var item in avgResult)
                                    {
                                        item.StatDate = SearchDate;
                                    }
                                    summaryStatasticsRepository.Insert(avgResult);


                                }

                                break;
                            case "SUM":
                                var sumResult = dailyStatasticsRepository.Collection.Aggregate()
                                  .Match(x => x.StatName == dailyTask.StatName && x.CreatedOn >= SearchDate && x.CreatedOn < SearchDate.AddDays(1))
                                   .Group(g => new { g.DeviceId, g.StatName, g.DeviceName,g.DeviceType }, g => new { key = g.Key, value = g.Sum(s => s.StatValue) })
                                   .Project(x => new SummaryStatistics
                                   {
                                       DeviceId = x.key.DeviceId,
                                       StatName = x.key.StatName,
                                       StatValue = x.value,
                                       DeviceName = x.key.DeviceName,
                                       DeviceType=x.key.DeviceType

                                   //  StatDate = SearchDate
                               }).ToList();
                                if (sumResult.Count > 0)
                                {
                                    foreach (var item in sumResult)
                                    {
                                        item.StatDate = SearchDate;
                                    }
                                    summaryStatasticsRepository.Insert(sumResult);
                                }
                                break;
                            case "MAX":
                                var maxResult = dailyStatasticsRepository.Collection.Aggregate()
                                    .Match(x => x.StatName == dailyTask.StatName && x.CreatedOn >= SearchDate && x.CreatedOn < SearchDate.AddDays(1))
                                   .Group(g => new { g.DeviceId, g.StatName, g.DeviceName,g.DeviceType }, g => new { key = g.Key, value = g.Max(s => s.StatValue) })
                                   .Project(x => new SummaryStatistics
                                   {
                                       DeviceId = x.key.DeviceId,
                                       StatName = x.key.StatName,
                                       StatValue = x.value,
                                       DeviceName = x.key.DeviceName,
                                       DeviceType=x.key.DeviceType
                                     
                               }).ToList();
                                if (maxResult.Count > 0)
                                {
                                    foreach (var item in maxResult)
                                    {
                                        item.StatDate = SearchDate;
                                       
                                    }
                                    summaryStatasticsRepository.Insert(maxResult);
                                }


                                break;
                        }


                    }

                }
                catch (Exception ex)
                {
                    WriteAuditEntry("Exception in Processing Dailtasks statnames" + ex.Message);
                }


                try
                {
                    //11/20/2015 NS modified for VSPLUS-2383
                    ConsolidateExchangeDatabases(SearchDate, SearchDateSQL);

                }
                catch (Exception ex)
                {
                    WriteAuditEntry("Exception in Processing ConsolidateExchangeDatabases" + ex.Message);
                }

                try
                {
                    //12/23/2015 WS modified for VSPLUS-1423
                    ConsolidateExchangeMailboxData(SearchDate, SearchDateSQL);

                }
                catch (Exception ex)
                {
                    WriteAuditEntry("Exception in Processing ConsolidateExchangeMailboxData" + ex.Message);
                }

                WriteAuditEntry("\r\n" + "\r\n" + "***********************************************" + "\r\n" + "Finished!");


            }
            catch (Exception ex)
            {

                WriteAuditEntry("Exception in  ProcessSpecificDate" + ex.Message);
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

        private void ConsolidateExchangeMailboxData(DateTime curDate, String searchDateStr = "")
        {
            try
            {
                Expression<Func<DailyStatistics, bool>> expression = (p => p.StatName.StartsWith("Mailbox."));
                var dailyStsts = dailyStatasticsRepository.Find(expression);

                var result = dailyStsts.GroupBy(g => new { g.StatName, g.DeviceName, g.DeviceId,g.DeviceType})

                             .Select(x => new SummaryStatistics
                             {

                                 StatValue = x.Sum(s => s.StatValue),
                                 DeviceName = x.Key.DeviceName,
                                 StatName = x.Key.StatName,
                                 DeviceId = x.Key.DeviceId,
                                 StatDate = curDate,
                                 DeviceType=x.Key.DeviceType


                             }).ToList();



                if (result.Count > 0)
                    summaryStatasticsRepository.Insert(result);

            }
            catch (Exception ex)
            {
                WriteAuditEntry(DateTime.Now.ToString() + "Error in processing ConsolidateExchangeMailboxData" + ex.Message);
            }

           
         

        }

        private void ConsolidateExchangeDatabases(DateTime curDate, String searchDateStr)
        {

            try
            {
                Expression<Func<DailyStatistics, bool>> expression = (p => p.StatName.StartsWith("ExDatabaseSizeMb"));
                var dailyStsts = dailyStatasticsRepository.Find(expression);

                var result = dailyStsts.GroupBy(g => new { g.StatName, g.DeviceName, g.DeviceId,g.DeviceType })

                             .Select(x => new SummaryStatistics
                             {

                                 StatValue = x.Average(s => s.StatValue),
                                 DeviceName = x.Key.DeviceName,
                                 StatName = x.Key.StatName,
                                 DeviceId = x.Key.DeviceId,
                                 StatDate = curDate,
                                 DeviceType=x.Key.DeviceType


                             }).ToList();




                if (result.Count > 0)
                    summaryStatasticsRepository.Insert(result);

            }
            catch (Exception ex)
            {

                WriteAuditEntry(DateTime.Now.ToString() + "Error in processing ConsolidateExchangeDatabases"+ex.Message);
            }
         

            

        }
        public void UpdateLocalTables()
        {

            try
            {
                string timeToUpdate = "false";


                Expression<Func<NameValue, bool>> expression = (p => p.Name == "LastTableUpdate");

                var result = nameValueRepository.Find(expression).FirstOrDefault();
                if (result != null)
                {
                    var svalue = string.IsNullOrEmpty(result.Value) ? DateTime.Now.AddDays(-7) : Convert.ToDateTime(result.Value);
                    if (svalue.AddDays(7) < DateTime.Now)
                    {
                        timeToUpdate = "true";
                    }
                    else
                    {
                        WriteAuditEntry(DateTime.Now.ToString() + " It is not time to update the local tables.");
                        return;
                    }


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
                        //objVsAdaptor.ExecuteNonQueryAny("VitalSigns", "", "DELETE FROM ValidLocations");
                        //SqlConnection con = objVsAdaptor.StartConnectionSQL("VitalSigns");
                        List<ValidLocation> validLocaions = new List<ValidLocation>();
                        string currentCounry = string.Empty;
                        List<string> states = null;
                        ValidLocation validLocation = null;
                        foreach (DataRow row in dt.Rows)
                        {

                            string country = Convert.ToString(row["Country"]);
                            string state = Convert.ToString(row["State"]);
                            if (country.Equals(currentCounry))
                            {
                                states.Add(state);
                            }
                            else
                            {
                                if (validLocation != null)
                                {
                                    validLocation.States = states;
                                    validLocaions.Add(validLocation);
                                }

                                validLocation = new ValidLocation();
                                validLocation.Country = country;
                                validLocation.States = new List<string>();
                                states = new List<string>();
                                states.Add(state);
                                currentCounry = country;
                            }
                        }
                        validLocation.States = states;
                        validLocaions.Add(validLocation);
                        validLocationsRepository.Insert(validLocaions);



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


                try
                {
                    var cleanUpTablesDate = new NameValue { Name = "LastTableUpdate", Value = DateTime.Now.ToString() };

                    var results = SaveNameValues(cleanUpTablesDate);

                }
                catch (Exception ex)
                {
                    WriteAuditEntry("Error in updating LastTableUpdate in name_value collection...." + ex.ToString());
                }
            }
            catch (Exception ex)
            {

                WriteAuditEntry(DateTime.Now.ToString() + "Error in processing UpdateLocalTables" + ex.Message);
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

            int DayNum = Convert.ToInt32(MyDate.DayOfWeek);


            if (DayNum == 0)
            {
                DayNum = 7;
            }


            return MyDate.AddDays(1 - DayNum);

        }

    

        public void DeleteDominoDailyStats()
        {
            //Durga  VSPLUS 2281
            //To Do
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

        public void GenerateSummaryforAllServers(DateTime StatDate, string statName, string operation)
        {
            try
            {


                Expression<Func<DailyStatistics, bool>> expression = (p => p.StatName == statName);
                var dailyStsts = dailyStatasticsRepository.Find(expression);

                var result = dailyStsts.GroupBy(g => new { g.DeviceName, g.StatName, g.DeviceId,g.DeviceType})

                             .Select(x => new SummaryStatistics
                             {
                               
                                 StatValue = x.Average(s => s.StatValue),
                                 DeviceName = x.Key.DeviceName,
                                 StatName = x.Key.StatName,
                                 DeviceId = x.Key.DeviceId,
                                 StatDate= StatDate,
                                 DeviceType=x.Key.DeviceType


                             }).ToList();




                if (result.Count > 0)
                    summaryStatasticsRepository.Insert(result);

                 Expression<Func<DailyStatistics, bool>> deleteExpression = (p => p.CreatedOn.Date < StatDate.AddDays(-2) && p.StatName== statName);
                dailyStatasticsRepository.Delete(expression);
            }
            catch (Exception ex)
            {

                throw ex;
            }


            WriteAuditEntry(DateTime.Now.ToString() + " Finished processing SummaryforAllDomino for Stat" + statName + " Date = " + StatDate);
            Thread.Sleep(250);
        }
        public void ConsolidateDiskStats(System.DateTime SearchDate)
        {
            try
            {
                string myDiskFree = "";

                foreach (string diskname in diskNames)
                {
                    myDiskFree = diskname + ".Free";
                    GenerateSummaryforAllServers(SearchDate, myDiskFree, "AVG");
                }
            }
            catch (Exception ex)
            {

                WriteAuditEntry(DateTime.Now.ToString() + " Error ConsolidateDiskStats " + ex.ToString());
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
            try
            {
                WriteAuditEntry(DateTime.Now.ToString() + " Cleaning up TravelerStats for today in case the query has been run today already. ");

                DateTime SearchDate = default(DateTime);
                SearchDate = Convert.ToDateTime(FixDate(DateTime.Today.AddDays(-30)));

                try
                {
                    Expression<Func<TravelerStats, bool>> expression = (p => p.DateUpdated <= DateTime.Now);
                    travelerStatsRepository.Delete(expression);
                }
                catch (Exception ex)
                {
                    WriteAuditEntry(DateTime.Now.ToString() + " Error in cleaning up TravelerStats " + ex.ToString());
                }

                WriteAuditEntry(DateTime.Now.ToString() + " Finished cleaning up TravelerStats ");
                try
                {
                    GC.Collect();
                }
                catch (Exception ex)
                {

                    WriteAuditEntry(DateTime.Now.ToString() + " Error in processing of garbage collection" + ex.ToString());
                }
            }
            catch (Exception ex)
            {

                WriteAuditEntry(DateTime.Now.ToString() + "Error in  Cleaning up TravelerStats for today in case the query has been run today already. ");
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
        //TO Do delete
        private void CleanUpVSTables()
        {

            try
            {
                //Cleaning Up Status Details
                statusDeatilsRepository.Delete();
                //Cleaning Up Status Table
                //   statusRepository.Delete();
                //Cleaning Up TravelerStats Table

              //  Expression<Func<TravelerStats, bool>> expression = (p => p.DateUpdated < DateTime.Now);
              //  travelerStatsRepository.Delete(expression);

                //To Do pending for Clean up Alert History

            }
            catch (Exception ex)
            {
                WriteAuditEntry("Error in Clean up VS Tables" + ex.ToString());

            }


        }

        public void CleanUpObsoleteData()
        {
            WriteAuditEntry("Cleaning up obsolete and processed data....");
            VSAdaptor objVSAdaptor = new VSAdaptor();
            try
            {
                Expression<Func<DailyStatistics, bool>> expression = (p => p.CreatedOn <= DateTime.Now.AddDays(-2));
                dailyStatasticsRepository.Delete(expression);
            }
            catch (Exception ex)
            {
                WriteAuditEntry("Exception = " + ex.ToString());
               
            }
        
            System.Threading.Tasks.Task taskVSStatsCleanUp = Task.Factory.StartNew(() => CleanUpVSTables());
            taskVSStatsCleanUp.Wait();

            WriteAuditEntry("\r\n" + "\r\n" + " * **********************************************" + "\r\n" + "Finished!");
        }
     

    }
}
