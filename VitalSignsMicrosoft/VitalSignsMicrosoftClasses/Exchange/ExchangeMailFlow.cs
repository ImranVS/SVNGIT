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
	class ExchangeMailFlow
	{
			public void PrereqForWindows(MonitoredItems.ExchangeServer Server, ref TestResults AllTestsList,System.Collections.ArrayList commandList)
			{
				string cmdlets = "-CommandName Test-Mailflow";
				
				using (ReturnPowerShellObjects results = Common.PrereqForExchangeWithCmdlets(Server.Name, Server.UserName, Server.Password, "Exchange", Server.IPAddress, commonEnums.ServerRoles.Windows, cmdlets, Server.AuthenticationType))
				{

					try
					{
						//getMailFlow(Server.MailProbeSourceServer,  Server, ref AllTestsList, results);
						getMailFlowHeatMap(Server.MailProbeSourceServer, Server, ref AllTestsList, results, commandList);

						results.PS.Commands.Clear();
					}
					catch (Exception ex)
					{
						Common.WriteDeviceHistoryEntry(Server.ServerType, Server.Name, "Error in PrereqForWindows: " + ex.Message, commonEnums.ServerRoles.MailFlow, Common.LogLevel.Normal);
					}

				}
				//Thread.Sleep(10 * 60 * 1000);
				GC.Collect();
			}

			public void getMailFlow(string Sourceservername, MonitoredItems.ExchangeServer myServer, ref TestResults AllTestsList, ReturnPowerShellObjects powershellobj)
			{
				
				Common.WriteDeviceHistoryEntry(myServer.ServerType, Sourceservername, "In getMailFlow.", commonEnums.ServerRoles.Windows, Common.LogLevel.Normal);
                //Common.WriteDeviceHistoryEntry("All", "Microsoft_Performance", "In getMailFlow.", Common.LogLevel.Normal);
				PowerShell powershell = powershellobj.PS;

				string emailAddress = "";
				string serverName = "";
				string testName = "";
				string threshold = "";
				try
				{
					
						Common.WriteDeviceHistoryEntry(myServer.ServerType, Sourceservername, "TestMailflowResult a record is configured", commonEnums.ServerRoles.Windows, Common.LogLevel.Normal);
						testName = myServer.MailProbeName;
						emailAddress = myServer.MailProbeAddress;
						serverName = myServer.Name;
						threshold = myServer.DeliveryThreshold.ToString();

						try
						{
							System.Collections.ObjectModel.Collection<PSObject> results = new System.Collections.ObjectModel.Collection<PSObject>();
							String str = "Test-Mailflow -ExecutionTimeout " + int.Parse(threshold.ToString()) * 60 + " " + Sourceservername + " -TargetEmailAddress " + emailAddress;
							Common.WriteDeviceHistoryEntry(myServer.ServerType, Sourceservername, "TestMailflowResult string: " + str, commonEnums.ServerRoles.Windows, Common.LogLevel.Normal);

							powershell.Streams.Error.Clear();
							powershell.AddScript(str);

							results = powershell.Invoke();
							Common.WriteDeviceHistoryEntry(myServer.ServerType, Sourceservername, "TestMailflowResult result count:" + results.Count.ToString(), commonEnums.ServerRoles.Windows, Common.LogLevel.Normal);
							if (results.Count > 0)
							{
								foreach (PSObject ps in results)
								{
									try
									{
										string status = ps.Properties["TestMailflowResult"].Value == null ? "Fail" : ps.Properties["TestMailflowResult"].Value.ToString();
										string sTime = ps.Properties["MessageLatencyTime"].Value == null ? "" : ps.Properties["MessageLatencyTime"].Value.ToString();

										if (sTime != "")
											sTime = sTime.Split(':')[2].ToString();


										Common.WriteDeviceHistoryEntry(myServer.ServerType, Sourceservername, "getMailFlow: TestMailflowResult status:" + status, commonEnums.ServerRoles.Windows, Common.LogLevel.Normal);
										string details = "";
										if (status == "Success")
										{
											status = "OK";
											details = "Exchange Mail flow Succeeded with latency time:" + sTime + " at: " + DateTime.Now;
											//AllTestsList.StatusDetails.Add(new TestList() { Details = details, TestName = myServer.ServerType, Category = commonEnums.ServerRoles.MailFlow, Result = commonEnums.ServerResult.Pass });
                                            Common.makeAlert(true, myServer, commonEnums.AlertType.Mail_flow, ref AllTestsList, details); 
										}
										else
										{
											status = "Issue";
											details = "Exchange Mail flow Failed with latency time:" + sTime + " at: " + DateTime.Now;
											//AllTestsList.StatusDetails.Add(new TestList() { Details = details, TestName = myServer.ServerType, Category = commonEnums.ServerRoles.MailFlow, Result = commonEnums.ServerResult.Fail });
                                            Common.makeAlert(false, myServer, commonEnums.AlertType.Mail_flow, ref AllTestsList, details);
										}

										SQLBuild objSQL = new SQLBuild();
										objSQL.ifExistsSQLSelect = "SELECT * FROM [vitalsigns].[dbo].[Status] where TypeANDName='" + testName + "-" + myServer.ServerType + "'";

										//If above query returns data, pass update statement
										objSQL.onTrueDML = " Update [vitalsigns].[dbo].[Status] set [Status]='" + status + "',StatusCode='" + status + "',Details='" + details + "' , LastUpdate='" + DateTime.Now + "', NextScan='" + myServer.NextScan + "' where TypeANDName='" + testName + "-" + myServer.ServerType+"'";

										//If above query does not return data, pass insert statement
										objSQL.onFalseDML = " INSERT INTO [vitalsigns].[dbo].[Status]([Name],[Location],[Type],[Category],[Details],[StatusCode],[Status],[TypeANDName],[LastUpdate],[NextScan]) VALUES ('" + testName + "','" + myServer.ServerType + "','" + myServer.ServerType +"','" + commonEnums.ServerRoles.MailFlow + "','" + details + "','" + status + "','" + status + "','" + testName + "-" + myServer.ServerType + "','" + DateTime.Now + "', '" + myServer.NextScan + "')";

										string sqlQuery = objSQL.GetSQL(objSQL);
										//Above GetSQL combines the given statements and returns a SQL string, which is to be passed to below List.
										AllTestsList.SQLStatements.Add(new SQLstatements() { SQL = sqlQuery, DatabaseName = "vitalsigns" });


										string strSQL = "INSERT INTO dbo.EXCHANGEMAILPROBEHISTORY(SENTDATETIME,SENTTO,DELIVERYTHRESHOLDINMINUTES,DELIVERYTIMEINMINUTES,STATUS,DETAILS,DEVICENAME,TARGETSERVER) VALUES('" + DateTime.Now.ToString() + "','" + emailAddress + "'," + threshold + "," + sTime + ",'" + status + "','" + "Exchange Mail flow Status: " + status + "','" + testName + "','" + Sourceservername + "')";
										Common.WriteDeviceHistoryEntry(myServer.ServerType, Sourceservername, "getMailFlow History:SQL:" + strSQL, commonEnums.ServerRoles.Windows, Common.LogLevel.Normal);
										AllTestsList.SQLStatements.Add(new SQLstatements() { SQL = strSQL, DatabaseName = "vitalsigns" });

										if (status == "OK")
										{
											strSQL = "INSERT INTO VSS_Statistics.dbo.EXCHANGEMAILSTATS(NAME,DATE,STATNAME,STATVALUE) VALUES('" + testName + "','" + DateTime.Now.ToString() + "','DeliveryTime.Seconds','" + sTime + "')";
											AllTestsList.SQLStatements.Add(new SQLstatements() { SQL = strSQL, DatabaseName = "VSS_StatisticS" });
										}
									}
									catch (Exception ex)
									{
										Common.WriteDeviceHistoryEntry(myServer.ServerType, Sourceservername, "Error in getMailFlow: " + ex.Message, commonEnums.ServerRoles.Windows, Common.LogLevel.Normal);
									}
								}
							}
							else
							{
								//AllTestsList.StatusDetails.Add(new TestList() { Details = "Exchange Mail flow Failed. Did not return any result", TestName = myServer.ServerType, Category = commonEnums.ServerRoles.MailFlow, Result = commonEnums.ServerResult.Fail });
                                Common.makeAlert(false, myServer, commonEnums.AlertType.Mail_flow, ref AllTestsList, "Exchange Mail flow Failed. Did not return any result");
								string status = "Fail";
								string details = "Exchange Mail flow Failed.";
								SQLBuild objSQL = new SQLBuild();
								objSQL.ifExistsSQLSelect = "SELECT * FROM [vitalsigns].[dbo].[Status] where TypeANDName='" + testName + "-" +myServer.ServerType+"'";

								//If above query returns data, pass update statement
								objSQL.onTrueDML = " Update [vitalsigns].[dbo].[Status] set [Status]='" + status + "',StatusCode='" + "Issue" + "',Details='" + details + "', LastUpdate='" + DateTime.Now + "', NextScan='" + myServer.NextScan + "' where TypeANDName='" + testName + "-" + myServer.ServerType+"'";

								//If above query does not return data, pass insert statement
								objSQL.onFalseDML = " INSERT INTO [vitalsigns].[dbo].[Status]([Name],[Location],[Type],[Category],[Details],[StatusCode],[Status],[TypeANDName],[LastUpdate],[NextScan]) VALUES ('" + testName + "','" + myServer.ServerType + "','" + myServer.ServerType+"','" + commonEnums.ServerRoles.MailFlow + "','" + details + "','" + status + "','" + status + "','" + testName + "-" +myServer.ServerType+"','" + DateTime.Now + "', '" + myServer.NextScan + "')";

								string sqlQuery = objSQL.GetSQL(objSQL);
								//Above GetSQL combines the given statements and returns a SQL string, which is to be passed to below List.
								AllTestsList.SQLStatements.Add(new SQLstatements() { SQL = sqlQuery, DatabaseName = "vitalsigns" });

							}
						}
						catch (Exception ex)
						{

							Common.WriteDeviceHistoryEntry(myServer.ServerType, Sourceservername, "Error in getMailFlow: " + ex.Message, commonEnums.ServerRoles.Windows, Common.LogLevel.Normal);

						}
                        //Common.WriteDeviceHistoryEntry("All", "Microsoft_Performance", "Ending For getMailFlow.", Common.LogLevel.Normal);
				}
				catch (Exception ex)
				{
					Common.WriteDeviceHistoryEntry(myServer.ServerType, Sourceservername, "Error in getMailFlow while getting settings: " + ex.Message, commonEnums.ServerRoles.MailFlow, Common.LogLevel.Normal);
				}

			}

			public void getMailFlowHeatMap(string Sourceservername, MonitoredItems.ExchangeServer myServer, ref TestResults AllTestsList, ReturnPowerShellObjects powershellobj,System.Collections.ArrayList ServerList)
			{

				Common.WriteDeviceHistoryEntry(myServer.ServerType, "HeatMap", "In getMailFlowHeatMap.", commonEnums.ServerRoles.MailFlow, Common.LogLevel.Normal);
				PowerShell powershell = powershellobj.PS;
                //Common.WriteDeviceHistoryEntry("All", "Microsoft_Performance", "In getMailFlowHeatMap.", Common.LogLevel.Normal);
				try
				{
					AllTestsList.SQLStatements.Add(new SQLstatements() { SQL = "DELETE  FROM dbo.MailLatencyStats", DatabaseName = "VSS_Statistics" });
					string dtNow = DateTime.Now.ToString();
					string PowerShellCmd = "$arr = @() \n\n";
					foreach (MonitoredItems.MailFlowTest s in ServerList)
					{
						Common.WriteDeviceHistoryEntry(myServer.ServerType, "HeatMap", "getMailFlowHeatMap from Server:" + s.SourceServer + " To Server:" + s.DestinationServer, commonEnums.ServerRoles.MailFlow, Common.LogLevel.Normal);
						PowerShellCmd += "$arr += Test-Mailflow " + s.SourceServer + " -TargetMailboxServer " + s.DestinationServer + " -ExecutionTimeout " + (s.LatencyRedThreshold / 1000 + 60) + " -ErrorAction SilentlyContinue|Foreach-Object{New-Object PSObject -Property @{\n " +
											"SourceServer='" + s.SourceServer + "'\n" +
											"TargetServer='" + s.DestinationServer + "'\n" +
											"TestMailflowResult=$_.TestMailflowResult\n" + 
											"MessageLatencyTime=$_.MessageLatencyTime\n" +
											"}}\n\n";
					}

					PowerShellCmd += "$arr";

					Common.WriteDeviceHistoryEntry(myServer.ServerType, "HeatMap", "getMailFlowHeatMap PS Script:" +PowerShellCmd, commonEnums.ServerRoles.MailFlow, Common.LogLevel.Normal);
					System.Collections.ObjectModel.Collection<PSObject> results = new System.Collections.ObjectModel.Collection<PSObject>();
					powershell.Streams.Error.Clear();
					powershell.AddScript(PowerShellCmd);

					results = powershell.Invoke();
					Common.WriteDeviceHistoryEntry(myServer.ServerType, "HeatMap", "getMailFlowHeatMap result count:" + results.Count.ToString(), commonEnums.ServerRoles.MailFlow, Common.LogLevel.Normal);
					if (results.Count > 0)
					{

						int i = 0;
						string sql = "";

						foreach (MonitoredItems.MailFlowTest s in ServerList)
						{
							string source = s.SourceServer;
							string target = s.DestinationServer;

							if (i < results.Count)
							{
								if (results[i] != null)
								{
									string psTarget = results[i].Properties["TargetServer"] == null ? "" : results[i].Properties["TargetServer"].Value.ToString();
									string psSource = results[i].Properties["SourceServer"] == null ? "" : results[i].Properties["SourceServer"].Value.ToString();
									string psStatus = results[i].Properties["TestMailflowResult"] == null ? "Fail" : results[i].Properties["TestMailflowResult"].Value.ToString();
									string psTime = results[i].Properties["MessageLatencyTime"] == null ? "" : results[i].Properties["MessageLatencyTime"].Value.ToString();

									if (psTarget == target && psSource == source)
									{
										i++;
										int sHour = Convert.ToInt32(psTime.Split(':')[0]) * 60 * 60;
										int sMin = Convert.ToInt32(psTime.Split(':')[1]) * 60;
										double sSec = Convert.ToDouble(psTime.Split(':')[2]);
										int iFinalSec = Convert.ToInt32((sHour + sMin + sSec) * 1000);

										sql += "('" + source + "','" + target + "','" + iFinalSec.ToString() + "','" + dtNow + "'),";
									}
									else
									{
										sql += "('" + source + "','" + target + "','-1','" + dtNow + "'),";
									}
								}
								else
								{
									i++;
									sql += "('" + source + "','" + target + "','-1','" + dtNow + "'),";
								}
							}
							else
							{
								sql += "('" + source + "','" + target + "','-1','" + dtNow + "'),";
							}



						}

						string sql1 = "INSERT INTO dbo.MailLatencyStats(sourceserver,destinationserver,latency,date) values" + sql.Substring(0, sql.Length - 1);
						AllTestsList.SQLStatements.Add(new SQLstatements() { SQL = sql1, DatabaseName = "VSS_Statistics" });
						string sql2 = "INSERT INTO dbo.MailLatencyDailyStats(sourceserver,destinationserver,latency,date) values" + sql.Substring(0, sql.Length - 1);
						AllTestsList.SQLStatements.Add(new SQLstatements() { SQL = sql2, DatabaseName = "VSS_Statistics" });
					}
				}
                    
				catch (Exception ex)
				{
					Common.WriteDeviceHistoryEntry(myServer.ServerType, "HeatMap", "Error in getMailFlowHeatMap: " + ex.Message, commonEnums.ServerRoles.MailFlow, Common.LogLevel.Normal);
				}
                //Common.WriteDeviceHistoryEntry("All", "Microsoft_Performance", "Ending For getMailFlowHeatMap.", Common.LogLevel.Normal);
			}
		}
}
