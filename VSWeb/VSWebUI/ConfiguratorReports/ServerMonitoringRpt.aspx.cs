using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace VSWebUI.ConfiguratorReports
{
    public partial class ServerMonitoringRpt : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ConfiguratorReports.ServerMonitoringXtraRpt report = new ConfiguratorReports.ServerMonitoringXtraRpt();
            this.ReportViewer1.Report = report;
            this.ReportViewer1.DataBind();
        }
        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (Request.QueryString["M"] == "C" && Request.QueryString["M"].ToString() != "")
            {
                this.MasterPageFile = "~/Site1.Master";

            }
            else
            {
                this.MasterPageFile = "~/DashboardSite.Master";

            }
        }

        protected void ReptBtn_Click(object sender, EventArgs e)
        {

            Response.Redirect("~/Configurator/Reports.aspx?M=" + Request.QueryString["M"], false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
            Context.ApplicationInstance.CompleteRequest();
        }

    }
}