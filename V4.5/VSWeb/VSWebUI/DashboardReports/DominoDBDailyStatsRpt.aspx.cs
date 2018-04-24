using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace VSWebUI.DashboardReports
{
    public partial class DominoDailyStatsRpt : System.Web.UI.Page
    {
        public void fillcombosrv()
        {
            DataTable d = new DataTable();
            d = VSWebBL.ReportsBL.ReportsBL.Ins.GetDominoDBStatsSrv();
            ServerComboBox.DataSource = d;
            ServerComboBox.TextField = "Server";
            ServerComboBox.ValueField = "Server";
            ServerComboBox.DataBind();
        }

        public void fillcombofolder(string selectedServer)
        {
            DataTable d = new DataTable();
            d = VSWebBL.ReportsBL.ReportsBL.Ins.GetDominoDBStatsFolder(selectedServer);
            FolderComboBox.DataSource = d;
            FolderComboBox.TextField = "Folder";
            FolderComboBox.ValueField = "Folder";
            FolderComboBox.DataBind();
        }

        public void FillReport()
        {
            DashboardReports.DominoDBDailyStatsXtraRpt report = new DashboardReports.DominoDBDailyStatsXtraRpt();
            string selectedServer = "";
            if (this.ServerComboBox.SelectedIndex >= 0)
            {
                selectedServer = this.ServerComboBox.SelectedItem.Value.ToString();
            }
            string selectedFolder = "";
            if (selectedServer != "")
            {
                if (this.FolderComboBox.SelectedIndex >= 0)
                {
                    selectedFolder = this.FolderComboBox.SelectedItem.Value.ToString();
                }
            }
            report.Parameters["ServerName"].Value = selectedServer;
            report.Parameters["FolderName"].Value = selectedFolder;
            report.CreateDocument();
            ASPxDocumentViewer1.Report = report;
            ASPxDocumentViewer1.DataBind();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                fillcombosrv();
            }
            FillReport();
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
            this.ServerComboBox.SelectedIndex = -1;
            this.FolderComboBox.SelectedIndex = -1;
            DashboardReports.DominoDBDailyStatsXtraRpt report = new DashboardReports.DominoDBDailyStatsXtraRpt();
            string selectedServer = "";
            string selectedFolder = "";
            this.FolderComboBox.Visible = false;
            this.SubmitButton.Visible = false;
            report.Parameters["ServerName"].Value = selectedServer;
            report.Parameters["FolderName"].Value = selectedFolder;
            report.CreateDocument();
            ASPxDocumentViewer1.Report = report;
            ASPxDocumentViewer1.DataBind();
        }

        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            FillReport();
        }

        protected void ServerComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedServer = "";
            if (this.ServerComboBox.SelectedIndex >= 0)
            {
                selectedServer = this.ServerComboBox.SelectedItem.Value.ToString();
                fillcombofolder(selectedServer);
                FolderComboBox.Visible = true;
                SubmitButton.Visible = true;
            }
        }

    }
}