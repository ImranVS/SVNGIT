using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DO;
using BL;
using System.Web.Security;
using System.Data;
using System.Net;
using System.Configuration;
using DevExpress.Web;

namespace License
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {


            if (!IsPostBack)
			{
				if (Request.QueryString["sessionExpaired"] == "expaired")
				{
					LoginHeader.InnerHtml = "Session Expired";
					ExpaireText.Visible = true;
					ExpaireText.InnerHtml="Your session expired due to inactivity.";

				}
                SignOut();

                if (Request.Cookies["UName"] != null)
                {
                    LoginTextBox.Text = Request.Cookies["UName"].Value;
                    RememberCheckBox.Checked = true;
                   
                }
            }
        }
        protected void SignOut()
        {
            try
            {
                //1/3/2014 NS commented out
                /*
                string message = "<script language=JavaScript>window.history.forward(1);</script>";

                if (!Page.ClientScript.IsStartupScriptRegistered("clientscript"))
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "clientscript", message, true);
                }
                */
                FormsAuthentication.SignOut();
                Session.Clear();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        protected void LoginButton_Click(object sender, EventArgs e)
        {
			//Users ReturnUsersObject = new Users();
			//ReturnUsersObject.LoginName = LoginTextBox.Text;
			//ReturnUsersObject = BL.UsersBL.Ins.GetData(ref ReturnUsersObject);


			//Session["UserID"] = ReturnUsersObject.ID;
			//DataTable sa = BL.UsersBL.Ins.GetIsAdmin(Session["UserID"].ToString());
			//if (sa.Rows.Count > 0)
			//{
				
			//        Session["UserType"] = sa.Rows[0]["UserType"].ToString();
				
			//}
			
			//Response.Redirect("~/LicenseGrid.aspx");
			//    Context.ApplicationInstance.CompleteRequest();
			

            if (RememberCheckBox.Checked == true)
            {
                
                Response.Cookies["UName"].Value = LoginTextBox.Text;
                // Response.Cookies["PWD"].Value = PasswordTextBox.Text;
                Response.Cookies["UName"].Expires = DateTime.Now.AddDays(double.Parse(ConfigurationSettings.AppSettings["Cookie_Expires"]));
                // Response.Cookies["PWD"].Expires = DateTime.Now.AddDays(15);
            }
            else
            {
                Response.Cookies["UName"].Expires = DateTime.Now.AddMonths(-1);
                Response.Cookies["PWD"].Expires = DateTime.Now.AddMonths(-1);
            }

            if (Session["Attempts"] == null || int.Parse(Session["Attempts"].ToString()) < 3)
            {
                VerifyUser();
            }
            else if (ValidateCaptcha.IsValid)
            {
                VerifyUser();
            }



        }

        public void VerifyUser()
        {

            Users UsersObject = new Users();
            Users ReturnUsersObject = new Users();
            UsersObject.LoginName = LoginTextBox.Text;
            UsersObject.Password = PasswordTextBox.Text;
		
            object ReturnValue = BL.UsersBL.Ins.VerifyUser(ref UsersObject);
            try
			{
				ReturnUsersObject = (Users)ReturnValue;
			
				
					
					ReturnUsersObject.LoginName = LoginTextBox.Text;
					ReturnUsersObject = BL.UsersBL.Ins.GetData(ref ReturnUsersObject);

					Session["UserFullName"] = ReturnUsersObject.FullName;
					Session["UserID"] = ReturnUsersObject.ID;
					DataTable sa = BL.UsersBL.Ins.GetIsAdmin(Session["UserID"].ToString());
					if (sa.Rows.Count > 0)
					{

						Session["UserType"] = sa.Rows[0]["UserType"].ToString();

					}
                    frmAuth();
                    Session["Attempts"] = null;

					Response.Redirect("~/LicenseGrid.aspx",false);
					Context.ApplicationInstance.CompleteRequest();
				
				//if (ReturnUsersObject.Password.IndexOf("temp_") == 0)
				//{
				//    Session["UserLogin"] = ReturnUsersObject.LoginName;
				//    Session["UserPassword"] = ReturnUsersObject.Password;
				//    Session["UserID"] = ReturnUsersObject.ID;
				//    frmAuth();
				//    Response.Redirect("~/Security/ForceChangePwd.aspx", false);
				//    Context.ApplicationInstance.CompleteRequest();
				//}
				//else
				//{


					
					//Session["UserID"] = ReturnUsersObject.ID;
					//Session["UserLogin"] = ReturnUsersObject.LoginName;
					//Session["UserPassword"] = ReturnUsersObject.Password;
					//Session["UserEmail"] = ReturnUsersObject.Email;
					//Session["UserSecurityQuestion1"] = ReturnUsersObject.SecurityQuestion1;
					//Session["UserSecurityQuestion1Answer"] = ReturnUsersObject.SecurityQuestion1Answer;
					//Session["UserSecurityQuestion2"] = ReturnUsersObject.SecurityQuestion2;
					//Session["UserSecurityQuestion2Answer"] = ReturnUsersObject.SecurityQuestion2Answer;



					
				//}
					Session["UserPreferences"] = BL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
			
			}
			catch{
				ErrorLabel.Text = ReturnValue.ToString();
				if (Session["Attempts"] == null)
				{
					Session["Attempts"] = 1;
				}
				else
				{
					Session["Attempts"] = int.Parse(Session["Attempts"].ToString()) + 1;
				}
				if (int.Parse(Session["Attempts"].ToString()) == 3) ValidateCaptcha.Visible = true;
				}

        }
		protected void frmAuth()
		{
			FormsAuthentication.RedirectFromLoginPage(LoginTextBox.Text, false);

		}
        protected void ResetButton_Click(object sender, EventArgs e)
        {

            LoginTextBox.Text = "";

            PasswordTextBox.Text = "";
        }
    
        protected void ForgotPwdLink_Click(object sender, EventArgs e)
        {
            Session["Username"] = LoginTextBox.Text;
            Response.Redirect("~/Security/ForgotPassword.aspx",false);
        }
    }
}