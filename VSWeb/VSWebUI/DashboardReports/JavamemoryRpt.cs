using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;
using VSWebBL;
namespace VSWebUI.DashboardReports
{
    public partial class JavamemoryRpt : DevExpress.XtraReports.UI.XtraReport

    {
		public JavamemoryRpt()
        {
            InitializeComponent();
        }

        private void AVGCPURpt_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
        
          DataTable dt=new DataTable();
          dt=VSWebBL.ReportsBL.XsdBL.Ins.GetTravlerData(this.ServerName.Value.ToString(),(DateTime)this.StartDate.Value,
              (DateTime)this.EndDate.Value,this.Threshold.Value.ToString(),this.ServerType.Value.ToString(),"MemoryJava");

          xrChart1.DataSource = dt;
          if (this.ServerType.Value.ToString() == "")
          {
			  xrLabel1.Text = "Average Allocated Java memory per day";
          }
          else
          {
			  xrLabel1.Text = "Traveler Allocated Java memory";
          }
          MonthYearLabel.Text = "Report for " + ((DateTime)this.StartDate.Value).ToShortDateString() + " - " + ((DateTime)this.EndDate.Value).ToShortDateString();
           

        }


    }
}
