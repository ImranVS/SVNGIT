using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using DevExpress.XtraCharts;
using System.Data;

namespace VSWebUI.DashboardReports
{
    public partial class ServerUpTimeXtraRpt : DevExpress.XtraReports.UI.XtraReport
    {
        public ServerUpTimeXtraRpt()
        {
            InitializeComponent();
        }

        private void ServerUpTimeXtraRpt_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            string serverName = "";
            string srvName = "";
            string sName = "";
            //12/24/2013 NS added
            string downMin = "";
            bool exactmatch;
            //1/30/2015 NS added for VSPLUS-1370
            string serverType = "";
            string srvType = "";
            string sType = "";
            serverName = this.ServerNameSQL.Value.ToString();
            srvName = this.ServerName.Value.ToString();
            exactmatch = (bool)this.ExactMatch.Value;
            //12/24/2013 NS added
            downMin = this.DownMin.Value.ToString();
            sName = srvName;
            //1/30/2015 NS added for VSPLUS-1370
            serverType = this.ServerTypeSQL.Value.ToString();
            srvType = this.ServerType.Value.ToString();
            sType = serverType;
            if (srvName == "" || exactmatch)
            {
                sName = serverName;
            }
            xrChart1.Series.Clear();
            DataTable dt = new DataTable();
            Series series1;
            XYDiagram seriesXY;
            //12/24/2013 NS modified - added down minutes
            dt = VSWebBL.ReportsBL.ReportsBL.Ins.ServerAvailabilityRptBL(int.Parse(this.DateM.Value.ToString()), int.Parse(this.DateY.Value.ToString()),
                sName, exactmatch, downMin, sType, int.Parse(this.DateD.Value.ToString()));
            //1/26/2016 NS modified for VSPLUS-2486
            if (dt.Rows.Count > 0)
            {
                if (this.RptType.Value == "Minutes")
                {
                    series1 = new Series("UpMinutes", ViewType.StackedBar);
                    series1.ValueDataMembers.AddRange(dt.Columns["UpMinutes"].ToString());
                    series1.LegendText = "Up Minutes";
                    xrChart1.Series.AddRange(new Series[] { series1 });
                    (series1.View as StackedBarSeriesView).Transparency = 160;
                    seriesXY = (XYDiagram)xrChart1.Diagram;
                    seriesXY.AxisX.Label.ResolveOverlappingOptions.AllowRotate = true;
                    seriesXY.AxisX.Label.ResolveOverlappingOptions.AllowHide = false;
                    seriesXY.AxisY.Title.Text = "Up Minutes per Month";
                    seriesXY.AxisX.Title.Text = "Server Name";
                    seriesXY.AxisY.Title.Visible = true;
                    seriesXY.AxisX.Title.Visible = true;
                    xrLabel3.Text = "Server Up Time in Minutes";
                }
                else
                {
                    series1 = new Series("UpPct", ViewType.StackedBar);
                    series1.ValueDataMembers.AddRange(dt.Columns["UpPct"].ToString());
                    series1.LegendText = "Up Percent";
                    xrChart1.Series.AddRange(new Series[] { series1 });
                    (series1.View as StackedBarSeriesView).Transparency = 160;
                    seriesXY = (XYDiagram)xrChart1.Diagram;
                    seriesXY.AxisX.Label.ResolveOverlappingOptions.AllowRotate = true;
                    seriesXY.AxisX.Label.ResolveOverlappingOptions.AllowHide = false;
                    seriesXY.AxisY.Title.Text = "Up Percent per Month";
                    seriesXY.AxisX.Title.Text = "Server Name";
                    seriesXY.AxisY.Title.Visible = true;
                    seriesXY.AxisX.Title.Visible = true;
                    xrLabel3.Text = "Server Up Time in Percent";
                }
                series1.ArgumentDataMember = dt.Columns["DeviceName"].ToString();
                series1.ShowInLegend = true;
                xrChart1.DataSource = dt;

                this.DataSource = dt;
                this.DataMember = "Table";
                xrTableCell1.DataBindings.Add("Text", dt, "DeviceName");
                xrTableCell2.DataBindings.Add("Text", dt, "DownMinutes");
                xrTableCell3.DataBindings.Add("Text", dt, "UpMinutes");
                xrTableCell8.DataBindings.Add("Text", dt, "UpPct");
            }
            MonthYearLabel.Text = "Report for " + this.MonthYear.Value.ToString();
        }

    }
}
