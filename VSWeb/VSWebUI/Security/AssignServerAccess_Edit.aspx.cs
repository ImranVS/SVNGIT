using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DevExpress.Web.ASPxTreeList;

namespace VSWebUI.Security
{
    public partial class AssignServerAccess_Edit : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string uname = "";
            string uid = "";
            if (!IsPostBack)
            {
                bool isadmin = false;
                if (Request.QueryString["FullName"] != null && Request.QueryString["FullName"] != "")
                {
                    uname = Request.QueryString["FullName"].ToString();
                }
                if (Request.QueryString["UserID"] != null && Request.QueryString["UserID"] != "")
                {
                    uid = Request.QueryString["UserID"].ToString();
                    DataTable sa = VSWebBL.SecurityBL.UsersBL.Ins.GetIsAdmin1(uid);
                    if (sa.Rows.Count > 0)
                    {
                        if (sa.Rows[0]["SuperAdmin"].ToString() == "Y")
                        {
                            isadmin = true;
                        }
                    }
                    if (isadmin)
                    {
                        ServerAccessTreeList.SettingsSelection.Enabled = false;
                        AssignServerAccessButton.Visible = false;
                        ResetServerAccessButton.Visible = false;
                        CancelButton.Text = "Back";
                    }
                }
                //10/30/2015 NS added for VSPLUS-2170
                if (Session["UserPreferences"] != null)
                {
                    DataTable UserPreferences = (DataTable)Session["UserPreferences"];
                    foreach (DataRow dr in UserPreferences.Rows)
                    {
                        if (dr[1].ToString() == "AssignServerAccess_Edit|ServerAccessTreelist")
                        {
                            ServerAccessTreeList.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                        }
                    }
                }
              
              
                servernamelbldisp.InnerText += uname;
                FillTreeList();
                RemoveRestricted();
            }
            else
            {
                FillTreeList();
                //FillTreeListFromSession();
            }
        }

        private void FillTreeList()
        {
            if (Request.QueryString["FullName"] != null && Request.QueryString["FullName"] != "")
            {
                DataTable DataServersTree = new DataTable();
                //DataServersTree = VSWebBL.SecurityBL.AdminTabBL.Ins.ServersVisibleUpdateGrid(Request.QueryString["FullName"].ToString());
                DataServersTree = VSWebBL.ConfiguratorBL.AlertsBL.Ins.GetServersFromProcedure();
                if (DataServersTree.Rows.Count > 0)
                {
                    foreach( DataRow row in DataServersTree.Rows )
                    {
                        if( row.ItemArray[5].ToString() == "7" )
                            row.Delete();
                    }
                    DataServersTree.AcceptChanges();
                }
                ServerAccessTreeList.DataSource = DataServersTree;
                ServerAccessTreeList.DataBind();
                Session["AllServerAccessList"] = DataServersTree;
            }
        }

        private void FillTreeListFromSession()
        {
            if (Session["AllServerAccessList"] != null && Session["AllServerAccessList"] != "")
            {
                ServerAccessTreeList.DataSource = (DataTable)Session["AllServerAccessList"];
                ServerAccessTreeList.DataBind();
            }
        }

        protected void CollapseAllButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (CollapseAllButton.Text == "Collapse All")
                {
                    ServerAccessTreeList.CollapseAll();
                    CollapseAllButton.Image.Url = "~/images/icons/add.png";
                    CollapseAllButton.Text = "Expand All";
                }
                else
                {
                    ServerAccessTreeList.ExpandAll();
                    CollapseAllButton.Image.Url = "~/images/icons/forbidden.png";
                    CollapseAllButton.Text = "Collapse All";
                }
            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
            }
        }

        private void RemoveRestricted()
        {
            string uname = "";
            DataTable dt = new DataTable();
            DataTable AllServersDT = new DataTable();
            DataTable ServerRestrictedDataTable = new DataTable();
            if (Request.QueryString["FullName"] != null && Request.QueryString["FullName"] != "")
            {
                uname = Request.QueryString["FullName"].ToString();
            }
            if (Session["AllServerAccessList"] != null && Session["AllServerAccessList"] != "")
            {
                AllServersDT = (DataTable)Session["AllServerAccessList"];
            }
            ServerRestrictedDataTable = VSWebBL.SecurityBL.AdminTabBL.Ins.GetAllServersNotVisible(uname);
            if (AllServersDT.Rows.Count > 0)
            {
                dt = VSWebBL.ConfiguratorBL.AlertsBL.Ins.GetServersFromProcedure();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        if (row.ItemArray[5].ToString() == "7")
                            row.Delete();
                    }
                    dt.AcceptChanges();
                }
                ServerAccessTreeList.DataSource = dt;
                ServerAccessTreeList.DataBind();
                if (ServerRestrictedDataTable.Rows.Count > 0)
                {
                    List<int> ServerID = new List<int>();
                    List<int> LocationID = new List<int>();
                    foreach (DataRow resser in ServerRestrictedDataTable.Rows)
                    {
                        foreach (DataRow allrow in AllServersDT.Rows)
                        {
                            if (resser["ID"].ToString() == allrow["actid"].ToString())
                            {
                                if (allrow["locid"] != null && allrow["locid"].ToString() != "" && resser["LocId"].ToString() == allrow["locid"].ToString())
                                {
                                    ServerID.Add(AllServersDT.Rows.IndexOf(allrow));
                                }
                            }
                        }
                    }
                    foreach (int Id in ServerID)
                    {
                        AllServersDT.Rows[Id].Delete();
                    }
                    AllServersDT.AcceptChanges();
                    try
                    {
                        TreeListNodeIterator iterator = ServerAccessTreeList.CreateNodeIterator();
                        TreeListNode node;
                        string locid = "";
                        string actid = "";
                        string srvtypeid = "";
                        while (true)
                        {
                            node = iterator.GetNext();
                            if (node == null) break;
                            if (node.Level == 2)
                            {
                                if (node.GetValue("LocId") != null)
                                {
                                    locid = node.GetValue("LocId").ToString();
                                    actid = node.GetValue("actid").ToString();
                                    srvtypeid = node.GetValue("srvtypeid").ToString();
                                    DataRow[] foundrows = AllServersDT.Select("LocId=" + locid + " AND actid=" + actid + " AND srvtypeid=" + srvtypeid);
                                    if (foundrows.Length > 0)
                                    {
                                        node.Selected = true;
                                    }
                                }
                            }
                        }
                        ServerAccessTreeList.ExpandAll();
                        Session["ServerAccessList"] = AllServersDT;
                    }
                    catch (Exception ex)
                    {
                        Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                        throw ex;
                    }
                }
                else
                {
                    ServerAccessTreeList.SelectAll();
                }
            }
        }

        protected void ResetServerAccessButton_Click(object sender, EventArgs e)
        {
            ServerAccessTreeList.DataSource = VSWebBL.ConfiguratorBL.AlertsBL.Ins.GetServersFromProcedure();
            ServerAccessTreeList.DataBind();
            ServerAccessTreeList.SelectAll();
            ServerAccessTreeList.ExpandAll();
        }

        protected void ServerAccessTreeList_PageSizeChanged(object sender, EventArgs e)
        {
            VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("AssignServerAccess_Edit|ServerAccessTreelist", ServerAccessTreeList.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        }

        protected void AssignServerAccessButton_Click(object sender, EventArgs e)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            DataTable locSel = new DataTable();
            string lid = "";
            string uname = "";
            bool insertedSrv = false;
            bool insertedLoc = false;
            if (Request.QueryString["FullName"] != null && Request.QueryString["FullName"] != "")
            {
                uname = Request.QueryString["FullName"].ToString();
            }
            GetDeSelectedServers(dictionary,locSel);
            try
            {
                insertedSrv = VSWebBL.SecurityBL.AdminTabBL.Ins.InsertRestricted_Servers(uname, dictionary);
                if (locSel.Rows.Count > 0)
                {
                    for (int i = 0; i < locSel.Rows.Count; i++)
                    {
                        lid += locSel.Rows[i]["LocationID"].ToString() + ",";
                    }
                }
                if (lid.Length > 0)
                {
                    lid = lid.Substring(0, lid.Length - 1);
                }
                insertedLoc = VSWebBL.SecurityBL.AdminTabBL.Ins.InsertRestricted_Locations(uname, lid);
                if (insertedSrv && insertedLoc)
                {
					//1/22/2016 Durga modified for VSPLUS--2516
					Session["AssignSeverAcessUpdateStatus"] = uname;
                    Response.Redirect("AssignServerAccess.aspx", false);
                    Context.ApplicationInstance.CompleteRequest();
                }
                else
                {
                    errorDiv.Style.Value = "display: block";
                    errorDiv.InnerHtml = "There was an issue writing Server or Location restrictions into the SQL table." +
                        "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                }
            }
            catch (Exception ex)
            {
                errorDiv.Style.Value = "display: block";
                errorDiv.InnerHtml = "There was an issue writing Server or Location restrictions into the SQL table: " + ex.Message +
                    "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
            }
        }

        private void GetDeSelectedServers(Dictionary<string, string> dictionary, DataTable locSel)
        {
            string uid = "";
            string separator = "###$$$###";
            if (Request.QueryString["UserID"] != null && Request.QueryString["UserID"] != "")
            {
                uid = Request.QueryString["UserID"].ToString();
            }
            try
            {
                locSel.Columns.Add("UserID");
                locSel.Columns.Add("LocationID");
                TreeListNodeIterator iterator = ServerAccessTreeList.CreateNodeIterator();
                TreeListNode node;
                int counter = 0;
                string locid = "";

                while (true)
                {
                    node = iterator.GetNext();
                    if (node == null) break;
                    counter = 0;
                    //Loop through child nodes. If all of the child nodes are de-selected, add a location to the list
                    //of restrictions
                    if (node.HasChildren)
                    {
                        foreach (TreeListNode childNode in node.ChildNodes)
                        {
                            if (!childNode.Selected)
                            {
                                counter += 1;
                                locid = childNode.GetValue("locid").ToString();
                            }
                        }
                        //If the de-selected number of nodes is the same as the total number of child nodes, add a location to the
                        //list
                        if (counter == node.ChildNodes.Count)
                        {
                            if (locid != "")
                            {
                                DataRow locdr = locSel.NewRow();
                                locdr["UserID"] = uid;
                                locdr["LocationID"] = locid;
                                locSel.Rows.Add(locdr);
                                DataColumn[] keyColumns = new DataColumn[1];
                                keyColumns[0] = locSel.Columns["LocationID"];
                                locSel.PrimaryKey = keyColumns;
                            }
                        }
                    }
                    if (node.Level == 2) //(node.ParentNode.Selected==false)
                    {
                        if (!node.Selected)
                        {
                            locid = node.GetValue("locid").ToString();
                            DataRow[] foundrows = locSel.Select("LocationID=" + locid + "");
                            if (foundrows.Length == 0)
                            {
                                dictionary.Add("'" + node.GetValue("ServerType") + "'" + separator + "'" + node.GetValue("Name") + "'", "'" + node.GetValue("Name") + "'");
                            }   
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
        }

        protected void CancelButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("AssignServerAccess.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }
        protected void DataBound(object sender, EventArgs e)
        {
            SetItemCount();

        }
        void SetItemCount()
        {


            int itemCount = ServerAccessTreeList.Nodes.OfType<TreeListNode>().Select(x => x.ChildNodes.Count).Sum();      // int itemCount = (int)ServersTreeList.GetSummaryValue()

            ServerAccessTreeList.SettingsPager.Summary.Text = "Page {0} of {1} (" + itemCount + " items)";
        }
    }
}