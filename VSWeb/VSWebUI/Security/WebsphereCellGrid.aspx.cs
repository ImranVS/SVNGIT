using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VSFramework;
using VSWebBL;
using VSWebDO;
using DevExpress.Web;
using System.Collections;
using DevExpress.Web.Data;
using System.Runtime.InteropServices;
using System.Data;
using DevExpress.Web.ASPxTreeList;

using System.Xml.Serialization;
using System.Xml;

namespace VSWebUI.Configurator
{
	public partial class WebsphererCellGrid : System.Web.UI.Page
	{
		protected void Page_PreInit(object sender, EventArgs e)
		{

			this.Master.contentCallEvent += new EventHandler(Master_ButtonClick);
		}

		private void Master_ButtonClick(object sender, EventArgs e)
		{

		}
		int cellvalue;
		int nodevalue;
		int servertype, WebsphereCellID;

		protected void Page_Load(object sender, EventArgs e)
		{

			bool popupopen;
			Control ctrl;
			Page.Title = "WebSphere Cell";

			if (!IsPostBack)
			{

				fillGrid();
				Session["DataEvents123"] = null;
				//FillWebsphereNodeStatusGrid();
			}
			else
			{
				fillGridfromSession();
				fillNodesTreeListbycellIDfromsession();
			}


			if (Session["UserPreferences"] != null)
			{
				DataTable UserPreferences = (DataTable)Session["UserPreferences"];
				foreach (DataRow dr in UserPreferences.Rows)
				{
					if (dr[1].ToString() == "WebsphererCellGrid|webspherecellgrid")
					{
						webspherecellgrid.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
					}
				}
			}
			//if (webspherecellgrid.VisibleRowCount > 0)
			//{
			//    NodesTreeList.UnselectAll();
			//    NodesTreeList.DataSource = "";
			//    NodesTreeList.DataBind();
			//    int index = webspherecellgrid.FocusedRowIndex;
			//    if (index > -1)
			//    {
			//        cellvalue = Convert.ToInt32(webspherecellgrid.GetRowValues(index, "CellID").ToString());
			//        //fillNodesTreeListbycellID(cellvalue);
			//        fillNodesTreeListbycellID(cellvalue);



			//    }
			//}
			expandButton();

		}

		public void fillGrid()
		{
			Session["Treeviewvisibility"] = " ";
			DataTable webspheretable = new DataTable();
			webspheretable = VSWebBL.ConfiguratorBL.WebsphereCellBL.Ins.GetWebsphereCellNames();
			if (webspheretable.Rows.Count == 0)
			{


				Session["Treeviewvisibility"] = "true";
			}
			try
			{
				Session["WebsphereCellNameTable"] = webspheretable;
				webspherecellgrid.DataSource = webspheretable;
				webspherecellgrid.DataBind();
			}
			catch (Exception ex)
			{
				//6/27/2014 NS added for VSPLUS-634
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw;
			}
		}
		public void fillGridfromSession()
		{
			DataTable webspheretable = new DataTable();
			if (Session["WebsphereCellNameTable"] != "" && Session["WebsphereCellNameTable"] != null)
				webspheretable = (DataTable)Session["WebsphereCellNameTable"];

			try
			{
				webspherecellgrid.DataSource = webspheretable;
				webspherecellgrid.DataBind();
			}
			catch (Exception ex)
			{
				//6/27/2014 NS added for VSPLUS-634
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw;
			}

		}

		protected void webspherecellgrid_HtmlRowCreated(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
		{


			//webspherecellgrid.EnableCallBacks = true;
		//	e.Row.Attributes.Add("onmouseover", "this.originalcolor=this.style.backgroundColor;" + "this.style.backgroundColor='#C0C0C0';");

			//e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalcolor;");


			if (e.RowType == GridViewRowType.EditForm)
			{


				try
				{
					if (e.GetValue("CellID") != " " && e.GetValue("CellID") != null)
					{
						Response.Redirect("~/Security/ImportWebsphereServers.aspx?CellID=" + e.GetValue("CellID") + "&CellName=" + e.GetValue("CellName") + "&HostName=" + e.GetValue("HostName") + "&ConnectionType=" + e.GetValue("ConnectionType") + "&PortNo=" + e.GetValue("PortNo") + "&GlobalSecurity=" + e.GetValue("GlobalSecurity") + "&Realm=" + e.GetValue("Realm"), false);
						//ASPxWebControl.RedirectOnCallback("ImportWebsphereServers.aspx?CellID=" + e.GetValue("CellID") + "&CellName=" + e.GetValue("CellName") + "&HostName=" + e.GetValue("HostName") + "&ConnectionType=" + e.GetValue("ConnectionType") + "&PortNo=" + e.GetValue("PortNo") + "&GlobalSecurity=" + e.GetValue("GlobalSecurity") + "&Realm=" + e.GetValue("Realm"));
						//Context.ApplicationInstance.CompleteRequest();
						Context.ApplicationInstance.CompleteRequest();
					}
					else
					{
						Response.Redirect("~/Security/ImportWebsphereServers.aspx",false);
						//Context.ApplicationInstance.CompleteRequest();
						Context.ApplicationInstance.CompleteRequest();
					}
				}
				catch (Exception ex)
				{
					Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
					//throw ex;
				}

			}
		}
		protected void btn_Clickeditserver(object sender, EventArgs e)
		{
			int ID;

			ImageButton btn = (ImageButton)sender;
			ID = Convert.ToInt32(btn.CommandArgument);
			int id = ID;
			WebsphereCell ProfileNamesObject = new WebsphereCell();

			if (id != null)
				ProfileNamesObject.CellID = id;

			bool s = true;
			if (s == true)
			{




				DataTable celldt = VSWebBL.SecurityBL.webspehereImportBL.Ins.GetSpecificCellData(id);
				Session["webcellid"] = id;
				VitalSignsWebSphereDLL.VitalSignsWebSphereDLL WSDll = new VitalSignsWebSphereDLL.VitalSignsWebSphereDLL();
				VitalSignsWebSphereDLL.VitalSignsWebSphereDLL.CellProperties cellFromInfo = new VitalSignsWebSphereDLL.VitalSignsWebSphereDLL.CellProperties();

				//Set properties for Cell to pass to DLL
				cellFromInfo.HostName = celldt.Rows[0]["HostName"].ToString();
				cellFromInfo.Port = Convert.ToInt32(celldt.Rows[0]["PortNo"].ToString());
				cellFromInfo.ConnectionType = celldt.Rows[0]["ConnectionType"].ToString();
				cellFromInfo.Realm = celldt.Rows[0]["Realm"].ToString();
				//int credid = Convert.ToInt32(celldt.Rows[0][""].ToString());
				Credentials creds = new Credentials();
				//creds.AliasName = CredentialsComboBox.Text.ToString();
				creds.ID = Convert.ToInt32(celldt.Rows[0]["CredentialsID"].ToString());

				DataTable dt = VSWebBL.ConfiguratorBL.CredentialsBL.Ins.getCredentialsById(creds);
				if (dt.Rows.Count > 0)
				{
					string password;
					string MyObjPwd;
					string[] MyObjPwdArr;
					byte[] MyPass;
					MyObjPwd = dt.Rows[0]["Password"].ToString();

					MyObjPwdArr = MyObjPwd.Split(',');
					MyPass = new byte[MyObjPwdArr.Length];
					for (int i = 0; i < MyObjPwdArr.Length; i++)
					{
						MyPass[i] = Byte.Parse(MyObjPwdArr[i]);
					}

					VSFramework.TripleDES tripleDes = new VSFramework.TripleDES();
					password = tripleDes.Decrypt(MyPass);

					cellFromInfo.UserName = dt.Rows[0]["UserID"].ToString();
					cellFromInfo.Password = password;


				}
				else
				{
					throw new Exception("Username and Password could not be retreived");
				}

				cellFromInfo.ID = id;

				//Call
				VitalSignsWebSphereDLL.VitalSignsWebSphereDLL.Cells cells = null;
				try
				{
					cells = WSDll.getServerList(cellFromInfo);
					foreach (VitalSignsWebSphereDLL.VitalSignsWebSphereDLL.Cell cell in cells.Cell)
					{

						bool ReturnValue;
						ReturnValue = VSWebBL.SecurityBL.webspehereImportBL.Ins.Insertwebspherenodesandservers(cell, id);
						if (ReturnValue == true)
						{
							NodesTreeList.Visible = true;
							CollapseAllButton.Visible = true;
							ImportButton.Visible = true;
							//expandButton();
						}
						else
						{
							NodesTreeList.Visible = false;
							CollapseAllButton.Visible = false;
							ImportButton.Visible = false;
						}
						
					}
				}
				catch (Exception ex)
				{
					errorDivForImportingWS.Style.Value = "display: block";
					errorDivForImportingWS.InnerHtml = "An error occurred. " + ex.Message +
						"<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
					return;
				}


				//checks to see if a connection was successfully made
				//cells should never be null, it should hti the return before it is null and hits this spot
				if (cells != null && cells.Connection_Status != "CONNECTED")
				{

					errorDivForImportingWS.Style.Value = "display: block";
					errorDivForImportingWS.InnerHtml = "A connection was not able to be made."+
						  "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
					return;
				}
				else
				{
					errorDivForImportingWS.Style.Value = "display: none";

					///////////////////////////////////////////////////////////////////////
					//This is where the population of the tree graph should be
					///////////////////////////////////////////////////////////////////////




					// 6/22/15 WS commented out for it not being needed anymore.  All insertion of data should be done on the OK press now (Which will be taken care of by Mukund and his team)
					//Insertdata();
				}
				fillNodesTreeListbycellID(id);
				//FillWebsphereNodeStatusGrid(id);
				//    {
				//        int index = NodesTreeList.FocusedRowIndex;
				//        if (index > -1)
				//        {
				//            nodevalue = Convert.ToInt32(NodesTreeList.GetRowValues(index, "NodeID").ToString());
				//        }
				//    }
				//    //Response.Redirect("~/Configurator/EditProfiles.aspx?id=" + id.ToString(), false);
				//    //Context.ApplicationInstance.CompleteRequest();

			}
			//DecodeCellsXML("C:\\Program Files (x86)\\IBM\\WebSphere\\AppClient\\VitalSigns\\xml\\AppServerList.xml");
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
		public void fillNodesTreeListbycellID(int id)
		{
			try
			{
				int cellid = Convert.ToInt32(id);
				
				//DataTable DataEventsTree = VSWebBL.SecurityBL.webspehereImportBL.Ins.GetnodesserversFromProcedure();
				DataTable bycelldata = VSWebBL.SecurityBL.webspehereImportBL.Ins.FetserversbycellID(cellid);
				//DataTable DataServersTree = VSWebBL.ConfiguratorBL.AlertsBL.Ins.GetServersFromProcedure();
				Session["DataEvents"] = bycelldata;
				if (bycelldata.Rows.Count > 0)
				{
					NodesTreeList.Visible = true;
					CollapseAllButton.Visible = true;
					ImportButton.Visible = true;
					
				}
				else
				{
					NodesTreeList.Visible = false;
					CollapseAllButton.Visible = false;
					ImportButton.Visible = false;
				}
				NodesTreeList.DataSource = (DataTable)Session["DataEvents"];
				NodesTreeList.DataBind();
				expandButton();
			}
			catch (Exception ex)
			{

				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
			}
		}

		public void fillNodesTreeListbycellIDfromsession()
		{
			try
			{
				if (Session["DataEvents"] != null)
				{
					NodesTreeList.DataSource = (DataTable)Session["DataEvents"];
					NodesTreeList.DataBind();
					expandButton();
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
		//public void fillNodesTreeListbycellID(int id)
		//{
		//    //Session["DataEvents"] = null;
		//    try
		//    {
		//        int cellid = Convert.ToInt32(id);
		//        //string id = Convert.ToString(cellid);
		//        NodesTreeList.CollapseAll();
		//        CollapseAllButton.Image.Url = "~/images/icons/add.png";
		//        CollapseAllButton.Text = "Expand All";
		//        if (Session["DataEvents123"] == null)
		//        {
		//            DataTable bycelldata = VSWebBL.SecurityBL.webspehereImportBL.Ins.FetserversbycellID(cellid);
		//            //DataTable bycelldata = VSWebBL.SecurityBL.webspehereImportBL.Ins.FetserversbycellID(cellid);
		//            //DataTable DataServersTree = VSWebBL.ConfiguratorBL.AlertsBL.Ins.GetServersFromProcedure();
		//            Session["DataEvents123"] = bycelldata;
		//            NodesTreeList.DataSource = (DataTable)Session["DataEvents123"];
		//            NodesTreeList.DataBind();
		//        }
		//        else
		//        {
		//            if(Page.IsPostBack)
		//            {
		//                Session["DataEvents123"] = null;
		//                NodesTreeList.DataSource = "";
		//                NodesTreeList.DataBind();
		//                NodesTreeList.ClearNodes();
		//            }
		//        }
		//        NodesTreeList.DataSource = (DataTable)Session["DataEvents123"];
		//        NodesTreeList.DataBind();

		//            //DataTable DataEventsTree = VSWebBL.SecurityBL.webspehereImportBL.Ins.GetnodesserversFromProcedure();

		//            //DataTable DataServersTree = VSWebBL.ConfiguratorBL.AlertsBL.Ins.GetServersFromProcedure();
		//            //if (bycelldata.Rows.Count > 0)
		//            //{
		//            //bycelldata = (DataTable)Session["DataEvents123"];
		//            //    NodesTreeList.DataSource = bycelldata;

		//            //    NodesTreeList.DataBind();

		//            //}
		//            //else
		//            //{


		//            //Session["DataEvents1"] = bycelldata;

		//            //NodesTreeList.DataSource = bycelldata;
		//            //NodesTreeList.RefreshVirtualTree();
		//            //NodesTreeList.DataBind();
		//            //NodesTreeList.LayoutChanged();
		//            //NodesTreeList.Nodes.Clear();
		//            //NodesTreeList.ClearNodes();
		//            }




		//    catch (Exception ex)
		//    {
		//        //Log.Entry.Ins.Write(Server.MapPath("~/LogFiles/"), "VSPlusLog.txt", DateTime.Now.ToString() + " Error in Page: " +
		//        //    Request.Url.AbsolutePath + ", Method: " + System.Reflection.MethodBase.GetCurrentMethod().Name +
		//        //    ", Error: " + ex.ToString());

		//        //6/27/2014 NS added for VSPLUS-634
		//        Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
		//    }
		//}

		//public void fillNodesTreeListbycellIDfromsession()
		//{
		//    try
		//    {
		//        //int cellid = Convert.ToInt32(id);
		//        ////string id = Convert.ToString(cellid);
		//        //NodesTreeList.CollapseAll();
		//        //CollapseAllButton.Image.Url = "~/images/icons/add.png";
		//        //CollapseAllButton.Text = "Expand All";
		//        //if (Session["DataEvents"] == null)
		//        //{
		//        //    //DataTable DataEventsTree = VSWebBL.SecurityBL.webspehereImportBL.Ins.GetnodesserversFromProcedure();
		//        //    DataTable bycelldata = VSWebBL.SecurityBL.webspehereImportBL.Ins.FetserversbycellID(cellid);
		//        //    //DataTable DataServersTree = VSWebBL.ConfiguratorBL.AlertsBL.Ins.GetServersFromProcedure();
		//        //    Session["DataEvents"] = bycelldata;
		//        //}
		//        //DataTable bycelldata = VSWebBL.SecurityBL.webspehereImportBL.Ins.FetserversbycellID(cellid);
		//        DataTable DataEvents = new DataTable();
		//        if (Session["DataEvents123"]== null)
		//        {
		//            if (Session["DataEvents123"] != "" && Session["DataEvents123"] != null)
		//                DataEvents = Session["DataEvents123"] as DataTable;
		//            NodesTreeList.DataSource = DataEvents;
		//            NodesTreeList.DataBind();
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
		//public void FillWebsphereNodeStatusGrid(int id)
		//{

		//    DataTable dt = new DataTable();

		//    dt = VSWebBL.DashboardBL.WebsphereBL.Ins.GetWebsphereNodeStatus(id);
		//    WebsphereNodesgridview.DataSource = dt;
		//    WebsphereNodesgridview.DataBind();



		//}
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
				dtSel.Columns.Add("Status");
				dtSel.Columns.Add("HostName");




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
				TreeListColumn columnStatus = NodesTreeList.Columns["Status"];
				TreeListColumn columnHostName = NodesTreeList.Columns["HostName"];
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
							dr["Status"] = node.GetValue("Status");
							dr["HostName"] = node.GetValue("HostName");
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
		protected void ImportButton_Click(object sender, EventArgs e)
		{
			object ReturnValue2;
			int sid;
			string sname;
			string enabledservers = "";
			DataTable dtsrv = new DataTable();
			DataTable dt = GetSelectedEvents(1);
			DataTable dt1 = new DataTable();

			dt1.Columns.Add("ID");
			dt1.Columns.Add("ServerName");
			dt1.Columns.Add("IPAddress");
			dt1.Columns.Add("Description");
			dt1.Columns.Add("ServerType");

			dt1.Columns.Add("ProfileName");
			if (dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					VSWebDO.Servers ServersObject = new VSWebDO.Servers();
					int cellid = Convert.ToInt32(dt.Rows[i]["ServerID"]);
					string enable = dt.Rows[i]["Enabled"].ToString();
					ServersObject.ServerName = dt.Rows[i]["NodeName"].ToString();
					ServersObject.LocationID = 1;//Location information required here
					string ServerName = dt.Rows[i]["NodeName"].ToString();
					ServerTypes STypeobject = new ServerTypes();
					STypeobject.ServerType = "WebSphere";
					ServersObject.Description = "WebSphere Product";
					ServersObject.IPAddress = "111.898.2435";
					ServersObject.ProfileName = "Default";




					DataRow dr = dt1.NewRow();
					dr["ID"] = "";
					dr["ServerName"] = dt.Rows[i]["NodeName"].ToString();
					dr["IPAddress"] = "111.898.2435";

					dr["Description"] = "WebSphere Product";
					dr["ServerType"] = "WebSphere";

					dr["ProfileName"] = "Default";
					dt1.Rows.Add(dr);


					ServerTypes ReturnValue = VSWebBL.SecurityBL.ServerTypesBL.Ins.GetDataForServerType(STypeobject);

					ServersObject.ServerTypeID = ReturnValue.ID;


					DataTable returntable = VSWebBL.SecurityBL.webspehereImportBL.Ins.GetServerName(ServersObject);
					if (returntable.Rows.Count == 0)
					{
						ReturnValue2 = VSWebBL.SecurityBL.webspehereImportBL.Ins.InsertwebsphereData1(ServersObject);
						DataTable returntable1 = VSWebBL.SecurityBL.webspehereImportBL.Ins.GetServerName(ServersObject);
						sid = Convert.ToInt32(returntable1.Rows[0]["ID"].ToString());
						sname = returntable1.Rows[0]["ServerName"].ToString();
					}
					else
					{
						sid = Convert.ToInt32(returntable.Rows[0]["ID"].ToString());
						sname = returntable.Rows[0]["ServerName"].ToString();
					}

					// Object result1 = VSWebBL.ConfiguratorBL.WebSpherepropertiesBL.Ins.();
					// ReturnValue = VSWebBL.SecurityBL.webspehereImportBL.Ins.InsertData(webspehereObject);


					bool update = VSWebBL.SecurityBL.webspehereImportBL.Ins.updatedataweb(sid, sname, enable);

					Object ReturnValue3 = VSWebBL.SecurityBL.ServersBL.Ins.UpdateAttributesData(CollectDataforServerAttributes(ServerName));

					if (dt.Rows[i]["NodeName"].ToString() != "")
					{
						Session["enabledservers"] += dt.Rows[i]["NodeName"].ToString() + ", ";
					}
				}


				Session["NodesServers"] = dt1;
				Response.Redirect("~/Security/WebSphereImportServers2.aspx");
				//fillNodesTreefromSession();
			}
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
		protected void webspherecellgrid_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
		{
			e.Cell.ToolTip = string.Format("{0}", e.CellValue);
		}
		protected void webspherecellgrid_PageSizeChanged(object sender, EventArgs e)
		{
			//ProfilesGridView.PageIndex;
			VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("WebsphererCellGrid|webspherecellgrid", webspherecellgrid.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
			Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
		}

		protected void webspherecellgrid_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
		{
			WebsphereCell webcellobj = new WebsphereCell();
			webcellobj.CellID = Convert.ToInt32(e.Keys[0]);
			try
			{
				VSWebBL.ConfiguratorBL.WebsphereCellBL.Ins.Deletecell(webcellobj);
			}
			catch (Exception ex)
			{
				//6/27/2014 NS added for VSPLUS-634
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
			}
			ASPxGridView gridview = (ASPxGridView)sender;
			gridview.CancelEdit();
			e.Cancel = true;
			fillGrid();
			if (Session["Treeviewvisibility"].ToString() == "true")
			{

				CollapseAllButton.Visible = false;
				ImportButton.Visible = false;
				NodesTreeList.Visible = false;

			}
			else
			{
				CollapseAllButton.Visible = true;
				ImportButton.Visible = true;
				NodesTreeList.Visible = true;

			}
		}

		protected void NewButton_Click(object sender, EventArgs e)
		{
			//3/2/2015 NS added for VSPLUS-1432
			Response.Redirect("~/Security/ImportWebsphereServers.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
			Context.ApplicationInstance.CompleteRequest();
		}
		protected void webspherecellgrid_FocusedRowChanged(object sender, EventArgs e)
		{
			webspherecellgrid.EnableCallBacks = false;
			if (webspherecellgrid.VisibleRowCount > 0)
			{
				NodesTreeList.UnselectAll();
				NodesTreeList.DataSource = "";
				NodesTreeList.DataBind();
				int index = webspherecellgrid.FocusedRowIndex;
				if (index > -1)
				{
					cellvalue = Convert.ToInt32(webspherecellgrid.GetRowValues(index, "CellID").ToString());
					//fillNodesTreeListbycellID(cellvalue);
					fillNodesTreeListbycellID(cellvalue);
					


				}
			}
			expandButton();
		}

		protected void DecodeCellsXML(string pathToAppServerListXML)
		{
			//NOTES:
			//Currently, this file Loads the XML file and puts it in classes and stores it in the "cells" variable.
			//It then iterates the object, finding each cell, each node in the cell, each server in that node and creates sql inserts
			//It currently does not execute the sql statements, as i am unsure what the real idea is right now
			//Nothing is currently returned, all it does is decode th XML, stores it in memory, constructs sql statements, and ends
			//Please modify to your needs as i am unsure the idea of the import wizard


			//string path = "AppServerList.xml";
			String sql = "";
			XmlSerializer serializer = new XmlSerializer(typeof(Cells));

			XmlDocument doc = new XmlDocument();
			doc.Load(pathToAppServerListXML);

			XmlNodeReader reader = new XmlNodeReader(doc);

			Cells cells = (Cells)serializer.Deserialize(reader);

			//inserts for cell table

			foreach (Cell cell in cells.Cell)
			{
				sql += "INSERT INTO WebsphereCell (CellName) VALUES ('" + cell.Name.ToString() + "');\n";

				foreach (Node node in cell.Nodes.Node)
				{
					sql += "INSERT INTO WebsphereNode (NodeName, CellId, HostName) VALUES ('" + node.Name.ToString() + "',(SELECT MAX(CellID) FROM WebsphereCell where CellName='" + cell.Name.ToString() + "'),'" + node.HostName.ToString() + "');\n";
					//INSERT

					foreach (string serverName in node.Servers.Server)
					{
						sql += "INSERT INTO Servers (ServerName, ServerTypeId, Description, LocationId, IPAddress) VALUES ('" + serverName + "', '22', 'WebSphere', (SELECT MIN(ID) From Locations),'');\n";
						sql += "INSERT INTO WebsphereServer (ServerName, CellId, NodeId, ServerId) VALUES ('" + serverName + "',(SELECT MAX(CellID) FROM WebsphereCell where CellName='" + cell.Name.ToString() + "')," +
							"(SELECT MAX(NodeId) FROM WebsphereNode where NodeName='" + node.Name.ToString() + "'), (SELECT MAX(ID) FROM Servers WHERE ServerName='" + serverName + "'));\n";
					}

				}
			}

			//sql = sql;
			//////////////////////////////////////////////////////////////////////////
			//INSERT THE SQL STATEMNT HERE
			/////////////////////////////////////////////////////////////////////////
		}


		[XmlRoot(ElementName = "servers")]
		public class Servers
		{
			[XmlElement(ElementName = "server")]
			public List<string> Server { get; set; }
		}

		[XmlRoot(ElementName = "node")]
		public class Node
		{
			[XmlElement(ElementName = "servers")]
			public Servers Servers { get; set; }
			[XmlAttribute(AttributeName = "hostName")]
			public string HostName { get; set; }
			[XmlAttribute(AttributeName = "name")]
			public string Name { get; set; }
		}

		[XmlRoot(ElementName = "nodes")]
		public class Nodes
		{
			[XmlElement(ElementName = "node")]
			public List<Node> Node { get; set; }
		}

		[XmlRoot(ElementName = "cell")]
		public class Cell
		{
			[XmlElement(ElementName = "nodes")]
			public Nodes Nodes { get; set; }
			[XmlAttribute(AttributeName = "name")]
			public string Name { get; set; }
		}

		[XmlRoot(ElementName = "Cells")]
		public class Cells
		{
			[XmlElement(ElementName = "cell")]
			public List<Cell> Cell { get; set; }
		}
		protected void btn_Click(object sender, EventArgs e)
		{
			ImageButton btn = (ImageButton)sender;
			
			WebsphereCell webcellobj = new WebsphereCell();
		
			WebsphereCellID = Convert.ToInt32(btn.CommandArgument);
			string name = btn.AlternateText;
			pnlAreaDtls.Style.Add("visibility", "visible");
			divmsg.InnerHtml = "Are you sure you want to delete the WebSphere Cell  " + name + "?";

		}
		protected void btn_CancelClick(object sender, EventArgs e)
		{

			fillGrid();

		}
		protected void OKButton_Click(object sender, EventArgs e)
		{
			NavigatorPopupControl.ShowOnPageLoad = false;
		}
		protected void btn_OkClick(object sender, EventArgs e)
		{
			WebsphereCell webcellobj = new WebsphereCell();
			webcellobj.CellID = WebsphereCellID;
			VSWebBL.ConfiguratorBL.WebsphereCellBL.Ins.Deletecell(webcellobj);

			fillGrid();


			Site1 currMaster = (Site1)this.Master;
			currMaster.refreshStatusBoxes();

		}
		public void expandButton()
		{

			try
			{

				if (CollapseAllButton.Text == "Collapse All")
				{
					NodesTreeList.ExpandAll();
					//CollapseButton.Image.Url = "~/images/icons/add.png";
					//CollapseButton.Text = "Expand All Rows";
				}
				else
				{
					NodesTreeList.CollapseAll();
					//CollapseButton.Image.Url = "~/images/icons/forbidden.png";
					//CollapseButton.Text = "Collapse All Rows";

				}
			}
			catch (Exception ex)
			{

				throw ex;
			}
		}
	}

}