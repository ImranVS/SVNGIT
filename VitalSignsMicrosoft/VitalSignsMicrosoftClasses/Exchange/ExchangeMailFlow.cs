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
	class ExchangeMailFlow
	{
        public void PrereqForWindows(MonitoredItems.ExchangeMailProbe Server, ref TestResults AllTestsList)
        {
            string cmdlets = "-CommandName Test-Mailflow";
		    foreach(MonitoredItems.ExchangeServer exchangeServer in Server.ExchangeServers)
            {
                using (ReturnPowerShellObjects results = Common.PrereqForExchangeWithCmdlets(exchangeServer.Name, exchangeServer.UserName, exchangeServer.Password, "Exchange", exchangeServer.IPAddress, commonEnums.ServerRoles.MailFlow, cmdlets, exchangeServer.AuthenticationType))
                {
                    if (!results.Connected)
                        continue;
                    try
                    {
                        //getMailFlow(Server.MailProbeSourceServer,  Server, ref AllTestsList, results);
                        getMailFlowHeatMap(Server, exchangeServer, ref AllTestsList, results);

                        results.PS.Commands.Clear();
                    }
                    catch (Exception ex)
                    {
                        Common.WriteDeviceHistoryEntry(Server.ServerType, Server.Name, "Error in PrereqForWindows: " + ex.Message, commonEnums.ServerRoles.MailFlow, Common.LogLevel.Normal);
                    }
                    break;
                }
                //Thread.Sleep(10 * 60 * 1000);
                GC.Collect();
            }
        }

		public void getMailFlow(string Sourceservername, MonitoredItems.ExchangeServer myServer, ref TestResults AllTestsList, ReturnPowerShellObjects powershellobj)
		{
				
			Common.WriteDeviceHistoryEntry(myServer.ServerType, Sourceservername, "In getMailFlow.", commonEnums.ServerRoles.Windows, Common.LogLevel.Normal);

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
			}
			catch (Exception ex)
			{
				Common.WriteDeviceHistoryEntry(myServer.ServerType, Sourceservername, "Error in getMailFlow while getting settings: " + ex.Message, commonEnums.ServerRoles.MailFlow, Common.LogLevel.Normal);
			}

		}

		public void getMailFlowHeatMap(MonitoredItems.ExchangeMailProbe myServer, MonitoredItems.ExchangeServer exchangeServer, ref TestResults AllTestsList, ReturnPowerShellObjects powershellobj)
		{

			Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "In getMailFlowHeatMap.", commonEnums.ServerRoles.MailFlow, Common.LogLevel.Normal);
			PowerShell powershell = powershellobj.PS;

			try
			{
				//AllTestsList.SQLStatements.Add(new SQLstatements() { SQL = "DELETE  FROM dbo.MailLatencyStats", DatabaseName = "VSS_Statistics" });
				string dtNow = DateTime.Now.ToString();
				string PowerShellCmd = "$arr = @() \n\n";
				foreach (MonitoredItems.ExchangeServer s1 in myServer.ExchangeServers)
				{
                    foreach (MonitoredItems.ExchangeServer s2 in myServer.ExchangeServers)
                    {
                        if (s1 == s2)
                            continue;
                        Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "getMailFlowHeatMap from Server:" + s1.Name + " To Server:" + s2.Name, commonEnums.ServerRoles.MailFlow, Common.LogLevel.Normal);
                        PowerShellCmd += "$arr += Test-Mailflow " + s1.Name + " -TargetMailboxServer " + s2.Name + " -ExecutionTimeout " + (myServer.LatencyRedThreshold / 1000 + 60) + " -ErrorAction SilentlyContinue|Foreach-Object{New-Object PSObject -Property @{\n " +
                                            "SourceServer='" + s1.Name + "'\n" +
                                            "TargetServer='" + s2.Name + "'\n" +
                                            "TestMailflowResult=$_.TestMailflowResult\n" +
                                            "MessageLatencyTime=$_.MessageLatencyTime\n" +
                                            "}}\n\n";
                    }
				}

				PowerShellCmd += "$arr";

				Common.WriteDeviceHistoryEntry(myServer.ServerType, "HeatMap", "getMailFlowHeatMap PS Script:" +PowerShellCmd, commonEnums.ServerRoles.MailFlow, Common.LogLevel.Normal);
				System.Collections.ObjectModel.Collection<PSObject> results = new System.Collections.ObjectModel.Collection<PSObject>();
				powershell.Streams.Error.Clear();
                powershell.Commands.Clear();
				powershell.AddScript(PowerShellCmd);

				results = powershell.Invoke();
				Common.WriteDeviceHistoryEntry(myServer.ServerType, "HeatMap", "getMailFlowHeatMap result count:" + results.Count.ToString(), commonEnums.ServerRoles.MailFlow, Common.LogLevel.Normal);

                List<VSNext.Mongo.Entities.LatencyResults> listOfResults = new List<VSNext.Mongo.Entities.LatencyResults>();
                MongoStatementsUpsert<VSNext.Mongo.Entities.Status> mongoUpsert = new MongoStatementsUpsert<VSNext.Mongo.Entities.Status>();
                mongoUpsert.filterDef = mongoUpsert.repo.Filter.Eq(i => i.TypeAndName, myServer.TypeANDName);

				if (results.Count > 0)
				{

					int i = 0;
					string sql = "";

                    foreach (MonitoredItems.ExchangeServer s1 in myServer.ExchangeServers)
                    {
                        foreach (MonitoredItems.ExchangeServer s2 in myServer.ExchangeServers)
                        {
                            if (s1 == s2)
                                continue;
                            string source = s1.Name;
                            string target = s2.Name;


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

                                        listOfResults.Add(new VSNext.Mongo.Entities.LatencyResults()
                                        {
                                            SourceServer = source,
                                            DestinationServer = target,
                                            Latency = iFinalSec
                                        });
                                    }
                                    else
                                    {
                                        listOfResults.Add(new VSNext.Mongo.Entities.LatencyResults()
                                        {
                                            SourceServer = source,
                                            DestinationServer = target,
                                            Latency = -1
                                        });
                                    }
                                }
                                else
                                {
                                    i++;

                                    listOfResults.Add(new VSNext.Mongo.Entities.LatencyResults()
                                    {
                                        SourceServer = source,
                                        DestinationServer = target,
                                        Latency = -1
                                    });
                                }
                            }
                            else
                            {
                                listOfResults.Add(new VSNext.Mongo.Entities.LatencyResults()
                                {
                                    SourceServer = source,
                                    DestinationServer = target,
                                    Latency = -1
                                });
                            }

                        }

					}


                    mongoUpsert.updateDef = mongoUpsert.repo.Updater.Set(x => x.LatencyResults, listOfResults)
                        .Set(x => x.DeviceId, myServer.ServerObjectID);
                    AllTestsList.MongoEntity.Add(mongoUpsert);

                    MongoStatementsInsert<VSNext.Mongo.Entities.DailyStatistics> mongoInsert = new MongoStatementsInsert<VSNext.Mongo.Entities.DailyStatistics>();
                    List<VSNext.Mongo.Entities.DailyStatistics> listOfStats = new List<VSNext.Mongo.Entities.DailyStatistics>();
                    foreach (VSNext.Mongo.Entities.LatencyResults entity in listOfResults)
                        listOfStats.Add(new VSNext.Mongo.Entities.DailyStatistics()
                        {
                            DeviceId = myServer.ServerObjectID,
                            DeviceType = "ExchangeMailFlow",
                            DeviceName = "ExchangeMailFlow",
                            StatName = entity.SourceServer + "-to-" + entity.DestinationServer,
                            StatValue = Convert.ToDouble(entity.Latency)
                        });
                    mongoInsert.listOfEntities = listOfStats;
                    AllTestsList.MongoEntity.Add(mongoInsert);


                    if (listOfResults.Exists(x => x.Latency == -1))
                    {
                        AllTestsList.AlertDetails.Add(new Alerting() { AlertType = commonEnums.AlertType.YellowThreshold, Category = "Exchange Mail Probe", Details = "A server did not respond in the allowed time", DeviceName = myServer.Name, DeviceType = commonEnums.AlertDevice.Exchange_Mail_Probe, ResetAlertQueue = commonEnums.ResetAlert.No });
                        AllTestsList.AlertDetails.Add(new Alerting() { AlertType = commonEnums.AlertType.RedThreshold, Category = "Exchange Mail Probe", Details = "A server did not respond in the allowed time", DeviceName = myServer.Name, DeviceType = commonEnums.AlertDevice.Exchange_Mail_Probe, ResetAlertQueue = commonEnums.ResetAlert.No });
                    }
                    else if(listOfResults.Max(x => x.Latency) > myServer.LatencyRedThreshold)
                    {
                        AllTestsList.AlertDetails.Add(new Alerting() { AlertType = commonEnums.AlertType.YellowThreshold, Category = "Exchange Mail Probe", Details = "A server did not respond below the yellow threshold value", DeviceName = myServer.Name, DeviceType = commonEnums.AlertDevice.Exchange_Mail_Probe, ResetAlertQueue = commonEnums.ResetAlert.No });
                        AllTestsList.AlertDetails.Add(new Alerting() { AlertType = commonEnums.AlertType.RedThreshold, Category = "Exchange Mail Probe", Details = "A server did not respond below the red threshold value", DeviceName = myServer.Name, DeviceType = commonEnums.AlertDevice.Exchange_Mail_Probe, ResetAlertQueue = commonEnums.ResetAlert.No });
                    }
                    else if (listOfResults.Max(x => x.Latency) > myServer.LatencyYellowThreshold && listOfResults.Max(x => x.Latency) <= myServer.LatencyRedThreshold)
                    {
                        AllTestsList.AlertDetails.Add(new Alerting() { AlertType = commonEnums.AlertType.YellowThreshold, Category = "Exchange Mail Probe", Details = "A server did not respond below the yellow threshold value", DeviceName = myServer.Name, DeviceType = commonEnums.AlertDevice.Exchange_Mail_Probe, ResetAlertQueue = commonEnums.ResetAlert.No });
                        AllTestsList.AlertDetails.Add(new Alerting() { AlertType = commonEnums.AlertType.RedThreshold, Category = "Exchange Mail Probe", Details = "All servers responded within the red threshold value", DeviceName = myServer.Name, DeviceType = commonEnums.AlertDevice.Exchange_Mail_Probe, ResetAlertQueue = commonEnums.ResetAlert.Yes });
                    }
                    else if (listOfResults.Max(x => x.Latency) <= myServer.LatencyYellowThreshold)
                    {
                        AllTestsList.AlertDetails.Add(new Alerting() { AlertType = commonEnums.AlertType.YellowThreshold, Category = "Exchange Mail Probe", Details = "All servers responded within the yellow threshold value", DeviceName = myServer.Name, DeviceType = commonEnums.AlertDevice.Exchange_Mail_Probe, ResetAlertQueue = commonEnums.ResetAlert.Yes });
                        AllTestsList.AlertDetails.Add(new Alerting() { AlertType = commonEnums.AlertType.RedThreshold, Category = "Exchange Mail Probe", Details = "All servers responded within the red threshold value", DeviceName = myServer.Name, DeviceType = commonEnums.AlertDevice.Exchange_Mail_Probe, ResetAlertQueue = commonEnums.ResetAlert.Yes });
                    }

                }
			}
			catch (Exception ex)
			{
				Common.WriteDeviceHistoryEntry(myServer.ServerType, "HeatMap", "Error in getMailFlowHeatMap: " + ex.Message, commonEnums.ServerRoles.MailFlow, Common.LogLevel.Normal);
			}
		}
	}
}
