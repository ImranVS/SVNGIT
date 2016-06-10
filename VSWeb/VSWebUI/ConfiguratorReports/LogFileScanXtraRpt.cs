using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;

namespace VSWebUI.ConfiguratorReports
{
    public partial class LogFileScanXtraRpt : DevExpress.XtraReports.UI.XtraReport
    {
        public LogFileScanXtraRpt()
        {
            InitializeComponent();
        }

        private void LogFileScanXtraRpt_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            DataTable dt = new DataTable();
            dt = VSWebBL.ReportsBL.ReportsBL.Ins.GetLogFileData();
            if (dt.Rows.Count > 0)
            {
                this.DataSource = dt;
                lfsKeyword.DataBindings.Add("Text", dt, "Keyword");
                xrCheckBox1.DataBindings.Add("Checked", dt, "RepeatOnce");
            }
        }

    }
}
