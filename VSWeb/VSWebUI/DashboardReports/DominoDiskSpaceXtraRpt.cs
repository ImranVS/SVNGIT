using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;
using DevExpress.XtraCharts;

namespace VSWebUI.DashboardReports
{
    public partial class DominoDiskSpaceXtraRpt : DevExpress.XtraReports.UI.XtraReport
    {
        public DominoDiskSpaceXtraRpt()
        {
            InitializeComponent();
        }

        private void DiskSpaceChart_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            string sName = "";
            sName = "'" + ((XRLabel)this.FindControl("xrLabel2", true)).Text + "'";
            SetGraphForDiskSpace(sName, this);
        }

        public void SetGraphForDiskSpace(string serverName, DashboardReports.DominoDiskSpaceXtraRpt report)
        {
            DataTable dt = VSWebBL.DashboardBL.DominoServerDetailsBL.Ins.SetGraphForDiskSpace(serverName, "",
                this.ServerType.Value.ToString());
            DiskSpaceChart.Series[0].DataSource = dt;
            DiskSpaceChart.Series[0].ArgumentDataMember = dt.Columns["DiskName"].ToString();
            DiskSpaceChart.Series[0].ValueDataMembers.AddRange(dt.Columns["DiskUsed"].ToString());
            DiskSpaceChart.Series[0].Visible = true;
            DiskSpaceChart.Series[1].DataSource = dt;
            DiskSpaceChart.Series[1].ArgumentDataMember = dt.Columns["DiskName"].ToString();
            DiskSpaceChart.Series[1].ValueDataMembers.AddRange(dt.Columns["DiskFree"].ToString());
            DiskSpaceChart.Series[1].Visible = true;
        }

        private void DominoDiskSpaceXtraRpt_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            string servername = "";
            servername = this.ServerName.Value.ToString();
            string servertype = "";
            servertype = this.ServerType.Value.ToString();
            DataTable dt = VSWebBL.DashboardBL.DominoServerDetailsBL.Ins.SetGraphForDiskSpace(servername, "",servertype);     
            DataView view = new DataView(dt);
            DataTable distinctValues = view.ToTable(true, "ServerName");
            this.DataSource = distinctValues;
            ((XRLabel)this.FindControl("xrLabel2", true)).DataBindings.Add("Text", distinctValues, "ServerName");
        }

    }
}
