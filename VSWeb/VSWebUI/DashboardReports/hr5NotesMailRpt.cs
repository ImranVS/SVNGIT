using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;
using VSWebBL;
namespace VSWebUI.DashboardReports
{
    public partial class hr5NotesMailRpt : DevExpress.XtraReports.UI.XtraReport
    {
        public hr5NotesMailRpt()
        {
            InitializeComponent();
        }

        private void hr5NotesMailRpt_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
           // hr5NotesMailfortwodaysTableAdapters.NotesMailStatsTableAdapter hr5 = new hr5NotesMailfortwodaysTableAdapters.NotesMailStatsTableAdapter();
           // hr5.Fill(this.hr5NotesMailfortwodays1.NotesMailStats, this.MyDevice.Value.ToString(), (DateTime)this.StartDate.Value, (DateTime)this.EndDate.Value);
            DataTable dt = new DataTable();
            dt = VSWebBL.ReportsBL.XsdBL.Ins.hr5BL(this.MyDevice.Value.ToString(), (DateTime)this.StartDate.Value,(DateTime) this.EndDate.Value);
            xrChart1.DataSource = dt;
        }

    }
}
