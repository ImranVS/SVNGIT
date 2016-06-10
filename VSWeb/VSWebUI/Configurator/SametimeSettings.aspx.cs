using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VSWebBL;

namespace VSWebUI.Configurator
{
    public partial class SametimeSettings : System.Web.UI.Page
    {
        
        protected void Page_Load(object sender, EventArgs e)
        {
            //Page.Title = "Sametime Settings";
            if (!IsPostBack)
            {
                ReadSettings();
            }
        }

        protected void StpButton_Click(object sender, EventArgs e)
        {
            WhichUserLabel.Text = "1";
            SameTimePopupControl.ShowOnPageLoad = true;
            passwordLabel.Text = "Please enter the Sametime password for " + UserTextBox.Text + " below:";
            passwordOK.Visible = true;
            passwordTextBox.Visible = true;
        }

        protected void FormCancelButton_Click(object sender, EventArgs e)
        {
            //UserTextBox.Text = "";
            //AdvancedSametimeCheckBox.Checked = false;
            Response.Redirect("~/Configurator/Default.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
            Context.ApplicationInstance.CompleteRequest();

        }

        protected void SametimeapplyButton_Click(object sender, EventArgs e)
        {
            WriteSettings();
        }

        private void ReadSettings()
        {
            try
            {
                //9/24/2013 NS modified
                //UserTextBox.Text = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("User Name");
                UserTextBox.Text = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("Sametime User 1");
            }
            catch (Exception ex)
            {
                //1/30/2014 NS added
                errorDiv.Style.Value = "display: block";
                //10/6/2014 NS modified for VSPLUS-990
                errorDiv.InnerHtml = "The following error has occurred while getting the data for Sametime User 1: " + ex.Message+
                    "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
            }
            //9/24/2013 NS added
            try
            {
                User2TextBox.Text = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("Sametime User 2");
            }
            catch (Exception ex)
            {
                //1/30/2014 NS added
                errorDiv.Style.Value = "display: block";
                //10/6/2014 NS modified for VSPLUS-990
                errorDiv.InnerHtml = "The following error has occurred while getting the data for Sametime User 2: " + ex.Message+
                    "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
            }
            try
            {
                string Adv=VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("AdvancedSametime");
                //SWebBL.SettingBL.SettingsBL.Ins.Getvalue("AdvancedSametime")!=null)
                if (Adv != "" && Adv != null )
                {
                    //9/25/2013 NS modified
                    if (Adv == "1" || Adv == "True")
                    {
                        Adv = "true";
                    }
                    else
                    {
                        Adv = "false";
                    }
                    AdvancedSametimeCheckBox.Checked = bool.Parse(Adv);
                }
               
            }
            catch (Exception ex)
            {
                //1/30/2014 NS added
                errorDiv.Style.Value = "display: block";
                //10/6/2014 NS modified for VSPLUS-990
                errorDiv.InnerHtml = "The following error has occurred while getting the data for Collect extended Sametime statistics: " + ex.Message+
                    "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
            }
            //1/30/2013 NS added for VSPLUS-322
            try
            {
                SametimeServletPortTextBox.Text = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("Sametime Servlet Port");
            }
            catch (Exception ex)
            {
                errorDiv.Style.Value = "display: block";
                //10/6/2014 NS modified for VSPLUS-990
                errorDiv.InnerHtml = "The following error has occurred while getting the data for Sametime Servlet Port: " + ex.Message+
                    "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
            }
        }

        private void WriteSettings()
        {

            bool returnval=false;
            string NotUpdatedSettingsNames=""; 
            try
            {
                if(UserTextBox.Text!=" ")
                {
                    //9/23/2013 NS updated per AF - the service uses the Sametime User column value
                    returnval = VSWebBL.SettingBL.SettingsBL.Ins.UpdateSvalue("Sametime User 1", UserTextBox.Text, VSWeb.Constants.Constants.SysString);
                    if (returnval == false) NotUpdatedSettingsNames += "Sametime User 1,";
                }
            }
            catch (Exception ex)
            {
                //1/30/2014 NS added
                errorDiv.Style.Value = "display: block";
                //10/6/2014 NS modified for VSPLUS-990
                errorDiv.InnerHtml = "The following error has occurred while updating the data for Sametime User 1: " + ex.Message+
                    "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
            }
            
            //9/23/2013 NS added
            try
            {
                if (User2TextBox.Text != " ")
                {
                    returnval = VSWebBL.SettingBL.SettingsBL.Ins.UpdateSvalue("Sametime User 2", User2TextBox.Text,VSWeb.Constants.Constants.SysString);
                    if (returnval == false) NotUpdatedSettingsNames += "Sametime User 2,";
                }
            }
            catch (Exception ex)
            {
                //1/30/2014 NS added
                errorDiv.Style.Value = "display: block";
                //10/6/2014 NS modified for VSPLUS-990
                errorDiv.InnerHtml = "The following error has occurred while updating the data for Sametime User 2: " + ex.Message+
                    "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
            }
            UpdatePassword();

            //1/30/2014 NS added for VSPLUS-322
            try
            {
                if (SametimeServletPortTextBox.Text != " ")
                {
                    returnval = VSWebBL.SettingBL.SettingsBL.Ins.UpdateSvalue("Sametime Servlet Port", SametimeServletPortTextBox.Text, VSWeb.Constants.Constants.SysString);
                    if (returnval == false) NotUpdatedSettingsNames += "Sametime Servlet Port,";
                }
            }
            catch (Exception ex)
            {
                //1/30/2014 NS added
                errorDiv.Style.Value = "display: block";
                //10/6/2014 NS modified for VSPLUS-990
                errorDiv.InnerHtml = "The following error has occurred while updating the data for Sametime Servlet Port: " + ex.Message+
                    "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
            }
            //9/23/2013 NS commented out - AF suggested we need to encrypt the passwords
            /*
            try
            {
                if(ViewState["mypass"]!="" && ViewState["mypass"]!=null)
                mypass = ViewState["mypass"].ToString();
                if (mypass != ""&& mypass!=null)
                {
                    returnval = VSWebBL.SettingBL.SettingsBL.Ins.UpdateSvalue("Sametime Password 1", mypass);
                    if (returnval == false) NotUpdatedSettingsNames += "Sametime Password 1,";

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            //9/23/2013 NS added
            try
            {
                if (ViewState["mypass2"] != "" && ViewState["mypass2"] != null)
                    mypass2 = ViewState["mypass2"].ToString();
                if (mypass2 != "" && mypass2 != null)
                {
                    returnval = VSWebBL.SettingBL.SettingsBL.Ins.UpdateSvalue("Sametime Password 2", mypass2);
                    if (returnval == false) NotUpdatedSettingsNames += "Sametime Password 2,";

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            */

            try
            {

                returnval = VSWebBL.SettingBL.SettingsBL.Ins.UpdateSvalue("AdvancedSametime", AdvancedSametimeCheckBox.Checked.ToString(), VSWeb.Constants.Constants.SysString);
                if (returnval == false) NotUpdatedSettingsNames += "AdvancedSametime";
            }



            catch (Exception ex)
            {
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
            finally
            {
                if (NotUpdatedSettingsNames == "")
                {
                    //12/24/2013 NS modified
                    //SameTimePopupControl.HeaderText = "Information";
                    //SameTimePopupControl.ShowOnPageLoad = true;
                    //passwordOK.Visible = true;
                    //passwordTextBox.Visible = false;
                    //passwordLabel.Text = "Data updated successfully.";
                    errorDiv.Style.Value = "display: none";
                    successDiv.Style.Value = "display: block";
                }
                else
                {
                    //12/24/2013 NS modified
                    //SameTimePopupControl.HeaderText = "Information";
                    //SameTimePopupControl.ShowOnPageLoad = true;
                    //passwordOK.Visible = true;
                    //passwordTextBox.Visible = false;
                    //passwordLabel.Text = NotUpdatedSettingsNames+" were not updated.";
                    successDiv.Style.Value = "display: none";
                    //10/6/2014 NS modified for VSPLUS-990
                    errorDiv.InnerHtml = "The following fields were not updated: " + NotUpdatedSettingsNames+
                        "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                    errorDiv.Style.Value = "display: block";
                }
            }
        }
        

        protected void passwordOK_Click(object sender, EventArgs e)
        {
            if (WhichUserLabel.Text == "1")
            {
                Session["mypass"] = passwordTextBox.Text;
            }
            else
            {
                Session["mypass2"] = passwordTextBox.Text;
            }
            SameTimePopupControl.ShowOnPageLoad = false;
        }

        protected void Stp2Button_Click(object sender, EventArgs e)
        {
            WhichUserLabel.Text = "2";
            SameTimePopupControl.ShowOnPageLoad = true;
            passwordLabel.Text = "Please enter the Sametime password for " + User2TextBox.Text + " below:";
            passwordOK.Visible = true;
            passwordTextBox.Visible = true;
        }

        public void UpdatePassword()
        {
            bool updated = false;
            byte[] MyPass;
            VSFramework.TripleDES mySecrets = new VSFramework.TripleDES();
            try
            {
                if (Session["mypass"] != "" & Session["mypass"] != null)
                {
                    MyPass = mySecrets.Encrypt(Session["mypass"].ToString());
                    System.Text.StringBuilder newstr = new System.Text.StringBuilder();
                    foreach (byte b in MyPass)
                    {
                        newstr.AppendFormat("{0}, ", b);
                    }
                    string bytepwd = newstr.ToString();
                    int n = bytepwd.LastIndexOf(", ");
                    bytepwd = bytepwd.Substring(0, n);
                    updated = VSWebBL.SettingBL.SettingsBL.Ins.UpdateSvalue("Sametime Password 1", bytepwd, VSWeb.Constants.Constants.SysByte);
                }
            }
            catch (Exception ex)
            {
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
            try
            {
                if (Session["mypass2"] != "" & Session["mypass2"] != null)
                {
                    MyPass = mySecrets.Encrypt(Session["mypass2"].ToString());
                    System.Text.StringBuilder newstr = new System.Text.StringBuilder();
                    foreach (byte b in MyPass)
                    {
                        newstr.AppendFormat("{0}, ", b);
                    }
                    string bytepwd = newstr.ToString();
                    int n = bytepwd.LastIndexOf(", ");
                    bytepwd = bytepwd.Substring(0, n);
                    updated = VSWebBL.SettingBL.SettingsBL.Ins.UpdateSvalue("Sametime Password 2", bytepwd,"System.Byte[]");
                }
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