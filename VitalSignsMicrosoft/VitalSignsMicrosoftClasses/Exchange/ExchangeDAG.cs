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

using VSFramework;
using System;
using MongoDB.Driver;

namespace VitalSignsMicrosoftClasses
{
	class ExchangeDAG
    {
	   // string sqlcon = "";
	   //SqlConnection con;
       VSFramework.VSAdaptor myAdapter = new VSFramework.VSAdaptor();
       VSFramework.XMLOperation myxmlAdapter = new VSFramework.XMLOperation();


       public ExchangeDAG()
        { 
            //InitializeComponent();
			//sqlcon = myxmlAdapter.GetDBConnectionString("VitalSigns");
			//con = new SqlConnection(sqlcon);
        }

       public void CheckServer(MonitoredItems.ExchangeServer myServer, ref TestResults AllTestResults)
	   {

		   RemoveOldDAGs(myServer, AllTestResults);

		   string cmdlets = "-CommandName Get-DatabaseAvailabilityGroup, Test-ReplicationHealth, Get-MailboxDatabase, Get-MailboxDatabaseCopyStatus";
		   ReturnPowerShellObjects results = Common.PrereqForExchangeWithCmdlets(myServer.Name, myServer.DAGPrimaryUserName, myServer.DAGPrimaryPassword, myServer.ServerType, myServer.DAGPrimaryIPAddress, commonEnums.ServerRoles.Empty, cmdlets, myServer.DAGPrimaryAuthenticationType);
		   string IPAddress = myServer.DAGPrimaryIPAddress;
		   if (results.Connected == false)
		   {
			   Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "Unable to connect to primary server.  Will attempt backup",commonEnums.ServerRoles.Empty,Common.LogLevel.Normal);
			   results.Dispose();
			   results = Common.PrereqForExchangeWithCmdlets(myServer.Name, myServer.DAGBackupUserName, myServer.DAGBackupPassword, myServer.ServerType, myServer.DAGBackupIPAddress, commonEnums.ServerRoles.Empty, cmdlets, myServer.DAGBackupAuthenticationType);
			   IPAddress = myServer.DAGBackupIPAddress;
		   }
		   if (results.Connected == false)
		   {
			   Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "Unable to connect to Backup server.  Will stop current scan", commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);

               MongoStatementsUpdate<VSNext.Mongo.Entities.Status> mongoStatement = new MongoStatementsUpdate<VSNext.Mongo.Entities.Status>();
               mongoStatement.filterDef = mongoStatement.repo.Filter.Where(i => i.TypeAndName == myServer.TypeANDName);
               mongoStatement.updateDef = mongoStatement.repo.Updater
                   .Set(i => i.MailboxCount, 0)
                   .Set(i => i.DatabaseCount, 0)
                   .Set(i => i.FileWitnessSereverName, "")
                   .Set(i => i.FileWitnessSereverStatus, "");
               AllTestResults.MongoEntity.Add(mongoStatement);


               Common.makeAlert(false, myServer, commonEnums.AlertType.Not_Responding, ref AllTestResults, "DAG");
			   results.Dispose();
			   return;
		   }
		   else
		   {
               MongoStatementsUpdate<VSNext.Mongo.Entities.Status> mongoStatement = new MongoStatementsUpdate<VSNext.Mongo.Entities.Status>();
               mongoStatement.filterDef = mongoStatement.repo.Filter.Where(i => i.TypeAndName == myServer.TypeANDName);
               mongoStatement.updateDef = mongoStatement.repo.Updater
                   .Set(i => i.MailboxCount, 0)
                   .Set(i => i.DatabaseCount, 0)
                   .Set(i => i.FileWitnessSereverName, "")
                   .Set(i => i.FileWitnessSereverStatus, "");
               AllTestResults.MongoEntity.Add(mongoStatement);


		   }

           Common.makeAlert(true, myServer, commonEnums.AlertType.Not_Responding, ref AllTestResults, "DAG");

		   using (results)
		   {

			   results.PS.Streams.Error.Clear();
			   results.PS.Commands.Clear();
			   results.PS.Commands.Clear();
			   results.PS.Streams.Error.Clear();
			   string strDAGName = myServer.Name;//GetDAGName(powerShellObjects.PS, powerShellObjects.runspace, myServer.Name, myServer.IPAddress, ref  AllTestResults);
			   string FileWitnessServer = GetDAGWitnessServer(results.PS, ref  AllTestResults, myServer);
			   Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "Finished getting the DAG witness Server", commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);
			   //Get Dagname for other functions.
			   results.PS.Commands.Clear();
			   results.PS.Streams.Error.Clear();

			   checkFileWitnessServer(results.PS, ref  AllTestResults, myServer, FileWitnessServer);
			   Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "Finished checking the DAG witness Server", commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);
			   //Get Dagname for other functions.
			   results.PS.Commands.Clear();
			   results.PS.Streams.Error.Clear();

			   DAGHealthMemberReport(results.PS, ref AllTestResults, myServer);
			   Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "Finished getting all the DAG members", commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);
			   results.PS.Commands.Clear();
			   results.PS.Streams.Error.Clear();
			   DAGHealthCopyStatus(results.PS, ref  AllTestResults, myServer);
			   Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "Finished getting the DAG database information", commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);
			   results.PS.Commands.Clear();
			   results.PS.Streams.Error.Clear();
			   DAGActionPreferences(results.PS, ref  AllTestResults, myServer);
			   Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "Finished getting the DAG Action Preferences", commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);

			   results.PS.Commands.Clear();
			   results.PS.Streams.Error.Clear();
			   DAGBackupDetails(results.PS, ref  AllTestResults, myServer);
			   
			   results.PS.Commands.Clear();
			   
			   GC.Collect();

		   } 

		   //Common.WriteDeviceHistoryEntry("Exchange", myServer.Name, DateTime.Now.ToString() + " Ending thread for DAG.", Common.LogLevel.Normal);
        }

	   public string GetDAGWitnessServer(PowerShell powershell, ref TestResults AllTestsList, MonitoredItems.ExchangeServer myServer)
       {
		   Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "In GetDAGWitnessServer.", commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);
		  
           //AllTestsList.Add(new TestList() { Details = "Service answered with   at " + System.DateTime.Now.ToShortTimeString(), TestName = servername+ " " + strAction, Category = commonEnums.ServerRoles.CAS, Result = commonEnums.ServerResult.Pass });
           try
           {
               System.Collections.ObjectModel.Collection<PSObject> results = new System.Collections.ObjectModel.Collection<PSObject>();

			   String str = "Get-DatabaseAvailabilityGroup | Where{$_.Name -eq '" + myServer.Name + "'} | Select-Object -Property WitnessServer";
               powershell.Streams.Error.Clear();

               powershell.AddScript(str);
               results = powershell.Invoke();

               if (powershell.Streams.Error.Count > 51)
               {
                   foreach (ErrorRecord er in powershell.Streams.Error)
                       Console.WriteLine(er.ErrorDetails);
				   Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "GetDAGWitnessServer received over 51 errors", commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);

               }
               else
               {

                    foreach (ErrorRecord er in powershell.Streams.Error)
                        Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "GetDAGWitnessServer errors: " + er.ToString(), commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);

                    Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "GetDAGWitnessServer output results: " + results.Count.ToString(), commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);

                   foreach (PSObject ps in results)
                   {

                       string strWitnessServer = ps.Properties["WitnessServer"].Value == null ? "" : ps.Properties["WitnessServer"].Value.ToString();

                       MongoStatementsUpdate<VSNext.Mongo.Entities.Status> mongoStatement = new MongoStatementsUpdate<VSNext.Mongo.Entities.Status>();
                       mongoStatement.filterDef = mongoStatement.repo.Filter.Where(i => i.TypeAndName == myServer.TypeANDName);
                       mongoStatement.updateDef = mongoStatement.repo.Updater.Set(i => i.FileWitnessSereverName, strWitnessServer);
                       AllTestsList.MongoEntity.Add(mongoStatement);

					   return strWitnessServer;
                   }
               }

           }
           catch (Exception ex)
           {

			   Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "Error in GetDAGWitnessServer: " + ex.Message, commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);

           }
           finally
           {
               // dispose the runspace and enable garbage collection
               //runspace.Dispose();
               //runspace = null;
           }
		   return "";
       }

	   public void checkFileWitnessServer(PowerShell powershell, ref TestResults AllTestsList, MonitoredItems.ExchangeServer myServer, string WitnessServer)
	   {
		   Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "In checkFileWitnessServer.", commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);

		   try
		   {
			   System.Collections.ObjectModel.Collection<PSObject> results = new System.Collections.ObjectModel.Collection<PSObject>();

			   String str = "Test-Connection " + WitnessServer + " -Count 1";
			   powershell.Streams.Error.Clear();

			   powershell.AddScript(str);
			   results = powershell.Invoke();

			   if (powershell.Streams.Error.Count > 51)
			   {
				   foreach (ErrorRecord er in powershell.Streams.Error)
					   Console.WriteLine(er.ErrorDetails);
				   Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "checkFileWitnessServer received over 51 errors", commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);
			   }
			   else
			   {
				   Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "checkFileWitnessServer output results: " + results.Count.ToString(), commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);

				   if (results.Count == 1)
				   {
                       MongoStatementsUpdate<VSNext.Mongo.Entities.Status> mongoStatement = new MongoStatementsUpdate<VSNext.Mongo.Entities.Status>();
                       mongoStatement.filterDef = mongoStatement.repo.Filter.Where(i => i.TypeAndName == myServer.TypeANDName);
                       mongoStatement.updateDef = mongoStatement.repo.Updater.Set(i => i.FileWitnessSereverStatus, "OK");
                       AllTestsList.MongoEntity.Add(mongoStatement);
				   }
				   else
				   {
                       MongoStatementsUpdate<VSNext.Mongo.Entities.Status> mongoStatement = new MongoStatementsUpdate<VSNext.Mongo.Entities.Status>();
                       mongoStatement.filterDef = mongoStatement.repo.Filter.Where(i => i.TypeAndName == myServer.TypeANDName);
                       mongoStatement.updateDef = mongoStatement.repo.Updater.Set(i => i.FileWitnessSereverStatus, "Not Responding");
                       AllTestsList.MongoEntity.Add(mongoStatement);
				   }
			   }

		   }
		   catch (Exception ex)
		   {

			   Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "Error in checkFileWitnessServer: " + ex.Message, commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);

		   }
	   }

       public void DAGHealthMemberReport(PowerShell powershell, ref TestResults AllTestsList, MonitoredItems.ExchangeServer myServer)
       {
		   Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "In DAGHealthMemberReport.", commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);

		   string serverErrors = "";
		   string testErrors = "";
		   int countErrors = 0;
		   string DAGName = myServer.Name;
		   try
		   {

			   System.Collections.ObjectModel.Collection<PSObject> results = new System.Collections.ObjectModel.Collection<PSObject>();
			   //Change the Path to the Script to suit your needs
			   System.IO.StreamReader sr = new System.IO.StreamReader(AppDomain.CurrentDomain.BaseDirectory.ToString() + "Scripts\\EX_DAGHealth_MemberReport.ps1");
			   String str = "$dagname='" + DAGName + "'\n\n" + sr.ReadToEnd();
			   powershell.Commands.Clear();
			   powershell.Streams.ClearStreams();
			   powershell.AddScript(str);
			   results = powershell.Invoke();

			   if (powershell.Streams.Error.Count > 51)
			   {
				   foreach (ErrorRecord er in powershell.Streams.Error)
					   Console.WriteLine(er.ErrorDetails);
				   Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "DAGHealthMemberReport received over 51 errors", commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);
				   //AllTestsList.StatusDetails.Add(new TestList() { TestName="DAG", Details = "DAG Errors:Error count>51 at " + System.DateTime.Now.ToShortTimeString(), Result = commonEnums.ServerResult.Fail });
				   Common.makeAlert(false, myServer, commonEnums.AlertType.DAG_Member_Health, ref AllTestsList, "DAG Errors:Error count>51 at " + System.DateTime.Now.ToShortTimeString(), "Member Report");
			   }
			   else
			   {
				   Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "EX_DAGHealth_MemberReport output results: " + results.Count.ToString(), commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);
                   List<VSNext.Mongo.Entities.DagServers> listOfDagTests = new List<VSNext.Mongo.Entities.DagServers>();
				   foreach (PSObject ps in results)
				   {

					   string strServer = ps.Properties["Server"].Value == null ? "" : ps.Properties["Server"].Value.ToString();
					   string strClusterService = (ps.Properties["ClusterService"] == null || ps.Properties["ClusterService"].Value == null) ? "Fail" : ps.Properties["ClusterService"].Value.ToString();
					   string strReplayService = (ps.Properties["ReplayService"] == null || ps.Properties["ReplayService"].Value == null) ? "Fail" : ps.Properties["ReplayService"].Value.ToString();
					   string strActiveManager = (ps.Properties["ActiveManager"] == null || ps.Properties["ActiveManager"].Value == null) ? "Fail" : ps.Properties["ActiveManager"].Value.ToString();
					   string strTasksRpcListener = (ps.Properties["TasksRpcListener"] == null || ps.Properties["TasksRpcListener"].Value == null) ? "Fail" : ps.Properties["TasksRpcListener"].Value.ToString();
					   string strTcpListener = (ps.Properties["TcpListener"] == null || ps.Properties["TcpListener"].Value == null) ? "Fail" : ps.Properties["TcpListener"].Value.ToString();
					   string strDagMembersUp = (ps.Properties["DagMembersUp"] == null || ps.Properties["DagMembersUp"].Value == null) ? "Fail" : ps.Properties["DagMembersUp"].Value.ToString();
					   string strClusterNetwork = (ps.Properties["ClusterNetwork"] == null || ps.Properties["ClusterNetwork"].Value == null) ? "Fail" : ps.Properties["ClusterNetwork"].Value.ToString();
					   string strQuorumGroup = (ps.Properties["QuorumGroup"] == null || ps.Properties["QuorumGroup"].Value == null) ? "Fail" : ps.Properties["QuorumGroup"].Value.ToString();
					   string strFileShareQuorum = (ps.Properties["FileShareQuorum"] == null || ps.Properties["FileShareQuorum"].Value == null) ? "Fail" : ps.Properties["FileShareQuorum"].Value.ToString();
					   string strDBCopySuspended = (ps.Properties["DBCopySuspended"] == null || ps.Properties["DBCopySuspended"].Value == null) ? "Fail" : ps.Properties["DBCopySuspended"].Value.ToString();
					   string strDBDisconnected = (ps.Properties["DBDisconnected"] == null || ps.Properties["DBDisconnected"].Value == null) ? "Fail" : ps.Properties["DBDisconnected"].Value.ToString();
					   string strDBLogCopyKeepingUp = (ps.Properties["DBLogCopyKeepingUp"] == null || ps.Properties["DBLogCopyKeepingUp"].Value == null) ? "Fail" : ps.Properties["DBLogCopyKeepingUp"].Value.ToString();
					   string strDBLogReplayKeepingUp = (ps.Properties["DBLogReplayKeepingUp"] == null || ps.Properties["DBLogReplayKeepingUp"].Value == null) ? "Fail" : ps.Properties["DBLogReplayKeepingUp"].Value.ToString();

					   if (strClusterService == "Fail")
					   {
						   countErrors++;
						   serverErrors += strServer + ",";
						   testErrors += "Cluster Service,";
					   }
					   if (strReplayService == "Fail")
					   {
						   countErrors++;
						   serverErrors += strServer + ",";
						   testErrors += "Replay Service,";
					   }
					   if (strActiveManager == "Fail")
					   {
						   countErrors++;
						   serverErrors += strServer + ",";
						   testErrors += "Active Manager,";
					   }
					   if (strTasksRpcListener == "Fail")
					   {
						   countErrors++;
						   serverErrors += strServer + ",";
						   testErrors += "Tasks RPC Listener,";
					   }
					   if (strTcpListener == "Fail")
					   {
						   countErrors++;
						   serverErrors += strServer + ",";
						   testErrors += "TCP Listener,";
					   }
					   if (strDagMembersUp == "Fail")
					   {
						   countErrors++;
						   serverErrors += strServer + ",";
						   testErrors += "DAG Memebers Up,";
					   }
					   if (strClusterNetwork == "Fail")
					   {
						   countErrors++;
						   serverErrors += strServer + ",";
						   testErrors += "Cluster Network,";
					   }
					   if (strQuorumGroup == "Fail")
					   {
						   countErrors++;
						   serverErrors += strServer + ",";
						   testErrors += "Quorum Group,";
					   }
					   if (strFileShareQuorum == "Fail")
					   {
						   countErrors++;
						   serverErrors += strServer + ",";
						   testErrors += "File Share Quorum,";
					   }
					   if (strDBCopySuspended == "Fail")
					   {
						   countErrors++;
						   serverErrors += strServer + ",";
						   testErrors += "DB Copy Suspend,";
					   }
					   if (strDBDisconnected == "Fail")
					   {
						   countErrors++;
						   serverErrors += strServer + ",";
						   testErrors += "DB Disconnected,";
					   }
					   if (strDBLogCopyKeepingUp == "Fail")
					   {
						   countErrors++;
						   serverErrors += strServer + ",";
						   testErrors += "DB Log Copy Keeping Up,";
					   }
					   if (strDBLogReplayKeepingUp == "Fail")
					   {
						   countErrors++;
						   serverErrors += strServer + ",";
						   testErrors += "DB Log Replay Keeping Up,";
					   }

                       listOfDagTests.Add(new VSNext.Mongo.Entities.DagServers()
                       {
                           DAGServerName = strServer,
                           ClusterServiceTest = strClusterService,
                           ReplayServiceTest = strReplayService,
                           ActiveManagerTest = strActiveManager,
                           TasksRPCListenerTest = strTasksRpcListener,
                           TCPListenerTest = strTcpListener,
                           DAGMembersUp = strDagMembersUp,
                           ClusterNetworkTest = strClusterNetwork,
                           QuorumGroupTest = strQuorumGroup,
                           FileShareQuorumTest = strFileShareQuorum,
                           DBCopySuspendTest = strDBCopySuspended,
                           DBDisconnectedTest = strDBDisconnected,
                           DBLogCopyKeepingUpTest = strDBLogCopyKeepingUp,
                           DBLogReplayKeepingUpTest = strDBLogReplayKeepingUp
                       });

                       

				   }

                   MongoStatementsUpdate<VSNext.Mongo.Entities.Status> mongoStatement = new MongoStatementsUpdate<VSNext.Mongo.Entities.Status>();
                   mongoStatement.filterDef = mongoStatement.repo.Filter.Where(i => i.TypeAndName == myServer.TypeANDName);
                   mongoStatement.updateDef = mongoStatement.repo.Updater
                       .Set(i => i.DagServers, listOfDagTests);
                   AllTestsList.MongoEntity.Add(mongoStatement);

			   }

		   }
		   catch (Exception ex)
		   {

			   Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "Error in DAGHealthMemberReport: " + ex.Message, commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);

		   }
       


		   try
		   {
			   if (countErrors == 0)
			   {
				   Common.makeAlert(true, myServer, commonEnums.AlertType.DAG_Member_Health, ref AllTestsList, "No errors were found for Member Report.", "Member Report");
			   }
			   else
			   {
				   string strTestErrors = testErrors.Remove(testErrors.Length - 1);
				   string strServerErros = serverErrors.Remove(serverErrors.Length - 1);
				   if (countErrors == 1)
				   {
					   Common.makeAlert(false, myServer, commonEnums.AlertType.DAG_Member_Health, ref AllTestsList, "The test " + strTestErrors + " failed on server " + strServerErros + ".", "Member Report");
				   }
				   else if (new HashSet<string>(serverErrors.Split(',').ToArray()).Count == 1)
				   {
					   Common.makeAlert(false, myServer, commonEnums.AlertType.DAG_Member_Health, ref AllTestsList, "Multiple tests failed on server " + strServerErros.Substring(0, strServerErros.IndexOf(',')) + ".", "Member Report");
				   }
				   else
				   {
					   Common.makeAlert(false, myServer, commonEnums.AlertType.DAG_Member_Health, ref AllTestsList, "More than one test failed for this Database Availability Group.", "Member Report");
				   }
			   }
		   }
		   catch (Exception ex)
		   {
			   Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "Error making alert for DAG Member Report. Error :" + ex.Message, commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);
		   }
       }

       public void DAGHealthCopyStatus(PowerShell powershell, ref TestResults AllTestsList, MonitoredItems.ExchangeServer myServer)
       {
		   int replyThresh;
		   int copyThresh;
		   Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "In DAGHealthCopyStatus.", commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);

		   //string SqlStr = "select ServerName, DatabaseName, ReplayQueueThreshold, CopyQueueThreshold from DAGQueueThresholds where DAGName='" + myServer.Name + "'";
		  // CommonDB db = new CommonDB();
		   //DataTable dt = db.GetData(SqlStr);

            


		   string serverErrors = "";
		   string replyErrors = "";
		   string copyErrors = "";
		   string DAGName = myServer.Name;
		   try
		   {
			   System.Collections.ObjectModel.Collection<PSObject> results = new System.Collections.ObjectModel.Collection<PSObject>();
			   //Change the Path to the Script to suit your needs
			   System.IO.StreamReader sr = new System.IO.StreamReader(AppDomain.CurrentDomain.BaseDirectory.ToString() + "Scripts\\EX_DAGHealth_CopyStatus.ps1");
			   String str = "$dagname='" + DAGName + "'\n\n" + sr.ReadToEnd();
			   powershell.Commands.Clear();
			   powershell.Streams.ClearStreams();
			   powershell.AddScript(str);
			   results = powershell.Invoke();

			   if (powershell.Streams.Error.Count > 51)
			   {
				   foreach (ErrorRecord er in powershell.Streams.Error)
					   Console.WriteLine(er.ErrorDetails);
				   Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "DAGHealthCopyStatus received over 51 errors", commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);

				   Common.makeAlert(false, myServer, commonEnums.AlertType.DAG_Database_Health, ref AllTestsList, "DAG Errors:Error count>51 at " + System.DateTime.Now.ToShortTimeString(), "Database");
			   }
			   else
			   {
				   Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "EX_DAGHealth_CopyStatus output results: " + results.Count.ToString(), commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);
                   List<VSNext.Mongo.Entities.DagServerDatabases> listOfDagEntities = new List<VSNext.Mongo.Entities.DagServerDatabases>();

				   foreach (PSObject ps in results)
				   {

					   string strServer = ps.Properties["MailboxServer"].Value == null ? "" : ps.Properties["MailboxServer"].Value.ToString();

					   string strDatabaseName = ps.Properties["DatabaseName"].Value == null ? "" : ps.Properties["DatabaseName"].Value.ToString();
					   //string strActivationPreference = ps.Properties["ActivationPreference"].Value == null ? "" : ps.Properties["ActivationPreference"].Value.ToString();
					   string strCopyQueue = ps.Properties["CopyQueueLength"].Value == null ? "0" : ps.Properties["CopyQueueLength"].Value.ToString();
					   string strReplayQueue = ps.Properties["ReplayQueueLength"].Value == null ? "0" : ps.Properties["ReplayQueueLength"].Value.ToString();
					   string strReplayLagged = ps.Properties["ReplayLagged"].Value == null ? "" : ps.Properties["ReplayLagged"].Value.ToString();
					   string strTruncationLagged = ps.Properties["TruncationLagged"].Value == null ? "" : ps.Properties["TruncationLagged"].Value.ToString();
					   string strContendIndex = ps.Properties["Content Index"].Value == null ? "" : ps.Properties["Content Index"].Value.ToString();

                       listOfDagEntities.Add(new VSNext.Mongo.Entities.DagServerDatabases()
                       {
                           DatabaseName = strDatabaseName,
                           CopyQueue = int.Parse(strCopyQueue),
                           ReplayQueue = int.Parse(strReplayQueue),
                           ReplayLagged = strReplayLagged,
                           TruncationLagged = strTruncationLagged,
                           ContendIndex = strContendIndex,
                           ServerName = strServer
                       });

					   if (strContendIndex != "Healthy")
					   {
						   serverErrors += strDatabaseName + ",";
					   }

                        DataRow[] rows = new DataRow[0];// dt.Select("(ServerName='" + strServer + "' or ServerName='AllDatabases') AND (DatabaseName='" + strDatabaseName + "' or DatabaseName='AllDatabases')");


                        string allDatabases = "AllDatabases";
					   if (myServer.DagDatabaseSettings.Exists(x => (x.ServerName == strServer || x.ServerName == allDatabases) && (x.DatabaseName == strDatabaseName || x.DatabaseName == allDatabases)))
					   {
                            MonitoredItems.ExchangeServer.DagDatabaseSetting dagSetting = myServer.DagDatabaseSettings.Find(x => (x.ServerName == strServer || x.ServerName == allDatabases) && (x.DatabaseName == strDatabaseName || x.DatabaseName == allDatabases));

                           copyThresh = dagSetting.CopyQueueThreshold;
						   replyThresh = dagSetting.ReplayQueueThreshold;
						   if (strCopyQueue != "")
							   if (Convert.ToDouble(strCopyQueue) > copyThresh && copyThresh != 0)
								   copyErrors += strDatabaseName + ",";
						   if (strReplayQueue != "")
							   if (Convert.ToDouble(strReplayQueue) > replyThresh && replyThresh != 0)
								   replyErrors += strDatabaseName + ",";
					   }
				   }

                   MongoStatementsUpdate<VSNext.Mongo.Entities.Status> mongoStatement = new MongoStatementsUpdate<VSNext.Mongo.Entities.Status>();
                   mongoStatement.filterDef = mongoStatement.repo.Filter.Where(i => i.TypeAndName == myServer.TypeANDName);
                   mongoStatement.updateDef = mongoStatement.repo.Updater
                       .Set(i => i.DagServerDatabases, listOfDagEntities);
                   AllTestsList.MongoEntity.Add(mongoStatement);

			   }

		   }
		   catch (Exception ex)
		   {

			   Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "Error in DAGHealthCopyStatus: " + ex.Message, commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);
		   }

			try
			{
				if (serverErrors != "")
				{
					serverErrors = serverErrors.Remove(serverErrors.Length - 1);
					if (serverErrors.Contains(','))
					{
						Common.makeAlert(false, myServer, commonEnums.AlertType.DAG_Database_Health, ref AllTestsList, "Multiple databases(" + serverErrors + ") are marked as unhealthy.", "Database");
					}
					else
					{
						Common.makeAlert(false, myServer, commonEnums.AlertType.DAG_Database_Health, ref AllTestsList, "The database " + serverErrors.Replace(",", "") + " is marked as unhealthy.", "Database");
					}
				}
				else
				{
					Common.makeAlert(true, myServer, commonEnums.AlertType.DAG_Database_Health, ref AllTestsList, "All databases are marked as Healthy.", "Database");
				}

				if (replyErrors != "")
				{
					if (replyErrors.Contains(','))
					{
						   
						Common.makeAlert(false, myServer, commonEnums.AlertType.DAG_Replay_Queue, ref AllTestsList, "Multiple databases(" + serverErrors + ") are over their replay queue limits.", "Database");
					}
					else
					{
						Common.makeAlert(false, myServer, commonEnums.AlertType.DAG_Replay_Queue, ref AllTestsList, "The database " + serverErrors.Replace(",", "") + " is over the replay queue limit.", "Database");
					}
				}
				else
				{
					Common.makeAlert(true, myServer, commonEnums.AlertType.DAG_Replay_Queue, ref AllTestsList, "All databases are with in their replay limits.", "Database");
				}

				if (copyErrors != "")
				{
					if (copyErrors.Contains(','))
					{
						Common.makeAlert(false, myServer, commonEnums.AlertType.DAG_Copy_Queue, ref AllTestsList, "Multiple databases(" + serverErrors + ") are over their copy queue limits.", "Database");
					}
					else
					{
						Common.makeAlert(false, myServer, commonEnums.AlertType.DAG_Copy_Queue, ref AllTestsList, "The database " + serverErrors.Replace(",", "") + " is over the copy queue limit.", "Database");
					}
				}
				else
				{
					Common.makeAlert(true, myServer, commonEnums.AlertType.DAG_Copy_Queue, ref AllTestsList, "All databases are with in their copy limits.", "Database");
				}

			}
			catch (Exception ex)
			{
				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "Error making alert for DAG Database Health. Error :" + ex.Message, commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);
			}
       }

	   public void DAGActionPreferences(PowerShell powershell, ref TestResults AllTestsList, MonitoredItems.ExchangeServer myServer)
	   {

		   string serverErrors = "";
		   string DAGErrors = "";

		   try
		   {
			   System.Collections.ObjectModel.Collection<PSObject> results = new System.Collections.ObjectModel.Collection<PSObject>();

			   string DAGName = myServer.Name;

			   string str = "$dagname='" + DAGName + "' \n" +
						   "Get-MailboxDatabase -IncludePreExchange" + myServer.VersionNo + " -status | Where-Object {$_.MasterServerOrAvailabilityGroup -like $dagname} | Foreach-Object{New-Object PSObject -Property @{ \n" +
						   "Name=$_.Name \n" +
						   "activationpreference=$_.activationpreference \n" +
						   "mountedonserver=$_.mountedonserver \n" +
						   "}}";


			   powershell.Commands.Clear();
			   powershell.Streams.ClearStreams();
			   powershell.AddScript(str);
			   results = powershell.Invoke();

			   if (powershell.Streams.Error.Count > 51)
			   {
				   foreach (ErrorRecord er in powershell.Streams.Error)
					   Console.WriteLine(er.ErrorDetails);
				   Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "DAGActivationPref&mounted received over 51 errors", commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);
				   //AllTestsList.StatusDetails.Add(new TestList() { TestName = "DAG", Details = "DAG Errors:Error count>51 at " + System.DateTime.Now.ToShortTimeString(), Result = commonEnums.ServerResult.Fail });
				   //success = false;
				   Common.makeAlert(false, myServer, commonEnums.AlertType.DAG_Activation_Preference, ref AllTestsList, "DAG Errors:Error count>51 at " + System.DateTime.Now.ToShortTimeString(), "Activation Preference");
			   }
			   else
			   {
				   Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "EX_DAG_activationPref&mounted output results: " + results.Count.ToString(), commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);

				   foreach (PSObject ps in results)
				   {
					   string name = ps.Properties["Name"].Value.ToString();
					   string tempActivation2 = ps.Properties["activationpreference"].Value.ToString().Replace(" ", "");
					   string[] list = tempActivation2.Split(new char[] { '[', ']' });
					   System.Collections.ArrayList arrString = new System.Collections.ArrayList();
					   System.Collections.ArrayList arrInt = new System.Collections.ArrayList();
					   foreach (string s in list)
					   {
						   if (s != null && s != "")
						   {
							   string p = s.Substring(0, s.IndexOf(','));
							   arrString.Add(s.Substring(0, s.IndexOf(',')));
							   p = s.Substring(s.IndexOf(',') + 1);
							   arrInt.Add(s.Substring(s.IndexOf(',') + 1));
						   }
					   }
					   string moutnedOn = ps.Properties["mountedonserver"].Value.ToString();

					   for (int i = 0; i < arrString.Count; i++)
					   {
						   string isActive;
						   if (moutnedOn.Contains(arrString[i].ToString()))
							   isActive = "Active";
						   else
							   isActive = "Passive";

                           MongoStatementsUpdate<VSNext.Mongo.Entities.Status> mongoStatement = new MongoStatementsUpdate<VSNext.Mongo.Entities.Status>();
                           mongoStatement.filterDef = mongoStatement.repo.Filter.Where(p => p.TypeAndName == myServer.TypeANDName) &
                               mongoStatement.repo.Filter.ElemMatch("dag_server_databases", mongoStatement.repo.Filter.Eq("database_name", name) & 
                               mongoStatement.repo.Filter.Eq("server_name", arrString[i].ToString()));
                               
                           mongoStatement.updateDef = mongoStatement.repo.Updater
                               .Set(p => p.DagServerDatabases[-1].ActionPreference, Convert.ToInt32(arrInt[i].ToString()))
                               .Set(p => p.DagServerDatabases[-1].IsActive, isActive);

                           AllTestsList.MongoEntity.Add(mongoStatement);

						   if (arrInt[i].ToString() == "1")
						   {
							   if (isActive != "Active")
							   {
								   serverErrors += arrString[i].ToString() + ",";
								   DAGErrors += DAGName + ",";
							   }
						   }

					   }
				   }
			   }

		   }
		   catch (Exception ex)
		   {

			   Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "Error in DAGHealthCopyStatus: " + ex.Message, commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);
			   //success = false;

		   }
		   finally
		   {
		   }

		   try
		   {
			   if (serverErrors != "")
			   {
				   if (serverErrors.Contains(','))
				   {
					   Common.makeAlert(false, myServer, commonEnums.AlertType.DAG_Activation_Preference, ref AllTestsList, "Multiple DAGs are not mounted on their first preference. ", "Activation Preference");
				   }
				   else
				   {
					   Common.makeAlert(false, myServer, commonEnums.AlertType.DAG_Activation_Preference, ref AllTestsList, DAGErrors.Replace(",", "") + " is not mounted on its first preference, server " + serverErrors.Replace(",", ""), "Activation Preference");
				   }
			   }
			   else
			   {
				   Common.makeAlert(true, myServer, commonEnums.AlertType.DAG_Activation_Preference, ref AllTestsList, "All DAGS are mounted on their first preference.", "Activation Preference");
			   }
		   }
		   catch (Exception ex)
		   {
			   Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "Error making alert for DAG Activation Preferences. Error :" + ex.Message, commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);
		   }

	   }

       

	   public void RemoveOldDAGs(MonitoredItems.ExchangeServer myServer, TestResults AllTestResults)
	   {

			

	   }

	   public void DAGBackupDetails(PowerShell powershell, ref TestResults AllTestsList, MonitoredItems.ExchangeServer myServer)
	   {
		   Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "in DAGBackupDetails", commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);

		   try
		   {
			   System.Collections.ObjectModel.Collection<PSObject> results = new System.Collections.ObjectModel.Collection<PSObject>();
			   //Change the Path to the Script to suit your needs
			   System.IO.StreamReader sr = new System.IO.StreamReader(AppDomain.CurrentDomain.BaseDirectory.ToString() + "Scripts\\EX_DAG_Database_Backups.ps1");
			   String str = "$dagname='" + myServer.Name + "'\n\n" + sr.ReadToEnd();
			   powershell.Commands.Clear();
			   powershell.Streams.ClearStreams();
			   powershell.AddScript(str);
			   results = powershell.Invoke();

			   if (powershell.Streams.Error.Count > 51)
			   {
				   foreach (ErrorRecord er in powershell.Streams.Error)
					   Console.WriteLine(er.ErrorDetails);
				   Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "DAGBackupDetails received over 51 errors", commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);
				   Common.makeAlert(false, myServer, commonEnums.AlertType.DAG_Database_Health, ref AllTestsList, "DAG Errors:Error count>51 at " + System.DateTime.Now.ToShortTimeString(), "Database");
			   }
			   else
			   {
				   Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "EX_DAG_Database_Backups output results: " + results.Count.ToString(), commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);
				   foreach (PSObject ps in results)
				   {

                       string strDatabase = ps.Properties["Name"].Value == null ? "" : ps.Properties["Name"].Value.ToString();
					   string strStorageGroup = ps.Properties["StorageGroup"].Value == null ? "" : ps.Properties["StorageGroup"].Value.ToString();
					   string strMounted = ps.Properties["Mounted"].Value == null ? "False" : ps.Properties["Mounted"].Value.ToString();
					   string strBackupInProgress = ps.Properties["BackupInProgress"].Value == null ? "False" : ps.Properties["BackupInProgress"].Value.ToString();
					   string strOnlineMaintenanceInProgress = ps.Properties["OnlineMaintenanceInProgress"].Value == null ? "False" : ps.Properties["OnlineMaintenanceInProgress"].Value.ToString();

					   //datetime stamp
					   string strLastFullBackup = ps.Properties["LastFullBackup"].Value == null ? null : "" + ps.Properties["LastFullBackup"].Value.ToString() + "";
					   string strLastIncrementalBackup = ps.Properties["LastIncrementalBackup"].Value == null ? null : "" + ps.Properties["LastIncrementalBackup"].Value.ToString() + "";
					   string strLastDifferentialBackup = ps.Properties["LastDifferentialBackup"].Value == null ? null : "" + ps.Properties["LastDifferentialBackup"].Value.ToString() + "";
					   string strLastCopyBackup = ps.Properties["LastCopyBackup"].Value == null ? null : "" + ps.Properties["LastCopyBackup"].Value.ToString() + "";

					   //days ago
					   string strLastFullBackupDaysAgo = ps.Properties["LastFullBackupDaysAgo"].Value == null ? "Never" : ps.Properties["LastFullBackupDaysAgo"].Value.ToString();
					   string strLastIncrementalBackupDaysAgo = ps.Properties["LastIncrementalBackupDaysAgo"].Value == null ? "Never" : ps.Properties["LastIncrementalBackupDaysAgo"].Value.ToString();
					   string strLastDifferentialBackupDaysAgo = ps.Properties["LastDifferentialBackupDaysAgo"].Value == null ? "Never" : ps.Properties["LastDifferentialBackupDaysAgo"].Value.ToString();
					   string strLastCopyBackupDaysAgo = ps.Properties["LastCopyBackupDaysAgo"].Value == null ? "Never" : ps.Properties["LastCopyBackupDaysAgo"].Value.ToString();

                       MongoStatementsUpdate<VSNext.Mongo.Entities.Status> mongoUpdate = new MongoStatementsUpdate<VSNext.Mongo.Entities.Status>();
                       mongoUpdate.filterDef = mongoUpdate.repo.Filter.Eq(i => i.TypeAndName, myServer.TypeANDName) 
                           & !mongoUpdate.repo.Filter.ElemMatch(i => i.DagDatabases, i => i.DatabaseName == strDatabase);
                       VSNext.Mongo.Entities.DagDatabases dbg = new VSNext.Mongo.Entities.DagDatabases()
                       {
                            DatabaseName = strDatabase
                       };
                       mongoUpdate.updateDef = mongoUpdate.repo.Updater.Push(i => i.DagDatabases, dbg);
                       AllTestsList.MongoEntity.Add(mongoUpdate);                       

                       mongoUpdate = new MongoStatementsUpdate<VSNext.Mongo.Entities.Status>();
                       mongoUpdate.filterDef = mongoUpdate.repo.Filter.Eq(i => i.DeviceName, myServer.Name) 
                           & mongoUpdate.repo.Filter.ElemMatch(i => i.DagDatabases, i => i.DatabaseName == strDatabase);
                       mongoUpdate.updateDef = mongoUpdate.repo.Updater
                           .Set(i => i.DagDatabases[-1].StorageGroup, strStorageGroup)
                           .Set(i => i.DagDatabases[-1].Mounted, Convert.ToBoolean(strMounted))
                           .Set(i => i.DagDatabases[-1].BackupInProgress, Convert.ToBoolean(strBackupInProgress))
                           .Set(i => i.DagDatabases[-1].OnlineMaintenanceInProgress, Convert.ToBoolean(strOnlineMaintenanceInProgress));
                       if(!string.IsNullOrWhiteSpace(strLastFullBackup))
                            mongoUpdate.updateDef.Set(i => i.DagDatabases[-1].LastFullBackupDate, string.IsNullOrWhiteSpace(strLastFullBackup) ?null : (DateTime?)Convert.ToDateTime(strLastFullBackup));
                       if (!string.IsNullOrWhiteSpace(strLastIncrementalBackup))
                           mongoUpdate.updateDef.Set(i => i.DagDatabases[-1].LastIncrementalBackupDate, string.IsNullOrWhiteSpace(strLastIncrementalBackup) ? null : (DateTime?)Convert.ToDateTime(strLastIncrementalBackup));
                       if(!string.IsNullOrWhiteSpace(strLastDifferentialBackup))
                            mongoUpdate.updateDef.Set(i => i.DagDatabases[-1].LastDifferentialBackupDate, string.IsNullOrWhiteSpace(strLastDifferentialBackup) ? null : (DateTime?)Convert.ToDateTime(strLastDifferentialBackup));
                       if(!string.IsNullOrWhiteSpace(strLastCopyBackup))
                            mongoUpdate.updateDef.Set(i => i.DagDatabases[-1].LastCopyBackupDate, string.IsNullOrWhiteSpace(strLastCopyBackup) ? null : (DateTime?)Convert.ToDateTime(strLastCopyBackup));

                       AllTestsList.MongoEntity.Add(mongoUpdate);
					   

				   }
			   }

		   }
		   catch (Exception ex)
		   {

			   Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "Error in DAGBackupDetails: " + ex.Message, commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);
			   //success = false;

		   }
		   finally
		   {

		   }
	   }


    }
}
