using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;

namespace VSWebUI.ConfiguratorReports
{
    public partial class ConfigUserListXtraRpt : DevExpress.XtraReports.UI.XtraReport
    {
        public ConfigUserListXtraRpt()
        {
            InitializeComponent();
        }

        private void ConfigUserListXtraRpt_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            string seluser = "";
            seluser = this.UserName.Value.ToString();
            DataTable dt = new DataTable();
            dt = VSWebBL.ReportsBL.ReportsBL.Ins.GetUserAccessList(seluser);
            if (dt.Rows.Count > 0)
            {
                this.DataSource = dt;
                //11/4/2015 NS modified for VSPLUS-2023
                GroupField gf = new GroupField("FullName");
                GroupHeader2.GroupFields.Add(gf);
                xrLabel3.DataBindings.Add("Text", dt, "FullName");
                culServerName.DataBindings.Add("Text", dt, "Name");
                culLocation.DataBindings.Add("Text", dt, "Location");
                culServerType.DataBindings.Add("Text", dt, "ServerType");
            }
        }

    }
}
