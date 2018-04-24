using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;
using VSWebBL;
namespace VSWebUI.DashboardReports
{// 6/6/2016 Durga Addded for  VSPLUS-2993
    public partial class O365MailboxstoragegrowthRpt : DevExpress.XtraReports.UI.XtraReport
    {
        public O365MailboxstoragegrowthRpt()
        {
            InitializeComponent();
        }

        private void AVGCPURpt_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
          
          DataTable dt=new DataTable();
          dt = VSWebBL.ReportsBL.XsdBL.Ins.GetO365MailboxData((DateTime)this.StartDate.Value,
              (DateTime)this.EndDate.Value,this.Threshold.Value.ToString());
            if(dt.Rows.Count>0)
          xrChart1.DataSource = dt;
        
        
           

        }


    }
}
