using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using VSWebDO;
using DevExpress.Web;
using System.Data.OleDb;
using System.IO;
using VSWebBL;
using VSFramework;
using System.Configuration;
using System.Collections;
using System.Net.Mail;
using System.Net;
using Ionic.Zip;
using System.ComponentModel;



namespace VSWebUI.Configurator
{
	public partial class Feedback : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			
				ViewFeedbackGridView();
		
			
		}
		protected void fileupld_FileUploadComplete(object sender, FileUploadCompleteEventArgs e)
		{

			try
			{
				e.CallbackData = SavePostedFile(e.UploadedFile);
				ASPxLabel7.ForeColor = System.Drawing.Color.Green;

				ASPxLabel7.Text = "File uploaded successfully.";
			}
			catch (Exception ex)
			{
				e.IsValid = false;
				e.ErrorText = ex.Message;
			}
			


		}
		string SavePostedFile(UploadedFile uploadedFile)
		{
	
			
			fileupld.SaveAs(Server.MapPath("~/LogFiles/" + uploadedFile.FileName));
			
			string fileName1 = Server.MapPath("~/LogFiles/" + uploadedFile.FileName);
			
			string fileExtension = Path.GetExtension(uploadedFile.FileName);
			
			string filename2 = "~/LogFiles/" + (uploadedFile.FileName).Trim();
			Session["paths"] = fileName1;
			Session["filepath"] = filename2;
			return fileName1;
				
		}

		private List<String> getSelectedRows()
		{
			
			List<String> list = new List<String>();


			list.Add(Session["paths"].ToString());

		

			return list;
		}
		protected void SendButton_Click(object sender, EventArgs e)
		{
			try
			{
				InsertFeedbackData();

				string emailAddressToSendTo = "rprsupport@rprwyatt.com";
				//string emailAddressToSendTo = "rprsupport@rprwyatt.com";
				string[] paths = getSelectedRows().ToArray();
				BackgroundWorker bw = new BackgroundWorker();
				bw.DoWork += new DoWorkEventHandler(ZipFiles);

				object[] paramsToSend = new object[] { paths, emailAddressToSendTo };

				bw.RunWorkerAsync(paramsToSend);
				
				 SendingMail.ForeColor = System.Drawing.Color.Green;
				 ASPxLabel7.Visible = false;
				 SendingMail.Text = "Mail Sended successfully.";
				 SubjectTextBox.Text = null;
				 TypeTextBox.Text = null;
				 MessageMemo.Text = null;
				
			}
			catch (Exception ex)
			{
				//6/27/2014 NS added for VSPLUS-634
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}
			finally { }

		}


		private void ZipFiles(object sender, DoWorkEventArgs e)
		{
			
			DataTable dt = VSWebBL.ConfiguratorBL.FeedbackBL.Ins.GetCompanyName();
				string companyname = dt.Rows[0]["CompanyName"].ToString();
			object[] paramsToReceive = e.Argument as object[];


			string[] paths = paramsToReceive[0] as string[];

			//string paths = paramsToReceive[0].ToString();
			string emailAddressToSendTo = paramsToReceive[1] as string;

			string logPath = "";

			logPath = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("Log Files Path");
			if (logPath == "")
			{
				logPath = AppDomain.CurrentDomain.BaseDirectory.ToString();
			}
			try
			{

				string[] oldZipFiles = System.IO.Directory.GetFiles(logPath, "LogFiles.z*");
				foreach (string file in oldZipFiles)
					System.IO.File.Delete(file);


				ZipFile zip = new ZipFile();

				zip.AddFiles(paths);
				zip.MaxOutputSegmentSize = 3 * 1024 * 1024;
				zip.Save(logPath + "LogFiles.zip");

				string[] zipFiles = System.IO.Directory.GetFiles(logPath, "LogFiles.z*");


				string host = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("PrimaryHostName");
				string port = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("primaryport");
				bool auth = Convert.ToBoolean(VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("primaryAuth").ToString());
				string PEmail = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("primaryUserID");
				string Ppwd = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("primarypwd");
				bool PSSL = Convert.ToBoolean(VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("primarySSL").ToString());
				bool gmail = host.ToUpper().Contains("GMAIL");


				Chilkat.MailMan mailman = new Chilkat.MailMan();

				try
				{
					mailman.UnlockComponent("MZLDADMAILQ_8nzv7Kxb4Rpx");
				}
				catch (Exception ex)
				{
					Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Error unlocking chilkat. Exception - " + ex.Message);
				}

				mailman.SmtpPort = Convert.ToInt32(port);
				mailman.SmtpHost = host;

				if (auth)
				{
					mailman.SmtpPassword = Ppwd;
					mailman.SmtpUsername = PEmail;
				}

				if (gmail && mailman.SmtpPort != 587)
				{
					mailman.SmtpPort = 587;
					mailman.SmtpSsl = true;
				}

				string errors = "";
				for (int i = 0; i < zipFiles.Length; i++)
				{

					string file = zipFiles[i];

					Chilkat.Email email = new Chilkat.Email();

					//email.Body = "Log Files sent from " + Session["UserLogin"].ToString() + ".  File  " + (i + 1) + " of " + zipFiles.Length + ".";
					email.Body = "Feedback received from " + companyname + " Details: \n\rSubject: " + Session["subject"].ToString() + "\nType :" + Session["Type36"].ToString() + "\nMessage :" + Session[" Message"].ToString() + "\nStatus:" + Session["Status"].ToString() + "\nAttachments:" + Session["filepath"].ToString() + " ";
					email.Subject = "VSPlus-Feedback:CompanyName:" + companyname + ""; 
					email.AddTo("RPR Wyatt", emailAddressToSendTo);
					email.FromAddress = PEmail;
					email.FromName = PEmail;

					email.AddFileAttachment(file);
					bool success = mailman.SendEmail(email);
				
					if (!success)
					{
						email = new Chilkat.Email();

						email.Body = "Attachements sent from " + Session["UserLogin"].ToString() + ".  File " + zipFiles[i] + " did not send due to an unknown error.";
				
						email.Subject = "Log Files";
						email.AddTo("RPR Wyatt", emailAddressToSendTo);
						email.FromAddress = PEmail;
						email.FromName = PEmail;

						mailman.SendEmail(email);
						Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Error sending file " + zipFiles[i] + ".");

						continue;
					}
					System.IO.File.Delete(file);

				}


				mailman.Dispose();


			}
			catch (Exception ex)
			{
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Error zipping log files. Exception - " + ex.Message);
			}

		}

		private void InsertFeedbackData()

		{
			

			object ReturnValue = VSWebBL.ConfiguratorBL.FeedbackBL.Ins.InsertData(CollectDataforFeedback());
			
			
		}
		private FeedBack CollectDataforFeedback()
		{
			FeedBack fd = new FeedBack();
		

			try

			{
				fd.Subject = SubjectTextBox.Text;
				Session["subject"] = SubjectTextBox.Text;

				
				fd.Type = TypeTextBox.Text;
				Session["Type36"] = TypeTextBox.Text;
				
				fd.Message = MessageMemo.Text;
				Session[" Message"] = MessageMemo.Text;
				
				fd.Status = "Pending";
				Session["Status"] = "Pending";
				fd.Attachments = Session["filepath"].ToString();
				


				return fd;
			}
			catch (Exception ex)
			{
				
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}


		}
		




		protected void ViewFeedbackGridViews_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
		{

			FeedBack DataTable = new FeedBack();
			DataTable.ID = Convert.ToInt32(e.Keys[0]);

			bool ReturnValue = VSWebBL.ConfiguratorBL.FeedbackBL.Ins.UpdateFeedback(DataTable);
			ViewFeedbackGridView();
		}
		private void ViewFeedbackGridView()
		{
			
			DataTable dt = VSWebBL.ConfiguratorBL.FeedbackBL.Ins.GetFeedback();
			ViewFeedbackGridViews.DataSource = dt;
			ViewFeedbackGridViews.DataBind();
		}
		public string userpwd { get; set; }

		public DataRow newrow { get; set; }

		

		
	}
}