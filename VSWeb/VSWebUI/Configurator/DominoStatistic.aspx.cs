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
    public partial class DominoStatistic : System.Web.UI.Page
    {
        int ServerKey;
        string Mode;
        protected void Page_Load(object sender, EventArgs e)
        {
          
             
             if (!IsPostBack)
             {
               
                 FillserverNameComboBox();
                 //FillStatisticComboBox();
             }
      
        try
        {
            if (Request.QueryString["ID"] != null && Request.QueryString["ID"] != "")
            {
                Mode = "Update";

                ServerKey = int.Parse(Request.QueryString["ID"]);
                
                if (!IsPostBack)
                {
                    DServerComboBox.Enabled=false;
                    DStatComboBox.Enabled=false;
                    //Fill DominoStatistic page
                    FillData(ServerKey);
                    //1/21/2014 NS modified
                    //CustomRoundPanel.HeaderText = "Custom Statistics - " + " " + DServerComboBox.Text;
                    //11/19/2014 NS modified
                    //CustomStatisticsLabel.Text += " - " + DServerComboBox.Text;
                    servernamelbldisp.InnerHtml += " - " + DServerComboBox.Text;
                }
            }
            else
            {
                Mode = "Insert";
               
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
         private void FillData(int ID)
        {
            try
            {

                DominoCustomStatValues DominoCustomObject = new DominoCustomStatValues();
                DominoCustomStatValues ReturnDCustObject = new DominoCustomStatValues();
                DominoCustomObject.ID = ID;
                ReturnDCustObject = VSWebBL.ConfiguratorBL.DominoCustomStatBL.Ins.GetData(DominoCustomObject);       
                //DominoStatistic fields
                DServerComboBox.Text=ReturnDCustObject.ServerName.ToString();
                DStatComboBox.Text=ReturnDCustObject.StatName.ToString();
                ThresholdTextBox.Text=ReturnDCustObject.ThresholdValue.ToString();
                if(ReturnDCustObject.GreaterThanORLessThan=="Greater Than")
                {
                    GreaterRadioButton.Checked=true;
                }
                else
                {
                    LessRadioButton.Checked=true;
                }
                TimesInRowTextBox.Text = ReturnDCustObject.TimesInARow.ToString();
                ConsolecmdTextBox.Text=ReturnDCustObject.ConsoleCommand.ToString();
                
            }
            catch (Exception ex)
            {
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex; 
            }
            finally { }
        }

        private DominoCustomStatValues CollectDataForDominoCustomStat()
        {
            try
            {
                //DominoStatistic 
                DominoCustomStatValues DominoCustomObject = new DominoCustomStatValues();
                DominoCustomObject.ServerName=DServerComboBox.Text;
                DominoCustomObject.StatName=DStatComboBox.Text;
                DominoCustomObject.ThresholdValue=float.Parse(ThresholdTextBox.Text);
                DominoCustomObject.TimesInARow = int.Parse(TimesInRowTextBox.Text);
                if(GreaterRadioButton.Checked==true)
                {
                DominoCustomObject.GreaterThanORLessThan="Greater Than";
                }
                else
                {
                    DominoCustomObject.GreaterThanORLessThan="Less than";
                }
                DominoCustomObject.ConsoleCommand=ConsolecmdTextBox.Text;
                if (Mode == "Update")
                {
                    DominoCustomObject.ID = ServerKey;
                }
                                
                return DominoCustomObject;
            }
            catch (Exception ex)
            {
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex; 
            }
            finally { }
        }
        private void UpdateDominoCustomStatData()
        {
            try
            {
                Object ReturnValue = VSWebBL.ConfiguratorBL.DominoCustomStatBL.Ins.UpdateData(CollectDataForDominoCustomStat());
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
                    Session["DominoStatUpdateStatus"] = DServerComboBox.Text;
                    Response.Redirect("CustomStatistics.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
                    Context.ApplicationInstance.CompleteRequest();
                }
            }
            catch (Exception ex)
            {
                //10/3/2014 NS modified for VSPLUS-990
                errorDiv.InnerHtml = "The following error has occurred: " + ex.Message+
                    "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                errorDiv.Style.Value = "display: block";
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
            }
        }
        private void InsertDominoCustomStat()
        {

            try
            {
                object ReturnValue = VSWebBL.ConfiguratorBL.DominoCustomStatBL.Ins.InsertData(CollectDataForDominoCustomStat());
                SetFocusOnError(ReturnValue);
                if (ReturnValue.ToString() == "True")
                {
                    //11/3/2014 NS modified
                    /*
                    ErrorMessageLabel.Text = "Data inserted successfully.";
                    ErrorMessagePopupControl.HeaderText = "Information";
                    ErrorMessagePopupControl.ShowCloseButton = false;
                    ValidationUpdatedButton.Visible = true;
                    ValidationOkButton.Visible = false;
                    ErrorMessagePopupControl.ShowOnPageLoad = true;
                    */
                    Session["DominoStatUpdateStatus"] = DServerComboBox.Text;
                    Response.Redirect("CustomStatistics.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
                    Context.ApplicationInstance.CompleteRequest();
                }
            }
            catch (Exception ex)
            {
                //11/3/2014 NS added for VSPLUS-990
                errorDiv.InnerHtml = "The following error has occurred: " + ex.Message +
                    "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                errorDiv.Style.Value = "display: block";
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex); 
                throw ex;
            }
            finally { }
        }

        protected void OKButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (Mode == "Update")
                {

                    UpdateDominoCustomStatData();
                }
                if (Mode == "Insert")
                {
                    InsertDominoCustomStat();
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
        private void SetFocusOnError(Object ReturnValue)
        {
            string ErrorMessage = ReturnValue.ToString();
            if (ErrorMessage.Substring(0, 2) == "ER")
            {
                ErrorMessageLabel.Text = ErrorMessage.Substring(3);
                ErrorMessagePopupControl.ShowOnPageLoad = true;

            }
        }

        protected void CancelButton_Click(object sender, EventArgs e)
        {
            //1/21/2014 NS added
            Session["Submenu"] = "LotusDomino";
            Response.Redirect("CustomStatistics.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
            Context.ApplicationInstance.CompleteRequest();
        }

        protected void ValidationUpdatedButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("CustomStatistics.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
            Context.ApplicationInstance.CompleteRequest();
        }
        private void FillserverNameComboBox()
        {
            DataTable DCStatDataTable = RestrServers();
            DServerComboBox.DataSource = DCStatDataTable;   //DataSource = DCStatDataTable;
            DServerComboBox.TextField = "ServerName";
            DServerComboBox.ValueField = "ID";
            DServerComboBox.DataBind();
        }

        public DataTable RestrServers()
        {
            DataTable DCStatDataTable = new DataTable();
            DCStatDataTable = VSWebBL.ConfiguratorBL.DominoClusterBL.Ins.GetServer();
            //4/3/2014 NS modified for VSPLUS-138
            //if (Session["RestrictedServers"] != "" && Session["RestrictedServers"] != null)
            if (ViewState["RestrictedServers"] != "" && ViewState["RestrictedServers"] != null)
            {

                List<int> ServerID = new List<int>();
                List<int> LocationID = new List<int>();
                //4/3/2014 NS modified for VSPLUS-138
                //DataTable resServers = (DataTable)Session["RestrictedServers"];
                DataTable resServers = (DataTable)ViewState["RestrictedServers"];
                foreach (DataRow dominorow in DCStatDataTable.Rows)
                {
                    foreach (DataRow resser in resServers.Rows)
                    {
                        if (resser["serverid"].ToString() == dominorow["ID"].ToString())
                        {
                            ServerID.Add(DCStatDataTable.Rows.IndexOf(dominorow));
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
                    DCStatDataTable.Rows[Id].Delete();
                }
                DCStatDataTable.AcceptChanges();
                foreach (int lid in LocationID)
                {
                    DataRow[] row = DCStatDataTable.Select("LocationID=" + lid + "");
                    for (int i = 0; i < row.Count(); i++)
                    {
                        DCStatDataTable.Rows.Remove(row[i]);
                        DCStatDataTable.AcceptChanges();
                    }
                }
                DCStatDataTable.AcceptChanges();

            }
            return DCStatDataTable;
        }

        private void FillStatisticComboBox()
        {
            DataTable DCStatDataTable = VSWebBL.ConfiguratorBL.DominoCustomStatBL.Ins.GetAllData();
            DStatComboBox.DataSource = DCStatDataTable;
            DStatComboBox.TextField = "StatName";
            DStatComboBox.ValueField = "StatName";
            DStatComboBox.DataBind();   
        }

        protected void DStatComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            CurrentValTextBox.Text = "";
        }

        protected void DServerComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            CurrentValTextBox.Text = "";
        }

        protected void CurrValButton_Click(object sender, EventArgs e)
        {
            //CurrentValue();
            //11//2014 NS added for VSPLUS-1133
            byte[] MyPass;
            string MyDominoPassword; //should be string
            string MyObjPwd;
            string[] MyObjPwdArr;
            VSFramework.TripleDES mySecrets = new VSFramework.TripleDES();
            try
            {
                MyObjPwd = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("Password");
                if (MyObjPwd != "")
                {
                    MyObjPwdArr = MyObjPwd.Split(',');
                    MyPass = new byte[MyObjPwdArr.Length];
                    for (int i = 0; i < MyObjPwdArr.Length; i++)
                    {
                        MyPass[i] = Byte.Parse(MyObjPwdArr[i]);
                    }
                }
                else
                {
                    //10/6/2014 NS modified for VSPLUS-990
                    errorDiv.InnerHtml = "The following error has occurred: Notes password may not be empty. Please update the password under Stored Passwords & Options\\IBM Domino Settings." +
                        "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                    errorDiv.Style.Value = "display: block";
                    MyPass = null;
                }
            }
            catch (Exception ex)
            {
                errorDiv.InnerHtml = "The following error has occurred: " + ex.Message +
                    "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                errorDiv.Style.Value = "display: block";
                MyPass = null;
                //5/15/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
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
                errorDiv.InnerHtml = "The following error has occurred: " + ex.Message +
                    "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                errorDiv.Style.Value = "display: block";
                MyDominoPassword = "";
                //5/15/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
            //3/25/2014 NS modified for VSPLUS-494
            if (MyDominoPassword != null)
            {
                Domino.NotesSession NotesSessionObject = new Domino.NotesSession();
                try
                {
                    NotesSessionObject.Initialize(MyDominoPassword);
                    CurrentValTextBox.Text = NotesSessionObject.SendConsoleCommand(DServerComboBox.Text, "sh stat " + DStatComboBox.Text);
                }
                catch (Exception ex)
                {
                    //7/8/2013 NS modified - the code now handles an exception gracefully
                    //throw ex; 
                    //11/3/2014 NS modified
                    /*
                    ErrorMessageLabel.Text = "Notes session could not be initialized.";
                    ErrorMessagePopupControl.HeaderText = "Initialization Failure";
                    ValidationUpdatedButton.Visible = true;
                    ValidationOkButton.Visible = false;
                    ErrorMessagePopupControl.ShowOnPageLoad = true;
                     */
                    //11/3/2014 NS added for VSPLUS-990
                    errorDiv.InnerHtml = "The following error has occurred: " + ex.Message +
                        "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                    errorDiv.Style.Value = "display: block";
                    //6/27/2014 NS added for VSPLUS-634
                    Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                }
                finally { }
                System.Runtime.InteropServices.Marshal.ReleaseComObject(NotesSessionObject);
            }
        }


        #region CurrentValCode
    //    Private Sub CurrentValue()
    //    Try
    //        Cursor.Current = Windows.Forms.Cursors.WaitCursor
    //        Me.TxtCurrentValue.Text = "Attempting to contact server...please wait."
    //        Application.DoEvents()
    //    Catch ex As Exception

    //    End Try

    //    Dim myRegistry As New RegistryHandler

    //    Dim MyDominoPassword As String
    //    Dim MyPass As Byte()
    //    Dim mySecrets As New TripleDES
    //    Try
    //        MyPass = myRegistry.ReadFromRegistry("Password")  'password as encrypted byte stream
    //        'Get the password from the registry
    //        MyDominoPassword = mySecrets.Decrypt(MyPass) 'password in clear text
    //    Catch ex As Exception

    //    End Try

    //    Try
    //        If MyDominoPassword = "" Then
    //            MessageBox.Show("You must register your Notes password before using this function.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
    //            Me.ButtonCurrentValue.Enabled = False
    //            Cursor.Current = Windows.Forms.Cursors.Default
    //            Me.TxtCurrentValue.Text = ""
    //            Application.DoEvents()
    //            Exit Sub
    //        End If

    //    Catch ex As Exception

    //    End Try


    //    Dim s As New Domino.NotesSession

    //    Try
    //        If Not (MyDominoPassword = "") Then
    //            s.Initialize(MyDominoPassword)
    //        Else
    //            s.Initialize()
    //        End If

    //    Catch ex As Exception
    //        MessageBox.Show("Error initializing Notes session.  " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
    //        System.Runtime.InteropServices.Marshal.ReleaseComObject(s)
    //        myRegistry = Nothing
    //        Exit Sub
    //    End Try

    //    Try
    //        Me.TxtCurrentValue.Text = s.SendConsoleCommand(Me.cmboDominoServers.Text, "sh stat " & Me.cmbStatistic.Text)
    //    Catch ex As Exception
    //        Me.TxtCurrentValue.Text = ex.Message
    //    End Try


    //    Try
    //        System.Runtime.InteropServices.Marshal.ReleaseComObject(s)
    //    Catch ex As Exception

    //    End Try

    //    Try
    //        Cursor.Current = Windows.Forms.Cursors.Default
    //        Application.DoEvents()
    //    Catch ex As Exception

    //    End Try
    //    myRegistry = Nothing
    //    Me.BringToFront()
    //End Sub
        #endregion
                   
      }
    }
