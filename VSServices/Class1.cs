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
            
            LogUtils utils = new LogUtils();
			ServiceOnStart(args);

            try
            {
                connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["VitalSignsMongo"].ToString();
            }
            catch (Exception ex)
            {
                LogUtils.WriteHistoryEntry(DateTime.Now.ToString() + " Error getting the conneciton string. Error : " + ex.Message, "VSServices.txt", LogUtils.LogLevel.Normal);
            }

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
                        string builddt = Convert.ToString(System.IO.File.GetLastWriteTime(FilePath + newt.Rows[i]["AssemblyName"].ToString()));

                        try
                        {
                            strSQL = "select * from VS_AssemblyVersionInfo where  AssemblyName='" + newt.Rows[i]["AssemblyName"].ToString() + "' and NodeName='" + NodeName + "'";
                            myAdapter.FillDatasetAny("VitalSigns", "None", strSQL, ref DsSettings, "VS_AssemblyVersionInfo");
                            dt = myAdapter.FetchData(myConnectionString.GetDBConnectionString("VitalSigns"), strSQL);
                          //  dt = DsSettings.Tables["VS_AssemblyVersionInfo"];
                            if (dt.Rows.Count > 0)
                            {
                                strSQL = "Update VS_AssemblyVersionInfo set AssemblyVersion='" + ver.ToString() + "', ProductVersion='" + fversion + "',BuildDate='" + builddt + "' where  AssemblyName='" + newt.Rows[i]["AssemblyName"].ToString() + "' and NodeName='" + NodeName + "'";
								myAdapter.ExecuteNonQueryAny("VitalSigns", "VitalSigns", strSQL);
                            }
                            else
                            {
                                strSQL = "Insert into VS_AssemblyVersionInfo(AssemblyName,AssemblyVersion,ProductVersion,BuildDate,FileArea,NodeName) " + "values('" + newt.Rows[i]["AssemblyName"].ToString() + "','" + ver.ToString() + "','" + fversion + "','" + builddt + "','" + newt.Rows[i]["FileArea"].ToString() + "','" + NodeName + "')";
								myAdapter.ExecuteNonQueryAny("VitalSigns", "VitalSigns", strSQL);
                            }


                        }
                        catch (Exception ex)
                        {
                            LogUtils.WriteHistoryEntry(DateTime.Now.ToString() + ",Filepath: " + FilePath + ",sql: " + strSQL + ", error:" + ex.Message, "AssemblyVersionError.txt", LogUtils.LogLevel.Verbose);
                        }


                    }
                    catch (Exception ex)
                    {
                        LogUtils.WriteHistoryEntry(DateTime.Now.ToString() + " error: " + ex.Message, "AssemblyVersionError.txt" + ", error:" + ex.Message, LogUtils.LogLevel.Verbose);

                    }
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
			
			String sql = "";
			VSFramework.VSAdaptor adapter = new VSFramework.VSAdaptor();
            try
            {
                if (servers.Count > 0)
                {
                    for (int i = 0; i < servers.Count; i++)
                    {
                        MonitoredItems.MonitoredDevice server = servers.get_Item(i);
                        Type t = server.GetType();
                        if (server.InsufficentLicenses)
                        {

                            VSNext.Mongo.Repository.Repository<VSNext.Mongo.Entities.Status> repository = new VSNext.Mongo.Repository.Repository<VSNext.Mongo.Entities.Status>();
                            FilterDefinition<VSNext.Mongo.Entities.Status> filterDef = repository.Filter.Where(p => p.DeviceType.Equals(server.ServerType) && p.DeviceName.Equals(server.Name));
                            UpdateDefinition<VSNext.Mongo.Entities.Status> updateDef = repository.Updater
                                .Set(p => p.CurrentStatus, "Insufficient Licenses")
                                .Set(p => p.Details, "There are not enough licenses to scan this server.")
                                .Set(p => p.LastUpdated, DateTime.Now)
                                .Set(p => p.Description, "There are not enough licenses to scan this server.")
                                .Set(p => p.NextScan, DateTime.Now.AddDays(1))
                                .Set(p => p.StatusCode, "Maintenance");
                            repository.Update(filterDef, updateDef);

                            servers.Delete(server.Name);
                            i--;
                        }
                        if (!server.isMasterRunning)
                        {
                            VSNext.Mongo.Repository.Repository<VSNext.Mongo.Entities.Status> repository = new VSNext.Mongo.Repository.Repository<VSNext.Mongo.Entities.Status>();
                            FilterDefinition<VSNext.Mongo.Entities.Status> filterDef = repository.Filter.Where(p => p.DeviceType.Equals(server.ServerType) && p.DeviceName.Equals(server.Name));
                            UpdateDefinition<VSNext.Mongo.Entities.Status> updateDef = repository.Updater
                                .Set(p => p.CurrentStatus, "Insufficient Licenses")
                                .Set(p => p.Details, "Master Service stopped running. Could not assign the server to a Node.")
                                .Set(p => p.LastUpdated, DateTime.Now)
                                .Set(p => p.Description, "Master Service stopped running. Could not assign the server to a Node.")
                                .Set(p => p.NextScan, DateTime.Now.AddDays(1))
                                .Set(p => p.StatusCode, "Maintenance");
                            repository.Update(filterDef, updateDef);
                            
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



    
        public static Boolean UpdateServiceCollection(VSNext.Mongo.Entities.Enums.ServerType ServerType, String NodeName)
        {

            try
            {
                
                VSNext.Mongo.Repository.Repository<VSNext.Mongo.Entities.Nodes> repository = new VSNext.Mongo.Repository.Repository<VSNext.Mongo.Entities.Nodes>(connectionString);
                FilterDefinition<VSNext.Mongo.Entities.Nodes> filterDef = repository.Filter.Eq(i => i.Name, NodeName);
                if (repository.Find(filterDef).ToList()[0].CollectionResets.Contains(ServerType))
                {
                    UpdateDefinition<VSNext.Mongo.Entities.Nodes> updateDef = repository.Updater.PullFilter(i => i.CollectionResets, new FilterDefinitionBuilder<VSNext.Mongo.Entities.Enums.ServerType>().Eq(p => p, ServerType));
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

        }

	}
}
