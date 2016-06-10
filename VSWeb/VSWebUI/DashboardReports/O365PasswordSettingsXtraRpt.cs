using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;
using DevExpress.XtraCharts;

namespace VSWebUI.DashboardReports
{
    public partial class O365PasswordSettingsXtraRpt : DevExpress.XtraReports.UI.XtraReport
    {
        public O365PasswordSettingsXtraRpt()
        {
            InitializeComponent();
        }

        private void O365PasswordSettingsXtraRpt_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            DataTable dt = new DataTable();
            string servername = this.ServerName.Value.ToString();
            dt = VSWebBL.DashboardBL.Office365BL.Ins.GetOffice365PwdExpSettings(servername);
            if (dt.Rows.Count > 0)
            {
                xrChart1.Series.Clear();
                Series series = new Series("Pie1", ViewType.Pie);
                xrChart1.Series.Add(series);
                xrChart1.Series["Pie1"].DataSource = dt;
                xrChart1.Series["Pie1"].ArgumentDataMember = "PasswordNeverExpires";
                xrChart1.Series["Pie1"].ValueDataMembers.AddRange(new string[] { "CountEach" });
                series.LegendTextPattern = "{A}: {V}";
                xrChart1.DataSource = dt;
            }
            dt = VSWebBL.DashboardBL.Office365BL.Ins.GetOffice365PwdStrongSettings(servername);
            if (dt.Rows.Count > 0)
            {
                xrChart2.Series.Clear();
                Series series = new Series("Pie1", ViewType.Pie);
                xrChart2.Series.Add(series);
                xrChart2.Series["Pie1"].DataSource = dt;
                xrChart2.Series["Pie1"].ArgumentDataMember = "StrongPasswordRequired";
                xrChart2.Series["Pie1"].ValueDataMembers.AddRange(new string[] { "CountEach" });
                series.LegendTextPattern = "{A}: {V}";
                xrChart2.DataSource = dt;
            }
            dt = VSWebBL.DashboardBL.Office365BL.Ins.GetOffice365UserSettings(servername);
            if (dt.Rows.Count > 0)
            {
                this.DataSource = dt;
                xrTableCell6.DataBindings.Add("Text", dt, "DisplayName");
                xrTableCell7.DataBindings.Add("Text", dt, "UserPrincipalName");
                xrTableCell8.DataBindings.Add("Text", dt, "UserType");
                xrTableCell9.DataBindings.Add("Text", dt, "StrongPasswordRequired");
                xrTableCell10.DataBindings.Add("Text", dt, "PasswordNeverExpires");
            }
        }

    }
}
