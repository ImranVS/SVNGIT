using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using DevExpress.XtraCharts;
using System.Data;

namespace VSWebUI.DashboardReports
{
    public partial class TravelerStatsHTTPXtraRpt : DevExpress.XtraReports.UI.XtraReport
    {
        public TravelerStatsHTTPXtraRpt()
        {
            InitializeComponent();
        }

        private void xrChart1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            DataTable dt = new DataTable();
            dt = VSWebBL.ReportsBL.ReportsBL.Ins.GetTravelerHTTPSessions(this.Parameters["TravelerName"].Value.ToString());
            if (dt.Rows.Count > 0)
            {
                xrChart1.DataSource = dt;
            }
            Series series = new Series("HttpSessions", ViewType.Line);
            series.Visible = true;
            series.ArgumentDataMember = dt.Columns["Date"].ToString();
            ValueDataMemberCollection seriesValueDataMembers = (ValueDataMemberCollection)series.ValueDataMembers;
            seriesValueDataMembers.AddRange(dt.Columns["StatValue"].ToString());
            series.LabelsVisibility = DevExpress.Utils.DefaultBoolean.False;

            xrChart1.Series.Add(series);
            xrChart1.SeriesTemplate.View = new SideBySideBarSeriesView();
            xrChart1.SeriesTemplate.ChangeView(ViewType.Line);
            LineSeriesView view = (LineSeriesView)xrChart1.SeriesTemplate.View;
            view.LineMarkerOptions.Visible = true;
            view.LineMarkerOptions.Size = 8;
            xrChart1.Legend.Visible = false;

            XYDiagram seriesXY = (XYDiagram)xrChart1.Diagram;
            seriesXY.AxisX.Title.Text = "Time";
            seriesXY.AxisX.Title.Visible = true;
            seriesXY.AxisX.Label.ResolveOverlappingOptions.AllowRotate = true;
            seriesXY.AxisX.Label.ResolveOverlappingOptions.AllowHide = false;
            //4/18/2014 NS commented out for VSPLUS-312
            //seriesXY.AxisX.DateTimeGridAlignment = DateTimeMeasurementUnit.Minute;
            seriesXY.AxisX.DateTimeMeasureUnit = DateTimeMeasurementUnit.Minute;
            seriesXY.AxisX.DateTimeOptions.Format = DateTimeFormat.ShortTime;
            seriesXY.AxisY.Range.AlwaysShowZeroLevel = false;
            seriesXY.AxisY.Title.Text = "Sessions";
            seriesXY.AxisY.Title.Visible = true;

            double min = Convert.ToDouble(((XYDiagram)xrChart1.Diagram).AxisY.Range.MinValue);
            double max = Convert.ToDouble(((XYDiagram)xrChart1.Diagram).AxisY.Range.MaxValue);

            int gs = (int)((max - min) / 5);

            if (gs == 0)
            {
                gs = 1;
                seriesXY.AxisY.GridSpacingAuto = false;
                seriesXY.AxisY.GridSpacing = gs;
            }
        }

        private void TravelerStatsHTTPXtraRpt_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            string travelername = "";
            travelername = this.Parameters["TravelerName"].Value.ToString();
            this.ServerNameLabel.Text = "Traveler server - " + travelername;
        }

    }
}
