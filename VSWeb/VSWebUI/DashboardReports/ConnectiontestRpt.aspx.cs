using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace VSWebUI.DashboardReports
{
    public partial class ConnectiontestRpt : System.Web.UI.Page
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
                ServerListFilterComboBox.SelectedIndex = -1;
                ServerListFilterComboBox.DataSource = dt;
                ServerListFilterComboBox.TextField = "Name";
                ServerListFilterComboBox.ValueField = "Name";
                ServerListFilterComboBox.DataBind();
            }

        }


        protected void ServerListFilterComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
           
            
        }

        protected void ServerListResetButton_Click(object sender, EventArgs e)
        {
            this.ServerListFilterComboBox.SelectedIndex = -1;
            DashboardReports.ConnectionstestXtraRpt report = new DashboardReports.ConnectionstestXtraRpt();
            report.Parameters["Name"].Value = "";
            report.CreateDocument();
            ASPxDocumentViewer1.Report = report;
            ASPxDocumentViewer1.DataBind();
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
            fillcombo();
            FillReport();
        }

        public void FillReport()
        {
            string ServerName = "";
            string selectedServer = "";

            if (this.ServerListFilterComboBox.SelectedIndex >= 0)
            {

                selectedServer = this.ServerListFilterComboBox.SelectedItem.Value.ToString();

            }
            if (this.ServerComboBox.SelectedIndex >= 0)
            {
                ServerName = this.ServerComboBox.SelectedItem.Value.ToString();

            }
            DashboardReports.ConnectionstestXtraRpt report = new DashboardReports.ConnectionstestXtraRpt();
            report.Parameters["Name"].Value = selectedServer;
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