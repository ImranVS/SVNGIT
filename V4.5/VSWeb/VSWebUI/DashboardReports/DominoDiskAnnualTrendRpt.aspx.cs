using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace VSWebUI.DashboardReports
{
    public partial class DominoDiskAnnualTrend : System.Web.UI.Page
    {
        //2/6/2015 NS added for VSPLUS-1370 
        string selectedType = "";
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
                fillcombo(selectedType);
            }
        }

        public void fillcombo(string sType)
        {
            DataTable dt = new DataTable();
            dt = VSWebBL.ReportsBL.ReportsBL.Ins.getDominoSummaryStats(sType);
            ServerListFilterListBox.DataSource = dt;
            ServerListFilterListBox.TextField = "ServerName";
            ServerListFilterListBox.ValueField = "ServerName";
            ServerListFilterListBox.DataBind();
            dt = VSWebBL.ReportsBL.ReportsBL.Ins.getDominoSummaryDiskStats(sType);
            DiskListFilterListBox.DataSource = dt;
            DiskListFilterListBox.TextField = "DiskName";
            DiskListFilterListBox.ValueField = "DiskName";
            DiskListFilterListBox.DataBind();
        }

        public void fillservertypelist()
        {
            DataTable dt = new DataTable();
            dt = VSWebBL.ReportsBL.ReportsBL.Ins.SrvDiskFreeSpaceServerTypes();
            ServerTypeFilterListBox.DataSource = dt;
            ServerTypeFilterListBox.TextField = "ServerType";
            ServerTypeFilterListBox.ValueField = "ServerType";
            ServerTypeFilterListBox.DataBind();
        }

        public void Report()
        {
            string selectedServer = "";
            string selectedDisk = "";
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
            if (this.DiskListFilterListBox.SelectedItems.Count > 0)
            {
                selectedDisk = "";
                for (int i = 0; i < this.DiskListFilterListBox.SelectedItems.Count; i++)
                {
                    selectedDisk += "'" + this.DiskListFilterListBox.SelectedItems[i].Text + "'" + ",";
                }
                try
                {
                    selectedDisk = selectedDisk.Substring(0, selectedDisk.Length - 1);
                }
                catch
                {
                    selectedDisk = "";
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
            string date = DateTime.Now.ToString();
            //10/24/2013 NS modified - added jQuery month/year control
            /*
            if (this.DateParamEdit.Text == "")
            {
                date = DateTime.Now.ToString();
                this.DateParamEdit.Date = Convert.ToDateTime(date);
            }
            else
            {
                date = this.DateParamEdit.Value.ToString();
            }
             */
            if (startDate.Value.ToString() == "")
            {
                date = DateTime.Now.ToString();
                startDate.Value = (Convert.ToDateTime(date)).Year.ToString();
            }
            else
            {
                date = startDate.Value.ToString();
            }
            //DateTime dt = Convert.ToDateTime(date);
            DashboardReports.DominoDiskAnnualTrendXtraRpt report = new DashboardReports.DominoDiskAnnualTrendXtraRpt();
            //report.Parameters["DateY"].Value = dt.Year;
            report.Parameters["DateY"].Value = date;
            report.Parameters["ServerName"].Value = selectedServer;
            report.Parameters["DiskName"].Value = selectedDisk;
            //2/6/2015 NS added for VSPLUS-1370
            report.Parameters["ServerType"].Value = selectedType;
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

        protected void ServerListResetButton_Click(object sender, EventArgs e)
        {
            this.ServerListFilterListBox.UnselectAll();
            this.DiskListFilterListBox.UnselectAll();
            //2/6/2015 NS added for VSPLUS-1370 
            this.ServerTypeFilterListBox.UnselectAll();
            fillcombo("");
            DashboardReports.DominoDiskAnnualTrendXtraRpt report = new DashboardReports.DominoDiskAnnualTrendXtraRpt();
            string date = DateTime.Now.ToString("M/d/yyyy");
            DateTime dt = Convert.ToDateTime(date);
            report.Parameters["DateY"].Value = dt.Year;
            report.Parameters["ServerName"].Value = "";
            report.Parameters["DiskName"].Value = "";
            report.Parameters["ServerType"].Value = "";
            report.CreateDocument();
            ASPxDocumentViewer1.Report = report;
            ASPxDocumentViewer1.DataBind();
        }

        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            Report();
        }

        protected void ReptBtn_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Configurator/Reports.aspx?M=" + Request.QueryString["M"], false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
            Context.ApplicationInstance.CompleteRequest();
        }
    }
}