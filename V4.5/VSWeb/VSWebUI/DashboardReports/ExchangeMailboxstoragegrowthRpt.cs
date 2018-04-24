using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;
using VSWebBL;
namespace VSWebUI.DashboardReports
{// 6/6/2016 Durga Addded for  VSPLUS-2993
    public partial class ExchangeMailboxstoragegrowthRpt : DevExpress.XtraReports.UI.XtraReport
    {
        public ExchangeMailboxstoragegrowthRpt()
        {
            InitializeComponent();
        }

        private void AVGCPURpt_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
          
          DataTable dt=new DataTable();
          if (this.Type.Value.ToString() == "Databases")
          dt = VSWebBL.ReportsBL.XsdBL.Ins.GetExchangeMailboxData(this.DocumentName.Value.ToString(), (DateTime)this.StartDate.Value,
              (DateTime)this.EndDate.Value,this.Threshold.Value.ToString());
            else
              dt = VSWebBL.ReportsBL.XsdBL.Ins.GetExchangeServerData(this.DocumentName.Value.ToString(), (DateTime)this.StartDate.Value,
              (DateTime)this.EndDate.Value,this.Threshold.Value.ToString());
            if(dt.Rows.Count>0)
          xrChart1.DataSource = dt;
        
        
           

        }


    }
}
