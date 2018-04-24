using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using System.Data;
using System.IO;

namespace VSWebUI.DashboardReports
{
    public partial class TravelerStatsRpt : System.Web.UI.Page
    {
        #region PrivateFunctions
        private void LoadReport(int ind1, int ind2)
        {
            string interval = "000-001";
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
                fillintervalcombo();
                fillservernamecombo();
            }
            if (this.IntervalFilterComboBox.Items.Count > 0)
            {
                this.IntervalFilterComboBox.SelectedIndex = ind1;
                ind = this.IntervalFilterComboBox.SelectedIndex;
                this.IntervalFilterComboBox.SelectedItem.Value = this.IntervalFilterComboBox.Items[ind].Value.ToString();
                interval = this.IntervalFilterComboBox.Items[ind].Value.ToString();
            }
            if (this.ServerFilterComboBox.Items.Count > 0)
            {
                this.ServerFilterComboBox.SelectedIndex = ind2;
                ind = this.ServerFilterComboBox.SelectedIndex;
                this.ServerFilterComboBox.SelectedItem.Value = this.ServerFilterComboBox.Items[ind].Value.ToString();
                servername = this.ServerFilterComboBox.Items[ind].Value.ToString();
            }
            DashboardReports.TravelerStatsXtraRpt report = new DashboardReports.TravelerStatsXtraRpt();
            report.Parameters["Interval"].Value = interval;
            report.Parameters["ServerName"].Value = servername;
            this.ReportViewer1.Report = report;
            this.ReportViewer1.DataBind();
        }

        private void fillintervalcombo()
        {
            DataTable dt = new DataTable();
            dt = VSWebBL.ReportsBL.ReportsBL.Ins.fillTravelerInterval();
            IntervalFilterComboBox.DataSource = dt;
            IntervalFilterComboBox.TextField = "Interval";
            IntervalFilterComboBox.ValueField = "Interval";

            IntervalFilterComboBox.DataBind();

        }

        private void fillservernamecombo()
        {
            DataTable dt = new DataTable();
            dt = VSWebBL.ReportsBL.ReportsBL.Ins.fillTravelerServer();
            ServerFilterComboBox.DataSource = dt;
            ServerFilterComboBox.TextField = "TravelerServerName";
            ServerFilterComboBox.ValueField = "TravelerServerName";

            ServerFilterComboBox.DataBind();

        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            LoadReport(this.IntervalFilterComboBox.SelectedIndex, this.ServerFilterComboBox.SelectedIndex);
            if (!IsPostBack)
            {
                DataTable dt = VSWebBL.ConfiguratorBL.ReportsBL.Ins.GetReportID(Path.GetFileName(Request.Path));
                if (dt.Rows.Count > 0)
                {
                    Session["ReportID"] = dt.Rows[0]["ID"].ToString();
                }
                DataTable dt2 = VSWebBL.ConfiguratorBL.ReportsBL.Ins.GetUserReportFavorites(Session["UserID"].ToString(), Session["ReportID"].ToString());
                if (dt2.Rows.Count > 0)
                {
                    ASPxRatingControl1.Value = Convert.ToInt32(Convert.ToBoolean(dt2.Rows[0]["IsFavorite"].ToString()));
                }
            }
        }

        protected void Page_Unload(object sender, EventArgs e)
        {
            string isfavorite = "False";
            if (ASPxRatingControl1.Value == 1)
            {
                isfavorite = "True";
            }
            VSWebBL.ConfiguratorBL.ReportsBL.Ins.UpdateUserReportFavorites(Session["UserID"].ToString(), Session["ReportID"].ToString(), isfavorite);
        }

        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            LoadReport(this.IntervalFilterComboBox.SelectedIndex, this.ServerFilterComboBox.SelectedIndex);
        }

        protected void ResetButton_Click(object sender, EventArgs e)
        {
            int ind = 0;
            LoadReport(ind,ind);
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