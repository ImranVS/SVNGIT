using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace VSWebUI.DashboardReports
{
    public partial class DominoAccessBrowserRpt : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string selectedServer = "";
            string selectedStat = "";
            fillcombo("Domino");
            fillcombosrv();
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
            if (this.StatComboBox.SelectedIndex >= 0)
            {
                selectedStat = this.StatComboBox.SelectedItem.Value.ToString();
            }
            else if (this.StatComboBox.Items.Count > 0)
            {
                selectedStat = this.StatComboBox.Items[0].Value.ToString();
                StatComboBox.SelectedIndex = 0;
            }
            Report(selectedServer,selectedStat);
        }

        //26/4/2016 sowmya added for VSPLUS-2881
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

        public void fillcombosrv()
        {
            DataTable dt = new DataTable();
            dt = VSWebBL.ReportsBL.ReportsBL.Ins.GetUserCountServers("");
            ServerListFilterListBox.DataSource = dt;
            ServerListFilterListBox.TextField = "ServerName";
            ServerListFilterListBox.ValueField = "ServerName";
            ServerListFilterListBox.DataBind();
        }

        public void fillcombo(string stattype)
        {
            DataTable dt = new DataTable();
            dt = VSWebBL.ReportsBL.ReportsBL.Ins.GetDominoStatList(stattype,"Domino.");
            StatComboBox.DataSource = dt;
            StatComboBox.TextField = "StatName";
            StatComboBox.ValueField = "StatName";
            StatComboBox.DataBind();
        }

        private void Report(string selectedServer,string selectedStat)
        {
            DashboardReports.DominoAccessBrowserXtraRpt report = new DashboardReports.DominoAccessBrowserXtraRpt();
            report.Parameters["ServerName"].Value = selectedServer;
            report.Parameters["StatName"].Value = selectedStat;
            report.Parameters["StartDate"].Value = dtPick.FromDate;
            report.Parameters["EndDate"].Value = dtPick.ToDate;
            report.CreateDocument();
            ASPxDocumentViewer1.Report = report;
            ASPxDocumentViewer1.DataBind();
        }

        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            string selectedServer = "";
            string selectedStat = "";
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
            if (this.StatComboBox.SelectedIndex >= 0)
            {
                selectedStat = this.StatComboBox.SelectedItem.Value.ToString();
            }
            Report(selectedServer, selectedStat);
        }

        protected void ServerListResetButton_Click(object sender, EventArgs e)
        {
            this.ServerListFilterListBox.UnselectAll();
            string selectedServer = "";
            string selectedStat = "";
            if (this.StatComboBox.SelectedIndex >= 0)
            {
                selectedStat = this.StatComboBox.SelectedItem.Value.ToString();
            }
            Report(selectedServer, selectedStat);
        }
    }
}