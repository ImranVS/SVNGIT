using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

namespace VSWebUI
{
    public partial class SessionExpired : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            SignOut();
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

			Response.Redirect("~/Login.aspx?SessionExpired=" + "Expired", false);

        }

        protected void SignOut()
        {
            try
            {
                string message = "<script language=JavaScript>window.history.forward(1);</script>";

                if (!Page.ClientScript.IsStartupScriptRegistered("clientscript"))
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "clientscript", message, true);
                }

                FormsAuthentication.SignOut();
                Session.Clear();
                //Response.Redirect("~/Login.aspx");

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        protected void LoginButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("Login.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
            Context.ApplicationInstance.CompleteRequest();
        }
    }
}