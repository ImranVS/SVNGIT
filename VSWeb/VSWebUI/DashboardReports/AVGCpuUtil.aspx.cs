using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace VSWebUI
{
    public partial class WebForm6 : System.Web.UI.Page
    {
        string srvtype;
        protected void Page_Load(object sender, EventArgs e)
        {
            //2/11/2015 NS added for VSPLUS-1428
            //2/24/2015 NS modified for VSPLUS-1499
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
            //12/17/2013 NS modified
            //d = VSWebBL.ReportsBL.ReportsBL.Ins.getDominoSummaryStats();
            d = VSWebBL.ReportsBL.XsdBL.Ins.getServersForCPUUtil(sType);
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
            DashboardReports.AVGCPURpt rpt = new DashboardReports.AVGCPURpt();           
            string selectedServer = "";
            //8/5/2013 NS modified
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
            //11/21/2014 NS modified 
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
            rpt.Parameters["EndDate"].Value = dtPick.ToDate; //EndDateEdit.Text;
            //7/14/2014 NS added
            rpt.Parameters["Threshold"].Value = TCutoffTextBox.Text;
            rpt.Parameters["ServerType"].Value = srvtype;
            ReportViewer1.Report = rpt;
            ReportViewer1.DataBind();
        }

        protected void ResetButton_Click(object sender, EventArgs e)
        {
            //this.ServerComboBox.SelectedIndex = -1;
            this.ServerListBox.UnselectAll();
            DashboardReports.AVGCPURpt rpt = new DashboardReports.AVGCPURpt();
                
            rpt.Parameters["ServerName"].Value = "";
            //11/21/2014 NS commented out
            //rpt.Parameters["StartDate"].Value = DateTime.Now.ToString();
            //rpt.Parameters["EndDate"].Value = DateTime.Now.ToString();
            //7/14/2014 NS added
            rpt.Parameters["Threshold"].Value = "";
            TCutoffTextBox.Text = "";
            //ServerComboBox.Text = "";
            //11/21/2014 NS commented out 
            //StartDateEdit.Text = DateTime.Now.ToShortDateString();
            //EndDateEdit.Text = DateTime.Now.ToShortDateString();
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

            Response.Redirect("~/Configurator/Reports.aspx?M=" + Request.QueryString["M"], false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
            Context.ApplicationInstance.CompleteRequest();
        }
       
    }
}