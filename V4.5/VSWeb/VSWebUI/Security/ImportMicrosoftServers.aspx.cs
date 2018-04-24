using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using VSWebDO;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Management.Automation.Remoting;
using System.Collections;
using System.Security;

namespace VSWebUI.Security
{
	public partial class ImportMicrosoftServers : System.Web.UI.Page
	{
		protected void Page_PreInit(object sender, EventArgs e)
		{

			this.Master.contentCallEvent += new EventHandler(Master_ButtonClick);
		}

		private void Master_ButtonClick(object sender, EventArgs e)
		{

		}
		private string _ServerType = "";
		private string ServerType
		{
			get
			{
				if (ViewState["_ServerType"] == null)
				{
					ViewState.Add("_ServerType", _ServerType);
				}
				return ViewState["_ServerType"].ToString();
			}
			set { ViewState.Add("_ServerType", value); }
		}

		private string _Pwd = "";
		private string Pwd
		{
			get
			{
				if (ViewState["_Pwd"] == null)
				{
					ViewState.Add("_Pwd", _Pwd);
				}
				return ViewState["_Pwd"].ToString();
			}
			set { ViewState.Add("_Pwd", value); }
		}

		private string _Uid = "";
		private string Uid
		{
			get
			{
				if (ViewState["_Uid"] == null)
				{
					ViewState.Add("_Uid", _Pwd);
				}
				return ViewState["_Uid"].ToString();
			}
			set { ViewState.Add("_Uid", value); }
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			string ServerName;

			if (!IsPostBack)
			{
				if (Request.QueryString["ServerType"] != null)
					ServerType = Request.QueryString["ServerType"].ToString();
                if (ServerType == "Database Availability Group")
                {
                    //3/3/2016 NS modified for VSPLUS-2685
                    //SSLCheckBox.Visible = false;
                    ASPxLabel13.Visible = false;
                    RequireSSLRadioButtonList.Visible = false;
                }
				FillCredentialsComboBox();
				FillprofileComboBox();
				ServerName = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("Primary Exchange Server");
				if (ServerName != "")
				{
					ExchangeServerTextBox.Text = ServerName;
				}

			}


		}

		protected void CredentialsComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (CredentialsComboBox.Text != "")
			{
				UserIdtextBox.Text = "";
				UserIdtextBox.Enabled = false;
				PasswordTextbox.Enabled = false;
				UserIdtextBox.BackColor = System.Drawing.Color.LightGray;
				PasswordTextbox.BackColor = System.Drawing.Color.LightGray;
				ASPxButton1.Visible = false;
				ASPxLabel6.Visible = false;
			}
			else
			{
				UserIdtextBox.Enabled = true;
				PasswordTextbox.Enabled = true;
				UserIdtextBox.BackColor = System.Drawing.Color.White;
				PasswordTextbox.BackColor = System.Drawing.Color.White;
				ASPxButton1.Visible = true;
				ASPxLabel6.Visible = true;
			}
		}
		//    Credentials c = new Credentials();
		//    c.AliasName = CredentialsComboBox.Text;
		//    DataTable Dt = VSWebBL.ConfiguratorBL.CredentialsBL.Ins.getCredentialsByName(c);
		//    UserIdtextBox.Text = Dt.Rows[0]["UserID"].ToString();
		//    VSFramework.TripleDES tripleDes = new VSFramework.TripleDES();
		//    string MyObjPwd;
		//    string[] MyObjPwdArr;
		//    byte[] MyPass;
		//    MyObjPwd = Dt.Rows[0]["Password"].ToString();
		//    if (MyObjPwd != "")
		//    {
		//        MyObjPwdArr = MyObjPwd.Split(',');
		//        MyPass = new byte[MyObjPwdArr.Length];
		//        for (int i = 0; i < MyObjPwdArr.Length; i++)
		//        {
		//            MyPass[i] = Byte.Parse(MyObjPwdArr[i]);
		//        }
		//        PasswordTextbox.Text = tripleDes.Decrypt(MyPass);
		//        //ViewState.Add("Pwd", PasswordTextbox.Text);
		//        addPwd = tripleDes.Decrypt(MyPass);
		//    }
		//    //getCredentials();
		//}

		private ArrayList getCredentials()
		{
			ArrayList Cred = new ArrayList();
			Credentials c = new Credentials();
			c.ID = Convert.ToInt32(CredentialsComboBox.Value);
			Session["importcredential"] = CredentialsComboBox.Text;
			DataTable Dt = VSWebBL.ConfiguratorBL.CredentialsBL.Ins.getCredentialsById(c);
			Cred.Add(Dt.Rows[0]["UserID"].ToString());
			//UserIdtextBox.Text = Dt.Rows[0]["UserID"].ToString();
			VSFramework.TripleDES tripleDes = new VSFramework.TripleDES();
			string MyObjPwd;
			string[] MyObjPwdArr;
			byte[] MyPass;
			MyObjPwd = Dt.Rows[0]["Password"].ToString();
			if (MyObjPwd != "")
			{
				MyObjPwdArr = MyObjPwd.Split(',');
				MyPass = new byte[MyObjPwdArr.Length];
				for (int i = 0; i < MyObjPwdArr.Length; i++)
				{
					MyPass[i] = Byte.Parse(MyObjPwdArr[i]);
				}
				//PasswordTextbox.Text = tripleDes.Decrypt(MyPass);
				Cred.Add(tripleDes.Decrypt(MyPass));
				//PasswordTextbox.Text = tripleDes.Decrypt(MyPass);
				//ViewState.Add("Pwd", PasswordTextbox.Text);
			}
			return Cred;
		}

		public void CheckServers()
		{
			if (ServerType != "" && (ServerType == "Exchange" || ServerType == "Skype for Business" || ServerType == "Database Availability Group") && (ExchangeServerTextBox.Text.ToLower().Contains("http://") == false && ExchangeServerTextBox.Text.ToLower().Contains("https://") == false))
			{
				//10/6/2014 NS modified for VSLPLUS-934
				errorDiv.InnerHtml = "Not a valid Server Address. Server Address should be in the form of 'https://myServer.com'" +
					"<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
				errorDiv.Style.Value = "display: block";
				return;
			}

			//DataTable dt = new DataTable();
			//string strError = "";
			string UserName = "";
			string Password = "";

			if (CredentialsComboBox.Text != "")
			{
				ArrayList Cred = getCredentials();
				if (Cred.Count > 1)
				{
					UserName = Cred[0].ToString();
					Password = Cred[1].ToString();

					if (UserName == "" || Password == "")
					{
						//10/6/2014 NS modified for VSPLUS-990
						errorDiv.InnerHtml = "User ID or Password not set for the selected Credentials." +
							"<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
						errorDiv.Style.Value = "display: block";
						return;
					}

				}
			}
			else
			{
				UserName = UserIdtextBox.Text;// "jnittech\\administrator";
				Password = PasswordTextbox.Text;// "Pa$$w0rd";
				if (UserName == "" || Password == "")
				{
					//10/6/2014 NS modified for VSPLUS-990
					errorDiv.InnerHtml = "Please select Credentials or enter User ID and Password." +
						"<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
					errorDiv.Style.Value = "display: block";
					return;
				}
			}
			Uid = UserName;
			Pwd = Password;

			if (ServerType == "" && ServerType==null)
			{
				errorDiv.InnerHtml = "Please select a Server Type from the drop down box." +
										"<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
				errorDiv.Style.Value = "display: block";
				return;
			}

			string IPAddress = ExchangeServerTextBox.Text.Trim();// "https://jnittech-exchg1.jnittech.com";

			switch (ServerType)
			{
				case "Exchange":
					LoadExchangeServers(IPAddress, Password, UserName);
					break;
				case "Database Availability Group":
					LoadExchangeDAG(IPAddress, Password, UserName);
					break;
				case "Active Directory":
					LoadActiveDirectoryServers(IPAddress, Password, UserName);
					break;
				case "SharePoint":
					LoadSharePointServers(IPAddress, Password, UserName);
					break;
				case "Windows":
					LoadActiveDirectoryServers(IPAddress, Password, UserName);
					break;
				case "Skype for Business":
					LoadLyncServers(IPAddress, Password, UserName);
					break;
			}
		}




		protected void ImportButton_Click(object sender, EventArgs e)
		{
			if(CredentialsComboBox.Text == "")
				Session["importcredential"] = "";
			ImportServers();
		}
		protected void CreateCredentialsImportButton_Click(object sender, EventArgs e)
		{
			if (Uid == "" || Pwd == "")
			{
				//10/6/2014 NS modified for VSPLUS-990
				errorDiv.InnerHtml = "Please enter User ID and Password." +
					"<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
				errorDiv.Style.Value = "display: block";
				return;
			}
			CreateCredentials();
			ImportServers();
			

		}
		private void ImportServers()
		{
			//1. Check whether selected servers have already been imported
			//2. If the servers are not in the table, import server and location info
			Object ReturnValue;
			Servers ServersObject;
			DataTable dtsrv = new DataTable();
			if (LocComboBox.SelectedIndex != -1)
			{
				if (LocComboBox.SelectedItem.Value.ToString() != "")
				{
					if (SrvCheckBoxList.SelectedItems.Count > 0)
					{
						dtsrv.Columns.Add("ID");
						dtsrv.Columns.Add("ServerName");
						dtsrv.Columns.Add("IPAddress");
						dtsrv.Columns.Add("Description");
						dtsrv.Columns.Add("ServerType");
						dtsrv.Columns.Add("Location");
						dtsrv.Columns.Add("LocationID");
						dtsrv.Columns.Add("ProfileName");
						//5/16/2013 NS modified
						//for (int i = 0; i < SrvCheckBoxList.SelectedItems.Count; i++)
						string SSL = "https://";
                        //3/3/2016 NS modified for VSPLUS-2685
                        /*
						if (SSLCheckBox.Checked && SSLCheckBox.IsVisible())
							SSL = "https://";
						else if (SSLCheckBox.IsVisible())
							SSL = "http://";
						else
							SSL = "";
                        */
                        if (RequireSSLRadioButtonList.SelectedIndex != -1 && RequireSSLRadioButtonList.IsVisible())
                        {
                            if (RequireSSLRadioButtonList.SelectedItem.Value.ToString() == "1")
                            {
                                SSL = "https://";
                            }
                            else
                            {
                                SSL = "http://";
                            }
                        }
                        else
                        {
                            SSL = "";
                        }
						for (int i = 0; i < SrvCheckBoxList.Items.Count; i++)
						{
							if (SrvCheckBoxList.Items[i].Selected)
							{
								DataRow dr = dtsrv.NewRow();
								dr["ID"] = "";
								dr["ServerName"] = SrvCheckBoxList.Items[i].ToString();
								if (ServerType == "DAG")
								{
									dr["IPAddress"] = SrvCheckBoxList.Items[i].ToString();
								}
								else
								{
									if (IPCheckBoxList.Items[i].Text.ToLower().Contains("http") == false)
										dr["IPAddress"] = SSL + IPCheckBoxList.Items[i].Text;
									else
										dr["IPAddress"] = IPCheckBoxList.Items[i].Text;
								}
								if (IPCheckBoxList.Items[i].Text == "")
								{
									dr["IPAddress"] = "dummyaddress.yourdomain.com";
								}
								dr["Description"] = "Production";
								dr["ServerType"] = ServerType;
								//if (ServerType == "DAG")
								//    dr["ServerType"] = "Database Availability Group";
								//else
								//    dr["ServerType"] = "Exchange";
								dr["Location"] = LocComboBox.SelectedItem.Value.ToString();
								dr["LocationID"] = LocIDComboBox.Items[LocComboBox.SelectedIndex].Text;
								dr["ProfileName"] = ProfileComboBox.SelectedItem.Value.ToString();
								dtsrv.Rows.Add(dr);
								ServersObject = CollectDataForServers("Insert", dr);
								DataTable dt = VSWebBL.SecurityBL.ServersBL.Ins.GetDataByName(ServersObject);
								if (dt.Rows.Count == 0)
								{
									ReturnValue = VSWebBL.SecurityBL.ServersBL.Ins.InsertData(ServersObject);
								}
								else
								{
									dt.Rows[0]["LocationID"] = LocIDComboBox.Items[LocComboBox.SelectedIndex].Text;
									dt.Rows[0]["Location"] = LocComboBox.SelectedItem.Value.ToString();
									ServersObject = CollectDataForServers("Update", dt.Rows[0]);
									ReturnValue = VSWebBL.SecurityBL.ServersBL.Ins.UpdateData(ServersObject);
								}
							}
						}
						Session["ImportedServers"] = dtsrv;

						//if (ServerType == "DAG")
						//	Response.Redirect("~/Security/ImportExchangeServers2.aspx?ServerType=DAG", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
						//else
						//Response.Redirect("~/Security/ImportExchangeServers2.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
						string URL = "~/Security/ImportMicrosoftServers2.aspx?ServerType=" + ServerType.Replace(" ", "_") + "";
						if (ServerType == "Exchange")
							URL += "&AuthType=" + AuthenticationTypeComboBox.Text.ToString();
                        URL+="&ProfileName=" +ProfileComboBox.Text.ToString();
						Response.Redirect(URL, false);
						Context.ApplicationInstance.CompleteRequest();
					}
				}
			}
		}
		private void CreateCredentials()
		{
			Object ReturnValue = VSWebBL.ConfiguratorBL.CredentialsBL.Ins.InsertData(CollectDataForCredentials());
			Session["importcredential"] = ServerType + "-" + LocComboBox.Text;
		}
		private Credentials CollectDataForCredentials()
		{
			VSFramework.TripleDES tripleDes = new VSFramework.TripleDES();
			try
			{
				Credentials CredentialsObject = new Credentials();
				CredentialsObject.AliasName = ServerType + "-" + LocComboBox.Text;
				CredentialsObject.UserID = Uid;

				string rawPass = Pwd;
				byte[] encryptedPass = tripleDes.Encrypt(rawPass);
				string encryptedPassAsString = string.Join(", ", encryptedPass.Select(s => s.ToString()).ToArray());

				CredentialsObject.Password = encryptedPassAsString;


				return CredentialsObject;
			}
			catch (Exception ex)
			{
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}
			finally { }
		}
		private Servers CollectDataForServers(string Mode, DataRow ServersRow)
		{
			try
			{
				Servers ServersObject = new Servers();
				if (Mode == "Update")
				{
					ServersObject.ID = int.Parse(ServersRow["ID"].ToString());
				}
				ServersObject.ServerName = ServersRow["ServerName"].ToString();
				ServersObject.IPAddress = ServersRow["IPAddress"].ToString();
				ServersObject.Description = ServersRow["Description"].ToString();
				ServerTypes STypeobject = new ServerTypes();
				STypeobject.ServerType = ServersRow["ServerType"].ToString();
				ServersObject.LocationID = int.Parse(ServersRow["LocationID"].ToString());
				ServerTypes ReturnValue = VSWebBL.SecurityBL.ServerTypesBL.Ins.GetDataForServerType(STypeobject);
				ServersObject.ServerTypeID = ReturnValue.ID;

				//DataTable ReturnValue = VSWebBL.SecurityBL.AdminTabBL.Ins.GetNavigatorByDisplayText(ServersRow["ServerType"].ToString());
				//ServersObject.ServerTypeID =int.Parse(ReturnValue.Rows[0]["ID"].ToString());

				//ServersObject.ServerTypeID = ServerTypeComboBox.Text;
				// ServersObject.LocationID = int.Parse(ServersRow["LocationID"].ToString());
				//ServersObject.ServerTypeID = int.Parse(ServersRow["ServerType"].ToString());
				//ServersObject.LocationID = int.Parse(ServersRow["Location"].ToString());
				Locations LOCobject = new Locations();
				LOCobject.Location = ServersRow["Location"].ToString();

				Locations ReturnLocValue = VSWebBL.SecurityBL.LocationsBL.Ins.GetDataForLocation(LOCobject);
				ServersObject.LocationID = ReturnLocValue.ID;
				ProfileNames proobject = new ProfileNames();
				proobject.ProfileName = ServersRow["ProfileName"].ToString();
				ServersObject.ProfileName = proobject.ProfileName;


				return ServersObject;
			}
			catch (Exception ex)
			{
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}
			finally { }
		}

		protected void DoneButton_Click(object sender, EventArgs e)
		{
			string ProfileName = "";
			if (ProfileComboBox.SelectedIndex < 0)
			{
				ProfileName = "Default";
			}
			else
			{
				ProfileComboBox.SelectedItem.Text = ProfileComboBox.SelectedItem.Text;
			}
			Response.Redirect("~/Security/ImportMicrosoftServers2.aspx?ProfileName=" + ProfileName, false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
			Context.ApplicationInstance.CompleteRequest();
		}

		protected void SelectAllButton_Click(object sender, EventArgs e)
		{
			SrvCheckBoxList.SelectAll();
		}

		protected void DeselectAllButton_Click(object sender, EventArgs e)
		{
			SrvCheckBoxList.UnselectAll();
		}

		protected void LoadServersButton_Click(object sender, EventArgs e)
		{
			ServerType = ServerTypeComboBox.Text;
			if (ServerType == "Exchange" || ServerType == "Skype for Business")
			{
                //3/3/2016 NS modified for VSPLUS-2685
                //SSLCheckBox.Visible = true;
                ASPxLabel13.Visible = true;
                RequireSSLRadioButtonList.Visible = true;
			}
			else
			{
                //3/3/2016 NS modified for VSPLUS-2685
				//SSLCheckBox.Visible = false;
                ASPxLabel13.Visible = false;
                RequireSSLRadioButtonList.Visible = false;
				infoDiv.Visible = false;
				ASPxRoundPanel1.Visible = false;
			}
			CheckServers();
		}

		/*protected void LoadDAGButton_Click(object sender, EventArgs e)
		{
			//LoadDominoServers();
			//PrereqForExchange("", "", "", "");
			//LoadExchangeServers();
			ServerType = "DAG";
			LoadExchangeDAG();
			SSLCheckBox.Visible = false;
			//12/20/2013 NS modified - make the panel visible if the server list is loaded
			//ASPxRoundPanel1.Visible = true;
		}*/

		public SecureString String2SecureString(string password)
		{
			SecureString remotePassword = new SecureString();
			for (int i = 0; i < password.Length; i++)
				remotePassword.AppendChar(password[i]);

			return remotePassword;
		}
		private void FillCredentialsComboBox()
		{
			DataTable CredentialsDataTable = VSWebBL.ConfiguratorBL.ServicesBL.Ins.GetCredentials();
			CredentialsComboBox.DataSource = CredentialsDataTable;


			CredentialsComboBox.TextField = "AliasName";
			CredentialsComboBox.ValueField = "ID";
			CredentialsComboBox.DataBind();
			CredentialsComboBox.Items.Add("", "");

			//CredentialsComboBox.Items[CredentialsComboBox.Items.Count - 1].Selected = true;

		}
		private void FillprofileComboBox()
		{
			DataTable ProfileNamesDataTable = VSWebBL.SecurityBL.ProfilesNamesBL.Ins.GetAllData();
			ProfileComboBox.DataSource = ProfileNamesDataTable;
			ProfileComboBox.TextField = "ProfileName";
			ProfileComboBox.ValueField = "ID";
			ProfileComboBox.DataBind();
		}


		public void LoadExchangeDAG(string IPAddress, string Password, string UserName)
		{
			DataTable dt = new DataTable();
			string strError = "";
			//ViewState["Pwd"] = Password;
			//ViewState.Add("Pwd", Password);
			bool updatedsrv = VSWebBL.SettingBL.SettingsBL.Ins.UpdateSvalue("Primary Exchange Server", ExchangeServerTextBox.Text, VSWeb.Constants.Constants.SysString);
			System.Uri uri = new Uri(IPAddress + "/powershell?serializationLevel=Full");
			System.Security.SecureString securePassword = String2SecureString(Password);
			PSCredential creds = new PSCredential(UserName, securePassword);
			Runspace runspace = RunspaceFactory.CreateRunspace();
			PowerShell powershell = PowerShell.Create();

			PSCommand command = new PSCommand();
			command.AddCommand("New-PSSession");
			command.AddParameter("ConfigurationName", "Microsoft.Exchange");
			command.AddParameter("ConnectionUri", uri);
			command.AddParameter("Credential", creds);
            command.AddParameter("Authentication", AuthenticationTypeComboBox.Text.ToString());
			System.Collections.ObjectModel.Collection<PSObject> results = new System.Collections.ObjectModel.Collection<PSObject>();

			PSSessionOption sessionOption = new PSSessionOption();
			sessionOption.SkipCACheck = true;
			sessionOption.SkipCNCheck = true;
			sessionOption.SkipRevocationCheck = true;

			command.AddParameter("SessionOption", sessionOption);
			powershell.AddScript(@"set-executionpolicy unrestricted");
			powershell.Commands = command;

			try
			{
				using (runspace)
				{
					runspace.Open();
					powershell.Runspace = runspace;
					System.Collections.ObjectModel.Collection<PSSession> result = powershell.Invoke<PSSession>();

					if (result.Count == 0)
					{
						foreach (ErrorRecord current in powershell.Streams.Error)
						{
							throw new Exception(strError + "Exception Importing Servers: " + current.Exception.ToString() + ",\r\nInner Exception: " + current.Exception.InnerException);
							//strError += "Exception Importing Servers: " + current.Exception.ToString() + ",\r\nInner Exception: " + current.Exception.InnerException;
							//errorDiv.InnerHtml = strError;
							//errorDiv.Style.Value = "display: block";
							//return;
						}
					}
					PSSession pssession = (PSSession)result[0];
					command = new PSCommand();
					command.AddCommand("Set-Variable");
					command.AddParameter("Name", "ra");
					command.AddParameter("Value", result[0]);
					powershell.Commands = command; ;
					powershell.Invoke();


					command = new PSCommand();
					command.AddScript("Import-PSSession -Session $ra -CommandName Get-DatabaseAvailabilityGroup, Test-ReplicationHealth, Get-MailboxDatabase, Get-MailboxDatabaseCopyStatus");
					powershell.Commands = command;
					powershell.Invoke();

					if (powershell.Streams.Error.Count > 0)
					{
						string searchMsg = "Running the Get-Command command in a remote session returned no results";
						if (powershell.Streams.Error.Where(record => record.Exception.ToString().Contains(searchMsg)).ToArray().Length > 0)
						{
							throw new Exception("Failed to load the module on the specified server.  Please ensure the Exchange PowerShell Module is installed on the specified server");
						}
					}

					powershell.Streams.Error.Clear();

					String str = "Get-DatabaseAvailabilityGroup | Select-Object -Property Name,WitnessServer | sort Name";
					powershell.AddScript(str);

					results = powershell.Invoke();

					dt.Columns.Add("ServerName", typeof(string));
					dt.Columns.Add("IPAddress", typeof(string));

					foreach (PSObject ps in results)
					{
						try
						{
							string name = ps.Properties["Name"].Value.ToString();
							string WitnessServer = ps.Properties["WitnessServer"].Value.ToString();

							DataRow dr = dt.NewRow();
							dr["ServerName"] = name;
							dr["IPAddress"] = WitnessServer;
							dt.Rows.Add(dr);
						}
						catch (Exception ex)
						{
							throw ex;
							//10/6/2014 NS modified for VSPLUS-990
							//errorDiv.InnerHtml = "The following error has occurred: " + ex.Message +
							//	"<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
							//errorDiv.Style.Value = "display: block";
							//errorinfoDiv.Style.Value = "display: block";
							//Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
							//return;
						}

					}


					if (pssession != null)
					{
						Command cmd = new Command("remove-pssession");
						cmd.Parameters.Add("id", pssession.Id);
						powershell.Commands.Clear();
						powershell.Commands.AddCommand(cmd);
						powershell.Invoke();
					}
					if (runspace != null)
					{
						if (runspace.RunspaceStateInfo.State == RunspaceState.Opened)
						{
							runspace.Close();
							runspace.Dispose();
							powershell.Dispose();
						}
					}

					FillServerSelectonScreen(dt);


				}
				GC.Collect();
			}
			catch (Exception ex)
			{
				//errorDiv.InnerHtml = "An error has occurred and has been logged.";
				//errorDiv.Style.Value = "display: block";
				//LogErrorMessage(ex.Message.ToString());
				HandleImportError(ex);
				return;
			}


		}

		public void LoadExchangeServers(string IPAddress, string Password, string UserName)
		{
			DataTable dt = new DataTable();
			string strError = "";
			//ViewState["Pwd"] = Password;
			//ViewState.Add("Pwd", Password);
			bool updatedsrv = VSWebBL.SettingBL.SettingsBL.Ins.UpdateSvalue("Primary Exchange Server", ExchangeServerTextBox.Text, VSWeb.Constants.Constants.SysString);
			System.Uri uri = new Uri(IPAddress + "/powershell?serializationLevel=Full");
			System.Security.SecureString securePassword = String2SecureString(Password);
			PSCredential creds = new PSCredential(UserName, securePassword);
			Runspace runspace = RunspaceFactory.CreateRunspace();
			PowerShell powershell = PowerShell.Create();

			PSCommand command = new PSCommand();
			command.AddCommand("New-PSSession");
			command.AddParameter("ConfigurationName", "Microsoft.Exchange");
			command.AddParameter("ConnectionUri", uri);
			command.AddParameter("Credential", creds);
			command.AddParameter("Authentication", AuthenticationTypeComboBox.Text.ToString());
			System.Collections.ObjectModel.Collection<PSObject> results = new System.Collections.ObjectModel.Collection<PSObject>();

			PSSessionOption sessionOption = new PSSessionOption();
			sessionOption.SkipCACheck = true;
			sessionOption.SkipCNCheck = true;
			sessionOption.SkipRevocationCheck = true;

			command.AddParameter("SessionOption", sessionOption);
			powershell.AddScript(@"set-executionpolicy unrestricted");
			powershell.Commands = command;

			try
			{
				using (runspace)
				{
					runspace.Open();
					powershell.Runspace = runspace;
					System.Collections.ObjectModel.Collection<PSSession> result = powershell.Invoke<PSSession>();

					if (result.Count == 0)
					{
						foreach (ErrorRecord current in powershell.Streams.Error)
						{
							throw new Exception(strError + "Exception Importing Servers: " + current.Exception.ToString() + ",\r\nInner Exception: " + current.Exception.InnerException);
							//strError += "Exception Importing Servers: " + current.Exception.ToString() + ",\r\nInner Exception: " + current.Exception.InnerException;
							//WesTest.Text = "Username: " + UserName + " ... Password: " + Password + " ... IP: " + uri.AbsoluteUri;
							//10/6/2014 NS modified for VSPLUS-990
							//errorDiv.InnerHtml = strError +
							//	"<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
							//errorDiv.Style.Value = "display: block";
							//return;
						}
					}
					PSSession pssession = (PSSession)result[0];
					command = new PSCommand();
					command.AddCommand("Set-Variable");
					command.AddParameter("Name", "ra");
					command.AddParameter("Value", result[0]);
					powershell.Commands = command; ;
					powershell.Invoke();


					command = new PSCommand();
					command.AddScript("Import-PSSession -Session $ra -CommandName Get-ExchangeServer -FormatTypeName *");
					powershell.Commands = command;
					powershell.Invoke();

					if (powershell.Streams.Error.Count > 0)
					{
						string searchMsg = "Running the Get-Command command in a remote session returned no results";
						if (powershell.Streams.Error.Where(record => record.Exception.ToString().Contains(searchMsg)).ToArray().Length > 0)
						{
							throw new Exception("Failed to load the module on the specified server.  Please ensure the Exchange PowerShell Module is installed on the specified server");
						}
					}

					powershell.Streams.Error.Clear();

					String str = "Get-ExchangeServer | select Name, Fqdn | sort name";
					powershell.AddScript(str);

					results = powershell.Invoke();

					dt.Columns.Add("ServerName", typeof(string));
					dt.Columns.Add("IPAddress", typeof(string));

					foreach (PSObject ps in results)
					{
						try
						{
							string name = ps.Properties["Name"].Value.ToString();
							string Fqdn = ps.Properties["Fqdn"].Value.ToString();

							DataRow dr = dt.NewRow();
							dr["ServerName"] = name;
							dr["IPAddress"] = Fqdn;
							dt.Rows.Add(dr);
						}
						catch (Exception ex)
						{
							throw ex;
							//10/6/2014 NS modified for VSPLUS-990
							//errorDiv.InnerHtml = "The following error has occurred: " + ex.Message +
							//	"<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
							//errorDiv.Style.Value = "display: block";
							//errorinfoDiv.Style.Value = "display: block";
							//Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
							//return;
						}

					}


					if (pssession != null)
					{
						Command cmd = new Command("remove-pssession");
						cmd.Parameters.Add("id", pssession.Id);
						powershell.Commands.Clear();
						powershell.Commands.AddCommand(cmd);
						powershell.Invoke();
					}
					if (runspace != null)
					{
						if (runspace.RunspaceStateInfo.State == RunspaceState.Opened)
						{
							runspace.Close();
							runspace.Dispose();
							powershell.Dispose();
						}
					}

					FillServerSelectonScreen(dt);


				}
				GC.Collect();
			}
			catch (Exception ex)
			{
				//10/6/2014 NS modified for VSPLUS-990
				//errorDiv.InnerHtml = "An error has occurred and has been logged." +
				//	"<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
				//errorDiv.Style.Value = "display: block";
				//LogErrorMessage(ex.Message.ToString());
				HandleImportError(ex);
				return;
			}


		}

		public void LoadSharePointServers(string IPAddress, string Password, string UserName)
		{
			DataTable dt = new DataTable();
			string strError = "";

			bool updatedsrv = VSWebBL.SettingBL.SettingsBL.Ins.UpdateSvalue("Primary Exchange Server", ExchangeServerTextBox.Text, VSWeb.Constants.Constants.SysString);
			//System.Uri uri = new Uri(IPAddress + "/powershell?serializationLevel=Full");
			System.Security.SecureString securePassword = String2SecureString(Password);
			PSCredential creds = new PSCredential(UserName, securePassword);
			PowerShell powershell = PowerShell.Create();


			Runspace runspace = RunspaceFactory.CreateRunspace();
			PSCommand command = new PSCommand();

			System.Collections.ObjectModel.Collection<PSObject> results = new System.Collections.ObjectModel.Collection<PSObject>();

			//powershell.Commands = command;

			try
			{
				using (runspace)
				{
					runspace.Open();
					powershell.Runspace = runspace;
					System.Collections.ObjectModel.Collection<PSSession> result;// = powershell.Invoke<PSSession>();

					command = new PSCommand();
					command.AddCommand("New-PSSession");
					command.AddParameter("ComputerName", IPAddress.Replace("https://", "").Replace("http://", ""));
					command.AddParameter("Credential", creds);
					command.AddParameter("Authentication", "Credssp");
					//command.AddParameter("Authentication", "Default");
					powershell.Commands.Clear();
					powershell.Streams.Error.Clear();
					powershell.Commands = command;
					result = powershell.Invoke<PSSession>();

					if (result.Count == 0)
					{
						foreach (ErrorRecord current in powershell.Streams.Error)
						{
							throw new Exception(strError + "Exceptino Importing Servers: " + current.Exception.ToString() + ",\r\nInner Exception: " + current.Exception.InnerException);
							//strError += "Exception Importing Servers: " + current.Exception.ToString() + ",\r\nInner Exception: " + current.Exception.InnerException;
							//errorDiv.InnerHtml = strError +
							//	"<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
							//errorDiv.Style.Value = "display: block";
							//return;
						}
					}

					PSSession pssession = (PSSession)result[0];
					command = new PSCommand();
					command.AddCommand("Set-Variable");
					command.AddParameter("Name", "ra");
					command.AddParameter("Value", result[0]);
					powershell.Commands = command; ;
					powershell.Invoke();

					// First import the cmdlets in the current runspace (using Import-PSSession)

					string script = "Get-SPServer | Foreach-Object { [System.Net.Dns]::GetHostByName($_.Address).HostName} | sort";
					powershell.Commands.Clear();
					powershell.Streams.ClearStreams();
					powershell.AddScript("Invoke-Command -Session $ra -ScriptBlock {Add-PSSnapin Microsoft.SharePoint.Powershell; " + script + "}");
					results = powershell.Invoke();

					if (powershell.Streams.Error.Count > 0)
					{
						string searchMsg = "No Windows PowerShell snap-ins matching the pattern ";
						if (powershell.Streams.Error.Where(record => record.Exception.ToString().Contains(searchMsg)).ToArray().Length > 0)
						{
							throw new Exception("Failed to load the PSSnapIn on the specified server.  Please ensure the SharePoint PowerShell PSSnapIn is installed on the specified server");
						}
					}

					if (powershell.Streams.Error.Count > 0)
					{
						string searchMsg = "is not recognized as the name of a cmdlet, function, script file, or operable program.";
						if (powershell.Streams.Error.Where(record => record.Exception.ToString().Contains(searchMsg)).ToArray().Length > 0)
						{
							throw new Exception("Failed to load the cmdlet on the specified server.  Please ensure the SharePoint PowerShell PSSnapIn is installed on the specified server");
						}
					}

					dt.Columns.Add("ServerName", typeof(string));
					dt.Columns.Add("IPAddress", typeof(string));

					foreach (PSObject ps in results)
					{
						try
						{
							string name = ps.BaseObject.ToString();

							DataRow dr = dt.NewRow();
							dr["ServerName"] = name;
							dr["IPAddress"] = name;
							dt.Rows.Add(dr);

						}
						catch (Exception ex)
						{
							throw ex;
							//10/6/2014 NS modified for VSPLUS-990
							//errorDiv.InnerHtml = "The following error has occurred: " + ex.Message +
							//	"<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
							//errorDiv.Style.Value = "display: block";
							//errorinfoDiv.Style.Value = "display: block";
							//Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
							//return;
						}

					}


					if (pssession != null)
					{
						Command cmd = new Command("remove-pssession");
						cmd.Parameters.Add("id", pssession.Id);
						powershell.Commands.Clear();
						powershell.Commands.AddCommand(cmd);
						powershell.Invoke();
					}
					if (runspace != null)
					{
						if (runspace.RunspaceStateInfo.State == RunspaceState.Opened)
						{
							runspace.Close();
							runspace.Dispose();
							powershell.Dispose();
						}
					}

					FillServerSelectonScreen(dt);


				}

				GC.Collect();
			}
			catch (Exception ex)
			{
				//10/6/2014 NS modified for VSPLUS-990
				//errorDiv.InnerHtml = "An error has occurred and has been logged." +
				//	"<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
				//errorDiv.Style.Value = "display: block";
				//LogErrorMessage(ex.Message.ToString());
				HandleImportError(ex);
				return;
			}



		}

		public void LoadActiveDirectoryServers(string IPAddress, string Password, string UserName)
		{
			DataTable dt = new DataTable();
			string strError = "";
			//ViewState["Pwd"] = Password;
			//ViewState.Add("Pwd", Password);
			bool updatedsrv = VSWebBL.SettingBL.SettingsBL.Ins.UpdateSvalue("Primary Exchange Server", ExchangeServerTextBox.Text, VSWeb.Constants.Constants.SysString);
			//System.Uri uri = new Uri(IPAddress + "/powershell?serializationLevel=Full");
			System.Security.SecureString securePassword = String2SecureString(Password);
			PSCredential creds = new PSCredential(UserName, securePassword);
			Runspace runspace = RunspaceFactory.CreateRunspace();
			PowerShell powershell = PowerShell.Create();

			PSCommand command = new PSCommand();
			command.AddCommand("New-PSSession");
			command.AddParameter("computer", IPAddress.Replace("https://", "").Replace("http://", ""));
			command.AddParameter("Credential", creds);

			powershell.Commands = command;
			System.Collections.ObjectModel.Collection<PSObject> results = new System.Collections.ObjectModel.Collection<PSObject>();

			try
			{
				using (runspace)
				{
					runspace.Open();
					powershell.Runspace = runspace;
					System.Collections.ObjectModel.Collection<PSSession> result = powershell.Invoke<PSSession>();

					if (result.Count == 0)
					{
						foreach (ErrorRecord current in powershell.Streams.Error)
						{
							throw new Exception(strError + "Exception Importing Servers: " + current.Exception.ToString() + ",\r\nInner Exception: " + current.Exception.InnerException);
							//strError += "Exception Importing Servers: " + current.Exception.ToString() + ",\r\nInner Exception: " + current.Exception.InnerException;
							//10/6/2014 NS modified for VSPLUS-990
							//errorDiv.InnerHtml = strError +
							//	"<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
							//errorDiv.Style.Value = "display: block";
							//return;
						}
					}
					PSSession pssession = (PSSession)result[0];
					command = new PSCommand();
					command.AddCommand("Set-Variable");
					command.AddParameter("Name", "ra");
					command.AddParameter("Value", result[0]);
					powershell.Commands = command; ;
					powershell.Invoke();


					command = new PSCommand();
					command.AddScript("Invoke-Command -session $ra -script { Import-Module ActiveDirectory }");
					powershell.Commands = command;
					powershell.Invoke();

					if (powershell.Streams.Error.Count > 0)
					{
						string searchMsg = "was not loaded because no valid module file";
						if (powershell.Streams.Error.Where(record => record.Exception.ToString().Contains(searchMsg)).ToArray().Length > 0)
						{
							throw new Exception("Failed to load the module on the specified server.  Please ensure the ActiveDirectory PowerShell Module is installed on the specified server");
						}
					}

					command = new PSCommand();
					command.AddScript("Import-PSSession -Session $ra -module ActiveDirectory");
					powershell.Commands = command;
					powershell.Invoke();

					if (powershell.Streams.Error.Count > 0)
					{
						string searchMsg = "Running the Get-Command command in a remote session returned no results";
						if (powershell.Streams.Error.Where(record => record.Exception.ToString().Contains(searchMsg)).ToArray().Length > 0)
						{
							throw new Exception("Failed to load the module on the specified server.  Please ensure the ActiveDirectory PowerShell Module is installed on the specified server");
						}
					}

					powershell.Streams.Error.Clear();

					String str;
					if (ServerType == "Active Directory")
						str = "Get-ADDomainController -filter * | select HostName | sort HostName";
					else
						str = "Get-ADComputer -filter * -properties * | where {$_.OperatingSystem -ne $null} |  select  Name,DNSHostName | sort Name";
					powershell.AddScript(str);

					results = powershell.Invoke();

					dt.Columns.Add("ServerName", typeof(string));
					dt.Columns.Add("IPAddress", typeof(string));

					foreach (PSObject ps in results)
					{
						try
						{
							if (ServerType == "Active Directory")
							{
								string name = ps.Properties["HostName"].Value.ToString();

								DataRow dr = dt.NewRow();
								dr["ServerName"] = name;
								dr["IPAddress"] = name;
								dt.Rows.Add(dr);
							}
							else
							{
								string name = ps.Properties["DNSHostName"].Value.ToString();
								string IP = ps.Properties["DNSHostName"].Value.ToString();

								DataRow dr = dt.NewRow();
								dr["ServerName"] = name;
								dr["IPAddress"] = IP;
								dt.Rows.Add(dr);
							}
						}
						catch (Exception ex)
						{
							throw ex;
							//10/6/2014 NS modified for VSPLUS-990
							//errorDiv.InnerHtml = "The following error has occurred: " + ex.Message +
							//	"<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
							//errorDiv.Style.Value = "display: block";
							//errorinfoDiv.Style.Value = "display: block";
							//Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
							//return;
						}

					}


					if (pssession != null)
					{
						Command cmd = new Command("remove-pssession");
						cmd.Parameters.Add("id", pssession.Id);
						powershell.Commands.Clear();
						powershell.Commands.AddCommand(cmd);
						powershell.Invoke();
					}
					if (runspace != null)
					{
						if (runspace.RunspaceStateInfo.State == RunspaceState.Opened)
						{
							runspace.Close();
							runspace.Dispose();
							powershell.Dispose();
						}
					}

					FillServerSelectonScreen(dt);


				}
				GC.Collect();
			}
			catch (Exception ex)
			{
				//10/6/2014 NS modified for VSPLUS-990
				//errorDiv.InnerHtml = "An error has occurred and has been logged." +
				//	"<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
				//errorDiv.Style.Value = "display: block";
				//LogErrorMessage(ex.Message.ToString());
				HandleImportError(ex);
				return;
			}


		}

		public void LoadLyncServers(string IPAddress, string Password, string UserName)
		{
			DataTable dt = new DataTable();
			string strError = "";
			//ViewState["Pwd"] = Password;
			//ViewState.Add("Pwd", Password);
			bool updatedsrv = VSWebBL.SettingBL.SettingsBL.Ins.UpdateSvalue("Primary Exchange Server", ExchangeServerTextBox.Text, VSWeb.Constants.Constants.SysString);
			System.Uri uri = new Uri(IPAddress + "/OcsPowershell");
			System.Security.SecureString securePassword = String2SecureString(Password);
			PSCredential creds = new PSCredential(UserName, securePassword);
			Runspace runspace = RunspaceFactory.CreateRunspace();
			PowerShell powershell = PowerShell.Create();

			PSCommand command = new PSCommand();
			command.AddCommand("New-PSSession");
			command.AddParameter("ConnectionUri", uri);
			command.AddParameter("Credential", creds);
			command.AddParameter("Authentication", "Default");
			System.Collections.ObjectModel.Collection<PSObject> results = new System.Collections.ObjectModel.Collection<PSObject>();

			PSSessionOption sessionOption = new PSSessionOption();
			sessionOption.SkipCACheck = true;
			sessionOption.SkipCNCheck = true;
			sessionOption.SkipRevocationCheck = true;

			command.AddParameter("SessionOption", sessionOption);
			powershell.AddScript(@"set-executionpolicy unrestricted");
			powershell.Commands = command;

			try
			{
				using (runspace)
				{
					runspace.Open();
					powershell.Runspace = runspace;
					System.Collections.ObjectModel.Collection<PSSession> result = powershell.Invoke<PSSession>();

					if (result.Count == 0)
					{
						foreach (ErrorRecord current in powershell.Streams.Error)
						{
							throw new Exception(current.Exception.ToString());
							//strError += "Exception Importing Servers: " + current.Exception.ToString() + ",\r\nInner Exception: " + current.Exception.InnerException;
							//10/6/2014 NS modified for VSPLUS-990
							//errorDiv.InnerHtml = strError +
							//	"<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
							//errorDiv.Style.Value = "display: block";
							//return;
						}
					}
					PSSession pssession = (PSSession)result[0];
					command = new PSCommand();
					command.AddCommand("Set-Variable");
					command.AddParameter("Name", "ra");
					command.AddParameter("Value", result[0]);
					powershell.Commands = command; ;
					powershell.Invoke();


					command = new PSCommand();
					command.AddScript("Import-PSSession -Session $ra -CommandName Get-CsComputer -FormatTypeName *");
					powershell.Commands = command;
					powershell.Invoke();

					if (powershell.Streams.Error.Count > 0)
					{
						string searchMsg = "Running the Get-Command command in a remote session returned no results.";
						if (powershell.Streams.Error.Where(record => record.Exception.ToString().Contains(searchMsg)).ToArray().Length > 0)
						{
							throw new Exception("Failed to retrieve the appropiate cmdlet from the specified server when importing.  Please ensure the Lync PowerShell Module is installed on the specified server");
						}
					}

					powershell.Streams.Error.Clear();

					String str = "Get-CsComputer | select Identity, Fqdn | sort Identity";
					powershell.AddScript(str);

					results = powershell.Invoke();

					if (powershell.Streams.Error.Count > 0)
					{
						string searchMsg = "is not recognized as the name of a cmdlet, function, script file, or operable program.";
						if (powershell.Streams.Error.Where(record => record.Exception.ToString().Contains(searchMsg)).ToArray().Length > 0)
						{
							throw new Exception("Failed to retrieve the appropiate cmdlet from the specified server.  Please ensure the Lync PowerShell Module is installed on the specified server");
						}
					}

					dt.Columns.Add("ServerName", typeof(string));
					dt.Columns.Add("IPAddress", typeof(string));

					foreach (PSObject ps in results)
					{
						try
						{
							string name = ps.Properties["Identity"].Value.ToString();
							string Fqdn = ps.Properties["Fqdn"].Value.ToString();

							DataRow dr = dt.NewRow();
							dr["ServerName"] = name;
							dr["IPAddress"] = Fqdn;
							dt.Rows.Add(dr);
						}
						catch (Exception ex)
						{
							throw ex;
							//10/6/2014 NS modified for VSPLUS-990
							//errorDiv.InnerHtml = "An error has occurred and has been logged." +
							//	"<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
							//errorDiv.Style.Value = "display: block";
							//errorinfoDiv.Style.Value = "display: block";
							//Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
							//return;
						}

					}


					if (pssession != null)
					{
						Command cmd = new Command("remove-pssession");
						cmd.Parameters.Add("id", pssession.Id);
						powershell.Commands.Clear();
						powershell.Commands.AddCommand(cmd);
						powershell.Invoke();
					}
					if (runspace != null)
					{
						if (runspace.RunspaceStateInfo.State == RunspaceState.Opened)
						{
							runspace.Close();
							runspace.Dispose();
							powershell.Dispose();
						}
					}

					FillServerSelectonScreen(dt);


				}
				GC.Collect();
			}
			catch (Exception ex)
			{
				//10/6/2014 NS modified for VSPLUS-990
				//errorDiv.InnerHtml = "An error has occurred and has been logged." +
				//	"<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
				//errorDiv.Style.Value = "display: block";
				//LogErrorMessage(ex.Message.ToString());
				HandleImportError(ex);
				return;
			}


		}

		public void FillServerSelectonScreen(DataTable dt)
		{
			if (dt.Rows.Count == 0)
			{
				errorDiv.InnerHtml = "No Servers Found." +
							"<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
				errorDiv.Style.Value = "display: block";
				return;
			}
			errorDiv.Style.Value = "display: none;";
			errorinfoDiv.Style.Value = "display: none";
			DataTable LocationsDataTable = new DataTable();
			LocationsDataTable = VSWebBL.SecurityBL.LocationsBL.Ins.GetAllData();
			if (LocationsDataTable.Rows.Count > 0)
			{
				LocComboBox.DataSource = LocationsDataTable;
				LocComboBox.TextField = "Location";
				LocComboBox.ValueField = "Location";
				LocComboBox.DataBind();
				LocIDComboBox.DataSource = LocationsDataTable;
				LocIDComboBox.TextField = "ID";
				LocIDComboBox.ValueField = "ID";
				LocIDComboBox.DataBind();
			}
			else
			{
				//10/6/2014 NS modified for VSPLUS-990
				errorDiv.InnerHtml = "All imported servers must be assigned to a location. There were no locations found. Please create at least one location entry using the 'Setup & Security - Maintain Server Locations' menu option." +
					"<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
				errorDiv.Style.Value = "display: block";
				return;
			}
			DataTable servers = new DataTable();
			servers.Columns.Add("ServerName", typeof(string));
			servers.Columns.Add("IPAddress", typeof(string));

			foreach (DataRow row in dt.Rows)
			{
				try
				{
					DataRow[] foundRows;

					DataTable importedDT;
					importedDT = VSWebBL.ConfiguratorBL.AlertsBL.Ins.GetServer();
					foundRows = importedDT.Select("ServerName = '" + row["ServerName"].ToString() + "'");
					if (foundRows.Count() == 0)
					{
						DataRow newRow = servers.NewRow();
						newRow["ServerName"] = row["ServerName"].ToString();
						newRow["IPAddress"] = row["IPADDRESS"].ToString();
						servers.Rows.Add(newRow);

					}
				}
				catch (Exception ex)
				{
					//10/6/2014 NS modified for VSPLUS-990
					
					errorDiv.InnerHtml = "An error has occurred and has been logged." +
						"<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
					errorDiv.Style.Value = "display: block";
					errorinfoDiv.Style.Value = "display: block";
					Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
					return;
				}





			}

			if (servers.Rows.Count > 0)
			{
				infoDiv.Style.Value = "display: block";
				SrvCheckBoxList.DataSource = servers;
				SrvCheckBoxList.TextField = "ServerName";
				SrvCheckBoxList.ValueField = "ServerName";
				SrvCheckBoxList.DataBind();
				IPCheckBoxList.DataSource = servers;
				IPCheckBoxList.TextField = "IPAddress";
				IPCheckBoxList.ValueField = "IPAddress";
				IPCheckBoxList.DataBind();
				ASPxRoundPanel1.Visible = true;
			}
			else
			{
				//10/6/2014 NS modified for VSPLUS-990
				errorDiv.InnerHtml = "There are no new servers in the address book that have not already been imported into VitalSigns." +
					"<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
				errorDiv.Style.Value = "display: block";
				return;
			}


		}

		protected void ServerTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (ServerTypeComboBox.Text == "Database Availability Group")
			{
				infoForServers.InnerHtml = "To search for DAG's, enter a Exchange Server on the same forest";
				infoForServers.Style.Value = "display: block";
			}
			else if (ServerTypeComboBox.Text == "Windows")
			{
				infoForServers.InnerHtml = "To search for Windows Servers, enter a Active Directory Server on the same pool";
				infoForServers.Style.Value = "display: block";
			}
			else
			{
				infoForServers.InnerHtml = "";
				infoForServers.Style.Value = "display: none";
			}
		}

		protected void HandleImportError(Exception ex)
		{

			string Error = "";

			if(ex.Message.Contains("TrustedHosts configuration setting"))
			{
				Error = "Please ensure the remote server and this server have each other defined in their TrustedHosts variable for WinRM";
			}
			else if (ex.Message.Contains("Access is denied."))
			{
				Error = "Please ensure the credentials you have provided are correct and have the appropriate rights";
			}
			else if (ex.Message.Contains("The maximum number of concurrent shells for this user has been exceeded"))
			{
				Error = "You have exceded the maximum number of conenctions for this user. Please either use a different user account or go into your WSMan configuration and increase the number of allowed conenctions per user";
			}
			else if (ex.Message.Contains("Verify that the WS-Management service is running on the remote host and configured to"))
			{
				Error = "Please ensure Remote PowerShell is enabled on both the remote server and this server";
			}
			else if (ex.Message.Contains("The WinRM client received an HTTP bad request status (400)"))
			{
				Error = "The server returned back a HTTP bad request status. Confirm the credentials are correct";
			}
			else if (ex.Message.Contains("System.Management.Automation.Remoting.PSRemotingTransportException"))
			{
				Error = "Please ensure that the WinRM service is started on the machine we will be using as the directory server to import from";
			}
			else
			{
				Error = "An unaccounted for error has occurred";
			}


			errorDiv.InnerHtml = Error + ". For the complete error, please refer to the VitalSigns Web Log." +
					"<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
			errorDiv.Style.Value = "display: block";
			LogErrorMessage(ex.Message.ToString());
		}

		protected void LogErrorMessage(string message)
		{
			Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + message);
		}
      
        //13/05/2016 sowmya added for VSPLUS-2942
        protected void ServerTypeComboBox_SelectedIndexChanged1(object sender, EventArgs e)
        {
            infoForServers.Visible = true;

            if (ServerTypeComboBox.Text == "Active Directory")
            {
                infoForServers.InnerText = " Please enter the Active Direcotry's FQDN. The credentials should be an administrative account on the domain.";
            }
            else if (ServerTypeComboBox.Text == "Database Availability Group")
            {
                infoForServers.InnerText = "Please enter a Exchange Server's URL, including HTTP/HTTPS. The credentials should be an administrative account with the format of USERNAME@DOMAIN.com.";
            }
            else if (ServerTypeComboBox.Text == "Exchange")
            {
                infoForServers.InnerText = "Please enter the Exchange Server's URL, including HTTP/HTTPS. The credentials should be an administrative account with the format of USERNAME@DOMAIN.com.";
            }
            else if (ServerTypeComboBox.Text == "Skype for Business")
            {
                infoForServers.InnerText = " Please enter a Skype for Business' URL, including HTTP/HTTPS. The credentials should be an administrative account.";
            }
            else if (ServerTypeComboBox.Text == "SharePoint")
            {
                infoForServers.InnerText = " Please enter the SharePoint Server Name. The credentials should be a SharePoint account with administrative access.";
            }
            else if (ServerTypeComboBox.Text == "Windows")
            {
                infoForServers.InnerText = " Please enter the Active Direcotry's FQDN, where the Windows Servers can be found. The credentials should be an administrative account on the domain.";
            }
        }

	}
}