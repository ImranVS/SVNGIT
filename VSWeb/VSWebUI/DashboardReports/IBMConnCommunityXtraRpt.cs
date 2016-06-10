using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;

namespace VSWebUI.DashboardReports
{
    public partial class IBMConnCommunityXtraRpt : DevExpress.XtraReports.UI.XtraReport
    {
        public IBMConnCommunityXtraRpt()
        {
            InitializeComponent();
        }


        private void IBMConnCommunityXtraRpt_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            DataTable dt = new DataTable();
            dt = VSWebBL.ReportsBL.ReportsBL.Ins.GetCommList(this.Name.Value.ToString());
            if (dt.Rows.Count > 0)
            {
                this.DataSource = dt;
                xrLabel2.DataBindings.Add("Text", dt, "Name");
                ctusers.DataBindings.Add("Text", dt, "Users");
                ctowner.DataBindings.Add("Text", dt, "Owners");
            }
        }
    }
}
