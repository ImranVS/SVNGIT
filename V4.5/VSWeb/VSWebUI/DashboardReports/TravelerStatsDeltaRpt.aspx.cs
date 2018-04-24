using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace VSWebUI.DashboardReports
{
    public partial class TravelerStatsDeltaRpt : System.Web.UI.Page
    {
        #region PrivateFunctions
        private void LoadReport(int ind1, int ind2)
        {
            string interval = "000-001";
            string travelername = "";
            string startdate = "";
            string enddate = "";
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
            if (this.TravelerFilterComboBox.Items.Count > 0)
            {
                this.TravelerFilterComboBox.SelectedIndex = ind2;
                ind = this.TravelerFilterComboBox.SelectedIndex;
                this.TravelerFilterComboBox.SelectedItem.Value = this.TravelerFilterComboBox.Items[ind].Value.ToString();
                travelername = this.TravelerFilterComboBox.Items[ind].Value.ToString();
            }
            //11/24/2014 NS modified
            //startdate = StartDateEdit.Text;
            //enddate = EndDateEdit.Text;
            startdate = dtPick.FromDate;
            enddate = dtPick.ToDate;
            DashboardReports.TravelerStatsDeltaXtraRpt report = new DashboardReports.TravelerStatsDeltaXtraRpt();
            report.Parameters["Interval"].Value = interval;
            report.Parameters["ServerName"].Value = travelername;
            report.Parameters["StartDate"].Value = startdate;
            report.Parameters["EndDate"].Value = enddate;
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
            TravelerFilterComboBox.DataSource = dt;
            TravelerFilterComboBox.TextField = "TravelerServerName";
            TravelerFilterComboBox.ValueField = "TravelerServerName";

            TravelerFilterComboBox.DataBind();

        }


        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            /*
            DateTime dt = new DateTime();
            if (StartDateEdit.Text == "")
            {
                dt = DateTime.Now;
                //dt = new DateTime(dt.Year, dt.Month, 1);
                StartDateEdit.Text = dt.ToShortDateString();
            }

            if (EndDateEdit.Text == "")
            {
                EndDateEdit.Text = DateTime.Now.ToShortDateString();
            }
             */
            LoadReport(this.IntervalFilterComboBox.SelectedIndex, this.TravelerFilterComboBox.SelectedIndex);
        }

        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            LoadReport(this.IntervalFilterComboBox.SelectedIndex, this.TravelerFilterComboBox.SelectedIndex);
        }

        protected void ResetButton_Click(object sender, EventArgs e)
        {
            LoadReport(0, 0);
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