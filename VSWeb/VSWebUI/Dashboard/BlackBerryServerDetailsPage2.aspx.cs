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
using System.Text;
using System.Windows.Forms;
//using VSWebBL;

namespace VSWebUI.Configurator
{
    public partial class BlackBerryServerDetailsPage2 : System.Web.UI.Page
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
                    //diskspaceASPxRoundPanel.Visible = false;
                    //cpuASPxRoundPanel.Visible = false;
                    //memASPxRoundPanel.Visible = false;
                    //usersASPxRoundPanel.Visible = false;
                    //mailASPxRoundPanel.Visible = false;
                    //performanceASPxRoundPanel.Width = 1000;
                    //performanceWebChartControl.Width = 800;
                    ASPxPageControl1.TabPages[1].ClientVisible = false;
                    ASPxPageControl1.TabPages[2].ClientVisible = false;
                }

                if (Request.QueryString["Name"] != "" && Request.QueryString["Name"] != null)
                {
                    servernamelbl.Text = Request.QueryString["Name"];

                    //linkDiskSpaceMore.NavigateUrl = "DiskHealth.aspx?server=" + servernamelbl.Text;
                    //CpuHyperLink.NavigateUrl = "DominoServerStatisticsDetail.aspx?name=Cpu&server=" + servernamelbl.Text;
                    //MemHyperLink.NavigateUrl = "DominoServerStatisticsDetail.aspx?name=Mem&server=" + servernamelbl.Text;
                    //PerfrmHyperLink.NavigateUrl = "DominoServerStatisticsDetail.aspx?name=Perfm&server=" + servernamelbl.Text;
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
            //SetGraph("hh", servernamelbl.Text);
            //SetGraphForDiskSpace(servernamelbl.Text);
            //SetGraphForCPU("hh", servernamelbl.Text);
            //SetGraphForMemory("hh", servernamelbl.Text);
            //SetGraphForUsers("hh", servernamelbl.Text);
            SetGraphForMail(servernamelbl.Text);
            //12/4/2013 NS commented out - the page should only load on tab click
            //2/12/2014 NS uncommented out - tab click event takes too long, we will use a link to open Web Admin in a new window instead
            SetupWebAdmin();
            SetGraphforBESmsgsent("hh", servernamelbl.Text);
           
            SetGraphforBESmsgrecvd("hh", servernamelbl.Text);

            // DataBase Code
            if (!IsPostBack && !IsCallback)
            {
                // performanceASPxRadioButtonList.Items.RemoveAt(0);
                // performanceASPxRadioButtonList.Items[0].Selected = true;
                FillGrid();
                //FillALLGrid();
                //FillMonitoredGrid();
                FillMonMoniteredGrid();
                Fillmaintenancegrid();
                FillAlertHistory();
                FillOutageTab();
                getBlackberry();
                FillHealthAssessmentStatusGrid(Request.QueryString["Type"].ToString(), servernamelbl.Text);//07-07-2014

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
                FillGridfromSession();
                //FillALLGridfromSession();
                //FillgridfromSession();
                FillNonMongridfromSession();
                Fillmaintenancegridfromsession();
                FillAlertHistoryfromSession();
                FilloutagefromSession();
            }
            //2/6/2013 NS added
            UpdateButtonVisibility();
        }
        public void SetGraphforBESmsgsent(string paramGraph, string serverName)
        {
            BESmsgsentWebChartControl1.Series.Clear();
            DataTable dt = VSWebBL.DashboardBL.BESdetailsBAL.Ins.SetGraphforBESmsgsent(paramGraph, serverName);

            Series series = null;
            series = new Series("BlackBerryServers", ViewType.Line);
            series.Visible = true;
            series.ArgumentDataMember = dt.Columns["dtfrom"].ToString();

            ValueDataMemberCollection seriesValueDataMembers = (ValueDataMemberCollection)series.ValueDataMembers;
            seriesValueDataMembers.AddRange(dt.Columns["maxval"].ToString());
            BESmsgsentWebChartControl1.Series.Add(series);

            ((XYDiagram)BESmsgsentWebChartControl1.Diagram).PaneLayoutDirection = PaneLayoutDirection.Horizontal;

            XYDiagram seriesXY = (XYDiagram)BESmsgsentWebChartControl1.Diagram;
            seriesXY.AxisY.Title.Text = "Messages Sent";
            seriesXY.AxisY.Title.Visible = true;
            seriesXY.AxisX.Title.Text = "Date/Time";
            seriesXY.AxisX.Title.Visible = true;
            BESmsgsentWebChartControl1.Legend.Visible = false;

            //((SplineSeriesView)series.View).LineTensionPercent = 100;
            ((LineSeriesView)series.View).LineMarkerOptions.Size = 4;
            ((LineSeriesView)series.View).LineMarkerOptions.Color = Color.White;

            AxisBase axis = ((XYDiagram)BESmsgsentWebChartControl1.Diagram).AxisX;
            //4/18/2014 NS commented out for VSPLUS-312
            //axis.DateTimeGridAlignment = DateTimeMeasurementUnit.Hour;
            //axis.MinorCount = 15;
            //axis.GridSpacing = 0.5;
            axis.Range.SideMarginsEnabled = false;
            axis.GridLines.Visible = false;
            //axis.DateTimeOptions.Format = DateTimeFormat.Custom;

            ((LineSeriesView)series.View).Color = Color.Blue;

            //10/9/2013 NS moved the ds assignment prior to setting the grid spacing option

            BESmsgsentWebChartControl1.DataSource = dt;
            BESmsgsentWebChartControl1.DataBind();
            AxisBase axisy = ((XYDiagram)BESmsgsentWebChartControl1.Diagram).AxisY;
            axisy.Range.AlwaysShowZeroLevel = false;
            axisy.Range.SideMarginsEnabled = true;
            axisy.GridLines.Visible = true;



            //10/8/2013 NS added
            seriesXY.AxisY.NumericOptions.Format = NumericFormat.Number;
            seriesXY.AxisY.NumericOptions.Precision = 0;
            seriesXY.AxisY.GridSpacingAuto = true;

            double min = Convert.ToDouble(((XYDiagram)BESmsgsentWebChartControl1.Diagram).AxisY.Range.MinValue);
            double max = Convert.ToDouble(((XYDiagram)BESmsgsentWebChartControl1.Diagram).AxisY.Range.MaxValue);

            int gs = (int)((max - min) / 5);

            if (gs == 0)
            {
                gs = 1;
                seriesXY.AxisY.GridSpacingAuto = false;
                seriesXY.AxisY.GridSpacing = gs;
            }

        }

        protected void BESmsgsentASPxRadioButtonList_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetGraphforBESmsgsent(BESmsgsentASPxRadioButtonList1.Value.ToString(), servernamelbl.Text);
        }
       
        public void SetGraphforBESmsgrecvd(string paramGraph, string serverName)
        {
            BESmsgrecvdWebChartControl1.Series.Clear();
            DataTable dt = VSWebBL.DashboardBL.BESdetailsBAL.Ins.SetGraphforBESmsgrecvd(paramGraph, serverName);

            Series series = null;
            series = new Series("BlackBerryServers", ViewType.Line);
            series.Visible = true;
            series.ArgumentDataMember = dt.Columns["dtfrom"].ToString();

            ValueDataMemberCollection seriesValueDataMembers = (ValueDataMemberCollection)series.ValueDataMembers;
            seriesValueDataMembers.AddRange(dt.Columns["maxval"].ToString());
            BESmsgrecvdWebChartControl1.Series.Add(series);

            ((XYDiagram)BESmsgrecvdWebChartControl1.Diagram).PaneLayoutDirection = PaneLayoutDirection.Horizontal;

            XYDiagram seriesXY = (XYDiagram)BESmsgrecvdWebChartControl1.Diagram;
            seriesXY.AxisY.Title.Text = "Messages Received";
            seriesXY.AxisY.Title.Visible = true;
            seriesXY.AxisX.Title.Text = "Date/Time";
            seriesXY.AxisX.Title.Visible = true;
            BESmsgrecvdWebChartControl1.Legend.Visible = false;

            //((SplineSeriesView)series.View).LineTensionPercent = 100;
            ((LineSeriesView)series.View).LineMarkerOptions.Size = 4;
            ((LineSeriesView)series.View).LineMarkerOptions.Color = Color.White;

            AxisBase axis = ((XYDiagram)BESmsgrecvdWebChartControl1.Diagram).AxisX;
            //4/18/2014 NS commented out for VSPLUS-312
            //axis.DateTimeGridAlignment = DateTimeMeasurementUnit.Hour;
            //axis.MinorCount = 15;
            //axis.GridSpacing = 0.5;
            axis.Range.SideMarginsEnabled = false;
            axis.GridLines.Visible = false;
            //axis.DateTimeOptions.Format = DateTimeFormat.Custom;

            ((LineSeriesView)series.View).Color = Color.Blue;

            //10/9/2013 NS moved the ds assignment prior to setting the grid spacing option

            BESmsgrecvdWebChartControl1.DataSource = dt;
            BESmsgrecvdWebChartControl1.DataBind();
            AxisBase axisy = ((XYDiagram)BESmsgrecvdWebChartControl1.Diagram).AxisY;
            axisy.Range.AlwaysShowZeroLevel = false;
            axisy.Range.SideMarginsEnabled = true;
            axisy.GridLines.Visible = true;



            //10/8/2013 NS added
            seriesXY.AxisY.NumericOptions.Format = NumericFormat.Number;
            seriesXY.AxisY.NumericOptions.Precision = 0;
            seriesXY.AxisY.GridSpacingAuto = true;

            double min = Convert.ToDouble(((XYDiagram)BESmsgrecvdWebChartControl1.Diagram).AxisY.Range.MinValue);
            double max = Convert.ToDouble(((XYDiagram)BESmsgrecvdWebChartControl1.Diagram).AxisY.Range.MaxValue);

            int gs = (int)((max - min) / 5);

            if (gs == 0)
            {
                gs = 1;
                seriesXY.AxisY.GridSpacingAuto = false;
                seriesXY.AxisY.GridSpacing = gs;
            }

        }

        protected void BESmsgrecvdASPxRadioButtonList_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetGraphforBESmsgrecvd(BESmsgrecvdASPxRadioButtonList1.Value.ToString(), servernamelbl.Text);
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

        //        // Create a constant line.
        //        ConstantLine constantLine1 = new ConstantLine("Constant Line 1");
        //        diagram.AxisY.ConstantLines.Add(constantLine1);

        //        // Define its axis value.
        //        constantLine1.AxisValue = 2000;

        //        // Customize the behavior of the constant line.
        //        // constantLine1.Visible = true;
        //        //constantLine1.ShowInLegend = true;
        //        // constantLine1.LegendText = "Some Threshold";
        //        constantLine1.ShowBehind = true;

        //        // Customize the constant line's title.
        //        constantLine1.Title.Visible = true;
        //        constantLine1.Title.Text = "Threshold:2000";
        //        constantLine1.Title.TextColor = Color.Red;
        //        // constantLine1.Title.Antialiasing = false;
        //        //constantLine1.Title.Font = new Font("Tahoma", 14, FontStyle.Bold);
        //        constantLine1.Title.ShowBelowLine = true;
        //        constantLine1.Title.Alignment = ConstantLineTitleAlignment.Far;

        //        // Customize the appearance of the constant line.
        //        constantLine1.Color = Color.Red;
        //        constantLine1.LineStyle.DashStyle = DashStyle.Solid;
        //        constantLine1.LineStyle.Thickness = 2;

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

        //        throw ex;
        //    }

        //}



        //public void SetGraphForDiskSpace(string serverName)
        //{
        //    if (!IsPostBack)
        //    {
        //        //int cnt = 0;
        //        diskspaceWebChartControl.Series.Clear();
        //        DataTable dt = VSWebBL.DashboardBL.DominoServerDetailsBL.Ins.SetGraphForDiskSpace(serverName);
        //        diskspaceWebChartControl.DataSource = dt;

        //        double[] double1 = new double[dt.Rows.Count];
        //        double[] double2 = new double[dt.Rows.Count];

        //        Series series = null;

        //        //diskspaceWebChartControl.Series.Clear();

        //        for (int i = 0; i < dt.Rows.Count; i++)
        //        {
        //            if (series != null)
        //            {
        //                diskspaceWebChartControl.Series.Add(series);
        //                diskspaceWebChartControl.DataBind();
        //            }


        //            series = new Series(dt.Rows[i]["DiskName"].ToString(), ViewType.Pie);

        //            string val1 = dt.Rows[i]["DiskFree"].ToString();
        //            string val2 = dt.Rows[i]["DiskUsed"].ToString();

        //            if (val1 != "" && val2 != "")
        //            {
        //                double1[i] = Convert.ToDouble(dt.Rows[i]["DiskFree"].ToString());
        //                double2[i] = Convert.ToDouble(dt.Rows[i]["DiskUsed"].ToString());

        //                //if (dt.Rows[i]["DiskFree"] != "" && dt.Rows[i]["DiskFree"] != null)
        //                //    double1[i] = Convert.ToDouble(dt.Rows[i]["DiskFree"].ToString());
        //                //if (dt.Rows[i]["DiskUsed"] != "" && dt.Rows[i]["DiskUsed"] != null)
        //                //    double2[i] = Convert.ToDouble(dt.Rows[i]["DiskUsed"].ToString());

        //                series.Points.Add(new SeriesPoint("Disk Free", double1[i]));
        //                series.Points.Add(new SeriesPoint("Disk Used", double2[i]));
        //                series.ShowInLegend = true;

        //                series.LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;
        //                PieSeriesLabel seriesLabel = (PieSeriesLabel)series.Label;
        //                seriesLabel.Position = PieSeriesLabelPosition.Radial;
        //                seriesLabel.BackColor = System.Drawing.Color.Transparent;
        //                seriesLabel.TextColor = System.Drawing.Color.Black;

        //                PieSeriesView seriesView = (PieSeriesView)series.View;
        //                seriesView.Titles.Add(new SeriesTitle());
        //                seriesView.Titles[0].Dock = ChartTitleDockStyle.Bottom;

        //                seriesView.Titles[0].Text = series.Name.Substring(5);
        //                seriesView.Titles[0].Visible = true;
        //                seriesView.Titles[0].WordWrap = true;
        //            }
        //        }

        //        if (series != null)
        //        {
        //            diskspaceWebChartControl.Series.Add(series);
        //            diskspaceWebChartControl.DataBind();
        //        }

        //        for (int c = 0; c < diskspaceWebChartControl.Series.Count; c++)
        //        {
        //            if (c == 0)
        //            {
        //                PiePointOptions seriesPointOptions = (PiePointOptions)series.LegendPointOptions;
        //                series.LegendPointOptions.PointView = PointView.Argument;
        //                diskspaceWebChartControl.Series[0].ShowInLegend = true;
        //                diskspaceWebChartControl.Series[0].LegendPointOptions.PointView = PointView.Argument;
        //                diskspaceWebChartControl.Series[0].ShowInLegend = true;
        //                diskspaceWebChartControl.Legend.Visible = true;
        //            }
        //            else
        //            {

        //                diskspaceWebChartControl.Series[c].ShowInLegend = false;
        //            }


        //        }


        //    }
        //}
        //to get data--- sampath 07/07/2014
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
        public void getBlackberry()
        {
            DataTable dt = VSWebBL.DashboardBL.DominoServerDetailsBL.Ins.getBlackberry(servernamelbl.Text);
            if (dt.Rows.Count > 0)
            {
                lbl_Role.Text = dt.Rows[0]["OperatingSystem"].ToString();
                lbl_OverallStatus.Text = dt.Rows[0]["Status"].ToString();
                lbl_PendingMessages.Text = dt.Rows[0]["PendingMail"].ToString();
                lbl_ServerVersion.Text = dt.Rows[0]["BESVersion"].ToString();
                lbl_LicensesUsed.Text = dt.Rows[0]["LicensesUsed"].ToString();
                lbl_SRPConnection.Text = dt.Rows[0]["SRPConnectionn"].ToString();
            }
        }
        public void SetGraphForCPU(string paramGraph, string serverName)
        {
           // cpuWebChartControl.Series.Clear();
            DataTable dt = VSWebBL.DashboardBL.DominoServerDetailsBL.Ins.SetGraphForCPU(paramGraph, serverName);

            Series series = null;
            series = new Series("DominoServer", ViewType.Line);
            series.Visible = true;
            series.ArgumentDataMember = dt.Columns["dtfrom"].ToString();

            ValueDataMemberCollection seriesValueDataMembers = (ValueDataMemberCollection)series.ValueDataMembers;
            seriesValueDataMembers.AddRange(dt.Columns["maxval"].ToString());
            //cpuWebChartControl.Series.Add(series);

           // ((XYDiagram)cpuWebChartControl.Diagram).PaneLayoutDirection = PaneLayoutDirection.Horizontal;

           // XYDiagram seriesXY = (XYDiagram)cpuWebChartControl.Diagram;
            //seriesXY.AxisY.Title.Text = "CPU";
            //seriesXY.AxisY.Title.Visible = true;
            //seriesXY.AxisX.Title.Text = "Date/Time";
            //seriesXY.AxisX.Title.Visible = true;
           // cpuWebChartControl.Legend.Visible = false;

            // ((SplineSeriesView)series.View).LineTensionPercent = 100;
            ((LineSeriesView)series.View).LineMarkerOptions.Size = 4;
            ((LineSeriesView)series.View).LineMarkerOptions.Color = Color.White;

            //AxisBase axis = ((XYDiagram)cpuWebChartControl.Diagram).AxisX;
            //4/18/2014 NS commented out for VSPLUS-312
            //axis.DateTimeGridAlignment = DateTimeMeasurementUnit.Hour;
            //5/13/2014 NS commented out for VSPLUS-621
            //axis.GridSpacingAuto = false;
            //axis.MinorCount = 15;
            //axis.GridSpacing = 0.5;
           // axis.Range.SideMarginsEnabled = false;
            //axis.GridLines.Visible = false;
            //axis.DateTimeOptions.Format = DateTimeFormat.Custom;
            //axis.DateTimeOptions.FormatString = "dd/MM HH:mm";
            ((LineSeriesView)series.View).Color = Color.Blue;

            //AxisBase axisy = ((XYDiagram)cpuWebChartControl.Diagram).AxisY;
            //axisy.Range.AlwaysShowZeroLevel = false;
           // axisy.Range.SideMarginsEnabled = true;
            //axisy.GridLines.Visible = true;
            //cpuWebChartControl.DataSource = dt;
            //cpuWebChartControl.DataBind();

            //return dt;     
        }

        //protected void cpuASPxRadioButtonList_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    SetGraphForCPU(cpuASPxRadioButtonList.Value.ToString(), servernamelbl.Text);
        //}

        //public void SetGraphForMemory(string paramGraph, string serverName)
        //{
        //    memWebChartControl.Series.Clear();
        //    DataTable dt = VSWebBL.DashboardBL.DominoServerDetailsBL.Ins.SetGraphForMemory(paramGraph, serverName);

        //    Series series = null;
        //    series = new Series("DominoServer", ViewType.Line);
        //    series.Visible = true;
        //    series.ArgumentDataMember = dt.Columns["dtfrom"].ToString();

        //    ValueDataMemberCollection seriesValueDataMembers = (ValueDataMemberCollection)series.ValueDataMembers;
        //    seriesValueDataMembers.AddRange(dt.Columns["maxval"].ToString());
        //    memWebChartControl.Series.Add(series);

        //    ((XYDiagram)memWebChartControl.Diagram).PaneLayoutDirection = PaneLayoutDirection.Horizontal;

        //    XYDiagram seriesXY = (XYDiagram)memWebChartControl.Diagram;
        //    seriesXY.AxisY.Title.Text = "Memory";
        //    seriesXY.AxisY.Title.Visible = true;
        //    seriesXY.AxisX.Title.Text = "Date/Time";
        //    seriesXY.AxisX.Title.Visible = true;
        //    memWebChartControl.Legend.Visible = false;

        //    //((SplineSeriesView)series.View).LineTensionPercent = 100;
        //    ((LineSeriesView)series.View).LineMarkerOptions.Size = 4;
        //    ((LineSeriesView)series.View).LineMarkerOptions.Color = Color.White;

        //    AxisBase axis = ((XYDiagram)memWebChartControl.Diagram).AxisX;
        //    //4/18/2014 NS commented out for VSPLUS-312
        //    //axis.DateTimeGridAlignment = DateTimeMeasurementUnit.Hour;
        //    //5/13/2014 NS commented out for VSPLUS-621
        //    //axis.GridSpacingAuto = false;
        //    //axis.MinorCount = 15;
        //    //axis.GridSpacing = 0.5;
        //    axis.Range.SideMarginsEnabled = false;
        //    axis.GridLines.Visible = false;
        //    //axis.DateTimeOptions.Format = DateTimeFormat.Custom;
        //    //axis.DateTimeOptions.FormatString = "dd/MM HH:mm";

        //    ((LineSeriesView)series.View).Color = Color.Blue;

        //    AxisBase axisy = ((XYDiagram)memWebChartControl.Diagram).AxisY;
        //    axisy.Range.AlwaysShowZeroLevel = false;
        //    axisy.Range.SideMarginsEnabled = true;
        //    axisy.GridLines.Visible = true;
        //    memWebChartControl.DataSource = dt;
        //    memWebChartControl.DataBind();
        //}

        //protected void memASPxRadioButtonList_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    SetGraphForMemory(memASPxRadioButtonList.Value.ToString(), servernamelbl.Text);
        //}

        //public void SetGraphForUsers(string paramGraph, string serverName)
        //{
        //    usersWebChartControl.Series.Clear();
        //    DataTable dt = VSWebBL.DashboardBL.DominoServerDetailsBL.Ins.SetGraphForUsers(paramGraph, serverName);

        //    Series series = null;
        //    series = new Series("DominoServer", ViewType.Line);
        //    series.Visible = true;
        //    series.ArgumentDataMember = dt.Columns["dtfrom"].ToString();

        //    ValueDataMemberCollection seriesValueDataMembers = (ValueDataMemberCollection)series.ValueDataMembers;
        //    seriesValueDataMembers.AddRange(dt.Columns["maxval"].ToString());
        //    usersWebChartControl.Series.Add(series);

        //    ((XYDiagram)usersWebChartControl.Diagram).PaneLayoutDirection = PaneLayoutDirection.Horizontal;

        //    XYDiagram seriesXY = (XYDiagram)usersWebChartControl.Diagram;
        //    seriesXY.AxisY.Title.Text = "User Count";
        //    seriesXY.AxisY.Title.Visible = true;
        //    seriesXY.AxisX.Title.Text = "Date/Time";
        //    seriesXY.AxisX.Title.Visible = true;
        //    usersWebChartControl.Legend.Visible = false;

        //    //((SplineSeriesView)series.View).LineTensionPercent = 100;
        //    ((LineSeriesView)series.View).LineMarkerOptions.Size = 4;
        //    ((LineSeriesView)series.View).LineMarkerOptions.Color = Color.White;

        //    AxisBase axis = ((XYDiagram)usersWebChartControl.Diagram).AxisX;
        //    //4/18/2014 NS commented out for VSPLUS-312
        //    //axis.DateTimeGridAlignment = DateTimeMeasurementUnit.Hour;
        //    //axis.MinorCount = 15;
        //    //axis.GridSpacing = 0.5;
        //    axis.Range.SideMarginsEnabled = false;
        //    axis.GridLines.Visible = false;
        //    //axis.DateTimeOptions.Format = DateTimeFormat.Custom;

        //    ((LineSeriesView)series.View).Color = Color.Blue;

        //    //10/9/2013 NS moved the ds assignment prior to setting the grid spacing option
        //    usersWebChartControl.DataSource = dt;
        //    usersWebChartControl.DataBind();

        //    AxisBase axisy = ((XYDiagram)usersWebChartControl.Diagram).AxisY;
        //    axisy.Range.AlwaysShowZeroLevel = false;
        //    axisy.Range.SideMarginsEnabled = true;
        //    axisy.GridLines.Visible = true;

        //    //10/8/2013 NS added
        //    seriesXY.AxisY.NumericOptions.Format = NumericFormat.Number;
        //    seriesXY.AxisY.NumericOptions.Precision = 0;
        //    seriesXY.AxisY.GridSpacingAuto = true;

        //    double min = Convert.ToDouble(((XYDiagram)usersWebChartControl.Diagram).AxisY.Range.MinValue);
        //    double max = Convert.ToDouble(((XYDiagram)usersWebChartControl.Diagram).AxisY.Range.MaxValue);

        //    int gs = (int)((max - min) / 5);

        //    if (gs == 0)
        //    {
        //        gs = 1;
        //        seriesXY.AxisY.GridSpacingAuto = false;
        //        seriesXY.AxisY.GridSpacing = gs;
        //    }

        //}

        //protected void usersASPxRadioButtonList_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //  //  SetGraphForUsers(usersASPxRadioButtonList.Value.ToString(), servernamelbl.Text);
        //}

        //// public void FillCombobox()
        // {
        //    DataTable trafictab = VSWebBL.DashboardBL.DashboardBL.Ins.GetCombobox();
        //   MailComboBox.DataSource = trafictab;
        //   MailComboBox.TextField = "NameandType";
        //   MailComboBox.DataBind();
        // }

        public void SetGraphForMail(string serverName)
        {

           // MailWebChartControl.Series.Clear();
            DataTable dt = VSWebBL.DashboardBL.DashboardBL.Ins.GetMailStatus(serverName);

            Series series = new Series("DeviceType", ViewType.Bar);

            series.ArgumentDataMember = dt.Columns["Mail"].ToString();

            ValueDataMemberCollection seriesValueDataMembers = (ValueDataMemberCollection)series.ValueDataMembers;
            seriesValueDataMembers.AddRange(dt.Columns["Value"].ToString());
           // MailWebChartControl.Series.Add(series);

           // MailWebChartControl.Legend.Visible = false;

            ((SideBySideBarSeriesView)series.View).ColorEach = true;

           // AxisBase axisx = ((XYDiagram)MailWebChartControl.Diagram).AxisX;
           // axisx.GridLines.Visible = false;
           // XYDiagram seriesXY = (XYDiagram)MailWebChartControl.Diagram;
           // seriesXY.AxisX.Title.Text = "Mail Status";
           // seriesXY.AxisX.Title.Visible = true;
           // seriesXY.AxisX.Label.ResolveOverlappingOptions.AllowRotate = false;
            //seriesXY.AxisX.Label.ResolveOverlappingOptions.AllowStagger = true;
            //seriesXY.AxisY.Title.Text = "Count";
            //seriesXY.AxisY.Title.Visible = true;
            //10/8/2013 NS added
            //seriesXY.AxisY.NumericOptions.Format = NumericFormat.Number;
            //seriesXY.AxisY.NumericOptions.Precision = 0;
            //seriesXY.AxisY.Label.ResolveOverlappingOptions.AllowRotate = false;

            //AxisBase axisy = ((XYDiagram)MailWebChartControl.Diagram).AxisY;
            //axisy.Range.AlwaysShowZeroLevel = false;
            //axisy.GridLines.Visible = false;

            //MailWebChartControl.DataSource = dt;
            //MailWebChartControl.DataBind();

            //double min = Convert.ToDouble(((XYDiagram)MailWebChartControl.Diagram).AxisY.Range.MinValue);
            //double max = Convert.ToDouble(((XYDiagram)MailWebChartControl.Diagram).AxisY.Range.MaxValue);

            //int gs = (int)((max - min) / 5);

            //if (gs == 0)
            //{
            //    gs = 1;
            //    seriesXY.AxisY.GridSpacingAuto = false;
            //    seriesXY.AxisY.GridSpacing = gs;
            //}
            //else
            //{
            //    seriesXY.AxisY.GridSpacingAuto = true;
            //}


        }

        //protected void mailASPxRadioButtonList_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    SetGraphForMail(mailASPxRadioButtonList.Value.ToString(), servernamelbl.Text);
        //}


        //DataBase Code


        public void FillGrid()
        {
            DataTable Statustab = new DataTable();
            try
            {
                Statustab = VSWebBL.DashboardBL.DatabaseHealthBL.Ins.GetData(servernamelbl.Text);

                if (Statustab.Rows.Count > 0)
                {

                }
                Session["Statustab"] = Statustab;
                //MonitoredDBGridView.DataSource = Statustab;
                //MonitoredDBGridView.DataBind();


            }
            catch (Exception)
            {

                throw;
            }

        }

        public void FillGridfromSession()
        {
            DataTable dt = new DataTable();
            if (Session["Statustab"] != "" && Session["Statustab"] != null)
                dt = Session["Statustab"] as DataTable;
            if (dt.Rows.Count > 0)
            {
                //MonitoredDBGridView.DataSource = dt;
                //MonitoredDBGridView.DataBind();
            }
        }

        //protected void MonitoredDBGridView_HtmlDataCellPrepared(object sender, DevExpress.Web.ASPxGridViewTableDataCellEventArgs e)
        //{
        //    if (e.DataColumn.FieldName == "Status" && (e.CellValue.ToString() == "OK" || e.CellValue.ToString() == "Scanning"))
        //    {
        //        e.Cell.BackColor = System.Drawing.Color.LightGreen;
        //    }

        //    else if (e.DataColumn.FieldName == "Status" && e.CellValue.ToString() == "Not Responding")
        //    {
        //        e.Cell.BackColor = System.Drawing.Color.Red;
        //        e.Cell.ForeColor = System.Drawing.Color.White;
        //    }
        //    else if (e.DataColumn.FieldName == "Status" && e.CellValue.ToString() == "Not Scanned")
        //    {
        //        e.Cell.BackColor = System.Drawing.Color.Blue;
        //        e.Cell.ForeColor = System.Drawing.Color.White;
        //    }
        //    else if (e.DataColumn.FieldName == "Status" && e.CellValue.ToString() == "disabled")
        //    {
        //        e.Cell.BackColor = System.Drawing.Color.Gray;
        //        // e.Cell.ForeColor = System.Drawing.Color.White;
        //    }
        //    else if (e.DataColumn.FieldName == "Status")
        //    {
        //        e.Cell.BackColor = System.Drawing.Color.Yellow;
        //        // e.DataColumn.GroupFooterCellStyle.ForeColor = System.Drawing.Color.Yellow;
        //    }

        //}

        //protected void MonitoredDBGridView_SelectionChanged(object sender, EventArgs e)
        //{

        //    if (MonitoredDBGridView.Selection.Count > 0)
        //    {
        //        System.Collections.Generic.List<object> Type = MonitoredDBGridView.GetSelectedFieldValues("Name");
        //        System.Collections.Generic.List<object> c = MonitoredDBGridView.GetSelectedFieldValues("Category");
        //        if (Type.Count > 0 && c.Count > 0)
        //        {

        //            string Name = Type[0].ToString();
        //            string RT = c[0].ToString();
        //            if (RT == "Database Response Time")
        //                DevExpress.Web.ASPxWebControl.RedirectOnCallback("Performance.aspx?Name=" + Name + "");

        //        }


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

        //public void FillALLGrid()
        //{
        //    DataTable Dailytab = new DataTable();
        //    try
        //    {
        //        Dailytab = VSWebBL.DashboardBL.DatabaseHealthBL.Ins.GetAllData(servernamelbl.Text);

        //        if (Dailytab.Rows.Count > 0)
        //        {

        //        }
        //        Session["Dailytab"] = Dailytab;
        //        AllDBGridView.DataSource = Dailytab;
        //        AllDBGridView.DataBind();
        //    }
        //    catch (Exception)
        //    {

        //        throw;
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
        //protected void DominoserverTasksgrid_HtmlDataCellPrepared(object sender, DevExpress.Web.ASPxGridViewTableDataCellEventArgs e)
        //{
        //    if (e.DataColumn.FieldName == "StatusSummary" && (e.CellValue.ToString() == "OK" || e.CellValue.ToString() == "Scanning"))
        //    {
        //        e.Cell.BackColor = System.Drawing.Color.LightGreen;
        //    }

        //    else if (e.DataColumn.FieldName == "StatusSummary" && e.CellValue.ToString() == "Not Responding")
        //    {
        //        e.Cell.BackColor = System.Drawing.Color.Red;
        //        e.Cell.ForeColor = System.Drawing.Color.White;
        //    }
        //    else if (e.DataColumn.FieldName == "StatusSummary" && e.CellValue.ToString() == "Not Scanned")
        //    {
        //        e.Cell.BackColor = System.Drawing.Color.Blue;
        //        e.Cell.ForeColor = System.Drawing.Color.White;
        //    }
        //    else if (e.DataColumn.FieldName == "StatusSummary" && e.CellValue.ToString() == "disabled")
        //    {
        //        e.Cell.BackColor = System.Drawing.Color.Gray;
        //        // e.Cell.ForeColor = System.Drawing.Color.White;
        //    }
        //    else if (e.DataColumn.FieldName == "StatusSummary")
        //    {
        //        e.Cell.BackColor = System.Drawing.Color.Yellow;
        //        // e.DataColumn.GroupFooterCellStyle.ForeColor = System.Drawing.Color.Yellow;
        //    }
        //}

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

        //public void FillMonitoredGrid()
        //{

        //    try
        //    {
        //        DataTable StatusTable = VSWebBL.DashboardBL.DashboardBL.Ins.GetMoniteredServerTasks(servernamelbl.Text);

        //        if (StatusTable.Rows.Count <= 0)
        //        {
        //            MonitoredDBGridView.Visible = false;
        //            // MonitorDBRoundPanel1.Visible = false;
        //            Label5.Visible = false;
        //        }
        //        Session["MoniterdeServerTasks"] = StatusTable;
        //        DominoserverTasksgrid.DataSource = StatusTable;
        //        DominoserverTasksgrid.DataBind();


        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //    finally
        //    {
        //    }
        //}

        //public void FillgridfromSession()
        //{
        //    if (Session["MoniterdeServerTasks"] != "" && Session["MoniterdeServerTasks"] != null)
        //    {
        //        DataTable StatusTable = Session["MoniterdeServerTasks"] as DataTable;
        //        try
        //        {
        //            DominoserverTasksgrid.DataSource = StatusTable;
        //            DominoserverTasksgrid.DataBind();

        //        }
        //        catch (Exception)
        //        {

        //            throw;
        //        }
        //        finally
        //        {
        //        }
        //    }
        //    else
        //    {
        //        MonitoredDBGridView.Visible = false;
        //        //  MonitorDBRoundPanel1.Visible = false;
        //        Label5.Visible = false;
        //    }
        //}

        public void FillMonMoniteredGrid()
        {

            try
            {
                DataTable StatusTable1 = VSWebBL.DashboardBL.DashboardBL.Ins.GetNonMoniteredServerTasks(servernamelbl.Text);

                Session["NonMoniterdeServerTasks"] = StatusTable1;
                //NonmoniterdGrid.DataSource = StatusTable1;
                //NonmoniterdGrid.DataBind();


            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
            }
        }

        public void FillNonMongridfromSession()
        {
            if (Session["NonMoniterdeServerTasks"] != "" && Session["NonMoniterdeServerTasks"] != null)
            {
                DataTable StatusTable1 = Session["NonMoniterdeServerTasks"] as DataTable;
                try
                {
                    //NonmoniterdGrid.DataSource = StatusTable1;
                    //NonmoniterdGrid.DataBind();

                }
                catch (Exception)
                {

                    throw;
                }
                finally
                {
                }
            }
        }

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

        //protected void NonmoniterdGrid_HtmlDataCellPrepared(object sender, DevExpress.Web.ASPxGridViewTableDataCellEventArgs e)
        //{
        //    if (e.DataColumn.FieldName == "StatusSummary" && (e.CellValue.ToString() == "OK" || e.CellValue.ToString() == "Scanning"))
        //    {
        //        e.Cell.BackColor = System.Drawing.Color.LightGreen;
        //    }

        //    else if (e.DataColumn.FieldName == "StatusSummary" && e.CellValue.ToString() == "Not Responding")
        //    {
        //        e.Cell.BackColor = System.Drawing.Color.Red;
        //        e.Cell.ForeColor = System.Drawing.Color.White;
        //    }
        //    else if (e.DataColumn.FieldName == "StatusSummary" && e.CellValue.ToString() == "Not Scanned")
        //    {
        //        e.Cell.BackColor = System.Drawing.Color.Blue;
        //        e.Cell.ForeColor = System.Drawing.Color.White;
        //    }
        //    else if (e.DataColumn.FieldName == "StatusSummary" && e.CellValue.ToString() == "disabled")
        //    {
        //        e.Cell.BackColor = System.Drawing.Color.Gray;
        //        // e.Cell.ForeColor = System.Drawing.Color.White;
        //    }
        //    else if (e.DataColumn.FieldName == "StatusSummary")
        //    {
        //        e.Cell.BackColor = System.Drawing.Color.Yellow;
        //    }   // e.DataColumn.GroupFooterCellStyle.ForeColor = System.Drawing.Color.Yellow;
        //}

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
            catch (Exception)
            {

                throw;
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
            catch (Exception)
            {

                throw;
            }


        }

        //protected void AllDBGridView_HtmlDataCellPrepared(object sender, DevExpress.Web.ASPxGridViewTableDataCellEventArgs e)
        //{
        //    if (e.DataColumn.FieldName == "Status" && (e.CellValue.ToString() == "OK" || e.CellValue.ToString() == "Scanning"))
        //    {
        //        e.Cell.BackColor = System.Drawing.Color.LightGreen;
        //    }

        //    else if (e.DataColumn.FieldName == "Status" && e.CellValue.ToString() == "Not Responding")
        //    {
        //        e.Cell.BackColor = System.Drawing.Color.Red;
        //        e.Cell.ForeColor = System.Drawing.Color.White;
        //    }
        //    else if (e.DataColumn.FieldName == "Status" && e.CellValue.ToString() == "Not Scanned")
        //    {
        //        e.Cell.BackColor = System.Drawing.Color.Blue;
        //        e.Cell.ForeColor = System.Drawing.Color.White;
        //    }
        //    else if (e.DataColumn.FieldName == "Status" && e.CellValue.ToString() == "disabled")
        //    {
        //        e.Cell.BackColor = System.Drawing.Color.Gray;
        //        // e.Cell.ForeColor = System.Drawing.Color.White;
        //    }
        //    else if (e.DataColumn.FieldName == "Status")
        //    {
        //        e.Cell.BackColor = System.Drawing.Color.Yellow;
        //        // e.DataColumn.GroupFooterCellStyle.ForeColor = System.Drawing.Color.Yellow;
        //    }

        //}

		//protected void BackButton_Click(object sender, EventArgs e)
		//{
		//    if (Session["BackURL"] != "" && Session["BackURL"] != null)
		//    {
		//        Response.Redirect(Session["BackURL"].ToString());
		//        Session["BackURL"] = "";

		//    }
		//}

        //protected void OpenButton1_Click(object sender, EventArgs e)
        //{
        //    if (AllDBGridView.FocusedRowIndex > -1)
        //    {
        //        DataTable dt = new DataTable();
        //        int index;
        //        if (AllDBGridView.FocusedRowIndex > -1)
        //        {
        //            index = AllDBGridView.FocusedRowIndex;
        //        }
        //        else
        //        {
        //            index = 0;
        //        }
        //        string HostName = "Nohost";
        //        string Server = AllDBGridView.GetRowValues(index, "Server").ToString();
        //        DataTable servertab = VSWebBL.DashboardBL.DatabaseHealthBL.Ins.GetIPfromServers(Server);
        //        if (servertab.Rows.Count > 0)
        //            HostName = servertab.Rows[0]["IPAddress"].ToString();
        //        string filepath = AllDBGridView.GetRowValues(index, "Folder").ToString();
        //        string fileName = AllDBGridView.GetRowValues(index, "FileName").ToString();
        //        filepath = filepath.Replace("'\'", "'/'");
        //        if (filepath == "")
        //        {
        //            Page.ClientScript.RegisterStartupScript(this.GetType(), "Script", "PopupCenter('http://" + HostName + "/" + fileName + "?OpenDatabase');", true);
        //        }
        //        else
        //        {
        //            Page.ClientScript.RegisterStartupScript(this.GetType(), "Script", "PopupCenter('http://" + HostName + "/" + filepath + "/" + fileName + "?OpenDatabase');", true);
        //        }




        //        //if (filepath == "")
        //        //{
        //        //    Response.Write("<script>window.open('http://" + HostName + "/" + fileName + "?OpenDatabase','Open Database','width=500px height=100px',1)</script>");
        //        //}
        //        //else
        //        //{
        //        //    Response.Write("<script>window.open('http://" + HostName + "/" + filepath + "/" + fileName + "?OpenDatabase','Open Database','width=500px height=100px',1)</script>");
        //        //}
        //    }
        //    else
        //    {
        //        Dominomsglbl.Text = "Please select  Database in All Databases";
        //        DominomsgPopupControl.ShowOnPageLoad = true;
        //    }
        //}

        //protected void CompactButton_Click(object sender, EventArgs e)
        //{
        //    popuptextBox.Text = "";
        //    DBPopupControl.HeaderText = "Compact Options";
        //    ASPxLabel5.Text = "Please enter any desired compact options.";
        //    ASPxLabel6.Text = "Use -c for corrupt databases and -B for in place compaction with file size reduction.";
        //    ASPxLabel8.Text = "Google 'IBM domino compact switches' for a full list of options.";
        //    ASPxLabel7.Visible = false;
        //    DBPopupControl.ShowOnPageLoad = true;
        //    Session["ActBtn"] = "CompactButton";
        //}

        //protected void FixupButton_Click(object sender, EventArgs e)
        //{
        //    popuptextBox.Text = "";
        //    DBPopupControl.HeaderText = "FixedUp Options";
        //    ASPxLabel5.Text = "Please enter any desired FIXUP options.";
        //    ASPxLabel6.Text = "For example, use -Q to check more quickly but less thoroughly.";
        //    ASPxLabel8.Text = "Google 'IBM Fixup compact switches' for a full list of options.";
        //    ASPxLabel7.Visible = true;
        //    ASPxLabel7.Text = "Use -V to prevents Fixup from running on views. This option reduces the time it takes Fixup to run.";
        //    DBPopupControl.ShowOnPageLoad = true;
        //    Session["ActBtn"] = "Fixup";
        //}

        //protected void UpdallButton_Click(object sender, EventArgs e)
        //{
        //    popuptextBox.Text = "";
        //    DBPopupControl.HeaderText = "Updall Options";
        //    ASPxLabel5.Text = "Please enter any desired UPDALL options.";
        //    ASPxLabel6.Text = "For example, use -R to rebuild all used views (resource intensive).";
        //    ASPxLabel8.Text = "Use -X to rebuild the full text index.";
        //    ASPxLabel7.Visible = false;
        //    DBPopupControl.ShowOnPageLoad = true;
        //    Session["ActBtn"] = "Updall";
        //}

        //protected void DominoOKButton_Click(object sender, EventArgs e)
        //{
        //    if (Session["ActBtn"] != "" && Session["ActBtn"] != null)
        //    {
        //        if (AllDBGridView.FocusedRowIndex > -1)
        //        {
        //            if (Session["ActBtn"] == "CompactButton")
        //            {

        //                //            NotesSession  session =new NotesSession();
        //                //// NotesUIWorkspace workspace = New NotesUIWorkspace();
        //                //    // NotesUIDocument  uidoc= new NotesUIDocument();
        //                ////Set uidoc = workspace.CurrentDocument
        //                //        NotesDocument doc =new NotesDocument();
        //                //    //doc = uidoc.Document
        //                //// Variant StatValue;
        //                //    string dbPath;
        //                //if(doc.FolderReferences.Folder(0) != "" )
        //                //{

        //                //    dbPath = doc.FolderReferences.Folder(0) &doc.FolderReferences.Filename(0);
        //                //}

        //                //else
        //                //                {
        //                //    dbPath = doc.FolderReferences.Filename(0);
        //                //}
        //                ////serverName$ =doc.Server(0);
        //                //   
        //                //// Config$ ="lo compact "+ dbPath + " " + Options;
        //                ////consoleReturn$ = session.SendConsoleCommand(serverName$, Config$)
        //                ////Print  "Sent the command " &Config$  & ".  Note that this will only work if YOU have the appropriate remote console and admin rights."
        //                string Server = "";
        //                string Config = "";
        //                string Folder = "";
        //                string FileName = "";
        //                DataRow myRow = AllDBGridView.GetDataRow(AllDBGridView.FocusedRowIndex);
        //                Server = myRow["Server"].ToString();
        //                Folder = myRow["Folder"].ToString();
        //                FileName = myRow["Filename"].ToString();
        //                string Options;
        //                Options = popuptextBox.Text;
        //                Config = "lo compact " + Folder + FileName + "" + Options;
        //                bool returnval = VSWebBL.DashboardBL.LotusTravelerHealthBLL.Ins.SENDTravelerConsoleCommand(Server, Config, Session["UserFullName"].ToString());
        //                if (returnval == true)
        //                {
        //                    Dominomsglbl.Text = "Sent the command " + Config + ". Note that this will only work if YOU have the appropriate remote console and admin rights.";
        //                    DominomsgPopupControl.ShowOnPageLoad = true;
        //                }

        //            }

        //            if (Session["ActBtn"] == "Fixup")
        //            {
        //                string Server = "";
        //                string Config = "";
        //                string Folder = "";
        //                string FileName = "";
        //                DataRow myRow = AllDBGridView.GetDataRow(AllDBGridView.FocusedRowIndex);
        //                Server = myRow["Server"].ToString();
        //                Folder = myRow["Folder"].ToString();
        //                FileName = myRow["Filename"].ToString();
        //                string Options;
        //                Options = popuptextBox.Text;
        //                Config = "Load fixupdbpath options " + Folder + FileName + "" + Options;
        //                bool returnval = VSWebBL.DashboardBL.LotusTravelerHealthBLL.Ins.SENDTravelerConsoleCommand(Server, Config, Session["UserFullName"].ToString());
        //                if (returnval == true)
        //                {
        //                    Dominomsglbl.Text = "Sent the command " + Config + ". Note that this will only work if YOU have the appropriate remote console and admin rights.";
        //                    DominomsgPopupControl.ShowOnPageLoad = true;
        //                }
        //                // Config ="Load fixupdbpath options "+ dbPath + " " + Options;


        //            }
        //            if (Session["ActBtn"] == "Updall")
        //            {
        //                string Server = "";
        //                string Config = "";
        //                string Folder = "";
        //                string FileName = "";
        //                DataRow myRow = AllDBGridView.GetDataRow(AllDBGridView.FocusedRowIndex);
        //                Server = myRow["Server"].ToString();
        //                Folder = myRow["Folder"].ToString();
        //                FileName = myRow["Filename"].ToString();
        //                string Options;
        //                Options = popuptextBox.Text;
        //                Config = "Load updalldbpath options " + Folder + FileName + "" + Options;
        //                bool returnval = VSWebBL.DashboardBL.LotusTravelerHealthBLL.Ins.SENDTravelerConsoleCommand(Server, Config, Session["UserFullName"].ToString());
        //                if (returnval == true)
        //                {
        //                    Dominomsglbl.Text = "Sent the command " + Config + ". Note that this will only work if YOU have the appropriate remote console and admin rights.";
        //                    DominomsgPopupControl.ShowOnPageLoad = true;
        //                }


        //                // Config$ ="Load updalldbpath options"+ dbPath + " " + Options;

        //            }
        //            DBPopupControl.ShowOnPageLoad = false;

        //        }

        //        else
        //        {
        //            Dominomsglbl.Text = "Please Select Server";
        //            DominomsgPopupControl.ShowOnPageLoad = true;
        //            DBPopupControl.ShowOnPageLoad = false;
        //        }
        //    }
        //}

        //protected void CancelButton_Click(object sender, EventArgs e)
        //{
        //    DBPopupControl.ShowOnPageLoad = false;
        //}

        //protected void msgbtn_Click(object sender, EventArgs e)
        //{
        //    DominomsgPopupControl.ShowOnPageLoad = false;
        //}

        protected void OpenDBButton_Click(object sender, EventArgs e)
        {

        }

        protected void ASPxPageControl1_TabClick(object source, DevExpress.Web.TabControlCancelEventArgs e)
        {
            if (e.Tab.Text == "Web Admin")
            {
                //12/4/2013 NS modified
                SetupWebAdmin();
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
                    //SetGraph("hh", servernamelbl.Text);
                    //SetGraphForDiskSpace(servernamelbl.Text);
                    //SetGraphForCPU("hh", servernamelbl.Text);
                    //SetGraphForMemory("hh", servernamelbl.Text);
                    //SetGraphForUsers("hh", servernamelbl.Text);
                    //SetGraphForMail(servernamelbl.Text);
                }
                else
                {
                    if (e.Tab.Text == "Databases")
                    {
                        //FillgridfromSession();
                        //FillALLGridfromSession();
                    }
                    else
                    {
                        if (e.Tab.Text == "Server Tasks")
                        {
                            //FillgridfromSession();
                            FillNonMongridfromSession();
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
                                    if (e.Tab.Text == "Outages")
                                    {
                                        FilloutagefromSession();
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public void SetupWebAdmin()
        {
            string serverip = "";
            string servername = Request.QueryString["Name"];
            string dbpath = "webadmin.nsf";
            DataTable dt = new DataTable();
            dt = VSWebBL.ConfiguratorBL.AlertsBL.Ins.GetServerIP(servername);
            if (dt.Rows.Count > 0)
            {
                serverip = dt.Rows[0]["IPAddress"].ToString();
            }
            //3/21/2014 NS modified - hide WebAdmin tab for non-Domino pages - the code below doesn't work because it sits inside the UpdatePanel
            //if (Request.QueryString["Type"] != "Domino")
            //{
            //    ASPxPageControl1.TabPages[6].Visible = false;
            //}
            if (serverip != "")
            {
                url = "http://" + serverip + "/" + dbpath + "?OpenDatabase";
                //2/12/2014 NS modified
                //fraHtml.Attributes.Add("src", url);
                WebAdminButton.Text = "Click here to open the Web Admin page for " + servernamelbl.Text;
                webAdminLabel.Text = url;
            }
            else
            {
                WebAdminButton.Text = "No IP address found - you will not be able to open WebAdmin";
                webAdminLabel.Text = "";
            }
        }

        public void UpdateButtonVisibility()
        {
            bool isadmin = false;
            DataTable sa = VSWebBL.SecurityBL.UsersBL.Ins.GetIsConsoleComm(Session["UserID"].ToString());
            if (sa.Rows.Count > 0)
            {
                if (sa.Rows[0]["IsConsoleComm"].ToString() == "True")
                {
                    isadmin = true;
                }
            }
            if (isadmin)
            {
                //CompactButton.Visible = true;
                //FixupButton.Visible = true;
                //UpdallButton.Visible = true;
            }
            //1/2/2014 NS added
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
        

        //protected void ServerTasksPopupMenu_ItemClick(object source, DevExpress.Web.MenuItemEventArgs e)
        //{
        //    if (e.Item.Name == "ServerTaskStart")
        //    {
        //        if (DominoserverTasksgrid.FocusedRowIndex > -1)
        //        {
        //            YesButton.Visible = true;
        //            NOButton.Visible = true;
        //            CancelButton.Visible = false;
        //            // CancelButton.Text = "Cancel";
        //            Session["MenuItem"] = "ServerTaskStart";
        //            ServerTaskStart();
        //        }
        //        else
        //        {
        //            msgPopupControl1.HeaderText = "Server Tasks Start";
        //            msgPopupControl1.ShowOnPageLoad = true;
        //            msgLabel.Text = "Please select a task in the grid.";
        //            YesButton.Visible = false;
        //            NOButton.Visible = false;
        //            CancelButton.Visible = true;
        //            CancelButton.Text = "OK";
        //        }
        //    }
        //    if (e.Item.Name == "ServerTaskStop")
        //    {
        //        if (DominoserverTasksgrid.FocusedRowIndex > -1)
        //        {
        //            YesButton.Visible = true;
        //            NOButton.Visible = true;
        //            CancelButton.Visible = false;
        //            // CancelButton.Text = "Cancel";
        //            Session["MenuItem"] = "ServerTaskStop";
        //            ServerTaskStop();
        //        }
        //        else
        //        {
        //            msgPopupControl1.HeaderText = "Server Task Stop";
        //            msgPopupControl1.ShowOnPageLoad = true;
        //            msgLabel.Text = "Please select a task in the grid.";
        //            YesButton.Visible = false;
        //            NOButton.Visible = false;
        //            CancelButton.Visible = true;
        //            CancelButton.Text = "OK";
        //        }
        //    }
        //    if (e.Item.Name == "ServerTaskRestart")
        //    {
        //        if (DominoserverTasksgrid.FocusedRowIndex > -1)
        //        {
        //            YesButton.Visible = true;
        //            NOButton.Visible = true;
        //            CancelButton.Visible = false;
        //            // CancelButton.Text = "Cancel";
        //            Session["MenuItem"] = "ServerTaskRestart";
        //            ServerTaskRestart();
        //        }
        //        else
        //        {
        //            msgPopupControl1.HeaderText = "Server Task Restart";
        //            msgPopupControl1.ShowOnPageLoad = true;
        //            msgLabel.Text = "Please select a task in the grid.";
        //            YesButton.Visible = false;
        //            NOButton.Visible = false;
        //            CancelButton.Visible = true;
        //            CancelButton.Text = "OK";
        //        }
        //    }
        //}

        private void ServerTaskRestart()
        {
            string TellCommand = "";
            string myUserName = "";
            string myDeviceName = "";
            string myServerName = "";

          //  DataRow myRow = DominoserverTasksgrid.GetDataRow(DominoserverTasksgrid.FocusedRowIndex);
            try
            {

               // Session["TaskName"] = myRow["TaskName"].ToString();
                Session["MenuItem"] = "ServerTaskRestart";

                myUserName = Session["UserLogin"].ToString();
                Session["myUserName"] = myUserName;
                myServerName = servernamelbl.Text;
                Session["myServerName"] = myServerName;

                VSWebDO.DominoServerTasks DSTasksObject = new VSWebDO.DominoServerTasks();
            //    DSTasksObject.TaskName = myRow["TaskName"].ToString();
                VSWebDO.DominoServerTasks ReturnValue = VSWebBL.ConfiguratorBL.DominoServerTasksBL.Ins.GetDataForTaskName(DSTasksObject);
                TellCommand = ReturnValue.LoadString;

                TellCommand = "tell " + TellCommand.ToString().Substring(3) + " restart";
                Session["TellCommand"] = TellCommand;




                VSWebBL.DashboardBL.LotusTravelerHealthBLL.Ins.SENDTravelerConsoleCommand(myServerName, TellCommand, myUserName);

                //msgPopupControl1.HeaderText = "Server Tasks Restart";
                //msgPopupControl1.ShowOnPageLoad = true;
                //msgLabel.Text = "The request to restart the task '" + myRow["TaskName"].ToString() + "' is sent.";
                //YesButton.Visible = false;
                //NOButton.Visible = false;
                //CancelButton.Visible = true;
                //CancelButton.Text = "OK";
            }
            catch (Exception)
            {
                myUserName = "";
            }
            //msgPopupControl1.ShowOnPageLoad = true;
            //msgLabel.Text = "Are you sure you want to Restart " + myRow["TaskName"].ToString() + " on server " + myServerName + "?";                      

        }

        private void ServerTaskStop()
        {
            string TellCommand = "";
            string myUserName = "";
            string myDeviceName = "";
            string myServerName = "";

          //  DataRow myRow = DominoserverTasksgrid.GetDataRow(DominoserverTasksgrid.FocusedRowIndex);
            try
            {
              //  Session["TaskName"] = myRow["TaskName"].ToString();
                Session["MenuItem"] = "ServerTaskStop";

                myUserName = Session["UserLogin"].ToString();
                Session["myUserName"] = myUserName;
                myServerName = servernamelbl.Text;
                Session["myServerName"] = myServerName;

                VSWebDO.DominoServerTasks DSTasksObject = new VSWebDO.DominoServerTasks();
            //    DSTasksObject.TaskName = myRow["TaskName"].ToString();
                VSWebDO.DominoServerTasks ReturnValue = VSWebBL.ConfiguratorBL.DominoServerTasksBL.Ins.GetDataForTaskName(DSTasksObject);
                TellCommand = ReturnValue.LoadString;

                TellCommand = "tell " + TellCommand.Substring(3) + " exit";
                Session["TellCommand"] = TellCommand;



                VSWebBL.DashboardBL.LotusTravelerHealthBLL.Ins.SENDTravelerConsoleCommand(myServerName, TellCommand, myUserName);

                //msgPopupControl1.HeaderText = "Server Tasks Stop";
                //msgPopupControl1.ShowOnPageLoad = true;
                //msgLabel.Text = "The request to stop the task '" + myRow["TaskName"].ToString() + "' is sent.";
                //YesButton.Visible = false;
                //NOButton.Visible = false;
                //CancelButton.Visible = true;
                //CancelButton.Text = "OK";
            }
            catch (Exception)
            {
                myUserName = "";
            }
            //msgPopupControl1.ShowOnPageLoad = true;
            //msgLabel.Text = "Are you sure you want to Stop " + myRow["TaskName"].ToString() + " on server " + myServerName + "?";                      

        }

        private void ServerTaskStart()
        {
            string TellCommand = "";
            string myUserName = "";
            string myDeviceName = "";
            string myServerName = "";

           // DataRow myRow = DominoserverTasksgrid.GetDataRow(DominoserverTasksgrid.FocusedRowIndex);
            try
            {

                myUserName = Session["UserLogin"].ToString();
                myServerName = servernamelbl.Text;

                VSWebDO.DominoServerTasks DSTasksObject = new VSWebDO.DominoServerTasks();
              //  DSTasksObject.TaskName = myRow["TaskName"].ToString();
                VSWebDO.DominoServerTasks ReturnValue = VSWebBL.ConfiguratorBL.DominoServerTasksBL.Ins.GetDataForTaskName(DSTasksObject);
                TellCommand = ReturnValue.LoadString;

                VSWebBL.DashboardBL.LotusTravelerHealthBLL.Ins.SENDTravelerConsoleCommand(myServerName, TellCommand, myUserName);

                //msgPopupControl1.HeaderText = "Server Tasks Start";
                //msgPopupControl1.ShowOnPageLoad = true;
                //msgLabel.Text = "The request to start the task '" + myRow["TaskName"].ToString() + "' is sent.";
                //YesButton.Visible = false;
                //NOButton.Visible = false;
                //CancelButton.Visible = true;
                //CancelButton.Text = "OK";
            }
            catch (Exception)
            {
                myUserName = "";
            }

        }


        //protected void DominoserverTasksgrid_HtmlRowCreated(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
        //{
        //    int index = DominoserverTasksgrid.FocusedRowIndex;
        //    if (e.VisibleIndex != index)
        //    {

        //        e.Row.Attributes.Add("onmouseover", "this.originalcolor=this.style.backgroundColor;" + "this.style.backgroundColor='#C0C0C0';"); //'#C0C0C0'


        //        e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalcolor;");
        //    }
        //}

        //protected void YesButton_Click(object sender, EventArgs e)
        //{

        //    if (Session["MenuItem"] == "ServerTaskRestart" || Session["MenuItem"] == "ServerTaskExit")
        //    {
        //        if (Session["TellCommand"] != "" && Session["myServerName"] != "" && Session["myUserName"] != "")
        //        {
        //            msgLabel.Text = Session["TellCommand"].ToString() + "," + Session["myServerName"].ToString();
        //            VSWebBL.DashboardBL.LotusTravelerHealthBLL.Ins.SENDTravelerConsoleCommand(Session["myServerName"].ToString(), Session["TellCommand"].ToString(), Session["myUserName"].ToString());

        //        } YesButton.Visible = false;
        //        NOButton.Visible = false;
        //        CancelButton.Visible = true;
        //        CancelButton.Text = "OK";
        //        Session["myUserName"] = "";
        //        Session["myServerName"] = "";
        //        Session["myDeviceName"] = "";
        //        Session["TellCommand"] = "";


        //    }
        //}
        //protected void CancelButton1_Click(object sender, EventArgs e)
        //{
        //    msgPopupControl1.ShowOnPageLoad = false;
        //}

        //protected void NOButton_Click(object sender, EventArgs e)
        //{
        //    if (Session["myUserName"] != "" && Session["myUserName"] != null)
        //        msgLabel.Text = "No changes made to the task " + Session["TaskName"].ToString() + ".";
        //    YesButton.Visible = false;
        //    NOButton.Visible = false;
        //    CancelButton.Visible = true;
        //    CancelButton.Text = "OK";
        //    Session["myUserName"] = "";
        //    Session["myServerName"] = "";
        //    Session["myDeviceName"] = "";
        //    Session["TellCommand"] = "";
        //    Session["TaskName"] = "";
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
		//    }
		//    catch (Exception)
		//    {

		//        throw;
		//    }
		//}

		//protected void EditInConfigButton_Click(object sender, EventArgs e)
		//{
		//    //1/2/2014 NS added
		//    string id = "";
		//    Session["Submenu"] = "BlackBerry";
		//    if (Request.QueryString["Name"] != "" && Request.QueryString["Name"] != null)
		//    {
		//        id = (VSWebBL.SecurityBL.ServersBL.Ins.GetServerIDbyServerName(Request.QueryString["Name"].ToString())).ToString();
		//        Response.Redirect("~/Configurator/BlackBerryEntertpriseServer.aspx?Key=" + id, false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
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
                string Durationtype = "";
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
            catch (Exception)
            {
                throw;
            }
        }

        //protected void MonitoredDBGridView_PageSizeChanged(object sender, EventArgs e)
        //{
        //    //ProfilesGridView.PageIndex;
        //    VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("DominoServerDetailsPage2|MonitoredDBGridView", MonitoredDBGridView.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
        //    Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        //}
        //protected void AllDBGridView_PageSizeChanged(object sender, EventArgs e)
        //{
        //    //ProfilesGridView.PageIndex;
        //    VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("DominoServerDetailsPage2|AllDBGridView", AllDBGridView.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
        //    Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        //}
        //protected void DominoserverTasksgrid_PageSizeChanged(object sender, EventArgs e)
        //{
        //    //ProfilesGridView.PageIndex;
        //    VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("DominoServerDetailsPage2|DominoserverTasksgrid", DominoserverTasksgrid.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
        //    Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        //}
        //protected void NonmoniterdGrid_PageSizeChanged(object sender, EventArgs e)
        //{
        //    //ProfilesGridView.PageIndex;
        //    VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("DominoServerDetailsPage2|NonmoniterdGrid", NonmoniterdGrid.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
        //    Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        //}
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
				Session["Submenu"] = "LotusDomino";
				if (Request.QueryString["Name"] != "" && Request.QueryString["Name"] != null)
				{
					id = (VSWebBL.SecurityBL.ServersBL.Ins.GetServerIDbyServerName(Request.QueryString["Name"].ToString())).ToString();
					Response.Redirect("~/Configurator/BlackBerryEntertpriseServer.aspx?Key=" + id, false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
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
  




   
