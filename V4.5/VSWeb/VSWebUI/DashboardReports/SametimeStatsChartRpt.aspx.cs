using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace VSWebUI.DashboardReports
{
    public partial class SametimeStatsChartRpt : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string selectedStat = "";
            if (!IsPostBack)
            {
                fillcombo();
            }
            if (StatComboBox.SelectedIndex >= 0)
            {
                selectedStat = StatComboBox.SelectedItem.Value.ToString();
            }
            DashboardReports.SametimeStatsChartXtraRpt report = new DashboardReports.SametimeStatsChartXtraRpt();
            report.Parameters["StatName"].Value = selectedStat;
            report.Parameters["StartDate"].Value = dtPick.FromDate;
            report.Parameters["EndDate"].Value = dtPick.ToDate;
            this.ReportViewer1.Report = report;
            this.ReportViewer1.DataBind();
        }

        public void fillcombo()
        {
            DataTable dt = new DataTable();
            dt = VSWebBL.ReportsBL.ReportsBL.Ins.GetSametimeStatNames();
            StatComboBox.DataSource = dt;
            StatComboBox.TextField = "StatName";
            StatComboBox.ValueField = "StatName";
            StatComboBox.DataBind();
            StatComboBox.SelectedIndex = 0;
        }

        protected void ReptBtn_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Configurator/Reports.aspx?M=" + Request.QueryString["M"], false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
            Context.ApplicationInstance.CompleteRequest();
        }
    }
}