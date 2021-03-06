﻿using System.Collections.Generic;
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
//using System.Net.Http;
//using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Configuration;
namespace VitalSignsMicrosoftClasses
{
	class Office365Common
	{
		CultureInfo culture = CultureInfo.CurrentCulture;
		public string vbCrLf = System.Environment.NewLine;
		public string nodeName = "";
		#region main
		public void checkServer(MonitoredItems.Office365Server Server, ref TestResults AllTestsList, ReturnPowerShellObjects results)
		{
			if (ConfigurationManager.AppSettings["VSNodeName"] != null)
				nodeName = ConfigurationManager.AppSettings["VSNodeName"].ToString();

			if (Server.Mode == "ADFS")
				doADFSTest(Server, ref AllTestsList);

			using (results)
			{

				Parameters p = new Parameters();
				p.myServer = Server;
				p.PSO = results;
				p.TS = AllTestsList;

				TestAutoDiscovery(Server, ref AllTestsList);
				TestIMAP(Server, ref AllTestsList);
				TestSMTP(Server, ref AllTestsList);
				TestPOP(Server, ref AllTestsList);
				TestMAPIConectivity(Server, ref AllTestsList, results);

				//Thread o365Th = null;
				//o365Th = new Thread(() => doLyncTests(p));
				//o365Th.Name = Server.Name + " doLyncTests";
				//WaitForThread(o365Th, Server);
				doLyncTests(p);
				if (Server.EnableCreateSiteTest)
				{
					doSPOTests(p);
					//o365Th = new Thread(() => doSPOTests(p));
					//o365Th.Name = Server.Name + " doSPOTests";
					//WaitForThread(o365Th, Server);
				}


				//o365Th = new Thread(() => getMailBoxStats(p));
				//o365Th.Name = Server.Name + " getMailBoxStats";
				//WaitForThread(o365Th, Server);
				getMailBoxStats(p);

				//o365Th = new Thread(() => getMobileStats(p));
				//o365Th.Name = Server.Name + " getMobileStats";
				//WaitForThread(o365Th, Server);
				getMobileStats(p);
				//Common.WriteDeviceHistoryEntry(Server.ServerType, Server.Name, "do REST API Tests ", Common.LogLevel.Normal);

				//o365Th = new Thread(() => doAPITests(p));
				//o365Th.Name = Server.Name + " doAPITests";
				//WaitForThread(o365Th, Server);

				//getMailboxes(Server, ref AllTestsList, results);
				//getMailboxeDetails(Server, ref AllTestsList, results);
				getMsolAccountSku(Server, ref AllTestsList, results);
				//getAllUsers(Server, ref AllTestsList, results);
				//getMobileUsers(Server, ref AllTestsList, results);
				//getMailboxActivity(Server, ref AllTestsList, results);
				getMsolCompanyInfo(Server, ref AllTestsList, results);

				//getLyncStats(Server, ref AllTestsList, results);
				//getLyncDevices(Server, ref AllTestsList, results);
				//getLyncPAVTimeReport(Server, ref AllTestsList, results);
				//getLyncConferenceReport(Server, ref AllTestsList, results);
				//getLyncP2PSessionReport(Server, ref AllTestsList, results);
				//doO365RESTApiTests(Server, ref AllTestsList);

				if (Server.Mode == "Dir Sync" && Server.DirSyncServerName != "")
				{
					ReturnPowerShellObjects PSO = null;
					PSO = Common.PrereqForWindows(Server.DirSyncServerName, Server.DirSyncUID, Server.DirSyncPWD, "Windows", Server.DirSyncServerName, commonEnums.ServerRoles.Empty);
					using (PSO)
					{
						System.Security.SecureString securePassword = Common.String2SecureString(Server.DirSyncPWD);
						PSCredential creds = new PSCredential(Server.DirSyncUID, securePassword);
						getDirSyncStats(Server, creds, Server.DirSyncServerName, AllTestsList, PSO);
					}


				}

				//updateResults(Server, ref AllTestsList);

			}
			//}

			GC.Collect();
		}
		private void getMailBoxStats(Parameters p)
		{
			getMailboxeDetails(p.myServer, ref p.TS, p.PSO);
			getMailboxActivity(p.myServer, ref p.TS, p.PSO);

		}
		private void doLyncTests(Parameters p)
		{
			getLyncStats(p.myServer, ref p.TS, p.PSO);
			getLyncDevices(p.myServer, ref p.TS, p.PSO);
			getLyncPAVTimeReport(p.myServer, ref p.TS, p.PSO);
			getLyncConferenceReport(p.myServer, ref p.TS, p.PSO);
			getLyncP2PSessionReport(p.myServer, ref p.TS, p.PSO);
		}
		private void doSPOTests(Parameters p)
		{
			createSPOSite(p.myServer, ref p.TS, p.PSO);
			removeSPOSite(p.myServer, ref p.TS, p.PSO);
		}
		private void getMobileStats(Parameters p)
		{
			getMobileUsers(p.myServer, ref p.TS, p.PSO);
		}
		private void doAPITests(Parameters p)
		{
			doO365RESTApiTests(p.myServer, ref p.TS);
		}
		#endregion
		#region restAPI
		public void doO365RESTApiTests(MonitoredItems.Office365Server myServer, ref TestResults AllTestsList)
		{
			O365RESTApi p = new O365RESTApi();
			p.doO365RESTApiTests(myServer, ref AllTestsList);
		}
		
		#endregion
		#region general
		private void WaitForThread(Thread Th, MonitoredItems.MicrosoftServer Server)
		{
			Th.CurrentCulture = Thread.CurrentThread.CurrentCulture;
			Th.Start();
			if (!Th.Join(TimeSpan.FromSeconds(120)))
			{
				//If the thread takes longer than 60 seconds
				Common.WriteDeviceHistoryEntry(Server.ServerType, Server.Name, Th.Name + " Thread is hung.  Will now abort the thread and continue.");
				Th.Abort();
			}

		}
		public static SecureString String2SecureString(string password)
		{
			SecureString remotePassword = new SecureString();
			for (int i = 0; i < password.Length; i++)
				remotePassword.AppendChar(password[i]);

			return remotePassword;
		}
		private void GetWMIPowerShell(MonitoredItems.Office365Server myServer, ref ReturnPowerShellObjects PSO, PSCredential creds, string IPAddress, String scriptName, bool isPowershellFile)
		{
			try
			{

				string scriptToExecute;
				if (isPowershellFile)
				{
					Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "GetWMIPowerShell : Scrpt file location:" + AppDomain.CurrentDomain.BaseDirectory.ToString() + "Scripts\\" + scriptName, Common.LogLevel.Verbose);
					System.IO.StreamReader scriptStream = new System.IO.StreamReader(AppDomain.CurrentDomain.BaseDirectory.ToString() + "Scripts\\" + scriptName);
					scriptToExecute = scriptStream.ReadToEnd();
				}
				else
				{
					scriptToExecute = scriptName;
				}
				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "GetWMIPowerShell : Script:" + scriptToExecute, Common.LogLevel.Verbose);
				PSO.PS.Commands.Clear();
				PSCommand cmd = new PSCommand();



				String script = @"$a = Invoke-Command -Session $sess -ScriptBlock {" + scriptToExecute + "}" + System.Environment.NewLine +
								@"echo $a";

				//string s = PSO.Session.Id.ToString();
				cmd.AddScript(@"$sess = (Get-PSSession)[0]");
				cmd.AddScript(script);
				//Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "GetWMIPowerShell : Sesion Id:" + PSO.Session.Id.ToString(), Common.LogLevel.Verbose);
				PSO.PS.Commands = cmd;
			}
			catch (Exception ex)
			{
				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "GetWMIPowerShell: Exception: " + ex.Message.ToString(), Common.LogLevel.Verbose);
			}

		}
		#endregion
		#region connectivity
		public void doURLTest(MonitoredItems.Office365Server myServer, ref TestResults AllTestsList)
		{
			O365RESTApi p = new O365RESTApi();
			p.doURLTest(myServer, ref AllTestsList);


		}
		public ReturnPowerShellObjects testO365ServerConnectivity(MonitoredItems.Office365Server Server, ref TestResults AllTestsList, ref bool isResponding)
		{
			doURLTest(Server, ref AllTestsList);
			string cmdlets = "-CommandName Test-MAPIConnectivity,Get-MailboxActivityReport,Get-Mailbox,Get-MailboxStatistics,Get-MsolAccountSku,Get-MsolUser,Get-User,Get-DistributionGroup,Get-MobileDeviceStatistics,Get-MobileDevice,Get-MsolCompanyInformation,Get-CsActiveUserReport,Get-CsClientDeviceReport,Get-CsP2PAVTimeReport,Get-CsConferenceReport,Get-CsP2PSessionReport";
			Common.WriteDeviceHistoryEntry(Server.ServerType, Server.Name, "Before  PrereqForOffice365WithCmdlets", Common.LogLevel.Normal);
			//pre-check to see if ADFS /Normal
			ReturnPowerShellObjects results = Common.PrereqForOffice365WithCmdlets(Server.Name, Server.UserName, Server.Password, Server.ServerType, Server.IPAddress, commonEnums.ServerRoles.Windows, cmdlets, Server);
			Common.WriteDeviceHistoryEntry(Server.ServerType, Server.Name, "After  PrereqForOffice365WithCmdlets", Common.LogLevel.Normal);
			if (results.Connected == false)
			{
				if (Server.AuthenticationTest == false)
					Common.makeAlert(false, Server, commonEnums.AlertType.Authentication, ref AllTestsList, "Failed to Authenticate.", Server.Category);
				else
					Common.makeAlert(true, Server, commonEnums.AlertType.Authentication, ref AllTestsList, "Pass.", Server.Category);


				Common.WriteDeviceHistoryEntry(Server.ServerType, Server.Name, " checkServer: PS SESSION IS NULL", Common.LogLevel.Normal);
				Server.Status = "Not Responding";
				Server.StatusCode = "Not Responding";
				//***************************************************Not Responding********************************************//
			}
			else
			{
				Common.makeAlert(true, Server, commonEnums.AlertType.Authentication, ref AllTestsList, "Pass", Server.Category);
			}
			return results;
		}
		#endregion
		#region ADFS
		public void doADFSTest(MonitoredItems.Office365Server myServer, ref TestResults AllTestsList)
		{
			O365RESTApi p = new O365RESTApi();
			p.doADFSCheck(myServer, ref AllTestsList);


		}
		#endregion
		#region dirSync
		private void getDirSyncStats(MonitoredItems.Office365Server myServer, PSCredential creds, string servername, TestResults AllTestsList, ReturnPowerShellObjects PSO)
		{
			try
			{

				if (PSO.PS == null)
					Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "getDirSyncStats : PS IS NULL", Common.LogLevel.Verbose);
				GetWMIPowerShell(myServer, ref PSO, creds, servername, "O365_DirSyncStatus.ps1", true);

				if (PSO.PS == null)
					Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "getDirSyncStats : PS IS NULL", Common.LogLevel.Verbose);
				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "Before Invoke getDirSyncStats: ", Common.LogLevel.Verbose);

				Collection<PSObject> results = PSO.PS.Invoke();
				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "after Invoke getDirSyncStats: ", Common.LogLevel.Verbose);
				foreach (ErrorRecord current in PSO.PS.Streams.Error)
				{
					string strError = "Exception: " + current.Exception.ToString() + ",\r\nInner Exception: " + current.Exception.InnerException;
					Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "Error in getDirSyncStats: " + strError, Common.LogLevel.Verbose);
				}

				foreach (PSObject ps in results)
				{
					//string runProfile="";
					string runProfile = ((System.Management.Automation.PSProperty)(ps.Properties["RunProfile"])).Value.ToString();
					Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "in runProfile getDirSyncStats: " + runProfile, Common.LogLevel.Verbose);
					string runStartTime = ps.Properties["RunStartTime"].Value.ToString();
					Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "in runStartTime getDirSyncStats: " + runStartTime, Common.LogLevel.Verbose);
					string runEndTime = ps.Properties["RunEndTime"].Value.ToString();
					Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "in runEndTime getDirSyncStats: " + runEndTime, Common.LogLevel.Verbose);
					string runStatus = ps.Properties["RunStatus"].Value.ToString();
					string runNumber = ps.Properties["RunNumber"].Value.ToString();
					double dRunNumber = 0;
					if (runNumber != "")
					{
						dRunNumber = Convert.ToDouble(runNumber);
						dRunNumber = dRunNumber - 1;
					}
					Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "in runStatus getDirSyncStats: " + runStatus, Common.LogLevel.Verbose);
					if (runProfile == "Export")
					{
						Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "in DirSyncExportTest getDirSyncStats: ", Common.LogLevel.Verbose);
						if (myServer.DirSyncExportTest)
						{
							if (runStatus.ToLower() == "success" || runStatus.ToLower() == "in-progress")
								Common.makeAlert(true, myServer, commonEnums.AlertType.DirSync_Export, ref AllTestsList, " Dir Sync Last Exported UTC time at: " + runEndTime, myServer.Category);
							else
								Common.makeAlert(true, myServer, commonEnums.AlertType.DirSync_Export, ref AllTestsList, "Dir Sync Failed. Last Successful UTC time at: " + runEndTime, myServer.Category);
						}
						if (runNumber != "")
						{
							string lastTime = getDirSyncLastRun(myServer, creds, servername, AllTestsList, PSO, dRunNumber.ToString());
							//calculate the difference between this run and last run
							DateTime dtThisRun = Convert.ToDateTime(runStartTime);
							DateTime dtLastRun = Convert.ToDateTime(lastTime);
							double iTimeDiff = dtThisRun.Subtract(dtLastRun).TotalMilliseconds;
							DateTime dtNow = DateTime.Now;
							int weekNumber = culture.Calendar.GetWeekOfYear(dtNow, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday);
							string sqlQuery = "Insert into VSS_Statistics.dbo.MicrosoftDailyStats(ServerName,ServerTypeId,Date,StatName,StatValue,WeekNumber,MonthNumber,YearNumber,DayNumber, HourNumber, Details) "
									+ " values('" + myServer.Name + "','" + myServer.ServerTypeId + "',GetDate(),'DirSyncActual" + "@" + nodeName + "'" + " ," + iTimeDiff.ToString() +
								   "," + weekNumber + ", " + dtNow.Month.ToString() + ", " + dtNow.Year.ToString() + ", " + dtNow.Day.ToString() + ", " + dtNow.Hour.ToString() + ", '')";
							AllTestsList.SQLStatements.Add(new SQLstatements() { SQL = sqlQuery, DatabaseName = "VSS_Statistics" });
							string sqlQuery2 = "Insert into VSS_Statistics.dbo.MicrosoftDailyStats(ServerName,ServerTypeId,Date,StatName,StatValue,WeekNumber,MonthNumber,YearNumber,DayNumber, HourNumber, Details) "
									+ " values('" + myServer.Name + "','" + myServer.ServerTypeId + "',GetDate(),'DirSyncEstimated" + "@" + nodeName + "'" + " ," + myServer.DirSyncExportThreshold.ToString() +
								   "," + weekNumber + ", " + dtNow.Month.ToString() + ", " + dtNow.Year.ToString() + ", " + dtNow.Day.ToString() + ", " + dtNow.Hour.ToString() + ", '')";
							AllTestsList.SQLStatements.Add(new SQLstatements() { SQL = sqlQuery2, DatabaseName = "VSS_Statistics" });

						}
					}
					else
					{
						if (myServer.DirSyncImportTest)
						{
							if (runStatus.ToLower() == "success" || runStatus.ToLower() == "in-progress")
								Common.makeAlert(true, myServer, commonEnums.AlertType.DirSync_Import, ref AllTestsList, " Dir Sync Last Imported at UTC time: " + runEndTime, myServer.Category);
							else
								Common.makeAlert(false, myServer, commonEnums.AlertType.DirSync_Import, ref AllTestsList, "Dir Sync Failed. Last Successful UTC time at: " + runEndTime, myServer.Category);
						}
					}



				}
			}
			catch (Exception ex)
			{
				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "Dir Sync Tests: Exception: " + ex.Message.ToString(), Common.LogLevel.Verbose);
			}

		}
		private string getDirSyncLastRun(MonitoredItems.Office365Server myServer, PSCredential creds, string servername, TestResults AllTestsList, ReturnPowerShellObjects PSO, string runNumber)
		{
			string runStartTime = "";

			try
			{
				if (PSO.PS == null)
					Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "getDirSyncStats : PS IS NULL", Common.LogLevel.Verbose);
				System.IO.StreamReader scriptStream = new System.IO.StreamReader(AppDomain.CurrentDomain.BaseDirectory.ToString() + "Scripts\\O365_DirSyncStatus-2.ps1");
				string scriptToExecute = scriptStream.ReadToEnd();
				scriptToExecute = scriptToExecute.Replace("###", runNumber);

				GetWMIPowerShell(myServer, ref PSO, creds, servername, scriptToExecute, false);

				if (PSO.PS == null)
					Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "getDirSyncStats : PS IS NULL", Common.LogLevel.Verbose);
				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "Before Invoke getDirSyncStats: ", Common.LogLevel.Verbose);

				Collection<PSObject> results = PSO.PS.Invoke();
				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "after Invoke getDirSyncStats: ", Common.LogLevel.Verbose);
				foreach (ErrorRecord current in PSO.PS.Streams.Error)
				{
					string strError = "Exception: " + current.Exception.ToString() + ",\r\nInner Exception: " + current.Exception.InnerException;
					Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "Error in getDirSyncStats: " + strError, Common.LogLevel.Verbose);
				}

				foreach (PSObject ps in results)
				{
					//string runProfile="";
					string runProfile = ((System.Management.Automation.PSProperty)(ps.Properties["RunProfile"])).Value.ToString();
					Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "in runProfile getDirSyncStats: " + runProfile, Common.LogLevel.Verbose);

					Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "in last runStartTime getDirSyncStats: " + runStartTime, Common.LogLevel.Verbose);

					if (runProfile != "Export")
					{
						Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "in DirSyncExportTest getDirSyncStats: ", Common.LogLevel.Verbose);

						runStartTime = ps.Properties["RunStartTime"].Value.ToString();
					}
				}
			}
			catch (Exception ex)
			{
				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "Dir Sync Tests: Exception: " + ex.Message.ToString(), Common.LogLevel.Verbose);
			}
			return runStartTime;

		}
		#endregion
		#region MSOL
		public void getMsolAccountSku(MonitoredItems.Office365Server myServer, ref TestResults AllTestsList, ReturnPowerShellObjects powershellobj)
		{
			try
			{
				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "getMsolAccountSku: Starting.", Common.LogLevel.Normal);
				System.Collections.ObjectModel.Collection<PSObject> results = new System.Collections.ObjectModel.Collection<PSObject>();
				//String str = " Get-Mailbox -ResultSize Unlimited | Select Name,Alais,DisplayName,StorageLimitStatus,membertype,servername,ProhibitSendQuota,LastLogonTime";
				//string str = "Get-Command| where {$_.Name -like '*Msol*'}";
				string str = "Get-MsolAccountSku | Select AccountName,SkuPartNumber,ConsumedUnits,WarningUnits,ActiveUnits";

				powershellobj.PS.Commands.Clear();
				powershellobj.PS.Streams.ClearStreams();
				powershellobj.PS.AddScript(str);
				results = powershellobj.PS.Invoke();
				int iConsumedUnits = 0;
				int iWarningUnits = 0;
				int iActiveUnits = 0;
				string AccountName = "";
				string LicenseType = "";
				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "getMailboxes Results: " + results.Count.ToString(), Common.LogLevel.Normal);
				DateTime dtNow = DateTime.Now;
				int weekNumber = culture.Calendar.GetWeekOfYear(dtNow, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday);
				if (results.Count > 0)
				{
					foreach (PSObject ps in results)
					{
						AccountName += ps.Properties["AccountName"].Value == null ? "" : ps.Properties["AccountName"].Value.ToString() + ";";
						LicenseType += ps.Properties["SkuPartNumber"].Value == null ? "" : ps.Properties["SkuPartNumber"].Value.ToString() + ";";

						string ConsumedUnits = ps.Properties["ConsumedUnits"].Value == null ? "" : ps.Properties["ConsumedUnits"].Value.ToString();
						string WarningUnits = ps.Properties["WarningUnits"].Value == null ? "" : ps.Properties["WarningUnits"].Value.ToString();
						string ActiveUnits = ps.Properties["ActiveUnits"].Value == null ? "" : ps.Properties["ActiveUnits"].Value.ToString();
						if (ConsumedUnits != "")
							iConsumedUnits += Convert.ToInt32(ConsumedUnits);
						if (WarningUnits != "")
							iWarningUnits += Convert.ToInt32(WarningUnits);
						if (ActiveUnits != "")
							iActiveUnits += Convert.ToInt32(ActiveUnits);

						Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "getMsolAccountSku Results: AccountName:" + AccountName, Common.LogLevel.Normal);
						Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "getMsolAccountSku Results: ConsumedUnits:" + ConsumedUnits, Common.LogLevel.Normal);
						Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "getMsolAccountSku Results: ActiveUnits:" + ActiveUnits, Common.LogLevel.Normal);



					}
					SQLBuild objSQL = new SQLBuild();
					objSQL.ifExistsSQLSelect = "SELECT * FROM Office365AccountStats WHERE ServerId=" + myServer.ServerId;
					objSQL.onFalseDML = "INSERT INTO Office365AccountStats (ServerId,AccountName, ActiveUnits, ConsumedUnits, WarningUnits, LicenseType, LastUpdatedDate) VALUES ('" + myServer.ServerId + "', '" + AccountName + "',";
					objSQL.onFalseDML += iActiveUnits.ToString() + "," + iConsumedUnits.ToString() + "," + iWarningUnits.ToString() + ",'" + LicenseType + "','" + DateTime.Now + "')";


					objSQL.onTrueDML = "UPDATE Office365AccountStats set AccountName='" + AccountName + "', ActiveUnits=" + iActiveUnits.ToString() + ", ConsumedUnits=" + iConsumedUnits.ToString() + ",WarningUnits=" + iWarningUnits.ToString() + ",LicenseType='" + LicenseType + "',LastUpdatedDate='" + DateTime.Now + "' Where ServerId=" + myServer.ServerId.ToString();
					string sqlQuery = objSQL.GetSQL(objSQL);
					AllTestsList.SQLStatements.Add(new SQLstatements() { SQL = sqlQuery, DatabaseName = "vitalsigns" });

				}

			}
			catch (Exception ex)
			{
				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "getMsolAccountSku: Exception: " + ex.Message.ToString(), Common.LogLevel.Verbose);
				//myServer.ADQueryTest = "Fail";
			}
		}
		public void getMsolCompanyInfo(MonitoredItems.Office365Server myServer, ref TestResults AllTestsList, ReturnPowerShellObjects powershellobj)
		{
			try
			{
				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "getMailboxes: Starting.", Common.LogLevel.Normal);
				System.Collections.ObjectModel.Collection<PSObject> results = new System.Collections.ObjectModel.Collection<PSObject>();
				//String str = " Get-Mailbox -ResultSize Unlimited | Select Name,Alais,DisplayName,StorageLimitStatus,membertype,servername,ProhibitSendQuota,LastLogonTime";
				string str = "Get-MsolCompanyInformation";
				//DisplayName                              : RPR VitalSigns
				//PreferredLanguage                        : en
				//Street                                   : 1 Pleasant Street
				//City                                     : Reading
				//State                                    : MA
				//PostalCode                               : 01867
				//Country                                  : 
				//CountryLetterCode                        : US
				//TelephoneNumber                          : 781-608-4060
				//MarketingNotificationEmails              : {}
				//TechnicalNotificationEmails              : {aforbes@rprwyatt.com}
				//SelfServePasswordResetEnabled            : True
				//UsersPermissionToCreateGroupsEnabled     : True
				//UsersPermissionToCreateLOBAppsEnabled    : True
				//UsersPermissionToReadOtherUsersEnabled   : True
				//UsersPermissionToUserConsentToAppEnabled : True
				//DirectorySynchronizationEnabled          : False
				//LastDirSyncTime                          : 
				//LastPasswordSyncTime                     : 
				//PasswordSynchronizationEnabled           : False

				//powershellobj.PS.Commands.Clear();
				//powershellobj.PS.Streams.ClearStreams();
				powershellobj.PS.AddScript(str);
				results = powershellobj.PS.Invoke();

				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "getMailboxes Results: " + results.Count.ToString(), Common.LogLevel.Normal);
				DateTime dtNow = DateTime.Now;
				int weekNumber = culture.Calendar.GetWeekOfYear(dtNow, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday);
				if (results.Count > 0)
				{
					foreach (PSObject ps in results)
					{
						string DisplayName = ps.Properties["DisplayName"].Value == null ? "" : ps.Properties["DisplayName"].Value.ToString();
						string PreferredLanguage = ps.Properties["PreferredLanguage"].Value == null ? "" : ps.Properties["PreferredLanguage"].Value.ToString();
						string Street = ps.Properties["Street"].Value == null ? "" : ps.Properties["Street"].Value.ToString();
						string City = ps.Properties["City"].Value == null ? "" : ps.Properties["City"].Value.ToString();
						string State = ps.Properties["State"].Value == null ? "" : ps.Properties["State"].Value.ToString();
						string Country = ps.Properties["Country"].Value == null ? "" : ps.Properties["Country"].Value.ToString();
						string PostalCode = ps.Properties["PostalCode"].Value == null ? "" : ps.Properties["PostalCode"].Value.ToString();
						string TelephoneNumber = ps.Properties["TelephoneNumber"].Value == null ? "" : ps.Properties["TelephoneNumber"].Value.ToString();
						//string[] email = ps.Properties["TechnicalNotificationEmails"].Value.ToString().ToArray(1);

						//foreach (string s in ps.Properties["TechnicalNotificationEmails"].Value)
						//{
						//    email += s;

						//}
						//string TechnicalNotificationEmails = ps.Properties["TechnicalNotificationEmails"].Value == null ? "" : ps.Properties["TechnicalNotificationEmails"].Value.ToString();
						string TechnicalNotificationEmails = "";
						Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "getMsolCompanyInfo Results: DisplayName:" + DisplayName, Common.LogLevel.Normal);
						Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "getMsolCompanyInfo Results: TechnicalNotificationEmails:" + TechnicalNotificationEmails, Common.LogLevel.Normal);
						Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "getMsolCompanyInfo Results: Street:" + Street, Common.LogLevel.Normal);

						SQLBuild objSQL = new SQLBuild();
						objSQL.ifExistsSQLSelect = "SELECT * FROM Office365AccountStats WHERE ServerId=" + myServer.ServerId;
						objSQL.onFalseDML = "INSERT INTO Office365AccountStats (ServerId,CompanyDisplayName, PreferredLanguage, Street, City, State, Country,PostalCode,Telephone,TechnicalNotificationEmails,LastUpdatedDate) VALUES ('" + myServer.ServerId + "', '" + DisplayName + "','";
						objSQL.onFalseDML += PreferredLanguage + "','" + Street + "','" + City + "','" + State + "','" + Country + "','" + PostalCode + "','" + TelephoneNumber + "','" + TechnicalNotificationEmails + "','" + DateTime.Now + "')";


						objSQL.onTrueDML = "UPDATE Office365AccountStats set CompanyDisplayName='" + DisplayName + "', PreferredLanguage='" + PreferredLanguage + "', Street='" + Street + "',City='" + City + "',State='" + State + "',PostalCode='" + PostalCode + "',Telephone='" + TelephoneNumber + "',TechnicalNotificationEmails='" + TechnicalNotificationEmails + "',LastUpdatedDate='" + DateTime.Now + "' Where ServerId=" + myServer.ServerId.ToString();
						string sqlQuery = objSQL.GetSQL(objSQL);
						AllTestsList.SQLStatements.Add(new SQLstatements() { SQL = sqlQuery, DatabaseName = "vitalsigns" });

					}

				}

			}
			catch (Exception ex)
			{
				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "getMsolCompanyInfo: Exception: " + ex.Message.ToString(), Common.LogLevel.Verbose);
				//myServer.ADQueryTest = "Fail";
			}
		}
		#endregion
		#region mailBox
		public void getMailboxes(MonitoredItems.Office365Server myServer, ref TestResults AllTestsList, ReturnPowerShellObjects powershellobj)
		{
			try
			{
				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "getMailboxes: Starting.", Common.LogLevel.Normal);
				System.Collections.ObjectModel.Collection<PSObject> results = new System.Collections.ObjectModel.Collection<PSObject>();
				//String str = " Get-Mailbox -ResultSize Unlimited | Select Name,Alais,DisplayName,StorageLimitStatus,membertype,servername,ProhibitSendQuota,LastLogonTime";
				string str = "Get-Mailbox -ResultSize Unlimited | Get-MailboxStatistics | Select DisplayName,Database,TotalItemSize,ItemCount,StorageLimitStatus,ServerName,LastLogonTime,LastLogoffTime";
				//Get - Mailbox | select *
				//Get-Mailbox | select RecipientTypeDetails,ProhibitSendQuota,ProhibitSendReceiveQuota,IssueWarningQuota,IsInactiveMailbox
				powershellobj.PS.Commands.Clear();
				powershellobj.PS.Streams.ClearStreams();
				powershellobj.PS.AddScript(str);
				results = powershellobj.PS.Invoke();
				//AllTestsList.SQLStatements.Add(new SQLstatements() { SQL = "DELETE FROM ExchangeMailFiles WHERE Server='" + myServer.Name + "'", DatabaseName = "VSS_Statistics" });
				//AllTestsList.SQLStatements.Add(new SQLstatements() { SQL = "DELETE FROM O365AdditionalMailDetails WHERE Server='" + myServer.Name + "'", DatabaseName = "VSS_Statistics" });
				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "getMailboxes Results: " + results.Count.ToString(), Common.LogLevel.Normal);
				DateTime dtNow = DateTime.Now;
				int weekNumber = culture.Calendar.GetWeekOfYear(dtNow, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday);
				if (results.Count > 0)
				{
					foreach (PSObject ps in results)
					{
						string DisplayName = ps.Properties["DisplayName"].Value == null ? "" : ps.Properties["DisplayName"].Value.ToString();
						string Database = ps.Properties["Database"].Value == null ? "" : ps.Properties["Database"].Value.ToString();
						//string IssueWarningQuota = "Unlimited"; //ps.Properties["IssueWarningQuota"].Value == null ? "Unlimited" : ps.Properties["IssueWarningQuota"].Value.ToString();
						//string ProhibitSendQuota = "Unlimited";// ps.Properties["ProhibitSendQuota"].Value == null ? "Unlimited" : ps.Properties["ProhibitSendQuota"].Value.ToString();
						//string ProhibitSendReceiveQuota = "Unlimited";//ps.Properties["ProhibitSendReceiveQuota"].Value == null ? "Unlimited" : ps.Properties["ProhibitSendReceiveQuota"].Value.ToString();
						string totalItemSize = ps.Properties["TotalItemSize"].Value == null ? "0" : ps.Properties["TotalItemSize"].Value.ToString();
						try
						{
							if (totalItemSize != "Unlimited")
							{
								if (totalItemSize.IndexOf("MB") > 0)
									totalItemSize = totalItemSize.Substring(0, totalItemSize.IndexOf("MB")).Trim();
								if (totalItemSize.IndexOf("KB") > 0)
								{
									totalItemSize = totalItemSize.Substring(0, totalItemSize.IndexOf("KB")).Trim();
									totalItemSize = (Convert.ToDouble(totalItemSize) / 1000).ToString();
								}
								if (totalItemSize.IndexOf("GB") > 0)
								{
									totalItemSize = totalItemSize.Substring(0, totalItemSize.IndexOf("GB")).Trim();
									totalItemSize = (Convert.ToDouble(totalItemSize) * 1000).ToString();
								}
							}
						}
						catch
						{
						}

						//string TotalItemSize = ps.Properties["TotalItemSize"].Value == null ? "0" : ps.Properties["TotalItemSize"].Value.ToString();
						string ItemCount = ps.Properties["ItemCount"].Value == null ? "0" : ps.Properties["ItemCount"].Value.ToString();
						string StorageLimitStatus = ps.Properties["StorageLimitStatus"].Value == null ? "" : ps.Properties["StorageLimitStatus"].Value.ToString();
						string ServerName = ps.Properties["ServerName"].Value == null ? "" : ps.Properties["ServerName"].Value.ToString();
						string LastLogonTime = ps.Properties["LastLogonTime"].Value == null ? "" : ps.Properties["LastLogonTime"].Value.ToString();
						string LastLogoffTime = ps.Properties["LastLogoffTime"].Value == null ? "" : ps.Properties["LastLogoffTime"].Value.ToString();


						SQLBuild objSQL = new SQLBuild();
						objSQL.ifExistsSQLSelect = "SELECT * FROM ExchangeMailFiles WHERE Server='" + myServer.Name + "' AND DisplayName='" + DisplayName + "'";
						objSQL.onFalseDML = "INSERT INTO ExchangeMailFiles ([ScanDate],[Database],[DisplayName],[Server],[TotalItemSizeInMB],[ItemCount],[StorageLimitStatus]) VALUES " +
							"('" + DateTime.Now + "','" + Database + "','" + DisplayName + "','" + myServer.Name + "'," + totalItemSize + "," + ItemCount + ",'" + StorageLimitStatus + "')";




						objSQL.onTrueDML = "UPDATE ExchangeMailFiles set [ScanDate]='" + DateTime.Now + "',[Database]='" + Database + "',[TotalItemSizeInMB]=" + totalItemSize +
							",[ItemCount]=" + ItemCount + ",[StorageLimitStatus]='"+ StorageLimitStatus +"' Where Server='" + myServer.Name.ToString() + "' AND DisplayName='" + DisplayName + "'";
						string sqlQuery = objSQL.GetSQL(objSQL);
						AllTestsList.SQLStatements.Add(new SQLstatements() { SQL = sqlQuery, DatabaseName = "VSS_Statistics" });

						//string sqlQuery = "INSERT INTO ExchangeMailFiles ([ScanDate],[Database],[DisplayName],[Server],[TotalItemSizeInMB],[ItemCount],[StorageLimitStatus]) VALUES " +
						//    "('" + DateTime.Now + "','" + Database + "','" + DisplayName + "','" + myServer.Name + "','" + totalItemSize + "','" + ItemCount + "','" + StorageLimitStatus + "')";


						//AllTestsList.SQLStatements.Add(new SQLstatements() { SQL = sqlQuery, DatabaseName = "VSS_Statistics" });

						// Details
						//sqlQuery = "INSERT INTO dbo.O365AdditionalMailDetails ([Server],[LastLogonTime],[LastLoggoffTime],[DisplayName]) VALUES " +
						//    "('" + myServer.Name + "','" + LastLogonTime + "','" + LastLogoffTime + "','" + DisplayName + "')";


						//AllTestsList.SQLStatements.Add(new SQLstatements() { SQL = sqlQuery, DatabaseName = "VSS_Statistics" });

						SQLBuild objSQL2 = new SQLBuild();
						objSQL2.ifExistsSQLSelect = "SELECT * FROM O365AdditionalMailDetails WHERE Server='" + myServer.Name + "' AND DisplayName='" + DisplayName + "'";
						objSQL2.onFalseDML = "INSERT INTO dbo.O365AdditionalMailDetails ([Server],[LastLogonTime],[LastLoggoffTime],[DisplayName]) VALUES " +
							"('" + myServer.Name + "','" + LastLogonTime + "','" + LastLogoffTime + "','" + DisplayName + "')";


						objSQL2.onTrueDML = "UPDATE dbo.O365AdditionalMailDetails set [LastLogonTime]='" + LastLogonTime + "',[LastLoggoffTime]='" + LastLogoffTime + "' Where Server='" + myServer.Name.ToString() + "' AND DisplayName='" + DisplayName + "'";
						string sqlQuery2 = objSQL2.GetSQL(objSQL2);
						AllTestsList.SQLStatements.Add(new SQLstatements() { SQL = sqlQuery2, DatabaseName = "VSS_Statistics" });



					}

				}
				else
				{
					//myServer.ADQueryTest = "Fail";
					//Common.makeAlert(true, myServer, commonEnums.AlertType.AD_Query_Latency, ref AllTestsList, myServer.ServerType);
				}
			}
			catch (Exception ex)
			{
				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "getMailboxes: Exception: " + ex.Message.ToString(), Common.LogLevel.Verbose);
				//myServer.ADQueryTest = "Fail";
				//Common.makeAlert(false, myServer, commonEnums.AlertType.Mailbox_Database_Size, ref AllTestsList, myServer.ServerType);
			}
		}
		public void getMailboxeDetails(MonitoredItems.Office365Server myServer, ref TestResults AllTestsList, ReturnPowerShellObjects powershellobj)
		{
			try
			{
				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "getMailboxes: Starting.", Common.LogLevel.Normal);
				System.Collections.ObjectModel.Collection<PSObject> results = new System.Collections.ObjectModel.Collection<PSObject>();
				//String str = " Get-Mailbox -ResultSize Unlimited | Select Name,Alais,DisplayName,StorageLimitStatus,membertype,servername,ProhibitSendQuota,LastLogonTime";
				//string str = "Get-Mailbox | Get-MailboxStatistics | Select DisplayName,Database,TotalItemSize,ItemCount,StorageLimitStatus,ServerName,LastLogonTime,LastLogoffTime";
				//Get - Mailbox | select *
				String str = "Get-Mailbox -ResultSize Unlimited | select DisplayName,RecipientTypeDetails,ProhibitSendQuota,ProhibitSendReceiveQuota,IssueWarningQuota,IsInactiveMailbox,Database,ServerName";
				powershellobj.PS.Commands.Clear();
				powershellobj.PS.Streams.ClearStreams();
				powershellobj.PS.AddScript(str);
				results = powershellobj.PS.Invoke();
				//AllTestsList.SQLStatements.Add(new SQLstatements() { SQL = "DELETE FROM ExchangeMailFiles WHERE Server='" + myServer.Name + "'", DatabaseName = "VSS_Statistics" });
				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "getMailboxes Results: " + results.Count.ToString(), Common.LogLevel.Normal);
				DateTime dtNow = DateTime.Now;
				int weekNumber = culture.Calendar.GetWeekOfYear(dtNow, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday);
				if (results.Count > 0)
				{
					foreach (PSObject ps in results)
					{
						string DisplayName = ps.Properties["DisplayName"].Value == null ? "" : ps.Properties["DisplayName"].Value.ToString();
						string Database = ps.Properties["Database"].Value == null ? "" : ps.Properties["Database"].Value.ToString();
						string IssueWarningQuota = ps.Properties["IssueWarningQuota"].Value == null ? "Unlimited" : ps.Properties["IssueWarningQuota"].Value.ToString();
						if (IssueWarningQuota != "Unlimited")
							IssueWarningQuota = IssueWarningQuota.Substring(0, IssueWarningQuota.IndexOf("GB"));
						string ProhibitSendQuota = ps.Properties["ProhibitSendQuota"].Value == null ? "Unlimited" : ps.Properties["ProhibitSendQuota"].Value.ToString();
						if (ProhibitSendQuota != "Unlimited")
							ProhibitSendQuota = ProhibitSendQuota.Substring(0, ProhibitSendQuota.IndexOf("GB"));
						string ProhibitSendReceiveQuota = ps.Properties["ProhibitSendReceiveQuota"].Value == null ? "Unlimited" : ps.Properties["ProhibitSendReceiveQuota"].Value.ToString();
						if (ProhibitSendReceiveQuota != "Unlimited")
							ProhibitSendReceiveQuota = ProhibitSendReceiveQuota.Substring(0, ProhibitSendReceiveQuota.IndexOf("GB"));
						string RecipientTypeDetails = ps.Properties["RecipientTypeDetails"].Value == null ? "" : ps.Properties["RecipientTypeDetails"].Value.ToString();
						bool IsInactiveMailbox = !Convert.ToBoolean(ps.Properties["IsInactiveMailbox"].Value == null ? "False" : ps.Properties["IsInactiveMailbox"].Value.ToString());

						//string TotalItemSize = ps.Properties["ItemCount"].Value == null ? "0" : ps.Properties["ItemCount"].Value.ToString();
						//string ItemCount = ps.Properties["ItemCount"].Value == null ? "0" : ps.Properties["ItemCount"].Value.ToString();
						//string StorageLimitStatus = ps.Properties["StorageLimitStatus"].Value == null ? "" : ps.Properties["StorageLimitStatus"].Value.ToString();
						string ServerName = ps.Properties["ServerName"].Value == null ? "" : ps.Properties["ServerName"].Value.ToString();

						//string sqlQuery = "INSERT INTO ExchangeMailFiles ([ScanDate],[Database],[DisplayName],[Server],[IssueWarningQuota],[ProhibitSendQuota],[ProhibitSendReceiveQuota]) VALUES " +
						//"('" + DateTime.Now + "','" + Database + "','" + DisplayName + "','" + myServer.Name + "','" + IssueWarningQuota + "','" + ProhibitSendQuota + "','" + ProhibitSendReceiveQuota + "')";


						//AllTestsList.SQLStatements.Add(new SQLstatements() { SQL = sqlQuery, DatabaseName = "VSS_Statistics" });

						SQLBuild objSQL = new SQLBuild();
						objSQL.ifExistsSQLSelect = "SELECT * FROM ExchangeMailFiles WHERE Server='" + myServer.Name + "' AND DisplayName='" + DisplayName + "'";
						objSQL.onFalseDML = "INSERT INTO ExchangeMailFiles ([ScanDate],[Database],[DisplayName],[IssueWarningQuota],[ProhibitSendQuota],[ProhibitSendReceiveQuota],[Server]) VALUES " +
							"('" + DateTime.Now + "','" + Database + "','" + DisplayName + "','" + IssueWarningQuota + "','" + ProhibitSendQuota + "','" + ProhibitSendReceiveQuota + "','" + myServer.Name + "')";




						objSQL.onTrueDML = "UPDATE ExchangeMailFiles set IssueWarningQuota='" + IssueWarningQuota + "',ProhibitSendQuota='" + ProhibitSendQuota + "',ProhibitSendReceiveQuota='" + ProhibitSendReceiveQuota +
							"',ScanDate='" + DateTime.Now + "' Where Server='" + myServer.Name.ToString() + "' AND DisplayName='" + DisplayName + "'";
						string sqlQuery = objSQL.GetSQL(objSQL);
						AllTestsList.SQLStatements.Add(new SQLstatements() { SQL = sqlQuery, DatabaseName = "VSS_Statistics" });

						// Details
						SQLBuild objSQL2 = new SQLBuild();
						objSQL2.ifExistsSQLSelect = "SELECT * FROM O365AdditionalMailDetails WHERE Server='" + myServer.Name + "' AND DisplayName='" + DisplayName + "'";
						objSQL2.onFalseDML = "INSERT INTO dbo.O365AdditionalMailDetails ([Server],[MailBoxType],[IsActive],[DisplayName]) VALUES " +
							"('" + myServer.Name + "','" + RecipientTypeDetails + "','" + IsInactiveMailbox + "','" + DisplayName + "')";




						objSQL2.onTrueDML = "UPDATE dbo.O365AdditionalMailDetails set MailBoxType='" + RecipientTypeDetails + "',IsActive='" + IsInactiveMailbox + "' Where Server='" + myServer.Name + "' AND DisplayName='" + DisplayName + "'";
						string sqlQuery2 = objSQL2.GetSQL(objSQL2);
						AllTestsList.SQLStatements.Add(new SQLstatements() { SQL = sqlQuery2, DatabaseName = "VSS_Statistics" });


					}

				}
				else
				{
					//myServer.ADQueryTest = "Fail";
					//Common.makeAlert(true, myServer, commonEnums.AlertType.AD_Query_Latency, ref AllTestsList, myServer.ServerType);
				}
			}
			catch (Exception ex)
			{
				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "getMailboxeDetails: Exception: " + ex.Message.ToString(), Common.LogLevel.Verbose);
				//myServer.ADQueryTest = "Fail";
				//Common.makeAlert(false, myServer, commonEnums.AlertType.Mailbox_Database_Size, ref AllTestsList, myServer.ServerType);
			}
		}
		public void getMailboxActivity(MonitoredItems.Office365Server myServer, ref TestResults AllTestsList, ReturnPowerShellObjects powershellobj)
		{
			try
			{
				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "getMsolAccountSku: Starting.", Common.LogLevel.Normal);
				System.Collections.ObjectModel.Collection<PSObject> results = new System.Collections.ObjectModel.Collection<PSObject>();
				//String str = " Get-Mailbox -ResultSize Unlimited | Select Name,Alais,DisplayName,StorageLimitStatus,membertype,servername,ProhibitSendQuota,LastLogonTime";
				//string str = "Get-Command| where {$_.Name -like '*Msol*'}";
				string str = "Get-MailboxActivityReport ";

				powershellobj.PS.Commands.Clear();
				powershellobj.PS.Streams.ClearStreams();
				powershellobj.PS.AddScript(str);
				results = powershellobj.PS.Invoke();

				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "getMailboxes Results: " + results.Count.ToString(), Common.LogLevel.Normal);
				DateTime dtNow = DateTime.Now;
				int weekNumber = culture.Calendar.GetWeekOfYear(dtNow, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday);
				if (results.Count > 0)
				{
					foreach (PSObject ps in results)
					{
						string TotalActiveUserMailBoxes = ps.Properties["TotalNumberOfActiveMailboxes"].Value == null ? "" : ps.Properties["TotalNumberOfActiveMailboxes"].Value.ToString();
						Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "getMsolAccountSku Results: TotalActiveUserMailBoxes:" + TotalActiveUserMailBoxes, Common.LogLevel.Normal);
						string sqlQuery = "UPDATE Office365AccountStats set TotalActiveUserMailBoxes=" + TotalActiveUserMailBoxes + ",LastUpdatedDate='" + DateTime.Now + "' Where ServerId=" + myServer.ServerId.ToString();
						AllTestsList.SQLStatements.Add(new SQLstatements() { SQL = sqlQuery, DatabaseName = "vitalsigns" });
						break;
					}

				}

			}
			catch (Exception ex)
			{
				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "getMsolAccountSku Results: Exception: " + ex.Message.ToString(), Common.LogLevel.Verbose);
			}
		}
		#endregion
		#region mobileDevices
		public void getMobileUsers(MonitoredItems.Office365Server myServer, ref TestResults AllTestsList, ReturnPowerShellObjects powershellobj)
		{
			try
			{
				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "getMobileUsers: Starting.", Common.LogLevel.Normal);
				System.Collections.ObjectModel.Collection<PSObject> results = new System.Collections.ObjectModel.Collection<PSObject>();
				//String str = " Get-Mailbox -ResultSize Unlimited | Select Name,Alais,DisplayName,StorageLimitStatus,membertype,servername,ProhibitSendQuota,LastLogonTime";
				//string str = "Get-MobileDevice";
				//string str = "$MB = Get-MobileDevice -Resultsize unlimited " + "\n";
				//str += "$MB | foreach {Get-MobileDeviceStatistics $_.identity} | Select-Object UserDisplayName,FirstSyncTime,LastPolicyUpdateTime,LastSyncAttemptTime,LastSuccessSync,DeviceType,DeviceID,DeviceModel,DeviceFriendlyName,DeviceOS,DeviceOSLanguage,Identity,DeviceAccessState,NumberOfFoldersSynced ,DeviceOSLanguage,DevicePolicyApplied,Status,DeviceUserAgent,DeviceAccessState,DeviceActiveSyncVersion,DeviceMobileOperator" + "\n" ;
				string str = "Get-MobileDevice | Select-Object UserDisplayName,FirstSyncTime,LastPolicyUpdateTime,LastSyncAttemptTime,LastSuccessSync,DeviceType,DeviceID,DeviceModel,DeviceFriendlyName,DeviceOS,Identity,DeviceAccessState,NumberOfFoldersSynced ,DeviceOSLanguage,DevicePolicyApplied,Status,DeviceUserAgent,DeviceActiveSyncVersion,DeviceMobileOperator";
				//str += " $MB | foreach {Get-MobileDevice |Select UserDisplayName} | Select-Object *  ";
				//Get-MobileDeviceStatistics -Identity joe";
				//powershellobj.PS.Commands.Clear();
				//powershellobj.PS.Streams.ClearStreams();
				powershellobj.PS.AddScript(str);
				results = powershellobj.PS.Invoke();

				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "getMobileUsers Results: " + results.Count.ToString(), Common.LogLevel.Normal);
				DateTime dtNow = DateTime.Now;
				int weekNumber = culture.Calendar.GetWeekOfYear(dtNow, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday);
				string strSQL = "";
				string strSQLValues = "";
				if (results.Count > 0)
				{
					foreach (PSObject ps in results)
					{
						//string DeviceId = ps.Properties["DeviceId"].Value == null ? "" : ps.Properties["DeviceId"].Value.ToString();
						//string DeviceMobileOperator = ps.Properties["DeviceMobileOperator"].Value == null ? "" : ps.Properties["DeviceMobileOperator"].Value.ToString();
						//string DeviceOS = ps.Properties["DeviceOS"].Value == null ? "" : ps.Properties["DeviceOS"].Value.ToString();
						//string DeviceModel = ps.Properties["DeviceModel"].Value == null ? "" : ps.Properties["DeviceModel"].Value.ToString();
						//string DeviceTelephoneNumber = ps.Properties["DeviceTelephoneNumber"].Value == null ? "" : ps.Properties["DeviceTelephoneNumber"].Value.ToString();
						//string DeviceType = ps.Properties["DeviceType"].Value == null ? "" : ps.Properties["DeviceType"].Value.ToString();
						//string ClientVersion = ps.Properties["ClientVersion"].Value == null ? "" : ps.Properties["ClientVersion"].Value.ToString();
						//string FriendlyName = ps.Properties["FriendlyName"].Value == null ? "" : ps.Properties["FriendlyName"].Value.ToString();
						ActiveSyncDevice myDevice = new ActiveSyncDevice();
						myDevice.User = ps.Properties["UserDisplayName"].Value == null ? "" : ps.Properties["UserDisplayName"].Value.ToString();

						myDevice.DeviceID = ps.Properties["DeviceID"].Value == null ? "" : ps.Properties["DeviceID"].Value.ToString();
						myDevice.DeviceMobileOperator = ps.Properties["DeviceMobileOperator"].Value == null ? "" : ps.Properties["DeviceMobileOperator"].Value.ToString();
						myDevice.DeviceActiveSyncVersion = ps.Properties["DeviceActiveSyncVersion"].Value == null ? "" : ps.Properties["DeviceActiveSyncVersion"].Value.ToString();
						myDevice.DeviceFriendlyName = ps.Properties["DeviceFriendlyName"].Value == null ? "" : ps.Properties["DeviceFriendlyName"].Value.ToString();
						myDevice.DeviceOSLanguage = ps.Properties["DeviceOSLanguage"].Value == null ? "" : ps.Properties["DeviceOSLanguage"].Value.ToString();
						myDevice.DeviceModel = ps.Properties["DeviceModel"].Value == null ? "" : ps.Properties["DeviceModel"].Value.ToString();
						myDevice.DeviceType = ps.Properties["DeviceType"].Value == null ? "" : ps.Properties["DeviceType"].Value.ToString();
						myDevice.DeviceOS = ps.Properties["DeviceOS"].Value == null ? "" : ps.Properties["DeviceOS"].Value.ToString();
						//myDevice.LastSuccessSync = ps.Properties["LastSuccessSync"].Value == null ? "" : ps.Properties["LastSuccessSync"].Value.ToString();
						myDevice.DeviceUserAgent = ps.Properties["DeviceType"].Value == null ? "" : ps.Properties["DeviceType"].Value.ToString();
						myDevice.DevicePolicyApplied = ps.Properties["DevicePolicyApplied"].Value == null ? "" : ps.Properties["DevicePolicyApplied"].Value.ToString();

						// Status returns 'DeviceOk', which is a bit nerdy
						myDevice.Status = ps.Properties["Status"].Value == null ? "" : ps.Properties["Status"].Value.ToString();
						if (myDevice.Status == "DeviceOk") { myDevice.Status = "OK"; }

						myDevice.DeviceUserAgent = ps.Properties["DeviceUserAgent"].Value == null ? "" : ps.Properties["DeviceUserAgent"].Value.ToString();
						// myDevice.Identity = ps.Properties["Identity"].Value == null ? "" : ps.Properties["Identity"].Value.ToString();
						myDevice.DeviceAccessState = ps.Properties["DeviceAccessState"].Value == null ? "" : ps.Properties["DeviceAccessState"].Value.ToString();
						if (myDevice.DeviceAccessState == "Allowed") { myDevice.DeviceAccessState = "Allow"; }

						//Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "getMobileUsers Results: DeviceId:" + DeviceId, Common.LogLevel.Normal);
						//Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "getMobileUsers Results: DeviceMobileOperator:" + DeviceMobileOperator, Common.LogLevel.Normal);
						//Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "getMobileUsers Results: DeviceOS:" + DeviceOS, Common.LogLevel.Normal);
						strSQL = "IF EXISTS ( " +
									"SELECT * FROM [vitalsigns].[dbo].[Traveler_Devices] where " +
									"DeviceID='" + myDevice.DeviceID + "') " +
									"begin " +

									"update [vitalsigns].[dbo].[Traveler_Devices] set " +
									"[UserName]='" + myDevice.User + "',  [Security_Policy]='" + myDevice.DevicePolicyApplied + "', [DeviceName]='" + myDevice.DeviceModel + "', " +
									" [OS_Type]= '" + myDevice.DeviceOS + "',[OS_Type_Min]= '" + myDevice.DeviceOS + "', [Client_Build]='" + myDevice.DeviceActiveSyncVersion + "', [device_type]='" + myDevice.DeviceType + "', " +
									"[ServerName]='" + myServer.Name + "', [Access]='" + myDevice.DeviceAccessState + "', [DeviceID]='" + myDevice.DeviceID + "', [LastUpdated]='" + DateTime.Now + "' " +
									"where DeviceID='" + myDevice.DeviceID + "' " +

									"end  else " +

									"begin " +
									"INSERT INTO vitalsigns.dbo.Traveler_Devices (UserName, Security_Policy, DeviceName, LastSyncTime, OS_Type, Client_Build, device_type, ServerName, Access, DeviceID, LastUpdated, IsActive,OS_Type_Min) " +
									" VALUES ('" + myDevice.User + "', '" + myDevice.DevicePolicyApplied + "', '" + myDevice.DeviceModel + "',  NULL, '" + myDevice.DeviceOS + "', '" +
									myDevice.DeviceActiveSyncVersion + "', '" + myDevice.DeviceType + "', '" + myServer.Name + "', '" + myDevice.DeviceAccessState + "', '" + myDevice.DeviceID + "', '" + DateTime.Now + "', 1,'" + myDevice.DeviceOS + "') end";

						AllTestsList.SQLStatements.Add(new SQLstatements() { SQL = strSQL + strSQLValues });
					}

				}

			}
			catch (Exception ex)
			{
				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "getMobileUsers Results: Exception: " + ex.Message.ToString(), Common.LogLevel.Verbose);
				//myServer.ADQueryTest = "Fail";
				//Common.makeAlert(false, myServer, commonEnums.AlertType., ref AllTestsList, myServer.ServerType);
			}
		}

		public void getMobileUsersHourly(MonitoredItems.Office365Server myServer, ref TestResults AllTestsList, ReturnPowerShellObjects powershellobj)
		{
			try
			{
				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "getMobileUsersHourly: Starting.", Common.LogLevel.Normal);
				System.Collections.ObjectModel.Collection<PSObject> results = new System.Collections.ObjectModel.Collection<PSObject>();
				//String str = " Get-Mailbox -ResultSize Unlimited | Select Name,Alais,DisplayName,StorageLimitStatus,membertype,servername,ProhibitSendQuota,LastLogonTime";
				//string str = "Get-MobileDevice";
				string str = "$MB = Get-MobileDevice -Resultsize unlimited " + "\n";
				str += "$MB | foreach {Get-MobileDeviceStatistics $_.identity} | Select-Object UserDisplayName,FirstSyncTime,LastPolicyUpdateTime,LastSyncAttemptTime,LastSuccessSync,DeviceType,DeviceID,DeviceModel,DeviceFriendlyName,DeviceOS,DeviceOSLanguage,Identity,DeviceAccessState,NumberOfFoldersSynced ,DevicePolicyApplied,Status,DeviceUserAgent,DeviceActiveSyncVersion,DeviceMobileOperator" + "\n" ;
				//string str = "Get-MobileDevice | Select-Object UserDisplayName,FirstSyncTime,LastPolicyUpdateTime,LastSyncAttemptTime,LastSuccessSync,DeviceType,DeviceID,DeviceModel,DeviceFriendlyName,DeviceOS,DeviceOSLanguage,Identity,DeviceAccessState,NumberOfFoldersSynced ,DeviceOSLanguage,DevicePolicyApplied,Status,DeviceUserAgent,DeviceAccessState,DeviceActiveSyncVersion,DeviceMobileOperator";
				//str += " $MB | foreach {Get-MobileDevice |Select UserDisplayName} | Select-Object *  ";
				//Get-MobileDeviceStatistics -Identity joe";
				//powershellobj.PS.Commands.Clear();
				//powershellobj.PS.Streams.ClearStreams();
				powershellobj.PS.AddScript(str);
				results = powershellobj.PS.Invoke();

				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "getMobileUsersHourly Results: " + results.Count.ToString(), Common.LogLevel.Normal);
				DateTime dtNow = DateTime.Now;
				int weekNumber = culture.Calendar.GetWeekOfYear(dtNow, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday);
				string strSQL = "";
				string strSQLValues = "";
				if (results.Count > 0)
				{
					foreach (PSObject ps in results)
					{
						//string DeviceId = ps.Properties["DeviceId"].Value == null ? "" : ps.Properties["DeviceId"].Value.ToString();
						//string DeviceMobileOperator = ps.Properties["DeviceMobileOperator"].Value == null ? "" : ps.Properties["DeviceMobileOperator"].Value.ToString();
						//string DeviceOS = ps.Properties["DeviceOS"].Value == null ? "" : ps.Properties["DeviceOS"].Value.ToString();
						//string DeviceModel = ps.Properties["DeviceModel"].Value == null ? "" : ps.Properties["DeviceModel"].Value.ToString();
						//string DeviceTelephoneNumber = ps.Properties["DeviceTelephoneNumber"].Value == null ? "" : ps.Properties["DeviceTelephoneNumber"].Value.ToString();
						//string DeviceType = ps.Properties["DeviceType"].Value == null ? "" : ps.Properties["DeviceType"].Value.ToString();
						//string ClientVersion = ps.Properties["ClientVersion"].Value == null ? "" : ps.Properties["ClientVersion"].Value.ToString();
						//string FriendlyName = ps.Properties["FriendlyName"].Value == null ? "" : ps.Properties["FriendlyName"].Value.ToString();
						ActiveSyncDevice myDevice = new ActiveSyncDevice();
						myDevice.User = ps.Properties["Identity"].Value == null ? "" : ps.Properties["Identity"].Value.ToString();
						try
						{
							myDevice.User = myDevice.User.Split('/')[3];
						}
						catch { }

						myDevice.DeviceID = ps.Properties["DeviceID"].Value == null ? "" : ps.Properties["DeviceID"].Value.ToString();
						myDevice.DeviceMobileOperator = ps.Properties["DeviceMobileOperator"].Value == null ? "" : ps.Properties["DeviceMobileOperator"].Value.ToString();
						myDevice.DeviceActiveSyncVersion = ps.Properties["DeviceActiveSyncVersion"].Value == null ? "" : ps.Properties["DeviceActiveSyncVersion"].Value.ToString();
						myDevice.DeviceFriendlyName = ps.Properties["DeviceFriendlyName"].Value == null ? "" : ps.Properties["DeviceFriendlyName"].Value.ToString();
						myDevice.DeviceOSLanguage = ps.Properties["DeviceOSLanguage"].Value == null ? "" : ps.Properties["DeviceOSLanguage"].Value.ToString();
						myDevice.DeviceModel = ps.Properties["DeviceModel"].Value == null ? "" : ps.Properties["DeviceModel"].Value.ToString();
						myDevice.DeviceType = ps.Properties["DeviceType"].Value == null ? "" : ps.Properties["DeviceType"].Value.ToString();
						myDevice.DeviceOS = ps.Properties["DeviceOS"].Value == null ? "" : ps.Properties["DeviceOS"].Value.ToString();
						myDevice.LastSuccessSync = ps.Properties["LastSuccessSync"].Value == null ? "" : ps.Properties["LastSuccessSync"].Value.ToString();
						myDevice.DeviceUserAgent = ps.Properties["DeviceType"].Value == null ? "" : ps.Properties["DeviceType"].Value.ToString();
						myDevice.DevicePolicyApplied = ps.Properties["DevicePolicyApplied"].Value == null ? "" : ps.Properties["DevicePolicyApplied"].Value.ToString();

						// Status returns 'DeviceOk', which is a bit nerdy
						myDevice.Status = ps.Properties["Status"].Value == null ? "" : ps.Properties["Status"].Value.ToString();
						if (myDevice.Status == "DeviceOk") { myDevice.Status = "OK"; }

						myDevice.DeviceUserAgent = ps.Properties["DeviceUserAgent"].Value == null ? "" : ps.Properties["DeviceUserAgent"].Value.ToString();
						// myDevice.Identity = ps.Properties["Identity"].Value == null ? "" : ps.Properties["Identity"].Value.ToString();
						myDevice.DeviceAccessState = ps.Properties["DeviceAccessState"].Value == null ? "" : ps.Properties["DeviceAccessState"].Value.ToString();
						if (myDevice.DeviceAccessState == "Allowed") { myDevice.DeviceAccessState = "Allow"; }

						//Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "getMobileUsers Results: DeviceId:" + DeviceId, Common.LogLevel.Normal);
						//Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "getMobileUsers Results: DeviceMobileOperator:" + DeviceMobileOperator, Common.LogLevel.Normal);
						//Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "getMobileUsers Results: DeviceOS:" + DeviceOS, Common.LogLevel.Normal);
						strSQL = "IF EXISTS ( " +
									"SELECT * FROM [vitalsigns].[dbo].[Traveler_Devices] where " +
									"DeviceID='" + myDevice.DeviceID + "') " +
									"begin " +

									"update [vitalsigns].[dbo].[Traveler_Devices] set " +
									"[UserName]='" + myDevice.User + "',  [Security_Policy]='" + myDevice.DevicePolicyApplied + "', [DeviceName]='" + myDevice.DeviceModel + "', [ConnectionState]='" + myDevice.Status + "', " +
									"[LastSyncTime]='" + myDevice.LastSuccessSync + "', [OS_Type]= '" + myDevice.DeviceOS + "', [Client_Build]='" + myDevice.DeviceActiveSyncVersion + "', [device_type]='" + myDevice.DeviceType + "', " +
									"[ServerName]='" + myServer.Name + "', [Access]='" + myDevice.DeviceAccessState + "', [DeviceID]='" + myDevice.DeviceID + "', [LastUpdated]='" + DateTime.Now + "' " +
									"where DeviceID='" + myDevice.DeviceID + "' " +

									"end  else " +

									"begin " +
									"INSERT INTO vitalsigns.dbo.Traveler_Devices (UserName, Security_Policy, DeviceName, ConnectionState, LastSyncTime, OS_Type, Client_Build, device_type, ServerName, Access, DeviceID, LastUpdated, IsActive) " +
									" VALUES ('" + myDevice.User + "', '" + myDevice.DevicePolicyApplied + "', '" + myDevice.DeviceModel + "', '" + myDevice.Status + "','" + myDevice.LastSuccessSync +"', '" + myDevice.DeviceOS + "', '" +
									myDevice.DeviceActiveSyncVersion + "', '" + myDevice.DeviceType + "', '" + myServer.Name + "', '" + myDevice.DeviceAccessState + "', '" + myDevice.DeviceID + "', '" + DateTime.Now + "', 1) end";

						AllTestsList.SQLStatements.Add(new SQLstatements() { SQL = strSQL + strSQLValues });
					}

				}

			}
			catch (Exception ex)
			{
				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "getMobileUsers Results: Exception: " + ex.Message.ToString(), Common.LogLevel.Verbose);
				//myServer.ADQueryTest = "Fail";
				//Common.makeAlert(false, myServer, commonEnums.AlertType., ref AllTestsList, myServer.ServerType);
			}
		}
		#endregion
		#region mailTests
		private void TestAutoDiscovery(MonitoredItems.Office365Server myServer, ref TestResults AutoDiscoveryIssueList)
		{

			string strResponse = "";
			string strURL = myServer.IPAddress + "/autodiscover/autodiscover.xml";
			try
			{
				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "Attempting to verify Auto discovery service.", Common.LogLevel.Normal);
				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "Using address " + strURL, Common.LogLevel.Normal);
			}
			catch (Exception ex)
			{
			}


			Chilkat.Http WebPage = null;
			bool success = false;
			try
			{
				WebPage = new Chilkat.Http();
				WebPage.Password = myServer.Password;
				WebPage.Login = myServer.UserName;

				success = WebPage.UnlockComponent("MZLDADHttp_efwTynJYYR3X");
				if ((success != true))
				{
					// myServer.ResponseDetails = "Failed to unlock component";
					Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "Failed to unlock Chilkat HTTP component in TestAutoDiscovery.");
				}
				else
				{
					Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "Unlocked Chilkat HTTP component for TestAutoDiscovery.");
				}

			}
			catch (Exception ex)
			{
				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "In  TestAutoDiscovery: " + ex.Message.ToString(), Common.LogLevel.Normal);
			}



			try
			{

				if ((success != true))
				{
					strResponse = WebPage.LastErrorText;

					myServer.Description = "Unable to connect to the auto discovery URL  at " + DateTime.Now.ToString("HH:mm:ss tt") + " because " + strResponse;
					myServer.ResponseDetails = "Unable to connect to the auto discovery service. ";
					Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "Failed to connect because " + strResponse);
					//myServer.ResponseTime = -1;
					//AutoDiscoveryIssueList.StatusDetails.Add(new TestList() { Details = "Unable to connect to the CAS auto discovery service at " + DateTime.Now.ToString("HH:mm:ss tt") + " because " + strResponse, TestName = "IMAP", Category = commonEnums.ServerRoles.CAS, Result = commonEnums.ServerResult.Fail });
					// Common.WriteTestResults(myServer.Name, "Client Access", "IMAP", "Fail", myServer.Description);
					Common.makeAlert(false, myServer, commonEnums.AlertType.Auto_Discovery, ref AutoDiscoveryIssueList, "Unable to connect to the CAS auto discovery service at " + DateTime.Now.ToString("HH:mm:ss tt") + " because " + strResponse, myServer.Category);
				}
				else
				{


					try
					{
						strResponse = WebPage.QuickGetStr(strURL);
						Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "Auto Discovery Status:" + strResponse);
						if (strResponse.IndexOf("ErrorCode") > 1)
						{
							Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "Auto Discovery Status:" + "PASS");
							//AutoDiscoveryIssueList.StatusDetails.Add(new TestList() { Details = "Auto discover service responded.", TestName = "Discovery Service", Category = commonEnums.ServerRoles.CAS, Result = commonEnums.ServerResult.Pass });
							Common.makeAlert(true, myServer, commonEnums.AlertType.Auto_Discovery, ref AutoDiscoveryIssueList, "Auto Discovery Responded:Pass", myServer.Category);
						}
						else
						{
							Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "Auto Discovery Status:" + "FAIL");
							//AutoDiscoveryIssueList.StatusDetails.Add(new TestList() { Details = "Auto discover service did not respond.", TestName = "Discovery Service", Category = commonEnums.ServerRoles.CAS, Result = commonEnums.ServerResult.Fail });
							Common.makeAlert(false, myServer, commonEnums.AlertType.Auto_Discovery, ref AutoDiscoveryIssueList, "Auto discover service responded.", myServer.Category);
						}
						// Common.WriteTestResults(myServer.Name , "IMAP", "Pass",  "Service answered with  " + strResponse.Trim() + " at " + System.DateTime.Now.ToShortTimeString());

					}
					catch (Exception ex)
					{
						Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "In  TestAutoDiscovery: " + ex.Message.ToString(), Common.LogLevel.Normal);
					}



				}

			}
			catch (Exception ex)
			{
				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "In  TestAutoDiscovery: " + ex.Message.ToString(), Common.LogLevel.Normal);
			}
			finally
			{
				WebPage.Dispose();

			}



		}
		private void TestSMTP(MonitoredItems.Office365Server myServer, ref TestResults SMTPIssueList)
		{

			long done;
			long start;
			start = DateTime.Now.Ticks;

			TimeSpan elapsed;
			myServer.ResponseDetails = "";
			string strResponse = "";
			try
			{
				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "Attempting to contact SMTP service.");
				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "Using address " + myServer.IPAddress);
				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "Using port " + myServer.Port);

			}
			catch (Exception ex)
			{
			}






			Chilkat.Socket Socket = null;
			bool success = false;
			try
			{
				Socket = new Chilkat.Socket();
				success = Socket.UnlockComponent("MZLDADSocket_OACwPK2ZlEn9");
				if ((success != true))
				{
					myServer.ResponseDetails = "Failed to unlock component";
					Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "Failed to unlock Chilkat component");
				}
				else
				{
					Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "Unlocked Chilkat component");
				}

			}
			catch (Exception ex)
			{
			}


			try
			{
				bool ssl = false;
				ssl = false;
				int maxWaitMillisec = 0;
				maxWaitMillisec = 20000;
				success = Socket.Connect(myServer.IPAddress, 25, ssl, maxWaitMillisec);

			}
			catch (Exception ex)
			{
				success = false;
				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "Failed to connect #1 because " + ex.ToString());
			}


			try
			{
				//  Connect to port 5555 of localhost.
				//  The string "localhost" is for testing on a single computer.
				//  It would typically be replaced with an IP hostname, such
				//  as "www.chilkatsoft.com".

				if ((success != true))
				{
					strResponse = Socket.LastErrorText;
					//myServer.ResponseTime = -1;
					myServer.Description = "Unable to connect to the SMTP server at " + DateTime.Now.ToString("HH:mm:ss tt") + " because " + strResponse;
					myServer.ResponseDetails = "Unable to connect to the SMTP server. ";
					Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "Failed to connect because " + strResponse);
					//myServer.ResponseTime = -1;
					//SMTPIssueList.StatusDetails.Add(new TestList() { Details = "Unable to connect to the SMTP server at " + DateTime.Now.ToString("HH:mm:ss tt") + " because " + strResponse, TestName = "SMTP", Category = commonEnums.ServerRoles.CAS, Result = commonEnums.ServerResult.Fail });
					//Common.WriteTestResults(myServer.Name, "Client Access", "SMTP", "Fail", myServer.Description);

					//SMTPIssueList.AlertDetails.Add(new Alerting() { DeviceType = commonEnums.AlertDevice.Exchange, DeviceName = myServer.Name, AlertType = commonEnums.AlertType.SMTP, Details = "The SMTP server did not respond, detected at " + DateTime.Now.ToString(), Location = myServer.Location, ResetAlertQueue = commonEnums.ResetAlert.No });
					Common.makeAlert(false, myServer, commonEnums.AlertType.SMTP, ref SMTPIssueList, "Unable to connect to the SMTP server at " + DateTime.Now.ToString("HH:mm:ss tt") + " because " + strResponse.ToString().Substring(0, 150), myServer.Category);
				}
				else
				{
					//  Set maximum timeouts for reading an writing (in millisec)
					Socket.MaxReadIdleMs = 10000;
					Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "Successfully connected to " + myServer.Name);

					try
					{
						Socket.SendString("EHLO");
						strResponse = Socket.ReceiveString();
						done = DateTime.Now.Ticks;
						elapsed = new TimeSpan(done - start);
						//myServer.ResponseTime = elapsed.TotalMilliseconds;
						Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "Service answered with  " + strResponse.Trim());
						myServer.ResponseDetails = "Server response: " + strResponse.Trim() + vbCrLf + vbCrLf;
						myServer.ResponseDetails += " SMTP server connected in " + elapsed.TotalMilliseconds + " ms." + vbCrLf + "Target response is " + myServer.ResponseThreshold + " ms.";
						// myServer.Description = " SMTP server connected in " & Microsoft.VisualBasic.Strings.Format(elapsed.TotalMilliseconds, "F1") & " ms. " & vbCrLf & "Target response is " & myServer.ResponseThreshold & " ms."
						myServer.Description = " SMTP service connected in " + elapsed.TotalMilliseconds.ToString("F1") + " ms at " + System.DateTime.Now.ToShortTimeString();
						//SMTPIssueList.StatusDetails.Add(new TestList() { Details = "Service answered with  " + strResponse.Trim(), TestName = "SMTP", Category = commonEnums.ServerRoles.CAS, Result = commonEnums.ServerResult.Pass });

						//SMTPIssueList.AlertDetails.Add(new Alerting() { DeviceType = commonEnums.AlertDevice.Exchange, DeviceName = myServer.Name, AlertType = commonEnums.AlertType.SMTP, Details = "The SMTP server is OK, detected at " + DateTime.Now.ToString(), Location = myServer.Location, ResetAlertQueue = commonEnums.ResetAlert.Yes });
						Common.makeAlert(true, myServer, commonEnums.AlertType.SMTP, ref SMTPIssueList, "Service answered with  " + strResponse.Trim(), myServer.Category);
						DateTime dtNow = DateTime.Now;
						int weekNumber = culture.Calendar.GetWeekOfYear(dtNow, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday);
						string sqlQuery = "Insert into VSS_Statistics.dbo.MicrosoftDailyStats(ServerName,ServerTypeId,Date,StatName,StatValue,WeekNumber,MonthNumber,YearNumber,DayNumber, HourNumber, Details) "
								+ " values('" + myServer.Name + "','" + myServer.ServerTypeId + "',GetDate(),'SMTP" + "@" + nodeName + "'" + " ," + elapsed.TotalMilliseconds.ToString() +
							   "," + weekNumber + ", " + dtNow.Month.ToString() + ", " + dtNow.Year.ToString() + ", " + dtNow.Day.ToString() + ", " + dtNow.Hour.ToString() + ", '')";
						SMTPIssueList.SQLStatements.Add(new SQLstatements() { SQL = sqlQuery, DatabaseName = "VSS_Statistics" });
					}
					catch (Exception ex)
					{
					}


					//  Close the connection with the server
					//  Wait a max of 20 seconds (20000 millsec)
					Socket.Close(20000);

				}

			}
			catch (Exception ex)
			{
			}
			finally
			{
				Socket.Dispose();

			}


			//return SMTPIssueList;

		}
		private void TestPOP(MonitoredItems.Office365Server myServer, ref TestResults POPIssueList)
		{

			long done;
			long start;
			start = DateTime.Now.Ticks;

			TimeSpan elapsed;
			myServer.ResponseDetails = "";
			string strResponse = "";
			try
			{
				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "Attempting to contact POP service");
				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "Using address " + myServer.IPAddress);
				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "Using port " + myServer.Port);

			}
			catch (Exception ex)
			{
			}



			Chilkat.Socket Socket = null;
			bool success = false;
			try
			{
				Socket = new Chilkat.Socket();
				success = Socket.UnlockComponent("MZLDADSocket_OACwPK2ZlEn9");
				if ((success != true))
				{
					myServer.ResponseDetails = "Failed to unlock component";
					Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "Failed to unlock Chilkat component");
				}
				else
				{
					Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "Unlocked Chilkat component");
				}

			}
			catch (Exception ex)
			{
			}


			try
			{
				bool ssl = false;
				ssl = false;
				int maxWaitMillisec = 0;
				maxWaitMillisec = 20000;
				myServer.Port = 110;
				success = Socket.Connect(myServer.IPAddress, myServer.Port, ssl, maxWaitMillisec);

			}
			catch (Exception ex)
			{
				success = false;
				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "Failed to connect #1 because " + ex.ToString());
			}


			try
			{
				//  Connect to port 5555 of localhost.
				//  The string "localhost" is for testing on a single computer.
				//  It would typically be replaced with an IP hostname, such
				//  as "www.chilkatsoft.com".

				if ((success != true))
				{
					strResponse = Socket.LastErrorText;
					//myServer.ResponseTime = -1;
					myServer.Description = "Unable to connect to the POP server at " + DateTime.Now.ToString("HH:mm:ss tt") + " because " + strResponse;
					myServer.ResponseDetails = "Unable to connect to the POP server. ";
					Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "Failed to connect because " + strResponse);
					//myServer.ResponseTime = -1;
					//POPIssueList.StatusDetails.Add(new TestList() { Details = "Unable to connect to the POP3 server.", TestName = "POP3", Category = commonEnums.ServerRoles.CAS, Result = commonEnums.ServerResult.Fail });
					//POPIssueList.SQLStatements.Add(new SQLstatements() { SQL = "Insert into WindowsServices(ServerName,Service_Name,Status,StartMode,DisplayName,Monitored) values('aaaa','bbbb','Issue','none','xxxx',1)" });
					//Common.WriteTestResults(myServer.Name, "Client Access", "POP3", "Fail", myServer.Description);

					//POPIssueList.AlertDetails.Add(new Alerting() { DeviceType = commonEnums.AlertDevice.Exchange, DeviceName = myServer.Name, AlertType = commonEnums.AlertType.POP, Details = "The POP server did not respond, detected at " + DateTime.Now.ToString(), Location = myServer.Location, ResetAlertQueue = commonEnums.ResetAlert.No });
					Common.makeAlert(false, myServer, commonEnums.AlertType.POP3, ref POPIssueList, "Unable to connect to the POP3 server.", myServer.Category);
				}
				else
				{
					//  Set maximum timeouts for reading an writing (in millisec)
					Socket.MaxReadIdleMs = 10000;
					Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "Successfully connected to " + myServer.Name);

					try
					{
						Socket.SendString("noop");
						strResponse = Socket.ReceiveString();
						done = DateTime.Now.Ticks;
						elapsed = new TimeSpan(done - start);
						//myServer.ResponseTime = elapsed.TotalMilliseconds;
						Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "Service answered with  " + strResponse.Trim());
						//Common.WriteTestResults(myServer.Name, "Client Access", "POP3", "Pass", "Service answered with  " + strResponse.Trim() + " at " + System.DateTime.Now.ToShortTimeString());
						//POPIssueList.StatusDetails.Add(new TestList() { Details = "Service answered with  " + strResponse.Trim(), TestName = "POP3", Category = commonEnums.ServerRoles.CAS, Result = commonEnums.ServerResult.Pass });
						Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, myServer.ResponseDetails);

						//POPIssueList.AlertDetails.Add(new Alerting() { DeviceType = commonEnums.AlertDevice.Exchange, DeviceName = myServer.Name, AlertType = commonEnums.AlertType.POP, Details = "The POP server is OK, detected at " + DateTime.Now.ToString(), Location = myServer.Location, ResetAlertQueue = commonEnums.ResetAlert.Yes });
						Common.makeAlert(true, myServer, commonEnums.AlertType.POP3, ref POPIssueList, "Service answered with  " + strResponse.Trim(), myServer.Category);
						DateTime dtNow = DateTime.Now;
						int weekNumber = culture.Calendar.GetWeekOfYear(dtNow, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday);
						string sqlQuery = "Insert into VSS_Statistics.dbo.MicrosoftDailyStats(ServerName,ServerTypeId,Date,StatName,StatValue,WeekNumber,MonthNumber,YearNumber,DayNumber, HourNumber, Details) "
								+ " values('" + myServer.Name + "','" + myServer.ServerTypeId + "',GetDate(),'POP" + "@" + nodeName + "'" + " ," + elapsed.TotalMilliseconds.ToString() +
							   "," + weekNumber + ", " + dtNow.Month.ToString() + ", " + dtNow.Year.ToString() + ", " + dtNow.Day.ToString() + ", " + dtNow.Hour.ToString() + ", '')";
						POPIssueList.SQLStatements.Add(new SQLstatements() { SQL = sqlQuery, DatabaseName = "VSS_Statistics" });
					}
					catch (Exception ex)
					{
					}


					//  Close the connection with the server
					//  Wait a max of 20 seconds (20000 millsec)
					Socket.Close(20000);

				}

			}
			catch (Exception ex)
			{
			}
			finally
			{
				Socket.Dispose();

			}
			//POPIssueList.Add(new IssuesList() { Issue = "Unable to connect to the POP server at", Name = "POP Issue" });
			//return POPIssueList;
		}
		private void TestIMAP(MonitoredItems.Office365Server myServer, ref TestResults IMAPIssueList)
		{

			long done;
			long start;
			start = DateTime.Now.Ticks;

			TimeSpan elapsed;
			myServer.ResponseDetails = "";
			string strResponse = "";
			try
			{
				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "Attempting to contact IMAP service.");
				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "Using address " + myServer.IPAddress);

			}
			catch (Exception ex)
			{
			}

			myServer.Port = 143;

			Chilkat.Socket Socket = null;
			bool success = false;
			try
			{
				Socket = new Chilkat.Socket();
				success = Socket.UnlockComponent("MZLDADSocket_OACwPK2ZlEn9");
				if ((success != true))
				{
					myServer.ResponseDetails = "Failed to unlock component";
					Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "Failed to unlock Chilkat component");
				}
				else
				{
					Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "Unlocked Chilkat component");
				}

			}
			catch (Exception ex)
			{
			}


			try
			{
				bool ssl = false;
				ssl = false;
				int maxWaitMillisec = 0;
				maxWaitMillisec = 20000;
				success = Socket.Connect(myServer.IPAddress, myServer.Port, ssl, maxWaitMillisec);

			}
			catch (Exception ex)
			{
				success = false;
				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "Failed to connect #1 because " + ex.ToString());
			}


			try
			{
				//  Connect to port 5555 of localhost.
				//  The string "localhost" is for testing on a single computer.
				//  It would typically be replaced with an IP hostname, such
				//  as "www.chilkatsoft.com".

				if ((success != true))
				{
					strResponse = Socket.LastErrorText;
					//myServer.ResponseTime = -1;
					myServer.Description = "Unable to connect to the IMAP server at " + DateTime.Now.ToString("HH:mm:ss tt") + " because " + strResponse;
					myServer.ResponseDetails = "Unable to connect to the IMAP server. ";
					Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "Failed to connect because " + strResponse);
					//myServer.ResponseTime = -1;
					//IMAPIssueList.StatusDetails.Add(new TestList() { Details = "Unable to connect to the IMAP server at " + DateTime.Now.ToString("HH:mm:ss tt") + " because " + strResponse, TestName = "IMAP", Category = commonEnums.ServerRoles.CAS, Result = commonEnums.ServerResult.Fail });
					// Common.WriteTestResults(myServer.Name, "Client Access", "IMAP", "Fail", myServer.Description);
					//myServer.ResponseTime = -1;
					//IMAPIssueList.AlertDetails.Add(new Alerting() { DeviceType = commonEnums.AlertDevice.Exchange, DeviceName = myServer.Name, AlertType = commonEnums.AlertType.IMAP, Details = "The IMAP server did not respond, detected at " + DateTime.Now.ToString(), Location = myServer.Location, ResetAlertQueue = commonEnums.ResetAlert.No });
					Common.makeAlert(false, myServer, commonEnums.AlertType.IMAP, ref IMAPIssueList, "Unable to connect to the IMAP server at " + DateTime.Now.ToString("HH:mm:ss tt") + " because " + strResponse, myServer.Category);
				}
				else
				{
					//  Set maximum timeouts for reading an writing (in millisec)
					Socket.MaxReadIdleMs = 10000;
					Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "Successfully connected to " + myServer.Name);

					try
					{
						//Socket.SendString("EHLO");
						strResponse = Socket.ReceiveString();
						done = DateTime.Now.Ticks;
						elapsed = new TimeSpan(done - start);
						Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "Service answered with  " + strResponse.Trim());
						myServer.ResponseDetails = "Server response: " + strResponse.Trim() + vbCrLf + vbCrLf;
						myServer.ResponseDetails += " IMAP server connected in " + elapsed.TotalMilliseconds + " ms." + vbCrLf + "Target response is " + myServer.ResponseThreshold + " ms.";
						// myServer.Description = " IMAP server connected in " & Microsoft.VisualBasic.Strings.Format(elapsed.TotalMilliseconds, "F1") & " ms. " & vbCrLf & "Target response is " & myServer.ResponseThreshold & " ms."
						myServer.Description = " IMAP service connected in " + elapsed.TotalMilliseconds.ToString("F1") + " ms at " + System.DateTime.Now.ToShortTimeString();
						//Common.WriteTestResults(myServer.Name , "IMAP", "Pass",  "Service answered with  " + strResponse.Trim() + " at " + System.DateTime.Now.ToShortTimeString());
						//IMAPIssueList.StatusDetails.Add(new TestList() { Details = "Service answered with  " + strResponse.Trim(), TestName = "IMAP", Category = commonEnums.ServerRoles.CAS, Result = commonEnums.ServerResult.Pass });
						Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, myServer.ResponseDetails);

						//IMAPIssueList.AlertDetails.Add(new Alerting() { DeviceType = commonEnums.AlertDevice.Exchange, DeviceName = myServer.Name, AlertType = commonEnums.AlertType.IMAP, Details = "The IMAP server is OK, detected at " + DateTime.Now.ToString(), Location = myServer.Location, ResetAlertQueue = commonEnums.ResetAlert.Yes });
						Common.makeAlert(true, myServer, commonEnums.AlertType.IMAP, ref IMAPIssueList, commonEnums.AlertType.IMAP.ToString(), myServer.Category);

						CultureInfo culture = CultureInfo.CurrentCulture;
						//myServer.ResponseTime = elapsed.TotalMilliseconds;
						DateTime dtNow = DateTime.Now;
						int weekNumber = culture.Calendar.GetWeekOfYear(dtNow, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday);
						string sqlQuery = "Insert into VSS_Statistics.dbo.MicrosoftDailyStats(ServerName,ServerTypeId,Date,StatName,StatValue,WeekNumber,MonthNumber,YearNumber,DayNumber, HourNumber, Details) "
								+ " values('" + myServer.Name + "','" + myServer.ServerTypeId + "',GetDate(),'IMAP" + "@" + nodeName + "'" + " ," + elapsed.TotalMilliseconds.ToString() +
							   "," + weekNumber + ", " + dtNow.Month.ToString() + ", " + dtNow.Year.ToString() + ", " + dtNow.Day.ToString() + ", " + dtNow.Hour.ToString() + ", '')";
						IMAPIssueList.SQLStatements.Add(new SQLstatements() { SQL = sqlQuery, DatabaseName = "VSS_Statistics" });


					}
					catch (Exception ex)
					{
						//myServer.ResponseTime = -1;
					}


					//  Close the connection with the server
					//  Wait a max of 20 seconds (20000 millsec)
					Socket.Close(20000);

				}

			}
			catch (Exception ex)
			{
				//myServer.ResponseTime = -1;
			}
			finally
			{
				Socket.Dispose();

			}



		}
		public void TestMAPIConectivity(MonitoredItems.Office365Server myServer, ref TestResults AllTestsList, ReturnPowerShellObjects powershellobj)
		{
			try
			{
				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "getMailboxes: Starting.", Common.LogLevel.Normal);
				System.Collections.ObjectModel.Collection<PSObject> results = new System.Collections.ObjectModel.Collection<PSObject>();
				//String str = " Get-Mailbox -ResultSize Unlimited | Select Name,Alais,DisplayName,StorageLimitStatus,membertype,servername,ProhibitSendQuota,LastLogonTime";
				string str = "Test-MAPIConnectivity -Identity " + myServer.UserName;
				powershellobj.PS.Commands.Clear();
				powershellobj.PS.Streams.ClearStreams();
				powershellobj.PS.AddScript(str);
				results = powershellobj.PS.Invoke();

				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "TestMAPIConectivity Results: " + results.Count.ToString(), Common.LogLevel.Normal);
				DateTime dtNow = DateTime.Now;
				int weekNumber = culture.Calendar.GetWeekOfYear(dtNow, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday);
				if (results.Count > 0)
				{
					foreach (PSObject ps in results)
					{
						string Result = ps.Properties["Result"].Value == null ? "" : ps.Properties["Result"].Value.ToString();
						string Err = ps.Properties["Error"].Value == null ? "" : ps.Properties["Error"].Value.ToString();
						if (Result == "Success")
							Common.makeAlert(true, myServer, commonEnums.AlertType.RPC, ref AllTestsList, "IMAP Connectivity Succeeded at: " + DateTime.Now.ToString("HH:mm:ss tt"), myServer.Category);
						else
							Common.makeAlert(false, myServer, commonEnums.AlertType.RPC, ref AllTestsList, "Unable to connect to the IMAP server at " + DateTime.Now.ToString("HH:mm:ss tt") + " because " + Err, myServer.Category);
					}

				}
				else
				{
					//myServer.ADQueryTest = "Fail";
					//Common.makeAlert(true, myServer, commonEnums.AlertType.AD_Query_Latency, ref AllTestsList, myServer.ServerType);
					Common.makeAlert(false, myServer, commonEnums.AlertType.RPC, ref AllTestsList, "Unable to connect to the IMAP server at " + DateTime.Now.ToString("HH:mm:ss tt") + " because It did not fetch any result.", myServer.Category);
				}
			}
			catch (Exception ex)
			{
				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "TestMAPIConectivity: Exception: " + ex.Message.ToString(), Common.LogLevel.Verbose);
				//myServer.ADQueryTest = "Fail";
				//Common.makeAlert(false, myServer, commonEnums.AlertType.Mailbox_Database_Size, ref AllTestsList, myServer.ServerType);
			}
		}
		#endregion
		#region spo
		public void createSPOSite(MonitoredItems.Office365Server myServer, ref TestResults AllTestsList, ReturnPowerShellObjects powershellobj)
		{
			long done;
			long start;
			TimeSpan elapsed = new TimeSpan(0);
			try
			{
				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "createSPOSite: Starting.", Common.LogLevel.Normal);
				System.Collections.ObjectModel.Collection<PSObject> results = new System.Collections.ObjectModel.Collection<PSObject>();

				string str = "New-SPOSite -Title 'VSTest' -Url 'https://" + myServer.tenantName + ".sharepoint.com/sites/VSTest' -Owner '" + myServer.UserName + "' -StorageQuota '500' ";

				powershellobj.PS.Commands.Clear();
				powershellobj.PS.Streams.ClearStreams();
				powershellobj.PS.AddScript(str);
				start = DateTime.Now.Ticks;
				results = powershellobj.PS.Invoke();
				done = DateTime.Now.Ticks;
				elapsed = new TimeSpan(done - start);
				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "createSPOSite Results: " + results.Count.ToString(), Common.LogLevel.Normal);
				DateTime dtNow = DateTime.Now;
				int weekNumber = culture.Calendar.GetWeekOfYear(dtNow, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday);

				str = "Get-SPOSite -Identity 'https://" + myServer.tenantName + ".sharepoint.com/sites/VSTest'";
				powershellobj.PS.Commands.Clear();
				powershellobj.PS.Streams.ClearStreams();
				powershellobj.PS.AddScript(str);
				results = powershellobj.PS.Invoke();
				if (results.Count > 0)
				{
					Common.makeAlert(true, myServer, commonEnums.AlertType.Create_Site, ref AllTestsList, "", myServer.Category);
					string sqlQuery = "Insert into VSS_Statistics.dbo.MicrosoftDailyStats(ServerName,ServerTypeId,Date,StatName,StatValue,WeekNumber,MonthNumber,YearNumber,DayNumber, HourNumber, Details) "
							+ " values('" + myServer.Name + "','" + myServer.ServerTypeId + "',GetDate(),'CreateSite" + "@" + nodeName + "'" + " ," + elapsed.TotalMilliseconds.ToString() +
						   "," + weekNumber + ", " + dtNow.Month.ToString() + ", " + dtNow.Year.ToString() + ", " + dtNow.Day.ToString() + ", " + dtNow.Hour.ToString() + ", '')";
					AllTestsList.SQLStatements.Add(new SQLstatements() { SQL = sqlQuery, DatabaseName = "VSS_Statistics" });
				}
				else
					Common.makeAlert(false, myServer, commonEnums.AlertType.Create_Site, ref AllTestsList, "", myServer.Category);

			}
			catch (Exception ex)
			{
				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "createSPOSite Results: Exception: " + ex.Message.ToString(), Common.LogLevel.Verbose);
			}

		}
		public void removeSPOSite(MonitoredItems.Office365Server myServer, ref TestResults AllTestsList, ReturnPowerShellObjects powershellobj)
		{
			try
			{
				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "removeSPOSite: Starting.", Common.LogLevel.Normal);
				System.Collections.ObjectModel.Collection<PSObject> results = new System.Collections.ObjectModel.Collection<PSObject>();
				//String str = " Get-Mailbox -ResultSize Unlimited | Select Name,Alais,DisplayName,StorageLimitStatus,membertype,servername,ProhibitSendQuota,LastLogonTime";
				//string str = "Get-Command| where {$_.Name -like '*Msol*'}";
				string str = "Remove-SPOSite  -Identity 'https://" + myServer.tenantName + ".sharepoint.com/sites/VSTest' -Confirm:$false " + "\n";
				str += " Remove-SPODeletedSite -Identity 'https://" + myServer.tenantName + ".sharepoint.com/sites/VSTest' -Confirm:$false ";
				powershellobj.PS.Commands.Clear();
				powershellobj.PS.Streams.ClearStreams();
				powershellobj.PS.AddScript(str);
				results = powershellobj.PS.Invoke();

				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "removeSPOSite Results: " + results.Count.ToString(), Common.LogLevel.Normal);
				DateTime dtNow = DateTime.Now;
				int weekNumber = culture.Calendar.GetWeekOfYear(dtNow, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday);
				str = "Get-SPOSite -Identity 'https://" + myServer.tenantName + ".sharepoint.com/sites/VSTest'";
				powershellobj.PS.Commands.Clear();
				powershellobj.PS.Streams.ClearStreams();
				powershellobj.PS.AddScript(str);
				results = powershellobj.PS.Invoke();
				if (results.Count > 0)
					Common.makeAlert(false, myServer, commonEnums.AlertType.Delete_Site, ref AllTestsList, "", myServer.Category);
				else
					Common.makeAlert(true, myServer, commonEnums.AlertType.Delete_Site, ref AllTestsList, "", myServer.Category);
			}
			catch (Exception ex)
			{
				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "removeSPOSite Results: Exception: " + ex.Message.ToString(), Common.LogLevel.Verbose);
			}

		}
		#endregion
		#region lync
		public void getLyncStats(MonitoredItems.Office365Server myServer, ref TestResults AllTestsList, ReturnPowerShellObjects powershellobj)
		{
			try
			{
				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "getLyncStats: Starting.", Common.LogLevel.Normal);
				System.Collections.ObjectModel.Collection<PSObject> results = new System.Collections.ObjectModel.Collection<PSObject>();
				//String str = " Get-Mailbox -ResultSize Unlimited | Select Name,Alais,DisplayName,StorageLimitStatus,membertype,servername,ProhibitSendQuota,LastLogonTime";
				//string str = "Get-Command| where {$_.Name -like '*Msol*'}";
				string str = "Get-CsActiveUserReport ";

				powershellobj.PS.Commands.Clear();
				powershellobj.PS.Streams.ClearStreams();
				powershellobj.PS.AddScript(str);
				results = powershellobj.PS.Invoke();

				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "getLyncStats Results: " + results.Count.ToString(), Common.LogLevel.Normal);
				DateTime dtNow = DateTime.Now;
				int weekNumber = culture.Calendar.GetWeekOfYear(dtNow, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday);
				if (results.Count > 0)
				{
					foreach (PSObject ps in results)
					{
						string AccountName = ps.Properties["TenantName"].Value == null ? "0" : ps.Properties["TenantName"].Value.ToString();
						string ActiveUsers = ps.Properties["ActiveUsers"].Value == null ? "0" : ps.Properties["ActiveUsers"].Value.ToString();
						string ActiveIMUsers = ps.Properties["ActiveIMUsers"].Value == null ? "0" : ps.Properties["ActiveIMUsers"].Value.ToString();
						string ActiveAudioUsers = ps.Properties["ActiveAudioUsers"].Value == null ? "0" : ps.Properties["ActiveAudioUsers"].Value.ToString();
						string ActiveVideousers = ps.Properties["ActiveVideousers"].Value == null ? "0" : ps.Properties["ActiveVideousers"].Value.ToString();
						string ActiveApplicationSharingUsers = ps.Properties["ActiveApplicationSharingUsers"].Value == null ? "0" : ps.Properties["ActiveApplicationSharingUsers"].Value.ToString();
						string ActiveFileTransferUsers = ps.Properties["ActiveFileTransferUsers"].Value == null ? "0" : ps.Properties["ActiveFileTransferUsers"].Value.ToString();

						Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "getLyncStats Results: AccountName:" + AccountName, Common.LogLevel.Normal);

						string sqlQuery = "DELETE O365LyncStats WHERE SERVERID=" + myServer.ServerId.ToString();
						AllTestsList.SQLStatements.Add(new SQLstatements() { SQL = sqlQuery, DatabaseName = "vitalsigns" });

						sqlQuery = "INSERT INTO O365LyncStats (SERVERID,ACCOUNTNAME,ActiveUsers,ActiveIMUsers,ActiveAudioUsers,ActiveVideousers,ActiveApplicationSharingUsers,ActiveFileTransferUsers) VALUES(" + myServer.ServerId + ",'" + AccountName + "'," + ActiveUsers + "," + ActiveIMUsers + "," + ActiveAudioUsers + "," + ActiveVideousers + "," + ActiveApplicationSharingUsers + "," + ActiveFileTransferUsers + ")";
						AllTestsList.SQLStatements.Add(new SQLstatements() { SQL = sqlQuery, DatabaseName = "vitalsigns" });
						break;
					}

				}

			}
			catch (Exception ex)
			{
				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "getLyncStats Results: Exception: " + ex.Message.ToString(), Common.LogLevel.Verbose);
			}
		}
		public void getLyncDevices(MonitoredItems.Office365Server myServer, ref TestResults AllTestsList, ReturnPowerShellObjects powershellobj)
		{
			try
			{
				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "getLyncDevices: Starting.", Common.LogLevel.Normal);
				System.Collections.ObjectModel.Collection<PSObject> results = new System.Collections.ObjectModel.Collection<PSObject>();
				//String str = sr.ReadToEnd();
				//String str = " Get-Mailbox -ResultSize Unlimited | Select Name,Alais,DisplayName,StorageLimitStatus,membertype,servername,ProhibitSendQuota,LastLogonTime";
				//string str = "Get-Command| where {$_.Name -like '*Msol*'}";
				string str = "Get-CsClientDeviceReport ";

				powershellobj.PS.Commands.Clear();
				powershellobj.PS.Streams.ClearStreams();
				powershellobj.PS.AddScript(str);
				results = powershellobj.PS.Invoke();

				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "getLyncStats Results: " + results.Count.ToString(), Common.LogLevel.Normal);
				DateTime dtNow = DateTime.Now;
				int weekNumber = culture.Calendar.GetWeekOfYear(dtNow, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday);
				if (results.Count > 0)
				{
					foreach (PSObject ps in results)
					{
						string AccountName = ps.Properties["TenantName"].Value == null ? "0" : ps.Properties["TenantName"].Value.ToString();
						string WindowsUsers = ps.Properties["WindowsUsers"].Value == null ? "0" : ps.Properties["WindowsUsers"].Value.ToString();
						string WindowsPhoneUsers = ps.Properties["WindowsPhoneUsers"].Value == null ? "0" : ps.Properties["WindowsPhoneUsers"].Value.ToString();
						string AndroidUsers = ps.Properties["AndroidUsers"].Value == null ? "0" : ps.Properties["AndroidUsers"].Value.ToString();
						string iPhoneUsers = ps.Properties["iPhoneUsers"].Value == null ? "0" : ps.Properties["iPhoneUsers"].Value.ToString();
						string iPadUsers = ps.Properties["iPadUsers"].Value == null ? "0" : ps.Properties["iPadUsers"].Value.ToString();

						Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "getLyncDevices Results: AccountName:" + AccountName, Common.LogLevel.Normal);

						string sqlQuery = "DELETE O365LyncDevices WHERE SERVERID=" + myServer.ServerId.ToString();
						AllTestsList.SQLStatements.Add(new SQLstatements() { SQL = sqlQuery, DatabaseName = "vitalsigns" });

						sqlQuery = "INSERT INTO O365LyncDevices (SERVERID,ACCOUNTNAME,WindowsUsers,WindowsPhoneUsers,AndroidUsers,iPhoneUsers,iPadUsers) VALUES(" + myServer.ServerId + ",'" + AccountName + "'," + WindowsUsers + "," + WindowsPhoneUsers + "," + AndroidUsers + "," + iPhoneUsers + "," + iPadUsers + ")";
						AllTestsList.SQLStatements.Add(new SQLstatements() { SQL = sqlQuery, DatabaseName = "vitalsigns" });
						break;
					}

				}

			}
			catch (Exception ex)
			{
				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "getLyncDevices Results: Exception: " + ex.Message.ToString(), Common.LogLevel.Verbose);
			}
		}
		public void getLyncPAVTimeReport(MonitoredItems.Office365Server myServer, ref TestResults AllTestsList, ReturnPowerShellObjects powershellobj)
		{
			try
			{
				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "getLyncPAVTimeReport: Starting.", Common.LogLevel.Normal);
				System.Collections.ObjectModel.Collection<PSObject> results = new System.Collections.ObjectModel.Collection<PSObject>();
				//String str = " Get-Mailbox -ResultSize Unlimited | Select Name,Alais,DisplayName,StorageLimitStatus,membertype,servername,ProhibitSendQuota,LastLogonTime";
				//string str = "Get-Command| where {$_.Name -like '*Msol*'}";
				string str = "Get-CsP2PAVTimeReport ";

				powershellobj.PS.Commands.Clear();
				powershellobj.PS.Streams.ClearStreams();
				powershellobj.PS.AddScript(str);
				results = powershellobj.PS.Invoke();

				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "getLyncPAVTimeReport Results: " + results.Count.ToString(), Common.LogLevel.Normal);
				DateTime dtNow = DateTime.Now;
				int weekNumber = culture.Calendar.GetWeekOfYear(dtNow, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday);
				if (results.Count > 0)
				{
					foreach (PSObject ps in results)
					{
						string AccountName = ps.Properties["TenantName"].Value == null ? "0" : ps.Properties["TenantName"].Value.ToString();
						string TotalAudioMinutes = ps.Properties["TotalAudioMinutes"].Value == null ? "0" : ps.Properties["TotalAudioMinutes"].Value.ToString();
						string TotalVideoMinutes = ps.Properties["TotalVideoMinutes"].Value == null ? "0" : ps.Properties["TotalVideoMinutes"].Value.ToString();


						Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "getLyncPAVTimeReport Results: AccountName:" + AccountName, Common.LogLevel.Normal);

						string sqlQuery = "DELETE O365LYNCPAVTimeReport WHERE SERVERID=" + myServer.ServerId.ToString();
						AllTestsList.SQLStatements.Add(new SQLstatements() { SQL = sqlQuery, DatabaseName = "vitalsigns" });

						sqlQuery = "INSERT INTO O365LYNCPAVTimeReport (SERVERID,ACCOUNTNAME,TotalAudioMinutes,TotalVideoMinutes) VALUES(" + myServer.ServerId + ",'" + AccountName + "'," + TotalAudioMinutes + "," + TotalVideoMinutes + ")";
						AllTestsList.SQLStatements.Add(new SQLstatements() { SQL = sqlQuery, DatabaseName = "vitalsigns" });
						break;
					}

				}

			}
			catch (Exception ex)
			{
				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "getLyncPAVTimeReport Results: Exception: " + ex.Message.ToString(), Common.LogLevel.Verbose);
			}
		}
		public void getLyncConferenceReport(MonitoredItems.Office365Server myServer, ref TestResults AllTestsList, ReturnPowerShellObjects powershellobj)
		{
			try
			{
				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "getLyncDevices: Starting.", Common.LogLevel.Normal);
				System.Collections.ObjectModel.Collection<PSObject> results = new System.Collections.ObjectModel.Collection<PSObject>();
				//String str = " Get-Mailbox -ResultSize Unlimited | Select Name,Alais,DisplayName,StorageLimitStatus,membertype,servername,ProhibitSendQuota,LastLogonTime";
				//string str = "Get-Command| where {$_.Name -like '*Msol*'}";
				string str = "Get-CsConferenceReport";

				powershellobj.PS.Commands.Clear();
				powershellobj.PS.Streams.ClearStreams();
				powershellobj.PS.AddScript(str);
				results = powershellobj.PS.Invoke();

				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "getLyncConferenceReport Results: " + results.Count.ToString(), Common.LogLevel.Normal);
				DateTime dtNow = DateTime.Now;
				int weekNumber = culture.Calendar.GetWeekOfYear(dtNow, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday);
				if (results.Count > 0)
				{
					foreach (PSObject ps in results)
					{
						string AccountName = ps.Properties["TenantName"].Value == null ? "0" : ps.Properties["TenantName"].Value.ToString();
						string TotalConferences = ps.Properties["TotalConferences"].Value == null ? "0" : ps.Properties["TotalConferences"].Value.ToString();
						string AVConferences = ps.Properties["AVConferences"].Value == null ? "0" : ps.Properties["AVConferences"].Value.ToString();
						string IMConferences = ps.Properties["IMConferences"].Value == null ? "0" : ps.Properties["IMConferences"].Value.ToString();
						string ApplicationSharingConferences = ps.Properties["ApplicationSharingConferences"].Value == null ? "0" : ps.Properties["ApplicationSharingConferences"].Value.ToString();
						string WebConferences = ps.Properties["WebConferences"].Value == null ? "0" : ps.Properties["WebConferences"].Value.ToString();
						string TelephonyConferences = ps.Properties["TelephonyConferences"].Value == null ? "0" : ps.Properties["TelephonyConferences"].Value.ToString();

						Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "getLyncConferenceReport Results: AccountName:" + AccountName, Common.LogLevel.Normal);

						string sqlQuery = "DELETE O365LYNCConferenceReport WHERE SERVERID=" + myServer.ServerId.ToString();
						AllTestsList.SQLStatements.Add(new SQLstatements() { SQL = sqlQuery, DatabaseName = "vitalsigns" });

						sqlQuery = "INSERT INTO O365LYNCConferenceReport (SERVERID,ACCOUNTNAME,TotalConferences,AVConferences,IMConferences,ApplicationSharingConferences,WebConferences,TelephonyConferences) VALUES(" + myServer.ServerId + ",'" + AccountName + "'," + TotalConferences + "," + AVConferences + "," + IMConferences + "," + ApplicationSharingConferences + "," + WebConferences + "," + TelephonyConferences + ")";
						AllTestsList.SQLStatements.Add(new SQLstatements() { SQL = sqlQuery, DatabaseName = "vitalsigns" });
						break;
					}

				}

			}
			catch (Exception ex)
			{
				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "getLyncConferenceReport Results: Exception: " + ex.Message.ToString(), Common.LogLevel.Verbose);
			}
		}
		public void getLyncP2PSessionReport(MonitoredItems.Office365Server myServer, ref TestResults AllTestsList, ReturnPowerShellObjects powershellobj)
		{
			try
			{
				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "getLyncP2PSessionReport: Starting.", Common.LogLevel.Normal);
				System.Collections.ObjectModel.Collection<PSObject> results = new System.Collections.ObjectModel.Collection<PSObject>();
				//String str = " Get-Mailbox -ResultSize Unlimited | Select Name,Alais,DisplayName,StorageLimitStatus,membertype,servername,ProhibitSendQuota,LastLogonTime";
				//string str = "Get-Command| where {$_.Name -like '*Msol*'}";
				string str = "Get-CsP2PSessionReport ";

				powershellobj.PS.Commands.Clear();
				powershellobj.PS.Streams.ClearStreams();
				powershellobj.PS.AddScript(str);
				results = powershellobj.PS.Invoke();

				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "getLyncP2PSessionReport Results: " + results.Count.ToString(), Common.LogLevel.Normal);
				DateTime dtNow = DateTime.Now;
				int weekNumber = culture.Calendar.GetWeekOfYear(dtNow, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday);
				if (results.Count > 0)
				{
					foreach (PSObject ps in results)
					{
						string AccountName = ps.Properties["TenantName"].Value == null ? "0" : ps.Properties["TenantName"].Value.ToString();
						string TotalP2PSessions = ps.Properties["TotalP2PSessions"].Value == null ? "0" : ps.Properties["TotalP2PSessions"].Value.ToString();
						string P2PIMSessions = ps.Properties["P2PIMSessions"].Value == null ? "0" : ps.Properties["P2PIMSessions"].Value.ToString();
						string P2PAudioSessions = ps.Properties["P2PAudioSessions"].Value == null ? "0" : ps.Properties["P2PAudioSessions"].Value.ToString();
						string P2PVideoSessions = ps.Properties["P2PVideoSessions"].Value == null ? "0" : ps.Properties["P2PVideoSessions"].Value.ToString();
						string P2PApplicationSharingSessions = ps.Properties["P2PApplicationSharingSessions"].Value == null ? "0" : ps.Properties["P2PApplicationSharingSessions"].Value.ToString();
						string P2PFileTransferSessions = ps.Properties["P2PFileTransferSessions"].Value == null ? "0" : ps.Properties["P2PFileTransferSessions"].Value.ToString();

						Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "getLyncP2PSessionReport Results: AccountName:" + AccountName, Common.LogLevel.Normal);

						string sqlQuery = "DELETE O365LYNCP2PSessionReport WHERE SERVERID=" + myServer.ServerId.ToString();
						AllTestsList.SQLStatements.Add(new SQLstatements() { SQL = sqlQuery, DatabaseName = "vitalsigns" });

						sqlQuery = "INSERT INTO O365LYNCP2PSessionReport (SERVERID,ACCOUNTNAME,TotalP2PSessions,P2PIMSessions,P2PAudioSessions,P2PVideoSessions,P2PApplicationSharingSessions,P2PFileTransferSessions) VALUES(" + myServer.ServerId + ",'" + AccountName + "'," + TotalP2PSessions + "," + P2PIMSessions + "," + P2PAudioSessions + "," + P2PVideoSessions + "," + P2PApplicationSharingSessions + "," + P2PFileTransferSessions + ")";
						AllTestsList.SQLStatements.Add(new SQLstatements() { SQL = sqlQuery, DatabaseName = "vitalsigns" });
						break;
					}

				}

			}
			catch (Exception ex)
			{
				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "getLyncP2PSessionReport Results: Exception: " + ex.Message.ToString(), Common.LogLevel.Verbose);
			}
		}
		#endregion
		#region unused Functions
		public void getAllUsers(MonitoredItems.Office365Server myServer, ref TestResults AllTestsList, ReturnPowerShellObjects powershellobj)
		{
			try
			{
				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "getAllUsers: Starting.", Common.LogLevel.Normal);
				System.Collections.ObjectModel.Collection<PSObject> results = new System.Collections.ObjectModel.Collection<PSObject>();

				string str = "Get-User | Select *";
				powershellobj.PS.Commands.Clear();
				powershellobj.PS.Streams.ClearStreams();
				powershellobj.PS.AddScript(str);
				results = powershellobj.PS.Invoke();

				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "getAllUsers Results: " + results.Count.ToString(), Common.LogLevel.Normal);
				DateTime dtNow = DateTime.Now;
				int weekNumber = culture.Calendar.GetWeekOfYear(dtNow, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday);
				if (results.Count > 0)
				{
					myServer.UserCount = results.Count;
					foreach (PSObject ps in results)
					{
						string AccountDisabled = ps.Properties["AccountDisabled"].Value == null ? "" : ps.Properties["AccountDisabled"].Value.ToString();
						string DisplayName = ps.Properties["DisplayName"].Value == null ? "" : ps.Properties["DisplayName"].Value.ToString();
						string RecipientType = ps.Properties["RecipientType"].Value == null ? "" : ps.Properties["RecipientType"].Value.ToString();
						string ExchangeVersion = ps.Properties["ExchangeVersion"].Value == null ? "" : ps.Properties["ExchangeVersion"].Value.ToString();
						string City = ps.Properties["City"].Value == null ? "" : ps.Properties["City"].Value.ToString();
						string Company = ps.Properties["Company"].Value == null ? "" : ps.Properties["Company"].Value.ToString();
						string CountryOrRegion = ps.Properties["CountryOrRegion"].Value == null ? "" : ps.Properties["CountryOrRegion"].Value.ToString();
						string Department = ps.Properties["Department"].Value == null ? "" : ps.Properties["Department"].Value.ToString();
						string UserAccountControl = ps.Properties["UserAccountControl"].Value == null ? "" : ps.Properties["UserAccountControl"].Value.ToString();
						string FirstName = ps.Properties["FirstName"].Value == null ? "" : ps.Properties["FirstName"].Value.ToString();
						string LastName = ps.Properties["LastName"].Value == null ? "" : ps.Properties["LastName"].Value.ToString();
						string MobilePhone = ps.Properties["MobilePhone"].Value == null ? "" : ps.Properties["MobilePhone"].Value.ToString();
						string StateOrProvince = ps.Properties["StateOrProvince"].Value == null ? "" : ps.Properties["StateOrProvince"].Value.ToString();
						string StreetAddress = ps.Properties["StreetAddress"].Value == null ? "" : ps.Properties["StreetAddress"].Value.ToString();
						string WindowsEmailAddress = ps.Properties["WindowsEmailAddress"].Value == null ? "" : ps.Properties["WindowsEmailAddress"].Value.ToString();
						string IsValid = ps.Properties["IsValid"].Value == null ? "" : ps.Properties["IsValid"].Value.ToString();
						string Name = ps.Properties["Name"].Value == null ? "" : ps.Properties["Name"].Value.ToString();

						Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "getAllUsers Results: displayName:" + DisplayName, Common.LogLevel.Normal);
						Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "getAllUsers Results: AccountDisabled:" + AccountDisabled, Common.LogLevel.Normal);
						Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "getAllUsers Results: IsValid:" + IsValid, Common.LogLevel.Normal);
						//myServer.ADQueryTest = "Pass";
						//**********************************************************ADD SQL STATEMENT*************************************************************************\\
						//string sqlQuery = "Insert into VSS_Statistics.dbo.MicrosoftDailyStats(ServerName,ServerTypeId,Date,StatName,StatValue,WeekNumber,MonthNumber,YearNumber,DayNumber, HourNumber, Details) "
						//        + " values('" + myServer.Name + "','" + myServer.ServerTypeId + "','" + dtNow + "','O365@QueryLatency#" + forest + "'" + " ," + seconds.ToString() +
						//       "," + weekNumber + ", " + dtNow.Month.ToString() + ", " + dtNow.Year.ToString() + ", " + dtNow.Day.ToString() + ", " + dtNow.Hour.ToString() + ", '')";
						//AllTestsList.SQLStatements.Add(new SQLstatements() { SQL = sqlQuery, DatabaseName = "VSS_Statistics" });

						//*******************************************************ALERTS???*****************************************************************\\

						//double threshold = 10;
						//double temp = 0;
						//if (Double.TryParse(seconds, out temp) && threshold > Double.Parse(seconds))
						//{
						//    //myServer.ADQueryTest = "Pass";
						//    Common.makeAlert(true, myServer, commonEnums.AlertType.AD_Query_Latency, ref AllTestsList, myServer.ServerType);
						//}
						//else
						//{
						//    //myServer.ADQueryTest = "Fail";
						//    Common.makeAlert(false, myServer, commonEnums.AlertType.AD_Query_Latency, ref AllTestsList, myServer.ServerType);

						//}
					}

				}
				else
				{
					//myServer.ADQueryTest = "Fail";
					//Common.makeAlert(true, myServer, commonEnums.AlertType.AD_Query_Latency, ref AllTestsList, myServer.ServerType);
				}
			}
			catch (Exception ex)
			{
				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "getAllUsers: Exception: " + ex.Message.ToString(), Common.LogLevel.Verbose);
				//myServer.ADQueryTest = "Fail";
			}
		}
		public void getAllDistributionGroups(MonitoredItems.Office365Server myServer, ref TestResults AllTestsList, ReturnPowerShellObjects powershellobj)
		{
			try
			{
				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "getMailboxes: Starting.", Common.LogLevel.Normal);
				System.Collections.ObjectModel.Collection<PSObject> results = new System.Collections.ObjectModel.Collection<PSObject>();
				//String str = " Get-Mailbox -ResultSize Unlimited | Select Name,Alais,DisplayName,StorageLimitStatus,membertype,servername,ProhibitSendQuota,LastLogonTime";
				string str = "Get-DistributionGroup | Select *";
				//powershellobj.PS.Commands.Clear();
				//powershellobj.PS.Streams.ClearStreams();
				powershellobj.PS.AddScript(str);
				results = powershellobj.PS.Invoke();

				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "getMailboxes Results: " + results.Count.ToString(), Common.LogLevel.Normal);
				DateTime dtNow = DateTime.Now;
				int weekNumber = culture.Calendar.GetWeekOfYear(dtNow, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday);
				if (results.Count > 0)
				{
					foreach (PSObject ps in results)
					{
						string displayName = ps.Properties["AccountSkuId"].Value == null ? "" : ps.Properties["AccountSkuId"].Value.ToString();
						string servername = ps.Properties["ConsumedUnits"].Value == null ? "" : ps.Properties["ConsumedUnits"].Value.ToString();
						string LastLogonTime = ps.Properties["ActiveUnits"].Value == null ? "" : ps.Properties["ActiveUnits"].Value.ToString();
						Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "getAllDistributionGroups Results: displayName:" + displayName, Common.LogLevel.Normal);
						Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "getAllDistributionGroups Results: servername:" + servername, Common.LogLevel.Normal);
						Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "getAllDistributionGroups Results: LastLogonTime:" + LastLogonTime, Common.LogLevel.Normal);
						//myServer.ADQueryTest = "Pass";
						//**********************************************************ADD SQL STATEMENT*************************************************************************\\
						//string sqlQuery = "Insert into VSS_Statistics.dbo.MicrosoftDailyStats(ServerName,ServerTypeId,Date,StatName,StatValue,WeekNumber,MonthNumber,YearNumber,DayNumber, HourNumber, Details) "
						//        + " values('" + myServer.Name + "','" + myServer.ServerTypeId + "','" + dtNow + "','O365@QueryLatency#" + forest + "'" + " ," + seconds.ToString() +
						//       "," + weekNumber + ", " + dtNow.Month.ToString() + ", " + dtNow.Year.ToString() + ", " + dtNow.Day.ToString() + ", " + dtNow.Hour.ToString() + ", '')";
						//AllTestsList.SQLStatements.Add(new SQLstatements() { SQL = sqlQuery, DatabaseName = "VSS_Statistics" });

						//*******************************************************ALERTS???*****************************************************************\\

						//double threshold = 10;
						//double temp = 0;
						//if (Double.TryParse(seconds, out temp) && threshold > Double.Parse(seconds))
						//{
						//    //myServer.ADQueryTest = "Pass";
						//    Common.makeAlert(true, myServer, commonEnums.AlertType.AD_Query_Latency, ref AllTestsList, myServer.ServerType);
						//}
						//else
						//{
						//    //myServer.ADQueryTest = "Fail";
						//    Common.makeAlert(false, myServer, commonEnums.AlertType.AD_Query_Latency, ref AllTestsList, myServer.ServerType);

						//}
					}

				}
			}
			catch (Exception ex)
			{
				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "getAllDistributionGroups Results: Exception: " + ex.Message.ToString(), Common.LogLevel.Verbose);
				//myServer.ADQueryTest = "Fail";
			}
		}
		public void getAllMsolUsers(MonitoredItems.Office365Server myServer, ref TestResults AllTestsList, ReturnPowerShellObjects powershellobj)
		{
			try
			{
				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "getAllMsolUsers: Starting.", Common.LogLevel.Normal);
				System.Collections.ObjectModel.Collection<PSObject> results = new System.Collections.ObjectModel.Collection<PSObject>();
				//String str = " Get-Mailbox -ResultSize Unlimited | Select Name,Alais,DisplayName,StorageLimitStatus,membertype,servername,ProhibitSendQuota,LastLogonTime";
				string str = "Get-MsolUser";
				//powershellobj.PS.Commands.Clear();
				//powershellobj.PS.Streams.ClearStreams();
				powershellobj.PS.AddScript(str);
				results = powershellobj.PS.Invoke();

				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "getAllMsolUsers Results: " + results.Count.ToString(), Common.LogLevel.Normal);
				DateTime dtNow = DateTime.Now;
				int weekNumber = culture.Calendar.GetWeekOfYear(dtNow, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday);
				if (results.Count > 0)
				{
					foreach (PSObject ps in results)
					{
						string displayName = ps.Properties["AccountSkuId"].Value == null ? "" : ps.Properties["AccountSkuId"].Value.ToString();
						string servername = ps.Properties["ConsumedUnits"].Value == null ? "" : ps.Properties["ConsumedUnits"].Value.ToString();
						string LastLogonTime = ps.Properties["ActiveUnits"].Value == null ? "" : ps.Properties["ActiveUnits"].Value.ToString();
						Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "getAllMsolUsers Results: displayName:" + displayName, Common.LogLevel.Normal);
						Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "getAllMsolUsers Results: servername:" + servername, Common.LogLevel.Normal);
						Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "getAllMsolUsers Results: LastLogonTime:" + LastLogonTime, Common.LogLevel.Normal);
						//myServer.ADQueryTest = "Pass";
						//**********************************************************ADD SQL STATEMENT*************************************************************************\\
						//string sqlQuery = "Insert into VSS_Statistics.dbo.MicrosoftDailyStats(ServerName,ServerTypeId,Date,StatName,StatValue,WeekNumber,MonthNumber,YearNumber,DayNumber, HourNumber, Details) "
						//        + " values('" + myServer.Name + "','" + myServer.ServerTypeId + "','" + dtNow + "','O365@QueryLatency#" + forest + "'" + " ," + seconds.ToString() +
						//       "," + weekNumber + ", " + dtNow.Month.ToString() + ", " + dtNow.Year.ToString() + ", " + dtNow.Day.ToString() + ", " + dtNow.Hour.ToString() + ", '')";
						//AllTestsList.SQLStatements.Add(new SQLstatements() { SQL = sqlQuery, DatabaseName = "VSS_Statistics" });

						//*******************************************************ALERTS???*****************************************************************\\

						//double threshold = 10;
						//double temp = 0;
						//if (Double.TryParse(seconds, out temp) && threshold > Double.Parse(seconds))
						//{
						//    //myServer.ADQueryTest = "Pass";
						//    Common.makeAlert(true, myServer, commonEnums.AlertType.AD_Query_Latency, ref AllTestsList, myServer.ServerType);
						//}
						//else
						//{
						//    //myServer.ADQueryTest = "Fail";
						//    Common.makeAlert(false, myServer, commonEnums.AlertType.AD_Query_Latency, ref AllTestsList, myServer.ServerType);

						//}
					}

				}
				else
				{
					//myServer.ADQueryTest = "Fail";
				}
			}
			catch (Exception ex)
			{
				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "getAllMSolUsers: Exception: " + ex.Message.ToString(), Common.LogLevel.Verbose);
				//myServer.ADQueryTest = "Fail";
			}
		}
		private void updateResults(MonitoredItems.Office365Server Server, ref TestResults AllTestResults)
		{
			//AllTestResults.SQLStatements.Add(new SQLstatements() { SQL = "delete from dbo.ActiveDirectoryTest where ServerId=" + Server.ServerId, DatabaseName = "vitalsigns" });
			//string sSQL = " INSERT INTO DBO.ActiveDirectoryTest(ServerId,LogonTest,QueryTest,LDApPortTest,LastScanDate) values(" + Server.ServerId + ",'" + Server.ADLogonTest + "','" + Server.ADQueryTest + "','" + Server.ADPortTest + "','" + DateTime.Now.ToString() +"')";
			//AllTestResults.SQLStatements.Add(new SQLstatements() { SQL = sSQL, DatabaseName = "vitalsigns" });
		}

		#endregion
	}
	public class Parameters
	{
		public MonitoredItems.Office365Server myServer;
		public ReturnPowerShellObjects PSO;
		public TestResults TS;
	}
}
