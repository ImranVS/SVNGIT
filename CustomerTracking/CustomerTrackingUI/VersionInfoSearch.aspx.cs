using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CustomerServiceBL;
using System.Data;
using CustomerTrackingDO;
using DevExpress.Web.ASPxGridView;
using DevExpress.Web.ASPxClasses;

namespace CustomerTracking
{
    public partial class VersionInfoSearch : System.Web.UI.Page
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
                        if (dr[1].ToString() == "VersionInfoGrid|VersionInfoGridView")
                        {
                            VersionInfoSearchGridView.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                        }
                    }
                }
            }
            else
            {
                FillVersionInfoGridfromSession();

            }

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
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            //ContactSearchGridView.DataBind();
            FillVersionInfoGrid();


        }


        public void FillVersionInfoGrid()
        {
            string VersionNumber = VersionNumberTextbox.Text;
            string id = "";
            if (CustomerComboBox.SelectedItem != null && CustomerComboBox.SelectedItem.Value.ToString() != "0")
            {
                id = CustomerComboBox.SelectedItem.Value.ToString();
            }
            DataTable dt = CustomerServiceBL.CustomerDetails.Ins.GetCustomerVersionInfoDetailsBySearch(id, VersionNumber);

            Session["VersionInfoGrid"] = dt;
            VersionInfoSearchGridView.DataSource = dt;
            VersionInfoSearchGridView.DataBind();

        }
        private void FillVersionInfoGridfromSession()
        {
            try
            {
                DataTable VersionInfoDetailsDatatable = new DataTable();
                if (Session["VersionInfoGrid"] != "" && Session["VersionInfoGrid"] != null)
                    VersionInfoDetailsDatatable = (DataTable)Session["VersionInfoGrid"];
                if (VersionInfoDetailsDatatable.Rows.Count > 0)
                {
                    VersionInfoSearchGridView.DataSource = VersionInfoDetailsDatatable;
                    VersionInfoSearchGridView.DataBind();
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

        protected void VersionInfoGridView_HtmlRowCreated(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType == GridViewRowType.EditForm)
            {
                if (e.GetValue("VersionInfoID") != null && e.GetValue("VersionInfoID") != "")
                {
                    ASPxWebControl.RedirectOnCallback("VersionInfoEdit.aspx?ID=" + e.GetValue("ID") + "&VersionInfoID=" + e.GetValue("VersionInfoID") + "&PrevPage=VersionInfoSearch.aspx");
                }
                else
                {
                    ASPxWebControl.RedirectOnCallback("VersionInfoEdit.aspx?PrevPage=VersionInfoSearch.aspx");
                }
            }
        }

        protected void VersionInfoGridView_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            try
            {
                CustomerTasks VersionInfoObject = new CustomerTasks();
                VersionInfoObject.Name = e.Keys[0].ToString();
                Object ReturnValue = CustomerServiceBL.CustomerDetails.Ins.DeleteVersionInfoData(VersionInfoObject);

                ASPxGridView gridview = (ASPxGridView)sender;
                gridview.CancelEdit();
                e.Cancel = true;
                FillVersionInfoGrid();
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected void VersionInfoGridView_PageSizeChanged(object sender, EventArgs e)
        {
            CustomerServiceBL.UserPreferencesBL.Ins.UpdateUserPreferences("VersionInfoGrid|VersionInfoSearchGridView", VersionInfoSearchGridView.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
            Session["UserPreferences"] = CustomerServiceBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        }
    }
}