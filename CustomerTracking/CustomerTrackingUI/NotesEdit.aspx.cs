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
    public partial class NotesEdit : System.Web.UI.Page
    {
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
                    if (Request.QueryString["NotesID"] != null && Request.QueryString["NotesID"] != "")
                    {
                        // Mode = "Update";                        
                        FillNotesDetails(Convert.ToInt32(Request.QueryString["NotesID"]));
                        CustomerComboBox.Enabled = false;
                        CustomerComboBox.SelectedIndex = Convert.ToInt32(Request.QueryString["ID"]) - 1;
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
                if (Request.QueryString["NotesID"] != null && Request.QueryString["NotesID"] != "")
                {
                    UpdateNotes();
                }
                else
                {
                    InsertNotes();

                }
                string prevPage = Request.QueryString["PrevPage"] + "?TabIndex=2";
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
            string prevPage = Request.QueryString["PrevPage"] + "?TabIndex=2";
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

        private void FillNotesDetails(int ID)
        {
            try
            {
                NotesTask NotesObject = new NotesTask();
                NotesTask ReturnNotes = new NotesTask();
                NotesObject.ID = ID;
                ReturnNotes = CustomerServiceBL.CustomerDetails.Ins.GetNotesDataForID(NotesObject, "");
                if (ReturnNotes != null)
                {
                    NCDateEdit.Date = DateTime.Parse(ReturnNotes.Date.ToString());
                    //NCDateTextbox.Text = ReturnNotes.Date.ToString();
                    NCDetailTextbox.Text = ReturnNotes.Details.ToString();                    
                }
            }
            catch (Exception ex)
            { throw ex; }
            finally
            {
            }
        }

        private void UpdateNotes()
        {
            try
            {
                bool update = false;
                NotesTask NotesObject = new NotesTask();

                NotesObject.ID = Convert.ToInt32(Request.QueryString["NotesID"].ToString());
                NotesObject.CustID = Convert.ToInt32(CustomerComboBox.SelectedItem.Value);
                //NotesObject.ID = Convert.ToInt32(CustomerComboBox.SelectedItem.Value);
                NotesObject.Date = NCDateEdit.Text;
                //NotesObject.Date = NCDateTextbox.Text;
                NotesObject.Details = NCDetailTextbox.Text;

                update = CustomerServiceBL.CustomerDetails.Ins.UpdateNotesData(NotesObject,"Update","");
                if (update)
                {
                    successDiv.Style.Value = "display: block";
                    errorDiv1.Style.Value = "display: none";
                    successDiv.InnerText = "Notes has been successfully updated";
                }
                else
                {
                    successDiv.Style.Value = "display: none";
                    errorDiv1.Style.Value = "display: block";
                    errorDiv1.InnerText = "Notes has NOT been updated";
                }
            }
            catch (Exception ex)
            { throw ex; }
            finally
            {
            }
        }

        private void InsertNotes()
        {
            try
            {
                bool update = false;
                NotesTask NotesObject = new NotesTask();
                //NotesObject.CustID = Convert.ToInt32(CustomerComboBox.SelectedItem.Value);
                //NotesObject.Date = NCDateTextbox.Text;
                NotesObject.Date = NCDateEdit.Text;
                NotesObject.Details = NCDetailTextbox.Text;

                update = CustomerServiceBL.CustomerDetails.Ins.UpdateNotesData(NotesObject, "Insert", CustomerComboBox.SelectedItem.Value.ToString());
                if (update)
                {
                    successDiv.Style.Value = "display: block";
                    errorDiv1.Style.Value = "display: none";
                    successDiv.InnerText = "New Notes has been successfully created";
                }
                else
                {
                    successDiv.Style.Value = "display: none";
                    errorDiv1.Style.Value = "display: block";
                    errorDiv1.InnerText = "Notes has NOT been created";
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