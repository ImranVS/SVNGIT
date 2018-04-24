using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace VSWebUI.DashboardReports
{
    public partial class TravelerDeviceSyncRpt : System.Web.UI.Page
    {
        private void LoadReport(string startdate, string enddate, string selectedServer, string selectedServerList)
        {
            DashboardReports.TravelerDeviceSyncXtraRpt report = new DashboardReports.TravelerDeviceSyncXtraRpt();
            report.Parameters["StartDate"].Value = startdate;
            report.Parameters["EndDate"].Value = enddate;
            report.Parameters["ServerName"].Value = selectedServer;
            report.Parameters["ServerNameSQL"].Value = selectedServerList;
            this.ReportViewer1.Report = report;
            this.ReportViewer1.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            string selectedServer = "";
            string selectedServerList = "";
            if (this.ServerListFilterListBox.SelectedItems.Count > 0)
            {
                selectedServer = "";
                for (int i = 0; i < this.ServerListFilterListBox.SelectedItems.Count; i++)
                {
                    selectedServer += this.ServerListFilterListBox.SelectedItems[i].Text + ",";
                    selectedServerList += "'" + this.ServerListFilterListBox.SelectedItems[i].Text + "'" + ",";
                }
                try
                {
                    selectedServer = selectedServer.Substring(0, selectedServer.Length - 1);
                    selectedServerList = selectedServerList.Substring(0, selectedServerList.Length - 1);
                }
                catch
                {
                    selectedServer = "";     // throw ex; 
                    selectedServerList = "";
                }
                finally { }
            }
            //11/24/2014 NS modified
            /*
            DateTime dt = new DateTime();
            if (StartDateEdit.Text == "")
            {
                dt = DateTime.Now;
                dt = dt.AddDays(-7);
                StartDateEdit.Text = dt.ToShortDateString();
            }
            if (EndDateEdit.Text == "")
            {
                EndDateEdit.Text = DateTime.Now.ToShortDateString();
            }
             */
            string startdate = dtPick.FromDate;
            string enddate = dtPick.ToDate;
            LoadReport(startdate, enddate, selectedServer, selectedServerList);
            if (!IsPostBack)
            {
                fillcombo();
            }
        }

        public void fillcombo()
        {
            DataTable dt = new DataTable();
            dt = VSWebBL.ReportsBL.ReportsBL.Ins.GetTravelerDevices();
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

        protected void ReptBtn_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Configurator/Reports.aspx?M=" + Request.QueryString["M"], false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
            Context.ApplicationInstance.CompleteRequest();
        }

        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            string selectedServer = "";
            string selectedServerList = "";
            if (this.ServerListFilterListBox.SelectedItems.Count > 0)
            {
                selectedServer = "";
                for (int i = 0; i < this.ServerListFilterListBox.SelectedItems.Count; i++)
                {
                    selectedServer += this.ServerListFilterListBox.SelectedItems[i].Text + ",";
                    selectedServerList += "'" + this.ServerListFilterListBox.SelectedItems[i].Text + "'" + ",";
                }
                try
                {
                    selectedServer = selectedServer.Substring(0, selectedServer.Length - 1);
                    selectedServerList = selectedServerList.Substring(0, selectedServerList.Length - 1);
                }
                catch
                {
                    selectedServer = "";     // throw ex; 
                    selectedServerList = "";
                }
                finally { }
            }
            //11/24/2014 NS added
            string startdate = dtPick.FromDate;
            string enddate = dtPick.ToDate;
            LoadReport(startdate, enddate, selectedServer, selectedServerList);
        }

        protected void ResetButton_Click(object sender, EventArgs e)
        {
            /*
            DateTime dt = new DateTime();
            dt = DateTime.Now;
            dt = dt.AddDays(-7);
            StartDateEdit.Text = dt.ToShortDateString();
            EndDateEdit.Text = DateTime.Now.ToShortDateString();
             */
            //11/24/2014 NS added
            string startdate = dtPick.FromDate;
            string enddate = dtPick.ToDate;
            ServerListFilterListBox.UnselectAll();
            LoadReport(startdate, enddate,"","");
        }
    }
}