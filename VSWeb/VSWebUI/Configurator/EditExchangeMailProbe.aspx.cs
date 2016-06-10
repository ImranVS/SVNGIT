using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;


using VSFramework;
using VSWebBL;
using VSWebDO;

using DevExpress.Web;

namespace VSWebUI
{
    public partial class EditExchangeMailProbe : System.Web.UI.Page
    {
        string ExchangeName;
        string MailID;
        string Mode;
        string savedfile;
        bool flag = false;
        // string UploadDirectory ;
        protected void Page_Load(object sender, EventArgs e)
        {

            if (Request.QueryString["tab"] != null)
            {
                ASPxPageControl1.ActiveTabIndex = Convert.ToInt32(Request.QueryString["tab"].ToString());
            }

            if (!IsPostBack)
            {

                FillSourceServerComboBox();
                if (Session["UserPreferences"] != null)
                {
                    DataTable UserPreferences = (DataTable)Session["UserPreferences"];
                    foreach (DataRow dr in UserPreferences.Rows)
                    {
                        if (dr[1].ToString() == "EditExchangeMailProbe|MaintWinListGridView")
                        {
                            MaintWinListGridView.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                        }
                        if (dr[1].ToString() == "EditExchangeMailProbe|AlertGridView")
                        {
                            AlertGridView.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                        }
                    }
                }
            }
            try
            {
                if (Request.QueryString["Name"] != null && Request.QueryString["Name"] != "")
                {
                    Mode = "Update";

                    //MailID = Request.QueryString["ExchangeMailAddress"];
                    ExchangeName = Request.QueryString["Name"];
                    //For Validation Summary
                    ////ApplyValidationSummarySettings();
                    ////ApplyEditorsSettings();
                    if (!IsPostBack)
                    {
                        //Fill Server Attributes Tab & Advanced Tab
                        FillData(ExchangeName);
                        FillMaintenanceGrid();
                        FillAlertGridView();
                        //10/21/2014 NS modified for VSPLUS-934
                        //ExchangeMailProbeRoundPanel.HeaderText = "Exchange Mail Probe -" + "  " + NameTextBox.Text;
                        lblServer.InnerHtml += " - " + NameTextBox.Text;

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

                        RetryTextBox.Text = "5";
                        ScanIntervalTextBox.Text = "8";
                        offScanTextBox.Text = "30";
                        DeliveryThresholdTextBox.Text = "5";
                        EnableCheckBox.Checked = true;
                        NameTextBox.Focus();
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
        //private void textEdit1_Spin(object sender, SpinEventArgs e)
        //{
        //    e.Handled = true;
        //}
        //private void tableView1_ShownEditor(object sender, EditorEventArgs e)
        //{
        //    TextEdit edit = e.Editor as TextEdit;
        //    edit.AllowSpinOnMouseWheel = false;
        //}

        //public partial class MainWindow : Window
        //{
        //    public MainWindow()
        //    {
        //        InitializeComponent();
        //        EventManager.RegisterClassHandler(typeof(TextEdit), TextEdit.SpinEvent, new SpinEventHandler(TextBox_Spin));
        //    }

        //    private void TextBox_Spin(object sender, DevExpress.Xpf.Editors.SpinEventArgs e)
        //    {
        //        e.Handled = true;
        //    }
        //}

        private void FillData(string Name)
        {
            try
            {

                ExchangeMailProbeClass ExchangeMailProbeObject = new ExchangeMailProbeClass();
                ExchangeMailProbeClass ReturnObject = new ExchangeMailProbeClass();
                //ExchangeMailProbeObject.ExchangeMailAddress = MailID;
                ExchangeMailProbeObject.Name = Name;
                ReturnObject = VSWebBL.ConfiguratorBL.ExchangeMailProbeBL.Ins.GetData(ExchangeMailProbeObject);
                //Cluster Setting fields
                NameTextBox.Text = ReturnObject.Name.ToString();
                CategoryComboBox.Text = ReturnObject.Category.ToString();
                ScanIntervalTextBox.Text = ReturnObject.ScanInterval.ToString();
                offScanTextBox.Text = ReturnObject.OffHoursScanInterval.ToString();
                EnableCheckBox.Checked = ReturnObject.Enabled;//(ScanTextBox.Text != null ? true : false);
                SourceServerComboBox.Text = ReturnObject.SourceServerID.ToString();
                SendToTextBox.Text = ReturnObject.ExchangeMailAddress;
                //FileNameTextBox.Text = ReturnObject.Filename;
                FilePathHyperLink.NavigateUrl = ReturnObject.Filename;
                FilePathHyperLink.Text = ReturnObject.Filename;
                //12/7/2012 NS modified
                //TargetComboBox.Text = ReturnObject.DestinationServerID.ToString();
                RetryTextBox.Text = ReturnObject.RetryInterval.ToString();
                DeliveryThresholdTextBox.Text = ReturnObject.DeliveryThreshold.ToString();
                Session["ReturnUrl"] = "EditExchangeMailProbe.aspx?Name=" + Name + "&tab=2";


            }
            catch (Exception ex)
            {
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
            finally { }
        }



        private ExchangeMailProbeClass CollectDataForExchangeMailProbe()
        {
            try
            {

                ExchangeMailProbeClass ExchangeMailProbeObject = new ExchangeMailProbeClass();
                ExchangeMailProbeObject.Name = NameTextBox.Text;
                ExchangeMailProbeObject.Enabled = EnableCheckBox.Checked;
                ExchangeMailProbeObject.Category = CategoryComboBox.Text;
                ExchangeMailProbeObject.ScanInterval = int.Parse(ScanIntervalTextBox.Text);
                ExchangeMailProbeObject.OffHoursScanInterval = int.Parse(offScanTextBox.Text);
                ExchangeMailProbeObject.DeliveryThreshold = int.Parse(DeliveryThresholdTextBox.Text);




                //string filename = FileUploadControl.UploadedFiles[0].FileName;
                //string filename1 = + '^' + filename;
                //string fpath = Server.MapPath("~/files/");
                //FileUploadControl.UploadedFiles[0].PostedFile.SaveAs(fpath + filename);
                // UploadDirectory = fpath + filename;
                FilePathHyperLink.Text = savedfile;
                FilePathHyperLink.NavigateUrl = savedfile;




                // ExchangeMailProbeObject.Filename = FileNameTextBox.Text;

                ExchangeMailProbeObject.Filename = FilePathHyperLink.NavigateUrl;//FileUploadControl.UploadedFiles[0].FileName.ToString();
                ExchangeMailProbeObject.ExchangeMailAddress = SendToTextBox.Text;
                ExchangeMailProbeObject.RetryInterval = int.Parse(RetryTextBox.Text);
                ExchangeMailProbeObject.SourceServer = SourceServerComboBox.Text;
				if (SourceServerComboBox.Value != null)
				{
					ExchangeMailProbeObject.SourceServerID = int.Parse(SourceServerComboBox.Value.ToString());
				}
				else
				{
					ExchangeMailProbeObject.SourceServerID = 0;
				}
				ASPxPageControl1.ActiveTabIndex = 0;
                return ExchangeMailProbeObject;
			
            }
            catch (Exception ex)
            {
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
            finally { }
        }

        private void UpdateExchangeMailProbeData()
        {
            ExchangeMailProbeClass mailObj = new ExchangeMailProbeClass();
            mailObj.ExchangeMailAddress = MailID;
            mailObj.Name = NameTextBox.Text;
            DataTable returntable = VSWebBL.ConfiguratorBL.ExchangeMailProbeBL.Ins.GetMailAddress(mailObj, ExchangeName);

            if (returntable.Rows.Count > 0)
            {
                ErrorMessageLabel.Text = "There is already a probe being mailed for this name.  Please enter another probe name.";
                ErrorMessagePopupControl.ShowOnPageLoad = true;
                flag = true;
            }
            else
            {
                try
                {
                    Object ReturnValue = VSWebBL.ConfiguratorBL.ExchangeMailProbeBL.Ins.UpdateData(CollectDataForExchangeMailProbe(), ExchangeName);
                    //SetFocusOnError(ReturnValue);
                    if (ReturnValue.ToString() == "True")
                    {
                        //2/20/2014 NS modified to redirect to the server grid page
                        Session["ExchangeMailProbeUpdateStatus"] = NameTextBox.Text;
                        Response.Redirect("ExchangeMailprobe.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
                        Context.ApplicationInstance.CompleteRequest();
                        /*
                        ErrorMessageLabel.Text = "Mail probe information update completed successfully.";
                        ErrorMessagePopupControl.HeaderText = "Information";
                        ErrorMessagePopupControl.ShowCloseButton = false;
                        ValidationUpdatedButton.Visible = true;
                        ValidationOkButton.Visible = false;
                        ErrorMessagePopupControl.ShowOnPageLoad = true;
                        */
                    }
					   
					else if (ReturnValue.ToString().Equals("false"))
					{
						flag = true;
						
						errorDiv.InnerHtml = "The following error has occurred: could not insert a record into the SQL table." +
							"<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
						errorDiv.Style.Value = "display: block";
						
						Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Error while trying to insert to SQL.");

					}
					else if (!string.IsNullOrEmpty(ReturnValue.ToString()))
						{
							flag = true;
							errorDiv.InnerHtml = ReturnValue.ToString() +
								"<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
							errorDiv.Style.Value = "display: block";
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

        private void InsertExchangeMailProbe()
        {

            //This is called if defining a new Mail Probe
            //See if this name is already used

            ExchangeMailProbeClass mailObj = new ExchangeMailProbeClass();
            //7/26/2013 NS modified
            //If it's a new probe, set MailId to the value of SendToTextBox
            if (MailID == null || MailID == "")
            {
                MailID = SendToTextBox.Text;
            }
            mailObj.ExchangeMailAddress = MailID;
            mailObj.Name = NameTextBox.Text;
            DataTable returntable = VSWebBL.ConfiguratorBL.ExchangeMailProbeBL.Ins.GetMailAddress(mailObj, "");

            if (returntable.Rows.Count > 0)
            {
                ErrorMessageLabel.Text = "There is already a probe being mailed for this name.  Please enter a different probe name.";
                ErrorMessagePopupControl.ShowOnPageLoad = true;
                flag = true;
            }
            else
            {
                try
                {
                    object ReturnValue = VSWebBL.ConfiguratorBL.ExchangeMailProbeBL.Ins.InsertData(CollectDataForExchangeMailProbe());
                   // SetFocusOnError(ReturnValue);
                    if (ReturnValue.ToString().Equals("True"))
                    {
                        /*
                        ErrorMessageLabel.Text = "New mail probe created successfully.";
                        ErrorMessagePopupControl.HeaderText = "Information";
                        ErrorMessagePopupControl.ShowCloseButton = false;
                        ValidationUpdatedButton.Visible = true;
                        ValidationOkButton.Visible = false;
                        ErrorMessagePopupControl.ShowOnPageLoad = true;
                        */
                      
						Session["ExchangeMailProbeUpdateStatus"] = NameTextBox.Text;
						Response.Redirect("ExchangeMailprobe.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
						Context.ApplicationInstance.CompleteRequest();
                    }
					else if (ReturnValue.ToString().Equals("false"))
					{
						flag = true;
						
						errorDiv.InnerHtml = "The following error has occurred: could not insert a record into the SQL table." +
							"<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
						errorDiv.Style.Value = "display: block";
						
						Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Error while trying to insert to SQL.");

					}
					else if (!string.IsNullOrEmpty(ReturnValue.ToString()))
						{
							flag = true;
							errorDiv.InnerHtml = ReturnValue.ToString() +
								"<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
							errorDiv.Style.Value = "display: block";
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
                ErrorMessageLabel.Text = ErrorMessage.Substring(3);
                ErrorMessagePopupControl.ShowOnPageLoad = true;
                flag = true;
            }
        }

        protected void ValidationUpdatedButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("ExchangeMailProbe.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
            Context.ApplicationInstance.CompleteRequest();
        }

        protected void formOKButton_Click(object sender, EventArgs e)
        {
            // write to  Registry...
            try
            {
                VSWebBL.SettingBL.SettingsBL.Ins.UpdateSvalue("ExchangeMail Probe Update", "True", VSWeb.Constants.Constants.SysString);
            }
            catch (Exception ex)
            {
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }



            // file Upload Code

            try
            {
                if (FileUploadControl.UploadedFiles[0].FileName != null && FileUploadControl.UploadedFiles[0].FileName != "")
                {
                    string uploadFolder = Server.MapPath("~/files/");
                    string fileName = FileUploadControl.UploadedFiles[0].FileName;
                    FileUploadControl.UploadedFiles[0].SaveAs(uploadFolder + fileName);
                    savedfile = "~/files/" + fileName;
                }
                else if (Mode == "Update")
                {
                    savedfile = FilePathHyperLink.Text;

                }
            }
            catch (Exception ex)
            {
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }


            // insert or Update
            try
            {
                if (Mode == "Update")
                {

                    UpdateExchangeMailProbeData();
                }
                if (Mode == "Insert")
                {

                    InsertExchangeMailProbe();
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

        protected void formCancelButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("ExchangeMailProbe.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
            Context.ApplicationInstance.CompleteRequest();
        }

        private void FillSourceServerComboBox()
        {
            DataTable DSTasksDataTable = RestrServers();
            SourceServerComboBox.DataSource = DSTasksDataTable;
            SourceServerComboBox.TextField = "ServerName";
            SourceServerComboBox.ValueField = "ID";
            SourceServerComboBox.DataBind();
        }


        public DataTable RestrServers()
        {
            DataTable DCTasksDataTable = VSWebBL.ConfiguratorBL.ExchangeMailProbeBL.Ins.GetServername();
            if (Session["RestrictedServers"] != "" && Session["RestrictedServers"] != null && DCTasksDataTable.Rows.Count > 0)
            {

                List<int> ServerID = new List<int>();
                List<int> LocationID = new List<int>();
                DataTable resServers = (DataTable)Session["RestrictedServers"];
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
                            //LocationID.Add(DCTasksDataTable.Rows.IndexOf(dominorow));
                        }
                    }

                }
                foreach (int Id in ServerID)
                {
                    DCTasksDataTable.Rows[Id].Delete();
                }
                DCTasksDataTable.AcceptChanges();
                //foreach (int Lid in LocationID)
                //{
                //    DCTasksDataTable.Rows[Lid].Delete();
                //}
                foreach (int lid in LocationID)
                {
                    DataRow[] row = DCTasksDataTable.Select("LocationID=" + lid + "");
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

        private Status CollectDataforStatus()
        {
            Status St = new Status();
            try
            {
                St.Category = CategoryComboBox.Text;

                //St.DeadMail = 0;
                St.Description = "Sends ExchangeMail to target address and verifies delivery.";
                // St.Details = "";
                St.DownCount = 0;
                //St.Location = "Cluster";
                St.Name = NameTextBox.Text;
                // St.MailDetails = " ";
                // St.PendingMail = 0;
                St.sStatus = "Not Scanned";
                St.Type = "ExchangeMail Probe";
                St.Upcount = 0;
                St.UpPercent = 1;
                St.LastUpdate = System.DateTime.Now;
                St.ResponseTime = 0;
                St.TypeANDName = NameTextBox.Text + "-NMP";
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
                    /*
                    ErrorMessageLabel.Text = "New information posted successfully.";
                    ErrorMessagePopupControl.HeaderText = "Information";
                    ErrorMessagePopupControl.ShowCloseButton = false;
                    ValidationUpdatedButton.Visible = true;
                    ValidationOkButton.Visible = false;
                    ErrorMessagePopupControl.ShowOnPageLoad = true;
                     */
                    //2/20/2014 NS modified to redirect to the server grid page
                    Session["ExchangeMailProbeUpdateStatus"] = NameTextBox.Text;
                    Response.Redirect("ExchangeMailprobe.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
                    Context.ApplicationInstance.CompleteRequest();
                }

            }
            catch (Exception ex)
            {
                //2/20/2014 NS modified
                //ErrorMessageLabel.Text = "Error attempting to update the status table: " + ex.Message;
                //ErrorMessagePopupControl.ShowOnPageLoad = true;
                errorDiv.Style.Value = "display: block";
                //10/3/2014 NS modified for VSPLUS-990
                errorDiv.InnerHtml = "The following error has occurred: " + ex.Message+
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
                MaintDataTable = VSWebBL.ConfiguratorBL.MaintenanceBL.Ins.GetMaintenDataOnServerID(SourceServerComboBox.Text);
                if (MaintDataTable.Rows.Count > 0)
                {
                    DataTable dtcopy = MaintDataTable.Copy();
                    dtcopy.PrimaryKey = new DataColumn[] { dtcopy.Columns["ID"] };
                    Session["MaintServers"] = dtcopy;
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
                if (Session["MaintServers"] != "" && Session["MaintServers"] != null)
                    ServersDataTable = (DataTable)Session["MaintServers"];
                if (ServersDataTable.Rows.Count > 0)
                {
                    MaintWinListGridView.DataSource = ServersDataTable;
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

        private void FillAlertGridView()
        {
            try
            {

                DataTable AlertDataTable = new DataTable();
                DataSet AlertDataSet = new DataSet();
                AlertDataTable = VSWebBL.ConfiguratorBL.AlertsBL.Ins.GetAlertTab(SourceServerComboBox.Text, "ExchangeMail Probe");
                if (AlertDataTable.Rows.Count > 0)
                {
                    DataTable dtcopy = AlertDataTable.Copy();
                    // dtcopy.PrimaryKey = new DataColumn[] { dtcopy.Columns["ID"] };
                    Session["AlertDataTable"] = dtcopy;
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
                if (Session["AlertDataTable"] != "" && Session["AlertDataTable"] != null)
                    AlertDataTable = (DataTable)Session["AlertDataTable"];//VSWebBL.ConfiguratorBL.DominoPropertiesBL.Ins.GetAllData();
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

        protected void AlertGridView_PageSizeChanged(object sender, EventArgs e)
        {
            //ProfilesGridView.PageIndex;
            VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("EditExchangeMailProbe|AlertGridView", AlertGridView.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        }
        protected void MaintWinListGridView_PageSizeChanged(object sender, EventArgs e)
        {
            //ProfilesGridView.PageIndex;
            VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("EditExchangeMailProbe|MaintWinListGridView", MaintWinListGridView.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        }
    }


}



