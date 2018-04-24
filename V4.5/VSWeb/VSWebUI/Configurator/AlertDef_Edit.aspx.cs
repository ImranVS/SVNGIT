using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxTreeList;
using System.Data;

using VSFramework;
using VSWebBL;
using VSWebDO;
using DevExpress.Web;
using System.Collections;
using DevExpress.Web.Data;
using System.Web.UI.HtmlControls;

//using Log;
namespace VSWebUI.Configurator
{
	public partial class AlertDef_Edit : System.Web.UI.Page
	{
		int profileid;
		string tempname;
		int profileid3;
		int var1;
		int var2;
		DataTable dtSeltemp;
		string lastid;
		DataTable dtSel;
		DataTable DataEventsTree2;
		DataTable DataEventsTree;
		DataTable Htable = null;
		//5/13/2015 NS added for VSPLUS-1763
		DataTable Etable = null;
		string flag, day;
		int AlertKey;
		bool sun, mon, tue, wed, thu, fri, sat, listofdays;
		string profilename;
		public string SMSFromNumber = "";
		public string eSMSFromNumber = "";
		//7/1/2015 NS added for VSPLUS-1894
		//bool SMSConfig = false;
		public class CustomExceptions : Exception
		{
			//  public MyException(string message)
			// : base(message) { }

			public CustomExceptions(string message)
				: base(message) { }

			//  public CustomExceptions(string message)
			//  : base(message) { }


		}
		protected void Page_Load(object sender, EventArgs e)
		{
			try
			{

				if (!IsPostBack)
				{

					//int i = 0;
					//int k = 0;
					//Session["firsttempid"] = null;
					Session["DataEvents2"] = null;
					Session["DataEvents3"] = null;
					Session["DataServers"] = null;
					if (Session["UserPreferences"] != null)
					{
						DataTable UserPreferences = (DataTable)Session["UserPreferences"];
						foreach (DataRow dr in UserPreferences.Rows)
						{
							if (dr[1].ToString() == "AlertDef_Edit|HoursGridView")
							{
								HoursGridView.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
							}
							if (dr[1].ToString() == "AlertDef_Edit|EventsGridView")
							{
								EventsTreeList.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
							}
							if (dr[1].ToString() == "AlertDef_Edit|ServersGridView")
							{
								ServersTreeList.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
							}
						}
					}

				}


				if (Request.QueryString["AlertKey"] != "" && Request.QueryString["AlertKey"] != null)
				{
					flag = "Update";
					AlertKey = int.Parse(Request.QueryString["AlertKey"]);

					if (!IsPostBack)
					{

						fillData(AlertKey);
						//fillEventsTreeList();
					}
				}
				else
				{
					flag = "Insert";
				}


				if (!IsPostBack)
				{

					FillAlerteventsComboBox();
					FillHoursGrid();
					//4/2/2015 NS added for VSPLUS-219
					FillEscalationGrid();

					fillEventsTreeList();

					fillServersTreeList();
					ServersTreeList.CollapseAll();
					EventsTreeList.CollapseAll();
					//EventsTreeList2.CollapseAll();
					//7/1/2015 NS added for VSPLUS-1894
					Session["SMSConfig"] = isSMSConfigured();
				}
				else
				{
					//tempname = AlerteventsComboBox.Text;
					FillAlerteventsComboBoxfromsession();

					fillHoursGridfromSession();
					//4/2/2015 NS added for VSPLUS-219
					fillEscalationGridfromSession();

					fillEventsTreefromSession();

					fillServersTreefromSession();
					ServersTreeList.CollapseAll();
					EventsTreeList.CollapseAll();

				}
			}
			catch (Exception ex)
			{
				//Log.Entry.Ins.Write(Server.MapPath("~/LogFiles/"), "VSPlusLog.txt", DateTime.Now.ToString() + " Error in Page: " +
				//    Request.Url.AbsolutePath + ", Method: " + System.Reflection.MethodBase.GetCurrentMethod().Name +
				//    ", Error: " + ex.ToString());

				//6/27/2014 NS added for VSPLUS-634
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
			}
			finally { }

		}
		public void fillData(int Key)
		{
			try
			{
				AlertNames AlertObj = new AlertNames();
				AlertObj.AlertKey = Key;
				AlertNames Returnobj = VSWebBL.ConfiguratorBL.AlertsBL.Ins.GetAlertNamebyKey(AlertObj);

				AlertNameTextBox.Text = Returnobj.AlertName;
				//AlerteventsComboBox.SelectedItem.Value = Returnobj.Templateid;
				if (Returnobj.Templateid != null && Returnobj.Templateid != 0)
				{

					DataTable templatenames = VSWebBL.ConfiguratorBL.AlertsBL.Ins.GetEventsTemplateNames(Returnobj.Templateid);
					if (AlerteventsComboBox.SelectedIndex == -1)
					{
						Session["firsttempid"] = Returnobj.Templateid;
					}
					else
					{
						Session["firsttempid"] = null;
					}
					string templatename = templatenames.Rows[0]["Name"].ToString();

					AlerteventsComboBox.Text = templatename;
				}
				else
				{

					AlerteventsComboBox.Text = "None";

				}

				//AlerteventsComboBox.Items.FindByText(templatename.ToString()).Selected = true;
			}
			catch (Exception ex)
			{
				//Log.Entry.Ins.Write(Server.MapPath("~/LogFiles/"), "VSPlusLog.txt", DateTime.Now.ToString() + " Error in Page: " +
				//    Request.Url.AbsolutePath + ", Method: " + System.Reflection.MethodBase.GetCurrentMethod().Name +
				//    ", Error: " + ex.ToString());

				//6/27/2014 NS added for VSPLUS-634
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
			}
		}
		public void FillHoursGrid()
		{
			try
			{
				AlertNames AlertNameobj = new AlertNames();
				AlertNameobj.AlertName = AlertNameTextBox.Text;
				DataTable Hourstable = new DataTable();
				Hourstable = VSWebBL.ConfiguratorBL.AlertsBL.Ins.GetAlertDetails(AlertNameobj);
				if (Hourstable.Rows.Count > 0)
				{
					Hourstable.PrimaryKey = new DataColumn[] { Hourstable.Columns["ID"] };
					
				}
				Session["AlertDetails"] = Hourstable;
				HoursGridView.DataSource = Hourstable;
				HoursGridView.DataBind();
			}
			catch (Exception ex)
			{
				//Log.Entry.Ins.Write(Server.MapPath("~/LogFiles/"), "VSPlusLog.txt", DateTime.Now.ToString() + " Error in Page: " +
				//    Request.Url.AbsolutePath + ", Method: " + System.Reflection.MethodBase.GetCurrentMethod().Name +
				//    ", Error: " + ex.ToString());

				//6/27/2014 NS added for VSPLUS-634
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
			}
		}
		public void fillHoursGridfromSession()
		{
			DataTable Hourstable = new DataTable();
			try
			{
				if (Session["AlertDetails"] != "" && Session["AlertDetails"] != null)
					Hourstable = Session["AlertDetails"] as DataTable;
				if (Hourstable.Rows.Count > 0)
				{
					Hourstable.PrimaryKey = new DataColumn[] { Hourstable.Columns["ID"] };
				}
				HoursGridView.DataSource = Hourstable;
				HoursGridView.DataBind();
			}
			catch (Exception ex)
			{
				//Log.Entry.Ins.Write(Server.MapPath("~/LogFiles/"), "VSPlusLog.txt", DateTime.Now.ToString() + " Error in Page: " +
				//    Request.Url.AbsolutePath + ", Method: " + System.Reflection.MethodBase.GetCurrentMethod().Name +
				//    ", Error: " + ex.ToString());

				//6/27/2014 NS added for VSPLUS-634
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
			}
		}
		public void fillEventsTreeList()
		{

			int j = 0;
			evet1.Visible = true;

			//1669-alertprorofiles--somaraj

			try
			{
				DataTable filteredData = null;
				Session["DataEvents2"] = null;
				EventsTreeList.CollapseAll();
				CollapseAllButton.Image.Url = "~/images/icons/add.png";
				CollapseAllButton.Text = "Expand All";
				if (Session["DataEvents2"] == null)
				{
					if (flag == "Update" && AlerteventsComboBox.Text == "None")
					{
						tempname = "None";
						profileid3 = 0;
						dtSel = VSWebBL.ConfiguratorBL.AlertsBL.Ins.GetSelectedEvents(AlertKey);
						Session["profileid3"] = profileid3;
					}
					else if (flag == "Insert" && AlerteventsComboBox.Text == "")
					{
						EventsTreelist();
						//tempname = "None";
						AlerteventsComboBox.Text = "None";
						profileid3 = 0;
						//dtSel = VSWebBL.ConfiguratorBL.AlertsBL.Ins.GetSelectedEvents(AlertKey);
						//Session["profileid3"] = profileid3;
					}
					else if (flag == "Update" && AlerteventsComboBox.Text != "None")
					{
						tempname = AlerteventsComboBox.Text;
						Session["profilename2"] = tempname;
						DataTable templateids = VSWebBL.ConfiguratorBL.AlertsBL.Ins.GetEventsTemplateidbyNames(tempname);
						profileid3 = Convert.ToInt32(templateids.Rows[0]["ID"]);
						Session["profileid3"] = profileid3;
					}
					else if (flag == "Insert" && AlerteventsComboBox.Text != "None")
					{
						tempname = AlerteventsComboBox.Text;
						Session["profilename2"] = tempname;
						DataTable templateids = VSWebBL.ConfiguratorBL.AlertsBL.Ins.GetEventsTemplateidbyNames(tempname);
						profileid3 = Convert.ToInt32(templateids.Rows[0]["ID"]);
						Session["profileid3"] = profileid3;
					}


					if (profileid3 != null)
					{
						dtSeltemp = VSWebBL.ConfiguratorBL.AlertsBL.Ins.GetSelectedEventsfortemplate2(profileid3);
					}
					if (dtSeltemp.Rows.Count > 0)
					{
						for (j = 0; j < dtSeltemp.Rows.Count; j++)
						{
							string eventsid = dtSeltemp.Rows[j]["ServerTypeID"].ToString();
							lastid += eventsid + ",";

						}

						while (lastid.EndsWith(","))
							lastid = lastid.Substring(0, lastid.Length - 1);
						Session["lastid"] = lastid;
					}
					if ((flag == "Insert") && (tempname == "" || tempname == null || tempname == String.Empty) && (AlerteventsComboBox.Text == "None"))
					{
						EventsTreelist();
						
					}
					else if ((flag == "Insert") && (Session["profilename"] == "None"))
					{
						EventsTreelist();
					}
					else if ((flag == "Update") && (AlerteventsComboBox.Text == "None"))
					{
						EventsTreelist();
					}
					else if (Session["lastid"] != null)
					{
						//changed by somaraju for template wise showing all events
						//DataEventsTree = VSWebBL.ConfiguratorBL.AlertsBL.Ins.GetEventsFromProcedurewithtemplate(Session["lastid"].ToString());
						EventsTreelist();
					}

					//EventsTreeList.FocusedNode.Selected = true;
					EventsTreeList.DataSource = (DataTable)Session["DataEvents2"];
					EventsTreeList.DataBind();

				}

				if (flag == "Insert" && Session["profileid"] != null && Session["profilename"] != null)
				{
					dtSel = VSWebBL.ConfiguratorBL.AlertsBL.Ins.GetSelectedEventsfortemplate2(profileid3);

				}

				else if (flag == "Insert" && Session["profilename"] == null && Session["profilename"] == null)
				{
					if (AlertKey == null || AlertKey == 0)
					{
						//dtSel = VSWebBL.ConfiguratorBL.AlertsBL.Ins.GetSelectedEvents(AlertKey);
						dtSel = VSWebBL.ConfiguratorBL.AlertsBL.Ins.GetSelectedEventsfortemplate2(profileid3);

					}


				}
				else if ((flag == "Update") && (AlerteventsComboBox.Text != null) && (Session["profilename"] != null))
				{
					if (Session["firsttempid"] == null)
					{
						Session["firsttempid"] = 0;
					}
					if ((Session["firsttempid"].ToString() != null) || (Session["profileid"].ToString() != null))
					//if ((Session["firsttempid"].ToString() != null && Session["firsttempid"].ToString() != "0") && (Session["profileid"].ToString() != null && Session["profileid"].ToString() != "0"))
					{

						if (Session["firsttempid"].ToString() == Session["profileid"].ToString())
						{
							dtSel = VSWebBL.ConfiguratorBL.AlertsBL.Ins.GetSelectedEvents(AlertKey);
						}

						else if (Session["profilename"].ToString() != "None")
						{
							DataTable datawithkey = new DataTable();
							DataTable datawithtemplate = new DataTable();
							datawithkey = VSWebBL.ConfiguratorBL.AlertsBL.Ins.GetSelectedEvents(AlertKey);
							datawithtemplate = VSWebBL.ConfiguratorBL.AlertsBL.Ins.GetSelectedEventsfortemplate2(profileid3);


							dtSel = datawithkey.Copy();
							dtSel.Merge(datawithtemplate);


						}


					}
				}
				//else if (flag == "Update" && Session["profilename"].ToString() == null)
				else if (flag == "Update" && AlerteventsComboBox.SelectedIndex == -1)
				{
					if (AlertKey != null || AlertKey != 0)
					{
						//if (Session["profileid"] != Session["profileid3"])
						//{
						// dtSel = VSWebBL.ConfiguratorBL.AlertsBL.Ins.GetSelectedEventsfortemplate2(profileid3);
						//}
						//else
						//{
						dtSel = VSWebBL.ConfiguratorBL.AlertsBL.Ins.GetSelectedEvents(AlertKey);
						//}

						//dtSel = VSWebBL.ConfiguratorBL.AlertsBL.Ins.GetSelectedEvents(AlertKey);

					}


				}


				if (dtSel.Rows.Count > 0)
				{
					//EventsTreeList.FocusedNode.Selected = false;

					TreeListNodeIterator iterator = EventsTreeList.CreateNodeIterator();
					TreeListNode node;


					for (int i = 0; i < dtSel.Rows.Count; i++)
					{
						if (Convert.ToInt32(dtSel.Rows[i]["EventID"]) == 0 && Convert.ToInt32(dtSel.Rows[i]["ServerTypeID"]) == 0)
						{
							//select all
							while (true)
							{
								node = iterator.GetNext();
								if (node == null) break;
								node.Selected = true;
							}
						}
						else if (Convert.ToInt32(dtSel.Rows[i]["EventID"]) == 0 && (Convert.ToInt32(dtSel.Rows[i]["ServerTypeID"]) != 0))
						{
							//parent selected
							while (true)
							{
								node = iterator.GetNext();
								if (node == null) break;
								if ((Convert.ToInt32(dtSel.Rows[i]["ServerTypeID"]) == Convert.ToInt32(node.GetValue("actid"))) && node.GetValue("tbl").ToString() == "ServerTypes")
								{
									node.Selected = true;
								}
								else if (node.GetValue("SrvId").ToString() != "")
								{
									if ((Convert.ToInt32(dtSel.Rows[i]["ServerTypeID"]) == Convert.ToInt32(node.GetValue("SrvId"))) && node.GetValue("tbl").ToString() != "ServerTypes")
									{
										node.Selected = true;
									}
								}
							}
						}
						else if (Convert.ToInt32(dtSel.Rows[i]["EventID"]) != 0 && (Convert.ToInt32(dtSel.Rows[i]["ServerTypeID"]) != 0))
						//if (Convert.ToInt32(dtSel.Rows[i]["EventID"]) != 0)
						{
							//specific selected
							while (true)
							{

								node = iterator.GetNext();
								if (node == null) break;

								if ((Convert.ToInt32(dtSel.Rows[i]["EventID"]) == Convert.ToInt32(node.GetValue("actid"))) && node.GetValue("tbl").ToString() != "ServerTypes")
								{
									node.Selected = true;
								}

							}
						}
						iterator.Reset();
					}
				}

			}

			catch (Exception ex)
			{


				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
			}
		}
		public void fillEventsTreefromSession()
		{
			evet1.Visible = true;
			//evtdrpdwn.Visible = false;
			DataTable DataEvents = new DataTable();
			//if (AlerteventsComboBox.SelectedIndex >= 0)
			//{
			//    EventsTreeList.UnselectAll();

			//}
			Session["DataEvents3"] = null;
			try
			{
				if (Session["DataEvents2"] != "" && Session["DataEvents2"] != null)
					DataEvents = (DataTable)Session["DataEvents2"];
				if (DataEvents.Rows.Count > 0)
				{
					DataEvents.PrimaryKey = new DataColumn[] { DataEvents.Columns["ID"] };
				}
				EventsTreeList.DataSource = DataEvents;
				Session["changeddata"] = DataEvents;
				EventsTreeList.DataBind();

			}
			catch (Exception ex)
			{
				//Log.Entry.Ins.Write(Server.MapPath("~/LogFiles/"), "VSPlusLog.txt", DateTime.Now.ToString() + " Error in Page: " +
				//    Request.Url.AbsolutePath + ", Method: " + System.Reflection.MethodBase.GetCurrentMethod().Name +
				//    ", Error: " + ex.ToString());

				//6/27/2014 NS added for VSPLUS-634
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
			}
		}
		public void fillServersTreeList()
		{
			try
			{
				ServersTreeList.CollapseAll();
				CollapseAllSrvButton.Image.Url = "~/images/icons/add.png";
				CollapseAllSrvButton.Text = "Expand All";
				if (Session["DataServers"] == null)
				{
					DataTable DataServersTree = VSWebBL.ConfiguratorBL.AlertsBL.Ins.GetServersFromProcedure();
					Session["DataServers"] = DataServersTree;
				}
				ServersTreeList.DataSource = (DataTable)Session["DataServers"];
				ServersTreeList.DataBind();

				DataTable dtSel = VSWebBL.ConfiguratorBL.AlertsBL.Ins.GetSelectedServers(AlertKey);
				if (dtSel.Rows.Count > 0)
				{
					TreeListNodeIterator iterator = ServersTreeList.CreateNodeIterator();
					TreeListNode node;
					for (int i = 0; i < dtSel.Rows.Count; i++)
					{
						if (Convert.ToInt32(dtSel.Rows[i]["ServerID"]) == 0 && Convert.ToInt32(dtSel.Rows[i]["LocationID"]) == 0)
						{
							//select all
							while (true)
							{
								node = iterator.GetNext();
								if (node == null) break;
								node.Selected = true;
							}
						}
						else if (Convert.ToInt32(dtSel.Rows[i]["ServerID"]) == 0 && (Convert.ToInt32(dtSel.Rows[i]["LocationID"]) != 0))
						{
							//parent selected
							while (true)
							{
								node = iterator.GetNext();
								if (node == null) break;
								if ((Convert.ToInt32(dtSel.Rows[i]["LocationID"]) == Convert.ToInt32(node.GetValue("actid"))) && node.GetValue("tbl").ToString() == "Locations")
								{
									node.Selected = true;
								}
								else if (node.GetValue("LocId").ToString() != "")
								{
									if ((Convert.ToInt32(dtSel.Rows[i]["LocationID"]) == Convert.ToInt32(node.GetValue("LocId"))) && node.GetValue("tbl").ToString() != "Locations")
									{
										node.Selected = true;
									}
								}
							}
						}
						else if (Convert.ToInt32(dtSel.Rows[i]["ServerID"]) != 0 && (Convert.ToInt32(dtSel.Rows[i]["LocationID"]) != 0))
						{
							//specific selected
							while (true)
							{

								node = iterator.GetNext();
								if (node == null) break;
								//11/25/2013 NS modified - selection is loaded incorrectly when servers and URLs are selected
								if ((Convert.ToInt32(dtSel.Rows[i]["ServerID"]) == Convert.ToInt32(node.GetValue("actid"))) &&
									node.GetValue("tbl").ToString() != "Locations")
								{
									if (node.GetValue("LocId") != null)
									{
                                        //22/7/2016 Durga Modified for VSPLUS-3125
                                        if ((Convert.ToInt32(dtSel.Rows[i]["LocationID"]) == Convert.ToInt32(node.GetValue("LocId"))) && (Convert.ToInt32(dtSel.Rows[i]["ServerTypeID"]) == Convert.ToInt32(node.GetValue("srvtypeid"))))
										{
											node.Selected = true;
										}
									}
								}

							}
						}
						iterator.Reset();
					}
				}
			}
			catch (Exception ex)
			{
				//Log.Entry.Ins.Write(Server.MapPath("~/LogFiles/"), "VSPlusLog.txt", DateTime.Now.ToString() + " Error in Page: " +
				//    Request.Url.AbsolutePath + ", Method: " + System.Reflection.MethodBase.GetCurrentMethod().Name +
				//    ", Error: " + ex.ToString());

				//6/27/2014 NS added for VSPLUS-634
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
			}
		}
		public void fillServersTreefromSession()
		{

			DataTable DataServers = new DataTable();
			try
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
			catch (Exception ex)
			{
				//Log.Entry.Ins.Write(Server.MapPath("~/LogFiles/"), "VSPlusLog.txt", DateTime.Now.ToString() + " Error in Page: " +
				//    Request.Url.AbsolutePath + ", Method: " + System.Reflection.MethodBase.GetCurrentMethod().Name +
				//    ", Error: " + ex.ToString());

				//6/27/2014 NS added for VSPLUS-634
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
			}
		}
		private DataTable GetSelectedEvents()
		{
			DataTable dtSel = new DataTable();
			try
			{
				//dtSel.Columns.Add("AlertKey");
				dtSel.Columns.Add("EventID");
				dtSel.Columns.Add("ServerTypeID");
				//10/16/2014 NS added
				dtSel.Columns.Add("ConsecutiveFailures");
				//string selValues = "";
				TreeListNodeIterator iterator = EventsTreeList.CreateNodeIterator();
				TreeListNode node;
				//TreeListColumn columnActid = EventsTreeList.Columns["actid"];
				TreeListColumn columnActid = EventsTreeList.Columns["actid"];
				TreeListColumn columnSrvId = EventsTreeList.Columns["SrvId"];
				TreeListColumn columnTbl = EventsTreeList.Columns["tbl"];
				while (true)
				{
					node = iterator.GetNext();

					if (node == null) break;


					//if (node.Level == 1 && node.ParentNode.Selected)
					//{
					//    // root node selected ie All Events selected
					//    DataRow dr = dtSel.NewRow();
					//    //dr["AlertKey"] = AlertKey;
					//    dr["EventID"] = 0;// node.GetValue("actid");
					//    dr["ServerTypeID"] = 0;//node.GetValue("SrvId");
					//    dtSel.Rows.Add(dr);
					//    break;
					//}
					//else if (node.Level == 1 )
					//{
					//    // level 1 node selected ie One Servertype selected and all events under it
					//    if (node.Selected)
					//    {
					//        DataRow dr = dtSel.NewRow();
					//        //dr["AlertKey"] = AlertKey;
					//        dr["EventID"] = node.GetValue("actid");
					//        dr["ServerTypeID"] = ((System.Data.DataRowView)(node.DataItem)).Row.ItemArray[3];
					//        dtSel.Rows.Add(dr);
					//    }
					//}
					else if (node.Level == 2)//&& node.ParentNode.Selected == false)
					{
						if (node.Selected)
						{
							DataRow dr = dtSel.NewRow();
							//dr["AlertKey"] = AlertKey;
							dr["EventID"] = node.GetValue("actid");
							dr["ServerTypeID"] = node.GetValue("SrvId");
							//10/16/2014 NS added
							dr["ConsecutiveFailures"] = node.GetValue("ConsecutiveFailures");
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

				//6/27/2014 NS added for VSPLUS-634
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
			}
			return dtSel;
		}
		protected void OKButton_Click(object sender, EventArgs e)
		{
		
			//DataTable dt;
			try
			{

				DataTable dt = GetSelectedEvents(1);
				DataTable dt1 = GetSelectedServers(1);
				
				
			DataTable	Hourstable = Session["AlertDetails"] as DataTable;
				

				//4/2/2015 NS added for VSPLUS-219

				DataTable Escalationtable = Session["EscalationDetails"] as DataTable;
				
				if (dt.Rows.Count > 0 && dt1.Rows.Count > 0 && Hourstable.Rows.Count > 0)
				{
					InsertAlertData();					
					//5/13/2015 NS modified for VSPLUS-1763
					//4/2/2015 NS added for VSPLUS-219
					if (Escalationtable.Rows.Count > 0)
					{
						InsertEscalationData();
					}
					AlertDetails Alertobj = new AlertDetails();
					if (Session["key"] != null && Session["key"] != "")
					{
						if (Convert.ToInt32(Session["key"].ToString()) > 0 && Hourstable.Rows.Count!=1)
						{

							//7/3/2013 NS added detailes deletion

							Alertobj.ID = Convert.ToInt32(Session["key"].ToString());

							VSWebBL.ConfiguratorBL.AlertsBL.Ins.DeleteHoursData(Alertobj);
							Session["AlertDetails"] = Htable;
							fillHoursGridfromSession();
							Session["key"] = "";
						}
					}
                    EscalationDetails escalationObj = new EscalationDetails();
                    if (Session["Ekey"] != null && Session["Ekey"] != "")
                    {
                        if (Convert.ToInt32(Session["Ekey"].ToString()) > 0)
                        {

                            //7/3/2013 NS added detailes deletion

                            escalationObj.ID = Convert.ToInt32(Session["Ekey"].ToString());

                            VSWebBL.ConfiguratorBL.AlertsBL.Ins.DeleteEscalationData(escalationObj);
                            Session["EscalationDetails"] = Escalationtable;
                            fillEscalationGridfromSession();
                            Session["Ekey"] = "";
                        }
                    }
					Response.Redirect("AlertDefinitions_Grid.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
					Context.ApplicationInstance.CompleteRequest();
				}
				else
                {
                    //3/10/2016 Durga Modified for VSPLUS-2677
                    OKButton.ClientSideEvents.Init = "function(s, e) { LoadingPanel.Hide(); }";
					//2/28/2014 NS modified
					/*
					ErrorMessageLabel.Text = " Please enter atleast one Hours entry and select at least one Event, one Server.";
					ErrorMessagePopupControl.HeaderText = "Information";
					ErrorMessagePopupControl.ShowCloseButton = false;
					ValidationUpdatedButton.Visible = false;
					ValidationOkButton.Visible = true;
					ErrorMessagePopupControl.ShowOnPageLoad = true;
					 */
					errorDiv.Style.Value = "display: block";
					//10/3/2014 NS modified for VSPLUS-990
                    //11/13/2015 NS modified for VSPLUS-2355
					errorDiv.InnerHtml = "Please create at least one Hours entry, and select at least one Event and one Server." +
						"<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
				}

			}
			catch (Exception ex)
			{

				//Log.Entry.Ins.Write(Server.MapPath("~/LogFiles/"), "VSPlusLog.txt", DateTime.Now.ToString() + " Error in Page: " +
				//    Request.Url.AbsolutePath + ", Method: " + System.Reflection.MethodBase.GetCurrentMethod().Name +
				//    ", Error: " + ex.ToString());

				//6/27/2014 NS added for VSPLUS-634
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
			}
		}
		protected void CancelButton_Click(object sender, EventArgs e)
		{
			Response.Redirect("AlertDefinitions_Grid.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
			Context.ApplicationInstance.CompleteRequest();
		}
		public void InsertAlertData()
		{
			if (Session["AlertDetails"] != null)
			{
				try
				{
					//6/24/2015 NS added
					List<int> list = new List<int>();
					bool result = false;
					bool result2 = false, result3 = true;
					DataTable Hourstable = Session["AlertDetails"] as DataTable;
					DataTable dt = new DataTable();
					AlertDetails alertObj = new AlertDetails();
					AlertNames AltObj = new AlertNames();
					AltObj.AlertName = AlertNameTextBox.Text;

					if (flag == "Update")
					{
						tempname = AlerteventsComboBox.Text;
						DataTable templateids = VSWebBL.ConfiguratorBL.AlertsBL.Ins.GetEventsTemplateidbyNames(tempname);
						if (templateids.Rows.Count > 0)
						{
							int profileid = Convert.ToInt32(templateids.Rows[0]["ID"]);
							AltObj.Templateid = profileid;
							AltObj.AlertKey = AlertKey;
						}
						else
						{
							AltObj.Templateid = 0;
							AltObj.AlertKey = AlertKey;
						}
					}
					//if (flag == "Insert")
					//{
					//    tempname = AlerteventsComboBox.Text;
					//    DataTable templateids = VSWebBL.ConfiguratorBL.AlertsBL.Ins.GetEventsTemplateidbyNames(tempname);
					//    if (templateids.Rows.Count > 0)
					//    {
					//        int profileid = Convert.ToInt32(templateids.Rows[0]["ID"]);
					//        AltObj.Templateid = profileid;
					//        AltObj.AlertKey = AlertKey;
					//    }
					//    else
					//    {
					//        AltObj.Templateid = 0;
					//        AltObj.AlertKey = AlertKey;
					//    }
					//}
					DataTable returntab = VSWebBL.ConfiguratorBL.AlertsBL.Ins.GetDataByAlertName(AltObj);
					if (returntab.Rows.Count > 0)
					{
						ErrorMessagePopupControl.ShowOnPageLoad = true;
						ErrorMessageLabel.Text = "The Alert Name you entered already exists. Please enter a different name.";
					}
					else
					{
						try
						{
							if (flag == "Insert")
							{
								result = VSWebBL.ConfiguratorBL.AlertsBL.Ins.InsertAlertName(collectAlertKey());
								for (int i = 0; i < Hourstable.Rows.Count; i++)
								{
									alertObj.HoursIndicator = VSWebBL.ConfiguratorBL.AlertsBL.Ins.GetHrsIndicator(Hourstable.Rows[i]["HoursIndicator"].ToString());
									alertObj.SendTo = Hourstable.Rows[i]["SendTo"].ToString();
									alertObj.CopyTo = Hourstable.Rows[i]["CopyTo"].ToString();
									alertObj.BlindCopyTo = Hourstable.Rows[i]["BlindCopyTo"].ToString();
									//12/1/2014 NS added for VSPLUS-946
									alertObj.SMSTo = Hourstable.Rows[i]["SMSTo"].ToString();
									alertObj.StartTime = Hourstable.Rows[i]["StartTime"].ToString();
									alertObj.Duration = Convert.ToInt32(Hourstable.Rows[i]["Duration"].ToString());
									alertObj.Day = Hourstable.Rows[i]["Day"].ToString();
									alertObj.SendSNMPTrap = Convert.ToBoolean(Hourstable.Rows[i]["SendSNMPTrap"].ToString());
									//4/4/2014 NS added for VSPLUS-519
									alertObj.EnablePersistentAlert = Convert.ToBoolean(Hourstable.Rows[i]["EnablePersistentAlert"].ToString());
									//12/4/2014 NS added for VSPLUS-1229
									alertObj.ScriptID = Convert.ToInt32(Hourstable.Rows[i]["ScriptID"].ToString());
									dt = VSWebBL.ConfiguratorBL.AlertsBL.Ins.GetDataByAlertName(collectAlertKey());//brijesh
									alertObj.AlertKey = int.Parse(dt.Rows[0]["AlertKey"].ToString());//brijesh
									AlertKey = alertObj.AlertKey;
									result2 = VSWebBL.ConfiguratorBL.AlertsBL.Ins.InsertAlertDetails(alertObj);//brijesh  
								}
							}

							if (flag == "Update")
							{
								int i, j, l = 0;
								result = VSWebBL.ConfiguratorBL.AlertsBL.Ins.UpdateName(collectAlertKey());
								DataTable dt2 = VSWebBL.ConfiguratorBL.AlertsBL.Ins.GetAlertDetailsData(AlertKey);
								if (dt2.Rows.Count > 0)
								{
									for (i = 0; i < dt2.Rows.Count; i++)
									{
										int dtID = int.Parse(dt2.Rows[i]["ID"].ToString());
										list.Add(dtID);
										for (j = i; j < Hourstable.Rows.Count; j++, l = j)
										{
											if (int.Parse(Hourstable.Rows[j]["ID"].ToString()) != 0)
											{
												int hoursTableID = int.Parse(Hourstable.Rows[j]["ID"].ToString());
												if (dtID == hoursTableID)
												{
													alertObj.HoursIndicator = VSWebBL.ConfiguratorBL.AlertsBL.Ins.GetHrsIndicator(Hourstable.Rows[j]["HoursIndicator"].ToString());
													alertObj.SendTo = Hourstable.Rows[j]["SendTo"].ToString();
													alertObj.CopyTo = Hourstable.Rows[j]["CopyTo"].ToString();
													alertObj.BlindCopyTo = Hourstable.Rows[j]["BlindCopyTo"].ToString();
													//12/1/2014 NS added for VSPLUS-946
													alertObj.SMSTo = Hourstable.Rows[j]["SMSTo"].ToString();
													alertObj.StartTime = Hourstable.Rows[j]["StartTime"].ToString();
													alertObj.Duration = Convert.ToInt32(Hourstable.Rows[j]["Duration"].ToString());
													alertObj.Day = Hourstable.Rows[j]["Day"].ToString();
													alertObj.SendSNMPTrap = Convert.ToBoolean(Hourstable.Rows[j]["SendSNMPTrap"].ToString());
													//4/4/2014 NS added for VSPLUS-519
													alertObj.EnablePersistentAlert = Convert.ToBoolean(Hourstable.Rows[j]["EnablePersistentAlert"].ToString());
													alertObj.AlertKey = AlertKey;
													alertObj.ID = Convert.ToInt32(Hourstable.Rows[j]["ID"].ToString());
													//12/4/2014 NS added for VSPLUS-1229
													alertObj.ScriptID = Convert.ToInt32(Hourstable.Rows[j]["ScriptID"].ToString());
													result3 = VSWebBL.ConfiguratorBL.AlertsBL.Ins.UpdateAlertDetails(alertObj);
												}
											}
										}
									}
									if (dt2.Rows.Count < Hourstable.Rows.Count)
									{
										for (int x = 0; x < Hourstable.Rows.Count; x++)
										{
											int hoursTableID = int.Parse(Hourstable.Rows[x]["ID"].ToString());
											if (!list.Contains(hoursTableID))
											{
												alertObj.HoursIndicator = VSWebBL.ConfiguratorBL.AlertsBL.Ins.GetHrsIndicator(Hourstable.Rows[x]["HoursIndicator"].ToString());
												alertObj.SendTo = Hourstable.Rows[x]["SendTo"].ToString();
												alertObj.CopyTo = Hourstable.Rows[x]["CopyTo"].ToString();
												alertObj.BlindCopyTo = Hourstable.Rows[x]["BlindCopyTo"].ToString();
												//12/1/2014 NS added for VSPLUS-946
												alertObj.SMSTo = Hourstable.Rows[x]["SMSTo"].ToString();
												alertObj.StartTime = Hourstable.Rows[x]["StartTime"].ToString();
												alertObj.Duration = Convert.ToInt32(Hourstable.Rows[x]["Duration"].ToString());
												alertObj.Day = Hourstable.Rows[x]["Day"].ToString();
												alertObj.SendSNMPTrap = Convert.ToBoolean(Hourstable.Rows[x]["SendSNMPTrap"].ToString());
												//4/4/2014 NS added for VSPLUS-519
												alertObj.EnablePersistentAlert = Convert.ToBoolean(Hourstable.Rows[x]["EnablePersistentAlert"].ToString());
												alertObj.AlertKey = AlertKey;
												//12/4/2014 NS added for VSPLUS-1229
												alertObj.ScriptID = Convert.ToInt32(Hourstable.Rows[x]["ScriptID"].ToString());
												result3 = VSWebBL.ConfiguratorBL.AlertsBL.Ins.InsertAlertDetails(alertObj);
											}
										}
									}
								}
								else
								{
									for (int k = 0; k < Hourstable.Rows.Count; k++)
									{
										alertObj.HoursIndicator = VSWebBL.ConfiguratorBL.AlertsBL.Ins.GetHrsIndicator(Hourstable.Rows[k]["HoursIndicator"].ToString());
										alertObj.SendTo = Hourstable.Rows[k]["SendTo"].ToString();
										alertObj.CopyTo = Hourstable.Rows[k]["CopyTo"].ToString();
										alertObj.BlindCopyTo = Hourstable.Rows[k]["BlindCopyTo"].ToString();
										//12/1/2014 NS added for VSPLUS-946
										alertObj.SMSTo = Hourstable.Rows[k]["SMSTo"].ToString();
										alertObj.StartTime = Hourstable.Rows[k]["StartTime"].ToString();
										alertObj.Duration = Convert.ToInt32(Hourstable.Rows[k]["Duration"].ToString());
										alertObj.Day = Hourstable.Rows[k]["Day"].ToString();
										alertObj.SendSNMPTrap = Convert.ToBoolean(Hourstable.Rows[k]["SendSNMPTrap"].ToString());
										//4/4/2014 NS added for VSPLUS-519
										alertObj.EnablePersistentAlert = Convert.ToBoolean(Hourstable.Rows[k]["EnablePersistentAlert"].ToString());
										alertObj.AlertKey = AlertKey;
										//12/4/2014 NS added for VSPLUS-1229
										alertObj.ScriptID = Convert.ToInt32(Hourstable.Rows[k]["ScriptID"].ToString());
										result3 = VSWebBL.ConfiguratorBL.AlertsBL.Ins.InsertAlertDetails(alertObj);
									}
								}
							}

							bool retnServers = false, retnEvents = false, retnHours = false;
							if (result == true || result2 == true || result3 == true)
							{
								//Servers & locations
								string AlertName = AlertNameTextBox.Text;
								//retnServerTypes = VSWebBL.ConfiguratorBL.AlertsBL.Ins.InsertRestrictedServerTypes(AlertName, RestrictedServertypes());
								//retnEvents = VSWebBL.ConfiguratorBL.AlertsBL.Ins.InsertRestrictedEvents(AlertName, RestrictedEvents());
								//retnLocations = VSWebBL.ConfiguratorBL.AlertsBL.Ins.InsertRestrictedLocations(AlertName, RestrictedLocations());
								//retnServers = VSWebBL.ConfiguratorBL.AlertsBL.Ins.InsertRestrictedServers(AlertName, RestrictedServers());

								//loop
								//retnHours = VSWebBL.ConfiguratorBL.AlertsBL.Ins.InsertHours(AlertName, Hourstable);
								retnHours = true;
								retnEvents = VSWebBL.ConfiguratorBL.AlertsBL.Ins.InsertSelectedEvents(AlertKey, GetSelectedEvents(AlertKey));
								retnServers = VSWebBL.ConfiguratorBL.AlertsBL.Ins.InsertSelectedServers(AlertKey, GetSelectedServers(AlertKey));

							}
							if (retnEvents == true && retnServers == true && retnHours == true)
							{
								if (flag == "Insert")
								{
									ErrorMessageLabel.Text = "New record created successfully.";
								}
								if (flag == "Update")
								{
									ErrorMessageLabel.Text = "Record updated successfully.";
								}

								ErrorMessagePopupControl.HeaderText = "Information";
								ErrorMessagePopupControl.ShowCloseButton = false;
								ValidationUpdatedButton.Visible = true;
								ValidationOkButton.Visible = false;
								ErrorMessagePopupControl.ShowOnPageLoad = true;
							}

						}
						catch (Exception ex)
						{
							//Log.Entry.Ins.Write(Server.MapPath("~/LogFiles/"), "VSPlusLog.txt", DateTime.Now.ToString() + " Error in Page: " +
							//    Request.Url.AbsolutePath + ", Method: " + System.Reflection.MethodBase.GetCurrentMethod().Name +
							//    ", Error: in block for insert/update : " + ex.ToString());

							//6/27/2014 NS added for VSPLUS-634
							Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
						}

					}
				}
				catch (Exception ex)
				{
					//Log.Entry.Ins.Write(Server.MapPath("~/LogFiles/"), "VSPlusLog.txt", DateTime.Now.ToString() + " Error in Page: " +
					//    Request.Url.AbsolutePath + ", Method: " + System.Reflection.MethodBase.GetCurrentMethod().Name +
					//    ", Error: " + ex.ToString());

					//6/27/2014 NS added for VSPLUS-634
					Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				}
			}
		}
		public AlertNames collectAlertKey()
		{
			AlertNames Aobj = new AlertNames();
			try
			{

				if (flag == "Update")
				{
					Aobj.AlertKey = AlertKey;
				}
				Aobj.AlertName = AlertNameTextBox.Text;
				tempname = AlerteventsComboBox.Text;
				DataTable templateids = VSWebBL.ConfiguratorBL.AlertsBL.Ins.GetEventsTemplateidbyNames(tempname);
				if (templateids.Rows.Count > 0)
				{
					int profileid = Convert.ToInt32(templateids.Rows[0]["ID"]);
					Aobj.Templateid = profileid;
					//Aobj.AlertKey = AlertKey;
					//Aobj.Templateid = Convert.ToInt32(AlerteventsComboBox.SelectedItem.Value); 
				}
				else
				{
					Aobj.Templateid = 0;

				}
			}
			catch (Exception ex)
			{
				//Log.Entry.Ins.Write(Server.MapPath("~/LogFiles/"), "VSPlusLog.txt", DateTime.Now.ToString() + " Error in Page: " +
				//    Request.Url.AbsolutePath + ", Method: " + System.Reflection.MethodBase.GetCurrentMethod().Name +
				//    ", Error: " + ex.ToString());

				//6/27/2014 NS added for VSPLUS-634
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
			}
			return Aobj;
		}
		protected void HoursGridView_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
		{
			try
			{
				if (e.Column.FieldName == "HoursIndicator")
				{
					ASPxComboBox TypeComboBox = e.Editor as ASPxComboBox;
					FillTypeComboBox(TypeComboBox);
					TypeComboBox.Callback += new CallbackEventHandlerBase(TypeComboBox_OnCallback);
				}
			}
			catch (Exception ex)
			{
				//Log.Entry.Ins.Write(Server.MapPath("~/LogFiles/"), "VSPlusLog.txt", DateTime.Now.ToString() + " Error in Page: " +
				//    Request.Url.AbsolutePath + ", Method: " + System.Reflection.MethodBase.GetCurrentMethod().Name +
				//    ", Error: " + ex.ToString());

				//6/27/2014 NS added for VSPLUS-634
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
			}
		}
		private void TypeComboBox_OnCallback(object source, CallbackEventArgsBase e)
		{
			try
			{
				FillTypeComboBox(source as ASPxComboBox);
			}
			catch (Exception ex)
			{
				//Log.Entry.Ins.Write(Server.MapPath("~/LogFiles/"), "VSPlusLog.txt", DateTime.Now.ToString() + " Error in Page: " +
				//    Request.Url.AbsolutePath + ", Method: " + System.Reflection.MethodBase.GetCurrentMethod().Name +
				//    ", Error: " + ex.ToString());

				//6/27/2014 NS added for VSPLUS-634
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
			}
		}
		private void FillTypeComboBox(ASPxComboBox TypeComboBox)
		{
			try
			{

				DataTable TypeDataTable = VSWebBL.ConfiguratorBL.AlertsBL.Ins.GetTypes();
				//DataTable ServerDataTable = VSWebBL.SecurityBL.AdminTabBL.Ins.GetNavigatorChildsByRefName("ServersDevices");
				TypeComboBox.DataSource = TypeDataTable;
				TypeComboBox.TextField = "Type";
				TypeComboBox.ValueField = "ID";
				TypeComboBox.DataBind();

				ASPxComboBox cmbType = (ASPxComboBox)HoursGridView.FindEditFormTemplateControl("cmbType");
				ASPxLabel lblStartTime = (ASPxLabel)HoursGridView.FindEditFormTemplateControl("lblStartTime");
				ASPxTimeEdit timeEdtStartTime = (ASPxTimeEdit)HoursGridView.FindEditFormTemplateControl("timeEdtStartTime");
				ASPxLabel lblDuration = (ASPxLabel)HoursGridView.FindEditFormTemplateControl("lblDuration");
				//ASPxLabel lblMin = (ASPxLabel)HoursGridView.FindEditFormTemplateControl("lblMin");
				ASPxTextBox txtDuration = (ASPxTextBox)HoursGridView.FindEditFormTemplateControl("txtDuration");
				ASPxLabel lblDays = (ASPxLabel)HoursGridView.FindEditFormTemplateControl("lblDays");
				ASPxListBox lstBoxDays = (ASPxListBox)HoursGridView.FindEditFormTemplateControl("lstBoxDays");
				if (TypeComboBox.SelectedItem != null)
				{
					if (TypeComboBox.SelectedItem.Text != "Specific Hours")
					{
						//    /*
						//    lblStartTime.Visible = false;
						//    timeEdtStartTime.Visible = false;
						//    lblDuration.Visible = false;
						//    lblMin.Visible = false;
						//    txtDuration.Visible = false;
						//    lblDays.Visible = false;
						//    lstBoxDays.Visible = false;
						//     */
						lblStartTime.ClientVisible = false;
						timeEdtStartTime.ClientVisible = false;
						lblDuration.ClientVisible = false;
						//lblMin.ClientVisible = false;
						txtDuration.ClientVisible = false;
						lblDays.ClientVisible = false;
						lstBoxDays.ClientVisible = false;
					}
					else
					{
						/*
					//    lblStartTime.Visible = true;
					//    timeEdtStartTime.Visible = true;
					//    lblDuration.Visible = true;
					//    lblMin.Visible = true;
					//    txtDuration.Visible = true;
					//    lblDays.Visible = true;
					//    lstBoxDays.Visible = true;
					//     */
						lblStartTime.ClientVisible = true;
						timeEdtStartTime.ClientVisible = true;
						lblDuration.ClientVisible = true;
						//lblMin.ClientVisible = true;
						txtDuration.ClientVisible = true;
						lblDays.ClientVisible = true;
						lstBoxDays.ClientVisible = true;
					}
				}
			}
			catch (Exception ex)
			{
				//Log.Entry.Ins.Write(Server.MapPath("~/LogFiles/"), "VSPlusLog.txt", DateTime.Now.ToString() + " Error in Page: " +
				//    Request.Url.AbsolutePath + ", Method: " + System.Reflection.MethodBase.GetCurrentMethod().Name +
				//    ", Error: " + ex.ToString());

				//6/27/2014 NS added for VSPLUS-634
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
			}
		}
		protected void HoursGridView_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
		{
			try
			{
				ASPxComboBox cmbType = (ASPxComboBox)HoursGridView.FindEditFormTemplateControl("cmbType");
				ASPxLabel lblStartTime = (ASPxLabel)HoursGridView.FindEditFormTemplateControl("lblStartTime");
				ASPxTimeEdit timeEdtStartTime = (ASPxTimeEdit)HoursGridView.FindEditFormTemplateControl("timeEdtStartTime");
				ASPxLabel lblDuration = (ASPxLabel)HoursGridView.FindEditFormTemplateControl("lblDuration");
				//ASPxLabel lblMin = (ASPxLabel)HoursGridView.FindEditFormTemplateControl("lblMin");
				ASPxTextBox txtDuration = (ASPxTextBox)HoursGridView.FindEditFormTemplateControl("txtDuration");
				ASPxLabel lblDays = (ASPxLabel)HoursGridView.FindEditFormTemplateControl("lblDays");
				ASPxListBox lstBoxDays = (ASPxListBox)HoursGridView.FindEditFormTemplateControl("lstBoxDays");

				if (cmbType.SelectedItem != null)
				{
					if (cmbType.SelectedItem.Text != "Specific Hours")
					{
						/*
						lblStartTime.Visible = false;
						timeEdtStartTime.Visible = false;
						lblDuration.Visible = false;
						lblMin.Visible = false;
						txtDuration.Visible = false;
						lblDays.Visible = false;
						lstBoxDays.Visible = false;
						 */
						lblStartTime.ClientVisible = false;
						timeEdtStartTime.ClientVisible = false;
						lblDuration.ClientVisible = false;
						//lblMin.ClientVisible = false;
						txtDuration.ClientVisible = false;
						lblDays.ClientVisible = false;
						lstBoxDays.ClientVisible = false;
					}
					else
					{
						/*
						lblStartTime.Visible = true;
						timeEdtStartTime.Visible = true;
						lblDuration.Visible = true;
						lblMin.Visible = true;
						txtDuration.Visible = true;
						lblDays.Visible = true;
						lstBoxDays.Visible = true;
						 */
						lblStartTime.ClientVisible = true;
						timeEdtStartTime.ClientVisible = true;
						lblDuration.ClientVisible = true;
						//lblMin.ClientVisible = true;
						txtDuration.ClientVisible = true;
						lblDays.ClientVisible = true;
						lstBoxDays.ClientVisible = true;
						if (timeEdtStartTime.Text == null || timeEdtStartTime.Text == "")
						{

							timeEdtStartTime.Text = "12:00 AM";

						}
					}
				}
			}
			catch (Exception ex)
			{
				//Log.Entry.Ins.Write(Server.MapPath("~/LogFiles/"), "VSPlusLog.txt", DateTime.Now.ToString() + " Error in Page: " +
				//    Request.Url.AbsolutePath + ", Method: " + System.Reflection.MethodBase.GetCurrentMethod().Name +
				//    ", Error: " + ex.ToString());

				//6/27/2014 NS added for VSPLUS-634
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
			}
		}
		protected void HoursGridView_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
		{
			try
			{
				string strSMSTo = "";

				ASPxComboBox cmbType = (ASPxComboBox)HoursGridView.FindEditFormTemplateControl("cmbType");
				ASPxTimeEdit timeEdtStartTime = (ASPxTimeEdit)HoursGridView.FindEditFormTemplateControl("timeEdtStartTime");
				ASPxLabel lblStartTime = (ASPxLabel)HoursGridView.FindEditFormTemplateControl("lblStartTime");
				ASPxLabel lblDuration = (ASPxLabel)HoursGridView.FindEditFormTemplateControl("lblDuration");
				//ASPxLabel lblMin = (ASPxLabel)HoursGridView.FindEditFormTemplateControl("lblMin");
				ASPxLabel lblDays = (ASPxLabel)HoursGridView.FindEditFormTemplateControl("lblDays");

				//ASPxTimeEdit timeEdtEndTime = (ASPxTimeEdit)HoursGridView.FindEditFormTemplateControl("timeEdtEndTime");
				ASPxTextBox txtDuration = (ASPxTextBox)HoursGridView.FindEditFormTemplateControl("txtDuration");
				ASPxLabel lblSendTo = (ASPxLabel)HoursGridView.FindEditFormTemplateControl("lblSendTo");
				ASPxTextBox txtSendTo = (ASPxTextBox)HoursGridView.FindEditFormTemplateControl("txtSendTo");
				ASPxLabel lblCopyTo = (ASPxLabel)HoursGridView.FindEditFormTemplateControl("lblCopyTo");
				ASPxTextBox txtCopyTo = (ASPxTextBox)HoursGridView.FindEditFormTemplateControl("txtCopyTo");
				ASPxLabel lblBlindCopyTo = (ASPxLabel)HoursGridView.FindEditFormTemplateControl("lblBlindCopyTo");
				ASPxTextBox txtBlindCopyTo = (ASPxTextBox)HoursGridView.FindEditFormTemplateControl("txtBlindCopyTo");
				//12/1/2014 NS added for VSPLUS-946
				ASPxLabel lblSMSTo = (ASPxLabel)HoursGridView.FindEditFormTemplateControl("lblSMSTo");
				ASPxTextBox txtSMSTo = (ASPxTextBox)HoursGridView.FindEditFormTemplateControl("txtSMSTo");
				//ASPxTextBox txtEscalateTo = (ASPxTextBox)HoursGridView.FindEditFormTemplateControl("txtEscalateTo");
				ASPxCheckBox chkBxSendSNMPTrap = (ASPxCheckBox)HoursGridView.FindEditFormTemplateControl("chkBxSendSNMPTrap");
				ASPxListBox lstBoxDays = (ASPxListBox)HoursGridView.FindEditFormTemplateControl("lstBoxDays");
				//12/4/2014 NS added for VSPLUS-1229
				ASPxLabel lblScript = (ASPxLabel)HoursGridView.FindEditFormTemplateControl("lblScript");
				ASPxComboBox cmbScript = (ASPxComboBox)HoursGridView.FindEditFormTemplateControl("cmbScript");
				ASPxComboBox cmbMechanism = (ASPxComboBox)HoursGridView.FindEditFormTemplateControl("cmbMechanism");
				ASPxCheckBox chkboxPersistent = (ASPxCheckBox)HoursGridView.FindEditFormTemplateControl("chkboxPersistent");
				if (e.RowType == GridViewRowType.EditForm)
				{
					FillTypeComboBox(cmbType);
					//12/4/2014 NS added for VSPLUS-1229
					FillScriptCombobox(cmbScript);

					if (!((DevExpress.Web.ASPxGridView)(sender)).IsNewRowEditing)
						cmbType.Value = e.GetValue("HoursIndicator");
					if (!((DevExpress.Web.ASPxGridView)(sender)).IsNewRowEditing)
						timeEdtStartTime.Text = e.GetValue("StartTime").ToString();
					if (!((DevExpress.Web.ASPxGridView)(sender)).IsNewRowEditing)
						txtDuration.Value = e.GetValue("Duration");
					if (!((DevExpress.Web.ASPxGridView)(sender)).IsNewRowEditing)
						txtSendTo.Value = e.GetValue("SendTo");


					if (!((DevExpress.Web.ASPxGridView)(sender)).IsNewRowEditing)
						txtCopyTo.Value = e.GetValue("CopyTo");
					if (!((DevExpress.Web.ASPxGridView)(sender)).IsNewRowEditing)
						txtBlindCopyTo.Value = e.GetValue("BlindCopyTo");
					//12/1/2014 NS added for VSPLUS-946
					if (!((DevExpress.Web.ASPxGridView)(sender)).IsNewRowEditing)
					{
						//txtSMSTo.Value = e.GetValue("SMSTo");
						strSMSTo = e.GetValue("SMSTo").ToString();
						if (strSMSTo != "")
						{
							SMSFromNumber = e.GetValue("SMSTo").ToString();
						}
					}
					HtmlGenericControl divControl = (HtmlGenericControl)HoursGridView.FindEditFormTemplateControl("divSendto");
					divControl.InnerHtml = " <input type='tel' id='phone' value='" + strSMSTo + "'  runat='server'>" +
											"<input type='hidden' id='hidden_phone' name='hidden_phone' value='" + strSMSTo + "' runat='server' /><br><br>" +
											"<div><span id='valid-msg' class='hide'>✓ Valid</span>" +
											"<span id='error-msg' class='hide'>Invalid number</span></div>";
					if (strSMSTo != "")
					{
						divControl.Style.Value = "display: block";
						//7/1/2015 NS added for VSPLUS-1894
						HtmlGenericControl divSMSControl = (HtmlGenericControl)HoursGridView.FindEditFormTemplateControl("divSMSToMsg");
						bool isconfig = false;
						isconfig = Convert.ToBoolean(Session["SMSConfig"]);
						if (!isconfig)
						{
							divSMSControl.Style.Value = "display: block";
						}
						else
						{
							divSMSControl.Style.Value = "display: none";
						}
					}
					if (!((DevExpress.Web.ASPxGridView)(sender)).IsNewRowEditing)
						chkBxSendSNMPTrap.Checked = Convert.ToBoolean(e.GetValue("SendSNMPTrap"));
					//12/10/2014 NS added for VSPLUS-1229
					if (!((DevExpress.Web.ASPxGridView)(sender)).IsNewRowEditing)
					{
						for (int i = 0; i < cmbScript.Items.Count; i++)
						{
							if (cmbScript.Items[i].Text == e.GetValue("ScriptName").ToString())
							{
								cmbScript.SelectedIndex = i;
							}
						}
					}
					//if (!((DevExpress.Web.ASPxGridView)(sender)).IsNewRowEditing)
					//{
					//    //chkBxSendSNMPTrap.Value = e.GetValue("chkBxSendSNMPTrap").ToString();                    
					//    chkBxSendSNMPTrap.Value = e.GetValue("chkBxSendSNMPTrap");
					//}

					if (!((DevExpress.Web.ASPxGridView)(sender)).IsNewRowEditing)
					{
						if (e.GetValue("Day").ToString().Contains("Sunday"))
						{
							lstBoxDays.Items[6].Selected = true;
						}
						if (e.GetValue("Day").ToString().Contains("Monday"))
						{
							lstBoxDays.Items[0].Selected = true;
						}
						if (e.GetValue("Day").ToString().Contains("Tuesday"))
						{
							lstBoxDays.Items[1].Selected = true;
						}
						if (e.GetValue("Day").ToString().Contains("Wednesday"))
						{
							lstBoxDays.Items[2].Selected = true;
						}
						if (e.GetValue("Day").ToString().Contains("Thursday"))
						{
							lstBoxDays.Items[3].Selected = true;
						}
						if (e.GetValue("Day").ToString().Contains("Friday"))
						{
							lstBoxDays.Items[4].Selected = true;
						}
						if (e.GetValue("Day").ToString().Contains("Saturday"))
						{
							lstBoxDays.Items[5].Selected = true;
						}
					}
					//5/31/2013 NS added
					else
					{
						for (int i = 0; i < 7; i++)
						{
							//  lstBoxDays.Items[i].Selected = true;
						}
					}
					if (cmbType.Text != "Specific Hours" || cmbType.Text == "" || cmbType.Text == null)
					{
						lblStartTime.ClientVisible = false;
						timeEdtStartTime.ClientVisible = false;
						lblDuration.ClientVisible = false;
						//lblMin.ClientVisible = false;
						txtDuration.ClientVisible = false;
						lblDays.ClientVisible = false;
						lstBoxDays.ClientVisible = false;
					}
					else
					{
						lblStartTime.ClientVisible = true;
						if (timeEdtStartTime.Text == null || timeEdtStartTime.Text == "")
						{

							timeEdtStartTime.Text = "12:00 AM";

						}
						timeEdtStartTime.ClientVisible = true;
						lblDuration.ClientVisible = true;
						//lblMin.ClientVisible = false;
						txtDuration.ClientVisible = true;
						lblDays.ClientVisible = true;
						lstBoxDays.ClientVisible = true;
					}

					if (txtSendTo.Text != "" || ((DevExpress.Web.ASPxGridView)(sender)).IsNewRowEditing)
					{
						cmbMechanism.SelectedIndex = 0;
						lblScript.ClientVisible = false;
						cmbScript.ClientVisible = false;
						lblSMSTo.ClientVisible = false;
						//phone.ClientVisible = false;
						lblSendTo.ClientVisible = true;
						lblCopyTo.ClientVisible = true;
						lblBlindCopyTo.ClientVisible = true;
						txtSendTo.ClientVisible = true;
						txtCopyTo.ClientVisible = true;
						txtBlindCopyTo.ClientVisible = true;
						chkboxPersistent.ClientVisible = true;
						//6/11/2015 NS modified for VSPLUS-1862
						//chkBxSendSNMPTrap.ClientVisible = true;
					}
					else if (strSMSTo != "")
					{
						cmbMechanism.SelectedIndex = 1;
						lblSendTo.ClientVisible = false;
						lblCopyTo.ClientVisible = false;
						lblBlindCopyTo.ClientVisible = false;
						txtSendTo.ClientVisible = false;
						txtCopyTo.ClientVisible = false;
						txtBlindCopyTo.ClientVisible = false;
						chkboxPersistent.ClientVisible = false;
						//6/11/2015 NS modified for VSPLUS-1862
						//chkBxSendSNMPTrap.ClientVisible = false;
						lblScript.ClientVisible = false;
						cmbScript.ClientVisible = false;
						lblSMSTo.ClientVisible = true;
						//phone.ClientVisible = true;
					}
					else if (cmbScript.SelectedIndex >= 0)
					{
						cmbMechanism.SelectedIndex = 2;
						lblSendTo.ClientVisible = false;
						lblCopyTo.ClientVisible = false;
						lblBlindCopyTo.ClientVisible = false;
						txtSendTo.ClientVisible = false;
						txtCopyTo.ClientVisible = false;
						txtBlindCopyTo.ClientVisible = false;
						chkboxPersistent.ClientVisible = false;
						//6/11/2015 NS modified for VSPLUS-1862
						//chkBxSendSNMPTrap.ClientVisible = false;
						lblSMSTo.ClientVisible = false;
						//phone.ClientVisible = false;
						lblScript.ClientVisible = true;
						cmbScript.ClientVisible = true;
					}
					//6/11/2015 NS added for VSPLUS-1862
					else if (chkBxSendSNMPTrap.Checked)
					{
						cmbMechanism.SelectedIndex = 3;
						lblSendTo.ClientVisible = false;
						lblCopyTo.ClientVisible = false;
						lblBlindCopyTo.ClientVisible = false;
						txtSendTo.ClientVisible = false;
						txtCopyTo.ClientVisible = false;
						txtBlindCopyTo.ClientVisible = false;
						chkboxPersistent.ClientVisible = false;
						//6/11/2015 NS modified for VSPLUS-1862
						//chkBxSendSNMPTrap.ClientVisible = false;
						lblSMSTo.ClientVisible = false;
						//phone.ClientVisible = false;
						lblScript.ClientVisible = false;
						cmbScript.ClientVisible = false;
					}
					//5/14/2015 NS modified for VSPLUS-1752
					else
					{
						cmbMechanism.SelectedIndex = 4;
						lblSendTo.ClientVisible = false;
						lblCopyTo.ClientVisible = false;
						lblBlindCopyTo.ClientVisible = false;
						txtSendTo.ClientVisible = false;
						txtCopyTo.ClientVisible = false;
						txtBlindCopyTo.ClientVisible = false;
						chkboxPersistent.ClientVisible = false;
						lblSMSTo.ClientVisible = false;
						//phone.ClientVisible = false;
						lblScript.ClientVisible = false;
						cmbScript.ClientVisible = false;
					}
				}
			}
			catch (Exception ex)
			{
				//Log.Entry.Ins.Write(Server.MapPath("~/LogFiles/"), "VSPlusLog.txt", DateTime.Now.ToString() + " Error in Page: " +
				//    Request.Url.AbsolutePath + ", Method: " + System.Reflection.MethodBase.GetCurrentMethod().Name +
				//    ", Error: " + ex.ToString());

				//6/27/2014 NS added for VSPLUS-634
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
			}
		}
		protected void HoursGridView_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
		{
			try
			{
				DataTable Htable = Session["AlertDetails"] as DataTable;
				

				DataRow[] dr = Htable.Select("ID=" + Convert.ToInt32(e.Keys[0]));
				foreach (DataRow r in dr)
				{
					r.Delete();
					
					r.AcceptChanges();
				}
				//7/3/2013 NS added detailes deletion
				AlertDetails Alertobj = new AlertDetails();
				Alertobj.ID = Convert.ToInt32(e.Keys[0]);
				// VSWebBL.ConfiguratorBL.AlertsBL.Ins.DeleteHoursData(Alertobj);
				Session["AlertDetails"] = Htable;
               
                    Session["key"] = Alertobj.ID;
                
				ASPxGridView gridview = (ASPxGridView)sender;
				gridview.CancelEdit();
				e.Cancel = true;
				//fillHoursGridfromSession();
			}
			catch (Exception ex)
			{
				//Log.Entry.Ins.Write(Server.MapPath("~/LogFiles/"), "VSPlusLog.txt", DateTime.Now.ToString() + " Error in Page: " +
				//    Request.Url.AbsolutePath + ", Method: " + System.Reflection.MethodBase.GetCurrentMethod().Name +
				//    ", Error: " + ex.ToString());

				//6/27/2014 NS added for VSPLUS-634
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
			}
		}
		protected void HoursGridView_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
		{
			bool smsempty = false;
			ASPxListBox lstBoxDays = (ASPxListBox)HoursGridView.FindEditFormTemplateControl("lstBoxDays");
			ASPxComboBox cmbType = (ASPxComboBox)HoursGridView.FindEditFormTemplateControl("cmbType");
			//6/24/2015 NS added for VSPLUS-1838
			ASPxComboBox cmbMechanism = (ASPxComboBox)HoursGridView.FindEditFormTemplateControl("cmbMechanism");
			ASPxHiddenField hidden_phone1 = (ASPxHiddenField)HoursGridView.FindEditFormTemplateControl("hidden_phone1");
			try
			{
				if (Session["AlertDetails"] != null && Session["AlertDetails"] != "")
				{
					Htable = Session["AlertDetails"] as DataTable;
				}
				ASPxGridView gridView = (ASPxGridView)sender;


				if (cmbType.Text == "Specific Hours")
				{
					for (int i = 0; i < 7; i++)
					{
						if (lstBoxDays.Items[i].Selected == true)
						{

							listofdays = true;
						}

					}

					if (lstBoxDays.SelectedItems.Count == 0)
					{
						listofdays = false;
						throw new Exception("Please select at least one day.");

					}
				}

				//6/24/2015 NS added for VSPLUS-1838
				if (cmbMechanism.SelectedItem.Text == "SMS")
				{
					if (hidden_phone1.Contains("smsto"))
					{
						//9/28/2015 NS modified for VSPLUS-2186
						if (hidden_phone1["smsto"] == "" || hidden_phone1["smsto"].ToString() == "invalid")
						{
							smsempty = true;
							//throw new Exception("Please enter a phone number into the SMS field.");
							throw new CustomExceptions("Please enter a valid phone number into the SMS field.");
						}
					}
				}
				//4/2/2015 NS modified for VSPLUS-219
				UpdateData("Insert", GetRow(Htable, e.NewValues.GetEnumerator(), 0), "Hours");

				//Update Grid after inserting new row, refresh grid as in page load
				gridView.CancelEdit();
				e.Cancel = true;
				fillHoursGridfromSession();

			}
			catch (Exception ex)
			{
				//Log.Entry.Ins.Write(Server.MapPath("~/LogFiles/"), "VSPlusLog.txt", DateTime.Now.ToString() + " Error in Page: " +
				//    Request.Url.AbsolutePath + ", Method: " + System.Reflection.MethodBase.GetCurrentMethod().Name +
				//    ", Error: " + ex.ToString());

				//6/27/2014 NS added for VSPLUS-634
				if (cmbType.Text == "Specific Hours" && listofdays == false)
				{
					throw new Exception("Please select at least one day.");
				}
				else if (cmbMechanism.SelectedItem.Text == "SMS" && smsempty)
				{
					//9/28/2015 NS modified for VSPLUS-2186
					throw new CustomExceptions("Please enter a valid phone number into the SMS field.");
				}
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
			}
		}

		protected void HoursGridView_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
		{
			bool smsempty = false;
            //11/4/2015 NS added for VSPLUS-2186
            bool smsinvalid = false;
			ASPxGridView gridView = (ASPxGridView)sender;
			ASPxListBox lstBoxDays = (ASPxListBox)HoursGridView.FindEditFormTemplateControl("lstBoxDays");
			ASPxComboBox cmbType = (ASPxComboBox)HoursGridView.FindEditFormTemplateControl("cmbType");
			//6/24/2015 NS added for VSPLUS-1838
			ASPxComboBox cmbMechanism = (ASPxComboBox)HoursGridView.FindEditFormTemplateControl("cmbMechanism");
			ASPxHiddenField hidden_phone1 = (ASPxHiddenField)HoursGridView.FindEditFormTemplateControl("hidden_phone1");
			try
			{
				if (Session["AlertDetails"] != null && Session["AlertDetails"] != "")
					Htable = Session["AlertDetails"] as DataTable;
				if (cmbType.Text == "Specific Hours")
				{
					for (int i = 0; i < 7; i++)
					{
						if (lstBoxDays.Items[i].Selected == true)
						{

							listofdays = true;
						}

					}

					if (lstBoxDays.SelectedItems.Count == 0)
					{
						listofdays = false;
						throw new Exception("Please select at least one day.");

					}
				}
				//6/24/2015 NS added for VSPLUS-1838
				if (cmbMechanism.SelectedItem.Text == "SMS")
				{
					if (hidden_phone1.Contains("smsto"))
					{
						//11/4/2015 NS modified for VSPLUS-2186
                        if (hidden_phone1["smsto"] == "")
                        {
                            smsempty = true;
                            //throw new Exception("Please enter a phone number into the SMS field.");
                            throw new CustomExceptions("Please enter a phone number into the SMS field.");
                        }
                        else if (hidden_phone1["smsto"].ToString() == "invalid")
                        {
                            smsinvalid = true;
                            throw new CustomExceptions("Please enter a valid phone number into the SMS field.");
                        }
					}
				}
				gridView.DoRowValidation();

				//DataTable dataTable = STSettingsDataSet.Tables[0];
				//DataRow STSettingsRow = dataTable.NewRow();
				//STSettingsRow=GetRow(STSettingsDataSet, e.NewValues.GetEnumerator());

				//Insert row in DB
				//4/2/2015 NS modified for VSPLUS-219
				UpdateData("Update", GetRow(Htable, e.NewValues.GetEnumerator(), Convert.ToInt32(e.Keys[0])), "Hours");
				//Update Grid after inserting new row, refresh grid as in page load
				gridView.CancelEdit();
				e.Cancel = true;
				fillHoursGridfromSession();
			}
			catch (Exception ex)
			{
				//Log.Entry.Ins.Write(Server.MapPath("~/LogFiles/"), "VSPlusLog.txt", DateTime.Now.ToString() + " Error in Page: " +
				//    Request.Url.AbsolutePath + ", Method: " + System.Reflection.MethodBase.GetCurrentMethod().Name +
				//    ", Error: " + ex.ToString());

				//6/27/2014 NS added for VSPLUS-634
				if (cmbType.Text == "Specific Hours" && listofdays == false)
				{
					throw new Exception("Please select at least one day.");
				}
				else if (cmbMechanism.SelectedItem.Text == "SMS" && smsempty)
				{
					throw new Exception("Please enter a phone number into the SMS field.");
				}
                //11/4/2015 NS added for VSPLUS-2186
                else if (cmbMechanism.SelectedItem.Text == "SMS" && smsinvalid)
                {
                    throw new Exception("Please enter a valid phone number into the SMS field.");
                }
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
			}
		}
		protected DataRow GetRow(DataTable HoursObject, IDictionaryEnumerator enumerator, int Keys)
		{
			DataTable dataTable = HoursObject;
			DataRow DRRow = dataTable.NewRow();
			try
			{

				if (Keys == 0)
					DRRow = dataTable.NewRow();
				else
					DRRow = dataTable.Rows.Find(Keys);
				//IDictionaryEnumerator enumerator = e.NewValues.GetEnumerator();
				enumerator.Reset();
				while (enumerator.MoveNext())
					DRRow[enumerator.Key.ToString()] = (enumerator.Value == null ? "False" : enumerator.Value);

			}
			catch (Exception ex)
			{
				//Log.Entry.Ins.Write(Server.MapPath("~/LogFiles/"), "VSPlusLog.txt", DateTime.Now.ToString() + " Error in Page: " +
				//    Request.Url.AbsolutePath + ", Method: " + System.Reflection.MethodBase.GetCurrentMethod().Name +
				//    ", Error: " + ex.ToString());

				//6/27/2014 NS added for VSPLUS-634
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
			}
			return DRRow;
		}
		//4/2/2015 NS modified for VSPLUS-219
		private void UpdateData(string Mode, DataRow UsersRow, string WhichGrid)
		{
			try
			{

				if (WhichGrid == "Hours")
				{
					CollectData(Mode, UsersRow);
				}
				else
				{
					CollectDataEscalate(Mode, UsersRow);
				}
			}
			catch (Exception ex)
			{
				//Log.Entry.Ins.Write(Server.MapPath("~/LogFiles/"), "VSPlusLog.txt", DateTime.Now.ToString() + " Error in Page: " +
				//    Request.Url.AbsolutePath + ", Method: " + System.Reflection.MethodBase.GetCurrentMethod().Name +
				//    ", Error: " + ex.ToString());

				//6/27/2014 NS added for VSPLUS-634
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
			}
		}
		private void CollectData(string Mode, DataRow AlertRow)
		{
			try
			{
				DataTable Hourstable = Session["AlertDetails"] as DataTable;
				ASPxComboBox cmbType = (ASPxComboBox)HoursGridView.FindEditFormTemplateControl("cmbType");
				ASPxTimeEdit timeEdtStartTime = (ASPxTimeEdit)HoursGridView.FindEditFormTemplateControl("timeEdtStartTime");
				ASPxTextBox txtDuration = (ASPxTextBox)HoursGridView.FindEditFormTemplateControl("txtDuration");
				//ASPxTimeEdit timeEdtEndTime = (ASPxTimeEdit)HoursGridView.FindEditFormTemplateControl("timeEdtEndTime");
				ASPxTextBox txtSendTo = (ASPxTextBox)HoursGridView.FindEditFormTemplateControl("txtSendTo");
				ASPxTextBox txtCopyTo = (ASPxTextBox)HoursGridView.FindEditFormTemplateControl("txtCopyTo");
				ASPxTextBox txtBlindCopyTo = (ASPxTextBox)HoursGridView.FindEditFormTemplateControl("txtBlindCopyTo");
				//12/1/2014 NS added for VSPLUS-946
				ASPxTextBox txtSMSTo = (ASPxTextBox)HoursGridView.FindEditFormTemplateControl("txtSMSTo");
				//6/23/2015 NS added for VSPLUS-1838
				ASPxHiddenField hidden_phone1 = (ASPxHiddenField)HoursGridView.FindEditFormTemplateControl("hidden_phone1");
				//ASPxTextBox txtEscalateTo = (ASPxTextBox)HoursGridView.FindEditFormTemplateControl("txtEscalateTo");
				ASPxCheckBox chkBxSendSNMPTrap = (ASPxCheckBox)HoursGridView.FindEditFormTemplateControl("chkBxSendSNMPTrap");
				ASPxListBox lstBoxDays = (ASPxListBox)HoursGridView.FindEditFormTemplateControl("lstBoxDays");
				//4/4/2014 NS added for VSPLUS-519
				ASPxCheckBox chkboxPersistent = (ASPxCheckBox)HoursGridView.FindEditFormTemplateControl("chkboxPersistent");
				//FillTypeComboBox(cmbType);
				//12/4/2014 NS added for VSPLUS-1229
				ASPxComboBox cmbScript = (ASPxComboBox)HoursGridView.FindEditFormTemplateControl("cmbScript");
				HoursIndicator Busibject = new HoursIndicator();
				//4/22/2015 NS added - ID may not be 0, otherwise, Business Hours record with ID 0 is not treated right
				Busibject.ID = -1;
				Busibject.Type = cmbType.Text;
				DataTable returntable = VSWebBL.ConfiguratorBL.Businesshoursbl.Ins.GetName(Busibject);
				if (returntable.Rows.Count > 0)
				{
					if (returntable.Rows[0]["Starttime"] != null && returntable.Rows[0]["Starttime"] != "")
					{
						Session["starttime"] = returntable.Rows[0]["Starttime"].ToString();
					}
					if (returntable.Rows[0]["Duration"].ToString() != null && returntable.Rows[0]["Duration"].ToString() != "")
					{
						Session["Duration"] = Convert.ToInt32(returntable.Rows[0]["Duration"].ToString());
					}
					if (returntable.Rows[0]["Issunday"].ToString() != null && returntable.Rows[0]["Issunday"].ToString() != "")
					{
						sun = Convert.ToBoolean(returntable.Rows[0]["Issunday"].ToString());
					} if (returntable.Rows[0]["IsMonday"].ToString() != null && returntable.Rows[0]["IsMonday"].ToString() != "")
					{
						mon = Convert.ToBoolean(returntable.Rows[0]["IsMonday"].ToString());
					}
					if (returntable.Rows[0]["IsTuesday"].ToString() != null && returntable.Rows[0]["IsTuesday"].ToString() != "")
					{
						tue = Convert.ToBoolean(returntable.Rows[0]["IsTuesday"].ToString());
					}
					if (returntable.Rows[0]["IsWednesday"].ToString() != null && returntable.Rows[0]["IsWednesday"].ToString() != "")
					{
						wed = Convert.ToBoolean(returntable.Rows[0]["IsWednesday"].ToString());
					}
					if (returntable.Rows[0]["IsThursday"].ToString() != null && returntable.Rows[0]["IsThursday"].ToString() != "")
					{
						thu = Convert.ToBoolean(returntable.Rows[0]["IsThursday"].ToString());
					}
					if (returntable.Rows[0]["IsFriday"].ToString() != null && returntable.Rows[0]["IsFriday"].ToString() != "")
					{
						fri = Convert.ToBoolean(returntable.Rows[0]["IsFriday"].ToString());
					}
					if (returntable.Rows[0]["Issaturday"].ToString() != null && returntable.Rows[0]["Issaturday"].ToString() != "")
					{
						sat = Convert.ToBoolean(returntable.Rows[0]["Issaturday"].ToString());
					}
					//string day = " ";
					if (sun == true)
					{

						day += "Sunday, ";
					}
					if (mon == true)
					{

						day += "Monday, ";
					}
					if (tue == true)
					{

						day += "Tuesday, ";
					}
					if (wed == true)
					{
						//3/30/2015 NS modified - typo
						//day += "Wednsesday, ";
						day += "Wednesday, ";
					}
					if (thu == true)
					{

						day += "Thursday, ";
					}
					if (fri == true)
					{

						day += "Friday, ";
					}
					if (sat == true)
					{
						//3/30/2015 NS modified - proper case
						day += "Saturday";
					}
					//4/22/2015 NS added
					if (day != null && day != "")
					{
						if (day.Substring(day.Length - 1, 1) == ",")
						{
							day = day.Substring(0, day.Length - 1);
						}
					}
				}
				if (Mode == "Insert")
				{
					int maxid = 0;
					if (Hourstable.Rows.Count > 0)
					{
						Hourstable.DefaultView.Sort = "ID DESC";
						maxid = int.Parse(Hourstable.DefaultView[0]["ID"].ToString());
					}

					DataRow r = Hourstable.NewRow();
					r["ID"] = maxid + 1;
					//r["EscalateTo"] = txtEscalateTo.Text;
					if (chkBxSendSNMPTrap.Checked)
					{
						r["SendSNMPTrap"] = 1;
					}
					else
					{
						r["SendSNMPTrap"] = 0;
					}
					//4/4/2014 NS added for VSPLUS-519
					if (chkboxPersistent.Checked)
					{
						r["EnablePersistentAlert"] = 1;
					}
					else
					{
						r["EnablePersistentAlert"] = 0;
					}
					r["BlindCopyTo"] = txtBlindCopyTo.Text;
					//12/1/2014 NS added for VSPLUS-946
					//6/23/2015 NS modified for VSPLUS-1838
					if (hidden_phone1.Contains("smsto"))
					{
						r["SMSTo"] = hidden_phone1["smsto"];
					}
					else
					{
						r["SMSTo"] = "";
					}
					//// if(AlertRow["EndTime"].ToString()!=""&&AlertRow["EndTime"].ToString()!=null)
					//r["EndTime"] = timeEdtEndTime.Text;
					//6/26/2013 NS added

					// if (AlertRow["HoursIndicator"].ToString() != "" && AlertRow["HoursIndicator"].ToString() != null)
					r["HoursIndicator"] = cmbType.Text;
					// // if(AlertRow["StartTime"].ToString()!=""&&AlertRow["StartTime"].ToString()!=null)
					//r["StartTime"] = timeEdtStartTime.Text;
					r["CopyTo"] = txtCopyTo.Text;
					//if(AlertRow["AlertKey"].ToString()!=""&&AlertRow["AlertKey"].ToString()!=null)
					r["AlertKey"] = AlertKey;
					r["SendTo"] = txtSendTo.Text;
					string days = "";

					if (cmbType.Text == "All Hours" || cmbType.Text == "Specific Hours")
					{
						if (cmbType.Text == "All Hours")
						{

							r["StartTime"] = null;
							r["Duration"] = 0;
							days = ReadSettings(cmbType.Text);
							r["Day"] = days;
						}

						if (cmbType.Text == "Specific Hours")
						{

							r["StartTime"] = timeEdtStartTime.Text;
							if (txtDuration.Text != "")
							{
								r["Duration"] = Convert.ToInt32(txtDuration.Text);
							}

							if (txtDuration.Text == "0")
							{

								//r["Duration"] = 0;

								bussinesserror.InnerHtml = "Please enter duration greater than 0." +
									"<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
								bussinesserror.Style.Value = "display: block";
							}
							for (int i = 0; i < 7; i++)
							{
								if (lstBoxDays.Items[i].Selected == true)
								{
									if (lstBoxDays.SelectedItems.Count == 1)
									{
										days += lstBoxDays.SelectedItem.Text;
									}
									else
									{
										days += lstBoxDays.Items[i].Text + ", ";
									}

								}
							}

							if (lstBoxDays.SelectedItems.Count == 1)
							{
							}
							else if (lstBoxDays.SelectedItems.Count == 0)
							{
								days = "";
							}
							else
							{
								days = days.Remove(days.Length - 2);
							}
							r["Day"] = days;

						}


					}
					else
					{
						if (Session["starttime"] != null && Session["starttime"] != "")
						{
							r["StartTime"] = Session["starttime"].ToString();
						}
						if (Session["Duration"] != null && Session["Duration"] != "")
						{
							r["Duration"] = Convert.ToInt32(Session["Duration"].ToString());
						}
						r["Day"] = day;


					}
					//days = days.Remove((days.Length) - 2);
					//r["Day"] = days;
					//12/4/2014 NS added for VSPLUS-1229
					if (cmbScript.SelectedIndex != -1)
					{
						r["ScriptName"] = cmbScript.SelectedItem.Text;
						r["ScriptID"] = cmbScript.SelectedItem.Value;
					}
					else
					{
						r["ScriptName"] = "";
						r["ScriptID"] = 0;
					}
					Hourstable.Rows.Add(r);
					//5/13/2015 NS added
					Hourstable.AcceptChanges();
					Session["AlertDetails"] = Hourstable;
				}
				if (Mode == "Update")
				{

					DataRow[] dr = Hourstable.Select("ID=" + int.Parse(AlertRow["ID"].ToString()));
					string days = "";
					if (dr.Length > 0)
					{
						foreach (DataRow r in dr)
						{
							r["ID"] = int.Parse(AlertRow["ID"].ToString());
							if (AlertRow["AlertKey"].ToString() != "" && AlertRow["AlertKey"].ToString() != null)
							{
								r["AlertKey"] = int.Parse(AlertRow["AlertKey"].ToString());
							}
							//if (AlertRow["HoursIndicator"].ToString() != "" && AlertRow["HoursIndicator"].ToString() != null)
							//{
							r["HoursIndicator"] = cmbType.Text;
							//}
							r["SendTo"] = txtSendTo.Text;
							r["CopyTo"] = txtCopyTo.Text;
							r["BlindCopyTo"] = txtBlindCopyTo.Text;
							//12/1/2014 NS added for VSPLUS-946
							//6/23/2015 NS modified for VSPLUS-1838
							if (hidden_phone1.Contains("smsto"))
							{
								r["SMSTo"] = hidden_phone1["smsto"];
							}
							else
							{
								r["SMSTo"] = "";
							}
							// if (AlertRow["StartTime"].ToString() != "" && AlertRow["StartTime"].ToString() != null)

							// if (AlertRow["EndTime"].ToString() != "" && AlertRow["EndTime"].ToString() != null)
							//r["EndTime"] = timeEdtStartTime.Text;
							if (cmbType.Text == "All Hours" || cmbType.Text == "Specific Hours")
							{
								if (cmbType.Text == "All Hours")
								{

									r["StartTime"] = null;
									r["Duration"] = 0;
									days = ReadSettings(cmbType.Text);
									r["Day"] = days;

								}

								if (cmbType.Text == "Specific Hours")
								{
									r["StartTime"] = timeEdtStartTime.Text;
									r["Duration"] = Convert.ToInt32(txtDuration.Text);
									for (int i = 0; i < 7; i++)
									{
										if (lstBoxDays.Items[i].Selected == true)
										{
											if (lstBoxDays.SelectedItems.Count == 1)
											{
												days += lstBoxDays.SelectedItem.Text;
											}
											else
											{
												days += lstBoxDays.Items[i].Text + ", ";
											}
										}
									}
									if (lstBoxDays.SelectedItems.Count == 1)
									{
									}
									else if (lstBoxDays.SelectedItems.Count == 0)
									{
										days = "";
									}
									else
									{
										days = days.Remove(days.Length - 2);
									}
									r["Day"] = days;

								}
							}


							else
							{

								if (Session["starttime"] != null && Session["starttime"] != "")
								{
									r["StartTime"] = Session["starttime"].ToString();
								}
								if (Session["Duration"] != null && Session["Duration"] != "")
								{
									r["Duration"] = Convert.ToInt32(Session["Duration"].ToString());
								}
								string input = day;
								int index = input.LastIndexOf(",");
								if (index > 0)
									input = input.Substring(0, index);
								r["Day"] = input;

							}

							// r["Day"] = days;
							//12/4/2014 NS added for VSPLUS-1229
							if (cmbScript.SelectedIndex != -1)
							{
								r["ScriptName"] = cmbScript.SelectedItem.Text;
								r["ScriptID"] = cmbScript.SelectedItem.Value;
							}
							else
							{
								r["ScriptName"] = "";
								r["ScriptID"] = 0;
							}
							//r["EscalateTo"] = AlertRow["EscalateTo"].ToString();
							if (chkBxSendSNMPTrap.Checked)
							{
								r["SendSNMPTrap"] = 1;
							}
							else
							{
								r["SendSNMPTrap"] = 0;
							}
							//4/4/2014 NS added for VSPLUS-519
							if (chkboxPersistent.Checked)
							{
								r["EnablePersistentAlert"] = 1;
							}
							else
							{
								r["EnablePersistentAlert"] = 0;
							}

							//1/25/2014 NS added - the row needs to be updated in the Hourstable object before
							//the object is reassigned back to Session
							//6/11/2015 NS added for VSPLUS-1862
							Hourstable.AcceptChanges();
							Session["AlertDetails"] = Hourstable;
						}
					}
				}
			}
			catch (Exception ex)
			{
				//Log.Entry.Ins.Write(Server.MapPath("~/LogFiles/"), "VSPlusLog.txt", DateTime.Now.ToString() + " Error in Page: " +
				//    Request.Url.AbsolutePath + ", Method: " + System.Reflection.MethodBase.GetCurrentMethod().Name +
				//    ", Error: " + ex.ToString());

				//6/27/2014 NS added for VSPLUS-634
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
			}
		}
		protected void ValidationUpdatedButton_Click(object sender, EventArgs e)
		{
			try
			{
				Response.Redirect("AlertDefinitions_Grid.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
				Context.ApplicationInstance.CompleteRequest();
			}
			catch (Exception ex)
			{
				//Log.Entry.Ins.Write(Server.MapPath("~/LogFiles/"), "VSPlusLog.txt", DateTime.Now.ToString() + " Error in Page: " +
				//    Request.Url.AbsolutePath + ", Method: " + System.Reflection.MethodBase.GetCurrentMethod().Name +
				//    ", Error: " + ex.ToString());

				//6/27/2014 NS added for VSPLUS-634
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
			}
		}
		private DataTable GetSelectedEvents(int AlertKey)
		{
			DataTable dtSel = new DataTable();
			try
			{
				dtSel.Columns.Add("AlertKey");
				dtSel.Columns.Add("EventID");
				dtSel.Columns.Add("ServerTypeID");
				//10/16/2014 NS added
				dtSel.Columns.Add("ConsecutiveFailures");
				//string selValues = "";
				TreeListNodeIterator iterator = EventsTreeList.CreateNodeIterator();
				TreeListNode node;
				//TreeListColumn columnActid = EventsTreeList.Columns["actid"];
				TreeListColumn columnActid = EventsTreeList.Columns["actid"];
				TreeListColumn columnSrvId = EventsTreeList.Columns["SrvId"];
				TreeListColumn columnTbl = EventsTreeList.Columns["tbl"];
				while (true)
				{
					node = iterator.GetNext();

					if (node == null) break;


					//if (node.Level == 1 && node.ParentNode.Selected)
					//{
					//    // root node selected ie All Events selected
					//    DataRow dr = dtSel.NewRow();
					//    dr["AlertKey"] = AlertKey;
					//    dr["EventID"] = 0;// node.GetValue("actid");
					//    dr["ServerTypeID"] = 0;//node.GetValue("SrvId");
					//    dtSel.Rows.Add(dr);
					//    break;
					//}
					//else if (node.Level == 1 && node.Selected )
					//{
					//    // level 1 node selected ie One Servertype selected and all events under it

					//    DataRow dr = dtSel.NewRow();
					//    dr["AlertKey"] = AlertKey;
					//    dr["EventID"] = 0;//node.GetValue("actid");
					//    dr["ServerTypeID"] = ((System.Data.DataRowView)(node.DataItem)).Row.ItemArray[3];//node.GetValue("SrvId");
					//    dtSel.Rows.Add(dr);
					//}
					//else
					if (node.Level == 2)
					{
						if (node.Selected)
						{
							DataRow dr = dtSel.NewRow();
							dr["AlertKey"] = AlertKey;
							dr["EventID"] = node.GetValue("actid");
							dr["ServerTypeID"] = node.GetValue("SrvId");
							//10/16/2014 NS added
							dr["ConsecutiveFailures"] = node.GetValue("ConsecutiveFailures");
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

				//6/27/2014 NS added for VSPLUS-634
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
			}
			return dtSel;
		}
		private DataTable GetSelectedServers(int AlertKey)
		{

			DataTable dtSel = new DataTable();
			try
			{
				dtSel.Columns.Add("AlertKey");
				dtSel.Columns.Add("ServerID");
				dtSel.Columns.Add("LocationID");
				//11/26/2013 NS added
				dtSel.Columns.Add("ServerTypeID");

				//string selValues = "";
				TreeListNodeIterator iterator = ServersTreeList.CreateNodeIterator();
				TreeListNode node;

				TreeListColumn columnActid = ServersTreeList.Columns["actid"];
				TreeListColumn columnSrvId = ServersTreeList.Columns["LocId"];
				TreeListColumn columnTbl = ServersTreeList.Columns["tbl"];
				//11/26/2013 NS added
				TreeListColumn columnSrvTypeId = ServersTreeList.Columns["srvtypeid"];
				while (true)
				{
					node = iterator.GetNext();

					if (node == null) break;
					if (node.Level == 1 && node.ParentNode.Selected)
					{
						// root node selected ie All Servers selected
						DataRow dr = dtSel.NewRow();
						dr["AlertKey"] = AlertKey;
						dr["ServerID"] = 0;// node.GetValue("actid");
						dr["LocationID"] = 0;//node.GetValue("LocId");
						//11/26/2013 NS added
						dr["ServerTypeID"] = 0;//node.GetValue("srvtypeid");
						dtSel.Rows.Add(dr);
						break;
					}
					else if (node.Level == 1 && node.ParentNode.Selected == false && node.Selected)
					{
						// level 1 node selected ie One Location selected and all Servers under it

						DataRow dr = dtSel.NewRow();
						dr["AlertKey"] = AlertKey;
						dr["ServerID"] = 0;//node.GetValue("actid");
						dr["LocationID"] = ((System.Data.DataRowView)(node.DataItem)).Row.ItemArray[3];// node.GetValue("LocId");
						//11/26/2013 NS added
						dr["ServerTypeID"] = 0;//node.GetValue("srvtypeid");
						dtSel.Rows.Add(dr);
					}
					else if (node.Level == 2 && node.ParentNode.Selected == false) //(node.ParentNode.Selected==false)
					{

						if (node.Selected)
						{
							DataRow dr = dtSel.NewRow();
							dr["AlertKey"] = AlertKey;
							dr["ServerID"] = node.GetValue("actid");
							dr["LocationID"] = node.GetValue("LocId");
							//11/26/2013 NS added
							dr["ServerTypeID"] = node.GetValue("srvtypeid");
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

				//6/27/2014 NS added for VSPLUS-634
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
			}

			return dtSel;

		}
		protected void CollapseAllButton_Click(object sender, EventArgs e)
		{
			try
			{
				if (CollapseAllButton.Text == "Collapse All")
				{
					EventsTreeList.CollapseAll();
					CollapseAllButton.Image.Url = "~/images/icons/add.png";
					CollapseAllButton.Text = "Expand All";
				}
				else
				{
					EventsTreeList.ExpandAll();
					CollapseAllButton.Image.Url = "~/images/icons/forbidden.png";
					CollapseAllButton.Text = "Collapse All";
				}
			}
			catch (Exception ex)
			{
				//Log.Entry.Ins.Write(Server.MapPath("~/LogFiles/"), "VSPlusLog.txt", DateTime.Now.ToString() + " Error in Page: " +
				//    Request.Url.AbsolutePath + ", Method: " + System.Reflection.MethodBase.GetCurrentMethod().Name +
				//    ", Error: " + ex.ToString());

				//6/27/2014 NS added for VSPLUS-634
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
			}
		}
		protected void CollapseAllSrvButton_Click(object sender, EventArgs e)
		{
			try
			{
				if (CollapseAllSrvButton.Text == "Collapse All")
				{
					ServersTreeList.CollapseAll();
					CollapseAllSrvButton.Image.Url = "~/images/icons/add.png";
					CollapseAllSrvButton.Text = "Expand All";
				}
				else
				{
					ServersTreeList.ExpandAll();
					CollapseAllSrvButton.Image.Url = "~/images/icons/forbidden.png";
					CollapseAllSrvButton.Text = "Collapse All";
				}
			}
			catch (Exception ex)
			{
				//Log.Entry.Ins.Write(Server.MapPath("~/LogFiles/"), "VSPlusLog.txt", DateTime.Now.ToString() + " Error in Page: " +
				//    Request.Url.AbsolutePath + ", Method: " + System.Reflection.MethodBase.GetCurrentMethod().Name +
				//    ", Error: " + ex.ToString());

				//6/27/2014 NS added for VSPLUS-634
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
			}
		}
		public string ReadSettings(string strtype)
		{
			string strDays = "";

			if (strtype == "Business Hours")
			{
				string sun = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("BusinessHoursSunday");
				if (sun != "" && sun != null && sun == "1")
				{
					strDays += "Sunday, ";
				}


				string Mon = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("BusinessHoursMonday");
				if (Mon != "" && Mon != null && Mon == "1")
				{
					strDays += "Monday, ";

				}
				string Tue = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("BusinessHoursTuesday");
				if (Tue != "" && Tue != null && Tue == "1")
				{
					strDays += "Tuesday, ";


				}
				string wed = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("BusinessHoursWednesday");
				if (wed != "" && wed != null && wed == "1")
				{
					strDays += "Wednesday, ";

				}

				string Thur = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("BusinessHoursThursday");
				if (Thur != "" && Thur != null && Thur == "1")
				{
					strDays += "Thursday, ";

				}
				string Fri = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("BusinessHoursFriday");
				if (Fri != "" && Fri != null && Fri == "1")
				{
					strDays += "Friday, ";
				}


				string Sat = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("BusinessHoursSaturday");
				if (Sat != "" && Sat != null && Sat == "1")
				{
					strDays += "Saturday, ";

				}

				if (strDays != "")
				{
					strDays = strDays.Remove(strDays.Length - 2);
				}
			}
			else
			{
				strDays = "Sunday, Monday, Tuesday, Wednesday, Thursday, Friday, Saturday";
			}
			return strDays;
		}
		public void HoursGridView_PageSizeChanged(object sender, EventArgs e)
		{
			//ProfilesGridView.PageIndex;
			VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("AlertDef_Edit|HoursGridView", HoursGridView.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
			Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
		}
		//12/4/2014 NS added for VSPLUS-1229
		public void FillScriptCombobox(ASPxComboBox ScriptComboBox)
		{
			try
			{
				DataTable ScriptDataTable = VSWebBL.ConfiguratorBL.AlertsBL.Ins.GetScripts();
				ScriptComboBox.DataSource = ScriptDataTable;
				ScriptComboBox.TextField = "ScriptName";
				ScriptComboBox.ValueField = "ID";
				ScriptComboBox.DataBind();
			}
			catch (Exception ex)
			{
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
			}
		}
		protected void HoursGridView_CustomColumnDisplayText(object sender, ASPxGridViewColumnDisplayTextEventArgs e)
		{
			if (e.Column.FieldName == "Day")
			{
				//3/26/2015 NS added for DevEx upgrade to 14.2
				e.EncodeHtml = false;
				if (e.Value.ToString().Trim() != "")
				{
					e.DisplayText = string.Join(",", e.Value.ToString().Split(',').Select(str => str.Trim() == "" ? str.Trim() : str.Trim().Substring(0, 2)));
					if (e.DisplayText.Substring(e.DisplayText.Length - 1, 1) == ",")
					{
						e.DisplayText = e.DisplayText.Substring(0, e.DisplayText.Length - 1);
					}
				}
			}
		}
		//4/2/2015 NS added for VSPLUS-219
		protected void EscalationGridView_RowInserting(object sender, ASPxDataInsertingEventArgs e)
		{
			bool smsempty = false;
			try
			{
				if (Session["EscalationDetails"] != null && Session["EscalationDetails"] != "")
				{
					Etable = Session["EscalationDetails"] as DataTable;
				}
				ASPxGridView gridView = (ASPxGridView)sender;
				//6/24/2015 NS added for VSPLUS-1838
				ASPxComboBox cmbEMechanism = (ASPxComboBox)EscalationGridView.FindEditFormTemplateControl("cmbEMechanism");
				ASPxHiddenField ehidden_phone1 = (ASPxHiddenField)EscalationGridView.FindEditFormTemplateControl("ehidden_phone1");
				if (cmbEMechanism.SelectedItem.Text == "SMS")
				{
					if (ehidden_phone1.Contains("esmsto"))
					{
						//9/28/2015 NS modified for VSPLUS-2186
						if (ehidden_phone1["esmsto"] == "" || ehidden_phone1["esmsto"].ToString() == "invalid")
						{
							smsempty = true;
							throw new CustomExceptions("Please enter a valid phone number into the SMS field.");
						}
					}
				}
				//Insert row in DB
				UpdateData("Insert", GetRow(Etable, e.NewValues.GetEnumerator(), 0), "Escalation");

				//Update Grid after inserting new row, refresh grid as in page load
				gridView.CancelEdit();
				e.Cancel = true;
				fillEscalationGridfromSession();
			}
			catch (Exception ex)
			{
				//6/27/2014 NS added for VSPLUS-634
				if (smsempty)
				{
					//9/28/2015 NS modified for VSPLUS-2186
					throw new CustomExceptions("Please enter a valid phone number into the SMS field.");
				}
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
			}
		}
		public void FillEscalationGrid()
		{
			try
			{
				AlertNames AlertNameobj = new AlertNames();
				AlertNameobj.AlertName = AlertNameTextBox.Text;
				DataTable Escalationtable = new DataTable();
				Escalationtable = VSWebBL.ConfiguratorBL.AlertsBL.Ins.GetEscalationDetails(AlertNameobj);
				if (Escalationtable.Rows.Count > 0)
				{
					Escalationtable.PrimaryKey = new DataColumn[] { Escalationtable.Columns["ID"] };
				}
				Session["EscalationDetails"] = Escalationtable;
				EscalationGridView.DataSource = Escalationtable;
				EscalationGridView.DataBind();
			}
			catch (Exception ex)
			{
				//6/27/2014 NS added for VSPLUS-634
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
			}
		}
		public void fillEscalationGridfromSession()
		{
			DataTable Escalationtable = new DataTable();
			try
			{
				if (Session["EscalationDetails"] != "" && Session["EscalationDetails"] != null)
					Escalationtable = Session["EscalationDetails"] as DataTable;
				if (Escalationtable.Rows.Count > 0)
				{
					Escalationtable.PrimaryKey = new DataColumn[] { Escalationtable.Columns["ID"] };
				}
				EscalationGridView.DataSource = Escalationtable;
				EscalationGridView.DataBind();
			}
			catch (Exception ex)
			{
				//6/27/2014 NS added for VSPLUS-634
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
			}
		}
		private void CollectDataEscalate(string Mode, DataRow EscalateRow)
		{
			try
			{
				DataTable Escalationtable = Session["EscalationDetails"] as DataTable;

				ASPxTextBox txtEDuration = (ASPxTextBox)EscalationGridView.FindEditFormTemplateControl("txtEDuration");
				ASPxTextBox txtESendTo = (ASPxTextBox)EscalationGridView.FindEditFormTemplateControl("txtESendTo");
				ASPxTextBox txtESMSTo = (ASPxTextBox)EscalationGridView.FindEditFormTemplateControl("txtESMSTo");
				//6/23/2015 NS added for VSPLUS-1838
				ASPxHiddenField ehidden_phone1 = (ASPxHiddenField)EscalationGridView.FindEditFormTemplateControl("ehidden_phone1");
				ASPxComboBox cmbEScript = (ASPxComboBox)EscalationGridView.FindEditFormTemplateControl("cmbEScript");
				if (Mode == "Insert")
				{
					int maxid = 0;
					if (Escalationtable.Rows.Count > 0)
					{
						Escalationtable.DefaultView.Sort = "ID DESC";
						maxid = int.Parse(Escalationtable.DefaultView[0]["ID"].ToString());
					}

					DataRow r = Escalationtable.NewRow();
					r["ID"] = maxid + 1;
					//6/23/2015 NS modified for VSPLUS-1838
					//r["SMSTo"] = txtESMSTo.Text;
					if (ehidden_phone1.Contains("esmsto"))
					{
						r["SMSTo"] = ehidden_phone1["esmsto"];
					}
					else
					{
						r["SMSTo"] = "";
					}
					r["EscalateTo"] = txtESendTo.Text;
					r["EscalationInterval"] = Convert.ToInt32(txtEDuration.Text);
					if (cmbEScript.SelectedIndex != -1)
					{
						r["ScriptName"] = cmbEScript.SelectedItem.Text;
						r["ScriptID"] = cmbEScript.SelectedItem.Value;
					}
					else
					{
						r["ScriptName"] = "";
						r["ScriptID"] = 0;
					}
					Escalationtable.Rows.Add(r);
					//5/13/2015 NS added for VSPLUS-1763
					Escalationtable.AcceptChanges();
					Session["EscalationDetails"] = Escalationtable;
				}
				if (Mode == "Update")
				{
					DataRow[] dr = Escalationtable.Select("ID=" + int.Parse(EscalateRow["ID"].ToString()));
					if (dr.Length > 0)
					{
						foreach (DataRow r in dr)
						{
							r["ID"] = int.Parse(EscalateRow["ID"].ToString());
							if (EscalateRow["AlertKey"].ToString() != "" && EscalateRow["AlertKey"].ToString() != null)
							{
								r["AlertKey"] = int.Parse(EscalateRow["AlertKey"].ToString());
							}
							r["EscalateTo"] = txtESendTo.Text;
							//6/23/2015 NS modified for VSPLUS-1838
							//r["SMSTo"] = txtESMSTo.Text;
							if (ehidden_phone1.Contains("esmsto"))
							{
								r["SMSTo"] = ehidden_phone1["esmsto"];
							}
							else
							{
								r["SMSTo"] = "";
							}
							r["EscalationInterval"] = Convert.ToInt32(txtEDuration.Text);
							if (cmbEScript.SelectedIndex != -1)
							{
								r["ScriptName"] = cmbEScript.SelectedItem.Text;
								r["ScriptID"] = cmbEScript.SelectedItem.Value;
							}
							else
							{
								r["ScriptName"] = "";
								r["ScriptID"] = 0;
							}
							Session["EscalationDetails"] = Escalationtable;
						}
					}
				}
			}
			catch (Exception ex)
			{
				//6/27/2014 NS added for VSPLUS-634
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
			}
		}
		public void InsertEscalationData()
		{
			if (Session["EscalationDetails"] != null)
			{
				try
				{
					//6/24/2015 NS added
					List<int> list = new List<int>();
					bool result = false;
					bool result2 = false, result3 = true;
					DataTable Escalationtable = Session["EscalationDetails"] as DataTable;
					DataTable dt = new DataTable();
					EscalationDetails escalationObj = new EscalationDetails();
					AlertNames AltObj = new AlertNames();
					AltObj.AlertName = AlertNameTextBox.Text;
					if (flag == "Update")
					{
						AltObj.AlertKey = AlertKey;
					}
					try
					{
						if (flag == "Insert")
						{
							for (int i = 0; i < Escalationtable.Rows.Count; i++)
							{
								escalationObj.EscalateTo = Escalationtable.Rows[i]["EscalateTo"].ToString();
								escalationObj.EscalationInterval = Convert.ToInt32(Escalationtable.Rows[i]["EscalationInterval"].ToString());
								escalationObj.SMSTo = Escalationtable.Rows[i]["SMSTo"].ToString();
								escalationObj.ScriptID = Convert.ToInt32(Escalationtable.Rows[i]["ScriptID"].ToString());
								dt = VSWebBL.ConfiguratorBL.AlertsBL.Ins.GetDataByAlertName(collectAlertKey());//brijesh
								escalationObj.AlertKey = int.Parse(dt.Rows[0]["AlertKey"].ToString());//brijesh
								AlertKey = escalationObj.AlertKey;
								result2 = VSWebBL.ConfiguratorBL.AlertsBL.Ins.InsertEscalationDetails(escalationObj);//brijesh  
							}
						}

						if (flag == "Update")
						{
							int i, j, l = 0;
							DataTable dt2 = VSWebBL.ConfiguratorBL.AlertsBL.Ins.GetEscalationDetailsData(AlertKey);
							if (dt2.Rows.Count > 0)
							{
								for (i = 0; i < dt2.Rows.Count; i++)
								{
									int dtID = int.Parse(dt2.Rows[i]["ID"].ToString());
									list.Add(dtID);
									for (j = i; j < Escalationtable.Rows.Count; j++, l = j)
									{
										if (int.Parse(Escalationtable.Rows[j]["ID"].ToString()) != 0)
										{
											int escalationTableID = int.Parse(Escalationtable.Rows[j]["ID"].ToString());
											if (dtID == escalationTableID)
											{
												escalationObj.EscalateTo = Escalationtable.Rows[j]["EscalateTo"].ToString();
												escalationObj.EscalationInterval = Convert.ToInt32(Escalationtable.Rows[j]["EscalationInterval"].ToString());
												escalationObj.SMSTo = Escalationtable.Rows[j]["SMSTo"].ToString();
												escalationObj.AlertKey = AlertKey;
												escalationObj.ID = Convert.ToInt32(Escalationtable.Rows[j]["ID"].ToString());
												escalationObj.ScriptID = Convert.ToInt32(Escalationtable.Rows[j]["ScriptID"].ToString());
												result3 = VSWebBL.ConfiguratorBL.AlertsBL.Ins.UpdateEscalationDetails(escalationObj);
											}
										}
									}
								}
								if (dt2.Rows.Count < Escalationtable.Rows.Count)
								{
									//l = l - 1;
									for (int x = 0; x < Escalationtable.Rows.Count; x++)
									{
										int escalationTableID = int.Parse(Escalationtable.Rows[x]["ID"].ToString());
										if (!list.Contains(escalationTableID))
										{
											escalationObj.EscalateTo = Escalationtable.Rows[x]["EscalateTo"].ToString();
											escalationObj.EscalationInterval = Convert.ToInt32(Escalationtable.Rows[x]["EscalationInterval"].ToString());
											escalationObj.SMSTo = Escalationtable.Rows[x]["SMSTo"].ToString();
											escalationObj.AlertKey = AlertKey;
											escalationObj.ScriptID = Convert.ToInt32(Escalationtable.Rows[x]["ScriptID"].ToString());
											result3 = VSWebBL.ConfiguratorBL.AlertsBL.Ins.InsertEscalationDetails(escalationObj);
										}
									}
								}
							}
							else
							{
								for (int k = 0; k < Escalationtable.Rows.Count; k++)
								{
									escalationObj.EscalateTo = Escalationtable.Rows[k]["EscalateTo"].ToString();
									escalationObj.EscalationInterval = Convert.ToInt32(Escalationtable.Rows[k]["EscalationInterval"].ToString());
									escalationObj.SMSTo = Escalationtable.Rows[k]["SMSTo"].ToString();
									escalationObj.AlertKey = AlertKey;
									escalationObj.ScriptID = Convert.ToInt32(Escalationtable.Rows[k]["ScriptID"].ToString());
									result3 = VSWebBL.ConfiguratorBL.AlertsBL.Ins.InsertEscalationDetails(escalationObj);
								}
							}
						}
					}
					catch (Exception ex)
					{
						Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
					}
				}
				catch (Exception ex)
				{
					//6/27/2014 NS added for VSPLUS-634
					Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				}
			}
		}
		protected void EscalationGridView_RowUpdating(object sender, ASPxDataUpdatingEventArgs e)
		{
			bool smsempty = false;
            //11/4/2015 NS added for VSPLUS-2186
            bool smsinvalid = false;
			try
			{
				if (Session["EscalationDetails"] != null && Session["EscalationDetails"] != "")
					Etable = Session["EscalationDetails"] as DataTable;
				ASPxGridView gridView = (ASPxGridView)sender;
				//6/24/2015 NS added for VSPLUS-1838
				ASPxComboBox cmbEMechanism = (ASPxComboBox)EscalationGridView.FindEditFormTemplateControl("cmbEMechanism");
				ASPxHiddenField ehidden_phone1 = (ASPxHiddenField)EscalationGridView.FindEditFormTemplateControl("ehidden_phone1");
				if (cmbEMechanism.SelectedItem.Text == "SMS")
				{
					if (ehidden_phone1.Contains("esmsto"))
					{
						if (ehidden_phone1["esmsto"] == "")
						{
							smsempty = true;
							throw new Exception("Please enter a phone number into the SMS field.");
						}
                        //11/4/2015 NS added for VSPLUS-2186
                        else if (ehidden_phone1["esmsto"].ToString() == "invalid")
                        {
                            smsinvalid = true;
                            throw new Exception("Please enter a valid phone number into the SMS field.");
                        }
					}
				}
				gridView.DoRowValidation();

				UpdateData("Update", GetRow(Etable, e.NewValues.GetEnumerator(), Convert.ToInt32(e.Keys[0])), "Escalation");
				//Update Grid after inserting new row, refresh grid as in page load
				gridView.CancelEdit();
				e.Cancel = true;
				fillEscalationGridfromSession();
			}
			catch (Exception ex)
			{
				//6/27/2014 NS added for VSPLUS-634
				if (smsempty)
				{
					throw new Exception("Please enter a phone number into the SMS field.");
				}
                else if (smsinvalid)
                {
                    throw new Exception("Please enter a valid phone number into the SMS field.");
                }
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
			}

		}
		protected void EscalationGridView_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
		{
			try
			{
				string strSMSTo = "";
				ASPxTextBox txtEDuration = (ASPxTextBox)EscalationGridView.FindEditFormTemplateControl("txtEDuration");
				ASPxLabel lblESendTo = (ASPxLabel)EscalationGridView.FindEditFormTemplateControl("lblESendTo");
				ASPxTextBox txtESendTo = (ASPxTextBox)EscalationGridView.FindEditFormTemplateControl("txtESendTo");
				ASPxLabel lblESMSTo = (ASPxLabel)EscalationGridView.FindEditFormTemplateControl("lblESMSTo");
				//ASPxTextBox txtESMSTo = (ASPxTextBox)EscalationGridView.FindEditFormTemplateControl("txtESMSTo");
				ASPxLabel lblEScript = (ASPxLabel)EscalationGridView.FindEditFormTemplateControl("lblEScript");
				ASPxComboBox cmbEScript = (ASPxComboBox)EscalationGridView.FindEditFormTemplateControl("cmbEScript");
				ASPxComboBox cmbEMechanism = (ASPxComboBox)EscalationGridView.FindEditFormTemplateControl("cmbEMechanism");
				if (e.RowType == GridViewRowType.EditForm)
				{
					FillScriptCombobox(cmbEScript);

					if (!((DevExpress.Web.ASPxGridView)(sender)).IsNewRowEditing)
						txtEDuration.Value = e.GetValue("EscalationInterval");
					if (!((DevExpress.Web.ASPxGridView)(sender)).IsNewRowEditing)
						txtESendTo.Value = e.GetValue("EscalateTo");
					//if (!((DevExpress.Web.ASPxGridView)(sender)).IsNewRowEditing)
					//    txtESMSTo.Value = e.GetValue("SMSTo");
					if (!((DevExpress.Web.ASPxGridView)(sender)).IsNewRowEditing)
					{
						strSMSTo = e.GetValue("SMSTo").ToString();
						if (strSMSTo != "")
						{
							eSMSFromNumber = e.GetValue("SMSTo").ToString();
						}
					}
					HtmlGenericControl divControl = (HtmlGenericControl)EscalationGridView.FindEditFormTemplateControl("divESendto");
					divControl.InnerHtml = " <input type='tel' id='ephone' value='" + strSMSTo + "'  runat='server'>" +
											"<input type='hidden' id='ehidden_phone' name='ehidden_phone' value='" + strSMSTo + "' runat='server' /><br><br>" +
											"<div><span id='e-valid-msg' class='hide'>✓ Valid</span>" +
											"<span id='e-error-msg' class='hide'>Invalid number</span></div>";
					if (strSMSTo != "")
					{
						divControl.Style.Value = "display: block";
						//7/1/2015 NS added for VSPLUS-1894
						HtmlGenericControl divSMSControl = (HtmlGenericControl)EscalationGridView.FindEditFormTemplateControl("divESMSToMsg");
						bool isconfig = false;
						isconfig = Convert.ToBoolean(Session["SMSConfig"]);
						if (!isconfig)
						{
							divSMSControl.Style.Value = "display: block";
						}
						else
						{
							divSMSControl.Style.Value = "display: none";
						}
					}
					if (!((DevExpress.Web.ASPxGridView)(sender)).IsNewRowEditing)
					{
						for (int i = 0; i < cmbEScript.Items.Count; i++)
						{
							if (cmbEScript.Items[i].Text == e.GetValue("ScriptName").ToString())
							{
								cmbEScript.SelectedIndex = i;
							}
						}
					}
					if (txtESendTo.Text != "")
					{
						cmbEMechanism.SelectedIndex = 0;
						lblEScript.ClientVisible = false;
						cmbEScript.ClientVisible = false;
						lblESMSTo.ClientVisible = false;
						//txtESMSTo.ClientVisible = false;
						lblESendTo.ClientVisible = true;
						txtESendTo.ClientVisible = true;
					}
					else if (strSMSTo != "")
					{
						cmbEMechanism.SelectedIndex = 1;
						lblESendTo.ClientVisible = false;
						txtESendTo.ClientVisible = false;
						lblEScript.ClientVisible = false;
						cmbEScript.ClientVisible = false;
						lblESMSTo.ClientVisible = true;
						//txtESMSTo.ClientVisible = true;
					}
					else if (cmbEScript.SelectedIndex >= 0)
					{
						cmbEMechanism.SelectedIndex = 2;
						lblESendTo.ClientVisible = false;
						txtESendTo.ClientVisible = false;
						lblESMSTo.ClientVisible = false;
						//txtESMSTo.ClientVisible = false;
						lblEScript.ClientVisible = true;
						cmbEScript.ClientVisible = true;
					}
				}
			}
			catch (Exception ex)
			{
				//6/27/2014 NS added for VSPLUS-634
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
			}
		}
		protected void EscalationGridView_RowDeleting(object sender, ASPxDataDeletingEventArgs e)
		{
			try
			{
				DataTable Etable = Session["EscalationDetails"] as DataTable;
				DataRow[] dr = Etable.Select("ID=" + Convert.ToInt32(e.Keys[0]));
				foreach (DataRow r in dr)
				{
					r.Delete();
					r.AcceptChanges();
				}
				EscalationDetails escalationObj = new EscalationDetails();
				escalationObj.ID = Convert.ToInt32(e.Keys[0]);
				//VSWebBL.ConfiguratorBL.AlertsBL.Ins.DeleteEscalationData(escalationObj);
				Session["EscalationDetails"] = Etable;
                Session["Ekey"] = escalationObj.ID;
				ASPxGridView gridview = (ASPxGridView)sender;
				gridview.CancelEdit();
				e.Cancel = true;
				//fillEscalationGridfromSession();
			}
			catch (Exception ex)
			{
				//Log.Entry.Ins.Write(Server.MapPath("~/LogFiles/"), "VSPlusLog.txt", DateTime.Now.ToString() + " Error in Page: " +
				//    Request.Url.AbsolutePath + ", Method: " + System.Reflection.MethodBase.GetCurrentMethod().Name +
				//    ", Error: " + ex.ToString());

				//6/27/2014 NS added for VSPLUS-634
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
			}
		}
		protected void HoursGridView_CustomUnboundColumnData(object sender, ASPxGridViewColumnDataEventArgs e)
		{
			if (e.Column.FieldName == "SendToAll")
			{
				string sendTo = "";
				string smsTo = "";
				string scriptName = "";
				if (e.GetListSourceFieldValue("SendTo") != null)
				{
					sendTo = ConvertFromDBVal<string>(e.GetListSourceFieldValue("SendTo"));
				}
				if (e.GetListSourceFieldValue("SMSTo") != null)
				{
					smsTo = ConvertFromDBVal<string>(e.GetListSourceFieldValue("SMSTo"));
				}
				if (e.GetListSourceFieldValue("ScriptName") != null)
				{
					scriptName = ConvertFromDBVal<string>(e.GetListSourceFieldValue("ScriptName"));
				}
				if (sendTo != null && sendTo != "")
				{
					e.Value = sendTo;
				}
				else if (smsTo != null && smsTo != "")
				{
					e.Value = smsTo + " (SMS)";
				}
				else if (scriptName != null && scriptName != "")
				{
					e.Value = scriptName + " (script)";
				}
			}
		}
		protected void EscalationGridView_CustomUnboundColumnData(object sender, ASPxGridViewColumnDataEventArgs e)
		{
			if (e.Column.FieldName == "EscalateToAll")
			{
				string sendTo = "";
				string smsTo = "";
				string scriptName = "";
				if (e.GetListSourceFieldValue("EscalateTo") != null)
				{
					sendTo = ConvertFromDBVal<string>(e.GetListSourceFieldValue("EscalateTo"));
				}
				if (e.GetListSourceFieldValue("SMSTo") != null)
				{
					smsTo = ConvertFromDBVal<string>(e.GetListSourceFieldValue("SMSTo"));
				}
				if (e.GetListSourceFieldValue("ScriptName") != null)
				{
					scriptName = ConvertFromDBVal<string>(e.GetListSourceFieldValue("ScriptName"));
				}
				if (sendTo != null && sendTo != "")
				{
					e.Value = sendTo;
				}
				else if (smsTo != null && smsTo != "")
				{
					e.Value = smsTo + " (SMS)";
				}
				else if (scriptName != null && scriptName != "")
				{
					e.Value = scriptName + " (script)";
				}
			}
		}
		//6/23/2015 NS added
		public static T ConvertFromDBVal<T>(object obj)
		{
			if (obj == null || obj == DBNull.Value)
			{
				return default(T); // returns the default value for the type
			}
			else
			{
				return (T)obj;
			}
		}
		protected void EventsTreeList_PageSizeChanged(object sender, EventArgs e)
		{
			//ProfilesGridView.PageIndex;
			VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("AlertDef_Edit|EventsGridView", EventsTreeList.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
			Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
		}
		protected void EventsTreeList2_PageSizeChanged(object sender, EventArgs e)
		{
			//ProfilesGridView.PageIndex;
			VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("AlertDef_Edit|EventsGridView", EventsTreeList.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
			Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
		}
		protected void ServersTreeList_PageSizeChanged(object sender, EventArgs e)
		{
			//ProfilesGridView.PageIndex;
			VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("AlertDef_Edit|ServersGridView", ServersTreeList.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
			Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
		}
		protected void HoursGridView_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
		{
			if (((ASPxGridView)sender).IsEditing)
			{
				e.Properties["cpIsEdit"] = true;
			}
			else
			{
				e.Properties["cpIsEdit"] = false;
			}
			e.Properties["cpSMSTxtTo"] = SMSFromNumber;
			//7/1/2015 NS added for VSPLUS-1894
			e.Properties["cpSMSConfig"] = Convert.ToBoolean(Session["SMSConfig"]);
		}
		protected void EscalationGridView_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
		{
			if (((ASPxGridView)sender).IsEditing)
			{
				e.Properties["cpIsEdit"] = true;
			}
			else
			{
				e.Properties["cpIsEdit"] = false;
			}
			e.Properties["cpSMSTxtTo"] = eSMSFromNumber;
			e.Properties["cpSMSConfig"] = Convert.ToBoolean(Session["SMSConfig"]);
		}
		//7/1/2015 NS added for VSPLUS-1894
		public bool isSMSConfigured()
		{
			bool isconfig = false;
			string SMSAccountSid = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("SMSAccountSid");
			string SMSAuthToken = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("SMSAuthToken");
			string SMSFrom = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("SMSFrom");
			if (SMSAccountSid != null && SMSAccountSid != "" && SMSAuthToken != null && SMSAuthToken != "" && SMSFrom != null && SMSFrom != "")
			{
				isconfig = true;
			}
			return isconfig;
		}
		private void FillAlerteventsComboBox()
		{
			DataTable AlerteventnamesNamesdt = VSWebBL.ConfiguratorBL.AlertsBL.Ins.GetAllDataForalerteventNames();
			AlerteventsComboBox.DataSource = AlerteventnamesNamesdt;
			Session["Alerteventnames"] = AlerteventnamesNamesdt;
			AlerteventsComboBox.TextField = "Name";
			AlerteventsComboBox.ValueField = "ID";
			AlerteventsComboBox.DataBind();

			AlerteventsComboBox.Items.Insert(0, new ListEditItem("None"));

		}
		private void FillAlerteventsComboBoxfromsession()
		{
			DataTable dt = new DataTable();

			if (Session["Alerteventnames"] != "" && Session["Alerteventnames"] != null)
			{
				dt = Session["Alerteventnames"] as DataTable;

				AlerteventsComboBox.DataSource = dt;
				AlerteventsComboBox.TextField = "Name";
				AlerteventsComboBox.ValueField = "ID";
				AlerteventsComboBox.DataBind();
				//if (flag == "Insert")
				//{
				AlerteventsComboBox.Items.Insert(0, new ListEditItem("None", "-1"));
				//}
				//AlerteventsComboBox.Items.Add("None", "");


			}
		}
		protected void NewEventTemplate_Click(object sender, EventArgs e)
		{
			try
			{
				Response.Redirect("AlertEventTemplate_Edit.aspx", false);
				Context.ApplicationInstance.CompleteRequest();
			}
			catch (Exception ex)
			{

				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
			}
		}
		protected void AlerteventsComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			//VSPLUS:1669 create multiple alerttemplates
			evet1.Visible = true;

			Session["DataEvents2"] = null;
			if (AlerteventsComboBox.SelectedIndex >= 0)
			{
				EventsTreeList.UnselectAll();
			}

			if (AlerteventsComboBox.Text == "None")
			{
				profilename = "None";//AlerteventsComboBox.SelectedItem.Text;
				profileid = 0;
			}
			else
			{
				profilename = AlerteventsComboBox.SelectedItem.Text;
				Session["profilename"] = profilename;
				DataTable templateids = VSWebBL.ConfiguratorBL.AlertsBL.Ins.GetEventsTemplateidbyNames(profilename);
				profileid = Convert.ToInt32(templateids.Rows[0]["ID"]);
			}
			Session["profilename"] = profilename;

			Session["profileid"] = profileid;


			fillEventsTreeList();
			fillServersTreeList();

		}

		protected void HoursGridView_CustomErrorText(object sender, ASPxGridViewCustomErrorTextEventArgs e)
		{
			Exception ex = e.Exception;
			if (ex != null)
			{

				if (ex.Message.IndexOf("ErrCode!-1") != -1)
				{
					e.ErrorText = ex.Message;
				}
				else if (ex.Message.IndexOf("ErrCode=-1") > 0)
				{
					e.ErrorText = "some error";
				}
				else
				{
					e.ErrorText = ex.Message;
				}
			}
		}

		protected void EscalationGridView_CustomErrorText(object sender, ASPxGridViewCustomErrorTextEventArgs e)
		{
			Exception ex = e.Exception;
			if (ex != null)
			{

				if (ex.Message.IndexOf("ErrCode!-1") != -1)
				{
					e.ErrorText = ex.Message;
				}
				else if (ex.Message.IndexOf("ErrCode=-1") > 0)
				{
					e.ErrorText = "some error";
				}
				else
				{
					e.ErrorText = ex.Message;
				}
			}
		}
		public void EventsTreelist()
		{
			DataEventsTree = VSWebBL.ConfiguratorBL.AlertsBL.Ins.GetEventsFromProcedure();

			var Servertypes = from n in DataEventsTree.AsEnumerable()
							  where n.Field<string>("tbl").Contains("ServerTypes")
							  select n;
			var Events = from n in DataEventsTree.AsEnumerable()
						 where n.Field<string>("tbl").Contains("EventsMaster")
						 select n;
			DataTable filteredDT = null;
			DataTable Eventsdt = null;
			DataTable Servertypesdt = null;
			DataTable sum = new DataTable();
			if ((Servertypes != null && Servertypes.Count() > 0) && (Events != null && Events.Count() > 0))
			{
				Servertypesdt = Servertypes.CopyToDataTable();
				Eventsdt = Events.CopyToDataTable();
				sum = Servertypesdt.Copy();
				sum.Merge(Eventsdt);
				//var customers = sum.AsEnumerable();
				//	var orders = Locationsdt.AsEnumerable();
				var LocationIdNotNull = from n in sum.AsEnumerable()
										where !n.IsNull("SrvId")
										select n;
				filteredDT = LocationIdNotNull.CopyToDataTable();
				DataTable filterbylocation = (from n in Servertypesdt.AsEnumerable()
											  join prod in filteredDT.AsEnumerable() on n.Field<int>("id") equals prod.Field<int>("SrvId")
											  select n).Distinct().CopyToDataTable();
				DataTable Filteredservers = new DataTable();
				Filteredservers = filterbylocation.Copy();
				Filteredservers.Merge(filteredDT);
				Session["DataEvents2"] = Filteredservers;
			}
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















