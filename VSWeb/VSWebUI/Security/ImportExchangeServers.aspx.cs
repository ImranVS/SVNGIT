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
    public partial class ImportExchangeServers : System.Web.UI.Page
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
				if (ServerType == "DAG")
					SSLCheckBox.Visible = false;
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
				UserIdtextBox.BackColor = System.Drawing.Color.White ;
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
			c.AliasName = CredentialsComboBox.Text;
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
        public void LoadExchangeServers()
        {
			if (ExchangeServerTextBox.Text.ToLower().Contains("http://")==false && ExchangeServerTextBox.Text.ToLower().Contains("https://")==false)
			{
                //10/6/2014 NS modified for VSLPLUS-934
				errorDiv.InnerHtml = "Not a Valid Exchange Server Address. Server Address shoud be in the form of 'https://myexchange.com'"+
                    "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
				errorDiv.Style.Value = "display: block";
				return;
			}

			DataTable dt = new DataTable();
			string strError = "";
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
						errorDiv.InnerHtml = "User ID or Password not set for the selected Credentials."+
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
					errorDiv.InnerHtml = "Please Enter the User ID and Password."+
                        "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
					errorDiv.Style.Value = "display: block";
					return;
				}
			}
			Uid = UserName;
			Pwd = Password;
			

			string IPAddress = ExchangeServerTextBox.Text;// "https://jnittech-exchg1.jnittech.com";
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

					//foreach (ErrorRecord current in powershell.Streams.Error)
					//{
					//    strError += "Exception Importing Servers: " + current.Exception.ToString() + ",\r\nInner Exception: " + current.Exception.InnerException;
					//    errorDiv.InnerHtml = strError;
					//    errorDiv.Style.Value = "display: block";
					//    return;
					//}

					if (result.Count == 0)
					{
						//errorDiv.InnerHtml = "Unexpected number of Remote Runspace connections returned.";
						//errorDiv.Style.Value = "display: block";
						//return;
						foreach (ErrorRecord current in powershell.Streams.Error)
						{
							strError += "Exception Importing Servers: " + current.Exception.ToString() + ",\r\nInner Exception: " + current.Exception.InnerException;
                            //10/6/2014 NS modified for VSPLUS-990
							errorDiv.InnerHtml = strError+
                                "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
							errorDiv.Style.Value = "display: block";
							return;
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

					powershell.Streams.Error.Clear();

					String str = "Get-ExchangeServer | select Name, Fqdn";
					powershell.AddScript(str);

					results = powershell.Invoke();
					
					dt.Columns.Add("ServerName", typeof(string));
					dt.Columns.Add("IPAddress", typeof(string));
					if (results.Count > 0)
					{
						foreach (PSObject ps in results)
						{
							

							string Fqdn =  ps.Properties["Fqdn"].Value.ToString();
							string name = ps.Properties["Name"].Value.ToString();
							//load exchange servers
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

								try
								{
									DataRow[] foundRows;

									DataTable importedDT;
									importedDT = VSWebBL.ConfiguratorBL.AlertsBL.Ins.GetServer();
									foundRows = importedDT.Select("ServerName = '" + name + "'");
									if (foundRows.Length == 0)
									{
										DataRow dr = dt.NewRow();
										dr["ServerName"] = name;
										dr["IPAddress"] = Fqdn;
										dt.Rows.Add(dr);
										dr = dt.NewRow();
									}
								}
								catch (Exception ex)
								{
                                    //10/6/2014 NS modified for VSPLUS-990
									errorDiv.InnerHtml = "The following error has occurred: " + ex.Message+
                                        "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
									errorDiv.Style.Value = "display: block";
									errorinfoDiv.Style.Value = "display: block";
									Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
									return;
								}


							}
							else
							{
                                //10/6/2014 NS modified for VSPLUS-990
								errorDiv.InnerHtml = "All imported servers must be assigned to a location. There were no locations found. Please create at least one location entry using the 'Setup & Security - Maintain Server Locations' menu option."+
                                    "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
								errorDiv.Style.Value = "display: block";
								return;
							}
						}
						results.Clear();
						result.Clear();
						if (dt.Rows.Count > 0)
						{
							infoDiv.Style.Value = "display: block";
							SrvCheckBoxList.DataSource = dt;
							SrvCheckBoxList.TextField = "ServerName";
							SrvCheckBoxList.ValueField = "ServerName";
							SrvCheckBoxList.DataBind();
							IPCheckBoxList.DataSource = dt;
							IPCheckBoxList.TextField = "IPAddress";
							IPCheckBoxList.ValueField = "IPAddress";
							IPCheckBoxList.DataBind();
							ASPxRoundPanel1.Visible = true;
						}
						else
						{
                            //10/6/2014 NS modified for VSPLUS-990
							errorDiv.InnerHtml = "There are no new servers in the address book that have not already been imported into VitalSigns."+
                                "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
							errorDiv.Style.Value = "display: block";
							return;
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
							if (runspace.RunspaceStateInfo.State == RunspaceState.Opened)
							{
								runspace.Close();
								runspace.Dispose();
								powershell.Dispose();
							}
					}
					else
					{
                        //10/6/2014 NS modified for VSPLUS-990
						errorDiv.InnerHtml = "No Servers Found."+
                            "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
						errorDiv.Style.Value = "display: block";
						return;
					}

				}
				GC.Collect();
			}
			catch (Exception ex)
			{
                //10/6/2014 NS modified for VSPLUS-990
				errorDiv.InnerHtml = ex.Message.ToString()+
                    "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
				errorDiv.Style.Value = "display: block";
				return;
			}


			}


           

        protected void ImportButton_Click(object sender, EventArgs e)
        {
			ImportServers();
        }
		protected void CreateCredentialsImportButton_Click(object sender, EventArgs e)
		{
			if (Uid == "" || Pwd == "")
			{
                //10/6/2014 NS modified for VSPLUS-990
				errorDiv.InnerHtml = "Please Enter the User ID and Password."+
                    "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
				errorDiv.Style.Value = "display: block";
				return;
			}
			ImportServers();
			CreateCredentials();

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
						if (SSLCheckBox.Checked)
							SSL = "https://";
						else
							SSL = "http://";

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
								if (ServerType == "DAG")
									dr["ServerType"] = "Database Availability Group";
								else
								dr["ServerType"] = "Exchange";
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
						
						if (ServerType == "DAG")
							Response.Redirect("~/Security/ImportExchangeServers2.aspx?ServerType=DAG", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
						else
						Response.Redirect("~/Security/ImportExchangeServers2.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
						Context.ApplicationInstance.CompleteRequest();
					}
				}
			}
		}
		private void CreateCredentials()
		{
			Object ReturnValue = VSWebBL.ConfiguratorBL.CredentialsBL.Ins.InsertData(CollectDataForCredentials());
		}
		private Credentials CollectDataForCredentials()
		{
			VSFramework.TripleDES tripleDes = new VSFramework.TripleDES();
			try
			{
				Credentials CredentialsObject = new Credentials();
				CredentialsObject.AliasName = "Exchange-" + LocComboBox.Text;
				CredentialsObject.UserID = Uid ;

				string rawPass =Pwd;
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
			Response.Redirect("~/Security/ImportExchangeServers2.aspx?ProfileNames=" + ProfileName, false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
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
            //LoadDominoServers();
			//PrereqForExchange("", "", "", "");
			ServerType = "";
			SSLCheckBox.Visible = true;
			LoadExchangeServers();
			//LoadExchangeDAG();
            //12/20/2013 NS modified - make the panel visible if the server list is loaded
            //ASPxRoundPanel1.Visible = true;
        }
		protected void LoadDAGButton_Click(object sender, EventArgs e)
        {
            //LoadDominoServers();
			//PrereqForExchange("", "", "", "");
			//LoadExchangeServers();
			ServerType = "DAG";
			LoadExchangeDAG();
			SSLCheckBox.Visible = false;
            //12/20/2013 NS modified - make the panel visible if the server list is loaded
            //ASPxRoundPanel1.Visible = true;
        }
		
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
		public void LoadExchangeDAG()
		{
			if (ExchangeServerTextBox.Text.ToLower().Contains("http://") == false && ExchangeServerTextBox.Text.ToLower().Contains("https://") == false)
			{
				errorDiv.InnerHtml = "Not a Valid Exchange Server Address!. Server Address shoud be in the form of 'https://myexchange.com'";
				errorDiv.Style.Value = "display: block";
				return;
			}

			DataTable dt = new DataTable();
			string strError = "";
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
						errorDiv.InnerHtml = "User Id or Password not set for the selected Credentials";
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
					errorDiv.InnerHtml = "Please Enter the User Id and Password.";
					errorDiv.Style.Value = "display: block";
					return;
				}
			}
			Uid = UserName;
			Pwd = Password;


			string IPAddress = ExchangeServerTextBox.Text;// "https://jnittech-exchg1.jnittech.com";
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

					//foreach (ErrorRecord current in powershell.Streams.Error)
					//{
					//    strError += "Exception Importing Servers: " + current.Exception.ToString() + ",\r\nInner Exception: " + current.Exception.InnerException;
					//    errorDiv.InnerHtml = strError;
					//    errorDiv.Style.Value = "display: block";
					//    return;
					//}

					if (result.Count == 0)
					{
						//errorDiv.InnerHtml = "Unexpected number of Remote Runspace connections returned.";
						//errorDiv.Style.Value = "display: block";
						//return;
						foreach (ErrorRecord current in powershell.Streams.Error)
						{
							strError += "Exception Importing Servers: " + current.Exception.ToString() + ",\r\nInner Exception: " + current.Exception.InnerException;
							errorDiv.InnerHtml = strError;
							errorDiv.Style.Value = "display: block";
							return;
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

					powershell.Streams.Error.Clear();

					String str = "Get-DatabaseAvailabilityGroup | Select-Object -Property Name,WitnessServer";
					powershell.AddScript(str);

					results = powershell.Invoke();

					dt.Columns.Add("DAGName", typeof(string));
					dt.Columns.Add("WitnessServer", typeof(string));
					if (results.Count > 0)
					{
						foreach (PSObject ps in results)
						{


							string Fqdn = ps.Properties["Name"].Value.ToString();
							string name = ps.Properties["Name"].Value.ToString();
							//load exchange servers
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

								try
								{
									DataRow[] foundRows;

									DataTable importedDT;
									importedDT = VSWebBL.ConfiguratorBL.AlertsBL.Ins.GetServer();
									foundRows = importedDT.Select("ServerName = '" + name + "'");
									if (foundRows.Length == 0)
									{
										DataRow dr = dt.NewRow();
										dr["DAGName"] = name;
										dr["WitnessServer"] = name;
										dt.Rows.Add(dr);
										dr = dt.NewRow();
									}
								}
								catch (Exception ex)
								{
									errorDiv.InnerHtml = "The following error has occurred: " + ex.Message;
									errorDiv.Style.Value = "display: block";
									errorinfoDiv.Style.Value = "display: block";
									Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
									return;
								}


							}
							else
							{
								errorDiv.InnerHtml = "All imported servers must be assigned to a location. There were no locations found. Please create at least one location entry using the 'Setup & Security - Maintain Server Locations' menu option.";
								errorDiv.Style.Value = "display: block";
								return;
							}
						}
						results.Clear();
						result.Clear();
						if (dt.Rows.Count > 0)
						{
							infoDiv.Style.Value = "display: block";
							SrvCheckBoxList.DataSource = dt;
							SrvCheckBoxList.TextField = "DAGName";
							SrvCheckBoxList.ValueField = "DAGName";
							SrvCheckBoxList.DataBind();
							IPCheckBoxList.DataSource = dt;
							IPCheckBoxList.TextField = "DAGName";
							IPCheckBoxList.ValueField = "DAGName";
							IPCheckBoxList.DataBind();
							ASPxRoundPanel1.Visible = true;
						}
						else
						{
							errorDiv.InnerHtml = "There are no new servers in the address book that have not already been imported into VitalSigns.";
							errorDiv.Style.Value = "display: block";
							return;
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
							if (runspace.RunspaceStateInfo.State == RunspaceState.Opened)
							{
								runspace.Close();
								runspace.Dispose();
								powershell.Dispose();
							}
					}
					else
					{
						errorDiv.InnerHtml = "No Servers Found!.";
						errorDiv.Style.Value = "display: block";
						return;
					}

				}
				GC.Collect();
			}
			catch (Exception ex)
			{
				errorDiv.InnerHtml = ex.Message.ToString();
				errorDiv.Style.Value = "display: block";
				return;
			}


		}
		
    }
}