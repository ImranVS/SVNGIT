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
    public partial class MaintainMenu : System.Web.UI.Page
    {
		protected void Page_PreInit(object sender, EventArgs e)
		{

			this.Master.contentCallEvent += new EventHandler(Master_ButtonClick);
		}

		private void Master_ButtonClick(object sender, EventArgs e)
		{

		}
        DataTable MenusDataTable = null;

        static int serverid = 0;
        protected void Page_Load(object sender, EventArgs e)
        {


            pnlAreaDtls.Style.Add("visibility", "hidden");
            if (!IsPostBack)
            {
                FillMenusGrid();
                if (Session["UserPreferences"] != null)
                {
                    DataTable UserPreferences = (DataTable)Session["UserPreferences"];
                    foreach (DataRow dr in UserPreferences.Rows)
                    {
                        if (dr[1].ToString() == "MaintainMenus|MenusGridView")
                        {
                            MenusGridView.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                        }
                    }
                }
            }
            else
            {

                FillMenusGridfromSession();

            }
        }

        private void FillMenusGrid()
        {
            try
            {

                MenusDataTable = new DataTable();
                DataSet MenusDataSet = new DataSet();
                MenusDataTable = VSWebBL.SecurityBL.MenusBL.Ins.GetAllData();
                if (MenusDataTable.Rows.Count > 0)
                {
                    //DataTable dtcopy = MenusDataTable.Copy();
                    //dtcopy.PrimaryKey = new DataColumn[] { dtcopy.Columns["ID"] };
                    MenusDataTable.PrimaryKey = new DataColumn[] { MenusDataTable.Columns["ID"] };
                }
                Session["Menus"] = MenusDataTable;
                MenusGridView.DataSource = MenusDataTable;
                MenusGridView.DataBind();


            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
            finally { }
        }

        private void FillMenusGridfromSession()
        {
            try
            {

                MenusDataTable = new DataTable();
                if (Session["Menus"] != null && Session["Menus"] != "")
                    MenusDataTable = (DataTable)Session["Menus"];//VSWebBL.ConfiguratorBL.DominoPropertiesBL.Ins.GetAllData();
                if (MenusDataTable.Rows.Count > 0)
                {
                    MenusGridView.DataSource = MenusDataTable;
                    MenusGridView.DataBind();
                }
            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
            finally { }
        }

        protected DataRow GetRow(DataTable ServerObject, IDictionaryEnumerator enumerator, int Keys)
        {
            DataTable dataTable = ServerObject;
            DataRow DRRow = dataTable.NewRow();
            if (Keys == 0)
                DRRow = dataTable.NewRow();
            else
                DRRow = dataTable.Rows.Find(Keys);
            //IDictionaryEnumerator enumerator = e.NewValues.GetEnumerator();
            enumerator.Reset();
            while (enumerator.MoveNext())
                DRRow[enumerator.Key.ToString()] = (enumerator.Value == null ? "False" : enumerator.Value);
            return DRRow;
        }

        protected void MenusGridView_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            if (Session["Menus"] != null && Session["Menus"] != "")
                MenusDataTable = (DataTable)Session["Menus"];
            ASPxGridView gridView = (ASPxGridView)sender;

            //Insert row in DB
            UpdateMenusData("Insert", GetRow(MenusDataTable, e.NewValues.GetEnumerator(), 0));

            //Update Grid after inserting new row, refresh grid as in page load
            gridView.CancelEdit();
            e.Cancel = true;
            FillMenusGrid();
        }

        private void UpdateMenusData(string Mode, DataRow MenusRow)
        {

            if (Mode == "Insert")
            {
                Object ReturnValue = VSWebBL.SecurityBL.MenusBL.Ins.InsertData(CollectDataForMenus(Mode, MenusRow));
            }
            if (Mode == "Update")
            {
                Object ReturnValue = VSWebBL.SecurityBL.MenusBL.Ins.UpdateData(CollectDataForMenus(Mode, MenusRow));
            }
        }

        private Menus CollectDataForMenus(string Mode, DataRow MenusRow)
        {
            try
            {
                Menus MenusObject = new Menus();
                if (Mode == "Update")
                {
                    MenusObject.ID = int.Parse(MenusRow["ID"].ToString());
                }
                MenusObject.DisplayText = MenusRow["DisplayText"].ToString();
                MenusObject.OrderNum = int.Parse(MenusRow["OrderNum"].ToString());
                MenusObject.ParentMenu = MenusRow["ParentMenu"].ToString();
                MenusObject.PageLink = MenusRow["PageLink"].ToString();
                MenusObject.Level = int.Parse(MenusRow["Level"].ToString());
                MenusObject.RefName = (MenusRow["RefName"].ToString() == "" ? MenusObject.DisplayText : MenusRow["RefName"].ToString()); ;
                MenusObject.ImageURL = MenusRow["ImageURL"].ToString();
                MenusObject.MenuArea = MenusRow["MenuArea"].ToString();


                return MenusObject;
            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
            finally { }
        }

        protected void MenusGridView_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            if (Session["Menus"] != null && Session["Menus"] != "")
                MenusDataTable = (DataTable)Session["Menus"];
            ASPxGridView gridView = (ASPxGridView)sender;

            UpdateMenusData("Update", GetRow(MenusDataTable, e.NewValues.GetEnumerator(), Convert.ToInt32(e.Keys[0])));
            gridView.CancelEdit();
            e.Cancel = true;
            FillMenusGrid();
        }

        protected void MenusGridView_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            Menus ServerObject = new Menus();
            ServerObject.ID = Convert.ToInt32(e.Keys[0]);

            Object ReturnValue = VSWebBL.SecurityBL.MenusBL.Ins.DeleteData(ServerObject);

            if (Convert.ToBoolean(ReturnValue) == false)
            {
            }
            else
            {
                ASPxGridView gridView = (ASPxGridView)sender;
                gridView.CancelEdit();
                e.Cancel = true;
                FillMenusGrid();
            }

        }

        protected void MenusGridView_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        {

            if (e.Column.FieldName == "Parentmenu")
            {
                ASPxComboBox ParentmenuComboBox = e.Editor as ASPxComboBox;
                FillParentmenuComboBox(ParentmenuComboBox);
                ParentmenuComboBox.Callback += new CallbackEventHandlerBase(ParentmenuComboBox_OnCallback);

            }
        }


        private void ParentmenuComboBox_OnCallback(object source, CallbackEventArgsBase e)
        {
            FillParentmenuComboBox(source as ASPxComboBox);
        }

        private void FillParentmenuComboBox(ASPxComboBox ParentmenuComboBox)
        {
            DataTable LocationDataTable = VSWebBL.SecurityBL.MenusBL.Ins.GetParentMenu();
            ParentmenuComboBox.DataSource = LocationDataTable;
            ParentmenuComboBox.TextField = "Parentmenu";
            ParentmenuComboBox.ValueField = "Parentmenu";
            ParentmenuComboBox.DataBind();
            ParentmenuComboBox.Items.Insert(0, new DevExpress.Web.ListEditItem("", ""));
        }

        protected void ValidationOkButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("MaintainMenus.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
            Context.ApplicationInstance.CompleteRequest();
        }

        protected void btn_Click(object sender, EventArgs e)
        {
            ImageButton btn = (ImageButton)sender;
            Menus ServerObject = new Menus();
            ServerObject.ID = Convert.ToInt32(btn.CommandArgument);
            serverid = Convert.ToInt32(btn.CommandArgument);
            string name = btn.AlternateText;
            pnlAreaDtls.Style.Add("visibility", "visible");
            divmsg.InnerHtml = "Are you sure you want to delete the server " + name + "?";

        }
        protected void btn_OkClick(object sender, EventArgs e)
        {
            Menus ServerObject = new Menus();
            ServerObject.ID = serverid;
            Object ReturnValue = VSWebBL.SecurityBL.MenusBL.Ins.DeleteData(ServerObject);
            if (Convert.ToBoolean(ReturnValue) == false)
            {
                NavigatorPopupControl.ShowOnPageLoad = true;
                MsgLabel.Text = "This server cannot be deleted, other dependencies exist.";
            }
            else
            {
                FillMenusGrid();
            }
        }
        protected void btn_CancelClick(object sender, EventArgs e)
        {
            FillMenusGrid();
        }
        protected void OKButton_Click(object sender, EventArgs e)
        {
            NavigatorPopupControl.ShowOnPageLoad = false;
        }

        protected void MenusGridView_AutoFilterCellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        {
            if (e.Column.FieldName == "Parentmenu")
            {
                ASPxComboBox ParentmenuComboBox = e.Editor as ASPxComboBox;
                FillParentmenuComboBox(ParentmenuComboBox);
                ParentmenuComboBox.Callback += new CallbackEventHandlerBase(ParentmenuComboBox_OnCallback);

            }
        }

        protected void MenusGridView_PageSizeChanged(object sender, EventArgs e)
        {
            VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("MaintainMenus|MenusGridView", MenusGridView.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        }
    }
}
