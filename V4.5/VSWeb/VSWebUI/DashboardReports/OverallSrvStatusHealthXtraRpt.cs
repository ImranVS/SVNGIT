using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;

namespace VSWebUI.DashboardReports
{
    public partial class OverallSrvStatusHealthXtraRpt : DevExpress.XtraReports.UI.XtraReport
    {
        public OverallSrvStatusHealthXtraRpt()
        {
            InitializeComponent();
        }

        private void OverallSrvStatusHealthXtraRpt_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            DataTable dt = new DataTable();
            dt = VSWebBL.ReportsBL.ReportsBL.Ins.GetOverallServerHealth();
            if (dt.Rows.Count > 0)
            {
                this.DataSource = dt;
                xrLabel2.DataBindings.Add("Text", dt, "Type");
                mtServerName.DataBindings.Add("Text", dt, "Name");
                mtStatus.DataBindings.Add("Text", dt, "Status");
                mtDetails.DataBindings.Add("Text", dt, "Details");
                mtDescription.DataBindings.Add("Text", dt, "Description");
                mtLocation.DataBindings.Add("Text", dt, "Location");
                mtPending.DataBindings.Add("Text", dt, "PendingMail");
                mtDead.DataBindings.Add("Text", dt, "DeadMail");
                mtHeld.DataBindings.Add("Text",dt,"HeldMail");
                mtCPU.DataBindings.Add("Text", dt, "CPU");
                //formattingRule1.DataSource = dt;
                //formattingRule2.DataSource = dt;
                //formattingRule3.DataSource = dt;
                //this.Detail.FormattingRules.Add(formattingRule1);
                //this.Detail.FormattingRules.Add(formattingRule2);
                //this.Detail.FormattingRules.Add(formattingRule3);
            }
        }

    }
}
