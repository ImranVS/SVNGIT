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
    public partial class NetworkDeviceProperties : System.Web.UI.Page
	{
		string path;
        int ServerKey;
        string Mode;
		string loginname;
        bool flag = false;
		VSFramework.TripleDES tripleDes = new VSFramework.TripleDES();
        protected void Page_Load(object sender, EventArgs e)
        {
			PasswordTextBox.Attributes["type"] = "password";
            Page.Title = "Network Device Properties";
            try
            {



				//filldropdown_CredentialsComboBox();

                if (Request.QueryString["ID"] != null && Request.QueryString["ID"] != "")
                {
                    Mode = "Update";
					ASPxMenu1.Visible = true;
                    ServerKey = int.Parse(Request.QueryString["ID"]);

                    //For Validation Summary
                    ////ApplyValidationSummarySettings();
                    ////ApplyEditorsSettings();
                    if (!IsPostBack)
                    {
						filldropdown_CredentialsComboBox();
						FillData(ServerKey);
                        filLoccombo();
                        //fill NetworkDeviceProperties
                      
                        //10/21/2014 NS modified for VSPLUS-934
                        //NDRoundPanel.HeaderText = "Network Device -  " + NameTextBox.Text;
                        lblServer.InnerHtml += " - " + NameTextBox.Text;
                    }
					

                }
                else
                {
                    Mode = "Insert";
					ASPxMenu1.Visible = false;
                    if (!IsPostBack)
                    {
						filldropdown_CredentialsComboBox();
						filLoccombo();
                        RetryIntervalTextBox.Text = "2";
                        OffScanTextBox.Text = "30";
                        ScanIntervalTextBox.Text = "8";
                        ResponseThrTextBox.Text = "250";
                        CategoryTextBox.Text = "Router";
                        EnabledCheckBox.Checked = true;

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
            Loc = VSWebBL.ConfiguratorBL.NetworkDevicesBL.Ins.GetLocation();
            LocComboBox.DataSource = Loc;
            LocComboBox.TextField = "Location";
            LocComboBox.ValueField = "Location";
            LocComboBox.DataBind();
        }

        private void FillData(int ID)
        {
            try
            {

				string MyObjPwd;
				string[] MyObjPwdArr;
				byte[] MyPass;
                NetworkDevices NetworkDevicesObject = new NetworkDevices();
                NetworkDevices ReturnObject = new NetworkDevices();
                NetworkDevicesObject.ID = ID;

                ReturnObject = VSWebBL.ConfiguratorBL.NetworkDevicesBL.Ins.GetData(NetworkDevicesObject);
                NameTextBox.Text = ReturnObject.Name;
                IPTextBox.Text = ReturnObject.Address;
                DescriptionTextBox.Text = ReturnObject.Description;
                CategoryTextBox.Text = ReturnObject.Category;
                EnabledCheckBox.Checked = ReturnObject.Enabled;
				IncludeOnDashBoardCheckBox.Checked = ReturnObject.IncludeOnDashBoard;
                LocComboBox.Text = ReturnObject.Location;
				Img1.Src = ReturnObject.ImageURL;
				//if (Img1.Src != null)
				//{
				//    checkbx.Checked = true;

				//}
				//if (checkbx.Checked)
				//{
				    CredentialsComboBox.Visible = true;
				//}
				CredentialsComboBox.Text = ReturnObject.imagename;
 				
			   //// LocationTextBox.Text = ReturnObject.Location;
                ScanIntervalTextBox.Text = ReturnObject.ScanningInterval.ToString();
                OffScanTextBox.Text = ReturnObject.OffHoursScanInterval.ToString();
                RetryIntervalTextBox.Text = ReturnObject.RetryInterval.ToString();
                ResponseThrTextBox.Text = ReturnObject.ResponseThreshold.ToString();
                NetworkTypeCombobox.Text = ReturnObject.NetworkType== null ? "" : ReturnObject.NetworkType.ToString();

                UserNameTextBox.Text = ReturnObject.Username == null ? "" : ReturnObject.Username;
                PasswordTextBox.Text = ReturnObject.Password == null ? "" : ReturnObject.Password;
				if (PasswordTextBox.Text != "" && PasswordTextBox.Text != null)
				{
					PasswordTextBox.Text = "      ";
				}
				else
				{
					PasswordTextBox.Text = "";
				}

                MyObjPwd = ReturnObject.Password == null ? "" : ReturnObject.Password;
				//if (MyObjPwd != "")
				//{
				//    MyObjPwdArr = MyObjPwd.Split(',');
				//    MyPass = new byte[MyObjPwdArr.Length];
				//    for (int j = 0; j < MyObjPwdArr.Length; j++)
				//    {
				//        MyPass[j] = Byte.Parse(MyObjPwdArr[j]);
				//    }
				//    ViewState["PWD"] = tripleDes.Decrypt(MyPass);
				//}


				if (MyObjPwd != "")
				{
					MyObjPwdArr = MyObjPwd.Split(',');
					MyPass = new byte[MyObjPwdArr.Length];

					try
					{
						for (int j = 0; j < MyObjPwdArr.Length; j++)
						{
							MyPass[j] = Byte.Parse(MyObjPwdArr[j]);
						}
						ViewState["PWD"] = tripleDes.Decrypt(MyPass);
					}
					catch (Exception ex)
					{
						if (ex.Message == "Input string was not in a correct format.")
						{
							ViewState["PWD"] = MyObjPwd;
						}
					}
				}


				

				//TripleDES tripleDES = new TripleDES();
				//byte[] encryptedPass=  ReturnObject.Password;
				//string pwd = tripleDES.Decrypt(encryptedPass);
				//PasswordTextBox.Text = pwd;
				//PasswordTextBox.Attributes.Add("value",ReturnObject.Password.ToString());
				
				}
            catch (Exception ex)
            {
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex; 
            }
            finally { }
        }

        private NetworkDevices CollectDataForNetworkDevices()
        {
            try
            {
                //Cluster Settings
                NetworkDevices NetworkDevicesObject = new NetworkDevices();
                NetworkDevicesObject.Name =NameTextBox.Text;
                NetworkDevicesObject.Enabled = EnabledCheckBox.Checked;
				NetworkDevicesObject.IncludeOnDashBoard = IncludeOnDashBoardCheckBox.Checked;
                NetworkDevicesObject.Category = CategoryTextBox.Text;
				NetworkDevicesObject.ImageURL = Img1.Src;
				//NetworkDevicesObject.First_Alert_Threshold = int.Parse(AlertTextBox.Text);

                NetworkDevicesObject.ScanningInterval= int.Parse(ScanIntervalTextBox.Text);
                NetworkDevicesObject.OffHoursScanInterval = int.Parse(OffScanTextBox.Text);
                //NetworkDevicesObject.Location = LocComboBox.Text; // LocationTextBox.Text;

                NetworkDevicesObject.RetryInterval=int.Parse(RetryIntervalTextBox.Text);
                NetworkDevicesObject.ResponseThreshold = int.Parse(ResponseThrTextBox.Text);
                NetworkDevicesObject.Address = IPTextBox.Text;
                NetworkDevicesObject.Description = DescriptionTextBox.Text;
				

                Locations LOCobject = new Locations();
                LOCobject.Location = LocComboBox.Text;
                Locations ReturnLocValue = VSWebBL.SecurityBL.LocationsBL.Ins.GetDataForLocation(LOCobject);
                NetworkDevicesObject.LocationID = ReturnLocValue.ID;
                NetworkDevicesObject.Location= LocComboBox.Text;
				NetworkDevicesObject.NetworkType = NetworkTypeCombobox.Text.ToString();
				if (PasswordTextBox.Text != "")
				{
					if (PasswordTextBox.Text == "      ")
						PasswordTextBox.Text = ViewState["PWD"].ToString();

					TripleDES tripleDES = new TripleDES();
					byte[] encryptedPass = tripleDES.Encrypt(PasswordTextBox.Text);
					string encryptedPassAsString = string.Join(", ", encryptedPass.Select(s => s.ToString()).ToArray());
					NetworkDevicesObject.Password = encryptedPassAsString;
				}
				
				NetworkDevicesObject.Username = UserNameTextBox.Text;

                if (Mode == "Update")
                    NetworkDevicesObject.ID = ServerKey;
                return NetworkDevicesObject;
            }
            catch (Exception ex)
            {
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex; 
            }
            finally { }
        }

        private void UpdateNetworkDevicesData()
        {

            try
            {


                NetworkDevices NDObj = new NetworkDevices();
                NDObj.Address = IPTextBox.Text;
                NDObj.Name = NameTextBox.Text;
                NDObj.ID = ServerKey;
                DataTable returntable = VSWebBL.ConfiguratorBL.NetworkDevicesBL.Ins.GetIPAddress(NDObj);

                if (returntable.Rows.Count > 0)
                {
                    //3/19/2014 NS modified
                    //ErrorMessageLabel.Text = "This Name or IP Address is already being monitored. Please enter another IP Address or Name.";
                    //ErrorMessagePopupControl.ShowOnPageLoad = true;
                    //10/6/2014 NS modified for VSPLUS-990
                    errorDiv.InnerHtml = "This Name or IP Address is already being monitored. Please enter another Name or IP Address."+
                        "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                    errorDiv.Style.Value = "display: block";
                    flag = true;
                    //IPAddressTextBox.Focus();
                }
                else
                {
                    try
                    {
                        Object ReturnValue = VSWebBL.ConfiguratorBL.NetworkDevicesBL.Ins.UpdateData(CollectDataForNetworkDevices());
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
                            Session["NetworkDeviceUpdateStatus"] = NameTextBox.Text;
                            Response.Redirect("NetworkDevicesGrid.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
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
           

       
        private void InsertNetworkDevices()
        {
            try
            {

           
            NetworkDevices NDObj = new NetworkDevices();
            NDObj.Address = IPTextBox.Text;
            NDObj.Name = NameTextBox.Text;    
            //UrlObj.Name = NameTextBox.Text;
            DataTable returntable = VSWebBL.ConfiguratorBL.NetworkDevicesBL.Ins.GetIPAddress(NDObj);

            if (returntable.Rows.Count > 0)
            {
                //3/19/2014 NS modified
                //ErrorMessageLabel.Text = "This Name or IP Address is already being monitored. Please enter another IP Address or Name.";
                //ErrorMessagePopupControl.ShowOnPageLoad = true;
                //10/6/2014 NS modified for VSPLUS-990
                errorDiv.InnerHtml = "This Name or IP Address is already being monitored. Please enter another IP Address or Name."+
                    "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                errorDiv.Style.Value = "display: block";
                flag = true;
                //IPAddressTextBox.Focus();
            }
            else
            {

                try
                {
                    object ReturnValue = VSWebBL.ConfiguratorBL.NetworkDevicesBL.Ins.InsertData(CollectDataForNetworkDevices());
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
                        Session["NetworkDeviceUpdateStatus"] = NameTextBox.Text;
                        Response.Redirect("NetworkDevicesGrid.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
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
            Response.Redirect("NetworkDevicesGrid.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
            Context.ApplicationInstance.CompleteRequest();
        }



        protected void FormOKButton_Click(object sender, EventArgs e)
        {

            // Write to Registy
            try
            {
                VSWebBL.SettingBL.SettingsBL.Ins.UpdateSvalue("Network Device Update", "True", VSWeb.Constants.Constants.SysString);
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

                    UpdateNetworkDevicesData();
                }
                if (Mode == "Insert")
                {
                    InsertNetworkDevices();
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
                St.Type = "Network Device";
                St.Upcount = 0;
                St.UpPercent = 1;
                St.LastUpdate = System.DateTime.Now;
                St.ResponseTime = 0;
                //5/5/2016 NS modified
                //Changed -ND to -Network Device
                St.TypeANDName = NameTextBox.Text + "-Network Device";
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
                    Session["NetworkDeviceUpdateStatus"] = NameTextBox.Text;
                    Response.Redirect("NetworkDevicesGrid.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
                    Context.ApplicationInstance.CompleteRequest();
                }

            }
            catch (Exception ex)
            {
                //3/19/2014 NS modified
                //ErrorMessageLabel.Text = "Error attempting to update the status table :" + ex.Message;
                //ErrorMessagePopupControl.ShowOnPageLoad = true;
                //10/6/2014 NS modified for VSPLUS-990
                errorDiv.InnerHtml = "Error attempting to update the status table: " + ex.Message+
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
                errorDiv.InnerHtml = ErrorMessage.Substring(3)+
                    "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                errorDiv.Style.Value = "display: block";
                flag = true;

            }
        }

        protected void FormCancelButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("NetworkDevicesGrid.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
            Context.ApplicationInstance.CompleteRequest();
        }

		 protected void PingButton_Click(object sender, EventArgs e)
		 {
             try
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
                         //4/10/2015 NS modified
                         //ErrorMessagePopupControl.HeaderText = "Response Time";
                         //ErrorMessageLabel.Text = "Ping to " + reply.Address + " responded in " + reply.RoundtripTime + " ms.";
                         //ErrorMessagePopupControl.ShowOnPageLoad = true;
                         //successDiv.InnerHtml = "Ping to " + reply.Address + " responded in " + reply.RoundtripTime + " ms." +
                         //   "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                         //successDiv.Style.Value = "display: block";


                         Div1.InnerHtml = "Ping to " + reply.Address + " responded in " + reply.RoundtripTime + " ms." +

                         "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                         Div1.Style.Value = "display: block";
                         errorDiv.Style.Value = "display: none";

                     }
                 }
                 else
                 {
                     //4/10/2015 NS modified
                     //ErrorMessageLabel.Text = IPTextBox.Text + " did not respond.";
                     //ErrorMessagePopupControl.ShowOnPageLoad = true;
                     errorDiv.InnerHtml = IPTextBox.Text + " did not respond." +
                            "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                     errorDiv.Style.Value = "display: block";
                     Div1.Style.Value = "display: none";
                 }
             }
             catch
             {
                 errorDiv.InnerHtml =  "Please enter valid IP or Host Name." +
                          "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                 errorDiv.Style.Value = "display: block";
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

        }


		protected void ASPxMenu1_ItemClick(object source, DevExpress.Web.MenuItemEventArgs e)
		{


			//if (e.Item.Name == "ServerDetailsPage")
			//{
			//    Response.Redirect("~/DashBoard/DominoServerDetailsPage2.aspx", false);
			//    Context.ApplicationInstance.CompleteRequest();
			//}

			DataTable dt;
			Session["Submenu"] = "LotusDomino";
			//if (Request.QueryString["Name"] != "" && Request.QueryString["Name"] != null)
			//{
			//    id = (VSWebBL.SecurityBL.ServersBL.Ins.GetServerIDbyServerName(Request.QueryString["Name"].ToString())).ToString();
			//    Response.Redirect("~/Dashboard/DominoServerDetailsPage2.aspx?Key=" + id, false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
			//    Context.ApplicationInstance.CompleteRequest();
			//}
			if (Request.QueryString["ID"] != "" && Request.QueryString["ID"] != null)
			{
                //9/14/2015 NS modified for VSPLUS-2148
				//dt = (VSWebBL.SecurityBL.ServersBL.Ins.GetServerDetailsByID(Convert.ToInt32(Request.QueryString["ID"])));
                dt = VSWebBL.ConfiguratorBL.NetworkDevicesBL.Ins.GetDataByID(Request.QueryString["ID"].ToString());
				if (dt.Rows.Count > 0)
				{
					Response.Redirect("~/Dashboard/NetworkServerDetails.aspx?Name=" + dt.Rows[0]["ServerName"].ToString() + "&Type=" + dt.Rows[0]["Type"].ToString() + "&LastDate=" + dt.Rows[0]["LastUpdate"].ToString(), false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
					Context.ApplicationInstance.CompleteRequest();
				}
			}


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
		//protected void checkbx_CheckedChanged(object sender, EventArgs e)
		//{
		//    if (checkbx.Checked)
		//    {
				
		//        lblSelectImage.Visible = true;
		//        Img1.Visible = true;
		//        NameLabel.Visible = true;
		//        CredentialsComboBox.Visible = true;
		//    }
		//    else
		//    {
			
		//        lblSelectImage.Visible = false;
		//        Img1.Visible = false;
		//        NameLabel.Visible = false;
		//        CredentialsComboBox.Visible = false;


		//    }
		//}
	
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
			Div1.Style.Value = "display: none";
		}

		protected void filldropdown_CredentialsComboBox()
		{
			DataTable dt = new DataTable();
			//string item = CredentialsComboBox.SelectedItem.Text;
			dt = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.filldropdown_CredentialsComboBox();
			CredentialsComboBox.DataSource = dt;
			CredentialsComboBox.DataBind();
		}
        }





}





    