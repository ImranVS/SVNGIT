using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;

namespace VSWebUI.DashboardReports
{
    public partial class DominoDBAvgSizeXtraRpt : DevExpress.XtraReports.UI.XtraReport
    {
        public DominoDBAvgSizeXtraRpt()
        {
            InitializeComponent();
        }

        private void DominoDBAvgSizeXtraRpt_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            DataTable dt = new DataTable();
            dt = VSWebBL.ReportsBL.ReportsBL.Ins.GetDominoDBAvgSize(this.ServerName.Value.ToString());
            if (dt.Rows.Count > 0)
            {
                this.DataSource = dt;
                GroupField gf = new GroupField("Server");
                this.GroupHeader2.GroupFields.Add(gf);
                xrLabel2.DataBindings.Add("Text", dt, "Server");
                tcFolder.DataBindings.Add("Text", dt, "Folder");
                tcTotalFiles.DataBindings.Add("Text", dt, "TotalFiles");
                tcTotalLogicalSize.DataBindings.Add("Text", dt, "TotalLogicalSize");
                tcAverageSize.DataBindings.Add("Text", dt, "AverageSize");
            }
        }

    }
}
