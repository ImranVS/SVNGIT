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


        IRepository<DailyStatistics> dailyStatisticsRepository;
        IRepository<SummaryStatistics> summaryStatisticsRepository;
        IRepository<DailyTasks> dailyTasksRepository;
        IRepository<Status> statusRepository;
        IRepository<Outages > OutagesRepository;
        IRepository<Nodes> nodesRepository;
        IRepository<TravelerStats> travelerStatsRepository;
        IRepository<TravelerStatusSummary> travelerSummaryStatsRepository;
        IRepository<StatusDetails> statusDetailsRepository;
        IRepository<NameValue> nameValueRepository;
        IRepository<ConsolidationResults> consolidationResultsRepository;
        IRepository<EventsDetected> eventsRepository;

        IRepository<ValidLocation> validLocationsRepository;
        IRepository<MobileDeviceTranslations> mobileDeviceTranslationsRepository;
        IRepository<MobileDevices> mobileDevicesRepository;
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
           // string connetionString = System.Configuration.ConfigurationManager.AppSettings["VitalSignsMongo"];

            string connetionString = System.Configuration.ConfigurationManager.ConnectionStrings["VitalSignsMongo"].ToString();

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
                dailyStatisticsRepository = _unitOfWork.Repository<DailyStatistics>();
                summaryStatisticsRepository = _unitOfWork.Repository<SummaryStatistics>();
                dailyTasksRepository = _unitOfWork.Repository<DailyTasks>();
                statusRepository = _unitOfWork.Repository<Status>();
                nodesRepository = _unitOfWork.Repository<Nodes>();
                travelerStatsRepository = _unitOfWork.Repository<TravelerStats>();
                travelerSummaryStatsRepository = _unitOfWork.Repository<TravelerStatusSummary>();
                statusDetailsRepository = _unitOfWork.Repository<StatusDetails>();
                nameValueRepository = _unitOfWork.Repository<NameValue>();

                validLocationsRepository = _unitOfWork.Repository<ValidLocation>();
                consolidationResultsRepository = _unitOfWork.Repository<ConsolidationResults>();
                mobileDeviceTranslationsRepository = _unitOfWork.Repository<MobileDeviceTranslations>();
                OutagesRepository = _unitOfWork.Repository<Outages>();
                eventsRepository = _unitOfWork.Repository<EventsDetected>();
                try
                {
                    if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings[cultureName]))
                        culture = ConfigurationManager.AppSettings[cultureName];

                }
                catch (Exception ex)
                {
                    WriteAuditEntry("Exception in getting culture.  Error: " + ex.Message);
                    culture = "en-US";
                }
              

                RegistryHandler myRegistry = new RegistryHandler();
                try
                {
                    Expression<Func<NameValue, bool>> expression = (p => p.Name == "Log Level");

                    var result = nameValueRepository.Find(expression).FirstOrDefault();

                    if (result == null)
                    {
                        NameValue nameValue = new NameValue { Name = "Log Level", Value = "2" };
                        nameValueRepository.Insert(nameValue);
                    }
                    logLevel = myRegistry.ReadFromRegistry("Log Level") == null ? LogUtils.LogLevel.Verbose : (LogUtils.LogLevel)Convert.ToInt32(myRegistry.ReadFromRegistry("Log Level"));
                }
                catch (Exception ex)
                {
                    logLevel = LogUtils.LogLevel.Verbose;
                    WriteAuditEntry("Exception in getting logLevel.  Error: " + ex.Message);
                }

                try
                {
                    appPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                    appPath = string.IsNullOrEmpty(appPath) ? @"c:\" : appPath;
                }
                catch (Exception ex)
                {
                    appPath = @"c:\";
                    WriteAuditEntry("Exception in getting appPath.  Error: " + ex.Message);
                }
              
                try
                {
                    dateFormat = objDateUtils.GetDateFormat();
                }
                catch (Exception ex)
                {
                    WriteAuditEntry("Exception while handling Date Format.  Error: " + ex.Message);
                }
                try
                {
                  
                    string logBak = appPath + @"\Log_Files\Daily_Tasks_Log_Bak.txt";
                    logDest = appPath + @"\Log_Files\Daily_Tasks_Log.txt";
                    if (File.Exists(logBak))
                    {
                        File.Delete(logBak);
                    }
                    if (File.Exists(logDest))
                    {
                        File.Move(logDest, logBak);
                        File.Delete(logDest);
                    }
                }
                catch (Exception ex)
                {

                    WriteAuditEntry("Exception in moving old Daily_Tasks_Log.txt to Daily_Tasks_Log_Bak.txt.  Error: " + ex.Message);
                }
               
                //To DO 
                try
                {
                    myRegistry.WriteToRegistry("Daily Tasks Start", DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString());
                    myRegistry.WriteToRegistry("Daily Tasks Build", builddNumber);
                }
                catch (Exception ex)
                {

                    WriteAuditEntry("Error in WriteToRegistry.  Error: " + ex.Message);
                }
                try
                {
                    Expression<Func<NameValue, bool>> expression = (p => p.Name == "ProductName");

                    var result = nameValueRepository.Find(expression).FirstOrDefault();

                    if (result == null)
                    {
                        NameValue nameValue = new NameValue { Name = "ProductName", Value = "VitalSigns" };
                        nameValueRepository.Insert(nameValue);
                    }
                    productName = myRegistry.ReadFromRegistry("ProductName").ToString();
                    if (productName == "")
                    {
                        productName = "VitalSigns";

                    }
                }
                catch (Exception ex)
                {
                    WriteAuditEntry("Error in getting ProductName.  Error: " + ex.Message);
                    productName = "VitalSigns";
                }
                WriteAuditEntry(DateTime.Now.ToString() + " VitalSigns Daily Tasks service is starting up.");
              //  WriteAuditEntry(DateTime.Now.ToString() + " VitalSigns Daily Tasks Build Number: " + AssemblyInfo);
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
                    WriteAuditEntry("Daily Task is stopping because it is only supposed to run one the Primary Node....");
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
                    WriteAuditEntry("Building a list of statistics to consolidate ");
                    ConsolidateStatistics();
                }

                catch (Exception ex)
                {
                    WriteAuditEntry("Exception in ConsolidateStatistics ...." + ex.ToString());
                }

                try
                {
                    WriteAuditEntry("Starting CleanUpObsoleteData......");
                    Task cleanupObsoleteDatatask = Task.Factory.StartNew(() => CleanUpObsoleteData());

                    cleanupObsoleteDatatask.Wait();

                }
                catch (Exception ex)
                {
                    WriteAuditEntry("OOPS, error cleaning up old data...." + ex.ToString());
                }

             
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
                    Expression<Func<NameValue, bool>> filterExpression = (p => p.Name == "CleanUpTablesDate");

                     result = nameValueRepository.Find(filterExpression).FirstOrDefault();
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

            }
            catch (Exception ex)
            {
                WriteAuditEntry("OOPS, error in processing TravelerSummaryStats " + ex.ToString());
            }
            finally
            {
                this.Stop();
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

             //   appPath = @"C:\Program Files (x86)";
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

                        
                        Directory.Delete(myFolder,true);
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

                     // Directory.Move(folder, destFolder);

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
                
                int GoBackDays;
                RegistryHandler registry = new RegistryHandler();
                string registryName = "Daily Service Lookback Day Count";
                try
                {
                    
                    GoBackDays = Convert.ToInt32(registry.ReadFromRegistry(registryName).ToString());
                }
                catch (Exception ex)
                {
                    WriteAuditEntry("Exception in ConsolidateStatistics when getting lookback count. Error: " + ex.ToString());
                    GoBackDays = 3;
                }

                int n = 0;
                for (n = GoBackDays; n >= 1; n--)
                {
                    WriteAuditEntry("\r\n" + "\r\n" + "*************************************  ---> Processing " + DateTime.Today.AddDays(-n).ToString());

                    ProcessSpecificDate(DateTime.Today.AddDays(-n), "DATEADD(dd,-" + n.ToString() + ",GETDATE())");
                }

                try
                {

                    registry.WriteToRegistry(registryName, "3");
                }
                catch (Exception ex)
                {
                    WriteAuditEntry("Exception in ConsolidateStatistics when setting lookback count. Error: " + ex.ToString());
                }

                try
                {
                    CleanUpTravelerSummaryData();
                    ProcessTravelerstats();
                    

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

                var result = travelerStatsRepository.Collection.Aggregate()
                    .Group(x => new { x.DeviceId, x.MailServerName }, g => new { id = g.Key })
                    .Project(x => new { deviceId = x.id.DeviceId, MailServerName = x.id.MailServerName })
                    .ToList();

                foreach (var item in result)
                {
                    Expression<Func<TravelerStats, bool>> Expression = (p => p.MailServerName == item.MailServerName && p.DateUpdated < DateTime.Now && p.DeviceId == item.deviceId);
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
                    var min = travelerStatsRepository.Find(x => true).Where(x => x.DateUpdated < DateTime.Now).Min(x => x.DateUpdated).ToString();

                    Expression<Func<TravelerStats, bool>> minExpression = (p => p.MailServerName == item.MailServerName && p.DateUpdated == Convert.ToDateTime(min) && p.DeviceId == item.deviceId);
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
                    var max = travelerStatsRepository.Find(x => true).Where(x => x.DateUpdated < DateTime.Now).Max(x => x.DateUpdated).ToString();

                    Expression<Func<TravelerStats, bool>> maxExpression = (p => p.MailServerName == item.MailServerName && p.DateUpdated == Convert.ToDateTime(max) && p.DeviceId == item.deviceId);
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
                bool alreadyProcessed = false;
                try
                {
                    Expression<Func<ConsolidationResults, bool>> filterExpression = (p => p.ScanDate == SearchDate);

                    var  result = consolidationResultsRepository.Find(filterExpression).FirstOrDefault();
                    if(result!=null)
                    {
                        alreadyProcessed = true;
                    }
                }
                catch (Exception ex)
                {

                    alreadyProcessed = false;
                    WriteAuditEntry("Exception in getting  ConsolidationResults" + ex.Message);
                }
                try
                {
                    if (alreadyProcessed)
                    {
                        WriteAuditEntry(DateTime.Now.ToString() + " " + SearchDate.ToString() + " has already been processed", LogUtils.LogLevel.Normal);
                        return;
                    }
                   else
                    {
                        WriteAuditEntry(DateTime.Now.ToString() + " " + SearchDate.ToString() + " has NOT already been processed", LogUtils.LogLevel.Normal);
             


                              ConsolidationResults consolidationResults = new ConsolidationResults { ScanDate = SearchDate, Result = "Sucess" };
                        consolidationResultsRepository.Insert(consolidationResults);
                    }

                   
                }
                catch (Exception ex)
                {

                    WriteAuditEntry("Exception in inserting data into ConsolidationResults" + ex.Message);
                }
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

                        Expression<Func<DailyStatistics, bool>> filterDef = (x => ((x.StatName == dailyTask.StatName)
                            || (x.StatName.StartsWith(dailyTask.StatName + "@") && x.DeviceType == Enums.ServerType.Office365.ToDescription()))
                            && x.CreatedOn >= SearchDate && x.CreatedOn < SearchDate.AddDays(1));

                        switch (dailyTask.AggregationType.ToUpper())
                        {

                            case "AVG":
                                var avgResult = dailyStatisticsRepository.Collection.Aggregate()
                               		.Match(filterDef)
                                   .Group(g => new { g.DeviceId, g.StatName, g.DeviceName,g.DeviceType }, g => new { key = g.Key, value = g.Average(s => s.StatValue) })
                                   .Project(x => new SummaryStatistics
                                   {
                                       DeviceId = x.key.DeviceId,
                                       StatName = x.key.StatName,
                                       StatValue = x.value,
                                       DeviceName = x.key.DeviceName,
                                       DeviceType=x.key.DeviceType
                                   //StatDate= SearchDate


                               }).ToList().Select(x => { x.AggregationType = dailyTask.AggregationType.ToUpper(); return x; }).ToList(); ;
                                if (avgResult.Count > 0)
                                {
                                    foreach (var item in avgResult)
                                    {
                                        item.StatDate = SearchDate;
                                    }
                                    summaryStatisticsRepository.Insert(avgResult);


                                }

                                break;
                            case "SUM":
                                var sumResult = dailyStatisticsRepository.Collection.Aggregate()
                                   .Match(filterDef)
                                   .Group(g => new { g.DeviceId, g.StatName, g.DeviceName,g.DeviceType }, g => new { key = g.Key, value = g.Sum(s => s.StatValue) })
                                   .Project(x => new SummaryStatistics
                                   {
                                       DeviceId = x.key.DeviceId,
                                       StatName = x.key.StatName,
                                       StatValue = x.value,
                                       DeviceName = x.key.DeviceName,
                                       DeviceType=x.key.DeviceType

                                       //  StatDate = SearchDate
                                   }).ToList().Select(x => { x.AggregationType = dailyTask.AggregationType.ToUpper(); return x; }).ToList(); ;
                                if (sumResult.Count > 0)
                                {
                                    foreach (var item in sumResult)
                                    {
                                        item.StatDate = SearchDate;
                                    }
                                    summaryStatisticsRepository.Insert(sumResult);
                                }
                                break;
                            case "MAX":
                                var maxResult = dailyStatisticsRepository.Collection.Aggregate()
                                   .Match(filterDef)
                                   .Group(g => new { g.DeviceId, g.StatName, g.DeviceName,g.DeviceType }, g => new { key = g.Key, value = g.Max(s => s.StatValue) })
                                   .Project(x => new SummaryStatistics
                                   {
                                       DeviceId = x.key.DeviceId,
                                       StatName = x.key.StatName,
                                       StatValue = x.value,
                                       DeviceName = x.key.DeviceName,
                                       DeviceType=x.key.DeviceType

                                   }).ToList().Select(x => { x.AggregationType = dailyTask.AggregationType.ToUpper(); return x; }).ToList(); ;
                                if (maxResult.Count > 0)
                                {
                                    foreach (var item in maxResult)
                                    {
                                        item.StatDate = SearchDate;
                                       
                                    }
                                    summaryStatisticsRepository.Insert(maxResult);
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

        private void ConsolidateExchangeMailboxData(DateTime curDate, String searchDateStr = "")
        {
            try
            {
                Expression<Func<DailyStatistics, bool>> expression = (p => p.StatName.StartsWith("Mailbox."));
                var dailyStsts = dailyStatisticsRepository.Find(expression);

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
                    summaryStatisticsRepository.Insert(result);

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
                var dailyStsts = dailyStatisticsRepository.Find(expression);

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
                    summaryStatisticsRepository.Insert(result);

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

                    try
                    {
                        List<VSNext.Mongo.Entities.MobileDeviceTranslations> entitiesList = new List<MobileDeviceTranslations>();
                        root.Device.ToList().ForEach(x => entitiesList.Add(new MobileDeviceTranslations()
                        {
                            OriginalValue = x.DeviceType,
                            OSType = x.OSName,
                            TranslatedValue = x.TranslatedValue,
                            Type = "Device"
                        }));
                        mobileDeviceTranslationsRepository.Delete(x => x.Type == "Device");
                        mobileDeviceTranslationsRepository.Insert(entitiesList);
                    }
                    catch (Exception ex)
                    {
                        WriteAuditEntry(DateTime.Now.ToString() + " Error adding new device translations " + ex.ToString());
                    }
                }


                if ((root.OS.Length > 0))
                {

                    try
                    {
                        List<VSNext.Mongo.Entities.MobileDeviceTranslations> entitiesList = new List<MobileDeviceTranslations>();
                        root.OS.ToList().ForEach(x => entitiesList.Add(new MobileDeviceTranslations()
                        {
                            OriginalValue = x.OSType,
                            OSType = x.OSName,
                            TranslatedValue = x.TranslatedValue,
                            Type = "OS"
                        }));
                        mobileDeviceTranslationsRepository.Delete(x => x.Type == "OS");
                        mobileDeviceTranslationsRepository.Insert(entitiesList);
                    }
                    catch (Exception ex)
                    {
                        WriteAuditEntry(DateTime.Now.ToString() + " Error adding new OS translations " + ex.ToString());
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

    
        public void GenerateSummaryforAllServers(DateTime StatDate, string statName, string operation)
        {
            try
            {


                Expression<Func<DailyStatistics, bool>> expression = (p => p.StatName == statName);
                var dailyStsts = dailyStatisticsRepository.Find(expression);

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
                    summaryStatisticsRepository.Insert(result);

                 Expression<Func<DailyStatistics, bool>> deleteExpression = (p => p.CreatedOn.Date < StatDate.AddDays(-2) && p.StatName== statName);
                dailyStatisticsRepository.Delete(expression);
            }
            catch (Exception ex)
            {

                WriteAuditEntry(DateTime.Now.ToString() + " Error in processing SummaryforAllDomino for Stat" + statName + " Date = " + StatDate);
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

        private void CleanUpTravelerSummaryData()
        {
            try
            {
                WriteAuditEntry(DateTime.Now.ToString() + " Cleaning up TravelerStats for today in case the query has been run today already. ");


                try
                {
                    Expression<Func<TravelerStats, bool>> expression = (p => p.DateUpdated <= DateTime.Now.AddDays(-1));
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

        //TO Do delete
        private void CleanUpVSTables()
        {

            try
            {
                // Cleaning Up Status Details
                Expression<Func<StatusDetails, bool>> expression = (p => p.LastUpdate < DateTime.Now.AddMinutes(-120));
                statusDetailsRepository.Delete(expression);
                // Cleaning Up Status Table
                Expression<Func<Status, bool>> statusExpression = (p => p.CreatedOn < DateTime.Now.AddMinutes(-120));
                statusRepository.Delete(statusExpression);
              //  Cleaning Up TravelerStats Table

                Expression<Func<TravelerStats, bool>> travelerstatsExpression = (p => p.DateUpdated < DateTime.Now);
                travelerStatsRepository.Delete(travelerstatsExpression);

      
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
                //Delete daily stats older than 3 days since by then they should be consolidated
                Expression<Func<DailyStatistics, bool>> expression = (p => p.CreatedOn <= DateTime.Now.AddDays(-3));
                dailyStatisticsRepository.Delete(expression);
            }
            catch (Exception ex)
            {
                WriteAuditEntry("Exception deleting old data from daily stats collection = " + ex.ToString());
               
            }

            try
            {
                // Delete old outage records
                Expression<Func<Outages , bool>> expression = (p => p.CreatedOn <= DateTime.Now.AddDays(-30));
                OutagesRepository.Delete(expression);
                
            }
            catch (Exception ex)
            {
                WriteAuditEntry("Exception deleting old data from daily stats collection = " + ex.ToString());

            }

            try
            {
                // Delete events older than 30 days
                Expression<Func<EventsDetected, bool>> expression = (p => p.CreatedOn <= DateTime.Now.AddDays(-30) && p.EventDismissed != null);
                eventsRepository.Delete(expression);
               
            }
            catch (Exception ex)
            {
                WriteAuditEntry("Exception deleting old events from events_detected collection = " + ex.ToString());

            }

            try
            {
                // Delete mobile devices older than 2 days
                Expression<Func<MobileDevices, bool>> expression = (p => p.ModifiedOn <= DateTime.Now.AddDays(-2));
                mobileDevicesRepository.Delete(expression);

            }
            catch (Exception ex)
            {
                WriteAuditEntry("Exception deleting old events from events_detected collection = " + ex.ToString());

            }


            System.Threading.Tasks.Task taskVSStatsCleanUp = Task.Factory.StartNew(() => CleanUpVSTables());
            taskVSStatsCleanUp.Wait();

            WriteAuditEntry("\r\n" + "\r\n" + " * **********************************************" + "\r\n" + "Finished!");
        }
     

    }
}
