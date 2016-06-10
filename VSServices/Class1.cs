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

namespace RPRWyatt.VitalSigns.Services
{
	public abstract class VSServices : System.ServiceProcess.ServiceBase
	{
		String logPath;
		Dictionary<string, string> fileNameDicnry = new Dictionary<string, string>();

		private static LogUtils.LogLevel DefaultLogLevel = LogUtils.LogLevel.Normal;

		protected override void OnStart(string[] args)
		{
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

			if (servers.Count > 0)
			{
				for (int i = 0; i < servers.Count; i++)
				{
				    MonitoredItems.MonitoredDevice server = servers.get_Item(i);
				    Type t = server.GetType();
				    if (server.InsufficentLicenses)
				    {

				        //7/8/2015 NS modified for VSPLUS-1959
				        sql = "DELETE FROM Status WHERE TypeANDName='" + server.Name + "-" + ServerTypeForTypeAndName + "';"
				            + " INSERT INTO Status (Type, Location, Category, Name, Status, Details, LastUpdate, Description, NextScan, TypeANDName, StatusCode) VALUES "
				            + "('" + ServerType + "','" + server.Location + "','" + server.Category + "','" + server.Name + "','Insufficient Licenses','There are not enough licenses to scan this server.',getDate(),"
				            + "'There are not enough licenses to scan this server.',dateadd(day,1,getdate()),'" + server.Name + "-" + ServerTypeForTypeAndName + "','Maintenance');";
				        adapter.ExecuteNonQueryAny("VitalSigns", "VitalSigns", sql);

				        servers.Delete(server.Name);
				        i--;
				    }
				    if (!server.isMasterRunning)
				    {
				        sql = "DELETE FROM Status WHERE TypeANDName='" + server.Name + "-" + ServerTypeForTypeAndName + "';"
				            + " INSERT INTO Status (Type, Location, Category, Name, Status, Details, LastUpdate, Description, NextScan, TypeANDName, StatusCode) VALUES "
				            + "('" + ServerType + "','" + server.Location + "','" + server.Category + "','" + server.Name + "','Master Service Stopped.','Master Service stopped running. Could not assign the server to a Node.',getDate(),"
				            + "'Master Service stopped running. Could not assign the server to a Node.',dateadd(day,1,getdate()),'" + server.Name + "-" + ServerTypeForTypeAndName + "','Maintenance');";
				        adapter.ExecuteNonQueryAny("VitalSigns", "VitalSigns", sql);

				        servers.Delete(server.Name);
				        i--;
				    }
				}

				AlertLibrary.Alertdll myAlert = new AlertLibrary.Alertdll();
				myAlert.SysMessageForLicenses();

			}
		}




		public class MicrosoftHelperObject
		{
			
			public void CheckForInsufficentLicenses(Object objServers, string ServerType, string ServerTypeForTypeAndName)
			{
				VSServices.CheckForInsufficentLicenses(objServers, ServerType, ServerTypeForTypeAndName);
			}

		}

	}
}
