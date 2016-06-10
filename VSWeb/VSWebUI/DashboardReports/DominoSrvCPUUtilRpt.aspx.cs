using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace VSWebUI.DashboardReports
{
    public partial class DominoSrvCPUUtilRpt : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Report();

            if (!IsPostBack)
            {
                fillcombo();
            }

        }

        public void Report()
        {
            string selectedServer = "";
            /*
            if (this.ServerListFilterComboBox.SelectedIndex >= 0)
            {
                selectedServer = this.ServerListFilterComboBox.SelectedItem.Value.ToString();
            }
             */
            if (this.ServerListFilterListBox.SelectedItems.Count > 0)
            {
                selectedServer = "";
                for (int i = 0; i < this.ServerListFilterListBox.SelectedItems.Count; i++)
                {
                    selectedServer += "'" + this.ServerListFilterListBox.SelectedItems[i].Text + "'" + ",";
                }
                try
                {
                    selectedServer = selectedServer.Substring(0, selectedServer.Length - 1);
                }
                catch
                {
                    selectedServer = "";     // throw ex; 
                }
                finally { }
            }
            DashboardReports.DominoSrvCPUUtilXtraRpt report = new DashboardReports.DominoSrvCPUUtilXtraRpt();
            report.Parameters["ServerName"].Value = selectedServer;
            this.ReportViewer1.Report = report;
            this.ReportViewer1.DataBind();
        }

        public void fillcombo()
        {
            DataTable dt = new DataTable();
            dt = VSWebBL.ReportsBL.ReportsBL.Ins.DominoSrvCPUUtilRptBL();
            /*
            ServerListFilterComboBox.DataSource = dt;
            ServerListFilterComboBox.TextField = "ServerName";
            ServerListFilterComboBox.ValueField = "ServerName";
            ServerListFilterComboBox.DataBind();
            */
            ServerListFilterListBox.DataSource = dt;
            ServerListFilterListBox.TextField = "ServerName";
            ServerListFilterListBox.ValueField = "ServerName";
            ServerListFilterListBox.DataBind();
        }

        protected void ServerListResetButton_Click(object sender, EventArgs e)
        {
            //this.ServerListFilterComboBox.SelectedIndex = -1;
            this.ServerListFilterListBox.UnselectAll();
            DashboardReports.DominoSrvCPUUtilXtraRpt report = new DashboardReports.DominoSrvCPUUtilXtraRpt();
            report.Parameters["ServerName"].Value = "";
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

        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            Report();
        }

    }
}