using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace VSWebUI.Security
{
    public partial class ImportServers4 : System.Web.UI.Page
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
            int count = 0;
            DataTable dt = new DataTable();
            dt = (DataTable)Session["ImportedServers"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                SrvLabel.Text += dt.Rows[i]["ServerName"].ToString() + ", ";
                count = i + 1;
            }
            SrvLabel.Text = SrvLabel.Text.Substring(0, SrvLabel.Text.Length - 2);
            SrvCountLabel.Text = count.ToString();
        }
 
        protected void DoneButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Configurator/Default.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
            Context.ApplicationInstance.CompleteRequest();
        }

        protected void ImportAddlButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Security/ImportServers.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
            Context.ApplicationInstance.CompleteRequest();
        }

    }
}