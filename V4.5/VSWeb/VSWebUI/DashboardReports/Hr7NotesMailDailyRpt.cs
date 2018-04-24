using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;
using VSWebBL;

namespace VSWebUI.DashboardReports
{
    public partial class Hr7NotesMailDailyRpt : DevExpress.XtraReports.UI.XtraReport
    {
        public Hr7NotesMailDailyRpt()
        {
            InitializeComponent();
        }

        private void Hr7NotesMailDailyRpt_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
          //  Hr7NotesMailTableAdapters.DeviceDailyStatsTableAdapter hr7 = new Hr7NotesMailTableAdapters.DeviceDailyStatsTableAdapter();
           // hr7.Fill(this.hr7NotesMail1.DeviceDailyStats, this.MyDevice.Value.ToString(), (DateTime)this.StartDate.Value, (DateTime)this.EndDate.Value);
            DataTable dt = new DataTable();
            dt = VSWebBL.ReportsBL.XsdBL.Ins.hr7BL(this.MyDevice.Value.ToString(), (DateTime)this.StartDate.Value, (DateTime)this.EndDate.Value);
            xrChart1.DataSource = dt;
        
        }

    }
}
