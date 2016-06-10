using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;
using VSWebBL;
namespace VSWebUI.DashboardReports
{//2/1/2016 Durga Added for VSPLUS 2174
    public partial class TravelerCmemoryRpt : DevExpress.XtraReports.UI.XtraReport

    {
		public TravelerCmemoryRpt()
        {
            InitializeComponent();
        }

        private void AVGCPURpt_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
          
          DataTable dt=new DataTable();
          dt=VSWebBL.ReportsBL.XsdBL.Ins.GetTravlerData(this.ServerName.Value.ToString(),(DateTime)this.StartDate.Value,
			  (DateTime)this.EndDate.Value, this.Threshold.Value.ToString(), this.ServerType.Value.ToString(), "MemoryC");

          xrChart1.DataSource = dt;
          if (this.ServerType.Value.ToString() == "")
          {
			  xrLabel1.Text = "Average Traveler Allocated C memory per day";
          }
          else
          {
			  xrLabel1.Text = "Traveler Allocated C memory";
          }
          MonthYearLabel.Text = "Report for " + ((DateTime)this.StartDate.Value).ToShortDateString() + " - " + ((DateTime)this.EndDate.Value).ToShortDateString();
           

        }


    }
}
