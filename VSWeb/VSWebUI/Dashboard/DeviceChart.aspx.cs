using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace VSWebUI
{
    public partial class WebForm3 : System.Web.UI.Page
    {
		protected void Page_PreInit(object sender, EventArgs e)
		{

			this.Master.contentCallEvent += new EventHandler(Master_ButtonClick);
		}

		private void Master_ButtonClick(object sender, EventArgs e)
		{

		}
        protected void Page_Load(object sender, EventArgs e)
        {
            string Name = Request.QueryString["Name"];
            string Type = Request.QueryString["Type"];

            DataTable statustab = VSWebBL.DashboardBL.DashboardBL.Ins.GetStatusChart(Name, Type);
            WebChartControl1.DataSource = statustab;
            WebChartControl1.DataBind();

        }
    }
}