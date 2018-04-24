using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using VSWebBL;
using VSWebDO;

namespace VSWebUI.Configurator
{
	public partial class IBMConnectionsGrid : System.Web.UI.Page
	{
		int UserID;
		protected void Page_Load(object sender, EventArgs e)
		{
			 
            if (!IsPostBack)
            {
              
                FillIBMConnectionsGrid();
               
                if (Session["UserPreferences"] != null)
                {
                    DataTable UserPreferences = (DataTable)Session["UserPreferences"];
                    foreach (DataRow dr in UserPreferences.Rows)
                    {
                        if (dr[1].ToString() == "IBMConnectionsGrid|IBMConnectionsGridview")
                        {
							IBMConnectionsGridview.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                        }
                    }
                }
            }
            else
            {
                 FillIBMConnectionsGridBySession();
            }
            
			if (Session["IBMConnectionsStatus"] != null)
            {
				if (Session["IBMConnectionsStatus"].ToString() != "")
                {
                  
					successDiv.InnerHtml = "IBMConnections information for <b>" + Session["IBMConnectionsStatus"].ToString() +
                        "</b> updated successfully."+
                        "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                    successDiv.Style.Value = "display: block";
					Session["IBMConnectionsStatus"] = "";
                }
            }

        }

		public void FillIBMConnectionsGrid()
		{
			try
			{
				DataTable IBMConnectionsServers = new DataTable();
				DataTable ServerTypeTable = new  DataTable();
				//LotusSametimeGrid objectlotussametime = new LotusSametimeGrid();
				//sametime = VSWebBL.ConfiguratorBL.SametimeServerBL.Ins.getdata();
				if (Session["UserID"] != null)
				{
					UserID = Convert.ToInt32(Session["UserID"]);
				}
				//ServerTypeTable = VSWebBL.ConfiguratorBL.IBMConnectionsServersBL.Ins.GetServerType(ServerNameTextBox.Text);
				
				//if (ServerTypeTable.Rows.Count > 0)
				//{
				//    string ServerType = ServerTypeTable.Rows[0]["ServerType"].ToString();
				IBMConnectionsServers = VSWebBL.ConfiguratorBL.IBMConnectionsServersBL.Ins.GetdataforIBMConnectionsServersGridbyUser(UserID);

				if (IBMConnectionsServers.Rows.Count > 0)
				{
					if (Session["RestrictedServers"] != "" && Session["RestrictedServers"] != null)
					{
						List<int> ServerID = new List<int>();
						List<int> LocationID = new List<int>();
						DataTable resServers = (DataTable)Session["RestrictedServers"];
						foreach (DataRow dominorow in IBMConnectionsServers.Rows)
						{
							foreach (DataRow resser in resServers.Rows)
							{
								if (resser["serverid"].ToString() == dominorow["SID"].ToString())
								{
									ServerID.Add(IBMConnectionsServers.Rows.IndexOf(dominorow));
								}
								if (resser["locationID"].ToString() == dominorow["LocationID"].ToString())
								{
									LocationID.Add(Convert.ToInt32(dominorow["LocationID"].ToString()));
									
								}
							}

						}
						foreach (int Id in ServerID)
						{
							IBMConnectionsServers.Rows[Id].Delete();
						}
						IBMConnectionsServers.AcceptChanges();
						
						foreach (int lid in LocationID)
						{
							DataRow[] row = IBMConnectionsServers.Select("LocationID=" + lid + "");
							for (int i = 0; i < row.Count(); i++)
							{
								IBMConnectionsServers.Rows.Remove(row[i]);
								IBMConnectionsServers.AcceptChanges();
							}
						}
						IBMConnectionsServers.AcceptChanges();

					}
					Session["IBMConnectionsServers"] = IBMConnectionsServers;
					IBMConnectionsGridview.DataSource = IBMConnectionsServers;
					IBMConnectionsGridview.DataBind();
				}
			}
			catch (Exception ex)
			{
				//6/27/2014 NS added for VSPLUS-634
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}
			finally
			{
			}
		}

		public void FillIBMConnectionsGridBySession()
		{
			try
			{
				DataTable IBMConnectionsServersbysession = new DataTable();
				if (Session["IBMConnectionsServers"] != null && Session["IBMConnectionsServers"] != "")
					IBMConnectionsServersbysession = (DataTable)Session["IBMConnectionsServers"];
				if (IBMConnectionsServersbysession.Rows.Count > 0)
				{
					IBMConnectionsGridview.DataSource = IBMConnectionsServersbysession;
					IBMConnectionsGridview.DataBind();
				}
			}
			catch (Exception ex)
			{
				//6/27/2014 NS added for VSPLUS-634
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}
		}


		protected void IBMConnectionsGridview_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
		{
			//SametimeServers SametimeObject = new SametimeServers();
			IBMConnectionsServers IBMConnectionsObject = new IBMConnectionsServers();
			IBMConnectionsObject.ID = Convert.ToInt32(e.Keys[0]);
			//delete  a Row
			Object Stsobject = VSWebBL.ConfiguratorBL.IBMConnectionsServersBL.Ins.DeleteIBMConnectionsServers(IBMConnectionsObject);
			ASPxGridView griedview = (ASPxGridView)sender;
			griedview.CancelEdit();
			e.Cancel = true;
			FillIBMConnectionsGrid();
		}


		protected void IBMConnectionsGridview_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
		{
			
			e.Row.Attributes.Add("onmouseover", "this.originalcolor=this.style.backgroundColor;" + "this.style.backgroundColor='#C0C0C0';");

			e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalcolor;");
		
			if (e.RowType == GridViewRowType.EditForm)
			{
				if (e.GetValue("SID").ToString() != " ")
				{
					
					try
					{
						ASPxWebControl.RedirectOnCallback("IBMConnections.aspx?ID=" + e.GetValue("SID"));
						Context.ApplicationInstance.CompleteRequest();
					}
					catch (Exception ex)
					{
						Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
						//throw ex;
					}
				}
			}
		}

		protected void IBMConnectionsGridview_SelectionChanged(object sender, EventArgs e)
		{
			if (IBMConnectionsGridview.Selection.Count > 0)
			{
				System.Collections.Generic.List<object> Type = IBMConnectionsGridview.GetSelectedFieldValues("SID");

				if (Type.Count > 0)
				{
					string ID = Type[0].ToString();

					//Mukund: VSPLUS-844, Page redirect on callback
					try
					{
						DevExpress.Web.ASPxWebControl.RedirectOnCallback("IBMConnections.aspx?ID=" + ID + "");
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

		protected void IBMConnectionsGridview_PageSizeChanged(object sender, EventArgs e)
		{
			//ProfilesGridView.PageIndex;
			VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("IBMConnectionsGrid|IBMConnectionsGridview", IBMConnectionsGridview.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
			Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
		}


		
	}
}