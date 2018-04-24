using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DevExpress.Web;
using DevExpress.Web.Data;
using System.Collections;
using System.Net.NetworkInformation;
using System.Net;
using System.Text;
using System.Reflection;
using Microsoft.Win32;

namespace VSWebUI
{
    public class RegistryHandler
    {
        public void WriteToRegistry(string KeyName, object KeyValue)
        {
            RegistryKey aKey;
            aKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("Software\\VitalSigns", true);
            if (aKey == null)
            {
                aKey = Microsoft.Win32.Registry.LocalMachine.CreateSubKey("Software\\VitalSigns");
            }
            aKey.SetValue(KeyName, KeyValue);
            aKey.Flush();
        }
    }

    public partial class WebForm8 : System.Web.UI.Page
    {
        ASPxCheckBox chkbox;
        //6/5/2015 NS added for VSPLUS-1838
        public string SMSFromNumber = "12055555555";
        string ids;
        //11/10/2015 NS modified for VSPLUS-2335
        DataTable Emergencydt = new DataTable();
        protected void Page_Load(object sender, EventArgs e)
        {
            bool popupopen;
            Control ctrl;
            successTest1Div.Style.Value = "display: none";
            successTest2Div.Style.Value = "display: none";
            errorTest1Div.Style.Value = "display: none";
            errorTest2Div.Style.Value = "display: none";
            //11/10/2015 NS added for VSPLUS-2335
            DataColumn dc = new DataColumn();
            dc.ColumnName = "ID";
            Emergencydt.Columns.Add(dc);
            dc = new DataColumn();
            dc.ColumnName = "Email";
            Emergencydt.Columns.Add(dc);
            Emergencydt.AcceptChanges();

            //Page.Title = "Alert Settings";
            if (!IsPostBack)
            {
                //OKButton.Enabled = false; 
                Session["AlertDataEvents"] = null;
                ReadSettings();
                //1/8/2014 NS added
                bool issuperadmin = false;
                SMTPRoundPanel1.Enabled = false;
                SMTPRoundPanel2.Enabled = false;
                SNMPRoundPanel.Enabled = false;
                OKButton.Enabled = false;
                //2/5/2014 NS added - Clear Alerts and Delete Alerts should only be available to Super admin
                ClearAlertsButton.Enabled = false;
                DeleteAlertsButton.Enabled = false;
                DataTable dsconfig = VSWebBL.SecurityBL.UsersBL.Ins.GetIsAdmin(Session["UserID"].ToString());
                if (dsconfig.Rows.Count > 0)
                {
                    if (dsconfig.Rows[0]["SuperAdmin"].ToString() == "Y")
                    {
                        issuperadmin = true;
                    }
                }
                if (issuperadmin)
                {
                    //10/14/2014 NS modified
                    //AlertsOnButton.Enabled = true;
                    //AlertsOffButton.Enabled = true;
                    ASPxMenu1.Items[0].Items[0].Visible = true;
                    ASPxMenu1.Items[0].Items[1].Visible = true;
                    SMTPRoundPanel1.Enabled = true;
                    SMTPRoundPanel2.Enabled = true;
                    SNMPRoundPanel.Enabled = true;
                    OKButton.Enabled = true;
                    //2/5/2014 NS added - Clear Alerts and Delete Alerts should only be available to Super admin
                    ClearAlertsButton.Enabled = true;
                    DeleteAlertsButton.Enabled = true;
                }
                string alertsPriorTo = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("DeleteAlertsPriorTo");
                if (alertsPriorTo != null && alertsPriorTo != "")
                {
                    DeleteAlertsTextBox.Text = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("DeleteAlertsPriorTo");
                }
                //10/17/2014 NS added
                fillEventsGridView();
                if (Session["UserPreferences"] != null)
                {
                    DataTable UserPreferences = (DataTable)Session["UserPreferences"];
                    foreach (DataRow dr in UserPreferences.Rows)
                    {
                        if (dr[1].ToString() == "Alert_Settings|EventsGridView")
                        {
                            EventsGridView.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                        }
                    }
                }
                //7/17/2015 NS added for VSPLUS-1562
                //fillEmergencyGridView();
            }
            //1/17/2013 NS added for VSPLUS-299
            else
            {
                //6/5/2015 NS added for VSPLUS-1838
                if (hidden_phone.Value != "")
                {
                    SMSFromNumber = hidden_phone.Value;
                }
                popupopen = ResetPwdPopupControl.ShowOnPageLoad;

                ctrl = GetPostBackControl(Page);
                //6/11/2013 NS added - if the control that triggered postback is not the Cancel button, update password
                if (popupopen && ctrl != null)
                {
                    if (ctrl.ID != "ResetPwdCancelBtn")
                    {
                        //successDivPwd.Style.Value = "display: block";
                        UpdatePassword();
                    }
                }
                //10/17/2014 NS added
                //if (!IsCallback)
                {
                    fillEventsGridViewfromsession();
                }
                //7/17/2015 NS added for VSPLUS-1562
                //fillEmergencyGridViewFromSession();
            }
            fillEmergencyGridView();
            //7/10/2014 NS added for VSPLUS-812
            AlertLimitsCheckChanged();
            //10/14/2014 NS added
            ReadOnOffSettings();
        }
        protected void EventsGridView_PageSizeChanged(object sender, EventArgs e)
        {
            //ProfilesGridView.PageIndex;
            VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("Alert_Settings|EventsGridView", EventsGridView.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        }

        public void fillEventsGridView()
        {
            try
            {
                if (Session["AlertDataEvents"] == null)
                {
                    DataTable DataEvents = VSWebBL.ConfiguratorBL.AlertsBL.Ins.GetAllEvents("");
                    //DataColumn dc = new DataColumn("ConsecutiveFailures", System.Type.GetType("System.Boolean"));
                    //dc.DefaultValue = false;
                    //DataEvents.Columns.Add(dc);
                    Session["AlertDataEvents"] = DataEvents;
                }
                EventsGridView.DataSource = (DataTable)Session["AlertDataEvents"];
                EventsGridView.DataBind();
                ((GridViewDataColumn)EventsGridView.Columns["ServerType"]).GroupBy();
            }
            catch (Exception ex)
            {
                //Log.Entry.Ins.Write(Server.MapPath("~/LogFiles/"), "VSPlusLog.txt", DateTime.Now.ToString() + " Error in Page: " +
                //    Request.Url.AbsolutePath + ", Method: " + System.Reflection.MethodBase.GetCurrentMethod().Name +
                //    ", Error: " + ex.ToString());

                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
            }
        }

        public void fillEventsGridViewfromsession()
        {
            if (Session["AlertDataEvents"] != "" && Session["AlertDataEvents"] != null)
            {
                EventsGridView.DataSource = (DataTable)Session["AlertDataEvents"];
                EventsGridView.DataBind();
            }
        }

        public void ReadSettings()
        {
            try
            {

                // primary SMTP Server

                HostNameTextBox.Text = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("PrimaryHostName");

                FromTextBox.Text = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("primaryFrom");
                UserIDTextBox.Text = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("primaryUserID");
                //1/17/2014 NS commented out for VSPLUS-299
                //pwdTextBox.Text = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("primarypwd");
                PortTextBox.Text = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("primaryport");
                string primaryAuth = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("primaryAuth");
                if (primaryAuth != "" && primaryAuth != null)
                    AuthCheckBox.Checked = bool.Parse(primaryAuth);
                string primarySSL = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("primarySSL");
                if (primarySSL != null && primarySSL != "")
                    SSLCheckBox.Checked = bool.Parse(primarySSL);

                // Second SMTP

                HostNameTextBox1.Text = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("SecondaryHostName");

                FromTextBox1.Text = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("SecondaryFrom");
                UserIDTextBox1.Text = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("SecondaryUserID");
                //1/17/2014 NS commented out for VSPLUS-299
                //pwdTextBox1.Text = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("Secondarypwd");
                PortTextBox1.Text = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("Secondaryport");
                string SecondaryAuth = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("SecondaryAuth");
                if (SecondaryAuth != "" && SecondaryAuth != null)
                    AuthCheckBox1.Checked = bool.Parse(SecondaryAuth);
                string SecondarySSL = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("SecondarySSL");
                if (SecondarySSL != "" && SecondarySSL != null)
                    SSLCheckBox1.Checked = bool.Parse(SecondarySSL);

                // SNMP Settings

                SNMPTextBox.Text = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("SNMPHostName");
                string SNMP = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("EnableSNMP");
                if (SNMP != "" && SNMP != null)
                    SNMPCheckBox.Checked = bool.Parse(SNMP);
                if (SNMPCheckBox.Checked == true)
                {
                    //SNMPTextBox.Enabled = true;
                    SNMPTextBox.ClientEnabled = true;
                }
                else
                {
                    //SNMPTextBox.Enabled = false;
                    SNMPTextBox.ClientEnabled = false;
                }

                //Windows Event Log Settings
                //4/8/2014 NS added for VSPLUS-403
                string WinLog = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("AlertsWinLog");
                if (WinLog != "" && WinLog != null)
                {
                    WinLogCheckBox.Checked = bool.Parse(WinLog);
                }

                //Advanced Alert Settings
                //4/7/2014 NS added for VSPLUS-519
                string interval = "";
                string duration = "";

                //Alerts Per day and Per Definition
                AlertsPerDefTextBox.Text = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("TotalMaximumAlertsPerDefinition");
                AlertsPerDayTextBox.Text = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("TotalMaximumAlertPerDay");

                interval = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("PersistentAlertInterval");
                duration = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("PersistentAlertDuration");

                //day = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("TotalMaximumAlertsPerDay");
                //definition = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("TotalMaximumAlertsPerDefinition");
                if (interval != "0")
                {
                    for (int i = 0; i < IntervalComboBox.Items.Count; i++)
                    {
                        if (IntervalComboBox.Items[i].Value.ToString() == interval)
                        {
                            IntervalComboBox.SelectedIndex = i;
                        }
                    }
                    for (int i = 0; i < DurationComboBox.Items.Count; i++)
                    {
                        if (DurationComboBox.Items[i].Value.ToString() == duration)
                        {
                            DurationComboBox.SelectedIndex = i;
                        }
                    }

                    if (IntervalComboBox.SelectedIndex >= 0)
                    {
                        PersistentCheckBox.Checked = true;
                        IntervalLabel.ClientVisible = true;
                        IntervalComboBox.ClientVisible = true;
                        MinLabel.ClientVisible = true;
                        DurationLabel.ClientVisible = true;
                        DurationComboBox.ClientVisible = true;
                        HoursLabel.ClientVisible = true;
                    }
                }
                else
                {
                    PersistentCheckBox.Checked = false;
                    IntervalLabel.ClientVisible = false;
                    IntervalComboBox.ClientVisible = false;
                    MinLabel.ClientVisible = false;
                    DurationLabel.ClientVisible = false;
                    DurationComboBox.ClientVisible = false;
                    HoursLabel.ClientVisible = false;
                }

                if (AlertsPerDayTextBox.Text != "0" || AlertsPerDefTextBox.Text != "0")
                {
                    AlertLimitsCheckBox.Checked = true;
                    MaxDefLabel.ClientVisible = true;
                    AlertsPerDefTextBox.ClientVisible = true;
                    MaxDayLabel.ClientVisible = true;
                    AlertsPerDayTextBox.ClientVisible = true;
                }
                else
                {
                    AlertLimitsCheckBox.Checked = false;
                    MaxDayLabel.ClientVisible = false;
                    AlertsPerDayTextBox.ClientVisible = false;
                    MaxDefLabel.ClientVisible = false;
                    AlertsPerDefTextBox.ClientVisible = false;
                }
                //10/20/2014 NS added for VSPLUS-730
                string AlertsRepeatOn = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("AlertsRepeatOn");
                if (AlertsRepeatOn != "" && AlertsRepeatOn != null)
                {
                    RepeatOccurCheckBox.Checked = bool.Parse(AlertsRepeatOn);
                    if (RepeatOccurCheckBox.Checked)
                    {
                        RepeatOccurLabel.ClientVisible = true;
                        RepeatOccurTextBox.ClientVisible = true;
                        EventsGridView.ClientVisible = true;
                    }
                    else
                    {
                        RepeatOccurLabel.ClientVisible = false;
                        RepeatOccurTextBox.ClientVisible = false;
                        EventsGridView.ClientVisible = false;
                    }
                }
                string AlertsRepeatNum = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("AlertsRepeatOccurrences");
                if (AlertsRepeatNum != "" && AlertsRepeatNum != null)
                {
                    RepeatOccurTextBox.Text = AlertsRepeatNum;
                }
                //12/1/2014 NS added for VSPLUS-946
                string SMSSid = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("SMSAccountSid");
                if (SMSSid != "" && SMSSid != null)
                {
                    SMSSidTextBox.Text = SMSSid;
                }
                string SMSAuthToken = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("SMSAuthToken");
                if (SMSAuthToken != "" && SMSAuthToken != null)
                {
                    SMSAuthTokenTextBox.Text = SMSAuthToken;
                }
                //12/12/2014 NS added for VSPLUS-946
                string SMSFrom = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("SMSFrom");
                if (SMSFrom != "" && SMSFrom != null)
                {
                    //6/5/2015 NS modified for VSPLUS-1838
                    //SMSFromTextBox.Text = SMSFrom;
                    SMSFromNumber = SMSFrom;
                    //7/9/2015 NS added for VSPLUS-1957
                    hidden_phone.Value = SMSFrom;
                }
            }
            catch (Exception ex)
            {
                //1/8/2014 NS added
                errorDiv2.Style.Value = "display: block";
                //10/3/2014 NS modified for VSPLUS-990
                errorDiv2.InnerHtml = "The following error has occurred: " + ex.Message +
                    "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
            }


        }

        public void ReadOnOffSettings()
        {
            try
            {
                //1/6/2014 NS added
                bool alertson = false;
                alertson = bool.Parse(VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("AlertsOn"));
                if (alertson)
                {
                    //12/17/2015 NS modified
                    //alertsOn.Style.Value = "display: block";
                    //alertsOff.Style.Value = "display: none";
                    AlertsImg.ImageUrl = "~/images/icons/greenbell.png";
                    AlertsImg.ToolTip = "Alerts are ON";
                    AlertsImg.Visible = true;
                    //10/13/2014 NS modified
                    //AlertsOffButton.Visible = true;
                    //AlertsOnButton.Visible = false;
                    ASPxMenu1.Items[0].Items[1].Visible = true;
                    ASPxMenu1.Items[0].Items[0].Visible = false;
                }
                else
                {
                    //12/17/2015 NS modified
                    //alertsOn.Style.Value = "display: none";
                    //alertsOff.Style.Value = "display: block";
                    AlertsImg.ImageUrl = "~/images/icons/redbell.png";
                    AlertsImg.ToolTip = "Alerts are OFF";
                    AlertsImg.Visible = true;
                    //10/13/2014 NS modified
                    //AlertsOffButton.Visible = false;
                    //AlertsOnButton.Visible = true;
                    //10/13/2014 NS added
                    ASPxMenu1.Items[0].Items[1].Visible = false;
                    ASPxMenu1.Items[0].Items[0].Visible = true;
                }
            }
            catch (Exception ex)
            {
                //1/8/2014 NS added
                errorDiv2.Style.Value = "display: block";
                //10/3/2014 NS modified for VSPLUS-990
                errorDiv2.InnerHtml = "The following error has occurred: " + ex.Message +
                    "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
            }
        }

        public void WriteSettings()
        {
            bool returnvalue;
            string Notupdate = "";
            successDiv2.Style.Value = "display: none";

            try
            {
                returnvalue = VSWebBL.SettingBL.SettingsBL.Ins.UpdateSvalue("PrimaryHostName", HostNameTextBox.Text, VSWeb.Constants.Constants.SysString);
                if (returnvalue == false)
                {
                    Notupdate += "Primary Host Name";
                }

            }


            catch (Exception ex)
            {
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw;
            }

            try
            {
                returnvalue = VSWebBL.SettingBL.SettingsBL.Ins.UpdateSvalue("primaryFrom", FromTextBox.Text, VSWeb.Constants.Constants.SysString);
                if (returnvalue == false)
                {
                    if (Notupdate == "")
                    {
                        Notupdate += "Primary From";
                    }
                    else
                    {
                        Notupdate += ", Primary From";
                    }
                }
            }
            catch (Exception ex)
            {
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw;
            }
            try
            {
                returnvalue = VSWebBL.SettingBL.SettingsBL.Ins.UpdateSvalue("primaryUserID", UserIDTextBox.Text, VSWeb.Constants.Constants.SysString);
                if (returnvalue == false)
                {
                    if (Notupdate == "")
                    {
                        Notupdate += "Primary User ID";
                    }
                    else
                    {
                        Notupdate += ", Primary User ID";
                    }
                }
            }
            catch (Exception ex)
            {
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw;
            }
            //1/17/2014 NS commented out for VSPLUS-299
            /*
            try
            {
                returnvalue = VSWebBL.SettingBL.SettingsBL.Ins.UpdateSvalue("primarypwd", pwdTextBox.Text);
                if (returnvalue == false)
                {
                    Notupdate += ",primarypwd";
                }
            }
            catch (Exception)
            {

                throw;
            }
             */
            try
            {
                returnvalue = VSWebBL.SettingBL.SettingsBL.Ins.UpdateSvalue("primaryport", PortTextBox.Text, VSWeb.Constants.Constants.SysString);
                if (returnvalue == false)
                {
                    if (Notupdate == "")
                    {
                        Notupdate += "Primary Port";
                    }
                    else
                    {
                        Notupdate += ", Primary Port";
                    }
                }
            }
            catch (Exception ex)
            {
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw;
            }
            try
            {
                returnvalue = VSWebBL.SettingBL.SettingsBL.Ins.UpdateSvalue("primaryAuth", AuthCheckBox.Checked.ToString(), VSWeb.Constants.Constants.SysString);
                if (returnvalue == false)
                {
                    if (Notupdate == "")
                    {
                        Notupdate += "Primary Requires Authentication";
                    }
                    else
                    {
                        Notupdate += ", Primary Requires Authentication";
                    }
                }
            }
            catch (Exception ex)
            {
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw;
            }
            try
            {
                returnvalue = VSWebBL.SettingBL.SettingsBL.Ins.UpdateSvalue("primarySSL", SSLCheckBox.Checked.ToString(), VSWeb.Constants.Constants.SysString);
                if (returnvalue == false)
                {
                    if (Notupdate == "")
                    {
                        Notupdate += "Primary Requires SSL";
                    }
                    else
                    {
                        Notupdate += ", Primary Requires SSL";
                    }
                }
            }
            catch (Exception ex)
            {
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw;
            }


            // writing Secondary values


            try
            {
                returnvalue = VSWebBL.SettingBL.SettingsBL.Ins.UpdateSvalue("SecondaryHostName", HostNameTextBox1.Text, VSWeb.Constants.Constants.SysString);
                if (returnvalue == false)
                {
                    if (Notupdate == "")
                    {
                        Notupdate += "Secondary Host Name";
                    }
                    else
                    {
                        Notupdate += ", Secondary Host Name";
                    }
                }

            }


            catch (Exception ex)
            {
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw;
            }

            try
            {
                returnvalue = VSWebBL.SettingBL.SettingsBL.Ins.UpdateSvalue("SecondaryFrom", FromTextBox1.Text, VSWeb.Constants.Constants.SysString);
                if (returnvalue == false)
                {
                    if (Notupdate == "")
                    {
                        Notupdate += "Secondary From";
                    }
                    else
                    {
                        Notupdate += ", Secondary From";
                    }
                }
            }
            catch (Exception ex)
            {
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw;
            }
            try
            {
                returnvalue = VSWebBL.SettingBL.SettingsBL.Ins.UpdateSvalue("SecondaryUserID", UserIDTextBox1.Text, VSWeb.Constants.Constants.SysString);
                if (returnvalue == false)
                {
                    if (Notupdate == "")
                    {
                        Notupdate += "Secondary User ID";
                    }
                    else
                    {
                        Notupdate += ", Secondary User ID";
                    }
                }
            }
            catch (Exception ex)
            {
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw;
            }
            //1/17/2014 NS commented out for VSPLUS-299
            /*
            try
            {
                returnvalue = VSWebBL.SettingBL.SettingsBL.Ins.UpdateSvalue("Secondarypwd", pwdTextBox1.Text);
                if (returnvalue == false)
                {
                    Notupdate += ",Secondarypwd";
                }
            }
            catch (Exception)
            {

                throw;
            }
             */
            try
            {
                returnvalue = VSWebBL.SettingBL.SettingsBL.Ins.UpdateSvalue("Secondaryport", PortTextBox1.Text, VSWeb.Constants.Constants.SysString);
                if (returnvalue == false)
                {
                    if (Notupdate == "")
                    {
                        Notupdate += "Secondary Port";
                    }
                    else
                    {
                        Notupdate += ", Secondary Port";
                    }
                }
            }
            catch (Exception ex)
            {
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw;
            }
            try
            {
                returnvalue = VSWebBL.SettingBL.SettingsBL.Ins.UpdateSvalue("SecondaryAuth", AuthCheckBox1.Checked.ToString(), VSWeb.Constants.Constants.SysString);
                if (returnvalue == false)
                {
                    if (Notupdate == "")
                    {
                        Notupdate += "Secondary Requires Authentication";
                    }
                    else
                    {
                        Notupdate += ", Secondary Requires Authentication";
                    }
                }
            }
            catch (Exception ex)
            {
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw;
            }
            try
            {
                returnvalue = VSWebBL.SettingBL.SettingsBL.Ins.UpdateSvalue("SecondarySSL", SSLCheckBox1.Checked.ToString(), VSWeb.Constants.Constants.SysString);
                if (returnvalue == false)
                {
                    if (Notupdate == "")
                    {
                        Notupdate += "Secondary Requires SSL";
                    }
                    else
                    {
                        Notupdate += ", Secondary Requires SSL";
                    }
                }
            }
            catch (Exception ex)
            {
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw;
            }

            // Write SNMP 

            try
            {
                returnvalue = VSWebBL.SettingBL.SettingsBL.Ins.UpdateSvalue("SNMPHostName", SNMPTextBox.Text, VSWeb.Constants.Constants.SysString);
                if (returnvalue == false)
                {
                    if (Notupdate == "")
                    {
                        Notupdate += "SNMP Host Name";
                    }
                    else
                    {
                        Notupdate += ", SNMP Host Name";
                    }
                }
            }
            catch (Exception ex)
            {
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw;
            }

            try
            {
                returnvalue = VSWebBL.SettingBL.SettingsBL.Ins.UpdateSvalue("EnableSNMP", SNMPCheckBox.Checked.ToString(), VSWeb.Constants.Constants.SysString);
                if (returnvalue == false)
                {
                    if (Notupdate == "")
                    {
                        Notupdate += "Enable SNMP Traps";
                    }
                    else
                    {
                        Notupdate += ", Enable SNMP Traps";
                    }
                }
            }
            catch (Exception ex)
            {
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw;
            }

            //4/8/2014 NS added for VSPLUS-403
            try
            {
                returnvalue = VSWebBL.SettingBL.SettingsBL.Ins.UpdateSvalue("AlertsWinLog", WinLogCheckBox.Checked.ToString().ToLower(), VSWeb.Constants.Constants.SysString);
                if (returnvalue == false)
                {
                    if (Notupdate == "")
                    {
                        Notupdate += "Write alerts to Windows log";
                    }
                    else
                    {
                        Notupdate += ", Write alerts to Windows log";
                    }
                }
            }
            catch (Exception ex)
            {
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw;
            }

            //4/7/2014 NS added for VSPLUS-519
            try
            {
                if (PersistentCheckBox.Checked)
                {
                    returnvalue = VSWebBL.SettingBL.SettingsBL.Ins.UpdateSvalue("PersistentAlertInterval", IntervalComboBox.Text, VSWeb.Constants.Constants.SysString);
                }
                else
                {
                    returnvalue = VSWebBL.SettingBL.SettingsBL.Ins.UpdateSvalue("PersistentAlertInterval", "0", VSWeb.Constants.Constants.SysString);
                }
                if (returnvalue == false)
                {
                    if (Notupdate == "")
                    {
                        Notupdate += "Persistent Alert Interval";
                    }
                    else
                    {
                        Notupdate += ", Persistent Alert Interval";
                    }
                }
            }


            catch (Exception ex)
            {
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw;
            }

            try
            {
                returnvalue = VSWebBL.SettingBL.SettingsBL.Ins.UpdateSvalue("PersistentAlertDuration", DurationComboBox.Value.ToString(), VSWeb.Constants.Constants.SysString);
                if (returnvalue == false)
                {
                    if (Notupdate == "")
                    {
                        Notupdate += "Persistent Alert Duration";
                    }
                    else
                    {
                        Notupdate += ", Persistent Alert Duration";
                    }
                }
            }


            catch (Exception ex)
            {
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw;
            }

            try
            {
                if (AlertLimitsCheckBox.Checked)
                {
                    returnvalue = VSWebBL.SettingBL.SettingsBL.Ins.UpdateSvalue("TotalMaximumAlertsPerDefinition", AlertsPerDefTextBox.Value.ToString(), VSWeb.Constants.Constants.SysString);
                }
                else
                {
                    returnvalue = VSWebBL.SettingBL.SettingsBL.Ins.UpdateSvalue("TotalMaximumAlertsPerDefinition", "0", VSWeb.Constants.Constants.SysString);
                }
                if (returnvalue == false)
                {
                    if (Notupdate == "")
                    {
                        Notupdate += "Total Maximum Alerts per Definition";
                    }
                    else
                    {
                        Notupdate += ", Total Maximum Alerts per Definition";
                    }
                }
            }


            catch (Exception ex)
            {
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw;
            }

            try
            {
                returnvalue = VSWebBL.SettingBL.SettingsBL.Ins.UpdateSvalue("TotalMaximumAlertPerDay", AlertsPerDayTextBox.Value.ToString(), VSWeb.Constants.Constants.SysString);
                if (returnvalue == false)
                {
                    if (Notupdate == "")
                    {
                        Notupdate += "Total Maximum Alerts per Day";
                    }
                    else
                    {
                        Notupdate += ", Total Maximum Alerts per Day";
                    }
                }
            }


            catch (Exception ex)
            {
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw;
            }

            try
            {
                returnvalue = VSWebBL.SettingBL.SettingsBL.Ins.UpdateSvalue("DeleteAlertsPriorTo", DeleteAlertsTextBox.Text, VSWeb.Constants.Constants.SysString);

                if (returnvalue == false)
                {
                    if (Notupdate == "")
                    {
                        Notupdate += "Delete Alerts Prior To";
                    }
                    else
                    {
                        Notupdate += ", Delete Alerts Prior To";
                    }
                }
            }
            catch (Exception ex)
            {
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw;
            }

            //10/20/2014 NS added for VSPLUS-730
            try
            {
                if (RepeatOccurCheckBox.Checked)
                {
                    returnvalue = VSWebBL.SettingBL.SettingsBL.Ins.UpdateSvalue("AlertsRepeatOccurrences", RepeatOccurTextBox.Text, VSWeb.Constants.Constants.SysString);
                    if (returnvalue == false)
                    {
                        if (Notupdate == "")
                        {
                            Notupdate += "Alert about recurrences - number of recurrences not entered";
                        }
                        else
                        {
                            Notupdate += ", Alert about recurrences - number of recurrences not entered";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw;
            }
            try
            {
                //11/21/2014 NS added for VSPLUS-1178
                // sowjanya modified for VSPLUS-3165
                string ids = GetSelectedEventIDs();
                if (RepeatOccurCheckBox.Checked)
                {

                    if (ids != "")
                    {
                        returnvalue = VSWebBL.ConfiguratorBL.AlertsBL.Ins.UpdateEventsMaster(ids);
                    }
                    if (returnvalue == false || ids == "")
                    {
                        if (Notupdate == "")
                        {
                            Notupdate += "Alert about recurrences - no events selected";
                        }
                        else
                        {
                            Notupdate += ", Alert about recurrences - no events selected";
                        }
                    }
                }


                if (!RepeatOccurCheckBox.Checked)
                {
                   // ids = GetSelectedEventIDs();

                    if (ids != "")
                    {
                        //Session["AlertDataEvents"] = null;
                        returnvalue = VSWebBL.ConfiguratorBL.AlertsBL.Ins.UpdateEventsMasterforUncheckedCondition(ids);
                        DataTable Events = VSWebBL.ConfiguratorBL.AlertsBL.Ins.GetAllEvents("");
                        Session["AlertDataEvents"] = Events;

                    }


                }




          
            try
            {
                if (ids != "")
                {
                    returnvalue = VSWebBL.SettingBL.SettingsBL.Ins.UpdateSvalue("AlertsRepeatOn", RepeatOccurCheckBox.Checked.ToString(), VSWeb.Constants.Constants.SysString);
                }
                if (returnvalue == false)
                {
                    if (Notupdate == "")
                    {
                        Notupdate += "Alert about recurrences";
                    }
                    else
                    {
                        Notupdate += ", Alert about recurrences";
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
           
            catch (Exception ex)
            {
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw;
            }
            //12/1/2014 NS added for VSPLUS-946
            try
            {
                returnvalue = VSWebBL.SettingBL.SettingsBL.Ins.UpdateSvalue("SMSAccountSid", SMSSidTextBox.Text, VSWeb.Constants.Constants.SysString);
                if (returnvalue == false)
                {
                    if (Notupdate == "")
                    {
                        Notupdate += "SMS Account Sid";
                    }
                    else
                    {
                        Notupdate += ", SMS Account Sid";
                    }
                }
            }
            catch (Exception ex)
            {
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw;
            }
            try
            {
                returnvalue = VSWebBL.SettingBL.SettingsBL.Ins.UpdateSvalue("SMSAuthToken", SMSAuthTokenTextBox.Text, VSWeb.Constants.Constants.SysString);
                if (returnvalue == false)
                {
                    if (Notupdate == "")
                    {
                        Notupdate += "SMS Auth Token";
                    }
                    else
                    {
                        Notupdate += ", SMS Auth Token";
                    }
                }
            }
            catch (Exception ex)
            {
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw;
            }
            //12/12/2014 NS added for VSPLUS-946
            try
            {
                //6/5/2015 NS modified for VSPLUS-1838
                //returnvalue = VSWebBL.SettingBL.SettingsBL.Ins.UpdateSvalue("SMSFrom", SMSFromTextBox.Text, VSWeb.Constants.Constants.SysString);
                //9/28/2015 NS modified for VSPLUs-2186
                if (hidden_phone.Value != "invalid")
                {
                    returnvalue = VSWebBL.SettingBL.SettingsBL.Ins.UpdateSvalue("SMSFrom", hidden_phone.Value, VSWeb.Constants.Constants.SysString);
                    if (returnvalue == false)
                    {
                        if (Notupdate == "")
                        {
                            Notupdate += "Phone Number From (SMS/Text tab)";
                        }
                        else
                        {
                            Notupdate += ", Phone Number From (SMS/Text tab)";
                        }
                    }
                }
                else
                {
                    if (Notupdate == "")
                    {
                        Notupdate += "Phone Number From (SMS/Text tab)";
                    }
                    else
                    {
                        Notupdate += ", Phone Number From (SMS/Text tab)";
                    }
                }
            }
            catch (Exception ex)
            {
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw;
            }

            if (Notupdate == "" || Notupdate == null)
            {
                //12/24/2013 NS modified
                //MsgPopupControl.ShowOnPageLoad = true;
                //MsgLabel.Text = "Alert setting saved.";
                errorDiv.Style.Value = "display: none";
                successDiv.Style.Value = "display: block";
            }
            else
            {
                //12/24/2013 NS modified
                //MsgPopupControl.ShowOnPageLoad = true;
                //MsgLabel.Text = Notupdate + " were not updated.";
                successDiv.Style.Value = "display: none";
                //10/3/2014 NS modified for VSPLUS-990
                errorDiv.InnerHtml = "The following fields were not updated: " + Notupdate +
                    "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                errorDiv.Style.Value = "display: block";
            }

        }

        public DataTable RestrServers()
        {
            DataTable DCTasksDataTable = VSWebBL.ConfiguratorBL.AlertsBL.Ins.GetServer();
            if (Session["RestrictedServers"] != "" && Session["RestrictedServers"] != null)
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



        protected void OKButton_Click(object sender, EventArgs e)
        {

            WriteSettings();
        }

        protected void CancelButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Configurator/Default.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
            Context.ApplicationInstance.CompleteRequest();
        }

        protected void MsgButton_Click(object sender, EventArgs e)
        {
            MsgPopupControl.ShowOnPageLoad = false;
        }

        protected void SNMPCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (SNMPCheckBox.Checked == true)
            {
                SNMPTextBox.ClientEnabled = true;
            }
            else
            {
                SNMPTextBox.ClientEnabled = false;
            }
        }

        protected void AlertsOnButton_Click(object sender, EventArgs e)
        {
            try
            {
                VSWebBL.SettingBL.SettingsBL.Ins.UpdateSvalue("AlertsOn", "True", VSWeb.Constants.Constants.SysString);
                //12/17/2015 NS modified
                //alertsOn.Style.Value = "display: block";
                //alertsOff.Style.Value = "display: none";
                AlertsImg.ImageUrl = "~/images/icons/greenbell.png";
                AlertsImg.ToolTip = "Alerts are ON";
                AlertsImg.Visible = true;
                //1/29/2014 MD added
                //10/13/2014 NS modified
                //AlertsOffButton.Visible = true;
                //AlertsOnButton.Visible = false;
                ASPxMenu1.Items[0].Items[1].Visible = true;
                ASPxMenu1.Items[0].Items[0].Visible = false;
            }
            catch (Exception ex)
            {
                //10/3/2014 NS modified for VSPLUS-990
                errorDiv.InnerHtml = "Unable to update the Alerts On value in the Settings table: " + ex.Message +
                    "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                errorDiv.Style.Value = "display: block";
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
            }
        }

        protected void AlertsOffButton_Click(object sender, EventArgs e)
        {
            try
            {
                VSWebBL.SettingBL.SettingsBL.Ins.UpdateSvalue("AlertsOn", "False", VSWeb.Constants.Constants.SysString);
                //12/17/2015 NS modified
                //alertsOn.Style.Value = "display: none";
                //alertsOff.Style.Value = "display: block";
                AlertsImg.ImageUrl = "~/images/icons/redbell.png";
                AlertsImg.ToolTip = "Alerts are OFF";
                AlertsImg.Visible = true;

                //1/29/2014 MD added
                //10/13/2014 NS modified
                //AlertsOffButton.Visible = false;
                //AlertsOnButton.Visible = true;
                ASPxMenu1.Items[0].Items[1].Visible = false;
                ASPxMenu1.Items[0].Items[0].Visible = true;
            }
            catch (Exception ex)
            {
                //10/3/2014 NS modified for VSPLUS-990
                errorDiv.InnerHtml = "Unable to update the Alerts On value in the Settings table: " + ex.Message +
                    "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                errorDiv.Style.Value = "display: block";
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
            }
        }

        protected void AlertsOffClick(object sender, EventArgs e)
        {
            try
            {
                VSWebBL.SettingBL.SettingsBL.Ins.UpdateSvalue("AlertsOn", "False", VSWeb.Constants.Constants.SysString);
                //alertsOn.Style.Value = "display: none";
                //alertsOff.Style.Value = "display: block";
            }
            catch (Exception ex)
            {
                //10/3/2014 NS modified for VSPLUS-990
                errorDiv.InnerHtml = "Unable to update the Alerts On value in the Settings table: " + ex.Message +
                    "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                errorDiv.Style.Value = "display: block";
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
            }
        }

        protected void AlertsOnClick(object sender, EventArgs e)
        {
            try
            {
                VSWebBL.SettingBL.SettingsBL.Ins.UpdateSvalue("AlertsOn", "True", VSWeb.Constants.Constants.SysString);
                //alertsOn.Style.Value = "display: block";
                //alertsOff.Style.Value = "display: none";
            }
            catch (Exception ex)
            {
                //10/3/2014 NS modified for VSPLUS-990
                errorDiv.InnerHtml = "Unable to update the Alerts On value in the Settings table: " + ex.Message +
                    "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                errorDiv.Style.Value = "display: block";
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
            }
        }

        protected void ViewAlertHistoryButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Dashboard/OverallServerAlerts.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
            Context.ApplicationInstance.CompleteRequest();
        }

        protected void ClearAlertsButton_Click(object sender, EventArgs e)
        {
            string errorstr = "";
            errorstr = VSWebBL.ConfiguratorBL.AlertsBL.Ins.ClearAlertHistory();
            if (errorstr != "")
            {
                //10/3/2014 NS modified for VSPLUS-990
                errorDiv.InnerHtml = "The following error has occurred while trying to clear the alerts history: " + errorstr +
                    "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                errorDiv.Style.Value = "display: block";
            }
            else
            {
                //10/3/2014 NS modified for VSPLUS-990
                successDiv.InnerHtml = "Alerts have been cleared successfully." +
                    "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                successDiv.Style.Value = "display: block";
            }
        }

        protected void DeleteAlertsButton_Click(object sender, EventArgs e)
        {
            string errorstr = "";
            if (DeleteAlertsTextBox.Text != "" && Convert.ToInt32(DeleteAlertsTextBox.Text) > 0)
            {
                bool update = VSWebBL.SettingBL.SettingsBL.Ins.UpdateSvalue("DeleteAlertsPriorTo", DeleteAlertsTextBox.Text, VSWeb.Constants.Constants.SysString);
                if (update)
                {
                    errorstr = VSWebBL.ConfiguratorBL.AlertsBL.Ins.DeleteAlertHistory(DateTime.Now.AddDays(-Convert.ToInt32(DeleteAlertsTextBox.Text)));
                    if (errorstr != "")
                    {
                        //10/3/2014 NS modified for VSPLUS-990
                        errorDiv.InnerHtml = "The following error has occurred while trying to delete the alerts history: " + errorstr +
                            "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                        errorDiv.Style.Value = "display: block";
                    }
                    else
                    {
                        //10/3/2014 NS modified for VSPLUS-990
                        successDiv.InnerHtml = "Alerts have been deleted successfully." +
                            "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                        successDiv.Style.Value = "display: block";
                    }
                }
            }
            else
            {
                //10/3/2014 NS modified for VSPLUS-990
                errorDiv.InnerHtml = "Please enter a number of days to include in deletion." +
                    "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                errorDiv.Style.Value = "display: block";
            }

        }

        protected void RegisterButton1_Click(object sender, EventArgs e)
        {
            //1/2/2016 Sowjanya added for VSPLUS-2559
            successDiv.Style.Value = "display: none";
            successDiv2.Style.Value = "display: none";
            TMsgerror1.Style.Value = "display: none";
            TMsgerror2.Style.Value = "display: none";
            TMsgsuccess1.Style.Value = "display: none";
            //1/17/2014 NS added for VSPLUS-299
            SetWhichPwd.Text = "SMTPPwd1";
            if (!ResetPwdPopupControl.ShowOnPageLoad)
            {
                ResetPwdPopupControl.ShowOnPageLoad = true;
            }
            else
            {
                ResetPwdPopupControl.ShowOnPageLoad = false;
            }
        }

        protected void RegisterButton2_Click(object sender, EventArgs e)
        {
            //1/2/2016 Sowjanya added for VSPLUS-2559
            successDiv.Style.Value = "display: none";
            successDiv2.Style.Value = "display: none";
            TMsgsuccess2.Style.Value = "display: none";
            TMsgerror1.Style.Value = "display: none";
            TMsgerror2.Style.Value = "display: none";
            //1/17/2014 NS added for VSPLUS-299
            SetWhichPwd.Text = "SMTPPwd2";
            if (!ResetPwdPopupControl.ShowOnPageLoad)
            {
                ResetPwdPopupControl.ShowOnPageLoad = true;
            }
            else
            {
                ResetPwdPopupControl.ShowOnPageLoad = false;
            }
        }

        protected void ResetPwdOKBtn_Click(object sender, EventArgs e)
        {
            ResetPwdPopupControl.ShowOnPageLoad = false;
        }

        //6/11/2013 NS added a new function that returns a control that triggered a postback (1/17/2014 NS added for VSPLUS-299)
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
            VSFramework.TripleDES mySecrets = new VSFramework.TripleDES();
            try
            {
                MyPass = mySecrets.Encrypt(ResetPwdTextBox.Text);
                System.Text.StringBuilder newstr = new System.Text.StringBuilder();
                foreach (byte b in MyPass)
                {
                    newstr.AppendFormat("{0}, ", b);
                }
                string bytepwd = newstr.ToString();
                int n = bytepwd.LastIndexOf(", ");
                bytepwd = bytepwd.Substring(0, n);
                if (SetWhichPwd.Text == "SMTPPwd1")
                {
                    updated = VSWebBL.SettingBL.SettingsBL.Ins.UpdateSvalue("primarypwd", ResetPwdTextBox.Text, VSWeb.Constants.Constants.SysString);
                    if (updated == true)
                    {
                        //successDiv.Style.Value = "display: none";
                        //10/3/2014 NS modified for VSPLUS-990
                        successDiv2.InnerHtml = "Primary SMTP server password updated succesfully. " +
                            "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                        successDiv2.Style.Value = "display: block";
                    }
                    else
                    {
                        errorDiv2.InnerHtml = "Secondary SMTP server password is not  updated.  " +
                            "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                        errorDiv2.Style.Value = "display: block";
                    }

                }

                else
                {
                    updated = VSWebBL.SettingBL.SettingsBL.Ins.UpdateSvalue("Secondarypwd", ResetPwdTextBox.Text, VSWeb.Constants.Constants.SysString);
                    if (updated == true)
                    {
                        //successDiv.Style.Value = "display: none";
                        //10/3/2014 NS modified for VSPLUS-990
                        successDiv2.InnerHtml = "Secondary SMTP server password updated succesfully." +
                            "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                        successDiv2.Style.Value = "display: block";
                    }
                    else
                    {
                        errorDiv2.InnerHtml = "Secondary SMTP server password is not  updated.  " +
                            "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                        errorDiv2.Style.Value = "display: block";
                    }
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

        protected void ResetPwdCancelBtn_Click(object sender, EventArgs e)
        {
            ResetPwdPopupControl.ShowOnPageLoad = false;
        }

        protected void PersistentCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (PersistentCheckBox.Checked)
            {
                IntervalLabel.ClientVisible = true;
                IntervalComboBox.ClientVisible = true;
                MinLabel.ClientVisible = true;
                DurationComboBox.ClientVisible = true;
                DurationLabel.ClientVisible = true;
                //7/10/2014 NS added
                HoursLabel.ClientVisible = true;
            }
            else
            {
                IntervalLabel.ClientVisible = false;
                IntervalComboBox.ClientVisible = false;
                MinLabel.ClientVisible = false;
                DurationComboBox.ClientVisible = false;
                DurationLabel.ClientVisible = false;
                //7/10/2014 NS added
                HoursLabel.ClientVisible = false;
            }
        }

        //7/10/2014 NS added for VSPLUS-812
        protected void AlertLimitsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            AlertLimitsCheckChanged();
        }

        public void AlertLimitsCheckChanged()
        {
            if (AlertLimitsCheckBox.Checked)
            {
                MaxDefLabel.ClientVisible = true;
                MaxDayLabel.ClientVisible = true;
                AlertsPerDefTextBox.ClientVisible = true;
                AlertsPerDayTextBox.ClientVisible = true;
            }
            else
            {
                MaxDefLabel.ClientVisible = false;
                MaxDayLabel.ClientVisible = false;
                AlertsPerDefTextBox.ClientVisible = false;
                AlertsPerDayTextBox.ClientVisible = false;
                AlertsPerDefTextBox.Text = "0";
                AlertsPerDayTextBox.Text = "0";

            }
        }

        protected void ASPxMenu1_ItemClick(object source, DevExpress.Web.MenuItemEventArgs e)
        {
            if (e.Item.Name == "TurnOnItem")
            {
                try
                {
                    VSWebBL.SettingBL.SettingsBL.Ins.UpdateSvalue("AlertsOn", "True", VSWeb.Constants.Constants.SysString);
                    //12/17/2015 NS modified
                    //alertsOn.Style.Value = "display: block";
                    //alertsOff.Style.Value = "display: none";
                    AlertsImg.ImageUrl = "~/images/icons/greenbell.png";
                    AlertsImg.ToolTip = "Alerts are ON";
                    AlertsImg.Visible = true;

                    //1/29/2014 MD added
                    //10/13/2014 NS added
                    //AlertsOffButton.Visible = true;
                    //AlertsOnButton.Visible = false;
                    ASPxMenu1.Items[0].Items[1].Visible = true;
                    ASPxMenu1.Items[0].Items[0].Visible = false;
                }
                catch (Exception ex)
                {
                    //10/3/2014 NS modified for VSPLUS-990
                    errorDiv.InnerHtml = "Unable to update the Alerts On value in the Settings table: " + ex.Message +
                        "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                    errorDiv.Style.Value = "display: block";
                    //6/27/2014 NS added for VSPLUS-634
                    Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                }
            }
            else if (e.Item.Name == "TurnOffItem")
            {
                try
                {
                    VSWebBL.SettingBL.SettingsBL.Ins.UpdateSvalue("AlertsOn", "False", VSWeb.Constants.Constants.SysString);
                    //12/17/2015 NS modified
                    //alertsOn.Style.Value = "display: none";
                    //alertsOff.Style.Value = "display: block";
                    AlertsImg.ImageUrl = "~/images/icons/redbell.png";
                    AlertsImg.ToolTip = "Alerts are OFF";
                    AlertsImg.Visible = true;

                    //1/29/2014 MD added
                    //10/13/2014 NS modified
                    //AlertsOffButton.Visible = false;
                    //AlertsOnButton.Visible = true;
                    ASPxMenu1.Items[0].Items[1].Visible = false;
                    ASPxMenu1.Items[0].Items[0].Visible = true;
                }
                catch (Exception ex)
                {
                    //10/3/2014 NS modified for VSPLUS-990
                    errorDiv.InnerHtml = "Unable to update the Alerts On value in the Settings table: " + ex.Message +
                        "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                    errorDiv.Style.Value = "display: block";
                    //6/27/2014 NS added for VSPLUS-634
                    Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                }
            }
            else if (e.Item.Name == "ViewHistoryItem")
            {
                Response.Redirect("~/Dashboard/OverallServerAlerts.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
                Context.ApplicationInstance.CompleteRequest();
            }
            else if (e.Item.Name == "ClearAlertsItem")
            {
                string errorstr = "";
                errorstr = VSWebBL.ConfiguratorBL.AlertsBL.Ins.ClearAlertHistory();
                if (errorstr != "")
                {
                    //10/3/2014 NS modified for VSPLUS-990
                    errorDiv.InnerHtml = "The following error has occurred while trying to clear the alerts history: " + errorstr +
                        "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                    errorDiv.Style.Value = "display: block";
                }
                else
                {
                    //10/3/2014 NS modified for VSPLUS-990
                    successDiv.InnerHtml = "Alerts have been cleared successfully." +
                        "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                    successDiv.Style.Value = "display: block";
                }
            }
            //8/7/2015 NS modified for VSPLUS-2059
            else if (e.Item.Name == "DeleteAlertsItem")
            {
                string errorstr = "";
                DateTime dat = new DateTime();
                errorstr = VSWebBL.ConfiguratorBL.AlertsBL.Ins.DeleteAlertHistory(dat);
                if (errorstr != "")
                {
                    //10/3/2014 NS modified for VSPLUS-990
                    errorDiv.InnerHtml = "The following error has occurred while trying to delete the alerts history: " + errorstr +
                        "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                    errorDiv.Style.Value = "display: block";
                }
                else
                {
                    //10/3/2014 NS modified for VSPLUS-990
                    successDiv.InnerHtml = "Alerts have been deleted successfully." +
                        "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                    successDiv.Style.Value = "display: block";
                }
            }
            else if (e.Item.Name == "DeleteAlerts30Item")
            {
                string errorstr = "";
                if (DeleteAlertsTextBox.Text != "" && Convert.ToInt32(DeleteAlertsTextBox.Text) > 0)
                {
                    bool update = VSWebBL.SettingBL.SettingsBL.Ins.UpdateSvalue("DeleteAlertsPriorTo", DeleteAlertsTextBox.Text, VSWeb.Constants.Constants.SysString);
                    if (update)
                    {
                        errorstr = VSWebBL.ConfiguratorBL.AlertsBL.Ins.DeleteAlertHistory(DateTime.Now.AddDays(-Convert.ToInt32(DeleteAlertsTextBox.Text)));
                        if (errorstr != "")
                        {
                            //10/3/2014 NS modified for VSPLUS-990
                            errorDiv.InnerHtml = "The following error has occurred while trying to delete the alerts history: " + errorstr +
                                "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                            errorDiv.Style.Value = "display: block";
                        }
                        else
                        {
                            //10/3/2014 NS modified for VSPLUS-990
                            successDiv.InnerHtml = "Alerts have been deleted successfully." +
                                "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                            successDiv.Style.Value = "display: block";
                        }
                    }
                }
                else
                {
                    //10/3/2014 NS modified for VSPLUS-990
                    errorDiv.InnerHtml = "Please enter a number of days to include in deletion." +
                        "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                    errorDiv.Style.Value = "display: block";
                }
            }
        }

        //10/20/2014 NS added for VSPLUS-730
        public string GetSelectedEventIDs()
        {
            string ids = "";
            DataTable dt = Session["AlertDataEvents"] as DataTable;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (Convert.ToBoolean(dt.Rows[i]["ConsecutiveFailures"]) == true && dt.Rows[i]["ID"].ToString() != "" && dt.Rows[i]["ID"] != null)
                {
                    if (ids == "")
                    {
                        ids += dt.Rows[i]["ID"].ToString();
                    }
                    else
                    {
                        ids += "," + dt.Rows[i]["ID"].ToString();
                    }
                }
            }
            return ids;
        }

        protected void chkBoxCF_Init(object sender, EventArgs e)
        {
            ASPxCheckBox checkBox = (ASPxCheckBox)sender;
            GridViewDataItemTemplateContainer container = (GridViewDataItemTemplateContainer)checkBox.NamingContainer;

            string key = string.Format("{0}|{1}", container.Column.FieldName, container.KeyValue);
            checkBox.ClientSideEvents.CheckedChanged = string.Format("function(s, e) {{ grid.PerformCallback('{0}|' + s.GetChecked()); }}", key);
        }

        protected void EventsGridView_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            // sowjanya modified for VSPLUS-3165
            string[] parts = e.Parameters.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
             //DataTable Events = VSWebBL.ConfiguratorBL.AlertsBL.Ins.GetAllEvents("");
             // Session["AlertDataEvents"] = Events;
            DataTable DataEvents = (DataTable)Session["AlertDataEvents"];
          
            DataEvents.PrimaryKey = new DataColumn[] { DataEvents.Columns["ID"] };
            DataRow row = DataEvents.Rows.Find(Convert.ToInt32(parts[1]));
            row[parts[0]] = Convert.ToBoolean(parts[2]);
            Session["AlertDataEvents1"] = DataEvents;
            EventsGridView.DataSource = DataEvents;
            EventsGridView.DataBind();
        }

        protected void RepeatOccurCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            // sowjanya modified for VSPLUS-3165

            successDiv.Style.Value = "display:none";
            errorDiv.Style.Value = "display: none";
            if (RepeatOccurCheckBox.Checked)
            {
                RepeatOccurLabel.ClientVisible = true;
                RepeatOccurTextBox.ClientVisible = true;
                DataTable DataEvents = VSWebBL.ConfiguratorBL.AlertsBL.Ins.GetAllEvents("");
                EventsGridView.DataSource = DataEvents;
                EventsGridView.DataBind();
                EventsGridView.ClientVisible = true;
            }
            else
            {
                RepeatOccurLabel.ClientVisible = false;
                RepeatOccurTextBox.ClientVisible = false;

                EventsGridView.ClientVisible = false;
                if (Session["AlertDataEvents1"] != null)
                {
                    DataTable dt = Session["AlertDataEvents1"] as DataTable;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (Convert.ToBoolean(dt.Rows[i]["ConsecutiveFailures"]) == true)
                        {
                            dt.Rows[i]["ConsecutiveFailures"] = false;
                        }
                    }

                    DataTable DataEvents = VSWebBL.ConfiguratorBL.AlertsBL.Ins.GetAllEvents("");
                    //DataTable dt1 = Session["AlertDataEvents"] as DataTable;
                    for (int i = 0; i < DataEvents.Rows.Count; i++)
                    {
                        if (Convert.ToBoolean(DataEvents.Rows[i]["ConsecutiveFailures"]) == true)
                        {
                            dt.Rows[i]["ConsecutiveFailures"] = true;
                        }
                    }
                    Session["AlertDataEvents"] = dt;
                    EventsGridView.DataSource = dt;
                    EventsGridView.DataBind();
                }

                else
                {
                    //Session["AlertDataEvents"] = dt;
                    //EventsGridView.DataSource = dt;
                    EventsGridView.DataSource = Session["AlertDataEvents"];
                    EventsGridView.DataBind();
                }
            }
        }

        //10/27/2014 NS added for VSPLUS-1105
        protected void CollapseAllButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (CollapseAllButton.Text == "Collapse All")
                {
                    EventsGridView.CollapseAll();
                    CollapseAllButton.Image.Url = "~/images/icons/add.png";
                    CollapseAllButton.Text = "Expand All";
                }
                else
                {
                    EventsGridView.ExpandAll();
                    CollapseAllButton.Image.Url = "~/images/icons/forbidden.png";
                    CollapseAllButton.Text = "Collapse All";
                }
            }
            catch (Exception ex)
            {
                //Log.Entry.Ins.Write(Server.MapPath("~/LogFiles/"), "VSPlusLog.txt", DateTime.Now.ToString() + " Error in Page: " +
                //    Request.Url.AbsolutePath + ", Method: " + System.Reflection.MethodBase.GetCurrentMethod().Name +
                //    ", Error: " + ex.ToString());

                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
            }
        }

        protected void TestConnectionButton1_Click(object sender, EventArgs e)
        {
            successDiv.Style.Value = "display:none";
            successDiv2.Style.Value = "display:none";
            TMsgsuccess1.Style.Value = "display:none";
            TMsgerror2.Style.Value = "display:none";
            TMsgerror1.Style.Value = "display:none";
            string pwd = "";
            pwd = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("primarypwd");
            SendEHLOWithChilkat(HostNameTextBox.Text, PortTextBox.Text, SSLCheckBox.Checked, AuthCheckBox.Checked, UserIDTextBox.Text, pwd, FromTextBox1.Text, true);
            /*
            Ping pingSender = new Ping();

            string data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
            byte[] buffer = Encoding.ASCII.GetBytes(data);

            int timeout = 10000;

            PingOptions options = new PingOptions(64, true);
            try
            {
                // PingReply reply = pingSender.Send(IPTextBox.Text, timeout, buffer, options);
                PingReply reply = pingSender.Send(HostNameTextBox.Text, timeout, buffer, options);

                if (reply.Status == IPStatus.Success)
                {

                    if (reply.RoundtripTime.ToString() != null)
                    {
                        successTest1Div.InnerHtml = "Ping to " + reply.Address + " responded in " + reply.RoundtripTime + " ms." +
                           "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                        successTest1Div.Style.Value = "display: block";
                        errorTest1Div.Style.Value = "display: none";
                    }
                }
                else
                {
                    errorTest1Div.InnerHtml = HostNameTextBox.Text + " did not respond." +
                           "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                    errorTest1Div.Style.Value = "display: block";
                    successTest1Div.Style.Value = "display: none";
                }
            }
            catch (Exception ex)
            {
                errorDiv.InnerHtml = "The following error has occurred: " + ex.InnerException +
                    "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                errorDiv.Style.Value = "display: block";
                successTest1Div.Style.Value = "display: none";
                errorTest1Div.Style.Value = "display: none";
                successTest2Div.Style.Value = "display: none";
                errorTest2Div.Style.Value = "display: none";
            }
             */
        }

        protected void TestConnectionButton2_Click(object sender, EventArgs e)
        {
            successDiv.Style.Value = "display:none";
            successDiv2.Style.Value = "display:none";
            TMsgerror2.Style.Value = "display:none";
            TMsgsuccess1.Style.Value = "display:none";
            string pwd = "";
            pwd = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("Secondarypwd");
            SendEHLOWithChilkat(HostNameTextBox1.Text, PortTextBox1.Text, SSLCheckBox1.Checked, AuthCheckBox1.Checked, UserIDTextBox1.Text, pwd, FromTextBox.Text, false);
            /*
            Ping pingSender = new Ping();

            string data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
            byte[] buffer = Encoding.ASCII.GetBytes(data);

            int timeout = 10000;

            PingOptions options = new PingOptions(64, true);

            // PingReply reply = pingSender.Send(IPTextBox.Text, timeout, buffer, options);
            try
            {
                PingReply reply = pingSender.Send(HostNameTextBox1.Text, timeout, buffer, options);

                if (reply.Status == IPStatus.Success)
                {

                    if (reply.RoundtripTime.ToString() != null)
                    {
                        successTest2Div.InnerHtml = "Ping to " + reply.Address + " responded in " + reply.RoundtripTime + " ms." +
                           "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                        successTest2Div.Style.Value = "display: block";
                        errorTest2Div.Style.Value = "display: none";
                    }
                }
                else
                {
                    errorTest2Div.InnerHtml = HostNameTextBox1.Text + " did not respond." +
                           "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                    errorTest2Div.Style.Value = "display: block";
                    successTest2Div.Style.Value = "display: none";
                }
            }
            catch (Exception ex)
            {
                errorDiv.InnerHtml = "The following error has occurred: " + ex.InnerException +
                    "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                errorDiv.Style.Value = "display: block";
                successTest1Div.Style.Value = "display: none";
                errorTest1Div.Style.Value = "display: none";
                successTest2Div.Style.Value = "display: none";
                errorTest2Div.Style.Value = "display: none";
            }
             */
        }

        private void SendEHLOWithChilkat(string hostname, string portname, bool isSSL, bool isAuth, string username, string pwd,
            string from, bool isTest1)
        {
            //Chilkat.Socket Socket = new Chilkat.Socket();
            Chilkat.MailMan mailman = new Chilkat.MailMan();
            Chilkat.Email email = new Chilkat.Email();
            bool success = false;
            successTest1Div.Style.Value = "display: none";
            successTest2Div.Style.Value = "display: none";
            errorTest1Div.Style.Value = "display: none";
            errorTest2Div.Style.Value = "display: none";
            try
            {
                mailman.UnlockComponent("MZLDADMAILQ_8nzv7Kxb4Rpx");
            }
            catch
            {
                errorDiv.InnerHtml = "The following error has occurred: failed to unlock Chilkat component." +
                    "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                errorDiv.Style.Value = "display: block";
            }
            try
            {
                mailman.SmtpHost = hostname;
                mailman.SmtpPort = Convert.ToInt32(portname);
                if (isSSL)
                {
                    mailman.SmtpSsl = true;
                }
                if (isAuth)
                {
                    mailman.SmtpUsername = username;
                    mailman.SmtpPassword = pwd;
                }

                email.Body = "Testing alert settings";
                email.Subject = "Testing alert settings";
                email.AddTo("alerttest@rprwyatt.com", "alerttest@rprwyatt.com");
                email.FromAddress = from;
                email.ReplyTo = username;
                email.FromName = from;
                success = mailman.SendEmail(email);
                if (success)
                {
                    if (isTest1)
                    {
                        successTest1Div.InnerHtml = "SMTP service successfully connected." +
                            "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                        successTest1Div.Style.Value = "display: block";
                        errorTest1Div.Style.Value = "display: none";
                    }
                    else
                    {
                        //9/28/2015 NS modified for VSPLUS-2188
                        successTest2Div.InnerHtml = "SMTP service successfully connected." +
                            "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                        successTest2Div.Style.Value = "display: block";
                        errorTest2Div.Style.Value = "display: none";
                    }
                }
                else
                {
                    if (isTest1)
                    {
                        errorTest1Div.InnerHtml = "Unable to connect to the SMTP server " + hostname + " with the specified settings/credentials." +
                               "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                        errorTest1Div.Style.Value = "display: block";
                        successTest1Div.Style.Value = "display: none";
                    }
                    else
                    {
                        errorTest2Div.InnerHtml = "Unable to connect to the SMTP server " + hostname + " with the specified settings/credentials." +
                               "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                        errorTest2Div.Style.Value = "display: block";
                        successTest2Div.Style.Value = "display: none";
                    }
                }
                email.Dispose();
            }
            catch (Exception ex)
            {
                errorDiv.InnerHtml = "The following error has occurred while trying to connect to the server: " + ex.Message +
                    "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                errorDiv.Style.Value = "display: block";
            }
            try
            {
                mailman.Dispose();
                GC.Collect();
            }
            catch
            {
            }
            /*
            try
            {
                success = Socket.UnlockComponent("MZLDADSocket_OACwPK2ZlEn9");
                if (success != true)
                {
                    errorDiv.InnerHtml = "The following error has occurred: failed to unlock Chilkat component." +
                    "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                    errorDiv.Style.Value = "display: block";
                }
            }
            catch (Exception ex)
            {
                errorDiv.InnerHtml = "The following error has occurred: " + ex.Message +
                    "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                errorDiv.Style.Value = "display: block";
            }

            try
            {
                int port = 25;
                bool ssl = false;
                int maxWaitMillisec = 20000;
                if (portname != "")
                {
                    port = Convert.ToInt32(portname);
                }
                //Socket.SocksUsername = username;
                //Socket.SocksPassword = pwd;
                success = Socket.Connect(hostname, port, ssl, maxWaitMillisec);
            }
            catch (Exception ex)
            {
                errorDiv.InnerHtml = "The following error has occurred: " + ex.Message +
                    "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                errorDiv.Style.Value = "display: block";
            }

            try
            {
                if (success != true)
                {
                    if (isTest1)
                    {
                        errorTest1Div.InnerHtml = "Unable to connect to the SMTP server " + hostname +
                               "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                        errorTest1Div.Style.Value = "display: block";
                        successTest1Div.Style.Value = "display: none";
                    }
                    else
                    {
                        errorTest2Div.InnerHtml = "Unable to connect to the SMTP server " + hostname +
                               "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                        errorTest2Div.Style.Value = "display: block";
                        successTest2Div.Style.Value = "display: none";
                    }
                }
                else
                {
                    Socket.MaxReadIdleMs = 10000;
                    Socket.MaxSendIdleMs = 10000;
                    try
                    {
                        string strResponse = "";
                        TimeSpan elapsed;
                        Socket.SendString("EHLO");
                        strResponse = Socket.ReceiveString();
                        done = DateTime.Now.Ticks;
                        elapsed = new TimeSpan(done - start);
                        if (strResponse.Trim() == "" || (strResponse.ToUpper()).IndexOf("NOT AVAILABLE") != -1)
                        {
                            if (isTest1)
                            {
                                errorTest1Div.InnerHtml = "Service connected in " + elapsed.TotalMilliseconds.ToString() + " ms. but NO RESPONSE came back from the server." +
                                    "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                                errorTest1Div.Style.Value = "display: block";
                                successTest1Div.Style.Value = "display: none";
                            }
                            else
                            {
                                errorTest2Div.InnerHtml = "Service connected in " + elapsed.TotalMilliseconds.ToString() + " ms. but NO RESPONSE came back from the server." +
                                    "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                                errorTest2Div.Style.Value = "display: block";
                                successTest2Div.Style.Value = "display: none";
                            }
                        }
                        else
                        {
                            if (isTest1)
                            {
                                successTest1Div.InnerHtml = "SMTP service connected in " + elapsed.TotalMilliseconds.ToString() + " ms." +
                                    "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>" +
                                    "<br />Received response: " + strResponse;
                                successTest1Div.Style.Value = "display: block";
                                errorTest1Div.Style.Value = "display: none";
                            }
                            else
                            {
                                successTest2Div.InnerHtml = "SMTP service connected in " + elapsed.TotalMilliseconds.ToString() + " ms." +
                                    "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>" +
                                    "<br />Received response: " + strResponse;
                                successTest2Div.Style.Value = "display: block";
                                errorTest2Div.Style.Value = "display: none";
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        errorDiv.InnerHtml = "The following error has occurred: " + ex.Message +
                            "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                        errorDiv.Style.Value = "display: block";
                    }
                    Socket.Close(20000);
                }
            }
            catch (Exception ex)
            {
                errorDiv.InnerHtml = "The following error has occurred: " + ex.Message +
                    "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                errorDiv.Style.Value = "display: block";
            }
            finally
            {
                Socket.Dispose();
            }
             */
        }

        //6/23/2015 NS added
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (!this.DesignMode)
            {
                this.UpdatePanel1.Unload += new EventHandler(UpdatePanel_Unload);
            }
        }
        protected void UpdatePanel_Unload(object sender, EventArgs e)
        {
            RegisterUpdatePanel((UpdatePanel)sender);
        }
        protected void RegisterUpdatePanel(UpdatePanel panel)
        {
            var sType = typeof(ScriptManager);
            var mInfo = sType.GetMethod("System.Web.UI.IScriptManagerInternal.RegisterUpdatePanel", BindingFlags.NonPublic | BindingFlags.Instance);
            if (mInfo != null)
                mInfo.Invoke(ScriptManager.GetCurrent(Page), new object[] { panel });
        }
        //7/17/2015 NS added for VSPLUS-1562
        public void fillEmergencyGridView()
        {
            try
            {
                Emergencydt = VSWebBL.ConfiguratorBL.AlertsBL.Ins.GetAlertEmergencyContacts();
                if (Emergencydt.Rows.Count > 0)
                {
                    DataTable dtcopy = Emergencydt.Copy();
                    dtcopy.PrimaryKey = new DataColumn[] { dtcopy.Columns["ID"] };
                    Session["EmergencyAlerts"] = dtcopy;
                    EmergencyUsersGridView.DataSource = dtcopy;
                    EmergencyUsersGridView.DataBind();
                }
                else
                {
                    EmergencyUsersGridView.DataSource = Emergencydt;
                    EmergencyUsersGridView.DataBind();
                    Session["EmergencyAlerts"] = Emergencydt;
                }
            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
            }
        }
        //7/17/2015 NS added for VSPLUS-1562
        public void fillEmergencyGridViewFromSession()
        {
            if (Session["EmergencyAlerts"] != "" && Session["EmergencyAlerts"] != null)
            {
                EmergencyUsersGridView.DataSource = (DataTable)Session["EmergencyAlerts"];
                EmergencyUsersGridView.DataBind();
            }
        }
        //7/17/2015 NS added for VSPLUS-1562
        protected DataRow GetRow(DataTable ServerObject, IDictionaryEnumerator enumerator, int Keys)
        {
            DataTable dataTable = ServerObject;
            DataRow DRRow = dataTable.NewRow();
            if (Keys == 0)
                DRRow = dataTable.NewRow();
            else
                DRRow = dataTable.Rows.Find(Keys);
            //IDictionaryEnumerator enumerator = e.NewValues.GetEnumerator();
            enumerator.Reset();
            while (enumerator.MoveNext())
                DRRow[enumerator.Key.ToString()] = (enumerator.Value == null ? "False" : enumerator.Value);
            return DRRow;
        }
        //7/17/2015 NS added for VSPLUS-1562
        private void UpdateEmergencyData(string Mode, DataRow GridRow)
        {
            if (Mode == "Insert")
            {
                Object ReturnValue = VSWebBL.ConfiguratorBL.AlertsBL.Ins.InsertEmergencyAlertData(GridRow["ID"].ToString(), GridRow["Email"].ToString());
            }
            if (Mode == "Update")
            {
                Object ReturnValue = VSWebBL.ConfiguratorBL.AlertsBL.Ins.UpdateEmergencyAlertData(GridRow["ID"].ToString(), GridRow["Email"].ToString());
            }
            //12/17/2015 NS added for VSPLUS-1562
            /*
            dt = VSWebBL.ConfiguratorBL.AlertsBL.Ins.GetAlertEmergencyContacts();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    emergencyContacts += dt.Rows[i]["Email"] + ",";
                }
            }
            handler.WriteToRegistry("Alert Emergency Contacts", emergencyContacts);
            handler.WriteToRegistry("Alert Emergency PrimaryHostName", HostNameTextBox.Text);
            handler.WriteToRegistry("Alert Emergency primaryport", PortTextBox.Text);
            handler.WriteToRegistry("Alert Emergency primaryUserID", UserIDTextBox.Text);
            handler.WriteToRegistry("Alert Emergency primarypwd", VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("primarypwd"));
            handler.WriteToRegistry("Alert Emergency primaryFrom", FromTextBox.Text);
            handler.WriteToRegistry("Alert Emergency primaryAuth", VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("primaryAuth"));
            handler.WriteToRegistry("Alert Emergency primarySSL", VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("primarySSL"));
             */
        }
        //7/17/2015 NS added for VSPLUS-1562
        protected void EmergencyUsersGridView_RowInserting(object sender, ASPxDataInsertingEventArgs e)
        {
            if (Session["EmergencyAlerts"] != null && Session["EmergencyAlerts"] != "")
                Emergencydt = (DataTable)Session["EmergencyAlerts"];
            ASPxGridView gridView = (ASPxGridView)sender;
            DataRow newrow = GetRow(Emergencydt, e.NewValues.GetEnumerator(), 0);

            DataRow[] matchrow = Emergencydt.Select("Email = '" + newrow.ItemArray[1] + "' ");
            if (matchrow.Length > 0)
            {
                throw new ArgumentException("E-mail address already exists.");
            }
            UpdateEmergencyData("Insert", GetRow(Emergencydt, e.NewValues.GetEnumerator(), 0));
            gridView.CancelEdit();
            e.Cancel = true;
            fillEmergencyGridView();
        }
        //7/17/2015 NS added for VSPLUS-1562
        protected void EmergencyUsersGridView_RowUpdating(object sender, ASPxDataUpdatingEventArgs e)
        {
            Emergencydt = (DataTable)Session["EmergencyAlerts"];
            ASPxGridView gridView = (ASPxGridView)sender;
            DataRow newrow = GetRow(Emergencydt, e.NewValues.GetEnumerator(), 0);

            UpdateEmergencyData("Update", GetRow(Emergencydt, e.NewValues.GetEnumerator(), Convert.ToInt32(e.Keys[0])));
            gridView.CancelEdit();
            e.Cancel = true;
            fillEmergencyGridView();

        }
        //7/17/2015 NS added for VSPLUS-1562
        protected void EmergencyUsersGridView_RowDeleting(object sender, ASPxDataDeletingEventArgs e)
        {
            Object ReturnValue = VSWebBL.ConfiguratorBL.AlertsBL.Ins.DeleteData(e.Keys[0].ToString());
            ASPxGridView gridView = (ASPxGridView)sender;
            gridView.CancelEdit();
            e.Cancel = true;
            fillEmergencyGridView();
        }

        protected void Test1Btn_Click(object sender, EventArgs e)
        {
            successDiv2.Style .Value = "display: none";
            TMsgsuccess1.Style.Value = "display: none";
            TMsgerror1.Style.Value = "display: none";
            TMsgerror2.Style.Value = "display: none";


            if (!TestMsgPopupControl1.ShowOnPageLoad)
            {
                TestMsgPopupControl1.ShowOnPageLoad = true;
            }
            else
            {
                TestMsgPopupControl1.ShowOnPageLoad = false;
            }
            Session["Click"] = "Primary";
        }

        protected void Test2Btn_Click(object sender, EventArgs e)
        {
            TMsgsuccess1.Style.Value = "display: none";
            TMsgsuccess2.Style.Value = "display: none";
            TMsgerror2.Style.Value = "display: none";
            successDiv2.Style.Value = "display: none";

            if (!TestMsgPopupControl1.ShowOnPageLoad)
            {
                TestMsgPopupControl1.ShowOnPageLoad = true;
            }
            else
            {
                TestMsgPopupControl1.ShowOnPageLoad = false;
            }
            Session["Click"] = "Secondary";
        }

        protected void SendTest1_Click(object sender, EventArgs e)
        {
            string pwd;
            if (Session["Click"].ToString() == "Primary")
            {
                TMsgsuccess1.Style.Value = "display:none";
                pwd = "";
                pwd = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("primarypwd");

                SendTestMail1(HostNameTextBox.Text, PortTextBox.Text, SSLCheckBox.Checked, AuthCheckBox.Checked, UserIDTextBox.Text, pwd, FromTextBox1.Text, true);
            }
            else if (Session["Click"].ToString() == "Secondary")
            {

                TMsgsuccess2.Style.Value = "display:none";
                pwd = "";
                pwd = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("Secondarypwd");
                SendTestMail1(HostNameTextBox1.Text, PortTextBox1.Text, SSLCheckBox1.Checked, AuthCheckBox1.Checked, UserIDTextBox1.Text, pwd, FromTextBox.Text, false);
            }
            TestMsgPopupControl1.ShowOnPageLoad = false;

        }

        protected void CancelTest1_Click(object sender, EventArgs e)
        {
            TestMsgPopupControl1.ShowOnPageLoad = false;
        }

        private void SendTestMail1(string hostname, string portname, bool isSSL, bool isAuth, string username, string pwd, string from, bool isTest1)
        {
            Chilkat.MailMan mailman = new Chilkat.MailMan();
            Chilkat.Email email = new Chilkat.Email();
            bool success = false;
            TMsgsuccess1.Style.Value = "display: none";
            TMsgsuccess2.Style.Value = "display: none";
            TMsgerror1.Style.Value = "display: none";
            TMsgerror2.Style.Value = "display: none";
            try
            {
                mailman.UnlockComponent("MZLDADMAILQ_8nzv7Kxb4Rpx");
            }
            catch
            {
                errorDiv.InnerHtml = "The following error has occurred: failed to unlock Chilkat component." +
                    "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                errorDiv.Style.Value = "display: block";
            }
            try
            {
                mailman.SmtpHost = hostname;
                mailman.SmtpPort = Convert.ToInt32(portname);
                if (isSSL)
                {
                    mailman.SmtpSsl = true;
                }
                if (isAuth)
                {
                    mailman.SmtpUsername = username;
                    mailman.SmtpPassword = pwd;
                }

                email.Body = "VitalSigns Test Message";
                email.Subject = "VitalSigns Test Message";
                email.AddTo("Test", TestMail.Text);
                email.FromAddress = from;
                email.ReplyTo = username;
                email.FromName = from;
                success = mailman.SendEmail(email);
                if (success)
                {
                    if (isTest1)
                    {
                        TMsgsuccess1.InnerHtml = "Email sent successfully." +
                            "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                        TMsgsuccess1.Style.Value = "display: block";
                        TMsgerror1.Style.Value = "display: none";
                    }
                    else
                    {
                        TMsgsuccess2.InnerHtml = "Email sent successfully." +
                            "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                        TMsgsuccess2.Style.Value = "display: block";
                        TMsgerror2.Style.Value = "display: none";
                    }
                }
                else
                {
                    if (isTest1)
                    {
                        TMsgerror1.InnerHtml = "Email could not able to send. Unable to connect to the SMTP server smtp.gmail.com with the specified settings/credentials." +
                               "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                        TMsgerror1.Style.Value = "display: block";
                        TMsgsuccess1.Style.Value = "display: none";
                    }
                    else
                    {
                        TMsgerror2.InnerHtml = "Email could not able to send. Unable to connect to the SMTP server smtp.gmail.com with the specified settings/credentials." +
                               "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                        TMsgerror2.Style.Value = "display: block";
                        TMsgsuccess2.Style.Value = "display: none";
                    }
                }
                email.Dispose();
            }
            catch (Exception ex)
            {
                errorDiv.InnerHtml = "The following error has occurred while trying to connect to the server: " + ex.Message +
                    "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                errorDiv.Style.Value = "display: block";
            }
            try
            {
                mailman.Dispose();
                GC.Collect();
            }
            catch
            {
            }
        }
    }
}

