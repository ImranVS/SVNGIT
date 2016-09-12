using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.XtraCharts;
using DevExpress.XtraReports;
using System.Data;

namespace VSWebUI.DashboardReports
{
    public partial class IBMConnCommunityRpt : System.Web.UI.Page
    {
        //6/16/2016 Sowjanya modified for VSPLUS-3059
        protected void Page_Load(object sender, EventArgs e)
        {

            FillReport();
            if (!IsPostBack)
            {
                fillServersCombo();
                fillcombo();
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
        public void fillcombo()
        {
            
         
            DataTable dt = new DataTable();
            string Server = "";
            if (ServerComboBox.Items.Count > 0)
            {
                Server = Convert.ToString(ServerComboBox.SelectedItem.Value);
                DataTable Serverids = VSWebBL.ReportsBL.ReportsBL.Ins.GetID(Server);
                if (Serverids.Rows.Count > 0)
                {
                    int id = Convert.ToInt32(Serverids.Rows[0]["ID"].ToString());

                    dt = VSWebBL.ReportsBL.ReportsBL.Ins.CommunityNames(id);
                }
               // UserFilterComboBox.SelectedItem = null;
                UserFilterComboBox.SelectedIndex = -1;
                UserFilterComboBox.DataSource = dt;
                UserFilterComboBox.TextField = "Name";
                UserFilterComboBox.ValueField = "Name";
                UserFilterComboBox.DataBind();
            }
        }   

        protected void UserResetButton_Click(object sender, EventArgs e)
        {
            this.UserFilterComboBox.SelectedIndex = -1;
            DashboardReports.IBMConnCommunityXtraRpt report = new DashboardReports.IBMConnCommunityXtraRpt();
            report.Parameters["Name"].Value = "";
            report.CreateDocument();
            ASPxDocumentViewer1.Report = report;
            ASPxDocumentViewer1.DataBind();
        }

        protected void ReptBtn_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Configurator/Reports.aspx?M=" + Request.QueryString["M"], false);
            Context.ApplicationInstance.CompleteRequest();
   
        }

        protected void UserFilterComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }

        public void fillServersCombo()
        {
            DataTable dt = new DataTable();
            //string Community = "";
            //if (ServerListFilterComboBox.Items.Count > 0)
            //{
            //Community = ServerListFilterComboBox.SelectedItem.Value.ToString();
            ServerComboBox.SelectedIndex = 0;
            dt = VSWebBL.ReportsBL.ReportsBL.Ins.GetIBMConnectionsServers();
            ServerComboBox.DataSource = dt;

            ServerComboBox.TextField = "ServerName";
            ServerComboBox.ValueField = "ServerName";
            ServerComboBox.DataBind();
            //}
        }

        protected void ServerComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillReport();
            fillcombo();
           
        }

        public void FillReport()
        {
            string ServerName = "";
            string selectedCommunity = "";
            if (this.UserFilterComboBox.SelectedIndex >= 0)
            {
                selectedCommunity = this.UserFilterComboBox.SelectedItem.Value.ToString();
            }
            if (this.ServerComboBox.SelectedIndex >= 0)
            {
                ServerName = this.ServerComboBox.SelectedItem.Value.ToString();

            }
            DashboardReports.IBMConnCommunityXtraRpt report = new DashboardReports.IBMConnCommunityXtraRpt();
            report.Parameters["Name"].Value = selectedCommunity;
            report.Parameters["ServerName"].Value = ServerName;
            report.CreateDocument();
            ASPxDocumentViewer1.Report = report;
            ASPxDocumentViewer1.DataBind();

        }

        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            FillReport();
        }

    }
}