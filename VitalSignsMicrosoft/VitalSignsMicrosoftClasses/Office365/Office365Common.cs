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

//using System.Net.Http;
//using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Configuration;
using System.Collections;
using System.Xml.Linq;

namespace VitalSignsMicrosoftClasses
{
	class Office365Common
	{
		CultureInfo culture = CultureInfo.CurrentCulture;
		public string vbCrLf = System.Environment.NewLine;
		public string nodeName = "";
		DateTime time = DateTime.Now;              // Use current time
		string format = "t";
        List<VSNext.Mongo.Entities.MobileDeviceTranslations> listOfOsTranslations = new List<VSNext.Mongo.Entities.MobileDeviceTranslations>();
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
                getMsolAccountSku(Server, ref AllTestsList, results);
                //getMsolCompanyInfo(Server, ref AllTestsList, results);
				//getUserswithLicencesandServices(Server, ref AllTestsList, results);
				//getServiceStatus(Server, ref AllTestsList, results);
				//getMobileStats(p);//soma
				//Common.CommonDailyTasks(testServer, ref AllTestResults, testServer.ServerType);
				if (Server.EnableAutoDiscoveryTest)
					try
					{
						TestAutoDiscovery(Server, ref AllTestsList);
					}
					catch (Exception ex)
					{ Common.WriteDeviceHistoryEntry(Server.ServerType, Server.Name, "Error with Auto Discovery Test: " + ex.Message.ToString(), Common.LogLevel.Normal); }

				if (Server.EnableIMAPTest)
					try
					{
						TestIMAP(Server, ref AllTestsList);
					}
					catch (Exception ex)
					{ Common.WriteDeviceHistoryEntry(Server.ServerType, Server.Name, "Error with IMAP Test: " + ex.Message.ToString(), Common.LogLevel.Normal); }
				if (Server.EnableSMTPTest)
					try
					{
						TestSMTP(Server, ref AllTestsList);
					}
					catch (Exception ex)

					{ Common.WriteDeviceHistoryEntry(Server.ServerType, Server.Name, "Error with SMTP Test: " + ex.Message.ToString(), Common.LogLevel.Normal); }
				if (Server.EnablePOPTest)
					try
					{
						TestPOP(Server, ref AllTestsList);
					}
					catch (Exception ex)
					{ Common.WriteDeviceHistoryEntry(Server.ServerType, Server.Name, "Error with POP Test: " + ex.Message.ToString(), Common.LogLevel.Normal); }
				if (Server.EnableMAPIConnectivityTest)
					try
					{
						TestMAPIConectivity(Server, ref AllTestsList, results);
					}
					catch (Exception ex)
					{ Common.WriteDeviceHistoryEntry(Server.ServerType, Server.Name, "Error with MAPIConnectivity Test: " + ex.Message.ToString(), Common.LogLevel.Normal); }
				doLyncTests(p);
				if (Server.EnableCreateSiteTest)
				{
					doSPOTests(p);
				}


				//o365Th = new Thread(() => getMailBoxStats(p));
				//o365Th.Name = Server.Name + " getMailBoxStats";
				//WaitForThread(o365Th, Server);
				//getMailBoxStats(p);

				//o365Th = new Thread(() => getMobileStats(p));
				//o365Th.Name = Server.Name + " getMobileStats";
				//WaitForThread(o365Th, Server);
				getMobileStats(p);
                getMobileUsersHourly(p.myServer, ref p.TS, p.PSO);
				//Common.WriteDeviceHistoryEntry(Server.ServerType, Server.Name, "do REST API Tests ", Common.LogLevel.Normal);

				//o365Th = new Thread(() => doAPITests(p));
				//o365Th.Name = Server.Name + " doAPITests";
				//WaitForThread(o365Th, Server);

				//getMailboxes(Server, ref AllTestsList, results);
				//getMailboxeDetails(Server, ref AllTestsList, results);
				
				//getAllUsers(Server, ref AllTestsList, results);
				//getMobileUsers(Server, ref AllTestsList, results);
				//getMailboxActivity(Server, ref AllTestsList, results);


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
            return;
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
			string cmdlets = "-CommandName Test-MAPIConnectivity,Get-MailboxActivityReport,Get-Mailbox,Get-MailboxStatistics,Get-MsolAccountSku,Get-MsolUser,Get-User,Get-DistributionGroup,Get-MobileDeviceStatistics,Get-MobileDevice,Get-MsolCompanyInformation,Get-CsActiveUserReport,Get-CsClientDeviceReport,Get-CsP2PAVTimeReport,Get-CsConferenceReport,Get-CsP2PSessionReport,Get-MessageTrace,Get-Recipient";
			Common.WriteDeviceHistoryEntry(Server.ServerType, Server.Name, "Before  PrereqForOffice365WithCmdlets", Common.LogLevel.Normal);
			//pre-check to see if ADFS /Normal
			ReturnPowerShellObjects results = Common.PrereqForOffice365WithCmdlets(Server.Name, Server.UserName, Server.Password, Server.ServerType, Server.IPAddress, commonEnums.ServerRoles.Windows, cmdlets, Server);

			Common.WriteDeviceHistoryEntry(Server.ServerType, Server.Name, "After  PrereqForOffice365WithCmdlets", Common.LogLevel.Normal);
			if (results.Connected == false)
			{
				if (Server.AuthenticationTest == false)
				{
					Common.makeAlert(false, Server, commonEnums.AlertType.Authentication, ref AllTestsList, "Failed to Authenticate ", "Connectivity");
					Common.makeAlert(false, Server, commonEnums.AlertType.Not_Responding, ref AllTestsList, "Tenant is Not Responding. Failed to Authenticate with the provided credentials", "Connectivity");
				}
				else
				{
					Common.makeAlert(true, Server, commonEnums.AlertType.Authentication, ref AllTestsList, "Successfully authenticated using provided credentials", "Connectivity");
					Common.makeAlert(false, Server, commonEnums.AlertType.Not_Responding, ref AllTestsList, "Tenant is Not Responding. Power Shell connectivity failed ", "Connectivity");
				}



				Common.WriteDeviceHistoryEntry(Server.ServerType, Server.Name, " checkServer: PS SESSION IS NULL", Common.LogLevel.Normal);
				Server.Status = "Not Responding";
				Server.StatusCode = "Not Responding";
				//***************************************************Not Responding********************************************//
			}
			else
			{
				Common.makeAlert(true, Server, commonEnums.AlertType.Authentication, ref AllTestsList, "Successfully authenticated using provided credentials", "Connectivity");
				Common.makeAlert(true, Server, commonEnums.AlertType.Not_Responding, ref AllTestsList, "Tenant is Responding", "Connectivity");
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
						
						if (runNumber != "")
						{
							string lastTime = getDirSyncLastRun(myServer, creds, servername, AllTestsList, PSO, dRunNumber.ToString());
							//calculate the difference between this run and last run
							DateTime dtThisRun = Convert.ToDateTime(runStartTime);
							DateTime dtLastRun = Convert.ToDateTime(lastTime);
							double iTimeDiff = dtThisRun.Subtract(dtLastRun).TotalMinutes;
                            
						Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "in DirSyncExportTest getDirSyncStats: ", Common.LogLevel.Verbose);
						if (myServer.DirSyncExportTest)
						{
							if (runStatus.ToLower() == "success" || runStatus.ToLower() == "in-progress")
                            {
                                if ((myServer.DirSyncExportThreshold != 0) && (myServer.DirSyncExportThreshold > 0) && (myServer.DirSyncExportThreshold < iTimeDiff))
									Common.makeAlert(false, myServer, commonEnums.AlertType.DirSync_Export, ref AllTestsList, "Dir Sync Export in " + iTimeDiff + " minutes last successful at UTC time: " + runEndTime + " , but it did not meet the threshold of " + myServer.DirSyncExportThreshold.ToString(), "Dir Sync");
							else
                                    Common.makeAlert(true, myServer, commonEnums.AlertType.DirSync_Export, ref AllTestsList, " Dir Sync Export was last successful at UTC time: " + runEndTime, "Dir Sync");
						}
								
							else
								Common.makeAlert(false, myServer, commonEnums.AlertType.DirSync_Export, ref AllTestsList, "Dir Sync Failed. Last Successful UTC time at: " + runEndTime, "Dir Sync");
						}

							DateTime dtNow = DateTime.Now;
							int weekNumber = culture.Calendar.GetWeekOfYear(dtNow, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday);
							string sqlQuery = "Insert into VSS_Statistics.dbo.MicrosoftDailyStats(ServerName,ServerTypeId,Date,StatName,StatValue,WeekNumber,MonthNumber,YearNumber,DayNumber, HourNumber, Details) "
									+ " values('" + myServer.Name + "','" + myServer.ServerTypeId + "',GetDate(),'DirSyncActual" + "@" + nodeName + "'" + " ," + iTimeDiff.ToString() +
								   "," + weekNumber + ", " + dtNow.Month.ToString() + ", " + dtNow.Year.ToString() + ", " + dtNow.Day.ToString() + ", " + dtNow.Hour.ToString() + ", '')";
							//AllTestsList.SQLStatements.Add(new SQLstatements() { SQL = sqlQuery, DatabaseName = "VSS_Statistics" });
                            AllTestsList.MongoEntity.Add(Common.GetInsertIntoDailyStats(myServer, "DirSyncActual" + "@" + nodeName, iTimeDiff.ToString()));
							string sqlQuery2 = "Insert into VSS_Statistics.dbo.MicrosoftDailyStats(ServerName,ServerTypeId,Date,StatName,StatValue,WeekNumber,MonthNumber,YearNumber,DayNumber, HourNumber, Details) "
									+ " values('" + myServer.Name + "','" + myServer.ServerTypeId + "',GetDate(),'DirSyncEstimated" + "@" + nodeName + "'" + " ," + myServer.DirSyncExportThreshold.ToString() +
								   "," + weekNumber + ", " + dtNow.Month.ToString() + ", " + dtNow.Year.ToString() + ", " + dtNow.Day.ToString() + ", " + dtNow.Hour.ToString() + ", '')";
							//AllTestsList.SQLStatements.Add(new SQLstatements() { SQL = sqlQuery2, DatabaseName = "VSS_Statistics" });
                            AllTestsList.MongoEntity.Add(Common.GetInsertIntoDailyStats(myServer, "DirSyncEstimated" + "@" + nodeName, myServer.DirSyncExportThreshold.ToString()));

						}

					}
					else
					{
						if (myServer.DirSyncImportTest)
						{
                            string lastTime = getDirSyncLastRun(myServer, creds, servername, AllTestsList, PSO, dRunNumber.ToString());
                            //calculate the difference between this run and last run
                            DateTime dtThisRun = Convert.ToDateTime(runStartTime);
                            DateTime dtLastRun = Convert.ToDateTime(lastTime);
                            double iTimeDiff = dtThisRun.Subtract(dtLastRun).TotalMinutes;

							if (runStatus.ToLower() == "success" || runStatus.ToLower() == "in-progress")
                            {
                                if ((myServer.DirSyncImportThreshold != 0) && (myServer.DirSyncImportThreshold > 0) && (myServer.DirSyncImportThreshold < iTimeDiff))
									Common.makeAlert(false, myServer, commonEnums.AlertType.DirSync_Import, ref AllTestsList, " Dir Sync import in " + iTimeDiff + " minutes last successful at UTC time: " + runEndTime + " , but it did not meet the threshold of " + myServer.DirSyncImportThreshold.ToString() + " minutes", "Dir Sync");
							else
                                    Common.makeAlert(true, myServer, commonEnums.AlertType.DirSync_Import, ref AllTestsList, " Dir Sync Import was last successful at UTC time: " + runEndTime, "Dir Sync");

                            }
							else
								Common.makeAlert(false, myServer, commonEnums.AlertType.DirSync_Import, ref AllTestsList, "Dir Sync Failed. Last Successful UTC time at: " + runEndTime, "Dir Sync");
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
                //List<VSNext.Mongo.Entities.Office365> list = new List<VSNext.Mongo.Entities.Office365>();
                
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

                    VSNext.Mongo.Entities.Office365 o365 = new VSNext.Mongo.Entities.Office365();
                    o365.ServerId = myServer.ServerId;
                    o365.AccountName = AccountName;
                    o365.ActiveUnits = iActiveUnits.ToString();
                    o365.ConsumedUnits = iConsumedUnits;
                    o365.WarningUnits = iWarningUnits;
                    o365.LicenseType = LicenseType;


					objSQL.onTrueDML = "UPDATE Office365AccountStats set AccountName='" + AccountName + "', ActiveUnits=" + iActiveUnits.ToString() + ", ConsumedUnits=" + iConsumedUnits.ToString() + ",WarningUnits=" + iWarningUnits.ToString() + ",LicenseType='" + LicenseType + "',LastUpdatedDate='" + DateTime.Now + "' Where ServerId=" + myServer.ServerId.ToString();
					string sqlQuery = objSQL.GetSQL(objSQL);
					//AllTestsList.SQLStatements.Add(new SQLstatements() { SQL = sqlQuery, DatabaseName = "vitalsigns" });
                    //list.Add(o365);

                    MongoStatementsUpsert<VSNext.Mongo.Entities.Office365> updateStatement = new MongoStatementsUpsert<VSNext.Mongo.Entities.Office365>();
                    updateStatement.filterDef = updateStatement.repo.Filter.Where(i => i.AccountName == o365.AccountName && i.ServerId == o365.ServerId );
                    updateStatement.updateDef = updateStatement.repo.Updater.Set(i => i.ActiveUnits, o365.AccountName).Set(i => i.ConsumedUnits, o365.ConsumedUnits)
                        .Set(i => i.WarningUnits, o365.WarningUnits)
                        .Set(i => i.LicenseType, o365.LicenseType)
                        .Set(i => i.ServerId, o365.ServerId);

                    AllTestsList.MongoEntity.Add(updateStatement);
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
                List<VSNext.Mongo.Entities.Office365> list = new List<VSNext.Mongo.Entities.Office365>();
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
						//AllTestsList.SQLStatements.Add(new SQLstatements() { SQL = sqlQuery, DatabaseName = "vitalsigns" });
                        VSNext.Mongo.Entities.Office365 o365 = new VSNext.Mongo.Entities.Office365();
                        o365.ServerId = myServer.ServerId;
                        o365.PreferredLanguage = PreferredLanguage;
                        o365.Street = Street.ToString();
                        o365.City = City;
                        o365.State = State;
                        o365.PostalCode = PostalCode;
                        o365.Telephone = TelephoneNumber;
                        o365.Country = Country;
                        o365.TechnicalNotificationEmails = TechnicalNotificationEmails;

                        MongoStatementsUpsert<VSNext.Mongo.Entities.Office365> updateStatement = new MongoStatementsUpsert<VSNext.Mongo.Entities.Office365>();
                        updateStatement.filterDef = updateStatement.repo.Filter.Where(i => i.ServerId == o365.ServerId);
                        updateStatement.updateDef = updateStatement.repo.Updater.Set(i => i.PreferredLanguage, o365.PreferredLanguage).Set(i => i.Street, o365.Street)
                            .Set(i => i.City, o365.City)
                            .Set(i => i.State, o365.State)
                            .Set(i => i.PostalCode, o365.PostalCode)
                            .Set(i => i.Telephone, o365.Telephone)
                            .Set(i => i.Country, o365.Country)
                             .Set(i => i.TechnicalNotificationEmails, o365.TechnicalNotificationEmails);

                        AllTestsList.MongoEntity.Add(updateStatement);

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
				//string str = "Get-Mailbox -ResultSize Unlimited | Get-MailboxStatistics | Select DisplayName,Database,TotalItemSize,ItemCount,StorageLimitStatus,ServerName,LastLogonTime,LastLogoffTime";
                string str ="$results=@() \n";
str +="Get-Mailbox -ResultSize unlimited| select Database,ServerName,identity | % { \n";
str +="$stats=Get-MailboxStatistics -Identity $_.identity |select TotalItemSize,ItemCount,StorageLimitStatus,LastLogonTime,LastLogoffTime,DisplayName \n";
str +="$stats |Add-Member -Type NoteProperty -Name Database -Value $_.Database    \n";
str +="$stats |Add-Member -Type NoteProperty -Name ServerName -Value $_.ServerName  \n";
str +="$results +=($stats) \n";
str +="} \n";
str +="$results \n";
str += "Clear-Variable 'results' -ErrorAction SilentlyContinue \n";
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
						if (DisplayName != "Discovery Search Mailbox")
						{
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
								",[ItemCount]=" + ItemCount + ",[StorageLimitStatus]='" + StorageLimitStatus + "' Where Server='" + myServer.Name.ToString() + "' AND DisplayName='" + DisplayName + "'";
							string sqlQuery = objSQL.GetSQL(objSQL);
							AllTestsList.SQLStatements.Add(new SQLstatements() { SQL = sqlQuery, DatabaseName = "VSS_Statistics" });

							//string sqlQuery = "INSERT INTO ExchangeMailFiles ([ScanDate],[Database],[DisplayName],[Server],[TotalItemSizeInMB],[ItemCount],[StorageLimitStatus]) VALUES " +
							//    "('" + DateTime.Now + "','" + Database + "','" + DisplayName + "','" + myServer.Name + "','" + totalItemSize + "','" + ItemCount + "','" + StorageLimitStatus + "')";


							//AllTestsList.SQLStatements.Add(new SQLstatements() { SQL = sqlQuery, DatabaseName = "VSS_Statistics" });

							// Details
							//sqlQuery = "INSERT INTO dbo.O365AdditionalMailDetails ([Server],[LastLogonTime],[LastLoggoffTime],[DisplayName]) VALUES " +
							//    "('" + myServer.Name + "','" + LastLogonTime + "','" + LastLogoffTime + "','" + DisplayName + "')";


							//AllTestsList.SQLStatements.Add(new SQLstatements() { SQL = sqlQuery, DatabaseName = "VSS_Statistics" });

                            ////SQLBuild objSQL2 = new SQLBuild();
                            ////objSQL2.ifExistsSQLSelect = "SELECT * FROM O365AdditionalMailDetails WHERE Server='" + myServer.Name + "' AND DisplayName='" + DisplayName + "'";
                            ////if (LastLogonTime != "")
                            ////    objSQL2.onFalseDML = "INSERT INTO dbo.O365AdditionalMailDetails ([Server],[LastLogonTime],[LastLoggoffTime],[DisplayName]) VALUES " +
                            ////        "('" + myServer.Name + "','" + LastLogonTime + "','" + LastLogoffTime + "','" + DisplayName + "')";
                            ////else
                            ////    objSQL2.onFalseDML = "INSERT INTO dbo.O365AdditionalMailDetails ([Server],[DisplayName]) VALUES " +
                            ////    "('" + myServer.Name + "','" + DisplayName + "')";

                            ////if (LastLogonTime != "")
                            ////    objSQL2.onTrueDML = "UPDATE dbo.O365AdditionalMailDetails set [LastLogonTime]='" + LastLogonTime + "',[LastLoggoffTime]='" + LastLogoffTime + "' Where Server='" + myServer.Name.ToString() + "' AND DisplayName='" + DisplayName + "'";
                            ////else
                            ////    objSQL2.onTrueDML = "UPDATE dbo.O365AdditionalMailDetails set DisplayName='" + DisplayName + "' Where Server='" + myServer.Name.ToString() + "' AND DisplayName='" + DisplayName + "'";

                            ////string sqlQuery2 = objSQL2.GetSQL(objSQL2);
                            ////AllTestsList.SQLStatements.Add(new SQLstatements() { SQL = sqlQuery2, DatabaseName = "VSS_Statistics" });

                            MongoStatementsUpsert<VSNext.Mongo.Entities.Mailbox> mongoStatement = new MongoStatementsUpsert<VSNext.Mongo.Entities.Mailbox>();
                            mongoStatement.filterDef = mongoStatement.repo.Filter.Where(i => i.DatabaseName == Database && i.DisplayName == DisplayName && i.DeviceName == myServer.Name);
                            mongoStatement.updateDef = mongoStatement.repo.Updater
                                //.Set(i => i.IssueWarningQuota, IssueWarningQuota)
                                //.Set(i => i.ProhibitSendQuota, ProhibitSendQuota)
                                //.Set(i => i.ProhibitSendReceiveQuota, ProhibitSendReceiveQuota)
                                .Set(i => i.DisplayName, DisplayName)
                                .Set(i => i.DatabaseName, Database)
                                .Set(i => i.TotalItemSizeMb, Convert.ToDouble(totalItemSize))
                                .Set(i => i.ItemCount, Convert.ToInt32(ItemCount))
                                .Set(i => i.StorageLimitStatus, StorageLimitStatus)
                                .Set(i => i.LastLogonTime , Convert.ToDateTime(LastLogonTime))
                                .Set(i => i.LastLogoffTime, Convert.ToDateTime(LastLogoffTime));

                            AllTestsList.MongoEntity.Add(mongoStatement);
						}

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
						if (DisplayName != "Discovery Search Mailbox")
						{
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

                            //SQLBuild objSQL = new SQLBuild();
                            //objSQL.ifExistsSQLSelect = "SELECT * FROM ExchangeMailFiles WHERE Server='" + myServer.Name + "' AND DisplayName='" + DisplayName + "'";
                            //objSQL.onFalseDML = "INSERT INTO ExchangeMailFiles ([ScanDate],[Database],[DisplayName],[IssueWarningQuota],[ProhibitSendQuota],[ProhibitSendReceiveQuota],[Server]) VALUES " +
                            //    "('" + DateTime.Now + "','" + Database + "','" + DisplayName + "','" + IssueWarningQuota + "','" + ProhibitSendQuota + "','" + ProhibitSendReceiveQuota + "','" + myServer.Name + "')";




                            //objSQL.onTrueDML = "UPDATE ExchangeMailFiles set IssueWarningQuota='" + IssueWarningQuota + "',ProhibitSendQuota='" + ProhibitSendQuota + "',ProhibitSendReceiveQuota='" + ProhibitSendReceiveQuota +
                            //    "',ScanDate='" + DateTime.Now + "' Where Server='" + myServer.Name.ToString() + "' AND DisplayName='" + DisplayName + "'";
                            //string sqlQuery = objSQL.GetSQL(objSQL);
                            //AllTestsList.SQLStatements.Add(new SQLstatements() { SQL = sqlQuery, DatabaseName = "VSS_Statistics" });

                            //// Details
                            //SQLBuild objSQL2 = new SQLBuild();
                            //objSQL2.ifExistsSQLSelect = "SELECT * FROM O365AdditionalMailDetails WHERE Server='" + myServer.Name + "' AND DisplayName='" + DisplayName + "'";
                            //objSQL2.onFalseDML = "INSERT INTO dbo.O365AdditionalMailDetails ([Server],[MailBoxType],[IsActive],[DisplayName]) VALUES " +
                            //    "('" + myServer.Name + "','" + RecipientTypeDetails + "','" + IsInactiveMailbox + "','" + DisplayName + "')";




                            //objSQL2.onTrueDML = "UPDATE dbo.O365AdditionalMailDetails set MailBoxType='" + RecipientTypeDetails + "',IsActive='" + IsInactiveMailbox + "' Where Server='" + myServer.Name + "' AND DisplayName='" + DisplayName + "'";
                            //string sqlQuery2 = objSQL2.GetSQL(objSQL2);
                            //AllTestsList.SQLStatements.Add(new SQLstatements() { SQL = sqlQuery2, DatabaseName = "VSS_Statistics" });

                            MongoStatementsUpsert<VSNext.Mongo.Entities.Mailbox> mongoStatement = new MongoStatementsUpsert<VSNext.Mongo.Entities.Mailbox>();
                            mongoStatement.filterDef = mongoStatement.repo.Filter.Where(i => i.DatabaseName == Database && i.DisplayName == DisplayName && i.DeviceName == myServer.Name);
                            mongoStatement.updateDef = mongoStatement.repo.Updater
                                .Set(i => i.IssueWarningQuota, IssueWarningQuota)
                                .Set(i => i.ProhibitSendQuota, ProhibitSendQuota)
                                .Set(i => i.ProhibitSendReceiveQuota, ProhibitSendReceiveQuota)
                                .Set(i => i.DisplayName, DisplayName)
                                .Set(i => i.DatabaseName, Database)
                                .Set(i => i.IsActive, IsInactiveMailbox)
                                .Set(i => i.MailboxType, RecipientTypeDetails);
                                //.Set(i => i.TotalItemSizeMb, Convert.ToDouble(totalItemSize))
                                //.Set(i => i.ItemCount, Convert.ToInt32(ItemCount))
                                //.Set(i => i.StorageLimitStatus, StorageLimitStatus)
                                //.Set(i => i.LastLogonTime, Convert.ToDateTime(LastLogonTime))
                                //.Set(i => i.LastLogoffTime, Convert.ToDateTime(LastLogoffTime));

                            AllTestsList.MongoEntity.Add(mongoStatement);


						}
					}
                    results = null;
                    GC.Collect();

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

                        MongoStatementsUpsert<VSNext.Mongo.Entities.Office365> updateStatement = new MongoStatementsUpsert<VSNext.Mongo.Entities.Office365>();
                        updateStatement.filterDef = updateStatement.repo.Filter.Where(i =>  i.ServerId == myServer.ServerId);
                        updateStatement.updateDef = updateStatement.repo.Updater.Set(i => i.TotalActiveUserMailboxes, TotalActiveUserMailBoxes);
                        AllTestsList.MongoEntity.Add(updateStatement);

						break;
					}

				}

			}
			catch (Exception ex)
			{
				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "getMsolAccountSku Results: Exception: " + ex.Message.ToString(), Common.LogLevel.Verbose);
			}
		}

		public void getMsolGroups(MonitoredItems.Office365Server myServer, ref TestResults AllTestsList, ReturnPowerShellObjects powershellobj)
		{
			try
			{
                MongoStatementsInsert<VSNext.Mongo.Entities.Office365Groups> msi = new MongoStatementsInsert<VSNext.Mongo.Entities.Office365Groups>();
				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "getMSOLGroups: Starting.", Common.LogLevel.Normal);
				System.Collections.ObjectModel.Collection<PSObject> results = new System.Collections.ObjectModel.Collection<PSObject>();
				String str = "Get-MSOLGroup -All";
				powershellobj.PS.Commands.Clear();
				powershellobj.PS.Streams.ClearStreams();
				powershellobj.PS.AddScript(str);
				results = powershellobj.PS.Invoke();
				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "getMSOLGroups Results: " + results.Count.ToString(), Common.LogLevel.Normal);
				DateTime dtNow = DateTime.Now;
				int weekNumber = culture.Calendar.GetWeekOfYear(dtNow, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday);
				System.Collections.ArrayList groupId = new System.Collections.ArrayList();
				if (results.Count > 0)
				{
					AllTestsList.SQLStatements.Add(new SQLstatements() { SQL = "delete from dbo.O365Groups where ServerId=" + myServer.ServerId.ToString(), DatabaseName = "Vitalsigns" });
					foreach (PSObject ps in results)
					{
						string objectId = ps.Properties["ObjectId"].Value == null ? "" : ps.Properties["ObjectId"].Value.ToString();
						groupId.Add(objectId);

						string displayName = ps.Properties["DisplayName"].Value == null ? "" : ps.Properties["DisplayName"].Value.ToString();
						string groupType = ps.Properties["GroupType"].Value == null ? "" : ps.Properties["GroupType"].Value.ToString();
						string groupDescription = ps.Properties["Description"].Value == null ? "" : ps.Properties["Description"].Value.ToString();


						string sqlQuery = "INSERT INTO dbo.O365Groups ([ServerId],[GroupId],[GroupName],[GroupType],[GroupDescription]) VALUES " +
							"(" + myServer.ServerId + ",'" + objectId + "','" + displayName + "','" + groupType + "','" + groupDescription + "')";
                        //VSNext.Mongo.Entities.Office365GroupMembers Office365Members = new VSNext.Mongo.Entities.Office365GroupMembers();
                        //Office365Members.UserPrincipleName = "ddd";
                        //VSNext.Mongo.Entities.Office365GroupMembers Office365Members2 = new VSNext.Mongo.Entities.Office365GroupMembers();
                        //Office365Members2.UserPrincipleName = "eee";
                        //MongoStatementsInsert<VSNext.Mongo.Entities.Office365GroupMembers> msil = new MongoStatementsInsert<VSNext.Mongo.Entities.Office365GroupMembers>();

                        //msil.listOfEntities.Add(Office365Members);
                        //msil.listOfEntities.Add(Office365Members2);


						//AllTestsList.SQLStatements.Add(new SQLstatements() { SQL = sqlQuery, DatabaseName = "Vitalsigns" });
                        VSNext.Mongo.Entities.Office365Groups Office365 = new VSNext.Mongo.Entities.Office365Groups();
                        Office365.ServerId = int.Parse(myServer.ServerId);
                        Office365.GroupId = objectId;
                        Office365.GroupName = displayName;
                        Office365.GroupType = groupType;
                        Office365.GroupDescription = groupDescription;
                        //Office365.Members = msil.listOfEntities.ToArray();

                        msi.listOfEntities.Add(Office365);

					}
                    MongoStatementsInsert<VSNext.Mongo.Entities.Office365GroupMembers> msil = new MongoStatementsInsert<VSNext.Mongo.Entities.Office365GroupMembers>();
                    foreach (VSNext.Mongo.Entities.Office365Groups s in msi.listOfEntities)
					{
						str = "Get-MsolGroupMember -groupObjectid '" + s.GroupId + "' | Select DisplayName,EmailAddress,GroupMemberType";
						powershellobj.PS.Commands.Clear();
						powershellobj.PS.Streams.ClearStreams();
						powershellobj.PS.AddScript(str);

						results = powershellobj.PS.Invoke();
						if (results.Count > 0)
						{
							AllTestsList.SQLStatements.Add(new SQLstatements() { SQL = "delete from dbo.O365UserGroups where GroupId='" + s.GroupId + "'", DatabaseName = "Vitalsigns" });
                            VSNext.Mongo.Entities.Office365GroupMembers Office365Members = new VSNext.Mongo.Entities.Office365GroupMembers();
							//AllTestsList.SQLStatements.Add(new SQLstatements() { SQL = "delete from dbo.O365Groups where ServerId=" + myServer.ServerId.ToString(), DatabaseName = "Vitalsigns" });
							foreach (PSObject ps in results)
							{
								string displayName = ps.Properties["DisplayName"].Value == null ? "" : ps.Properties["DisplayName"].Value.ToString();
								string groupMemberType = ps.Properties["GroupMemberType"].Value == null ? "" : ps.Properties["GroupMemberType"].Value.ToString();
								string EmailAddress = ps.Properties["EmailAddress"].Value == null ? "" : ps.Properties["EmailAddress"].Value.ToString();
								//string groupDescription = ps.Properties["Description"].Value == null ? "" : ps.Properties["Description"].Value.ToString();

								SQLBuild objSQL = new SQLBuild();
								objSQL.ifExistsSQLSelect = "SELECT * FROM dbo.O365MSOLUsers WHERE ServerId='" + myServer.ServerId + "' AND DisplayName='" + displayName.Replace("'", "''") + "'";
								objSQL.onFalseDML = "INSERT INTO dbo.O365MSOLUsers ([ServerId],[DisplayName],[UserPrincipalName],LastUpdated) VALUES " +
									"(" + myServer.ServerId + ",'" + displayName.Replace("'", "''") + "','" + EmailAddress + "','" + DateTime.Now.ToString() + "')";


								objSQL.onTrueDML = "UPDATE dbo.O365MSOLUsers set UserPrincipalName='" + EmailAddress + "' Where ServerId=" + myServer.ServerId.ToString() + " and DisplayName='" + displayName.Replace("'", "''") + "'";
								string sqlQuery = objSQL.GetSQL(objSQL);
								AllTestsList.SQLStatements.Add(new SQLstatements() { SQL = sqlQuery, DatabaseName = "Vitalsigns" });


								string strInsert = "insert into dbo.O365UserGroups(groupid,UserPrincipalName) values('" + s.GroupId + "','" + EmailAddress + "')";
								//AllTestsList.SQLStatements.Add(new SQLstatements() { SQL = strInsert, DatabaseName = "Vitalsigns" });
                                
                                Office365Members.UserPrincipleName = EmailAddress;

                                msil.listOfEntities.Add(Office365Members);
							}
                            s.Members = msil.listOfEntities.ToArray();
						}
                        results = null;
                        GC.Collect();

					}
                    AllTestsList.MongoEntity.Add(msi);

				}
                results = null;
                GC.Collect();
			}
			catch (Exception ex)
			{
				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "getMSOLGroups: Exception: " + ex.Message.ToString(), Common.LogLevel.Verbose);
			}
		}
		public void getMsolUsers(MonitoredItems.Office365Server myServer, ref TestResults AllTestsList, ReturnPowerShellObjects powershellobj)
		{
			try
			{
                            MongoStatementsInsert<VSNext.Mongo.Entities.Office365MSOLUsers> msi = new MongoStatementsInsert<VSNext.Mongo.Entities.Office365MSOLUsers>();

				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "getMSOLGroups: Starting.", Common.LogLevel.Normal);
				System.Collections.ObjectModel.Collection<PSObject> results = new System.Collections.ObjectModel.Collection<PSObject>();
				String str = "Get-MSOLUser -All | Select DisplayName,FirstName,LastName,UserPrincipalName,StrongPasswordRequired,PasswordNeverExpires,UserType,Title,IsLicensed,Department,{$_.Licenses.AccountSkuId},LastUpdated";
				powershellobj.PS.Commands.Clear();
				powershellobj.PS.Streams.ClearStreams();
				powershellobj.PS.AddScript(str);
				results = powershellobj.PS.Invoke();
				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "getMSOLGroups Results: " + results.Count.ToString(), Common.LogLevel.Normal);
				DateTime dtNow = DateTime.Now;
				int weekNumber = culture.Calendar.GetWeekOfYear(dtNow, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday);
				if (results.Count > 0)
				{
					AllTestsList.SQLStatements.Add(new SQLstatements() { SQL = "delete from dbo.O365MSOLUsers where ServerId=" + myServer.ServerId.ToString(), DatabaseName = "Vitalsigns" });
					foreach (PSObject ps in results)
					{
						string displayName = ps.Properties["DisplayName"].Value == null ? "" : ps.Properties["DisplayName"].Value.ToString();

						try
						{
							string firstName = ps.Properties["FirstName"].Value == null ? "" : ps.Properties["FirstName"].Value.ToString();
							string lastName = ps.Properties["LastName"].Value == null ? "" : ps.Properties["LastName"].Value.ToString();
							string userPrincipleName = ps.Properties["UserPrincipalName"].Value == null ? "" : ps.Properties["UserPrincipalName"].Value.ToString();
							string StrongPasswordRequired = ps.Properties["StrongPasswordRequired"].Value == null ? "0" : ps.Properties["StrongPasswordRequired"].Value.ToString();
							string PasswordNeverExpires = ps.Properties["PasswordNeverExpires"].Value == null ? "0" : ps.Properties["PasswordNeverExpires"].Value.ToString();
							string userType = ps.Properties["UserType"].Value == null ? "" : ps.Properties["UserType"].Value.ToString();
							string title = ps.Properties["Title"].Value == null ? "" : ps.Properties["Title"].Value.ToString();
							string isLicensed = ps.Properties["IsLicensed"].Value == null ? "0" : ps.Properties["IsLicensed"].Value.ToString();
							string department = ps.Properties["Department"].Value == null ? "" : ps.Properties["Department"].Value.ToString();
							string license = "";
							if (StrongPasswordRequired.ToLower() == "true")
							{
								StrongPasswordRequired = "1";
							}
							else
							{
								StrongPasswordRequired = "0";
							}
							if (PasswordNeverExpires.ToLower() == "true")
							{
								PasswordNeverExpires = "1";
							}
							else
							{
								PasswordNeverExpires = "0";
							}

							if (isLicensed.ToLower() == "true")
								isLicensed = "1";
							else
								isLicensed = "0";

							if (isLicensed == "1")
								license = ps.Properties["$_.Licenses.AccountSkuId"].Value == null ? "" : ps.Properties["$_.Licenses.AccountSkuId"].Value.ToString();

							string sqlQuery = "INSERT INTO dbo.O365MSOLUsers ([ServerId],[DisplayName],[FirstName],[LastName],[UserPrincipalName],StrongPasswordRequired,PasswordNeverExpires,UserType,Title,IsLicensed,Department,LastUpdated,License) VALUES " +
								"(" + myServer.ServerId + ",'" + displayName.Replace("'", "''") + "','" + firstName.Replace("'", "''") + "','" + lastName.Replace("'", "''") + "','" + userPrincipleName.Replace("'", "''") + "','" + StrongPasswordRequired + "','" + PasswordNeverExpires + "','" + userType + "','" + title.Replace("'", "''") + "','" + isLicensed + "','" + department.Replace("'", "''") + "','" + DateTime.Now.ToString() + "','" + license + "')";


                            VSNext.Mongo.Entities.Office365MSOLUsers Office365MSOLUsers = new VSNext.Mongo.Entities.Office365MSOLUsers();
                            Office365MSOLUsers.ServerId = int.Parse(myServer.ServerId);
                            Office365MSOLUsers.FirstName = firstName;
                            Office365MSOLUsers.LastName = lastName ;
                            Office365MSOLUsers.DisplayName = displayName;
                            Office365MSOLUsers.UserPrincipalName = userPrincipleName;
                            Office365MSOLUsers.StrongPasswordRequired = StrongPasswordRequired;
                            Office365MSOLUsers.PasswordNeverExpires = PasswordNeverExpires;
                            Office365MSOLUsers.UserType = userType;
                            Office365MSOLUsers.Title = title;
                            Office365MSOLUsers.IsLicensed = isLicensed;
                            Office365MSOLUsers.Department = department;

                            msi.listOfEntities.Add(Office365MSOLUsers);
                            

							//AllTestsList.SQLStatements.Add(new SQLstatements() { SQL = sqlQuery, DatabaseName = "Vitalsigns" });
                            //MongoStatementsInsert<VSNext.Mongo.Entities.Office365MSOLUsers> updateStatement = new MongoStatementsInsert<VSNext.Mongo.Entities.Office365MSOLUsers>();

                            //updateStatement.listOfEntities = updateStatement.repo.Updater.Set(i => i.ServerId, Convert.ToInt32(myServer.ServerId)).Set(i => i.FirstName, firstName)
                            //    .Set(i => i.LastName, lastName)
                            //    .Set(i => i.DisplayName, displayName)
                            //    .Set(i => i.UserPrincipalName, userPrincipleName)
                            //.Set(i => i.StrongPasswordRequired, StrongPasswordRequired)
                            //.Set(i => i.PasswordNeverExpires, PasswordNeverExpires)
                            //.Set(i => i.UserType, userType)
                            //.Set(i => i.Title, title)
                            //.Set(i => i.IsLicensed, isLicensed)
                            //.Set(i => i.Department, department);

                            //AllTestsList.MongoEntity.Add(updateStatement);
						}
                           
						catch (Exception ex)
						{
							Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "getMSOLGroups: Exception getting user: " + displayName + " Ex:" + ex.Message.ToString(), Common.LogLevel.Verbose);
						}

					}
                    AllTestsList.MongoEntity.Add(msi);

				}
                results = null;
                GC.Collect();
			}
			catch (Exception ex)
			{
				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "getMSOLGroups: Exception: " + ex.Message.ToString(), Common.LogLevel.Verbose);
			}
		}

		#endregion
		#region mobileDevices
		public void getMobileUsers(MonitoredItems.Office365Server myServer, ref TestResults AllTestsList, ReturnPowerShellObjects powershellobj)
		{

			try
			{
                CommonDB db = new CommonDB();
                VSNext.Mongo.Repository.Repository<VSNext.Mongo.Entities.MobileDeviceTranslations> repoTranslations = new VSNext.Mongo.Repository.Repository<VSNext.Mongo.Entities.MobileDeviceTranslations>(db.GetMongoConnectionString());
                FilterDefinition<VSNext.Mongo.Entities.MobileDeviceTranslations> filterDefTranslations = repoTranslations.Filter.Eq(x => x.Type, "OS");
                List<VSNext.Mongo.Entities.MobileDeviceTranslations> tempList = repoTranslations.Find(filterDefTranslations).ToList();
                if (tempList.Count > 0)
                    listOfOsTranslations = tempList.ToList();

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
						ActiveSyncDevice myDevice = new ActiveSyncDevice();
						myDevice.User = ps.Properties["UserDisplayName"].Value == null ? "" : ps.Properties["UserDisplayName"].Value.ToString();
						string Username = (myDevice.User).ToLower();
						try
						{
							myDevice.DeviceID = ps.Properties["DeviceID"].Value == null ? "" : ps.Properties["DeviceID"].Value.ToString();
							myDevice.DeviceMobileOperator = ps.Properties["DeviceMobileOperator"].Value == null ? "" : ps.Properties["DeviceMobileOperator"].Value.ToString();
							myDevice.DeviceActiveSyncVersion = ps.Properties["DeviceActiveSyncVersion"].Value == null ? "" : ps.Properties["DeviceActiveSyncVersion"].Value.ToString();
							myDevice.DeviceFriendlyName = ps.Properties["DeviceFriendlyName"].Value == null ? "" : ps.Properties["DeviceFriendlyName"].Value.ToString();
							myDevice.DeviceOSLanguage = ps.Properties["DeviceOSLanguage"].Value == null ? "" : ps.Properties["DeviceOSLanguage"].Value.ToString();
							myDevice.DeviceModel = ps.Properties["DeviceModel"].Value == null ? "" : ps.Properties["DeviceModel"].Value.ToString();
							myDevice.DeviceType = ps.Properties["DeviceType"].Value == null ? "" : ps.Properties["DeviceType"].Value.ToString();
							myDevice.DeviceOS = ps.Properties["DeviceOS"].Value == null ? "" : ps.Properties["DeviceOS"].Value.ToString();
							myDevice.DeviceUserAgent = ps.Properties["DeviceUserAgent"].Value == null ? "" : ps.Properties["DeviceUserAgent"].Value.ToString();
                            myDevice.LastSuccessSync = ps.Properties["LastSuccessSync"].Value == null ? "" : ps.Properties["LastSuccessSync"].Value.ToString();

                            string[] aDeviceType = null;
							string DeviceType = "";
							string OsType = "";
							string[] DeviceName = null;
							string OSName = "";
							string TranslatedValue = "";
							if (!string.IsNullOrEmpty(myDevice.DeviceOS))
							{
								if ((myDevice.DeviceOS).ToLower().Contains("ios"))
								{
									if (myDevice.DeviceUserAgent.Contains("/"))
									{
										DeviceName = (myDevice.DeviceUserAgent).Split('-');
										OSName = DeviceName[0];
										aDeviceType = (myDevice.DeviceUserAgent).Split('/');

										DeviceType = aDeviceType[0];
									}
									if (aDeviceType.Count() > 0)
									{
										OsType = aDeviceType[1];

									}

								}
								else if ((myDevice.DeviceOS).ToLower().Contains("android"))
								{
									if (!string.IsNullOrEmpty(myDevice.DeviceOS))
									{
										string doubleVal = Convert.ToString(myDevice.DeviceOS);
										TranslatedValue = Convert.ToString(string.Join(".", doubleVal.Split('.').Take(2)));
									}
								}
								else if ((myDevice.DeviceOS).ToLower().Contains("windows"))
								{
									if (!string.IsNullOrEmpty(myDevice.DeviceOS))
									{
										string doubleVal = Convert.ToString(myDevice.DeviceOS);
										TranslatedValue = Convert.ToString(string.Join(".", doubleVal.Split('.').Take(2)));
									}
								}
								TranslatedValue = string.IsNullOrEmpty(TranslatedValue) ? myDevice.DeviceOS : TranslatedValue;
								if ((myDevice.DeviceOS).ToLower().Contains("ios"))
								{

                                    tempList = listOfOsTranslations.Where(x => x.OriginalValue == OSName && x.OSType == OsType).ToList();
                                    TranslatedValue = tempList.Count > 0 ? tempList[0].TranslatedValue : myDevice.DeviceOS;
								}
							}

							//soma
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
                           

                            MongoStatementsUpsert<VSNext.Mongo.Entities.MobileDevices> mongoStatement = new MongoStatementsUpsert<VSNext.Mongo.Entities.MobileDevices>();
                            mongoStatement.filterDef = mongoStatement.repo.Filter.Where(i => i.DeviceID == myDevice.DeviceID && i.ServerName == myServer.Name);
                            mongoStatement.updateDef = mongoStatement.repo.Updater
                                .Set(i => i.UserName, myDevice.User)
                                .Set(i => i.ServerName, myServer.Name)
                                .Set(i => i.DeviceID, myDevice.DeviceID)
                                .Set(i => i.SecurityPolicy, myDevice.DevicePolicyApplied)
                                .Set(i => i.DeviceName, myDevice.DeviceFriendlyName)
                                .Set(i => i.ConnectionState, myDevice.Status)
                                .Set(i => i.LastSyncTime, myDevice.LastSuccessSync == "" ? null : (DateTime?)DateTime.Parse(myDevice.LastSuccessSync))
                                .Set(i => i.OSType, myDevice.DeviceOS)
                                .Set(i => i.OSTypeMin, TranslatedValue)
                                .Set(i => i.ClientBuild, myDevice.DeviceActiveSyncVersion)
                                .Set(i => i.DeviceType, myDevice.DeviceType)
                                .Set(i => i.Access, myDevice.DeviceAccessState)
                                .Set(i => i.IsActive, true)
                                .Set(i => i.SyncType, "ActiveSync");

                            AllTestsList.MongoEntity.Add(mongoStatement);

						}
						catch (Exception ex)
						{
							Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "getMobileUsers Results: Error while processing data for User: " + Username + " : Exception: " + ex.Message.ToString(), Common.LogLevel.Normal);
                            Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "getMobileUsers Results: Error while processing data for User: " + Username + " : Exception: " + ex.StackTrace, Common.LogLevel.Normal);
                            Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "getMobileUsers Results: Error while processing data for User: " + Username + " : Exception: " + ex, Common.LogLevel.Normal);
                        }

					}

				}

			}
			catch (Exception ex)
			{
				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "getMobileUsers Results: Exception: " + ex.Message.ToString(), Common.LogLevel.Normal);
				//myServer.ADQueryTest = "Fail";
				//Common.makeAlert(false, myServer, commonEnums.AlertType., ref AllTestsList, myServer.ServerType);
			}
		}

		private string TranslateAppleOSType(string RawValue)
		{
			if (RawValue.Length > 0)
			{
				RawValue = Convert.ToString(string.Join(".", RawValue.Split('.').Take(2)));
				//RawValue = RawValue.Substring(0, RawValue.LastIndexOf(' '));
				//RawValue = (RawValue, RawValue.IndexOf("(") - 1);
			}

			return RawValue;
		}
		public void getMobileUsersHourly(MonitoredItems.Office365Server myServer, ref TestResults AllTestsList, ReturnPowerShellObjects powershellobj)
		{
            DataTable dt;
            string keyDevices = "";
            Dictionary<string, string> dict = new Dictionary<string, string>();
            CommonDB db = new CommonDB();
			try
			{
                VSNext.Mongo.Repository.Repository<VSNext.Mongo.Entities.MobileDevices> repositoryMobileDevices = new VSNext.Mongo.Repository.Repository<VSNext.Mongo.Entities.MobileDevices>(db.GetMongoConnectionString());
                List<VSNext.Mongo.Entities.MobileDevices> tempList = repositoryMobileDevices.Find(x => true).ToList();
                foreach(VSNext.Mongo.Entities.MobileDevices entity in tempList)
                {
                    if (keyDevices == "")
                        keyDevices = "'" + entity.DeviceID + "'";
                    else
                        keyDevices += ",'" + entity.DeviceID + "'";
                    dict.Add(entity.DeviceID, entity.ThresholdSyncTime.ToString());
                }
                
            }
            catch
            {
            }
            if (keyDevices != "")
            {
                try
                {

                    VSNext.Mongo.Repository.Repository<VSNext.Mongo.Entities.MobileDeviceTranslations> repoTranslations = new VSNext.Mongo.Repository.Repository<VSNext.Mongo.Entities.MobileDeviceTranslations>(db.GetMongoConnectionString());
                    FilterDefinition<VSNext.Mongo.Entities.MobileDeviceTranslations> filterDefTranslations = repoTranslations.Filter.Eq(x => x.Type, "OS");
                    List<VSNext.Mongo.Entities.MobileDeviceTranslations> tempList = repoTranslations.Find(filterDefTranslations).ToList();
                    if (tempList.Count > 0)
                        listOfOsTranslations = tempList.ToList();

                    Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "getMobileUsersHourly: Starting.", Common.LogLevel.Normal);
				System.Collections.ObjectModel.Collection<PSObject> results = new System.Collections.ObjectModel.Collection<PSObject>();
				//String str = " Get-Mailbox -ResultSize Unlimited | Select Name,Alais,DisplayName,StorageLimitStatus,membertype,servername,ProhibitSendQuota,LastLogonTime";
				//string str = "Get-MobileDevice";
                    //string str = "$MB = Get-MobileDevice -Resultsize unlimited " + "\n";
                    string str = "";
                    if (keyDevices != "")
                        str = "$MB = Get-MobileDevice |Where-Object DeviceId -In ( " + keyDevices + ")" + "\n";
				str += "$MB | foreach {Get-MobileDeviceStatistics $_.identity} | Select-Object UserDisplayName,FirstSyncTime,LastPolicyUpdateTime,LastSyncAttemptTime,LastSuccessSync,DeviceType,DeviceID,DeviceModel,DeviceFriendlyName,DeviceOS,DeviceOSLanguage,Identity,DeviceAccessState,NumberOfFoldersSynced ,DevicePolicyApplied,Status,DeviceUserAgent,DeviceActiveSyncVersion,DeviceMobileOperator" + "\n";
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
						string Username = (myDevice.User).ToLower();
						try
						{
							myDevice.User = myDevice.User.Split('/')[3];
							Username = (myDevice.User).ToLower();
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
						string[] aDeviceType = null;
						string DeviceType = "";
						string OsType = "";
						string[] DeviceName = null;
						string OSName = "";
						string TranslatedValue = "";
						if (!string.IsNullOrEmpty(myDevice.DeviceOS))
						{
							if ((myDevice.DeviceOS).ToLower().Contains("ios"))
							{
                                    if (myDevice.DeviceUserAgent != null)
                                    {
								if (myDevice.DeviceUserAgent.Contains("/"))
								{
									DeviceName = (myDevice.DeviceUserAgent).Split('-');
									OSName = DeviceName[0];
									aDeviceType = (myDevice.DeviceUserAgent).Split('/');

									DeviceType = aDeviceType[0];
								}
								if (aDeviceType.Count() > 0)
								{
									OsType = aDeviceType[1];

								}
                                    }

							}
							else if ((myDevice.DeviceOS).ToLower().Contains("android"))
							{
								if (!string.IsNullOrEmpty(myDevice.DeviceOS))
								{
									string doubleVal = Convert.ToString(myDevice.DeviceOS);
									TranslatedValue = Convert.ToString(string.Join(".", doubleVal.Split('.').Take(2)));
								}
							}
							else if ((myDevice.DeviceOS).ToLower().Contains("windows"))
							{
								if (!string.IsNullOrEmpty(myDevice.DeviceOS))
								{
									string doubleVal = Convert.ToString(myDevice.DeviceOS);
									TranslatedValue = Convert.ToString(string.Join(".", doubleVal.Split('.').Take(2)));
								}
							}
							TranslatedValue = string.IsNullOrEmpty(TranslatedValue) ? myDevice.DeviceOS : TranslatedValue;
							if ((myDevice.DeviceOS).ToLower().Contains("ios"))
							{
                                    tempList = listOfOsTranslations.Where(x => x.OriginalValue == OSName && x.OSType == OsType).ToList();
                                    TranslatedValue = tempList.Count > 0 ? tempList[0].TranslatedValue : myDevice.DeviceOS;

                            }
						}
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
									"[UserName]='" + Username + "',  [Security_Policy]='" + myDevice.DevicePolicyApplied + "', [DeviceName]='" + myDevice.DeviceModel + "', [ConnectionState]='" + myDevice.Status + "', " +
									"[LastSyncTime]='" + myDevice.LastSuccessSync + "', [OS_Type]= '" + myDevice.DeviceOS + "',[OS_Type_Min]='" + TranslatedValue + "', [Client_Build]='" + myDevice.DeviceActiveSyncVersion + "', [device_type]='" + myDevice.DeviceType + "', " +
									"[ServerName]='" + myServer.Name + "', [Access]='" + myDevice.DeviceAccessState + "', [DeviceID]='" + myDevice.DeviceID + "', [LastUpdated]='" + DateTime.Now + "' " +
									"where DeviceID='" + myDevice.DeviceID + "' " +

									"end  else " +

									"begin " +
									"INSERT INTO vitalsigns.dbo.Traveler_Devices (UserName, Security_Policy, DeviceName, ConnectionState, LastSyncTime, OS_Type,OS_Type_Min, Client_Build, device_type, ServerName, Access, DeviceID, LastUpdated, IsActive) " +
									" VALUES ('" + Username + "', '" + myDevice.DevicePolicyApplied + "', '" + myDevice.DeviceModel + "', '" + myDevice.Status + "','" + myDevice.LastSuccessSync + "', '" + myDevice.DeviceOS + "','" + TranslatedValue + "', '" +
									myDevice.DeviceActiveSyncVersion + "', '" + myDevice.DeviceType + "', '" + myServer.Name + "', '" + myDevice.DeviceAccessState + "', '" + myDevice.DeviceID + "', '" + DateTime.Now + "', 1) end";

						//AllTestsList.SQLStatements.Add(new SQLstatements() { SQL = strSQL + strSQLValues });
                        MongoStatementsUpsert<VSNext.Mongo.Entities.MobileDevices> mongoStatement = new MongoStatementsUpsert<VSNext.Mongo.Entities.MobileDevices>();
                        mongoStatement.filterDef = mongoStatement.repo.Filter.Where(i => i.DeviceID == myDevice.DeviceID && i.ServerName == myServer.Name);
                        mongoStatement.updateDef = mongoStatement.repo.Updater
                            .Set(i => i.UserName, Username)
                            .Set(i => i.SecurityPolicy, myDevice.DevicePolicyApplied)
                            .Set(i => i.DeviceName, myDevice.DeviceModel)
                            .Set(i => i.ConnectionState, myDevice.Status)
                            .Set(i => i.LastSyncTime, myDevice.LastSuccessSync == "" ? null : (DateTime?)DateTime.Parse(myDevice.LastSuccessSync))
                            .Set(i => i.OSType, myDevice.DeviceOS)
                            .Set(i => i.OSTypeMin, TranslatedValue)
                            .Set(i => i.ClientBuild, myDevice.DeviceActiveSyncVersion)
                            .Set(i => i.DeviceType, myDevice.DeviceType)
                            .Set(i => i.Access, myDevice.DeviceAccessState)
                            .Set(i => i.IsActive, true)
                            .Set(i => i.SyncType, "ActiveSync");

                        AllTestsList.MongoEntity.Add(mongoStatement);
                            string val = dict[myDevice.DeviceID].ToString();
                            DateTime dtNowUtc = DateTime.UtcNow;
                            DateTime dtLastSyncUtc = new DateTime();
                            if (myDevice.LastSuccessSync != "")
                                 dtLastSyncUtc = Convert.ToDateTime(myDevice.LastSuccessSync);
                            TimeSpan elapsed = dtNow.Subtract(dtLastSyncUtc);
//                            string alertDesc= "Active Sync device for user: " + Username  + " and device is overdue. The threshold was set at " + val + " but the device was last synced " +  elapsed.Minutes.ToString() + " ago.";
                            string alertDesc = myDevice.DeviceID + " for " + Username + " last synced at: " + dtLastSyncUtc.ToString();
                            MonitoredItems.MicrosoftServer m = new MonitoredItems.MicrosoftServer();
                            m.Name = Username;
                            m.ServerTypeId = 11;
                            m.ServerType = "Mobile Users";

                            if (elapsed.TotalMinutes  > Convert.ToInt32(val))
                                Common.makeAlert(true, m, commonEnums.AlertType.Active_Sync_Devices, ref AllTestsList, alertDesc,"ActiveSync");
                            else
                                Common.makeAlert(false, m, commonEnums.AlertType.Active_Sync_Devices, ref AllTestsList, alertDesc, "ActiveSync");
                            


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
		}
		#endregion
		#region SendAndReceiveMailStats
		public void getMailStatusInfo(MonitoredItems.Office365Server myServer, ref TestResults AllTestsList, ReturnPowerShellObjects powershellobj)
		{
			try
			{
				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "getMailboxes: Starting.", Common.LogLevel.Normal);
				System.Collections.ObjectModel.Collection<PSObject> results = new System.Collections.ObjectModel.Collection<PSObject>();
				//String str = " Get-Mailbox -ResultSize Unlimited | Select Name,Alais,DisplayName,StorageLimitStatus,membertype,servername,ProhibitSendQuota,LastLogonTime";
				//string str = "Get-MessageTrace -StartDate ([DateTime]::Today.AddDays(-1)) -EndDate ([DateTime]::Today) | Select Received,SenderAddress,RecipientAddress,Size";
				String str = AppDomain.CurrentDomain.BaseDirectory.ToString() + "Scripts\\withparms.ps1";
				//Get - Mailbox | select *
				//Get-Mailbox | select RecipientTypeDetails,ProhibitSendQuota,ProhibitSendReceiveQuota,IssueWarningQuota,IsInactiveMailbox
				powershellobj.PS.Streams.Error.Clear();

				System.IO.StreamReader sr = new System.IO.StreamReader(str);
				String s = sr.ReadToEnd();
				powershellobj.PS.Commands.Clear();
				//powershellobj.PS.Streams.ClearStreams();
				powershellobj.PS.AddScript(s);
				results = powershellobj.PS.Invoke();
				//AllTestsList.SQLStatements.Add(new SQLstatements() { SQL = "DELETE FROM ExchangeMailFiles WHERE Server='" + myServer.Name + "'", DatabaseName = "VSS_Statistics" });
				//AllTestsList.SQLStatements.Add(new SQLstatements() { SQL = "DELETE FROM O365AdditionalMailDetails WHERE Server='" + myServer.Name + "'", DatabaseName = "VSS_Statistics" });
				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "getMailboxes Results: " + results.Count.ToString(), Common.LogLevel.Normal);
				DateTime dtNow = DateTime.Now;
				int weekNumber = culture.Calendar.GetWeekOfYear(dtNow, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday);


				if (results.Count > 0)
				{
					List<MailCount> mailCounts = new List<MailCount>();

					foreach (PSObject ps in results)
					{
						MailCount mailCount = new MailCount();
						//mailCount.RecipientName = ps.Properties["RecipientName "].Value.ToString() == null ? "" : ps.Properties["RecipientName "].Value.ToString();
						mailCount.RecipientName = ps.Properties["RecipientName"].Value == null ? "" : ps.Properties["RecipientName"].Value.ToString();
						mailCount.Inbound = Convert.ToInt32(ps.Properties["Inbound"].Value == null ? "" : ps.Properties["Inbound"].Value);
						mailCount.Outbound = Convert.ToInt32(ps.Properties["Outbound"].Value == null ? "" : ps.Properties["Outbound"].Value);
						mailCount.InboundSize = Convert.ToInt32(ps.Properties["InboundSize"].Value == null ? "0" : ps.Properties["InboundSize"].Value);
						mailCount.OutboundSize = Convert.ToInt32(ps.Properties["OutboundSize"].Value == null ? "0" : ps.Properties["OutboundSize"].Value);
						mailCounts.Add(mailCount);

						string sqlQuery = Common.GetInsertIntoDailyStats(myServer.Name, myServer.ServerTypeId.ToString(), "Mailbox." + mailCount.RecipientName + ".Sent.Count", mailCount.Outbound.ToString());
						AllTestsList.SQLStatements.Add(new SQLstatements() { SQL = sqlQuery, DatabaseName = "VSS_Statistics" });

						sqlQuery = Common.GetInsertIntoDailyStats(myServer.Name, myServer.ServerTypeId.ToString(), "Mailbox." + mailCount.RecipientName + ".Received.Count", mailCount.Inbound.ToString());
						AllTestsList.SQLStatements.Add(new SQLstatements() { SQL = sqlQuery, DatabaseName = "VSS_Statistics" });

						sqlQuery = Common.GetInsertIntoDailyStats(myServer.Name, myServer.ServerTypeId.ToString(), "Mailbox." + mailCount.RecipientName + ".Sent.SizeMB", mailCount.OutboundSize.ToString());
						AllTestsList.SQLStatements.Add(new SQLstatements() { SQL = sqlQuery, DatabaseName = "VSS_Statistics" });

						sqlQuery = Common.GetInsertIntoDailyStats(myServer.Name, myServer.ServerTypeId.ToString(), "Mailbox." + mailCount.RecipientName + ".Received.SizeMB", mailCount.InboundSize.ToString());
						AllTestsList.SQLStatements.Add(new SQLstatements() { SQL = sqlQuery, DatabaseName = "VSS_Statistics" });
					}
					int intSACount = 0;
					int intRACount = 0;
					int intTotalRASize = 0;
					int intTotalSASize = 0;
					int TotalSendReceiveMailcount = 0;
					int TotalSendReceiveMailSizeCount = 0;
					if (mailCounts.Count() > 0)
					{
						//intRACount=intSACount = mailCounts.Count();
						intRACount = mailCounts.Sum(x => x.Inbound);
						intSACount = mailCounts.Sum(x => x.Outbound);
						intTotalRASize = mailCounts.Sum(x => x.InboundSize);
						intTotalSASize = mailCounts.Sum(x => x.OutboundSize);
						TotalSendReceiveMailcount = intRACount + intSACount;
						TotalSendReceiveMailSizeCount = intTotalRASize + TotalSendReceiveMailSizeCount;
						string sqlQuery = Common.GetInsertIntoDailyStats(myServer.Name, myServer.ServerTypeId.ToString(), "Mail_SentCount", intSACount.ToString());
						AllTestsList.SQLStatements.Add(new SQLstatements() { SQL = sqlQuery, DatabaseName = "VSS_Statistics" });

						sqlQuery = Common.GetInsertIntoDailyStats(myServer.Name, myServer.ServerTypeId.ToString(), "Mail_ReceivedCount", intRACount.ToString());
						AllTestsList.SQLStatements.Add(new SQLstatements() { SQL = sqlQuery, DatabaseName = "VSS_Statistics" });


						sqlQuery = Common.GetInsertIntoDailyStats(myServer.Name, myServer.ServerTypeId.ToString(), "Mail_SentSizeMB", intTotalRASize.ToString());
						AllTestsList.SQLStatements.Add(new SQLstatements() { SQL = sqlQuery, DatabaseName = "VSS_Statistics" });

						sqlQuery = Common.GetInsertIntoDailyStats(myServer.Name, myServer.ServerTypeId.ToString(), "Mail_ReceivedSizeMB", intTotalSASize.ToString());
						AllTestsList.SQLStatements.Add(new SQLstatements() { SQL = sqlQuery, DatabaseName = "VSS_Statistics" });

						//string sqlQuery2 = "Insert into VSS_Statistics.dbo.MicrosoftDailyStats(ServerName,ServerTypeId,Date,StatName,StatValue,WeekNumber,MonthNumber,YearNumber,DayNumber, HourNumber, Details) "
						//        + " values('" + myServer.Name + "','" + myServer.ServerTypeId + "',GetDate(),'Office365SendandReceiveMailscount' ," + TotalSendReceiveMailcount +
						//       "," + weekNumber + ", " + dtNow.Month.ToString() + ", " + dtNow.Year.ToString() + ", " + dtNow.Day.ToString() + ", " + dtNow.Hour.ToString() + ", ''),('" + myServer.Name + "','" + myServer.ServerTypeId + "',GetDate(),'Office365SentAndRecieveMailSize' ," + TotalSendReceiveMailSizeCount +
						//       "," + weekNumber + ", " + dtNow.Month.ToString() + ", " + dtNow.Year.ToString() + ", " + dtNow.Day.ToString() + ", " + dtNow.Hour.ToString() + ", '')";
						//AllTestsList.SQLStatements.Add(new SQLstatements() { SQL = sqlQuery2, DatabaseName = "VSS_Statistics" });
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
				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "getMailStatus: Exception: " + ex.Message.ToString(), Common.LogLevel.Verbose);
				//myServer.ADQueryTest = "Fail";
				//Common.makeAlert(false, myServer, commonEnums.AlertType.Mailbox_Database_Size, ref AllTestsList, myServer.ServerType);
			}
		}
		#endregion
		#region ServiceDetailStatus
		public void getServiceStatus(MonitoredItems.Office365Server myServer, ref TestResults AllTestsList, ReturnPowerShellObjects powershellobj)
		{
			try
			{

                List<VSNext.Mongo.Entities.Office365> list = new List<VSNext.Mongo.Entities.Office365>();
				
				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "getMailboxes: Starting.", Common.LogLevel.Normal);
				System.Collections.ObjectModel.Collection<PSObject> results = new System.Collections.ObjectModel.Collection<PSObject>();
				System.Security.SecureString securePassword = Common.String2SecureString(myServer.Password);
				PSCredential creds = new PSCredential(myServer.UserName, securePassword);
				//String str = AppDomain.CurrentDomain.BaseDirectory.ToString() + "Scripts\\servicestatus.ps1" + "  " + myServer.UserName + "  " + myServer.Password;
                System.IO.StreamReader sr = new System.IO.StreamReader(AppDomain.CurrentDomain.BaseDirectory.ToString() + "Scripts\\servicestatus.ps1");
                string startOfScript = "$uname= '" + myServer.UserName + "'; \n $pwd= '" + myServer.Password + "';\n";
                String str = startOfScript + sr.ReadToEnd();

				powershellobj.PS.Streams.Error.Clear();
				powershellobj.PS.Commands.Clear();
				powershellobj.PS.Streams.ClearStreams();
				powershellobj.PS.AddScript(str);
				results = powershellobj.PS.Invoke();
				string strSQL = "";
				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "O365ServiceDetails Results: " + results.Count.ToString(), Common.LogLevel.Normal);
				DateTime dtNow = DateTime.Now;
				int weekNumber = culture.Calendar.GetWeekOfYear(dtNow, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday);
				if (results.Count > 0)
				{
					List<O365ServiceDetails> O365ServiceDetails = new List<O365ServiceDetails>();
					string sqlQuery = "delete from dbo.O365ServiceDetails where ServerId=" + myServer.ServerId.ToString();
					AllTestsList.SQLStatements.Add(new SQLstatements() { SQL = sqlQuery, DatabaseName = "Vitalsigns" });
					foreach (PSObject ps in results)
					{
                        VSNext.Mongo.Entities.Office365ServiceDetails O365ServiceDetail = new VSNext.Mongo.Entities.Office365ServiceDetails();
                        try
                        {
                       
						O365ServiceDetail.ServerId = Convert.ToInt32(myServer.ServerId == null ? "" : myServer.ServerId);
						O365ServiceDetail.ServiceName = (ps.Properties["ServiceName"].Value == null ? "" : ps.Properties["ServiceName"].Value).ToString();
						O365ServiceDetail.ServiceID = (ps.Properties["Id"].Value == null ? "" : ps.Properties["Id"].Value).ToString();
						O365ServiceDetail.StartTime = Convert.ToDateTime(ps.Properties["StartTime"].Value == null ? "0" : ps.Properties["StartTime"].Value);
						O365ServiceDetail.EndTime = Convert.ToDateTime(ps.Properties["EndTime"].Value == null ? "0" : ps.Properties["EndTime"].Value);
						O365ServiceDetail.Status = (ps.Properties["Status"].Value == null ? "" : ps.Properties["Status"].Value).ToString();
						O365ServiceDetail.EventType = (ps.Properties["EventType"].Value == null ? "" : ps.Properties["EventType"].Value).ToString();
						O365ServiceDetail.Message = (ps.Properties["Message"].Value == null ? "" : ps.Properties["Message"].Value).ToString();
						strSQL = "INSERT INTO dbo.[O365ServiceDetails] (ServerId, ServiceName, ServiceID, StartTime, EndTime, Status, EventType, Message) " +
								" VALUES ('" + O365ServiceDetail.ServerId + "', '" + O365ServiceDetail.ServiceName + "', '" + O365ServiceDetail.ServiceID + "', '" + O365ServiceDetail.StartTime + "', '" +
								O365ServiceDetail.EndTime + "', '" + O365ServiceDetail.Status + "', '" + O365ServiceDetail.EventType + "', '" + O365ServiceDetail.Message + "')";
						//AllTestsList.SQLStatements.Add(new SQLstatements() { SQL = strSQL + strSQLValues });
						//AllTestsList.SQLStatements.Add(new SQLstatements() { SQL = strSQL, DatabaseName = "Vitalsigns" });
                        }
                        catch
                        {

                        MongoStatementsUpsert<VSNext.Mongo.Entities.Office365ServiceDetails> updateStatement = new MongoStatementsUpsert<VSNext.Mongo.Entities.Office365ServiceDetails>();
                        updateStatement.filterDef = updateStatement.repo.Filter.Where(i => i.ServerId == O365ServiceDetail.ServerId);
                        updateStatement.updateDef = updateStatement.repo.Updater.Set(i => i.ServiceName, O365ServiceDetail.ServiceName).Set(i => i.ServiceID, O365ServiceDetail.ServiceID)
                            .Set(i => i.StartTime, O365ServiceDetail.StartTime)
                            .Set(i => i.EndTime, O365ServiceDetail.EndTime)
                            .Set(i => i.Status, O365ServiceDetail.Status)
                        .Set(i => i.EventType, O365ServiceDetail.EventType)
                        .Set(i => i.Message, O365ServiceDetail.Message);

                        AllTestsList.MongoEntity.Add(updateStatement);
					}
					}

				}
                results = null;
                GC.Collect();
			}
			catch (Exception ex)
			{
				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "getServiceStatusDetails: Exception: " + ex.Message.ToString(), Common.LogLevel.Verbose);
			}
		}
		#endregion
		#region Users with Licences and Services
		public void getUserswithLicencesandServices(MonitoredItems.Office365Server myServer, ref TestResults AllTestsList, ReturnPowerShellObjects powershellobj)
		{
			try
			{
				 MongoStatementsInsert<VSNext.Mongo.Entities.Office365UsersLicensesServices> msi = new MongoStatementsInsert<VSNext.Mongo.Entities.Office365UsersLicensesServices>();
				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "Users with Licences and Services Hourly: Starting.", Common.LogLevel.Normal);
				System.Collections.ObjectModel.Collection<PSObject> results = new System.Collections.ObjectModel.Collection<PSObject>();
				System.Security.SecureString securePassword = Common.String2SecureString(myServer.Password);
				PSCredential creds = new PSCredential(myServer.UserName, securePassword);
				//String str = AppDomain.CurrentDomain.BaseDirectory.ToString() + "Scripts\\O365UserswithLicensesandServices.ps1" + "  " + myServer.UserName + "  " + myServer.Password;
                
                System.IO.StreamReader sr = new System.IO.StreamReader(AppDomain.CurrentDomain.BaseDirectory.ToString() + "Scripts\\O365UserswithLicensesandServices.ps1");
                
				powershellobj.PS.Streams.Error.Clear();
				powershellobj.PS.Commands.Clear();
				powershellobj.PS.Streams.ClearStreams();
				powershellobj.PS.AddScript(sr.ReadToEnd());
				//////string str = "Get-MsolUser |Where {$_.IsLicensed -eq $true }| Select-Object DisplayName,Licenses, @{Name='MDM';Expression={$_.Licenses[0].ServiceStatus[0].ProvisioningStatus}}, @{Name='Yammer';Expression={$_.Licenses[0].ServiceStatus[1].ProvisioningStatus}}, @{Name='AD RMS';Expression={$_.Licenses[0].ServiceStatus[2].ProvisioningStatus}}, @{Name='OfficePro';Expression={$_.Licenses[0].ServiceStatus[3].ProvisioningStatus}}, @{Name='Skype';Expression={$_.Licenses[0].ServiceStatus[4].ProvisioningStatus}}, @{Name='OfficeWeb';Expression={$_.Licenses[0].ServiceStatus[5].ProvisioningStatus}}, @{Name='SharePoint';Expression={$_.Licenses[0].ServiceStatus[6].ProvisioningStatus}}, @{Name='Exchange';Expression={$_.Licenses[0].ServiceStatus[7].ProvisioningStatus}}";
				results = powershellobj.PS.Invoke();
				
				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "Users with Licences and Services Results: " + results.Count.ToString(), Common.LogLevel.Normal);
				DateTime dtNow = DateTime.Now;
				int weekNumber = culture.Calendar.GetWeekOfYear(dtNow, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday);
				string strSQL = "";
				string strSQLValues = "";
                Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "Users with Licences and Services Results: errors: " + powershellobj.PS.Streams.Error.Count.ToString(), Common.LogLevel.Normal);
                
				if (results.Count > 0)
				{
					List<O365UserswithLicensesandServices> userLicenseList = new List<O365UserswithLicensesandServices>();
					string sqlQuery = "delete from dbo.O365UserswithLicensesandServices where ServerId=" + myServer.ServerId.ToString();
					AllTestsList.SQLStatements.Add(new SQLstatements() { SQL = sqlQuery, DatabaseName = "Vitalsigns" });
					foreach (PSObject result in results)
					{
						O365UserswithLicensesandServices userLicense = new O365UserswithLicensesandServices();
						var formatedResult = (from x in result.Members
											  select new { name = x.Name, value = x.Value }).ToList();
						formatedResult.RemoveRange(formatedResult.Count - 4, 4);
						userLicense.DisplayName = formatedResult.Where(x => x.name.Equals("DisplayName")).FirstOrDefault().value.ToString();
						var xmlResult = new XElement("Root",
					from x in formatedResult
					where x.value != null
					select new XElement("Row", new XAttribute("Key", x.name), new XAttribute("Value", x.value)));
						userLicense.XMLConfiguration = xmlResult.ToString();
						userLicenseList.Add(userLicense);
                        //strSQL = "INSERT INTO vitalsigns.dbo.O365UserswithLicensesandServices (DisplayName, XMLConfiguration, ServerId) VALUES ('" + userLicense.DisplayName + "', '" + userLicense.XMLConfiguration + "', '" + myServer.ServerId + "')";
                        //AllTestsList.SQLStatements.Add(new SQLstatements() { SQL = strSQL, DatabaseName = "Vitalsigns" });
                         VSNext.Mongo.Entities.Office365UsersLicensesServices o365Server = new VSNext.Mongo.Entities.Office365UsersLicensesServices();
                        o365Server.ServerId = Convert.ToInt32(myServer.ServerId);
                        o365Server.DisplayName = userLicense.DisplayName;
                        o365Server.XMLConfiguration = userLicense.XMLConfiguration;
                       
                        msi.listOfEntities.Add(o365Server);
					}
                    AllTestsList.MongoEntity.Add(msi);
				}
			}
			catch (Exception ex)
			{
				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "Users with Licences and Services Results: Exception: " + ex.Message.ToString(), Common.LogLevel.Verbose);
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
					Common.makeAlert(false, myServer, commonEnums.AlertType.Auto_Discovery, ref AutoDiscoveryIssueList, "Unable to connect to the CAS auto discovery service at " + DateTime.Now.ToString("HH:mm:ss tt") + " because " + strResponse, "Overall");
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
							Common.makeAlert(true, myServer, commonEnums.AlertType.Auto_Discovery, ref AutoDiscoveryIssueList, "Auto Discovery service responded", "Overall");
						}
						else
						{
							Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "Auto Discovery Status:" + "FAIL");
							//AutoDiscoveryIssueList.StatusDetails.Add(new TestList() { Details = "Auto discover service did not respond.", TestName = "Discovery Service", Category = commonEnums.ServerRoles.CAS, Result = commonEnums.ServerResult.Fail });
							Common.makeAlert(false, myServer, commonEnums.AlertType.Auto_Discovery, ref AutoDiscoveryIssueList, "Auto discover service responded", "Overall");
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
					Common.makeAlert(false, myServer, commonEnums.AlertType.SMTP, ref SMTPIssueList, "Unable to connect to the SMTP server at " + DateTime.Now.ToString("HH:mm:ss tt") + " because " + strResponse.ToString().Substring(0, 150), "Overall");
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

                        // 22/7/2016 Durga Modified for VSPLUS-3114
                        int index = strResponse.IndexOf("at");
                        int lastindex = strResponse.LastIndexOf(" +0000");
                        string time = strResponse.Substring(index + 2, strResponse.Length - index - 2);
                        DateTime Converttedtime = Convert.ToDateTime(time);
                        strResponse = strResponse.Substring(0, index + 2);
                        strResponse += " " + Converttedtime.ToString();

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
						Common.makeAlert(true, myServer, commonEnums.AlertType.SMTP, ref SMTPIssueList, "Service answered with  " + strResponse.Trim(), "Overall");
						DateTime dtNow = DateTime.Now;
						int weekNumber = culture.Calendar.GetWeekOfYear(dtNow, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday);
						string sqlQuery = "Insert into VSS_Statistics.dbo.MicrosoftDailyStats(ServerName,ServerTypeId,Date,StatName,StatValue,WeekNumber,MonthNumber,YearNumber,DayNumber, HourNumber, Details) "
								+ " values('" + myServer.Name + "','" + myServer.ServerTypeId + "',GetDate(),'SMTP" + "@" + nodeName + "'" + " ," + elapsed.TotalMilliseconds.ToString() +
							   "," + weekNumber + ", " + dtNow.Month.ToString() + ", " + dtNow.Year.ToString() + ", " + dtNow.Day.ToString() + ", " + dtNow.Hour.ToString() + ", '')";
						//SMTPIssueList.SQLStatements.Add(new SQLstatements() { SQL = sqlQuery, DatabaseName = "VSS_Statistics" });
                        SMTPIssueList.MongoEntity.Add(Common.GetInsertIntoDailyStats(myServer, "SMTP" + "@" + nodeName, elapsed.TotalMilliseconds.ToString()));
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
					Common.makeAlert(false, myServer, commonEnums.AlertType.POP3, ref POPIssueList, "Unable to connect to the POP3 server", "Overall");
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
						Common.makeAlert(true, myServer, commonEnums.AlertType.POP3, ref POPIssueList, "Successfully connected via POP3", "Overall");
						DateTime dtNow = DateTime.Now;
						int weekNumber = culture.Calendar.GetWeekOfYear(dtNow, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday);
						string sqlQuery = "Insert into VSS_Statistics.dbo.MicrosoftDailyStats(ServerName,ServerTypeId,Date,StatName,StatValue,WeekNumber,MonthNumber,YearNumber,DayNumber, HourNumber, Details) "
								+ " values('" + myServer.Name + "','" + myServer.ServerTypeId + "',GetDate(),'POP" + "@" + nodeName + "'" + " ," + elapsed.TotalMilliseconds.ToString() +
							   "," + weekNumber + ", " + dtNow.Month.ToString() + ", " + dtNow.Year.ToString() + ", " + dtNow.Day.ToString() + ", " + dtNow.Hour.ToString() + ", '')";
						//POPIssueList.SQLStatements.Add(new SQLstatements() { SQL = sqlQuery, DatabaseName = "VSS_Statistics" });
                        POPIssueList.MongoEntity.Add(Common.GetInsertIntoDailyStats(myServer, "POP" + "@" + nodeName, elapsed.TotalMilliseconds.ToString()));
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
					Common.makeAlert(false, myServer, commonEnums.AlertType.IMAP, ref IMAPIssueList, "Unable to connect to the IMAP server at " + DateTime.Now.ToString("HH:mm:ss tt") + " because " + strResponse, "Overall");
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
						Common.makeAlert(true, myServer, commonEnums.AlertType.IMAP, ref IMAPIssueList, "Successfully connected via IMAP", "Overall");

						CultureInfo culture = CultureInfo.CurrentCulture;
						//myServer.ResponseTime = elapsed.TotalMilliseconds;
						DateTime dtNow = DateTime.Now;
						int weekNumber = culture.Calendar.GetWeekOfYear(dtNow, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday);
						string sqlQuery = "Insert into VSS_Statistics.dbo.MicrosoftDailyStats(ServerName,ServerTypeId,Date,StatName,StatValue,WeekNumber,MonthNumber,YearNumber,DayNumber, HourNumber, Details) "
								+ " values('" + myServer.Name + "','" + myServer.ServerTypeId + "',GetDate(),'IMAP" + "@" + nodeName + "'" + " ," + elapsed.TotalMilliseconds.ToString() +
							   "," + weekNumber + ", " + dtNow.Month.ToString() + ", " + dtNow.Year.ToString() + ", " + dtNow.Day.ToString() + ", " + dtNow.Hour.ToString() + ", '')";
						//IMAPIssueList.SQLStatements.Add(new SQLstatements() { SQL = sqlQuery, DatabaseName = "VSS_Statistics" });
                        IMAPIssueList.MongoEntity.Add(Common.GetInsertIntoDailyStats(myServer, "IMAP" + "@" + nodeName, elapsed.TotalMilliseconds.ToString()));


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
							Common.makeAlert(true, myServer, commonEnums.AlertType.MAPI_Connectivity, ref AllTestsList, "MAPI Connectivity Succeeded at: " + DateTime.Now.ToString("HH:mm:ss tt"), "Overall");
						else
							Common.makeAlert(false, myServer, commonEnums.AlertType.MAPI_Connectivity, ref AllTestsList, "Unable to connect to the IMAP server at " + DateTime.Now.ToString("HH:mm:ss tt") + " because " + Err, "Overall");
					}

				}
				else
				{
					//myServer.ADQueryTest = "Fail";
					//Common.makeAlert(true, myServer, commonEnums.AlertType.AD_Query_Latency, ref AllTestsList, myServer.ServerType);
					Common.makeAlert(false, myServer, commonEnums.AlertType.MAPI_Connectivity, ref AllTestsList, "Unable to connect to the IMAP server at " + DateTime.Now.ToString("HH:mm:ss tt") + " because It did not fetch any result.", "Overall");
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
                    if ((myServer.CreateSiteThreshold != 0) && (myServer.CreateSiteThreshold > 0) && (myServer.CreateSiteThreshold < elapsed.TotalMilliseconds))
                        //Common.makeAlert(false, myServer, commonEnums.AlertType.Create_Site, ref AllTestsList, "Successfully created a test site, but it did not meet the threshold time of " + myServer.CreateSiteThreshold.ToString() + " ms", "Performance");
						Common.makeAlert(elapsed.TotalMilliseconds, myServer.CreateSiteThreshold, myServer, commonEnums.AlertType.Create_Site, ref AllTestsList, "Create Site: The Site was created in " + elapsed.TotalMilliseconds + " ms at " + time.ToString(format) + ", but it did not meet the threshold of " + myServer.CreateSiteThreshold + " ms", "Performance");
					else
                        Common.makeAlert(true, myServer, commonEnums.AlertType.Create_Site, ref AllTestsList, "Successfully created a test site", "Performance");

					string sqlQuery = "Insert into VSS_Statistics.dbo.MicrosoftDailyStats(ServerName,ServerTypeId,Date,StatName,StatValue,WeekNumber,MonthNumber,YearNumber,DayNumber, HourNumber, Details) "
							+ " values('" + myServer.Name + "','" + myServer.ServerTypeId + "',GetDate(),'CreateSite" + "@" + nodeName + "'" + " ," + elapsed.TotalMilliseconds.ToString() +
						   "," + weekNumber + ", " + dtNow.Month.ToString() + ", " + dtNow.Year.ToString() + ", " + dtNow.Day.ToString() + ", " + dtNow.Hour.ToString() + ", '')";
					//AllTestsList.SQLStatements.Add(new SQLstatements() { SQL = sqlQuery, DatabaseName = "VSS_Statistics" });
                    AllTestsList.MongoEntity.Add(Common.GetInsertIntoDailyStats(myServer, "CreateSite" + "@" + nodeName, elapsed.TotalMilliseconds.ToString()));
				}
				else
                    Common.makeAlert(false, myServer, commonEnums.AlertType.Create_Site, ref AllTestsList, "Failed to create a test site", "Performance");

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
					Common.makeAlert(false, myServer, commonEnums.AlertType.Delete_Site, ref AllTestsList, "Failed to delete a test site", "Performance");
				else
					Common.makeAlert(true, myServer, commonEnums.AlertType.Delete_Site, ref AllTestsList, "Successfully deleted a test site", "Performance");
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
                MongoStatementsInsert<VSNext.Mongo.Entities.Office365LyncStats> msi = new MongoStatementsInsert<VSNext.Mongo.Entities.Office365LyncStats>();

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
						//AllTestsList.SQLStatements.Add(new SQLstatements() { SQL = sqlQuery, DatabaseName = "vitalsigns" });
						//break;

                        VSNext.Mongo.Entities.Office365LyncStats o365Server = new VSNext.Mongo.Entities.Office365LyncStats();
                        o365Server.ServerId = Convert.ToInt32(myServer.ServerId);
                        o365Server.AccountName = AccountName;
                        o365Server.ActiveUsers = Convert.ToInt32(ActiveUsers);
                        o365Server.ActiveIMUsers = Convert.ToInt32(ActiveIMUsers);
                        o365Server.ActiveAudioUsers = Convert.ToInt32(ActiveAudioUsers);
                        o365Server.ActiveVideoUsers = Convert.ToInt32(ActiveVideousers);
                        o365Server.ActiveApplicationSharingUsers = Convert.ToInt32(ActiveApplicationSharingUsers);
                        o365Server.ActiveFileTransferUsers = Convert.ToInt32(ActiveFileTransferUsers);
                        msi.listOfEntities.Add(o365Server);
                        break;
					}
                    AllTestsList.MongoEntity.Add(msi);

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
                MongoStatementsInsert<VSNext.Mongo.Entities.Office365LyncDevices> msi = new MongoStatementsInsert<VSNext.Mongo.Entities.Office365LyncDevices>();
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
						//AllTestsList.SQLStatements.Add(new SQLstatements() { SQL = sqlQuery, DatabaseName = "vitalsigns" });

                        VSNext.Mongo.Entities.Office365LyncDevices o365Server = new VSNext.Mongo.Entities.Office365LyncDevices();
                        o365Server.ServerId = Convert.ToInt32(myServer.ServerId);
                        o365Server.AccountName = AccountName;
                        o365Server.WindowsUsers = Convert.ToInt32(WindowsUsers);
                        o365Server.WindowsPhoneUsers = Convert.ToInt32(WindowsPhoneUsers);
                        o365Server.AndroidUsers = Convert.ToInt32(AndroidUsers);
                        o365Server.IphoneUsers = Convert.ToInt32(iPhoneUsers);
                        o365Server.IpadUsers = Convert.ToInt32(iPadUsers);
                        msi.listOfEntities.Add(o365Server);
						break;
					}
                    AllTestsList.MongoEntity.Add(msi);

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
                MongoStatementsInsert<VSNext.Mongo.Entities.Office365LyncPAVTimeReport> msi = new MongoStatementsInsert<VSNext.Mongo.Entities.Office365LyncPAVTimeReport>();
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
						//AllTestsList.SQLStatements.Add(new SQLstatements() { SQL = sqlQuery, DatabaseName = "vitalsigns" });
                        VSNext.Mongo.Entities.Office365LyncPAVTimeReport o365Server = new VSNext.Mongo.Entities.Office365LyncPAVTimeReport();
                        o365Server.ServerId = Convert.ToInt32(myServer.ServerId);
                        o365Server.AccountName = AccountName;
                        o365Server.TotalAudioMinutes = Convert.ToInt32(TotalAudioMinutes);
                        o365Server.TotalVideoMinutes = Convert.ToInt32(TotalVideoMinutes);
                       
                        msi.listOfEntities.Add(o365Server);
						break;
					}
                    AllTestsList.MongoEntity.Add(msi);
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
                MongoStatementsInsert<VSNext.Mongo.Entities.Office365LyncConferenceReport> msi = new MongoStatementsInsert<VSNext.Mongo.Entities.Office365LyncConferenceReport>();
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
                        VSNext.Mongo.Entities.Office365LyncConferenceReport o365Server = new VSNext.Mongo.Entities.Office365LyncConferenceReport();
                        o365Server.AccountName = AccountName;
                        o365Server.ServerId = Convert.ToInt32(myServer.ServerId);
                        o365Server.TotalConferences = Convert.ToInt32(TotalConferences);
                        o365Server.AVConferences = Convert.ToInt32(AVConferences);
                        o365Server.IMConferences = Convert.ToInt32(IMConferences);
                        o365Server.ApplicationSharingConferences = Convert.ToInt32(ApplicationSharingConferences);
                        o365Server.WebConferences = Convert.ToInt32(WebConferences);
                        o365Server.TelephonyConferences = Convert.ToInt32(TelephonyConferences);
                        msi.listOfEntities.Add(o365Server);
						break;
					}
                    AllTestsList.MongoEntity.Add(msi);

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
                MongoStatementsInsert<VSNext.Mongo.Entities.Office365LyncP2PSessionReport> msi = new MongoStatementsInsert<VSNext.Mongo.Entities.Office365LyncP2PSessionReport>();
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
						//AllTestsList.SQLStatements.Add(new SQLstatements() { SQL = sqlQuery, DatabaseName = "vitalsigns" });
                        VSNext.Mongo.Entities.Office365LyncP2PSessionReport o365Server = new VSNext.Mongo.Entities.Office365LyncP2PSessionReport();
                        o365Server.ServerId = Convert.ToInt32(myServer.ServerId);
                        o365Server.AccountName = AccountName;
                        o365Server.TotalP2PSessions = Convert.ToInt32(TotalP2PSessions);
                        o365Server.P2PIMSessions = Convert.ToInt32(P2PIMSessions);
                        o365Server.P2PAudioSessions = Convert.ToInt32(P2PAudioSessions);
                        o365Server.P2PVideoSessions = Convert.ToInt32(P2PVideoSessions);
                        o365Server.P2PApplicationSharingSessions = Convert.ToInt32(P2PApplicationSharingSessions);
                        o365Server.P2PFileTransferSessions = Convert.ToInt32(P2PFileTransferSessions);
                        msi.listOfEntities.Add(o365Server);
						break;
					}
                    AllTestsList.MongoEntity.Add(msi);

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
		#region Deletesummarystatsdata
		public void Deletesummarystatsdata(MonitoredItems.Office365Server myServer, ref TestResults AllTestsList, string ServerType)
		{
			if (myServer.ServerType == "Office365")
			{
				string sql = "delete from MicrosoftSummaryStats where servername='" + myServer.Name + "'and ServerTypeId= " + myServer.ServerTypeId + " and CAST(Date AS DATE)  = CAST( getdate() AS DATE)";
				AllTestsList.SQLStatements.Add(new SQLstatements() { SQL = sql, DatabaseName = "VSS_Statistics" });
			}
		}
		#endregion
		public void doSummaryStats(MonitoredItems.Office365Server myServer, ref TestResults AllTestsList, ReturnPowerShellObjects powershellobj)
		{
			try
			{
				//get the
				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "doSummaryStats: Starting.", Common.LogLevel.Normal);
				string SqlQuery = "select COUNT(*) CNT from O365AdditionalMailDetails  where LastLogonTime > GETDATE()-1 and Server='" + myServer.Name + "'";
				string SqlQuery2 = "update dbo.O365AdditionalMailDetails set InActiveDaysCount=DATEDIFF(D,lastlogontime,getdate()) where lastlogontime is not null and Server='" + myServer.Name + "'";

				AllTestsList.SQLStatements.Add(new SQLstatements() { SQL = SqlQuery2, DatabaseName = "VSS_Statistics" });


				CommonDB DB = new CommonDB("VSS_STATISTICS");
				DataTable dtServers = DB.GetData(SqlQuery.ToString());
				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "doSummaryStats: Count of LastLogonTime > GETDATE()-1." + dtServers.Rows.Count.ToString(), Common.LogLevel.Normal);
				if (dtServers.Rows.Count > 0)
				{

					for (int i = 0; i < dtServers.Rows.Count; i++)
					{
						DataRow DR = dtServers.Rows[i];
						string activeUsersCount = DR["CNT"].ToString();

						DateTime dtNow = DateTime.Now;
						int weekNumber = culture.Calendar.GetWeekOfYear(dtNow, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday);
						string sqlQuery = "Insert into VSS_Statistics.dbo.MicrosoftSummaryStats(ServerName,ServerTypeId,Date,StatName,StatValue,WeekNumber,MonthNumber,YearNumber) "
								+ " values('" + myServer.Name + "','" + myServer.ServerTypeId + "',GetDate(),'ActiveUsersCount" + "'" + " ," + activeUsersCount +
							   "," + weekNumber + ", " + dtNow.Month.ToString() + ", " + dtNow.Year.ToString() + ")";
						AllTestsList.SQLStatements.Add(new SQLstatements() { SQL = sqlQuery, DatabaseName = "VSS_Statistics" });
					}
				}

				//string sSQL="insert into dbo.MicrosoftSummaryStats ('"+ myServer.Name + "','" + myServer.ServerTypeId.ToString() +"',"

			}
			catch (Exception ex)
			{
				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "getMailStatus: Exception: " + ex.Message.ToString(), Common.LogLevel.Verbose);
				//myServer.ADQueryTest = "Fail";
				//Common.makeAlert(false, myServer, commonEnums.AlertType.Mailbox_Database_Size, ref AllTestsList, myServer.ServerType);
			}
		}
	}
	public class Parameters
	{
		public MonitoredItems.Office365Server myServer;
		public ReturnPowerShellObjects PSO;
		public TestResults TS;
	}
}
