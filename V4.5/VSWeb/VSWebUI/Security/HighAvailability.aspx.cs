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
    public partial class HighAvailability : System.Web.UI.Page
    {
		protected void Page_PreInit(object sender, EventArgs e)
		{

			this.Master.contentCallEvent += new EventHandler(Master_ButtonClick);
		}

		private void Master_ButtonClick(object sender, EventArgs e)
		{

		}

        DataTable ServerNodes;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack && !IsCallback)
            {
                Session["DataServers"] = null;
                Session["ServerNodes"] = null;
                Session["SelectedServers"] = null;
                Session["UnSelectedServers"] = null;

                GetServerNodes();
                PopulateServerNodes();
                fillServersTreeList();

                //SuccessServerNode.Visible = false;
                //ErrorServerNode.Visible = false;
            }
            else
            {
                //SuccessServerNode.Visible = false;
                //ErrorServerNode.Visible = false;
                fillServersTreefromSession();                
            }            
        }

        public void PopulateServerNodes()
        {
            ServerNodeComboBox.Items.Clear();
            if (Session["ServerNodes"] != null)
            {
                ServerNodeComboBox.DataSource = Session["ServerNodes"];
                ServerNodeComboBox.ValueField = "NodeID";
                ServerNodeComboBox.ValueType = typeof(Int32);
                ServerNodeComboBox.TextField = "NodeHostName";
                ServerNodeComboBox.DataBind();
            }
        }

        public void GetServerNodes()
        {
            ServerNodes = VSWebBL.SecurityBL.ProfilesMasterBL.Ins.GetServerNodes();
            if (ServerNodes != null && ServerNodes.Rows.Count > 0)
            {
                PrimaryNodeHostName.Text = ServerNodes.Rows[0][1].ToString();
                PrimaryNodeIPAddress.Text = ServerNodes.Rows[0][2].ToString();
                PrimaryNodeDescription.Text = ServerNodes.Rows[0][3].ToString();
                if (ServerNodes.Rows.Count > 1)
                {
                    SecondaryNodeHostName.Text = ServerNodes.Rows[1][1].ToString();
                    SecondaryNodeIPAddress.Text = ServerNodes.Rows[1][2].ToString();
                    SecondaryNodeDescription.Text = ServerNodes.Rows[1][3].ToString();
                }
                Session["ServerNodes"] = ServerNodes;
            }
            

            //Session["ServerNodes"] = ServerNodes;
        }

        public void fillServersTreeList()
        {
            try
            {
                
                    DataTable DataServersTree = VSWebBL.ConfiguratorBL.AlertsBL.Ins.GetServersFromProcedure();
                    string PrimaryNode = "";
                    DataTable ServerNodes = Session["ServerNodes"] as DataTable;
                    if (ServerNodes != null && ServerNodes.Rows.Count > 0)
                    {
                        PrimaryNode = ServerNodes.Rows[0][1].ToString();
                    }
                    foreach(DataRow dr in DataServersTree.Rows)
                    {
                        if (dr["LocId"].ToString() != "")
                        {
                            if (dr["MonitoredBy"].ToString() == "")
                            {
                                if (PrimaryNode != "")
                                {
                                    dr["MonitoredBy"] = "Unassigned(Monitored by " + PrimaryNode + ")";
                                }
                                else
                                {
                                    dr["MonitoredBy"] = "Unassigned";
                                }
                            }
                        }
                    }
                    Session["DataServers"] = DataServersTree;
               

                ServersTreeList.DataSource = (DataTable)Session["DataServers"];
                ServersTreeList.DataBind();
                ServersTreeList.ExpandAll();
                ServersTreeList.UnselectAll();


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
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
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
                //Log.Entry.Ins.Write(Server.MapPath("~/LogFiles/"), "VSPlusLog.txt", DateTime.Now.ToString() + " Error in Page: " +
                //    Request.Url.AbsolutePath + ", Method: " + System.Reflection.MethodBase.GetCurrentMethod().Name +
                //    ", Error: " + ex.ToString());
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
        }

        private void GetSelectedServers()
        {
            DataTable dtSel = new DataTable();
            DataTable dtUnSel = new DataTable();
            try
            {
                dtSel.Columns.Add("ServerID");
                dtSel.Columns.Add("Name");
                dtSel.Columns.Add("ServerType");

                dtUnSel.Columns.Add("ServerID");
                dtUnSel.Columns.Add("Name");
                dtUnSel.Columns.Add("ServerType");

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
                            dr["ServerType"] = node.GetValue("ServerType");
                            dtSel.Rows.Add(dr);
                        }
                        else
                        {
                            DataRow dr = dtUnSel.NewRow();
                            dr["ServerID"] = node.GetValue("actid");
                            dr["Name"] = node.GetValue("Name");
                            dr["ServerType"] = node.GetValue("ServerType");
                            dtUnSel.Rows.Add(dr);
                        }
                    }
                }
                Session["SelectedServers"] = dtSel;
                Session["UnSelectedServers"] = dtUnSel;
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

        //private DataTable GetUnSelectedServers()
        //{

        //    DataTable dtSel = new DataTable();
        //    DataTable dtUnSel = new DataTable();
        //    try
        //    {
        //        dtSel.Columns.Add("ServerID");
        //        dtSel.Columns.Add("Name");
        //        dtSel.Columns.Add("ServerType");

        //        dtUnSel.Columns.Add("ServerID");
        //        dtUnSel.Columns.Add("Name");
        //        dtUnSel.Columns.Add("ServerType");

        //        //string selValues = "";
        //        TreeListNodeIterator iterator = ServersTreeList.CreateNodeIterator();
        //        TreeListNode node;

        //        while (true)
        //        {
        //            node = iterator.GetNext();

        //            if (node == null) break;
        //            if (node.Level == 2) //(node.ParentNode.Selected==false)
        //            {

        //                if (node.Selected)
        //                {
        //                    DataRow dr = dtSel.NewRow();
        //                    dr["ServerID"] = node.GetValue("actid");
        //                    dr["Name"] = node.GetValue("Name");
        //                    dr["ServerType"] = node.GetValue("ServerType");
        //                    dtSel.Rows.Add(dr);
        //                }
        //                else
        //                {
        //                    DataRow dr = dtUnSel.NewRow();
        //                    dr["ServerID"] = node.GetValue("actid");
        //                    dr["Name"] = node.GetValue("Name");
        //                    dr["ServerType"] = node.GetValue("ServerType");
        //                    dtUnSel.Rows.Add(dr);
        //                }
        //            }


        //        }
        //        Session["SelectedServers"] = dtSel;
        //        Session["UnSelectedServers"] = dtUnSel;

        //    }
        //    catch (Exception ex)
        //    {
        //        //Log.Entry.Ins.Write(Server.MapPath("~/LogFiles/"), "VSPlusLog.txt", DateTime.Now.ToString() + " Error in Page: " +
        //        //    Request.Url.AbsolutePath + ", Method: " + System.Reflection.MethodBase.GetCurrentMethod().Name +
        //        //    ", Error: " + ex.ToString());
        //    }

        //    return dtSel;

        //}

        protected void SelCriteriaRadioButtonList_SelectedIndexChanged(object sender, EventArgs e)
        {
            errorDiv.Style.Value = "display: none;";
            successDiv.Style.Value = "display: none";
            if (SelCriteriaRadioButtonList.SelectedIndex == 0)
            {
                SecondaryNodePanel.Visible = false;
                ServerRoundPanel.Visible = false;

            }
            else if (SelCriteriaRadioButtonList.SelectedIndex == 1)
            {
                SecondaryNodePanel.Visible = true;
                ServerRoundPanel.Visible = false;
            }
            else
            {
                SecondaryNodePanel.Visible = true;
                ServerRoundPanel.Visible = true;
                PopulateServerNodes();
            }
        }

        protected void SaveSettings_Click(object sender, EventArgs e)
        {
            errorDiv.Style.Value = "display: none;";
            successDiv.Style.Value = "display: none;";
            ErrorUpdateServers.Style.Value = "display: none;";
            SuccessUpdateServers.Style.Value = "display: none;";
            int runModeError = 0;
            int serverNodeError1 = 0;
            int serverNodeError2 = 0;
            int PrimaryNodeID = 1;
            Int16 SecondaryNodeID = 2;
            DataTable serverNodes = Session["ServerNodes"] as DataTable;
            if (SelCriteriaRadioButtonList.SelectedIndex == 0)
            {

                if (serverNodes == null)
                {
                    if (PrimaryNodeHostName.Text != "" & PrimaryNodeIPAddress.Text != "" && PrimaryNodeDescription.Text != "")
                    {
                        serverNodeError1 = VSWebBL.SecurityBL.ProfilesMasterBL.Ins.UpdateServerNodes(PrimaryNodeID, PrimaryNodeHostName.Text, PrimaryNodeIPAddress.Text, PrimaryNodeDescription.Text);

                        if (serverNodeError1 != 0)
                        {
                            runModeError = VSWebBL.SecurityBL.ProfilesMasterBL.Ins.UpdateRunMode("StandAlone");
                        }
                        if (serverNodeError1 == 0 || runModeError == 0)
                        {
                            //10/6/2014 NS modified for VSPLUS-990
                            errorDiv.InnerHtml = "Specified settings were NOT updated."+
                                "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                            errorDiv.Style.Value = "display: block";
                        }
                        else
                        {
                            //10/6/2014 NS modified for VSPLUS-990
                            successDiv.InnerHtml = "Specified settings were updated."+
                                "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                            successDiv.Style.Value = "display: block";
                        }
                    }
                    else
                    {
                        //10/6/2014 NS modified for VSPLUS-990
                        errorDiv.InnerHtml = "Please provide the Primary Node details."+
                            "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                        errorDiv.Style.Value = "display: block";
                    }
                }
                else
                {
                    if (PrimaryNodeHostName.Text != "" & PrimaryNodeIPAddress.Text != "" && PrimaryNodeDescription.Text != "")
                    {
                        serverNodeError1 = VSWebBL.SecurityBL.ProfilesMasterBL.Ins.UpdateServerNodes(PrimaryNodeID, PrimaryNodeHostName.Text, PrimaryNodeIPAddress.Text, PrimaryNodeDescription.Text);

                        if (serverNodeError1 != 0)
                        {
                            runModeError = VSWebBL.SecurityBL.ProfilesMasterBL.Ins.UpdateRunMode("StandAlone");
                        }
                        if (serverNodeError1 == 0 || runModeError == 0)
                        {
                            //10/6/2014 NS modified for VSPLUS-990
                            errorDiv.InnerHtml = "Specified settings were NOT updated."+
                                "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                            errorDiv.Style.Value = "display: block";
                        }
                        else
                        {
                            //10/6/2014 NS modified for VSPLUS-990
                            successDiv.InnerHtml = "Specified settings were updated."+
                                "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                            successDiv.Style.Value = "display: block";
                        }
                    }
                    else
                    {
                        runModeError = VSWebBL.SecurityBL.ProfilesMasterBL.Ins.UpdateRunMode("StandAlone");

                        if (runModeError == 0)
                        {
                            //10/6/2014 NS modified for VSPLUS-990
                            errorDiv.InnerHtml = "Specified settings were NOT updated."+
                                "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                            errorDiv.Style.Value = "display: block";
                        }
                        else
                        {
                            //10/6/2014 NS modified for VSPLUS-990
                            successDiv.InnerHtml = "Specified settings were updated."+
                                "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                            successDiv.Style.Value = "display: block";
                        }
                    }
                }

            }
            else if (SelCriteriaRadioButtonList.SelectedIndex == 1)
            {
                if (serverNodes == null)
                {
                    if (PrimaryNodeHostName.Text != "" & PrimaryNodeIPAddress.Text != "" && PrimaryNodeDescription.Text != "" && SecondaryNodeHostName.Text != "" && SecondaryNodeIPAddress.Text != "" && SecondaryNodeDescription.Text != "")
                    {
                        serverNodeError1 = VSWebBL.SecurityBL.ProfilesMasterBL.Ins.UpdateServerNodes(PrimaryNodeID, PrimaryNodeHostName.Text, PrimaryNodeIPAddress.Text, PrimaryNodeDescription.Text);

                        if (serverNodeError1 != 0)
                        {
                            serverNodeError2 = VSWebBL.SecurityBL.ProfilesMasterBL.Ins.UpdateServerNodes(SecondaryNodeID, SecondaryNodeHostName.Text, SecondaryNodeIPAddress.Text, SecondaryNodeDescription.Text);

                        }
                        if (serverNodeError1 != 0 && serverNodeError2 != 0)
                        {
                            runModeError = VSWebBL.SecurityBL.ProfilesMasterBL.Ins.UpdateRunMode("FailOver");
                        }
                        if (serverNodeError1 == 0 || serverNodeError2 == 0 || runModeError == 0)
                        {
                            //10/6/2014 NS modified for VSPLUS-990
                            errorDiv.InnerHtml = "Specified settings were NOT updated."+
                                "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                            errorDiv.Style.Value = "display: block";
                        }
                        else
                        {
                            //10/6/2014 NS modified for VSPLUS-990
                            successDiv.InnerHtml = "Specified settings were updated."+
                                "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                            successDiv.Style.Value = "display: block";
                        }
                    }
                    else
                    {
                        //10/6/2014 NS modified for VSPLUS-990
                        errorDiv.InnerHtml = "Please provide Primary & Secondary Node details."+
                            "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                        errorDiv.Style.Value = "display: block";
                    }
                }
                else if (serverNodes.Rows.Count < 2)
                {
                    serverNodeError1 = 1;
                    serverNodeError2 = 1;
                    if (SecondaryNodeHostName.Text != "" && SecondaryNodeIPAddress.Text != "" && SecondaryNodeDescription.Text != "")
                    {
                        if (PrimaryNodeHostName.Text != "" & PrimaryNodeIPAddress.Text != "" && PrimaryNodeDescription.Text != "")
                        {
                            serverNodeError1 = VSWebBL.SecurityBL.ProfilesMasterBL.Ins.UpdateServerNodes(PrimaryNodeID, PrimaryNodeHostName.Text, PrimaryNodeIPAddress.Text, PrimaryNodeDescription.Text);
                        }
                        if (SecondaryNodeHostName.Text != "" && SecondaryNodeIPAddress.Text != "" && SecondaryNodeDescription.Text != "")
                        {
                            //serverNodeError1 = 1;
                            serverNodeError2 = VSWebBL.SecurityBL.ProfilesMasterBL.Ins.UpdateServerNodes(SecondaryNodeID, SecondaryNodeHostName.Text, SecondaryNodeIPAddress.Text, SecondaryNodeDescription.Text);
                        }
                        if (serverNodeError1 != 0 && serverNodeError2 != 0)
                        {
                            runModeError = VSWebBL.SecurityBL.ProfilesMasterBL.Ins.UpdateRunMode("FailOver");
                        }
                        if (serverNodeError1 == 0 || serverNodeError2 == 0 || runModeError == 0)
                        {
                            //10/6/2014 NS modified for VSPLUS-990
                            errorDiv.InnerHtml = "Specified settings were NOT updated."+
                                "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                            errorDiv.Style.Value = "display: block";
                        }
                        else
                        {
                            //10/6/2014 NS modified for VSPLUS-990
                            successDiv.InnerHtml = "Specified settings were updated."+
                                "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                            successDiv.Style.Value = "display: block";
                        }
                    }
                    else
                    {
                        //10/6/2014 NS modified for VSPLUS-990
                        errorDiv.InnerHtml = "Please provide Secondary Node details."+
                            "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                        errorDiv.Style.Value = "display: block";
                    }
                }
                else
                {
                    serverNodeError1 = 1;
                    serverNodeError2 = 1;
                    if (PrimaryNodeHostName.Text != "" & PrimaryNodeIPAddress.Text != "" && PrimaryNodeDescription.Text != "")
                    {
                        serverNodeError1 = VSWebBL.SecurityBL.ProfilesMasterBL.Ins.UpdateServerNodes(PrimaryNodeID, PrimaryNodeHostName.Text, PrimaryNodeIPAddress.Text, PrimaryNodeDescription.Text);
                    }
                    if (SecondaryNodeHostName.Text != "" && SecondaryNodeIPAddress.Text != "" && SecondaryNodeDescription.Text != "")
                    {
                        serverNodeError2 = VSWebBL.SecurityBL.ProfilesMasterBL.Ins.UpdateServerNodes(SecondaryNodeID, SecondaryNodeHostName.Text, SecondaryNodeIPAddress.Text, SecondaryNodeDescription.Text);
                    }
                    if (serverNodeError1 != 0 && serverNodeError2 != 0)
                    {
                        runModeError = VSWebBL.SecurityBL.ProfilesMasterBL.Ins.UpdateRunMode("FailOver");
                    }
                    if (serverNodeError1 == 0 || serverNodeError2 == 0 || runModeError == 0)
                    {
                        //10/6/2014 NS modified for VSPLUS-990
                        errorDiv.InnerHtml = "Specified settings were NOT updated."+
                            "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                        errorDiv.Style.Value = "display: block";
                    }
                    else
                    {
                        //10/6/2014 NS modified for VSPLUS-990
                        successDiv.InnerHtml = "Specified settings were updated."+
                            "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                        successDiv.Style.Value = "display: block";
                    }
                }
            }
            else if (SelCriteriaRadioButtonList.SelectedIndex == 2)
            {
                if (serverNodes == null)
                {
                    if (PrimaryNodeHostName.Text != "" & PrimaryNodeIPAddress.Text != "" && PrimaryNodeDescription.Text != "" && SecondaryNodeHostName.Text != "" && SecondaryNodeIPAddress.Text != "" && SecondaryNodeDescription.Text != "")
                    {
                        serverNodeError1 = VSWebBL.SecurityBL.ProfilesMasterBL.Ins.UpdateServerNodes(PrimaryNodeID, PrimaryNodeHostName.Text, PrimaryNodeIPAddress.Text, PrimaryNodeDescription.Text);

                        if (serverNodeError1 != 0)
                        {
                            serverNodeError2 = VSWebBL.SecurityBL.ProfilesMasterBL.Ins.UpdateServerNodes(SecondaryNodeID, SecondaryNodeHostName.Text, SecondaryNodeIPAddress.Text, SecondaryNodeDescription.Text);

                        }
                        if (serverNodeError1 != 0 && serverNodeError2 != 0)
                        {
                            runModeError = VSWebBL.SecurityBL.ProfilesMasterBL.Ins.UpdateRunMode("LoadBalancing");
                        }
                        if (serverNodeError1 == 0 || serverNodeError2 == 0 || runModeError == 0)
                        {
                            //10/6/2014 NS modified for VSPLUS-990
                            errorDiv.InnerHtml = "Specified settings were NOT updated."+
                                "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                            errorDiv.Style.Value = "display: block";
                        }
                        else
                        {
                            //10/6/2014 NS modified for VSPLUS-990
                            successDiv.InnerHtml = "Specified settings were updated."+
                                "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                            successDiv.Style.Value = "display: block";
                        }
                    }
                    else
                    {
                        //10/6/2014 NS modified for VSPLUS-990
                        errorDiv.InnerHtml = "Please provide Primary & Secondary Node details."+
                            "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                        errorDiv.Style.Value = "display: block";
                    }
                }

                else if (serverNodes.Rows.Count < 2)
                {
                    serverNodeError1 = 1;
                    serverNodeError2 = 1;
                    if (SecondaryNodeHostName.Text != "" && SecondaryNodeIPAddress.Text != "" && SecondaryNodeDescription.Text != "")
                    {
                        if (PrimaryNodeHostName.Text != "" & PrimaryNodeIPAddress.Text != "" && PrimaryNodeDescription.Text != "")
                        {
                            serverNodeError1 = VSWebBL.SecurityBL.ProfilesMasterBL.Ins.UpdateServerNodes(PrimaryNodeID, PrimaryNodeHostName.Text, PrimaryNodeIPAddress.Text, PrimaryNodeDescription.Text);
                        }
                        if (SecondaryNodeHostName.Text != "" && SecondaryNodeIPAddress.Text != "" && SecondaryNodeDescription.Text != "")
                        {
                            //serverNodeError1 = 1;
                            serverNodeError2 = VSWebBL.SecurityBL.ProfilesMasterBL.Ins.UpdateServerNodes(SecondaryNodeID, SecondaryNodeHostName.Text, SecondaryNodeIPAddress.Text, SecondaryNodeDescription.Text);
                        }
                        if (serverNodeError1 != 0 && serverNodeError2 != 0)
                        {
                            runModeError = VSWebBL.SecurityBL.ProfilesMasterBL.Ins.UpdateRunMode("LoadBalancing");
                        }
                        if (serverNodeError1 == 0 || serverNodeError2 == 0 || runModeError == 0)
                        {
                            //10/6/2014 NS modified for VSPLUS-990
                            errorDiv.InnerHtml = "Specified settings were NOT updated."+
                                "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                            errorDiv.Style.Value = "display: block";
                        }
                        else
                        {
                            //10/6/2014 NS modified for VSPLUS-990
                            successDiv.InnerHtml = "Specified settings were updated."+
                                "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                            successDiv.Style.Value = "display: block";
                        }
                    }
                    else
                    {
                        //10/6/2014 NS modified for VSPLUS-990
                        errorDiv.InnerHtml = "Please provide Secondary Node details."+
                            "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                        errorDiv.Style.Value = "display: block";
                    }
                }

                else
                {
                    serverNodeError1 = 1;
                    serverNodeError2 = 1;
                    if (PrimaryNodeHostName.Text != "" & PrimaryNodeIPAddress.Text != "" && PrimaryNodeDescription.Text != "")
                    {
                        serverNodeError1 = VSWebBL.SecurityBL.ProfilesMasterBL.Ins.UpdateServerNodes(PrimaryNodeID, PrimaryNodeHostName.Text, PrimaryNodeIPAddress.Text, PrimaryNodeDescription.Text);
                    }
                    if (SecondaryNodeHostName.Text != "" && SecondaryNodeIPAddress.Text != "" && SecondaryNodeDescription.Text != "")
                    {
                        serverNodeError2 = VSWebBL.SecurityBL.ProfilesMasterBL.Ins.UpdateServerNodes(SecondaryNodeID, SecondaryNodeHostName.Text, SecondaryNodeIPAddress.Text, SecondaryNodeDescription.Text);
                    }
                    if (serverNodeError1 != 0 && serverNodeError2 != 0)
                    {
                        runModeError = VSWebBL.SecurityBL.ProfilesMasterBL.Ins.UpdateRunMode("LoadBalancing");
                    }
                    if (serverNodeError1 == 0 || serverNodeError2 == 0 || runModeError == 0)
                    {
                        //10/6/2014 NS modified for VSPLUS-990
                        errorDiv.InnerHtml = "Specified settings were NOT updated."+
                            "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                        errorDiv.Style.Value = "display: block";
                    }
                    else
                    {
                        //10/6/2014 NS modified for VSPLUS-990
                        successDiv.InnerHtml = "Specified settings were updated."+
                            "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                        successDiv.Style.Value = "display: block";
                    }
                }
                GetServerNodes();
                PopulateServerNodes();
            }
        }

        public void ApplyToServers_Click(object sender, EventArgs e)
        {
            errorDiv.Style.Value = "display: none;";
            successDiv.Style.Value = "display: none;";
            
            DataTable serverNodes = Session["ServerNodes"] as DataTable;
            int nodeNotSelected = 2;
            if (serverNodes != null && serverNodes.Rows.Count >= 2)
            {
                if ((ServerNodeComboBox.SelectedItem != null) && Convert.ToInt32(ServerNodeComboBox.SelectedItem.Value) > 0)
                {
                    GetSelectedServers();
                    DataTable dtSelectedServers = Session["SelectedServers"] as DataTable;
                    DataTable dtUnSelectedServers = Session["UnSelectedServers"] as DataTable;
                    List<DataRow> serversSelected = dtSelectedServers.AsEnumerable().ToList();
                    List<DataRow> serversUnSelected = dtUnSelectedServers.AsEnumerable().ToList();
                    if (Convert.ToInt32(ServerNodeComboBox.SelectedItem.Value) != 1)
                    {
                        nodeNotSelected = 1;
                    }
                    UpdateServerMonitoringNodes(serversSelected, Convert.ToInt32(ServerNodeComboBox.SelectedItem.Value));
                    //UpdateServerMonitoringNodes(serversUnSelected, nodeNotSelected);
                }
                else
                {                    
                    //10/6/2014 NS modified for VSPLUS-990
                    ErrorUpdateServers.InnerHtml = "Please select Server Node."+
                        "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                    ErrorUpdateServers.Style.Value = "display: block";
                     
                }
            }
            else
            {                
                //10/6/2014 NS modified for VSPLUS-990
                ErrorUpdateServers.InnerHtml = "Please select Server Node."+
                    "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                ErrorUpdateServers.Style.Value= "display: block";
            }
            GetServerNodes();
            PopulateServerNodes();
            fillServersTreeList();
        }


        public void UpdateServerMonitoringNodes(List<DataRow> serversSelected, int nodeSelected)
        {
            int Update = 0;
            string ServerErrors = "";
            if (serversSelected.Count > 0)
            {
                string AppliedServers = "";
                foreach (DataRow server in serversSelected)
                {
                    Update = VSWebBL.SecurityBL.ProfilesMasterBL.Ins.UpdateServerMonitoringNodes(Convert.ToInt32(server[0]), server[2].ToString(), nodeSelected);
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
                    //10/6/2014 NS modified for VSPLUS-990
                    ErrorUpdateServers.InnerHtml = "Settings for the following server: " + ServerErrors + " were NOT updated."+
                        "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                    ErrorUpdateServers.Style.Value = "display: block";
                }
                if (AppliedServers != "")
                {
                    //10/6/2014 NS modified for VSPLUS-990
                    SuccessUpdateServers.InnerHtml = "Settings for the following server: " + AppliedServers + " were updated."+
                        "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                    SuccessUpdateServers.Style.Value = "display: block";
                }
            }
            else
            {
                //10/6/2014 NS modified for VSPLUS-990
                ErrorUpdateServers.InnerHtml = "Please Select the Server(s)."+
                    "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                ErrorUpdateServers.Style.Value = "display: block";
            }

        }
                
    }
}
