using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VSWebBL;
using System.Data;
using VSWebDO;
using System.Configuration;

namespace VSWebUI.Security
{
    public partial class MailChangePwd : System.Web.UI.Page
    {
		protected void Page_PreInit(object sender, EventArgs e)
		{

			this.Master.contentCallEvent += new EventHandler(Master_ButtonClick);
		}

		private void Master_ButtonClick(object sender, EventArgs e)
		{

		}
        static int pwduserID = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            ErrorMsg.Text = "";
            if (Request.QueryString["id"] == null)
            {
                Response.Redirect("~/Security/MaintainUsers.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
                Context.ApplicationInstance.CompleteRequest();
            }
            else
            {
                pwduserID = Convert.ToInt32(Request.QueryString["id"]);
                Users Userobj = new Users();
                Userobj.ID = pwduserID;
                DataTable dt = VSWebBL.SecurityBL.UsersBL.Ins.GetUserbyID(Userobj);
                //5/14/2015 NS modifed
                //ChangepwdRoundPanel.HeaderText = "Change password for " + dt.Rows[0]["FullName"].ToString();
                unameLbl.Text = dt.Rows[0]["FullName"].ToString();
            }
        }

        protected void CancelButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Security/MaintainUsers.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
            Context.ApplicationInstance.CompleteRequest();
        }

        protected void SaveButton_Click(object sender, EventArgs e)
        {
            Exception ex1;
            int id = pwduserID;  
            
            Users Userobj = new Users();
            Userobj.ID = id;
            string pwd = "temp_" + CreateRandomPassword(6);
            Userobj.Password = pwd;

            bool isSent = true;

            if (chkSendMail.Checked)
            {
                try
                {
                    DataTable dt = VSWebBL.SecurityBL.UsersBL.Ins.GetUserbyID(Userobj);
                    DataRow dr = dt.Rows[0];
                    Sendmail(dr, pwd);
                    isSent = true;
                }
                catch(Exception ex) 
                {
                    isSent = false;
                    successDiv.Style.Value = "display: none";
                    //10/6/2014 NS modified for VSPLUS-990
                    errorDiv.InnerHtml = "The following error has occurred: " + ex.Message + "<br />E-mail was not sent, password was not changed."+
                        "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                    errorDiv.Style.Value = "display: block";
                    Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                    
                }
               
            }
            if (isSent)
            { 
                bool s = VSWebBL.SecurityBL.UsersBL.Ins.UpdateAccntPassword(Userobj);

                Response.Redirect("~/Security/MaintainUsers.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
                Context.ApplicationInstance.CompleteRequest();

            }
        }
        public void Sendmail(DataRow UsersRow, string pwd)
        {

            //pass Generate Code
            //11/21/2013 NS modified
            string toEmailAddress;
            string subject;
            string mailbody;
            string[] mailparams = new string[5];
            Settings[] settingsObject = new Settings[5];
            Settings[] rtsettingsObject = new Settings[5];
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
            //12/4/2013 NS added SSL setting
            settingsObject[4].sname = "primarySSL";
            mailparams[0] = "smtp.gmail.com";
            mailparams[1] = ConfigurationSettings.AppSettings["AdminMailID"]; //"web.vitalsigns@gmail.com";
            mailparams[2] = ConfigurationSettings.AppSettings["Password"];        //"vitalsigns2012";
            mailparams[3] = "587";
            //12/4/2013 NS added SSL setting
            mailparams[4] = "true";
            for (int i = 0; i < 5; i++)
            {
                try
                {
                    rtsettingsObject[i] = VSWebBL.SettingBL.SettingsBL.Ins.GetData(settingsObject[i]);
                    if (rtsettingsObject[i].svalue == "" || rtsettingsObject[i].svalue == null)
                    {
                        //
                    }
                    else
                    {
                        mailparams[i] = rtsettingsObject[i].svalue;
                    }
                }
                catch (Exception ex)
                {
                    Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                    throw ex;
                }
            }

            toEmailAddress = UsersRow["Email"].ToString();
            subject = "Your VitalSigns Account Information Update";

            if (pwd!=null)
            {
                //11/20/2014 NS modified for VSPLUS-1190
                //mailbody = "Your VitalSigns Account Details: \nLoginName:" + UsersRow["LoginName"].ToString() + "\nPassword:" + pwd + "";
                mailbody = "Your VitalSigns account details are as follows: \n\rUser name: " + UsersRow["LoginName"].ToString() + "\nPassword:" + pwd + "";
            }
            else
            {
                //11/20/2014 NS modified for VSPLUS-1190
                //mailbody = "Your VitalSigns Account Details: \nLoginName: " + UsersRow["LoginName"].ToString() + "\nPassword :" + Session["randompwd"].ToString() + "";
                mailbody = "Your VitalSigns account details are as follows: \n\rUser name: " + UsersRow["LoginName"].ToString() + "\nPassword :" + Session["randompwd"].ToString() + "";
            }
            var client = new System.Net.Mail.SmtpClient(mailparams[0], Convert.ToInt32(mailparams[3]))
            {
                Credentials = new System.Net.NetworkCredential(mailparams[1], mailparams[2]),
                EnableSsl = Convert.ToBoolean(mailparams[4])
            };
            client.Send(mailparams[1], toEmailAddress, subject, mailbody);
        }

        private static string CreateRandomPassword(int passwordLength)
        {
            string allowedChars = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789!@$?_-";
            char[] chars = new char[passwordLength];
            Random rd = new Random();

            for (int i = 0; i < passwordLength; i++)
            {
                chars[i] = allowedChars[rd.Next(0, allowedChars.Length)];
            }

            return new string(chars);
        }
    }
}