using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace VSWebUI.DashboardReports
{
    public partial class O365PasswordSettingsRpt : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            fillcombo();
            string selectedServer = "";
            if (this.ServerListComboBox.SelectedIndex != -1)
            {
                selectedServer = ServerListComboBox.SelectedItem.Value.ToString();
            }
            else
            {
                if (ServerListComboBox.Items.Count > 0)
                {
                    ServerListComboBox.SelectedIndex = 0;
                    selectedServer = ServerListComboBox.SelectedItem.Value.ToString();
                }
            }
            Report(selectedServer);
        }

        public void fillcombo()
        {
            DataTable dt = new DataTable();
            dt = VSWebBL.DashboardBL.Office365BL.Ins.GetOffice365Servers();
            ServerListComboBox.DataSource = dt;
            ServerListComboBox.TextField = "ServerName";
            ServerListComboBox.ValueField = "ServerName";
            ServerListComboBox.DataBind();
        }

        //26/4/2016 sowmya added for VSPLUS-2881
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

        private void Report(string selectedServer)
        {
            DashboardReports.O365PasswordSettingsXtraRpt report = new DashboardReports.O365PasswordSettingsXtraRpt();
            report.Parameters["ServerName"].Value = selectedServer;
            report.CreateDocument();
            ASPxDocumentViewer1.Report = report;
            ASPxDocumentViewer1.DataBind();
        }
    }
}