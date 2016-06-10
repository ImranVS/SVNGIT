using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace VSWebUI.DashboardReports
{
    public partial class ResponseTimeTrendRpt : System.Web.UI.Page
    {
        public bool exactmatch;

        protected void Page_Load(object sender, EventArgs e)
        {
            string selectedServer = "";
            string selectedServerList = "";
            string date;
            exactmatch = false;
            string selectedType = "";
            string selectedTypeList = "";
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
            date = DateTime.Now.ToString();
            /*
            if (startDate.Value.ToString() == "")
            {
                date = DateTime.Now.ToString();
            }
            else
            {
                date = startDate.Value.ToString();
            }
             */
            if (this.ServerTypeFilterListBox.SelectedItems.Count > 0)
            {
                selectedType = "";
                for (int i = 0; i < this.ServerTypeFilterListBox.SelectedItems.Count; i++)
                {
                    selectedType += this.ServerTypeFilterListBox.SelectedItems[i].Text + ",";
                    selectedTypeList += "'" + this.ServerTypeFilterListBox.SelectedItems[i].Text + "'" + ",";
                }
                try
                {
                    selectedType = selectedType.Substring(0, selectedType.Length - 1);
                    selectedTypeList = selectedTypeList.Substring(0, selectedTypeList.Length - 1);
                }
                catch
                {
                    selectedType = "";     // throw ex; 
                    selectedTypeList = "";
                }
                finally { }
            }
            DateTime dt = Convert.ToDateTime(date);
            string strfrom = "";
            string strto = "";
            string rpttype = "1";
            strfrom = dtPick.FromDate;
            strto = dtPick.ToDate;
            DateTime dtfrom = DateTime.Parse(strfrom);
            DateTime dtto = DateTime.Parse(strto);
            if ((dtto - dtfrom).Days <= 31)
            {
                rpttype = "0"; //weekly average
            }
            DashboardReports.ResponseTimeTrendXtraRpt report = new DashboardReports.ResponseTimeTrendXtraRpt();
            /*
            report.Parameters["DateM"].Value = dt.Month;
            report.Parameters["DateY"].Value = dt.Year;
            report.Parameters["DateD"].Value = dt.Day;
            System.Globalization.DateTimeFormatInfo mfi = new System.Globalization.DateTimeFormatInfo();
            string strMonthName = mfi.GetMonthName(dt.Month).ToString() + ", " + dt.Year.ToString();
            report.Parameters["MonthYear"].Value = strMonthName;
             */
            report.Parameters["DateFrom"].Value = strfrom;
            report.Parameters["DateTo"].Value = strto;
            report.Parameters["ServerName"].Value = selectedServer;
            report.Parameters["ServerNameSQL"].Value = selectedServerList;
            report.Parameters["ExactMatch"].Value = exactmatch;
            report.Parameters["ServerType"].Value = selectedType;
            report.Parameters["ServerTypeSQL"].Value = selectedTypeList;
            report.Parameters["RptType"].Value = rpttype;
            this.ReportViewer1.Report = report;
            this.ReportViewer1.DataBind();
            if (!IsPostBack)
            {
                fillcombo("");
                fillservertypelist();
            }
            else
            {
                fillcombo(selectedTypeList);
            }
        }

        public void fillcombo(string sType)
        {
            DataTable dt = new DataTable();
            dt = VSWebBL.ReportsBL.ReportsBL.Ins.fillServerListByType(sType);
            ServerListFilterListBox.DataSource = dt;
            ServerListFilterListBox.TextField = "ServerName";
            ServerListFilterListBox.ValueField = "ServerName";
            ServerListFilterListBox.DataBind();
        }

        public void fillservertypelist()
        {
            DataTable dt = new DataTable();
            dt = VSWebBL.ReportsBL.ReportsBL.Ins.fillServerTypeList2("ResponseTime");
            ServerTypeFilterListBox.DataSource = dt;
            ServerTypeFilterListBox.TextField = "ServerType";
            ServerTypeFilterListBox.ValueField = "ServerType";
            ServerTypeFilterListBox.DataBind();
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
            this.ServerFilterTextBox.Text = "";
            this.ServerFilterTextBox.Value = "";
            this.ServerTypeFilterListBox.UnselectAll();
            this.ServerListFilterListBox.UnselectAll();
            fillcombo("");
            DashboardReports.ResponseTimeTrendXtraRpt report = new DashboardReports.ResponseTimeTrendXtraRpt();
            report.Parameters["ServerName"].Value = "";
            report.Parameters["ServerType"].Value = "";
            report.Parameters["ServerTypeSQL"].Value = "";
            this.ReportViewer1.Report = report;
            this.ReportViewer1.DataBind();
        }
    }
}