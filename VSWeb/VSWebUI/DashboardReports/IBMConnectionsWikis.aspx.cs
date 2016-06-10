using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace VSWebUI.DashboardReports
{
    public partial class IBMConnectionsWikis : System.Web.UI.Page
    {
        //11-04-2016 Durga Modified for VSPLUS-2829
        List<string> strringlist = new List<string>();
        string WikiType = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            FillReport();
            GetWikitype();
            if (!IsPostBack)
            {
                fillWikiTypeCombobox();
                fillcombo(WikiType);
            }
        }

        public void fillcombo(string statname1)
        {
            DataTable dt = new DataTable();

            dt = VSWebBL.ReportsBL.ReportsBL.Ins.GetIBMConnectionsServerlist(statname1);
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
         if (Request.UrlReferrer != null)
             Response.Redirect(Request.RawUrl); 
        }

        public void FillReport()
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
            GetWikitype();
           
            strfrom = dtPick.FromDate;
            strto = dtPick.ToDate;
            DateTime dtfrom = DateTime.Parse(strfrom);
            DateTime dtto = DateTime.Parse(strto);
            
            DashboardReports.IBMConnectionsWikisRpt report = new DashboardReports.IBMConnectionsWikisRpt();
            report.Parameters["ServerName"].Value = selectedServer;
            report.Parameters["DateFrom"].Value = strfrom;
            report.Parameters["DateTo"].Value = strto;
            report.Parameters["WikiType"].Value = WikiType;
            this.ReportViewer1.Report = report;
            this.ReportViewer1.DataBind();

        }
        protected void WikiTypeCombobox_SelectedIndexChanged(object sender, EventArgs e)
        {

            FillReport();
            GetWikitype();
            fillcombo(WikiType);

        }

        public void fillWikiTypeCombobox()
        {
            DataTable dt = new DataTable();
            dt = VSWebBL.ReportsBL.ReportsBL.Ins.GetonnectionsStatsNamesofWikis("NUM_OF_WIKIS_WIKIS_CREATED_YESTERDAY", "NUM_OF_WIKIS_PAGES_CREATED_YESTERDAY");
            if (dt.Rows.Count > 0)
            {
                WikiTypeCombobox.SelectedIndex = 0;
                WikiTypeCombobox.TextField = "StatName";
                WikiTypeCombobox.ValueField = "StatName";
                WikiTypeCombobox.DataSource = dt;
                WikiTypeCombobox.DataBind();
            }
        }
        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            FillReport();
        }
        public void GetWikitype()

          {
              if (WikiTypeCombobox.Text == "Wikis Created")
            {
                WikiType = "NUM_OF_WIKIS_WIKIS_CREATED_YESTERDAY";
            }
            else
            {
                WikiType = "NUM_OF_WIKIS_PAGES_CREATED_YESTERDAY";
            }
    }

    }
}