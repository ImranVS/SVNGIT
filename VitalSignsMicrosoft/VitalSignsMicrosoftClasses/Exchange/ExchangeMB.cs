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
            //Common.WriteDeviceHistoryEntry("All", "Microsoft_Performance","In testMailboxReplicationService", Common.LogLevel.Normal);

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
                //Common.WriteDeviceHistoryEntry("All", "Microsoft_Performance", "" + myServer.ServerType + myServer.Name + " End in In testMailboxReplicationService ", Common.LogLevel.Normal);

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

                //Common.WriteDeviceHistoryEntry("All", "Microsoft_Performance", "Ending For testMailboxReplicationService", Common.LogLevel.Normal);


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
            //Common.WriteDeviceHistoryEntry("All", "Microsoft_Performance", "In testMailboxAssisstantsService", Common.LogLevel.Normal);

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
                //Common.WriteDeviceHistoryEntry("All", "Microsoft_Performance", "" + myServer.ServerType + myServer.Name + " End in In testMailboxAssisstantsService ", Common.LogLevel.Normal);

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
                //Common.WriteDeviceHistoryEntry("All", "Microsoft_Performance", "Ending For testMailboxAssisstantsService", Common.LogLevel.Normal);


            }
            catch (Exception ex)
            {
                Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "Error in testMailboxAssisstantsService: " + ex.Message, commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);

            }
            finally
            {

            }
           

        }


        




		public void getMailBoxInfo(MonitoredItems.ExchangeServer Server, ref TestResults AllTestsList, string serverVersionNo, string DummyServernameForLogs, MonitoredItems.ExchangeServersCollection MyExchangeServers, ReturnPowerShellObjects results)
		{
			string ServerNames = "'" + String.Join("','", MyExchangeServers.Cast<MonitoredItems.ExchangeServer>().ToArray().Select(s => s.Name).ToList() + "-Exchange") + "'";
			AllTestsList.SQLStatements.Add(new SQLstatements() { DatabaseName = "Vitalsigns", SQL = "Delete from StatusDetail WHERE TypeANDName in (" + ServerNames + ") AND Category='Mailbox'" });
			
				getMailboxDatabaseDetails(Server, results.PS, ref AllTestsList, serverVersionNo, DummyServernameForLogs, MyExchangeServers);

				getIndividualMailboxes(results.PS, ref AllTestsList, DummyServernameForLogs, MyExchangeServers);
				getMailStats(Server, results.PS, ref AllTestsList, serverVersionNo, DummyServernameForLogs, MyExchangeServers);
				results.PS.Commands.Clear();


		}
		private void getMailboxDatabaseDetails(MonitoredItems.ExchangeServer Server, PowerShell powershell, ref TestResults AllTestResults, string serverVersionNo, string DummyServerName, MonitoredItems.ExchangeServersCollection MyExchangeServers)
		{
			try
			{

				string SqlStr = "select ServerName, DatabaseName, DatabaseSizeThreshold, WhiteSpaceThreshold from ExchangeDatabaseSettings";
				CommonDB db = new CommonDB();
				DataTable dt = db.GetData(SqlStr);
				bool overallWhitespaceAlert = true;
				bool overallDBSizeAlert = true;


				Common.WriteDeviceHistoryEntry("Exchange", DummyServerName, "In getMailboxDatabaseDetails.", commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);
                //Common.WriteDeviceHistoryEntry("All", "Microsoft_Performance", "In getMailboxDatabaseDetails.", Common.LogLevel.Normal);
                string ServerNames = "'" + String.Join("','", MyExchangeServers.Cast<MonitoredItems.ExchangeServer>().ToArray().Select(s => s.Name).ToList()) + "'";
				AllTestResults.SQLStatements.Add(new SQLstatements() { SQL = "DELETE FROM ExchangeMailboxOverview WHERE ServerName in (" + ServerNames + ")", DatabaseName = "vitalsigns" });

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
                    //Common.WriteDeviceHistoryEntry("All", "Microsoft_Performance", "getMailboxDatabaseDetails received over 51 errors", Common.LogLevel.Normal);
				}
				else
				{
					Common.WriteDeviceHistoryEntry("Exchange", DummyServerName, "getMailboxDatabaseDetails output results: " + results.Count.ToString(), commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);
                    //Common.WriteDeviceHistoryEntry("All", "Microsoft_Performance", "getMailboxDatabaseDetails output results: " + results.Count.ToString(), Common.LogLevel.Normal);

					foreach (PSObject ps in results)
					{

						string ServerName = ps.Properties["ServerName"].Value == null ? "" : ps.Properties["ServerName"].Value.ToString();
						string DatabaseName = ps.Properties["DataBaseName"].Value == null ? "" : ps.Properties["DataBaseName"].Value.ToString();
						string SizeMB = ps.Properties["SizeMB"].Value == null ? "" : ps.Properties["SizeMB"].Value.ToString();
						string WhiteSpaceMB = ps.Properties["WhiteSpaceMB"].Value == null ? "" : ps.Properties["WhiteSpaceMB"].Value.ToString();
						string MBXs = ps.Properties["MBXs"].Value == null ? "" : ps.Properties["MBXs"].Value.ToString();
						string MBXsdisc = ps.Properties["MBXsdisc"].Value == null ? "" : ps.Properties["MBXsdisc"].Value.ToString();
						string Mbcount = ps.Properties["Mbcount"].Value == null ? "" : ps.Properties["Mbcount"].Value.ToString();

						if (MyExchangeServers.SearchByName(ServerName) == null)
							return;

						string sqlQuery = "INSERT INTO ExchangeMailboxOverview ([ServerName],[DatabaseName],[SizeMB],[WhiteSpaceMB],[TotalMailboxes],[DisconnectMailboxes],[ConnectedMailboxes]) VALUES " +
							"('" + ServerName + "','" + DatabaseName + "'," + SizeMB + "," + WhiteSpaceMB + ",'" + MBXs + "','" + MBXsdisc + "','" + Mbcount + "')";


						AllTestResults.SQLStatements.Add(new SQLstatements() { SQL = sqlQuery, DatabaseName = "vitalsigns" });

						sqlQuery = " IF EXISTS (SELECT * FROM [vitalsigns].[dbo].[ExgMailHealth] where ServerName='" + ServerName + "')" +
										  " begin" +
										  " update [vitalsigns].[dbo].[ExgMailHealth] set [Status]='' where ServerName='" + ServerName + "'" +
										  " end " +
										   " else " +
										  " begin" +
										  " INSERT INTO [vitalsigns].[dbo].[ExgMailHealth]([ServerName],[Status]) VALUES ('" + ServerName + "','')" +
										  " end";

						AllTestResults.SQLStatements.Add(new SQLstatements() { SQL = sqlQuery, DatabaseName = "vitalsigns" });


						sqlQuery = " IF EXISTS (SELECT * FROM [vitalsigns].[dbo].[ExgMailHealthDetails] where ServerName='" + ServerName + "' and DatabaseName='" + DatabaseName + "')" +
										" begin" +
										" update [vitalsigns].[dbo].[ExgMailHealthDetails] set [MailBoxes]=" + Mbcount + ",[Size]=" + SizeMB + ",[WhiteSpaceSize]=" + WhiteSpaceMB + " where ServerName='" + ServerName + "' and DatabaseName='" + DatabaseName + "'" +
										" end " +
										 " else " +
										" begin" +
										" INSERT INTO [vitalsigns].[dbo].[ExgMailHealthDetails]([ServerName],[DatabaseName],[MailBoxes],[Size],[WhiteSpaceSize]) VALUES ('" + ServerName + "','" + DatabaseName + "'," + Mbcount + "," + SizeMB + "," + WhiteSpaceMB + ")" +
										" end";


						AllTestResults.SQLStatements.Add(new SQLstatements() { SQL = sqlQuery, DatabaseName = "vitalsigns" });


						sqlQuery = Common.GetInsertIntoDailyStats(ServerName, Server.ServerTypeId.ToString(), "ExDatabaseSizeMb." + DatabaseName, SizeMB);
						AllTestResults.SQLStatements.Add(new SQLstatements() { SQL = sqlQuery, DatabaseName = "VSS_Statistics" });



						//alerts
						if (dt.Rows.Count > 0 && dt.Rows[0]["DatabaseName"].ToString() != "NoAlerts")
						{
							DataRow[] curr;
							//if (dt.Rows[0]["DatabaseName"].ToString() == "AllDatabases")
							//{
							//    DataTable tempDT = new DataTable();
							//    tempDT.Rows.Add();
							//    tempDT.Columns.Add("DatabaseSizeThreshold", typeof(string));
							//    tempDT.Columns.Add("WhiteSpaceThreshold", typeof(string));
							//    tempDT.Rows[0]["DatabaseSizeThreshold"] = dt.Rows[0]["DatabaseSizeThreshold"].ToString();
							//    tempDT.Rows[0]["WhiteSpaceThreshold"] = dt.Rows[0]["WhiteSpaceThreshold"].ToString();
							//    curr = new DataRow()[1];
							//    curr[0] = tempDT.Rows[0];
							//}
							//else
							//{
							curr = dt.Select("ServerName='" + ServerName + "' AND (DatabaseName='" + DatabaseName + "' or DatabaseName='AllDatabases')");
							//}
							if (curr.Count() > 0)
							{
								//ServerName, ServerType, DatabaseName, DatabaseSizeThreshold, WhiteSpaceThreshold
								try
								{
									MonitoredItems.ExchangeServer currServer = MyExchangeServers.SearchByName(ServerName);

									if (currServer != null)
									{
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
											overallDBSizeAlert = false;
											overallWhitespaceAlert = false;
										}
										else if (boolSize)
										{
											//details = "The size of database " + DatabaseName + " excedes the threshold limit of " + Double.Parse(sizeThreshold);
											overallDBSizeAlert = false;
										}
										else if (boolWhiteSpace)
										{
											//details = "The size of the whitespace of database " + DatabaseName + " excedes the threshold of " + Double.Parse(whitespaceThreshold);
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
										 */
									}
								}
								catch (Exception ex)
								{
								}
							}
						}




					}

					string details = "";
					if (!overallDBSizeAlert && !overallWhitespaceAlert)
					{
						details = "The whitespace and the size of some databases excedes their thresholds.";
					}
					else if (!overallDBSizeAlert)
					{
						details = "The size of some databases excedes their thresholds.";
					}
					else if (!overallWhitespaceAlert)
					{
						details = "The size of the whitespace of some databases excedes their thresholds.";
					}
					else
					{
						details = "All databases are within their thresholds for size and whitespace";
					}

					if (!overallDBSizeAlert || !overallWhitespaceAlert)
						Common.makeAlert(false, Server, commonEnums.AlertType.Mailbox_Database_Size, ref AllTestResults, details, "MailBox");
					else
						Common.makeAlert(true, Server, commonEnums.AlertType.Mailbox_Database_Size, ref AllTestResults, details, "MailBox");

				}

			}
			catch (Exception ex)
			{

				Common.WriteDeviceHistoryEntry("Exchange", DummyServerName, "Error in getMailboxDatabaseDetails : " + ex.Message, commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);

			}

		}
		private void getIndividualMailboxes(PowerShell powershell, ref TestResults AllTestResults, string DummyServerForLogs, MonitoredItems.ExchangeServersCollection MyExchangeServers)
		{
			try
			{
				List<indvMailboxes> list = new List<indvMailboxes>();
				string ServerNames = "'" + String.Join("','", MyExchangeServers.Cast<MonitoredItems.ExchangeServer>().ToArray().Select(s => s.Name).ToList()) + "'";
				AllTestResults.SQLStatements.Add(new SQLstatements() { SQL = "DELETE FROM ExchangeMailFiles Where Server in (" + ServerNames + ")", DatabaseName = "VSS_Statistics" });

				Common.WriteDeviceHistoryEntry("Exchange", DummyServerForLogs, "In getIndividualMailboxes.", commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);
                //Common.WriteDeviceHistoryEntry("All", "Microsoft_Performance", "In getIndividualMailboxes.", Common.LogLevel.Normal);
				System.Collections.ObjectModel.Collection<PSObject> results = new System.Collections.ObjectModel.Collection<PSObject>();
				System.IO.StreamReader sr = new System.IO.StreamReader(AppDomain.CurrentDomain.BaseDirectory.ToString() + "Scripts\\EX_MailBoxQuotaStats.ps1");

				String str = sr.ReadToEnd();
				powershell.Streams.Error.Clear();

				powershell.AddScript(str);
				results = powershell.Invoke();

				//if (powershell.Streams.Error.Count > 51)
				//{
				//
				// Common.WriteDeviceHistoryEntry("Exchange", DummyServerForLogs, "getIndividualMailboxes received over 51 errors", commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);
				//}
				// else
				//{
				Common.WriteDeviceHistoryEntry("Exchange", DummyServerForLogs, "getIndividualMailboxes output results: " + results.Count.ToString(), commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);
                //Common.WriteDeviceHistoryEntry("All", "Microsoft_Performance", "getIndividualMailboxes output results: " + results.Count.ToString(), Common.LogLevel.Normal);
				foreach (PSObject ps in results)
				{

					string DisplayName = ps.Properties["DisplayName"].Value == null ? "" : ps.Properties["DisplayName"].Value.ToString();
					string Database = ps.Properties["Database"].Value == null ? "" : ps.Properties["Database"].Value.ToString();
					string IssueWarningQuota = "Unlimited"; //ps.Properties["IssueWarningQuota"].Value == null ? "Unlimited" : ps.Properties["IssueWarningQuota"].Value.ToString();
					string ProhibitSendQuota = "Unlimited";// ps.Properties["ProhibitSendQuota"].Value == null ? "Unlimited" : ps.Properties["ProhibitSendQuota"].Value.ToString();
					string ProhibitSendReceiveQuota = "Unlimited";//ps.Properties["ProhibitSendReceiveQuota"].Value == null ? "Unlimited" : ps.Properties["ProhibitSendReceiveQuota"].Value.ToString();
					string TotalItemSize = ps.Properties["TotalItemSize"].Value == null ? "0" : ps.Properties["TotalItemSize"].Value.ToString();
					string ItemCount = ps.Properties["ItemCount"].Value == null ? "0" : ps.Properties["ItemCount"].Value.ToString();
					string StorageLimitStatus = ps.Properties["StorageLimitStatus"].Value == null ? "" : ps.Properties["StorageLimitStatus"].Value.ToString();
					string ServerName = ps.Properties["ServerName"].Value == null ? "" : ps.Properties["ServerName"].Value.ToString();

					if (MyExchangeServers.SearchByName(ServerName) == null)
					{
						Common.WriteDeviceHistoryEntry("Exchange", DummyServerForLogs, "In loop.  Server not being scanned on this server.  Will not be added.", commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);
						continue;
					}
					string sqlQuery = "INSERT INTO ExchangeMailFiles ([ScanDate],[Database],[DisplayName],[Server],[IssueWarningQuota],[ProhibitSendQuota],[ProhibitSendReceiveQuota],[TotalItemSizeInMB],[ItemCount],[StorageLimitStatus]) VALUES " +
						"('" + DateTime.Now + "','" + Database + "','" + DisplayName + "','" + ServerName + "','" + IssueWarningQuota + "','" + ProhibitSendQuota + "','" + ProhibitSendReceiveQuota + "','" + TotalItemSize + "','" + ItemCount + "','" + StorageLimitStatus + "')";


					AllTestResults.SQLStatements.Add(new SQLstatements() { SQL = sqlQuery, DatabaseName = "VSS_Statistics" });


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

				Common.WriteDeviceHistoryEntry("Exchange", DummyServerForLogs, "Error in getIndividualMailboxes : " + ex.Message, commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);

			}

		}

		private void getMailStats(MonitoredItems.ExchangeServer Server, PowerShell powershell, ref TestResults AllTestResults, string serverVersionNo, string DummyServerForLogs, MonitoredItems.ExchangeServersCollection MyExchangeServers)
		{



			try
			{
				List<indvMailboxes> list = new List<indvMailboxes>();

				Common.WriteDeviceHistoryEntry("Exchange", DummyServerForLogs, "In getMailStats.", commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);
                //Common.WriteDeviceHistoryEntry("All", "Microsoft_Performance", "In getMailStats.", Common.LogLevel.Normal);
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
                //Common.WriteDeviceHistoryEntry("All", "Microsoft_Performance", "getMailStats output results: " + results.Count.ToString(), Common.LogLevel.Normal);
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


						string sqlQuery = Common.GetInsertIntoDailyStats(currServer, Server.ServerTypeId.ToString(), "Mail_DeliverySuccessRate", MailDeliverySuccessRate);
						AllTestResults.SQLStatements.Add(new SQLstatements() { SQL = sqlQuery, DatabaseName = "VSS_Statistics" });

						sqlQuery = Common.GetInsertIntoDailyStats(currServer, Server.ServerTypeId.ToString(), "Mail_ReceivedSizeMB", ReceivedSizeMB);
						AllTestResults.SQLStatements.Add(new SQLstatements() { SQL = sqlQuery, DatabaseName = "VSS_Statistics" });

						sqlQuery = Common.GetInsertIntoDailyStats(currServer, Server.ServerTypeId.ToString(), "Mail_ReceivedCount", ReceivedCount);
						AllTestResults.SQLStatements.Add(new SQLstatements() { SQL = sqlQuery, DatabaseName = "VSS_Statistics" });

						sqlQuery = Common.GetInsertIntoDailyStats(currServer, Server.ServerTypeId.ToString(), "Mail_SentSizeMB", SentSizeMB);
						AllTestResults.SQLStatements.Add(new SQLstatements() { SQL = sqlQuery, DatabaseName = "VSS_Statistics" });

						sqlQuery = Common.GetInsertIntoDailyStats(currServer, Server.ServerTypeId.ToString(), "Mail_SentCount", SentCount);
						AllTestResults.SQLStatements.Add(new SQLstatements() { SQL = sqlQuery, DatabaseName = "VSS_Statistics" });

						sqlQuery = Common.GetInsertIntoDailyStats(currServer, Server.ServerTypeId.ToString(), "Mail_DeliverCount", DeliverCount);
						AllTestResults.SQLStatements.Add(new SQLstatements() { SQL = sqlQuery, DatabaseName = "VSS_Statistics" });

						sqlQuery = Common.GetInsertIntoDailyStats(currServer, Server.ServerTypeId.ToString(), "Mail_FailCount", FailCount);
						AllTestResults.SQLStatements.Add(new SQLstatements() { SQL = sqlQuery, DatabaseName = "VSS_Statistics" });
					}
					else if (type == "User")
					{


						string Name = ps.Properties["Name"].Value == null ? "" : ps.Properties["Name"].Value.ToString();
						string Sent = ps.Properties["Sent"].Value == null ? "0" : ps.Properties["Sent"].Value.ToString();
						string Received = ps.Properties["Received"].Value == null ? "0" : ps.Properties["Received"].Value.ToString();
						string SentSizeMB = ps.Properties["SentSizeMB"].Value == null ? "0" : ps.Properties["SentSizeMB"].Value.ToString();
						string RecSizeMB = ps.Properties["RecSizeMB"].Value == null ? "0" : ps.Properties["RecSizeMB"].Value.ToString();



						//insert to table
						string sqlQuery = Common.GetInsertIntoDailyStats(Server.Name, Server.ServerTypeId.ToString(), "Mailbox." + Name + ".Sent.Count", Sent);
						AllTestResults.SQLStatements.Add(new SQLstatements() { SQL = sqlQuery, DatabaseName = "VSS_Statistics" });

						sqlQuery = Common.GetInsertIntoDailyStats(Server.Name, Server.ServerTypeId.ToString(), "Mailbox." + Name + ".Received.Count", Received);
						AllTestResults.SQLStatements.Add(new SQLstatements() { SQL = sqlQuery, DatabaseName = "VSS_Statistics" });

						sqlQuery = Common.GetInsertIntoDailyStats(Server.Name, Server.ServerTypeId.ToString(), "Mailbox." + Name + ".Sent.SizeMB", SentSizeMB);
						AllTestResults.SQLStatements.Add(new SQLstatements() { SQL = sqlQuery, DatabaseName = "VSS_Statistics" });

						sqlQuery = Common.GetInsertIntoDailyStats(Server.Name, Server.ServerTypeId.ToString(), "Mailbox." + Name + ".Received.SizeMB", RecSizeMB);
						AllTestResults.SQLStatements.Add(new SQLstatements() { SQL = sqlQuery, DatabaseName = "VSS_Statistics" });

					}

				}

			}
			catch (Exception ex)
			{

				Common.WriteDeviceHistoryEntry("Exchange", DummyServerForLogs, "Error in getMailStats : " + ex.Message, commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);

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
