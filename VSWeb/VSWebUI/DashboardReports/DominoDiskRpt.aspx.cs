using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace VSWebUI.DashboardReports
{
    public partial class DominoDiskRpt : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string selectedServer = "";
            string date;
            if (this.ServerListFilterComboBox.SelectedIndex >= 0)
            {
                selectedServer = this.ServerListFilterComboBox.SelectedItem.Value.ToString();
            }
            if (this.DateParamEdit.Text == "")
            {
                date = DateTime.Now.ToString();
                this.DateParamEdit.Date = Convert.ToDateTime(date);
            }
            else
            {
                date = this.DateParamEdit.Value.ToString();
            }
            DateTime dt = Convert.ToDateTime(date); 
            DashboardReports.DominoDiskXtraRpt report = new DashboardReports.DominoDiskXtraRpt();
            report.Parameters["DateM"].Value = dt.Month;
            report.Parameters["DateY"].Value = dt.Year;
            report.Parameters["ServerName"].Value = selectedServer;
            this.ReportViewer1.Report = report;
            this.ReportViewer1.DataBind();
        }

        protected void ServerListFilterComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedServer = "";
            if (this.ServerListFilterComboBox.SelectedIndex >= 0)
            {
                selectedServer = this.ServerListFilterComboBox.SelectedItem.Value.ToString();
            }
            DashboardReports.DominoDiskXtraRpt report = new DashboardReports.DominoDiskXtraRpt();
            report.Parameters["ServerName"].Value = selectedServer;
            this.ReportViewer1.Report = report;
            this.ReportViewer1.DataBind();
        }

        protected void ServerListResetButton_Click(object sender, EventArgs e)
        {
            this.ServerListFilterComboBox.SelectedIndex = -1;
            DashboardReports.DominoDiskXtraRpt report = new DashboardReports.DominoDiskXtraRpt();
            report.Parameters["ServerName"].Value = "";
            this.ReportViewer1.Report = report;
            this.ReportViewer1.DataBind();
        }

        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            string date;
            string selectedServer = "";
            if (this.ServerListFilterComboBox.SelectedIndex >= 0)
            {
                selectedServer = this.ServerListFilterComboBox.SelectedItem.Value.ToString();
            }
            if (this.DateParamEdit.Text == "")
            {
                date = DateTime.Now.ToString();
            }
            else
            {
                date = this.DateParamEdit.Value.ToString();
            }
            DateTime dt = Convert.ToDateTime(date);
            DashboardReports.DominoDiskXtraRpt report = new DashboardReports.DominoDiskXtraRpt();
            report.Parameters["DateM"].Value = dt.Month;
            report.Parameters["DateY"].Value = dt.Year;
            report.Parameters["ServerName"].Value = selectedServer;
            this.ReportViewer1.Report = report;
            this.ReportViewer1.DataBind();
        }
    }
}