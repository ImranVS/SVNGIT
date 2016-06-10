using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;

namespace VSWebUI.DashboardReports
{
    public partial class DominoServerHealthStatusXtraRpt : DevExpress.XtraReports.UI.XtraReport
    {
        public DominoServerHealthStatusXtraRpt()
        {
            InitializeComponent();
        }

        private void DominoServerHealthStatusXtraRpt_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            DataTable dt = new DataTable();
            dt = VSWebBL.ReportsBL.ReportsBL.Ins.GetDominoServerHealth();
            if (dt.Rows.Count > 0)
            {
                this.DataSource = dt;
                xrLabel2.DataBindings.Add("Text", dt, "StatusCode");
                mtServerName.DataBindings.Add("Text", dt, "Name");
                mtLocation.DataBindings.Add("Text", dt, "Location");
                xrTableCell4.DataBindings.Add("Text", dt, "Details");
                mtResponseTime.DataBindings.Add("Text", dt, "ResponseTime");
                mtResponseThreshold.DataBindings.Add("Text", dt, "ResponseThreshold");
            }
        }

    }
}
