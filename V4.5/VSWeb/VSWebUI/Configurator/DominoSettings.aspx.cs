using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using VSWebBL;



namespace VSWebUI.Configurator
{
    public partial class webform : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            bool popupopen;
            Control ctrl;
            //Page.Title = "IBM Domino Settings";
           // ValidateDominoPasswordSettings();
            if (!IsPostBack)
            {
				successDivPwd.Style.Value = "display:none";
                readsettings();
            }
            //6/11/2013 NS added
            else
            {
                popupopen = ResetPwdPopupControl.ShowOnPageLoad;
                ctrl = GetPostBackControl(Page);
                //6/11/2013 NS added - if the control that triggered postback is not the Cancel button, update password
                if (popupopen && ctrl != null)
                {
                    if (ctrl.ID != "ResetPwdCancelBtn")
                    {
                        UpdatePassword();
                        TestDominoPwdForCorrectness();
                    }
                }
            }
            if (EXJournalingEnabledCheckBox.Checked)
            {
                ThresholdTextBox.Enabled = true;
            }
            else
            {
                ThresholdTextBox.Enabled = false;
            }
        }

        //6/11/2013 NS added a new function that returns a control that triggered a postback
        public static Control GetPostBackControl(Page page)
        {
            Control control = null;
            string ctrlname = page.Request.Params.Get("__EVENTTARGET");
            if (ctrlname != null && ctrlname != string.Empty)
            {
                control = page.FindControl(ctrlname);
            }
            else
            {
                foreach (string ctl in page.Request.Form.AllKeys)
                {
                    Control c = page.FindControl(ctl) as Control;
                    if (c is DevExpress.Web.ASPxButton)
                    {
                        control = c;
                        break;
                    }
                }
            }
            return control;
        }

        public void UpdatePassword()
        {
            bool updated = false;
            byte[] MyPass;
            //VSFramework.RegistryHandler myRegistry = new VSFramework.RegistryHandler();
            VSFramework.TripleDES mySecrets = new VSFramework.TripleDES();
            try
            {
                MyPass = mySecrets.Encrypt(ResetPwdTextBox.Text);
                //myRegistry.WriteToRegistry("Password", MyPass);
                System.Text.StringBuilder newstr = new System.Text.StringBuilder();
                foreach (byte b in MyPass)
                {
                    newstr.AppendFormat("{0}, ", b);
                }
                string bytepwd = newstr.ToString();
                int n = bytepwd.LastIndexOf(", ");
                bytepwd = bytepwd.Substring(0, n);
                //10/25/2013 NS modified - Mukund updated the function to include data type
                //updated = VSWebBL.SettingBL.SettingsBL.Ins.UpdateSvalue("Password", bytepwd);
                //1/13/2014 NS modified - depending on which password is being set, update the corresponding Settings column
                if (SetWhichPwd.Text == "NotesPwd")
                {
                    updated = VSWebBL.SettingBL.SettingsBL.Ins.UpdateSvalue("Password", bytepwd, VSWeb.Constants.Constants.SysByte);
                }
                else
                {
                    updated = VSWebBL.SettingBL.SettingsBL.Ins.UpdateSvalue("Domino HTTP Password", bytepwd, VSWeb.Constants.Constants.SysByte);
                }
            }
            catch (Exception ex)
            {
                //1/13/2014 NS modified
                //throw ex;
                //10/3/2014 NS modified for VSPLUS-990
                errorDiv.InnerHtml = "The following error has occurred: " + ex.Message+
                    "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                errorDiv.Style.Value = "display: block";
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
            }
            //ResetPwdPopupControl.ShowOnPageLoad = false;
        }
        public void readsettings()
        {
         try 
	      {
              string Sp = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("Sensitive Operations");
              if (Sp != "" && Sp != null)
              {
                  PromptforpasswordCheckBox.Checked = Convert.ToBoolean(Sp);
              }
              else
              {
                  PromptforpasswordCheckBox.Checked = false;
              }
            // SuppressMultiThrededOperationsCheckBox.Checked=Convert.ToBoolean(VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("Use SMTP When Domino Down"));
              string notesProgram = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("Notes Program Directory");
              string newnotesPogram = notesProgram.Replace("\\", "/");
              NotesProgramTextBox.Text = newnotesPogram;

              string NotesIDfile = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("Notes User ID");
              string newNotesIDfile = NotesIDfile.Replace("\\", "/");
              NotesIDfileTextBox.Text = newNotesIDfile;
              string NotesINIT = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("Notes.ini");
              string newNotesINIT = NotesINIT.Replace("\\", "/");
              NotesINITextBox.Text = newNotesINIT;

             BlackBerryAgentTextBox.Text = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("BB Agent");
             string Expansion = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("Expansion Factor");
             if(Expansion!=null&&Expansion!="")
             AlertonCheckBox.Checked = bool.Parse(Expansion);
             //string op = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("SuppressMultiThread");
             //if(op!=""&& op!=null)
             //SuppressMultiThrededOperationsCheckBox.Checked = bool.Parse(op);
             string Stuck= VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("Stuck");
             if(Stuck!=""&&Stuck!=null)
             AlertonstuckpendingmessageCheckBox.Checked = bool.Parse(Stuck);
             SendpendingmailAlertTextBox.Text = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("minutes");

             //10/4/2013 NS added
             string ExJournalEnabled = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("Enable ExJournal");
             if (ExJournalEnabled != null && ExJournalEnabled != "")
             {
                 EXJournalingEnabledCheckBox.Checked = bool.Parse(ExJournalEnabled);
                 string ExJournalThreshold = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("ExJournal Threshold");
                 if (ExJournalThreshold != null && ExJournalThreshold != "")
                 {
                     ThresholdTextBox.Text = ExJournalThreshold;
                 }
             }

             string EnableConsoleCommands = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("Enable Domino Console Commands");
             if (EnableConsoleCommands != null && EnableConsoleCommands != "")
             {
                 EnableConsoleCommandCheckBox.Checked = bool.Parse(EnableConsoleCommands);
             }
             //Mukund 27-Jan-14, VSPLUS-310
             DominoUsernameTextBox.Text = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("Domino HTTP User");
             //2/12/2014 NS added for VSPLUS-377
             //06Aug14, Mukund, removed for VSPLUS-864 IBM Domino Settings
             //string FwdCmdsEnabled = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("Enable Domino Console Commands");
             //if (FwdCmdsEnabled != null && FwdCmdsEnabled != "")
             //{
             //    FwdRemoteConsoleCmdsCheckBox.Checked = bool.Parse(FwdCmdsEnabled);
             //}
             //CY: 4/7/14 added for VS 476
             ConsecutiveTelnetTextBox.Text = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("ConsecutiveTelnet");

             //06Aug14, Mukund, removed for VSPLUS-864 IBM Domino Settings
             //string MutexNotesAPIEnabled = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("Use_NotesAPI_Mutex");
             //if (MutexNotesAPIEnabled != null && MutexNotesAPIEnabled != "")
             //{
             //    MutexNotesAPICheckBox.Checked = bool.Parse(MutexNotesAPIEnabled);
             //}
	      }
	    catch (Exception ex)
	    {
            //6/27/2014 NS added for VSPLUS-634
            Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
        	throw ex;
	    }
    }

       public void writesettings()
        {
            bool returnval = false;
            string NotUpdatedSettingsNames = "";

            string Promptforpassword,  NotesProgram, NotesIDfile, NotesINI, Expansion, Stuck, BBAgent, minutes, threshold, DominoUserName;
            try
            {
                if (PromptforpasswordCheckBox.Checked)
                {
                    Promptforpassword = "True";
                }
                else
                {
                    Promptforpassword = "False";
                }
                returnval = VSWebBL.SettingBL.SettingsBL.Ins.UpdateSvalue("Sensitive Operations", Promptforpassword, VSWeb.Constants.Constants.SysString);
               if (returnval == false) NotUpdatedSettingsNames += " Promptforpassword ,";
               
            }
            catch (Exception ex)
            {
                //1/13/2014 NS modified
                //throw a;
                successDiv.Style.Value = "display: none";
                //10/3/2014 NS modified for VSPLUS-990
                errorDiv.InnerHtml = "The following error has occurred: " + ex.Message+
                    "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                errorDiv.Style.Value = "display: block";
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
            }

            try
            {
                if(AlertonstuckpendingmessageCheckBox.Checked)
                
                {
                    Stuck = "True";
                }
                else
                {
                    Stuck = "False";
                }
                returnval = VSWebBL.SettingBL.SettingsBL.Ins.UpdateSvalue("Stuck", Stuck, VSWeb.Constants.Constants.SysString);
               if (returnval == false) NotUpdatedSettingsNames += "Alert on stuck pending message ,";

            }
            catch (Exception ex)
            {
                //1/13/2014 NS modified
                //throw a;
                successDiv.Style.Value = "display: none";
                //10/3/2014 NS modified for VSPLUS-990
                errorDiv.InnerHtml = "The following error has occurred: " + ex.Message+
                    "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                errorDiv.Style.Value = "display: block";
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
            }

            try
            {
                if(AlertonCheckBox.Checked==true)
               
                {
                    Expansion = "True";
                }
                else
                {
                    Expansion= "False";
                }
                returnval = VSWebBL.SettingBL.SettingsBL.Ins.UpdateSvalue("Expansion Factor", Expansion, VSWeb.Constants.Constants.SysString);
                if (returnval == false) NotUpdatedSettingsNames += "Alert on Expansion Factor ,";

            }
            catch (Exception ex)
            {
                //1/13/2014 NS modified
                //throw a;
                successDiv.Style.Value = "display: none";
                //10/3/2014 NS modified for VSPLUS-990
                errorDiv.InnerHtml = "The following error has occurred: " + ex.Message+
                    "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                errorDiv.Style.Value = "display: block";
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
            }

            //06Aug14, Mukund, removed for VSPLUS-864 IBM Domino Settings

            //try
            //{

            //    if (SuppressMultiThrededOperationsCheckBox.Checked)
            //    {
            //        SuppressMultiThrededOperations = "True";

            //    }
            //    else
            //    {
            //        SuppressMultiThrededOperations = "False";
            //    }

            //    returnval = VSWebBL.SettingBL.SettingsBL.Ins.UpdateSvalue("SuppressMultiThread", SuppressMultiThrededOperations, VSWeb.Constants.Constants.SysString);
            //   if (returnval == false) NotUpdatedSettingsNames += "Suppress MultiThreaded Operations ,";
            //}
            //catch (Exception ex)
            //{
            //    //1/13/2014 NS modified
            //    //throw b;
            //    successDiv.Style.Value = "display: none";
            //    errorDiv.InnerHtml = "The following error has occurred: " + ex.Message;
            //    errorDiv.Style.Value = "display: block";
            //    //6/27/2014 NS added for VSPLUS-634
            //    Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
            //}
            try
            {
                if (NotesProgramTextBox.Text != "" && NotesProgramTextBox.Text != null)
                {
                    NotesProgram = NotesProgramTextBox.Text;
                    returnval = VSWebBL.SettingBL.SettingsBL.Ins.UpdateSvalue("Notes Program Directory", NotesProgram, VSWeb.Constants.Constants.SysString);
                   if (returnval == false) NotUpdatedSettingsNames += "Notes Program Directory,";
                }
            }
            catch (Exception ex)
            {
                //1/13/2014 NS modified
                //throw c;
                successDiv.Style.Value = "display: none";
                //10/3/2014 NS modified for VSPLUS-990
                errorDiv.InnerHtml = "The following error has occurred: " + ex.Message+
                    "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                errorDiv.Style.Value = "display: block";
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
            }

            try
            {
                if (NotesIDfileTextBox.Text != "" && NotesIDfileTextBox.Text != null)
                {
                    NotesIDfile = NotesIDfileTextBox.Text;
                    returnval = VSWebBL.SettingBL.SettingsBL.Ins.UpdateSvalue("Notes User ID", NotesIDfile, VSWeb.Constants.Constants.SysString);
                   if (returnval == false) NotUpdatedSettingsNames += "Notes User ID,";

                }
            }
            catch (Exception ex)
            {
                //1/13/2014 NS modified
                //throw d;
                successDiv.Style.Value = "display: none";
                //10/3/2014 NS modified for VSPLUS-990
                errorDiv.InnerHtml = "The following error has occurred: " + ex.Message+
                    "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                errorDiv.Style.Value = "display: block";
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
            }

            try
            {
                if (BlackBerryAgentTextBox.Text != "" && BlackBerryAgentTextBox.Text != null)
                {
                    BBAgent = BlackBerryAgentTextBox.Text;
                    returnval = VSWebBL.SettingBL.SettingsBL.Ins.UpdateSvalue("BB Agent", BBAgent, VSWeb.Constants.Constants.SysString);
                   if (returnval == false) NotUpdatedSettingsNames += "BlackBerry Agent,";
                }
            }
            catch (Exception ex)
            {
                //1/13/2014 NS modified
                //throw d;
                successDiv.Style.Value = "display: none";
                //10/3/2014 NS modified for VSPLUS-990
                errorDiv.InnerHtml = "The following error has occurred: " + ex.Message+
                    "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                errorDiv.Style.Value = "display: block";
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
            }
            try
            {
                if (SendpendingmailAlertTextBox.Text != "" && SendpendingmailAlertTextBox.Text != null)
                {
                   minutes = SendpendingmailAlertTextBox.Text;
                   returnval = VSWebBL.SettingBL.SettingsBL.Ins.UpdateSvalue("minutes", minutes, VSWeb.Constants.Constants.SysString);
                 if (returnval == false) NotUpdatedSettingsNames += "minutes,";
                }
            }
            catch (Exception ex)
            {
                //1/13/2014 NS modified
                //throw d;
                successDiv.Style.Value = "display: none";
                //10/3/2014 NS modified for VSPLUS-990
                errorDiv.InnerHtml = "The following error has occurred: " + ex.Message+
                    "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                errorDiv.Style.Value = "display: block";
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
            }

           //10/4/2013 NS added
            try
            {
                if (EXJournalingEnabledCheckBox.Checked)
                {
                    returnval = VSWebBL.SettingBL.SettingsBL.Ins.UpdateSvalue("Enable ExJournal", "true", VSWeb.Constants.Constants.SysString);
                }
                else
                {
                    returnval = VSWebBL.SettingBL.SettingsBL.Ins.UpdateSvalue("Enable ExJournal", "false", VSWeb.Constants.Constants.SysString);
                }
                if (returnval == false) NotUpdatedSettingsNames += "Enable ExJournal,";
                if (ThresholdTextBox.Text != "" && ThresholdTextBox.Text != null)
                {
                    threshold = ThresholdTextBox.Text;
                    returnval = VSWebBL.SettingBL.SettingsBL.Ins.UpdateSvalue("ExJournal Threshold", threshold, VSWeb.Constants.Constants.SysString);
                    if (returnval == false) NotUpdatedSettingsNames += "ExJournal Threshold,";
                }
                //CY: 4/7/14 added for VS 476
                if (ConsecutiveTelnetTextBox.Text != "" && ConsecutiveTelnetTextBox.Text != null)
                {
                    returnval = VSWebBL.SettingBL.SettingsBL.Ins.UpdateSvalue("ConsecutiveTelnet", ConsecutiveTelnetTextBox.Text, VSWeb.Constants.Constants.SysInt32);
                    if (returnval == false) NotUpdatedSettingsNames += "ConsecutiveTelnet,";
                }
            }
            catch (Exception ex)
            {
                //1/13/2014 NS modified
                //throw e;
                successDiv.Style.Value = "display: none";
                //10/3/2014 NS modified for VSPLUS-990
                errorDiv.InnerHtml = "The following error has occurred: " + ex.Message+
                    "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                errorDiv.Style.Value = "display: block";
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
            }

            try
            {
                if (EnableConsoleCommandCheckBox.Checked)
                {
                    returnval = VSWebBL.SettingBL.SettingsBL.Ins.UpdateSvalue("Enable Domino Console Commands", "true", VSWeb.Constants.Constants.SysString);
                }
                else
                {
                    returnval = VSWebBL.SettingBL.SettingsBL.Ins.UpdateSvalue("Enable Domino Console Commands", "false", VSWeb.Constants.Constants.SysString);
                }
            }
            catch (Exception ex)
            {
                successDiv.Style.Value = "display: none";
                //10/3/2014 NS modified for VSPLUS-990
                errorDiv.InnerHtml = "The following error has occurred: " + ex.Message +
                    "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                errorDiv.Style.Value = "display: block";
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
            }
            try
            {
                if (NotesINITextBox.Text != "" && NotesINITextBox.Text != null)
                {
                    NotesINI = NotesINITextBox.Text;
                    returnval = VSWebBL.SettingBL.SettingsBL.Ins.UpdateSvalue("Notes.ini", NotesINI, VSWeb.Constants.Constants.SysString);
                    if (returnval == false) NotUpdatedSettingsNames += "Notes.ini,";
                }
            }
            catch (Exception ex)
            {
                //1/13/2014 NS modified
                //throw f;
                successDiv.Style.Value = "display: none";
                //10/3/2014 NS modified for VSPLUS-990
                errorDiv.InnerHtml = "The following error has occurred: " + ex.Message+
                    "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                errorDiv.Style.Value = "display: block";
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
            }

            //1/13/2014 NS added
            try
            {
                if (DominoUsernameTextBox.Text != "" && DominoUsernameTextBox.Text != null)
                {
                    DominoUserName = DominoUsernameTextBox.Text;
                    returnval = VSWebBL.SettingBL.SettingsBL.Ins.UpdateSvalue("Domino HTTP User", DominoUserName, VSWeb.Constants.Constants.SysString);
                    if (returnval == false) NotUpdatedSettingsNames += "Domino HTTP User,";
                }
            }
            catch (Exception ex)
            {
                //1/13/2014 NS modified
                //throw g;
                successDiv.Style.Value = "display: none";
                //10/3/2014 NS modified for VSPLUS-990
                errorDiv.InnerHtml = "The following error has occurred: " + ex.Message+
                    "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                errorDiv.Style.Value = "display: block";
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
            }

		    //11/12/15 WS ADDED FOR VSPLUS 2299
			try
			{
				VSWebBL.ConfiguratorBL.DominoPropertiesBL.Ins.ForceDominoTableRefresh();
			}
			catch (Exception ex)
			{
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
			}

			//2/12/2014 NS added for VSPLUS-377
			//06Aug14, Mukund, removed for VSPLUS-864 IBM Domino Settings
			//try
			//{
			//    if (FwdRemoteConsoleCmdsCheckBox.Checked)
			//    {
			//        returnval = VSWebBL.SettingBL.SettingsBL.Ins.UpdateSvalue("Enable Domino Console Commands", "true", VSWeb.Constants.Constants.SysString);
			//    }
			//    else
			//    {
			//        returnval = VSWebBL.SettingBL.SettingsBL.Ins.UpdateSvalue("Enable Domino Console Commands", "false", VSWeb.Constants.Constants.SysString);
			//    }
			//    if (returnval == false) NotUpdatedSettingsNames += "Forward Remote Console Commands,";
			//}
			//catch (Exception ex)
			//{
			//    successDiv.Style.Value = "display: none";
			//    errorDiv.InnerHtml = "The following error has occurred: " + ex.Message;
			//    errorDiv.Style.Value = "display: block";
			//    //6/27/2014 NS added for VSPLUS-634
			//    Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
			//}

			finally
			{
				if (NotUpdatedSettingsNames == "")
				{
					//12/24/2013 NS modified
					//MsgPopupControl.ShowOnPageLoad = true;
					//MsgLabel.Text = "Data updated successfully.";
					errorDiv.Style.Value = "display: none";
					successDiv.Style.Value = "display: block";
				}
				else
				{
					//12/24/2013 NS modified
					//MsgPopupControl.ShowOnPageLoad = true;
					//MsgLabel.Text = NotUpdatedSettingsNames + " were not updated.";
					successDiv.Style.Value = "display: none";
					//10/3/2014 NS modified for VSPLUS-990
					errorDiv.InnerHtml = "The following fields were not updated: " + NotUpdatedSettingsNames +
						"<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
					errorDiv.Style.Value = "display: block";
				}

				//06Aug14, Mukund, removed for VSPLUS-864 IBM Domino Settings

				//try
				//{

				//    if (MutexNotesAPICheckBox.Checked)
				//    {
				//        MutexNotesAPI = "True";

				//    }
				//    else
				//    {
				//        MutexNotesAPI = "False";
				//    }

				//    returnval = VSWebBL.SettingBL.SettingsBL.Ins.UpdateSvalue("Use_NotesAPI_Mutex", MutexNotesAPI, VSWeb.Constants.Constants.SysString);
				//    if (returnval == false) NotUpdatedSettingsNames += "User Notes API Mutex,";
				//}
				//catch (Exception ex)
				//{
				//    //1/13/2014 NS modified
				//    //throw b;
				//    successDiv.Style.Value = "display: none";
				//    errorDiv.InnerHtml = "The following error has occurred: " + ex.Message;
				//    errorDiv.Style.Value = "display: block";
				//    //6/27/2014 NS added for VSPLUS-634
				//    Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				//}
			}

        }



      //public void ValidateDominoPasswordSettings()
      //  {
      //      Domino.NotesSession Session = new Domino.NotesSession();

      //      try
      //      {
      //         // Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
      //          VitalSingsLabel.Text = "Please wait...testing Notes connection";
      //        //  Application.DoEvents();

      //          object myEncryptedPassword = null;
      //          //RegistryHandler myRegistry = new RegistryHandler();
      //          string MyDominoPassword = "";
      //          try
      //          {
      //            //  myEncryptedPassword = myRegistry.ReadFromRegistry("Password");
      //          }
      //          catch (Exception ex)
      //          {
      //              myEncryptedPassword = null;
      //              MyDominoPassword = null;
      //          }

      //          if ((myEncryptedPassword != null))
      //          {
      //              byte[] MyPass = null;
      //          //    MyPass = myRegistry.ReadFromRegistry("Password");
      //              //password as encrypted byte stream
      //          //   TripleDES mySecrets = new TripleDES();
      //              //Get the password from the registry
      //            //  MyDominoPassword = mySecrets.Decrypt(myEncryptedPassword);
      //              //password in clear text
      //          }

      //          if (MyDominoPassword == null | string.IsNullOrEmpty(MyDominoPassword))
      //          {
      //              VitalSingsLabel.BackColor = System.Drawing.Color.Red;
      //              VitalSingsLabel.ForeColor = System.Drawing.Color.White;
      //              VitalSingsLabel.Text = "You must register a Notes password with " + myCaller.MyProductName + " so that the Windows Service application can monitor Domino servers." + " This password is required because a Windows Service cannot prompt for a password, since it has no User Interface.  The password is encrypted prior to saving.";
      //             // Cursor.Current = System.Windows.Forms.Cursors.Default;
      //              VitalSingsLabel.Height = 74;
      //              goto CleanUp;
      //          }

      //          string MyIDName = null;
      //          //See if the password is OK, 
      //          if ((MyDominoPassword != null))
      //          {
      //              try
      //              {
      //                  Session.Initialize(MyDominoPassword);
      //                  MyIDName = Session.CommonUserName;
      //                  if (string.IsNullOrEmpty(MyIDName))
      //                  {
      //                      VitalSingsLabel.BackColor = System.Drawing.Color.Red;
      //                      VitalSingsLabel.ForeColor = System.Drawing.Color.White;
      //                      VitalSingsLabel.Height = 74;
      //                     // VitalSingsLabel.Text = "You must register a Notes password with " + myCaller.MyProductName + " so that the Windows Service application can monitor Domino servers." + " This password is required because a Windows Service with no interface cannot prompt for a password.  The password is encrypted prior to saving.";
      //                  }
      //                  else
      //                  {
      //                      VitalSingsLabel.BackColor = System.Drawing.Color.LightGreen;
      //                      VitalSingsLabel.ForeColor = System.Drawing.Color.Black;
      //                      VitalSingsLabel.Height = 74;
      //                     // VitalSingsLabel.Text = myCaller.MyProductName + " is using the ID: <b>" + MyIDName + "</b>.  The password has been entered, tested, and encrypted for use by the background monitoring service. <br/><br/> <b>Important:</b> If you switch IDs using the Lotus Notes client, you must provide the new password for " + myCaller.MyProductName + " to function.";
      //                     // Application.DoEvents();
      //                      try
      //                      {
      //                          string FileToCopy = "";
      //                          string NewCopy = "";
      //                          string Directory = "";
      //                          // will store Notes program directory
      //                          Directory = Session.GetEnvironmentString("Directory", true);
      //                          //returns the data folder, such as C:\Program Files\lotus\notes\data

      //                          FileToCopy = AppDomain.CurrentDomain.BaseDirectory() + "Data\\vitalstatus.ntf";
      //                          NewCopy = Directory + "\\vitalstatus.ntf";

      //                          if (System.IO.File.Exists(FileToCopy) == true)
      //                          {
      //                              if (System.IO.File.Exists(NewCopy))
      //                              {
      //                                  System.IO.File.Delete(NewCopy);
      //                              }
      //                              System.IO.File.Copy(FileToCopy, NewCopy);
      //                              //  MsgBox("File Copied")
      //                          }

      //                      }
      //                      catch (Exception ex)
      //                      {
      //                          // MsgBox(ex.ToString)
      //                      }

      //                  }

      //                  if (string.IsNullOrEmpty(NotesIDfileTextBox.Text) & string.IsNullOrEmpty(NotesProgramTextBox.Text))
      //                  {
      //                      string KeyFile = "";
      //                      // will store the name of the ID File
      //                      string Directory = "";
      //                      // will store Notes program directory
      //                      string FullKey = "";

      //                      try
      //                      {
      //                          KeyFile = Session.GetEnvironmentString("KeyFilename", true);
      //                          Directory = Session.GetEnvironmentString("Directory", true);
      //                          //returns the data folder, such as C:\Program Files\lotus\notes\data
      //                         // myRegistry.WriteToRegistry("Notes Data Directory", Directory);
      //                          FullKey = Directory + "\\" + KeyFile;
      //                          this.NotesIDfileTextBox.Text = FullKey;
      //                          this.NotesINITextBox.Text = Directory + "\\notes.ini";


      //                      }
      //                      catch (Exception ex)
      //                      {
      //                      }




      //                     // Microsoft.Win32.RegistryKey aKey = null;
      //                      string strNotesPath = "";
      //                      try
      //                      {
      //                        //  aKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(".nsf\\Shell\\Open\\Command");
      //                          //strNotesPath = aKey.GetValueNames
      //                        //  strNotesPath = aKey.GetValue("");
      //                          // will return something like C:\Program Files\lotus\notes\notes.exe /defini %1
      //                          int EndPos = 0;
      //                          if(strNotesPath == "")
      //                          {
      //                           EndPos = (strNotesPath.ToUpper+,+"\\NOTES.EXE");
      //                          }
      //                          strNotesPath = strNotesPath.Substring(0, EndPos);
      //                          this.NotesProgramTextBox.Text = strNotesPath;
      //                      }
      //                      catch (Exception ex)
      //                      {
      //                        //  MessageBox.Show("Please enter the location of the Notes program folder, where Notes.exe can be found.");
      //                          this.NotesProgramTextBox.Focus();
      //                      }

      //                  }
      //              }
      //              catch (Exception ex)
      //              {
      //                  VitalSingsLabel.Text = "Error initializing Notes session. Please register your Notes password and try again.  If you have already registered the password but continue to get this error message, you should re-install the Notes client on this machine.  Details: " + ex.Message;
      //              }
      //              finally
      //              {
      //                  System.Runtime.InteropServices.Marshal.ReleaseComObject(Session);
      //              }
      //          }

      //          // Me.SpringPanel1.Text = ""
      //      }
      //      catch (Exception ex)
      //      {
      //         // Cursor.Current = System.Windows.Forms.Cursors.Default;
      //          //  Me.SpringPanel1.Text = ""
      //      }
      //  CleanUp:

      //      try
      //      {
      //          System.Runtime.InteropServices.Marshal.ReleaseComObject(Session);

      //      }
      //      catch (Exception ex)
      //      {
      //      }

      //    //  Cursor.Current = System.Windows.Forms.Cursors.Default;
      //  }

      public void ValidateNotesPath()
      {
          //string path = Environment.GetEnvironmentVariable("PATH");
          ////";")
          //string[] paths =path.Split(';');
          //// Dim messageOutput As String

          //bool InPath = false;
          //foreach (string pathItem in paths)
          //{
          //    if (pathItem.ToUpper()=="NOTES")
          //    {
          //        InPath = true;
          //    }
          //}
          //if (InPath == false)
          //{
          //  //  MessageBox.Show("You must add the Lotus Notes program directory to your system path for VitalSigns to work. Please double-check this.", "Error");
          //}
      }

 //     private void WritePreferences()
      
         // try
         // {
         //     //Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
         //     StatusText.Text = "Saving Preferences...";
         //    // Application.DoEvents();

         // }
         // catch (Exception ex)
         // {
         // }

         // try
         // {
         //     ProgressBar1.Maximum = 10;
         //     this.ProgressBar1.Value = 1;
         //     //Application.DoEvents();

         // }
         // catch (Exception ex)
         // {
         // }

         // //Save the current values of the preferences into the registry
         //// RegistryHandler myRegistry = new RegistryHandler();
         // string myPreferenceString = "";


         // try
         // {
         //     if (rdoDigitPlainText.Checked == true)
         //     {
         //         myRegistry.WriteToRegistry("FontSize", SliderFontSize.Value);
         //         myCaller.MyFontSize = SliderFontSize.Value;
         //         myRegistry.WriteToRegistry("FontBold", cbBold.Checked);
         //         myCaller.MyFontBold = cbBold.Checked;
         //     }


         // }
         // catch (Exception ex)
         // {
         // }

         // //Sametime
         // try
         // {
         //  //   myRegistry.WriteToRegistry("Sametime Username", txtSametimeUserName.Text);
         // }
         // catch (Exception ex)
         // {
         //   //  myRegistry.WriteToRegistry("Sametime Username", "");
         // }

         // try
         // {
         //    // myRegistry.WriteToRegistry("AdvancedSametime", cbAdvancedSametime.Checked);
         // }
         // catch (Exception ex)
         // {
         //    // myRegistry.WriteToRegistry("AdvancedSametime", false);
         // }


         // //write the preferences for multithreaded operation
         // try
         // {
         //     //myRegistry.WriteToRegistry("SuppressMultiThread", cbMultithread.Checked);
         // }
         // catch (Exception ex)
         // {
         //   //  myRegistry.WriteToRegistry("SuppressMultiThread", false);
         // }


         // //Stuck Messages

         // try
         // {
         //     //myRegistry.WriteToRegistry("AlertOnStuckMessages", CheckBoxXStuckMessages.Checked);
         // }
         // catch (Exception ex)
         // {
         //    // myRegistry.WriteToRegistry("AlertOnStuckMessages", false);
         // }


         // try
         // {
         //  //   myRegistry.WriteToRegistry("StuckMessageThreshold", TextBoxXStuckMessages.Text);
         // }
         // catch (Exception ex)
         // {
         //    // myRegistry.WriteToRegistry("StuckMessageThreshold", "15");
         // }

         // try
         // {
         //    // myRegistry.WriteToRegistry("Alert Repeat", cbRepeatAlerts.Checked);
         // }
         // catch (Exception ex)
         // {
         //    // myRegistry.WriteToRegistry("Alert Repeat", false);
         // }

         // try
         // {
         //   //  myRegistry.WriteToRegistry("Alert Repeat Interval", txtRepeatAlertInterval.Text);
         // }
         // catch (Exception ex)
         // {
         //    // myRegistry.WriteToRegistry("Alert Repeat Interval", 15);
         // }


         // //SQL Server stuff
         // myRegistry.WriteToRegistry("Use SQL Server", cbSQLServer.Checked);
         // myRegistry.WriteToRegistry("SQL Security", cbSQLSecurity.Checked);
         // myRegistry.WriteToRegistry("SQL Server", txtSQLServerName.Text);
         // if (cbSQLSecurity.Checked == false & !string.IsNullOrEmpty(txtSQLPassword.Text))
         // {
         //     myRegistry.WriteToRegistry("SQL Username", txtSQLUsername.Text);
         //     byte[] MyPass = null;
         //     //convert the plain text password to an encrypted byte stream
         //     TripleDES mySecrets = new TripleDES();
         //     MyPass = mySecrets.Encrypt(txtSQLPassword.Text);
         //     //save the encrypted password to the registry
         //     myRegistry.WriteToRegistry("SQLPassword", MyPass);
         // }


         // if (!string.IsNullOrEmpty(TextBoxBannerText.Text))
         // {
         //     myRegistry.WriteToRegistry("BannerText", TextBoxBannerText.Text);
         //     myCaller.BannerText = TextBoxBannerText.Text;

         //     // LabelBanner.Text = TextBoxBannerText.Text
         // }
         // else
         // {
         //     myRegistry.WriteToRegistry("BannerText", "VitalSigns Server Monitor");
         //     myCaller.BannerText = "VitalSigns Server Monitor";

         // }
         // myCaller.BannerText = TextBoxBannerText.Text;

         // try
         // {
         //     if (!string.IsNullOrEmpty(TextBoxProductName.Text))
         //     {
         //         myRegistry.WriteToRegistry("ProductName", TextBoxProductName.Text);
         //     }
         //     myCaller.Text = TextBoxProductName.Text;
         //     // myCaller.ProductName = TextBoxProductName.Text

         // }
         // catch (Exception ex)
         // {
         // }

         // try
         // {
         //     this.ProgressBar1.Value = 2;
         //    // Application.DoEvents();

         // }
         // catch (Exception ex)
         // {
         // }

         // if (this.cbVerboseLog.CheckState == CheckState.Checked)
         // {
         //     myRegistry.WriteToRegistry("Log Level", 0);
         // }
         // else
         // {
         //     myRegistry.WriteToRegistry("Log Level", 1);
         // }

         // //If Me.txtHTMLPath.Text <> "" Then
         // //    myRegistry.WriteToRegistry("HTML Path", Me.txtHTMLPath.Text)
         // //    myRegistry.WriteToRegistry("Enable HTML", Me.cbHTML2.Checked)
         // //End If

         // try
         // {
         //     this.ProgressBar1.Value = 4;
         //    // Application.DoEvents();

         // }
         // catch (Exception ex)
         // {
         // }


         // //Notes Output

         // if (this.cbXNotesOutput.Checked == true)
         // {
         //     myRegistry.WriteToRegistry("Notes Output", cbXNotesOutput.Checked);
         //     myRegistry.WriteToRegistry("Notes Output Database", txtOutputDatabase.Text);
         //     myRegistry.WriteToRegistry("Notes Output Server", cmbOutputServer.Text);
         // }
         // else
         // {
         //     myRegistry.WriteToRegistry("Notes Output", "False");
         // }


         // try
         // {
         //     myRegistry.WriteToRegistry("Backup Data", cbBackup.Checked);

         // }
         // catch (Exception ex)
         // {
         // }


         // try
         // {
         //     myRegistry.WriteToRegistry("Show ToolTips", cbToolTips.Checked);
         //     myCaller.boolShowToolTips = cbToolTips.Checked;
         // }
         // catch (Exception ex)
         // {
         //     myCaller.boolShowToolTips = true;
         // }

         // try
         // {
         //     myRegistry.WriteToRegistry("Freeze Names", this.chkFreeze.Checked);
         //     myCaller.boolFreezeColumns = this.CheckBoxSaveWindow.Checked;
         // }
         // catch (Exception ex)
         // {
         //     //keeps the device name from scrolling off the screen
         //     myCaller.boolFreezeColumns = true;
         // }


         // try
         // {
         //     myRegistry.WriteToRegistry("Save Window", this.CheckBoxSaveWindow.Checked);
         //     myCaller.boolSaveWindowSettings = this.CheckBoxSaveWindow.Checked;
         // }
         // catch (Exception ex)
         // {
         //     myCaller.boolSaveWindowSettings = true;
         // }

         // try
         // {
         //     myRegistry.WriteToRegistry("Show Disabled Devices", this.cbShowDisabled.Checked);
         //     myCaller.boolShowDisabled = this.cbShowDisabled.Checked;
         // }
         // catch (Exception ex)
         // {
         //     myCaller.boolShowDisabled = false;
         // }

         // try
         // {
         //     this.ProgressBar1.Value = 6;
         //     Application.DoEvents();

         // }
         // catch (Exception ex)
         // {
         // }

         // try
         // {
         //     myRegistry.WriteToRegistry("SNMP Port", this.txtSNMPPort.Text);
         // }
         // catch (Exception ex)
         // {
         //     myRegistry.WriteToRegistry("SNMP Port", "161");
         // }


         // if (!string.IsNullOrEmpty(TextBoxRefreshInterval.Text))
         // {
         //     myRegistry.WriteToRegistry("Refresh Interval", TextBoxRefreshInterval.Text);
         //     //to do -- test for a valid positive number in range from 15 to 300 seconds
         //     Int16 myInteger = default(Int16);
         //     try
         //     {
         //         myInteger = Convert.ToInt32(TextBoxRefreshInterval.Text);
         //         if (myInteger < 15)
         //         {
         //             myRegistry.WriteToRegistry("Refresh Interval", "15");
         //         }
         //         if (myInteger > 300)
         //         {
         //             myRegistry.WriteToRegistry("Refresh Interval", "300");
         //         }
         //     }
         //     catch (Exception ex)
         //     {
         //         myRegistry.WriteToRegistry("Refresh Interval", "30");
         //     }
         // }
         // else
         // {
         //     myRegistry.WriteToRegistry("Refresh Interval", "30");
         // }

         // try
         // {
         //     myRegistry.WriteToRegistry("StatusTabWelcomePage", this.cbStatusWelcomePreference.Checked);


         // }
         // catch (Exception ex)
         // {
         // }

         // try
         // {
         //     this.ProgressBar1.Value = 8;
         //     Application.DoEvents();

         // }
         // catch (Exception ex)
         // {
         // }


         // try
         // {
         //     if (myCaller.useColorMeter == true)
         //     {
         //         myRegistry.WriteToRegistry("PercentPreference", "Color Meter");
         //     }
         //     else if (myCaller.useColorBar == true)
         //     {
         //         myRegistry.WriteToRegistry("PercentPreference", "Color Bar");
         //     }
         //     else
         //     {
         //         myRegistry.WriteToRegistry("PercentPreference", "Plain text");
         //     }

          //}
          //catch (Exception ex)
          //{
          //}

          //try
          //{
          //    this.ProgressBar1.Value = 10;
          //    Application.DoEvents();

          //}
          //catch (Exception ex)
          //{
          //}


      //    try
      //    {
      //        if (myCaller.useLEDS == true)
      //        {
      //            myRegistry.WriteToRegistry("DigitPreference", "LEDs");
      //        }
      //        else
      //        {
      //            myRegistry.WriteToRegistry("DigitPreference", "Plain Text");
      //        }

      //    }
      //    catch (Exception ex)
      //    {
      //    }

      //    try
      //    {
      //        this.ProgressBar1.Value = 0;
      //        Application.DoEvents();

      //    }
      //    catch (Exception ex)
      //    {
      //    }

      //    try
      //    {
      //        StatusText.Text = "Ready";
      //        btnUIPrefApply.Enabled = false;
      //        Cursor.Current = System.Windows.Forms.Cursors.Default;

      //    }
      //    catch (Exception ex)
      //    {
      //    }

      //    try
      //    {
      //        myRegistry.WriteToRegistry("BusinessHoursEnd", this.TxtEnd.Text);
      //        myRegistry.WriteToRegistry("BusinessHoursStart", this.TxtStart.Text);
      //        myRegistry.WriteToRegistry("24 Hours", this.cb24Hours.Checked);

      //    }
      //    catch (Exception ex)
      //    {
      //    }

      //    SetCanceltoClose();

     // }

      
     





        protected void SaveButton_Click(object sender, EventArgs e)
        {
			errorDivPwd.Style.Value = "width: 130px; display: none";
			successDivPwd.Style.Value = "width: 130px; display: none";
			successDiv.Style.Value = "display: none";
            writesettings();

       }

        protected void CancelButton_Click(object sender, EventArgs e)
        {
           //PromptforpasswordCheckBox.Checked = false;
           //SuppressMultiThrededOperationsCheckBox.Checked = false;
           //NotesProgramTextBox.Text = "";
           //NotesIDfileTextBox.Text = "";
           //NotesINITextBox.Text = "";
            Response.Redirect("~/Configurator/Default.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
            Context.ApplicationInstance.CompleteRequest();


        }

       protected void RegisterButton_Click(object sender, EventArgs e)
       {
           //1/13/2014 NS added
           SetWhichPwd.Text = "NotesPwd";
		   successDiv.Style.Value = "display: none";
		   errorDivPwd.Style.Value = "width: 130px; display: none";
		   successDivPwd.Style.Value = "width: 130px; display: none";

           if (!ResetPwdPopupControl.ShowOnPageLoad)
           {
               ResetPwdPopupControl.ShowOnPageLoad = true;
           }
           else
           {
               ResetPwdPopupControl.ShowOnPageLoad = false;
           }
       }

       protected void ASPxButton1_Click(object sender, EventArgs e)
       {
           MsgPopupControl.ShowOnPageLoad = false;
       }

       protected void ResetPwdCancelBtn_Click(object sender, EventArgs e)
       {
           ResetPwdPopupControl.ShowOnPageLoad = false;
       }

       protected void ResetPwdOKBtn_Click(object sender, EventArgs e)
       {
           ResetPwdPopupControl.ShowOnPageLoad = false;
       }

       protected void EXJournalingEnabledCheckBox_CheckedChanged(object sender, EventArgs e)
       {
           if (EXJournalingEnabledCheckBox.Checked)
           {
               ThresholdTextBox.Enabled = true;
           }
           else
           {
               ThresholdTextBox.Enabled = false;
           }
       }

       protected void EnableConsoleCommandCheckBox_CheckedChanged(object sender, EventArgs e)
       {
           if (EnableConsoleCommandCheckBox.Checked)
           {
           }
       }

       protected void RegisterDominoPwdButton_Click(object sender, EventArgs e)
       {
           //1/13/2014 NS added
           SetWhichPwd.Text = "DominoPwd";
           if (!ResetPwdPopupControl.ShowOnPageLoad)
           {
               ResetPwdPopupControl.ShowOnPageLoad = true;
           }
           else
           {
               ResetPwdPopupControl.ShowOnPageLoad = false;
           }
       }

       private void TestDominoPwdForCorrectness()
       {
           //2/10/2014 NS modified for VSPLUS-366
           if (SetWhichPwd.Text == "NotesPwd")
           {
               errorDivPwd.Style.Value = "width: 130px; display: none";
               Domino.NotesSession NotesSessionObject = new Domino.NotesSession();
               try
               {
                   NotesSessionObject.Initialize(ResetPwdTextBox.Text);
                   successDivPwd.Style.Value = "width: 130px; display: block";
               }
               catch (Exception ex)
               {
                   errorDivPwd.Style.Value = "width: 130px; display: block";
                   //6/27/2014 NS added for VSPLUS-634
                   Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
               }
           }
           else
           {
               errorDivDomPwd.Style.Value = "width: 130px; display: none";
               Domino.NotesSession NotesSessionObject = new Domino.NotesSession();
               try
               {
                   NotesSessionObject.Initialize(ResetPwdTextBox.Text);
                   successDivDomPwd.Style.Value = "width: 130px; display: block";
               }
               catch (Exception ex)
               {
                   errorDivDomPwd.Style.Value = "width: 130px; display: block";
                   //6/27/2014 NS added for VSPLUS-634
                   Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
               }
           }
       }
    }
}