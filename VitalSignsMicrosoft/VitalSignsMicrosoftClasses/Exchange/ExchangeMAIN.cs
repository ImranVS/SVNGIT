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
		MonitoredItems.ExchangeServersCollection myExchangeMailProbes;
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
				Common.WriteDeviceHistoryEntry("All", "Exchange", "Exchagne Service is starting up", Common.LogLevel.Normal);
				//myExchangeServers = CreateExchangeServersCollection();

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

				if (db.RecordExists("SELECT * FROM SelectedFeatures sf WHERE sf.FeatureID in (SELECT ID FROM Features WHERE Name='Exchange')"))
				{
					Common.WriteDeviceHistoryEntry("All", "Exchange", "Exchange is marked for scanning so will start Exchange Related Tasks", Common.LogLevel.Normal);

					CreateExchangeServersCollection();
					InitStatusTable(myExchangeServers);
					StartExchangeThreads(true);

					//CreateExchangeMailProbeCollection();
					////set the status for all mail probes
					//InitStatusTable(myExchangeMailProbes);
					Thread.Sleep(60 * 1000 * 1);
					StartMailProbeThreads();

					CreateExchangeDAGCollection();
					InitStatusTable(myDagCollection);
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

				//sleep for one minute to allow time for the collection to be made 
				Thread.Sleep(60 * 1000 * 1);

				Thread HourlyTasksThread = new Thread(new ThreadStart(HourlyTasks));
				HourlyTasksThread.CurrentCulture = c;
				HourlyTasksThread.IsBackground = true;
				HourlyTasksThread.Name = "HourlyTasks - Exchange";
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
				DailyTasksThread.Name = "DailyTasks - Exchange";
				DailyTasksThread.Start();
				Thread.Sleep(2000);

				Common.WriteDeviceHistoryEntry("All", "Exchange", "All Processes are started in startProcess", Common.LogLevel.Normal);
			}
			catch (Exception ex)
			{
				Common.WriteDeviceHistoryEntry("All", "Exchange", "Error starting StartProcess exception CreateExchangeServersCollection: " + ex.Message.ToString() + "..." + ex.StackTrace.ToString(), Common.LogLevel.Normal);
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
			StringBuilder SQL = new StringBuilder();
			SQL.Append(" select distinct Sr.ID,Sr.ServerName,S.ServerType,S.ID as ServerTypeId,L.Location,sa.ScanInterval,sa.RetryInterval,sa.OffHourInterval,sa.Enabled,sr.ipaddress,sa.category,cr.UserID,cr.Password, ");
			SQL.Append(" CASSmtp,CASPop3,CASImap,CASOARPC,CASOWA,CASActiveSync,CASEWS,CASECP,CASAutoDiscovery,CASOAB,SubQThreshold,PoisonQThreshold,UnReachableQThreshold,TotalQThreshold,");
			SQL.Append(" ES.VersionNo, sa.ResponseTime,ScanDAGHealth,cr2.UserID ASUserId,cr2.Password ASPassword, sa.ConsOvrThresholdBefAlert,sa.ConsFailuresBefAlert,ES.EnableLatencyTest,ES.LatencyRedThreshold,ES.LatencyYellowThreshold, ");
			SQL.Append(" ShadowQThreshold, st.LastUpdate, st.Status, st.StatusCode, di.CurrentNodeID, sa.CPU_Threshold, sa.MemThreshold, ES.AuthenticationType ");
			SQL.Append("  from Servers Sr ");
			SQL.Append(" inner join ServerTypes S on Sr.ServerTypeID=S.ID  inner join Locations L on Sr.LocationID =L.ID  left outer join ServerAttributes sa on sr.ID=sa.serverid ");
			SQL.Append(" inner join credentials cr on sa.CredentialsId=cr.ID ");
			SQL.Append(" left outer join ExchangeSettings ES on sr.ID=ES.ServerId ");
			SQL.Append(" left outer join credentials cr2 on ES.ActiveSyncCredentialsId=cr2.ID ");
			if (ConfigurationManager.AppSettings["VSNodeName"] != null)
			{
				string NodeName = ConfigurationManager.AppSettings["VSNodeName"].ToString();
				SQL.Append(" inner join DeviceInventory di on Sr.ID=di.DeviceID and Sr.ServerTypeId=di.DeviceTypeId ");
				SQL.Append(" inner join Nodes on (Nodes.ID=di.CurrentNodeId or di.CurrentNodeId=-1) and Nodes.Name='" + NodeName + "' ");
			}
			SQL.Append(" left outer join Status st on st.Type=S.ServerType and st.Name=Sr.ServerName ");
			SQL.Append(" where S.ServerType='Exchange' and sa.Enabled = 1 order by sr.id");
			DataTable dtServers = DB.GetData(SQL.ToString());

			listOfIdsForExchange = String.Join(",", dtServers.AsEnumerable().Select(r => r.Field<Int32>("ID").ToString()).ToList());
			//Loop through servers
			MonitoredItems.ExchangeThresholdSettings ExchgThreshold = new MonitoredItems.ExchangeThresholdSettings();
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

					MonitoredItems.ExchangeServer myExchangeServer = null;// = new MonitoredItems.ExchangeServer();

					//Checks to see if the server is newly added or exists.  Adds if it is new
					try
					{
						myExchangeServer = myExchangeServers.SearchByName(DR["ServerName"].ToString());
						if (myExchangeServer == null)
						{
							//New server.  Set inits and add to collection

							myExchangeServer = InitForExchangeServers(myExchangeServer, DR);
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
						//New server.  Set inits and add to collection
						myExchangeServer = InitForExchangeServers(myExchangeServer, DR);
						myExchangeServers.Add(myExchangeServer);
						newServers++;
					}

					ExchgThreshold = GetExhangeThresholdSettings(DR);
					myExchangeServer.ThresholdSetting = ExchgThreshold;

					myExchangeServer = SetExchangeServerSettings(myExchangeServer, DR);

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

		private MonitoredItems.ExchangeServer InitForExchangeServers(MonitoredItems.ExchangeServer MyExchangeServer, DataRow DR)
		{
			MyExchangeServer = new MonitoredItems.ExchangeServer();
			MyExchangeServer.Role = new String[0];
			MyExchangeServer.ServerType = "Exchange";
			MyExchangeServer.LastScan = DR["LastUpdate"] == null || DR["LastUpdate"].ToString() == "" ? DateTime.Now : DateTime.Parse(DR["LastUpdate"].ToString());
			MyExchangeServer.Status = DR["Status"] == null || DR["Status"].ToString() == "" ? "Not Scanned" : DR["Status"].ToString();
			MyExchangeServer.StatusCode = DR["StatusCode"] == null || DR["StatusCode"].ToString() == "" ? "Maintenance" : DR["StatusCode"].ToString();

			return MyExchangeServer;
		}

		private MonitoredItems.ExchangeServer SetExchangeServerSettings(MonitoredItems.ExchangeServer MyExchangeServer, DataRow DR)
		{
			MyExchangeServer.ServerId = DR["ID"].ToString();
			MyExchangeServer.IPAddress = DR["IPAddress"].ToString();
			MyExchangeServer.Name = DR["ServerName"].ToString();
			MyExchangeServer.UserName = DR["UserID"].ToString();
			MyExchangeServer.Password = decodePasswordFromEncodedString(DR["Password"].ToString(), MyExchangeServer.Name);
			MyExchangeServer.Location = DR["Location"].ToString();
			MyExchangeServer.ResponseThreshold = long.Parse(DR["ResponseTime"].ToString());
			MyExchangeServer.ScanInterval = int.Parse(DR["ScanInterval"].ToString());
			MyExchangeServer.OffHoursScanInterval = int.Parse(DR["OffHourInterval"].ToString());
			MyExchangeServer.RetryInterval = int.Parse(DR["RetryInterval"].ToString());
			MyExchangeServer.CPU_Threshold = int.Parse(DR["CPU_Threshold"].ToString());
			MyExchangeServer.Memory_Threshold = int.Parse(DR["MemThreshold"].ToString());

			MyExchangeServer.DAGScan = DR["ScanDAGHealth"].ToString() == "" ? false : bool.Parse(DR["ScanDAGHealth"].ToString());
			MyExchangeServer.VersionNo =  DR["VersionNo"].ToString();
			MyExchangeServer.CASActiveSync = DR["CASActiveSync"].ToString() == "" ? false : Convert.ToBoolean(DR["CASActiveSync"]);
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
			MyExchangeServer.ServerDaysAlert = DR["ConsOvrThresholdBefAlert"].ToString();
			MyExchangeServer.FailureThreshold = int.Parse(DR["ConsFailuresBefAlert"].ToString());
			MyExchangeServer.Category = DR["Category"].ToString();
			MyExchangeServer.ServerTypeId =int.Parse( DR["ServerTypeId"].ToString());
			MyExchangeServer.LatencyRedThreshold = (DR["LatencyRedThreshold"] == null || DR["LatencyRedThreshold"].ToString() == "") ? 20000 : int.Parse(DR["LatencyRedThreshold"].ToString());
			MyExchangeServer.LatencyYellowThreshold = (DR["LatencyYellowThreshold"] == null || DR["LatencyYellowThreshold"].ToString() == "") ? 20000 : int.Parse(DR["LatencyYellowThreshold"].ToString());
			MyExchangeServer.EnableLatencyTest = (DR["EnableLatencyTest"] == null || DR["EnableLatencyTest"].ToString() == "") ? false : Convert.ToBoolean(DR["EnableLatencyTest"]);
			MyExchangeServer.Enabled = true;
			MyExchangeServer.InsufficentLicenses = DR["CurrentNodeID"] != null && DR["CurrentNodeID"].ToString() == "-1" ? true : false;
			MyExchangeServer.AuthenticationType = DR["AuthenticationType"] != null && DR["AuthenticationType"].ToString() != "" ? DR["AuthenticationType"].ToString() : "Default";

			return MyExchangeServer;
		}

		private MonitoredItems.ExchangeThresholdSettings GetExhangeThresholdSettings(DataRow DR)
		{
			MonitoredItems.ExchangeThresholdSettings ExchgThreshold = new MonitoredItems.ExchangeThresholdSettings();
			ExchgThreshold.PoisonQThreshold = DR["PoisonQThreshold"].ToString() == "" ? -1 : Convert.ToInt32(DR["PoisonQThreshold"]);
			ExchgThreshold.SubQThreshold = DR["SubQThreshold"].ToString() == "" ? -1 : Convert.ToInt32(DR["SubQThreshold"]);
			ExchgThreshold.TotalQThreshold = DR["TotalQThreshold"].ToString() == "" ? -1 : Convert.ToInt32(DR["TotalQThreshold"]);
			ExchgThreshold.UnReachableQThreshold = DR["UnReachableQThreshold"].ToString() == "" ? -1 : Convert.ToInt32(DR["UnReachableQThreshold"]);
			ExchgThreshold.ShadowQThreshold = DR["ShadowQThreshold"].ToString() == "" ? -1 : Convert.ToInt32(DR["ShadowQThreshold"]);
			return ExchgThreshold;
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
			SQL.Append(" ,S.ServerType, S.ID as ServerTypeId, C.UserID,C.Password, st.LastUpdate, st.Status, st.StatusCode, di.CurrentNodeID , sa.CPU_Threshold, sa.MemThreshold from LyncServers LS inner join Servers Sr  on LS.ServerID=Sr.ID");
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
			MyLyncServer.LastScan = DR["LastUpdate"] == null || DR["LastUpdate"].ToString() == "" ? DateTime.Now : DateTime.Parse(DR["LastUpdate"].ToString());
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
			MyLyncServer.CPU_Threshold = int.Parse(DR["CPU_Threshold"].ToString());
			MyLyncServer.Memory_Threshold = int.Parse(DR["MemThreshold"].ToString());
			MyLyncServer.InsufficentLicenses = DR["CurrentNodeID"] != null && DR["CurrentNodeID"].ToString() == "-1" ? true : false;

			return MyLyncServer;
		}

		#endregion

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
					thisServer = SelectExchangeServerToMonitor();
				
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
                    Common.SetupServer(thisServer, thisServer.ServerType);
					TestResults AllTestResults = new TestResults();
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

		public MonitoredItems.ExchangeServer SelectExchangeServerToMonitor()
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
			String ServerType = "Exchange";
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

					for (int n = 0; n < myExchangeServers.Count; n++)
					{
						ServerOne = myExchangeServers.get_Item(n);
						if (ServerOne.Name == serverName && ServerOne.IsBeingScanned == false && ServerOne.Enabled)
						{
							Common.WriteDeviceHistoryEntry("All", "Exchange", serverName + " was marked 'Scan ASAP' so it will be scanned next.");

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
			for (int n = 0; n < myExchangeServers.Count; n++)
			{
				ServerOne = myExchangeServers.get_Item(n);
				if (ServerOne.Name == ScanASAP && ServerOne.IsBeingScanned == false && ServerOne.Enabled)
				{
					Common.WriteDeviceHistoryEntry("All", "Exchange", ScanASAP + " was marked 'Scan ASAP' so it will be scanned next.");
					myRegistry.WriteToRegistry("ScanExchangeASAP", "n/a");

					//ServerOne.ScanASAP = true;

					return ServerOne;
				}

			}


			//Searches for the first enounter of a Not Responding server that is due for a scan
			for (int n = 0; n < myExchangeServers.Count; n++)
			{
				ServerOne = myExchangeServers.get_Item(n);
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
			for (int n = 0; n < myExchangeServers.Count; n++)
			{
				ServerOne = myExchangeServers.get_Item(n);
                if ((ServerOne.Status == "Not Scanned" || ServerOne.Status == "Master Service Stopped.") && ServerOne.IsBeingScanned == false && ServerOne.Enabled)
				{
					Common.WriteDeviceHistoryEntry("All", "Exchange", "Selecting " + ServerOne.Name + " because the status is " + ServerOne.Status + ".");
					return ServerOne;
				}
			}


			//Searches for all servers that are due for a scan
			List<MonitoredItems.ExchangeServer> ScanCanidates = new List<MonitoredItems.ExchangeServer>();

			foreach (MonitoredItems.ExchangeServer srv in myExchangeServers)
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
                ////ThreadPool.QueueUserWorkItem(new WaitCallback(MonitorExchange(i)), i);
                //ThreadPool.QueueUserWorkItem(new WaitCallback(delegate(object state){ MonitorExchange(i); }), null);

                //workingThread = new Thread(() => RoleMonitoring(ClassName, results, AllTestResults, thisServer, ref AlivePSO));
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

		public MonitoredItems.ExchangeServer findServerToScanBasedOnlyOnTime()
		{
			if (myExchangeServers.Count == 0)
				return null;
			if (myExchangeServers.Count == 1)
				return myExchangeServers.get_Item(0);

			MonitoredItems.ExchangeServer s1 = myExchangeServers.get_Item(0);
			MonitoredItems.ExchangeServer s2 = myExchangeServers.get_Item(1);

			for (int n = 2; n < myExchangeServers.Count; n++)
			{
				if (s1.IsBeingScanned)
				{
					s1 = myExchangeServers.get_Item(n);
					continue;
				}
				if (s2.IsBeingScanned)
				{
					s2 = myExchangeServers.get_Item(n);
					continue;
				}
				if (DateTime.Compare(s1.LastScan, s2.LastScan) < 0)
				{
					if (!myExchangeServers.get_Item(n).IsBeingScanned)
						s2 = myExchangeServers.get_Item(n);
				}
				else
				{
					if (!myExchangeServers.get_Item(n).IsBeingScanned)
						s1 = myExchangeServers.get_Item(n);
				}
			}

			if (DateTime.Compare(s1.LastScan, s2.LastScan) < 0)
				if (!s1.IsBeingScanned)
					return s1;
			if (!s2.IsBeingScanned)
				return s2;
			return null;

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
                            mongoUpdate.filterDef = mongoUpdate.repo.Filter.Where(i => i.ServerName == myServer.Name && i.ServerType == myServer.ServerType);
                            mongoUpdate.updateDef = mongoUpdate.repo.Updater
                                .Set(i => i.SoftwareVersion, Convert.ToDouble(Version))
                                .Set(i => i.ServerRoles, myServer.Role.ToList());

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
		public MonitoredItems.ExchangeServer findLyncServerToScanBasedOnlyOnTime()
		{
			if (myLyncServers.Count == 0)
				return null;
			if (myLyncServers.Count == 1)
				return myLyncServers.get_Item(0);

			MonitoredItems.ExchangeServer s1 = myLyncServers.get_Item(0);
			MonitoredItems.ExchangeServer s2 = myLyncServers.get_Item(1);

			for (int n = 2; n < myLyncServers.Count; n++)
			{
				if (s1.IsBeingScanned)
				{
					s1 = myLyncServers.get_Item(n);
					continue;
				}
				if (s2.IsBeingScanned)
				{
					s2 = myLyncServers.get_Item(n);
					continue;
				}
				if (DateTime.Compare(s1.LastScan, s2.LastScan) < 0)
				{
					if (!myLyncServers.get_Item(n).IsBeingScanned)
						s2 = myLyncServers.get_Item(n);
				}
				else
				{
					if (!myLyncServers.get_Item(n).IsBeingScanned)
						s1 = myLyncServers.get_Item(n);
				}
			}

			if (DateTime.Compare(s1.LastScan, s2.LastScan) < 0)
				if (!s1.IsBeingScanned)
					return s1;
			if (!s2.IsBeingScanned)
				return s2;
			return null;

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
                        if (Convert.ToInt32(server.VersionNo) > newestVersion)
                        {
                            newestVersion = Convert.ToInt32(server.VersionNo);
                            testServer = server;
                        }
                    }
                Common.CommonDailyTasks(testServer, ref AllTestResults, testServer.ServerType);






				Common.WriteDeviceHistoryEntry("All", "Exchange", "Finished Daily Tasks.", commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);
				Common.WriteDeviceHistoryEntry("Exchange", DummyServerForLogs.Name, "Finished Daily Tasks.", commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);

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
                            EMB.getMailBoxInfo(testServer, ref AllTestResults, testServer.VersionNo.ToString(), DummyServerForLogs.Name, myExchangeServers, results);
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
                            string cmdlets = "-CommandName Get-MailboxDatabase,Get-MailboxStatistics";
                            
                            ReturnPowerShellObjects results = Common.PrereqForExchangeWithCmdlets(myServer.Name, myServer.DAGPrimaryUserName, myServer.DAGPrimaryPassword, myServer.ServerType, myServer.DAGPrimaryIPAddress, commonEnums.ServerRoles.Empty, cmdlets, myServer.DAGPrimaryAuthenticationType);

                            string Version = "";
                            string IPAddress = "";

                            if (results.Connected == false)
                            {
                                Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "Unable to connect to primary server.  Will attempt backup", commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);
                                results.Dispose();
                                results = Common.PrereqForExchangeWithCmdlets(myServer.Name, myServer.DAGBackupUserName, myServer.DAGBackupPassword, myServer.ServerType, myServer.DAGBackupIPAddress, commonEnums.ServerRoles.Empty, cmdlets, myServer.DAGBackupAuthenticationType);
                                IPAddress = myServer.DAGBackupIPAddress;
                                Version = myExchangeServers.SearchByIPAddress(IPAddress) == null ? "" : myExchangeServers.SearchByIPAddress(IPAddress).VersionNo;
                            }
                            else
                            {

                                Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "Connected to primary server.", commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);
                                IPAddress = myServer.DAGPrimaryIPAddress;
                                Version = myExchangeServers.SearchByIPAddress(IPAddress) == null ? "" : myExchangeServers.SearchByIPAddress(IPAddress).VersionNo;

                            }

                            if (results.Connected == false)
                            {
                                Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "Unable to connect to backup server.", commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);
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

                    }






                    /*

                    int newestVersion = -1;
                    MonitoredItems.ExchangeServer myServer = null;
                    if (myDagCollection != null)
                    {
                        foreach (MonitoredItems.ExchangeServer server in myDagCollection)
                        {





                            if(server.Status == "Not Responding")
                                continue;

                            MonitoredItems.ExchangeServer primaryServer = myExchangeServers.SearchByIPAddress(server.DAGPrimaryIPAddress);
                            MonitoredItems.ExchangeServer secondaryServer = myExchangeServers.SearchByIPAddress(server.DAGBackupIPAddress);

                            if (primaryServer != null && Convert.ToInt32(primaryServer.VersionNo) > newestVersion && primaryServer.Status != "Not Responding" && primaryServer.Status != "Maintenance")
                            {
                                newestVersion = Convert.ToInt32(primaryServer.VersionNo);
                                myServer = server;
                            }

                            if (secondaryServer != null && Convert.ToInt32(secondaryServer.VersionNo) > newestVersion && secondaryServer.Status != "Not Responding" && secondaryServer.Status != "Maintenance")
                            {
                                newestVersion = Convert.ToInt32(secondaryServer.VersionNo);
                                myServer = server;
                            }
                        }
                    }

                    if (myServer != null)
                    {





                        Common.WriteDeviceHistoryEntry("Exchange", DummyServerForLogs.Name, "Server " + myServer.Name + " will be used to perform tests.", commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);

                        TestResults AllTestResults = new TestResults();
                        string Version = "";

                        string cmdlets = "-CommandName ";//g;
		                ReturnPowerShellObjects results = Common.PrereqForExchangeWithCmdlets(myServer.Name, myServer.DAGPrimaryUserName, myServer.DAGPrimaryPassword, myServer.ServerType, myServer.DAGPrimaryIPAddress, commonEnums.ServerRoles.Empty, cmdlets, myServer.DAGPrimaryAuthenticationType);
                        Version = myExchangeServers.SearchByIPAddress(myServer.DAGPrimaryIPAddress) == null ? "" : myExchangeServers.SearchByIPAddress(myServer.DAGPrimaryIPAddress).VersionNo;
		                string IPAddress = myServer.DAGPrimaryIPAddress;
                        if (results.Connected == false)
		                {
			                Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "Unable to connect to primary server.  Will attempt backup",commonEnums.ServerRoles.Empty,Common.LogLevel.Normal);
			                results.Dispose();
			                results = Common.PrereqForExchangeWithCmdlets(myServer.Name, myServer.DAGBackupUserName, myServer.DAGBackupPassword, myServer.ServerType, myServer.DAGBackupIPAddress, commonEnums.ServerRoles.Empty, cmdlets, myServer.DAGBackupAuthenticationType);
			                IPAddress = myServer.DAGBackupIPAddress;
                            Version = myExchangeServers.SearchByIPAddress(myServer.DAGBackupIPAddress) == null ? "" : myExchangeServers.SearchByIPAddress(myServer.DAGBackupIPAddress).VersionNo;
		                }
                        if (results.Connected == false)
                        {
                            Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "Unable to connect to backup server.", commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);
                            results.Dispose();
                            return;
                        }
                        else
                        {
                            //getMailboxDatabaseDetails(myServer, results.PS, ref AllTestResults, Version, DummyServerForLogs.Name)
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
                     * */
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
                string SqlStr = "select ServerName, DatabaseName, DatabaseSizeThreshold, WhiteSpaceThreshold from ExchangeDatabaseSettings";
                CommonDB db = new CommonDB();
                DataTable dt = db.GetData(SqlStr);

                Common.WriteDeviceHistoryEntry("Exchange", DummyServerName, "In getMailboxDatabaseDetails.", commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);
                string ServerNames = "'" + String.Join("','", myExchangeServers.Cast<MonitoredItems.ExchangeServer>().ToArray().Select(s => s.Name).ToList()) + "'";
                
                System.Collections.ObjectModel.Collection<PSObject> results = new System.Collections.ObjectModel.Collection<PSObject>();
                System.IO.StreamReader sr = new System.IO.StreamReader(AppDomain.CurrentDomain.BaseDirectory.ToString() + "Scripts\\EX_MailBoxReportByTotalSize&Count.ps1");
                string startOfScript = "$databases = Get-MailboxDatabase -IncludePreExchange" + serverVersionNo + " -status | sort name\n";
                String str = startOfScript + sr.ReadToEnd();
                powershell.Streams.Error.Clear();

                powershell.AddScript(str);
                results = powershell.Invoke();

                if (powershell.Streams.Error.Count > 51)
                {

                    Common.WriteDeviceHistoryEntry("Exchange", DummyServerName, "getMailboxDatabaseDetails received over 51 errors", commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);
                }
                else
                {
                    Common.WriteDeviceHistoryEntry("Exchange", DummyServerName, "getMailboxDatabaseDetails output results: " + results.Count.ToString(), commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);

                    Dictionary<MonitoredItems.ExchangeServer, String> dict = new Dictionary<MonitoredItems.ExchangeServer, string>();
                    foreach (PSObject ps in results)
                    {

                        string ServerName = ps.Properties["ServerName"].Value == null ? "" : ps.Properties["ServerName"].Value.ToString();
                        string DatabaseName = ps.Properties["DataBaseName"].Value == null ? "" : ps.Properties["DataBaseName"].Value.ToString();
                        string SizeMB = ps.Properties["SizeMB"].Value == null ? "0" : ps.Properties["SizeMB"].Value.ToString();
                        string WhiteSpaceMB = ps.Properties["WhiteSpaceMB"].Value == null ? "0" : ps.Properties["WhiteSpaceMB"].Value.ToString();
                        string MBXs = ps.Properties["MBXs"].Value == null ? "0" : ps.Properties["MBXs"].Value.ToString();
                        string MBXsdisc = ps.Properties["MBXsdisc"].Value == null ? "0" : ps.Properties["MBXsdisc"].Value.ToString();
                        string Mbcount = ps.Properties["Mbcount"].Value == null ? "0" : ps.Properties["Mbcount"].Value.ToString();
                        string DagName = ps.Properties["DagName"].Value == null ? "" : ps.Properties["DagName"].Value.ToString();


                        MongoStatementsUpdate<VSNext.Mongo.Entities.Status> mongoUpdate = new MongoStatementsUpdate<VSNext.Mongo.Entities.Status>();
                        mongoUpdate.filterDef = mongoUpdate.repo.Filter.Eq(i => i.TypeAndName, DagName + "-" + "Database Availability Group") 
                            & !mongoUpdate.repo.Filter.ElemMatch(i => i.DagDatabases, i => i.DatabaseName == DatabaseName);
                        VSNext.Mongo.Entities.DagDatabases dbg = new VSNext.Mongo.Entities.DagDatabases()
                        {
                            DatabaseName = DatabaseName
                        };
                        mongoUpdate.updateDef = mongoUpdate.repo.Updater.Push(i => i.DagDatabases, dbg);
                        AllTestResults.MongoEntity.Add(mongoUpdate);         

                        mongoUpdate = new MongoStatementsUpdate<VSNext.Mongo.Entities.Status>();
                        mongoUpdate.filterDef = mongoUpdate.repo.Filter.Where(i => i.TypeAndName == DagName.ToString() + "-Database Availability Group") & mongoUpdate.repo.Filter.ElemMatch(i => i.DagDatabases, i => i.DatabaseName == DatabaseName);
                        mongoUpdate.updateDef = mongoUpdate.repo.Updater
                            .Set(i => i.DagDatabases[-1].ConnectedMailboxCount, Convert.ToInt32(Mbcount))
                            .Set(i => i.DagDatabases[-1].DisconnectedMailboxCount, Convert.ToInt32(MBXsdisc))
                            .Set(i => i.DagDatabases[-1].MailboxCount, Convert.ToInt32(MBXs))
                            .Set(i => i.DagDatabases[-1].SizeMB, Convert.ToDouble(SizeMB))
                            .Set(i => i.DagDatabases[-1].WhiteSpaceMB, Convert.ToDouble(WhiteSpaceMB))
                            .Set(i => i.DagDatabases[-1].ServerName, ServerName);
                        AllTestResults.MongoEntity.Add(mongoUpdate);
                        
                        if(myDagCollection.SearchByName(DagName) != null)
                            AllTestResults.MongoEntity.Add(Common.GetInsertIntoDailyStats(Server, "ExDatabaseSizeMb." + DatabaseName, SizeMB));

                        /*
                        //alerts
                        if (dt.Rows.Count > 0 && dt.Rows[0]["DatabaseName"].ToString() != "NoAlerts")
                        {
                            DataRow[] curr;
                            curr = dt.Select("ServerName='" + DagName + "' AND (DatabaseName='" + DatabaseName + "' or DatabaseName='AllDatabases')");
                            
                            if (curr.Count() > 0)
                            {
                                try
                                {
                                    MonitoredItems.ExchangeServer currServer = myExchangeServers.SearchByName(ServerName);

                                    if (currServer != null)
                                    {
                                        if (!dict.ContainsKey(currServer))
                                            dict.Add(currServer, "");
                                        DataRow row = curr[0];
                                        string sizeThreshold = row["DatabaseSizeThreshold"].ToString();
                                        string whitespaceThreshold = row["WhiteSpaceThreshold"].ToString();

                                        double dblWhiteSpace;
                                        double dblSize;

                                        bool boolWhiteSpace = Double.TryParse(WhiteSpaceMB, out dblWhiteSpace);
                                        bool boolSize = Double.TryParse(SizeMB, out dblSize);

                                        if (boolWhiteSpace)
                                            boolWhiteSpace = Double.Parse(WhiteSpaceMB) > Double.Parse(whitespaceThreshold) && Double.Parse(whitespaceThreshold) != 0;
                                        if (boolSize)
                                            boolSize = Double.Parse(SizeMB) > Double.Parse(sizeThreshold) && Double.Parse(sizeThreshold) != 0;

                                        if (boolSize && boolWhiteSpace)
                                        {
                                            //details = "The whtiespace and the size of database " + DatabaseName + " excedes the thresholds";
                                            dict[currServer] += "The whitespace and the size of database " + DatabaseName + " excedes the thresholds. ";
                                            overallDBSizeAlert = false;
                                            overallWhitespaceAlert = false;
                                        }
                                        else if (boolSize)
                                        {
                                            //details = "The size of database " + DatabaseName + " excedes the threshold limit of " + Double.Parse(sizeThreshold);
                                            dict[currServer] += "The size of database " + DatabaseName + "  is " + Math.Round(Double.Parse(SizeMB), 2) + " MB and excedes the threshold of " + sizeThreshold + " MB. ";
                                            overallDBSizeAlert = false;
                                        }
                                        else if (boolWhiteSpace)
                                        {
                                            //details = "The size of the whitespace of database " + DatabaseName + " excedes the threshold of " + Double.Parse(whitespaceThreshold);
                                            dict[currServer] += "The whitespace of database " + DatabaseName + " is " + Math.Round(Double.Parse(WhiteSpaceMB), 2) + " MB and  excedes the threshold of " + whitespaceThreshold + " MB. ";
                                            overallWhitespaceAlert = false;
                                        }
                                        else
                                        {
                                            //details = "Database " + DatabaseName + " is within thresholds";
                                        }
                                        /*
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


                        */

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

                Common.WriteDeviceHistoryEntry("Exchange", DummyServerName, "Error in getMailboxDatabaseDetails : " + ex.Message, commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);

            }

        }


        #endregion

        public Collection<PSObject> TestDatabaseCorruptionQueue(MonitoredItems.ExchangeServer myServer, ref TestResults AllTestsList, string DummyServernameForLogs, PowerShell powershell)
        {

            Collection<PSObject> results = null;

            Common.WriteDeviceHistoryEntry("Exchange", DummyServernameForLogs, "In TestDatabaseCorruptionQueue ", commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);
            try
            {
                results = new System.Collections.ObjectModel.Collection<PSObject>();

                System.IO.StreamReader sr = new System.IO.StreamReader(AppDomain.CurrentDomain.BaseDirectory.ToString() + "Scripts\\EX_DatabaseCorruptionCheck_Queue.ps1");
                String str = sr.ReadToEnd();

                powershell.AddScript(str);
                results = powershell.Invoke();

                Common.WriteDeviceHistoryEntry("Exchange", DummyServernameForLogs, "EX_DatabaseCorruptionCheck_Queue output results: " + results.Count.ToString(), commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);

            }
            catch (Exception ex)
            {

                Common.WriteDeviceHistoryEntry("Exchange", DummyServernameForLogs, "Error in TestDatabaseCorruptionQueue: " + ex.Message, commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);

            }
            finally
            {

            }

            return results;

        }

        public void TestDatabaseCorruptionStatus(MonitoredItems.ExchangeServer myServer, ref TestResults AllTestsList, string DummyServernameForLogs, PowerShell powershell, Collection<PSObject> input)
        {

            Common.WriteDeviceHistoryEntry("Exchange", DummyServernameForLogs, "In TestDatabaseCorruptionStatus ", commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);
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

                Common.WriteDeviceHistoryEntry("Exchange", DummyServernameForLogs, "EX_DatabaseCorruptionCheck_Status output results: " + results.Count.ToString(), commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);




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

                        Common.WriteDeviceHistoryEntry("Exchange", DummyServernameForLogs, "TestDatabaseCorruptionQueue Object Results: ", commonEnums.ServerRoles.Empty, Common.LogLevel.Verbose);
                        Common.WriteDeviceHistoryEntry("Exchange", DummyServernameForLogs, "DatabaseName: " + DatabaseName, commonEnums.ServerRoles.Empty, Common.LogLevel.Verbose);
                        Common.WriteDeviceHistoryEntry("Exchange", DummyServernameForLogs, "ServerName: " + ServerName, commonEnums.ServerRoles.Empty, Common.LogLevel.Verbose);
                        Common.WriteDeviceHistoryEntry("Exchange", DummyServernameForLogs, "CorruptionsDetected: " + CorruptionsDetected, commonEnums.ServerRoles.Empty, Common.LogLevel.Verbose);
                        Common.WriteDeviceHistoryEntry("Exchange", DummyServernameForLogs, "ErrorCode: " + ErrorCode, commonEnums.ServerRoles.Empty, Common.LogLevel.Verbose);
                        Common.WriteDeviceHistoryEntry("Exchange", DummyServernameForLogs, "Corruptions: " + Corruptions, commonEnums.ServerRoles.Empty, Common.LogLevel.Verbose);
                        Common.WriteDeviceHistoryEntry("Exchange", DummyServernameForLogs, "Progress: " + Progress, commonEnums.ServerRoles.Empty, Common.LogLevel.Verbose);
                        Common.WriteDeviceHistoryEntry("Exchange", DummyServernameForLogs, "JobState: " + JobState, commonEnums.ServerRoles.Empty, Common.LogLevel.Verbose);


                    }


                    foreach (string server in objList.Select(l => l.ServerName).Distinct())
                    {
                        string msg = "";
                        bool reset;
                        List<DatabaseCorruptionResult> tempList = objList.Where(l => l.ServerName == server).ToList();
                        if(tempList.Where(l => l.CorruptionsDetected != "0").ToList().Count() > 0)
                        {
                            int TotalNumOfCorruptions = tempList.Sum(l => Convert.ToInt32(l.CorruptionsDetected));
                            msg = "There were " + TotalNumOfCorruptions + " corruptions detected on databases on the server";
                        }
                        else if (tempList.Where(l => l.Progress != "100").ToList().Count() > 0)
                        {
                            int NumOfUnfinishedScans = tempList.Where(l => l.Progress != "100").ToList().Count();
                            msg = "There were " + NumOfUnfinishedScans + " tests unfinished in the allowed time. This can indicate there might be an issue with the server";
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

                Common.WriteDeviceHistoryEntry("Exchange", DummyServernameForLogs, "Error in TestDatabaseCorruptionQueue: " + ex.Message, commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);

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
		private void CreateExchangeMailProbeCollection()
		{
			//Fetch all servers
			if (myExchangeMailProbes == null)
				myExchangeMailProbes = new MonitoredItems.ExchangeServersCollection();
			MonitoredItems.ExchangeServersCollection newCollection = new MonitoredItems.ExchangeServersCollection();
			CommonDB DB = new CommonDB();
			StringBuilder SQL = new StringBuilder();
			SQL.Append(" select distinct Sr.ID,Sr.ServerName,S.ServerType,L.Location,EMP.ScanInterval,EMP.RetryInterval,EMP.OffHoursScanInterval,sa.Enabled,sr.ipaddress,sa.category,cr.UserID,cr.Password,");
			SQL.Append(" EMP.Name, EMP.EXCHANGEMAILADDRESS,EMP.DELIVERYTHRESHOLD, ES.AuthenticationType  ");
			SQL.Append(" from Servers Sr  inner join ServerTypes S on Sr.ServerTypeID=S.ID  inner join Locations L on Sr.LocationID =L.ID    ");
			SQL.Append("  inner join ServerAttributes sa on sr.ID=sa.serverid  inner join credentials cr on sa.CredentialsId=cr.ID  ");
			SQL.Append("  inner join ExchangeMailProbe EMP on EMP.SourceServerId=Sr.ID and EMP.Enabled=1 AND EMP.Category='Exchange'  ");
			SQL.Append("  where S.ServerType='Exchange' and sa.Enabled = 1 order by sr.id");
			
			DataTable dtServers = DB.GetData(SQL.ToString());
			//Loop through servers
			if (dtServers.Rows.Count > 0)
			{
				for (int i = 0; i < dtServers.Rows.Count; i++)
				{
					DataRow DR = dtServers.Rows[i];
					MonitoredItems.ExchangeServer myExchangeServer = new MonitoredItems.ExchangeServer();
					newCollection.Add(SetMailProbeSettings(myExchangeServer, DR));
				}

				int updatedServers = 0;
				int newServers = 0;
				int removedServers = 0;

				//Removes servers not in the new lsit
				foreach (MonitoredItems.ExchangeServer server in myExchangeMailProbes)
				{
					string currName = server.Name;
					try
					{
						MonitoredItems.ExchangeServer newServer = newCollection.SearchByName(currName);
						if (newServer == null)
						{
							//incase it doesnt throw and exception, if not found, removed server from list
							if (server.Enabled)
							myExchangeMailProbes.Delete(currName);
							removedServers++;
						}
					}
					catch (Exception ex)
					{
						//server not found
						if (server.Enabled)
							myExchangeMailProbes.Delete(currName);
						removedServers++;
					}
				}

				// adds/updates new servers
				foreach (MonitoredItems.ExchangeServer server in newCollection)
				{
					string currName = server.Name;
					try
					{
						MonitoredItems.ExchangeServer oldServer = myExchangeMailProbes.SearchByName(currName);

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
							oldServer.DeliveryThreshold = server.DeliveryThreshold;
							oldServer.MailProbeName = server.MailProbeName;
							oldServer.MailProbeAddress = server.MailProbeAddress;

							if (oldServer.Enabled == false)
								oldServer.Enabled = true;
							updatedServers++;
						}
						else
						{
							myExchangeMailProbes.Add(server);
							newServers++;
						}
					}
					catch (NullReferenceException ex)
					{
						myExchangeMailProbes.Add(server);
						newServers++;
					}

				}
				//myExchangeServers = newCollection;
				/**********************************************************/
				Common.WriteDeviceHistoryEntry("All", "Exchange", "There are " + myExchangeMailProbes.Count + " servers in the collection.  " + newServers + " were new and " + updatedServers + " were updated.");
			}
			else
			{
				myExchangeMailProbes = new MonitoredItems.ExchangeServersCollection();
			}


			//At this point we have all Servers with ALL the information(including Threshold settings)
		}
		private MonitoredItems.ExchangeServer SetMailProbeSettings(MonitoredItems.ExchangeServer MyExchangeServer, DataRow DR)
		{
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
			MyExchangeServer.LastScan = DateTime.Now;
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
			Thread MailFlowThread = null;
			int totalWait = 0;
			while (true)
			{
				try
				{
					if (MailFlowThread != null && MailFlowThread.IsAlive)
						MailFlowThread.Abort();
				}
				catch (Exception ex)
				{
					Common.WriteDeviceHistoryEntry("Exchange", "HeatMap", "Error killing MailFlow thread: " + ex.Message.ToString());
				}
				CommonDB DB = new CommonDB();
				int iInterval = 10;

				try
				{
					string sInterval = DB.GetData("Select svalue from Settings where sname='HeatMap Scan Interval'").Rows[0][0].ToString();
					if (sInterval != "")
						iInterval = Convert.ToInt32(sInterval);
				}
				catch
				{
					iInterval = 20;
				}

				try
				{
					myExchangeMailProbes = new MonitoredItems.ExchangeServersCollection();
					System.Collections.ArrayList mailFlow = new System.Collections.ArrayList();
					for (int n = 0; n < myExchangeServers.Count; n++)
					{
						MonitoredItems.ExchangeServer exServer = myExchangeServers.get_Item(n);
                        if (exServer.EnableLatencyTest)
                        {
                            Common.WriteDeviceHistoryEntry("Exchange", "HeatMap", "Adding Server to collection:" + exServer.Name.ToString(), commonEnums.ServerRoles.MailFlow, Common.LogLevel.Normal);
                            myExchangeMailProbes.Add(exServer);
                        }
					}
					totalWait = (3 - iInterval) * 60 * 1000;  //3 for a few mre minutes
					for (int i = 0; i < myExchangeMailProbes.Count; i++)
					{
						MonitoredItems.ExchangeServer exServer1 = myExchangeMailProbes.get_Item(i);
						for (int j = 0; j < myExchangeMailProbes.Count; j++)
						{
							MonitoredItems.ExchangeServer exServer2 = myExchangeMailProbes.get_Item(j);
							mailFlow.Add(new MonitoredItems.MailFlowTest() { SourceServer = exServer1.Name, DestinationServer = exServer2.Name, LatencyRedThreshold = exServer1.LatencyRedThreshold, LatencyYellowThreshold = exServer1.LatencyRedThreshold });
							totalWait += exServer1.LatencyRedThreshold + 1;
						}
					}
					TestResults AllTestResults = new TestResults();
					if (myExchangeMailProbes.Count > 0)
					{
						MailFlowThread = new Thread(() =>
						{
							MonitoredItems.ExchangeServer thisServer = myExchangeMailProbes.get_Item(0);
							ExchangeMailFlow mailFlowTest = new ExchangeMailFlow();
							mailFlowTest.PrereqForWindows(thisServer, ref AllTestResults, mailFlow);

							DB.UpdateAllTests(AllTestResults, thisServer, thisServer.ServerType);
						});
						MailFlowThread.CurrentCulture = c;
						MailFlowThread.IsBackground = true;
						MailFlowThread.Priority = ThreadPriority.Normal;
						MailFlowThread.Name = "MailFlowWorkerThread";
						MailFlowThread.Start();

					}
				}
				catch (Exception ex)
				{
					Common.WriteDeviceHistoryEntry("Exchange", "HeatMap", "Error Getting heatMap Data: " + ex.Message.ToString());
				}
				//how do we get the scan interval??
				Common.WriteDeviceHistoryEntry("Exchange", "HeatMap", "Waiting for " + iInterval.ToString() + " seconds to restart the Loop ");
				Thread.Sleep(1000 * 60 * iInterval);

				//if, after it sleeps the interval the test is not hung in remove-psssession, will keep sleeping another 5 minutes
				int currSleepTimer = 0;
				while (MailFlowThread != null && MailFlowThread.IsAlive)
				{
					Common.WriteDeviceHistoryEntry("Exchange", "HeatMap", "Sleeping for another 5 minutes to wait for test, it is not hung that we know of. Will sleep a max of another scan interval length. ");
					Thread.Sleep(1000 * 60 * 5);
					currSleepTimer += 1000 * 60 * 5;
					if (currSleepTimer > totalWait)
						break;
				}

				
			}

		}
		public MonitoredItems.ExchangeServer SelectMailProbeServerToMonitor()
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

			try
			{
				ScanASAP = myRegistry.ReadFromRegistry("ScanExchangeASAP").ToString();
			}
			catch (Exception ex)
			{
				ScanASAP = "";
			}

			//Searches for the server marked as ScanASAP, if it exists
			for (int n = 0; n < myExchangeMailProbes.Count; n++)
			{
				ServerOne = myExchangeMailProbes.get_Item(n);
				if (ServerOne.Name == ScanASAP && ServerOne.IsBeingScanned == false && ServerOne.Enabled)
				{
					Common.WriteDeviceHistoryEntry("All", "Exchange", ScanASAP + " was marked 'Scan ASAP' so it will be scanned next.");
					myRegistry.WriteToRegistry("ScanExchangeASAP", "n/a");

					//ServerOne.ScanASAP = true;

					return ServerOne;
				}

			}


			//Searches for the first enounter of a Not Responding server that is due for a scan
			for (int n = 0; n < myExchangeMailProbes.Count; n++)
			{
				ServerOne = myExchangeMailProbes.get_Item(n);
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
			for (int n = 0; n < myExchangeMailProbes.Count; n++)
			{
				ServerOne = myExchangeMailProbes.get_Item(n);
                if ((ServerOne.Status == "Not Scanned" || ServerOne.Status == "Master Service Stopped." )&& ServerOne.IsBeingScanned == false && ServerOne.Enabled)
				{
					Common.WriteDeviceHistoryEntry("All", "Exchange", "Selecting " + ServerOne.Name + " because the status is " + ServerOne.Status + ".");
					return ServerOne;
				}
			}


			//Searches for all servers that are due for a scan
			List<MonitoredItems.ExchangeServer> ScanCanidates = new List<MonitoredItems.ExchangeServer>();

			foreach (MonitoredItems.ExchangeServer srv in myExchangeMailProbes)
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
		//mail flow

		#endregion
		

		#region DAG
		private void CreateExchangeDAGCollection()
		{
			//Fetch all servers
			if (myDagCollection == null)
				myDagCollection = new MonitoredItems.ExchangeServersCollection();
			MonitoredItems.ExchangeServersCollection newCollection = new MonitoredItems.ExchangeServersCollection();
			CommonDB DB = new CommonDB();
			StringBuilder SQL = new StringBuilder();
			SQL.Append(" select distinct sr.ID, Sr.ServerName, S.ServerType, L.Location, sa.ScanInterval, sa.RetryInterval, sa.OffHourInterval, ");
			SQL.Append(" sa.Enabled, sa.category, ds.ReplyQThreshold, ds.CopyQThreshold, (select IPAddress from Servers where Servers.ID=ds.PrimaryConnection) as PrimaryIPAddress, ");
			SQL.Append(" (select IPAddress from Servers where Servers.ID=ds.BackupConnection) as BackupIPAddress, ");
			SQL.Append(" crp.UserID as PrimaryUserID, crp.Password as PrimaryPassword, crb.UserID as BackupUserID, crb.Password as BackupPassword, st.LastUpdate, st.Status, st.StatusCode, di.CurrentNodeID, ");
			SQL.Append(" esp.AuthenticationType as PrimaryAuthenticationType, esb.AuthenticationType as BackupAuthenticationType ");
			SQL.Append(" from Servers Sr inner join ServerTypes S on Sr.ServerTypeID=S.ID inner join Locations L on Sr.LocationID = L.ID ");
			SQL.Append(" inner join DagSettings ds on ds.ServerID=sr.ID inner join ServerAttributes sa on sr.ID=sa.ServerID ");
			SQL.Append(" inner join ServerAttributes sap on ds.PrimaryConnection=sap.ServerID inner join credentials crp on sap.CredentialsId=crp.ID ");
			SQL.Append(" inner join ServerAttributes sab on ds.BackupConnection=sab.ServerID inner join credentials crb on sab.CredentialsId=crb.ID ");
			SQL.Append(" inner join ExchangeSettings esp on esp.ServerID=sap.ServerID ");
			SQL.Append(" inner join ExchangeSettings esb on esb.ServerID=sab.ServerID  ");
			if (ConfigurationManager.AppSettings["VSNodeName"] != null)
			{
				string NodeName = ConfigurationManager.AppSettings["VSNodeName"].ToString();
				SQL.Append(" inner join DeviceInventory di on Sr.ID=di.DeviceID and Sr.ServerTypeId=di.DeviceTypeId ");
				SQL.Append(" inner join Nodes on (Nodes.ID=di.CurrentNodeId or di.CurrentNodeId=-1) and Nodes.Name='" + NodeName + "' ");
			}
			SQL.Append(" left outer join Status st on st.Type=S.ServerType and st.Name=Sr.ServerName ");
			SQL.Append(" where sa.Enabled=1 ");

			DataTable dtServers = DB.GetData(SQL.ToString());

			listOfIdsForDag = String.Join(",", dtServers.AsEnumerable().Select(r => r.Field<Int32>("ID").ToString()).ToList());

			//Loop through servers
			if (dtServers.Rows.Count > 0)
			{
				for (int i = 0; i < dtServers.Rows.Count; i++)
				{
					DataRow DR = dtServers.Rows[i];
					MonitoredItems.ExchangeServer myExchangeServer = new MonitoredItems.ExchangeServer();
					newCollection.Add(SetDAGSettings(myExchangeServer, DR));
				}

				int updatedServers = 0;
				int newServers = 0;
				int removedServers = 0;

				//Removes servers not in the new lsit
				foreach (MonitoredItems.ExchangeServer server in myDagCollection)
				{
					string currName = server.Name;
					try
					{
						MonitoredItems.ExchangeServer newServer = newCollection.SearchByName(currName);
						if (newServer == null)
						{
							//incase it doesnt throw and exception, if not found, removed server from list
							if (server.Enabled)
								myDagCollection.Delete(currName);
							removedServers++;
						}
					}
					catch (Exception ex)
					{
						//server not found
						if (server.Enabled)
							myDagCollection.Delete(currName);
						removedServers++;
					}
				}

				// adds/updates new servers
				foreach (MonitoredItems.ExchangeServer server in newCollection)
				{
					string currName = server.Name;
					try
					{
						MonitoredItems.ExchangeServer oldServer = myDagCollection.SearchByName(currName);

						if (oldServer != null)
						{

							oldServer.Name = server.Name;
							oldServer.Location = server.Location;
							oldServer.ScanInterval = server.ScanInterval;
							oldServer.OffHoursScanInterval = server.OffHoursScanInterval;
							oldServer.RetryInterval = server.RetryInterval;
							oldServer.LastScan = server.LastScan;

							oldServer.DAGPrimaryIPAddress = server.DAGPrimaryIPAddress;
							oldServer.DAGPrimaryPassword = server.DAGPrimaryPassword;
							oldServer.DAGPrimaryUserName = server.DAGPrimaryUserName;

							oldServer.DAGBackupIPAddress = server.DAGBackupIPAddress;
							oldServer.DAGBackupPassword = server.DAGBackupPassword;
							oldServer.DAGBackupUserName = server.DAGBackupUserName;

							if (oldServer.Enabled == false)
								oldServer.Enabled = true;
							updatedServers++;
							
						}
						else
						{
							myDagCollection.Add(server);
							newServers++;
						}
					}
					catch (NullReferenceException ex)
					{
						myDagCollection.Add(server);
						newServers++;
					}

				}
				//myExchangeServers = newCollection;
				/**********************************************************/
				Common.WriteDeviceHistoryEntry("All", "Exchange", "There are " + myDagCollection.Count + " servers in the collection.  " + newServers + " were new and " + updatedServers + " were updated.");
			}
			else
			{
				myDagCollection = new MonitoredItems.ExchangeServersCollection();
			}

			Common.InsertInsufficentLicenses(myDagCollection);

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
			MyExchangeServer.LastScan = DR["LastUpdate"] == null || DR["LastUpdate"].ToString() == "" ? DateTime.Now : DateTime.Parse(DR["LastUpdate"].ToString());
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
						thisServer = SelectDAGServerToMonitor();
				
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

					if(thisServer != null)
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
