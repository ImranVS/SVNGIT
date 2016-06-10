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

namespace VitalSignsMicrosoftClasses
{
	class SharepointCommon
	{
		CultureInfo culture = CultureInfo.CurrentCulture;
		private Dictionary<string,string> getSpStatsList()
		{
			Dictionary<string,string> cmdList = new Dictionary<string,string>();
			cmdList.Add("\\SharePoint Foundation(_total)\\Sql Query Executing  time", "SQLQueryExeTime");
			cmdList.Add("\\SharePoint Foundation(_total)\\Executing Sql Queries", "ExeQueries");
			cmdList.Add("\\SharePoint Foundation(_total)\\Responded Page Requests Rate", "RPRR");
			cmdList.Add("\\SharePoint Foundation(_total)\\Executing Time/Page Request", "ExeTime");
			cmdList.Add("\\SharePoint Foundation(_total)\\Current Page Requests", "CurPageReq");
			cmdList.Add("\\SharePoint Foundation(_total)\\Reject Page Requests Rate", "RejPageReq");
			cmdList.Add("\\SharePoint Foundation(_total)\\Incoming Page Requests Rate","IncomingPageReq");
			cmdList.Add("\\SharePoint Foundation(_total)\\Active Threads","ActiveThreads");
			cmdList.Add("\\Web Service(_total)\\Total Connection Attempts (all instances)","WebServiceConnectionAttempts");
			cmdList.Add("\\ASP.NET\\Application Restarts", "ApplicationRestarts");
			cmdList.Add("\\ASP.NET\\Request Execution Time", "RequestExecutionTime");
			cmdList.Add("\\ASP.NET\\Requests Rejected", "RejectedRequests");
			cmdList.Add("\\ASP.NET\\Requests Queued", "QueuedRequests");
			cmdList.Add("\\ASP.NET\\Worker Process Restarts", "WorkerProcessRestarts");
			cmdList.Add("\\ASP.NET\\Request Wait Time", "RequestWaitTime");
			cmdList.Add("\\ASP.NET Applications(__total__)\\Requests/Sec", "RequestsPerSecond");
			cmdList.Add("\\ASP.NET Applications(__total__)\\requests total", "TotalRequests");   
			cmdList.Add("\\ASP.NET Applications(__total__)\\sessions active", "ActiveSessions");   
			cmdList.Add("\\ASP.NET Applications(__total__)\\requests rejected", "RequestsRejected");   
			cmdList.Add("\\Web Service(_total)\\Bytes Sent/sec", "TotalBytesSentPerSec");   
			cmdList.Add("\\Web Service(_total)\\Bytes Received/sec", "TotalBytesReceivedPerSec");  

			return cmdList;
		}
		private Dictionary<string, string> getSpStatsListForDatabase()
		{
			Dictionary<string, string> cmdList = new Dictionary<string, string>();
			cmdList.Add("\\Memory\\Available MBytes", "Mem_Available_MB");
			cmdList.Add("\\Memory\\% Committed Bytes In Use", "Mem_PercBytesInUse");
			cmdList.Add("\\Memory\\Page Faults/sec", "Mem_PageFaultsPerSec");
			cmdList.Add("\\Memory\\Pages Input/sec", "Mem_PagesInputsPerSec");
			cmdList.Add("\\Memory\\Pages/sec", "Mem_PagesPerSec");
			cmdList.Add("\\Memory\\Pool Nonpaged Bytes", "Mem_PoolNonpagedBytes");
			cmdList.Add("\\Network Interface(*)\\Bytes Total/sec", "Net_BytesTotalPerSec");
			cmdList.Add("\\Network Interface(*)\\Packets/sec", "Net_PacketsPerSec");
			cmdList.Add("\\PhysicalDisk(*)\\Current Disk Queue Length", "Phys_DiskQueueLength");
			cmdList.Add("\\PhysicalDisk(*)\\% Disk Time", "Phys_PercDiskTime");
			cmdList.Add("\\PhysicalDisk(*)\\Disk Read Bytes/sec", "Phys_DiskReadBytesPerSec"); 
			cmdList.Add("\\PhysicalDisk(*)\\Disk Write Bytes/sec", "Phys_DiskWriteBytesPerSec");
			cmdList.Add("\\PhysicalDisk(*)\\Avg. Disk sec/Transfer", "Phys_AvgDiskSecPerTransfer");
			cmdList.Add("\\Process(*)\\% Processor Time", "Proc_PercProcessorTime");
			cmdList.Add("\\Process(*)\\Page Faults/sec", "Proc_PageFaultsPerSec");
			cmdList.Add("\\Process(*)\\Page File Bytes Peak", "Proc_PageFileBytesPeak");
			cmdList.Add("\\Process(*)\\Page File Bytes", "Proc_PageFileBytes");
			cmdList.Add("\\Process(*)\\Private Bytes", "Proc_PrivateBytes");
			cmdList.Add("\\Process(*)\\Virtual Bytes Peak", "Proc_VirtualBytesPeak");
			cmdList.Add("\\Process(*)\\Virtual Bytes", "Proc_VirtualBytes");
			cmdList.Add("\\Process(*)\\Working Set Peak", "Proc_WorkingSetPeak");
			cmdList.Add("\\Process(*)\\Working Set", "Proc_WorkingSet");
			cmdList.Add("\\Processor(*)\\% Processor Time", "Proc_PercProcessorTime");
			cmdList.Add("\\Processor(*)\\Interrupts/sec", "Proc_InteruptsPerSec");
			cmdList.Add("\\Redirector\\Server Sessions Hung", "Redir_ServerSessionsHung");
			cmdList.Add("\\Server\\Work Item Shortages", "Server_WorkItemShortages");
			cmdList.Add("\\SQLServer:Buffer Manager\\Buffer cache hit ratio", "SQL_BufferCacheHitRatio");
			cmdList.Add("\\SQLServer:Databases(*)\\Transactions/sec", "SQL_TransactionsPerSec");
			cmdList.Add("\\SQLServer:Databases(*)\\Data File(s) Size (KB)", "SQL_DataFilesSizeInKB");
			cmdList.Add("\\SQLServer:Databases(*)\\Log File(s) Size (KB)", "SQL_LogFilesSizeInKB");
			cmdList.Add("\\SQLServer:General Statistics\\User Connections", "SQL_UserConnections");
			cmdList.Add("\\SQLServer:Locks(*)\\Lock Wait Time (ms)", "SQL_LockWaitTimeInMS");
			cmdList.Add("\\SQLServer:Locks(*)\\Lock Waits/sec", "SQL_LockWaitsPerSec");
			cmdList.Add("\\SQLServer:Locks(*)\\Number of Deadlocks/sec", "SQL_NumOfDealocksPerSec");
			cmdList.Add("\\SQLServer:Transactions\\Free Space in tempdb (KB)", "SQL_FreeSpaceInTempDBInKB");
			cmdList.Add("\\System\\Context Switches/sec", "Sys_ContextSwitchesPerSec");
			cmdList.Add("\\System\\Processor Queue Length", "Sys_ProcessorQueueLength");
			cmdList.Add("\\SQLServer:SQL Statistics\\Batch Requests/sec", "SQL_BatchRequestsPerSec");

	
			return cmdList;
		}

		private Dictionary<string, string> getSpStatsForTotalSum()
		{
			Dictionary<string, string> cmdList = new Dictionary<string, string>();
			cmdList.Add("\\Network Interface(*)\\Bytes Received/sec", "NetworkBytesReceivedPerSec");   
			cmdList.Add("\\Network Interface(*)\\Bytes Sent/sec", "NetworkBytesSentPerSec");   

			return cmdList;
		}

		public void checkServer(MonitoredItems.SharepointServer Server, ref TestResults AllTestsList, ReturnPowerShellObjects results)
		{
			try
			{
				switch (Server.Role)
				{

					case "Database":
						doSpStatsForDB(Server, ref AllTestsList, results);
						break;

					default:
						doSharepointTests(Server, ref AllTestsList, results);
						doSpStats(Server, ref AllTestsList, results);
						doSpStatsWithNoTotal(Server, ref AllTestsList, results);
						TestSiteCollections(Server, ref AllTestsList, results);
						GetVersion(Server, ref AllTestsList, results);
						GetSPConfigurationStats(Server, ref AllTestsList, results);
						GetRequiredServices(Server, ref AllTestsList, results);
						CheckTimerJobs(Server, ref AllTestsList, results);
						break;

				}

				results.PS.Commands.Clear();
					
				
				GC.Collect();
			}
			catch (Exception ex)
			{
				Common.WriteDeviceHistoryEntry(Server.ServerType, Server.Name, "Error in CheckServer: " + ex.Message, commonEnums.ServerRoles.SharePoint, Common.LogLevel.Normal);
			}
		}

		public void doSharepointTests( MonitoredItems.SharepointServer myServer, ref TestResults AllTestsList, ReturnPowerShellObjects powershellobj)
		{
			try
			{
				System.Collections.ObjectModel.Collection<PSObject> results = new System.Collections.ObjectModel.Collection<PSObject>();
				//Change the Path to the Script to suit your needs
				System.IO.StreamReader sr = new System.IO.StreamReader(AppDomain.CurrentDomain.BaseDirectory.ToString() + "Scripts\\SP_GatherSites.ps1");
				String str = "Invoke-Command -Session $ra -ScriptBlock {Add-PSSnapin Microsoft.SharePoint.Powershell; " + sr.ReadToEnd() + "}";
				powershellobj.PS.Commands.Clear();
				powershellobj.PS.Streams.ClearStreams();
				//results = powershell.Invoke();
				//Command cmds = new Command(str);
				//cmds.Parameters.Add("dagname", DAGName);
				//powershell.AddParameter("dagname", DAGName);
				powershellobj.PS.AddScript(str);
				//powershell.AddParameter("dagname", DAGName);
				results = powershellobj.PS.Invoke();

				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "SP_GatherSites Results: " + results.Count, Common.LogLevel.Normal);

				foreach (PSObject ps in results)
				{
					string WebApp = ps.Properties["WebApp"].Value == null ? "" : ps.Properties["WebApp"].Value.ToString();
					string SiteColl = ps.Properties["SiteColl"].Value == null ? "" : ps.Properties["SiteColl"].Value.ToString();  //site collection
					string Site = ps.Properties["Site"].Value == null ? "" : ps.Properties["Site"].Value.ToString();

					//**********************************************************ADD SQL STATEMENT*************************************************************************\\
					string sql = "";
					AllTestsList.SQLStatements.Add(new SQLstatements() { DatabaseName = "vitalsigns", SQL = sql });


					//*******************************************************ALERTS???*****************************************************************\\
				}
				int threshold = 10;
				if (results.Count > threshold)
				{
					//Common.makeAlert(true, ?????, commonEnums.AlertType.Web_Count, ref AllTestsList, "There were " + results.Count + " sites located in the scan");
				}
				else
				{
					//Common.makeAlert(false, ?????, commonEnums.AlertType.Web_Count, ref AllTestsList, "There were " + results.Count + " sites located in the scan when more than " + threshold + " were expected");
				}
			}
			catch (Exception ex)
			{
				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "Error in DoSharePointTests: " + ex.Message, commonEnums.ServerRoles.SharePoint, Common.LogLevel.Normal);
			}
		}
		public void doSpStats(MonitoredItems.SharepointServer myServer, ref TestResults AllTestsList, ReturnPowerShellObjects powershellobj)
		{
			try
			{
				Dictionary<string, string> cmdList;
				cmdList = getSpStatsList();
				string cmds = String.Join("','", cmdList.Keys);
				string tableVals = String.Join(",", cmdList.Values);
				System.Collections.ObjectModel.Collection<PSObject> results = new System.Collections.ObjectModel.Collection<PSObject>();
				String str = "Invoke-Command -Session $ra -ScriptBlock {Add-PSSnapin Microsoft.SharePoint.Powershell; " + "(Get-Counter -Counter '" + cmds + "').CounterSamples | Select Path, CookedValue }";
				powershellobj.PS.Commands.Clear();
				powershellobj.PS.Streams.ClearStreams();
				powershellobj.PS.AddScript(str);
				results = powershellobj.PS.Invoke();

				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "doSpStats Results: " + results.Count, Common.LogLevel.Normal);
				DateTime dtNow = DateTime.Now;
				int weekNumber = culture.Calendar.GetWeekOfYear(dtNow, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday);
				foreach (PSObject ps in results)
				{
					string statName = tableVals.IndexOf(',') > 0 ? tableVals.Substring(0, tableVals.IndexOf(',')) : tableVals;
					tableVals = tableVals.Substring(tableVals.IndexOf(',') + 1);
					string propValue = ps.Properties["CookedValue"].Value == null ? "" : ps.Properties["CookedValue"].Value.ToString();
					string path = ps.Properties["Path"].Value == null ? "" : ps.Properties["Path"].Value.ToString();  //site collection


					//**********************************************************ADD SQL STATEMENT*************************************************************************\\
					string sqlQuery = "Insert into VSS_Statistics.dbo.MicrosoftDailyStats(ServerName,ServerTypeId,Date,StatName,StatValue,WeekNumber,MonthNumber,YearNumber,DayNumber, HourNumber, Details) "
									+ " values('" + myServer.Name + "','" + myServer.ServerTypeId + "','" + dtNow + "','SP@" + statName + "'" + " ," + propValue.ToString() +
								   "," + weekNumber + ", " + dtNow.Month.ToString() + ", " + dtNow.Year.ToString() + ", " + dtNow.Day.ToString() + ", " + dtNow.Hour.ToString() + ", '')";
					AllTestsList.SQLStatements.Add(new SQLstatements() { SQL = sqlQuery, DatabaseName = "VSS_Statistics" });

					if( (new String[] {"TotalBytesReceivedPerSec", "ActiveSessions", "RequestsRejected", "TotalRequests", "NetworkBytesSentPerSec","NetworkBytesReceivedPerSec", "TotalBytesSentPerSec"}).Contains(statName))
					{
						string newStatName;
						switch (statName)
						{
							case "TotalBytesReceivedPerSec":
								newStatName = "WebServices Bytes Received";
								break;
							
							case "ActiveSessions":
								newStatName = "IISCurrent Connections";
								break;

							case "RequestsRejected":
								newStatName = "IISAppRequests Rejected";
								break;

							case "TotalRequests":
								newStatName = "IISAppRequests";
								break;

							case "TotalBytesSentPerSec":
								newStatName = "WebServices Bytes Sent";
								break;
							
							default:
								newStatName="";
								break;
						}

						SQLBuild sql = new SQLBuild();
						sql.ifExistsSQLSelect = "SELECT * FROM WindowsStatus WHERE ServerID = '" + myServer.ServerId + "' AND SName = '" + newStatName + "'";
						sql.onFalseDML = "INSERT INTO WindowsStatus (ServerID, SName, SValue) VALUES ('" + myServer.ServerId + "','" + newStatName + "','" + propValue.ToString() + "')";
						sql.onTrueDML = "UPDATE WindowsStatus set SValue='" + propValue.ToString() + "' WHERE ServerID='" + myServer.ServerId + "' AND SName='" + newStatName + "'";


						AllTestsList.SQLStatements.Add(new SQLstatements() {
							DatabaseName="VitalSigns", 
							SQL=sql.GetSQL(sql)
						});
					}


					//*******************************************************ALERTS???*****************************************************************\\
				}

			
			}
			catch (Exception ex)
			{
				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "Error in doSpStats: " + ex.Message, commonEnums.ServerRoles.SharePoint, Common.LogLevel.Normal);
			}
		}

		public void doSpStatsForDB(MonitoredItems.SharepointServer myServer, ref TestResults AllTestsList, ReturnPowerShellObjects powershellobj)
		{
			try
			{
				Dictionary<string, string> cmdList;
				//if (myServer.Role == "Database")
				cmdList = getSpStatsListForDatabase();
				//else
				//	cmdList = getSpStatsList();
				string cmds = String.Join("','", cmdList.Keys);
				string tableVals = String.Join(",", cmdList.Values);
				System.Collections.ObjectModel.Collection<PSObject> results = new System.Collections.ObjectModel.Collection<PSObject>();
				String str = "Invoke-Command -Session $ra -ScriptBlock {Add-PSSnapin Microsoft.SharePoint.Powershell; " + "(Get-Counter -ComputerName sp-db1 -Counter '" + cmds + "').CounterSamples | Select Path, CookedValue }";
				powershellobj.PS.Commands.Clear();
				powershellobj.PS.Streams.ClearStreams();
				powershellobj.PS.AddScript(str);
				results = powershellobj.PS.Invoke();

				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "doSpStatsForDB Results: " + results.Count, Common.LogLevel.Normal);
				DateTime dtNow = DateTime.Now;
				int weekNumber = culture.Calendar.GetWeekOfYear(dtNow, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday);
				foreach (PSObject ps in results)
				{
					string statName = tableVals.IndexOf(',') > 0 ? tableVals.Substring(0, tableVals.IndexOf(',')) : tableVals;
					tableVals = tableVals.Substring(tableVals.IndexOf(',') + 1);
					string propValue = ps.Properties["CookedValue"].Value == null ? "" : ps.Properties["CookedValue"].Value.ToString();
					string path = ps.Properties["Path"].Value == null ? "" : ps.Properties["Path"].Value.ToString();  //site collection


					//**********************************************************ADD SQL STATEMENT*************************************************************************\\
					string sqlQuery = "Insert into VSS_Statistics.dbo.MicrosoftDailyStatsTempForSpDb(ServerName,ServerTypeId,Date,StatName,StatValue,WeekNumber,MonthNumber,YearNumber,DayNumber, HourNumber, Details) "
									+ " values('" + myServer.Name + "','" + myServer.ServerTypeId + "','" + dtNow + "','SP@" + path + "'" + " ," + propValue.ToString() +
								   "," + weekNumber + ", " + dtNow.Month.ToString() + ", " + dtNow.Year.ToString() + ", " + dtNow.Day.ToString() + ", " + dtNow.Hour.ToString() + ", '')";
					AllTestsList.SQLStatements.Add(new SQLstatements() { SQL = sqlQuery, DatabaseName = "VSS_Statistics" });


					//*******************************************************ALERTS???*****************************************************************\\
				}
			}
			catch (Exception ex)
			{
				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "Error in doSpStatsForDB: " + ex.Message, commonEnums.ServerRoles.SharePoint, Common.LogLevel.Normal);
			}
		}

		public void doSpStatsWithNoTotal(MonitoredItems.SharepointServer myServer, ref TestResults AllTestsList, ReturnPowerShellObjects powershellobj)
		{
			try
			{
				Dictionary<string, string> cmdList;
				cmdList = getSpStatsForTotalSum();
				string[] cmds = cmdList.Keys.ToArray();
				//string cmds = String.Join("','", cmdList.Keys);
				string tableVals = String.Join(",", cmdList.Values);
				System.Collections.ObjectModel.Collection<PSObject> results = new System.Collections.ObjectModel.Collection<PSObject>();
				//String str = "Invoke-Command -Session $ra -ScriptBlock {Add-PSSnapin Microsoft.SharePoint.Powershell; " + "(Get-Counter -ComputerName sp-db1 -Counter '" + cmds + "').CounterSamples | Select Path, CookedValue }";
				string scriptPath = AppDomain.CurrentDomain.BaseDirectory.ToString() + "Scripts\\SP_CountersWithoutTotal.ps1";
				string script = "param ($arrOfCounters); Invoke-Command -Session $ra -FilePath '" + scriptPath + "' -ArgumentList (,$arrOfCounters)";

				PSCommand command = new PSCommand();
				command.AddCommand("Invoke-Command");
				command.AddParameter("Session", "$ra");
				command.AddParameter("FilePath", "'" + scriptPath + "'");
				command.AddParameter("ArgumentList", cmds);
				//command.AddParameter("arrOfCounters", cmds);
				
				powershellobj.PS.Commands.Clear();
				powershellobj.PS.Streams.ClearStreams();
				//powershellobj.PS.Commands = command;
				powershellobj.PS.AddScript(script);
				powershellobj.PS.AddParameter("arrOfCounters", cmds);
				results = powershellobj.PS.Invoke();
				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "SP_CountersWtihoutTotal Results: " + results.Count, Common.LogLevel.Normal);

				if (results.Count == 0)
					foreach (ErrorRecord err in powershellobj.PS.Streams.Error)
						Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "SP_CountersWtihoutTotal Errors: " + err.Exception, Common.LogLevel.Normal);

				DateTime dtNow = DateTime.Now;
				int weekNumber = culture.Calendar.GetWeekOfYear(dtNow, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday);
				foreach (PSObject ps in results)
				{
					string statName = tableVals.IndexOf(',') > 0 ? tableVals.Substring(0, tableVals.IndexOf(',')) : tableVals;
					tableVals = tableVals.Substring(tableVals.IndexOf(',') + 1);
					string propValue = ps.Properties["CookedValue"].Value == null ? "" : ps.Properties["CookedValue"].Value.ToString();
					string path = ps.Properties["Path"].Value == null ? "" : ps.Properties["Path"].Value.ToString();  //site collection

					string sqlQuery = "Insert into VSS_Statistics.dbo.MicrosoftDailyStats(ServerName,ServerTypeId,Date,StatName,StatValue,WeekNumber,MonthNumber,YearNumber,DayNumber, HourNumber, Details) "
									+ " values('sp-db1.jnittech.com','" + myServer.ServerTypeId + "','" + dtNow + "','SP@" + statName + "'" + " ," + propValue.ToString() +
								   "," + weekNumber + ", " + dtNow.Month.ToString() + ", " + dtNow.Year.ToString() + ", " + dtNow.Day.ToString() + ", " + dtNow.Hour.ToString() + ", '')";
					AllTestsList.SQLStatements.Add(new SQLstatements() { SQL = sqlQuery, DatabaseName = "VSS_Statistics" });

					
					if( (new String[] {  "NetworkBytesSentPerSec","NetworkBytesReceivedPerSec"}).Contains(statName))
					{
						string newStatName;
						switch (statName)
						{
							case "NetworkBytesSentPerSec":
								newStatName = "NetworkBytesSent";
								break;
							
							case "NetworkBytesReceivedPerSec":
								newStatName = "NetworkBytes Received";
								break;
							
							default:
								newStatName="";
								break;
						}

						SQLBuild sql = new SQLBuild();
						sql.ifExistsSQLSelect = "SELECT * FROM WindowsStatus WHERE ServerID = '" + myServer.ServerId + "' AND SName = '" + newStatName + "'";
						sql.onFalseDML = "INSERT INTO WindowsStatus (ServerID, SName, SValue) VALUES ('" + myServer.ServerId + "','" + newStatName + "','" + propValue.ToString() + "')";
						sql.onTrueDML = "UPDATE WindowsStatus set SValue='" + propValue.ToString() + "' WHERE ServerID='" + myServer.ServerId + "' AND SName='" + newStatName + "'";

						AllTestsList.SQLStatements.Add(new SQLstatements() {
							DatabaseName="VitalSigns", 
							SQL=sql.GetSQL(sql)
						});
					}

					//*******************************************************ALERTS???*****************************************************************\\
				}
			}
			catch (Exception ex)
			{
				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "Error in doSpStatsForDB: " + ex.Message, commonEnums.ServerRoles.SharePoint, Common.LogLevel.Normal);
			}
		}


		public void GetVersion(MonitoredItems.SharepointServer myServer, ref TestResults AllTestsList, ReturnPowerShellObjects powershellobj)
		{
			try
			{
				System.Collections.ObjectModel.Collection<PSObject> results = new System.Collections.ObjectModel.Collection<PSObject>();
				System.IO.StreamReader sr = new System.IO.StreamReader(AppDomain.CurrentDomain.BaseDirectory.ToString() + "Scripts\\SP_GetVersion.ps1");
				String str = "Invoke-Command -Session $ra -ScriptBlock {Add-PSSnapin Microsoft.SharePoint.Powershell; " + sr.ReadToEnd() + "}";
				powershellobj.PS.Commands.Clear();
				powershellobj.PS.Streams.ClearStreams();
				powershellobj.PS.AddScript(str);
				results = powershellobj.PS.Invoke();

				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "SP_GetVersion Results: " + results.Count, Common.LogLevel.Normal);

				foreach (PSObject ps in results)
				{
					string DisplayName = ps.Properties["DisplayName"].Value == null ? "" : ps.Properties["DisplayName"].Value.ToString();
					string Build = ps.Properties["Build"].Value == null ? "" : ps.Properties["Build"].Value.ToString();  //site collection

					myServer.VersionNo = DisplayName + "(" + Build + ")";
				}
			}
			catch (Exception ex)
			{
				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "Error in getVersion: " + ex.Message, commonEnums.ServerRoles.SharePoint, Common.LogLevel.Normal);
			}
		}

		public void TestSiteCollections(MonitoredItems.SharepointServer myServer, ref TestResults AllTestsList, ReturnPowerShellObjects powershellobj)
		{
			try
			{
				bool failedAlert = false;

				System.Collections.ArrayList siteList = new System.Collections.ArrayList();
				System.Collections.ArrayList ruleList = new System.Collections.ArrayList();

				System.Collections.ObjectModel.Collection<PSObject> results = new System.Collections.ObjectModel.Collection<PSObject>();
				System.IO.StreamReader sr = new System.IO.StreamReader(AppDomain.CurrentDomain.BaseDirectory.ToString() + "Scripts\\SP_TestAllSiteCollections.ps1");
				String str = "Invoke-Command -Session $ra -ScriptBlock {Add-PSSnapin Microsoft.SharePoint.Powershell; " + sr.ReadToEnd() + "}";
				powershellobj.PS.Commands.Clear();
				powershellobj.PS.Streams.ClearStreams();
				powershellobj.PS.AddScript(str);
				results = powershellobj.PS.Invoke();

				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "SP_TestAllSiteCollections Results: " + results.Count, Common.LogLevel.Normal);

				foreach (PSObject ps in results)
				{
					string Site = ps.Properties["Site"].Value == null ? "" : ps.Properties["Site"].Value.ToString().Substring("SPSite URL=".Length);
					string Rules = ps.Properties["Results"].Value.ToString();
					string PassedCount = ps.Properties["PassedCount"].Value == null ? "0" : ps.Properties["PassedCount"].Value.ToString();
					string FailedWarningCount = ps.Properties["FailedWarningCount"].Value == null ? "" : ps.Properties["FailedWarningCount"].Value.ToString();
					string FailedErrorCount = ps.Properties["FailedErrorCount"].Value == null ? "" : ps.Properties["FailedErrorCount"].Value.ToString();

					if (FailedErrorCount != "0" || FailedWarningCount != "0")
					{
						failedAlert = true;
						siteList.Add(Site);

						List<string> RulesList = new List<string>();
						while (true)
						{
							if (Rules == "")
								break;
							string s = "";
							if (Rules.IndexOf('\n') >= 0)
							{
								s = Rules.Substring(0, Rules.IndexOf('\n'));
								Rules = Rules.Substring(Rules.IndexOf('\n') + 1);
							}
							else
							{
								s = Rules;
								Rules = "";
							}

							if (s.Trim() != "")
								RulesList.Add(s);
						}

						foreach (string line in RulesList)
						{
							//"SPSiteHealthResult Status=Passed RuleName=\"Conflicting Content Types\" RuleId=befe203b-a8c0-48c2-b5f0-27c10f9e1622 "
							string status = line.Substring(line.IndexOf("Status=") + "Status=".Length, line.IndexOf(" ", line.IndexOf("Status=")) - line.IndexOf("Status=") - "Status=".Length);
							if (status != "Passed")
							{
								string RuleParseString = "RuleName=\"";
								string currRule = line.Substring(line.IndexOf(RuleParseString) + RuleParseString.Length, line.IndexOf("\"", line.IndexOf(RuleParseString) + RuleParseString.Length) - line.IndexOf(RuleParseString) - RuleParseString.Length);
								ruleList.Add(currRule);
							}
						}						
					}
					else
					{
					}
				}

				if (failedAlert)
				{
					if(siteList.Count > 1)
						Common.makeAlert(false, myServer, commonEnums.AlertType.Site_Health_Check, ref AllTestsList, "Two or more sites failed a total of " + ruleList.Count + " rules.", "SharePoint");
					else if(ruleList.Count > 1)
						Common.makeAlert(false, myServer, commonEnums.AlertType.Site_Health_Check, ref AllTestsList, "The site " + siteList[0].ToString() + " failed " + ruleList.Count + " rules.", "SharePoint");
					else
						Common.makeAlert(false, myServer, commonEnums.AlertType.Site_Health_Check, ref AllTestsList, "The site " + siteList[0].ToString() + " failed the rule " + ruleList[0].ToString() + "", "SharePoint");
				}
				else
				{
					Common.makeAlert(true, myServer, commonEnums.AlertType.Site_Health_Check, ref AllTestsList, "All sites passed all of their respected rules.", "SharePoint");
				}

			}
			catch (Exception ex)
			{
				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "Error in TestSiteCollections: " + ex.Message, commonEnums.ServerRoles.SharePoint, Common.LogLevel.Normal);
			}
		}

		public void GetSPConfigurationStats(MonitoredItems.SharepointServer myServer, ref TestResults AllTestsList, ReturnPowerShellObjects powershellobj)
		{
			try
			{
				System.Collections.ObjectModel.Collection<PSObject> results = new System.Collections.ObjectModel.Collection<PSObject>();
				String sr = AppDomain.CurrentDomain.BaseDirectory.ToString() + "Scripts\\SP_IssAndAspNetStats.ps1";
				String str = "Invoke-Command -Session $ra -FilePath '" + sr + "'";
				powershellobj.PS.Commands.Clear();
				powershellobj.PS.Streams.ClearStreams();
				powershellobj.PS.AddScript(str);
				results = powershellobj.PS.Invoke();

				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "SP_IssAndAspNetStats Results: " + results.Count, Common.LogLevel.Normal);

				foreach (PSObject ps in results)
				{
					string ISS_Version = ps.Properties["ISS_Version"].Value == null ? "" : ps.Properties["ISS_Version"].Value.ToString();
					string ISS_Status = ps.Properties["ISS_Status"].Value == null ? "" : ps.Properties["ISS_Status"].Value.ToString();
					string ASPNetVersions = ps.Properties["ASPNetVersions"].Value == null ? "" : ps.Properties["ASPNetVersions"].Value.ToString();

					string[] value = { ISS_Version, ISS_Status, ASPNetVersions };
					string[] name = { "IIS Version", "IIS Service State", "ASP.NET Version" };

					SQLBuild sql;
					//string sql = "INSERT INTO WindowsStatus (ServerID, SName, SValue) VALUES ";
					for (int i = 0; i < value.Length; i++)
					{
						sql = new SQLBuild();
						sql.ifExistsSQLSelect = "SELECT * FROM WindowsStatus WHERE ServerID = '" + myServer.ServerId + "' AND SName = '" + name[i].ToString() + "'";
						sql.onFalseDML = "INSERT INTO WindowsStatus (ServerID, SName, SValue) VALUES ('" + myServer.ServerId + "','" + name[i].ToString() + "','" + value[i].ToString() + "')";
						sql.onTrueDML = "UPDATE WindowsStatus set SValue='" + value[i].ToString() + "' WHERE ServerID='" + myServer.ServerId + "' AND SName='" + name[i].ToString() + "'";					
					
						AllTestsList.SQLStatements.Add(new SQLstatements() {DatabaseName="VitalSigns", SQL=sql.GetSQL(sql) });
					}

				}

				

			}
			catch (Exception ex)
			{
				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "Error in TestSiteCollections: " + ex.Message, commonEnums.ServerRoles.SharePoint, Common.LogLevel.Normal);
			}
		}

		private void GetRequiredServices(MonitoredItems.SharepointServer myServer, ref TestResults AllTestsList, ReturnPowerShellObjects powershellobj)
		{
			//runspace = powershellobj.runspace;
			PowerShell powershell = powershellobj.PS;

			Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "In GetRequiredServices ", commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);
			try
			{
				String sr = AppDomain.CurrentDomain.BaseDirectory.ToString() + "Scripts\\SP_GetServices.ps1";
				String str = "Invoke-Command -Session $ra -FilePath '" + sr + "'";
				powershell.Streams.Error.Clear();

				powershell.AddScript(str);

				Collection<PSObject> results = powershell.Invoke();

				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "GetRequiredServices output results: " + results.Count.ToString(), commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);

				Dictionary<string,string> dict = new Dictionary<string,string>();

				foreach (PSObject ps in results)
				{
					string Name = ps.Properties["Name"].Value == null ? "" : ps.Properties["Name"].Value.ToString();
					string Status = ps.Properties["Status"].Value == null ? "" : ps.Properties["Status"].Value.ToString();

					dict.Add(Name, Status);
				}
				
				string[] RequiredServices;
				
				switch(myServer.Role)
				{
					case "Database":
						
						break;

					case "WebFrontEnd":

						break;

					case "Application":

						break;

				}

				CommonDB db = new CommonDB();

				if (db.GetData("SELECT * FROM [WindowsServices] WHERE ServerName='" + myServer.Name + "' AND ServerRequired=1").Rows.Count > 0)
				{
					foreach(string key in dict.Keys)
					{
						string service = key;

						SQLBuild objSQL = new SQLBuild();
						objSQL.ifExistsSQLSelect = "SELECT * FROM [vitalsigns].[dbo].[WindowsServices] WHERE ServerName='" + myServer.Name + "' and Service_Name='" + service + "'";
						objSQL.onTrueDML = "update [vitalsigns].[dbo].[WindowsServices] set " +
											"[DateStamp]='" + DateTime.Now + "', ServerRequired='1',ServerTypeId=" + myServer.ServerTypeId.ToString() + " WHERE [ServerName]='" + myServer.Name + "' and [Service_Name]='" + service + "'";

						objSQL.onFalseDML = "Insert into WindowsServices(ServerName,Service_Name,Monitored,DateStamp,ServerRequired,ServertypeId) "
										+ " values('" + myServer.Name + "','" + service + "', 1,'" + DateTime.Now + "', '1'," + myServer.ServerTypeId.ToString() + ")";

						string sqlQuery = objSQL.GetSQL(objSQL);
						AllTestsList.SQLStatements.Add(new SQLstatements() { SQL = sqlQuery, DatabaseName = "vitalsigns" });
					}
				}
				else
				{
					foreach(string key in dict.Keys)
					{
						string service = key;

						SQLBuild objSQL = new SQLBuild();
						objSQL.ifExistsSQLSelect = "SELECT * FROM [vitalsigns].[dbo].[WindowsServices] WHERE ServerName='" + myServer.Name + "' and Service_Name='" + service + "'";
						objSQL.onTrueDML = "update [vitalsigns].[dbo].[WindowsServices] set " +
											"[DateStamp]='" + DateTime.Now + "', ServerRequired='1', Monitored='1',ServerTypeId=" + myServer.ServerTypeId.ToString() + " WHERE [ServerName]='" + myServer.Name + "' and [Service_Name]='" + service + "'";

						objSQL.onFalseDML = "Insert into WindowsServices(ServerName,Service_Name,Monitored,DateStamp,ServerRequired,ServertypeId) "
										+ " values('" + myServer.Name + "','" + service + "', 1,'" + DateTime.Now + "', '1'," + myServer.ServerTypeId.ToString() + ")";
						string sqlQuery = objSQL.GetSQL(objSQL);
						AllTestsList.SQLStatements.Add(new SQLstatements() { SQL = sqlQuery, DatabaseName = "vitalsigns" });
					}
				}

			}
			catch (Exception ex)
			{
				AllTestsList.StatusDetails.Add(new TestList() { Details = "There was an Error Executing the Required Services Script at " + System.DateTime.Now.ToShortTimeString(), TestName = "Services", Category = commonEnums.ServerRoles.Windows, Result = commonEnums.ServerResult.Fail });
				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "Error in getRequiredHealth: " + ex.Message, commonEnums.ServerRoles.SharePoint, Common.LogLevel.Normal);

			}
			finally
			{

			}
		}

		private void CheckTimerJobs(MonitoredItems.SharepointServer myServer, ref TestResults AllTestsList, ReturnPowerShellObjects powershellobj)
		{
			//runspace = powershellobj.runspace;
			PowerShell powershell = powershellobj.PS;

			Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "In CheckTimerJobs ", commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);
			try
			{
				String sr = AppDomain.CurrentDomain.BaseDirectory.ToString() + "Scripts\\SP_TimedJobs.ps1";
				String str = "Invoke-Command -Session $ra -FilePath '" + sr + "'";
				powershell.Streams.Error.Clear();

				powershell.AddScript(str);

				Collection<PSObject> results = powershell.Invoke();

				foreach (ErrorRecord er in powershell.Streams.Error)
					Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, er.Exception.ToString(), Common.LogLevel.Normal);

				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "CheckTimerJobs output results: " + results.Count.ToString(), commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);

                List<string> FarmNames = new List<string>();
				List<string> JobDefInserts = new List<string>();

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

                    if (!FarmNames.Contains("" + Farm + "")) FarmNames.Add("" + Farm + "");
					JobDefInserts.Add("('" + JobDefinitionTitle + "', '" + ServerName + "', '" + WebApplicationName + "', '" + Status + "', '" + StartTime + "', '" + EndTime + "', '" + DatabaseName + "', '" + ErrorMessage + "', '" + Schedule + "', '" + Farm + "')");
				
				}

                if (FarmNames.Count > 0)
				{
                    string sql = "DELETE FROM [SharePointTimerJobs] WHERE Farm IN ('" + String.Join("','", FarmNames) + "')";
					AllTestsList.SQLStatements.Add(new SQLstatements() { SQL = sql, DatabaseName = "VitalSigns"});

					sql = "INSERT INTO [SharePointTimerJobs] (JobName, ServerName, WebApplicationName, Status, StartTime, EndTime, DatabaseName, ErrorMessage, Schedule, Farm) VALUES " + String.Join(",", JobDefInserts) + "";
					AllTestsList.SQLStatements.Add(new SQLstatements() { SQL = sql, DatabaseName = "VitalSigns"});
				}

			}
			catch (Exception ex)
			{
				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "Error in CheckTimerJobs: " + ex.Message, commonEnums.ServerRoles.SharePoint, Common.LogLevel.Normal);

			}
			finally
			{

			}
		}


	}
}
