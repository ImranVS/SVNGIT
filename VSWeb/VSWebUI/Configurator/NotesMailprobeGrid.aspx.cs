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
	public partial class NotesMailprobeGrid : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{

			if (!IsPostBack)
			{
				FillNotesMailProbeGrid();
				FillNotesMailProbeHistoryGrid();
				if (Session["UserPreferences"] != null)
				{
					DataTable UserPreferences = (DataTable)Session["UserPreferences"];
					foreach (DataRow dr in UserPreferences.Rows)
					{
						if (dr[1].ToString() == "NotesMailprobeGrid|NotesMailProbeGridView")
						{
							NotesMailProbeGridView.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
						}
						if (dr[1].ToString() == "NotesMailprobeGrid|MailProbeHistoryGridView")
						{
							MailProbeHistoryGridView.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
						}
					}
				}
			}

			else
			{

				FillNotesMailProbeGridfromSession();
				FillNotesMailProbeGridHistoryfromSession();

			}
			//2/20/2014 NS added
			if (Session["NotesMailProbeUpdateStatus"] != null)
			{
				if (Session["NotesMailProbeUpdateStatus"].ToString() != "")
				{
					//10/6/2014 NS modified for VSPLUS-990
					successDiv.InnerHtml = "NotesMail Probe information for <b>" + Session["NotesMailProbeUpdateStatus"].ToString() +
						"</b> updated successfully." +
						"<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
					successDiv.Style.Value = "display: block";
					Session["NotesMailProbeUpdateStatus"] = "";
				}
			}
		}


		private void FillNotesMailProbeGrid()
		{
			try
			{

				DataTable DSTaskSettingsDataTable = new DataTable();

				DSTaskSettingsDataTable = VSWebBL.ConfiguratorBL.NotesMailProbeBL.Ins.GetAllData();
				if (DSTaskSettingsDataTable.Rows.Count >= 0)//delete the single row in grid--somaraj
				{
					if (Session["RestrictedServers"] != "" && Session["RestrictedServers"] != null)
					{
						List<int> ServerID = new List<int>();
						List<int> LocationID = new List<int>();
						DataTable resServers = (DataTable)Session["RestrictedServers"];
						foreach (DataRow resser in resServers.Rows)
						{
							foreach (DataRow Windowsrow in DSTaskSettingsDataTable.Rows)
							{

								if (resser["serverid"].ToString() == Windowsrow["ID"].ToString())
								{
									ServerID.Add(DSTaskSettingsDataTable.Rows.IndexOf(Windowsrow));
								}
								if (resser["locationID"].ToString() == Windowsrow["locationid"].ToString())
								{
									LocationID.Add(Convert.ToInt32(Windowsrow["locationid"].ToString()));
									//LocationID.Add(DSTaskSettingsDataTable.Rows.IndexOf(Windowsrow));
								}
							}

						}
						foreach (int Id in ServerID)
						{
							DSTaskSettingsDataTable.Rows[Id].Delete();
						}
						DSTaskSettingsDataTable.AcceptChanges();

						//foreach (int Lid in LocationID)
						//{
						//    DSTaskSettingsDataTable.Rows[Lid].Delete();
						//}
						foreach (int lid in LocationID)
						{
							DataRow[] row = DSTaskSettingsDataTable.Select("locationid=" + lid + "");
							for (int i = 0; i < row.Count(); i++)
							{
								DSTaskSettingsDataTable.Rows.Remove(row[i]);
								DSTaskSettingsDataTable.AcceptChanges();
							}
						}
						DSTaskSettingsDataTable.AcceptChanges();
					}
					Session["NotesMailProbe"] = DSTaskSettingsDataTable;
					this.NotesMailProbeGridView.DataSource = DSTaskSettingsDataTable;
					this.NotesMailProbeGridView.DataBind();
					//Response.Redirect("~/Configurator/NotesMailprobeGrid.aspx");
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




		private void FillNotesMailProbeGridfromSession()
		{
			try
			{

				DataTable DCTaskSettingsDataTable = new DataTable();
				if (Session["NotesMailProbe"] != "" && Session["NotesMailProbe"] != null)
					DCTaskSettingsDataTable = (DataTable)Session["NotesMailProbe"];//VSWebBL.ConfiguratorBL.DominoPropertiesBL.Ins.GetAllData();
				if (DCTaskSettingsDataTable.Rows.Count > 0)
				{
					//Server.Transfer("~/Configurator/NotesMailprobeGrid.aspx");
					this.NotesMailProbeGridView.DataSource = DCTaskSettingsDataTable;
					this.NotesMailProbeGridView.DataBind();
					
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





		protected void NotesMailProbeGridView_HtmlRowCreated(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
		{
			if (e.RowType == GridViewRowType.EditForm)
			{

				//Mukund: VSPLUS-844, Page redirect on callback
				try
				{
					if (e.GetValue("Name") != null && e.GetValue("Name") != "")
					{
						ASPxWebControl.RedirectOnCallback("EditNotesMailProbe.aspx?Name=" + e.GetValue("Name"));
						Context.ApplicationInstance.CompleteRequest();//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback

					}
					else
					{
						ASPxWebControl.RedirectOnCallback("EditNotesMailProbe.aspx");
						Context.ApplicationInstance.CompleteRequest();//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
					}
				}
				catch (Exception ex)
				{
					Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
					//throw ex;
				}
			}

		}

		protected void NotesMailProbeGridView_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
		{
			try
			{


				NotesMailProbe MailProbObject = new NotesMailProbe();
				MailProbObject.Name = e.Keys[0].ToString();
				//Delete row from DB
				Object ReturnValue = VSWebBL.ConfiguratorBL.NotesMailProbeBL.Ins.DeleteData(MailProbObject);

				//Update Grid after inserting new row, refresh grid as in page load
				ASPxGridView gridView = (ASPxGridView)sender;
				gridView.CancelEdit();
				e.Cancel = true;
				//this.NotesMailProbeGridView.UpdateEdit();
				//Response.Redirect("~/Configurator/ NotesMailprobeGrid.aspx");
				FillNotesMailProbeGrid();
			}
			catch (Exception ex)
			{
				//6/27/2014 NS added for VSPLUS-634
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}
		}

		private void FillNotesMailProbeHistoryGrid()
		{
			try
			{

				DataTable ProbHistoryDataTable = new DataTable();

				ProbHistoryDataTable = VSWebBL.ConfiguratorBL.NotesMailProbeBL.Ins.GetAllHistoryData();
				if (ProbHistoryDataTable.Rows.Count > 0)
				{

					Session["NotesMailProbeHistory"] = ProbHistoryDataTable;
					MailProbeHistoryGridView.DataSource = ProbHistoryDataTable;
					MailProbeHistoryGridView.DataBind();
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

		private void FillNotesMailProbeGridHistoryfromSession()
		{
			try
			{

				DataTable HistoryDataTable = new DataTable();
				if (Session["NotesMailProbeHistory"] != null && Session["NotesMailProbeHistory"] != "")
					HistoryDataTable = (DataTable)Session["NotesMailProbeHistory"];//VSWebBL.ConfiguratorBL.DominoPropertiesBL.Ins.GetAllData();
				if (HistoryDataTable.Rows.Count > 0)
				{
					MailProbeHistoryGridView.DataSource = HistoryDataTable;
					MailProbeHistoryGridView.DataBind();
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
		protected void NotesMailProbeGridView_PageSizeChanged(object sender, EventArgs e)
		{
			//ProfilesGridView.PageIndex;
			VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("NotesMailprobeGrid|NotesMailProbeGridView", NotesMailProbeGridView.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
			Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
		}
		protected void MailProbeHistoryGridView_PageSizeChanged(object sender, EventArgs e)
		{
			//ProfilesGridView.PageIndex;
			VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("NotesMailprobeGrid|MailProbeHistoryGridView", MailProbeHistoryGridView.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
			Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
		}

		protected void NewButton_Click(object sender, EventArgs e)
		{
			//3/2/2015 NS added for VSPLUS-1432
			Response.Redirect("~/Configurator/EditNotesMailProbe.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
			Context.ApplicationInstance.CompleteRequest();
		}




	}
}