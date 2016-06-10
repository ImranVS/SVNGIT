using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;

using VSFramework;
using VSWebBL;
using VSWebDO;

using DevExpress.Web;


namespace VSWebUI.Configurator
{
	public partial class O365ServerProperties : System.Web.UI.Page
	{
		VSFramework.TripleDES tripleDes = new VSFramework.TripleDES();
		string Mode;
		Object result2;
		string ID;
		string modemes;
		bool flag;
		bool checkedvalue;
		DataTable name = new DataTable();
		protected void Page_Load(object sender, EventArgs e)
		{
			//UserNameTextBox.Attributes.Add("autocomplete", "off");
			//PasswordTextBox.Attributes.Add("autocomplete", "off");
			Response.Cache.SetNoStore();
			if (ASPxPageControl1.ActiveTabPage.Text == "Tests/Options")
			{
				if (Request.QueryString["tab"] != null)
				{
					ASPxPageControl1.ActiveTabIndex = Convert.ToInt32(Request.QueryString["tab"].ToString());
					Resetoffice365Button.Visible = true;
					
				}
			}
			if (!IsPostBack)
			{

				if (Request.QueryString["Mode"] != null)
				{
					string Mode = Request.QueryString["Mode"].ToString();
					modemes = Request.QueryString["Mode"].ToString();
					Session["mode"] = modemes;
					if (Mode == "Insert")
					{
						Session["ID"] = null;
					}
				}
				FillCredentialsComboBox();
				int id = 0;
				if (Request.QueryString["ID"] != null)
				{
					id = Convert.ToInt32(Request.QueryString["ID"].ToString());

					name = VSWebBL.ConfiguratorBL.O365ServerBL.Ins.Get(id.ToString());

					if (name.Rows.Count > 0)
					{
						//FillNodesGridView();
						Session["ID"] = name.Rows[0][0].ToString();

					}
					else
					{
						//UpdateNodesData();
						Session["ID"] = "0";

					}
				}
				if (Session["UserPreferences"] != null)
				{
					DataTable UserPreferences = (DataTable)Session["UserPreferences"];
					foreach (DataRow dr in UserPreferences.Rows)
					{
						if (dr[1].ToString() == "O365ServerServerProperties|MaintWinListGridView")
						{
							MaintWinListGridView.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
						}
						if (dr[1].ToString() == "O365ServerServerProperties|AlertGridView")
						{
							AlertGridView.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
						}

						if (dr[1].ToString() == "O365ServerProperties|NodesGridView")
						{
							Nodes.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
						}

					}
				}
				//FillTestsGridView();
				FillNodesGridView();


			}
			else
			{
				errorDiv.Visible = false;
				errorDiv.Style.Value = "display: none";
				//FillTestsGridViewfromSession();
				FillNodesGridViewfromSession();
				if (Session["O365ServerUpdateStatus"] == "")
				{
					successDiv1.InnerHtml = "";
					successDiv1.Style.Value = "display: none";

				}
				if (Session["NodeError"] == "")
				{
					Div1.InnerHtml = "";
					Div1.Style.Value = "display: none";
				}
			}
			//2/11/2016 NS modified for VSPLUS-2568
			Page.Title = "Office 365 Server Properties";
			if (Request.QueryString["tab"] != null)
			{
				ASPxPageControl1.ActiveTabIndex = Convert.ToInt32(Request.QueryString["tab"].ToString());
			}

			try
			{
				if ((Request.QueryString["ID"] != null && Request.QueryString["ID"] != ""))
				{
					if ((Session["ID"].ToString() != "0") && (Request.QueryString["ID"] != null))
					{
						Mode = "Update";
						NameTextBox.Enabled = false;

						//11/19/2013 NS commented out
						//URL = Request.QueryString["TheURL"];
						//ID = Session["ID"].ToString();
						EnabledCheckBox.Enabled = true;
						if ((Request.QueryString["ID"] != null && Request.QueryString["ID"] != ""))
						{
							ID = Request.QueryString["ID"];
						}

						if (!IsPostBack)
						{
							FillData(ID);
							FilltestsData(ID);
							FillMaintenanceGrid();
							FillAlertGridView();
							if (Request.QueryString["add"] == "OK")
							{

								SortRadioButtonList1.SelectedIndex = 2;
								DirSyncPanel.Visible = true;
								txtservername.Text = Session["SeverName"].ToString();
								creComboBox.Text = Session["Credentials"].ToString();
								//IPAddressTextBox.Text = Session["ipaddress"].ToString();
								CategoryTextBox.Text = Session["category"].ToString();
								EnabledCheckBox.Checked = Convert.ToBoolean(Session["checked"]);
								NameTextBox.Text = Session["Name"].ToString();
								ScanTextBox.Text = Session["ScanInterval"].ToString();
								RespThrTextBox.Text = Session["ResponseThreshold"].ToString();
								RetryTextBox.Text = Session["RetryInterval"].ToString();
								SrvAtrFailBefAlertTextBox.Text = Session["SrvAtrFailBefAlertTextBox"].ToString();
								OffScanTextBox.Text = Session["OffScan"].ToString();
								UserNameTextBox.Text = Session["Uname"].ToString();
								PasswordTextBox.Text = Session["Pwd"].ToString();
                                CostperuserTextBox.Text = Session["Costperuser"].ToString();
							}

							servernamelbldisp.InnerHtml += " - " + NameTextBox.Text;
						}
						else
						{
							FillMaintServersGridfromSession();
						}
					}
				}
				else
				{
					Mode = "Insert";
					if (!IsPostBack)
					{
						//filldropdown();
						RetryTextBox.Text = "2";
						OffScanTextBox.Text = "30";
						ScanTextBox.Text = "8";
						//IPAddressTextBox.Text = "http://";
						RespThrTextBox.Text = "2500";
						if (Request.QueryString["add"] == "OK")
						{
							SortRadioButtonList1.SelectedIndex = 2;
							DirSyncPanel.Visible = true;
							txtservername.Text = Session["SeverName"].ToString();
							creComboBox.Text = Session["Credentials"].ToString();
							//IPAddressTextBox.Text = Session["ipaddress"].ToString();
							CategoryTextBox.Text = Session["category"].ToString();
							EnabledCheckBox.Checked = Convert.ToBoolean(Session["checked"]);
							NameTextBox.Text = Session["Name"].ToString();
							ScanTextBox.Text = Session["ScanInterval"].ToString();
							RespThrTextBox.Text = Session["ResponseThreshold"].ToString();
							RetryTextBox.Text = Session["RetryInterval"].ToString();
							SrvAtrFailBefAlertTextBox.Text = Session["SrvAtrFailBefAlertTextBox"].ToString();
							OffScanTextBox.Text = Session["OffScan"].ToString();
							UserNameTextBox.Text = Session["Uname"].ToString();
							PasswordTextBox.Text = Session["Pwd"].ToString();
                            CostperuserTextBox.Text = Session["Costperuser"].ToString();
						}
					}
				}
			}
			catch (Exception ex)
			{
				//6/27/2014 NS added for VSPLUS-634
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				//throw ex;
			}
			finally { }
		}
		private void FillData(string ID)
		{
			try
			{
				O365Server O365ServerObject = new O365Server();
				O365Server ReturnDCObject = new O365Server();
				O365ServerObject.ID = ID;
				ReturnDCObject = VSWebBL.ConfiguratorBL.O365ServerBL.Ins.GetData(O365ServerObject);
				//Cluster Setting fields
				NameTextBox.Text = ReturnDCObject.Name.ToString();
				CategoryTextBox.Text = ReturnDCObject.Category.ToString();
                CostperuserTextBox.Text =  ReturnDCObject.Costperuser.ToString();
				string modevalue = ReturnDCObject.mode.ToString();
				if ((modevalue == "ADFS"))
				{
					SortRadioButtonList1.SelectedIndex = 0;
					DirSyncPanel.Visible = false;
					//lblservername.Visible = false;
					//txtservername.Visible = false;
					//lblcrecombo.Visible = false;

					//creComboBox.Visible = false;
					//ASPxButton1.Visible = false;
				}
				if (modevalue == "Cloud Only")
				{
					SortRadioButtonList1.SelectedIndex = 1;
					DirSyncPanel.Visible = false;
					//lblservername.Visible = false;
					//txtservername.Visible = false;
					//lblcrecombo.Visible = false;

					//creComboBox.Visible = false;
					//ASPxButton1.Visible = false;
				}
				if (modevalue == "Dir Sync")
				{
					SortRadioButtonList1.SelectedIndex = 2;
					DirSyncPanel.Visible = true;
					//lblservername.Visible = true;
					//txtservername.Visible = true;
					//lblcrecombo.Visible = true;

					//creComboBox.Visible = true;
					//ASPxButton1.Visible = true;
					//NameTextBox.Visible = true;
				}

				txtservername.Text = ReturnDCObject.servername.ToString();
				int crid = Convert.ToInt32(ReturnDCObject.CredentialsId);
				DataTable cell1 = VSWebBL.SecurityBL.webspehereImportBL.Ins.GetCrentials(crid);
				if (cell1.Rows.Count > 0)
				{
					creComboBox.Text = cell1.Rows[0]["AliasName"].ToString();
				}
				else
				{
					creComboBox.Text = "";
				}
				ScanTextBox.Text = ReturnDCObject.ScanInterval.ToString();
				OffScanTextBox.Text = ReturnDCObject.OffHoursScanInterval.ToString();
				EnabledCheckBox.Checked = ReturnDCObject.Enabled;//(ScanTextBox.Text != null ? true : false);
				RetryTextBox.Text = ReturnDCObject.RetryInterval.ToString();
				RespThrTextBox.Text = ReturnDCObject.ResponseThreshold.ToString();
				//IPAddressTextBox.Text = ReturnDCObject.URL;
				UserNameTextBox.Text = ReturnDCObject.UserName;
				PasswordTextBox.Text = "      ";
				string MyObjPwd;
				string[] MyObjPwdArr;
				byte[] MyPass;

				MyObjPwd = ReturnDCObject.PW.ToString();

				if (MyObjPwd != "")
				{
					MyObjPwdArr = MyObjPwd.Split(',');
					MyPass = new byte[MyObjPwdArr.Length];

					try
					{
						for (int j = 0; j < MyObjPwdArr.Length; j++)
						{
							MyPass[j] = Byte.Parse(MyObjPwdArr[j]);
						}
						hdnPwd.Value = tripleDes.Decrypt(MyPass);//soma
					}
					catch (Exception ex)
					{
						if (ex.Message == "Input string was not in a correct format.")
						{
							hdnPwd.Value = MyObjPwd;//soma
						}
					}

				}
				SrvAtrFailBefAlertTextBox.Text = ReturnDCObject.FailureThreshold.ToString();
				Session["ReturnUrl"] = "O365ServerProperties.aspx?ID=" + ID + "&tab=1";

			}
			catch (Exception ex)
			{
				//6/27/2014 NS added for VSPLUS-634
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				//throw ex;
			}
			finally { }
		}
		private void FilltestsData(string ID)
		{
			try
			{
				Office365Tests O365ServerObject = new Office365Tests();
				//O365Server ReturnDCObject = new O365Server();
				O365ServerObject.ServerId = Convert.ToInt32(ID);
				int i;
				DataTable testsdt = VSWebBL.ConfiguratorBL.O365ServerBL.Ins.GetOffice365TestsData(Convert.ToInt32(ID));
				//int count = testsdt.Rows.Count - 1;
				for (i = 0; testsdt.Rows.Count > i; i++)
				{
					if (testsdt.Rows[i]["Tests"].ToString() == "SMTP")
					{
						chbxsmtp.Checked = Convert.ToBoolean(testsdt.Rows[i]["EnableSimulationTests"]);
					}
					if (testsdt.Rows[i]["Tests"].ToString() == "POP")
					{
						chbxpop.Checked = Convert.ToBoolean(testsdt.Rows[i]["EnableSimulationTests"]); ;
					}
					if (testsdt.Rows[i]["Tests"].ToString() == "IMAP")
					{
						chbximap.Checked = Convert.ToBoolean(testsdt.Rows[i]["EnableSimulationTests"]); ;
					}
					if (testsdt.Rows[i]["Tests"].ToString() == "OWA")
					{
						chbxowa.Checked = Convert.ToBoolean(testsdt.Rows[i]["EnableSimulationTests"]); ;
					}
					if (testsdt.Rows[i]["Tests"].ToString() == "Auto Discovery")
					{
						chbxautodiscovery.Checked = Convert.ToBoolean(testsdt.Rows[i]["EnableSimulationTests"]); ;
					}
					if (testsdt.Rows[i]["Tests"].ToString() == "MAPI Connectivity")
					{
						chbxmapi.Checked = Convert.ToBoolean(testsdt.Rows[i]["EnableSimulationTests"]); ;
					}
					if (testsdt.Rows[i]["Tests"].ToString() == "Create Calendar")
					{
						chbxcalender.Checked = Convert.ToBoolean(testsdt.Rows[i]["EnableSimulationTests"]); ;
					}
					if (testsdt.Rows[i]["Tests"].ToString() == "Inbox")
					{
						chbxinbox.Checked = Convert.ToBoolean(testsdt.Rows[i]["EnableSimulationTests"]); ;
					}
					if (testsdt.Rows[i]["Tests"].ToString() == "Create Task")
					{
						chbxtask.Checked = Convert.ToBoolean(testsdt.Rows[i]["EnableSimulationTests"]);
					}
					if (testsdt.Rows[i]["Tests"].ToString() == "Mail Flow Test")
					{
						chbxmailflow.Checked = Convert.ToBoolean(testsdt.Rows[i]["EnableSimulationTests"]);
						SrvAtrMailFlowThTextBox.Text = testsdt.Rows[i]["ResponseThreshold"].ToString();
					}
					if (testsdt.Rows[i]["Tests"].ToString() == "Create Folder Test")
					{
						chbxcreatefolder.Checked = Convert.ToBoolean(testsdt.Rows[i]["EnableSimulationTests"]);
						SrvAtrCreateFolderThTextBox.Text = testsdt.Rows[i]["ResponseThreshold"].ToString();
					}
					if (testsdt.Rows[i]["Tests"].ToString() == "Create Site Test")
					{
						chbxcreatesite.Checked = Convert.ToBoolean(testsdt.Rows[i]["EnableSimulationTests"]);
						srvAtrCreateSiteThTextBox.Text = testsdt.Rows[i]["ResponseThreshold"].ToString();
					}
					if (testsdt.Rows[i]["Tests"].ToString() == "OneDrive Upload Test")
					{
						chbxonedriveupload.Checked = Convert.ToBoolean(testsdt.Rows[i]["EnableSimulationTests"]);
						SrvAtrODUploadThTextBox.Text = testsdt.Rows[i]["ResponseThreshold"].ToString();
					}
					if (testsdt.Rows[i]["Tests"].ToString() == "OneDrive Download Test")
					{
						chbxonedrivedownload.Checked = Convert.ToBoolean(testsdt.Rows[i]["EnableSimulationTests"]);
						SrvAtrODDownloadThTextBox.Text = testsdt.Rows[i]["ResponseThreshold"].ToString();

					}
					if (testsdt.Rows[i]["Tests"].ToString() == "Dir Sync Imp/Export Test")
					{
						chbxdirsync.Checked = Convert.ToBoolean(testsdt.Rows[i]["EnableSimulationTests"]);
						dirsynctxt.Text = testsdt.Rows[i]["ResponseThreshold"].ToString();
					}

				}
				//Cluster Setting fields
				//NameTextBox.Text = ReturnDCObject.Name.ToString();
				//CategoryTextBox.Text = ReturnDCObject.Category.ToString();
				//string modevalue = ReturnDCObject.mode.ToString();
				//if ((modevalue == "ADFS"))
				//{
				//    SortRadioButtonList1.SelectedIndex = 0;
				//    DirSyncPanel.Visible = false;
				//    //lblservername.Visible = false;
				//    //txtservername.Visible = false;
				//    //lblcrecombo.Visible = false;

				//    //creComboBox.Visible = false;
				//    //ASPxButton1.Visible = false;
				//}
				//if (modevalue == "Cloud Only")
				//{
				//    SortRadioButtonList1.SelectedIndex = 1;
				//    DirSyncPanel.Visible = false;
				//    //lblservername.Visible = false;
				//    //txtservername.Visible = false;
				//    //lblcrecombo.Visible = false;

				//    //creComboBox.Visible = false;
				//    //ASPxButton1.Visible = false;
				//}
				//if (modevalue == "Dir Sync")
				//{
				//    SortRadioButtonList1.SelectedIndex = 2;
				//    DirSyncPanel.Visible = true;
				//    //lblservername.Visible = true;
				//    //txtservername.Visible = true;
				//    //lblcrecombo.Visible = true;

				//    //creComboBox.Visible = true;
				//    //ASPxButton1.Visible = true;
				//    //NameTextBox.Visible = true;
				//}

				//txtservername.Text = ReturnDCObject.servername.ToString();
				//int crid = Convert.ToInt32(ReturnDCObject.CredentialsId);
				//DataTable cell1 = VSWebBL.SecurityBL.webspehereImportBL.Ins.GetCrentials(crid);
				//if (cell1.Rows.Count > 0)
				//{
				//    creComboBox.Text = cell1.Rows[0]["AliasName"].ToString();
				//}
				//else
				//{
				//    creComboBox.Text = "";
				//}
				//ScanTextBox.Text = ReturnDCObject.ScanInterval.ToString();
				//OffScanTextBox.Text = ReturnDCObject.OffHoursScanInterval.ToString();
				//EnabledCheckBox.Checked = ReturnDCObject.Enabled;//(ScanTextBox.Text != null ? true : false);
				//RetryTextBox.Text = ReturnDCObject.RetryInterval.ToString();
				//RespThrTextBox.Text = ReturnDCObject.ResponseThreshold.ToString();
				////IPAddressTextBox.Text = ReturnDCObject.URL;
				//UserNameTextBox.Text = ReturnDCObject.UserName;
				//PasswordTextBox.Text = "      ";
				//string MyObjPwd;
				//string[] MyObjPwdArr;
				//byte[] MyPass;

				//MyObjPwd = ReturnDCObject.PW.ToString();

				//if (MyObjPwd != "")
				//{
				//    MyObjPwdArr = MyObjPwd.Split(',');
				//    MyPass = new byte[MyObjPwdArr.Length];

				//    try
				//    {
				//        for (int j = 0; j < MyObjPwdArr.Length; j++)
				//        {
				//            MyPass[j] = Byte.Parse(MyObjPwdArr[j]);
				//        }
				//        hdnPwd.Value = tripleDes.Decrypt(MyPass);//soma
				//    }
				//    catch (Exception ex)
				//    {
				//        if (ex.Message == "Input string was not in a correct format.")
				//        {
				//            hdnPwd.Value = MyObjPwd;//soma
				//        }
				//    }

				//}
				//SrvAtrFailBefAlertTextBox.Text = ReturnDCObject.FailureThreshold.ToString();
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
		private void FillCredentialsComboBox()
		{
			DataTable CredentialsDataTable = VSWebBL.ConfiguratorBL.ServicesBL.Ins.GetWebsphereserverCredentials(21);
			creComboBox.DataSource = CredentialsDataTable;
			creComboBox.TextField = "AliasName";
			creComboBox.ValueField = "ID";
			creComboBox.DataBind();
		}
		private O365Server CollectDataForO365Server()
		{
			try
			{
				//Cluster Settings
				O365Server O365ServerObject = new O365Server();
				//12/9/2013 NS added
				O365ServerObject.ID = ID;
				O365ServerObject.Name = NameTextBox.Text;
				O365ServerObject.Category = CategoryTextBox.Text;
                O365ServerObject.Costperuser = CostperuserTextBox.Text;
				O365ServerObject.Enabled = EnabledCheckBox.Checked;
				O365ServerObject.mode = SortRadioButtonList1.SelectedItem.Text;
				O365ServerObject.servername = txtservername.Text;
				DataTable dt = VSWebBL.SecurityBL.webspehereImportBL.Ins.GetCredentialValue(creComboBox.Text);
				if (dt.Rows.Count > 0)
					O365ServerObject.CredentialsId = Convert.ToInt32(dt.Rows[0]["ID"].ToString());
				O365ServerObject.OffHoursScanInterval = int.Parse(OffScanTextBox.Text);
				O365ServerObject.ScanInterval = int.Parse(ScanTextBox.Text);
				O365ServerObject.RetryInterval = int.Parse(RetryTextBox.Text);
				O365ServerObject.ResponseThreshold = int.Parse(RespThrTextBox.Text);
				//O365ServerObject.SearchStringNotFound = RequiredTextBox.Text;
				//O365ServerObject.SearchStringFound = txtSearch.Text;
				//O365ServerObject.URL = IPAddressTextBox.Text;
				O365ServerObject.imageurl = Image1.ImageUrl;
				O365ServerObject.UserName = UserNameTextBox.Text;
				if (string.IsNullOrEmpty(PasswordTextBox.Text.Trim()))
					PasswordTextBox.Text = hdnPwd.Value;//soma
				TripleDES tripleDES = new TripleDES();
				byte[] encryptedPass = tripleDES.Encrypt(PasswordTextBox.Text);
				string encryptedPassAsString = string.Join(", ", encryptedPass.Select(s => s.ToString()).ToArray());
				O365ServerObject.PW = encryptedPassAsString;
				//ASPxCheckBox EnableSimulationTestsCheckBox = (ASPxCheckBox)Tests.FindEditFormTemplateControl("EnableSimulationTestsCheckBox");
				Locations LOCobject = new Locations();
				ServerTypes STypeobject = new ServerTypes();
				STypeobject.ServerType = "Office365";
				ServerTypes ReturnValue = VSWebBL.SecurityBL.ServerTypesBL.Ins.GetDataForServerType(STypeobject);
				O365ServerObject.ServerTypeId = ReturnValue.ID;
				Locations ReturnLocValue = VSWebBL.SecurityBL.LocationsBL.Ins.GetDataForLocation(LOCobject);
				O365ServerObject.LocationId = ReturnLocValue.ID;
				O365ServerObject.Location = ReturnLocValue.Location;
				O365ServerObject.FailureThreshold = int.Parse(SrvAtrFailBefAlertTextBox.Text);
				return O365ServerObject;
			}
			catch (Exception ex)
			{
				//6/27/2014 NS added for VSPLUS-634
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}
			finally { }
		}
		private Office365Tests CollectDataForO365Tests()
		{
			try
			{
				//Pass and Fail tests
				Office365Tests O365testsObject = new Office365Tests();
				if (chbxsmtp.Checked)
				{
					O365testsObject.ServerId = Convert.ToInt32(Session["ServerID"]);
					O365testsObject.EnableSimulationTests = chbxsmtp.Checked ? true : chbxsmtp.Checked;
					O365testsObject.Type = "Service Availability Tests";
					O365testsObject.Tests = chbxsmtp.Text;
					Object result2 = VSWebBL.ConfiguratorBL.O365ServerBL.Ins.InsertDatafortestsnew(O365testsObject);
				}
				if (chbximap.Checked)
				{
					O365testsObject.ServerId = Convert.ToInt32(Session["ServerID"]);
					O365testsObject.EnableSimulationTests = chbximap.Checked ? true : chbximap.Checked;
					O365testsObject.Type = "Service Availability Tests";
					O365testsObject.Tests = chbximap.Text;
					Object result2 = VSWebBL.ConfiguratorBL.O365ServerBL.Ins.InsertDatafortestsnew(O365testsObject);
				}
				if (chbxowa.Checked)
				{
					O365testsObject.ServerId = Convert.ToInt32(Session["ServerID"]);
					O365testsObject.EnableSimulationTests = chbxowa.Checked ? true : chbxowa.Checked;
					O365testsObject.Type = "Service Availability Tests";
					O365testsObject.Tests = chbxowa.Text;
					Object result2 = VSWebBL.ConfiguratorBL.O365ServerBL.Ins.InsertDatafortestsnew(O365testsObject);
				}
				if (chbxpop.Checked)
				{
					O365testsObject.ServerId = Convert.ToInt32(Session["ServerID"]);
					O365testsObject.EnableSimulationTests = chbxpop.Checked ? true : chbxpop.Checked;
					O365testsObject.Type = "Service Availability Tests";
					O365testsObject.Tests = chbxpop.Text;
					Object result2 = VSWebBL.ConfiguratorBL.O365ServerBL.Ins.InsertDatafortestsnew(O365testsObject);
				}
				if (chbxautodiscovery.Checked)
				{
					O365testsObject.ServerId = Convert.ToInt32(Session["ServerID"]);
					O365testsObject.EnableSimulationTests = chbxautodiscovery.Checked ? true : chbxautodiscovery.Checked;
					O365testsObject.Type = "Service Availability Tests";
					O365testsObject.Tests = chbxautodiscovery.Text;
					Object result2 = VSWebBL.ConfiguratorBL.O365ServerBL.Ins.InsertDatafortestsnew(O365testsObject);
				}
				if (chbxmapi.Checked)
				{
					O365testsObject.ServerId = Convert.ToInt32(Session["ServerID"]);
					O365testsObject.EnableSimulationTests = chbxmapi.Checked ? true : chbxmapi.Checked;
					O365testsObject.Type = "Service Availability Tests";
					O365testsObject.Tests = chbxmapi.Text;
					Object result2 = VSWebBL.ConfiguratorBL.O365ServerBL.Ins.InsertDatafortestsnew(O365testsObject);
				}
				if (chbxcalender.Checked)
				{
					O365testsObject.ServerId = Convert.ToInt32(Session["ServerID"]);
					O365testsObject.EnableSimulationTests = chbxcalender.Checked ? true : chbxcalender.Checked;
					O365testsObject.Type = "User Scenario Tests";
					O365testsObject.Tests = chbxcalender.Text;
					Object result2 = VSWebBL.ConfiguratorBL.O365ServerBL.Ins.InsertDatafortestsnew(O365testsObject);
				}

				if (chbxinbox.Checked)
				{
					O365testsObject.ServerId = Convert.ToInt32(Session["ServerID"]);
					O365testsObject.EnableSimulationTests = chbxinbox.Checked ? true : chbxinbox.Checked;
					O365testsObject.Type = "User Scenario Tests";
					O365testsObject.Tests = chbxinbox.Text;
					Object result2 = VSWebBL.ConfiguratorBL.O365ServerBL.Ins.InsertDatafortestsnew(O365testsObject);
				}
				if (chbxtask.Checked)
				{
					O365testsObject.ServerId = Convert.ToInt32(Session["ServerID"]);
					O365testsObject.EnableSimulationTests = chbxtask.Checked ? true : chbxtask.Checked;
					O365testsObject.Type = "User Scenario Tests";
					O365testsObject.Tests = chbxtask.Text;
					Object result2 = VSWebBL.ConfiguratorBL.O365ServerBL.Ins.InsertDatafortestsnew(O365testsObject);
				}
				//Performance Tests
				if (chbxmailflow.Checked)
				{
					O365testsObject.ServerId = Convert.ToInt32(Session["ServerID"]);
					O365testsObject.EnableSimulationTests = chbxmailflow.Checked ? true : chbxmailflow.Checked;
					O365testsObject.ResponseThreshold = Convert.ToInt32(string.IsNullOrEmpty(SrvAtrMailFlowThTextBox.Text) ? "0" : SrvAtrMailFlowThTextBox.Text);
					O365testsObject.Type = "User Scenario Tests";
					O365testsObject.Tests = chbxmailflow.Text;
					Object result3 = VSWebBL.ConfiguratorBL.O365ServerBL.Ins.InsertDatafortestsnew(O365testsObject);
				}
				if (chbxcreatefolder.Checked)
				{
					O365testsObject.ServerId = Convert.ToInt32(Session["ServerID"]);
					O365testsObject.EnableSimulationTests = chbxcreatefolder.Checked ? true : chbxcreatefolder.Checked;
					O365testsObject.ResponseThreshold = Convert.ToInt32(string.IsNullOrEmpty(SrvAtrCreateFolderThTextBox.Text) ? "0" : SrvAtrCreateFolderThTextBox.Text);
					O365testsObject.Type = "User Scenario Tests";
					O365testsObject.Tests = chbxcreatefolder.Text;
					Object result4 = VSWebBL.ConfiguratorBL.O365ServerBL.Ins.InsertDatafortestsnew(O365testsObject);
				}
				if (chbxcreatesite.Checked)
				{
					O365testsObject.ServerId = Convert.ToInt32(Session["ServerID"]);
					O365testsObject.EnableSimulationTests = chbxcreatesite.Checked ? true : chbxcreatesite.Checked;
					O365testsObject.ResponseThreshold = Convert.ToInt32(string.IsNullOrEmpty(srvAtrCreateSiteThTextBox.Text) ? "0" : srvAtrCreateSiteThTextBox.Text);
					O365testsObject.Type = "User Scenario Tests";
					O365testsObject.Tests = chbxcreatesite.Text;
					Object result4 = VSWebBL.ConfiguratorBL.O365ServerBL.Ins.InsertDatafortestsnew(O365testsObject);
				}
				if (chbxonedriveupload.Checked)
				{
					O365testsObject.ServerId = Convert.ToInt32(Session["ServerID"]);
					O365testsObject.EnableSimulationTests = chbxonedriveupload.Checked ? true : chbxonedriveupload.Checked;
					O365testsObject.ResponseThreshold = Convert.ToInt32(string.IsNullOrEmpty(SrvAtrODUploadThTextBox.Text) ? "0" : SrvAtrODUploadThTextBox.Text);
					O365testsObject.Type = "User Scenario Tests";
					O365testsObject.Tests = chbxonedriveupload.Text;
					Object result5 = VSWebBL.ConfiguratorBL.O365ServerBL.Ins.InsertDatafortestsnew(O365testsObject);
				}
				if (chbxonedrivedownload.Checked)
				{
					O365testsObject.ServerId = Convert.ToInt32(Session["ServerID"]);
					O365testsObject.EnableSimulationTests = chbxonedrivedownload.Checked ? true : chbxonedrivedownload.Checked;
					O365testsObject.ResponseThreshold = Convert.ToInt32(string.IsNullOrEmpty(SrvAtrODDownloadThTextBox.Text) ? "0" : SrvAtrODDownloadThTextBox.Text);
					O365testsObject.Type = "User Scenario Tests";
					O365testsObject.Tests = chbxonedrivedownload.Text;
					Object result6 = VSWebBL.ConfiguratorBL.O365ServerBL.Ins.InsertDatafortestsnew(O365testsObject);
				}
				//Directory Syncronization Test
				if (chbxdirsync.Checked)
				{
					O365testsObject.ServerId = Convert.ToInt32(Session["ServerID"]);
					O365testsObject.EnableSimulationTests = chbxdirsync.Checked ? true : chbxdirsync.Checked;
					O365testsObject.ResponseThreshold = Convert.ToInt32(string.IsNullOrEmpty(dirsynctxt.Text) ? "0" : dirsynctxt.Text);
					O365testsObject.Type = "User Scenario Tests";
					O365testsObject.Tests = chbxdirsync.Text;
					Object result7 = VSWebBL.ConfiguratorBL.O365ServerBL.Ins.InsertDatafortestsnew(O365testsObject);
				}
				//O365testsObject.ServerId = ;
				//O365testsObject.Tests = chbxsmtp.Text;
				//O365testsObject.Name = NameTextBox.Text;
				//O365testsObject.Category = CategoryTextBox.Text;
				//O365testsObject.Enabled = EnabledCheckBox.Checked;
				//O365testsObject.mode = SortRadioButtonList1.SelectedItem.Text;
				//O365testsObject.servername = txtservername.Text;
				//DataTable dt = VSWebBL.SecurityBL.webspehereImportBL.Ins.GetCredentialValue(creComboBox.Text);
				//if (dt.Rows.Count > 0)
				//    O365ServerObject.CredentialsId = Convert.ToInt32(dt.Rows[0]["ID"].ToString());
				//O365ServerObject.OffHoursScanInterval = int.Parse(OffScanTextBox.Text);
				//O365ServerObject.ScanInterval = int.Parse(ScanTextBox.Text);
				//O365ServerObject.RetryInterval = int.Parse(RetryTextBox.Text);
				//O365ServerObject.ResponseThreshold = int.Parse(RespThrTextBox.Text);
				//O365ServerObject.SearchStringNotFound = RequiredTextBox.Text;
				//O365ServerObject.SearchStringFound = txtSearch.Text;
				//O365ServerObject.URL = IPAddressTextBox.Text;
				//O365ServerObject.imageurl = Image1.ImageUrl;
				//O365ServerObject.UserName = UserNameTextBox.Text;
				//if (string.IsNullOrEmpty(PasswordTextBox.Text.Trim()))
				//    PasswordTextBox.Text = hdnPwd.Value;//soma
				//TripleDES tripleDES = new TripleDES();
				//byte[] encryptedPass = tripleDES.Encrypt(PasswordTextBox.Text);
				//string encryptedPassAsString = string.Join(", ", encryptedPass.Select(s => s.ToString()).ToArray());
				//O365ServerObject.PW = encryptedPassAsString;
				//ASPxCheckBox EnableSimulationTestsCheckBox = (ASPxCheckBox)Tests.FindEditFormTemplateControl("EnableSimulationTestsCheckBox");
				//Locations LOCobject = new Locations();
				//ServerTypes STypeobject = new ServerTypes();
				//STypeobject.ServerType = "Office365";
				//ServerTypes ReturnValue = VSWebBL.SecurityBL.ServerTypesBL.Ins.GetDataForServerType(STypeobject);
				//O365ServerObject.ServerTypeId = ReturnValue.ID;
				//Locations ReturnLocValue = VSWebBL.SecurityBL.LocationsBL.Ins.GetDataForLocation(LOCobject);
				//O365ServerObject.LocationId = ReturnLocValue.ID;
				//O365ServerObject.Location = ReturnLocValue.Location;
				//O365ServerObject.FailureThreshold = int.Parse(SrvAtrFailBefAlertTextBox.Text);
				return O365testsObject;
			}
			catch (Exception ex)
			{
				//6/27/2014 NS added for VSPLUS-634
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}
			finally { }
		}
		private void UpdateO365ServerData()
		{
			try
			{
				O365Server UrlObj = new O365Server();
				//11/19/2013 NS added ID below
				UrlObj.ID = ID;
				EnabledCheckBox.Enabled = true;
				//UrlObj.URL = IPAddressTextBox.Text;
				UrlObj.Name = NameTextBox.Text;
				UrlObj.Category = CategoryTextBox.Text;
                UrlObj.Costperuser = CostperuserTextBox.Text;
				DataTable returntable = VSWebBL.ConfiguratorBL.O365ServerBL.Ins.GetIPAddress(UrlObj, "Update");
				if (returntable.Rows.Count > 0)
				{
					errorDiv.InnerHtml = "This O365Server Name is already being monitored. Please enter another IP Name." +
		"<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
					errorDiv.Style.Value = "display: block";
					flag = true;
					//IPAddressTextBox.Focus();
				}
				else
				{
					try
					{
						Object ReturnValue = VSWebBL.ConfiguratorBL.O365ServerBL.Ins.UpdateData(CollectDataForO365Server());

						SetFocusOnError(ReturnValue);
						if (ReturnValue.ToString() == "True")
						{
							//modemes = Request.QueryString["Mode"].ToString();
							//Session["O365ServerUpdateStatus"] = NameTextBox.Text;
							//Response.Redirect("O365ServerGrid.aspx?modemes=" + modemes, false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
							//Context.ApplicationInstance.CompleteRequest();
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
			}

			catch (Exception ex)
			{
				//6/27/2014 NS added for VSPLUS-634
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}
		}
		private void InsertO365Server()
		{
			try
			{
				DataTable name = new DataTable();
				O365Server UrlObj = new O365Server();
				//UrlObj.URL = IPAddressTextBox.Text;
				UrlObj.Name = NameTextBox.Text;
				UrlObj.Category = CategoryTextBox.Text;
                UrlObj.Costperuser = CostperuserTextBox.Text;

				DataTable returntable = VSWebBL.ConfiguratorBL.O365ServerBL.Ins.GetIPAddress(UrlObj, "Insert");

				if (returntable.Rows.Count > 0)
				{
					errorDiv.InnerHtml = "This O365Server Name is already being monitored. Please enter another Name." +
	 "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
					errorDiv.Style.Value = "display: block";
					flag = true;
					//IPAddressTextBox.Focus();
				}

				else
				{

					try
					{
						object ReturnValue = VSWebBL.ConfiguratorBL.O365ServerBL.Ins.InsertData(CollectDataForO365Server());
						SetFocusOnError(ReturnValue);
						name = VSWebBL.ConfiguratorBL.O365ServerBL.Ins.GetIdFromName(NameTextBox.Text);
						Session["ServerID"] = name.Rows[0]["ID"].ToString();
						//List<object> fieldValues = Tests.GetSelectedFieldValues(new string[] { "Id", "EnableSimulationTests", "ResponseThreshold", "Tests", "Type" });
						List<object> fieldValuesnodes = Nodes.GetSelectedFieldValues(new string[] { "Id", "SelectedNodes" });
						//if ((ReturnValue.ToString() == "True") && (fieldValues.Count > 0) && (fieldValuesnodes.Count > 0))
						//{
						//    if (name.Rows.Count > 0)
						//    {
						//        Session["ID"] = name.Rows[0][0].ToString();
						//        Session["O365ServerUpdateStatus"] = NameTextBox.Text;
						//        modemes = Request.QueryString["Mode"].ToString();
						//        Response.Redirect("O365ServerGrid.aspx?ID=" + Session["ID"].ToString() + "&Mode=" + modemes, false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
						//        Context.ApplicationInstance.CompleteRequest();
						//    }

						//}
						//                else
						//                {
						//                    errorDiv.Visible = true;

						//                    errorDiv.InnerHtml = "Please fill the Required fields, in the Tests/Options & Nodes tab. " +
						//"<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
						//                    errorDiv.Style.Value = "display: block";
						//                }
					}
					catch (Exception ex)
					{
						Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
						throw ex;
					}
					finally { }
				}
			}
			catch (Exception ex)
			{
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}
		}
		private void SetFocusOnError(Object ReturnValue)
		{
			string ErrorMessage = ReturnValue.ToString();
			if (ErrorMessage.Substring(0, 2) == "ER")
			{
				errorDiv.InnerHtml = ErrorMessage.Substring(3) +
			"<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
				errorDiv.Style.Value = "display: block";
			}
		}
		protected void FormOkButton_Click(object sender, EventArgs e)
		{

			try
			{
				errorDiv.Visible = false;
				errorDiv.Style.Value = "display: none";
				modemes = Request.QueryString["Mode"].ToString();
				if ((NameTextBox.Text == "") && (UserNameTextBox.Text == ""))
				{
					errorDiv.Visible = true;

					errorDiv.InnerHtml = "Please fill the Required fields, in the Account Properties tab. " +
			"<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
					errorDiv.Style.Value = "display: block";
					//Response.Redirect("O365ServerProperties.aspx?Mode=" + modemes, false);

				}
				else if ((SortRadioButtonList1.SelectedIndex == 2) && (txtservername.Text == ""))
				{
					errorDiv.Visible = true;

					errorDiv.InnerHtml = "Please fill the Required fields, in the Account Properties tab. " +
					"<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
					errorDiv.Style.Value = "display: block";
					//Response.Redirect("O365ServerProperties.aspx?Mose=" + modemes, false);


				}
				else
				{
					CheckNodes();
					if (Session["NodeError"] == null || Session["NodeError"] == "")
					{

						if (Session["ID"] != null)
						{
							if (Convert.ToInt32(Session["ID"].ToString()) != 0)
							{
								if ((NameTextBox.Text != "") && (UserNameTextBox.Text != ""))
								{
									Mode = "Update";
									UpdateO365ServerData();
									UpdateTestsData();
									UpdateNodesData();
									//List<object> fieldValues = Tests.GetSelectedFieldValues(new string[] { "Id", "EnableSimulationTests", "ResponseThreshold", "Tests", "Type" });
									List<object> fieldValuesnodes = Nodes.GetSelectedFieldValues(new string[] { "Id", "SelectedNodes" });
									//                if (fieldValues.Count == 0)
									//                {
									//                    errorDiv.Visible = true;

									//                    errorDiv.InnerHtml = "Please fill the Required fields, in the Tests/Options tab. " +
									//"<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
									//                    errorDiv.Style.Value = "display: block";
									//                }
									if (fieldValuesnodes.Count == 0)
									{
										errorDiv.Visible = true;

										errorDiv.InnerHtml = "Please select a node on the Nodes tab." +
					  "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
										errorDiv.Style.Value = "display: block";
									}


									else if ((fieldValuesnodes.Count > 0))
									{
										modemes = Request.QueryString["Mode"].ToString();
										Session["O365ServerUpdateStatus"] = NameTextBox.Text;
										Response.Redirect("O365ServerGrid.aspx?modemes=" + modemes, false);
										Context.ApplicationInstance.CompleteRequest();
									}
								}
								else
								{
									errorDiv.Visible = true;

									errorDiv.InnerHtml = "Please fill the Required fields, in the Account Properties tab. " +
				"<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
									errorDiv.Style.Value = "display: block";
								}
							}
						}
						else
						{
							Mode = "Insert";

							InsertO365Server();
							if ((NameTextBox.Text != "") && (UserNameTextBox.Text != ""))
							{
								InsertTestsData();
							}
							else
							{
								errorDiv.Visible = true;

								errorDiv.InnerHtml = "Please fill the Required fields, in the Account Properties tab. " +
			"<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
								errorDiv.Style.Value = "display: block";
							}
							//UpdateTestsData();
							UpdateNodesData();
							//                List<object> fieldValues = Tests.GetSelectedFieldValues(new string[] { "Id", "EnableSimulationTests", "ResponseThreshold", "Tests", "Type" });
							List<object> fieldValuesnodes = Nodes.GetSelectedFieldValues(new string[] { "Id", "SelectedNodes" });

							if (fieldValuesnodes.Count == 0)
							{
								errorDiv.Visible = true;

								errorDiv.InnerHtml = "Please select a node on the Nodes tab. " +
			  "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
								errorDiv.Style.Value = "display: block";
							}

							else if ((fieldValuesnodes.Count > 0))
							{
								modemes = Request.QueryString["Mode"].ToString();
								Session["O365ServerUpdateStatus"] = NameTextBox.Text;
								Response.Redirect("O365ServerGrid.aspx?modemes=" + modemes, false);
								Context.ApplicationInstance.CompleteRequest();
							}
						}


					}

					else
					{
						errorDiv.Visible = true;
						errorDiv.InnerHtml = "There can be one node from one location. Select other nodes from different locations." +
							"<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
						errorDiv.Style.Value = "display: block";
						//Session["NodeError"] = "";
						Session["NodeError"] = "";
					}
				}
			}
			catch (Exception ex)
			{

				errorDiv.Visible = true;
				//2/11/2016 NS modified for VSPLUS-2568
				errorDiv.InnerHtml = "While saving the Office 365 account, an exceptional & an internal error has occured. " +
			"<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
				errorDiv.Style.Value = "display: block";
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}
			finally { }
		}
		protected void FormCancelButton_Click(object sender, EventArgs e)
		{
			Response.Redirect("O365ServerGrid.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
			Context.ApplicationInstance.CompleteRequest();
		}
		protected void ValidationUpdatedButton_Click(object sender, EventArgs e)
		{
			Response.Redirect("O365ServerProperties.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
			Context.ApplicationInstance.CompleteRequest();
		}
		private Status CollectDataforStatus()
		{
			Status St = new Status();
			try
			{

				//St.DeadMail = 0;
				//St.Description = IPAddressTextBox.Text;
				//St.Details = "";
				St.DownCount = 0;
				// St.Location = "Cluster";
				St.Name = NameTextBox.Text;
				//St.MailDetails = "Mail Details";
				// St.PendingMail = 0;
				St.sStatus = "Not Scanned";
				St.Type = "Office365";
				St.Upcount = 0;
				St.UpPercent = 1;
				St.LastUpdate = System.DateTime.Now;
				St.ResponseTime = 0;
				St.TypeANDName = NameTextBox.Text + "-Office365";
				St.Icon = 0;
				// St.NextScan = System.DateTime.Now;

				return St;
			}
			catch (Exception ex)
			{
				//6/27/2014 NS added for VSPLUS-634
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}
		}
		private void InsertStatus()
		{

			try
			{
				object ReturnValue = VSWebBL.StatusBL.StatusTBL.Ins.InsertData(CollectDataforStatus());
				// SetFocusOnError(ReturnValue);
				if (ReturnValue.ToString() == "True")
				{
					modemes = Request.QueryString["Mode"].ToString();
					Session["O365ServerUpdateStatus"] = NameTextBox.Text;
					Response.Redirect("O365ServerGrid.aspx?modemes=" + modemes, false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
					Context.ApplicationInstance.CompleteRequest();
				}

			}
			catch (Exception ex)
			{
				errorDiv.InnerHtml = "Error attempting to update the status table: " + ex.Message +
			"<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
				errorDiv.Style.Value = "display: block";
				//6/27/2014 NS added for VSPLUS-634
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
			}
			finally { }
		}
		protected void NameTextBox_TextChanged(object sender, EventArgs e)
		{
			char Quote;
			Quote = (char)34;

			string myName = NameTextBox.Text;
			try
			{
				if (myName.IndexOf("'") > 0)
					myName = myName.Replace("'", "");

				if (myName.IndexOf(Quote) > 0)
					myName = myName.Replace(Quote, '~');

				if (myName.IndexOf(",") > 0)
					myName = myName.Replace(",", " ");

				if (myName != NameTextBox.Text)
					NameTextBox.Text = myName;

			}
			catch (Exception ex)
			{
				//   MessageBox.Show(ex.Message)
				//6/27/2014 NS added for VSPLUS-634
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
			}

		}
		protected void OKCopy_Click(object sender, EventArgs e)
		{
			bool check = false;
			Credentials Csibject = new Credentials();

			Csibject.AliasName = AliasName.Text;
			if (Csibject.AliasName == "")
			{
				Div3.InnerHtml = "Please Enter Alias Name." +
						"<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
				Div3.Style.Value = "display: block";
			}

			//DataTable returntable = VSWebBL.ConfiguratorBL.Businesshoursbl.Ins.GetName(Csibject);
			DataTable returntable = VSWebBL.SecurityBL.webspehereImportBL.Ins.GetAliasName(Csibject);

			if (returntable.Rows.Count > 0)
			{

				Div3.InnerHtml = "This Alias already exists. Enter another one." +
					"<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
				Div3.Style.Value = "display: block";
				if (returntable.Rows[0]["AliasName"].ToString() == "")
				{
					Div3.InnerHtml = "Please Enter Alias Name." +
						"<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
					Div3.Style.Value = "display: block";
				}

			}
			else
			{

				check = VSWebBL.SecurityBL.webspehereImportBL.Ins.Insertpwd(AliasName.Text, UserID.Text, Password.Text, 21);
				if ((Request.QueryString["ID"] == null) || (Request.QueryString["ID"] == "0"))
				{
					modemes = "Insert";
					Response.Redirect("~/Configurator/O365ServerProperties.aspx?add=OK" + "&Mode=" + modemes, false);
				}
				else
				{
					EnabledCheckBox.Enabled = true;
					modemes = "Update";
					Response.Redirect("~/Configurator/O365ServerProperties.aspx?add=OK" + "&Mode=" + modemes + "&ID=" + ID, false);
				}

			}

		}
		protected void Cancel_Click(object sender, EventArgs e)
		{
			Response.Redirect("~/Configurator/O365ServerProperties.aspx?add=OK");

		}
		protected void btn_clickcopyprofile(object sender, EventArgs e)
		{

			CopyProfilePopupControl.ShowOnPageLoad = true;
			UserID.Visible = true;
			OKCopy.Visible = true;
			Cancel.Visible = true;
			Password.Visible = true;
			Session["SeverName"] = txtservername.Text;
			Session["Name"] = NameTextBox.Text;
			Session["checked"] = EnabledCheckBox.Checked;
			Session["Credentials"] = creComboBox.Text;
			//Session["ipaddress"] = IPAddressTextBox.Text;
			Session["category"] = CategoryTextBox.Text;
			Session["ScanInterval"] = ScanTextBox.Text;
			Session["ResponseThreshold"] = RespThrTextBox.Text;
			Session["RetryInterval"] = RetryTextBox.Text;
			Session["SrvAtrFailBefAlertTextBox"] = SrvAtrFailBefAlertTextBox.Text;
			Session["OffScan"] = OffScanTextBox.Text;
			Session["Uname"] = UserNameTextBox.Text;
			Session["Pwd"] = PasswordTextBox.Text;
            Session["Costperuser"] = CostperuserTextBox.Text;
		}
		private void FillMaintenanceGrid()
		{
			try
			{
				DataTable MaintDataTable = new DataTable();
				DataSet ServersDataSet = new DataSet();
				MaintDataTable = VSWebBL.ConfiguratorBL.MaintenanceBL.Ins.GetMaintenDataOnServerID(NameTextBox.Text);
				if (MaintDataTable.Rows.Count > 0)
				{
					DataTable dtcopy = MaintDataTable.Copy();
					dtcopy.PrimaryKey = new DataColumn[] { dtcopy.Columns["ID"] };
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
		private void FillMaintServersGridfromSession()
		{
			try
			{
				DataTable ServersDataTable = new DataTable();
				//4/3/2014 NS modified for VSPLUS-138
				//if(Session["MaintServers"]!=null&&Session["MaintServers"]!="")
				//ServersDataTable = (DataTable)Session["MaintServers"];//VSWebBL.ConfiguratorBL.DominoPropertiesBL.Ins.GetAllData();
				if (ViewState["MaintServers"] != null && ViewState["MaintServers"] != "")
					ServersDataTable = (DataTable)ViewState["MaintServers"];//VSWebBL.ConfiguratorBL.DominoPropertiesBL.Ins.GetAllData();
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
		private void FillAlertGridView()
		{
			try
			{

				DataTable AlertDataTable = new DataTable();
				DataSet AlertDataSet = new DataSet();
				AlertDataTable = VSWebBL.ConfiguratorBL.AlertsBL.Ins.GetAlertTab(NameTextBox.Text, "O365Server");
				if (AlertDataTable.Rows.Count > 0)
				{
					DataTable dtcopy = AlertDataTable.Copy();
					// dtcopy.PrimaryKey = new DataColumn[] { dtcopy.Columns["ID"] };
					//4/3/2014 NS modified for VSPLUS-138
					//Session["AlertDataTable"] = dtcopy;
					ViewState["AlertDataTable"] = dtcopy;
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
		protected void SortRadioButtonList1_SelectedIndexChanged(object sender, EventArgs e)
		{
			int dir = Convert.ToInt32(SortRadioButtonList1.SelectedItem.Value);

			if (dir == 3)
			{
				DirSyncPanel.Visible = true;
				//lblservername.Visible = true;
				//txtservername.Visible = true;
				//lblcrecombo.Visible = true;

				//creComboBox.Visible = true;
				//ASPxButton1.Visible = true;
				//ASPxPageControl1.TabPages[1].Enabled=true;
				//ASPxPageControl1.TabPages[].Visible = true;

			}
			if (dir == 2)
			{
				DirSyncPanel.Visible = false;
				//lblservername.Visible = false;
				//txtservername.Visible = false;
				//lblcrecombo.Visible = false;

				//creComboBox.Visible = false;
				//ASPxButton1.Visible = false;
			}
			if (dir == 1)
			{
				DirSyncPanel.Visible = false;
				//lblservername.Visible = false;
				//txtservername.Visible = false;
				//lblcrecombo.Visible = false;

				//creComboBox.Visible = false;
				//ASPxButton1.Visible = false;
			}

		}
		protected void MaintWinListGridView_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
		{
			e.Row.Attributes.Add("onmouseover", "this.style.backgroundColor='#C0C0C0';");

			e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='white';");
		}
		protected void MaintWinListGridView_PageSizeChanged(object sender, EventArgs e)
		{
			//ProfilesGridView.PageIndex;
			VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("O365ServerProperties|MaintWinListGridView", MaintWinListGridView.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
			Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
		}
		protected void AlertGridView_PageSizeChanged(object sender, EventArgs e)
		{
			//ProfilesGridView.PageIndex;
			VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("O365ServerProperties|AlertGridView", AlertGridView.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
			Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
		}
		private void FillNodesGridView()
		{
			object sumObject;
			int idvaue;
			try
			{
				Nodes.Selection.UnselectAll();
				Session["Nodes"] = null;
				DataTable dt1 = new DataTable();
				DataTable dt = new DataTable();
				if (Session["ID"] != null)
				{

					if (Session["ID"].ToString() != "0")
					{

						int id = Convert.ToInt32(Session["ID"].ToString());
						dt = VSWebBL.ConfiguratorBL.O365ServerBL.Ins.GetNodesTabadd(id.ToString());
						//dt = VSWebBL.ConfiguratorBL.O365ServerBL.Ins.GetNodes();
					}
					else
					{
						dt = VSWebBL.ConfiguratorBL.O365ServerBL.Ins.GetNodesTabadd("0");
					}
				}
				else
				{
					dt = VSWebBL.ConfiguratorBL.O365ServerBL.Ins.GetNodesTabadd("0");
				}

				DataColumn[] columns = new DataColumn[1];
				columns[0] = dt.Columns["Id"];

				if (dt.Rows.Count > 0)
				{
					sumObject = dt.Compute("Max(Id)", "");
					idvaue = Convert.ToInt32(sumObject);
					for (int i = 0; i < dt.Rows.Count; i++)
					{
						if (dt.Rows[i]["SelectedNodes"].ToString() != "")
						{
							bool chkvalue = Convert.ToBoolean(dt.Rows[i]["SelectedNodes"]);
							if (chkvalue)
							{
								Nodes.Selection.IsRowSelected(i);

								enablemsg.Visible = false;
							}
							else
							{
								enablemsg.Visible = true;
							}
						}
					}

				}

				Session["Nodes"] = dt;
				Nodes.DataSource = dt;
				Nodes.DataBind();

			}
			catch (Exception ex)
			{
				//6/27/2014 NS added for VSPLUS-634
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}
			finally { }
		}
		private void FillNodesGridViewfromSession()
		{
			try
			{
				DataTable NodesDataTable = new DataTable();
				DataTable dt = new DataTable();

				if (Session["Nodes"] != null && Session["Nodes"] != "")
					dt = (DataTable)Session["Nodes"];

				if (dt.Rows.Count > 0)
				{
					//GridViewDataColumn column2 = Tests.Columns["ResponseThreshold"] as GridViewDataColumn;

					int startIndex = Nodes.PageIndex * Nodes.SettingsPager.PageSize;
					int endIndex = Math.Min(Nodes.VisibleRowCount, startIndex + Nodes.SettingsPager.PageSize);
					Session["endindex"] = endIndex;
					for (int i = startIndex; i < endIndex; i++)
					{
						if (Nodes.Selection.IsRowSelected(i))
						{

							checkedvalue = Convert.ToBoolean(dt.Rows[i]["SelectedNodes"] = "true");

						}
						else
						{

							checkedvalue = Convert.ToBoolean(dt.Rows[i]["SelectedNodes"] = "false");
						}
						for (int j = 0; j < dt.Rows.Count; j++)
						{
							if (dt.Rows[j]["SelectedNodes"].ToString() != "")
							{

								bool chkvalue = Convert.ToBoolean(dt.Rows[j]["SelectedNodes"]);
								if (chkvalue)
								{
									Nodes.Selection.IsRowSelected(j);

								}
							}
						}

					}
					Nodes.DataSource = dt;
					Nodes.DataBind();
					Nodes.KeyFieldName = "Id";

				}
			}
			catch (Exception ex)
			{

				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}
			finally { }
		}
		protected void checkToMonitor_Init(object sender, EventArgs e)
		{
			ASPxCheckBox chk = sender as ASPxCheckBox;
			GridViewDataItemTemplateContainer container = chk.NamingContainer as GridViewDataItemTemplateContainer;

			chk.ClientSideEvents.CheckedChanged = String.Format("function (s,e) {{ cb.PerformCallback('{0}|' + s.GetChecked()); }}", container.KeyValue);
		}
		protected void checkSubScribedAlerts_Init(object sender, EventArgs e)
		{
			ASPxCheckBox chk = sender as ASPxCheckBox;
			GridViewDataItemTemplateContainer container = chk.NamingContainer as GridViewDataItemTemplateContainer;

			chk.ClientSideEvents.CheckedChanged = String.Format("function (s,e) {{ cb.PerformCallback('{0}|' + s.GetChecked()); }}", container.KeyValue);
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
		protected void cb1_callback(object sender, DevExpress.Web.CallbackEventArgs e)
		{
			String[] parameters = e.Parameter.Split('|');
			string Id = parameters[0];
			bool isChecked = Convert.ToBoolean(parameters[1]);

			DataTable dt = new DataTable();

			if (Session["Nodes"] != null && Session["Nodes"] != "")
				dt = (DataTable)Session["Nodes"];

			if (dt.Rows.Count > 0)
			{
				DataRow row = dt.Rows.Find(Id);
				(dt.Rows.Find(Id))["SelectedNodes"] = isChecked;
				DataRow row2 = dt.Rows.Find(Id);
			}
		}
		private DataTable GetSelectedTests()
		{
			DataTable dtSel = new DataTable();
			try
			{
				dtSel.Columns.Add("Id");
				dtSel.Columns.Add("ResponseThreshold");
				//dtSel.Columns.Add("SubscribedAlerts");

				DataTable dt = (DataTable)Session["Tests"];
				if (dt != null)
				{
					foreach (DataRow row in dt.Rows)
					{
						if (row["EnableSimulationTests"].ToString().ToLower() == "true")
						{
							DataRow dr = dtSel.NewRow();
							dr["id"] = row["ID"];
							dtSel.Rows.Add(dr);
						}

					}
				}
			}
			catch (Exception ex)
			{
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}

			return dtSel;

		}
		private DataTable GetSelectedNodes()
		{


			DataTable dtSel1 = new DataTable();
			try
			{
				dtSel1.Columns.Add("Id");
				dtSel1.Columns.Add("Location");
				//dtSel.Columns.Add("SubscribedAlerts");

				DataTable dt = (DataTable)Session["Nodes"];
				if (dt != null)
				{
					foreach (DataRow row in dt.Rows)
					{
						if (row["SelectedNodes"].ToString().ToLower() == "true")
						{
							DataRow dr = dtSel1.NewRow();
							dr["id"] = row["Id"];
							dr["Location"] = row["Location"];
							dtSel1.Rows.Add(dr);
							//dtSel1.Rows.Add(dr);
						}

					}
				}
			}
			catch (Exception ex)
			{
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}

			return dtSel1;

		}
		private void UpdateTestsData()
		{
			try
			{
				Office365Tests O365testsObject = new Office365Tests();
				if (chbxsmtp.Checked ? O365testsObject.EnableSimulationTests = true : O365testsObject.EnableSimulationTests = false)
				{
					O365testsObject.ServerId = Convert.ToInt32(Request.QueryString["ID"].ToString());
					O365testsObject.EnableSimulationTests = chbxsmtp.Checked ? true : chbxsmtp.Checked;
					O365testsObject.Type = "Service Availability Tests";
					O365testsObject.Tests = chbxsmtp.Text;
					//Object result2 = VSWebBL.ConfiguratorBL.O365ServerBL.Ins.UpdateDatafortestsnew(O365testsObject);
				}
				else
				{
					O365testsObject.ServerId = Convert.ToInt32(Request.QueryString["ID"].ToString());
					O365testsObject.EnableSimulationTests = chbxsmtp.Checked ? true : chbxsmtp.Checked;
					O365testsObject.Type = "Service Availability Tests";
					O365testsObject.Tests = chbxsmtp.Text;
				}
				Object result21 = VSWebBL.ConfiguratorBL.O365ServerBL.Ins.UpdateDatafortestsnew(O365testsObject);
				if (chbxpop.Checked)
				{
					O365testsObject.ServerId = Convert.ToInt32(Request.QueryString["ID"].ToString());
					O365testsObject.EnableSimulationTests = chbxpop.Checked ? true : chbxpop.Checked;
					O365testsObject.Type = "Service Availability Tests";
					O365testsObject.Tests = chbxpop.Text;
					//Object result2 = VSWebBL.ConfiguratorBL.O365ServerBL.Ins.UpdateDatafortestsnew(O365testsObject);
				}
				else
				{
					O365testsObject.ServerId = Convert.ToInt32(Request.QueryString["ID"].ToString());
					O365testsObject.EnableSimulationTests = chbxpop.Checked ? true : chbxpop.Checked;
					O365testsObject.Type = "Service Availability Tests";
					O365testsObject.Tests = chbxpop.Text;

				}
				Object result22 = VSWebBL.ConfiguratorBL.O365ServerBL.Ins.UpdateDatafortestsnew(O365testsObject);
				if (chbximap.Checked)
				{
					O365testsObject.ServerId = Convert.ToInt32(Request.QueryString["ID"].ToString());
					O365testsObject.EnableSimulationTests = chbximap.Checked ? true : chbximap.Checked;
					O365testsObject.Type = "Service Availability Tests";
					O365testsObject.Tests = chbximap.Text;
					//Object result2 = VSWebBL.ConfiguratorBL.O365ServerBL.Ins.UpdateDatafortestsnew(O365testsObject);
				}
				else
				{
					O365testsObject.ServerId = Convert.ToInt32(Request.QueryString["ID"].ToString());
					O365testsObject.EnableSimulationTests = chbximap.Checked ? true : chbximap.Checked;
					O365testsObject.Type = "Service Availability Tests";
					O365testsObject.Tests = chbximap.Text;

				}
				Object result23 = VSWebBL.ConfiguratorBL.O365ServerBL.Ins.UpdateDatafortestsnew(O365testsObject);
				if (chbxowa.Checked)
				{
					O365testsObject.ServerId = Convert.ToInt32(Request.QueryString["ID"].ToString());
					O365testsObject.EnableSimulationTests = chbxowa.Checked ? true : chbxowa.Checked;
					O365testsObject.Type = "User Scenario Tests";
					O365testsObject.Tests = chbxowa.Text;
					//Object result2 = VSWebBL.ConfiguratorBL.O365ServerBL.Ins.UpdateDatafortestsnew(O365testsObject);
				}
				else
				{
					O365testsObject.ServerId = Convert.ToInt32(Request.QueryString["ID"].ToString());
					O365testsObject.EnableSimulationTests = chbxowa.Checked ? true : chbxowa.Checked;
					O365testsObject.Type = "User Scenario Tests";
					O365testsObject.Tests = chbxowa.Text;

				}
				Object result32 = VSWebBL.ConfiguratorBL.O365ServerBL.Ins.UpdateDatafortestsnew(O365testsObject);
				if (chbxautodiscovery.Checked)
				{
					O365testsObject.ServerId = Convert.ToInt32(Request.QueryString["ID"].ToString()); ;
					O365testsObject.EnableSimulationTests = chbxautodiscovery.Checked ? true : chbxautodiscovery.Checked;
					O365testsObject.Type = "Service Availability Tests";
					O365testsObject.Tests = chbxautodiscovery.Text;
					//Object result2 = VSWebBL.ConfiguratorBL.O365ServerBL.Ins.UpdateDatafortestsnew(O365testsObject);
				}
				else
				{
					O365testsObject.ServerId = Convert.ToInt32(Request.QueryString["ID"].ToString()); ;
					O365testsObject.EnableSimulationTests = chbxautodiscovery.Checked ? true : chbxautodiscovery.Checked;
					O365testsObject.Type = "Service Availability Tests";
					O365testsObject.Tests = chbxautodiscovery.Text;

				}
				Object result24 = VSWebBL.ConfiguratorBL.O365ServerBL.Ins.UpdateDatafortestsnew(O365testsObject);
				if (chbxmapi.Checked)
				{
					O365testsObject.ServerId = Convert.ToInt32(Request.QueryString["ID"].ToString()); ;
					O365testsObject.EnableSimulationTests = chbxmapi.Checked ? true : chbxmapi.Checked;
					O365testsObject.Type = "Service Availability Tests";
					O365testsObject.Tests = chbxmapi.Text;
					//Object result2 = VSWebBL.ConfiguratorBL.O365ServerBL.Ins.UpdateDatafortestsnew(O365testsObject);
				}
				else
				{
					O365testsObject.ServerId = Convert.ToInt32(Request.QueryString["ID"].ToString()); ;
					O365testsObject.EnableSimulationTests = chbxmapi.Checked ? true : chbxmapi.Checked;
					O365testsObject.Type = "Service Availability Tests";
					O365testsObject.Tests = chbxmapi.Text;

				}
				Object result25 = VSWebBL.ConfiguratorBL.O365ServerBL.Ins.UpdateDatafortestsnew(O365testsObject);
				if (chbxcalender.Checked)
				{
					O365testsObject.ServerId = Convert.ToInt32(Request.QueryString["ID"].ToString()); ;
					O365testsObject.EnableSimulationTests = chbxcalender.Checked ? true : chbxcalender.Checked;
					O365testsObject.Type = "User Scenario Tests";
					O365testsObject.Tests = chbxcalender.Text;
					//Object result2 = VSWebBL.ConfiguratorBL.O365ServerBL.Ins.UpdateDatafortestsnew(O365testsObject);
				}
				else
				{
					O365testsObject.ServerId = Convert.ToInt32(Request.QueryString["ID"].ToString()); ;
					O365testsObject.EnableSimulationTests = chbxcalender.Checked ? true : chbxcalender.Checked;
					O365testsObject.Type = "User Scenario Tests";
					O365testsObject.Tests = chbxcalender.Text;

				}
				Object result26 = VSWebBL.ConfiguratorBL.O365ServerBL.Ins.UpdateDatafortestsnew(O365testsObject);
				if (chbxinbox.Checked)
				{
					O365testsObject.ServerId = Convert.ToInt32(Request.QueryString["ID"].ToString()); ;
					O365testsObject.EnableSimulationTests = chbxinbox.Checked ? true : chbxinbox.Checked;
					O365testsObject.Type = "User Scenario Tests";
					O365testsObject.Tests = chbxinbox.Text;
					//Object result2 = VSWebBL.ConfiguratorBL.O365ServerBL.Ins.UpdateDatafortestsnew(O365testsObject);
				}
				else
				{
					O365testsObject.ServerId = Convert.ToInt32(Request.QueryString["ID"].ToString()); ;
					O365testsObject.EnableSimulationTests = chbxinbox.Checked ? true : chbxinbox.Checked;
					O365testsObject.Type = "User Scenario Tests";
					O365testsObject.Tests = chbxinbox.Text;

				}
				Object result27 = VSWebBL.ConfiguratorBL.O365ServerBL.Ins.UpdateDatafortestsnew(O365testsObject);
				if (chbxtask.Checked)
				{
					O365testsObject.ServerId = Convert.ToInt32(Session["ID"]);
					O365testsObject.EnableSimulationTests = chbxtask.Checked ? true : chbxtask.Checked;
					O365testsObject.Type = "User Scenario Tests";
					O365testsObject.Tests = chbxtask.Text;
					//Object result2 = VSWebBL.ConfiguratorBL.O365ServerBL.Ins.UpdateDatafortestsnew(O365testsObject);
				}
				else
				{
					O365testsObject.ServerId = Convert.ToInt32(Session["ID"]);
					O365testsObject.EnableSimulationTests = chbxtask.Checked ? true : chbxtask.Checked;
					O365testsObject.Type = "User Scenario Tests";
					O365testsObject.Tests = chbxtask.Text;

				}
				Object result28 = VSWebBL.ConfiguratorBL.O365ServerBL.Ins.UpdateDatafortestsnew(O365testsObject);
				//Performance Tests
				if (chbxmailflow.Checked)
				{
					O365testsObject.ServerId = Convert.ToInt32(Request.QueryString["ID"].ToString()); ;
					O365testsObject.EnableSimulationTests = chbxmailflow.Checked ? true : chbxmailflow.Checked;
					O365testsObject.ResponseThreshold = Convert.ToInt32(string.IsNullOrEmpty(SrvAtrMailFlowThTextBox.Text) ? "0" : SrvAtrMailFlowThTextBox.Text);
					O365testsObject.Type = "User Scenario Tests";
					O365testsObject.Tests = chbxmailflow.Text;
					//Object result3 = VSWebBL.ConfiguratorBL.O365ServerBL.Ins.UpdateDatafortestsnew(O365testsObject);
				}
				else
				{
					O365testsObject.ServerId = Convert.ToInt32(Request.QueryString["ID"].ToString()); ;
					O365testsObject.EnableSimulationTests = chbxmailflow.Checked ? true : chbxmailflow.Checked;
					O365testsObject.ResponseThreshold = Convert.ToInt32(string.IsNullOrEmpty(SrvAtrMailFlowThTextBox.Text) ? "0" : SrvAtrMailFlowThTextBox.Text);
					O365testsObject.Type = "User Scenario Tests";
					O365testsObject.Tests = chbxmailflow.Text;

				}
				Object result31 = VSWebBL.ConfiguratorBL.O365ServerBL.Ins.UpdateDatafortestsnew(O365testsObject);
				if (chbxcreatefolder.Checked)
				{
					O365testsObject.ServerId = Convert.ToInt32(Request.QueryString["ID"].ToString()); ;
					O365testsObject.EnableSimulationTests = chbxcreatefolder.Checked ? true : chbxcreatefolder.Checked;
					O365testsObject.ResponseThreshold = Convert.ToInt32(string.IsNullOrEmpty(SrvAtrCreateFolderThTextBox.Text) ? "0" : SrvAtrCreateFolderThTextBox.Text);
					O365testsObject.Type = "User Scenario Tests";
					O365testsObject.Tests = chbxcreatefolder.Text;
					//Object result4 = VSWebBL.ConfiguratorBL.O365ServerBL.Ins.UpdateDatafortestsnew(O365testsObject);
				}
				else
				{
					O365testsObject.ServerId = Convert.ToInt32(Request.QueryString["ID"].ToString()); ;
					O365testsObject.EnableSimulationTests = chbxcreatefolder.Checked ? true : chbxcreatefolder.Checked;
					O365testsObject.ResponseThreshold = Convert.ToInt32(string.IsNullOrEmpty(SrvAtrCreateFolderThTextBox.Text) ? "0" : SrvAtrCreateFolderThTextBox.Text);
					O365testsObject.Type = "User Scenario Tests";
					O365testsObject.Tests = chbxcreatefolder.Text;

				}
				Object result4 = VSWebBL.ConfiguratorBL.O365ServerBL.Ins.UpdateDatafortestsnew(O365testsObject);
				if (chbxcreatesite.Checked)
				{
					O365testsObject.ServerId = Convert.ToInt32(Request.QueryString["ID"].ToString()); ;
					O365testsObject.EnableSimulationTests = chbxcreatesite.Checked ? true : chbxcreatesite.Checked;
					O365testsObject.ResponseThreshold = Convert.ToInt32(string.IsNullOrEmpty(srvAtrCreateSiteThTextBox.Text) ? "0" : srvAtrCreateSiteThTextBox.Text);
					O365testsObject.Type = "User Scenario Tests";
					O365testsObject.Tests = chbxcreatesite.Text;
					//Object result4 = VSWebBL.ConfiguratorBL.O365ServerBL.Ins.UpdateDatafortestsnew(O365testsObject);
				}
				else
				{
					O365testsObject.ServerId = Convert.ToInt32(Request.QueryString["ID"].ToString()); ;
					O365testsObject.EnableSimulationTests = chbxcreatesite.Checked ? true : chbxcreatesite.Checked;
					O365testsObject.ResponseThreshold = Convert.ToInt32(string.IsNullOrEmpty(srvAtrCreateSiteThTextBox.Text) ? "0" : srvAtrCreateSiteThTextBox.Text);
					O365testsObject.Type = "User Scenario Tests";
					O365testsObject.Tests = chbxcreatesite.Text;

				}
				Object result41 = VSWebBL.ConfiguratorBL.O365ServerBL.Ins.UpdateDatafortestsnew(O365testsObject);
				if (chbxonedriveupload.Checked)
				{
					O365testsObject.ServerId = Convert.ToInt32(Request.QueryString["ID"].ToString()); ;
					O365testsObject.EnableSimulationTests = chbxonedriveupload.Checked ? true : chbxonedriveupload.Checked;
					O365testsObject.ResponseThreshold = Convert.ToInt32(string.IsNullOrEmpty(SrvAtrODUploadThTextBox.Text) ? "0" : SrvAtrODUploadThTextBox.Text);
					O365testsObject.Type = "User Scenario Tests";
					O365testsObject.Tests = chbxonedriveupload.Text;
					//Object result5 = VSWebBL.ConfiguratorBL.O365ServerBL.Ins.UpdateDatafortestsnew(O365testsObject);
				}
				else
				{
					O365testsObject.ServerId = Convert.ToInt32(Request.QueryString["ID"].ToString()); ;
					O365testsObject.EnableSimulationTests = chbxonedriveupload.Checked ? true : chbxonedriveupload.Checked;
					O365testsObject.ResponseThreshold = Convert.ToInt32(string.IsNullOrEmpty(SrvAtrODUploadThTextBox.Text) ? "0" : SrvAtrODUploadThTextBox.Text);
					O365testsObject.Type = "User Scenario Tests";
					O365testsObject.Tests = chbxonedriveupload.Text;

				}
				Object result5 = VSWebBL.ConfiguratorBL.O365ServerBL.Ins.UpdateDatafortestsnew(O365testsObject);
				if (chbxonedrivedownload.Checked)
				{
					O365testsObject.ServerId = Convert.ToInt32(Request.QueryString["ID"].ToString()); ;
					O365testsObject.EnableSimulationTests = chbxonedrivedownload.Checked ? true : chbxonedrivedownload.Checked;
					O365testsObject.ResponseThreshold = Convert.ToInt32(string.IsNullOrEmpty(SrvAtrODDownloadThTextBox.Text) ? "0" : SrvAtrODDownloadThTextBox.Text);
					O365testsObject.Type = "User Scenario Tests";
					O365testsObject.Tests = chbxonedrivedownload.Text;
					//Object result6 = VSWebBL.ConfiguratorBL.O365ServerBL.Ins.UpdateDatafortestsnew(O365testsObject);
				}
				else
				{
					O365testsObject.ServerId = Convert.ToInt32(Request.QueryString["ID"].ToString()); ;
					O365testsObject.EnableSimulationTests = chbxonedrivedownload.Checked ? true : chbxonedrivedownload.Checked;
					O365testsObject.ResponseThreshold = Convert.ToInt32(string.IsNullOrEmpty(SrvAtrODDownloadThTextBox.Text) ? "0" : SrvAtrODDownloadThTextBox.Text);
					O365testsObject.Type = "User Scenario Tests";
					O365testsObject.Tests = chbxonedrivedownload.Text;

				}
				Object result6 = VSWebBL.ConfiguratorBL.O365ServerBL.Ins.UpdateDatafortestsnew(O365testsObject);
				//Directory Syncronization Test
				if (chbxdirsync.Checked)
				{
					O365testsObject.ServerId = Convert.ToInt32(Request.QueryString["ID"].ToString()); ;
					O365testsObject.EnableSimulationTests = chbxdirsync.Checked ? true : chbxdirsync.Checked;
					O365testsObject.ResponseThreshold = Convert.ToInt32(string.IsNullOrEmpty(dirsynctxt.Text) ? "0" : dirsynctxt.Text);
					O365testsObject.Type = "User Scenario Tests";
					O365testsObject.Tests = chbxdirsync.Text;
					//Object result7 = VSWebBL.ConfiguratorBL.O365ServerBL.Ins.UpdateDatafortestsnew(O365testsObject);
				}
				else
				{
					O365testsObject.ServerId = Convert.ToInt32(Request.QueryString["ID"].ToString()); ;
					O365testsObject.EnableSimulationTests = chbxdirsync.Checked ? true : chbxdirsync.Checked;
					O365testsObject.ResponseThreshold = Convert.ToInt32(string.IsNullOrEmpty(dirsynctxt.Text) ? "0" : dirsynctxt.Text);
					O365testsObject.Type = "User Scenario Tests";
					O365testsObject.Tests = chbxdirsync.Text;
				}
				Object result7 = VSWebBL.ConfiguratorBL.O365ServerBL.Ins.UpdateDatafortestsnew(O365testsObject);

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
		private void InsertTestsData()
		{
			CollectDataForO365Tests();

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
		private void CheckNodes()
		{
			List<object> fieldValues = Nodes.GetSelectedFieldValues(new string[] { "Id", "SelectedNodes" });
			DataTable dt = GetSelectedNodes();

			var duplicates = dt.AsEnumerable()
							 .GroupBy(r => r["Location"])//Using Column Name
							 .Where(gr => gr.Count() > 1);
			//.Select(g => g.Key);
			foreach (var d in duplicates)
			//    Console.WriteLine(d);
			//Console.ReadKey();
			{
				Session["NodeError"] = "There can be one node from one location. Select other nodes from different locations.";
			}
		}
		private void UpdateNodesData()
		{
			try
			{

				List<object> fieldValuesnodes = Nodes.GetSelectedFieldValues(new string[] { "Id", "SelectedNodes" });


				if (fieldValuesnodes.Count > 0)
				{
					if (Session["ID"] != null)
					{
						if (Session["ID"].ToString() != "0")
						{
							Object result2 = VSWebBL.ConfiguratorBL.O365ServerBL.Ins.UpdateNodes(Convert.ToInt32(Session["ID"].ToString()), fieldValuesnodes);

						}
						else
						{
							Object result2 = VSWebBL.ConfiguratorBL.O365ServerBL.Ins.InsertDataforNodes(fieldValuesnodes, NameTextBox.Text);
						}
					}
					else
					{
						Object result2 = VSWebBL.ConfiguratorBL.O365ServerBL.Ins.InsertDataforNodes(fieldValuesnodes, NameTextBox.Text);
					}
				}
				else
				{
					if (fieldValuesnodes.Count == 0)
					{
						errorDiv.Visible = true;
						errorDiv.InnerHtml = "Please select at least one node." +
					   "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
						errorDiv.Style.Value = "display: block";

					}

					Session["O365ServerUpdateStatus"] = NameTextBox.Text;
				}
			}
			catch (Exception ex)
			{

				errorDiv.InnerHtml = "The following error has occurred: " + ex.Message +
					"<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
				errorDiv.Style.Value = "display: block";

				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
			}
		}
		private void InsertNodesData()
		{
			try
			{
				DataTable dt = GetSelectedNodes();
				List<DataRow> NodesSelected = dt.AsEnumerable().ToList();
			}
			catch (Exception ex)
			{
				//10/13/2014 NS modified for VSPLUS-990
				errorDiv.InnerHtml = "The following error has occurred: " + ex.Message +
					"<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
				errorDiv.Style.Value = "display: block";
				//6/27/2014 NS added for VSPLUS-634
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
			}
		}
		protected void Nodes_PreRender(object sender, EventArgs e)
		{
			bool isValid = true;
			try
			{

				if (isValid)
				{

					ASPxGridView Nodes = (ASPxGridView)sender;
					for (int i = 0; i < Nodes.VisibleRowCount; i++)
					{
						if (Nodes.GetRowValues(i, "SelectedNodes").ToString() != "")
							Nodes.Selection.SetSelection(i, Convert.ToBoolean(Nodes.GetRowValues(i, "SelectedNodes").ToString()) == true);
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
		protected void Nodes_CommandButtonInitialize(object sender, ASPxGridViewCommandButtonEventArgs e)
		{
			var grid = sender as ASPxGridView;
			if (e.ButtonType == ColumnCommandButtonType.SelectCheckbox)
			{

				string Location = grid.GetRowValues(e.VisibleIndex, "Location").ToString();
				if (Location == "" || Location == null)
					e.Visible = false;
				else
					e.Visible = true;
			}
		}
		protected void Nodes_SelectionChanged(object sender, EventArgs e)
		{
			List<object> fieldValues = Nodes.GetSelectedFieldValues(new string[] { "Name" });
			if (fieldValues.Count == 0)
			{
				EnabledCheckBox.Enabled = false;
				EnabledCheckBox.Checked = false;
				enablemsg.Visible = true;
			}
			else
			{
				EnabledCheckBox.Enabled = true;
				enablemsg.Visible = false;
			}
		}
		protected void Resetoffice365Button_Click(object sender, EventArgs e)
		{


			chbxsmtp.Checked = false;
			chbxpop.Checked = false;
			chbximap.Checked = false;
			chbxautodiscovery.Checked = false;
			chbxmapi.Checked = false;
			chbxcalender.Checked = false;
			chbxinbox.Checked = false;
			chbxtask.Checked = false;
            //Durga Added for VSPLUS-2913 2/5/2016
            chbxowa.Checked = false;
            chbxcreatesite.Checked = false;
			chbxcreatefolder.Checked = false;
			chbxmailflow.Checked = false;
			chbxonedrivedownload.Checked = false;
			chbxdirsync.Checked = false;
			chbxonedriveupload.Checked = false;
			SrvAtrMailFlowThTextBox.Text = "200";
			SrvAtrCreateFolderThTextBox.Text = "500";
			SrvAtrODUploadThTextBox.Text = "200";
			SrvAtrODDownloadThTextBox.Text = "200";
            //Durga Added for VSPLUS-2913 2/5/2016
            srvAtrCreateSiteThTextBox.Text = "500";
			dirsynctxt.Text = "200";
			//if (Request.UrlReferrer != null)

		}
		protected void ASPxPageControl1_ActiveTabChanged(object source, TabControlEventArgs e)
		{
			if (ASPxPageControl1.ActiveTabIndex == 1)
			{
				Resetoffice365Button.Visible = true;
			}
			else
			{
				Resetoffice365Button.Visible = false;
			}
		}
		protected void TestsGridView_OnPageSizeChanged(object sender, EventArgs e)
		{
			////ProfilesGridView.PageIndex;
			//VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("O365ServerProperties|TestsGridView", Tests.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
			//Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
		}
		protected void NodesGridView_OnPageSizeChanged(object sender, EventArgs e)
		{
			//ProfilesGridView.PageIndex;
			VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("O365ServerProperties|NodesGridView", Nodes.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
			Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
		}
	}
}