using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace VSWebUI.Dashboard
{
    public partial class ServerDetails : System.Web.UI.Page
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
            WebChartControl1.Width = new Unit(Convert.ToInt32(chartWidth.Value));
            WebChartControl2.Width = new Unit(Convert.ToInt32(chartWidth.Value));
            WebChartControl3.Width = new Unit(Convert.ToInt32(chartWidth.Value));
            WebChartControl4.Width = new Unit(Convert.ToInt32(chartWidth.Value));
            //WebChartControl1.CrosshairEnabled = DevExpress.Utils.DefaultBoolean.True;
            if (!IsPostBack)
            {
                SetGraph("hh");
                
                HtmlGenericControl body = (HtmlGenericControl)Master.FindControl("Body1");
                body.Attributes.Add("onload", "DoCallback()");
                body.Attributes.Add("onResize", "Resized()");
            }
        }

        protected void ASPxRadioButtonList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetGraph(ASPxRadioButtonList1.Value.ToString());
        }
        private void SetGraph(string Duration)
        {
            if (Duration == "dd")
            {
                SqlDataSource1.SelectCommand = "SELECT Date,StatValue FROM [DominoDailyStats] where  ServerName='azphxdom1/RPRWyatt' and  [StatName]='Mem.PercentUsed' and Date > DATEADD (dd , -1 ,'2012-06-22 08:17:44.000') ";
                //SqlDataSource1.SelectCommand = "SELECT CONVERT(varchar, DATEPART ( dd, date ))+'-' +CONVERT(varchar, DATEPART ( hh, date )) as Date,AVG([StatValue]) as StatValue FROM [DominoDailyStats] where  ServerName='azphxdom1/RPRWyatt' and  [StatName]='Mem.PercentUsed' and Date > DATEADD (dd , -1 ,'2012-06-22 08:17:44.000') GROUP BY CONVERT(varchar, DATEPART ( dd, date ))+'-' +CONVERT(varchar, DATEPART ( hh, date )) order by CONVERT(varchar, DATEPART ( dd, date ))+'-' +CONVERT(varchar, DATEPART ( hh, date )) Asc";
            }
            else
            {
                SqlDataSource1.SelectCommand = "SELECT  [StatName],CONVERT(varchar, DATEPART ( hh, date )) + ':'+CONVERT(varchar, DATEPART ( n, date )) as Date,[StatValue] FROM [VSS_Statistics].[dbo].[DominoDailyStats] where ServerName='azphxdom1/RPRWyatt' and [StatName]='Mem.PercentUsed' and Date > DATEADD (" + Duration + " , -1 ,'2012-06-22 08:17:44.000' ) order by Date asc;";
            }
        }

        protected void WebChartControl1_CustomCallback(object sender, DevExpress.XtraCharts.Web.CustomCallbackEventArgs e)
        {
            WebChartControl1.Width = new Unit(Convert.ToInt32(chartWidth.Value));
        }

        protected void WebChartControl2_CustomCallback(object sender, DevExpress.XtraCharts.Web.CustomCallbackEventArgs e)
        {
            WebChartControl2.Width = new Unit(Convert.ToInt32(chartWidth.Value));
        }

        protected void WebChartControl3_CustomCallback(object sender, DevExpress.XtraCharts.Web.CustomCallbackEventArgs e)
        {
            WebChartControl3.Width = new Unit(Convert.ToInt32(chartWidth.Value));
        }

        protected void WebChartControl4_CustomCallback(object sender, DevExpress.XtraCharts.Web.CustomCallbackEventArgs e)
        {
            WebChartControl4.Width = new Unit(Convert.ToInt32(chartWidth.Value));
        }
    }
}