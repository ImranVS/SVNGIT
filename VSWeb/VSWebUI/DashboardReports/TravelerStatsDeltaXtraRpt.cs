using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using DevExpress.XtraCharts;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace VSWebUI.DashboardReports
{
    public partial class TravelerStatsDeltaXtraRpt : DevExpress.XtraReports.UI.XtraReport
    {
        public TravelerStatsDeltaXtraRpt()
        {
            InitializeComponent();
        }

        private void TravelerStatsDeltaXtraRpt_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            string interval = "";
            string travelername = "";
            //TravelerStatsRptDSTableAdapters.TravelerStatsTableAdapter travelerAdapter = new TravelerStatsRptDSTableAdapters.TravelerStatsTableAdapter();
            //travelerAdapter.Fill(this.travelerStatsRptDS1.TravelerStats, this.Parameters["Interval"].Value.ToString(), this.Parameters["ServerName"].Value.ToString());
            if (this.Parameters["Interval"].Value.ToString() == "")
            {
                this.IntervalLabel.Text = "";
            }
            else
            {
                interval = this.Parameters["Interval"].Value.ToString();
                travelername = this.Parameters["ServerName"].Value.ToString();
                if (interval.Substring(4, 3).ToUpper() == "INF")
                {
                    this.IntervalLabel.Text = "Traveler server " + travelername + " taking " + Convert.ToInt32(interval.Substring(0, 3)) + " - " + "infinity seconds to complete";
                }
                else
                {
                    this.IntervalLabel.Text = "Traveler server " + travelername + " taking " + Convert.ToInt32(interval.Substring(0, 3)) + " - " + Convert.ToInt32(interval.Substring(4, 3)) + " seconds to complete";
                }
            }
        }

        private void xrChart1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            xrChart1.Series.Clear();
            DataTable dt = new DataTable();
            DateTime dtime = new DateTime();
            bool isSummary = false;
            dtime = DateTime.Now;
            dtime = dtime.AddDays(-3);
            isSummary = Convert.ToDateTime(this.Parameters["StartDate"].Value.ToString()) < dtime;
            dt = VSWebBL.ReportsBL.ReportsBL.Ins.GetTravelerStatsDelta(this.Parameters["Interval"].Value.ToString(),
                    this.Parameters["ServerName"].Value.ToString(), this.Parameters["StartDate"].Value.ToString(),
                    this.Parameters["EndDate"].Value.ToString(), isSummary);
            xrChart1.SeriesDataMember = "MailServerName";
            xrChart1.SeriesTemplate.ArgumentDataMember = "DateUpdated";
            xrChart1.SeriesTemplate.ValueDataMembers.AddRange(new string[] { "Delta" });
            xrChart1.SeriesTemplate.View = new SideBySideBarSeriesView();
            xrChart1.SeriesTemplate.ChangeView(ViewType.Line);
            LineSeriesView view = (LineSeriesView)xrChart1.SeriesTemplate.View;
            view.LineMarkerOptions.Visible = true;
            view.LineMarkerOptions.Size = 8;
            xrChart1.Legend.Visible = true;

            XYDiagram seriesXY = (XYDiagram)xrChart1.Diagram;
            seriesXY.AxisX.Title.Visible = true;
            seriesXY.AxisX.Label.ResolveOverlappingOptions.AllowRotate = true;
            seriesXY.AxisX.Label.ResolveOverlappingOptions.AllowHide = true;
            //4/18/2014 NS commented out for VSPLUS-312
            //seriesXY.AxisX.DateTimeGridAlignment = DateTimeMeasurementUnit.Minute;
            //4/29/2014 NS modified for VSPLUS-557
            if (isSummary)
            {
                seriesXY.AxisX.Title.Text = "Date";
                seriesXY.AxisY.Title.Text = "Open Times Delta (Average)";
                //seriesXY.AxisX.DateTimeMeasureUnit = DateTimeMeasurementUnit.Day;
                seriesXY.AxisX.DateTimeOptions.Format = DateTimeFormat.ShortDate;
                seriesXY.AxisY.Range.AlwaysShowZeroLevel = false;
            }
            else
            {
                seriesXY.AxisX.Title.Text = "Time";
                seriesXY.AxisY.Title.Text = "Open Times Delta";
                seriesXY.AxisX.DateTimeMeasureUnit = DateTimeMeasurementUnit.Hour;
                seriesXY.AxisX.DateTimeOptions.Format = DateTimeFormat.General;
                seriesXY.AxisY.Range.AlwaysShowZeroLevel = false;
            }
            seriesXY.AxisY.Title.Visible = true;
            if (dt.Rows.Count > 0)
            {
                xrChart1.DataSource = dt;
            }
            
        }

        private void xrChart1_BoundDataChanged(object sender, EventArgs e)
        {
            UI uiobj = new UI();
            uiobj.RecalibrateChartAxes(xrChart1.Diagram);
        }

    }
}
