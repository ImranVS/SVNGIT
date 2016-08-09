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

using MongoDB.Driver;
using VSNext.Mongo.Entities;
using VSNext.Mongo.Repository;

namespace VitalSignsMicrosoftClasses
{
	public class ActiveDirectoryMAIN
	{
		Mutex ADMutex = new Mutex();
		string CurrentCulture = "en-US";
		string CultureStringName = "CultureString";
		CultureInfo c;
		MonitoredItems.ActiveDirectoryServersCollection myActiveDirectoryServers;
		Thread DiagTestThread = null;

        string serverType = VSNext.Mongo.Entities.Enums.ServerType.ActiveDirectory.ToDescription();
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
				Common.WriteDeviceHistoryEntry("All", serverType, "AD Service is starting up", Common.LogLevel.Normal);
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

				if (db.RecordExists("SELECT * FROM SelectedFeatures sf WHERE sf.FeatureID in (SELECT ID FROM Features WHERE Name='" + serverType + "')"))
				{
					Common.WriteDeviceHistoryEntry("All", serverType, "Server is marked for scanning so will start Server Related Tasks", Common.LogLevel.Normal);
					CreateActiveDirectoryServersCollection();
					Common.InitStatusTable(myActiveDirectoryServers);
					StartADThreads();
					//CreateSharepointCollection();
					//InitStatusTable(myExchangeServers);
					//StartMailProbeThreads();
					//TestResults AllTestResults = new TestResults();
					//ActiveDirectoryCommon adCommon = new ActiveDirectoryCommon();
					//MonitoredItems.ActiveDirectoryServer AdServer= new MonitoredItems.ActiveDirectoryServer();
					//AdServer.Name = "jnittech.com";
					//AdServer.UserName = "jnittech\\administrator";
					//AdServer.Password = "Pa$$w0rd";
					//AdServer.IPAddress = "jnittech-ad.jnittech.com";
					//AdServer.ServerType = "Active Directory";
					//adCommon.checkServer(AdServer, ref AllTestResults);
					//MicrosoftCommon msCommon = new MicrosoftCommon();
					//msCommon.PrereqForWindows(AdServer, ref AllTestResults);

					//sleep for one minute to allow time for the collection to be made 
					Thread.Sleep(60 * 1000 * 1);

					Thread HourlyTasksThread = new Thread(new ThreadStart(HourlyTasks));
					HourlyTasksThread.CurrentCulture = c;
					HourlyTasksThread.IsBackground = true;
					HourlyTasksThread.Name = "HourlyTasks - AD";
					HourlyTasksThread.Priority = ThreadPriority.Normal;
					HourlyTasksThread.Start();
					Thread.Sleep(2000);
					/*
					Thread monitorChanges = new Thread(new ThreadStart(CheckForTableChanges));
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
					DailyTasksThread.Name = "DailyTasks - AD";
					DailyTasksThread.Start();
					Thread.Sleep(2000);
				}
				else
				{
					myActiveDirectoryServers = null;
					Common.WriteDeviceHistoryEntry("All", serverType, "Server is not marked for scanning", Common.LogLevel.Normal);
				}


				

				Common.WriteDeviceHistoryEntry("All", serverType, "All Processes are started in startProcess", Common.LogLevel.Normal);
			}
			catch (Exception ex)
			{
				Common.WriteDeviceHistoryEntry("All", serverType, "Error starting StartProcess exception CreateADServersCollection: " + ex.StackTrace.ToString(), Common.LogLevel.Normal);
				throw ex;

			}

		}
		int serverThreadCount = 0;
		int initialServerThreadCount = 0;
		System.Collections.ArrayList AliveServerMainThreads = new System.Collections.ArrayList();
		private void StartADThreads()
		{
			int startThreads = 0;
			int maxThreadCount = Common.getThreadCount("ActiveDirectory");
			serverThreadCount = myActiveDirectoryServers.Count / 3;
			if (serverThreadCount <= 1)
				if (myActiveDirectoryServers.Count > 1)
				serverThreadCount = 2;
				else
					serverThreadCount = 1;
			if (serverThreadCount <= 1 && myActiveDirectoryServers.Count > 1)
				serverThreadCount = 2;

			// 5/19/15 WS commented out.  VSPLUS 1776
			if (serverThreadCount > maxThreadCount)
				serverThreadCount = maxThreadCount;
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
				Thread monitorAD = new Thread(() => MonitorAD(i));
				monitorAD.CurrentCulture = c == null ? new CultureInfo("en-US") : c;  //Should only be null on our local copies if using wrapper
				monitorAD.IsBackground = true;
				monitorAD.Priority = ThreadPriority.Normal;
				monitorAD.Name = i.ToString() + "-AD Monitoring Thread";
				AliveServerMainThreads.Add(monitorAD);
				monitorAD.Start();
				Thread.Sleep(2000);
			}

		}
		public MonitoredItems.ActiveDirectoryServer SelectServerToMonitor()
		{

			DateTime tNow = DateTime.Now;
			DateTime tScheduled;

			DateTime timeOne;
			DateTime timeTwo;

			MonitoredItems.ActiveDirectoryServer SelectedServer = null;

			MonitoredItems.ActiveDirectoryServer ServerOne = null;
			MonitoredItems.ActiveDirectoryServer ServerTwo = null;

			RegistryHandler myRegistry = new RegistryHandler();

			String strSQL = "";
			String ServerType = "ActiveDirectory";
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

					for (int n = 0; n < myActiveDirectoryServers.Count; n++)
					{
						ServerOne = myActiveDirectoryServers.get_Item(n);
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

			//Searches for the first enounter of a Not Responding server that is due for a scan
			for (int n = 0; n < myActiveDirectoryServers.Count; n++)
			{
				ServerOne = myActiveDirectoryServers.get_Item(n);
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
			for (int n = 0; n < myActiveDirectoryServers.Count; n++)
			{
				ServerOne = myActiveDirectoryServers.get_Item(n);
				if ((ServerOne.Status == "Not Scanned" || ServerOne.Status == "Master Service Stopped.") && ServerOne.IsBeingScanned == false && ServerOne.Enabled)
				{
					Common.WriteDeviceHistoryEntry("All", serverType, "Selecting " + ServerOne.Name + " because the status is " + ServerOne.Status + ".");
					return ServerOne;
				}
			}


			//Searches for all servers that are due for a scan
			List<MonitoredItems.ActiveDirectoryServer> ScanCanidates = new List<MonitoredItems.ActiveDirectoryServer>();

			foreach (MonitoredItems.ActiveDirectoryServer srv in myActiveDirectoryServers)
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
		private void MonitorAD(int threadNum)
		{
			Thread.CurrentThread.CurrentCulture = c;
			//MonitoredItems.ExchangeServer CurrentServer;
			CommonDB DB = new CommonDB();
			while (true)
			{


				ADMutex.WaitOne();
				MonitoredItems.ActiveDirectoryServer thisServer;
				try
				{
					thisServer = SelectServerToMonitor();
				
					if (thisServer != null && !thisServer.IsBeingScanned)
					{
						thisServer.IsBeingScanned = true;
					}
				}
				catch(Exception ex)
				{
					thisServer = null;
				}
				finally
				{
					ADMutex.ReleaseMutex();
				}

				if(thisServer != null)
				{

					Common.WriteDeviceHistoryEntry("All", serverType, "Scanning Server " + thisServer.Name + " on thread " + threadNum);
					thisServer.IsBeingScanned = true;

					MaintenanceDll maintenance = new MaintenanceDll();
					if (maintenance.InMaintenance(thisServer.ServerType , thisServer.Name))
					{
						Common.ServerInMaintenance(thisServer);
						goto CleanUp;
					}

					if (thisServer.StatusCode == "Maintenance")
					{
						Common.WriteDeviceHistoryEntry(thisServer.ServerType, thisServer.Name, "Doing a fast scan sicne it is in maintenance", Common.LogLevel.Normal);
						thisServer.FastScan = true;
					}
                    Common.SetupServer(thisServer, thisServer.ServerType);
					TestResults AllTestResults = new TestResults();
					ActiveDirectoryCommon ADCommon = new ActiveDirectoryCommon();
					bool notResponding = false;

					//This is to make it so all the cmdlets are not gettign downlaoded to try to cut back on time
					string cmdList = "-CommandName Get-ADDomainController";
					

					using (ReturnPowerShellObjects PSO = Common.TestRepsonding(thisServer, ref notResponding, ref AllTestResults, cmdList))
					{
						if (!notResponding)
						{

                            MicrosoftCommon MSCommon = new MicrosoftCommon();
                            MSCommon.PrereqForWindows(thisServer, AllTestResults, PSO);

							if (!thisServer.FastScan)
							{
								ADCommon.checkServer(thisServer, ref AllTestResults, ref notResponding);
							}
							

							VSNext.Mongo.Repository.Repository<VSNext.Mongo.Entities.Server> ServerRepo = new VSNext.Mongo.Repository.Repository<VSNext.Mongo.Entities.Server>(DB.GetMongoConnectionString());
                            if (ServerRepo.Find(i => i.DeviceName == thisServer.Name && i.DeviceType == thisServer.ServerType).Where(j => j.WindowServices != null && j.WindowServices.Where(k => k.ServerRequired).Count() > 0).Count() == 0)
                            {
                                MongoStatementsUpdate<VSNext.Mongo.Entities.Server> updateStatement = new MongoStatementsUpdate<VSNext.Mongo.Entities.Server>();
                                updateStatement.filterDef = updateStatement.repo.Filter.Where(i => i.DeviceName == thisServer.Name && i.DeviceType == thisServer.ServerType)
                                    & updateStatement.repo.Filter.ElemMatch("windows_services", updateStatement.repo.Filter.Regex("display_name", "Active Directory") & updateStatement.repo.Filter.Eq("server_required", false));
                                updateStatement.updateDef = updateStatement.repo.Updater
                                    .Set(i => i.WindowServices[-1].Monitored, true)
                                    .Set(i => i.WindowServices[-1].ServerRequired, true);
                                updateStatement.embeddedDocument = true;

                                AllTestResults.MongoEntity.Add(updateStatement);
                            }
							DB.UpdateAllTests(AllTestResults, thisServer, thisServer.ServerType);
							thisServer.IsBeingScanned = false;
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

		}

		private void CreateActiveDirectoryServersCollection()
		{
			//Fetch all servers
			if (myActiveDirectoryServers == null)
				myActiveDirectoryServers = new MonitoredItems.ActiveDirectoryServersCollection();

			CommonDB DB = new CommonDB();

			//This sql gets all the servers assigned to this node AND insufficent licenses
			StringBuilder SQL = new StringBuilder();
			SQL.Append(" select distinct Sr.ID,Sr.ServerName,S.ServerType, S.ID as ServerTypeId,L.Location,sa.ScanInterval,sa.RetryInterval,sa.OffHourInterval,sa.Enabled,sr.ipaddress,sa.category,cr.UserID,cr.Password,sa.ResponseTime, ");
			SQL.Append(" st.LastUpdate, st.Status, st.StatusCode, di.CurrentNodeID, sa.CPU_Threshold, sa.MemThreshold, sa.ConsOvrThresholdBefAlert,sa.ConsFailuresBefAlert ");
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
			SQL.Append(" where S.ServerType='" + serverType+"' and sa.Enabled = 1 order by sr.id");
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

					MonitoredItems.ActiveDirectoryServer myADServer = null;

					//Checks to see if the server is newly added or exists.  Adds if it is new
					try
					{
						myADServer = myActiveDirectoryServers.SearchByName(DR["ServerName"].ToString());
						if (myADServer == null)
						{
							//New server.  Set inits and add to collection

							myADServer = InitForADServers(myADServer, DR);
							myActiveDirectoryServers.Add(myADServer);
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
						myADServer = InitForADServers(myADServer, DR);
						myActiveDirectoryServers.Add(myADServer);
						newServers++;
					}

					myADServer = SetADServerSettings(myADServer, DR);


				}



				//Removes servers not in the new lsit
				foreach (MonitoredItems.ActiveDirectoryServer server in myActiveDirectoryServers)
				{
					string currName = server.Name;
					try
					{
						if (!ServerNameList.Contains(currName))
						{
							//incase it doesnt throw and exception, if not found, removed server from list
							myActiveDirectoryServers.Delete(currName);
							removedServers++;

						}
					}
					catch (Exception ex)
					{
						//server not found
						myActiveDirectoryServers.Delete(currName);
						removedServers++;

					}

				}

				//myExchangeServers = newCollection;
				/**********************************************************/
				Common.WriteDeviceHistoryEntry("All",serverType , "There are " + myActiveDirectoryServers.Count + " servers in the collection.  " + newServers + " were new and " + updatedServers + " were updated.");
			}
			else
			{
				myActiveDirectoryServers = new MonitoredItems.ActiveDirectoryServersCollection();
			}

			//This function does status inserts for insufficent licenses, removes those servers from the collection and raises/resets system message accordingly
			Common.InsertInsufficentLicenses(myActiveDirectoryServers);

			//At this point we have all Servers with ALL the information(including Threshold settings)
		}

		private MonitoredItems.ActiveDirectoryServer InitForADServers(MonitoredItems.ActiveDirectoryServer MyADServer, DataRow DR)
		{
			MyADServer = new MonitoredItems.ActiveDirectoryServer();

			MyADServer.Enabled = true;
			MyADServer.ServerTypeId = int.Parse(DR["ServerTypeId"].ToString());
			MyADServer.ServerType = DR["ServerType"].ToString();
			MyADServer.LastScan = DR["LastUpdate"] == null || DR["LastUpdate"].ToString() == "" ? DateTime.Now : DateTime.Parse(DR["LastUpdate"].ToString());
			MyADServer.Status = DR["Status"] == null || DR["Status"].ToString() == "" ? "Not Scanned" : DR["Status"].ToString();
			MyADServer.StatusCode = DR["StatusCode"] == null || DR["StatusCode"].ToString() == "" ? "Maintenance" : DR["StatusCode"].ToString();

			return MyADServer;
		}

		private MonitoredItems.ActiveDirectoryServer SetADServerSettings(MonitoredItems.ActiveDirectoryServer MyADServer, DataRow DR)
		{
			MyADServer.IPAddress = DR["IPAddress"].ToString().Replace("http:\\\\", "").Replace("https:\\\\", "").Replace("http://", "").Replace("https://", "");
			MyADServer.Name = DR["ServerName"].ToString().Replace("http:\\\\", "").Replace("https:\\\\", "").Replace("http://", "").Replace("https://", "");
			MyADServer.ServerId = DR["ID"].ToString();
			MyADServer.UserName = DR["UserID"].ToString();
			MyADServer.Password = Common.decodePasswordFromEncodedString(DR["Password"].ToString(), MyADServer.Name);
			MyADServer.Location = DR["Location"].ToString();
			
			MyADServer.ResponseThreshold = long.Parse(DR["ResponseTime"].ToString());
			MyADServer.ScanInterval = int.Parse(DR["ScanInterval"].ToString());
			MyADServer.OffHoursScanInterval = int.Parse(DR["OffHourInterval"].ToString());
			MyADServer.RetryInterval = int.Parse(DR["RetryInterval"].ToString());
			MyADServer.CPU_Threshold = int.Parse(DR["CPU_Threshold"].ToString());
			MyADServer.Memory_Threshold = int.Parse(DR["MemThreshold"].ToString());
			MyADServer.ServerDaysAlert = DR["ConsOvrThresholdBefAlert"].ToString();
			MyADServer.FailureThreshold = int.Parse(DR["ConsFailuresBefAlert"].ToString());

			//MyExchangeServer.FailureThreshold = int.Parse(DR["ConsFailuresBefAlert"].ToString());
			MyADServer.Category = DR["Category"].ToString();

			MyADServer.InsufficentLicenses = DR["CurrentNodeID"] != null && DR["CurrentNodeID"].ToString() == "-1" ? true : false;

			return MyADServer;
		}

        /*
		protected void InitStatusTable(MonitoredItems.ActiveDirectoryServersCollection collection)
		{
			try
			{
				String type = "";
				CommonDB db = new CommonDB();
				if (collection.Count > 0)
					type = collection.get_Item(0).ServerType;

				if (type != "")
				{
					foreach (MonitoredItems.ActiveDirectoryServer server in collection)
					{
						String sql = "IF NOT EXISTS(SELECT * FROM Status WHERE TypeANDName = '" + server.Name + "-" + type + "') BEGIN " +
							"INSERT INTO Status ( Type, Location, Category, Name, Status, Details, Description, TypeANDName, StatusCode ) VALUES " +
							" ('" + type + "', '" + server.Location + "', '" + server.Category + "', '" + server.Name + "', '" + server.Status + "', 'This server has not yet been scanned.', " +
							"'Microsoft " + type + " Server', '" + server.Name + "-" + type + "', '" + server.StatusCode + "') END";

						db.Execute(sql);
					}
					Common.WriteDeviceHistoryEntry("All", serverType , type + " Servers are marked as Not Scanned", Common.LogLevel.Normal);

				}
			}
			catch (Exception ex)
			{
				Common.WriteDeviceHistoryEntry("All", serverType, "Error in init status.  Error: " + ex.Message, Common.LogLevel.Normal);
			}

		}
        */
		public void RefreshActiveDirectoryCollection()
		{
			if (c != null)
			{
				CreateActiveDirectoryServersCollection();
				StartADThreads();
			}
		}

		#region DailyTasks

		private void DailyTasks()
		{
			Thread.Sleep(10 * 1000);
		}


		#endregion

		#region HourlyTasks


		private void HourlyTasks()
		{
			MonitoredItems.ActiveDirectoryServer DummyServerForLogs = new MonitoredItems.ActiveDirectoryServer() { Name = "HourlyTask", ServerType = serverType };
			Thread HourlyTasksThread = null;
			int Hour = -1;
			while (true)
			{
				if (Hour == -1 || Hour != DateTime.Now.Hour)
				{
					if (HourlyTasksThread != null && HourlyTasksThread.IsAlive)
					{
						Common.WriteDeviceHistoryEntry(DummyServerForLogs.ServerType, DummyServerForLogs.Name, "The thread for Hourly Tasks got hung up and will be killed to start the next cycle.", commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);
						HourlyTasksThread.Abort();
					}

					HourlyTasksThread = new Thread(() => HourlyTasksMainThread(DummyServerForLogs));
					HourlyTasksThread.CurrentCulture = c;
					HourlyTasksThread.IsBackground = true;
					HourlyTasksThread.Priority = ThreadPriority.Normal;
					HourlyTasksThread.Name = "HourlyTaskWorkerThread - AD";
					HourlyTasksThread.Start();
					//Thread.Sleep(60 * 60 * 1000); //sleeps for 1 hour
					Hour = DateTime.Now.Hour;
				}


				//sleep for 5 mins
				Thread.Sleep(1000 * 60);

			}
		}

		private void HourlyTasksMainThread(MonitoredItems.ActiveDirectoryServer DummyServerForLogs)
		{


			Common.WriteDeviceHistoryEntry("All", DummyServerForLogs.ServerType, "Time to do Hourly Tasks for " + DummyServerForLogs.ServerType + ".", commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);

			try
			{
				try
				{

					TestResults testResults = new TestResults();

					if (myActiveDirectoryServers != null)
						foreach (MonitoredItems.ActiveDirectoryServer server in myActiveDirectoryServers)
						{
							server.OffHours = Common.OffHours(server.Name);
							Common.RecordUpAndDownTimes(server, ref testResults);
						}

					CommonDB db = new CommonDB();
					db.UpdateSQLStatements(testResults, DummyServerForLogs);



                    foreach (MonitoredItems.ActiveDirectoryServer Server in myActiveDirectoryServers)
                    {
                        try
                        {
                            if (Server.StatusCode == "Not Responding")
                                break;
                            TestResults AllTestResults = new TestResults();


                            DiagnoseTest(Server, ref AllTestResults);
                            
                            MongoStatementsUpdate<VSNext.Mongo.Entities.Status> updateStatement = new MongoStatementsUpdate<VSNext.Mongo.Entities.Status>();
                            updateStatement.filterDef = updateStatement.repo.Filter.Where(i => i.TypeAndName == Server.TypeANDName);
                            updateStatement.updateDef = updateStatement.repo.Updater
                                .Set(i => i.AdvertisingTest, Server.ADAdvertisingTest)
                                .Set(i => i.FrsSysVolTest, Server.ADFrsSysVolTest)
                                .Set(i => i.ReplicationTest, Server.ADReplicationsTest)
                                .Set(i => i.ServicesTest, Server.ADServicesTest)
                                .Set(i => i.DnsTest, Server.ADDNSTest)
                                .Set(i => i.FsmoCheckTest, Server.ADFsmoCheckTest);
                            AllTestResults.MongoEntity.Add(updateStatement);

                            Common.SetHourlyAlertsToObject(AllTestResults, myActiveDirectoryServers);
                            
                            db.UpdateSQLStatements(AllTestResults, Server);

                        }
                        catch (Exception ex)
                        {
                            Common.WriteDeviceHistoryEntry("All", "Active_Directory_Diag_Thread", "Error in Diag Thread With server " + Server.Name + ":" + ex.Message.ToString(), Common.LogLevel.Normal);
                        }

                    }




				}
				catch (Exception ex)
				{
					Common.WriteDeviceHistoryEntry("All", DummyServerForLogs.ServerType, "Error setting OffHours.  Error: " + ex.Message, commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);
				}


				Common.WriteDeviceHistoryEntry("All", DummyServerForLogs.ServerType, "Finished Hourly Tasks.", commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);
				Common.WriteDeviceHistoryEntry(DummyServerForLogs.ServerType, DummyServerForLogs.Name, "Finished Hourly Tasks.", commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);
			}
			catch (Exception ex)
			{
				Common.WriteDeviceHistoryEntry(DummyServerForLogs.ServerType, DummyServerForLogs.Name, "Error in Hourly Tasks Main Thread.  Error: " + ex.Message, commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);
			}



		}

		

		#endregion

		#region
		

		public void DiagnoseTest(MonitoredItems.ActiveDirectoryServer myServer, ref TestResults AllTestsList)
		{
			try
			{
				PowerShell PS = PowerShell.Create();
				Runspace runspace = RunspaceFactory.CreateRunspace();
				PS.Runspace = runspace;
				PS.Runspace.Open();

				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "DiagnoseTest: Starting.", Common.LogLevel.Normal);
				System.Collections.ObjectModel.Collection<PSObject> results = new System.Collections.ObjectModel.Collection<PSObject>();
				System.IO.StreamReader sr = new System.IO.StreamReader(AppDomain.CurrentDomain.BaseDirectory.ToString() + "Scripts\\AD_DCDiagnose.ps1");
				String str = sr.ReadToEnd();
				//str = "$DCs = Get-ADDomainController -filter * | where {$_.Hostname -eq  '" + myServer.Name + "'}" + str;

				PS.Commands.Clear();
				PS.Streams.ClearStreams();
				//powershellobj.PS.AddScript(str);
				cmdScriptWrapper(ref PS, "Scripts\\AD_DCDiagnose.ps1", myServer);
				results = PS.Invoke();

				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "DiagnoseTest Results: " + results.Count, Common.LogLevel.Normal);
				DateTime dtNow = DateTime.Now;
				int weekNumber = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(dtNow, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday);
				if (results.Count > 0)
				{
					Collection<PSObject> trimmedResults = new Collection<PSObject>();
					for (int i = 0; i < results.Count; i++)
					{
						if (results[i].BaseObject.ToString() != "")
						{
							trimmedResults.Add(results[i]);
						}
					}
					results = trimmedResults;
					DiagnoseTestParse(results, myServer, ref AllTestsList);

				}
				else
				{
					myServer.ADAdvertisingTest = "N/A";
					myServer.ADFrsSysVolTest = "N/A";
					myServer.ADReplicationsTest = "N/A";
					myServer.ADServicesTest = "N/A";
					myServer.ADDNSTest = "N/A";
					myServer.ADFsmoCheckTest = "N/A";

				}
			}
			catch (Exception ex)
			{
				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "DiagnoseTest Results: Exception: " + ex.Message.ToString(), Common.LogLevel.Verbose);
				myServer.ADAdvertisingTest = "N/A";
				myServer.ADFrsSysVolTest = "N/A";
				myServer.ADReplicationsTest = "N/A";
				myServer.ADServicesTest = "N/A";
				myServer.ADDNSTest = "N/A";
				myServer.ADFsmoCheckTest = "N/A";
				Common.makeAlert(false, myServer, commonEnums.AlertType.Query_Latency, ref AllTestsList, myServer.ServerType);
			}
		}

		public void DiagnoseTestParse(Collection<PSObject> arr, MonitoredItems.ActiveDirectoryServer myServer, ref TestResults AllTestsList)
		{
			Collection<PSObject> trimmedResults = new Collection<PSObject>();

			for (int i = 0; i < arr.Count; i++)
			{
				string s = arr[i].BaseObject.ToString().Replace(" ", "");

				if (arr[i].BaseObject.ToString().Replace(" ", "").StartsWith("Startingtest:") || arr[i].BaseObject.ToString().Replace(" ", "").StartsWith("........................."))
				{
					trimmedResults.Add(arr[i]);
				}

			}

			for (int i = 0; i < arr.Count; i++)
			{
				if (arr[i].BaseObject.ToString().Replace(" ", "").StartsWith("SummaryofDNStestresults:"))
				{
					trimmedResults.Add(arr[i + 2]);
					trimmedResults.Add(arr[i + 5]);
					i = i + 4;
				}
			}
			bool firstDNSTestDone = false;

			try
			{
				for (int i = 0; i < trimmedResults.Count; i = i + 2)
				{
					string testLine = trimmedResults[i].ToString();
					string resultLine = trimmedResults[i + 1].ToString();

					if (testLine.Replace(" ", "") == "AuthBascForwDelDynRRegExt")
					{
						//DNS Results
						//                                Auth Basc Forw Del  Dyn  RReg Ext          
						//   JNITTECH-AD                  PASS WARN PASS FAIL WARN PASS PASS 


						int startIndex = resultLine.TakeWhile(char.IsWhiteSpace).Count();
						resultLine = resultLine.Substring(startIndex);
						//JNITTECH-AD                  PASS WARN PASS FAIL WARN PASS PASS 

						resultLine = resultLine.Substring(resultLine.IndexOf("  "));
						//                  PASS WARN PASS FAIL WARN PASS PASS 

						startIndex = resultLine.TakeWhile(char.IsWhiteSpace).Count();
						resultLine = resultLine.Substring(startIndex);
						//PASS WARN PASS FAIL WARN PASS PASS 

						string AuthenticationTest = resultLine.Substring(0, resultLine.IndexOf(" "));
						resultLine = resultLine.Substring(resultLine.IndexOf(" ") + 1);
						//WARN PASS FAIL WARN PASS PASS 
						string AuthError = "The DNS Authentication Test failed ";
						if (AuthenticationTest != "PASS")
							AuthError = findDNSError("Auth", arr, AuthError, "DNS Authentication");
						else
							AuthError = AuthError.Replace("failed", "passed");

						string BasicTest = resultLine.Substring(0, resultLine.IndexOf(" "));
						resultLine = resultLine.Substring(resultLine.IndexOf(" ") + 1);
						//PASS FAIL WARN PASS PASS 
						string BasicError = "The DNS Basic Test failed ";
						if (BasicTest != "PASS")
							BasicError = findDNSError("Basc", arr, BasicError, "DNS Basic");
						else
							BasicError = BasicError.Replace("failed", "passed");

						string FowardersTest = resultLine.Substring(0, resultLine.IndexOf(" "));
						resultLine = resultLine.Substring(resultLine.IndexOf(" ") + 1);
						//FAIL WARN PASS PASS 
						string FowardersError = "The DNS Fowarders Test failed ";
						if (FowardersTest != "PASS")
							FowardersError = findDNSError("Forw", arr, FowardersError, "DNS Fowarders");
						else
							FowardersError = FowardersError.Replace("failed", "passed");

						string DelegationTest = resultLine.Substring(0, resultLine.IndexOf(" "));
						resultLine = resultLine.Substring(resultLine.IndexOf(" ") + 1);
						//WARN PASS PASS 
						string DelegationError = "The DNS Delegation Test failed ";
						if (DelegationTest != "PASS")
							DelegationError = findDNSError("Del", arr, DelegationError, "DNS Delegation");
						else
							DelegationError = DelegationError.Replace("failed", "passed");

						string DynamicTest = resultLine.Substring(0, resultLine.IndexOf(" "));
						resultLine = resultLine.Substring(resultLine.IndexOf(" ") + 1);
						//PASS PASS 
						string DynamicError = "The DNS Dynamic Update Test failed ";
						if (DynamicTest != "PASS")
							DynamicError = findDNSError("Dyn", arr, DynamicError, "DNS Dynamic Update");
						else
							DynamicError = DynamicError.Replace("failed", "passed");

						string RecordRegistrationTest = resultLine.Substring(0, resultLine.IndexOf(" "));
						resultLine = resultLine.Substring(resultLine.IndexOf(" ") + 1);
						//PASS 
						string RecordRegistrationError = "The DNS Record Registration Test failed ";
						if (RecordRegistrationTest != "PASS")
							RecordRegistrationError = findDNSError("Rreg", arr, RecordRegistrationError, "DNS Record Registration");
						else
							RecordRegistrationError = RecordRegistrationError.Replace("failed", "passed");

						string ExtNameTest = resultLine.Substring(0, resultLine.IndexOf(" "));
						resultLine = resultLine.Substring(resultLine.IndexOf(" ") + 1);
						string ExtNameError = "The DNS External Name Test failed ";
						if (ExtNameTest != "PASS")
							ExtNameError = findDNSError("Ext", arr, ExtNameError, "DNS External Name");
						else
							ExtNameError = ExtNameError.Replace("failed", "passed");

						Common.makeAlert(AuthenticationTest == "PASS" ? true : false, myServer, commonEnums.AlertType.DNS_Authentication_Test, ref AllTestsList, AuthError, "Active Directory");
						Common.makeAlert(BasicTest == "PASS" ? true : false, myServer, commonEnums.AlertType.DNS_Basic_Test, ref AllTestsList, BasicError, "Active Directory");
						Common.makeAlert(FowardersTest == "PASS" ? true : false, myServer, commonEnums.AlertType.DNS_Fowarders_Test, ref AllTestsList, FowardersError, "Active Directory");
						Common.makeAlert(DelegationTest == "PASS" ? true : false, myServer, commonEnums.AlertType.DNS_Delegation_Test, ref AllTestsList, DelegationError, "Active Directory");
						Common.makeAlert(DynamicTest == "PASS" ? true : false, myServer, commonEnums.AlertType.DNS_Dynamic_Update_Test, ref AllTestsList, DynamicError, "Active Directory");
						Common.makeAlert(RecordRegistrationTest == "PASS" ? true : false, myServer, commonEnums.AlertType.DNS_Record_Registration_Test, ref AllTestsList, RecordRegistrationError, "Active Directory");
						Common.makeAlert(ExtNameTest == "PASS" ? true : false, myServer, commonEnums.AlertType.DNS_External_Name_Test, ref AllTestsList, ExtNameError, "Active Directory");

						continue;
					}

					string test = testLine.Substring(testLine.IndexOf(':') + 1).Replace(" ", "");
					string result = resultLine.Contains("passed") ? "Pass" : "Fail";

					if (result == "Fail")
					{
						if (checkForAccessDenied(arr, testLine, resultLine))
							result = "N/A";
					}
					commonEnums.AlertType alertType = commonEnums.AlertType.Active_Sync;  //using this to see if "null"
					switch (test)
					{
						case "Advertising":
							myServer.ADAdvertisingTest = result;
							alertType = commonEnums.AlertType.Advertising_Test;
							break;

						case "FrsSysVol":
							myServer.ADFrsSysVolTest = result;
							alertType = commonEnums.AlertType.FRS_System_Volume_Test;
							break;

						case "Replications":
							myServer.ADReplicationsTest = result;
							alertType = commonEnums.AlertType.Replications_Test;
							break;

						case "Services":
							myServer.ADServicesTest = result;
							alertType = commonEnums.AlertType.Services_Test;
							break;

						case "DNS":
							if (firstDNSTestDone)
							{
								if (myServer.ADDNSTest == "Fail" || result == "Fail")
									myServer.ADDNSTest = "Fail";
								else
									myServer.ADDNSTest = "Pass";

								alertType = commonEnums.AlertType.DNS_Test;
							}
							else
							{
								myServer.ADDNSTest = result;
								firstDNSTestDone = true;
							}
							break;

						case "FsmoCheck":
							myServer.ADFsmoCheckTest = result;
							alertType = commonEnums.AlertType.FSMO_Check_Test;
							break;

					}
					string details;
					if (result == "Pass")
						details = "The " + test + " test passed with no issues.";
					else if (result == "N/A")
						details = "The " + test + " test failed with a message of Access Denied ";
					else
						details = "The " + test + " test failed ";

					if (alertType != commonEnums.AlertType.Active_Sync)
						Common.makeAlert(result == "Pass" ? true : false, myServer, alertType, ref AllTestsList, details, "Active Directory");

				}
			}
			catch (Exception ex)
			{ }

			//return sql.Substring(0,sql.Length-1);
		}

		public Boolean checkForAccessDenied(Collection<PSObject> arr, string testLine, string resultLine)
		{
			int i = 0;
			while (arr[i].ToString() != testLine)
				i++;

			int endIndex = i;
			while (arr[endIndex].ToString() != resultLine)
				endIndex++;

			for (int n = i; n < endIndex; n++)
				if (arr[n].ToString().Contains("Access is denied"))
					return true;

			return false;
		}


		public string findDNSError(string type, Collection<PSObject> arr, string error, string testName)
		{

			for (int i = 0; i < arr.Count(); i++)
			{
				if (arr[i].ToString().Contains(type) && arr[i].ToString().Contains("TEST:"))
					return "The " + testName + " test failed with the following message: " + arr[i + 1].ToString().Substring(arr[i + 1].ToString().TakeWhile(char.IsWhiteSpace).Count());
			}

			return error;
		}

		private void cmdScriptWrapper(ref PowerShell powershell, String scriptName, MonitoredItems.ActiveDirectoryServer Server)
		{

			System.IO.StreamReader scriptStream = new System.IO.StreamReader(AppDomain.CurrentDomain.BaseDirectory.ToString() + scriptName);
			string scriptToExecute = scriptStream.ReadToEnd();

			System.Security.SecureString securePassword = Common.String2SecureString(Server.Password);
			PSCredential creds = new PSCredential(Server.UserName, securePassword);

			powershell.AddCommand("Set-Variable");
			powershell.AddParameter("Name", "cred");
			powershell.AddParameter("Value", creds);

			powershell.AddScript(@"$cmd={" + scriptToExecute + "}");
			powershell.AddScript(@"Invoke-Command -ComputerName " + Server.Name + " -Credential $cred -ScriptBlock $cmd");



		}


		#endregion

	}
}
