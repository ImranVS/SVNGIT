using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DevExpress.Web;
using VSWebBL;
using VSWebDO;
using System.Collections;
using DevExpress.Web.ASPxTreeList;

namespace VSWebUI.Security
{
    public partial class ApplyServerSettings : System.Web.UI.Page
    {
        object msgloc = "";
        protected DataTable ProfilesDataTable = null;
        protected DataTable DSTasksDataTable = null;
        //protected DataSet DSTaskSettingsDataSet = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            //12/12/2013 NS modified
            //lblError.Text = "";
            lblMessage.Text = "";
            if (!IsPostBack && !IsCallback)
            {
                Session["DataServers"] = null;
                Session["Profiles"] = null;
                Session["DominoServers"] = null;
                Session["DominoTasks"] = null;

                tblServer.Style.Add("visibility", "hidden");
                ApplyServersButton.Style.Add("visibility", "hidden");
                Session["Mode"] = "Insert";
                Session["ProfileId"] = "0";
                //ProfileTextBox.Enabled = true;
                FillServerTypeComboBox();
                fillDominoServersTreeList();
                FillDominoServerTasksGrid();
                //if (Request.QueryString["ProfileId"] != null && Request.QueryString["ProfileId"] != "")
                //{
                //    Session["ProfileId"] = Request.QueryString["ProfileId"];
                //    Session["Mode"] = "Update";
                //    //ProfileTextBox.Enabled = false;
                //    FillData(int.Parse(Session["ProfileId"].ToString()));
                //}
            }
            else
            {
                fillDominoServersTreeListfromSession();
                FillDominoServerTasksGridfromSession();
                FillProfilesGridfromSession();
                fillServersTreefromSession();
                //ServersGridView.DataBind();
            }
            
        }
        private void FillData(int ProfileId)
        {
            UserProfileMaster LocObject =new UserProfileMaster();
            LocObject.ID = ProfileId;
            //UserProfileMaster ProfileMasterDataTable = VSWebBL.SecurityBL.UserProfileMasterBL.Ins.GetData(LocObject);
            //ProfileTextBox.Text = ProfileMasterDataTable.Name;

            DataTable ProfileDtlDataTable = VSWebBL.SecurityBL.UserProfileMasterBL.Ins.GetUserProfileDetailedData(LocObject);
            if (ProfileDtlDataTable.Rows.Count > 0)
            {
                tblServer.Style.Add("visibility", "visible");
                ApplyServersButton.Style.Add("visibility", "visible");

                ServerTypeComboBox.Text = ProfileDtlDataTable.Rows[0]["ServerType"].ToString();


                FillProfilesGrid(ServerTypeComboBox.SelectedItem.Text);
                fillServersTreeList();
                GridViewDataColumn column2 = ProfilesGridView.Columns["DefaultValue"] as GridViewDataColumn;
                for (int i = 0; i < ProfileDtlDataTable.Rows.Count; i++)
                {
                    for (int j = 0; j < ProfilesGridView.VisibleRowCount; j++)
                    {
                        if (ProfilesGridView.GetRowValues(j, ProfilesGridView.KeyFieldName).ToString() == ProfileDtlDataTable.Rows[i]["ProfilesMasterId"].ToString())
                        {
                            ProfilesGridView.Selection.SelectRow(j);
                            ASPxTextBox txtValue = (ASPxTextBox)ProfilesGridView.FindRowCellTemplateControl(j, column2, "txtDefaultValue");
                            txtValue.Text = ProfileDtlDataTable.Rows[i]["DefaultValue"].ToString();
                        }
                    }
                }
            }

        }
        private void FillServerTypeComboBox()
        {
            DataTable ServerDataTable = VSWebBL.SecurityBL.ServerTypesBL.Ins.GetAllData();
            ServerTypeComboBox.DataSource = ServerDataTable;
            ServerTypeComboBox.TextField = "ServerType";
            ServerTypeComboBox.ValueField = "ServerType";
            ServerTypeComboBox.DataBind();
        }

        protected void ServerTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ServerTypeComboBox.SelectedIndex != -1)
            {
                Session["DataServers"] = null;
                Session["Profiles"] = null;

                //12/12/2013 NS added
                errorDiv.Style.Value = "display: none;";
                errorDiv2.Style.Value = "display: none";
                
                successDiv.Style.Value = "display: none";

                ApplyServersButton.Style.Add("visibility", "visible");
                tblServer.Style.Add("visibility", "visible");
                FillProfilesGrid(ServerTypeComboBox.SelectedItem.Text);
                fillServersTreeList();
            }
            //12/12/2013 NS modified
            //lblError.Text = "";
            lblMessage.Text = "";
        }
        private void FillProfilesGrid(string ServerType)
        {
            try
            {

                ProfilesDataTable = new DataTable();
                DataSet ProfilesDataSet = new DataSet();
                ProfilesDataTable = VSWebBL.SecurityBL.ProfilesMasterBL.Ins.GetAllDataByServerType(ServerType);
                if (ProfilesDataTable.Rows.Count > 0)
                {
                    ProfilesDataTable.PrimaryKey = new DataColumn[] { ProfilesDataTable.Columns["ID"] };
                }
                Session["Profiles"] = ProfilesDataTable;
                ProfilesGridView.DataSource = ProfilesDataTable;
                ProfilesGridView.DataBind();


            }
            catch (Exception ex)
            {

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
            { throw ex; }
            finally { }
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
            { throw ex; }
            finally { }
        }
        private void FillProfilesGridfromSession()
        {
            try
            {

                ProfilesDataTable = new DataTable();
                if (Session["Profiles"] != null && Session["Profiles"] != "")
                {
                    ProfilesDataTable = (DataTable)Session["Profiles"];
                }
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
                            ProfilesDataTable.Rows[i]["isSelected"] ="true";
                        }
                        else
                        { 
                            ProfilesDataTable.Rows[i]["isSelected"] = "false";                       
                        }

                    }

                    ProfilesGridView.DataSource =  ProfilesDataTable;
                    ProfilesGridView.DataBind();
                }
            }
            catch (Exception ex)
            { throw ex; }
            finally { }
        }

        //protected void OKButton_Click(object sender, EventArgs e)
        //{
        //    UpdateUserProfileData();
        //}
        //private void UpdateUserProfileData()
        //{
        //    lblMessage.Text = "";
        //    List<Object> SelectItemServerID = ProfilesGridView.GetSelectedFieldValues("ID");
        //    if (SelectItemServerID.Count == 0)
        //    { 
            
        //    }
        //    else
        //    {
        //        int id = int.Parse(Session["ProfileId"].ToString());
        //        Object ReturnValue = new Object();
        //        UserProfileMaster objUserProfile = CollectDataForUserProfiles(Session["Mode"].ToString(), id);
        //        if (Session["Mode"] == "Insert")
        //        {
        //            ReturnValue = VSWebBL.SecurityBL.UserProfileMasterBL.Ins.InsertData(objUserProfile);
        //            if (ReturnValue == "")
        //            {
        //                //ReturnValue = "Profile Already Exists";
        //                lblMessage.Text = "Profile Already Exists"; ;
        //            }
        //            else
        //            {
        //                Session["Mode"] = "Update";
        //            }
        //        }
        //        if (Session["Mode"] == "Update")
        //        {
        //            ReturnValue = VSWebBL.SecurityBL.UserProfileMasterBL.Ins.UpdateData(objUserProfile);

        //        }
        //        if (ReturnValue == "true")
        //        {
        //            UserProfileMaster rtnUserProfile = VSWebBL.SecurityBL.UserProfileMasterBL.Ins.GetDataForName(objUserProfile);
        //            UpdateProfilesGridData(rtnUserProfile.ID);
        //        }
        //    }
        //}

        private void UpdateProfilesGridData(int UserProfileMasterID)
        {
             GridViewDataColumn column2 = ProfilesGridView.Columns["DefaultValue"] as GridViewDataColumn;

            List<UserProfileDetailed> list = new List<UserProfileDetailed>();
            DataTable dt = new DataTable();
            int startIndex = ProfilesGridView.PageIndex * ProfilesGridView.SettingsPager.PageSize;
            int endIndex = Math.Min(ProfilesGridView.VisibleRowCount, startIndex + ProfilesGridView.SettingsPager.PageSize);
            int ProfileMasterID = 0;
            for (int i = 0; i < ProfilesDataTable.Rows.Count; i++)
            {
                if (ProfilesDataTable.Rows[i]["isSelected"] == "true")
                {
                     ProfileMasterID = Convert.ToInt32(ProfilesDataTable.Rows[i]["ID"]);
                    list.Add(new UserProfileDetailed(i, UserProfileMasterID, ProfileMasterID, ProfilesDataTable.Rows[i]["DefaultValue"].ToString()));
                }               
            }

            //for (int i = 0; i < ProfilesGridView.VisibleRowCount; i++)
            //{
            //    if (ProfilesGridView.Selection.IsRowSelected(i))
            //    {
            //        ASPxTextBox txtValue = (ASPxTextBox)ProfilesGridView.FindRowCellTemplateControl(i, column2, "txtValue");
            //        int ProfileMasterID = Convert.ToInt32(ProfilesGridView.GetRowValues(i, ProfilesGridView.KeyFieldName));
            //        list.Add(new UserProfileDetailed(i, UserProfileMasterID, ProfileMasterID, txtValue.Text));
            //    }
            //}
            if (list.Count > 0)
            {
                int Update = VSWebBL.SecurityBL.UserProfileMasterBL.Ins.UpdateProfileDetails(list);
                if(Update !=0) lblMessage.Text = "Profile saved successfully."; 
            }
        }

        //private UserProfileMaster CollectDataForUserProfiles(string Mode, int id)
        //{
        //    try
        //    {
        //        UserProfileMaster UserProfilesObject = new UserProfileMaster();
        //        UserProfilesObject.Name = ProfileTextBox.Text;// UserProfilesRow["Name"].ToString();
        //        if (Mode == "Update")
        //            UserProfilesObject.ID = id;// int.Parse(UserProfilesRow["ID"].ToString());

        //        return UserProfilesObject;
        //    }
        //    catch (Exception ex)
        //    { throw ex; }
        //    finally { }
        //}
        //protected void CancelButton_Click(object sender, EventArgs e)
        //{
        //    Response.Redirect("UserProfilesGrid.aspx");

        //}

        //public void fillServersTreeListfromSession()
        //{
        //    try
        //    {
        //        if (Session["DataServers"] != null)
        //        {
        //            DataTable ServersGridViewTable = new DataTable();
        //            if (Session["DataServers"] != null && Session["DataServers"] != "")
        //            {
        //                ServersGridViewTable = (DataTable)Session["DataServers"];
        //            }
        //            if (ServersGridViewTable.Rows.Count > 0)
        //            {
        //                ServersGridView.DataSource = (DataTable)Session["DataServers"];
        //                ServersGridView.DataBind();
        //            }
        //        }


        //        //    DataTable DataServersTree = VSWebBL.SecurityBL.ServersBL.Ins.GetAllDataByServerType(ServerTypeComboBox.SelectedItem.Text);
        //        //    Session["DataServers"] = DataServersTree;
        //        //}
        //        //ServersGridView.DataSource = (DataTable)Session["DataServers"];
        //        //ServersGridView.DataBind();

        //        //DataTable dtSel = VSWebBL.ConfiguratorBL.AlertsBL.Ins.GetSelectedServers(AlertKey);
        //        //if (dtSel.Rows.Count > 0)
        //        //{
        //        //    TreeListNodeIterator iterator = ServersTreeList.CreateNodeIterator();
        //        //    TreeListNode node;
        //        //    for (int i = 0; i < dtSel.Rows.Count; i++)
        //        //    {
        //        //        if (Convert.ToInt32(dtSel.Rows[i]["ServerID"]) == 0 && Convert.ToInt32(dtSel.Rows[i]["UserProfileID"]) == 0)
        //        //        {
        //        //            //select all
        //        //            while (true)
        //        //            {
        //        //                node = iterator.GetNext();
        //        //                if (node == null) break;
        //        //                node.Selected = true;
        //        //            }
        //        //        }
        //        //        else if (Convert.ToInt32(dtSel.Rows[i]["ServerID"]) == 0 && (Convert.ToInt32(dtSel.Rows[i]["UserProfileID"]) != 0))
        //        //        {
        //        //            //parent selected
        //        //            while (true)
        //        //            {
        //        //                node = iterator.GetNext();
        //        //                if (node == null) break;
        //        //                if ((Convert.ToInt32(dtSel.Rows[i]["UserProfileID"]) == Convert.ToInt32(node.GetValue("actid"))) && node.GetValue("tbl").ToString() == "UserProfiles")
        //        //                {
        //        //                    node.Selected = true;
        //        //                }
        //        //                else if (node.GetValue("LocId").ToString() != "")
        //        //                {
        //        //                    if ((Convert.ToInt32(dtSel.Rows[i]["UserProfileID"]) == Convert.ToInt32(node.GetValue("LocId"))) && node.GetValue("tbl").ToString() != "UserProfiles")
        //        //                    {
        //        //                        node.Selected = true;
        //        //                    }
        //        //                }
        //        //            }
        //        //        }
        //        //        else if (Convert.ToInt32(dtSel.Rows[i]["ServerID"]) != 0 && (Convert.ToInt32(dtSel.Rows[i]["UserProfileID"]) != 0))
        //        //        {
        //        //            //specific selected
        //        //            while (true)
        //        //            {

        //        //                node = iterator.GetNext();
        //        //                if (node == null) break;
        //        //                if ((Convert.ToInt32(dtSel.Rows[i]["ServerID"]) == Convert.ToInt32(node.GetValue("actid"))) && node.GetValue("tbl").ToString() != "UserProfiles")
        //        //                {
        //        //                    node.Selected = true;
        //        //                }

        //        //            }
        //        //        }
        //        //        iterator.Reset();
        //        //    }
        //        //}
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //}
        //public void fillServersTreeList()
        //{
        //    try
        //    {
        //        if (Session["DataServers"] == null)
        //        {
        //            DataTable DataServersTree = VSWebBL.SecurityBL.ServersBL.Ins.GetAllDataByServerType(ServerTypeComboBox.SelectedItem.Text);
        //            Session["DataServers"] = DataServersTree;
        //        }

        //        ServersGridView.DataSource = (DataTable)Session["DataServers"];
        //        ServersGridView.DataBind();

        //        //DataTable dtSel = VSWebBL.ConfiguratorBL.AlertsBL.Ins.GetSelectedServers(AlertKey);
        //        //if (dtSel.Rows.Count > 0)
        //        //{
        //        //    TreeListNodeIterator iterator = ServersTreeList.CreateNodeIterator();
        //        //    TreeListNode node;
        //        //    for (int i = 0; i < dtSel.Rows.Count; i++)
        //        //    {
        //        //        if (Convert.ToInt32(dtSel.Rows[i]["ServerID"]) == 0 && Convert.ToInt32(dtSel.Rows[i]["UserProfileID"]) == 0)
        //        //        {
        //        //            //select all
        //        //            while (true)
        //        //            {
        //        //                node = iterator.GetNext();
        //        //                if (node == null) break;
        //        //                node.Selected = true;
        //        //            }
        //        //        }
        //        //        else if (Convert.ToInt32(dtSel.Rows[i]["ServerID"]) == 0 && (Convert.ToInt32(dtSel.Rows[i]["UserProfileID"]) != 0))
        //        //        {
        //        //            //parent selected
        //        //            while (true)
        //        //            {
        //        //                node = iterator.GetNext();
        //        //                if (node == null) break;
        //        //                if ((Convert.ToInt32(dtSel.Rows[i]["UserProfileID"]) == Convert.ToInt32(node.GetValue("actid"))) && node.GetValue("tbl").ToString() == "UserProfiles")
        //        //                {
        //        //                    node.Selected = true;
        //        //                }
        //        //                else if (node.GetValue("LocId").ToString() != "")
        //        //                {
        //        //                    if ((Convert.ToInt32(dtSel.Rows[i]["UserProfileID"]) == Convert.ToInt32(node.GetValue("LocId"))) && node.GetValue("tbl").ToString() != "UserProfiles")
        //        //                    {
        //        //                        node.Selected = true;
        //        //                    }
        //        //                }
        //        //            }
        //        //        }
        //        //        else if (Convert.ToInt32(dtSel.Rows[i]["ServerID"]) != 0 && (Convert.ToInt32(dtSel.Rows[i]["UserProfileID"]) != 0))
        //        //        {
        //        //            //specific selected
        //        //            while (true)
        //        //            {

        //        //                node = iterator.GetNext();
        //        //                if (node == null) break;
        //        //                if ((Convert.ToInt32(dtSel.Rows[i]["ServerID"]) == Convert.ToInt32(node.GetValue("actid"))) && node.GetValue("tbl").ToString() != "UserProfiles")
        //        //                {
        //        //                    node.Selected = true;
        //        //                }

        //        //            }
        //        //        }
        //        //        iterator.Reset();
        //        //    }
        //        //}
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //}

        public void ApplyToServers_Click(object sender, EventArgs e)
        {
            //12/12/2013 NS added
            errorDiv.Style.Value = "display: none;";
            errorDiv2.Style.Value = "display: none";
            List<object> fieldValues = ProfilesGridView.GetSelectedFieldValues(new string[] { "RelatedField", "DefaultValue", "RelatedTable" });
            //List<object> serversSelected = ServersGridView.GetSelectedFieldValues(new string[] {"ID","ServerName" });
            DataTable dt = GetSelectedServers();
            List<DataRow> serversSelected = dt.AsEnumerable().ToList();
            //List<DataRow> list = dt.AsEnumerable().ToList();
            int Update = 0;
            string ServerErrors ="";
            if (fieldValues.Count > 0 && serversSelected.Count > 0)
            {
                GridViewDataColumn column2 = ProfilesGridView.Columns["DefaultValue"] as GridViewDataColumn;
                
                List<ProfilesMaster> list = new List<ProfilesMaster>();
                 
                int startIndex = ProfilesGridView.PageIndex * ProfilesGridView.SettingsPager.PageSize;
                int endIndex = Math.Min(ProfilesGridView.VisibleRowCount, startIndex + ProfilesGridView.SettingsPager.PageSize);
                int ProfileMasterID = 0;
                for (int i = 0; i < ProfilesDataTable.Rows.Count; i++)
                {
                    if (ProfilesDataTable.Rows[i]["isSelected"].ToString() == "true")
                    {
                        ProfileMasterID = Convert.ToInt32(ProfilesDataTable.Rows[i]["ID"]);
                        list.Add(new ProfilesMaster(0, 0, "", ProfilesDataTable.Rows[i]["AttributeName"].ToString(), "", ProfilesDataTable.Rows[i]["DefaultValue"].ToString(), ProfilesDataTable.Rows[i]["RelatedTable"].ToString(), ProfilesDataTable.Rows[i]["RelatedField"].ToString()));
                        //list.Add(new UserProfileDetailed(i, UserProfileMasterID, ProfileMasterID, ProfilesDataTable.Rows[i]["DefaultValue"].ToString()));
                    }
                }
                //for (int i = 0; i < ProfilesGridView.VisibleRowCount; i++)
                //{
                //    if (ProfilesGridView.Selection.IsRowSelected(i))
                //    {
                //        ASPxTextBox txtValue = (ASPxTextBox)ProfilesGridView.FindRowCellTemplateControl(i, column2, "txtValue");
                //        int ProfileMasterID = Convert.ToInt32(ProfilesGridView.GetRowValues(i, ProfilesGridView.KeyFieldName));
                //        list.Add(new ProfilesMaster(0, 0, "", "", "", txtValue.Text, ProfilesGridView.GetRowValues(i, "RelatedTable").ToString(), ProfilesGridView.GetRowValues(i, "RelatedField").ToString()));
                //        //i, UserProfileMasterID, ProfileMasterID, txtValue.Text

                //    }
                //}
                //foreach (object[] server in serversSelected)
                string AppliedServers= "";
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
                    errorDiv2.InnerHtml = "Settings for the server(s) " + ServerErrors + " were NOT updated.";
                    errorDiv2.Style.Value = "display: block";
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
                            parameters += ", " + fieldValue.AttributeName + " = "  + fieldValue.DefaultValue;
                        }
                    }
                    ProfilesGridView.Selection.UnselectAll();
                    ServersTreeList.UnselectAll();

                    //12/12/2013 NS moved the code into the else block, otherwise on unsuccessful apply
                    //the whole server list would get wiped out
                    //Clearing the Data Grids
                    ServerTypeComboBox.SelectedIndex = -1;
                    ProfilesGridView.DataSource = null;
                    ProfilesGridView.DataBind();
                    //ServersGridView.DataSource = null;
                    //ServersGridView.DataBind();
                    ServersTreeList.ClearNodes();
                    ServersTreeList.DataSource = null;
                    ServersTreeList.DataBind();
                    Session["DataServers"] = null;
                    Session["Profiles"] = null;
                    //12/12/2013 NS modified
                    //lblMessage.Text = "Settings: " + parameters + " for the selected servers: " + AppliedServers + " are updated";
                    successDiv.InnerHtml = "The following settings for the server(s) " + AppliedServers + " were successfully updated: " + parameters;
                    successDiv.Style.Value = "display: block";
                    ApplyServersButton.Style.Value = "visibility: hidden";
                    tblServer.Style.Value = "visibility: hidden";
                }
            }
            else
            {
                //12/12/2013 NS modified
                //lblError.Text = "Please select required Attributes and Servers";
                errorDiv.Style.Value = "display: block";
            }
        }

        public void fillServersTreeList()
        {
            try
            {
                if (Session["DataServers"] == null)
                {
                    DataTable DataServersTree = VSWebBL.ConfiguratorBL.AlertsBL.Ins.GetServersFromProcedure();
                    DataTable filteredData = DataServersTree.Select("ServerType='" + ServerTypeComboBox.Text + "' or ServerType is null").CopyToDataTable();
                    Session["DataServers"] = filteredData;
                }

                ServersTreeList.DataSource = (DataTable)Session["DataServers"];
                ServersTreeList.DataBind();
                ServersTreeList.ExpandAll();

                
            }
            catch (Exception ex)
            {
                //Log.Entry.Ins.Write(Server.MapPath("~/LogFiles/"), "VSPlusLog.txt", DateTime.Now.ToString() + " Error in Page: " +
                //    Request.Url.AbsolutePath + ", Method: " + System.Reflection.MethodBase.GetCurrentMethod().Name +
                //    ", Error: " + ex.ToString());
            }
        }

        public void fillDominoServersTreeList()
        {
            try
            {
                if (Session["DominoServers"] == null)
                {
                    DataTable DataServersTree = VSWebBL.ConfiguratorBL.AlertsBL.Ins.GetServersFromProcedure();
                    DataTable filteredData = DataServersTree.Select("ServerType= 'Domino' or ServerType is null").CopyToDataTable();
                    Session["DominoServers"] = filteredData;
                }

                DominoServerTreeList.DataSource = (DataTable)Session["DominoServers"];
                DominoServerTreeList.DataBind();
                DominoServerTreeList.ExpandAll();


            }
            catch (Exception ex)
            {
                //Log.Entry.Ins.Write(Server.MapPath("~/LogFiles/"), "VSPlusLog.txt", DateTime.Now.ToString() + " Error in Page: " +
                //    Request.Url.AbsolutePath + ", Method: " + System.Reflection.MethodBase.GetCurrentMethod().Name +
                //    ", Error: " + ex.ToString());
            }
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
                //Log.Entry.Ins.Write(Server.MapPath("~/LogFiles/"), "VSPlusLog.txt", DateTime.Now.ToString() + " Error in Page: " +
                //    Request.Url.AbsolutePath + ", Method: " + System.Reflection.MethodBase.GetCurrentMethod().Name +
                //    ", Error: " + ex.ToString());
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
            }

            return dtSel;

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
            }
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
            }
        }

        protected void AddTasksToDominoServers_Click(object sender, EventArgs e)
        {
            UpdateTasksForDominoServers(1);            
        }

        protected void RemoveTasksFromDominoServers_Click(object sender, EventArgs e)
        {
            UpdateTasksForDominoServers(0);
        }
        protected void UpdateTasksForDominoServers(int Enabled)
        {
            successDivDomino.Style.Value = "display: none";
            errorDiv3.Style.Value = "display: none;";
            errorDiv4.Style.Value = "display: none";
            List<object> fieldValues = DominoTasksGridView.GetSelectedFieldValues(new string[] { "TaskID" });//, "SendLoadCommand", "SendRestartCommand", "RestartOffHours", "SendExitCommand" 
            //List<object> serversSelected = ServersGridView.GetSelectedFieldValues(new string[] {"ID","ServerName" });
            DataTable dt = GetSelectedDominoServers();
            List<DataRow> serversSelected = dt.AsEnumerable().ToList();
            //List<DataRow> list = dt.AsEnumerable().ToList();
            int Update = 0;
            string ServerErrors = "";

            if (fieldValues.Count > 0 && serversSelected.Count > 0)
            {
                foreach (DataRow server in serversSelected)
                {
                    string AppliedServers = "";
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
                            Update = VSWebBL.ConfiguratorBL.DominoServerTasksBL.Ins.UpdateDominoServerTasks(Convert.ToInt32(DSTasksDataTable.Rows[i]["TaskID"]), Convert.ToInt32(server[0]), Enabled,SendLoadCommand, SendRestartCommand, RestartOffHours, SendExitCommand);
                            if (Update == 0 )
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
                                errorDiv4.InnerHtml = "Settings for the server(s) " + ServerErrors + " were NOT updated.";
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
                                successDivDomino.InnerHtml = "The following settings for the server(s) " + AppliedServers + " were successfully updated: " + parameters;
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
            }

            return dtSel;

        }
        
    }
}