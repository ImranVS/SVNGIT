using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace VSWebUI.DashboardReports
{
    public partial class DominoDiskXtraRpt : DevExpress.XtraReports.UI.XtraReport
    {
        public DominoDiskXtraRpt()
        {
            InitializeComponent();
        }

        private void DominoDiskXtraRpt_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            DominoDiskRptDSTableAdapters.DominoDailyStatsTableAdapter dailyAdapter = new DominoDiskRptDSTableAdapters.DominoDailyStatsTableAdapter();
            dailyAdapter.Fill(this.dominoDiskRptDS1.DominoDailyStats, (int)this.DateM.Value, (int)this.DateY.Value, this.ServerName.Value.ToString());
        }

    }
}
