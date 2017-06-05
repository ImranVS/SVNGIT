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

namespace VitalSignsMicrosoftClasses
{
	public class WindowsMAIN
	{
		string CurrentCulture = "en-US";
		string CultureStringName = "CultureString";
		CultureInfo c;
		MonitoredItems.MicrosoftServersCollection myWindowsServers;
		List<MonitoredItems.MicrosoftServersCollection> ListOfCollectionsForLatencyTest;

		string serverType = "Windows";
		public void StartProcess(dynamic MicrosoftHelperObj)
		{
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
				Common.WriteDeviceHistoryEntry("All", serverType, "Windows Service is starting up", Common.LogLevel.Normal);
				//Sets the log level
				Common.setLogLevel();

				try
				{
					VSFramework.RegistryHandler mySettigns = new VSFramework.RegistryHandler();
					mySettigns.WriteToRegistry("VS " + serverType + " Service Start", DateTime.Now.ToString());
				}
				catch
				{
					Common.WriteDeviceHistoryEntry("All", serverType, "Error updating start time for Service in settings table", Common.LogLevel.Normal);
				}

				//creates colelction and starts thread to monitor changes for the colelction
				CommonDB db = new CommonDB();

				if (db.RecordExists("SELECT * FROM SelectedFeatures sf WHERE sf.FeatureID in (SELECT ID FROM Features WHERE Name='" + serverType + "')"))
				{
					Common.WriteDeviceHistoryEntry("All", serverType, "Server is marked for scanning so will start Server Related Tasks", Common.LogLevel.Normal);
					CreateWindowsServersCollection();
					Common.InitStatusTable(myWindowsServers);
					StartWindowsThreads();

					Thread.Sleep(60 * 1000 * 1);

					Thread HourlyTasksThread = new Thread(new ThreadStart(HourlyTasks));
					HourlyTasksThread.CurrentCulture = c;
					HourlyTasksThread.IsBackground = true;
					HourlyTasksThread.Name = "HourlyTasks - WIN";
					HourlyTasksThread.Priority = ThreadPriority.Normal;
					HourlyTasksThread.Start();
					Thread.Sleep(2000);

					/*
					Thread DailyTasksThread = new Thread(new ThreadStart(DailyTasks));
					DailyTasksThread.CurrentCulture = c;
					DailyTasksThread.IsBackground = true;
					DailyTasksThread.Priority = ThreadPriority.Normal;
					DailyTasksThread.Name = "DailyTasks - WIN";
					DailyTasksThread.Start();
					Thread.Sleep(2000);
					*/
				}
				else
				{
					myWindowsServers = null;
					Common.WriteDeviceHistoryEntry("All", serverType, "Server is not marked for scanning", Common.LogLevel.Normal);
				}

				Thread LatencyTestThread = new Thread(new ThreadStart(ServerLatencyTest));
				LatencyTestThread.CurrentCulture = c;
				LatencyTestThread.IsBackground = true;
				LatencyTestThread.Name = "Latency Test";
				LatencyTestThread.Priority = ThreadPriority.Normal;
				LatencyTestThread.Start();
				Thread.Sleep(2000);


				//sleep for one minute to allow time for the collection to be made 


				Common.WriteDeviceHistoryEntry("All", serverType, "All Processes are started in startProcess", Common.LogLevel.Normal);
			}
			catch (Exception ex)
			{
				Common.WriteDeviceHistoryEntry("All", serverType, "Error starting StartProcess exception CreateServersCollection: " + ex.StackTrace.ToString(), Common.LogLevel.Normal);
				throw ex;

			}

		}
		int serverThreadCount = 0;
		int initialServerThreadCount = 0;
		System.Collections.ArrayList AliveServerMainThreads = new System.Collections.ArrayList();
		private void StartWindowsThreads()
		{
			int maxThreadCount = Common.getThreadCount("Windows");
			int startThreads = 0;
			serverThreadCount = myWindowsServers.Count / 3;
			if (serverThreadCount <= 1)
				if (myWindowsServers.Count > 1)
					serverThreadCount = 2;
				else
					serverThreadCount = 1;
			if (serverThreadCount <= 1 && myWindowsServers.Count > 1)
				serverThreadCount = 2;

			// 5/19/15 WS commented out.  VSPLUS 1776
			if (serverThreadCount > maxThreadCount)
				serverThreadCount = maxThreadCount;
			//serverThreadCount = 1;
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

			Common.WriteDeviceHistoryEntry("All", serverType, "There are " + serverThreadCount + " threads open", Common.LogLevel.Normal);
			if (c == null)
				c = new CultureInfo("en-US");
			for (int i = startThreads; i < serverThreadCount; i++)
			{
				//workingThread = new Thread(() => RoleMonitoring(ClassName, results, AllTestResults, thisServer, ref AlivePSO));
				Thread monitorAD = new Thread(() => MonitorWindows(i));
				monitorAD.CurrentCulture = c == null ? new CultureInfo("en-US") : c;  //Should only be null on our local copies if using wrapper
				monitorAD.IsBackground = true;
				monitorAD.Priority = ThreadPriority.Normal;
				monitorAD.Name = i.ToString() + "-Windows WIN";
				AliveServerMainThreads.Add(monitorAD);
				monitorAD.Start();
				Thread.Sleep(2000);
			}


		}
		public MonitoredItems.MicrosoftServer SelectServerToMonitor()
		{

			DateTime tNow = DateTime.Now;
			DateTime tScheduled;

			DateTime timeOne;
			DateTime timeTwo;

			MonitoredItems.MicrosoftServer SelectedServer = null;

			MonitoredItems.MicrosoftServer ServerOne = null;
			MonitoredItems.MicrosoftServer ServerTwo = null;

			RegistryHandler myRegistry = new RegistryHandler();

			String ScanASAP = "";
			String strSQL = "";
			String ServerType = "Windows";
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

					for (int n = 0; n < myWindowsServers.Count; n++)
					{
						ServerOne = myWindowsServers.get_Item(n);
						if (ServerOne.Name == serverName && ServerOne.IsBeingScanned == false && ServerOne.Enabled)
						{
							Common.WriteDeviceHistoryEntry("All", serverType, serverName + " was marked 'Scan ASAP' so it will be scanned next.");

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


			try
			{
				ScanASAP = myRegistry.ReadFromRegistry("ScanWindowsASAP").ToString();
			}
			catch (Exception ex)
			{
				ScanASAP = "";
			}


			//Searches for the server marked as ScanASAP, if it exists
			for (int n = 0; n < myWindowsServers.Count; n++)
			{
				ServerOne = myWindowsServers.get_Item(n);
				if (ServerOne.Name == ScanASAP && ServerOne.IsBeingScanned == false && ServerOne.Enabled)
				{
					Common.WriteDeviceHistoryEntry("All", serverType, ScanASAP + " was marked 'Scan ASAP' so it will be scanned next.");
					myRegistry.WriteToRegistry("ScanWindowsASAP", "n/a");

					//ServerOne.ScanASAP = true;

					return ServerOne;
				}

			}


			//Searches for the first enounter of a Not Responding server that is due for a scan
			for (int n = 0; n < myWindowsServers.Count; n++)
			{
				ServerOne = myWindowsServers.get_Item(n);
				if ((ServerOne.Status == "Not Responding" || ServerOne.Status == "Master Service Stopped.") && ServerOne.IsBeingScanned == false && ServerOne.Enabled)
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
			for (int n = 0; n < myWindowsServers.Count; n++)
			{
				ServerOne = myWindowsServers.get_Item(n);
				if (ServerOne.Status == "Not Scanned" && ServerOne.IsBeingScanned == false && ServerOne.Enabled)
				{
					Common.WriteDeviceHistoryEntry("All", serverType, "Selecting " + ServerOne.Name + " because the status is " + ServerOne.Status + ".");
					return ServerOne;
				}
			}


			//Searches for all servers that are due for a scan
			List<MonitoredItems.MicrosoftServer> ScanCanidates = new List<MonitoredItems.MicrosoftServer>();

			foreach (MonitoredItems.MicrosoftServer srv in myWindowsServers)
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

			return SelectedServer;
		}
		private void MonitorWindows(int threadNum)
		{
			Thread.CurrentThread.CurrentCulture = c;
			CommonDB DB = new CommonDB();
			while (true)
			{

				MonitoredItems.MicrosoftServer thisServer = SelectServerToMonitor();
				try
				{
					if (thisServer != null && !thisServer.IsBeingScanned)
					{
						Common.WriteDeviceHistoryEntry("All", serverType, "Scanning Server " + thisServer.Name + " on thread " + threadNum);
						thisServer.IsBeingScanned = true;

						MaintenanceDll maintenance = new MaintenanceDll();
						if (maintenance.InMaintenance(thisServer.ServerType, thisServer.Name))
						{
							Common.ServerInMaintenance(thisServer);
							goto CleanUp;
						}

						if (thisServer.StatusCode == "Maintenance")
						{
							thisServer.FastScan = true;
							Common.WriteDeviceHistoryEntry(thisServer.ServerType, thisServer.Name, "Doing a fast scan sicne it is in maintenance", Common.LogLevel.Normal);
						}
                        
						TestResults AllTestResults = new TestResults();
						ReturnPowerShellObjects PSO = null;
                        Common.SetupServer(thisServer, thisServer.ServerType, AllTestResults);
                        bool notResponding = true;
						
						using (PSO = Common.TestRepsonding(thisServer, ref notResponding, ref AllTestResults))
						{
							if (!notResponding)
							{

								MicrosoftCommon MSCommon = new MicrosoftCommon();
								MSCommon.PrereqForWindows(thisServer, AllTestResults, PSO);

								DB.UpdateAllTests(AllTestResults, thisServer, thisServer.ServerType);
							}
							else
							{
								//Common.WriteDeviceHistoryEntry(thisServer.ServerType, thisServer.Name, "All threads are closed, starting Not Responding update scripts", Common.LogLevel.Normal);
								//DB.NotRespondingQueries(thisServer, thisServer.ServerType);
							}
						}

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
							Common.WriteDeviceHistoryEntry("ALL", serverType, "Server " + thisServer.Name + " is already being scanned and will not start scanning again");
						}
						catch (Exception ex)
						{
							Common.WriteDeviceHistoryEntry("ALL", serverType, "Server returned as null");
						}
					}


					Common.WriteDeviceHistoryEntry("All", serverType, "Waiting for 5 seconds to restart the Loop ");
					// Sleep for 3 minutes 
					Thread.Sleep(1000 * 5);
					//break;
				}
				catch (Exception ex)
				{
					Common.WriteDeviceHistoryEntry(thisServer.ServerType, thisServer.Name, "Error in MonitorWindows: " + ex.Message, commonEnums.ServerRoles.Windows, Common.LogLevel.Normal);
				}
			}

		}


		
		private void CreateWindowsServersCollection()
		{
			//Fetch all servers
			if (myWindowsServers == null)
				myWindowsServers = new MonitoredItems.MicrosoftServersCollection();

			CommonDB DB = new CommonDB();
			StringBuilder SQL = new StringBuilder();
			SQL.Append(" select distinct Sr.ID,Sr.ServerName,S.ServerType, S.ID as ServerTypeId,L.Location,sa.ScanInterval,sa.RetryInterval,sa.OffHourInterval,sa.Enabled,sr.ipaddress,sa.category,cr.UserID,cr.Password,sa.ResponseTime, ");
			SQL.Append("  st.LastUpdate, st.Status, st.StatusCode, di.CurrentNodeID, sa.CPU_Threshold, sa.MemThreshold  ");
			SQL.Append("  from Servers Sr ");
			SQL.Append(" inner join ServerTypes S on Sr.ServerTypeID=S.ID  inner join Locations L on Sr.LocationID =L.ID  left outer join ServerAttributes sa on sr.ID=sa.serverid ");
			SQL.Append(" inner join credentials cr on sa.CredentialsId=cr.ID ");


			if (ConfigurationManager.AppSettings["VSNodeName"] != null)
			{
				string NodeName = ConfigurationManager.AppSettings["VSNodeName"].ToString();
				SQL.Append(" inner join DeviceInventory di on Sr.ID=di.DeviceID and Sr.ServerTypeId=di.DeviceTypeId ");
				SQL.Append(" inner join Nodes on (Nodes.ID=di.CurrentNodeId or di.CurrentNodeId=-1) and Nodes.Name='" + NodeName + "' ");
			}
			SQL.Append(" left outer join Status st on st.Type=S.ServerType and st.Name=Sr.ServerName ");

			SQL.Append(" where S.ServerType='" + serverType + "' and sa.Enabled = 1 order by sr.id");
			DataTable dtServers = DB.GetData(SQL.ToString());
			//Loop through servers
			if (dtServers.Rows.Count > 0)
			{
				List<string> ServerNameList = new List<string>();

				int updatedServers = 0;
				int newServers = 0;
				int removedServers = 0;

				for (int i = 0; i < dtServers.Rows.Count; i++)
				{
					DataRow DR = dtServers.Rows[i];
					ServerNameList.Add(DR["ServerName"].ToString());

					MonitoredItems.MicrosoftServer myServer = null;

					//Checks to see if the server is newly added or exists.  Adds if it is new
					try
					{
						myServer = myWindowsServers.SearchByName(DR["ServerName"].ToString());
						if (myServer == null)
						{
							//New server.  Set inits and add to collection

							myServer = InitForWindowsServers(myServer, DR);
							myWindowsServers.Add(myServer);
							newServers++;

						}
						else
						{
							updatedServers++;
						}
					}
					catch (Exception ex)
					{
						//New server.  Set inits and add to collection
						myServer = InitForWindowsServers(myServer, DR);
						myWindowsServers.Add(myServer);
						newServers++;
					}

					myServer = SetWindowsServerSettings(myServer, DR);


				}



				//Removes servers not in the new lsit
				foreach (MonitoredItems.MicrosoftServer server in myWindowsServers)
				{
					string currName = server.Name;
					try
					{
						if (!ServerNameList.Contains(currName))
						{
							//incase it doesnt throw and exception, if not found, removed server from list
							myWindowsServers.Delete(currName);
							removedServers++;

						}
					}
					catch (Exception ex)
					{
						//server not found
						myWindowsServers.Delete(currName);
						removedServers++;

					}

				}


				//myExchangeServers = newCollection;
				/**********************************************************/
				Common.WriteDeviceHistoryEntry("All", serverType, "There are " + myWindowsServers.Count + " servers in the collection.  " + newServers + " were new and " + updatedServers + " were updated.");
			}
			else
			{
				myWindowsServers = new MonitoredItems.MicrosoftServersCollection();
			}

			Common.InsertInsufficentLicenses(myWindowsServers);
			//At this point we have all Servers with ALL the information(including Threshold settings)
		}

		private MonitoredItems.MicrosoftServer SetWindowsServerSettings(MonitoredItems.MicrosoftServer myServer, DataRow DR)
		{
			myServer.IPAddress = DR["IPAddress"].ToString();
			myServer.Name = DR["ServerName"].ToString();
			myServer.UserName = DR["UserID"].ToString();
			myServer.Password = Common.decodePasswordFromEncodedString(DR["Password"].ToString(), myServer.Name);
			myServer.Location = DR["Location"].ToString();
			myServer.ResponseThreshold = long.Parse(DR["ResponseTime"].ToString());
			myServer.ScanInterval = int.Parse(DR["ScanInterval"].ToString());
			myServer.OffHoursScanInterval = int.Parse(DR["OffHourInterval"].ToString());
			myServer.RetryInterval = int.Parse(DR["RetryInterval"].ToString());
			myServer.CPU_Threshold = int.Parse(DR["CPU_Threshold"].ToString());
			myServer.Memory_Threshold = int.Parse(DR["MemThreshold"].ToString());
			myServer.Category = DR["Category"].ToString();
			myServer.ServerId = DR["ID"].ToString();
			myServer.InsufficentLicenses = DR["CurrentNodeID"] != null && DR["CurrentNodeID"].ToString() == "-1" ? true : false;

			return myServer;
		}

		private MonitoredItems.MicrosoftServer InitForWindowsServers(MonitoredItems.MicrosoftServer myServer, DataRow DR)
		{
			myServer = new MonitoredItems.MicrosoftServer();


			myServer.LastScan = DR["LastUpdate"] == null || DR["LastUpdate"].ToString() == "" ? DateTime.Now : DateTime.Parse(DR["LastUpdate"].ToString());
			myServer.Status = DR["Status"] == null || DR["Status"].ToString() == "" ? "Not Scanned" : DR["Status"].ToString();
			myServer.StatusCode = DR["StatusCode"] == null || DR["StatusCode"].ToString() == "" ? "Maintenance" : DR["StatusCode"].ToString();
			myServer.ServerType = DR["ServerType"].ToString();
			myServer.ServerTypeId = int.Parse(DR["ServerTypeId"].ToString());
			myServer.Enabled = true;

			return myServer;
		}

		#region HourlyDailyTasks


		private void HourlyTasks()
		{
			MonitoredItems.MicrosoftServer DummyServerForLogsHourly = new MonitoredItems.MicrosoftServer() { Name = "HourlyTask", ServerType = serverType };
			Thread HourlyTasksThread = null;
			int Hour = -1;

			MonitoredItems.MicrosoftServer DummyServerForLogsDaily = new MonitoredItems.MicrosoftServer() { Name = "HourlyTask", ServerType = serverType };
			Thread DailyTasksThread = null;
			int Day = -1;

			while (true)
			{
				try
				{

					if (Hour == -1 || Hour != DateTime.Now.Hour)
					{
						if (HourlyTasksThread != null && HourlyTasksThread.IsAlive)
						{
							Common.WriteDeviceHistoryEntry(DummyServerForLogsHourly.ServerType, DummyServerForLogsHourly.Name, "The thread for Hourly Tasks got hung up and will be killed to start the next cycle.", commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);
							HourlyTasksThread.Abort();
						}

						HourlyTasksThread = new Thread(() => HourlyTasksMainThread(DummyServerForLogsHourly));
						HourlyTasksThread.CurrentCulture = c;
						HourlyTasksThread.IsBackground = true;
						HourlyTasksThread.Priority = ThreadPriority.Normal;
						HourlyTasksThread.Name = "HourlyTaskWorkerThread - WIN";
						HourlyTasksThread.Start();
						//Thread.Sleep(60 * 60 * 1000); //sleeps for 1 hour
						Hour = DateTime.Now.Hour;
					}

					if (Day == -1 || Day != DateTime.Now.Day)
					{
						if (DailyTasksThread != null && DailyTasksThread.IsAlive)
						{
							Common.WriteDeviceHistoryEntry(DummyServerForLogsDaily.ServerType, DummyServerForLogsDaily.Name, "The thread for Daily Tasks got hung up and will be killed to start the next cycle.", commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);
							DailyTasksThread.Abort();
						}

						DailyTasksThread = new Thread(() => DailyTasksMainThread(DummyServerForLogsDaily));
						DailyTasksThread.CurrentCulture = c;
						DailyTasksThread.IsBackground = true;
						DailyTasksThread.Priority = ThreadPriority.Normal;
						DailyTasksThread.Name = "DailyTaskWorkerThread - WIN";
						DailyTasksThread.Start();

						Day = DateTime.Now.Day;
					}
				}
				catch (Exception ex)
				{
					Common.WriteDeviceHistoryEntry(DummyServerForLogsDaily.ServerType, DummyServerForLogsDaily.Name, "The thread for Daily and Hourly tasks had an error: " + ex.Message, commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);
				}
				//sleep for 5 mins
				Thread.Sleep(1000 * 60 * 5);

			}
		}

		private void HourlyTasksMainThread(MonitoredItems.MicrosoftServer DummyServerForLogs)
		{


			Common.WriteDeviceHistoryEntry("All", DummyServerForLogs.ServerType, "Time to do Hourly Tasks for " + DummyServerForLogs.ServerType + ".", commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);
			try
			{
				try
				{
					TestResults testResults = new TestResults();

					if (myWindowsServers != null)
						foreach (MonitoredItems.SharepointServer server in myWindowsServers)
						{
							server.OffHours = Common.OffHours(server.Name);
							Common.RecordUpAndDownTimes(server, ref testResults);
						}

					CommonDB db = new CommonDB();

					db.UpdateSQLStatements(testResults, DummyServerForLogs);

				}
				catch (Exception ex)
				{
					Common.WriteDeviceHistoryEntry("All", DummyServerForLogs.ServerType, "Error setting OffHours.  Error: " + ex.Message, commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);
				}

			}
			catch (Exception ex) 
			{
				Common.WriteDeviceHistoryEntry("All", DummyServerForLogs.ServerType, "Error in Hourly Thread.  Error: " + ex.Message, commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);
			}



		}

		private void DailyTasksMainThread(MonitoredItems.MicrosoftServer DummyServerForLogs)
		{


			Common.WriteDeviceHistoryEntry("All", DummyServerForLogs.ServerType, "Time to do Daily Tasks for " + DummyServerForLogs.ServerType + ".", commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);

			try
			{
				


			}
			catch (Exception ex) { }
		}

		#endregion


		#region LatencyTest

		public void initLatencyCollection()
		{
			String sql = "select NL.TestName, NL.ScanInterval, NL.TestDuration, NLS.LatencyRedThreshold, svrs.ServerName, svrs.ID, svrs.IPAddress, creds.UserID, creds.Password " +
						"from NetworkLatency NL " +
						"inner join NetworkLatencyServers NLS on NL.NetworkLatencyId = NLS.NetworkLatencyID and NL.Enable=1 and NLS.Enabled=1 " +
						"inner join Servers svrs on NLS.ServerID = svrs.ID " +
						"inner join ServerAttributes svratt on svratt.ServerId = svrs.ID " +
						"inner join Credentials creds on creds.ID = svratt.CredentialsId";
			if (ConfigurationManager.AppSettings["VSNodeName"] != null)
			{
				string NodeName = ConfigurationManager.AppSettings["VSNodeName"].ToString();
				sql += " inner join DeviceInventory di on NLS.NetworkLatencyID=di.DeviceID and di.ServerTypeId=(select ID from ServerTypes where ServerType='Network Latency') ";
				sql += " inner join Nodes on Nodes.ID=di.CurrentNodeId and Nodes.Name='" + NodeName + "' ";
			}

			CommonDB db = new CommonDB();

			DataTable dt = db.GetData(sql);

			MonitoredItems.MicrosoftServersCollection coll = new MonitoredItems.MicrosoftServersCollection();
			foreach (DataRow row in dt.Rows)
			{
				MonitoredItems.MicrosoftServer server = new MonitoredItems.MicrosoftServer();

				server.ScanInterval = Convert.ToInt32(row["ScanInterval"].ToString());
				server.IPAddress = row["IPAddress"].ToString();
				server.Name = row["ServerName"].ToString();
				server.Password = Common.decodePasswordFromEncodedString(row["Password"].ToString(), row["ServerName"].ToString());
				server.UserName = row["UserID"].ToString();
				server.ServerId = row["ID"].ToString();

				//USING DESCRIPTION TO HOLD TEST NAME
				server.Description = row["TestName"].ToString();
				//USING RETRYINTERVAL FOR TEST DURIATION
				server.RetryInterval = Convert.ToInt32(row["TestDuration"].ToString());
				coll.Add(server);
			}


			ListOfCollectionsForLatencyTest = new List<MonitoredItems.MicrosoftServersCollection>();
			List<string> ListOfTestNames = dt.AsEnumerable().Select(s => s.Field<string>("TestName")).Distinct().ToList();

			foreach (string TestName in ListOfTestNames)
			{
				MonitoredItems.MicrosoftServersCollection tempColl = new MonitoredItems.MicrosoftServersCollection();
				foreach (MonitoredItems.MicrosoftServer server in coll)
				{
					if (server.Description == TestName)
					{
						tempColl.Add(server);
					}
				}
				ListOfCollectionsForLatencyTest.Add(tempColl);
			}
		}

		public void ServerLatencyTest()
		{
			Common.WriteDeviceHistoryEntry("LatencyTest", "LatencyTest", "Starting Latency Test", commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);

			initLatencyCollection();

			//A list of collections of servers based off their TestName is now made

			while (true)
			{
				foreach (MonitoredItems.MicrosoftServersCollection currColl in ListOfCollectionsForLatencyTest)
				{
					MonitoredItems.MicrosoftServer server = currColl.get_Item(0);
					if (server.IsBeingScanned)
						continue;
					//if was never scaned or it is due for a scan
					DateTime d = server.LastScan.AddMinutes(server.ScanInterval);

					int i = server.LastScan.AddMinutes(server.ScanInterval).CompareTo(DateTime.Now);
					if ((server.Status.ToLower() == "not yet scanned" || (server.LastScan.AddMinutes(server.ScanInterval) < (DateTime.Now))) && server.IsBeingScanned == false)
					{
						//Scan Here;
						server.IsBeingScanned = true;

						Common.WriteDeviceHistoryEntry("LatencyTest", "LatencyTest", "Scanning server " + server.Name + " in collection " + server.Description, commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);
						
						Thread TestLatency = new Thread(() => TestResponseTimeBetweenServers(currColl));
						TestLatency.CurrentCulture = c == null ? new CultureInfo("en-US") : c;  //Should only be null on our local copies if using wrapper
						TestLatency.IsBackground = true;
						TestLatency.Priority = ThreadPriority.Normal;
						TestLatency.Name = server.Description + "-LatencyTests";
						TestLatency.Start();
						Thread.Sleep(2000);
					}

				}

				Thread.Sleep(5000);

			}
		}


		public void TestResponseTimeBetweenServers(MonitoredItems.MicrosoftServersCollection myServersCollection)
		{
			string TestName = myServersCollection.get_Item(0).Description;
			CommonDB db = new CommonDB();
			string NetworkLatencyID = db.GetData("Select NetworkLatencyId from NetworkLatency where TestName='" + TestName + "'").Rows[0][0].ToString();

			Collection<PSObject> psObjCol = new Collection<PSObject>();
			foreach (MonitoredItems.MicrosoftServer Server in myServersCollection)
			{
				PSObject psObj = new PSObject();

				System.Security.SecureString securePassword = Common.String2SecureString(Server.Password);

				PSCredential creds = new PSCredential(Server.UserName, securePassword);

				psObj.Properties.Add(new PSNoteProperty("Name", Server.Name));
				psObj.Properties.Add(new PSNoteProperty("ID", Server.ServerId));
				psObj.Properties.Add(new PSNoteProperty("Creds", creds));
				psObjCol.Add(psObj);
			}

			Runspace runspace = RunspaceFactory.CreateRunspace();
			PowerShell powershell = PowerShell.Create();
			System.Collections.ObjectModel.Collection<PSObject> results = new System.Collections.ObjectModel.Collection<PSObject>();

			runspace.Open();
			powershell.Runspace = runspace;

			string script = AppDomain.CurrentDomain.BaseDirectory.ToString() + "Scripts\\TestResponseTimesBetweenServers.ps1";
			PSCommand command = new PSCommand();
			command.AddCommand(script);
			command.AddParameter("Servers", psObjCol);
			command.AddParameter("TestDuration", myServersCollection.get_Item(0).RetryInterval);

			powershell.Commands = command;

			results = powershell.Invoke();

			Common.WriteDeviceHistoryEntry("LatencyTest", "LatencyTest", "Results : " + results.Count, commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);

			string sqlDelete = "DELETE FROM NetworkLatencyStats WHERE NetworkLatencyID = (select NL.NetworkLatencyID from VitalSigns.dbo.NetworkLatency NL where TestName='" + TestName + "')";

			string sqlInsert = "INSERT INTO NetworkLatencyStats (NetworkLatencyID, SourceServer, DestinationServer, Latency, Date) VALUES ";

			foreach (PSObject ps in results)
			{
				string sourceServerID = ps.Properties["FromID"].Value.ToString();
				string destServerID = ps.Properties["ToID"].Value.ToString();
				string sourceServer = ps.Properties["From"].Value.ToString();
				string destServer = ps.Properties["To"].Value.ToString();
				string avgTime = ps.Properties["AvgTime"].Value.ToString();

				sqlInsert += "(" + NetworkLatencyID + ", '" + sourceServer + "', '" + destServer + "', " + avgTime + ", getDate()),";
			}

			sqlInsert = sqlInsert.Substring(0, sqlInsert.Length - 1);

			db = new CommonDB("VSS_Statistics");
			db.Execute(sqlDelete);
			db.Execute(sqlInsert);

			foreach (MonitoredItems.MicrosoftServer server in myServersCollection)
			{
				server.IsBeingScanned = false;
				server.NextScan = DateTime.Now.AddMinutes(server.ScanInterval);
				server.LastScan = DateTime.Now;
				server.Status = "Scanned";
			}

		}

		#endregion


		public void RefreshWindowsCollection()
		{
			CreateWindowsServersCollection();
			StartWindowsThreads();
		}

		public void RefreshLatencyCollection()
		{
			initLatencyCollection();
		}

	}
}
