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
{
    public partial class ResponseTimeRpt : System.Web.UI.Page
    {
        private static ResponseTimeRpt _self = new ResponseTimeRpt();

        public static ResponseTimeRpt Ins
        {
            get
            {
                return _self;
            }
        }
        DashboardReports.ResponseTimeXtraRpt report;

        public DashboardReports.ResponseTimeXtraRpt GetRpt()
        {

            report = new DashboardReports.ResponseTimeXtraRpt();
            DataTable dt = new DataTable();
            string typeval = "";
            typeval = "Domino";
            dt = VSWebBL.ReportsBL.ReportsBL.Ins.GetResponseTimes(typeval);
            if (dt.Rows.Count > 0)
            {
                report.BuildChart(0, dt.Rows.Count, dt, (XRChart)report.FindControl("xrChart1", true));
            }
            return report;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FillCombobox();
            }
            report = new DashboardReports.ResponseTimeXtraRpt();
            report.Parameters["TypeVal"].Value = TypeComboBox.SelectedItem.Text;
            report.CreateDocument();
            ASPxDocumentViewer1.Report = report;
            ASPxDocumentViewer1.DataBind();
        }

        public void FillCombobox()
        {
            DataTable Typetab = VSWebBL.DashboardBL.KeyMetricsBL.Ins.GetType();
            TypeComboBox.DataSource = Typetab;
            TypeComboBox.TextField = "Type";
            TypeComboBox.DataBind();
            //9/26/2013 NS commented out the option All in order to display a manageable amount of data per page
            //TypeComboBox.Items.Insert(0, new DevExpress.Web.ListEditItem("All", 0));
            //TypeComboBox.Items[0].Selected = true;
            TypeComboBox.SelectedItem = TypeComboBox.Items.FindByText("Domino");
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
            DashboardReports.ResponseTimeXtraRpt report = new DashboardReports.ResponseTimeXtraRpt();
            report.Parameters["TypeVal"].Value = TypeComboBox.SelectedItem.Text;
            report.CreateDocument();
            ASPxDocumentViewer1.Report = report;
            ASPxDocumentViewer1.DataBind();
        }

        //10/31/2013 NS modified - SCHEDULED REPORTS sample code
        protected void EmailRptButton_Click(object sender, EventArgs e)
        {
            try
            {
                // Create a new report.                
                DashboardReports.ResponseTimeXtraRpt report = new DashboardReports.ResponseTimeXtraRpt();
                report = (DashboardReports.ResponseTimeXtraRpt)ASPxDocumentViewer1.Report;

                // Create a new memory stream and export the report into it as PDF.
                MemoryStream mem = new MemoryStream();
                report.ExportToPdf(mem);

                // Create a new attachment and put the PDF report into it.
                mem.Seek(0, System.IO.SeekOrigin.Begin);
                Attachment att = new Attachment(mem, "TestReport.pdf", "application/pdf");

                // Create a new message and attach the PDF report to it.
                MailMessage mail = new MailMessage();
                mail.Attachments.Add(att);

                // Specify sender and recipient options for the e-mail message.
                mail.From = new MailAddress("web.vitalsigns@gmail.com", "VitalSigns");
                report.ExportOptions.Email.RecipientAddress = "natallya.shkarayeva@gmail.com";
                report.ExportOptions.Email.RecipientName = "Natallya Shkarayeva";
                report.ExportOptions.Email.Subject = "Response Time report (VS)";
                mail.To.Add(new MailAddress(report.ExportOptions.Email.RecipientAddress,
                    report.ExportOptions.Email.RecipientName));

                // Specify other e-mail options.
                mail.Subject = report.ExportOptions.Email.Subject;
                mail.Body = "This is a test scheduled report from VS.";

                // Send the e-mail message via the specified SMTP server.
                string myEmailAddress = ConfigurationSettings.AppSettings["AdminMailID"]; //"web.vitalsigns@gmail.com";
                string mypwd = ConfigurationSettings.AppSettings["Password"];        //"vitalsigns2012";
                myEmailAddress = "web.vitalsigns@gmail.com";
                mypwd = "vitalsigns2012";
                SmtpClient smtp = new SmtpClient("smtp.gmail.com",587)
                {
                    Credentials = new System.Net.NetworkCredential(myEmailAddress, mypwd),
                    EnableSsl = true
                };
                smtp.Send(mail);

                // Close the memory stream.
                mem.Close();
            }
            catch (Exception ex)
            {
                throw ex;
                //MessageBox.Show(this, "Error sending a report.\n" + ex.ToString());
            }
        }
    }
}