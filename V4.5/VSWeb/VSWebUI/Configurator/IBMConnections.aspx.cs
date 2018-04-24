using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DevExpress.Web;
using System.Net;
using VSWebDO;
using System.Xml.Serialization;
using DevExpress.Web.ASPxTreeList;
using VitalSignsWebSphereDLL;
using System.Net;
using System.Net.Sockets;

namespace VSWebUI.Configurator
{
	public partial class IBMConnections : System.Web.UI.Page
	{
		string Mode;
		int key;
		IPAddress[] ipaddress;
		string fileName1;
		IPHostEntry hostEntry;
		bool flag = false;
		bool validhostname = true;
		string tests;
		int samecellid;
		bool insertcell = false;
		bool cellnameinsert = true;
		string UIip, dbipaddress, port, convertdbip;
		VSFramework.TripleDES mytestenkey = new VSFramework.TripleDES();
		VitalSignsWebSphereDLL.VitalSignsWebSphereDLL WSDll = new VitalSignsWebSphereDLL.VitalSignsWebSphereDLL();
		VitalSignsWebSphereDLL.VitalSignsWebSphereDLL.CellProperties cellFromInfo = new VitalSignsWebSphereDLL.VitalSignsWebSphereDLL.CellProperties();
		protected void Page_Load(object sender, EventArgs e)
		{
			try
			{
				if (Request.QueryString["ID"] != null && Request.QueryString["ID"] != "")
				{
					Session["IBMConnectionsid"] = Request.QueryString["ID"];
				
					Mode = "Update";

					key = Convert.ToInt32(Session["IBMConnectionsid"].ToString());

					if (!IsPostBack)
					{
						Session["DataEvents"] = null;



						WebsphereCell IBMCObject = new WebsphereCell();
						IBMCObject.IBMConnectionSID = key;
						DataTable dt = VSWebBL.ConfiguratorBL.IBMConnectionsServersBL.Ins.GetcellID(IBMCObject);


						if (dt.Rows.Count > 0)
						{
							Session["webcellid"] = dt.Rows[0]["CellID"].ToString();
							FillWebsphereCell(key);

						}
						else
						{
							Session["webcellid"] = null;
						}

						filldetails(key);
						FilltestsData(key);
						FillMaintenanceGrid();
						FillAlertGridView();
						FillCredentialsComboBox();
						FillIBMConnectionsCredentialComboBox();

						lblServer.InnerHtml += " - " + ServerNameTextBox.Text;
						if (Session["UserPreferences"] != null)
						{
							DataTable UserPreferences = (DataTable)Session["UserPreferences"];
							foreach (DataRow dr in UserPreferences.Rows)
							{
								if (dr[1].ToString() == "SametimeServer|MaintWinListGridView")
								{
									MaintWinListGridView.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
								}
								if (dr[1].ToString() == "SametimeServer|AlertGridView")
								{
									AlertGridView.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
								}
							}
						}
					}

					else
					{
						fillNodesTreeListbycellIDfromsession();
						FillMaintServersGridfromSession();
						FillAlertGridViewfromSession();

					}
				}

				else
				{
					Mode = "Insert";
					if (!IsPostBack)
					{
						RetryIntervalTextBox.Text = "2";
						OffHoursScanIntervalTextBox.Text = "30";
						ScanIntervalTextBox.Text = "8";
						ResponseThresholdTextBox.Text = "250";

						CategoryTextBox.Text = "Production";
						EnabledForScanningCheckBox.Checked = true;
					}
				}
			}
			catch(Exception ex)
			{
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}

		}




		private void FillCredentialsComboBox()
		{
			DataTable CredentialsDataTable = VSWebBL.ConfiguratorBL.ServicesBL.Ins.GetCredentials();
			WebCredentialsComboBox.DataSource = CredentialsDataTable;
			WebCredentialsComboBox.TextField = "AliasName";
			WebCredentialsComboBox.ValueField = "ID";
			WebCredentialsComboBox.DataBind();

			DataTable STCredentialsDataTable = VSWebBL.ConfiguratorBL.IBMConnectionsServersBL.Ins.GetIBMConnectionsCredentials(Convert.ToInt32(Session["ServerTypeID"].ToString()));
			cbChatUser1.DataSource = STCredentialsDataTable;
			cbChatUser1.TextField = "AliasName";
			cbChatUser1.ValueField = "ID";
			cbChatUser1.DataBind();
			cbChatUser1.Items.Add("None", "");

			

			cbChatUser2.DataSource = STCredentialsDataTable;
			cbChatUser2.TextField = "AliasName";
			cbChatUser2.ValueField = "ID";
			cbChatUser2.DataBind();
			cbChatUser2.Items.Add("None", "");

			 

		}

		public void filldetails(int id)
		{
			IBMConnectionsServers IBMConnectionObject = new IBMConnectionsServers();
			IBMConnectionsServers returnIBMConnectionObject = new IBMConnectionsServers();


			IBMConnectionObject.ID = id;
			Session["severid"] = IBMConnectionObject.ID.ToString();


			returnIBMConnectionObject = VSWebBL.ConfiguratorBL.IBMConnectionsServersBL.Ins.GetdatawithId(IBMConnectionObject);

			if (returnIBMConnectionObject.Category == "")
			{


				ServerNameTextBox.Text = returnIBMConnectionObject.Name.ToString();
				IPAddressTextBox.Text = returnIBMConnectionObject.IPAddress.ToString();
				lblServerID.Text = returnIBMConnectionObject.SID.ToString();
				DescriptionTextBox.Text = returnIBMConnectionObject.Description.ToString();
				CategoryTextBox.Text = returnIBMConnectionObject.Category.ToString();
				EnabledForScanningCheckBox.Checked = returnIBMConnectionObject.Enabled;

				LocationTextBox.Text = returnIBMConnectionObject.Location.ToString();
				ScanIntervalTextBox.Text = returnIBMConnectionObject.ScanInterval.ToString();
				OffHoursScanIntervalTextBox.Text = returnIBMConnectionObject.OffHoursScanInterval.ToString();
				RetryIntervalTextBox.Text = returnIBMConnectionObject.RetryInterval.ToString();
				OffHoursScanIntervalTextBox.Text = returnIBMConnectionObject.OffHoursScanInterval.ToString();
				ResponseThresholdTextBox.Text = returnIBMConnectionObject.ResponseThreshold.ToString();
				SrvAtrFailBefAlertTextBox.Text = returnIBMConnectionObject.FailureThreshold.ToString();


				

				try
				{
					
					ThisserverrequiresSSLCheckBox.Checked = Convert.ToBoolean(returnIBMConnectionObject.SSL);


					SrvAtrFailBefAlertTextBox.Text = returnIBMConnectionObject.FailureThreshold.ToString();

					cbChatUser1.SelectedItem = cbChatUser1.Items.FindByValue(Convert.ToInt32(returnIBMConnectionObject.ChatUser1Credentials));
					for (int i = 0; i < cbChatUser1.Items.Count; i++)
					{
						if (cbChatUser1.Items[i].Value.ToString() == returnIBMConnectionObject.ChatUser1Credentials.ToString())
							cbChatUser1.Items[i].Selected = true;
					}

					cbChatUser2.SelectedItem = cbChatUser2.Items.FindByValue(Convert.ToInt32(returnIBMConnectionObject.ChatUser2Credentials));
					for (int i = 0; i < cbChatUser2.Items.Count; i++)
					{
						if (cbChatUser2.Items[i].Value.ToString() == returnIBMConnectionObject.ChatUser2Credentials.ToString())
							cbChatUser2.Items[i].Selected = true;
					}



					chkChatSimulation.Checked = Convert.ToBoolean(returnIBMConnectionObject.TestChatSimulation);
					Session["ReturnUrl"] = "IBMConnections.aspx?ID=" + lblServerID.Text + "&tab=1";

				}
				catch (Exception ex)
				{
					
					Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
					throw ex;
				}
				RetryIntervalTextBox.Text = "2";
				OffHoursScanIntervalTextBox.Text = "30";
				ScanIntervalTextBox.Text = "8";
				ResponseThresholdTextBox.Text = "250";
				
				CategoryTextBox.Text = "Production";
				EnabledForScanningCheckBox.Checked = true;

			}

			else
			{
				Credentials cred = new Credentials();
				

				//WebCredentialsComboBox.Text = celldt.Rows[0]["AliasName"].ToString();

				CredentialComboBox.Value = returnIBMConnectionObject.CredentialID.ToString();

                if (returnIBMConnectionObject.CredentialID == 0)
                {
                    CredentialComboBox.Value = "";
                }
				DBCredentialsComboBox.Value = returnIBMConnectionObject.DBCredentialsID.ToString();
				if (returnIBMConnectionObject.DBCredentialsID == 0)
				{
					DBCredentialsComboBox.Value = "";
				}
                ServerNameTextBox.Text = returnIBMConnectionObject.Name;
				IPAddressTextBox.Text = returnIBMConnectionObject.IPAddress;
				lblServerID.Text = returnIBMConnectionObject.SID.ToString();
				DescriptionTextBox.Text = returnIBMConnectionObject.Description;
				CategoryTextBox.Text = returnIBMConnectionObject.Category;
				EnabledForScanningCheckBox.Checked = returnIBMConnectionObject.Enabled;
				ThisserverrequiresSSLCheckBox.Checked = returnIBMConnectionObject.SSL;
				LocationTextBox.Text = returnIBMConnectionObject.Location;
				ScanIntervalTextBox.Text = returnIBMConnectionObject.ScanInterval.ToString();
				OffHoursScanIntervalTextBox.Text = returnIBMConnectionObject.OffHoursScanInterval.ToString();
				RetryIntervalTextBox.Text = returnIBMConnectionObject.RetryInterval.ToString();
				OffHoursScanIntervalTextBox.Text = returnIBMConnectionObject.OffHoursScanInterval.ToString();
				ResponseThresholdTextBox.Text = returnIBMConnectionObject.ResponseThreshold.ToString();
				SrvAtrFailBefAlertTextBox.Text = returnIBMConnectionObject.FailureThreshold.ToString();

                //3/14/2015 NS modified
				//ProtocolComboBox.Text = returnIBMConnectionObject.ProxyServerProtocall;
				//ProxyTypeComboBox.Text = returnIBMConnectionObject.ProxyServerType;

				HostNameTextBox.Text = returnIBMConnectionObject.DBHostName;

				DataBaseNameTextBox.Text = returnIBMConnectionObject.DBName;
				DB2PortTextBox.Text = returnIBMConnectionObject.DBPort;
				PortNumberTextBox.Text = returnIBMConnectionObject.PortNumber.ToString();
				IBMConnectionsURLTextBox.Text = returnIBMConnectionObject.URL;
				//getdata.Platform = SortRadioButtonList1.SelectedItem.Text;
				PortCheckBox.Checked = returnIBMConnectionObject.EnableDB2port;

				cbChatUser1.SelectedItem = cbChatUser1.Items.FindByValue(Convert.ToInt32(returnIBMConnectionObject.ChatUser1Credentials));
				for (int i = 0; i < cbChatUser1.Items.Count; i++)
				{
					if (cbChatUser1.Items[i].Value.ToString() == returnIBMConnectionObject.ChatUser1Credentials.ToString())
						cbChatUser1.Items[i].Selected = true;
				}

				cbChatUser2.SelectedItem = cbChatUser2.Items.FindByValue(Convert.ToInt32(returnIBMConnectionObject.ChatUser2Credentials));
				for (int i = 0; i < cbChatUser2.Items.Count; i++)
				{
					if (cbChatUser2.Items[i].Value.ToString() == returnIBMConnectionObject.ChatUser2Credentials.ToString())
						cbChatUser2.Items[i].Selected = true;
				}
				chkChatSimulation.Checked = Convert.ToBoolean(returnIBMConnectionObject.TestChatSimulation);
					
		}


			}

		private void FillMaintenanceGrid()
		{
			try
			{

				DataTable MaintDataTable = new DataTable();
				DataSet ServersDataSet = new DataSet();
				MaintDataTable = VSWebBL.ConfiguratorBL.MaintenanceBL.Ins.GetMaintenDataOnServerID(ServerNameTextBox.Text);
				if (MaintDataTable.Rows.Count > 0)
				{
					DataTable dtcopy = MaintDataTable.Copy();
					dtcopy.PrimaryKey = new DataColumn[] { dtcopy.Columns["ID"] };
					//4/3/2014 NS modified for VSPLUS-138
					//Session["MaintServers"] = dtcopy;
					ViewState["MaintServers"] = dtcopy;
					MaintWinListGridView.DataSource = MaintDataTable;
					MaintWinListGridView.DataBind();
				}
			}
			catch (Exception ex)
			{
				//6/27/2014 NS added for VSPLUS-634
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}
			finally { }
		}

		private void FillAlertGridView()
		{
			try
			{

				DataTable AlertDataTable = new DataTable();
				DataTable ServerTypeTable = new DataTable();
				
				DataSet AlertDataSet = new DataSet();
				ServerTypeTable = VSWebBL.ConfiguratorBL.IBMConnectionsServersBL.Ins.GetServerType(ServerNameTextBox.Text);
				//string ServerTypeTable1 = VSWebBL.ConfiguratorBL.IBMConnectionsServersBL.Ins.GetServerType1(ServerNameTextBox.Text);
				if (ServerTypeTable.Rows.Count > 0)
				{
					string ServerType = ServerTypeTable.Rows[0]["ServerType"].ToString();
					Session["ServerType"] = ServerType;
					Session["ServerTypeID"] = ServerTypeTable.Rows[0]["ID"].ToString();
				
					AlertDataTable = VSWebBL.ConfiguratorBL.AlertsBL.Ins.GetAlertTab(ServerNameTextBox.Text, ServerType);

					if (AlertDataTable.Rows.Count > 0)
					{
						DataTable dtcopy = AlertDataTable.Copy();

						ViewState["AlertDataTable"] = dtcopy;
						AlertGridView.DataSource = AlertDataTable;
						AlertGridView.DataBind();
					}
				}
			}
			catch (Exception ex)
			{
				//6/27/2014 NS added for VSPLUS-634
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}
			finally { }
		}

		private void FillMaintServersGridfromSession()
		{
			try
			{

				DataTable ServersDataTable = new DataTable();
				
				if (ViewState["MaintServers"] != "" && ViewState["MaintServers"] != null)
					ServersDataTable = (DataTable)ViewState["MaintServers"];
				if (ServersDataTable.Rows.Count > 0)
				{
					MaintWinListGridView.DataSource = ServersDataTable;
					MaintWinListGridView.DataBind();
				}
			}
			catch (Exception ex)
			{
				//6/27/2014 NS added for VSPLUS-634
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}
			finally { }
		}
		private void FillAlertGridViewfromSession()
		{
			try
			{
				DataTable AlertDataTable = new DataTable();
				//4/3/2014 NS modified for VSPLUS-138
				//if (Session["AlertDataTable"] != "" && Session["AlertDataTable"] != null)
				//    AlertDataTable = (DataTable)Session["AlertDataTable"];//VSWebBL.ConfiguratorBL.DominoPropertiesBL.Ins.GetAllData();
				if (ViewState["AlertDataTable"] != "" && ViewState["AlertDataTable"] != null)
					AlertDataTable = (DataTable)ViewState["AlertDataTable"];//VSWebBL.ConfiguratorBL.DominoPropertiesBL.Ins.GetAllData();
				if (AlertDataTable.Rows.Count > 0)
				{
					AlertGridView.DataSource = AlertDataTable;
					AlertGridView.DataBind();
				}
			}
			catch (Exception ex)
			{
				//6/27/2014 NS added for VSPLUS-634
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}
			finally { }
		}

		protected void ASPxMenu1_ItemClick(object source, DevExpress.Web.MenuItemEventArgs e)
		{

            DataTable dt;
			Session["Submenu"] = "LotusDomino";
			if (Request.QueryString["ID"] != "" && Request.QueryString["ID"] != null)
			{
				dt = (VSWebBL.SecurityBL.ServersBL.Ins.GetServerDetailsByID(Convert.ToInt32(Request.QueryString["ID"])));
				if (dt.Rows.Count > 0)
				{
					Response.Redirect("~/Dashboard/ConnectionsDetailsPage.aspx?Name=" + dt.Rows[0]["ServerName"].ToString() + "&Type=" + dt.Rows[0]["Type"].ToString() + "&Status=" + dt.Rows[0]["Status"].ToString() + "&LastDate=" + dt.Rows[0]["LastUpdate"].ToString(), false);
					Context.ApplicationInstance.CompleteRequest();
				}
			}


		}


		protected void ServerNameTextBox_TextChanged(object sender, EventArgs e)
		{
			char Quote;
			Quote = (char)34;

			string myName = ServerNameTextBox.Text;
			try
			{
				if (myName.IndexOf("'") > 0)
					myName = myName.Replace("'", "");

				if (myName.IndexOf(Quote) > 0)
					myName = myName.Replace(Quote, '~');

				if (myName.IndexOf(",") > 0)
					myName = myName.Replace(",", " ");

				if (myName != ServerNameTextBox.Text)
					ServerNameTextBox.Text = myName;

			}
			catch (Exception ex)
			{
				//   MessageBox.Show(ex.Message)
				//6/27/2014 NS added for VSPLUS-634
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
			}
		}

		protected void DescriptionTextBox_TextChanged(object sender, EventArgs e)
		{
			char Quote;
			Quote = (char)34;

			string myName = DescriptionTextBox.Text;
			try
			{
				if (myName.IndexOf("'") > 0)
					myName = myName.Replace("'", "");

				if (myName.IndexOf(Quote) > 0)
					myName = myName.Replace(Quote, '~');

				if (myName.IndexOf(",") > 0)
					myName = myName.Replace(",", " ");

				if (myName != DescriptionTextBox.Text)
					DescriptionTextBox.Text = myName;

			}
			catch (Exception ex)
			{
				//   MessageBox.Show(ex.Message)
				//6/27/2014 NS added for VSPLUS-634
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
			}
		}

		protected void LocationTextBox_TextChanged(object sender, EventArgs e)
		{
			char Quote;
			Quote = (char)34;

			string myName = LocationTextBox.Text;
			try
			{
				if (myName.IndexOf("'") > 0)
					myName = myName.Replace("'", "");

				if (myName.IndexOf(Quote) > 0)
					myName = myName.Replace(Quote, '~');

				if (myName.IndexOf(",") > 0)
					myName = myName.Replace(",", " ");

				if (myName != LocationTextBox.Text)
					LocationTextBox.Text = myName;

			}
			catch (Exception ex)
			{
				//   MessageBox.Show(ex.Message)
				//6/27/2014 NS added for VSPLUS-634
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
			}
		}
		protected void cb_callback(object sender, DevExpress.Web.CallbackEventArgs e)
		{
			String[] parameters = e.Parameter.Split('|');
			string Id = parameters[0];
			bool isChecked = Convert.ToBoolean(parameters[1]);

			DataTable dt = new DataTable();

			if (Session["Tests"] != null && Session["Tests"] != "")
				dt = (DataTable)Session["Tests"];

			if (dt.Rows.Count > 0)
			{
				DataRow row = dt.Rows.Find(Id);
				(dt.Rows.Find(Id))["EnableSimulationTests"] = isChecked;
				DataRow row2 = dt.Rows.Find(Id);
			}
		}
		protected void Tests_PreRender(object sender, EventArgs e)
		{
			bool isValid = true;
			try
			{

				if (isValid)
				{

					ASPxGridView Tests = (ASPxGridView)sender;
					for (int i = 0; i < Tests.VisibleRowCount; i++)
					{
						if (Tests.GetRowValues(i, "EnableSimulationTests") != null)
							Tests.Selection.SetSelection(i, (bool)Tests.GetRowValues(i, "EnableSimulationTests") == true);
					}
				}
			}
			catch (Exception ex)
			{
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}
			finally { }

		}
		//protected void TestsGridView_OnPageSizeChanged(object sender, EventArgs e)
		//{
		//    VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("O365ServerProperties|TestsGridView", Tests.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
		//    Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
		//}
		protected void CellnameTextBox_Validation(object sender, ValidationEventArgs e)
		{
			DataTable returntable = VSWebBL.SecurityBL.webspehereImportBL.Ins.GetNmae(CellnameTextBox.Text);


			if (returntable.Rows.Count > 0)
			{
				errorDivForImportingWS.InnerHtml = "Cell Name already exists. Please enter another value in the Cell Name field." +
					//"<button type=\"button\" onclick='javascript:document.getElementById(\"ContentPlaceHolder1_ASPxPageControl1_ASPxRoundPanel7_ASPxCallbackPanel1_CellnameTextBox_I\").focus();' class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
			"<button type=\"button\" onclick='javascript:var SearchInput = $(\"#ContentPlaceHolder1_ASPxPageControl1_ASPxRoundPanel7_ASPxCallbackPanel1_CellnameTextBox_I\");var strLength= SearchInput.val().length * 2;SearchInput.focus();SearchInput[0].setSelectionRange(strLength, strLength);' class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
				errorDivForImportingWS.Style.Value = "display: block";

				//Div3.InnerHtml = "Name already exists.Please enter another Name." +

				//"<button type=\"button\" onclick='javascript:document.getElementById(\"ContentPlaceHolder1_ASPxPageControl1_ASPxRoundPanel7_ASPxCallbackPanel1_CellnameTextBox_I\").focus();' class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
				//Div3.Style.Value = "display: block";

			}
			else
			{
				errorDivForImportingWS.Style.Value = "display: none";
			}

		}
		protected void HostName_Validation(object sender, ValidationEventArgs e)
		{
			try
			{

				//hostEntry = Dns.GetHostEntry(HostName.Text);
				IPAddress[] ipaddress = Dns.GetHostAddresses(HostName.Text);
				if (ipaddress != null)
				{

					errorDivForImportingWS.Style.Value = "display: none";
				}
			}
			catch (Exception ex)
			{
				errorDivForImportingWS.InnerHtml = "Please enter a valid Host Name." +
					"<button type=\"button\" onclick='javascript:var SearchInput = $(\"#ContentPlaceHolder1_ASPxPageControl1_ASPxRoundPanel7_ASPxCallbackPanel2_HostName_I\");var strLength= SearchInput.val().length * 2;SearchInput.focus();SearchInput[0].setSelectionRange(strLength, strLength);' class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
				errorDivForImportingWS.Style.Value = "display: block";

				validhostname = false;

				

			}
		}

		protected void ConnectionComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (ConnectionComboBox.SelectedItem.Text == "SOAP")
			{
				txtport.Text = "8879";
			}
			else
			{
				txtport.Text = "8702";
			}
		}
		protected void chbx_CheckedChanged(object sender, EventArgs e)
		{
			if (chbx.Checked)
			{
				CredentialsLabel.Visible = true;
				WebCredentialsComboBox.Visible = true;
				Credentialsbtn.Visible = true;

				reallbl.Visible = true;
				realmtxtbx.Visible = true;



			}
			else
			{
				CredentialsLabel.Visible = false;
				WebCredentialsComboBox.Visible = false;
				Credentialsbtn.Visible = false;

				reallbl.Visible = false;
				realmtxtbx.Visible = false;

			}

		}
		protected void Cancel_Click(object sender, EventArgs e)
		{
		//	Response.Redirect("~/Configurator/IBMConnections.aspx?ID=" + Convert.ToInt32(Session["IBMConnectionsid"].ToString()) + "&tabs=1");
            CopyProfilePopupControl.ShowOnPageLoad = false;
		}
		protected void btn_clickcopyprofile(object sender, EventArgs e)
		{
            
			CopyProfilePopupControl.ShowOnPageLoad = true;
			UserID.Visible = true;
			OKCopy.Visible = true;
			Cancel.Visible = true;
			Password.Visible = true;
			Session["cellname"] = CellnameTextBox.Text;
			Session["HostName"] = HostName.Text;

			Session["ConnectionComboBox"] = ConnectionComboBox.Text;
			Session["txtport"] = txtport.Text;
			Session["chbx"] = chbx.Checked;

			Session["realmtxtbx"] = realmtxtbx.Text;


		}//popup
		protected void Refresh_Click(object sender, EventArgs e)
		{
			//Declorations
			VitalSignsWebSphereDLL.VitalSignsWebSphereDLL WSDll = new VitalSignsWebSphereDLL.VitalSignsWebSphereDLL();
			VitalSignsWebSphereDLL.VitalSignsWebSphereDLL.CellProperties cellFromInfo = new VitalSignsWebSphereDLL.VitalSignsWebSphereDLL.CellProperties();

			//Set properties for Cell to pass to DLL
			cellFromInfo.HostName = HostName.Text.ToString();
			cellFromInfo.Port = Convert.ToInt32(txtport.Text.ToString());
			cellFromInfo.ConnectionType = ConnectionComboBox.Text.ToString();
			cellFromInfo.Realm = realmtxtbx.Text.ToString();

			Credentials creds = new Credentials();
			creds.AliasName = WebCredentialsComboBox.Text.ToString();

            DataTable CredentialIDDataTable = VSWebBL.ConfiguratorBL.IBMConnectionsServersBL.Ins.GetCredentialID(WebCredentialsComboBox.Text);
            if (CredentialIDDataTable.Rows.Count > 0)
            {
                creds.ID= int.Parse(CredentialIDDataTable.Rows[0]["ID"].ToString());

            }
			//creds.ID = Convert.ToInt32(WebCredentialsComboBox.Value.ToString());

			DataTable dt = VSWebBL.ConfiguratorBL.CredentialsBL.Ins.getCredentialsById(creds);
			if (dt.Rows.Count > 0)
			{
				string password;
				string MyObjPwd;
				string[] MyObjPwdArr;
				byte[] MyPass;
				MyObjPwd = dt.Rows[0]["Password"].ToString();

				MyObjPwdArr = MyObjPwd.Split(',');
				MyPass = new byte[MyObjPwdArr.Length];
				for (int i = 0; i < MyObjPwdArr.Length; i++)
				{
					MyPass[i] = Byte.Parse(MyObjPwdArr[i]);
				}

				VSFramework.TripleDES tripleDes = new VSFramework.TripleDES();
				password = tripleDes.Decrypt(MyPass);

				cellFromInfo.UserName = dt.Rows[0]["UserID"].ToString();
				cellFromInfo.Password = password;


			}
			else
			{
				throw new Exception("Username and Password could not be retreived");
			}


			//Call
			VitalSignsWebSphereDLL.VitalSignsWebSphereDLL.Cells cells = null;
			try
			{

				//cells = WSDll.getServerList(cellFromInfo);


				cells = WSDll.getServerList(cellFromInfo);
				foreach (VitalSignsWebSphereDLL.VitalSignsWebSphereDLL.Cell cell in cells.Cell)
				{

					bool ReturnValue;
					WebsphereCell IBMConnectionObject = new WebsphereCell();
					DataTable dt1 = new DataTable();
					IBMConnectionObject.IBMConnectionSID = int.Parse(Request.QueryString["ID"]);
					dt1 = VSWebBL.ConfiguratorBL.IBMConnectionsServersBL.Ins.GetcellID(IBMConnectionObject);
					if (dt1.Rows.Count > 0)
					{
						samecellid = Convert.ToInt32(dt1.Rows[0]["CellID"].ToString());


					}
					key = int.Parse(Request.QueryString["ID"]);
					ReturnValue = VSWebBL.ConfiguratorBL.IBMConnectionsServersBL.Ins.InsertwebsphereSametimenodesandservers(cell, samecellid, key);

				}


			}
			catch (Exception ex)
			{
				errorDivForImportingWS.Style.Value = "display: block";
				errorDivForImportingWS.InnerHtml = "An error occurred. " + ex.Message +
                    "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
				return;
			}


			//checks to see if a connection was successfully made
			//cells should never be null, it should hti the return before it is null and hits this spot
			if (cells != null && cells.Connection_Status != "CONNECTED")
			{

				errorDivForImportingWS.Style.Value = "display: block";
				errorDivForImportingWS.InnerHtml = "A connection was not able to be made."+
					 "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
				return;
			}
			else
			{
				errorDivForImportingWS.Style.Value = "display: none";

				
			}
            fillIBMConnectionsTree();
		}
		protected void OKCopy_Click(object sender, EventArgs e)
		{
			bool check = false;
			Credentials Csibject = new Credentials();

			Csibject.AliasName = AliasName.Text;

			string rawPass = Password.Text;
			byte[] encryptedPass = mytestenkey.Encrypt(rawPass);
			string encryptedPassAsString = string.Join(", ", encryptedPass.Select(s => s.ToString()).ToArray());


			//DataTable returntable = VSWebBL.ConfiguratorBL.Businesshoursbl.Ins.GetName(Csibject);
			DataTable returntable = VSWebBL.SecurityBL.webspehereImportBL.Ins.GetAliasName(Csibject);
			if (returntable.Rows.Count > 0)
			{

				Div3.InnerHtml = "This AliasName is already existed. Please enter another Name." +
					"<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
				Div3.Style.Value = "display: block";


			}
			else
			{

				//check = VSWebBL.SecurityBL.webspehereImportBL.Ins.Insertpwd(AliasName.Text, UserID.Text, encryptedPassAsString, 3);
				//Response.Redirect("~/Configurator/SametimeServer.aspx");

				check = VSWebBL.SecurityBL.webspehereImportBL.Ins.Insertpwd(AliasName.Text, UserID.Text, encryptedPassAsString, 27);
				//Response.Redirect("~/Configurator/IBMConnections.aspx?ID=" + Convert.ToInt32(Session["IBMConnectionsid"].ToString()) + "&tabs=1");
                CopyProfilePopupControl.ShowOnPageLoad = false;
                FillIBMConnectionsCredentialComboBox();
			}

		}
		protected void CollapseAllButton_Click(object sender, EventArgs e)
		{
			try
			{
				if (CollapseAllButton.Text == "Collapse All")
				{
					SametimeNodesTreeList.CollapseAll();
					CollapseAllButton.Image.Url = "~/images/icons/add.png";
					CollapseAllButton.Text = "Expand All";
				}
				else
				{
					SametimeNodesTreeList.ExpandAll();
					CollapseAllButton.Image.Url = "~/images/icons/forbidden.png";
					CollapseAllButton.Text = "Collapse All";
				}
			}
			catch (Exception ex)
			{
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
			}
		}
		protected void MaintWinListGridView_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
		{
			e.Row.Attributes.Add("onmouseover", "this.style.backgroundColor='#C0C0C0';");

			e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='white';");
		}

		protected void MaintWinListGridView_PageSizeChanged(object sender, EventArgs e)
		{
			
			VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("SametimeServer|MaintWinListGridView", MaintWinListGridView.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
			Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
		}
		protected void AlertGridView_PageSizeChanged(object sender, EventArgs e)
		{
			
			VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("SametimeServer|AlertGridView", AlertGridView.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
			Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
		}

		protected void MaintWinListGridView_SelectionChanged(object sender, EventArgs e)
		{
			if (MaintWinListGridView.Selection.Count > 0)
			{
				System.Collections.Generic.List<object> Type = MaintWinListGridView.GetSelectedFieldValues("ID");

				if (Type.Count > 0)
				{
					string ID = Type[0].ToString();

					//Mukund: VSPLUS-844, Page redirect on callback
					try
					{
						DevExpress.Web.ASPxWebControl.RedirectOnCallback("MaintenanceWin.aspx?ID=" + ID + "");
						Context.ApplicationInstance.CompleteRequest();//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
					}
					catch (Exception ex)
					{
						Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
						//throw ex;
					}
				}


			}
		}

		protected void okButton_Click(object sender, EventArgs e)
		{

			VSWebBL.SettingBL.SettingsBL.Ins.UpdateSvalue("IBM Connections Server Update", "True", VSWeb.Constants.Constants.SysString);
			try
			{
				if (Mode == "Update")
				{
							try
							{
                                //4/5/2016 Durga commented the code for VSPLUS-2905
                                //if (validhostname == true)
                                //{
                                //    ipaddress = Dns.GetHostAddresses(HostName.Text);
                                //    foreach (IPAddress var in ipaddress)
                                //    {
                                //        if (var.AddressFamily == AddressFamily.InterNetwork)
                                //        {
                                //            UIip = var.ToString();

                                //            break;
                                //        }
                                //    }
                                //    DataTable dt = VSWebBL.SecurityBL.webspehereImportBL.Ins.GetAllHostNmaes();
                                    //for (int i = 0; i < dt.Rows.Count; i++)
                                    //{
                                    //    dbipaddress = dt.Rows[i]["HostName"].ToString();
                                    //    port = dt.Rows[i]["PortNo"].ToString();
                                    //    ipaddress = Dns.GetHostAddresses(dbipaddress);

                                    //    foreach (IPAddress var in ipaddress)
                                    //    {
                                    //        if (var.AddressFamily == AddressFamily.InterNetwork)
                                    //        {
                                    //            convertdbip = var.ToString();
                                    //            break;
                                    //        }
                                    //    }

                                    //    if (convertdbip == UIip && port == txtport.Text)
                                    //    {
                                    //        if (Session["cellid"] == null)
                                    //        {
                                    //            cellnameinsert = false;
                                    //        }
                                    //        else if (Session["HostName"].ToString() == HostName.Text && Session["PortNo"].ToString() == txtport.Text)
                                    //        {
                                    //            cellnameinsert = true;
                                    //        }
                                    //        else
                                    //        {
                                    //            cellnameinsert = false;
                                    //        }
                                    //    }
                                    //}
                                    //if (cellnameinsert == true)
                                    //{
										UpdateTestsData();
									//	Insertdata();
										UpdateIBMConnectionsServers();
                                        Response.Redirect("~/Configurator/IBMConnectionsGrid.aspx");
										insertcell = true;

                                    //}

                                    //else
                                    //{
                                    //    errorDiv.InnerHtml = "Cell Name already exists with this WebSphere  configuration.Please enter another WebSphere  configuration." +
                                    //             "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                                    //    errorDiv.Style.Value = "display: block";
                                    //}
									//}
                    //            }
                    //            else
                    //            {
                    //                errorDivForImportingWS.InnerHtml = "Please enter valid Host Name." +
                    //"<button type=\"button\" onclick='javascript:var SearchInput = $(\"#ContentPlaceHolder1_ASPxPageControl1_ASPxRoundPanel7_ASPxCallbackPanel2_HostName_I\");var strLength= SearchInput.val().length * 2;SearchInput.focus();SearchInput[0].setSelectionRange(strLength, strLength);' class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                    //                errorDivForImportingWS.Style.Value = "display: block";
                    //            }
							}

							catch (Exception ex)
							{


							}

						//}

						
					
				}
				




				//..............................


                //object ReturnValue2;
                //string enabledservers = "";
                //if (Session["webcellid"] != null && Session["webcellid"] != "")
                //{
                //    int cellsid = Convert.ToInt32(Session["webcellid"].ToString());
                //    bool updatewebservers = VSWebBL.SecurityBL.webspehereImportBL.Ins.updatewebservers(cellsid);
                //    DataTable dt = GetSelectedEvents(cellsid);
                //    if (dt.Rows.Count > 0)
                //    {
                //        for (int i = 0; i < dt.Rows.Count; i++)
                //        {
                //            Servers ServersObject = new Servers();
							
                //            int serverid = Convert.ToInt32(dt.Rows[i]["ServerID"]);
							
                //            string enable = dt.Rows[i]["Enabled"].ToString();
                //            ServersObject.ServerName = dt.Rows[i]["NodeName"].ToString();
                //            ServersObject.LocationID = 1;//Location information required here
                //            string ServerName = dt.Rows[i]["NodeName"].ToString();
                //            ServerTypes STypeobject = new ServerTypes();
                //            STypeobject.ServerType = Session["ServerType"].ToString();
							
                //            ServersObject.Description = "IBM Connection WebSphere Product";
                //            ServersObject.IPAddress = "111.898.2435";

                //            ServerTypes ReturnValue = VSWebBL.SecurityBL.ServerTypesBL.Ins.GetDataForServerType(STypeobject);
                //            ServersObject.ServerTypeID = ReturnValue.ID;

                //            //	bool update = VSWebBL.SecurityBL.webspehereImportBL.Ins.updatedata(cellid, enable);
                //            DataTable returntable = VSWebBL.SecurityBL.webspehereImportBL.Ins.GetServerName(ServersObject);
                //            int sid = 0; string sname = "";
                //            if (returntable.Rows.Count == 0)
                //            {
                //                ReturnValue2 = VSWebBL.SecurityBL.webspehereImportBL.Ins.InsertwebsphereData(ServersObject);
                //                DataTable returntable1 = VSWebBL.SecurityBL.webspehereImportBL.Ins.GetServerName(ServersObject);
                //                sid = Convert.ToInt32(returntable1.Rows[0]["ID"].ToString());
                //                sname = returntable1.Rows[0]["ServerName"].ToString();
                //            }
                //            else
                //            {
                //                sid = Convert.ToInt32(returntable.Rows[0]["ID"].ToString());
                //                sname = returntable.Rows[0]["ServerName"].ToString();
                //            }

                //            //bool update = VSWebBL.SecurityBL.webspehereImportBL.Ins.updatedata(serverid, enable);
                //            bool update = VSWebBL.SecurityBL.webspehereImportBL.Ins.updatedataweb(sid, sname, enable);
                //            Object ReturnValue3 = VSWebBL.SecurityBL.ServersBL.Ins.UpdateAttributesData(CollectDataforServerAttributes(ServerName));
                //            if (dt.Rows[i]["NodeName"].ToString() != "")
                //            {
                //                Session["enabledservers"] += dt.Rows[i]["NodeName"].ToString() + ", ";
                //            }
                //        }

                //    }


                //    if (CellnameTextBox.Text == "" || HostName.Text == "" || ConnectionComboBox.Text == "")
                //    {
                //        //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please enter Cell Name, Host Name, Port no, select connection type in WebSphere  Settings Tab.')", true);

                //        ASPxPageControl1.ActiveTabIndex = 2;


                //    }
                //    else if (insertcell == false)
                //    {
                //        errorDiv.InnerHtml = "Cell Name already exists with this WebSphere  configuration.Please enter another WebSphere  configuration." +
                //                           "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                //        errorDiv.Style.Value = "display: block";

                //    }
                //    else
                //    {
                //            Session["NodesServers"] = "";
						
                //    }
					//}
					//else
					//{
					//    Session["NodesServers"] = "";
					//    Response.Redirect("~/Configurator/LotusSametimeGrid.aspx");
						
					//}

                //}
				//...................................

			}
			catch (Exception ex)
			{
				//1/7/2014 NS modified
				//throw ex;
				successDiv.Style.Value = "display: none";
				errorDiv.Style.Value = "display: block";
				//10/6/2014 NS modified for VSPLUS-990
				errorDiv.InnerHtml = "The following error has occurred: " + ex.Message +
					"<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
				//6/27/2014 NS added for VSPLUS-634
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
			}
		}

		protected void CancelButton_Click(object sender, EventArgs e)
		{
			Response.Redirect("IBMConnectionsGrid.aspx", false);
			Context.ApplicationInstance.CompleteRequest();

		}

		protected void ValidationUpdatedButton_Click(object sender, EventArgs e)
		{
			Response.Redirect("IBMConnectionsGrid.aspx", false);
			Context.ApplicationInstance.CompleteRequest();
		}

		public void UpdateIBMConnectionsServers()
		{
			SametimeServers sts = new SametimeServers();

			Object Updatedetails = VSWebBL.ConfiguratorBL.IBMConnectionsServersBL.Ins.UpdateIBMConnectionsServers(GetdataForIBMConnectionsServers());
			SetFocusOnError(Updatedetails);
			if (Updatedetails.ToString() == "True")
			{
				
				Session["IBMConnectionsStatus"] = ServerNameTextBox.Text;
				Response.Redirect("IBMConnectionsGrid.aspx", false);
				Context.ApplicationInstance.CompleteRequest();
				
			}
		}

		public IBMConnectionsServers GetdataForIBMConnectionsServers()
		{
			try
			{
				DataTable dt = new DataTable();
				IBMConnectionsServers getdata = new IBMConnectionsServers();
				
				getdata.Category = CategoryTextBox.Text;
				dt = VSWebBL.ConfiguratorBL.IBMConnectionsServersBL.Ins.GetwebspherecellforIBMCS(getdata,key);
				if (dt.Rows.Count > 0)
				{
					getdata.WSCellID = Convert.ToInt32(dt.Rows[0]["CellID"].ToString());
					Session["webspherecellid"] = Convert.ToInt32(dt.Rows[0]["CellID"].ToString());

				}
                //4/5/2016 Durga commented the code for VSPLUS-2905
				//getdata.WSCellID = Convert.ToInt32(dt.Rows[0]["CellID"].ToString());
				// getdata.Description = DescriptionTextBox.Text;
				getdata.ResponseThreshold = Convert.ToInt32(ScanIntervalTextBox.Text);
				getdata.RetryInterval = Convert.ToInt32(RetryIntervalTextBox.Text);
				getdata.OffHoursScanInterval = Convert.ToInt32(OffHoursScanIntervalTextBox.Text);
				getdata.ResponseThreshold = Convert.ToInt32(ResponseThresholdTextBox.Text);
				getdata.ScanInterval = int.Parse(ScanIntervalTextBox.Text);
				
			
				
				getdata.SSL = Convert.ToBoolean(ThisserverrequiresSSLCheckBox.Checked);
			
				getdata.Enabled = Convert.ToBoolean(EnabledForScanningCheckBox.Checked);

				DataTable CredentialIDDataTable = VSWebBL.ConfiguratorBL.IBMConnectionsServersBL.Ins.GetCredentialID(CredentialComboBox.Text);
				if (CredentialIDDataTable.Rows.Count > 0)
				{
					getdata.CredentialID = int.Parse(CredentialIDDataTable.Rows[0]["ID"].ToString());

				}


				//getdata.CredentialID = Convert.ToInt32(CredentialComboBox.Value);
				DataTable CredentialIDDataTable1 = VSWebBL.ConfiguratorBL.IBMConnectionsServersBL.Ins.GetCredentialID(DBCredentialsComboBox.Text);
				if (CredentialIDDataTable1.Rows.Count > 0)
				{
					getdata.DBCredentialsID = int.Parse(CredentialIDDataTable1.Rows[0]["ID"].ToString());

				}
				//getdata.DBCredentialsID = Convert.ToInt32(DBCredentialsComboBox.Value);
				
				//getdata.ProxyServerProtocall = ProtocolComboBox.SelectedItem.Text;
				//getdata.ProxyServerType = ProxyTypeComboBox.SelectedItem.Text;
				//getdata.ProxyServerProtocall = ProtocolComboBox.Text;
				//getdata.ProxyServerType = ProxyTypeComboBox.Text;
				getdata.DBHostName = HostNameTextBox.Text;
				getdata.DBName = DataBaseNameTextBox.Text;
				getdata.DBPort = DB2PortTextBox.Text;
				//getdata.PortNumber = PortNumberTextBox.Text;
				getdata.URL = IBMConnectionsURLTextBox.Text;
				//getdata.Platform = SortRadioButtonList1.SelectedItem.Text;
				getdata.EnableDB2port = Convert.ToBoolean(PortCheckBox.Checked);


				getdata.SID = int.Parse(lblServerID.Text);
				if (Mode == "Update")
				{
					getdata.ID = key;

				}
				getdata.FailureThreshold = int.Parse(SrvAtrFailBefAlertTextBox.Text);
				getdata.TestChatSimulation = Convert.ToBoolean(chkChatSimulation.Checked);

				getdata.ChatUser1Credentials = Convert.ToInt32(cbChatUser1.Value);
				getdata.ChatUser2Credentials = Convert.ToInt32(cbChatUser2.Value);
				


				return getdata;
			}
			catch (Exception ex)
			{
				//6/27/2014 NS added for VSPLUS-634
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}


		}

		private void SetFocusOnError(Object ReturnValue)
		{
			string ErrorMessage = ReturnValue.ToString();
			if (ErrorMessage.Substring(0, 2) == "ER")
			{
				//1/7/2014 NS modified
				//ErrorMessageLabel1.Text = ErrorMessage.Substring(3);
				//ErrorMessagePopupControl.ShowOnPageLoad = true; 
				successDiv.Style.Value = "display: none";
				//10/6/2014 NS modified for VSPLUS-990
				errorDiv.InnerHtml = ErrorMessage.Substring(3) +
					"<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
				errorDiv.Style.Value = "display: block";
			}
		}

		public void InsertIBMConnectionsServers()
		{
			//See if this IP address is already used

			IBMConnectionsServers IBMConnectionsobj = new IBMConnectionsServers();
			IBMConnectionsobj.IPAddress = IPAddressTextBox.Text;
			DataTable returntab = VSWebBL.ConfiguratorBL.IBMConnectionsServersBL.Ins.GetIPAddress(IBMConnectionsobj);
			if (returntab.Rows.Count > 0)
			{
				
				successDiv.Style.Value = "display: none";
				errorDiv.Style.Value = "display: block";
				errorDiv.InnerHtml = "The IP address you entered is already being monitored. Please enter a different IP Address." +
					"<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
				flag = true;
			}
			else
			{
				object insert = VSWebBL.ConfiguratorBL.IBMConnectionsServersBL.Ins.insertdetail(GetdataForIBMConnectionsServers());
				SetFocusOnError(insert);
				if (insert.ToString() == "True")
				{
					//1/7/2014 NS modified
					/*
					ErrorMessageLabel1.Text = "Data inserted successfully.";
					ErrorMessagePopupControl.HeaderText = "Information";
					ErrorMessagePopupControl.ShowCloseButton = false;
					ValidationUpdatedButton.Visible = true;
					ValidationOkButton.Visible = false;
					ErrorMessagePopupControl.ShowOnPageLoad = true;
					 */
					successDiv.Style.Value = "display: block";
					//10/6/2014 NS modified for VSPLUS-990
					successDiv.InnerHtml = "Sametime information inserted successfully." +
						"<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
					errorDiv.Style.Value = "display: none";
				}
			}
		}

		private void FillIBMConnectionsCredentialComboBox()
		{
			
			DataTable CredentialsDataTable = VSWebBL.ConfiguratorBL.IBMConnectionsServersBL.Ins.GetIBMConnectionsCredentials(27);
			CredentialComboBox.DataSource = CredentialsDataTable;
			CredentialComboBox.TextField = "AliasName";
			CredentialComboBox.ValueField = "ID";
			CredentialComboBox.DataBind();

			DBCredentialsComboBox.DataSource = CredentialsDataTable;
			DBCredentialsComboBox.TextField = "AliasName";
			DBCredentialsComboBox.ValueField = "ID";
			DBCredentialsComboBox.DataBind();
			
		

		}

        //public void Insertdata()
        //{
        //    bool ReturnValue;
        //    WebsphereCell webspehereObject = new WebsphereCell();
        //    IBMConnectionsServers IBMConnectionObject = new IBMConnectionsServers();
        //    DataTable dt = new DataTable();
        //    webspehereObject.IBMConnectionSID = int.Parse(Request.QueryString["ID"]);
        //    IBMConnectionObject.SID = int.Parse(Request.QueryString["ID"]);
        //    dt = VSWebBL.ConfiguratorBL.IBMConnectionsServersBL.Ins.GetwebspherecellforIBMC(IBMConnectionObject);
        //    if (dt.Rows.Count > 0)
        //    {
        //        webspehereObject.CellID = Convert.ToInt32(dt.Rows[0]["CellID"].ToString());
        //        Session["webspherecellid"] = Convert.ToInt32(dt.Rows[0]["CellID"].ToString());

        //    }
        //    webspehereObject.Name = CellnameTextBox.Text;
        //    webspehereObject.ConnectionType = ConnectionComboBox.Text;
        //    //if (ConnectionComboBox.SelectedItem.Text == "SOAP")
        //    //{
        //    //    porttextbox.Text = "8879";
        //    //}
        //    //else
        //    //{
        //    //    porttextbox.Text = "9809";
        //    //}
        //    webspehereObject.GlobalSecurity = chbx.Checked;
        //    webspehereObject.HostName = HostName.Text;
        //    if (txtport.Text != null && txtport.Text != "")
        //    {
        //        webspehereObject.PortNo = Convert.ToInt32(txtport.Text);
        //    }
        //     string Credential = WebCredentialsComboBox.Text;
        //     DataTable CredentialIDDataTable = VSWebBL.ConfiguratorBL.IBMConnectionsServersBL.Ins.GetCredentialID(WebCredentialsComboBox.Text);
        //     if (CredentialIDDataTable.Rows.Count > 0)
        //     {
        //         webspehereObject.CredentialsID = int.Parse(CredentialIDDataTable.Rows[0]["ID"].ToString());

        //     }
        //    //if (WebCredentialsComboBox.Text != null && WebCredentialsComboBox.Text != "")
        //    //{
        //    //    webspehereObject.CredentialsID = Convert.ToInt32(WebCredentialsComboBox.Value);
        //    //}
        //    webspehereObject.Realm = realmtxtbx.Text;

        //    ReturnValue = VSWebBL.ConfiguratorBL.IBMConnectionsServersBL.Ins.InsertData(webspehereObject,key);
        //    if (ReturnValue == true)
        //    {
        //        //successDiv.Style.Value = "display: block";
        //        //Response.Redirect("LotusSametimeGrid.aspx", false);
        //        //Context.ApplicationInstance.CompleteRequest();
        //    }
        //}

		private DataTable GetSelectedEvents(int CellID)
		{
			DataTable dtSel = new DataTable();
			try
			{
				dtSel.Columns.Add("CellID");
				dtSel.Columns.Add("ServerID");
				dtSel.Columns.Add("NodeID");
				dtSel.Columns.Add("Enabled");
				dtSel.Columns.Add("NodeName");
				dtSel.Columns.Add("Status");
				dtSel.Columns.Add("HostName");
				TreeListNodeIterator iterator = SametimeNodesTreeList.CreateNodeIterator();
				TreeListNode node;

				TreeListColumn columnActid = SametimeNodesTreeList.Columns["actid"];
				TreeListColumn columnSrvId = SametimeNodesTreeList.Columns["SrvId"];
				TreeListColumn columnTbl = SametimeNodesTreeList.Columns["tbl"];
				TreeListColumn columnNodeName = SametimeNodesTreeList.Columns["NodeName"];
				TreeListColumn columnStatus = SametimeNodesTreeList.Columns["Status"];
				TreeListColumn columnHostName = SametimeNodesTreeList.Columns["HostName"];

				while (true)
				{
					node = iterator.GetNext();

					if (node == null) break;



					if (node.Level == 2)
					{
						if (node.Selected)
						{

							DataRow dr = dtSel.NewRow();
							dr["CellID"] = CellID;
							dr["ServerID"] = node.GetValue("actid");
							dr["NodeID"] = node.GetValue("SrvId");
							dr["NodeName"] = node.GetValue("Name");
							dr["Status"] = node.GetValue("Status");
							dr["HostName"] = node.GetValue("HostName");
							dr["Enabled"] = true;
							dtSel.Rows.Add(dr);
						}
						else
						{
							DataRow dr = dtSel.NewRow();
							dr["CellID"] = CellID;
							dr["ServerID"] = node.GetValue("actid");
							dr["NodeID"] = node.GetValue("SrvId");
							dr["NodeName"] = node.GetValue("Name");
							dr["Status"] = node.GetValue("Status");
							dr["HostName"] = node.GetValue("HostName");
							dr["Enabled"] = false;
							dtSel.Rows.Add(dr);
						}
					}
				}


			}
			catch (Exception ex)
			{

				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
			}
			return dtSel;
		}

		public ServerAttributes CollectDataforServerAttributes(string servername)
		{
			ServerAttributes Mobj = new ServerAttributes();
			try
			{

				int ReturnValue = VSWebBL.SecurityBL.ServersBL.Ins.GetServerIDbyServerName(servername);

				Mobj.ServerId = ReturnValue;

			}
			catch (Exception ex)
			{

				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}

			return Mobj;

		}

		public void fillIBMConnectionsTree()
		{
			try
			{
				WebsphereCell IBMConnectionObject = new WebsphereCell();
				DataTable dt = new DataTable();
				IBMConnectionObject.IBMConnectionSID = int.Parse(Request.QueryString["ID"]);
				dt = VSWebBL.ConfiguratorBL.IBMConnectionsServersBL.Ins.GetcellID(IBMConnectionObject);
				if (dt.Rows.Count > 0)
				{
					samecellid = Convert.ToInt32(dt.Rows[0]["CellID"].ToString());
					Session["id"] = samecellid;

					SametimeNodesTreeList.CollapseAll();
					CollapseAllButton.Image.Url = "~/images/icons/add.png";
					CollapseAllButton.Text = "Expand All";
					if (Session["DataEvents"] == null)
					{

						DataTable bycelldata = VSWebBL.ConfiguratorBL.IBMConnectionsServersBL.Ins.FetsametimeserversbycellID(samecellid);

						Session["DataEvents"] = bycelldata;
					}
					SametimeNodesTreeList.DataSource = (DataTable)Session["DataEvents"];
					SametimeNodesTreeList.DataBind();

				}
			}
			catch (Exception ex)
			{
				//Log.Entry.Ins.Write(Server.MapPath("~/LogFiles/"), "VSPlusLog.txt", DateTime.Now.ToString() + " Error in Page: " +
				//    Request.Url.AbsolutePath + ", Method: " + System.Reflection.MethodBase.GetCurrentMethod().Name +
				//    ", Error: " + ex.ToString());

				//6/27/2014 NS added for VSPLUS-634
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
			}
		}

		private void UpdateTestsData()
		{
			try
			{
				IBMConnectionTests IBMConnectionTestsObct = new IBMConnectionTests();
				//Performance Tests
                if (chbxCreateActivity.Checked)
                {
					IBMConnectionTestsObct.ServerId = Convert.ToInt32(Request.QueryString["ID"].ToString()); ;
					IBMConnectionTestsObct.EnableSimulationTests = chbxCreateActivity.Checked ? true : chbxCreateActivity.Checked;
					IBMConnectionTestsObct.ResponseThreshold = Convert.ToInt32(string.IsNullOrEmpty(SrvAtrCreateActivityThTextBox.Text) ? "0" : SrvAtrCreateActivityThTextBox.Text);
					
                }
                else
                {
                    IBMConnectionTestsObct.ServerId = Convert.ToInt32(Request.QueryString["ID"].ToString()); ;
                    IBMConnectionTestsObct.EnableSimulationTests = chbxCreateActivity.Checked ? true : chbxCreateActivity.Checked;
                    IBMConnectionTestsObct.ResponseThreshold = Convert.ToInt32(string.IsNullOrEmpty(SrvAtrCreateActivityThTextBox.Text) ? "0" : SrvAtrCreateActivityThTextBox.Text);
				

                }
				tests = chbxCreateActivity.Text;

				IBMConnectionTestsObct.Id = VSWebBL.ConfiguratorBL.IBMConnectionsServersBL.Ins.GetIBMConnectionTestsId(tests);
				Object result31 = VSWebBL.ConfiguratorBL.IBMConnectionsServersBL.Ins.UpdateDatafortestsnew(IBMConnectionTestsObct);
				if (chbxCreateBlog.Checked)
				{
					IBMConnectionTestsObct.ServerId = Convert.ToInt32(Request.QueryString["ID"].ToString()); ;
					IBMConnectionTestsObct.EnableSimulationTests = chbxCreateBlog.Checked ? true : chbxCreateBlog.Checked;
					IBMConnectionTestsObct.ResponseThreshold = Convert.ToInt32(string.IsNullOrEmpty(SrvAtrCreateBlogThTextBox.Text) ? "0" : SrvAtrCreateBlogThTextBox.Text);
					
				}
				else
				{
					IBMConnectionTestsObct.ServerId = Convert.ToInt32(Request.QueryString["ID"].ToString()); ;
					IBMConnectionTestsObct.EnableSimulationTests = chbxCreateBlog.Checked ? true : chbxCreateBlog.Checked;
					IBMConnectionTestsObct.ResponseThreshold = Convert.ToInt32(string.IsNullOrEmpty(SrvAtrCreateBlogThTextBox.Text) ? "0" : SrvAtrCreateBlogThTextBox.Text);
					

				}
				tests = chbxCreateBlog.Text;

				IBMConnectionTestsObct.Id = VSWebBL.ConfiguratorBL.IBMConnectionsServersBL.Ins.GetIBMConnectionTestsId(tests);
				Object result4 = VSWebBL.ConfiguratorBL.IBMConnectionsServersBL.Ins.UpdateDatafortestsnew(IBMConnectionTestsObct);

				if (chbxCreateBookmark.Checked)
				{
					IBMConnectionTestsObct.ServerId = Convert.ToInt32(Request.QueryString["ID"].ToString()); ;
					IBMConnectionTestsObct.EnableSimulationTests = chbxCreateBookmark.Checked ? true : chbxCreateBookmark.Checked;
					IBMConnectionTestsObct.ResponseThreshold = Convert.ToInt32(string.IsNullOrEmpty(SrvAtrCreateBookmarkThTextBox.Text) ? "0" : SrvAtrCreateBookmarkThTextBox.Text);
				
				}
				else
				{
					IBMConnectionTestsObct.ServerId = Convert.ToInt32(Request.QueryString["ID"].ToString()); ;
					IBMConnectionTestsObct.EnableSimulationTests = chbxCreateBookmark.Checked ? true : chbxCreateBookmark.Checked;
					IBMConnectionTestsObct.ResponseThreshold = Convert.ToInt32(string.IsNullOrEmpty(SrvAtrCreateBookmarkThTextBox.Text) ? "0" : SrvAtrCreateBookmarkThTextBox.Text);
				

				}
				tests = chbxCreateBookmark.Text;

				IBMConnectionTestsObct.Id = VSWebBL.ConfiguratorBL.IBMConnectionsServersBL.Ins.GetIBMConnectionTestsId(tests);
				Object result5 = VSWebBL.ConfiguratorBL.IBMConnectionsServersBL.Ins.UpdateDatafortestsnew(IBMConnectionTestsObct);

				if (chbxCreateCommunity.Checked)
				{
					IBMConnectionTestsObct.ServerId = Convert.ToInt32(Request.QueryString["ID"].ToString()); ;
					IBMConnectionTestsObct.EnableSimulationTests = chbxCreateCommunity.Checked ? true : chbxCreateCommunity.Checked;
					IBMConnectionTestsObct.ResponseThreshold = Convert.ToInt32(string.IsNullOrEmpty(SrvAtrCreateCommunityThTextBox.Text) ? "0" : SrvAtrCreateCommunityThTextBox.Text);
					
				}
				else
				{
					IBMConnectionTestsObct.ServerId = Convert.ToInt32(Request.QueryString["ID"].ToString()); ;
					IBMConnectionTestsObct.EnableSimulationTests = chbxCreateCommunity.Checked ? true : chbxCreateCommunity.Checked;
					IBMConnectionTestsObct.ResponseThreshold = Convert.ToInt32(string.IsNullOrEmpty(SrvAtrCreateCommunityThTextBox.Text) ? "0" : SrvAtrCreateCommunityThTextBox.Text);
					

				}
				tests = chbxCreateCommunity.Text;

				IBMConnectionTestsObct.Id = VSWebBL.ConfiguratorBL.IBMConnectionsServersBL.Ins.GetIBMConnectionTestsId(tests);
				Object result6 = VSWebBL.ConfiguratorBL.IBMConnectionsServersBL.Ins.UpdateDatafortestsnew(IBMConnectionTestsObct);
				if (chbxCreateFile.Checked)
				{
					IBMConnectionTestsObct.ServerId = Convert.ToInt32(Request.QueryString["ID"].ToString()); ;
					IBMConnectionTestsObct.EnableSimulationTests = chbxCreateFile.Checked ? true : chbxCreateFile.Checked;
					IBMConnectionTestsObct.ResponseThreshold = Convert.ToInt32(string.IsNullOrEmpty(SrvAtrCreateFileThTextBox.Text) ? "0" : SrvAtrCreateFileThTextBox.Text);
				
				}
				else
				{
					IBMConnectionTestsObct.ServerId = Convert.ToInt32(Request.QueryString["ID"].ToString()); ;
					IBMConnectionTestsObct.EnableSimulationTests = chbxCreateFile.Checked ? true : chbxCreateFile.Checked;
					IBMConnectionTestsObct.ResponseThreshold = Convert.ToInt32(string.IsNullOrEmpty(SrvAtrCreateFileThTextBox.Text) ? "0" : SrvAtrCreateFileThTextBox.Text);
					

				}
				tests = chbxCreateFile.Text;

				IBMConnectionTestsObct.Id = VSWebBL.ConfiguratorBL.IBMConnectionsServersBL.Ins.GetIBMConnectionTestsId(tests);
				Object result7 = VSWebBL.ConfiguratorBL.IBMConnectionsServersBL.Ins.UpdateDatafortestsnew(IBMConnectionTestsObct);
				if (chbxCreateWiki.Checked)
				{
					IBMConnectionTestsObct.ServerId = Convert.ToInt32(Request.QueryString["ID"].ToString()); ;
					IBMConnectionTestsObct.EnableSimulationTests = chbxCreateWiki.Checked ? true : chbxCreateWiki.Checked;
					IBMConnectionTestsObct.ResponseThreshold = Convert.ToInt32(string.IsNullOrEmpty(SrvAtrCreateWikiThTextBox.Text) ? "0" : SrvAtrCreateWikiThTextBox.Text);
					
				}
				else
				{
					IBMConnectionTestsObct.ServerId = Convert.ToInt32(Request.QueryString["ID"].ToString()); ;
					IBMConnectionTestsObct.EnableSimulationTests = chbxCreateWiki.Checked ? true : chbxCreateWiki.Checked;
					IBMConnectionTestsObct.ResponseThreshold = Convert.ToInt32(string.IsNullOrEmpty(SrvAtrCreateWikiThTextBox.Text) ? "0" : SrvAtrCreateWikiThTextBox.Text);
					

				}
				tests = chbxCreateWiki.Text;

				IBMConnectionTestsObct.Id = VSWebBL.ConfiguratorBL.IBMConnectionsServersBL.Ins.GetIBMConnectionTestsId(tests);
				Object result8 = VSWebBL.ConfiguratorBL.IBMConnectionsServersBL.Ins.UpdateDatafortestsnew(IBMConnectionTestsObct);

				if (chbxSearchProfile.Checked)
				{
					IBMConnectionTestsObct.ServerId = Convert.ToInt32(Request.QueryString["ID"].ToString()); ;
					IBMConnectionTestsObct.EnableSimulationTests = chbxSearchProfile.Checked ? true : chbxSearchProfile.Checked;
					IBMConnectionTestsObct.ResponseThreshold = Convert.ToInt32(string.IsNullOrEmpty(SrvAtrSearchProfileThTextBox.Text) ? "0" : SrvAtrSearchProfileThTextBox.Text);
					
				}
				else
				{
					IBMConnectionTestsObct.ServerId = Convert.ToInt32(Request.QueryString["ID"].ToString()); ;
					IBMConnectionTestsObct.EnableSimulationTests = chbxSearchProfile.Checked ? true : chbxSearchProfile.Checked;
					IBMConnectionTestsObct.ResponseThreshold = Convert.ToInt32(string.IsNullOrEmpty(SrvAtrSearchProfileThTextBox.Text) ? "0" : SrvAtrSearchProfileThTextBox.Text);
					

				}
				tests = chbxSearchProfile.Text;

				IBMConnectionTestsObct.Id = VSWebBL.ConfiguratorBL.IBMConnectionsServersBL.Ins.GetIBMConnectionTestsId(tests);
				Object result9 = VSWebBL.ConfiguratorBL.IBMConnectionsServersBL.Ins.UpdateDatafortestsnew(IBMConnectionTestsObct);
			}
			catch (Exception ex)
			{
				//    //10/13/2014 NS modified for VSPLUS-990
				//    errorDiv.Visible = true;
				//    errorDiv.InnerHtml = "The following error has occurred: " + ex.Message +
				//        "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
				//    errorDiv.Style.Value = "display: block";
				//    //6/27/2014 NS added for VSPLUS-634
				//    Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
			}
		}

		private void FilltestsData(int key)
		{
			try
			{
				
				IBMConnectionTests IBMConnectionTestsObct = new IBMConnectionTests();
				//O365Server ReturnDCObject = new O365Server();
				IBMConnectionTestsObct.ServerId = Convert.ToInt32(key);
				int i = 0;
				DataTable testsdt = VSWebBL.ConfiguratorBL.IBMConnectionsServersBL.Ins.GetIBMConnectionTestsData(Convert.ToInt32(key));
				for (i = 0; testsdt.Rows.Count > i; i++)
				{

					if (testsdt.Rows[i]["Tests"].ToString() == "Create Activity")
					{
						chbxCreateActivity.Checked = Convert.ToBoolean(testsdt.Rows[i]["EnableSimulationTests"]);
						SrvAtrCreateActivityThTextBox.Text = testsdt.Rows[i]["ResponseThreshold"].ToString();
					}
					if (testsdt.Rows[i]["Tests"].ToString() == "Create Blog")
					{
						chbxCreateBlog.Checked = Convert.ToBoolean(testsdt.Rows[i]["EnableSimulationTests"]);
						SrvAtrCreateBlogThTextBox.Text = testsdt.Rows[i]["ResponseThreshold"].ToString();
					}
					if (testsdt.Rows[i]["Tests"].ToString() == "Create Bookmark")
					{
						chbxCreateBookmark.Checked = Convert.ToBoolean(testsdt.Rows[i]["EnableSimulationTests"]);
						SrvAtrCreateBookmarkThTextBox.Text = testsdt.Rows[i]["ResponseThreshold"].ToString();
					}
					if (testsdt.Rows[i]["Tests"].ToString() == "Create Community")
					{
						chbxCreateCommunity.Checked = Convert.ToBoolean(testsdt.Rows[i]["EnableSimulationTests"]);
						SrvAtrCreateCommunityThTextBox.Text = testsdt.Rows[i]["ResponseThreshold"].ToString();

					}
					if (testsdt.Rows[i]["Tests"].ToString() == "Create File")
					{
						chbxCreateFile.Checked = Convert.ToBoolean(testsdt.Rows[i]["EnableSimulationTests"]);
						SrvAtrCreateFileThTextBox.Text = testsdt.Rows[i]["ResponseThreshold"].ToString();
					}
					if (testsdt.Rows[i]["Tests"].ToString() == "Create Wiki")
					{
						chbxCreateWiki.Checked = Convert.ToBoolean(testsdt.Rows[i]["EnableSimulationTests"]);
						SrvAtrCreateWikiThTextBox.Text = testsdt.Rows[i]["ResponseThreshold"].ToString();
					}
					if (testsdt.Rows[i]["Tests"].ToString() == "Search Profile")
					{
						chbxSearchProfile.Checked = Convert.ToBoolean(testsdt.Rows[i]["EnableSimulationTests"]);
						SrvAtrSearchProfileThTextBox.Text = testsdt.Rows[i]["ResponseThreshold"].ToString();
					}

				}
				
				//Session["ReturnUrl"] = "O365ServerProperties.aspx?ID=" + ID + "&tab=1";

			}
			catch (Exception ex)
			{
				//6/27/2014 NS added for VSPLUS-634
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				//throw ex;
			}
			finally { }
		}

		private void FillWebsphereCell(int IBMConnectionSID)
		{
			WebsphereCell IBMConnectionObject = new WebsphereCell();
			DataTable dt = new DataTable();
			//sametimeObject.SametimeId = int.Parse(Request.QueryString["ID"]);
			IBMConnectionObject.IBMConnectionSID = Convert.ToInt32(Session["IBMConnectionsid"].ToString());
			dt = VSWebBL.ConfiguratorBL.IBMConnectionsServersBL.Ins.GetcellID(IBMConnectionObject);

			int Cellid = Convert.ToInt32(dt.Rows[0]["CellID"].ToString());
			Session["cellid"] = Cellid;
			CellnameTextBox.Text = dt.Rows[0]["Name"].ToString();
			Session["cellinfo"] = dt.Rows[0]["Name"].ToString();

			HostName.Text = dt.Rows[0]["HostName"].ToString();
			Session["HostName"] = dt.Rows[0]["HostName"].ToString();
			ConnectionComboBox.Text = dt.Rows[0]["ConnectionType"].ToString();
			txtport.Text = dt.Rows[0]["PortNo"].ToString();
			Session["PortNo"] = dt.Rows[0]["PortNo"].ToString();


			chbx.Checked = Convert.ToBoolean(dt.Rows[0]["GlobalSecurity"].ToString());
			if (chbx.Checked == true)
			{


				CredentialsLabel.Visible = true;
				reallbl.Visible = true;
				WebCredentialsComboBox.Visible = true;
				realmtxtbx.Visible = true;
				//WebCredentialsComboBox.Value = Convert.ToInt32(dt.Rows[0]["CredentialsID"].ToString());
				int credentialid = Convert.ToInt32(dt.Rows[0]["CredentialsID"].ToString());


				Credentials cred = new Credentials();
				cred.ID = credentialid;
				DataTable celldt = VSWebBL.ConfiguratorBL.CredentialsBL.Ins.getCredentialsById(cred);

				WebCredentialsComboBox.Text = celldt.Rows[0]["AliasName"].ToString();
				realmtxtbx.Text = dt.Rows[0]["Realm"].ToString();
			}

		}


        public void fillNodesTreeListbycellIDfromsession()
        {
            try
            {

                if (Session["DataEvents"] != null)
                {
                    SametimeNodesTreeList.DataSource = (DataTable)Session["DataEvents"];
                    SametimeNodesTreeList.DataBind();
                }

            }
            catch (Exception ex)
            {

                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
            }
        }

		
	}
}