using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using System.IO;
using System.Threading;
using System.Security;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Management.Automation.Remoting;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Collections;
using System.Data;
using System.Net.NetworkInformation;
using System.Net;
using System.Security;
using System.Security.Cryptography;

namespace VitalSignsMSCollector
{
	public class MyContext : ApplicationContext
	{
		#region Private Fields
		private bool stopIncrementingSteps = false;
		private bool Busy = false;
		private bool criticalError = false;
		private TimeSpan RunTime;// = new TimeSpan(15, 44, 0); //11:15
		private NotifyIcon withEventsField_Tray = new NotifyIcon();
		int step = 0;
		private NotifyIcon Tray = new NotifyIcon();
		private ContextMenuStrip CMS = new ContextMenuStrip();

		private ToolStripMenuItem mnuExit = new ToolStripMenuItem("Exit");

		private ToolStripMenuItem mnuConfigure = new ToolStripMenuItem("Configure");
		private ToolStripMenuItem mnuShow = new ToolStripMenuItem("Show Performance Graph");

		private ToolStripMenuItem mnuCheckUpdates = new ToolStripMenuItem("Check For Updates");
		private ToolStripMenuItem mnuCheck = new ToolStripMenuItem("Check For Updates");

		private System.ComponentModel.BackgroundWorker BGW = new System.ComponentModel.BackgroundWorker();
		private System.ComponentModel.BackgroundWorker BGW2 = new System.ComponentModel.BackgroundWorker();
		string logFileName = "";
		int updateStyle = 1;//manual run
		string Country = "United States";
		string State = "New Jersey";
		string City = "Monroe";
		string serviceURL = "http://jnitinc.com/WebService/AddO365Stats.php";
		string pingURL = "https://outlook.office365.com";
		string accountName = "";
		string hostName = "";
		Dictionary<string, string> databaseBkp = new Dictionary<string, string>();
		#endregion
		#region events
		
		private void BGW_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
		{
			
			
			string userName = ConfigurationManager.AppSettings["UserName"].ToString();
			accountName = userName.Split(new char[] { '@' })[1].ToString();
			string password = ConfigurationManager.AppSettings["Password"].ToString();
			 Country = ConfigurationManager.AppSettings["Country"].ToString();
			 State = ConfigurationManager.AppSettings["State"].ToString();
			 City = ConfigurationManager.AppSettings["City"].ToString();
			 serviceURL = ConfigurationManager.AppSettings["serviceURL"].ToString();
			 serviceURL += "/AddO365Stats.php";
			pingURL = ConfigurationManager.AppSettings["PingURL"].ToString();

			if (userName == "" || password == "")
			{
				frmConfigure frmc = new frmConfigure();
				frmc.loadMe();
				frmc.ShowDialog();
			}
			string scanInterval = ConfigurationManager.AppSettings["ScanIntervalMins"].ToString();
			if (scanInterval == "")
				scanInterval = "8";

			int iScanInterval = Convert.ToInt32(scanInterval);
			while (true)
			{
				writeTologFile("collecting");
				Busy = true;
				BGW.ReportProgress(0, "Collecting");
				// perform the actual "work":
				//Monitor O365 here:
				if (userName != "" && password != "")
					testO365ServerConnectivity(userName, password);
				Busy = false;
				BGW.ReportProgress(0, "Idle");
				writeTologFile("collecting");

				System.Threading.Thread.Sleep(1000 * 60 * iScanInterval);
			}
		}
		public static SecureString String2SecureString(string password)
		{
			SecureString remotePassword = new SecureString();
			for (int i = 0; i < password.Length; i++)
				remotePassword.AppendChar(password[i]);

			return remotePassword;
		}
		private void doPingTest()
		{
			
			try
			{
				pingURL=pingURL.ToLower().Replace("http://", "").Replace("https://", "");
				IPHostEntry IpEntry = Dns.GetHostEntry(pingURL);

				Ping pingSender = new Ping();

				PingOptions options = new PingOptions();
				options.DontFragment = true;
				

				string data = "a";
				byte[] buffer = Encoding.ASCII.GetBytes(data);
				int timeout = 5000;
				PingReply reply = pingSender.Send(pingURL, timeout, buffer, options);
				hostName = IpEntry.HostName;
				if (reply.Status == IPStatus.Success)
					submitRequest(serviceURL + "?AccountName=" + accountName + "&Country=" + Country + "&State=" + State + "&City=" + City + "&StatName=O365SPingTest&HostName=" + hostName + "&StatValue=" + reply.RoundtripTime.ToString(), "GET", "", "", "");
				else
					submitRequest(serviceURL + "?AccountName=" + accountName + "&Country=" + Country + "&State=" + State + "&City=" + City + "&StatName=O365SPingTest&HostName=" + hostName + "&StatValue=" + "-1", "GET", "", "", "");


			}
			catch
			{
			}
		}
		public ReturnPowerShellObjects DoO365LogonTests(string UserName, string Password, string cmdlets)
		{
			writeTologFile("O365LogonTests");
			ReturnPowerShellObjects PSObj = new ReturnPowerShellObjects();
			try
			{
				bool authTest=true;
				string tenantName = "";
				if (UserName != "")
					tenantName = UserName.Split('@')[1].ToString().Split('.')[0].ToString();
				InitialSessionState session = InitialSessionState.CreateDefault();
				session.ImportPSModule(new string[] { "MSOnline" });

				System.Uri uri = new Uri("https://" + pingURL + "/powershell-liveid/");
				System.Security.SecureString securePassword = String2SecureString(Password);

				PSCredential creds = new PSCredential(UserName, securePassword);
				//PSCredential creds2 = new PSCredential(UserName, securePassword);
				Runspace runspace = RunspaceFactory.CreateOutOfProcessRunspace(new TypeTable(new string[0]));
				runspace.Open();
				PowerShell powerShell = PowerShell.Create();
				PSObj.PS = powerShell;

				powerShell.Runspace = runspace;

				PSCommand psSession = new PSCommand();
				psSession.AddCommand("New-PSSession");
				psSession.AddParameter("ConfigurationName", "Microsoft.Exchange");
				psSession.AddParameter("ConnectionUri", uri);
				psSession.AddParameter("Credential", creds);
				psSession.AddParameter("Authentication", "Basic");
				psSession.AddParameter("AllowRedirection");

				powerShell.Commands = psSession;
				Collection<PSObject> result = powerShell.Invoke<PSObject>();
				foreach (ErrorRecord err in powerShell.Streams.Error)
				{
					writeTologFile(err.Exception.Message);
					if (err.Exception.Message.ToLower().Contains("access denied"))
						authTest = false;
				}

				if (!authTest)
					return PSObj;
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
					writeTologFile(PSObj.ErrorMessage);
				}

				PSCommand command;

				foreach (String mod in new String[] { "MSOnline", "Microsoft.Online.SharePoint.PowerShell" })
				{
					command = new PSCommand();
					command.AddCommand("Import-Module");
					command.AddParameter("Name", mod);
					powerShell.Commands = command;
					powerShell.Invoke();
				}

				string response = "";
				long done;
				long start;
					TimeSpan elapsed = new TimeSpan(0);;

				if (Convert.ToBoolean(ConfigurationManager.AppSettings["ServerLoginTest"].ToString()))
				{
					writeTologFile("Performing O365 Server Login Test");
				PSCommand connect = new PSCommand();
				connect.AddCommand("Connect-MsolService");
				connect.AddParameter("Credential", creds);
				powerShell.Commands = connect;
				
				
				start = DateTime.Now.Ticks;

				powerShell.Invoke();

				done = DateTime.Now.Ticks;
				elapsed = new TimeSpan(done - start);

				//http://jnitinc.com/WebService/AddO365Stats.php?StatName=SNWeb&StatValue=2&Country=TestCountry&State=TestState&City=TestCity
				 response = submitRequest(serviceURL + "?AccountName=" + accountName + "&Country=" + Country + "&State=" + State + "&City=" + City + "&StatName=O365ServerLoginTest&StatValue=" + elapsed.TotalMilliseconds.ToString()+"&HostName=" + hostName , "GET", "", "", "");
				}

				string sharePointURL = "";
				if (tenantName != "")
				{
					sharePointURL = "https://" + tenantName + "-admin.sharepoint.com";
				}
				
				if (Convert.ToBoolean(ConfigurationManager.AppSettings["SPOLoginTest"].ToString()))
				{
					writeTologFile("Performing SPO Server Login Test");
				PSCommand connectSPOL = new PSCommand();
				connectSPOL.AddCommand("Connect-SPOService");
				connectSPOL.AddParameter("Url", sharePointURL);
				connectSPOL.AddParameter("Credential", creds);

				powerShell.Commands = connectSPOL;
				
				start = DateTime.Now.Ticks;

				powerShell.Invoke();
				done = DateTime.Now.Ticks;
				elapsed = new TimeSpan(done - start);
				response = submitRequest(serviceURL + "?AccountName=" + accountName + "&Country=" + Country + "&State=" + State + "&City=" + City + "&StatName=O365SPOLoginTest&StatValue=" + elapsed.TotalMilliseconds.ToString() + "&HostName=" + hostName, "GET", "", "", "");
				}
				PSObj.PS = powerShell;
				PSObj.Connected = true;

				command = new PSCommand();
				command.AddScript("$PID");
				powerShell.Commands = command; ;
				Collection<PSObject> results = powerShell.Invoke();
			}
			catch (Exception ex)
			{
				writeTologFile(ex.Message.ToString());
				try
				{
					PSCommand command = new PSCommand();
					command.AddScript("$PID");
					PSObj.PS.Commands = command; ;
					Collection<PSObject> results = PSObj.PS.Invoke();
				}
				catch (Exception ex2)
				{
				}
				PSObj.Connected = false;
			}
			return PSObj;
		}

		public void testO365ServerConnectivity(string userName, string pwd)
		{

			if (Convert.ToBoolean(ConfigurationManager.AppSettings["PingTest"].ToString()))
			{
				writeTologFile("Performing Ping Test");
				doPingTest();
			}
			ReturnPowerShellObjects results = DoO365LogonTests(userName, pwd, "-CommandName Test-MAPIConnectivity,Get-MailboxActivityReport,Get-Mailbox,Get-MailboxStatistics,Get-MsolAccountSku,Get-MsolUser,Get-User,Get-DistributionGroup,Get-MobileDeviceStatistics,Get-MobileDevice,Get-MsolCompanyInformation,Get-CsActiveUserReport,Get-CsClientDeviceReport,Get-CsP2PAVTimeReport,Get-CsConferenceReport,Get-CsP2PSessionReport");
			using (results)
			{
				writeTologFile("All tests Complete. Will Dispose off PS");
			}
		}
		public string submitRequest(string URL, string requestMethod, string message, string userId, string pwd)
		{
			string responseFromServer = "";
			writeTologFile("Submit Data.");
			try
			{


				//SharePointOnlineCredentials creds = new SharePointOnlineCredentials(userId, Common.String2SecureString(pwd));
				System.Net.WebRequest request = System.Net.WebRequest.Create(URL);
				System.Net.CredentialCache c = new System.Net.CredentialCache();
				request.Credentials = BuildCredentials(URL, userId, pwd, "BASIC");
				//request.Credentials = creds;
				//System.Net.WebClient wc = new System.Net.WebClient();

				request.Method = requestMethod;
				//request.Headers.Add("X-FORMS_BASED_AUTH_ACCEPTED: f");

				if (requestMethod == "POST")
				{
					request.ContentType = "application/json";
					string s = "" + (char)34;
					message = message.Replace("'", s);
					byte[] bytes = System.Text.Encoding.ASCII.GetBytes(message);
					request.ContentLength = bytes.Length;
					System.IO.Stream os = request.GetRequestStream();
					os.Write(bytes, 0, bytes.Length); //Push it out there
					os.Close();
				}
				//SharePointOnlineCredentials 
				//Microsoft.SharePoint.Client.ClientRuntimeContext.SetupRequestCredential(

				System.Net.WebResponse ws = request.GetResponse();
				Stream dataStream = ws.GetResponseStream();
				// Open the stream using a StreamReader for easy access.
				StreamReader reader = new StreamReader(dataStream);
				responseFromServer = reader.ReadToEnd();
			}
			catch (Exception ex)
			{
				responseFromServer = "-1";
				string s = ex.Message.ToString();
				writeTologFile(s);
			}
			return responseFromServer;
		}

		private System.Net.ICredentials BuildCredentials(string siteurl, string username, string password, string authtype)
		{
			System.Net.NetworkCredential cred;
			if (username.Contains(@"\"))
			{
				string domain = username.Substring(0, username.IndexOf(@"\"));
				username = username.Substring(username.IndexOf(@"\") + 1);
				cred = new System.Net.NetworkCredential(username, password, domain);
			}
			else
			{
				cred = new System.Net.NetworkCredential(username, password);
			}
			System.Net.CredentialCache cache = new System.Net.CredentialCache();
			if (authtype.Contains(":"))
			{
				authtype = authtype.Substring(authtype.IndexOf(":") + 1); //remove the TMG: prefix
			}
			cache.Add(new Uri(siteurl), authtype, cred);
			return cache;
		}

		private void BGW2_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
		{
		}

		private void BGW_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
		{
			Tray.Text = Convert.ToString(e.UserState);
			// change the current tooltip on the tray

			// you can also make a balloon appear over your TrayIcon:
			Tray.ShowBalloonTip(Convert.ToInt32(TimeSpan.FromSeconds(10).TotalMilliseconds), "VitalSigns O365 Collector...", Convert.ToString(e.UserState), ToolTipIcon.Info);
		}
		private void BGW2_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
		{
			//frmp.progressText = e.UserState.ToString();
			//frmp.step = e.ProgressPercentage;
			//frmp.changeProgressStatus();
			//if (e.ProgressPercentage == 100)
			//    frmp.Close();
			//if (criticalError || isError)
			//{
			//frmp.Close();
			//MessageBox.Show(e.UserState.ToString() + Environment.NewLine + "Please check the Log File for more information!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
			//show another dialog or show the close button on progress window
			//}
		}

		private void ME_DblClick(object sender, System.EventArgs e)
		{
			//MessageBox.Show("test");
			//mdiMain main = new mdiMain();
			//main.ShowDialog();
		}
		private void CMS_Opening(object sender, System.ComponentModel.CancelEventArgs e)
		{
			//e.Cancel = this.Busy;
			// keep user from seeing the exit menu while we are "working"
			e.Cancel = false;
		}
		private void mnuExit_Click(object sender, System.EventArgs e)
		{
			if (!Busy)
			{
				Application.Exit();
			}
			else
			{
				Tray.ShowBalloonTip(Convert.ToInt32(TimeSpan.FromSeconds(10).TotalMilliseconds), " Busy!", "Try again..", ToolTipIcon.Info);
			}
		}
		private void MyContext_ThreadExit(object sender, System.EventArgs e)
		{
			Tray.Visible = false;
		}
		private void CMS_Configure(object sender, System.EventArgs e)
		{
			frmConfigure frmC = new frmConfigure();
			frmC.loadMe();
			frmC.ShowDialog();
		}
		private void CMS_CheckForUpdates(object sender, System.EventArgs e)
		{
			//Tray.ShowBalloonTip(Convert.ToInt32(TimeSpan.FromSeconds(10).TotalMilliseconds), " Busy!", "Try again..", ToolTipIcon.Info);
			//checkAndGetUpdates(1);
			//BGW.ReportProgress(0, "Sleeping");

			BGW2.DoWork += BGW2_DoWork;
			BGW2.ProgressChanged += BGW2_ProgressChanged;
			BGW2.WorkerReportsProgress = true;
			BGW2.WorkerSupportsCancellation = true;
			BGW2.RunWorkerAsync();
			
			//frmp.Show();
		}
		#endregion
		#region Load
		public MyContext()
		{
			
			ThreadExit += MyContext_ThreadExit;
			Tray.Icon = Properties.Resources.VitalSigns;
			Tray.Visible = true;
			CMS.Items.Add(mnuExit);
			CMS.Items.Add(mnuConfigure);
			CMS.Items.Add(mnuShow);
			//CMS.Items.Add(mnuCheckUpdates);
			Tray.ContextMenuStrip = CMS;
			setEvents();
			BGW.WorkerReportsProgress = true;
			BGW.RunWorkerAsync();
		}
		private void setEvents()
		{
			Tray.DoubleClick += ME_DblClick;
			CMS.Opening += CMS_Opening;
			mnuExit.Click += mnuExit_Click;
			mnuConfigure.Click += CMS_Configure;
			mnuCheckUpdates.Click += CMS_CheckForUpdates;
			mnuCheck.Click += CMS_CheckForUpdates;
			mnuShow.Click += new EventHandler(mnuShow_Click);
			BGW.DoWork += BGW_DoWork;
			BGW.ProgressChanged += BGW_ProgressChanged;
			string sTime = "11:15";
			string Hr = sTime.Split(':')[0].ToString().Trim();
			string min = sTime.Split(':')[1].ToString().Trim();
			try
			{
				RunTime = new TimeSpan(Convert.ToInt32(Hr), Convert.ToInt32(min), 0); //11:15

			}
			catch (Exception ex)
			{
				throw ex;
			}

		}
		void mnuShow_Click(object sender, EventArgs e)
		{
			string URL = ConfigurationManager.AppSettings["URL"].ToString();
			if ( URL!= "")
				System.Diagnostics.Process.Start(URL);
		}
		#endregion
		#region helpers
		private void writeTologFile(string sText)
		{
			
			string logFolder = AppDomain.CurrentDomain.BaseDirectory.ToString() + "VSCollector.log";
			try
			{

				if (logFolder.Trim() != "")
				{
					//logFolder = logFolder.EndsWith("\\") ? logFolder : logFolder + "\\";
					if (!File.Exists(logFolder))
					{
						try
						{
							using (FileStream f = File.Create(logFolder))
							{
								while (!f.CanWrite)
								{
								}
							}
						}
						catch (Exception ex)
						{
							MessageBox.Show(ex.InnerException.ToString());

						}
					}
				}

				if (logFolder.Trim() == "")
					return;
				
			}
			catch
			{
			}
			string sLogFormat = DateTime.Now.ToShortDateString().ToString() + " " + DateTime.Now.ToLongTimeString().ToString() + " ==> ";
			try
			{
				StreamWriter sw = new StreamWriter(logFolder, true);
				sw.WriteLine(sLogFormat + sText);
				sw.Flush();
				sw.Close();
			}
			catch
			{

			}

		}
		private void reportProgress(string progressText)
		{
			//string logFolder = ConfigurationManager.AppSettings["logFile"].ToString();
			 writeTologFile(progressText);
			if (!stopIncrementingSteps)
				step += 1;

			if (updateStyle == 1)
				BGW2.ReportProgress(step, progressText);
			else
				if (progressText.Length > 63)
					BGW.ReportProgress(step, progressText.Substring(1, 63));
				else
					BGW.ReportProgress(step, progressText);
			if (step >= 100)
				Thread.Sleep(3000);

			//if (updateStyle == 1)
			//    BGW2.CancelAsync();
		}
		private void prepareLogFileName()
		{
			string sYear = DateTime.Now.Year.ToString();
			string sMonth = DateTime.Now.Month.ToString();
			string sDay = DateTime.Now.Day.ToString();
			string sTime = DateTime.Now.Hour.ToString() + "-" + DateTime.Now.Minute.ToString();
			logFileName = sYear + "-" + sMonth + "-" + sDay + "-" + sTime + ".txt";
		}
		private string checkCriticalError(bool rollBack)
		{
			if (criticalError)
				reportProgress("Critical Error Encountered..");
			//if back up was unsuccessful, nothing much to do

			//if the control reaches here, then the problem might be after the backup. in this case, we need to roll back to backup
			try
			{
				if (rollBack)
				{
					stopIncrementingSteps = true;
					//restore the backup
					step = 5;
					//restoreBackup();
					stopIncrementingSteps = false;
					step = 40;
					//check if scripts have started executing
					//if (isScriptsExecInProgress || isScriptsExecSucess)
					//{
					//    string restoreDbError = restoreDatabaseScripts();
					//    if (restoreDbError != "")
					//    {
					//        return restoreDbError;
					//        //todo: restore the database, that were backed up
					//        //restoreDB();
					//    }
					//}
					step = 70;
					//stopIncrementingSteps = false;
				}
				return "";
			}
			catch (Exception ex)
			{
				return "RESTORE from backup FAILED. Please contact Administrator to manually restore the application: Error:" + ex.Message.ToString();
			}
		}
		private void resetAllErrorFlags()
		{
			
		}
		private DateTime GetNextRun(TimeSpan time)
		{
			DateTime dt = DateTime.Today.Add(time);
			if (DateTime.Now > dt)
			{
				dt = dt.AddDays(1);
			}

			// Optional -- If you want to skip Saturday and Sunday:
			while (dt.DayOfWeek == DayOfWeek.Saturday || dt.DayOfWeek == DayOfWeek.Sunday)
			{
				dt = dt.AddDays(1);
			}

			return dt;
		}

		private byte[] iv = {
	65,
	110,
	68,
	1,
	69,
	178,
	200,
	235
	//Initialization Vector
};
		private byte[] key = {
	8,
	2,
	11,
	4,
	5,
	6,
	7,
	8,
	4,
	10,
	11,
	12,
	13,
	21,
	15,
	16,
	17,
	18,
	2,
	20,
	21,
	16,
	16,
	24
	//Encryption Key
};
		public string Decrypt(byte[] inputInBytes)
		{
			string functionReturnValue = null;
			if (inputInBytes == null)
				return functionReturnValue;
			// UTFEncoding is used to transform the decrypted Byte Array
			// information back into a string.
			UTF8Encoding utf8encoder = new UTF8Encoding();
			TripleDESCryptoServiceProvider tdesProvider = new TripleDESCryptoServiceProvider();

			// As before we must provide the encryption/decryption key along with
			// the init vector.
			ICryptoTransform cryptoTransform = tdesProvider.CreateDecryptor(this.key, this.iv);

			// Provide a memory stream to decrypt information into
			MemoryStream decryptedStream = new MemoryStream();
			CryptoStream cryptStream = new CryptoStream(decryptedStream, cryptoTransform, CryptoStreamMode.Write);
			cryptStream.Write(inputInBytes, 0, inputInBytes.Length);
			cryptStream.FlushFinalBlock();
			decryptedStream.Position = 0;

			// Read the memory stream and convert it back into a string
			byte[] result = new byte[decryptedStream.Length];
			decryptedStream.Read(result, 0, Convert.ToInt32(decryptedStream.Length));
			cryptStream.Close();
			UTF8Encoding myutf = new UTF8Encoding();
			return myutf.GetString(result);
			return functionReturnValue;
		}
		public byte[] Encrypt(string plainText)
		{
			// Declare a UTF8Encoding object so we may use the GetByte
			// method to transform the plainText into a Byte array.
			UTF8Encoding utf8encoder = new UTF8Encoding();
			byte[] inputInBytes = utf8encoder.GetBytes(plainText);

			// Create a new TripleDES service provider
			TripleDESCryptoServiceProvider tdesProvider = new TripleDESCryptoServiceProvider();

			// The ICryptTransform interface uses the TripleDES
			// crypt provider along with encryption key and init vector
			// information
			ICryptoTransform cryptoTransform = tdesProvider.CreateEncryptor(this.key, this.iv);

			// All cryptographic functions need a stream to output the
			// encrypted information. Here we declare a memory stream
			// for this purpose.
			MemoryStream encryptedStream = new MemoryStream();
			CryptoStream cryptStream = new CryptoStream(encryptedStream, cryptoTransform, CryptoStreamMode.Write);

			// Write the encrypted information to the stream. Flush the information
			// when done to ensure everything is out of the buffer.
			cryptStream.Write(inputInBytes, 0, inputInBytes.Length);
			cryptStream.FlushFinalBlock();
			encryptedStream.Position = 0;

			// Read the stream back into a Byte array and return it to the calling
			// method.
			byte[] result = new byte[encryptedStream.Length];
			encryptedStream.Read(result, 0, Convert.ToInt32(encryptedStream.Length));
			cryptStream.Close();
			return result;
		}
		#endregion
		

		
		


	}

}
