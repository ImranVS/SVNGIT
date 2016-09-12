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
using System.Net;
using System.Net.Http;
using System.Web;
using System.Text.RegularExpressions;
using MongoDB.Driver;
namespace VitalSignsMicrosoftClasses
{
    class ExchangeCAS : IServerRole
    {
        public string vbCrLf = System.Environment.NewLine;
		//string sqlcon = "";
		//SqlConnection con;
        VSFramework.VSAdaptor myAdapter = new VSFramework.VSAdaptor();
        VSFramework.XMLOperation myxmlAdapter = new VSFramework.XMLOperation();
		CultureInfo culture = CultureInfo.CurrentCulture;
        public ExchangeCAS()
        {
			//InitializeComponent();
			//sqlcon = myxmlAdapter.GetDBConnectionString("VitalSigns");
			//con = new SqlConnection(sqlcon);
        }

        public void CheckServer(MonitoredItems.ExchangeServer myServer, ReturnPowerShellObjects powerShellObjects, ref TestResults AllTestResults)
        {
			Common.WriteDeviceHistoryEntry("Exchange", myServer.Name, "In CAS.", commonEnums.ServerRoles.CAS, Common.LogLevel.Normal);
			Common.WriteDeviceHistoryEntry("Exchange", myServer.Name, "In CAS. Runspace Instance ID" + powerShellObjects.PS.Runspace.InstanceId.ToString(), commonEnums.ServerRoles.CAS, Common.LogLevel.Normal);
			

			if (myServer.CASAutoDiscovery)
			{
				try
				{
					TestAutoDiscovery(myServer, ref AllTestResults);
				}
				catch (Exception ex)
				{
					Common.WriteDeviceHistoryEntry("Exchange", myServer.Name, "Attempting to RUN SMTP services. Error:" + ex.StackTrace.ToString() + ex.Message.ToString());
				}
			}
			else
			{
				Common.WriteDeviceHistoryEntry("Exchange", myServer.Name, "Auto discovery is not enabled so it will not be tested");
			}

			if (myServer.CASSmtp)
			{
				try
				{
					TestSMTP(myServer, ref AllTestResults);
				}
				catch (Exception ex)
				{
					Common.WriteDeviceHistoryEntry("Exchange", myServer.Name, "Attempting to RUN SMTP services. Error:" + ex.StackTrace.ToString() + ex.Message.ToString(), commonEnums.ServerRoles.CAS);
				}
			}
			else
			{
				Common.WriteDeviceHistoryEntry("Exchange", myServer.Name, "SMTP is not enabled so it will not be tested", commonEnums.ServerRoles.CAS);
			}

			if (myServer.CASPop3)
			{
				try
				{
					TestPOP(myServer, ref AllTestResults);
				}
				catch (Exception ex)
				{
					Common.WriteDeviceHistoryEntry("Exchange", myServer.Name, "Attempting to RUN POP services. Error:" + ex.StackTrace.ToString() + ex.Message.ToString(), commonEnums.ServerRoles.CAS);
				}
			}
			else
			{
				Common.WriteDeviceHistoryEntry("Exchange", myServer.Name, "POP3 is not enabled so it will not be tested", commonEnums.ServerRoles.CAS);
			}

			if (myServer.CASImap)
			{
				try
				{
					TestIMAP(myServer, ref AllTestResults);
				}
				catch (Exception ex)
				{
					Common.WriteDeviceHistoryEntry("Exchange", myServer.Name, "Attempting to RUN IMAP services. Error:" + ex.StackTrace.ToString() + ex.Message.ToString(), commonEnums.ServerRoles.CAS);
				}
			}
			else
			{
				Common.WriteDeviceHistoryEntry("Exchange", myServer.Name, "IMAP is not enabled so it will not be tested", commonEnums.ServerRoles.CAS);
			}

			if (myServer.CASActiveSync)
			{
				try
				{
					TestActiveSync(powerShellObjects.PS, myServer, ref AllTestResults);
				}
				catch (Exception ex)
				{
					Common.WriteDeviceHistoryEntry("Exchange", myServer.Name, "Attempting to RUN ActiveSync services. Error:" + ex.StackTrace.ToString() + ex.Message.ToString(), commonEnums.ServerRoles.CAS);
				}
			}
			else
			{
				Common.WriteDeviceHistoryEntry("Exchange", myServer.Name, "ActiveSync is not enabled so it will not be tested", commonEnums.ServerRoles.CAS);
			}


            //GetActiveSyncDevices
			if (myServer.CASActiveSync)
			{
				try
				{
                    //This has been moved to its own thread.  ExchangeMAIN.GetExchangeDevices()
					//GetActiveSyncDevices(powerShellObjects.PS, myServer, ref AllTestResults);
				}
				catch (Exception ex)
				{
					Common.WriteDeviceHistoryEntry("Exchange", myServer.Name, "Attempting to RUN ActiveSyncDevices services. Error:" + ex.StackTrace.ToString() + ex.Message.ToString(), commonEnums.ServerRoles.CAS);
				}
			}
			else
			{
				Common.WriteDeviceHistoryEntry("Exchange", myServer.Name, "ActiveSyncDevices is not enabled so it will not be tested", commonEnums.ServerRoles.CAS);
			}

			if (myServer.CASOWA)
			{
				try
				{
					if (myServer.VersionNo=="2010")
						doOWATest(myServer, ref AllTestResults);
					else
						doOWATest2013(myServer, ref AllTestResults);
				}
				catch (Exception ex)
				{
					Common.WriteDeviceHistoryEntry("Exchange", myServer.Name, "Attempting to test OWA. Error:" + ex.StackTrace.ToString() + ex.Message.ToString(), commonEnums.ServerRoles.CAS);
				}
			}
			else
			{
				Common.WriteDeviceHistoryEntry("Exchange", myServer.Name, "OWA Test is not enabled so it will not be tested", commonEnums.ServerRoles.CAS);
			}

			if (myServer.CASOARPC)
			{
				try
				{
					TestRPC(powerShellObjects.PS, myServer, ref AllTestResults);

				}
				catch (Exception ex)
				{
					Common.WriteDeviceHistoryEntry("Exchange", myServer.Name, "Attempting to RUN RPC services. Error:" + ex.StackTrace.ToString() + ex.Message.ToString(), commonEnums.ServerRoles.CAS);
				}
			}
			else
			{
				Common.WriteDeviceHistoryEntry("Exchange", myServer.Name, "RPC is not enabled so it will not be tested", commonEnums.ServerRoles.CAS);
			}

			//Common.WriteDeviceHistoryEntry("Exchange", myServer.Name, DateTime.Now.ToString() + " Ending thread for CAS.", Common.LogLevel.Normal);
			//powerShellObjects.runspace.Close();
			//powerShellObjects.PS.Runspace.Close();
			Common.WriteDeviceHistoryEntry("Exchange", myServer.Name, "In CAS. Runspace is closing: Instance ID" + powerShellObjects.PS.Runspace.InstanceId.ToString(), commonEnums.ServerRoles.CAS, Common.LogLevel.Normal);
			Common.WriteDeviceHistoryEntry("Exchange", myServer.Name, "In CAS. Runspace is closing: Instance ID" + powerShellObjects.PS.Runspace.RunspaceStateInfo.State.ToString(), commonEnums.ServerRoles.CAS, Common.LogLevel.Normal);
        }


        private void TestSMTP(MonitoredItems.ExchangeServer myServer, ref TestResults SMTPIssueList)
        {

            long done;
            long start;
            start = DateTime.Now.Ticks;

            TimeSpan elapsed;
            myServer.ResponseDetails = "";
            string strResponse = "";
            try
            {
				Common.WriteDeviceHistoryEntry("Exchange", myServer.Name, "Attempting to contact SMTP service.", commonEnums.ServerRoles.CAS);
				Common.WriteDeviceHistoryEntry("Exchange", myServer.Name, "Using address " + myServer.IPAddress, commonEnums.ServerRoles.CAS);
                Common.WriteDeviceHistoryEntry("Exchange", myServer.Name, "Using URL " + myServer.SMTPURLs, commonEnums.ServerRoles.CAS);
				Common.WriteDeviceHistoryEntry("Exchange", myServer.Name, "Using port " + myServer.Port, commonEnums.ServerRoles.CAS);

            }
            catch (Exception ex)
            {
            }

  
               
        


            Chilkat.Socket Socket = new Chilkat.Socket();
            bool success = false;
            try
            {
                success = Socket.UnlockComponent("MZLDADSocket_OACwPK2ZlEn9");
                if ((success != true))
                {
                    myServer.ResponseDetails = "Failed to unlock component";
					Common.WriteDeviceHistoryEntry("Exchange", myServer.Name, "Failed to unlock Chilkat component", commonEnums.ServerRoles.CAS);
                }
                else
                {
					Common.WriteDeviceHistoryEntry("Exchange", myServer.Name, "Unlocked Chilkat component", commonEnums.ServerRoles.CAS);
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
                maxWaitMillisec = 60000;
                //success = Socket.Connect(myServer.IPAddress, 25, ssl, maxWaitMillisec);
                success = Socket.Connect(myServer.SMTPURLs, 25, ssl, maxWaitMillisec);
            }
            catch (Exception ex)
            {
                success = false;
				Common.WriteDeviceHistoryEntry("Exchange", myServer.Name, "Failed to connect #1 because " + ex.ToString(), commonEnums.ServerRoles.CAS);
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
					Common.WriteDeviceHistoryEntry("Exchange", myServer.Name, "Failed to connect because " + strResponse, commonEnums.ServerRoles.CAS);
                    //myServer.ResponseTime = -1;
                    //SMTPIssueList.StatusDetails.Add(new TestList() { Details = "Unable to connect to the SMTP server at " + DateTime.Now.ToString("HH:mm:ss tt") + " because " + strResponse, TestName = "SMTP", Category = commonEnums.ServerRoles.CAS, Result = commonEnums.ServerResult.Fail });
                    //Common.WriteTestResults(myServer.Name, "Client Access", "SMTP", "Fail", myServer.Description);

					//SMTPIssueList.AlertDetails.Add(new Alerting() { DeviceType = commonEnums.AlertDevice.Exchange, DeviceName = myServer.Name, AlertType = commonEnums.AlertType.SMTP, Details = "The SMTP server did not respond, detected at " + DateTime.Now.ToString(), Location = myServer.Location, ResetAlertQueue = commonEnums.ResetAlert.No });
                    Common.makeAlert(false, myServer, commonEnums.AlertType.SMTP, ref SMTPIssueList, "Unable to connect to the SMTP server due to Connection rejected ", "CAS");
                }
                else
                {
                    //  Set maximum timeouts for reading an writing (in millisec)
                    Socket.MaxReadIdleMs = 10000;
                    Common.WriteDeviceHistoryEntry("Exchange", myServer.Name, "Successfully connected to " + myServer.Name);

                    try
                    {
                        Socket.SendString("EHLO");
                        strResponse = Socket.ReceiveString();
                        done = DateTime.Now.Ticks;
                        elapsed = new TimeSpan(done - start);
                        //myServer.ResponseTime = elapsed.TotalMilliseconds;
						Common.WriteDeviceHistoryEntry("Exchange", myServer.Name, "Service answered with  " + strResponse.Trim(), commonEnums.ServerRoles.CAS);
                        myServer.ResponseDetails = "Server response: " + strResponse.Trim() + vbCrLf + vbCrLf;
                        myServer.ResponseDetails += " SMTP server connected in " + elapsed.TotalMilliseconds + " ms." + vbCrLf + "Target response is " + myServer.ResponseThreshold + " ms.";
                        // myServer.Description = " SMTP server connected in " & Microsoft.VisualBasic.Strings.Format(elapsed.TotalMilliseconds, "F1") & " ms. " & vbCrLf & "Target response is " & myServer.ResponseThreshold & " ms."
                        myServer.Description = " SMTP service connected in " + elapsed.TotalMilliseconds.ToString("F1") + " ms at " + System.DateTime.Now.ToShortTimeString();
                        //SMTPIssueList.StatusDetails.Add(new TestList() { Details = "Service answered with  " + strResponse.Trim(), TestName = "SMTP", Category = commonEnums.ServerRoles.CAS, Result = commonEnums.ServerResult.Pass });

						//SMTPIssueList.AlertDetails.Add(new Alerting() { DeviceType = commonEnums.AlertDevice.Exchange, DeviceName = myServer.Name, AlertType = commonEnums.AlertType.SMTP, Details = "The SMTP server is OK, detected at " + DateTime.Now.ToString(), Location = myServer.Location, ResetAlertQueue = commonEnums.ResetAlert.Yes });
						Common.makeAlert(true, myServer, commonEnums.AlertType.SMTP, ref SMTPIssueList, "Service answered with  " + strResponse.Trim(), "CAS");

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

        private void TestPOP(MonitoredItems.ExchangeServer myServer, ref TestResults POPIssueList)
        {

            long done;
            long start;
            start = DateTime.Now.Ticks;

            TimeSpan elapsed;
            myServer.ResponseDetails = "";
            string strResponse = "";
            try
            {
				Common.WriteDeviceHistoryEntry("Exchange", myServer.Name, "Attempting to contact POP service", commonEnums.ServerRoles.CAS);
				Common.WriteDeviceHistoryEntry("Exchange", myServer.Name, "Using address " + myServer.IPAddress, commonEnums.ServerRoles.CAS);
                Common.WriteDeviceHistoryEntry("Exchange", myServer.Name, "Using URL " + myServer.POP3URLs, commonEnums.ServerRoles.CAS);
				Common.WriteDeviceHistoryEntry("Exchange", myServer.Name, "Using port " + myServer.Port, commonEnums.ServerRoles.CAS);

            }
            catch (Exception ex)
            {
            }





            Chilkat.Socket Socket = new Chilkat.Socket();
            bool success = false;
            try
            {
                success = Socket.UnlockComponent("MZLDADSocket_OACwPK2ZlEn9");
                if ((success != true))
                {
                    myServer.ResponseDetails = "Failed to unlock component";
					Common.WriteDeviceHistoryEntry("Exchange", myServer.Name, "Failed to unlock Chilkat component", commonEnums.ServerRoles.CAS);
                }
                else
                {
					Common.WriteDeviceHistoryEntry("Exchange", myServer.Name, "Unlocked Chilkat component", commonEnums.ServerRoles.CAS);
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
                maxWaitMillisec = 60000;
                myServer.Port = 110;
                //success = Socket.Connect(myServer.IPAddress, myServer.Port, ssl, maxWaitMillisec);
                success = Socket.Connect(myServer.POP3URLs, myServer.Port, ssl, maxWaitMillisec);

            }
            catch (Exception ex)
            {
                success = false;
				Common.WriteDeviceHistoryEntry("Exchange", myServer.Name, "Failed to connect #1 because " + ex.ToString(), commonEnums.ServerRoles.CAS);
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
					Common.WriteDeviceHistoryEntry("Exchange", myServer.Name, "Failed to connect because " + strResponse, commonEnums.ServerRoles.CAS);
                    //myServer.ResponseTime = -1;
                    //POPIssueList.StatusDetails.Add(new TestList() { Details = "Unable to connect to the POP3 server.", TestName = "POP3", Category = commonEnums.ServerRoles.CAS, Result = commonEnums.ServerResult.Fail });
                    //POPIssueList.SQLStatements.Add(new SQLstatements() { SQL = "Insert into WindowsServices(ServerName,Service_Name,Status,StartMode,DisplayName,Monitored) values('aaaa','bbbb','Issue','none','xxxx',1)" });
                    //Common.WriteTestResults(myServer.Name, "Client Access", "POP3", "Fail", myServer.Description);

					//POPIssueList.AlertDetails.Add(new Alerting() { DeviceType = commonEnums.AlertDevice.Exchange, DeviceName = myServer.Name, AlertType = commonEnums.AlertType.POP, Details = "The POP server did not respond, detected at " + DateTime.Now.ToString(), Location = myServer.Location, ResetAlertQueue = commonEnums.ResetAlert.No });
                    Common.makeAlert(false, myServer, commonEnums.AlertType.POP3, ref POPIssueList, "Unable to connect to the POP3 server due to Connection rejected.", "CAS");
                }
                else
                {
                    //  Set maximum timeouts for reading an writing (in millisec)
                    Socket.MaxReadIdleMs = 10000;
					Common.WriteDeviceHistoryEntry("Exchange", myServer.Name, "Successfully connected to " + myServer.Name, commonEnums.ServerRoles.CAS);

                    try
                    {
                        Socket.SendString("noop");
                        strResponse = Socket.ReceiveString();
                        done = DateTime.Now.Ticks;
                        elapsed = new TimeSpan(done - start);
                        //myServer.ResponseTime = elapsed.TotalMilliseconds;
						Common.WriteDeviceHistoryEntry("Exchange", myServer.Name, "Service answered with  " + strResponse.Trim(), commonEnums.ServerRoles.CAS);
                        //Common.WriteTestResults(myServer.Name, "Client Access", "POP3", "Pass", "Service answered with  " + strResponse.Trim() + " at " + System.DateTime.Now.ToShortTimeString());
                        //POPIssueList.StatusDetails.Add(new TestList() { Details = "Service answered with  " + strResponse.Trim(), TestName = "POP3", Category = commonEnums.ServerRoles.CAS, Result = commonEnums.ServerResult.Pass });
						Common.WriteDeviceHistoryEntry("Exchange", myServer.Name, myServer.ResponseDetails, commonEnums.ServerRoles.CAS);

						//POPIssueList.AlertDetails.Add(new Alerting() { DeviceType = commonEnums.AlertDevice.Exchange, DeviceName = myServer.Name, AlertType = commonEnums.AlertType.POP, Details = "The POP server is OK, detected at " + DateTime.Now.ToString(), Location = myServer.Location, ResetAlertQueue = commonEnums.ResetAlert.Yes });
						Common.makeAlert(true, myServer, commonEnums.AlertType.POP3, ref POPIssueList, "Service answered with  " + strResponse.Trim(), "CAS");
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

        private void TestIMAP(MonitoredItems.ExchangeServer myServer, ref TestResults IMAPIssueList)
        {

            long done;
            long start;
            start = DateTime.Now.Ticks;

            TimeSpan elapsed;
            myServer.ResponseDetails = "";
            string strResponse = "";
            try
            {
				Common.WriteDeviceHistoryEntry("Exchange", myServer.Name, "Attempting to contact IMAP service.", commonEnums.ServerRoles.CAS);
				Common.WriteDeviceHistoryEntry("Exchange", myServer.Name, "Using address " + myServer.IPAddress, commonEnums.ServerRoles.CAS);
                Common.WriteDeviceHistoryEntry("Exchange", myServer.Name, "Using URL " + myServer.IMAPURLs, commonEnums.ServerRoles.CAS);

            }
            catch (Exception ex)
            {
            }

            myServer.Port = 143;

            Chilkat.Socket Socket = new Chilkat.Socket();
            bool success = false;
            try
            {
                success = Socket.UnlockComponent("MZLDADSocket_OACwPK2ZlEn9");
                if ((success != true))
                {
                    myServer.ResponseDetails = "Failed to unlock component";
					Common.WriteDeviceHistoryEntry("Exchange", myServer.Name, "Failed to unlock Chilkat component", commonEnums.ServerRoles.CAS);
                }
                else
                {
					Common.WriteDeviceHistoryEntry("Exchange", myServer.Name, "Unlocked Chilkat component", commonEnums.ServerRoles.CAS);
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
                maxWaitMillisec = 60000;
                //success = Socket.Connect(myServer.IPAddress, myServer.Port, ssl, maxWaitMillisec);
                success = Socket.Connect(myServer.IMAPURLs, myServer.Port, ssl, maxWaitMillisec);

            }
            catch (Exception ex)
            {
                success = false;
				Common.WriteDeviceHistoryEntry("Exchange", myServer.Name, "Failed to connect #1 because " + ex.ToString(), commonEnums.ServerRoles.CAS);
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
					Common.WriteDeviceHistoryEntry("Exchange", myServer.Name, "Failed to connect because " + strResponse, commonEnums.ServerRoles.CAS);
                    //myServer.ResponseTime = -1;
                    //IMAPIssueList.StatusDetails.Add(new TestList() { Details = "Unable to connect to the IMAP server at " + DateTime.Now.ToString("HH:mm:ss tt") + " because " + strResponse, TestName = "IMAP", Category = commonEnums.ServerRoles.CAS, Result = commonEnums.ServerResult.Fail });
                    // Common.WriteTestResults(myServer.Name, "Client Access", "IMAP", "Fail", myServer.Description);
					//myServer.ResponseTime = -1;
					//IMAPIssueList.AlertDetails.Add(new Alerting() { DeviceType = commonEnums.AlertDevice.Exchange, DeviceName = myServer.Name, AlertType = commonEnums.AlertType.IMAP, Details = "The IMAP server did not respond, detected at " + DateTime.Now.ToString(), Location = myServer.Location, ResetAlertQueue = commonEnums.ResetAlert.No });                   
                    Common.makeAlert(false, myServer, commonEnums.AlertType.IMAP, ref IMAPIssueList, "Unable to connect to the IMAP server due to Connection rejected.", "CAS");
                }
                else
                {
                    //  Set maximum timeouts for reading an writing (in millisec)
                    Socket.MaxReadIdleMs = 10000;
					Common.WriteDeviceHistoryEntry("Exchange", myServer.Name, "Successfully connected to " + myServer.Name, commonEnums.ServerRoles.CAS);

                    try
                    {
                        //Socket.SendString("EHLO");
                        strResponse = Socket.ReceiveString();
                        done = DateTime.Now.Ticks;
                        elapsed = new TimeSpan(done - start);
						Common.WriteDeviceHistoryEntry("Exchange", myServer.Name, "Service answered with  " + strResponse.Trim(), commonEnums.ServerRoles.CAS);
                        myServer.ResponseDetails = "Server response: " + strResponse.Trim() + vbCrLf + vbCrLf;
                        myServer.ResponseDetails += " IMAP server connected in " + elapsed.TotalMilliseconds + " ms." + vbCrLf + "Target response is " + myServer.ResponseThreshold + " ms.";
                        // myServer.Description = " IMAP server connected in " & Microsoft.VisualBasic.Strings.Format(elapsed.TotalMilliseconds, "F1") & " ms. " & vbCrLf & "Target response is " & myServer.ResponseThreshold & " ms."
                        myServer.Description = " IMAP service connected in " + elapsed.TotalMilliseconds.ToString("F1") + " ms at " + System.DateTime.Now.ToShortTimeString();
                        //Common.WriteTestResults(myServer.Name, "CAS", "IMAP", "Pass",  "Service answered with  " + strResponse.Trim() + " at " + System.DateTime.Now.ToShortTimeString());
                        //IMAPIssueList.StatusDetails.Add(new TestList() { Details = "Service answered with  " + strResponse.Trim(), TestName = "IMAP", Category = commonEnums.ServerRoles.CAS, Result = commonEnums.ServerResult.Pass });
						Common.WriteDeviceHistoryEntry("Exchange", myServer.Name, myServer.ResponseDetails, commonEnums.ServerRoles.CAS);

						//IMAPIssueList.AlertDetails.Add(new Alerting() { DeviceType = commonEnums.AlertDevice.Exchange, DeviceName = myServer.Name, AlertType = commonEnums.AlertType.IMAP, Details = "The IMAP server is OK, detected at " + DateTime.Now.ToString(), Location = myServer.Location, ResetAlertQueue = commonEnums.ResetAlert.Yes });
						Common.makeAlert(true, myServer, commonEnums.AlertType.IMAP, ref IMAPIssueList, "Service answered with  " + strResponse.Trim(), "CAS");

						CultureInfo culture = CultureInfo.CurrentCulture;
						//myServer.ResponseTime = elapsed.TotalMilliseconds;
						

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

        public void TestRPC(PowerShell powershell, MonitoredItems.ExchangeServer myServer, ref TestResults AllTestsList)
        {

            string servername = myServer.Name;
			Common.WriteDeviceHistoryEntry("Exchange", servername, "In TestRPC ", commonEnums.ServerRoles.CAS, Common.LogLevel.Normal);
            string FailedDatabase = "";
            string FailedError = "";

            try
            {
                System.Collections.ObjectModel.Collection<PSObject> results = new System.Collections.ObjectModel.Collection<PSObject>();
                //Change the Path to the Script to suit your needs
                String str = "Test-MAPIConnectivity -Server " + servername;
				Common.WriteDeviceHistoryEntry("Exchange", servername, "Executing script --> " + str, commonEnums.ServerRoles.CAS, Common.LogLevel.Normal);
                powershell.AddScript(str);
                results = powershell.Invoke();

                if (powershell.Streams.Error.Count > 51)
                {
                    foreach (ErrorRecord er in powershell.Streams.Error)
                        Console.WriteLine(er.ErrorDetails);
                    //AllTestsList.StatusDetails.Add(new TestList() { TestName = "RPC", Details = "TestRPC Errors:Error count>51 at " + System.DateTime.Now.ToShortTimeString(), Result = commonEnums.ServerResult.Fail });
					//AllTestsList.AlertDetails.Add(new Alerting() { DeviceType = commonEnums.AlertDevice.Exchange, DeviceName = myServer.Name, AlertType = commonEnums.AlertType.MAPI, Details = "The TestRPC connectivity has an issue, detected at " + DateTime.Now.ToString(), Location = myServer.Location, ResetAlertQueue = commonEnums.ResetAlert.Yes });
                    Common.makeAlert(false, myServer, commonEnums.AlertType.RPC, ref AllTestsList, "TestRPC Errors:Error count>51 at " + System.DateTime.Now.ToShortTimeString(), "CAS");

				}
                else
                {
					Common.WriteDeviceHistoryEntry("Exchange", servername, "TestRPC output results: " + results.Count.ToString(), commonEnums.ServerRoles.CAS, Common.LogLevel.Normal);
					bool allSuccess = true;
                    foreach (PSObject ps in results)
                    {

                        // string strScenario = ps.Properties["Scenario"].Value == null ? "" : ps.Properties["Scenario"].Value.ToString();
                        string strResult = ps.Properties["Result"].Value == null ? "" : ps.Properties["Result"].Value.ToString();
						string Database = ps.Properties["Database"].Value == null ? "" : ps.Properties["Database"].Value.ToString();
                        string Error = ps.Properties["Error"].Value == null ? "" : ps.Properties["Error"].Value.ToString();

                        if (strResult == "Success")
                        {
							//AllTestsList.StatusDetails.Add(new TestList() { Details = "Successfully Connected via RPC " + System.DateTime.Now.ToShortTimeString(), TestName = "RPC - " + Database, Category = commonEnums.ServerRoles.CAS, Result = commonEnums.ServerResult.Pass });
							//AllTestsList.AlertDetails.Add(new Alerting() { DeviceType = commonEnums.AlertDevice.Exchange, DeviceName = myServer.Name, AlertType = commonEnums.AlertType.MAPI, Details = "The MAPI connectivity is OK, detected at " + DateTime.Now.ToString(), Location = myServer.Location, ResetAlertQueue = commonEnums.ResetAlert.No });
                           // Common.makeAlert(false, myServer, commonEnums.AlertType.MAPI, ref AllTestsList, "Successfully Connected via RPC " + System.DateTime.Now.ToShortTimeString());
							//allSuccess = true;
						}
                        else
                        {
							//AllTestsList.StatusDetails.Add(new TestList() { Details = "Failed RPC connectivity " + System.DateTime.Now.ToShortTimeString(), TestName = "RPC - " + Database, Category = commonEnums.ServerRoles.CAS, Result = commonEnums.ServerResult.Fail });
							//AllTestsList.AlertDetails.Add(new Alerting() { DeviceType = commonEnums.AlertDevice.Exchange, DeviceName = myServer.Name, AlertType = commonEnums.AlertType.MAPI, Details = "The MAPI connectivity has an issue, detected at " + DateTime.Now.ToString(), Location = myServer.Location, ResetAlertQueue = commonEnums.ResetAlert.Yes });
                           // Common.makeAlert(true, myServer, commonEnums.AlertType.MAPI, ref AllTestsList, "Failed RPC connectivity " + System.DateTime.Now.ToShortTimeString());
							allSuccess = false;
                            FailedDatabase = Database;
                            Common.WriteDeviceHistoryEntry("Exchange", servername, "RPC database " + Database + " failed with error of " + Error, Common.LogLevel.Normal);
                            try
                            {
                                Error = Error.StartsWith("[") ? Error.Substring(Error.IndexOf("]:") + "]:".Length) : Error;
                                int index = Error.IndexOf(". ");
                                if (index == -1) index = Error.IndexOf(".\r");
                                if (index == -1) index = Error.IndexOf(".\n");
                                Error = Error.Substring(0, index);
                            }
                            catch (Exception ex)
                            {
                                Common.WriteDeviceHistoryEntry("Exchange", servername, "Exception parsing error: " + ex.Message, Common.LogLevel.Normal);
                            }
                            FailedError = Error;
						}
                    }
					if (results.Count > 0)
					{
						if (allSuccess)
						{
							Common.makeAlert(true, myServer, commonEnums.AlertType.RPC, ref AllTestsList, "Successfully connected to all databases via RPC connectivity ", "CAS");
                            //AllTestsList.StatusDetails.Add(new TestList() { Details = "Successfully connected to all databases via RPC connectivity at " + System.DateTime.Now.ToShortTimeString(), TestName = "RPC", Category = commonEnums.ServerRoles.CAS, Result = commonEnums.ServerResult.Pass });
						}
						else
						{
							Common.makeAlert(false, myServer, commonEnums.AlertType.RPC, ref AllTestsList, "The RPC connectivity test failed while connecting to database " + FailedDatabase + " and produced an error of \"" + FailedError + "\" " , "CAS");
							//AllTestsList.StatusDetails.Add(new TestList() { Details = "One or more RPC connectivities failed while connecting to databases at " + System.DateTime.Now.ToShortTimeString(), TestName = "RPC", Category = commonEnums.ServerRoles.CAS, Result = commonEnums.ServerResult.Fail });
						}
					}
					if(results.Count == 0)
					{
						Common.makeAlert(false, myServer, commonEnums.AlertType.RPC, ref AllTestsList, "RPC returned no results " , "CAS");
						//AllTestsList.StatusDetails.Add(new TestList() { Details = "RPC returned no results at " + System.DateTime.Now.ToShortTimeString(), TestName = "RPC", Category = commonEnums.ServerRoles.CAS, Result = commonEnums.ServerResult.Fail });
					}
				}

            }
            catch (Exception ex)
            {

				Common.WriteDeviceHistoryEntry("Exchange", servername, "Error in TestRPC: " + ex.Message, commonEnums.ServerRoles.CAS, Common.LogLevel.Normal);

            }
            finally
            {
                // dispose the runspace and enable garbage collection
                //runspace.Dispose();
                //runspace = null;
            }
        }

        public void TestActiveSync(PowerShell powershell, MonitoredItems.ExchangeServer myServer, ref TestResults AllTestsList)
        {
            string servername = myServer.Name;
			bool overallActiveSyncPassed = true;
			Common.WriteDeviceHistoryEntry("Exchange", servername, "In TestActiveSync.", commonEnums.ServerRoles.CAS, Common.LogLevel.Normal);
            string FailedScenario = "";
            string FailedError = "";

            try
            {
                //powershell = PowerShell.Create();
                //System.IO.StreamReader scriptStream = new System.IO.StreamReader(AppDomain.CurrentDomain.BaseDirectory.ToString() + "Scripts\\" + scriptName);
                //string scriptToExecute = "Test-ActiveSyncConnectivity -ClientAccessServer " + myServer.Name + " -TrustAnySSLCertificate  -MailboxCredential $creds1 | select Scenario, Result";
				//if activesyncuserid is empty, try to revert back to regular userid and pwd
				string pwd = "";
				string userId = "";
                //if (myServer.ActiveSyncUserName.ToString() == "")
                //{
                //    userId = myServer.UserName;
                //    pwd = myServer.Password;
                //}
                //else
                //{
                //    userId = myServer.ActiveSyncUserName;
                //    pwd = myServer.ActiveSyncPassword;
                //}
                if (myServer.ActiveSyncUserName.ToString() == "")
                {
                    userId = myServer.UserName;
                    pwd = myServer.Password;
                }
                else
                {
                    userId = myServer.ActiveSyncUserName;
                    pwd = myServer.ActiveSyncPassword;
                }


				System.Security.SecureString securePassword = Common.String2SecureString(pwd);

				PSCredential creds = new PSCredential(userId, securePassword);
                powershell.AddCommand("Set-Variable");
                powershell.AddParameter("Name", "creds1");
                powershell.AddParameter("Value", creds);
                System.Collections.ObjectModel.Collection<PSObject> results = new System.Collections.ObjectModel.Collection<PSObject>();

                String str = "Test-ActiveSyncConnectivity -ClientAccessServer " + myServer.Name + " -TrustAnySSLCertificate  -MailboxCredential $creds1 | select Scenario, Result, Error";
                
                powershell.AddScript(str);
                results = powershell.Invoke();

                if (powershell.Streams.Error.Count > 51)
                {
                    foreach (ErrorRecord er in powershell.Streams.Error)
                        Console.WriteLine(er.ErrorDetails);
                    //AllTestsList.StatusDetails.Add(new TestList() { TestName="ActiveSync", Details = "TestActiveSync Errors:Error count>51 at " + System.DateTime.Now.ToShortTimeString(), Result = commonEnums.ServerResult.Fail });
					//AllTestsList.AlertDetails.Add(new Alerting() { DeviceType = commonEnums.AlertDevice.Exchange, DeviceName = myServer.Name, AlertType = commonEnums.AlertType.ActiveSync, Details = "The ActiveSync is not OK, detected at " + DateTime.Now.ToString(), Location = myServer.Location, ResetAlertQueue = commonEnums.ResetAlert.No });
					Common.makeAlert(false, myServer, commonEnums.AlertType.Active_Sync, ref AllTestsList, "ActiveSync failed to connect", "CAS");
					overallActiveSyncPassed = false;
                }
				else if (results.Count == 0)
				{
					overallActiveSyncPassed = false;
				}
				else
                {
					Common.WriteDeviceHistoryEntry("Exchange", servername, "TestActiveSync output results: " + results.Count.ToString(), commonEnums.ServerRoles.CAS, Common.LogLevel.Normal);

                    foreach (PSObject ps in results)
                    {

                        string strScenario = ps.Properties["Scenario"].Value == null ? "" : ps.Properties["Scenario"].Value.ToString();
                        string strResult = ps.Properties["Result"].Value == null ? "" : ps.Properties["Result"].Value.ToString();
                        string strError = ps.Properties["Error"].Value == null ? "" : ps.Properties["Error"].Value.ToString();

                        if (strResult == "Success")
                        {
                            //myResult = commonEnums.ServerResult.Pass;
							//AllTestsList.AlertDetails.Add(new Alerting() { DeviceType = commonEnums.AlertDevice.Exchange, DeviceName = myServer.Name, AlertType = commonEnums.AlertType.ActiveSync, Details = "The ActiveSync is OK, detected at " + DateTime.Now.ToString(), Location = myServer.Location, ResetAlertQueue = commonEnums.ResetAlert.Yes });
							//Common.makeAlert(true, myServer, commonEnums.AlertType.Active_Sync, ref AllTestsList);
                        }
                        else
                        {
                           // myResult = commonEnums.ServerResult.Fail;
							//AllTestsList.AlertDetails.Add(new Alerting() { DeviceType = commonEnums.AlertDevice.Exchange, DeviceName = myServer.Name, AlertType = commonEnums.AlertType.ActiveSync, Details = "The ActiveSync is not OK, detected at " + DateTime.Now.ToString(), Location = myServer.Location, ResetAlertQueue = commonEnums.ResetAlert.No });
							//Common.makeAlert(false, myServer, commonEnums.AlertType.Active_Sync, ref AllTestsList);
                            Common.WriteDeviceHistoryEntry("Exchange", servername, "Active Sync Scenerio " + strScenario + " failed with error of " + strError, Common.LogLevel.Normal);
							overallActiveSyncPassed = false;
                            FailedScenario = strScenario;
                            try
                            {
                                strError = strError.StartsWith("[System") ? strError.Substring(strError.IndexOf("]:") + "]:".Length) : strError;
                                int index = strError.IndexOf(". ");
                                if (index == -1) index = strError.IndexOf(".\r");
                                if (index == -1) index = strError.IndexOf(".\n");
                                strError = strError.Substring(0, index);
                            }
                            catch (Exception ex)
                            {
                                Common.WriteDeviceHistoryEntry("Exchange", servername, "Exception parsing error: " + ex.Message, Common.LogLevel.Normal);
                            }
                            FailedError = strError;

                        }

                        //AllTestsList.StatusDetails.Add(new TestList() { Details = strResult + " at " + System.DateTime.Now.ToShortTimeString(), TestName = "ActiveSync - " + strScenario, Category = commonEnums.ServerRoles.CAS, Result = myResult });
                        //Common.makeAlert(false, myServer, commonEnums.AlertType.Active_Sync, ref AllTestsList, strResult + " at " + System.DateTime.Now.ToShortTimeString());
                    }
                }

            }
            catch (Exception ex)
            {
				overallActiveSyncPassed = false;
				Common.WriteDeviceHistoryEntry("Exchange", servername, "Error in TestActiveSync: " + ex.Message, commonEnums.ServerRoles.CAS, Common.LogLevel.Normal);

            }
            finally
            {
				if (overallActiveSyncPassed)
				{
					Common.makeAlert(true, myServer, commonEnums.AlertType.Active_Sync, ref AllTestsList, "All ActiveSync tests passsed ", "CAS");
					//AllTestsList.StatusDetails.Add(new TestList() { Details = "All ActiveSync tests passsed at " + System.DateTime.Now.ToShortTimeString(), TestName = "Overall ActiveSync", Category = commonEnums.ServerRoles.CAS, Result = commonEnums.ServerResult.Pass });
				}
				else
				{
					//Common.makeAlert(false, myServer, commonEnums.AlertType.Active_Sync, ref AllTestsList, "There was an issue with one or more ActiveSync tests ", "CAS");
                    Common.makeAlert(false, myServer, commonEnums.AlertType.Active_Sync, ref AllTestsList, "The ActiveSync test failed at the " + FailedScenario + " scenerio and produced an error message of \"" + FailedError + "\" ", "CAS");
					//AllTestsList.StatusDetails.Add(new TestList() { Details = "There was an issue with one or more ActiveSync tests at " + System.DateTime.Now.ToShortTimeString(), TestName = "Overall ActiveSync", Category = commonEnums.ServerRoles.CAS, Result = commonEnums.ServerResult.Fail });
				}

                // dispose the runspace and enable garbage collection
                //runspace.Dispose();
                //runspace = null;
            }

        }

        public void TestActiveSyncOLD(ReturnPowerShellObjects pso, MonitoredItems.ExchangeServer myServer, ref TestResults ActiveSyncIssueList)
        {
            Common.WriteDeviceHistoryEntry("Exchange", myServer.Name, "Testing ActiveSync", Common.LogLevel.Normal);
            //AllTestsList.Add(new TestList() { Details = "Service answered with   at " + System.DateTime.Now.ToShortTimeString(), TestName = servername+ " " + strAction, Category = commonEnums.ServerRoles.CAS, Result = commonEnums.ServerResult.Pass });
            try
            {
                //$uname= 'alan@jnittech.com';
                //$Pwd='Vitalsigns123!';
                //$secpasswd = ConvertTo-SecureString $Pwd -AsPlainText –Force;
                //$cred=New-object -typename System.Management.Automation.PSCredential -argumentlist $uname,$secpasswd;

                //Test-ActiveSyncConnectivity -ClientAccessServer JNITTECH-EXCHG1 -TrustAnySSLCertificate  -MailboxCredential $cred 

                System.Collections.ObjectModel.Collection<PSObject> results = new System.Collections.ObjectModel.Collection<PSObject>();
                PowerShell powershell = pso.PS;

                //System.Uri uri = new Uri(myServer.IPAddress + "/powershell?serializationLevel=Full");
                System.Uri uri = new Uri(myServer.IPAddress + "/powershell?serializationLevel=Full");
                //System.Security.SecureString securePassword = Common.String2SecureString(myServer.Password);
                //PSCredential creds = new PSCredential(myServer.UserName, securePassword);
                System.Security.SecureString securePassword = Common.String2SecureString(myServer.UserName);
                PSCredential creds = new PSCredential(myServer.Password, securePassword);
                powershell.AddCommand("Set-Variable");
                powershell.AddParameter("Name", "cred");
                powershell.AddParameter("Value", creds);

                String script = "Test-ActiveSyncConnectivity -ClientAccessServer " + myServer.Name + "-TrustAnySSLCertificate  -MailboxCredential ";
                //powershell.AddScript("Test-ActiveSyncConnectivity -ClientAccessServer " + myServer.IPAddress.Replace("https://", "").Replace("http://", "") + " -TrustAnySSLCertificate  -MailboxCredential ");
                powershell.AddScript("Test-ActiveSyncConnectivity -ClientAccessServer " + myServer.IPAddress.Replace("https://", "").Replace("http://", "") + " -TrustAnySSLCertificate  -MailboxCredential ");
                //powershell.AddScript(script);

                results = powershell.Invoke();


                if (powershell.Streams.Error.Count > 51)
                {
                    foreach (ErrorRecord er in powershell.Streams.Error)
                        Console.WriteLine(er.ErrorDetails);
                    //ActiveSyncIssueList.StatusDetails.Add(new TestList() { Details = "Errors: Instance:" + oExchange + " Error count>51 at " + System.DateTime.Now.ToShortTimeString(), TestName = strAction, Category = commonEnums.ServerRoles.HUB, Result = commonEnums.ServerResult.Fail });
					Common.makeAlert(false, myServer, commonEnums.AlertType.Active_Sync, ref ActiveSyncIssueList, "Errors: Error count>51 at " + System.DateTime.Now.ToShortTimeString(), "CAS");
                }
                else
                {
                    Common.WriteDeviceHistoryEntry("Exchange", myServer.Name, "Test ActiveSync output results: " + results.Count.ToString(), Common.LogLevel.Normal);

                    foreach (PSObject ps in results)
                    {

                        string strPing = ps.Properties["Ping"].Value == null ? "" : ps.Properties["Ping"].Value.ToString();
                        string strSyncData = ps.Properties["Sync Data"].Value == null ? "" : ps.Properties["Sync Data"].Value.ToString();
                        string strGetItemEstimate = ps.Properties["GetItemEstimate"].Value == null ? "" : ps.Properties["GetItemEstimate"].Value.ToString();
                        string strFolderSync = ps.Properties["FolderSync"].Value == null ? "" : ps.Properties["FolderSync"].Value.ToString();
                        string strFirstSync = ps.Properties["First Sync"].Value == null ? "" : ps.Properties["First Sync"].Value.ToString();

                        //ActiveSyncIssueList.StatusDetails.Add(new TestList() { Details = strSyncData, TestName = "ActiveSync", Category = commonEnums.ServerRoles.HUB, Result = commonEnums.ServerResult.Pass });

                    }
                }

            }
            catch (Exception ex)
            {

				Common.WriteDeviceHistoryEntry("Exchange", myServer.Name, "Error in ActiveSync test: " + ex.Message, Common.LogLevel.Normal);

            }
            finally
            {
                // dispose the runspace and enable garbage collection
                //runspace.Dispose();
                //runspace = null;
            }
        }

        public void TestServiceHealth(MonitoredItems.ExchangeServer myServer, ref TestResults ServiceIssueList)
        {
            // Test-ServiceHealth -Server JNITTECH-EXCHG1  

            //returns

            //Server          Check                      Result     Error                                                                                                              
            //------          -----                      ------     -----                                                                                                              
            //JNITTECH-EXCHG1 ClusterService             Passed                                                                                                                        
            //JNITTECH-EXCHG1 ReplayService              Passed                                                                                                                        
            //JNITTECH-EXCHG1 ActiveManager              Passed                                                                                                                        
            //JNITTECH-EXCHG1 TasksRpcListener           Passed                                                                                                                        
            //JNITTECH-EXCHG1 TcpListener                Passed                                                                                                                        
            //JNITTECH-EXCHG1 ServerLocatorService       Passed                                                                                                                        
            //JNITTECH-EXCHG1 DagMembersUp               Passed                                                                                                                        
            //JNITTECH-EXCHG1 ClusterNetwork             Passed                                                                                                                        
            //JNITTECH-EXCHG1 QuorumGroup                Passed                                                                                                                        
            //JNITTECH-EXCHG1 FileShareQuorum            Passed                                                                                                                        
            //JNITTECH-EXCHG1 DBCopySuspended            Passed                                                                                                                        
            //JNITTECH-EXCHG1 DBCopyFailed               Passed                                                                                                                        
            //JNITTECH-EXCHG1 DBInitializing             Passed                                                                                                                        
            //JNITTECH-EXCHG1 DBDisconnected             Passed                                                                                                                        
            //JNITTECH-EXCHG1 DBLogCopyKeepingUp         Passed                                                                                                                        
            //JNITTECH-EXCHG1 DBLogReplayKeepingUp       Passed                    
        }


        public void GetActiveSyncDevices(PowerShell powershell, MonitoredItems.ExchangeServer myServer, ref TestResults AllTestsList)
        {
            string servername = myServer.Name;
			Common.WriteDeviceHistoryEntry("Exchange", servername, "In GetActiveSyncDevices.", commonEnums.ServerRoles.CAS, Common.LogLevel.Normal);
            try
            {

   
                System.Collections.ObjectModel.Collection<PSObject> results = new System.Collections.ObjectModel.Collection<PSObject>();

                //String str = "$UserList = Get-CASMailbox -Filter {hasactivesyncdevicepartnership -eq $true -and -not displayname -like 'CAS_{*'} | Get-Mailbox ";
                //str = str + vbCrLf;
                //str = str + "$UserList | foreach {Get-ActiveSyncDeviceStatistics -Mailbox $_.Name } ";

                System.IO.StreamReader scriptStream = new System.IO.StreamReader(AppDomain.CurrentDomain.BaseDirectory.ToString() + "Scripts\\" + "EX_ActiveSyncDeviceList2.ps1");
                String str = scriptStream.ReadToEnd();

				powershell.Commands.Clear();
				powershell.Streams.ClearStreams();
                powershell.AddScript(str);
                results = powershell.Invoke();

				foreach (ErrorRecord er in powershell.Streams.Error)
					Common.WriteDeviceHistoryEntry("Exchange", servername, "Error executing EX_ActiveSyncDeviceList2.ps1: " + er.Exception.ToString(), commonEnums.ServerRoles.CAS, Common.LogLevel.Normal);
                
				if (powershell.Streams.Error.Count > 0)
                {
					Common.makeAlert(false, myServer, commonEnums.AlertType.Active_Sync_Devices, ref AllTestsList, "There were errors gathering the ActivSync Devices. Please view the log file in the VitalSignsPlus folder at Log_Files/Microsoft/Exchange/Exchange_" + servername + "/Exchange_" + servername + "_CAS.txt for more info ", "CAS");
                }
                else
                {
					Common.WriteDeviceHistoryEntry("Exchange", servername, "GetActiveSyncDevices output results: " + results.Count.ToString(), commonEnums.ServerRoles.CAS, Common.LogLevel.Normal);
                    string strSQL = "";
                    string strSQLValues = "";
                    foreach (PSObject ps in results)
                    {
                        if (ps.Properties["DeviceType"].Value != null && ps.Properties["DeviceType"].Value.ToString() == "TestActiveSyncConnectivity")
                        {
                            Common.WriteDeviceHistoryEntry("Exchange", servername, "GetActiveSyncDevices. Device has a type of TestActiveSyncConnectivity so it will not be added.", commonEnums.ServerRoles.CAS, Common.LogLevel.Verbose);
                            continue;
                        }
                        ActiveSyncDevice myDevice = new ActiveSyncDevice() ;
                        myDevice.User = ps.Properties["User"].Value == null ? "" : ps.Properties["User"].Value.ToString();
                        myDevice.DeviceID = ps.Properties["DeviceID"].Value == null ? "" : ps.Properties["DeviceID"].Value.ToString();
                        myDevice.DeviceMobileOperator = ps.Properties["DeviceMobileOperator"].Value == null ? "" : ps.Properties["DeviceMobileOperator"].Value.ToString();
                        myDevice.DeviceActiveSyncVersion = ps.Properties["DeviceActiveSyncVersion"].Value == null ? "" : ps.Properties["DeviceActiveSyncVersion"].Value.ToString();
                        myDevice.DeviceFriendlyName = ps.Properties["DeviceFriendlyName"].Value == null ? "" : ps.Properties["DeviceFriendlyName"].Value.ToString();
                        myDevice.DeviceOSLanguage = ps.Properties["DeviceOSLanguage"].Value == null ? "" : ps.Properties["DeviceOSLanguage"].Value.ToString();
                        myDevice.DeviceModel = ps.Properties["DeviceModel"].Value == null ? "" : ps.Properties["DeviceModel"].Value.ToString();
                        myDevice.DeviceType = ps.Properties["DeviceModel"].Value == null ? "" : ps.Properties["DeviceType"].Value.ToString();
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

					   Common.WriteDeviceHistoryEntry("Exchange", servername, "Identity: " + myDevice.Identity, commonEnums.ServerRoles.CAS);
					   Common.WriteDeviceHistoryEntry("Exchange", servername, "Device OS : " + myDevice.DeviceOS, commonEnums.ServerRoles.CAS);
					   Common.WriteDeviceHistoryEntry("Exchange", servername, "DeviceUserAgent: " + myDevice.DeviceUserAgent, commonEnums.ServerRoles.CAS);

                        MongoStatementsUpsert<VSNext.Mongo.Entities.MobileDevices> mongoStatement = new MongoStatementsUpsert<VSNext.Mongo.Entities.MobileDevices>();
                        mongoStatement.filterDef = mongoStatement.repo.Filter.Where(i => i.DeviceID == myDevice.DeviceID && i.ServerName == "Exchange");
                        mongoStatement.updateDef = mongoStatement.repo.Updater
                            .Set(i => i.UserName, myDevice.User)
                            .Set(i => i.SecurityPolicy, myDevice.DevicePolicyApplied)
                            .Set(i => i.DeviceName, myDevice.DeviceFriendlyName)
                            .Set(i => i.ConnectionState, myDevice.Status)
                            .Set(i => i.LastSyncTime, myDevice.LastSuccessSync == "" ? null : (DateTime?) DateTime.Parse(myDevice.LastSuccessSync))
                            .Set(i => i.OSType, myDevice.DeviceOS)
                            .Set(i => i.ClientBuild, myDevice.DeviceActiveSyncVersion)
                            .Set(i => i.DeviceType, myDevice.DeviceType)
                            .Set(i => i.Access, myDevice.DeviceAccessState)
                            .Set(i => i.IsActive, true)
                            .Set(i => i.SyncType, "ActiveSync");

                        AllTestsList.MongoEntity.Add(mongoStatement);


                    }

					myServer.UserCount = results.Count();

                    AllTestsList.MongoEntity.Add(Common.GetInsertIntoDailyStats(myServer, "CAS@ActiveSync#User.Count", results.Count().ToString()));

                    MongoStatementsDelete<VSNext.Mongo.Entities.MobileDevices> mongoDeleteStatement = new MongoStatementsDelete<VSNext.Mongo.Entities.MobileDevices>();
                    mongoDeleteStatement.filterDef = mongoDeleteStatement.repo.Filter.Where(i => i.ServerName == "Exchange" && i.ModifiedOn < DateTime.Now.AddDays(-1));
                    AllTestsList.MongoEntity.Add(mongoDeleteStatement);


					//AllTestsList.AlertDetails.Add(new Alerting() { DeviceType = commonEnums.AlertDevice.Exchange, DeviceName = myServer.Name, AlertType = commonEnums.AlertType.ActiveSyncDevices, Details = "The ActiveSyncDevices is OK, detected at " + DateTime.Now.ToString(), Location = myServer.Location, ResetAlertQueue = commonEnums.ResetAlert.Yes });
					Common.makeAlert(true, myServer, commonEnums.AlertType.Active_Sync_Devices, ref AllTestsList,"There was no issue gathering Active Sync Devices", "CAS");
                }


            }
            catch (Exception ex)
            {

				Common.WriteDeviceHistoryEntry("Exchange", servername, "Error in GetActiveSyncDevices: " + ex.Message, commonEnums.ServerRoles.CAS, Common.LogLevel.Normal);

            }
            finally
            {
                // dispose the runspace and enable garbage collection
                //runspace.Dispose();
                //runspace = null;
            }

        }

        private void TestAutoDiscovery(MonitoredItems.ExchangeServer myServer, ref TestResults AutoDiscoveryIssueList)
        {

            string strResponse = "";
            string strURL = myServer.AutoDiscoveryURLs + "/autodiscover/autodiscover.xml";
           // string strURL = myServer.IPAddress + "/autodiscover/autodiscover.xml";
            try
            {
				Common.WriteDeviceHistoryEntry("Exchange", myServer.Name, "Attempting to verify Auto discovery service.", commonEnums.ServerRoles.CAS);
				Common.WriteDeviceHistoryEntry("Exchange", myServer.Name, "Using address " + strURL, commonEnums.ServerRoles.CAS);
			}
            catch (Exception ex)
            {
            }

          
            Chilkat.Http WebPage = new Chilkat.Http();
            bool success = false;
            try
            {
                //WebPage.Password = myServer.Password;
                //WebPage.Login = myServer.UserName;
                WebPage.Password = myServer.AutoDiscoveryCASPassword;
                WebPage.Login = myServer.AutoDiscoveryCASUserName;

                success = WebPage.UnlockComponent("MZLDADHttp_efwTynJYYR3X");
                if ((success != true))
                {
                   // myServer.ResponseDetails = "Failed to unlock component";
					Common.WriteDeviceHistoryEntry("Exchange", myServer.Name, "Failed to unlock Chilkat HTTP component in TestAutoDiscovery.", commonEnums.ServerRoles.CAS);
                }
                else
                {
					Common.WriteDeviceHistoryEntry("Exchange", myServer.Name, "Unlocked Chilkat HTTP component for TestAutoDiscovery.", commonEnums.ServerRoles.CAS);
                }

            }
            catch (Exception ex)
            {
            }



            try
            {

                if ((success != true))
                {
                    strResponse = WebPage.LastErrorText;
                  
                    myServer.Description = "Unable to connect to the auto discovery URL  at " + DateTime.Now.ToString("HH:mm:ss tt") + " because " + strResponse;
                    myServer.ResponseDetails = "Unable to connect to the auto discovery service. ";
					Common.WriteDeviceHistoryEntry("Exchange", myServer.Name, "Failed to connect because " + strResponse, commonEnums.ServerRoles.CAS);
                    //myServer.ResponseTime = -1;
                    //AutoDiscoveryIssueList.StatusDetails.Add(new TestList() { Details = "Unable to connect to the CAS auto discovery service at " + DateTime.Now.ToString("HH:mm:ss tt") + " because " + strResponse, TestName = "IMAP", Category = commonEnums.ServerRoles.CAS, Result = commonEnums.ServerResult.Fail });
                    // Common.WriteTestResults(myServer.Name, "Client Access", "IMAP", "Fail", myServer.Description);
                    Common.makeAlert(false, myServer, commonEnums.AlertType.Auto_Discovery, ref AutoDiscoveryIssueList, "Unable to connect to the CAS auto discovery service  because" + strResponse, "CAS");
                }
                else
                {
                   

                    try
                    {
                        strResponse = WebPage.QuickGetStr(strURL) ;
						Common.WriteDeviceHistoryEntry("Exchange", myServer.Name, "Auto Discovery Status:" + strResponse, commonEnums.ServerRoles.CAS);
                        if (!String.IsNullOrWhiteSpace(strResponse) && strResponse.IndexOf("ErrorCode") > 1)
                        {
							Common.WriteDeviceHistoryEntry("Exchange", myServer.Name, "Auto Discovery Status:" + "PASS", commonEnums.ServerRoles.CAS);
                            //AutoDiscoveryIssueList.StatusDetails.Add(new TestList() { Details = "Auto discover service responded.", TestName = "Discovery Service", Category = commonEnums.ServerRoles.CAS, Result = commonEnums.ServerResult.Pass });
							Common.makeAlert(true, myServer, commonEnums.AlertType.Auto_Discovery, ref AutoDiscoveryIssueList, "The Auto discovery service responded", "CAS");
                        }
                        else
                        {
							Common.WriteDeviceHistoryEntry("Exchange", myServer.Name, "Auto Discovery Status:" + "FAIL", commonEnums.ServerRoles.CAS);
                            //AutoDiscoveryIssueList.StatusDetails.Add(new TestList() { Details = "Auto discover service did not respond.", TestName = "Discovery Service", Category = commonEnums.ServerRoles.CAS, Result = commonEnums.ServerResult.Fail });
                            Common.makeAlert(false, myServer, commonEnums.AlertType.Auto_Discovery, ref AutoDiscoveryIssueList, "The Auto discovery service did not responded", "CAS"); 
                        }
                       // Common.WriteTestResults(myServer.Name, "CAS", "IMAP", "Pass",  "Service answered with  " + strResponse.Trim() + " at " + System.DateTime.Now.ToShortTimeString());
                       
                    }
                    catch (Exception ex)
                    {
                    }


                  
                }

            }
            catch (Exception ex)
            {
            }
            finally
            {
                WebPage.Dispose();

            }



        }


		private void doOWATest(MonitoredItems.ExchangeServer myServer, ref TestResults IssueList)
		{
			bool owaTest = false;
			string errorMessage = "";
			string finalCookie = "";
			try
			{
				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, " OWA Test:Preparing the first call: GET ", commonEnums.ServerRoles.CAS, Common.LogLevel.Normal);
                //string URL1 = myServer.IPAddress + "/owa/auth/logon.aspx?url=" + myServer.IPAddress + "/owa/&reason=0";
                //string URL2 = myServer.IPAddress + "/owa/auth.owa";
                //string URL3 = myServer.IPAddress + "/owa";
                //string USERNAME = myServer.UserName;
                //string PASSWORD = myServer.Password;
                string URL1 = myServer.OWAURLs + "/owa/auth/logon.aspx?url=" + myServer.OWAURLs + "/owa/&reason=0";
                string URL2 = myServer.OWAURLs + "/owa/auth.owa";
                string URL3 = myServer.OWAURLs + "/owa";
                string USERNAME = myServer.OWACASUserName;
                string PASSWORD = myServer.OWACASPassword;
				string ck1 = "logondata=acc=1&lgn=" + HttpUtility.UrlEncode(myServer.UserName);
				string ck2 = "";

				//---------------- FIRST CALL GET-----------------------
				CookieContainer cookies = new CookieContainer();
				HttpWebRequest webRequest = WebRequest.Create(URL1) as HttpWebRequest;
				webRequest.Timeout = 60000; //set to 1 min
				webRequest.UserAgent = ": Mozilla/5.0 (Windows NT 6.1; WOW64; Trident/7.0; rv:11.0) like Gecko";
				webRequest.ContentType = "application/x-www-form-urlencoded";
				webRequest.Accept = "text/html, application/xhtml+xml, */*";
				webRequest.Headers.Add("DNT", "1");

				webRequest.Headers.Add("Cookie", ck1);
				webRequest.KeepAlive = true;
				System.Net.ServicePointManager.ServerCertificateValidationCallback +=
				delegate(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certificate,
										System.Security.Cryptography.X509Certificates.X509Chain chain,
										System.Net.Security.SslPolicyErrors sslPolicyErrors)
				{
					return true; // **** Always accept
				};
				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, " OWA Test: Cookie1: " + ck1, commonEnums.ServerRoles.CAS, Common.LogLevel.Normal);
				webRequest.CookieContainer = cookies;
				WebResponse webresp = webRequest.GetResponse();
				//ck1 = webresp.Headers.Get(7).ToString();
				WebHeaderCollection webh = webresp.Headers;
				ck1 = webh["Set-Cookie"].ToString();
				//StreamReader responseReader = webRequest.GetResponse().GetResponseStream();
				StreamReader responseReader = new StreamReader(
					  webresp.GetResponseStream()
				   );
				string responseData = responseReader.ReadToEnd();
				responseReader.Close();
				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, " OWA Test: First Call Success. Cookie extracted: " + ck1, commonEnums.ServerRoles.CAS, Common.LogLevel.Normal);
				//-----------SECOND CALL-POST-------------------------------
				//string postData =
				//      String.Format(
				//         "destination=https%3A%2F%2Fjnittech-exchg1.jnittech.com%2Fowa%2F&flags=4&forcedownlevel=0&trusted=4&username=jnittech%5Cadministrator&password=Pa%24%24w0rd&isUtf8=1",
				//        USERNAME, PASSWORD
				//      );
				string postData =
					  String.Format(
						 "destination={0}&flags=4&forcedownlevel=0&trusted=4&username={1}&password={2}&isUtf8=1",
						HttpUtility.UrlEncode(URL3), HttpUtility.UrlEncode(USERNAME), HttpUtility.UrlEncode(PASSWORD)
					  );

				webRequest = WebRequest.Create(URL2) as HttpWebRequest;
				webRequest.Method = "POST";
				webRequest.Timeout = 60000; //set to 1 min
				webRequest.UserAgent = ": Mozilla/5.0 (Windows NT 6.1; WOW64; Trident/7.0; rv:11.0) like Gecko";
				webRequest.Accept = "text/html, application/xhtml+xml, */*";
				webRequest.Headers.Add("DNT", "1");
				webRequest.KeepAlive = true;
				webRequest.Referer = URL1;
				ck1 = ck1 + "; PBack=0";
				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, " OWA Test: Preparing Second Call. Cookie Sent: " + ck1, commonEnums.ServerRoles.CAS, Common.LogLevel.Normal);
				//webRequest.AllowAutoRedirect = true;
				webRequest.Headers.Add("Cookie", ck1);
				webRequest.Headers.Add("Accept-Language", "en-US");
				//webRequest.Connection.a
				webRequest.Headers.Add("Cache-Control", "no-cache");
				webRequest.ContentType = "application/x-www-form-urlencoded";
				//webRequest.Connection = "Keep-Alive";
				//webRequest.CookieContainer = cookies;
				webRequest.AllowAutoRedirect = false;
				// write the form values into the request message
				StreamWriter requestWriter = new StreamWriter(webRequest.GetRequestStream());
				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, " OWA Test: Preparing Second Call. POST Data Sent: " + postData, commonEnums.ServerRoles.CAS, Common.LogLevel.Normal);
				requestWriter.Write(postData);
				requestWriter.Close();
				webresp = webRequest.GetResponse();
				webh = webresp.Headers;
				string headerCookie = webh["Set-Cookie"].ToString();
				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, " OWA Test: Second Call Success. Cookie Received: " + headerCookie, commonEnums.ServerRoles.CAS, Common.LogLevel.Normal);
				//ck2 = webresp.Headers.Get(3).ToString();
				//ck2 = webresp.Headers.ToString();
				string strCADataCookie = Regex.Replace(headerCookie, "(.*)cadata=\"(.*)\"(.*)", "$2");
				string strSessionIDCookie = Regex.Replace(headerCookie, "(.*)sessionid=(.*)(,|;)(.*)", "$2");
				ck2 = ck1 + "; " + "cadata=" + strCADataCookie + "; sessionid=" + strSessionIDCookie;
				// we don't need the contents of the response, just the cookie it issues
				webRequest.GetResponse().Close();
				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, " OWA Test: Preparing last call. Cookie Sent: " + ck2, commonEnums.ServerRoles.CAS, Common.LogLevel.Normal);
				//------------------- LAST CALL GET---------------------------
				webRequest = WebRequest.Create(URL3) as HttpWebRequest;
				webRequest.Timeout = 60000; //set to 1 min
				webRequest.UserAgent = ": Mozilla/5.0 (Windows NT 6.1; WOW64; Trident/7.0; rv:11.0) like Gecko";
				//webRequest.ContentType = "application/x-www-form-urlencoded";
				webRequest.Accept = "text/html, application/xhtml+xml, */*";
				webRequest.Headers.Add("DNT", "1");
				webRequest.Headers.Add("Accept-Language", "en-US");
				//webRequest.Connection.a
				webRequest.Headers.Add("Cache-Control", "no-cache");
				webRequest.Headers.Add("Accept-Encoding", "gzip, deflate");
				webRequest.Headers.Add("Cookie", ck2);
				webRequest.Referer = URL1;
				webRequest.KeepAlive = true;
				//webRequest.AllowAutoRedirect = false;
				System.Net.ServicePointManager.ServerCertificateValidationCallback +=
				delegate(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certificate,
										System.Security.Cryptography.X509Certificates.X509Chain chain,
										System.Net.Security.SslPolicyErrors sslPolicyErrors)
				{
					return true; // **** Always accept
				};
				//webRequest.CookieContainer = cookies;
				webresp = webRequest.GetResponse();
				webh = webresp.Headers;
				headerCookie = webh["Set-Cookie"].ToString();
				string userContextCookie = headerCookie.Substring(headerCookie.IndexOf("UserContext=") + 12);
				userContextCookie = userContextCookie.Substring(0, userContextCookie.IndexOf(";"));
				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, " OWA Test: Last call Success. Cookie received: " + headerCookie, commonEnums.ServerRoles.CAS, Common.LogLevel.Normal);
				//ck1 = webresp.Headers.ToString();
				//StreamReader responseReader = webRequest.GetResponse().GetResponseStream();
				responseReader = new StreamReader(
					  webresp.GetResponseStream()
				   );
				responseData = responseReader.ReadToEnd();
				responseReader.Close();
				if (headerCookie.ToLower().Contains("usercontext") && userContextCookie != "")
					owaTest = true;
				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, " OWA Test: FINAL RESULT: " + owaTest.ToString(), commonEnums.ServerRoles.CAS, Common.LogLevel.Normal);
				finalCookie = ck2 + ";" + " UserContext=" + userContextCookie;

			}
			catch (Exception ex)
			{
				errorMessage = ex.Message.ToString();
				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "Error in OWA Test: " + ex.Message, commonEnums.ServerRoles.CAS, Common.LogLevel.Normal);
			}
			Common.makeAlert(owaTest, myServer, commonEnums.AlertType.OWA_Login, ref IssueList, "Outlook Web App Test." + errorMessage, "CAS");
			try
			{
				if (owaTest)
					doComposeMailTest(myServer, ref IssueList, finalCookie);
				else
					Common.makeAlert(false, myServer, commonEnums.AlertType.Compose_Email, ref IssueList, "Outlook Web App Test Failed. Marking Compose Mial test as FAIL." + errorMessage, "CAS");
			}
			catch (Exception ex2)
			{
				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "Error in OWA Test: " + ex2.Message, commonEnums.ServerRoles.CAS, Common.LogLevel.Normal);
			}

		}

		private void doComposeMailTest(MonitoredItems.ExchangeServer myServer, ref TestResults IssueList,string cookie)
		{
			//string URL1="/owa/?ae=Item&t=IPM.Note&a=New";
			bool composeEmailTest = false;
			string errorMessage = "";
			try
			{
				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, " Comose Email Test:Preparing the first call: GET ", commonEnums.ServerRoles.CAS, Common.LogLevel.Normal);
                //string URL1 = myServer.IPAddress + "/owa/?ae=Item&t=IPM.Note&a=New";
                //string URL2 = myServer.IPAddress + "/owa/auth.owa";
                //string URL3 = myServer.IPAddress + "/owa";
                //string USERNAME = myServer.UserName;
                //string PASSWORD = myServer.Password;
                string URL1 = myServer.IPAddress + "/owa/?ae=Item&t=IPM.Note&a=New";
                string URL2 = myServer.IPAddress + "/owa/auth.owa";
                string URL3 = myServer.IPAddress + "/owa";
                string USERNAME = myServer.UserName;
                string PASSWORD = myServer.Password;
				string ck1 = "logondata=acc=1&lgn=" + HttpUtility.UrlEncode(myServer.UserName);
				string ck2 = "";

				//---------------- FIRST CALL GET-----------------------
				CookieContainer cookies = new CookieContainer();
				HttpWebRequest webRequest = WebRequest.Create(URL1) as HttpWebRequest;
				webRequest.Timeout = 60000; //set to 1 min
				webRequest.UserAgent = ": Mozilla/5.0 (Windows NT 6.1; WOW64; Trident/7.0; rv:11.0) like Gecko";
				webRequest.ContentType = "application/x-www-form-urlencoded";
				webRequest.Accept = "text/html, application/xhtml+xml, */*";
				webRequest.Headers.Add("DNT", "1");

				webRequest.Headers.Add("Cookie", cookie);
				webRequest.KeepAlive = true;
				System.Net.ServicePointManager.ServerCertificateValidationCallback +=
				delegate(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certificate,
										System.Security.Cryptography.X509Certificates.X509Chain chain,
										System.Net.Security.SslPolicyErrors sslPolicyErrors)
				{
					return true; // **** Always accept
				};
				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, " OWA Compose Mail Test: Cookie1: " + cookie, commonEnums.ServerRoles.CAS, Common.LogLevel.Normal);
				//webRequest.CookieContainer = cookies;
				WebResponse webresp = webRequest.GetResponse();
				//ck1 = webresp.Headers.Get(7).ToString();
				WebHeaderCollection webh = webresp.Headers;
				//ck1 = webh["Set-Cookie"].ToString();
				//StreamReader responseReader = webRequest.GetResponse().GetResponseStream();
				StreamReader responseReader = new StreamReader(
					  webresp.GetResponseStream()
				   );
				string responseData = responseReader.ReadToEnd();
				responseReader.Close();
				if (responseData != "")
					composeEmailTest = true;
			}
			catch (Exception ex)
			{
				errorMessage = ex.Message.ToString();
				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "Error in OWA Compose Mail Test: " + ex.Message, commonEnums.ServerRoles.CAS, Common.LogLevel.Normal);
			}

			Common.makeAlert(composeEmailTest, myServer, commonEnums.AlertType.Compose_Email, ref IssueList, "Outlook Compose Email Test." + errorMessage, "CAS"); 

		}

		private void doOWATest2013(MonitoredItems.ExchangeServer myServer, ref TestResults IssueList)
		{
			Common.WriteDeviceHistoryEntry("Exchange", myServer.Name, "Server is 2013 Version. Doing 2013 OWA test.", commonEnums.ServerRoles.CAS);
			bool owaTest = false;
			string errorMessage = "";
			string finalCookie = "";
			try
			{
				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, " OWA Test:Preparing the first call: GET ", commonEnums.ServerRoles.CAS, Common.LogLevel.Normal);
                //string URL1 = myServer.IPAddress + "/owa/auth/logon.aspx?url=" + myServer.IPAddress + "/owa/&reason=0";
                //string URL2 = myServer.IPAddress + "/owa/auth.owa";
                //string URL3 = myServer.IPAddress + "/owa/#authRedirect=true";
                //string URL4 = myServer.IPAddress + "/owa/userspecificresourceinjector.ashx?ver=15.0.1044.25&appcacheclient=1&layout=mouse";
                //string USERNAME = myServer.UserName;
                //string PASSWORD = myServer.Password;
                string URL1 = myServer.IPAddress + "/owa/auth/logon.aspx?url=" + myServer.IPAddress + "/owa/&reason=0";
                string URL2 = myServer.IPAddress + "/owa/auth.owa";
                string URL3 = myServer.IPAddress + "/owa/#authRedirect=true";
                string URL4 = myServer.IPAddress + "/owa/userspecificresourceinjector.ashx?ver=15.0.1044.25&appcacheclient=1&layout=mouse";
                string USERNAME = myServer.UserName;
                string PASSWORD = myServer.Password;
				

				//---------------- FIRST CALL GET-----------------------
				CookieContainer cookies = new CookieContainer();
				HttpWebRequest webRequest = WebRequest.Create(URL1) as HttpWebRequest;
				webRequest.Timeout = 60000; //set to 1 min
				webRequest.UserAgent = ": Mozilla/5.0 (Windows NT 6.1; WOW64; Trident/7.0; rv:11.0) like Gecko";
				webRequest.ContentType = "application/x-www-form-urlencoded";
				webRequest.Accept = "text/html, application/xhtml+xml, */*";
				webRequest.Headers.Add("DNT", "1");

				webRequest.KeepAlive = true;
				System.Net.ServicePointManager.ServerCertificateValidationCallback +=
				delegate(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certificate,
										System.Security.Cryptography.X509Certificates.X509Chain chain,
										System.Net.Security.SslPolicyErrors sslPolicyErrors)
				{
					return true; // **** Always accept
				};
				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, " OWA Test:", commonEnums.ServerRoles.CAS, Common.LogLevel.Normal);
				webRequest.CookieContainer = cookies;
				WebResponse webresp = webRequest.GetResponse();
				//ck1 = webresp.Headers.Get(7).ToString();
				WebHeaderCollection webh = webresp.Headers;
				//ck1 = webh["Set-Cookie"].ToString();
				//StreamReader responseReader = webRequest.GetResponse().GetResponseStream();
				StreamReader responseReader = new StreamReader(
					  webresp.GetResponseStream()
				   );
				string responseData = responseReader.ReadToEnd();
				responseReader.Close();
				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, " OWA Test: First Call Success. ", commonEnums.ServerRoles.CAS, Common.LogLevel.Normal);
				//-----------SECOND CALL-POST-------------------------------
				//string postData =
				//      String.Format(
				//         "destination=https%3A%2F%2Fjnittech-exchg1.jnittech.com%2Fowa%2F&flags=4&forcedownlevel=0&trusted=4&username=jnittech%5Cadministrator&password=Pa%24%24w0rd&isUtf8=1",
				//        USERNAME, PASSWORD
				//      );
				string postData =
				  String.Format(
					 "destination={0}&flags=4&forcedownlevel=0&username={1}&password={2}&isUtf8=1",
					HttpUtility.UrlEncode(URL3), HttpUtility.UrlEncode(USERNAME), HttpUtility.UrlEncode(PASSWORD)
				  );

				webRequest = WebRequest.Create(URL2) as HttpWebRequest;
				webRequest.Method = "POST";
				webRequest.Timeout = 60000; //set to 1 min
				webRequest.UserAgent = ": Mozilla/5.0 (Windows NT 6.1; WOW64; Trident/7.0; rv:11.0) like Gecko";
				webRequest.Accept = "text/html, application/xhtml+xml, */*";
				webRequest.Headers.Add("DNT", "1");
				webRequest.KeepAlive = true;
				webRequest.Referer = URL1;
				//ck1 = ck1 + "; PBack=0";
				//Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, " OWA Test: Preparing Second Call. Cookie Sent: " + ck1, commonEnums.ServerRoles.CAS, Common.LogLevel.Normal);
				//webRequest.AllowAutoRedirect = true;
				//webRequest.Headers.Add("Cookie", ck1);
				webRequest.Headers.Add("Accept-Language", "en-US");
				//webRequest.Connection.a
				webRequest.Headers.Add("Cache-Control", "no-cache");
				webRequest.ContentType = "application/x-www-form-urlencoded";
				//webRequest.Connection = "Keep-Alive";
				webRequest.CookieContainer = cookies;
				webRequest.AllowAutoRedirect = false;
				// write the form values into the request message
				StreamWriter requestWriter = new StreamWriter(webRequest.GetRequestStream());
				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, " OWA Test: Preparing Second Call. POST Data Sent: " + postData, commonEnums.ServerRoles.CAS, Common.LogLevel.Normal);
				requestWriter.Write(postData);
				requestWriter.Close();
				webresp = webRequest.GetResponse();
				webh = webresp.Headers;
				string headerCookie = webh["Set-Cookie"].ToString();
				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, " OWA Test: Second Call Success. Cookie Received: " + headerCookie, commonEnums.ServerRoles.CAS, Common.LogLevel.Normal);
				//ck2 = webresp.Headers.Get(3).ToString();
				//ck2 = webresp.Headers.ToString();
				//string strCADataCookie = Regex.Replace(headerCookie, "(.*)cadata=\"(.*)\"(.*)", "$2");
				//string strSessionIDCookie = Regex.Replace(headerCookie, "(.*)sessionid=(.*)(,|;)(.*)", "$2");
				//ck2 = ck1 + "; " + "cadata=" + strCADataCookie + "; sessionid=" + strSessionIDCookie;
				// we don't need the contents of the response, just the cookie it issues
				webRequest.GetResponse().Close();
				//Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, " OWA Test: Preparing last call. Cookie Sent: " + ck2, commonEnums.ServerRoles.CAS, Common.LogLevel.Normal);
				//------------------- LAST CALL GET---------------------------
				webRequest = WebRequest.Create(URL4) as HttpWebRequest;
				webRequest.Timeout = 60000; //set to 1 min
				webRequest.UserAgent = ": Mozilla/5.0 (Windows NT 6.1; WOW64; Trident/7.0; rv:11.0) like Gecko";
				//webRequest.ContentType = "application/x-www-form-urlencoded";
				webRequest.Accept = "text/html, application/xhtml+xml, */*";
				webRequest.Headers.Add("DNT", "1");
				webRequest.Headers.Add("Accept-Language", "en-US");
				//webRequest.Connection.a
				webRequest.Headers.Add("Cache-Control", "no-cache");
				webRequest.Headers.Add("Accept-Encoding", "gzip, deflate");
				//webRequest.Headers.Add("Cookie", ck2);
				webRequest.CookieContainer = cookies;
				webRequest.Referer = URL1;
				webRequest.KeepAlive = true;
				//webRequest.AllowAutoRedirect = false;
				System.Net.ServicePointManager.ServerCertificateValidationCallback +=
				delegate(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certificate,
										System.Security.Cryptography.X509Certificates.X509Chain chain,
										System.Net.Security.SslPolicyErrors sslPolicyErrors)
				{
					return true; // **** Always accept
				};
				//webRequest.CookieContainer = cookies;
				webresp = webRequest.GetResponse();
				webh = webresp.Headers;
				headerCookie = webh["Set-Cookie"].ToString();
				string userContextCookie = headerCookie.Substring(headerCookie.IndexOf("UC=") + 3);
				userContextCookie = userContextCookie.Substring(0, userContextCookie.IndexOf(";"));
				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, " OWA Test: Last call Success. Cookie received: " + headerCookie, commonEnums.ServerRoles.CAS, Common.LogLevel.Normal);
				//ck1 = webresp.Headers.ToString();
				//StreamReader responseReader = webRequest.GetResponse().GetResponseStream();
				responseReader = new StreamReader(
					  webresp.GetResponseStream()
				   );
				responseData = responseReader.ReadToEnd();
				responseReader.Close();
				if (headerCookie.ToLower().Contains("uc") && userContextCookie != "")
					owaTest = true;
				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, " OWA Test: FINAL RESULT: " + owaTest.ToString(), commonEnums.ServerRoles.CAS, Common.LogLevel.Normal);
				//finalCookie = ck2 + ";" + " UserContext=" + userContextCookie;

			}
			catch (Exception ex)
			{
				errorMessage = ex.Message.ToString();
				Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "Error in OWA Test: " + ex.Message, commonEnums.ServerRoles.CAS, Common.LogLevel.Normal);
			}
			Common.makeAlert(owaTest, myServer, commonEnums.AlertType.OWA_Login, ref IssueList, "Outlook Web App Test." + errorMessage, "CAS");
			Common.makeAlert(owaTest, myServer, commonEnums.AlertType.Compose_Email, ref IssueList, "Outlook Web App Compose Mail Test." + errorMessage, "CAS");
			

		}
    }

}