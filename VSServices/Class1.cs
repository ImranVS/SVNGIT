using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading;
using System.IO;
using System.Web;

using LogUtilities;
using System.Diagnostics;
using System.Reflection;

using MongoDB.Driver;

namespace RPRWyatt.VitalSigns.Services
{
	public abstract class VSServices : System.ServiceProcess.ServiceBase
	{
		String logPath;
		Dictionary<string, string> fileNameDicnry = new Dictionary<string, string>();
        static String connectionString = "";

        private static LogUtils.LogLevel DefaultLogLevel = LogUtils.LogLevel.Normal;

		protected override void OnStart(string[] args)
		{
            try
            {
                connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["VitalSignsMongo"].ToString();
            }
            catch (Exception ex)
            {
                LogUtils.WriteHistoryEntry(DateTime.Now.ToString() + " Error getting the conneciton string. Error : " + ex.Message, "VSServices.txt", LogUtils.LogLevel.Normal);
            }

            LogUtils utils = new LogUtils();
			ServiceOnStart(args);

            

        }

		protected abstract void ServiceOnStart(string[] args);

		public virtual MicrosoftHelperObject getMicrosoftHelperObject()
		{
			return new MicrosoftHelperObject();
		}

		public virtual void WriteAuditEntry(string strMsg, string fileName, LogUtils.LogLevel LogLevelInput = LogUtils.LogLevel.Normal)
		{
			LogUtils.WriteAuditEntry(strMsg, fileName, LogLevelInput);
		}

		public virtual void WriteDeviceHistoryEntry(string DeviceType, string DeviceName, string strMsg, LogUtils.LogLevel LogLevelInput = LogUtils.LogLevel.Normal)
		{
			LogUtils.WriteDeviceHistoryEntry(DeviceType, DeviceName, strMsg, LogLevelInput);
		}

		public virtual void WriteHistoryEntry(string strMsg, string fileName, LogUtils.LogLevel logLevelInput = LogUtils.LogLevel.Verbose)
		{
			LogUtils.WriteHistoryEntry(strMsg, fileName, logLevelInput);
		}

		public virtual void WriteEntryForNotResponding(string ServerName, string ServerType)
		{
			LogUtils.WriteEntryForNotResponding(ServerName + "-" + ServerType);
		}

		public virtual void WriteEntryForNotResponding(string TypeANDName)
		{
			LogUtils.WriteEntryForNotResponding(TypeANDName);
		}

        public static void ReadXmlFile(string NodeName)
        {
            try
            {
                VSFramework.VSAdaptor myAdapter = new VSFramework.VSAdaptor();
                VSFramework.XMLOperation myConnectionString = new VSFramework.XMLOperation();
                DataSet DsSettings = new DataSet();
                string FilePath = "";
                string strSQL = "";

                string VSWebPath = "";
                string LogFilesPath = "";
                try
                {
                    strSQL = "Select svalue from Settings Where sname='VSWebPath'";
                    myAdapter.FillDatasetAny("VitalSigns", "None", strSQL, ref DsSettings, "Settings");
                    VSWebPath = DsSettings.Tables["Settings"].Rows[0]["svalue"].ToString();
                }
                catch (Exception ex)
                {
                    VSWebPath = "0";
                }
                try
                {
                    strSQL = "Select svalue from Settings Where sname='Log Files Path'";
                    myAdapter.FillDatasetAny("VitalSigns", "None", strSQL, ref DsSettings, "Settings1");
                    LogFilesPath = DsSettings.Tables["Settings1"].Rows[0]["svalue"].ToString();
                    LogFilesPath = LogFilesPath.ToUpper().Substring(0, LogFilesPath.ToUpper().IndexOf("LOG_FILES"));

                }
                catch (Exception ex)
                {
                    LogFilesPath = "0";
                }
                LogUtils.WriteHistoryEntry(DateTime.Now.ToString() + ",VSWebPath: " + VSWebPath + ",LogFilesPath: " + LogFilesPath, "AssemblyVersionError.txt", LogUtils.LogLevel.Verbose);

                DataTable newt = new DataTable();
                newt.ReadXml(LogFilesPath + "Assemblies.xml");
                newt.Columns.Add("BuildDate");

                LogUtils.WriteHistoryEntry(DateTime.Now.ToString() + ",xml rows: " + newt.Rows.Count, "AssemblyVersionError.txt", LogUtils.LogLevel.Verbose);
                List<VSNext.Mongo.Entities.AssemblyInfo> listAssemblyInfo = new List<VSNext.Mongo.Entities.AssemblyInfo>();

                for (int i = 0; i <= newt.Rows.Count - 1; i++)
                {

                    DataTable dt = new DataTable();
                    try
                    {
                        try
                        {
                            if ((newt.Rows[i]["FileArea"].ToString() == "VSWebUI"))
                            {
                                FilePath = VSWebPath;
                            }
                            else if ((newt.Rows[i]["FileArea"].ToString() == "Services"))
                            {
                                FilePath = LogFilesPath;
                            }
                        }
                        catch
                        {
                            FilePath = "";
                        }
                        LogUtils.WriteHistoryEntry(DateTime.Now.ToString() + ",Filepath: " + FilePath, "AssemblyVersionError.txt", LogUtils.LogLevel.Verbose);

						string ver = AssemblyName.GetAssemblyName((FilePath + newt.Rows[i]["AssemblyName"].ToString())).Version.Major.ToString() + "." + AssemblyName.GetAssemblyName((FilePath + newt.Rows[i]["AssemblyName"].ToString())).Version.Minor.ToString() + "." + AssemblyName.GetAssemblyName((FilePath + newt.Rows[i]["AssemblyName"].ToString())).Version.Build.ToString();
                        FileVersionInfo fvi = FileVersionInfo.GetVersionInfo((FilePath + newt.Rows[i]["AssemblyName"].ToString()));
						string fversion = fvi.ProductMajorPart.ToString() + "." + fvi.ProductMinorPart.ToString() + "." + fvi.ProductBuildPart;
                        DateTime builddt = System.IO.File.GetLastWriteTime(FilePath + newt.Rows[i]["AssemblyName"]);
                        try
                        {
                            listAssemblyInfo.Add(new VSNext.Mongo.Entities.AssemblyInfo()
                            {
                                AssemblyName = newt.Rows[i]["AssemblyName"].ToString(),
                                AssemblyVersion = ver.ToString(),
                                ProductVersion = fversion,
                                BuildDate = builddt,
                                FileArea = newt.Rows[i]["FileArea"].ToString()
                            });
                            
                        }
                        catch (Exception ex)
                        {
                            LogUtils.WriteHistoryEntry(DateTime.Now.ToString() + " Error adding new AssemblyInfo to list error:" + ex.Message, "AssemblyVersionError.txt", LogUtils.LogLevel.Verbose);
                        }

                        }
                    catch (Exception ex)
                    {
                        LogUtils.WriteHistoryEntry(DateTime.Now.ToString() + " error: " + ex.Message, "AssemblyVersionError.txt" + ", error:" + ex.Message, LogUtils.LogLevel.Verbose);

                    }
                }

                try
                {
                    VSNext.Mongo.Repository.Repository<VSNext.Mongo.Entities.Nodes> repository = new VSNext.Mongo.Repository.Repository<VSNext.Mongo.Entities.Nodes>(connectionString);
                    FilterDefinition<VSNext.Mongo.Entities.Nodes> filterDef = repository.Filter.Eq(x => x.Name, NodeName);
                    UpdateDefinition<VSNext.Mongo.Entities.Nodes> updateDef = repository.Updater.Set(x => x.AssemblyInfo, listAssemblyInfo);
                    
                    repository.Update(filterDef, updateDef);
                }
                catch (Exception ex)
                {
                    LogUtils.WriteHistoryEntry(DateTime.Now.ToString() + " error when updating node collection. Error: " + ex.Message, "AssemblyVersionError.txt" + ", error:" + ex.Message, LogUtils.LogLevel.Verbose);

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

		public static void CheckForInsufficentLicenses(Object objServers, string ServerType, string ServerTypeForTypeAndName)
		{
			MonitoredItems.MonitoredDevicesCollection servers = objServers as MonitoredItems.MonitoredDevicesCollection;
			VSFramework.VSAdaptor adapter = new VSFramework.VSAdaptor();
            try
            {
                if (servers.Count > 0)
                {
                    for (int i = 0; i < servers.Count; i++)
                    {
                        MonitoredItems.MonitoredDevice server = servers.get_Item(i);
                        
                        if (server.InsufficentLicenses)
                        {
                            
                            VSNext.Mongo.Repository.Repository<VSNext.Mongo.Entities.Status> repository = new VSNext.Mongo.Repository.Repository<VSNext.Mongo.Entities.Status>(connectionString);
                            FilterDefinition<VSNext.Mongo.Entities.Status> filterDef = repository.Filter.Where(p => p.DeviceType.Equals(server.ServerType) && p.DeviceName.Equals(server.Name) && p.DeviceId.Equals(server.ServerObjectID));
                            UpdateDefinition<VSNext.Mongo.Entities.Status> updateDef = repository.Updater
                                .Set(p => p.CurrentStatus, "Insufficient Licenses")
                                .Set(p => p.Details, "There are not enough licenses to scan this server.")
                                .Set(p => p.LastUpdated, DateTime.Now)
                                .Set(p => p.Description, "There are not enough licenses to scan this server.")
                                .Set(p => p.NextScan, DateTime.Now.AddDays(1))
                                .Set(p => p.StatusCode, "Maintenance")
                                .Set(p => p.TypeAndName, server.Name + "-" + server.ServerType);
                            repository.Upsert(filterDef, updateDef);

                            servers.Delete(server.Name);
                            i--;
                        }
                        else if (!server.isMasterRunning)
                        {
                            VSNext.Mongo.Repository.Repository<VSNext.Mongo.Entities.Status> repository = new VSNext.Mongo.Repository.Repository<VSNext.Mongo.Entities.Status>(connectionString);
                            FilterDefinition<VSNext.Mongo.Entities.Status> filterDef = repository.Filter.Where(p => p.DeviceType.Equals(server.ServerType) && p.DeviceName.Equals(server.Name) && p.DeviceId.Equals(server.ServerObjectID));
                            UpdateDefinition<VSNext.Mongo.Entities.Status> updateDef = repository.Updater
                                .Set(p => p.CurrentStatus, "Insufficient Licenses")
                                .Set(p => p.Details, "Master Service stopped running. Could not assign the server to a Node.")
                                .Set(p => p.LastUpdated, DateTime.Now)
                                .Set(p => p.Description, "Master Service stopped running. Could not assign the server to a Node.")
                                .Set(p => p.NextScan, DateTime.Now.AddDays(1))
                                .Set(p => p.StatusCode, "Maintenance")
                                .Set(p => p.TypeAndName, server.Name + "-" + server.ServerType);
                            repository.Upsert(filterDef, updateDef);
                            
                            servers.Delete(server.Name);
                            i--;
                        }
                    }

                    //10/3/2016 NS commented out per discussion with Wes - the insufficient licenses system message is being queued 
                    //using the QueueSysMessage function elsewhere. The SysMessageForLicenses is outdated and will be removed from the Alertdll
                    //AlertLibrary.Alertdll myAlert = new AlertLibrary.Alertdll();
                    //myAlert.SysMessageForLicenses();

                }
            }
            catch(Exception ex)
            {

            }
		}

        public static MonitoredItems.MonitoredDevice SelectServerToMonitor(MonitoredItems.MonitoredDevicesCollection collection)
        {
            DateTime tNow = DateTime.Now;
            MonitoredItems.MonitoredDevice SelectedServer = null;
            VSFramework.RegistryHandler myRegistry = new VSFramework.RegistryHandler();

            //Look for ScanNow's
            try
            {

                VSNext.Mongo.Repository.Repository<VSNext.Mongo.Entities.Server> repository = new VSNext.Mongo.Repository.Repository<VSNext.Mongo.Entities.Server>(connectionString);
                FilterDefinition<VSNext.Mongo.Entities.Server> filterDef = repository.Filter.Eq(x => x.ScanNow, true) &
                    repository.Filter.Eq(x => x.DeviceType, collection.get_Item(0).ServerType);
                List<VSNext.Mongo.Entities.Server> list = repository.Find(filterDef).ToList();
                if (list.Count > 0)
                {
                    for (int i = 0; i < list.Count(); i++)
                    {
                        SelectedServer = collection.FindByObjectId(list[i].Id);
                        if (SelectedServer != null)
                            break;
                    }
                    if (SelectedServer != null)
                    {
                        filterDef = repository.Filter.Eq(x => x.Id, list[0].Id);
                        UpdateDefinition<VSNext.Mongo.Entities.Server> updateDef = repository.Updater.Set(x => x.ScanNow, false);
                        repository.Update(filterDef, updateDef);
                        return SelectedServer;
                    }
                }

            }
            catch (Exception ex)
            {

            }

            //Scans not scanned servers
            try
            {
                foreach (MonitoredItems.MonitoredDevice server in collection)
                {
                    if (server.Status == "Not Scanned" && server.Enabled == true && server.IsBeingScanned == false)
                    {
                        LogUtils.WriteDeviceHistoryEntry("All", "SelectServer", tNow.ToString() + " >>> Selecting " + server.Name + " because the status is " + server.Status, LogUtils.LogLevel.Verbose);
                        return server;
                    }
                }
            }
            catch (Exception ex)
            {

            }

            //Scans not responding server
            try
            {
                foreach (MonitoredItems.MonitoredDevice server in collection)
                {
                    if (server.Status == "Not Responding" && server.Enabled == true && server.IsBeingScanned == false)
                    {
                        if(DateTime.Compare(tNow, server.NextScan) > 0)
                        {
                            LogUtils.WriteDeviceHistoryEntry("All", "SelectServer", tNow.ToString() + " >>> Selecting " + server.Name + " because status is " + server.Status + ".  Next scheduled scan is " + server.NextScan.ToShortTimeString());
                            return server;
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }

            MonitoredItems.MonitoredDevicesCollection ScanCanidates = new MonitoredItems.MonitoredDevicesCollection();
            foreach(MonitoredItems.MonitoredDevice server in collection)
            {
                if(server.IsBeingScanned == false && server.Enabled == true)
                {
                    tNow = DateTime.Now;
                    if(DateTime.Compare(tNow, server.NextScan) > 0)
                    {
                        ScanCanidates.Add(server);
                    }
                }
            }

            if(ScanCanidates.Count == 0)
            {
                Thread.Sleep(10000);
                return null;
            }

            //Returns the server that is the most overdue
            tNow = DateTime.Now;
            return ScanCanidates.Cast<MonitoredItems.MonitoredDevice>().OrderBy(x => x.NextScan).ToList()[0];

        }

        public static Boolean UpdateServiceCollection(VSNext.Mongo.Entities.Enums.ServerType ServerType, String NodeName)
        {

            try
            {

                VSNext.Mongo.Repository.Repository<VSNext.Mongo.Entities.Nodes> repository = new VSNext.Mongo.Repository.Repository<VSNext.Mongo.Entities.Nodes>(connectionString);

                FilterDefinition<VSNext.Mongo.Entities.Nodes> filterDef = repository.Filter.Eq(i => i.Name, NodeName);
                if (repository.Find(filterDef).ToList()[0].CollectionResets.Where(x => x.DeviceType == ServerType.ToString() && x.Reset.HasValue && x.Reset.Value).Count() > 0)
                {
                    FilterDefinitionBuilder<VSNext.Mongo.Entities.CollectionReset> filterDefBuilder = new FilterDefinitionBuilder<VSNext.Mongo.Entities.CollectionReset>();

                    filterDef = filterDef & repository.Filter.ElemMatch(x => x.CollectionResets, filterDefBuilder.Eq(x => x.DeviceType, ServerType.ToString()));

                    UpdateDefinition<VSNext.Mongo.Entities.Nodes> updateDef = repository.Updater.Set(i => i.CollectionResets.ElementAt(-1).DateCleared, DateTime.Now)
                        .Set(i => i.CollectionResets.ElementAt(-1).Reset, false);
                    try
                    {
                        repository.Update(filterDef, updateDef);
                    }
                    catch (Exception ex) { }
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            { }
            return false;
        }

        public class MicrosoftHelperObject
		{
			
			public void CheckForInsufficentLicenses(Object objServers, string ServerType, string ServerTypeForTypeAndName)
			{
				VSServices.CheckForInsufficentLicenses(objServers, ServerType, ServerTypeForTypeAndName);
			}

            public Boolean UpdateServiceCollection(VSNext.Mongo.Entities.Enums.ServerType ServerType, String NodeName)
            {
                return VSServices.UpdateServiceCollection(ServerType, NodeName);
            }

            public MonitoredItems.MonitoredDevice SelectServerToMonitor(MonitoredItems.MonitoredDevicesCollection collection)
            {
                return VSServices.SelectServerToMonitor(collection);
            }
        }

	}
}
