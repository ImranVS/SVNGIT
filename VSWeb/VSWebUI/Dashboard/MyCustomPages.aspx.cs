using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DevExpress.Web;
using System.Web.UI.HtmlControls;
using VSWebDO;

namespace VSWebUI.Dashboard
{
    public partial class MyCustomPages : System.Web.UI.Page
    {
		protected void Page_PreInit(object sender, EventArgs e)
		{

			this.Master.contentCallEvent += new EventHandler(Master_ButtonClick);
		}

		private void Master_ButtonClick(object sender, EventArgs e)
		{

		}

        protected void ASPxButton1_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Dashboard/CustomPageConfig.aspx?M=d", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
            Context.ApplicationInstance.CompleteRequest();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                bool isadmin = false;
                Company companyvar = new Company();
                companyvar = VSWebBL.ConfiguratorBL.CompanyBL.Ins.GetLogo();
                //6/19/2015 NS modified
                //Label3.Text = companyvar.CompanyName;
                //Label2.Text = Session["UserFullName"].ToString() + "'s";
                title2Div.InnerHtml = companyvar.CompanyName + " custom pages";
                titleDiv.InnerHtml = Session["UserFullName"].ToString() + "'s" + " custom pages";
                DataTable sa = VSWebBL.SecurityBL.UsersBL.Ins.GetIsAdmin(Session["UserID"].ToString());
                if (sa.Rows.Count > 0)
                {
                    if (sa.Rows[0]["SuperAdmin"].ToString() == "Y")
                    {
                        isadmin = true;
                    }
                }
                DataTable dt = VSWebBL.DashboardBL.DashboardBL.Ins.GetMyCustomPages(Session["UserID"].ToString(),false,isadmin);
                if (dt.Rows.Count > 0)
                {
                    privatePages.Controls.RemoveAt(0);
                    publicPages.Controls.RemoveAt(0);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        HtmlGenericControl li = new HtmlGenericControl("li");
                        if (dt.Rows[i]["IsPrivate"].ToString() == "True")
                        {
                            privatePages.Controls.Add(li);
                        }
                        else
                        {
                            publicPages.Controls.Add(li);
                        }
                        HtmlGenericControl anchor = new HtmlGenericControl("a");

                   
                       

                       
                        anchor.Attributes.Add("href", "/Configurator/WebReport1.aspx?url=" + dt.Rows[i]["URL"].ToString());
                        
                        anchor.Attributes.Add("style", "color:Black;font-weight:normal;font-size:14px;font-family:Arial,Helvetica,sans-serif;");
                        anchor.Attributes.Add("onmouseover", "this.style.color='#00F'");
                        anchor.Attributes.Add("onmouseout", "this.style.color='black'");
                        anchor.InnerText = dt.Rows[i]["Title"].ToString();
                        li.Controls.Add(anchor);
                    }
                }
            }
        }
    }
}