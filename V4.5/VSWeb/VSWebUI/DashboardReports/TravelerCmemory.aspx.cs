using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace VSWebUI
{
	//2/1/2016 Durga added for VSPLUS 2174
	public partial class TravelerCmemory : System.Web.UI.Page
    {
        string srvtype;
        protected void Page_Load(object sender, EventArgs e)
        {
           
            srvtype = Request.QueryString["ServerType"];
            if (srvtype != null)
            {
                if (srvtype.IndexOf("?") != -1)
                {
                    srvtype = srvtype.Substring(0, srvtype.IndexOf("?"));
                }
            }
            else
            {
                srvtype = "";
            }
            FillReport();
            if (!IsPostBack)
            {
                fillcombo(srvtype);
            }
               
        }


        public void fillcombo(string sType)
        {
            DataTable d = new DataTable();
            //2/19/2016 Durga Added for VSPLUS 2174
            string StatName = "MemoryC";
          d = VSWebBL.ReportsBL.XsdBL.Ins.getServersForTraveler(sType, StatName);
           
            ServerListBox.DataSource = d;
            ServerListBox.TextField = "ServerName";
            ServerListBox.ValueField = "ServerName";
            ServerListBox.DataBind();
         }


        public void FillReport()
        {
			DashboardReports.TravelerCmemoryRpt rpt = new DashboardReports.TravelerCmemoryRpt();           
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
          

            rpt.Parameters["StartDate"].Value = dtPick.FromDate; 
            rpt.Parameters["EndDate"].Value = dtPick.ToDate; 
          
            rpt.Parameters["Threshold"].Value = TCutoffTextBox.Text;
            rpt.Parameters["ServerType"].Value = srvtype;
            ReportViewer1.Report = rpt;
            ReportViewer1.DataBind();
        }

        protected void ResetButton_Click(object sender, EventArgs e)
        {
           
            this.ServerListBox.UnselectAll();
			DashboardReports.TravelerCmemoryRpt rpt = new DashboardReports.TravelerCmemoryRpt();
                
            rpt.Parameters["ServerName"].Value = "";
          
            rpt.Parameters["Threshold"].Value = "";
            TCutoffTextBox.Text = "";
          
            this.ReportViewer1.Report= rpt;
            this.ReportViewer1.DataBind();
        }

        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            FillReport();
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

            Response.Redirect("~/Configurator/Reports.aspx?M=" + Request.QueryString["M"], false);
            Context.ApplicationInstance.CompleteRequest();
        }
       
    }
}