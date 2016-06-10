using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.HtmlControls;
using DevExpress.XtraCharts;
using DevExpress.Web;
using System.Drawing;


namespace VSWebUI.Dashboard
{
	public partial class Office365Health : System.Web.UI.Page
	{
		string NodeName;
		string accountName;
		string selectedAccName;
		int ID;
		string Mode;
		string location;
		string selectedMode;
		protected void Page_PreInit(object sender, EventArgs e)
		{

			this.Master.contentCallEvent += new EventHandler(Master_ButtonClick);
		}
		private void Master_ButtonClick(object sender, EventArgs e)
		{

		}
		protected void Page_Load(object sender, EventArgs e)
		{
			if (Request.QueryString["Name"] != null)
				accountName = Request.QueryString["Name"].ToString();
			if (accountName != "")
			{
                //2/26/2016 NS modified for VSPLUS-2648
				//fill0365Grid(accountName);
                GetO365Tests(accountName);
				if (officehealthgrid.VisibleRowCount > 0)
				{
					int index = officehealthgrid.FocusedRowIndex;
					officehealthgrid.Selection.UnselectAll();
					if (index > -1)
					{
						//servernamelbldisp.InnerHtml = "";
						ID = Convert.ToInt32(officehealthgrid.GetRowValues(index, "Id").ToString());
						selectedAccName = officehealthgrid.GetRowValues(index, "AccountName").ToString();
						ASPxPageControl1.ActiveTabIndex = 0;
						DataTable dt1 = VSWebBL.DashboardBL.Office365BL.Ins.GetNodeName(ID, selectedAccName);
						NodeName = dt1.Rows[0]["Name"].ToString();
						location = dt1.Rows[0]["Location"].ToString();
						selectedMode = dt1.Rows[0]["Mode"].ToString();
						servernamelbldisp.Text = "Microsoft Office 365 Health" + " - " + selectedAccName;
						servernamelbldisp.CssClass = "header";
					}
				}

				if (!IsPostBack && !IsCallback)
				{
					HtmlGenericControl body = (HtmlGenericControl)Master.FindControl("Body1");
					body.Attributes.Add("onload", "DoCallback()");
					body.Attributes.Add("onResize", "Resized()");
				}
				else
				{
					Fillmaintenancegridfromsession();

					//Alert tab
					FillAlertHistoryfromSession();

					//Outages tab
					FilloutagefromSession();
				}
			//1/28/2015 NS added for VSPLUS-1404
				if (!IsPostBack)
				{

                    //23/2/2016 Sowmya Added for VSPLUS 2637
					if (Session["UserPreferences"] != null)
					{
						DataTable UserPreferences = (DataTable)Session["UserPreferences"];
						foreach (DataRow dr in UserPreferences.Rows)
						{
                            if (dr[1].ToString() == "Office365Health|officehealthgrid")
							{
                                officehealthgrid.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
								
							}
                            if (dr[1].ToString() == "Office365Health|HealthAssessmentGrid")
                            {
                                HealthAssessmentGrid.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                            }
						}
					}
					//servernamelbldisp.InnerHtml += " - " + selectedAccName;
				}
			    FillMailboxChart(selectedAccName);
				FillMailboxStatListView();
				FillGeneralStatListView();
				Fillmaintenancegrid();

				//Alert tab
				FillAlertHistory();

				//Outages tab
				FillOutageTab();
                FillDevicesChart();
				FillHealthAssessmentStatusGrid("Office365", selectedAccName);
                FillPwdSettingsGrid();
				FillServiceDetailsgrid(selectedAccName);
				SetGraphForMailTests();
				SetGraphForSiteTests();
				SetGraphForTaskFolderTests();
				SetGraphForMailScenarioTests();
				SetGraphForOneDriveStats();
				SetGraphForDeviceType();
				//SetGraphForDeviceType(string SortField, string servername)
				SetGraphConfReport();
				SetGraphP2PSessions();
				SetGraphForAVSessions();
                //3/9/2016 NS added for VSPLUS-2648
                SetGraphForMailBoxTypes();
                SetGraphForUsersDaily();
                SetGraphForActiveInactiveUsers();
                SetGraphForInactiveMailboxes();
                SetGraphForPwdExpSettings();
                SetGraphForPwdStrongSettings();
                FillLicensesInfo();
			}
		}
		public void Fillmaintenancegridfromsession()
		{
			FillMaintenance();

		}
		public void fill0365Grid(string name)
		{
			DataTable o365table = new DataTable();
			o365table = VSWebBL.DashboardBL.Office365BL.Ins.get0635grid(name);
			try
			{
				Session["0365Table"] = o365table;
				officehealthgrid.DataSource = o365table;
				officehealthgrid.DataBind();
			}
			catch (Exception ex)
			{
				//6/27/2014 NS added for VSPLUS-634
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw;
			}
		}
		public void FillAlertHistory()
		{
			DataTable AlertHistorytab = new DataTable();
			try
			{
				AlertHistorytab = VSWebBL.DashboardBL.ExchangeServerDetailsBL.Ins.GetAlertHistry(Request.QueryString["Name"].ToString());
				Session["AlertHistorytab"] = AlertHistorytab;
				AlertGridView.DataSource = AlertHistorytab;
				AlertGridView.DataBind();
			}
			catch (Exception ex)
			{
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}


		}

		public void FillAlertHistoryfromSession()
		{
			DataTable AlertHistorytab = new DataTable();
			try
			{

				AlertHistorytab = (DataTable)Session["AlertHistorytab"];
				AlertGridView.DataSource = AlertHistorytab;
				AlertGridView.DataBind();
			}
			catch (Exception ex)
			{
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}


		}
		public void FillOutageTab()
		{
			DataTable dt = new DataTable();
			
			dt = VSWebBL.DashboardBL.ExchangeServerDetailsBL.Ins.GetOutage(selectedAccName, Request.QueryString["Type"]?? "Office365");
			Session["Outage"] = dt;
			OutageGridView.DataSource = dt;
			OutageGridView.DataBind();
		}

		public void FilloutagefromSession()
		{
			DataTable dt = new DataTable();
			dt = Session["Outage"] as DataTable;
			OutageGridView.DataSource = dt;
			OutageGridView.DataBind();
		}
		protected void btnsearch_Click(object sender, EventArgs e)
		{
			FillMaintenance();
		}
		public void Fillmaintenancegrid()
		{
			Session["SLmaitservers"] = "";
			DataTable dt = VSWebBL.ConfiguratorBL.MaintenanceBL.Ins.Getmaintenanceserversbyservername(Request.QueryString["Name"].ToString(), "", "", "", "");
			maintenancegrid.DataSource = dt;
			maintenancegrid.DataBind();
			Session["SLmaitservers"] = dt;
		}
		protected void ClearButton_Click(object sender, EventArgs e)
		{
			txtfromdate.Text = "";
			txttodate.Text = "";
			ASPxTimeEdit1.Text = "";
			ASPxTimeEdit2.Text = "";
			FillMaintenance();
		}
		public void FillMaintenance()
		{
			string fromdate = txtfromdate.Text;
			string todate = txttodate.Text;
			// string fromtime = //txtfromtime.Text;ASPxTimeEdit1

			string fromtime = ASPxTimeEdit1.Text;
			//  string totime = txttotime.Text;
			string totime = ASPxTimeEdit2.Text;
			if (txtfromdate.Text != "" && txttodate.Text != "")
			{
				if (Convert.ToDateTime(fromdate) > Convert.ToDateTime(todate))
				{
					MsgPopupControl.ShowOnPageLoad = true;
					ErrmsgLabel.Text = "From Date value should be less than To Date.";
				}

				else
				{

					if ((ASPxTimeEdit1.Text != null && ASPxTimeEdit2.Text != null) && (ASPxTimeEdit1.Text != "" && ASPxTimeEdit2.Text != ""))
					{
						string fhour = fromtime.Substring(0, fromtime.IndexOf(":"));
						string thour = totime.Substring(0, totime.IndexOf(":"));
						int fhourInt = int.Parse(fhour);
						int thourInt = int.Parse(thour);
						string fminute = fromtime.Substring(3, 2);
						string tminute = totime.Substring(3, 2);
						int fminuteInt = int.Parse(fminute);
						int tminuteInt = int.Parse(tminute);
						if (fhourInt >= 24 || thourInt >= 24 || fminuteInt >= 60 || tminuteInt >= 60)
						{
							MsgPopupControl.ShowOnPageLoad = true;
							ErrmsgLabel.Text = "Invalid hour/minute entry.";
						}

						else
						{
							Session["SLmaitservers"] = "";
							DataTable dt = VSWebBL.ConfiguratorBL.MaintenanceBL.Ins.Getmaintenanceserversbyservername(Request.QueryString["Name"].ToString(), fromdate, todate, fromtime, totime);
							Session["SLmaitservers"] = dt;
							maintenancegrid.DataSource = dt;
							maintenancegrid.DataBind();
						}
					}
				}
			}
			else
			{
				if ((ASPxTimeEdit1.Text != null && ASPxTimeEdit2.Text != null) && (ASPxTimeEdit1.Text != "" && ASPxTimeEdit2.Text != ""))
				{
					string fhour = fromtime.Substring(0, fromtime.IndexOf(":"));
					string thour = totime.Substring(0, totime.IndexOf(":"));
					int fhourInt = int.Parse(fhour);
					int thourInt = int.Parse(thour);
					string fminute = fromtime.Substring(3, 2);
					string tminute = totime.Substring(3, 2);
					int fminuteInt = int.Parse(fminute);
					int tminuteInt = int.Parse(tminute);
					if (fhourInt >= 24 || thourInt >= 24 || fminuteInt >= 60 || tminuteInt >= 60)
					{
						MsgPopupControl.ShowOnPageLoad = true;
						ErrmsgLabel.Text = "Invalid hour/minute entry.";
					}

					else
					{
						Session["SLmaitservers"] = "";
						DataTable dt = VSWebBL.ConfiguratorBL.MaintenanceBL.Ins.Getmaintenanceserversbyservername(Request.QueryString["Name"].ToString(), fromdate, todate, fromtime, totime);
						Session["SLmaitservers"] = dt;
						maintenancegrid.DataSource = dt;
						maintenancegrid.DataBind();
					}
				}
				//1/4/2013 NS - when Clear button is clicked, the grid needs to return to its original state
				//which means return all records regardless of the dates/times
				else
				{
					Session["SLmaitservers"] = "";
					DataTable dt = VSWebBL.ConfiguratorBL.MaintenanceBL.Ins.Getmaintenanceserversbyservername(Request.QueryString["Name"].ToString(), fromdate, todate, fromtime, totime);
					Session["SLmaitservers"] = dt;
					maintenancegrid.DataSource = dt;
					maintenancegrid.DataBind();
				}
			}

		}
		protected void maintenancegrid_PageSizeChanged(object sender, EventArgs e)
		{
			//ProfilesGridView.PageIndex;
			VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("ExchangeServerDetailsPage3|maintenancegrid", maintenancegrid.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
			Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
		}
		protected void officehealthgrid_OnPageSizeChanged(object sender, EventArgs e)
		{
			//ProfilesGridView.PageIndex;
			VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("Office365Health|officehealthgrid", officehealthgrid.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
			Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
		}
		protected void OKButton_Click(object sender, EventArgs e)
		{

			MsgPopupControl.ShowOnPageLoad = false;

		}
		protected void AlertGridView_PageSizeChanged(object sender, EventArgs e)
		{
			//ProfilesGridView.PageIndex;
			VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("ExchangeServerDetailsPage3|AlertGridView", AlertGridView.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
			Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
		}

		protected void OutageGridView_PageSizeChanged(object sender, EventArgs e)
		{
			//ProfilesGridView.PageIndex;
			VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("ExchangeServerDetailsPage3|OutageGridView", OutageGridView.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
			Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
		}
		protected void officehealthgrid_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
		{
			string status = "";
			status = e.GetValue("Status").ToString();
            if (e.DataColumn.FieldName == "Status")
            {
                e.Cell.Wrap = false;
            }
			if (e.DataColumn.FieldName == "Status" && (e.CellValue.ToString() == "OK" || e.CellValue.ToString() == "Scanning" || e.CellValue.ToString() == "Telnet"))
			{
				e.Cell.BackColor = System.Drawing.Color.LightGreen;
			}

			else if (e.DataColumn.FieldName == "Status" && e.CellValue.ToString() == "Not Responding")
			{
				e.Cell.BackColor = System.Drawing.Color.Red;
				e.Cell.ForeColor = System.Drawing.Color.White;
			}
			else if (e.DataColumn.FieldName == "Status" && e.CellValue.ToString() == "Not Scanned")
			{
				e.Cell.BackColor = System.Drawing.Color.Blue;
				e.Cell.ForeColor = System.Drawing.Color.White;
			}
			else if (e.DataColumn.FieldName == "Status" && e.CellValue.ToString() == "Disabled")
			{
				e.Cell.BackColor = System.Drawing.Color.FromName("#D0D0D0");
				//e.Cell.BackColor = System.Drawing.Color.Gray;
				// e.Cell.ForeColor = System.Drawing.Color.White;
			}
			else if (e.DataColumn.FieldName == "Status" && e.CellValue.ToString() == "Issue")
			{
				e.Cell.BackColor = System.Drawing.Color.Yellow;
			}
			else if (e.DataColumn.FieldName == "Status")
			{
				e.Cell.BackColor = System.Drawing.Color.Yellow;
				// e.DataColumn.GroupFooterCellStyle.ForeColor = System.Drawing.Color.Yellow;

			}
            //2/26/2016 NS added for VSPLUS-2648
            string[] excludeArr = new string[] { "AccountName","Id","Mode","NodeName","Location","Status","LastUpdate" };
            int foundindex = Array.IndexOf(excludeArr, e.DataColumn.FieldName);
            if (foundindex == -1)
            {
                if (e.CellValue.ToString().ToLower() == "pass")
                {
                    e.Cell.BackColor = System.Drawing.Color.LightGreen;
                }
                else if (e.CellValue.ToString().ToLower() == "fail")
                {
                    e.Cell.BackColor = System.Drawing.Color.Red;
                    e.Cell.ForeColor = System.Drawing.Color.White;
                }
            }

		}
		public void FillMailboxStatListView()
		{
			//DataTable dt = VSWebBL.DashboardBL.Office365BL.Ins.FillStatListView("Mailboxes",accountName );
			DataTable dt = VSWebBL.DashboardBL.Office365BL.Ins.FillStatListView("Mailboxes", selectedAccName);
			if (dt.Rows.Count > 0)
			{
				lblNoOfMailBoxesValue.Text = dt.Rows[0].ItemArray[0].ToString();

            
				//lblSizeOfMailBoxesValue.Text = dt.Rows[0].ItemArray[2].ToString() + " MB";
				if (dt.Rows[0].ItemArray[2] != DBNull.Value)
				{
					double mailbox = Convert.ToDouble(dt.Rows[0].ItemArray[2]);

					lblSizeOfMailBoxesValue.Text = Math.Round(mailbox / 1000, 3) + " GB";
				}
				lblTotalNoOfItemsValue.Text = dt.Rows[0].ItemArray[1].ToString();
				//lblAvgSizeOfMailBoxesValue.Text = dt.Rows[0].ItemArray[3].ToString() + " MB";
				if (dt.Rows[0].ItemArray[3] != DBNull.Value)
				{
					double mailboxvalue = Convert.ToDouble(dt.Rows[0].ItemArray[3]);

					lblAvgSizeOfMailBoxesValue.Text = Math.Round(mailboxvalue / 1000, 3) + " GB";
				}
				lblAvgCountOfItemsValue.Text = dt.Rows[0].ItemArray[4].ToString();
			}
			//MailboxStatListView.DataSource = dt;
			//MailboxStatListView.DataBind();
		}
		public void FillGeneralStatListView()
		{
			//DataTable dt = VSWebBL.DashboardBL.Office365BL.Ins.FillStatListView("General",accountName);
			DataTable dt = VSWebBL.DashboardBL.Office365BL.Ins.FillStatListView("General", selectedAccName);
			if (dt.Rows.Count > 0)
			{
                //lblTotalLicensesValue.Text = dt.Rows[0].ItemArray[0].ToString();
                //lblTotalConsumedLicensesValue.Text = dt.Rows[0].ItemArray[1].ToString();
                //lblTotalWarningLicensesValue.Text = dt.Rows[0].ItemArray[2].ToString();
                //if (((lblTotalLicensesValue.Text != null) && (lblTotalLicensesValue.Text != "")) && ((lblTotalConsumedLicensesValue.Text != null) && (lblTotalConsumedLicensesValue.Text != "")))
                //{
                //    int totallicenses = Convert.ToInt32(lblTotalLicensesValue.Text);
                //    int totalConsumedlicenses = Convert.ToInt32(lblTotalConsumedLicensesValue.Text);
                //    int remainglicenses = totallicenses - totalConsumedlicenses;
                //    lblTotalRemainingLicensesValue.Text = Convert.ToString(remainglicenses);
                //}
			}
			//GeneralStatListView.DataSource = dt;
			//GeneralStatListView.DataBind();deviceTypeWebChart
		}
		public void FillMailboxChart(string Name)
		{
			SetGraphForDeviceType("No_of_Users", Name);

		}

        public void FillPwdSettingsGrid()
        {
            DataTable dt = new DataTable();
            dt = VSWebBL.DashboardBL.Office365BL.Ins.GetOffice365UserSettings(selectedAccName);
            try
            {
                Session["0365PwdSettings"] = dt;
                O365Usersettinggrid.DataSource = dt;
                O365Usersettinggrid.DataBind();
            }
            catch (Exception ex)
            {
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw;
            }
        }
		public void FillServiceDetailsgrid( string name)
		{
			DataTable dt = new DataTable();
			dt = VSWebBL.DashboardBL.Office365BL.Ins.GetOffice365UserServicedetails(selectedAccName);
			try
			{
				if (dt.Rows.Count > 0)
				{
					Session["0365servicedetails"] = dt;
					servicedetailsgrid.DataSource = dt;
					servicedetailsgrid.DataBind();
					servicedetailsgrid.DetailRows.ExpandRow(0);
				}
			}
			catch (Exception ex)
			{
				//6/27/2014 NS added for VSPLUS-634
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw;
			}
		}
		protected void chkSingleExpanded_CheckedChanged(object sender, EventArgs e)
		{
			servicedetailsgrid.SettingsDetail.AllowOnlyOneMasterRowExpanded = chkSingleExpanded.Checked;
			if (servicedetailsgrid.SettingsDetail.AllowOnlyOneMasterRowExpanded)
			{
				servicedetailsgrid.DetailRows.CollapseAllRows();
			}
			
			ASPxPageControl1.ActiveTabIndex = 7;
			
		}

		protected void deviceTypeWebChart_CustomCallback(object sender, DevExpress.XtraCharts.Web.CustomCallbackEventArgs e)
		{
			this.deviceTypeWebChart.Width = new Unit(Convert.ToInt32(chartWidth.Value));
		}
		
		public DataTable SetGraphForDeviceType(string SortField, string servername)
		{

			deviceTypeWebChart.Series.Clear();
			DataTable dt1 = VSWebBL.DashboardBL.Office365BL.Ins.SetGraphForMailFiles(servername);
			DataView dv = dt1.DefaultView;
			dv.Sort = "TotalItemSizeInGB asc";
			DataTable dt = dv.ToTable();
			Series series = new Series("Title", ViewType.Bar);

			series.ArgumentDataMember = dt.Columns["Title"].ToString();
			//10/30/2013 NS added - point labels on series
			series.LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;

			ValueDataMemberCollection seriesValueDataMembers = (ValueDataMemberCollection)series.ValueDataMembers;
			seriesValueDataMembers.AddRange(dt.Columns["TotalItemSizeInGB"].ToString());
			deviceTypeWebChart.Series.Add(series);

			deviceTypeWebChart.Legend.Visible = false;

			//((SideBySideBarSeriesView)series.View).ColorEach = true;

			//AxisBase axisx = ((XYDiagram)deviceTypeWebChart.Diagram).AxisX;

			XYDiagram seriesXY = (XYDiagram)deviceTypeWebChart.Diagram;
			// seriesXY.AxisX.Title.Text = "Milliseconds";
			// seriesXY.AxisX.Title.Visible = true;
			((DevExpress.XtraCharts.XYDiagram)deviceTypeWebChart.Diagram).Rotated = true;
			//11/16/2013 NS modified the AllowRotate option to false, otherwise the server names were unreadable
			seriesXY.AxisX.Label.ResolveOverlappingOptions.AllowRotate = false;
			seriesXY.AxisX.Label.ResolveOverlappingOptions.AllowHide = true;
			//seriesXY.AxisY.Title.Text = "Mailbox Size (GB)";
			//seriesXY.AxisY.Title.Visible = true;

			AxisBase axisy = ((XYDiagram)deviceTypeWebChart.Diagram).AxisY;
			//4/18/2014 NS modified for VSPLUS-312
			axisy.Range.AlwaysShowZeroLevel = true;
			double min = Convert.ToDouble(((XYDiagram)deviceTypeWebChart.Diagram).AxisY.Range.MinValue);
			double max = Convert.ToDouble(((XYDiagram)deviceTypeWebChart.Diagram).AxisY.Range.MaxValue);

			int gs = (int)((max - min) / 5);

			if (gs == 0)
			{
				gs = 1;
				seriesXY.AxisY.GridSpacingAuto = false;
				seriesXY.AxisY.GridSpacing = gs;
			}
			else
			{
				seriesXY.AxisY.GridSpacingAuto = true;
			}
			deviceTypeWebChart.DataSource = dt;
			deviceTypeWebChart.DataBind();
			ChartTitle title = new ChartTitle();
			if (servername == "All")
			{
				title.Text = "Top 5 Mailboxes";
			}
			else
			{
				title.Text = "Top 5 Mailboxes for " + servername;
			}
            title.Text = "Top 5 Mailboxes";
            System.Drawing.Font font = new System.Drawing.Font(title.Font.FontFamily.Name, 12);
            title.Font = font;
			deviceTypeWebChart.Titles.Clear();
			deviceTypeWebChart.Titles.Add(title);
			return dt;
		}

        public void FillDevicesChart()
        {
            DataTable dt = new DataTable();
            dt = VSWebBL.DashboardBL.Office365BL.Ins.SetGraphLastLogonUsers(selectedAccName);
			if (dt.Rows.Count > 0)
			{
				DataRow[] maxRow = dt.Select("No_of_Users= MAX(No_of_Users)");
				if (maxRow != null)
				{

					int maxVal = Convert.ToInt32(maxRow[0]["No_of_Users"]);


					if (maxVal > 0)
					{
						DevicesWebChart.Series.Clear();
						Series series2 = new Series("Duration", ViewType.Pie);
						for (int i = 0; i < dt.Rows.Count; i++)
						{
							series2.Points.Add(new SeriesPoint(dt.Rows[i]["Duration"], dt.Rows[i]["No_of_Users"]));
						}
						DevicesWebChart.Series.Add(series2);
						series2.Label.PointOptions.PointView = PointView.Argument;
						series2.ToolTipEnabled = DevExpress.Utils.DefaultBoolean.True;


						DevicesWebChart.Legend.Visible = false;
						ChartTitle title2 = new ChartTitle();
						title2 = new ChartTitle();
						title2.Text = "Last Logon";
						System.Drawing.Font font = new System.Drawing.Font(title2.Font.FontFamily.Name, 12);
						title2.Font = font;

						DevicesWebChart.Titles.Clear();
						DevicesWebChart.Titles.Add(title2);

						DevicesWebChart.DataSource = dt;
						DevicesWebChart.DataBind();
					}
					else
					{
					
						ChartTitle Title = new ChartTitle();
						Title.Text = "Last Logon";
						System.Drawing.Font font = new System.Drawing.Font(Title.Font.FontFamily.Name, 12);
						Title.Font = font;
						DevicesWebChart.Titles.Clear();
						DevicesWebChart.Titles.Add(Title);
						EmptyChartText myText = DevicesWebChart.EmptyChartText;

						myText.Antialiasing = true;
						myText.Text = "There is no data to display.";
						myText.TextColor = Color.Black;
                        myText.Font = font;
					}
				}
			}
			else
			{
				ChartTitle Title = new ChartTitle();
				Title.Text = "Last Logon";
				System.Drawing.Font font = new System.Drawing.Font(Title.Font.FontFamily.Name, 12);
				Title.Font = font;
				DevicesWebChart.Titles.Clear();
				DevicesWebChart.Titles.Add(Title);
				EmptyChartText myText = DevicesWebChart.EmptyChartText;

				myText.Antialiasing = true;
				myText.Text = "There is no data to display.";
				myText.TextColor = Color.Black;
                myText.Font = font;
			}
        }
		protected void ASPxMenu1_Init(object sender, EventArgs e)
		{

			string[][] arrOfMenuItems = {	
			new string[] {"Dashboard", "OverallHealth1.aspx"}
			//new string[] {"Skype for Business Stats", "Office365LyncHealth.aspx"}
			};

			DevExpress.Web.MenuItem item;
			foreach (string[] arr in arrOfMenuItems)
			{
				item = new DevExpress.Web.MenuItem();
				item.Text = arr[0];
				item.NavigateUrl = arr[1];

				ASPxMenu1.Items[0].Items.Add(item);
			}
		}
		
		//1/15/2015 NS added for VSPLUS-1316
		private void FillHealthAssessmentStatusGrid(string type, string servername)
		{
			try
			{
				string TypeAndName = servername + "-" + location;
				DataTable StatusTable12 = VSWebBL.DashboardBL.Office365BL.Ins.GetHealthAssessmentStatusDetails(TypeAndName, servername);

				Session["HealthAssessmentO365"] = StatusTable12;
				HealthAssessmentGrid.DataSource = StatusTable12;
				HealthAssessmentGrid.DataBind();
			}
			catch (Exception ex)
			{
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}
		}
		//1/15/2015 NS added for VSPLUS-1316
		public void FillHealthAssessmentFromSession()
		{
			try
			{
				if (Session["HealthAssessmentO365"] != null)
				{
					DataTable dt = Session["HealthAssessmentO365"] as DataTable;
					HealthAssessmentGrid.DataSource = dt;
					HealthAssessmentGrid.DataBind();
				}
			}
			catch (Exception ex)
			{
				//Response.Write("Error : " + ex);
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}
		}
		public DataTable SetGraphForDeviceType()
		{

			deviceTypeWebChart0.Series.Clear();
			// DataTable dt = VSWebBL.DashboardBL.Office365BL.Ins.SetGraphDeviceTypes(accountName);
			DataTable dt = VSWebBL.DashboardBL.Office365BL.Ins.SetGraphDeviceTypes(selectedAccName);
			Series series = new Series("DeviceType", ViewType.Bar);

			series.ArgumentDataMember = dt.Columns["DeviceType"].ToString();
			//10/30/2013 NS added - point labels on series
			series.LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;

			ValueDataMemberCollection seriesValueDataMembers = (ValueDataMemberCollection)series.ValueDataMembers;
			seriesValueDataMembers.AddRange(dt.Columns["No_of_Users"].ToString());
			deviceTypeWebChart0.Series.Add(series);
			deviceTypeWebChart0.Legend.Visible = false;

			((SideBySideBarSeriesView)series.View).ColorEach = true;

			//AxisBase axisx = ((XYDiagram)deviceTypeWebChart0.Diagram).AxisX;

			XYDiagram seriesXY = (XYDiagram)deviceTypeWebChart0.Diagram;
			// seriesXY.AxisX.Title.Text = "Milliseconds";
			// seriesXY.AxisX.Title.Visible = true;
			((DevExpress.XtraCharts.XYDiagram)deviceTypeWebChart0.Diagram).Rotated = true;
			//11/16/2013 NS modified the AllowRotate option to false, otherwise the server names were unreadable
			seriesXY.AxisX.Label.ResolveOverlappingOptions.AllowRotate = false;
			seriesXY.AxisX.Label.ResolveOverlappingOptions.AllowHide = false;
			//seriesXY.AxisY.Title.Text = "Users";
            System.Drawing.Font font = new System.Drawing.Font(seriesXY.AxisY.Title.Font.FontFamily.Name, 10);
            seriesXY.AxisY.Title.Font = font;
			//seriesXY.AxisY.Title.Visible = true;
            //25/2/2016 Sowmya Added for VSPLUS 2636
            ((DevExpress.XtraCharts.XYDiagram)deviceTypeWebChart0.Diagram).AxisY.Label.Angle = 360;
			AxisBase axisy = ((XYDiagram)deviceTypeWebChart0.Diagram).AxisY;
            UI uiobj = new UI();
            uiobj.RecalibrateChartAxes(deviceTypeWebChart0.Diagram, "Y", "int", "int");
			//2/6/2013 NS modified chart height calculations based on the number of rows
			if (dt.Rows.Count > 0)
			{
				if (dt.Rows.Count < 6)
				{
					deviceTypeWebChart0.Height = 200;
				}
				else
				{
					if (dt.Rows.Count >= 6 && dt.Rows.Count < 12)
					{
						deviceTypeWebChart0.Height = ((dt.Rows.Count) * 50) + 20;
					}
					else
					{
						if (dt.Rows.Count >= 12 && dt.Rows.Count < 100)
						{
							deviceTypeWebChart0.Height = ((dt.Rows.Count) * 40) + 20;
						}
						else
						{
							if (dt.Rows.Count >= 100)
							{
								deviceTypeWebChart0.Height = ((dt.Rows.Count) * 20) + 20;
							}
						}
					}
				}
			}
			//ResponseWebChartControl.Width = new Unit(1000);
			deviceTypeWebChart0.DataSource = dt;
			deviceTypeWebChart0.DataBind();
			ChartTitle title = new ChartTitle();
            //25/2/2016 Sowmya Added for VSPLUS 2636
            title.Text = "Device Types";
            font = new System.Drawing.Font(title.Font.FontFamily.Name, 12);
            title.Font = font;
			deviceTypeWebChart0.Titles.Clear();
			deviceTypeWebChart0.Titles.Add(title);
			return dt;
		}
		public DataTable SetGraphP2PSessions()
		{

			P2PSessionsChart1.Series.Clear();
			//DataTable dt = VSWebBL.DashboardBL.Office365BL.Ins.SetGraphP2PSessions(accountName);
			DataTable dt = VSWebBL.DashboardBL.Office365BL.Ins.SetGraphP2PSessions(selectedAccName);
			Series series = new Series("usercount", ViewType.Bar);

			series.ArgumentDataMember = dt.Columns["DeviceType"].ToString();
			//10/30/2013 NS added - point labels on series
			series.LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;

			ValueDataMemberCollection seriesValueDataMembers = (ValueDataMemberCollection)series.ValueDataMembers;
			seriesValueDataMembers.AddRange(dt.Columns["No_of_Users"].ToString());
			P2PSessionsChart1.Series.Add(series);

			P2PSessionsChart1.Legend.Visible = false;

			((SideBySideBarSeriesView)series.View).ColorEach = true;

			//AxisBase axisx = ((XYDiagram)deviceTypeWebChart.Diagram).AxisX;

			XYDiagram seriesXY = (XYDiagram)P2PSessionsChart1.Diagram;
			// seriesXY.AxisX.Title.Text = "Milliseconds";
			// seriesXY.AxisX.Title.Visible = true;
			((DevExpress.XtraCharts.XYDiagram)P2PSessionsChart1.Diagram).Rotated = true;
			//11/16/2013 NS modified the AllowRotate option to false, otherwise the server names were unreadable
			seriesXY.AxisX.Label.ResolveOverlappingOptions.AllowRotate = false;
			seriesXY.AxisX.Label.ResolveOverlappingOptions.AllowHide = false;
			//seriesXY.AxisY.Title.Text = "Users";
            System.Drawing.Font font = new System.Drawing.Font(seriesXY.AxisY.Title.Font.FontFamily.Name, 10);
            seriesXY.AxisY.Title.Font = font;
			//seriesXY.AxisY.Title.Visible = true;
            //25/2/2016 Sowmya Added for VSPLUS 2636
            ((DevExpress.XtraCharts.XYDiagram)P2PSessionsChart1.Diagram).AxisY.Label.Angle = 360;
     		AxisBase axisy = ((XYDiagram)P2PSessionsChart1.Diagram).AxisY;
            UI uiobj = new UI();
            uiobj.RecalibrateChartAxes(P2PSessionsChart1.Diagram, "Y", "int", "int");
            //2/6/2013 NS modified chart height calculations based on the number of rows
			if (dt.Rows.Count > 0)
			{
				if (dt.Rows.Count < 6)
				{
					P2PSessionsChart1.Height = 200;
				}
				else
				{
					if (dt.Rows.Count >= 6 && dt.Rows.Count < 12)
					{
						P2PSessionsChart1.Height = ((dt.Rows.Count) * 50) + 20;
					}
					else
					{
						if (dt.Rows.Count >= 12 && dt.Rows.Count < 100)
						{
							P2PSessionsChart1.Height = ((dt.Rows.Count) * 40) + 20;
						}
						else
						{
							if (dt.Rows.Count >= 100)
							{
								P2PSessionsChart1.Height = ((dt.Rows.Count) * 20) + 20;
							}
						}
					}
				}
			}
			//ResponseWebChartControl.Width = new Unit(1000);
			P2PSessionsChart1.DataSource = dt;
			P2PSessionsChart1.DataBind();
			ChartTitle title = new ChartTitle();

			title.Text = "P2P Sessions";
            font = new System.Drawing.Font(title.Font.FontFamily.Name, 12);
            title.Font = font;
			P2PSessionsChart1.Titles.Clear();
			P2PSessionsChart1.Titles.Add(title);
			return dt;
		}
		public DataTable SetGraphForAVSessions()
		{
			// DataTable dt = VSWebBL.DashboardBL.Office365BL.Ins.SetGraphAVSessions(accountName);
			DataTable dt = VSWebBL.DashboardBL.Office365BL.Ins.SetGraphAVSessions(selectedAccName);
			if (dt.Rows.Count > 0)
			{
				DataRow[] maxRow = dt.Select("No_of_Users= MAX(No_of_Users)");
				if (maxRow != null)
				{

					int maxVal = Convert.ToInt32(maxRow[0]["No_of_Users"]);


					if (maxVal > 0)
					{
						AVSessionsChart.Series.Clear();
						Series series = new Series("OSType", ViewType.Pie);

						for (int i = 0; i < dt.Rows.Count; i++)
						{
							series.Points.Add(new SeriesPoint(dt.Rows[i]["DeviceType"], dt.Rows[i]["No_of_Users"]));
						}
						AVSessionsChart.Series.Add(series);
						series.Label.PointOptions.PointView = PointView.Argument;
						series.ToolTipEnabled = DevExpress.Utils.DefaultBoolean.True;
						//series.Label.PointOptions.ValueNumericOptions.Format = NumericFormat.General;
						// series.Label.PointOptions.ValueNumericOptions.Precision = 0;
						AVSessionsChart.Legend.Visible = false;
						ChartTitle title = new ChartTitle();
						title.Text = "AV Sessions";
						System.Drawing.Font font = new System.Drawing.Font(title.Font.FontFamily.Name, 12);
						title.Font = font;
						AVSessionsChart.Titles.Clear();
						AVSessionsChart.Titles.Add(title);
						AVSessionsChart.DataSource = dt;
						AVSessionsChart.DataBind();

					}
					else
					{

						ChartTitle Title = new ChartTitle();
						Title.Text = "AV Sessions";
						System.Drawing.Font font = new System.Drawing.Font(Title.Font.FontFamily.Name, 12);
						Title.Font = font;
						AVSessionsChart.Titles.Clear();
						AVSessionsChart.Titles.Add(Title);
						EmptyChartText myText = AVSessionsChart.EmptyChartText;

						myText.Antialiasing = true;
						myText.Text = "There is no data to display.";
						myText.TextColor = Color.Black;
                        myText.Font = font;
					}
				}
			}

			else
			{

				ChartTitle Title = new ChartTitle();
				Title.Text = "AV Sessions";
				System.Drawing.Font font = new System.Drawing.Font(Title.Font.FontFamily.Name, 12);
				Title.Font = font;
				AVSessionsChart.Titles.Clear();
				AVSessionsChart.Titles.Add(Title);
				EmptyChartText myText = AVSessionsChart.EmptyChartText;
				myText.Antialiasing = true;
				myText.Text = "There is no data to display.";
				myText.TextColor = Color.Black;
                myText.Font = font;
			}
				
			
			return dt;
		}
		public DataTable SetGraphConfReport()
		{
			// DataTable dt = VSWebBL.DashboardBL.Office365BL.Ins.SetGraphConfReport(accountName);
			DataTable dt = VSWebBL.DashboardBL.Office365BL.Ins.SetGraphConfReport(selectedAccName);
			if (dt.Rows.Count > 0)
			{
				DataRow[] maxRow = dt.Select("No_of_Users= MAX(No_of_Users)");
				if (maxRow != null)
				{

					int maxVal = Convert.ToInt32(maxRow[0]["No_of_Users"]);


					if (maxVal > 0)
					{
						ConfReportChart.Series.Clear();
						Series series = new Series("DeviceCount", ViewType.Pie);
						for (int i = 0; i < dt.Rows.Count; i++)
						{
							series.Points.Add(new SeriesPoint(dt.Rows[i]["DeviceType"], dt.Rows[i]["No_Of_users"]));
						}
						ConfReportChart.Series.Add(series);
						series.Label.PointOptions.PointView = PointView.Argument;
						series.ToolTipEnabled = DevExpress.Utils.DefaultBoolean.True;
						//series.Label.PointOptions.ValueNumericOptions.Format = NumericFormat.General;
						// series.Label.PointOptions.ValueNumericOptions.Precision = 0;

						ConfReportChart.Legend.Visible = false;
						ChartTitle title = new ChartTitle();

						title.Text = "Conference Report";
						System.Drawing.Font font = new System.Drawing.Font(title.Font.FontFamily.Name, 12);
						title.Font = font;
						ConfReportChart.Titles.Clear();
						ConfReportChart.Titles.Add(title);

						ConfReportChart.DataSource = dt;
						ConfReportChart.DataBind();
					}
					else
					{
						ChartTitle title = new ChartTitle();

						title.Text = "Conference Report";
						System.Drawing.Font font = new System.Drawing.Font(title.Font.FontFamily.Name, 12);
						title.Font = font;
						ConfReportChart.Titles.Clear();
						ConfReportChart.Titles.Add(title);
						EmptyChartText myText = ConfReportChart.EmptyChartText;
						myText.Antialiasing = true;
						myText.Text = "There is no data to display.";
						myText.TextColor = Color.Black;
                        myText.Font = font;
					}
				}
			}
			else
			{
				ChartTitle title = new ChartTitle();

				title.Text = "Conference Report";
				System.Drawing.Font font = new System.Drawing.Font(title.Font.FontFamily.Name, 12);
				title.Font = font;
				ConfReportChart.Titles.Clear();
				ConfReportChart.Titles.Add(title);
				EmptyChartText myText = ConfReportChart.EmptyChartText;
				myText.Antialiasing = true;
				myText.Text = "There is no data to display.";
				myText.TextColor = Color.Black;
                myText.Font = font;
			}
			return dt;
		}
		public DataTable SetGraphForMailTests()
		{
			MailTestsWebChart.Series.Clear();
			DataTable dt = VSWebBL.DashboardBL.Office365BL.Ins.GetMailTestsResponseTimes(NodeName, selectedAccName);
			MailTestsWebChart.SeriesDataMember = "StatName";
			MailTestsWebChart.SeriesTemplate.ArgumentDataMember = "Date";
			MailTestsWebChart.SeriesTemplate.ValueDataMembers.AddRange(new string[] { "StatValue" });
            ((LineSeriesView)MailTestsWebChart.SeriesTemplate.View).LineMarkerOptions.Size = 7;
            ChartTitle title = new ChartTitle();

			title.Text = "Mail Services";
            System.Drawing.Font font = new System.Drawing.Font(title.Font.FontFamily.Name, 12);
            title.Font = font;
			
			MailTestsWebChart.Titles.Clear();
			MailTestsWebChart.Titles.Add(title);

			MailTestsWebChart.DataSource = dt;
			MailTestsWebChart.DataBind();
            for (int i = 0; i < MailTestsWebChart.Series.Count; i++)
            {
                MailTestsWebChart.Series[i].CrosshairLabelPattern = "{A}: {V:n1}";
            }
            return dt;
		}
		public DataTable SetGraphForMailScenarioTests()
		{
			MailTestScenarioWebChart.Series.Clear();
			DataTable dt = VSWebBL.DashboardBL.Office365BL.Ins.GetMailScenarioTestsResponseTimes(NodeName,selectedAccName);
			MailTestScenarioWebChart.SeriesDataMember = "StatName";
			MailTestScenarioWebChart.SeriesTemplate.ArgumentDataMember = "Date";
			MailTestScenarioWebChart.SeriesTemplate.ValueDataMembers.AddRange(new string[] { "StatValue" });
            ((LineSeriesView)MailTestScenarioWebChart.SeriesTemplate.View).LineMarkerOptions.Size = 7;
			ChartTitle title = new ChartTitle();

			title.Text = "Mail Scenario Tests";
            System.Drawing.Font font = new System.Drawing.Font(title.Font.FontFamily.Name, 12);
            title.Font = font;

			MailTestScenarioWebChart.Titles.Clear();
			MailTestScenarioWebChart.Titles.Add(title);

			MailTestScenarioWebChart.DataSource = dt;
			MailTestScenarioWebChart.DataBind();
			return dt;
		}
		public DataTable SetGraphForOneDriveStats()
		{
			OneDriveWebChart.Series.Clear();
			DataTable dt = VSWebBL.DashboardBL.Office365BL.Ins.GetOneDriveStats(NodeName,selectedAccName);
			OneDriveWebChart.SeriesDataMember = "StatName";
			OneDriveWebChart.SeriesTemplate.ArgumentDataMember = "Date";
			OneDriveWebChart.SeriesTemplate.ValueDataMembers.AddRange(new string[] { "StatValue" });
            ((LineSeriesView)OneDriveWebChart.SeriesTemplate.View).LineMarkerOptions.Size = 7;
			ChartTitle title = new ChartTitle();

			title.Text = "OneDrive Tests";
            System.Drawing.Font font = new System.Drawing.Font(title.Font.FontFamily.Name, 12);
            title.Font = font;

			OneDriveWebChart.Titles.Clear();
			OneDriveWebChart.Titles.Add(title);

			OneDriveWebChart.DataSource = dt;
			OneDriveWebChart.DataBind();
			return dt;
		}
		public DataTable SetGraphForSiteTests()
		{
			SiteTestsWebChart.Series.Clear();
			DataTable dt = VSWebBL.DashboardBL.Office365BL.Ins.GetSiteTestsResponseTimes(NodeName,selectedAccName);
			SiteTestsWebChart.SeriesDataMember = "StatName";
			SiteTestsWebChart.SeriesTemplate.ArgumentDataMember = "Date";
			SiteTestsWebChart.SeriesTemplate.ValueDataMembers.AddRange(new string[] { "StatValue" });
            ((LineSeriesView)SiteTestsWebChart.SeriesTemplate.View).LineMarkerOptions.Size = 7;
			ChartTitle title = new ChartTitle();

			title.Text = "Site Tests";
            System.Drawing.Font font = new System.Drawing.Font(title.Font.FontFamily.Name, 12);
            title.Font = font;

			SiteTestsWebChart.Titles.Clear();
			SiteTestsWebChart.Titles.Add(title);

			SiteTestsWebChart.DataSource = dt;
			SiteTestsWebChart.DataBind();
			return dt;
		}
		public DataTable SetGraphForTaskFolderTests()
		{
			TaskFolderTestsWebChart.Series.Clear();
			DataTable dt = VSWebBL.DashboardBL.Office365BL.Ins.GetTaskFolderTestsResponseTimes(NodeName,selectedAccName);
			TaskFolderTestsWebChart.SeriesDataMember = "StatName";
			TaskFolderTestsWebChart.SeriesTemplate.ArgumentDataMember = "Date";
			TaskFolderTestsWebChart.SeriesTemplate.ValueDataMembers.AddRange(new string[] { "StatValue" });
            ((LineSeriesView)TaskFolderTestsWebChart.SeriesTemplate.View).LineMarkerOptions.Size = 7;
			ChartTitle title = new ChartTitle();

			title.Text = "Folder Creation Test";
            System.Drawing.Font font = new System.Drawing.Font(title.Font.FontFamily.Name, 12);
            title.Font = font;

			TaskFolderTestsWebChart.Titles.Clear();
			TaskFolderTestsWebChart.Titles.Add(title);

			TaskFolderTestsWebChart.DataSource = dt;
			TaskFolderTestsWebChart.DataBind();
			return dt;
		}
		protected void deviceTypeWebChart0_CustomCallback(object sender, DevExpress.XtraCharts.Web.CustomCallbackEventArgs e)
		{
			deviceTypeWebChart0.Width = new Unit(Convert.ToInt32(chartWidth.Value));
		}
		protected void P2PSessionsChart1_CustomCallback(object sender, DevExpress.XtraCharts.Web.CustomCallbackEventArgs e)
		{
			P2PSessionsChart1.Width = new Unit(Convert.ToInt32(chartWidth.Value));
		}
		protected void AVSessionsChart_CustomCallback(object sender, DevExpress.XtraCharts.Web.CustomCallbackEventArgs e)
		{
			AVSessionsChart.Width = new Unit(Convert.ToInt32(chartWidth.Value));
		}
		protected void ConfReportChart_CustomCallback(object sender, DevExpress.XtraCharts.Web.CustomCallbackEventArgs e)
		{
			ConfReportChart.Width = new Unit(Convert.ToInt32(chartWidth.Value));
		}
		protected void MailTestsWebChart_CustomCallback(object sender, DevExpress.XtraCharts.Web.CustomCallbackEventArgs e)
		{
			MailTestsWebChart.Width = new Unit(Convert.ToInt32(chartWidth.Value));
		}
		protected void DirSyncWebChart_CustomCallback(object sender, DevExpress.XtraCharts.Web.CustomCallbackEventArgs e)
		{
			//DirSyncWebChart.Width = new Unit(Convert.ToInt32(chartWidth.Value));
		}
		protected void DevicesWebChart_CustomCallback(object sender, DevExpress.XtraCharts.Web.CustomCallbackEventArgs e)
		{
			DevicesWebChart.Width = new Unit(Convert.ToInt32(chartWidth.Value));
		}
		protected void MailTestScenarioWebChart_CustomCallback(object sender, DevExpress.XtraCharts.Web.CustomCallbackEventArgs e)
		{
			MailTestScenarioWebChart.Width = new Unit(Convert.ToInt32(chartWidth.Value));
		}
		protected void SiteTestsWebChart_CustomCallback(object sender, DevExpress.XtraCharts.Web.CustomCallbackEventArgs e)
		{
			SiteTestsWebChart.Width = new Unit(Convert.ToInt32(chartWidth.Value));
		}
		protected void TaskFolderTestsWebChart_CustomCallback(object sender, DevExpress.XtraCharts.Web.CustomCallbackEventArgs e)
		{
			TaskFolderTestsWebChart.Width = new Unit(Convert.ToInt32(chartWidth.Value));
		}
		protected void OneDriveWebChart_CustomCallback(object sender, DevExpress.XtraCharts.Web.CustomCallbackEventArgs e)
		{
			OneDriveWebChart.Width = new Unit(Convert.ToInt32(chartWidth.Value));
		}
		protected void HealthAssessmentGrid_HtmlDataCellPrepared(object sender, DevExpress.Web.ASPxGridViewTableDataCellEventArgs e)
		{
			if (e.DataColumn.FieldName == "Result" && e.CellValue.ToString() == "Pass")
			{
				e.Cell.BackColor = System.Drawing.Color.LightGreen;
			}
			else if (e.DataColumn.FieldName == "Result" && e.CellValue.ToString() == "Fail")
			{
				e.Cell.BackColor = System.Drawing.Color.Red;
				e.Cell.ForeColor = System.Drawing.Color.White;
			}
		}
		//1/28/2015 NS added for VSPLUS-1404
    
		protected void HealthAssessmentGrid_PageSizeChanged(object sender, EventArgs e)
		{
            //23/2/2016 Sowmya Added for VSPLUS 2637
            VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("Office365Health|HealthAssessmentGrid", HealthAssessmentGrid.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
			Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
		}
		protected void officehealthgrid_HtmlRowCreated(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
		{
			e.Row.Attributes.Add("onmouseover", "this.originalcolor=this.style.backgroundColor;" + "this.style.backgroundColor='#C0C0C0';");

			e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalcolor;");
		}
		protected void ASPxPageControl1_ActiveTabChanged(object source, TabControlEventArgs e)
		{

		}
        
        //2/26/2016 NS added for VSPLUS-2648
        private void GetO365Tests(string accountName)
        {
            DataTable dt = VSWebBL.DashboardBL.Office365BL.Ins.GetOffice365Tests(accountName);
            if (dt.Rows.Count > 0)
            {
                officehealthgrid.DataSource = dt;
                officehealthgrid.Columns.Clear();
                officehealthgrid.AutoGenerateColumns = true;
                officehealthgrid.KeyFieldName = "Id";
                officehealthgrid.DataBind();
            }
        }
        //2/26/2016 NS added for VSPLUS-2648
        protected void officehealthgrid_DataBound(object sender, EventArgs e)
        {
            for (int i = 0; i < officehealthgrid.Columns.Count; i++)
            {
                GridViewColumn c = officehealthgrid.Columns[i];
                GridViewDataColumn dataColumn = c as GridViewDataColumn;
                if (dataColumn.FieldName == "Id" || dataColumn.FieldName == "Mode")
                {
                    c.Visible = false;
                }
                GridViewDataDateColumn dateColumn = c as GridViewDataDateColumn;
                if (dateColumn != null)
                {
                    if (dateColumn.FieldName == "LastUpdate")
                    {
                        dateColumn.PropertiesDateEdit.DisplayFormatString = "G";
                        dateColumn.Width = new Unit("140px");
                    }
                }
            }
            //officehealthgrid.Width = new System.Web.UI.WebControls.Unit("100%");
        }
        //3/9/2016 NS added for VSPLUS-2648
        public DataTable SetGraphForMailBoxTypes()
        {
            MailBoxTypeWebChart.Series.Clear();
            DataTable dt = VSWebBL.DashboardBL.Office365BL.Ins.GetMailBoxTypes(selectedAccName);
            Series series = new Series("StatName", ViewType.Doughnut);

            series.ArgumentDataMember = dt.Columns["StatName"].ToString();
            series.LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;
            DoughnutSeriesLabel label = (DoughnutSeriesLabel)series.Label;
            label.TextPattern = "{A}: {VP:P0}";
            series.LegendTextPattern = "{A}: {V}";

            ValueDataMemberCollection seriesValueDataMembers = (ValueDataMemberCollection)series.ValueDataMembers;
            seriesValueDataMembers.AddRange(dt.Columns["StatValue"].ToString());
            MailBoxTypeWebChart.Series.Add(series);

            XYDiagram seriesXY = (XYDiagram)deviceTypeWebChart.Diagram;
            seriesXY.AxisX.Label.ResolveOverlappingOptions.AllowRotate = false;
            seriesXY.AxisX.Label.ResolveOverlappingOptions.AllowHide = true;
            
            //ChartTitle title = new ChartTitle();

            //title.Text = "Mail Box Types";

            //MailBoxTypeWebChart.Titles.Clear();
            //MailBoxTypeWebChart.Titles.Add(title);

            MailBoxTypeWebChart.DataSource = dt;
            MailBoxTypeWebChart.DataBind();
            return dt;
        }

        public DataTable SetGraphForUsersDaily()
        {
            string fromdate = "";
            string todate = "";
            fromdate = dtPick.FromDate;
            todate = dtPick.ToDate;
            UsersDailyWebChart.Series.Clear();
            DataTable dt = VSWebBL.DashboardBL.Office365BL.Ins.GetUsersDailyCount("ActiveUsersCount", selectedAccName,fromdate,todate);
            ChartTitle title = new ChartTitle();

            title.Text = "Daily User Logins";
            System.Drawing.Font font = new System.Drawing.Font(title.Font.FontFamily.Name, 12);
            title.Font = font;

            UsersDailyWebChart.Titles.Clear();
            UsersDailyWebChart.Titles.Add(title);
            if (dt.Rows.Count > 0)
            {
                UsersDailyWebChart.SeriesDataMember = "StatName";
                UsersDailyWebChart.SeriesTemplate.ArgumentDataMember = "Date";
                UsersDailyWebChart.SeriesTemplate.ValueDataMembers.AddRange(new string[] { "StatValue" });
                ((LineSeriesView)UsersDailyWebChart.SeriesTemplate.View).LineMarkerOptions.Size = 7;
              

                UsersDailyWebChart.DataSource = dt;
                UsersDailyWebChart.DataBind();
            }
            else
            {

                EmptyChartText myText = UsersDailyWebChart.EmptyChartText;

                myText.Antialiasing = true;
                myText.Text = "There is no data to display.";
                myText.TextColor = Color.Black;
             
            }
            return dt;
        }

        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            SetGraphForUsersDaily();
        }

        private void SetGraphForActiveInactiveUsers()
        {
            InactiveUsersWebChart.Series.Clear();
            DataTable dt = VSWebBL.DashboardBL.Office365BL.Ins.GetActiveInactiveUsers(selectedAccName);
            if (dt.Rows.Count > 0)
            {
                Series series = new Series("StatName", ViewType.Doughnut);

                series.ArgumentDataMember = dt.Columns["StatName"].ToString();
                series.LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;
                DoughnutSeriesLabel label = (DoughnutSeriesLabel)series.Label;
                label.TextPattern = "{A}: {VP:P0}";
                series.LegendTextPattern = "{A}: {V}";

                ValueDataMemberCollection seriesValueDataMembers = (ValueDataMemberCollection)series.ValueDataMembers;
                seriesValueDataMembers.AddRange(dt.Columns["StatValue"].ToString());
                InactiveUsersWebChart.Series.Add(series);

                XYDiagram seriesXY = (XYDiagram)deviceTypeWebChart.Diagram;
                seriesXY.AxisX.Label.ResolveOverlappingOptions.AllowRotate = false;
                seriesXY.AxisX.Label.ResolveOverlappingOptions.AllowHide = true;

                InactiveUsersWebChart.DataSource = dt;
                InactiveUsersWebChart.DataBind();
            
            }
        }

        public void SetGraphForInactiveMailboxes()
        {
            InactiveMailboxesWebChart.Series.Clear();
            DataTable dt1 = VSWebBL.DashboardBL.Office365BL.Ins.GetInactiveMailboxes(selectedAccName);
            DataView dv = dt1.DefaultView;
            dv.Sort = "TotalDaysInactive ASC";
            DataTable dt = dv.ToTable();
            Series series = new Series("Title", ViewType.Bar);

            series.ArgumentDataMember = dt.Columns["Title"].ToString();
            series.LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;

            ValueDataMemberCollection seriesValueDataMembers = (ValueDataMemberCollection)series.ValueDataMembers;
            seriesValueDataMembers.AddRange(dt.Columns["TotalDaysInactive"].ToString());
            InactiveMailboxesWebChart.Series.Add(series);

            XYDiagram seriesXY = (XYDiagram)deviceTypeWebChart.Diagram;
            seriesXY.AxisX.Label.ResolveOverlappingOptions.AllowRotate = false;
            seriesXY.AxisX.Label.ResolveOverlappingOptions.AllowHide = true;
            InactiveMailboxesWebChart.DataSource = dt;
            InactiveMailboxesWebChart.DataBind();
            ChartTitle title = new ChartTitle();
            title.Text = "Top 5 Inactive Mailboxes for " + selectedAccName;
            title.Text = "Top 5 Inactive Mailboxes";
            System.Drawing.Font font = new System.Drawing.Font(title.Font.FontFamily.Name, 12);
            title.Font = font;
            InactiveMailboxesWebChart.Titles.Clear();
            InactiveMailboxesWebChart.Titles.Add(title);
        }

        public void SetGraphForPwdExpSettings()
        {
            DataTable dt = new DataTable();
            dt = VSWebBL.DashboardBL.Office365BL.Ins.GetOffice365PwdExpSettings(selectedAccName);
            if (dt.Rows.Count > 0)
            {
                Passwordneverexpires.Series.Clear();
                Series series = new Series("Pie1", ViewType.Pie);
                Passwordneverexpires.Series.Add(series);
                Passwordneverexpires.Series["Pie1"].DataSource = dt;
                Passwordneverexpires.Series["Pie1"].ArgumentDataMember = "PasswordNeverExpires";
                Passwordneverexpires.Series["Pie1"].ValueDataMembers.AddRange(new string[] { "CountEach" });
                series.LegendTextPattern = "{A}: {V}";
                Passwordneverexpires.DataSource = dt;
            }
        }

        public void SetGraphForPwdStrongSettings()
        {
            DataTable dt = new DataTable();
            dt = VSWebBL.DashboardBL.Office365BL.Ins.GetOffice365PwdStrongSettings(selectedAccName);
            if (dt.Rows.Count > 0)
            {
                strongpasswordWebChart.Series.Clear();
                Series series = new Series("Pie1", ViewType.Pie);
                strongpasswordWebChart.Series.Add(series);
                strongpasswordWebChart.Series["Pie1"].DataSource = dt;
                strongpasswordWebChart.Series["Pie1"].ArgumentDataMember = "StrongPasswordRequired";
                strongpasswordWebChart.Series["Pie1"].ValueDataMembers.AddRange(new string[] { "CountEach" });
                series.LegendTextPattern = "{A}: {V}";
                strongpasswordWebChart.DataSource = dt;
            }
        }
        // 6/6/2016 Durga Addded for VSPLUS-3001

        public void FillLicensesInfo()
        {
            DataTable dt = new DataTable();
            dt = VSWebBL.DashboardBL.Office365BL.Ins.GetLicensesInfo(selectedAccName);
            if (dt.Rows.Count > 0)
            {
                TotalLicenseslb.Text = "Total Licenses:" + " " + dt.Rows[0]["ActiveUnits"].ToString();

                MonthlyLicenseCostlb.Text = "Monthly License Cost:" + " " + (Convert.ToInt32(dt.Rows[0]["ActiveUnits"]?? "0") * Convert.ToInt32(dt.Rows[0]["Costperuser"] ?? "0"));
                UnassignedLicenseslb.Text = "Unassigned Licenses:" + " " + (Convert.ToInt32(dt.Rows[0]["ActiveUnits"] ?? "0") - Convert.ToInt32(dt.Rows[0]["ConsumedUnits"] ?? "0"));

                int costofUnassignedlicenses = (Convert.ToInt32(dt.Rows[0]["ActiveUnits"] ?? "0") - Convert.ToInt32(dt.Rows[0]["ConsumedUnits"]?? "0")) * (Convert.ToInt32(dt.Rows[0]["Costperuser"] ?? "0"));
                costofUnassignedlicenseslb.Text = "Monthly cost of Unassigned licenses:" + " " + costofUnassignedlicenses.ToString();
            }
            DataTable Inactiveusersdt = new DataTable();
            Inactiveusersdt = VSWebBL.DashboardBL.Office365BL.Ins.GetInactiveUsersCount(selectedAccName);
            if (Inactiveusersdt.Rows.Count > 0)
            {
                InactiveUserslb.Text = "Inactive Users:" + " " + Inactiveusersdt.Rows[0]["count"].ToString();
                if (dt.Rows.Count > 0)
                {
                    costofinactiveuserslb.Text = "Monthly cost of inactive users:" + " " + Convert.ToInt32(Inactiveusersdt.Rows[0]["count"] ?? "0") * Convert.ToInt32(dt.Rows[0]["Costperuser"] ?? "0");
                }
                 }
        }
	}
}