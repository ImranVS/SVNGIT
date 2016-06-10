using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using VSWebBL;
using VSWebDO;

namespace VSWebUI
{
    public partial class WebForm15 : System.Web.UI.Page
    {
        string Mode;
        int key;
        BlackBerryServers BlackBerryObject = new BlackBerryServers();
        protected void Page_Load(object sender, EventArgs e)
        {

            Page.Title = "BlackBerry Servers Properties";

            if (Request.QueryString["tab"] != null)
            {
                ASPxPageControl1.ActiveTabIndex = Convert.ToInt32(Request.QueryString["tab"].ToString());
            }

            if (Request.QueryString["Key"] != "" && Request.QueryString["Key"] != null)
            {
                Mode = "update";
                key = int.Parse(Request.QueryString["Key"]);
                if (!IsPostBack)
                {

                    //getcombo(); //get the combo details
                    filldataBlackBerryService(key);
                    CmbBSEVersion();
                    //5/22/2015 NS modified
                    //BBRoundPanel.HeaderText = "BES Modules to be monitored on " + AddressTextBox.Text;
                    servernamelbldisp.InnerHtml += " - " + AddressTextBox.Text;
                    FillMaintenanceGrid();
                    FillAlertGridView();
                    if (Session["UserPreferences"] != null)
                    {
                        DataTable UserPreferences = (DataTable)Session["UserPreferences"];
                        foreach (DataRow dr in UserPreferences.Rows)
                        {
                            if (dr[1].ToString() == "BlackBerryEntertpriseServer|MaintWinListGridView")
                            {
                                MaintWinListGridView.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                            }
                            if (dr[1].ToString() == "BlackBerryEntertpriseServer|AlertGridView")
                            {
                                AlertGridView.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                            }
                        }
                    }
                }
                else
                {
                    FillMaintServersGridfromSession();
                    FillAlertGridViewfromSession();

                }
            }
            else
            {
                Mode = "Insert";
                if (!IsPostBack)
                {
                    CmbBSEVersion();
                    RetryIntervalTextBox.Text = "2";
                    OffHoursScanIntervalTextBox.Text = "30";
                    ScanIntervalTextBox.Text = "8";
                    pendingTextBox.Text = "1000";
                    EnableforScanningCheckBox.Checked = true;
                    TimeZoneAdjestmentTextBox.Text = "0";
                    SNMPCommunityTextBox.Text = "public";
                    UsdateformateCheckBox.Checked = true;
                    BlackBerrySychronizationServiceCheckBox.Enabled = false;
                    BlackBerrySychronizationServiceCheckBox.Checked = false;
                    BlackBerryPolicyServiceCheckBox.Enabled = false;
                    BlackBerryPolicyServiceCheckBox.Checked = false;
                    BlackBerryMobileDataServiceCheckBox.Enabled = false;
                    BlackBerryMobileDataServiceCheckBox.Checked = false;
                    BlackBerryMDSServicesCheckBox.Enabled = false;
                    BlackBerryMDSServicesCheckBox.Checked = false;
                    BlackBerryMDSConnectionServiceCheckBox.Enabled = false;
                    BlackBerryMDSConnectionServiceCheckBox.Checked = false;
                    
                }
            }
        }


        //public BlackBerryServers getkey()
        //{
        //    BlackBerryServers BlackServerObject = new BlackBerryServers();
        //    BlackServerObject.key= (Object)VSWebBL.ConfiguratorBL.BlackBerryServersBL.Ins.getkey(

        //}

        //checking for the duplication of name
        //public void check()
        //{
        //    BlackBerryServers BlackBerryServerOject = new BlackBerryServers();
        //    string name=NameTextBox.Text;
        //    //BlackBerryServerOject.key= ( VSWebBL.ConfiguratorBL.BlackBerryServersBL.Ins.getkey(name);
        //    DataTable d = new DataTable();
        //    d = VSWebBL.ConfiguratorBL.BlackBerryServersBL.Ins.getkey(name);
        //    BlackBerryServerOject.key =   Convert.ToInt32(d.Rows[0]["Key"]);
        //    DataTable datawithname = new DataTable();
        //    //string name=NameTextBox.Text;
        //    datawithname = VSWebBL.ConfiguratorBL.BlackBerryServersBL.Ins.finddatawithname(BlackBerryServerOject,name);
        //    if (datawithname.Rows.Count > 0)
        //    {
        //        ErrorMessageLabel.Text = "The Name already Exists";
        //        ErrorMessagePopupControl.ShowOnPageLoad = true;
        //        ErrorMessagePopupControl.ShowCloseButton = true;
        //        ValidationOkButton.Visible = false;
        //    }
        //}


        public void CmbBSEVersion()
        {
            BSEversionComboBox.Items.Clear();
            BSEversionComboBox.Items.Add("BlackBerry 2.x", "BlackBerry 2.x");
            BSEversionComboBox.Items.Add("BlackBerry 4.0x", "BlackBerry 4.0x");
            BSEversionComboBox.Items.Add(" BlackBerry 4.1x", "BlackBerry 4.1x");
            BSEversionComboBox.Items.Add("BlackBerry 5.x", "BlackBerry 5.x");
            //WebForm15 combo = new WebForm15();
            //BSEversionComboBox.SelectedIndex = 0;

        }

        public BlackBerryServers filldataBlackBerryService(int key)
        {
            int serverid = 0;
            CmbBSEVersion(); lblprdserver.Visible = false;
            SrvAtrCategoryComboBox.Visible = false;

            BlackBerryServers BlackBerryObject = new BlackBerryServers();
            BlackBerryObject.key = key;
            DataTable dt = new DataTable();
            try
            {
                dt = VSWebBL.ConfiguratorBL.BlackBerryServersBL.Ins.getdatawithid(BlackBerryObject);
                //1/7/2014 NS modified
                if (dt.Rows.Count > 0)
                {
                    serverid = int.Parse(dt.Rows[0]["ServerID"].ToString());
                    BlackBerryObject.ServerID = serverid;
                    getcombo(BlackBerryObject);
                    if (dt.Rows[0]["HAOption"].ToString() != null && dt.Rows[0]["HAOption"].ToString() != "")
                    {
                        rbtnservermode.Value = dt.Rows[0]["HAOption"];
                    }
                    lblprdserver.Visible = true;
                    SrvAtrCategoryComboBox.Visible = true;

                    if (rbtnservermode.Value.ToString() == "Stand Alone Server")
                    {
                        lblprdserver.Visible = false;
                        SrvAtrCategoryComboBox.Visible = false;
                    }
                    if (dt.Rows[0]["HAPartner"].ToString() != null && dt.Rows[0]["HAPartner"].ToString() != "")
                    {
                        DataTable dt1 = new DataTable();
                        dt1 = VSWebBL.ConfiguratorBL.BlackBerryServersBL.Ins.getHAName(dt.Rows[0]["HAPartner"].ToString());
                        if (dt1.Rows.Count > 0)                            
                            SrvAtrCategoryComboBox.Value = dt1.Rows[0]["ServerName"];
                    }

                    if (dt.Rows[0]["Category"].ToString() != null && dt.Rows[0]["Category"].ToString() != "")
                        categoryTextBox.Text = dt.Rows[0]["Category"].ToString();
                    NameTextBox.Text = dt.Rows[0]["Name"].ToString();
					NameTextBox.Enabled = false;
                    AddressTextBox.Text = dt.Rows[0]["IPAddress"].ToString();
					AddressTextBox.Enabled = false;
                    if (dt.Rows[0]["Category"].ToString() != null && dt.Rows[0]["Category"].ToString() != "")
                        categoryTextBox.Text = dt.Rows[0]["Category"].ToString();
					//categoryTextBox.Enabled = false;
                    DescriptionTextBox.Text = dt.Rows[0]["Description"].ToString();
					DescriptionTextBox.Enabled = false;
                    if (dt.Rows[0]["PendingThreshold"].ToString() != null && dt.Rows[0]["PendingThreshold"].ToString() != "")
                        pendingTextBox.Text = dt.Rows[0]["PendingThreshold"].ToString();

                    if (dt.Rows[0]["ScanInterval"].ToString() != null && dt.Rows[0]["ScanInterval"].ToString() != "")
                        ScanIntervalTextBox.Text = dt.Rows[0]["ScanInterval"].ToString();
                    if (dt.Rows[0]["RetryInterval"].ToString() != null && dt.Rows[0]["RetryInterval"].ToString() != "")
                        RetryIntervalTextBox.Text = dt.Rows[0]["RetryInterval"].ToString();
                    if (dt.Rows[0]["Enabled"].ToString() != null && dt.Rows[0]["Enabled"].ToString() != "")
                        EnableforScanningCheckBox.Checked = Convert.ToBoolean(dt.Rows[0]["Enabled"]);
                    if (dt.Rows[0]["OffHoursScanInterval"].ToString() != null && dt.Rows[0]["OffHoursScanInterval"].ToString() != "")
                        OffHoursScanIntervalTextBox.Text = dt.Rows[0]["OffHoursScanInterval"].ToString();
                    if (dt.Rows[0]["SNMPCommunity"].ToString() != null && dt.Rows[0]["SNMPCommunity"].ToString() != "")
                        SNMPCommunityTextBox.Text = dt.Rows[0]["SNMPCommunity"].ToString();
                    //if (dt.Rows[0]["NotificationGroup"].ToString() != null && dt.Rows[0]["NotificationGroup"].ToString() != "")
                    //    SendToTextBox.Text = dt.Rows[0]["NotificationGroup"].ToString();
                    if (dt.Rows[0]["Router"].ToString() != null && dt.Rows[0]["Router"].ToString() != "")
                        BlackBerryRouterServiceCheckBox.Checked = Convert.ToBoolean(dt.Rows[0]["Router"].ToString());
                    if (dt.Rows[0]["Alert"].ToString() != null && dt.Rows[0]["Alert"].ToString() != "")
                        BlackBerryAlertServiceCheckBox.Checked = Convert.ToBoolean(dt.Rows[0]["Alert"].ToString());
                    if (dt.Rows[0]["Attachment"].ToString() != null && dt.Rows[0]["Attachment"].ToString() != "")
                        BlackBerryAttachmentServiceCheckBox.Checked = Convert.ToBoolean(dt.Rows[0]["Attachment"].ToString());
                    if (dt.Rows[0]["RouterIP"].ToString() != null && dt.Rows[0]["RouterIP"].ToString() != "")
                        BlackBerryRouterserviceTextBox.Text = dt.Rows[0]["RouterIP"].ToString();
                    if (dt.Rows[0]["AlertIP"].ToString() != null && dt.Rows[0]["AlertIP"].ToString() != "")
                        BlackBerryAlertServiceTextBox.Text = dt.Rows[0]["AlertIP"].ToString();
                    if (dt.Rows[0]["AttachmentIP"].ToString() != null && dt.Rows[0]["AttachmentIP"].ToString() != "")
                        BlackBerryAttachmTextBox.Text = dt.Rows[0]["AttachmentIP"].ToString();
                    if (dt.Rows[0]["TimeZoneAdjustment"].ToString() != null && dt.Rows[0]["TimeZoneAdjustment"].ToString() != "")
                        TimeZoneAdjestmentTextBox.Text = dt.Rows[0]["TimeZoneAdjustment"].ToString();
                    if (dt.Rows[0]["USDateFormat"].ToString() != null && dt.Rows[0]["USDateFormat"].ToString() != "")
                        UsdateformateCheckBox.Checked = Convert.ToBoolean(dt.Rows[0]["USDateFormat"]);
                    lblSid.Text = dt.Rows[0]["SID"].ToString();

                    if (dt.Rows[0]["Messaging"].ToString() != null && dt.Rows[0]["Messaging"].ToString() != "")
                        BlackBerryMessagingAgentCheckBox.Checked = Convert.ToBoolean(dt.Rows[0]["Messaging"]);
                    if (dt.Rows[0]["Controller"].ToString() != null && dt.Rows[0]["Controller"].ToString() != "")
                        BlackBerryControllerServiceCheckBox.Checked = Convert.ToBoolean(dt.Rows[0]["Controller"]);
                    if (dt.Rows[0]["Dispatcher"].ToString() != null && dt.Rows[0]["Dispatcher"].ToString() != "")
                        BlackBerryDispacherServiceCheckBox.Checked = Convert.ToBoolean(dt.Rows[0]["Dispatcher"]);
                    if (dt.Rows[0]["Synchronization"].ToString() != null && dt.Rows[0]["Synchronization"].ToString() != "")
                        BlackBerrySychronizationServiceCheckBox.Checked = Convert.ToBoolean(dt.Rows[0]["Synchronization"]);
                    if (dt.Rows[0]["Policy"].ToString() != null && dt.Rows[0]["Policy"].ToString() != "")
                        BlackBerryPolicyServiceCheckBox.Checked = Convert.ToBoolean(dt.Rows[0]["Policy"]);
                    if (dt.Rows[0]["MDSServices"].ToString() != null && dt.Rows[0]["MDSServices"].ToString() != "")
                        BlackBerryMDSServicesCheckBox.Checked = Convert.ToBoolean(dt.Rows[0]["MDSServices"]);
                    if (dt.Rows[0]["MDSConnection"].ToString() != null && dt.Rows[0]["MDSConnection"].ToString() != "")
                        BlackBerryMDSConnectionServiceCheckBox.Checked = Convert.ToBoolean(dt.Rows[0]["MDSConnection"]);
                    if (dt.Rows[0]["MDS"].ToString() != null && dt.Rows[0]["MDS"].ToString() != "")
                        BlackBerryMobileDataServiceCheckBox.Checked = Convert.ToBoolean(dt.Rows[0]["MDS"]);

                    BlackBerryMessagingAgentCheckBox.Enabled = true;
                    BlackBerryControllerServiceCheckBox.Enabled = true;
                    BlackBerryDispacherServiceCheckBox.Enabled = true;
                    BlackBerrySychronizationServiceCheckBox.Enabled = true;
                    BlackBerryPolicyServiceCheckBox.Enabled = true;
                    BlackBerryMDSServicesCheckBox.Enabled = true;
                    BlackBerryMDSConnectionServiceCheckBox.Enabled = true;
                    BlackBerryMobileDataServiceCheckBox.Enabled = true;

                    Session["ReturnUrl"] = "BlackBerryEntertpriseServer.aspx?key=" + lblSid.Text + "&tab=3";

                    BSEversionComboBox.Text = dt.Rows[0]["BESVersion"].ToString();
                    if (BSEversionComboBox.Text == "BlackBerry 2.x")
                    {
                        BlackBerrySychronizationServiceCheckBox.Enabled = false;
                        BlackBerrySychronizationServiceCheckBox.Checked = false;
                        BlackBerryPolicyServiceCheckBox.Enabled = false;
                        BlackBerryPolicyServiceCheckBox.Checked = false;
                        BlackBerryMobileDataServiceCheckBox.Enabled = false;
                        BlackBerryMobileDataServiceCheckBox.Checked = false;
                        BlackBerryMDSServicesCheckBox.Enabled = false;
                        BlackBerryMDSServicesCheckBox.Checked = false;
                        BlackBerryMDSConnectionServiceCheckBox.Enabled = false;
                        BlackBerryMDSConnectionServiceCheckBox.Checked = false;
                        //  SendToTextBox.Enabled = false;
                    }
                    if (BSEversionComboBox.Text == "BlackBerry 4.1x")
                    {
                        BlackBerryMobileDataServiceCheckBox.Enabled = false;
                        BlackBerryMobileDataServiceCheckBox.Checked = false;
                        BlackBerryMDSServicesCheckBox.Enabled = true;
                        BlackBerryMessagingAgentCheckBox.Enabled = false;
                        BlackBerryMessagingAgentCheckBox.Checked = false;
                        BlackBerryMDSConnectionServiceCheckBox.Enabled = true;
                    }

                    if (BSEversionComboBox.Text == "BlackBerry 4.0x")
                    {
                        BlackBerryMDSConnectionServiceCheckBox.Enabled = false;
                        BlackBerryMDSServicesCheckBox.Enabled = false;
                        BlackBerryMDSConnectionServiceCheckBox.Checked = false;
                        BlackBerryMDSServicesCheckBox.Checked = false;
                        BlackBerryMobileDataServiceCheckBox.Enabled = true;
                    }

                    if (BSEversionComboBox.Text == "BlackBerry 5.x")
                    {
                        BlackBerryMobileDataServiceCheckBox.Enabled = false;
                        BlackBerryMobileDataServiceCheckBox.Checked = false;
                        BlackBerryMDSServicesCheckBox.Enabled = true;
                        BlackBerryMessagingAgentCheckBox.Enabled = false;
                        BlackBerryMessagingAgentCheckBox.Checked = false;

                    }
                }
            }
            catch (Exception ex)
            {
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
            }
            return BlackBerryObject;

        }

        public BlackBerryServers getdataBlackservice()
        {
            BlackBerryServers BlackBerryObject = new BlackBerryServers();

            BlackBerryObject.Address = AddressTextBox.Text;
            BlackBerryObject.Category = categoryTextBox.Text;
            BlackBerryObject.Description = DescriptionTextBox.Text;
            //5/22/2015 NS modified for VSPLUS-1792
            //BlackBerryObject.Enabled = Convert.ToBoolean(EnableforScanningCheckBox.Enabled);
            BlackBerryObject.Enabled = Convert.ToBoolean(EnableforScanningCheckBox.Checked);
            BlackBerryObject.Name = NameTextBox.Text;
            BlackBerryObject.RetryInterval = Convert.ToInt32(RetryIntervalTextBox.Text);
            BlackBerryObject.ScanInterval = Convert.ToInt32(ScanIntervalTextBox.Text);
            BlackBerryObject.OffHoursScanInterval = Convert.ToInt32(OffHoursScanIntervalTextBox.Text);
            BlackBerryObject.HAOption = rbtnservermode.SelectedItem.Text;
           // BlackBerryObject.HAOption = Session["r2"].ToString();

            if (rbtnservermode.SelectedIndex.ToString() == "Stand Alone Server")
            {
                rbtnservermode.SelectedIndex = 0;
            }
            DataTable dt = new DataTable();
            //6/27/2014 NS added for VSPLUS-634 - the getServerID function must be called in try/catch to trap any issues
            try
            {
                dt = VSWebBL.ConfiguratorBL.BlackBerryServersBL.Ins.getServerID(SrvAtrCategoryComboBox.Text);
                //6/27/2014 NS added for VSPLUS-634 - the data table must contain at least one row
                if (dt.Rows.Count > 0)
                {
                    BlackBerryObject.HAPartner = Convert.ToString(dt.Rows[0]["ID"].ToString()); //Session["d2"].ToString();
                }
            }
            catch (Exception ex)
            {
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
            }
            if (BlackBerryObject.HAOption.ToLower() == "stand alone server")
            {
                BlackBerryObject.HAPartner = "0";
            }

            if (pendingTextBox.Text != "")
                BlackBerryObject.PendingThreshold = Convert.ToInt32(pendingTextBox.Text);
            if (Mode == "update")
            {
                BlackBerryObject.key = key;
            }
            BlackBerryObject.Policy = Convert.ToBoolean(BlackBerryPolicyServiceCheckBox.Checked);
            BlackBerryObject.Synchronization = Convert.ToBoolean(BlackBerrySychronizationServiceCheckBox.Checked);
            BlackBerryObject.Controller = Convert.ToBoolean(BlackBerryControllerServiceCheckBox.Checked);
            BlackBerryObject.Messaging = Convert.ToBoolean(BlackBerryMessagingAgentCheckBox.Checked);
            BlackBerryObject.Dispatcher = Convert.ToBoolean(BlackBerryDispacherServiceCheckBox.Checked);
            BlackBerryObject.MDS = Convert.ToBoolean(BlackBerryMobileDataServiceCheckBox.Checked);
            BlackBerryObject.MDSConnection = Convert.ToBoolean(BlackBerryMDSConnectionServiceCheckBox.Checked);
            BlackBerryObject.MDSServices = Convert.ToBoolean(BlackBerryMDSServicesCheckBox.Checked);
            if (OtherServicesToMonitorTextBox.Text != "")
            {
                BlackBerryObject.OtherServices = OtherServicesToMonitorTextBox.Text;
            }
            else
            {
                BlackBerryObject.OtherServices = "None";
            }
            BlackBerryObject.SNMPCommunity = SNMPCommunityTextBox.Text;
            BlackBerryObject.BESVersion = BSEversionComboBox.Text;

            BlackBerryObject.Router = BlackBerryRouterServiceCheckBox.Checked;
            BlackBerryObject.Alert = BlackBerryAlertServiceCheckBox.Checked;
            if (BlackBerryAlertServiceTextBox.Text != "")
            {

                BlackBerryObject.AlertIP = BlackBerryAlertServiceTextBox.Text;
            }
            else
            {
                BlackBerryObject.AlertIP = AddressTextBox.Text;
            }
            if (BlackBerryRouterserviceTextBox.Text != "")
            {
                BlackBerryObject.RouterIP = BlackBerryRouterserviceTextBox.Text;
            }
            else
            {
                BlackBerryObject.RouterIP = AddressTextBox.Text;
            }
            BlackBerryObject.Attachment = BlackBerryAttachmentServiceCheckBox.Checked;
            if (BlackBerryAttachmTextBox.Text != "")
            {
                BlackBerryObject.AttachmentIP = BlackBerryAttachmTextBox.Text;
            }
            else
            {
                BlackBerryObject.AttachmentIP = AddressTextBox.Text;
            }
            if (TimeZoneAdjestmentTextBox.Text != null && TimeZoneAdjestmentTextBox.Text != "")
                BlackBerryObject.TimeZoneAdjustment = Convert.ToInt32(TimeZoneAdjestmentTextBox.Text);
            BlackBerryObject.USDateFormat = UsdateformateCheckBox.Checked;
            // BlackBerryObject.NotificationGroup = SendToTextBox.Text;
            BlackBerryObject.ServerID = int.Parse(lblSid.Text);

            return BlackBerryObject;
        }


        private void SetFocusOnError(Object ReturnValue)
        {
            string ErrorMessage = ReturnValue.ToString();
            if (ErrorMessage.Substring(0, 2) == "ER")
            {
                //3/19/2014 NS modified
                //ErrorMessageLabel.Text = ErrorMessage.Substring(3);
                //ErrorMessagePopupControl.ShowOnPageLoad = true;
                //10/3/2014 NS modified for VSPLUS-990
                errorDiv.InnerHtml = ErrorMessage.Substring(3)+
                    "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                errorDiv.Style.Value = "display: block";
            }
        }

        //public void updatecheck()
        //{

        //}


        public void updateBlackBerryService()
        {
            //BlackBerryServers BlackBerryServerObject = new BlackBerryServers();
            //BlackBerryServerObject.key = key;
            Object updatedetails;
            try
            {
                updatedetails = VSWebBL.ConfiguratorBL.BlackBerryServersBL.Ins.updatedetails(getdataBlackservice());
                SetFocusOnError(updatedetails);
                if (updatedetails.ToString() == "True")
                {
                    //3/19/2014 NS modified to redirect to the BB grid page
                    /*
                    ErrorMessageLabel.Text = "Data updated successfully.";
                    ErrorMessagePopupControl.HeaderText = "Information";
                    ErrorMessagePopupControl.ShowCloseButton = false;
                    ValidationUpdatedButton.Visible = true;
                    ValidationOkButton.Visible = false;
                    ErrorMessagePopupControl.ShowOnPageLoad = true;
                     */
                    Session["BlackberryUpdateStatus"] = NameTextBox.Text;
                    Response.Redirect("BlackBerry.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
                    Context.ApplicationInstance.CompleteRequest();
                }

            }
            catch (Exception ex)
            {
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
            }
        }

        public void insertBlackBerryService()
        {
            BlackBerryServers BlackBerryServerObject = new BlackBerryServers();
            Object inserObject;
            try
            {
                inserObject = VSWebBL.ConfiguratorBL.BlackBerryServersBL.Ins.insertBlackBerryServer(getdataBlackservice());
                SetFocusOnError(inserObject);
                if (inserObject.ToString() == "True")
                {
                    //3/19/2014 NS modified to redirect to the BB grid page
                    /*
                    ErrorMessageLabel.Text = "Data inserted successfully.";
                    ErrorMessagePopupControl.HeaderText = "Information";
                    ErrorMessagePopupControl.ShowCloseButton = false;
                    ValidationUpdatedButton.Visible = true;
                    ValidationOkButton.Visible = false;
                    ErrorMessagePopupControl.ShowOnPageLoad = true;
                     */
                    Session["BlackberryUpdateStatus"] = NameTextBox.Text;
                    Response.Redirect("BlackBerry.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
                    Context.ApplicationInstance.CompleteRequest();
                }
            }
            catch (Exception ex)
            {
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
            }
        }

        protected void ValidationUpdatedButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("BlackBerry.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
            Context.ApplicationInstance.CompleteRequest();

        }

        protected void OKButton_Click(object sender, EventArgs e)
        {
            if (Mode == "update")
            {
                // check();
                if (ErrorMessageLabel.Text == "")
                {
                    updateBlackBerryService();
                }

				DataTable returntable = VSWebBL.ConfiguratorBL.BlackBerryServersBL.Ins.GetBESName(NameTextBox.Text);
				if (returntable.Rows.Count == 0)
				{
					getdataforstatustable();
				}
                forstatustable();
            }
            if (Mode == "Insert")
			{
			
                // check();
                if (ErrorMessageLabel.Text == "")
                {
                    insertBlackBerryService();
                }
            }
            BlackBerryObject.HAOption = rbtnservermode.SelectedIndex.ToString();
            BlackBerryObject.HAPartner = Convert.ToString(SrvAtrCategoryComboBox.Value);

            //Object inserObject;
            //inserObject = VSWebBL.ConfiguratorBL.BlackBerryServersBL.Ins.Insert(BlackBerryObject);


            //int i = VSWebBL.SettingBL.SettingsBL.Ins.Insert(r2val, d2val);

        }

        public void forstatustable()
        {
            string sname = "BlackBerry Server Update";
            string svalue = "True";
            Object returval;
            //6/27/2014 NS added for VSPLUS-634
            try
            {
                returval = VSWebBL.SettingBL.SettingsBL.Ins.UpdateSvalue(sname, svalue, VSWeb.Constants.Constants.SysString);
            }
            catch (Exception ex)
            {
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
            }
        }



        protected void TestButton_Click(object sender, EventArgs e)
        {
            //  BlackBerryServers BlackBerryServersObject = new BlackBerryServers();
            // Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
            //  Me.StatusText.Text = "Opening BlackBerry Server...please wait."
            // Application.DoEvents();
            //  frmPingBESNow myForm = new frmPingBESNow();
            //myForm.myBESCaller = this;
            //myForm.myIPAddress = this.txtIPAddress.Text;
            //myForm.myCommunity = this.txtSNMPCommunity.Text;
            //myForm.ShowDialog();
            //string sendto,snmp;
            //sendto = SendToTextBox.Text;
            // snmp = SNMPCommunityTextBox.Text;
            //  Response.Redirect("QueryBESServer.aspx?sendto="+SendToTextBox.Text+"&smnp="+SNMPCommunityTextBox.Text+"");
        }

        protected void BSEversionComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {


            if (BSEversionComboBox.Value.ToString() == "BlackBerry 2.x")
            {
                BlackBerrySychronizationServiceCheckBox.Enabled = false;
                BlackBerrySychronizationServiceCheckBox.Checked = false;
                BlackBerryPolicyServiceCheckBox.Enabled = false;
                BlackBerryPolicyServiceCheckBox.Checked = false;
                BlackBerryMobileDataServiceCheckBox.Enabled = false;
                BlackBerryMobileDataServiceCheckBox.Checked = false;
                BlackBerryMDSServicesCheckBox.Enabled = false;
                BlackBerryMDSServicesCheckBox.Checked = false;
                BlackBerryMDSConnectionServiceCheckBox.Enabled = false;
                BlackBerryMDSConnectionServiceCheckBox.Checked = false;
                //SendToTextBox.Enabled = false;
            }

            if (BSEversionComboBox.Value.ToString() == "BlackBerry 4.0x")
            {
                BlackBerryMDSConnectionServiceCheckBox.Enabled = false;
                BlackBerryMDSServicesCheckBox.Enabled = false;
                BlackBerryMDSConnectionServiceCheckBox.Checked = false;
                BlackBerryMDSServicesCheckBox.Checked = false;
                BlackBerryMobileDataServiceCheckBox.Enabled = true;
            }
            if (BSEversionComboBox.Value.ToString() == "BlackBerry 4.1x")
            {
                BlackBerryMobileDataServiceCheckBox.Enabled = false;
                BlackBerryMobileDataServiceCheckBox.Checked = false;
                BlackBerryMDSServicesCheckBox.Enabled = true;
                BlackBerryMessagingAgentCheckBox.Enabled = false;
                BlackBerryMessagingAgentCheckBox.Checked = false;
                BlackBerryMDSConnectionServiceCheckBox.Enabled = true;

            }
            if (BSEversionComboBox.Value.ToString() == "BlackBerry 5.x")
            {
                BlackBerryMobileDataServiceCheckBox.Enabled = false;
                BlackBerryMobileDataServiceCheckBox.Checked = false;
                BlackBerryMDSServicesCheckBox.Enabled = true;
                BlackBerryMDSServicesCheckBox.Checked = false;
                BlackBerryMessagingAgentCheckBox.Enabled = false;
                BlackBerryMessagingAgentCheckBox.Checked = false;
            }


        }

        public Status getdataforstatus()
        {
            Status statusObject = new Status();
            statusObject.Category = categoryTextBox.Text;
            statusObject.DeadMail = '0';
            statusObject.Description = "Sends test messages to a BlackBerry Device via BlackBerry Enterprise Server for Domino.";
            statusObject.DownCount = '0';
            statusObject.Name = NameTextBox.Text;
            statusObject.MailDetails = "";
            statusObject.PendingMail = '0';
            statusObject.sStatus = "Not Scanned";
            statusObject.Type = "BlackBerry Device";
            statusObject.Upcount = '0';
            statusObject.UpPercent = '1';
            statusObject.LastUpdate = System.DateTime.Now;
            statusObject.ResponseTime = '0';
            statusObject.TypeANDName = NameTextBox.Text + "-BlackBerry";
            return statusObject;
        }

        public void getdataforstatustable()
        {
            Object retval;
            //6/27/2014 NS added for VSPLUS-634
            try
            {
                retval = VSWebBL.StatusBL.StatusTBL.Ins.InsertData(getdataforstatus());
            }
            catch (Exception ex)
            {
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
            }
        }





        protected void navgButton_Click(object sender, EventArgs e)
        {

        }

        protected void CancelButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("BlackBerry.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
            Context.ApplicationInstance.CompleteRequest();
        }

        private void FillMaintenanceGrid()
        {
            try
            {

                DataTable MaintDataTable = new DataTable();
                DataSet ServersDataSet = new DataSet();
                MaintDataTable = VSWebBL.ConfiguratorBL.MaintenanceBL.Ins.GetMaintenDataOnServerID(NameTextBox.Text);
                if (MaintDataTable.Rows.Count > 0)
                {
                    DataTable dtcopy = MaintDataTable.Copy();
                    dtcopy.PrimaryKey = new DataColumn[] { dtcopy.Columns["ID"] };
                    //4/3/2014 NS modified for VSPLUS-138
                    //Session["MaintServers"] = dtcopy;
                    ViewState["MaintServers"] = dtcopy;
                    MaintWinListGridView.DataSource = MaintDataTable;
                    MaintWinListGridView.DataBind();
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

        private void FillMaintServersGridfromSession()
        {
            try
            {

                DataTable ServersDataTable = new DataTable();
                //4/3/2014 NS modified for VSPLUS-138
                //if (Session["MaintServers"] != null && Session["MaintServers"] != "")
                if (ViewState["MaintServers"] != null && ViewState["MaintServers"] != "")
                {
                    //4/3/2014 NS modified for VSPLUS-138
                    //ServersDataTable = (DataTable)Session["MaintServers"];
                    ServersDataTable = (DataTable)ViewState["MaintServers"];
                    if (ServersDataTable.Rows.Count > 0)
                    {
                        MaintWinListGridView.DataSource = ServersDataTable;
                        MaintWinListGridView.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex; }
            finally { }
        }

        private void FillAlertGridView()
        {
            try
            {

                DataTable AlertDataTable = new DataTable();
                DataSet AlertDataSet = new DataSet();
                AlertDataTable = VSWebBL.ConfiguratorBL.AlertsBL.Ins.GetAlertTab(NameTextBox.Text, "BES");
                if (AlertDataTable.Rows.Count > 0)
                {
                    DataTable dtcopy = AlertDataTable.Copy();
                    // dtcopy.PrimaryKey = new DataColumn[] { dtcopy.Columns["ID"] };
                    //4/3/2014 NS modified for VSPLUS-138
                    //Session["AlertDataTable"] = dtcopy;
                    ViewState["AlertDataTable"] = dtcopy;
                    AlertGridView.DataSource = AlertDataTable;
                    AlertGridView.DataBind();
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
        private void FillAlertGridViewfromSession()
        {
            try
            {
                DataTable AlertDataTable = new DataTable();
                //4/3/2014 NS modified for VSPLUS-138
                //if (Session["AlertDataTable"] != "" && Session["AlertDataTable"] != null)
                //    AlertDataTable = (DataTable)Session["AlertDataTable"];//VSWebBL.ConfiguratorBL.DominoPropertiesBL.Ins.GetAllData();
                if (ViewState["AlertDataTable"] != "" && ViewState["AlertDataTable"] != null)
                    AlertDataTable = (DataTable)ViewState["AlertDataTable"];//VSWebBL.ConfiguratorBL.DominoPropertiesBL.Ins.GetAllData();
                if (AlertDataTable.Rows.Count > 0)
                {
                    AlertGridView.DataSource = AlertDataTable;
                    AlertGridView.DataBind();
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
        protected void MaintWinListGridView_HtmlRowCreated(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
        {
            e.Row.Attributes.Add("onmouseover", "this.style.backgroundColor='#C0C0C0';");

            e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='white';");
        }

        protected void MaintWinListGridView_SelectionChanged(object sender, EventArgs e)
        {
            if (MaintWinListGridView.Selection.Count > 0)
            {
                System.Collections.Generic.List<object> Type = MaintWinListGridView.GetSelectedFieldValues("ID");

                if (Type.Count > 0)
                {
                    string ID = Type[0].ToString();

                    //Mukund: VSPLUS-844, Page redirect on callback
                    try
                    {
                        DevExpress.Web.ASPxWebControl.RedirectOnCallback("MaintenanceWin.aspx?ID=" + ID + "");
                        Context.ApplicationInstance.CompleteRequest();//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
                    }
                    catch (Exception ex)
                    {
                        Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                        //throw ex;
                    }
                }


            }
        }


        //    protected void BSEversionComboBox_TextChanged(object sender, EventArgs e)
        //{

        //    if (BSEversionComboBox.Value.ToString() == "BlackBerry 2.x")
        //    {
        //        BlackBerrySychronizationServiceCheckBox.Enabled = false;
        //        BlackBerrySychronizationServiceCheckBox.Checked = false;
        //        BlackBerryPolicyServiceCheckBox.Enabled = false;
        //        BlackBerryPolicyServiceCheckBox.Checked = false;
        //        BlackBerryMobileDataServiceCheckBox.Enabled = false;
        //        BlackBerryMobileDataServiceCheckBox.Checked = false;
        //        BlackBerryMDSServicesCheckBox.Enabled = false;
        //        BlackBerryMDSServicesCheckBox.Checked = false;
        //        BlackBerryMDSConnectionServiceCheckBox.Enabled = false;
        //        BlackBerryMDSConnectionServiceCheckBox.Checked = false;
        //        SendToTextBox.Enabled = false;
        //    }

        //    if (BSEversionComboBox.Value.ToString() == "BlackBerry 4.0.x")
        //    {
        //        BlackBerryMDSConnectionServiceCheckBox.Enabled = false;
        //        BlackBerryMDSServicesCheckBox.Enabled = false;
        //        BlackBerryMDSConnectionServiceCheckBox.Checked = false;
        //        BlackBerryMDSServicesCheckBox.Checked = false;
        //        BlackBerryMobileDataServiceCheckBox.Enabled = false;
        //    }
        //    if (BSEversionComboBox.Value.ToString() == "BlackBerry 4.1.x")
        //    {
        //        BlackBerryMobileDataServiceCheckBox.Enabled = false;
        //        BlackBerryMobileDataServiceCheckBox.Checked = false;
        //        BlackBerryMDSServicesCheckBox.Enabled = true;
        //        BlackBerryMDSServicesCheckBox.Checked = false;
        //        BlackBerryMessagingAgentCheckBox.Enabled = false;
        //        BlackBerryMessagingAgentCheckBox.Checked = false;
        //    }
        //    if (BSEversionComboBox.Value.ToString() == "BlackBerry 5.x")
        //    {
        //        BlackBerryMobileDataServiceCheckBox.Enabled = false;
        //        BlackBerryMobileDataServiceCheckBox.Checked = false;
        //        BlackBerryMDSServicesCheckBox.Enabled = true;
        //        BlackBerryMDSServicesCheckBox.Checked = false;
        //        BlackBerryMessagingAgentCheckBox.Enabled = false;
        //        BlackBerryMessagingAgentCheckBox.Checked = false;

        //    }





        //}

        protected void MaintWinListGridView_PageSizeChanged(object sender, EventArgs e)
        {
            //ProfilesGridView.PageIndex;
            VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("BlackBerryEntertpriseServer|MaintWinListGridView", MaintWinListGridView.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        }

        protected void AlertGridView_PageSizeChanged(object sender, EventArgs e)
        {
            //ProfilesGridView.PageIndex;
            VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("BlackBerryEntertpriseServer|AlertGridView", AlertGridView.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        }

        protected void rbtnservermode_SelectedIndexChanged(object sender, EventArgs e)
        {

            lblprdserver.Visible = true;

            if (rbtnservermode.Value.ToString() == "HA Server-Typically Active")
            {
                SrvAtrCategoryComboBox.Visible = true;
            }
            else if (rbtnservermode.Value.ToString() == "HA Server-Typically Standby")
            {
                SrvAtrCategoryComboBox.Visible = true;
            }

            else
            {
                SrvAtrCategoryComboBox.Visible = false;
                lblprdserver.Visible = false;
            }
            //string r2 = rbtnservermode.Value.ToString();
            //Session["r2"] = r2;

            //string d2 = SrvAtrCategoryComboBox.SelectedItem.Text;
            //Session["d2"] = d2;
        }


        // binding data to combo 
        public void getcombo( BlackBerryServers BlackBerryServerObject)
        {
           
            DataTable dt = new DataTable();
           // BlackBerryServers BlackBerryServerObject = new BlackBerryServers();
            try
            {
                dt = VSWebBL.ConfiguratorBL.BlackBerryServersBL.Ins.fillcombo(BlackBerryServerObject);
                if (dt.Rows.Count > 0)
                {
                    SrvAtrCategoryComboBox.DataSource = dt;
                    SrvAtrCategoryComboBox.TextField = "ServerName";
                    SrvAtrCategoryComboBox.ValueField = "ServerName";
                    SrvAtrCategoryComboBox.DataBind();
                }
            }
            catch (Exception ex)
            {
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
			if (Request.QueryString["Key"] != "" && Request.QueryString["Key"] != null)
			{
				dt = (VSWebBL.SecurityBL.ServersBL.Ins.GetServerDetailsByID(Convert.ToInt32(Request.QueryString["Key"])));
				if (dt.Rows.Count > 0)
				{
					Response.Redirect("~/Dashboard/BlackBerryServerDetailsPage2.aspx?Name=" + dt.Rows[0]["ServerName"].ToString() + "&Type=" + dt.Rows[0]["Type"].ToString() + "&LastDate=" + dt.Rows[0]["LastUpdate"].ToString(), false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
					Context.ApplicationInstance.CompleteRequest();
				}
			}


		}
    }
}