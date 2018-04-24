using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;

namespace VSWebUI.ConfiguratorReports
{
    public partial class NotesDBsXtraRpt : DevExpress.XtraReports.UI.XtraReport
    {
        public NotesDBsXtraRpt()
        {
            InitializeComponent();
        }

        private void NotesDBsXtraRpt_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            DataTable dt = new DataTable();
            dt = VSWebBL.ReportsBL.ReportsBL.Ins.GetNotesDBs();
            if (dt.Rows.Count > 0)
            {
                this.DataSource = dt;
                dblTaskName.DataBindings.Add("Text", dt, "Name");
                dblFileName.DataBindings.Add("Text", dt, "FileName");
                dblServerName.DataBindings.Add("Text", dt, "ServerName");
                dblCategory.DataBindings.Add("Text", dt, "Category");
                dblScanInterval.DataBindings.Add("Text", dt, "ScanInterval");
                dblOffHrsInterval.DataBindings.Add("Text", dt, "OffHoursScanInterval");
                dblEnabled.DataBindings.Add("Text", dt, "Enabled");
                dblResponseThreshold.DataBindings.Add("Text", dt, "ResponseThreshold");
                dblRetryInterval.DataBindings.Add("Text", dt, "RetryInterval");
            }
        }

    }
}
