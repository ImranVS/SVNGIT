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
    public partial class TravelerStatsXtraRpt : DevExpress.XtraReports.UI.XtraReport
    {
        public TravelerStatsXtraRpt()
        {
            InitializeComponent();
        }

        private void TravelerStatsXtraRpt_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            string interval = "";
            string servername = "";
            //TravelerStatsRptDSTableAdapters.TravelerStatsTableAdapter travelerAdapter = new TravelerStatsRptDSTableAdapters.TravelerStatsTableAdapter();
            //travelerAdapter.Fill(this.travelerStatsRptDS1.TravelerStats, this.Parameters["Interval"].Value.ToString(), this.Parameters["ServerName"].Value.ToString());
            if (this.Parameters["Interval"].Value.ToString() == "")
            {
                this.IntervalLabel.Text = "";
            }
            else
            {
                interval = this.Parameters["Interval"].Value.ToString();
                servername = this.Parameters["ServerName"].Value.ToString();
                //5/21/2015 NS modified for VSPLUS-1791
                if (interval.Substring(4, 3).ToUpper() == "INF")
                {
                    this.IntervalLabel.Text = "Traveler server " + servername + " taking " + Convert.ToInt32(interval.Substring(0, 3)) + " - " + "infinity seconds to complete";
                }
                else
                {
                    this.IntervalLabel.Text = "Traveler server " + servername + " taking " + Convert.ToInt32(interval.Substring(0, 3)) + " - " + Convert.ToInt32(interval.Substring(4, 3)) + " seconds to complete";
                }
            }
        }

        private void xrChart1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            DataTable dt = new DataTable();
            dt = VSWebBL.ReportsBL.ReportsBL.Ins.GetTravelerStats(this.Parameters["ServerName"].Value.ToString(), this.Parameters["Interval"].Value.ToString());
            
            xrChart1.SeriesDataMember = "MailServerName";
            xrChart1.SeriesTemplate.ArgumentDataMember = "DateUpdated";
            xrChart1.SeriesTemplate.ValueDataMembers.AddRange(new string[] { "OpenTimes" });
            xrChart1.SeriesTemplate.View = new SideBySideBarSeriesView();
            xrChart1.SeriesTemplate.ChangeView(ViewType.Line);
            LineSeriesView view = (LineSeriesView)xrChart1.SeriesTemplate.View;
            view.LineMarkerOptions.Visible = true;
            view.LineMarkerOptions.Kind = MarkerKind.Diamond;
            view.LineMarkerOptions.Size = 8;
            xrChart1.Legend.Visible = true;

            XYDiagram seriesXY = (XYDiagram)xrChart1.Diagram;
            seriesXY.AxisX.Title.Text = "Time";
            seriesXY.AxisX.Title.Visible = true;
            seriesXY.AxisX.Label.ResolveOverlappingOptions.AllowRotate = true;
            seriesXY.AxisX.Label.ResolveOverlappingOptions.AllowHide = false;
            seriesXY.AxisY.NumericOptions.Format = NumericFormat.Number;
            seriesXY.AxisY.NumericOptions.Precision = 0;
            //4/18/2014 NS commented out for VSPLUS-312
            //seriesXY.AxisX.DateTimeGridAlignment = DateTimeMeasurementUnit.Minute;
            seriesXY.AxisX.DateTimeMeasureUnit = DateTimeMeasurementUnit.Minute;
            seriesXY.AxisX.DateTimeOptions.Format = DateTimeFormat.ShortTime;
            seriesXY.AxisY.Range.AlwaysShowZeroLevel = false;
            seriesXY.AxisY.Title.Text = "Open Times";
            seriesXY.AxisY.Title.Visible = true;
            if (dt.Rows.Count > 0)
            {
                xrChart1.DataSource = dt;
            }
        }

        private void xrChart1_BoundDataChanged(object sender, EventArgs e)
        {
            if ((XYDiagram)xrChart1.Diagram != null)
            {
                XYDiagram seriesXY = (XYDiagram)xrChart1.Diagram;
                double min = Convert.ToDouble(((XYDiagram)xrChart1.Diagram).AxisY.Range.MinValue);
                double max = Convert.ToDouble(((XYDiagram)xrChart1.Diagram).AxisY.Range.MaxValue);

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

    }
}
