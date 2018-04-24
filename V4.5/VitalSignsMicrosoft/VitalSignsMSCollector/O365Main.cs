using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Management.Automation.Remoting;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Threading;
using System.Collections;
using System.Data;

namespace VitalSignsMSCollector
{
	public class ReturnPowerShellObjects : IDisposable
	 {
		 public string[] ExchangeServersCollection;
		 public PowerShell PS;
		 //public Runspace runspace;
		 //public PSSession Session;
		 public String ErrorMessage = "";
		 private Boolean disposed;
		 public String ServerType = "";
		 public Boolean Connected = false;

		 public void Dispose()
		 {
			 Dispose(true);
			 GC.SuppressFinalize(this);
		 }

		 ~ReturnPowerShellObjects()
		 {
			 Dispose(true);
		 }

		 protected virtual void Dispose(bool disposing)
		 {
			 if (disposed)
				 return;

			 if (disposing)
			 {
				 try
				 {
					 //if (!(this.Session == null && this.runspace == null && this.PS == null))
					 if (!(this.PS == null))
					 {
						 //Specificly remove session ID
						 //if (this.Session != null)
						 //{
						 //    Command cmd = new Command("remove-pssession");
						 //    cmd.Parameters.Add("id", this.Session.Id);

						 //    try
						 //    {
						 //        this.PS.Commands.Clear();
						 //        this.PS.Commands.AddCommand(cmd);
						 //        this.PS.Invoke();
						 //    }
						 //    catch (Exception ex)
						 //    {
						 //        Common.WriteDeviceHistoryEntry("All", "Microsoft", "Exception removing PSSession with ID. Exception: " + ex.Message.ToString(), Common.LogLevel.Normal);
						 //    }
						 //}

						 //Remove all sessions
						 try
						 {
							 String script = "Get-PSSession | Remove-PSSession";
							 this.PS.Commands.Clear();
							 this.PS.AddScript(script);
							 this.PS.Invoke();
						 }
						 catch (Exception ex)
						 {
							 //Common.WriteDeviceHistoryEntry("All", "Microsoft", "Exception removing all PSSession. Exception: " + ex.Message.ToString(), Common.LogLevel.Normal);
						 }


						 //Removes any and all modules improted
						 try
						 {
							 String script = "Get-Module tmp* | Remove-Module";
							 this.PS.Commands.Clear();
							 this.PS.AddScript(script);
							 this.PS.Invoke();
						 }
						 catch (Exception ex)
						 {
							 //Common.WriteDeviceHistoryEntry("All", "Microsoft", "Exception removing all imported modules. Exception: " + ex.Message.ToString(), Common.LogLevel.Normal);
						 }

						 string PID = "";
						 //Stops the process from running
						 try
						 {
							 PSCommand command = new PSCommand();
							 command.AddScript("$PID");
							 this.PS.Commands = command; ;
							 Collection<PSObject> results = this.PS.Invoke();
							 PID = results[0].BaseObject.ToString();
							 
						 }
						
						 catch (Exception ex)
						 {
							 //Common.WriteDeviceHistoryEntry("All", "Microsoft", "Exception removing all imported modules. Exception: " + ex.Message.ToString(), Common.LogLevel.Normal);
						 }

						 //Stops the process from running
						 try
						 {
							 String script = "Stop-Process $PID";
							 this.PS.Commands.Clear();
							 this.PS.AddScript(script);
							 this.PS.Invoke();
						 }
						 catch(PSRemotingTransportException ex)
						 {
							 //This error is expected.  The above cmd will stop the process from running due to large code rework otherwise
						 }
						 catch (Exception ex)
						 {
							 //sCommon.WriteDeviceHistoryEntry("All", "Microsoft", "Exception stopping process. Exception: " + ex.Message.ToString(), Common.LogLevel.Normal);
						 }


						 //closes the runspaces.  Might be repeats in there but better to be safe
						 try
						 {
							 //if (this.Session.Runspace.RunspaceStateInfo.State == RunspaceState.Opened)
								// this.Session.Runspace.Close();
							 if (this.PS.Runspace.RunspaceStateInfo.State == RunspaceState.Opened)
								 this.PS.Runspace.Close();
						 }
						 catch (Exception ex)
						 {
							 //Common.WriteDeviceHistoryEntry("All", "Microsoft", "Exception removing PSSession with ID. Exception: " + ex.Message.ToString(), Common.LogLevel.Normal);
						 }


						 //disposes of other resources and nulls them

						 this.PS.Dispose();
						 this.PS.Commands.Clear();
						 this.PS.Streams.ClearStreams();

						 this.ErrorMessage = null;
						 this.ExchangeServersCollection = null;
						 this.PS = null;
						 //this.Session = null;
					 }

				 }
				 catch (Exception ex)
				 {
					 //Common.WriteDeviceHistoryEntry("All", "Microsoft", "Exception cleaning up ReturnPowerShellObjects. Exception: " + ex.Message.ToString());
				 }
			 }


			 disposed = true;
		 }
	 }
	class O365Main
	{
		public static SecureString String2SecureString(string password)
        {
            SecureString remotePassword = new SecureString();
            for (int i = 0; i < password.Length; i++)
                remotePassword.AppendChar(password[i]);

            return remotePassword;
        }

		public static ReturnPowerShellObjects PrereqForOffice365WithCmdlets( string UserName, string Password, string cmdlets)
		{
			ReturnPowerShellObjects PSObj = new ReturnPowerShellObjects();
			try
			{
				string tenantName = "";
				if (UserName != "")
					tenantName = UserName.Split('@')[1].ToString().Split('.')[0].ToString();
				//Common.WriteDeviceHistoryEntry(ServerType, ServerName, "In  PrereqForOffice365WithCmdlets", Common.LogLevel.Normal);
				//Common.WriteDeviceHistoryEntry(ServerType, ServerName, "In  PrereqForOffice365WithCmdlets: before Import", Common.LogLevel.Normal);
				InitialSessionState session = InitialSessionState.CreateDefault();
				session.ImportPSModule(new string[] { "MSOnline" });
				//Common.WriteDeviceHistoryEntry(ServerType, ServerName, "In  PrereqForOffice365WithCmdlets: after Import", Common.LogLevel.Normal);

				System.Uri uri = new Uri("https://outlook.office365.com" + "/powershell-liveid/");
				System.Security.SecureString securePassword = String2SecureString(Password);

				PSCredential creds = new PSCredential(UserName, securePassword);
				Runspace runspace = RunspaceFactory.CreateOutOfProcessRunspace(new TypeTable(new string[0]));
				//Common.WriteDeviceHistoryEntry(ServerType, ServerName, "In  PrereqForOffice365WithCmdlets: before open", Common.LogLevel.Normal);
				runspace.Open();
				//Common.WriteDeviceHistoryEntry(ServerType, ServerName, "In  PrereqForOffice365WithCmdlets: after open", Common.LogLevel.Normal);
				PowerShell powerShell = PowerShell.Create();
				PSObj.PS = powerShell;
				//Common.WriteDeviceHistoryEntry(ServerType, ServerName, "In  PrereqForOffice365WithCmdlets-2", Common.LogLevel.Normal);


				powerShell.Runspace = runspace;

				PSCommand psSession = new PSCommand();
				psSession.AddCommand("New-PSSession");
				psSession.AddParameter("ConfigurationName", "Microsoft.Exchange");
				psSession.AddParameter("ConnectionUri", uri);
				psSession.AddParameter("Credential", creds);
				psSession.AddParameter("Authentication", "Basic");
				psSession.AddParameter("AllowRedirection");

				powerShell.Commands = psSession;
				//var result = powerShell.Invoke();
				Collection<PSObject> result = powerShell.Invoke<PSObject>();
				//Common.WriteDeviceHistoryEntry("All", ServerType, "In  PrereqForOffice365WithCmdlets result:" + result[0], Common.LogLevel.Normal);
				//Server.AuthenticationTest = true;
				foreach (ErrorRecord err in powerShell.Streams.Error)
				{
					//WriteDeviceHistoryEntry(ServerType, ServerName, "In  PrereqForOffice365WithCmdlets error" + err.Exception, Common.LogLevel.Normal);
					//if (err.Exception.Message.ToLower().Contains("access denied"))
						//Server.AuthenticationTest = false;
					string error = "";
				}

				//PSSession pssession = (PSSession)result[0];
				//PSObj.Session = pssession;



				PSCommand setVar = new PSCommand();
				setVar.AddScript("$ra = $(Get-PSSession)[0]");

				powerShell.Commands = setVar;
				powerShell.Runspace = runspace;
				powerShell.Invoke();

				PSCommand importSession = new PSCommand();
				importSession.AddScript("Import-PSSession -AllowClobber -Session $ra " + cmdlets + " -FormatTypeName *");
				powerShell.Commands = importSession;
				powerShell.Runspace = runspace;
				powerShell.Invoke();

				string searchMsg = "Running the Get-Command command in a remote session returned no results";
				if (powerShell.Streams.Error.Where(record => record.Exception.ToString().Contains(searchMsg)).ToArray().Length > 0)
				{
					PSObj.ErrorMessage = "The module was not able to be located";
				}


				PSCommand command;

				foreach (String mod in new String[] { "MSOnlne", "Microsoft.Online.SharePoint.PowerShell" })
				{
					command = new PSCommand();
					command.AddCommand("Import-Module");
					command.AddParameter("Name", mod);
					powerShell.Commands = command;
					powerShell.Invoke();

					//WriteDeviceHistoryEntry(ServerType, ServerName, "Imported the module " + mod + ".", LogLevel.Normal);
				}


				//WriteDeviceHistoryEntry(ServerType, ServerName, "In  PrereqForOffice365WithCmdlets before MSOL connection", Common.LogLevel.Normal);
				PSCommand connect = new PSCommand();
				connect.AddCommand("Connect-MsolService");
				connect.AddParameter("Credential", creds);
				powerShell.Commands = connect;
				powerShell.Invoke();
				//WriteDeviceHistoryEntry(ServerType, ServerName, "In  PrereqForOffice365WithCmdlets after MSOL connection", Common.LogLevel.Normal);
				PSCommand connectSPOL = new PSCommand();
				connectSPOL.AddCommand("Connect-SPOService");
				//WriteDeviceHistoryEntry(ServerType, ServerName, "In  PrereqForOffice365WithCmdlets before SPO connection", Common.LogLevel.Normal);
				string sharePointURL = "";
				if (tenantName != "")
				{
					//Server.tenantName = tenantName;
					sharePointURL = "https://" + tenantName + "-admin.sharepoint.com";
				}
				//WriteDeviceHistoryEntry(ServerType, ServerName, "In  PrereqForOffice365WithCmdlets before SPO connection-URL:" + sharePointURL, Common.LogLevel.Normal);
				connectSPOL.AddParameter("Url", sharePointURL);
				connectSPOL.AddParameter("Credential", creds);
				powerShell.Commands = connectSPOL;
				powerShell.Invoke();
				//WriteDeviceHistoryEntry(ServerType, ServerName, "In  PrereqForOffice365WithCmdlets after SPO connection-URL:" + sharePointURL, Common.LogLevel.Normal);

				PSObj.PS = powerShell;
				//PSObj.runspace = runspace;
				PSObj.Connected = true;

				command = new PSCommand();
				command.AddScript("$PID");
				powerShell.Commands = command; ;
				Collection<PSObject> results = powerShell.Invoke();

				//WriteDeviceHistoryEntry(ServerType, ServerName, "O365: $PID = " + results[0].BaseObject, commonEnums.ServerRoles.Empty, LogLevel.Normal);

			}
			catch (Exception ex)
			{
				try
				{
					PSCommand command = new PSCommand();
					command.AddScript("$PID");
					PSObj.PS.Commands = command; ;
					Collection<PSObject> results = PSObj.PS.Invoke();

					//WriteDeviceHistoryEntry(ServerType, ServerName, "O365 Failed: $PID = " + results[0].BaseObject, commonEnums.ServerRoles.Empty, LogLevel.Normal);
				}
				catch (Exception ex2)
				{
					//WriteDeviceHistoryEntry(ServerType, ServerName, "O365 Failed: " + ex2.Message, commonEnums.ServerRoles.Empty, LogLevel.Normal);
				}
				PSObj.Connected = false;
				//if (PSObj.PS == null)
				//PSObj.PS = PowerShell.Create();
				//if (PSObj.runspace == null)
				//	PSObj.runspace = RunspaceFactory.CreateRunspace();
				//if (PSObj.PS.Runspace == null)
				//	PSObj.PS.Runspace = PSObj.runspace;
				//WriteDeviceHistoryEntry(ServerType, ServerName, " Failed Imported the PSSession." + ex.Message.ToString(), role, LogLevel.Normal);
			}


			return PSObj;
			//    }
			//}
		}

		public void testO365ServerConnectivity(string userName, string pwd)
		{
			
			//doURLTest(Server, ref AllTestsList);
			//string cmdlets = "-CommandName Test-MAPIConnectivity,Get-MailboxActivityReport,Get-Mailbox,Get-MailboxStatistics,Get-MsolAccountSku,Get-MsolUser,Get-User,Get-DistributionGroup,Get-MobileDeviceStatistics,Get-MobileDevice,Get-MsolCompanyInformation,Get-CsActiveUserReport,Get-CsClientDeviceReport,Get-CsP2PAVTimeReport,Get-CsConferenceReport,Get-CsP2PSessionReport";
			//Common.WriteDeviceHistoryEntry(Server.ServerType, Server.Name, "Before  PrereqForOffice365WithCmdlets", Common.LogLevel.Normal);
			//pre-check to see if ADFS /Normal
			ReturnPowerShellObjects results = PrereqForOffice365WithCmdlets( userName, pwd, "CommandName Connect-MsolService, Test-MAPIConnectivity,Get-MailboxActivityReport,Get-Mailbox,Get-MailboxStatistics,Get-MsolAccountSku,Get-MsolUser,Get-User,Get-DistributionGroup,Get-MobileDeviceStatistics,Get-MobileDevice,Get-MsolCompanyInformation,Get-CsActiveUserReport,Get-CsClientDeviceReport,Get-CsP2PAVTimeReport,Get-CsConferenceReport,Get-CsP2PSessionReport");
			
			using (results)
					{
						//Common.WriteDeviceHistoryEntry(testServer.ServerType, testServer.Name, " Hourly Task started.", Common.LogLevel.Normal);
						string t = "";
						
					}
			//Common.WriteDeviceHistoryEntry(Server.ServerType, Server.Name, "After  PrereqForOffice365WithCmdlets", Common.LogLevel.Normal);
			//if (results.Connected == false)
			//{
			//    if (Server.AuthenticationTest == false)
			//        Common.makeAlert(false, Server, commonEnums.AlertType.Authentication, ref AllTestsList, "Failed to Authenticate.", Server.Category);
			//    else
			//        Common.makeAlert(true, Server, commonEnums.AlertType.Authentication, ref AllTestsList, "Pass.", Server.Category);


			//    Common.WriteDeviceHistoryEntry(Server.ServerType, Server.Name, " checkServer: PS SESSION IS NULL", Common.LogLevel.Normal);
			//    Server.Status = "Not Responding";
			//    Server.StatusCode = "Not Responding";
			//    //***************************************************Not Responding********************************************//
			//}
			//else
			//{
			//    Common.makeAlert(true, Server, commonEnums.AlertType.Authentication, ref AllTestsList, "Pass", Server.Category);
			//}
			
		}

	}
}
