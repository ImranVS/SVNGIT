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


namespace VSWebUI.Configurator
{
    public partial class MailService : System.Web.UI.Page
    {
        int ServerKey;
        string Mode;
        bool flag = false;
        protected void  Page_Load(object sender, EventArgs e)
        {         
            if (Request.QueryString["tab"] != null)
            {
                ASPxPageControl1.ActiveTabIndex = Convert.ToInt32(Request.QueryString["tab"].ToString());
            }

            //PortTextBox.Enabled = false;
             try
            {
                if (Request.QueryString["key"] != null && Request.QueryString["key"] != "")
                {
                    Mode = "Update";

                    ServerKey = int.Parse(Request.QueryString["key"]);


                    if (!IsPostBack)
                    {
                        FillAlertGridView();
                        filLoccombo();

                        FillData(ServerKey);

                        FillMaintenanceGrid();
                        //10/21/2014 NS modified for VSPLUS-934
                        //MailServiceRoundPanel.HeaderText = "Mail Service -" + " " + MSNameTextBox.Text;
                        lblServer.InnerHtml += " - " + MSNameTextBox.Text;

                        if (Session["UserPreferences"] != null)
                        {
                            DataTable UserPreferences = (DataTable)Session["UserPreferences"];
                            foreach (DataRow dr in UserPreferences.Rows)
                            {
                                if (dr[1].ToString() == "MailService|MaintWinListGridView")
                                {
                                    MaintWinListGridView.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                                }
                                if (dr[1].ToString() == "MailService|AlertGridView")
                                {
                                    AlertGridView.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                                }
                            }
                        }
                    }
                    else
                    {
                        FillAlertGridViewfromSession();
                        FillMaintServersGridfromSession();
                    }


                }
                else
                {
                    Mode = "Insert";
                    if (!IsPostBack)
                    {
                        try
                        {
                            filLoccombo();
                            RetryIntervalTextBox.Text = "2";
                            OffScanIntervalTextBox.Text = "30";
                            ScanIntervalTextBox.Text = "8";
                            MSRespThresholdTextBox.Text = "4000";
                            ProtocolComboBox.Value = "SMTP";
                            PortTextBox.Text = "25";
                            FailureTextBox.Text = "2";
                            EnableCheckBox.Checked = true;
                            MSNameTextBox.Focus();
                           
                        }
                        catch (Exception ex)
                        {
                            //6/27/2014 NS added for VSPLUS-634
                            Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                            throw ex;
                        }
                       
                        
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
       

        public DataTable RestrServers()
        {
            
            DataTable DCTasksDataTable = VSWebBL.ConfiguratorBL.MailServicesBL.Ins.GetServers();
            //4/3/2014 NS modified for VSPLUS-138
            //if (Session["RestrictedServers"] != "" && Session["RestrictedServers"] != null)
            if (ViewState["RestrictedServers"] != "" && ViewState["RestrictedServers"] != null)
            {
                
                 DataTable newresdt=new DataTable();
                List<int> ServerID = new List<int>();
                List<int> LocationID = new List<int>();
                //4/3/2014 NS modified for VSPLUS-138
                //DataTable resServers = (DataTable)Session["RestrictedServers"];
                DataTable resServers = (DataTable)ViewState["RestrictedServers"];
                foreach (DataRow dominorow in DCTasksDataTable.Rows)
                {
                    foreach (DataRow resser in resServers.Rows)
                    {
                        if (resser["serverid"].ToString() == dominorow["ID"].ToString())
                        {
                            ServerID.Add(DCTasksDataTable.Rows.IndexOf(dominorow));
                        }
                        if (resser["locationID"].ToString() == dominorow["LocationID"].ToString())
                        {

                            LocationID.Add(Convert.ToInt32(dominorow["LocationID"].ToString()));
                           
                           
                           //int lid =Convert.ToInt32(resser["locationID"].ToString());
                           //DataRow[] row  = DCTasksDataTable.Select("LocationID=" + lid + "");
                           // newresdt=row.CopyToDataTable();
                           // rowlocation=

                            //LocationID.Add(DCTasksDataTable.Rows.IndexOf(dominorow));
                        }
                    }

                }
                foreach (int Id in ServerID)
                {
                    DCTasksDataTable.Rows[Id].Delete();
                }
                DCTasksDataTable.AcceptChanges();
                //for (int i = 0; i <; i++)
                //{

                //}
                //int i = 0;
                foreach (int lid in LocationID)
                {
                    DataRow[] row = DCTasksDataTable.Select("LocationID=" + lid + "");
                    for (int i = 0; i < row.Count(); i++)
                    {
                        DCTasksDataTable.Rows.Remove(row[i]);
                        DCTasksDataTable.AcceptChanges();
                    }
                }

                //foreach (DataRow rowlid in newresdt.Rows)
                //{
                //    //row = DCTasksDataTable.Select("LocationID=" + lid + "");
                //    DCTasksDataTable.Rows.Remove(rowlid);
                    
                //    //i++;
                //}
                DCTasksDataTable.AcceptChanges();
                //for (int c = 0; c < row.Count(); c++)
                //{

                //}
              

            }
            return DCTasksDataTable;
        }

        private void FillData(int ID)
        {
            try
            {

                MailServices MailServicesObject = new MailServices();
                MailServices ReturnObject = new MailServices();
                MailServicesObject.key = ID;                         
                //MailServicesObject.key = ID;
               
                ReturnObject = VSWebBL.ConfiguratorBL.MailServicesBL.Ins.GetData(MailServicesObject);
                if (ReturnObject.Status != "")
                {
                    MSNameTextBox.Text = ReturnObject.Name.ToString();
                    AddressTextBox.Text = ReturnObject.Address.ToString();
                    ProtocolComboBox.Value = ReturnObject.Category.ToString();
                    DescriptionTextBox.Text = ReturnObject.Description.ToString();
                    EnableCheckBox.Checked = ReturnObject.Enabled;
                    PortTextBox.Text = ReturnObject.Port.ToString();
                    ScanIntervalTextBox.Text = ReturnObject.ScanInterval.ToString();
                    OffScanIntervalTextBox.Text = ReturnObject.OffHoursScanInterval.ToString();
                    RetryIntervalTextBox.Text = ReturnObject.RetryInterval.ToString();
                    FailureTextBox.Text = ReturnObject.FailureThreshold.ToString();
                    //ServersCombobox.Text = ReturnObject.ServerName.ToString();
                   //ServersCombobox.SelectedItem.Value = ReturnObject.ID;
                    //10/28/2013 NS modified - the correct object containing the Response Threshold value is ReturnObject
                    //MSRespThresholdTextBox.Text = MailServicesObject.ResponseThreshold.ToString();
                    MSRespThresholdTextBox.Text = ReturnObject.ResponseThreshold.ToString();
                    Session["ReturnUrl"] = "MailService.aspx?Key=" + ID+"&tab=1";
                    LocComboBox.Text = ReturnObject.LocationText;
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

        private MailServices CollectDataForMailServices()
        {
            try
            {
                MailServices MailServicesObject = new MailServices();
                MailServicesObject.Name = MSNameTextBox.Text;
                MailServicesObject.Address = AddressTextBox.Text;
                MailServicesObject.Category = ProtocolComboBox.Text;
                MailServicesObject.Description = DescriptionTextBox.Text;
                MailServicesObject.Enabled = EnableCheckBox.Checked;
                MailServicesObject.Port = short.Parse(PortTextBox.Text);
                MailServicesObject.ScanInterval = int.Parse(ScanIntervalTextBox.Text);
                MailServicesObject.OffHoursScanInterval = int.Parse(OffScanIntervalTextBox.Text);
                MailServicesObject.RetryInterval = int.Parse(RetryIntervalTextBox.Text);
                MailServicesObject.FailureThreshold = short.Parse(FailureTextBox.Text);
                MailServicesObject.ResponseThreshold = int.Parse(MSRespThresholdTextBox.Text);
                
                if (Mode == "Update")
                   // MailServicesObject.ID = ServerKey;
                    MailServicesObject.key = ServerKey;

                Locations LOCobject = new Locations();
                LOCobject.Location = LocComboBox.Text;
                Locations ReturnLocValue = VSWebBL.SecurityBL.LocationsBL.Ins.GetDataForLocation(LOCobject);
                MailServicesObject.LocationId = ReturnLocValue.ID;

                return MailServicesObject;
            }
            catch (Exception ex)
            {
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex; 
            }
            finally { }
        }


        private void UpdateMailServicesData()
        {
             MailServices mailObj = new MailServices();
            mailObj.Address = AddressTextBox.Text;
            mailObj.Name = MSNameTextBox.Text;
            mailObj.key = ServerKey;
            DataTable returntable = VSWebBL.ConfiguratorBL.MailServicesBL.Ins.GetIPAddress(mailObj);

            if (returntable.Rows.Count > 0)
            {
                //3/19/2014 NS modified
                //ErrorMessageLabel.Text = "This Mail Service Name/IP Address or host name is already being monitored.  Please enter a different IP Address/Name.";
                //ErrorMessagePopupControl.ShowOnPageLoad = true;
                errorDiv.Style.Value = "display: block";
                //10/3/2014 NS modified for VSPLUS-990
                errorDiv.InnerHtml = "The Mail Service Name/IP Address or host name you entered is already being monitored.  Please enter a different IP Address/Name."+
                    "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                flag = true;
            }
            else
            {
                try
                {
                    Object ReturnValue = VSWebBL.ConfiguratorBL.MailServicesBL.Ins.UpdateData(CollectDataForMailServices());
                    SetFocusOnError(ReturnValue);
                    if (ReturnValue.ToString() == "True")
                    {
                        //3/19/2014 NS modified to redirect to the mail services page
                        /*
                        ErrorMessageLabel.Text = "Mail service information updated successfully.";
                        ErrorMessagePopupControl.HeaderText = "Information";
                        ErrorMessagePopupControl.ShowCloseButton = false;
                        ValidationUpdatedButton.Visible = true;
                        ValidationOkButton.Visible = false;
                        ErrorMessagePopupControl.ShowOnPageLoad = true;
                        */
                        Session["MailServiceUpdateStatus"] = MSNameTextBox.Text;
                        Response.Redirect("MailServicesGrid.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
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
        private void InsertMailServices()
        {
            MailServices mailObj = new MailServices();
            mailObj.Address = AddressTextBox.Text;
            mailObj.Name = MSNameTextBox.Text;
            mailObj.key = ServerKey;
            DataTable returntable = VSWebBL.ConfiguratorBL.MailServicesBL.Ins.GetIPAddress(mailObj);
          
            if (returntable.Rows.Count > 0)
            {
                //3/19/2014 NS modified
                //ErrorMessageLabel.Text = "This Mail Service Name/IP Address or host name is already being monitored.  Please enter a different IP Address/Name.";
                //ErrorMessagePopupControl.ShowOnPageLoad = true;
                errorDiv.Style.Value = "display: block";
                //10/3/2014 NS modified for VSPLUS-990
                errorDiv.InnerHtml = "The Mail Service Name/IP Address or host name you entered is already being monitored.  Please enter a different IP Address/Name."+
                    "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                flag = true;
            }
            else
            {
                try
                {
                    object ReturnValue = VSWebBL.ConfiguratorBL.MailServicesBL.Ins.InsertData(CollectDataForMailServices());
                    SetFocusOnError(ReturnValue);
                    if (ReturnValue.ToString() == "True")
                    {
                        //3/19/2014 NS modified
                        /*
                        ErrorMessageLabel.Text = "New mail service created successfully.";
                        ErrorMessagePopupControl.HeaderText = "Information";
                        ErrorMessagePopupControl.ShowCloseButton = false;
                        ValidationUpdatedButton.Visible = true;
                        ValidationOkButton.Visible = false;
                        ErrorMessagePopupControl.ShowOnPageLoad = true;
                        */
                        Session["MailServiceUpdateStatus"] = MSNameTextBox.Text;
                        Response.Redirect("MailServicesGrid.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
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
        private void SetFocusOnError(Object ReturnValue)
        {
            string ErrorMessage = ReturnValue.ToString();
            if (ErrorMessage.Substring(0, 2) == "ER")
            {
                //3/19/2014 
                //ErrorMessageLabel.Text = ErrorMessage.Substring(3);
                //ErrorMessagePopupControl.ShowOnPageLoad = true;
                errorDiv.Style.Value = "display: block";
                //10/3/2014 NS modified for VSPLUS-990
                errorDiv.InnerHtml = ErrorMessage.Substring(3)+
                    "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";

            }
        }

        protected void formOKButton_Click(object sender, EventArgs e)
        {
            // Write to Registry
            VSWebBL.SettingBL.SettingsBL.Ins.UpdateSvalue("Mail Service Update", "True", VSWeb.Constants.Constants.SysString);


            try
            {
                if (Mode == "Update")
                {

                    UpdateMailServicesData();
                }
                if (Mode == "Insert")
                {
                    InsertMailServices();
                    if(flag==false)
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

        protected void ValidationUpdatedButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("MailServicesGrid.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
            Context.ApplicationInstance.CompleteRequest();
        }

        protected void ProtocolComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (ProtocolComboBox.Text)
            {
                case "SMTP":
                    PortTextBox.Enabled = true;
                    this.PortTextBox.Text = "25";
                    break;
                case "POP3":
                    this.PortTextBox.Enabled = true;
                    this.PortTextBox.Text = "110";
                    break;
                case "LDAP":
                    this.PortTextBox.Enabled = true;
                    this.PortTextBox.Text = "143";
                    break;
                case "Other":
                    this.PortTextBox.Enabled = true;                   
                    this.PortTextBox.Text = "";
                    break;
                case "IMAP":
                    this.PortTextBox.Enabled = true;
                    this.PortTextBox.Text = "389";
                    break;
            }

        }

        protected void MSNameTextBox_TextChanged(object sender, EventArgs e)
        {
           
            char Quote;
            Quote = (char)34;

            string myName = MSNameTextBox.Text;
            try
            {
                if (myName.IndexOf("'") > 0)
                    myName = myName.Replace("'", "");

                if (myName.IndexOf(Quote) > 0)
                    myName = myName.Replace(Quote, '~');

                if (myName.IndexOf(",") > 0)
                    myName = myName.Replace(",", " ");

                if (myName != MSNameTextBox.Text)
                    MSNameTextBox.Text = myName;

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
                    DescriptionTextBox.Text = myName;

            }
            catch (Exception ex)
            {
                // MessageBox.Show(ex.Message);
                ErrorMessageLabel.Text = ex.Message;
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
            }


        }

        protected void formCancelButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("MailServicesGrid.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
            Context.ApplicationInstance.CompleteRequest();
        }

        private Status CollectDataforStatus()
        {
            Status St = new Status();
            try
            {
                St.Category = ProtocolComboBox.Text;

                St.DeadMail = 0;
                St.Description = DescriptionTextBox.Text;
                // St.Details = "";
                St.DownCount = 0;
                //St.Location = "Cluster";
                St.Name = MSNameTextBox.Text;
                St.MailDetails = " ";
                St.PendingMail = 0;
                St.sStatus = "Not Scanned";
                St.Type = "Mail";
                St.Upcount = 0;
                St.UpPercent = 1;
                St.LastUpdate = System.DateTime.Now;
                St.ResponseTime = 0;
                St.TypeANDName = MSNameTextBox.Text + "-MS";
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
                    ErrorMessageLabel.Text = "New mail service created successfully.";
                    ErrorMessagePopupControl.HeaderText = "Information";
                    ErrorMessagePopupControl.ShowCloseButton = false;
                    ValidationUpdatedButton.Visible = true;
                    ValidationOkButton.Visible = false;
                    ErrorMessagePopupControl.ShowOnPageLoad = true;
                    */
                    Session["MailServiceUpdateStatus"] = MSNameTextBox.Text;
                    Response.Redirect("MailServicesGrid.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
                    Context.ApplicationInstance.CompleteRequest(); 
                }

            }
            catch (Exception ex)
            {
                //3/19/2014 NS modified
                //ErrorMessageLabel.Text = "Error attempting to update the status table :" + ex.Message;
                //ErrorMessagePopupControl.ShowOnPageLoad = true;
                errorDiv.Style.Value = "display: block";
                //10/3/2014 NS modified for VSPLUS-990
                errorDiv.InnerHtml = "Error attempting to update the status table: " + ex.Message+
                    "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
            }
            finally { }
        }

        private void FillMaintenanceGrid()
        {
            try
            {

                DataTable MaintDataTable = new DataTable();
                DataSet ServersDataSet = new DataSet();
                MaintDataTable = VSWebBL.ConfiguratorBL.MaintenanceBL.Ins.GetMaintenDataOnServerID(MSNameTextBox.Text);
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
                else
                {
                    FillMaintenanceGrid();
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

        private void FillAlertGridView()
        {
            try
            {

                DataTable AlertDataTable = new DataTable();
                DataSet AlertDataSet = new DataSet();
                AlertDataTable = VSWebBL.ConfiguratorBL.AlertsBL.Ins.GetAlertTab(MSNameTextBox.Text,"Mail");
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

        protected void MaintWinListGridView_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
        {
            e.Row.Attributes.Add("onmouseover", "this.style.backgroundColor='#C0C0C0';");

            e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='white';");
        }

        protected void MaintWinListGridView_PageSizeChanged(object sender, EventArgs e)
        {
            //ProfilesGridView.PageIndex;
            VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("MailService|MaintWinListGridView", MaintWinListGridView.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        }
        protected void AlertGridView_PageSizeChanged(object sender, EventArgs e)
        {
            //ProfilesGridView.PageIndex;
            VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("MailService|AlertGridView", AlertGridView.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        }
    }
}