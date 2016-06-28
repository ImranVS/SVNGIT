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
using System.Globalization;
using VSFramework;
using System;

using VSNext.Mongo.Entities;
using VSNext.Mongo.Repository;
using MongoDB.Driver;

namespace VitalSignsMicrosoftClasses
{
	class ActiveDirectoryCommon
	{
		CultureInfo culture = CultureInfo.CurrentCulture;
		public void checkServer(MonitoredItems.ActiveDirectoryServer Server, ref TestResults AllTestsList, ref bool isResponding)
		{
			string cmdlets = "-CommandName Get-ADDomainController, Get-ADObject";

			using (ReturnPowerShellObjects results = Common.PrereqForActiveDirectoryWithCmdlets(Server.Name, Server.UserName, Server.Password, Server.ServerType, Server.IPAddress, commonEnums.ServerRoles.Windows, cmdlets))
			{

				if (results.Connected == false)
				{

					//***************************************************Not Responding********************************************//
				}
				else
				{


					checkADQueryLatency(Server, ref AllTestsList, results);
					checkActiveDirectoryAvailability(Server, ref AllTestsList, results);
					checkADLogonTest(Server, ref AllTestsList, results);
					//checkRequiredServices(Server, ref AllTestsList, results);
					//getADUsers(Server, ref AllTestsList, results);
					//DiagnoseTest(Server, ref AllTestsList, results);
					//firstScript(Server, ref AllTestsList, results);
					ReplicationSummary(Server, ref AllTestsList, results);


					updateResults(Server, ref AllTestsList);
					results.PS.Commands.Clear();


				}
			}
			GC.Collect();
		}

		private void updateResults(MonitoredItems.ActiveDirectoryServer Server, ref TestResults AllTestResults)
		{
			MongoStatementsUpdate<VSNext.Mongo.Entities.Status> updateStatement = new MongoStatementsUpdate<VSNext.Mongo.Entities.Status>();
            updateStatement.filterDef = updateStatement.repo.Filter.Where(i => i.TypeAndName == Server.TypeANDName);
            updateStatement.updateDef = updateStatement.repo.Updater
                .Set(i => i.LogonTest, Server.ADLogonTest)
                .Set(i => i.QueryTest, Server.ADQueryTest)
                .Set(i => i.LdapPortTest, Server.ADPortTest);

            AllTestResults.MongoEntity.Add(updateStatement);
		}
		private void checkActiveDirectoryAvailability(MonitoredItems.ActiveDirectoryServer myServer, ref TestResults AllTestsList, ReturnPowerShellObjects powershellobj)
		{
			try
			{
				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "checkActiveDirectoryAvailability: Starting.", Common.LogLevel.Normal);
				System.Collections.ObjectModel.Collection<PSObject> results = new System.Collections.ObjectModel.Collection<PSObject>();
				//Change the Path to the Script to suit your needs
				System.IO.StreamReader sr = new System.IO.StreamReader(AppDomain.CurrentDomain.BaseDirectory.ToString() + "Scripts\\AD_Availability.ps1");
				String str = sr.ReadToEnd();
				str = "$computers = Get-ADDomainController -Filter * | where {$_.Hostname -eq  '" + myServer.Name +"'} " + str;
				powershellobj.PS.Commands.Clear();
				powershellobj.PS.Streams.ClearStreams();
				//results = powershell.Invoke();
				//Command cmds = new Command(str);
				//cmds.Parameters.Add("dagname", DAGName);
				//powershell.AddParameter("dagname", DAGName);
				powershellobj.PS.AddScript(str);
				//powershell.AddParameter("dagname", DAGName);
				results = powershellobj.PS.Invoke();

				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "AD_Availability Port Test Results: " + results.Count, Common.LogLevel.Normal);
				if (results.Count != 0)
				{
					foreach (PSObject ps in results)
					{
						string Port = ps.Properties["Port"].Value == null ? "" : ps.Properties["Port"].Value.ToString();
						string Response = ps.Properties["Response"].Value == null ? "" : ps.Properties["Response"].Value.ToString();  //"Open" or “Closed / Filtered”
						string ComputerName = ps.Properties["ComputerName"].Value == null ? "" : ps.Properties["ComputerName"].Value.ToString();

						if (Response == "Open")
						{
							myServer.ADPortTest = "Pass";
							Common.makeAlert(true, myServer, commonEnums.AlertType.Port_Test, ref AllTestsList, myServer.ServerType);
						}
						else
						{
							myServer.ADPortTest = "Fail";
							Common.makeAlert(false, myServer, commonEnums.AlertType.Port_Test, ref AllTestsList, myServer.ServerType);
						}
					}
				}
				else
				{
					bool failImportModule = false;
					foreach (ErrorRecord err in powershellobj.PS.Streams.Error)
					{
						if (err.Exception.ToString().Contains("is not recognized as the name of a cmdlet, function"))
							failImportModule = true;
					}

					if (failImportModule)
					{
						myServer.ADPortTest = "N/A";
					}
					else
					{
						myServer.ADPortTest = "Fail";
						Common.makeAlert(false, myServer, commonEnums.AlertType.Port_Test, ref AllTestsList, myServer.ServerType);
					}
				}
			}
			catch (Exception ex)
			{
				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "AD_Availability Port Test Exception: " + ex.Message.ToString(), Common.LogLevel.Verbose);
				myServer.ADPortTest = "Fail";
				Common.makeAlert(false, myServer, commonEnums.AlertType.Port_Test, ref AllTestsList, myServer.ServerType);
			}
		}

		public void checkADQueryLatency(MonitoredItems.ActiveDirectoryServer myServer, ref TestResults AllTestsList, ReturnPowerShellObjects powershellobj)	
		{
			try
			{
				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "checkADQueryLatency: Starting.", Common.LogLevel.Normal);
				System.Collections.ObjectModel.Collection<PSObject> results = new System.Collections.ObjectModel.Collection<PSObject>();
				System.IO.StreamReader sr = new System.IO.StreamReader(AppDomain.CurrentDomain.BaseDirectory.ToString() + "Scripts\\AD_QueryLatency.ps1");
				String str = sr.ReadToEnd();
				str = "$DCs = Get-ADDomainController -filter * | where {$_.Hostname -eq  '" + myServer.Name + "'}" + str;
				powershellobj.PS.Commands.Clear();
				powershellobj.PS.Streams.ClearStreams();
				powershellobj.PS.AddScript(str);
				results = powershellobj.PS.Invoke();

				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "AD_QueryLatency Results: " + results.Count, Common.LogLevel.Normal);
				DateTime dtNow = DateTime.Now;
				int weekNumber = culture.Calendar.GetWeekOfYear(dtNow, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday);
				if (results.Count > 0)
				{
					foreach (PSObject ps in results)
					{
						string Name = ps.Properties["Name"].Value == null ? "" : ps.Properties["Name"].Value.ToString();
						string seconds = ps.Properties["Seconds"].Value == null ? "" : ps.Properties["Seconds"].Value.ToString();
						string forest = ps.Properties["Forest"].Value == null ? "" : ps.Properties["Forest"].Value.ToString();
						Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "AD_QueryLatency Results: Name:" + Name, Common.LogLevel.Normal);
						Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "AD_QueryLatency Results: Seconds:" + seconds, Common.LogLevel.Normal);
						Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "AD_QueryLatency Results: Forest:" + forest, Common.LogLevel.Normal);
						myServer.ADQueryTest = "Pass";

                        AllTestsList.MongoEntity.Add(Common.GetInsertIntoDailyStats(myServer, "AD@QueryLatency", seconds.ToString()));

						double threshold = 10;
						double temp = 0;
						if (Double.TryParse(seconds, out temp) && threshold > Double.Parse(seconds))
						{
							myServer.ADQueryTest = "Pass";
							Common.makeAlert(true, myServer, commonEnums.AlertType.Query_Latency, ref AllTestsList, myServer.ServerType);
						}
						else
						{
							myServer.ADQueryTest = "Fail";
							Common.makeAlert(false, myServer, commonEnums.AlertType.Query_Latency, ref AllTestsList, myServer.ServerType);

						}
					}

				}
				else
				{
					bool failImportModule = false;
					foreach (ErrorRecord err in powershellobj.PS.Streams.Error)
					{
						if (err.Exception.ToString().Contains("is not recognized as the name of a cmdlet, function"))
							failImportModule = true;
					}

					if (failImportModule)
					{
						myServer.ADQueryTest = "N/A";
					}
					else
					{
						myServer.ADQueryTest = "Fail";
						Common.makeAlert(false, myServer, commonEnums.AlertType.Query_Latency, ref AllTestsList, myServer.ServerType);
					}
				}
			}
			catch(Exception ex)
			{
				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "AD_QueryLatency Results: Exception: " + ex.Message.ToString(), Common.LogLevel.Verbose);
				myServer.ADQueryTest = "Fail";
				Common.makeAlert(false, myServer, commonEnums.AlertType.Query_Latency, ref AllTestsList, myServer.ServerType);
			}
		}

		private void checkADLogonTest(MonitoredItems.ActiveDirectoryServer myServer, ref TestResults AllTestsList, ReturnPowerShellObjects powershellobj)
		{
			try
			{
				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "checkADLogonTest: Starting.", Common.LogLevel.Normal);
				myServer.ADLogonTest = "Fail";
				System.Collections.ObjectModel.Collection<PSObject> results = new System.Collections.ObjectModel.Collection<PSObject>();
				//Change the Path to the Script to suit your needs
				System.IO.StreamReader sr = new System.IO.StreamReader(AppDomain.CurrentDomain.BaseDirectory.ToString() + "Scripts\\AD_LogonTest.ps1");
				String str = sr.ReadToEnd();
				string[] userNames = myServer.UserName.Split('\\');
				string userName = myServer.UserName.ToString();
				if (userNames.Length > 0)
					userName = userNames[1];
				str = str.Replace("#user", userName).Replace("#pwd", myServer.Password).Replace("#domain", myServer.Name);
				powershellobj.PS.Commands.Clear();
				powershellobj.PS.Streams.ClearStreams();
				//results = powershell.Invoke();
				//Command cmds = new Command(str);
				//cmds.Parameters.Add("dagname", DAGName);
				//powershell.AddParameter("dagname", DAGName);
				powershellobj.PS.AddScript(str);
				//powershell.AddParameter("dagname", DAGName);
				results = powershellobj.PS.Invoke();

				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "AD_LogonTest Results: " + results.Count, Common.LogLevel.Normal);

				if (results.Count > 0)
				{
					string sLogonTest = results[0].ToString();

					if (sLogonTest == "False")
					{
						myServer.ADLogonTest = "Fail";
						Common.makeAlert(false , myServer, commonEnums.AlertType.Logon_Test, ref AllTestsList, myServer.ServerType );
					}
					else
					{
						myServer.ADLogonTest = "Pass";
						Common.makeAlert(true, myServer, commonEnums.AlertType.Logon_Test, ref AllTestsList, myServer.ServerType);
					}
				}
			}
			catch (Exception ex)
			{
				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "AD_LogonTest: Exception: " + ex.Message.ToString(), Common.LogLevel.Verbose);
				myServer.ADLogonTest = "Fail";
				Common.makeAlert(false, myServer, commonEnums.AlertType.Logon_Test, ref AllTestsList, myServer.ServerType);
			}

		}
		/*
		public void firstScript(MonitoredItems.ActiveDirectoryServer myServer, ref TestResults AllTestsList, ReturnPowerShellObjects powershellobj)
		{
			try
			{
				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "checkADQueryLatency: Starting.", Common.LogLevel.Normal);
				System.Collections.ObjectModel.Collection<PSObject> results = new System.Collections.ObjectModel.Collection<PSObject>();
				System.IO.StreamReader sr = new System.IO.StreamReader(AppDomain.CurrentDomain.BaseDirectory.ToString() + "Scripts\\AD_ReplStatus.ps1");
				String str = sr.ReadToEnd();
				//str = "$DCs = Get-ADDomainController -filter * | where {$_.Hostname -eq  '" + myServer.Name + "'}" + str;

				powershellobj.PS.Commands.Clear();
				powershellobj.PS.Streams.ClearStreams();
				powershellobj.PS.AddScript(str);
				results = powershellobj.PS.Invoke();

				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "AD_QueryLatency Results: " + results.Count, Common.LogLevel.Normal);
				DateTime dtNow = DateTime.Now;
				int weekNumber = culture.Calendar.GetWeekOfYear(dtNow, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday);
				if (results.Count > 0)
				{
					Collection<PSObject> trimmedResults = new Collection<PSObject>();
					for (int i = 0; i < results.Count; i++)
					{
						if (results[i].BaseObject != "")
						{
							trimmedResults.Add(results[i]);
						}
					}
					results = trimmedResults;
					parseRepStatus(results);
					string s = trimmedResults[0].BaseObject.ToString();

				}
				else
				{
					myServer.ADQueryTest = "Fail";
					Common.makeAlert(true, myServer, commonEnums.AlertType.AD_Query_Latency, ref AllTestsList, myServer.ServerType);
				}
			}
			catch (Exception ex)
			{
				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "AD_QueryLatency Results: Exception: " + ex.Message.ToString(), Common.LogLevel.Verbose);
				myServer.ADQueryTest = "Fail";
				Common.makeAlert(false, myServer, commonEnums.AlertType.AD_Query_Latency, ref AllTestsList, myServer.ServerType);
			}
		}

		public void parseRepStatus(Collection<PSObject> arr)
		{
			try
			{
				Collection<ReplicationStatusObject> collOfObjects = new Collection<ReplicationStatusObject>();

				//finds the == 's
				int startingIndex = 0;
				for(startingIndex = 0; startingIndex < arr.Count; startingIndex++)
					if(arr[startingIndex].ToString().StartsWith("===="))
						break;
				for (int i = startingIndex + 1; i < arr.Count; i++)
				{
					string LDAP = arr[i].ToString();
					while (true)
					{
						i++;
						if (i >= arr.Count)
							break;
						if (arr[i].ToString().Replace(" ", "").StartsWith("Default-First-Site-Name") == false)
						{
							i--;
							break;
						}

						string firstSite = arr[i].ToString().Substring(arr[i].ToString().IndexOf('\\') + 1);
						i++;
						string guid = arr[i].ToString().Substring(arr[i].ToString().IndexOf(':') + 1).Replace(" ", "");
						i++;
						int startChar = arr[i].ToString().IndexOf("@");
						int endChar = arr[i].ToString().IndexOf(" was ");
						string lastAttemptTimeDate = arr[i].ToString().Substring(startChar + 2, endChar - startChar - 1);

						string temp = arr[i].ToString().Substring(endChar);
						bool success;
						if (arr[i].ToString().Substring(endChar) == " was successful.")
							success = true;
						else
							success = false;
						ReplicationStatusObject obj = new ReplicationStatusObject(LDAP, firstSite, guid, lastAttemptTimeDate, success);
						collOfObjects.Add(obj);
					}
				}

			}
			catch (Exception ex)
			{ };
		}
		*/
		public void ReplicationSummary(MonitoredItems.ActiveDirectoryServer myServer, ref TestResults AllTestsList, ReturnPowerShellObjects powershellobj)
		{
			try
			{
				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "ReplicationSummary: Starting.", Common.LogLevel.Normal);
				System.Collections.ObjectModel.Collection<PSObject> results = new System.Collections.ObjectModel.Collection<PSObject>();
				System.IO.StreamReader sr = new System.IO.StreamReader(AppDomain.CurrentDomain.BaseDirectory.ToString() + "Scripts\\AD_ReplSummary.ps1");
				String str = sr.ReadToEnd();
				//str = "$DCs = Get-ADDomainController -filter * | where {$_.Hostname -eq  '" + myServer.Name + "'}" + str;

				powershellobj.PS.Commands.Clear();
				powershellobj.PS.Streams.ClearStreams();
				cmdScriptWrapper(ref powershellobj.PS, "Scripts\\AD_ReplSummary.ps1", myServer);
				//powershellobj.PS.AddScript(str);
				results = powershellobj.PS.Invoke();

				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "ReplicationSummary Results: " + results.Count, Common.LogLevel.Normal);
				DateTime dtNow = DateTime.Now;
				int weekNumber = culture.Calendar.GetWeekOfYear(dtNow, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday);
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
					parseRepSummary(results, myServer, ref AllTestsList);

				}
				else
				{
					//myServer.ADQueryTest = "Fail";
					//Common.makeAlert(true, myServer, commonEnums.AlertType.Query_Latency, ref AllTestsList, myServer.ServerType);
				}
			}
			catch (Exception ex)
			{
				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "ReplicationSummary Results: Exception: " + ex.Message.ToString(), Common.LogLevel.Verbose);
				//myServer.ADQueryTest = "Fail";
				//Common.makeAlert(false, myServer, commonEnums.AlertType.Query_Latency, ref AllTestsList, myServer.ServerType);
			}
		}

		public void parseRepSummary(Collection<PSObject> arr, MonitoredItems.ActiveDirectoryServer myServer, ref TestResults AllTestsList)
		{

            List<VSNext.Mongo.Entities.ActiveDirectoryReplicationStatus> list = new List<VSNext.Mongo.Entities.ActiveDirectoryReplicationStatus>();

            string failsServerList = "";
			try
			{
				//Collection<ReplicationStatusObject> collOfObjects = new Collection<ReplicationStatusObject>();
				//finds the == 's
				int startingIndex = 0;
				for (startingIndex = 0; startingIndex < arr.Count; startingIndex++)
					if (arr[startingIndex].ToString().Replace(" ","").StartsWith("SourceDSA"))
						break;
				//startingIndex++;
				for (int i = startingIndex + 1; i < arr.Count; i++)
				{
					string line = arr[i].ToString();
					string trimmedLine = line;
					// AD-DCIN1                  28m:12s    0 /   5    0  

					int startIndex = line.TakeWhile(char.IsWhiteSpace).Count();
					trimmedLine = line.Substring(startIndex);
					//AD-DCIN1                  28m:12s    0 /   5    0  

					string serverName = trimmedLine.Substring(0, trimmedLine.IndexOf("  "));
					trimmedLine = trimmedLine.Replace(serverName, "");
					trimmedLine = trimmedLine.Substring(trimmedLine.TakeWhile(char.IsWhiteSpace).Count());
					//28m:12s    0 /   5    0  

					string largestDelta = trimmedLine.Substring(0, trimmedLine.IndexOf(' '));
					trimmedLine = trimmedLine.Replace(largestDelta, "");
					trimmedLine = trimmedLine.Substring(trimmedLine.TakeWhile(char.IsWhiteSpace).Count());
					//0 /   5    0  

					string fails = trimmedLine.Substring(0, trimmedLine.IndexOf(' '));
					trimmedLine = trimmedLine.Replace(fails, "");
					trimmedLine = trimmedLine.Substring(trimmedLine.TakeWhile(char.IsWhiteSpace).Count());
					////   5    0  

					trimmedLine = trimmedLine.Substring(1);
					trimmedLine = trimmedLine.Substring(trimmedLine.TakeWhile(char.IsWhiteSpace).Count());
					//5    0

					string total = trimmedLine.Substring(0, trimmedLine.IndexOf(' '));
					trimmedLine = trimmedLine.Replace(total, "");
					trimmedLine = trimmedLine.Substring(trimmedLine.TakeWhile(char.IsWhiteSpace).Count());
					//0  

					if (fails != "0")
						failsServerList += serverName + ",";

                    VSNext.Mongo.Entities.ActiveDirectoryReplicationStatus AdRepStatus = new VSNext.Mongo.Entities.ActiveDirectoryReplicationStatus();
                    AdRepStatus.DirectoryPartitions = total;
                    AdRepStatus.Fails = fails;
                    AdRepStatus.LargestDelta = largestDelta;
                    AdRepStatus.SoureServer = serverName;

                    list.Add(AdRepStatus);

				}


				MongoStatementsUpdate<VSNext.Mongo.Entities.Status> updateStatement = new MongoStatementsUpdate<VSNext.Mongo.Entities.Status>();
                updateStatement.filterDef = updateStatement.repo.Filter.Where(i => i.TypeAndName == myServer.TypeANDName);
                updateStatement.updateDef = updateStatement.repo.Updater.Set(i => i.ActiveDirectoryReplicationStatus, list.ToArray());

				if(failsServerList != "")
					if(failsServerList.Length < 50)
						Common.makeAlert(false, myServer, commonEnums.AlertType.Replication_Summary, ref AllTestsList, "This server has a failed replication with " + failsServerList, "Active Directory");
					else
						Common.makeAlert(false, myServer, commonEnums.AlertType.Replication_Summary, ref AllTestsList, "This server has a failed replication with many servers", "Active Directory");
				else
					Common.makeAlert(true, myServer, commonEnums.AlertType.Replication_Summary, ref AllTestsList, "This server had no issues with replication", "Active Directory");

			}
			catch (Exception ex)
			{ }

			//return sql.Substring(0,sql.Length-1);
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

			powershell.AddScript(@"$so = New-PSSessionOption -SkipCNCheck -SkipRevocationCheck –SkipCACheck ");
			powershell.AddScript(@"$cmd={" + scriptToExecute + "}");
			powershell.AddScript(@"Invoke-Command -ComputerName " + Server.Name + " -Credential $cred -ScriptBlock $cmd");



		}

	}

	class ReplicationStatusObject
	{
		string LDAP;
		string DefaultFirstSiteName;
		string GUID;
		string LastAttempt;
		bool successful;

		public ReplicationStatusObject(string _ldap, string _default, string _guid, string _last, bool _succ)
		{
			LDAP = _ldap;
			DefaultFirstSiteName = _default;
			GUID = _guid;
			LastAttempt = _last;
			successful = _succ;
		}
	}
	

}
