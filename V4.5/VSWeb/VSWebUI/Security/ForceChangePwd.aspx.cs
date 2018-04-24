using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VSWebBL;
using System.Data;
using VSWebDO;
using System.Web.Security;


namespace VSWebUI.Security
{
    public partial class ForceChangePwd : System.Web.UI.Page
    {
		protected void Page_PreInit(object sender, EventArgs e)
		{

			//this.Master.contentCallEvent += new EventHandler(Master_ButtonClick);
		}

		private void Master_ButtonClick(object sender, EventArgs e)
		{

		}

        protected void Page_Load(object sender, EventArgs e)
        {
            ErrorMsg.Text = "";
           // pwdLabel.Text = "";
            if(Session["UserLogin"]==null || Session["UserPassword"]==null)
                Response.Redirect("~/Login.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
            Context.ApplicationInstance.CompleteRequest();
			//if (Session["atemp"] == "Admin")
			//{
			//    labelDiv.InnerHtml = "Your password is reset by administrator. Please change the temporary  password.";
			//}
			//if (Session["atemp"]=="User")
			//{
			//    labelDiv.InnerHtml = "You have reset your password. Please change the temporary password.";
			//}
        }

        protected void CancelButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Login.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
            Context.ApplicationInstance.CompleteRequest();
        }

        protected void SaveButton_Click(object sender, EventArgs e)
        {
            if (CurrentpwdTextBox.Text == Session["UserPassword"].ToString())
            {
                if (NewpwdTextBox.Text == ConfirmpwdTextBox.Text)
                {
                    Users userobj = new Users();
                    userobj.ID = (int)Session["UserID"];
                    userobj.Password = NewpwdTextBox.Text;
                    bool Returnval = VSWebBL.SecurityBL.UsersBL.Ins.UpdateAccntPassword(userobj);

                    if (Returnval == true)
                    {
                        //01/23/2014 MD modified
                        //ErrorMsg.ForeColor = System.Drawing.Color.Black;
                        //ErrorMsg.Text = "Password changed successfully.";
                        successDiv.Style.Value = "display: block";
                        errorDiv.Style.Value = "display: none";
                        VerifyUser();
                    }
                }
                else
                {
                    //01/23/2014 MD modified
                    //ErrorMsg.Text = "Passwords don't match. Please make sure you enter the same password in the New and Confirm Password fields.";
                    successDiv.Style.Value = "display: none";
                    //10/6/2014 NS modified for VSPLUS-990
					errorDiv.InnerHtml = "Passwords do not match. Please make sure you enter the same password in the new and confirm password fields." +
                        "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                    errorDiv.Style.Value = "display: block";
                }
            }
            else
            {
                //01/23/2014 MD modified
                //pwdLabel.Text="Password is incorrect.";
                successDiv.Style.Value = "display: none";
                //10/6/2014 NS modified for VSPLUS-990
				errorDiv.InnerHtml = "The current password value you entered is incorrect." +
                    "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                errorDiv.Style.Value = "display: block";
            }
			Object AfterLogin = new Object();
			AfterLogin = VSWebBL.SecurityBL.UsersBL.Ins.UpdateIsFirstTimeLogin(Convert.ToInt32(Session["UserID"]));
        }


        public void VerifyUser()
        {

            Users UsersObject = new Users();
            Users ReturnUsersObject = new Users();
            UsersObject.LoginName = Session["UserLogin"].ToString();
            UsersObject.Password = NewpwdTextBox.Text;

            //5/17/2012 NS modified the pass by type to be able to get a handle on the UsersObject values

            //5/23/2012 Mukund Commented for return value type
            //string ReturnValue = VSWebBL.Security.UsersBL.Ins.VerifyUser(ref UsersObject);
            //if (ReturnValue.Substring(0, 4) == "User")
            //{
            object ReturnValue = VSWebBL.SecurityBL.UsersBL.Ins.VerifyUser(ref UsersObject);
            try
            {
                ReturnUsersObject = (Users)ReturnValue;
                Session["UserFullName"] = ReturnUsersObject.FullName;
                Session["UserID"] = ReturnUsersObject.ID;
             
                Session["UserLogin"] = ReturnUsersObject.LoginName;
                Session["UserPassword"] = ReturnUsersObject.Password;
                Session["UserEmail"] = ReturnUsersObject.Email;
                Session["UserSecurityQuestion1"] = ReturnUsersObject.SecurityQuestion1;
                Session["UserSecurityQuestion1Answer"] = ReturnUsersObject.SecurityQuestion1Answer;
                Session["UserSecurityQuestion2"] = ReturnUsersObject.SecurityQuestion2;
                Session["UserSecurityQuestion2Answer"] = ReturnUsersObject.SecurityQuestion2Answer;
                Session["Isconfigurator"] = ReturnUsersObject.IsConfigurator;
                Session["IsDashboard"] = ReturnUsersObject.Isdashboard;
                Session["Refreshtime"] = ReturnUsersObject.Refreshtime;
                Session["StartupURL"] = ReturnUsersObject.StartupURL;
                Session["Isconsolecomm"] = ReturnUsersObject.Isconsolecomm;

                frmAuth();
                Session["Attempts"] = null;
                GetRestrictedServersandlocations();


                if (Request.QueryString["ReturnUrl"] != "/" && Request.QueryString["ReturnUrl"] != null)
                {
                
                    if (Request.QueryString["ReturnUrl"].IndexOf("/Configurator") >= 0)
                    {
                        if (Convert.ToBoolean(Session["IsDashboard"].ToString()) == true && Convert.ToBoolean(Session["Isconfigurator"].ToString()) == false)
                        {
                            Response.Redirect("~/Dashboard/OverallHealth1.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
                            Context.ApplicationInstance.CompleteRequest();
                        }
                        else
                        {
                            Response.Redirect(Request.QueryString["ReturnUrl"], false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
                            Context.ApplicationInstance.CompleteRequest();
                        }
                    }
                    else
                    {
                        Response.Redirect(Request.QueryString["ReturnUrl"]);
                    }
                }
                else
                {
                    //VSPLUS-272 Go to Dashboard as default.
                    if (Session["StartupURL"].ToString() != "")
                    {
                        Response.Redirect(Session["StartupURL"].ToString(), false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
                        Context.ApplicationInstance.CompleteRequest();
                    }
                    else
                    {
                        if (Convert.ToBoolean(Session["Isconfigurator"].ToString()) != false)
                        {
                            Response.Redirect("~/Configurator/Default.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
                            Context.ApplicationInstance.CompleteRequest();
                        }
                        else if (Convert.ToBoolean(Session["IsDashboard"].ToString()) == true && Convert.ToBoolean(Session["Isconfigurator"].ToString()) == false)
                        {
                            Response.Redirect("~/Dashboard/OverallHealth1.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
                            Context.ApplicationInstance.CompleteRequest();
                        }
                    }
                }
                //}
            }
            catch(Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
               
            }

        }
        protected void frmAuth()
        {
            FormsAuthentication.RedirectFromLoginPage(Session["UserLogin"].ToString(), false);

        }

        private void GetRestrictedServersandlocations()
        {
            DataTable RestrictServers = new DataTable();
            int ID = Convert.ToInt32(Session["UserID"]);
            RestrictServers = VSWebBL.ConfiguratorBL.DominoPropertiesBL.Ins.Getrestrictedservers(ID);
            if (RestrictServers.Rows.Count > 0)
            {
                Session["RestrictedServers"] = RestrictServers;
            }

        }

    }
}