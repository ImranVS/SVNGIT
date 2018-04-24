using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;
using VSWebBL;

namespace VSWebUI.DashboardReports
{
    public partial class hr9NotesMailTimeAvarageRpt : DevExpress.XtraReports.UI.XtraReport
    {
        public hr9NotesMailTimeAvarageRpt()
        {
            InitializeComponent();
        }

        private void hr9NotesMailTimeAvarageRpt_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            //hr9NotesMailforTimeAverageTableAdapters.DeviceDailyStatsTableAdapter hr9 = new hr9NotesMailforTimeAverageTableAdapters.DeviceDailyStatsTableAdapter();
           // hr9.Fill(this.hr9NotesMailforTimeAverage1.DeviceDailyStats, this.MyDevice.Value.ToString(),(DateTime) this.StartDate.Value,(DateTime) this.EndDate.Value);
            DataTable dt = new DataTable();
            dt = VSWebBL.ReportsBL.XsdBL.Ins.hr9BL(this.MyDevice.Value.ToString(), (DateTime)this.StartDate.Value, (DateTime)this.EndDate.Value);
            xrChart1.DataSource = dt;

        }

    }
}
