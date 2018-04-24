using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using System.IO;
using System.Web.UI.HtmlControls;
using VSFramework;
using VSWebBL;
using VSWebDO;
using DevExpress.Web;
using System.Text;
using System.Xml;
using System.Diagnostics;
using System.Drawing;
using System.Xml.Serialization;
using DevExpress.Web.ASPxTreeList;
using VitalSignsWebSphereDLL;
using System.Net;
using System.Net.Sockets;
namespace VSWebUI.Configurator
{
	public partial class SameTimeServer : System.Web.UI.Page
	{
		DataTable WebSphereDataTable = null;
		int key;
		static int serverid = 0;
		string Mode;
		bool flag = false;
		string fileName1;
		int samecellid;
		bool insertcell = false;
		bool cellnameinsert = true;
	  IPHostEntry hostEntry;
	  bool validhostname = true;
	string UIip, dbipaddress, port, convertdbip;
	IPAddress[] ipaddress;
		VSFramework.TripleDES mytestenkey = new VSFramework.TripleDES();
		VitalSignsWebSphereDLL.VitalSignsWebSphereDLL WSDll = new VitalSignsWebSphereDLL.VitalSignsWebSphereDLL();
		VitalSignsWebSphereDLL.VitalSignsWebSphereDLL.CellProperties cellFromInfo = new VitalSignsWebSphereDLL.VitalSignsWebSphereDLL.CellProperties();
		//SametimeServers sametimeObject;
		//byte[] MyPass = null;
		//string mySametimePassword = "";
		//string mySametimeUsername = "";

		//string xmlSametime;

		protected void Page_Load(object sender, EventArgs e)
		{

			
			if (Request.QueryString["tab"] != null)
			{
				ASPxPageControl1.ActiveTabIndex = Convert.ToInt32(Request.QueryString["tab"].ToString());
			}
			if (Request.QueryString["tabs"] == "1")
			{
				//SortRadioButtonList1.Value = 2;
				//SortRadioButtonList1.SelectedIndex = 1;
				//ASPxPageControl1.TabPages[1].ClientVisible = true;
				//ASPxPageControl1.ActiveTabIndex = 1;
				chkWsMeetingServerStats.Visible = true;
				txtWsMeetingHostName.Visible = true;
				lblWsMeetingHostName.Visible = true;
				txtWSMeetingPort.Visible = true;
				lblWSMeetingPort.Visible = true;
				chkWsMeetingRequireSSL.Visible = true;
				lblWsMeetingRequireSSL.Visible = true;

				chkWsMediaServerStats.Visible = true;
				txtWsMediaHostname.Visible = true;
				lblWsMediaHostname.Visible = true;
				txtWsMediaPort.Visible = true;
				lblWsMediaPort.Visible = true;
				chkWsMediaRequireSSL.Visible = true;
				lblWsMediaRequireSSL.Visible = true;
				CredentialsLabel.Visible = true;
				WebCredentialsComboBox.Visible = true;
				Credentialsbtn.Visible = true;
				reallbl.Visible = true;
				realmtxtbx.Visible = true;
				CellnameTextBox.Text = Session["cellname"].ToString();
				HostName.Text = Session["HostName"].ToString();
				ConnectionComboBox.Text = Session["ConnectionComboBox"].ToString();
				txtport.Text = Session["txtport"].ToString();
				chbx.Checked = Convert.ToBoolean(Session["chbx"].ToString());
				
				
			}
			
			try
			{
				if (Request.QueryString["ID"] != null && Request.QueryString["ID"] != "")

				{
					Session["sametimeid"] = Request.QueryString["ID"];
					FillCredentialsComboBox();

					Mode = "Update";
					//key = int.Parse(Request.QueryString["ID"]);
					key = Convert.ToInt32(Session["sametimeid"].ToString());
					//lblServerID.Text = key.ToString();
					if (!IsPostBack)
					{
						Session["DataEvents"] = null;
						//fillsametimeNodesTree();
						//HtmlAnchor anchorFilepath = (HtmlAnchor)
						FillSametimeCredentialComboBox();
						WebsphereCell sametimeObject = new WebsphereCell();
						sametimeObject.SametimeId = key;
						DataTable dt = VSWebBL.ConfiguratorBL.SametimeServerBL.Ins.GetcellID(sametimeObject);

						
						if (dt.Rows.Count > 0)
						{
							Session["webcellid"] = dt.Rows[0]["CellID"].ToString();
							FillWebsphereCell(key);

						}
						filldetails(key);

						if (Request.QueryString["tabs"] == "1")
						{
							SortRadioButtonList1.Value = 2;
							SortRadioButtonList1.SelectedIndex = 1;
							ASPxPageControl1.TabPages[1].ClientVisible = true;
							ASPxPageControl1.ActiveTabIndex = 1;
						}
						FillMaintenanceGrid();
						FillAlertGridView();

						//10/21/2014 NS modified for VSPLUS-934
						//SameTimeRoundPanel.HeaderText = "Sametime Servers - " + ServerNameTextBox.Text;
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

			catch (Exception ex)
			{
				//6/27/2014 NS added for VSPLUS-634
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}


			if (!IsPostBack)
			{

				Session["Submenu"] = "";
				//FillWindowsServerGrid();
				if (Session["UserPreferences"] != null)
				{
					DataTable UserPreferences = (DataTable)Session["UserPreferences"];
					foreach (DataRow dr in UserPreferences.Rows)
					{
						if (dr[1].ToString() == "WebsphereGridView|WebsphereGridView")
						{
							//WebsphereGridView.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
						}
					}
				}
			}
			else
			{

				//FillWebSphereserverGridfromSession();

			}
			//3/27/2014 MD added for
			if (Session["WebSphereUpdateStatus"] != null)
			{
				if (Session["WebSphereUpdateStatus"].ToString() != "")
				{
					//10/21/2014 NS modified for VSPLUS-990
					successDiv.InnerHtml = "WebSphere Server information for <b>" + Session["WebSphereUpdateStatus"].ToString() +
						"</b> updated successfully." +
						"<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
					successDiv.Style.Value = "display: block";
					Session["WebSphereUpdateStatus"] = "";
				}
			}
		}

		protected void okButton_Click(object sender, EventArgs e)
		{
		
			VSWebBL.SettingBL.SettingsBL.Ins.UpdateSvalue("Sametime Server Update", "True", VSWeb.Constants.Constants.SysString);
			try
			{
				if (Mode == "Update")
				{

					if (SortRadioButtonList1.SelectedItem.Text == "WebSphere")
					{

						if (CellnameTextBox.Text == "" || HostName.Text == "" || ConnectionComboBox.Text == "")
						{

							ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please enter Cell Name, Host Name, Port no, select connection type in WebSphere  Settings Tab.')", true);

							ASPxPageControl1.ActiveTabIndex = 1;
						}
						else
						{
							try
							{
								if (validhostname == true)
			{
								ipaddress = Dns.GetHostAddresses(HostName.Text);
								foreach (IPAddress var in ipaddress)
								{
									if (var.AddressFamily == AddressFamily.InterNetwork)
									{
										UIip = var.ToString();

										break;
									}
								}
								DataTable dt = VSWebBL.SecurityBL.webspehereImportBL.Ins.GetAllHostNmaes();
								for (int i = 0; i < dt.Rows.Count; i++)
								{
									dbipaddress = dt.Rows[i]["HostName"].ToString();
									port = dt.Rows[i]["PortNo"].ToString();
									ipaddress = Dns.GetHostAddresses(dbipaddress);

									foreach (IPAddress var in ipaddress)
									{
										if (var.AddressFamily == AddressFamily.InterNetwork)
										{
											convertdbip = var.ToString();
											break;
										}
									}

									if (convertdbip == UIip && port == txtport.Text)
									{
										if (Session["cellid"] == null)
										{
											cellnameinsert = false;
										}
										else if (Session["HostName"].ToString() == HostName.Text && Session["PortNo"].ToString() == txtport.Text)
										{
											cellnameinsert = true;
										}
										else
										{
											cellnameinsert = false;
										}
									}
								}
									if (cellnameinsert == true)
									{
										UpdateSametimeServer();
										Insertdata();
										insertcell = true;

									}

									else
									{
										errorDiv.InnerHtml = "Cell Name already exists with this WebSphere  configuration.Please enter another WebSphere  configuration." +
												 "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
										errorDiv.Style.Value = "display: block";
									}
								//}
			}
								else
								{
									errorDivForImportingWS.InnerHtml = "Please enter valid Host Name." +
					"<button type=\"button\" onclick='javascript:var SearchInput = $(\"#ContentPlaceHolder1_ASPxPageControl1_ASPxRoundPanel7_ASPxCallbackPanel2_HostName_I\");var strLength= SearchInput.val().length * 2;SearchInput.focus();SearchInput[0].setSelectionRange(strLength, strLength);' class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
									errorDivForImportingWS.Style.Value = "display: block";
								}
							}

							catch (Exception ex)
							{
							

							}
							
						}
					}
					else
					{
						//Insertdata();
						UpdateSametimeServer();
					}
				}
				if (Mode == "Insert")
				{
					//else
					//{
					//    Insertdata();
					//}

					InsertdetailsSametimeServer();
					if (flag == false)
						InsertStatus();
				}




				//..............................


				object ReturnValue2;
				string enabledservers = "";
				if (Session["webcellid"] != null && Session["webcellid"] != "")
				{
					int cellsid = Convert.ToInt32(Session["webcellid"].ToString());
					bool updatewebservers = VSWebBL.SecurityBL.webspehereImportBL.Ins.updatewebservers(cellsid);
					DataTable dt = GetSelectedEvents(cellsid);
					if (dt.Rows.Count > 0)
					{
						for (int i = 0; i < dt.Rows.Count; i++)
						{
							Servers ServersObject = new Servers();
							//if (Convert.ToInt32(dt.Rows[i]["ServerID"]) != null)
							//{
							int serverid = Convert.ToInt32(dt.Rows[i]["ServerID"]);
							//}
							string enable = dt.Rows[i]["Enabled"].ToString();
							ServersObject.ServerName = dt.Rows[i]["NodeName"].ToString();
							ServersObject.LocationID = 1;//Location information required here
							string ServerName = dt.Rows[i]["NodeName"].ToString();
							ServerTypes STypeobject = new ServerTypes();
							STypeobject.ServerType = "WebSphere";
							ServersObject.Description = "Sametime WebSphere Product";
							ServersObject.IPAddress = "111.898.2435";

							ServerTypes ReturnValue = VSWebBL.SecurityBL.ServerTypesBL.Ins.GetDataForServerType(STypeobject);
							ServersObject.ServerTypeID = ReturnValue.ID;

							//	bool update = VSWebBL.SecurityBL.webspehereImportBL.Ins.updatedata(cellid, enable);
							DataTable returntable = VSWebBL.SecurityBL.webspehereImportBL.Ins.GetServerName(ServersObject);
							int sid = 0; string sname = "";
							if (returntable.Rows.Count == 0)
							{
								ReturnValue2 = VSWebBL.SecurityBL.webspehereImportBL.Ins.InsertwebsphereData(ServersObject);
								DataTable returntable1 = VSWebBL.SecurityBL.webspehereImportBL.Ins.GetServerName(ServersObject);
								sid = Convert.ToInt32(returntable1.Rows[0]["ID"].ToString());
								sname = returntable1.Rows[0]["ServerName"].ToString();
							}
							else
							{
								sid = Convert.ToInt32(returntable.Rows[0]["ID"].ToString());
								sname = returntable.Rows[0]["ServerName"].ToString();
							}

							//bool update = VSWebBL.SecurityBL.webspehereImportBL.Ins.updatedata(serverid, enable);
							bool update = VSWebBL.SecurityBL.webspehereImportBL.Ins.updatedataweb(sid, sname, enable);
							Object ReturnValue3 = VSWebBL.SecurityBL.ServersBL.Ins.UpdateAttributesData(CollectDataforServerAttributes(ServerName));
							if (dt.Rows[i]["NodeName"].ToString() != "")
							{
								Session["enabledservers"] += dt.Rows[i]["NodeName"].ToString() + ", ";
							}
						}

					}
					if (SortRadioButtonList1.SelectedItem.Text == "WebSphere")
					{

						if (CellnameTextBox.Text == "" || HostName.Text == "" || ConnectionComboBox.Text == "")
						{
							ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please enter Cell Name, Host Name, Port no, select connection type in WebSphere  Settings Tab.')", true);

							ASPxPageControl1.ActiveTabIndex = 1;


						}
						else if (insertcell==false)
						{
							errorDiv.InnerHtml = "Cell Name already exists with this WebSphere  configuration.Please enter another WebSphere  configuration." +
											   "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
							errorDiv.Style.Value = "display: block";

						}
						else
						{
							Session["NodesServers"] = "";
							Response.Redirect("~/Configurator/LotusSametimeGrid.aspx");
						}
					}
					else
					{
						Session["NodesServers"] = "";
						Response.Redirect("~/Configurator/LotusSametimeGrid.aspx");
						//fillNodesTreefromSession();
					}

				}
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

		public SametimeServers GetdataForSametimeServer()
		{
			try
			{
				SametimeServers getdata = new SametimeServers();
				// getdata.Name = ServerNameTextBox.Text;
				// getdata.LocationID = LocationTextBox.Text;
				// getdata.IPAddress = IPAddressTextBox.Text;
				getdata.Category = CategoryTextBox.Text;
				// getdata.Description = DescriptionTextBox.Text;
				getdata.ResponseThreshold = Convert.ToInt32(ScanIntervalTextBox.Text);
				getdata.RetryInterval = Convert.ToInt32(RetryIntervalTextBox.Text);
				getdata.OffHoursScanInterval = Convert.ToInt32(OffHoursScanIntervalTextBox.Text);
				getdata.ResponseThreshold = Convert.ToInt32(ResponseThresholdTextBox.Text);
				getdata.ScanInterval = int.Parse(ScanIntervalTextBox.Text);
				getdata.stcommlaunch = Convert.ToBoolean(CommunityservicesLancherCheckBox.Checked);
				getdata.stcommunity = Convert.ToBoolean(CommunityServicesCheckBox.Checked);
				getdata.stconfigurationapp = Convert.ToBoolean(CommunityServicesConfigurationCheckBox.Checked);
				getdata.stplaces = Convert.ToBoolean(SametimePlacesServicesCheckBox.Checked);
				getdata.stmux = Convert.ToBoolean(CommunityServicesMultiplexerCheckBox.Checked);
				getdata.stusers = Convert.ToBoolean(CommunityUserServicesCheckBox.Checked);
				getdata.stonlinedir = Convert.ToBoolean(CommunityOnlineDirectoryServicesCheckBox.Checked);
				getdata.stconference = Convert.ToBoolean(CommunityConferenceServicesCheckBox.Checked);
				getdata.stdirectory = Convert.ToBoolean(CommunityDirectoryServicesCheckBox.Checked);
				getdata.stlogger = Convert.ToBoolean(CommunityLoggingServicesCheckBox.Checked);
				getdata.stprivacy = Convert.ToBoolean(UserprivacyInformationCheckBox.Checked);
				getdata.stsecurity = Convert.ToBoolean(SecurityServicesCheckBox.Checked);
				getdata.stpresencemgr = Convert.ToBoolean(CommunityBuddyListPresenceceServicesCheckBox.Checked);
				getdata.stservicemanager = Convert.ToBoolean(JavaServiceManagerCheckBox.Checked);
				getdata.steventserver = Convert.ToBoolean(EventServerCheckBox.Checked);
				getdata.stpolicy = Convert.ToBoolean(SametimepolicyServicesCheckBox.Checked);
				getdata.stconfigurationbridge = Convert.ToBoolean(ConfigurationBridgeCheckBox.Checked);
				getdata.stadminsrv = Convert.ToBoolean(SametimeAdminCheckBox.Checked);
				getdata.stchatlogging = Convert.ToBoolean(CommunityChatLoggingServicesCheckBox.Checked);
				getdata.stpolling = Convert.ToBoolean(CommunitypollingServicesCheckBox.Checked);
				getdata.stresolve = Convert.ToBoolean(UsernameResolutionCheckBox.Checked);
				getdata.stuserstorage = Convert.ToBoolean(UserconnectListandPrefsCheckBox.Checked);
				getdata.stpresencecompatmgr = Convert.ToBoolean(CommunityBuddyListBackwordCompatabilityCheckBox.Checked);
				getdata.SSL = Convert.ToBoolean(ThisserverrequiresSSLCheckBox.Checked);
				getdata.stconference = Convert.ToBoolean(CommunityConferenceServicesCheckBox.Checked);
				getdata.Enabled = Convert.ToBoolean(EnabledForScanningCheckBox.Checked);
				getdata.srvawareness = Convert.ToBoolean(AwarenessCheckBox.Checked);

				getdata.srvdirectory = Convert.ToBoolean(DirectoryCheckBox.Checked);

				getdata.srvstorage = Convert.ToBoolean(StorageCheckBox.Checked);

				getdata.srvbuddylist = Convert.ToBoolean(BuddyListCheckBox.Checked);

				getdata.srvplace = Convert.ToBoolean(PlaceCheckBox.Checked);

				getdata.srvlookup = Convert.ToBoolean(LookUpCheckBox.Checked);

				getdata.srvtestchat = Convert.ToBoolean(TestchatCheckBox.Checked);

				getdata.srvtestmeeting = Convert.ToBoolean(TestmeetingCheckBox.Checked);

				getdata.srvquery = Convert.ToBoolean(QueryDirectoryCheckBox.Checked);
				//getdata.generalport = int.Parse(PortTextBox.Text);
				getdata.generalport = Convert.ToInt32(RetryIntervalTextBox.Text);
				getdata.db2hostname = HostNameTextBox.Text;

				//getdata.db2login = LoginTextBox.Text;

				//getdata.db2password = PasswordTextBox.Text;
				getdata.db2databasename = DataBaseNameTextBox.Text;
				getdata.db2port = DB2PortTextBox.Text;

				getdata.CredentialID = Convert.ToInt32(CredentialComboBox.Value);


				// getdata.proxyprotocol = ProtocolComboBox.SelectedItem.Text;
				//getdata.proxytype = ProtocolComboBox.SelectedItem.Text;
				getdata.proxyprotocol = ProtocolComboBox.Text;
				getdata.proxytype = ProxyTypeComboBox.Text;
				getdata.Platform = SortRadioButtonList1.SelectedItem.Text;
				//getdata.PortCheckBox = Convert.ToBoolean(enabledb2port.Checked);


				//1/7/2014 NS commented out
				/*
				if(UsersCheckBox.Checked==true)
				{
				   UsersTextBox.Enabled=true;
				   //1/24/2013 NS added a check below, otherwise the conversion to integer throws an exception
				   if (UsersTextBox.Text == "")
				   {
					   UsersTextBox.Text = "0";
				   }
				   getdata.UserThreshold = Convert.ToInt32(UsersTextBox.Text);
				}
				getdata.stsecurity = Convert.ToBoolean(SecurityServicesCheckBox.Checked);
				if(PlacesCheckBox.Checked==true)
				{
					PlacesTextBox.Enabled=true;
					//1/24/2013 NS added a check below, otherwise the conversion to integer throws an exception
					if (PlacesTextBox.Text == "") 
					{
						PlacesTextBox.Text = "0";
					}
					getdata.PlacesThreshold = Convert.ToInt32(PlacesTextBox.Text);
				}
				getdata.stlinks = Convert.ToBoolean(SametimeLinksAppLauncherCheckBox.Checked);

				if (ChatSessionsCheckBox.Checked==true)
				{
					ChatSessionsTextBox.Enabled = true;
					//1/24/2013 NS added a check below, otherwise the conversion to integer throws an exception
					if (ChatSessionsTextBox.Text == "")
					{
						ChatSessionsTextBox.Text = "0";
					}
					getdata.ChatThreshold = Convert.ToInt32(ChatSessionsTextBox.Text);
				}
				if (nWayChatSessionsCheckBox.Checked==true)
				{
					nWayChatSessionsTextBox.Enabled = true;
					//1/24/2013 NS added a check below, otherwise the conversion to integer throws an exception
					if (nWayChatSessionsTextBox.Text == "")
					{
						nWayChatSessionsTextBox.Text = "0";
					}
					getdata.NChatThreshold = Convert.ToInt32(nWayChatSessionsTextBox.Text);
				}
				 */
				getdata.SID = int.Parse(lblServerID.Text);
				if (Mode == "Update")
				{
					getdata.ID = key;

				}
				getdata.FailureThreshold = int.Parse(SrvAtrFailBefAlertTextBox.Text);
                getdata.TestChatSimulation = Convert.ToBoolean(chkChatSimulation.Checked);
				
				getdata.ChatUser1Credentials = Convert.ToInt32(cbChatUser1.Value);
				getdata.ChatUser2Credentials = Convert.ToInt32(cbChatUser2.Value);
				getdata.STScanExtendedStats = Convert.ToBoolean(chkStScanExtendedStats.Checked);
				getdata.STExtendedStatsPort = int.Parse(txtExSTPortNo.Text);
					
				getdata.WsScanMeetingServer=Convert.ToBoolean(chkWsMeetingServerStats.Checked);
				getdata.WsMeetingServerRequireSSL=Convert.ToBoolean(chkWsMeetingRequireSSL.Checked);
				getdata.WsMeetingPort =int.Parse(txtWSMeetingPort.Text);
				getdata.WsMeetingServerHost=txtWsMeetingHostName.Text;

				getdata.WsScanMediaServer=Convert.ToBoolean(chkWsMediaServerStats.Checked);
				getdata.WsMediaServerRequireSSL=Convert.ToBoolean(chkWsMediaRequireSSL.Checked);
				getdata.WsMediaPort =int.Parse(txtWsMediaPort.Text);
				getdata.WsMediaServerHost=txtWsMediaHostname.Text;


				return getdata;
			}
			catch (Exception ex)
			{
				//6/27/2014 NS added for VSPLUS-634
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}


		}
		public void filldetails(int id)
		{
			SametimeServers sametimeObject = new SametimeServers();
			SametimeServers returnsametimeObject = new SametimeServers();
			////DataTable sametime = (DataTable)VSWebBL.ConfiguratorBL.SametimeServerBL.Ins.getdatawithId(sametimeObject);

			sametimeObject.ID = id;
			Session["severid"] = sametimeObject.ID.ToString();

			//  object  returnsametimeObject1 = VSWebBL.ConfiguratorBL.SametimeServerBL.Ins.UpdateSametime(sametimeObject);
			returnsametimeObject = VSWebBL.ConfiguratorBL.SametimeServerBL.Ins.getdatawithId(sametimeObject);

			if (returnsametimeObject.Category == "")
			{

				//sametimeserver properties
				ServerNameTextBox.Text = returnsametimeObject.Name.ToString();
				IPAddressTextBox.Text = returnsametimeObject.IPAddress.ToString();
				lblServerID.Text = returnsametimeObject.SID.ToString();
				DescriptionTextBox.Text = returnsametimeObject.Description.ToString();
				CategoryTextBox.Text = returnsametimeObject.Category.ToString();
				EnabledForScanningCheckBox.Checked = returnsametimeObject.Enabled;
				//ThisserverrequiresSSLCheckBox.Checked=returnsametimeObject.
				LocationTextBox.Text = returnsametimeObject.Location.ToString();
				ScanIntervalTextBox.Text = returnsametimeObject.ScanInterval.ToString();
				OffHoursScanIntervalTextBox.Text = returnsametimeObject.OffHoursScanInterval.ToString();
				RetryIntervalTextBox.Text = returnsametimeObject.RetryInterval.ToString();
				OffHoursScanIntervalTextBox.Text = returnsametimeObject.OffHoursScanInterval.ToString();
				ResponseThresholdTextBox.Text = returnsametimeObject.ResponseThreshold.ToString();
				SrvAtrFailBefAlertTextBox.Text = returnsametimeObject.FailureThreshold.ToString();

				//if (returnsametimeObject.ChatThreshold.ToString() != "")
				//{
				//    ChatSessionsCheckBox.Checked = false;
				//    ChatSessionsTextBox.Enabled = false;
				//}
				//else
				//{
				//    ChatSessionsCheckBox.Checked = true;
				//    ChatSessionsTextBox.Enabled = true;
				//    ChatSessionsTextBox.Text = returnsametimeObject.ChatThreshold.ToString();
				//}

				//if (returnsametimeObject.NChatThreshold.ToString() != "")
				//{
				//    nWayChatSessionsCheckBox.Checked = false;
				//    nWayChatSessionsTextBox.Enabled = false;
				//}
				//else
				//{
				//    nWayChatSessionsCheckBox.Checked = true;
				//    nWayChatSessionsTextBox.Enabled = true;
				//    nWayChatSessionsTextBox.Text = returnsametimeObject.NChatThreshold.ToString();
				//}
				//if (returnsametimeObject.PlacesThreshold.ToString() != "")
				//{
				//    PlacesCheckBox.Checked = false;
				//    PlacesTextBox.Enabled = false;
				//}
				//else
				//{
				//    PlacesCheckBox.Checked = true;
				//    PlacesTextBox.Enabled = true;
				//    PlacesTextBox.Text = returnsametimeObject.PlacesThreshold.ToString();
				//}
				//if (returnsametimeObject.UserThreshold.ToString() != "")
				//{
				//    UsersCheckBox.Checked = false;
				//    UsersTextBox.Enabled = false;
				//}
				//else
				//{
				//    UsersCheckBox.Checked = true;
				//    UsersTextBox.Enabled = true;
				//    UsersTextBox.Text = returnsametimeObject.UserThreshold.ToString();
				//}

				try
				{
					AwarenessCheckBox.Checked = Convert.ToBoolean(returnsametimeObject.srvawareness);
					DirectoryCheckBox.Checked = Convert.ToBoolean(returnsametimeObject.srvdirectory);
					StorageCheckBox.Checked = Convert.ToBoolean(returnsametimeObject.srvstorage);
					BuddyListCheckBox.Checked = Convert.ToBoolean(returnsametimeObject.srvbuddylist);
					PlaceCheckBox.Checked = Convert.ToBoolean(returnsametimeObject.srvplace);
					LookUpCheckBox.Checked = Convert.ToBoolean(returnsametimeObject.srvlookup);
					TestchatCheckBox.Checked = Convert.ToBoolean(returnsametimeObject.srvtestchat);
					TestmeetingCheckBox.Checked = Convert.ToBoolean(returnsametimeObject.srvtestmeeting);
					QueryDirectoryCheckBox.Checked = Convert.ToBoolean(returnsametimeObject.srvquery);
					//ProxyTypeComboBox
					//ProxyTypeComboBox.DataSource = returnsametimeObject.proxytype;
					//ProxyTypeComboBox.TextField = "proxytype";
					//ProxyTypeComboBox.ValueField = "ServerID";
					//ProxyTypeComboBox.DataBind();
					DominoApplicationservervicesASPxCheckBox.Checked = Convert.ToBoolean(returnsametimeObject.nserver);
					CommunityservicesLancherCheckBox.Checked = Convert.ToBoolean(returnsametimeObject.stcommlaunch);
					CommunityServicesCheckBox.Checked = Convert.ToBoolean(returnsametimeObject.stcommunity);
					CommunityServicesConfigurationCheckBox.Checked = Convert.ToBoolean(returnsametimeObject.stconfigurationapp);
					SametimePlacesServicesCheckBox.Checked = Convert.ToBoolean(returnsametimeObject.stplaces);
					CommunityServicesMultiplexerCheckBox.Checked = Convert.ToBoolean(returnsametimeObject.stmux);
					CommunityUserServicesCheckBox.Checked = Convert.ToBoolean(returnsametimeObject.stusers);
					CommunityOnlineDirectoryServicesCheckBox.Checked = Convert.ToBoolean(returnsametimeObject.stonlinedir);
					CommunityConferenceServicesCheckBox.Checked = Convert.ToBoolean(returnsametimeObject.stconference);
					CommunityDirectoryServicesCheckBox.Checked = Convert.ToBoolean(returnsametimeObject.stdirectory);
					CommunityLoggingServicesCheckBox.Checked = Convert.ToBoolean(returnsametimeObject.stlogger);
					//SametimeLinksAppLauncherCheckBox.Checked=Convert.ToBoolean(returnsametimeObject.
					UserprivacyInformationCheckBox.Checked = Convert.ToBoolean(returnsametimeObject.stprivacy);
					SecurityServicesCheckBox.Checked = Convert.ToBoolean(returnsametimeObject.stsecurity);
					CommunityBuddyListPresenceceServicesCheckBox.Checked = Convert.ToBoolean(returnsametimeObject.stpresencemgr);
					JavaServiceManagerCheckBox.Checked = Convert.ToBoolean(returnsametimeObject.stservicemanager);
					// CommunityBuddyListsubscriptionServicesCheckBox.Checked=Convert.ToBoolean(returnsametimeObject
					EventServerCheckBox.Enabled = Convert.ToBoolean(returnsametimeObject.steventserver);
					SametimepolicyServicesCheckBox.Enabled = Convert.ToBoolean(returnsametimeObject.stpolicy);
					ConfigurationBridgeCheckBox.Enabled = Convert.ToBoolean(returnsametimeObject.stconfigurationbridge);
					SametimeAdminCheckBox.Checked = Convert.ToBoolean(returnsametimeObject.stadminsrv);
					// UserconnectListandPrefsCheckBox.Checked=Convert.ToBoolean(returnsametimeObject.
					CommunityChatLoggingServicesCheckBox.Checked = Convert.ToBoolean(returnsametimeObject.stchatlogging);
					CommunitypollingServicesCheckBox.Checked = Convert.ToBoolean(returnsametimeObject.stpolling);
					UsernameResolutionCheckBox.Checked = Convert.ToBoolean(returnsametimeObject.stresolve);
					// CommunityBuddyListBackwordCompatabilityCheckBox.Checked=Convert.ToBoolean(returnsametimeObject.
					UserconnectListandPrefsCheckBox.Checked = Convert.ToBoolean(returnsametimeObject.stuserstorage);
					CommunityBuddyListBackwordCompatabilityCheckBox.Checked = Convert.ToBoolean(returnsametimeObject.stpresencecompatmgr);
					ThisserverrequiresSSLCheckBox.Checked = Convert.ToBoolean(returnsametimeObject.SSL);
					CommunityConferenceServicesCheckBox.Checked = Convert.ToBoolean(returnsametimeObject.stconference);

					SrvAtrFailBefAlertTextBox.Text = returnsametimeObject.FailureThreshold.ToString();

					cbChatUser1.SelectedItem = cbChatUser1.Items.FindByValue(Convert.ToInt32(returnsametimeObject.ChatUser1Credentials));
					for (int i = 0; i < cbChatUser1.Items.Count; i++)
					{
						if (cbChatUser1.Items[i].Value.ToString() == returnsametimeObject.ChatUser1Credentials.ToString())
							cbChatUser1.Items[i].Selected = true;
					}
                   
					cbChatUser2.SelectedItem = cbChatUser2.Items.FindByValue(Convert.ToInt32(returnsametimeObject.ChatUser2Credentials));
					for (int i = 0; i < cbChatUser2.Items.Count; i++)
					{
						if (cbChatUser2.Items[i].Value.ToString() == returnsametimeObject.ChatUser2Credentials.ToString())
							cbChatUser2.Items[i].Selected = true;
					}
                   
					chkStScanExtendedStats.Checked = Convert.ToBoolean(returnsametimeObject.STScanExtendedStats);
					txtExSTPortNo.Text = returnsametimeObject.STExtendedStatsPort.ToString();

					chkChatSimulation.Checked = Convert.ToBoolean(returnsametimeObject.TestChatSimulation);
					chkWsMeetingServerStats.Checked = Convert.ToBoolean(returnsametimeObject.WsScanMeetingServer);
					chkWsMeetingRequireSSL.Checked = Convert.ToBoolean(returnsametimeObject.WsMeetingServerRequireSSL);
					txtWsMeetingHostName.Text = returnsametimeObject.WsMeetingServerHost.ToString();
					txtWSMeetingPort.Text = returnsametimeObject.WsMeetingPort.ToString();

					chkWsMediaServerStats.Checked = Convert.ToBoolean(returnsametimeObject.WsScanMediaServer);
					chkWsMediaRequireSSL.Checked = Convert.ToBoolean(returnsametimeObject.WsMediaServerRequireSSL);
					txtWsMediaHostname.Text = returnsametimeObject.WsMediaServerHost.ToString();
					txtWsMediaPort.Text = returnsametimeObject.WsMediaPort.ToString();

					Session["ReturnUrl"] = "SametimeServer.aspx?ID=" + lblServerID.Text + "&tab=1";

				}
				catch (Exception ex)
				{
					//6/27/2014 NS added for VSPLUS-634
					Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
					throw ex;
				}
				RetryIntervalTextBox.Text = "2";
				OffHoursScanIntervalTextBox.Text = "30";
				ScanIntervalTextBox.Text = "8";
				ResponseThresholdTextBox.Text = "250";
				//1/7/2014 NS commented out
				/*
				PlacesCheckBox.Checked = true;
				UsersCheckBox.Checked = true;
				ChatSessionsCheckBox.Checked = true;
				nWayChatSessionsCheckBox.Checked = true;
				ChatSessionsTextBox.Text = "200";
				PlacesTextBox.Text = "200";
				nWayChatSessionsTextBox.Text = "200";
				 */
				CategoryTextBox.Text = "Production";
				EnabledForScanningCheckBox.Checked = true;

			}

			else
			{

				QueryDirectoryCheckBox.Checked = Convert.ToBoolean(returnsametimeObject.srvquery);
				AwarenessCheckBox.Checked = Convert.ToBoolean(returnsametimeObject.srvawareness);
				DirectoryCheckBox.Checked = Convert.ToBoolean(returnsametimeObject.srvdirectory);
				StorageCheckBox.Checked = Convert.ToBoolean(returnsametimeObject.srvstorage);
				BuddyListCheckBox.Checked = Convert.ToBoolean(returnsametimeObject.srvbuddylist);
				PlaceCheckBox.Checked = Convert.ToBoolean(returnsametimeObject.srvplace);
				LookUpCheckBox.Checked = Convert.ToBoolean(returnsametimeObject.srvlookup);
				TestchatCheckBox.Checked = Convert.ToBoolean(returnsametimeObject.srvtestchat);
				TestmeetingCheckBox.Checked = Convert.ToBoolean(returnsametimeObject.srvtestmeeting);
				QueryDirectoryCheckBox.Checked = Convert.ToBoolean(returnsametimeObject.srvquery);
				HostNameTextBox.Text = returnsametimeObject.db2hostname.ToString();
				//LoginTextBox.Text = returnsametimeObject.db2login.ToString();
				//PasswordTextBox.Text = returnsametimeObject.db2password.ToString();
				CredentialComboBox.Value = returnsametimeObject.CredentialID.ToString();
				DataBaseNameTextBox.Text = returnsametimeObject.db2databasename.ToString();
				PortTextBox.Text = returnsametimeObject.db2port.ToString();

				DB2PortTextBox.Text = returnsametimeObject.db2port.ToString();
				PortCheckBox.Checked = Convert.ToBoolean(returnsametimeObject.enabledb2port);
				////DataTable sametime = (DataTable)returnsametimeObject;
				//ProxyTypeComboBox.DataSource = VSWebBL.ConfiguratorBL.SametimeServerBL.Ins.getdatawithId(sametimeObject);
				//    //getdatasametimedatatable;
				//ProxyTypeComboBox.TextField = "proxytype";
				//ProxyTypeComboBox.ValueField = "ID";
				//ProxyTypeComboBox.DataBind();
				ProxyTypeComboBox.Text = returnsametimeObject.proxytype.ToString();
				string radiovalue = returnsametimeObject.Platform.ToString();
				if (radiovalue == "WebSphere")
				{
					SortRadioButtonList1.SelectedIndex = 1;
					ASPxRoundPanel7.Visible = true;
                  //  ASPxPageControl1.TabPages[1].Visible = true;
					ASPxPageControl1.TabPages[1].ClientVisible = true;

					chkWsMeetingServerStats.Visible = true;
					txtWsMeetingHostName.Visible = true;
					lblWsMeetingHostName.Visible = true;
					txtWSMeetingPort.Visible = true;
					lblWSMeetingPort.Visible = true;
					chkWsMeetingRequireSSL.Visible = true;
					lblWsMeetingRequireSSL.Visible = true;

					chkWsMediaServerStats.Visible = true;
					txtWsMediaHostname.Visible = true;
					lblWsMediaHostname.Visible = true;
					txtWsMediaPort.Visible = true;
					lblWsMediaPort.Visible = true;
					chkWsMediaRequireSSL.Visible = true;
					lblWsMediaRequireSSL.Visible = true;

				}
				if (radiovalue == "Domino")
				{
					ASPxPageControl1.TabPages[1].ClientVisible = false;
					//ASPxPageControl1.TabPages[1].Visible = false;
					SortRadioButtonList1.SelectedIndex = 0;
					//SortRadioButtonList1.SelectedItem.Selected = true;

					chkWsMeetingServerStats.Visible = false;
					txtWsMeetingHostName.Visible = false;
					lblWsMeetingHostName.Visible = false;
					txtWSMeetingPort.Visible = false;
					lblWSMeetingPort.Visible = false;
					chkWsMeetingRequireSSL.Visible = false;
					lblWsMeetingRequireSSL.Visible = false;

					chkWsMediaServerStats.Visible = false;
					txtWsMediaHostname.Visible = false;
					lblWsMediaHostname.Visible = false;
					txtWsMediaPort.Visible = false;
					lblWsMediaPort.Visible = false;
					chkWsMediaRequireSSL.Visible = false;
					lblWsMediaRequireSSL.Visible = false;

				}


				ProtocolComboBox.Text = returnsametimeObject.proxyprotocol.ToString();
				//sametimeserver properties
				ServerNameTextBox.Text = returnsametimeObject.Name.ToString();
				IPAddressTextBox.Text = returnsametimeObject.IPAddress.ToString();
				lblServerID.Text = returnsametimeObject.SID.ToString();
				DescriptionTextBox.Text = returnsametimeObject.Description.ToString();
				CategoryTextBox.Text = returnsametimeObject.Category.ToString();
				EnabledForScanningCheckBox.Checked = returnsametimeObject.Enabled;
				//ThisserverrequiresSSLCheckBox.Checked=returnsametimeObject.
				LocationTextBox.Text = returnsametimeObject.Location.ToString();
				ScanIntervalTextBox.Text = returnsametimeObject.ScanInterval.ToString();
				OffHoursScanIntervalTextBox.Text = returnsametimeObject.OffHoursScanInterval.ToString();
				RetryIntervalTextBox.Text = returnsametimeObject.RetryInterval.ToString();
				OffHoursScanIntervalTextBox.Text = returnsametimeObject.OffHoursScanInterval.ToString();
				ResponseThresholdTextBox.Text = returnsametimeObject.ResponseThreshold.ToString();
				SrvAtrFailBefAlertTextBox.Text = returnsametimeObject.FailureThreshold.ToString();

				cbChatUser1.SelectedItem = cbChatUser1.Items.FindByValue( Convert.ToInt32(returnsametimeObject.ChatUser1Credentials));
				for (int i = 0; i < cbChatUser1.Items.Count; i++)
				{
					if (cbChatUser1.Items[i].Value.ToString() == returnsametimeObject.ChatUser1Credentials.ToString())
						cbChatUser1.Items[i].Selected = true;
				}

				cbChatUser2.SelectedItem = cbChatUser2.Items.FindByValue(Convert.ToInt32(returnsametimeObject.ChatUser2Credentials));
				for (int i = 0; i < cbChatUser2.Items.Count; i++)
				{
					if (cbChatUser2.Items[i].Value.ToString() == returnsametimeObject.ChatUser2Credentials.ToString())
						cbChatUser2.Items[i].Selected = true;
				}
				chkChatSimulation.Checked = Convert.ToBoolean(returnsametimeObject.TestChatSimulation);
				chkStScanExtendedStats.Checked = Convert.ToBoolean(returnsametimeObject.STScanExtendedStats);
				txtExSTPortNo.Text = returnsametimeObject.STExtendedStatsPort.ToString();
				chkWsMeetingServerStats.Checked = Convert.ToBoolean(returnsametimeObject.WsScanMeetingServer);
				chkWsMeetingRequireSSL.Checked = Convert.ToBoolean(returnsametimeObject.WsMeetingServerRequireSSL);
				txtWsMeetingHostName.Text = returnsametimeObject.WsMeetingServerHost.ToString();
				txtWSMeetingPort.Text = returnsametimeObject.WsMeetingPort.ToString();

				chkWsMediaServerStats.Checked = Convert.ToBoolean(returnsametimeObject.WsScanMediaServer);
				chkWsMediaRequireSSL.Checked = Convert.ToBoolean(returnsametimeObject.WsMediaServerRequireSSL);
				txtWsMediaHostname.Text = returnsametimeObject.WsMediaServerHost.ToString();
				txtWsMediaPort.Text = returnsametimeObject.WsMediaPort.ToString();
				//1/7/2014 NS commented out
				/*
				if (returnsametimeObject.ChatThreshold.ToString() == "")
				{
					ChatSessionsCheckBox.Checked = false;
					ChatSessionsTextBox.Enabled = false;
				}
				else
				{
					ChatSessionsCheckBox.Checked = true;
					ChatSessionsTextBox.Enabled = true;
					ChatSessionsTextBox.Text = returnsametimeObject.ChatThreshold.ToString();
				}

				if (returnsametimeObject.NChatThreshold.ToString() == "")
				{
					nWayChatSessionsCheckBox.Checked = false;
					nWayChatSessionsTextBox.Enabled = false;
				}
				else
				{
					nWayChatSessionsCheckBox.Checked = true;
					nWayChatSessionsTextBox.Enabled = true;
					nWayChatSessionsTextBox.Text = returnsametimeObject.NChatThreshold.ToString();
				}
				if (returnsametimeObject.PlacesThreshold.ToString() == "")
				{
					PlacesCheckBox.Checked = false;
					PlacesTextBox.Enabled = false;
				}
				else
				{
					PlacesCheckBox.Checked = true;
					PlacesTextBox.Enabled = true;
					PlacesTextBox.Text = returnsametimeObject.PlacesThreshold.ToString();
				}
				if (returnsametimeObject.UserThreshold.ToString() == "")
				{
					UsersCheckBox.Checked = false;
					UsersTextBox.Enabled = false;
				}
				else
				{
					UsersCheckBox.Checked = true;
					UsersTextBox.Enabled = true;
					UsersTextBox.Text = returnsametimeObject.UserThreshold.ToString();
				}
				*/
				try
				{
					DominoApplicationservervicesASPxCheckBox.Checked = Convert.ToBoolean(returnsametimeObject.nserver);
					CommunityservicesLancherCheckBox.Checked = Convert.ToBoolean(returnsametimeObject.stcommlaunch);
					CommunityServicesCheckBox.Checked = Convert.ToBoolean(returnsametimeObject.stcommunity);
					CommunityServicesConfigurationCheckBox.Checked = Convert.ToBoolean(returnsametimeObject.stconfigurationapp);
					SametimePlacesServicesCheckBox.Checked = Convert.ToBoolean(returnsametimeObject.stplaces);
					CommunityServicesMultiplexerCheckBox.Checked = Convert.ToBoolean(returnsametimeObject.stmux);
					CommunityUserServicesCheckBox.Checked = Convert.ToBoolean(returnsametimeObject.stusers);
					CommunityOnlineDirectoryServicesCheckBox.Checked = Convert.ToBoolean(returnsametimeObject.stonlinedir);
					CommunityConferenceServicesCheckBox.Checked = Convert.ToBoolean(returnsametimeObject.stconference);
					CommunityDirectoryServicesCheckBox.Checked = Convert.ToBoolean(returnsametimeObject.stdirectory);
					CommunityLoggingServicesCheckBox.Checked = Convert.ToBoolean(returnsametimeObject.stlogger);
					//SametimeLinksAppLauncherCheckBox.Checked=Convert.ToBoolean(returnsametimeObject.
					UserprivacyInformationCheckBox.Checked = Convert.ToBoolean(returnsametimeObject.stprivacy);
					SecurityServicesCheckBox.Checked = Convert.ToBoolean(returnsametimeObject.stsecurity);
					CommunityBuddyListPresenceceServicesCheckBox.Checked = Convert.ToBoolean(returnsametimeObject.stpresencemgr);
					JavaServiceManagerCheckBox.Checked = Convert.ToBoolean(returnsametimeObject.stservicemanager);
					// CommunityBuddyListsubscriptionServicesCheckBox.Checked=Convert.ToBoolean(returnsametimeObject
					EventServerCheckBox.Enabled = Convert.ToBoolean(returnsametimeObject.steventserver);
					SametimepolicyServicesCheckBox.Enabled = Convert.ToBoolean(returnsametimeObject.stpolicy);
					ConfigurationBridgeCheckBox.Enabled = Convert.ToBoolean(returnsametimeObject.stconfigurationbridge);
					SametimeAdminCheckBox.Checked = Convert.ToBoolean(returnsametimeObject.stadminsrv);
					// UserconnectListandPrefsCheckBox.Checked=Convert.ToBoolean(returnsametimeObject.
					CommunityChatLoggingServicesCheckBox.Checked = Convert.ToBoolean(returnsametimeObject.stchatlogging);
					CommunitypollingServicesCheckBox.Checked = Convert.ToBoolean(returnsametimeObject.stpolling);
					UsernameResolutionCheckBox.Checked = Convert.ToBoolean(returnsametimeObject.stresolve);
					// CommunityBuddyListBackwordCompatabilityCheckBox.Checked=Convert.ToBoolean(returnsametimeObject.
					UserconnectListandPrefsCheckBox.Checked = Convert.ToBoolean(returnsametimeObject.stuserstorage);
					CommunityBuddyListBackwordCompatabilityCheckBox.Checked = Convert.ToBoolean(returnsametimeObject.stpresencecompatmgr);
					ThisserverrequiresSSLCheckBox.Checked = Convert.ToBoolean(returnsametimeObject.SSL);
					CommunityConferenceServicesCheckBox.Checked = Convert.ToBoolean(returnsametimeObject.stconference);

					Session["ReturnUrl"] = "SametimeServer.aspx?ID=" + lblServerID.Text + "&tab=1";
				}
				catch (Exception ex)
				{
					//6/27/2014 NS added for VSPLUS-634
					Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
					throw ex;
				}

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


		#region ButtonCode



		//public Control getControlFromName(ref object containerObj, string name)
		//{
		//    try
		//    {
		//        Control tempCtrl = default(Control);
		//        foreach (tempCtrl in containerObj.Controls) 
		//        {
		//            if (tempCtrl.Name.ToUpper.Trim == name.ToUpper().Trim()) 
		//            {
		//                return tempCtrl;
		//            }
		//        }
		//    } 
		//    catch (Exception ex) 
		//    {
		//        return null;
		//    }
		//}



		//    private void ParseSametimeXML(string xmlString)
		//   {
		//    xmlSametime = xmlString;

		//    StringBuilder output = new StringBuilder();
		//    //  Dim xmlString As String = xmlSametime
		//    XmlDocument m_xmld = default(XmlDocument);
		//    XmlNodeList m_nodelist = default(XmlNodeList);
		//    XmlNode m_node = default(XmlNode);
		//    XmlNode child_node = default(XmlNode);

		//    try
		//    {
		//        //Create the XML Document

		//        m_xmld = new XmlDocument();
		//        //Load the Xml file
		//        m_xmld.Load(new StringReader(xmlString));
		//        //Get the list of name nodes 


		//        //EnumeratedProcesses
		//        m_nodelist = m_xmld.SelectNodes("/SametimeStatistics/EnumeratedProcesses");
		//    //Loop through the nodes

		//    } 
		//    catch (Exception ex) 
		//    {
		//        throw ex;
		//    }


		//    string myLabelName = null;

		//    if ( m_nodelist != null) 
		//    {
		//        foreach ( XmlNode  m_node1 in m_nodelist)
		//        {
		//            try 
		//            {
		//                if (m_node1.HasChildNodes) 
		//                {
		//                    foreach (XmlNode child_node1 in m_node1)
		//                    {
		//                        // TextBoxX1.Text += child_node.Name & ": " & child_node.InnerText & vbCrLf
		//                        myLabelName = child_node1.InnerText;
		//                        myLabelName = myLabelName.Replace(".exe", "");
		//                        myLabelName = myLabelName.Replace(".EXE", "");
		//                        try 
		//                        {
		//                            Debug.Print(myLabelName);
		//                            Label tempCtrl = (Label)getControlFromName(      ,"lbl" + myLabelName);
		//                            if ((tempCtrl != null)) 
		//                            {
		//                                tempCtrl.Text = "Running";
		//                                tempCtrl.BackColor = Color.LightGreen;
		//                                tempCtrl.ForeColor = System.Drawing.Color.Black;
		//                            }

		//                        } 
		//                        catch (Exception ex) 
		//                        {
		//                            throw ex;
		//                        }
		//                    }
		//                }
		//            } 
		//            catch (Exception ex) 
		//            {
		//                throw ex;
		//                // WriteDeviceHistoryEntry("Sametime", mySametimeServer.Name, Now.ToString & "  Error Parsing Sametime XML statistics at #3: " & ex.ToString)
		//            }
		//        }
		//    //Clear it out for next time so we don't continually parse the same information
		//    }
		//    else
		//    {
		//        //MessageBox.Show("Could not connect to the Sametime servlet. This could be because you have not registered a Sametime username/password, or because the SSL setting is incorrect.", "Servlet Error", MessageBoxButtons.OK);
		//    }
		//   xmlSametime = "";
		//  }

		#endregion


		public void UpdateSametimeServer()
		{
			SametimeServers sts = new SametimeServers();

			Object Updatedetails = VSWebBL.ConfiguratorBL.SametimeServerBL.Ins.UpdateSametime(GetdataForSametimeServer());
			SetFocusOnError(Updatedetails);
			if (Updatedetails.ToString() == "True")
			{
				//1/7/2014 NS modified
				/*
			  ErrorMessageLabel1.Text = "Data updated successfully.";
			  ErrorMessagePopupControl.HeaderText = "Information";
			  ErrorMessagePopupControl.ShowCloseButton = false;
			  ValidationUpdatedButton.Visible = true;
			  ValidationOkButton.Visible = false;
			  ErrorMessagePopupControl.ShowOnPageLoad = true;
				*/
				//2/20/2014 NS modified to redirect to the server grid page
				Session["SametimeUpdateStatus"] = ServerNameTextBox.Text;
				Response.Redirect("LotusSametimeGrid.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
				Context.ApplicationInstance.CompleteRequest();
				//successDiv.Style.Value = "display: block";
				//successDiv.InnerHtml = "Sametime information updated successfully.";
				//errorDiv.Style.Value = "display: none";
			}
		}

		//private void SetFocusOnError(Object ReturnValue)
		//{
		//    string ErrorMessage = ReturnValue.ToString();
		//    if (ErrorMessage.Substring(0, 2) == "ER")
		//    {
		//        ErrorMessageLabel.Text = ErrorMessage.Substring(3);
		//        ErrorMessagePopupControl.ShowOnPageLoad = true;

		//    }
		//}



		public void InsertdetailsSametimeServer()
		{
			//See if this IP address is already used

			SametimeServers sametimeobj = new SametimeServers();
			sametimeobj.IPAddress = IPAddressTextBox.Text;
			DataTable returntab = VSWebBL.ConfiguratorBL.SametimeServerBL.Ins.GetIPAddress(sametimeobj);
			if (returntab.Rows.Count > 0)
			{
				//1/7/2014 NS modified
				//ErrorMessageLabel1.Text = "This IP Address is already being monitored.  Please enter a different IP Address.";
				//ErrorMessagePopupControl.ShowOnPageLoad = true;
				successDiv.Style.Value = "display: none";
				errorDiv.Style.Value = "display: block";
				//10/6/2014 NS modified for VSPLUS-990
				errorDiv.InnerHtml = "The IP address you entered is already being monitored. Please enter a different IP Address." +
					"<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
				flag = true;
			}
			else
			{
				object insert = VSWebBL.ConfiguratorBL.SametimeServerBL.Ins.insertdetail(GetdataForSametimeServer());
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

		protected void CancelButton_Click(object sender, EventArgs e)
		{
			Response.Redirect("LotusSametimeGrid.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
			Context.ApplicationInstance.CompleteRequest();

		}

		protected void ValidationUpdatedButton_Click(object sender, EventArgs e)
		{
			Response.Redirect("LotusSametimeGrid.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
			Context.ApplicationInstance.CompleteRequest();
		}

		protected void ChatSessionsCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			//1/7/2014 NS commented out
			/*
			if (ChatSessionsCheckBox.Checked == true)
			{

				ChatSessionsTextBox.Enabled = true;
				ChatSessionsTextBox.Text = "";
				ChatSessionsTextBox.Focus();
			}
			else
			{

				ChatSessionsTextBox.Enabled = false;
				ChatSessionsTextBox.Text = "0";
			}
			 */
		}

		protected void nWayChatSessionsCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			//1/7/2014 NS commented out
			/*
			if (nWayChatSessionsCheckBox.Checked == true)
			{

				nWayChatSessionsTextBox.Enabled = true;
				nWayChatSessionsTextBox.Text = "";
				nWayChatSessionsTextBox.Focus();
			}
			else
			{
				nWayChatSessionsTextBox.Enabled = false;
				nWayChatSessionsTextBox.Text = "0";
			}
			 * */
		}

		protected void PlacesCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			//1/7/2014 NS commented out
			/*
			if (PlacesCheckBox.Checked == true)
			{

				PlacesTextBox.Enabled = true;
				PlacesTextBox.Text = "";
				PlacesTextBox.Focus();
			}
			else
			{
				PlacesTextBox.Enabled = false;
				PlacesTextBox.Text = "0";
			}
			 */
		}

		protected void UsersCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			//1/7/2014 NS commented out
			/*
			if (UsersCheckBox.Checked == true)
			{
				UsersTextBox.Enabled = true;
				UsersTextBox.Text = "";
				UsersTextBox.Focus();
			}
			else
			{
                
				UsersTextBox.Enabled = false;
				UsersTextBox.Text = "0";
			}
			 */
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
		private Status CollectDataforStatus()
		{
			Status St = new Status();
			try
			{
				St.Category = CategoryTextBox.Text;

				//St.DeadMail = 0;
				St.Description = DescriptionTextBox.Text;
				// St.Details = "";
				St.DownCount = 0;
				//St.Location = "Cluster";
				St.Name = ServerNameTextBox.Text;
				//St.MailDetails = "Mail Details";
				//St.PendingMail = 0;
				St.sStatus = "Not Scanned";
				St.Type = "Network Device";
				St.Upcount = 0;
				St.UpPercent = 1;
				St.LastUpdate = System.DateTime.Now;
				St.ResponseTime = 0;
				St.TypeANDName = ServerNameTextBox.Text + "-Sametime";
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
					//1/7/2014 NS modified
					/*
					ErrorMessageLabel.Text = "Status information updated successfully.";
					ErrorMessagePopupControl.HeaderText = "Information";
					ErrorMessagePopupControl.ShowCloseButton = false;
					ValidationUpdatedButton.Visible = true;
					ValidationOkButton.Visible = false;
					ErrorMessagePopupControl.ShowOnPageLoad = true;
					*/
					successDiv.Style.Value = "display: block";
					errorDiv.Style.Value = "display: none";
				}

			}
			catch (Exception ex)
			{
				//1/7/2014 NS modified
				//ErrorMessageLabel.Text = "Error attempting to update the status table :" + ex.Message;
				//ErrorMessagePopupControl.ShowOnPageLoad = true;
				successDiv.Style.Value = "display: none";
				errorDiv.Style.Value = "display: block";
				//10/6/2014 NS modified for VSPLUS-990
				errorDiv.InnerHtml = "Error attempting to update the status table: " + ex.Message +
					"<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
				//6/27/2014 NS added for VSPLUS-634
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
			}
			finally { }
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

		private void FillMaintServersGridfromSession()
		{
			try
			{

				DataTable ServersDataTable = new DataTable();
				//4/3/2014 NS modified for VSPLUS-138
				//if(Session["MaintServers"]!=""&&Session["MaintServers"]!=null)
				//ServersDataTable = (DataTable)Session["MaintServers"];
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


		private void FillAlertGridView()
		{
			try
			{

				DataTable AlertDataTable = new DataTable();
				DataSet AlertDataSet = new DataSet();
				AlertDataTable = VSWebBL.ConfiguratorBL.AlertsBL.Ins.GetAlertTab(ServerNameTextBox.Text, "Sametime");
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

		protected void QuerySametimeServerButton_Click(object sender, EventArgs e)
		{

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

		protected void MaintWinListGridView_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
		{
			e.Row.Attributes.Add("onmouseover", "this.style.backgroundColor='#C0C0C0';");

			e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='white';");
		}

		protected void MaintWinListGridView_PageSizeChanged(object sender, EventArgs e)
		{
			//ProfilesGridView.PageIndex;
			VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("SametimeServer|MaintWinListGridView", MaintWinListGridView.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
			Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
		}
		protected void AlertGridView_PageSizeChanged(object sender, EventArgs e)
		{
			//ProfilesGridView.PageIndex;
			VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("SametimeServer|AlertGridView", AlertGridView.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
			Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
		}


		private void FillWebsphereCell(int sametimeid)
		{
			WebsphereCell sametimeObject = new WebsphereCell();
			DataTable dt = new DataTable();
			//sametimeObject.SametimeId = int.Parse(Request.QueryString["ID"]);
			sametimeObject.SametimeId = Convert.ToInt32(Session["sametimeid"].ToString());
			dt = VSWebBL.ConfiguratorBL.SametimeServerBL.Ins.Getwebspherecell(sametimeObject);

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


		public ServerAttributes CollectDataforServerAttributes(DataRow ServersRow)
		{
			ServerAttributes Mobj = new ServerAttributes();
			try
			{

				int ReturnValue = VSWebBL.SecurityBL.ServersBL.Ins.GetServerIDbyServerName(ServersRow["ServerName"].ToString());

				Mobj.ServerId = ReturnValue;

			}
			catch (Exception ex)
			{

				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}

			return Mobj;

		}





		protected void ServerTaskDefGridView_HtmlRowCreated(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
		{
			if (e.RowType == GridViewRowType.EditForm)
			{
				//Mukund: VSPLUS-844, Page redirect on callback
				try
				{

					if (e.GetValue("TaskID") != " ")
					{
						ASPxWebControl.RedirectOnCallback("EditServerTask.aspx?TaskID=" + e.GetValue("TaskID"));
						Context.ApplicationInstance.CompleteRequest();//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback

					}
					else
					{
						ASPxWebControl.RedirectOnCallback("EditServerTask.aspx");
					}
				}
				catch (Exception ex)
				{
					Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
					//throw ex;
				}
			}
		}



		protected DataRow GetRow(DataTable ServerObject, IDictionaryEnumerator enumerator, int Keys)
		{
			DataTable dataTable = (DataTable)Session["Servers"];
			DataRow DRRow = dataTable.NewRow();
			if (Keys == 0)
				DRRow = dataTable.NewRow();
			else
				DRRow = dataTable.Rows.Find(Keys);
			//IDictionaryEnumerator enumerator = e.NewValues.GetEnumerator();
			enumerator.Reset();
			while (enumerator.MoveNext())
				DRRow[enumerator.Key.ToString()] = (enumerator.Value == null ? "False" : enumerator.Value);
			return DRRow;
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
				dt = (VSWebBL.SecurityBL.ServersBL.Ins.GetServerDetailsByID(Convert.ToInt32(Request.QueryString["ID"])));
				if (dt.Rows.Count > 0)
				{
					Response.Redirect("~/Dashboard/SametimeServerDetailsPage.aspx?Name=" + dt.Rows[0]["ServerName"].ToString() + "&Type=" + dt.Rows[0]["Type"].ToString() + "&Status=" + dt.Rows[0]["Status"].ToString() + "&LastDate=" + dt.Rows[0]["LastUpdate"].ToString(), false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
					Context.ApplicationInstance.CompleteRequest();
				}
			}


		}

		//public void fillsametimeNodesTree()
		//{


		//    try
		//    {
		//        WebsphereCell sametimeObject = new WebsphereCell();
		//        DataTable dt = new DataTable();
		//        sametimeObject.SametimeId = int.Parse(Request.QueryString["ID"]);
		//        dt = VSWebBL.ConfiguratorBL.SametimeServerBL.Ins.Getwebspherecell(sametimeObject);
		//        if (dt.Rows.Count > 0)
		//        {
		//            samecellid = Convert.ToInt32(dt.Rows[0]["CellID"].ToString());
		//            Session["id"] = samecellid;

		//            SametimeNodesTreeList.CollapseAll();
		//            CollapseAllButton.Image.Url = "~/images/icons/add.png";
		//            CollapseAllButton.Text = "Expand All";


		//            DataTable bycelldata = VSWebBL.SecurityBL.webspehereImportBL.Ins.FetsametimeserversbycellID(samecellid);
		//            Session["Nodesandservers"] = bycelldata;
		//            SametimeNodesTreeList.DataSource = (DataTable)Session["Nodesandservers"];
		//            SametimeNodesTreeList.DataBind();
		//            DataTable dtselected = VSWebBL.SecurityBL.webspehereImportBL.Ins.Fetsametimeservers(samecellid);
		//            if (dtselected.Rows.Count > 0)
		//            {

		//                for (int i = 0; i < dtselected.Rows.Count; i++)
		//                {
		//                    if (Convert.ToInt32(dtselected.Rows[i]["actid"]) != 0)
		//                    {
		//                        //specific selected
		//                        TreeListNodeIterator iterator = SametimeNodesTreeList.CreateNodeIterator();
		//                        TreeListNode node;
		//                        while (true)
		//                        {

		//                            node = iterator.GetNext();
		//                            if (node == null) break;

		//                            if ((Convert.ToInt32(dtselected.Rows[i]["actid"]) == Convert.ToInt32(node.GetValue("actid")))
		//                                &&  node.GetValue("tbl").ToString() == "Servers")
		//                            {
		//                                if (node.GetValue("SrvId") != null)
		//                                {
		//                                    if (dtselected.Rows[i]["Enabled"].ToString() == "True")
		//                                    {
		//                                        node.Selected = true;
		//                                    }
											
		//                                }

		//                            }
		//                        }
		//                    }
		//                }
		//            }


		//            SametimeNodesTreeList.DataSource = (DataTable)Session["Nodesandservers"];
		//            //SametimeNodesTreeList.DataSource = bycelldata;
		//            SametimeNodesTreeList.DataBind();
		//        }

		//    }
		//    catch (Exception ex)
		//    {

		//        Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
		//    }
		//}
		public void fillsametimeNodesTree()
		{
			try
			{
				WebsphereCell sametimeObject = new WebsphereCell();
				DataTable dt = new DataTable();
				sametimeObject.SametimeId = int.Parse(Request.QueryString["ID"]);
				dt = VSWebBL.ConfiguratorBL.SametimeServerBL.Ins.Getwebspherecell(sametimeObject);
				if (dt.Rows.Count > 0)
				{
					samecellid = Convert.ToInt32(dt.Rows[0]["CellID"].ToString());
					          Session["id"] = samecellid;

					SametimeNodesTreeList.CollapseAll();
					CollapseAllButton.Image.Url = "~/images/icons/add.png";
					CollapseAllButton.Text = "Expand All";
					if (Session["DataEvents"] == null)
					{

						DataTable bycelldata = VSWebBL.SecurityBL.webspehereImportBL.Ins.FetsametimeserversbycellID(samecellid);

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


		protected void SortRadioButtonList1_SelectedIndexChanged(object sender, EventArgs e)
		{
			int domino = Convert.ToInt32(SortRadioButtonList1.SelectedItem.Value);

			if (domino == 2)
			{
				//ASPxPageControl1.TabPages[1].Enabled=true;
				//ASPxPageControl1.TabPages[].Visible = true;
				ASPxPageControl1.TabPages[1].ClientVisible = true;

				chkWsMeetingServerStats.Visible = true;
				txtWsMeetingHostName.Visible = true;
				lblWsMeetingHostName.Visible = true;
				txtWSMeetingPort.Visible = true;
				lblWSMeetingPort.Visible = true;
				chkWsMeetingRequireSSL.Visible = true;
				lblWsMeetingRequireSSL.Visible = true;

				chkWsMediaServerStats.Visible = true;
				txtWsMediaHostname.Visible = true;
				lblWsMediaHostname.Visible = true;
				txtWsMediaPort.Visible = true;
				lblWsMediaPort.Visible = true;
				chkWsMediaRequireSSL.Visible = true;
				lblWsMediaRequireSSL.Visible = true;
			}
			else
			{
				//ASPxPageControl1.TabPages[1].Enabled = false;
				//ASPxPageControl1.TabPages[1].Visible = false;
				ASPxPageControl1.TabPages[1].ClientVisible = false;

				chkWsMeetingServerStats.Visible =false;
				txtWsMeetingHostName.Visible = false;
				lblWsMeetingHostName.Visible = false;
				txtWSMeetingPort.Visible = false;
				lblWSMeetingPort.Visible = false;
				chkWsMeetingRequireSSL.Visible = false;
				lblWsMeetingRequireSSL.Visible = false;

				chkWsMediaServerStats.Visible = false;
				txtWsMediaHostname.Visible = false;
				lblWsMediaHostname.Visible = false;
				txtWsMediaPort.Visible=false;
				lblWsMediaPort.Visible = false;
				chkWsMediaRequireSSL.Visible = false;
				lblWsMediaRequireSSL.Visible = false;


				
			}

		}
		public void Insertdata()
		{
			bool ReturnValue;
			WebsphereCell webspehereObject = new WebsphereCell();
			DataTable dt = new DataTable();
			webspehereObject.SametimeId = int.Parse(Request.QueryString["ID"]);
			dt = VSWebBL.ConfiguratorBL.SametimeServerBL.Ins.Getwebspherecell(webspehereObject);
			if (dt.Rows.Count > 0)
			{
				webspehereObject.CellID = Convert.ToInt32(dt.Rows[0]["CellID"].ToString());
				Session["webspherecellid"] = Convert.ToInt32(dt.Rows[0]["CellID"].ToString());

			}
			webspehereObject.Name = CellnameTextBox.Text;
			webspehereObject.ConnectionType = ConnectionComboBox.Text;
			//if (ConnectionComboBox.SelectedItem.Text == "SOAP")
			//{
			//    porttextbox.Text = "8879";
			//}
			//else
			//{
			//    porttextbox.Text = "9809";
			//}
			webspehereObject.GlobalSecurity = chbx.Checked;
			webspehereObject.HostName = HostName.Text;
			if (txtport.Text != null && txtport.Text != "")
			{
				webspehereObject.PortNo = Convert.ToInt32(txtport.Text);
			}
			if (WebCredentialsComboBox.Text != null && WebCredentialsComboBox.Text != "")
			{
				webspehereObject.CredentialsID = Convert.ToInt32(WebCredentialsComboBox.Value);
			}
			webspehereObject.Realm = realmtxtbx.Text;

			ReturnValue = VSWebBL.ConfiguratorBL.SametimeServerBL.Ins.InsertData(webspehereObject, key);
			if (ReturnValue == true)
			{
				//successDiv.Style.Value = "display: block";
				//Response.Redirect("LotusSametimeGrid.aspx", false);
				//Context.ApplicationInstance.CompleteRequest();
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
		private void FillCredentialsComboBox()
		{
			DataTable CredentialsDataTable = VSWebBL.ConfiguratorBL.ServicesBL.Ins.GetCredentials();
			WebCredentialsComboBox.DataSource = CredentialsDataTable;
			WebCredentialsComboBox.TextField = "AliasName";
			WebCredentialsComboBox.ValueField = "ID";
			WebCredentialsComboBox.DataBind();

			DataTable STCredentialsDataTable = VSWebBL.ConfiguratorBL.ServicesBL.Ins.GetSametimeCredentials();
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

				check = VSWebBL.SecurityBL.webspehereImportBL.Ins.Insertpwd(AliasName.Text, UserID.Text, encryptedPassAsString, 3);
			//	Response.Redirect("~/Configurator/SametimeServer.aspx?ID="+Convert.ToInt32(Session["sametimeid"].ToString())+"&tabs=1");
                CopyProfilePopupControl.ShowOnPageLoad = false;
                FillSametimeCredentialComboBox();
                FillCredentialsComboBox();
            }

		}
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
			creds.ID = Convert.ToInt32(WebCredentialsComboBox.Value.ToString());

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
					WebsphereCell sametimeObject = new WebsphereCell();
				DataTable dt1 = new DataTable();
				sametimeObject.SametimeId = int.Parse(Request.QueryString["ID"]);
				dt1 = VSWebBL.ConfiguratorBL.SametimeServerBL.Ins.Getwebspherecell(sametimeObject);
				if (dt1.Rows.Count > 0)
				{
					samecellid = Convert.ToInt32(dt1.Rows[0]["CellID"].ToString());
					
					
				}
					key = int.Parse(Request.QueryString["ID"]);
					ReturnValue = VSWebBL.SecurityBL.webspehereImportBL.Ins.InsertwebsphereSametimenodesandservers(cell, samecellid, key);

				} 


			}
			catch (Exception ex)
			{
				errorDivForImportingWS.Style.Value = "display: block";
				errorDivForImportingWS.InnerHtml = "An error occurred. " + ex.Message;
				return;
			}


			//checks to see if a connection was successfully made
			//cells should never be null, it should hti the return before it is null and hits this spot
			if (cells != null && cells.Connection_Status != "CONNECTED")
			{

				errorDivForImportingWS.Style.Value = "display: block";
				errorDivForImportingWS.InnerHtml = "A connection was not able to be made.";
				return;
			}
			else
			{
				errorDivForImportingWS.Style.Value = "display: none";

				///////////////////////////////////////////////////////////////////////
				//This is where the population of the tree graph should be
				///////////////////////////////////////////////////////////////////////




				// 6/22/15 WS commented out for it not being needed anymore.  All insertion of data should be done on the OK press now (Which will be taken care of by Mukund and his team)
				//Insertdata();
			}
			fillsametimeNodesTree();
		}
		
		protected void Cancel_Click(object sender, EventArgs e)
		{
			//Response.Redirect("~/Configurator/SametimeServer.aspx?ID=" + Convert.ToInt32(Session["sametimeid"].ToString()) + "&tabs=1");
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
			Session["HostName"]=HostName.Text;

			Session["ConnectionComboBox"] = ConnectionComboBox.Text;
			Session["txtport"] = txtport.Text;
			Session["chbx"] = chbx.Checked;

			Session["realmtxtbx"] = realmtxtbx.Text;


		}//popup
		private void FillSametimeCredentialComboBox()
		{
			DataTable CredentialsDataTable = VSWebBL.ConfiguratorBL.ServicesBL.Ins.GetWebsphereserverCredentials(3);
			CredentialComboBox.DataSource = CredentialsDataTable;
			CredentialComboBox.TextField = "AliasName";
			CredentialComboBox.ValueField = "ID";
			CredentialComboBox.DataBind();


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

		protected void CellnameTextBox_Validation(object sender, ValidationEventArgs e)
		{
			DataTable returntable = VSWebBL.SecurityBL.webspehereImportBL.Ins.GetNmae(CellnameTextBox.Text);


			if (returntable.Rows.Count > 0)
			{
				errorDivForImportingWS.InnerHtml = "Name already exists.Please enter another Name." +
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
				if (ipaddress!= null)
				{
				
					errorDivForImportingWS.Style.Value = "display: none";
				}
			}
			catch (Exception ex)
			{
				errorDivForImportingWS.InnerHtml = "Please enter valid Host Name." +
					"<button type=\"button\" onclick='javascript:var SearchInput = $(\"#ContentPlaceHolder1_ASPxPageControl1_ASPxRoundPanel7_ASPxCallbackPanel2_HostName_I\");var strLength= SearchInput.val().length * 2;SearchInput.focus();SearchInput[0].setSelectionRange(strLength, strLength);' class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
				errorDivForImportingWS.Style.Value = "display: block";

				validhostname = false;
			
				//Div3.InnerHtml = "Please enter valid Host Name." +
			//	 "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";

				//Div3.InnerHtml = "Cell Name already exists with this Websphere configuration.Please enter another Websphere configuration." +
				//                            "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
				//Div3.Style.Value = "display: block";
				//Div3.Style.Value = "display: block";

			}
		}
	
	
	
	}
}
