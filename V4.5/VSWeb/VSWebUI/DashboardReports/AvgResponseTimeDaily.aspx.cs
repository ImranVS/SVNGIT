using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace VSWebUI
{
    public partial class AvgResponseTimeDaily : System.Web.UI.Page
    {
        //2/4/2015 NS added for VSPLUS-1370
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
            dt=VSWebBL.ReportsBL.ReportsBL.Ins.getDistinctDomino(sType);

            /*
            ServerListFilterComboBox.DataSource = dt;
            ServerListFilterComboBox.TextField = "DeviceName";
            ServerListFilterComboBox.ValueField = "DeviceName";
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
            dt = VSWebBL.ReportsBL.ReportsBL.Ins.FillDeviceTypeComboforhistoricalresponseBL();
            ServerTypeFilterListBox.DataSource = dt;
            ServerTypeFilterListBox.TextField = "DeviceType";
            ServerTypeFilterListBox.ValueField = "DeviceType";
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
            DashboardReports.AvgDailyResponseTimeRpt report = new DashboardReports.AvgDailyResponseTimeRpt();
            report.Parameters["MyDevice"].Value = selectedServer;
            //2/5/2015 NS added for VSPLUS-1370
            report.Parameters["ServerType"].Value = selectedType;
            //1/14/2014 NS modified for VSPLUS-287
            //11/21/2014 NS commented out
            /*
            DateTime dt = new DateTime();
            if (StartDateEdit.Text == "")
            {
                dt = DateTime.Now;
                dt = new DateTime(dt.Year,dt.Month,1);
                StartDateEdit.Text = dt.ToShortDateString();
            }

            if (EndDateEdit.Text == "")
            {
                EndDateEdit.Text = DateTime.Now.ToShortDateString();
            }
            */
            // report.Parameters["StarDate"].Value = "2012-04-07 10:00:32.000";
            report.Parameters["EndDate"].Value = dtPick.ToDate; //EndDateEdit.Text;
            report.Parameters["StartDate"].Value = dtPick.FromDate; //StartDateEdit.Text;
            this.ReportViewer1.Report = report;
            this.ReportViewer1.DataBind();
        }

        protected void ServerListResetButton_Click(object sender, EventArgs e)
        {
            //this.ServerListFilterComboBox.SelectedIndex = -1;
            this.ServerListFilterListBox.UnselectAll();
            //2/5/2015 NS added for VSPLUS-1370
            this.ServerTypeFilterListBox.UnselectAll();
            fillcombo("");
            DashboardReports.AvgDailyResponseTimeRpt report = new DashboardReports.AvgDailyResponseTimeRpt();
            report.Parameters["MyDevice"].Value = "";
            //2/5/2015 NS added for VSPLUS-1370
            report.Parameters["ServerType"].Value = "";
            //11/21/2014 NS commented out
            //report.Parameters["StartDate"].Value = DateTime.Now.ToString();
            //report.Parameters["EndDate"].Value = DateTime.Now.ToString();
            //ServerListFilterComboBox.Text = "";
            //StartDateEdit.Text = DateTime.Now.ToShortDateString();
            //EndDateEdit.Text = DateTime.Now.ToShortDateString();
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