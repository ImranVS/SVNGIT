using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;

namespace VSWebUI.DashboardReports
{
    public partial class DominoServerHealthXtraRpt : DevExpress.XtraReports.UI.XtraReport
    {
        public DominoServerHealthXtraRpt()
        {
            InitializeComponent();
        }

        private void DominoServerHealthXtraRpt_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            DataTable dt = new DataTable();
            dt = VSWebBL.ReportsBL.ReportsBL.Ins.GetDominoServerHealth();
            if (dt.Rows.Count > 0)
            {
                this.DataSource = dt;
                xrLabel2.DataBindings.Add("Text", dt, "Location");
                mtServerName.DataBindings.Add("Text", dt, "Name");
                mtStatus.DataBindings.Add("Text", dt, "Status");
                mtDetails.DataBindings.Add("Text", dt, "Details");
                mtResponseTime.DataBindings.Add("Text", dt, "ResponseTime");
                mtResponseThreshold.DataBindings.Add("Text", dt, "ResponseThreshold");
            }
        }
    }
}
