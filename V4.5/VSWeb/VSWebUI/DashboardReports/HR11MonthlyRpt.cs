using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;
using VSWebBL;

namespace VSWebUI.DashboardReports
{
    public partial class HR11MonthlyRpt : DevExpress.XtraReports.UI.XtraReport
    {
        public HR11MonthlyRpt()
        {
            InitializeComponent();
        }

        private void HR11MonthlyRpt_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
          //  HRMonthlyTableAdapters.MonthlyDeviceDailyStatsTableAdapter Monthly = new HRMonthlyTableAdapters.MonthlyDeviceDailyStatsTableAdapter();
           // Monthly.Fill(this.hrMonthly1.MonthlyDeviceDailyStats, this.MyDevice.Value.ToString(), (DateTime)this.StartDate.Value, (DateTime)this.EndDate.Value);
            DataTable dt = new DataTable();
            dt = VSWebBL.ReportsBL.XsdBL.Ins.hr11Monthly((DateTime)this.StartDate.Value,(DateTime)this.EndDate.Value);

            xrChart1.DataSource = dt;
        }

    }
}
