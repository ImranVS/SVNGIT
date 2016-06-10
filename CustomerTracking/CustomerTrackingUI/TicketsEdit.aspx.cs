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
    public partial class TicketsEdit : System.Web.UI.Page
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
                    if (Request.QueryString["TicketID"] != null && Request.QueryString["TicketID"] != "")
                    {
                        // Mode = "Update";                        
                        FillTicketDetails(Convert.ToInt32(Request.QueryString["TicketID"]));
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
                if (Request.QueryString["TicketID"] != null && Request.QueryString["TicketID"] != "")
                {
                    UpdateTickets();
                }
                else
                {
                    InsertTickets();

                }
                string prevPage = Request.QueryString["PrevPage"] + "?TabIndex=4";
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
            string prevPage = Request.QueryString["PrevPage"] + "?TabIndex=4";
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

        private void FillTicketDetails(int ID)
        {
            try
            {
                TicketsTask TicketsObject = new TicketsTask();
                TicketsTask ReturnTickets = new TicketsTask();
                TicketsObject.ID = ID;
                ReturnTickets = CustomerServiceBL.CustomerDetails.Ins.GetTicketsDataForID(TicketsObject, "");
                if (ReturnTickets != null)
                {
                    TDateEdit.Date = DateTime.Parse(ReturnTickets.Date.ToString());
                    //TDateTextbox.Text = ReturnTickets.Date.ToString();
                    TicketStatusTextbox.Text = ReturnTickets.Status.ToString();
                    TicketNumberTextbox.Text = ReturnTickets.TicketNumber.ToString();
                    TicketDetailsTextbox.Text = ReturnTickets.Details.ToString();
                }
            }
            catch (Exception ex)
            { throw ex; }
            finally
            {
            }
        }

        private void UpdateTickets()
        {
            try
            {
                bool update = false;
                TicketsTask TicketsObject = new TicketsTask();
                TicketsObject.ID = Convert.ToInt32(Request.QueryString["TicketID"].ToString());
                TicketsObject.Date = TDateEdit.Text;
                //TicketsObject.Date = TDateTextbox.Text;
                TicketsObject.Details = TicketDetailsTextbox.Text;
                TicketsObject.Status = TicketStatusTextbox.Text;
                TicketsObject.TicketNumber = TicketNumberTextbox.Text;

                update = CustomerServiceBL.CustomerDetails.Ins.UpdateTicketsData(TicketsObject,"Update", "");
                if (update)
                {
                    successDiv.Style.Value = "display: block";
                    errorDiv1.Style.Value = "display: none";
                    successDiv.InnerText = "Tickets has been successfully updated";
                }
                else
                {
                    successDiv.Style.Value = "display: none";
                    errorDiv1.Style.Value = "display: block";
                    errorDiv1.InnerText = "Tickets has NOT been updated";
                }
            }
            catch (Exception ex)
            { throw ex; }
            finally
            {
            }
        }

        private void InsertTickets()
        {
            try
            {
                bool update = false;
                TicketsTask TicketsObject = new TicketsTask();

                TicketsObject.Date = TDateEdit.EditFormatString;
                //TicketsObject.Date = TDateTextbox.Text;
                TicketsObject.Details = TicketDetailsTextbox.Text;
                TicketsObject.Status = TicketStatusTextbox.Text;
                TicketsObject.TicketNumber = TicketNumberTextbox.Text;

                update = CustomerServiceBL.CustomerDetails.Ins.UpdateTicketsData(TicketsObject, "Insert", CustomerComboBox.SelectedItem.Value.ToString());
                if (update)
                {
                    successDiv.Style.Value = "display: block";
                    errorDiv1.Style.Value = "display: none";
                    successDiv.InnerText = "New Tickets has been successfully created";
                }
                else
                {
                    successDiv.Style.Value = "display: none";
                    errorDiv1.Style.Value = "display: block";
                    errorDiv1.InnerText = "Tickets has NOT been created";
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