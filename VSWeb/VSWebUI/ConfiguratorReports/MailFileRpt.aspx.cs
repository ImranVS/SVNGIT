using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace VSWebUI.ConfiguratorReports
{
    public partial class MailFileRpt : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int filesize = 0;
            if (this.MailFileTextBox.Text.ToString() != "")
            {
                filesize = Convert.ToInt32(this.MailFileTextBox.Text.ToString());
            }
            ConfiguratorReports.MailFileXtraRpt report = new ConfiguratorReports.MailFileXtraRpt();
            report.Parameters["FileSize"].Value = filesize;
            report.CreateDocument();
            ASPxDocumentViewer1.Report = report;
            ASPxDocumentViewer1.DataBind();
        }

        protected void MailFileButton_Click(object sender, EventArgs e)
        {
            int filesize = 0;
            if (this.MailFileTextBox.Text.ToString() != "")
            {
                filesize = Convert.ToInt32(this.MailFileTextBox.Text.ToString());
            }
            ConfiguratorReports.MailFileXtraRpt report = new ConfiguratorReports.MailFileXtraRpt();
            report.Parameters["FileSize"].Value = filesize;
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

        protected void ServerListResetButton_Click(object sender, EventArgs e)
        {
            MailFileTextBox.Text = "";
            ConfiguratorReports.MailFileXtraRpt report = new ConfiguratorReports.MailFileXtraRpt();
            report.Parameters["FileSize"].Value = "";
            report.CreateDocument();
            ASPxDocumentViewer1.Report = report;
            ASPxDocumentViewer1.DataBind();
        }
    }
}