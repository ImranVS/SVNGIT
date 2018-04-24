using Ionic.Zip;

using DevExpress.Web.ASPxTreeList;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net.Mail;

namespace VSWebUI.Configurator
{
	public partial class SendLogFiles : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			ScriptManager _scriptMan = ScriptManager.GetCurrent(this);
			_scriptMan.AsyncPostBackTimeout = 600;

			if (Session["LogFiles"] == null)
			{
				DataTable dt = getLogFilesDataTable();
				Session["LogFiles"] = dt;
			}

			LogFilesTree.DataSource = (DataTable)Session["LogFiles"];
			LogFilesTree.DataBind();

		}

		protected DataTable getLogFilesDataTable()
		{
			string logPath = "";

			logPath = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("Log Files Path");
			if (logPath == "")
			{
				logPath = AppDomain.CurrentDomain.BaseDirectory.ToString();
			}

			DataTable dt = new DataTable();
			dt.Columns.Add("Path", typeof(String));
			dt.Columns.Add("FullPath", typeof(String));
			dt.Columns.Add("ParentID", typeof(System.Int32));
			dt.Columns.Add("isFolder",typeof(Boolean));
			dt.Columns.Add("ID", typeof(System.Int32));
			dt.Columns["ID"].AutoIncrement = true;
			dt.Columns["ID"].AutoIncrementSeed=0;
			dt.Columns["ID"].AutoIncrementStep = 1;
			

			GetAllLogFiles(logPath, ref dt, -1);

			return dt;
		}

		protected void GetAllLogFiles(string path, ref DataTable dt, int i)
		{
			string[] filePaths = System.IO.Directory.GetFiles(path);
			string[] folderPaths = System.IO.Directory.GetDirectories(path);

			foreach (string file in filePaths)
			{
				if (file.Contains("LogFiles.z"))
					continue;
				DataRow row = dt.NewRow();
				row["FullPath"] = file;
				row["Path"] = file.Substring(file.LastIndexOf("\\") + 1);
				row["isFolder"] = false;
				if (i != -1)
				{
					row["ParentID"] = i;
				}
				dt.Rows.Add(row);
			}

			foreach (string dir in folderPaths)
			{
				DataRow row = dt.NewRow();
				row["FullPath"] = dir;
				row["Path"] = dir.Substring(dir.LastIndexOf("\\") + 1);
				row["isFolder"] = true;
				if (i != -1)
				{
					row["ParentID"] = i;
				}
				dt.Rows.Add(row);
				int k = int.Parse(row["ID"].ToString());
				GetAllLogFiles(dir, ref dt,k);

			}
		}

		private List<String> getSelectedRows()
		{
			//System.Collections.ArrayList<String> list = new System.Collections.ArrayList<String>();
			List<String> list = new List<String>();
			TreeListNodeIterator iterator = LogFilesTree.CreateNodeIterator();
			TreeListNode node;
			while (true)
			{
				node = iterator.GetNext();
				if (node == null) break;
				if(node.Selected && !Convert.ToBoolean(node["isFolder"].ToString()))
					list.Add(node["FullPath"].ToString());

			}

			return list;
		}

		protected void ZipButton_Click(object sender, EventArgs e)
		{
			errorDiv.Style.Value = "display: none";
			errorDiv2.Style.Value = "display: none";
			successDiv.Style.Value = "display: none";

			string emailAddressToSendTo = EmailAddressTextBox.Text;
			if (String.IsNullOrEmpty(emailAddressToSendTo))
			{
                //10/6/2014 NS modified for VSPLUS-990
				errorDiv2.InnerHtml = "There is no email address entered in the above box."+
                    "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
				errorDiv2.Style.Value = "display: block";
				return;
			}
			

			string[] paths = getSelectedRows().ToArray();

			if (paths.Length == 0)
			{
                //10/6/2014 NS modified for VSPLUS-990
				errorDiv.InnerHtml = "No log files were selected."+
                    "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
				errorDiv.Style.Value = "display: block";
				return;
			}

			
			object[] paramsToSend = new object[] {paths, emailAddressToSendTo};

            //10/6/2014 NS modified for VSPLUS-990
			successDiv.InnerHtml = "Log files are being sent to " + EmailAddressTextBox.Text + ".  If they are not received in a few minutes, please refer to the VitalSigns Web Log file." +
                "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
			successDiv.Style.Value = "display: block";

			System.Threading.Tasks.Task task = new System.Threading.Tasks.Task(() => ZipFiles(paramsToSend));
			task.Start();
			
			
		}

        

		private void ZipFiles(object sender)
		{

			object[] paramsToReceive = sender as object[];


			string[] paths = paramsToReceive[0] as string[];
			string emailAddressToSendTo = paramsToReceive[1] as string;

			string logPath = "";

			logPath = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("Log Files Path");
			if (logPath == "")
			{
				logPath = AppDomain.CurrentDomain.BaseDirectory.ToString();
			}
			try
			{

                //string[] oldZipFiles = System.IO.Directory.GetFiles(logPath, "LogFiles.z*");
                string[] oldZipFiles = System.IO.Directory.GetFiles(Server.MapPath("~/LogFiles/"), "LogFiles.z*");
				foreach (string file in oldZipFiles)
					System.IO.File.Delete(file);


				ZipFile zip = new ZipFile();

				zip.AddFiles(paths);
				zip.MaxOutputSegmentSize = 3 * 1024 * 1024;
                //zip.Save(logPath + "LogFiles.zip");
                zip.Save(Server.MapPath("~/LogFiles/LogFiles.zip"));

                //string[] zipFiles = System.IO.Directory.GetFiles(logPath, "LogFiles.z*");
                string[] zipFiles = System.IO.Directory.GetFiles(Server.MapPath("~/LogFiles/"), "LogFiles.z*");


				string host = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("PrimaryHostName");
				string port = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("primaryport");
				bool auth = Convert.ToBoolean(VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("primaryAuth").ToString());
				string PEmail = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("primaryUserID");
				string Ppwd = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("primarypwd");
				bool PSSL = Convert.ToBoolean(VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("primarySSL").ToString());
				bool gmail = host.ToUpper().Contains("GMAIL");


				for (int i = 0; i < zipFiles.Length; i++)
				{

					string file = zipFiles[i];

                    MailMessage mail = new MailMessage();

                    SmtpClient SmtpServer = new SmtpClient(host);
                mail.From = new MailAddress(PEmail);
                mail.To.Add(emailAddressToSendTo);
                mail.Subject = "Log Files";
                mail.Body = "Log Files sent from " + Session["UserLogin"].ToString() + ".  File  " + (i + 1) + " of " + zipFiles.Length + ".";

                System.Net.Mail.Attachment attachment;
                attachment = new System.Net.Mail.Attachment(file);
                mail.Attachments.Add(attachment);

                SmtpServer.Port = Convert.ToInt32(port);
                SmtpServer.Credentials = new System.Net.NetworkCredential(PEmail, Ppwd);
                SmtpServer.EnableSsl = PSSL;

                SmtpServer.Send(mail);                   

				}		
				

			}
			catch (Exception ex)
			{
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Error zipping log files. Exception - " + ex.Message);
                errorDiv.InnerText = "error in sending log files";
			}

		}
	
        //private void ZipFiles(object sender, DoWorkEventArgs e)
        //{

        //    object[] paramsToReceive = e.Argument as object[];


        //    string[] paths = paramsToReceive[0] as string[];
        //    string emailAddressToSendTo = paramsToReceive[1] as string;

        //    string logPath = "";

        //    logPath = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("Log Files Path");
        //    if (logPath == "")
        //    {
        //        logPath = AppDomain.CurrentDomain.BaseDirectory.ToString();
        //    }
        //    try
        //    {

        //        string[] oldZipFiles = System.IO.Directory.GetFiles(logPath, "LogFiles.z*");
        //        foreach (string file in oldZipFiles)
        //            System.IO.File.Delete(file);


        //        ZipFile zip = new ZipFile();

        //        zip.AddFiles(paths);
        //        zip.MaxOutputSegmentSize = 3 * 1024 * 1024;
        //        zip.Save(logPath + "LogFiles.zip");

        //        string[] zipFiles = System.IO.Directory.GetFiles(logPath, "LogFiles.z*");


        //        string host = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("PrimaryHostName");
        //        string port = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("primaryport");
        //        bool auth = Convert.ToBoolean(VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("primaryAuth").ToString());
        //        string PEmail = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("primaryUserID");
        //        string Ppwd = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("primarypwd");
        //        bool PSSL = Convert.ToBoolean(VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("primarySSL").ToString());
        //        bool gmail = host.ToUpper().Contains("GMAIL");


        //        Chilkat.MailMan mailman = new Chilkat.MailMan();

        //        try
        //        {
        //            mailman.UnlockComponent("MZLDADMAILQ_8nzv7Kxb4Rpx");
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Error unlocking chilkat. Exception - " + ex.Message);
        //        }

        //        mailman.SmtpPort = Convert.ToInt32(port);
        //        mailman.SmtpHost = host;

        //        if (auth)
        //        {
        //            mailman.SmtpPassword = Ppwd;
        //            mailman.SmtpUsername = PEmail;
        //        }

        //        if (gmail && mailman.SmtpPort != 587)
        //        {
        //            mailman.SmtpPort = 587;
        //            mailman.SmtpSsl = true;
        //        }

        //        string errors = "";
        //        for (int i = 0; i < zipFiles.Length; i++)
        //        {

        //            string file = zipFiles[i];

        //            Chilkat.Email email = new Chilkat.Email();

        //            email.Body = "Log Files sent from " + Session["UserLogin"].ToString() + ".  File  " + (i + 1) + " of " + zipFiles.Length + ".";
        //            email.Subject = "Log Files";
        //            email.AddTo("RPR Wyatt", emailAddressToSendTo);
        //            email.FromAddress = PEmail;
        //            email.FromName = PEmail;

        //            email.AddFileAttachment(file);
        //            bool success = mailman.SendEmail(email);
        //            if (!success)
        //            {
        //                email = new Chilkat.Email();

        //                email.Body = "Log Files sent from " + Session["UserLogin"].ToString() + ".  File " + zipFiles[i] + " did not send due to an unknown error.";
        //                email.Subject = "Log Files";
        //                email.AddTo("RPR Wyatt", emailAddressToSendTo);
        //                email.FromAddress = PEmail;
        //                email.FromName = PEmail;

        //                mailman.SendEmail(email);
        //                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Error sending file " + zipFiles[i] + ".");

        //                continue;
        //            }
        //            System.IO.File.Delete(file);

        //        }


        //        mailman.Dispose();
				

        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Error zipping log files. Exception - " + ex.Message);
        //    }

        //}
		
	}
}