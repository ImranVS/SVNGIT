using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

using CustomerServiceBL;
using DevExpress.Web.ASPxGridView;
using DevExpress.Web.ASPxClasses;
using CustomerTrackingDO;

namespace CustomerTracking
{
    public partial class Customer : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Title = "Customer Tracking Information";
            CustomerGridView.RowDeleting += new DevExpress.Web.Data.ASPxDataDeletingEventHandler(CustomerGridView_RowDeleting);
            if (!IsPostBack)
            {
                FillCustomerGrid();
                //FillContactsGrid();
                //FillNotesGrid();
                //FillTicketsGrid();
                //FillVersionInfoGrid();

                if (Session["UserPreferences"] != null)
                {
                    DataTable UserPreferences = (DataTable)Session["UserPreferences"];
                    foreach (DataRow dr in UserPreferences.Rows)
                    {
                        if (dr[1].ToString() == "CustomerGrid|CustomerGridView")
                        {
                            CustomerGridView.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                        }
                        
                        //if (dr[1].ToString() == "ContactsGrid|ContactsGridView")
                        //{
                        //    ContactsGridView.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                        //}


                        //if (dr[1].ToString() == "NotesGrid|NotesGridView")
                        //{
                        //    NotesGridView.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                        //}

                        //if (dr[1].ToString() == "TicketsrGrid|TicketsGridView")
                        //{
                        //    TicketsGridView.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                        //}

                        //if (dr[1].ToString() == "VersionInfoGrid|VersionInfoGridView")
                        //{
                        //    VersionInfoGridView.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                        //}
                    }
                }

            }
            else
            {
                FillCustomerGridfromSession();
                //FillContactsGridfromSession();
                //FillNotesGridfromSession();
                //FillTicketsGridfromSession();
                //FillVersionInfoGridfromSession();
            }

            //if (Session["CustomerUpdateStatus"] != null)
            //{
            //    if (Session["CustomerUpdateStatus"].ToString() != "")
            //    {
            //        successDiv.InnerHtml = "Customer Information for <b>" + Session["CustomerUpdateStatus"].ToString() +
            //            "<b> Updated Successfully.";
            //        successDiv.Style.Value = "display: block";
            //        Session["CustomerUpdateStatus"] = "";
            //    }
            //}
        }

        private void FillCustomerGrid()
        {
            try
            {
                DataTable CustomerDataTable = new DataTable();
                CustomerDataTable = CustomerServiceBL.CustomerDetails.Ins.GetAllCustomerData();
                if (CustomerDataTable.Rows.Count > 0)
                {
                    Session["CustomerGrid"] = CustomerDataTable;
                    CustomerGridView.DataSource = CustomerDataTable;
                    CustomerGridView.DataBind();
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

        private void FillCustomerGridfromSession()
        {
            try
            {
                DataTable CustomerDetailsDatatable = new DataTable();
                if (Session["CustomerGrid"] != "" && Session["CustomerGrid"] != null)
                    CustomerDetailsDatatable = (DataTable)Session["CustomerGrid"];
                if (CustomerDetailsDatatable.Rows.Count > 0)
                {
                    CustomerGridView.DataSource = CustomerDetailsDatatable;
                    CustomerGridView.DataBind();
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

        protected void CustomerGridView_HtmlRowCreated(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType == GridViewRowType.EditForm)
            {
                if (e.GetValue("ID") != null && e.GetValue("ID") != "")
                {
                    ASPxWebControl.RedirectOnCallback("CustomerInfo.aspx?ID=" + e.GetValue("ID") + "&TabIndex=0");
                }
                else
                {
                    ASPxWebControl.RedirectOnCallback("CustomerInfo.aspx");
                }
            }
        }

        protected void CustomerGridView_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            try
            {
                CustomerTasks CustomerObject = new CustomerTasks();
                CustomerObject.Name = e.Keys[0].ToString();
                Object ReturnValue = CustomerServiceBL.CustomerDetails.Ins.DeleteData(CustomerObject);
                
                ASPxGridView gridview = (ASPxGridView)sender;
                gridview.CancelEdit();
                e.Cancel = true;
                FillCustomerGrid();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //protected void FillContactsGrid()
        //{
        //    try
        //    {
        //        DataTable ContactsDataTable = new DataTable();

        //        ContactsDataTable = CustomerServiceBL.CustomerDetails.Ins.GetAllContactsData();
        //        if (ContactsDataTable.Rows.Count > 0)
        //        {
        //            Session["ContactsGrid"] = ContactsDataTable;
        //            ContactsGridView.DataSource = ContactsDataTable;
        //            ContactsGridView.DataBind();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //    }
        //}

        //private void FillContactsGridfromSession()
        ////{
        //    try
        //    {
        //        DataTable ContactsDT = new DataTable();
        //        if (Session["ContactsGrid"] != null && Session["ContactsGrid"] != "")
        //            ContactsDT = (DataTable)Session["ContactsGrid"];
        //        if (ContactsDT.Rows.Count > 0)
        //        {
        //            ContactsGridView.DataSource = ContactsDT;
        //            ContactsGridView.DataBind();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //    }
        //}
        //protected void FillNotesGrid()
        //{
        //    try
        //    {
        //        DataTable NotesDataTable = new DataTable();

        //        //NotesDataTable = CustomerServiceBL.CustomerDetails.Ins.GetAllNotesData();
        //        if (NotesDataTable.Rows.Count > 0)
        //        {
        //            Session["Notes"] = NotesDataTable;
        //            ContactsGridView.DataSource = NotesDataTable;
        //            ContactsGridView.DataBind();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //    }
        //}

        //private void FillNotesGridfromSession()
        //{
        //    try
        //    {
        //        DataTable NotesDT = new DataTable();
        //        if (Session["Notes"] != null && Session["Notes"] != "")
        //            NotesDT = (DataTable)Session["Notes"];
        //        if (NotesDT.Rows.Count > 0)
        //        {
        //            NotesGridView.DataSource = NotesDT;
        //            NotesGridView.DataBind();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //    }
        //}

        //protected void FillTicketsGrid()
        //{
        //    try
        //    {
        //        DataTable TicketsDataTable = new DataTable();

        //       // TicketsDataTable = CustomerServiceBL.CustomerDetails.Ins.GetAllTicketsData();
        //        if (TicketsDataTable.Rows.Count > 0)
        //        {
        //            Session["Tickets"] = TicketsDataTable;
        //            TicketsGridView.DataSource = TicketsDataTable;
        //            TicketsGridView.DataBind();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //    }
        //}

        //private void FillTicketsGridfromSession()
        //{
        //    try
        //    {
        //        DataTable TicketsDT = new DataTable();
        //        if (Session["Tickets"] != null && Session["Tickets"] != "")
        //            TicketsDT = (DataTable)Session["Tickets"];
        //        if (TicketsDT.Rows.Count > 0)
        //        {
        //            TicketsGridView.DataSource = TicketsDT;
        //            TicketsGridView.DataBind();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //    }
        //}

        //protected void FillVersionInfoGrid()
        //{
        //    try
        //    {
        //        DataTable VersionInfoDataTable = new DataTable();

        //     //   VersionInfoDataTable = CustomerServiceBL.CustomerDetails.Ins.GetAllVersionInfoData();
        //        if (VersionInfoDataTable.Rows.Count > 0)
        //        {
        //            Session["VersionInfo"] = VersionInfoDataTable;
        //            VersionInfoGridView.DataSource = VersionInfoDataTable;
        //            VersionInfoGridView.DataBind();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //    }
        //}

        //private void FillVersionInfoGridfromSession()
        //{
        //    try
        //    {
        //        DataTable VersionInfoDT = new DataTable();
        //        if (Session["VersionInfo"] != null && Session["VersionInfo"] != "")
        //            VersionInfoDT = (DataTable)Session["VersionInfo"];
        //        if (VersionInfoDT.Rows.Count > 0)
        //        {
        //            VersionInfoGridView.DataSource = VersionInfoDT;
        //            VersionInfoGridView.DataBind();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //    }
        //}

        protected void CustomerGridView_PageSizeChanged(object sender, EventArgs e)
        {
            CustomerServiceBL.UserPreferencesBL.Ins.UpdateUserPreferences("CustomerGrid|CustomerGridView", CustomerGridView.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
            Session["UserPreferences"] = CustomerServiceBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        }

        //protected void ContactsGridView_PageSizeChanged(object sender, EventArgs e)
        //{
        //    CustomerServiceBL.UserPreferencesBL.Ins.UpdateUserPreferences("ContactsGrid|ContactsGridView", ContactsGridView.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
        //    Session["UserPreferences"] = CustomerServiceBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        //}

        //protected void NotesGridView_PageSizeChanged(object sender, EventArgs e)
        //{
        //    CustomerServiceBL.UserPreferencesBL.Ins.UpdateUserPreferences("NotesGrid|NotesGridView", NotesGridView.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
        //    Session["UserPreferences"] = CustomerServiceBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        //}

        //protected void TicketsGridView_PageSizeChanged(object sender, EventArgs e)
        //{
        //    CustomerServiceBL.UserPreferencesBL.Ins.UpdateUserPreferences("TicketsGrid|TicketsGridView", TicketsGridView.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
        //    Session["UserPreferences"] = CustomerServiceBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        //}

        //protected void VersionInfoGridView_PageSizeChanged(object sender, EventArgs e)
        //{
        //    CustomerServiceBL.UserPreferencesBL.Ins.UpdateUserPreferences("VersionInfoGrid|VersionInfoGridView", VersionInfoGridView.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
        //    Session["UserPreferences"] = CustomerServiceBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        //}




        
        /*



                DataTable Customer = new DataTable();
                foreach (DataRow dr in Customer.Rows)
                {
                    if (dr[1].ToString() == "Customer|CustomerGridView")
                    {
                        CustomerGridView.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                    }
                }
            }
        }

        protected void CustomerGridView_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            CustomerTasks CTObject = new CustomerTasks();
            CTObject.ID = Convert.ToInt32(e.Keys[0]);
            Object ReturnValue = CustomerServiceBL.CustomerDetails.Ins.DeleteData(CTObject);
            ASPxGridView gridview = (ASPxGridView)sender;
            gridview.CancelEdit();
            e.Cancel = true;
            FillcustomerGrid();
            //throw new NotImplementedException();
        }
        protected void CustomerGridView_HtmlRowCreated(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType == GridViewRowType.EditForm)
            {
                if (e.GetValue("ID") != " " && e.GetValue("ID") != null)
                {
                    ASPxWebControl.RedirectOnCallback("CustomerInfo.aspx?ID=" + e.GetValue("ID"));
                }
                else
                {
                    ASPxWebControl.RedirectOnCallback("CustomerInfo.aspx");
                }
            }
        }
        protected void FillcustomerGrid()
        {
            try
            {
                DataTable CDTable = new DataTable();

                CDTable = CustomerServiceBL.CustomerDetails.Ins.GetAllData();
                if (CDTable.Rows.Count >= 0)
                {
                    CustomerGridView.DataSource = CDTable;
                    CustomerGridView.DataBind();

                }
            }
            catch (Exception ex)
            { throw ex; }
            finally { }
        }
        /*
            protected void CustomerGridView_RowDeleting(object sender, DevExpress.Web.ASPxGridView.ASPxDataDeletingEventArgs e)
            {
                CustomerTasks CTObject = new CustomerTasks();
                CTObject.ID = Convert.ToInt32(e.Keys[0]);
                Object ReturnValue = CustomerServiceBL.CustomerDetails.Ins.DeleteData(CTObject);
                ASPxGridView gridview = (ASPxGridView)sender;
                gridview.CancelEdit();
                e.Cancel = true;
                FillcustomerGrid();

                
        }*/
    }
}