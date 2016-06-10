using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DevExpress.Web;
using System.Drawing;
using System.Web.UI.HtmlControls;
using DevExpress.Web.ASPxScheduler;
using DevExpress.XtraScheduler;
using DevExpress.XtraScheduler.Xml;

namespace VSWebUI.Dashboard
{
	public partial class SummaryLandscape : System.Web.UI.Page
	{

		protected void Page_PreInit(object sender, EventArgs e)
		{
			//Mukund 05Nov13, Create an event handler for the master page's contentCallEvent event

			this.Master.contentCallEvent += new EventHandler(Master_ButtonClick);
            if (Session["showsummary"] != null)
            {
                if (Session["showsummary"].ToString() == "False")
                {
                    if (Session["UserLogin"] == null || Session["UserLogin"] == "")
                    {
                        Response.Redirect("~/login.aspx", false);
                        Context.ApplicationInstance.CompleteRequest();
                    }

                }
            }
		}
		private void Master_ButtonClick(object sender, EventArgs e)
		{
			Response.Redirect(Request.Url.ToString());

		}

		protected void Page_Load(object sender, EventArgs e)
		{
			try
			{
				bool exists = true;
				string pathdata = "";
				if (Session["CustomBackground"] != null)
				{
					pathdata = Session["CustomBackground"].ToString();
					exists = System.IO.Directory.Exists(Server.MapPath(pathdata));
				}


				if (!exists)
				{
					((HtmlGenericControl)this.Page.Master.FindControl("wrapdiv")).Style["background-image"] = Page.ResolveUrl(pathdata);
				}
				else
				{
					HtmlGenericControl body = (HtmlGenericControl)Master.FindControl("wrapdiv");
					body.Attributes.Add("style", "background-color:#f8f8c0;");

				}
				if (!IsPostBack)
				{
					BindDataView();
				}
			}
			catch (Exception ex)
			{
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}
			//10/9/2014 NS added 
			string url = HttpContext.Current.Request.Url.AbsoluteUri;
			Session["BackURL"] = url;// "SummaryLandscape.aspx";
		}

		protected void BindDataView()
		{
			try
			{
                //10/5/2015 NS modified
				//DataTable dt2 = VSWebBL.DashboardBL.DashboardBL.Ins.GetStatusGrid("'Domino','Exchange'");
                DataTable dt2 = VSWebBL.DashboardBL.DashboardBL.Ins.GetDeviceStatus("", "", "null", "", "", "ordernum,Name");
				ASPxDataView2.DataSource = dt2;
				ASPxDataView2.DataBind();
			}
			catch (Exception ex)
			{
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}
		}
		private HtmlControl FindHtmlControlByIdInControl(Control control, string id)
		{
			foreach (Control childControl in control.Controls)
			{
				if (childControl.ID != null && childControl.ID.Equals(id, StringComparison.OrdinalIgnoreCase) && childControl is HtmlControl)
				{
					return (HtmlControl)childControl;
				}

				if (childControl.HasControls())
				{
					HtmlControl result = FindHtmlControlByIdInControl(childControl, id);
					if (result != null) return result;
				}
			}

			return null;
		}
		protected void ASPxDataView2_DataBound(object sender, EventArgs e)
		{
			//    Mukund 10Jun2014 
			//VSPLUS-673: Executive Summary should have the same right-click menu as other screens
			try
			{

				ASPxDataView dataview = new ASPxDataView();
				dataview = (ASPxDataView)sender;
				if (dataview.Items.Count > 0)
				{
					for (int j = 0; j < dataview.Items.Count; j++)
					{
						ASPxPanel panel = new ASPxPanel();
						panel = (ASPxPanel)dataview.FindItemControl("ASPxPanel1", dataview.Items[j]);
						panel.BackColor = GetColor(dataview.Items[j].DataItem);

						string strURL = HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority;
						string strURLFile = "";

						string lblLoc = (dataview.Items[j].DataItem as DataRowView)["Location"].ToString();
                        //10/5/2015 NS modified for VSPLUS-2247
                        //string lblIP = (dataview.Items[j].DataItem as DataRowView)["IPAddress"].ToString();
						string lbltext = (dataview.Items[j].DataItem as DataRowView)["Name"].ToString();
						string lbltype = (dataview.Items[j].DataItem as DataRowView)["Type"].ToString();
						//10/9/2014 NS added
						string lblstatus = (dataview.Items[j].DataItem as DataRowView)["Status"].ToString();
						string Cat = (dataview.Items[j].DataItem as DataRowView)["Category"].ToString();

						string lbltext1 = "";
						//1/30/2015 NS added for VSPLUS-1367
						string lblDate = (dataview.Items[j].DataItem as DataRowView)["LastUpdate"].ToString();
						lbltext1 = lbltext;
						HtmlGenericControl divControl = (HtmlGenericControl)panel.FindControl("divmenu");// e.Item.FindControl("divControl") as HtmlGenericControl;
						string imgtext = "";
						imgtext = "<img src='" + (dataview.Items[j].DataItem as DataRowView)["imgsource"].ToString() + "'/>";
						string lbl2 = "";
						//lbl2 = "<font color='" + GetTextColor(dataview.Items[j].DataItem).ToString() + "' face='Tahoma' size='1'>" + (dataview.Items[j].DataItem as DataRowView)["Status"].ToString() + "</font>";
						lbl2 = "<span class='" + GetTextCSS(dataview.Items[j].DataItem) + "'>" + (dataview.Items[j].DataItem as DataRowView)["Status"].ToString() + "</span>";
						if (lbltext.Length > 16)
						{
							lbltext1 = lbltext.Substring(0, 14) + "...";
						}
						if (!(Session["UserFullName"] != null && Session["UserFullName"].ToString() == "Anonymous"))
						{
							string stredit = "";
							string strsuspend = "";
							if (Session["Isconfigurator"] != null)
							{
								if (Session["Isconfigurator"].ToString() == "True")
								{
									stredit = "                    ,'edit" + j + "': { name: 'Edit in Configurator', icon: 'cut' }";
									strsuspend = "                    ,'suspend" + j + "': { name: 'Suspend Temporarily', icon: 'copy' }";
								}
							}

							string id = (VSWebBL.SecurityBL.ServersBL.Ins.GetServerIDbyServerNameType(lbltext, lbltype)).ToString();
							if (lbltype == "Exchange")
							{
                                //10/5/2015 NS modified for VSPLUS-2247
                                strURLFile = "/Configurator/ExchangeServer.aspx?ID=" + id + "&name=" + lbltext + "&Cat=Exchange&Loc=" + lblLoc; // + "&ipaddr=" + lblIP;
							}
							else if (lbltype == "Mail")
							{
								id = (VSWebBL.ConfiguratorBL.MailServicesBL.Ins.GetServerIDbyServerName(lbltext)).ToString();
								strURLFile = "/Configurator/MailService.aspx?Key=" + id;
							}
							else if (lbltype == "Skype for Business")
							{

								strURLFile = "/Configurator/LyncServer.aspx?ID=" + id + "&Name=" + lbltext + "&Cat=" + Cat + "&Loc=" + lblLoc;

							}
							else if (lbltype == "SharePoint")
							{

								strURLFile = "/Configurator/SharepointServer.aspx?ID=" + id + "&Name=" + lbltext + "&Cat=" + Cat + "&Loc=" + lblLoc;

							}
							else if (lbltype.ToString() == "BES")
							{
								id = (VSWebBL.SecurityBL.ServersBL.Ins.GetServerIDbyServerName(lbltext)).ToString();

								strURLFile = "/Configurator/BlackBerryEntertpriseServer.aspx?Key=" + id;

							}
							else if (lbltype == "Sametime")
							{
								id = (VSWebBL.SecurityBL.ServersBL.Ins.GetServerIDbyServerName(lbltext)).ToString();
								strURLFile = "/Configurator/SametimeServer.aspx?ID=" + id;

							}


							else if (lbltype == "Network Device")
							{
								id = (VSWebBL.ConfiguratorBL.NetworkDevicesBL.Ins.GetServerIDbyServerName(lbltext)).ToString();
								strURLFile = "/Configurator/NetworkDeviceProperties.aspx?ID=" + id;

							}
							else if (lbltype == "URL")
							{
								id = (VSWebBL.ConfiguratorBL.URLsBL.Ins.GetServerIDbyServerName(lbltext)).ToString();
								strURLFile = "/Configurator/URLProperties.aspx?ID=" + id;

							}
							else if (lbltype == "Notes Database")
							{
								NotesDatabases nodj = new NotesDatabases();
								nodj.Name = lbltext;
								DataTable dt = VSWebBL.ConfiguratorBL.NotesDatabaseBL.Ins.GetName(nodj);
								if(dt.Rows.Count>0)
								{
								id = dt.Rows[0]["ID"].ToString();
									}
								strURLFile = "/Configurator/EditNotes.aspx?ID=" + id;

							}
							else if (lbltype == "NotesMail Probe")
							{


								strURLFile = "/Configurator/EditNotesMailProbe.aspx?Name=" + lbltext;

							}
							else if (lbltype == "ExchangeMail Probe")
							{


								strURLFile = "/Configurator/EditExchangeMailProbe.aspx?Name=" + lbltext;

							}
							else if (lbltype == "Windows")
							{
								id = (VSWebBL.SecurityBL.ServersBL.Ins.GetServerIDbyServerName(lbltext)).ToString();

								strURLFile = "/Configurator/WindowsProperties.aspx?ID=" + id + "&Name=" + lbltext + "&Cat=" + Cat + "&Loc=" + lblLoc;

							}
							else if (lbltype == "Active Directory")
							{
								id = (VSWebBL.SecurityBL.ServersBL.Ins.GetServerIDbyServerName(lbltext)).ToString();
                                //10/5/2015 NS modified for VSPLUS-2247
                                strURLFile = "/Configurator/ActiveDirectoryProperties.aspx?ID=" + id + "&name=" + lbltext + "&Cat=" + Cat + "&Loc=" + lblLoc; // +"&ipaddr=" + lblIP;

							}
                            //5/9/2016 Sowjanya modified for VSPLUS-2943
                            else if (lbltype == "IBM Connections")
                            {
                                id = (VSWebBL.SecurityBL.ServersBL.Ins.GetServerIDbyServerName(lbltext)).ToString();
                               
                                strURLFile = "/Configurator/IBMConnections.aspx?ID=" + id + "&name=" + lbltext + "&Cat=" + Cat + "&Loc=" + lblLoc; // +"&ipaddr=" + lblIP;

                            }
							else if (lbltype == "Office365")
							{


								strURLFile = "/Configurator/O365ServerProperties.aspx";

							}
							else
							{
								strURLFile = "/Configurator/DominoProperties.aspx?Key=" + id;
							}


							//lbltext1 = "<strong><font color='Black' face='Tahoma' size='2'><a class='dlink' href='DominoServerDetailsPage2.aspx?Name=" + lbltext + "&Type=Domino'>" + lbltext1 + "</a></font></strong>";
							//1/30/2015 NS modified for VSPLUS-1367
							//lbltext1 = "<a class='ahrefdom' href='DominoServerDetailsPage2.aspx?Name=" + lbltext + "&Type=Domino&Status=" + lblstatus + "'>" + lbltext1 + "</a>";
                            //10/5/2015 NS modified for VSPLUS-2247
							if (lbltype == "Domino")
							{
                                lbltext1 = "<a class='ahrefdom' title='" + lbltext + "' href='DominoServerDetailsPage2.aspx?Name=" + lbltext + "&Type=" + lbltype + "&Status=" + lblstatus + "'>" + lbltext1 + "</a>";
							}
							else if (lbltype == "Exchange")
							{
                                lbltext1 = "<a class='ahrefdom' title='" + lbltext + "' href='ExchangeServerDetailsPage3.aspx?Name=" + lbltext + "&Type=" + lbltype + "&Status=" + lblstatus + "&LastDate=" + lblDate + "'>" + lbltext1 + "</a>";
							}
							else if (lbltype == "Database Availability Group")
							{
                                lbltext1 = "<a class='ahrefdom' title='" + lbltext + "' href='DAGHealth.aspx?Name=" + lbltext + "'>" + lbltext1 + "</a>";
							}
							else if (lbltype == "SharePoint")
							{
                                lbltext1 = "<a class='ahrefdom' title='" + lbltext + "' href='Sharepointdetailspage.aspx?Name=" + lbltext + "&Type=" + lbltype + "&Status=" + lblstatus + "&LastDate=" + lblDate + "'>" + lbltext1 + "</a>";
							}
							else if (lbltype == "Active Directory")
							{
                                lbltext1 = "<a class='ahrefdom' title='" + lbltext + "' href='ActiveDirectoryServerDetailsPage3.aspx?Name=" + lbltext + "&Type=" + lbltype + "&Status=" + lblstatus + "&LastDate=" + lblDate + "'>" + lbltext1 + "</a>";
							}
							else if (lbltype == "URL")
							{
                                lbltext1 = "<a class='ahrefdom' title='" + lbltext + "' href='URLDetails.aspx?Name=" + lbltext + "&Type=" + lbltype + "&Status=" + lblstatus + "'>" + lbltext1 + "</a>";
							}
							else if (lbltype == "Sametime")
							{
                                lbltext1 = "<a class='ahrefdom' title='" + lbltext + "' href='SametimeServerDetailsPage.aspx?Name=" + lbltext + "&Type=" + lbltype + "&Status=" + lblstatus + "&LastDate=" + lblDate + "'>" + lbltext1 + "</a>";
							}
							else if (lbltype == "Notes Database")
							{
                                lbltext1 = "<a class='ahrefdom' title='" + lbltext + "' href='DominoServerDetailsPage2.aspx?Name=" + lbltext + "&Type=" + lbltype + "&Status=" + lblstatus + "&LastDate=" + lblDate + "'>" + lbltext1 + "</a>";
							}
							else if (lbltype == "Office365")
							{
                                lbltext1 = "<a class='ahrefdom' title='" + lbltext + "' href='office365health.aspx?Name=" + lbltext + "&Type=" + lbltype + "&LastDate=" + lblDate + "'>" + lbltext1 + "</a>";
							}
							else if (lbltype == "NotesMail Probe")
							{
                                lbltext1 = "<a class='ahrefdom' title='" + lbltext + "' href='NotesMailProbeDetailsPage.aspx?Name=" + lbltext + "&Type=" + lbltype + "&Status=" + lblstatus + "&LastDate=" + lblDate + "'>" + lbltext1 + "</a>";
							}
							else if (lbltype == "Skype for Business")
							{
                                lbltext1 = "<a class='ahrefdom' title='" + lbltext + "' href='Lyncdetailspage.aspx?Name=" + lbltext + "&Type=" + lbltype + "&LastDate=" + lblDate + "'>" + lbltext1 + "</a>";
							}
							else if (lbltype == "BES")
							{
                                lbltext1 = "<a class='ahrefdom' title='" + lbltext + "' href='BlackBerryServerDetailsPage2.aspx?Name=" + lbltext + "&Type=" + lbltype + "&LastDate=" + lblDate + "'>" + lbltext1 + "</a>";
							}
							else if (lbltype == "ExchangeMail Probe")
							{
                                lbltext1 = "<a class='ahrefdom' title='" + lbltext + "' href='ExchangeMailProbeDetailsPage.aspx?Name=" + lbltext + "&Type=" + lbltype + "&LastDate=" + lblDate + "'>" + lbltext1 + "</a>";
							}
							else if (lbltype == "Windows")
							{
                                lbltext1 = "<a class='ahrefdom' title='" + lbltext + "' href='WindowsServerDetails.aspx?Name=" + lbltext + "&Type=" + lbltype + "&LastDate=" + lblDate + "'>" + lbltext1 + "</a>";
							}
							else if (lbltype == "Network Device")
							{
                                lbltext1 = "<a class='ahrefdom' title='" + lbltext + "' href='NetworkServerDetails.aspx?Name=" + lbltext + "&Type=" + lbltype + "&LastDate=" + lblDate + "'>" + lbltext1 + "</a>";
							}
							else if (lbltype == "SNMP Device")
							{
                                lbltext1 = "<a class='ahrefdom' title='" + lbltext + "' href='SNMPDeviceDetails.aspx?Name=" + lbltext + "&Type=" + lbltype + "&LastDate=" + lblDate + "'>" + lbltext1 + "</a>";
							}
                            //5/12/2016 Sowjanya modified for VSPLUS-2943
                            else if (lbltype == "IBM Connections")
                            {
                                lbltext1 = "<a class='ahrefdom' title='" + lbltext + "' href='ConnectionsDetailsPage.aspx?Name=" + lbltext + "&Type=" + lbltype + "&Status=" + lblstatus + "&LastDate=" + lblDate + "'>" + lbltext1 + "</a>";
                            }
                            else
                            {
                                lbltext1 = "<a class='ahrefdom' title='" + lbltext + "' href='DominoServerDetailsPage2.aspx?Name=" + lbltext + "&Type=" + lbltype + "&Status=" + lblstatus + "&LastDate=" + lblDate + "'>" + lbltext1 + "</a>";
                            }
							divControl.InnerHtml =
						"     <div class='context-menu-one" + j + "'><table id='tbl' class='boxshadow' runat='server'>" +
							   "   <tr>" +
							   "     <td>" + imgtext +
							   "      </td>" +
							   "<td>" +
							   "            <div style='overflow: visible'>" + lbltext1 +
							   "            </div>" +
							   "        </td>" +
							   "    </tr>" +
							   "    <tr>" +
							   "        <td>" +
							   "            &nbsp;</td>" +
							   "        <td>" + lbl2 +
							   "        </td>" +
							   "    </tr>" +
							   "</table> </div>" +
							   "<script type='text/javascript'>" +
							   "        $(function () {" +
							   "            $.contextMenu({" +
							   "                selector: '.context-menu-one" + j + "'," +
							   "                callback: function (key, options) {" +
							   "                    var m =  key; " +
							   "                    var scan_n = 'scan" + j + "'; " +
							   "                    var edit_n = 'edit" + j + "'; " +
							   "                    var suspend_n = 'suspend" + j + "'; " +
							   " if(m==edit_n){window.location.replace('" + strURL + strURLFile + "');}" +
							   " if(m==scan_n){ScanNow('" + lbltext + "','" + lbltype + "');}" +
							   " if(m==suspend_n){SuspendNow('" + lbltext + "');}" +
							   "                }," +
							   "                items: {" +
							   "                    'scan" + j + "': { name: 'Scan Now', icon: 'edit' }" +
							 stredit +
							   strsuspend +
							   "                }" +
							   "            });" +
							   "" +
							   "            $('.context-menu-one" + j + "').on('click', function (e) {" +
							   "                console.log('clicked', this); " +
							   "            })" +
							   "        });" +
							   " " +
							   "    </script>";

						}
						else
						{
							lbltext1 = "<strong><font color='Black' face='Tahoma' size='2'>" + lbltext1 + "</font></strong>";
							divControl.InnerHtml =
						"    <table id='tbl' class='boxshadow' runat='server'>" +
							   "   <tr>" +
							   "     <td>" + imgtext +
							   "      </td>" +
							   "<td>" +
							   "            <div style='overflow: visible'>" + lbltext1 +
							   "            </div>" +
							   "        </td>" +
							   "    </tr>" +
							   "    <tr>" +
							   "        <td>" +
							   "            &nbsp;</td>" +
							   "        <td>" + lbl2 +
							   "        </td>" +
							   "    </tr>" +
							   "</table>";
						}

					}
				}

			}
			catch (Exception ex)
			{

				throw ex;
			}
		}

		public Color GetColor(object dataItem)
		{
			DataRowView row = dataItem as DataRowView;
			string status = "";
			if (row != null)
			{
				status = row["StatusCode"].ToString();
				//3/4/2013 NS modified per Alan's request - Telnet should be green
				if (status == "OK" || status == "Scanning")
				{
					return System.Drawing.Color.LightGreen;
				}
				else if (status == "Not Responding")
				{
					return System.Drawing.Color.Red;
				}
				else if (status == "Not Scanned")
				{
					return System.Drawing.Color.FromName("#87CEEB");
				}
				else if (status == "Disabled")
				{
					return System.Drawing.Color.FromName("#D0D0D0");
				}
				else if (status == "Maintenance")
				{
					return System.Drawing.Color.LightBlue;
				}
				else if (status == "")
				{
					return System.Drawing.Color.FromName("#ffff40");
				}
			}
			return Color.FromName("#ffff40");
		}

		public Color GetTextColor(object dataItem)
		{
			DataRowView row = dataItem as DataRowView;
			string status = "";
			if (row != null)
			{
				status = row["StatusCode"].ToString();
				if (status == "")
				{
					return System.Drawing.Color.FromName("#808080");
				}
				else
				{
					if (status == "Not Responding")
					{
						return System.Drawing.Color.White;
					}
					else
					{
						return System.Drawing.Color.Black;
					}
				}
			}
			return System.Drawing.Color.FromName("#808080");
		}
		public string GetTextCSS(object dataItem)
		{
			//    Mukund 10Jun2014 
			//VSPLUS-673: Executive Summary should have the same right-click menu as other screens

			DataRowView row = dataItem as DataRowView;
			string status = "";
			if (row != null)
			{
				status = row["StatusCode"].ToString();
				if (status == "")
				{
					return "dtldomOther";
				}
				else
				{
					if (status == "Not Responding")
					{
						return "dtldomWhite";
					}
					else
					{
						return "dtldomBlack";
					}
				}
			}
			return "dtldomOther";
		}
		/*
		protected void ASPxDataView3_DataBound(object sender, EventArgs e)
		{
			if (ASPxDataView3.Items.Count > 0)
			{
				for (int i = 0; i < ASPxDataView3.Items.Count; i++)
				{
					ASPxLabel lbl = new ASPxLabel();
					lbl = (ASPxLabel)ASPxDataView3.FindItemControl("ASPxLabel3", ASPxDataView3.Items[i]);
					lbl.Text = (ASPxDataView3.Items[i].DataItem as DataRowView)["Type"].ToString() + " List";

					ASPxDataView dataview = new ASPxDataView();
					dataview = (ASPxDataView)ASPxDataView3.FindItemControl("ASPxDataView2", ASPxDataView3.Items[i]);

					DataTable dt2 = VSWebBL.DashboardBL.DashboardBL.Ins.GetStatusGrid((ASPxDataView3.Items[i].DataItem as DataRowView)["Type"].ToString());
					dataview.DataSource = dt2;
					dataview.DataBind();

                    
					//if (dataview.Items.Count > 0)
					//{
					  //  for (int j = 0; j < dataview.Items.Count; j++)
						//{
						  //  ASPxPanel panel = new ASPxPanel();
						   // panel = (ASPxPanel)dataview.FindItemControl("ASPxPanel1",dataview.Items[j]);
							//panel.BackColor = GetColor(dataview.Items[j].DataItem);

							//ASPxLabel lbl = new ASPxLabel();
							//lbl = (ASPxLabel)panel.FindControl("ASPxLabel1");
							//lbl = (ASPxLabel)ASPxDataView2.FindItemControl("ASPxLabel1", ASPxDataView2.VisibleItems[i]);
							//lbl.Text = (dataview.Items[j].DataItem as DataRowView)["Name"].ToString();

							//ASPxImage img = new ASPxImage();
							//img = (ASPxImage)panel.FindControl("ASPxImage1");
							//img = (ASPxImage)ASPxDataView2.FindItemControl("ASPxImage1", ASPxDataView2.Items[i]);
							//img.ImageUrl = (dataview.Items[j].DataItem as DataRowView)["imgsource"].ToString();

							//ASPxLabel lbl2 = new ASPxLabel();
							//lbl2 = (ASPxLabel)panel.FindControl("ASPxLabel2");
							//lbl2.Text = (dataview.Items[j].DataItem as DataRowView)["Status"].ToString();
						//}
					//}
				}
			}
		}*/



		protected void BtnApply_Click(object sender, EventArgs e)
		{
			//    Mukund 10Jun2014 
			//VSPLUS-673: Executive Summary should have the same right-click menu as other screens

			try
			{

				// DataRow row = (DataRow)Session["myRow"];
				//if (row != null)
				if (hfName.Value != null)
				{
					List<string> serverIDValues = new List<string>();
					List<string> servertypeIDValues = new List<string>();
					Random random = new Random((int)DateTime.Now.Ticks);
					//RandomString(5, random);
					//string Name = row["Name"].ToString() + "-Temp-" + DateTime.Now.ToLongDateString();
					string Name = hfName.Value + "-Temp-" + DateTime.Now.ToString();//.ToLongDateString();
					string StartDate = DateTime.Now.ToShortDateString();
					string StartTime = DateTime.Now.ToShortTimeString();
					DateTime sdt = Convert.ToDateTime(StartDate);
					string Duration = TbDuration.Text;
					string EndDate = DateTime.Now.ToShortDateString();
					DateTime edt = Convert.ToDateTime(EndDate);
					string MaintType = "1";
					string MaintDaysList = "";
					string altime = DateTime.Now.ToShortTimeString();
					DateTime al = Convert.ToDateTime(altime);
					ASPxScheduler sh = new ASPxScheduler();
					Appointment apt = sh.Storage.CreateAppointment(AppointmentType.Pattern);
					Reminder r = apt.CreateNewReminder();

					//int min = Convert.ToInt32(MaintDurationTextBox.Text);
					int min = Convert.ToInt32(TbDuration.Text);
					r.AlertTime = al.AddMinutes(min);
					//3/24/2015 NS modified for DevExpress upgrade 14.2
					//ReminderXmlPersistenceHelper reminderHelper = new ReminderXmlPersistenceHelper(r, DateSavingType.LocalTime);
					ReminderXmlPersistenceHelper reminderHelper = new ReminderXmlPersistenceHelper(r);
					string rem = reminderHelper.ToXml().ToString();

					RecurrenceInfo reci = new RecurrenceInfo();
					reci.BeginUpdate();
					reci.AllDay = false;
					reci.Periodicity = 10;
					reci.Range = RecurrenceRange.EndByDate;
					reci.Start = sdt;
					reci.End = edt;
					reci.Duration = edt - sdt;
					reci.Type = RecurrenceType.Yearly;


					OccurrenceCalculator calc = OccurrenceCalculator.CreateInstance(reci);
					TimeInterval ttc = new TimeInterval(reci.Start, reci.End + new TimeSpan(1, 0, 0));


					var bcoll = calc.CalcOccurrences(ttc, apt);
					if (bcoll.Count != 0)
					{
						reci.OccurrenceCount = bcoll.Count;
					}
					else
					{
						reci.OccurrenceCount = 1;
					}
					reci.Range = RecurrenceRange.OccurrenceCount;
					reci.EndUpdate();
					string s = reci.ToXml();

					string EndDateIndicator = "";
					//DataTable dt = VSWebBL.SecurityBL.ServersBL.Ins.GetServerDetailsByName(row["Name"].ToString());
					DataTable dt = VSWebBL.SecurityBL.ServersBL.Ins.GetServerDetailsByName(hfName.Value);

					if (dt != null && dt.Rows.Count > 0)
					{
						//VSPLUS-833:Suspend temporarily is not working correctly 
						//19Jul14, Mukund, The below two were reversely assigned so suspend wasnt working. 
						servertypeIDValues.Add(dt.Rows[0][2].ToString());
						serverIDValues.Add(dt.Rows[0][0].ToString());
					}
					bool update = false;
					if (servertypeIDValues != null && servertypeIDValues.Count > 0)
					{
						update = VSWebBL.ConfiguratorBL.MaintenanceBL.Ins.UpdateMaintenanceWindows(null, Name, StartDate, StartTime, Duration,
							  EndDate, MaintType, MaintDaysList, EndDateIndicator, serverIDValues, s, rem, 1, true, servertypeIDValues, "true", "1");
					}
					if (update == true)
					{
						//SuccessMsg.InnerHtml = "Monitoring for " + row["Name"].ToString() + " has been temporarily suspended for a duration of " + TbDuration.Text + " minutes.";
						//10/3/2014 NS modified for VSPLUS-990
						SuccessMsg.InnerHtml = "Monitoring for " + hfName.Value + " has been temporarily suspended for a duration of " + TbDuration.Text + " minutes." +
							"<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";

						SuccessMsg.Style.Value = "display: block";

						BindDataView();
					}
					else
					{
						//10/3/2014 NS modified for VSPLUS-990
						ErrorMsg.InnerHtml = "The Settings were NOT updated." +
							"<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
						ErrorMsg.Style.Value = "display: block";
					}
					SuspendPopupControl.ShowOnPageLoad = false;
				}

			}
			catch (Exception ex)
			{
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}
		}


		//    Mukund 10Jun2014 
		//VSPLUS-673: Executive Summary should have the same right-click menu as other screens
		[System.Web.Services.WebMethod]
		public static string ScanNow(string strName, string strType)
		{
			try
			{
				Status StatusObj = new Status();
				StatusObj.Name = strName;
				StatusObj.Type = strType;

				string strScan = (strType == "NotesMail Probe" ? "ScanNotesMailProbeASAP" : (strType == "Mail" ? "" : "ScanDominoASAP"));

				bool bl = VSWebBL.StatusBL.StatusTBL.Ins.UpdateforScan(StatusObj);

				if (StatusObj.Type == "Domino")
				{
					bl = VSWebBL.SettingBL.SettingsBL.Ins.UpdateScanvalue("ScanDominoASAP", StatusObj.Name, VSWeb.Constants.Constants.SysString);
				}
				else if (StatusObj.Type == "Mail")
				{
					bl = VSWebBL.SettingBL.SettingsBL.Ins.UpdateScanvalue("ScanMailServiceASAP", StatusObj.Name, VSWeb.Constants.Constants.SysString);
				}
				else if (StatusObj.Type == "Network Device")
				{
					bl = VSWebBL.SettingBL.SettingsBL.Ins.UpdateScanvalue("ScanNetworkDeviceASAP", StatusObj.Name, VSWeb.Constants.Constants.SysString);
				}
				else if (StatusObj.Type == "Sametime Server")
				{
					bl = VSWebBL.SettingBL.SettingsBL.Ins.UpdateScanvalue("ScanSametimeASAP", StatusObj.Name, VSWeb.Constants.Constants.SysString);
				}
				else if (StatusObj.Type == "BES")
				{
					bl = VSWebBL.SettingBL.SettingsBL.Ins.UpdateScanvalue("ScanBlackBerryASAP", StatusObj.Name, VSWeb.Constants.Constants.SysString);
				}
				else if (StatusObj.Type == "URL")
				{
					bl = VSWebBL.SettingBL.SettingsBL.Ins.UpdateScanvalue("ScanURLASAP", StatusObj.Name, VSWeb.Constants.Constants.SysString);
				}
				else if (StatusObj.Type == "Exchange")
				{
					bl = VSWebBL.SettingBL.SettingsBL.Ins.UpdateScanvalue("ScanExchangeASAP", StatusObj.Name, VSWeb.Constants.Constants.SysString);
				}
				else if (StatusObj.Type == "SharePoint")
				{
					bl = VSWebBL.SettingBL.SettingsBL.Ins.UpdateScanvalue("ScanSharePointASAP", StatusObj.Name, VSWeb.Constants.Constants.SysString);
				}

				return "true";
			}
			catch (Exception)
			{

				return "false";

			}

		}
	}
}