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
    public partial class CustomerInfo : System.Web.UI.Page
    {
        string CustomerName;
        int CustomerKey;
        string Mode;
        bool flag = false;
        string CustomerID;
        string Savedfile;

        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Title = "Customer Details";
            //for (int i = 0; i < ASPxPageControl1.TabPages.Count; i++)
            //{
            //    ASPxPageControl1.TabPages[i].ClientVisible = false;
            //}
            //if (Request.QueryString["TabIndex"] != null)
            //{
            //    int i = Convert.ToInt32(Request.QueryString["TabIndex"].ToString());
            //    ASPxPageControl1.ActiveTabIndex = i;
            //    ASPxPageControl1.TabPages[i].ClientVisible = true;
            //}

            successDiv.Style.Value = "display: none";
            errorDiv1.Style.Value = "display: none";
            try
            {
                if (Request.QueryString["ID"] != null && Request.QueryString["ID"] != "")
                {
                    Mode = "Update";
                    CustomerKey = int.Parse(Request.QueryString["ID"]);
                    

                    if (!IsPostBack)
                    {
                       
                        //if (Request.QueryString["Type"] != null && Request.QueryString["Type"] != "" && Request.QueryString["Type"] == "Contact")
                        //{
                        //    FillContactsData(CustomerKey);
                        //}
                        //else
                        //{



                            //FillCustomerData(CustomerKey);
                        //}
                        
                        FillContactsGrid();
                        FillNotesGrid();
                        FillTicketsGrid();
                        FillVersionInfoGrid();
                        FillCustomerData(CustomerKey); 
                        NameTextbox.Enabled = false;
                        CustomerTrackingRoundPanel.HeaderText = "Customer - " + NameTextbox.Text;
                        //FillContactsData(CustomerKey);
                        //FillNotesData(CustomerKey);
                        //FillTicketsData(CustomerKey);
                        //FillVersionInfoData(CustomerKey);
                        ASPxRoundPanel1.HeaderText = "Customer Details -  " + " " + NameTextbox.Text;
                       // ASPxPageControl1.TabIndex = Convert.ToInt16(Request.QueryString["TabIndex"].ToString());
                        ASPxPageControl1.ActiveTabIndex = Convert.ToInt32(Request.QueryString["TabIndex"].ToString());
                    }
                }
                else
                {
                    Mode = "Insert";
                    if (!IsPostBack)
                    {
                        NameTextbox.Text = "";
                        Status_TypeTextbox.Text = " ";
                        AddressTextBox.Text = "";
                        ServerCountTextbox.Text = "";
                        CompReplacementTextbox.Text = "";
                        OverallStatusTextbox.Text = "";
                        NextFollowUpDate.Value = "";
                        LicExpDate.Value = "";
                        //NextFollowUpDateTextbox.Text = "";
                        //LicExpDateTextbox.Text = "";
                        BudInfoTextbox.Text = "";

                             
                    }

                }
            }
            catch (Exception ex)
            { throw ex; }
            finally
            {
            }
        }       

        //private void FillContactsData(int ID)
        //{
        //    try
        //    {
        //        ContactsTask ContactsObject = new ContactsTask();
        //        ContactsTask ReturnContactsObject = new ContactsTask();
        //        ContactsObject.ID = ID;
        //        ReturnContactsObject = CustomerServiceBL.CustomerDetails.Ins.GetContactsDataForID(ContactsObject, Mode);
        //        if (ReturnContactsObject != null)
        //        {
        //            //ContactNameTextbox.Text = ReturnContactsObject.ContactName.ToString();
        //            //PhoneNumberTextbox.Text = ReturnContactsObject.PhoneNumber.ToString();
        //            //TitleTextbox.Text = ReturnContactsObject.Title.ToString();
        //            //DetailsTextbox.Text = ReturnContactsObject.Details.ToString();
        //        }

        //        //Session["CustomerGrid"] = "CustomerInfo.aspx?ID=" + ID + "tab=2";
        //        //ASPxPageControl1.TabIndex = 2;
        //    }
        //    catch (Exception ex)
        //    { throw ex; }
        //    finally
        //    {
        //    }
        //}

        //private void FillNotesData(int ID)
        //{
        //    try
        //    {
        //        NotesTask NotesObject = new NotesTask();
        //        NotesTask ReturnNotesObject = new NotesTask();
        //        NotesObject.ID = ID;
        //        ReturnNotesObject = CustomerServiceBL.CustomerDetails.Ins.GetNotesDataForID(NotesObject, Mode);
        //        if (ReturnNotesObject != null)
        //        {
        //            NCDateTextbox.Text = ReturnNotesObject.Date.ToString();
        //            NCDetailTextbox.Text = ReturnNotesObject.Details.ToString();
        //        }

        //        //Session["NotesGrid"] = "CustomerInfo.aspx?ID=" + ID + "tab=3";
        //        //ASPxPageControl1.TabIndex = 3;
        //    }
        //    catch (Exception ex)
        //    { throw ex; }
        //    finally
        //    {
        //    }
        //}

        //private void FillTicketsData(int ID)
        //{
        //    try
        //    {
        //        TicketsTask TicketsObject = new TicketsTask();
        //        TicketsTask ReturnTicketsObject = new TicketsTask();
        //        TicketsObject.ID = ID;
        //        ReturnTicketsObject = CustomerServiceBL.CustomerDetails.Ins.GetTicketsDataForID(TicketsObject, Mode);
        //        if (ReturnTicketsObject != null)
        //        {
        //            TDateTextbox.Text= ReturnTicketsObject.Date.ToString();
        //            TicketDetailsTextbox.Text = ReturnTicketsObject.Details.ToString();
        //            TicketNumberTextbox.Text = ReturnTicketsObject.TicketNumber.ToString();
        //            TicketStatusTextbox.Text = ReturnTicketsObject.Status.ToString();
        //        }

        //       // Session["TicketsGrid"] = "CustomerInfo.aspx?ID=" + ID + "tab=4";
        //       // ASPxPageControl1.TabIndex = 4;
        //    }
        //    catch (Exception ex)
        //    { throw ex; }
        //    finally
        //    {
        //    }
        //}

        
        
        private CustomerTasks CollectDataForCustomer()
        {
            try
            {
                CustomerTasks CustomerObject = new CustomerTasks();
                CustomerObject.Name = NameTextbox.Text;
                CustomerObject.Status_Type = Status_TypeTextbox.Text;
                CustomerObject.Address = AddressTextBox.Text;
                CustomerObject.ServerCount = ServerCountTextbox.Text;
                CustomerObject.CompReplacement = CompReplacementTextbox.Text;
                CustomerObject.OverallStatus = OverallStatusTextbox.Text;
                CustomerObject.NextFollowUpDate = NextFollowUpDate.Text;
                CustomerObject.LicExpDate = LicExpDate.Text;
                CustomerObject.BudInfo = BudInfoTextbox.Text;

                return CustomerObject;
            }
            catch (Exception ex)
            { throw ex; }
            finally
            {
            }
        }

        private CustomerTasks CollectDataForContacts()
        {
            try
            {
                CustomerTasks ContactsObject = new CustomerTasks();
                ContactsObject.Name = NameTextbox.Text;
                //ContactsObject.Status_Type = CCStatusTypeTextbox.Text;
                //ContactsObject.Address = CCAddressTextbox.Text;
                //ContactsObject.ServerCount = CCServerCountTextbox.Text;
                //ContactsObject.CompReplacement = CCCompRepTextbox.Text;
                //ContactsObject.OverallStatus = CCOverallStatus.Text;
                //ContactsObject.NextFollowUpDate = CCNFDTextbox.Text;
                //ContactsObject.LicExpDate = CCLicExpDateTextbox.Text;
                //ContactsObject.BudInfo = CCBudInfoTextbox.Text;
                return ContactsObject;
            }
            catch (Exception ex)
            { throw ex; }
            finally
            {
            }
        }
        

        //private void UpdateContactsData()
        //{
        //    CustomerTasks ContactsObject = new CustomerTasks();
        //    ContactsObject.Name = ContactNameTextbox.Text;
        //    ContactsObject.Status_Type = CCStatusTypeTextbox.Text;
        //    DataTable returntable = CustomerServiceBL.CustomerDetails.Ins.GetStatusType(ContactsObject, "Update");

        //    if (returntable.Rows.Count > 0)
        //    {
        //        ErrorMessageLabel.Text = "This Contact is already present.  Please enter another Contact name.";
        //        ErrorMessagePopupControl.ShowOnPageLoad = true;
        //        flag = true;
        //    }
        //    try
        //    {
        //        Object ReturnValue = CustomerServiceBL.CustomerDetails.Ins.UpdateCustomerData(CollectDataForContacts());
        //        SetFocusOnError(ReturnValue);
        //        if (ReturnValue.ToString() == "True")
        //        {
        //            Session["ContactsGrid"] = ContactNameTextbox.Text;
        //            Response.Redirect("ContactSearch.aspx");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    { }

        //}
        
        protected void formOKButton_Click(object Sender, EventArgs e)
        {
            try
            {
                if (Request.QueryString["ID"] != null && Request.QueryString["ID"] != "")
                {
                    UpdateCustomerData();
                }
                else
                {
                    InsertCustomer();
                } 
                Response.Redirect("Customer.aspx");

            }
            catch (Exception Ex)
            { throw Ex; }
            finally
            {
            }
        }
        private void SetFocusOnError(Object ReturnValue)
        {
            string ErrorMessage = ReturnValue.ToString();
            if (ErrorMessage.Substring(0, 2) == "ER")
            {
                ErrorMessageLabel.Text = ErrorMessage.Substring(3);
                ErrorMessagePopupControl.ShowOnPageLoad = true;
                flag = true;
            }
        }

        protected void FormCancelButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("Customer.aspx");
        }

        protected void ValidationUpdatedButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("Customer.aspx");
        }

       /* private void FillCustomerGrid()
        {
            try
            {
                DataTable CustTable = new DataTable();
                CustTable = CustomerServiceBL.CustomerDetails.Ins.GetAllData();
                if (CustTable.Rows.Count > 0)
                {
                    DataTable dtcopy = CustTable.Copy();
                    dtcopy.PrimaryKey = new DataColumn[] { dtcopy.Columns["ID"] };
                    //FillCustomerGrid.DataSource = CustTable();

                }
            }
            catch (Exception ex)
            {throw ex; }
            finally
            {
            }
        }*/

        /*Customer*/

        private void InsertCustomer()
        {
            CustomerTasks CTObject = new CustomerTasks();
            CTObject.Name = NameTextbox.Text;
            CTObject.Address = AddressTextBox.Text;
            CTObject.BudInfo = BudInfoTextbox.Text;
            CTObject.CompReplacement = CompReplacementTextbox.Text;
            CTObject.LicExpDate = LicExpDate.Text;
            CTObject.NextFollowUpDate = NextFollowUpDate.Text;
            CTObject.OverallStatus = OverallStatusTextbox.Text;
            CTObject.ServerCount = ServerCountTextbox.Text;
            CTObject.Status_Type = Status_TypeTextbox.Text;
            try
            {
                if (CTObject.Name != null && CTObject.Name != "")
                {
                    bool ReturnValue = CustomerServiceBL.CustomerDetails.Ins.UpdateCustomerData(CTObject);
                    //SetFocusOnError(ReturnValue);
                    //if (ReturnValue)
                    //{
                    //    ErrorMessageLabel.Text = "Customer Record Created Successfully.";
                    //    ErrorMessagePopupControl.HeaderText = "Information";
                    //    ErrorMessagePopupControl.ShowCloseButton = false;
                    //    ValidationUpdatedButton.Visible = true;
                    //    ValidationOkButton.Visible = false;
                    //    ErrorMessagePopupControl.ShowOnPageLoad = true;
                    //}
                    //else
                    //{
                    //    flag = true;
                    //}
                    if (ReturnValue)
                    {
                        successDiv.Style.Value = "display: block";
                        errorDiv1.Style.Value = "display: none";
                        successDiv.InnerText = "New Customer has been successfully created";
                    }
                    else
                    {
                        successDiv.Style.Value = "display: none";
                        errorDiv1.Style.Value = "display: block";
                        errorDiv1.InnerText = "Customer has NOT been created";
                    }
                }
                else
                {
                }
            }
            catch (Exception ex)
            { throw ex; }
            finally
            {
            }
            /*}*/
        }

        private void UpdateCustomerData()
        {

            try
            {
                bool ReturnValue = CustomerServiceBL.CustomerDetails.Ins.UpdateCustomerData(CollectDataForCustomer());
                //SetFocusOnError(ReturnValue);
                if (ReturnValue)
                {
                    successDiv.Style.Value = "display: block";
                    errorDiv1.Style.Value = "display: none";
                    successDiv.InnerText = "Customer details has been successfully updated";
                }
                else
                {
                    successDiv.Style.Value = "display: none";
                    errorDiv1.Style.Value = "display: block";
                    errorDiv1.InnerText = "Customer details has NOT been updated";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            { }

        }

        private void FillCustomerData(int ID)
        {
            try
            {
                CustomerTasks CustomerObject = new CustomerTasks();
                CustomerTasks ReturnCObject = new CustomerTasks();
                CustomerObject.ID = ID;
                ReturnCObject = CustomerServiceBL.CustomerDetails.Ins.GetCustomerDataForID(CustomerObject, Mode);
                if (ReturnCObject != null)
                {
                    NameTextbox.Text = ReturnCObject.Name.ToString();
                    Status_TypeTextbox.Text = ReturnCObject.Status_Type.ToString();
                    AddressTextBox.Text = ReturnCObject.Address.ToString();
                    ServerCountTextbox.Text = ReturnCObject.ServerCount.ToString();
                    CompReplacementTextbox.Text = ReturnCObject.CompReplacement.ToString();
                    OverallStatusTextbox.Text = ReturnCObject.OverallStatus.ToString();
                    NextFollowUpDate.Date = DateTime.Parse(ReturnCObject.NextFollowUpDate.ToString());
                    LicExpDate.Date = DateTime.Parse(ReturnCObject.NextFollowUpDate.ToString());
                    BudInfoTextbox.Text = ReturnCObject.BudInfo.ToString();
                }

                //Session["CustomerGrid"] = "CustomerInfo.aspx?ID=" + ID + "tab=1";

            }
            catch (Exception ex)
            { throw ex; }
            finally
            {
            }
        }

        /*Contacts */

        public void FillContactsGrid()
        {
            string Name = "";
            string PhoneNumber = "";
            string Title = "";
            //if (NSTextbox.Text != null && NSTextbox.Text != "")
            //{
            DataTable dt = CustomerServiceBL.CustomerDetails.Ins.GetCustomerContactDetailsBySearch(CustomerKey.ToString(), Name, PhoneNumber, Title);
            if (dt.Rows.Count > 0)
            {
                Session["ContactsGrid"] = dt;
                ContactSearchGridView.DataSource = dt;
                ContactSearchGridView.DataBind();
            }
            //}

        }

        protected void ContactsGridView_HtmlRowCreated(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType == GridViewRowType.EditForm)
            {
                if (e.GetValue("ContactID") != null && e.GetValue("ContactID") != "")
                {
                    ASPxWebControl.RedirectOnCallback("ContactEdit.aspx?ID=" + e.GetValue("ID") + "&ContactID=" + e.GetValue("ContactID") + "&PrevPage=CustomerInfo.aspx");
                }
                else
                {
                    ASPxWebControl.RedirectOnCallback("ContactEdit.aspx?ID=" + CustomerKey.ToString() +"&PrevPage=CustomerInfo.aspx");
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

        /* Notes */

        public void FillNotesGrid()
        {
            DataTable dt = CustomerServiceBL.CustomerDetails.Ins.GetCustomerNotesDetailsBySearch(CustomerKey.ToString());
            if (dt.Rows.Count > 0)
            {
                Session["NotesGrid"] = dt;
                NotesSearchGridView.DataSource = dt;
                NotesSearchGridView.DataBind();
            }            
        }

        protected void NotesGridView_HtmlRowCreated(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType == GridViewRowType.EditForm)
            {
                if (e.GetValue("NotesID") != null && e.GetValue("NotesID") != "")
                {
                    ASPxWebControl.RedirectOnCallback("NotesEdit.aspx?ID=" + e.GetValue("ID") + "&NotesID=" + e.GetValue("NotesID") + "&PrevPage=CustomerInfo.aspx");
                }
                else
                {
                    ASPxWebControl.RedirectOnCallback("NotesEdit.aspx?ID=" + CustomerKey.ToString() + "&PrevPage=CustomerInfo.aspx");
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


        /* Tickets */

        public void FillTicketsGrid()
        {

            DataTable dt = CustomerServiceBL.CustomerDetails.Ins.GetCustomerTicketsDetailsBySearch(CustomerKey.ToString(), "", "");

            if (dt.Rows.Count > 0)
            {
                Session["TicketsGrid"] = dt;
                TicketSearchGridView.DataSource = dt;
                TicketSearchGridView.DataBind();
            }
            //}

        }

        protected void TicketsGridView_HtmlRowCreated(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType == GridViewRowType.EditForm)
            {
                if (e.GetValue("TicketID") != null && e.GetValue("TicketID") != "")
                {
                    ASPxWebControl.RedirectOnCallback("TicketsEdit.aspx?ID=" + e.GetValue("ID") + "&TicketID=" + e.GetValue("TicketID") + "&PrevPage=CustomerInfo.aspx");
                }
                else
                {
                    ASPxWebControl.RedirectOnCallback("TicketsEdit.aspx?ID=" + CustomerKey.ToString() + "&PrevPage=CustomerInfo.aspx");
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

        /* Version */

        public void FillVersionInfoGrid()
        {

            DataTable dt = CustomerServiceBL.CustomerDetails.Ins.GetCustomerVersionInfoDetailsBySearch(CustomerKey.ToString(), "");
            if (dt.Rows.Count > 0)
            {
                Session["VersionInfoGrid"] = dt;
                VersionInfoSearchGridView.DataSource = dt;
                VersionInfoSearchGridView.DataBind();
            }
            //}

        }

        protected void VersionInfoGridView_HtmlRowCreated(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType == GridViewRowType.EditForm)
            {
                if (e.GetValue("VersionInfoID") != null && e.GetValue("VersionInfoID") != "")
                {
                    ASPxWebControl.RedirectOnCallback("VersionInfoEdit.aspx?ID=" + e.GetValue("ID") + "&VersionInfoID=" + e.GetValue("VersionInfoID") + "&PrevPage=CustomerInfo.aspx");
                }
                else
                {
                    ASPxWebControl.RedirectOnCallback("VersionInfoEdit.aspx?ID=" + CustomerKey.ToString() + "&PrevPage=CustomerInfo.aspx");
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

    }
}