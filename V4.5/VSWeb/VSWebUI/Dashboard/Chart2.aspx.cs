using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace VSDashboard
{
    public partial class Chart2 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //WebChartControl1.CrosshairEnabled = DevExpress.Utils.DefaultBoolean.True;
            if (!IsPostBack)
            {
                SetGraph("hh");
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
    }
}