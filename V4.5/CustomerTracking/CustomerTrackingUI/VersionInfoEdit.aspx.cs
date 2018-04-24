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
    public partial class VersionInfoEdit : System.Web.UI.Page
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
                    if (Request.QueryString["VersionInfoID"] != null && Request.QueryString["VersionInfoID"] != "")
                    {
                        // Mode = "Update";                        
                        FillVersionDetails(Convert.ToInt32(Request.QueryString["VersionInfoID"]));
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
                if (Request.QueryString["VersionInfoID"] != null && Request.QueryString["VersionInfoID"] != "")
                {
                    UpdateVersionInfo();
                }
                else
                {
                    InsertVersionInfo();

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

        private void FillVersionDetails(int ID)
        {
            try
            {
                VersionInfoTask VersionInfoObject = new VersionInfoTask();
                VersionInfoTask ReturnContactsObject = new VersionInfoTask();
                VersionInfoObject.ID = ID;
                ReturnContactsObject = CustomerServiceBL.CustomerDetails.Ins.GetVersionInfoDataForID(VersionInfoObject, "");
                if (ReturnContactsObject != null)
                {
                    VersionNumberTextbox.Text = ReturnContactsObject.VersionNumber.ToString();
                    InstallDateEdit.Date = DateTime.Parse(ReturnContactsObject.InstallDate.ToString());
                    //InstallDateTextbox.Text = ReturnContactsObject.InstallDate.ToString();
                    VersionDetailsTextbox.Text = ReturnContactsObject.Details.ToString();                    
                }
            }
            catch (Exception ex)
            { throw ex; }
            finally
            {
            }
        }

        private void UpdateVersionInfo()
        {
            try
            {
                bool update = false;
                VersionInfoTask VersionObject = new VersionInfoTask();

                VersionObject.ID = Convert.ToInt32(Request.QueryString["VersionInfoID"].ToString());
                VersionObject.CustID = Convert.ToInt32(CustomerComboBox.SelectedItem.Value);
                VersionObject.VersionNumber = VersionNumberTextbox.Text;
                //VersionObject.InstallDate = InstallDateTextbox.Text;
                VersionObject.InstallDate = InstallDateEdit.Text;
                VersionObject.Details = VersionDetailsTextbox.Text;

                update = CustomerServiceBL.CustomerDetails.Ins.UpdateVersionInfo(VersionObject, "Update", "");
                if (update)
                {
                    successDiv.Style.Value = "display: block";
                    errorDiv1.Style.Value = "display: none";
                    successDiv.InnerText = "Version has been successfully updated";
                }
                else
                {
                    successDiv.Style.Value = "display: none";
                    errorDiv1.Style.Value = "display: block";
                    errorDiv1.InnerText = "Version has NOT been updated";
                }
            }
            catch (Exception ex)
            { throw ex; }
            finally
            {
            }
        }

        private void InsertVersionInfo()
        {
            try
            {
                bool update = false;
                VersionInfoTask VersionObject = new VersionInfoTask();
                
                //VersionObject.ID = Convert.ToInt32(Request.QueryString["VersionInfoID"].ToString());
                VersionObject.CustID = Convert.ToInt32(CustomerComboBox.SelectedItem.Value);
                VersionObject.VersionNumber = VersionNumberTextbox.Text;
                //VersionObject.InstallDate = InstallDateTextbox.Text;
                VersionObject.InstallDate = InstallDateEdit.Text;
                VersionObject.Details = VersionDetailsTextbox.Text;

                update = CustomerServiceBL.CustomerDetails.Ins.UpdateVersionInfo(VersionObject, "Insert", CustomerComboBox.SelectedItem.Value.ToString());
                if (update)
                {
                    successDiv.Style.Value = "display: block";
                    errorDiv1.Style.Value = "display: none";
                    successDiv.InnerText = "New Version has been successfully created";
                }
                else
                {
                    successDiv.Style.Value = "display: none";
                    errorDiv1.Style.Value = "display: block";
                    errorDiv1.InnerText = "Version has NOT been created";
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