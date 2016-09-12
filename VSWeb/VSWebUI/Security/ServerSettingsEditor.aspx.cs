using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

using DevExpress.Web;
using DevExpress.Web.ASPxTreeList;

namespace VSWebUI.Security
{
	public partial class ServerSettingsEditor : System.Web.UI.Page
	{
		string ServerTypeFilter = "";
		private void Master_ButtonClick(object sender, EventArgs e)
		{

		}
		object msgloc = "";
		protected DataTable ProfilesDataTable = null;
		protected DataTable DSTasksDataTable = null;
		protected DataTable ExchangeDataTable = null;
		protected DataTable DisksDataTable = null;
		int profilevalue;
		int servertypeid;
		int locationid ;
		int locationddlid;
   
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack && !IsCallback)
			{
				if (ProfileComboBox.SelectedIndex != -1)
				{
					ExchangeRolesLabel.Visible = false;
					ExchangeRolesComboBox.Visible = false;
				}
				//DiskSettingsClear.Visible = false;      
				ASPxPageControl1.ActiveTabIndex = 0;
				Session["DataServers"] = null;
				Session["Profiles"] = null;
				Session["DominoServers"] = null;
				Session["DominoTasks"] = null;
				Session["ExchangeServers"] = null;
				Session["WindowsServices"] = null;
				Session["DiskSettingsDisks"] = null;
				Session["DiskSettingsServers"] = null;
				Session["ServerLocations"] = null;
				Session["ServerCredentials"] = null;
				Session["BusinessHours"] = null;
				//ExchangeRolesComboBox.Visible = false;
				//ExchangeRolesLabel.Visible = false;
				
				tblServer.Style.Add("display", "none");
				ApplyServersButton.Style.Add("display", "none");
				Session["Mode"] = "Insert";
				Session["ProfileId"] = "0";
				//ProfileTextBox.Enabled = true;
				FillprofileComboBox();
				//if (ProfileComboBox.SelectedItem.Text != "")//commented by M.Somaraj
				//{
				//     profilevalue = Convert.ToInt32(ProfileComboBox.SelectedItem.Value);//commented by MS
				//    FillServerTypeComboBox(profilevalue);//commented by MS
				//}

				//FillExchangeRolesComboBox();//commented by MS
				//FillProfilesGrid(ServerTypeComboBox.SelectedItem.Text, "", ProfileComboBox.SelectedItem.Text);
				fillDominoServersTreeList();
				FillDominoServerTasksGrid();
				//FillExchangeServerServicesGrid();
				FillDiskSettingsTreeList();
				FillDiskSettingsGridView();
				FillServerLocations();
				FillServerLocationsTreeList();
				//FillExchangeRolesForExchangeTab();
				FillServerCredentialsTreeList();
				FillServerCredentials();
				FillBusinesshoursComboBox();
				FillBusinessHoursTreeList();
				SelCriteriaRadioButtonList.SelectedIndex = 0;
				DiskLabel.Text = "Current threshold: " + AdvDiskSpaceThTrackBar.Value + "% free space";
				if (SelCriteriaRadioButtonList.SelectedItem != null && SelCriteriaRadioButtonList.SelectedItem.Value.ToString() != "0")
				{
					AdvDiskSpaceThTrackBar.Visible = false;
					DiskLabel.Visible = false;
				}

				if (Session["UserPreferences"] != null)
				{
					DataTable UserPreferences = (DataTable)Session["UserPreferences"];
					foreach (DataRow dr in UserPreferences.Rows)
					{
						if (dr[1].ToString() == "ServerSettingsEditor|ProfilesGridView")
						{
							ProfilesGridView.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
						}
						if (dr[1].ToString() == "ServerSettingsEditor|DominoTasksGridView")
						{
							DominoTasksGridView.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
						}

						if (dr[1].ToString() == "ServerSettingsEditor|WindowsServices")
						{
							WindowsServices.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
						}
					}
				}
			}
			else
			{
				if (SelCriteriaRadioButtonList.SelectedItem != null && SelCriteriaRadioButtonList.SelectedItem.Value.ToString() != "0")
				{
					AdvDiskSpaceThTrackBar.Visible = false;
				}
				LocationSuccess.Style.Value = "display: none;";
				LocationSuccess.InnerHtml = "";
				LocationError.Style.Value = "display: none;";
				LocationError.InnerHtml = "";
				fillDominoServersTreeListfromSession();
				FillDominoServerTasksGridfromSession();
				FillProfilesGridfromSession();
				fillServersTreefromSession();
				FillExchangeServerServicesGridfromSession();
				fillExchangeServersTreeListfromSession();
				FillDiskSettingsTreeListfromSession();
				FillDiskSettingsGridViewfromSession();
				FillServerLocationsTreeListfromSession();
				FillServerCredentialsTreeListfromSession();
				FillBusinessHoursTreeListfromSession();
				//ServersGridView.DataBind();
			}
		}
		private void FillServerTypeComboBox(int profilevalue)
		{
			
            //2/29/2016 Durga Modified for VSPLUS-2668
            string pageName = "ServerSettingsEditor.aspx", controlName = "ServerTypeComboBox";
            DataTable ServerDataTable = VSWebBL.SecurityBL.ServerTypesBL.Ins.GetSpecifiServertypesforSSE(profilevalue, pageName, controlName);
			ServerTypeComboBox.DataSource = ServerDataTable;
			ServerTypeComboBox.TextField = "ServerType";
			ServerTypeComboBox.ValueField = "ServerTypeId";
			ServerTypeComboBox.DataBind();
		}
		private void FillprofileComboBox()
		{
			DataTable ProfileNamesDataTable = VSWebBL.SecurityBL.ProfilesNamesBL.Ins.GetAllData();
			ProfileComboBox.DataSource = ProfileNamesDataTable;
			ProfileComboBox.TextField = "ProfileName";
			ProfileComboBox.ValueField = "ID";
			ProfileComboBox.DataBind();
		}
		private void FillExchangeRolesComboBox(int servertypeid)
		{
			//DataTable ServerDataTable = VSWebBL.SecurityBL.ServerTypesBL.Ins.GetExchangeRoles();//Commented by MS
			DataTable ServerDataTable = VSWebBL.SecurityBL.ServerTypesBL.Ins.GetExchangeRoleswithprofile(servertypeid);////changed by MS

			DataRow General = ServerDataTable.NewRow();
			General["Id"] = 0;
			General["RoleName"] = "General";
			General["ServerTypeId"] = 5;
			ServerDataTable.Rows.InsertAt(General, 0);
			ExchangeRolesComboBox.DataSource = ServerDataTable;
			ExchangeRolesComboBox.TextField = "RoleName";
			ExchangeRolesComboBox.ValueField = "RoleName";
			ExchangeRolesComboBox.DataBind();
		}
		public void fillDominoServersTreeList()
		{
			try
			{
				Session["DominoServers"] = null;
				if (Session["DominoServers"] == null)
				{
					DataTable DataServersTree = new DataTable();
					DataServersTree = VSWebBL.ConfiguratorBL.AlertsBL.Ins.GetServersFromProcedure();
					Session["IntialAllservers"] = DataServersTree;
					if (DataServersTree.Rows.Count > 0)
					{
						if (Session["RestrictedServers"] != "" && Session["RestrictedServers"] != null)
						{
							List<int> ServerID = new List<int>();
							List<int> LocationID = new List<int>();
							DataTable resServers = (DataTable)Session["RestrictedServers"];
							foreach (DataRow resser in resServers.Rows)
							{
								foreach (DataRow dominorow in DataServersTree.Rows)
								{
									if (resser["serverid"].ToString() == dominorow["actid"].ToString())
									{
										ServerID.Add(DataServersTree.Rows.IndexOf(dominorow));
									}
									if (dominorow["locid"] != null && dominorow["locid"].ToString() != "" && (resser["locationID"].ToString() == dominorow["locid"].ToString()))
									{
										LocationID.Add(Convert.ToInt32(dominorow["locid"].ToString()));
									}
								}
							}
							foreach (int Id in ServerID)
							{
								DataServersTree.Rows[Id].Delete();
							}
							DataServersTree.AcceptChanges();

							foreach (int lid in LocationID)
							{
								DataRow[] row = DataServersTree.Select("locid=" + lid + "");
								for (int i = 0; i < row.Count(); i++)
								{
									DataServersTree.Rows.Remove(row[i]);
									DataServersTree.AcceptChanges();
								}
							}
							DataServersTree.AcceptChanges();
						}

						DataTable filteredData = DataServersTree.Select("ServerType= 'Domino' or ServerType is null").CopyToDataTable();
						Session["DominoServers"] = filteredData;

					}
				}
				DominoServerTreeList.DataSource = (DataTable)Session["DominoServers"];
				DominoServerTreeList.DataBind();
				DominoServerTreeList.ExpandAll();
			}

			catch (Exception ex)
			{
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}
			finally { }
		}
		private void FillDominoServerTasksGrid()
		{
			try
			{
				//DSTaskSettingsDataSet = new DataSet();
				DSTasksDataTable = new DataTable();
				DSTasksDataTable = VSWebBL.ConfiguratorBL.DominoServerTasksBL.Ins.GetAllData();
				//DataTable dtcopy = DSTasksDataTable.Copy();
				//dtcopy.Columns.Add("SendLoadCommand");
				//dtcopy.Columns.Add("SendRestartCommand");
				//dtcopy.Columns.Add("RestartOffHours");
				//dtcopy.Columns.Add("SendExitCommand"); 
				//DSTaskSettingsDataSet.Tables.Add(dtcopy);
				if (DSTasksDataTable.Rows.Count > 0)
				{
					Session["DominoTasks"] = DSTasksDataTable;// DSTaskSettingsDataSet.Tables[0];
					DominoTasksGridView.DataSource = DSTasksDataTable;//DSTaskSettingsDataSet.Tables[0];
					DominoTasksGridView.DataBind();
				}
			}
			catch (Exception ex)
			{
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}
			finally { }
		}
		public void fillDominoServersTreeListfromSession()
		{
			DataTable DataServers = new DataTable();
			try
			{
				if (Session["DominoServers"] != "" && Session["DominoServers"] != null)
					DataServers = Session["DominoServers"] as DataTable;
				if (DataServers.Rows.Count > 0)
				{
					DataServers.PrimaryKey = new DataColumn[] { DataServers.Columns["ID"] };
				}
				DominoServerTreeList.DataSource = DataServers;
				DominoServerTreeList.DataBind();
			}
			catch (Exception ex)
			{
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}
		}
		private void FillDominoServerTasksGridfromSession()
		{
			try
			{
				DSTasksDataTable = new DataTable();
				if (Session["DominoTasks"] != "" && Session["DominoTasks"] != null)
				{
					DSTasksDataTable = (DataTable)Session["DominoTasks"];
				}
				if (DSTasksDataTable.Rows.Count > 0)
				{
					int startIndex = DominoTasksGridView.PageIndex * DominoTasksGridView.SettingsPager.PageSize;
					int endIndex = Math.Min(DominoTasksGridView.VisibleRowCount, startIndex + DominoTasksGridView.SettingsPager.PageSize);
					for (int i = startIndex; i < endIndex; i++)
					{
						if (DominoTasksGridView.Selection.IsRowSelected(i))
						{
							//object o = DominoTasksGridView.GetRowValues(i, "SendLoadCommand");
							DSTasksDataTable.Rows[i]["isSelected"] = "true";
							ASPxCheckBox SendLoadCommand = DominoTasksGridView.FindRowCellTemplateControl(i, DominoTasksGridView.Columns[4] as GridViewDataColumn, "SendLoadCommand") as ASPxCheckBox;
							ASPxCheckBox SendRestartCommand = DominoTasksGridView.FindRowCellTemplateControl(i, DominoTasksGridView.Columns[5] as GridViewDataColumn, "SendRestartCommand") as ASPxCheckBox;
							ASPxCheckBox RestartOffHours = DominoTasksGridView.FindRowCellTemplateControl(i, DominoTasksGridView.Columns[6] as GridViewDataColumn, "RestartOffHours") as ASPxCheckBox;
							ASPxCheckBox SendExitCommand = DominoTasksGridView.FindRowCellTemplateControl(i, DominoTasksGridView.Columns[7] as GridViewDataColumn, "SendExitCommand") as ASPxCheckBox;
							if (SendLoadCommand != null && SendLoadCommand.Checked == true)
							{
								DSTasksDataTable.Rows[i]["SendLoadCommand"] = "true";
							}
							else
							{
								DSTasksDataTable.Rows[i]["SendLoadCommand"] = "false";
							}
							if (SendRestartCommand != null && SendRestartCommand.Checked == true)
							{
								DSTasksDataTable.Rows[i]["SendRestartCommand"] = "true";
							}
							else
							{
								DSTasksDataTable.Rows[i]["SendRestartCommand"] = "false";
							}
							if (RestartOffHours != null && RestartOffHours.Checked == true)
							{
								DSTasksDataTable.Rows[i]["RestartOffHours"] = "true";
							}
							else
							{
								DSTasksDataTable.Rows[i]["RestartOffHours"] = "false";
							}
							if (SendExitCommand != null && SendExitCommand.Checked == true)
							{
								DSTasksDataTable.Rows[i]["SendExitCommand"] = "true";
							}
							else
							{
								DSTasksDataTable.Rows[i]["SendExitCommand"] = "false";
							}
						}
						else
						{
							DSTasksDataTable.Rows[i]["isSelected"] = "false";
						}
					}
					DominoTasksGridView.DataSource = DSTasksDataTable;
					DominoTasksGridView.DataBind();
				}
			}
			catch (Exception ex)
			{
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}
			finally { }
		}
		private void FillProfilesGridfromSession()
		{
			try
			{
				// ProfilesDataTable = new DataTable();
				if (Session["Profiles"] != null && Session["Profiles"] != "")
				{
					ProfilesDataTable = (DataTable)Session["Profiles"];

					if (ProfilesDataTable.Rows.Count > 0)
					{
						GridViewDataColumn column2 = ProfilesGridView.Columns["DefaultValue"] as GridViewDataColumn;
						DataTable dt = new DataTable();
						int startIndex = ProfilesGridView.PageIndex * ProfilesGridView.SettingsPager.PageSize;
						int endIndex = Math.Min(ProfilesGridView.VisibleRowCount, startIndex + ProfilesGridView.SettingsPager.PageSize);
						for (int i = startIndex; i < endIndex; i++)
						{
							if (ProfilesGridView.Selection.IsRowSelected(i))
							{
								ASPxTextBox txtValue = (ASPxTextBox)ProfilesGridView.FindRowCellTemplateControl(i, column2, "txtDefaultValue");
								ProfilesDataTable.Rows[i]["DefaultValue"] = txtValue.Text;
								ProfilesDataTable.Rows[i]["isSelected"] = "true";
							}
							else
							{
								ProfilesDataTable.Rows[i]["isSelected"] = "false";
							}
						}
						ProfilesGridView.DataSource = ProfilesDataTable;
						ProfilesGridView.DataBind();
					}
				}
			}
			catch (Exception ex)
			{
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}
			finally { }
		}
		public void fillServersTreefromSession()
		{
			DataTable DataServers = new DataTable();
			try
			{
				if ((ServerTypeComboBox.Text == "Exchange") && (ExchangeRolesComboBox.Text == "" || ExchangeRolesComboBox.Text == null))
				{
					ServersTreeList.DataSource = null;
					ServersTreeList.DataBind();
				}
				else
				{
					if (Session["DataServers"] != "" && Session["DataServers"] != null)
						DataServers = Session["DataServers"] as DataTable;
					if (DataServers.Rows.Count > 0)
					{
						DataServers.PrimaryKey = new DataColumn[] { DataServers.Columns["ID"] };
					}
					ServersTreeList.DataSource = DataServers;
					ServersTreeList.DataBind();
				}
			}
			catch (Exception ex)
			{
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}
		}
		private void FillProfilesGrid(string ServerType, string RoleType, string ProfileName)
		{
			try
			{
				// ProfilesDataTable = new DataTable();
				//DataSet ProfilesDataSet = new DataSet();
                //2/23/2016 NS modified
                if (ServerType == "Office 365")
                {
                    ServerType = "Office365";
                }
				ProfilesDataTable = VSWebBL.SecurityBL.ProfilesMasterBL.Ins.GetAllDataByServerType(ServerType, RoleType, ProfileName);
				if (ProfilesDataTable.Rows.Count > 0)
				{
					ProfilesDataTable.PrimaryKey = new DataColumn[] { ProfilesDataTable.Columns["ID"] };

					if (ServerType == "Windows")
					{
						ProfilesDataTable = (from n in ProfilesDataTable.AsEnumerable()
											 where !n.Field<string>("RelatedField").Contains("LatencyRedThreshold") && !n.Field<string>("RelatedField").Contains("LatencyYellowThreshold")
											 select n).CopyToDataTable();
						//ProfilesDataTable = (from DataRow dr in ProfilesDataTable.Rows //Ms-Raj
						//                     where dr["RelatedField"].ToString() != "LatencyRedThreshold" && dr["RelatedField"].ToString() != "LatencyYellowThreshold"
						//                     select dr).CopyToDataTable();
					}
				}
				ProfilesGridView.PageIndex = 0;//MS-Raj-VSPLUS:2263
				Session["Profiles"] = ProfilesDataTable;
				ProfilesGridView.DataSource = ProfilesDataTable;
				ProfilesGridView.DataBind();
			}
			catch (Exception ex)
			{
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}
			finally { }
		}
		public void fillServersTreeList()
		{

			try
			{
				this.ServersTreeList.RefreshVirtualTree();
				this.ServersTreeList.UnselectAll();
				ServersTreeList.DataBind();
				errorDiv7.Style.Value = "display: none";
				Session["DataServers"] = null;
				if (Session["DataServers"] == null)
				{
					DataTable DataServersTree = new DataTable();
					if (ServerTypeComboBox.Text == "Exchange")
					{
						DataServersTree = VSWebBL.ConfiguratorBL.AlertsBL.Ins.GetExchangeServersFromProcedure();
					}
					else
					{
						DataServersTree = VSWebBL.ConfiguratorBL.AlertsBL.Ins.GetServersFromProcedure();
					}
					if (DataServersTree.Rows.Count > 0)
					{
						if (Session["RestrictedServers"] != "" && Session["RestrictedServers"] != null)
						{
							List<int> ServerID = new List<int>();
							List<int> LocationID = new List<int>();
							DataTable resServers = (DataTable)Session["RestrictedServers"];
							foreach (DataRow resser in resServers.Rows)
							{
								foreach (DataRow dominorow in DataServersTree.Rows)
								{

									if (resser["serverid"].ToString() == dominorow["actid"].ToString())
									{
										ServerID.Add(DataServersTree.Rows.IndexOf(dominorow));
									}
									if (dominorow["locid"] != null && dominorow["locid"].ToString() != "" && (resser["locationID"].ToString() == dominorow["locid"].ToString()))
									{
										LocationID.Add(Convert.ToInt32(dominorow["locid"].ToString()));
									}
								}

							}
							foreach (int Id in ServerID)
							{
								DataServersTree.Rows[Id].Delete();
							}
							DataServersTree.AcceptChanges();

							foreach (int lid in LocationID)
							{
								DataRow[] row = DataServersTree.Select("locid=" + lid + "");
								for (int i = 0; i < row.Count(); i++)
								{
									DataServersTree.Rows.Remove(row[i]);
									DataServersTree.AcceptChanges();
								}
							}
							DataServersTree.AcceptChanges();
						}
						DataTable filteredData = null;
						if (ServerTypeComboBox.Text == "Exchange")
						{
							string query = "(ServerType='" + ServerTypeComboBox.Text + "' or ServerType is null)";
							if (ExchangeRolesComboBox.Text.ToLower() == "hub")
							{
								query += " and (RoleType like '%Hub%' or RoleType is null)";
							}
							else if (ExchangeRolesComboBox.Text.ToLower() == "edge")
							{
								query += " and (RoleType like '%Edge%' or RoleType is null)";
							}
							else if (ExchangeRolesComboBox.Text.ToLower() == "mailbox")
							{
								query += " and (RoleType like '%Mailbox%' or RoleType is null)";
							}
							else if (ExchangeRolesComboBox.Text.ToLower() == "cas")
							{
								query += " and (RoleType like '%CAS%' or RoleType is null)";
							}
							if (DataServersTree.Select(query).Length != 0)
							{
								filteredData = DataServersTree.Select(query).CopyToDataTable();
							}
						}
						else
						{
							if (DataServersTree.Rows.Count > 0)
							{
								if (ServerTypeComboBox.Text == "General")
								{
									filteredData = DataServersTree;
								}
								else
								{
                                    //2/23/2016 NS modified
                                    string ServerType = ServerTypeComboBox.Text;
                                    if (ServerTypeComboBox.Text == "Office 365")
                                    {
                                        ServerType = "Office365";
                                    }
                                    filteredData = DataServersTree.Select("ServerType='" + ServerType + "' or ServerType is null").CopyToDataTable();
								}

							}
						}
						//filteredData = DataServersTree.Select("ServerType='" + ServerTypeComboBox.Text + "' or ServerType is null").CopyToDataTable();
						List<string> lstResult = (from table in filteredData.AsEnumerable()
												  where table.Field<string>("ServerType") != null
												  select table.Field<string>("tbl")).ToList();
						//if (lstResult.Count > 0)//commented MS-Raj
						//{
						//    Session["DataServers"] = filteredData;
						//    ServersTreeList.DataSource = (DataTable)Session["DataServers"];
						//    ServersTreeList.DataBind();
						//}
						//else
						//{
						//    Session["DataServers"] = null;
						//    ServersTreeList.RefreshVirtualTree();
						//    ServersTreeList.ClearNodes();
						//    this.ServersTreeList.RefreshVirtualTree();
						//}
						//=================MS-Raj:VSPLUS:2254==================
						var servers = from n in filteredData.AsEnumerable()
									  where n.Field<string>("tbl").Contains("Servers")
									  select n;
						var Locations = from n in filteredData.AsEnumerable()
										where n.Field<string>("tbl").Contains("Locations")
										select n;
						DataTable filteredDT = null;
						DataTable Serversdt = null;
						DataTable Locationsdt = null;
						DataTable sum = new DataTable();
						if ((servers != null && servers.Count() > 0) && (Locations != null && Locations.Count() > 0))
						{
							Serversdt = servers.CopyToDataTable();
							Locationsdt = Locations.CopyToDataTable();
							sum = Locationsdt.Copy();
							sum.Merge(Serversdt);
							var customers = sum.AsEnumerable();
							var orders = Locationsdt.AsEnumerable();
							var LocationIdNotNull = from n in sum.AsEnumerable()
													where !n.IsNull("LocId")
													select n;
							filteredDT = LocationIdNotNull.CopyToDataTable();
							DataTable filterbylocation = (from n in Locationsdt.AsEnumerable()
														  join prod in filteredDT.AsEnumerable() on n.Field<int>("id") equals prod.Field<int>("LocId")
														  select n).Distinct().CopyToDataTable();
							DataTable Filteredservers = new DataTable();
							Filteredservers = filterbylocation.Copy();
							Filteredservers.Merge(filteredDT);
							Session["DataServers"] = Filteredservers;
							//Session["DataServers"] = sum2;//somaraj
							//ServersTreeList.DataSource = (DataTable)Session["DataServers"];
							//ServersTreeList.DataBind();
							//ServersTreeList.ExpandAll();
						}
						else
						{
							Session["DataServers"] = null;
							ServersTreeList.DataSource = "";
							ServersTreeList.DataBind();
							errorDiv7.InnerHtml = "The server(s) do not exist." +
					"<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
							
							errorDiv7.Style.Value = "display: block";
							ServersTreeList.ExpandAll();
							ServersTreeList.RefreshVirtualTree();
							ServersTreeList.ClearNodes();
							this.ServersTreeList.RefreshVirtualTree();
						}

					}
				}
				ServersTreeList.PageIndex = 0;//Ms-Raj--VSPLUS:2263
				ServersTreeList.DataSource = (DataTable)Session["DataServers"];
				ServersTreeList.DataBind();
				ServersTreeList.ExpandAll();
				if ((ServerTypeComboBox.Text == "Exchange") && (ExchangeRolesComboBox.Text == "" || ExchangeRolesComboBox.Text == null))
				{
					Session["DataServers"] = null;
				}
			}
			catch (Exception ex)
			{
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}
			finally { }
		}
		protected void ServerTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			errorDiv5.Style.Value = "display: none";
			errorDiv6.Style.Value = "display: none";
			errorDiv7.Style.Value = "display: none";
			if (ServerTypeComboBox.SelectedIndex != -1)
			{
				ExchangeRolesComboBox.SelectedIndex = -1;
				Session["DataServers"] = null;
				Session["Profiles"] = null;

				if (ServerTypeComboBox.SelectedItem.Text != "Exchange")
				{
		
					Session["DataServers"] = null;
					CollapseAllButton.Visible = true;
					ApplyServersButton.Visible = true;
					ExchangeRolesLabel.Visible = false;
					ExchangeRolesComboBox.Visible = false;
					ExchangeRolesComboBox.Text = "";
					ProfilesGridView.PageIndex = 0;//MS-Raj-VSPLUS:2263
					FillProfilesGrid(ServerTypeComboBox.SelectedItem.Text, "", ProfileComboBox.SelectedItem.Text);
					fillServersTreeList();
				}
				else
				{
					//CY: VS 463
					errorDiv7.Style.Value = "display: none";
					servertypeid = Convert.ToInt32(ServerTypeComboBox.SelectedItem.Value);//changed by MS
					FillExchangeRolesComboBox(servertypeid);//changed by MS
					ProfilesGridView.PageIndex = 0;//MS-Raj-VSPLUS:2263
					ProfilesGridView.DataSource = "";//changed by MS
					ProfilesGridView.DataBind();
					ServersTreeList.DataSource = "";
					ServersTreeList.DataBind();
					ServersTreeList.UnselectAll();
					ExchangeRolesLabel.Visible = true;
					ExchangeRolesComboBox.Visible = true;
					if (ExchangeRolesComboBox.SelectedIndex != -1)
					{

						string role = ExchangeRolesComboBox.SelectedItem.Text;
						if (ExchangeRolesComboBox.Text == "General")
						{
							role = "";
						}
						CollapseAllButton.Visible = true;
						ApplyServersButton.Visible = true;
						ProfilesGridView.PageIndex = 0;//MS-Raj-VSPLUS:2263
						FillProfilesGrid(ServerTypeComboBox.SelectedItem.Text, role, ProfileComboBox.SelectedItem.Text);
						fillServersTreeList();
					}
					else
					{
						//fillServersTreeList();
						ServersTreeList.DataSource = null;
						ServersTreeList.DataBind();
						ProfilesGridView.PageIndex = 0;//MS-Raj-VSPLUS:2263
						FillProfilesGrid("", "", "");
						ServersTreeList.UnselectAll();
					}

				}
				//12/12/2013 NS added
				errorDiv.Style.Value = "display: none;";
				errorDiv2.Style.Value = "display: none";

				successDiv.Style.Value = "display: none";
				emptyDiv3.Style.Value = "display: none";
				tblServer.Style.Value = "display: block";
			}

		}
		protected void ProfileComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			errorDiv7.Style.Value = "display: none";
			if (ProfileComboBox.SelectedIndex != -1)
			{
				ServerTypeComboBox.SelectedIndex = -1;
				Session["DataServers"] = null;
				Session["Profiles"] = null;
				ServerTypeComboBox.Enabled = true;
				//ExchangeRolesLabel.Visible = false;
				//ExchangeRolesComboBox.Visible = false;
				if (ProfileComboBox.SelectedItem.Text != "")
				{

					ApplyServersButton.Visible = true;
					//FillProfilesGrid("", "", ProfileComboBox.SelectedItem.Text);
					//fillServersTreeList();
					////canclbuttn.Visible = true;
					//FillProfilesGrid(ProfileComboBox.SelectedItem.Text, "");
					profilevalue = Convert.ToInt32(ProfileComboBox.SelectedItem.Value);//changed by MS
					FillServerTypeComboBox(profilevalue);//changed by MS
					ProfilesGridView.DataSource = "";//changed by MS
					ProfilesGridView.DataBind();
					ServersTreeList.DataSource = "";
					ServersTreeList.DataBind();
					ServersTreeList.UnselectAll();

				}
			}
			//12/12/2013 NS added
			errorDiv.Style.Value = "display: none;";
			//errorDiv2.Style.Value = "display: none";
			
			successDiv.Style.Value = "display: none";
			emptyDiv3.Style.Value = "display: none";
			// tblServer.Style.Value = "display: block";
		}

		protected void ExchangeRolesComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			ExchangeRolesLabel.Visible = true;
			ExchangeRolesComboBox.Visible = true;
			successDiv.Style.Value = "display: none";
			if (ServerTypeComboBox.Text == "Exchange")
			{
				string role = ExchangeRolesComboBox.SelectedItem.Text;
				if (ExchangeRolesComboBox.Text == "General")
				{
					role = "";
				}
				CollapseAllButton.Visible = true;
				ApplyServersButton.Visible = true;
				FillProfilesGrid(ServerTypeComboBox.SelectedItem.Text, role, ProfileComboBox.SelectedItem.Text);
				fillServersTreeList();
				ServersTreeList.UnselectAll();
			}
		}
		protected void CollapseAllButton_Click(object sender, EventArgs e)
		{
			try
			{
				if (CollapseAllButton.Text == "Collapse All")
				{
					ServersTreeList.CollapseAll();
					CollapseAllButton.Image.Url = "~/images/icons/add.png";
					CollapseAllButton.Text = "Expand All";
				}
				else
				{
					ServersTreeList.ExpandAll();
					CollapseAllButton.Image.Url = "~/images/icons/forbidden.png";
					CollapseAllButton.Text = "Collapse All";
				}
			}
			catch (Exception ex)
			{
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}
		}
		protected void ApplyServersButton_Click(object sender, EventArgs e)
		{
			//12/12/2013 NS added
			errorDiv.Style.Value = "display: none;";
			errorDiv2.Style.Value = "display: none";
			errorDiv5.Style.Value = "display: none";
			errorDiv6.Style.Value = "display: none";
			errorDiv7.Style.Value = "display: none";
			successDiv.Style.Value = "display: none";
			List<object> fieldValues = ProfilesGridView.GetSelectedFieldValues(new string[] { "RelatedField", "DefaultValue", "RelatedTable" });
			//List<object> serversSelected = ServersGridView.GetSelectedFieldValues(new string[] {"ID","ServerName" });
			DataTable dt = GetSelectedServers();
			List<DataRow> serversSelected = dt.AsEnumerable().ToList();
			//List<DataRow> list = dt.AsEnumerable().ToList();
			int Update = 0;
			string ServerErrors = "";
			if (fieldValues.Count > 0 && serversSelected.Count > 0)
			{
				GridViewDataColumn column2 = ProfilesGridView.Columns["DefaultValue"] as GridViewDataColumn;
				List<ProfilesMaster> list = new List<ProfilesMaster>();
				int startIndex = ProfilesGridView.PageIndex * ProfilesGridView.SettingsPager.PageSize;
				int endIndex = Math.Min(ProfilesGridView.VisibleRowCount, startIndex + ProfilesGridView.SettingsPager.PageSize);
				int ProfileMasterID = 0;
				for (int i = 0; i < ProfilesDataTable.Rows.Count; i++)
				{
					if (ProfilesDataTable.Rows[i]["isSelected"].ToString() == "True")
					{
						ProfileMasterID = Convert.ToInt32(ProfilesDataTable.Rows[i]["ID"]);
						string DefaultVal = ProfilesDataTable.Rows[i]["DefaultValue"].ToString();
						string roleType = "";
						if (ServerTypeComboBox.Text == "Exchange")
						{
							roleType = ExchangeRolesComboBox.Text;
							if (ProfilesDataTable.Rows[i]["AttributeName"].ToString() == "CPU Threshold" || ProfilesDataTable.Rows[i]["AttributeName"].ToString() == "Memory Threshold")
								if (Convert.ToDouble(DefaultVal) < 1)
									DefaultVal = (Convert.ToDouble(DefaultVal) * 100).ToString();
						}
						list.Add(new ProfilesMaster(0, 0, "", ProfilesDataTable.Rows[i]["AttributeName"].ToString(), ProfilesDataTable.Rows[i]["UnitOfMeasurement"].ToString(), DefaultVal, ProfilesDataTable.Rows[i]["RelatedTable"].ToString(), ProfilesDataTable.Rows[i]["RelatedField"].ToString(), roleType, 0));
						//list.Add(new UserProfileDetailed(i, UserProfileMasterID, ProfileMasterID, ProfilesDataTable.Rows[i]["DefaultValue"].ToString()));
					}
				}
				//foreach (object[] server in serversSelected)
				string AppliedServers = "";
				foreach (DataRow server in serversSelected)
				{
					Update = VSWebBL.SecurityBL.UserProfileMasterBL.Ins.UpdateServerSettings(Convert.ToInt32(server[0]), list);
					if (Update == 0)
					{
						//12/12/2013 NS modified
						if (ServerErrors == "")
						{
							ServerErrors += server[1].ToString();
						}
						else
						{
							ServerErrors += ", " + server[1].ToString();
						}
					}
					else
					{
						//12/12/2013 NS modified
						if (AppliedServers == "")
						{
							AppliedServers += server[1].ToString();
						}
						else
						{
							AppliedServers += ", " + server[1].ToString();
						}
					}

				}
				if (ServerErrors != "")
				{
					//12/12/2013 NS modified
					//lblError.Text = "Settings for servers :" + ServerErrors + " are NOT updated";
					errorDiv2.InnerHtml = "Settings for the server(s) " + ServerErrors + " were NOT updated." +
						 "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
					errorDiv2.Style.Value = "display: block";
					FillServerTypeComboBox(profilevalue);
					FillprofileComboBox();
					//ServerTypeComboBox.SelectedIndex = -1;
					//ExchangeRolesComboBox.SelectedIndex = -1;
					ServersTreeList.UnselectAll();
					//ServersTreeList.DataSource = null;
					//ServersTreeList.DataBind();
					//ServersTreeList.RefreshVirtualTree();
					//ProfilesGridView.DataSource = null;
					//ProfilesGridView.DataBind();
					//lblMessage.ForeColor = 
				}
				else
				{
					string parameters = "";

					int count = 0;
					foreach (ProfilesMaster fieldValue in list)
					{
						if (count == 0)
						{
							parameters += fieldValue.AttributeName + " = " + fieldValue.DefaultValue;
							count++;
						}
						else
						{
							parameters += ", " + fieldValue.AttributeName + " = " + fieldValue.DefaultValue;
						}
					}
					ProfilesGridView.Selection.UnselectAll();
					ServersTreeList.UnselectAll();
					//ProfilesGridView.DataSource = ProfilesDataTable;
					//ProfilesGridView.DataBind();
					//ServersTreeList.RefreshVirtualTree();

					//12/12/2013 NS moved the code into the else block, otherwise on unsuccessful apply
					//the whole server list would get wiped out
					//Clearing the Data Grids
					//ServerTypeComboBox.Text = "";
					//ProfileComboBox.SelectedIndex = -1;
					//ServerTypeComboBox.SelectedIndex = -1;
					//  ExchangeRolesComboBox.SelectedIndex = -1;
					if (ServerTypeComboBox.Text == "Exchange")
					{
						string role = ExchangeRolesComboBox.SelectedItem.Text;
						if (role == "General")
						{
							FillProfilesGrid(ServerTypeComboBox.SelectedItem.Text, "", ProfileComboBox.SelectedItem.Text);//MS-somaraj change code as fillgrid after applybutton.
							fillServersTreeList();
						}
						else
						{
							FillProfilesGrid(ServerTypeComboBox.SelectedItem.Text, role, ProfileComboBox.SelectedItem.Text);//MS-somaraj change code as fillgrid after applybutton.
							fillServersTreeList();
						}
					}
					else
					{
						FillProfilesGrid(ServerTypeComboBox.SelectedItem.Text, "", ProfileComboBox.SelectedItem.Text);
						fillServersTreeList();
					}
					//DataTable dtrole = (DataTable)Session["Profiles"];
					//ProfilesGridView.DataSource = dtrole;
					//ProfilesGridView.DataBind();
					//fillServersTreeList();

					//ServersTreeList.RefreshVirtualTree();

					//CY: VS463
					//ProfilesGridView.DataSource = null;
					//ProfilesGridView.DataBind();

					//ServersTreeList.DataSource = null;
					//ServersTreeList.DataBind();

					//ServersTreeList.UnselectAll();
					//ServersTreeList.DataSource = null;
					//ServersTreeList.DataBind();
					//Session["DataServers"] = null;
					//Session["Profiles"] = null;

					//12/12/2013 NS modified
					//lblMessage.Text = "Settings: " + parameters + " for the selected servers: " + AppliedServers + " are updated";
					//10/3/2014 NS modified for VSPLUS-990
					successDiv.InnerHtml = "The following settings for the server(s) " + AppliedServers + " were successfully updated: " + parameters +
						"<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
					successDiv.Style.Value = "display: block";
					emptyDiv3.Style.Value = "display: block";

					//CY: VS643
					//CollapseAllButton.Visible = false;
					//ApplyServersButton.Visible = false;
					tblServer.Style.Value = "display: block";
				}
			}
			else
			{
				if (fieldValues.Count == 0)//MS-Raj-VSPLUS2250 modified.
				{
					errorDiv5.InnerHtml = "Please select at least one Attribute to proceed." +
				 "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
					errorDiv5.Style.Value = "display: block";
				}
				else if (Session["DataServers"]==null)
				{
					errorDiv7.Style.Value = "display: block";
					ProfilesGridView.Selection.UnselectAll();
				}
				else if (serversSelected.Count == 0)
				{
					errorDiv6.InnerHtml = "Please select at least one Server to proceed." +
				 "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
					errorDiv6.Style.Value = "display: block";
				}
				else
				{
					//12/12/2013 NS modified
					//lblError.Text = "Please select required Attributes and Servers";
					errorDiv.InnerHtml = "Please select at least one Attribute and one Server in order to proceed." +
				 "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
					errorDiv.Style.Value = "display: block";
				}
			}
		}

		private DataTable GetSelectedServers()
		{

			DataTable dtSel = new DataTable();
			try
			{
				dtSel.Columns.Add("ServerID");
				dtSel.Columns.Add("Name");

				//string selValues = "";
				TreeListNodeIterator iterator = ServersTreeList.CreateNodeIterator();
				TreeListNode node;

				while (true)
				{
					node = iterator.GetNext();

					if (node == null) break;
					if (node.Level == 2) //(node.ParentNode.Selected==false)
					{

						if (node.Selected)
						{
							DataRow dr = dtSel.NewRow();
							dr["ServerID"] = node.GetValue("actid");
							dr["Name"] = node.GetValue("Name");
							dtSel.Rows.Add(dr);
						}
					}


				}


			}
			catch (Exception ex)
			{
				//Log.Entry.Ins.Write(Server.MapPath("~/LogFiles/"), "VSPlusLog.txt", DateTime.Now.ToString() + " Error in Page: " +
				//    Request.Url.AbsolutePath + ", Method: " + System.Reflection.MethodBase.GetCurrentMethod().Name +
				//    ", Error: " + ex.ToString());
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}

			return dtSel;

		}

		protected void CollapseAllButton_Domino_Click(object sender, EventArgs e)
		{
			try
			{
				if (CollapseAllButton_Domino.Text == "Collapse All")
				{
					DominoServerTreeList.CollapseAll();
					CollapseAllButton_Domino.Image.Url = "~/images/icons/add.png";
					CollapseAllButton_Domino.Text = "Expand All";
				}
				else
				{
					DominoServerTreeList.ExpandAll();
					CollapseAllButton_Domino.Image.Url = "~/images/icons/forbidden.png";
					CollapseAllButton_Domino.Text = "Collapse All";
				}
			}
			catch (Exception ex)
			{
				//Log.Entry.Ins.Write(Server.MapPath("~/LogFiles/"), "VSPlusLog.txt", DateTime.Now.ToString() + " Error in Page: " +
				//    Request.Url.AbsolutePath + ", Method: " + System.Reflection.MethodBase.GetCurrentMethod().Name +
				//    ", Error: " + ex.ToString());
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}
		}

		protected void DominoServerTasksApply_Click(object sender, EventArgs e)
		{
			UpdateTasksForDominoServers(1);
		}

		protected void DominoServerTasksClear_Click(object sender, EventArgs e)
		{
			UpdateTasksForDominoServers(0);
		}

		protected void UpdateTasksForDominoServers(int Enabled)
		{
			successDivDomino.Style.Value = "display: none";
			errorDiv3.Style.Value = "display: none;";
			errorDiv4.Style.Value = "display: none";
			emptyDiv4.Style.Value = "display: block";
			List<object> fieldValues = DominoTasksGridView.GetSelectedFieldValues(new string[] { "TaskID" });//, "SendLoadCommand", "SendRestartCommand", "RestartOffHours", "SendExitCommand" 
			//List<object> serversSelected = ServersGridView.GetSelectedFieldValues(new string[] {"ID","ServerName" });
			DataTable dt = GetSelectedDominoServers();
			List<DataRow> serversSelected = dt.AsEnumerable().ToList();
			//List<DataRow> list = dt.AsEnumerable().ToList();
			int Update = 0;
			string ServerErrors = "";
			string AppliedServers = "";
			if (fieldValues.Count > 0 && serversSelected.Count > 0)
			{
				foreach (DataRow server in serversSelected)
				{
					
					for (int i = 0; i < DSTasksDataTable.Rows.Count; i++)
					{
						if (DSTasksDataTable.Rows[i]["isSelected"].ToString() == "true")
						{
							int SendLoadCommand = 0;
							int SendRestartCommand = 0;
							int RestartOffHours = 0;
							int SendExitCommand = 0;
							if (Enabled == 1)
							{
								if (DSTasksDataTable.Rows[i]["SendLoadCommand"].ToString() == "true")
								{
									SendLoadCommand = 1;
								}
								if (DSTasksDataTable.Rows[i]["SendRestartCommand"].ToString() == "true")
								{
									SendRestartCommand = 1;
								}
								if (DSTasksDataTable.Rows[i]["RestartOffHours"].ToString() == "true")
								{
									RestartOffHours = 1;
								}
								if (DSTasksDataTable.Rows[i]["SendExitCommand"].ToString() == "true")
								{
									SendExitCommand = 1;
								}
							}
							Update = VSWebBL.ConfiguratorBL.DominoServerTasksBL.Ins.UpdateDominoServerTasks(Convert.ToInt32(DSTasksDataTable.Rows[i]["TaskID"]), Convert.ToInt32(server[0]), Enabled, SendLoadCommand, SendRestartCommand, RestartOffHours, SendExitCommand);
							if (Update == 0)
							{
								if (ServerErrors == "")
								{
									ServerErrors += server[1].ToString();
								}
								else
								{
									if (ServerErrors.Contains(server[1].ToString()))
									{
									}
									else
									{
										ServerErrors += ", " + server[1].ToString();
									}
								}
							}
							else
							{
								//12/12/2013 NS modified
								if (AppliedServers == "")
								{
									AppliedServers += server[1].ToString();
								}
								else
								{
									if (AppliedServers.Contains(server[1].ToString()))
									{
									}
									else
									{
										AppliedServers += ", " + server[1].ToString();
									}
								}
							}
							if (ServerErrors != "")
							{
								//10/3/2014 NS modified for VSPLUS-990
								errorDiv4.InnerHtml = "Settings for the server(s) " + ServerErrors + " were NOT updated." +
									"<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
								errorDiv4.Style.Value = "display: block";
							}
							else
							{
								int count = 0;
								string parameters = "";
								for (int num = 0; num < DSTasksDataTable.Rows.Count; num++)
								{
									if (DSTasksDataTable.Rows[num]["isSelected"].ToString() == "true")
									{
										if (count == 0)
										{
											parameters = Convert.ToString(DSTasksDataTable.Rows[num]["TaskName"]);
											count++;
										}
										else
										{
											parameters += "," + Convert.ToString(DSTasksDataTable.Rows[num]["TaskName"]);
										}
									}
								}
								DominoTasksGridView.Selection.UnselectAll();
								DominoServerTreeList.UnselectAll();
								if (Enabled == 1)
								{
									//10/3/2014 NS modified for VSPLUS-990
									successDivDomino.InnerHtml = "The following server tasks for the server(s) " + AppliedServers + " were successfully added: " + parameters +
										"<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
								}
								else
								{
									//10/3/2014 NS modified for VSPLUS-990
									successDivDomino.InnerHtml = "The following server tasks for the server(s) " + AppliedServers + " were successfully removed: " + parameters +
										"<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
								}
								successDivDomino.Style.Value = "display: block";
							}
						}
					}
				}
			}
			else
			{
				//12/12/2013 NS modified
				//lblError.Text = "Please select required Attributes and Servers";
				emptyDiv4.Style.Value = "display: block";
				errorDiv3.Style.Value = "display: block";
			}

		}

		private DataTable GetSelectedDominoServers()
		{

			DataTable dtSel = new DataTable();
			try
			{
				dtSel.Columns.Add("ServerID");
				dtSel.Columns.Add("Name");

				//string selValues = "";
				TreeListNodeIterator iterator = DominoServerTreeList.CreateNodeIterator();
				TreeListNode node;

				while (true)
				{
					node = iterator.GetNext();

					if (node == null) break;
					if (node.Level == 2) //(node.ParentNode.Selected==false)
					{

						if (node.Selected)
						{
							DataRow dr = dtSel.NewRow();
							dr["ServerID"] = node.GetValue("actid");
							dr["Name"] = node.GetValue("Name");
							dtSel.Rows.Add(dr);
						}
					}


				}


			}
			catch (Exception ex)
			{
				//Log.Entry.Ins.Write(Server.MapPath("~/LogFiles/"), "VSPlusLog.txt", DateTime.Now.ToString() + " Error in Page: " +
				//    Request.Url.AbsolutePath + ", Method: " + System.Reflection.MethodBase.GetCurrentMethod().Name +
				//    ", Error: " + ex.ToString());
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}

			return dtSel;

		}

		protected void ProfilesGridView_OnPageSizeChanged(object sender, EventArgs e)
		{
			//ProfilesGridView.PageIndex;
			VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("ServerSettingsEditor|ProfilesGridView", ProfilesGridView.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
			Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
		}
		protected void DominoTasksGridView_OnPageSizeChanged(object sender, EventArgs e)
		{
			//ProfilesGridView.PageIndex;
			VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("ServerSettingsEditor|DominoTasksGridView", DominoTasksGridView.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
			Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
		}

		protected void WindowsServices_OnPageSizeChanged(object sender, EventArgs e)
		{
			//ProfilesGridView.PageIndex;
			VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("ServerSettingsEditor|WindowsServices", WindowsServices.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
			Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
		}
		protected void CollapseAllButton_Windows_Click(object sender, EventArgs e)
		{
			try
			{
				if (WindowsCollapseAll.Text == "Collapse All")
				{
					WindowsServerTreeList.CollapseAll();
					WindowsCollapseAll.Image.Url = "~/images/icons/add.png";
					WindowsCollapseAll.Text = "Expand All";
				}
				else
				{
					WindowsServerTreeList.ExpandAll();
					WindowsCollapseAll.Image.Url = "~/images/icons/forbidden.png";
					WindowsCollapseAll.Text = "Collapse All";
				}
			}
			catch (Exception ex)
			{
				//Log.Entry.Ins.Write(Server.MapPath("~/LogFiles/"), "VSPlusLog.txt", DateTime.Now.ToString() + " Error in Page: " +
				// Request.Url.AbsolutePath + ", Method: " + System.Reflection.MethodBase.GetCurrentMethod().Name +
				// ", Error: " + ex.ToString());
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}
		}

		private void FillExchangeServerServicesGrid()
		{
			try
			{
				//DSTaskSettingsDataSet = new DataSet();
				Session["WindowsServices"] = null;

				ExchangeDataTable = new DataTable();

				ExchangeDataTable = VSWebBL.ConfiguratorBL.DominoServerTasksBL.Ins.GetExchangeServiceData(ServerTypes.Value.ToString());
				ExchangeDataTable.Columns.Add("VitalSignsDisplayName");
				foreach (DataRow row in ExchangeDataTable.Rows)
				{
					row["VitalSignsDisplayName"] = row["DisplayName"] == null || row["DisplayName"] == "" ? row["Service_Name"] : row["DisplayName"];
				}
				ExchangeDataTable.DefaultView.Sort = "VitalSignsDisplayName asc";
				ExchangeDataTable = ExchangeDataTable.DefaultView.ToTable();
				//DataTable dtcopy = DSTasksDataTable.Copy();
				//dtcopy.Columns.Add("SendLoadCommand");
				//dtcopy.Columns.Add("SendRestartCommand");
				//dtcopy.Columns.Add("RestartOffHours");
				//dtcopy.Columns.Add("SendExitCommand");
				//DSTaskSettingsDataSet.Tables.Add(dtcopy);
				if (ExchangeDataTable.Rows.Count > 0)
				{

					Session["WindowsServices"] = ExchangeDataTable;// DSTaskSettingsDataSet.Tables[0];

				}
				WindowsServices.PageIndex = 0;
				WindowsServices.DataSource = ExchangeDataTable;//DSTaskSettingsDataSet.Tables[0];
				WindowsServices.DataBind();

			}
			catch (Exception ex)
			{
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}
			finally { }
		}

		private void FillExchangeServerServicesGridfromSession()
		{
			try
			{
				ExchangeDataTable = new DataTable();
				if (Session["WindowsServices"] != "" && Session["WindowsServices"] != null)
				{
					ExchangeDataTable = (DataTable)Session["WindowsServices"];
				}
				if (ExchangeDataTable.Rows.Count > 0)
				{
					int startIndex = WindowsServices.PageIndex * WindowsServices.SettingsPager.PageSize;
					int endIndex = Math.Min(WindowsServices.VisibleRowCount, startIndex + WindowsServices.SettingsPager.PageSize);
					for (int i = startIndex; i < endIndex; i++)
					{
						if (WindowsServices.Selection.IsRowSelected(i))
						{
							//object o = DominoTasksGridView.GetRowValues(i, "SendLoadCommand");

							ExchangeDataTable.Rows[i]["isSelected"] = "true";
							ASPxCheckBox Monitored1 = WindowsServices.FindRowCellTemplateControl(i, WindowsServices.Columns[5] as GridViewDataColumn, "Monitored1") as ASPxCheckBox;

							if (Monitored1 != null && Monitored1.Checked == true)
							{
								ExchangeDataTable.Rows[i]["Monitored"] = "true";
							}
							else
							{
								ExchangeDataTable.Rows[i]["Monitored"] = "false";
							}
						}
						else
						{
							ExchangeDataTable.Rows[i]["isSelected"] = "false";
						}


					}
					//WindowsServices.PageIndex = 0;
					WindowsServices.DataSource = ExchangeDataTable;
					WindowsServices.DataBind();
				}
			}
			catch (Exception ex)
			{
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}
			finally { }
		}

		public void FillExchangeServersTreeList()
		{
			try
			{
				//Session["WindowsServers"] = null;
				this.WindowsServerTreeList.RefreshVirtualTree();
				this.WindowsServerTreeList.UnselectAll();
				WindowsServerTreeList.DataBind();
				errorDiv8.Style.Value = "display: none";
				Session["WindowsServers"] = null;
				if (Session["WindowsServers"] == null)
				{
					DataTable DataServersTree = new DataTable();

					if (ServerTypes.Text == "Exchange")
					{
						DataServersTree = VSWebBL.ConfiguratorBL.AlertsBL.Ins.GetExchangeServersFromProcedure();
					}
					else if (ServerTypes.Text == "Active Directory")
					{
						DataServersTree = VSWebBL.ConfiguratorBL.AlertsBL.Ins.GetActiveDirectoryServersFromProcedure();
					}
					else
					{
						DataServersTree = VSWebBL.ConfiguratorBL.AlertsBL.Ins.GetSharePointServersFromProcedure();
					}

					if (DataServersTree.Rows.Count > 0)
					{
						if (Session["RestrictedServers"] != "" && Session["RestrictedServers"] != null)
						{
							List<int> ServerID = new List<int>();
							List<int> LocationID = new List<int>();
							DataTable resServers = (DataTable)Session["RestrictedServers"];
							foreach (DataRow resser in resServers.Rows)
							{
								foreach (DataRow dominorow in DataServersTree.Rows)
								{

									if (resser["serverid"].ToString() == dominorow["actid"].ToString())
									{
										ServerID.Add(DataServersTree.Rows.IndexOf(dominorow));
									}
									if (dominorow["locid"] != null && dominorow["locid"].ToString() != "" && (resser["locationID"].ToString() == dominorow["locid"].ToString()))
									{
										LocationID.Add(Convert.ToInt32(dominorow["locid"].ToString()));
									}
								}

							}
							foreach (int Id in ServerID)
							{
								DataServersTree.Rows[Id].Delete();
							}
							DataServersTree.AcceptChanges();

							foreach (int lid in LocationID)
							{
								DataRow[] row = DataServersTree.Select("locid=" + lid + "");
								for (int i = 0; i < row.Count(); i++)
								{
									DataServersTree.Rows.Remove(row[i]);
									DataServersTree.AcceptChanges();
								}
							}
							DataServersTree.AcceptChanges();
						}
						DataTable filteredData = null;
						string query = "";
						if (ServerTypes.Text == "Exchange")
						{
							query = "(ServerType='Exchange' or ServerType is null)";
						}
						else if (ServerTypes.Text == "SharePoint")
						{
							query = " ServerType='SharePoint' or ServerType is null";
						}
						else if (ServerTypes.Text.Contains("ActiveDirectory"))
						{
							query = " ServerType='ActiveDirectory' or ServerType is null ";
						}

						//else if (ExchangeRoles.Text.Contains("CAS"))
						//{
						//    query += " and (RoleType like '%CAS%')";
						//}
						if (DataServersTree.Select(query).Length != 0)
						{
							filteredData = DataServersTree.Select(query).CopyToDataTable();
						}

						filteredData = DataServersTree.Select(query).CopyToDataTable();

						Session["WindowsServers"] = filteredData;
						var servers = from n in filteredData.AsEnumerable()
									  where n.Field<string>("tbl").Contains("Servers")
									  select n;
						var Locations = from n in filteredData.AsEnumerable()
										where n.Field<string>("tbl").Contains("Locations")
										select n;
						DataTable filteredDT = null;
						DataTable Serversdt = null;
						DataTable Locationsdt = null;
						DataTable sum = new DataTable();
						if ((servers != null && servers.Count() > 0) && (Locations != null && Locations.Count() > 0))
						{
							Serversdt = servers.CopyToDataTable();
							Locationsdt = Locations.CopyToDataTable();
							sum = Locationsdt.Copy();
							sum.Merge(Serversdt);
							var customers = sum.AsEnumerable();
							var orders = Locationsdt.AsEnumerable();
							var LocationIdNotNull = from n in sum.AsEnumerable()
													where !n.IsNull("LocId")
													select n;
							filteredDT = LocationIdNotNull.CopyToDataTable();
							DataTable filterbylocation = (from n in Locationsdt.AsEnumerable()
														  join prod in filteredDT.AsEnumerable() on n.Field<int>("id") equals prod.Field<int>("LocId")
														  select n).Distinct().CopyToDataTable();
							DataTable Filteredservers = new DataTable();
							Filteredservers = filterbylocation.Copy();
							Filteredservers.Merge(filteredDT);
							Session["WindowsServers"] = Filteredservers;
							//Session["DataServers"] = sum2;//somaraj
							//ServersTreeList.DataSource = (DataTable)Session["DataServers"];
							//ServersTreeList.DataBind();
							//ServersTreeList.ExpandAll();
						}
						else
						{
							Session["WindowsServers"] = null;
							WindowsServerTreeList.DataSource = "";
							WindowsServerTreeList.DataBind();
							errorDiv8.InnerHtml = "The server(s) do not exist." +
					"<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";

							errorDiv8.Style.Value = "display: block";
							WindowsServerTreeList.ExpandAll();
							WindowsServerTreeList.RefreshVirtualTree();
							WindowsServerTreeList.ClearNodes();
							this.WindowsServerTreeList.RefreshVirtualTree();
						}

					}
					else if (DataServersTree.Rows.Count == 0)
					{
						Session["WindowsServers"] = null;
						WindowsServerTreeList.DataSource = "";
						WindowsServerTreeList.DataBind();
						errorDiv8.InnerHtml = "The server(s) do not exist." +
				"<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";

						errorDiv8.Style.Value = "display: block";
						WindowsServerTreeList.ExpandAll();
						WindowsServerTreeList.RefreshVirtualTree();
						WindowsServerTreeList.ClearNodes();
						this.WindowsServerTreeList.RefreshVirtualTree();
					}
				}

				WindowsServerTreeList.PageIndex = 0;//MsRaj								  

				WindowsServerTreeList.DataSource = (DataTable)Session["WindowsServers"];
				WindowsServerTreeList.DataBind();
				WindowsServerTreeList.ExpandAll();
			}
			catch (Exception ex)
			{
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}
			finally { }
		}

		public void fillExchangeServersTreeListfromSession()
		{
			DataTable DataServers = new DataTable();
			try
			{
				if (Session["WindowsServers"] != "" && Session["WindowsServers"] != null)
					DataServers = Session["WindowsServers"] as DataTable;
				if (DataServers.Rows.Count > 0)
				{
					DataServers.PrimaryKey = new DataColumn[] { DataServers.Columns["ServiceId"] };
				}
				WindowsServerTreeList.DataSource = DataServers;
				WindowsServerTreeList.DataBind();

			}
			catch (Exception ex)
			{
				//Log.Entry.Ins.Write(Server.MapPath("~/LogFiles/"), "VSPlusLog.txt", DateTime.Now.ToString() + " Error in Page: " +
				//    Request.Url.AbsolutePath + ", Method: " + System.Reflection.MethodBase.GetCurrentMethod().Name +
				//    ", Error: " + ex.ToString());
			}
		}
		private DataTable GetSelectedExchnageServers()
		{

			DataTable dtSel = new DataTable();
			try
			{
				dtSel.Columns.Add("ID");
				dtSel.Columns.Add("Name");

				//string selValues = "";
				TreeListNodeIterator iterator = WindowsServerTreeList.CreateNodeIterator();
				TreeListNode node;

				while (true)
				{
					node = iterator.GetNext();

					if (node == null) break;
					if (node.Level == 2) //(node.ParentNode.Selected==false)
					{

						if (node.Selected)
						{
							DataRow dr = dtSel.NewRow();
							dr["ID"] = node.GetValue("actid");
							dr["Name"] = node.GetValue("Name");
							dtSel.Rows.Add(dr);
						}
					}


				}


			}
			catch (Exception ex)
			{
				//Log.Entry.Ins.Write(Server.MapPath("~/LogFiles/"), "VSPlusLog.txt", DateTime.Now.ToString() + " Error in Page: " +
				//    Request.Url.AbsolutePath + ", Method: " + System.Reflection.MethodBase.GetCurrentMethod().Name +
				//    ", Error: " + ex.ToString());
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}

			return dtSel;

		}

		protected void WindowsServerServicesApply_Click(object sender, EventArgs e)
		{
			UpdateServicesForExchangeServers(1);
		}

		protected void ExchangeServerServicesClear_Click(object sender, EventArgs e)
		{
			UpdateServicesForExchangeServers(0);
		}
		protected void UpdateServicesForExchangeServers(int Enabled)
		{
			successDivDomino.Style.Value = "display: none";
			errorDiv3.Style.Value = "display: none;";
			errorDiv4.Style.Value = "display: none";
			emptyDiv4.Style.Value = "display: block";

			WindowsError.Style.Value = "display: none";
			WindowsSuccess.Style.Value = "display: none";


			List<object> fieldValues = WindowsServices.GetSelectedFieldValues("DisplayName");//, "SendLoadCommand", "SendRestartCommand", "RestartOffHours", "SendExitCommand" 
			//List<object> serversSelected = ServersGridView.GetSelectedFieldValues(new string[] {"ID","ServerName" });
			DataTable dt = GetSelectedExchnageServers();
			List<DataRow> serversSelected = dt.AsEnumerable().ToList();
			//List<DataRow> list = dt.AsEnumerable().ToList();
			int Update = 0;
			string ServerErrors = "";

			if (fieldValues.Count > 0 && serversSelected.Count > 0)
			{
				foreach (DataRow server in serversSelected)
				{
					string AppliedServers = "";
					for (int i = 0; i < ExchangeDataTable.Rows.Count; i++)
					{
						if (ExchangeDataTable.Rows[i]["isSelected"].ToString() == "true")
						{
							int isMonitored = Enabled;
							//int isMonitored = 0;
							//if (ExchangeDataTable.Rows[i]["Monitored"].ToString() == "true")
							//{
							//    isMonitored = 1;
							//}
							Update = VSWebBL.ConfiguratorBL.MailServicesBL.Ins.UpdateEXGServiceSettings(isMonitored, Convert.ToString(ExchangeDataTable.Rows[i]["DisplayName"]), Convert.ToString(ExchangeDataTable.Rows[i]["Service_Name"]), ServerTypes.Text, Convert.ToInt32(server["ID"]));
							if (Update == 0)
							{
								if (ServerErrors == "")
								{
									ServerErrors += server[1].ToString();
								}
								else
								{
									if (ServerErrors.Contains(server[1].ToString()))
									{
									}
									else
									{
										ServerErrors += ", " + server[1].ToString();
									}
								}
							}
							else
							{
								//12/12/2013 NS modified
								if (AppliedServers == "")
								{
									AppliedServers += server[1].ToString();
								}
								else
								{
									if (AppliedServers.Contains(server[1].ToString()))
									{
									}
									else
									{
										AppliedServers += ", " + server[1].ToString();
									}
								}
							}
							if (ServerErrors != "")
							{
								//10/3/2014 NS modified for VSPLUS-990
								WindowsError.InnerHtml = "Settings for the server(s) " + ServerErrors + " were NOT updated." +
									"<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
								WindowsError.Style.Value = "display: block";
							}
							else
							{
								int count = 0;
								string parameters = "";
								for (int num = 0; num < ExchangeDataTable.Rows.Count; num++)
								{
									if (ExchangeDataTable.Rows[num]["isSelected"].ToString() == "true")
									{
										if (count == 0)
										{
											parameters = Convert.ToString(ExchangeDataTable.Rows[num]["DisplayName"]);
											count++;
										}
										else
										{
											parameters += "," + Convert.ToString(ExchangeDataTable.Rows[num]["DisplayName"]);
										}
									}
								}
								WindowsServices.Selection.UnselectAll();
								WindowsServerTreeList.UnselectAll();
								//10/3/2014 NS modified for VSPLUS-990
								WindowsSuccess.InnerHtml = "The following services for the server(s) " + AppliedServers + " were successfully updated: " + parameters +
									"<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";

								WindowsSuccess.Style.Value = "display: block";
							}
						}
					}
				}
			}
			else
			{
				//10/3/2014 NS modified for VSPLUS-990
				WindowsError.InnerHtml = "Please select at least one server and one service." +
					"<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
				WindowsError.Style.Value = "display: block";

			}

		}

		protected void ServerTypes_SelectedIndexChanged(object sender, EventArgs e)
		{
			FillExchangeServerServicesGrid();
			FillExchangeServersTreeList();
		}

		#region "Set select checkbox"
		protected void SelectCheckboxSendLoadCommand_Init(object sender, EventArgs e)
		{
			GridViewDataItemTemplateContainer c = ((ASPxCheckBox)sender).NamingContainer as
		GridViewDataItemTemplateContainer;
			int index = c.VisibleIndex;
			string key = c.KeyValue.ToString();
			string ownerGridClientInstanceName = c.Grid.ClientInstanceName;

			((ASPxCheckBox)sender).ClientSideEvents.CheckedChanged = "function(s,e){ProcessSelection(" +
				ownerGridClientInstanceName + "," + index + ",s," + key + ",SendLoadCommandSelection);}";

			//if (c.Grid.Selection.IsRowSelected(index))
			//{
			//    ((ASPxCheckBox)sender).Checked = true;
			//}
			if (SendLoadCommandSelection.Contains("key" + key.ToString()))
			{
				((ASPxCheckBox)sender).Checked = true;
			}
		}
		protected void SelectCheckboxRestartASAP_Init(object sender, EventArgs e)
		{
			GridViewDataItemTemplateContainer c = ((ASPxCheckBox)sender).NamingContainer as
		GridViewDataItemTemplateContainer;
			int index = c.VisibleIndex;
			string key = c.KeyValue.ToString();
			string ownerGridClientInstanceName = c.Grid.ClientInstanceName;

			((ASPxCheckBox)sender).ClientSideEvents.CheckedChanged = "function(s,e){ProcessSelection(" +
				ownerGridClientInstanceName + "," + index + ",s," + key + ",RestartASAPSelection);}";

			//if (c.Grid.Selection.IsRowSelected(index))
			//{
			//    ((ASPxCheckBox)sender).Checked = true;
			//}
			if (RestartASAPSelection.Contains("key" + key.ToString()))
			{
				((ASPxCheckBox)sender).Checked = true;
			}
		}
		protected void SelectCheckboxRestartOffHours_Init(object sender, EventArgs e)
		{
			GridViewDataItemTemplateContainer c = ((ASPxCheckBox)sender).NamingContainer as
		GridViewDataItemTemplateContainer;
			int index = c.VisibleIndex;
			string key = c.KeyValue.ToString();
			string ownerGridClientInstanceName = c.Grid.ClientInstanceName;

			((ASPxCheckBox)sender).ClientSideEvents.CheckedChanged = "function(s,e){ProcessSelection(" +
				ownerGridClientInstanceName + "," + index + ",s," + key + ",RestartOffHoursSelection);}";

			//if (c.Grid.Selection.IsRowSelected(index))
			//{
			//    ((ASPxCheckBox)sender).Checked = true;
			//}
			if (RestartOffHoursSelection.Contains("key" + key.ToString()))
			{
				((ASPxCheckBox)sender).Checked = true;
			}
		}
		protected void SelectCheckboxSendExitCommand_Init(object sender, EventArgs e)
		{
			GridViewDataItemTemplateContainer c = ((ASPxCheckBox)sender).NamingContainer as
		GridViewDataItemTemplateContainer;
			int index = c.VisibleIndex;
			string key = c.KeyValue.ToString();
			string ownerGridClientInstanceName = c.Grid.ClientInstanceName;

			((ASPxCheckBox)sender).ClientSideEvents.CheckedChanged = "function(s,e){ProcessSelection(" +
				ownerGridClientInstanceName + "," + index + ",s," + key + ",SendExitCommandSelection);}";

			//if (c.Grid.Selection.IsRowSelected(index))
			//{
			//    ((ASPxCheckBox)sender).Checked = true;
			//}
			if (SendExitCommandSelection.Contains("key" + key.ToString()))
			{
				((ASPxCheckBox)sender).Checked = true;
			}
		}

		protected void CheckboxMonitored1_CheckChanged(object sender, EventArgs e)
		{
			ASPxCheckBox Checkbox = sender as ASPxCheckBox;
			DataTable dt = new DataTable();
			if (Session["WindowsServices"] != "" && Session["WindowsServices"] != null)
			{
				dt = (DataTable)Session["WindowsServices"];
			}

			if (dt.Rows.Count > 0)
			{
				GridViewRow row = Checkbox.NamingContainer as GridViewRow;
				DataRow CurrRow = dt.Select("Displayname=" + ((ASPxLabel)(row.FindControl("Service Name"))).Text)[0];
				CurrRow["Monitored"] = Checkbox.Checked;

				// DataColumn[] key = new DataColumn[1];
				// key[0] = dt.Columns["DisplayName"];
				// dt.PrimaryKey = key;
				// dt.se
			}

		}
		//protected void SelectCheckboxServiceMonitored_Init(object sender, EventArgs e)
		//{
		//    GridViewDataItemTemplateContainer c = ((ASPxCheckBox)sender).NamingContainer as
		//GridViewDataItemTemplateContainer;
		//    int index = c.VisibleIndex;
		//    string key = c.KeyValue.ToString();
		//    string ownerGridClientInstanceName = c.Grid.ClientInstanceName;

		//    ((ASPxCheckBox)sender).ClientSideEvents.CheckedChanged = "function(s,e){ProcessSelection(" +
		//        ownerGridClientInstanceName + "," + index + ",s," + key + ",ServiceMonitoredSelection);}";

		//    //if (c.Grid.Selection.IsRowSelected(index))
		//    //{
		//    //    ((ASPxCheckBox)sender).Checked = true;
		//    //}

		//    if (ServiceMonitoredSelection.Contains("key" + key.ToString()))
		//    {
		//        ((ASPxCheckBox)sender).Checked = true;
		//    }
		//}
		#endregion

		//Disk Settings
		public void FillDiskSettingsGridView()
		{
			try
			{
				Session["DiskSettingsDisks"] = null;
				DisksDataTable = new DataTable();

				DisksDataTable = VSWebBL.ConfiguratorBL.DominoServerTasksBL.Ins.GetDisksData();

				if (DisksDataTable.Rows.Count > 0)
				{

					Session["DiskSettingsDisks"] = DisksDataTable;// DSTaskSettingsDataSet.Tables[0];

				}
				DiskGridView.DataSource = DisksDataTable;//DSTaskSettingsDataSet.Tables[0];
				DiskGridView.DataBind();

			}
			catch (Exception ex)
			{
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}
			finally { }
		}

		public void FillDiskSettingsGridViewfromSession()
		{
			try
			{
				DisksDataTable = new DataTable();
				if (Session["DiskSettingsDisks"] != "" && Session["DiskSettingsDisks"] != null)
				{
					DisksDataTable = (DataTable)Session["DiskSettingsDisks"];
				}
				if (DisksDataTable.Rows.Count > 0)
				{
					int startIndex = DiskGridView.PageIndex * DiskGridView.SettingsPager.PageSize;
					int endIndex = Math.Min(DiskGridView.VisibleRowCount, startIndex + DiskGridView.SettingsPager.PageSize);
					for (int i = startIndex; i < endIndex; i++)
					{
						if (SelCriteriaRadioButtonList.SelectedItem != null && SelCriteriaRadioButtonList.SelectedItem.Value.ToString() == "1")
						{
							if (DiskGridView.Selection.IsRowSelected(i))
							{
								DisksDataTable.Rows[i]["isSelected"] = "true";
								GridViewDataColumn column1 = DiskGridView.Columns["Threshold"] as GridViewDataColumn;
								GridViewDataColumn column2 = DiskGridView.Columns["ThresholdType"] as GridViewDataColumn;
								DataTable dt = new DataTable();

								ASPxTextBox txtThreshold = (ASPxTextBox)DiskGridView.FindRowCellTemplateControl(i, column1, "txtFreeSpaceThresholdValue");
								ASPxComboBox txtThresholdType = (ASPxComboBox)DiskGridView.FindRowCellTemplateControl(i, column2, "txtFreeSpaceThresholdType");
								DisksDataTable.Rows[i]["Threshold"] = txtThreshold.Text;
								DisksDataTable.Rows[i]["ThresholdType"] = txtThresholdType.SelectedItem.Text;

							}
						}
						else
						{
							DisksDataTable.Rows[i]["isSelected"] = "false";
						}

					}
					DiskGridView.DataSource = DisksDataTable;
					DiskGridView.DataBind();
				}
			}
			catch (Exception ex)
			{
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}
			finally { }
		}

		public void FillDiskSettingsTreeList()
		{
			try
			{
				Session["DiskSettingsServers"] = null;
				if (Session["DiskSettingsServers"] == null)
				{
					DataTable DataServersTree = new DataTable();
					string Page = "ServerSettingsEditor.aspx", Control = "DiskSettingsTreeList";
					//DataServersTree = VSWebBL.ConfiguratorBL.AlertsBL.Ins.GetSpecificServersFromProcedure(Page, Control);
                    //14-04-2016 Durga Modified for VSPLUS-2841
                    DataTable tblFiltered = (DataTable)Session["IntialAllservers"];
                    //22/7/2016 Durga Modified for VSPLUS-3125
                    DataServersTree = tblFiltered.AsEnumerable()
                             .Where(r => r.Field<string>("ServerType") != "URL" && r.Field<string>("ServerType") != "Office365")
                             .CopyToDataTable();
                    
					if (DataServersTree.Rows.Count > 0)
					{
						if (Session["RestrictedServers"] != "" && Session["RestrictedServers"] != null)
						{
							List<int> ServerID = new List<int>();
							List<int> LocationID = new List<int>();
							DataTable resServers = (DataTable)Session["RestrictedServers"];
							foreach (DataRow resser in resServers.Rows)
							{
								foreach (DataRow dominorow in DataServersTree.Rows)
								{
									if (resser["serverid"].ToString() == dominorow["actid"].ToString())
									{
										ServerID.Add(DataServersTree.Rows.IndexOf(dominorow));
									}
									if (dominorow["locid"] != null && dominorow["locid"].ToString() != "" && (resser["locationID"].ToString() == dominorow["locid"].ToString()))
									{
										LocationID.Add(Convert.ToInt32(dominorow["locid"].ToString()));
									}
								}
							}
							foreach (int Id in ServerID)
							{
								DataServersTree.Rows[Id].Delete();
							}
							DataServersTree.AcceptChanges();

							foreach (int lid in LocationID)
							{
								DataRow[] row = DataServersTree.Select("locid=" + lid + "");
								for (int i = 0; i < row.Count(); i++)
								{
									DataServersTree.Rows.Remove(row[i]);
									DataServersTree.AcceptChanges();
								}
							}
							DataServersTree.AcceptChanges();
						}

						//VE-23 24-Jun-14, Mukund commented to get all servers
						//DataTable filteredData = DataServersTree.Select("ServerType= 'Domino' or ServerType is null").CopyToDataTable();
						//Session["DiskSettingsServers"] = filteredData;
						Session["DiskSettingsServers"] = DataServersTree;

					}
				}
				DiskSettingsTreeList.DataSource = (DataTable)Session["DiskSettingsServers"];
				DiskSettingsTreeList.DataBind();
				DiskSettingsTreeList.ExpandAll();
			}

			catch (Exception ex)
			{
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}
			finally { }
		}

		public void FillDiskSettingsTreeListfromSession()
		{
			DataTable DataServers = new DataTable();
			try
			{
				if (Session["DiskSettingsServers"] != "" && Session["DiskSettingsServers"] != null)
					DataServers = Session["DiskSettingsServers"] as DataTable;
				if (DataServers.Rows.Count > 0)
				{
					DataServers.PrimaryKey = new DataColumn[] { DataServers.Columns["ID"] };
				}
				DiskSettingsTreeList.DataSource = DataServers;
				DiskSettingsTreeList.DataBind();

			}
			catch (Exception ex)
			{
				//Log.Entry.Ins.Write(Server.MapPath("~/LogFiles/"), "VSPlusLog.txt", DateTime.Now.ToString() + " Error in Page: " +
				//    Request.Url.AbsolutePath + ", Method: " + System.Reflection.MethodBase.GetCurrentMethod().Name +
				//    ", Error: " + ex.ToString());
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}
		}

		protected void CollapseAllButton_DiskSettings_Click(object sender, EventArgs e)
		{
			try
			{
				if (DiskSettingsCollapseAll.Text == "Collapse All")
				{
					DiskSettingsTreeList.CollapseAll();
					DiskSettingsCollapseAll.Image.Url = "~/images/icons/add.png";
					DiskSettingsCollapseAll.Text = "Expand All";
				}
				else
				{
					DiskSettingsTreeList.ExpandAll();
					DiskSettingsCollapseAll.Image.Url = "~/images/icons/forbidden.png";
					DiskSettingsCollapseAll.Text = "Collapse All";
				}
			}
			catch (Exception ex)
			{
				//Log.Entry.Ins.Write(Server.MapPath("~/LogFiles/"), "VSPlusLog.txt", DateTime.Now.ToString() + " Error in Page: " +
				//    Request.Url.AbsolutePath + ", Method: " + System.Reflection.MethodBase.GetCurrentMethod().Name +
				//    ", Error: " + ex.ToString());
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}
		}

		private DataTable GetSelectedDiskSettingsServers()
		{

			DataTable dtSel = new DataTable();
			try
			{
				dtSel.Columns.Add("ServerID");
				dtSel.Columns.Add("Name");

				//string selValues = "";
				TreeListNodeIterator iterator = DiskSettingsTreeList.CreateNodeIterator();
				TreeListNode node;

				while (true)
				{
					node = iterator.GetNext();

					if (node == null) break;
					if (node.Level == 2) //(node.ParentNode.Selected==false)
					{
						if (node.Selected)
						{
							DataRow dr = dtSel.NewRow();
							dr["ServerID"] = node.GetValue("actid");
							dr["Name"] = node.GetValue("Name");
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

		protected void AdvDiskSpaceThTrackBar_ValueChanged(object sender, EventArgs e)
		{
			//12/17/2013 NS modified
			//DiskRoundPanel7.HeaderText = "Disk Space Alert at  " + AdvDiskSpaceThTrackBar.Value + "% free space";
			DiskLabel.Text = "Current threshold: " + AdvDiskSpaceThTrackBar.Value + "% free space";
		}



		protected void SelCriteriaRadioButtonList_SelectedIndexChanged(object sender, EventArgs e)
		{
			//5/12/2014 NS modified for VSPLUS-615
			if (SelCriteriaRadioButtonList.SelectedItem != null && SelCriteriaRadioButtonList.SelectedItem.Value.ToString() == "0")
			{
				DiskLabel.Visible = true;
				DiskLabel.Text = "Current threshold: " + AdvDiskSpaceThTrackBar.Value + "% free space";
				DiskGridView.Visible = false;
				for (int i = 0; i < DiskGridView.VisibleRowCount; i++)
				{
					DiskGridView.Selection.SetSelection(i, false);
				}
				GBLabel.Visible = false;
				GBTextBox.Visible = false;
				GBTitle.Visible = false;
			}
			else if (SelCriteriaRadioButtonList.SelectedItem != null && SelCriteriaRadioButtonList.SelectedItem.Value.ToString() == "1")
			{
				DiskGridView.Visible = true;
				AdvDiskSpaceThTrackBar.Visible = false;
				DiskLabel.Visible = false;

				GBLabel.Visible = false;
				GBTextBox.Visible = false;
				GBTitle.Visible = false;
			}
			else if (SelCriteriaRadioButtonList.SelectedItem != null && SelCriteriaRadioButtonList.SelectedItem.Value.ToString() == "2")
			{
				AdvDiskSpaceThTrackBar.Visible = false;
				DiskLabel.Visible = false;
				DiskGridView.Visible = false;
				for (int i = 0; i < DiskGridView.VisibleRowCount; i++)
				{
					DiskGridView.Selection.SetSelection(i, false);
				}
				GBLabel.Visible = false;
				GBTextBox.Visible = false;
				GBTitle.Visible = false;

			}
			//5/12/2014 NS added for VSPLUS-615
			else if (SelCriteriaRadioButtonList.SelectedItem != null && SelCriteriaRadioButtonList.SelectedItem.Value.ToString() == "3")
			{
				AdvDiskSpaceThTrackBar.Visible = false;
				DiskLabel.Visible = false;
				DiskGridView.Visible = false;
				for (int i = 0; i < DiskGridView.VisibleRowCount; i++)
				{
					DiskGridView.Selection.SetSelection(i, false);
				}
				GBLabel.Visible = true;
				GBTextBox.Visible = true;
				GBTitle.Visible = true;
			}
		}

		protected void DiskSettingsApply_Click(object sender, EventArgs e)
		{
			UpdateData(1);
		}

		protected void DiskSettingsClear_Click(object sender, EventArgs e)
		{
			UpdateData(0);
		}
		private bool UpdateDiskSettings(string serverName, int enabled)
		{
			bool ReturnValue = false;

			try
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("ServerName");
				dt.Columns.Add("DiskName");
				dt.Columns.Add("Threshold");
				dt.Columns.Add("ThresholdType");

				List<object> fieldValues = DiskGridView.GetSelectedFieldValues(new string[] { "DiskName", "Threshold", "ThresholdType" });
				if (SelCriteriaRadioButtonList.SelectedItem != null && SelCriteriaRadioButtonList.SelectedItem.Value.ToString() == "0")
				{
					DataRow row = dt.Rows.Add();
					row["ServerName"] = serverName;
					row["DiskName"] = "AllDisks";
					row["Threshold"] = (Convert.ToInt32(AdvDiskSpaceThTrackBar.Value)).ToString();
					row["ThresholdType"] = "Percent";
				}
				else if (SelCriteriaRadioButtonList.SelectedItem != null && SelCriteriaRadioButtonList.SelectedItem.Value.ToString() == "1")
				//else if(rdbSelFew.Checked)
				{
					foreach (object[] item in fieldValues)
					{
						DataRow row = dt.Rows.Add();
						row["ServerName"] = serverName;
						row["DiskName"] = item[0].ToString();
						row["Threshold"] = (item[1].ToString() != "" ? item[1].ToString() : (Convert.ToInt32(AdvDiskSpaceThTrackBar.Value)).ToString());
						//5/1/2014 NS added for VSPLUS-602
						row["ThresholdType"] = item[2].ToString();
					}
				}
				else if (SelCriteriaRadioButtonList.SelectedItem != null && SelCriteriaRadioButtonList.SelectedItem.Value.ToString() == "2")
				//else if (rdbNoAlerts.Checked)
				{

					DataRow row = dt.Rows.Add();
					row["ServerName"] = serverName;
					row["DiskName"] = "NoAlerts";
					row["Threshold"] = "0";
					//5/1/2014 NS added for VSPLUS-602
					row["ThresholdType"] = "Percent";
				}
				//5/12/2014 NS added for VSPLUS-615
				else if (SelCriteriaRadioButtonList.SelectedItem != null && SelCriteriaRadioButtonList.SelectedItem.Value.ToString() == "3")
				{
					DataRow row = dt.Rows.Add();
					row["ServerName"] = serverName;
					row["DiskName"] = "AllDisks";
					row["Threshold"] = GBTextBox.Text;
					row["ThresholdType"] = "GB";
				}

				//Mukund, 14Apr14 , included if condition to avoid error deleting blank records
				if (dt.Rows.Count > 0)
					ReturnValue = VSWebBL.ConfiguratorBL.DominoPropertiesBL.Ins.InsertDiskSettingsDataSSE(dt, enabled);
			}
			catch (Exception ex)
			{

				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}
			return ReturnValue;

		}

		private void UpdateData(int Enabled)
		{
			bool proceed = true;
			string errtext = "";
			int gbc = 0;
			DiskError.Style.Value = "display: none;";
			DiskError.InnerHtml = "";
			DiskSuccess.Style.Value = "display: none;";
			DiskSuccess.InnerHtml = errtext;
			string ServerErrors = "";
			string AppliedServers = "";
			DataTable dt = GetSelectedDiskSettingsServers();
			List<DataRow> serversSelected = dt.AsEnumerable().ToList();

			if (serversSelected.Count > 0)
			{
				foreach (DataRow server in serversSelected)
				{
					if (SelCriteriaRadioButtonList.SelectedItem != null && SelCriteriaRadioButtonList.SelectedItem.Value.ToString() == "1")
					{
						if (Enabled == 1)
						{
							List<object> fieldValues = DiskGridView.GetSelectedFieldValues(new string[] { "DiskName", "Threshold", "ThresholdType" });
							if (fieldValues.Count == 0)
							{
								proceed = false;
								errtext = "You have enabled a 'Selected Disks' option on the Disk Settings tab but selected no disks " +
								"in the grid or some of the selected disks do not have a threshold or threshold type value. <br />" +
								"Please correct the disk settings in order to save your changes.";
							}
							else
							{
								foreach (object[] item in fieldValues)
								{
									if (item[1].ToString() == "" || item[2].ToString() == "")
									{
										proceed = false;
										//isValid = false;
										errtext = "You have enabled a 'Selected Disks' option on the Disk Settings tab but selected no disks " +
										"in the grid or some of the selected disks do not have a threshold or threshold type value. <br />" +
										"Please correct the disk settings in order to save your changes.";
									}
								}
							}
						}
						else
						{
							proceed = true;
						}
					}
					if (SelCriteriaRadioButtonList.SelectedItem != null && SelCriteriaRadioButtonList.SelectedItem.Value.ToString() == "3")
					{
						if (GBTextBox.Text == "")
						{
							proceed = false;
							errtext = "You have enabled an 'All Disks - By GB' option on the Disk Settings tab but entered no threshold value. " +
								"You must enter a numeric threshold value in order to save your changes.";
						}
						else if (!int.TryParse(GBTextBox.Text, out gbc))
						{
							proceed = false;
							errtext = "You have enabled an 'All Disks - By GB' option on the Disk Settings tab but entered an invalid threshold value. " +
								"You must enter a numeric threshold value in order to save your changes.";
						}
					}
					if (proceed)
					{
						try
						{
							if (!(UpdateDiskSettings(server["Name"].ToString(), Enabled)))
							{
								if (ServerErrors == "")
								{
									ServerErrors += server["Name"].ToString();
								}
								else
								{
									if (ServerErrors.Contains(server["Name"].ToString()))
									{
									}
									else
									{
										ServerErrors += ", " + server["Name"].ToString();
									}
								}
							}
							else
							{
								if (AppliedServers == "")
								{
									AppliedServers += server["Name"].ToString();
								}
								else
								{
									if (AppliedServers.Contains(server["Name"].ToString()))
									{
									}
									else
									{
										AppliedServers += ", " + server["Name"].ToString();
									}
								}
							}

						}
						catch (Exception ex)
						{
							Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
							throw ex;
						}
						finally { }
					}
					else
					{
						DiskError.Style.Value = "display: block;";
						//10/3/2014 NS modified for VSPLUS-990
						DiskError.InnerHtml = errtext +
							"<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
					}
				}
				if (ServerErrors != "")
				{
					DiskError.Style.Value = "display: block;";
					//10/3/2014 NS modified for VSPLUS-990
					DiskError.InnerHtml = "Disk Settings for all the servers: " + ServerErrors + " were NOT updated." +
						"<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
				}
				if (AppliedServers != "")
				{
					DiskSuccess.Style.Value = "display: block;";
					//10/3/2014 NS modified for VSPLUS-990
					DiskSuccess.InnerHtml = "Disk Settings for all the servers: " + AppliedServers + "  were updated." +
						"<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
				}
			}
			else
			{
				DiskError.Style.Value = "display: block;";
				//10/3/2014 NS modified for VSPLUS-990
				DiskError.InnerHtml = "Please select at least one server." +
					"<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
			}
		}

		//Server Location change

		private void FillServerLocations()
		{
			DataTable LocationsDataTable = new DataTable();
			LocationsDataTable = VSWebBL.SecurityBL.LocationsBL.Ins.GetAllData();
			if (LocationsDataTable.Rows.Count > 0)
			{
				ServerLocation.DataSource = LocationsDataTable;
				ServerLocation.TextField = "Location";
				ServerLocation.ValueField = "ID";
				ServerLocation.DataBind();
			}
		}

		protected void CollapseAllButton_ServerLocations_Click(object sender, EventArgs e)
		{
			try
			{
				if (LocationsCollapseAll.Text == "Collapse All")
				{
					ServerLocationsTreeList.CollapseAll();
					LocationsCollapseAll.Image.Url = "~/images/icons/add.png";
					LocationsCollapseAll.Text = "Expand All";
				}
				else
				{
					ServerLocationsTreeList.ExpandAll();
					LocationsCollapseAll.Image.Url = "~/images/icons/forbidden.png";
					LocationsCollapseAll.Text = "Collapse All";
				}
			}
			catch (Exception ex)
			{
				//Log.Entry.Ins.Write(Server.MapPath("~/LogFiles/"), "VSPlusLog.txt", DateTime.Now.ToString() + " Error in Page: " +
				//    Request.Url.AbsolutePath + ", Method: " + System.Reflection.MethodBase.GetCurrentMethod().Name +
				//    ", Error: " + ex.ToString());
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}
		}


		public void FillServerLocationsTreeList()
		{
			try
			{
				Session["ServerLocations"] = null;
				if (Session["ServerLocations"] == null)
				{
					DataTable DataServersTree = new DataTable();
					DataServersTree = VSWebBL.ConfiguratorBL.AlertsBL.Ins.GetServersFromProcedure();
					//DataServersTree = (DataTable)Session["IntialAllservers"];
					DataServersTree = filterTable(DataServersTree);
					Session["ServerLocations"] = DataServersTree;
				}
				ServerLocationsTreeList.PageIndex = 0;
				ServerLocationsTreeList.DataSource = (DataTable)Session["ServerLocations"];
				ServerLocationsTreeList.DataBind();
				ServerLocationsTreeList.ExpandAll();
			}
			catch (Exception ex)
			{
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}
			finally { }
		}


		public void FillServerLocationsTreeListfromSession()
		{
			DataTable DataServers = new DataTable();
			try
			{
				if (Session["ServerLocations"] != "" && Session["ServerLocations"] != null)
					DataServers = Session["ServerLocations"] as DataTable;
				if (DataServers.Rows.Count > 0)
				{
					DataServers.PrimaryKey = new DataColumn[] { DataServers.Columns["ServiceId"] };
				}
				ServerLocationsTreeList.DataSource = DataServers;
				ServerLocationsTreeList.DataBind();

			}
			catch (Exception ex)
			{
				//Log.Entry.Ins.Write(Server.MapPath("~/LogFiles/"), "VSPlusLog.txt", DateTime.Now.ToString() + " Error in Page: " +
				//    Request.Url.AbsolutePath + ", Method: " + System.Reflection.MethodBase.GetCurrentMethod().Name +
				//    ", Error: " + ex.ToString());
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}
		}

		protected void ServerLocationsApply_Click(object sender, EventArgs e)
		{
			bool update = false;
			LocationSuccess.Style.Value = "display: none;";
			LocationSuccess.InnerHtml = "";
			LocationError.Style.Value = "display: none;";
			LocationError.InnerHtml = "";
			string ServerErrors = "";
			string AppliedServers = "";
			string ServerType = "";
			DataTable dt = GetSelectedServersforLocationChange();
			if (dt.Rows.Count > 0 && ServerLocation.Text != "")
			{
				 locationid = Convert.ToInt32(dt.Rows[0]["LocationID"]);
				 locationddlid = Convert.ToInt32(ServerLocation.SelectedItem.Value);
				 ServerType = dt.Rows[0]["ServerType"].ToString();

				if (locationid == locationddlid)
				{
					LocationSuccess.Style.Value = "display: none;";
					LocationSuccess.InnerHtml = "";
					LocationError.Style.Value = "display: block;";
					//10/3/2014 NS modified for VSPLUS-990
					LocationError.InnerHtml = "Please select different location." +
						"<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
				}
			}
		
			List<DataRow> serversSelected = dt.AsEnumerable().ToList();

			if ((serversSelected.Count > 0 && ServerLocation.SelectedIndex != -1) && (locationid != locationddlid))
			{
				foreach (DataRow server in serversSelected)
				{
					try
					{
						update = VSWebBL.SecurityBL.ServersBL.Ins.UpdateServerLocation(Convert.ToInt32(server["ServerID"].ToString()), Convert.ToInt32(ServerLocation.SelectedItem.Value),(server["ServerType"].ToString()));
						if (!update)
						{
							if (ServerErrors == "")
							{
								ServerErrors += server["Name"].ToString();
							}
							else
							{
								if (ServerErrors.Contains(server["Name"].ToString()))
								{
								}
								else
								{
									ServerErrors += ", " + server["Name"].ToString();
								}
							}
						}
						else
						{
							if (AppliedServers == "")
							{
								AppliedServers += server["Name"].ToString();
							}
							else
							{
								if (AppliedServers.Contains(server["Name"].ToString()))
								{
								}
								else
								{
									AppliedServers += ", " + server["Name"].ToString();
								}
							}
						}
						fillServersTreeList();
						FillServerLocationsTreeList();
						//CY: Added these inorder to make sure the locations for servers are updated across all tabs
						fillDominoServersTreeList();
						FillDiskSettingsTreeList();
						FillExchangeServersTreeList();
						//fillServersTreeList();
						//FillExchangeRolesForExchangeTab();
						ServerLocationsTreeList.UnselectAll();
						DiskSettingsTreeList.UnselectAll();
						WindowsServerTreeList.UnselectAll();
						DominoServerTreeList.UnselectAll();
						ServersTreeList.UnselectAll();
						ServerCredentialsTreeList.UnselectAll();
						
						
					}
					catch (Exception ex)
					{
						Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
						throw ex;
					}
					finally { }
				}
			}
			else if (serversSelected.Count == 0 && ServerLocation.SelectedIndex == -1)
			{
				LocationError.Style.Value = "display: block;";
				//10/3/2014 NS modified for VSPLUS-990
				LocationError.InnerHtml = "Please select a location and at least one server." +
					"<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
			}
			if (serversSelected.Count > 0 && ServerLocation.SelectedIndex == -1)
			{
				LocationError.Style.Value = "display: block;";
				//10/3/2014 NS modified for VSPLUS-990
				LocationError.InnerHtml = "Please select a location." +
					"<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
			}
			if (serversSelected.Count == 0 && ServerLocation.SelectedIndex != -1)
			{
				LocationError.Style.Value = "display: block;";
				//10/3/2014 NS modified for VSPLUS-990
				LocationError.InnerHtml = "Please select at least one server." +
					"<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
			}
			if (ServerErrors != "")
			{
				LocationError.Style.Value = "display: block;";
				//10/3/2014 NS modified for VSPLUS-990
				LocationError.InnerHtml = "Location for the servers: " + ServerErrors + " was NOT updated to " + ServerLocation.SelectedItem.Text.ToString() + "." +
					"<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
			}
			if ((AppliedServers != "")&&(locationid!= locationddlid))
				
			{
				LocationSuccess.Style.Value = "display: block;";
				//10/3/2014 NS modified for VSPLUS-990
				LocationSuccess.InnerHtml = "Location for the servers: " + AppliedServers + " is updated to " + ServerLocation.SelectedItem.Text.ToString() + "." +
					"<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
				ServerLocation.Text = "";
				ServerLocation.SelectedIndex = -1;
			}
		}


		private DataTable GetSelectedServersforLocationChange()
		{

			DataTable dtSel = new DataTable();
			try
			{
				dtSel.Columns.Add("ServerID");
				dtSel.Columns.Add("Name");
				dtSel.Columns.Add("LocationID");
				dtSel.Columns.Add("ServerType");
				//string selValues = "";
				TreeListNodeIterator iterator = ServerLocationsTreeList.CreateNodeIterator();
				TreeListNode node;

				while (true)
				{
					node = iterator.GetNext();

					if (node == null) break;
					//if ((node.ChildNodes.Count > 0))
					//{
						if (node.Level == 2) //(node.ParentNode.Selected==false)
						{
							if (node.Selected)
							{
								DataRow dr = dtSel.NewRow();
								dr["ServerID"] = node.GetValue("actid");
								dr["Name"] = node.GetValue("Name");
								dr["LocationID"] = node.GetValue("locId");
								dr["ServerType"] = node.GetValue("ServerType");
								dtSel.Rows.Add(dr);
							}
						}
					//}
					//else
					//{
					//    ServersTreeList.DataSource = null;
					//    ServersTreeList.DataBind();
					//}

				}
			}
			catch (Exception ex)
			{
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}

			return dtSel;

		}

		//private void FillExchangeRolesForExchangeTab()
		//{
		//    DataTable ServerDataTable = VSWebBL.SecurityBL.ServerTypesBL.Ins.GetExchangeRoles();
		//    ExchangeRoles.DataSource = ServerDataTable;
		//    ExchangeRoles.TextField = "RoleName";
		//    ExchangeRoles.ValueField = "RoleName";
		//    ExchangeRoles.DataBind();
		//}




		protected void CollapseAllButton_ServerCredentials_Click(object sender, EventArgs e)
		{
			try
			{
				if (CredentialsCollapseAll.Text == "Collapse All")
				{
					ServerCredentialsTreeList.CollapseAll();
					CredentialsCollapseAll.Image.Url = "~/images/icons/add.png";
					CredentialsCollapseAll.Text = "Expand All";
				}
				else
				{
					ServerCredentialsTreeList.ExpandAll();
					CredentialsCollapseAll.Image.Url = "~/images/icons/forbidden.png";
					CredentialsCollapseAll.Text = "Collapse All";
				}
			}
			catch (Exception ex)
			{
				//Log.Entry.Ins.Write(Server.MapPath("~/LogFiles/"), "VSPlusLog.txt", DateTime.Now.ToString() + " Error in Page: " +
				//    Request.Url.AbsolutePath + ", Method: " + System.Reflection.MethodBase.GetCurrentMethod().Name +
				//    ", Error: " + ex.ToString());
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}
		}


		protected void ServerCredentialsApply_Click(object sender, EventArgs e)
		{
			bool update = false;
			CredentialsSuccess.Style.Value = "display: none;";
			CredentialsSuccess.InnerHtml = "";
			CredentialsError.Style.Value = "display: none;";
			CredentialsError.InnerHtml = "";
			string ServerErrors = "";
			string AppliedServers = "";
			DataTable dt = GetSelectedServersforCredentialsChange();
			List<DataRow> serversSelected = dt.AsEnumerable().ToList();

			if (serversSelected.Count > 0 && ServerCredentials.SelectedIndex != -1)
			{
				foreach (DataRow server in serversSelected)
				{
					try
					{
						update = VSWebBL.SecurityBL.ServersBL.Ins.UpdateServerCredentials(Convert.ToInt32(server["ServerID"].ToString()), Convert.ToInt32(ServerCredentials.SelectedItem.Value), server["ServerType"].ToString());
						if (!update)
						{
							if (ServerErrors == "")
							{
								ServerErrors += server["Name"].ToString();
							}
							else
							{
								if (ServerErrors.Contains(server["Name"].ToString()))
								{
								}
								else
								{
									ServerErrors += ", " + server["Name"].ToString();
								}
							}


						}
						else
						{
							if (AppliedServers == "")
							{
								AppliedServers += server["Name"].ToString();
							}
							else
							{
								if (AppliedServers.Contains(server["Name"].ToString()))
								{
								}
								else
								{
									AppliedServers += ", " + server["Name"].ToString();
								}
							}
						}
						FillServerLocationsTreeList();
						//CY: Added these inorder to make sure the locations for servers are updated across all tabs
						fillDominoServersTreeList();
						FillDiskSettingsTreeList();
						FillExchangeServersTreeList();
						fillServersTreeList();
						//FillExchangeRolesForExchangeTab();
						ServerLocationsTreeList.UnselectAll();
						DiskSettingsTreeList.UnselectAll();
						WindowsServerTreeList.UnselectAll();
						DominoServerTreeList.UnselectAll();
						ServersTreeList.UnselectAll();
						ServerCredentialsTreeList.UnselectAll();
					}
					catch (Exception ex)
					{
						Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
						throw ex;
					}
					finally { }
				}
			}
			else
			{
				CredentialsError.Style.Value = "display: block;";
				//10/3/2014 NS modified for VSPLUS-990
				CredentialsError.InnerHtml = "Please select a credential and at least one server." +
					"<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
			}
			if (ServerErrors != "")
			{
				CredentialsError.Style.Value = "display: block;";
				//10/3/2014 NS modified for VSPLUS-990
				CredentialsError.InnerHtml = "Credentials for the servers: " + ServerErrors + " was NOT updated to " + ServerCredentials.SelectedItem.Text.ToString() + "." +
					"<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
			}
			if (AppliedServers != "")
			{
				CredentialsSuccess.Style.Value = "display: block;";
				//10/3/2014 NS modified for VSPLUS-990
				CredentialsSuccess.InnerHtml = "Credentials for the servers: " + AppliedServers + " is updated to " + ServerCredentials.SelectedItem.Text.ToString() + "." +
					"<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
			}
		}

		private DataTable GetSelectedServersforCredentialsChange()
		{

			DataTable dtSel = new DataTable();
			try
			{
				dtSel.Columns.Add("ServerID");
				dtSel.Columns.Add("Name");
				dtSel.Columns.Add("ServerType");

				//string selValues = "";
				TreeListNodeIterator iterator = ServerCredentialsTreeList.CreateNodeIterator();
				TreeListNode node;

				while (true)
				{
					node = iterator.GetNext();

					if (node == null) break;
					if (node.Level == 2) //(node.ParentNode.Selected==false)
					{
						if (node.Selected)
						{
							DataRow dr = dtSel.NewRow();
							dr["ServerID"] = node.GetValue("actid");
							dr["Name"] = node.GetValue("Name");
							dr["ServerType"] = node.GetValue("ServerType");
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

		public void FillServerCredentialsTreeList()
		{
			try
			{
				Session["ServerCredentials"] = null;
				if (Session["ServerCredentials"] == null)
				{
                    DataTable tblFiltered = new DataTable();
                    DataTable DataServersTree = new DataTable();
					//string st = CredentialType.Text.ToString();

					//if (st == "Exchange Active Sync Credentials")
					//    st = "Exchange";
					//else
					//    st = "";

					//DataServersTree = VSWebBL.ConfiguratorBL.AlertsBL.Ins.GetServersCredentialsFromProcedure(ServerTypeFilter);
                    tblFiltered = (DataTable)Session["IntialAllservers"];
                    tblFiltered = filterTable(tblFiltered);
                   
                    DataServersTree = tblFiltered.AsEnumerable()
                             .Where(r => r.Field<string>("ServerType") != "WebSphere")
                               .Where(r => r.Field<string>("ServerType") != "Sametime")
                             .CopyToDataTable();
					Session["ServerCredentials"] = DataServersTree;
				}
				ServerCredentialsTreeList.DataSource = (DataTable)Session["ServerCredentials"];
				ServerCredentialsTreeList.DataBind();
				ServerCredentialsTreeList.ExpandAll();
			}
			catch (Exception ex)
			{
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}
			finally { }
		}

		public void FillServerCredentialsTreeListfromSession()
		{
			DataTable DataServers = new DataTable();
			try
			{
				if (Session["ServerCredentials"] != "" && Session["ServerCredentials"] != null)
					DataServers = Session["ServerCredentials"] as DataTable;
				if (DataServers.Rows.Count > 0)
				{
					DataServers.PrimaryKey = new DataColumn[] { DataServers.Columns["ServiceId"] };
				}
				ServerCredentialsTreeList.DataSource = DataServers;
				ServerCredentialsTreeList.DataBind();

			}
			catch (Exception ex)
			{
				//Log.Entry.Ins.Write(Server.MapPath("~/LogFiles/"), "VSPlusLog.txt", DateTime.Now.ToString() + " Error in Page: " +
				//    Request.Url.AbsolutePath + ", Method: " + System.Reflection.MethodBase.GetCurrentMethod().Name +
				//    ", Error: " + ex.ToString());
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}
		}

		private void FillServerCredentials()
        {
            DataTable FilterdCredsdt = new DataTable();
            // 8/7/2016 Durga Addded for VSPLUS-2877
            DataTable CredentialsDataTable = VSWebBL.ConfiguratorBL.ServicesBL.Ins.GetCredentialsForSSE();

            ServerCredentials.DataSource = CredentialsDataTable;
			ServerCredentials.TextField = "AliasName";
			ServerCredentials.ValueField = "ID";
			ServerCredentials.DataBind();


		}

		private DataTable filterTable(DataTable DataServersTree)
		{
			if (DataServersTree.Rows.Count > 0)
			{
				if (Session["RestrictedServers"] != "" && Session["RestrictedServers"] != null)
				{
					List<int> ServerID = new List<int>();
					List<int> LocationID = new List<int>();
					DataTable resServers = (DataTable)Session["RestrictedServers"];
					foreach (DataRow resser in resServers.Rows)
					{
						foreach (DataRow dominorow in DataServersTree.Rows)
						{

							if (resser["serverid"].ToString() == dominorow["actid"].ToString())
							{
								ServerID.Add(DataServersTree.Rows.IndexOf(dominorow));
							}
							if (dominorow["locid"] != null && dominorow["locid"].ToString() != "" && (resser["locationID"].ToString() == dominorow["locid"].ToString()))
							{
								LocationID.Add(Convert.ToInt32(dominorow["locid"].ToString()));
							}
						}

					}
					foreach (int Id in ServerID)
					{
						DataServersTree.Rows[Id].Delete();
					}
					DataServersTree.AcceptChanges();

					foreach (int lid in LocationID)
					{
						DataRow[] row = DataServersTree.Select("locid=" + lid + "");
						for (int i = 0; i < row.Count(); i++)
						{
							DataServersTree.Rows.Remove(row[i]);
							DataServersTree.AcceptChanges();
						}
					}
					DataServersTree.AcceptChanges();

				}
			}

			return DataServersTree;
		}

		protected void CredentialType_SelectedIndexChanged(object sender, EventArgs e)
		{
			FillServerCredentialsTreeList();
		}

		public void FillBusinessHoursTreeList()
		{
			try
			{
				Session["BusinessHours"] = null;
				if (Session["BusinessHours"] == null)
				{
					DataTable DataServersTree = new DataTable();
					string Page = "ServerSettingsEditor.aspx", Control = "BusinessHoursTreeList";
					//DataServersTree = VSWebBL.ConfiguratorBL.AlertsBL.Ins.GetBUsinessHoursFromProcedure(Page, Control);
					DataServersTree = (DataTable)Session["IntialAllservers"];
					DataServersTree = filterTable(DataServersTree);
					Session["BusinessHours"] = DataServersTree;
				}
				BusinessHoursTreeList.DataSource = (DataTable)Session["BusinessHours"];
				BusinessHoursTreeList.DataBind();
				BusinessHoursTreeList.ExpandAll();
			}
			catch (Exception ex)
			{
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}
			finally { }
		}
		private void FillBusinesshoursComboBox()
		{
			DataTable Businesshoursname = VSWebBL.ConfiguratorBL.Businesshoursbl.Ins.GetBusinesshoursNames();

			BusinessHoursComboBox.DataSource = Businesshoursname;
			BusinessHoursComboBox.TextField = "Type";
			BusinessHoursComboBox.ValueField = "ID";
			BusinessHoursComboBox.DataBind();


		}
		public void FillBusinessHoursTreeListfromSession()
		{
			DataTable DataServers = new DataTable();
			try
			{
				if (Session["BusinessHours"] != "" && Session["BusinessHours"] != null)
					DataServers = Session["BusinessHours"] as DataTable;
				if (DataServers.Rows.Count > 0)
				{
					DataServers.PrimaryKey = new DataColumn[] { DataServers.Columns["ServiceId"] };
				}
				BusinessHoursTreeList.DataSource = DataServers;
				BusinessHoursTreeList.DataBind();

			}
			catch (Exception ex)
			{
				//Log.Entry.Ins.Write(Server.MapPath("~/LogFiles/"), "VSPlusLog.txt", DateTime.Now.ToString() + " Error in Page: " +
				//    Request.Url.AbsolutePath + ", Method: " + System.Reflection.MethodBase.GetCurrentMethod().Name +
				//    ", Error: " + ex.ToString());
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}
		}
		protected void BusinessHoursApply_Click(object sender, EventArgs e)
		{
			bool update = false;
			BusinessHoursSuccess.Style.Value = "display: none;";
			BusinessHoursSuccess.InnerHtml = "";
			BusinessHoursError.Style.Value = "display: none;";
			BusinessHoursError.InnerHtml = "";
			string ServerErrors = "";
			string AppliedServers = "";
			DataTable dt = GetSelectedServersforBusinessHoursChange();
			List<DataRow> serversSelected = dt.AsEnumerable().ToList();

			if (serversSelected.Count > 0 && BusinessHoursComboBox.SelectedIndex != -1)
			{
				foreach (DataRow server in serversSelected)
				{
					try
					{
						update = VSWebBL.SecurityBL.ServersBL.Ins.UpdateServerBusinessHours(Convert.ToInt32(server["ServerID"].ToString()), Convert.ToInt32(BusinessHoursComboBox.SelectedItem.Value));
						if (!update)
						{
							if (ServerErrors == "")
							{
								ServerErrors += server["Name"].ToString();
							}
							else
							{
								if (ServerErrors.Contains(server["Name"].ToString()))
								{
								}
								else
								{
									ServerErrors += ", " + server["Name"].ToString();
								}
							}
						}
						else
						{
							if (AppliedServers == "")
							{
								AppliedServers += server["Name"].ToString();
							}
							else
							{
								if (AppliedServers.Contains(server["Name"].ToString()))
								{
								}
								else
								{
									AppliedServers += ", " + server["Name"].ToString();
								}
							}
						}
						FillServerLocationsTreeList();
						//CY: Added these inorder to make sure the locations for servers are updated across all tabs
						fillDominoServersTreeList();
						FillDiskSettingsTreeList();
						FillExchangeServersTreeList();
						fillServersTreeList();
						FillBusinessHoursTreeList();
						//FillExchangeRolesForExchangeTab();
						BusinessHoursTreeList.UnselectAll();
						ServerLocationsTreeList.UnselectAll();
						DiskSettingsTreeList.UnselectAll();
						WindowsServerTreeList.UnselectAll();
						DominoServerTreeList.UnselectAll();
						ServersTreeList.UnselectAll();
						ServerCredentialsTreeList.UnselectAll();
					}
					catch (Exception ex)
					{
						Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
						throw ex;
					}
					finally { }
				}
			}
			else
			{
				BusinessHoursError.Style.Value = "display: block;";
				//10/3/2014 NS modified for VSPLUS-990
				BusinessHoursError.InnerHtml = "Please select an Hours definition and at least one server." +
					"<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
			}
			if (ServerErrors != "")
			{
				BusinessHoursError.Style.Value = "display: block;";
				//10/3/2014 NS modified for VSPLUS-990
				BusinessHoursError.InnerHtml = "Hours for the servers: " + ServerErrors + " was NOT updated to " + BusinessHoursComboBox.SelectedItem.Text.ToString() + "." +
					"<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
			}
			if (AppliedServers != "")
			{
				BusinessHoursSuccess.Style.Value = "display: block;";
				//10/3/2014 NS modified for VSPLUS-990
				BusinessHoursSuccess.InnerHtml = "Hours for the servers: " + AppliedServers + " is updated to " + BusinessHoursComboBox.SelectedItem.Text.ToString() + "." +
					"<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
			}
		}
		private DataTable GetSelectedServersforBusinessHoursChange()
		{

			DataTable dtSel = new DataTable();
			try
			{
				dtSel.Columns.Add("ServerID");
				dtSel.Columns.Add("Name");

				//string selValues = "";
				TreeListNodeIterator iterator = BusinessHoursTreeList.CreateNodeIterator();
				TreeListNode node;

				while (true)
				{
					node = iterator.GetNext();

					if (node == null) break;
					if (node.Level == 2) //(node.ParentNode.Selected==false)
					{
						if (node.Selected)
						{
							DataRow dr = dtSel.NewRow();
							dr["ServerID"] = node.GetValue("actid");
							dr["Name"] = node.GetValue("Name");
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
        protected void DataBound(object sender, EventArgs e)
        {
            SetItemCount();

        }
        void SetItemCount()
        {


            int itemCount = ServersTreeList.Nodes.OfType<TreeListNode>().Select(x => x.ChildNodes.Count).Sum();      // int itemCount = (int)ServersTreeList.GetSummaryValue()

            ServersTreeList.SettingsPager.Summary.Text = "Page {0} of {1} (" + itemCount + " items)";
        }

	}
}