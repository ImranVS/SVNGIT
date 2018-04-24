using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DevExpress.XtraCharts;
using System.Drawing;
using DevExpress.Web.ASPxScheduler;
using DevExpress.XtraScheduler;
using DevExpress.XtraScheduler.Xml;
using DevExpress.Web;
using System.Text;
using System.Windows.Forms;
using System.Web.UI.HtmlControls;
//using VSWebBL;

namespace VSWebUI.Configurator
{
    public partial class NetworkServerDetails : System.Web.UI.Page
    {
		protected void Page_PreInit(object sender, EventArgs e)
		{

			this.Master.contentCallEvent += new EventHandler(Master_ButtonClick);
		}

		private void Master_ButtonClick(object sender, EventArgs e)
		{

		}
        string url = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            //11/3/2014 NS added
            //this.performanceWebChartControl.Width = new Unit(Convert.ToInt32(chartWidth.Value));
			string type = "";
            if (!IsPostBack && !IsCallback)
            {
                //11/3/2014 NS added
                HtmlGenericControl body = (HtmlGenericControl)Master.FindControl("Body1");
                body.Attributes.Add("onload", "DoCallback()");
                if (Request.QueryString["Type"] != "" && Request.QueryString["Type"] != null)
                {
					type = Request.QueryString["Type"].ToString();
                    lblServerType.Text = Request.QueryString["Type"].ToString() + ":  ";
                }
                if (Request.QueryString["Typ"] != "" && Request.QueryString["Typ"] != null)
                {
					type = Request.QueryString["Typ"].ToString();
                    lblServerType.Text = Request.QueryString["Typ"].ToString();
                }

                if (Request.QueryString["Type"] == "URL" || Request.QueryString["Type"] == "Network Device")
                {
                   //11/3/2014 NS commented out
                    //performanceASPxRoundPanel.Width = 1000;
                    //performanceWebChartControl.Width = 800;
                    //ASPxPageControl1.TabPages[1].ClientVisible = false;
                    //ASPxPageControl1.TabPages[2].ClientVisible = false;
                }
                //9/19/2014 NS added for VSPLUS-934
                if (Request.QueryString["Status"] != "" && Request.QueryString["Status"] != null)
                {
                    serverstatus.InnerHtml = Request.QueryString["Status"];
                    if (Request.QueryString["Status"] == "OK" || Request.QueryString["Status"] == "Scanning")
                    {
                        serverstatus.Attributes["class"] = "OK";
                    }
                    else if (Request.QueryString["Status"] == "Not Responding")
                    {
                        serverstatus.Attributes["class"] = "NotResponding";
                    }
                    else if (Request.QueryString["Status"] == "Maintenance")
                    {
                        serverstatus.Attributes["class"] = "Maintenance";
                    }
                    else
                    {
                        serverstatus.Attributes["class"] = "Issue";
                    }
                }

                if (Request.QueryString["Name"] != "" && Request.QueryString["Name"] != null)
                {
                    servernamelbl.Text = Request.QueryString["Name"];
                    //9/19/2014 NS added for VSPLUS-934
                    servernamelbldisp.InnerHtml = lblServerType.Text + servernamelbl.Text;
                   
                }
				//if (Request.QueryString["LastDate"] != "" && Request.QueryString["LastDate"] != null)
				//{
				//    //9/19/2014 NS modified for VSPLUS-934
				//    //Lastscanned.Text = Request.QueryString["LastDate"].ToString();
				//    Lastscanned.InnerHtml = "Last scan date: " + Request.QueryString["LastDate"].ToString();
				//}
				//else
				//{
				DataTable dt = VSWebBL.DashboardBL.NetworkDeviceDetailsBL.Ins.getStatusAndLastScanDate(servernamelbl.Text, type);
                    if (dt.Rows.Count > 0)
                    {
                        //9/19/2014 NS modified for VSPLUS-934
                        //Lastscanned.Text = dt.Rows[0]["LastUpdate"].ToString();
                        Lastscanned.InnerHtml = "Last scan date: " + dt.Rows[0]["LastUpdate"].ToString();
						//serverstatus.Attributes["class"] = dt.Rows[0]["status"].ToString();
						serverstatus.InnerHtml = dt.Rows[0]["status"].ToString();
						if (dt.Rows[0]["status"].ToString() == "OK" || dt.Rows[0]["status"].ToString() == "Scanning")
						{
							serverstatus.Attributes["class"] = "OK";
						}
						else if (dt.Rows[0]["status"].ToString() == "Not Responding")
						{
							serverstatus.Attributes["class"] = "NotResponding";
						}
						else if (dt.Rows[0]["status"].ToString() == "Maintenance")
						{
							serverstatus.Attributes["class"] = "Maintenance";
						}
						else
						{
							serverstatus.Attributes["class"] = "Issue";
						}
                    }
                    else
                    {
                        //9/19/2014 NS modified for VSPLUS-934
                        //lbltext.Visible = false;
                        //Lastscanned.Visible = false;
                        //lbltext.Style.Add("display", "none");
                        Lastscanned.Style.Add("display", "none");
                    }
				//}
                
            }
            // FillCombobox();

            //01/23/2014 MD changed it for every page load for the graphs to be populated always.
            SetGraph("hh", servernamelbl.Text);
			SetGraphForCPU("hh", servernamelbl.Text);
			SetGraphForMemory("hh", servernamelbl.Text);
			FillDeviceInfo();
            //SetGraphForDiskSpace(servernamelbl.Text,"Disk.C");

            DisableDeviceInfoTab();
           
         
            //12/4/2013 NS commented out - the page should only load on tab click
            //2/12/2014 NS uncommented out - tab click event takes too long, we will use a link to open Web Admin in a new window instead
           
            // DataBase Code
            if (!IsPostBack && !IsCallback)
            {
                // performanceASPxRadioButtonList.Items.RemoveAt(0);
                // performanceASPxRadioButtonList.Items[0].Selected = true;
               
                Fillmaintenancegrid();
                FillAlertHistory();
               
                FillHealthAssessmentStatusGrid(Request.QueryString["Type"].ToString(), servernamelbl.Text);

                if (Session["UserPreferences"] != null)
                {
                    DataTable UserPreferences = (DataTable)Session["UserPreferences"];
                    foreach (DataRow dr in UserPreferences.Rows)
                    {
                       
                        if (dr[1].ToString() == "DominoServerDetailsPage2|maintenancegrid")
                        {
                            maintenancegrid.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                        }
                        if (dr[1].ToString() == "DominoServerDetailsPage2|AlertGridView")
                        {
                            AlertGridView.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                        }
                        if (dr[1].ToString() == "DominoServerDetailsPage2|OutageGridView")
                        {
                            OutageGridView.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                        }
                    }
                }
            }
            else
            {
                //2/6/2014 NS commented out the calls below - will move them into a tab click event and load only the ones
                //relevant to the tab
                //2/12/2014 NS uncommented out - the tab click event takes too long, tab loading looks like it's not happening
                	
              
              
                Fillmaintenancegridfromsession();
                FillAlertHistoryfromSession();
               
            }
            //2/6/2013 NS added
            //UpdateButtonVisibility();
            //9/30/2014 NS added for VSPLUS-934
            ASPxPopupControl1.ShowOnPageLoad = false;
            //11/3/2014 NS added for VSPLUS-1142
            UpdateButtonVisibility();
        }


        //11/3/2014 NS added for VSPLUS-1142
        public void UpdateButtonVisibility()
        {
            bool isconfig = false;
            DataTable dsconfig = VSWebBL.SecurityBL.UsersBL.Ins.GetIsConfig(Session["UserID"].ToString());
            if (dsconfig.Rows.Count > 0)
            {
                if (dsconfig.Rows[0]["IsConfigurator"].ToString() == "True")
                {
                    isconfig = true;
                }
            }
            if (isconfig)
            {
                //EditInConfigButton.Visible = true;
                ASPxMenu1.Items[0].Items[2].Visible = true;
                ASPxMenu1.Items[0].Items[3].Visible = true;
            }
            else
            {
                ASPxMenu1.Items[0].Items[2].Visible = false;
                ASPxMenu1.Items[0].Items[3].Visible = false;
            }
        }

	
		
	
    
    
        public DataTable SetGraph(string paramGraph, string DeviceName)
        {
            try
            {
                performanceWebChartControl.Series.Clear();
                DataTable dt = VSWebBL.DashboardBL.NetworkDeviceDetailsBL.Ins.SetGraph(paramGraph, DeviceName);

                Series series = null;
                series = new Series("DominoServer", ViewType.Line);
                series.Visible = true;
			    series.ArgumentDataMember = dt.Columns["dtfrom"].ToString();
				ValueDataMemberCollection seriesValueDataMembers = (ValueDataMemberCollection)series.ValueDataMembers;
				seriesValueDataMembers.AddRange(dt.Columns["maxval"].ToString());
			
                performanceWebChartControl.Series.Add(series);

                // Constant Line

                // Cast the chart's diagram to the XYDiagram type, to access its axes.
                XYDiagram diagram = (XYDiagram)performanceWebChartControl.Diagram;
                //Mukund 16Jul14, VSPLUS-824- Threshold in graph is not updating
                DataTable dt1 = VSWebBL.DashboardBL.DominoServerDetailsBL.Ins.ResponseThreshold(DeviceName);
                     if (dt1.Rows.Count > 0)
                     {
                         if (int.Parse(dt1.Rows[0]["ResponseThreshold"].ToString()) > 0)
                         {
                             // Create a constant line.
                             ConstantLine constantLine1 = new ConstantLine("Constant Line 1");
                             diagram.AxisY.ConstantLines.Add(constantLine1);

                             // Define its axis value.
                             constantLine1.AxisValue = dt1.Rows[0]["ResponseThreshold"].ToString();

                             // Customize the behavior of the constant line.
                             // constantLine1.Visible = true;
                             //constantLine1.ShowInLegend = true;
                             // constantLine1.LegendText = "Some Threshold";
                             constantLine1.ShowBehind = true;

                             // Customize the constant line's title.
                             constantLine1.Title.Visible = true;
                             constantLine1.Title.Text = "Threshold:" + dt1.Rows[0]["ResponseThreshold"].ToString();
                             constantLine1.Title.TextColor = Color.Red;
                             // constantLine1.Title.Antialiasing = false;
                             //constantLine1.Title.Font = new Font("Tahoma", 14, FontStyle.Bold);
                             constantLine1.Title.ShowBelowLine = true;
                             constantLine1.Title.Alignment = ConstantLineTitleAlignment.Far;

                             // Customize the appearance of the constant line.
                             constantLine1.Color = Color.Red;
                             constantLine1.LineStyle.DashStyle = DashStyle.Solid;
                             constantLine1.LineStyle.Thickness = 2;
                         }
                     }
                ((XYDiagram)performanceWebChartControl.Diagram).PaneLayoutDirection = PaneLayoutDirection.Horizontal;

                XYDiagram seriesXY = (XYDiagram)performanceWebChartControl.Diagram;
                seriesXY.AxisY.Title.Text = "Response Time";
                seriesXY.AxisY.Title.Visible = true;
                seriesXY.AxisX.Title.Text = "Date/Time";
                seriesXY.AxisX.Title.Visible = true;
                seriesXY.AxisX.Label.ResolveOverlappingOptions.AllowStagger = true;

                performanceWebChartControl.Legend.Visible = false;

                // ((SplineSeriesView)series.View).LineTensionPercent = 100;
                ((LineSeriesView)series.View).LineMarkerOptions.Size = 4;
                ((LineSeriesView)series.View).LineMarkerOptions.Color = Color.White;

                AxisBase axis = ((XYDiagram)performanceWebChartControl.Diagram).AxisX;
                //4/18/2014 NS commented out for VSPLUS-312
                //axis.DateTimeGridAlignment = DateTimeMeasurementUnit.Hour;
           
                axis.GridLines.Visible = false;
                //axis.DateTimeOptions.Format = DateTimeFormat.Custom;
                //axis.DateTimeOptions.FormatString = "MM/dd/yyyy HH:mm";
                ((LineSeriesView)series.View).Color = Color.Blue;

                AxisBase axisy = ((XYDiagram)performanceWebChartControl.Diagram).AxisY;
              
                axisy.GridLines.Visible = true;
                performanceWebChartControl.DataSource = dt;
                performanceWebChartControl.DataBind();

                return dt;
            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }

        }

        //7/16/2014 NS commented out for VSPLUS-813
        //DiskHealthGrid_SelectionChanged, ASPxCallbackPanel1_Callback, SetGraphForDiskSpace
        /*
		protected void DiskHealthGrid_SelectionChanged(object sender, EventArgs e)
		{
			if (DiskHealthGrid.Selection.Count > 0)
			{
				System.Collections.Generic.List<object> Type = DiskHealthGrid.GetSelectedFieldValues("DiskName");
				//3/22/2014 NS added
				//System.Collections.Generic.List<object> ID = DeviceGridView.GetSelectedFieldValues("ID");
				if (Type.Count > 0)
				{
					string Name = Type[0].ToString();
					SetGraphForDiskSpace(servernamelbl.Text, Name);
					//string SType = ServerType[0].ToString();
					//string LastUpdate = LastDate[0].ToString();
					// Session["Type"] = Type[0];
					//MD Notes 20Feb14
					//if (!(Session["UserFullName"] != null && Session["UserFullName"].ToString() == "Anonymous"))
					//{
					//    if (SType == "NotesMail Probe")
					//    {
					//        DevExpress.Web.ASPxWebControl.RedirectOnCallback("NotesMailProbeDetailsPage.aspx?Name=" + Name + "&Type=" + SType + "&LastDate=" + LastUpdate + "");
					//    }
					//    else if (SType == "Exchange") //MD exchange Dec13
					//    {
					//        DevExpress.Web.ASPxWebControl.RedirectOnCallback("ExchangeServerDetailsPage3.aspx?Name=" + Name + "&Type=" + SType + "&LastDate=" + LastUpdate + "");
					//    }
					//    //3/22/2014 NS added
					//    //else if (SType == "Exchange DAG")
					//    //{
					//    //    DevExpress.Web.ASPxWebControl.RedirectOnCallback("DAGDetail.aspx?Name=" + Name + "&Type=" + SType + "&LastDate=" + LastUpdate + "&ID=" + ID + "");
					//    //}
					//    //19/06/2014
					//    else if (SType == "BES")
					//    {
					//        DevExpress.Web.ASPxWebControl.RedirectOnCallback("BlackBerryServerDetailsPage2.aspx?Name=" + Name + "&Type=" + SType + "&LastDate=" + LastUpdate + "");
					//    }
					//    else
					//    {
					//        DevExpress.Web.ASPxWebControl.RedirectOnCallback("DominoServerDetailsPage2.aspx?Name=" + Name + "&Type=" + SType + "&LastDate=" + LastUpdate + "");
					//    }
					//}
				}


			}
		}
        

		protected void ASPxCallbackPanel1_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
		{
			
				//lblServerName.Text = "";
			int index = DiskHealthGrid.FocusedRowIndex;
				//8/9/2013 NS modified - when no rows were found, object reference not set error appeared
				if (index > -1)
				{
					string value = DiskHealthGrid.GetRowValues(index, "DiskName").ToString();
					SetGraphForDiskSpace(servernamelbl.Text, value);
				}
				
			
		}

		public void SetGraphForDiskSpace(string serverName, string DiskName )
		{
			//if (!IsPostBack)
			//{
				//int cnt = 0;
				diskspaceWebChartControl.Series.Clear();
				DataTable dt = VSWebBL.DashboardBL.DominoServerDetailsBL.Ins.SetGraphForDiskSpace(serverName,DiskName);
				diskspaceWebChartControl.DataSource = dt;

				double[] double1 = new double[dt.Rows.Count];
				double[] double2 = new double[dt.Rows.Count];

				Series series = null;

				//diskspaceWebChartControl.Series.Clear();

				for (int i = 0; i < dt.Rows.Count; i++)
				{
					if (series != null)
					{
						diskspaceWebChartControl.Series.Add(series);
						diskspaceWebChartControl.DataBind();
					}

					series = new Series(dt.Rows[i]["DiskName"].ToString(), ViewType.Pie);

					string val1 = dt.Rows[i]["DiskFree"].ToString();
					string val2 = dt.Rows[i]["DiskUsed"].ToString();

					if (val1 != "" && val2 != "")
					{
						double1[i] = Convert.ToDouble(dt.Rows[i]["DiskFree"].ToString());
						double2[i] = Convert.ToDouble(dt.Rows[i]["DiskUsed"].ToString());

						//if (dt.Rows[i]["DiskFree"] != "" && dt.Rows[i]["DiskFree"] != null)
						//    double1[i] = Convert.ToDouble(dt.Rows[i]["DiskFree"].ToString());
						//if (dt.Rows[i]["DiskUsed"] != "" && dt.Rows[i]["DiskUsed"] != null)
						//    double2[i] = Convert.ToDouble(dt.Rows[i]["DiskUsed"].ToString());

						series.Points.Add(new SeriesPoint("Disk Free", double1[i]));
						series.Points.Add(new SeriesPoint("Disk Used", double2[i]));
						series.ShowInLegend = true;

						series.LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;
						PieSeriesLabel seriesLabel = (PieSeriesLabel)series.Label;
						seriesLabel.Position = PieSeriesLabelPosition.Radial;
						seriesLabel.BackColor = System.Drawing.Color.Transparent;
						seriesLabel.TextColor = System.Drawing.Color.Black;

                        PieSeriesView seriesView = (PieSeriesView)series.View;
						seriesView.Titles.Add(new SeriesTitle());
						seriesView.Titles[0].Dock = ChartTitleDockStyle.Bottom;

						seriesView.Titles[0].Text = series.Name.Substring(5);
						seriesView.Titles[0].Visible = true;
						seriesView.Titles[0].WordWrap = true;
					}
				}

				if (series != null)
				{
					diskspaceWebChartControl.Series.Add(series);
					diskspaceWebChartControl.DataBind();
				}

				for (int c = 0; c < diskspaceWebChartControl.Series.Count; c++)
				{
					if (c == 0)
					{
						PiePointOptions seriesPointOptions = (PiePointOptions)series.LegendPointOptions;
						series.LegendPointOptions.PointView = PointView.Argument;
						diskspaceWebChartControl.Series[0].ShowInLegend = true;
						diskspaceWebChartControl.Series[0].LegendPointOptions.PointView = PointView.Argument;
						diskspaceWebChartControl.Series[0].ShowInLegend = true;
						diskspaceWebChartControl.Legend.Visible = true;
					}
					else
					{

						diskspaceWebChartControl.Series[c].ShowInLegend = false;
					}


				}


			//}
		}
        */
       

      

        //protected void cpuASPxRadioButtonList_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    SetGraphForCPU(cpuASPxRadioButtonList.Value.ToString(), servernamelbl.Text);
        //}

       

        //protected void memASPxRadioButtonList_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    SetGraphForMemory(memASPxRadioButtonList.Value.ToString(), servernamelbl.Text);
        //}

       

       


       

        //protected void mailASPxRadioButtonList_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    SetGraphForMail(mailASPxRadioButtonList.Value.ToString(), servernamelbl.Text);
        //}


        //DataBase Code



        //public void FillGridfromSession()
        //{
        //    DataTable dt = new DataTable();
        //    if (Session["Statustab"] != "" && Session["Statustab"] != null)
        //        dt = Session["Statustab"] as DataTable;
        //    if (dt.Rows.Count > 0)
        //    {
        //        MonitoredDBGridView.DataSource = dt;
        //        MonitoredDBGridView.DataBind();
        //    }
        //}

        //protected void AllDBGridView_SelectionChanged(object sender, EventArgs e)
        //{
        //    if (AllDBGridView.Selection.Count > 0)
        //    {
        //        System.Collections.Generic.List<object> Type = AllDBGridView.GetSelectedFieldValues("Server");

        //        if (Type.Count > 0)
        //        {
        //            string Name = Type[0].ToString();
        //            Session["Type"] = Type[0];
        //            DevExpress.Web.ASPxWebControl.RedirectOnCallback("Performance.aspx?Name=" + Server + "");
        //            //Response.Redirect("DeviceChart.aspx");
        //        }


        //    }
        //}

        //public void FillALLGridfromSession()
        //{
        //    DataTable dt = new DataTable();
        //    if (Session["Dailytab"] != "" && Session["Dailytab"] != null)
        //        dt = Session["Dailytab"] as DataTable;
        //    if (dt.Rows.Count > 0)
        //    {
        //        AllDBGridView.DataSource = dt;
        //        AllDBGridView.DataBind();
        //    }
        //}
        protected void DominoserverTasksgrid_HtmlDataCellPrepared(object sender, DevExpress.Web.ASPxGridViewTableDataCellEventArgs e)
        {
            if (e.DataColumn.FieldName == "StatusSummary" && (e.CellValue.ToString() == "OK" || e.CellValue.ToString() == "Scanning"))
            {
                e.Cell.BackColor = System.Drawing.Color.LightGreen;
            }

            else if (e.DataColumn.FieldName == "StatusSummary" && e.CellValue.ToString() == "Not Responding")
            {
                e.Cell.BackColor = System.Drawing.Color.Red;
                e.Cell.ForeColor = System.Drawing.Color.White;
            }
            else if (e.DataColumn.FieldName == "StatusSummary" && e.CellValue.ToString() == "Not Scanned")
            {
                e.Cell.BackColor = System.Drawing.Color.Blue;
                e.Cell.ForeColor = System.Drawing.Color.White;
            }
            else if (e.DataColumn.FieldName == "StatusSummary" && e.CellValue.ToString() == "disabled")
            {
                e.Cell.BackColor = System.Drawing.Color.Gray;
                // e.Cell.ForeColor = System.Drawing.Color.White;
            }
            else if (e.DataColumn.FieldName == "StatusSummary")
            {
                e.Cell.BackColor = System.Drawing.Color.Yellow;
                // e.DataColumn.GroupFooterCellStyle.ForeColor = System.Drawing.Color.Yellow;
            }
        }

        //protected void DominoserverTasksgrid_SelectionChanged(object sender, EventArgs e)
        //{
        //    if (DominoserverTasksgrid.Selection.Count > 0)
        //    {
        //        System.Collections.Generic.List<object> Type = DominoserverTasksgrid.GetSelectedFieldValues("Name");

        //        if (Type.Count > 0)
        //        {
        //            string Name = Type[0].ToString();
        //            Session["Type"] = Type[0];
        //            DevExpress.Web.ASPxWebControl.RedirectOnCallback("DominoServerDetailsPage2.aspx?Name=" + Name + "");
        //            //Response.Redirect("DeviceChart.aspx");
        //        }


        //    }
        //}

       

       


     

        //protected void NonmoniterdGrid_SelectionChanged(object sender, EventArgs e)
        //{
        //    if (NonmoniterdGrid.Selection.Count > 0)
        //    {
        //        System.Collections.Generic.List<object> Type = NonmoniterdGrid.GetSelectedFieldValues("Name");

        //        if (Type.Count > 0)
        //        {
        //            string Name = Type[0].ToString();

        //            DevExpress.Web.ASPxWebControl.RedirectOnCallback("DominoServerDetailsPage2.aspx?Name=" + Name + "");

        //        }


        //    }
        //}

        protected void NonmoniterdGrid_HtmlDataCellPrepared(object sender, DevExpress.Web.ASPxGridViewTableDataCellEventArgs e)
        {
            if (e.DataColumn.FieldName == "StatusSummary" && (e.CellValue.ToString() == "OK" || e.CellValue.ToString() == "Scanning"))
            {
                e.Cell.BackColor = System.Drawing.Color.LightGreen;
            }

            else if (e.DataColumn.FieldName == "StatusSummary" && e.CellValue.ToString() == "Not Responding")
            {
                e.Cell.BackColor = System.Drawing.Color.Red;
                e.Cell.ForeColor = System.Drawing.Color.White;
            }
            else if (e.DataColumn.FieldName == "StatusSummary" && e.CellValue.ToString() == "Not Scanned")
            {
                e.Cell.BackColor = System.Drawing.Color.Blue;
                e.Cell.ForeColor = System.Drawing.Color.White;
            }
            else if (e.DataColumn.FieldName == "StatusSummary" && e.CellValue.ToString() == "disabled")
            {
                e.Cell.BackColor = System.Drawing.Color.Gray;
                // e.Cell.ForeColor = System.Drawing.Color.White;
            }
            else if (e.DataColumn.FieldName == "StatusSummary")
            {
                e.Cell.BackColor = System.Drawing.Color.Yellow;
            }   // e.DataColumn.GroupFooterCellStyle.ForeColor = System.Drawing.Color.Yellow;
        }

        //protected void MailComboBox_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    SetGraphForMail(MailComboBox.SelectedItem.Text);
        //}

        public void Fillmaintenancegrid()
        {
            Session["SLmaitservers"] = "";
            DataTable dt = VSWebBL.ConfiguratorBL.MaintenanceBL.Ins.Getmaintenanceserversbyservername(Request.QueryString["Name"].ToString(), "", "", "", "");
            maintenancegrid.DataSource = dt;
            maintenancegrid.DataBind();
            Session["SLmaitservers"] = dt;
        }
        public void Fillmaintenancegridfromsession()
        {
            FillMaintenance();
            //if (Session["SLmaitservers"] != "" && Session["SLmaitservers"] != null)
            //{
            //    //DataTable dt = VSWebBL.ConfiguratorBL.MaintenanceBL.Ins.servermaintenance(System.DateTime.Now, 0, 0);
            //    maintenancegrid.DataSource = (DataTable)Session["SLmaitservers"];
            //    maintenancegrid.DataBind();
            //    //Session["maitservers"] = dt;
            //}
        }

        protected void btnsearch_Click(object sender, EventArgs e)
        {
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
        protected void ClearButton_Click(object sender, EventArgs e)
        {
            txtfromdate.Text = "";
            txttodate.Text = "";
            ASPxTimeEdit1.Text = "";
            ASPxTimeEdit2.Text = "";
            FillMaintenance();
        }


        protected void OKButton_Click(object sender, EventArgs e)
        {

            MsgPopupControl.ShowOnPageLoad = false;

        }



        //protected void maintenancegrid_HtmlDataCellPrepared(object sender, DevExpress.Web.ASPxGridViewTableDataCellEventArgs e)
        //{
        //    if (e.DataColumn.FieldName == "MaintType")
        //    {

        //        if (e.CellValue.ToString() == "1")
        //        {
        //            e.Cell.Text = "One time";
        //        }
        //        else if (e.CellValue.ToString() == "2")
        //        {
        //            e.Cell.Text = "Daily";
        //        }
        //        else if (e.CellValue.ToString() == "3")
        //        {
        //            e.Cell.Text = "Weekly";
        //        }
        //        else
        //        {
        //            e.Cell.Text = "Monthly";
        //        }

        //    }


        public void FillAlertHistory()
        {
            DataTable AlertHistorytab = new DataTable();
            try
            {
                AlertHistorytab = VSWebBL.DashboardBL.DominoServerDetailsBL.Ins.GetAlertHistry(Request.QueryString["Name"].ToString());
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

       

        protected void ASPxPageControl1_TabClick(object source, DevExpress.Web.TabControlCancelEventArgs e)
        {
            if (e.Tab.Text == "Web Admin")
            {
                //12/4/2013 NS modified
                

                /*
                string serverip = "";
                string url = "";
                string servername = Request.QueryString["Name"];
                string dbpath = "webadmin.nsf";
                DataTable dt = new DataTable();
                dt = VSWebBL.ConfiguratorBL.AlertsBL.Ins.GetServerIP(servername);
                if (dt.Rows.Count > 0)
                {
                    serverip = dt.Rows[0]["IPAddress"].ToString();
                }
                if (serverip != "")
                {
                    url = "http://" + serverip + "/" + dbpath + "?OpenDatabase";
                    fraHtml.Attributes.Add("src", url);
                }
                 */
            }
            //2/6/2014 NS added all else statements for each tab
            else
            {
                if (e.Tab.Text == "Overall")
                {
                    //12/4/2013 NS copied the code from the !IsPostback event so that when a tab is clicked
                    //and the code executes, the graphs would not lose data
                    SetGraph("hh", servernamelbl.Text);
                   // SetGraphForDiskSpace(servernamelbl.Text);
                   
                }
                else
                {
                    if (e.Tab.Text == "Databases")
                    {
                       
                    }
                    else
                    {
                        if (e.Tab.Text == "Server Tasks")
                        {
                           
                          
                        }
                        else
                        {
                            if (e.Tab.Text == "Maintenance")
                            {
                                Fillmaintenancegridfromsession();
                            }
                            else
                            {
                                if (e.Tab.Text == "Alerts History")
                                {
                                    FillAlertHistoryfromSession();
                                }
                                else
                                {
                                    
                                }
                            }
                        }
                    }
                }
            }
        }

       

        //public void UpdateButtonVisibility()
        //{
        //    bool isadmin = false;
        //    DataTable sa = VSWebBL.SecurityBL.UsersBL.Ins.GetIsConsoleComm(Session["UserID"].ToString());
        //    if (sa.Rows.Count > 0)
        //    {
        //        if (sa.Rows[0]["IsConsoleComm"].ToString() == "True")
        //        {
        //            isadmin = true;
        //        }
        //    }
        //    if (isadmin)
        //    {
        //        //CompactButton.Visible = true;
        //        //FixupButton.Visible = true;
        //        //UpdallButton.Visible = true;
        //    }
        //    //1/2/2014 NS added
        //    bool isconfig = false;
        //    DataTable dsconfig = VSWebBL.SecurityBL.UsersBL.Ins.GetIsConfig(Session["UserID"].ToString());
        //    if (dsconfig.Rows.Count > 0)
        //    {
        //        if (dsconfig.Rows[0]["IsConfigurator"].ToString() == "True")
        //        {
        //            isconfig = true;
        //        }
        //    }
        //    if (isconfig)
        //    {
        //        //EditInConfigButton.Visible = true;
        //    }
        //}

        
       

       
       

        
        protected void CancelButton1_Click(object sender, EventArgs e)
        {
        }

        protected void NOButton_Click(object sender, EventArgs e)
        {
           
             
        
          
            Session["myUserName"] = "";
            Session["myServerName"] = "";
            Session["myDeviceName"] = "";
            Session["TellCommand"] = "";
            Session["TaskName"] = "";
        }

        protected void ScanButton_Click(object sender, EventArgs e)
        {
            try
            {
                Status StatusObj = new Status();
                StatusObj.Name = Request.QueryString["Name"];
                StatusObj.Type = Request.QueryString["Type"];

                bool bl = VSWebBL.StatusBL.StatusTBL.Ins.UpdateforScan(StatusObj);

                if (Request.QueryString["Type"] == "Domino")
                {
					bl = VSWebBL.SettingBL.SettingsBL.Ins.UpdateScanvalue("ScanDominoASAP", StatusObj.Name, VSWeb.Constants.Constants.SysString);
                }
                else if (Request.QueryString["Type"] == "Mail")
                {
					bl = VSWebBL.SettingBL.SettingsBL.Ins.UpdateScanvalue("ScanMailServiceASAP", StatusObj.Name, VSWeb.Constants.Constants.SysString);
                }
                else if (Request.QueryString["Type"] == "Network Device")
                {
					bl = VSWebBL.SettingBL.SettingsBL.Ins.UpdateScanvalue("ScanNetworkDeviceASAP", StatusObj.Name, VSWeb.Constants.Constants.SysString);
                }
                else if (Request.QueryString["Type"] == "Sametime Server")
                {
					bl = VSWebBL.SettingBL.SettingsBL.Ins.UpdateScanvalue("ScanSametimeASAP", StatusObj.Name, VSWeb.Constants.Constants.SysString);
                }
                else if (Request.QueryString["Type"] == "BES")
                {
					bl = VSWebBL.SettingBL.SettingsBL.Ins.UpdateScanvalue("ScanBlackBerryASAP", StatusObj.Name, VSWeb.Constants.Constants.SysString);
                }
                else if (Request.QueryString["Type"] == "URL")
                {
					bl = VSWebBL.SettingBL.SettingsBL.Ins.UpdateScanvalue("ScanURLASAP", StatusObj.Name, VSWeb.Constants.Constants.SysString);
                }
                else if (Request.QueryString["Type"] == "Exchange")
                {
					bl = VSWebBL.SettingBL.SettingsBL.Ins.UpdateScanvalue("ScanExchangeASAP", StatusObj.Name, VSWeb.Constants.Constants.SysString);
                }
                else if (Request.QueryString["Type"] == "SharePoint")
                {
					bl = VSWebBL.SettingBL.SettingsBL.Ins.UpdateScanvalue("ScanSharePointASAP", StatusObj.Name, VSWeb.Constants.Constants.SysString);
                }
				else if (Request.QueryString["Type"] == "Skype for Business")
                {
					bl = VSWebBL.SettingBL.SettingsBL.Ins.UpdateScanvalue("LyncASAP", StatusObj.Name, VSWeb.Constants.Constants.SysString);
                }
            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
        }

        protected void EditInConfigButton_Click(object sender, EventArgs e)
        {
            //1/2/2014 NS added
            string id = "";
            Session["Submenu"] = "LotusDomino";
            if (Request.QueryString["Name"] != "" && Request.QueryString["Name"] != null)
            {
                id = (VSWebBL.SecurityBL.ServersBL.Ins.GetServerIDbyServerName(Request.QueryString["Name"].ToString())).ToString();
                Response.Redirect("~/Configurator/DominoProperties.aspx?Key=" + id, false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
                Context.ApplicationInstance.CompleteRequest();
            }
        }

        protected void BtnApply_Click(object sender, EventArgs e)
        {
            try
            {
                List<string> serverIDValues = new List<string>();
                List<string> servertypeIDValues = new List<string>();
                Random random = new Random((int)DateTime.Now.Ticks);
                //RandomString(5, random);
                string Name = servernamelbl.Text + "-Temp-" + DateTime.Now.ToLongDateString();
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
                DataTable dt = VSWebBL.SecurityBL.ServersBL.Ins.GetServerDetailsByName(servernamelbl.Text);
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
                          EndDate, MaintType, MaintDaysList, EndDateIndicator, serverIDValues, s, rem, 1, true, servertypeIDValues, "true","1");
                }
                if (update == true)
                {
                    //10/3/2014 NS modified for VSPLUS-990
                    SuccessMsg.InnerHtml = "Monitoring for " + servernamelbl.Text + " has been temporarily suspended for a duration of " + TbDuration.Text + " minutes." +
                    "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                    SuccessMsg.Style.Value = "display: block";

                }
                else
                {
                    //10/3/2014 NS modified for VSPLUS-990
                    ErrorMsg.InnerHtml = "The Settings were NOT updated."+
                        "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                    ErrorMsg.Style.Value = "display: block";
                }
                ASPxPopupControl1.ShowOnPageLoad = false;
            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
        }
      
        protected void maintenancegrid_PageSizeChanged(object sender, EventArgs e)
        {
            //ProfilesGridView.PageIndex;
            VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("DominoServerDetailsPage2|maintenancegrid", maintenancegrid.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        }
        protected void AlertGridView_PageSizeChanged(object sender, EventArgs e)
        {
            //ProfilesGridView.PageIndex;
            VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("DominoServerDetailsPage2|AlertGridView", AlertGridView.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        }
        protected void OutageGridView_PageSizeChanged(object sender, EventArgs e)
        {
            //ProfilesGridView.PageIndex;
            VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("DominoServerDetailsPage2|OutageGridView", OutageGridView.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        }





        public void FillHealthAssessmentFromSession()
        {
            try
            {
                if (Session["GridData"] != null)
                {
                    DataTable dt = Session["HealthAssessment"] as DataTable;
                    HealthAssessmentGrid1.DataSource = dt;
                    HealthAssessmentGrid1.DataBind();

                }
            }
            catch (Exception ex)
            {
                //Response.Write("Error : " + ex);
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
        }
        private void FillHealthAssessmentStatusGrid(string type, string servername)
        {
            try
            {
                //DataTable StatusTable1 = VSWebBL.DashboardBL.ExchangeServerDetailsBL.Ins.GetCASStatus(servernamelbl.Text);
                //3/25/2014 NS modified - we only need CAS to pass as a category
                //DataTable StatusTable1 = VSWebBL.DashboardBL.ExchangeServerDetailsBL.Ins.GetStatusDetails(servernamelbl.Text,"CAS Status");
                string TypeAndName = servername + "-" + type;
                DataTable StatusTable12 = VSWebBL.DashboardBL.ExchangeServerDetailsBL.Ins.GetHealthAssessmentStatusDetails(TypeAndName);

                Session["HealthAssessment"] = StatusTable12;
                HealthAssessmentGrid1.DataSource = StatusTable12;
                HealthAssessmentGrid1.DataBind();
            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
            finally
            {
            }

        }
        protected void HealthAssessmentGrid1_HtmlDataCellPrepared(object sender, DevExpress.Web.ASPxGridViewTableDataCellEventArgs e)
        {
            //3/25/2014 NS modified
            if (e.DataColumn.FieldName == "Result" && (e.CellValue.ToString() == "OK" || e.CellValue.ToString() == "Scanning" || e.CellValue.ToString() == "Telnet" ||
                e.CellValue.ToString() == "Running" || e.CellValue.ToString() == "Pass") || e.CellValue.ToString() == "Passed")
            {
                e.Cell.BackColor = System.Drawing.Color.LightGreen;
            }

            else if (e.DataColumn.FieldName == "Result" && (e.CellValue.ToString() == "Not Responding" || e.CellValue.ToString().ToUpper() == "FAIL"))
            {
                e.Cell.BackColor = System.Drawing.Color.Red;
                e.Cell.ForeColor = System.Drawing.Color.White;
            }
            else if (e.DataColumn.FieldName == "Result" && e.CellValue.ToString() == "Not Scanned")
            {
                e.Cell.BackColor = System.Drawing.Color.Blue;
                e.Cell.ForeColor = System.Drawing.Color.White;
            }
            else if (e.DataColumn.FieldName == "Result" && e.CellValue.ToString() == "Disabled")
            {
                e.Cell.BackColor = System.Drawing.Color.FromName("#D0D0D0");
                //e.Cell.BackColor = System.Drawing.Color.Gray;
                // e.Cell.ForeColor = System.Drawing.Color.White;
            }
            else if (e.DataColumn.FieldName == "Result" && e.CellValue.ToString() == "ISSUE")
            {
                e.Cell.BackColor = System.Drawing.Color.Yellow;
                // e.DataColumn.GroupFooterCellStyle.ForeColor = System.Drawing.Color.Yellow;

            }

        }

        protected void ASPxMenu1_ItemClick(object source, DevExpress.Web.MenuItemEventArgs e)
        {
            if (e.Item.Name == "BackItem")
            {
                if (Session["BackURL"] != "" && Session["BackURL"] != null)
                {
                    Response.Redirect(Session["BackURL"].ToString());
                    Session["BackURL"] = "";

                }
            }
            else if (e.Item.Name == "ScanItem")
            {
                try
                {
                    Status StatusObj = new Status();
                    StatusObj.Name = Request.QueryString["Name"];
                    StatusObj.Type = Request.QueryString["Type"];

                    bool bl = VSWebBL.StatusBL.StatusTBL.Ins.UpdateforScan(StatusObj);

                    if (Request.QueryString["Type"] == "Domino")
                    {
						bl = VSWebBL.SettingBL.SettingsBL.Ins.UpdateScanvalue("ScanDominoASAP", StatusObj.Name, VSWeb.Constants.Constants.SysString);
                    }
                    else if (Request.QueryString["Type"] == "Mail")
                    {
						bl = VSWebBL.SettingBL.SettingsBL.Ins.UpdateScanvalue("ScanMailServiceASAP", StatusObj.Name, VSWeb.Constants.Constants.SysString);
                    }
                    else if (Request.QueryString["Type"] == "Network Device")
                    {
						bl = VSWebBL.SettingBL.SettingsBL.Ins.UpdateScanvalue("ScanNetworkDeviceASAP", StatusObj.Name, VSWeb.Constants.Constants.SysString);
                    }
                    else if (Request.QueryString["Type"] == "Sametime Server")
                    {
						bl = VSWebBL.SettingBL.SettingsBL.Ins.UpdateScanvalue("ScanSametimeASAP", StatusObj.Name, VSWeb.Constants.Constants.SysString);
                    }
                    else if (Request.QueryString["Type"] == "BES")
                    {
						bl = VSWebBL.SettingBL.SettingsBL.Ins.UpdateScanvalue("ScanBlackBerryASAP", StatusObj.Name, VSWeb.Constants.Constants.SysString);
                    }
                    else if (Request.QueryString["Type"] == "URL")
                    {
						bl = VSWebBL.SettingBL.SettingsBL.Ins.UpdateScanvalue("ScanURLASAP", StatusObj.Name, VSWeb.Constants.Constants.SysString);
                    }
                    else if (Request.QueryString["Type"] == "Exchange")
                    {
						bl = VSWebBL.SettingBL.SettingsBL.Ins.UpdateScanvalue("ScanExchangeASAP", StatusObj.Name, VSWeb.Constants.Constants.SysString);
                    }
                    else if (Request.QueryString["Type"] == "SharePoint")
                    {
						bl = VSWebBL.SettingBL.SettingsBL.Ins.UpdateScanvalue("ScanSharePointASAP", StatusObj.Name, VSWeb.Constants.Constants.SysString);
                    }
					else if (Request.QueryString["Type"] == "Skype for Business")
                    {
                        bl = VSWebBL.SettingBL.SettingsBL.Ins.UpdateScanvalue("LyncASAP", StatusObj.Name, VSWeb.Constants.Constants.SysString);
                    }
                }
                catch (Exception ex)
                {
                    Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                    throw ex;
                }
            }
            else if (e.Item.Name == "EditConfigItem")
            {
                string id = "";
                Session["Submenu"] = "LotusDomino";
                if (Request.QueryString["Name"] != "" && Request.QueryString["Name"] != null)
                {
                    //9/14/2015 NS modified for VSPLUS-2148
                    //id = (VSWebBL.SecurityBL.ServersBL.Ins.GetServerIDbyServerName(Request.QueryString["Name"].ToString())).ToString();
                    id = (VSWebBL.ConfiguratorBL.NetworkDevicesBL.Ins.GetServerIDbyServerName(Request.QueryString["Name"].ToString())).ToString();
					Response.Redirect("~/Configurator/NetworkDeviceProperties.aspx?ID=" + id, false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
                    Context.ApplicationInstance.CompleteRequest();
                }
            }
            else if (e.Item.Name == "SuspendItem")
            {
                ASPxPopupControl1.HeaderText = "Suspend Temporarily";
                ASPxPopupControl1.ShowOnPageLoad = true;
            }
        }

        //11/3/2014 NS added
        protected void performanceWebChartControl_CustomCallback(object sender, DevExpress.XtraCharts.Web.CustomCallbackEventArgs e)
        {
            //performanceWebChartControl.Width = new Unit(Convert.ToInt32(chartWidth.Value));
        }


		public void SetGraphForCPU(string paramGraph, string serverName)
		{
			try
			{
				cpuWebChartControl.Series.Clear();
				DataTable dt = VSWebBL.DashboardBL.NetworkDeviceDetailsBL.Ins.SetGraphForCPU(paramGraph, serverName);
				if (dt.Rows.Count > 0)
				{
					Series series = null;
					series = new Series("DominoServer", ViewType.Line);
					series.Visible = true;
					series.ArgumentDataMember = dt.Columns["dtfrom"].ToString();

					ValueDataMemberCollection seriesValueDataMembers = (ValueDataMemberCollection)series.ValueDataMembers;
					seriesValueDataMembers.AddRange(dt.Columns["maxval"].ToString());
					cpuWebChartControl.Series.Add(series);

					// Constant Line

					// Cast the chart's diagram to the XYDiagram type, to access its axes.
					XYDiagram diagram = (XYDiagram)cpuWebChartControl.Diagram;

					((XYDiagram)cpuWebChartControl.Diagram).PaneLayoutDirection = PaneLayoutDirection.Horizontal;

					XYDiagram seriesXY = (XYDiagram)cpuWebChartControl.Diagram;
					seriesXY.AxisY.Title.Text = "CPU";
					seriesXY.AxisY.Title.Visible = true;
					seriesXY.AxisX.Title.Text = "Date/Time";
					seriesXY.AxisX.Title.Visible = true;
					seriesXY.AxisX.Label.ResolveOverlappingOptions.AllowStagger = true;

					cpuWebChartControl.Legend.Visible = false;

					// ((SplineSeriesView)series.View).LineTensionPercent = 100;
					((LineSeriesView)series.View).LineMarkerOptions.Size = 4;
					((LineSeriesView)series.View).LineMarkerOptions.Color = Color.White;

					AxisBase axis = ((XYDiagram)cpuWebChartControl.Diagram).AxisX;
					//4/18/2014 NS commented out for VSPLUS-312
					//axis.DateTimeGridAlignment = DateTimeMeasurementUnit.Hour;
					axis.Range.SideMarginsEnabled = false;
					axis.GridLines.Visible = false;
					//axis.DateTimeOptions.Format = DateTimeFormat.Custom;
					//axis.DateTimeOptions.FormatString = "MM/dd/yyyy HH:mm";
					((LineSeriesView)series.View).Color = Color.Blue;

					AxisBase axisy = ((XYDiagram)cpuWebChartControl.Diagram).AxisY;
					axisy.Range.AlwaysShowZeroLevel = false;
					axisy.Range.SideMarginsEnabled = true;
					axisy.GridLines.Visible = true;
					cpuWebChartControl.DataSource = dt;
					cpuWebChartControl.DataBind();
				}

			}
			catch (Exception ex)
			{
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}

		}

		public void SetGraphForMemory(string paramGraph, string serverName)
		{
			try
			{
				MemoryWebChart.Series.Clear();
				DataTable dt = VSWebBL.DashboardBL.NetworkDeviceDetailsBL.Ins.SetGraphForMemory(paramGraph, serverName);
				if (dt.Rows.Count > 0)
				{
					Series series = null;
					series = new Series("DominoServer", ViewType.Line);
					series.Visible = true;
					series.ArgumentDataMember = dt.Columns["dtfrom"].ToString();

					ValueDataMemberCollection seriesValueDataMembers = (ValueDataMemberCollection)series.ValueDataMembers;
					seriesValueDataMembers.AddRange(dt.Columns["maxval"].ToString());
					MemoryWebChart.Series.Add(series);

					// Constant Line

					// Cast the chart's diagram to the XYDiagram type, to access its axes.
					XYDiagram diagram = (XYDiagram)MemoryWebChart.Diagram;

					((XYDiagram)MemoryWebChart.Diagram).PaneLayoutDirection = PaneLayoutDirection.Horizontal;

					XYDiagram seriesXY = (XYDiagram)MemoryWebChart.Diagram;
					seriesXY.AxisY.Title.Text = "Memory";
					seriesXY.AxisY.Title.Visible = true;
					seriesXY.AxisX.Title.Text = "Date/Time";
					seriesXY.AxisX.Title.Visible = true;
					seriesXY.AxisX.Label.ResolveOverlappingOptions.AllowStagger = true;

					MemoryWebChart.Legend.Visible = false;

					// ((SplineSeriesView)series.View).LineTensionPercent = 100;
					((LineSeriesView)series.View).LineMarkerOptions.Size = 4;
					((LineSeriesView)series.View).LineMarkerOptions.Color = Color.White;

					AxisBase axis = ((XYDiagram)MemoryWebChart.Diagram).AxisX;
					//4/18/2014 NS commented out for VSPLUS-312
					//axis.DateTimeGridAlignment = DateTimeMeasurementUnit.Hour;
					axis.Range.SideMarginsEnabled = false;
					axis.GridLines.Visible = false;
					//axis.DateTimeOptions.Format = DateTimeFormat.Custom;
					//axis.DateTimeOptions.FormatString = "MM/dd/yyyy HH:mm";
					((LineSeriesView)series.View).Color = Color.Blue;

					AxisBase axisy = ((XYDiagram)MemoryWebChart.Diagram).AxisY;
					axisy.Range.AlwaysShowZeroLevel = false;
					axisy.Range.SideMarginsEnabled = true;
					axisy.GridLines.Visible = true;
					MemoryWebChart.DataSource = dt;
					MemoryWebChart.DataBind();
				}

			}
			catch (Exception ex)
			{
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}

		}

		public void FillDeviceInfo()
		{
			tbl.Controls.Clear();

			DataTable dt = VSWebBL.DashboardBL.NetworkDeviceDetailsBL.Ins.FillDeviceInfo(servernamelbl.Text);// new DataTable(); //CHANGE THIS
			//CHANGE THIS

			for (int i = 0; i < dt.Rows.Count; i++)
			{
				TableRow row = new TableRow();
				tbl.Controls.Add(row);
				for (int j = 0; j < 2; j++)
				{
					TableCell cell = new TableCell();
					DevExpress.Web.ASPxLabel label = new DevExpress.Web.ASPxLabel();
					label.Text = dt.Rows[i][j].ToString() + (j == 0 ? ":" : "");
					cell.Controls.Add(label);
					row.Controls.Add(cell);
						
				}
			}

		}
        //06/05/2016 sowmya added for VPLUS-2902
        public void DisableDeviceInfoTab()
        {
           
           string Networktype = VSWebBL.ConfiguratorBL.NetworkDevicesBL.Ins.GetNetworktype(Request.QueryString["Name"].ToString());
           if (Networktype == "Juniper ScreenOS")
           {
               ASPxPageControl1.TabPages[2].ClientVisible = false;
           }
           else
           {
               ASPxPageControl1.TabPages[2].ClientVisible = true;
           }

        }

    }

}
  




   
 