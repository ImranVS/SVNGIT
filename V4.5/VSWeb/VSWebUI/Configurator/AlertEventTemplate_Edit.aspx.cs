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

namespace VSWebUI.Configurator
{
	public partial class AlertEventTemplate_Edit : System.Web.UI.Page
	{
		int ID;
		string lastid;
		bool returns = true;
		string Mode;
		DataTable dtSel;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				Session["DataEvents"] = null;
				Session["DataServers"] = null;
				if (Request.QueryString["Mode"] != null)
				{
					string Mode = Request.QueryString["Mode"].ToString();
				}

				if (Request.QueryString["Key"] != null)
				{

					Mode = "update";
					ID = Convert.ToInt32(Request.QueryString["Key"].ToString());
					Session["id"] = ID;
					filldata(Convert.ToInt32(Session["id"].ToString()));
				}
				else
				{
					Mode = "insert";
				}
				if (Session["UserPreferences"] != null)
				{
					DataTable UserPreferences = (DataTable)Session["UserPreferences"];
					foreach (DataRow dr in UserPreferences.Rows)
					{

						if (dr[1].ToString() == "AlertEventTemplate_Edit|EventsTreeList")
						{
							EventsTreeList.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
						}

					}
				}

				//fillEventsTreeList();

			}
			else
			{
				fillEventsTreefromSession();
			}
			if (!IsPostBack)
			{
				fillEventsTreeList();
				EventsTreeList.CollapseAll();
			}
			else
			{
				//fillEventsTreeList();
				fillEventsTreefromSession();
				EventsTreeList.CollapseAll();
			}
		}
		//public void fillEventsTreeList()
		//{
		//    try
		//    {

		//        EventsTreeList.CollapseAll();
		//        CollapseAllButton.Image.Url = "~/images/icons/add.png";
		//        CollapseAllButton.Text = "Expand All";
		//        if (Session["DataEvents"] == null)
		//        {
		//            DataTable DataEventsTree = VSWebBL.ConfiguratorBL.AlertsBL.Ins.GetEventsFromProcedure();
		//            Session["DataEvents"] = DataEventsTree;
		//        }
		//        EventsTreeList.DataSource = (DataTable)Session["DataEvents"];
		//        EventsTreeList.DataBind();

		//        DataTable dtSel = VSWebBL.ConfiguratorBL.AlertsBL.Ins.GetSelectedEventsfortemplate(Convert.ToInt32(Session["id"].ToString()));
		//        string s = dtSel.Rows[0]["EventID"].ToString();
		//        string[] words = s.Split(',');

		//        DataTable dt = new DataTable();


		//        dt.Columns.Add("EventID");


		//        foreach (string str in words)
		//        {
		//            DataRow drow = dt.NewRow();   // Here you will get an actual instance of a DataRow
		//            drow["EventID"] = str;   // Assign values 
		//            dt.Rows.Add(drow);             // Don't forget to add the row to the DataTable.             
		//        }
		//        if (dtSel.Rows.Count > 0)
		//        {
		//            TreeListNodeIterator iterator = EventsTreeList.CreateNodeIterator();
		//            TreeListNode node;
		//            for (int i = 0; i < dtSel.Rows.Count; i++)
		//            {
		//                if (Convert.ToInt32(dtSel.Rows[i]["EventID"]) == 0 && Convert.ToInt32(dtSel.Rows[i]["ServerTypeID"]) == 0)
		//                {
		//                    //select all
		//                    while (true)
		//                    {
		//                        node = iterator.GetNext();
		//                        if (node == null) break;
		//                        node.Selected = true;
		//                    }
		//                }
		//                else if (Convert.ToInt32(dtSel.Rows[i]["EventID"]) == 0 && (Convert.ToInt32(dtSel.Rows[i]["ServerTypeID"]) != 0))
		//                {
		//                    //parent selected
		//                    while (true)
		//                    {
		//                        node = iterator.GetNext();
		//                        if (node == null) break;
		//                        if ((Convert.ToInt32(dtSel.Rows[i]["ServerTypeID"]) == Convert.ToInt32(node.GetValue("actid"))) && node.GetValue("tbl").ToString() == "ServerTypes")
		//                        {
		//                            node.Selected = true;
		//                        }
		//                        else if (node.GetValue("SrvId").ToString() != "")
		//                        {
		//                            if ((Convert.ToInt32(dtSel.Rows[i]["ServerTypeID"]) == Convert.ToInt32(node.GetValue("SrvId"))) && node.GetValue("tbl").ToString() != "ServerTypes")
		//                            {
		//                                node.Selected = true;
		//                            }
		//                        }
		//                    }
		//                }
		//                else if (Convert.ToInt32(dtSel.Rows[i]["EventID"]) != 0 && (Convert.ToInt32(dtSel.Rows[i]["ServerTypeID"]) != 0))
		//                {
		//                    //specific selected
		//                    while (true)
		//                    {

		//                        node = iterator.GetNext();
		//                        if (node == null) break;
		//                        if ((Convert.ToInt32(dtSel.Rows[i]["EventID"]) == Convert.ToInt32(node.GetValue("actid"))) && node.GetValue("tbl").ToString() != "ServerTypes")
		//                        {
		//                            node.Selected = true;
		//                        }

		//                    }
		//                }
		//                iterator.Reset();
		//            }
		//        }
		//    }
		//    catch (Exception ex)
		//    {
		//        //Log.Entry.Ins.Write(Server.MapPath("~/LogFiles/"), "VSPlusLog.txt", DateTime.Now.ToString() + " Error in Page: " +
		//        //    Request.Url.AbsolutePath + ", Method: " + System.Reflection.MethodBase.GetCurrentMethod().Name +
		//        //    ", Error: " + ex.ToString());

		//        //6/27/2014 NS added for VSPLUS-634
		//        Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
		//    }
		//}
		public void fillEventsTreeList()
		{
			int j;
			try
			{
				Session["DataEvents"] = null;
				EventsTreeList.ClearNodes();
				EventsTreeList.RefreshVirtualTree();
				EventsTreeList.CollapseAll();
				CollapseAllButton.Image.Url = "~/images/icons/add.png";
				CollapseAllButton.Text = "Expand All";
				if (Session["DataEvents"] == null)
				{
					DataTable DataEventsTree = VSWebBL.ConfiguratorBL.AlertsBL.Ins.GetEventsFromProcedure();
					Session["DataEvents"] = DataEventsTree;
				}
				EventsTreeList.DataSource = (DataTable)Session["DataEvents"];
				EventsTreeList.DataBind();
				if (Request.QueryString["Key"] != null)
				{
					dtSel = VSWebBL.ConfiguratorBL.AlertsBL.Ins.GetSelectedEventsfortemplate(Convert.ToInt32(Session["id"].ToString()));
					//for (j = 0; j < dtSel.Rows.Count; j++)
					//    {
					//        string eventsid = dtSel.Rows[j]["ServerTypeID"].ToString();
					//        lastid += eventsid + ",";

					//    }

					//    while (lastid.EndsWith(","))
					//        lastid = lastid.Substring(0, lastid.Length - 1);
					//    Session["lastid2"] = lastid;

					if (dtSel.Rows.Count > 0)
					{
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
		public void filldata(int ID)
		{
			DataTable dt = VSWebBL.ConfiguratorBL.AlertsBL.Ins.GetEventsTemplateNmaes(ID);
			AlertEventtb.Text = dt.Rows[0]["Name"].ToString();
		}
		public void fillEventsTreefromSession()
		{

			DataTable DataEvents = new DataTable();
			try
			{
				if (Session["DataEvents"] != "" && Session["DataEvents"] != null)
					DataEvents = (DataTable)Session["DataEvents"];
				if (DataEvents.Rows.Count > 0)
				{
					DataEvents.PrimaryKey = new DataColumn[] { DataEvents.Columns["ID"] };
				}
				EventsTreeList.DataSource = DataEvents;
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
		protected void EventsTreeList_PageSizeChanged(object sender, EventArgs e)
		{
			//ProfilesGridView.PageIndex;
			VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("AlertEventTemplate_Edit|EventsTreeList", EventsTreeList.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
			Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
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


					if (node.Level == 1 && node.ParentNode.Selected)
					{
						// root node selected ie All Events selected
						DataRow dr = dtSel.NewRow();
						//dr["AlertKey"] = AlertKey;
						dr["EventID"] = 0;// node.GetValue("actid");
						dr["ServerTypeID"] = 0;//node.GetValue("SrvId");
						dtSel.Rows.Add(dr);
						break;
					}
					else if (node.Level == 1 && node.ParentNode.Selected == false && node.Selected)
					{
						// level 1 node selected ie One Servertype selected and all events under it
						if (node.Selected)
						{
							DataRow dr = dtSel.NewRow();
							//dr["AlertKey"] = AlertKey;
							dr["EventID"] = node.GetValue("actid");
							dr["ServerTypeID"] = ((System.Data.DataRowView)(node.DataItem)).Row.ItemArray[3];
							dtSel.Rows.Add(dr);
						}
					}
					else if (node.Level == 2 && node.ParentNode.Selected == false)
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
		//private DataTable GetSelectedEvents(int AlertKey)
		//{
		//    DataTable dtSel = new DataTable();
		//    try
		//    {
		//        dtSel.Columns.Add("AlertKey");
		//        dtSel.Columns.Add("EventID");
		//        dtSel.Columns.Add("ServerTypeID");
		//        //10/16/2014 NS added
		//        dtSel.Columns.Add("ConsecutiveFailures");
		//        //string selValues = "";
		//        TreeListNodeIterator iterator = EventsTreeList.CreateNodeIterator();
		//        TreeListNode node;
		//        //TreeListColumn columnActid = EventsTreeList.Columns["actid"];
		//        TreeListColumn columnActid = EventsTreeList.Columns["actid"];
		//        TreeListColumn columnSrvId = EventsTreeList.Columns["SrvId"];
		//        TreeListColumn columnTbl = EventsTreeList.Columns["tbl"];
		//        while (true)
		//        {
		//            node = iterator.GetNext();

		//            if (node == null) break;


		//            if (node.Level == 1 && node.ParentNode.Selected)
		//            {
		//                // root node selected ie All Events selected
		//                DataRow dr = dtSel.NewRow();
		//                dr["AlertKey"] = AlertKey;
		//                dr["EventID"] = 0;// node.GetValue("actid");
		//                dr["ServerTypeID"] = 0;//node.GetValue("SrvId");
		//                dtSel.Rows.Add(dr);
		//                break;
		//            }
		//            else if (node.Level == 1 && node.ParentNode.Selected == false && node.Selected)
		//            {
		//                // level 1 node selected ie One Servertype selected and all events under it

		//                DataRow dr = dtSel.NewRow();
		//                dr["AlertKey"] = AlertKey;
		//                dr["EventID"] = 0;//node.GetValue("actid");
		//                dr["ServerTypeID"] = ((System.Data.DataRowView)(node.DataItem)).Row.ItemArray[3];//node.GetValue("SrvId");
		//                dtSel.Rows.Add(dr);
		//            }
		//            else if (node.Level == 2 && node.ParentNode.Selected == false)
		//            {
		//                if (node.Selected)
		//                {
		//                    DataRow dr = dtSel.NewRow();
		//                    dr["AlertKey"] = AlertKey;
		//                    dr["EventID"] = node.GetValue("actid");
		//                    dr["ServerTypeID"] = node.GetValue("SrvId");
		//                    //10/16/2014 NS added
		//                    dr["ConsecutiveFailures"] = node.GetValue("ConsecutiveFailures");
		//                    dtSel.Rows.Add(dr);
		//                }
		//            }
		//        }


		//    }
		//    catch (Exception ex)
		//    {
		//        //Log.Entry.Ins.Write(Server.MapPath("~/LogFiles/"), "VSPlusLog.txt", DateTime.Now.ToString() + " Error in Page: " +
		//        //    Request.Url.AbsolutePath + ", Method: " + System.Reflection.MethodBase.GetCurrentMethod().Name +
		//        //    ", Error: " + ex.ToString());

		//        //6/27/2014 NS added for VSPLUS-634
		//        Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
		//    }
		//    return dtSel;
		//}


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
					//else if (node.Level == 1 && node.ParentNode.Selected == false && node.Selected)
					//{
					//    // level 1 node selected ie One Servertype selected and all events under it

					//    DataRow dr = dtSel.NewRow();
					//    dr["AlertKey"] = AlertKey;
					//    dr["EventID"] = node.GetValue("actid");
					//    dr["ServerTypeID"] = ((System.Data.DataRowView)(node.DataItem)).Row.ItemArray[3];//node.GetValue("SrvId");
					//    dtSel.Rows.Add(dr);
					//}
					//else 
					if (node.Level == 2)// && node.ParentNode.Selected == false)
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
		protected void OKButton_Click(object sender, EventArgs e)
		{

			int i = 0;


			//DataTable dt = GetSelectedEvents();
			DataTable dt = GetSelectedEvents(1);
			//DataTable dt1 = GetSelectedServers(1);
			int count = dt.Rows.Count;
			if (dt.Rows.Count > 0)
			{
				errorDiv4.Style.Value = "display: none";
				for (i = 0; i < count; i++)
				{
					string eventsid = dt.Rows[i]["EventID"].ToString();
					lastid += eventsid + ",";


				}

				while (lastid.EndsWith(","))
					lastid = lastid.Substring(0, lastid.Length - 1);
				//lastid = lastid.Remove(lastid.Length - 1);

				InsertAlertEventsData();
				
			}
			else
			{
				errorDiv4.InnerHtml = "Select at least one event." +
								  "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
				errorDiv4.Style.Value = "display: block";
			}
		}
		protected void CancelButton_Click(object sender, EventArgs e)
		{
			Response.Redirect("~/Configurator/AlertEventTemplate.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
			Context.ApplicationInstance.CompleteRequest();
		}
		public void InsertAlertEventsData()
		{
			string mode = Request.QueryString["Mode"].ToString();
			if (Request.QueryString["Mode"] != null)
			{
				if (Request.QueryString["Mode"].ToString() == "insert")
				{

					returns = VSWebBL.ConfiguratorBL.AlertsBL.Ins.InserttemplateSelectedEvents(AlertEventtb.Text, lastid);
					if (returns.ToString() == "True")
					{
						Session["eventtemplatename"] = AlertEventtb.Text;
						Response.Redirect("AlertEventTemplate.aspx?Mode=" + mode, false);
						Context.ApplicationInstance.CompleteRequest();

					}
					else if (returns.ToString() == "False")
					{
						errorDiv4.InnerHtml = "Event template already exists." +
								  "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
						errorDiv4.Style.Value = "display: block";
					}


				}
				else
				{
					if (Request.QueryString["Name"] != null)
					{
						string name = Request.QueryString["Name"].ToString();
						Session["Name"] = name;
						//if (Session["Name"].ToString() != AlertEventtb.Text)
						//{
							returns = VSWebBL.ConfiguratorBL.AlertsBL.Ins.UpdatetemplateSelectedEvents(AlertEventtb.Text, lastid, Convert.ToInt32(Session["id"].ToString()), Session["Name"].ToString());
							if (returns.ToString() == "True")
							{
								Session["eventtemplatename"] = AlertEventtb.Text;
								Response.Redirect("AlertEventTemplate.aspx?Mode=" + mode, false);
								Context.ApplicationInstance.CompleteRequest();



							}
							else if (returns.ToString() == "False")
							{
								errorDiv4.InnerHtml = "Event template already exists." +
										  "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
								errorDiv4.Style.Value = "display: block";
							}
						//}
						//else 
						//{
						//    errorDiv4.InnerHtml = "Event template already exists." +
						//                  "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
						//    errorDiv4.Style.Value = "display: block";
						//}
					}
				}

			}
		}
		protected void MaintResetButton_Click(object sender, EventArgs e)
		{
			if (Request.UrlReferrer != null)


				// Response.Redirect(Request.UrlReferrer, false);
				// Response.Redirect(Request.UrlReferrer);


				Response.Redirect(Request.RawUrl);
		}
	}
}