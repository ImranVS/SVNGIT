using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;

using System.Data.SqlClient;
using System.Security;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Management.Automation.Remoting;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Configuration;
using MaintenanceDLL;
using VSFramework;
using System;
//using System.Net.Http;
//using System.Net.Http.Headers;
using System.Threading.Tasks;
namespace VitalSignsMicrosoftClasses
{
	public class Office365MAIN
	{
		string CurrentCulture = "en-US";
		string CultureStringName = "CultureString";
		CultureInfo c;
		string NodeName = "";
		string serverURL = "https://outlook.office365.com";
		MonitoredItems.Office365ServersCollection myOffice365Servers;

		string serverType = "Office365";
		public void StartProcess(dynamic MicrosoftHelperObj)
		{
			//string RawValue = "windows 10.0.156 ";
			//RawValue = RawValue.Substring(0, RawValue.LastIndexOf(' '));
			// num = "windows 10.0.156";
			///string doubleVal = Convert.ToString(string.Join(".", num.Split('.').Take(3)));
			try
			{
				Common.initHelperClasses(MicrosoftHelperObj);
			}
			catch (Exception ex)
			{
				Common.WriteDeviceHistoryEntry("All", serverType, "Error setting Helper Class", Common.LogLevel.Normal);
			}

			try
			{
				CurrentCulture = ConfigurationManager.AppSettings[CultureStringName].ToString();
			}
			catch (Exception ex)
			{
				CurrentCulture = "en-US";
			}
			c = new CultureInfo(CurrentCulture);

			try
			{
				Common.WriteDeviceHistoryEntry("All", serverType, serverType +" Service is starting up", Common.LogLevel.Normal);
				//Sets the log level
				Common.setLogLevel();

				try
				{
					VSFramework.RegistryHandler mySettigns = new VSFramework.RegistryHandler();
					mySettigns.WriteToRegistry("VS " + serverType +" Service Start", DateTime.Now.ToString());
				}
				catch
				{
					Common.WriteDeviceHistoryEntry("All", serverType, "Error updating start time for Service in settings table", Common.LogLevel.Normal);
				}

				//creates colelction and starts thread to monitor changes for the colelction
				CommonDB db = new CommonDB();

				if (db.RecordExists("SELECT * FROM SelectedFeatures sf WHERE sf.FeatureID in (SELECT ID FROM Features WHERE Name='" + "Office 365" + "')"))
				{
					DataTable dt = db.GetData("select svalue from dbo.Settings where sname='Office365URL'");
						if (dt.Rows.Count >0)
							if(dt.Rows[0][0].ToString() != "")
								serverURL =dt.Rows[0][0].ToString();


					Common.WriteDeviceHistoryEntry("All", serverType, "Server is marked for scanning so will start Server Related Tasks", Common.LogLevel.Normal);
					CreateOffice365ServersCollection();
					InitStatusTable(myOffice365Servers);

					StartO365Threads();

					//sleep for one minute to allow time for the collection to be made 
					Thread.Sleep(60 * 1000 * 1);

					Thread HourlyTasksThread = new Thread(new ThreadStart(HourlyTasks));
					HourlyTasksThread.CurrentCulture = c;
					HourlyTasksThread.IsBackground = true;
					HourlyTasksThread.Name = "HourlyTasks - O365";
					HourlyTasksThread.Priority = ThreadPriority.Normal;
					HourlyTasksThread.Start();
					Thread.Sleep(2000);

					//Being handled elsewhere...Common -> MonitorTables -> CheckForTableChanges
					/*Thread monitorChanges = new Thread(new ThreadStart(CheckForTableChanges));
					monitorChanges.CurrentCulture = c;
					monitorChanges.IsBackground = true;
					monitorChanges.Priority = ThreadPriority.Normal;
					monitorChanges.Name = "CheckForTableChanges";
					monitorChanges.Start();
					Thread.Sleep(2000);
					*/
					Thread DailyTasksThread = new Thread(new ThreadStart(DailyTasks));
					DailyTasksThread.CurrentCulture = c;
					DailyTasksThread.IsBackground = true;
					DailyTasksThread.Priority = ThreadPriority.Normal;
					DailyTasksThread.Name = "DailyTasks - O365";
					DailyTasksThread.Start();
					Thread.Sleep(2000);
				}
				else
				{
					myOffice365Servers = null;
					Common.WriteDeviceHistoryEntry("All", serverType, "Server is not marked for scanning", Common.LogLevel.Normal);
				}


				

				Common.WriteDeviceHistoryEntry("All", serverType, "All Processes are started in startProcess", Common.LogLevel.Normal);
			}
			catch (Exception ex)
			{
				Common.WriteDeviceHistoryEntry("All", serverType, "Error starting StartProcess exception CreateO365ServersCollection: " + ex.StackTrace.ToString(), Common.LogLevel.Normal);
				throw ex;

			}

		}
		int serverThreadCount = 0;
		int initialServerThreadCount = 0;
		System.Collections.ArrayList AliveServerMainThreads = new System.Collections.ArrayList();
		private void StartO365Threads()
		{
			int startThreads = 0;
			serverThreadCount = myOffice365Servers.Count / 3;
			if (serverThreadCount <= 1)
				if (myOffice365Servers.Count > 1)
				serverThreadCount = 2;
				else
					serverThreadCount = 1;
			if (serverThreadCount <= 1 && myOffice365Servers.Count > 1)
				serverThreadCount = 2;

			if (serverThreadCount > 34)
				serverThreadCount = 35;
			startThreads = initialServerThreadCount;
			if (initialServerThreadCount > serverThreadCount)
			{
				//remove the extra threads
				int j = initialServerThreadCount - serverThreadCount;
				//if inital threads are 5 and current threads are 3
				//5-3=2: //remove 2 threads
				foreach (Thread th in AliveServerMainThreads)
				{
					if (j > 0)
					{
						if (th.IsAlive)
							th.Abort();
						j -= 1;
					}
				}
			}
			initialServerThreadCount = serverThreadCount;

			Common.WriteDeviceHistoryEntry("All", serverType, "There are startThreads-" + startThreads + " threads open", Common.LogLevel.Normal);
			Common.WriteDeviceHistoryEntry("All", serverType, "There are " + serverThreadCount + " threads open", Common.LogLevel.Normal);
			for (int i = startThreads; i < serverThreadCount; i++)
			{
				Common.WriteDeviceHistoryEntry("All", serverType, "Getting the server to monitor-", Common.LogLevel.Normal);
				//workingThread = new Thread(() => RoleMonitoring(ClassName, results, AllTestResults, thisServer, ref AlivePSO));
				Thread monitorServer = new Thread(() => MonitorO365Server(i));
				monitorServer.CurrentCulture = c;
				monitorServer.IsBackground = true;
				monitorServer.Priority = ThreadPriority.Normal;
				monitorServer.Name = i.ToString() + "-O365 Monitoring";
				AliveServerMainThreads.Add(monitorServer);
				monitorServer.Start();
				Thread.Sleep(2000);
			}
			

			

		}
		
		public MonitoredItems.Office365Server SelectServerToMonitor()
		{
			MonitoredItems.Office365Server SelectedServer = null;

			try
			{


				DateTime tNow = DateTime.Now;
				DateTime tScheduled;

				DateTime timeOne;
				DateTime timeTwo;


				MonitoredItems.Office365Server ServerOne = null;
				MonitoredItems.Office365Server ServerTwo = null;

				RegistryHandler myRegistry = new RegistryHandler();
				String ScanASAP = "";
				//2494
				String strSQL = "";
				String ServerType = "Office365";
				CommonDB db = new CommonDB();
				String serverName = "";

				try
				{


					strSQL = "Select svalue from ScanSettings where sname = 'Scan" + ServerType + "ASAP'";
					DataTable dt = db.GetData(strSQL);
					foreach (DataRow row in dt.Rows)
					{
						try
						{
							serverName = row[0].ToString();
						}
						catch (Exception ex)
						{
							continue;
						}

						for (int n = 0; n < myOffice365Servers.Count; n++)
						{
							ServerOne = myOffice365Servers.get_Item(n);
							if (ServerOne.Name == serverName && ServerOne.IsBeingScanned == false && ServerOne.Enabled)
							{
								Common.WriteDeviceHistoryEntry("All", "Office365", serverName + " was marked 'Scan ASAP' so it will be scanned next.");

								strSQL = "DELETE FROM ScanSettings where sname = 'Scan" + ServerType + "ASAP' and svalue='" + serverName + "'";
								db.Execute(strSQL);

								return ServerOne;
							}

						}
					}

				}
				catch (Exception ex)
				{

				}

//2494
				try
				{
					ScanASAP = myRegistry.ReadFromRegistry("ScanOffice365ASAP").ToString();
				}
				catch (Exception ex)
				{
					ScanASAP = "";
				}


				//Searches for the server marked as ScanASAP, if it exists
				for (int n = 0; n < myOffice365Servers.Count; n++)
				{
					ServerOne = myOffice365Servers.get_Item(n);
					if (ServerOne.Name == ScanASAP && ServerOne.IsBeingScanned == false && ServerOne.Enabled)
					{
						Common.WriteDeviceHistoryEntry("All", serverType, ScanASAP + " was marked 'Scan ASAP' so it will be scanned next.");
						myRegistry.WriteToRegistry("ScanADASAP", "n/a");

						//ServerOne.ScanASAP = true;

						return ServerOne;
					}

				}


				//Searches for the first enounter of a Not Responding server that is due for a scan
				for (int n = 0; n < myOffice365Servers.Count; n++)
				{
					ServerOne = myOffice365Servers.get_Item(n);
					if (ServerOne.Status == "Not Responding" && ServerOne.IsBeingScanned == false && ServerOne.Enabled)
					{
						tScheduled = ServerOne.NextScan;
						if (DateTime.Compare(tNow, tScheduled) > 0)
						{
							Common.WriteDeviceHistoryEntry("All", serverType, "Selecting " + ServerOne.Name + " because the status is " + ServerOne.Status + ".  Next scheduled scan is at " + tScheduled.ToString());
							return ServerOne;
						}
					}
				}


				//Searches for the first encounter of a server that has not been scanned yet
				for (int n = 0; n < myOffice365Servers.Count; n++)
				{
					ServerOne = myOffice365Servers.get_Item(n);
					if ((ServerOne.Status == "Not Scanned" || ServerOne.Status == "Master Service Stopped.") && ServerOne.IsBeingScanned == false && ServerOne.Enabled)
					{
						Common.WriteDeviceHistoryEntry("All", serverType, "Selecting " + ServerOne.Name + " because the status is " + ServerOne.Status + ".");
						return ServerOne;
					}
				}


				//Searches for all servers that are due for a scan
				List<MonitoredItems.Office365Server> ScanCanidates = new List<MonitoredItems.Office365Server>();

				foreach (MonitoredItems.Office365Server srv in myOffice365Servers)
				{
					if (srv.IsBeingScanned == false && ServerOne.Enabled)
					{
						tNow = DateTime.Now;
						tScheduled = srv.NextScan;
						if (DateTime.Compare(tNow, tScheduled) > 0)
						{
							ScanCanidates.Add(srv);
						}
					}
				}

				if (ScanCanidates.Count == 0)
				{
					Thread.Sleep(10000);
					return null;
				}



				//Start with the first two servers
				ServerOne = ScanCanidates.ElementAt(0);
				if (ScanCanidates.Count > 1)
					ServerTwo = ScanCanidates.ElementAt(1);

				if (ScanCanidates.Count > 2)
				{
					try
					{
						for (int n = 2; n < ScanCanidates.Count - 1; n++)
						{
							timeOne = ServerOne.NextScan;
							timeTwo = ServerTwo.NextScan;
							if (DateTime.Compare(timeOne, timeTwo) < 0)
							{
								//time on one is earlier, so keep one
								ServerTwo = ScanCanidates.ElementAt(n);
							}
							else
							{
								//time on two is ealier, so keep two
								ServerOne = ScanCanidates.ElementAt(n);
							}
						}
					}
					catch (Exception ex)
					{
						Common.WriteDeviceHistoryEntry("All", serverType, "Error Selecting Server... " + ex.Message);
					}
				}

				if (ServerTwo != null)
				{
					timeOne = ServerOne.NextScan;
					timeTwo = ServerTwo.NextScan;

					if (DateTime.Compare(timeOne, timeTwo) < 0)
					{
						SelectedServer = ServerOne;
						tScheduled = ServerOne.NextScan;
					}
					else
					{
						SelectedServer = ServerTwo;
						tScheduled = ServerTwo.NextScan;
					}
					tNow = DateTime.Now;
				}
				else
				{
					SelectedServer = ServerOne;
					tScheduled = ServerOne.NextScan;
				}

				tScheduled = SelectedServer.NextScan;
				if (DateTime.Compare(tNow, tScheduled) < 0)
				{
					if (SelectedServer.Status != "Not Scanned")
					{
						SelectedServer = null;
					}
				}
				else
				{
					TimeSpan mySpan = tNow - tScheduled;
				}
			}
			catch (Exception ex)
			{
				Common.WriteDeviceHistoryEntry("All", serverType, "Getting the server to monitor" + ex.Message.ToString(), Common.LogLevel.Normal);
			}
			return SelectedServer;

		}
		private void MonitorO365Server(int threadNum)
		{
			Common.WriteDeviceHistoryEntry("All", serverType, "Getting the server to monitor1", Common.LogLevel.Normal);
			Thread.CurrentThread.CurrentCulture = c;
			//MonitoredItems.ExchangeServer CurrentServer;
			try
			{
				CommonDB DB = new CommonDB();
				Common.WriteDeviceHistoryEntry("All", serverType, "Getting the server to monitor2", Common.LogLevel.Normal);
				while (true)
				{


					Common.WriteDeviceHistoryEntry("All", serverType, "Getting the server to monitor", Common.LogLevel.Normal);
					MonitoredItems.Office365Server thisServer = SelectServerToMonitor();
					try
					{
						if (thisServer != null && !thisServer.IsBeingScanned)
						{
							Common.WriteDeviceHistoryEntry("All", serverType, "Scanning Server " + thisServer.Name + " on thread " + threadNum, Common.LogLevel.Normal);
							thisServer.IsBeingScanned = true;

							MaintenanceDll maintenance = new MaintenanceDll();
							if (maintenance.InMaintenance(thisServer.ServerType, thisServer.Name))
							{
								ServerInMaintenance(thisServer.ServerType, thisServer);
								goto CleanUp;
							}
							Common.WriteDeviceHistoryEntry("All", serverType, "Scanning Server " + thisServer.Name + " on thread " + threadNum + "1", Common.LogLevel.Normal);
							TestResults AllTestResults = new TestResults();
							Office365Common Office365Common = new Office365Common();
							bool isResponding = true;
							//Office365Common.checkServer(thisServer, ref AllTestResults, ref isResponding);
							ReturnPowerShellObjects results = Office365Common.testO365ServerConnectivity(thisServer, ref AllTestResults, ref isResponding);

							//Thread workingThread;
							//System.Collections.ArrayList AliveThreads = new System.Collections.ArrayList();
							//workingThread = new Thread(() => startWindowsMonitoring(thisServer, ref AllTestResults));
							//workingThread.CurrentCulture = c;
							//workingThread.IsBackground = true;
							//workingThread.Priority = ThreadPriority.Normal;
							//workingThread.Name = "WIN - Exchange";
							//workingThread.Start();
							//AliveThreads.Add(workingThread);

							//MicrosoftCommon MSCommon = new MicrosoftCommon();
							//MSCommon.PrereqForWindows(thisServer, ref AllTestResults);

							//if (DB.GetData("SELECT * FROM WindowsServices WHERE ServerName='" + thisServer.Name + "'").Rows.Count == 0)
							//{
							//    string sql = "UPDATE WindowsServices SET Monitored=1, ServerRequired=1 WHERE ServerName='" + thisServer.Name + "' AND DisplayName like '%Active Directory%'";
							//    AllTestResults.SQLStatements.Add(new SQLstatements { DatabaseName = "vitalsigns", SQL = sql }); 
							//}
							string errorMessage = "";
							if (thisServer.ADFSMode && thisServer.ADFSRedirectTest == false)
								errorMessage = "ADFS Service Unavailable";
							if (thisServer.StatusCode == "Not Responding")
							{
								DB.NotRespondingQueries(thisServer, thisServer.ServerType);
								DB.ProcessAlerts(AllTestResults, thisServer, thisServer.ServerType);
							}
							else
							{
								Common.WriteDeviceHistoryEntry("All", serverType, "Start General Tests " + thisServer.Name + " on thread " + threadNum + ". Start Time:" + DateTime.Now.ToString() , Common.LogLevel.Normal);
								//sart a new thread to do rest api tests as that does'nt required a PS connection
								DateTime thTime = DateTime.Now.AddMinutes(10);//soma

								Thread workingThread = new Thread(() => Office365Common.doO365RESTApiTests(thisServer, ref AllTestResults));
								workingThread.CurrentCulture = c;
								workingThread.IsBackground = true;
								workingThread.Priority = ThreadPriority.Normal;
								workingThread.Name = "RestAPI";
								workingThread.Start();

								Office365Common.checkServer(thisServer, ref AllTestResults, results);

								//wait for rest api thread
								bool thComplete = false;
								while (thTime > DateTime.Now && !thComplete)
								{
									if (!workingThread.IsAlive)
										thComplete = true;
								}
								if (!thComplete)
									workingThread.Abort();

								//end wait for rest api thread

								Common.WriteDeviceHistoryEntry("All", serverType, "Stopping General Tests " + thisServer.Name + " on thread " + threadNum + ". End Time:" + DateTime.Now.ToString(), Common.LogLevel.Normal);
								DB.UpdateAllTests(AllTestResults, thisServer, thisServer.ServerType);
							}
							thisServer.IsBeingScanned = false;


						CleanUp:

							Common.WriteDeviceHistoryEntry("All", serverType, "Stopping scan on  Server " + thisServer.Name + " on thread " + threadNum, Common.LogLevel.Normal);

							AllTestResults = null;
							//thisServer.LastScan = DateTime.Now;
							thisServer.IsBeingScanned = false;
							thisServer = null;



						}
						else
						{
							try
							{
								Common.WriteDeviceHistoryEntry("ALL", serverType, "Server " + thisServer.Name + " is already being scanned and will not start scanning again", Common.LogLevel.Normal);
							}
							catch (Exception ex)
							{
								Common.WriteDeviceHistoryEntry("ALL", serverType, "Server returned as null", Common.LogLevel.Normal);
							}
						}


						Common.WriteDeviceHistoryEntry("All", serverType, "Waiting for 5 seconds to restart the Loop ", Common.LogLevel.Normal);
						// Sleep for 3 minutes 
						Thread.Sleep(1000 * 5);
						//break;
					}
					catch
					{
						if (thisServer != null)
						{
							thisServer.IsBeingScanned = false;
							thisServer = null;
						}

					}
				}
			}
			catch (Exception ex1)
			{
				Common.WriteDeviceHistoryEntry("All", serverType, "Error before the main loop: " + ex1.Message.ToString(), Common.LogLevel.Normal);
			}
			

		}


		private void ServerInMaintenance(string ServerType, MonitoredItems.Office365Server myServer)
		{
			CommonDB db = new CommonDB();

			SQLBuild objSQL = new SQLBuild();
			objSQL.ifExistsSQLSelect = "SELECT * FROM Status WHERE TypeANDName='" + myServer.Name + "-" + ServerType + "'";
			objSQL.onFalseDML = "INSERT INTO STATUS (NAME, STATUS, STATUSCODE, LASTUPDATE, TYPE, LOCATION, CATEGORY, TYPEANDNAME, DESCRIPTION, UserCount, ResponseTime, SecondaryRole,ResponseThreshold, " +
						"DominoVersion, OperatingSystem, NextScan, Details, CPU, Memory) VALUES ('" + myServer.Name + "', 'Maintenance', 'Maintenance', '" + DateTime.Now.ToString() + "','" + ServerType + "','" +
						myServer.Location + "','" + myServer.Category + "','" + myServer.Name + "-" + ServerType + "', 'Microsoft " + ServerType + " Server', 0, 0, '', " +
						"'" + myServer.ResponseThreshold + "', '" + serverType +"', '" + myServer.OperatingSystem + "', '" + myServer.NextScan + "', " +
						"'This server is in a scheduled maintenance period.  Monitoring is temporarily disabled.', 0, 0 )";

			objSQL.onTrueDML = "UPDATE Status set Status='Maintenance', StatusCode='Maintenance', LastUpdate='" + DateTime.Now + "', Details='This server is in a scheduled maintenance period.  Monitoring is temporarily disabled.'," +
				" UserCount=0, CPU=0, Memory=0 WHERE TypeANDName='" + myServer.Name + "-" + ServerType + "'";

			string sqlQuery = objSQL.GetSQL(objSQL);
			db.Execute(sqlQuery);

		}

		private void CreateOffice365ServersCollection()
		{
			//Fetch all servers
			if (myOffice365Servers == null)
				myOffice365Servers = new MonitoredItems.Office365ServersCollection();

			MonitoredItems.Office365ServersCollection newCollection = new MonitoredItems.Office365ServersCollection();
			CommonDB DB = new CommonDB();
			Boolean nodeScan=false;
			if (ConfigurationManager.AppSettings["VSNodeName"] != null)
				NodeName = ConfigurationManager.AppSettings["VSNodeName"].ToString();

			//string sSQL = "select * from O365Nodes,Nodes where Nodes.Id=O365Nodes.NodeId and Nodes.Name='" + NodeName + "'";
			//DataTable dtNodes = DB.GetData(sSQL.ToString());
			//if (dtNodes.Rows.Count > 0)
			//    nodeScan = true;
			
			StringBuilder SQL = new StringBuilder();
			//SQL.Append(" select distinct Sr.ID,Sr.ServerName,S.ServerType, S.ID as ServerTypeId,L.Location,sa.ScanInterval,sa.RetryInterval,sa.OffHourInterval,sa.Enabled,sr.ipaddress,sa.category,cr.UserID,cr.Password,sa.ResponseTime ");
			//SQL.Append("  from Servers Sr ");
			//SQL.Append(" inner join ServerTypes S on Sr.ServerTypeID=S.ID  inner join Locations L on Sr.LocationID =L.ID  left outer join ServerAttributes sa on sr.ID=sa.serverid ");
			//SQL.Append(" inner join credentials cr on sa.CredentialsId=cr.ID ");
			//SQL.Append(" where S.ServerType='" + serverType+"' and sa.Enabled = 1 order by sr.id");
			SQL.Append("select O365.ID,o365.Name,Category,ScanInterval,OffHoursScanInterval,ResponseThreshold,RetryInterval,UserName,PW,O365.ServerTypeId,ST.ServerType,Mode,ServerName,Cred.UserId,Cred.Password ");
			if (nodeScan)
				SQL.Append(",L.Location Location ");
			else
				SQL.Append(",L.Location ");
			SQL.Append(" from O365Server O365 inner join ServerTypes ST on O365.ServerTypeid=ST.ID ");

			//if (nodeScan)
			//{
				SQL.Append(" inner join O365Nodes ONDT on ONDT.O365ServerId=O365.Id ");
				SQL.Append(" inner join Nodes on Nodes.Id=ONDT.NodeId and Nodes.Name='" + NodeName + "'");
				SQL.Append(" inner join Locations L on Nodes.LocationId=L.Id ");
				SQL.Append(" left outer join Credentials Cred on Cred.Id=o365.CredentialsId ");

			//}
			//else
			//{
			//    if (ConfigurationManager.AppSettings["VSNodeName"] != null)
			//    {
			//        NodeName = ConfigurationManager.AppSettings["VSNodeName"].ToString();
			//        SQL.Append(" inner join DeviceInventory di on O365.ID=di.DeviceID and ST.ID=di.DeviceTypeId ");
			//        SQL.Append(" inner join Nodes on Nodes.ID=di.CurrentNodeId and Nodes.Name='" + NodeName + "' ");
			//        SQL.Append(" inner join Locations L on Nodes.LocationID=L.ID ");
			//        SQL.Append(" left outer join Credentials Cred on Cred.Id=o365.CredentialsId ");
			//    }
			//}
			SQL.Append(" WHERE enabled=1 ");
			DataTable dtServers = DB.GetData(SQL.ToString());
			//Loop through servers
			//MonitoredItems.ExchangeThresholdSettings ExchgThreshold = new MonitoredItems.ExchangeThresholdSettings();
			if (dtServers.Rows.Count > 0)
			{
				for (int i = 0; i < dtServers.Rows.Count; i++)
				{
					DataRow DR = dtServers.Rows[i];

					MonitoredItems.Office365Server MyO365Server = new MonitoredItems.Office365Server();
					//myExchangeServer.ThresholdSetting = ExchgThreshold;
					newCollection.Add(SetO365ServerSettings(MyO365Server, DR));

				}

				int updatedServers = 0;
				int newServers = 0;
				int removedServers = 0;

				//Removes servers not in the new lsit
				foreach (MonitoredItems.Office365Server server in myOffice365Servers)
				{
					string currName = server.Name;
					try
					{
						MonitoredItems.Office365Server newServer = newCollection.SearchByName(currName);
						if (newServer == null)
						{
							//incase it doesnt throw and exception, if not found, removed server from list
							myOffice365Servers.Delete(currName);
							removedServers++;

						}
					}
					catch (Exception ex)
					{
						//server not found
						myOffice365Servers.Delete(currName);
						removedServers++;

					}

				}


				// adds/updates new servers
				foreach (MonitoredItems.Office365Server server in newCollection)
				{
					string currName = server.Name;
					try
					{
						MonitoredItems.Office365Server oldServer = myOffice365Servers.SearchByName(currName);

						if (oldServer != null)
						{

							oldServer.IPAddress = server.IPAddress;
							oldServer.Name = server.Name;
							oldServer.UserName = server.UserName;
							oldServer.Password = server.Password;
							oldServer.Location = server.Location;
							oldServer.ResponseThreshold = server.ResponseThreshold;
							oldServer.ScanInterval = server.ScanInterval;
							oldServer.OffHoursScanInterval = server.OffHoursScanInterval;
							oldServer.RetryInterval = server.RetryInterval;
							
							oldServer.ServerDaysAlert = server.ServerDaysAlert;
							oldServer.FailureThreshold = server.FailureThreshold;
							oldServer.Category = server.Category;

							oldServer.Enabled = true;

							updatedServers++;
						}
						else
						{
							myOffice365Servers.Add(server);
							newServers++;
						}
					}
					catch (NullReferenceException ex)
					{
						myOffice365Servers.Add(server);
						newServers++;
					}

				}


				//myExchangeServers = newCollection;
				/**********************************************************/
				Common.WriteDeviceHistoryEntry("All",serverType , "There are " + myOffice365Servers.Count + " servers in the collection.  " + newServers + " were new and " + updatedServers + " were updated.");
			}
			else
			{
				myOffice365Servers = new MonitoredItems.Office365ServersCollection();
			}


			//At this point we have all Servers with ALL the information(including Threshold settings)
		}

		private MonitoredItems.Office365Server SetO365ServerSettings(MonitoredItems.Office365Server MyO365Server, DataRow DR)
		{
			try
			{
				MyO365Server.IPAddress = serverURL;
				MyO365Server.Name = DR["Name"].ToString();
				MyO365Server.ServerId = DR["ID"].ToString();
				MyO365Server.UserName = DR["UserName"].ToString();
				MyO365Server.Password = Common.decodePasswordFromEncodedString(DR["PW"].ToString(), MyO365Server.Name);
				MyO365Server.DirSyncPWD = Common.decodePasswordFromEncodedString(DR["Password"].ToString(), MyO365Server.Name);
				MyO365Server.DirSyncUID = DR["UserId"].ToString();
				MyO365Server.Mode = DR["Mode"].ToString();
				MyO365Server.DirSyncServerName = DR["ServerName"].ToString();
				//MyO365Server.Location = DR["Location"].ToString();
				//MyO365Server.Role = new String[0];
				MyO365Server.VersionNo = "NA";
				MyO365Server.ADFSMode = false;  //set it to false initially
				MyO365Server.ADFSRedirectTest = false;  //set it to false initially
				
				MyO365Server.ResponseThreshold = long.Parse(DR["ResponseThreshold"].ToString());
				MyO365Server.ScanInterval = int.Parse(DR["ScanInterval"].ToString());
				MyO365Server.OffHoursScanInterval = int.Parse(DR["OffHoursScanInterval"].ToString());
				MyO365Server.RetryInterval = int.Parse(DR["RetryInterval"].ToString());

				MyO365Server.LastScan = DateTime.Now;
				MyO365Server.Status = "Not Scanned";
				MyO365Server.StatusCode = "Maintenance";
				MyO365Server.ServerType = DR["ServerType"].ToString();
				MyO365Server.Location = DR["Location"].ToString();
				//MyExchangeServer.FailureThreshold = int.Parse(DR["ConsFailuresBefAlert"].ToString());
				if (NodeName !="")
					MyO365Server.Category =NodeName;
				else
					MyO365Server.Category = DR["Category"].ToString();
				MyO365Server.ServerTypeId = int.Parse(DR["ServerTypeId"].ToString());
				MyO365Server.Enabled = true;
				Common.WriteDeviceHistoryEntry("All", serverType, "In SetO365ServerSettings: 1", Common.LogLevel.Normal);
				CommonDB db = new CommonDB();
				DataTable dt = db.GetData("Select Tests, EnableSimulationTests, ResponseThreshold from Office365Tests where ServerId=" + MyO365Server.ServerId + "");

				foreach (DataRow row in dt.Rows)
				{
					Common.WriteDeviceHistoryEntry("All", serverType, "In SetO365ServerSettings: 1" + row["Tests"].ToString(), Common.LogLevel.Normal);

					switch (row["Tests"].ToString())
					{
						case "Mail Flow Test":
							MyO365Server.EnableMailFlow = Convert.ToBoolean(row["EnableSimulationTests"].ToString());
							MyO365Server.MailFlowThreshold = Convert.ToInt32(row["ResponseThreshold"].ToString());
							break;
						case "Inbox":
							MyO365Server.EnableInboxTest = Convert.ToBoolean(row["EnableSimulationTests"].ToString());
							MyO365Server.InboxThreshold = Convert.ToInt32(row["ResponseThreshold"].ToString());
							break;
						case "OWA":
							MyO365Server.EnableOWATest = Convert.ToBoolean(row["EnableSimulationTests"].ToString());
							MyO365Server.ComposeEmailThreshold = Convert.ToInt32(row["ResponseThreshold"].ToString());
							break;
						case "SMTP":
							MyO365Server.EnableSMTPTest = Convert.ToBoolean(row["EnableSimulationTests"].ToString());
							//MyO365Server.ComposeEmailThreshold = Convert.ToInt32(row["ResponseThreshold"].ToString());
							break;
						case "POP":
							MyO365Server.EnablePOPTest = Convert.ToBoolean(row["EnableSimulationTests"].ToString());
							break;
						case "IMAP":
							MyO365Server.EnableIMAPTest = Convert.ToBoolean(row["EnableSimulationTests"].ToString());
							break;
						case "Auto Discovery":
							MyO365Server.EnableAutoDiscoveryTest = Convert.ToBoolean(row["EnableSimulationTests"].ToString());
							break;
						case "MAPI Connectivity":
							MyO365Server.EnableMAPIConnectivityTest = Convert.ToBoolean(row["EnableSimulationTests"].ToString());
							break;
						case "Create Task":
							MyO365Server.EnableCreateTaskTest = Convert.ToBoolean(row["EnableSimulationTests"].ToString());
							MyO365Server.CreateTaskThreshold = Convert.ToInt32(row["ResponseThreshold"].ToString());
							break;
						case "Create Folder Test":
							MyO365Server.EnableCreateFolderTest = Convert.ToBoolean(row["EnableSimulationTests"].ToString());
							MyO365Server.CreateFolderThreshold = Convert.ToInt32(row["ResponseThreshold"].ToString());
							break;
						case "OneDrive Upload Test":
							MyO365Server.EnableOneDriveUploadTest = Convert.ToBoolean(row["EnableSimulationTests"].ToString());
							MyO365Server.OneDriveUplaodThreshold = Convert.ToInt32(row["ResponseThreshold"].ToString());
							break;
						case "OneDrive Download Test":
							MyO365Server.EnableOneDriveDownloadTest = Convert.ToBoolean(row["EnableSimulationTests"].ToString());
							MyO365Server.OneDriveDownlaodThreshold = Convert.ToInt32(row["ResponseThreshold"].ToString());
							break;
						//case "OneDrive Search":
						//    MyO365Server.EnableOneDriveSearchTest = Convert.ToBoolean(row["EnableSimulationTests"].ToString());
						//    MyO365Server.OneDriveSearchThreshold = Convert.ToInt32(row["ResponseThreshold"].ToString());
						//    break;
						case "Create Site Test":
							MyO365Server.EnableCreateSiteTest = Convert.ToBoolean(row["EnableSimulationTests"].ToString());
							MyO365Server.CreateSiteThreshold = Convert.ToInt32(row["ResponseThreshold"].ToString());
							break;
						case "Create Calendar":
							MyO365Server.EnableCreateCalEntryTest = Convert.ToBoolean(row["EnableSimulationTests"].ToString());
							MyO365Server.CreateCalEntryThreshold = Convert.ToInt32(row["ResponseThreshold"].ToString());
							break;
						//case "Resolve User":
						//    MyO365Server.EnableResolveUserTest = Convert.ToBoolean(row["EnableSimulationTests"].ToString());
						//    MyO365Server.ResolveUserThreshold = Convert.ToInt32(row["ResponseThreshold"].ToString());
						//    break;
						case "Dir Sync Imp/Export Test":
							MyO365Server.DirSyncExportTest = Convert.ToBoolean(row["EnableSimulationTests"].ToString());
							MyO365Server.DirSyncExportThreshold = Convert.ToInt32(row["ResponseThreshold"].ToString());
							MyO365Server.DirSyncImportTest = Convert.ToBoolean(row["EnableSimulationTests"].ToString());
							MyO365Server.DirSyncImportThreshold = Convert.ToInt32(row["ResponseThreshold"].ToString());
							break;
					}
				}

			}
			catch(Exception ex)
			{
				Common.WriteDeviceHistoryEntry("All", serverType, "Error in SetO365ServerSettings: " + ex.ToString(), Common.LogLevel.Normal);
			}
			return MyO365Server;
		}

		protected void InitStatusTable(MonitoredItems.Office365ServersCollection collection)
		{
			try
			{
				String SqlStr = "";
				String type = "";
				//string Location = "";
				CommonDB db = new CommonDB();
				if (collection.Count > 0)
					type = collection.get_Item(0).ServerType;
				//Location = collection.get_Item(0).Location;

				if (type != "")
				{
					//SqlStr = "DELETE FROM Status WHERE Type='" + type + "' AND CATEGORY='" + NodeName +"'";
					//db.Execute(SqlStr);

					//SqlStr = " INSERT INTO Status ( Type, Location, Category, Name, Status, Details, Description, TypeANDName, StatusCode ) VALUES ";
					//int i = 0;
					foreach (MonitoredItems.Office365Server server in collection)
					{
						if (server.Location != "")
						{
						 SqlStr = "IF NOT EXISTS(SELECT * FROM Status WHERE TypeANDName = '" + server.Name + "-" +server.Location + "'  AND CATEGORY='" + NodeName + "') BEGIN " +
								"INSERT INTO Status ( Type, Location, Category, Name, Status, Details, Description, TypeANDName, StatusCode ) VALUES " +
								" ('" + type + "', '" + server.Location + "', '" + server.Category + "', '" + server.Name + "', '" + server.Status + "', 'This server has not yet been scanned.', " +
								"'Microsoft " + type + " Server', '" + server.Name + "-" + server.Location + "', '" + server.StatusCode + "')End";
						 db.Execute(SqlStr);
						//we do not want to insert a row without a location
						
							//i += 1;
							//SqlStr += " ('" + type + "', '" + server.Location + "', '" + server.Category + "', '" + server.Name + "', '" + server.Status + "', 'This server has not yet been scanned.', 'Microsoft " + type + " Server', '" +
								//server.Name + "-" + server.Location + "', '" + server.StatusCode + "') END,";
						}
					}
					
					//if (i > 0)
					//{
						//Remove the last comma
						//SqlStr = SqlStr.Remove(SqlStr.Length - 1);
					//}
					//db.Execute(SqlStr);
					Common.WriteDeviceHistoryEntry("All", serverType , type + " Servers are marked as Not Scanned", Common.LogLevel.Normal);

				}
			}
			catch (Exception ex)
			{
				Common.WriteDeviceHistoryEntry("All", serverType, "Error in init status.  Error: " + ex.Message, Common.LogLevel.Normal);
			}

		}

		#region DailyTasks

		private void DailyTasks()
		{
			MonitoredItems.Office365Server DummyServerForLogs = new MonitoredItems.Office365Server() { Name = "DailyTasks" };
			Thread DailyTasksThread = null;
			int Day = -1;
			while (true)
			{
				if (Day == -1 || Day != DateTime.Now.Day)
				{
					if (DailyTasksThread != null && DailyTasksThread.IsAlive)
					{
						Common.WriteDeviceHistoryEntry(serverType, DummyServerForLogs.Name, "The thread for Daily Tasks got hung up and will be killed to start the next cycle.", commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);
						DailyTasksThread.Abort();
					}

					DailyTasksThread = new Thread(() => DailyTasksMainThread(DummyServerForLogs));
					DailyTasksThread.CurrentCulture = c;
					DailyTasksThread.IsBackground = true;
					DailyTasksThread.Priority = ThreadPriority.Normal;
					DailyTasksThread.Name = "DailyTaskWorkerThread - O365";
					DailyTasksThread.Start();
					//Thread.Sleep(60 * 60 * 1000); //sleeps for 1 hour
					Day = DateTime.Now.Day;
				}
				//sleep for 5 mins
				Thread.Sleep(1000 * 60 * 5);//soma

			}
		}


		#endregion

		#region HourlyTasks


		private void HourlyTasks()
		{
			MonitoredItems.Office365Server DummyServerForLogs = new MonitoredItems.Office365Server() { Name = "HourlyTasks" };
			Thread HourlyTasksThread = null;
			int hour = -1;
			while (true)
			{
				if (hour == -1 || hour != DateTime.Now.Hour)
				{
					if (HourlyTasksThread != null && HourlyTasksThread.IsAlive)
					{
						Common.WriteDeviceHistoryEntry(serverType, DummyServerForLogs.Name, "The thread for Horly Tasks got hung up and will be killed to start the next cycle.", commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);
						HourlyTasksThread.Abort();
					}

					HourlyTasksThread = new Thread(() => HourlyTasksMainThread(DummyServerForLogs));
					HourlyTasksThread.CurrentCulture = c;
					HourlyTasksThread.IsBackground = true;
					HourlyTasksThread.Priority = ThreadPriority.Normal;
					HourlyTasksThread.Name = "HourlyTaskWorkerThread - O365";
					HourlyTasksThread.Start();
					//Thread.Sleep(60 * 60 * 1000); //sleeps for 1 hour
					hour = DateTime.Now.Hour;
				}
				//sleep for 5 mins
				Thread.Sleep(1000 * 60 * 5);

			}
		}

		private void HourlyTasksMainThread(MonitoredItems.Office365Server DummyServerForLogs)
		{

			Common.WriteDeviceHistoryEntry("All", serverType, " Hourly Task started.", Common.LogLevel.Normal);
			MonitoredItems.Office365Server testServer = null;
			try
			{
				if (myOffice365Servers != null)
					foreach (MonitoredItems.Office365Server server in myOffice365Servers)
					{
						if (server.Status != "Not Responding" && server.Status != "Maintenance" && server.Enabled)
						{

							testServer = server;
							Common.WriteDeviceHistoryEntry(testServer.ServerType, testServer.Name, " Hourly Task starting.", Common.LogLevel.Normal);
						}
					}
			}
			catch
			{
			}
			try
			{
				if (testServer != null)
				{


					TestResults AllTestResults = new TestResults();

					Office365Common Office365Common = new Office365Common();
					bool isResponding = true;
					ReturnPowerShellObjects results = Office365Common.testO365ServerConnectivity(testServer, ref AllTestResults, ref isResponding);

					using (results)
					{
						Common.WriteDeviceHistoryEntry(testServer.ServerType, testServer.Name, " Hourly Task started.", Common.LogLevel.Normal);
						Office365Common.getMobileUsersHourly(testServer, ref AllTestResults, results);
						Office365Common.getUserswithLicencesandServices(testServer, ref AllTestResults, results);
						//Office365Common.Deletesummarystatsdata(testServer, ref AllTestResults, testServer.ServerType);
						//Common.CommonDailyTasks(testServer, ref AllTestResults, testServer.ServerType);
						//Office365Common.getMailBoxInfo(testServer, ref AllTestResults, testServer.VersionNo.ToString(), DummyServerForLogs.Name, myOffice365Servers, results);

					}

					GC.Collect();
					while (testServer.IsBeingScanned)
					{
						string doSomething;
					}
					
					CommonDB DB = new CommonDB();
					DB.UpdateSQLStatements(AllTestResults, DummyServerForLogs);
					Common.WriteDeviceHistoryEntry(testServer.ServerType, testServer.Name, " Hourly Task Ended.", Common.LogLevel.Normal);
				}
				else
				{
					//no server was found to be used to scan
					Common.WriteDeviceHistoryEntry(serverType, "All", " Hourly Task No server found to scan ", Common.LogLevel.Normal);
				}
			}
			catch (Exception ex)
			{
				Common.WriteDeviceHistoryEntry(serverType, "All", " Hourly Task Ended with an error: " + ex.Message.ToString(), Common.LogLevel.Normal);
			}

		}
		private void DailyTasksMainThread(MonitoredItems.Office365Server DummyServerForLogs)
		{

			Common.WriteDeviceHistoryEntry("All", serverType, " Daily Task started.", Common.LogLevel.Normal);
			MonitoredItems.Office365Server testServer = null;
			try
			{
				if (myOffice365Servers != null)
					foreach (MonitoredItems.Office365Server server in myOffice365Servers)
					{
						if (server.Status != "Not Responding" && server.Status != "Maintenance" && server.Enabled)
						{

							testServer = server;
							Common.WriteDeviceHistoryEntry(testServer.ServerType, testServer.Name, " Daily Task starting.", Common.LogLevel.Normal);
						}
					}
			}
			catch
			{
			}
			try
			{
				if (testServer != null)
				{

					
					TestResults AllTestResults = new TestResults();

					Office365Common Office365Common = new Office365Common();
					bool isResponding = true;
					ReturnPowerShellObjects results = Office365Common.testO365ServerConnectivity(testServer, ref AllTestResults, ref isResponding);

					using (results)
					{

						Common.WriteDeviceHistoryEntry(testServer.ServerType, testServer.Name, " Daily Task started.", Common.LogLevel.Normal);
						//Office365Common.getMailBoxInfo(testServer, ref AllTestResults, results);
						Office365Common.getMsolUsers(testServer, ref AllTestResults, results);
						Office365Common.getMsolGroups(testServer, ref AllTestResults, results);
						Office365Common.getMailboxes(testServer, ref AllTestResults, results);
						Office365Common.getMailStatusInfo(testServer, ref AllTestResults, results);
						Office365Common.getServiceStatus(testServer, ref AllTestResults, results);
					//	Office365Common.getUserswithLicencesandServices(testServer, ref AllTestResults, results);



					}

					GC.Collect();
					//while (testServer.IsBeingScanned)
					//{
					//    string doSomething;
					//}
					//let the main thread sql get executed. 2 mins should be enough.
					//Thread.Sleep(120 * 1000);
					CommonDB DB = new CommonDB();
					DB.UpdateSQLStatements(AllTestResults, DummyServerForLogs);

					Common.WriteDeviceHistoryEntry(testServer.ServerType, testServer.Name, " Daily Task Ended.", Common.LogLevel.Normal);
					AllTestResults = new TestResults();
					
					Office365Common.Deletesummarystatsdata(testServer, ref AllTestResults, testServer.ServerType);
					Office365Common.doSummaryStats(testServer, ref AllTestResults, results);
                    Common.CommonDailyTasks(testServer, ref AllTestResults, testServer.ServerType);
					DB.UpdateSQLStatements(AllTestResults, DummyServerForLogs);

				}
				else
				{
					//no server was found to be used to scan
					Common.WriteDeviceHistoryEntry(serverType, "All", " Daily Task No server found to scan ", Common.LogLevel.Normal);
				}
			}
			catch (Exception ex)
			{
				Common.WriteDeviceHistoryEntry(serverType, "All", " Daily Task Ended with an error: " + ex.Message.ToString(), Common.LogLevel.Normal);
			}

		}



		#endregion

		public void RefreshOffice365Collction()
		{
			CreateOffice365ServersCollection();
			StartO365Threads();
		}

	}
}
