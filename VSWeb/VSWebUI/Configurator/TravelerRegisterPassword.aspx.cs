using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DevExpress.Web;
using System.Collections;
using VSWebBL;
using VSWebDO;


namespace VSWebUI.Configurator
{
	public partial class TravelerRegisterPassword : System.Web.UI.Page
	{
		int ID;
		protected void Page_Load(object sender, EventArgs e)
		{
			//ID = Convert.ToInt32(Request.QueryString["id"]);
			

			//'''''''''''
			if (Request.QueryString["id"] == null)
			{
				Response.Redirect("~/Configurator/TravelerDataStore.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
				Context.ApplicationInstance.CompleteRequest();
			}
			else
			{
				TravelerDS TravelerObject = new TravelerDS();
				ID = Convert.ToInt32(Request.QueryString["id"]);
				TravelerObject.ID = ID;
				DataTable dt = VSWebBL.ConfiguratorBL.TravelerBL.Ins.GetValuebyID(TravelerObject);
				string name = dt.Rows[0]["TravelerServicePoolName"].ToString();
				RegpwdRoundPanel.HeaderText = "Register Travelers Password for" + ":" + name;
				
				
				
			}


		}

		protected void OKButton_Click(object sender, EventArgs e)
		{

			TravelerDS TravelerObject = new TravelerDS();
			TravelerObject.Password = TravelPwdTextBox.Value.ToString();
			ID = Convert.ToInt32(Request.QueryString["id"]);
			TravelerObject.ID = ID;
			object returnvalue = VSWebBL.ConfiguratorBL.TravelerBL.Ins.UpdateTravelerPassword(TravelerObject);
			Response.Redirect("~/Configurator/TravelerDataStore.aspx");
		}

		protected void CancelButton_Click(object sender, EventArgs e)
		{


			Response.Redirect("~/Configurator/TravelerDataStore.aspx");

		}


	}
}