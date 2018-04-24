using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace VSWebUI.DashboardReports
{
    public partial class DailyMemoryUsedRpt : System.Web.UI.Page
    {
        //2/2/2015 NS added for VSPLUS-1370 
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
            dt = VSWebBL.ReportsBL.ReportsBL.Ins.getDailyMemoryUsedBL(sType);
            /*
            ServerListFilterComboBox.DataSource = dt;
            ServerListFilterComboBox.TextField = "servername";
            ServerListFilterComboBox.ValueField = "servername";
            ServerListFilterComboBox.DataBind();
             */
            ServerListFilterListBox.DataSource = dt;
            ServerListFilterListBox.TextField = "servername";
            ServerListFilterListBox.ValueField = "servername";
            ServerListFilterListBox.DataBind();
           
        }

        public void fillservertypelist()
        {
            DataTable dt = new DataTable();
            dt = VSWebBL.ReportsBL.ReportsBL.Ins.fillServerTypeList2("Mem.PercentAvailable");
            ServerTypeFilterListBox.DataSource = dt;
            ServerTypeFilterListBox.TextField = "ServerType";
            ServerTypeFilterListBox.ValueField = "ServerType";
            ServerTypeFilterListBox.DataBind();
        }

        public void Report()
        {
            string selectedServer = "";
            string date;
            /*
            if (this.ServerListFilterComboBox.SelectedIndex >= 0)
            {
                selectedServer = this.ServerListFilterComboBox.SelectedItem.Value.ToString();
            }
             */
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
            //10/22/2013 NS modified - added jQuery month/year control
            /*
            if (this.DateParamEdit.Text == "")
            {
                date = DateTime.Now.ToString();
            }
            else
            {
                date = this.DateParamEdit.Value.ToString();
            }
             */
            if (startDate.Value.ToString() == "")
            {
                date = DateTime.Now.ToString();
            }
            else
            {
                date = startDate.Value.ToString();
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
            DateTime dt = Convert.ToDateTime(date);
            DashboardReports.DailyMemoryUsedXtraRpt report = new DashboardReports.DailyMemoryUsedXtraRpt();
            report.Parameters["DateM"].Value = dt.Month;
            report.Parameters["DateY"].Value = dt.Year;
            System.Globalization.DateTimeFormatInfo mfi = new System.Globalization.DateTimeFormatInfo();
            string strMonthName = mfi.GetMonthName(dt.Month).ToString() + ", " + dt.Year.ToString();
            report.Parameters["MonthYear"].Value = strMonthName;
            report.Parameters["ServerName"].Value = selectedServer;
            //2/2/2015 NS added for VSPLUS-1370
            report.Parameters["ServerType"].Value = selectedType;
            this.ReportViewer1.Report = report;
            this.ReportViewer1.DataBind();
        }
        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            Report();
        }

        protected void ServerListResetButton_Click(object sender, EventArgs e)
        {
            //this.ServerListFilterComboBox.SelectedIndex = -1;
            //2/2/2015 NS added for VSPLUS-1370 
            this.ServerTypeFilterListBox.UnselectAll();
            this.ServerListFilterListBox.UnselectAll();
            fillcombo("");
            DashboardReports.DailyMemoryUsedXtraRpt report = new DashboardReports.DailyMemoryUsedXtraRpt();
            string date = DateTime.Now.ToString("M/d/yyyy");
            DateTime dt = Convert.ToDateTime(date);
            report.Parameters["DateM"].Value = dt.Month;
            report.Parameters["DateY"].Value = dt.Year;
            report.Parameters["ServerName"].Value = "";
            report.Parameters["ServerType"].Value = "";
            this.ReportViewer1.Report = report;
            this.ReportViewer1.DataBind();
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

        protected void ViewTabButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/DashboardReports/DailyMemoryUsedTab.aspx?M=" + Request.QueryString["M"], false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
            Context.ApplicationInstance.CompleteRequest();
        }

    }
}