using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

namespace VSWebUI
{
	public partial class ErrorPage : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			//if (Session["Isconfigurator"] == null)
			//{
			//    btnConfigurator.Enabled = false;
			//}
			//else if (Session["Isconfigurator"].ToString() == "False")
			//{
			//    btnConfigurator.Enabled = false;
			//}
			
            //if (Session["IsDashboard"] == null)
            //{
            //    btnDashboard.Enabled = false;
            //}
            //else if (Session["IsDashboard"].ToString() == "False")
            //{
            //    btnDashboard.Enabled = false;
            //}
			if (btnDashboard.Enabled == false && btnConfigurator.Enabled == false)
			{

				Loginbt.Visible = true;
			}
		}
        protected void Page_PreInit(object sender, EventArgs e)
        {

        }
		protected void LoginButton_Click(object sender, EventArgs e)
		{

            Response.Redirect("~/Login.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
            Context.ApplicationInstance.CompleteRequest();
		}

        protected void btnDashboard_Click(object sender, EventArgs e)
        {
            if (Session["UserID"] != null)
            {
                if (Session["UserID"] != "")
                {
                    Response.Redirect("~/Dashboard/Default.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
                    Context.ApplicationInstance.CompleteRequest();
                }
                else
                {
                    Session["UserFullName"] = "Anonymous";
                    Session["IsDashboard"] = "true";
                    Session["UserFullName"] = "Anonymous";
                    Response.Redirect("~/Dashboard/OverallHealth1.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
                    Context.ApplicationInstance.CompleteRequest();
                }
            }
            else
            {
                Session["UserFullName"] = "Anonymous";
                Session["IsDashboard"] = "true";
                Session["UserFullName"] = "Anonymous";
                Response.Redirect("~/Dashboard/OverallHealth1.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
                Context.ApplicationInstance.CompleteRequest();
            }
            
        }
        protected void btnConfigurator_Click(object sender, EventArgs e)
        {
			if (Session["Isconfigurator"] == null || Session["Isconfigurator"].ToString() == "False")
			{
				Response.Redirect("~/Login.aspx");
			}
            Response.Redirect("~/Configurator/Default.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
            Context.ApplicationInstance.CompleteRequest();
        }
	}
}