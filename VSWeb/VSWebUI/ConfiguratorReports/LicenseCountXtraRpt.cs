using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;
using VSWebBL;

namespace VSWebUI.ConfiguratorReports
{
    public partial class LicenseCountXtraRpt : DevExpress.XtraReports.UI.XtraReport
    {
        public LicenseCountXtraRpt()
        {
            InitializeComponent();
        }

        private void LicenseCountXtraRpt_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
           // LicenseCountRptDSTableAdapters.ServersTableAdapter serversAdapter = new LicenseCountRptDSTableAdapters.ServersTableAdapter();
           // serversAdapter.Fill(this.licenseCountRptDS1.Servers);
            DataTable dt = new DataTable();
            dt = VSWebBL.ReportsBL.XsdBL.Ins.LicenseCountServerBL();
            if (dt.Rows.Count > 0)
            {
                this.DataSource = dt;
                xrTableCell2.DataBindings.Add("Text", dt, "ServerName");
                xrTableCell6.DataBindings.Add("Text", dt, "ServerType");
                slServerName.DataBindings.Add("Text", dt, "ServerName");
                xrLabel1.DataBindings.Add("Text", dt, "Location");
            }

            DataTable dt2 = new DataTable();
            dt2 = VSWebBL.ReportsBL.XsdBL.Ins.LicenseCountSettingBL();
            if (dt2.Rows.Count > 0)
            {
                xrTableCell4.DataBindings.Add("Text", dt2, "svalue");
            }
            //LicenseCountRptDSTableAdapters.SettingsTableAdapter settingsAdapter = new LicenseCountRptDSTableAdapters.SettingsTableAdapter();
            //settingsAdapter.Fill(this.licenseCountRptDS1.Settings);
        }

    }
}
