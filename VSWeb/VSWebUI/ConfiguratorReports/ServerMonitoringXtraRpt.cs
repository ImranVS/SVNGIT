using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;
using VSWebBL;

namespace VSWebUI.ConfiguratorReports
{
    public partial class ServerMonitoringXtraRpt : DevExpress.XtraReports.UI.XtraReport
    {
        public ServerMonitoringXtraRpt()
        {
            InitializeComponent();
        }

        private void ServerMonitoringXtraRpt_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
           // ServerMonitoringRptDSTableAdapters.ServersTableAdapter serversAdapter = new ServerMonitoringRptDSTableAdapters.ServersTableAdapter();
           // serversAdapter.Fill(this.ServerMonitoringRptDS1.Servers);
            DataTable dt = new DataTable();
            dt = VSWebBL.ReportsBL.XsdBL.Ins.LicenseCountServerTypesBL();
            if (dt.Rows.Count > 0)
            {
                this.DataSource = dt;
                slUsedLicenses.DataBindings.Add("Text", dt, "UsedLicenses");
                slTotalLicenses.DataBindings.Add("Text", dt, "TotalLicenses");
                xrLabel1.DataBindings.Add("Text", dt, "ServerType");
            }

            //DataTable dt2 = new DataTable();
            //dt2 = VSWebBL.ReportsBL.XsdBL.Ins.LicenseCountSettingBL();
            //if (dt2.Rows.Count > 0)
            //{
            //    xrTableCell4.DataBindings.Add("Text", dt2, "svalue");
            //}
            //LicenseCountRptDSTableAdapters.SettingsTableAdapter settingsAdapter = new LicenseCountRptDSTableAdapters.SettingsTableAdapter();
            //settingsAdapter.Fill(this.licenseCountRptDS1.Settings);
        }

    }
}
