using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;

using VSFramework;
using VSWebBL;
using VSWebDO;

using DevExpress.Web;
using System.Net.NetworkInformation;
using System.Text;
using System.Net;
using System.Drawing;
using DevExpress.XtraReports.UI;
using VSWebUI;
using System.IO;

namespace VSWebUI.Configurator
{
    public partial class SNMPDeviceProperties : System.Web.UI.Page
    {
        int ServerKey;
        string Mode;
		string loginname;
        bool flag = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Title = "SNMP Device Properties";
            try
			{
				
				
                if (Request.QueryString["ID"] != null && Request.QueryString["ID"] != "")
                {
                    Mode = "Update";

                    ServerKey = int.Parse(Request.QueryString["ID"]);

                    //For Validation Summary
                    ////ApplyValidationSummarySettings();
                    ////ApplyEditorsSettings();
                    if (!IsPostBack)
					{
						filldropdown_CredentialsComboBox();
						FillData(ServerKey);
                        filLoccombo();
                        //fill  DeviceProperties
                       
                        //10/21/2014 NS modified for VSPLUS-934
                        //SNMPRoundPanel.HeaderText = "SNMP Device -  " + NameTextBox.Text;
                        lblServer.InnerHtml += " - " + NameTextBox.Text;
                    }
					


                }
                else
                {
                    Mode = "Insert";
                    if (!IsPostBack)
					{
						filldropdown_CredentialsComboBox();
                        filLoccombo();
						//RetryIntervalTextBox.Text = "2";
						//OffScanTextBox.Text = "30";
						//ScanIntervalTextBox.Text = "8";
						//ResponseThrTextBox.Text = "250";
						//CategoryTextBox.Text = "Router";
						//EnabledCheckBox.Checked = true;

                    }
                }

            }
            catch (Exception ex)
            {
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
            finally { }
        }
        public void filLoccombo()
        {
            DataTable Loc = new DataTable();
            Loc = VSWebBL.ConfiguratorBL.SNMPDevicesBL.Ins.GetLocation();
            LocComboBox.DataSource = Loc;
            LocComboBox.TextField = "Location";
            LocComboBox.ValueField = "Location";
            LocComboBox.DataBind();
        }

        private void FillData(int ID)
        {
            try
            {

                SNMPDevices SNMPDevicesObject = new SNMPDevices();
                SNMPDevices ReturnObject = new SNMPDevices();
                SNMPDevicesObject.ID = ID;
                ReturnObject = VSWebBL.ConfiguratorBL.SNMPDevicesBL.Ins.GetData(SNMPDevicesObject);
                NameTextBox.Text = ReturnObject.Name;
                IPTextBox.Text = ReturnObject.Address;
                DescriptionTextBox.Text = ReturnObject.Description;
                CategoryTextBox.Text = ReturnObject.Category;
                EnabledCheckBox.Checked = ReturnObject.Enabled;
				IncludeOnDashBoardCheckBox.Checked = ReturnObject.IncludeOnDashBoard;
				Img1.Src = ReturnObject.ImageURL;
			
					CredentialsComboBox.Visible = true;
			
				//CredentialsComboBox.Text = ReturnObject.imagename;
				CredentialsComboBox.Text = ReturnObject.imagename;
                LocComboBox.Text = ReturnObject.Location;
                // LocationTextBox.Text = ReturnObject.Location;
                ScanIntervalTextBox.Text = ReturnObject.ScanningInterval.ToString();
                OffScanTextBox.Text = ReturnObject.OffHoursScanInterval.ToString();
                RetryIntervalTextBox.Text = ReturnObject.RetryInterval.ToString();
                ResponseThrTextBox.Text = ReturnObject.ResponseThreshold.ToString();
                oidtextbox.Text = ReturnObject.OID.ToString();



            }
            catch (Exception ex)
            {
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
            finally { }
        }

        private SNMPDevices CollectDataForSNMPDevices()
        {
            try
            {
                //Cluster Settings
                SNMPDevices SNMPDevicesObject = new SNMPDevices();
                SNMPDevicesObject.Name = NameTextBox.Text;
                SNMPDevicesObject.Enabled = EnabledCheckBox.Checked;
				SNMPDevicesObject.IncludeOnDashBoard= IncludeOnDashBoardCheckBox.Checked; 
				SNMPDevicesObject.Category = CategoryTextBox.Text;
				SNMPDevicesObject.ImageURL = Img1.Src;
				
                //NetworkDevicesObject.First_Alert_Threshold = int.Parse(AlertTextBox.Text);
                SNMPDevicesObject.ScanningInterval = int.Parse(ScanIntervalTextBox.Text);
                SNMPDevicesObject.OffHoursScanInterval = int.Parse(OffScanTextBox.Text);
                //NetworkDevicesObject.Location = LocComboBox.Text; // LocationTextBox.Text;

                SNMPDevicesObject.RetryInterval = int.Parse(RetryIntervalTextBox.Text);
                SNMPDevicesObject.ResponseThreshold = int.Parse(ResponseThrTextBox.Text);
                SNMPDevicesObject.Address = IPTextBox.Text;
                SNMPDevicesObject.Description = DescriptionTextBox.Text;

                Locations LOCobject = new Locations();
                LOCobject.Location = LocComboBox.Text;
                Locations ReturnLocValue = VSWebBL.SecurityBL.LocationsBL.Ins.GetDataForLocation(LOCobject);
                SNMPDevicesObject.LocationID = ReturnLocValue.ID;
                SNMPDevicesObject.Location = LocComboBox.Text;
                SNMPDevicesObject.OID = oidtextbox.Text;

                if (Mode == "Update")
                    SNMPDevicesObject.ID = ServerKey;
                return SNMPDevicesObject;
            }
            catch (Exception ex)
            {
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
            finally { }
        }

        private void UpdateSNMPDevicesData()
        {

            try
            {


                SNMPDevices SNMPObj = new SNMPDevices();
                SNMPObj.Address = IPTextBox.Text;
                SNMPObj.Name = NameTextBox.Text;
                SNMPObj.ID = ServerKey;
                SNMPObj.OID = oidtextbox.Text;
                DataTable returntable = VSWebBL.ConfiguratorBL.SNMPDevicesBL.Ins.GetIPAddress(SNMPObj);

                if (returntable.Rows.Count > 0)
                {
                    //3/19/2014 NS modified
                    //ErrorMessageLabel.Text = "This Name or IP Address is already being monitored. Please enter another IP Address or Name.";
                    //ErrorMessagePopupControl.ShowOnPageLoad = true;
                    //10/6/2014 NS modified for VSPLUS-990
                    errorDiv.InnerHtml = "This Name or IP Address is already being monitored. Please enter another Name or IP Address." +
                        "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                    errorDiv.Style.Value = "display: block";
                    flag = true;
                    //IPAddressTextBox.Focus();
                }
                else
                {
                    try
                    {
                        Object ReturnValue = VSWebBL.ConfiguratorBL.SNMPDevicesBL.Ins.UpdateData(CollectDataForSNMPDevices());
                        SetFocusOnError(ReturnValue);
                        if (ReturnValue.ToString() == "True")
                        {
                            //3/19/2014 NS modified
                            /*
                            ErrorMessageLabel.Text = "Network device record updated successfully.";
                            ErrorMessagePopupControl.HeaderText = "Information";
                            ErrorMessagePopupControl.ShowCloseButton = false;
                            ValidationUpdatedButton.Visible = true;
                            ValidationOkButton.Visible = false;
                            ErrorMessagePopupControl.ShowOnPageLoad = true;
                            */
                            Session["SNMPDeviceUpdateStatus"] = NameTextBox.Text;
                            Response.Redirect("SNMPDevicesGrid.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
                            Context.ApplicationInstance.CompleteRequest();
                        }

                    }
                    catch (Exception ex)
                    {
                        //6/27/2014 NS added for VSPLUS-634
                        Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                        throw ex;
                    }
                    finally { }
                }
            }
            catch (Exception ex)
            {
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }

        }



        private void InsertSNMPDevices()
        {
            try
            {


                SNMPDevices SNMPObj = new SNMPDevices();
                SNMPObj.Address = IPTextBox.Text;
                SNMPObj.Name = NameTextBox.Text;
                SNMPObj.OID = oidtextbox.Text;
                //UrlObj.Name = NameTextBox.Text;
                DataTable returntable = VSWebBL.ConfiguratorBL.SNMPDevicesBL.Ins.GetIPAddress(SNMPObj);

                if (returntable.Rows.Count > 0)
                {
                    //3/19/2014 NS modified
                    //ErrorMessageLabel.Text = "This Name or IP Address is already being monitored. Please enter another IP Address or Name.";
                    //ErrorMessagePopupControl.ShowOnPageLoad = true;
                    //10/6/2014 NS modified for VSPLUS-990
                    errorDiv.InnerHtml = "This Name or IP Address is already being monitored. Please enter another IP Address or Name." +
                        "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                    errorDiv.Style.Value = "display: block";
                    flag = true;
                    //IPAddressTextBox.Focus();
                }
                else
                {

                    try
                    {
                        object ReturnValue = VSWebBL.ConfiguratorBL.SNMPDevicesBL.Ins.InsertData(CollectDataForSNMPDevices());
                        SetFocusOnError(ReturnValue);
                        if (ReturnValue.ToString() == "True")
                        {
                            //3/19/2014 NS modified
                            /*
                            ErrorMessageLabel.Text = "Network device record created successfully.";
                            ErrorMessagePopupControl.HeaderText = "Information";
                            ErrorMessagePopupControl.ShowCloseButton = false;
                            ValidationUpdatedButton.Visible = true;
                            ValidationOkButton.Visible = false;
                            ErrorMessagePopupControl.ShowOnPageLoad = true;
                            */
                            Session["SNMPDeviceUpdateStatus"] = NameTextBox.Text;
                            Response.Redirect("SNMPDevicesGrid.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
                            Context.ApplicationInstance.CompleteRequest();
                        }
                    }
                    catch (Exception ex)
                    {
                        //6/27/2014 NS added for VSPLUS-634
                        Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                        throw ex;
                    }
                    finally { }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }


        }


        protected void ValidationUpdatedButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("SNMPDevicesGrid.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
            Context.ApplicationInstance.CompleteRequest();
        }



        protected void FormOKButton_Click(object sender, EventArgs e)
        {

            // Write to Registy
            try
            {
                VSWebBL.SettingBL.SettingsBL.Ins.UpdateSvalue("SNMP Device Update", "True", VSWeb.Constants.Constants.SysString);
            }
            catch (Exception ex)
            {
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
            try
            {
                if (Mode == "Update")
                {

                    UpdateSNMPDevicesData();
                }
                if (Mode == "Insert")
                {
                    InsertSNMPDevices();
                    if (flag == false)
                        InsertStatus();

                }
            }
            catch (Exception ex)
            {
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
            finally { }
        }

        private Status CollectDataforStatus()
        {
            Status St = new Status();
            try
            {
                St.Category = CategoryTextBox.Text;
                //St.DeadMail = 0;
                St.Description = CategoryTextBox.Text;
                //St.Details = "";
                St.DownCount = 0;
                // St.Location = "Cluster";
                St.Name = NameTextBox.Text;
                //St.MailDetails = "Mail Details";
                // St.PendingMail = 0;
                St.sStatus = "Not Scanned";
                St.Type = "SNMP Device";
                St.Upcount = 0;
                St.UpPercent = 1;
                St.LastUpdate = System.DateTime.Now;
                St.ResponseTime = 0;
                St.TypeANDName = NameTextBox.Text + "-SNMP";
                St.Icon = 0;
                // St.NextScan = System.DateTime.Now;

                return St;
            }
            catch (Exception ex)
            {
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }


        }


        private void InsertStatus()
        {

            try
            {
                object ReturnValue = VSWebBL.StatusBL.StatusTBL.Ins.InsertData(CollectDataforStatus());
                // SetFocusOnError(ReturnValue);
                if (ReturnValue.ToString() == "True")
                {
                    //3/19/2014 NS modified
                    /*
                    ErrorMessageLabel.Text = "Data inserted successfully.";
                    ErrorMessagePopupControl.HeaderText = "Information";
                    ErrorMessagePopupControl.ShowCloseButton = false;
                    ValidationUpdatedButton.Visible = true;
                    ValidationOkButton.Visible = false;
                    ErrorMessagePopupControl.ShowOnPageLoad = true;
                    */
                    Session["SNMPDeviceUpdateStatus"] = NameTextBox.Text;
                    Response.Redirect("SNMPDevicesGrid.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
                    Context.ApplicationInstance.CompleteRequest();
                }

            }
            catch (Exception ex)
            {
                //3/19/2014 NS modified
                //ErrorMessageLabel.Text = "Error attempting to update the status table :" + ex.Message;
                //ErrorMessagePopupControl.ShowOnPageLoad = true;
                //10/6/2014 NS modified for VSPLUS-990
                errorDiv.InnerHtml = "Error attempting to update the status table: " + ex.Message +
                    "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                errorDiv.Style.Value = "display: block";
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
            }
            finally { }
        }
        private void SetFocusOnError(Object ReturnValue)
        {
            string ErrorMessage = ReturnValue.ToString();
            if (ErrorMessage.Substring(0, 2) == "ER")
            {
                //3/19/2014 NS modified
                //ErrorMessageLabel.Text = ErrorMessage.Substring(3);
                //ErrorMessagePopupControl.ShowOnPageLoad = true;
                //10/6/2014 NS modified for VSPLUS-990
                errorDiv.InnerHtml = ErrorMessage.Substring(3) +
                    "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                errorDiv.Style.Value = "display: block";
                flag = true;

            }
        }

        protected void FormCancelButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("SNMPDevicesGrid.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
            Context.ApplicationInstance.CompleteRequest();
        }

	
		protected void PingButton_Click(object sender, EventArgs e)
		{

			Ping pingSender = new Ping();


			string data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
			byte[] buffer = Encoding.ASCII.GetBytes(data);


			int timeout = 10000;


			PingOptions options = new PingOptions(64, true);


			// PingReply reply = pingSender.Send(IPTextBox.Text, timeout, buffer, options);
			PingReply reply = pingSender.Send(IPTextBox.Text, timeout, buffer, options);

			if (reply.Status == IPStatus.Success)
			{

				if (reply.RoundtripTime.ToString() != null)
				{
					ErrorMessagePopupControl.HeaderText = "Response Time";
					ErrorMessageLabel.Text = "Ping to " + reply.Address + " responded in " + reply.RoundtripTime + " ms.";
					ErrorMessagePopupControl.ShowOnPageLoad = true;
				}
			}
			else
			{
				ErrorMessageLabel.Text = IPTextBox.Text + " did not respond.";
				ErrorMessagePopupControl.ShowOnPageLoad = true;
			}

		}
        protected void NameTextBox_TextChanged(object sender, EventArgs e)
        {
            char Quote;
            Quote = (char)34;

            string myName = NameTextBox.Text;
            try
            {
                if (myName.IndexOf("'") > 0)
                    myName = myName.Replace("'", "");

                if (myName.IndexOf(Quote) > 0)
                    myName = myName.Replace(Quote, '~');

                if (myName.IndexOf(",") > 0)
                    myName = myName.Replace(",", " ");

                if (myName != NameTextBox.Text)
                    NameTextBox.Text = myName;

            }
            catch (Exception ex)
            {
                // MessageBox.Show(ex.Message);
                ErrorMessageLabel.Text = ex.Message;
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
            }

        }

        protected void DescriptionTextBox_TextChanged(object sender, EventArgs e)
        {
            char Quote;
            Quote = (char)34;

            string myName = DescriptionTextBox.Text;
            try
            {
                if (myName.IndexOf("'") > 0)
                    myName = myName.Replace("'", "");

                if (myName.IndexOf(Quote) > 0)
                    myName = myName.Replace(Quote, '~');

                if (myName.IndexOf(",") > 0)
                    myName = myName.Replace(",", " ");

                if (myName != DescriptionTextBox.Text)
                    NameTextBox.Text = myName;

            }
            catch (Exception ex)
            {
                // MessageBox.Show(ex.Message);
                ErrorMessageLabel.Text = ex.Message;
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
            }



            //protected void LocationTextBox_TextChanged(object sender, EventArgs e)
            //{
            //    char Quote;
            //    Quote = (char)34;

            //    string myName = LocationTextBox.Text;
            //    try
            //    {
            //        if (myName.IndexOf("'") > 0)
            //            myName = myName.Replace("'", "");

            //        if (myName.IndexOf(Quote) > 0)
            //            myName = myName.Replace(Quote, '~');

            //        if (myName.IndexOf(",") > 0)
            //            myName = myName.Replace(",", " ");

            //        if (myName != LocationTextBox.Text)
            //            NameTextBox.Text = myName;

            //    }
            //    catch (Exception ex)
            //    {
            //        // MessageBox.Show(ex.Message);
            //        ErrorMessageLabel.Text = ex.Message;
            //    }

        }
        protected void oidtextbox_TextChanged(object sender, EventArgs e)
        {
            char Quote;
            Quote = (char)34;

            string myName = oidtextbox.Text;
            try
            {
                if (myName.IndexOf("'") > 0)
                    myName = myName.Replace("'", "");

                if (myName.IndexOf(Quote) > 0)
                    myName = myName.Replace(Quote, '~');

                if (myName.IndexOf(",") > 0)
                    myName = myName.Replace(",", " ");

                if (myName != oidtextbox.Text)
                    NameTextBox.Text = myName;

            }
            catch (Exception ex)
            {
                // MessageBox.Show(ex.Message);
                ErrorMessageLabel.Text = ex.Message;
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
            }

        }
	
	
		protected void filldropdown_CredentialsComboBox()
		{
			DataTable dt = new DataTable();
			//string item = CredentialsComboBox.SelectedItem.Text;
			dt = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.filldropdown_CredentialsComboBox();
			CredentialsComboBox.DataSource = dt;
			CredentialsComboBox.DataBind();
		}
		protected void CredentialsComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			string name = CredentialsComboBox.SelectedItem.Text;
			Session["ImageName"] = name;
			DataTable dt = new DataTable();
			dt = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.getimagepathfornetwork(name);
			if (dt.Rows.Count > 0)
			{
				Img1.Src = dt.Rows[0]["Image"].ToString();
				Session["ImageURL"] = Img1.Src;
				//IPAddressTextBox.Text = dt.Rows[0]["Url"].ToString();
			}
		}

	

    }


}





    