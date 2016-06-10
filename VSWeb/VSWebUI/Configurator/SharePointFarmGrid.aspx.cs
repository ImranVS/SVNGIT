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
	public partial class SharePointFarmGrid : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{

				Session["Submenu"] = "";
				FillExchangeServerGrid();
				UI.Ins.GetUserPreferenceSession("MSServersGrid|MSServerGridView", SPFarmGrid);
				//if (Session["UserPreferences"] != null)
				//{
				//    DataTable UserPreferences = (DataTable)Session["UserPreferences"];
				//    foreach (DataRow dr in UserPreferences.Rows)
				//    {
				//        if (dr[1].ToString() == "MSServersGrid|MSServerGridView")
				//        {
				//            SPFarmGrid.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
				//        }
				//    }
				//}
			}
			else
			{

				FillExchangeServerGridfromSession();

			}
			//3/27/2014 MD added for
			if (Session["ExchangeUpdateStatus"] != null)
			{
				if (Session["ExchangeUpdateStatus"].ToString() != "")
				{
					//10/7/2014 NS modified for VSPLUS-990
					successDiv.InnerHtml = "SharePoint information for <b>" + Session["ExchangeUpdateStatus"].ToString() +
						"</b> updated successfully." +
						"<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
					successDiv.Style.Value = "display: block";
					Session["ExchangeUpdateStatus"] = "";
				}
			}
		}


		private void FillExchangeServerGrid()
		{
			try
			{

				DataTable DSTaskSettingsDataTable = new DataTable();

				DSTaskSettingsDataTable = VSWebBL.ConfiguratorBL.SharePointBAL.Ins.GetAllFarmData();
				if (DSTaskSettingsDataTable.Rows.Count > 0)
				{
					Session["SPFarmGrid"] = DSTaskSettingsDataTable;
					SPFarmGrid.DataSource = DSTaskSettingsDataTable;
					SPFarmGrid.DataBind();
				}
			}
			catch (Exception ex)
			{
				//6/27/2014 NS added for VSPLUS-634
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}
			finally { }
		}
		private void FillExchangeServerGridfromSession()
		{
			try
			{

				DataTable DSTaskSettingsDataTable = new DataTable();
				if (Session["SPFarmGrid"] != null && Session["SPFarmGrid"] != "")
					DSTaskSettingsDataTable = (DataTable)Session["SPFarmGrid"];//VSWebBL.ConfiguratorBL.ExchangePropertiesBL.Ins.GetAllData();
				if (DSTaskSettingsDataTable.Rows.Count > 0)
				{
					SPFarmGrid.DataSource = DSTaskSettingsDataTable;
					SPFarmGrid.DataBind();
				}
			}
			catch (Exception ex)
			{
				//6/27/2014 NS added for VSPLUS-634
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}
			finally { }
		}


		protected void SPFarmGrid_HtmlRowCreated(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
		{
			e.Row.Attributes.Add("onmouseover", "this.originalcolor=this.style.backgroundColor;" + "this.style.backgroundColor='#C0C0C0';");

			e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalcolor;");

			if (e.RowType == GridViewRowType.EditForm)
			{
				if (e.GetValue("ID").ToString() != " ")
				{
					//"DominoProperties.aspx?Key=" + e.GetValue("ID")
					//Mukund: VSPLUS-844, Page redirect on callback
					try
					{
						ASPxWebControl.RedirectOnCallback("SharePointFarm.aspx?Farm=" + e.GetValue("Farm"));
						Context.ApplicationInstance.CompleteRequest();//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
					}
					catch (Exception ex)
					{
						Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
						//throw ex;
					}

				}
			}
		}

		protected void SPFarmGrid_PageSizeChanged(object sender, EventArgs e)
		{
			//ProfilesGridView.PageIndex;
			VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("MSServersGrid|MSServerGridView", SPFarmGrid.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
			//Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
			UI.Ins.ChangeUserPreference("MSServersGrid|MSServerGridView", SPFarmGrid.SettingsPager.PageSize);
		}
	}
}