using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VSWebDO;
using VSWebUI;
using System.Data;
using System.Globalization;
using System.IO;
using DevExpress.Web;

namespace VSWebUI.Configurator
{
    public partial class EditNotes : System.Web.UI.Page
    {
        int ID;
        string Mode;
        string Serverkeys;
        string[] words;
        int myIndex = 0;
        string myServers = "";
        Domino.NotesSession NotesSessionObject = new Domino.NotesSession();
        string DBTitle;
        string DBFile;
        bool flag = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            //code added on 2nd june2012
            ServerListBox.Enabled = false;
            InitiateRepCheckBox.Visible = false;
            ServerListBox.Visible = false;
            GBLabel.Visible = false;
            ServerLabel.Visible = false;


            if (!IsPostBack)
            {

                FillTaskNameComboBox();
                FillListBox();
                
                if (Session["UserPreferences"] != null)
                {
                    DataTable UserPreferences = (DataTable)Session["UserPreferences"];
                    foreach (DataRow dr in UserPreferences.Rows)
                    {
                        if (dr[1].ToString() == "EditNotes|SelectNDGridView")
                        {
                            SelectNDGridView.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                        }
                    }
                }
            }
            else
            {
                //Grid needs to be bound each time
                if (SelectNotesPopupControl.ShowOnPageLoad && ViewState["NotesDB"] != null && ViewState["NotesDB"] != "")
                {
                    GridViewDataColumn column = new GridViewDataColumn();
                    DataTable ND = (DataTable)ViewState["NotesDB"];
                    if (ND.Rows.Count > 0)
                    {
                        SelectNDGridView.Columns.Clear();
                        SelectNDGridView.DataSource = ND;
                        SelectNDGridView.KeyFieldName = "ReplicaID";
                        for (int i = 0; i < ND.Columns.Count; i++)
                        {
                            column = new GridViewDataColumn(ND.Columns[i].ColumnName);
                            if (ND.Columns[i].ColumnName == "ReplicaID")
                            {
                                column.Visible = false;
                            }
                            else
                            {
                                column.Settings.AutoFilterCondition = AutoFilterCondition.Contains;
                            }
                            SelectNDGridView.Columns.Add(column);
                        }
                        SelectNDGridView.DataBind();
                    }
                }    
            }

            if (Request.QueryString["ID"] != null)
            {
                Mode = "Update";
                ID = int.Parse(Request.QueryString["ID"]);
                if (!IsPostBack)
                {

                    FillData(ID);
                    //11/19/2014 NS modified
                    //NotesDatabasesRoundPanel.HeaderText = "Notes Databases -" + " " + NameTextBox.Text;
                    servernamelbldisp.InnerHtml = "Notes Database -" + " " + NameTextBox.Text;
                }

            }
            else
            {
                Mode = "Insert";
                if (!IsPostBack)
                {
                    FillData(0);
                }
            }

        }
        private void FillData(int ID)
        {
            try
            {
                if (ID > 0)
                {
                    NotesDatabases NDObject = new NotesDatabases();
                    NDObject.ID = ID;

                    NDObject = VSWebBL.ConfiguratorBL.NotesDatabaseBL.Ins.GetDataOnID(NDObject);
                    //ND profile
                    NameTextBox.Text = NDObject.Name;
                    DominoServerComboBox.Text = NDObject.ServerName;
                    DBFileNameTextBox.Text = NDObject.FileName;
                    //Scan Settings
                    RetryIntervalTextBox.Text = NDObject.RetryInterval.ToString();
                    OffHoursTextBox.Text = NDObject.OffHoursScanInterval.ToString();
                    ScanIntervalTextBox.Text = NDObject.ScanInterval.ToString();
                    EnabledCheckBox.Checked = NDObject.Enabled;

                    //Type Roundpanel
                   
                    TriggerValueTextBox.Text = NDObject.ResponseThreshold.ToString();
                    InitiateRepCheckBox.Checked = NDObject.InitiateReplication;
                    AlertTypeComboBox.Text = NDObject.Category.ToString();
                    //11/19/2014 NS modified
                    /*
                    if (TriggerValueTextBox.Text != "0" && AlertTypeComboBox.Text != "Replication")
                    {
                        TriggerValueTextBox.Visible = true;
                    }
                     */
                    DisplayLabels();

                    // string [] lines = new string[ServerListBox.Items.Count];

                    // for(int i=0;i<ServerListBox.Items.Count; i++)
                    // {

                    ////lines[i] = lb.GetItemText(lb.Items[i]); // this could be safer if you're using other objects than strings.
                    //     // or
                    //      lines[i] = ServerListBox.Items[i].ToString();

                    //    }

                    if (NDObject.Category.ToString() == "Replication")
                    {
                        Serverkeys = "";
                        Serverkeys = NDObject.ReplicationDestination;
                        ServerListBox.Enabled = true;
                        ServerListBox.Visible = true;
                        ServerLabel.Visible = true;
                        InitiateRepCheckBox.Visible = true;
                        InitiateRepCheckBox.Enabled = true;

                        // Split the string at the space characters
                        words = Serverkeys.Split(',');
                        bool Found;
                        try
                        {
                            //'Explode the keys into an array, and use each key to locate the appropriate server
                            for (int i = 0; i < ServerListBox.Items.Count - 1; i++)
                            {
                                Found = false;
                                foreach (string x in words)
                                {
                                    if (x.ToString().Trim() == ServerListBox.Items[i].Text.Trim())  //lines[i].ToString())
                                    {

                                        Found = true;

                                    }

                                }

                                if (Found == true)
                                {
                                    ServerListBox.Items[i].Selected = true;

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
                    else
                        //If the type is not Replication, just load the box with all the servers, none selected.
                        for (int i = 0; i < DominoServerComboBox.Items.Count - 1; i++)
                        {
                            //Me.ListBox1.Items.Add(DominoServerlist(n))
                            ServerListBox.Items.Add(DominoServerComboBox.Items[i].ToString());
                        }

                }

                else
                {
                    RetryIntervalTextBox.Text = "2";
                    OffHoursTextBox.Text = "30";
                    ScanIntervalTextBox.Text = "8";
                    EnabledCheckBox.Checked = true;
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
        /// Fill TaskNameComboBox from DB
        /// </summary>
        /// <param name="TaskNameComboBox"></param>
        private void FillTaskNameComboBox()
        {
            DataTable DSTasksDataTable = RestrServers();
            DominoServerComboBox.DataSource = DSTasksDataTable;
            DominoServerComboBox.TextField = "ServerName";
            DominoServerComboBox.ValueField = "ID";
            DominoServerComboBox.DataBind();
        }
        private void FillListBox()
        {
            DataTable NotesDataTable = RestrServers();
            ServerListBox.DataSource = NotesDataTable;
            ServerListBox.TextField = "ServerName";
            ServerListBox.ValueField = "ID";
            ServerListBox.DataBind();
        }

        public DataTable RestrServers()
        {
            DataTable DCTasksDataTable = VSWebBL.ConfiguratorBL.DominoClusterBL.Ins.GetServer();
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
                            LocationID.Add(Convert.ToInt32(dominorow["locationid"].ToString()));
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

        protected void AlertTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            DisplayLabels();
        }

        public void DisplayLabels()
        {
            switch (AlertTypeComboBox.Text)
            {
                case "Replication":
                    ServerListBox.Enabled = true;
                    ServerListBox.Visible = true;
                    ServerLabel.Visible = true;
                    InitiateRepCheckBox.Visible = true;
                    InitiateRepCheckBox.Enabled = true;
                    ServerLabel.Visible = true;
                    GBLabel.Visible = false;
                    TriggerLabel.Text = "";
                    TriggerValueTextBox.Text = "0";
                    TriggerValueTextBox.Enabled = false;
                    TriggerValueTextBox.Visible = false;

                    break;
                case "Document Count":
                    GBLabel.Visible = false;
                    TriggerValueTextBox.Visible = true;
                    TriggerValueTextBox.Enabled = true;
                    TriggerLabel.Text = "documents";
                    break;
                case "Database Size":
                    GBLabel.Visible = true;
                    TriggerValueTextBox.Visible = true;
                    TriggerLabel.Text = "MB";
                    TriggerValueTextBox.Enabled = true;
                    break;
                case "Database Response Time":
                    TriggerValueTextBox.Enabled = true;
                    GBLabel.Visible = false;
                    TriggerValueTextBox.Visible = true;
                    TriggerLabel.Text = "ms.";
                    break;
                case "Database Disappearance":
                    GBLabel.Visible = false;
                    TriggerLabel.Text = "";
                    TriggerValueTextBox.Text = "0";
                    TriggerValueTextBox.Visible = false;
                    TriggerValueTextBox.Enabled = false;
                    break;
                case "Refresh All Views":
                    GBLabel.Visible = false;
                    TriggerLabel.Text = "";
                    TriggerValueTextBox.Text = "0";
                    TriggerValueTextBox.Visible = false;
                    TriggerValueTextBox.Enabled = false;
                    break;
            }
        }

        protected void TriggerValueTextBox_TextChanged(object sender, EventArgs e)
        {
            if (AlertTypeComboBox.Text == "Database Size")
            {
                try
                {
                    GBLabel.Visible = true;
                    GBLabel.Text = " (" + (Convert.ToDouble(TriggerValueTextBox.Text) / 1024).ToString("###,##0.00") + " GB" + ")";
                }
                catch (Exception ex)
                {
                    GBLabel.Text = "";
                    //6/27/2014 NS added for VSPLUS-634
                    Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                }
            }
            else
            {
                GBLabel.Visible = false;
            }
        }

        protected void NameTextBox_TextChanged(object sender, EventArgs e)
        {
            CleanupSpecialChars();
        }

        private NotesDatabases CollectDataForNotesDatabase()
        {
            //SetFocusOnControl();
            try
            {

                //Edit Notes
                NotesDatabases NDObject = new NotesDatabases();
                if (Mode == "Update")
                {
                    NDObject.ID = ID;
                }

                //ND profile
                NDObject.Name = NameTextBox.Text;
                if (DominoServerComboBox.SelectedItem!= null)
                {
                    NDObject.ServerID = int.Parse(DominoServerComboBox.SelectedItem.Value.ToString());
                    NDObject.ServerName = DominoServerComboBox.SelectedItem.Text;
                }


                
                NDObject.FileName = DBFileNameTextBox.Text;
                //Scan Settings
                NDObject.RetryInterval = int.Parse(RetryIntervalTextBox.Text);
                //if (IsNumeric(OffHoursTextBox.Text) == false)
                //{
                //    // NDObject.OffHoursScanInterval = int.Parse(OffHoursTextBox.Text);
                //    OffHoursTextBox.Focus();
                //}
               
                    //OffHoursTextBox.Focus();
                    //}
                    NDObject.OffHoursScanInterval = int.Parse(OffHoursTextBox.Text);
                    NDObject.ScanInterval = int.Parse(ScanIntervalTextBox.Text);
                    NDObject.Enabled = EnabledCheckBox.Checked;
                    //Type Roundpanel
                    if (TriggerValueTextBox.Text != "")
                        NDObject.ResponseThreshold = int.Parse(TriggerValueTextBox.Text);
                    NDObject.InitiateReplication = InitiateRepCheckBox.Checked;
                    NDObject.Category = AlertTypeComboBox.Text;
                    if (TriggerValueTextBox.Text != "")
                        NDObject.TriggerValue = float.Parse(TriggerValueTextBox.Text);
                    NDObject.TriggerType = AlertTypeComboBox.Text;


                //7/30/2013 NS added
                    NDObject.AboveBelow = "";
                    //Figure out which servers are selected



                    try
                    {
                        myIndex = 0;
                        if (ServerListBox.SelectedItems.Count > 0)
                        {
                            
                           foreach (Object item in ServerListBox.SelectedItems)
                            {
                                myIndex = 0;

                                foreach (Object server in DominoServerComboBox.Items)
                                {
                                    if (server.ToString() == item.ToString())
                                    {
                                        //string myServers="";
                                        myServers += ServerListBox.Items[myIndex].ToString() + ",";
                                    }
                                    myIndex += 1;
                                }
                                // Debug.Print(item.ToString)
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

                    try
                    {
                        //if(myServers!="")
                        myServers = myServers.Substring(0, myServers.Length - 1);

                    }
                    catch (Exception ex)
                    {
                        myServers = "";     // throw ex; 
                        //6/27/2014 NS added for VSPLUS-634
                        Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                    }
                    finally { }


                    NDObject.ReplicationDestination = myServers;
                
                    return NDObject;
                
            }
            catch (Exception ex)
            {
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex; 
            }
            finally { }
        }

        private void UpdateNotesDatabase()
        {
            try
            {
                Object ReturnValue = VSWebBL.ConfiguratorBL.NotesDatabaseBL.Ins.UpdateData(CollectDataForNotesDatabase());
                SetFocusOnError(ReturnValue);
                if (ReturnValue.ToString() == "True")
                {
                    //1/21/2014 NS modified
                    /*
                    ErrorMessageLabel.Text = "Data updated successfully.";
                    ErrorMessagePopupControl.HeaderText = "Information";
                    ErrorMessagePopupControl.ShowCloseButton = false;
                    ValidationUpdatedButton.Visible = true;
                    ValidationOkButton.Visible = false;
                    ErrorMessagePopupControl.ShowOnPageLoad = true;
                    */
                    Session["NotesDBUpdateStatus"] = NameTextBox.Text;
                    Response.Redirect("NotesDatabase.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
                    Context.ApplicationInstance.CompleteRequest();
                }

            }
            catch (Exception ex)
            {
                //10/3/2014 NS modified for VSPLUS-990
                errorDiv2.InnerHtml = "The following error has occurred: " + ex.Message+
                    "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                errorDiv2.Style.Value = "display: block";
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
            }
        }



        private void InsertNotesDatabase()
        {
            NotesDatabases NDObj = new NotesDatabases();
            NDObj.Name = NameTextBox.Text;
            DataTable returntable = VSWebBL.ConfiguratorBL.NotesDatabaseBL.Ins.GetName(NDObj);

            if (returntable.Rows.Count > 0)
            {
                ErrorMessageLabel.Text = "This Notes Database name is already in use.  Each db name must be unique to make sense on the Overall Health tab.  Please enter a different name.";
                ErrorMessagePopupControl.ShowOnPageLoad = true;
                flag = true;
                NameTextBox.Focus();
            }
            else
            {

                try
                {
                    object ReturnValue = VSWebBL.ConfiguratorBL.NotesDatabaseBL.Ins.InsertData(CollectDataForNotesDatabase());
                    SetFocusOnError(ReturnValue);
                    if (ReturnValue.ToString() == "True")
                    {
                        /*
                        ErrorMessageLabel.Text = "Data inserted successfully.";
                        ErrorMessagePopupControl.HeaderText = "Information";
                        ErrorMessagePopupControl.ShowCloseButton = false;
                        ValidationUpdatedButton.Visible = true;
                        ValidationOkButton.Visible = false;
                        ErrorMessagePopupControl.ShowOnPageLoad = true;
                        */
                        //12/8/2014 NS modified to redirect to the server grid page
                        Session["NotesDBUpdateStatus"] = NameTextBox.Text;
                        Response.Redirect("NotesDatabase.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
                        Context.ApplicationInstance.CompleteRequest();
                    }
                }
                catch (Exception ex)
                {
                    errorDiv.Style.Value = "display: block";
                    //10/3/2014 NS modified for VSPLUS-990
                    errorDiv.InnerHtml = "The following error has occurred: " + ex.Message +
                        "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
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
        //private void SetFocusOnControl()
        //{
            
        //    try
        //    {
        //        int.Parse(TriggerValueTextBox.Text);
        //    }
        //    catch
        //    {

        //       TriggerValueTextBox.Focus();
        //    }
        //    try
        //    {
        //        int.Parse(RetryIntervalTextBox.Text);
        //    }
        //    catch 
        //    {
        //        RetryIntervalTextBox.Focus();
                
        //    }
        //    try
        //    {
        //        int.Parse(OffHoursTextBox.Text);
        //    }
        //    catch 
        //    {
        //        OffHoursTextBox.Focus();
               
        //    }


        //}


        public bool IsNumeric(string value)
        {

            try
            {
                int.Parse(value);
                return true;
            }
            catch (Exception ex)
            {
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                return false;
               
            }
        }

        protected void FormOkButton_Click(object sender, EventArgs e)
        {
            //1/21/2016 NS modified for VSPLUS-2540
            CleanupSpecialChars();
            // Notify the service that a change has been made
            VSWebBL.SettingBL.SettingsBL.Ins.UpdateSvalue("Notes Database Update", "True", VSWeb.Constants.Constants.SysString);
            try
            {
                if (Mode == "Update")
                {

                    UpdateNotesDatabase();
                }
                if (Mode == "Insert")
                {
                    InsertNotesDatabase();
                    //12/8/2014 NS modified
                    if (flag == false)
                    {
                        InsertStatus();
                    }
                    else
                    {
                        Session["NotesDBUpdateStatus"] = NameTextBox.Text;
                        Response.Redirect("NotesDatabase.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
                        Context.ApplicationInstance.CompleteRequest();
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

        protected void FormCancelButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("NotesDatabase.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
            Context.ApplicationInstance.CompleteRequest();
        }

        protected void ValidationUpdatedButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("NotesDatabase.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
            Context.ApplicationInstance.CompleteRequest();
        }

        protected void NotesBrowseButton_Click(object sender, EventArgs e)
        {
            SelectNotesPopupControl.ShowOnPageLoad = true;
            //12/31/2013 NS added
            errorDiv.Style.Value = "display: none";
            FillNDfileNameComboBox();
        }
        private void FillNDfileNameComboBox()
        {
            DataTable NDFileDataTable = VSWebBL.ConfiguratorBL.DominoPropertiesBL.Ins.GetAllData();
            NDNameComboBox.DataSource = NDFileDataTable;
            NDNameComboBox.TextField = "Name";
            NDNameComboBox.ValueField = "Name";
            NDNameComboBox.DataBind();
        }

        private DataTable GetNotesDatabases(string ServerName)
        {
            Domino.NotesDbDirectory dir;
            Domino.NotesDatabase db;
            //4/26/2013 NS modified
            //VSFramework.RegistryHandler myRegistry = new VSFramework.RegistryHandler();
            byte[] MyPass;
            string MyDominoPassword; //should be string
            //object MyObjPwd;
            string MyObjPwd;
            string[] MyObjPwdArr;
            DataTable dt = new DataTable();
            VSFramework.TripleDES mySecrets = new VSFramework.TripleDES();
            try
            {
                //MyObjPwd = myRegistry.ReadFromRegistry("Password");
                MyObjPwd = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("Password");
                MyObjPwdArr = MyObjPwd.Split(',');
                MyPass = new byte[MyObjPwdArr.Length];
                for (int i = 0; i < MyObjPwdArr.Length; i++)
                {
                    MyPass[i] = Byte.Parse(MyObjPwdArr[i]);
                }
            }
            catch (Exception ex)
            {
                MyPass = null;
                //12/31/2013 NS modified
                /*
                SelectNotesPopupControl.ShowOnPageLoad = false;
                ErrorMessageLabel.Text = "The password specified in the Settings table is empty or invalid.";
                ErrorMessagePopupControl.HeaderText = "Incorrect Domino Password";
                ValidationUpdatedButton.Visible = true;
                ValidationOkButton.Visible = false;
                ErrorMessagePopupControl.ShowOnPageLoad = true;
                 */
                //10/3/2014 NS modified for VSPLUS-990
                errorDiv.InnerHtml = "The password specified in the Settings table is empty or invalid. " + ex.Message+
                    "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                errorDiv.Style.Value = "display: block";
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
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
                    //12/31/2013 NS modified
                    /*
                    SelectNotesPopupControl.ShowOnPageLoad = false;
                    ErrorMessageLabel.Text = "The password specified in the Settings table is empty or invalid.";
                    ErrorMessagePopupControl.HeaderText = "Incorrect Domino Password";
                    ValidationUpdatedButton.Visible = true;
                    ValidationOkButton.Visible = false;
                    ErrorMessagePopupControl.ShowOnPageLoad = true;
                     */
                    //10/3/2014 NS modified for VSPLUS-990
                    errorDiv.InnerHtml = "The password specified in the Settings table is empty or invalid."+
                        "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                    errorDiv.Style.Value = "display: block";
                }
            }
            catch (Exception ex)
            {
                MyDominoPassword = "";
                //12/31/2013 NS modified
                /*
                SelectNotesPopupControl.ShowOnPageLoad = false;
                ErrorMessageLabel.Text = "VitalSigns was unable to decrypt the password stored in the Settings table. Please ensure the password is correct.";
                ErrorMessagePopupControl.HeaderText = "Domino Password Could Not Be Decrypted";
                ValidationUpdatedButton.Visible = true;
                ValidationOkButton.Visible = false;
                ErrorMessagePopupControl.ShowOnPageLoad = true;
                 */
                //10/3/2014 NS modified for VSPLUS-990
                errorDiv.InnerHtml = "VitalSigns was unable to decrypt the password stored in the Settings table. Please ensure the password is correct. " + ex.Message+
                    "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                errorDiv.Style.Value = "display: block";
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
            }
            try
            {
                NotesSessionObject.Initialize(MyDominoPassword);
                dir = NotesSessionObject.GetDbDirectory(ServerName);
                db = dir.GetFirstDatabase(Domino.DB_TYPES.NOTES_DATABASE);
                DataRow dr = dt.NewRow();
                dt.Columns.Add("DBTitle", typeof(string));
                dt.Columns.Add("DBFile", typeof(string));
                dt.Columns.Add("ReplicaID", typeof(string));
                while (db != null)
                {
                    dr["DBTitle"] = db.Title;
                    dr["DBFile"] = db.FilePath;
                    dr["ReplicaID"] = db.ReplicaID;
                    dt.Rows.Add(dr);
                    dr = dt.NewRow();
                    db = dir.GetNextDatabase();
                }
                db = null;
                dr = null;
                System.Runtime.InteropServices.Marshal.ReleaseComObject(NotesSessionObject);
            }
            catch (Exception ex)
            {
                using (System.IO.StreamWriter _testData = new System.IO.StreamWriter(System.Web.HttpContext.Current.Server.MapPath("~/Config_EditNotes_Log.txt"), true))
                {
                    _testData.WriteLine(ex.Message); // Write the file.
                }
                db = null;
                dir = null;
                //12/31/2013 NS modified
                /*
                SelectNotesPopupControl.ShowOnPageLoad = false;
                ErrorMessageLabel.Text = "Notes session could not be initialized due to an incorrect password value.";
                ErrorMessagePopupControl.HeaderText = "Incorrect Domino Password";
                ValidationUpdatedButton.Visible = true;
                ValidationOkButton.Visible = false;
                ErrorMessagePopupControl.ShowOnPageLoad = true;
                 */
                //10/3/2014 NS modified for VSPLUS-990
                errorDiv.InnerHtml = "Notes session could not be initialized due to an incorrect password value. " +ex.Message+
                    "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                errorDiv.Style.Value = "display: block";
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
            }

            return dt;
        }

        // populate Select NotesDatabase form
        private void PopulateSelectBox(string ServerName)
        {
            GridViewDataColumn column;
            //bind to grid
            DataTable dt = new DataTable();
            dt = GetNotesDatabases(ServerName);
            //4/3/2014 NS modified for VSPLUS-138
            //Session["NotesDB"] = dt;
            ViewState["NotesDB"] = dt;
            if (!IsPostBack)
            {
                if (dt.Rows.Count > 0)
                {
                    SelectNDGridView.Columns.Clear();
                    SelectNDGridView.DataSource = dt;
                    SelectNDGridView.KeyFieldName = "ReplicaID";
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        column = new GridViewDataColumn(dt.Columns[i].ColumnName);
                        if (dt.Columns[i].ColumnName == "ReplicaID")
                        {
                            column.Visible = false;
                        }
                        SelectNDGridView.Columns.Add(column);
                    }
                    SelectNDGridView.DataBind();
                }
            }
            else
            {
                //4/3/2014 NS modified for VSPLUS-138
                //DataTable ND = (DataTable)Session["NotesDB"];
                DataTable ND = (DataTable)ViewState["NotesDB"];
                if (ND.Rows.Count > 0)
                {
                    SelectNDGridView.Columns.Clear();
                    SelectNDGridView.DataSource = ND;
                    SelectNDGridView.KeyFieldName = "ReplicaID";
                    for (int i = 0; i < ND.Columns.Count; i++) 
                    {
                        column = new GridViewDataColumn(ND.Columns[i].ColumnName);
                        if (dt.Columns[i].ColumnName == "ReplicaID")
                        {
                            column.Visible = false;
                        }
                        SelectNDGridView.Columns.Add(column); 
                    }
                    SelectNDGridView.DataBind();
                }
            }
        }


        protected void SelectNDGridView_SelectionChanged(object sender, EventArgs e)
        {
            
        }

        protected void selectButton_Click(object sender, EventArgs e)
        {
            SelectDB();
            SelectNotesPopupControl.ShowOnPageLoad = false;
        }


        private void SelectDB()
        {
            if (SelectNDGridView.Selection.Count > 0)
            {
                System.Collections.Generic.List<object> RepIDL = SelectNDGridView.GetSelectedFieldValues("ReplicaID");
                if (RepIDL.Count > 0)
                {
                    string ID = RepIDL[0].ToString();
                }
                System.Collections.Generic.List<object> DBFileL = SelectNDGridView.GetSelectedFieldValues("DBFile");
                if (DBFileL.Count > 0)
                {
                    DBFile = DBFileL[0].ToString();
                }
                System.Collections.Generic.List<object> DBTitleL = SelectNDGridView.GetSelectedFieldValues("DBTitle");
                if (DBTitleL.Count > 0)
                {
                    DBTitle = DBTitleL[0].ToString();
                }
                //}
                DominoServerComboBox.Text = NDNameComboBox.Text;
                NameTextBox.Text = DBTitle;
                DBFileNameTextBox.Text = DBFile;
                try
                {
                    Domino.NotesDatabase db;
                    NotesSessionObject.Initialize("");
                    db = NotesSessionObject.GetDatabase(NDNameComboBox.Text, DBFile);
                    if (db.IsOpen)
                    {
                        long myMB, myGB;
                        myMB = long.Parse((Math.Round(db.Size / 1024 / 1024, 0)).ToString());
                        myGB = myMB / 1024 / 1024;
                        string mydbInfo;
                        mydbInfo = db.AllDocuments.Count + " documents / ";
                        mydbInfo += (long.Parse((db.Size).ToString()) / 1024 / 1024).ToString("###,##0") + " MB ";
                        if (myGB > 1)
                        {
                            mydbInfo += (long.Parse((db.Size).ToString()) / 1024 / 1024).ToString("###,##0") + " GB ";
                        }
                        NDsizeLabel.Text = mydbInfo;
                    }
                    db = null;
                }
                catch (Exception ex)
                {
                    //6/27/2014 NS added for VSPLUS-634
                    Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                    //6/14/2016 NS added for VSPLUS-3072
                    errorDivMain.Style.Value = "display: block";
                    errorDivMain.InnerHtml = "The following error has occurred: " + ex.Message + ". <br />" +
                        "Note: while you are going to be able to save the current record, the Domino service will be unable to access the database in question until you add the ID to the database's ACL." +
                        "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                    //throw ex;
                }
            }
            System.Runtime.InteropServices.Marshal.ReleaseComObject(NotesSessionObject);
        }

        protected void CancelButton_Click(object sender, EventArgs e)
        {
            SelectNotesPopupControl.ShowOnPageLoad = false;
        }

        protected void NDNameComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            //12/31/2013 NS modified
            errorDiv.Style.Value = "display: none";
            try
            {
                SelectNDGridView.DataSource = "";
                SelectNDGridView.DataBind();
            }
            catch (Exception ex)
            {
                //12/31/2013 NS modified
                //10/3/2014 NS modified for VSPLUS-990
                errorDiv.InnerHtml = "The following error has occurred: " + ex.Message+
                    "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                errorDiv.Style.Value = "display: block";
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                //throw;
            }
            try
            {
                PopulateSelectBox(NDNameComboBox.Text);
            }
            catch (Exception ex)
            {
                //12/31/2013 NS modified
                /*
                ErrorMessageLabel.Text = "The Domino password stored in registry is incorrect. Please reset the password using the IBM Domino Settings menu option under Stored Passwords & Options and try again." + ex.ToString();
                ErrorMessagePopupControl.HeaderText = "Incorrect Domino Password";
                ErrorMessagePopupControl.ShowCloseButton = false;
                ValidationUpdatedButton.Visible = true;
                ValidationOkButton.Visible = false;
                ErrorMessagePopupControl.ShowOnPageLoad = true;
                //throw;
                */
                //10/3/2014 NS modified for VSPLUS-990
                errorDiv.InnerHtml = "The Domino password stored in the Settings table is incorrect. Please reset the password using the IBM Domino Settings menu option under Stored Passwords & Options and try again. " + ex.ToString()+
                    "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                errorDiv.Style.Value = "display: block";
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
            }

        }
        private Status CollectDataforStatus()
        {
            Status St = new Status();
            try
            {
                St.Category = AlertTypeComboBox.Text;
                St.DeadMail = 0;
                St.Description = "Verifies Notes Database.";
                St.DownCount = 0;
                St.Name = NameTextBox.Text;
                St.MailDetails = "";
                St.PendingMail = 0;
                St.sStatus = "Not Scanned";
                St.Type = "Notes Database";
                St.Upcount = 0;
                St.UpPercent = 1;
                St.LastUpdate = System.DateTime.Now;
                St.ResponseTime = 0;
                //5/5/2016 NS modified
                //Changed -NDB to -Notes Database
                St.TypeANDName = NameTextBox.Text+"-Notes Database";
                St.Icon = 0;

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
                    ErrorMessageLabel.Text = "Data inserted successfully.";
                    ErrorMessagePopupControl.HeaderText = "Information";
                    ErrorMessagePopupControl.ShowCloseButton = false;
                    ValidationUpdatedButton.Visible = true;
                    ValidationOkButton.Visible = false;
                    ErrorMessagePopupControl.ShowOnPageLoad = true;
                     */
                    //12/8/2014 NS modified to redirect to the server grid page
                    Session["NotesDBUpdateStatus"] = NameTextBox.Text;
                    Response.Redirect("NotesDatabase.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
                    Context.ApplicationInstance.CompleteRequest();
                }

            }
            catch (Exception ex)
            {
                //ErrorMessageLabel.Text = "Error attempting to update the status table :" + ex.Message;
                //ErrorMessagePopupControl.ShowOnPageLoad = true;
                errorDiv.Style.Value = "display: block";
                //10/3/2014 NS modified for VSPLUS-990
                errorDiv.InnerHtml = "The following error has occurred: " + ex.Message +
                    "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
            }
            finally { }
        }

        protected void SelectNDGridView_PageSizeChanged(object sender, EventArgs e)
        {
            //ProfilesGridView.PageIndex;
            VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("EditNotes|SelectNDGridView", SelectNDGridView.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        }

        //1/21/2016 NS modified for VSPLUS-2540
        private void CleanupSpecialChars()
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
                //   MessageBox.Show(ex.Message)
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
            }
        }
    }

}