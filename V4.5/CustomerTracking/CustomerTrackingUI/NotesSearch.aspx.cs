using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;

using VSFramework;
using CustomerServiceBL;
using CustomerTrackingDO;

using DevExpress.Web.ASPxPopupControl;
using DevExpress.Web.ASPxEditors;
using DevExpress.Web.ASPxGridView;
using DevExpress.Web.ASPxClasses;
using DevExpress.Web.ASPxTabControl;
using DevExpress.Web.ASPxHiddenField;

namespace CustomerTracking
{
    public partial class NotesSearch : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FillCustomerComboBox();
                if (Session["UserPreferences"] != null)
                {
                    DataTable UserPreferences = (DataTable)Session["UserPreferences"];
                    foreach (DataRow dr in UserPreferences.Rows)
                    {
                        if (dr[1].ToString() == "ContactsGrid|ContactsGridView")
                        {
                            NotesSearchGridView.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                        }
                    }
                }
            }
            else
            {
                FillNotesGridfromSession();

            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            //NotesSearchGridView.DataBind();
            FillNotesGrid();
        }

        public void FillCustomerComboBox()
        {
            DataTable dt = CustomerServiceBL.CustomerDetails.Ins.GetAllCustomerData();
            DevExpress.Web.ASPxEditors.ListEditItem item = new DevExpress.Web.ASPxEditors.ListEditItem("Select All", 0);
            CustomerComboBox.DataSource = dt;
            CustomerComboBox.TextField = "Name";
            CustomerComboBox.ValueField = "ID";
            CustomerComboBox.DataBind();
            CustomerComboBox.Items.Insert(0, item);
            CustomerComboBox.SelectedIndex = 0;
            //FillNotesGrid();
        }

        public void FillNotesGrid()
        {
            string id = "";
            if (CustomerComboBox.SelectedItem != null && CustomerComboBox.SelectedItem.Value.ToString() != "0")
            {
                id = CustomerComboBox.SelectedItem.Value.ToString();
            }
            DataTable dt = CustomerServiceBL.CustomerDetails.Ins.GetCustomerNotesDetailsBySearch(id);

            Session["NotesGrid"] = dt;
            NotesSearchGridView.DataSource = dt;
            NotesSearchGridView.DataBind();


        }

        private void FillNotesGridfromSession()
        {
            try
            {
                DataTable NotesDetailsDatatable = new DataTable();
                if (Session["NotesGrid"] != "" && Session["NotesGrid"] != null)
                    NotesDetailsDatatable = (DataTable)Session["NotesGrid"];
                if (NotesDetailsDatatable.Rows.Count > 0)
                {
                    NotesSearchGridView.DataSource = NotesDetailsDatatable;
                    NotesSearchGridView.DataBind();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
        }
        protected void NotesGridView_HtmlRowCreated(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType == GridViewRowType.EditForm)
            {
                if (e.GetValue("NotesID") != null && e.GetValue("NotesID") != "")
                {
                    ASPxWebControl.RedirectOnCallback("NotesEdit.aspx?ID=" + e.GetValue("ID") + "&NotesID=" + e.GetValue("NotesID") + "&PrevPage=NotesSearch.aspx");
                }
                else
                {
                    ASPxWebControl.RedirectOnCallback("NotesEdit.aspx?PrevPage=NotesSearch.aspx");
                }
            }
        }

        protected void NotesGridView_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            try
            {
                CustomerTasks NotesObject = new CustomerTasks();
                NotesObject.Name = e.Keys[0].ToString();
                Object ReturnValue = CustomerServiceBL.CustomerDetails.Ins.DeleteNotesData(NotesObject);

                ASPxGridView gridview = (ASPxGridView)sender;
                gridview.CancelEdit();
                e.Cancel = true;
                FillNotesGrid();
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected void NotesGridView_PageSizeChanged(object sender, EventArgs e)
        {
            CustomerServiceBL.UserPreferencesBL.Ins.UpdateUserPreferences("NotesGrid|NotesGridView", NotesSearchGridView.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
            Session["UserPreferences"] = CustomerServiceBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        }
    }
}