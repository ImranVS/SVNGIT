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
    public partial class TicketsSearch : System.Web.UI.Page
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
                        if (dr[1].ToString() == "TicketsGrid|TicketsGridView")
                        {
                            TicketSearchGridView.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                        }
                    }
                }
            }
            else
            {
                FillTicketsGridfromSession();

            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            //TicketsSearchGridView.DataBind();
            FillTicketsGrid();
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

        public void FillTicketsGrid()
        {

            string TicketNumber = TicketNumberTextbox.Text;
            string Status = StatusTextbox.Text;
            string id = "";
            if (CustomerComboBox.SelectedItem != null && CustomerComboBox.SelectedItem.Value.ToString() != "0")
            {
                id = CustomerComboBox.SelectedItem.Value.ToString();
            }
            DataTable dt = CustomerServiceBL.CustomerDetails.Ins.GetCustomerTicketsDetailsBySearch(id, TicketNumber, Status);

            Session["TicketsGrid"] = dt;
            TicketSearchGridView.DataSource = dt;
            TicketSearchGridView.DataBind();
        }
        private void FillTicketsGridfromSession()
        {
            try
            {
                DataTable TicketDetailsDatatable = new DataTable();
                if (Session["TicketsGrid"] != "" && Session["TicketsGrid"] != null)
                    TicketDetailsDatatable = (DataTable)Session["TicketsGrid"];
                if (TicketDetailsDatatable.Rows.Count > 0)
                {
                    TicketSearchGridView.DataSource = TicketDetailsDatatable;
                    TicketSearchGridView.DataBind();
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
        protected void TicketsGridView_HtmlRowCreated(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType == GridViewRowType.EditForm)
            {
                if (e.GetValue("TicketID") != null && e.GetValue("TicketID") != "")
                {
                    ASPxWebControl.RedirectOnCallback("TicketsEdit.aspx?ID=" + e.GetValue("ID") + "&TicketID=" + e.GetValue("TicketID") + "&PrevPage=TicketsSearch.aspx");
                }
                else
                {
                    ASPxWebControl.RedirectOnCallback("TicketsEdit.aspx?PrevPage=TicketsSearch.aspx");
                }
            }
        }

        protected void TicketsGridView_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            try
            {
                CustomerTasks TicketsObject = new CustomerTasks();
                TicketsObject.Name = e.Keys[0].ToString();
                Object ReturnValue = CustomerServiceBL.CustomerDetails.Ins.DeleteTicketsData(TicketsObject);

                ASPxGridView gridview = (ASPxGridView)sender;
                gridview.CancelEdit();
                e.Cancel = true;
                FillTicketsGrid();
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected void TicketsGridView_PageSizeChanged(object sender, EventArgs e)
        {
            CustomerServiceBL.UserPreferencesBL.Ins.UpdateUserPreferences("TicketsGrid|TicketsGridView", TicketSearchGridView.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
            Session["UserPreferences"] = CustomerServiceBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        }
    }
}