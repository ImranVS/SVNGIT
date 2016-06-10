using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using DevExpress.XtraCharts;
using System.Data;
using VSWebBL;
namespace VSWebUI.DashboardReports
{
    public partial class EXJournalDocumentTotalsRpt : DevExpress.XtraReports.UI.XtraReport
    { //22/4/2016 Durga added for  VSPLUS-2806
        public EXJournalDocumentTotalsRpt()
        {
            InitializeComponent();
        }

        private void EXJournalDocumentTotalsRpt_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            XYDiagram xdiag = (XYDiagram)this.xrChart1.Diagram;

          
            DataTable dt = new DataTable();
            dt = VSWebBL.ReportsBL.XsdBL.Ins.GetDominoDailyStatsInfo(this.ServerName.Value.ToString(),
                this.Threshold.Value.ToString(), "EXJournal.DocCount.Total");
            if(dt.Rows.Count>0)
            xrChart1.DataSource = dt;

         


        }


    }
}
