using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace VSWebUI.DashboardReports
{
    public partial class IBMConnectionBookmarksRpt : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Report();
            if (!IsPostBack)
            {
                fillcombo();
            }
        }

        public void fillcombo()
        {
            DataTable dt = new DataTable();

           string BType = "";
               BType = "NUM_OF_BOOKMARKS_BOOKMARKS_CREATED_YESTERDAY";
               dt = VSWebBL.ReportsBL.ReportsBL.Ins.GetIBMConnectionsServerlist(BType);
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
           
            strfrom = dtPick.FromDate;
            strto = dtPick.ToDate;
            DateTime dtfrom = DateTime.Parse(strfrom);
            DateTime dtto = DateTime.Parse(strto);
           
            DashboardReports.IBMConnectionBookmarksXtraRpt report = new DashboardReports.IBMConnectionBookmarksXtraRpt();
            report.Parameters["ServerName"].Value = selectedServer;
            report.Parameters["DateFrom"].Value = strfrom;
            report.Parameters["DateTo"].Value = strto;
             this.ReportViewer1.Report = report;
            this.ReportViewer1.DataBind();

        }

        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            Report();
        }

        protected void ForumsTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {

            Report();
            fillcombo();

        }

    }
}