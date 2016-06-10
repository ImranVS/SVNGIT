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
    public partial class DominoProperties : System.Web.UI.Page
    {

        #region "Declarations"
        /// <summary>
        /// Declarations
        /// </summary>

        //5/1/2014 NS added for VSPLUS-427
        bool isValid = true;
        string MyDominoPassword;
        string val;
		VSFramework.TripleDES encryptkey = new VSFramework.TripleDES();
        // the following commented code shown error at Run time 
        //Domino.NotesSession s = new Domino.NotesSession();
        //Domino.NotesDatabase db = default(Domino.NotesDatabase);
        //Domino.NotesDocument doc = default(Domino.NotesDocument);

        Domino.NotesSession NotesSessionObject = null; //new Domino.NotesSession();

        DataSet STSettingsDataSet = null;
        DataSet MaintWindowsDataSet = null;
        int ServerKey;

        Hashtable copiedValues = null;
        string[] copiedFields = new string[] { "Enabled", "TaskName", "SendLoadCommand", "SendRestartCommand", "RestartOffHours", "SendExitCommand" };
        #endregion

        #region "Page Control Events"

        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {

            try
			{
				
				ServerKey = int.Parse(Request.QueryString["Key"]);
				Session["Key"] = ServerKey;
				//ServerKey = int.Parse(Session["key"].ToString());
                if (Request.QueryString["tab"] != null)
                {
                    ASPxPageControl1.ActiveTabIndex = Convert.ToInt32(Request.QueryString["tab"].ToString());
                }
                //10/13/2014 NS added
                ErrorMessagePopupControl.ShowOnPageLoad = false;
                //For Validation Summary
                ////ApplyValidationSummarySettings();
                ////ApplyEditorsSettings();
                if (ServerKey > 0)
                {
                    if (!IsPostBack)
                    {

                        //Fill Server Attributes Tab & Advanced Tab
                        //8/7/2014 NS added
                        FillCredentialsComboBox();
                        FillData(ServerKey);
                        //11/18/2014 NS modified
                        //DominoRoundPanel.HeaderText = "Domino Properties -" + " " + SrvAtrSrvNameComboBox.Text;
                        servernamelbldisp.InnerHtml = "Domino Properties -" + " " + SrvAtrSrvNameComboBox.Text;
                        //Fill Server Tasks Tab Grid
                        FillDSTaskSettingsUpdateGrid(ServerKey.ToString());
                        //Fill Maintenance Windows Tab
                        //FillMaintenanceWindowsUpdateGrid(SrvAtrSrvNameComboBox.Text);

                        FillMaintenanceGrid();
                        FillAlertGridView();

                        FillDiskGridView();

                        if (Session["UserPreferences"] != null)
                        {
                            DataTable UserPreferences = (DataTable)Session["UserPreferences"];
                            foreach (DataRow dr in UserPreferences.Rows)
                            {
                                if (dr[1].ToString() == "DominoProperties|SrvTskDSTaskSettingsGridView")
                                {
                                    SrvTskDSTaskSettingsGridView.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                                }
                                if (dr[1].ToString() == "DominoProperties|DiskGridView")
                                {
                                    DiskGridView.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                                }
                                if (dr[1].ToString() == "DominoProperties|AlertGridView")
                                {
                                    AlertGridView.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                                }
                                if (dr[1].ToString() == "DominoProperties|MaintWinListGridView")
                                {
                                    MaintWinListGridView.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                                }
                            }
                        }
                        //6/17/2015 NS added for VSPLUS-1802
                        bool exjournalenabled = false;
                        try
                        {
                            exjournalenabled = Convert.ToBoolean(VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("Enable ExJournal"));
                        }
                        catch (Exception ex)
                        {
                            Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                            throw;
                        }
                        if (exjournalenabled)
                        {
                            ASPxRoundPanel2.Visible = true;
                        }
                    }
                    else
                    {
                        ASPxCheckBox SrvTskGrdDisallowCheckBox = (ASPxCheckBox)SrvTskDSTaskSettingsGridView.FindEditFormTemplateControl("SrvTskGrdDisallowCheckBox");

                        if (SrvTskGrdDisallowCheckBox == null)
                        {
                            FillDSTaskSettingsUpdateGridfromsession(ServerKey.ToString());
                        }

                        //fill data from session

                        //FillMaintenanceWindowsUpdateGridfromSession(SrvAtrSrvNameComboBox.Text);
                        FillMaintServersGridfromSession();
                        FillAlertGridViewfromSession();
                        FillCalanderdatafromSession();
                        //12/17/2013 NS added
                        FillDiskGridfromSession();
                        //5/12/2014 NS added for VSPLUS-615
                        if (SelCriteriaRadioButtonList.SelectedItem.Value.ToString() != "0")
                        {
                            AdvDiskSpaceThTrackBar.Visible = false;
                        }
                        //10/19/2015 NS added for VSPLUS-2279
                        ShowHideLookBack();
                    }

                }
            }
            catch (Exception ex)
            {
                //11/18/2014 NS modified
                //DominoRoundPanel.Visible = false;
                ASPxPageControl1.Visible = false;
                ErrorLabel.Visible = true;
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
            }
            finally { }
        }

        private void FillCredentialsComboBox()
        {
            try
            {
                DataTable CredentialsDataTable = VSWebBL.ConfiguratorBL.ServicesBL.Ins.GetCredentials();
                CredentialsComboBox.DataSource = CredentialsDataTable;
				
                CredentialsComboBox.TextField = "AliasName";
                CredentialsComboBox.ValueField = "ID";
			
                CredentialsComboBox.DataBind();
				CredentialsComboBox.Items.Insert(0, new ListEditItem("None", "-1"));
				
            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
        }

        private void FillDiskGridView()
        {
            try
            {

                DataTable DiskDataTable = new DataTable();

                DiskDataTable = VSWebBL.ConfiguratorBL.DominoPropertiesBL.Ins.GetRowsDiskSettings(SrvAtrSrvNameComboBox.Text);
                if (DiskDataTable.Rows.Count == 0 || (DiskDataTable.Rows.Count == 1 && DiskDataTable.Rows[0]["DiskName"].ToString() == "NoAlerts"))
                {
                    //12/16/2013 NS modified - created a radio button list
                    //5/12/2014 NS modified for VSPLUS-615
                    //SelCriteriaRadioButtonList.SelectedIndex = 2;
                    SelCriteriaRadioButtonList.SelectedIndex = 3;
                    AdvDiskSpaceThTrackBar.Visible = false;
                    DiskLabel.Visible = false;
                    Label4.Visible = false;
                    DiskGridView.Visible = false;
                    DiskGridInfo.Visible = false;
                    //rdbSelAll.Checked = false;
                    //rdbSelFew.Checked = false;
                    //rdbNoAlerts.Checked = true;
                    DiskDataTable = VSWebBL.ConfiguratorBL.DominoPropertiesBL.Ins.GetDiskSettings(SrvAtrSrvNameComboBox.Text, "");
                    //12/16/2013 NS added
                    //SelectDisksRoundPanel.Visible = false;
                    //SelectDisksRoundPanel.Enabled = false;
                    //1/31/2014 NS added for VSPLUS-289
                    infoDiskDiv.Style.Value = "display: none";
                    //5/12/2014 NS added for VSPLUS-615
                    GBTextBox.Visible = false;
                    GBLabel.Visible = false;
                    GBTitle.Visible = false;
                }
                //5/12/2014 NS modified for VSPLUS-615
                //else if (DiskDataTable.Rows.Count == 1 && DiskDataTable.Rows[0]["DiskName"].ToString() == "0")
                else if (DiskDataTable.Rows.Count == 1 && DiskDataTable.Rows[0]["DiskName"].ToString() == "AllDisks" &&
                    DiskDataTable.Rows[0]["ThresholdType"].ToString() == "Percent")
                {
                    //12/16/2013 NS modified - created a radio button list
                    SelCriteriaRadioButtonList.SelectedIndex = 0;
                    AdvDiskSpaceThTrackBar.Visible = true;
                    AdvDiskSpaceThTrackBar.Value = DiskDataTable.Rows[0]["Threshold"];
                    DiskLabel.Text = "Current threshold: " + DiskDataTable.Rows[0]["Threshold"].ToString() + "% free space";
                    DiskLabel.Visible = true;
                    Label4.Visible = true;
                    DiskGridView.Visible = false;
                    DiskGridInfo.Visible = false;
                    //rdbSelAll.Checked = true;
                    //rdbSelFew.Checked = false;
                    //rdbNoAlerts.Checked = false;

                    //12/16/2013 NS added
                    //SelectDisksRoundPanel.Visible = false;
                    //SelectDisksRoundPanel.Enabled = false;
                    //1/31/2014 NS added for VSPLUS-289
                    infoDiskDiv.Style.Value = "display: none";
                    //5/12/2014 NS added for VSPLUS-615
                    GBTextBox.Visible = false;
                    GBLabel.Visible = false;
                    GBTitle.Visible = false;
                    DiskDataTable = VSWebBL.ConfiguratorBL.DominoPropertiesBL.Ins.GetDiskSettings(SrvAtrSrvNameComboBox.Text, "All");
                }
                //5/12/2014 NS added for VSPLUS-615
                else if (DiskDataTable.Rows.Count == 1 && DiskDataTable.Rows[0]["DiskName"].ToString() == "AllDisks" &&
                  DiskDataTable.Rows[0]["ThresholdType"].ToString() == "GB")//if (DiskDataTable.Rows.Count> 0)
                {
                    SelCriteriaRadioButtonList.SelectedIndex = 1;
                    AdvDiskSpaceThTrackBar.Visible = false;
                    DiskLabel.Visible = false;
                    Label4.Visible = true;
                    DiskGridView.Visible = false;
                    DiskGridInfo.Visible = false;

                    infoDiskDiv.Style.Value = "display: none";
                    GBTextBox.Visible = true;
                    GBLabel.Visible = true;
                    GBTitle.Visible = true;
                    if (DiskDataTable.Rows.Count > 0)
                    {
                        GBTextBox.Text = DiskDataTable.Rows[0]["Threshold"].ToString();
                    }
                    DiskDataTable = VSWebBL.ConfiguratorBL.DominoPropertiesBL.Ins.GetDiskSettings(SrvAtrSrvNameComboBox.Text, "All");
                }
                else
                {
                    //12/16/2013 NS modified - created a radio button list
                    //5/12/2014 NS modified for VSPLUS-615
                    //SelCriteriaRadioButtonList.SelectedIndex = 1;
                    SelCriteriaRadioButtonList.SelectedIndex = 2;
                    AdvDiskSpaceThTrackBar.Visible = false;
                    DiskLabel.Visible = false;
                    Label4.Visible = false;
                    DiskGridView.Visible = true;
                    DiskGridInfo.Visible = true;
                    //rdbSelAll.Checked = false;
                    //rdbSelFew.Checked = true;
                    //rdbNoAlerts.Checked = false;
                    DiskDataTable = VSWebBL.ConfiguratorBL.DominoPropertiesBL.Ins.GetDiskSettings(SrvAtrSrvNameComboBox.Text, "");
                    //12/16/2013 NS added
                    //SelectDisksRoundPanel.Visible = true;
                    //SelectDisksRoundPanel.Enabled = true;
                    //1/31/2014 NS added for VSPLUS-289
                    infoDiskDiv.Style.Value = "display: block";
                    //5/12/2014 NS added for VSPLUS-615
                    GBTextBox.Visible = false;
                    GBLabel.Visible = false;
                    GBTitle.Visible = false;
                }
                //else
                //{ 

                //}
                Session["DiskDataTable"] = DiskDataTable;
                DiskGridView.DataSource = DiskDataTable;
                DiskGridView.DataBind();

            }
            catch (Exception ex)
            {
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
            finally { }
        }


        //For Validation Summary
        //1/6/2014 NS commented out
        /*
        private void ApplyValidationSummarySettings()
        {
            vsValidationSummary1.RenderMode = (ValidationSummaryRenderMode)Enum.Parse(typeof(ValidationSummaryRenderMode), "BulletedList");
            vsValidationSummary1.ShowErrorAsLink = true;
        }
         */
        private void ApplyEditorsSettings()
        {
            try
            {
                ASPxEdit[] editors = new ASPxEdit[] { SrvAtrLocationTextBox };
                foreach (ASPxEdit editor in editors)
                    editor.ValidationSettings.SetFocusOnError = true;
            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
            finally { }

        }

        #region "SrvTskDSTaskSettingsGridView Events"

        /// <summary>
        /// Fired when row is opened for editing/new
        /// Filling Tasks combo box and assigning value from grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SrvTskDSTaskSettingsGridView_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
        {
            try
            {

                if (e.RowType == GridViewRowType.EditForm)
                {
                    ASPxComboBox TaskNameComboBox = (sender as ASPxGridView).FindEditFormTemplateControl("SrvTskGrdTaskComboBox") as ASPxComboBox;
                    if (TaskNameComboBox != null)
                    {
                        FillTaskNameComboBox(TaskNameComboBox);
                        if (!((DevExpress.Web.ASPxGridView)(sender)).IsNewRowEditing)
                            TaskNameComboBox.Value = e.GetValue("TaskName");
                    }
                }


            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
            finally { }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SrvTskDSTaskSettingsGridView_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {



            try
            {
                ASPxGridView gridView = (ASPxGridView)sender;


                //Insert row in DB
                //UpdateServerTaskSettingsData("Insert", GetRow(STSettingsDataSet, e.NewValues.GetEnumerator()));
                UpdateServerTaskSettingsData("Insert", GetRowEditTemplate(gridView, "Insert"));
                //Update Grid after inserting new row, refresh grid as in page load
                gridView.CancelEdit();
                e.Cancel = true;
                FillDSTaskSettingsUpdateGrid(ServerKey.ToString());

            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
            finally { }
        }

        /// <summary>
        /// Return the new/edited row
        /// </summary>
        /// <param name="DSObject">Session DataSet Object</param>
        /// <param name="enumerator">Event Arguments</param>
        /// <returns></returns>
        protected DataRow GetRow(DataSet DSObject, IDictionaryEnumerator enumerator)
        {
            DataTable dataTable = DSObject.Tables[0];
            DataRow DRRow = dataTable.NewRow();

            //IDictionaryEnumerator enumerator = e.NewValues.GetEnumerator();
            enumerator.Reset();
            while (enumerator.MoveNext())
                DRRow[enumerator.Key.ToString()] = (enumerator.Value == null ? "False" : enumerator.Value);
            return DRRow;
        }

        /// <summary>
        /// Get data from SrvTskDSTaskSettingsGridView grid and return Datarow
        /// </summary>
        /// <param name="gridView">SrvTskDSTaskSettingsGridView</param>
        /// <returns>Datarow</returns>
        protected DataRow GetRowEditTemplate(ASPxGridView gridView, string Mode)
        {
            HiddenField SrvTskGrdMyIDHiddenField = (HiddenField)gridView.FindEditFormTemplateControl("SrvTskGrdMyIDHiddenField");
            ASPxCheckBox SrvTskGrdEnabledCheckBox = (ASPxCheckBox)gridView.FindEditFormTemplateControl("SrvTskGrdEnabledCheckBox");
            ASPxComboBox SrvTskGrdTaskComboBox = (ASPxComboBox)gridView.FindEditFormTemplateControl("SrvTskGrdTaskComboBox");
            ASPxCheckBox SrvTskGrdLoadCheckBox = (ASPxCheckBox)gridView.FindEditFormTemplateControl("SrvTskGrdLoadCheckBox");
            ASPxCheckBox SrvTskGrdRestartOffCheckBox = (ASPxCheckBox)gridView.FindEditFormTemplateControl("SrvTskGrdRestartOffCheckBox");
            ASPxCheckBox SrvTskGrdRestartASAPCheckBox = (ASPxCheckBox)gridView.FindEditFormTemplateControl("SrvTskGrdRestartASAPCheckBox");
            ASPxCheckBox SrvTskGrdDisallowCheckBox = (ASPxCheckBox)gridView.FindEditFormTemplateControl("SrvTskGrdDisallowCheckBox");

            STSettingsDataSet = (DataSet)Session["STSettingsDataSet"];
            DataTable dataTable = STSettingsDataSet.Tables[0];
            DataRow DRRow = dataTable.NewRow();
            if (Mode == "Update") DRRow["MyID"] = SrvTskGrdMyIDHiddenField.Value;
            DRRow["Enabled"] = SrvTskGrdEnabledCheckBox.Checked;
            DRRow["Taskname"] = SrvTskGrdTaskComboBox.Value;

            DRRow["SendLoadCommand"] = SrvTskGrdLoadCheckBox.Checked;
            DRRow["RestartOffHours"] = SrvTskGrdRestartOffCheckBox.Checked;
            DRRow["SendRestartCommand"] = SrvTskGrdRestartASAPCheckBox.Checked;
            DRRow["SendExitCommand"] = SrvTskGrdDisallowCheckBox.Checked;

            return DRRow;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SrvTskDSTaskSettingsGridView_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            try
            {
                ASPxGridView gridView = (ASPxGridView)sender;
                //Update DB
                UpdateServerTaskSettingsData("Update", GetRowEditTemplate(gridView, "Update"));
                //Update Grid after updating row, refresh grid 
                gridView.CancelEdit();
                e.Cancel = true;
                FillDSTaskSettingsUpdateGrid(ServerKey.ToString());

                #region "for default edit template"
                //DataRow STSettingsRow = dataTable.Rows.Find(e.Keys[0]);
                //IDictionaryEnumerator enumerator = e.NewValues.GetEnumerator();
                //enumerator.Reset();
                //while (enumerator.MoveNext())
                //    STSettingsRow[enumerator.Key.ToString()] = enumerator.Value;

                //Update row in DB
                //UpdateServerTaskSettingsData("Update", GetRow(STSettingsDataSet, e.NewValues.GetEnumerator()));

                //if (dataTable.Rows.Count > 0)
                //{
                //    DataTable dtcopy = dataTable.Copy();
                //    dtcopy.PrimaryKey = new DataColumn[] { dtcopy.Columns["MyID"] };
                //    STSettingsDataSet.Tables.Clear();
                //    STSettingsDataSet.Tables.Add(dtcopy);
                //    Session["STSettingsDataSet"] = STSettingsDataSet;
                //    gridView.DataSource = STSettingsDataSet.Tables[0];//DSTaskSettingsDataTable;
                //    gridView.DataBind();
                //}
                //gridView.CancelEdit();
                //e.Cancel = true;
                #endregion
            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
            finally { }
        }

        /// <summary>
        /// Delete Grid row
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SrvTskDSTaskSettingsGridView_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            try
            {
                ServerTaskSettings STSettingsObject = new ServerTaskSettings();
                STSettingsObject.MyID = Convert.ToInt32(e.Keys[0]);

                //Delete row from DB
                Object ReturnValue = VSWebBL.ConfiguratorBL.ServerTaskSettingsBL.Ins.DeleteData(STSettingsObject);

                //Update Grid after inserting new row, refresh grid as in page load
                ASPxGridView gridView = (ASPxGridView)sender;
                gridView.CancelEdit();
                e.Cancel = true;
                FillDSTaskSettingsUpdateGrid(ServerKey.ToString());

            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
            finally { }
        }

        #endregion

        //#region "MntWinDominoMaintGridView Events"

        ///// <summary>
        ///// Do tasks when grid is prepared for updating
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //protected void MntWinDominoMaintGridView_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        //{
        //    if (e.Column.FieldName != "MaintWindow") return;
        //    ASPxComboBox MaintWindowComboBox = e.Editor as ASPxComboBox;
        //    FillMaintWindowComboBox(MaintWindowComboBox);
        //    MaintWindowComboBox.Callback += new CallbackEventHandlerBase(MaintWindowComboBox_OnCallback);
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //protected void MntWinDominoMaintGridView_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        //{
        //    MaintWindowsDataSet = (DataSet)Session["MaintWindowsDataSet"];
        //    ASPxGridView gridView = (ASPxGridView)sender;

        //    //DataTable dataTable = STSettingsDataSet.Tables[0];
        //    //DataRow STSettingsRow = dataTable.NewRow();
        //    //STSettingsRow=GetRow(STSettingsDataSet, e.NewValues.GetEnumerator());

        //    //Insert row in DB
        //    UpdateDominoMaintData("Insert", GetRow(MaintWindowsDataSet, e.NewValues.GetEnumerator()));
        //    //Update Grid after inserting new row, refresh grid as in page load
        //    gridView.CancelEdit();
        //    e.Cancel = true;
        //    FillMaintenanceWindowsUpdateGrid(SrvAtrSrvNameComboBox.Text);
        //}


        //protected void MntWinDominoMaintGridView_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        //{
        //    MaintenanceWindows MWObject = new MaintenanceWindows();
        //    MWObject.MaintWindow = e.Keys[0].ToString();

        //    //Delete row from DB
        //    Object ReturnValue = VSWebBL.ConfiguratorBL.MaintenanceWindowsBL.Ins.DeleteData(MWObject);

        //    //Update Grid after inserting new row, refresh grid as in page load
        //    ASPxGridView gridView = (ASPxGridView)sender;
        //    gridView.CancelEdit();
        //    e.Cancel = true;
        //    FillMaintenanceWindowsUpdateGrid(SrvAtrSrvNameComboBox.Text);
        //}


        //#endregion


        /// <summary>
        /// OK Button Click Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void FormOkButton_Click(object sender, EventArgs e)
        {
            try
            {
                //5/1/2014 NS modified for VSPLUS-427
                bool proceed = true;
                //5/12/2014 NS added for VSPLUS-615
                string errtext = "";
                int gbc = 0;
                //5/12/2014 NS modified for VSPLUS-615
                //if (SelCriteriaRadioButtonList.SelectedIndex == 1)
                if (SelCriteriaRadioButtonList.SelectedItem.Value.ToString() == "1")
                {
                    //VSPLUS-968: Disk space settings are cleared after upgrade,Mukund 30Sep14. Commented below line
                    //VSWebBL.ConfiguratorBL.DominoPropertiesBL.Ins.DeleteAllRecordsfromDiskSettingsBL(SrvAtrSrvNameComboBox.Text);
					List<object> fieldValues = DiskGridView.GetSelectedFieldValues(new string[] { "DiskName", "Threshold", "ThresholdType", "DiskInfo" });
                    if (fieldValues.Count == 0)
                    {
                        proceed = false;
                        isValid = false;
                        errtext = "You have enabled a 'Selected Disks' option on the Disk Settings tab but selected no disks " +
                        "in the grid or some of the selected disks do not have a threshold or threshold type value. <br />" +
                        "Please correct the disk settings in order to save your changes.";
                    }
                    else
                    {
                        foreach (object[] item in fieldValues)
                        {
                            if (item[1].ToString() == "" || item[2].ToString() == "")
                            {
                                proceed = false;
                                isValid = false;
                                errtext = "You have enabled a 'Selected Disks' option on the Disk Settings tab but selected no disks " +
                                "in the grid or some of the selected disks do not have a threshold or threshold type value or Disk Info. <br />" +
                                "Please correct the disk settings in order to save your changes.";
                            }
                        }
                    }
                }

                //5/12/2014 NS added for VSPLUS-615
                if (SelCriteriaRadioButtonList.SelectedItem.Value.ToString() == "3")
                {
                    if (GBTextBox.Text == "")
                    {
                        proceed = false;
                        isValid = false;
                        errtext = "You have enabled an 'All Disks - By GB' option on the Disk Settings tab but entered no threshold value. " +
                            "You must enter a numeric threshold value in order to save your changes.";
                    }
                    else if (!int.TryParse(GBTextBox.Text, out gbc))
                    {
                        proceed = false;
                        isValid = false;
                        errtext = "You have enabled an 'All Disks - By GB' option on the Disk Settings tab but entered an invalid threshold value. " +
                            "You must enter a numeric threshold value in order to save your changes.";
                    }
                }
                if (proceed)
                {
                    try
                    {
                        //DataTable dt = GetDataView(DiskGridView);
                        //Save Server Attributes Tab
                        UpdateDominoServersData();
                    }
                    catch (Exception ex)
                    {
                        //6/27/2014 NS added for VSPLUS-634
                        Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                        throw ex;
                    }
                    finally { }
                }
                else
                {
                    errorDiv.Style.Value = "display: block;";
                    //10/6/2014 NS modified for VSPLUS-990
                    errorDiv.InnerHtml = errtext +
                        "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                }

            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
            finally { }
        }


        #endregion



        #region "Procedures/Functions"
        private DataTable GetDataView(ASPxGridView grid)
        {
            DataTable dt = new DataTable();
            foreach (GridViewColumn col in grid.VisibleColumns)
            {
                GridViewDataColumn dataColumn = col as GridViewDataColumn;
                if (dataColumn == null) continue;
                dt.Columns.Add(dataColumn.FieldName);
            }
            for (int i = 0; i < grid.VisibleRowCount; i++)
            {
                DataRow row = dt.Rows.Add();
                foreach (DataColumn col in dt.Columns)
                    row[col.ColumnName] = grid.GetRowValues(i, col.ColumnName);
            }
            return dt;
        }


        #region "Fetch Data"

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private void TaskNameComboBox_OnCallback(object source, CallbackEventArgsBase e)
        {
            try
            {

                FillTaskNameComboBox(source as ASPxComboBox);
            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
            finally { }
        }

        /// <summary>
        /// Fill TaskNameComboBox from DB
        /// </summary>
        /// <param name="TaskNameComboBox"></param>
        private void FillTaskNameComboBox(ASPxComboBox TaskNameComboBox)
        {
            try
            {

                DataTable DSTasksDataTable = VSWebBL.ConfiguratorBL.DominoServerTasksBL.Ins.GetAllData();
                TaskNameComboBox.DataSource = DSTasksDataTable;
                TaskNameComboBox.TextField = "TaskName";
                TaskNameComboBox.ValueField = "TaskName";
                TaskNameComboBox.DataBind();
            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
            finally { }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        //private void MaintWindowComboBox_OnCallback(object source, CallbackEventArgsBase e)
        //{
        //    FillMaintWindowComboBox(source as ASPxComboBox);
        //}

        /// <summary>
        /// Fill TaskNameComboBox from DB
        /// </summary>
        /// <param name="MaintWindowComboBox"></param>
        //private void FillMaintWindowComboBox(ASPxComboBox MaintWindowComboBox)
        //{
        //    DataTable DSTasksDataTable = VSWebBL.ConfiguratorBL.MaintenanceSettingsBL.Ins.GetAllData();
        //    MaintWindowComboBox.DataSource = DSTasksDataTable;
        //    MaintWindowComboBox.TextField = "Name";
        //    MaintWindowComboBox.ValueField = "Name";
        //    MaintWindowComboBox.DataBind();
        //}
        /// <summary>
        /// Fills Controls with data based on Key
        /// </summary>
        private void FillData(int Key)
        {
            try
            {

                DominoServers DominoServersObject = new DominoServers();
                DominoServers ReturnDSObject = new DominoServers();
                DominoServersObject.Key = Key;
                ReturnDSObject = VSWebBL.ConfiguratorBL.DominoPropertiesBL.Ins.GetData(DominoServersObject);
                //5/5/2014 NS modified for VSPLUS-589
                //if (ReturnDSObject.NotificationGroup != null)
                if (ReturnDSObject.ScanInterval != null)
                {
					if (ReturnDSObject.Category != null)
					{
						SrvAtrCategoryComboBox.Text = ReturnDSObject.Category.ToString();

					} SrvAtrDeadMailThTextBox.Text = ReturnDSObject.DeadThreshold.ToString();
                    SrvAtrDescriptionTextBox.Text = (ReturnDSObject.Name==null ? "" : ReturnDSObject.Name.ToString());
                    SrvAtrScanCheckBox.Checked = (ReturnDSObject.Enabled.ToString() == "True" ? true : false);
                    RequireSLLCheckBox.Checked = (ReturnDSObject.RequireSSL.ToString() == "True" ? true : false);
					ScanTravelerServer.Checked = (ReturnDSObject.ScanTravelerServer.ToString() == "True" ? true : false);
					EnableTravelerCheckBox.Checked = (ReturnDSObject.EnableTravelerBackend.ToString() == "True" ? true : false);

					ExternalAliastextBox.Text = (ReturnDSObject.ExternalAlias == null ? "" : ReturnDSObject.ExternalAlias.ToString());
					SrvAtrFailBefAlertTextBox.Text = (ReturnDSObject.FailureThreshold==null ? "" : ReturnDSObject.FailureThreshold.ToString());
                    SrvAtrLocationTextBox.Text = (ReturnDSObject.Location==null ? "" : ReturnDSObject.Location.ToString());
                    //ReturnDSObject.MailDirectory;
                    SrvAtrSrvNameComboBox.Text = (ReturnDSObject.Name==null ?  "" : ReturnDSObject.Name.ToString());
                    SrvAtrOffScanIntvlTextBox.Text = ReturnDSObject.OffHoursScanInterval.ToString();
                    SrvAtrPendingMailThTextBox.Text = ReturnDSObject.PendingThreshold.ToString();
                    //int msg = Convert.ToInt32(ReturnDSObject.CheckMailThreshold);
                    SrvAtrstopcountingCheckBox.Checked = (ReturnDSObject.CheckMailThreshold.ToString() == "True" ? true : false);
                    SrvAtrResponseThTextBox.Text = (ReturnDSObject.ResponseThreshold==null ? "" : ReturnDSObject.ResponseThreshold.ToString());
                    SrvAtrRetryIntvlTextBox.Text = (ReturnDSObject.RetryInterval==null ? "" :  ReturnDSObject.RetryInterval.ToString());
                    SrvAtrScanIntvlTextBox.Text = (ReturnDSObject.ScanInterval==null ? "" : ReturnDSObject.ScanInterval.ToString());
                    //ReturnDSObject.SearchString.ToString();
                    SrvAtrDeadMailCountTextBox.Text = (ReturnDSObject.DeadMailDeleteThreshold==null ? "" :  ReturnDSObject.DeadMailDeleteThreshold.ToString());
                    //5/18/2013 NS commented out;; Mukund reset on 14Apr14 for VSPLUS-550
                    SrvAtrDelDeadMailCheckBox.Checked = (SrvAtrDeadMailCountTextBox.Text != "" && SrvAtrDeadMailCountTextBox.Text != "0" ? true : false);
                    SrvAtrHeldMailThTextBox.Text = (ReturnDSObject.HeldThreshold==null ? "" :  ReturnDSObject.HeldThreshold.ToString());
                    SrvAtrDBHealthCheckBox.Checked = (ReturnDSObject.ScanDBHealth.ToString() == "True" ? true : false);
                    // dhiren
                    SendRouterRestartCheckBox.Checked = (ReturnDSObject.SendRouterRestart.ToString() == "True" ? true : false);

                    logCheckBox.Checked = (ReturnDSObject.scanlog.ToString() == "True" ? true : false);
                    agentlogCheckBox.Checked = (ReturnDSObject.scanagentlog.ToString() == "True" ? true : false);

					SendTextBox.Text = (ReturnDSObject.NotificationGroup == null ? "" : ReturnDSObject.NotificationGroup.ToString());

                    AdvMonitorBESNtwrkQCheckBox.Checked = ReturnDSObject.BES_Server;
					AdvBESMsgQTextBox.Text = (ReturnDSObject.BES_Threshold == null ? "" : ReturnDSObject.BES_Threshold.ToString());
                    // AdvIPAddressTextBox.Text = ReturnDSObject.IPAddress.ToString();
                    AdvNtwrkConCheckBox.Checked = (AdvIPAddressTextBox.Text != "" ? true : false);
                    AdvDiskSpaceThTrackBar.Value = (ReturnDSObject.DiskSpaceThreshold * 100).ToString();
                    //12/17/2013 NS modified
                    //DiskLabel.Text = (ReturnDSObject.DiskSpaceThreshold * 100).ToString()+"%";
                    DiskLabel.Text = "Current threshold: " + (ReturnDSObject.DiskSpaceThreshold * 100).ToString() + "% free space";
                    AdvMemoryThTrackBar.Value = (ReturnDSObject.Memory_Threshold * 100).ToString();
                    MemLabel.Text = (ReturnDSObject.Memory_Threshold * 100).ToString() + "%";
                    AdvCPUThTrackBar.Value = (ReturnDSObject.CPU_Threshold * 100).ToString();
                    CpuLabel.Text = (ReturnDSObject.CPU_Threshold * 100).ToString() + "%";
                    AdvClusterRepTextBox.Text = ReturnDSObject.Cluster_Rep_Delays_Threshold.ToString();
                    LoadClusterRepTextBox.Text = ReturnDSObject.Load_Cluster_Rep_Delays_Threshold.ToString();
                    lblServerID.Text = ReturnDSObject.Key.ToString();
                    //Mukund 11Apr14 modified
                    Session["ReturnUrl"] = "DominoProperties.aspx?Key=" + lblServerID.Text + "&tab=4";
                    MemRoundPanel9.HeaderText = "Memory Usage Alert at " + AdvMemoryThTrackBar.Value + "% utilization";
                    //12/17/2013 NS modified
                    //DiskRoundPanel7.HeaderText = "Disk Space Alert at  " + AdvDiskSpaceThTrackBar.Value + "% free space";
                    CPURoundPanel.HeaderText = "CPU Utilization Alert at " + AdvCPUThTrackBar.Value + "% utilization";

                    ServerDaysAlert.Text = ReturnDSObject.ServerDaysAlert.ToString();
                    int credentialsId = 0;
                    if (ReturnDSObject.CredentialsID.ToString() != null && ReturnDSObject.CredentialsID.ToString() != "")
                    {
                        credentialsId = ReturnDSObject.CredentialsID;
                        CredentialsComboBox.SelectedItem = CredentialsComboBox.Items.FindByValue(credentialsId);
                        for (int i = 0; i < CredentialsComboBox.Items.Count; i++)
                        {
                            if (CredentialsComboBox.Items[i].Value.ToString() == credentialsId.ToString())
                                CredentialsComboBox.Items[i].Selected = true;
                        }
                    }
					if (ReturnDSObject.CredentialsID == -1)
					{
						CredentialsComboBox.Text = "None";
					}
					EnableServletScan.Checked = (ReturnDSObject.ScanServlet.ToString() == "True" ? true : false);
                    //6/18/2015 NS added for VSPLUS-1802
                    EXJournalScanCheckBox.Checked = (ReturnDSObject.EXJEnabled.ToString() == "True" ? true : false);
                    //8/6/2015 NS added for VSPLUS-1802
                    if (EXJournalScanCheckBox.Checked)
                    {
                        EXJournalLookBackCheckBox.ClientVisible = true;
                    }
                    else
                    {
                        EXJournalLookBackCheckBox.ClientVisible = false;
                    }
                    if (ReturnDSObject.EXJStartTime != null && ReturnDSObject.EXJStartTime != "")
                    {
                        EXJournalLookBackCheckBox.Checked = true;
                        StartTimeEdit.Text = ReturnDSObject.EXJStartTime.ToString();
                        MaintDurationTextBox.Text = ReturnDSObject.EXJDuration.ToString();
                        LookBackPeriodTextBox.Text = ReturnDSObject.EXJLookBackDuration.ToString();
                        DurationTrackBar.Position = ReturnDSObject.EXJDuration;
                        lblEndTime.Text = Convert.ToDateTime(ReturnDSObject.EXJStartTime.ToString()).AddMinutes(Convert.ToDouble(ReturnDSObject.EXJDuration.ToString())).ToShortTimeString();
						//2/8/2016 Durga Added for VSPLUS-2580
						lblDuration.Text = DurationHoursMins(ReturnDSObject.EXJDuration.ToString());
                    }
                    ShowHideLookBack();
                    //2/23/2016 NS added for VSPLUS-2641
                    SrvAIThresholdTextBox.Text = ReturnDSObject.AvailabilityIndexThreshold.ToString();
                }
                else
                {
                    SrvAtrDescriptionTextBox.Text = ReturnDSObject.Description.ToString();
                    SrvAtrLocationTextBox.Text = ReturnDSObject.Location.ToString();
                    SrvAtrSrvNameComboBox.Text = ReturnDSObject.Name.ToString();
                    lblServerID.Text = ReturnDSObject.Key.ToString();
                    Session["ReturnUrl"] = "DominoProperties.aspx?Key=" + lblServerID.Text + "&tab=3";
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

        /// <summary>
        /// Fill grid SrvTskDSTaskSettingsGridView
        /// </summary>
        /// <param name="ServerKey"></param>
        private void FillDSTaskSettingsUpdateGrid(string ServerKey)
        {
            try
            {

                DataTable DSTaskSettingsDataTable = new DataTable();
                STSettingsDataSet = new DataSet();

                DSTaskSettingsDataTable = VSWebBL.ConfiguratorBL.DominoPropertiesBL.Ins.DSTaskSettingsUpdateGrid(ServerKey);

                DataTable dtcopy = DSTaskSettingsDataTable.Copy();
                dtcopy.PrimaryKey = new DataColumn[] { dtcopy.Columns["MyID"] };
                STSettingsDataSet.Tables.Add(dtcopy);
                Session["STSettingsDataSet"] = STSettingsDataSet;

                SrvTskDSTaskSettingsGridView.DataSource = STSettingsDataSet.Tables[0];//DSTaskSettingsDataTable;
                SrvTskDSTaskSettingsGridView.DataBind();


                //{
                //    DSTaskSettingsDataTable = VSWebBL.ConfiguratorBL.DominoPropertiesBL.Ins.DSTaskSettingsUpdategridFirstTime(ServerKey);
                //    if (DSTaskSettingsDataTable.Rows.Count > 0)
                //    {
                //        DataTable dtcopy = DSTaskSettingsDataTable.Copy();
                //        dtcopy.PrimaryKey = new DataColumn[] { dtcopy.Columns["MyID"] };
                //        STSettingsDataSet.Tables.Add(dtcopy);
                //        Session["STSettingsDataSet"] = STSettingsDataSet;

                //        SrvTskDSTaskSettingsGridView.DataSource = STSettingsDataSet.Tables[0];//DSTaskSettingsDataTable;
                //        SrvTskDSTaskSettingsGridView.DataBind();
                //    }
                //}
            }
            catch (Exception ex)
            {
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
            finally { }
        }


        // fill grid from Session

        private void FillDSTaskSettingsUpdateGridfromsession(string ServerKey)
        {
            try
            {

                DataTable DSTaskSettingsDataTable = new DataTable();
                STSettingsDataSet = new DataSet();
                if (Session["STSettingsDataSet"] != "" && Session["STSettingsDataSet"] != null)
                    DSTaskSettingsDataTable = ((DataSet)Session["STSettingsDataSet"]).Tables[0];//VSWebBL.ConfiguratorBL.DominoPropertiesBL.Ins.DSTaskSettingsUpdateGrid(ServerKey);

                if (DSTaskSettingsDataTable.Rows.Count > 0)
                {
                    DataTable dtcopy = DSTaskSettingsDataTable.Copy();
                    dtcopy.PrimaryKey = new DataColumn[] { dtcopy.Columns["MyID"] };
                    STSettingsDataSet.Tables.Add(dtcopy);
                    //    Session["STSettingsDataSet"] = STSettingsDataSet;

                    SrvTskDSTaskSettingsGridView.DataSource = STSettingsDataSet.Tables[0];//DSTaskSettingsDataTable;
                    // SrvTskDSTaskSettingsGridView.DataSource = DSTaskSettingsDataTable;
                    SrvTskDSTaskSettingsGridView.DataBind();
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


        /// <summary>
        /// Fill grid MntWinDominoMaintGridView
        /// </summary>
        /// <param name="ServerName"></param>
        //private void FillMaintenanceWindowsUpdateGrid(string ServerName)
        //{
        //    try
        //    {

        //        DataTable MaintWindowsDataTable = new DataTable();

        //        //DataTable DSTaskSettingsDataTable = new DataTable();
        //        MaintWindowsDataSet = new DataSet();

        //        MaintWindowsDataTable = VSWebBL.ConfiguratorBL.MaintenanceWindowsBL.Ins.MaintenanceWindowsUpdateGrid(ServerName);
        //        if (MaintWindowsDataTable.Rows.Count > 0)
        //        {
        //            DataTable dtcopy = MaintWindowsDataTable.Copy();
        //            dtcopy.PrimaryKey = new DataColumn[] { dtcopy.Columns["Name"] };
        //            MaintWindowsDataSet.Tables.Add(dtcopy);
        //            Session["MaintWindowsDataSet"] = MaintWindowsDataSet;

        //            MntWinDominoMaintGridView.DataSource = MaintWindowsDataSet.Tables[0];
        //            MntWinDominoMaintGridView.DataBind();
        //        }
        //    }
        //    catch (Exception ex)
        //    { throw ex; }
        //    finally { }
        //}
        //private void FillMaintenanceWindowsUpdateGridfromSession(string ServerName)
        //{
        //    try
        //    {

        //        DataTable MaintWindowsDataTable = new DataTable();

        //        //DataTable DSTaskSettingsDataTable = new DataTable();
        //        MaintWindowsDataSet = new DataSet();

        //       // MaintWindowsDataTable = VSWebBL.ConfiguratorBL.MaintenanceWindowsBL.Ins.MaintenanceWindowsUpdateGrid(ServerName);
        //        MaintWindowsDataTable = ((DataSet)Session["MaintWindowsDataSet"]).Tables[0];
        //        if (MaintWindowsDataTable.Rows.Count > 0)
        //        {
        //            DataTable dtcopy = MaintWindowsDataTable.Copy();
        //            dtcopy.PrimaryKey = new DataColumn[] { dtcopy.Columns["Name"] };
        //            MaintWindowsDataSet.Tables.Add(dtcopy);
        //            //Session["MaintWindowsDataSet"] = MaintWindowsDataSet;

        //            MntWinDominoMaintGridView.DataSource = MaintWindowsDataSet.Tables[0];
        //            MntWinDominoMaintGridView.DataBind();
        //        }
        //    }
        //    catch (Exception ex)
        //    { throw ex; }
        //    finally { }

        //}
        #endregion

        #region "Update Data"
        /// <summary>
        /// Saving data
        /// </summary>
        /// 
        /// 

        //private void DeleteCheakbox()
        //{
        //    try
        //    {
        //        Object ReturnValue;
        //        if (ServerKey > 0)
        //        {
        //            ReturnValue = VSWebBL.ConfiguratorBL.DominoPropertiesBL.Ins.UpdateData(CollectDataForDominoServers());
        //        }
        //        else
        //        {
        //            ReturnValue = VSWebBL.ConfiguratorBL.DominoPropertiesBL.Ins.InsertData(CollectDataForDominoServers());
        //        }

        //        ReturnValue = UpdateDiskSettings();

        //        SetFocusOnError(ReturnValue);
        //        if (ReturnValue.ToString() == "True")
        //        {
        //            //1/21/2014 NS modified for 
        //            /*
        //            ErrorMessageLabel.Text = "Data updated successfully.";
        //            ErrorMessagePopupControl.HeaderText = "Information";
        //            ErrorMessagePopupControl.ShowCloseButton = false;
        //            ValidationUpdatedButton.Visible = true;
        //            ValidationOkButton.Visible = false; 
        //            ErrorMessagePopupControl.ShowOnPageLoad = true;
        //            */
        //            Session["DominoUpdateStatus"] = SrvAtrSrvNameComboBox.Text;
        //            Response.Redirect("LotusDominoServers.aspx");
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        errorDiv.InnerHtml = "The following error has occurred: " + ex.Message;
        //        errorDiv.Style.Value = "display: block";
        //    }
        //}

        private void UpdateDominoServersData()
        {
            try
            {
                Object ReturnValue;
                if (ServerKey > 0)
                {
                    ReturnValue = VSWebBL.ConfiguratorBL.DominoPropertiesBL.Ins.UpdateData(CollectDataForDominoServers());
                }
                else
                {
                    ReturnValue = VSWebBL.ConfiguratorBL.DominoPropertiesBL.Ins.InsertData(CollectDataForDominoServers());
                }

                if (ReturnValue.ToString() == "True")
                {
                    ReturnValue = UpdateDiskSettings();
                    if (ReturnValue.ToString() == "True")
                    {
                        Session["DominoUpdateStatus"] = SrvAtrSrvNameComboBox.Text;
                        Response.Redirect("LotusDominoServers.aspx", false);
                        Context.ApplicationInstance.CompleteRequest();
                    }
                    else
                    {
                        SetFocusOnError(ReturnValue);
                    }
                }
                else
                {
                    SetFocusOnError(ReturnValue);
                }
            }
            catch (Exception ex)
            {
                //10/3/2014 NS modified for VSPLUS-990
                errorDiv.InnerHtml = "The following error has occurred: " + ex.Message +
                    "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                errorDiv.Style.Value = "display: block";
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
            }
        }
        private bool UpdateDiskSettings()
        {
            bool ReturnValue = false;

            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("ServerName");
                dt.Columns.Add("DiskName");
                dt.Columns.Add("Threshold");
                //5/1/2014 NS added for VSPLUS-602
                dt.Columns.Add("ThresholdType");
				dt.Columns.Add("DiskInfo");
                //5/1/2014 NS modified for VSPLUS-602
				List<object> fieldValues = DiskGridView.GetSelectedFieldValues(new string[] { "DiskName", "Threshold", "ThresholdType", "DiskInfo" });
                //if (DiskGridView.VisibleRowCount == fieldValues.Count)
                //12/17/2013 NS modified
                //5/12/2014 NS modified for VSPLUS-615
                if (SelCriteriaRadioButtonList.SelectedItem.Value.ToString() == "0")
                //if(rdbSelAll.Checked)
                {
                    DataRow row = dt.Rows.Add();
                    row["ServerName"] = SrvAtrSrvNameComboBox.Text;
                    //5/12/2014 NS modified for VSPLUS-615
                    //row["DiskName"] = "0";
                    //row["Threshold"] = "0";
                    row["DiskName"] = "AllDisks";
                    row["Threshold"] = (Convert.ToInt32(AdvDiskSpaceThTrackBar.Value)).ToString();
                    //5/1/2014 NS added for VSPLUS-602
                    row["ThresholdType"] = "Percent";
                }
                else if (SelCriteriaRadioButtonList.SelectedItem.Value.ToString() == "1")
                //else if(rdbSelFew.Checked)
                {
                    foreach (object[] item in fieldValues)
                    {
                        DataRow row = dt.Rows.Add();
                        row["ServerName"] = SrvAtrSrvNameComboBox.Text;
                        row["DiskName"] = item[0].ToString();
                        row["Threshold"] = (item[1].ToString() != "" ? item[1].ToString() : (Convert.ToInt32(AdvDiskSpaceThTrackBar.Value)).ToString());
                        //5/1/2014 NS added for VSPLUS-602
                        row["ThresholdType"] = item[2].ToString();
						row["DiskInfo"] = item[3].ToString();
                    }
                }
                else if (SelCriteriaRadioButtonList.SelectedItem.Value.ToString() == "2")
                //else if (rdbNoAlerts.Checked)
                {

                    DataRow row = dt.Rows.Add();
                    row["ServerName"] = SrvAtrSrvNameComboBox.Text;
                    row["DiskName"] = "NoAlerts";
                    row["Threshold"] = "0";
                    //5/1/2014 NS added for VSPLUS-602
                    row["ThresholdType"] = "Percent";
                }
                //5/12/2014 NS added for VSPLUS-615
                else if (SelCriteriaRadioButtonList.SelectedItem.Value.ToString() == "3")
                {
                    DataRow row = dt.Rows.Add();
                    row["ServerName"] = SrvAtrSrvNameComboBox.Text;
                    row["DiskName"] = "AllDisks";
                    row["Threshold"] = GBTextBox.Text;
                    row["ThresholdType"] = "GB";

                }

                //Mukund, 14Apr14 , included if condition to avoid error deleting blank records
                if (dt.Rows.Count > 0)
                    ReturnValue = VSWebBL.ConfiguratorBL.DominoPropertiesBL.Ins.InsertDiskSettingsData(dt);
            }
            catch (Exception ex)
            {
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
            return ReturnValue;

        }
        /// <summary>
        /// Saving data
        /// </summary>
        private void UpdateServerTaskSettingsData(string Mode, DataRow STSettingsRow)
        {
            try
            {
                if (Mode == "Update")
                {
                    Object ReturnValue = VSWebBL.ConfiguratorBL.ServerTaskSettingsBL.Ins.UpdateData(CollectDataForServerTaskSettings(Mode, STSettingsRow));
                }
                else if (Mode == "Insert")
                {
                    Object ReturnValue = VSWebBL.ConfiguratorBL.ServerTaskSettingsBL.Ins.InsertData(CollectDataForServerTaskSettings(Mode, STSettingsRow));
                }
                //SetFocusOnError(ReturnValue);

            }
            catch (Exception ex)
            {
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
            finally { }
        }

        //private void UpdateDominoMaintData(string Mode, DataRow MaintWindowsRow)
        //{
        //    if (Mode == "Insert")
        //    {
        //        Object ReturnValue = VSWebBL.ConfiguratorBL.MaintenanceWindowsBL.Ins.InsertData(CollectDataForMaintWindows(Mode, MaintWindowsRow));
        //    }
        //}

        private void SetFocusOnError(Object ReturnValue)
        {
            string ErrorMessage = ReturnValue.ToString();
            if (ErrorMessage.Substring(0, 2) == "ER")
            {
                //Find control & set focus

                //string ControlName = ErrorMessage.Substring(3, ErrorMessage.IndexOf("@") - 3);
                //if (ControlName.EndsWith("ComboBox"))
                //{
                //    DropDownList ddl = (DropDownList)FindControl(ControlName);
                //    ddl.Focus();
                //}
                //if (ControlName.EndsWith("TextBox"))
                //{
                //    TextBox txt = (TextBox)FindControl(ControlName);
                //    txt.Focus(); 
                //}
                //if (ControlName.EndsWith("CheckBox"))
                //{
                //    CheckBox chk = (CheckBox)FindControl(ControlName);
                //    chk.Focus();;
                //}                

                //6/18/2015 NS modified for VSPLUS-1802
                //ErrorMessageLabel.Text = ErrorMessage.Substring(3);
                //ErrorMessagePopupControl.ShowOnPageLoad = true;
                errorDiv.Style.Value = "display: block;";
                //10/3/2014 NS modified for VSPLUS-990
                errorDiv.InnerHtml = ErrorMessage.Substring(3) +
                    "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
            }
        }
        #endregion

        #region "CollectData"
        /// <summary>
        /// Fills DominoServers Object with Controls data
        /// </summary>
        /// <returns>DominoServers Object</returns>
        private DominoServers CollectDataForDominoServers()
        {
            try
            {
                DominoServers DominoServersObject = new DominoServers();
                if (AdvDiskSpaceThTrackBar.Value.ToString() != null && AdvBESMsgQTextBox.Text != null && SrvAtrDeadMailThTextBox.Text != null && SrvAtrFailBefAlertTextBox.Text != null && SrvAtrLocationTextBox.Text != null && SrvAtrPendingMailThTextBox.Text != null && SrvAtrResponseThTextBox.Text != null && SrvAtrRetryIntvlTextBox.Text != null && SrvAtrScanIntvlTextBox.Text != null && SrvAtrDeadMailThTextBox.Text != null && SrvAtrHeldMailThTextBox.Text != null && AdvMemoryThTrackBar.Value.ToString() != null && AdvCPUThTrackBar.Value.ToString() != null && AdvClusterRepTextBox.Text.ToString() != null)
                {
                    DominoServersObject.DiskSpaceThreshold = float.Parse(AdvDiskSpaceThTrackBar.Value.ToString()) / 100;
                    DominoServersObject.BES_Server = AdvMonitorBESNtwrkQCheckBox.Checked;
                    //DominoServersObject.BES_Threshold = int.Parse(AdvBESMsgQTextBox.Text);
                    DominoServersObject.Category = SrvAtrCategoryComboBox.Text;
                    if (SrvAtrDeadMailThTextBox.Text != null && SrvAtrDeadMailThTextBox.Text != "")
                    {
                        DominoServersObject.DeadThreshold = int.Parse(SrvAtrDeadMailThTextBox.Text);
                    }
                    //DominoServersObject.Description = SrvAtrDescriptionTextBox.Text;
                    DominoServersObject.Enabled = SrvAtrScanCheckBox.Checked;
                    //if (SrvAtrstopcountingCheckBox.Checked)
                    //{
                    //    bool val2 = true;
                    //}
                    //else
                    //{
                    //    bool val2 = false;
                    //}
                    DominoServersObject.CheckMailThreshold = SrvAtrstopcountingCheckBox.Checked;

                    DominoServersObject.RequireSSL = RequireSLLCheckBox.Checked;
					DominoServersObject.ScanTravelerServer = ScanTravelerServer.Checked;

					DominoServersObject.EnableTravelerBackend = EnableTravelerCheckBox.Checked;
                    DominoServersObject.SendRouterRestart = SendRouterRestartCheckBox.Checked;

                    DominoServersObject.ExternalAlias = ExternalAliastextBox.Text;
                    if (SrvAtrFailBefAlertTextBox.Text != null && SrvAtrFailBefAlertTextBox.Text != "")
                    {
                        DominoServersObject.FailureThreshold = int.Parse(SrvAtrFailBefAlertTextBox.Text);
                    }
                    //DominoServersObject.LocationID = int.Parse(SrvAtrLocationTextBox.Text);
                    //DominoServersObject.MailDirectory;
                    // DominoServersObject.Name = SrvAtrSrvNameComboBox.Text;
                    if (SrvAtrScanIntvlTextBox.Text != null && SrvAtrScanIntvlTextBox.Text != "")
                    {
                        DominoServersObject.ScanInterval = int.Parse(SrvAtrScanIntvlTextBox.Text);
                    }
                    if (SrvAtrOffScanIntvlTextBox.Text != null && SrvAtrOffScanIntvlTextBox.Text != "")
                    {
                        //if (Convert.ToInt32(SrvAtrOffScanIntvlTextBox.Text) > Convert.ToInt32(SrvAtrScanIntvlTextBox.Text))
                        //{
                        DominoServersObject.OffHoursScanInterval = int.Parse(SrvAtrOffScanIntvlTextBox.Text);
                        //}
                        //else
                        //{
                        //DominoServersObject.OffHoursScanInterval = int.Parse(SrvAtrOffScanIntvlTextBox.Text);
                        //}
                    }
                    if (SrvAtrPendingMailThTextBox.Text != null && SrvAtrPendingMailThTextBox.Text != "")
                    {
                        DominoServersObject.PendingThreshold = int.Parse(SrvAtrPendingMailThTextBox.Text);
                    }
                    if (SrvAtrResponseThTextBox.Text != null && SrvAtrResponseThTextBox.Text != "")
                    {
                        DominoServersObject.ResponseThreshold = int.Parse(SrvAtrResponseThTextBox.Text);
                    }
                    if (SrvAtrRetryIntvlTextBox.Text != null && SrvAtrRetryIntvlTextBox.Text != "")
                    {
                        DominoServersObject.RetryInterval = int.Parse(SrvAtrRetryIntvlTextBox.Text);
                    }

                    if (SrvAtrDeadMailCountTextBox.Text != null && SrvAtrDeadMailCountTextBox.Text != "" && SrvAtrDelDeadMailCheckBox.Checked)
                    {
                        //DominoServersObject.SearchString;
                        DominoServersObject.DeadMailDeleteThreshold = int.Parse(SrvAtrDeadMailCountTextBox.Text);
                        //DominoServersObject.IPAddress = AdvIPAddressTextBox.Text;
                    }
                    if (SrvAtrHeldMailThTextBox.Text != null && SrvAtrHeldMailThTextBox.Text != "")
                    {
                        DominoServersObject.HeldThreshold = int.Parse(SrvAtrHeldMailThTextBox.Text);
                    }
                    DominoServersObject.ScanDBHealth = SrvAtrDBHealthCheckBox.Checked;


                    DominoServersObject.scanlog = logCheckBox.Checked;
                    DominoServersObject.scanagentlog = agentlogCheckBox.Checked;
                    DominoServersObject.NotificationGroup = SendTextBox.Text;

                    DominoServersObject.Memory_Threshold = float.Parse(AdvMemoryThTrackBar.Value.ToString()) / 100;
                    DominoServersObject.CPU_Threshold = float.Parse(AdvCPUThTrackBar.Value.ToString()) / 100;
                    if (AdvClusterRepTextBox.Text != null && AdvClusterRepTextBox.Text != "")
                    {
                        DominoServersObject.Cluster_Rep_Delays_Threshold = float.Parse(AdvClusterRepTextBox.Text.ToString());
                    }
                    if (LoadClusterRepTextBox.Text != null && LoadClusterRepTextBox.Text != "")
                    {
                        DominoServersObject.Load_Cluster_Rep_Delays_Threshold = float.Parse(LoadClusterRepTextBox.Text.ToString());
                    }
                    if (ServerDaysAlert.Text != null && ServerDaysAlert.Text != "")
                    {
                        DominoServersObject.ServerDaysAlert = int.Parse(ServerDaysAlert.Text);
                    }
                    DominoServersObject.Key = Convert.ToInt16(lblServerID.Text);
                    //8/7/2014 NS added for VSPLUS-853
					DominoServersObject.CredentialsID = (CredentialsComboBox.Text == "" ?0 : Convert.ToInt32(CredentialsComboBox.Value));
					if (CredentialsComboBox.Text == "None")
					{
						DominoServersObject.CredentialsID = (CredentialsComboBox.Text == "None" ? -1 : Convert.ToInt32(CredentialsComboBox.Value));
						//    CredentialsComboBox.Value = DBNull.Value;
						//object val= CredentialsComboBox.Value;
						
  
					}
					DominoServersObject.ScanServlet = EnableServletScan.Checked;
                    //6/18/2015 NS added for VSPLUS-1802
                    DominoServersObject.EXJEnabled = EXJournalScanCheckBox.Checked;
                    if (EXJournalLookBackCheckBox.Checked)
                    {
                        DominoServersObject.EXJLookBackEnabled = true;
                        DominoServersObject.EXJStartTime = StartTimeEdit.Text;
                        DominoServersObject.EXJDuration = Convert.ToInt32(MaintDurationTextBox.Text);
                        DominoServersObject.EXJLookBackDuration = Convert.ToInt32(LookBackPeriodTextBox.Text);
                    }
                    else
                    {
                        DominoServersObject.EXJLookBackEnabled = false;
                        DominoServersObject.EXJStartTime = "";
                        DominoServersObject.EXJDuration = 0;
                        DominoServersObject.EXJLookBackDuration = 0;
                    }
                    //2/23/2016 NS added for VSPLUS-2641
                    DominoServersObject.AvailabilityIndexThreshold = Convert.ToInt32(SrvAIThresholdTextBox.Text);
                }
                //DominoServersObject.Modified_By = int.Parse(Session["UserID"].ToString());
                //DominoServersObject.Modified_On = DateTime.Now.ToString();
                return DominoServersObject;
            }
            catch (Exception ex)
            {
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
            finally { }
        }

        private ServerTaskSettings CollectDataForServerTaskSettings(string Mode, DataRow STSettingsRow)
        {
            try
            {
                ServerTaskSettings STSettingsObject = new ServerTaskSettings();
                STSettingsObject.Enabled = Convert.ToBoolean(STSettingsRow["Enabled"]);
                DominoServerTasks DSTasksObject = new DominoServerTasks();

                DSTasksObject.TaskName = STSettingsRow["TaskName"].ToString();
                DominoServerTasks ReturnValue = VSWebBL.ConfiguratorBL.DominoServerTasksBL.Ins.GetDataForTaskName(DSTasksObject);
                STSettingsObject.TaskID = ReturnValue.TaskID;

                STSettingsObject.ServerID = ServerKey;
                STSettingsObject.SendLoadCommand = Convert.ToBoolean(STSettingsRow["SendLoadCommand"]);
                STSettingsObject.SendRestartCommand = Convert.ToBoolean(STSettingsRow["SendRestartCommand"]);
                STSettingsObject.RestartOffHours = Convert.ToBoolean(STSettingsRow["RestartOffHours"]);
                STSettingsObject.SendExitCommand = Convert.ToBoolean(STSettingsRow["SendExitCommand"]);
                STSettingsObject.Modified_By = Convert.ToInt32(Session["UserId"].ToString());
                STSettingsObject.Modified_On = DateTime.Now.ToString();

                if (Mode == "Update") STSettingsObject.MyID = Convert.ToInt32(STSettingsRow["MyID"]);

                return STSettingsObject;
            }
            catch (Exception ex)
            {
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
            finally { }
        }

        //private MaintenanceWindows CollectDataForMaintWindows(string Mode, DataRow MaintWindowsRow)
        //{
        //    try
        //    {
        //        MaintenanceWindows MaintWindowsObject = new MaintenanceWindows();
        //        MaintWindowsObject.DeviceType = "Domino";
        //        MaintWindowsObject.Name = SrvAtrSrvNameComboBox.Text;
        //        MaintWindowsObject.MaintWindow = MaintWindowsRow["MaintWindow"].ToString();
        //        return MaintWindowsObject;
        //    }
        //    catch (Exception ex)
        //    { throw ex; }
        //    finally { }
        //}

        #endregion

        protected void AdvPingTestButton_Click(object sender, EventArgs e)
        {
            try
            {
                int ResponseTime = 0;
                nsoftware.IPWorks.Ping PingControl = new nsoftware.IPWorks.Ping("31504E3641414E5852464336354235353231000000000000000000000000000000000000000000004D3047595958364A00003042394E505658333642345A0000");
                try
                {
                    PingControl.PingHost(AdvIPAddressTextBox.Text);
                    ResponseTime = PingControl.ResponseTime;
                }
                catch (Exception ex)
                {
                    ErrorMessageLabel.Text = "Ping Test Error";
                    ErrorMessagePopupControl.ShowOnPageLoad = true;
                    //6/27/2014 NS added for VSPLUS-634
                    Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                }

                if (ResponseTime > 0)
                {
                    ErrorMessageLabel.Text = PingControl.ResponseSource + " responded in " + PingControl.ResponseTime + " ms.";
                    ErrorMessagePopupControl.ShowOnPageLoad = true;
                }
                else
                {
                    ErrorMessageLabel.Text = AdvIPAddressTextBox.Text + " did not respond.";
                    ErrorMessagePopupControl.ShowOnPageLoad = true;
                }
            }
            catch (Exception ex)
            {
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                Response.Write("<script>alert('Ping Test Error')</script>");
            }
            finally { }
        }



        #endregion

        protected void SrvAtrSuggestButton_Click(object sender, EventArgs e)
        {
            //This Function returns response time in milliseconds
            //Get the passwords from the registry
            //    RegistryHandler myRegistry = new RegistryHandler();
            //    string MyDominoPassword=null;
            //Byte[] MyPass=null;
            //try
            //{
            //    MyPass = (Byte[])myRegistry.ReadFromRegistry("Password"); //  'Domino password as encrypted byte stream
            //}
            //catch
            //{ }
            ////    MyPass = myRegistry.ReadFromRegistry("Password")  'Domino password as encrypted byte stream
            ////Catch ex As Exception
            ////    MyPass = Nothing
            ////End Try

            //TripleDES mySecrets = new TripleDES();
            //try
            //{
            //    if (MyPass !=null)
            //    {
            //        MyDominoPassword = mySecrets.Decrypt(MyPass);
            //    }
            //}
            //catch
            //{
            //    MyDominoPassword = null;
            //}

            // Domino.NotesSession s =new Domino.NotesSession();

            // long start, done;
            // TimeSpan elapsed;
            // start = DateTime.Now.Ticks;
            // double ReturnValue = 0;
            // try
            // {
            //     if (MyDominoPassword != null)
            //     {
            //         s.Initialize(MyDominoPassword);
            //     }
            //     else
            //     {
            //         s.Initialize();
            //     }
            // }
            // catch (Exception)
            // {
            //     System.Runtime.InteropServices.Marshal.ReleaseComObject(s);
            //     return;
            // }

            //Domino.NotesDbDirectory dbDir ;
            //Domino.NotesDatabase db;
            //try
            //{
            //   // System.TimeSpan span;
            //    dbDir = s.GetDbDirectory(SrvAtrSrvNameComboBox.Text);
            //    if (dbDir != null)
            //    {
            //        db = null;
            //        //        MessageBox.Show("Could not contact the Domino server " + SrvAtrSrvNameComboBox.Text)
            //        ErrorMessageLabel.Text = "Could not contact the Domino server " + SrvAtrSrvNameComboBox.Text;
            //        ErrorMessagePopupControl.HeaderText = "Contact failed.";
            //        ErrorMessagePopupControl.ShowOnPageLoad = true;
            //        try
            //        {
            //           // s = null;
            //            System.Runtime.InteropServices.Marshal.ReleaseComObject(s);
            //        }
            //        catch
            //        { }
            //    }
            //    db = dbDir.GetFirstDatabase(Domino.DB_TYPES.NOTES_DATABASE);
            //    done = DateTime.Now.Ticks;
            //    elapsed = new TimeSpan(done - start);
            //    ReturnValue = elapsed.TotalMilliseconds;
            //}
            //catch (Exception)
            //{
            //    ReturnValue = 0;
            //}
            //finally
            //{
            //    db = null;
            //    dbDir = null;
            //}
            //try
            //{
            //    System.Runtime.InteropServices.Marshal.ReleaseComObject(s);
            //}
            //catch (Exception)
            //{

            //}
            //ErrorMessageLabel.Text = SrvAtrSrvNameComboBox.Text + " responded in " + ReturnValue + " milliseconds.  Suggested Alert Threshold is 5 x the current response or about " + (ReturnValue * 5).ToString() + " milliseconds.";
            //ErrorMessagePopupControl.HeaderText = "Server responded!!!";
            //ErrorMessagePopupControl.ShowOnPageLoad = true;
            //    //MessageBox.Show(SrvAtrSrvNameComboBox.Text + " responded in " + ReturnValue + " milliseconds.  Suggested Alert Threshold is 5 x the current response or about " + Format(ReturnValue * 5, "#####") + " milliseconds.");
            //    SrvAtrResponseThTextBox.Text =(ReturnValue * 5).ToString();   

        }

        protected void FormCancelButton_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("LotusDominoServers.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
                Context.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
            finally { }
        }

        protected void ValidationUpdatedButton_Click(object sender, EventArgs e)
        {
            try
            {
                //8/7/2015 NS modified for VSPLUS-2059
                //Response.Redirect("LotusDominoServers.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
                //Context.ApplicationInstance.CompleteRequest();
                ErrorMessagePopupControl.ShowOnPageLoad = false;
            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
            finally { }
        }

        //popup SerevrTask page when click on Currently Running Task Button

        protected void SrvTskCurRunTskButton_Click(object sender, EventArgs e)
        {
            try
            {
                VSFramework.RegistryHandler myRegistry = new VSFramework.RegistryHandler();
                byte[] MyPass;
                string MyDominoPassword; //should be string
                object MyObjPwd;
                VSFramework.TripleDES mySecrets = new VSFramework.TripleDES();
                try
                {
                    MyObjPwd = myRegistry.ReadFromRegistry("Password");
                    MyPass = MyObjPwd as byte[];
                }
                catch (Exception ex)
                {
                    //6/27/2014 NS added for VSPLUS-634
                    Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                    MyPass = null;
                    throw ex;
                }
                try
                {
                    if (MyPass != null)
                    {
                        MyDominoPassword = mySecrets.Decrypt(MyPass);
                    }
                    else
                    {
                        MyDominoPassword = null;
                    }
                }
                catch (Exception ex)
                {
                    //6/27/2014 NS added for VSPLUS-634
                    Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                    MyDominoPassword = "";
                    throw ex;
                }
                //8/7/2015 NS modified for VSPLUS-2059
                try
                {
                    //10/13/2014 NS added for VSPLUS-1003
                    NotesSessionObject = new Domino.NotesSession();
                    NotesSessionObject.Initialize(MyDominoPassword);
                    string taskList = "";
                    try
                    {
                        taskList = NotesSessionObject.SendConsoleCommand(SrvAtrSrvNameComboBox.Text, "show task");
                        if (taskList != "")
                        {
                            RConsolePopupControl.ShowOnPageLoad = true;
                            RConsolePopupControl.Text = "Server tasks for " + SrvAtrSrvNameComboBox.Text + ":";
                            this.RConsoleMemo.Text = taskList;
                            this.RConsoleMemo.ReadOnly = true;
                            DoneButton.Focus();
                        }
                    }
                    catch (Exception ex)
                    {
                        ErrorMessageLabel.Text = "Could not get currently running tasks from the server.";
                        ErrorMessagePopupControl.HeaderText = "Error Reading Tasks Info";
                        //ErrorMessagePopupControl.ShowCloseButton = false;
                        ValidationUpdatedButton.Visible = true;
                        ValidationOkButton.Visible = false;
                        ErrorMessagePopupControl.ShowOnPageLoad = true;
                        //6/27/2014 NS added for VSPLUS-634
                        Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                    }
                }
                catch (Exception ex)
                {
                    ErrorMessageLabel.Text = "The Domino password stored in registry is incorrect. Please reset the password using the IBM Domino Settings menu option under Stored Passwords & Options and try again.";
                    ErrorMessagePopupControl.HeaderText = "Incorrect Domino Password";
                    //ErrorMessagePopupControl.ShowCloseButton = false;
                    ValidationUpdatedButton.Visible = true;
                    ValidationOkButton.Visible = false;
                    ErrorMessagePopupControl.ShowOnPageLoad = true;
                    //6/27/2014 NS added for VSPLUS-634
                    Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                    //throw ex;
                }
                System.Runtime.InteropServices.Marshal.ReleaseComObject(NotesSessionObject);
            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
            finally { }
        }


        protected void sendmailBrowsButton_Click(object sender, EventArgs e)
        {
            //frmNotesAddressDialog NotesAddress = new frmNotesAddressDialog();
            //var _with1 = NotesAddress;
            //_with1.myDomServerCaller = this;
            //_with1.ShowDialog();
            //NotesAddress = null;


        }

        protected void DoneButton_Click(object sender, EventArgs e)
        {
            try
            {
                RConsolePopupControl.ShowOnPageLoad = false;
            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
            finally { }
        }

        protected void RefreshButton_Click(object sender, EventArgs e)
        {
            try
            {

                VSFramework.RegistryHandler myRegistry = new VSFramework.RegistryHandler();
                byte[] MyPass;
                string MyDominoPassword; //should be string
                object MyObjPwd;
                VSFramework.TripleDES mySecrets = new VSFramework.TripleDES();
                try
                {
                    MyObjPwd = myRegistry.ReadFromRegistry("Password");
                    MyPass = MyObjPwd as byte[];
                }
                catch (Exception ex)
                {
                    //6/27/2014 NS added for VSPLUS-634
                    Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                    MyPass = null;
                    throw ex;
                }
                try
                {
                    if (MyPass != null)
                    {
                        MyDominoPassword = mySecrets.Decrypt(MyPass);
                    }
                    else
                    {
                        MyDominoPassword = null;
                    }
                }
                catch (Exception ex)
                {
                    //6/27/2014 NS added for VSPLUS-634
                    Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                    MyDominoPassword = "";
                    throw ex;
                }
                try
                {
                    NotesSessionObject.Initialize(MyDominoPassword);
                    RConsoleMemo.Text = NotesSessionObject.SendConsoleCommand(SrvAtrSrvNameComboBox.Text, "show task");
                }
                catch (Exception ex)
                {
                    ErrorMessageLabel.Text = "The Domino password stored in registry is incorrect. Please reset the password using the IBM Domino Settings menu option under Stored Passwords & Options and try again.";
                    ErrorMessagePopupControl.HeaderText = "Incorrect Domino Password";
                    //ErrorMessagePopupControl.ShowCloseButton = false;
                    ValidationUpdatedButton.Visible = true;
                    ValidationOkButton.Visible = false;
                    ErrorMessagePopupControl.ShowOnPageLoad = true;
                    //throw ex;
                    //6/27/2014 NS added for VSPLUS-634
                    Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                }
                System.Runtime.InteropServices.Marshal.ReleaseComObject(NotesSessionObject);

            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
            finally { }
        }

        protected void SrvTskDSTaskSettingsGridView_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            try
            {

                e.Cell.ToolTip = string.Format("{0}", e.CellValue);
            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
            finally { }
        }

        #region NotesDBAddressCode



        //protected void SelectButton_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if ((MailListBox.SelectedIndex != null))
        //        {
        //            SendTextBox.Text = MailListBox.SelectedItem.ToString();
        //        }

        //        //if ((myAlertCaller != null))
        //        //{
        //        //    myAlertCaller.txtAddress.Text = ListBox1.Items(ListBox1.SelectedIndex);
        //        //}

        //        //if ((myBESServerCaller != null))
        //        //{
        //        //    myBESServerCaller.txtAlertNotificationGroup.Text = ListBox1.Items(ListBox1.SelectedIndex);
        //        //}

        //        //if ((myDomServerCaller != null))
        //        //{
        //        //    myDomServerCaller.txtAlertNotificationGroup.Text = ListBox1.Items(ListBox1.SelectedIndex);
        //        //}
        //    }
        //    catch (Exception ex)
        //    {
        //        return;
        //    }

        //    try
        //    {
        //        System.Runtime.InteropServices.Marshal.ReleaseComObject(doc);

        //    }
        //    catch (Exception ex)
        //    {
        //    }

        //    try
        //    {
        //        System.Runtime.InteropServices.Marshal.ReleaseComObject(doc);

        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //    try
        //    {
        //        System.Runtime.InteropServices.Marshal.ReleaseComObject(doc);

        //    }
        //    catch (Exception ex)
        //    {
        //    }

        //    SendMailPopupControl.ShowOnPageLoad = false;

        //}

        //protected void SearchButton_Click(object sender, EventArgs e)
        //{
        //    if (string.IsNullOrEmpty(SearchTextBox.Text))
        //        return;
        //    if (this.SearchTextBox.Text.Length < 2)
        //        return;
        //    try
        //    {
        //        //Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
        //        this.MailListBox.Items.Clear();
        //        this.MailListBox.Items.Add("Retrieving names....");
        //       // this.Invalidate();
        //        //Application.DoEvents();

        //    }
        //    catch (Exception ex)
        //    {
        //    }

        //    bool match = false;



        //    Domino.NotesView view = default(Domino.NotesView);
        //    try
        //    {
        //        // strHomeServer = s.GetEnvironmentString("MailServer")
        //        db = s.GetDatabase(MyHomeServer, "names.nsf");
        //        view = db.GetView("$peoplegroupsflat");
        //        Domino.NotesDocumentCollection coll = default(Domino.NotesDocumentCollection);
        //        coll = view.GetAllDocumentsByKey(this.SearchTextBox.Text);
        //        doc = coll.GetFirstDocument();

        //        this.MailListBox.Items.Clear();
        //        Domino.NotesName myName = default(Domino.NotesName);
        //        string MyCase = doc.GetItemValue("Type")(0);

        //        while ((doc != null))
        //        {
        //            switch (MyCase)
        //            {
        //                case "Server":
        //                    break;
        //                //        Me.ListBox1.Items.Add(doc.GetItemValue("ServerName")(0))
        //                case "Person":
        //                    try
        //                    {
        //                        //Create a hierarchical name
        //                        myName = s.CreateName(doc.GetItemValue("Fullname")(0));
        //                        this.MailListBox.Items.Add(myName.Abbreviated);
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                    }

        //                    break;
        //                case "Group":
        //                    this.MailListBox.Items.Add(doc.GetItemValue("ListName")(0));
        //                    break;
        //                case "Database":
        //                    break;
        //                // Me.ListBox1.Items.Add(doc.GetItemValue("Fullname")(0))
        //                default:

        //                    break;
        //            }

        //            doc = coll.GetNextDocument(doc);
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //      //  Cursor.Current = System.Windows.Forms.Cursors.Default;
        //    }
        //  //  Cursor.Current = System.Windows.Forms.Cursors.Default;
        //   // this.Invalidate();
        //    //Application.DoEvents();

        //}

        //protected void CancelButton_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        System.Runtime.InteropServices.Marshal.ReleaseComObject(doc);

        //    }
        //    catch (Exception ex)
        //    {
        //    }

        //    try
        //    {
        //        System.Runtime.InteropServices.Marshal.ReleaseComObject(doc);

        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //    try
        //    {
        //        System.Runtime.InteropServices.Marshal.ReleaseComObject(doc);
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //    SendMailPopupControl.ShowOnPageLoad = false;

        //}

        //protected void SearchTextBox_TextChanged(object sender, EventArgs e)
        //{
        //    if (SearchTextBox.Text.Length > 1)
        //    {
        //        this.SearchButton.Enabled = true;
        //    }
        //    else
        //    {
        //        this.SearchButton.Enabled = false;
        //    }
        //}



        #endregion



        private void FillMaintenanceGrid()
        {
            try
            {

                DataTable MaintDataTable = new DataTable();
                DataSet ServersDataSet = new DataSet();
                MaintDataTable = VSWebBL.ConfiguratorBL.MaintenanceBL.Ins.GetMaintenDataOnServerID(SrvAtrSrvNameComboBox.Text);
                if (MaintDataTable.Rows.Count > 0)
                {
                    DataTable dtcopy = MaintDataTable.Copy();
                    dtcopy.PrimaryKey = new DataColumn[] { dtcopy.Columns["ID"] };
                    //4/2/2014 NS modified for VSPLUS-138
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
                //4/2/2014 NS modified for VSPLUS-138
                //if (Session["MaintServers"] != "" && Session["MaintServers"]!=null)
                //ServersDataTable = (DataTable)Session["MaintServers"];//VSWebBL.ConfiguratorBL.DominoPropertiesBL.Ins.GetAllData();
                if (ViewState["MaintServers"] != "" && ViewState["MaintServers"] != null)
                    ServersDataTable = (DataTable)ViewState["MaintServers"];//VSWebBL.ConfiguratorBL.DominoPropertiesBL.Ins.GetAllData();
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

        private void FillDiskGridfromSession()
        {
            try
            {

                DataTable ServersDataTable = new DataTable();
                if (Session["DiskDataTable"] != "" && Session["DiskDataTable"] != null)
                    ServersDataTable = (DataTable)Session["DiskDataTable"];//VSWebBL.ConfiguratorBL.DominoPropertiesBL.Ins.GetAllData();
                //5/2/2014 NS modified for VSPLUS-602
                if (ServersDataTable.Rows.Count > 0 && SelCriteriaRadioButtonList.SelectedItem.Value.ToString() == "1")
                {
                    //5/2/2014 NS added for VSPLUS-602
                    GridViewDataColumn column1 = DiskGridView.Columns["Threshold"] as GridViewDataColumn;
                    GridViewDataColumn column2 = DiskGridView.Columns["ThresholdType"] as GridViewDataColumn;
					GridViewDataColumn column3 = DiskGridView.Columns["DiskInfo"] as GridViewDataColumn;
                    DataTable dt = new DataTable();
                    for (int i = 0; i < DiskGridView.VisibleRowCount; i++)
                    {
                        if (DiskGridView.Selection.IsRowSelected(i))
                        {
                            ASPxTextBox txtThreshold = (ASPxTextBox)DiskGridView.FindRowCellTemplateControl(i, column1, "txtFreeSpaceThresholdValue");
                            ASPxComboBox txtThresholdType = (ASPxComboBox)DiskGridView.FindRowCellTemplateControl(i, column2, "txtFreeSpaceThresholdType");
							ASPxMemo txtDiskInfo = (ASPxMemo)DiskGridView.FindRowCellTemplateControl(i, column3, "DiskInfo");
							string diskcomment = txtDiskInfo.Text;
							string diskscomments=diskcomment.Replace("'","\"");

							ServersDataTable.Rows[i]["Threshold"] = txtThreshold.Text;
                            ServersDataTable.Rows[i]["ThresholdType"] = txtThresholdType.SelectedItem.Text;
							if (txtDiskInfo != null)
							{
								ServersDataTable.Rows[i]["DiskInfo"] = diskscomments;
							}
                        }
                    }
                    DiskGridView.DataSource = ServersDataTable;
                    DiskGridView.DataBind();
                }
            }
            catch (Exception ex)
            {
                errorDiv.Style.Value = "display: block;";
                //10/3/2014 NS modified for VSPLUS-990
                errorDiv.InnerHtml = "One of the selected row values is incorrect: threshold values must be numeric and threshold type must be set for each selected disk." +
                    "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
            }
        }
        private void FillAlertGridView()
        {
            try
            {

                DataTable AlertDataTable = new DataTable();
                DataSet AlertDataSet = new DataSet();
                AlertDataTable = VSWebBL.ConfiguratorBL.AlertsBL.Ins.GetAlertTab(SrvAtrSrvNameComboBox.Text, "Domino");
                if (AlertDataTable.Rows.Count > 0)
                {
                    DataTable dtcopy = AlertDataTable.Copy();
                    // dtcopy.PrimaryKey = new DataColumn[] { dtcopy.Columns["ID"] };
                    //4/2/2014 NS modified for VSPLUS-138
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
                //4/2/2014 NS modified for VSPLUS-138
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
        protected void SrvTskGrdDisallowCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ASPxCheckBox SrvTskGrdDisallowCheckBox = (ASPxCheckBox)SrvTskDSTaskSettingsGridView.FindEditFormTemplateControl("SrvTskGrdDisallowCheckBox");
                ASPxCheckBox SrvTskGrdLoadCheckBox = (ASPxCheckBox)SrvTskDSTaskSettingsGridView.FindEditFormTemplateControl("SrvTskGrdLoadCheckBox");
                ASPxCheckBox SrvTskGrdRestartOffCheckBox = (ASPxCheckBox)SrvTskDSTaskSettingsGridView.FindEditFormTemplateControl("SrvTskGrdRestartOffCheckBox");
                ASPxCheckBox SrvTskGrdRestartASAPCheckBox = (ASPxCheckBox)SrvTskDSTaskSettingsGridView.FindEditFormTemplateControl("SrvTskGrdRestartASAPCheckBox");
                if (SrvTskGrdDisallowCheckBox.Checked == true)
                {
                    SrvTskGrdLoadCheckBox.Checked = false;
                    SrvTskGrdRestartOffCheckBox.Checked = false;
                    SrvTskGrdRestartASAPCheckBox.Checked = false;
                    SrvTskGrdLoadCheckBox.Enabled = false;
                    SrvTskGrdRestartOffCheckBox.Enabled = false;
                    SrvTskGrdRestartASAPCheckBox.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
            finally { }
        }

        protected void AdvMemoryThTrackBar_PositionChanged(object sender, EventArgs e)
        {
            try
            {

                MemRoundPanel9.HeaderText = "Memory Usage Alert at " + AdvMemoryThTrackBar.Value + "% utilization";
            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
            finally { }
        }

        protected void AdvDiskSpaceThTrackBar_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                //12/17/2013 NS modified
                //DiskRoundPanel7.HeaderText = "Disk Space Alert at  " + AdvDiskSpaceThTrackBar.Value + "% free space";
                DiskLabel.Text = "Current threshold: " + AdvDiskSpaceThTrackBar.Value + "% free space";
            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
            finally { }
        }

        protected void AdvCPUThTrackBar_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                CPURoundPanel.HeaderText = "CPU Utilization Alert at " + AdvCPUThTrackBar.Value + "% utilization";
            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
            finally { }
        }

        protected void MaintWinListGridView_SelectionChanged(object sender, EventArgs e)
        {
            try
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
                        //Response.Redirect("MaintenanceWin.aspx?ID=" + ID + "",true);
                    }


                }
            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
            finally { }
        }

        protected void MaintWinListGridView_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
        {
            try
            {
                e.Row.Attributes.Add("onmouseover", "this.originalcolor=this.style.backgroundColor;" + "this.style.backgroundColor='#C0C0C0';");

                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalcolor;");
            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
            finally { }
        }

        protected void ToggleVeiwButton_Click(object sender, EventArgs e)
        {
            try
            {
                //DataTable
                if (ToggleVeiwButton.Text == "Switch to Calendar view")
                {
                    MaintWinListGridView.Visible = false;
                    DataTable MaintCalanderDataTable = VSWebBL.ConfiguratorBL.MaintenanceBL.Ins.GetMaintenCalDataOnServerID(SrvAtrSrvNameComboBox.Text);
                    Session["calanderdata"] = MaintCalanderDataTable;
                    ASPxScheduler1.AppointmentDataSource = MaintCalanderDataTable;
                    ASPxScheduler1.DataBind();
                    ASPxScheduler1.Visible = true;
                    ToggleVeiwButton.Text = "Switch to Grid View";
                }
                else if (ToggleVeiwButton.Text == "Switch to Grid View")
                {
                    ASPxScheduler1.Visible = false;
                    MaintWinListGridView.Visible = true;
                    FillMaintServersGridfromSession();
                    ToggleVeiwButton.Text = "Switch to Calendar view";
                }
            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
            finally { }
        }
        public void FillCalanderdatafromSession()
        {
            try
            {
                if (Session["calanderdata"] != null && Session["calanderdata"] != "")
                {
                    ASPxScheduler1.AppointmentDataSource = (DataTable)Session["calanderdata"];
                    ASPxScheduler1.DataBind();
                }
            }
            catch (Exception ex)
            {

                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
            finally { }
        }

        protected void SrvTskDSTaskSettingsGridView_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        {
            try
            {

                if (e.Column.FieldName == "TaskName")
                {
                    ASPxComboBox TaskNameComboBox = e.Editor as ASPxComboBox;
                    FillTaskNameComboBox(TaskNameComboBox);
                    TaskNameComboBox.Callback += new CallbackEventHandlerBase(TaskNameComboBox_OnCallback);
                }
            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
            finally { }
        }

        protected void SrvTskDSTaskSettingsGridView_AutoFilterCellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        {
            try
            {
                if (e.Column.FieldName == "TaskName")
                {
                    ASPxComboBox TaskNameComboBox = e.Editor as ASPxComboBox;
                    FillTaskNameComboBox(TaskNameComboBox);
                    TaskNameComboBox.Callback += new CallbackEventHandlerBase(TaskNameComboBox_OnCallback);
                }
            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
            finally { }
        }

        protected void DiskGridView_PreRender(object sender, EventArgs e)
        {
            try
            {
                //5/1/2014 NS added for VSPLUS-427
                if (isValid)
                {
                    //12/17/2013 NS modified
                    //5/12/2014 NS modified for VSPLUS-615
                    //if (SelCriteriaRadioButtonList.SelectedIndex == 1)
                    if (SelCriteriaRadioButtonList.SelectedItem.Value.ToString() == "1")
                    //if (rdbSelFew.Checked)
                    {
                        ASPxGridView gridView = (ASPxGridView)sender;
                        for (int i = 0; i < gridView.VisibleRowCount; i++)
                        {

                            gridView.Selection.SetSelection(i, (int)gridView.GetRowValues(i, "isSelected") == 1);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
            finally { }
        }
        protected void Page_Init(object sender, EventArgs e)
        {
            try
            {
                //if (!hf.Contains("state") ||
                //   hf["state"].ToString() != "edit")
                //    return;

                ReadOnlyTemplate template = new ReadOnlyTemplate();

                (DiskGridView.Columns["DiskName"] as GridViewDataColumn).EditItemTemplate = template;
            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
            finally { }
        }

        protected void DiskGridView_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            try
            {
                ASPxGridView gridView = (ASPxGridView)sender;

                DataTable DiskDataTable = (DataTable)Session["DiskDataTable"];

                DataRow dr = GetDiskRow(DiskDataTable, e.NewValues.GetEnumerator(), e.Keys[0].ToString());//Convert.ToInt32(e.Keys[0]));



                //gridView.UpdateEdit();
                Session["DiskDataTable"] = DiskDataTable;

                FillDiskGridfromSession();

                gridView.CancelEdit();
                e.Cancel = true;
            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
            finally { }
        }

        protected DataRow GetDiskRow(DataTable UserObject, IDictionaryEnumerator enumerator, string Keys)// int Keys)
        {
            try
            {
                DataTable dataTable = UserObject;
                DataRow DRRow = dataTable.NewRow();
                if (Keys == "0")
                    DRRow = dataTable.NewRow();
                else
                {
                    DataRow[] SelDRRow = dataTable.Select("DiskName='" + Keys + "'");
                    DRRow = SelDRRow[0];
                }
                //DRRow = dataTable.Rows.Find(Keys);
                //IDictionaryEnumerator enumerator = e.NewValues.GetEnumerator();
                enumerator.Reset();
                while (enumerator.MoveNext())
                    DRRow[enumerator.Key.ToString()] = (enumerator.Value == null ? "False" : enumerator.Value);
                return DRRow;
            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
            finally { }
        }

        //12/17/2013 NS modified
        /*
        protected void rdbSelAll_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbSelAll.Checked)
            {
                for (int i = 0; i < DiskGridView.VisibleRowCount; i++)
                {
                    DiskGridView.Selection.SetSelection(i, false);
                }
                SelectDisksRoundPanel.Enabled = false;
            }
        }
         */

        protected void SelCriteriaRadioButtonList_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //5/12/2014 NS modified for VSPLUS-615
                if (SelCriteriaRadioButtonList.SelectedItem.Value.ToString() == "0")
                {
                    AdvDiskSpaceThTrackBar.Visible = true;
                    Label4.Visible = true;
                    DiskLabel.Visible = true;
                    DiskLabel.Text = "Current threshold: " + AdvDiskSpaceThTrackBar.Value + "% free space";
                    DiskGridView.Visible = false;
                    DiskGridInfo.Visible = false;
                    for (int i = 0; i < DiskGridView.VisibleRowCount; i++)
                    {
                        DiskGridView.Selection.SetSelection(i, false);
                    }
                    //1/31/2014 NS added for VSPLUS-289
                    infoDiskDiv.Style.Value = "display: none";
                    //5/12/2014 NS added for VSPLUS-615
                    GBLabel.Visible = false;
                    GBTextBox.Visible = false;
                    GBTitle.Visible = false;
                }
                else if (SelCriteriaRadioButtonList.SelectedItem.Value.ToString() == "1")
                {
                    DiskGridView.Visible = true;
                    DiskGridInfo.Visible = true;
                    AdvDiskSpaceThTrackBar.Visible = false;
                    Label4.Visible = false;
                    DiskLabel.Visible = false;
                    //1/31/2014 NS added for VSPLUS-289
                    infoDiskDiv.Style.Value = "display: block";
                    //5/12/2014 NS added for VSPLUS-615
                    GBLabel.Visible = false;
                    GBTextBox.Visible = false;
                    GBTitle.Visible = false;
                }
                else if (SelCriteriaRadioButtonList.SelectedItem.Value.ToString() == "2")
                {
                    AdvDiskSpaceThTrackBar.Visible = false;
                    Label4.Visible = false;
                    DiskLabel.Visible = false;
                    DiskGridView.Visible = false;
                    DiskGridInfo.Visible = false;
                    for (int i = 0; i < DiskGridView.VisibleRowCount; i++)
                    {
                        DiskGridView.Selection.SetSelection(i, false);
                    }
                    //1/31/2014 NS added for VSPLUS-289
                    infoDiskDiv.Style.Value = "display: none";
                    //5/12/2014 NS added for VSPLUS-615
                    GBLabel.Visible = false;
                    GBTextBox.Visible = false;
                    GBTitle.Visible = false;
                }
                //5/12/2014 NS added for VSPLUS-615
                else if (SelCriteriaRadioButtonList.SelectedItem.Value.ToString() == "3")
                {
                    AdvDiskSpaceThTrackBar.Visible = false;
                    Label4.Visible = true;
                    DiskLabel.Visible = false;
                    DiskGridView.Visible = false;
                    DiskGridInfo.Visible = false;
                    for (int i = 0; i < DiskGridView.VisibleRowCount; i++)
                    {
                        DiskGridView.Selection.SetSelection(i, false);
                    }
                    infoDiskDiv.Style.Value = "display: none";
                    GBLabel.Visible = true;
                    GBTextBox.Visible = true;
                    GBTitle.Visible = true;
                }
            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
            finally { }
        }

        //12/17/2013 NS modified
        /*
        protected void rdbNoAlerts_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbNoAlerts.Checked)
            {
                for (int i = 0; i < DiskGridView.VisibleRowCount; i++)
                {
                    DiskGridView.Selection.SetSelection(i, false);
                }
                SelectDisksRoundPanel.Enabled = false;
            }
        }
        */

        //12/17/2013 NS commented out
        /*
        protected void rdbSelFew_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbSelFew.Checked)
            {
                SelectDisksRoundPanel.Enabled = true;
            }
        }
        */

        protected void DiskGridView_PageSizeChanged(object sender, EventArgs e)
        {
            try
            {
                //ProfilesGridView.PageIndex;
                VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("DominoProperties|DiskGridView", DiskGridView.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
                Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
            finally { }
        }
        protected void MaintWinListGridView_PageSizeChanged(object sender, EventArgs e)
        {
            try
            {
                //ProfilesGridView.PageIndex;
                VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("DominoProperties|MaintWinListGridView", MaintWinListGridView.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
                Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
            finally { }
        }
        protected void AlertGridView_PageSizeChanged(object sender, EventArgs e)
        {
            try
            {
                //ProfilesGridView.PageIndex;
                VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("DominoProperties|AlertGridView", AlertGridView.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
                Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
            finally { }
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
            if (Request.QueryString["key"] != "" && Request.QueryString["key"] != null)
            {
                dt = (VSWebBL.SecurityBL.ServersBL.Ins.GetServerDetailsByID(Convert.ToInt32(Request.QueryString["key"])));
                if (dt.Rows.Count > 0)
                {
                    Response.Redirect("~/Dashboard/DominoServerDetailsPage2.aspx?Name=" + dt.Rows[0]["ServerName"].ToString() + "&Type=" + dt.Rows[0]["Type"].ToString() + "&Status=" + dt.Rows[0]["Status"].ToString() + "&LastDate=" + dt.Rows[0]["LastUpdate"].ToString(), false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
                    Context.ApplicationInstance.CompleteRequest();
                }
            }


        }

        protected void SrvTskDSTaskSettingsGridView_PageSizeChanged(object sender, EventArgs e)
        {
            try
            {
                //ProfilesGridView.PageIndex;
                VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("DominoProperties|SrvTskDSTaskSettingsGridView", SrvTskDSTaskSettingsGridView.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
                Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
            }

            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
            finally { }
        }
        class ReadOnlyTemplate : ITemplate
        {
            public void InstantiateIn(Control _container)
            {

                try
                {
                    GridViewEditItemTemplateContainer container = _container as GridViewEditItemTemplateContainer;

                    ASPxLabel lbl = new ASPxLabel();
                    lbl.ID = "lbl";

                    container.Controls.Add(lbl);
                    lbl.Text = container.Text;
                }
                catch (Exception ex)
                {
                    Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                    throw ex;
                }
                finally { }
            }
        }

        protected void AlertGridView_CustomColumnDisplayText(object sender, ASPxGridViewColumnDisplayTextEventArgs e)
        {
            if (e.Column.FieldName == "Day")
            {
                //3/26/2015 NS added for DevEx upgrade to 14.2
                e.EncodeHtml = false;
                if (e.Value.ToString().Trim() != "")
                {
                    e.DisplayText = string.Join(",", e.Value.ToString().Split(',').Select(str => str.Trim() == "" ? str.Trim() : str.Trim().Substring(0, 2)));
                }
            }
        }
		protected void OKCopy_Click(object sender, EventArgs e)
		{
			bool check = false;
			Credentials Csibject = new Credentials();

			Csibject.AliasName = AliasName.Text;

			//DataTable returntable = VSWebBL.ConfiguratorBL.Businesshoursbl.Ins.GetName(Csibject);
			DataTable returntable = VSWebBL.SecurityBL.webspehereImportBL.Ins.GetAliasName(Csibject);
			string rawpass = Password.Text;
			byte[] encryptedpass = encryptkey.Encrypt(rawpass);
			string encryptedpasasstring=string.Join(", " ,encryptedpass.Select(s	=> s.ToString()).ToArray());


			if (returntable.Rows.Count > 0)
			{

				Div3.InnerHtml = "This Alias already exists. Enter another one." +
					"<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
				Div3.Style.Value = "display: block";

			}
			else
			{
				check = VSWebBL.SecurityBL.webspehereImportBL.Ins.Insertpwd(AliasName.Text, UserID.Text, encryptedpasasstring, 1);
				Response.Redirect("~/Configurator/DominoProperties.aspx?key=" + Session["Key"]);
			}

		}
		protected void Cancel_Click(object sender, EventArgs e)
		{
			Response.Redirect("~/Configurator/DominoProperties.aspx?key=" + Session["Key"]);

		}
		protected void btn_clickcopyprofile(object sender, EventArgs e)
		{

			CopyProfilePopupControl.ShowOnPageLoad = true;
			UserID.Visible = true;
			OKCopy.Visible = true;
			Cancel.Visible = true;
			Password.Visible = true;


		}//popup
        protected void AlertGridView_CustomUnboundColumnData(object sender, ASPxGridViewColumnDataEventArgs e)
        {
            if (e.Column.FieldName == "SendToAll")
            {
                string sendTo = (string)e.GetListSourceFieldValue("SendTo");
                //string smsTo = (string)e.GetListSourceFieldValue("SMSTo");
                //string scriptName = (string)e.GetListSourceFieldValue("ScriptName");
                if (sendTo != "")
                {
                    e.Value = sendTo;
                }
                //else if (smsTo != "")
                //{
                //    e.Value = smsTo + " (SMS)";
                //}
                //else if (scriptName != "")
                //{
                //    e.Value = scriptName + " (script)";
                //}
            }
        }

		protected void EnableTravelerCheckBox_CheckedChanged(object sender, EventArgs e)
		{

		}

		protected void CredentialsComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			//if (CredentialsComboBox.SelectedItem.Text == "None")
			//{
			//    ASPxButton1.Visible = true;
			//    ASPxButton1.CausesValidation = true;
				
			//}
			//else
			//{
			//    ASPxButton1.Visible = true;
			//    ASPxButton1.CausesValidation = true;
			//}

		}

        protected void StartTimeEdit_ValueChanged(object sender, EventArgs e)
        {
            if (StartTimeEdit.Value != null && StartTimeEdit.Text != "")
            {
                lblEndTime.Text = Convert.ToDateTime(StartTimeEdit.Value.ToString()).AddMinutes(Convert.ToDouble(MaintDurationTextBox.Text)).ToString("h:mm tt");
                lblEndTime.Visible = true;
                lblDuration.Text = DurationHoursMins(DurationTrackBar.Value.ToString());
            }
        }

        protected string DurationHoursMins(string period)
        {
            int time = 0;
            time = Convert.ToInt32(period);
            string duration = "";
            if ((time / 60) > 0)
			{//2/8/2016 Durga Modified for VSPLUS-2580
                if (time % 60 > 0)
                {
                    duration = (time / 60) + " hour(s)  " + (time % 60) + " minutes";
                }
                else
                {
                    duration = (time / 60) + " hour(s)";
                }
            }
            else
            {
                duration = (time % 60) + " minutes";
            }
            return duration;
        }

        protected void DurationTrackBar_PositionChanged(object sender, EventArgs e)
        {
            MaintDurationTextBox.Text = DurationTrackBar.Value.ToString();
            if (StartTimeEdit.Value != null)
            {
                lblEndTime.Text = Convert.ToDateTime(StartTimeEdit.Value.ToString()).AddMinutes(Convert.ToDouble(DurationTrackBar.Value)).ToString("h:mm tt");
            }
            lblDuration.Text = DurationHoursMins(DurationTrackBar.Value.ToString());
        }

        protected void EXJournalScanCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (EXJournalScanCheckBox.Checked)
            {
                EXJournalLookBackCheckBox.ClientVisible = true;
            }
            else
            {
                EXJournalLookBackCheckBox.ClientVisible = false;
                EXJournalLookBackCheckBox.Checked = false;
            }
            ShowHideLookBack();
        }

        protected void EXJournalLookBackCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            ShowHideLookBack();
        }

        public void ShowHideLookBack()
        {
            if (EXJournalLookBackCheckBox.Checked)
            {
                lblStartTime.ClientVisible = true;
                StartTimeEdit.ClientVisible = true;
                lblEndTime1.ClientVisible = true;
                lblEndTime.ClientVisible = true;
                lblDuration1.ClientVisible = true;
                MaintDurationTextBox.ClientVisible = true;
                lblDuration.ClientVisible = true;
                DurationTrackBar.ClientVisible = true;
                lblLookBack.ClientVisible = true;
                LookBackPeriodTextBox.ClientVisible = true;
                lblMin.ClientVisible = true;
            }
            else
            {
                lblStartTime.ClientVisible = false;
                StartTimeEdit.ClientVisible = false;
                lblEndTime1.ClientVisible = false;
                lblEndTime.ClientVisible = false;
                lblDuration1.ClientVisible = false;
                MaintDurationTextBox.ClientVisible = false;
                lblDuration.ClientVisible = false;
                DurationTrackBar.ClientVisible = false;
                lblLookBack.ClientVisible = false;
                LookBackPeriodTextBox.ClientVisible = false;
                lblMin.ClientVisible = false;
            }
        }
    }
}



  //private void BindGrid(ASPxGridView grid,DataTable dt)
  // {
  //  if (dt.Rows.Count > 0)
  //  {
  //      DataTable dtcopy = dt.Copy();
  //      dtcopy.PrimaryKey = new DataColumn[] { dtcopy.Columns["MyID"] };
  //      ds.Tables.Clear();
  //      ds.Tables.Add(dtcopy);
  //      Session["STSettingsDataSet"] = ds;
  //      grid.DataSource = ds.Tables[0];//DSTaskSettingsDataTable;
  //      grid.DataBind();
  //  }
  //}

    

/// <summary>
        /// Do tasks when grid is prepared for updating
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
//protected void SrvTskDSTaskSettingsGridView_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        //{
        //    if (e.Column.FieldName != "TaskName") return;
        //    ASPxComboBox TaskNameComboBox = e.Editor as ASPxComboBox;
        //    FillTaskNameComboBox(TaskNameComboBox);
        //    TaskNameComboBox.Callback += new CallbackEventHandlerBase(TaskNameComboBox_OnCallback);
        //}

//STSettingsDataSet = (DataSet)Session["STSettingsDataSet"];
//ASPxCheckBox SrvTskGrdEnabledCheckBox = (ASPxCheckBox)gridView.FindEditFormTemplateControl("SrvTskGrdEnabledCheckBox");
////ASPxMemo memo = pageControl.FindControl("notesEditor") as ASPxMemo;
//ASPxComboBox SrvTskGrdTaskComboBox = (ASPxComboBox)gridView.FindEditFormTemplateControl("SrvTskGrdTaskComboBox");
//ASPxCheckBox SrvTskGrdLoadCheckBox = (ASPxCheckBox)gridView.FindEditFormTemplateControl("SrvTskGrdLoadCheckBox");
//ASPxCheckBox SrvTskGrdRestartOffCheckBox = (ASPxCheckBox)gridView.FindEditFormTemplateControl("SrvTskGrdRestartOffCheckBox");
//ASPxCheckBox SrvTskGrdRestartASAPCheckBox = (ASPxCheckBox)gridView.FindEditFormTemplateControl("SrvTskGrdRestartASAPCheckBox");
//ASPxCheckBox SrvTskGrdDisallowCheckBox = (ASPxCheckBox)gridView.FindEditFormTemplateControl("SrvTskGrdDisallowCheckBox");
//DataTable dataTable = STSettingsDataSet.Tables[0];
//DataRow STSettingsRow = dataTable.NewRow();
////STSettingsRow=GetRow(STSettingsDataSet, e.NewValues.GetEnumerator());
//DataRow DRRow = dataTable.NewRow();
//DRRow["Enabled"] = SrvTskGrdEnabledCheckBox.Checked;
//DRRow["Taskname"] = SrvTskGrdTaskComboBox.Value;
//DRRow["SendLoadCommand"] = SrvTskGrdLoadCheckBox.Checked;
//DRRow["RestartOffHours"] = SrvTskGrdRestartOffCheckBox.Checked;
//DRRow["SendRestartCommand"] = SrvTskGrdRestartASAPCheckBox.Checked;
//DRRow["SendExitCommand"] = SrvTskGrdDisallowCheckBox.Checked;
