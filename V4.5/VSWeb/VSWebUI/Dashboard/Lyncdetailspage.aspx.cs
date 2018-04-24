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

namespace VSWebUI.Dashboard
{
    public partial class Lyncdetailspage : System.Web.UI.Page
    {
		protected void Page_PreInit(object sender, EventArgs e)
		{

			this.Master.contentCallEvent += new EventHandler(Master_ButtonClick);
		}

		private void Master_ButtonClick(object sender, EventArgs e)
		{

		}
		int ServerTypeId = 15;

        protected void Page_Load(object sender, EventArgs e)
        {
            FillHealthAssessmentStatusGrid(Request.QueryString["Type"].ToString(), servernamelbl.Text);

            if (!IsPostBack && !IsCallback)
            {
                

                if (Request.QueryString["Type"] != "" && Request.QueryString["Type"] != null)
                {
                    lblServerType.Text = Request.QueryString["Type"].ToString() + ":  ";
                }
                if (Request.QueryString["Typ"] != "" && Request.QueryString["Typ"] != null)
                {
                    lblServerType.Text = Request.QueryString["Type"].ToString();
                }

                if (Request.QueryString["Type"] == "URL" || Request.QueryString["Type"] == "Network Device")
                {
                    diskspaceASPxRoundPanel.Visible = false;
                    cpuASPxRoundPanel.Visible = false;
                    memASPxRoundPanel.Visible = false;
                    usersASPxRoundPanel.Visible = false;
                  
                   // performanceASPxRoundPanel.Width = 1000;
                   // performanceWebChartControl.Width = 800;
                    ASPxPageControl1.TabPages[1].ClientVisible = false;
                    ASPxPageControl1.TabPages[2].ClientVisible = false;
                }

                if (Request.QueryString["Name"] != "" && Request.QueryString["Name"] != null)
                {
                    servernamelbl.Text = Request.QueryString["Name"];

                    linkDiskSpaceMore.NavigateUrl = "DiskHealth.aspx?server=" + servernamelbl.Text;
                    CpuHyperLink.NavigateUrl = "DominoServerStatisticsDetail.aspx?name=Cpu&server=" + servernamelbl.Text;
                    MemHyperLink.NavigateUrl = "DominoServerStatisticsDetail.aspx?name=Mem&server=" + servernamelbl.Text;
                  //  PerfrmHyperLink.NavigateUrl = "DominoServerStatisticsDetail.aspx?name=Perfm&server=" + servernamelbl.Text;
                }
                if (Request.QueryString["LastDate"] != "" && Request.QueryString["LastDate"] != null)
                {
                    Lastscanned.Text = Request.QueryString["LastDate"].ToString();
                }
                else
                {
                    DataTable dt = VSWebBL.DashboardBL.QuickrHealthBLL.Ins.getLastScanDate(servernamelbl.Text);
                    if (dt.Rows.Count > 0)
                    {
                        Lastscanned.Text = dt.Rows[0]["LastUpdate"].ToString();
                    }
                    else
                    {
                        lbltext.Visible = false;
                        Lastscanned.Visible = false;
                    }
                }

            }
            // FillCombobox();

            //01/23/2014 MD changed it for every page load for the graphs to be populated always.
            //SetGraphForDiskSpace(servernamelbl.Text,"Disk.C");
            SetGridForLyncDisk(servernamelbl.Text);
            SetGraphForMemoryLync("hh", servernamelbl.Text);
            SetGraphForCPULync("hh", servernamelbl.Text);
             SetGraphforLyncChatLatency("hh", servernamelbl.Text);
            SetGraphForLyncEnabledUsers("hh", servernamelbl.Text);
            SetGraphforLyncUsersConnected("hh", servernamelbl.Text);
            SetGraphforLyncVoiceenabled("hh", servernamelbl.Text);
            SetGraphForchatstatus("hh",servernamelbl.Text);
            SetGraphforLyncGroupChatLatency("hh", servernamelbl.Text);
            //7/14/2014 NS added for VSPLUS-813
            //5/6/2015 NS modified for VSPLUS-1707
            SetGraphForDiskSpace2("'" + servernamelbl.Text + "'");
            DiskSpaceWebChartControl1.ClientVisible = true;
            totlWebChartControl1.ClientVisible = true;
            DiskSpaceLync.ClientVisible = false;
            SetGraphforperformance("hh", servernamelbl.Text);
            totlWebChartControl1.Visible = false;
            //12/4/2013 NS commented out - the page should only load on tab click
            //2/12/2014 NS uncommented out - tab click event takes too long, we will use a link to open Web Admin in a new window instead
           

            // DataBase Code
            if (!IsPostBack && !IsCallback)
            {
                // performanceASPxRadioButtonList.Items.RemoveAt(0);
                // performanceASPxRadioButtonList.Items[0].Selected = true;

               
                Fillmaintenancegrid();
                FillAlertHistory();
                FillOutageTab();
                FillHealthAssessmentStatusGrid(Request.QueryString["Type"].ToString(), servernamelbl.Text);



                if (Session["UserPreferences"] != null)
                {
                    DataTable UserPreferences = (DataTable)Session["UserPreferences"];
                    foreach (DataRow dr in UserPreferences.Rows)
                    {
                        //if (dr[1].ToString() == "DominoServerDetailsPage2|MonitoredDBGridView")
                        //{
                        //    MonitoredDBGridView.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                        //}
                        //if (dr[1].ToString() == "DominoServerDetailsPage2|AllDBGridView")
                        //{
                        //    AllDBGridView.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                        //}
                        //if (dr[1].ToString() == "DominoServerDetailsPage2|DominoserverTasksgrid")
                        //{
                        //    DominoserverTasksgrid.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                        //}
                        //if (dr[1].ToString() == "DominoServerDetailsPage2|NonmoniterdGrid")
                        //{
                        //    NonmoniterdGrid.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                        //}
                        if (dr[1].ToString() == "LyncServerDetailsPage|maintenancegrid")
                        {
                            maintenancegrid.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                        }
                        if (dr[1].ToString() == "LyncServerDetailsPage|AlertGridView")
                        {
                            AlertGridView.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                        }
                        if (dr[1].ToString() == "LyncServerDetailsPage|OutageGridView")
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
              
                SetGridFromSession();              
                Fillmaintenancegridfromsession();
                FillAlertHistoryfromSession();
                FilloutagefromSession();

                FillHealthAssessmentStatusGrid(Request.QueryString["Type"].ToString(), servernamelbl.Text);
           
            }
            //2/6/2013 NS added
           // UpdateButtonVisibility();
        }
        public void SetGridForLyncDisk(string ServerName)
        {
            try
            {
                DataTable dt = VSWebBL.DashboardBL.LyncDetailsBAL.Ins.SetGridForLyncDisk(ServerName);
                Session["GridData"] = dt;
                DiskSpaceLync.DataSource = dt;
                DiskSpaceLync.DataBind();
                //((GridViewDataColumn)DiskHealthGrid.Columns["ServerName"]).GroupBy();
                int rowIndex = DiskSpaceLync.FindVisibleIndexByKeyValue("ID");
                //Session["rowIndex"] = rowIndex;
            }
            catch (Exception ex)
            {
                //Response.Write("Error : " + ex);
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
        }
        public void FillHealthAssessmentFromSession()
        {
            try
            {
                if (Session["GridData"] != null)
                {
                    DataTable dt = Session["HealthAssessment"] as DataTable;
                    HealthAssessmentGrid11.DataSource = dt;
                    HealthAssessmentGrid11.DataBind();

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
                HealthAssessmentGrid11.DataSource = StatusTable12;
                HealthAssessmentGrid11.DataBind();
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
        protected void HealthAssessmentGrid11_HtmlDataCellPrepared(object sender, DevExpress.Web.ASPxGridViewTableDataCellEventArgs e)
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
        //;;;;;;;;;;;;;;
        public void SetGridFromSession()
        {
            try
            {
                if (Session["GridData"] != null)
                {
                    DataTable dt = Session["GridData"] as DataTable;
                    DiskSpaceLync.DataSource = dt;
                    DiskSpaceLync.DataBind();
                    int rowIndex = DiskSpaceLync.FindVisibleIndexByKeyValue("ID");
                    Session["rowIndex"] = rowIndex;
                }
            }
            catch (Exception ex)
            {
                //Response.Write("Error : " + ex);
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
        }
        protected void DiskSpaceLync_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            string thresholdType = e.GetValue("ThresholdType").ToString();
            //if (thresholdType == "")
            //    thresholdType = "Percent";
            double valThreshold;
            if (e.GetValue("Threshold").ToString() != "")
            {
                valThreshold = (double)e.GetValue("Threshold");

                //if the threshold type is "%" then divide the value by 100
                if (thresholdType == "Percent" && valThreshold > 0)
                    valThreshold = (valThreshold / 100);
            }
            else
            {
                valThreshold = 0.0;
            }
            if (e.DataColumn.FieldName == "DiskName")
            {
                if (thresholdType == "Percent")
                {
                    double valPercentFree;
                    if (e.GetValue("PercentFree").ToString() != "")
                    {
                        valPercentFree = (double)e.GetValue("PercentFree");
                    }
                    else
                    {
                        valPercentFree = 0.0;
                    }

                    if (valPercentFree > valThreshold && (valPercentFree <= 1 - (0.8 * (1 - valThreshold))))
                    {
                        e.Cell.BackColor = System.Drawing.Color.Yellow;
                    }
                    else if ((valPercentFree) <= valThreshold)
                    {
                        e.Cell.BackColor = System.Drawing.Color.Red;
                        e.Cell.ForeColor = System.Drawing.Color.White;
                    }
                    else
                    {
                        e.Cell.BackColor = System.Drawing.Color.Green;
                        e.Cell.ForeColor = System.Drawing.Color.White;
                    }
                }
            }
            if (e.DataColumn.FieldName == "DiskName")
            {
                if (thresholdType == "GB")
                {
                    double valGBFree;
                    if (e.GetValue("DiskFree").ToString() != "")
                    {
                        valGBFree = (double)e.GetValue("DiskFree");
                    }
                    else
                    {
                        valGBFree = 0.0;
                    }

                    //double valPercentFree = (double)e.GetValue("PercentFree");

                    //if (valGBFree > valThreshold && (valPercentFree <= 1 - (0.8 * (1 - 0))))
                    if (valGBFree > valThreshold && (valGBFree <= (valThreshold + ((20 * valThreshold) / 100))))
                    {
                        e.Cell.BackColor = System.Drawing.Color.Yellow;
                    }
                    else if ((valGBFree) <= valThreshold)
                    {
                        e.Cell.BackColor = System.Drawing.Color.Red;
                        e.Cell.ForeColor = System.Drawing.Color.White;
                    }
                    else
                    {
                        e.Cell.BackColor = System.Drawing.Color.Green;
                        e.Cell.ForeColor = System.Drawing.Color.White;
                    }
                }
            }
            //if (e.DataColumn.FieldName == "AverageQueueLength")
            //{
            //    double valThreshold;
            //    if (e.GetValue("Threshold").ToString() != "")
            //    {
            //        valThreshold = (double)e.GetValue("Threshold");
            //    }
            //    else
            //    {
            //        valThreshold = 0.0;
            //    }

            //    if ((valThreshold) >= 0.3 && valThreshold < 1.0)
            //    {
            //        e.Cell.BackColor = System.Drawing.Color.Yellow;
            //    }
            //    else if ((valThreshold) >= 1.0)
            //    {
            //        e.Cell.BackColor = System.Drawing.Color.Red;
            //    }
            //    else
            //    {
            //        e.Cell.BackColor = System.Drawing.Color.Green;
            //        e.Cell.ForeColor = System.Drawing.Color.White;
            //    }
            //}
            //if (e.DataColumn.FieldName == "PercentUtilization")
            //{
            //    double valThreshold;
            //    if (e.GetValue("Threshold").ToString() != "")
            //    {
            //        valThreshold = (double)e.GetValue("Threshold");
            //    }
            //    else
            //    {
            //        valThreshold = 0.0;
            //    }

            //    if ((valThreshold) >= 0.8 && valThreshold < 1.0)
            //    {
            //        e.Cell.BackColor = System.Drawing.Color.Yellow;
            //    }
            //    else if ((valThreshold) >= 1.0)
            //    {
            //        e.Cell.BackColor = System.Drawing.Color.Red;
            //    }
            //    else
            //    {
            //        e.Cell.BackColor = System.Drawing.Color.Green;
            //        e.Cell.ForeColor = System.Drawing.Color.White;
            //    }
            //}
        }

        protected void DiskSpaceLync_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            //ASPxGridView grid = sender as ASPxGridView;
            //int index = int.Parse(e.Parameters);
            //int exCount = 0;
            //for (int i = grid.SettingsPager.PageSize * grid.PageIndex; i < index; i++)
            //{
            //    if (grid.IsRowExpanded(i))
            //        exCount += 1;
            //}
            //grid.CollapseAll();
            //grid.FocusedRowIndex = index - exCount / 2;
            //grid.ExpandRow(index - exCount / 2);

        }

        protected void DiskSpaceLync_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
        {
            int index = DiskSpaceLync.FocusedRowIndex;
            if (e.VisibleIndex != index)
            {
                e.Row.Attributes.Add("onmouseover", "this.originalcolor=this.style.backgroundColor;" + "this.style.backgroundColor='#C0C0C0';");

                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalcolor;");

            }
        }
        protected void DiskSpaceLync_PageSizeChanged(object sender, EventArgs e)
        {
            //ProfilesGridView.PageIndex;
            VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("DiskHealth|DiskHealthGrid", DiskSpaceLync.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        }
        public void FillOutageTab()
        {
            DataTable dt = new DataTable();
            dt = VSWebBL.DashboardBL.DominoServerDetailsBL.Ins.GetOutage(servernamelbl.Text, Request.QueryString["Type"].ToString());
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




        public void SetGraphForchatstatus(string paramGraph, string serverName)
        {
              totlWebChartControl1.Series.Clear();

            DataTable dt1 = VSWebBL.DashboardBL.LyncDetailsBAL.Ins.SetGraphForchatstatus(serverName,paramGraph);
            DataTable dt2 = dt1.DefaultView.ToTable(true, "StatName");

            //DataSet ds = new DataSet();
            //DataTable finaldt1 = dt1.Clone();
            for (int i = 0; i < dt2.Rows.Count; i++)
            {
                DataTable finaldt = new DataTable();
                finaldt = dt1.Clone();
                //string str = dt2.Rows[i][0].ToString();
                DataRow[] dr = dt1.Select("StatName = '" + dt2.Rows[i][0].ToString() + "'");
                for (int c = 0; c < dr.Length; c++)
                {
                    finaldt.NewRow();
                    finaldt.ImportRow(dr[c]);                    
                }

                Series series = new Series(dt2.Rows[i][0].ToString(), ViewType.Line);

                series.DataSource = finaldt;

                series.ArgumentDataMember = dt1.Columns["Date"].ToString();

                ValueDataMemberCollection seriesValueDataMembers = (ValueDataMemberCollection)series.ValueDataMembers;
                seriesValueDataMembers.AddRange(dt1.Columns["StatValue"].ToString());

                totlWebChartControl1.Series.Add(series);

                XYDiagram seriesXY = (XYDiagram)totlWebChartControl1.Diagram;
                seriesXY.AxisX.Title.Text = "Time";
                seriesXY.AxisX.Title.Visible = true;
                seriesXY.AxisY.Title.Text = "StatName";
                seriesXY.AxisY.Title.Visible = true;

                //transactionPerMinuteWebChart.Legend.Visible = false;

               // ((SplineSeriesView)series.View).LineTensionPercent = 100;
                ((LineSeriesView)series.View).LineMarkerOptions.Size = 8;
                ((LineSeriesView)series.View).LineMarkerOptions.Color = Color.White;

                //series.CrosshairLabelPattern = "{A} : {V}";

                AxisBase axis = ((XYDiagram)totlWebChartControl1.Diagram).AxisX;
                //4/18/2014 NS commented out for VSPLUS-312
                //axis.DateTimeGridAlignment = DateTimeMeasurementUnit.Hour;
                axis.GridSpacingAuto = false;
                axis.MinorCount = 15;
                axis.GridSpacing = 1;
                axis.Range.SideMarginsEnabled = false;
                axis.GridLines.Visible = false;
                //axis.DateTimeOptions.Format = DateTimeFormat.Custom;
                //axis.DateTimeOptions.FormatString = "MM/dd/yyyy HH:mm";

                AxisBase axisy = ((XYDiagram)totlWebChartControl1.Diagram).AxisY;
                axisy.Range.AlwaysShowZeroLevel = false;
                axisy.Range.SideMarginsEnabled = true;
                axisy.GridLines.Visible = true;         
            }
        }

        public DataTable SetGraphforperformance(string paramGraph, string DeviceName)
        {
            try
            {
                performanceWebChartControl.Series.Clear();
				DataTable dt = VSWebBL.DashboardBL.LyncDetailsBAL.Ins.SetGraphforperformance(paramGraph, DeviceName, ServerTypeId);

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
                DataTable dt1 = VSWebBL.DashboardBL.LyncDetailsBAL.Ins.ResponseThresholdForLync(DeviceName);
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
                axis.Range.SideMarginsEnabled = false;
                axis.GridLines.Visible = false;
                //axis.DateTimeOptions.Format = DateTimeFormat.Custom;
                //axis.DateTimeOptions.FormatString = "MM/dd/yyyy HH:mm";
                ((LineSeriesView)series.View).Color = Color.Blue;

                AxisBase axisy = ((XYDiagram)performanceWebChartControl.Diagram).AxisY;
                axisy.Range.AlwaysShowZeroLevel = false;
                axisy.Range.SideMarginsEnabled = true;
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

        public void SetGraphforLyncUsersConnected(string paramGraph, string serverName)
        {
           usersconnectedWebChartControl1.Series.Clear();
		   DataTable dt = VSWebBL.DashboardBL.LyncDetailsBAL.Ins.SetGraphforLyncUsersConnected(paramGraph, serverName, ServerTypeId);

            Series series = null;
            series = new Series("DominoServer", ViewType.Line);
            series.Visible = true;
            series.ArgumentDataMember = dt.Columns["dtfrom"].ToString();

            ValueDataMemberCollection seriesValueDataMembers = (ValueDataMemberCollection)series.ValueDataMembers;
            seriesValueDataMembers.AddRange(dt.Columns["maxval"].ToString());
            usersconnectedWebChartControl1.Series.Add(series);

            ((XYDiagram)usersconnectedWebChartControl1.Diagram).PaneLayoutDirection = PaneLayoutDirection.Horizontal;

            XYDiagram seriesXY = (XYDiagram)usersconnectedWebChartControl1.Diagram;
			seriesXY.AxisY.Title.Text = "Skype for Business UsersConnected";
            seriesXY.AxisY.Title.Visible = true;
            seriesXY.AxisX.Title.Text = "Date/Time";
            seriesXY.AxisX.Title.Visible = true;
            usersconnectedWebChartControl1.Legend.Visible = false;

            //((SplineSeriesView)series.View).LineTensionPercent = 100;
            ((LineSeriesView)series.View).LineMarkerOptions.Size = 4;
            ((LineSeriesView)series.View).LineMarkerOptions.Color = Color.White;

            AxisBase axis = ((XYDiagram)usersWebChartControl.Diagram).AxisX;
            //4/18/2014 NS commented out for VSPLUS-312
            //axis.DateTimeGridAlignment = DateTimeMeasurementUnit.Hour;
            //axis.MinorCount = 15;
            //axis.GridSpacing = 0.5;
            axis.Range.SideMarginsEnabled = false;
            axis.GridLines.Visible = false;
            //axis.DateTimeOptions.Format = DateTimeFormat.Custom;

            ((LineSeriesView)series.View).Color = Color.Blue;

            //10/9/2013 NS moved the ds assignment prior to setting the grid spacing option

            usersconnectedWebChartControl1.DataSource = dt;
            usersconnectedWebChartControl1.DataBind();
            AxisBase axisy = ((XYDiagram)usersWebChartControl.Diagram).AxisY;
            axisy.Range.AlwaysShowZeroLevel = false;
            axisy.Range.SideMarginsEnabled = true;
            axisy.GridLines.Visible = true;



            //10/8/2013 NS added
            seriesXY.AxisY.NumericOptions.Format = NumericFormat.Number;
            seriesXY.AxisY.NumericOptions.Precision = 0;
            seriesXY.AxisY.GridSpacingAuto = true;

            double min = Convert.ToDouble(((XYDiagram)usersWebChartControl.Diagram).AxisY.Range.MinValue);
            double max = Convert.ToDouble(((XYDiagram)usersWebChartControl.Diagram).AxisY.Range.MaxValue);

            int gs = (int)((max - min) / 5);

            if (gs == 0)
            {
                gs = 1;
                seriesXY.AxisY.GridSpacingAuto = false;
                seriesXY.AxisY.GridSpacing = gs;
            }

        }

        protected void usersconnectedASPxRadioButtonList_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetGraphforLyncUsersConnected(usersconnectedASPxRadioButtonList1.Value.ToString(), servernamelbl.Text);
        }

        public void SetGraphforLyncVoiceenabled(string paramGraph, string serverName)
        {
            voiceenabledWebChartControl1.Series.Clear();
			DataTable dt = VSWebBL.DashboardBL.LyncDetailsBAL.Ins.SetGraphforLyncVoiceenabled(paramGraph, serverName, ServerTypeId);

            Series series = null;
            series = new Series("LyncServer", ViewType.Line);
            series.Visible = true;
            series.ArgumentDataMember = dt.Columns["dtfrom"].ToString();

            ValueDataMemberCollection seriesValueDataMembers = (ValueDataMemberCollection)series.ValueDataMembers;
            seriesValueDataMembers.AddRange(dt.Columns["maxval"].ToString());
            voiceenabledWebChartControl1.Series.Add(series);

            ((XYDiagram)voiceenabledWebChartControl1.Diagram).PaneLayoutDirection = PaneLayoutDirection.Horizontal;

            XYDiagram seriesXY = (XYDiagram)voiceenabledWebChartControl1.Diagram;
			seriesXY.AxisY.Title.Text = "Skype for Business VoiceEnabledUsers";
            seriesXY.AxisY.Title.Visible = true;
            seriesXY.AxisX.Title.Text = "Date/Time";
            seriesXY.AxisX.Title.Visible = true;
            voiceenabledWebChartControl1.Legend.Visible = false;

            //((SplineSeriesView)series.View).LineTensionPercent = 100;
            ((LineSeriesView)series.View).LineMarkerOptions.Size = 4;
            ((LineSeriesView)series.View).LineMarkerOptions.Color = Color.White;

            AxisBase axis = ((XYDiagram)voiceenabledWebChartControl1.Diagram).AxisX;
            //4/18/2014 NS commented out for VSPLUS-312
            //axis.DateTimeGridAlignment = DateTimeMeasurementUnit.Hour;
            //axis.MinorCount = 15;
            //axis.GridSpacing = 0.5;
            axis.Range.SideMarginsEnabled = false;
            axis.GridLines.Visible = false;
            //axis.DateTimeOptions.Format = DateTimeFormat.Custom;

            ((LineSeriesView)series.View).Color = Color.Blue;

            //10/9/2013 NS moved the ds assignment prior to setting the grid spacing option

            voiceenabledWebChartControl1.DataSource = dt;
            voiceenabledWebChartControl1.DataBind();
            AxisBase axisy = ((XYDiagram)usersWebChartControl.Diagram).AxisY;
            axisy.Range.AlwaysShowZeroLevel = false;
            axisy.Range.SideMarginsEnabled = true;
            axisy.GridLines.Visible = true;



            //10/8/2013 NS added
            seriesXY.AxisY.NumericOptions.Format = NumericFormat.Number;
            seriesXY.AxisY.NumericOptions.Precision = 0;
            seriesXY.AxisY.GridSpacingAuto = true;

            double min = Convert.ToDouble(((XYDiagram)usersWebChartControl.Diagram).AxisY.Range.MinValue);
            double max = Convert.ToDouble(((XYDiagram)usersWebChartControl.Diagram).AxisY.Range.MaxValue);

            int gs = (int)((max - min) / 5);

            if (gs == 0)
            {
                gs = 1;
                seriesXY.AxisY.GridSpacingAuto = false;
                seriesXY.AxisY.GridSpacing = gs;
            }

        }

        protected void voiceenabledASPxRadioButtonList_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetGraphforLyncVoiceenabled(voiceenabledASPxRadioButtonList1.Value.ToString(), servernamelbl.Text);
        }


        //public DataTable SetGraph(string paramGraph, string DeviceName)
        //{
        //    try
        //    {
        //        performanceWebChartControl.Series.Clear();
        //        DataTable dt = VSWebBL.DashboardBL.DominoServerDetailsBL.Ins.SetGraph(paramGraph, DeviceName);

        //        Series series = null;
        //        series = new Series("DominoServer", ViewType.Line);
        //        series.Visible = true;
        //        series.ArgumentDataMember = dt.Columns["dtfrom"].ToString();

        //        ValueDataMemberCollection seriesValueDataMembers = (ValueDataMemberCollection)series.ValueDataMembers;
        //        seriesValueDataMembers.AddRange(dt.Columns["maxval"].ToString());
        //        performanceWebChartControl.Series.Add(series);

        //        // Constant Line

        //        // Cast the chart's diagram to the XYDiagram type, to access its axes.
        //        XYDiagram diagram = (XYDiagram)performanceWebChartControl.Diagram;
        //        //Mukund 16Jul14, VSPLUS-824- Threshold in graph is not updating
        //        DataTable dt1 = VSWebBL.DashboardBL.DominoServerDetailsBL.Ins.ResponseThreshold(DeviceName);
        //        if (dt1.Rows.Count > 0)
        //        {
        //            if (int.Parse(dt1.Rows[0]["ResponseThreshold"].ToString()) > 0)
        //            {
        //                // Create a constant line.
        //                ConstantLine constantLine1 = new ConstantLine("Constant Line 1");
        //                diagram.AxisY.ConstantLines.Add(constantLine1);

        //                // Define its axis value.
        //                constantLine1.AxisValue = dt1.Rows[0]["ResponseThreshold"].ToString();

        //                // Customize the behavior of the constant line.
        //                // constantLine1.Visible = true;
        //                //constantLine1.ShowInLegend = true;
        //                // constantLine1.LegendText = "Some Threshold";
        //                constantLine1.ShowBehind = true;

        //                // Customize the constant line's title.
        //                constantLine1.Title.Visible = true;
        //                constantLine1.Title.Text = "Threshold:" + dt1.Rows[0]["ResponseThreshold"].ToString();
        //                constantLine1.Title.TextColor = Color.Red;
        //                // constantLine1.Title.Antialiasing = false;
        //                //constantLine1.Title.Font = new Font("Tahoma", 14, FontStyle.Bold);
        //                constantLine1.Title.ShowBelowLine = true;
        //                constantLine1.Title.Alignment = ConstantLineTitleAlignment.Far;

        //                // Customize the appearance of the constant line.
        //                constantLine1.Color = Color.Red;
        //                constantLine1.LineStyle.DashStyle = DashStyle.Solid;
        //                constantLine1.LineStyle.Thickness = 2;
        //            }
        //        }
        //        ((XYDiagram)performanceWebChartControl.Diagram).PaneLayoutDirection = PaneLayoutDirection.Horizontal;

        //        XYDiagram seriesXY = (XYDiagram)performanceWebChartControl.Diagram;
        //        seriesXY.AxisY.Title.Text = "Response Time";
        //        seriesXY.AxisY.Title.Visible = true;
        //        seriesXY.AxisX.Title.Text = "Date/Time";
        //        seriesXY.AxisX.Title.Visible = true;
        //        seriesXY.AxisX.Label.ResolveOverlappingOptions.AllowStagger = true;

        //        performanceWebChartControl.Legend.Visible = false;

        //        // ((SplineSeriesView)series.View).LineTensionPercent = 100;
        //        ((LineSeriesView)series.View).LineMarkerOptions.Size = 4;
        //        ((LineSeriesView)series.View).LineMarkerOptions.Color = Color.White;

        //        AxisBase axis = ((XYDiagram)performanceWebChartControl.Diagram).AxisX;
        //        //4/18/2014 NS commented out for VSPLUS-312
        //        //axis.DateTimeGridAlignment = DateTimeMeasurementUnit.Hour;
        //        axis.Range.SideMarginsEnabled = false;
        //        axis.GridLines.Visible = false;
        //        //axis.DateTimeOptions.Format = DateTimeFormat.Custom;
        //        //axis.DateTimeOptions.FormatString = "MM/dd/yyyy HH:mm";
        //        ((LineSeriesView)series.View).Color = Color.Blue;

        //        AxisBase axisy = ((XYDiagram)performanceWebChartControl.Diagram).AxisY;
        //        axisy.Range.AlwaysShowZeroLevel = false;
        //        axisy.Range.SideMarginsEnabled = true;
        //        axisy.GridLines.Visible = true;
        //        performanceWebChartControl.DataSource = dt;
        //        performanceWebChartControl.DataBind();

        //        return dt;
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
        //        throw ex;
        //    }

        //}

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
        public void SetGraphForDiskSpace2(string serverName)
        {
            DataTable dt = VSWebBL.DashboardBL.DominoServerDetailsBL.Ins.SetGraphForDiskSpace(serverName, "","");
            DiskSpaceWebChartControl1.DataSource = dt;
            DiskSpaceWebChartControl1.Series[0].DataSource = dt;
            DiskSpaceWebChartControl1.Series[0].ArgumentDataMember = dt.Columns["DiskName"].ToString();
            DiskSpaceWebChartControl1.Series[0].ValueDataMembers.AddRange(dt.Columns["DiskUsed"].ToString());
            DiskSpaceWebChartControl1.Series[0].Visible = true;
            DiskSpaceWebChartControl1.Series[1].DataSource = dt;
            DiskSpaceWebChartControl1.Series[1].ArgumentDataMember = dt.Columns["DiskName"].ToString();
            DiskSpaceWebChartControl1.Series[1].ValueDataMembers.AddRange(dt.Columns["DiskFree"].ToString());
            DiskSpaceWebChartControl1.Series[1].Visible = true;
        }
      
        public void SetGraphForCPULync(string paramGraph, string serverName)
        {
            cpuWebChartControl.Series.Clear();
			DataTable dt = VSWebBL.DashboardBL.LyncDetailsBAL.Ins.SetGraphForCPULync(paramGraph, serverName, ServerTypeId);

            Series series = null;
            series = new Series("LyncServer", ViewType.Line);
            series.Visible = true;
            series.ArgumentDataMember = dt.Columns["dtfrom"].ToString();

            ValueDataMemberCollection seriesValueDataMembers = (ValueDataMemberCollection)series.ValueDataMembers;
            seriesValueDataMembers.AddRange(dt.Columns["maxval"].ToString());
            cpuWebChartControl.Series.Add(series);

            ((XYDiagram)cpuWebChartControl.Diagram).PaneLayoutDirection = PaneLayoutDirection.Horizontal;

            XYDiagram seriesXY = (XYDiagram)cpuWebChartControl.Diagram;
            seriesXY.AxisY.Title.Text = "CPU";
            seriesXY.AxisY.Title.Visible = true;
            seriesXY.AxisX.Title.Text = "Date/Time";
            seriesXY.AxisX.Title.Visible = true;
            cpuWebChartControl.Legend.Visible = false;

            // ((SplineSeriesView)series.View).LineTensionPercent = 100;
            ((LineSeriesView)series.View).LineMarkerOptions.Size = 4;
            ((LineSeriesView)series.View).LineMarkerOptions.Color = Color.White;

            AxisBase axis = ((XYDiagram)cpuWebChartControl.Diagram).AxisX;
            //4/18/2014 NS commented out for VSPLUS-312
            //axis.DateTimeGridAlignment = DateTimeMeasurementUnit.Hour;
            //5/13/2014 NS commented out for VSPLUS-621
            //axis.GridSpacingAuto = false;
            //axis.MinorCount = 15;
            //axis.GridSpacing = 0.5;
            axis.Range.SideMarginsEnabled = false;
            axis.GridLines.Visible = false;
            //axis.DateTimeOptions.Format = DateTimeFormat.Custom;
            //axis.DateTimeOptions.FormatString = "dd/MM HH:mm";
            ((LineSeriesView)series.View).Color = Color.Blue;

            AxisBase axisy = ((XYDiagram)cpuWebChartControl.Diagram).AxisY;
            axisy.Range.AlwaysShowZeroLevel = false;
            axisy.Range.SideMarginsEnabled = true;
            axisy.GridLines.Visible = true;
            cpuWebChartControl.DataSource = dt;
            cpuWebChartControl.DataBind();

            //return dt;     
        }

        //protected void cpuASPxRadioButtonList_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    SetGraphForCPU(cpuASPxRadioButtonList.Value.ToString(), servernamelbl.Text);
        //}

        public void SetGraphForMemoryLync(string paramGraph, string serverName)
        {
            memWebChartControl.Series.Clear();
			DataTable dt = VSWebBL.DashboardBL.LyncDetailsBAL.Ins.SetGraphForMemoryLync(paramGraph, serverName, ServerTypeId);

            Series series = null;
            series = new Series("LyncServer", ViewType.Line);
            series.Visible = true;
            series.ArgumentDataMember = dt.Columns["dtfrom"].ToString();

            ValueDataMemberCollection seriesValueDataMembers = (ValueDataMemberCollection)series.ValueDataMembers;
            seriesValueDataMembers.AddRange(dt.Columns["maxval"].ToString());
            memWebChartControl.Series.Add(series);

            ((XYDiagram)memWebChartControl.Diagram).PaneLayoutDirection = PaneLayoutDirection.Horizontal;

            XYDiagram seriesXY = (XYDiagram)memWebChartControl.Diagram;
            seriesXY.AxisY.Title.Text = "Memory";
            seriesXY.AxisY.Title.Visible = true;
            seriesXY.AxisX.Title.Text = "Date/Time";
            seriesXY.AxisX.Title.Visible = true;
            memWebChartControl.Legend.Visible = false;

            //((SplineSeriesView)series.View).LineTensionPercent = 100;
            ((LineSeriesView)series.View).LineMarkerOptions.Size = 4;
            ((LineSeriesView)series.View).LineMarkerOptions.Color = Color.White;

            AxisBase axis = ((XYDiagram)memWebChartControl.Diagram).AxisX;
            //4/18/2014 NS commented out for VSPLUS-312
            //axis.DateTimeGridAlignment = DateTimeMeasurementUnit.Hour;
            //5/13/2014 NS commented out for VSPLUS-621
            //axis.GridSpacingAuto = false;
            //axis.MinorCount = 15;
            //axis.GridSpacing = 0.5;
            axis.Range.SideMarginsEnabled = false;
            axis.GridLines.Visible = false;
            //axis.DateTimeOptions.Format = DateTimeFormat.Custom;
            //axis.DateTimeOptions.FormatString = "dd/MM HH:mm";

            ((LineSeriesView)series.View).Color = Color.Blue;

            AxisBase axisy = ((XYDiagram)memWebChartControl.Diagram).AxisY;
            axisy.Range.AlwaysShowZeroLevel = false;
            axisy.Range.SideMarginsEnabled = true;
            axisy.GridLines.Visible = true;
            memWebChartControl.DataSource = dt;
            memWebChartControl.DataBind();
        }

        //protected void memASPxRadioButtonList_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    SetGraphForMemory(memASPxRadioButtonList.Value.ToString(), servernamelbl.Text);
        //}

    
        //// public void FillCombobox()
        // {
        //    DataTable trafictab = VSWebBL.DashboardBL.DashboardBL.Ins.GetCombobox();
        //   MailComboBox.DataSource = trafictab;
        //   MailComboBox.TextField = "NameandType";
        //   MailComboBox.DataBind();
        // }

      

        //protected void mailASPxRadioButtonList_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    SetGraphForMail(mailASPxRadioButtonList.Value.ToString(), servernamelbl.Text);
        //}


        //DataBase Code

        public void SetGraphforLyncChatLatency(string paramGraph, string serverName)
        {
            chatlatencyWebChartControl1.Series.Clear();
			DataTable dt = VSWebBL.DashboardBL.LyncDetailsBAL.Ins.SetGraphforLyncChatLatency(paramGraph, serverName, ServerTypeId);

            Series series = null;
            series = new Series("DominoServer", ViewType.Line);
            series.Visible = true;
            series.ArgumentDataMember = dt.Columns["dtfrom"].ToString();

            ValueDataMemberCollection seriesValueDataMembers = (ValueDataMemberCollection)series.ValueDataMembers;
            seriesValueDataMembers.AddRange(dt.Columns["maxval"].ToString());
            chatlatencyWebChartControl1.Series.Add(series);

            ((XYDiagram)chatlatencyWebChartControl1.Diagram).PaneLayoutDirection = PaneLayoutDirection.Horizontal;

            XYDiagram seriesXY = (XYDiagram)chatlatencyWebChartControl1.Diagram;
			seriesXY.AxisY.Title.Text = "Skype for Business ChatLatency";
            seriesXY.AxisY.Title.Visible = true;
            seriesXY.AxisX.Title.Text = "Date/Time";
            seriesXY.AxisX.Title.Visible = true;
            chatlatencyWebChartControl1.Legend.Visible = false;

            //((SplineSeriesView)series.View).LineTensionPercent = 100;
            ((LineSeriesView)series.View).LineMarkerOptions.Size = 4;
            ((LineSeriesView)series.View).LineMarkerOptions.Color = Color.White;

            AxisBase axis = ((XYDiagram)chatlatencyWebChartControl1.Diagram).AxisX;
            //4/18/2014 NS commented out for VSPLUS-312
            //axis.DateTimeGridAlignment = DateTimeMeasurementUnit.Hour;
            //axis.MinorCount = 15;
            //axis.GridSpacing = 0.5;
            axis.Range.SideMarginsEnabled = false;
            axis.GridLines.Visible = false;
            //axis.DateTimeOptions.Format = DateTimeFormat.Custom;

            ((LineSeriesView)series.View).Color = Color.Blue;

            //10/9/2013 NS moved the ds assignment prior to setting the grid spacing option

            chatlatencyWebChartControl1.DataSource = dt;
            chatlatencyWebChartControl1.DataBind();
            AxisBase axisy = ((XYDiagram)usersWebChartControl.Diagram).AxisY;
            axisy.Range.AlwaysShowZeroLevel = false;
            axisy.Range.SideMarginsEnabled = true;
            axisy.GridLines.Visible = true;



            //10/8/2013 NS added
            seriesXY.AxisY.NumericOptions.Format = NumericFormat.Number;
            seriesXY.AxisY.NumericOptions.Precision = 0;
            seriesXY.AxisY.GridSpacingAuto = true;

            double min = Convert.ToDouble(((XYDiagram)usersWebChartControl.Diagram).AxisY.Range.MinValue);
            double max = Convert.ToDouble(((XYDiagram)usersWebChartControl.Diagram).AxisY.Range.MaxValue);

            int gs = (int)((max - min) / 5);

            if (gs == 0)
            {
                gs = 1;
                seriesXY.AxisY.GridSpacingAuto = false;
                seriesXY.AxisY.GridSpacing = gs;
            }

        }
        protected void chatlatencyASPxRadioButtonList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetGraphforLyncChatLatency(chatlatencyASPxRadioButtonList1.Value.ToString(), servernamelbl.Text);
        }
        public void SetGraphforLyncGroupChatLatency(string paramGraph, string serverName)
        {
            GroupChatWebChartControl1.Series.Clear();
			DataTable dt = VSWebBL.DashboardBL.LyncDetailsBAL.Ins.SetGraphforLyncGroupChatLatency(paramGraph, serverName, ServerTypeId);

            Series series = null;
            series = new Series("DominoServer", ViewType.Line);
            series.Visible = true;
            series.ArgumentDataMember = dt.Columns["dtfrom"].ToString();

            ValueDataMemberCollection seriesValueDataMembers = (ValueDataMemberCollection)series.ValueDataMembers;
            seriesValueDataMembers.AddRange(dt.Columns["maxval"].ToString());
            GroupChatWebChartControl1.Series.Add(series);

            ((XYDiagram)GroupChatWebChartControl1.Diagram).PaneLayoutDirection = PaneLayoutDirection.Horizontal;

            XYDiagram seriesXY = (XYDiagram)GroupChatWebChartControl1.Diagram;
			seriesXY.AxisY.Title.Text = "Skype for Business GroupChatLatency";
            seriesXY.AxisY.Title.Visible = true;
            seriesXY.AxisX.Title.Text = "Date/Time";
            seriesXY.AxisX.Title.Visible = true;
            GroupChatWebChartControl1.Legend.Visible = false;

            //((SplineSeriesView)series.View).LineTensionPercent = 100;
            ((LineSeriesView)series.View).LineMarkerOptions.Size = 4;
            ((LineSeriesView)series.View).LineMarkerOptions.Color = Color.White;

            AxisBase axis = ((XYDiagram)GroupChatWebChartControl1.Diagram).AxisX;
            //4/18/2014 NS commented out for VSPLUS-312
            //axis.DateTimeGridAlignment = DateTimeMeasurementUnit.Hour;
            //axis.MinorCount = 15;
            //axis.GridSpacing = 0.5;
            axis.Range.SideMarginsEnabled = false;
            axis.GridLines.Visible = false;
            //axis.DateTimeOptions.Format = DateTimeFormat.Custom;

            ((LineSeriesView)series.View).Color = Color.Blue;

            //10/9/2013 NS moved the ds assignment prior to setting the grid spacing option

            GroupChatWebChartControl1.DataSource = dt;
            GroupChatWebChartControl1.DataBind();
            AxisBase axisy = ((XYDiagram)usersWebChartControl.Diagram).AxisY;
            axisy.Range.AlwaysShowZeroLevel = false;
            axisy.Range.SideMarginsEnabled = true;
            axisy.GridLines.Visible = true;



            //10/8/2013 NS added
            seriesXY.AxisY.NumericOptions.Format = NumericFormat.Number;
            seriesXY.AxisY.NumericOptions.Precision = 0;
            seriesXY.AxisY.GridSpacingAuto = true;

            double min = Convert.ToDouble(((XYDiagram)usersWebChartControl.Diagram).AxisY.Range.MinValue);
            double max = Convert.ToDouble(((XYDiagram)usersWebChartControl.Diagram).AxisY.Range.MaxValue);

            int gs = (int)((max - min) / 5);

            if (gs == 0)
            {
                gs = 1;
                seriesXY.AxisY.GridSpacingAuto = false;
                seriesXY.AxisY.GridSpacing = gs;
            }

        }
        protected void GroupChatASPxRadioButtonList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetGraphforLyncGroupChatLatency(GroupChatASPxRadioButtonList1.Value.ToString(), servernamelbl.Text);
        }
     

        protected void MonitoredDBGridView_HtmlDataCellPrepared(object sender, DevExpress.Web.ASPxGridViewTableDataCellEventArgs e)
        {
            if (e.DataColumn.FieldName == "Status" && (e.CellValue.ToString() == "OK" || e.CellValue.ToString() == "Scanning"))
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
            else if (e.DataColumn.FieldName == "Status" && e.CellValue.ToString() == "disabled")
            {
                e.Cell.BackColor = System.Drawing.Color.Gray;
                // e.Cell.ForeColor = System.Drawing.Color.White;
            }
            else if (e.DataColumn.FieldName == "Status")
            {
                e.Cell.BackColor = System.Drawing.Color.Yellow;
                // e.DataColumn.GroupFooterCellStyle.ForeColor = System.Drawing.Color.Yellow;
            }

        }

    
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

        public void SetGraphForLyncEnabledUsers(string paramGraph, string serverName)
        {
            usersWebChartControl.Series.Clear();
            DataTable dt = VSWebBL.DashboardBL.LyncDetailsBAL.Ins.SetGraphForLyncEnabledUsers(paramGraph, serverName, ServerTypeId);

            Series series = null;
            series = new Series("DominoServer", ViewType.Line);
            series.Visible = true;
            series.ArgumentDataMember = dt.Columns["dtfrom"].ToString();

            ValueDataMemberCollection seriesValueDataMembers = (ValueDataMemberCollection)series.ValueDataMembers;
            seriesValueDataMembers.AddRange(dt.Columns["maxval"].ToString());
            usersWebChartControl.Series.Add(series);

            ((XYDiagram)usersWebChartControl.Diagram).PaneLayoutDirection = PaneLayoutDirection.Horizontal;

            XYDiagram seriesXY = (XYDiagram)usersWebChartControl.Diagram;
			seriesXY.AxisY.Title.Text = "Skype for Business EnabledUsers";
            seriesXY.AxisY.Title.Visible = true;
            seriesXY.AxisX.Title.Text = "Date/Time";
            seriesXY.AxisX.Title.Visible = true;
            usersWebChartControl.Legend.Visible = false;

            //((SplineSeriesView)series.View).LineTensionPercent = 100;
            ((LineSeriesView)series.View).LineMarkerOptions.Size = 4;
            ((LineSeriesView)series.View).LineMarkerOptions.Color = Color.White;

            AxisBase axis = ((XYDiagram)usersWebChartControl.Diagram).AxisX;
            //4/18/2014 NS commented out for VSPLUS-312
            //axis.DateTimeGridAlignment = DateTimeMeasurementUnit.Hour;
            //axis.MinorCount = 15;
            //axis.GridSpacing = 0.5;
            axis.Range.SideMarginsEnabled = false;
            axis.GridLines.Visible = false;
            //axis.DateTimeOptions.Format = DateTimeFormat.Custom;

            ((LineSeriesView)series.View).Color = Color.Blue;

            //10/9/2013 NS moved the ds assignment prior to setting the grid spacing option
            usersWebChartControl.DataSource = dt;
            usersWebChartControl.DataBind();

            AxisBase axisy = ((XYDiagram)usersWebChartControl.Diagram).AxisY;
            axisy.Range.AlwaysShowZeroLevel = false;
            axisy.Range.SideMarginsEnabled = true;
            axisy.GridLines.Visible = true;

            //10/8/2013 NS added
            seriesXY.AxisY.NumericOptions.Format = NumericFormat.Number;
            seriesXY.AxisY.NumericOptions.Precision = 0;
            seriesXY.AxisY.GridSpacingAuto = true;

            double min = Convert.ToDouble(((XYDiagram)usersWebChartControl.Diagram).AxisY.Range.MinValue);
            double max = Convert.ToDouble(((XYDiagram)usersWebChartControl.Diagram).AxisY.Range.MaxValue);

            int gs = (int)((max - min) / 5);

            if (gs == 0)
            {
                gs = 1;
                seriesXY.AxisY.GridSpacingAuto = false;
                seriesXY.AxisY.GridSpacing = gs;
            }

        }

        protected void usersASPxRadioButtonList_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetGraphForLyncEnabledUsers(usersASPxRadioButtonList.Value.ToString(), servernamelbl.Text);
        }
     


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

        protected void AllDBGridView_HtmlDataCellPrepared(object sender, DevExpress.Web.ASPxGridViewTableDataCellEventArgs e)
        {
            if (e.DataColumn.FieldName == "Status" && (e.CellValue.ToString() == "OK" || e.CellValue.ToString() == "Scanning"))
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
            else if (e.DataColumn.FieldName == "Status" && e.CellValue.ToString() == "disabled")
            {
                e.Cell.BackColor = System.Drawing.Color.Gray;
                // e.Cell.ForeColor = System.Drawing.Color.White;
            }
            else if (e.DataColumn.FieldName == "Status")
            {
                e.Cell.BackColor = System.Drawing.Color.Yellow;
                // e.DataColumn.GroupFooterCellStyle.ForeColor = System.Drawing.Color.Yellow;
            }

        }

		//protected void BackButton_Click(object sender, EventArgs e)
		//{
		//    if (Session["BackURL"] != "" && Session["BackURL"] != null)
		//    {
		//        Response.Redirect(Session["BackURL"].ToString());
		//        Session["BackURL"] = "";

		//    }
		//}

      


                //if (filepath == "")
                //{
                //    Response.Write("<script>window.open('http://" + HostName + "/" + fileName + "?OpenDatabase','Open Database','width=500px height=100px',1)</script>");
                //}
                //else
                //{
                //    Response.Write("<script>window.open('http://" + HostName + "/" + filepath + "/" + fileName + "?OpenDatabase','Open Database','width=500px height=100px',1)</script>");
                //}
          
        

      
   
     

        protected void OpenDBButton_Click(object sender, EventArgs e)
        {

        }

      

     

        //public void UpdateButtonVisibility()
        //{
        //    bool isadmin = false;

        //    if (sa.Rows.Count > 0)
        //    {
        //        if (sa.Rows[0]["IsConsoleComm"].ToString() == "True")
        //        {
        //            isadmin = true;
        //        }
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
        //        EditInConfigButton.Visible = true;
        //    }
        //}

        

      

      
		//protected void ScanButton_Click(object sender, EventArgs e)
		//{
		//    try
		//    {
		//        Status StatusObj = new Status();
		//        StatusObj.Name = Request.QueryString["Name"];
		//        StatusObj.Type = Request.QueryString["Type"];

		//        bool bl = VSWebBL.StatusBL.StatusTBL.Ins.UpdateforScan(StatusObj);

		//        if (Request.QueryString["Type"] == "Domino")
		//        {
		//            bl = VSWebBL.SettingBL.SettingsBL.Ins.UpdateSvalue("ScanDominoASAP", StatusObj.Name, VSWeb.Constants.Constants.SysString);
		//        }
		//        else if (Request.QueryString["Type"] == "Mail")
		//        {
		//            bl = VSWebBL.SettingBL.SettingsBL.Ins.UpdateSvalue("ScanMailServiceASAP", StatusObj.Name, VSWeb.Constants.Constants.SysString);
		//        }
		//        else if (Request.QueryString["Type"] == "Network Device")
		//        {
		//            bl = VSWebBL.SettingBL.SettingsBL.Ins.UpdateSvalue("ScanNetworkDeviceASAP", StatusObj.Name, VSWeb.Constants.Constants.SysString);
		//        }
		//        else if (Request.QueryString["Type"] == "Sametime Server")
		//        {
		//            bl = VSWebBL.SettingBL.SettingsBL.Ins.UpdateSvalue("ScanSametimeASAP", StatusObj.Name, VSWeb.Constants.Constants.SysString);
		//        }
		//        else if (Request.QueryString["Type"] == "BES")
		//        {
		//            bl = VSWebBL.SettingBL.SettingsBL.Ins.UpdateSvalue("ScanBlackBerryASAP", StatusObj.Name, VSWeb.Constants.Constants.SysString);
		//        }
		//        else if (Request.QueryString["Type"] == "URL")
		//        {
		//            bl = VSWebBL.SettingBL.SettingsBL.Ins.UpdateSvalue("ScanURLASAP", StatusObj.Name, VSWeb.Constants.Constants.SysString);
		//        }
		//        else if (Request.QueryString["Type"] == "Exchange")
		//        {
		//            bl = VSWebBL.SettingBL.SettingsBL.Ins.UpdateSvalue("ScanExchangeASAP", StatusObj.Name, VSWeb.Constants.Constants.SysString);
		//        }
		//        else if (Request.QueryString["Type"] == "SharePoint")
		//        {
		//            bl = VSWebBL.SettingBL.SettingsBL.Ins.UpdateSvalue("ScanSharePointASAP", StatusObj.Name, VSWeb.Constants.Constants.SysString);
		//        }
		//        else if (Request.QueryString["Type"] == "Lync")
		//        {
		//            bl = VSWebBL.SettingBL.SettingsBL.Ins.UpdateSvalue("LyncASAP", StatusObj.Name, VSWeb.Constants.Constants.SysString);
		//        }
		//    }
		//    catch (Exception ex)
		//    {
		//        Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
		//        throw ex;
		//    }
		//}

		//protected void EditInConfigButton_Click(object sender, EventArgs e)
		//{
		//    //1/2/2014 NS added
		//    string id = "";
		//    string name = "";
		//    Session["Submenu"] = "LyncServers";
		//    if (Request.QueryString["Name"] != "" && Request.QueryString["Name"] != null)
		//    {
		//        id = (VSWebBL.SecurityBL.ServersBL.Ins.GetServerIDbyServerName(Request.QueryString["Name"].ToString())).ToString();
		//        name = Request.QueryString["Name"].ToString();
		//        DataTable dt = VSWebBL.SecurityBL.ServersBL.Ins.GetServerDetailsByName1(name);
		//        string Loc = dt.Rows[0]["Location"].ToString();
		//        string Cat = dt.Rows[0]["ServerType"].ToString();
		//        //servernamelbl.Text

		//        Response.Redirect("~/Configurator/ExchangeServer.aspx?ID=" + id + "&Name=" + name + "&Cat=" + dt.Rows[0]["ServerType"].ToString() + "&Loc=" + dt.Rows[0]["Location"].ToString() , false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
		//        Context.ApplicationInstance.CompleteRequest();
		//    }
		//}

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
                          EndDate, MaintType, MaintDaysList, EndDateIndicator, serverIDValues, s, rem, 1, true, servertypeIDValues, "true", "1");
                }
                if (update == true)
                {
                    //10/3/2014 NS modified for VSPLUS-990
                    SuccessMsg.InnerHtml = "Monitoring for " + servernamelbl.Text + " has been temporarily suspended for a duration of " + TbDuration.Text + " minutes."+
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
				string name = "";
				Session["Submenu"] = "MSServers";
				if (Request.QueryString["Name"] != "" && Request.QueryString["Name"] != null)
				{
					id = (VSWebBL.SecurityBL.ServersBL.Ins.GetServerIDbyServerName(Request.QueryString["Name"].ToString())).ToString();
					name = Request.QueryString["Name"].ToString();
					DataTable dt = VSWebBL.SecurityBL.ServersBL.Ins.GetServerDetailsByName1(name);
					string Loc = dt.Rows[0]["Location"].ToString();
					string Cat = dt.Rows[0]["ServerType"].ToString();
					//servernamelbl.Text

					Response.Redirect("~/Configurator/LyncServer.aspx?ID=" + id + "&Name=" + name + "&Cat=" + dt.Rows[0]["ServerType"].ToString() + "&Loc=" + dt.Rows[0]["Location"].ToString() + "&ipaddr=" + dt.Rows[0]["ipaddress"].ToString(), false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
					Context.ApplicationInstance.CompleteRequest();
				}
			}
			else if (e.Item.Name == "SuspendItem")
			{
				ASPxPopupControl1.HeaderText = "Suspend Temporarily";
				ASPxPopupControl1.ShowOnPageLoad = true;
			}
		}
     


    }
}

