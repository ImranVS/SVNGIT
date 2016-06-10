using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace VSWebUI.DashboardReports
{
    public partial class IBMConnectionsProfiles : System.Web.UI.Page
    {
        //11-05-2016 Durga Modified for VSPLUS-2836
        List<string> strringlist = new List<string>();
        string ProfileType = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            FillReport();
            GetProfileType();
            if (!IsPostBack)
            {
                fillProfilesCombobox();
                fillcombo(ProfileType);
            }
        }

        public void fillcombo(string statname1)
        {
            DataTable dt = new DataTable();

            dt = VSWebBL.ReportsBL.ReportsBL.Ins.GetIBMConnectionsServerlist(statname1);
            ServerListFilterListBox.DataSource = dt;
            ServerListFilterListBox.TextField = "ServerName";
            ServerListFilterListBox.ValueField = "ServerName";
            ServerListFilterListBox.DataBind();
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
            GetProfileType();
           
            strfrom = dtPick.FromDate;
            strto = dtPick.ToDate;
            DateTime dtfrom = DateTime.Parse(strfrom);
            DateTime dtto = DateTime.Parse(strto);

            DashboardReports.IBMConnectionsProfilesRpt report = new DashboardReports.IBMConnectionsProfilesRpt();
            report.Parameters["ServerName"].Value = selectedServer;
            report.Parameters["DateFrom"].Value = strfrom;
            report.Parameters["DateTo"].Value = strto;
            report.Parameters["ProfileType"].Value = ProfileType;
            this.ReportViewer1.Report = report;
            this.ReportViewer1.DataBind();

        }
        protected void ProfilesCombobox_SelectedIndexChanged(object sender, EventArgs e)
        {

            FillReport();
            GetProfileType();
            fillcombo(ProfileType);

        }

        public void fillProfilesCombobox()
        {
            DataTable dt = new DataTable();
            dt = VSWebBL.ReportsBL.ReportsBL.Ins.GetonnectionsStatsNames("NUM_OF_PROFILES_CREATED_YESTERDAY", "NUM_OF_PROFILES_EDITED_YESTERDAY");
            if (dt.Rows.Count > 0)
            {
                ProfilesCombobox.SelectedIndex = 0;
                ProfilesCombobox.TextField = "StatName";
                ProfilesCombobox.ValueField = "StatName";
                ProfilesCombobox.DataSource = dt;
                ProfilesCombobox.DataBind();
            }
        }
        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            FillReport();
        }
        public void GetProfileType()

          {
         if (ProfilesCombobox.Text == "Created")
            {
                ProfileType = "NUM_OF_PROFILES_CREATED_YESTERDAY";
            }
            else
            {
                ProfileType = "NUM_OF_PROFILES_EDITED_YESTERDAY";
            }
    }

    }
}