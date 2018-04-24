using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace VSWebUI
{
    public partial class EXJournalDocumentTotals : System.Web.UI.Page
    {
        string srvtype;
        protected void Page_Load(object sender, EventArgs e)
        {
           
            FillReport();
            if (!IsPostBack)
            {
                fillServerListBox("EXJournal.DocCount.Total");
            }
               
        }


        public void fillServerListBox(string StatName)
        {
            DataTable dt = new DataTable();

            dt = VSWebBL.ReportsBL.XsdBL.Ins.GetServersListFromDominoDailyStats(StatName);
          
            ServerListBox.DataSource = dt;
            ServerListBox.TextField = "ServerName";
            ServerListBox.ValueField = "ServerName";
            ServerListBox.DataBind();
         }


        public void FillReport()
        {
            DashboardReports.EXJournalDocumentTotalsRpt rpt = new DashboardReports.EXJournalDocumentTotalsRpt();           
            string selectedServer = "";
          
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
                    selectedServer = "";    
                }
                finally { }
            }
            rpt.Parameters["ServerName"].Value = selectedServer;
            rpt.Parameters["Threshold"].Value = TCutoffTextBox.Text;
            ReportViewer1.Report = rpt;
            ReportViewer1.DataBind();
        }

        protected void ResetButton_Click(object sender, EventArgs e)
        {

            if (Request.UrlReferrer != null)
                Response.Redirect(Request.RawUrl); 
        }

        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            FillReport();
        }

        protected void Page_PreInit(object sender, EventArgs e)
        {
           
                this.MasterPageFile = "~/Reports.Master";

        }

      
    }
}