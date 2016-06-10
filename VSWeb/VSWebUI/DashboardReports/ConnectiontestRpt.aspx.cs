using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace VSWebUI.DashboardReports
{
    public partial class ConnectiontestRpt : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string selectedServer = "";
           
            if (this.ServerListFilterComboBox.SelectedIndex >= 0)
            {
                
                selectedServer = this.ServerListFilterComboBox.SelectedItem.Value.ToString();
               
            }
            DashboardReports.ConnectionstestXtraRpt report = new DashboardReports.ConnectionstestXtraRpt();
            report.Parameters["Name"].Value = selectedServer;
            report.CreateDocument();
            ASPxDocumentViewer1.Report = report;
            ASPxDocumentViewer1.DataBind();
            
            if (!IsPostBack)
            {
                fillcombo();
            }
        }

        public void fillcombo()
        {
            DataTable dt = new DataTable();
            dt = VSWebBL.ReportsBL.ReportsBL.Ins.CommunityNames();
            ServerListFilterComboBox.DataSource = dt;
            ServerListFilterComboBox.TextField = "Name";
            ServerListFilterComboBox.ValueField = "Name";
            ServerListFilterComboBox.DataBind();

        }


        protected void ServerListFilterComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedServer = "";
          
            if (this.ServerListFilterComboBox.SelectedIndex >= 0)
            {
                selectedServer = this.ServerListFilterComboBox.SelectedItem.Value.ToString();
               
            }
            DashboardReports.ConnectionstestXtraRpt report = new DashboardReports.ConnectionstestXtraRpt();
            report.Parameters["Name"].Value = selectedServer;
            report.CreateDocument();
            ASPxDocumentViewer1.Report = report;
            ASPxDocumentViewer1.DataBind();
        }

        protected void ServerListResetButton_Click(object sender, EventArgs e)
        {
            this.ServerListFilterComboBox.SelectedIndex = -1;
            DashboardReports.ConnectionstestXtraRpt report = new DashboardReports.ConnectionstestXtraRpt();
            report.Parameters["Name"].Value = "";
            report.CreateDocument();
            ASPxDocumentViewer1.Report = report;
            ASPxDocumentViewer1.DataBind();
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