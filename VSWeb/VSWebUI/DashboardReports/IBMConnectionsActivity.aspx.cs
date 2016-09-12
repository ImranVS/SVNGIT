﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace VSWebUI.DashboardReports
{
    public partial class IBMConnectionsActivity : System.Web.UI.Page
    {
        //13-04-2016 Durga Modified for  VSPLUS-2832
        List<string> strringlist = new List<string>();
        string ActivityType = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            FillReport();
          
            if (!IsPostBack)
            {
                fillActivityTypeCombobox();
                fillcombo();
            }
        }

        public void fillcombo()
        {
            DataTable dt = new DataTable();
            string Activity = "";
            if (ActivityTypeCombobox.Items.Count > 0)
            {
                Activity = ActivityTypeCombobox.SelectedItem.Value.ToString();
                dt = VSWebBL.ReportsBL.ReportsBL.Ins.GetIBMConnectionsServerlist(Activity);
                ServerComboBox.DataSource = dt;
                ServerComboBox.TextField = "ServerName";
                ServerComboBox.ValueField = "ServerName";
                ServerComboBox.DataBind();
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

        public void FillReport()
        {
            string selectedServer = "";
           
            string strfrom = "";
            string strto = "";
            string selectedActivityType = "";
            if (ActivityTypeCombobox.SelectedIndex != -1)
            {
                selectedActivityType = ActivityTypeCombobox.SelectedItem.Value.ToString();
            }
            if (ServerComboBox.SelectedIndex != -1)
            {
                selectedServer = ServerComboBox.SelectedItem.Value.ToString();
            }
            strfrom = dtPick.FromDate;
            strto = dtPick.ToDate;
            DateTime dtfrom = DateTime.Parse(strfrom);
            DateTime dtto = DateTime.Parse(strto);
            
            DashboardReports.IBMConnectionsActivityRpt report = new DashboardReports.IBMConnectionsActivityRpt();
            report.Parameters["ServerName"].Value = selectedServer;
            report.Parameters["DateFrom"].Value = strfrom;
            report.Parameters["DateTo"].Value = strto;
            report.Parameters["ActivityType"].Value = selectedActivityType;
            this.ReportViewer1.Report = report;
            this.ReportViewer1.DataBind();

        }
        protected void ActivityTypeCombobox_SelectedIndexChanged(object sender, EventArgs e)
        {

            FillReport();
           
            fillcombo();

        }

        public void fillActivityTypeCombobox()
        {
            DataTable dt = new DataTable();
            dt = VSWebBL.ReportsBL.ReportsBL.Ins.GetActivityStatNames();
            if (dt.Rows.Count > 0)
            {
                ActivityTypeCombobox.SelectedIndex = 0;
                ActivityTypeCombobox.TextField = "Userstatname";
                ActivityTypeCombobox.ValueField = "StatName";
                ActivityTypeCombobox.DataSource = dt;
                ActivityTypeCombobox.DataBind();
            }
        }
        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            FillReport();
        }
    

    }
}