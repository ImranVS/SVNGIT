using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VSWebBL;
using System.Data;
using VSWebDO;

namespace VSWebUI.Security
{
    public partial class ChangePwd : System.Web.UI.Page
    {
        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (Request.QueryString["M"] == "C" && Request.QueryString["M"].ToString() != "")
            {
                this.MasterPageFile = "~/Site1.Master";

            }
            else if (Request.QueryString["M"] == "d" && Request.QueryString["M"].ToString() != "")
            {
                this.MasterPageFile = "~/DashboardSite.Master";

            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ErrorMsg.Text = "";
            pwdLabel.Text = "";
        }

        protected void CancelButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Configurator/Default.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
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
                        //12/31/2013 NS modified
                        //ErrorMsg.ForeColor = System.Drawing.Color.Black;
                        //ErrorMsg.Text = "Password changed successfully.";
                        successDiv.Style.Value = "display: block";
                        errorDiv.Style.Value = "display: none";
                    }
                }
                else
                {
                    //12/31/2013 NS modified
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
                //12/31/2013 NS modified
                //pwdLabel.Text="Password is incorrect.";
                successDiv.Style.Value = "display: none";
                //10/6/2014 NS modified for VSPLUS-990
				errorDiv.InnerHtml = "The current password value you entered is incorrect." +
                    "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                errorDiv.Style.Value = "display: block";
            }
        }    
    }
}