using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;

namespace VSWebUI.DashboardReports
{
    public partial class DominoDBDailyStatsXtraRpt : DevExpress.XtraReports.UI.XtraReport
    {
        public DominoDBDailyStatsXtraRpt()
        {
            InitializeComponent();
        }

        private void DominoDBDailyStatsXtraRpt_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            DataTable dt = new DataTable();
            dt = VSWebBL.ReportsBL.ReportsBL.Ins.GetDominoDBStats(this.ServerName.Value.ToString(),this.FolderName.Value.ToString());
            if (dt.Rows.Count > 0)
            {
                this.DataSource = dt;
                xrLabel2.DataBindings.Add("Text", dt, "Server");
                xrLabel3.DataBindings.Add("Text", dt, "Folder");
                mtTitle.DataBindings.Add("Text", dt, "Title");
                mtFileNamePath.DataBindings.Add("Text", dt, "FileNamePath");
                mtStatus.DataBindings.Add("Text", dt, "Status");
                mtTemplate.DataBindings.Add("Text", dt, "DesignTemplateName");
                //6/13/2014 NS commented out data bindings below for VSPLUS-727
                //mtDocCount.DataBindings.Add("Text", dt, "DocumentCount");
                //mtPercentUsed.DataBindings.Add("Text", dt, "PercentUsed");
                mtMailFile.DataBindings.Add("Checked", dt, "IsMailFile");
            }
        }
    }
}
