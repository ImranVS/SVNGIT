using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace VSWebUI.DashboardReports
{
    public partial class ExchangeUserCountRpt : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                fillUserTypeCombo();
                fillServerCombo();
            }
            Report();
        }

        public void fillUserTypeCombo()
        {
            DataTable dt = new DataTable();
            dt = VSWebBL.ReportsBL.ReportsBL.Ins.GetMSUserCountTypes(RptTypeRadioButtonList.SelectedItem.Value.ToString());
            if (dt.Rows.Count > 0)
            {
                UserTypeComboBox.SelectedIndex = 0;
                UserTypeComboBox.TextField = "StatName";
                UserTypeComboBox.ValueField = "StatName";
                UserTypeComboBox.DataSource = dt;
                UserTypeComboBox.DataBind();
            }
        }

        public void fillServerCombo()
        {
            DataTable d = new DataTable();
            string sType = "";
            string uType = "";
            sType = RptTypeRadioButtonList.SelectedItem.Value.ToString();
            if (UserTypeComboBox.Items.Count > 0)
            {
                uType = UserTypeComboBox.SelectedItem.Value.ToString();
                d = VSWebBL.ReportsBL.ReportsBL.Ins.GetMSServers(sType, uType);
                ServerListBox.DataSource = d;
                ServerListBox.TextField = "ServerName";
                ServerListBox.ValueField = "ServerName";
                ServerListBox.DataBind();
            }
        }

        protected void SubmitBtn_Click(object sender, EventArgs e)
        {
            Report();
        }

        private void Report()
        {
            string selectedStatType = "";
            string selectedUserType = "";
            string selectedServer = "";
            if (UserTypeComboBox.SelectedIndex != -1)
            {
                selectedUserType = UserTypeComboBox.SelectedItem.Value.ToString();
            }
            if (RptTypeRadioButtonList.SelectedIndex != -1)
            {
                selectedStatType = RptTypeRadioButtonList.SelectedItem.Value.ToString();
            }
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
            DashboardReports.ExchangeUserCountXtraRpt report = new DashboardReports.ExchangeUserCountXtraRpt();
            report.Parameters["ServerName"].Value = selectedServer;
            report.Parameters["StatType"].Value = selectedStatType;
            report.Parameters["UserType"].Value = selectedUserType;
            report.Parameters["StartDate"].Value = dtPick.FromDate; // StartDateEdit.Text;
            report.Parameters["EndDate"].Value = dtPick.ToDate;
            report.CreateDocument();
            ASPxDocumentViewer1.Report = report;
            ASPxDocumentViewer1.DataBind();
        }

        protected void ResetButton_Click(object sender, EventArgs e)
        {
            ServerListBox.UnselectAll();
            DashboardReports.ExchangeUserCountXtraRpt report = new DashboardReports.ExchangeUserCountXtraRpt();
            report.Parameters["ServerName"].Value = "";
            ASPxDocumentViewer1.Report = report;
            ASPxDocumentViewer1.DataBind();
        }
    }
}