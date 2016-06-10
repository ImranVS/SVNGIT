using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VSWebBL;
using VSWebDO;
using System.Data;

using DevExpress.Web;
using System.Runtime.InteropServices;

namespace VSWebUI.Configurator
{
	public partial class DominoELSDefinitions_Grid : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				fillGrid();
				if (Session["UserPreferences"] != null)
				{
					DataTable UserPreferences = (DataTable)Session["UserPreferences"];
					foreach (DataRow dr in UserPreferences.Rows)
					{
						if (dr[1].ToString() == "ELSDefinitions_Grid|ELSDefGridView")
						{
							DELSDefGridView.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
						}
					}
				}
			}
			else
			{
				fillGridfromSession();


			}
            if (Session["EventName"] != null)
            {
                if (Session["EventName"].ToString() != "")
                {
                    //6/7/16 sowjanya added for VSPLUS-3004
                    successDiv.InnerHtml = "Domino information for <b>" + Session["EventName"].ToString() +
                        "</b> updated successfully." +
                        "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                    successDiv.Style.Value = "display: block";
                    Session["EventName"] = "";
                }
            }

		}

		public void fillGrid()
		{
			DataTable AlertsTable = new DataTable();
			AlertsTable = VSWebBL.ConfiguratorBL.DELSBL.Ins.GetDELSNames();
			try
			{
				Session["DELSNameTable"] = AlertsTable;
				DELSDefGridView.DataSource = AlertsTable;
				DELSDefGridView.DataBind();
			}
			catch (Exception ex)
			{
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw;
			}
		}

		public void fillGridfromSession()
		{
			DataTable EventsTable = new DataTable();
			if (Session["DELSNameTable"] != "" && Session["DELSNameTable"] != null)
				EventsTable = (DataTable)Session["DELSNameTable"];

			try
			{
				DELSDefGridView.DataSource = EventsTable;
				DELSDefGridView.DataBind();
			}
			catch (Exception ex)
			{
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw;
			}

		}

		protected void DELSDefGridView_HtmlRowCreated(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
		{
			e.Row.Attributes.Add("onmouseover", "this.originalcolor=this.style.backgroundColor;" + "this.style.backgroundColor='#C0C0C0';");

			e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalcolor;");


			if (e.RowType == GridViewRowType.EditForm)
			{

				try
				{
					if (e.GetValue("ID") != " " && e.GetValue("ID") != null)
					{
						ASPxWebControl.RedirectOnCallback("DominoELS_Edit.aspx?EventKey=" + e.GetValue("ID"));
						Context.ApplicationInstance.CompleteRequest();
					}
					else
					{
						ASPxWebControl.RedirectOnCallback("DominoELS_Edit.aspx");
						Context.ApplicationInstance.CompleteRequest();
					}
				}
				catch (Exception ex)
				{
					Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				}

			}
		}

		protected void DELSDefGridView_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
		{
			e.Cell.ToolTip = string.Format("{0}", e.CellValue);
		}

		protected void DELSDefGridView_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
		{
			int id = Convert.ToInt32(e.Keys[0]);
			try
			{
				VSWebBL.ConfiguratorBL.DELSBL.Ins.DeleteDELSDef(id);
			}
			catch (Exception ex)
			{
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
			}
			ASPxGridView gridview = (ASPxGridView)sender;
			gridview.CancelEdit();
			e.Cancel = true;
			fillGrid();

		}


		public static Control GetPostBackControl(Page page)
		{
			Control control = null;
			string ctrlname = page.Request.Params.Get("__EVENTTARGET");
			if (ctrlname != null && ctrlname != string.Empty)
			{
				control = page.FindControl(ctrlname);
			}
			else
			{
				foreach (string ctl in page.Request.Form.AllKeys)
				{
					Control c = page.FindControl(ctl) as Control;
					if (c is DevExpress.Web.ASPxButton)
					{
						control = c;
						break;
					}
				}
			}
			return control;
		}

		protected void DELSDefGridView_PageSizeChanged(object sender, EventArgs e)
		{
			
			VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("ELSDefinitions_Grid|ELSDefGridView", DELSDefGridView.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
			Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));

		}

		protected void NewButton_Click(object sender, EventArgs e)
		{
			Response.Redirect("~/Configurator/DominoELS_Edit.aspx", false);
			Context.ApplicationInstance.CompleteRequest();
		}
	}
}