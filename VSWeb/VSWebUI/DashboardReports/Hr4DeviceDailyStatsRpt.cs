using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;
using VSWebBL;
namespace VSWebUI.DashboardReports
{
    public partial class Hr4DeviceDailyStatsRpt : DevExpress.XtraReports.UI.XtraReport
    {
        public Hr4DeviceDailyStatsRpt()
        {
            InitializeComponent();
        }

        private void Hr4DeviceDailyStatsRpt_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
           // HR4RptTableAdapters.DeviceDailyStatsTableAdapter hr4 = new HR4RptTableAdapters.DeviceDailyStatsTableAdapter();
            //hr4DeviceDailyStatsTableAdapters.DeviceDailyStatsTableAdapter hr4 = new hr4DeviceDailyStatsTableAdapters.DeviceDailyStatsTableAdapter();
            //hr4.Fill(this.hr4DeviceDailyStats1.DeviceDailyStats, this.MyDevice.Value.ToString(),(DateTime)this.StartDate.Value,(DateTime) this.EndDate.Value);
           // hr4.Fill(this.hR4Rpt1.DeviceDailyStats, this.MyDevice.Value.ToString(), (DateTime)this.StartDate.Value, (DateTime)this.EndDate.Value);

            DataTable dt = new DataTable();
            dt = VSWebBL.ReportsBL.XsdBL.Ins.hr4BL(this.MyDevice.Value.ToString(), (DateTime)this.StartDate.Value, (DateTime)this.EndDate.Value);
            xrChart1.DataSource = dt;
        }

    }
}
