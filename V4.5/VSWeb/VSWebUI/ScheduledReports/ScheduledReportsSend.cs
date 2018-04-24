using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Net.Mail;
using System.Configuration;
using System.Data;

using DevExpress.XtraReports.UI;

namespace VSWebUI.ScheduledReportsSend
{
    public class ScheduledReports
    {
        //2/18/2014 NS added - constant values below are report IDs as they exist in the ReportItems table
        const int ResponseTimes = 22;
        const int DailyMailVolume = 11;
        const int DominoDiskTrend = 34;
        private static ScheduledReports _self = new ScheduledReports();
        public static ScheduledReports Ins
        {
            get
            {
                return _self;
            }
        }
        public DataTable GetDT()
        {
            DataTable dt = new DataTable();
            dt = VSWebUI.DashboardReports.DailyMailVolumeRpt.Ins.GetDT();
            return dt;
        }
        public void SendReport(int ReportID, string Subject, string Body, string SendTo, string CopyTo, string BlindCopyTo,
            string FileFormat)
        {
            DevExpress.XtraReports.UI.XtraReport report = null;
            string myEmailAddress = "";
            string mypwd = "";
            string[] mailparams = new string[5];
            Settings[] settingsObject = new Settings[5];
            Settings[] rtsettingsObject = new Settings[5];
            //try
            //{
                switch (ReportID)
                {
                    case ResponseTimes:
                        report = new DashboardReports.ResponseTimeXtraRpt();
                        report = (DashboardReports.ResponseTimeXtraRpt)VSWebUI.DashboardReports.ResponseTimeRpt.Ins.GetRpt();
                        break;
                    case DailyMailVolume:
                        report = new DashboardReports.DailyMailVolumeXtraRpt();
                        report = (DashboardReports.DailyMailVolumeXtraRpt)VSWebUI.DashboardReports.DailyMailVolumeRpt.Ins.GetRpt();
                        break;
                    case DominoDiskTrend:
                        report = new DashboardReports.DominoDiskAvgXtraRpt();
                        report = (DashboardReports.DominoDiskAvgXtraRpt)VSWebUI.DashboardReports.DominoDiskTrendRpt.Ins.GetRpt();
                        break;
                }
                if (report != null)
                {
                    // Create a new memory stream and export the report into it as PDF.
                    MemoryStream mem = new MemoryStream();
                    //WriteServiceHistoryEntry(DateTime.Now.ToString() + " report size: " + report.PageSize);
                    //try
                    //{
                        switch (FileFormat.ToLower())
                        {
                            case "pdf":
                                report.ExportToPdf(mem);
                                break;
                            case "xls":
                                report.ExportToXls(mem);
                                break;
                            case "xlsx":
                                report.ExportToXlsx(mem);
                                break;
                            case "csv":
                                report.ExportToText(mem);
                                break;
                        }
                    //}
                    //catch (Exception ex)
                    //{
                    //    WriteServiceHistoryEntry(DateTime.Now.ToString() + " report not exported: " + ex.Message);
                    //}
                    

                    // Create a new attachment and put the PDF report into it.
                    mem.Seek(0, System.IO.SeekOrigin.Begin);
                    Attachment att = new Attachment(mem, "VSReport." + FileFormat.ToLower(), "application/" + FileFormat.ToLower());
                    // Create a new message and attach the PDF report to it.
                    MailMessage mail = new MailMessage();
                    mail.Attachments.Add(att);
                    // Specify sender and recipient options for the e-mail message.
                    for (int i = 0; i < 5; ++i)
                    {
                        settingsObject[i] = new Settings();
                    }
                    for (int i = 0; i < 5; ++i)
                    {
                        rtsettingsObject[i] = new Settings();
                    }
                    settingsObject[0].sname = "PrimaryHostName";
                    settingsObject[1].sname = "primaryUserID";
                    settingsObject[2].sname = "primarypwd";
                    settingsObject[3].sname = "primaryport";
                    settingsObject[4].sname = "primarySSL";
                    mailparams[0] = "smtp.gmail.com";
                    mailparams[1] = ConfigurationSettings.AppSettings["AdminMailID"]; //"web.vitalsigns@gmail.com";
                    mailparams[2] = ConfigurationSettings.AppSettings["Password"];        //"vitalsigns2012";
                    mailparams[3] = "587";
                    mailparams[4] = "true";
                    for (int i = 0; i < 5; i++)
                    {
                        try
                        {
                            rtsettingsObject[i] = VSWebBL.SettingBL.SettingsBL.Ins.GetData(settingsObject[i]);
                            if (rtsettingsObject[i].svalue == "" || rtsettingsObject[i].svalue == null)
                            {
                                //do nothing
                                //WriteServiceHistoryEntry(DateTime.Now.ToString() + " " + settingsObject[i].sname + " is empty");
                            }
                            else
                            {
                                mailparams[i] = rtsettingsObject[i].svalue;
                                //WriteServiceHistoryEntry(DateTime.Now.ToString() + " " + settingsObject[i].sname + " " + mailparams[i]);
                            }
                        }
                        catch (Exception)
                        {
                            throw;
                        }
                    }

                    mail.From = new MailAddress(mailparams[1], "VitalSigns");
                    report.ExportOptions.Email.RecipientAddress = SendTo;
                    //report.ExportOptions.Email.RecipientName = SendTo;
                    report.ExportOptions.Email.Subject = Subject;
                    mail.To.Add(new MailAddress(report.ExportOptions.Email.RecipientAddress));
                    if (CopyTo != "")
                    {
                        mail.CC.Add(new MailAddress(CopyTo));
                    }
                    if (BlindCopyTo != "")
                    {
                        mail.Bcc.Add(new MailAddress(BlindCopyTo));
                    }
                    // Specify other e-mail options.
                    mail.Subject = report.ExportOptions.Email.Subject;
                    mail.Body = Body;

                    // Send the e-mail message via the specified SMTP server.
                    myEmailAddress = mailparams[1]; //"web.vitalsigns@gmail.com";
                    mypwd = mailparams[2];        //"vitalsigns2012";
                    SmtpClient smtp = new SmtpClient(mailparams[0], Convert.ToInt32(mailparams[3]))
                    {
                        Credentials = new System.Net.NetworkCredential(myEmailAddress, mypwd),
                        EnableSsl = true
                    };
                    smtp.Send(mail);

                    // Close the memory stream.
                    mem.Close();
                }
            //}
            //catch (Exception ex)
            //{
            //    WriteServiceHistoryEntry(DateTime.Now.ToString() + " The following error has occurred in SendReport: " + ex.Message);
            //}
        }
        //2/4/2014 NS added for sched reports
        protected void WriteServiceHistoryEntry(string strMsg)
        {
            bool appendMode = true;
            string ServiceLogDestination = AppDomain.CurrentDomain.BaseDirectory.ToString() + "/log_files/ScheduledReports_Service.txt";
            try
            {
                StreamWriter sw = new StreamWriter(ServiceLogDestination, appendMode, System.Text.Encoding.Unicode);
                sw.WriteLine(strMsg);
                sw.Close();
                sw = null;
            }
            catch
            {
            }
            finally
            {
                GC.Collect();
            }
        }
    }
}