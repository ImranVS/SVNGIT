using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace VSWebUI.DashboardReports
{
    public partial class IBMConnectionFileRpt : System.Web.UI.Page
    {
       
        protected void Page_Load(object sender, EventArgs e)
        {
            Report();

            if (!IsPostBack)
            {
                fillfiletypecombo();
                fillservercombo();         
            }        
        }

        public void fillservercombo()
        {
            string FType = "";

            if (FileTypeComboBox.Items.Count > 0)
            {

                DataTable dt = new DataTable();
                FType = FileTypeComboBox.SelectedItem.Value.ToString();
                dt = VSWebBL.ReportsBL.ReportsBL.Ins.GetIBMConnectionsServerlist(FType);
                ServerListFilterListBox.DataSource = dt;
                ServerListFilterListBox.TextField = "ServerName";
                ServerListFilterListBox.ValueField = "ServerName";
                ServerListFilterListBox.DataBind();
            }
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

        protected void ResetButton_Click(object sender, EventArgs e)
        {
            
                if (Request.UrlReferrer != null)
                   Response.Redirect(Request.RawUrl);         
        }

        public void Report()
        {
            string selectedServer = "";
            string selectedFileType = "";
            if (FileTypeComboBox.SelectedIndex != -1)
            {
                selectedFileType = FileTypeComboBox.SelectedItem.Value.ToString();
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
                    selectedServer = ""; 
                }
                finally { }
            }
            string strfrom = "";
            string strto = "";         
            strfrom = dtPick.FromDate;
            strto = dtPick.ToDate;
            DateTime dtfrom = DateTime.Parse(strfrom);
            DateTime dtto = DateTime.Parse(strto);
            DashboardReports.IBMConnectionFilextraRpt report = new DashboardReports.IBMConnectionFilextraRpt();
            report.Parameters["ServerName"].Value = selectedServer;
            report.Parameters["DateFrom"].Value = strfrom;
            report.Parameters["DateTo"].Value = strto;
            report.Parameters["FileType"].Value = selectedFileType;
            this.ReportViewer1.Report = report;
            this.ReportViewer1.DataBind();

        }
        public void fillfiletypecombo()
        {
            DataTable dt = new DataTable();
            dt = VSWebBL.ReportsBL.ReportsBL.Ins.Getfiletypes("NUM_OF_FILES_FILES_CREATED_YESTERDAY", "NUM_OF_FILES_FILES_UPDATED_YESTERDAY", "NUM_OF_FILES_FILES_DOWNLOADED_YESTERDAY", "NUM_OF_FILES_FILES_REVISIONED_YESTERDAY");
            if (dt.Rows.Count > 0)
            {
                FileTypeComboBox.SelectedIndex = 0;
                FileTypeComboBox.TextField = "Userstatname";
                FileTypeComboBox.ValueField = "statname";
                FileTypeComboBox.DataSource = dt;
                FileTypeComboBox.DataBind();
            }
        }

        protected void FileTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Report();
            fillservercombo();
        }

        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            Report();
        }
    }
}