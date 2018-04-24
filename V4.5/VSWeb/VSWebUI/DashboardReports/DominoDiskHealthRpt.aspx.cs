using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace VSWebUI.DashboardReports
{
    public partial class DominoDiskHealthRpt : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string selectedServer = "";
            string selectedServerSQL = "";
            if (this.ServerListFilterComboBox.SelectedIndex >= 0)
            {
                //5/6/2015 NS modified for VSPLUS-1707
                selectedServer = this.ServerListFilterComboBox.SelectedItem.Value.ToString();
                selectedServerSQL = "'" + this.ServerListFilterComboBox.SelectedItem.Value.ToString() + "'";
            }
            DashboardReports.DominoDiskHealthXtraRpt report = new DashboardReports.DominoDiskHealthXtraRpt();
            report.Parameters["ServerName"].Value = selectedServer;
            report.Parameters["ServerNameSQL"].Value = selectedServerSQL;
            this.ReportViewer1.Report = report;
            this.ReportViewer1.DataBind();

            if (!IsPostBack)
            {
                fillcombo();
            }
        }

        public void fillcombo()
        {
            DataTable dt = new DataTable();
            dt = VSWebBL.ReportsBL.ReportsBL.Ins.DominoDiskHealthRptBL();
            ServerListFilterComboBox.DataSource = dt;
            ServerListFilterComboBox.TextField = "ServerName";
            ServerListFilterComboBox.ValueField = "ServerName";
            ServerListFilterComboBox.DataBind();

        }



        protected void ServerListFilterComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedServer = "";
            string selectedServerSQL = "";
            if (this.ServerListFilterComboBox.SelectedIndex >= 0)
            {
                selectedServer = this.ServerListFilterComboBox.SelectedItem.Value.ToString();
                selectedServerSQL = "'" + this.ServerListFilterComboBox.SelectedItem.Value.ToString() + "'";
            }
            DashboardReports.DominoDiskHealthXtraRpt report = new DashboardReports.DominoDiskHealthXtraRpt();
            report.Parameters["ServerName"].Value = selectedServer;
            report.Parameters["ServerNameSQL"].Value = selectedServerSQL;
            this.ReportViewer1.Report = report;
            this.ReportViewer1.DataBind();
        }

        protected void ServerListResetButton_Click(object sender, EventArgs e)
        {
            this.ServerListFilterComboBox.SelectedIndex = -1;
            DashboardReports.DominoDiskHealthXtraRpt report = new DashboardReports.DominoDiskHealthXtraRpt();
            report.Parameters["ServerName"].Value = "";
            report.Parameters["ServerNameSQL"].Value = "";
            this.ReportViewer1.Report = report;
            this.ReportViewer1.DataBind();
        }

        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (Request.QueryString["M"] == "C" && Request.QueryString["M"].ToString() != "")
            {
                this.MasterPageFile = "~/Reports.Master";

            }
            else
            {
                this.MasterPageFile = "~/Reports.Master";

            }
        }
        protected void ReptBtn_Click(object sender, EventArgs e)
        {

            Response.Redirect("~/Configurator/Reports.aspx?M=" + Request.QueryString["M"], false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
            Context.ApplicationInstance.CompleteRequest();
        }

    }
}