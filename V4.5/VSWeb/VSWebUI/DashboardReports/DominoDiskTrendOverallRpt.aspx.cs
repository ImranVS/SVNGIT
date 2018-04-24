using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace VSWebUI.DashboardReports
{
    public partial class DominoDiskTrendOverallRpt : System.Web.UI.Page
    {
        public bool exactmatch;
        //2/2/2015 NS added for VSPLUS-1370 
        string selectedType = "";
        string selectedTypeSQL = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            Report();
            if (!IsPostBack)
            {
                fillcombo("");
                fillservertypelist();
            }
            else
            {
                fillcombo(selectedTypeSQL);
            }
        }

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
            int daysrem = -1;
            string selectedServer = "";
            string selectedServerList = "";
            exactmatch = false;
            bool isSummary = true;
            if (this.ServerFilterTextBox.Text != "")
            {
                selectedServer = this.ServerFilterTextBox.Text;
            }
            if (this.ServerListFilterListBox.SelectedItems.Count > 0)
            {
                selectedServer = "";
                for (int i = 0; i < this.ServerListFilterListBox.SelectedItems.Count; i++)
                {
                    selectedServer += this.ServerListFilterListBox.SelectedItems[i].Text + ",";
                    selectedServerList += "'" + this.ServerListFilterListBox.SelectedItems[i].Text + "'" + ",";
                }
                exactmatch = true;
                try
                {
                    selectedServer = selectedServer.Substring(0, selectedServer.Length - 1);
                    selectedServerList = selectedServerList.Substring(0, selectedServerList.Length - 1);
                }
                catch
                {
                    selectedServer = "";     // throw ex; 
                    selectedServerList = "";
                    exactmatch = false;
                }
                finally { }
            }
            //2/2/2015 NS added for VSPLUS-1370
            if (this.ServerTypeFilterListBox.SelectedItems.Count > 0)
            {
                selectedType = "";
                selectedTypeSQL = "";
                for (int i = 0; i < this.ServerTypeFilterListBox.SelectedItems.Count; i++)
                {
                    selectedType += this.ServerTypeFilterListBox.SelectedItems[i].Text + ",";
                    selectedTypeSQL += "'" + this.ServerTypeFilterListBox.SelectedItems[i].Text + "'" + ",";
                }
                try
                {
                    selectedType = selectedType.Substring(0, selectedType.Length - 1);
                    selectedTypeSQL = selectedTypeSQL.Substring(0, selectedTypeSQL.Length - 1);
                }
                catch
                {
                    selectedType = "";     // throw ex; 
                    selectedTypeSQL = "";
                }
                finally { }
            }
            if (this.NumberDaysTextBox.Text.ToString() != "")
            {
                daysrem = Convert.ToInt32(this.NumberDaysTextBox.Value.ToString());
            }
            DashboardReports.DominoDiskAvgXtraRpt report = new DashboardReports.DominoDiskAvgXtraRpt();
            report.Parameters["ServerName"].Value = selectedServer;
            report.Parameters["ServerNameSQL"].Value = selectedServerList;
            report.Parameters["DaysRem"].Value = daysrem;
            report.Parameters["ExactMatch"].Value = exactmatch;
            report.Parameters["IsSummary"].Value = isSummary;
            //2/2/2015 NS added for VSPLUS-1370
            report.Parameters["ServerType"].Value = selectedType;
            report.Parameters["ServerTypeSQL"].Value = selectedTypeSQL;
            report.CreateDocument();
            ASPxDocumentViewer1.Report = report;
            ASPxDocumentViewer1.DataBind();
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

        protected void ResetButton_Click(object sender, EventArgs e)
        {
            bool isSummary = true;
            this.ServerFilterTextBox.Text = "";
            this.ServerFilterTextBox.Value = "";
            this.NumberDaysTextBox.Text = "";
            this.NumberDaysTextBox.Value = "";
            this.ServerListFilterListBox.UnselectAll();
            //2/2/2015 NS added for VSPLUS-1370 
            this.ServerTypeFilterListBox.UnselectAll();
            fillcombo("");
            DashboardReports.DominoDiskAvgXtraRpt report = new DashboardReports.DominoDiskAvgXtraRpt();
            report.Parameters["ServerName"].Value = "";
            report.Parameters["ServerNameSQL"].Value = "";
            report.Parameters["DaysRem"].Value = "";
            report.Parameters["IsSummary"].Value = isSummary;
            report.Parameters["ServerType"].Value = "";
            report.Parameters["ServerTypeSQL"].Value = "";
            report.CreateDocument();
            ASPxDocumentViewer1.Report = report;
            ASPxDocumentViewer1.DataBind();
        }

        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            Report();
        }
    }
}