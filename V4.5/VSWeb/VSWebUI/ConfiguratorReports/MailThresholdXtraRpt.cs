using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;

namespace VSWebUI.ConfiguratorReports
{
    public partial class MailThresholdXtraRpt : DevExpress.XtraReports.UI.XtraReport
    {
        public MailThresholdXtraRpt()
        {
            InitializeComponent();
        }

        private void MailThresholdXtraRpt_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            DataTable dt = new DataTable();
            dt = VSWebBL.ReportsBL.ReportsBL.Ins.GetMailThreshold();
            if (dt.Rows.Count > 0)
            {
                this.DataSource = dt;
                mtServerName.DataBindings.Add("Text", dt, "ServerName");
                mtDeadThreshold.DataBindings.Add("Text", dt, "DeadThreshold");
                mtPendingThreshold.DataBindings.Add("Text", dt, "PendingThreshold");
                mtHeldMailThreshold.DataBindings.Add("Text", dt, "HeldMailThreshold");
              
            }
        }

    }
}
