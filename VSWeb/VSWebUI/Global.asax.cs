using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace VSWebUI
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            SetVSWebPath();
        }
        //Mukund 1704
        protected void SetVSWebPath()
        {
            try
            {
                Settings st = new Settings();
                st.sname = "VSWebPath";
                st.svalue = Server.MapPath("~/bin/");
                st.stype = "System.String";
                VSWebBL.SettingBL.SettingsBL.Ins.UpdateSettings(st, "VSWebPath");
            }
            catch (Exception ex)
            {

                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Global.asax Exception - " + ex);
                //throw ex;
            }
        }
        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}