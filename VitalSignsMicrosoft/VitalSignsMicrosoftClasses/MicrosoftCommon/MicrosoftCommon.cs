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
using MongoDB.Driver;
using System;

namespace VitalSignsMicrosoftClasses
{
	class MicrosoftCommon
	{
		CultureInfo culture = CultureInfo.CurrentCulture;
		public void PrereqForWindows(MonitoredItems.MicrosoftServer Server, TestResults AllTestsList, ReturnPowerShellObjects PSO)
		{
			Common.WriteDeviceHistoryEntry(Server.ServerType, Server.Name, "In PrereqForWindows.", commonEnums.ServerRoles.Windows, Common.LogLevel.Normal);
			try
			{
				Thread winThread = null;
				if (Server.FastScan)
				{
					Common.WriteDeviceHistoryEntry(Server.ServerType, Server.Name, "In PrereqForWindows Fast Scan.", commonEnums.ServerRoles.Windows, Common.LogLevel.Normal);
					winThread = new Thread(() => getResponseTime( Server, AllTestsList, PSO));
					winThread.Name = Server.Name + " WIN_RT";
					WaitForWinThread(winThread, Server);

				}
				else
				{
					Common.WriteDeviceHistoryEntry(Server.ServerType, Server.Name, "In PrereqForWindows. Call Individual Functions. ", commonEnums.ServerRoles.Windows, Common.LogLevel.Normal);
					System.Security.SecureString securePassword = Common.String2SecureString(Server.Password);
					PSCredential creds = new PSCredential(Server.UserName, securePassword);

					//event log
					winThread = new Thread(() => GetEventLog(creds, Server, AllTestsList, PSO));
					winThread.Name = Server.Name + " WIN_CompVersAndDays";
					WaitForWinThread(winThread, Server);
					PSO.PS.Commands.Clear();

					//Working, displaying in Dashboard
					winThread = new Thread(() => GetCPU(creds, Server, AllTestsList, PSO));
					winThread.Name = Server.Name + " WIN_CPU";
					WaitForWinThread(winThread, Server);
					PSO.PS.Commands.Clear();

					// Call Individual Functions
					//Getting output RPC Client Access & Outlook Web App counts, where to save & display
					if (Server.ServerType == "Exchange")
					{
						winThread = new Thread(() => GetActiveConnections(creds, Server, AllTestsList, PSO));
						winThread.Name = Server.Name + " WIN_ActiveConnections";
						WaitForWinThread(winThread, Server);
						PSO.PS.Commands.Clear();
					}
					//Working, displaying in Dashboard
					winThread = new Thread(() => GetServices(creds, Server, AllTestsList, PSO));
					winThread.Name = Server.Name + " WIN_Services";
					WaitForWinThread(winThread, Server);
					PSO.PS.Commands.Clear();

					//15Jan14,Working earlier, giving nulls now.saving to ExchangeDailyStats table
					winThread = new Thread(() => GetPhysicalMemory(creds, Server, AllTestsList, PSO));
					winThread.Name = Server.Name + " WIN_PhysicalMemory";
					WaitForWinThread(winThread, Server);
					PSO.PS.Commands.Clear();

					//Working, displaying in Dashboard
					winThread = new Thread(() => GetDiskSpace(creds, Server, AllTestsList, PSO));
					winThread.Name = Server.Name + " WIN_DiskSpace";
					WaitForWinThread(winThread, Server);
					PSO.PS.Commands.Clear();

					//PingServer(creds, Server, ref AllTestsList);
					winThread = new Thread(() => GetComputerVersionAndDays(creds, Server, AllTestsList, PSO));
					winThread.Name = Server.Name + " WIN_CompVersAndDays";
					WaitForWinThread(winThread, Server);
					PSO.PS.Commands.Clear();

					

					//GetQueue(runspace, Server.Name, ServerType, creds, Server.IPAddress, Server, ref AllTestsList);
					winThread = new Thread(() => getResponseTime(Server, AllTestsList, PSO));
					winThread.Name = Server.Name + " WIN_RT";
					WaitForWinThread(winThread, Server);
					PSO.PS.Commands.Clear();
				}
				Common.WriteDeviceHistoryEntry(Server.ServerType, Server.Name, "In PrereqForWindows.Completed Individual Functions. ", commonEnums.ServerRoles.Windows, Common.LogLevel.Normal);
			}
			catch (Exception ex)
			{
				Common.WriteDeviceHistoryEntry(Server.ServerType, Server.Name, "Error in PrereqForWindows: " + ex.Message, commonEnums.ServerRoles.Windows, Common.LogLevel.Normal);
			}

		}

		private void GetServices(PSCredential creds, MonitoredItems.MicrosoftServer myServer, TestResults AllTestsList, ReturnPowerShellObjects PSO)
		{
			Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "In GetServices. ", commonEnums.ServerRoles.Windows, Common.LogLevel.Normal);

			string sql = "Select Service_Name, Monitored from WindowsServices where ServerName='" + myServer.Name + "' and Monitored=1";
			CommonDB db = new CommonDB();
			DataTable dt = db.GetData(sql);

            VSNext.Mongo.Repository.Repository<VSNext.Mongo.Entities.Server> ServerRepo = new VSNext.Mongo.Repository.Repository<VSNext.Mongo.Entities.Server>(db.GetMongoConnectionString());
            List<VSNext.Mongo.Entities.Server> listOfServers = ServerRepo.Find(i => i.DeviceName == myServer.Name).ToList();
            List<VSNext.Mongo.Entities.WindowServices> windowsServicesExistingList;
            List<VSNext.Mongo.Entities.WindowServices> windowsServicesNewList = new List<VSNext.Mongo.Entities.WindowServices>();
            if (listOfServers.Count > 0 && listOfServers[0].WindowServices != null && listOfServers[0].WindowServices.Count > 0)
                windowsServicesExistingList = listOfServers[0].WindowServices;
            else
                windowsServicesExistingList = new List<VSNext.Mongo.Entities.WindowServices>();

			string downServices = "";
            string getdownservices = "";
			try
			{
				GetWMIPowerShell(ref PSO, creds, myServer.IPAddress, "GetServices.ps1", true);
				{
					Collection<PSObject> results = PSO.PS.Invoke();
					Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "Get-WMIObject Win32_Service output results: " + results.Count.ToString(), commonEnums.ServerRoles.Windows, Common.LogLevel.Normal);
					foreach (PSObject ps in results)
					{
						
						DataRow[] row = dt.Select("Service_Name='" +ps.Properties["Name"].Value.ToString()+ "'");
                        if (row.Count() > 0 && ps.Properties["State"].Value.ToString() != "Running")
                        {
                            downServices += ps.Properties["Caption"].Value.ToString() + ",";
                            getdownservices += ps.Properties["Name"].Value.ToString() + ",";
                        }

                        VSNext.Mongo.Entities.WindowServices winService = windowsServicesExistingList.Find(i => i.ServiceName == ps.Properties["Name"].Value.ToString());
                        if (winService != null)
                        {
                            winService.DisplayName = ps.Properties["Name"].Value.ToString();
                            winService.Status = ps.Properties["State"].Value.ToString();
                            winService.StartupMode = ps.Properties["StartMode"].Value.ToString();
                            winService.DisplayName = ps.Properties["Caption"].Value.ToString();
                        }
                        else
                        {
                            winService = new VSNext.Mongo.Entities.WindowServices();
                            winService.ServiceName = ps.Properties["Name"].Value.ToString();
                            winService.Status = ps.Properties["State"].Value.ToString();
                            winService.StartupMode = ps.Properties["StartMode"].Value.ToString();
                            winService.DisplayName = ps.Properties["Caption"].Value.ToString();
                            winService.Monitored = false;
                            winService.ServerRequired = false;
                        }

                        windowsServicesNewList.Add(winService);

                       
					}
                    MongoStatementsUpdate<VSNext.Mongo.Entities.Server> mongoUpdate = new MongoStatementsUpdate<VSNext.Mongo.Entities.Server>();
                    mongoUpdate.filterDef = mongoUpdate.repo.Filter.Where(i => i.DeviceName == myServer.Name && i.DeviceType == myServer.ServerType);
                    mongoUpdate.updateDef = mongoUpdate.repo.Updater.Set(i => i.WindowServices, windowsServicesNewList);
                    AllTestsList.MongoEntity.Add(mongoUpdate);
			                  
					if (downServices != "")
					{
                        string Details="";
						string[] servicesnames = downServices.Substring(0, downServices.Length - 1).Split(',');
                        string[] newservices = getdownservices.Substring(0, getdownservices.Length - 1).Split(',');
                        if (servicesnames.Count() >= 1)
                        {
                            for (int i = 0; i < servicesnames.Count(); i++)
                            {
                                Details = "The service " + newservices[i] + " is not running as of " + System.DateTime.Now.ToShortTimeString() + ".";

                                string getservice = newservices[i].ToString();
                               string Services = "Services"+ ":" + getservice;
                                
                              
                                Common.makeAlert(false, myServer,  Services, ref AllTestsList, Details, "Windows");
                            }
                        }
                        
                        else
                        {
                            Details = "Some Services are not running as of " + System.DateTime.Now.ToShortTimeString() + ".";

                        }
					}
					else
					{
						Common.makeAlert(true, myServer, commonEnums.AlertType.Services, ref AllTestsList, "All monitored services are running", "Windows");
					}
				}
			}
			catch (Exception ex)
			{
				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "Error in GetServices: " + ex.Message, commonEnums.ServerRoles.Windows, Common.LogLevel.Normal);
			}
			finally
			{
				// Finally dispose the powershell and set all variables to null to free
				// up any resources.
				//powershell.Runspace.Close();
				//powershell.Runspace.Dispose();
				//powershell.Dispose();
				//powershell = null;
			}
		}

		private void GetActiveConnections(PSCredential creds, MonitoredItems.MicrosoftServer myServer, TestResults AllTestsList, ReturnPowerShellObjects PSO)
		{
			Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "In GetActiveConnections. ", commonEnums.ServerRoles.Windows, Common.LogLevel.Normal);
			if (myServer.GetType() != typeof(MonitoredItems.ExchangeServer))
				return;
			MonitoredItems.ExchangeServer server = myServer as MonitoredItems.ExchangeServer;
			try
			{
				GetWMIPowerShell(ref PSO, creds, myServer.IPAddress, "EX_Get_CASActiveConnections.ps1", true);
				CommonDB DB = new CommonDB();

				Collection<PSObject> results = PSO.PS.Invoke();
				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "EX_Get_CASActiveConnections output results: " + results.Count.ToString(), commonEnums.ServerRoles.Windows, Common.LogLevel.Normal);
				foreach (PSObject ps in results)
				{
					DateTime dtNow = DateTime.Now;
					int weekNumber = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(dtNow, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday);
                    if (Convert.ToInt32(ps.Properties["RPC Client Access"].Value.ToString()) < 1000000)
                    {
                        AllTestsList.MongoEntity.Add(Common.GetInsertIntoDailyStats(myServer, "CAS@RPCClient#User.Count", ps.Properties["RPC Client Access"].ToString()));

                    }
                    if (Convert.ToInt32(ps.Properties["Outlook Web App"].Value.ToString()) < 1000000)
                    {
                        AllTestsList.MongoEntity.Add(Common.GetInsertIntoDailyStats(myServer, "CAS@OWAClient#User.Count", ps.Properties["Outlook Web App"].ToString()));
                    }
					server.OWAUsers = long.Parse(ps.Properties["Outlook Web App"].Value.ToString());
					server.RPCClientAccesUsers = long.Parse(ps.Properties["RPC Client Access"].Value.ToString());
				}
			}
			catch (Exception ex)
			{
				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "Error in GetActiveConnections: " + ex.Message, commonEnums.ServerRoles.Windows, Common.LogLevel.Normal);
			}
			finally
			{
				// Finally dispose the powershell and set all variables to null to free
				// up any resources.
				//powershell.Runspace.Close();
				//powershell.Runspace.Dispose();
				//powershell.Dispose();
				//powershell = null;
			}
		}

		private void GetPhysicalMemory(PSCredential creds, MonitoredItems.MicrosoftServer myServer, TestResults AllTestsList, ReturnPowerShellObjects PSO)
		{
			Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "In GetPhysicalMemory. ", commonEnums.ServerRoles.Windows, Common.LogLevel.Normal);
			try
			{
				GetWMIPowerShell(ref PSO, creds, myServer.IPAddress, "EX_PhysicalMemory.ps1", true);
				CommonDB DB = new CommonDB();

				Collection<PSObject> results = PSO.PS.Invoke();
				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "EX_PhysicalMemory output results: " + results.Count.ToString(), commonEnums.ServerRoles.Windows, Common.LogLevel.Normal);
				foreach (PSObject ps in results)
				{
					double t = double.Parse(ps.Properties["PercentMemoryFree"].Value.ToString()) / 100;
					double ActualVal = Math.Round(double.Parse(ps.Properties["PercentMemoryFree"].Value.ToString()) / 100, 2);
					double percentUsed = Math.Round(1 - ActualVal, 2);
					DateTime dtNow = DateTime.Now;
					int weekNumber = culture.Calendar.GetWeekOfYear(dtNow, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday);

                    AllTestsList.MongoEntity.Add(Common.GetInsertIntoDailyStats(myServer, "Mem.PercentAvailable", (ActualVal * 100).ToString()));
                    
                    MongoStatementsUpdate<VSNext.Mongo.Entities.Status> mongoUpdate = new MongoStatementsUpdate<VSNext.Mongo.Entities.Status>();
                    mongoUpdate.filterDef = mongoUpdate.repo.Filter.Where(i => i.TypeAndName == myServer.TypeANDName);
                    mongoUpdate.updateDef = mongoUpdate.repo.Updater.Set(i => i.Memory, percentUsed);
                    AllTestsList.MongoEntity.Add(mongoUpdate);

					//AllTestsList.StatusDetails.Add(new TestList() { Details = ps.Properties["PercentMemoryFree"].Value.ToString() + "% free at " + System.DateTime.Now.ToShortTimeString(), TestName = "Memory", Category = commonEnums.ServerRoles.Windows, Result = commonEnums.ServerResult.Pass });

					string alertMessage = "The Memory is at " + (ActualVal*100).ToString() + "% and the threshold is set at " + myServer.Memory_Threshold.ToString() + "%";
					Common.makeAlert((ActualVal * 100), myServer.Memory_Threshold, myServer, commonEnums.AlertType.Memory, ref AllTestsList, alertMessage, "Windows");
				}

			}
			catch (Exception ex)
			{
				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "Error in GetPhysicalMemory: " + ex.Message, commonEnums.ServerRoles.Windows, Common.LogLevel.Normal);
				//AllTestsList.StatusDetails.Add(new TestList() { Details = "Failed at " + System.DateTime.Now.ToShortTimeString(), TestName = "Memory", Category = commonEnums.ServerRoles.Windows, Result = commonEnums.ServerResult.Fail });
				//Common.makeAlert(false, myServer, commonEnums.AlertType.Memory, ref AllTestsList, "Failed at " + System.DateTime.Now.ToShortTimeString(), "Windows"); 
			}
			finally
			{
				// Finally dispose the powershell and set all variables to null to free
				// up any resources.
				//powershell.Runspace.Close();
				//powershell.Runspace.Dispose();
				//powershell.Dispose();
				//powershell = null;
			}
		}

		private void GetCPU(PSCredential creds, MonitoredItems.MicrosoftServer myServer, TestResults AllTestsList, ReturnPowerShellObjects PSO)
		{
			Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "In GetCPU.", commonEnums.ServerRoles.Windows, Common.LogLevel.Normal);

			try
			{
				GetWMIPowerShell(ref PSO, creds, myServer.IPAddress, "EX_CPU.ps1", true);
				CommonDB DB = new CommonDB();
				Collection<PSObject> results = PSO.PS.Invoke();
				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "EX_CPU output results: " + results.Count.ToString(), commonEnums.ServerRoles.Windows, Common.LogLevel.Normal);

				string s = "";

				foreach (PSObject ps in results)
				{
					DateTime dtNow = DateTime.Now;
					int weekNumber = culture.Calendar.GetWeekOfYear(dtNow, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday);
					double ActualVal = Math.Round(double.Parse(ps.Properties["Average"].Value.ToString()) / 100, 2);

                    AllTestsList.MongoEntity.Add(Common.GetInsertIntoDailyStats(myServer, "Platform.System.PctCombinedCpuUtil", (ActualVal * 100).ToString()));

                    MongoStatementsUpdate<VSNext.Mongo.Entities.Status> mongoUpdate = new MongoStatementsUpdate<VSNext.Mongo.Entities.Status>();
                    mongoUpdate.filterDef = mongoUpdate.repo.Filter.Where(i => i.TypeAndName == myServer.TypeANDName);
                    mongoUpdate.updateDef = mongoUpdate.repo.Updater
                        .Set(i => i.CPU, ActualVal)
                        .Set(i => i.CPUthreshold, myServer.CPU_Threshold);
                    AllTestsList.MongoEntity.Add(mongoUpdate);

					//AllTestsList.StatusDetails.Add(new TestList() { Details = cpuLevel + "% at " + System.DateTime.Now.ToShortTimeString(), TestName = "CPU", Category = commonEnums.ServerRoles.Windows, Result = commonEnums.ServerResult.Pass });

					//Alert call, Mukund 03Apr14
					string alertMessage = "The CPU is at " + (ActualVal * 100).ToString() + "% and the threshold is set at " + myServer.CPU_Threshold.ToString() + "%";
					Common.makeAlert(ActualVal * 100, myServer.CPU_Threshold, myServer, commonEnums.AlertType.CPU, ref AllTestsList, alertMessage, "Windows");
				}

			}
			catch (Exception ex)
			{
				//AllTestsList.StatusDetails.Add(new TestList() { Details = "Failed at " + System.DateTime.Now.ToShortTimeString(), TestName = "CPU", Category = commonEnums.ServerRoles.Windows, Result = commonEnums.ServerResult.Fail });
				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "Error in GetCPU: " + ex.Message, commonEnums.ServerRoles.Windows, Common.LogLevel.Normal);
				//Common.makeAlert(false, myServer, commonEnums.AlertType.CPU, ref AllTestsList, "Failed at " + System.DateTime.Now.ToShortTimeString(), "Windows");
			}
			finally
			{
				// Finally dispose the powershell and set all variables to null to free up any resources.
				//powershell.Runspace.Close();
				//powershell.Runspace.Dispose();
				//powershell.Dispose();
				//powershell = null;
			}
		}

        private void GetDiskSpace(PSCredential creds, MonitoredItems.MicrosoftServer myServer, TestResults AllTestsList, ReturnPowerShellObjects PSO)
        {
            Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "In GetDiskSpace.", commonEnums.ServerRoles.Windows, Common.LogLevel.Normal);
            CommonDB DB = new CommonDB();
            DataTable dt = null;
            String AllDrives = "AllDisks";
            Boolean monitorAllSameLevel = false;
            String DrivesOverTheLimit = "";
            String getall="";
            string ThresholdValue = "";

            List<VSNext.Mongo.Entities.DiskStatus> listOfDisks = new List<VSNext.Mongo.Entities.DiskStatus>();

            try
            {
                string drivesSqlStm = "SELECT s.ServerName, ds.DiskName, ds.Threshold, ds.ThresholdType FROM DiskSettings ds, Servers s WHERE s.ServerName = '" + myServer.Name + "' and s.Id = ds.ServerID";
                dt = DB.GetData(drivesSqlStm);
                if (dt.Rows.Count != 0 && dt.Rows[0]["DiskName"].ToString().ToLower() == AllDrives.ToLower())
                    monitorAllSameLevel = true;
            }
            catch (Exception ex)
            {
                Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "Error in GetDiskSpace while getting drives: " + ex.Message, commonEnums.ServerRoles.Windows, Common.LogLevel.Normal);
            }
            try
            {
                GetWMIPowerShell(ref PSO, creds, myServer.IPAddress, "EX_DiskInfo.ps1", true);

                Collection<PSObject> results = PSO.PS.Invoke();
                Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "EX_DiskInfo output results: " + results.Count.ToString(), commonEnums.ServerRoles.Windows, Common.LogLevel.Normal);
                foreach (PSObject ps in results)
                {

                    try
                    {
                        string thresholdType = "";
                        string thresholdLevel = "";

                        try
                        {
                            if (monitorAllSameLevel)
                            {
                                thresholdLevel = dt.Rows[0]["Threshold"].ToString();
                                thresholdType = dt.Rows[0]["ThresholdType"].ToString();
                            }
                            else
                            {
                                foreach (DataRow row in dt.Rows)
                                {
                                    if (row["DiskName"].ToString() == ps.Properties["DeviceID"].Value.ToString())
                                    {
                                        thresholdLevel = row["Threshold"].ToString();
                                        thresholdType = row["ThresholdType"].ToString();
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "Error in GetDiskSpace getting thresholds.", commonEnums.ServerRoles.Windows, Common.LogLevel.Normal);
                            throw ex;
                        }
                        try
                        {
                            
                            listOfDisks.Add(new VSNext.Mongo.Entities.DiskStatus()
                            {
                                DiskName = ps.Properties["DeviceID"].Value.ToString(),
                                DiskFree = Math.Round(Double.Parse(ps.Properties["FreeSpace"].Value.ToString()), 2),
                                DiskSize = Math.Round(Double.Parse(ps.Properties["Size"].Value.ToString()), 2),
                                PercentFree = Math.Round(Double.Parse(ps.Properties["PercentFree"].Value.ToString()), 2)
                            });


                            AllTestsList.MongoEntity.Add(Common.GetInsertIntoDailyStats(myServer, "Disk." + ps.Properties["DeviceID"].Value.ToString() + ".Free", Math.Round(Double.Parse(ps.Properties["FreeSpace"].Value.ToString()) * 1024 * 1024 * 1024, 0).ToString()));

                            /////////////////////////////ADD IN DISKSPACE TO STATUS OR SERVERS COLLECTION. NOT DECIDED WHERE///////////////////////////////////////
                        }

                        catch (Exception ex)
                        {
                            Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "Error in GetDiskSpace during sql inserts.", commonEnums.ServerRoles.Windows, Common.LogLevel.Normal);
                            throw ex;
                        }

                        try
                        {
                            //Alert call, Mukund 03Apr14
                            double ActualVal;

                            if (!(thresholdType == "" || thresholdLevel == ""))
                            {
                                if (thresholdType == "Percent")
                                {
                                    ActualVal = double.Parse(ps.Properties["PercentFree"].Value.ToString()) * 100;
                                    if (thresholdLevel != "")
                                        thresholdLevel = (double.Parse(thresholdLevel)).ToString();


                                }
                                else
                                {
                                    ActualVal = double.Parse(ps.Properties["FreeSpace"].Value.ToString());
                                }

                                string thresholdTypeSymbol = (dt.Rows[0]["ThresholdType"].ToString() == "Percent" ? "%" : "GB");
                                bool resetAlert = true;
                                if (ActualVal < double.Parse(thresholdLevel))
                                {
                                    resetAlert = false;
                                    //Common.makeAlert(false, myServer, "Disk Space " + ps.Properties["DeviceID"].Value.ToString(), ref AllTestsList, "The server " + myServer.Name + " has " + ActualVal + "" + thresholdTypeSymbol + " available space on drive " + ps.Properties["DeviceID"].Value.ToString() + ". The threshold is " + dt.Rows[0]["Threshold"].ToString() + "" + thresholdTypeSymbol + ".", "Windows");

                                    //DrivesOverTheLimit += ps.Properties["DeviceID"].Value.ToString() + ",";                                   
                                    //getall += "Available space on drive "+ps.Properties["DeviceID"].Value.ToString()+" is "+ps.Properties["FreeSpace"].Value.ToString()+".Total space is "+ps.Properties["Size"].Value.ToString() +" GB. The Threshold is  " + dt.Rows[0]["Threshold"].ToString() + " " + dt.Rows[0]["ThresholdType"].ToString() +".";
                                    //ThresholdValue = (double.Parse(ps.Properties["Size"].Value.ToString()) * (double.Parse(thresholdLevel))).ToString();
                                    //Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "Drive " + ps.Properties["DeviceID"].Value.ToString() + " is marked for monitoring with a threshold of " + thresholdLevel + " " + thresholdType + ".  Current level is " + ActualVal, commonEnums.ServerRoles.Windows, Common.LogLevel.Normal);
                                    //Common.makeAlert(ActualVal, double.Parse(thresholdLevel), myServer, commonEnums.AlertType.Disk_Space, ref AllTestsList, "Windows");
                                }
                                else
                                {
                                    resetAlert = true;
                                    //Common.makeAlert(true, myServer, "Disk Space " + ps.Properties["DeviceID"].Value.ToString(), ref AllTestsList, "The drive " + ps.Properties["DeviceID"].Value.ToString() + " is within the threshold.", "Windows");
                                }
                                Common.makeAlert(resetAlert, myServer, "Disk Space " + ps.Properties["DeviceID"].Value.ToString(), ref AllTestsList, "The server " + myServer.Name + " has " + ActualVal + "" + thresholdTypeSymbol + " available space on drive " + ps.Properties["DeviceID"].Value.ToString() + ". The threshold is " + dt.Rows[0]["Threshold"].ToString() + "" + thresholdTypeSymbol + ".", "Windows");

                            }
                            else
                            {
                                Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "Drive " + ps.Properties["DeviceID"].Value.ToString() + " is not marked for monitoring", commonEnums.ServerRoles.Windows, Common.LogLevel.Normal);
                            }
                        }
                        catch (Exception ex)
                        {
                            Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "Error in GetDiskSpace during Alerting.", commonEnums.ServerRoles.Windows, Common.LogLevel.Normal);
                            throw ex;
                        }
                    }
                    catch (Exception ex)
                    {
                        Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "Error in GetDiskSpace parsing PS output: " + ps.BaseObject.ToString() + ".  Error: " + ex.Message, commonEnums.ServerRoles.Windows, Common.LogLevel.Normal);
                    }
                  
                }

                MongoStatementsUpdate<VSNext.Mongo.Entities.Status> mongoUpdate = new MongoStatementsUpdate<VSNext.Mongo.Entities.Status>();
                mongoUpdate.filterDef = mongoUpdate.repo.Filter.Where(i => i.DeviceName == myServer.Name);
                mongoUpdate.updateDef = mongoUpdate.repo.Updater.Set(i => i.Disks, listOfDisks);


            }
            catch (Exception ex)
            {
                Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "Error in GetDiskSpace: " + ex.Message, commonEnums.ServerRoles.Windows, Common.LogLevel.Normal);
            }
            finally
            {

            }

        }
		private void GetWMIPowerShell(ref ReturnPowerShellObjects PSO, PSCredential creds, string IPAddress, String scriptName, bool isPowershellFile)
		{
			string scriptToExecute;
			if (isPowershellFile)
			{
				System.IO.StreamReader scriptStream = new System.IO.StreamReader(AppDomain.CurrentDomain.BaseDirectory.ToString() + "Scripts\\" + scriptName);
				scriptToExecute = scriptStream.ReadToEnd();
			}
			else
			{
				scriptToExecute = scriptName;
			}

			PSO.PS.Commands.Clear();
			PSCommand cmd = new PSCommand();
			
			

			String script = @"$a = Invoke-Command -Session $sess -ScriptBlock {" + scriptToExecute + "}" + System.Environment.NewLine +
							@"echo $a";

			//string s = PSO.Session.Id.ToString();
			cmd.AddScript(@"$sess = (Get-PSSession)[0]");
			cmd.AddScript(script);

			PSO.PS.Commands = cmd;

		}	

		private void GetComputerVersionAndDays(PSCredential creds, MonitoredItems.MicrosoftServer myServer, TestResults AllTestsList, ReturnPowerShellObjects PSO)
		{
			Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "In GetComputerVersion.", commonEnums.ServerRoles.Windows, Common.LogLevel.Normal);

			try
			{
				GetWMIPowerShell(ref PSO, creds, myServer.IPAddress, "GetComputerVersionAndUpTime.ps1", true);
				CommonDB DB = new CommonDB();

				Collection<PSObject> results = PSO.PS.Invoke();
				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "GetComputerVersion output results: " + results.Count.ToString(), commonEnums.ServerRoles.Windows, Common.LogLevel.Normal);

				foreach (PSObject ps in results)
				{

					string OS = ps.Properties["ComputerVersion"].Value.ToString();
					string days = ps.Properties["UpDays"].Value.ToString();

					myServer.OperatingSystem = OS;

                    MongoStatementsUpdate<VSNext.Mongo.Entities.Status> mongoUpdate = new MongoStatementsUpdate<VSNext.Mongo.Entities.Status>();
                    mongoUpdate.filterDef = mongoUpdate.repo.Filter.Where(i => i.TypeAndName == myServer.TypeANDName);
                    mongoUpdate.updateDef = mongoUpdate.repo.Updater.Set(i => i.ElapsedDays, int.Parse(days));
                    AllTestsList.MongoEntity.Add(mongoUpdate);


					if (Convert.ToInt32(myServer.ServerDaysAlert) > 0 && Convert.ToInt32(myServer.ServerDaysAlert) < Convert.ToInt32(days))
					{
						Common.makeAlert(false, myServer, commonEnums.AlertType.Reboot_Overdue, ref AllTestsList, " This server is due for a reboot", "Windows");
					}
					else
					{
						Common.makeAlert(true, myServer, commonEnums.AlertType.Reboot_Overdue, ref AllTestsList, "This server is not due for a reboot", "Windows");
					}

				}

			}
			catch (Exception ex)
			{
				//AllTestsList.StatusDetails.Add(new TestList() { Details = "Failed at " + System.DateTime.Now.ToShortTimeString(), TestName = "CPU", Category = commonEnums.ServerRoles.Windows, Result = commonEnums.ServerResult.Fail });
				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "Error in GetWindowsVersion: " + ex.Message, commonEnums.ServerRoles.Windows, Common.LogLevel.Normal);
				//Common.makeAlert(false, myServer, commonEnums.AlertType.Reboot_Overdue, ref AllTestsList, "Failed at " + System.DateTime.Now.ToShortTimeString(), "Windows");
			}
			finally
			{
				// Finally dispose the powershell and set all variables to null to free up any resources.
				//powershell.Runspace.Close();
				//powershell.Runspace.Dispose();
				//powershell.Dispose();
				//powershell = null;
			}
		}

		private void getResponseTime(MonitoredItems.MicrosoftServer myServer, TestResults AllTestsList, ReturnPowerShellObjects PSO)
		{

			Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "In getResponseTime ", commonEnums.ServerRoles.Windows, Common.LogLevel.Normal);
			try
			{

				String str = "$server = '" + myServer.IPAddress.Replace("https://", "").Replace("http://", "") + "' \n" + new System.IO.StreamReader(AppDomain.CurrentDomain.BaseDirectory.ToString() + "Scripts\\" + "EX_Test-ResponseTime.ps1").ReadToEnd();
				PSO.PS.Streams.Error.Clear();
				PSO.PS.AddScript(str);
				long start = DateTime.Now.Ticks;
				Collection<PSObject> results = PSO.PS.Invoke();
				if (PSO.PS.Streams.Error.Count > 51)
				{
					foreach (ErrorRecord er in PSO.PS.Streams.Error)
						Console.WriteLine(er.ErrorDetails);
					//AllTestsList.StatusDetails.Add(new TestList() { TestName = "Services", Details = "getResponseTime Errors:Error count>51 at " + System.DateTime.Now.ToShortTimeString(), Result = commonEnums.ServerResult.Fail });
					myServer.ResponseTime = 0;
					Common.makeAlert(false, myServer, commonEnums.AlertType.Response_Time, ref AllTestsList, "getResponseTime Errors:Error count>51 at " + System.DateTime.Now.ToShortTimeString(), "Windows");
				}
				else
				{
					Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "getResponseTime output results: " + results.Count.ToString(), commonEnums.ServerRoles.Windows, Common.LogLevel.Normal);
					foreach (PSObject ps in results)
					{
						string Average = ps.Properties["Average"].Value == null ? "" : ps.Properties["Average"].Value.ToString();
						myServer.ResponseTime = Math.Ceiling(Convert.ToDouble(Average));
						if (myServer.ResponseTime == 0)
							myServer.ResponseTime = 1;
						long elapsedTime = DateTime.Now.Ticks - start;

						DateTime dtNow = DateTime.Now;
						int weekNumber = culture.Calendar.GetWeekOfYear(dtNow, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday);

                        AllTestsList.MongoEntity.Add(Common.GetInsertIntoDailyStats(myServer, "ResponseTime", myServer.ResponseTime.ToString()));

						Common.makeAlert(myServer.ResponseTime, myServer.ResponseThreshold, myServer, commonEnums.AlertType.Response_Time, ref AllTestsList, "Windows");
						Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "getResponseTime returned a time of " + myServer.ResponseTime.ToString() + " ms", commonEnums.ServerRoles.Windows, Common.LogLevel.Normal);
					}
				}
			}
			catch (Exception ex)
			{
				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "Error in getResponseTime: " + ex.Message, commonEnums.ServerRoles.Windows, Common.LogLevel.Normal);
				myServer.ResponseTime = 0;
			}
			finally
			{
			}
		}

		private void WaitForWinThread(Thread winThread, MonitoredItems.MicrosoftServer Server)
		{
			winThread.CurrentCulture = Thread.CurrentThread.CurrentCulture;
			winThread.Start();
			if (!winThread.Join(TimeSpan.FromSeconds(60)))
			{
				//If the thread takes longer than 60 seconds
				Common.WriteDeviceHistoryEntry(Server.ServerType, Server.Name, winThread.Name + " Thread is hung.  Will now abort the thread and continue.", commonEnums.ServerRoles.Windows, Common.LogLevel.Normal);
				winThread.Abort();
			}
			
		}
		private void GetEventLog(PSCredential creds, MonitoredItems.MicrosoftServer myServer, TestResults AllTestsList, ReturnPowerShellObjects PSO)
		{
			CommonDB DB = new CommonDB();
			string sSQL = "select distinct AliasName,EventName,ELSM.EventId EventId,EventKey,EventLevel,Source,TaskCategory  from ELSDetail ELSD,Servers S,ELSMaster ELSM where S.ID=ELSD.ServerId and ELSD.ELSId=ELSM.ID and S.ServerName='" + myServer.Name + "'";
			DataTable dtServers = DB.GetData(sSQL.ToString());
			//Loop through servers
			string logScript = "";
			if (dtServers.Rows.Count > 0)
			{
				for (int i = 0; i < dtServers.Rows.Count; i++)
				{
					DataRow DR = dtServers.Rows[i];
					logScript += Environment.NewLine;
					logScript += "Get-EventLog -After (Get-Date).AddMinutes(-" + myServer.ScanInterval.ToString() + ")";
					//logScript += "Get-EventLog -After (Get-Date).AddMinutes(-600)";

					if (DR["EventName"].ToString() != "")
						logScript += " -LogName " + DR["EventName"].ToString();
					if (DR["EventLevel"].ToString() != "")
						logScript += " -EntryType " + DR["EventLevel"].ToString();
					if (DR["EventKey"].ToString() != "")
						logScript += " -Message " + "*" + DR["EventKey"].ToString() + "*";
					if (DR["Source"].ToString() != "")
						logScript += " -Source " + DR["Source"].ToString();
					if (DR["EventId"].ToString() != "")
						logScript += " -InstanceId " + DR["EventId"].ToString();

					logScript += "| select  @{Name='AliasName';Expression={'" + DR["AliasName"].ToString() + "'}} ,  @{Name='LogName';Expression={'" + DR["EventName"].ToString() + "'}}, Index,TimeGenerated,EntryType,Source,InstanceId,Message";
				}
			}
			Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "In GetEventLog.", commonEnums.ServerRoles.Windows, Common.LogLevel.Normal);
			Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "In GetEventLog. logScript" + logScript, commonEnums.ServerRoles.Windows, Common.LogLevel.Normal);

			try
			{
				GetWMIPowerShell(ref PSO, creds, myServer.IPAddress, logScript, false);
				

				Collection<PSObject> results = PSO.PS.Invoke();
				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "GetComputerVersion output results: " + results.Count.ToString(), commonEnums.ServerRoles.Windows, Common.LogLevel.Normal);

				if (results.Count == 1 && results[0] == null)
				{
					Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "GetEventLog output NO results: " + results.Count.ToString(), commonEnums.ServerRoles.Windows, Common.LogLevel.Normal);
				}
				else
				{
					foreach (PSObject ps in results)
					{
						if (ps != null)
						{
							string indx = ps.Properties["Index"].Value.ToString();
							string aliasName = ps.Properties["AliasName"].Value.ToString();
							string LogName = ps.Properties["LogName"].Value.ToString();
							string time = ps.Properties["TimeGenerated"].Value.ToString();
							string entryType = ps.Properties["EntryType"].Value.ToString();
							string source = ps.Properties["Source"].Value.ToString();
							string instanceId = ps.Properties["InstanceId"].Value.ToString();
							string msg = ps.Properties["Message"].Value.ToString();
							msg = msg.Replace("'", "`");
							if (msg.Length > 1999)
								msg = msg.Substring(0, 1999);
							string sql = "INSERT INTO EventHistory(AliasName,LogName,IndexNo,EventTime,EntryType,Source,InstanceId,MessageDetails,DeviceName,DeviceType,LastUpdated) values('" + aliasName + "','" + LogName +"',"+ indx + ",'" + time + "','" + entryType + "','" + source + "'," + instanceId + ",'" + msg + "','" + myServer.Name + "','" + myServer.ServerType + "',getdate())";

							AllTestsList.SQLStatements.Add(new SQLstatements() { SQL = sql, DatabaseName = "VitalSigns" });

						}
						

					}
				}

			}
			catch (Exception ex)
			{
				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "Error in GetEventLog: " + ex.Message, commonEnums.ServerRoles.Windows, Common.LogLevel.Normal);
			}
			finally
			{
			}
		}
	}
}
