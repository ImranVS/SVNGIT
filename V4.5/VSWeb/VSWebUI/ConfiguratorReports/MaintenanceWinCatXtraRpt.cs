using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;
using DevExpress.Utils;

namespace VSWebUI.ConfiguratorReports
{
    public partial class MaintenanceWinCatXtraRpt : DevExpress.XtraReports.UI.XtraReport
    {
        public MaintenanceWinCatXtraRpt()
        {
            InitializeComponent();
        }

        private void MaintenanceWinCatXtraRpt_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            DataTable dt = new DataTable();
            dt = VSWebBL.ReportsBL.ReportsBL.Ins.GetMaintWin();
            if (dt.Rows.Count > 0)
            {
                this.DataSource = dt;
                mwServerName.DataBindings.Add("Text", dt, "ServerName");
                mwStartDate.DataBindings.Add("Text", dt, "StartDate", "{0:MM/dd/yyyy}");
                mwStartTime.DataBindings.Add("Text", dt, "StartTime");
                mwDuration.DataBindings.Add("Text", dt, "Duration");
                mwEndDate.DataBindings.Add("Text", dt, "EndDate", "{0:MM/dd/yyyy}");
                mwMaintType.DataBindings.Add("Text", dt, "MaintType");
                mwMaintDaysList.DataBindings.Add("Text", dt, "MaintDaysList");
                xrLabel2.DataBindings.Add("Text", dt, "Name");
            }
        }

    }
}
