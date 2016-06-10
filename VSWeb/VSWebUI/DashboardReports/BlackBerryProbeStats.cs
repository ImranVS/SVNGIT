using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using VSWebBL;
using System.Data;

namespace VSWebUI.DashboardReports
{
    public partial class BlackBerryProbeStats : DevExpress.XtraReports.UI.XtraReport
    {
        public BlackBerryProbeStats()
        {
            InitializeComponent();
        }

        private void BlackBerryProbeStats_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
          //  BlackBerryTableAdapters.BlackBerryProbeStatsTableAdapter black = new BlackBerryTableAdapters.BlackBerryProbeStatsTableAdapter();
           
           // black.Fill(this.blackBerry1.BlackBerryProbeStats,this.MyDevice.Value.ToString(),(DateTime)this.StartDate.Value,(DateTime)this.EndDate.Value) ;

            DataTable dt = new DataTable();

            dt = VSWebBL.ReportsBL.XsdBL.Ins.getBlackBerryBL(this.MyDevice.Value.ToString(), (DateTime)this.StartDate.Value, (DateTime)this.EndDate.Value);
            xrChart1.DataSource = dt;
  
        }

       

    }
}
