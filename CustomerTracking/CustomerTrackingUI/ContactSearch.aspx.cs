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
    public partial class ContactSearch : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //Session["CustomerNames"] = "";
                FillCustomerComboBox();
                
                if (Session["UserPreferences"] != null)
                {
                    DataTable UserPreferences = (DataTable)Session["UserPreferences"];
                    foreach (DataRow dr in UserPreferences.Rows)
                    {
                        if (dr[1].ToString() == "ContactsGrid|ContactsGridView")
                        {
                            ContactSearchGridView.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                        }
                    }
                }

            }
            else
            {
                FillContactsGridfromSession();
               
            }

        
        }

        public void FillCustomerComboBox()
        {
            //Session["CustomerNames"];
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
            FillContactsGrid();


        }


        public void FillContactsGrid()
        {
            string Name = NSTextbox.Text;
            string PhoneNumber = PSTextbox.Text;
            string Title = TSTextbox.Text;
            string id = "";
            if (CustomerComboBox.SelectedItem != null && CustomerComboBox.SelectedItem.Value.ToString() != "0")
            {
                id = CustomerComboBox.SelectedItem.Value.ToString();
            }

            DataTable dt = CustomerServiceBL.CustomerDetails.Ins.GetCustomerContactDetailsBySearch(id, Name, PhoneNumber, Title);

            Session["ContactsGrid"] = dt;
            ContactSearchGridView.DataSource = dt;
            ContactSearchGridView.DataBind();
        }

        private void FillContactsGridfromSession()
        {
            try
            {
                DataTable ContactDetailsDatatable = new DataTable();
                if (Session["ContactsGrid"] != "" && Session["ContactsGrid"] != null)
                    ContactDetailsDatatable = (DataTable)Session["ContactsGrid"];
                if (ContactDetailsDatatable.Rows.Count > 0)
                {
                    ContactSearchGridView.DataSource = ContactDetailsDatatable;
                    ContactSearchGridView.DataBind();
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

        protected void ContactsGridView_HtmlRowCreated(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType == GridViewRowType.EditForm)
            {
                if (e.GetValue("ContactID") != null && e.GetValue("ContactID") != "")
                {
                    ASPxWebControl.RedirectOnCallback("ContactEdit.aspx?ID=" + e.GetValue("ID") + "&ContactID=" + e.GetValue("ContactID") + "&PrevPage=ContactSearch.aspx");
                }
                else
                {
                    ASPxWebControl.RedirectOnCallback("ContactEdit.aspx?PrevPage=ContactSearch.aspx");
                }
            }
        }

        protected void ContactsGridView_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            try
            {
                CustomerTasks ContactsObject = new CustomerTasks();
                ContactsObject.Name = e.Keys[0].ToString();
                Object ReturnValue = CustomerServiceBL.CustomerDetails.Ins.DeleteContactsData(ContactsObject);

                ASPxGridView gridview = (ASPxGridView)sender;
                gridview.CancelEdit();
                e.Cancel = true;
                FillContactsGrid();
            }
            catch (Exception)
            {
                throw;
            }
        }

        //protected void ContactsGridView_PageSizeChanged(object sender, EventArgs e)
        //{
        //    CustomerServiceBL.UserPreferencesBL.Ins.UpdateUserPreferences("ContactsGrid|ContactsGridView", ContactSearchGridView.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
        //    Session["UserPreferences"] = CustomerServiceBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        //}
    }
}