using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace VSWebUI.DashboardReports
{
    public partial class DeviceUptimeRpt : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string selectedType = "";
            //2/4/2015 NS added for VSPLUS-1370
            if (this.ServerTypeFilterListBox.SelectedItems.Count > 0)
            {
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
                    selectedType = "";
                }
                finally { }
            }
            string selectedServer = "";
            //8/5/2013 NS modified
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
            string date;
            if (this.DateParamEdit.Text == "")
            {
                date = DateTime.Now.ToString();
                this.DateParamEdit.Date = Convert.ToDateTime(date);
            }
            else
            {
                date = this.DateParamEdit.Value.ToString();
            }
            DateTime dt = Convert.ToDateTime(date);
            DashboardReports.DeviceUptimeXtraRpt report = new DashboardReports.DeviceUptimeXtraRpt();
            report.Parameters["DateVal"].Value = dt;
            report.Parameters["ServerName"].Value = selectedServer;
            //2/4/2015 NS added for VSPLUS-1370
            report.Parameters["ServerType"].Value = selectedType;
            this.ReportViewer1.Report = report;
            this.ReportViewer1.DataBind();

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
            dt = VSWebBL.ReportsBL.ReportsBL.Ins.getDeviceUptimeRptBL(sType);
            //8/5/2013 NS modified
            /*
            ServerListFilterComboBox.DataSource = dt;
            ServerListFilterComboBox.TextField = "NameANDType";
            ServerListFilterComboBox.ValueField = "NameANDType";
            ServerListFilterComboBox.DataBind();
             */
            ServerListFilterListBox.DataSource = dt;
            ServerListFilterListBox.TextField = "DeviceName";
            ServerListFilterListBox.ValueField = "DeviceName";
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


        protected void ServerListResetButton_Click(object sender, EventArgs e)
        {
            //this.ServerListFilterComboBox.SelectedIndex = -1;
            //2/4/2015 NS added for VSPLUS-1370 
            this.ServerTypeFilterListBox.UnselectAll();
            this.ServerListFilterListBox.UnselectAll();
            fillcombo("");
            DashboardReports.DeviceUptimeXtraRpt report = new DashboardReports.DeviceUptimeXtraRpt();
            report.Parameters["ServerName"].Value = "";
            //2/4/2015 NS added for VSPLUS-1370
            report.Parameters["ServerType"].Value = "";
            this.ReportViewer1.Report = report;
            this.ReportViewer1.DataBind();
        }

        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            string selectedServer = "";
            string selectedType = "";
            string date;
            //8/5/2013 NS modified
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
            //2/4/2015 NS added for VSPLUS-1370
            if (this.ServerTypeFilterListBox.SelectedItems.Count > 0)
            {
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
                    selectedType = "";
                }
                finally { }
            }
            if (this.DateParamEdit.Text == "")
            {
                date = DateTime.Now.ToString();
            }
            else
            {
                date = this.DateParamEdit.Value.ToString();
            }
            DateTime dt = Convert.ToDateTime(date);
            DashboardReports.DeviceUptimeXtraRpt report = new DashboardReports.DeviceUptimeXtraRpt();
            report.Parameters["DateVal"].Value = dt;
            report.Parameters["ServerName"].Value = selectedServer;
            report.Parameters["ServerType"].Value = selectedType;
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


    }
}