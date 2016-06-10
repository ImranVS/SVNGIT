using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;

namespace VSWebUI.ConfiguratorReports
{
    public partial class ServerTaskXtraRpt : DevExpress.XtraReports.UI.XtraReport
    {
        public ServerTaskXtraRpt()
        {
            InitializeComponent();
        }

        private void ServerTaskXtraRpt_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            //ServerTaskRptDSTableAdapters.ServerTasksTableAdapter servertaskAdapter = new ServerTaskRptDSTableAdapters.ServerTasksTableAdapter();
            //servertaskAdapter.Fill(this.serverTaskRptDS1.ServerTasks);
            DataTable dt = new DataTable();
            dt = VSWebBL.ReportsBL.ReportsBL.Ins.GetServerTasks();
            if (dt.Rows.Count > 0)
            {
                this.DataSource = dt;
                xrLabel2.DataBindings.Add("Text", dt, "ServerName");
                tlTaskName.DataBindings.Add("Text", dt, "TaskName");
                tlEnabledCheckBox.DataBindings.Add("Checked", dt, "Enabled");
                tlRestartOffHrsCheckBox.DataBindings.Add("Checked", dt, "RestartOffHours");
                tlRetryCount.DataBindings.Add("Text", dt, "RetryCount");
                tlMaxBusyTime.DataBindings.Add("Text", dt, "MaxBusyTime");
            }
        }

    }
}
