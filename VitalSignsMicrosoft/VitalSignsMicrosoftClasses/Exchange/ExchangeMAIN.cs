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

namespace VitalSignsMicrosoftClasses
{
	
	public class ExchangeMAIN
	{
		string CurrentCulture = "en-US";
		string CultureStringName = "CultureString";
		CultureInfo c;
		string listOfIdsForExchange = "";
		string listOfIdsForDag = "";
		string listOfIdsForLync = "";
		MonitoredItems.ExchangeServersCollection myExchangeServers;
		MonitoredItems.ExchangeMailProbesCollection myExchangeMailProbes;
		MonitoredItems.ExchangeServersCollection myDagCollection;

		private Mutex ExchangeMutex = new Mutex();
		private Mutex DAGMutex = new Mutex();
		private Mutex LyncMutex = new Mutex();


		public void StartProcess(dynamic MicrosoftHelperObj)
		{
			try
			{
				Common.initHelperClasses(MicrosoftHelperObj);
			}
			catch (Exception ex)
			{
				Common.WriteDeviceHistoryEntry("All", "Exchange", "Error setting Helper Class", Common.LogLevel.Normal);
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
                //Thread.Sleep(5000);
				Common.WriteDeviceHistoryEntry("All", "Exchange", "Exchagne Service is starting up", Common.LogLevel.Normal);
                //myExchangeServers = CreateExchangeServersCollection();
                //Thread.Sleep(5000);
                //Sets the log level
                Common.setLogLevel();

				try
				{
					VSFramework.RegistryHandler mySettigns = new VSFramework.RegistryHandler();
					mySettigns.WriteToRegistry("VS Exchange Service Start", DateTime.Now.ToString());
				}
				catch
				{
					Common.WriteDeviceHistoryEntry("All", "Exchange", "Error updating start time for Exchange Service in settings table", Common.LogLevel.Normal);
				}

				//creates colelction and starts thread to monitor changes for the colelction
				CommonDB db = new CommonDB();

				//if (db.RecordExists("SELECT * FROM SelectedFeatures sf WHERE sf.FeatureID in (SELECT ID FROM Features WHERE Name='Exchange')"))
                if(true)
				{
					Common.WriteDeviceHistoryEntry("All", "Exchange", "Exchange is marked for scanning so will start Exchange Related Tasks", Common.LogLevel.Normal);
                   // Thread.Sleep(5000);
                    CreateExchangeServersCollection();
                    //Thread.Sleep(5000);
                    Common.InitStatusTable(myExchangeServers);
					StartExchangeThreads(true);

					//CreateExchangeMailProbeCollection();
					////set the status for all mail probes
					//InitStatusTable(myExchangeMailProbes);


                    
                    
					StartMailProbeThreads();
                    

					CreateExchangeDAGCollection();
					Common.InitStatusTable(myDagCollection);
					DeleteOldDagDataOnStart();
					StartDAGThreads();


                    
                    Thread ExchangeDevices = new Thread(new ThreadStart(ExchangeDevicesLoop));
                    ExchangeDevices.CurrentCulture = c;
                    ExchangeDevices.IsBackground = true;
                    ExchangeDevices.Priority = ThreadPriority.Normal;
                    ExchangeDevices.Name = "Exchange Devices Main Thread - Exchange";
                    ExchangeDevices.Start();
                    Thread.Sleep(2000);


                    

				}
				else
				{
					myExchangeServers = null;
					Common.WriteDeviceHistoryEntry("All", "Exchange", "Exchange is not marked for scanning", Common.LogLevel.Normal);
				 }


                /*
				if (db.RecordExists("SELECT * FROM SelectedFeatures sf WHERE sf.FeatureID in (SELECT ID FROM Features WHERE Name='Skype for Business')"))
				{
					Common.WriteDeviceHistoryEntry("All", "Skype for Business", "Skype for Business is marked for scanning so will start Skype for Business Related Tasks", Common.LogLevel.Normal);

					CreateLyncServersCollection();
					InitStatusTable(myLyncServers);
					StartLyncThreads();
				}
				else
				{
					myLyncServers = null;
					Common.WriteDeviceHistoryEntry("All", "Skype for Business", "Skype for Business is not marked for scanning", Common.LogLevel.Normal);

				}
                */
				//sleep for one minute to allow time for the collection to be made 
				Thread.Sleep(60 * 1000 * 1);

				Thread HourlyTasksThread = new Thread(new ThreadStart(HourlyTasks));
				HourlyTasksThread.CurrentCulture = c;
				HourlyTasksThread.IsBackground = true;
				HourlyTasksThread.Name = "HourlyTasks - Exchange";
				HourlyTasksThread.Priority = ThreadPriority.Normal;
				HourlyTasksThread.Start();
				Thread.Sleep(2000);


				Thread DailyTasksThread = new Thread(new ThreadStart(DailyTasks));
				DailyTasksThread.CurrentCulture = c;
				DailyTasksThread.IsBackground = true;
				DailyTasksThread.Priority = ThreadPriority.Normal;
				DailyTasksThread.Name = "DailyTasks - Exchange";
				DailyTasksThread.Start();
				Thread.Sleep(2000);
                
				Common.WriteDeviceHistoryEntry("All", "Exchange", "All Processes are started in startProcess", Common.LogLevel.Normal);
			}
			catch (Exception ex)
			{
				Common.WriteDeviceHistoryEntry("All", "Exchange", "Error starting StartProcess exception CreateExchangeServersCollection: " + ex.Message.ToString() + "..." + ex.StackTrace.ToString(), Common.LogLevel.Normal);
                Thread.Sleep(5000);
                throw ex;

			}

		}

		protected void InitStatusTable(MonitoredItems.ExchangeServersCollection collection)
		{
			try
			{
				String type = "";
				CommonDB db = new CommonDB();
				if (collection.Count > 0)
					type = collection.get_Item(0).ServerType;
				
				if (type != "")
				{
					foreach (MonitoredItems.ExchangeServer server in collection)
					{
						String sql = "IF NOT EXISTS(SELECT * FROM Status WHERE TypeANDName = '" + server.Name + "-" + type + "') BEGIN " +
							"INSERT INTO Status ( Type, Location, Category, Name, Status, Details, Description, TypeANDName, StatusCode ) VALUES " + 
							" ('" + type + "', '" + server.Location + "', '" + server.Category + "', '" + server.Name + "', '" + server.Status + "', 'This server has not yet been scanned.', " +
							"'Microsoft " + type + " Server', '" + server.Name + "-" + type + "', '" + server.StatusCode + "') END";
						
						db.Execute(sql);
					}
					Common.WriteDeviceHistoryEntry("All", "Exchange", type + " Servers are marked as Not Scanned", Common.LogLevel.Normal);

				}
			}
			catch (Exception ex)
			{
				Common.WriteDeviceHistoryEntry("All", "Exchange", "Error in init status.  Error: " + ex.Message, Common.LogLevel.Normal);
			}

		}

		#region Collections

		#region ExchangeColl
		private void CreateExchangeServersCollection()
		{
			//Fetch all servers
			if (myExchangeServers == null)
				myExchangeServers = new MonitoredItems.ExchangeServersCollection();

            CommonDB DB = new CommonDB();
            List<VSNext.Mongo.Entities.Server> listOfServers = new List<Server>();
            List<VSNext.Mongo.Entities.Status> listOfStatus = new List<Status>();
            List<VSNext.Mongo.Entities.Credentials> listOfCredentials = new List<Credentials>();
            List<VSNext.Mongo.Entities.Location> listOfLocations = new List<Location>();
            string NodeName = null;
            try
            {
                NodeName = ConfigurationManager.AppSettings["VSNodeName"].ToString();

                VSNext.Mongo.Repository.Repository<VSNext.Mongo.Entities.Server> repository = new VSNext.Mongo.Repository.Repository<VSNext.Mongo.Entities.Server>(DB.GetMongoConnectionString());
                FilterDefinition<VSNext.Mongo.Entities.Server> filterDef = repository.Filter.Eq(x => x.DeviceType, VSNext.Mongo.Entities.Enums.ServerType.Exchange.ToDescription());

                ProjectionDefinition<VSNext.Mongo.Entities.Server> projectionDef = repository.Project
                    .Include(x => x.Id)
                    .Include(x => x.DeviceName)                    
                    .Include(x => x.Category)
                    .Include(x => x.OffHoursScanInterval)
                    .Include(x => x.RetryInterval)
                    .Include(x => x.ScanInterval)
                    .Include(x => x.IPAddress)
                    .Include(x => x.SubmissionQueueThreshold)
                    .Include(x => x.PosionQueueThreshold)
                    .Include(x => x.UnreachableQueueThreshold)
                    .Include(x => x.ShadowQueueThreshold)
                    .Include(x => x.TotalQueueThreshold)
                    .Include(x => x.SoftwareVersion)
                    .Include(x => x.ResponseTime)
                    .Include(x => x.CredentialsId)
                    .Include(x => x.ConsecutiveOverThresholdBeforeAlert)
                    .Include(x => x.ConsecutiveFailuresBeforeAlert)
                    .Include(x => x.EnableLatencyTest)
                    .Include(x => x.CpuThreshold)
                    .Include(x => x.MemoryThreshold)
                    .Include(x => x.AuthenticationType)
                    .Include(x => x.LocationId)
                    .Include(x => x.DeviceType)
                    .Include(x => x.CurrentNode)
                    .Include(x => x.SimulationTests)
                    .Include(x => x.ArePrerequisitesDone)
                    .Include(x => x.ActiveSyncCredentialsId);


                listOfServers = repository.Find(filterDef, projectionDef).ToList();

                VSNext.Mongo.Repository.Repository<VSNext.Mongo.Entities.Status> repositoryStatus = new VSNext.Mongo.Repository.Repository<Status>(DB.GetMongoConnectionString());
                FilterDefinition<VSNext.Mongo.Entities.Status> filterDefStatus = repositoryStatus.Filter.Eq(x => x.DeviceType, VSNext.Mongo.Entities.Enums.ServerType.Exchange.ToDescription());
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
                VSNext.Mongo.Repository.Repository<VSNext.Mongo.Entities.Location> repositoryLocation = new VSNext.Mongo.Repository.Repository<Location>(DB.GetMongoConnectionString());
                listOfLocations = repositoryLocation.Find(x => true).ToList();
            }
            catch(Exception ex)
            {
                Common.WriteDeviceHistoryEntry("All", "Exchange", "Exception in CreateExchangeServersCollection when getting the data from the db. Exception: " + ex.Message.ToString(), Common.LogLevel.Normal);
            }
            
            listOfIdsForExchange = String.Join(",", listOfServers.Select(x => x.Id).ToList());
			//Loop through servers
			MonitoredItems.ExchangeThresholdSettings ExchgThreshold = new MonitoredItems.ExchangeThresholdSettings();
			if (listOfServers.Count > 0)
			{
				List<string> ServerNameList = new List<string>();

				int updatedServers = 0;
				int newServers = 0;
				int removedServers = 0;

				for (int i = 0; i < listOfServers.Count; i++)
				{
					VSNext.Mongo.Entities.Server entity = listOfServers[i];
					ServerNameList.Add(entity.DeviceName.ToString());

					MonitoredItems.ExchangeServer myExchangeServer = null;// = new MonitoredItems.ExchangeServer();
                    VSNext.Mongo.Entities.Status statusEntry = listOfStatus.Find(x => x.DeviceId == entity.Id);
					//Checks to see if the server is newly added or exists.  Adds if it is new
					try
					{
                        try
                        {
                            myExchangeServer = myExchangeServers.SearchByName(entity.DeviceName.ToString());
                        }
                        catch(Exception ex)
                        {

                        }

						if (myExchangeServer == null)
						{
                            //New server.  Set inits and add to collection

                            myExchangeServer = new MonitoredItems.ExchangeServer();
                            myExchangeServer.Role = new String[0];
                            myExchangeServer.ServerType = "Exchange";

                            myExchangeServer.LastScan = statusEntry == null || !statusEntry.LastUpdated.HasValue || statusEntry.LastUpdated.ToString() == "" ? DateTime.Now.AddHours(-1) : statusEntry.LastUpdated.Value;
                            myExchangeServer.Status = statusEntry == null || statusEntry.CurrentStatus.ToString() == "" ? "Not Scanned" : statusEntry.CurrentStatus;
                            myExchangeServer.StatusCode = statusEntry == null || statusEntry.StatusCode.ToString() == "" ? "Maintenance" : statusEntry.StatusCode;

                            myExchangeServers.Add(myExchangeServer);
							newServers++;

						}
						else
						{
							updatedServers++;
						}
					}
					catch (Exception ex)
					{
                        Common.WriteDeviceHistoryEntry("All", "Exchange", "Exception in CreateExchangeServersCollection when init new server. Exception: " + ex.Message.ToString(), Common.LogLevel.Normal);
                    }

                    ExchgThreshold = new MonitoredItems.ExchangeThresholdSettings();
                    ExchgThreshold.PoisonQThreshold = entity.PosionQueueThreshold.HasValue ? entity.PosionQueueThreshold.Value : -1;
                    ExchgThreshold.SubQThreshold = entity.SubmissionQueueThreshold.HasValue ? entity.SubmissionQueueThreshold.Value : -1;
                    ExchgThreshold.TotalQThreshold = entity.TotalQueueThreshold.HasValue ? entity.TotalQueueThreshold.Value : -1;
                    ExchgThreshold.UnReachableQThreshold = entity.UnreachableQueueThreshold.HasValue ? entity.UnreachableQueueThreshold.Value : -1;
                    ExchgThreshold.ShadowQThreshold = entity.ShadowQueueThreshold.HasValue ? entity.ShadowQueueThreshold.Value : -1;
                    myExchangeServer.ThresholdSetting = ExchgThreshold;



                    myExchangeServer.ServerObjectID = entity.Id;
                    myExchangeServer.IPAddress = entity.IPAddress;
                    myExchangeServer.Name = entity.DeviceName;

                    try
                    {
                        var creds = listOfCredentials.Where(x => x.Id == entity.CredentialsId).First();
                        myExchangeServer.UserName = creds.UserId;
                        myExchangeServer.Password = decodePasswordFromEncodedString(creds.Password, myExchangeServer.Name);
                    }                       
                    catch(Exception ex)
                    {
                        
                    }

                    try
                    {
                        var location = listOfLocations.Where(x => x.Id == entity.LocationId).First();
                        myExchangeServer.Location = location.LocationName;
                    }
                    catch (Exception ex)
                    {

                    }

                    try
                    {
                        var status = listOfStatus.Where(x => x.DeviceId == entity.Id).First();
                        myExchangeServer.Status = status.CurrentStatus;
                        myExchangeServer.StatusCode = status.StatusCode;
                        myExchangeServer.LastScan = status.LastUpdated.HasValue ? status.LastUpdated.Value : DateTime.Now.AddHours(-1);
                        myExchangeServer.ResponseDetails = status.Details;
                    }
                    catch(Exception ex)
                    {

                    }

                    myExchangeServer.ResponseThreshold = entity.ResponseTime.HasValue ? entity.ResponseTime.Value : -1;
                    myExchangeServer.ScanInterval = entity.ScanInterval.HasValue ? entity.ScanInterval.Value : 8;
                    myExchangeServer.OffHoursScanInterval = entity.OffHoursScanInterval.HasValue ? entity.OffHoursScanInterval.Value : 10;
                    myExchangeServer.RetryInterval = entity.RetryInterval.HasValue ? entity.RetryInterval.Value : 3;
                    myExchangeServer.CPU_Threshold = entity.CpuThreshold.HasValue ? entity.CpuThreshold.Value : .9;
                    myExchangeServer.Memory_Threshold = entity.MemoryThreshold.HasValue ? entity.MemoryThreshold.Value : .9;

                    //myExchangeServer.DAGScan = DR["ScanDAGHealth"].ToString() == "" ? fals

                    /*e : bool.Parse(DR["ScanDAGHealth"].ToString());
                    //myExchangeServer.VersionNo = DR["VersionNo"].ToString();
                    myExchangeServer.CASActiveSync = DR["CASActiveSync"].ToString() == "" ? false : Convert.ToBoolean(DR["CASActiveSync"]);
                    MyExchangeServer.CASAutoDiscovery = DR["CASAutoDiscovery"].ToString() == "" ? false : Convert.ToBoolean(DR["CASAutoDiscovery"]);
                    MyExchangeServer.CASECP = DR["CASECP"].ToString() == "" ? false : Convert.ToBoolean(DR["CASECP"]);
                    MyExchangeServer.CASEWS = DR["CASEWS"].ToString() == "" ? false : Convert.ToBoolean(DR["CASEWS"]);
                    MyExchangeServer.CASImap = DR["CASImap"].ToString() == "" ? false : Convert.ToBoolean(DR["CASImap"]);
                    MyExchangeServer.CASOAB = DR["CASOAB"].ToString() == "" ? false : Convert.ToBoolean(DR["CASOAB"]);
                    MyExchangeServer.CASOARPC = DR["CASOARPC"].ToString() == "" ? false : Convert.ToBoolean(DR["CASOARPC"]);
                    MyExchangeServer.CASOWA = DR["CASOWA"].ToString() == "" ? false : Convert.ToBoolean(DR["CASOWA"]);
                    MyExchangeServer.CASPop3 = DR["CASPop3"].ToString() == "" ? false : Convert.ToBoolean(DR["CASPop3"]);
                    MyExchangeServer.CASSmtp = DR["CASSmtp"].ToString() == "" ? false : Convert.ToBoolean(DR["CASSmtp"]);
                    MyExchangeServer.ActiveSyncUserName = DR["ASUserId"].ToString();
                    MyExchangeServer.ActiveSyncPassword = decodePasswordFromEncodedString(DR["ASPassword"].ToString(), MyExchangeServer.Name);
                    */

                    myExchangeServer.ServerDaysAlert = entity.ConsecutiveOverThresholdBeforeAlert.HasValue ? "" + entity.ConsecutiveOverThresholdBeforeAlert.Value : "3";
                    myExchangeServer.FailureThreshold = entity.ConsecutiveFailuresBeforeAlert.HasValue ? entity.ConsecutiveFailuresBeforeAlert.Value : 3;
                    myExchangeServer.Category = entity.Category;
                    /*
                    myExchangeServer.LatencyRedThreshold = (DR["LatencyRedThreshold"] == null || DR["LatencyRedThreshold"].ToString() == "") ? 20000 : int.Parse(DR["LatencyRedThreshold"].ToString());
                    myExchangeServer.LatencyYellowThreshold = (DR["LatencyYellowThreshold"] == null || DR["LatencyYellowThreshold"].ToString() == "") ? 20000 : int.Parse(DR["LatencyYellowThreshold"].ToString());
                    MyExchangeServer.EnableLatencyTest = (DR["EnableLatencyTest"] == null || DR["EnableLatencyTest"].ToString() == "") ? false : Convert.ToBoolean(DR["EnableLatencyTest"]);
                    */
                    myExchangeServer.Enabled = true;
                    //myExchangeServer.InsufficentLicenses = entity.CurrentNode != null && entity.CurrentNode == "-1" ? true : false;
                    myExchangeServer.InsufficentLicenses = entity.CurrentNode == null || entity.CurrentNode != NodeName;
                    myExchangeServer.CurrentNode = entity.CurrentNode;
                    myExchangeServer.AuthenticationType = entity.AuthenticationType != null && entity.AuthenticationType != "" ? entity.AuthenticationType : "Default";
                    myExchangeServer.IsPrereqsDone = entity.ArePrerequisitesDone.HasValue ? entity.ArePrerequisitesDone.Value : false;
                    // MyExchangeServer.TestId = int.Parse(DR["TestId"].ToString());
                    //MyExchangeServer.CASUserName = DR["CASUserId"].ToString();
                    //MyExchangeServer.CASPassword = decodePasswordFromEncodedString(DR["CASPassword"].ToString(), MyExchangeServer.Name);
                    //MyExchangeServer.URLs =  DR["URLs"].ToString();
                    Common.WriteDeviceHistoryEntry("All", myExchangeServer.ServerType, "In SetExchangeServerSettings: 1", Common.LogLevel.Normal);


                    if(entity.SimulationTests != null)
                    {

                        if (entity.SimulationTests.Where(x => x.Name == "SMTP").Count() > 0)
                            myExchangeServer.CASSmtp = true;
                        else
                            myExchangeServer.CASSmtp = false;

                        if (entity.SimulationTests.Where(x => x.Name == "Outlook Anywhere").Count() > 0)
                            myExchangeServer.CASEWS = true;
                        else
                            myExchangeServer.CASEWS = false;

                        if (entity.SimulationTests.Where(x => x.Name == "OWA").Count() > 0)
                            myExchangeServer.CASOWA = true;
                        else
                            myExchangeServer.CASOWA = false;

                        if (entity.SimulationTests.Where(x => x.Name == "POP3").Count() > 0)
                            myExchangeServer.CASPop3 = true;
                        else
                            myExchangeServer.CASPop3 = false;

                        if (entity.SimulationTests.Where(x => x.Name == "Auto Discovery").Count() > 0)
                            myExchangeServer.CASAutoDiscovery = true;
                        else
                            myExchangeServer.CASAutoDiscovery = false;

                        if (entity.SimulationTests.Where(x => x.Name == "Outlook Native RPC").Count() > 0)
                            myExchangeServer.CASOARPC = true;
                        else
                            myExchangeServer.CASOARPC = false;

                        if (entity.SimulationTests.Where(x => x.Name == "IMAP").Count() > 0)
                            myExchangeServer.CASImap = true;
                        else
                            myExchangeServer.CASImap = false;

                        if (entity.SimulationTests.Where(x => x.Name == "Active Sync").Count() > 0)
                            myExchangeServer.CASActiveSync = true;
                        else
                            myExchangeServer.CASActiveSync = false;

                    }

                    try
                    {
                        var creds = listOfCredentials.Where(x => x.Id == entity.ActiveSyncCredentialsId).First();
                        myExchangeServer.ActiveSyncCASUserName = creds.UserId;
                        myExchangeServer.ActiveSyncCASPassword = decodePasswordFromEncodedString(creds.Password, myExchangeServer.Name);
                    }
                    catch (Exception ex)
                    {

                    }


                    /*
                    CommonDB db = new CommonDB();
                    DataTable dt = db.GetData("select TestName,cr3.UserID CASUserId,cr3.Password CASPassword,CT.URLs from [ExchangeTestNames] ET inner join [CASServerTests] CT on ET.TestId=CT.TestId left outer join credentials cr3 on cr3.ID=CT.CredentialsId  where ServerId=" + MyExchangeServer.ServerId + "");

                    foreach (DataRow row in dt.Rows)
                    {
                        Common.WriteDeviceHistoryEntry("All", MyExchangeServer.ServerType, "In SetExchangeServerSettings: 1" + row["TestName"].ToString(), Common.LogLevel.Normal);

                        switch (row["TestName"].ToString())
                        {
                            case "SMTP":
                                MyExchangeServer.CASSmtp = true;
                                // MyExchangeServer.CASSmtp = Convert.ToBoolean(DR["CASSmtp"].ToString());
                                MyExchangeServer.SMTPCASUserName = row["CASUserId"].ToString();
                                if (MyExchangeServer.SMTPCASUserName == "" || MyExchangeServer.SMTPCASUserName == null)
                                {
                                    MyExchangeServer.SMTPCASUserName = MyExchangeServer.UserName;
                                }
                                MyExchangeServer.SMTPCASPassword = decodePasswordFromEncodedString(row["CASPassword"].ToString(), MyExchangeServer.Name);
                                if (MyExchangeServer.SMTPCASPassword == "" || MyExchangeServer.SMTPCASPassword == null)
                                {
                                    MyExchangeServer.SMTPCASPassword = MyExchangeServer.Password;
                                }
                                MyExchangeServer.SMTPURLs = row["URLs"].ToString();
                                if (MyExchangeServer.SMTPURLs == "" || MyExchangeServer.SMTPURLs == null)
                                {
                                    MyExchangeServer.SMTPURLs = MyExchangeServer.IPAddress;
                                }

                                break;
                            case "POP3":
                                MyExchangeServer.CASPop3 = true;
                                // MyExchangeServer.CASPop3 = Convert.ToBoolean(DR["CASPop3"].ToString());
                                MyExchangeServer.POP3CASUserName = row["CASUserId"].ToString();
                                if (MyExchangeServer.POP3CASUserName == "" || MyExchangeServer.POP3CASUserName == null)
                                {
                                    MyExchangeServer.POP3CASUserName = MyExchangeServer.UserName;
                                }
                                MyExchangeServer.POP3CASPassword = decodePasswordFromEncodedString(row["CASPassword"].ToString(), MyExchangeServer.Name);
                                if (MyExchangeServer.POP3CASPassword == "" || MyExchangeServer.POP3CASPassword == null)
                                {
                                    MyExchangeServer.POP3CASPassword = MyExchangeServer.Password;
                                }
                                MyExchangeServer.POP3URLs = row["URLs"].ToString();
                                if (MyExchangeServer.POP3URLs == "" || MyExchangeServer.POP3URLs == null)
                                {
                                    MyExchangeServer.POP3URLs = MyExchangeServer.IPAddress;
                                }
                                break;
                            case "IMAP":
                                MyExchangeServer.CASImap = true;
                                //  MyExchangeServer.CASImap = Convert.ToBoolean(DR["CASImap"].ToString());
                                MyExchangeServer.IMAPCASUserName = row["CASUserId"].ToString();
                                if (MyExchangeServer.IMAPCASUserName == "" || MyExchangeServer.IMAPCASUserName == null)
                                {
                                    MyExchangeServer.IMAPCASUserName = MyExchangeServer.UserName;
                                }
                                MyExchangeServer.IMAPCASPassword = decodePasswordFromEncodedString(row["CASPassword"].ToString(), MyExchangeServer.Name);
                                if (MyExchangeServer.IMAPCASPassword == "" || MyExchangeServer.IMAPCASPassword == null)
                                {
                                    MyExchangeServer.IMAPCASPassword = MyExchangeServer.Password;
                                }
                                MyExchangeServer.IMAPURLs = row["URLs"].ToString();
                                if (MyExchangeServer.IMAPURLs == "" || MyExchangeServer.IMAPURLs == null)
                                {
                                    MyExchangeServer.IMAPURLs = MyExchangeServer.IPAddress;
                                }
                                break;
                            case "Outlook Anywhere":
                                MyExchangeServer.CASEWS = true;
                                //MyExchangeServer.CASEWS = Convert.ToBoolean(DR["CASEWS"].ToString());
                                MyExchangeServer.OutlookAnywhereCASUserName = row["CASUserId"].ToString();
                                if (MyExchangeServer.OutlookAnywhereCASUserName == "" || MyExchangeServer.OutlookAnywhereCASUserName == null)
                                {
                                    MyExchangeServer.OutlookAnywhereCASUserName = MyExchangeServer.UserName;
                                }
                                MyExchangeServer.OutlookAnywhereCASPassword = decodePasswordFromEncodedString(row["CASPassword"].ToString(), MyExchangeServer.Name);
                                if (MyExchangeServer.OutlookAnywhereCASPassword == "" || MyExchangeServer.OutlookAnywhereCASPassword == null)
                                {
                                    MyExchangeServer.OutlookAnywhereCASPassword = MyExchangeServer.Password;
                                }
                                MyExchangeServer.OutlookAnywhereURLs = row["URLs"].ToString();
                                if (MyExchangeServer.OutlookAnywhereURLs == "" || MyExchangeServer.OutlookAnywhereURLs == null)
                                {
                                    MyExchangeServer.OutlookAnywhereURLs = MyExchangeServer.IPAddress;
                                }
                                break;
                            case "Auto Discovery":
                                MyExchangeServer.CASAutoDiscovery = true;
                                //MyExchangeServer.CASAutoDiscovery = Convert.ToBoolean(DR["CASAutoDiscovery"].ToString());
                                MyExchangeServer.AutoDiscoveryCASUserName = row["CASUserId"].ToString();

                                if (MyExchangeServer.AutoDiscoveryCASUserName == "" || MyExchangeServer.AutoDiscoveryCASUserName == null)
                                {
                                    MyExchangeServer.AutoDiscoveryCASUserName = MyExchangeServer.UserName;
                                }
                                MyExchangeServer.AutoDiscoveryCASPassword = decodePasswordFromEncodedString(row["CASPassword"].ToString(), MyExchangeServer.Name);
                                if (MyExchangeServer.AutoDiscoveryCASPassword == "" || MyExchangeServer.AutoDiscoveryCASPassword == null)
                                {
                                    MyExchangeServer.AutoDiscoveryCASPassword = MyExchangeServer.Password;
                                }
                                MyExchangeServer.AutoDiscoveryURLs = row["URLs"].ToString();
                                if (MyExchangeServer.AutoDiscoveryURLs == "" || MyExchangeServer.AutoDiscoveryURLs == null)
                                {
                                    MyExchangeServer.AutoDiscoveryURLs = MyExchangeServer.IPAddress;
                                }
                                break;

                            case "Active Sync":
                                MyExchangeServer.CASActiveSync = true;
                                //MyExchangeServer.CASActiveSync = Convert.ToBoolean(DR["CASActiveSync"].ToString());
                                MyExchangeServer.ActiveSyncCASUserName = row["CASUserId"].ToString();
                                if (MyExchangeServer.ActiveSyncCASUserName == "" || MyExchangeServer.ActiveSyncCASUserName == null)
                                {
                                    MyExchangeServer.ActiveSyncCASUserName = MyExchangeServer.UserName;
                                }
                                MyExchangeServer.ActiveSyncCASPassword = decodePasswordFromEncodedString(row["CASPassword"].ToString(), MyExchangeServer.Name);
                                if (MyExchangeServer.ActiveSyncCASPassword == "" || MyExchangeServer.ActiveSyncCASPassword == null)
                                {
                                    MyExchangeServer.ActiveSyncCASPassword = MyExchangeServer.Password;
                                }
                                MyExchangeServer.ActiveSyncURLs = row["URLs"].ToString();
                                if (MyExchangeServer.ActiveSyncURLs == "" || MyExchangeServer.ActiveSyncURLs == null)
                                {
                                    MyExchangeServer.ActiveSyncURLs = MyExchangeServer.IPAddress;
                                }
                                break;
                            case "OWA (Outlook Web App)":
                                MyExchangeServer.CASOWA = true;
                                //MyExchangeServer.CASOWA = Convert.ToBoolean(DR["CASOWA"].ToString());
                                MyExchangeServer.OWACASUserName = row["CASUserId"].ToString();
                                if (MyExchangeServer.OWACASUserName == "" || MyExchangeServer.OWACASUserName == null)
                                {
                                    MyExchangeServer.OWACASUserName = MyExchangeServer.UserName;
                                }
                                MyExchangeServer.OWACASPassword = decodePasswordFromEncodedString(row["CASPassword"].ToString(), MyExchangeServer.Name);
                                if (MyExchangeServer.OWACASPassword == "" || MyExchangeServer.OWACASPassword == null)
                                {
                                    MyExchangeServer.OWACASPassword = MyExchangeServer.Password;
                                }
                                MyExchangeServer.OWAURLs = row["URLs"].ToString();
                                if (MyExchangeServer.OWAURLs == "" || MyExchangeServer.OWAURLs == null)
                                {
                                    MyExchangeServer.OWAURLs = MyExchangeServer.IPAddress;
                                }
                                break;

                            case "Outlook Native RPC":
                                MyExchangeServer.CASOARPC = true;
                                // MyExchangeServer.CASOARPC = Convert.ToBoolean(DR["CASOARPC"].ToString());
                                MyExchangeServer.RPCCASUserName = row["CASUserId"].ToString();
                                if (MyExchangeServer.RPCCASUserName == "" || MyExchangeServer.RPCCASUserName == null)
                                {
                                    MyExchangeServer.RPCCASUserName = MyExchangeServer.UserName;
                                }
                                MyExchangeServer.RPCCASPassword = decodePasswordFromEncodedString(row["CASPassword"].ToString(), MyExchangeServer.Name);
                                if (MyExchangeServer.RPCCASPassword == "" || MyExchangeServer.RPCCASPassword == null)
                                {
                                    MyExchangeServer.RPCCASPassword = MyExchangeServer.Password;
                                }
                                MyExchangeServer.RPCURLs = row["URLs"].ToString();
                                if (MyExchangeServer.RPCURLs == "" || MyExchangeServer.RPCURLs == null)
                                {
                                    MyExchangeServer.RPCURLs = MyExchangeServer.IPAddress;
                                }
                                break;

                        }

                    }
                    */







                    //myExchangeServer = SetExchangeServerSettings(myExchangeServer, DR);

                    //newCollection.Add(SetExchangeServerSettings(myExchangeServer, DR));

                }

				

				//Removes servers not in the new lsit
				foreach (MonitoredItems.ExchangeServer server in myExchangeServers)
				{
					string currName = server.Name;
					try
					{
						//MonitoredItems.ExchangeServer newServer = newCollection.SearchByName(currName);
						if (!ServerNameList.Contains(currName))
						{
							//incase it doesnt throw and exception, if not found, removed server from list
							myExchangeServers.Delete(currName);
							removedServers++;

						}
					}
					catch (Exception ex)
					{
						//server not found
						myExchangeServers.Delete(currName);
						removedServers++;

					}

				}


				

				//myExchangeServers = newCollection;
				/**********************************************************/
				Common.WriteDeviceHistoryEntry("All", "Exchange", "There are " + myExchangeServers.Count + " servers in the collection.  " + newServers + " were new and " + updatedServers + " were updated.");
			}
			else
			{
				myExchangeServers = new MonitoredItems.ExchangeServersCollection();
			}

			Common.InsertInsufficentLicenses(myExchangeServers);

			//At this point we have all Servers with ALL the information(including Threshold settings)
		}


		#endregion

		#region LyncColl

		private void CreateLyncServersCollection()
		{
			//Fetch all servers
			if (myLyncServers == null)
				myLyncServers = new MonitoredItems.ExchangeServersCollection();

			//myLyncServers = new MonitoredItems.ExchangeServersCollection();
			CommonDB DB = new CommonDB();
			StringBuilder SQL = new StringBuilder();
			SQL.Append(" select LS.ScanInterval,LS.RetryInterval,LS.OffHoursScanInterval,LS.Category,LS.FailureThreshold,LS.ResponseThreshold,LS.CPUThreshold,Sr.IPAddress,Sr.ServerName,L.Location ");
			SQL.Append(" ,S.ServerType, S.ID as ServerTypeId, C.UserID,C.Password, st.LastUpdate, st.Status, st.StatusCode, di.CurrentNodeID , LS.MemoryThreshold from LyncServers LS inner join Servers Sr  on LS.ServerID=Sr.ID");
			SQL.Append(" inner join ServerTypes S on Sr.ServerTypeID=S.ID  and S.ServerType='Skype for Business'	inner join Locations L on Sr.LocationID =L.ID ");
			SQL.Append(" 	inner join Credentials C on LS.CredentialsId=C.ID ");
			if (ConfigurationManager.AppSettings["VSNodeName"] != null)
			{
				string NodeName = ConfigurationManager.AppSettings["VSNodeName"].ToString();
				SQL.Append(" inner join DeviceInventory di on Sr.ID=di.DeviceID and Sr.ServerTypeId=di.DeviceTypeId ");
				SQL.Append(" inner join Nodes on (Nodes.ID=di.CurrentNodeId or di.CurrentNodeId=-1) and Nodes.Name='" + NodeName + "' ");
			}
			SQL.Append(" left outer join Status st on st.Type=S.ServerType and st.Name=Sr.ServerName ");
			SQL.Append(" 	WHERE LS.Enabled = 1 ");

			DataTable dtServers = DB.GetData(SQL.ToString());
			listOfIdsForLync = String.Join(",", dtServers.AsEnumerable().Select(r => r.Field<string>("ID").ToString()).ToList());
			//Loop through servers
			//MonitoredItems.ExchangeThresholdSettings ExchgThreshold = new MonitoredItems.ExchangeThresholdSettings();

			/*
			if (dtServers.Rows.Count > 0)
			{
				for (int i = 0; i < dtServers.Rows.Count; i++)
				{
					DataRow DR = dtServers.Rows[i];

					
					MonitoredItems.ExchangeServer myExchangeServer = new MonitoredItems.ExchangeServer();
					myExchangeServer.Enabled = true;
					myExchangeServer.IPAddress = DR["IPAddress"].ToString();
					myExchangeServer.Name = DR["ServerName"].ToString();
					myExchangeServer.ScanInterval = Convert.ToInt32(DR["ScanInterval"].ToString());
					myExchangeServer.UserName = DR["UserId"].ToString();
					myExchangeServer.Password = decodePasswordFromEncodedString(DR["Password"].ToString(), myExchangeServer.Name);
					myExchangeServer.Location = DR["Location"].ToString();
					myExchangeServer.ServerType = "Lync";
					myExchangeServer.LastScan = DR["LastUpdate"] == null || DR["LastUpdate"].ToString() == "" ? DateTime.Now : DateTime.Parse(DR["LastUpdate"].ToString());
					myExchangeServer.Status = DR["Status"] == null || DR["Status"].ToString() == "" ? "Not Scanned" : DR["Status"].ToString();
					myExchangeServer.StatusCode = DR["StatusCode"] == null || DR["StatusCode"].ToString() == "" ? "Maintenance" : DR["StatusCode"].ToString();
					myExchangeServer.ServerTypeId = int.Parse(DR["ServerTypeId"].ToString());
					myExchangeServer.CPU_Threshold = int.Parse(DR["CPU_Threshold"].ToString());
					myExchangeServer.Memory_Threshold = int.Parse(DR["MemThreshold"].ToString());
					myExchangeServer.InsufficentLicenses = DR["CurrentNodeID"] != null && DR["CurrentNodeID"].ToString() == "-1" ? true : false;
					newCollection.Add(myExchangeServer);

				}
				//Common.WriteDeviceHistoryEntry("All", "Lync", "There are " + myLyncServers.Count + " servers in the collection");


				
				int updatedServers = 0;
				int newServers = 0;
				int removedServers = 0;

				//Removes servers not in the new lsit
				foreach (MonitoredItems.ExchangeServer server in myLyncServers)
				{
					string currName = server.Name;
					try
					{
						MonitoredItems.ExchangeServer newServer = newCollection.SearchByName(currName);
						if (newServer == null)
						{
							//incase it doesnt throw and exception, if not found, removed server from list
							myLyncServers.Delete(currName);
							removedServers++;

						}
					}
					catch (Exception ex)
					{
						//server not found
						myLyncServers.Delete(currName);
						removedServers++;

					}

				}


				// adds/updates new servers
				foreach (MonitoredItems.ExchangeServer server in newCollection)
				{
					string currName = server.Name;
					try
					{
						MonitoredItems.ExchangeServer oldServer = myLyncServers.SearchByName(currName);

						if (oldServer != null)
						{

							oldServer.IPAddress = server.IPAddress;
							oldServer.Name = server.Name;
							oldServer.ScanInterval = server.ScanInterval;
							oldServer.UserName = server.UserName;
							oldServer.Password = server.Password;
							oldServer.Location = server.Location;

							oldServer.Enabled = true;

							updatedServers++;
						}
						else
						{
							myLyncServers.Add(server);
							newServers++;
						}
					}
					catch (NullReferenceException ex)
					{
						myLyncServers.Add(server);
						newServers++;
					}

				}


				//myExchangeServers = newCollection;
				//**********************************************************
				Common.WriteDeviceHistoryEntry("All", "Lync", "There are " + myLyncServers.Count + " servers in the collection.  " + newServers + " were new and " + updatedServers + " were updated.");
			}
			else
			{
				myLyncServers = new MonitoredItems.ExchangeServersCollection();
			}


	*/








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

					MonitoredItems.ExchangeServer myExchangeServer = null;

					//Checks to see if the server is newly added or exists.  Adds if it is new
					try
					{
						myExchangeServer = myLyncServers.SearchByName(DR["ServerName"].ToString());
						if (myExchangeServer == null)
						{
							//New server.  Set inits and add to collection

							myExchangeServer = InitForLyncServers(myExchangeServer, DR);
							myLyncServers.Add(myExchangeServer);
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
						myExchangeServer = InitForLyncServers(myExchangeServer, DR);
						myLyncServers.Add(myExchangeServer);
						newServers++;
					}

					myExchangeServer = SetLyncServerSettings(myExchangeServer, DR);

					//newCollection.Add(SetExchangeServerSettings(myExchangeServer, DR));

				}

				

				//Removes servers not in the new lsit
				foreach (MonitoredItems.ExchangeServer server in myLyncServers)
				{
					string currName = server.Name;
					try
					{

						if (!ServerNameList.Contains(currName))
						{
							//incase it doesnt throw and exception, if not found, removed server from list
							myLyncServers.Delete(currName);
							removedServers++;

						}
					}
					catch (Exception ex)
					{
						//server not found
						myLyncServers.Delete(currName);
						removedServers++;

					}

				}



				//myExchangeServers = newCollection;
				/**********************************************************/
				Common.WriteDeviceHistoryEntry("All", "Skype for Business", "There are " + myLyncServers.Count + " servers in the collection.  " + newServers + " were new and " + updatedServers + " were updated.");
			}
			else
			{
				myLyncServers = new MonitoredItems.ExchangeServersCollection();
			}




			Common.InsertInsufficentLicenses(myLyncServers);
		}

		private MonitoredItems.ExchangeServer InitForLyncServers(MonitoredItems.ExchangeServer MyLyncServer, DataRow DR)
		{

			MyLyncServer.ServerType = "Skype for Business";
			MyLyncServer.LastScan = DR["LastUpdate"] == null || DR["LastUpdate"].ToString() == "" ? DateTime.Now.AddMinutes(-30) : DateTime.Parse(DR["LastUpdate"].ToString());
			MyLyncServer.Status = DR["Status"] == null || DR["Status"].ToString() == "" ? "Not Scanned" : DR["Status"].ToString();
			MyLyncServer.StatusCode = DR["StatusCode"] == null || DR["StatusCode"].ToString() == "" ? "Maintenance" : DR["StatusCode"].ToString();
			MyLyncServer.Enabled = true;

			return MyLyncServer;
		}


		private MonitoredItems.ExchangeServer SetLyncServerSettings(MonitoredItems.ExchangeServer MyLyncServer, DataRow DR)
		{

			MyLyncServer.IPAddress = DR["IPAddress"].ToString();
			MyLyncServer.Name = DR["ServerName"].ToString();
			MyLyncServer.ScanInterval = Convert.ToInt32(DR["ScanInterval"].ToString());
			MyLyncServer.UserName = DR["UserId"].ToString();
			MyLyncServer.Password = decodePasswordFromEncodedString(DR["Password"].ToString(), MyLyncServer.Name);
			MyLyncServer.Location = DR["Location"].ToString();

			MyLyncServer.ServerTypeId = int.Parse(DR["ServerTypeId"].ToString());
			MyLyncServer.CPU_Threshold = int.Parse(DR["CPUThreshold"].ToString());
			MyLyncServer.Memory_Threshold = int.Parse(DR["MemoryThreshold"].ToString());
			MyLyncServer.InsufficentLicenses = DR["CurrentNodeID"] != null && DR["CurrentNodeID"].ToString() == "-1" ? true : false;

			return MyLyncServer;
		}

        #endregion

        private void CreateExchangeMailProbeCollection()
        {
            //Fetch all servers
            if (myExchangeMailProbes == null)
                myExchangeMailProbes = new MonitoredItems.ExchangeMailProbesCollection();
            MonitoredItems.ExchangeMailProbesCollection newCollection = new MonitoredItems.ExchangeMailProbesCollection();
            CommonDB DB = new CommonDB();
            string NodeName = null;
            List<VSNext.Mongo.Entities.Server> listOfExchangeServers = new List<Server>();
            List<VSNext.Mongo.Entities.ServerOther> listOfExchangeMailProbes = new List<ServerOther>();
            List<VSNext.Mongo.Entities.Credentials> listOfCredentials = new List<Credentials>();

            try
            {
                NodeName = ConfigurationManager.AppSettings["VSNodeName"].ToString();

                VSNext.Mongo.Repository.Repository<VSNext.Mongo.Entities.ServerOther> repository = new VSNext.Mongo.Repository.Repository<VSNext.Mongo.Entities.ServerOther>(DB.GetMongoConnectionString());
                FilterDefinition<VSNext.Mongo.Entities.ServerOther> filterDef = repository.Filter.Eq(x => x.Type, VSNext.Mongo.Entities.Enums.ServerType.ExchangeMailProbe.ToDescription());

                ProjectionDefinition<VSNext.Mongo.Entities.ServerOther> projectionDef = repository.Project
                    .Include(x => x.Id)
                    .Include(x => x.Name)
                    .Include(x => x.OffHoursScanInterval)
                    .Include(x => x.RetryInterval)
                    .Include(x => x.ScanInterval)
                    .Include(x => x.ExchangeMailProbeServers)
                    .Include(x => x.MailProbeRedThreshold)
                    .Include(x => x.MailProbeYellowThreshold)
                    .Include(x => x.CurrentNode);
                
                listOfExchangeMailProbes = repository.Find(filterDef, projectionDef).ToList();

                VSNext.Mongo.Repository.Repository<VSNext.Mongo.Entities.Server> repositoryServer = new VSNext.Mongo.Repository.Repository<VSNext.Mongo.Entities.Server>(DB.GetMongoConnectionString());
                FilterDefinition<VSNext.Mongo.Entities.Server> filterDefServer = repositoryServer.Filter.Eq(x => x.DeviceType, VSNext.Mongo.Entities.Enums.ServerType.Exchange.ToDescription());

                ProjectionDefinition<VSNext.Mongo.Entities.Server> projectionDefServer = repositoryServer.Project
                    .Include(x => x.Id)
                    .Include(x => x.DeviceName)
                    .Include(x => x.IPAddress)
                    .Include(x => x.AuthenticationType)
                    .Include(x => x.CredentialsId);

                listOfExchangeServers = repositoryServer.Find(filterDefServer, projectionDefServer).ToList();

                VSNext.Mongo.Repository.Repository<VSNext.Mongo.Entities.Credentials> repositoryCredentials = new VSNext.Mongo.Repository.Repository<Credentials>(DB.GetMongoConnectionString());
                listOfCredentials = repositoryCredentials.Find(x => true).ToList();
            }
            catch (Exception ex)
            {
                Common.WriteDeviceHistoryEntry("All", "ExchangeMailProbe", "Exception in CreateExchangeMailProbeCollection when getting the data from the db. Exception: " + ex.Message.ToString(), Common.LogLevel.Normal);
            }
            
            //Loop through servers
            if (listOfExchangeMailProbes.Count > 0)
            {
                int updatedServers = 0;
                int newServers = 0;
                int removedServers = 0;

                for (int i = 0; i < listOfExchangeMailProbes.Count; i++)
                {
                    VSNext.Mongo.Entities.ServerOther entity = listOfExchangeMailProbes[i];
                    
                    MonitoredItems.ExchangeMailProbe myExchangeMailProbe = null;// = new MonitoredItems.ExchangeServer();
                   
                    //Checks to see if the server is newly added or exists.  Adds if it is new
                    try
                    {
                        try
                        {
                            myExchangeMailProbe = myExchangeMailProbes.SearchByName(entity.Name.ToString());
                        }
                        catch (Exception ex)
                        {

                        }

                        if (myExchangeMailProbe == null)
                        {
                            //New server.  Set inits and add to collection

                            myExchangeMailProbe = new MonitoredItems.ExchangeMailProbe();
                            myExchangeMailProbe.ServerType = Enums.ServerType.ExchangeMailProbe.ToDescription();

                            //myExchangeMailProbe.LastScan = statusEntry == null || !statusEntry.LastUpdated.HasValue || statusEntry.LastUpdated.ToString() == "" ? DateTime.Now.AddHours(-1) : statusEntry.LastUpdated.Value;
                            //myExchangeMailProbe.Status = statusEntry == null || statusEntry.CurrentStatus.ToString() == "" ? "Not Scanned" : statusEntry.CurrentStatus;
                            //myExchangeMailProbe.StatusCode = statusEntry == null || statusEntry.StatusCode.ToString() == "" ? "Maintenance" : statusEntry.StatusCode;

                            myExchangeMailProbes.Add(myExchangeMailProbe);
                            newServers++;

                        }
                        else
                        {
                            updatedServers++;
                        }
                    }
                    catch (Exception ex)
                    {
                        Common.WriteDeviceHistoryEntry("All", "ExchangeMailProbe", "Exception in CreateExchangeServersCollection when init new server. Exception: " + ex.Message.ToString(), Common.LogLevel.Normal);
                    }

                    myExchangeMailProbe.ServerObjectID = entity.Id;
                    myExchangeMailProbe.Name = entity.Name;

                    //try
                    //{
                    //    var creds = listOfCredentials.Where(x => x.Id == entity.CredentialsId).First();
                    //    myExchangeServer.UserName = creds.UserId;
                    //    myExchangeServer.Password = decodePasswordFromEncodedString(creds.Password, myExchangeServer.Name);
                    //}
                    //catch (Exception ex)
                    //{

                    //}

                    //try
                    //{
                    //    var location = listOfLocations.Where(x => x.Id == entity.LocationId).First();
                    //    myExchangeServer.Location = location.LocationName;
                    //}
                    //catch (Exception ex)
                    //{

                    //}

                    //try
                    //{
                    //    var status = listOfStatus.Where(x => x.DeviceId == entity.Id).First();
                    //    myExchangeServer.Status = status.CurrentStatus;
                    //    myExchangeServer.StatusCode = status.StatusCode;
                    //    myExchangeServer.LastScan = status.LastUpdated.HasValue ? status.LastUpdated.Value : DateTime.Now.AddHours(-1);
                    //    myExchangeServer.ResponseDetails = status.Details;
                    //}
                    //catch (Exception ex)
                    //{

                    //}


                    myExchangeMailProbe.ScanInterval = entity.ScanInterval.HasValue ? entity.ScanInterval.Value : 8;
                    myExchangeMailProbe.OffHoursScanInterval = entity.OffHoursScanInterval.HasValue ? entity.OffHoursScanInterval.Value : 10;

                    myExchangeMailProbe.Enabled = true;
                    //myExchangeServer.InsufficentLicenses = entity.CurrentNode != null && entity.CurrentNode == "-1" ? true : false;
                    myExchangeMailProbe.InsufficentLicenses = entity.CurrentNode == null || entity.CurrentNode != NodeName;
                    myExchangeMailProbe.CurrentNode = entity.CurrentNode;
                    myExchangeMailProbe.LatencyRedThreshold = entity.MailProbeRedThreshold.GetValueOrDefault(0);
                    myExchangeMailProbe.LatencyYellowThreshold = entity.MailProbeYellowThreshold.GetValueOrDefault(0);

                    foreach(ExchangeMailProbeServer currMailProbeExchangeServer in entity.ExchangeMailProbeServers)
                    {
                        if (!listOfExchangeServers.Exists(x => x.Id == currMailProbeExchangeServer.DeviceId))
                            continue;
                        Server currentExchangeServer = listOfExchangeServers.First(x => x.Id == currMailProbeExchangeServer.DeviceId);
                        Credentials credential = new Credentials();
                        if (listOfCredentials.Exists(x => x.Id == currentExchangeServer.CredentialsId))
                            credential = listOfCredentials.First(x => x.Id == currentExchangeServer.CredentialsId);


                        MonitoredItems.ExchangeServer newExchangeServer = new MonitoredItems.ExchangeServer()
                        {
                            Name = currentExchangeServer.DeviceName,
                            IPAddress = currentExchangeServer.IPAddress,
                            AuthenticationType = currentExchangeServer.AuthenticationType,
                            UserName = credential.UserId,
                            Password = decodePasswordFromEncodedString(credential.Password, currentExchangeServer.DeviceName)
                        };
                        myExchangeMailProbe.ExchangeServers.Add(newExchangeServer);
                    }
                   

                    //myExchangeServer = SetExchangeServerSettings(myExchangeServer, DR);

                    //newCollection.Add(SetExchangeServerSettings(myExchangeServer, DR));

                }



                //Removes servers not in the new lsit
                foreach (MonitoredItems.ExchangeMailProbe server in myExchangeMailProbes)
                {
                    string currName = server.Name;
                    try
                    {
                        //MonitoredItems.ExchangeServer newServer = newCollection.SearchByName(currName);
                        if (!listOfExchangeMailProbes.Select(x => x.Name).Contains(currName))
                        {
                            //incase it doesnt throw and exception, if not found, removed server from list
                            myExchangeMailProbes.Delete(currName);
                            removedServers++;

                        }
                    }
                    catch (Exception ex)
                    {
                        //server not found
                        myExchangeMailProbes.Delete(currName);
                        removedServers++;

                    }

                }

                //myExchangeServers = newCollection;
                /**********************************************************/
                Common.WriteDeviceHistoryEntry("All", "ExchangeMailProbe", "There are " + myExchangeMailProbes.Count + " servers in the collection.  " + newServers + " were new and " + updatedServers + " were updated.");
            }
            else
            {
                myExchangeMailProbes = new MonitoredItems.ExchangeMailProbesCollection();
            }

            Common.InsertInsufficentLicenses(myExchangeMailProbes);
        }


        private string decodePasswordFromEncodedString(string s, string serverName)
		{

			TripleDES tripledes = new TripleDES();
			try
			{

				string[] str1 = s.Replace(" ", "").Split(',');

				byte[] bytes = str1.Select(t => Convert.ToByte(t)).ToArray();

				return tripledes.Decrypt(bytes);
			}
			catch (Exception ex)
			{
				Common.WriteDeviceHistoryEntry("All", "Exchange", "The password for " + serverName + " does not seem to be encrypted and will use the raw string.  Error: " + ex.Message);
				return s;
			}
		}

		#endregion


		#region Exchange

		private void MonitorExchange(int threadNum)
		{
			Thread.CurrentThread.CurrentCulture = c;
			//MonitoredItems.ExchangeServer CurrentServer;
			CommonDB DB = new CommonDB();

            //  Common.WriteDeviceHistoryEntry("All", "Exchange", "Thread  Count  " + threadNum);
            Common.WriteDeviceHistoryEntry("All", "Exchange", "Thread  Count  " + threadNum + "", Common.LogLevel.Normal);
			//Runspace runspace = RunspaceFactory.CreateRunspace();
			//runspace.Open();
			while (true)
			{
				ExchangeMutex.WaitOne();
				MonitoredItems.ExchangeServer thisServer;
				try
				{
					thisServer = Common.SelectServerToMonitor(myExchangeServers) as MonitoredItems.ExchangeServer;
				
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
					ExchangeMutex.ReleaseMutex();
				}

				if(thisServer != null)
				{
					Common.WriteDeviceHistoryEntry("All", "Exchange", "Scanning Server " + thisServer.Name + " on thread " + threadNum);
					thisServer.IsBeingScanned = true;

					MaintenanceDll maintenance = new MaintenanceDll();
					if (maintenance.InMaintenance("Exchange", thisServer.Name))
					{
						Common.ServerInMaintenance(thisServer);
						goto CleanUp;
					}
                    
					TestResults AllTestResults = new TestResults();

                    Common.SetupServer(thisServer, thisServer.ServerType, AllTestResults, AuthenticationType: thisServer.AuthenticationType);

                    //IServerRole ServerRole = null;
                    // Create Powershell Instance to pass for Check Server
                    ReturnPowerShellObjects results = null;
					//results = Common.PrereqForExchange(thisServer.Name, thisServer.UserName, thisServer.Password, "Exchange", thisServer.IPAddress);

					//this list will get all the class names
					Thread workingThread;
					System.Collections.ArrayList AliveThreads = new System.Collections.ArrayList();
					bool notResponding = false;

					if (thisServer.StatusCode == "Maintenance")
					{
						Common.WriteDeviceHistoryEntry(thisServer.ServerType, thisServer.Name, "Doing a fast scan sicne it is in maintenance", Common.LogLevel.Normal);
						thisServer.FastScan = true;
					}

					using(Common.TestRepsonding(thisServer, ref notResponding, ref AllTestResults, AuthenticationType: thisServer.AuthenticationType))
					{
						//Exchange is different since multithreded more.  This will ensure the returned PSO is destructed
					}

					if (!notResponding)
					{

						if (!thisServer.FastScan)
						{
							
							if (thisServer.Role == null || thisServer.Role.Count() == 0)
							{
								getServerRolesAndVersion(thisServer, AllTestResults);
							}

					
							foreach (string ClassName in Common.getRoleClasses(thisServer.Role, thisServer.VersionNo))
							//foreach (string ClassName in ArClasses)
							{
								workingThread = new Thread(() => RoleMonitoring(ClassName, results, AllTestResults, thisServer));
								workingThread.CurrentCulture = c;
								workingThread.IsBackground = true;
								workingThread.Priority = ThreadPriority.Normal;
								workingThread.Name = ClassName.Split('.')[1].ToString() + " - Exchange";
								workingThread.Start();
								AliveThreads.Add(workingThread);
								Thread.Sleep(2000);

							}
						}

						//Do the windows services
						workingThread = new Thread(() => startWindowsMonitoring(thisServer, ref AllTestResults));
						workingThread.CurrentCulture = c;
						workingThread.IsBackground = true;
						workingThread.Priority = ThreadPriority.Normal;
						workingThread.Name = "WIN - Exchange";
						workingThread.Start();
						AliveThreads.Add(workingThread);
						Thread.Sleep(2000);


						//Monitors and waits for all threads to finish their respective jobs

						bool waiting;
						int counter = 0;
						string aliveThreads;
						int threashold = 30;
						int killThreadsCoutner = 0;
						int killThreadsThreshold = 60 * 20;		// 20 minutes
						do
						{
							counter++;
							waiting = false;
							aliveThreads = "";
							foreach (Thread thread in AliveThreads)
							{
								if (thread.IsAlive)
								{
									waiting = true;
									if (counter > threashold)
									{
										aliveThreads += thread.Name + ", ";
									}
								}
							}

							if (counter > threashold)
							{
								try
								{
									if (aliveThreads.Length > 3)
									{
										Common.WriteDeviceHistoryEntry("Exchange", thisServer.Name, "Threads " + aliveThreads.Substring(0, aliveThreads.Length - 2) + " is still alive", Common.LogLevel.Normal);
										counter = 0;
									}
									else
									{
										Common.WriteDeviceHistoryEntry("Exchange", thisServer.Name, "Threads string is less than 3 characters", Common.LogLevel.Normal);
										counter = 0;
									}
								}
								catch (Exception ex)
								{
									Common.WriteDeviceHistoryEntry("Exchange", thisServer.Name, "Error printing out threads.  Error: " + ex.Message, Common.LogLevel.Normal);
									counter = 0;
								}
							}

							if (killThreadsCoutner > killThreadsThreshold)
							{
								Common.WriteDeviceHistoryEntry("Exchange", thisServer.Name, "The threads did not end on their own in a timely fashion so they will be killed...");

								foreach (Thread thread in AliveThreads)
								{
									if (thread.IsAlive)
									{
										thread.Abort();
										Common.WriteDeviceHistoryEntry("Exchange", thisServer.Name, "The thread " + thread.Name + " has been killed");
									}
								}

								Common.WriteDeviceHistoryEntry("Exchange", thisServer.Name, "All threads have been killed...resuming with updating and other scanning.");

								break;
							}
							killThreadsCoutner++;

							//server was not responding.  will kill all threads
							if (notResponding)
							{
								Common.WriteDeviceHistoryEntry("Exchange", thisServer.Name, "Server is not responding...will kill all threads now");
								foreach (Thread thread in AliveThreads)
								{
									if (thread.IsAlive)
									{
										thread.Abort();
										Common.WriteDeviceHistoryEntry("Exchange", thisServer.Name, "The thread " + thread.Name + " has been killed");
									}
								}

								Common.WriteDeviceHistoryEntry("Exchange", thisServer.Name, "All threads have been killed...will make appropiate changes for Not Responding.");
								break;
							}
							Thread.Sleep(1000);


						} while (waiting);

					
						//Update the Test results in the DB
						if (notResponding)
						{
							Common.WriteDeviceHistoryEntry("Exchange", thisServer.Name, "All threads are closed, starting Not Responding update scripts", Common.LogLevel.Normal);
							DB.UpdateAllTests(AllTestResults, thisServer, "Exchange");
							DB.NotRespondingQueries(thisServer, "Exchange");
						}
						else
						{
							Common.WriteDeviceHistoryEntry("Exchange", thisServer.Name, "All threads are closed, starting DB update scripts", Common.LogLevel.Normal);
							DB.UpdateAllTests(AllTestResults, thisServer, "Exchange");
						}

					}
					else
					{
						//Common.WriteDeviceHistoryEntry("Exchange", thisServer.Name, "All threads are closed, starting Not Responding update scripts", Common.LogLevel.Normal);
						//DB.UpdateAllTests(AllTestResults, thisServer, "Exchange");
						//DB.NotRespondingQueries(thisServer, "Exchange");
					}
					

				CleanUp:

					Common.WriteDeviceHistoryEntry("All", "Exchange", "Stopping scan on  Server " + thisServer.Name + " on thread " + threadNum,Common.LogLevel.Normal);

					AliveThreads = null;

					AllTestResults = null;
					//thisServer.LastScan = DateTime.Now;
					thisServer.IsBeingScanned = false;
					thisServer = null;



				}
				else
				{
					try
					{
						Common.WriteDeviceHistoryEntry("ALL", "Exchange", "Server " + thisServer.Name + " is already being scanned and will not start scanning again");
					}
					catch (Exception ex)
					{
						Common.WriteDeviceHistoryEntry("ALL", "Exchange", "Server returned as null");
					}
				}


				Common.WriteDeviceHistoryEntry("All", "Exchange", "Waiting for 5 seconds to restart the Loop ");
				// Sleep for 3 minutes 
				Thread.Sleep(1000 * 5);
				//break;
			}

		}

		int exchangeThreadCount = 0;
		int initialExchangeThreadCount = 0;
		System.Collections.ArrayList AliveExchangeMainThreads = new System.Collections.ArrayList();
		private void StartExchangeThreads(bool firstTime)
		{
			int maxThreadCount = Common.getThreadCount("Exchange");
			int startThreads = 0;
			exchangeThreadCount = myExchangeServers.Count / 3;
			if (exchangeThreadCount <= 1)
				exchangeThreadCount = 2;

			// 5/19/15 WS commented out.  VSPLUS 1776
			if (exchangeThreadCount > maxThreadCount)
				exchangeThreadCount = maxThreadCount;
            startThreads = initialExchangeThreadCount;
            if (initialExchangeThreadCount > exchangeThreadCount)
            {
                //remove the extra threads
                int j = initialExchangeThreadCount - exchangeThreadCount;
                //if inital threads are 5 and current threads are 3
                //5-3=2: //remove 2 threads
                foreach (Thread th in AliveExchangeMainThreads)
                {
                    if (j > 0)
                    {
                        if (th.IsAlive)
                            th.Abort();
                        j -= 1;
                    }
                }
            }
			initialExchangeThreadCount = exchangeThreadCount;
					
			Common.WriteDeviceHistoryEntry("All", "Exchange", "There are " + exchangeThreadCount + " threads open", Common.LogLevel.Normal);
			if (c == null)
				c = new CultureInfo("en-US");

			for (int i = startThreads; i < exchangeThreadCount; i++)
			{
                Thread monitorExchange = new Thread(() => MonitorExchange(i));
                monitorExchange.CurrentCulture = c == null ? new CultureInfo("en-US") : c;  //Should only be null on our local copies if using wrapper
                monitorExchange.IsBackground = true;
                monitorExchange.Priority = ThreadPriority.Normal;
                monitorExchange.Name = i.ToString() + "-Exchange Monitoring";
                AliveExchangeMainThreads.Add(monitorExchange);
                monitorExchange.Start();
                Thread.Sleep(2000);
			}
			
		}

		private void RoleMonitoring(string ClassName, ReturnPowerShellObjects results, TestResults AllTestResults, MonitoredItems.ExchangeServer thisServer)
		{
			try
			{
				string role = ClassName.Split('.')[1].ToString().Replace("Exchange", "").ToLower();
				commonEnums.ServerRoles roleEnum = commonEnums.ServerRoles.Empty;
				string cmdlets = "";
				switch (role)
				{
					case "cas":
						roleEnum = commonEnums.ServerRoles.CAS;
						cmdlets = "-CommandName Test-MAPIConnectivity, Test-ActiveSyncConnectivity, Get-CasMailbox, Get-ActiveSyncDevice, get-ActiveSyncDeviceStatistics ";
						break;
					case "mb":
						roleEnum = commonEnums.ServerRoles.MailBox;
                        cmdlets = "-CommandName Test-MRSHealth, Test-AssistantHealth";
						break;
					case "edge":
						roleEnum = commonEnums.ServerRoles.Edge;
						cmdlets = "-CommandName Get-Queue";
						break;
					case "hub":
						roleEnum = commonEnums.ServerRoles.HUB;
						cmdlets = "-CommandName Get-Queue";
						break;
					case "hubedge":
						if (thisServer.Role.Contains("HUB", StringComparer.InvariantCultureIgnoreCase))
						{
							roleEnum = commonEnums.ServerRoles.HUB;
						}
						else if (thisServer.Role.Contains("EDGE", StringComparer.InvariantCultureIgnoreCase))
						{
							roleEnum = commonEnums.ServerRoles.Edge;
						}
						else
						{
							roleEnum = commonEnums.ServerRoles.CAS;
						}
						roleEnum = commonEnums.ServerRoles.HUB;
						cmdlets = "-CommandName Get-Queue";
						break;
				}


				Common.WriteDeviceHistoryEntry("Exchange", thisServer.Name, "Starting thread for " + ClassName.Split('.')[1].ToString() + ".", Common.LogLevel.Normal);
				results = Common.PrereqForExchangeWithCmdlets(thisServer.Name, thisServer.UserName, thisServer.Password, "Exchange", thisServer.IPAddress, roleEnum, cmdlets, thisServer.AuthenticationType);
				}
			catch (Exception ex)
			{
				Common.WriteDeviceHistoryEntry("Exchange", thisServer.Name, "Error in RoleMonitoring 1st half.  Error: " + ex.Message, Common.LogLevel.Normal);
			}
			try
			{	
				using (results)
				{
					//AlivePSO.Add(results);
					IServerRole ServerRole = null;

					results.PS.Commands.Clear();
					System.Reflection.Assembly A = System.Reflection.Assembly.Load(ClassName.Split(new Char[] { '.' })[0].ToString());
					ServerRole = (IServerRole)Activator.CreateInstance(A.GetType(ClassName));
					ServerRole.CheckServer(thisServer, results, ref AllTestResults);
					results.PS.Commands.Clear();
				}
			}
			catch (Exception ex)
			{
				Common.WriteDeviceHistoryEntry("Exchange", thisServer.Name, "Error in RoleMonitoring 2nd half.  Error: " + ex.Message, Common.LogLevel.Normal);
			}
			GC.Collect();
			Thread.Sleep(2000);
			Common.WriteDeviceHistoryEntry("Exchange", thisServer.Name, "Ending thread for " + ClassName.Split('.')[1].ToString() + ".", Common.LogLevel.Normal);


		}

		private void startWindowsMonitoring(MonitoredItems.ExchangeServer myServer,  ref TestResults AllTestResults)
		{
			
			using(ReturnPowerShellObjects results = Common.PrereqForWindows(myServer.IPAddress.Replace("https://","").Replace("http://",""), myServer.UserName, myServer.Password, myServer.ServerType, myServer.IPAddress, commonEnums.ServerRoles.Windows))
			{
				try
				{

					MicrosoftCommon MSCommon = new MicrosoftCommon();
					MSCommon.PrereqForWindows(myServer, AllTestResults, results);
				}
				catch
				{
				}
			}

            string cmdlets = "-CommandName Test-ServiceHealth";


            using (ReturnPowerShellObjects results = Common.PrereqForExchangeWithCmdlets(myServer.Name, myServer.UserName, myServer.Password, "Exchange", myServer.IPAddress, commonEnums.ServerRoles.Windows, cmdlets, myServer.AuthenticationType))
            {
                try
                {
                    ExchangeCommon EC = new ExchangeCommon();
                    EC.PrereqForWindows(myServer, ref AllTestResults, results);
                }
                catch
                {
                }
            }
			
		}

		private void getServerRolesAndVersion(MonitoredItems.ExchangeServer myServer, TestResults AllTestResults)
		{

			Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "No roles are assigned so will find roles for the server", commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);
			

			//Runspace runspace = PSO.runspace;
			//PowerShell powershell = PSO.PS;

			using (ReturnPowerShellObjects PSO = Common.PrereqForExchangeWithCmdlets(myServer.Name, myServer.UserName, myServer.Password, myServer.ServerType, myServer.IPAddress, commonEnums.ServerRoles.Empty, "-CommandName Get-ExchangeServer", myServer.AuthenticationType))
			{
				try
				{
					PowerShell powershell = PSO.PS;
					System.IO.StreamReader scriptStream = new System.IO.StreamReader(AppDomain.CurrentDomain.BaseDirectory.ToString() + "Scripts\\" + "EX_GetServerRolesAndVersion.ps1");
					String str = "$ServerName='" + myServer.Name + "' \n " + scriptStream.ReadToEnd();
					//String str = "Test-ServiceHealth | select Role";
					//GetWMIPowerShell(ref powershell, creds, IPAddress, str, false);
					powershell.Streams.Error.Clear();

					powershell.AddScript(str);

					Collection<PSObject> results = powershell.Invoke();

					if (powershell.Streams.Error.Count > 51)
					{
						foreach (ErrorRecord er in powershell.Streams.Error)
							Console.WriteLine(er.ErrorDetails);
					}
					else
					{
						Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "EX_GetServerRolesAndVersions output results: " + results.Count.ToString(), commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);



						foreach (PSObject ps in results)
						{
							string Name = ps.Properties["Name"].Value == null ? "" : ps.Properties["Name"].Value.ToString();
							string Roles = ps.Properties["Roles"].Value == null ? "" : ps.Properties["Roles"].Value.ToString();
							string Version = ps.Properties["Version"].Value == null ? "" : ps.Properties["Version"].Value.ToString();

							string ServerRoles = "";

							if (Roles.Contains("Edge"))
								ServerRoles += "'Edge',";
							if (Roles.Contains("ClientAccess"))
								ServerRoles += "'CAS',";
							if (Roles.Contains("Mailbox"))
								ServerRoles += "'Mailbox',";
							if (Roles.Contains("Hub"))
								ServerRoles += "'Hub',";


							myServer.Role = ServerRoles.Remove(ServerRoles.Length - 1).Replace("'", "").Split(new char[] { ',' });
							myServer.VersionNo = Version;

                            MongoStatementsUpdate<VSNext.Mongo.Entities.Server> mongoUpdate = new MongoStatementsUpdate<VSNext.Mongo.Entities.Server>();
                            mongoUpdate.filterDef = mongoUpdate.repo.Filter.Where(i => i.DeviceName == myServer.Name && i.DeviceType == myServer.ServerType);
                            mongoUpdate.updateDef = mongoUpdate.repo.Updater
                                .Set(i => i.SoftwareVersion, Convert.ToDouble(Version))
                                .Set(i => i.ServerRoles, myServer.Role.ToList());

                            AllTestResults.MongoEntity.Add(mongoUpdate);

                            MongoStatementsUpdate<VSNext.Mongo.Entities.Status> mongoStatusUpdate = new MongoStatementsUpdate<VSNext.Mongo.Entities.Status>();
                            mongoStatusUpdate.filterDef = mongoStatusUpdate.repo.Filter.Where(i => i.DeviceName == myServer.Name && i.DeviceType == myServer.ServerType);
                            mongoStatusUpdate.updateDef = mongoStatusUpdate.repo.Updater
                                .Set(i => i.ServerRoles, myServer.Role.ToList());

                            AllTestResults.MongoEntity.Add(mongoUpdate);

                            Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "Roles: " + ServerRoles + "...Version: " + Version, commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);

						}

					}

				}
				catch (Exception ex)
				{
					Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "Error in getServerRolesAndVersion: " + ex.Message, commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);

				}
				finally
				{

				}
			}

			GC.Collect();
			Thread.Sleep(2000);
		}

        private void ExchangeDevicesLoop()
        {
            List<Thread> listOfThreads = new List<Thread>();
            do
            {

                foreach(MonitoredItems.ExchangeServer server in myExchangeServers)
                {
                    if(server.CASActiveSync && listOfThreads.Where(s => s.Name == "Device Monitoring - " + server.Name).Count() == 0)
                    {
                        
                        Thread t = new Thread(() => GetExchangeDevices(server));
                        t.CurrentCulture = c;
                        t.IsBackground = true;
                        t.Priority = ThreadPriority.Normal;
                        t.Name = "Device Monitoring - " + server.Name;
                        t.Start();
                        Thread.Sleep(2000);

                        listOfThreads.Add(t);

                    }
                }

                Thread.Sleep(5 * 60 * 1000);

            } while (true);

        }


        private void GetExchangeDevices(MonitoredItems.ExchangeServer server)
        {
            
            string cmdNames = "-CommandName Get-CASMailbox,Get-ActiveSyncDevice,Get-ActiveSyncDeviceStatistics";
            do
            {

                if(server.StatusCode != "Not Responding")
                {
                    using(ReturnPowerShellObjects pso = Common.PrereqForExchangeWithCmdlets(server.Name, server.UserName, server.Password, server.ServerType, server.IPAddress, commonEnums.ServerRoles.CAS, cmdNames, server.AuthenticationType))
                    {
                        TestResults AllTestsResults = new TestResults();
                        ExchangeCAS ex = new ExchangeCAS();

                        ex.GetActiveSyncDevices(pso.PS, server, ref AllTestsResults);

                        CommonDB DB = new CommonDB();
                        DB.UpdateSQLStatements(AllTestsResults, server);

                    }
                    

                }



                Thread.Sleep(1000 * 60 * server.ScanInterval);

            } while (true);

        }



		#endregion



		#region LYNC

		MonitoredItems.ExchangeServersCollection myLyncServers;
		int lyncThreadCount = 0;
		int initialLyncThreadCount = 0;
		System.Collections.ArrayList AliveLyncMainThreads = new System.Collections.ArrayList();
		private void StartLyncThreads()
		{
			int maxThreadCount = Common.getThreadCount("Skype for Business");
			int startThreads = 0;
			lyncThreadCount = myLyncServers.Count / 3;

			if (lyncThreadCount <= 1 && myLyncServers.Count>0)
				lyncThreadCount = 2;

			// 5/19/15 WS commented out.  VSPLUS 1776
			if (lyncThreadCount > maxThreadCount)
				lyncThreadCount = maxThreadCount;
			startThreads = initialLyncThreadCount;

			if (initialLyncThreadCount > lyncThreadCount)
			{
				//remove the extra threads
				int j = initialLyncThreadCount - lyncThreadCount;
				//if inital threads are 5 and current threads are 3
				//5-3=2: //remove 2 threads
				foreach (Thread th in AliveLyncMainThreads)
				{
					if (j > 0)
					{
						if (th.IsAlive)
							th.Abort();
						j -= 1;
					}
				}
			}
			initialLyncThreadCount = lyncThreadCount;
			Common.WriteDeviceHistoryEntry("All", "Skype for Business", "There are " + lyncThreadCount + " threads open");
			if (c == null)
				c = new CultureInfo("en-US");
			for (int i = startThreads; i < lyncThreadCount; i++)
			{
				//workingThread = new Thread(() => RoleMonitoring(ClassName, results, AllTestResults, thisServer, ref AlivePSO));
				Thread monitorLync = new Thread(() => MonitorLYNC(i));
				monitorLync.CurrentCulture = c == null ? new CultureInfo("en-US") : c;  //Should only be null on our local copies if using wrapper
				monitorLync.IsBackground = true;
				monitorLync.Priority = ThreadPriority.Normal;
				monitorLync.Name = i.ToString() + "-Skype for Business Monitoring";
				monitorLync.Start();
				Thread.Sleep(2000);
			}


		}

		public MonitoredItems.ExchangeServer SelectLyncServerToMonitor()
		{

			DateTime tNow = DateTime.Now;
			DateTime tScheduled;

			DateTime timeOne;
			DateTime timeTwo;

			MonitoredItems.ExchangeServer SelectedServer = null;

			MonitoredItems.ExchangeServer ServerOne = null;
			MonitoredItems.ExchangeServer ServerTwo = null;

			RegistryHandler myRegistry = new RegistryHandler();

			String ScanASAP = "";
			String strSQL = "";
			String ServerType = "Skype for Business";
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

					for (int n = 0; n < myLyncServers.Count; n++)
					{
						ServerOne = myLyncServers.get_Item(n);
						if (ServerOne.Name == serverName && ServerOne.IsBeingScanned == false && ServerOne.Enabled)
						{
							Common.WriteDeviceHistoryEntry("All", "Skype for Business", serverName + " was marked 'Scan ASAP' so it will be scanned next.");

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
				ScanASAP = myRegistry.ReadFromRegistry("ScanLyncASAP").ToString();
			}
			catch (Exception ex)
			{
				ScanASAP = "";
			}

			try
			{
				if (ScanASAP != "")
				{
					ServerOne = myLyncServers.SearchByName(ScanASAP);
					if (ServerOne != null && ServerOne.IsBeingScanned == false)
					{
						Common.WriteDeviceHistoryEntry("All", "Skype for Business", ScanASAP + " was marked 'Scan ASAP' so it will be scanned next.");
						return ServerOne;
					}
				}

			}
			catch
			{
			}

			//Searches for the server marked as ScanASAP, if it exists
			//for (int n = 0; n < myLyncServers.Count; n++)
			//{
			//    ServerOne = myLyncServers.SearchByName(n);
			//    if (ServerOne.Name == ScanASAP && ServerOne.IsBeingScanned == false)
			//    {
			//        Common.WriteDeviceHistoryEntry("All", "Lync", ScanASAP + " was marked 'Scan ASAP' so it will be scanned next.");
			//        myRegistry.WriteToRegistry("ScanLyncASAP", "n/a");

			//        //ServerOne.ScanASAP = true;

			//        return ServerOne;
			//    }

			//}


			//Searches for the first enounter of a Not Responding server that is due for a scan
			for (int n = 0; n < myLyncServers.Count; n++)
			{
				ServerOne = myLyncServers.get_Item(n);
				if (ServerOne.Status == "Not Responding" && ServerOne.IsBeingScanned == false)
				{
					tScheduled = ServerOne.NextScan;
					if (DateTime.Compare(tNow, tScheduled) > 0)
					{
						Common.WriteDeviceHistoryEntry("All", "Skype for Business", "Selecting " + ServerOne.Name + " because the status is " + ServerOne.Status + ".  Next scheduled scan is at " + tScheduled.ToString());
						return ServerOne;
					}
				}
			}


			//Searches for the first encounter of a server that has not been scanned yet
			for (int n = 0; n < myLyncServers.Count; n++)
			{
				ServerOne = myLyncServers.get_Item(n);
                if ((ServerOne.Status == "Not Scanned" || ServerOne.Status == "Master Service Stopped.") && ServerOne.IsBeingScanned == false)
				{
					Common.WriteDeviceHistoryEntry("All", "Skype for Business", "Selecting " + ServerOne.Name + " because the status is " + ServerOne.Status + ".");
					return ServerOne;
				}
			}


			//Searches for all servers that are due for a scan
			List<MonitoredItems.ExchangeServer> ScanCanidates = new List<MonitoredItems.ExchangeServer>();

			foreach (MonitoredItems.ExchangeServer srv in myLyncServers)
			{
				if (srv.IsBeingScanned == false)
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
					Common.WriteDeviceHistoryEntry("All", "Skype for Business", "Error Selecting Skype for Business Server... " + ex.Message);
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

		private void MonitorLYNC(int threadNum)
		{
			Thread.CurrentThread.CurrentCulture = c;
			while (true)
			{
				CommonDB DB = new CommonDB();

				LyncMutex.WaitOne();
				MonitoredItems.ExchangeServer thisServer;
				try
				{
					thisServer = SelectLyncServerToMonitor();
				
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
					LyncMutex.ReleaseMutex();
				}

				if(thisServer != null)
				{
					thisServer.IsBeingScanned = true;
					ReturnPowerShellObjects results = null;
					
					TestResults AllTestResults = new TestResults();

					// not responding

					Common.WriteDeviceHistoryEntry("All", "Skype for Business", "Scanning Server " + thisServer.Name + " on thread " + threadNum);
					thisServer.IsBeingScanned = true;

					MaintenanceDll maintenance = new MaintenanceDll();
					if (maintenance.InMaintenance("Skype for Business", thisServer.Name))
					{
						Common.ServerInMaintenance(thisServer);
						goto CleanUp;
					}

					if (thisServer.StatusCode == "Maintenance")
					{
						Common.WriteDeviceHistoryEntry(thisServer.ServerType, thisServer.Name, "Doing a fast scan sicne it is in maintenance", Common.LogLevel.Normal);
						thisServer.FastScan = true;
					}
					bool notResponding = false;
					using (ReturnPowerShellObjects PSO = Common.TestRepsonding(thisServer, ref notResponding, ref AllTestResults))
					{

					}
					System.Collections.ArrayList AliveThreads = new System.Collections.ArrayList();

					if (!notResponding)
					{
						
						// not responding
						if (!thisServer.FastScan)
						{
							results = Common.PrereqForLync(thisServer.Name, thisServer.UserName, thisServer.Password, thisServer.IPAddress);
							using (results)
							{
								LYNCCommon LYNCClass = new LYNCCommon();
								LYNCClass.CheckServer(thisServer, results, ref AllTestResults);
							}
						}
						
						

						Common.WriteDeviceHistoryEntry("Skype for Business", thisServer.Name, "Starting thread for WIN.", Common.LogLevel.Normal);
						MicrosoftCommon MSCommon = new MicrosoftCommon();
						results = Common.PrereqForWindows(thisServer.Name, thisServer.UserName, thisServer.Password, thisServer.ServerType, thisServer.IPAddress, commonEnums.ServerRoles.Windows);
						using(results)
						{
							MSCommon.PrereqForWindows(thisServer, AllTestResults, results);
						}

						//server was not responding.  will kill all threads
						Thread.Sleep(1000);
						DB.UpdateAllTests(AllTestResults, thisServer, "Skype for Business");
					}
					else
					{
						//Common.WriteDeviceHistoryEntry("Skype for Business", thisServer.Name, "All threads are closed, starting Not Responding update scripts", Common.LogLevel.Normal);
						//DB.NotRespondingQueries(thisServer, "Skype for Business");
					}


					thisServer.IsBeingScanned = false;
					GC.Collect();
					Thread.Sleep(1000 * 60 * 10);
				CleanUp:

					Common.WriteDeviceHistoryEntry("All", "Skype for Business", "Stopping scan on  Server " + thisServer.Name + " on thread " + threadNum);

					AliveThreads = null;

					AllTestResults = null;
					//thisServer.LastScan = DateTime.Now;
					thisServer.IsBeingScanned = false;
					thisServer = null;
				}
				else
				{
					try
					{
						Common.WriteDeviceHistoryEntry("ALL", "Skype for Business", "Server " + thisServer.Name + " is already being scanned and will not start scanning again");
					}
					catch (Exception ex)
					{
						Common.WriteDeviceHistoryEntry("ALL", "Skype for Business", "Server returned as null");
					}
				}


				Common.WriteDeviceHistoryEntry("All", "Skype for Business", "Waiting for 5 seconds to restart the Loop ");
				// Sleep for 3 minutes 
				Thread.Sleep(1000 * 5);
			}
		}

		#endregion



		#region DailyTasks

		private void DailyTasks()
		{

			MonitoredItems.ExchangeServer DummyServerForLogs = new MonitoredItems.ExchangeServer() { Name = "DailyTask" };



			Common.WriteDeviceHistoryEntry("All", "Exchange", "Time to do Daily Tasks for Exchange.", commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);

			try
			{
                TestResults AllTestResults = new TestResults();
               
                ClearOldEmptyTempFolders(DummyServerForLogs);
                MonitoredItems.ExchangeServer testServer = null;
                int newestVersion = 0;
                if (myExchangeServers != null)
                    foreach (MonitoredItems.ExchangeServer server in myExchangeServers)
                    {
                        if ((Convert.ToInt32(server.VersionNo) > newestVersion || newestVersion == 0) && server.StatusCode != "Not Responding")
                        {
                            newestVersion = server.VersionNo == null ? 1: Convert.ToInt32(server.VersionNo);
                            testServer = server;
                        }
                    }
                


                

                        

                if (testServer != null)
                {

                    Common.WriteDeviceHistoryEntry("All", "Exchange", "Server " + testServer.Name + " will be used to perform tests.", commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);
                    Common.WriteDeviceHistoryEntry("Exchange", DummyServerForLogs.Name, "Server " + testServer.Name + " will be used to perform tests.", commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);
                    
                    ExchangeMB EMB = new ExchangeMB();

                    string cmdlets = "-CommandName Get-ExchangeServer, Get-MailboxDatabase, Get-MailboxStatistics, Get-Mailbox, Get-MessageTrackingLog, ";
                    cmdlets += "Get-OWAVirtualDirectory, Get-ECPVirtualDirectory, Get-OABVirtualDirectory, Get-WebServicesVirtualDirectory, Get-MAPIVirtualDirectory, Get-ActiveSyncVirtualDirectory, ";
                    cmdlets += "Get-OutlookAnywhere, Get-ClientAccessServer, Get-TransportServer, Get-MailboxRepairRequest, New-MailboxRepairRequest, Get-MailboxFolderStatistics, Get-DatabaseAvailabilityGroup, Get-User, Get-Group";

                    using (ReturnPowerShellObjects results = Common.PrereqForExchangeWithCmdlets(testServer.Name, testServer.UserName, testServer.Password, "Exchange", testServer.IPAddress, commonEnums.ServerRoles.Empty, cmdlets, testServer.AuthenticationType))
                    {
                        Collection<PSObject> DBCorruptionTest = TestDatabaseCorruptionQueue(testServer, ref AllTestResults, DummyServerForLogs.Name, results.PS);
                        EMB.getMailBoxInfo(testServer, ref AllTestResults, DummyServerForLogs.Name, myExchangeServers, results);
                    }

                    GC.Collect();
                    
                    CommonDB DB = new CommonDB();
                    DB.UpdateSQLStatements(AllTestResults, DummyServerForLogs);
                }
                else
                {
                    //no server was found to be used to scan
                }






                Common.CommonDailyTasks(testServer, ref AllTestResults, testServer.ServerType);



                Common.WriteDeviceHistoryEntry("All", "Exchange", "Finished Daily Tasks.", commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);
				Common.WriteDeviceHistoryEntry("Exchange", DummyServerForLogs.Name, "Finished Daily Tasks.", commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);

                Common.WriteDeviceHistoryEntry("Exchange", DummyServerForLogs.Name, "Starting Mailbox Permission loop.", commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);

                try
                {
                    if (testServer != null)
                    {
                        ExchangeMB EMB = new ExchangeMB();

                        string cmdlets = "-CommandName Get-Mailbox, Get-MailboxPermission, Get-User, Get-Group";

                        using (ReturnPowerShellObjects results = Common.PrereqForExchangeWithCmdlets(testServer.Name, testServer.UserName, testServer.Password, "Exchange", testServer.IPAddress, commonEnums.ServerRoles.Empty, cmdlets, testServer.AuthenticationType))
                        {
                            // Collection<PSObject> DBCorruptionTest = TestDatabaseCorruptionQueue(testServer, ref AllTestResults, DummyServerForLogs.Name, results.PS);
                            EMB.getMailboxPermissions(testServer, results.PS, ref AllTestResults, DummyServerForLogs.Name, myExchangeServers);
                        }
                    }
                }
                catch(Exception ex)
                {

                }

                GC.Collect();

            }
			catch (Exception ex)
			{
				Common.WriteDeviceHistoryEntry("Exchange", DummyServerForLogs.Name, "Error in Daily Tasks.  Error: " + ex.Message, commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);
			}
			Thread.Sleep(10 * 1000);


		}

        public void ClearOldEmptyTempFolders(MonitoredItems.ExchangeServer DummyServerForLogs)
        {
            //Removes all empty folders in the Windows Temp directory where 
            try
            {
                Runspace runspace = RunspaceFactory.CreateOutOfProcessRunspace(new TypeTable(new string[0]));

                PowerShell powershell = PowerShell.Create();

                PSCommand command = new PSCommand();
                command.AddScript("Get-ChildItem ($env:SystemDrive + '\\Windows\\TEMP') | ? { $_.Name -like 'tmp_*' -and $_.LastWriteTime -lt (Get-Date).AddDays(-1) } | % { " +
					"if((Get-ChildItem $_.FullName).Count -ne 0 -or {" +
					"(Get-ChildItem $_.FullName | ? { $_.FullName.EndsWith('.psd1')} | Get-Content | % { if($_.ToString().Contains('Import-PSSession -Session $ra -CommandName ')){$_}}).Length -gt 0}) " +
					"{ Remove-Item $_.FullName -Recurse }}");

                powershell.Runspace = runspace;
                powershell.Runspace.Open();

				powershell.Commands = command;
		
                Collection<PSObject> result = powershell.Invoke();

                foreach (ErrorRecord err in powershell.Streams.Error)
                {
                    Common.WriteDeviceHistoryEntry(DummyServerForLogs.ServerType, DummyServerForLogs.Name, "Error removing old empty folders in ClearOldEmptyTempFolders. " + err.Exception.ToString(), Common.LogLevel.Normal);
                }
            }
            catch (Exception ex)
            {
                Common.WriteDeviceHistoryEntry(DummyServerForLogs.ServerType, DummyServerForLogs.Name, "Exception in ClearOldEmptyTempFolders. " + ex.ToString(), Common.LogLevel.Normal);
            }

        }


		#endregion

		#region HourlyTasks


		private void HourlyTasks()
		{
			MonitoredItems.ExchangeServer DummyServerForLogs = new MonitoredItems.ExchangeServer() { Name = "HourlyTask", ServerType="Exchange" };
			Thread HourlyTasksThread = null;
			int Hour = -1;
			while (true)
			{
				if (Hour == -1 || Hour != DateTime.Now.Hour)
				{
					if (HourlyTasksThread != null && HourlyTasksThread.IsAlive)
					{
						//WS commented out due to long hourly tasks time locally and to prevent thread from being aborted if still scanning

						//Common.WriteDeviceHistoryEntry("Exchange", DummyServerForLogs.Name, "The thread for Hourly Tasks got hung up and will be killed to start the next cycle.", commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);
						//HourlyTasksThread.Abort();
					}

					HourlyTasksThread =  new Thread(() => HourlyTasksMainThread(DummyServerForLogs));
					HourlyTasksThread.CurrentCulture = c;
					HourlyTasksThread.IsBackground = true;
					HourlyTasksThread.Priority = ThreadPriority.Normal;
					HourlyTasksThread.Name = "HourlyTaskWorkerThread - Exchange";
					HourlyTasksThread.Start();
					//Thread.Sleep(60 * 60 * 1000); //sleeps for 1 hour
					Hour = DateTime.Now.Hour;
				}
				//sleep for 5 mins
				Thread.Sleep(1000 * 60 * 5);

			}
		}

		private void HourlyTasksMainThread(MonitoredItems.ExchangeServer DummyServerForLogs)
		{


			Common.WriteDeviceHistoryEntry("All", "Exchange", "Time to do Hourly Tasks for Exchange.", commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);

			try
			{
				try
				{

					TestResults testResults = new TestResults();

					if (myExchangeServers != null)
						foreach (MonitoredItems.ExchangeServer server in myExchangeServers)
						{
							server.OffHours = Common.OffHours(server.Name);
							Common.RecordUpAndDownTimes(server, ref testResults);
						}
					if (myLyncServers != null)
						foreach (MonitoredItems.ExchangeServer server in myLyncServers)
						{
							server.OffHours = Common.OffHours(server.Name);
							Common.RecordUpAndDownTimes(server, ref testResults);
						}

					if (myDagCollection != null)
						foreach (MonitoredItems.ExchangeServer server in myDagCollection)
						{
							server.OffHours = Common.OffHours(server.Name);
							Common.RecordUpAndDownTimes(server, ref testResults);
						}

					if (myExchangeMailProbes != null)
						foreach (MonitoredItems.ExchangeServer server in myExchangeMailProbes)
						{
							server.OffHours = Common.OffHours(server.Name);
							//Common.RecordUpAndDownTimes(server, ref testResults);
						}

					CommonDB db = new CommonDB();
					db.UpdateSQLStatements(testResults, DummyServerForLogs);

				}
				catch (Exception ex)
				{
					Common.WriteDeviceHistoryEntry("All", DummyServerForLogs.ServerType, "Error setting OffHours.  Error: " + ex.Message, commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);
				}

                List<Thread> listOfThreads = new List<Thread>();



                Thread ExchangeHourly = new Thread(() =>
                {
                    try
                    { 
                        int newestVersion = -1;
                        MonitoredItems.ExchangeServer testServer = null;
                        if (myExchangeServers != null)
                            foreach (MonitoredItems.ExchangeServer server in myExchangeServers)
                            {
                                if (Convert.ToInt32(server.VersionNo) > newestVersion && server.Status != "Not Responding" && server.Status != "Maintenance" && server.Enabled)
                                {
                                    newestVersion = Convert.ToInt32(server.VersionNo);
                                    testServer = server;
                                }
                            }

                        //Runspace runspace = RunspaceFactory.CreateRunspace();

                        if (testServer != null)
                        {

                            Common.WriteDeviceHistoryEntry("All", "Exchange", "Server " + testServer.Name + " will be used to perform tests.", commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);
                            Common.WriteDeviceHistoryEntry("Exchange", DummyServerForLogs.Name, "Server " + testServer.Name + " will be used to perform tests.", commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);

                            TestResults AllTestResults = new TestResults();
                            ExchangeMB EMB = new ExchangeMB();

                            string cmdlets = "-CommandName Get-ExchangeServer, Get-MailboxDatabase, Get-MailboxStatistics, Get-Mailbox, Get-MessageTrackingLog, ";
                            cmdlets += "Get-OWAVirtualDirectory, Get-ECPVirtualDirectory, Get-OABVirtualDirectory, Get-WebServicesVirtualDirectory, Get-MAPIVirtualDirectory, Get-ActiveSyncVirtualDirectory, ";
                            cmdlets += "Get-OutlookAnywhere, Get-ClientAccessServer, Get-TransportServer, Get-MailboxRepairRequest, New-MailboxRepairRequest";

                            using (ReturnPowerShellObjects results = Common.PrereqForExchangeWithCmdlets(testServer.Name, testServer.UserName, testServer.Password, "Exchange", testServer.IPAddress, commonEnums.ServerRoles.Empty, cmdlets, testServer.AuthenticationType))
                            {
                                Collection<PSObject> DBCorruptionTest = TestDatabaseCorruptionQueue(testServer, ref AllTestResults, DummyServerForLogs.Name, results.PS);
                                checkHealthCheckPages(testServer, ref AllTestResults, DummyServerForLogs.Name, results.PS);
                                //EMB.getMailBoxInfo(testServer, ref AllTestResults, testServer.VersionNo.ToString(), DummyServerForLogs.Name, myExchangeServers, results);
                                TestDatabaseCorruptionStatus(testServer, ref AllTestResults, DummyServerForLogs.Name, results.PS, DBCorruptionTest);
                            }

                            GC.Collect();

                            Common.SetHourlyAlertsToObject(AllTestResults, myExchangeServers);

                            CommonDB DB = new CommonDB();
                            DB.UpdateSQLStatements(AllTestResults, DummyServerForLogs);
                        }
                        else
                        {
                            //no server was found to be used to scan
                        }
                    }
                    catch (Exception ex)
                    {
                        Common.WriteDeviceHistoryEntry(DummyServerForLogs.ServerType, DummyServerForLogs.Name, "Exception in Exchange Hourly Thread: " + ex.ToString(), commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);
                    }
                });
                ExchangeHourly.CurrentCulture = c;
                ExchangeHourly.IsBackground = true;
                ExchangeHourly.Priority = ThreadPriority.Normal;
                ExchangeHourly.Name = "HourlyTaskWorkerThread - Exchange Specific Tests";
                ExchangeHourly.Start();

                listOfThreads.Add(ExchangeHourly);




                Thread DAGHourly = new Thread(() =>
                {

                    if (myDagCollection != null)
                    {
                        foreach (MonitoredItems.ExchangeServer myServer in myDagCollection)
                        {
                            try
                            {
                                Common.WriteDeviceHistoryEntry(myServer.ServerType, DummyServerForLogs.Name, "Hourly test for " + myServer.Name + " starting.", commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);
                                string cmdlets = "-CommandName Get-MailboxDatabase,Get-MailboxStatistics";

                                ReturnPowerShellObjects results = Common.PrereqForExchangeWithCmdlets(myServer.Name, myServer.DAGPrimaryUserName, myServer.DAGPrimaryPassword, myServer.ServerType, myServer.DAGPrimaryIPAddress, commonEnums.ServerRoles.Empty, cmdlets, myServer.DAGPrimaryAuthenticationType);

                                string Version = "";
                                string IPAddress = "";

                                if (results.Connected == false)
                                {
                                    Common.WriteDeviceHistoryEntry(myServer.ServerType, DummyServerForLogs.Name, "Unable to connect to primary server.  Will attempt backup", commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);
                                    results.Dispose();
                                    results = Common.PrereqForExchangeWithCmdlets(myServer.Name, myServer.DAGBackupUserName, myServer.DAGBackupPassword, myServer.ServerType, myServer.DAGBackupIPAddress, commonEnums.ServerRoles.Empty, cmdlets, myServer.DAGBackupAuthenticationType);
                                    IPAddress = myServer.DAGBackupIPAddress;
                                    Version = myExchangeServers.SearchByIPAddress(IPAddress) == null ? "" : myExchangeServers.SearchByIPAddress(IPAddress).VersionNo;
                                }
                                else
                                {

                                    Common.WriteDeviceHistoryEntry(myServer.ServerType, DummyServerForLogs.Name, "Connected to primary server.", commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);
                                    IPAddress = myServer.DAGPrimaryIPAddress;
                                    Version = myExchangeServers.SearchByIPAddress(IPAddress) == null ? "" : myExchangeServers.SearchByIPAddress(IPAddress).VersionNo;

                                }

                                if (results.Connected == false)
                                {
                                    Common.WriteDeviceHistoryEntry(myServer.ServerType, DummyServerForLogs.Name, "Unable to connect to backup server.", commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);
                                    results.Dispose();
                                    return;
                                }
                                else
                                {
                                    TestResults AllTestResults = new TestResults();

                                    getMailboxDatabaseDetails(myServer, results.PS, ref AllTestResults, Version, DummyServerForLogs.Name);

                                    GC.Collect();

                                    Common.SetHourlyAlertsToObject(AllTestResults, myExchangeServers);

                                    CommonDB DB = new CommonDB();
                                    DB.UpdateSQLStatements(AllTestResults, DummyServerForLogs);
                                }
                            }
                            catch(Exception ex)
                            {
                                Common.WriteDeviceHistoryEntry(myServer.ServerType, DummyServerForLogs.Name, "Exception in Dag Hourly Thread: " + ex.ToString(), commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);
                            }

                        }

                    }

                });
                DAGHourly.CurrentCulture = c;
                DAGHourly.IsBackground = true;
                DAGHourly.Priority = ThreadPriority.Normal;
                DAGHourly.Name = "HourlyTaskWorkerThread - DAG Specific Tests";
                DAGHourly.Start();

                listOfThreads.Add(DAGHourly);


                while (listOfThreads.Where(i => i.IsAlive).Count() > 0)
                {
                    Common.WriteDeviceHistoryEntry("Exchange", DummyServerForLogs.Name, "Waiting on threads " + String.Join(",", listOfThreads.Where(i => i.IsAlive == false).Select(i => i.Name)), commonEnums.ServerRoles.Empty, Common.LogLevel.Verbose);
                    Thread.Sleep(new TimeSpan(0, 0, 30));
                }


				Common.WriteDeviceHistoryEntry("All", "Exchange", "Finished Hourly Tasks.", commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);
				Common.WriteDeviceHistoryEntry("Exchange", DummyServerForLogs.Name, "Finished Hourly Tasks.", commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);
			}
			catch (Exception ex)
			{
				Common.WriteDeviceHistoryEntry("Exchange", DummyServerForLogs.Name, "Error in Hourly Tasks Main Thread.  Error: " + ex.Message, commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);
			}

			GC.Collect();

		}



		#region CAS Tests

		private void checkHealthCheckPages(MonitoredItems.ExchangeServer myServer, ref TestResults AllTestsList, string DummyServernameForLogs, PowerShell powershell)
		{
			string strMsg = "";
			Common.WriteDeviceHistoryEntry("Exchange", DummyServernameForLogs, "In checkHealthCheckPages ", commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);
			try
			{
				System.Collections.ObjectModel.Collection<PSObject> results = new System.Collections.ObjectModel.Collection<PSObject>();
				
				System.IO.StreamReader sr = new System.IO.StreamReader(AppDomain.CurrentDomain.BaseDirectory.ToString() + "Scripts\\EX_HealthCheck.ps1"); 
				String str = sr.ReadToEnd();
				
				powershell.AddScript(str);
				results = powershell.Invoke();

				Common.WriteDeviceHistoryEntry("Exchange", DummyServernameForLogs, "checkHealthCheckPages output results: " + results.Count.ToString(), commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);

				if (results.Count > 0)
				{
					Dictionary<string, string> dict = new Dictionary<string,string>();
					foreach (PSObject ps in results)
					{

						string URL = ps.Properties["URL"].Value == null ? "" : ps.Properties["URL"].Value.ToString();
						string TestType = ps.Properties["TestType"].Value == null ? "" : ps.Properties["TestType"].Value.ToString();  //Internal URL or External URL

						//Result will always be "Error" with the current script.  Surpresses all resutls but Error
						string Result = ps.Properties["Result"].Value == null ? "" : ps.Properties["Result"].Value.ToString();
						string Server = ps.Properties["Server"].Value == null ? "" : ps.Properties["Server"].Value.ToString();

						if(!dict.ContainsKey(Server))
						{
							dict.Add(Server, "");
						}

						if(Result == "Error")
						{

							dict[Server] += "The " + TestType + " " + URL + ", ";

							
						}
					}
					foreach(string key in dict.Keys)
					{
						string dictErrors = dict[key].ToString();
						if (dictErrors.Length > 0)
						{
							strMsg = "There is an error with the following URLs :" + dictErrors.Substring(0, dictErrors.Length - 2) + ".";
						}
						else
						{
							strMsg = "There were no issues detected.";
						}

						try
						{
							MonitoredItems.ExchangeServer serv = myExchangeServers.SearchByName(key);
							if (serv == null)
							{
								Common.WriteDeviceHistoryEntry("Exchange", DummyServernameForLogs, "In checkHealthCheckPages. Cannot make an alert for server " + key + " due to it not being in the collection.", commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);
							}
							else
							{
								Common.makeAlert(!(dictErrors.Length > 0), myExchangeServers.SearchByName(key), commonEnums.AlertType.Health_Check, ref AllTestsList, strMsg, "Health Check");
							}
						}
						catch (Exception ex)
						{
							Common.WriteDeviceHistoryEntry("Exchange", DummyServernameForLogs, "In checkHealthCheckPages. Cannot make an alert for server " + key + " due to it not being in the collection.", commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);
						}
					}
					
				}

			}
			catch (Exception ex)
			{

				Common.WriteDeviceHistoryEntry("Exchange", DummyServernameForLogs, "Error in checkHealthCheckPages: " + ex.Message, commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);

			}
			finally
			{

			}
		}

		#endregion

        #region DAG

        private void getMailboxDatabaseDetails(MonitoredItems.ExchangeServer Server, PowerShell powershell, ref TestResults AllTestResults, string serverVersionNo, string DummyServerName)
        {
            try
            {
                bool overallDBSizeAlert;
                bool overallWhitespaceAlert;
                //string SqlStr = "select ServerName, DatabaseName, DatabaseSizeThreshold, WhiteSpaceThreshold from ExchangeDatabaseSettings";
                //CommonDB db = new CommonDB();
                //DataTable dt = db.GetData(SqlStr);

                Common.WriteDeviceHistoryEntry(Server.ServerType, DummyServerName, "In getMailboxDatabaseDetails.", commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);
                string ServerNames = "'" + String.Join("','", myExchangeServers.Cast<MonitoredItems.ExchangeServer>().ToArray().Select(s => s.Name).ToList()) + "'";

                CommonDB db = new CommonDB();


                System.Collections.ObjectModel.Collection<PSObject> results = new System.Collections.ObjectModel.Collection<PSObject>();
                System.IO.StreamReader sr = new System.IO.StreamReader(AppDomain.CurrentDomain.BaseDirectory.ToString() + "Scripts\\EX_MailBoxReportByTotalSize&Count.ps1");
                string startOfScript = "$databases = Get-MailboxDatabase -IncludePreExchange" + serverVersionNo + " -status | sort name\n";
                String str = startOfScript + sr.ReadToEnd();
                powershell.Streams.Error.Clear();

                powershell.Commands.Clear();
                powershell.AddScript(str);
                results = powershell.Invoke();

                if (powershell.Streams.Error.Count > 51)
                {

                    Common.WriteDeviceHistoryEntry(Server.ServerType, DummyServerName, "getMailboxDatabaseDetails received over 51 errors", commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);
                }
                else
                {
                    Common.WriteDeviceHistoryEntry(Server.ServerType, DummyServerName, "getMailboxDatabaseDetails output results: " + results.Count.ToString(), commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);
                    
                    Dictionary<MonitoredItems.ExchangeServer, String> dict = new Dictionary<MonitoredItems.ExchangeServer, string>();
                    foreach (PSObject ps in results)
                    {
                        Common.WriteDeviceHistoryEntry(Server.ServerType, DummyServerName, "getMailboxDatabaseDetails output results: " + ps.BaseObject.ToString(), commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);
                        string ServerName = ps.Properties["ServerName"].Value == null ? "" : ps.Properties["ServerName"].Value.ToString();
                        Common.WriteDeviceHistoryEntry(Server.ServerType, DummyServerName, "getMailboxDatabaseDetails output results: " + ServerName, commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);
                        string DatabaseName = ps.Properties["DataBaseName"].Value == null ? "" : ps.Properties["DataBaseName"].Value.ToString();
                        Common.WriteDeviceHistoryEntry(Server.ServerType, DummyServerName, "getMailboxDatabaseDetails output results: " + DatabaseName, commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);
                        string SizeMB = ps.Properties["SizeMB"].Value == null ? "0" : ps.Properties["SizeMB"].Value.ToString();
                        Common.WriteDeviceHistoryEntry(Server.ServerType, DummyServerName, "getMailboxDatabaseDetails output results: " + SizeMB, commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);
                        string WhiteSpaceMB = ps.Properties["WhiteSpaceMB"].Value == null ? "0" : ps.Properties["WhiteSpaceMB"].Value.ToString();
                        Common.WriteDeviceHistoryEntry(Server.ServerType, DummyServerName, "getMailboxDatabaseDetails output results: " + WhiteSpaceMB, commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);
                        string MBXs = ps.Properties["MBXs"].Value == null ? "0" : ps.Properties["MBXs"].Value.ToString();
                        Common.WriteDeviceHistoryEntry(Server.ServerType, DummyServerName, "getMailboxDatabaseDetails output results: " + MBXs, commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);
                        string MBXsdisc = ps.Properties["MBXsdisc"].Value == null ? "0" : ps.Properties["MBXsdisc"].Value.ToString();
                        Common.WriteDeviceHistoryEntry(Server.ServerType, DummyServerName, "getMailboxDatabaseDetails output results: " + MBXsdisc, commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);
                        string Mbcount = ps.Properties["Mbcount"].Value == null ? "0" : ps.Properties["Mbcount"].Value.ToString();
                        Common.WriteDeviceHistoryEntry(Server.ServerType, DummyServerName, "getMailboxDatabaseDetails output results: " + Mbcount, commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);
                        string DagName = ps.Properties["DagName"] == null || ps.Properties["DagName"].Value == null ? "" : ps.Properties["DagName"].Value.ToString();
                        Common.WriteDeviceHistoryEntry(Server.ServerType, DummyServerName, "getMailboxDatabaseDetails output results: " + DagName, commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);

                        if (myDagCollection.SearchByName(DagName) != null)
                        {
                            var currServer = myDagCollection.SearchByName(DagName);

                            MongoStatementsUpdate<VSNext.Mongo.Entities.Status> mongoUpdate = new MongoStatementsUpdate<VSNext.Mongo.Entities.Status>();
                            mongoUpdate.filterDef = mongoUpdate.repo.Filter.Eq(i => i.DeviceId, currServer.ServerObjectID)
                                & !mongoUpdate.repo.Filter.ElemMatch(i => i.DagDatabases, i => i.DatabaseName == DatabaseName);
                            VSNext.Mongo.Entities.DagDatabases dbg = new VSNext.Mongo.Entities.DagDatabases()
                            {
                                DatabaseName = DatabaseName
                            };
                            mongoUpdate.updateDef = mongoUpdate.repo.Updater.Push(i => i.DagDatabases, dbg);
                            AllTestResults.MongoEntity.Add(mongoUpdate);

                            mongoUpdate = new MongoStatementsUpdate<VSNext.Mongo.Entities.Status>();
                            mongoUpdate.filterDef = mongoUpdate.repo.Filter.Eq(i => i.DeviceId, currServer.ServerObjectID) &
                                mongoUpdate.repo.Filter.ElemMatch(i => i.DagDatabases, i => i.DatabaseName == DatabaseName);
                            mongoUpdate.updateDef = mongoUpdate.repo.Updater
                                .Set(i => i.DagDatabases[-1].ConnectedMailboxCount, Convert.ToInt32(Mbcount))
                                .Set(i => i.DagDatabases[-1].DisconnectedMailboxCount, Convert.ToInt32(MBXsdisc))
                                .Set(i => i.DagDatabases[-1].MailboxCount, Convert.ToInt32(MBXs))
                                .Set(i => i.DagDatabases[-1].SizeMB, Convert.ToDouble(SizeMB))
                                .Set(i => i.DagDatabases[-1].WhiteSpaceMB, Convert.ToDouble(WhiteSpaceMB))
                                .Set(i => i.DagDatabases[-1].ServerName, ServerName);
                            AllTestResults.MongoEntity.Add(mongoUpdate);


                            AllTestResults.MongoEntity.Add(Common.GetInsertIntoDailyStats(Server, "ExDatabaseSizeMb." + DatabaseName, SizeMB));
                        }

                        //alerts
                        string allDatabases = "AllDatabases";
                        if (Server.DagDatabaseSettings != null && Server.DagDatabaseSettings.Exists(x => (x.ServerName == ServerName || x.ServerName == allDatabases) && (x.DatabaseName == DatabaseName || x.DatabaseName == allDatabases)))
                        {
                            MonitoredItems.ExchangeServer.DagDatabaseSetting dagSetting = Server.DagDatabaseSettings.Find(x => (x.ServerName == ServerName || x.ServerName == allDatabases) && (x.DatabaseName == DatabaseName || x.DatabaseName == allDatabases));

                            try
                            {
                                MonitoredItems.ExchangeServer currServer = myExchangeServers.SearchByName(ServerName);

                                if (currServer != null)
                                {
                                    if (!dict.ContainsKey(currServer))
                                        dict.Add(currServer, "");

                                    string sizeThreshold = dagSetting.DatabaseSizeThreshold.ToString();
                                    string whitespaceThreshold = dagSetting.WhiteSpaceThreshold.ToString();

                                    double dblWhiteSpace;
                                    double dblSize;

                                    bool boolWhiteSpace = Double.TryParse(WhiteSpaceMB, out dblWhiteSpace);
                                    bool boolSize = Double.TryParse(SizeMB, out dblSize);

                                    if (boolWhiteSpace)
                                        boolWhiteSpace = Double.Parse(WhiteSpaceMB) > Double.Parse(whitespaceThreshold) && Double.Parse(whitespaceThreshold) != 0;
                                    if (boolSize)
                                        boolSize = Double.Parse(SizeMB) > Double.Parse(sizeThreshold) && Double.Parse(sizeThreshold) != 0;

                                    string details = "";

                                    if (boolSize && boolWhiteSpace)
                                    {
                                        details = "The whtiespace and the size of database " + DatabaseName + " excedes the thresholds";
                                        dict[currServer] += "The whitespace and the size of database " + DatabaseName + " excedes the thresholds. ";
                                        overallDBSizeAlert = false;
                                        overallWhitespaceAlert = false;
                                    }
                                    else if (boolSize)
                                    {
                                        details = "The size of database " + DatabaseName + " excedes the threshold limit of " + Double.Parse(sizeThreshold);
                                        dict[currServer] += "The size of database " + DatabaseName + "  is " + Math.Round(Double.Parse(SizeMB), 2) + " MB and excedes the threshold of " + sizeThreshold + " MB. ";
                                        overallDBSizeAlert = false;
                                    }
                                    else if (boolWhiteSpace)
                                    {
                                        details = "The size of the whitespace of database " + DatabaseName + " excedes the threshold of " + Double.Parse(whitespaceThreshold);
                                        dict[currServer] += "The whitespace of database " + DatabaseName + " is " + Math.Round(Double.Parse(WhiteSpaceMB), 2) + " MB and  excedes the threshold of " + whitespaceThreshold + " MB. ";
                                        overallWhitespaceAlert = false;
                                    }
                                    else
                                    {
                                        details = "Database " + DatabaseName + " is within thresholds";
                                    }
                                        
                                    if (boolSize || boolWhiteSpace)
                                        Common.makeAlert(false, currServer, commonEnums.AlertType.Mailbox_Database_Size, ref AllTestResults, details, "MailBox");
                                    else
                                        Common.makeAlert(true, currServer, commonEnums.AlertType.Mailbox_Database_Size, ref AllTestResults, details, "MailBox");
                                         
                                }
                            }
                            catch (Exception ex)
                            {
                            }
                            
                        }


                        

                    }
                    foreach (MonitoredItems.ExchangeServer currServer in dict.Keys)
                    {

                        string details = dict[currServer];

                        if (dict[currServer] == "")
                            Common.makeAlert(false, Server, commonEnums.AlertType.Mailbox_Database_Size, ref AllTestResults, details, "MailBox");
                        else
                            Common.makeAlert(true, Server, commonEnums.AlertType.Mailbox_Database_Size, ref AllTestResults, "The size and whitespace of all databases are below their threshold values.", "MailBox");
                    }

                }

            }
            catch (Exception ex)
            {
                StackTrace st = new StackTrace(ex, true);
                StackFrame[] frames = st.GetFrames();

                foreach (StackFrame frame in frames)
                    Common.WriteDeviceHistoryEntry(Server.ServerType, DummyServerName, "Error in getMailboxDatabaseDetails : " + frame.GetFileLineNumber(), commonEnums.ServerRoles.Empty, Common.LogLevel.Normal); 

                Common.WriteDeviceHistoryEntry(Server.ServerType, DummyServerName, "Error in getMailboxDatabaseDetails : " + ex.ToString(), commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);

            }

        }


        #endregion

        public Collection<PSObject> TestDatabaseCorruptionQueue(MonitoredItems.ExchangeServer myServer, ref TestResults AllTestsList, string DummyServernameForLogs, PowerShell powershell)
        {

            Collection<PSObject> results = null;

            Common.WriteDeviceHistoryEntry(myServer.ServerType, DummyServernameForLogs, "In TestDatabaseCorruptionQueue ", commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);
            try
            {
                results = new System.Collections.ObjectModel.Collection<PSObject>();

                System.IO.StreamReader sr = new System.IO.StreamReader(AppDomain.CurrentDomain.BaseDirectory.ToString() + "Scripts\\EX_DatabaseCorruptionCheck_Queue.ps1");
                String str = sr.ReadToEnd();

                powershell.AddScript(str);
                results = powershell.Invoke();

                Common.WriteDeviceHistoryEntry(myServer.ServerType, DummyServernameForLogs, "EX_DatabaseCorruptionCheck_Queue output results: " + results.Count.ToString(), commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);

            }
            catch (Exception ex)
            {

                Common.WriteDeviceHistoryEntry(myServer.ServerType, DummyServernameForLogs, "Error in TestDatabaseCorruptionQueue: " + ex.Message, commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);

            }
            finally
            {

            }

            return results;

        }

        public void TestDatabaseCorruptionStatus(MonitoredItems.ExchangeServer myServer, ref TestResults AllTestsList, string DummyServernameForLogs, PowerShell powershell, Collection<PSObject> input)
        {

            Common.WriteDeviceHistoryEntry(myServer.ServerType, DummyServernameForLogs, "In TestDatabaseCorruptionStatus ", commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);
            try
            {
                System.Collections.ObjectModel.Collection<PSObject> results = new System.Collections.ObjectModel.Collection<PSObject>();

                System.IO.StreamReader sr = new System.IO.StreamReader(AppDomain.CurrentDomain.BaseDirectory.ToString() + "Scripts\\EX_DatabaseCorruptionCheck_Status.ps1");
                String str = sr.ReadToEnd();

                PSCommand cmd = new PSCommand();
                cmd.AddScript(str);
                cmd.AddParameter("Identities", input);
                powershell.Commands = cmd;
                results = powershell.Invoke();

                Common.WriteDeviceHistoryEntry(myServer.ServerType, DummyServernameForLogs, "EX_DatabaseCorruptionCheck_Status output results: " + results.Count.ToString(), commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);




                if (results.Count > 0)
                {

                    List<DatabaseCorruptionResult> objList = new List<DatabaseCorruptionResult>();

                    foreach (PSObject ps in results)
                    {

                        string DatabaseName = ps.Properties["DatabaseName"].Value == null ? "" : ps.Properties["DatabaseName"].Value.ToString();
                        string ServerName = ps.Properties["ServerName"].Value == null ? "" : ps.Properties["ServerName"].Value.ToString();
                        string CorruptionsDetected = ps.Properties["CorruptionsDetected"].Value == null ? "" : ps.Properties["CorruptionsDetected"].Value.ToString();
                        string ErrorCode = ps.Properties["ErrorCode"].Value == null ? "" : ps.Properties["ErrorCode"].Value.ToString();
                        string Corruptions = ps.Properties["Corruptions"].Value == null ? "" : ps.Properties["Corruptions"].Value.ToString();
                        string Progress = ps.Properties["Progress"].Value == null ? "" : ps.Properties["Progress"].Value.ToString();
                        string JobState = ps.Properties["JobState"].Value == null ? "" : ps.Properties["JobState"].Value.ToString();

                        objList.Add(new DatabaseCorruptionResult
                        {
                            DatabaseName = DatabaseName,
                            ServerName = ServerName,
                            CorruptionsDetected = CorruptionsDetected,
                            ErrorCode = ErrorCode,
                            Corruptions = Corruptions,
                            Progress = Progress,
                            JobState = JobState
                            
                        });

                        Common.WriteDeviceHistoryEntry(myServer.ServerType, DummyServernameForLogs, "TestDatabaseCorruptionQueue Object Results: ", commonEnums.ServerRoles.Empty, Common.LogLevel.Verbose);
                        Common.WriteDeviceHistoryEntry(myServer.ServerType, DummyServernameForLogs, "DatabaseName: " + DatabaseName, commonEnums.ServerRoles.Empty, Common.LogLevel.Verbose);
                        Common.WriteDeviceHistoryEntry(myServer.ServerType, DummyServernameForLogs, "ServerName: " + ServerName, commonEnums.ServerRoles.Empty, Common.LogLevel.Verbose);
                        Common.WriteDeviceHistoryEntry(myServer.ServerType, DummyServernameForLogs, "CorruptionsDetected: " + CorruptionsDetected, commonEnums.ServerRoles.Empty, Common.LogLevel.Verbose);
                        Common.WriteDeviceHistoryEntry(myServer.ServerType, DummyServernameForLogs, "ErrorCode: " + ErrorCode, commonEnums.ServerRoles.Empty, Common.LogLevel.Verbose);
                        Common.WriteDeviceHistoryEntry(myServer.ServerType, DummyServernameForLogs, "Corruptions: " + Corruptions, commonEnums.ServerRoles.Empty, Common.LogLevel.Verbose);
                        Common.WriteDeviceHistoryEntry(myServer.ServerType, DummyServernameForLogs, "Progress: " + Progress, commonEnums.ServerRoles.Empty, Common.LogLevel.Verbose);
                        Common.WriteDeviceHistoryEntry(myServer.ServerType, DummyServernameForLogs, "JobState: " + JobState, commonEnums.ServerRoles.Empty, Common.LogLevel.Verbose);


                    }


                    foreach (string server in objList.Select(l => l.ServerName).Distinct())
                    {
                        string msg = "";
                        bool reset;
                        List<DatabaseCorruptionResult> tempList = objList.Where(l => l.ServerName == server).ToList();
                        if(tempList.Where(l => l.CorruptionsDetected != "0").ToList().Count() > 0)
                        {
                            int TotalNumOfCorruptions = tempList.Sum(l => Convert.ToInt32(l.CorruptionsDetected));
                            msg = "There were corruptions detected on databases " + String.Join(", ", tempList.Where(l => l.CorruptionsDetected != "0").Select(i => i.DatabaseName).Distinct());
                        }
                        else if (tempList.Where(l => l.Progress != "100").ToList().Count() > 0)
                        {
                            int NumOfUnfinishedScans = tempList.Where(l => l.Progress != "100").ToList().Count();
                            if (NumOfUnfinishedScans <= 3)
                            {
                                msg = "The databases " + String.Join(", ", tempList.Where(l => l.Progress != "100").Select(i => i.DatabaseName).Distinct()) + " did not finish the corruption test in the allowed time. This can indicate there might be an issue with the server";
                            }
                            else
                            {
                                msg = "There were " + NumOfUnfinishedScans + " databases that did not finish the corruption test in the allowed time. This can indicate there might be an issue with the server";
                            }
                        }

                        if(msg == "")
                        {
                            msg = "No issues were detected";
                            reset = true;
                        }
                        else
                        {
                            reset = false;
                        }
                        try
                        {
                            MonitoredItems.MicrosoftServer serv = myExchangeServers.SearchByName(server);
                            if (serv != null)
                                Common.makeAlert(reset, serv, commonEnums.AlertType.Database_Corruption, ref AllTestsList, msg, "Database");
                        }
                        catch(Exception ex)
                        {

                        }
                    }

                }






            }
            catch (Exception ex)
            {

                Common.WriteDeviceHistoryEntry(myServer.ServerType, DummyServernameForLogs, "Error in TestDatabaseCorruptionQueue: " + ex.Message, commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);

            }
            finally
            {

            }

        }

        class DatabaseCorruptionResult
        {

            public string DatabaseName;
            public string ServerName;
            public string CorruptionsDetected;
            public string ErrorCode;
            public string Corruptions;
            public string Progress;
            public string JobState;

        }



		#endregion



		//MailProbe not set up for HA.  If we use again, change queries to reflect HA changes
		#region MailProbe
		private MonitoredItems.ExchangeServer SetMailProbeSettings(MonitoredItems.ExchangeServer MyExchangeServer, DataRow DR)
		{
            MyExchangeServer.ServerType = VSNext.Mongo.Entities.Enums.ServerType.ExchangeMailFlow.ToDescription();
			MyExchangeServer.IPAddress = DR["IPAddress"].ToString();
			MyExchangeServer.Name = DR["NAME"].ToString();
			MyExchangeServer.UserName = DR["UserID"].ToString();
			MyExchangeServer.Password = decodePasswordFromEncodedString(DR["Password"].ToString(), MyExchangeServer.Name);
			MyExchangeServer.Location = DR["Location"].ToString();
			MyExchangeServer.Role = new String[0];
			MyExchangeServer.ScanInterval = int.Parse(DR["ScanInterval"].ToString());
			MyExchangeServer.OffHoursScanInterval = int.Parse(DR["OffHoursScanInterval"].ToString());
			MyExchangeServer.RetryInterval = int.Parse(DR["RetryInterval"].ToString());
			MyExchangeServer.DeliveryThreshold = int.Parse(DR["DELIVERYTHRESHOLD"].ToString());
			MyExchangeServer.MailProbeAddress = DR["EXCHANGEMAILADDRESS"].ToString();
			MyExchangeServer.MailProbeName = DR["NAME"].ToString();
			MyExchangeServer.MailProbeSourceServer = DR["SERVERNAME"].ToString();
			MyExchangeServer.LastScan = DateTime.Now.AddMinutes(-30);
			MyExchangeServer.Status = "Not Scanned";
			MyExchangeServer.StatusCode = "Maintenance";


			MyExchangeServer.ServerType = "Exchange Mail Flow";
			MyExchangeServer.Category = "MailFlow";

			MyExchangeServer.Enabled = true;

			MyExchangeServer.AuthenticationType = DR["AuthenticationType"] != null && DR["AuthenticationType"] != "" ? DR["AuthenticationType"].ToString() : "Default";

			return MyExchangeServer;
		}
		int mailProbeThreadCount = 0;
		int initialMailProbeThreadCount = 0;

		private void StartMailProbeThreads()
		{

			//int startThreads = 0;
			//mailProbeThreadCount = myExchangeMailProbes.Count / 3;
			//if (mailProbeThreadCount <= 1)
			//    mailProbeThreadCount = 2;

			//if (mailProbeThreadCount > 34)
			//    mailProbeThreadCount = 35;
			//startThreads = initialMailProbeThreadCount;
			//if (initialMailProbeThreadCount > mailProbeThreadCount)
			//{
			//    //remove the extra threads
			//    int j = initialMailProbeThreadCount - mailProbeThreadCount;
			//    //if inital threads are 5 and current threads are 3
			//    //5-3=2: //remove 2 threads
			//    foreach (Thread th in AliveExchangeMainThreads)
			//    {
			//        if (j > 0)
			//        {
			//            if (th.IsAlive)
			//                th.Abort();
			//            j -= 1;
			//        }
			//    }
			//}
			//initialMailProbeThreadCount = mailProbeThreadCount;
			if (c == null)
				c = new CultureInfo("en-US");
            //for (int i = startThreads; i < mailProbeThreadCount; i++)
            //{
            CreateExchangeMailProbeCollection();
				Thread MainMailProbeThread = new Thread(new ThreadStart(GetMailFlowHeatMap));
				MainMailProbeThread.CurrentCulture = c == null ? new CultureInfo("en-US") : c;  //Should only be null on our local copies if using wrapper
				MainMailProbeThread.IsBackground = true;
				MainMailProbeThread.Name = "Main MailProbe Thread";
				MainMailProbeThread.Priority = ThreadPriority.Normal;
				MainMailProbeThread.Start();
				Thread.Sleep(2000);
			//}

			

		}
		//private void MonitorMailProbe()
		//{
		//    CommonDB DB = new CommonDB();
		//    while (true)
		//    {
		//        try
		//        {
		//            MonitoredItems.ExchangeServer thisServer = SelectMailProbeServerToMonitor();
		//            if (thisServer != null && !thisServer.IsBeingScanned)
		//            {
		//                Common.WriteDeviceHistoryEntry("All", thisServer.ServerType , "Scanning Server " + thisServer.Name + " on thread ");
		//                thisServer.IsBeingScanned = true;

		//                TestResults AllTestResults = new TestResults();
		//                ExchangeMailFlow mailFlow = new ExchangeMailFlow();
		//                mailFlow.PrereqForWindows(thisServer, ref AllTestResults);
						
		//                thisServer.LastScan = DateTime.Now;
		//                DB.UpdateAllTests(AllTestResults, thisServer, thisServer.ServerType);
		//                thisServer.IsBeingScanned = false;
		//            }
		//            else
		//            {
		//                try
		//                {
		//                    Common.WriteDeviceHistoryEntry("ALL", thisServer.ServerType, "Server " + thisServer.Name + " is already being scanned and will not start scanning again");
		//                }
		//                catch (Exception ex)
		//                {
		//                    Common.WriteDeviceHistoryEntry("ALL", thisServer.ServerType, "Server returned as null");
		//                }
		//            }
		//        }
		//        catch
		//        {

		//        }


		//        Common.WriteDeviceHistoryEntry("All", "Exchange", "Waiting for 5 seconds to restart the Loop ");
		//        // Sleep for 1 minutes 
		//        Thread.Sleep(1000 * 60);
		//        //break;
		//    }

		//}

		private void GetMailFlowHeatMap()
		{
            Thread.CurrentThread.CurrentCulture = c;
            CommonDB DB = new CommonDB();

            while (true)
            {
                MonitoredItems.ExchangeMailProbe thisServer;
                try
                {
                    thisServer = Common.SelectServerToMonitor(myExchangeMailProbes) as MonitoredItems.ExchangeMailProbe;

                    if (thisServer != null && !thisServer.IsBeingScanned)
                    {
                        thisServer.IsBeingScanned = true;
                    }
                }
                catch (Exception ex)
                {
                    thisServer = null;
                }
                finally
                {
                    
                }

                if (thisServer != null)
                {
                    Common.WriteDeviceHistoryEntry("All", "ExchangeMailProbe", "Scanning Server " + thisServer.Name);
                    thisServer.IsBeingScanned = true;
                    
                    TestResults AllTestResults = new TestResults();

                    Thread MailFlowThread = new Thread(() =>
                    {
                        ExchangeMailFlow mailFlowTest = new ExchangeMailFlow();
                        mailFlowTest.PrereqForWindows(thisServer, ref AllTestResults);

                        DB.UpdateAllTests(AllTestResults, thisServer, thisServer.ServerType);
                    });
                    MailFlowThread.CurrentCulture = c;
                    MailFlowThread.IsBackground = true;
                    MailFlowThread.Priority = ThreadPriority.Normal;
                    MailFlowThread.Name = "MailFlowWorkerThread";
                    MailFlowThread.Start();

                    if(!MailFlowThread.Join((int)(1000 * 60 * 5 *+thisServer.ExchangeServers.Count * thisServer.LatencyRedThreshold))){
                        Common.WriteDeviceHistoryEntry("All", "ExchangeMailProbe", "Thread has exceded the time " + (thisServer.ExchangeServers.Count * thisServer.LatencyRedThreshold) + "ms and has exited");
                        Common.WriteDeviceHistoryEntry(thisServer.ServerType, thisServer.Name, "Thread has exceded the time " + (thisServer.ExchangeServers.Count * thisServer.LatencyRedThreshold) + "ms and has exited");
                    }

                    thisServer.LastScan = DateTime.Now;
                    thisServer.IsBeingScanned = false;                   
                }



				
			}

		}
		#endregion
		

		#region DAG
		private void CreateExchangeDAGCollection()
		{
			//Fetch all servers
			if (myDagCollection == null)
				myDagCollection = new MonitoredItems.ExchangeServersCollection();

            CommonDB DB = new CommonDB();
            VSFramework.TripleDES tripleDes = new TripleDES();
            List<VSNext.Mongo.Entities.Server> listOfServers = new List<Server>();
            List<VSNext.Mongo.Entities.Server> listOfExchangeServers = new List<Server>();
            List<VSNext.Mongo.Entities.Status> listOfStatus = new List<Status>();
            List<VSNext.Mongo.Entities.Credentials> listOfCredentials = new List<Credentials>();
            List<VSNext.Mongo.Entities.Location> listOfLocations = new List<Location>();
            string NodeName = null;
            try
            {

                NodeName = ConfigurationManager.AppSettings["VSNodeName"].ToString();

                VSNext.Mongo.Repository.Repository<VSNext.Mongo.Entities.Server> repository = new VSNext.Mongo.Repository.Repository<VSNext.Mongo.Entities.Server>(DB.GetMongoConnectionString());
                FilterDefinition<VSNext.Mongo.Entities.Server> filterDef = repository.Filter.Eq(x => x.DeviceType, VSNext.Mongo.Entities.Enums.ServerType.DatabaseAvailabilityGroup.ToDescription());

                ProjectionDefinition<VSNext.Mongo.Entities.Server> projectionDef = repository.Project
                    .Include(x => x.Id)
                    .Include(x => x.DeviceName)
                    .Include(x => x.DeviceType)
                    .Include(x => x.LocationId)
                    .Include(x => x.OffHoursScanInterval)
                    .Include(x => x.RetryInterval)
                    .Include(x => x.ScanInterval)
                    .Include(x => x.Category)
                    .Include(x => x.ReplyQueueThreshold)
                    .Include(x => x.CopyQueueThreshold)
                    .Include(x => x.PrimaryServerId)
                    .Include(x => x.BackupServerId)
                    .Include(x => x.CurrentNode)
                    .Include(x => x.DatabaseInfo);
                listOfServers = repository.Find(filterDef, projectionDef).ToList();

                VSNext.Mongo.Repository.Repository<VSNext.Mongo.Entities.Status> repositoryStatus = new VSNext.Mongo.Repository.Repository<Status>(DB.GetMongoConnectionString());
                FilterDefinition<VSNext.Mongo.Entities.Status> filterDefStatus = repositoryStatus.Filter.Eq(x => x.DeviceType, VSNext.Mongo.Entities.Enums.ServerType.DatabaseAvailabilityGroup.ToDescription());
                ProjectionDefinition<VSNext.Mongo.Entities.Status> projectionDefStatus = repositoryStatus.Project
                    .Include(x => x.DeviceId)
                    .Include(x => x.StatusCode)
                    .Include(x => x.CurrentStatus)
                    .Include(x => x.LastUpdated)
                    .Include(x => x.DeviceType)
                    .Include(x => x.DeviceName);

                listOfStatus = repositoryStatus.Find(filterDefStatus, projectionDefStatus).ToList();

                VSNext.Mongo.Repository.Repository<VSNext.Mongo.Entities.Credentials> repositoryCredentials = new VSNext.Mongo.Repository.Repository<Credentials>(DB.GetMongoConnectionString());
                listOfCredentials = repositoryCredentials.Find(x => true).ToList();
                VSNext.Mongo.Repository.Repository<VSNext.Mongo.Entities.Location> repositoryLocation = new VSNext.Mongo.Repository.Repository<Location>(DB.GetMongoConnectionString());
                listOfLocations = repositoryLocation.Find(x => true).ToList();

                listOfExchangeServers = repository.Find(repository.Filter.Eq(x => x.DeviceType, Enums.ServerType.Exchange.ToDescription())).ToList();
            }
            catch (Exception ex)
            {
                Common.WriteDeviceHistoryEntry("All", Enums.ServerType.DatabaseAvailabilityGroup.ToDescription(), "Exception in CreateExchangeDAGCollection when getting the data from the db. Exception: " + ex.Message.ToString(), Common.LogLevel.Normal);
            }



            MonitoredItems.ExchangeServersCollection newCollection = new MonitoredItems.ExchangeServersCollection();
			
            if (listOfServers.Count > 0)
            {
                List<string> ServerNameList = new List<string>();

                int updatedServers = 0;
                int newServers = 0;
                int removedServers = 0;

                for (int i = 0; i < listOfServers.Count; i++)
                {
                    VSNext.Mongo.Entities.Server entity = listOfServers[i];
                    ServerNameList.Add(entity.DeviceName.ToString());

                    MonitoredItems.ExchangeServer myDagServer = null;// = new MonitoredItems.ExchangeServer();
                    VSNext.Mongo.Entities.Status statusEntry = listOfStatus.Find(x => x.DeviceId == entity.Id);
                    //Checks to see if the server is newly added or exists.  Adds if it is new
                    try
                    {
                        try
                        {
                            myDagServer = myDagCollection.SearchByName(entity.DeviceName.ToString());
                        }
                        catch (Exception ex)
                        {

                        }

                        if (myDagServer == null)
                        {
                            //New server.  Set inits and add to collection

                            myDagServer = new MonitoredItems.ExchangeServer();
                            myDagServer.Role = new String[0];
                            myDagServer.ServerType = Enums.ServerType.DatabaseAvailabilityGroup.ToDescription();

                            myDagServer.LastScan = statusEntry == null || !statusEntry.LastUpdated.HasValue || statusEntry.LastUpdated.ToString() == "" ? DateTime.Now.AddHours(-1) : statusEntry.LastUpdated.Value;
                            myDagServer.Status = statusEntry == null || statusEntry.CurrentStatus.ToString() == "" ? "Not Scanned" : statusEntry.CurrentStatus;
                            myDagServer.StatusCode = statusEntry == null || statusEntry.StatusCode.ToString() == "" ? "Maintenance" : statusEntry.StatusCode;

                            myDagCollection.Add(myDagServer);
                            newServers++;

                        }
                        else
                        {
                            updatedServers++;
                        }
                    }
                    catch (Exception ex)
                    {
                        Common.WriteDeviceHistoryEntry("All", Enums.ServerType.DatabaseAvailabilityGroup.ToDescription(), "Exception in CreateExchangeDAGCollection when init new server. Exception: " + ex.Message.ToString(), Common.LogLevel.Normal);
                    }

                    myDagServer.ServerObjectID = entity.Id;
                    myDagServer.Name = entity.DeviceName;
                    myDagServer.ScanInterval = entity.ScanInterval.HasValue ? entity.ScanInterval.Value : 8;
                    myDagServer.OffHoursScanInterval = entity.OffHoursScanInterval.HasValue ? entity.OffHoursScanInterval.Value : 10;
                    myDagServer.RetryInterval = entity.RetryInterval.HasValue ? entity.RetryInterval.Value : 3;
                    myDagServer.Enabled = true;
                    myDagServer.AuthenticationType = entity.AuthenticationType != null && entity.AuthenticationType != "" ? entity.AuthenticationType : "Default";
                    myDagServer.CurrentNode = entity.CurrentNode;
                    try
                    {
                        if(entity.PrimaryServerId != null)
                        {
                            var primaryServer = listOfExchangeServers.Where(x => x.Id == entity.PrimaryServerId).First();
                            var creds = listOfCredentials.Where(x => x.Id == primaryServer.CredentialsId).First();
                            var password = tripleDes.Decrypt(creds.Password);

                            myDagServer.DAGPrimaryAuthenticationType = primaryServer.AuthenticationType;
                            myDagServer.DAGPrimaryIPAddress = primaryServer.IPAddress;
                            myDagServer.DAGPrimaryPassword = password;
                            myDagServer.DAGPrimaryUserName = creds.UserId;

                        }
                    }
                    catch(Exception ex)
                    {

                    }

                    try
                    {
                        if (entity.BackupServerId != null)
                        {
                            var backupServer = listOfExchangeServers.Where(x => x.Id == entity.BackupServerId).First();
                            var creds = listOfCredentials.Where(x => x.Id == backupServer.CredentialsId).First();
                            var password = tripleDes.Decrypt(creds.Password);

                            myDagServer.DAGBackupAuthenticationType = backupServer.AuthenticationType;
                            myDagServer.DAGBackupIPAddress = backupServer.IPAddress;
                            myDagServer.DAGBackupPassword = password;
                            myDagServer.DAGBackupUserName = creds.UserId;

                        }
                    }
                    catch (Exception ex)
                    {

                    }

                    try
                    {
                        var status = listOfStatus.Where(x => x.DeviceId == entity.Id).First();
                        myDagServer.Status = status.CurrentStatus;
                        myDagServer.StatusCode = status.StatusCode;
                        myDagServer.LastScan = status.LastUpdated.HasValue ? status.LastUpdated.Value : DateTime.Now.AddHours(-1);
                    }
                    catch (Exception ex)
                    {

                    }

                    myDagServer.DAGCopyQueueThreshold = entity.CopyQueueThreshold.HasValue ? entity.CopyQueueThreshold.Value : 10;
                    myDagServer.DAGReplyQueueThreshold = entity.ReplyQueueThreshold.HasValue ? entity.ReplyQueueThreshold.Value : 10;

                    if(entity.DatabaseInfo != null)
                    {
                        if (myDagServer.DagDatabaseSettings == null) myDagServer.DagDatabaseSettings = new List<MonitoredItems.ExchangeServer.DagDatabaseSetting>();
                        foreach(VSNext.Mongo.Entities.DagDatabases currDatabase in entity.DatabaseInfo)
                        {
                            if(!myDagServer.DagDatabaseSettings.Exists(x => x.DatabaseName == currDatabase.DatabaseName && x.ServerName == currDatabase.ServerName))
                            {
                                myDagServer.DagDatabaseSettings.Add(new MonitoredItems.ExchangeServer.DagDatabaseSetting()
                                {
                                    ServerName = currDatabase.ServerName,
                                    DatabaseName = currDatabase.DatabaseName,
                                    CopyQueueThreshold = currDatabase.CopyThreshold.HasValue ? currDatabase.CopyThreshold.Value : 0,
                                    ReplayQueueThreshold = currDatabase.ReplayThreshold.HasValue ? currDatabase.ReplayThreshold.Value : 0,
                                    WhiteSpaceThreshold = currDatabase.DatabaseWhiteTSpaceThreshold.HasValue ? currDatabase.DatabaseWhiteTSpaceThreshold.Value : 0,
                                    DatabaseSizeThreshold = currDatabase.DatabaseSizeThreshold.HasValue ? currDatabase.DatabaseSizeThreshold.Value : 0
                                });
                            }
                            myDagServer.DagDatabaseSettings.Find(x => x.DatabaseName == currDatabase.DatabaseName && x.ServerName == currDatabase.ServerName).CopyQueueThreshold = currDatabase.CopyThreshold.Value;
                            myDagServer.DagDatabaseSettings.Find(x => x.DatabaseName == currDatabase.DatabaseName && x.ServerName == currDatabase.ServerName).CopyQueueThreshold = currDatabase.ReplayThreshold.Value;
                        }
                        
                    }

                    updatedServers++;

                }
                
                //Removes servers not in the new lsit
                foreach (MonitoredItems.ExchangeServer server in myDagCollection)
                {
                    string currName = server.Name;
                    try
                    {
                        //MonitoredItems.ExchangeServer newServer = newCollection.SearchByName(currName);
                        if (!ServerNameList.Contains(currName))
                        {
                            //incase it doesnt throw and exception, if not found, removed server from list
                            myDagCollection.Delete(currName);
                            removedServers++;

                        }
                    }
                    catch (Exception ex)
                    {
                        //server not found
                        myDagCollection.Delete(currName);
                        removedServers++;

                    }

                }
                
                //myExchangeServers = newCollection;
                /**********************************************************/
                Common.WriteDeviceHistoryEntry("All", Enums.ServerType.DatabaseAvailabilityGroup.ToDescription(), "There are " + myDagCollection.Count + " servers in the DAG collection.  " + newServers + " were new and " + updatedServers + " were updated.");
            }
            else
            {
                myDagCollection = new MonitoredItems.ExchangeServersCollection();
            }
            Common.WriteDeviceHistoryEntry("All", Enums.ServerType.DatabaseAvailabilityGroup.ToDescription(), "There are " + myDagCollection.Count + " servers in the DAG collection before node check.");
            Common.InsertInsufficentLicenses(myDagCollection);
            Common.WriteDeviceHistoryEntry("All", Enums.ServerType.DatabaseAvailabilityGroup.ToDescription(), "There are " + myDagCollection.Count + " servers in the DAG collection after node check.");
            //At this point we have all Servers with ALL the information(including Threshold settings)
        }

		private MonitoredItems.ExchangeServer SetDAGSettings(MonitoredItems.ExchangeServer MyExchangeServer, DataRow DR)
		{
            MyExchangeServer.ServerId = DR["ID"].ToString();
			MyExchangeServer.Name = DR["ServerName"].ToString();
			MyExchangeServer.Location = DR["Location"].ToString();
			MyExchangeServer.ScanInterval = int.Parse(DR["ScanInterval"].ToString());
			MyExchangeServer.OffHoursScanInterval = int.Parse(DR["OffHourInterval"].ToString());
			MyExchangeServer.RetryInterval = int.Parse(DR["RetryInterval"].ToString());
			MyExchangeServer.LastScan = DR["LastUpdate"] == null || DR["LastUpdate"].ToString() == "" ? DateTime.Now.AddMinutes(-30) : DateTime.Parse(DR["LastUpdate"].ToString());
			MyExchangeServer.Status = DR["Status"] == null || DR["Status"].ToString() == "" ? "Not Scanned" : DR["Status"].ToString();
			MyExchangeServer.StatusCode = DR["StatusCode"] == null || DR["StatusCode"].ToString() == "" ? "Maintenance" : DR["StatusCode"].ToString();
			MyExchangeServer.ServerType = "Database Availability Group";
			MyExchangeServer.Category = "Database Availability Group";
			MyExchangeServer.Enabled = true;
			MyExchangeServer.DAGPrimaryIPAddress = DR["PrimaryIPAddress"].ToString();
			MyExchangeServer.DAGPrimaryPassword = decodePasswordFromEncodedString(DR["PrimaryPassword"].ToString(), MyExchangeServer.Name); ;
			MyExchangeServer.DAGPrimaryUserName = DR["PrimaryUserID"].ToString();
			MyExchangeServer.DAGPrimaryAuthenticationType = DR["PrimaryAuthenticationType"] != null && DR["PrimaryAuthenticationType"] != "" ? DR["PrimaryAuthenticationType"].ToString() : "Default";

			MyExchangeServer.DAGBackupIPAddress = DR["BackupIPAddress"].ToString();
			MyExchangeServer.DAGBackupPassword = decodePasswordFromEncodedString(DR["BackupPassword"].ToString(), MyExchangeServer.Name);;
			MyExchangeServer.DAGBackupUserName = DR["BackupUserID"].ToString();
			MyExchangeServer.DAGBackupAuthenticationType = DR["BackupAuthenticationType"] != null && DR["BackupAuthenticationType"] != "" ? DR["BackupAuthenticationType"].ToString() : "Default";
			MyExchangeServer.InsufficentLicenses = DR["CurrentNodeID"] != null && DR["CurrentNodeID"].ToString() == "-1" ? true : false;

			return MyExchangeServer;
		}

		int dagThreadCount = 0;
		int initialDagThreadCount = 0;

		private void StartDAGThreads()
		{
			int maxThreadCount = Common.getThreadCount("DAG");
			int startThreads = 0;
			dagThreadCount = myDagCollection.Count / 3;
			if (dagThreadCount <= 1)
				dagThreadCount = 2;

			// 5/19/15 WS commented out.  VSPLUS 1776
			if (dagThreadCount > maxThreadCount)
				dagThreadCount = maxThreadCount;
			startThreads = initialDagThreadCount;

			initialDagThreadCount = dagThreadCount;
			if (c == null)
				c = new CultureInfo("en-US");
			for (int i = startThreads; i < dagThreadCount; i++)
			{

				Thread MainDAGThread = new Thread(new ThreadStart(MonitorDAG));
				MainDAGThread.CurrentCulture = c == null ? new CultureInfo("en-US") : c;  //Should only be null on our local copies if using wrapper
				MainDAGThread.IsBackground = true;
				MainDAGThread.Name = "Main DAG Thread";
				MainDAGThread.Priority = ThreadPriority.Normal;
				MainDAGThread.Start();
				Thread.Sleep(2000);
			}



		}

		private void MonitorDAG()
		{
			CommonDB DB = new CommonDB();
			while (true)
			{
				try
				{

					DAGMutex.WaitOne();
					MonitoredItems.ExchangeServer thisServer;
					try
					{
						thisServer = Common.SelectServerToMonitor(myDagCollection) as MonitoredItems.ExchangeServer;
				
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
						DAGMutex.ReleaseMutex();
					}

                    Common.WriteDeviceHistoryEntry("All", thisServer.ServerType, "Scanning Server " + (thisServer == null ? "null" : thisServer.Name) + " on thread ");

                    if (thisServer != null)
					{
						Common.WriteDeviceHistoryEntry("All", thisServer.ServerType, "Scanning Server " + thisServer.Name + " on thread ");
						thisServer.IsBeingScanned = true;

						TestResults AllTestResults = new TestResults();
						ExchangeDAG dag = new ExchangeDAG();
						dag.CheckServer(thisServer, ref AllTestResults);

						thisServer.Status = "";
						thisServer.LastScan = DateTime.Now;
						DB.UpdateAllTests(AllTestResults, thisServer, thisServer.ServerType);
						thisServer.IsBeingScanned = false;
						
					}
					else
					{
						try
						{
							Common.WriteDeviceHistoryEntry("ALL", thisServer.ServerType, "Server " + thisServer.Name + " is already being scanned and will not start scanning again");
						}
						catch (Exception ex)
						{
							Common.WriteDeviceHistoryEntry("ALL", thisServer.ServerType, "Server returned as null");
						}
					}
				}
				catch
				{

				}


				Common.WriteDeviceHistoryEntry("All", "Exchange", "Waiting for 5 seconds to restart the Loop ");
				// Sleep for 1 minutes 
				Thread.Sleep(1000 * 5);
				//break;
			}

		}

		public MonitoredItems.ExchangeServer SelectDAGServerToMonitor()
		{

			DateTime tNow = DateTime.Now;
			DateTime tScheduled;

			DateTime timeOne;
			DateTime timeTwo;

			MonitoredItems.ExchangeServer SelectedServer = null;

			MonitoredItems.ExchangeServer ServerOne = null;
			MonitoredItems.ExchangeServer ServerTwo = null;

			RegistryHandler myRegistry = new RegistryHandler();

			String ScanASAP = "";
			String strSQL = "";
			String ServerType = "DAG";
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

					for (int n = 0; n < myLyncServers.Count; n++)
					{
						ServerOne = myLyncServers.get_Item(n);
						if (ServerOne.Name == serverName && ServerOne.IsBeingScanned == false && ServerOne.Enabled)
						{
							Common.WriteDeviceHistoryEntry("All", "Database Availability Group", serverName + " was marked 'Scan ASAP' so it will be scanned next.");

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
				ScanASAP = myRegistry.ReadFromRegistry("ScanExchangeASAP").ToString();
			}
			catch (Exception ex)
			{
				ScanASAP = "";
			}

			//Searches for the server marked as ScanASAP, if it exists
			for (int n = 0; n < myDagCollection.Count; n++)
			{
				ServerOne = myDagCollection.get_Item(n);
				if (ServerOne.Name == ScanASAP && ServerOne.IsBeingScanned == false && ServerOne.Enabled)
				{
					Common.WriteDeviceHistoryEntry("All", "Exchange", ScanASAP + " was marked 'Scan ASAP' so it will be scanned next.");
					myRegistry.WriteToRegistry("ScanExchangeASAP", "n/a");

					//ServerOne.ScanASAP = true;

					return ServerOne;
				}

			}


			//Searches for the first enounter of a Not Responding server that is due for a scan
			for (int n = 0; n < myDagCollection.Count; n++)
			{
				ServerOne = myDagCollection.get_Item(n);
				if (ServerOne.Status == "Not Responding" && ServerOne.IsBeingScanned == false && ServerOne.Enabled)
				{
					tScheduled = ServerOne.NextScan;
					if (DateTime.Compare(tNow, tScheduled) > 0)
					{
						Common.WriteDeviceHistoryEntry("All", "Exchange", "Selecting " + ServerOne.Name + " because the status is " + ServerOne.Status + ".  Next scheduled scan is at " + tScheduled.ToString());
						return ServerOne;
					}
				}
			}


			//Searches for the first encounter of a server that has not been scanned yet
			for (int n = 0; n < myDagCollection.Count; n++)
			{
				ServerOne = myDagCollection.get_Item(n);
                if (ServerOne.Status == "Not Scanned" || ServerOne.Status == "Master Service Stopped." && ServerOne.IsBeingScanned == false && ServerOne.Enabled)
				{
					Common.WriteDeviceHistoryEntry("All", "Exchange", "Selecting " + ServerOne.Name + " because the status is " + ServerOne.Status + ".");
					return ServerOne;
				}
			}


			//Searches for all servers that are due for a scan
			List<MonitoredItems.ExchangeServer> ScanCanidates = new List<MonitoredItems.ExchangeServer>();

			foreach (MonitoredItems.ExchangeServer srv in myDagCollection)
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
					Common.WriteDeviceHistoryEntry("All", "Exchange", "Error Selecting Exchange Server... " + ex.Message);
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

		public void DeleteOldDagDataOnStart()
		{
			//TestResults tests = new TestResults();
			//if (listOfIdsForDag == null || listOfIdsForDag.Trim() == "")
			//    return;
			//string[] sql = { 
			//                   "delete from dbo.DAGDatabaseDetails where DAGID in (select distinct ds.ID From DagStatus ds inner join Servers svr on ds.DAGName = svr.ServerName where svr.ID in (" + listOfIdsForDag + "))",
			//                   "delete from dbo.DagDatabase where DagMemberId in (Select ID from DagMembers where DagId in (select distinct ds.ID From DagStatus ds inner join Servers svr on ds.DAGName = svr.ServerName where svr.ID in (" + listOfIdsForDag + ")))",
			//                   "delete from dbo.DagMembers where ID in (Select ID from DagMembers where DagId in (select distinct ds.ID From DagStatus ds inner join Servers svr on ds.DAGName = svr.ServerName where svr.ID in (" + listOfIdsForDag + ")))", 
			//                   "delete from dbo.DagStatus where DagName in (select ServerName from Servers svr where svr.ID in (" + listOfIdsForDag + "))"
			//               };
			//CommonDB db = new CommonDB();
			//foreach (string s in sql)
			//{
			//    try
			//    {
			//        db.Execute(s);
			//    }
			//    catch (Exception ex)
			//    {
			//        Common.WriteDeviceHistoryEntry("All", "Database Availability Group", "Error removing DAG Info. Sql:" + s.ToString() + "...Error:" + ex.Message, Common.LogLevel.Normal);
			//    }
			//}
		}

		#endregion


		public void RefreshExchangeCollection()
 {
			if (c != null)
			{
				CreateExchangeServersCollection();
				StartExchangeThreads(false);
			}
		}

		public void RefreshLyncCollection()
		{
			if (c != null)
			{
				CreateLyncServersCollection();
				StartLyncThreads();
			}
		}

		public void RefreshDAGCollection()
		{
			if (c != null)
			{
				CreateExchangeDAGCollection();
				StartDAGThreads();
			}
		}

		public void RefreshExchangeMailFlowCollection()
		{
			if (c != null)
			{
				//CreateExchangeMailProbeCollection();
				//StartMailProbeThreads();
			}
		}
	}
}
