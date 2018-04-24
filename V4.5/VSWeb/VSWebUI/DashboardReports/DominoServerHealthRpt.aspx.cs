using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DevExpress.XtraReports.UI;

namespace VSWebUI.DashboardReports
{
    public partial class DominoServerHealthRpt : System.Web.UI.Page
    {
        //12/10/2015 NS added for VSPLUS-2395
        private static DominoServerHealthRpt _self = new DominoServerHealthRpt();
        public static DominoServerHealthRpt Ins
        {
            get
            {
                return _self;
            }
        }
        DashboardReports.DominoServerHealthXtraRpt report;
        public DashboardReports.DominoServerHealthXtraRpt GetRpt()
        {
            report = new DashboardReports.DominoServerHealthXtraRpt();
            DataTable dt = new DataTable();
            dt = VSWebBL.ReportsBL.ReportsBL.Ins.GetDominoServerHealth();
            report.DataSource = dt;
            XRLabel xrLabel2;
            XRTableCell mtServerName;
            XRTableCell mtStatus;
            XRTableCell mtDetails;
            XRTableCell mtResponse;
            XRTableCell mtResponseThreshold;
            xrLabel2 = (XRLabel)report.FindControl("xrLabel2", true);
            xrLabel2.DataBindings.Add("Text", dt, "Location");
            mtServerName = (XRTableCell)report.FindControl("mtServerName", true);
            mtServerName.DataBindings.Add("Text", dt, "Name");
            mtStatus = (XRTableCell)report.FindControl("mtStatus", true);
            mtStatus.DataBindings.Add("Text", dt, "Status");
            mtDetails = (XRTableCell)report.FindControl("mtDetails", true);
            mtDetails.DataBindings.Add("Text", dt, "Details");
            mtResponse = (XRTableCell)report.FindControl("mtResponseTime", true);
            mtResponse.DataBindings.Add("Text", dt, "ResponseTime");
            mtResponseThreshold = (XRTableCell)report.FindControl("mtResponseThreshold", true);
            mtResponseThreshold.DataBindings.Add("Text", dt, "ResponseThreshold");
            return report;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            DashboardReports.DominoServerHealthXtraRpt report = new DashboardReports.DominoServerHealthXtraRpt();
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