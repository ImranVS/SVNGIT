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

			SQLBuild sql = new SQLBuild();
			sql.ifExistsSQLSelect= "Select * from ActiveDirectoryTest Where ServerID = " + Server.ServerId + "";
			sql.onFalseDML = " INSERT INTO DBO.ActiveDirectoryTest(ServerId,LogonTest,QueryTest,LDApPortTest,LastScanDate) values("
				+ Server.ServerId + ",'" + Server.ADLogonTest + "','" + Server.ADQueryTest + "','" + Server.ADPortTest + "','"
				+ DateTime.Now.ToString() + "')";
			sql.onTrueDML = " UPDATE ActiveDirectoryTest SET LogonTest = '" + Server.ADLogonTest + "', QueryTest = '" + Server.ADQueryTest + "', LDApPortTest = '" + Server.ADPortTest + "',"
				+ " LastScanDate = '" + DateTime.Now.ToString() + "' WHERE ServerId=" + Server.ServerId;
			AllTestResults.SQLStatements.Add(new SQLstatements() { SQL = sql.GetSQL(sql), DatabaseName = "VitalSigns" });

			//AllTestResults.SQLStatements.Add(new SQLstatements() { SQL = "delete from dbo.ActiveDirectoryTest where ServerId=" + Server.ServerId, DatabaseName = "vitalsigns" });
			//string sSQL = " INSERT INTO DBO.ActiveDirectoryTest(ServerId,LogonTest,QueryTest,LDApPortTest,Advertising,FrsSysVol,Replications,Services,DNS,FsmoCheck,LastScanDate) values(" 
			//    + Server.ServerId + ",'" + Server.ADLogonTest + "','" + Server.ADQueryTest + "','" + Server.ADPortTest + "','" 
			//    + Server.ADAdvertisingTest + "','" + Server.ADFrsSysVolTest + "','" + Server.ADReplicationsTest + "','" + Server.ADServicesTest + "','" 
			//    + Server.ADDNSTest + "','" + Server.ADFsmoCheckTest + "','"+ DateTime.Now.ToString() +"')";
			//AllTestResults.SQLStatements.Add(new SQLstatements() { SQL = sSQL, DatabaseName = "vitalsigns" });

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

						//**********************************************************ADD SQL STATEMENT*************************************************************************\\
						string sql = "";
						AllTestsList.SQLStatements.Add(new SQLstatements() { DatabaseName = "vitalsigns", SQL = sql });

						//*******************************************************ALERTS???*****************************************************************\\
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
						//**********************************************************ADD SQL STATEMENT*************************************************************************\\
						string sqlQuery = "Insert into VSS_Statistics.dbo.MicrosoftDailyStats(ServerName,ServerTypeId,Date,StatName,StatValue,WeekNumber,MonthNumber,YearNumber,DayNumber, HourNumber, Details) "
								+ " values('" + myServer.Name + "','" + myServer.ServerTypeId + "','" + dtNow + "','AD@QueryLatency'" + " ," + seconds.ToString() +
							   "," + weekNumber + ", " + dtNow.Month.ToString() + ", " + dtNow.Year.ToString() + ", " + dtNow.Day.ToString() + ", " + dtNow.Hour.ToString() + ", '')";
						AllTestsList.SQLStatements.Add(new SQLstatements() { SQL = sqlQuery, DatabaseName = "VSS_Statistics" });

						//*******************************************************ALERTS???*****************************************************************\\

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

		//public void getADUsers(MonitoredItems.ActiveDirectoryServer myServer, ref TestResults AllTestsList, ReturnPowerShellObjects powershellobj)
		//{

		//    System.Collections.ObjectModel.Collection<PSObject> results = new System.Collections.ObjectModel.Collection<PSObject>();
		//    //Change the Path to the Script to suit your needs
		//    //System.IO.StreamReader sr = new System.IO.StreamReader(AppDomain.CurrentDomain.BaseDirectory.ToString() + "Scripts\\AD_QueryLatency.ps1");
		//    String str = "Get-ADUser -filter *";
		//    powershellobj.PS.Commands.Clear();
		//    powershellobj.PS.Streams.ClearStreams();
		//    //results = powershell.Invoke();
		//    //Command cmds = new Command(str);
		//    //cmds.Parameters.Add("dagname", DAGName);
		//    //powershell.AddParameter("dagname", DAGName);
		//    powershellobj.PS.AddScript(str);
		//    //powershell.AddParameter("dagname", DAGName);
		//    results = powershellobj.PS.Invoke();

		//    Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "Get-ADUser -filter * Results: " + results.Count, Common.LogLevel.Normal);
		//    DateTime dtNow = DateTime.Now;
		//    int weekNumber = culture.Calendar.GetWeekOfYear(dtNow, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday);
		//    foreach (PSObject ps in results)
		//    {
		//        string Name = ps.Properties["Name"].Value == null ? "" : ps.Properties["Name"].Value.ToString();
		//        string Class = ps.Properties["ObjectClass"].Value == null ? "" : ps.Properties["ObjectClass"].Value.ToString();
		//        string Enabled = ps.Properties["Enabled"].Value == null ? "" : ps.Properties["Enabled"].Value.ToString();
		//        Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "getADUsers Results: Name:" + Name, Common.LogLevel.Normal);
		//        Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "getADUsers Results: Class:" + Class, Common.LogLevel.Normal);
		//        Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "getADUsers Results: Enabled:" + Enabled, Common.LogLevel.Normal);

		//        ////**********************************************************ADD SQL STATEMENT*************************************************************************\\
		//        //string sqlQuery = "Insert into VSS_Statistics.dbo.MicrosoftDailyStats(ServerName,Date,StatName,StatValue,WeekNumber,MonthNumber,YearNumber,DayNumber, HourNumber, Details) "
		//        //        + " values('" + myServer.Name + "','" + dtNow + "','AD@QueryLatency#" + Enabled + "'" + " ," + seconds.ToString() +
		//        //       "," + weekNumber + ", " + dtNow.Month.ToString() + ", " + dtNow.Year.ToString() + ", " + dtNow.Day.ToString() + ", " + dtNow.Hour.ToString() + ", '')";
		//        //AllTestsList.SQLStatements.Add(new SQLstatements() { SQL = sqlQuery, DatabaseName = "VSS_Statistics" });

		//        //*******************************************************ALERTS???*****************************************************************\\

				
		//    }
		//}

		//public void checkRequiredServices(MonitoredItems.ActiveDirectoryServer myServer, ref TestResults AllTestsList, ReturnPowerShellObjects powershellobj)
		//{

		//    string requiredServices = "NTDS";
		//    string[] ListOfServices = (requiredServices).Split(',');

		//    CommonDB db = new CommonDB();


		//    //if a row exists, then dont change the monitored tag...else set it to 1
		//    if (db.GetData("SELECT * FROM [WindowsServices] WHERE ServerName='" + myServer.Name + "' AND ServerRequired=1").Rows.Count > 0)
		//    {
		//        foreach (string service in ListOfServices.Where(w => !string.IsNullOrEmpty(w)))
		//        {
		//            SQLBuild objSQL = new SQLBuild();
		//            objSQL.ifExistsSQLSelect = "SELECT * FROM [vitalsigns].[dbo].[WindowsServices] WHERE ServerName='" + myServer.Name + "' and Service_Name='" + service + "'";
		//            objSQL.onTrueDML = "update [vitalsigns].[dbo].[WindowsServices] set " +
		//                                "[DateStamp]='" + DateTime.Now + "', ServerRequired='1' WHERE [ServerName]='" + myServer.Name + "' and [Service_Name]='" + service + "'";

		//            objSQL.onFalseDML = "Insert into WindowsServices(ServerName,Service_Name,Monitored,DateStamp,ServerRequired) "
		//                            + " values('" + myServer.Name + "','" + service + "', 1,'" + DateTime.Now + "', '1')";
		//            string sqlQuery = objSQL.GetSQL(objSQL);
		//            AllTestsList.SQLStatements.Add(new SQLstatements() { SQL = sqlQuery, DatabaseName = "vitalsigns" });
		//        }
		//    }
		//    else
		//    {
		//        foreach (string service in ListOfServices.Where(w => !string.IsNullOrEmpty(w)))
		//        {
		//            SQLBuild objSQL = new SQLBuild();
		//            objSQL.ifExistsSQLSelect = "SELECT * FROM [vitalsigns].[dbo].[WindowsServices] WHERE ServerName='" + myServer.Name + "' and Service_Name='" + service + "'";
		//            objSQL.onTrueDML = "update [vitalsigns].[dbo].[WindowsServices] set " +
		//                                "[DateStamp]='" + DateTime.Now + "', ServerRequired='1', Monitored='1' WHERE [ServerName]='" + myServer.Name + "' and [Service_Name]='" + service + "'";

		//            objSQL.onFalseDML = "Insert into WindowsServices(ServerName,Service_Name,Monitored,DateStamp,ServerRequired) "
		//                            + " values('" + myServer.Name + "','" + service + "', 1,'" + DateTime.Now + "', '1')";
		//            string sqlQuery = objSQL.GetSQL(objSQL);
		//            AllTestsList.SQLStatements.Add(new SQLstatements() { SQL = sqlQuery, DatabaseName = "vitalsigns" });
		//        }
		//    }
		//}

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

					//**********************************************************ADD SQL STATEMENT*************************************************************************\\
					string sql = "";
					AllTestsList.SQLStatements.Add(new SQLstatements() { DatabaseName = "vitalsigns", SQL = sql });

					//*******************************************************ALERTS???*****************************************************************\\
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
					
					//AllTestsList.SQLStatements(

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
			string sql = "INSERT INTO [vitalsigns].[dbo].[ActiveDirectoryReplicationSummary] ([ServerName],[SourceServer],[LargestDelta],[Fails],[DirectoryPartitions],[LastScanTime]) VALUES ";
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
					//([ServerName],[SourceServer],[LargestDelta],[Fails],[DirectoryPartitions],[LastScanTime])
					sql += "('" + myServer.Name + "', '" + serverName + "', '" + largestDelta + "', '" + fails + "','" + total + "','" + DateTime.Now + "'),";

					if (fails != "0")
						failsServerList += serverName + ",";
				}

				AllTestsList.SQLStatements.Add(new SQLstatements() { DatabaseName="vitalsigns", SQL="DELETE FROM ActiveDirectoryReplicationSummary WHERE ServerName='" + myServer.Name + "'"});
				AllTestsList.SQLStatements.Add(new SQLstatements() { DatabaseName = "vitalsigns", SQL = sql.Substring(0, sql.Length - 1) });

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

		//public void DiagnoseTest(MonitoredItems.ActiveDirectoryServer myServer, ref TestResults AllTestsList, ReturnPowerShellObjects powershellobj)
		//{
		//    try
		//    {
		//        Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "checkADQueryLatency: Starting.", Common.LogLevel.Normal);
		//        System.Collections.ObjectModel.Collection<PSObject> results = new System.Collections.ObjectModel.Collection<PSObject>();
		//        System.IO.StreamReader sr = new System.IO.StreamReader(AppDomain.CurrentDomain.BaseDirectory.ToString() + "Scripts\\AD_DCDiagnose.ps1");
		//        String str = sr.ReadToEnd();
		//        //str = "$DCs = Get-ADDomainController -filter * | where {$_.Hostname -eq  '" + myServer.Name + "'}" + str;

		//        powershellobj.PS.Commands.Clear();
		//        powershellobj.PS.Streams.ClearStreams();
		//        //powershellobj.PS.AddScript(str);
		//        cmdScriptWrapper(ref powershellobj.PS, "Scripts\\AD_DCDiagnose.ps1", myServer);
		//        results = powershellobj.PS.Invoke();

		//        Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "AD_QueryLatency Results: " + results.Count, Common.LogLevel.Normal);
		//        DateTime dtNow = DateTime.Now;
		//        int weekNumber = culture.Calendar.GetWeekOfYear(dtNow, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday);
		//        if (results.Count > 0)
		//        {
		//            Collection<PSObject> trimmedResults = new Collection<PSObject>();
		//            for (int i = 0; i < results.Count; i++)
		//            {
		//                if (results[i].BaseObject.ToString() != "")
		//                {
		//                    trimmedResults.Add(results[i]);
		//                }
		//            }
		//            results = trimmedResults;
		//            DiagnoseTestParse(results, myServer, ref AllTestsList);

		//            //AllTestsList.SQLStatements(

		//        }
		//        else
		//        {
		//            myServer.ADQueryTest = "Fail";
		//            Common.makeAlert(true, myServer, commonEnums.AlertType.Query_Latency, ref AllTestsList, myServer.ServerType);
		//        }
		//    }
		//    catch (Exception ex)
		//    {
		//        Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "AD_QueryLatency Results: Exception: " + ex.Message.ToString(), Common.LogLevel.Verbose);
		//        myServer.ADQueryTest = "Fail";
		//        Common.makeAlert(false, myServer, commonEnums.AlertType.Query_Latency, ref AllTestsList, myServer.ServerType);
		//    }
		//}

		//public void DiagnoseTestParse(Collection<PSObject> arr, MonitoredItems.ActiveDirectoryServer myServer, ref TestResults AllTestsList)
		//{
		//    Collection<PSObject> trimmedResults = new Collection<PSObject>();

		//    for (int i = 0; i < arr.Count; i++)
		//    {
		//        string s = arr[i].BaseObject.ToString().Replace(" ", "");

		//        if (arr[i].BaseObject.ToString().Replace(" ", "").StartsWith("Startingtest:") || arr[i].BaseObject.ToString().Replace(" ", "").StartsWith("........................."))
		//        {
		//            trimmedResults.Add(arr[i]);
		//        }

		//    }

		//    for (int i = 0; i < arr.Count; i++)
		//    {
		//        if (arr[i].BaseObject.ToString().Replace(" ", "").StartsWith("SummaryofDNStestresults:"))
		//        {
		//            trimmedResults.Add(arr[i + 2]);
		//            trimmedResults.Add(arr[i + 5]);
		//            i = i + 4;
		//        }
		//    }
		//    bool firstDNSTestDone = false;

		//    try
		//    {
		//        for (int i = 0; i < trimmedResults.Count; i = i + 2)
		//        {
		//            string testLine = trimmedResults[i].ToString();
		//            string resultLine = trimmedResults[i + 1].ToString();

		//            if (testLine.Replace(" ", "") == "AuthBascForwDelDynRRegExt")
		//            {
		//                //DNS Results
		//                //                                Auth Basc Forw Del  Dyn  RReg Ext          
		//                //   JNITTECH-AD                  PASS WARN PASS FAIL WARN PASS PASS 


		//                int startIndex = resultLine.TakeWhile(char.IsWhiteSpace).Count();
		//                resultLine = resultLine.Substring(startIndex);
		//                //JNITTECH-AD                  PASS WARN PASS FAIL WARN PASS PASS 

		//                resultLine = resultLine.Substring(resultLine.IndexOf("  "));
		//                //                  PASS WARN PASS FAIL WARN PASS PASS 

		//                startIndex = resultLine.TakeWhile(char.IsWhiteSpace).Count();
		//                resultLine = resultLine.Substring(startIndex);
		//                //PASS WARN PASS FAIL WARN PASS PASS 

		//                string AuthenticationTest = resultLine.Substring(0, resultLine.IndexOf(" "));
		//                resultLine = resultLine.Substring(resultLine.IndexOf(" ") + 1);
		//                //WARN PASS FAIL WARN PASS PASS 
		//                string AuthError = "The DNS Authentication Test failed at " + DateTime.Now + ".";
		//                if (AuthenticationTest != "PASS")
		//                    AuthError = findDNSError("Auth", arr, AuthError, "DNS Authentication");
		//                else
		//                    AuthError = AuthError.Replace("failed", "passed");

		//                string BasicTest = resultLine.Substring(0, resultLine.IndexOf(" "));
		//                resultLine = resultLine.Substring(resultLine.IndexOf(" ") + 1);
		//                //PASS FAIL WARN PASS PASS 
		//                string BasicError = "The DNS Basic Test failed at " + DateTime.Now + ".";
		//                if (BasicTest != "PASS")
		//                    BasicError = findDNSError("Basc", arr, BasicError, "DNS Basic");
		//                else
		//                    BasicError = BasicError.Replace("failed", "passed");

		//                string FowardersTest = resultLine.Substring(0, resultLine.IndexOf(" "));
		//                resultLine = resultLine.Substring(resultLine.IndexOf(" ") + 1);
		//                //FAIL WARN PASS PASS 
		//                string FowardersError = "The DNS Fowarders Test failed at " + DateTime.Now + ".";
		//                if (FowardersTest != "PASS")
		//                    FowardersError = findDNSError("Forw", arr, FowardersError, "DNS Fowarders");
		//                else
		//                    FowardersError = FowardersError.Replace("failed", "passed");

		//                string DelegationTest = resultLine.Substring(0, resultLine.IndexOf(" "));
		//                resultLine = resultLine.Substring(resultLine.IndexOf(" ") + 1);
		//                //WARN PASS PASS 
		//                string DelegationError = "The DNS Delegation Test failed at " + DateTime.Now + ".";
		//                if (DelegationTest != "PASS")
		//                    DelegationError = findDNSError("Del", arr, DelegationError, "DNS Delegation");
		//                else
		//                    DelegationError = DelegationError.Replace("failed", "passed");

		//                string DynamicTest = resultLine.Substring(0, resultLine.IndexOf(" "));
		//                resultLine = resultLine.Substring(resultLine.IndexOf(" ") + 1);
		//                //PASS PASS 
		//                string DynamicError = "The DNS Dynamic Update Test failed at " + DateTime.Now + ".";
		//                if (DynamicTest != "PASS")
		//                    DynamicError = findDNSError("Dyn", arr, DynamicError, "DNS Dynamic Update");
		//                else
		//                    DynamicError = DynamicError.Replace("failed", "passed");

		//                string RecordRegistrationTest = resultLine.Substring(0, resultLine.IndexOf(" "));
		//                resultLine = resultLine.Substring(resultLine.IndexOf(" ") + 1);
		//                //PASS 
		//                string RecordRegistrationError = "The DNS Record Registration Test failed at " + DateTime.Now + ".";
		//                if (RecordRegistrationTest != "PASS")
		//                    RecordRegistrationError = findDNSError("Rreg", arr, RecordRegistrationError, "DNS Record Registration");
		//                else
		//                    RecordRegistrationError = RecordRegistrationError.Replace("failed", "passed");

		//                string ExtNameTest = resultLine.Substring(0, resultLine.IndexOf(" "));
		//                resultLine = resultLine.Substring(resultLine.IndexOf(" ") + 1);
		//                string ExtNameError = "The DNS External Name Test failed at " + DateTime.Now + ".";
		//                if (ExtNameTest != "PASS")
		//                    ExtNameError = findDNSError("Ext", arr, ExtNameError, "DNS External Name");
		//                else
		//                    ExtNameError = ExtNameError.Replace("failed", "passed");

		//                Common.makeAlert(AuthenticationTest == "PASS" ? true : false, myServer, commonEnums.AlertType.DNS_Authentication_Test, ref AllTestsList, AuthError, "Active Directory");
		//                Common.makeAlert(BasicTest == "PASS" ? true : false, myServer, commonEnums.AlertType.DNS_Basic_Test, ref AllTestsList, BasicError, "Active Directory");
		//                Common.makeAlert(FowardersTest == "PASS" ? true : false, myServer, commonEnums.AlertType.DNS_Fowarders_Test, ref AllTestsList, FowardersError, "Active Directory");
		//                Common.makeAlert(DelegationTest == "PASS" ? true : false, myServer, commonEnums.AlertType.DNS_Delegation_Test, ref AllTestsList, DelegationError, "Active Directory");
		//                Common.makeAlert(DynamicTest == "PASS" ? true : false, myServer, commonEnums.AlertType.DNS_Dynamic_Update_Test, ref AllTestsList, DynamicError, "Active Directory");
		//                Common.makeAlert(RecordRegistrationTest == "PASS" ? true : false, myServer, commonEnums.AlertType.DNS_Record_Registration_Test, ref AllTestsList, RecordRegistrationError, "Active Directory");
		//                Common.makeAlert(ExtNameTest == "PASS" ? true : false, myServer, commonEnums.AlertType.DNS_External_Name_Test, ref AllTestsList, ExtNameError, "Active Directory");

		//                continue;
		//            }

		//            string test = testLine.Substring(testLine.IndexOf(':') + 1).Replace(" ", "");
		//            string result = resultLine.Contains("passed") ? "Pass" : "Fail";

		//            if (result == "Fail")
		//            {
		//                if (checkForAccessDenied(arr, testLine, resultLine))
		//                    result = "N/A";
		//            }
		//            commonEnums.AlertType alertType = commonEnums.AlertType.Active_Sync;  //using this to see if "null"
		//            switch (test)
		//            {
		//                case "Advertising":
		//                    myServer.ADAdvertisingTest = result;
		//                    alertType = commonEnums.AlertType.Advertising_Test;
		//                    break;

		//                case "FrsSysVol":
		//                    myServer.ADFrsSysVolTest = result;
		//                    alertType = commonEnums.AlertType.FRS_System_Volume_Test;
		//                    break;

		//                case "Replications":
		//                    myServer.ADReplicationsTest = result;
		//                    alertType = commonEnums.AlertType.Replications_Test;
		//                    break;

		//                case "Services":
		//                    myServer.ADServicesTest = result;
		//                    alertType = commonEnums.AlertType.Services_Test;
		//                    break;

		//                case "DNS":
		//                    if (firstDNSTestDone)
		//                    {
		//                        if (myServer.ADDNSTest == "Fail" || result == "Fail")
		//                            myServer.ADDNSTest = "Fail";
		//                        else
		//                            myServer.ADDNSTest = "Pass";

		//                        alertType = commonEnums.AlertType.DNS_Test;
		//                    }
		//                    else
		//                    {
		//                        myServer.ADDNSTest = result;
		//                        firstDNSTestDone = true;
		//                    }
		//                    break;

		//                case "FsmoCheck":
		//                    myServer.ADFsmoCheckTest = result;
		//                    alertType = commonEnums.AlertType.FSMO_Check_Test;
		//                    break;

		//            }
		//            string details;
		//            if (result == "Pass")
		//                details = "The " + test + " test passed with no issues.";
		//            else if (result == "N/A")
		//                details = "The " + test + " test failed with a message of Access Denied at " + DateTime.Now + ".";
		//            else
		//                details = "The " + test + " test failed at " + DateTime.Now + ".";

		//            if (alertType != commonEnums.AlertType.Active_Sync)
		//                Common.makeAlert(result == "Pass" ? true : false, myServer, alertType, ref AllTestsList, details, "Active Directory");

		//        }
		//    }
		//    catch (Exception ex)
		//    { }

		//    //return sql.Substring(0,sql.Length-1);
		//}

		//public Boolean checkForAccessDenied(Collection<PSObject> arr, string testLine, string resultLine)
		//{
		//    int i = 0;
		//    while (arr[i].ToString() != testLine)
		//        i++;

		//    int endIndex = i;
		//    while (arr[endIndex].ToString() != resultLine)
		//        endIndex++;

		//    for (int n = i; n < endIndex; n++)
		//        if (arr[n].ToString().Contains("Access is denied"))
		//            return true;

		//    return false;
		//}

		//public string findDNSError(string type, Collection<PSObject> arr, string error, string testName)
		//{

		//    for (int i = 0; i < arr.Count(); i++)
		//    {
		//        if (arr[i].ToString().Contains(type) && arr[i].ToString().Contains("TEST:"))
		//            return "The " + testName + " test failed with the following message: " + arr[i + 1].ToString().Substring(arr[i + 1].ToString().TakeWhile(char.IsWhiteSpace).Count());
		//    }

		//    return error;
		//}

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
