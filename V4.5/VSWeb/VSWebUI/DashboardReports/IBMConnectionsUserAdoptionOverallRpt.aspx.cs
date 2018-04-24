using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace VSWebUI.DashboardReports
{
    public partial class IBMConnectionsUserAdoptionOverallRpt : System.Web.UI.Page
    {
        string selectedServer = "";
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

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                fillcombo();
            }
            selectedServer = ServerComboBox.SelectedItem.Text;
            DashboardReports.IBMConnectionsUserAdoptionOverallXtraRpt report = new DashboardReports.IBMConnectionsUserAdoptionOverallXtraRpt();
            report.Parameters["ServerName"].Value = selectedServer;
            report.CreateDocument();
            ASPxDocumentViewer1.Report = report;
            ASPxDocumentViewer1.DataBind();
        }

        public void fillcombo()
        {
            DataTable dt = new DataTable();
            dt = VSWebBL.ReportsBL.ReportsBL.Ins.GetIBMConnectionsServerlist("");
            ServerComboBox.DataSource = dt;
            ServerComboBox.TextField = "ServerName";
            ServerComboBox.ValueField = "ServerName";
            ServerComboBox.DataBind();
            if (dt.Rows.Count > 0)
            {
                ServerComboBox.SelectedIndex = 0;
                selectedServer = ServerComboBox.SelectedItem.Text;
            }
        }
    }
}