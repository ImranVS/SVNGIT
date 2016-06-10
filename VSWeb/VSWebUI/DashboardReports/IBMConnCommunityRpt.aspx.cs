using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.XtraCharts;
using DevExpress.XtraReports;
using System.Data;

namespace VSWebUI.DashboardReports
{
    public partial class IBMConnCommunityRpt : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            string selectedCommunity = "";
            if (this.UserFilterComboBox.SelectedIndex >= 0)
            {
                selectedCommunity = this.UserFilterComboBox.SelectedItem.Value.ToString();
            }
            DashboardReports.IBMConnCommunityXtraRpt report = new DashboardReports.IBMConnCommunityXtraRpt();
            report.Parameters["Name"].Value = selectedCommunity;
            report.CreateDocument();
            ASPxDocumentViewer1.Report = report;
            ASPxDocumentViewer1.DataBind();
            if (!IsPostBack)
            {
                fillcombo();
            }
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
        public void fillcombo()
        {
            DataTable dt = new DataTable();
            dt = VSWebBL.ReportsBL.ReportsBL.Ins.GetCommuniyList();
            UserFilterComboBox.DataSource = dt;
            UserFilterComboBox.TextField = "Name";
            UserFilterComboBox.ValueField = "Name";
            UserFilterComboBox.DataBind();
        }   

        protected void UserResetButton_Click(object sender, EventArgs e)
        {
            this.UserFilterComboBox.SelectedIndex = -1;
            DashboardReports.IBMConnCommunityXtraRpt report = new DashboardReports.IBMConnCommunityXtraRpt();
            report.Parameters["Name"].Value = "";
            report.CreateDocument();
            ASPxDocumentViewer1.Report = report;
            ASPxDocumentViewer1.DataBind();
        }

        protected void ReptBtn_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Configurator/Reports.aspx?M=" + Request.QueryString["M"], false);
            Context.ApplicationInstance.CompleteRequest();
   
        }

        protected void UserFilterComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedCommunity = "";
            if (this.UserFilterComboBox.SelectedIndex >= 0)
            {
                selectedCommunity = this.UserFilterComboBox.SelectedItem.Value.ToString();
            }
            DashboardReports.IBMConnCommunityXtraRpt report = new DashboardReports.IBMConnCommunityXtraRpt();
            report.Parameters["Name"].Value = selectedCommunity;
            report.CreateDocument();
            ASPxDocumentViewer1.Report = report;
            ASPxDocumentViewer1.DataBind();
        }
    }
}