using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using VSFramework;
using CustomerServiceBL;
using CustomerTrackingDO;

namespace CustomerTracking
{
    public partial class ContactEdit : System.Web.UI.Page
    {
        //string Mode = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string prevPage = Request.UrlReferrer.ToString();
                string CustID = "";
                
                successDiv.Style.Value = "display: none";
                errorDiv1.Style.Value = "display: none";
                FillCustomerComboBox();                
                if (Request.QueryString["ID"] != null && Request.QueryString["ID"] != "")
                {
                    CustID = Request.QueryString["ID"].ToString();
                    prevPage += "?ID =" + CustID;
                    if (Request.QueryString["ContactID"] != null && Request.QueryString["ContactID"] != "")
                    {
                       // Mode = "Update";                        
                        FillContactDetails(Convert.ToInt32(Request.QueryString["ContactID"]));
                        CustomerComboBox.Enabled = false;
                        CustomerComboBox.SelectedIndex = Convert.ToInt32(Request.QueryString["ID"]) -1;
                    }
                    else
                    {
                        //Mode = "Insert";                        
                        CustomerComboBox.Enabled = false;
                        CustomerComboBox.SelectedIndex = Convert.ToInt32(Request.QueryString["ID"]) - 1;
                    }
                }
                else
                {
                    //Mode = Mode;
                }
            }
        }
        protected void formOKButton_Click(object Sender, EventArgs e)
        {
            try
            {
                if (Request.QueryString["ContactID"] != null && Request.QueryString["ContactID"] != "")
                {
                    UpdateContact();
                }
                else 
                {
                   InsertContact();

                }
                string prevPage = Request.QueryString["PrevPage"] +"?TabIndex=1";
                if (Request.QueryString["ID"] != null && Request.QueryString["ID"] != "")
                {
                    prevPage += "&ID=" + Request.QueryString["ID"].ToString();
                }
                Response.Redirect(prevPage);
            }
            catch (Exception Ex)
            { throw Ex; }
            finally
            {
            }
        }
        protected void FormCancelButton_Click(object sender, EventArgs e)
        {
            string prevPage = Request.QueryString["PrevPage"] + "?TabIndex=1";
            if (Request.QueryString["ID"] != null && Request.QueryString["ID"] != "")
            {
                prevPage += "&ID=" + Request.QueryString["ID"].ToString();
            }
            Response.Redirect(prevPage);
        }
        public void FillCustomerComboBox()
        {
            //Session["CustomerNames"];
            DataTable dt = CustomerServiceBL.CustomerDetails.Ins.GetAllCustomerData();

            CustomerComboBox.DataSource = dt;
            CustomerComboBox.TextField = "Name";
            CustomerComboBox.ValueField = "ID";
            CustomerComboBox.DataBind();
        }
       
        private void FillContactDetails(int ID)
        {
            try
            {
                ContactsTask ContactsObject = new ContactsTask();
                ContactsTask ReturnContactsObject = new ContactsTask();
                ContactsObject.ID = ID;
                ReturnContactsObject = CustomerServiceBL.CustomerDetails.Ins.GetContactsDataForID(ContactsObject, "");
                if (ReturnContactsObject != null)
                {
                    ContactNameTextbox.Text = ReturnContactsObject.ContactName.ToString();
                    PhoneNumberTextbox.Text = ReturnContactsObject.PhoneNumber.ToString();
                    TitleTextbox.Text = ReturnContactsObject.Title.ToString();
                    DetailsTextbox.Text = ReturnContactsObject.Details.ToString();
                }
            }
            catch (Exception ex)
            { throw ex; }
            finally
            {
            }
        }

        private void UpdateContact()
        {
            try
            {
                bool update = false;
                ContactsTask ContactsObject = new ContactsTask();
                ContactsTask ReturnContactsObject = new ContactsTask();

                ContactsObject.ID = Convert.ToInt32(Request.QueryString["ContactID"].ToString()) ;
                ContactsObject.CustID = Convert.ToInt32(CustomerComboBox.SelectedItem.Value);
                ContactsObject.ContactName = ContactNameTextbox.Text;
                ContactsObject.PhoneNumber = PhoneNumberTextbox.Text;
                ContactsObject.Title = TitleTextbox.Text;
                ContactsObject.Details = DetailsTextbox.Text;

                update = CustomerServiceBL.CustomerDetails.Ins.UpdateContactsData(ContactsObject, "Update", "");
                if (update)
                {
                    successDiv.Style.Value = "display: block";
                    errorDiv1.Style.Value = "display: none";
                    successDiv.InnerText = "Contact has been successfully updated";
                }
                else
                {
                    successDiv.Style.Value = "display: none";
                    errorDiv1.Style.Value = "display: block";
                    errorDiv1.InnerText = "Contact has NOT been updated";
                }
            }
            catch (Exception ex)
            { throw ex; }
            finally
            {
            }
        }

        private void InsertContact()
        {
            try
            {
                bool update = false;
                ContactsTask ContactsObject = new ContactsTask();
                ContactsTask ReturnContactsObject = new ContactsTask();

                
                ContactsObject.ContactName = ContactNameTextbox.Text;
                ContactsObject.PhoneNumber = PhoneNumberTextbox.Text;
                ContactsObject.Title = TitleTextbox.Text;
                ContactsObject.Details = DetailsTextbox.Text;
               
                update = CustomerServiceBL.CustomerDetails.Ins.UpdateContactsData(ContactsObject, "Insert", CustomerComboBox.SelectedItem.Value.ToString());
                if (update)
                {
                    successDiv.Style.Value = "display: block";
                    errorDiv1.Style.Value = "display: none";
                    successDiv.InnerText = "New Contact has been successfully created";
                }
                else
                {
                    successDiv.Style.Value = "display: none";
                    errorDiv1.Style.Value = "display: block";
                    errorDiv1.InnerText = "Contact has NOT been created";
                }
            }
            catch (Exception ex)
            { throw ex; }
            finally
            {
            }
        }
    }
}