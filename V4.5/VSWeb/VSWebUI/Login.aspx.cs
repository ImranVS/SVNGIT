using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VSWebDO;
using VSWebBL;
using System.Web.Security;
using System.Data;
using System.Net;
using System.Configuration;
using DevExpress.Web;

namespace VSWebUI
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {


            if (!IsPostBack)
			{
				

				if (Request.QueryString["SessionExpired"] == "Expired")
				{
					LoginHeader.InnerHtml = "Session Expired";
					ExpaireText.Visible = true;
					ExpaireText.InnerHtml="Your session expired due to inactivity.";

				}

                SignOut();
                CreateMenu();

                string returnvalue = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("Show Dashboard only/Exec Summary Buttons ");
                

                if (returnvalue =="True" || returnvalue == "")
                {
                    DashOnlyButton.Visible = true;
                    SummaryButton.Visible = true;
                    Session["showsummary"] = "True";
                }
                else  {
                    DashOnlyButton.Visible = false;
                    SummaryButton.Visible = false;
                    Session["showsummary"] = "False";

                }

                //if (Request.Cookies["YourAppLogin"] != null)

                //   LoginTextBox.Text = Request.Cookies["YourAppLogin"].Values["username"];
                Session["BlackBerryServers"] = "";
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
                Session["Locations"] = "";

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
			//5/20/2015 NS modified for VSPLUS-1753
			string constate = "";
			
			VSWebBL.Adaptor objAdaptor = new VSWebBL.Adaptor();
			constate = objAdaptor.TestConnection();
			//Users UsersObject = new Users();
			//Users ReturnUsersObject = new Users();
			//bool success = VSWebBL.SecurityBL.UsersBL.Ins.GetIsFirstTimeLogin();
			//if (success == true)
			//{
			//    Response.Redirect("~/Security/MyAccount.aspx");
			//}
			
			

				if (constate == "")
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



					object ReturnValue = "";

					try
					{
						ReturnValue = VSWebBL.SecurityBL.UsersBL.Ins.VerifyUser(ref UsersObject);

						ReturnUsersObject = (Users)ReturnValue;


						//Session["Status"] = ReturnUsersObject.Status;
						//string status = Session["Status"].ToString();
						//if (status == "inactive")
						//{
						//    Response.Write("hi");

						//}


						//VSPLUS-1859:Sowjanya
						//Session["atemp"] = "";
						//if (ReturnUsersObject.Password.IndexOf("temp_") == 0)
						//{
						//    Session["atemp"] = "Admin";
						//}
						//else if (ReturnUsersObject.Password.IndexOf("Utemp_") == 0)
						//{
						//    Session["atemp"] = "User";
						//}
						//else
						//{
						//}
						Session["UserLogin"] = ReturnUsersObject.LoginName;
						Session["UserFullName"] = ReturnUsersObject.FullName;
						Session["UserPassword"] = ReturnUsersObject.Password;
						Session["UserID"] = ReturnUsersObject.ID;
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
						Session["CustomBackground"] = ReturnUsersObject.CustomBackground;
						frmAuth();
						Session["Attempts"] = null;
						bool success = VSWebBL.SecurityBL.UsersBL.Ins.GetIsFirstTimeLogin(Convert.ToInt32(Session["UserID"]));
						if (success == true)
						{
							Response.Redirect("~/Security/Welcome Users.aspx");
							Context.ApplicationInstance.CompleteRequest();
						}

						 if (ReturnUsersObject.Password.IndexOf("temp_") == 0 || ReturnUsersObject.Password.IndexOf("Utemp_") == 0)
						{
							 //sowjanya vsplus-1811
							//Session["UserLogin"] = ReturnUsersObject.LoginName;
							//Session["UserPassword"] = ReturnUsersObject.Password;
							//Session["UserID"] = ReturnUsersObject.ID;
							//frmAuth();
							Response.Redirect("~/Security/ForceChangePwd.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
							Context.ApplicationInstance.CompleteRequest();
						}
						else
						{

							//Sowjanya VSPLUS-1811
							//Session["UserFullName"] = ReturnUsersObject.FullName;
							//Session["UserID"] = ReturnUsersObject.ID;


							//5/17/2012 NS added session variables for login, password
							//Session["UserLogin"] = UsersObject.LoginName;
							//Session["UserPassword"] = UsersObject.Password;
							//Session["UserEmail"] = UsersObject.Email;
							//Session["UserSecurityQuestion1"] = UsersObject.SecurityQuestion1;
							//Session["UserSecurityQuestion1Answer"] = UsersObject.SecurityQuestion1Answer;
							//Session["UserSecurityQuestion2"] = UsersObject.SecurityQuestion2;
							//Session["UserSecurityQuestion2Answer"] = UsersObject.SecurityQuestion2Answer;

							//5/23/2012 Mukund Changed above commented for Users Object

							 //Sowjanya vsplus-1811
							//Session["UserLogin"] = ReturnUsersObject.LoginName;
							//Session["UserPassword"] = ReturnUsersObject.Password;
							//Session["UserEmail"] = ReturnUsersObject.Email;
							//Session["UserSecurityQuestion1"] = ReturnUsersObject.SecurityQuestion1;
							//Session["UserSecurityQuestion1Answer"] = ReturnUsersObject.SecurityQuestion1Answer;
							//Session["UserSecurityQuestion2"] = ReturnUsersObject.SecurityQuestion2;
							//Session["UserSecurityQuestion2Answer"] = ReturnUsersObject.SecurityQuestion2Answer;
							//Session["Isconfigurator"] = ReturnUsersObject.IsConfigurator;
							//Session["IsDashboard"] = ReturnUsersObject.Isdashboard;
							//Session["Refreshtime"] = ReturnUsersObject.Refreshtime;
							//Session["StartupURL"] = ReturnUsersObject.StartupURL;
							//Session["Isconsolecomm"] = ReturnUsersObject.Isconsolecomm;
							//Session["CustomBackground"] = ReturnUsersObject.CustomBackground;
							

							//frmAuth();
							//Session["Attempts"] = null;
							GetRestrictedServersandlocations();
							Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
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
							// Sowjanya
							//bool success = VSWebBL.SecurityBL.UsersBL.Ins.GetIsFirstTimeLogin(Convert.ToInt32(Session["UserID"]));
							//if (success == true)
							//{
							//    Response.Redirect("~/Security/Welcome Users.aspx");
								
							// }
							
							if (Request.QueryString["ReturnUrl"] != "/" && Request.QueryString["ReturnUrl"] != null)
							{
								//Response.Redirect(ViewState["PreviousPage"].ToString());
								//  FormsAuthentication.RedirectFromLoginPage(LoginTextBox.Text, false);
								if (Request.QueryString["ReturnUrl"].IndexOf("/Configurator") >= 0)
								{
									if (Convert.ToBoolean(Session["IsDashboard"].ToString()) == true && Convert.ToBoolean(Session["Isconfigurator"].ToString()) == false)
									{
										Response.Redirect("~/Dashboard/OverallHealth1.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
										//5/1 WS commented out due to crashes on Dashboard only and unsure why it was changed
										//Response.Redirect("~/Dashboard/ScanNowItems.aspx", false); 
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
									Response.Redirect(Session["StartupURL"].ToString());
								else
								{
									if (Convert.ToBoolean(Session["Isconfigurator"].ToString()) != false)
									{
										Response.Redirect("~/Configurator/Default.aspx");
									}
									else if (Convert.ToBoolean(Session["IsDashboard"].ToString()) == true && Convert.ToBoolean(Session["Isconfigurator"].ToString()) == false)
									{
										Response.Redirect("~/Dashboard/OverallHealth1.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
										//5/1 WS commented out due to crashes on Dashboard only and unsure why it was changed
										//Response.Redirect("~/Dashboard/ScanNowItems.aspx", false);
										Context.ApplicationInstance.CompleteRequest();
									}
								}
							}
						}
					}
					catch
					{
						//ErrorLabel.Text = ReturnValue.ToString();
						conerror.InnerHtml = ReturnValue.ToString();
						conerror.Style.Value = "display: block";
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
				else
				{
					//ErrorLabel.Text = constate;
					conerror.InnerHtml = "An error occurred while trying to connect to SQL Server. The server was not found or was not accessible.";
					conerror.Style.Value = "display: block";
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


		
        public void CreateMenu()
        {
            NavBarGroup group;
            NavBarItem item;
            group = new NavBarGroup("Anonymous Login Menu", "ref", "");
            item = new NavBarItem("Go to Dash Board", "DashBoard", "~/Dashboard/OverallHealth1.aspx", "~/Dashboard/OverallHealth1.aspx");


            group.Items.Add(item);
            MainMenu.Groups.Add(group);

            //set the IsDashbaord to true and set the user name to Anonymous
            Session["IsDashboard"] = "true";
            Session["UserFullName"] = "Anonymous";


        }

        protected void DashOnlyButton_Click(object sender, EventArgs e)
        {
            SignOut();
            Session["UserFullName"] = "Anonymous";
            Session["IsDashboard"] = "true";
            string returnvalue = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("Show Dashboard only/Exec Summary Buttons ");

            if (returnvalue =="True")
                
            {
                Session["showsummary"] = "True";
            }
            else
            {
                Session["showsummary"] = "False";
            }

          
			//Session["UserID"] = 1;
			Response.Redirect("~/Dashboard/OverallHealth1.aspx", false);
			//5/1 WS commented out due to crashes on Dashboard only and unsure why it was changed
			//Response.Redirect("~/Dashboard/ScanNowItems.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
            Context.ApplicationInstance.CompleteRequest();
        }


        protected void SummaryButton_Click(object sender, EventArgs e)
        {
            SignOut();
            Session["UserFullName"] = "Anonymous";
            Session["IsDashboard"] = "true";
            Session["UserFullName"] = "Anonymous";
            Session["divControl"] = "Summery";
            string returnvalue = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("Show Dashboard only/Exec Summary Buttons ");
            if (returnvalue == "True")
            {
                Session["showsummary"] = "True";
            }
            else
            {
                Session["showsummary"] = "False";
            }
            Response.Redirect("~/Dashboard/SummaryLandscape.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
            Context.ApplicationInstance.CompleteRequest();
        }
        protected void ForgotPwdLink_Click(object sender, EventArgs e)
        {
            Session["Username"] = LoginTextBox.Text;
            Response.Redirect("~/Security/ForgotPassword.aspx");
        }
    }
}