using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DevExpress.XtraCharts;
using System.Drawing;
//using VSWebBL;

namespace VSWebUI.Configurator
{
    public partial class ExchangeServerDetailsPage2 : System.Web.UI.Page
    {

		int ServerTypeId = 5;

		protected void Page_PreInit(object sender, EventArgs e)
		{

			this.Master.contentCallEvent += new EventHandler(Master_ButtonClick);
		}

		private void Master_ButtonClick(object sender, EventArgs e)
		{

		}
        protected void Page_Load(object sender, EventArgs e)
        {

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



                if (Request.QueryString["Name"] != "" && Request.QueryString["Name"] != null)
                {
                    servernamelbl.Text = Request.QueryString["Name"];

                    linkDiskSpaceMore.NavigateUrl = "DiskHealth.aspx?server=" + servernamelbl.Text;
                    CpuHyperLink.NavigateUrl = "DominoServerStatisticsDetail.aspx?name=Cpu&server=" + servernamelbl.Text;
                    MemHyperLink.NavigateUrl = "DominoServerStatisticsDetail.aspx?name=Mem&server=" + servernamelbl.Text;
                    PerfrmHyperLink.NavigateUrl = "DominoServerStatisticsDetail.aspx?name=Perfm&server=" + servernamelbl.Text;
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

                // Overall tab
                SetGraph("hh", servernamelbl.Text);
                SetGraphForDiskSpace(servernamelbl.Text);
                SetGraphForCPU("hh", servernamelbl.Text);
                SetGraphForMemory("hh", servernamelbl.Text);
                SetGraphForRPCUsers("hh", servernamelbl.Text);
                SetGraphForOutlookUsers("hh", servernamelbl.Text);

                //--------
                SetGraphForMail(servernamelbl.Text);



            }

            // DataBase Code
            if (!IsPostBack && !IsCallback)
            {
                // performanceASPxRadioButtonList.Items.RemoveAt(0);
                // performanceASPxRadioButtonList.Items[0].Selected = true;

                //Maintenance tab
                Fillmaintenancegrid();

                //Alert tab
                FillAlertHistory();

                //Outages tab
                FillOutageTab();

                //Services tab
                FillMonitoredServicesGrid();
                FillNonMonitoredServicesGrid();

                //Exchange tab
                ExchangeServerHealth();
                ExchangeMailBoxReport();

                ExDAGHealthCopyStatus();
                ExDAGHealthCopySummary();
                ExDAGHealthMemberReport();

                ExQueuesStatus();

                //Web Admin tab
                SetupWebAdmin();

                if (Session["UserPreferences"] != null)
                {
                    DataTable UserPreferences = (DataTable)Session["UserPreferences"];
                    foreach (DataRow dr in UserPreferences.Rows)
                    {
                        if (dr[1].ToString() == "ExchangeServerDetailsPage2|ExchangeServerHealthGrid")
                        {
                            ExchangeServerHealthGrid.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                        }
                        if (dr[1].ToString() == "ExchangeServerDetailsPage2|ExchangeMailBoxReportGrid")
                        {
                            ExchangeMailBoxReportGrid.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                        }
                        if (dr[1].ToString() == "ExchangeServerDetailsPage2|ExDAGHealthCopyStatusGrid")
                        {
                            ExDAGHealthCopyStatusGrid.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                        }
                        if (dr[1].ToString() == "ExchangeServerDetailsPage2|ExDAGHealthCopySummaryGrid")
                        {
                            ExDAGHealthCopySummaryGrid.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                        }
                        if (dr[1].ToString() == "ExchangeServerDetailsPage2|ExDAGHealthMemberReportGrid")
                        {
                            ExDAGHealthMemberReportGrid.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                        }
                        if (dr[1].ToString() == "ExchangeServerDetailsPage2|ExQueuesGrid")
                        {
                            ExQueuesGrid.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                        }
                        if (dr[1].ToString() == "ExchangeServerDetailsPage2|MonitoredServicesGrid")
                        {
                            MonitoredServicesGrid.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                        }
                        if (dr[1].ToString() == "ExchangeServerDetailsPage2|NonMonitoredServicesGrid")
                        {
                            NonMonitoredServicesGrid.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                        }
                        if (dr[1].ToString() == "ExchangeServerDetailsPage2|maintenancegrid")
                        {
                            maintenancegrid.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                        }
                        if (dr[1].ToString() == "ExchangeServerDetailsPage2|AlertGridView")
                        {
                            AlertGridView.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                        }
                        if (dr[1].ToString() == "ExchangeServerDetailsPage2|OutageGridView")
                        {
                            OutageGridView.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                        }    
                    }
                }
            }
            else
            {
                //Maintenance tab
                Fillmaintenancegridfromsession();

                //Alert tab
                FillAlertHistoryfromSession();

                //Outages tab
                FilloutagefromSession();

                //Services tab
                FillMonitoredServicesGrid();
                FillNonMonitoredServicesGrid();

                //Exchange tab
                ExchangeServerHealthfromSession();
                ExchangeMailBoxReportfromSession();

                ExQueuesStatusfromSession();

                //Web Admin tab
            }
   
        }

        private void ExchangeServerHealth()
        {
            try
            {
                DataTable StatusTable1 = VSWebBL.DashboardBL.ExchangeServerDetailsBL.Ins.GetExchangeServerHealth(servernamelbl.Text);

                Session["ExchangeServerHealth"] = StatusTable1;
                ExchangeServerHealthGrid.DataSource = StatusTable1;
                ExchangeServerHealthGrid.DataBind();


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

        private void ExchangeServerHealthfromSession()
        {
            try
            {
                DataTable ExchangeServerHealthtab = new DataTable();
                ExchangeServerHealthtab = (DataTable)Session["ExchangeServerHealth"];
                ExchangeServerHealthGrid.DataSource = ExchangeServerHealthtab;
                ExchangeServerHealthGrid.DataBind();
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

        private void ExchangeMailBoxReport()
        {
            try
            {
                DataTable StatusTable1 = VSWebBL.DashboardBL.ExchangeServerDetailsBL.Ins.GetExchangeMailBoxReport(servernamelbl.Text);

                Session["ExchangeMailBoxReport"] = StatusTable1;
                ExchangeMailBoxReportGrid.DataSource = StatusTable1;
                ExchangeMailBoxReportGrid   .DataBind();


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

        private void ExchangeMailBoxReportfromSession()
        {
            try
            {
                DataTable ExchangeServerHealthtab = new DataTable();
                ExchangeServerHealthtab = (DataTable)Session["ExchangeMailBoxReport"];
                ExchangeMailBoxReportGrid.DataSource = ExchangeServerHealthtab;
                ExchangeServerHealthGrid.DataBind();
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

        public void FillOutageTab()
        {
            DataTable dt = new DataTable();
            dt = VSWebBL.DashboardBL.ExchangeServerDetailsBL.Ins.GetOutage(servernamelbl.Text, Request.QueryString["Type"].ToString());
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

        public DataTable SetGraph(string paramGraph, string DeviceName)
        {
            try
            {
                performanceWebChartControl.Series.Clear();
				DataTable dt = VSWebBL.DashboardBL.ExchangeServerDetailsBL.Ins.SetGraph(paramGraph, DeviceName, ServerTypeId);
                if (dt.Rows.Count > 0)
                {
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

                    // Create a constant line.
                    ConstantLine constantLine1 = new ConstantLine("Constant Line 1");
                    diagram.AxisY.ConstantLines.Add(constantLine1);

                    // Define its axis value.
                    constantLine1.AxisValue = 2000;

                    // Customize the behavior of the constant line.
                    // constantLine1.Visible = true;
                    //constantLine1.ShowInLegend = true;
                    // constantLine1.LegendText = "Some Threshold";
                    constantLine1.ShowBehind = true;

                    // Customize the constant line's title.
                    constantLine1.Title.Visible = true;
                    constantLine1.Title.Text = "Threshold:2000";
                    constantLine1.Title.TextColor = Color.Red;
                    // constantLine1.Title.Antialiasing = false;
                    //constantLine1.Title.Font = new Font("Tahoma", 14, FontStyle.Bold);
                    constantLine1.Title.ShowBelowLine = true;
                    constantLine1.Title.Alignment = ConstantLineTitleAlignment.Far;

                    // Customize the appearance of the constant line.
                    constantLine1.Color = Color.Red;
                    constantLine1.LineStyle.DashStyle = DashStyle.Solid;
                    constantLine1.LineStyle.Thickness = 2;

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
                }
                return dt;
            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
         
        }

       

        public void SetGraphForDiskSpace(string serverName)
        {
            if (!IsPostBack)
            {
                //int cnt = 0;
                diskspaceWebChartControl.Series.Clear();
                DataTable dt = VSWebBL.DashboardBL.ExchangeServerDetailsBL.Ins.SetGraphForDiskSpace(serverName);
                diskspaceWebChartControl.DataSource = dt;
                if (dt.Rows.Count > 0)
                {
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

                            seriesView.Titles[0].Text = series.Name;//.Substring(5);
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

                }
            }
        }

        public void SetGraphForCPU(string paramGraph, string serverName)
        {
            cpuWebChartControl.Series.Clear();
			DataTable dt = VSWebBL.DashboardBL.ExchangeServerDetailsBL.Ins.SetGraphForCPU(paramGraph, serverName, ServerTypeId);
            if (dt.Rows.Count > 0)
            {
                Series series = null;
                series = new Series("DominoServer", ViewType.Line);
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
                axis.GridSpacingAuto = false;
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
            }
            //return dt;     
        }

        //protected void cpuASPxRadioButtonList_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    SetGraphForCPU(cpuASPxRadioButtonList.Value.ToString(), servernamelbl.Text);
        //}

        public void SetGraphForMemory(string paramGraph, string serverName)
        {
            memWebChartControl.Series.Clear();
			DataTable dt = VSWebBL.DashboardBL.ExchangeServerDetailsBL.Ins.SetGraphForMemory(paramGraph, serverName, ServerTypeId);
            if (dt.Rows.Count > 0)
            {
                Series series = null;
                series = new Series("DominoServer", ViewType.Line);
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
                axis.GridSpacingAuto = false;
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
        }

        //protected void memASPxRadioButtonList_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    SetGraphForMemory(memASPxRadioButtonList.Value.ToString(), servernamelbl.Text);
        //}

        public void SetGraphForRPCUsers(string paramGraph, string serverName)
        {
            RPCusersWebChartControl.Series.Clear();
            //DataTable dt = VSWebBL.DashboardBL.ExchangeServerDetailsBL.Ins.SetGraphForUsers(paramGraph, serverName);
			DataTable dt = VSWebBL.DashboardBL.ExchangeServerDetailsBL.Ins.SetGraphRPCClientAccess(paramGraph, serverName, ServerTypeId);
            if (dt.Rows.Count > 0)
            {
                Series series = null;
                series = new Series("DominoServer", ViewType.Line);
                series.Visible = true;
                series.ArgumentDataMember = dt.Columns["dtfrom"].ToString();

                ValueDataMemberCollection seriesValueDataMembers = (ValueDataMemberCollection)series.ValueDataMembers;
                seriesValueDataMembers.AddRange(dt.Columns["maxval"].ToString());
                RPCusersWebChartControl.Series.Add(series);

                ((XYDiagram)RPCusersWebChartControl.Diagram).PaneLayoutDirection = PaneLayoutDirection.Horizontal;

                XYDiagram seriesXY = (XYDiagram)RPCusersWebChartControl.Diagram;
                seriesXY.AxisY.Title.Text = "User Count";
                seriesXY.AxisY.Title.Visible = true;
                seriesXY.AxisX.Title.Text = "Date/Time";
                seriesXY.AxisX.Title.Visible = true;
                RPCusersWebChartControl.Legend.Visible = false;

                //((SplineSeriesView)series.View).LineTensionPercent = 100;
                ((LineSeriesView)series.View).LineMarkerOptions.Size = 4;
                ((LineSeriesView)series.View).LineMarkerOptions.Color = Color.White;

                AxisBase axis = ((XYDiagram)RPCusersWebChartControl.Diagram).AxisX;
                //4/18/2014 NS commented out for VSPLUS-312
                //axis.DateTimeGridAlignment = DateTimeMeasurementUnit.Hour;
                //axis.MinorCount = 15;
                //axis.GridSpacing = 0.5;
                axis.Range.SideMarginsEnabled = false;
                axis.GridLines.Visible = false;
                //axis.DateTimeOptions.Format = DateTimeFormat.Custom;

                ((LineSeriesView)series.View).Color = Color.Blue;

                //10/9/2013 NS moved the ds assignment prior to setting the grid spacing option
                RPCusersWebChartControl.DataSource = dt;
                RPCusersWebChartControl.DataBind();

                AxisBase axisy = ((XYDiagram)RPCusersWebChartControl.Diagram).AxisY;
                axisy.Range.AlwaysShowZeroLevel = false;
                axisy.Range.SideMarginsEnabled = true;
                axisy.GridLines.Visible = true;

                //10/8/2013 NS added
                seriesXY.AxisY.NumericOptions.Format = NumericFormat.Number;
                seriesXY.AxisY.NumericOptions.Precision = 0;
                seriesXY.AxisY.GridSpacingAuto = true;

                double min = Convert.ToDouble(((XYDiagram)RPCusersWebChartControl.Diagram).AxisY.Range.MinValue);
                double max = Convert.ToDouble(((XYDiagram)RPCusersWebChartControl.Diagram).AxisY.Range.MaxValue);

                int gs = (int)((max - min) / 5);

                if (gs == 0)
                {
                    gs = 1;
                    seriesXY.AxisY.GridSpacingAuto = false;
                    seriesXY.AxisY.GridSpacing = gs;
                }
            }
        }

        protected void usersASPxRadioButtonList_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetGraphForRPCUsers(usersASPxRadioButtonList.Value.ToString(), servernamelbl.Text);
        }
        public void SetGraphForOutlookUsers(string paramGraph, string serverName)
        {
            OutlookusersWebChartControl.Series.Clear();
            //DataTable dt = VSWebBL.DashboardBL.ExchangeServerDetailsBL.Ins.SetGraphForUsers(paramGraph, serverName);
			DataTable dt = VSWebBL.DashboardBL.ExchangeServerDetailsBL.Ins.SetGraphOutlookWebApp(paramGraph, serverName, ServerTypeId);
            if (dt.Rows.Count > 0)
            {
                Series series = null;
                series = new Series("DominoServer", ViewType.Line);
                series.Visible = true;
                series.ArgumentDataMember = dt.Columns["dtfrom"].ToString();

                ValueDataMemberCollection seriesValueDataMembers = (ValueDataMemberCollection)series.ValueDataMembers;
                seriesValueDataMembers.AddRange(dt.Columns["maxval"].ToString());
                OutlookusersWebChartControl.Series.Add(series);

                ((XYDiagram)OutlookusersWebChartControl.Diagram).PaneLayoutDirection = PaneLayoutDirection.Horizontal;

                XYDiagram seriesXY = (XYDiagram)OutlookusersWebChartControl.Diagram;
                seriesXY.AxisY.Title.Text = "User Count";
                seriesXY.AxisY.Title.Visible = true;
                seriesXY.AxisX.Title.Text = "Date/Time";
                seriesXY.AxisX.Title.Visible = true;
                OutlookusersWebChartControl.Legend.Visible = false;

                //((SplineSeriesView)series.View).LineTensionPercent = 100;
                ((LineSeriesView)series.View).LineMarkerOptions.Size = 4;
                ((LineSeriesView)series.View).LineMarkerOptions.Color = Color.White;

                AxisBase axis = ((XYDiagram)OutlookusersWebChartControl.Diagram).AxisX;
                //4/18/2014 NS commented out for VSPLUS-312
                //axis.DateTimeGridAlignment = DateTimeMeasurementUnit.Hour;
                //axis.MinorCount = 15;
                //axis.GridSpacing = 0.5;
                axis.Range.SideMarginsEnabled = false;
                axis.GridLines.Visible = false;
                //axis.DateTimeOptions.Format = DateTimeFormat.Custom;

                ((LineSeriesView)series.View).Color = Color.Blue;

                //10/9/2013 NS moved the ds assignment prior to setting the grid spacing option
                OutlookusersWebChartControl.DataSource = dt;
                OutlookusersWebChartControl.DataBind();

                AxisBase axisy = ((XYDiagram)OutlookusersWebChartControl.Diagram).AxisY;
                axisy.Range.AlwaysShowZeroLevel = false;
                axisy.Range.SideMarginsEnabled = true;
                axisy.GridLines.Visible = true;

                //10/8/2013 NS added
                seriesXY.AxisY.NumericOptions.Format = NumericFormat.Number;
                seriesXY.AxisY.NumericOptions.Precision = 0;
                seriesXY.AxisY.GridSpacingAuto = true;

                double min = Convert.ToDouble(((XYDiagram)OutlookusersWebChartControl.Diagram).AxisY.Range.MinValue);
                double max = Convert.ToDouble(((XYDiagram)OutlookusersWebChartControl.Diagram).AxisY.Range.MaxValue);

                int gs = (int)((max - min) / 5);

                if (gs == 0)
                {
                    gs = 1;
                    seriesXY.AxisY.GridSpacingAuto = false;
                    seriesXY.AxisY.GridSpacing = gs;
                }
            }
        }

        protected void OutlookusersASPxRadioButtonList_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetGraphForOutlookUsers(OutlookusersASPxRadioButtonList.Value.ToString(), servernamelbl.Text);
        }

       //// public void FillCombobox()
       // {
       //    DataTable trafictab = VSWebBL.DashboardBL.DashboardBL.Ins.GetCombobox();
       //   MailComboBox.DataSource = trafictab;
       //   MailComboBox.TextField = "NameandType";
       //   MailComboBox.DataBind();
       // }

        public void SetGraphForMail(string serverName)
        {
           
            MailWebChartControl.Series.Clear();
            DataTable dt = VSWebBL.DashboardBL.DashboardBL.Ins.GetMailStatus(serverName);
            if (dt.Rows.Count > 0)
            {
                Series series = new Series("DeviceType", ViewType.Bar);

                series.ArgumentDataMember = dt.Columns["Mail"].ToString();

                ValueDataMemberCollection seriesValueDataMembers = (ValueDataMemberCollection)series.ValueDataMembers;
                seriesValueDataMembers.AddRange(dt.Columns["Value"].ToString());
                MailWebChartControl.Series.Add(series);

                MailWebChartControl.Legend.Visible = false;

                ((SideBySideBarSeriesView)series.View).ColorEach = true;

                AxisBase axisx = ((XYDiagram)MailWebChartControl.Diagram).AxisX;
                axisx.GridLines.Visible = false;
                XYDiagram seriesXY = (XYDiagram)MailWebChartControl.Diagram;
                seriesXY.AxisX.Title.Text = "Mail Status";
                seriesXY.AxisX.Title.Visible = true;
                seriesXY.AxisX.Label.ResolveOverlappingOptions.AllowRotate = false;
                seriesXY.AxisX.Label.ResolveOverlappingOptions.AllowStagger = true;
                seriesXY.AxisY.Title.Text = "Count";
                seriesXY.AxisY.Title.Visible = true;
                //10/8/2013 NS added
                seriesXY.AxisY.NumericOptions.Format = NumericFormat.Number;
                seriesXY.AxisY.NumericOptions.Precision = 0;
                seriesXY.AxisY.Label.ResolveOverlappingOptions.AllowRotate = false;

                AxisBase axisy = ((XYDiagram)MailWebChartControl.Diagram).AxisY;
                axisy.Range.AlwaysShowZeroLevel = false;
                axisy.GridLines.Visible = false;

                MailWebChartControl.DataSource = dt;
                MailWebChartControl.DataBind();

                double min = Convert.ToDouble(((XYDiagram)MailWebChartControl.Diagram).AxisY.Range.MinValue);
                double max = Convert.ToDouble(((XYDiagram)MailWebChartControl.Diagram).AxisY.Range.MaxValue);

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
            }
            
        }

        //protected void mailASPxRadioButtonList_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    SetGraphForMail(mailASPxRadioButtonList.Value.ToString(), servernamelbl.Text);
        //}


        //DataBase Code


  
        protected void MonitoredDBGridView_HtmlDataCellPrepared(object sender, DevExpress.Web.ASPxGridViewTableDataCellEventArgs e)
        {
           if (e.DataColumn.FieldName == "Status" &&(e.CellValue.ToString() == "OK"|| e.CellValue.ToString()=="Scanning"))
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

         public void FillMonitoredServicesGrid()
         {

             try
             {
                 DataTable StatusTable1 = VSWebBL.DashboardBL.ExchangeServerDetailsBL.Ins.GetMoniteredServices(servernamelbl.Text,"1");

                 Session["MonitoredServices"] = StatusTable1;
                 MonitoredServicesGrid.DataSource = StatusTable1;
                 MonitoredServicesGrid.DataBind();


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
         public void FillNonMonitoredServicesGrid()
         {

             try
             {
                 DataTable StatusTable1 = VSWebBL.DashboardBL.ExchangeServerDetailsBL.Ins.GetMoniteredServices(servernamelbl.Text, "0");

                 Session["NonMonitoredServices"] = StatusTable1;
                 NonMonitoredServicesGrid.DataSource = StatusTable1;
                 NonMonitoredServicesGrid.DataBind();


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
        //  public void FillMonMoniteredGrid()
        //{

        //    try
        //    {
        //        DataTable StatusTable1 = VSWebBL.DashboardBL.DashboardBL.Ins.GetNonMoniteredServerTasks(servernamelbl.Text);
              
        //        Session["NonMoniterdeServerTasks"] = StatusTable1;
        //        NonmoniterdGrid.DataSource = StatusTable1;
        //        NonmoniterdGrid.DataBind();
             

        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //    finally
        //    {
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
            DataTable dt = VSWebBL.ConfiguratorBL.MaintenanceBL.Ins.Getmaintenanceserversbyservername(Request.QueryString["Name"].ToString(),"","","","");
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

                AlertHistorytab =(DataTable) Session["AlertHistorytab"];
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

        protected void BackButton_Click(object sender, EventArgs e)
        {
            if (Session["BackURL"] != "" && Session["BackURL"] != null)
            {
                Response.Redirect(Session["BackURL"].ToString());
                Session["BackURL"] = "";
            
            }
        }

        
        protected void OpenDBButton_Click(object sender, EventArgs e)
        {

        }

        protected void ASPxPageControl1_TabClick(object source, DevExpress.Web.TabControlCancelEventArgs e)
        {
            if (e.Tab.Text == "Web Admin")
            {
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
            }
        }

        public void SetupWebAdmin()
        {
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
        }

      

        //protected void ServerTasksPopupMenu_ItemClick(object source, DevExpress.Web.MenuItemEventArgs e)
        //{
        //    if (e.Item.Name == "ServerTaskStart")
        //    {
        //        if (DominoserverTasksgrid.FocusedRowIndex > -1)
        //        {
        //            YesButton.Visible = true;
        //            NOButton.Visible = true;
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
        //        }
        //    }
        //    if (e.Item.Name == "ServerTaskStop")
        //    {
        //        if (DominoserverTasksgrid.FocusedRowIndex > -1)
        //        {
        //            YesButton.Visible = true;
        //            NOButton.Visible = true;
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
        //        }
        //    }
        //    if (e.Item.Name == "ServerTaskRestart")
        //    {
        //        if (DominoserverTasksgrid.FocusedRowIndex > -1)
        //        {
        //            YesButton.Visible = true;
        //            NOButton.Visible = true;
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
        //        }
        //    }
        //}

        //private void ServerTaskRestart()
        //{
        //    string TellCommand = "";
        //    string myUserName = "";
        //    string myDeviceName = "";
        //    string myServerName = "";

        //    DataRow myRow = DominoserverTasksgrid.GetDataRow(DominoserverTasksgrid.FocusedRowIndex);
        //    try
        //    {

        //        Session["TaskName"] = myRow["TaskName"].ToString();
        //        Session["MenuItem"] = "ServerTaskRestart";

        //        myUserName = Session["UserLogin"].ToString();
        //        Session["myUserName"] = myUserName;
        //        myServerName = servernamelbl.Text;
        //        Session["myServerName"] = myServerName;

        //        VSWebDO.DominoServerTasks DSTasksObject = new VSWebDO.DominoServerTasks();
        //        DSTasksObject.TaskName = myRow["TaskName"].ToString();
        //        VSWebDO.DominoServerTasks ReturnValue = VSWebBL.ConfiguratorBL.DominoServerTasksBL.Ins.GetDataForTaskName(DSTasksObject);
        //        TellCommand = ReturnValue.LoadString;

        //        TellCommand = "tell " + TellCommand.ToString().Substring(3) + " restart";
        //        Session["TellCommand"] = TellCommand;




        //        VSWebBL.DashboardBL.LotusTravelerHealthBLL.Ins.SENDTravelerConsoleCommand(myServerName, TellCommand, myUserName);

        //        msgPopupControl1.HeaderText = "Server Tasks Restart";
        //        msgPopupControl1.ShowOnPageLoad = true;
        //        msgLabel.Text = "The request to restart the task '" + myRow["TaskName"].ToString() + "' is sent.";
        //        YesButton.Visible = false;
        //        NOButton.Visible = false;
        //    }
        //    catch (Exception)
        //    {
        //        myUserName = "";
        //    }
        //    //msgPopupControl1.ShowOnPageLoad = true;
        //    //msgLabel.Text = "Are you sure you want to Restart " + myRow["TaskName"].ToString() + " on server " + myServerName + "?";                      
  
        //}

        //private void ServerTaskStop()
        //{
        //    string TellCommand = "";
        //    string myUserName = "";
        //    string myDeviceName = "";
        //    string myServerName = "";

        //    DataRow myRow = DominoserverTasksgrid.GetDataRow(DominoserverTasksgrid.FocusedRowIndex);
        //    try
        //    {
        //        Session["TaskName"] = myRow["TaskName"].ToString();
        //        Session["MenuItem"] = "ServerTaskStop";

        //        myUserName = Session["UserLogin"].ToString();
        //         Session["myUserName"] = myUserName;
        //        myServerName = servernamelbl.Text;
        //         Session["myServerName"] = myServerName;

        //         VSWebDO.DominoServerTasks DSTasksObject = new VSWebDO.DominoServerTasks();
        //         DSTasksObject.TaskName = myRow["TaskName"].ToString();
        //         VSWebDO.DominoServerTasks ReturnValue = VSWebBL.ConfiguratorBL.DominoServerTasksBL.Ins.GetDataForTaskName(DSTasksObject);
        //         TellCommand = ReturnValue.LoadString;

        //         TellCommand = "tell " + TellCommand.Substring(3) + " exit";
        //         Session["TellCommand"] = TellCommand;



        //         VSWebBL.DashboardBL.LotusTravelerHealthBLL.Ins.SENDTravelerConsoleCommand(myServerName, TellCommand, myUserName);

        //         msgPopupControl1.HeaderText = "Server Tasks Stop";
        //         msgPopupControl1.ShowOnPageLoad = true;
        //         msgLabel.Text = "The request to stop the task '" + myRow["TaskName"].ToString() + "' is sent.";
        //         YesButton.Visible = false;
        //         NOButton.Visible = false;
        //     }
        //    catch (Exception)
        //    {
        //        myUserName = "";
        //    }
        //    //msgPopupControl1.ShowOnPageLoad = true;
        //    //msgLabel.Text = "Are you sure you want to Stop " + myRow["TaskName"].ToString() + " on server " + myServerName + "?";                      
  
        //}

        //private void ServerTaskStart()
        //{
        //    string TellCommand = "";
        //    string myUserName = "";
        //    string myDeviceName = "";
        //    string myServerName = "";

        //    DataRow myRow = DominoserverTasksgrid.GetDataRow(DominoserverTasksgrid.FocusedRowIndex);
        //    try
        //    {

        //        myUserName = Session["UserLogin"].ToString();
        //        myServerName = servernamelbl.Text;

        //        VSWebDO.DominoServerTasks DSTasksObject = new VSWebDO.DominoServerTasks();
        //        DSTasksObject.TaskName = myRow["TaskName"].ToString();
        //       VSWebDO.DominoServerTasks ReturnValue = VSWebBL.ConfiguratorBL.DominoServerTasksBL.Ins.GetDataForTaskName(DSTasksObject);
        //       TellCommand = ReturnValue.LoadString;

        //        VSWebBL.DashboardBL.LotusTravelerHealthBLL.Ins.SENDTravelerConsoleCommand(myServerName, TellCommand, myUserName);

        //        msgPopupControl1.HeaderText = "Server Tasks Start";
        //        msgPopupControl1.ShowOnPageLoad = true;
        //        msgLabel.Text = "The request to start the task '" + myRow["TaskName"].ToString() + "' is sent.";
        //        YesButton.Visible = false;
        //        NOButton.Visible = false;
        //    }
        //    catch (Exception)
        //    {
        //        myUserName = "";
        //    }
                     
        //}
      

        //protected void DominoserverTasksgrid_HtmlRowCreated(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
        //{
        //    int index = DominoserverTasksgrid.FocusedRowIndex;
        //    if (e.VisibleIndex != index)
        //    {

        //        e.Row.Attributes.Add("onmouseover", "this.originalcolor=this.style.backgroundColor;" + "this.style.backgroundColor='#C0C0C0';"); //'#C0C0C0'


        //        e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalcolor;");
        //    }
        //}



        private void ExDAGHealthCopyStatus()
        {
            try
            {
                DataTable StatusTable1 = VSWebBL.DashboardBL.ExchangeServerDetailsBL.Ins.GetExDAGHealthCopyStatus(servernamelbl.Text);

                Session["ExDAGHealthCopyStatus"] = StatusTable1;
                ExDAGHealthCopyStatusGrid.DataSource = StatusTable1;
                ExDAGHealthCopyStatusGrid.DataBind();


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

        private void ExDAGHealthCopyStatusfromSession()
        {
            try
            {
                DataTable ExDAGHealthCopyStatustab = new DataTable();
                ExDAGHealthCopyStatustab = (DataTable)Session["ExDAGHealthCopyStatus"];
                ExDAGHealthCopyStatusGrid.DataSource = ExDAGHealthCopyStatustab;
                ExDAGHealthCopyStatusGrid.DataBind();
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

        private void ExDAGHealthCopySummary()
        {
            try
            {
                DataTable StatusTable1 = VSWebBL.DashboardBL.ExchangeServerDetailsBL.Ins.GetExDAGHealthCopySummary(servernamelbl.Text);

                Session["ExDAGHealthCopySummary"] = StatusTable1;
                ExDAGHealthCopySummaryGrid.DataSource = StatusTable1;
                ExDAGHealthCopySummaryGrid.DataBind();


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

        private void ExDAGHealthCopySummaryfromSession()
        {
            try
            {
                DataTable ExDAGHealthCopySummarytab = new DataTable();
                ExDAGHealthCopySummarytab = (DataTable)Session["ExDAGHealthCopySummary"];
                ExDAGHealthCopySummaryGrid.DataSource = ExDAGHealthCopySummarytab;
                ExDAGHealthCopySummaryGrid.DataBind();
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
        private void ExDAGHealthMemberReport()
        {
            try
            {
                DataTable StatusTable1 = VSWebBL.DashboardBL.ExchangeServerDetailsBL.Ins.GetExDAGHealthMemberReport(servernamelbl.Text);

                Session["ExDAGHealthMemberReport"] = StatusTable1;
                ExDAGHealthMemberReportGrid.DataSource = StatusTable1;
                ExDAGHealthMemberReportGrid.DataBind();


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

        private void ExDAGHealthMemberReportfromSession()
        {
            try
            {
                DataTable ExDAGHealthMemberReporttab = new DataTable();
                ExDAGHealthMemberReporttab = (DataTable)Session["ExDAGHealthMemberReport"];
                ExDAGHealthMemberReportGrid.DataSource = ExDAGHealthMemberReporttab;
                ExDAGHealthMemberReportGrid.DataBind();
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


        private void ExQueuesStatus()
        {
            try
            {
                DataTable StatusTable1 = VSWebBL.DashboardBL.ExchangeServerDetailsBL.Ins.GetExQueuesStatus(servernamelbl.Text);

                Session["ExQueues"] = StatusTable1;
                ExQueuesGrid.DataSource = StatusTable1;
                ExQueuesGrid.DataBind();


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

        private void ExQueuesStatusfromSession()
        {
            try
            {
                DataTable ExQueuestab = new DataTable();
                ExQueuestab = (DataTable)Session["ExQueues"];
                ExQueuesGrid.DataSource = ExQueuestab;
                ExQueuesGrid.DataBind();
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

        protected void ExchangeServerHealthGrid_PageSizeChanged(object sender, EventArgs e)
        {
            //ProfilesGridView.PageIndex;
            VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("ExchangeServerDetailsPage2|ExchangeServerHealthGrid", ExchangeServerHealthGrid.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        }
        protected void ExchangeMailBoxReportGrid_PageSizeChanged(object sender, EventArgs e)
        {
            //ProfilesGridView.PageIndex;
            VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("ExchangeServerDetailsPage2|ExchangeMailBoxReportGrid", ExchangeMailBoxReportGrid.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        }
        protected void ExDAGHealthCopyStatusGrid_PageSizeChanged(object sender, EventArgs e)
        {
            //ProfilesGridView.PageIndex;
            VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("ExchangeServerDetailsPage2|ExDAGHealthCopyStatusGrid", ExDAGHealthCopyStatusGrid.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        }
        protected void ExDAGHealthCopySummaryGrid_PageSizeChanged(object sender, EventArgs e)
        {
            //ProfilesGridView.PageIndex;
            VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("ExchangeServerDetailsPage2|ExDAGHealthCopySummaryGrid", ExDAGHealthCopySummaryGrid.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        }
        protected void ExDAGHealthMemberReportGrid_PageSizeChanged(object sender, EventArgs e)
        {
            //ProfilesGridView.PageIndex;
            VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("ExchangeServerDetailsPage2|ExDAGHealthMemberReportGrid", ExDAGHealthMemberReportGrid.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        }
        protected void ExQueuesGrid_PageSizeChanged(object sender, EventArgs e)
        {
            //ProfilesGridView.PageIndex;
            VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("ExchangeServerDetailsPage2|ExQueuesGrid", ExQueuesGrid.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        }
        protected void MonitoredServicesGrid_PageSizeChanged(object sender, EventArgs e)
        {
            //ProfilesGridView.PageIndex;
            VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("ExchangeServerDetailsPage2|MonitoredServicesGrid", MonitoredServicesGrid.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        }
        protected void NonMonitoredServicesGrid_PageSizeChanged(object sender, EventArgs e)
        {
            //ProfilesGridView.PageIndex;
            VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("ExchangeServerDetailsPage2|NonMonitoredServicesGrid", NonMonitoredServicesGrid.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        }
        protected void maintenancegrid_PageSizeChanged(object sender, EventArgs e)
        {
            //ProfilesGridView.PageIndex;
            VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("ExchangeServerDetailsPage2|maintenancegrid", maintenancegrid.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        }
        protected void AlertGridView_PageSizeChanged(object sender, EventArgs e)
        {
            //ProfilesGridView.PageIndex;
            VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("ExchangeServerDetailsPage2|AlertGridView", AlertGridView.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        }
        protected void OutageGridView_PageSizeChanged(object sender, EventArgs e)
        {
            //ProfilesGridView.PageIndex;
            VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("ExchangeServerDetailsPage2|OutageGridView", OutageGridView.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        }
        
      }

    }




   
