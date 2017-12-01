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
	public class SharepointMAIN
	{
		Mutex SharePointMutex = new Mutex();
		string CurrentCulture = "en-US";
		string CultureStringName = "CultureString";
		CultureInfo c;
		string listOfIds = "";
		MonitoredItems.SharepointServersCollection mySharepointServers;

		string serverType = "SharePoint";
        string SharePointFarmServerType = "SharePoint Farm";
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
				Common.WriteDeviceHistoryEntry("All", serverType, "SP Service is starting up", Common.LogLevel.Normal);
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

				//if (db.RecordExists("SELECT * FROM SelectedFeatures sf WHERE sf.FeatureID in (SELECT ID FROM Features WHERE Name='" + serverType + "')"))
                if(true)
				{
					Common.WriteDeviceHistoryEntry("All", serverType, "Server type is marked for scanning so will start Server Related Tasks", Common.LogLevel.Normal);
					CreateSharepointServersCollection();
					Common.InitStatusTable(mySharepointServers);
					StartSPThreads();

					Thread.Sleep(60 * 1000 * 1);

					Thread HourlyTasksThread = new Thread(new ThreadStart(HourlyTasks));
					HourlyTasksThread.CurrentCulture = c;
					HourlyTasksThread.IsBackground = true;
					HourlyTasksThread.Name = "HourlyTasks - SP";
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
					DailyTasksThread.Name = "DailyTasks - SP";
					DailyTasksThread.Start();
					Thread.Sleep(2000);

				}
				else
				{
					mySharepointServers = null;
					Common.WriteDeviceHistoryEntry("All", serverType, "Server is not marked for scanning", Common.LogLevel.Normal);
				}


				//sleep for one minute to allow time for the collection to be made 
				

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
		private void StartSPThreads()
		{
			int maxThreadCount = Common.getThreadCount("SharePoint");
			int startThreads = 0;
			serverThreadCount = mySharepointServers.Count / 3;
			if (serverThreadCount <= 1)
				if (mySharepointServers.Count > 1)
					serverThreadCount = 2;
				else
					serverThreadCount = 1;
			if (serverThreadCount <= 1 && mySharepointServers.Count > 1)
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
				Thread monitorAD = new Thread(() => MonitorSP(i));
				monitorAD.CurrentCulture = c == null ? new CultureInfo("en-US") : c;  //Should only be null on our local copies if using wrapper
				monitorAD.IsBackground = true;
				monitorAD.Priority = ThreadPriority.Normal;
				monitorAD.Name = i.ToString() + "-SP";
				AliveServerMainThreads.Add(monitorAD);
				monitorAD.Start();
				Thread.Sleep(2000);
			}


		}
		public MonitoredItems.SharepointServer SelectServerToMonitor()
		{

			DateTime tNow = DateTime.Now;
			DateTime tScheduled;

			DateTime timeOne;
			DateTime timeTwo;

			MonitoredItems.SharepointServer SelectedServer = null;

			MonitoredItems.SharepointServer ServerOne = null;
			MonitoredItems.SharepointServer ServerTwo = null;

			RegistryHandler myRegistry = new RegistryHandler();

			String ScanASAP = "";
			String strSQL = "";
			String ServerType = "SharePoint";
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

					for (int n = 0; n < mySharepointServers.Count; n++)
					{
						ServerOne = mySharepointServers.get_Item(n);
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
				ScanASAP = myRegistry.ReadFromRegistry("ScanSharePointASAP").ToString();
			}
			catch (Exception ex)
			{
				ScanASAP = "";
			}


			//Searches for the server marked as ScanASAP, if it exists
			for (int n = 0; n < mySharepointServers.Count; n++)
			{
				ServerOne = mySharepointServers.get_Item(n);
				if (ServerOne.Name == ScanASAP && ServerOne.IsBeingScanned == false && ServerOne.Enabled)
				{
					Common.WriteDeviceHistoryEntry("All", serverType, ScanASAP + " was marked 'Scan ASAP' so it will be scanned next.");
					myRegistry.WriteToRegistry("ScanADASAP", "n/a");

					//ServerOne.ScanASAP = true;

					return ServerOne;
				}

			}


			//Searches for the first enounter of a Not Responding server that is due for a scan
			for (int n = 0; n < mySharepointServers.Count; n++)
			{
				ServerOne = mySharepointServers.get_Item(n);
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
			for (int n = 0; n < mySharepointServers.Count; n++)
			{
				ServerOne = mySharepointServers.get_Item(n);
				if ((ServerOne.Status == "Not Scanned" || ServerOne.Status == "Master Service Stopped.") && ServerOne.IsBeingScanned == false && ServerOne.Enabled)
				{
					Common.WriteDeviceHistoryEntry("All", serverType, "Selecting " + ServerOne.Name + " because the status is " + ServerOne.Status + ".");
					return ServerOne;
				}
			}


			//Searches for all servers that are due for a scan
			List<MonitoredItems.SharepointServer> ScanCanidates = new List<MonitoredItems.SharepointServer>();

			foreach (MonitoredItems.SharepointServer srv in mySharepointServers)
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
		private void MonitorSP(int threadNum)
		{

			Thread.CurrentThread.CurrentCulture = c;
			CommonDB DB = new CommonDB();
			while (true)
			{

				SharePointMutex.WaitOne();
				MonitoredItems.SharepointServer thisServer;
				try
				{
					thisServer = (MonitoredItems.SharepointServer)Common.SelectServerToMonitor(mySharepointServers);
				
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
					SharePointMutex.ReleaseMutex();
				}

				try
				{
					if (thisServer != null)
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
							Common.WriteDeviceHistoryEntry(thisServer.ServerType, thisServer.Name, "Doing a fast scan sicne it is in maintenance", Common.LogLevel.Normal);
							thisServer.FastScan = true;
						}
                        
						TestResults AllTestResults = new TestResults();
						ReturnPowerShellObjects PSO = null;

                        Common.SetupServer(thisServer, thisServer.ServerType, AllTestResults);

                        bool notResponding = true;
						
						using (PSO = Common.TestRepsonding(thisServer, ref notResponding, ref AllTestResults))
						{
							if (!notResponding)
							{
								//if(thisServer.Name == "sp-app1.jnittech.com")
								//if (thisServer.Role == null || thisServer.Role == "")

                                MicrosoftCommon MSCommon = new MicrosoftCommon();
                                MSCommon.PrereqForWindows(thisServer, AllTestResults, PSO);

							    if (!thisServer.FastScan)
								{
									GetRoles(thisServer, ref AllTestResults);

									SharepointCommon SPCommon = new SharepointCommon();
									SPCommon.checkServer(thisServer, ref AllTestResults, PSO);

								}
								

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
					Common.WriteDeviceHistoryEntry(thisServer.ServerType, thisServer.Name, "Error in MonitorSP: " + ex.Message, commonEnums.ServerRoles.SharePoint, Common.LogLevel.Normal);
				}
			}

		}

		private void CreateSharepointServersCollection()
		{
            if (mySharepointServers == null)
                mySharepointServers = new MonitoredItems.SharepointServersCollection();

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
                FilterDefinition<VSNext.Mongo.Entities.Server> filterDef = repository.Filter.Eq(x => x.DeviceType, VSNext.Mongo.Entities.Enums.ServerType.SharePoint.ToDescription());

                ProjectionDefinition<VSNext.Mongo.Entities.Server> projectionDef = repository.Project
                    .Include(x => x.Id)
                    .Include(x => x.DeviceName)
                    .Include(x => x.LocationId)
                    .Include(x => x.OffHoursScanInterval)
                    .Include(x => x.RetryInterval)
                    .Include(x => x.ScanInterval)
                    .Include(x => x.IPAddress)
                    .Include(x => x.Category)
                    .Include(x => x.CredentialsId)
                    .Include(x => x.ResponseTime)
                    .Include(x => x.CpuThreshold)
                    .Include(x => x.MemoryThreshold)
                    .Include(x => x.CurrentNode)
                    .Include(x => x.SimulationTests);
                //farm??
                //Roles??

                listOfServers = repository.Find(filterDef, projectionDef).ToList();

                VSNext.Mongo.Repository.Repository<VSNext.Mongo.Entities.Status> repositoryStatus = new VSNext.Mongo.Repository.Repository<Status>(DB.GetMongoConnectionString());
                FilterDefinition<VSNext.Mongo.Entities.Status> filterDefStatus = repositoryStatus.Filter.Eq(x => x.DeviceType, VSNext.Mongo.Entities.Enums.ServerType.SharePoint.ToDescription());
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
            catch (Exception ex)
            {
                Common.WriteDeviceHistoryEntry("All", "Exchange", "Exception in CreateExchangeServersCollection when getting the data from the db. Exception: " + ex.Message.ToString(), Common.LogLevel.Normal);
            }




            /*

   //         //Fetch all servers
   //         if (mySharepointServers == null)
			//	mySharepointServers = new MonitoredItems.SharepointServersCollection();
			//MonitoredItems.SharepointServersCollection newCollection = new MonitoredItems.SharepointServersCollection();
			//CommonDB DB = new CommonDB();
			//StringBuilder SQL = new StringBuilder();
			//SQL.Append(" select distinct Sr.ID,Sr.ServerName,S.ServerType, S.ID as ServerTypeId,L.Location,sa.ScanInterval,sa.RetryInterval,sa.OffHourInterval,sa.Enabled,sr.ipaddress,sa.category,cr.UserID,cr.Password,sa.ResponseTime,spf.Farm, ");
			//SQL.Append(" (select RoleName from RolesMaster where id=(select RoleID from ServerRoles where ServerID=sr.ID)) as RoleName, st.LastUpdate, st.Status, st.StatusCode, di.CurrentNodeID, sa.CPU_Threshold, sa.MemThreshold,  ");
   //         SQL.Append("  spss.ConflictingContentType ,spss.CustomizedFiles, spss.MissingGalleries, spss.MissingParentContentTypes, spss.MissingSiteTemplates, spss.UnsupportedLanguagePack, spss.UnsupportedMUI ");
			//SQL.Append("  from Servers Sr ");
			//SQL.Append(" inner join ServerTypes S on Sr.ServerTypeID=S.ID  inner join Locations L on Sr.LocationID =L.ID  left outer join ServerAttributes sa on sr.ID=sa.serverid ");
			//SQL.Append(" inner join credentials cr on sa.CredentialsId=cr.ID ");
			//SQL.Append(" left join SharePointFarms spf on spf.ServerId=sr.ID ");
   //         SQL.Append(" left join SharePointServerSettings spss on spss.ServerId=sr.ID ");
			//if (ConfigurationManager.AppSettings["VSNodeName"] != null)
			//{
			//	string NodeName = ConfigurationManager.AppSettings["VSNodeName"].ToString();
			//	SQL.Append(" inner join DeviceInventory di on Sr.ID=di.DeviceID and Sr.ServerTypeId=di.DeviceTypeId ");
			//	SQL.Append(" inner join Nodes on (Nodes.ID=di.CurrentNodeId or di.CurrentNodeId=-1) and Nodes.Name='" + NodeName + "' ");
			//}
			//SQL.Append(" left outer join Status st on st.Type=S.ServerType and st.Name=Sr.ServerName ");
			//SQL.Append(" where S.ServerType='" + serverType + "' and sa.Enabled = 1 order by sr.id");
			//DataTable dtServers = DB.GetData(SQL.ToString());
			//listOfIds = String.Join(",", dtServers.AsEnumerable().Select(r => r.Field<Int32>("ID").ToString()).ToList());
			//Loop through servers
			//MonitoredItems.ExchangeThresholdSettings ExchgThreshold = new MonitoredItems.ExchangeThresholdSettings();
            */
			if (listOfServers.Count > 0)
			{
				List<string> ServerNameList = new List<string>();

				int updatedServers = 0;
				int newServers = 0;
				int removedServers = 0;

				for (int i = 0; i < listOfServers.Count; i++)
				{
					Server entity = listOfServers[i];
					ServerNameList.Add(entity.DeviceName.ToString());

					MonitoredItems.SharepointServer mySPServer = null;
                    VSNext.Mongo.Entities.Status statusEntry = listOfStatus.Find(x => x.DeviceId == entity.Id);
                    //Checks to see if the server is newly added or exists.  Adds if it is new
                    try
					{
						mySPServer = mySharepointServers.SearchByName(entity.DeviceName.ToString());
						if (mySPServer == null)
						{
							//New server.  Set inits and add to collection

							//mySPServer = InitForSPServers(mySPServer, DR);

                            mySPServer = new MonitoredItems.SharepointServer();

                            mySPServer.LastScan = statusEntry != null && statusEntry.LastUpdated.HasValue ? statusEntry.LastUpdated.Value : DateTime.Now.AddMinutes(-30);
                            mySPServer.Status = statusEntry != null && statusEntry.CurrentStatus == null ? statusEntry.CurrentStatus : "Not Scanned";
                            mySPServer.StatusCode = statusEntry != null && statusEntry.StatusCode == null ? statusEntry.StatusCode : "Maintenance";
                            mySPServer.ServerType = Enums.ServerType.SharePoint.ToDescription();
                            mySPServer.Enabled = true;

                            mySharepointServers.Add(mySPServer);
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
                        mySPServer = new MonitoredItems.SharepointServer();

                        mySPServer.LastScan = DateTime.Now.AddMinutes(-30);
                        mySPServer.Status = "Not Scanned";
                        mySPServer.StatusCode = "Maintenance";
                        mySPServer.ServerType = Enums.ServerType.SharePoint.ToDescription();
                        mySPServer.Enabled = true;

                        mySharepointServers.Add(mySPServer);
                        //mySPServer = InitForSPServers(mySPServer, DR);
						//mySharepointServers.Add(mySPServer);
						newServers++;
					}

                    //mySPServer = SetSPServerSettings(mySPServer, DR);
                    mySPServer.IPAddress = entity.IPAddress;
                    mySPServer.Name = entity.DeviceName;

                    try
                    {
                        var creds = listOfCredentials.Where(x => x.Id == entity.CredentialsId).First();
                        TripleDES tripleDes = new TripleDES();
                        mySPServer.UserName = creds.UserId;
                        mySPServer.Password = tripleDes.Decrypt(creds.Password);
                    }
                    catch (Exception ex)
                    {

                    }

                    try
                    {
                        var location = listOfLocations.Where(x => x.Id == entity.LocationId).First();
                        mySPServer.Location = location.LocationName;
                    }
                    catch (Exception ex)
                    {

                    }

                    try
                    {
                        var status = listOfStatus.Where(x => x.DeviceId == entity.Id).First();
                        mySPServer.Status = status.CurrentStatus;
                        mySPServer.StatusCode = status.StatusCode;
                        mySPServer.LastScan = status.LastUpdated.HasValue ? status.LastUpdated.Value : DateTime.Now.AddHours(-1);
                        mySPServer.ResponseDetails = status.Details;
                    }
                    catch (Exception ex)
                    {

                    }

                    mySPServer.ResponseThreshold = entity.ResponseTime.GetValueOrDefault();
                    mySPServer.ScanInterval = entity.ScanInterval.GetValueOrDefault();
                    mySPServer.OffHoursScanInterval = entity.OffHoursScanInterval.GetValueOrDefault();
                    mySPServer.RetryInterval = entity.RetryInterval.GetValueOrDefault();
                    mySPServer.CPU_Threshold = entity.CpuThreshold.GetValueOrDefault();
                    mySPServer.Memory_Threshold = entity.MemoryThreshold.GetValueOrDefault();

                    mySPServer.Category = entity.Category;
                    mySPServer.ServerObjectID = entity.Id;
                    //mySPServer.Farm = DR["Farm"] == null ? "" : DR["Farm"].ToString();
                    if (entity.SimulationTests != null)
                    {
                        foreach(NameValuePair nameValue in entity.SimulationTests)
                        {
                            switch(nameValue.Name)
                            {
                                case "Conflicting Content Types":
                                    mySPServer.ConflictingContentType = true;
                                    break;
                                case "Customized Files":
                                    mySPServer.CustomizedFiles = true;
                                    break;
                                case "Missing Galleries":
                                    mySPServer.MissingGalleries = true;
                                    break;
                                case "Missing Parent Content Types":
                                    mySPServer.MissingParentContentTypes = true;
                                    break;
                                case "Missing Site Templates":
                                    mySPServer.MissingSiteTemplates = true;
                                    break;
                                case "Unsupported MUI References":
                                    mySPServer.UnsupportedMUI = true;
                                    break;
                                case "Unsupported Language Pack References":
                                    mySPServer.UnsupportedLanguagePack = true;
                                    break;
                            }
                                                             
                        }
                    }

                    //mySPServer.InsufficentLicenses = entity.CurrentNode == null || entity.CurrentNode != NodeName;
                    mySPServer.CurrentNode = entity.CurrentNode;

                }



				//Removes servers not in the new lsit
				foreach (MonitoredItems.SharepointServer server in mySharepointServers)
				{
					string currName = server.Name;
					try
					{
						if (!ServerNameList.Contains(currName))
						{
							//incase it doesnt throw and exception, if not found, removed server from list
							mySharepointServers.Delete(currName);
							removedServers++;

						}
					}
					catch (Exception ex)
					{
						//server not found
						mySharepointServers.Delete(currName);
						removedServers++;

					}

				}


				//myExchangeServers = newCollection;
				/**********************************************************/
				Common.WriteDeviceHistoryEntry("All", serverType, "There are " + mySharepointServers.Count + " servers in the collection.  " + newServers + " were new and " + updatedServers + " were updated.");
			}
			else
			{
				mySharepointServers = new MonitoredItems.SharepointServersCollection();
			}

			//This function does status inserts for insufficent licenses, removes those servers from the collection and raises/resets system message accordingly
			Common.InsertInsufficentLicenses(mySharepointServers);

			//At this point we have all Servers with ALL the information(including Threshold settings)
		}

		private MonitoredItems.SharepointServer SetSPServerSettings(MonitoredItems.SharepointServer MySPServer, DataRow DR)
		{
			MySPServer.IPAddress = DR["IPAddress"].ToString();
			MySPServer.Name = DR["ServerName"].ToString();
			MySPServer.UserName = DR["UserID"].ToString();
			MySPServer.Password = Common.decodePasswordFromEncodedString(DR["Password"].ToString(), MySPServer.Name);
			MySPServer.Location = DR["Location"].ToString();
			//MySPServer.Role = DR["RoleName"] == null ? "" : DR["RoleName"].ToString();
			MySPServer.ResponseThreshold = long.Parse(DR["ResponseTime"].ToString());
			MySPServer.ScanInterval = int.Parse(DR["ScanInterval"].ToString());
			MySPServer.OffHoursScanInterval = int.Parse(DR["OffHourInterval"].ToString());
			MySPServer.RetryInterval = int.Parse(DR["RetryInterval"].ToString());
			MySPServer.CPU_Threshold = int.Parse(DR["CPU_Threshold"].ToString());
			MySPServer.Memory_Threshold = int.Parse(DR["MemThreshold"].ToString());
			
			MySPServer.Category = DR["Category"].ToString();
			MySPServer.ServerId = DR["ID"].ToString();
			MySPServer.Farm = DR["Farm"] == null ? "" : DR["Farm"].ToString();
            MySPServer.ConflictingContentType = DR["ConflictingContentType"] == null || DR["ConflictingContentType"].ToString() == "" ? true : Convert.ToBoolean(DR["ConflictingContentType"].ToString());
            MySPServer.CustomizedFiles = DR["CustomizedFiles"] == null || DR["CustomizedFiles"].ToString() == "" ? true : Convert.ToBoolean(DR["CustomizedFiles"].ToString());
            MySPServer.MissingGalleries = DR["MissingGalleries"] == null || DR["MissingGalleries"].ToString() == "" ? true : Convert.ToBoolean(DR["MissingGalleries"].ToString());
            MySPServer.MissingParentContentTypes = DR["MissingParentContentTypes"] == null || DR["MissingParentContentTypes"].ToString() == "" ? true : Convert.ToBoolean(DR["MissingParentContentTypes"].ToString());
            MySPServer.MissingSiteTemplates = DR["MissingSiteTemplates"] == null || DR["MissingSiteTemplates"].ToString() == "" ? true : Convert.ToBoolean(DR["MissingSiteTemplates"].ToString());
            MySPServer.UnsupportedLanguagePack = DR["UnsupportedLanguagePack"] == null || DR["UnsupportedLanguagePack"].ToString() == "" ? true : Convert.ToBoolean(DR["UnsupportedLanguagePack"].ToString());
            MySPServer.UnsupportedMUI = DR["UnsupportedMUI"] == null || DR["UnsupportedMUI"].ToString() == "" ? true : Convert.ToBoolean(DR["UnsupportedMUI"].ToString());

			MySPServer.InsufficentLicenses = DR["CurrentNodeID"] != null && DR["CurrentNodeID"].ToString() == "-1" ? true : false;

			return MySPServer;
		}

		private MonitoredItems.SharepointServer InitForSPServers(MonitoredItems.SharepointServer MySPServer, DataRow DR)
		{
			MySPServer = new MonitoredItems.SharepointServer();

			MySPServer.LastScan = DR["LastUpdate"] == null || DR["LastUpdate"].ToString() == "" ? DateTime.Now : DateTime.Parse(DR["LastUpdate"].ToString());
			MySPServer.Status = DR["Status"] == null || DR["Status"].ToString() == "" ? "Not Scanned" : DR["Status"].ToString();
			MySPServer.StatusCode = DR["StatusCode"] == null || DR["StatusCode"].ToString() == "" ? "Maintenance" : DR["StatusCode"].ToString();
			MySPServer.ServerType = DR["ServerType"].ToString();
			MySPServer.ServerTypeId = int.Parse(DR["ServerTypeId"].ToString());
			MySPServer.Enabled = true;

			return MySPServer;
		}

		public void RefreshSharePointCollection()
		{
			if (c != null)
			{
				CreateSharepointServersCollection();
				StartSPThreads();
			}
		}

		public void GetRoles(MonitoredItems.SharepointServer myServer, ref TestResults AllTestsList)
		{
			try
			{
				ReturnPowerShellObjects PSO = Common.PrereqForSharepointWithCmdlets(myServer.Name, myServer.UserName, myServer.Password, myServer.ServerType, myServer.IPAddress, commonEnums.ServerRoles.Empty, "");
				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "No roles are assigned so will find roles for the server", commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);
				if (PSO.Connected == false)
				{
					PSO.Dispose();
					PSO = Common.PrereqForSharepointDBWithCmdlets(myServer.Name, myServer.UserName, myServer.Password, myServer.ServerType, myServer.IPAddress, commonEnums.ServerRoles.Empty, "");
					if (PSO != null)
					{
						PSO.Dispose();
						myServer.Role = "Database";
						return;
					}
				}

				using (PSO)
				{
					if (myServer.Role != "Database")
					{
						System.Collections.ObjectModel.Collection<PSObject> results = new System.Collections.ObjectModel.Collection<PSObject>();
						System.IO.StreamReader sr = new System.IO.StreamReader(AppDomain.CurrentDomain.BaseDirectory.ToString() + "Scripts\\SP_GetRoles.ps1");
						String str = "Invoke-Command -Session $ra -ScriptBlock {Add-PSSnapin Microsoft.SharePoint.Powershell; " + sr.ReadToEnd() + "}";
						PSO.PS.Commands.Clear();
						PSO.PS.Streams.ClearStreams();
						PSO.PS.AddScript(str);
						results = PSO.PS.Invoke();

						Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "SP_GetRoles Results: " + results.Count, Common.LogLevel.Normal);

						foreach (PSObject ps in results)
						{
							string Name = ps.Properties["Name"].Value == null ? "" : ps.Properties["Name"].Value.ToString();
							string Role = ps.Properties["Role"].Value == null ? "" : ps.Properties["Role"].Value.ToString();
							string Farm = ps.Properties["Farm"].Value == null ? "" : ps.Properties["Farm"].Value.ToString();

							MonitoredItems.SharepointServer currServer = mySharepointServers.SearchByName(Name);
							if (currServer != null)
							{
								currServer.Role = Role;

                                MongoStatementsUpdate<VSNext.Mongo.Entities.Server> mongoUpdate = new MongoStatementsUpdate<VSNext.Mongo.Entities.Server>();
                                mongoUpdate.filterDef = mongoUpdate.repo.Filter.Where(i => i.DeviceName == currServer.Name && i.DeviceType == currServer.ServerType);
                                mongoUpdate.updateDef = mongoUpdate.repo.Updater.Set(i => i.ServerRoles, new List<String>() { Role });
                                AllTestsList.MongoEntity.Add(mongoUpdate);

								if (Role != "Database")
								{
									currServer.Farm = Farm;
								}
								else
								{
									if(!currServer.Farm.Contains(Farm + ','))
										currServer.Farm += Farm + ',';
								}

                                MongoStatementsUpsert<VSNext.Mongo.Entities.Server> mongoUpsertServer = new MongoStatementsUpsert<VSNext.Mongo.Entities.Server>();
                                mongoUpsertServer.filterDef = mongoUpsertServer.repo.Filter.Where(i => i.DeviceName == Farm && i.DeviceType == SharePointFarmServerType);
                                mongoUpsertServer.updateDef = mongoUpsertServer.repo.Updater.AddToSet(i => i.FarmServers, currServer.Name);
                                AllTestsList.MongoEntity.Add(mongoUpsertServer);

                                MongoStatementsUpsert<VSNext.Mongo.Entities.Status> mongoUpsertStatus = new MongoStatementsUpsert<VSNext.Mongo.Entities.Status>();
                                mongoUpsertStatus.filterDef = mongoUpsertStatus.repo.Filter.Where(i => i.TypeAndName == Farm + "-" + SharePointFarmServerType);
                                mongoUpsertStatus.updateDef = mongoUpsertStatus.repo.Updater
                                    .Set(i => i.DeviceType, SharePointFarmServerType)
                                    .Set(i => i.DeviceName, Farm);
                                AllTestsList.MongoEntity.Add(mongoUpsertStatus);

							}
						}

					}
					PSO.PS.Commands.Clear();

				}

				GC.Collect();
				Thread.Sleep(2000);
			}
			catch (Exception ex)
			{
				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "Error in GetRoles: " + ex.Message, commonEnums.ServerRoles.SharePoint, Common.LogLevel.Normal);
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
			MonitoredItems.SharepointServer DummyServerForLogsHourly = new MonitoredItems.SharepointServer() { Name = "HourlyTask", ServerType = serverType };
			Thread HourlyTasksThread = null;
			int Hour = -1;

			MonitoredItems.SharepointServer DummyServerForLogsDaily = new MonitoredItems.SharepointServer() { Name = "HourlyTask", ServerType = serverType };
			Thread DailyTasksThread = null;
			int Day = -1;

			while (true)
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
					HourlyTasksThread.Name = "HourlyTaskWorkerThread - SP";
					HourlyTasksThread.Start();
					//Thread.Sleep(60 * 60 * 1000); //sleeps for 1 hour
					Hour = DateTime.Now.Hour;
				}

				if (Day == -1 || Hour != DateTime.Now.Day)
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
					DailyTasksThread.Name = "DailyTaskWorkerThread - SP";
					DailyTasksThread.Start();
					
					Day = DateTime.Now.Day;
				}
				//sleep for 5 mins
				Thread.Sleep(1000 * 60 * 5);

			}
		}

		private void HourlyTasksMainThread(MonitoredItems.SharepointServer DummyServerForLogs)
		{


            Common.WriteDeviceHistoryEntry(DummyServerForLogs.ServerType, DummyServerForLogs.Name, "Time to do Hourly Tasks for " + DummyServerForLogs.ServerType + ".", commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);

			try
			{
				try
				{
					TestResults testResults = new TestResults();

					if (mySharepointServers != null)
						foreach (MonitoredItems.SharepointServer server in mySharepointServers)
						{
							server.OffHours = Common.OffHours(server.Name);
							Common.RecordUpAndDownTimes(server, ref testResults);
						}

					CommonDB db = new CommonDB();

					db.UpdateSQLStatements(testResults, DummyServerForLogs);

				}
				catch (Exception ex)
				{
                    Common.WriteDeviceHistoryEntry(DummyServerForLogs.ServerType, DummyServerForLogs.Name, "Error setting OffHours.  Error: " + ex.Message, commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);
				}

				List<String> Farms = new List<String>();
				List<MonitoredItems.SharepointServer> serverCollection = new List<MonitoredItems.SharepointServer>();
				foreach (MonitoredItems.SharepointServer tempServer in mySharepointServers)
				{
					if (!Farms.Contains(tempServer.Farm) && tempServer.Role != "" && !(new string[]{"Database", "Invalid"}.Contains(tempServer.Role)))
					{
						Farms.Add(tempServer.Farm);
						serverCollection.Add(tempServer);
					}
				}

				//individual cmds
				foreach (MonitoredItems.SharepointServer myServer in mySharepointServers)
				{

					//MonitoredItems.SharepointServer myServer = serverCollection[0];
					//Common.WriteDeviceHistoryEntry("All", DummyServerForLogs.ServerType, "Server " + myServer.Name + " will be used on farm " + myServer.Farm + ".", commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);
					string cmdlets = "";


					TestResults AllTestsList = new TestResults();
					using (ReturnPowerShellObjects powershellobj = Common.PrereqForSharepointWithCmdlets(myServer.Name, myServer.UserName, myServer.Password, myServer.ServerType, myServer.IPAddress, commonEnums.ServerRoles.Empty, cmdlets))
					{
                        AllTestsList = new TestResults();

						//NetworkLatencyTest(myServer, AllTestsList, powershellobj, DummyServerForLogs);
						//ContentDatabases(myServer, AllTestsList, powershellobj, DummyServerForLogs);
						//TestSiteCollCreationAndFileUplaod(myServer, AllTestsList, powershellobj, DummyServerForLogs);
						//TestSiteCollectionSize(myServer, AllTestsList, powershellobj, DummyServerForLogs);

                        if ((new string[] { "WebFrontEnd", "SingleServer", "Application" }).Contains(myServer.Role))
                        {
                            GetUserAndSiteActivity(myServer, AllTestsList, powershellobj, DummyServerForLogs);
                        }
						CommonDB db = new CommonDB();
						db.UpdateSQLStatements(AllTestsList, DummyServerForLogs);

						powershellobj.PS.Commands.Clear();

					}
					GC.Collect();
					Thread.Sleep(3 * 1000);
				}


				//farm cmds
				foreach (MonitoredItems.SharepointServer myServer in serverCollection)
				{

				//MonitoredItems.SharepointServer myServer = serverCollection[0];
                    Common.WriteDeviceHistoryEntry(DummyServerForLogs.ServerType, DummyServerForLogs.Name, "Server " + myServer.Name + " will be used on farm " + myServer.Farm + ".", commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);
					string cmdlets = "-CommandName xxx";

                    
					TestResults AllTestsList = new TestResults();
					using (ReturnPowerShellObjects powershellobj = Common.PrereqForSharepointWithCmdlets(myServer.Name, myServer.UserName, myServer.Password, myServer.ServerType, myServer.IPAddress, commonEnums.ServerRoles.Empty, cmdlets))
					{

						NetworkLatencyTest(myServer, AllTestsList, powershellobj, DummyServerForLogs);
						ContentDatabases(myServer, AllTestsList, powershellobj, DummyServerForLogs);
						TestSiteCollCreationAndFileUplaod(myServer, AllTestsList, powershellobj, DummyServerForLogs);
						TestSiteCollectionSize(myServer, AllTestsList, powershellobj, DummyServerForLogs);
                        CheckTimerJobs(myServer, AllTestsList, powershellobj, DummyServerForLogs);
						//GetUserAndSiteActivity(myServer, AllTestsList, powershellobj, DummyServerForLogs);

						CommonDB db = new CommonDB();
						db.UpdateSQLStatements(AllTestsList, DummyServerForLogs);

						powershellobj.PS.Commands.Clear();
						
					}
					GC.Collect();
					Thread.Sleep(3 * 1000);
				}


			}
			catch (Exception ex) { }



		}

		private void DailyTasksMainThread(MonitoredItems.SharepointServer DummyServerForLogs)
		{


			Common.WriteDeviceHistoryEntry("All", DummyServerForLogs.ServerType, "Time to do Daily Tasks for " + DummyServerForLogs.ServerType + ".", commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);
			return;
			//Nothing past here is being done that is meaningful
			try
			{
				List<String> Farms = new List<String>();
				List<MonitoredItems.SharepointServer> serverCollection = new List<MonitoredItems.SharepointServer>();
				foreach(MonitoredItems.SharepointServer tempServer in mySharepointServers)
				{
					if (!Farms.Contains(tempServer.Farm) && !tempServer.Role.Contains("Database") && tempServer.Role != "")
					{
						Farms.Add(tempServer.Farm);
						serverCollection.Add(tempServer);
					}
				}

				//MonitoredItems.SharepointServer myServer = mySharepointServers.get_Item(0);
				//foreach (MonitoredItems.SharepointServer currServer in mySharepointServers)
					//if (!currServer.IsBeingScanned && !currServer.Name.Contains("db"))

						//myServer = currServer;
				foreach (MonitoredItems.SharepointServer myServer in serverCollection)
				{
					Common.WriteDeviceHistoryEntry("All", DummyServerForLogs.ServerType, "Server " + myServer.Name + " will be used on farm " + myServer.Farm + ".", commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);
					string cmdlets = "-CommandName xxx";
					

					TestResults AllTestsList = new TestResults();
					using (ReturnPowerShellObjects powershellobj = Common.PrereqForSharepointWithCmdlets(myServer.Name, myServer.UserName, myServer.Password, myServer.ServerType, myServer.IPAddress, commonEnums.ServerRoles.Empty, cmdlets, Modules: new string[] {"WebAdministration"}))
					{

						//ContentDatabases(myServer, AllTestsList, powershellobj, DummyServerForLogs);
						//TestSiteCollCreationAndFileUplaod(myServer, AllTestsList, powershellobj, DummyServerForLogs);

						CommonDB db = new CommonDB();
						db.UpdateSQLStatements(AllTestsList, DummyServerForLogs);

					
					}
					GC.Collect();
					Thread.Sleep(3 * 1000);
				}


			}
			catch(Exception ex){}
		}

		private void ContentDatabases(MonitoredItems.SharepointServer myServer, TestResults AllTestsList, ReturnPowerShellObjects powershellobj, MonitoredItems.SharepointServer DummyServerForLogs)
		{
			try
			{
				string sql = "INSERT INTO SharePointDatabaseDetails ([WebAppName],[DatabaseName],[DatabaseID],[DatabaseSiteCount],[MaxSiteCountThreshold],[WarningSiteCountThreshold]" +
								",[IsDBReadOnly],[DatabaseServer]) VALUES ";

				System.Collections.ObjectModel.Collection<PSObject> results = new System.Collections.ObjectModel.Collection<PSObject>();

				System.IO.StreamReader sr = new System.IO.StreamReader(AppDomain.CurrentDomain.BaseDirectory.ToString() + "Scripts\\SP_EnumContentDatabases.ps1");
				String str = "Invoke-Command -Session $ra -ScriptBlock {Add-PSSnapin Microsoft.SharePoint.Powershell; " + sr.ReadToEnd() + "\n}";
				
				powershellobj.PS.Commands.Clear();
				powershellobj.PS.Streams.ClearStreams();
				powershellobj.PS.AddScript(str);
				
				results = powershellobj.PS.Invoke();

				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "SP_EnumContentDatabases Results: " + results.Count, Common.LogLevel.Normal);
				
                List<VSNext.Mongo.Entities.SharePointWebApplication> listOfWebApps = new List<VSNext.Mongo.Entities.SharePointWebApplication>();

				foreach (PSObject ps in results)
				{
					string WebApplicationName = ps.Properties["WebApplicationName"].Value == null ? "" : ps.Properties["WebApplicationName"].Value.ToString();
					string ContentDatabaseName = ps.Properties["ContentDatabaseName"].Value == null ? "" : ps.Properties["ContentDatabaseName"].Value.ToString();
					string ContentDatabaseID = ps.Properties["ContentDatabaseID"].Value == null ? "" : ps.Properties["ContentDatabaseID"].Value.ToString();
					string DatabaseSiteCount = ps.Properties["DatabaseSiteCount"].Value == null ? "0" : ps.Properties["DatabaseSiteCount"].Value.ToString();
					string MaxSiteCountThreshold = ps.Properties["MaxSiteCountThreshold"].Value == null ? "0" : ps.Properties["MaxSiteCountThreshold"].Value.ToString();
					string WarningSiteCountThreshold = ps.Properties["WarningSiteCountThreshold"].Value == null ? "0" : ps.Properties["WarningSiteCountThreshold"].Value.ToString();
					string IsContentDBReadOnly = ps.Properties["IsContentDBReadOnly"].Value == null ? "False" : ps.Properties["IsContentDBReadOnly"].Value.ToString();
					string WhoIsDBServer = ps.Properties["WhoIsDBServer"].Value == null ? "" : ps.Properties["WhoIsDBServer"].Value.ToString();

					MonitoredItems.SharepointServer currServer = mySharepointServers.SearchByName(WhoIsDBServer);
					if (currServer == null)
						currServer = myServer;
					
					sql += "('" + WebApplicationName + "','" + ContentDatabaseName + "','" + ContentDatabaseID + "','" + DatabaseSiteCount + "','" + MaxSiteCountThreshold + "','" + 
									WarningSiteCountThreshold  + "','" + IsContentDBReadOnly + "','" + WhoIsDBServer + "'),";

                    listOfWebApps.Add(new VSNext.Mongo.Entities.SharePointWebApplication()
                    {
                        ContentDatabaseId = ContentDatabaseID,
                        ContentDBReadOnly = Boolean.Parse(IsContentDBReadOnly),
                        DatabaseServer = WhoIsDBServer,
                        MaxSiteCountThreshold = Convert.ToInt32(MaxSiteCountThreshold),
                        WarningSiteCountThreshold = Convert.ToInt32(WarningSiteCountThreshold),
                        WebApplicationName = WebApplicationName,
                        ContentDatabaseName = ContentDatabaseName,
                        DatabaseSiteCount = Convert.ToInt32(DatabaseSiteCount)
                    });

					int dbSize = Convert.ToInt32(DatabaseSiteCount);
					int warningSize = Convert.ToInt32(WarningSiteCountThreshold);
					int maxSize = Convert.ToInt32(MaxSiteCountThreshold);

					if (dbSize > warningSize)
						//throw warning error
						Common.makeAlert(false, currServer, commonEnums.AlertType.Database_Site_Warning_Count, ref AllTestsList, "The database " + ContentDatabaseName + " has + " + DatabaseSiteCount + " sites", "SharePoint");
					else
						//reset warning error
						Common.makeAlert(true, currServer, commonEnums.AlertType.Database_Site_Warning_Count, ref AllTestsList, "The database " + ContentDatabaseName + " has + " + DatabaseSiteCount + " sites", "SharePoint");

					if (dbSize >= maxSize)
						//throw max error
						Common.makeAlert(false, currServer, commonEnums.AlertType.Database_Site_Max_Count, ref AllTestsList, "The database " + ContentDatabaseName + " has + " + DatabaseSiteCount + " sites", "SharePoint");
					else
						//reset max error
						Common.makeAlert(false, currServer, commonEnums.AlertType.Database_Site_Max_Count, ref AllTestsList, "The database " + ContentDatabaseName + " has + " + DatabaseSiteCount + " sites", "SharePoint");

				}

                MongoStatementsUpdate<VSNext.Mongo.Entities.Status> mongoUpdate = new MongoStatementsUpdate<VSNext.Mongo.Entities.Status>();
                mongoUpdate.filterDef = mongoUpdate.repo.Filter.Where(i => i.TypeAndName == myServer.Farm + "-" + SharePointFarmServerType);
                mongoUpdate.updateDef = mongoUpdate.repo.Updater.Set(i => i.SharePointWebApplications, listOfWebApps);
                AllTestsList.MongoEntity.Add(mongoUpdate);
                

				Common.WriteDeviceHistoryEntry("All", DummyServerForLogs.ServerType, "Finished Hourly Tasks.", commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);
				Common.WriteDeviceHistoryEntry(DummyServerForLogs.ServerType, DummyServerForLogs.Name, "Finished Hourly Tasks.", commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);
			}
			catch (Exception ex)
			{
				Common.WriteDeviceHistoryEntry(DummyServerForLogs.ServerType, DummyServerForLogs.Name, "Error in ContentDatabases.  Error: " + ex.Message, commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);
			}



		}

		private void NetworkLatencyTest(MonitoredItems.SharepointServer myServer, TestResults AllTestsList, ReturnPowerShellObjects powershellobj, MonitoredItems.SharepointServer DummyServerForLogs)
		{
			try
			{
				//string sql = "INSERT INTO SharePointDatabaseDetails ([WebAppName],[DatabaseName],[DatabaseID],[DatabaseSiteCount],[MaxSiteCountThreshold],[WarningSiteCountThreshold]" +
				//				",[IsDBReadOnly],[DatabaseServer]) VALUES ";
				string serversList = "";
				foreach(MonitoredItems.SharepointServer server in mySharepointServers)
					serversList += "\"" + server.Name + "\",";

				//string dbList = "";
				//foreach(MonitoredItems.SharepointServer server in mySharepointServers)
				//    if(server.Role.Contains("Database"))
				//        dbList += "\"" + server.Name + "\",";


				double runTime = .1;
				double percentThreshold = .1;

				System.Collections.ObjectModel.Collection<PSObject> results = new System.Collections.ObjectModel.Collection<PSObject>();

				System.IO.StreamReader sr = new System.IO.StreamReader(AppDomain.CurrentDomain.BaseDirectory.ToString() + "Scripts\\SP_EnumContentDatabases.ps1");
				String str = "Invoke-Command -Session $s -FilePath .\\SP_NetworkLatencyTest.ps1 -ArgumentList @(" + serversList.Substring(0,serversList.Length-1) + "), " + percentThreshold + "," + runTime + " | fl";

				powershellobj.PS.Commands.Clear();
				powershellobj.PS.Streams.ClearStreams();
				powershellobj.PS.AddScript(str);

				results = powershellobj.PS.Invoke();

				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "SP_EnumContentDatabases Results: " + results.Count, Common.LogLevel.Normal);

				foreach (PSObject ps in results)
				{
					string Name = ps.Properties["Name"].Value == null ? "" : ps.Properties["Name"].Value.ToString();
					string MeetRequirements = ps.Properties["MeetRequirements"].Value == null ? "Fail" : ps.Properties["MeetRequirements"].Value.ToString();
					string Over1MsInPercent = ps.Properties["Over1MsInPercent"].Value == null ? "" : ps.Properties["Over1MsInPercent"].Value.ToString();
					string Over2MsInPercent = ps.Properties["Over2MsInPercent"].Value == null ? "" : ps.Properties["Over2MsInPercent"].Value.ToString();
					string Over3MsInPercent = ps.Properties["Over3MsInPercent"].Value == null ? "" : ps.Properties["Over3MsInPercent"].Value.ToString();
					string Over4MsInPercent = ps.Properties["Over4MsInPercent"].Value == null ? "" : ps.Properties["Over4MsInPercent"].Value.ToString();
					string Over5MsInPercent = ps.Properties["Over5MsInPercent"].Value == null ? "" : ps.Properties["Over5MsInPercent"].Value.ToString();

					MonitoredItems.SharepointServer currServer = mySharepointServers.SearchByName(Name);
					if (currServer == null)
						currServer = myServer;

					//sql += "('" + WebApplicationName + "','" + ContentDatabaseName + "','" + ContentDatabaseID + "','" + DatabaseSiteCount + "','" + MaxSiteCountThreshold + "','" +
					//				WarningSiteCountThreshold + "','" + IsContentDBReadOnly + "','" + WhoIsDBServer + "'),";

					

					if (MeetRequirements == "Fail")
						//throw warning error
						Common.makeAlert(false, currServer, commonEnums.AlertType.Network_Latency_Test, ref AllTestsList, "The server took longer than 1ms to respond " + percentThreshold + "% of the time after many pings.", "SharePoint");
					else
						//reset warning error
						Common.makeAlert(true, currServer, commonEnums.AlertType.Network_Latency_Test, ref AllTestsList, "The server responded in a timely manner after many pings.", "SharePoint");

					
				}

				Common.WriteDeviceHistoryEntry("All", DummyServerForLogs.ServerType, "Finished Hourly Tasks.", commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);
				Common.WriteDeviceHistoryEntry(DummyServerForLogs.ServerType, DummyServerForLogs.Name, "Finished Hourly Tasks.", commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);
			}
			catch (Exception ex)
			{
				Common.WriteDeviceHistoryEntry(DummyServerForLogs.ServerType, DummyServerForLogs.Name, "Error in NetworkLatencyTest.  Error: " + ex.Message, commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);
			}



		}

		public void TestSiteCollCreationAndFileUplaod(MonitoredItems.SharepointServer myServer, TestResults AllTestsList, ReturnPowerShellObjects powershellobj, MonitoredItems.SharepointServer DummyServerForLogs)
		{
			try
			{
				System.Collections.ObjectModel.Collection<PSObject> results = new System.Collections.ObjectModel.Collection<PSObject>();

				string pathToTestFiles = AppDomain.CurrentDomain.BaseDirectory.ToString() + "Scripts\\TestFiles";
				string owners = myServer.UserName;
				string password = myServer.Password;
				//string webAppName = "http://sharepoint.jnittech.com:100";
				CommonDB db = new CommonDB();
				DataTable dt = db.GetData("select * from SharePointFarmSettings where FarmName = '" + myServer.Farm + "'");
				if (dt.Rows.Count == 0)
				{
					Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "There are no configured options for farm so will not do collection and file upload test", Common.LogLevel.Normal);
					return;
				}

				string webAppName = dt.Rows[0]["TestApplicationURL"].ToString();
				bool LogOnTestSetting = Convert.ToBoolean(dt.Rows[0]["LogOnTest"].ToString());
				bool SiteCreationTestSetting = Convert.ToBoolean(dt.Rows[0]["SiteCreationTest"].ToString());
				bool FileUploadTestSetting = Convert.ToBoolean(dt.Rows[0]["FileUploadTest"].ToString());

				if (webAppName == "" || (!LogOnTestSetting && !SiteCreationTestSetting && !FileUploadTestSetting))
				{
					Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "There are no configured options for farm so will not do collection and file upload test", Common.LogLevel.Normal);				
					return;
				}
				string scriptPath = AppDomain.CurrentDomain.BaseDirectory.ToString() + "Scripts\\SP_TestSiteCollCreationAndFileUpload.ps1";

				String str = "& '" + scriptPath + "' -path '" + pathToTestFiles + "' -owners '" + owners + "' -webapp '" + webAppName + "' -pwd '" + password + "'";
				powershellobj.PS.Commands.Clear();
				powershellobj.PS.Streams.ClearStreams();
				powershellobj.PS.AddScript(str);
				results = powershellobj.PS.Invoke();

				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "SP_TestSiteCollCreationAndFileUpload Results: " + results.Count, Common.LogLevel.Normal);

				if (results.Count == 0)
				{

				}

				foreach (PSObject ps in results)
				{
					string SiteCreation = ps.Properties["SiteCreation"].Value == null ? "False" : ps.Properties["SiteCreation"].Value.ToString();
					string FileUpload = ps.Properties["FileUpload"].Value == null ? "False" : ps.Properties["FileUpload"].Value.ToString();

					string LogOnTest = powershellobj.Connected == false ? "Fail" : "Pass";

                    MongoStatementsUpdate<VSNext.Mongo.Entities.Status> mongoUpdate = new MongoStatementsUpdate<VSNext.Mongo.Entities.Status>();
                    mongoUpdate.filterDef = mongoUpdate.repo.Filter.Where(i => i.TypeAndName == myServer.Farm + "-" + SharePointFarmServerType);
                    if (LogOnTestSetting)
                    {
                        mongoUpdate.updateDef = mongoUpdate.repo.Updater.Set(i => i.LogonTest, LogOnTest);
                    }
                    else
                    {
                        mongoUpdate.updateDef = mongoUpdate.repo.Updater.Unset(i => i.LogonTest);
                    }

                    if (FileUploadTestSetting)
                    {
                        mongoUpdate.updateDef = mongoUpdate.updateDef.Set(i => i.FileUploadTest, FileUpload);
                    }
                    else
                    {
                        mongoUpdate.updateDef = mongoUpdate.repo.Updater.Unset(i => i.FileUploadTest);
                    }

                    if (SiteCreationTestSetting)
                    {
                        mongoUpdate.updateDef = mongoUpdate.updateDef.Set(i => i.SiteCreationTest, SiteCreation);
                    }
                    else
                    {
                        mongoUpdate.updateDef = mongoUpdate.repo.Updater.Unset(i => i.SiteCreationTest);
                    }
                    
                    AllTestsList.MongoEntity.Add(mongoUpdate);

				}
			}
			catch (Exception ex)
			{
				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "Error in TestSiteCollections: " + ex.Message, commonEnums.ServerRoles.SharePoint, Common.LogLevel.Normal);
			}
		}

		public void TestSiteCollectionSize(MonitoredItems.SharepointServer myServer, TestResults AllTestsList, ReturnPowerShellObjects powershellobj, MonitoredItems.SharepointServer DummyServerForLogs)
		{



			try
			{

				System.Collections.ObjectModel.Collection<PSObject> results = new System.Collections.ObjectModel.Collection<PSObject>();

				String path = AppDomain.CurrentDomain.BaseDirectory.ToString() + "Scripts\\SP_SiteCollectionSize.ps1";
				String str = "Invoke-Command -Session $ra -FilePath '" + path + "'";

				powershellobj.PS.Commands.Clear();
				powershellobj.PS.Streams.ClearStreams();
				powershellobj.PS.AddScript(str);

				results = powershellobj.PS.Invoke();

				Common.WriteDeviceHistoryEntry(DummyServerForLogs.ServerType, DummyServerForLogs.Name, "SP_SiteCollectionSize Results: " + results.Count, Common.LogLevel.Normal);

				if(powershellobj.PS.Streams != null && powershellobj.PS.Streams.Error != null && powershellobj.PS.Streams.Error.Count > 0)
					for (int i = 0; i < powershellobj.PS.Streams.Error.Count; i++)
					{
						Common.WriteDeviceHistoryEntry(DummyServerForLogs.ServerType, DummyServerForLogs.Name, "SP_SiteCollectionSize Error: " + powershellobj.PS.Streams.Error[i].ErrorDetails, Common.LogLevel.Normal);
						Common.WriteDeviceHistoryEntry(DummyServerForLogs.ServerType, DummyServerForLogs.Name, "SP_SiteCollectionSize Error: " + powershellobj.PS.Streams.Error[i].Exception, Common.LogLevel.Normal);
					}
                List<VSNext.Mongo.Entities.SharePointSiteCollection> siteColList = new List<VSNext.Mongo.Entities.SharePointSiteCollection>();

				foreach (PSObject ps in results)
				{
					string URL = ps.Properties["URL"].Value == null ? "" : ps.Properties["URL"].Value.ToString();
					string SizeMB = ps.Properties["SizeMB"].Value == null ? "0" : ps.Properties["SizeMB"].Value.ToString();
                    string Owner = ps.Properties["Owner"].Value == null ? "" : ps.Properties["Owner"].Value.ToString();
                    string NumOfSites = ps.Properties["NumOfSites"].Value == null ? "0" : ps.Properties["NumOfSites"].Value.ToString();
                    string WebApplication = ps.Properties["WebApplication"].Value == null ? "" : ps.Properties["WebApplication"].Value.ToString();

                    siteColList.Add(new VSNext.Mongo.Entities.SharePointSiteCollection()
                    {
                        Owner = Owner,
                        URL = URL,
                        SiteCount = int.Parse(NumOfSites),
                        SizeMB = double.Parse(SizeMB),
                        WebApplication = WebApplication
                    });

					


				}

                MongoStatementsUpdate<VSNext.Mongo.Entities.Status> mongoUpdate = new MongoStatementsUpdate<VSNext.Mongo.Entities.Status>();
                mongoUpdate.filterDef = mongoUpdate.repo.Filter.Where(i => i.TypeAndName == myServer.Farm + "-" + SharePointFarmServerType);
                mongoUpdate.updateDef = mongoUpdate.repo.Updater.Set(i => i.SharePointSiteCollections, siteColList);
                AllTestsList.MongoEntity.Add(mongoUpdate);


			}
			catch (Exception ex)
			{
				Common.WriteDeviceHistoryEntry(DummyServerForLogs.ServerType, DummyServerForLogs.Name, "Error in SP_SiteCollectionSize.  Error: " + ex.Message, commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);
			}



		}

		public void GetUserAndSiteActivity(MonitoredItems.SharepointServer myServer, TestResults AllTestsList, ReturnPowerShellObjects powershellobj, MonitoredItems.SharepointServer DummyServerForLogs)
		{



			Common.WriteDeviceHistoryEntry(DummyServerForLogs.ServerType, DummyServerForLogs.Name, "In GetUserAndSiteActivity", Common.LogLevel.Normal);



            PowerShell powershell = powershellobj.PS;

            //Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "In GetRequiredServices ", commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);

            String sr = AppDomain.CurrentDomain.BaseDirectory.ToString() + "Scripts\\SP_UserAndSiteActivity.ps1";
            String str = "Invoke-Command -Session $ra -FilePath '" + sr + "'";
            powershell.Streams.Error.Clear();

            powershell.AddScript(str);

            Collection<PSObject> results = powershell.Invoke();

            Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "GetUserAndSiteActivity output results: " + results.Count.ToString(), commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);
            
            Common.WriteDeviceHistoryEntry(DummyServerForLogs.ServerType, DummyServerForLogs.Name, "SP_UserAndSiteActivity Results: " + results.Count, Common.LogLevel.Normal);

			if (powershellobj.PS.Streams != null && powershellobj.PS.Streams.Error != null && powershellobj.PS.Streams.Error.Count > 0)
				for (int i = 0; i < powershellobj.PS.Streams.Error.Count; i++)
				{
                Common.WriteDeviceHistoryEntry(DummyServerForLogs.ServerType, DummyServerForLogs.Name, "SP_UserAndSiteActivity Error: " + powershellobj.PS.Streams.Error[i].ErrorDetails, Common.LogLevel.Normal);
                Common.WriteDeviceHistoryEntry(DummyServerForLogs.ServerType, DummyServerForLogs.Name, "SP_UserAndSiteActivity Error: " + powershellobj.PS.Streams.Error[i].Exception, Common.LogLevel.Normal);
				}


            if (results.Count > 0)
            {
                //Common.WriteDeviceHistoryEntry(DummyServerForLogs.ServerType, DummyServerForLogs.Name, "BaseObj: " + results[0].BaseObject.ToString(), Common.LogLevel.Normal);
                DateTime dateTime = DateTime.Now;

                List<String> listForURLs = new List<String>();
                List<String> listForUsers = new List<String>();
                List<String> listForCounts = new List<String>();

                List<VSNext.Mongo.Entities.SharePointWebTrafficDailyStatistics> listOfWebTraffic = new List<VSNext.Mongo.Entities.SharePointWebTrafficDailyStatistics>();

                try
                {
                    string Url, Count, User, DisplayCount;


                    foreach (PSObject paths in results)
                    {
                        Common.WriteDeviceHistoryEntry(DummyServerForLogs.ServerType, DummyServerForLogs.Name, "path: " + paths.Properties["Name"].Value, Common.LogLevel.Normal);
                        if (paths.Properties["Name"].Value != null)
                        {

                            Url = paths.Properties["Name"].Value.ToString();
                            Count = paths.Properties["Count"].Value == null ? "" : paths.Properties["Count"].Value.ToString();
                            listForURLs.Add("SELECT '" + Url + "' Name");
                            listForCounts.Add("('" + myServer.Name + "',(SELECT ID FROM SharePointSiteRelativeUrl WHERE ServerRelativeUrl = '" + Url + "'), null, '" + Count + "', '" + dateTime + "', '"
                                + Common.GetWeekNumber(dateTime) + "', '" + dateTime.Month + "', '" + dateTime.Year + "', '" + dateTime.Day + "', '" + dateTime.Hour + "')");



                            // sqlForURLs += String.Join(" union ", listForURLs) + ")tbl where tbl.Name not in (select ServerRelativeUrl from SharePointSiteRelativeUrl)";
                        }

                        if (paths.Properties["UName"].Value != null)
                        {
                            //string Url = paths.Properties["Name"].Value == null ? "" : paths.Properties["Name"].Value.ToString();
                            // Url = paths.Properties["Name"].Value == null ? "" : paths.Properties["Name"].Value.ToString();

                            User = paths.Properties["UName"].Value == null ? "" : paths.Properties["UName"].Value.ToString();
                            DisplayCount = paths.Properties["UDisplacount"].Value == null ? "" : paths.Properties["UDisplacount"].Value.ToString();

                            listForUsers.Add("SELECT '" + User + "' Name");
                            listForCounts.Add("('" + myServer.Name + "',null, (SELECT ID FROM SharePointUsers WHERE UserName = '" + User + "'), '" + DisplayCount + "', '" + dateTime + "', '"
                                + Common.GetWeekNumber(dateTime) + "', '" + dateTime.Month + "', '" + dateTime.Year + "', '" + dateTime.Day + "', '" + dateTime.Hour + "')");
                            // sqlForUsers += String.Join(" union ", listForUsers) + ")tbl where tbl.Name not in (select UserName from SharePointUsers)";
                            //  sqlForCounts += String.Join(",", listForCounts);
                        }
                    }
                    /*
                    if (listForURLs.Count > 0)
                    {
                        sqlForURLs += String.Join(" union ", listForURLs) + ")tbl where tbl.Name not in (select ServerRelativeUrl from SharePointSiteRelativeUrl)";
                        AllTestsList.SQLStatements.Add(new SQLstatements() { DatabaseName = "VSS_Statistics", SQL = sqlForURLs });
                        Common.WriteDeviceHistoryEntry(DummyServerForLogs.ServerType, DummyServerForLogs.Name, "SQL: " + sqlForURLs, Common.LogLevel.Normal);

                    }
                    if (listForUsers.Count > 0)
                    {
                        sqlForUsers += String.Join(" union ", listForUsers) + ")tbl where tbl.Name not in (select UserName from SharePointUsers)";
                        AllTestsList.SQLStatements.Add(new SQLstatements() { DatabaseName = "VSS_Statistics", SQL = sqlForUsers });
                        Common.WriteDeviceHistoryEntry(DummyServerForLogs.ServerType, DummyServerForLogs.Name, "SQL: " + sqlForUsers, Common.LogLevel.Normal);
                    }
                    if (listForCounts.Count > 0)
                    {
                        sqlForCounts += String.Join(",", listForCounts);
                        AllTestsList.SQLStatements.Add(new SQLstatements() { DatabaseName = "VSS_Statistics", SQL = sqlForCounts });
                        Common.WriteDeviceHistoryEntry(DummyServerForLogs.ServerType, DummyServerForLogs.Name, "SQL: " + sqlForCounts, Common.LogLevel.Normal);
				}



                */

                }

                catch (Exception ex)
                {
                    Common.WriteDeviceHistoryEntry(DummyServerForLogs.ServerType, DummyServerForLogs.Name, "Error in GetUserAndSiteActivity.  Error: " + ex.Message, commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);
                }
            }

		}

        private void CheckTimerJobs(MonitoredItems.SharepointServer myServer, TestResults AllTestsList, ReturnPowerShellObjects powershellobj, MonitoredItems.SharepointServer DummyServerForLogs)
        {
            //runspace = powershellobj.runspace;
            PowerShell powershell = powershellobj.PS;

            Common.WriteDeviceHistoryEntry(DummyServerForLogs.ServerType, DummyServerForLogs.Name, "In CheckTimerJobs ", commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);
            try
            {
                String sr = AppDomain.CurrentDomain.BaseDirectory.ToString() + "Scripts\\SP_TimedJobs.ps1";
                String str = "Invoke-Command -Session $ra -FilePath '" + sr + "'";
                powershell.Streams.Error.Clear();

                powershell.AddScript(str);

                Collection<PSObject> results = powershell.Invoke();

                foreach (ErrorRecord er in powershell.Streams.Error)
                    Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, er.Exception.ToString(), Common.LogLevel.Normal);

                Common.WriteDeviceHistoryEntry(DummyServerForLogs.ServerType, DummyServerForLogs.Name, "CheckTimerJobs output results: " + results.Count.ToString(), commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);

                List<string> FarmNames = new List<string>();
                List<string> JobDefInserts = new List<string>();

                Dictionary<string, List<VSNext.Mongo.Entities.SharePointTimerJob>> dictOfJobs = new Dictionary<string, List<VSNext.Mongo.Entities.SharePointTimerJob>>();

                foreach (PSObject ps in results)
                {

                    //JobDefinitionTitle, WebApplicationName, ServerName, Status, StartTime, EndTime, DatabaseName, ErrorMessage
                    string JobDefinitionTitle = ps.Properties["JobDefinitionTitle"].Value == null ? "" : ps.Properties["JobDefinitionTitle"].Value.ToString();
                    string WebApplicationName = ps.Properties["WebApplicationName"].Value == null ? "" : ps.Properties["WebApplicationName"].Value.ToString();
                    string ServerName = ps.Properties["ServerName"].Value == null ? "" : ps.Properties["ServerName"].Value.ToString();
                    string Status = ps.Properties["Status"].Value == null ? "" : ps.Properties["Status"].Value.ToString();
                    string StartTime = ps.Properties["StartTime"].Value == null ? "" : ps.Properties["StartTime"].Value.ToString();
                    string EndTime = ps.Properties["EndTime"].Value == null ? "" : ps.Properties["EndTime"].Value.ToString();
                    string DatabaseName = ps.Properties["DatabaseName"].Value == null ? "" : ps.Properties["DatabaseName"].Value.ToString();
                    string ErrorMessage = ps.Properties["ErrorMessage"].Value == null ? "" : ps.Properties["ErrorMessage"].Value.ToString();
                    string Schedule = ps.Properties["Schedule"].Value == null ? "" : ps.Properties["Schedule"].Value.ToString();
                    string Farm = ps.Properties["Farm"].Value == null ? "" : ps.Properties["Farm"].Value.ToString();

                    if(!dictOfJobs.ContainsKey(Farm))
                        dictOfJobs.Add(Farm, new List<VSNext.Mongo.Entities.SharePointTimerJob>());

                    dictOfJobs[Farm].Add(new VSNext.Mongo.Entities.SharePointTimerJob()
                    {
                        DatabaseName = DatabaseName,
                        EndTime = EndTime == "" ? null : (DateTime?)Convert.ToDateTime(EndTime),
                        ErrorMessage = ErrorMessage,
                        JobDefinitionTitle = JobDefinitionTitle,
                        Schedule = Schedule,
                        ServerName = ServerName,
                        StartTime = StartTime == "" ? null : (DateTime?)Convert.ToDateTime(StartTime),
                        Status = Status,
                        WebApplicationName = WebApplicationName
                    });

                }

                foreach(string FarmName in dictOfJobs.Keys)
                {
                    MongoStatementsUpdate<VSNext.Mongo.Entities.Status> mongoUpdate = new MongoStatementsUpdate<VSNext.Mongo.Entities.Status>();
                    mongoUpdate.filterDef = mongoUpdate.repo.Filter.Where(i => i.TypeAndName == FarmName + "-" + SharePointFarmServerType);
                    mongoUpdate.updateDef = mongoUpdate.repo.Updater.Set(i => i.SharePointTimerJobs, dictOfJobs[FarmName]);
                    AllTestsList.MongoEntity.Add(mongoUpdate);
                }





            }
            catch (Exception ex)
            {
                Common.WriteDeviceHistoryEntry(DummyServerForLogs.ServerType, DummyServerForLogs.Name, "Error in CheckTimerJobs: " + ex.Message, commonEnums.ServerRoles.SharePoint, Common.LogLevel.Normal);

            }
            finally
            {

            }
        }


		#endregion

	}
}
