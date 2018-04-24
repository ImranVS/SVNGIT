using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Drawing;
using System.IO;

namespace VSWebUI.DashboardReports
{
    public partial class ServerAvailabilityRpt : System.Web.UI.Page
    {
        public bool exactmatch;
        //1/27/2016 NS added for VSPLUS-2533
        private static ServerAvailabilityRpt _self = new ServerAvailabilityRpt();

        public static ServerAvailabilityRpt Ins
        {
            get
            {
                return _self;
            }
        }
        DashboardReports.ServerAvailabilityXtraRpt report;

        public DashboardReports.ServerAvailabilityXtraRpt GetRpt()
        {
            string date;
            string srvName = "";
            bool exactmatch = false;
            string srvType = "";
            date = DateTime.Now.ToString();
            DateTime dt = Convert.ToDateTime(date);
            dt.AddDays(-1);
            try
            {
                report = new DashboardReports.ServerAvailabilityXtraRpt();
                report.Parameters["DateM"].Value = dt.Month;
                report.Parameters["DateY"].Value = dt.Year;
                report.Parameters["DateD"].Value = dt.Day;
                System.Globalization.DateTimeFormatInfo mfi = new System.Globalization.DateTimeFormatInfo();
                string strMonthName = mfi.GetMonthName(dt.Month).ToString() + ", " + dt.Year.ToString();
                report.Parameters["MonthYear"].Value = strMonthName;
                report.Parameters["ServerName"].Value = srvName;
                report.Parameters["ServerNameSQL"].Value = srvName;
                report.Parameters["ExactMatch"].Value = exactmatch;
                report.Parameters["DownMin"].Value = "";
                report.Parameters["ServerType"].Value = srvType;
                report.Parameters["ServerTypeSQL"].Value = srvType;
                report.SetReport(report);
            }
            catch (Exception ex)
            {
                WriteServiceHistoryEntry(DateTime.Now.ToString() + " The following error has occurred in GetRpt: " + ex.Message);
            }
            return report;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            /*select devicename,SUM(statvalue) downminutes,
((dbo.CalcNumDaysinMonth(GETDATE()))*24*60)-SUM(statvalue) upminutestotal,
MonthNumber,YearNumber from dbo.DeviceUpTimeStats
where StatName='HourlyDownTimeMinutes'
group by DeviceName,MonthNumber,YearNumber
order by YearNumber desc, MonthNumber desc, DeviceName
             */
            string selectedServer = "";
            string selectedServerList = "";
            string date;
            //12/24/2013 NS added
            string downMin = "";
            exactmatch = false;
            //1/30/2015 NS added for VSPLUS-1370 
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
            //12/24/2013 NS added
            downMin = DownFilterTextBox.Text;
            //10/23/2013 NS modified - added jQuery month/year control
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
            }
            else
            {
                date = startDate.Value.ToString();
            }
            //1/30/2015 NS added for VSPLUS-1370
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
            DashboardReports.ServerAvailabilityXtraRpt report = new DashboardReports.ServerAvailabilityXtraRpt();
            report.Parameters["DateM"].Value = dt.Month;
            report.Parameters["DateY"].Value = dt.Year;
            report.Parameters["DateD"].Value = dt.Day;
            System.Globalization.DateTimeFormatInfo mfi = new System.Globalization.DateTimeFormatInfo();
            string strMonthName = mfi.GetMonthName(dt.Month).ToString() + ", " + dt.Year.ToString();
            report.Parameters["MonthYear"].Value = strMonthName;
            report.Parameters["ServerName"].Value = selectedServer;
            report.Parameters["ServerNameSQL"].Value = selectedServerList;
            report.Parameters["ExactMatch"].Value = exactmatch;
            //12/24/2013 NS added
            report.Parameters["DownMin"].Value = downMin;
            //1/30/2015 NS added for VSPLUS-1370
            report.Parameters["ServerType"].Value = selectedType;
            report.Parameters["ServerTypeSQL"].Value = selectedTypeList;
            report.PageColor = Color.Transparent;
            report.CreateDocument();
            ASPxDocumentViewer1.Report = report;
            ASPxDocumentViewer1.DataBind();
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
            dt = VSWebBL.ReportsBL.ReportsBL.Ins.fillDownMinutesServer(sType);
            ServerListFilterListBox.DataSource = dt;
            ServerListFilterListBox.TextField = "ServerName";
            ServerListFilterListBox.ValueField = "ServerName";
            ServerListFilterListBox.DataBind();
        }

        public void fillservertypelist()
        {
            DataTable dt = new DataTable();
            dt = VSWebBL.ReportsBL.ReportsBL.Ins.fillServerTypeList();
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

        protected void ReptBtn_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Configurator/Reports.aspx?M=" + Request.QueryString["M"], false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
            Context.ApplicationInstance.CompleteRequest();
        }

        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            string selectedServer = "";
            string selectedServerList = "";
            string date;
            //12/24/2013 NS added
            string downMin = "";
            //1/30/2015 NS added for VSPLUS-1370 
            string selectedType = "";
            string selectedTypeList = "";
            exactmatch = false;
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
            //12/24/2013 NS added
            downMin = DownFilterTextBox.Text;
            //10/23/2013 NS modified - added jQuery month/year control
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
            }
            else
            {
                date = startDate.Value.ToString();
            }
            //1/30/2015 NS added for VSPLUS-1370
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
            DashboardReports.ServerAvailabilityXtraRpt report = new DashboardReports.ServerAvailabilityXtraRpt();
            report.Parameters["DateM"].Value = dt.Month;
            report.Parameters["DateY"].Value = dt.Year;
            report.Parameters["DateD"].Value = dt.Day;
            System.Globalization.DateTimeFormatInfo mfi = new System.Globalization.DateTimeFormatInfo();
            string strMonthName = mfi.GetMonthName(dt.Month).ToString() + ", " + dt.Year.ToString();
            report.Parameters["MonthYear"].Value = strMonthName;
            report.Parameters["ServerName"].Value = selectedServer;
            report.Parameters["ServerNameSQL"].Value = selectedServerList;
            report.Parameters["ExactMatch"].Value = exactmatch;
            //12/24/2013 NS added
            report.Parameters["DownMin"].Value = downMin;
            //1/30/2015 NS added for VSPLUS-1370
            report.Parameters["ServerType"].Value = selectedType;
            report.Parameters["ServerTypeSQL"].Value = selectedTypeList;
            report.CreateDocument();
            ASPxDocumentViewer1.Report = report;
            ASPxDocumentViewer1.DataBind();
        }

        protected void ResetButton_Click(object sender, EventArgs e)
        {
            this.ServerFilterTextBox.Text = "";
            this.ServerFilterTextBox.Value = "";
            //1/30/2015 NS added for VSPLUS-1370 
            this.ServerTypeFilterListBox.UnselectAll();
            this.ServerListFilterListBox.UnselectAll();
            //12/24/2013 NS added
            this.DownFilterTextBox.Text = "";
            this.DownFilterTextBox.Value = "";
            fillcombo("");
            DashboardReports.ServerAvailabilityXtraRpt report = new DashboardReports.ServerAvailabilityXtraRpt();
            report.Parameters["ServerName"].Value = "";
            //12/24/2013 NS added
            report.Parameters["DownMin"].Value = "";
            //1/30/2015 NS added for VSPLUS-1370
            report.Parameters["ServerType"].Value = "";
            report.Parameters["ServerTypeSQL"].Value = "";
            report.CreateDocument();
            ASPxDocumentViewer1.Report = report;
            ASPxDocumentViewer1.DataBind();
        }
        //1/27/2016 NS added for VSPLUs-2533
        protected void WriteServiceHistoryEntry(string strMsg)
        {
            bool appendMode = true;
            string ServiceLogDestination = AppDomain.CurrentDomain.BaseDirectory.ToString() + "/log_files/ScheduledReports_Service.txt";
            try
            {
                StreamWriter sw = new StreamWriter(ServiceLogDestination, appendMode, System.Text.Encoding.Unicode);
                sw.WriteLine(strMsg);
                sw.Close();
                sw = null;
            }
            catch
            {
            }
            finally
            {
                GC.Collect();
            }
        }
    }
}