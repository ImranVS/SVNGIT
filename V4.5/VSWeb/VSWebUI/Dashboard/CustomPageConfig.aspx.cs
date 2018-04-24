using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DevExpress.Web;
using System.Collections;

namespace VSWebUI.Dashboard
{
    public partial class CustomPageConfig : System.Web.UI.Page
    {
		protected void Page_PreInit(object sender, EventArgs e)
		{

			this.Master.contentCallEvent += new EventHandler(Master_ButtonClick);
		}

		private void Master_ButtonClick(object sender, EventArgs e)
		{

		}
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FillConfigureURLsGridView();
                if (Session["UserPreferences"] != null)
                {
                    DataTable UserPreferences = (DataTable)Session["UserPreferences"];
                    foreach (DataRow dr in UserPreferences.Rows)
                    {
                        if (dr[1].ToString() == "CustomPageConfig|ConfigureURLsGridView")
                        {
                            ConfigureURLsGridView.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                        }
                    }
                }
            }
            else
            {
                FillConfigureURLsGridViewFromSession();
            }
        }

        public void FillConfigureURLsGridView()
        {
            bool isadmin = false;
            DataTable sa = VSWebBL.SecurityBL.UsersBL.Ins.GetIsAdmin(Session["UserID"].ToString());
            if (sa.Rows.Count > 0)
            {
                if (sa.Rows[0]["SuperAdmin"].ToString() == "Y")
                {
                    isadmin = true;
                }
            }
            DataTable dt = VSWebBL.DashboardBL.DashboardBL.Ins.GetMyCustomPages(Session["UserID"].ToString(),true,isadmin);
            dt.PrimaryKey = new DataColumn[] { dt.Columns["ID"] };
            ConfigureURLsGridView.DataSource = dt;
            ConfigureURLsGridView.KeyFieldName = "ID";
            if (isadmin)
            {
                ConfigureURLsGridView.Columns["IsPrivate"].Visible = true;
                ConfigureURLsGridView.Columns["IsPrivate"].ShowInCustomizationForm = true;
            }
            ConfigureURLsGridView.DataBind();
            Session["CustomPages"] = dt;
        }

        public void FillConfigureURLsGridViewFromSession()
        {
            bool isadmin = false;
            DataTable sa = VSWebBL.SecurityBL.UsersBL.Ins.GetIsAdmin(Session["UserID"].ToString());
            if (sa.Rows.Count > 0)
            {
                if (sa.Rows[0]["SuperAdmin"].ToString() == "Y")
                {
                    isadmin = true;
                }
            }
            DataTable dt = new DataTable();
            if (Session["CustomPages"] != null && Session["CustomPages"] != "")
                dt = (DataTable)Session["CustomPages"];
            if (dt.Rows.Count > 0)
            {
                ConfigureURLsGridView.DataSource = dt;
                if (isadmin)
                {
                    ConfigureURLsGridView.Columns["IsPrivate"].Visible = true;
                    ConfigureURLsGridView.Columns["IsPrivate"].ShowInCustomizationForm = true;
                }
                ConfigureURLsGridView.DataBind();
            }
        }

        protected void ConfigureURLsGridView_RowCommand(object sender, DevExpress.Web.ASPxGridViewRowCommandEventArgs e)
        {
            int index = e.VisibleIndex;
            int ID = Convert.ToInt32(ConfigureURLsGridView.KeyFieldName);
        }

        protected void ConfigureURLsGridView_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            ASPxGridView gridView = (ASPxGridView)sender;
            DataTable dataTable = new DataTable();
            dataTable = (DataTable)Session["CustomPages"];
            DataRow DRRow = dataTable.NewRow();
            DRRow = GetRow(dataTable, e.NewValues.GetEnumerator(), 0);
            bool inserted = false;
            if (DRRow["IsPrivate"].ToString() == "")
            {
                DRRow["IsPrivate"] = "true";
            }
            inserted = VSWebBL.ConfiguratorBL.URLsBL.Ins.InsertCustomPageValue(Session["UserID"].ToString(), DRRow["URL"].ToString(), DRRow["Title"].ToString(), Convert.ToBoolean(DRRow["IsPrivate"].ToString()),"", true);
            gridView.CancelEdit();
            e.Cancel = true;
            FillConfigureURLsGridView();
        }

        protected void ConfigureURLsGridView_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            ASPxGridView gridView = (ASPxGridView)sender;
            DataTable dataTable = new DataTable();
            dataTable = (DataTable)Session["CustomPages"];
            DataRow DRRow = dataTable.NewRow();
            DRRow = GetRow(dataTable, e.NewValues.GetEnumerator(), Convert.ToInt32(e.Keys[0]));
            bool inserted = false;
            inserted = VSWebBL.ConfiguratorBL.URLsBL.Ins.InsertCustomPageValue(Session["UserID"].ToString(), DRRow["URL"].ToString(), DRRow["Title"].ToString(), Convert.ToBoolean(DRRow["IsPrivate"].ToString()), DRRow["ID"].ToString(), false);
            gridView.CancelEdit();
            e.Cancel = true;
            FillConfigureURLsGridView();
        }

        protected DataRow GetRow(DataTable UserObject, IDictionaryEnumerator enumerator, int Keys)
        {
            DataTable dataTable = UserObject;
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

        protected void ConfigureURLsGridView_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            string ID = "";
            ID = e.Keys[0].ToString();
            VSWebBL.DashboardBL.DashboardBL.Ins.DeleteMyCustomPage(ID);

            //Update Grid after inserting new row, refresh grid as in page load
            ASPxGridView gridView = (ASPxGridView)sender;
            gridView.CancelEdit();
            e.Cancel = true;
            FillConfigureURLsGridView();
        }

        protected void ConfigureURLsGridView_PageSizeChanged(object sender, EventArgs e)
        {
            //ProfilesGridView.PageIndex;
            VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("CustomPageConfig|ConfigureURLsGridView", ConfigureURLsGridView.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        }
    }
}