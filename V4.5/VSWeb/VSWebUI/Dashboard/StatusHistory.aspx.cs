using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

using DevExpress.Web;

namespace VSWebUI.Dashboard
{
	public partial class StatusHistory : System.Web.UI.Page
	{

		protected void Page_PreInit(object sender, EventArgs e)
		{
			this.Master.contentCallEvent += new EventHandler(Master_ButtonClick);
		}
		private void Master_ButtonClick(object sender, EventArgs e)
		{
			FillGrid();
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				FillGrid();
                if (Session["UserPreferences"] != null)
                {
                    DataTable UserPreferences = (DataTable)Session["UserPreferences"];
                    foreach (DataRow dr in UserPreferences.Rows)
                    {
                        if (dr[1].ToString() == "StatusHistory|HistoryGridView")
                        {
                            HistoryGridView.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                        }
                    }
                }
			}
			else
			{
				//Mukund 05Nov13, Handle grid refresh Status List, when user expands a group. Timer refresh calls  Master_ButtonClick for DB data & expands all groups
				FillgridfromSession();

			}
		}

		private void FillGrid()
		{
			try
			{
				string statusThreshold = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("StatusChangedThreshold");
				int time;
				if (statusThreshold == "")
					time = 15;
				else
					time = Convert.ToInt32(statusThreshold);

				DataTable StatusTable = VSWebBL.DashboardBL.DashboardBL.Ins.GetDeviceStatusHistory(time);
				Session["HistoryTable"] = StatusTable;

				if (StatusTable.Rows.Count > 0)
				{
					if (Session["RestrictedServers"] != "" && Session["RestrictedServers"] != null)
					{
						List<int> ServerID = new List<int>();
						List<int> LocationID = new List<int>();
						DataTable resServers = (DataTable)Session["RestrictedServers"];
						foreach (DataRow resser in resServers.Rows)
						{
							foreach (DataRow dominorow in StatusTable.Rows)
							{

								if (resser["serverid"].ToString() == dominorow["ID"].ToString())
								{
									ServerID.Add(StatusTable.Rows.IndexOf(dominorow));
								}
							}

						}
						foreach (int Id in ServerID)
						{
							StatusTable.Rows[Id].Delete();
						}
						StatusTable.AcceptChanges();
					}


					HistoryGridView.DataSource = StatusTable;
					HistoryGridView.DataBind();
				}

				
			}
			catch (Exception ex)
			{
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}
		}

		public void FillgridfromSession()
		{
			if (Session["HistoryTable"] == null || Session["HistoryTable"] == "")
			{
				try
				{
					string statusThreshold = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("StatusChangedThreshold");
					int time;
					if (statusThreshold == "")
						time = 15;
					else
						time = Convert.ToInt32(statusThreshold);
					DataTable StatusTable = VSWebBL.DashboardBL.DashboardBL.Ins.GetDeviceStatusHistory(time);
					Session["HistoryTable"] = StatusTable;
				}
				catch (Exception ex)
				{
					Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
					throw ex;
				}
			}

			if (Session["HistoryTable"] != "" && Session["HistoryTable"] != null)
			{
				DataTable StatusTable = Session["HistoryTable"] as DataTable;
				try
				{
					HistoryGridView.DataSource = StatusTable;
					HistoryGridView.DataBind();
				}
				catch (Exception ex)
				{
					Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
					throw ex;
				}
				finally
				{
				}
			}
		}


		protected void HistoryGridView_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
		{
			ASPxGridView gv = sender as ASPxGridView;
			Label hfStatus = gv.FindRowCellTemplateControl(e.VisibleIndex, e.DataColumn, "hfNameLabel2") as Label;

			if (e.DataColumn.FieldName == "NewStatus" || e.DataColumn.FieldName == "OldStatus")
			{
				if (hfStatus.Text.ToString() == "Not Responding")
				{
					//e.DataColumn.CellStyle.CssClass = "GridCss3";
					e.Cell.CssClass = "GridCss3";
				}

				//6/10/2013 NS modified - added Telnet status
				if (hfStatus.Text.ToString() == "OK" || hfStatus.Text.ToString() == "Scanning")
				{
					e.Cell.BackColor = System.Drawing.Color.LightGreen;
				}

				else if (hfStatus.Text.ToString() == "Not Responding")
				{
					e.Cell.BackColor = System.Drawing.Color.Red;
					e.Cell.ForeColor = System.Drawing.Color.White;
					//e.DataColumn.CellStyle.CssClass = "GridCss3";
				}
				else if (hfStatus.Text.ToString() == "Not Scanned")
				{

					e.Cell.BackColor = System.Drawing.Color.FromName("#87CEEB");
					e.Cell.ForeColor = System.Drawing.Color.Black;
				}
				//3/1/2013 NS modified the value of text below - lower case disabled did not match the returned status with capital D
				else if (hfStatus.Text.ToString() == "Disabled")
				{
					e.Cell.BackColor = System.Drawing.Color.FromName("#C8C8C8");
					// e.Cell.ForeColor = System.Drawing.Color.White;
				}
				else if (hfStatus.Text.ToString() == "Maintenance")
				{
					e.Cell.BackColor = System.Drawing.Color.LightBlue;

				}
				else
				{
					e.Cell.BackColor = System.Drawing.Color.Yellow;
					// e.DataColumn.GroupFooterCellStyle.ForeColor = System.Drawing.Color.Yellow;

				}
			}
		}

		protected string GetImage(object val)
		{
			DateTime lastUpdated = DateTime.Parse(val.ToString());
			DateTime curr = DateTime.Now;

			string statusThreshold = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("StatusChangedThreshold");
			int time;
			if (statusThreshold == "")
				time = 15;
			else
				time = Convert.ToInt32(statusThreshold);

			if ((curr - lastUpdated).TotalMinutes < time)
			{
				double k = (curr - lastUpdated).TotalMinutes;
				return "~/images/icons/exclamation.png";
			}
			return "";
		}
         protected void HistoryGridView_OnPageSizeChanged(object sender, EventArgs e)
        {
            //ProfilesGridView.PageIndex;
            VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("StatusHistory|HistoryGridView", HistoryGridView.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        }
    }

	}
