using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;

namespace VSWebUI.ConfiguratorReports
{
    public partial class MaintenanceWinXtraRpt : DevExpress.XtraReports.UI.XtraReport
    {
        public MaintenanceWinXtraRpt()
        {
            InitializeComponent();
        }

        private void MaintenanceWinXtraRpt_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            DataTable dt = new DataTable();
            dt = VSWebBL.ReportsBL.ReportsBL.Ins.GetMaintWin();
            if (dt.Rows.Count > 0)
            {
                this.DataSource = dt;
                mwStartDate.DataBindings.Add("Text", dt, "StartDate", "{0:MM/dd/yyyy}");
                mwStartTime.DataBindings.Add("Text", dt, "StartTime");
                mwDuration.DataBindings.Add("Text", dt, "Duration");
                mwEndDate.DataBindings.Add("Text", dt, "EndDate", "{0:MM/dd/yyyy}");
                mwMaintType.DataBindings.Add("Text", dt, "MaintType");
                mwMaintDaysList.DataBindings.Add("Text", dt, "MaintDaysList");
                mwName.DataBindings.Add("Text", dt, "Name");
                mwServerName.DataBindings.Add("Text", dt, "ServerName");
            }
        }

    }
}
