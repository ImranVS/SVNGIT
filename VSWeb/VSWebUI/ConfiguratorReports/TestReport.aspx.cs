using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace VSWebUI.ConfiguratorReports
{
    public partial class TestReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
          
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            string str = dtPick.FromDate;
            string str1 = dtPick.ToDate;

            Label1.Text = "From Date: " + str + "<br>To Date: " + str1;
        }
    }
}