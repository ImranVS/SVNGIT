using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;

namespace VSWebUI.ConfiguratorReports
{
    public partial class ServerListTypeXtraRpt : DevExpress.XtraReports.UI.XtraReport
    {
        public ServerListTypeXtraRpt()
        {
            InitializeComponent();
        }

        private void ServerListTypeXtraRpt_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            //ServerListTypeRptDSTableAdapters.ServersTableAdapter serverAdapter = new ServerListTypeRptDSTableAdapters.ServersTableAdapter();
            //serverAdapter.Fill(this.serverListTypeRptDS1.Servers, this.ServerType.Value.ToString());
            DataTable dt = new DataTable();
            dt = VSWebBL.ReportsBL.ReportsBL.Ins.GetServerList(this.ServerType.Value.ToString(), "ServerType");
            if (dt.Rows.Count > 0)
            {
                this.DataSource = dt;
                xrLabel3.DataBindings.Add("Text", dt, "ServerType");
                slServerName.DataBindings.Add("Text", dt, "ServerName");
                slIP.DataBindings.Add("Text", dt, "IPAddress");
                slLocation.DataBindings.Add("Text", dt, "Location");
                //7/14/2014 NS added for VSPLUS-428
                slOS.DataBindings.Add("Text", dt, "OperatingSystem");
                slRelease.DataBindings.Add("Text", dt, "Release");
            }
        }

    }
}
