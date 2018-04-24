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
	public partial class CloudApplicationsServerProperties : System.Web.UI.Page
	{
		string Mode;
		//11/19/2013 NS modified
		//string URL;
		string ID;
		bool flag;
		string MyObjPwd;
		string[] MyObjPwdArr;
		byte[] MyPass;
		VSFramework.TripleDES tripleDes = new VSFramework.TripleDES();

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{

				if (Session["UserPreferences"] != null)
				{
					DataTable UserPreferences = (DataTable)Session["UserPreferences"];
					foreach (DataRow dr in UserPreferences.Rows)
					{
						if (dr[1].ToString() == "CloudApplicationsServerProperties|MaintWinListGridView")
						{
							MaintWinListGridView.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
						}
						if (dr[1].ToString() == "CloudApplicationsServerProperties|AlertGridView")
						{
							AlertGridView.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
						}
					}
				}
				filldropdown();
			}


			Page.Title = "Cloud Applications Server Properties";
			if (Request.QueryString["tab"] != null)
			{
				ASPxPageControl1.ActiveTabIndex = Convert.ToInt32(Request.QueryString["tab"].ToString());
			}

			try
			{
				//11/19/2013 NS modified - need a unique ID
				//if (Request.QueryString["TheURL"] != null && Request.QueryString["TheURL"] != "")
				if ((Request.QueryString["ID"] != null && Request.QueryString["ID"] != "") || (Request.QueryString["Key"] != null && Request.QueryString["Key"] != ""))
				{
					Mode = "Update";
					ASPxMenu1.Visible = true;
					//11/19/2013 NS commented out
					//URL = Request.QueryString["TheURL"];
					//Request.QueryString["ID"] = Request.QueryString["Key"];
					if ((Request.QueryString["ID"] != null && Request.QueryString["ID"] != ""))
					{
						ID = Request.QueryString["ID"];
					}
					//else
					//{
					//    ID = Request.QueryString["Key"];
					//}
					//ID = Request.QueryString["Key"];//Somaraj
					//For Validation Summary
					////ApplyValidationSummarySettings();
					////ApplyEditorsSettings();
					if (!IsPostBack)
					{

						//Fill Server Attributes Tab & Advanced Tab
						//11/19/2013 NS modified
						//FillData(URL);
						FillData(ID);
						FillMaintenanceGrid();
						filldropdown();

						FillAlertGridView();
						// IPAddressTextBox.Enabled = false;
						//10/7/2014 NS modified for VSPLUS-934
						//URLRoundPanel.HeaderText = "Cloud Application Server -  " + " " + CredentialsComboBox.Text;
						servernamelbldisp.InnerHtml += " - " + CredentialsComboBox.Text;
					}
					else
					{
						FillMaintServersGridfromSession();
						FillAlertGridViewfromSession();
						filldropdown();

					}

				}
				else
				{
					Mode = "Insert";
					ASPxMenu1.Visible = false;
					if (!IsPostBack)
					{
						filldropdown();
						RetryTextBox.Text = "2";
						OffScanTextBox.Text = "30";
						ScanTextBox.Text = "8";
						IPAddressTextBox.Text = "http://";
						RespThrTextBox.Text = "2500";

						EnabledCheckBox.Checked = true;
						//   NameTextBox.Focus();

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

		//11/19/2013 NS modified
		//private void FillData(string URL)
		private void FillData(string ID)
		{
			try
			{

				CloudApplicationsServer CloudApplicationsServerObject = new CloudApplicationsServer();
				CloudApplicationsServer ReturnDCObject = new CloudApplicationsServer();
				//11/19/2013 NS modified
				//URLsObject.TheURL = URL;
				CloudApplicationsServerObject.ID = ID;
				ReturnDCObject = VSWebBL.ConfiguratorBL.CloudApplicationsServerBL.Ins.GetData(CloudApplicationsServerObject);
				//Cluster Setting fields
				CredentialsComboBox.Text = ReturnDCObject.Name.ToString();
				CategoryTextBox.Text = ReturnDCObject.Category.ToString();
				ScanTextBox.Text = ReturnDCObject.ScanInterval.ToString();
				OffScanTextBox.Text = ReturnDCObject.OffHoursScanInterval.ToString();
				EnabledCheckBox.Checked = ReturnDCObject.Enabled;//(ScanTextBox.Text != null ? true : false);
				RetryTextBox.Text = ReturnDCObject.RetryInterval.ToString();
				RespThrTextBox.Text = ReturnDCObject.ResponseThreshold.ToString();
				IPAddressTextBox.Text = ReturnDCObject.URL;
				Image1.ImageUrl = ReturnDCObject.imageurl;
				RequiredTextBox.Text = ReturnDCObject.SearchStringNotFound;
				txtSearch.Text = ReturnDCObject.SearchStringFound;
				UserNameTextBox.Text = ReturnDCObject.UserName;
				PasswordTextBox.Text = ReturnDCObject.PW;
				if (PasswordTextBox.Text != "" && PasswordTextBox.Text != null)
				{
					PasswordTextBox.Text = "      ";
				}
				else
				{
					PasswordTextBox.Text = "";
				}

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
						ViewState["PWD"] = tripleDes.Decrypt(MyPass);
					}
					catch (Exception ex)
					{
						if (ex.Message == "Input string was not in a correct format.")
						{
							ViewState["PWD"] = MyObjPwd;
						}
					}
				}

				SrvAtrFailBefAlertTextBox.Text = ReturnDCObject.FailureThreshold.ToString();
				//11/19/2013 NS modified
				//Session["ReturnUrl"] = "URLProperties.aspx?TheURL=" + URL + "tab=1";
				//Mukund 11Apr14 -missing "&"
				Session["ReturnUrl"] = "CloudApplicationsServerProperties.aspx?ID=" + ID + "&tab=1";

			}
			catch (Exception ex)
			{
				//6/27/2014 NS added for VSPLUS-634
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}
			finally { }
		}

		private CloudApplicationsServer CollectDataForCloudApplicationsServer()
		{
			try
			{
				//Cluster Settings
				CloudApplicationsServer CloudApplicationsServerObject = new CloudApplicationsServer();
				//12/9/2013 NS added
				CloudApplicationsServerObject.ID = ID;
				CloudApplicationsServerObject.Name = CredentialsComboBox.Text;
				CloudApplicationsServerObject.Category = CategoryTextBox.Text;
				CloudApplicationsServerObject.Enabled = EnabledCheckBox.Checked;

				//URLsObject.First_Alert_Threshold = int.Parse(AlertTextBox.Text);
				CloudApplicationsServerObject.OffHoursScanInterval = int.Parse(OffScanTextBox.Text);
				CloudApplicationsServerObject.ScanInterval = int.Parse(ScanTextBox.Text);
				CloudApplicationsServerObject.RetryInterval = int.Parse(RetryTextBox.Text);
				CloudApplicationsServerObject.ResponseThreshold = int.Parse(RespThrTextBox.Text);
				CloudApplicationsServerObject.SearchStringNotFound = RequiredTextBox.Text;
				CloudApplicationsServerObject.SearchStringFound = txtSearch.Text;
				CloudApplicationsServerObject.URL = IPAddressTextBox.Text;
				CloudApplicationsServerObject.imageurl = Image1.ImageUrl;
				CloudApplicationsServerObject.UserName = UserNameTextBox.Text;
				if (PasswordTextBox.Text != "")
				{
					if (PasswordTextBox.Text == "      ")
						PasswordTextBox.Text = ViewState["PWD"].ToString();
					TripleDES tripleDES = new TripleDES();
					byte[] encryptedPass = tripleDES.Encrypt(PasswordTextBox.Text);
					string encryptedPassAsString = string.Join(", ", encryptedPass.Select(s => s.ToString()).ToArray());
					CloudApplicationsServerObject.PW = encryptedPassAsString;
				}

				//  CloudApplicationsServerObject.PW = PasswordTextBox.Text;

				Locations LOCobject = new Locations();


				ServerTypes STypeobject = new ServerTypes();
				STypeobject.ServerType = "Cloud";
				ServerTypes ReturnValue = VSWebBL.SecurityBL.ServerTypesBL.Ins.GetDataForServerType(STypeobject);
				CloudApplicationsServerObject.ServerTypeId = ReturnValue.ID;


				Locations ReturnLocValue = VSWebBL.SecurityBL.LocationsBL.Ins.GetDataForLocation(LOCobject);
				CloudApplicationsServerObject.LocationId = ReturnLocValue.ID;
				CloudApplicationsServerObject.Location = ReturnLocValue.Location;
				CloudApplicationsServerObject.FailureThreshold = int.Parse(SrvAtrFailBefAlertTextBox.Text);

				return CloudApplicationsServerObject;
			}
			catch (Exception ex)
			{
				//6/27/2014 NS added for VSPLUS-634
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}
			finally { }
		}



		private void UpdateCloudApplicationsServerData()
		{
			try
			{


				CloudApplicationsServer UrlObj = new CloudApplicationsServer();
				//11/19/2013 NS added ID below
				UrlObj.ID = ID;
				UrlObj.URL = IPAddressTextBox.Text;
				UrlObj.Name = CredentialsComboBox.Text;
				UrlObj.Category = CategoryTextBox.Text;
				DataTable returntable = VSWebBL.ConfiguratorBL.CloudApplicationsServerBL.Ins.GetIPAddress(UrlObj, "Update");

				if (returntable.Rows.Count > 0)
				{
					//3/19/2014 NS modified
					//ErrorMessageLabel.Text = "This URL Address/Name is already being monitored.  Please enter another IP Address/Name.";
					//ErrorMessagePopupControl.ShowOnPageLoad = true;
					//10/8/2014 NS modified for VSPLUS-990
					errorDiv.InnerHtml = "This CloudApplicationsServer Address/Name is already being monitored. Please enter another IP Address/Name." +
						"<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
					errorDiv.Style.Value = "display: block";
					flag = true;
					//IPAddressTextBox.Focus();
				}
				else
				{
					try
					{
						Object ReturnValue = VSWebBL.ConfiguratorBL.CloudApplicationsServerBL.Ins.UpdateData(CollectDataForCloudApplicationsServer());
						SetFocusOnError(ReturnValue);
						if (ReturnValue.ToString() == "True")
						{
							//3/19/2014 NS modified
							/*
							ErrorMessageLabel.Text = "The URL has been successfully updated.";
							ErrorMessagePopupControl.HeaderText = "Information";
							ErrorMessagePopupControl.ShowCloseButton = false;
							ValidationUpdatedButton.Visible = true;
							ValidationOkButton.Visible = false;
							ErrorMessagePopupControl.ShowOnPageLoad = true;
							*/
							Session["CloudApplicationsServerUpdateStatus"] = CredentialsComboBox.Text;
							Response.Redirect("CloudApplicationsServerGrid.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
							Context.ApplicationInstance.CompleteRequest();
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
		private void InsertCloudApplicationsServer()
		{
			try
			{


				CloudApplicationsServer UrlObj = new CloudApplicationsServer();
				UrlObj.URL = IPAddressTextBox.Text;
				UrlObj.Name = CredentialsComboBox.Text;
				UrlObj.Category = CategoryTextBox.Text;
				DataTable returntable = VSWebBL.ConfiguratorBL.CloudApplicationsServerBL.Ins.GetIPAddress(UrlObj, "Insert");

				if (returntable.Rows.Count > 0)
				{
					//ErrorMessageLabel.Text = "This URL Address/Name is already being monitored.  Please enter another IP Address/Name.";
					//ErrorMessagePopupControl.ShowOnPageLoad = true;
					//10/8/2014 NS modified for VSPLUS-990
					errorDiv.InnerHtml = "This CloudApplicationsServer Address/Name is already being monitored. Please enter another IP Address/Name." +
						"<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
					errorDiv.Style.Value = "display: block";
					flag = true;
					//IPAddressTextBox.Focus();
				}
				else
				{

					try
					{
						object ReturnValue = VSWebBL.ConfiguratorBL.CloudApplicationsServerBL.Ins.InsertData(CollectDataForCloudApplicationsServer());
						SetFocusOnError(ReturnValue);
						if (ReturnValue.ToString() == "True")
						{
							//3/19/2014 NS modified
							/*
							ErrorMessageLabel.Text = "The URL has been successfully updated.";
							ErrorMessagePopupControl.HeaderText = "Information";
							ErrorMessagePopupControl.ShowCloseButton = false;
							ValidationUpdatedButton.Visible = true;
							ValidationOkButton.Visible = false;
							ErrorMessagePopupControl.ShowOnPageLoad = true;
							*/
							Session["CloudApplicationsServerUpdateStatus"] = CredentialsComboBox.Text;
							Response.Redirect("CloudApplicationsServerGrid.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
							Context.ApplicationInstance.CompleteRequest();
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
		private void SetFocusOnError(Object ReturnValue)
		{
			string ErrorMessage = ReturnValue.ToString();
			if (ErrorMessage.Substring(0, 2) == "ER")
			{
				//3/19/2014 NS modified
				//ErrorMessageLabel.Text = ErrorMessage.Substring(3);
				//ErrorMessagePopupControl.ShowOnPageLoad = true;
				//10/8/2014 NS modified for VSPLUS-990
				errorDiv.InnerHtml = ErrorMessage.Substring(3) +
					"<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
				errorDiv.Style.Value = "display: block";
			}
		}

		protected void FormOkButton_Click(object sender, EventArgs e)
		{
			//try
			//{
			//    VSWebBL.SettingBL.SettingsBL.Ins.UpdateSvalue("CloudApplicationsServer Update", "True", VSWeb.Constants.Constants.SysString);
			//}
			//catch (Exception ex)
			//{
			//    //6/27/2014 NS added for VSPLUS-634
			//    Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
			//    throw ex;
			//}
			try
			{
				if (Mode == "Update")
				{

					UpdateCloudApplicationsServerData();
				}
				if (Mode == "Insert")
				{
					InsertCloudApplicationsServer();
					if (flag == false)
						InsertStatus();
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

		protected void FormCancelButton_Click(object sender, EventArgs e)
		{
			Response.Redirect("CloudApplicationsServerGrid.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
			Context.ApplicationInstance.CompleteRequest();
		}

		protected void ValidationUpdatedButton_Click(object sender, EventArgs e)
		{
			Response.Redirect("CloudApplicationsServerGrid.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
			Context.ApplicationInstance.CompleteRequest();
		}

		private Status CollectDataforStatus()
		{
			Status St = new Status();
			try
			{

				//St.DeadMail = 0;
				St.Description = IPAddressTextBox.Text;
				//St.Details = "";
				St.DownCount = 0;
				// St.Location = "Cluster";
				St.Name = CredentialsComboBox.Text;
				//St.MailDetails = "Mail Details";
				// St.PendingMail = 0;
				St.sStatus = "Not Scanned";
				St.Type = "Cloud";
				St.Upcount = 0;
				St.UpPercent = 1;
				St.LastUpdate = System.DateTime.Now;
				St.ResponseTime = 0;
				St.TypeANDName = CredentialsComboBox.Text + "-Cloud";
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
					//3/19/2014 NS modified
					/*
					ErrorMessageLabel.Text = "The URL has been successfully updated.";
					ErrorMessagePopupControl.HeaderText = "Information";
					ErrorMessagePopupControl.ShowCloseButton = false;
					ValidationUpdatedButton.Visible = true;
					ValidationOkButton.Visible = false;
					ErrorMessagePopupControl.ShowOnPageLoad = true;
					*/
					Session["CloudApplicationsServerUpdateStatus"] = CredentialsComboBox.Text;
					Response.Redirect("CloudApplicationsServerGrid.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
					Context.ApplicationInstance.CompleteRequest();
				}

			}
			catch (Exception ex)
			{
				//3/19/2014 NS modified
				//ErrorMessageLabel.Text = "Error attempting to update the status table :" + ex.Message;
				//ErrorMessagePopupControl.ShowOnPageLoad = true;
				//10/8/2014 NS modified for VSPLUS-990
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

			string myName = CredentialsComboBox.Text;
			try
			{
				if (myName.IndexOf("'") > 0)
					myName = myName.Replace("'", "");

				if (myName.IndexOf(Quote) > 0)
					myName = myName.Replace(Quote, '~');

				if (myName.IndexOf(",") > 0)
					myName = myName.Replace(",", " ");

				if (myName != CredentialsComboBox.Text)
					CredentialsComboBox.Text = myName;

			}
			catch (Exception ex)
			{
				//   MessageBox.Show(ex.Message)
				//6/27/2014 NS added for VSPLUS-634
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
			}

		}



		private void FillMaintenanceGrid()
		{
			try
			{

				DataTable MaintDataTable = new DataTable();
				DataSet ServersDataSet = new DataSet();
				MaintDataTable = VSWebBL.ConfiguratorBL.MaintenanceBL.Ins.GetMaintenDataOnServerID(CredentialsComboBox.Text);
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
				AlertDataTable = VSWebBL.ConfiguratorBL.AlertsBL.Ins.GetAlertTab(CredentialsComboBox.Text, "CloudApplicationsServer");
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


		protected void MaintWinListGridView_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
		{
			e.Row.Attributes.Add("onmouseover", "this.style.backgroundColor='#C0C0C0';");

			e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='white';");
		}

		protected void MaintWinListGridView_PageSizeChanged(object sender, EventArgs e)
		{
			//ProfilesGridView.PageIndex;
			VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("CloudApplicationsServerProperties|MaintWinListGridView", MaintWinListGridView.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
			Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
		}

		protected void AlertGridView_PageSizeChanged(object sender, EventArgs e)
		{
			//ProfilesGridView.PageIndex;
			VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("CloudApplicationsServerProperties|AlertGridView", AlertGridView.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
			Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
		}

		protected void CredentialsComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			string name = CredentialsComboBox.SelectedItem.Text;
			DataTable dt = new DataTable();
			dt = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.getimagepath(name);
			if (dt.Rows.Count > 0)
			{
				Image1.ImageUrl = dt.Rows[0]["Image"].ToString();
				IPAddressTextBox.Text = dt.Rows[0]["Url"].ToString();
			}
		}
		protected void filldropdown()
		{
			DataTable dt = new DataTable();
			//string item = CredentialsComboBox.SelectedItem.Text;
			dt = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.filldropdown();
			CredentialsComboBox.DataSource = dt;
			CredentialsComboBox.DataBind();
		}

		protected void ASPxMenu1_ItemClick(object source, DevExpress.Web.MenuItemEventArgs e)
		{


			//if (e.Item.Name == "ServerDetailsPage")
			//{
			//    Response.Redirect("~/DashBoard/DominoServerDetailsPage2.aspx", false);
			//    Context.ApplicationInstance.CompleteRequest();
			//}

			DataTable dt;
			Session["Submenu"] = "LotusDomino";
			//if (Request.QueryString["Name"] != "" && Request.QueryString["Name"] != null)
			//{
			//    id = (VSWebBL.SecurityBL.ServersBL.Ins.GetServerIDbyServerName(Request.QueryString["Name"].ToString())).ToString();
			//    Response.Redirect("~/Dashboard/DominoServerDetailsPage2.aspx?Key=" + id, false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
			//    Context.ApplicationInstance.CompleteRequest();
			//}
			if (Request.QueryString["ID"] != "" && Request.QueryString["ID"] != null)
			{
				dt = (VSWebBL.SecurityBL.ServersBL.Ins.GetCloudDetailsByID(Convert.ToInt32(Request.QueryString["ID"])));
				//dt = (VSWebBL.SecurityBL.ServersBL.Ins.GetServerDetailsByID(Convert.ToInt32(Request.QueryString["ID"])));
				if (dt.Rows.Count > 0)
				{
					
					Response.Redirect("~/Dashboard/CloudDetails.aspx?Name=" + dt.Rows[0]["Name"].ToString() + "&Type=" + dt.Rows[0]["Type"].ToString() + "&LastDate=" + dt.Rows[0]["LastUpdate"].ToString() , false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
					Context.ApplicationInstance.CompleteRequest();
				}
			}


		}
	}

}