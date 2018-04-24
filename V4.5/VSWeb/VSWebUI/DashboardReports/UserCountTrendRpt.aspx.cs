using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace VSWebUI.DashboardReports
{
    public partial class UserCountTrendRpt : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //7/29/2015 NS added for VSPLUS-2023
            string selectedServer = "";
            fillcombo();
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
            Report(selectedServer);
            
        }

        protected void SubmitButton_Click(object sender, EventArgs e)
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
            Report(selectedServer);
        }

        private void Report(string selectedServer)
        {
            DashboardReports.UserCountTrendXtraRpt report = new DashboardReports.UserCountTrendXtraRpt();
            //7/29/2015 NS added for VSPLUS-2023
            report.Parameters["ServerName"].Value = selectedServer;
            report.Parameters["TypeVal"].Value = TypeComboBox.SelectedItem.Text;
            report.Parameters["StartDate"].Value = dtPick.FromDate;
            report.Parameters["EndDate"].Value = dtPick.ToDate;
            report.CreateDocument();
            ASPxDocumentViewer1.Report = report;
            ASPxDocumentViewer1.DataBind();
        }

        //7/29/2015 NS added for VSPLUS-2023
        public void fillcombo()
        {
            DataTable dt = new DataTable();
            dt = VSWebBL.ReportsBL.ReportsBL.Ins.GetUserCountServers("");
            ServerListFilterListBox.DataSource = dt;
            ServerListFilterListBox.TextField = "ServerName";
            ServerListFilterListBox.ValueField = "ServerName";
            ServerListFilterListBox.DataBind();
        }

        //7/29/2015 NS added for VSPLUS-2023
        protected void ServerListResetButton_Click(object sender, EventArgs e)
        {
            this.ServerListFilterListBox.UnselectAll();
            Report("");
        }
    }
}