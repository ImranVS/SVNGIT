using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;
using VSWebBL;

namespace VSWebUI.DashboardReports
{
    public partial class hr8DeviceDailyStatsforweeklyRpt : DevExpress.XtraReports.UI.XtraReport
    {
        public hr8DeviceDailyStatsforweeklyRpt()
        {
            InitializeComponent();
        }

        private void hr8DeviceDailyStatsforweeklyRpt_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
           // hr8DeviceDailyStatsforweeklyTableAdapters.DeviceDailyStatsTableAdapter hr8 = new hr8DeviceDailyStatsforweeklyTableAdapters.DeviceDailyStatsTableAdapter();
           // hr8.Fill(this.hr8DeviceDailyStatsforweekly1.DeviceDailyStats, this.MyDevice.Value.ToString(), (DateTime)this.StartDate.Value, (DateTime)this.EndDate.Value);
            DataTable dt = new DataTable();
            dt = VSWebBL.ReportsBL.XsdBL.Ins.hr8BL(this.MyDevice.Value.ToString(), (DateTime)this.StartDate.Value, (DateTime)this.EndDate.Value);
            xrChart1.DataSource = dt;

        }

    }
}
