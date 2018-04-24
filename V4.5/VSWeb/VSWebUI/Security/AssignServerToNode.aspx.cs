using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

using DevExpress.Web;
using DevExpress.Web.ASPxTreeList;
using VSWebBL;
using VSWebDO;
using System.Reflection;

namespace VSWebUI.Security
{
	public partial class AssignServerToNode : System.Web.UI.Page
	{
		//static int ID = 0;

		//GridViewCommandColumn ComandColumn { get { return (GridViewCommandColumn)AssignNodesGridView.Columns[0]; } }

		protected void Page_PreInit(object sender, EventArgs e)
		{

			this.Master.contentCallEvent += new EventHandler(Master_ButtonClick);
		}

		private void Master_ButtonClick(object sender, EventArgs e)
		{

		}

		protected void Page_Load(object sender, EventArgs e)
		{
			pnlAreaDtls.Style.Add("visibility", "hidden");

			FillNodesGrid();

			if (!IsPostBack && !IsCallback)
			{
				DetailsGrid_CustomCallback(null, null);
				//selectAllMode.DataSource = Enum.GetValues(typeof(GridViewSelectAllCheckBoxMode));
				//selectAllMode.DataBind();
				//selectAllMode.SelectedIndex = 1;
				Session["AssignNodes"] = null;
				Session["AssignNodeServer"] = null;
				FillAssignNodes();
				FillAssignNodesgrid();
				//FillNodesSettingsGrid();

				//Sowjanya 1528
				UI.Ins.GetUserPreferenceSession("AssignNodesGridView|AssignNodesGridView", AssignNodesGridView);
				UI.Ins.GetUserPreferenceSession("AssignServerToNode|NodesGrid", NodesGrid);
				UI.Ins.GetUserPreferenceSession("AssignServerToNode|servicesGrid", servicesGrid);

				//if (Session["UserPreferences"] != null)
				//{
				//    DataTable UserPreferences = (DataTable)Session["UserPreferences"];
				//    foreach (DataRow dr in UserPreferences.Rows)
				//    {
				//        if (dr[1].ToString() == "NodesGridView|NodesGridView")
				//        {
				//            NodesGrid.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
				//        }
				//        if (dr[1].ToString() == "AssignNodesGridView|AssignNodesGridView")
				//        {
				//            AssignNodesGridView.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
				//        }
				//        if (dr[1].ToString() == "AssignServerToNode|NodesGrid")
				//        {
				//            NodesGrid.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
				//        }
				//        if (dr[1].ToString() == "AssignServerToNode|servicesGrid")
				//        {
				//            servicesGrid.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
				//        }
				//    }
				//}

			}
			else
			{
				//FillNodeSettingsGridfromSession();
				FillAssignNodesgridfromSession();
				//Sowjanya 1735 while filtering data comes from session
				FillServicesgridfromSession();
			}
			if (Session["AssignNodes"] != null)
			{
				if (Session["AssignNodes"].ToString() != "")
				{

					NodeSuccess.InnerHtml = "WebSphere Server information for <b>" + Session["AssignNodes"].ToString() +
						"</b> updated successfully." +
						"<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
					NodeSuccess.Style.Value = "display: block";
					Session["AssignNodes"] = "";
				}
			}
		}
		/*
		private void FillNodesSettingsGrid()
		{
			try
			{

				DataTable NodesSettings = new DataTable();
				NodesSettings = VSWebBL.SecurityBL.NodesBL.Ins.GetAllDatafromNodes();
				if (NodesSettings.Rows.Count > 0)
				{
					if (Session["RestrictedServers"] != "" && Session["RestrictedServers"] != null)
					{
						List<int> ServerID = new List<int>();
						List<int> LocationID = new List<int>();
						DataTable resServers = (DataTable)Session["RestrictedServers"];
						foreach (DataRow resser in resServers.Rows)
						{
							foreach (DataRow Windowsrow in NodesSettings.Rows)
							{

								if (resser["serverid"].ToString() == Windowsrow["ID"].ToString())
								{
									ServerID.Add(NodesSettings.Rows.IndexOf(Windowsrow));
								}
								if (resser["locationID"].ToString() == Windowsrow["locationid"].ToString())
								{
									LocationID.Add(Convert.ToInt32(Windowsrow["locationid"].ToString()));
								}
							}

						}
						foreach (int Id in ServerID)
						{
							NodesSettings.Rows[Id].Delete();
						}
						NodesSettings.AcceptChanges();
						foreach (int lid in LocationID)
						{
							DataRow[] row = NodesSettings.Select("locationid=" + lid + "");
							for (int i = 0; i < row.Count(); i++)
							{
								NodesSettings.Rows.Remove(row[i]);
								NodesSettings.AcceptChanges();
							}
						}
						NodesSettings.AcceptChanges();
					}
					Session["NodeServer"] = NodesSettings;
					NodesGridView.DataSource = NodesSettings;
					NodesGridView.DataBind();
				}
			}
			catch (Exception ex)
			{
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}
			finally { }
		}
		private void FillNodeSettingsGridfromSession()
		{
			try
			{

				DataTable NodesSettings = new DataTable();
				if (Session["NodeServer"] != null && Session["NodeServer"] != "")
					NodesSettings = (DataTable)Session["NodeServer"];//VSWebBL.ConfiguratorBL.WindowsPropertiesBL.Ins.GetAllData();
				if (NodesSettings.Rows.Count > 0)
				{
					NodesGridView.DataSource = NodesSettings;
					NodesGridView.DataBind();
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
		*/
		private void FillAssignNodesgrid()
		{
			try
			{

				DataTable ASSignNodesSettings = new DataTable();
				ASSignNodesSettings = VSWebBL.SecurityBL.NodesBL.Ins.GetforAssignNodes();
				if (ASSignNodesSettings.Rows.Count > 0)
				{
					if (Session["RestrictedServers"] != "" && Session["RestrictedServers"] != null)
					{
						List<int> ServerID = new List<int>();
						List<int> LocationID = new List<int>();
						DataTable resServers = (DataTable)Session["RestrictedServers"];
						foreach (DataRow resser in resServers.Rows)
						{
							foreach (DataRow Windowsrow in ASSignNodesSettings.Rows)
							{

								if (resser["serverid"].ToString() == Windowsrow["ID"].ToString())
								{
									ServerID.Add(ASSignNodesSettings.Rows.IndexOf(Windowsrow));
								}
								if (resser["locationID"].ToString() == Windowsrow["locationid"].ToString())
								{
									LocationID.Add(Convert.ToInt32(Windowsrow["locationid"].ToString()));
								}
							}

						}
						foreach (int Id in ServerID)
						{
							ASSignNodesSettings.Rows[Id].Delete();
						}
						ASSignNodesSettings.AcceptChanges();
						foreach (int lid in LocationID)
						{
							DataRow[] row = ASSignNodesSettings.Select("locationid=" + lid + "");
							for (int i = 0; i < row.Count(); i++)
							{
								ASSignNodesSettings.Rows.Remove(row[i]);
								ASSignNodesSettings.AcceptChanges();
							}
						}
						ASSignNodesSettings.AcceptChanges();
					}
					Session["AssignNodeServer"] = ASSignNodesSettings;
					AssignNodesGridView.DataSource = ASSignNodesSettings;
					AssignNodesGridView.DataBind();
				}
			}
			catch (Exception ex)
			{
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}
			finally { }
		}

		private void FillAssignNodesgridfromSession()
		{
			try
			{

				DataTable ASSignNodesSettings = new DataTable();
				if (Session["AssignNodeServer"] != null && Session["AssignNodeServer"] != "")
					ASSignNodesSettings = (DataTable)Session["AssignNodeServer"];//VSWebBL.ConfiguratorBL.WindowsPropertiesBL.Ins.GetAllData();
				if (ASSignNodesSettings.Rows.Count > 0)
				{
					AssignNodesGridView.DataSource = ASSignNodesSettings;
					AssignNodesGridView.DataBind();
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
		private void FillServicesgridfromSession()
		{
			try
			{
						DataTable Services = new DataTable();
						if (Session["Servicesgrid"] != null && Session["Servicesgrid"] != "")
							Services = (DataTable)Session["Servicesgrid"];
						if (Services.Rows.Count > 0)
						{
							servicesGrid.DataSource = Services;
							servicesGrid.DataBind();
						}
					
				
			}
			catch (Exception ex)
			{

				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}
			finally { }
		}
		protected void NodesGridView_HtmlRowCreated(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
		{
            //3/4/2016 NS modified for VSPLUS-2687
			//e.Row.Attributes.Add("onmouseover", "this.originalcolor=this.style.backgroundColor;" + "this.style.backgroundColor='#C0C0C0';");
			//e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalcolor;");

			if (e.RowType == GridViewRowType.EditForm)
			{

				try
				{
					if (e.GetValue("ID") != " " && e.GetValue("ID") != null)
					{
						ASPxWebControl.RedirectOnCallback("~/Configurator/NodeSettingsProperties.aspx?ID=" + e.GetValue("ID"));
						Context.ApplicationInstance.CompleteRequest();//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
					}
					else
					{
						ASPxWebControl.RedirectOnCallback("~/Configurator/NodeSettingsProperties.aspx");
						Context.ApplicationInstance.CompleteRequest();
					}
				}
				catch (Exception ex)
				{
					Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
					//throw ex;
				}

				//}
			}
		}
		/*
		protected void NodesGridView_SelectionChanged(object sender, EventArgs e)
		{
			if (NodesGridView.Selection.Count > 0)
			{
				System.Collections.Generic.List<object> Type = NodesGridView.GetSelectedFieldValues("ID");
				System.Collections.Generic.List<object> Name = NodesGridView.GetSelectedFieldValues("Name");
				System.Collections.Generic.List<object> HostName = NodesGridView.GetSelectedFieldValues("HostName");
				System.Collections.Generic.List<object> Alive = NodesGridView.GetSelectedFieldValues("Alive");
				System.Collections.Generic.List<object> Version = NodesGridView.GetSelectedFieldValues("Version");
				System.Collections.Generic.List<object> CredentialsID = NodesGridView.GetSelectedFieldValues("CredentialsID");
				System.Collections.Generic.List<object> NodeType = NodesGridView.GetSelectedFieldValues("NodeType");
				System.Collections.Generic.List<object> LoadFactor = NodesGridView.GetSelectedFieldValues("LoadFactor");

				System.Collections.Generic.List<object> IsPrimaryNode = NodesGridView.GetSelectedFieldValues("IsPrimaryNode");
				if (Type.Count > 0)
				{
					string ID = Type[0].ToString();
					string name = Name[0].ToString();
					string category = HostName[0].ToString();
					string Loc = Alive[0].ToString();
					string ipaddr = Version[0].ToString();
					string crd = CredentialsID[0].ToString();
					string ntype = NodeType[0].ToString();
					string lfact = LoadFactor[0].ToString();

					string primnode = IsPrimaryNode[0].ToString();
					try
					{
						DevExpress.Web.ASPxWebControl.RedirectOnCallback("NodeSettingsProperties.aspx?ID=" + ID);
						Context.ApplicationInstance.CompleteRequest();

					}
					catch (Exception ex)
					{
						Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
					}
				}

			}

		}
		*/
		protected void NodesGridView_PageSizeChanged(object sender, EventArgs e)
		{
			VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("NodesGrid|NodesGrid", NodesGrid.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
			Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
		}
		protected void AssignNodesGridView_OnPageSizeChanged(object sender, EventArgs e)
		{
			VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("AssignNodesGridView|AssignNodesGridView", AssignNodesGridView.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
			Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
		}
		protected void NewButton_Click(object sender, EventArgs e)
		{

			Response.Redirect("~/Configurator/NodeSettingsProperties.aspx", false);
			Context.ApplicationInstance.CompleteRequest();
		}

		private void FillAssignNodes()
		{
			DataTable NodesDataTable = new DataTable();
			NodesDataTable = VSWebBL.SecurityBL.NodesBL.Ins.GetAllData();
			DataRow row = NodesDataTable.NewRow();
			row["ID"] = "-1";
			row["Name"] = "";
			NodesDataTable.Rows.InsertAt(row,0);
			if (NodesDataTable.Rows.Count > 0)
			{
				ServerNodes.DataSource = NodesDataTable;
				ServerNodes.TextField = "Name";
				ServerNodes.ValueField = "ID";
				ServerNodes.DataBind();

				ServerNodes.Items[0].Selected = true;
			}
		}


		protected void NodesApply_Click(object sender, EventArgs e)
		{
			UpdateServersData();
			VSWebBL.SecurityBL.NodesBL.Ins.forceCollectionRefresh();
			Response.Redirect("~/Security/AssignServerToNode.aspx", false);
		}
		private void UpdateServersData()
		{
			try
			{

				//List<object> fieldValues = new List<object>();
				List<object> fieldValues = AssignNodesGridView.GetSelectedFieldValues(new string[] { "ID" });
				DataTable dt = GetSelectedServices();
				List<DataRow> servicesSelected = dt.AsEnumerable().ToList();

				if (servicesSelected.Count > 0)
				{
					foreach (DataRow row in servicesSelected)
					{
						if (row["id"] != null)
							fieldValues.Add(row["id"]);
					}
				}
				Object result2 = VSWebBL.SecurityBL.NodesBL.Ins.Updatenodes(Convert.ToInt32(ServerNodes.SelectedItem.Value), fieldValues);
				Object ReturnValue = Session["AssignNodeServer"];

				if (ReturnValue.ToString() == "True")
				{

					Response.Redirect("~/Security/AssignServerToNode.aspx", false);
					Context.ApplicationInstance.CompleteRequest();
				}

			}
			catch (Exception ex)
			{
				//10/13/2014 NS modified for VSPLUS-990
				//errorDiv.InnerHtml = "The following error has occurred: " + ex.Message +
				//	"<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
				//errorDiv.Style.Value = "display: block";
				//6/27/2014 NS added for VSPLUS-634
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
			}
		}
		private DataTable GetSelectedServices()
		{


			DataTable dtSel = new DataTable();
			try
			{
				dtSel.Columns.Add("id");

				DataTable dt = (DataTable)Session["AssignNodeServer"];
				if (dt != null)
				{
					foreach (DataRow row in dt.Rows)
					{
						if (row["ID"].ToString().ToLower() == "true")
						{
							DataRow dr = dtSel.NewRow();
							dr["id"] = row["ID"];
							dtSel.Rows.Add(dr);
						}
					}
				}
			}
			catch (Exception ex)
			{
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}

			return dtSel;

		}

		protected void NodesGridView_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
		{
			Nodes NodesObject = new Nodes();
			NodesObject.ID = Convert.ToInt32(e.Keys[0]);
			Object ReturnValue = VSWebBL.SecurityBL.NodesBL.Ins.DeleteData(NodesObject);
			ASPxGridView gridView = (ASPxGridView)sender;
			gridView.CancelEdit();
			e.Cancel = true;
            FillNodesGrid();
			//FillNodesSettingsGrid();

		}

		//protected void btn_Clickdelete(object sender, EventArgs e)
		//{
		//    ImageButton bttnDel = (ImageButton)sender;
		//    Nodes AssignnodesObject = new Nodes();
		//    AssignnodesObject.ID = Convert.ToInt32(bttnDel.CommandArgument);
		//    //ID = Convert.ToInt32(bttnDel.CommandArgument);
		//    string NodeName = bttnDel.AlternateText;
		//    pnlAreaDtls.Style.Add("visibility", "visible");
		//    //divmsg.InnerHtml = "Are you sure you want to delete the server " + NodeName + "?";

		//}
		
			
			//protected void GridView_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
			//{
			//    e.Properties["cpVisibleRowCount"] = AssignNodesGridView.VisibleRowCount;
			//}
			//protected void lnkSelectAllRows_Load(object sender, EventArgs e)
			//{
			//    ((ASPxHyperLink)sender).Visible = ComandColumn.SelectAllCheckboxMode != GridViewSelectAllCheckBoxMode.AllPages;
			//}
			//protected void lnkClearSelection_Load(object sender, EventArgs e)
			//{
			//    ((ASPxHyperLink)sender).Visible = ComandColumn.SelectAllCheckboxMode != GridViewSelectAllCheckBoxMode.AllPages;
			//}






		private void FillNodesGrid()
		{
			try
			{
				int row = NodesGrid.FocusedRowIndex;

				DataTable NodesGridData = VSWebBL.SecurityBL.NodesBL.Ins.GetAllNodeServicesDetails();

				NodesGrid.DataSource = NodesGridData;
				NodesGrid.DataBind();

				if (row < 0 && NodesGridData.Rows.Count > 0)
					row = 0;
				NodesGrid.FocusedRowIndex = row;

			}
			catch (Exception ex)
			{
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}
			finally { }
		}

		private void HideColumnsForFeatures()
		{
			DataTable ConfigDt = VSWebBL.SecurityBL.MenusBL.Ins.GetSelectedFeatures();
			string features = ConfigDt.AsEnumerable().Select(row => row["Name"].ToString()).Aggregate((s1, s2) => s1 + "," + s2);

			if (!(features.Contains("Exchange") || features.Contains("SharePoint") || features.Contains("Skype for Business") || features.Contains("Windows") || features.Contains("Office 365")))
				NodesGrid.Columns["Microsoft"].Visible = false;
			if (!(features.Contains("Domino")))
				NodesGrid.Columns["Domino"].Visible = false;

		}

		protected void NodesGrid_OnDataBound(object sender, EventArgs e)
		{
			ASPxGridView grid = (ASPxGridView)sender;

			foreach (GridViewColumn col in grid.Columns)
			{
				if (col is GridViewDataColumn)
					col.Caption = ((GridViewDataColumn)col).FieldName;
			}
		}

		protected void NodesGrid_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
		{

			if (e.DataColumn.FieldName == "Alive")
			{
				if (e.GetValue("Alive").ToString() == "Yes")
				{
					e.Cell.BackColor = System.Drawing.Color.LightGreen;
				}
				else if (e.GetValue("isDisabled").ToString() == "Yes")
				{
					e.Cell.BackColor = System.Drawing.Color.Gray;
					e.Cell.Text = "DISABLED";
				}
				else
				{
					e.Cell.BackColor = System.Drawing.Color.Red;
					e.Cell.ForeColor = System.Drawing.Color.White;
				}
			}

			if (e.DataColumn.FieldName == "isDisabled")
			{
				if (e.GetValue("isDisabled").ToString() == "No")
				{
					e.Cell.BackColor = System.Drawing.Color.LightGreen;
				}
				else
				{
					e.Cell.BackColor = System.Drawing.Color.Red;
					e.Cell.ForeColor = System.Drawing.Color.White;
				}
			}
			
		}



		protected void servicesGrid_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
		{
			string[] notAlwaysRunning = new string[] {
				"Daily Service",
				"DB Health"
			};

			string[] OnlyPrimary = new String[] {
				"DB Health",
				"Alerting"
			};
			string s = e.GetValue("IsPrimaryNode").ToString();
			if (e.DataColumn.FieldName != "")
			{
				if (OnlyPrimary.Contains(e.DataColumn.FieldName) && e.GetValue("IsPrimaryNode").ToString() == "False")
				{
                    //3/4/2016 NS modified for VSPLUS-2687
					e.Cell.BackColor = System.Drawing.Color.LightGray;
					e.Cell.Text = "N/A";
				}
				else
				{
					if (e.GetValue(e.DataColumn.FieldName) == null || e.GetValue(e.DataColumn.FieldName).ToString() == "" || e.GetValue(e.DataColumn.FieldName).ToString() == "0")
						e.Cell.Text = "Not Found";
					if (notAlwaysRunning.Contains(e.DataColumn.FieldName) && e.Cell.Text != "Not Found" && e.GetValue("Alive").ToString() == "Yes")
					{
                        //3/4/2016 NS modified for VSPLUS-2687
                        e.Cell.BackColor = System.Drawing.Color.LightGray;
						return;
					}
					if (e.GetValue("Alive").ToString() == "No")
					{
                        //3/4/2016 NS modified for VSPLUS-2687
                        e.Cell.BackColor = System.Drawing.Color.LightGray;
						e.Cell.Text = "N/A";
					}
					else if (e.GetValue(e.DataColumn.FieldName).ToString() == "Running")
					{
						e.Cell.BackColor = System.Drawing.Color.LightGreen;
					}
					else
					{
						e.Cell.BackColor = System.Drawing.Color.Red;
						e.Cell.ForeColor = System.Drawing.Color.White;
					}

				}

			}
		}

		public string SetIcon(GridViewDataItemTemplateContainer Container)
		{
			System.Web.UI.WebControls.Image img = (System.Web.UI.WebControls.Image)Container.FindControl("IconImage");
			Label lbl = (Label)Container.FindControl("lblIcon");
			string lblOS = lbl.Text;

			if (lbl.Text == "True")
			{
				img.ImageUrl = "~/images/icons/Monitoring.png";
			}
			else
			{
				img.Visible = false;
			}

			return "";
		}


		protected void NodesGrid_SelectionChanged(object sender, EventArgs e)
		{
			if (NodesGrid.Selection.Count > 0)
			{
				int index = NodesGrid.FocusedRowIndex;

				if (index >= 0)
				{
					string NodeName = NodesGrid.GetRowValues(index, "Name").ToString();
					Div2.InnerHtml = NodeName + " - Services Status";

					DataTable dt = VSWebBL.SecurityBL.NodesBL.Ins.GetNodeServices(NodeName);
				
					servicesGrid.DataSource = dt;
					servicesGrid.DataBind();
				}
			}
		}


		protected void DetailsGrid_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
		{
			if (NodesGrid.FocusedRowIndex == null || NodesGrid.FocusedRowIndex == -1)
				return;

			string NodeName = NodesGrid.GetRowValues(NodesGrid.FocusedRowIndex, "Name").ToString();

			Div2.InnerHtml = NodeName + " - Services Status";
			DataTable dt = VSWebBL.SecurityBL.NodesBL.Ins.GetNodeServices(NodeName);
			Session["Servicesgrid"] = dt;
			servicesGrid.DataSource = dt;
			servicesGrid.DataBind();
		}
		protected void NodesGrid_PageSizeChanged(object sender, EventArgs e)
		{
			//ProfilesGridView.PageIndex;
			VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("AssignServerToNode|NodesGrid", NodesGrid.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
			//Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
			UI.Ins.ChangeUserPreference("AssignServerToNode|NodesGrid", NodesGrid.SettingsPager.PageSize);//Sowjanya 1528
		}
		protected void servicesGrid_PageSizeChanged(object sender, EventArgs e)
		{
			//ProfilesGridView.PageIndex;
			VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("AssignServerToNode|servicesGrid", servicesGrid.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
			//Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
			UI.Ins.ChangeUserPreference("AssignServerToNode|servicesGrid", servicesGrid.SettingsPager.PageSize);//Sowjanya 1528
		}





		}
	}
