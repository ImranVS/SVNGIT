using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;
using DevExpress.XtraCharts;

namespace VSWebUI.DashboardReports
{
    public partial class SametimeStatsChartXtraRpt : DevExpress.XtraReports.UI.XtraReport
    {
        public SametimeStatsChartXtraRpt()
        {
            InitializeComponent();
        }

        private void xrChart1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            DataTable dt;
            xrChart1.Series.Clear();
            dt = VSWebBL.DashboardBL.SametimeHealthBL.Ins.GetSametimeSummaryStats(this.StartDate.Value.ToString(),this.EndDate.Value.ToString(),this.StatName.Value.ToString());
            if (dt.Rows.Count > 0)
            {
                xrChart1.SeriesDataMember = dt.Columns["ServerName"].ToString();
                xrChart1.SeriesTemplate.ArgumentDataMember = "Date";
                xrChart1.SeriesTemplate.ValueDataMembers.AddRange(new string[] { dt.Columns["StatValue"].ToString() });
                AxisBase axisy = ((XYDiagram)xrChart1.Diagram).AxisY;
                //4/18/2014 NS modified for VSPLUS-312
                axisy.Range.AlwaysShowZeroLevel = true;
                xrChart1.DataSource = dt;
            }
        }

        private void SametimeStatsChartXtraRpt_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            xrLabel1.Text += " " + this.StatName.Value.ToString();
        }
        //7/9/2015 NS added for VSPLUS-1973
        private void xrChart1_BoundDataChanged(object sender, EventArgs e)
        {
            UI uiobj = new UI();
            uiobj.RecalibrateChartAxes(xrChart1.Diagram);
        }

    }
}
