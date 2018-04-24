using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace VSWebUI.DashboardReports
{
    public partial class DominoDBAvgSizeRpt : System.Web.UI.Page
    {
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
                fillcombosrv();
            }
            FillReport();
        }

        public void fillcombosrv()
        {
            DataTable d = new DataTable();
            d = VSWebBL.ReportsBL.ReportsBL.Ins.GetDominoDBStatsSrv();
            ServerComboBox.DataSource = d;
            ServerComboBox.TextField = "Server";
            ServerComboBox.ValueField = "Server";
            ServerComboBox.DataBind();
        }

        public void FillReport()
        {
            DashboardReports.DominoDBAvgSizeXtraRpt report = new DashboardReports.DominoDBAvgSizeXtraRpt();
            string selectedServer = "";
            if (this.ServerComboBox.SelectedIndex >= 0)
            {
                selectedServer = this.ServerComboBox.SelectedItem.Value.ToString();
            }
            report.Parameters["ServerName"].Value = selectedServer;
            this.ReportViewer1.Report = report;
            this.ReportViewer1.DataBind();
        }
    }
}