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
    public partial class TravelerStatsSrvXtraRpt : DevExpress.XtraReports.UI.XtraReport
    {
        public TravelerStatsSrvXtraRpt()
        {
            InitializeComponent();
        }

        private void TravelerStatsSrvXtraRpt_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            string servername = "";
            string travelername = "";
            //TravelerStatsRptDSTableAdapters.TravelerStats1TableAdapter travelerAdapter = new TravelerStatsRptDSTableAdapters.TravelerStats1TableAdapter();
            //travelerAdapter.Fill(this.travelerStatsRptDS1.TravelerStats1, this.Parameters["ServerName"].Value.ToString(), this.Parameters["TravelerName"].Value.ToString());
            if (this.Parameters["ServerName"].Value.ToString() == "")
            {
                this.ServerNameLabel.Text = "";
            }
            else
            {
                servername = this.Parameters["ServerName"].Value.ToString();
                travelername = this.Parameters["TravelerName"].Value.ToString();
                this.ServerNameLabel.Text = "The traveler server " + travelername + " is accessing files on server " + servername;
            }
        }

        private void xrChart1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            DataTable dt = new DataTable();
            dt = VSWebBL.ReportsBL.ReportsBL.Ins.GetTravelerStatsSrv(this.Parameters["ServerName"].Value.ToString(), this.Parameters["TravelerName"].Value.ToString());
            xrChart1.SeriesDataMember = "Interval";
            xrChart1.SeriesTemplate.ArgumentDataMember = "DateUpdated";
            xrChart1.SeriesTemplate.ValueDataMembers.AddRange(new string[] { "OpenTimes" });
            xrChart1.SeriesTemplate.View = new SideBySideBarSeriesView();
            xrChart1.SeriesTemplate.ChangeView(ViewType.Line);
            LineSeriesView view = (LineSeriesView)xrChart1.SeriesTemplate.View;
            view.LineMarkerOptions.Visible = false;
            xrChart1.Legend.Visible = true;

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
