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
namespace VSWebUI.Security
{
    public partial class UserProfiles : System.Web.UI.Page
    {
		protected void Page_PreInit(object sender, EventArgs e)
		{

			this.Master.contentCallEvent += new EventHandler(Master_ButtonClick);
		}

		private void Master_ButtonClick(object sender, EventArgs e)
		{

		}
        object msgloc = "";
        protected DataTable ProfilesDataTable = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                tblServer.Style.Add("visibility", "hidden");
                ApplyServersButton.Style.Add("visibility", "hidden");
                Session["Mode"] = "Insert";
                Session["ProfileId"] = "0";
                ProfileTextBox.Enabled = true;
                FillServerTypeComboBox();
                if (Request.QueryString["ProfileId"] != null && Request.QueryString["ProfileId"] != "")
                {
                    Session["ProfileId"] = Request.QueryString["ProfileId"];
                    Session["Mode"] = "Update";
                    ProfileTextBox.Enabled = false;
                    FillData(int.Parse(Session["ProfileId"].ToString()));
                }
                if (Session["UserPreferences"] != null)
                {
                    DataTable UserPreferences = (DataTable)Session["UserPreferences"];
                    foreach (DataRow dr in UserPreferences.Rows)
                    {
                        if (dr[1].ToString() == "UserProfiles|ProfilesGridView")
                        {
                            ProfilesGridView.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                        }
                        if (dr[1].ToString() == "UserProfiles|ServersGridView")
                        {
                            ServersGridView.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                        }
                    }
                }
            }
        }
        private void FillData(int ProfileId)
        {
            UserProfileMaster LocObject =new UserProfileMaster();
            LocObject.ID = ProfileId;
            UserProfileMaster ProfileMasterDataTable = VSWebBL.SecurityBL.UserProfileMasterBL.Ins.GetData(LocObject);
            ProfileTextBox.Text = ProfileMasterDataTable.Name;

            DataTable ProfileDtlDataTable = VSWebBL.SecurityBL.UserProfileMasterBL.Ins.GetUserProfileDetailedData(LocObject);
            if (ProfileDtlDataTable.Rows.Count > 0)
            {
                tblServer.Style.Add("visibility", "visible");
                ApplyServersButton.Style.Add("visibility", "visible");

                ServerTypeComboBox.Text = ProfileDtlDataTable.Rows[0]["ServerType"].ToString();


                FillProfilesGrid(ServerTypeComboBox.SelectedItem.Text);
                fillServersTreeList();
                GridViewDataColumn column2 = ProfilesGridView.Columns["Value"] as GridViewDataColumn;
                for (int i = 0; i < ProfileDtlDataTable.Rows.Count; i++)
                {
                    for (int j = 0; j < ProfilesGridView.VisibleRowCount; j++)
                    {
                        if (ProfilesGridView.GetRowValues(j, ProfilesGridView.KeyFieldName).ToString() == ProfileDtlDataTable.Rows[i]["ProfilesMasterId"].ToString())
                        {
                            ProfilesGridView.Selection.SelectRow(j);
                            ASPxTextBox txtValue = (ASPxTextBox)ProfilesGridView.FindRowCellTemplateControl(j, column2, "txtValue");
                            txtValue.Text = ProfileDtlDataTable.Rows[i]["Value"].ToString();
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
                ApplyServersButton.Style.Add("visibility", "visible");
                tblServer.Style.Add("visibility", "visible");
                FillProfilesGrid(ServerTypeComboBox.SelectedItem.Text);
                fillServersTreeList();
            }

        }
        private void FillProfilesGrid(string ServerType)
        {
            try
            {

                ProfilesDataTable = new DataTable();
                DataSet ProfilesDataSet = new DataSet();
                ProfilesDataTable = VSWebBL.SecurityBL.ProfilesMasterBL.Ins.GetAllDataByServerType(ServerType,"","");
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

                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;


            }
            finally { }
        }

        protected void OKButton_Click(object sender, EventArgs e)
        {
            UpdateUserProfileData();
        }
        private void UpdateUserProfileData()
        {
            lblMessage.Text = "";
            List<Object> SelectItemServerID = ProfilesGridView.GetSelectedFieldValues("ID");
            if (SelectItemServerID.Count == 0)
            { 
            
            }
            else
            {
                int id = int.Parse(Session["ProfileId"].ToString());
                Object ReturnValue = new Object();
                UserProfileMaster objUserProfile = CollectDataForUserProfiles(Session["Mode"].ToString(), id);
                if (Session["Mode"] == "Insert")
                {
                    ReturnValue = VSWebBL.SecurityBL.UserProfileMasterBL.Ins.InsertData(objUserProfile);
                    if (ReturnValue == "")
                    {
                        //ReturnValue = "Profile Already Exists";
                        lblMessage.Text = "Profile Already Exists"; ;
                    }
                    else
                    {
                        Session["Mode"] = "Update";
                    }
                }
                if (Session["Mode"] == "Update")
                {
                    ReturnValue = VSWebBL.SecurityBL.UserProfileMasterBL.Ins.UpdateData(objUserProfile);

                }
                if (ReturnValue == "true")
                {
                    UserProfileMaster rtnUserProfile = VSWebBL.SecurityBL.UserProfileMasterBL.Ins.GetDataForName(objUserProfile);
                    UpdateProfilesGridData(rtnUserProfile.ID);
                }
            }
        }

        private void UpdateProfilesGridData(int UserProfileMasterID)
        {

           // List<Object> SelectItemServerID = ProfilesGridView.GetSelectedFieldValues(new string[] { "ID", "Value" });
 
            GridViewDataColumn column2 = ProfilesGridView.Columns["Value"] as GridViewDataColumn;

            List<UserProfileDetailed> list = new List<UserProfileDetailed>();
            
            for (int i = 0; i < ProfilesGridView.VisibleRowCount; i++)
            {
                if (ProfilesGridView.Selection.IsRowSelected(i))
                {
                    ASPxTextBox txtValue = (ASPxTextBox)ProfilesGridView.FindRowCellTemplateControl(i, column2, "txtValue");
                    int ProfileMasterID = Convert.ToInt32(ProfilesGridView.GetRowValues(i, ProfilesGridView.KeyFieldName));
                    list.Add(new UserProfileDetailed(i, UserProfileMasterID, ProfileMasterID, txtValue.Text));

                }
            }
            if (list.Count > 0)
            {
                int Update = VSWebBL.SecurityBL.UserProfileMasterBL.Ins.UpdateProfileDetails(list);
                if(Update !=0) lblMessage.Text = "Profile saved successfully."; 
            }
        }

        private UserProfileMaster CollectDataForUserProfiles(string Mode, int id)
        {
            try
            {
                UserProfileMaster UserProfilesObject = new UserProfileMaster();
                UserProfilesObject.Name = ProfileTextBox.Text;// UserProfilesRow["Name"].ToString();
                if (Mode == "Update")
                    UserProfilesObject.ID = id;// int.Parse(UserProfilesRow["ID"].ToString());

                return UserProfilesObject;
            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
            finally { }
        }
        protected void CancelButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("UserProfilesGrid.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
            Context.ApplicationInstance.CompleteRequest();

        }

        public void fillServersTreeList()
        {
            try
            {
                if (Session["DataServers"] == null)
                {
                    DataTable DataServersTree = VSWebBL.SecurityBL.ServersBL.Ins.GetAllDataByServerType(ServerTypeComboBox.SelectedItem.Text);
                    Session["DataServers"] = DataServersTree;
                }

                ServersGridView.DataSource = (DataTable)Session["DataServers"];
                ServersGridView.DataBind();

                //DataTable dtSel = VSWebBL.ConfiguratorBL.AlertsBL.Ins.GetSelectedServers(AlertKey);
                //if (dtSel.Rows.Count > 0)
                //{
                //    TreeListNodeIterator iterator = ServersTreeList.CreateNodeIterator();
                //    TreeListNode node;
                //    for (int i = 0; i < dtSel.Rows.Count; i++)
                //    {
                //        if (Convert.ToInt32(dtSel.Rows[i]["ServerID"]) == 0 && Convert.ToInt32(dtSel.Rows[i]["UserProfileID"]) == 0)
                //        {
                //            //select all
                //            while (true)
                //            {
                //                node = iterator.GetNext();
                //                if (node == null) break;
                //                node.Selected = true;
                //            }
                //        }
                //        else if (Convert.ToInt32(dtSel.Rows[i]["ServerID"]) == 0 && (Convert.ToInt32(dtSel.Rows[i]["UserProfileID"]) != 0))
                //        {
                //            //parent selected
                //            while (true)
                //            {
                //                node = iterator.GetNext();
                //                if (node == null) break;
                //                if ((Convert.ToInt32(dtSel.Rows[i]["UserProfileID"]) == Convert.ToInt32(node.GetValue("actid"))) && node.GetValue("tbl").ToString() == "UserProfiles")
                //                {
                //                    node.Selected = true;
                //                }
                //                else if (node.GetValue("LocId").ToString() != "")
                //                {
                //                    if ((Convert.ToInt32(dtSel.Rows[i]["UserProfileID"]) == Convert.ToInt32(node.GetValue("LocId"))) && node.GetValue("tbl").ToString() != "UserProfiles")
                //                    {
                //                        node.Selected = true;
                //                    }
                //                }
                //            }
                //        }
                //        else if (Convert.ToInt32(dtSel.Rows[i]["ServerID"]) != 0 && (Convert.ToInt32(dtSel.Rows[i]["UserProfileID"]) != 0))
                //        {
                //            //specific selected
                //            while (true)
                //            {

                //                node = iterator.GetNext();
                //                if (node == null) break;
                //                if ((Convert.ToInt32(dtSel.Rows[i]["ServerID"]) == Convert.ToInt32(node.GetValue("actid"))) && node.GetValue("tbl").ToString() != "UserProfiles")
                //                {
                //                    node.Selected = true;
                //                }

                //            }
                //        }
                //        iterator.Reset();
                //    }
                //}
            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
        }

        protected void ProfilesGridView_PageSizeChanged(object sender, EventArgs e)
        {
            //ProfilesGridView.PageIndex;
            VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("UserProfiles|ProfilesGridView", ProfilesGridView.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        }
        protected void ServersGridView_PageSizeChanged(object sender, EventArgs e)
        {
            //ProfilesGridView.PageIndex;
            VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("UserProfiles|ServersGridView", ServersGridView.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        }
    }
}