using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace VSWebUI.DashboardReports
{
    public partial class ExchangeMsgRpt : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Report();
            if (!IsPostBack)
            {
                fillcombo("Mail_SentCount");
            }
        }

        public void fillcombo(string statname)
        {
            DataTable dt = new DataTable();
            dt = VSWebBL.ReportsBL.ReportsBL.Ins.GetExchangeServerList(statname);
            ServerListFilterListBox.DataSource = dt;
            ServerListFilterListBox.TextField = "ServerName";
            ServerListFilterListBox.ValueField = "ServerName";
            ServerListFilterListBox.DataBind();
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

        protected void ResetButton_Click(object sender, EventArgs e)
        {
            this.ServerListFilterListBox.UnselectAll();
            fillcombo("Mail_SentCount");
            DashboardReports.ExchangeMsgXtraRpt report = new DashboardReports.ExchangeMsgXtraRpt();
            report.Parameters["ServerName"].Value = "";
            this.ReportViewer1.Report = report;
            this.ReportViewer1.DataBind();
        }

        public void Report()
        {
            string selectedServer = "";
            if (this.ServerListFilterListBox.SelectedItems.Count > 0)
            {
                selectedServer = "";
                for (int i = 0; i < this.ServerListFilterListBox.SelectedItems.Count; i++)
                {
                    selectedServer += "'" + this.ServerListFilterListBox.SelectedItems[i].Text + "'" + ",";
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
            string strfrom = "";
            string strto = "";
            string rpttype = "1";
            strfrom = dtPick.FromDate;
            strto = dtPick.ToDate;
            DateTime dtfrom = DateTime.Parse(strfrom);
            DateTime dtto = DateTime.Parse(strto);
            if (dtfrom.Date == dtto.Date)
            {
                rpttype = "0";
            }
            DashboardReports.ExchangeMsgXtraRpt report = new DashboardReports.ExchangeMsgXtraRpt();
            report.Parameters["ServerName"].Value = selectedServer;
            report.Parameters["DateFrom"].Value = strfrom;
            report.Parameters["DateTo"].Value = strto;
            report.Parameters["RptType"].Value = rpttype;
            this.ReportViewer1.Report = report;
            this.ReportViewer1.DataBind();

        }
    }
}