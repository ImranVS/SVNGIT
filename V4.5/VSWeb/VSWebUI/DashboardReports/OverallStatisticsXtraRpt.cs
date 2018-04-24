using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;

namespace VSWebUI.DashboardReports
{
    public partial class OverallStatisticsXtraRpt : DevExpress.XtraReports.UI.XtraReport
    {
        public OverallStatisticsXtraRpt()
        {
            InitializeComponent();
        }

        private void OverallStatisticsXtraRpt_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            string servername;
            string srvname;
            string sname;
            bool exactmatch;
            servername = this.ServerNameSQL.Value.ToString();
            srvname = this.ServerName.Value.ToString();
            exactmatch = (bool)this.ExactMatch.Value;
            sname = srvname;
            if (srvname == "" || exactmatch)
            {
                sname = servername;
            }
            DataTable dt = new DataTable();
            dt = VSWebBL.DashboardBL.MailHealthBL.Ins.GetServerMailDelivered(sname,int.Parse(this.DateM.Value.ToString()), int.Parse(this.DateY.Value.ToString()), exactmatch);
            this.DataSource = dt;
            this.DataMember = "Table";
            MonthYearLabel.Text = "Report for " + this.MonthYear.Value.ToString();
            xrTableCell1.DataBindings.Add("Text", dt, "ServerName");
            xrTableCell2.DataBindings.Add("Text", dt, "DownTime");
            xrTableCell3.DataBindings.Add("Text", dt, "MailDeliv");
        }

    }
}
