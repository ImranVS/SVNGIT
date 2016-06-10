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
	public partial class DominoELS_Edit : System.Web.UI.Page
	{
		DataTable DELSDataTable = null;
		DataTable Dtable = null;
		object msgloc = "";
		string flag, day;
		int EventKey;
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
			errorDiv.Style.Value = "display: none";

			if (Request.QueryString["EventKey"] != null && Request.QueryString["EventKey"] != "")
			{
				flag = "update";
				EventKey = Convert.ToInt16(Request.QueryString["EventKey"].ToString());
				Session["ID"] = EventKey;
				if (!IsPostBack)
				{
                    Session["key"] = null;
					fillData(EventKey);
				}

			}
			else
				flag = "insert";

			if (!IsPostBack)
			{
				//if (flag == "update")
				//    fillData(EventKey);
				FillDELSGrid();
				fillServersTreeList();
				ServersTreeList.CollapseAll();
				if (Session["UserPreferences"] != null)
				{
					DataTable UserPreferences = (DataTable)Session["UserPreferences"];
					foreach (DataRow dr in UserPreferences.Rows)
					{
						if (dr[1].ToString() == "DELS_Edit|DELSGridView")
						{
							DELSGridView.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
						}
					}
				}
			}
			else
			{
				FillDELSGridfromSession();
				fillServersTreefromSession();
				ServersTreeList.CollapseAll();


			}

		}

		public void fillData(int Key)
		{
			try
			{

				string elsName = VSWebBL.ConfiguratorBL.DELSBL.Ins.GetEventNamebyKey(Key);
				Session["Name"] = elsName;
				EventNameTextBox.Text = elsName;
			}
			catch (Exception ex)
			{
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
			}
		}

		private void FillDELSGrid()
		{
			try
			{
				DominoEventLog nameObj = new DominoEventLog();
				nameObj.Name = EventNameTextBox.Text;
				DELSDataTable = new DataTable();
				DELSDataTable = VSWebBL.ConfiguratorBL.DELSBL.Ins.GetDELSData(nameObj);

				//if (DELSDataTable.Rows.Count > 0)
				//{
				//    DELSDataTable.PrimaryKey = new DataColumn[] { DELSDataTable.Columns["ID"] };
				//}
				DataTable dtcopy = DELSDataTable.Copy();
				dtcopy.PrimaryKey = new DataColumn[] { dtcopy.Columns["ID"] };
				Session["DELStable"] = dtcopy;
				DELSGridView.Selection.IsRowSelected(1);

				DELSGridView.DataSource = DELSDataTable;
				DELSGridView.DataBind();

				//if (EventKey != 0)
				//{
				//    DataTable ELSDataTable1 = new DataTable();
				//    ELSDataTable1 = VSWebBL.ConfiguratorBL.ELSBL.Ins.GetSelectedEventsForKey(EventKey);
				//    foreach (DataRow r in ELSDataTable1.Rows)
				//    {
				//        int startIndex = DELSGridView.PageIndex * DELSGridView.SettingsPager.PageSize;
				//        int endIndex = Math.Min(DELSGridView.VisibleRowCount, startIndex + DELSGridView.SettingsPager.PageSize);
				//        for (int i = startIndex; i < endIndex; i++)
				//        {
				//            DataRow row = DELSGridView.GetDataRow(i);
				//            if (Convert.ToInt32(row[1]) == Convert.ToInt32(r[0]))
				//                DELSGridView.Selection.SelectRow(i);
				//        }
				//    }

				//}

			}
			catch (Exception ex)
			{
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}
			finally { }
		}
		private void FillDELSGridfromSession()
		{
			try
			{

				DELSDataTable = new DataTable();
				if (Session["DELStable"] != "" && Session["DELStable"] != null)
					DELSDataTable = (DataTable)Session["DELStable"];
				if (DELSDataTable.Rows.Count > 0)
				{
					DELSDataTable.PrimaryKey = new DataColumn[] { DELSDataTable.Columns["ID"] };
				}
				
					DELSGridView.DataSource = DELSDataTable;
					DELSGridView.DataBind();
				
			}
			catch (Exception ex)
			{
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}
			finally { }
		}

		private void UpdateEventMasterData(string Mode, DataRow EventMasterRow)
		{

			//int DominoEventLogId = Convert.ToInt32(Session["DominoEventLogId"].ToString());

			if (Mode == "Insert")
			{
				int DominoEventLogId = VSWebBL.ConfiguratorBL.DELSBL.Ins.GetDominoEventLogIdbyName(EventNameTextBox.Text);
				//Object ReturnValue = VSWebBL.ConfiguratorBL.DELSBL.Ins.InsertData(CollectDataForELS(Mode, EventMasterRow), DominoEventLogId);
				//if (ReturnValue == "")
				//{
				//    msgloc = "The Event you entered already exist.";
				//}
			}
			if (Mode == "Update")
			{
				bool ReturnValue = VSWebBL.ConfiguratorBL.DELSBL.Ins.UpdateData(CollectDataForELS(Mode, EventMasterRow));
				if (ReturnValue == true)
				{
					successDiv.InnerHtml = "Event  information for <b>" + Session["ELSUpdateStatus"].ToString() +
							"</b> updated successfully." +
							"<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
					successDiv.Style.Value = "display: block";
				}
			}

		}

		private LogFile CollectDataForELS(string Mode, DataRow DELSRow)
		{
			VSFramework.TripleDES tripleDes = new VSFramework.TripleDES();
			try
			{
				//ELSMaster EventMasterObject = new ELSMaster();
				LogFile LogObject = new LogFile();
				LogObject.Keyword = DELSRow["Keyword"].ToString();
				LogObject.NotRequiredKeyword = DELSRow["NotRequiredKeyword"].ToString();
				LogObject.RepeatOnce = Convert.ToBoolean(DELSRow["RepeatOnce"]);
				LogObject.Log = Convert.ToBoolean(DELSRow["Log"]);
				LogObject.AgentLog = Convert.ToBoolean(DELSRow["AgentLog"]);

				//LogObject.DominoEventLogId = Convert.ToInt32(DELSRow["DominoEventLogId"]);

				//EventMasterObject.EventName = ELSRow["EventName"].ToString();
				//EventMasterObject.AliasName = ELSRow["AliasName"].ToString();
				//Session["ELSUpdateStatus"] = EventMasterObject.AliasName;
				//EventMasterObject.EventKey = ELSRow["EventKey"].ToString();

				//EventMasterObject.EventId = ELSRow["EventId"].ToString();
				//EventMasterObject.EventLevel = ELSRow["EventLevel"].ToString();

				//EventMasterObject.Source = ELSRow["Source"].ToString();
				//EventMasterObject.TaskCategory = ELSRow["TaskCategory"].ToString();

				if (Mode == "Update")
					LogObject.ID = int.Parse(DELSRow["ID"].ToString());

				return LogObject;
			}
			catch (Exception ex)
			{
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}
			finally { }
		}

		protected DataRow GetRow(DataTable LocObject, IDictionaryEnumerator enumerator, int Keys)
		{
			DataTable dataTable = LocObject;
			DataRow DRRow = null;


			if (Keys == -1)
				DRRow = dataTable.NewRow();
			else
				DRRow = dataTable.Rows.Find(Keys);

			enumerator.Reset();
			while (enumerator.MoveNext())
				DRRow[enumerator.Key.ToString()] = (enumerator.Value == null ? "" : enumerator.Value);
			return DRRow;
		}

		protected void DELSGridView_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
		{

			DELSDataTable = (DataTable)Session["DELStable"];

			
			ASPxTextBox txtKeyword = (ASPxTextBox)DELSGridView.FindEditFormTemplateControl("LogFileTextBox");
			ASPxTextBox txtNotRequiredKeyword = (ASPxTextBox)DELSGridView.FindEditFormTemplateControl("NotLogFileTextBox");
            ASPxCheckBox LogFileCheckBox = (ASPxCheckBox)DELSGridView.FindEditFormTemplateControl("LogFileCheckBox");
			ASPxCheckBox logCheckBox = (ASPxCheckBox)DELSGridView.FindEditFormTemplateControl("logCheckBox");
			ASPxCheckBox AgentlogCheckBox = (ASPxCheckBox)DELSGridView.FindEditFormTemplateControl("AgentlogCheckBox");

			//UpdateEventMasterData("Insert", GetRow(DELSDataTable, e.NewValues.GetEnumerator(), -1));
			try
			{
				if (Session["DELStable"] != null && Session["DELStable"] != "")
				{
					DELSDataTable = Session["DELStable"] as DataTable;
				}
				ASPxGridView gridView = (ASPxGridView)sender;

				UpdateData("Insert", GetRow(DELSDataTable, e.NewValues.GetEnumerator(), 0));

				gridView.CancelEdit();
				e.Cancel = true;

				FillDELSGridfromSession();
			}
			catch (Exception ex)
			{

				errorDiv.InnerHtml = msgloc.ToString() +
					"<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
				errorDiv.Style.Value = "display: block";
			}

		}

		private DataTable GetSelectedServers(int EventKey)
		{

			DataTable dtSel = new DataTable();
			try
			{
				dtSel.Columns.Add("DominoEventLogId");
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
					//if (node.Level == 1 && node.ParentNode.Selected)
					//{
					//    // root node selected ie All Servers selected
					//    DataRow dr = dtSel.NewRow();
					//    dr["DominoEventLogId"] = EventKey;
					//    dr["ServerID"] = 0;// node.GetValue("actid");
					//    dr["LocationID"] = 0;//node.GetValue("LocId");
					//    //11/26/2013 NS added
					//    dr["ServerTypeID"] = 0;//node.GetValue("srvtypeid");
					//    dtSel.Rows.Add(dr);
					//    break;
					//}
					//else if (node.Level == 1 && node.ParentNode.Selected == false && node.Selected)
					//{
					//    // level 1 node selected ie One Location selected and all Servers under it

					//    DataRow dr = dtSel.NewRow();
					//    dr["DominoEventLogId"] = EventKey;
					//    dr["ServerID"] = 0;//node.GetValue("actid");
					//    dr["LocationID"] = ((System.Data.DataRowView)(node.DataItem)).Row.ItemArray[3];// node.GetValue("LocId");
					//    //11/26/2013 NS added
					//    dr["ServerTypeID"] = 0;//node.GetValue("srvtypeid");
					//    dtSel.Rows.Add(dr);
					//}
					//else 
						if (node.Level == 2 ) //(node.ParentNode.Selected==false)
					{

						if (node.Selected)
						{
							DataRow dr = dtSel.NewRow();
							dr["DominoEventLogId"] = EventKey;
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
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
			}

			return dtSel;

		}

		protected void DELSGridView_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
		{
            //5/10/2016 Sowjanya modified for VSPLUS-2939

            try
            {
                DataTable Htable = Session["DELStable"] as DataTable;


                DataRow[] dr = Htable.Select("ID=" + Convert.ToInt32(e.Keys[0]));
                foreach (DataRow r in dr)
                {
                  
                    r.Delete();

                    r.AcceptChanges();
                }

                LogFile LocObject = new LogFile();
                LocObject.ID = Convert.ToInt32(e.Keys[0]);
                int id = Convert.ToInt32(e.Keys[0]);
                Session["DELStable"] = Htable;
               
               if (Session["key"] != null)
                {
                    Session["key"] +=","+ id.ToString();
                  
                }
                else
                {
                    Session["key"] += id.ToString();
                }
             

                ASPxGridView gridview = (ASPxGridView)sender;
                gridview.CancelEdit();
                e.Cancel = true;

            }
            catch
            {

            }
		}

		protected void DELSGridView_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
		{
			try
			{
				DELSDataTable = (DataTable)Session["DELStable"] ;
				if (Session["DELSDataTable"] != null && Session["DELSDataTable"] != "")
					DELSDataTable = Session["DELSDataTable"] as DataTable;
				ASPxGridView gridView = (ASPxGridView)sender;
				//6/24/2015 NS added for VSPLUS-1838

				ASPxTextBox txtKeyword = (ASPxTextBox)DELSGridView.FindEditFormTemplateControl("LogFileTextBox");
				ASPxTextBox txtNotRequiredKeyword = (ASPxTextBox)DELSGridView.FindEditFormTemplateControl("NotLogFileTextBox");
				ASPxCheckBox LogFileCheckBox = (ASPxCheckBox)DELSGridView.FindEditFormTemplateControl("LogFileCheckBox");
				ASPxCheckBox logCheckBox = (ASPxCheckBox)DELSGridView.FindEditFormTemplateControl("logCheckBox");
				ASPxCheckBox AgentlogCheckBox = (ASPxCheckBox)DELSGridView.FindEditFormTemplateControl("AgentlogCheckBox");
				
				gridView.DoRowValidation();

				UpdateData("Update", GetRow(DELSDataTable, e.NewValues.GetEnumerator(), Convert.ToInt32(e.Keys[0])));
				//Update Grid after inserting new row, refresh grid as in page load
				gridView.CancelEdit();
				e.Cancel = true;
                FillDELSGridfromSession();
			}
			catch (Exception ex)
			{


				successDiv.InnerHtml = msgloc.ToString() + "Success." +
					"<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
				successDiv.Style.Value = "display: block";
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
					DataTable DataServersTree = VSWebBL.ConfiguratorBL.DELSBL.Ins.GetServersFromProcedure();
					Session["DataServers"] = DataServersTree;
				}
				ServersTreeList.DataSource = (DataTable)Session["DataServers"];
				ServersTreeList.DataBind();

				DataTable dtSel = VSWebBL.ConfiguratorBL.DELSBL.Ins.GetSelectedServers(EventKey);
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
								if ((Convert.ToInt32(dtSel.Rows[i]["ServerID"]) == Convert.ToInt32(node.GetValue("actid"))) &&
									node.GetValue("tbl").ToString() != "Locations")
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
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
			}
		}

		protected void OKButton_Click(object sender, EventArgs e)
		{
			//insert/update event name first
			try{
				DataTable dt = GetSelectedServers(1);

				DataTable DELStable2 = (DataTable)Session["DELStable"];


				if (dt.Rows.Count > 0 && DELStable2.Rows.Count > 0)
				{
					InsertAlertData(flag);
					//5/13/2015 NS modified for VSPLUS-1763
					//4/2/2015 NS added for VSPLUS-219
                    LogFile LocObject = new LogFile();

                    //5/10/2016 Sowjanya modified for VSPLUS-2939

                    if (Session["key"] != null && Session["key"] != "")
                    {
                      
                        {
                            string m = Session["key"].ToString();
                            
                            string[] ids = m.Split(',');
                           
                            for (int i = 0; i < ids.Length; i++)
                            {
                                LocObject.ID = Convert.ToInt32(ids[i]);
                              
                                 Object ReturnValue = VSWebBL.ConfiguratorBL.DELSBL.Ins.DeleteData(LocObject);
                            }
                            Session["DELStable"] = DELSDataTable;
                            FillDELSGridfromSession();
                            Session["key"] = null;
                        }
                    }
                    Session["EventName"] = EventNameTextBox.Text;
					Response.Redirect("DominoELSDefinitions_Grid.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
					Context.ApplicationInstance.CompleteRequest();
				}
				else
                {//3/5/2015 Durga modified for VSPLUS-2910
                    if (dt.Rows.Count == 0 && DELStable2.Rows.Count == 0)
                    {
                        errorDiv.Style.Value = "display: block";
                        //10/3/2014 NS modified for VSPLUS-990
                        //11/13/2015 NS modified for VSPLUS-2355
                        errorDiv.InnerHtml = "Please create at least one Domino Event Log entry and select at least one  Server." +
                            "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                    }
                    //  else if (dt.Rows.Count == 0 && DELStable2.Rows.Count > 0 )
                    else if (dt.Rows.Count > 0 && DELStable2.Rows.Count == 0)
                    {
                        errorDiv.Style.Value = "display: block";
                        errorDiv.InnerHtml = "Please create at least one Domino Event Log entry." +
                           "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                    }
                    else if (dt.Rows.Count == 0 && DELStable2.Rows.Count > 0)
                    {
                        errorDiv.Style.Value = "display: block";
                        errorDiv.InnerHtml = "Please select at least one  Server." +
                          "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
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
			
			  // bool ReturnValue = VSWebBL.ConfiguratorBL.DELSBL.Ins.updateEventName(EventKey, EventNameTextBox.Text);
			
		}
		protected void CancelButton_Click(object sender, EventArgs e)
		{
			Response.Redirect("DominoELSDefinitions_Grid.aspx", false);
			Context.ApplicationInstance.CompleteRequest();

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
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
			}
		}

		protected void ServersTreeList_PageSizeChanged(object sender, EventArgs e)
		{
			VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("ELSDef_Edit|ServersGridView", ServersTreeList.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
			Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
		}
		public void InsertAlertData(string flag)
		{
			try
			{

				if (Session["DELStable"] != null)
				{
					//6/24/2015 NS added
					List<int> list = new List<int>();
					bool result = false;

					bool result2 = false, result3 = true;
					DataTable DELStable = Session["DELStable"] as DataTable;
					DataTable dt = new DataTable();
					//AlertNames AltObj = new AlertNames();
					//AlertDetails alertObj = new AlertDetails();
					LogFile logObj = new LogFile();
					DominoEventLog NameObj = new DominoEventLog();

					NameObj.Name = EventNameTextBox.Text;
					NameObj.ID = EventKey;
					DataTable returntab = VSWebBL.ConfiguratorBL.DELSBL.Ins.GetDataByAlertName(NameObj);

					if (returntab.Rows.Count > 0)
					{
						//ErrorMessagePopupControl.ShowOnPageLoad = true;
						//ErrorMessageLabel.Text = "The Alert Name you entered already exists. Please enter a different name.";
						errorDiv.Style.Value = "display: block";

						errorDiv.InnerHtml = "The Event Name you entered already exists. Please enter a different name." +
							"<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
					}
					else
					{

						if (flag == "insert")
						{
							//result = VSWebBL.ConfiguratorBL.AlertsBL.Ins.InsertAlertName();
							result = VSWebBL.ConfiguratorBL.DELSBL.Ins.insertEventName(EventNameTextBox.Text);
							for (int i = 0; i < DELStable.Rows.Count; i++)
							{
								logObj.Keyword = DELStable.Rows[i]["Keyword"].ToString();
								logObj.NotRequiredKeyword = DELStable.Rows[i]["NotRequiredKeyword"].ToString();
								logObj.RepeatOnce = Convert.ToBoolean(DELStable.Rows[i]["RepeatOnce"].ToString());
								logObj.Log = Convert.ToBoolean(DELStable.Rows[i]["Log"].ToString());
								logObj.AgentLog = Convert.ToBoolean(DELStable.Rows[i]["AgentLog"].ToString());


								 EventKey = VSWebBL.ConfiguratorBL.DELSBL.Ins.GetDominoEventLogIdbyName(EventNameTextBox.Text);
								//dt = VSWebBL.ConfiguratorBL.AlertsBL.Ins.GetDataByAlertName(collectAlertKey());//brijesh
								//alertObj.AlertKey = int.Parse(dt.Rows[0]["AlertKey"].ToString());//brijesh
								logObj.DominoEventLogId = EventKey;
								//AlertKey = alertObj.AlertKey;
								result2 = VSWebBL.ConfiguratorBL.DELSBL.Ins.InsertData(logObj);
								//result2 = VSWebBL.ConfiguratorBL.AlertsBL.Ins.InsertAlertDetails(alertObj);//brijesh  
							}
						}
						else
						{

							if (flag == "update")
							{

								int i, j, l = 0;
							     //	logObj.ID = Convert.ToInt32(DELStable.Rows[0]["ID"].ToString());
								//result = VSWebBL.ConfiguratorBL.AlertsBL.Ins.UpdateName(Session["ID"]);
								result = VSWebBL.ConfiguratorBL.DELSBL.Ins.updateEventName(EventKey, EventNameTextBox.Text);
								DataTable dt2 = VSWebBL.ConfiguratorBL.DELSBL.Ins.GetLogFilesData(EventKey);
								if (dt2.Rows.Count > 0)
								{
									for (i = 0; i < dt2.Rows.Count; i++)
									{
										int dtID = int.Parse(dt2.Rows[i]["ID"].ToString());
										list.Add(dtID);
										for (j = i; j < DELStable.Rows.Count; j++, l = j)
										{
											if (int.Parse(DELStable.Rows[j]["ID"].ToString()) != 0)
											{
												int hoursTableID = int.Parse(DELStable.Rows[j]["ID"].ToString());
												if (dtID == hoursTableID)
												{
                                                    //19/05/2016 sowmya added for VSPLUS-2932
                                                    logObj.ID = Convert.ToInt32(DELStable.Rows[j]["ID"].ToString());
													logObj.Keyword = DELStable.Rows[j]["Keyword"].ToString();
													logObj.NotRequiredKeyword = DELStable.Rows[j]["NotRequiredKeyword"].ToString();
													logObj.RepeatOnce = Convert.ToBoolean(DELStable.Rows[j]["RepeatOnce"].ToString());
													logObj.Log = Convert.ToBoolean(DELStable.Rows[j]["Log"].ToString());
													logObj.AgentLog = Convert.ToBoolean(DELStable.Rows[j]["AgentLog"].ToString());
													logObj.DominoEventLogId = EventKey;
													result3 = VSWebBL.ConfiguratorBL.DELSBL.Ins.UpdateLogfileDetails(logObj);
												}
											}
										}
									}
									if (dt2.Rows.Count < DELStable.Rows.Count)
									{
										for (int x = 0; x < DELStable.Rows.Count; x++)
										{
											int DELStableID = int.Parse(DELStable.Rows[x]["ID"].ToString());
											if (!list.Contains(DELStableID))
											{
                                                //19/05/2016 sowmya added for VSPLUS-2932
                                                logObj.ID = Convert.ToInt32(DELStable.Rows[x]["ID"].ToString());
												logObj.Keyword = DELStable.Rows[x]["Keyword"].ToString();
												logObj.NotRequiredKeyword = DELStable.Rows[x]["NotRequiredKeyword"].ToString();
												logObj.RepeatOnce = Convert.ToBoolean(DELStable.Rows[x]["RepeatOnce"].ToString());
												logObj.Log = Convert.ToBoolean(DELStable.Rows[x]["Log"].ToString());
												logObj.AgentLog = Convert.ToBoolean(DELStable.Rows[x]["AgentLog"].ToString());
												logObj.DominoEventLogId = EventKey;
												result3 = VSWebBL.ConfiguratorBL.DELSBL.Ins.InsertData(logObj);
											}
										}
									}
								}
								else
								{
									for (int k = 0; k < DELStable.Rows.Count; k++)
									{
                                        //19/05/2016 sowmya added for VSPLUS-2932
                                        logObj.ID = Convert.ToInt32(DELStable.Rows[k]["ID"].ToString());
										logObj.Keyword = DELStable.Rows[k]["Keyword"].ToString();
										logObj.NotRequiredKeyword = DELStable.Rows[k]["NotRequiredKeyword"].ToString();
										logObj.RepeatOnce = Convert.ToBoolean(DELStable.Rows[k]["RepeatOnce"].ToString());
										logObj.Log = Convert.ToBoolean(DELStable.Rows[k]["Log"].ToString());
										logObj.AgentLog = Convert.ToBoolean(DELStable.Rows[k]["AgentLog"].ToString());
										logObj.DominoEventLogId = EventKey;

										result3 = VSWebBL.ConfiguratorBL.DELSBL.Ins.InsertData(logObj);
									}
								}

							}



						}

						bool retnServers = false, retnHours = false;
						if (result == true || result2 == true || result3 == true)
						{
							//Servers & locations
							string EventName = EventNameTextBox.Text;
							EventKey = VSWebBL.ConfiguratorBL.DELSBL.Ins.GetDominoEventLogIdbyName(EventNameTextBox.Text);
							//retnServerTypes = VSWebBL.ConfiguratorBL.AlertsBL.Ins.InsertRestrictedServerTypes(AlertName, RestrictedServertypes());
							//retnEvents = VSWebBL.ConfiguratorBL.AlertsBL.Ins.InsertRestrictedEvents(AlertName, RestrictedEvents());
							//retnLocations = VSWebBL.ConfiguratorBL.AlertsBL.Ins.InsertRestrictedLocations(AlertName, RestrictedLocations());
							//retnServers = VSWebBL.ConfiguratorBL.AlertsBL.Ins.InsertRestrictedServers(AlertName, RestrictedServers());

							//loop
							//retnHours = VSWebBL.ConfiguratorBL.AlertsBL.Ins.InsertHours(AlertName, Hourstable);
							//retnHours = true;
							//retnEvents = VSWebBL.ConfiguratorBL.AlertsBL.Ins.InsertSelectedEvents(AlertKey, GetSelectedEvents(AlertKey));
							retnServers = VSWebBL.ConfiguratorBL.DELSBL.Ins.InsertSelectedServers(EventKey, GetSelectedServers(EventKey));

						}

						if (retnServers == true)
						{
							if (flag == "insert")
							{
								ErrorMessageLabel.Text = "New record created successfully.";
							}
							if (flag == "update")
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
				}
			}

			catch (Exception ex)
			{
			}
		}
		
	
		

		protected void DELSGridView_CustomErrorText(object sender, ASPxGridViewCustomErrorTextEventArgs e)
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
		protected void DELSGridView_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
		{

		}
		private void UpdateData(string Mode, DataRow UsersRow)
		{
			try{
					CollectData(Mode, UsersRow);
				
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
		private void CollectData(string Mode, DataRow DELSRow)
		{
			try
			{
				DataTable DELSDataTable = Session["DELStable"] as DataTable;
				LogFile LogFileObject = new LogFile();
				//ContentPlaceHolder cph = (ContentPlaceHolder)Master.FindControl("ContentPlaceHolder1");

				//ASPxGridView gv = (ASPxGridView)cph.FindControl("DELSGridView");
				//cph.FindControl("LogFileGridView");
				ASPxTextBox txtKeyword = (ASPxTextBox)DELSGridView.FindEditFormTemplateControl("LogFileTextBox");
				ASPxTextBox txtNotRequiredKeyword = (ASPxTextBox)DELSGridView.FindEditFormTemplateControl("NotLogFileTextBox");

				ASPxCheckBox LogFileCheckBox = (ASPxCheckBox)DELSGridView.FindEditFormTemplateControl("LogFileCheckBox");
				ASPxCheckBox logCheckBox = (ASPxCheckBox)DELSGridView.FindEditFormTemplateControl("logCheckBox");
				ASPxCheckBox AgentlogCheckBox = (ASPxCheckBox)DELSGridView.FindEditFormTemplateControl("AgentlogCheckBox");
				LogFileObject.Keyword = txtKeyword.Text;
				LogFileObject.NotRequiredKeyword = txtNotRequiredKeyword.Text;

				//if (LogFileCheckBox.Checked == true)
				//{
				//    LogFileObject.RepeatOnce = true;
				//}
				//else
				//{
				//    LogFileObject.RepeatOnce = false;
				//}
				//if (logCheckBox.Checked == true)
				//{
				//    LogFileObject.Log = true;
				//}
				//else
				//{
				//    LogFileObject.Log = false;
				//}
				//if (AgentlogCheckBox.Checked == true)
				//{
				//    LogFileObject.AgentLog = true;
				//}
				//else
				//{
				//    LogFileObject.AgentLog = false;
				//}


				if (Mode == "Insert")
				{
					int maxid = 0;
					if (DELSDataTable.Rows.Count > 0)
					{
						DELSDataTable.DefaultView.Sort = "ID DESC";
						maxid = int.Parse(DELSDataTable.DefaultView[0]["ID"].ToString());
					}

					DataRow r = DELSDataTable.NewRow();
					r["ID"] = maxid + 1;


					r["Keyword"] = txtKeyword.Text;
					r["NotRequiredKeyword"] = txtNotRequiredKeyword.Text;
					if (LogFileCheckBox.Checked)
					{
						r["RepeatOnce"] = 1;
					}
					else
					{
						r["RepeatOnce"] = 0;
					}
					if (logCheckBox.Checked)
					{
						r["Log"] = 1;
					}
					else
					{

						r["Log"] = 0;
					}
					if (AgentlogCheckBox.Checked)
					{
						r["AgentLog"] = 1;
					}
					else
					{

						r["AgentLog"] = 0;
					}
					DELSDataTable.Rows.Add(r);

					DELSDataTable.AcceptChanges();
					Session["DELStable"] = DELSDataTable;
				}

				if (Mode == "Update")
				{


					DataRow[] dr = DELSDataTable.Select("ID=" + int.Parse(DELSRow["ID"].ToString()));
					//string days = "";
					if (dr.Length > 0)
					{
						foreach (DataRow r in dr)
						{
							r["ID"] = int.Parse(DELSRow["ID"].ToString());
							//if (DELSRow["AlertKey"].ToString() != "" && DELSRow["AlertKey"].ToString() != null)
							//{
							//    r["AlertKey"] = int.Parse(DELSRow["AlertKey"].ToString());
							//}

							r["Keyword"] = txtKeyword.Text;
							r["NotRequiredKeyword"] = txtNotRequiredKeyword.Text;

							if (LogFileCheckBox.Checked)
							{
								r["RepeatOnce"] = 1;
							}
							else
							{
								r["RepeatOnce"] = 0;
							}
							if (logCheckBox.Checked)
							{
								r["Log"] = 1;
							}
							else
							{

								r["Log"] = 0;
							}
							if (AgentlogCheckBox.Checked)
							{
								r["AgentLog"] = 1;
							}
							else
							{

								r["AgentLog"] = 0;
							}


							DELSDataTable.AcceptChanges();
							Session["DELStable"] = DELSDataTable;
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

		protected void ValidationUpdatedButton_Click(object sender, EventArgs e)
		{
			try
			{
				Response.Redirect("DominoELSDefinitions_Grid.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
				Context.ApplicationInstance.CompleteRequest();
			}
			catch (Exception ex)
			{
				
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
			}
		}

		//protected void EventNameTextBox_Validation(object sender, ValidationEventArgs e)
		//{
		//    //DataTable returntable = VSWebBL.SecurityBL.webspehereImportBL.Ins.GetNmae(EventNameTextBox.Text);
		//    DataTable returntable = VSWebBL.ConfiguratorBL.DELSBL.Ins.GetDataByAlertName(EventNameTextBox.Text);

		//    if (returntable.Rows.Count > 0)
		//    {
		//        if (Request.QueryString["EventKey"] != null)
		//        {
		//            if (Session["Name"].ToString() != EventNameTextBox.Text)
		//            {

		//                errormsg.InnerHtml = "Name already exists.Please enter another Name." +
		//                    //  "<button type=\"button\" onclick=' class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
		//                              "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";

		//                errormsg.Style.Value = "display: block";
		//                Session["Namevalidation"] = "true";

		//            }

		//        }
		//        else
		//        {
		//            errorDiv.InnerHtml = "Please create at least one DominoEventLog entry and select at least one  Server." +
		//                                    "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
		//            errorDiv.Style.Value = "display: block";
		//            Session["Namevalidation"] = "true";

		//        }
		//    }
		//    else
		//    {
		//        errormsg.Style.Value = "display: none";


		//    }

		//}

	}
}