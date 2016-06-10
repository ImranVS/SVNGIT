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
	public partial class ELS_Edit : System.Web.UI.Page
	{
		DataTable ELSDataTable = null;
		object msgloc = "";
		string flag, day;
		int EventKey;
		string EventName;
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
				EventKey = Convert.ToInt16( Request.QueryString["EventKey"].ToString());
				EventName = Request.QueryString["Name"].ToString();
			}
			else
				flag = "insert";

			if (!IsPostBack)
			{
				if (flag == "update")
					fillData(EventKey);
				FillELSGrid();
				fillServersTreeList();
				if (Session["UserPreferences"] != null)
				{
					DataTable UserPreferences = (DataTable)Session["UserPreferences"];
					foreach (DataRow dr in UserPreferences.Rows)
					{
						if (dr[1].ToString() == "ELS_Edit|ELSGridView")
						{
							ELSGridView.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
						}
					}
				}
			}
			else
			{
				    FillELSGridfromSession();
					fillServersTreefromSession();


			}

		}

		public void fillData(int Key)
		{
			try
			{
				
				string elsName = VSWebBL.ConfiguratorBL.ELSBL.Ins.GetEventNamebyKey(Key);

				EventNameTextBox.Text = elsName;
			}
			catch (Exception ex)
			{
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
			}
		}

		private void FillELSGrid()
		{
			try
			{

				ELSDataTable = new DataTable();
				DataSet CredentialsDataSet = new DataSet();
				ELSDataTable = VSWebBL.ConfiguratorBL.ELSBL.Ins.GetAllData();
				//if (ELSDataTable.Rows.Count > 0)
				//{
				DataTable dtcopy = ELSDataTable.Copy();
				dtcopy.PrimaryKey = new DataColumn[] { dtcopy.Columns["ID"] };

				Session["ELS"] = dtcopy;
				ELSGridView.Selection.IsRowSelected(1);

				ELSGridView.DataSource = ELSDataTable;
				ELSGridView.DataBind();

				if (EventKey != 0)
				{
					DataTable ELSDataTable1 = new DataTable();
					ELSDataTable1 = VSWebBL.ConfiguratorBL.ELSBL.Ins.GetSelectedEventsForKey(EventKey);
					foreach (DataRow r in ELSDataTable1.Rows)
					{
					int startIndex = ELSGridView.PageIndex * ELSGridView.SettingsPager.PageSize;
					int endIndex = Math.Min(ELSGridView.VisibleRowCount, startIndex + ELSGridView.SettingsPager.PageSize);
					for (int i = startIndex; i < endIndex; i++)
					{
						DataRow row = ELSGridView.GetDataRow(i);
						if (Convert.ToInt32(row[1])==Convert.ToInt32(r[0]))
							ELSGridView.Selection.SelectRow(i);
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
		private void FillELSGridfromSession()
		{
			try
			{

				ELSDataTable = new DataTable();
				if (Session["ELS"] != "" && Session["ELS"] != null)
					ELSDataTable = (DataTable)Session["ELS"];
				if (ELSDataTable.Rows.Count > 0)
				{
					ELSGridView.DataSource = ELSDataTable;
					ELSGridView.DataBind();
				}
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
			if (Mode == "Insert")
			{
				Object ReturnValue = VSWebBL.ConfiguratorBL.ELSBL.Ins.InsertData(CollectDataForELS(Mode, EventMasterRow));
				if (ReturnValue == "")
				{
					msgloc = "The Event you entered already exist.";
				}
			}
			if (Mode == "Update")
			{
				bool ReturnValue = VSWebBL.ConfiguratorBL.ELSBL.Ins.UpdateData(CollectDataForELS(Mode, EventMasterRow));
				if (ReturnValue == true)
				{
					successDiv.InnerHtml = "Event  information for <b>" + Session["ELSUpdateStatus"].ToString() +
							"</b> updated successfully." +
							"<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
					successDiv.Style.Value = "display: block";
				}
			}

		}

		private ELSMaster CollectDataForELS(string Mode, DataRow ELSRow)
		{
			VSFramework.TripleDES tripleDes = new VSFramework.TripleDES();
			try
			{
				ELSMaster EventMasterObject = new ELSMaster();
				EventMasterObject.EventName = ELSRow["EventName"].ToString();
				EventMasterObject.AliasName = ELSRow["AliasName"].ToString();
				Session["ELSUpdateStatus"] = EventMasterObject.AliasName;
				EventMasterObject.EventKey = ELSRow["EventKey"].ToString();

				EventMasterObject.EventId = ELSRow["EventId"].ToString();
				EventMasterObject.EventLevel = ELSRow["EventLevel"].ToString();

				EventMasterObject.Source = ELSRow["Source"].ToString();
				EventMasterObject.TaskCategory = ELSRow["TaskCategory"].ToString();

				if (Mode == "Update")
					EventMasterObject.ID = int.Parse(ELSRow["ID"].ToString());

				return EventMasterObject;
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

		protected void ELSGridView_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
		{

			ELSDataTable = (DataTable)Session["ELS"];

			ASPxGridView gridView = (ASPxGridView)sender;
			UpdateEventMasterData("Insert", GetRow(ELSDataTable, e.NewValues.GetEnumerator(), -1));
			gridView.CancelEdit();
			e.Cancel = true;

			errorDiv.InnerHtml = msgloc.ToString() +
				"<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
			errorDiv.Style.Value = "display: block";
			FillELSGrid();

		}

		private DataTable GetSelectedServers(int EventKey)
		{

			DataTable dtSel = new DataTable();
			try
			{
				dtSel.Columns.Add("EventKey");
				dtSel.Columns.Add("ServerID");
				dtSel.Columns.Add("LocationID");
				dtSel.Columns.Add("ServerTypeID");

				TreeListNodeIterator iterator = ServersTreeList.CreateNodeIterator();
				TreeListNode node;

				TreeListColumn columnActid = ServersTreeList.Columns["actid"];
				TreeListColumn columnSrvId = ServersTreeList.Columns["LocId"];
				TreeListColumn columnTbl = ServersTreeList.Columns["tbl"];
				TreeListColumn columnSrvTypeId = ServersTreeList.Columns["srvtypeid"];
				while (true)
				{
					node = iterator.GetNext();

					if (node == null) break;
					//if (node.Level == 1 && node.ParentNode.Selected)
					//{
					//    // root node selected ie All Servers selected
					//    DataRow dr = dtSel.NewRow();
					//    dr["EventKey"] = EventKey;
					//    dr["ServerID"] =  node.GetValue("actid");
					//    dr["LocationID"] = node.GetValue("LocId");
					//    dr["ServerTypeID"] = node.GetValue("srvtypeid");
					//    dtSel.Rows.Add(dr);
					//    break;
					//}
					//else if (node.Level == 1 && node.ParentNode.Selected == false && node.Selected)
					//{

					//    DataRow dr = dtSel.NewRow();
					//    dr["EventKey"] = EventKey;
					//    dr["ServerID"] = node.GetValue("actid");
					//    dr["LocationID"] = ((System.Data.DataRowView)(node.DataItem)).Row.ItemArray[3];// node.GetValue("LocId");
					//    dr["ServerTypeID"] = node.GetValue("srvtypeid");
					//    dtSel.Rows.Add(dr);
					//}
					//else 
					if (node.Level == 2) //(node.ParentNode.Selected==false)
					{

						if (node.Selected)
						{
							DataRow dr = dtSel.NewRow();
							dr["EventKey"] = EventKey;
							dr["ServerID"] = node.GetValue("actid");
							dr["LocationID"] = node.GetValue("LocId");
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

		protected void ELSGridView_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
		{
			ELSMaster LocObject = new ELSMaster();
			LocObject.ID = Convert.ToInt32(e.Keys[0]);

			Object ReturnValue = VSWebBL.ConfiguratorBL.ELSBL.Ins.DeleteData(LocObject);

			ASPxGridView gridView = (ASPxGridView)sender;
			gridView.CancelEdit();
			e.Cancel = true;
			fillData(EventKey);
			FillELSGrid();
		}

		protected void ELSGridView_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
		{

			ELSDataTable = (DataTable)Session["ELS"];
			ASPxGridView gridView = (ASPxGridView)sender;

			UpdateEventMasterData("Update", GetRow(ELSDataTable, e.NewValues.GetEnumerator(), Convert.ToInt32(e.Keys[0])));
			gridView.CancelEdit();
			e.Cancel = true;
			fillData(EventKey);
			
			successDiv.InnerHtml = msgloc.ToString() +"Success." +
				"<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
			successDiv.Style.Value = "display: block";

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
					DataTable DataServersTree = VSWebBL.ConfiguratorBL.ELSBL.Ins.GetServersFromProcedure();
					Session["DataServers"] = DataServersTree;
				}
				ServersTreeList.DataSource = (DataTable)Session["DataServers"];
				ServersTreeList.DataBind();

				DataTable dtSel = VSWebBL.ConfiguratorBL.ELSBL.Ins.GetSelectedServers(EventKey);
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
			if (flag == "insert")
			{
				bool ReturnValuename = VSWebBL.ConfiguratorBL.ELSBL.Ins.insertEventName(EventNameTextBox.Text);
				if (ReturnValuename)
				{
					
					successDiv.InnerHtml = "This event definition is saved sucessfully." +
						"<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
					successDiv.Style.Value = "display: block";
					
				}
				else
				{
					errorDiv.Style.Value = "display: block";
					errorDiv.InnerHtml = "This event definition is already exists." +
						"<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
				}
				InsertAlertData();
			}
			else
			{
				if (Request.QueryString["Name"] != null)
				{
					string name = Request.QueryString["Name"].ToString();
					Session["Name"] = name;
					bool ReturnValue = VSWebBL.ConfiguratorBL.ELSBL.Ins.updateEventName(EventKey, EventNameTextBox.Text, Session["Name"].ToString());
					if (ReturnValue)
					{

						successDiv.InnerHtml = "This event definition is updated sucessfully." +
							"<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
						successDiv.Style.Value = "display: block";
					}
					else
					{
						errorDiv.Style.Value = "display: block";
						errorDiv.InnerHtml = "This event definition is already exists." +
							"<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
					}
				}
				InsertAlertData();
				
			}
			
		}
		protected void CancelButton_Click(object sender, EventArgs e)
		{
			Response.Redirect("~/Configurator/ELSDefinitions_Grid.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
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
		public void InsertAlertData()
		{
			int startIndex = ELSGridView.PageIndex * ELSGridView.SettingsPager.PageSize;
					int endIndex = Math.Min(ELSGridView.VisibleRowCount, startIndex + ELSGridView.SettingsPager.PageSize);
					DataTable dtELSTask = (DataTable) Session["ELS"];
				try
				{
					for (int i = startIndex; i < endIndex; i++)
					{
						if (ELSGridView.Selection.IsRowSelected(i))
						{
							dtELSTask.Rows[i]["isSelected"] = "true";
						}
					}
					DataTable dtELSTask1  =GetSelectedServers(EventKey);

					bool ReturnValue = VSWebBL.ConfiguratorBL.ELSBL.Ins.insertEventServers(EventNameTextBox.Text, dtELSTask, dtELSTask1);

				}
				catch (Exception ex)
				{
					
					Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				}
		}
		private void EventNameComboBox_OnCallback(object source, CallbackEventArgsBase e)
		{
			FillEventNameComboBox(source as ASPxComboBox);
		}
		private void EventLevelComboBox_OnCallback(object source, CallbackEventArgsBase e)
		{
			FillEventLevelComboBox(source as ASPxComboBox);
		}
		protected void ELSGridView_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
		{
			if (e.Column.FieldName == "EventName")
			{
				ASPxComboBox MyComboBox = e.Editor as ASPxComboBox;
				FillEventNameComboBox(MyComboBox);
				MyComboBox.Callback += new CallbackEventHandlerBase(EventNameComboBox_OnCallback);
			}
			if (e.Column.FieldName == "EventLevel")
			{
				ASPxComboBox MyComboBox = e.Editor as ASPxComboBox;
				FillEventLevelComboBox(MyComboBox);
				MyComboBox.Callback += new CallbackEventHandlerBase(EventLevelComboBox_OnCallback);
			}

		}
		private void FillEventNameComboBox(ASPxComboBox MyComboBox)
		{
			MyComboBox.Items.Add("System", "System");
			MyComboBox.Items.Add("Application", "Application");
			MyComboBox.Items.Add("Security", "Security");
			MyComboBox.Items.Add("Setup", "Setup");
		}
		private void FillEventLevelComboBox(ASPxComboBox MyComboBox)
		{
			MyComboBox.Items.Add("Critical", "Critical");
			MyComboBox.Items.Add("Error", "Error");
			MyComboBox.Items.Add("Information", "Information");
			MyComboBox.Items.Add("Warning", "Warning");
		}

	}
}















