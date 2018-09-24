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

using MongoDB.Driver;

using VSFramework;
using System;

namespace VitalSignsMicrosoftClasses
{
	class ExchangeMB : IServerRole
	{

		public ExchangeMB()
		{

		}

		public void CheckServer(MonitoredItems.ExchangeServer myServer, ReturnPowerShellObjects powerShellObjects, ref TestResults AllTestResults)
		{
            Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "In CheckServer for Mailbox", commonEnums.ServerRoles.MailBox, Common.LogLevel.Normal);

            testMailboxReplicationService(myServer, AllTestResults, powerShellObjects);
            testMailboxAssisstantsService(myServer, AllTestResults, powerShellObjects);

            Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "Done CheckServer for Mailbox", commonEnums.ServerRoles.MailBox, Common.LogLevel.Normal);
		}


        private void testMailboxReplicationService(MonitoredItems.ExchangeServer myServer, TestResults AllTestResults, ReturnPowerShellObjects PSO)
        {

            Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "In testMailboxReplicationService", commonEnums.ServerRoles.MailBox, Common.LogLevel.Normal);

            try
            {
                PowerShell powershell = PSO.PS;

                String str = "Test-MRSHealth " + myServer.Name + " | ? { $_.Passed -ne 'True' }";
                //String str = "Test-ServiceHealth | select Role";
                //GetWMIPowerShell(ref powershell, creds, IPAddress, str, false);
                powershell.Streams.Error.Clear();

                powershell.AddScript(str);

                Collection<PSObject> results = powershell.Invoke();


                Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "testMailboxReplicationService output results: " + results.Count.ToString(), commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);

                if(results.Count > 0)
                {

                    string errorStr = "";

                    foreach (PSObject ps in results)
                    {
                        string Check = ps.Properties["Check"].Value == null ? "" : ps.Properties["Check"].Value.ToString();
                        string Message = ps.Properties["Message"].Value == null ? "" : ps.Properties["Message"].Value.ToString();


                        errorStr += "The test for " + Check + " did not pass and produced the following error: " + Message + "";
                    

                    }

                    Common.makeAlert(false, myServer, commonEnums.AlertType.Mailbox_Replication_Service_Test, ref AllTestResults, errorStr, "Mailbox");

                }
                else
                {

                    Common.makeAlert(true, myServer, commonEnums.AlertType.Mailbox_Replication_Service_Test, ref AllTestResults, "There were no errors found in the Mailbox Replication Service", "Mailbox");

                }

                

            }
            catch (Exception ex)
            {
                Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "Error in testMailboxReplicationService: " + ex.Message, commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);

            }
            finally
            {

            }
            

    }

        private void testMailboxAssisstantsService(MonitoredItems.ExchangeServer myServer, TestResults AllTestResults, ReturnPowerShellObjects PSO)
        {

            Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "In testMailboxAssisstantsService", commonEnums.ServerRoles.MailBox, Common.LogLevel.Normal);

            try
            {
                PowerShell powershell = PSO.PS;

                String str = "(Test-AssistantHealth -ServerName " + myServer.Name + ")| % {$_.Events[0].Split(\"`n\") | % { if($_.StartsWith('Message')) { $_.Replace('Message: ', '') } } }";
                //String str = "Test-ServiceHealth | select Role";
                //GetWMIPowerShell(ref powershell, creds, IPAddress, str, false);
                powershell.Streams.Error.Clear();

                powershell.AddScript(str);

                Collection<PSObject> results = powershell.Invoke();


                Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "testMailboxAssisstantsService output results: " + results.Count.ToString(), commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);

                string errorStr = "";

                foreach (PSObject ps in results)
                {
                    string baseObj = ps.BaseObject.ToString();

                    if (!baseObj.StartsWith("The mailbox assistants troubleshooter didn't detect any problems with the Assistant service on "))
                    {
                        errorStr += baseObj + " ";
                    }                   

                }

                if (errorStr == "")
                { 
                    Common.makeAlert(true, myServer, commonEnums.AlertType.MailBox_Assistants_Service_Test, ref AllTestResults, "No errors were found in the service.", "Mailbox");
                }
                else
                {
                    Common.makeAlert(false, myServer, commonEnums.AlertType.MailBox_Assistants_Service_Test, ref AllTestResults, errorStr, "Mailbox");
                }


            }
            catch (Exception ex)
            {
                Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "Error in testMailboxAssisstantsService: " + ex.Message, commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);

            }
            finally
            {

            }
            

        }


        




		public void getMailBoxInfo(MonitoredItems.ExchangeServer Server, ref TestResults AllTestsList, string DummyServernameForLogs, MonitoredItems.ExchangeServersCollection MyExchangeServers, ReturnPowerShellObjects results)
		{
			getIndividualMailboxes(results.PS, ref AllTestsList, DummyServernameForLogs, MyExchangeServers);
			getMailStats(Server, results.PS, ref AllTestsList, DummyServernameForLogs, MyExchangeServers);
            getUsersAndGroups(Server, results.PS, ref AllTestsList, DummyServernameForLogs, MyExchangeServers);
            //getMailboxPermissions(Server, results.PS, ref AllTestsList, DummyServernameForLogs, MyExchangeServers);
			results.PS.Commands.Clear();
		}
		
 		private void getIndividualMailboxes(PowerShell powershell, ref TestResults AllTestResults, string DummyServerForLogs, MonitoredItems.ExchangeServersCollection MyExchangeServers)
		{
			try
			{
				List<indvMailboxes> list = new List<indvMailboxes>();
				string ServerNames = "'" + String.Join("','", MyExchangeServers.Cast<MonitoredItems.ExchangeServer>().ToArray().Select(s => s.Name).ToList()) + "'";

				Common.WriteDeviceHistoryEntry("Exchange", DummyServerForLogs, "In getIndividualMailboxes.", commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);

				System.Collections.ObjectModel.Collection<PSObject> results = new System.Collections.ObjectModel.Collection<PSObject>();
				System.IO.StreamReader sr = new System.IO.StreamReader(AppDomain.CurrentDomain.BaseDirectory.ToString() + "Scripts\\EX_MailBoxQuotaStats.ps1");
                String str = sr.ReadToEnd();


                string startingPoint = "a";
                CommonDB db = new CommonDB();
                //search db for the starting point and override if value found
                VSNext.Mongo.Repository.Repository<VSNext.Mongo.Entities.Server> serverRepo = new VSNext.Mongo.Repository.Repository<VSNext.Mongo.Entities.Server>(db.GetMongoConnectionString());

                for (char currStarting = startingPoint[0]; currStarting < 'z'; currStarting++)
                {
                    powershell.Streams.Error.Clear();
                    PSCommand cmd = new PSCommand();
                    //cmd.AddCommand(AppDomain.CurrentDomain.BaseDirectory.ToString() + "Scripts\\EX_MailBoxQuotaStats.ps1");
                    cmd.AddScript(str);
                    cmd.AddParameter("startingChar", currStarting);
                    powershell.Commands = cmd;
                    results = powershell.Invoke();

                    Common.WriteDeviceHistoryEntry("Exchange", DummyServerForLogs, "getIndividualMailboxes output results for " + currStarting + " : " + results.Count.ToString(), commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);

                    foreach (PSObject ps in results)
                    {
                        try
                        {
                            string DisplayName = ps.Properties["DisplayName"].Value == null ? "" : ps.Properties["DisplayName"].Value.ToString();
                            string Database = ps.Properties["Database"].Value == null ? "" : ps.Properties["Database"].Value.ToString();
                            string IssueWarningQuota = ps.Properties["IssueWarningQuota"].Value == null ? "Unlimited" : ps.Properties["IssueWarningQuota"].Value.ToString();
                            string ProhibitSendQuota = ps.Properties["ProhibitSendQuota"].Value == null ? "Unlimited" : ps.Properties["ProhibitSendQuota"].Value.ToString();
                            string ProhibitSendReceiveQuota = ps.Properties["ProhibitSendReceiveQuota"].Value == null ? "Unlimited" : ps.Properties["ProhibitSendReceiveQuota"].Value.ToString();
                            string TotalItemSize = ps.Properties["TotalItemSize"].Value == null ? "0" : ps.Properties["TotalItemSize"].Value.ToString();
                            string ItemCount = ps.Properties["ItemCount"].Value == null ? "0" : ps.Properties["ItemCount"].Value.ToString();
                            string StorageLimitStatus = ps.Properties["StorageLimitStatus"].Value == null ? "" : ps.Properties["StorageLimitStatus"].Value.ToString();
                            string ServerName = ps.Properties["ServerName"].Value == null ? "" : ps.Properties["ServerName"].Value.ToString();
                            string SAMAccountName = ps.Properties["SAMAccountName"].Value == null ? "" : ps.Properties["SAMAccountName"].Value.ToString();
                            string PrimarySmtpAddress = ps.Properties["PrimarySmtpAddress"].Value == null ? "" : ps.Properties["PrimarySmtpAddress"].Value.ToString();
                            string Company = ps.Properties["Company"].Value == null ? "" : ps.Properties["Company"].Value.ToString();
                            string Department = ps.Properties["Department"].Value == null ? "" : ps.Properties["Department"].Value.ToString();
                            string LastLogonTime = ps.Properties["LastLogonTime"] == null || ps.Properties["LastLogonTime"].Value == null ? null : ps.Properties["LastLogonTime"].Value.ToString();
                            string Identity = ps.Properties["Identity"].Value == null ? "" : ps.Properties["Identity"].Value.ToString();
                            string RetentionPolicy = ps.Properties["RetentionPolicy"].Value == null ? "" : ps.Properties["RetentionPolicy"].Value.ToString();
                            string LitigationHoldEnabled = ps.Properties["LitigationHoldEnabled"].Value == null ? "" : ps.Properties["LitigationHoldEnabled"].Value.ToString();
                            string RecipientTypeDetails = ps.Properties["RecipientTypeDetails"].Value == null ? "" : ps.Properties["RecipientTypeDetails"].Value.ToString();
                            string DistinguishedName = ps.Properties["DistinguishedName"].Value == null ? "" : ps.Properties["DistinguishedName"].Value.ToString();

                            List<VSNext.Mongo.Entities.Mailbox.Folder> listOfFolders = new List<VSNext.Mongo.Entities.Mailbox.Folder>();
                            try
                            {
                                PSObject folders = (PSObject)ps.Properties["Folders"].Value;
                                if (folders != null)
                                {
                                    PSObject[] results2 = ((System.Collections.ArrayList)folders.BaseObject).OfType<PSObject>().ToArray();
                                    foreach (PSObject ps2 in results2)
                                    {
                                        try
                                        {
                                            string Name = ps2.Properties["Name"].Value == null ? "" : ps2.Properties["Name"].Value.ToString();
                                            string ItemsInFolder = ps2.Properties["ItemsInFolder"].Value == null ? "" : ps2.Properties["ItemsInFolder"].Value.ToString();
                                            string DeletedItemsInFolder = ps2.Properties["DeletedItemsInFolder"].Value == null ? "" : ps2.Properties["DeletedItemsInFolder"].Value.ToString();
                                            string FolderSize = ps2.Properties["FolderSize"].Value == null ? "" : ps2.Properties["FolderSize"].Value.ToString();
                                            string ItemsInFolderAndSubfolders = ps2.Properties["ItemsInFolderAndSubfolders"].Value == null ? "" : ps2.Properties["ItemsInFolderAndSubfolders"].Value.ToString();
                                            string FolderAndSubfolderSize = ps2.Properties["FolderAndSubfolderSize"].Value == null ? "" : ps2.Properties["FolderAndSubfolderSize"].Value.ToString();

                                            int tempInt = 0;
                                            double tempDouble = 0;
                                            string bytesInFolder = "0";
                                            string bytesInSubFolder = "0";

                                            System.Text.RegularExpressions.MatchCollection matches = new System.Text.RegularExpressions.Regex(@"(?<=\()(\d+[,.]?)*").Matches(FolderSize);
                                            if (matches.Count > 0)
                                                bytesInFolder = matches[0].Value;

                                            matches = new System.Text.RegularExpressions.Regex(@"(?<=\()(\d+[,.]?)*").Matches(FolderAndSubfolderSize);
                                            if (matches.Count > 0)
                                                bytesInSubFolder = matches[0].Value;

                                            VSNext.Mongo.Entities.Mailbox.Folder folder = new VSNext.Mongo.Entities.Mailbox.Folder();
                                            folder.Name = Name;
                                            folder.ItemCount = int.TryParse(ItemsInFolder, out tempInt) ? (int?)tempInt : null;
                                            folder.DeletedItemCount = int.TryParse(DeletedItemsInFolder, out tempInt) ? (int?)tempInt : null;
                                            folder.TotalItemSizeMb = double.TryParse(bytesInFolder, out tempDouble) ? (double?)tempDouble / 1024 / 1024 : null;
                                            folder.ItemsAndSubfolderItemsCount = int.TryParse(ItemsInFolderAndSubfolders, out tempInt) ? (int?)tempInt : null;
                                            folder.ItemsAndSubfolderItemsSizeMb = double.TryParse(bytesInSubFolder, out tempDouble) ? (double?)tempDouble / 1024 / 1024 : null;
                                            listOfFolders.Add(folder);
                                        }
                                        catch (Exception ex)
                                        {
                                            var st = new StackTrace(ex, true);
                                            // Get the top stack frame
                                            var frame = st.GetFrame(0);
                                            // Get the line number from the stack frame
                                            var line = frame.GetFileLineNumber();
                                            Common.WriteDeviceHistoryEntry("Exchange", DummyServerForLogs, "Exception processing specfic folder at line " + line + ". Folder: " + ps2.ToString() + ". Exception: " + ex.Message.ToString(), commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);
                                        }
                                    }
                                }
                            }
                            catch (Exception fodlerException)
                            {
                                var st = new StackTrace(fodlerException, true);
                                // Get the top stack frame
                                var frame = st.GetFrame(0);
                                // Get the line number from the stack frame
                                var line = frame.GetFileLineNumber();
                                Common.WriteDeviceHistoryEntry("Exchange", DummyServerForLogs, "Exception processing folders at line " + line.ToString() + ". Exception: " + fodlerException.Message.ToString(), commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);
                            }
                            if (MyExchangeServers.SearchByName(ServerName) == null)
                            {
                                Common.WriteDeviceHistoryEntry("Exchange", DummyServerForLogs, "In loop.  Server not being scanned on this server.  Will not be added.", commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);
                                //continue;
                            }

                            bool testBool = false;
                            MongoStatementsUpsert<VSNext.Mongo.Entities.Mailbox> mongoStatement = new MongoStatementsUpsert<VSNext.Mongo.Entities.Mailbox>();
                            mongoStatement.filterDef = mongoStatement.repo.Filter.Where(i => i.DatabaseName == Database && i.DisplayName == DisplayName && i.DeviceName == "Exchange");
                            mongoStatement.updateDef = mongoStatement.repo.Updater
                                .Set(i => i.IssueWarningQuota, IssueWarningQuota)
                                .Set(i => i.ProhibitSendQuota, ProhibitSendQuota)
                                .Set(i => i.ProhibitSendReceiveQuota, ProhibitSendReceiveQuota)
                                .Set(i => i.TotalItemSizeMb, Convert.ToDouble(TotalItemSize))
                                .Set(i => i.ItemCount, Convert.ToInt32(ItemCount))
                                .Set(i => i.StorageLimitStatus, StorageLimitStatus)
                                .Set(i => i.SAMAccountName, SAMAccountName)
                                .Set(i => i.PrimarySmtpAddress, PrimarySmtpAddress)
                                .Set(i => i.Company, Company)
                                .Set(i => i.Department, Department)
                                .Set(i => i.Folders, listOfFolders)
                                .Set(i => i.LastLogonTime, LastLogonTime == null ? null : Convert.ToDateTime(LastLogonTime) as DateTime?)
                                .Set(i => i.Identity, Identity)
                                .Set(i => i.RetentionPolicy, RetentionPolicy)
                                .Set(i => i.LitigationHoldEnabled, Boolean.TryParse(LitigationHoldEnabled, out testBool) ? testBool : false)
                                .Set(x => x.RecipientTypeDetails, RecipientTypeDetails)
                                .Set(x => x.DistinguishedName, DistinguishedName);

                            AllTestResults.MongoEntity.Add(mongoStatement);



                            double testForNum;
                            if (double.TryParse(TotalItemSize, out testForNum))
                            {
                                if (Double.TryParse(ProhibitSendReceiveQuota, out testForNum) && Convert.ToDouble(TotalItemSize) > Convert.ToDouble(ProhibitSendReceiveQuota))
                                {
                                    //issue alert for send/receive quota
                                    list.Add(new indvMailboxes(ServerName, "SendAndReceive"));
                                }
                                else if (Double.TryParse(ProhibitSendQuota, out testForNum) && Convert.ToDouble(TotalItemSize) > Convert.ToDouble(ProhibitSendQuota))
                                {
                                    //issue alert for send quota
                                    list.Add(new indvMailboxes(ServerName, "Send"));
                                }
                                else
                                {
                                    //reset alert for send/receive quota
                                    list.Add(new indvMailboxes(ServerName, "NoAlerts"));
                                }
                            }
                            
                        }
                        catch(Exception ex)
                        {
                            var st = new StackTrace(ex, true);
                            // Get the top stack frame
                            var frame = st.GetFrame(0);
                            // Get the line number from the stack frame
                            var line = frame.GetFileLineNumber();

                            Common.WriteDeviceHistoryEntry("Exchange", DummyServerForLogs, "Error in getIndividualMailboxes processing a mailbox at line " + line.ToString() + ": " + ex.Message, commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);

                        }
                    }
                    
                }
				String[] servers = list.Select(L => L.ServerName).ToList().ToArray();
				foreach (string serverName in list.Select(L => L.ServerName).Distinct())
				{
					try
					{
						MonitoredItems.ExchangeServer currServer = MyExchangeServers.SearchByName(serverName);

						List<indvMailboxes> currList = list.Where(L => L.ServerName == serverName).ToList();
						if (currServer == null)
							continue;
						bool reset = false;

						int NoAlerts = currList.Where(L => L.TypeOfAlert == "NoAlerts").Count();
						int Send = currList.Where(L => L.TypeOfAlert == "Send").Count();
						int SendAndReceive = currList.Where(L => L.TypeOfAlert == "SendAndReceive").Count();

						String details = "";
						if (SendAndReceive > 0)
						{
							details = "There are " + SendAndReceive + " mailboxes that cannot receive emails";
							Common.makeAlert(false, currServer, commonEnums.AlertType.Mailbox_Receive_Prohibited, ref AllTestResults, details, "MailBox");
						}
						else
						{
							details = "All mailboxes can receive emails";
							Common.makeAlert(true, currServer, commonEnums.AlertType.Mailbox_Receive_Prohibited, ref AllTestResults, details, "MailBox");
						}

						if (Send > 0)
						{
							details = "There are " + Send + " mailboxes that cannot send emails";
							Common.makeAlert(false, currServer, commonEnums.AlertType.Mailbox_Send_Prohibited, ref AllTestResults, details, "MailBox");
						}
						else
						{
							details = "All mailboxes can send emails";
							Common.makeAlert(true, currServer, commonEnums.AlertType.Mailbox_Send_Prohibited, ref AllTestResults, details, "MailBox");
						}
					}
					catch (Exception ex)
					{
						Common.WriteDeviceHistoryEntry("Exchange", DummyServerForLogs, "Error in getIndividualMailboxes loop : " + ex.Message, commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);
					}

				}
			}
			catch (Exception ex)
			{
                var st = new StackTrace(ex, true);
                // Get the top stack frame
                var frame = st.GetFrame(0);
                // Get the line number from the stack frame
                var line = frame.GetFileLineNumber();

                Common.WriteDeviceHistoryEntry("Exchange", DummyServerForLogs, "Error in getIndividualMailboxes at line " + line.ToString() + ": " + ex.Message, commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);

			}

		}

		private void getMailStats(MonitoredItems.ExchangeServer Server, PowerShell powershell, ref TestResults AllTestResults, string DummyServerForLogs, MonitoredItems.ExchangeServersCollection MyExchangeServers)
		{



			try
			{
				List<indvMailboxes> list = new List<indvMailboxes>();

				Common.WriteDeviceHistoryEntry("Exchange", DummyServerForLogs, "In getMailStats.", commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);

				System.Collections.ObjectModel.Collection<PSObject> results = new System.Collections.ObjectModel.Collection<PSObject>();
				String str = AppDomain.CurrentDomain.BaseDirectory.ToString() + "Scripts\\EX_MailStats.ps1";

				//String str = sr.ReadToEnd();
				powershell.Streams.Error.Clear();

				System.IO.StreamReader sr = new System.IO.StreamReader(str);
				String s = sr.ReadToEnd();

				powershell.Commands.Clear();
				powershell.AddScript(s);

				results = powershell.Invoke();


				Common.WriteDeviceHistoryEntry("Exchange", DummyServerForLogs, "getMailStats output results: " + results.Count.ToString(), commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);

				foreach (ErrorRecord err in powershell.Streams.Error)
				{
					Common.WriteDeviceHistoryEntry("Exchange", DummyServerForLogs, "getMailStats PS errors: " + err.Exception, commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);
					Common.WriteDeviceHistoryEntry("Exchange", DummyServerForLogs, "getMailStats PS errors: " + err.ErrorDetails, commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);
				}

				foreach (PSObject ps in results)
				{

					string type = ps.Properties["Type"].Value == null ? "" : ps.Properties["Type"].Value.ToString();

					//WS Added Type for VSPLUS-1423
					if (type == "Server")
					{

						string currServer = ps.Properties["Server"].Value == null ? "" : ps.Properties["Server"].Value.ToString();
						string MailDeliverySuccessRate = ps.Properties["MailDeliverySuccessRate"].Value == null ? "" : ps.Properties["MailDeliverySuccessRate"].Value.ToString();
						string ReceivedSizeMB = ps.Properties["ReceivedSizeMB"].Value == null ? "0" : ps.Properties["ReceivedSizeMB"].Value.ToString();
						string ReceivedCount = ps.Properties["ReceivedCount"].Value == null ? "0" : ps.Properties["ReceivedCount"].Value.ToString();
						string SentSizeMB = ps.Properties["SentSizeMB"].Value == null ? "0" : ps.Properties["SentSizeMB"].Value.ToString();
						string SentCount = ps.Properties["SentCount"].Value == null ? "0" : ps.Properties["SentCount"].Value.ToString();
						string DeliverCount = ps.Properties["DeliverCount"].Value == null ? "0" : ps.Properties["DeliverCount"].Value.ToString();
						string FailCount = ps.Properties["FailCount"].Value == null ? "0" : ps.Properties["FailCount"].Value.ToString();

						if (MyExchangeServers.SearchByName(currServer) == null)
							continue;

						if (MailDeliverySuccessRate == "")
						{
							if (FailCount == "0" && DeliverCount == "0")
							{
								MailDeliverySuccessRate = "1";
							}
							else
							{
								double numerator = Convert.ToInt32(DeliverCount);
								double denominator = Convert.ToInt32(DeliverCount) + Convert.ToInt32(FailCount);
								MailDeliverySuccessRate = Math.Round(numerator / denominator, 2).ToString();
							}
						}


                        AllTestResults.MongoEntity.Add(Common.GetInsertIntoDailyStats(MyExchangeServers.SearchByName(currServer), "Mail_DeliverySuccessRate", MailDeliverySuccessRate));
                        AllTestResults.MongoEntity.Add(Common.GetInsertIntoDailyStats(MyExchangeServers.SearchByName(currServer), "Mail_ReceivedSizeMB", ReceivedSizeMB));
                        AllTestResults.MongoEntity.Add(Common.GetInsertIntoDailyStats(MyExchangeServers.SearchByName(currServer), "Mail_ReceivedCount", ReceivedCount));
                        AllTestResults.MongoEntity.Add(Common.GetInsertIntoDailyStats(MyExchangeServers.SearchByName(currServer), "Mail_SentSizeMB", SentSizeMB));
                        AllTestResults.MongoEntity.Add(Common.GetInsertIntoDailyStats(MyExchangeServers.SearchByName(currServer), "Mail_SentCount", SentCount));
                        AllTestResults.MongoEntity.Add(Common.GetInsertIntoDailyStats(MyExchangeServers.SearchByName(currServer), "Mail_DeliverCount", DeliverCount));
                        AllTestResults.MongoEntity.Add(Common.GetInsertIntoDailyStats(MyExchangeServers.SearchByName(currServer), "Mail_FailCount", FailCount));

					}
					else if (type == "User")
					{


						string Name = ps.Properties["Name"].Value == null ? "" : ps.Properties["Name"].Value.ToString();
						string Sent = ps.Properties["Sent"].Value == null ? "0" : ps.Properties["Sent"].Value.ToString();
						string Received = ps.Properties["Received"].Value == null ? "0" : ps.Properties["Received"].Value.ToString();
						string SentSizeMB = ps.Properties["SentSizeMB"].Value == null ? "0" : ps.Properties["SentSizeMB"].Value.ToString();
						string RecSizeMB = ps.Properties["RecSizeMB"].Value == null ? "0" : ps.Properties["RecSizeMB"].Value.ToString();



						//insert to table

                        AllTestResults.MongoEntity.Add(Common.GetInsertIntoDailyStats(Server, "Mailbox." + Name + ".Sent.Count", Sent));
                        AllTestResults.MongoEntity.Add(Common.GetInsertIntoDailyStats(Server, "Mailbox." + Name + ".Received.Count", Received));
                        AllTestResults.MongoEntity.Add(Common.GetInsertIntoDailyStats(Server, "Mailbox." + Name + ".Sent.SizeMB", SentSizeMB));
                        AllTestResults.MongoEntity.Add(Common.GetInsertIntoDailyStats(Server, "Mailbox." + Name + ".Received.SizeMB", RecSizeMB));
					}

				}

			}
			catch (Exception ex)
			{

				Common.WriteDeviceHistoryEntry("Exchange", DummyServerForLogs, "Error in getMailStats : " + ex.Message, commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);

			}





		}

        private void getUsersAndGroups(MonitoredItems.ExchangeServer Server, PowerShell powershell, ref TestResults AllTestResults, string DummyServerForLogs, MonitoredItems.ExchangeServersCollection MyExchangeServers)
        {
            try
            {
                List<indvMailboxes> list = new List<indvMailboxes>();

                Common.WriteDeviceHistoryEntry("Exchange", DummyServerForLogs, "In getUsersAndGroups.", commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);

                System.Collections.ObjectModel.Collection<PSObject> results = new System.Collections.ObjectModel.Collection<PSObject>();
                String str = AppDomain.CurrentDomain.BaseDirectory.ToString() + "Scripts\\EX_UsersAndGroups.ps1";

                //String str = sr.ReadToEnd();
                powershell.Streams.Error.Clear();

                System.IO.StreamReader sr = new System.IO.StreamReader(str);
                String s = sr.ReadToEnd();

                powershell.Commands.Clear();
                powershell.AddScript(s);

                results = powershell.Invoke();


                Common.WriteDeviceHistoryEntry("Exchange", DummyServerForLogs, "getUsersAndGroups output results: " + results.Count.ToString(), commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);

                foreach (ErrorRecord err in powershell.Streams.Error)
                {
                    Common.WriteDeviceHistoryEntry("Exchange", DummyServerForLogs, "getUsersAndGroups PS errors: " + err.Exception, commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);
                    Common.WriteDeviceHistoryEntry("Exchange", DummyServerForLogs, "getUsersAndGroups PS errors: " + err.ErrorDetails, commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);
                }
                MongoStatementsUpsert<VSNext.Mongo.Entities.UsersAndGroups> mongoUpsert = new MongoStatementsUpsert<VSNext.Mongo.Entities.UsersAndGroups>();
                List<VSNext.Mongo.Entities.UsersAndGroups> userAndGroupList = mongoUpsert.repo.Find(mongoUpsert.repo.Filter.Where(x => true)).ToList();
                Dictionary<string, string> dictOfNameIds = new Dictionary<string, string>();
                foreach (PSObject ps in results)
                {
                    MongoStatementsUpsert<VSNext.Mongo.Entities.UsersAndGroups> upsert = new MongoStatementsUpsert<VSNext.Mongo.Entities.UsersAndGroups>();
                    VSNext.Mongo.Entities.UsersAndGroups obj = new VSNext.Mongo.Entities.UsersAndGroups();


                    string Identity = ps.Properties["Id"].Value == null ? "" : ps.Properties["Id"].Value.ToString();
                    string objectId = MongoDB.Bson.ObjectId.GenerateNewId().ToString();

                    DefinitionContainer<VSNext.Mongo.Entities.UsersAndGroups> defContainer = new DefinitionContainer<VSNext.Mongo.Entities.UsersAndGroups>();
                    defContainer.filterDef = mongoUpsert.repo.Filter.Eq(x => x.Identity, Identity);
                    
                    defContainer.updateDef = mongoUpsert.repo.Updater.Set(x => x.Type, ps.Properties["Type"].Value == null ? "" : ps.Properties["Type"].Value.ToString())
                        .Set(x => x.DistinguishedName, ps.Properties["DistinguishedName"].Value == null ? "" : ps.Properties["DistinguishedName"].Value.ToString())
                        .Set(x => x.Name, ps.Properties["Name"].Value == null ? "" : ps.Properties["Name"].Value.ToString())
                        .Set(x => x.IsValid, ps.Properties["IsValid"].Value == null ? "" : ps.Properties["IsValid"].Value.ToString())
                        .Set(x => x.DisplayName, ps.Properties["DisplayName"].Value == null ? "" : ps.Properties["DisplayName"].Value.ToString())
                        .Set(x => x.SamAccountName, ps.Properties["SamAccountName"].Value == null ? "" : ps.Properties["SamAccountName"].Value.ToString())
                        .SetOnInsert(x => x.Id, objectId );

                    dictOfNameIds.Add(Identity, objectId);

                    if (obj.Type == "User")
                    {
                        defContainer.updateDef = defContainer.updateDef.Set(x => x.UserPrincipalName, ps.Properties["UserPrincipalName"].Value == null ? "" : ps.Properties["UserPrincipalName"].Value.ToString());
                    }
                    else if (obj.Type == "Group")
                    {
                        object members =  ps.Properties["Members"].Value == null ? null : ps.Properties["Members"].Value;
                        if (members != null)
                        {
                            foreach (string member in (System.Collections.ArrayList)((PSObject)members).BaseObject)
                            {
                                if(userAndGroupList.Exists(x => x.Identity == member))
                                {
                                    defContainer.updateDef = defContainer.updateDef.AddToSet(x => x.Members, userAndGroupList.Find(x => x.Identity == member).Id);
                                }
                            }
                        }
                    }
                    mongoUpsert.listOfDefinitions.Add(defContainer);
                }

                AllTestResults.MongoEntity.Add(mongoUpsert);
                
            }
            catch (Exception ex)
            {
                Common.WriteDeviceHistoryEntry("Exchange", DummyServerForLogs, "Error in getUsersAndGroups : " + ex.Message, commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);
            }
        }

        public void getMailboxPermissions(MonitoredItems.ExchangeServer Server, PowerShell powershell, ref TestResults AllTestResults, string DummyServerForLogs, MonitoredItems.ExchangeServersCollection MyExchangeServers)
        {

            string str1 = "";

            try
            {
                List<indvMailboxes> list = new List<indvMailboxes>();

                Common.WriteDeviceHistoryEntry("Exchange", DummyServerForLogs, "In getMailboxPermissions.", commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);

                System.Collections.ObjectModel.Collection<PSObject> results = new System.Collections.ObjectModel.Collection<PSObject>();
                String str = AppDomain.CurrentDomain.BaseDirectory.ToString() + "Scripts\\EX_GetMailboxPermissions.ps1";

                //String str = sr.ReadToEnd();
                powershell.Streams.Error.Clear();

                System.IO.StreamReader sr = new System.IO.StreamReader(str);
                String s = sr.ReadToEnd();

                CommonDB db = new CommonDB();
                VSNext.Mongo.Repository.Repository<VSNext.Mongo.Entities.Mailbox> mailboxRepo = new VSNext.Mongo.Repository.Repository<VSNext.Mongo.Entities.Mailbox>(db.GetMongoConnectionString());
                MongoStatementsUpdate<VSNext.Mongo.Entities.Mailbox> mongoMailboxUpdate;

                VSNext.Mongo.Repository.Repository<VSNext.Mongo.Entities.UsersAndGroups> usersGroupRepo = new VSNext.Mongo.Repository.Repository<VSNext.Mongo.Entities.UsersAndGroups>(db.GetMongoConnectionString());
                MongoStatementsUpdate<VSNext.Mongo.Entities.UsersAndGroups> mongoUserGroupUpdate;

                List<VSNext.Mongo.Entities.Mailbox> listOfMailboxes = mailboxRepo.Find(x => x.DeviceName == "Exchange").ToList().OrderBy(x => x.LastPermissionCheck == null).OrderBy(x => x.LastPermissionCheck).ToList();
                Dictionary<string, string> mailboxesDict = listOfMailboxes.Where(x => x.Identity != null).ToDictionary(x => x.Identity, x => x.Id);
                Dictionary<string, string> usersAndGroupsDict = usersGroupRepo.Find(x => true).ToList().Where(x => x.Identity != null).ToDictionary(x => x.Identity, x => x.Id);
                
                foreach (VSNext.Mongo.Entities.Mailbox mailbox in listOfMailboxes)
                {
                    powershell.Commands.Clear();
                    powershell.AddScript(s);
                    powershell.AddParameter("MailboxName", mailbox.SAMAccountName);

                    results = powershell.Invoke();

                    Common.WriteDeviceHistoryEntry("Exchange", DummyServerForLogs, "getMailboxPermissions output results: " + results.Count.ToString(), commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);

                    foreach (ErrorRecord err in powershell.Streams.Error)
                    {
                        Common.WriteDeviceHistoryEntry("Exchange", DummyServerForLogs, "getMailboxPermissions PS errors: " + err.Exception, commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);
                        Common.WriteDeviceHistoryEntry("Exchange", DummyServerForLogs, "getMailboxPermissions PS errors: " + err.ErrorDetails, commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);
                    }

                    if (results.Count > 0)
                    {
                        mongoMailboxUpdate = new MongoStatementsUpdate<VSNext.Mongo.Entities.Mailbox>(mailboxRepo);
                        mongoMailboxUpdate.filterDef = mailboxRepo.Filter.Eq(x => x.Identity, mailbox.Identity);
                        mongoMailboxUpdate.updateDef = mailboxRepo.Updater.Unset(x => x.UsersWithPermission);
                        mongoMailboxUpdate.Execute();

                        mongoUserGroupUpdate = new MongoStatementsUpdate<VSNext.Mongo.Entities.UsersAndGroups>(usersGroupRepo);
                        mongoUserGroupUpdate.filterDef = usersGroupRepo.Filter.Empty;
                        mongoUserGroupUpdate.updateDef = usersGroupRepo.Updater.PullFilter(x => x.Mailboxes, new FilterDefinitionBuilder<VSNext.Mongo.Entities.UsersAndGroups.MailboxInfo>().Eq(y => y.MailboxId, mailbox.Id));
                        mongoUserGroupUpdate.Execute();
                    }

                    foreach (PSObject ps in results)
                    {
                        
                        string User = ps.Properties["User"].Value == null ? "" : ps.Properties["User"].Value.ToString();
                        string Identity = ps.Properties["Identity"].Value == null ? "" : ps.Properties["Identity"].Value.ToString();
                        str1 += "Mailbox: " + Identity + "...User: " + User + '\n';
                        string userGroupObjectId = usersAndGroupsDict[User];
                        string mailboxObjectId = mailboxesDict[Identity];

                        mongoMailboxUpdate = new MongoStatementsUpdate<VSNext.Mongo.Entities.Mailbox>(mailboxRepo);
                        mongoMailboxUpdate.filterDef = mailboxRepo.Filter.Eq(x => x.Identity, Identity);
                        mongoMailboxUpdate.updateDef = mailboxRepo.Updater.AddToSet(x => x.UsersWithPermission, userGroupObjectId);
                        mongoMailboxUpdate.Execute();
                        
                        mongoUserGroupUpdate = new MongoStatementsUpdate<VSNext.Mongo.Entities.UsersAndGroups>(usersGroupRepo);
                        mongoUserGroupUpdate.filterDef = usersGroupRepo.Filter.Eq(x => x.Identity, User);
                        mongoUserGroupUpdate.updateDef = usersGroupRepo.Updater.AddToSet(x => x.Mailboxes, new VSNext.Mongo.Entities.UsersAndGroups.MailboxInfo() { DisplayName = mailbox.DisplayName, MailboxId = mailbox.Id, MailboxSizeMb = mailbox.TotalItemSizeMb } );
                        mongoUserGroupUpdate.Execute();
                    }

                    mongoMailboxUpdate = new MongoStatementsUpdate<VSNext.Mongo.Entities.Mailbox>(mailboxRepo);
                    mongoMailboxUpdate.filterDef = mailboxRepo.Filter.Eq(x => x.Id, mailbox.Id);
                    mongoMailboxUpdate.updateDef = mailboxRepo.Updater.Set(x => x.LastPermissionCheck, DateTime.Now);
                    mongoMailboxUpdate.Execute();

                }
                //= new MongoStatementsUpsert<VSNext.Mongo.Entities.Mailbox>(mailboxRepo);
                
                


                

                

            }
            catch (Exception ex)
            {

                Common.WriteDeviceHistoryEntry("Exchange", DummyServerForLogs, "Error in getMailboxPermissions : " + ex, commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);

            }





        }


                            }

    public class indvMailboxes
		{
			public String ServerName, TypeOfAlert;
			public indvMailboxes(String serverName, String typeOfAlert)
			{
				ServerName=serverName;
				TypeOfAlert=typeOfAlert;
			}

			
		}
}
