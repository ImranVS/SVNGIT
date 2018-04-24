using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace VSWebUI.DashboardReports
{
    public partial class DBClusterRpt : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FillCombobox();
            }
            string clustername = "";
            //4/29/2015 NS modified for VSPLUS-1692
            if (ClusterNameComboBox.SelectedIndex != -1)
            {
                clustername = this.ClusterNameComboBox.SelectedItem.Value.ToString();
            }
            DashboardReports.DBClusterXtraRpt report = new DashboardReports.DBClusterXtraRpt();
            report.Parameters["ClusterName"].Value = clustername;
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

        public void FillCombobox()
        {
            DataTable dt = VSWebBL.ReportsBL.ReportsBL.Ins.GetDBClusterNames();
            //4/29/2015 NS modified for VSPLUS-1692
            if (dt.Rows.Count > 0)
            {
                ClusterNameComboBox.DataSource = dt;
                ClusterNameComboBox.TextField = "ClusterName";
                ClusterNameComboBox.DataBind();
                ClusterNameComboBox.SelectedItem = ClusterNameComboBox.Items[0];
            }
        }

        protected void ReptBtn_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Configurator/Reports.aspx?M=" + Request.QueryString["M"], false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
            Context.ApplicationInstance.CompleteRequest();
        }

        protected void ClusterNameComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string clustername = "";
            clustername = this.ClusterNameComboBox.SelectedItem.Value.ToString();
            DashboardReports.DBClusterXtraRpt report = new DashboardReports.DBClusterXtraRpt();
            report.Parameters["ClusterName"].Value = clustername;
            report.CreateDocument();
            ASPxDocumentViewer1.Report = report;
            ASPxDocumentViewer1.DataBind();
        }
    }
}