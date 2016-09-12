using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace VSWebUI.DashboardReports
{ //6/16/2016 Sowjanya modified for VSPLUS-3059
    public partial class IBMConnectionForumsRpt : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Report();
            if (!IsPostBack)
            {
                
                fillForumsTypeCombo();
                fillcombo();
            }
        }

        public void fillcombo()
        {
            DataTable dt = new DataTable();

           string FType = "";

           if (ForumsTypeComboBox.Items.Count > 0)
           {
               FType = ForumsTypeComboBox.SelectedItem.Value.ToString();
               dt = VSWebBL.ReportsBL.ReportsBL.Ins.GetIBMConnectionsServerlist(FType);
               ServerComboBox.DataSource = dt;
               ServerComboBox.TextField = "ServerName";
               ServerComboBox.ValueField = "ServerName";
               ServerComboBox.DataBind();
           }


          
        }

        public void fillForumsTypeCombo()
        {
            DataTable dt = new DataTable();
            dt = VSWebBL.ReportsBL.ReportsBL.Ins.GetMSForumTypes();
            if (dt.Rows.Count > 0)
            {
                ForumsTypeComboBox.SelectedIndex = 0;
                ForumsTypeComboBox.TextField = "Dstatname";
                ForumsTypeComboBox.ValueField = "StatName";
                ForumsTypeComboBox.DataSource = dt;
                ForumsTypeComboBox.DataBind();
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
            string selectedForumType = "";
            if (ForumsTypeComboBox.SelectedIndex != -1)
            {
                selectedForumType = ForumsTypeComboBox.SelectedItem.Value.ToString();
            }

            if (ServerComboBox.SelectedIndex != -1)
            {
                selectedServer = ServerComboBox.SelectedItem.Value.ToString();
            }
            string strfrom = "";
            string strto = "";
           
            strfrom = dtPick.FromDate;
            strto = dtPick.ToDate;
            DateTime dtfrom = DateTime.Parse(strfrom);
            DateTime dtto = DateTime.Parse(strto);
           
            DashboardReports.IBMConnectionForumsRptXtraRpt report = new DashboardReports.IBMConnectionForumsRptXtraRpt();
            report.Parameters["ServerName"].Value = selectedServer;
            report.Parameters["DateFrom"].Value = strfrom;
            report.Parameters["DateTo"].Value = strto;
            report.Parameters["ForumType"].Value = selectedForumType;
            this.ReportViewer1.Report = report;
            this.ReportViewer1.DataBind();

        }

        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            Report();
        }

        protected void ForumsTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {

            Report();
            fillcombo();

        }

    }
}