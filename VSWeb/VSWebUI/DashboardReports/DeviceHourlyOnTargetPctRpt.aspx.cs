using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
namespace VSWebUI.DashboardReports
{
    public partial class DeviceHourlyOnTargetPctRpt : System.Web.UI.Page
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
           DataTable dt=new DataTable();
            dt=VSWebBL.ReportsBL.ReportsBL.Ins.getDeviceHourlyOnTargetPctRptBL(sType);
            //8/2/2013 NS modified
            /*
            ServerListFilterComboBox.DataSource = dt;
            ServerListFilterComboBox.TextField = "devicename";
            ServerListFilterComboBox.ValueField = "devicename";
            ServerListFilterComboBox.DataBind();
            */
            ServerListFilterListBox.DataSource = dt;
            ServerListFilterListBox.TextField = "devicename";
            ServerListFilterListBox.ValueField = "devicename";
            ServerListFilterListBox.DataBind();
        }

        public void fillservertypelist()
        {
            DataTable dt = new DataTable();
            dt = VSWebBL.ReportsBL.ReportsBL.Ins.getDeviceHourlyOnTargetPctServerTypes();
            ServerTypeFilterListBox.DataSource = dt;
            ServerTypeFilterListBox.TextField = "devicetype";
            ServerTypeFilterListBox.ValueField = "devicetype";
            ServerTypeFilterListBox.DataBind();
        }

        public void Report()
        {
            string selectedServer = "";
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
            DashboardReports.DeviceHourlyOnTargetPctXtraRpt report = new DashboardReports.DeviceHourlyOnTargetPctXtraRpt();
            //report.Parameters["DateVal"].Value = dt;
            report.Parameters["ServerName"].Value = selectedServer;
            //2/2/2015 NS added for VSPLUS-1370
            report.Parameters["ServerType"].Value = selectedType;
            this.ReportViewer1.Report = report;
            this.ReportViewer1.DataBind();
        }
        protected void ServerListResetButton_Click(object sender, EventArgs e)
        {
            //this.ServerListFilterComboBox.SelectedIndex = -1;
            this.ServerListFilterListBox.UnselectAll();
            //2/2/2015 NS added for VSPLUS-1370 
            this.ServerTypeFilterListBox.UnselectAll();
            fillcombo("");
            DashboardReports.DeviceHourlyOnTargetPctXtraRpt report = new DashboardReports.DeviceHourlyOnTargetPctXtraRpt();
            report.Parameters["ServerName"].Value = "";
            report.Parameters["ServerType"].Value = "";
            this.ReportViewer1.Report = report;
            this.ReportViewer1.DataBind();
        }

        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            Report();
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