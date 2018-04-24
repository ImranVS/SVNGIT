using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;
using VSWebBL;
namespace VSWebUI.DashboardReports
{
    public partial class AVGCPURpt : DevExpress.XtraReports.UI.XtraReport
    {
        public AVGCPURpt()
        {
            InitializeComponent();
        }

        private void AVGCPURpt_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
          //  AvgCpuUtilTableAdapters.DominoSummaryStatsTableAdapter AVG = new AvgCpuUtilTableAdapters.DominoSummaryStatsTableAdapter();
           // AVG.Fill(this.avgCpuUtil1.DominoSummaryStats, this.ServerName.Value.ToString(), (DateTime)this.StartDate.Value, (DateTime)this.EndDate.Value);
          DataTable dt=new DataTable();
          dt=VSWebBL.ReportsBL.XsdBL.Ins.getdataBL(this.ServerName.Value.ToString(),(DateTime)this.StartDate.Value,
              (DateTime)this.EndDate.Value,this.Threshold.Value.ToString(),this.ServerType.Value.ToString());

          xrChart1.DataSource = dt;
          if (this.ServerType.Value.ToString() == "")
          {
              xrLabel1.Text = "Average CPU utilization per day";
          }
          else
          {
              xrLabel1.Text = "Traveler CPU utilization per day";
          }
          MonthYearLabel.Text = "Report for " + ((DateTime)this.StartDate.Value).ToShortDateString() + " - " + ((DateTime)this.EndDate.Value).ToShortDateString();
           

        }


    }
}
