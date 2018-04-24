using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Configuration;
using System.IO;
using System.Net.Mail;
using System.Data;
using DevExpress.XtraReports.UI;

namespace VSWebUI.DashboardReports
{// 3/17/2016 Durga Addded for VSPLUS-2702
    public partial class Stalemailboxes : System.Web.UI.Page
    {
        private static Stalemailboxes _self = new Stalemailboxes();

        public static Stalemailboxes Ins
        {
            get
            {
                return _self;
            }
        }
        DashboardReports.StalemailboxeRpt report;


        protected void Page_Load(object sender, EventArgs e)
        {
           
            fillcombo();
            FillReport();
            
        }
        public void fillcombo()
        {
            DataTable d = new DataTable();
           
            d = VSWebBL.ReportsBL.XsdBL.Ins.GetO365Server();

            ServerListBox.DataSource = d;
            ServerListBox.TextField = "Server";
            ServerListBox.ValueField = "Server";
            ServerListBox.DataBind();
        }
       public void FillReport()
        {
            report = new DashboardReports.StalemailboxeRpt();

            string selectedServer = "";
            if (this.ServerListBox.SelectedItems.Count > 0)
            {
              
                for (int i = 0; i < this.ServerListBox.SelectedItems.Count; i++)
                {
                    selectedServer += "'" + this.ServerListBox.SelectedItems[i].Text + "'" + ",";
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
       


            report.Parameters["TypeVal"].Value = selectedServer;
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

      


        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            FillReport();
        }
        protected void ResetButton_Click(object sender, EventArgs e)
        {

            this.ServerListBox.UnselectAll();
            DashboardReports.StalemailboxeRpt report = new DashboardReports.StalemailboxeRpt();

            report.CreateDocument();
            report.Parameters["TypeVal"].Value = "";
            this.ASPxDocumentViewer1.Report = report;
            this.ASPxDocumentViewer1.DataBind();


        }
    }
}