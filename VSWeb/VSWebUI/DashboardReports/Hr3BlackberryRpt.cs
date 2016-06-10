using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;
using VSWebUI;

namespace VSWebUI.DashboardReports
{
    public partial class hr3BlackBerryRpt : DevExpress.XtraReports.UI.XtraReport
    {
        public hr3BlackBerryRpt()
        {
            InitializeComponent();
        }

        private void hr3BlackBerryRpt_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
          //  BlackBerryTableAdapters.BlackBerryProbeStatsTableAdapter BB = new BlackBerryTableAdapters.BlackBerryProbeStatsTableAdapter();
           // BB.Fill(blackBerry1.BlackBerryProbeStats, this.MyDevice.Value.ToString(), (DateTime)this.StartDate.Value, (DateTime)this.EndDate.Value);
           // Hr3BlackBerryTableAdapters.BlackBerryProbeStatsTableAdapter hr3 = new Hr3BlackBerryTableAdapters.BlackBerryProbeStatsTableAdapter();
            //hr3.Fill(this.hr3BlackBerry1.BlackBerryProbeStats, this.MyDevice.Value.ToString(), (DateTime)this.StartDate.Value, (DateTime)this.EndDate.Value);

            DataTable dt = new DataTable();
            dt = VSWebBL.ReportsBL.XsdBL.Ins.Hr3RptBL(this.MyDevice.Value.ToString(),(DateTime)this.StartDate.Value,(DateTime)this.EndDate.Value);
            xrChart1.DataSource = dt;

        }

    }
}
