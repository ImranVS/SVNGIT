using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using VSWebDO;
using VSWebBL;
using System.Data;
using System.Configuration;

namespace VSWebUI.Security
{
	public partial class ForgotPassword : System.Web.UI.Page
	{
		//protected void Page_PreInit(object sender, EventArgs e)
		//{

		//    //this.Master.contentCallEvent += new EventHandler(Master_ButtonClick);
		//}

		//private void Master_ButtonClick(object sender, EventArgs e)
		//{

		//}

		protected void ForgotPwdVerifyAccountButton_Click(object sender, EventArgs e)
		{
			Users UsersObject = VerifyAccount();
			//Sowjanya vsplus-1811
			
			DataTable success = VSWebBL.SecurityBL.UsersBL.Ins.GetIsnotLoggedIn(ForgotPwdLoginNameTextBox.Text);
			//bool success = VSWebBL.SecurityBL.UsersBL.Ins.GetIsFirstTimeLogin(Convert.ToInt32(Session["UserID"]));
			if (success.Rows.Count > 0)
			{
				if (success.Rows[0]["IsFirstTimeLogin"].ToString() == "True")
				{
					ForgotPwdPanel.Style.Add("height", "0px");
					if(SortRadioButtonList1.Items.Count > 1)
					{
					SortRadioButtonList1.Items.RemoveAt(0);
					SortRadioButtonList1.Items[0].Selected = true;
					}
					ForgotPwdSecQuestion1Label.Visible = false;
					ForgotPwdSecQuestion1TextBox.Visible = false;
					ForgotPwdSecQuestion1AnsLabel.Visible = false;
					ForgotPwdSecQuestion1AnsTextBox.Visible = false;
					ForgotPwdSecQuestion2AnsLabel.Visible = false;
					ForgotPwdSecQuestion2TextBox.Visible = false;
					ASPxLabel2.Visible = false;
					ForgotPwdSecQuestion2AnsTextBox.Visible = false;
					ForgotPwdNewPasswordLabel.Visible = false;
					ForgotPwdNewPasswordTextBox.Visible = false;
					ForgotPwdConfirmPasswordLabel.Visible = false;
					ForgotPwdConfirmPasswordTextBox.Visible = false;
					Passwordlabel.Visible = false;
				}
				else
				{
					//SortRadioButtonList1.Properties.Items[0].Enabled = false;
				}

				//if (success == true)
				//{

				//    SortRadioButtonList1.Visible = false;
				//}
				//else
				//{
				//    SortRadioButtonList1.Visible = true;
				//}

				//code modified on 31/5/2012
				Session["SecurityQuestion1Ans"] = UsersObject.SecurityQuestion1Answer;
				Session["SecurityQuestion2Ans"] = UsersObject.SecurityQuestion2Answer;
				Session["PwdUserID"] = UsersObject.ID;
				if (UsersObject.Email == null || UsersObject.Email == "")
				{
					SortRadioButtonList1.Visible = false;
				}
				else
				{
					SortRadioButtonList1.Visible = true;
				}
				if (UsersObject.SecurityQuestion1 != null || UsersObject.SecurityQuestion2 != null)
				{
					ForgotPwdPanel.Visible = true;
					ForgotPwdSecQuestion1TextBox.Value = UsersObject.SecurityQuestion1.ToString().Replace("&quot;", "'");
					ForgotPwdSecQuestion2TextBox.Value = UsersObject.SecurityQuestion2.ToString().Replace("&quot;", "'");
					ForgotPwdVerifyAcctLabel.Visible = false;
					ForgotPwdPanel.Enabled = true;
					ForgotPwdResetPasswordButton.Enabled = true;
					ForgotPwdLoginNameTextBox.Enabled = false;
					ForgotPwdEmailTextBox.Enabled = false;

				}
				else
				{
					ForgotPwdPanel.Visible = false;
					ForgotPwdVerifyAcctLabel.Visible = true;
				}
			}
		}

		public Users VerifyAccount()
		{
			Users ReturnUsersObject = new Users();
			Users UsersObject = new Users();
			UsersObject.LoginName = ForgotPwdLoginNameTextBox.Text;
			UsersObject.Email = ForgotPwdEmailTextBox.Text;
			bool success = false;
			success = VSWebBL.SecurityBL.UsersBL.Ins.VerifyAccount(ref UsersObject);
			if (success)
			{
				ReturnUsersObject = VSWebBL.SecurityBL.UsersBL.Ins.GetData(ref UsersObject);
			}
			else
			{
				ForgotPwdVerifyAcctLabel.Visible = true;

			}
			return ReturnUsersObject;
		}

		private Users CollectDataForUsers()
		{
			try
			{
				Users UsersObject = new Users();

				UsersObject.Password = ForgotPwdNewPasswordTextBox.Text;
				UsersObject.ID = int.Parse(Session["PwdUserID"].ToString());


				return UsersObject;
			}
			catch (Exception ex)
			{
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}
			finally { }
		}




		protected void SortRadioButtonList1_SelectedIndexChanged(object sender, EventArgs e)
		{
			int questionoremail = Convert.ToInt32(SortRadioButtonList1.SelectedItem.Value);

			if (questionoremail == 2)
			{
				mailpanel.ClientVisible = true;
				ForgotPwdResetPasswordButton.Enabled = true;
				ForgotPwdPanel.ClientVisible = false;

				//ASPxPageControl1.TabPages[1].Enabled=true;
				//ASPxPageControl1.TabPages[].Visible = true;
				//ASPxPageControl1.TabPages[1].ClientVisible = true;
			}
			else
			{
				//ASPxPageControl1.TabPages[1].Enabled = false;
				//ASPxPageControl1.TabPages[1].Visible = false;
				//ASPxPageControl1.TabPages[1].ClientVisible = false;
				mailpanel.ClientVisible = false;
				ForgotPwdPanel.ClientVisible = true;

			}

		}


		protected void ForgotPwdResetPasswordButton_Click(object sender, EventArgs e)
		{
			// Users UsersObject = new Users();
			if (SortRadioButtonList1.SelectedItem.Value.ToString() == "1")
			{
				if (ForgotPwdSecQuestion1AnsTextBox.Text.ToUpper() == Session["SecurityQuestion1Ans"].ToString().ToUpper())
				{
					if (ForgotPwdSecQuestion2AnsTextBox.Text.ToUpper() == Session["SecurityQuestion2Ans"].ToString().ToUpper())
					{
						Object ReturnValue = VSWebBL.SecurityBL.UsersBL.Ins.UpdateAccntPassword(CollectDataForUsers());

						Response.Redirect("~/Login.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
						Context.ApplicationInstance.CompleteRequest();
					}
					else
					{

						ErrorMsgLabel.Text = "One or both of the answers to the security questions are incorrect. Cannot reset the password.";
						ForgotPwdPanel.Enabled = true;
						ForgotPwdResetPasswordButton.Enabled = true;
					}
				}
				else
				{
					ErrorMsgLabel.Text = "One or both of the answers to the security questions are incorrect. Cannot reset the password.";
					ForgotPwdPanel.Enabled = true;
					ForgotPwdResetPasswordButton.Enabled = true;
				}
			}


			if (SortRadioButtonList1.SelectedItem.Value.ToString() == "2")
			{




				Exception ex1;
				int id = int.Parse(Session["PwdUserID"].ToString());
				//string pwd;
				
				Users Userobj = new Users();
				Userobj.ID = id;
				//bool isadmin = false;
				//Session["name"] = ForgotPwdLoginNameTextBox.Text;
                //6/23/2015 NS modified - the query needs to check the ID, not LoginName
				//VSPLUS-1859: Before Login we didn't get ID so we have to take LoginName
				//DataTable sa = VSWebBL.SecurityBL.UsersBL.Ins.GetIsAdmin(ForgotPwdLoginNameTextBox.Text);
                //DataTable sa = VSWebBL.SecurityBL.UsersBL.Ins.GetIsAdmin(Session["UserID"].ToString());
				//if (sa.Rows.Count > 0)
				//{
				//    if (sa.Rows[0]["SuperAdmin"].ToString() == "Y")
				//    {
				//        isadmin = true;
				//        pwd = "temp_" + CreateRandomPassword(6);
				//        Userobj.Password = pwd;
				//        //Session["Apwd"] = Userobj.Password;
				//    }
				//    else
				//    {
				//        pwd = "Utemp_" + CreateRandomPassword(6);
				//        Userobj.Password = pwd;
				//        //Session["Upwd"] = Userobj.Password;
				//    }

					string pwd = "temp_" + CreateRandomPassword(6);
					Userobj.Password = pwd;

					bool isSent = true;

					//if (chkSendMail.Checked)
					//{
					try
					{
							DataTable dt = VSWebBL.SecurityBL.UsersBL.Ins.GetUserbyID(Userobj);
							DataRow dr = dt.Rows[0];
							Sendmail(dr, pwd);
							isSent = true;
						}
						catch (Exception ex)
						{
							isSent = false;
							successDiv.Style.Value = "display: none";
							//10/6/2014 NS modified for VSPLUS-990
							errorDiv.InnerHtml = "The following error has occurred: " + ex.Message + "<br />E-mail was not sent, password was not changed." +
								"<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
							errorDiv.Style.Value = "display: block";
							Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);

						}

					//}
					if (isSent)
					{
						bool s = VSWebBL.SecurityBL.UsersBL.Ins.UpdateAccntPassword(Userobj);

						Response.Redirect("~/Login.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
						Context.ApplicationInstance.CompleteRequest();

					}
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

			if (pwd != null)
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

		protected void ForgotPwdCancelButton_Click(object sender, EventArgs e)
		{
			Response.Redirect("~/Login.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
			Context.ApplicationInstance.CompleteRequest();
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				if (Session["UserName"] != null)
				{
					ForgotPwdLoginNameTextBox.Text = Session["UserName"].ToString();
					unameLbl.Text = Session["UserName"].ToString();
				}
				else
				{
					ForgotPwdLoginNameTextBox.Text = "";
					unameLbl.Text = "";
					unameLbl.Enabled = false;
				}
			}
			ForgotPwdPanel.Enabled = false;
			ForgotPwdResetPasswordButton.Enabled = false;
		}


	}
}