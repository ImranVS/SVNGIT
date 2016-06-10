using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace VSWebUI.DashboardReports
{
    public partial class TravelerStatsHTTPRpt : System.Web.UI.Page
    {
        #region PrivateFunctions
        private void LoadReport(int ind1)
        {
            string travelername = "";
            int ind = -1;
            if (ind1 == -1)
            {
                ind1 = 0;
            }
            if (!IsPostBack)
            {
                fillservernamecombo();
            }
            if (this.TravelerFilterComboBox.Items.Count > 0)
            {
                this.TravelerFilterComboBox.SelectedIndex = ind1;
                ind = this.TravelerFilterComboBox.SelectedIndex;
                this.TravelerFilterComboBox.SelectedItem.Value = this.TravelerFilterComboBox.Items[ind].Value.ToString();
                travelername = this.TravelerFilterComboBox.Items[ind].Value.ToString();
            }
            DashboardReports.TravelerStatsHTTPXtraRpt report = new DashboardReports.TravelerStatsHTTPXtraRpt();
            report.Parameters["TravelerName"].Value = travelername;
            this.ReportViewer1.Report = report;
            this.ReportViewer1.DataBind();
        }

        private void fillservernamecombo()
        {
            DataTable dt = new DataTable();
            dt = VSWebBL.ReportsBL.ReportsBL.Ins.fillTravelerServer();
            TravelerFilterComboBox.DataSource = dt;
            TravelerFilterComboBox.TextField = "TravelerServerName";
            TravelerFilterComboBox.ValueField = "TravelerServerName";

            TravelerFilterComboBox.DataBind();

        }
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            LoadReport(this.TravelerFilterComboBox.SelectedIndex);
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

        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            LoadReport(this.TravelerFilterComboBox.SelectedIndex);
        }

        protected void ResetButton_Click(object sender, EventArgs e)
        {
            int ind = 0;
            LoadReport(ind);
        }
    }
}