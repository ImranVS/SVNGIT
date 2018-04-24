using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace VSWebUI.DashboardReports
{
    public partial class TravelerStatsSrvRpt : System.Web.UI.Page
    {
        #region PrivateFunctions
        private void LoadReport(int ind1, int ind2)
        {
            string travelername = "";
            string servername = "";
            int ind = -1;
            if (ind1 == -1)
            {
                ind1 = 0;
            }
            if (ind2 == -1)
            {
                ind2 = 0;
            }
            if (!IsPostBack)
            {
                fillmailservercombo();
                fillservernamecombo();
            }
            if (this.TravelerFilterComboBox.Items.Count > 0)
            {
                this.TravelerFilterComboBox.SelectedIndex = ind1;
                ind = this.TravelerFilterComboBox.SelectedIndex;
                this.TravelerFilterComboBox.SelectedItem.Value = this.TravelerFilterComboBox.Items[ind].Value.ToString();
                travelername = this.TravelerFilterComboBox.Items[ind].Value.ToString();
            }
            if (this.ServerFilterComboBox.Items.Count > 0)
            {
                this.ServerFilterComboBox.SelectedIndex = ind2;
                ind = this.ServerFilterComboBox.SelectedIndex;
                this.ServerFilterComboBox.SelectedItem.Value = this.ServerFilterComboBox.Items[ind].Value.ToString();
                servername = this.ServerFilterComboBox.Items[ind].Value.ToString();
            }
            DashboardReports.TravelerStatsSrvXtraRpt report = new DashboardReports.TravelerStatsSrvXtraRpt();
            report.Parameters["TravelerName"].Value = travelername;
            report.Parameters["ServerName"].Value = servername;
            this.ReportViewer1.Report = report;
            this.ReportViewer1.DataBind();
        }

        private void fillmailservercombo()
        {
            DataTable dt = new DataTable();
            dt = VSWebBL.ReportsBL.ReportsBL.Ins.fillMailServer();
            ServerFilterComboBox.DataSource = dt;
            ServerFilterComboBox.TextField = "MailServerName";
            ServerFilterComboBox.ValueField = "MailServerName";

            ServerFilterComboBox.DataBind();

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
            LoadReport(this.TravelerFilterComboBox.SelectedIndex, this.ServerFilterComboBox.SelectedIndex);
        }

        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            LoadReport(this.TravelerFilterComboBox.SelectedIndex, this.ServerFilterComboBox.SelectedIndex);
        }

        protected void ResetButton_Click(object sender, EventArgs e)
        {
            int ind = 0;
            LoadReport(ind, ind);
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