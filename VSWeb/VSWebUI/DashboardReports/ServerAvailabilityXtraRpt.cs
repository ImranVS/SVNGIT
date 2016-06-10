using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using DevExpress.XtraCharts;
using System.Data;

namespace VSWebUI.DashboardReports
{
    public partial class ServerAvailabilityXtraRpt : DevExpress.XtraReports.UI.XtraReport
    {
        public ServerAvailabilityXtraRpt()
        {
            InitializeComponent();
        }

        private void ServerAvailabilityXtraRpt_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            SetReport(this);
        }

        public void SetReport(DashboardReports.ServerAvailabilityXtraRpt report)
        {
            string serverName = "";
            string srvName = "";
            string sName = "";
            //12/24/2013 NS added
            string downMin = "";
            bool exactmatch;

            serverName = this.ServerNameSQL.Value.ToString();
            srvName = this.ServerName.Value.ToString();
            exactmatch = (bool)this.ExactMatch.Value;
            //12/24/2013 NS added
            downMin = this.DownMin.Value.ToString();
            sName = srvName;
            if (srvName == "" || exactmatch)
            {
                sName = serverName;
            }
            xrChart1.Series.Clear();
            DataTable dt = new DataTable();
            //12/24/2013 NS modified - added down minutes
            dt = VSWebBL.ReportsBL.ReportsBL.Ins.ServerAvailabilityRptBL(int.Parse(this.DateM.Value.ToString()), int.Parse(this.DateY.Value.ToString()),
                sName, exactmatch, downMin, this.ServerTypeSQL.Value.ToString(), int.Parse(this.DateD.Value.ToString()));
            //1/26/2016 NS modified for VSPLUS-2486
            if (dt.Rows.Count > 0)
            {
                Series series1 = new Series("DownMinutes", ViewType.StackedBar);

                series1.ArgumentDataMember = dt.Columns["DeviceName"].ToString();
                series1.ValueDataMembers.AddRange(dt.Columns["DownMinutes"].ToString());
                series1.ShowInLegend = true;
                series1.LegendText = "Down Minutes";

                xrChart1.Series.AddRange(new Series[] { series1 });
                //xrChart1.Legend.Visible = true;
                (series1.View as StackedBarSeriesView).Transparency = 160;
                XYDiagram seriesXY = (XYDiagram)xrChart1.Diagram;
                seriesXY.AxisX.Label.ResolveOverlappingOptions.AllowRotate = true;
                seriesXY.AxisX.Label.ResolveOverlappingOptions.AllowHide = false;
                seriesXY.AxisY.Title.Text = "Down Minutes per Month";
                seriesXY.AxisX.Title.Text = "Server Name";
                seriesXY.AxisY.Title.Visible = true;
                seriesXY.AxisX.Title.Visible = true;
                xrChart1.DataSource = dt;

                this.DataSource = dt;
                this.DataMember = "Table";
                xrTableCell1.DataBindings.Add("Text", dt, "DeviceName");
                xrTableCell2.DataBindings.Add("Text", dt, "DownMinutes");
                xrTableCell3.DataBindings.Add("Text", dt, "UpMinutes");
            }
            MonthYearLabel.Text = "Report for " + this.MonthYear.Value.ToString();
        }
    }
}
