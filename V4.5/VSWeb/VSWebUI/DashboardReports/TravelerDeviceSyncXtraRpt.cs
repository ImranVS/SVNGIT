using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Data;
using DevExpress.XtraReports.UI;
using DevExpress.XtraCharts;

namespace VSWebUI.DashboardReports
{
    public partial class TravelerDeviceSyncXtraRpt : DevExpress.XtraReports.UI.XtraReport
    {
        public TravelerDeviceSyncXtraRpt()
        {
            InitializeComponent();
        }

        private void xrChart1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            xrChart1.Series.Clear();
            DataTable dt = new DataTable();
            dt = VSWebBL.ReportsBL.ReportsBL.Ins.GetTravelerDeviceSyncs(this.Parameters["ServerNameSQL"].Value.ToString(),this.Parameters["StartDate"].Value.ToString(), this.Parameters["EndDate"].Value.ToString());
            if (dt.Rows.Count > 0)
            {
                xrChart1.DataSource = dt;
            }
            xrChart1.SeriesDataMember = "ServerName";
            xrChart1.SeriesTemplate.ArgumentDataMember = "Date";
            xrChart1.SeriesTemplate.ValueDataMembers.AddRange(new string[] { "StatValue" });
            LineSeriesView view = (LineSeriesView)xrChart1.SeriesTemplate.View;
            view.LineMarkerOptions.Visible = true;
            view.LineMarkerOptions.Size = 8;
            xrChart1.Legend.Visible = true;
            xrChart1.BackColor = System.Drawing.Color.Transparent;

            XYDiagram seriesXY = (XYDiagram)xrChart1.Diagram;
            seriesXY.AxisX.Title.Visible = true;
            seriesXY.AxisX.Label.ResolveOverlappingOptions.AllowRotate = true;
            seriesXY.AxisX.Label.ResolveOverlappingOptions.AllowHide = false;
            seriesXY.AxisX.Label.ResolveOverlappingOptions.AllowStagger = false;
            seriesXY.AxisX.Title.Text = "Date";
            
            seriesXY.AxisY.Title.Text = "Device Sync Volume";
            seriesXY.AxisX.DateTimeMeasureUnit = DateTimeMeasurementUnit.Day;
            seriesXY.AxisX.DateTimeOptions.Format = DateTimeFormat.ShortDate;
            seriesXY.AxisY.Range.AlwaysShowZeroLevel = false;
            seriesXY.AxisY.Title.Visible = true;
            //9/2/2015 NS commented out
            /*
            double min = Convert.ToDouble(((XYDiagram)xrChart1.Diagram).AxisY.Range.MinValue);
            double max = Convert.ToDouble(((XYDiagram)xrChart1.Diagram).AxisY.Range.MaxValue);

            int gs = (int)((max - min) / 5);

            if (gs == 0)
            {
                gs = 1;
                seriesXY.AxisY.GridSpacingAuto = false;
                seriesXY.AxisY.GridSpacing = gs;
            }
             */
        }

        private void TravelerDeviceSyncXtraRpt_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            this.BackColor = System.Drawing.Color.Transparent;
        }

        //9/2/2015 NS added
        private void xrChart1_BoundDataChanged(object sender, EventArgs e)
        {
            UI uiobj = new UI();
            uiobj.RecalibrateChartAxes(xrChart1.Diagram);
        }

    }
}
