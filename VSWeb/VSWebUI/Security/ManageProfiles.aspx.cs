using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;

using DevExpress.Web;

using VSWebBL;
using VSWebDO;

namespace VSWebUI.Security
{
    public partial class ManageProfiles : System.Web.UI.Page
    {
		protected void Page_PreInit(object sender, EventArgs e)
		{

			this.Master.contentCallEvent += new EventHandler(Master_ButtonClick);
		}

		private void Master_ButtonClick(object sender, EventArgs e)
		{

		}
        protected DataTable ProfilesDataTable = null;

        static string Id= "";

        void Page_Load(object sender, EventArgs e)
        {
            pnlAreaDtls.Style.Add("visibility", "hidden");
            if (!IsPostBack)
            {
                Session["Submenu"] = "";
                FillProfilesGrid();
                if (Session["UserPreferences"] != null)
                {
                    DataTable UserPreferences = (DataTable)Session["UserPreferences"];
                    foreach (DataRow dr in UserPreferences.Rows)
                    {
                        if (dr[1].ToString() == "ManageProfiles|ProfilesGridView")
                        {
                            ProfilesGridView.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                        }
                    }
                }
            }
            else
            {
                FillProfilesGridfromSession();
            }
        }
        private void FillProfilesGrid()
        {
            try
            {

                ProfilesDataTable = new DataTable();
                DataSet ProfilesDataSet = new DataSet();
                ProfilesDataTable = VSWebBL.SecurityBL.ProfilesMasterBL.Ins.GetAllData();
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
                //if (ex.Message == "These columns don't currently have unique values.")
                //{
                //    ProfilesGridView.ProfilesText.EmptyDataRow = "There are duplicate Profiles entered in the database. Cannot display data. Delete the duplicates from backend and retry.";
                //}
                //else 
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex; 
            
            
            }
            finally { }
        }

        private void FillProfilesGridfromSession()
        {
            try
            {

                ProfilesDataTable = new DataTable();
                if (Session["Profiles"] != null && Session["Profiles"] != "")
                    ProfilesDataTable = (DataTable)Session["Profiles"];
                if (ProfilesDataTable.Rows.Count > 0)
                {
                    ProfilesGridView.DataSource = ProfilesDataTable;
                    ProfilesGridView.DataBind();
                }
            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
            finally { }
        }
        private void FillServerTypeComboBox(ASPxComboBox ServerTypeComboBox)
        {
            DataTable ServerDataTable = VSWebBL.SecurityBL.ServerTypesBL.Ins.GetAllData();
           ServerTypeComboBox.DataSource = ServerDataTable;
            ServerTypeComboBox.TextField = "ServerType";
            ServerTypeComboBox.ValueField = "ServerType";
            ServerTypeComboBox.DataBind();
        }
        private void FillRoleTypeComboBox(ASPxComboBox ServerTypeComboBox)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("RoleType");
            dt.Columns.Add("RoleTypeValue");
            dt.Rows.Add("No role","");
            dt.Rows.Add("CAS", "CAS");
            dt.Rows.Add("EDGE","EDGE");
            dt.Rows.Add("MailBox","MailBox");
            dt.Rows.Add("HUB", "HUB");
            ServerTypeComboBox.DataSource = dt;
            ServerTypeComboBox.TextField = "RoleType";
            ServerTypeComboBox.ValueField = "RoleType";
            ServerTypeComboBox.DataBind();
        }
        private void FillTableComboBox(ASPxComboBox TableComboBox, string TableName)
        {
            DataTable ServerDataTable = VSWebBL.SecurityBL.ProfilesMasterBL.Ins.GetColumns(TableName);
            TableComboBox.DataSource = ServerDataTable;
            TableComboBox.TextField = "COLUMN_NAME";
            TableComboBox.ValueField = "COLUMN_NAME";
            TableComboBox.DataBind();
        }
        private void FillColumnComboBox(ASPxComboBox TableComboBox, string TableName)
        {
            DataTable ServerDataTable = VSWebBL.SecurityBL.ProfilesMasterBL.Ins.GetColumns(TableName);
            TableComboBox.DataSource = ServerDataTable;
            TableComboBox.TextField = "COLUMN_NAME";
            TableComboBox.ValueField = "COLUMN_NAME";
            TableComboBox.DataBind();
        }
        protected void ProfilesGridView_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            try
            {
                ASPxComboBox cmbType = (ASPxComboBox)ProfilesGridView.FindEditFormTemplateControl("cmbType");
                 if (cmbType.SelectedItem != null)
                {
                    //if (cmbType.SelectedItem.Text != "Specific Hours")
                    //{
                       
                    //}

                    //FillTableComboBox(cmbType, cmbType.SelectedItem.Text);
                }
            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
        }
        protected void ProfilesGridView_AutoFilterCellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        {
            if (e.Column.FieldName == "ServerType")
            {
                ASPxComboBox ServerTypeComboBox = e.Editor as ASPxComboBox;
                FillServerTypeComboBox(ServerTypeComboBox);
                ServerTypeComboBox.Callback += new CallbackEventHandlerBase(ServerTypeComboBox_OnCallback);
            }
            if (e.Column.FieldName == "RelatedField")
            {
                //object val = ProfilesGridView.GetRowValuesByKeyValue(e.KeyValue, "RelatedTable");
                //if (val == null) return;
                //string country = (string)val;

                ASPxComboBox combo = e.Editor as ASPxComboBox;
                //FillTableComboBox(combo, country);

                combo.Callback += new CallbackEventHandlerBase(cmbColumn_OnCallback);
            }
            if (e.Column.FieldName == "RoleType")
            {
                //object val = ProfilesGridView.GetRowValuesByKeyValue(e.KeyValue, "RelatedTable");
                //if (val == null) return;
                //string country = (string)val;

                ASPxComboBox combo = e.Editor as ASPxComboBox;
                //FillTableComboBox(combo, country);

                combo.Callback += new CallbackEventHandlerBase(RoleTypeComboBox_OnCallback);
            }
        }
        protected void ProfilesGridView_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        {
            if (e.Column.FieldName == "ServerType")
            {
                ASPxComboBox ServerTypeComboBox = e.Editor as ASPxComboBox;
                FillServerTypeComboBox(ServerTypeComboBox);
                ServerTypeComboBox.Callback += new CallbackEventHandlerBase(ServerTypeComboBox_OnCallback);
            }
            if (e.Column.FieldName == "RelatedField")
            {
                //object val = ProfilesGridView.GetRowValuesByKeyValue(e.KeyValue, "RelatedTable");
                //if (val == null) return;
                //string country = (string)val;

                ASPxComboBox combo = e.Editor as ASPxComboBox;
                //FillTableComboBox(combo, country);

                combo.Callback += new CallbackEventHandlerBase(cmbColumn_OnCallback);
            }
            if (e.Column.FieldName == "RoleType")
            {
                //object val = ProfilesGridView.GetRowValuesByKeyValue(e.KeyValue, "RelatedTable");
                //if (val == null) return;
                //string country = (string)val;

                ASPxComboBox combo = e.Editor as ASPxComboBox;
                //FillTableComboBox(combo, country);

                combo.Callback += new CallbackEventHandlerBase(RoleTypeComboBox_OnCallback);
            }
        }
        void cmbColumn_OnCallback(object source, CallbackEventArgsBase e)
        {
            FillTableComboBox(source as ASPxComboBox, e.Parameter);
        }
        private void ServerTypeComboBox_OnCallback(object source, CallbackEventArgsBase e)
        {
            FillServerTypeComboBox(source as ASPxComboBox);
        }

        private void RoleTypeComboBox_OnCallback(object source, CallbackEventArgsBase e)
        {
            FillRoleTypeComboBox(source as ASPxComboBox);
        }
        protected DataRow GetRow(DataTable ProfilesObject, IDictionaryEnumerator enumerator, string Keys)
        {

            DataTable dataTable = ProfilesObject;
            DataRow DRRow = dataTable.NewRow();
            try
            {
                if (Keys == "0")
                    DRRow = dataTable.NewRow();
                else
                    DRRow = dataTable.Rows.Find(Keys);
               enumerator.Reset();
                while (enumerator.MoveNext())
                    DRRow[enumerator.Key.ToString()] = (enumerator.Value == null ? "False" : enumerator.Value);
            }
            catch(Exception ex)
            {
                if (ex.Message.IndexOf("Column 'Id' is constrained to be unique.") >= 0)
                {
                     throw new Exception("Record already exists");
                }
                else
                {
                    Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                    throw ex;
                }

            }
            return DRRow;
        }

        protected void ProfilesGridView_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            if (Session["Profiles"] != null && Session["Profiles"] != "")
                ProfilesDataTable = (DataTable)Session["Profiles"];
            ASPxGridView gridView = (ASPxGridView)sender;

            //Insert row in DB
            DataRow dr = GetRow(ProfilesDataTable, e.NewValues.GetEnumerator(), "0");
            string retsval = VSWebBL.SecurityBL.ProfilesMasterBL.Ins.Getvalue(dr["AttributeName"].ToString(), dr["ServerType"].ToString());
            if (retsval == "")
            {
                UpdateProfilesData("Insert", GetRow(ProfilesDataTable, e.NewValues.GetEnumerator(), "0"),"");
            }
            else
            {
                    throw new Exception("Record already exists");            
            }
            gridView.CancelEdit();
            e.Cancel = true;
            FillProfilesGrid();
        }

        private void UpdateProfilesData(string Mode, DataRow ProfilesRow, string strsname)
        {
            Object ReturnValue = VSWebBL.SecurityBL.ProfilesMasterBL.Ins.UpdateProfiles(CollectDataForProfiles(Mode, ProfilesRow), strsname);
        }

        private ProfilesMaster CollectDataForProfiles(string Mode, DataRow ProfilesRow)
        {
            try
            {
                ProfilesMaster ProfilesObject = new ProfilesMaster();
                //if (Mode == "Update")
                //{
                    //ProfilesObject.ID = int.Parse(ProfilesRow["ID"].ToString());
                //}
               // ProfilesObject.ServerTypeId = Convert.ToInt32(ProfilesRow["ServerTypeId"]);
                ProfilesObject.ServerType = ProfilesRow["ServerType"].ToString();
                ProfilesObject.RelatedTable = ProfilesRow["RelatedTable"].ToString();
                ProfilesObject.RelatedField = ProfilesRow["RelatedField"].ToString();
                ProfilesObject.DefaultValue = ProfilesRow["DefaultValue"].ToString();
                ProfilesObject.UnitOfMeasurement = ProfilesRow["UnitofMeasurement"].ToString();
                ProfilesObject.AttributeName = ProfilesRow["AttributeName"].ToString();
                ProfilesObject.RoleType = "";
                if(ProfilesObject.RelatedTable == "ExchangeSettings")
                {
                    ProfilesObject.RoleType = ProfilesRow["RoleType"].ToString();
                }
               
                return ProfilesObject;
            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
            finally { }
        }
        protected void ProfilesGridView_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            if (Session["Profiles"] != null && Session["Profiles"] != "")
                ProfilesDataTable = (DataTable)Session["Profiles"];
            ASPxGridView gridView = (ASPxGridView)sender;

            DataRow DRRow = ProfilesDataTable.NewRow();
            DRRow = ProfilesDataTable.Rows.Find(e.Keys[0].ToString());

            DataRow dr = GetRow(ProfilesDataTable, e.NewValues.GetEnumerator(), e.Keys[0].ToString());
            string retsval="";
            if (dr["Id"].ToString() != DRRow["Id"].ToString())
            {
                retsval = VSWebBL.SecurityBL.ProfilesMasterBL.Ins.Getvalue(dr["AttributeName"].ToString(), dr["ServerType"].ToString());
            }          
            if (retsval == "")
            {
                //Update row in DB
                UpdateProfilesData("Update", GetRow(ProfilesDataTable, e.NewValues.GetEnumerator(), e.Keys[0].ToString()), DRRow["Id"].ToString());
            }
            else
            {
                throw new Exception("Record already exists");
            }
            //Update Grid after inserting new row, refresh grid as in page load
            gridView.CancelEdit();
            e.Cancel = true;
            FillProfilesGrid();
        }

        protected void ProfilesGridView_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            ProfilesMaster StObject = new ProfilesMaster();
            StObject.Id =int.Parse(e.Keys[0].ToString());

            //Delete row from DB


            Object ReturnValue = VSWebBL.SecurityBL.ProfilesMasterBL.Ins.DeleteData(StObject);



            if (Convert.ToBoolean(ReturnValue) == false)
            {

            }
            else
            {
                ASPxGridView gridView = (ASPxGridView)sender;
                gridView.CancelEdit();
                e.Cancel = true;
                FillProfilesGrid();
            }
            //Update Grid after inserting new row, refresh grid as in page load

        }
        protected void btn_Click(object sender, EventArgs e)
        {
            ImageButton btn = (ImageButton)sender;
            ProfilesMaster ProfilesObject = new ProfilesMaster();
            ProfilesObject.Id =int.Parse(btn.CommandArgument);
            Id = btn.CommandArgument;
            string name = btn.AlternateText;
            pnlAreaDtls.Style.Add("visibility", "visible");
            divmsg.InnerHtml = "Are you sure you want to delete the Profile " + name + "?";
        }
        protected void btn_OkClick(object sender, EventArgs e)
        {
            ProfilesMaster ProfilesObject = new ProfilesMaster();
            ProfilesObject.Id =int.Parse(Id);
            Object ReturnValue =  VSWebBL.SecurityBL.ProfilesMasterBL.Ins.DeleteData(ProfilesObject);
            if (Convert.ToBoolean(ReturnValue) == false)
            {
                NavigatorPopupControl.ShowOnPageLoad = true;
                MsgLabel.Text = "This Profile cannot be deleted, other dependencies exist.";
            }
            else
            {
                //ASPxGridView gridView = (ASPxGridView)sender;
                //gridView.CancelEdit();
                //e.Cancel = true;
                FillProfilesGrid();
            }
            //pnlAreaDtls.Style.Add("visibility", "hidden");
        }
        protected void btn_CancelClick(object sender, EventArgs e)
        {
            // ASPxGridView gridView = (ASPxGridView)sender;
            //gridView.CancelEdit();
            //e.Cancel = true;
            FillProfilesGrid();
            // pnlAreaDtls.Style.Add("visibility", "hidden");
        }

        protected void ProfilesGridView_CustomErrorText(object sender, ASPxGridViewCustomErrorTextEventArgs e)
        {

        }

        protected void ProfilesGridView_PageSizeChanged(object sender, EventArgs e)
        {
            //ProfilesGridView.PageIndex;
            VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("ManageProfiles|ProfilesGridView", ProfilesGridView.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        }
    }
}