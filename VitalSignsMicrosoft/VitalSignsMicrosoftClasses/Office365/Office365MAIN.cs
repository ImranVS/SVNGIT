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
using MongoDB.Driver;
using VSNext.Mongo.Entities;

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
        string connectionString = ConfigurationManager.ConnectionStrings["VitalSignsMongo"].ToString();
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

				if (true || db.RecordExists("SELECT * FROM SelectedFeatures sf WHERE sf.FeatureID in (SELECT ID FROM Features WHERE Name='" + "Office 365" + "')"))
				{
                    VSFramework.RegistryHandler mySettings = new RegistryHandler();
                    try
                    {
                        var office365UrlSetting = mySettings.ReadFromRegistry("Office365URL");
                        if (office365UrlSetting != null && !String.IsNullOrWhiteSpace(office365UrlSetting.ToString()))
                            serverURL = office365UrlSetting.ToString();
                    }
                    catch(Exception ex)
                    {

                    }
                    
					Common.WriteDeviceHistoryEntry("All", serverType, "Server is marked for scanning so will start Server Related Tasks", Common.LogLevel.Normal);
					CreateOffice365ServersCollection();
                    Common.InitStatusTable(myOffice365Servers);

                    StartO365Threads(false );

					//sleep for one minute to allow time for the collection to be made 
					Thread.Sleep(60 * 1000 * 1);

					Thread HourlyTasksThread = new Thread(new ThreadStart(HourlyTasks));
					HourlyTasksThread.CurrentCulture = c;
					HourlyTasksThread.IsBackground = true;
					HourlyTasksThread.Name = "HourlyTasks - O365";
					HourlyTasksThread.Priority = ThreadPriority.Normal;
					HourlyTasksThread.Start();
					Thread.Sleep(2000);

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
				//throw ex;

			}

		}
		int serverThreadCount = 0;
		int initialServerThreadCount = 0;
		System.Collections.ArrayList AliveServerMainThreads;
		private void StartO365Threads(bool killThreads = false)
		{
            /*
            int maxThreadCount = Common.getThreadCount("O365");
            int startThreads = 0;
            serverThreadCount = myOffice365Servers.Count / 3;
            if (serverThreadCount <= 1)
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

            Common.WriteDeviceHistoryEntry("All", "Exchange", "There are " + serverThreadCount + " threads open", Common.LogLevel.Normal);
            if (c == null)
                c = new CultureInfo("en-US");

            for (int i = startThreads; i < serverThreadCount; i++)
            {
                Thread monitorExchange = new Thread(() => MonitorO365Server(i));
                monitorExchange.CurrentCulture = c == null ? new CultureInfo("en-US") : c;  //Should only be null on our local copies if using wrapper
                monitorExchange.IsBackground = true;
                monitorExchange.Priority = ThreadPriority.Normal;
                monitorExchange.Name = i.ToString() + "-Exchange Monitoring";
                AliveServerMainThreads.Add(monitorExchange);
                monitorExchange.Start();
                Thread.Sleep(2000);
            }
            */
            
            int startThreads = 0;
            if (!killThreads)
                AliveServerMainThreads = new System.Collections.ArrayList();
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
                if(killThreads)
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

			Common.WriteDeviceHistoryEntry("All", serverType, "There are startThreads-" + startThreads + " threads open");
			Common.WriteDeviceHistoryEntry("All", serverType, "There are " + serverThreadCount + " threads open");
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
		
//		public MonitoredItems.Office365Server SelectServerToMonitor()
//		{
            

//            MonitoredItems.Office365Server SelectedServer = null;

//			try
//			{


//				DateTime tNow = DateTime.Now;
//				DateTime tScheduled;

//				DateTime timeOne;
//				DateTime timeTwo;


//				MonitoredItems.Office365Server ServerOne = null;
//				MonitoredItems.Office365Server ServerTwo = null;

//				RegistryHandler myRegistry = new RegistryHandler();
//				String ScanASAP = "";
//				//2494
//				String strSQL = "";
//				String ServerType = "Office365";
//				CommonDB db = new CommonDB();
//				String serverName = "";

//				try
//				{


//					strSQL = "Select svalue from ScanSettings where sname = 'Scan" + ServerType + "ASAP'";
//					DataTable dt = db.GetData(strSQL);
//					foreach (DataRow row in dt.Rows)
//					{
//						try
//						{
//							serverName = row[0].ToString();
//						}
//						catch (Exception ex)
//						{
//							continue;
//						}

//						for (int n = 0; n < myOffice365Servers.Count; n++)
//						{
//							ServerOne = myOffice365Servers.get_Item(n);
//							if (ServerOne.Name == serverName && ServerOne.IsBeingScanned == false && ServerOne.Enabled)
//							{
//								Common.WriteDeviceHistoryEntry("All", "Office365", serverName + " was marked 'Scan ASAP' so it will be scanned next.");

//								strSQL = "DELETE FROM ScanSettings where sname = 'Scan" + ServerType + "ASAP' and svalue='" + serverName + "'";
//								db.Execute(strSQL);

//								return ServerOne;
//							}

//						}
//					}

//				}
//				catch (Exception ex)
//				{

//				}

////2494
//				try
//				{
//					ScanASAP = myRegistry.ReadFromRegistry("ScanOffice365ASAP").ToString();
//				}
//				catch (Exception ex)
//				{
//					ScanASAP = "";
//				}


//				//Searches for the server marked as ScanASAP, if it exists
//				for (int n = 0; n < myOffice365Servers.Count; n++)
//				{
//					ServerOne = myOffice365Servers.get_Item(n);
//					if (ServerOne.Name == ScanASAP && ServerOne.IsBeingScanned == false && ServerOne.Enabled)
//					{
//						Common.WriteDeviceHistoryEntry("All", serverType, ScanASAP + " was marked 'Scan ASAP' so it will be scanned next.");
//						myRegistry.WriteToRegistry("ScanADASAP", "n/a");

//						//ServerOne.ScanASAP = true;

//						return ServerOne;
//					}

//				}


//				//Searches for the first enounter of a Not Responding server that is due for a scan
//				for (int n = 0; n < myOffice365Servers.Count; n++)
//				{
//					ServerOne = myOffice365Servers.get_Item(n);
//					if (ServerOne.Status == "Not Responding" && ServerOne.IsBeingScanned == false && ServerOne.Enabled)
//					{
//						tScheduled = ServerOne.NextScan;
//						if (DateTime.Compare(tNow, tScheduled) > 0)
//						{
//							Common.WriteDeviceHistoryEntry("All", serverType, "Selecting " + ServerOne.Name + " because the status is " + ServerOne.Status + ".  Next scheduled scan is at " + tScheduled.ToString());
//							return ServerOne;
//						}
//					}
//				}


//				//Searches for the first encounter of a server that has not been scanned yet
//				for (int n = 0; n < myOffice365Servers.Count; n++)
//				{
//					ServerOne = myOffice365Servers.get_Item(n);
//					if ((ServerOne.Status == "Not Scanned" || ServerOne.Status == "Master Service Stopped.") && ServerOne.IsBeingScanned == false && ServerOne.Enabled)
//					{
//						Common.WriteDeviceHistoryEntry("All", serverType, "Selecting " + ServerOne.Name + " because the status is " + ServerOne.Status + ".");
//						return ServerOne;
//					}
//				}


//				//Searches for all servers that are due for a scan
//				List<MonitoredItems.Office365Server> ScanCanidates = new List<MonitoredItems.Office365Server>();

//				foreach (MonitoredItems.Office365Server srv in myOffice365Servers)
//				{
//					if (srv.IsBeingScanned == false && ServerOne.Enabled)
//					{
//						tNow = DateTime.Now;
//						tScheduled = srv.NextScan;
//						if (DateTime.Compare(tNow, tScheduled) > 0)
//						{
//							ScanCanidates.Add(srv);
//						}
//					}
//				}

//				if (ScanCanidates.Count == 0)
//				{
//					Thread.Sleep(10000);
//					return null;
//				}



//				//Start with the first two servers
//				ServerOne = ScanCanidates.ElementAt(0);
//				if (ScanCanidates.Count > 1)
//					ServerTwo = ScanCanidates.ElementAt(1);

//				if (ScanCanidates.Count > 2)
//				{
//					try
//					{
//						for (int n = 2; n < ScanCanidates.Count - 1; n++)
//						{
//							timeOne = ServerOne.NextScan;
//							timeTwo = ServerTwo.NextScan;
//							if (DateTime.Compare(timeOne, timeTwo) < 0)
//							{
//								//time on one is earlier, so keep one
//								ServerTwo = ScanCanidates.ElementAt(n);
//							}
//							else
//							{
//								//time on two is ealier, so keep two
//								ServerOne = ScanCanidates.ElementAt(n);
//							}
//						}
//					}
//					catch (Exception ex)
//					{
//						Common.WriteDeviceHistoryEntry("All", serverType, "Error Selecting Server... " + ex.Message);
//					}
//				}

//				if (ServerTwo != null)
//				{
//					timeOne = ServerOne.NextScan;
//					timeTwo = ServerTwo.NextScan;

//					if (DateTime.Compare(timeOne, timeTwo) < 0)
//					{
//						SelectedServer = ServerOne;
//						tScheduled = ServerOne.NextScan;
//					}
//					else
//					{
//						SelectedServer = ServerTwo;
//						tScheduled = ServerTwo.NextScan;
//					}
//					tNow = DateTime.Now;
//				}
//				else
//				{
//					SelectedServer = ServerOne;
//					tScheduled = ServerOne.NextScan;
//				}

//				tScheduled = SelectedServer.NextScan;
//				if (DateTime.Compare(tNow, tScheduled) < 0)
//				{
//					if (SelectedServer.Status != "Not Scanned")
//					{
//						SelectedServer = null;
//					}
//				}
//				else
//				{
//					TimeSpan mySpan = tNow - tScheduled;
//				}
//			}
//			catch (Exception ex)
//			{
//				Common.WriteDeviceHistoryEntry("All", serverType, "Getting the server to monitor" + ex.Message.ToString(), Common.LogLevel.Normal);
//			}
//			return SelectedServer;

//		}
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
					MonitoredItems.Office365Server thisServer = Common.SelectServerToMonitor(myOffice365Servers) as MonitoredItems.Office365Server;
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

							string errorMessage = "";
							if (thisServer.ADFSMode && thisServer.ADFSRedirectTest == false)
								errorMessage = "ADFS Service Unavailable";
							if (results.Connected == false && thisServer.StatusCode == "Not Responding")
							{
                                thisServer.IncrementDownCount();
								DB.NotRespondingQueries(thisServer, thisServer.ServerType);
								DB.ProcessAlerts(AllTestResults, thisServer, thisServer.ServerType);
							}
							else
							{
                                thisServer.IncrementUpCount();

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
            //CommonDB db = new CommonDB();

            //SQLBuild objSQL = new SQLBuild();
            //objSQL.ifExistsSQLSelect = "SELECT * FROM Status WHERE TypeANDName='" + myServer.Name + "-" + ServerType + "'";
            //objSQL.onFalseDML = "INSERT INTO STATUS (NAME, STATUS, STATUSCODE, LASTUPDATE, TYPE, LOCATION, CATEGORY, TYPEANDNAME, DESCRIPTION, UserCount, ResponseTime, SecondaryRole,ResponseThreshold, " +
            //            "DominoVersion, OperatingSystem, NextScan, Details, CPU, Memory) VALUES ('" + myServer.Name + "', 'Maintenance', 'Maintenance', '" + DateTime.Now.ToString() + "','" + ServerType + "','" +
            //            myServer.Location + "','" + myServer.Category + "','" + myServer.Name + "-" + ServerType + "', 'Microsoft " + ServerType + " Server', 0, 0, '', " +
            //            "'" + myServer.ResponseThreshold + "', '" + serverType +"', '" + myServer.OperatingSystem + "', '" + myServer.NextScan + "', " +
            //            "'This server is in a scheduled maintenance period.  Monitoring is temporarily disabled.', 0, 0 )";

            //objSQL.onTrueDML = "UPDATE Status set Status='Maintenance', StatusCode='Maintenance', LastUpdate='" + DateTime.Now + "', Details='This server is in a scheduled maintenance period.  Monitoring is temporarily disabled.'," +
            //    " UserCount=0, CPU=0, Memory=0 WHERE TypeANDName='" + myServer.Name + "-" + ServerType + "'";

            //string sqlQuery = objSQL.GetSQL(objSQL);
            //db.Execute(sqlQuery);

            
VSNext.Mongo.Repository.Repository<VSNext.Mongo.Entities.Status> repo = new VSNext.Mongo.Repository.Repository<VSNext.Mongo.Entities.Status>(connectionString);
string TypeAndName = myServer.Name + "-" + ServerType;
FilterDefinition<VSNext.Mongo.Entities.Status> filterdef = repo.Filter.Where(i => i.TypeAndName == TypeAndName);
UpdateDefinition<VSNext.Mongo.Entities.Status> updatedef = default(UpdateDefinition<VSNext.Mongo.Entities.Status>);
updatedef = repo.Updater
    .Set(i => i.DeviceName, myServer.Name)
     .Set(i => i.CurrentStatus, "Maintenance")
     .Set(i => i.StatusCode, "Maintenance")
     .Set(i => i.LastUpdated, DateTime.Now)
    .Set(i => i.Category, myServer.Category)
    .Set(i => i.TypeAndName, TypeAndName)
    .Set(i => i.Description, "Microsoft " + ServerType + " Server.")
    .Set(i => i.Details, "This server is in a scheduled maintenance period.  Monitoring is temporarily disabled.")
    .Set(i => i.DeviceType, ServerType)
    .Set(i => i.UserCount, 0)
    .Set(i => i.Location, myServer.Location)
    .Set(i => i.LastUpdated, DateTime.Now)
    .Set(i => i.ResponseTime, 0)
    .Set(i => i.NextScan, myServer.NextScan)
    .Set(i => i.DominoVersion,  ServerType)
    .Set(i => i.OperatingSystem, myServer.OperatingSystem)
    .Set(i => i.CPU, 0)
    .Set(i => i.Memory, 0)
    .Set(i => i.DeviceId, myServer.ServerObjectID);
repo.Upsert(filterdef, updatedef);

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================


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

            List<VSNext.Mongo.Entities.Server> listOfServers = new List<Server>();
            List<VSNext.Mongo.Entities.Status> listOfStatus = new List<Status>();
            List<VSNext.Mongo.Entities.Credentials> listOfCredentials = new List<Credentials>();
            List<VSNext.Mongo.Entities.Location> listOfLocations = new List<Location>();
            List<VSNext.Mongo.Entities.Nodes> listOfNodes = new List<Nodes>();

            VSNext.Mongo.Repository.Repository<VSNext.Mongo.Entities.Server> repository = new VSNext.Mongo.Repository.Repository<VSNext.Mongo.Entities.Server>(DB.GetMongoConnectionString());
            FilterDefinition<VSNext.Mongo.Entities.Server> filterDef = repository.Filter.Eq(x => x.DeviceType, VSNext.Mongo.Entities.Enums.ServerType.Office365.ToDescription());

            ProjectionDefinition<VSNext.Mongo.Entities.Server> projectionDef = repository.Project
                .Include(x => x.Id)
                .Include(x => x.DeviceName)
                .Include(x => x.Category)
                .Include(x => x.OffHoursScanInterval)
                .Include(x => x.RetryInterval)
                .Include(x => x.ScanInterval)
                .Include(x => x.ResponseTime)
                .Include(x => x.CredentialsId)
                .Include(x => x.Mode)
                .Include(x => x.DirectorySyncServerName)
                .Include(x => x.DirectorySyncCredentialsId)
                .Include(x => x.LocationId)
                .Include(x => x.IsEnabled)
                .Include(x => x.DeviceType)
                .Include(x => x.CurrentNode)
                .Include(x => x.SimulationTests)
                .Include(x => x.NodeIds);


            listOfServers = repository.Find(filterDef, projectionDef).ToList();

            VSNext.Mongo.Repository.Repository<VSNext.Mongo.Entities.Status> repositoryStatus = new VSNext.Mongo.Repository.Repository<Status>(DB.GetMongoConnectionString());
            FilterDefinition<VSNext.Mongo.Entities.Status> filterDefStatus = repositoryStatus.Filter.Eq(x => x.DeviceType, VSNext.Mongo.Entities.Enums.ServerType.Office365.ToDescription());
            ProjectionDefinition<VSNext.Mongo.Entities.Status> projectionDefStatus = repositoryStatus.Project
                .Include(x => x.DeviceId)
                .Include(x => x.StatusCode)
                .Include(x => x.CurrentStatus)
                .Include(x => x.LastUpdated)
                .Include(x => x.DeviceType)
                .Include(x => x.DeviceName)
                .Include(x => x.Details);

            listOfStatus = repositoryStatus.Find(filterDefStatus, projectionDefStatus).ToList();

            VSNext.Mongo.Repository.Repository<VSNext.Mongo.Entities.Credentials> repositoryCredentials = new VSNext.Mongo.Repository.Repository<Credentials>(DB.GetMongoConnectionString());
            listOfCredentials = repositoryCredentials.Find(x => true).ToList();
            VSNext.Mongo.Repository.Repository <VSNext.Mongo.Entities.Location> repositoryLocation = new VSNext.Mongo.Repository.Repository<Location>(DB.GetMongoConnectionString());
            listOfLocations = repositoryLocation.Find(x => true).ToList();
            VSNext.Mongo.Repository.Repository<VSNext.Mongo.Entities.Nodes> repositoryNodes = new VSNext.Mongo.Repository.Repository<Nodes>(DB.GetMongoConnectionString());
            listOfNodes = repositoryNodes.Find(x => true).ToList();


            StringBuilder SQL = new StringBuilder();
			SQL.Append("select O365.ID,o365.Name,Category,ScanInterval,OffHoursScanInterval,ResponseThreshold,RetryInterval,UserName,PW,O365.ServerTypeId, " +
                "ST.ServerType,Mode,ServerName,Cred.UserId,Cred.Password ");
			if (nodeScan)
				SQL.Append(",L.Location Location ");
			else
				SQL.Append(",L.Location ");
			SQL.Append(" from O365Server O365 inner join ServerTypes ST on O365.ServerTypeid=ST.ID ");
			SQL.Append(" inner join O365Nodes ONDT on ONDT.O365ServerId=O365.Id ");
			SQL.Append(" inner join Nodes on Nodes.Id=ONDT.NodeId and Nodes.Name='" + NodeName + "'");
			SQL.Append(" inner join Locations L on Nodes.LocationId=L.Id ");
			SQL.Append(" left outer join Credentials Cred on Cred.Id=o365.CredentialsId ");

		
			SQL.Append(" WHERE enabled=1 ");
			//DataTable dtServers = DB.GetData(SQL.ToString());
			//Loop through servers
			if (listOfServers.Count > 0)
			{
				int updatedServers = 0;
				int newServers = 0;

				// adds/updates new servers
				for (int i = 0; i < listOfServers.Count; i++)
				{
					//DataRow DR = dtServers.Rows[i];
                    VSNext.Mongo.Entities.Server currServer = listOfServers[i];
                    //if the O365 server is not set to scan this node, skip it
                    if (currServer.NodeIds != null && currServer.NodeId.Count() > 0 && !currServer.NodeIds.Contains(listOfNodes.Where(x => x.Name == NodeName).FirstOrDefault().Id))
                        continue;

					MonitoredItems.Office365Server oldServer = myOffice365Servers.SearchByName(currServer.DeviceName);
					if (oldServer == null)
					{
						oldServer = new MonitoredItems.Office365Server();
						myOffice365Servers.Add(oldServer);
						newServers++;
					}
					else
					{
                        myOffice365Servers.Delete(currServer.DeviceName);
                        myOffice365Servers.Add(oldServer);
						updatedServers++;
					}

                    try
                    {

                        oldServer.IPAddress = serverURL;
                        oldServer.Name = currServer.DeviceName;
                        oldServer.ServerObjectID = currServer.Id;
                        if (currServer.CredentialsId != null)
                        {
                            try
                            {
                                var creds = listOfCredentials.First(x => x.Id == currServer.CredentialsId);
                                oldServer.UserName = creds.UserId;
                                oldServer.Password = Common.decodePasswordFromEncodedString(creds.Password, oldServer.Name);
                            }
                            catch (Exception ex)
                            {

                            }
                            
                        }

                        if (currServer.DirectorySyncCredentialsId != null)
                        {
                            try
                            {
                                var creds = listOfCredentials.First(x => x.Id == currServer.DirectorySyncCredentialsId);
                                oldServer.DirSyncUID = creds.UserId;
                                oldServer.DirSyncPWD = Common.decodePasswordFromEncodedString(creds.Password, oldServer.Name);
                            }
                            catch (Exception ex)
                            {

                            }

                        }

                        oldServer.Mode = currServer.Mode;
                        oldServer.DirSyncServerName = currServer.DirectorySyncServerName;
                        oldServer.VersionNo = "NA";
                        oldServer.ADFSMode = false;  //set it to false initially
                        oldServer.ADFSRedirectTest = false;  //set it to false initially

                        oldServer.ResponseThreshold = long.Parse(currServer.ResponseTime.HasValue ? currServer.ResponseTime.Value.ToString() : "0");
                        oldServer.ScanInterval = currServer.ScanInterval.Value;
                        oldServer.OffHoursScanInterval = currServer.OffHoursScanInterval.Value;
                        oldServer.RetryInterval = currServer.RetryInterval.Value;

                        try
                        {
                            VSNext.Mongo.Entities.Status currStatus = listOfStatus.First(x => x.DeviceId == currServer.Id);
                            if (currStatus != null)
                            {
                                oldServer.LastScan = currStatus.LastUpdated.HasValue ? currStatus.LastUpdated.Value : DateTime.Now.AddMinutes(-30);
                                oldServer.Status = String.IsNullOrWhiteSpace(currStatus.CurrentStatus) ? "Not Scanned" : currStatus.CurrentStatus;
                                oldServer.StatusCode = String.IsNullOrWhiteSpace(currStatus.StatusCode) ? "Maintenance" : currStatus.StatusCode;
                                oldServer.ResponseDetails = String.IsNullOrWhiteSpace(currStatus.Details) ? "This server has not yet been scanned." : currStatus.Details;
                            }
                            else
                            {
                                oldServer.LastScan = DateTime.Now.AddMinutes(-30);
                                oldServer.Status = "Not Scanned";
                                oldServer.StatusCode = "Maintenance";
                                oldServer.ResponseDetails = "This server has not yet been scanned.";
                            }
                        }
                        catch (Exception ex)
                        {
                            oldServer.LastScan = DateTime.Now.AddMinutes(-30);
                            oldServer.Status = "Not Scanned";
                            oldServer.StatusCode = "Maintenance";
                            oldServer.ResponseDetails = "This server has not yet been scanned.";
                        }

                        oldServer.ServerType = currServer.DeviceType;
                        //if(currServer.LocationId != null)
                        //{
                        //    try
                        //    {
                        //        oldServer.Location = listOfLocations.Find(x => x.Id == currServer.LocationId).LocationName;
                        //    }
                        //    catch(Exception ex)
                        //    {

                        //    }
                        //}

                        try
                        {
                            oldServer.Location = listOfNodes.Where(x => x.Name == NodeName).First().Location;
                        }
                        catch(Exception ex)
                        {

                        }

                        if (NodeName != "")
                            oldServer.Category = NodeName;
                        else
                            oldServer.Category = currServer.Category;
                        oldServer.Enabled = true;

                        Common.WriteDeviceHistoryEntry("All", serverType, "In SetO365ServerSettings: 1", Common.LogLevel.Normal);

                        oldServer.EnableAutoDiscoveryTest = false;
                        oldServer.EnableMailFlow = false;
                        oldServer.EnableInboxTest = false;
                        oldServer.EnableOWATest = false;
                        oldServer.EnableSMTPTest = false;
                        oldServer.EnableMAPIConnectivityTest = false;
                        oldServer.EnableCreateTaskTest = false;
                        oldServer.EnableCreateFolderTest = false;
                        oldServer.EnableOneDriveUploadTest = false;
                        oldServer.EnableOneDriveDownloadTest = false;
                        oldServer.EnableCreateSiteTest = false;
                        oldServer.EnableCreateCalEntryTest = false;
                        oldServer.DirSyncExportTest = false;
                        oldServer.DirSyncImportTest = false;
                        foreach (NameValuePair row in currServer.SimulationTests)
                        {
                            Common.WriteDeviceHistoryEntry("All", serverType, "In SetO365ServerSettings: 1" + row.Name.ToString(), Common.LogLevel.Normal);

                            switch (row.Name.ToString())
                            {
                                case "Mail Flow":
                                    oldServer.EnableMailFlow = true;
                                    oldServer.MailFlowThreshold = Convert.ToInt32(row.Value);
                                    break;
                                case "Inbox":
                                    oldServer.EnableInboxTest = true;
                                    oldServer.InboxThreshold = Convert.ToInt32(row.Value);
                                    break;
                                case "OWA":
                                    oldServer.EnableOWATest = true;
                                    oldServer.ComposeEmailThreshold = Convert.ToInt32(row.Value);
                                    break;
                                case "SMTP":
                                    oldServer.EnableSMTPTest = true;
                                    //oldServer.ComposeEmailThreshold = Convert.ToInt32(row["ResponseThreshold"].ToString());
                                    break;
                                case "POP3":
                                    oldServer.EnablePOPTest = Convert.ToBoolean(row.Value);
                                    break;
                                case "IMAP":
                                    oldServer.EnableIMAPTest = Convert.ToBoolean(row.Value);
                                    break;
                                case "Auto Discovery":
                                    oldServer.EnableAutoDiscoveryTest = true;
                                    break;
                                case "MAPI Connectivity":
                                    oldServer.EnableMAPIConnectivityTest = true;
                                    break;
                                case "Create Task":
                                    oldServer.EnableCreateTaskTest = true;
                                    oldServer.CreateTaskThreshold = Convert.ToInt32(row.Value);
                                    break;
                                case "Create Folder":
                                    oldServer.EnableCreateFolderTest = true;
                                    oldServer.CreateFolderThreshold = Convert.ToInt32(row.Value);
                                    break;
                                case "OneDrive Upload":
                                    oldServer.EnableOneDriveUploadTest = true;
                                    oldServer.OneDriveUplaodThreshold = Convert.ToInt32(row.Value);
                                    break;
                                case "OneDrive Download":
                                    oldServer.EnableOneDriveDownloadTest = true;
                                    oldServer.OneDriveDownlaodThreshold = Convert.ToInt32(row.Value);
                                    break;
                                //case "OneDrive Search":
                                //    oldServer.EnableOneDriveSearchTest = Convert.ToBoolean(row["EnableSimulationTests"].ToString());
                                //    oldServer.OneDriveSearchThreshold = Convert.ToInt32(row["ResponseThreshold"].ToString());
                                //    break;
                                case "Create Site":
                                    oldServer.EnableCreateSiteTest = true;
                                    oldServer.CreateSiteThreshold = Convert.ToInt32(row.Value);
                                    break;
                                case "Create Calendar":
                                    oldServer.EnableCreateCalEntryTest = true;
                                    oldServer.CreateCalEntryThreshold = Convert.ToInt32(row.Value);
                                    break;
                                //case "Resolve User":
                                //    oldServer.EnableResolveUserTest = Convert.ToBoolean(row["EnableSimulationTests"].ToString());
                                //    oldServer.ResolveUserThreshold = Convert.ToInt32(row["ResponseThreshold"].ToString());
                                //    break;
                                case "Dir Sync Imp/Export":
                                    oldServer.DirSyncExportTest = true;
                                    oldServer.DirSyncExportThreshold = Convert.ToInt32(row.Value);
                                    oldServer.DirSyncImportTest = true;
                                    oldServer.DirSyncImportThreshold = Convert.ToInt32(row.Value);
                                    break;
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        Common.WriteDeviceHistoryEntry("All", serverType, "Error in SetO365ServerSettings: " + ex.ToString(), Common.LogLevel.Normal);
                    }



                }

				Common.WriteDeviceHistoryEntry("All",serverType , "There are " + myOffice365Servers.Count + " servers in the collection.  " + newServers + " were new and " + updatedServers + " were updated.");
			}
			else
			{
				myOffice365Servers = new MonitoredItems.Office365ServersCollection();
			}


			//At this point we have all Servers with ALL the information(including Threshold settings)
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
            Thread HourlyTasksUpDownTimesThread = null;
            int hour = -1;
			while (true)
			{
				if (hour == -1 || hour != DateTime.Now.Hour)
				{
					if (HourlyTasksThread != null && HourlyTasksThread.IsAlive)
					{
						Common.WriteDeviceHistoryEntry(serverType, DummyServerForLogs.Name, "The thread for Hourly Tasks is still running from the previous hour, will let that thread finish without terminating.", commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);
						//HourlyTasksThread.Abort();
                        //dont kill the running thread but skip this cycle.
                        goto skip;
					}

					HourlyTasksThread = new Thread(() => HourlyTasksMainThread(DummyServerForLogs));
					HourlyTasksThread.CurrentCulture = c;
					HourlyTasksThread.IsBackground = true;
					HourlyTasksThread.Priority = ThreadPriority.Normal;
					HourlyTasksThread.Name = "HourlyTaskWorkerThread - O365";
					HourlyTasksThread.Start();
					//Thread.Sleep(60 * 60 * 1000); //sleeps for 1 hour
                skip:

                    HourlyTasksUpDownTimesThread = new Thread(() => HourlyTasksUpDownMainThread(DummyServerForLogs));
                    HourlyTasksUpDownTimesThread.CurrentCulture = c;
                    HourlyTasksUpDownTimesThread.IsBackground = true;
                    HourlyTasksUpDownTimesThread.Priority = ThreadPriority.Normal;
                    HourlyTasksUpDownTimesThread.Name = "HourlyTasksUpDownTimesThread - O365";
                    HourlyTasksUpDownTimesThread.Start();

                    hour = DateTime.Now.Hour;
				}
            
				//sleep for 5 mins
				Thread.Sleep(1000 * 60 * 5);

			}
		}

        private void HourlyTasksUpDownMainThread(MonitoredItems.Office365Server DummyServerForLogs)
        {

            Common.WriteDeviceHistoryEntry("All", serverType, " Hourly Task Up Down started.", Common.LogLevel.Normal);
            MonitoredItems.Office365Server testServer = null;

            try
            {
                TestResults AllTestResults = new TestResults();
                foreach (MonitoredItems.Office365Server myServer in myOffice365Servers)
                    Common.RecordUpAndDownTimes(myServer, ref AllTestResults);


                CommonDB db = new CommonDB();
                db.ProcessMongoStatements(AllTestResults, DummyServerForLogs);
            }
            catch (Exception ex)
            {
                Common.WriteDeviceHistoryEntry(testServer.ServerType, testServer.Name, " Error in Hourly Task Up Down. Loop for all servers. Error : " + ex.Message, Common.LogLevel.Normal);
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
                    CommonDB DB = new CommonDB();
					using (results)
					{
						Common.WriteDeviceHistoryEntry(testServer.ServerType, testServer.Name, " Hourly Task started.", Common.LogLevel.Normal);
						//Office365Common.getMobileUsersHourly(testServer, ref AllTestResults, results);
						Office365Common.getUserswithLicencesandServices(testServer, ref AllTestResults, results);
                        DB.ProcessMongoStatements(AllTestResults, DummyServerForLogs);
                         AllTestResults = new TestResults();
                        Office365Common.getMsolUsers(testServer, ref AllTestResults, results);
                        DB.ProcessMongoStatements(AllTestResults, DummyServerForLogs);
                         AllTestResults = new TestResults();
                        Office365Common.getMsolGroups(testServer, ref AllTestResults, results);
                        DB.ProcessMongoStatements(AllTestResults, DummyServerForLogs);
                        AllTestResults = new TestResults();
                        Office365Common.getServiceStatus(testServer, ref AllTestResults, results);
                        DB.ProcessMongoStatements(AllTestResults, DummyServerForLogs);
                        AllTestResults = new TestResults();
                        Office365Common.getMailboxeDetails(testServer, ref AllTestResults, results);
                        DB.ProcessMongoStatements(AllTestResults, DummyServerForLogs);
						//Office365Common.Deletesummarystatsdata(testServer, ref AllTestResults, testServer.ServerType);
						//Common.CommonDailyTasks(testServer, ref AllTestResults, testServer.ServerType);
						//Office365Common.getMailBoxInfo(testServer, ref AllTestResults, testServer.VersionNo.ToString(), DummyServerForLogs.Name, myOffice365Servers, results);

					}

					GC.Collect();
                    //while (testServer.IsBeingScanned)
                    //{
                    //    string doSomething;
                    //}
					
					
					
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
                    CommonDB DB = new CommonDB();
					using (results)
					{

						Common.WriteDeviceHistoryEntry(testServer.ServerType, testServer.Name, " Daily Task started.", Common.LogLevel.Normal);
						//Office365Common.getMailBoxInfo(testServer, ref AllTestResults, results);
                        ////Office365Common.getMsolCompanyInfo(testServer, ref AllTestResults, results);
                        //Office365Common.getMsolUsers(testServer, ref AllTestResults, results);
                        //Office365Common.getMsolGroups(testServer, ref AllTestResults, results);
                        //Office365Common.getServiceStatus(testServer, ref AllTestResults, results);
						Office365Common.getMailboxes(testServer, ref AllTestResults, results);
                        DB.ProcessMongoStatements(AllTestResults, DummyServerForLogs);
                        AllTestResults = new TestResults();
						Office365Common.getMailStatusInfo(testServer, ref AllTestResults, results);
                        DB.ProcessMongoStatements(AllTestResults, DummyServerForLogs);
					//	Office365Common.getUserswithLicencesandServices(testServer, ref AllTestResults, results);

					}

					GC.Collect();
					//while (testServer.IsBeingScanned)
					//{
					//    string doSomething;
					//}
					//let the main thread sql get executed. 2 mins should be enough.
					//Thread.Sleep(120 * 1000);
					
					

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
					Common.WriteDeviceHistoryEntry("All", serverType, " Daily Task No server found to scan ", Common.LogLevel.Normal);
				}
			}
			catch (Exception ex)
			{
				Common.WriteDeviceHistoryEntry("All", serverType, " Daily Task Ended with an error: " + ex.Message.ToString(), Common.LogLevel.Normal);
			}

		}



		#endregion

		public void RefreshOffice365Collction()
		{

			CreateOffice365ServersCollection();
            foreach (Thread th in AliveServerMainThreads)
            {
                    if (th.IsAlive)
                        th.Abort();
            }
             serverThreadCount = 0;
             initialServerThreadCount = 0;
             AliveServerMainThreads = new System.Collections.ArrayList();
			StartO365Threads(true );

			
		}
       

	}
}
