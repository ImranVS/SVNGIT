using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using VSWebBL;
using DevExpress.Web.ASPxTreeList;
using System.Collections;
using VSWebDO;

namespace VSWebUI.Configurator
{
	public partial class WebsphereNodesandServers : System.Web.UI.Page
	{
		string CellName;
		int CellID;
		DataTable bycelldata ;
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				Fillcombobox();
				if (Session["enabledservers"] != null)
				{
					successDiv.InnerHtml = "The servers  <b>" + Session["enabledservers"] +
							"</b> are enabled successfully." +
							"<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
					successDiv.Style.Value = "display: block";
					Session["enabledservers"] = null;
				}
			}
			else
			{
				fillNodesTreefromSession();
			}

		}
		public void fillNodesTreefromSession()
		{

			DataTable DataEvents = new DataTable();
			try
			{
				if (Session["NodesServers"] != "" && Session["NodesServers"] != null)
					DataEvents = Session["NodesServers"] as DataTable;
				NodesTreeList.DataSource = DataEvents;
				NodesTreeList.DataBind();

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
		public void Fillcombobox()
		{
			DataTable cellnamesdt = VSWebBL.ConfiguratorBL.WebsphereCellBL.Ins.Fillcombobox();
			ASPxComboBox2.DataSource = cellnamesdt;
			//ASPxComboBox2.ValueField = "CellName";
			//ASPxComboBox2.ValueType = "CompanyName";
			ASPxComboBox2.DataBindItems();

		}
		protected void CollapseAllButton_Click(object sender, EventArgs e)
		{
			try
			{
				if (CollapseAllButton.Text == "Collapse All")
				{
					NodesTreeList.CollapseAll();
					CollapseAllButton.Image.Url = "~/images/icons/add.png";
					CollapseAllButton.Text = "Expand All";
				}
				else
				{
					NodesTreeList.ExpandAll();
					CollapseAllButton.Image.Url = "~/images/icons/forbidden.png";
					CollapseAllButton.Text = "Collapse All";
				}
			}
			catch (Exception ex)
			{
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
			}
		}
		public void fillNodesTreeListbycellID()
		{
			try
			{
				int cellid = Convert.ToInt32(Request.QueryString["CellID"]);
				//string id = Convert.ToString(cellid);
				NodesTreeList.CollapseAll();
				CollapseAllButton.Image.Url = "~/images/icons/add.png";
				CollapseAllButton.Text = "Expand All";
				if (Session["DataEvents"] == null)
				{
					//DataTable DataEventsTree = VSWebBL.SecurityBL.webspehereImportBL.Ins.GetnodesserversFromProcedure();
					DataTable bycelldata = VSWebBL.SecurityBL.webspehereImportBL.Ins.FetserversbycellID(cellid);
					//DataTable DataServersTree = VSWebBL.ConfiguratorBL.AlertsBL.Ins.GetServersFromProcedure();
					Session["DataEvents"] = bycelldata;
				}
				NodesTreeList.DataSource = (DataTable)Session["DataEvents"];
				NodesTreeList.DataBind();

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

		protected void ASPxComboBox2_SelectedIndexChanged(object sender, EventArgs e)
		{
			CellName = ASPxComboBox2.SelectedItem.Text;
			CellID = Convert.ToInt32(ASPxComboBox2.SelectedItem.Value);
			 bycelldata = VSWebBL.SecurityBL.webspehereImportBL.Ins.FetserversbycellID(CellID);
			Session["NodesServers"] = bycelldata;
			NodesTreeList.DataSource = (DataTable)Session["NodesServers"];
			NodesTreeList.DataBind();

		}
		private DataTable GetSelectedEvents(int CellID)
		{
			DataTable dtSel = new DataTable();
			try
			{
				dtSel.Columns.Add("CellID");
				dtSel.Columns.Add("ServerID");
				dtSel.Columns.Add("NodeID");
				dtSel.Columns.Add("Enabled");
				dtSel.Columns.Add("NodeName");
				//10/16/2014 NS added
				//dtSel.Columns.Add("ConsecutiveFailures");
				//string selValues = "";
				TreeListNodeIterator iterator = NodesTreeList.CreateNodeIterator();
				TreeListNode node;
				//TreeListColumn columnActid = EventsTreeList.Columns["actid"];
				TreeListColumn columnActid = NodesTreeList.Columns["actid"];
				TreeListColumn columnSrvId = NodesTreeList.Columns["SrvId"];
				TreeListColumn columnTbl = NodesTreeList.Columns["tbl"];
				TreeListColumn columnNodeName = NodesTreeList.Columns["NodeName"];
				while (true)
				{
					node = iterator.GetNext();

					if (node == null) break;


					//if (node.Level == 1 && node.ParentNode.Selected)
					//{
					//    // root node selected ie All Events selected
					//    DataRow dr = dtSel.NewRow();
					//    dr["CellID"] = CellID;
					//    dr["ServerID"] = 0;// node.GetValue("actid");
					//    dr["NodeID"] = 0;//node.GetValue("SrvId");
					//    dr["Enabled"] = true;
					//    dtSel.Rows.Add(dr);
					//    break;
					//}
					//else if (node.Level == 1 && node.ParentNode.Selected == false && node.Selected)
					//{
					//    // level 1 node selected ie One Servertype selected and all events under it

					//    DataRow dr = dtSel.NewRow();
					//    dr["CellID"] = CellID;
					//    dr["ServerID"] = 0; //node.GetValue("actid");
					//    dr["NodeID"] = ((System.Data.DataRowView)(node.DataItem)).Row.ItemArray[3];//node.GetValue("SrvId");
					//    dr["Enabled"] = true;
					//    dtSel.Rows.Add(dr);
					//}
					////else if (node.Level == 1 && node.ParentNode.Selected == false && node.HasChildren)
					////{
					////    // level 1 node selected ie One Servertype selected and all events under it

					////    DataRow dr = dtSel.NewRow();
					////    dr["CellID"] = CellID;
					////    dr["ServerID"] = node.GetValue("actid");
					////    dr["NodeID"] = ((System.Data.DataRowView)(node.DataItem)).Row.ItemArray[3];//node.GetValue("SrvId");
					////    dr["Enabled"] = true;
					////    dtSel.Rows.Add(dr);
					////}
					//else 
					if (node.Level == 2)
					{
						if (node.Selected)
						{
							DataRow dr = dtSel.NewRow();
							dr["CellID"] = CellID;
							dr["ServerID"] = node.GetValue("actid");
							dr["NodeID"] = node.GetValue("SrvId");
							dr["NodeName"] = node.GetValue("Name");
							dr["Enabled"] = true;
							
							//10/16/2014 NS added
							//dr["ConsecutiveFailures"] = node.GetValue("ConsecutiveFailures");
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
		public ServerAttributes CollectDataforServerAttributes(string servername)
		{
			ServerAttributes Mobj = new ServerAttributes();
			try
			{

				int ReturnValue = VSWebBL.SecurityBL.ServersBL.Ins.GetServerIDbyServerName(servername);

				Mobj.ServerId = ReturnValue;

			}
			catch (Exception ex)
			{

				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}

			return Mobj;

		}
		protected void OKButton_Click(object sender, EventArgs e)
		{
			object ReturnValue2;
			string enabledservers="";
			DataTable dt = GetSelectedEvents(1);
			if (dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					Servers ServersObject = new Servers();
					int cellid = Convert.ToInt32(dt.Rows[i]["ServerID"]);
					string enable = dt.Rows[i]["Enabled"].ToString();
					ServersObject. ServerName = dt.Rows[i]["NodeName"].ToString();
					ServersObject.LocationID = 1;//Location information required here
				string ServerName = dt.Rows[i]["NodeName"].ToString();
					ServerTypes STypeobject = new ServerTypes();
					STypeobject.ServerType = "WebSphere";
					ServersObject.Description = "WebSphere Product";
					ServersObject.IPAddress = "111.898.2435";
					
						ServerTypes ReturnValue = VSWebBL.SecurityBL.ServerTypesBL.Ins.GetDataForServerType(STypeobject);
				ServersObject.ServerTypeID = ReturnValue.ID;
					
					bool update = VSWebBL.SecurityBL.webspehereImportBL.Ins.updatedata(cellid,enable);
					ReturnValue2 = VSWebBL.SecurityBL.webspehereImportBL.Ins.InsertwebsphereData(ServersObject);
					Object ReturnValue3 = VSWebBL.SecurityBL.ServersBL.Ins.UpdateAttributesData(CollectDataforServerAttributes(ServerName));
					if (dt.Rows[i]["NodeName"].ToString() != "")
					{
						Session ["enabledservers"] += dt.Rows[i]["NodeName"].ToString()+", ";
					}
				}


				Session["NodesServers"] = "";
				Response.Redirect("~/Configurator/WebsphereNodesandServers.aspx");
				//fillNodesTreefromSession();
			}
		}

		protected void CancelButton_Click(object sender, EventArgs e)
		{
			Response.Redirect("WebsphereNodesandServers.aspx");
			//Session["NodesServers"] = "";
			//fillNodesTreefromSession();
		}

		protected void NodesTreeList_FocusedNodeChanged(object sender, EventArgs e)
		{
			
		}
	}
}