using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;
using VSWebBL;

namespace VSWebUI.DashboardReports
{
    public partial class Hr6DailyDeviceRpt : DevExpress.XtraReports.UI.XtraReport
    {
        public Hr6DailyDeviceRpt()
        {
            InitializeComponent();
        }

        private void Hr6DailyDeviceRpt_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
           // Hr6DailyDevicestatsTableAdapters.DeviceDailyStatsTableAdapter hr6 = new Hr6DailyDevicestatsTableAdapters.DeviceDailyStatsTableAdapter();
           // hr6.Fill(this.hr6DailyDevicestats1.DeviceDailyStats, this.MyDevice.Value.ToString(), (DateTime)this.StartDate.Value, (DateTime)this.EndDate.Value);
            DataTable dt = new DataTable();
            dt = VSWebBL.ReportsBL.XsdBL.Ins.hr6BL(this.MyDevice.Value.ToString(), (DateTime)this.StartDate.Value, (DateTime)this.EndDate.Value);
            xrChart1.DataSource = dt;
        }

    }
}
