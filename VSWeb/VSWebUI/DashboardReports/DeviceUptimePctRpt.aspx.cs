using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace VSWebUI.DashboardReports
{
    public partial class DeviceUptimePctRpt : System.Web.UI.Page
    {
        string selectedType = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            PopulateReport();
            if (!IsPostBack)
            {
                fillcombo("");
                fillservertypelist();
            }
            else
            {
                fillcombo(selectedType);
            }
        }

        public void PopulateReport()
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
            //2/2/2015 NS added for VSPLUS-1370
            if (this.ServerTypeFilterListBox.SelectedItems.Count > 0)
            {
                selectedType = "";
                for (int i = 0; i < this.ServerTypeFilterListBox.SelectedItems.Count; i++)
                {
                    selectedType += "'" + this.ServerTypeFilterListBox.SelectedItems[i].Text + "'" + ",";
                }
                try
                {
                    selectedType = selectedType.Substring(0, selectedType.Length - 1);
                }
                catch
                {
                    selectedType = "";     // throw ex; 
                }
                finally { }
            }
            string date;
            string date2;
            //11/21/2014 NS modified
            if (dtPick.ToDate == "")
            {
                date = DateTime.Now.ToString();
            }
            else
            {
                date = dtPick.ToDate;
            }
            if (dtPick.FromDate == "")
            {
                date2 = (DateTime.Now.AddDays(-7)).ToString();
            }
            else
            {
                date2 = dtPick.FromDate;
            }
            DateTime dt = Convert.ToDateTime(date);
            DateTime dt2 = Convert.ToDateTime(date2);
            DashboardReports.DeviceUptimePctXtraRpt report = new DashboardReports.DeviceUptimePctXtraRpt();
            report.Parameters["StartDateVal"].Value = dt2;
            report.Parameters["EndDateVal"].Value = dt;
            report.Parameters["ServerName"].Value = selectedServer;
            //2/4/2015 NS added for VSPLUS-1370
            report.Parameters["ServerType"].Value = selectedType;
            this.ReportViewer1.Report = report;
            this.ReportViewer1.DataBind();
        }

        public void fillcombo(string sType)
        {
            DataTable dt = new DataTable();
            dt = VSWebBL.ReportsBL.ReportsBL.Ins.GetDeviceUptimePctServerNames(sType);
            ServerListFilterListBox.DataSource = dt;
            ServerListFilterListBox.TextField = "ServerName";
            ServerListFilterListBox.ValueField = "ServerName";
            ServerListFilterListBox.DataBind();
        }

        public void fillservertypelist()
        {
            DataTable dt = new DataTable();
            dt = VSWebBL.ReportsBL.ReportsBL.Ins.GetDeviceUptimePctServerTypes();
            ServerTypeFilterListBox.DataSource = dt;
            ServerTypeFilterListBox.TextField = "ServerType";
            ServerTypeFilterListBox.ValueField = "ServerType";
            ServerTypeFilterListBox.DataBind();
        }

        protected void ReptBtn_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Configurator/Reports.aspx?M=" + Request.QueryString["M"], false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
            Context.ApplicationInstance.CompleteRequest();
        }

        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            PopulateReport();
        }

        protected void ServerListResetButton_Click(object sender, EventArgs e)
        {
            this.ServerListFilterListBox.UnselectAll();
            //2/4/2015 NS added for VSPLUS-1370
            this.ServerTypeFilterListBox.UnselectAll();
            fillcombo("");
            PopulateReport();
        }
    }
}