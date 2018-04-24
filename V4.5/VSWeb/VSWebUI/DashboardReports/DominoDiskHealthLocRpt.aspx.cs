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
    public partial class DominoDiskHealthLocRpt : System.Web.UI.Page
    {
        //12/9/2015 NS added for VSPLUS-2395
        private static DominoDiskHealthLocRpt _self = new DominoDiskHealthLocRpt();
        public static DominoDiskHealthLocRpt Ins
        {
            get
            {
                return _self;
            }
        }
        DashboardReports.DominoDiskHealthLocXtraRpt report;
        public DashboardReports.DominoDiskHealthLocXtraRpt GetRpt()
        {
            report = new DashboardReports.DominoDiskHealthLocXtraRpt();
            DataTable dt = new DataTable();
            dt = VSWebBL.ReportsBL.XsdBL.Ins.DominoDiskSpaceLocBL(report.Parameters["LocationName"].Value.ToString());
            report.DataSource = dt;
            XRLabel xrLabel2;
            XRLabel xrLabel3;
            XRTableCell mtDiskName;
            XRTableCell mtPercentFree;
            XRTableCell mtDiskSize;
            XRTableCell mtDiskFree;
            XRTableCell mtUpdated;
            XRTableCell mtThreshold;
            xrLabel2 = (XRLabel)report.FindControl("xrLabel2",true);
            xrLabel2.DataBindings.Add("Text", dt, "Location");
            xrLabel3 = (XRLabel)report.FindControl("xrLabel3", true);
            xrLabel3.DataBindings.Add("Text", dt, "ServerName");
            mtDiskName = (XRTableCell)report.FindControl("mtDiskName", true);
            mtDiskName.DataBindings.Add("Text", dt, "DiskName");
            mtPercentFree = (XRTableCell)report.FindControl("mtPercentFree", true);
            mtPercentFree.DataBindings.Add("Text", dt, "PercentFree");
            mtDiskSize = (XRTableCell)report.FindControl("mtDiskSize", true);
            mtDiskSize.DataBindings.Add("Text", dt, "DiskSize");
            mtDiskFree = (XRTableCell)report.FindControl("mtDiskFree", true);
            mtDiskFree.DataBindings.Add("Text", dt, "DiskFree");
            mtUpdated = (XRTableCell)report.FindControl("mtUpdated", true);
            mtUpdated.DataBindings.Add("Text", dt, "Updated");
            mtThreshold = (XRTableCell)report.FindControl("mtThreshold", true);
            mtThreshold.DataBindings.Add("Text", dt, "Threshold");
            return report;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            string selectedServer = "";
            if (this.ServerListFilterComboBox.SelectedIndex >= 0)
            {
                selectedServer = this.ServerListFilterComboBox.SelectedItem.Value.ToString();
            }
            DashboardReports.DominoDiskHealthLocXtraRpt report = new DashboardReports.DominoDiskHealthLocXtraRpt();
            report.Parameters["LocationName"].Value = selectedServer;
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
            dt = VSWebBL.ReportsBL.ReportsBL.Ins.DominoDiskHealthLocRptBL();
            ServerListFilterComboBox.DataSource = dt;
            ServerListFilterComboBox.TextField = "Location";
            ServerListFilterComboBox.ValueField = "Location";

            ServerListFilterComboBox.DataBind();
          
        }



        protected void ServerListFilterComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedServer = "";
            if (this.ServerListFilterComboBox.SelectedIndex >= 0)
            {
                selectedServer = this.ServerListFilterComboBox.SelectedItem.Value.ToString();
            }
            DashboardReports.DominoDiskHealthLocXtraRpt report = new DashboardReports.DominoDiskHealthLocXtraRpt();
            report.Parameters["LocationName"].Value = selectedServer;
            this.ReportViewer1.Report = report;
            this.ReportViewer1.DataBind();
        }

        protected void ServerListResetButton_Click(object sender, EventArgs e)
        {
            this.ServerListFilterComboBox.SelectedIndex = -1;
            DashboardReports.DominoDiskHealthLocXtraRpt report = new DashboardReports.DominoDiskHealthLocXtraRpt();
            report.Parameters["LocationName"].Value = "";
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