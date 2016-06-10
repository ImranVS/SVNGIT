using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CustomerTrackingDO;
using CustomerServiceBL;
using System.Web.Security;
using System.Data;
using System.Net;
using System.Configuration;
using DevExpress.Web.ASPxNavBar;

namespace CustomerTracking
{
    public partial class Login : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {


            if (!IsPostBack)
            {
                SignOut();
                CreateMenu();
                //if (Request.Cookies["YourAppLogin"] != null)

                //   LoginTextBox.Text = Request.Cookies["YourAppLogin"].Values["username"];
                /*Session["BlackBerryServers"] = "";
                Session["MaintServers"] = "";
                Session["BlackBerryDevicePrbegrid"] = "";
                Session["DominoCluster"] = "";
                Session["DominoCustom"] = "";
                Session["NotesDB"] = "";
                Session["DominoServer"] = "";
                Session["sametime"] = "";
                Session["MailServices"] = "";
                Session["MaintServers"] = "";
                Session["NetworkDevices"] = "";
                Session["NotesDatabase"] = "";
                Session["NotesMailProbe"] = "";
                Session["URLs"] = "";
                Session["ServerVisibleDataGrid"] = "";
                Session["visible"] = "";
                Session["ServerNotVisibleDataGrid"] = "";
                Session["NotVisible"] = "";
                Session["Servers"] = "";
                Session["Users"] = "";
                Session["Locations"] = "";*/

                if (Request.Cookies["UName"] != null)
                {
                    LoginTextBox.Text = Request.Cookies["UName"].Value;
                    RememberCheckBox.Checked = true;
                    //if (Request.Cookies["PWD"] != null)
                    //    PasswordTextBox.Text.Attributes.Add("value", Request.Cookies["PWD"].Value);
                    // if (Request.Cookies["UName"] != null) && Request.Cookies["PWD"] != null)
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
            if (RememberCheckBox.Checked == true)
            {
                //HttpCookie cookie = new HttpCookie("YourAppLogin");
                //  cookie.Values.Add("username", LoginTextBox.Text);
                //  cookie.Expires = DateTime.Now.AddDays(15);
                //  Response.Cookies.Add(cookie);

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

            //5/17/2012 NS modified the pass by type to be able to get a handle on the UsersObject values

            //5/23/2012 Mukund Commented for return value type
            //string ReturnValue = VSWebBL.Security.UsersBL.Ins.VerifyUser(ref UsersObject);
            //if (ReturnValue.Substring(0, 4) == "User")
            //{
            object ReturnValue = CustomerServiceBL.UsersBL.Ins.VerifyUser(ref UsersObject);

            try
            {
                ReturnUsersObject = (Users)ReturnValue;

                if (ReturnUsersObject.Password.IndexOf("temp_") == 0)
                {
                    Session["UserLogin"] = ReturnUsersObject.LoginName;
                    Session["UserPassword"] = ReturnUsersObject.Password;
                    Session["UserID"] = ReturnUsersObject.ID;
                    frmAuth();
                    Response.Redirect("~/Security/ForceChangePwd.aspx");
                }
                else
                {


                    Session["UserFullName"] = ReturnUsersObject.FullName;
                    Session["UserID"] = ReturnUsersObject.ID;
                    //5/17/2012 NS added session variables for login, password
                    //Session["UserLogin"] = UsersObject.LoginName;
                    //Session["UserPassword"] = UsersObject.Password;
                    //Session["UserEmail"] = UsersObject.Email;
                    //Session["UserSecurityQuestion1"] = UsersObject.SecurityQuestion1;
                    //Session["UserSecurityQuestion1Answer"] = UsersObject.SecurityQuestion1Answer;
                    //Session["UserSecurityQuestion2"] = UsersObject.SecurityQuestion2;
                    //Session["UserSecurityQuestion2Answer"] = UsersObject.SecurityQuestion2Answer;

                    //5/23/2012 Mukund Changed above commented for Users Object
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
                    //GetRestrictedServersandlocations();
                    //Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
                    DataTable UserPreferences = (DataTable)Session["UserPreferences"];
                    foreach (DataRow dr in UserPreferences.Rows)
                    {
                        if (dr[1].ToString() == "ViewBy")
                        {
                            Session["ViewBy"] = dr[2].ToString();
                        }
                        if (dr[1].ToString() == "FilterBy")
                        {
                            Session["FilterByValue"] = dr[2].ToString();
                        }
                    }
                    //if (Convert.ToBoolean(Session["Isconfigurator"].ToString()) == true && Convert.ToBoolean(Session["IsDashboard"].ToString()) == false)
                    //{
                    //    Response.Redirect("~/Configurator/Default.aspx");
                    //}
                    //else if (Convert.ToBoolean(Session["IsDashboard"].ToString()) == true && Convert.ToBoolean(Session["Isconfigurator"].ToString()) == false)
                    //{
                    //2/13/2013 NS modified
                    //if (ViewState["PreviousPage"] != null)
                    if (Request.QueryString["ReturnUrl"] != "/" && Request.QueryString["ReturnUrl"] != null)
                    {
                        //Response.Redirect(ViewState["PreviousPage"].ToString());
                        //  FormsAuthentication.RedirectFromLoginPage(LoginTextBox.Text, false);
                        if (Request.QueryString["ReturnUrl"].IndexOf("/Configurator") >= 0)
                        {
                            if (Convert.ToBoolean(Session["IsDashboard"].ToString()) == true && Convert.ToBoolean(Session["Isconfigurator"].ToString()) == false)
                            {
                                Response.Redirect("~/Dashboard/OverallHealth1.aspx");
                            }
                            else
                            {
                                Response.Redirect(Request.QueryString["ReturnUrl"]);
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
                            Response.Redirect(Session["StartupURL"].ToString());
                        else
                        {
                            if (Convert.ToBoolean(Session["Isconfigurator"].ToString()) != false)
                            {
                                Response.Redirect("~/Configurator/Default.aspx");
                            }
                            else if (Convert.ToBoolean(Session["IsDashboard"].ToString()) == true && Convert.ToBoolean(Session["Isconfigurator"].ToString()) == false)
                            {
                                Response.Redirect("~/Dashboard/OverallHealth1.aspx");
                            }
                        }
                    }
                }
            }
            catch
            {
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
        /*private void GetRestrictedServersandlocations()
        {
            DataTable RestrictServers = new DataTable();
            int ID = Convert.ToInt32(Session["UserID"]);
            RestrictServers = VSWebBL.ConfiguratorBL.DominoPropertiesBL.Ins.Getrestrictedservers(ID);
            if (RestrictServers.Rows.Count > 0)
            {
                Session["RestrictedServers"] = RestrictServers;
            }

        }*/

        public void CreateMenu()
        {
            NavBarGroup group;
            NavBarItem item;
            group = new NavBarGroup("Anonymous Login Menu", "ref", "");
            item = new NavBarItem("Go to Dash Board", "DashBoard", "~/Dashboard/OverallHealth1.aspx", "~/Dashboard/OverallHealth1.aspx");
            group.Items.Add(item);
            //MainMenu.Groups.Add(group);

            //set the IsDashbaord to true and set the user name to Anonymous
            Session["IsDashboard"] = "true";
            Session["UserFullName"] = "Anonymous";


        }

        protected void DashOnlyButton_Click(object sender, EventArgs e)
        {
            SignOut();
            Session["IsDashboard"] = "true";
            Response.Redirect("~/Dashboard/OverallHealth1.aspx");
        }

        protected void ForgotPwdLink_Click(object sender, EventArgs e)
        {
            Session["Username"] = LoginTextBox.Text;
            Response.Redirect("~/Security/ForgotPassword.aspx");
        }
    }
}