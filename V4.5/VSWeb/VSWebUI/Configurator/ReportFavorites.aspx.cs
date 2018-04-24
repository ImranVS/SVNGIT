using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.HtmlControls;

namespace VSWebUI.Configurator
{
    public partial class ReportFavorites : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DataTable dt = VSWebBL.ConfiguratorBL.ReportsBL.Ins.GetReportFavorites(Session["UserID"].ToString());
                if (dt.Rows.Count > 0)
                {
                    favoriteReports.Controls.RemoveAt(0);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        HtmlGenericControl li = new HtmlGenericControl("li");
                            favoriteReports.Controls.Add(li);
                        HtmlGenericControl anchor = new HtmlGenericControl("a");
                        anchor.Attributes.Add("href", dt.Rows[i]["PageURL"].ToString());
                        anchor.Attributes.Add("style", "color:Black;font-weight:normal;font-size:14px;font-family:Arial,Helvetica,sans-serif;");
                        anchor.Attributes.Add("onmouseover", "this.style.color='#00F'");
                        anchor.Attributes.Add("onmouseout", "this.style.color='black'");
                        anchor.InnerText = dt.Rows[i]["Name"].ToString();
                        li.Controls.Add(anchor);
                    }
                }
                DataTable dt2 = VSWebBL.ConfiguratorBL.ReportsBL.Ins.GetReportPopularity();
                if (dt2.Rows.Count > 0)
                {
                    popularReports.Controls.RemoveAt(0);
                    for (int i = 0; i < dt2.Rows.Count; i++)
                    {
                        HtmlGenericControl li = new HtmlGenericControl("li");
                        popularReports.Controls.Add(li);
                        HtmlGenericControl anchor = new HtmlGenericControl("a");
                        anchor.Attributes.Add("href", dt2.Rows[i]["PageURL"].ToString());
                        anchor.Attributes.Add("style", "color:Black;font-weight:normal;font-size:14px;font-family:Arial,Helvetica,sans-serif;");
                        anchor.Attributes.Add("onmouseover", "this.style.color='#00F'");
                        anchor.Attributes.Add("onmouseout", "this.style.color='black'");
                        anchor.InnerText = dt2.Rows[i]["Name"].ToString();
                        li.Controls.Add(anchor);
                    }
                }
                DataTable dt3 = VSWebBL.ConfiguratorBL.ReportsBL.Ins.GetReportTopRated();
                if (dt3.Rows.Count > 0)
                {
                    topReports.Controls.RemoveAt(0);
                    for (int i = 0; i < dt3.Rows.Count; i++)
                    {
                        HtmlGenericControl li = new HtmlGenericControl("li");
                        topReports.Controls.Add(li);
                        HtmlGenericControl anchor = new HtmlGenericControl("a");
                        anchor.Attributes.Add("href", dt3.Rows[i]["PageURL"].ToString());
                        anchor.Attributes.Add("style", "color:Black;font-weight:normal;font-size:14px;font-family:Arial,Helvetica,sans-serif;");
                        anchor.Attributes.Add("onmouseover", "this.style.color='#00F'");
                        anchor.Attributes.Add("onmouseout", "this.style.color='black'");
                        anchor.InnerText = dt3.Rows[i]["Name"].ToString();
                        li.Controls.Add(anchor);
                    }
                }
            }
        }

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
    }
}