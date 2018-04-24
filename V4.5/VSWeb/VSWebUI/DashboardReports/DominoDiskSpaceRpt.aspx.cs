using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace VSWebUI.DashboardReports
{
    public partial class DominoDiskSpaceRpt : System.Web.UI.Page
    {
        //2/6/2015 NS added for VSPLUS-1370 
        string selectedType = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            //5/4/2015 NS modified for VSPLUS-1707
            Report();
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

        //5/4/2015 NS added for VSPLUS-1707
        public void fillcombo(string sType)
        {
            DataTable dt = new DataTable();
            dt = VSWebBL.ReportsBL.ReportsBL.Ins.fillDominoDiskAvgConsumption(sType);
            ServerListFilterListBox.DataSource = dt;
            ServerListFilterListBox.TextField = "ServerName";
            ServerListFilterListBox.ValueField = "ServerName";
            ServerListFilterListBox.DataBind();
        }

        public void fillservertypelist()
        {
            DataTable dt = new DataTable();
            dt = VSWebBL.ReportsBL.ReportsBL.Ins.fillDominoDiskAvgConsumptionServerTypes();
            ServerTypeFilterListBox.DataSource = dt;
            ServerTypeFilterListBox.TextField = "ServerType";
            ServerTypeFilterListBox.ValueField = "ServerType";
            ServerTypeFilterListBox.DataBind();
        }

        public void Report()
        {
            string selectedServer = "";
            if (this.ServerFilterTextBox.Text != "")
            {
                selectedServer = this.ServerFilterTextBox.Text;
            }
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
            //2/6/2015 NS added for VSPLUS-1370
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
            DashboardReports.DominoDiskSpaceXtraRpt report = new DashboardReports.DominoDiskSpaceXtraRpt();
            report.Parameters["ServerName"].Value = selectedServer;
            //2/6/2015 NS added for VSPLUS-1370
            report.Parameters["ServerType"].Value = selectedType;
            report.CreateDocument();
            ASPxDocumentViewer1.Report = report;
            ASPxDocumentViewer1.DataBind();
        }

        protected void ReptBtn_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Configurator/Reports.aspx?M=" + Request.QueryString["M"]);
        }

        protected void ServerListResetButton_Click(object sender, EventArgs e)
        {
            this.ServerListFilterListBox.UnselectAll();
            this.ServerTypeFilterListBox.UnselectAll();
            fillcombo("");
            DashboardReports.DominoDiskSpaceXtraRpt report = new DashboardReports.DominoDiskSpaceXtraRpt();
            report.Parameters["ServerName"].Value = "";
            report.Parameters["ServerType"].Value = "";
            report.CreateDocument();
            ASPxDocumentViewer1.Report = report;
            ASPxDocumentViewer1.DataBind();
        }
    }
}