using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using VSWebBL;
using DevExpress.Web;

namespace VSWebUI.Configurator
{
	public partial class BusinessHoursGrid : System.Web.UI.Page
    {
		//int id;
		//DataTable BusinessDataTable = null;
		static int id = 0;

		protected void Page_Load(object sender, EventArgs e)
		{

			//Div1.Style.Value = "display:none";
			FillBusinesshoursgrid();
			UI.Ins.GetUserPreferenceSession("BusinessHoursGrid|BusinessrGridView", BusinessrGridView);

		}

		private void FillBusinesshoursgrid()
		{
			try
			{
				//DataTable dt = VSWebBL.ConfiguratorBL.FeedbackBL.Ins.GetFeedback();
				DataTable dt = VSWebBL.ConfiguratorBL.Businesshoursbl.Ins.GetBusinessHours();
				//DataTable DefaultGMT = VSWebBL.ConfiguratorBL.Businesshoursbl.Ins.GetDefaultGMT();

				//Session["DefaultGMT"] = DefaultGMT.Rows[0]["GMT"].ToString();
				//string Defaultgmt = Session["DefaultGMT"].ToString();
				BusinessrGridView.DataSource = dt;
				BusinessrGridView.DataBind();


			}
			catch (Exception ex)
			{
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}
			finally { }
		}
		protected void BusinessrGridView_HtmlRowCreated(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
		{
			//if (e.RowType == GridViewRowType.EditForm)
			//{
			//    ASPxWebControl.RedirectOnCallback("DominoProperties.aspx?Key=" + e.GetValue("ID"));
			//}
			e.Row.Attributes.Add("onmouseover", "this.originalcolor=this.style.backgroundColor;" + "this.style.backgroundColor='#C0C0C0';");

			e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalcolor;");
			//1/7/2014 NS added
			if (e.RowType == GridViewRowType.EditForm)
			{
				try
					{
						if (e.GetValue("ID") != "" && e.GetValue("ID") != null)
						{
							//Mukund: VSPLUS-844, Page redirect on callback

							Response.Redirect("MaintenanceBusinessHrs.aspx?Key=" + e.GetValue("ID"), false);
							Context.ApplicationInstance.CompleteRequest();//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
						}
						else
						{
							Response.Redirect("MaintenanceBusinessHrs.aspx", false);
							Context.ApplicationInstance.CompleteRequest();

						}
					}
					catch (Exception ex)
					{
						Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
						//throw ex;
					}
				
			}
		}

		protected void BusinessrGridView_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
		{

			try
			{
				
			HoursIndicator businessobj = new HoursIndicator();

			businessobj.ID = Convert.ToInt32(e.Keys[0]);
			DataTable dt = VSWebBL.ConfiguratorBL.Businesshoursbl.Ins.GetBusiandOffhoursName(businessobj);
				
			if (dt.Rows[0]["Type"].ToString() == "Business Hours")
			{
			
			}
			
				
				
			else{
					//Div1.Style.Value = "display:none";
					Object ReturnValue = VSWebBL.ConfiguratorBL.Businesshoursbl.Ins.DeleteBusinessHoursDetails(businessobj);
				if(!Convert.ToBoolean(ReturnValue))
				{
					MsgLabel.Text = "This Hours Definition may not be deleted. Please re-assign any servers (Maintain Servers) and/or alert definitions (Alert Definitions) before deleting this record.";
					NavigatorPopupControl.ShowOnPageLoad = true;

				}	
					
			}
			}
			catch (Exception ex)
			{
				//6/27/2014 NS added for VSPLUS-634
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
			}

			ASPxGridView gridview = (ASPxGridView)sender;
			gridview.CancelEdit();
			e.Cancel = true;
			FillBusinesshoursgrid();
		}
		protected void btn_Click(object sender, EventArgs e)
		{
			ImageButton btn = (ImageButton)sender;
			// btn.Attributes["onClick"]=
			HoursIndicator ServerObject = new HoursIndicator();
			ServerObject.ID = Convert.ToInt32(btn.CommandArgument);
			id = Convert.ToInt32(btn.CommandArgument);
			string name = btn.AlternateText;
			pnlAreaDtls.Style.Add("visibility", "visible");
			divmsg.InnerHtml = "Are you sure you want to delete the " + name + "?";

		}
		protected void btn_OkClick(object sender, EventArgs e)
		{
			HoursIndicator businessobj = new HoursIndicator();
			businessobj.ID = id;
			Object ReturnValue = VSWebBL.ConfiguratorBL.Businesshoursbl.Ins.DeleteBusinessHoursDetails(businessobj);
					
			if (Convert.ToBoolean(ReturnValue) == false)
			{
				NavigatorPopupControl.ShowOnPageLoad = true;
				MsgLabel.Text = "This Business Hours cannot be deleted, other dependencies exist.";
			}
			else
			{
				
				FillBusinesshoursgrid();
				Response.Redirect("~/Configurator/BusinessHoursGrid.aspx", false);
			}

			

		}
	
		protected void OKButton_Click(object sender, EventArgs e)
		{
			NavigatorPopupControl.ShowOnPageLoad = false;
			Response.Redirect("~/Configurator/BusinessHoursGrid.aspx", false);
		}
		protected void btn_CancelClick(object sender, EventArgs e)
		{

			FillBusinesshoursgrid();
			Response.Redirect("~/Configurator/BusinessHoursGrid.aspx", false);
			
		}

        protected void NewButton_Click(object sender, EventArgs e)
        {
            //4/15/2015 NS added for VSPLUS-1432
            Response.Redirect("~/Configurator/MaintenanceBusinessHrs.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
            Context.ApplicationInstance.CompleteRequest();
        }

        private DevExpress.Utils.DefaultBoolean DeleteButtonVisibleCriteria(ASPxGridView grid, int visibleIndex)
        {
            string bushrsname = grid.GetRowValues(visibleIndex, "Type").ToString();
            return (bushrsname != "Business Hours") == true ? DevExpress.Utils.DefaultBoolean.True : DevExpress.Utils.DefaultBoolean.False;
            //return (bushrsname != "Business Hours");
        }

        protected void BusinessrGridView_CustomButtonInitialize(object sender, ASPxGridViewCustomButtonEventArgs e)
        {
            if (e.VisibleIndex == -1) return;

            if (e.ButtonID == "deleteButton")
            {
                e.Visible = DeleteButtonVisibleCriteria((ASPxGridView)sender, e.VisibleIndex);
            }
        }
		protected void BusinessrGridView_PageSizeChanged(object sender, EventArgs e)
		{
			//ProfilesGridView.PageIndex;
			//DataTable table = new DataTable()
			VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("BusinessHoursGrid|BusinessrGridView", BusinessrGridView.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
			//	Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
			UI.Ins.ChangeUserPreference("BusinessHoursGrid|BusinessrGridView", BusinessrGridView.SettingsPager.PageSize);

		}

	}
}