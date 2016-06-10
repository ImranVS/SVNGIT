using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;
using VSWebBL;

namespace VSWebUI.DashboardReports
{
    public partial class Hrs2NotesMail : DevExpress.XtraReports.UI.XtraReport
    {
        public Hrs2NotesMail()
        {
            InitializeComponent();
        }

        private void Hrs2NotesMail_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
           // HR2RptTableAdapters.NotesMailStatsTableAdapter note = new HR2RptTableAdapters.NotesMailStatsTableAdapter();
           // Hrs2TableAdapters.NotesMailStatsTableAdapter note = new Hrs2TableAdapters.NotesMailStatsTableAdapter();
           // note.Fill(this.hR2Rpt1.NotesMailStats, this.MyDevice.Value.ToString(),(DateTime)this.StartDate.Value,(DateTime) this.EndDate.Value);
            DataTable dt = new DataTable();
            dt = VSWebBL.ReportsBL.XsdBL.Ins.hr2BL(this.MyDevice.Value.ToString(), (DateTime)this.StartDate.Value, (DateTime)this.EndDate.Value);
            xrChart1.DataSource = dt;
           
        }




    }
}
