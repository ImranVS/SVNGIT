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
    // 3/28/2016 Durga Addded for VSPLUS-2695
    public partial class CostPerUserServedchart : System.Web.UI.Page
    {
        private static CostPerUserServedchart _self = new CostPerUserServedchart();

        public static CostPerUserServedchart Ins
        {
            get
            {
                return _self;
            }
        }
        DashboardReports.CostPerUserServedchartRpt report;

        public DashboardReports.CostPerUserServedchartRpt GetRpt()
        {

            report = new DashboardReports.CostPerUserServedchartRpt();
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
                fillUserTypeCombo();
            }
            FillReport(); 
         
            GetUserType();
        }

        public void FillCombobox()
        {
            DataTable Typetab = VSWebBL.DashboardBL.KeyMetricsBL.Ins.GetServerTypeForCostperUserserved();
            TypeComboBox.DataSource = Typetab;
            TypeComboBox.TextField = "ServerType";
            TypeComboBox.DataBind();

            try
            {
                TypeComboBox.SelectedItem = TypeComboBox.Items.FindByText("Domino");
            }
            catch
            {
                TypeComboBox.SelectedIndex = 0;
            }
        }
        protected void TypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetUserType();
            FillReport();
           
         

        }
        public void GetUserType()
        {
            if (TypeComboBox.Text == "Domino")
            {
                UserTypeComboBox.Visible = false;
                UserTypelabel.Visible = false;

            }
            else
            {
                UserTypeComboBox.Visible = true;
                UserTypelabel.Visible = true;
            }

        }
    
       
        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            FillReport();
        }

      
        protected void EmailRptButton_Click(object sender, EventArgs e)
        {
            try
            {

                DashboardReports.CostPerUserServedchartRpt report = new DashboardReports.CostPerUserServedchartRpt();
                report = (DashboardReports.CostPerUserServedchartRpt)ASPxDocumentViewer1.Report;
   
                MemoryStream mem = new MemoryStream();
                report.ExportToPdf(mem);
                 mem.Seek(0, System.IO.SeekOrigin.Begin);
                Attachment att = new Attachment(mem, "TestReport.pdf", "application/pdf");
                MailMessage mail = new MailMessage();
                mail.Attachments.Add(att);
                mail.From = new MailAddress("web.vitalsigns@gmail.com", "VitalSigns");
                report.ExportOptions.Email.RecipientAddress = "natallya.shkarayeva@gmail.com";
                report.ExportOptions.Email.RecipientName = "Natallya Shkarayeva";
                report.ExportOptions.Email.Subject = "Response Time report (VS)";
                mail.To.Add(new MailAddress(report.ExportOptions.Email.RecipientAddress,
                    report.ExportOptions.Email.RecipientName));

             
                mail.Subject = report.ExportOptions.Email.Subject;
                mail.Body = "This is a test scheduled report from VS.";

              
                string myEmailAddress = ConfigurationSettings.AppSettings["AdminMailID"]; 
                string mypwd = ConfigurationSettings.AppSettings["Password"];      
                myEmailAddress = "web.vitalsigns@gmail.com";
                mypwd = "vitalsigns2012";
                SmtpClient smtp = new SmtpClient("smtp.gmail.com",587)
                {
                    Credentials = new System.Net.NetworkCredential(myEmailAddress, mypwd),
                    EnableSsl = true
                };
                smtp.Send(mail);

                
                mem.Close();
            }
            catch (Exception ex)
            {
                throw ex;
              
            }
        }
        public void fillUserTypeCombo()
        {
            DataTable dt = new DataTable();
            dt = VSWebBL.ReportsBL.ReportsBL.Ins.GetMSUserCountTypes("AVG");
            if (dt.Rows.Count > 0)
            {
                UserTypeComboBox.SelectedIndex = 0;
                UserTypeComboBox.TextField = "StatName";
                UserTypeComboBox.ValueField = "StatName";
                UserTypeComboBox.DataSource = dt;
                UserTypeComboBox.DataBind();
            }
        }

        public void FillReport()
        {
            DashboardReports.CostPerUserServedchartRpt rpt = new DashboardReports.CostPerUserServedchartRpt();
            string selectedServer = "";
            if (this.TypeComboBox.SelectedIndex >= 0)
            {
                selectedServer = this.TypeComboBox.SelectedItem.Value.ToString();
            }

            rpt.Parameters["ServerType"].Value = selectedServer;
        

            rpt.Parameters["ServerName"].Value = SearchTextBox.Text;
            rpt.Parameters["UserType"].Value = UserTypeComboBox.Text;

            rpt.CreateDocument();
            ASPxDocumentViewer1.Report = rpt;
            ASPxDocumentViewer1.DataBind();
        }
    }
}