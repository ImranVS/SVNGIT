using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace VSWebUI.DashboardReports
{
    public partial class MAXCpuUtilRpt : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            FillReport();

            if (!IsPostBack)
            {
                fillcombo();
            }
        }

        public void fillcombo()
        {
            DataTable d = new DataTable();
            //12/17/2013 NS modified
            //d = VSWebBL.ReportsBL.ReportsBL.Ins.getDominoSummaryStats();
            d = VSWebBL.ReportsBL.XsdBL.Ins.getServersForCPUUtil("");
            /*
            ServerComboBox.DataSource = d;
            ServerComboBox.TextField = "ServerName";
            ServerComboBox.ValueField = "ServerName";
            ServerComboBox.DataBind();
             */
            ServerListBox.DataSource = d;
            ServerListBox.TextField = "ServerName";
            ServerListBox.ValueField = "ServerName";
            ServerListBox.DataBind();
        }


        public void FillReport()
        {
            DashboardReports.MAXCpuUtilXtraRpt rpt = new DashboardReports.MAXCpuUtilXtraRpt();
            string selectedServer = "";
            /*
            if (this.ServerComboBox.SelectedIndex >= 0)
            {
                selectedServer = this.ServerComboBox.SelectedItem.Value.ToString();
            }
            */
            if (this.ServerListBox.SelectedItems.Count > 0)
            {
                selectedServer = "";
                for (int i = 0; i < this.ServerListBox.SelectedItems.Count; i++)
                {
                    selectedServer += "'" + this.ServerListBox.SelectedItems[i].Text + "'" + ",";
                }
                try
                {
                    selectedServer = selectedServer.Substring(0, selectedServer.Length - 1);
                }
                catch
                {
                    selectedServer = "";     // throw ex; 
                }
                finally { }
            }
            rpt.Parameters["ServerName"].Value = selectedServer;
            //11/21/2014 NS commented out
            /*
            if (StartDateEdit.Text == "")
            {
                StartDateEdit.Text = DateTime.Today.AddDays(-7).ToShortDateString();
            }

            if (EndDateEdit.Text == "")
            {
                EndDateEdit.Text = DateTime.Today.ToShortDateString();
            }
            */
            rpt.Parameters["StartDate"].Value = dtPick.FromDate; // StartDateEdit.Text;
            rpt.Parameters["EndDate"].Value = dtPick.ToDate; // EndDateEdit.Text;
            //27/05/2016 sowmya added for vaplus-2971
            rpt.Parameters["Threshold"].Value = TCutoffTextBox.Text;
            ReportViewer1.Report = rpt;
            ReportViewer1.DataBind();
        }

        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            FillReport();
        }

        protected void ResetButton_Click(object sender, EventArgs e)
        {
            //this.ServerComboBox.SelectedIndex = -1;
            this.ServerListBox.UnselectAll();
            DashboardReports.MAXCpuUtilXtraRpt rpt = new DashboardReports.MAXCpuUtilXtraRpt();

            rpt.Parameters["ServerName"].Value = "";
            //11/21/2014 NS commented out
            //rpt.Parameters["StartDate"].Value = DateTime.Now.ToString();
            //rpt.Parameters["EndDate"].Value = DateTime.Now.ToString();
            //ServerComboBox.Text = "";
            //StartDateEdit.Text = DateTime.Now.ToShortDateString();
            //EndDateEdit.Text = DateTime.Now.ToShortDateString();
            this.ReportViewer1.Report = rpt;
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