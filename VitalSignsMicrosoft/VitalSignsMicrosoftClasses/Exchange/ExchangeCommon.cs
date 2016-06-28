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
	class ExchangeCommon
	{
		public void PrereqForWindows(MonitoredItems.ExchangeServer Server, ref TestResults AllTestsList, ReturnPowerShellObjects results )
		{
			getServiceHealth(Server, ref AllTestsList, results);			
		}

		private void getServiceHealth( MonitoredItems.ExchangeServer myServer, ref TestResults AllTestsList, ReturnPowerShellObjects powershellobj)
		{
			//runspace = powershellobj.runspace;
			PowerShell powershell = powershellobj.PS;

			Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "In getServiceHealth ", commonEnums.ServerRoles.Windows, Common.LogLevel.Normal);
			try
			{
				String str = "Test-ServiceHealth";
				powershell.Streams.Error.Clear();

				powershell.AddScript(str);

				Collection<PSObject> results = powershell.Invoke();

				if (powershell.Streams.Error.Count > 51)
				{
					foreach (ErrorRecord er in powershell.Streams.Error)
						Console.WriteLine(er.ErrorDetails); 
					//AllTestsList.StatusDetails.Add(new TestList() { TestName = "Services", Details = "Services Errors:Error count>51 at " + System.DateTime.Now.ToShortTimeString(), Result = commonEnums.ServerResult.Fail });
					Common.makeAlert(false, myServer, commonEnums.AlertType.Services, ref AllTestsList, "Services Errors:Error count>51 at " + System.DateTime.Now.ToShortTimeString(), "Exchange");
				}
				else
				{
					Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "getServiceHealth output results: " + results.Count.ToString(), commonEnums.ServerRoles.Windows, Common.LogLevel.Normal);
					bool pass = true;

					string ServerRoles = "";

					foreach (PSObject ps in results)
					{
						string Role = ps.Properties["Role"].Value == null ? "" : ps.Properties["Role"].Value.ToString();
						string RequiredServicesRunning = ps.Properties["RequiredServicesRunning"].Value == null ? "" : ps.Properties["RequiredServicesRunning"].Value.ToString();
						string ServicesRunning = ps.Properties["ServicesRunning"].Value == null ? "" : ps.Properties["ServicesRunning"].Value.ToString();
						string ServicesNotRunning = ps.Properties["ServicesNotRunning"].Value == null ? "" : ps.Properties["ServicesNotRunning"].Value.ToString();



						switch (Role)
						{
							case "Mailbox Server Role":
								ServerRoles += "Mailbox,";
								break;
							case "Client Access Server Role":
								ServerRoles += "CAS,";
								break;
							case "Edge Transport Role":
								ServerRoles += "Edge,";
								break;
							case "Hub Transport Server Role":
								ServerRoles += "Hub,";
								break;
							case "Unified Messaging Server Role":
								ServerRoles += "Unified,";
								break;
							default:
								Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "Returned role doesnt exist: " + Role, commonEnums.ServerRoles.Windows, Common.LogLevel.Normal);
								break;
						}

						if (RequiredServicesRunning != "True")
							pass = false;


						string[] ListOfServices = (ServicesRunning + " " + ServicesNotRunning).Split(' ');

						CommonDB db = new CommonDB();


                        VSNext.Mongo.Repository.Repository<VSNext.Mongo.Entities.Server> ServerRepo = new VSNext.Mongo.Repository.Repository<VSNext.Mongo.Entities.Server>(db.GetMongoConnectionString());
                        if (ServerRepo.Find(i => i.ServerName == myServer.Name && i.ServerType == myServer.ServerType).Where(j => j.WindowServices != null && j.WindowServices.Where(k => k.ServerRequired).Count() > 0).Count() == 0)
                        {
                            foreach (string service in ListOfServices.Where(w => !string.IsNullOrEmpty(w)))
                            {
                                MongoStatementsUpdate<VSNext.Mongo.Entities.Server> updateStatement = new MongoStatementsUpdate<VSNext.Mongo.Entities.Server>();
                                updateStatement.filterDef = updateStatement.repo.Filter.Where(i => i.ServerName == myServer.Name && i.ServerType == myServer.ServerType)
                                    & updateStatement.repo.Filter.ElemMatch("windows_services", updateStatement.repo.Filter.Eq("service_name", service) & updateStatement.repo.Filter.Eq("server_required", false));
                                updateStatement.updateDef = updateStatement.repo.Updater
                                    .Set(i => i.WindowServices[-1].Monitored, true)
                                    .Set(i => i.WindowServices[-1].ServerRequired, true);

                                AllTestsList.MongoEntity.Add(updateStatement);
                            }
                        }
					}

					if (ServerRoles != "")
					{
						myServer.Role = ServerRoles.Remove(ServerRoles.Length - 1).Split(new char[] { ',' }); ;
						Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "New Server Roles: " + ServerRoles, commonEnums.ServerRoles.Windows, Common.LogLevel.Normal);
					}

					if (results.Count > 0)
					{
						if (pass)
						{
							//Common.makeAlert(true, myServer, commonEnums.AlertType.Services, ref AllTestsList, "All monitored services are running");
							//AllTestsList.StatusDetails.Add(new TestList() { Details = "All Required Services Running at " + System.DateTime.Now.ToShortTimeString(), TestName = "Services", Category = commonEnums.ServerRoles.CAS, Result = commonEnums.ServerResult.Pass });
						}
						else
						{
							//String sql = "SELECT DisplayName FROM WindowsServices WHERE ServerRequired=1 AND Status <> 'Running' AND ServerName='" + myServer.Name + "'";
							//CommonDB db = new CommonDB();
							//DataTable dt = db.GetData(sql);
							//if (dt.Rows.Count > 1)
							//    AllTestsList.StatusDetails.Add(new TestList() { Details = "There are " + dt.Rows.Count + " Services are not running at " + System.DateTime.Now.ToShortTimeString() + ".  See Health Assessment tab for more details.", TestName = "Services", Category = commonEnums.ServerRoles.CAS, Result = commonEnums.ServerResult.Fail });
							//else if (dt.Rows.Count == 1)
							//    AllTestsList.StatusDetails.Add(new TestList() { Details = "The service " + dt.Rows[0]["DisplayName"].ToString() + " is not running at " + System.DateTime.Now.ToShortTimeString() + ".  See Health Assessment tab for more details.", TestName = "Services", Category = commonEnums.ServerRoles.CAS, Result = commonEnums.ServerResult.Fail });
							//else
							//    AllTestsList.StatusDetails.Add(new TestList() { Details = "Some Services are not running at " + System.DateTime.Now.ToShortTimeString() + ".  See Health Assessment tab for more details.", TestName = "Services", Category = commonEnums.ServerRoles.CAS, Result = commonEnums.ServerResult.Fail });

						}

					}
					else
					{
						//AllTestsList.StatusDetails.Add(new TestList() { Details = "We were unable to get the required services at " + System.DateTime.Now.ToShortTimeString(), TestName = "Services", Category = commonEnums.ServerRoles.Windows, Result = commonEnums.ServerResult.Fail });

					}
				}

			}
			catch (Exception ex)
			{
				AllTestsList.StatusDetails.Add(new TestList() { Details = "There was an Error Executing the Required Services Script at " + System.DateTime.Now.ToShortTimeString(), TestName = "Services", Category = commonEnums.ServerRoles.Windows, Result = commonEnums.ServerResult.Fail });
				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "Error in getServicesHealth: " + ex.Message, commonEnums.ServerRoles.Windows, Common.LogLevel.Normal);

			}
			finally
			{

			}
		}


	}
}
