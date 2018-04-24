using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;

using DevExpress.Web;

using VSFramework;
using VSWebBL;
using VSWebDO;

namespace VSWebUI.Security
{
    public partial class ManageSettings : System.Web.UI.Page
    {
		protected void Page_PreInit(object sender, EventArgs e)
		{

			this.Master.contentCallEvent += new EventHandler(Master_ButtonClick);
		}

		private void Master_ButtonClick(object sender, EventArgs e)
		{

		}
        protected DataTable SettingsDataTable = null;

        static string sname= "";

        void Page_Load(object sender, EventArgs e)
        {
            pnlAreaDtls.Style.Add("visibility", "hidden");
            if (!IsPostBack)
            {
                FillSettingsGrid();
                if (Session["UserPreferences"] != null)
                {
                    DataTable UserPreferences = (DataTable)Session["UserPreferences"];
                    foreach (DataRow dr in UserPreferences.Rows)
                    {
                        if (dr[1].ToString() == "ManageSettings|SettingsGridView")
                        {
                            SettingsGridView.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                        }
                    }
                }
            }
            else
            {

                FillSettingsGridfromSession();

            }
        }
        private void FillSettingsGrid()
        {
            try
            {

                SettingsDataTable = new DataTable();
                DataSet SettingsDataSet = new DataSet();
                SettingsDataTable = VSWebBL.SettingBL.SettingsBL.Ins.GetAllData();
                if (SettingsDataTable.Rows.Count > 0)
                {
                    //DataTable dtcopy = SettingsDataTable.Copy();
                    //dtcopy.PrimaryKey = new DataColumn[] { dtcopy.Columns["ID"] };
                    SettingsDataTable.PrimaryKey = new DataColumn[] { SettingsDataTable.Columns["sname"] };
                }
                Session["Settings"] = SettingsDataTable;
                SettingsGridView.DataSource = SettingsDataTable;
                SettingsGridView.DataBind();


            }
            catch (Exception ex)
            {
                if (ex.Message == "These columns don't currently have unique values.")
                {
                    SettingsGridView.SettingsText.EmptyDataRow = "There are duplicate Settings entered in the database. Cannot display data. Delete the duplicates from backend and retry.";
                }
                else
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex; 
            
            
            }
            finally { }
        }

        private void FillSettingsGridfromSession()
        {
            try
            {

                SettingsDataTable = new DataTable();
                if (Session["Settings"] != null && Session["Settings"] != "")
                    SettingsDataTable = (DataTable)Session["Settings"];//VSWebBL.ConfiguratorBL.DominoPropertiesBL.Ins.GetAllData();
                if (SettingsDataTable.Rows.Count > 0)
                {
                    SettingsGridView.DataSource = SettingsDataTable;
                    SettingsGridView.DataBind();
                }
            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
            finally { }
        }


        protected DataRow GetRow(DataTable ServerObject, IDictionaryEnumerator enumerator, string Keys)
        {

            DataTable dataTable = ServerObject;
            DataRow DRRow = dataTable.NewRow();
            try
            {
                if (Keys == "0")
                    DRRow = dataTable.NewRow();
                else
                    DRRow = dataTable.Rows.Find(Keys);
                //IDictionaryEnumerator enumerator = e.NewValues.GetEnumerator();
                enumerator.Reset();
                while (enumerator.MoveNext())
                    DRRow[enumerator.Key.ToString()] = (enumerator.Value == null ? "False" : enumerator.Value);
            }
            catch(Exception ex)
            {
                if (ex.Message.IndexOf("Column 'sname' is constrained to be unique.") >= 0)
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

        protected void SettingsGridView_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            if (Session["Settings"] != null && Session["Settings"] != "")
                SettingsDataTable = (DataTable)Session["Settings"];
            ASPxGridView gridView = (ASPxGridView)sender;

            //DataTable dataTable = STSettingsDataSet.Tables[0];
            //DataRow STSettingsRow = dataTable.NewRow();
            //STSettingsRow=GetRow(STSettingsDataSet, e.NewValues.GetEnumerator());

            //Insert row in DB
            DataRow dr = GetRow(SettingsDataTable, e.NewValues.GetEnumerator(), "0");
            string retsval = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue(dr["sname"].ToString());
            if (retsval == "")
            {
                UpdateSettingsData("Insert", GetRow(SettingsDataTable, e.NewValues.GetEnumerator(), "0"),"");
                //Update Grid after inserting new row, refresh grid as in page load

            }
            else
            {
                    throw new Exception("Record already exists");            
            }
            gridView.CancelEdit();
            e.Cancel = true;
            FillSettingsGrid();
        }

        private void UpdateSettingsData(string Mode, DataRow SettingsRow, string strsname)
        {
            Object ReturnValue = VSWebBL.SettingBL.SettingsBL.Ins.UpdateSettings(CollectDataForSettings(Mode, SettingsRow), strsname);

            //if (Mode == "Insert")
            //{
            //    Object ReturnValue = VSWebBL.SettingBL.SettingsBL.Ins.InsertData(CollectDataForSettings(Mode, SettingsRow));
            //}
            //if (Mode == "Update")
            //{
            //    Object ReturnValue = VSWebBL.SettingBL.SettingsBL.Ins.UpdateData(CollectDataForSettings(Mode, SettingsRow));

            //}
        }

        private Settings CollectDataForSettings(string Mode, DataRow SettingsRow)
        {
            try
            {
                Settings SettingsObject = new Settings();
                //if (Mode == "Update")
                //{
                    //SettingsObject.ID = int.Parse(SettingsRow["ID"].ToString());
                //}
                SettingsObject.sname = SettingsRow["sname"].ToString();
                SettingsObject.svalue = SettingsRow["svalue"].ToString();
                SettingsObject.stype = SettingsRow["stype"].ToString();
             
               
                return SettingsObject;
            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
            finally { }
        }
        protected void SettingsGridView_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            if (Session["Settings"] != null && Session["Settings"] != "")
                SettingsDataTable = (DataTable)Session["Settings"];
            ASPxGridView gridView = (ASPxGridView)sender;

            DataRow DRRow = SettingsDataTable.NewRow();
            DRRow = SettingsDataTable.Rows.Find(e.Keys[0].ToString());

            DataRow dr = GetRow(SettingsDataTable, e.NewValues.GetEnumerator(), e.Keys[0].ToString());
            string retsval="";
            if (dr["sname"].ToString() != DRRow["sname"].ToString())
            {
                retsval = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue(dr["sname"].ToString());
            }          
            if (retsval == "")
            {
                //Update row in DB
                UpdateSettingsData("Update", GetRow(SettingsDataTable, e.NewValues.GetEnumerator(), e.Keys[0].ToString()), DRRow["sname"].ToString());
            }
            else
            {
                throw new Exception("Record already exists");
            }
            //Update Grid after inserting new row, refresh grid as in page load
            gridView.CancelEdit();
            e.Cancel = true;
            FillSettingsGrid();
        }

        protected void SettingsGridView_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            Settings StObject = new Settings();
            StObject.sname = e.Keys[0].ToString();

            //Delete row from DB


            Object ReturnValue = VSWebBL.SettingBL.SettingsBL.Ins.DeleteData(StObject);



            if (Convert.ToBoolean(ReturnValue) == false)
            {

                //ErrorMessageLabel.Text = "Server Cannot Be Deleted ";
                //ErrorMessagePopupControl.HeaderText = "Alert!!";
                //ErrorMessagePopupControl.ShowCloseButton = false;
                //ValidationUpdatedButton.Visible = true;
                //ValidationOkButton.Visible = true;
                //ErrorMessagePopupControl.ShowOnPageLoad = true;
                // ScriptManager.RegisterStartupScript(this, this.GetType(), "Not deleted", "alert('This Server Is Using Somewhere, Cannot Delete.');window.open('MaintainSettings.aspx');", true);
            }
            else
            {
                ASPxGridView gridView = (ASPxGridView)sender;
                gridView.CancelEdit();
                e.Cancel = true;
                FillSettingsGrid();
            }
            //Update Grid after inserting new row, refresh grid as in page load

        }
        protected void btn_Click(object sender, EventArgs e)
        {
            ImageButton btn = (ImageButton)sender;
            Settings SettingsObject = new Settings();
            SettingsObject.sname = btn.CommandArgument;
            sname = btn.CommandArgument;
            string name = btn.AlternateText;
            pnlAreaDtls.Style.Add("visibility", "visible");
            divmsg.InnerHtml = "Are you sure you want to delete the setting " + name + "?";
        }
        protected void btn_OkClick(object sender, EventArgs e)
        {
            Settings SettingsObject = new Settings();
            SettingsObject.sname = sname;
            Object ReturnValue =  VSWebBL.SettingBL.SettingsBL.Ins.DeleteData(SettingsObject);
            if (Convert.ToBoolean(ReturnValue) == false)
            {
                NavigatorPopupControl.ShowOnPageLoad = true;
                MsgLabel.Text = "This setting cannot be deleted, other dependencies exist.";
            }
            else
            {
                //ASPxGridView gridView = (ASPxGridView)sender;
                //gridView.CancelEdit();
                //e.Cancel = true;
                FillSettingsGrid();
            }
            //pnlAreaDtls.Style.Add("visibility", "hidden");
        }
        protected void btn_CancelClick(object sender, EventArgs e)
        {
            // ASPxGridView gridView = (ASPxGridView)sender;
            //gridView.CancelEdit();
            //e.Cancel = true;
            FillSettingsGrid();
            // pnlAreaDtls.Style.Add("visibility", "hidden");
        }

        protected void SettingsGridView_CustomErrorText(object sender, ASPxGridViewCustomErrorTextEventArgs e)
        {

        }

        protected void SettingsGridView_PageSizeChanged(object sender, EventArgs e)
        {
            //ProfilesGridView.PageIndex;
            VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("ManageSettings|SettingsGridView", SettingsGridView.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        }
    }
}