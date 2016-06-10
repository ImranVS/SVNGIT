using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;


namespace VSWebUI
{
    public partial class WebForm18 : System.Web.UI.Page
    {
        string Mode;
        string Key;
        string hidevalue;
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Title = "BlackBerry Device Probe Properties";
            if (Request.QueryString["tab"] != null)
            {
                ASPxPageControl1.ActiveTabIndex = Convert.ToInt32(Request.QueryString["tab"].ToString());
            }
            fillTargetserverComboBox();
            if (Request.QueryString["NotesMailAddress"] != "" && Request.QueryString["NotesMailAddress"] != null)
            {
                Mode = "Update";
                Key = Request.QueryString["NotesMailAddress"];
                if (!IsPostBack)
                {
                   
                    filldata(Key);
                    FillMaintenanceGrid();
                    FillAlertGridView();
                    BBRoundPanel.HeaderText = "BlackBerry Device Probe - " + NameTextBox.Text;

                    if (Session["UserPreferences"] != null)
                    {
                        DataTable UserPreferences = (DataTable)Session["UserPreferences"];
                        foreach (DataRow dr in UserPreferences.Rows)
                        {
                            if (dr[1].ToString() == "BlackBerryDeviceProbe|MaintWinListGridView")
                            {
                                MaintWinListGridView.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                            }
                            if (dr[1].ToString() == "BlackBerryDeviceProbe|AlertGridView")
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
                   ScanIntervalTextBox.Text = "8";
                   RetryIntervalTextBox.Text = "5";
                   offHoursscanIntervalTextBox.Text = "30";
                   DeliveryThresholdTextBox.Text = "5";
                   EnabledforScanningCheckBox.Checked = true;
                   //TargetserverComboBox.Text = "select";
                   //confserverComboBox.Text = "Select";
                   SourceComboBox.Text = "Not Used";
               }
            }
         }
        

        public BlackBerry getdata()
        {
            BlackBerry BlackBerryObject = new BlackBerry();
            BlackBerryObject.Enabled = Convert.ToBoolean(EnabledforScanningCheckBox.Checked);
            BlackBerryObject.Name = NameTextBox.Text;
            BlackBerryObject.Category = CategoryTextBox.Text;
            BlackBerryObject.OffHoursScanInterval = Convert.ToInt32(offHoursscanIntervalTextBox.Text);
            BlackBerryObject.RetryInterval = Convert.ToInt32(RetryIntervalTextBox.Text);
            BlackBerryObject.ScanInterval = Convert.ToInt32(ScanIntervalTextBox.Text);
            BlackBerryObject.DeliveryThreshold = Convert.ToInt32(DeliveryThresholdTextBox.Text);
          //BlackBerryObject.DestinationDatabase=
            BlackBerryObject.NotesMailAddress = DevicesNotesMailAddressTextBox.Text;
          //BlackBerryObject.InternetMailAddress=DevicesInternetMailAddressTextBox.Text;
            if (TargetserverComboBox.Value.ToString() != "")
            BlackBerryObject.DestinationServerID = int.Parse(TargetserverComboBox.Value.ToString());
            BlackBerryObject.DestinationDatabase = LookfortheMessageDatabaseTextBox.Text;
            BlackBerryObject.ConfirmationDatabase = LookfortheDatabaseTextBox.Text;
          //BlackBerryObject.ConfirmationServer = LookfortheserverTextBox.Text;
            BlackBerryObject.InternetMailAddress = InternetMailAddress.Text;
            BlackBerryObject.SourceServer = SourceComboBox.Text;
            if (confserverComboBox.Value.ToString() != "")
            BlackBerryObject.ConfirmationServerID =int.Parse( confserverComboBox.Value.ToString());
             


                      
            if (Mode == "Update")
            {
              //  BlackBerryObject.NotesMailAddress = Key;
                NotesMailAddressID.Value = Key;
                hidevalue = NotesMailAddressID.Value;
                
            }
           
            return BlackBerryObject;
         }

        public void fillTargetserverComboBox()
        {
            DataTable dt = new DataTable();
            dt = RestrServers();
            TargetserverComboBox.DataSource = dt;
           // TargetserverComboBox.Text = "select";
            TargetserverComboBox.TextField = "Name";
            TargetserverComboBox.ValueField = "ID";
            TargetserverComboBox.DataBind();

            
         //   TargetserverComboBox.SelectedIndex = 1;
            confserverComboBox.DataSource = dt;
            
            confserverComboBox.TextField = "Name";
            confserverComboBox.ValueField = "ID";
            confserverComboBox.DataBind();
           // confserverComboBox.SelectedIndex = 1;
            SourceComboBox.DataSource = dt;
            SourceComboBox.TextField = "Name";
            SourceComboBox.ValueField = "ID";
            SourceComboBox.DataBind();
           // SourceComboBox.SelectedIndex = 1;
        }


        public DataTable RestrServers()
        {
            DataTable DCTasksDataTable = new DataTable();
            DCTasksDataTable = VSWebBL.ConfiguratorBL.BlackBerryBL.Ins.getdatadominoserver();
            //4/3/2014 NS modified for VSPLUS-138
            //if (Session["RestrictedServers"] != "" && Session["RestrictedServers"] != null)
            if (ViewState["RestrictedServers"] != "" && ViewState["RestrictedServers"] != null)
            {

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
                            //LocationID.Add(DSTaskSettingsDataTable.Rows.IndexOf(dominorow));
                        }
                    }

                }
                foreach (int Id in ServerID)
                {
                    DCTasksDataTable.Rows[Id].Delete();
                }
                DCTasksDataTable.AcceptChanges();
                foreach (int lid in LocationID)
                {
                    DataRow[] row = DCTasksDataTable.Select("locationid=" + lid + "");
                    for (int i = 0; i < row.Count(); i++)
                    {
                        DCTasksDataTable.Rows.Remove(row[i]);
                        DCTasksDataTable.AcceptChanges();
                    }
                }
                DCTasksDataTable.AcceptChanges();

            }
            return DCTasksDataTable;
        }

        public void filldata( string  id)
        {
            BlackBerry BlackBerryObject = new BlackBerry();
            DataTable dt = new DataTable();
            BlackBerryObject.NotesMailAddress = Key;
            dt = VSWebBL.ConfiguratorBL.BlackBerryBL.Ins.gettable(BlackBerryObject);
            NameTextBox.Text = dt.Rows[0]["Name"].ToString();
            EnabledforScanningCheckBox.Checked = Convert.ToBoolean(dt.Rows[0]["Enabled"]);
            CategoryTextBox.Text = dt.Rows[0]["Category"].ToString();
            ScanIntervalTextBox.Text = dt.Rows[0]["ScanInterval"].ToString();
            offHoursscanIntervalTextBox.Text = dt.Rows[0]["OffHoursScanInterval"].ToString();
            RetryIntervalTextBox.Text = dt.Rows[0]["RetryInterval"].ToString();
        //  DevicesInternetMailAddressTextBox.Text = dt.Rows[0]["InternetMailAddress"].ToString();
            InternetMailAddress.Text = dt.Rows[0]["InternetMailAddress"].ToString();
            confserverComboBox.Text = dt.Rows[0]["ConfirmationServer"].ToString();
            LookfortheDatabaseTextBox.Text = dt.Rows[0]["ConfirmationDatabase"].ToString();
            LookfortheMessageDatabaseTextBox.Text = dt.Rows[0]["DestinationDatabase"].ToString();
            TargetserverComboBox.Text = dt.Rows[0]["DestinationServer"].ToString();
            DeliveryThresholdTextBox.Text = dt.Rows[0]["DeliveryThreshold"].ToString();
            DevicesNotesMailAddressTextBox.Text = dt.Rows[0]["NotesMailAddress"].ToString();
            SourceComboBox.Text = dt.Rows[0]["SourceServer"].ToString();

            Session["ReturnUrl"] = "BlackBerryDeviceProbe.aspx?NotesMailAddress=" + id+"&tab=1";
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

        public void updateBlackBerry()
        {
               try
            {
                BlackBerry bbobj = new BlackBerry();
                bbobj.NotesMailAddress = Key;
                bbobj.Name = NameTextBox.Text;
                DataTable dt = VSWebBL.ConfiguratorBL.BlackBerryBL.Ins.GetDatabyName(bbobj,"");
                if (dt.Rows.Count > 0)
                {
                    //3/19/2014 NS modified
                    //ErrorMessagePopupControl.ShowOnPageLoad = true;
                    //ErrorMessageLabel.Text = "The name of the probe you entered already exisits. Please enter another name.";
                    //10/3/2014 NS modified for VSPLUS-990
                    errorDiv.InnerHtml = "The name of the probe you entered already exisits. Please enter another name."+
                        "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                    errorDiv.Style.Value = "display: block";
                }
                else
                {
                    Object update = VSWebBL.ConfiguratorBL.BlackBerryBL.Ins.updateBlackBerryDevice(getdata(), hidevalue);
                    SetFocusOnError(update);
                    if (update.ToString() == "True")
                    {
                        //3/19/2014 NS modified
                        /*
                        ErrorMessageLabel.Text = "Data updated successfully.";
                        ErrorMessagePopupControl.HeaderText = "Information";
                        ErrorMessagePopupControl.ShowCloseButton = false;
                        ValidationUpdatedButton.Visible = true;
                        ValidationOkButton.Visible = false;
                        ErrorMessagePopupControl.ShowOnPageLoad = true;
                        */
                        Session["BlackberryDeviceUpdateStatus"] = NameTextBox.Text;
                        Response.Redirect("BlackBerryDeviceProbesgrid.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
                        Context.ApplicationInstance.CompleteRequest();
                    }
                }
                      }
            catch (Exception ex)
            {
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw;
            }
         
        }
        public void check()
        {
            DataTable d = new DataTable();
          string name = NameTextBox.Text;
          string internetmail = DevicesNotesMailAddressTextBox.Text;
          d = VSWebBL.ConfiguratorBL.BlackBerryBL.Ins.checkname(name,internetmail);
          if (d.Rows.Count > 0)
          {
              //3/19/2014 NS modified
              //ErrorMessageLabel.Text = "BlackBerryname is already exists enter another name or NotesMailAddress";
              //ErrorMessagePopupControl.ShowCloseButton = false;
              //ErrorMessagePopupControl.ShowOnPageLoad = true;
              //10/3/2014 NS modified for VSPLUS-990
              errorDiv.InnerHtml = "Blackberry name already exists. Please enter another name."+
                  "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
              errorDiv.Style.Value = "display: block";
          }
        }

        public Status getdataforstatustable()
        {
            Status statusObject = new Status();
            statusObject.Category = CategoryTextBox.Text;
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

        public void  insertstatus()
       {
           Object retval;
           retval = VSWebBL.StatusBL.StatusTBL.Ins.InsertData(getdataforstatustable());
       }

        //public Settings update()
        //{
        //    Settings settingobject = new Settings();
        //    //name = "BlackBerry";
        //    //value = "True";
        //    settingobject.sname = "BlackBerry";
        //    settingobject.svalue = "True";
        //    return settingobject;
        //}

        public void updatesetting()
        {
            string  name = "BlackBerry";
            string  value = "True";
            Object returnval;
            returnval = VSWebBL.SettingBL.SettingsBL.Ins.UpdateSvalue(name, value, VSWeb.Constants.Constants.SysString);
        }



        public void insertdetailsBlackBerryDevice()
        {
            try
            {
                BlackBerry bbobj = new BlackBerry();
                bbobj.NotesMailAddress = "";
                bbobj.Name = NameTextBox.Text;
                string mail = DevicesNotesMailAddressTextBox.Text;
                DataTable dt = VSWebBL.ConfiguratorBL.BlackBerryBL.Ins.GetDatabyName(bbobj,mail);
                if (dt.Rows.Count > 0)
                {
                    //3/19/2014 NS modified
                    //ErrorMessagePopupControl.ShowOnPageLoad = true;
                    //ErrorMessageLabel.Text = "Name/NotesmailAddress is alredy Exisited. Please enter another Name/Mailaddress";
                    //10/3/2014 NS modified for VSPLUS-990
                    errorDiv.InnerHtml = "Name/Notes Mail Address already exists. Please enter another Name/Notes Mail Address."+
                        "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                    errorDiv.Style.Value = "display: block";
                }
                else
                {
                    
                    Object insert;
                    insert = VSWebBL.ConfiguratorBL.BlackBerryBL.Ins.insertBlackBerryDeviceProdbegrid(getdata());
                    SetFocusOnError(insert);
                    if (insert.ToString() == "True")
                    {
                        //3/19/2014 NS modified
                        /*
                        ErrorMessageLabel.Text = "Data inserted successfully.";
                        ErrorMessagePopupControl.HeaderText = "Information";
                        ErrorMessagePopupControl.ShowCloseButton = false;
                        ValidationOkButton.Visible = false;
                        ValidationUpdatedButton.Visible = true;
                        ErrorMessagePopupControl.ShowOnPageLoad = true;
                         */
                        Session["BlackberryDeviceUpdateStatus"] = NameTextBox.Text;
                        Response.Redirect("BlackBerryDeviceProbesgrid.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
                        Context.ApplicationInstance.CompleteRequest();
                    }
                }
            }
            catch (Exception ex)
            {
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw;
            }
         }

        protected void FormCancelButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("BlackBerryDeviceProbesgrid.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
            Context.ApplicationInstance.CompleteRequest();
        }

        protected void FormOkButton_Click(object sender, EventArgs e)
        {
            if (Mode == "Update")
            {
                //check();
                if (ErrorMessageLabel.Text =="")
                {
                    updateBlackBerry();
                }
               // insertstatus();
            }
            if (Mode == "Insert")
            {
               // check();
                if (ErrorMessageLabel.Text == "")
                {
                    insertdetailsBlackBerryDevice();
                    insertstatus();
                }
               
            }
        }

        //protected void TargetserverComboBox_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    TargetserverComboBox.SelectedItem.Text = ViewState["targetserver"].ToString();
        //}

        private void FillMaintenanceGrid()
        {
            try
            {

                DataTable MaintDataTable = new DataTable();
                DataSet ServersDataSet = new DataSet();
                MaintDataTable = VSWebBL.ConfiguratorBL.MaintenanceBL.Ins.GetMaintenDataOnServerIDs(SourceComboBox.Text,TargetserverComboBox.Text,confserverComboBox.Text );
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
                AlertDataTable = VSWebBL.ConfiguratorBL.AlertsBL.Ins.GetAlertTabbyDiffServers(SourceComboBox.Text, TargetserverComboBox.Text, confserverComboBox.Text,"BES");
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

        protected void ASPxPageControl1_ActiveTabChanged(object source, DevExpress.Web.TabControlEventArgs e)
        {

        }

        protected void ValidationUpdatedButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("BlackBerryDeviceProbesgrid.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
            Context.ApplicationInstance.CompleteRequest();
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

        protected void MaintWinListGridView_HtmlRowCreated(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
        {
            e.Row.Attributes.Add("onmouseover", "this.style.backgroundColor='#C0C0C0';");

            e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='white';");
        }

        protected void MaintWinListGridView_PageSizeChanged(object sender, EventArgs e)
        {
            VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("BlackBerryDeviceProbe|MaintWinListGridView", MaintWinListGridView.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        }
        protected void AlertGridView_PageSizeChanged(object sender, EventArgs e)
        {
            VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("BlackBerryDeviceProbe|AlertGridView", AlertGridView.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        }
    }
}