using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace VSWebUI
{
    public partial class AlertSettings : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
           
            if (!IsPostBack)
            {
              OKButton.Enabled = false;
              MinutesTextBox.Enabled = false;
              FillPrimaryCombobox();
              FillBackupCombobox();
              ReadSettings();
            }
        }

        protected void FormCancelButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Configurator/Default.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
            Context.ApplicationInstance.CompleteRequest();
        }

        public DataTable RestrServers()
        {
            DataTable DCTasksDataTable = VSWebBL.ConfiguratorBL.DominoClusterBL.Ins.GetServer();
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


        public void FillPrimaryCombobox()
        {
            DataTable primarydt = RestrServers();
            PrimaryComboBox.DataSource = primarydt;
            PrimaryComboBox.TextField = "ServerName";
            PrimaryComboBox.ValueField = "ServerName";
            PrimaryComboBox.DataBind();
        }
        public void FillBackupCombobox()
        {
            DataTable Backupdt = RestrServers();
            BackupComboBox.DataSource = Backupdt;
            BackupComboBox.TextField = "ServerName";
            BackupComboBox.ValueField = "ServerName";
            BackupComboBox.DataBind();


        }

        // read from Settings Table

        public void ReadSettings()
        {
            try
            {
                PrimaryComboBox.Text = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("Primary Server");
                BackupComboBox.Text = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("Backup Server");
                RepeatCheckBox.Checked = bool.Parse(VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("Alert Repeat"));
                if (RepeatCheckBox.Checked == true)
                {
                    MinutesTextBox.Text = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("Alert Repeat Interval");
                    MinutesTextBox.Enabled = true;
                }
                bool Alert = false;
                Alert =bool.Parse(VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("EnableAlerts"));
                if (Alert == true)
                {
                    AlertButton.Text = "Alert OFF";
                    AlertButton.BackColor = System.Drawing.Color.Green;
                }
                else
                {
                    AlertButton.Text = "Alert ON";
                }


            }
            catch (Exception ex)
            {
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw;
            }
          

        }

        // Write into Settings table
        public void WriteSettings()
        {
            bool returnvalue;
            string Notupdate = "";
            try
            {
                returnvalue = VSWebBL.SettingBL.SettingsBL.Ins.UpdateSvalue("Primary Server", PrimaryComboBox.Text, VSWeb.Constants.Constants.SysString);
                if (returnvalue == false)
                {
                    Notupdate += "Primary Server";
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
                returnvalue = VSWebBL.SettingBL.SettingsBL.Ins.UpdateSvalue("Backup Server", BackupComboBox.Text, VSWeb.Constants.Constants.SysString);
                if (returnvalue == false)
                {
                    Notupdate += ",Backup Server";
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
                if (AlertButton.Text == "Alert OFF")
                {
                    returnvalue = VSWebBL.SettingBL.SettingsBL.Ins.UpdateSvalue("EnableAlerts", "True", VSWeb.Constants.Constants.SysString);
                    if (returnvalue == false)
                    {
                        Notupdate += ",EnableAlerts";
                    }
                }
                else 
                {
                    returnvalue = VSWebBL.SettingBL.SettingsBL.Ins.UpdateSvalue("EnableAlerts", "False", VSWeb.Constants.Constants.SysString);
                    if (returnvalue == false)
                    {
                        Notupdate += ",EnableAlerts";
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
                returnvalue = VSWebBL.SettingBL.SettingsBL.Ins.UpdateSvalue("Alert Repeat", RepeatCheckBox.Checked.ToString(), VSWeb.Constants.Constants.SysString);
                    if (returnvalue == false)
                    {
                        Notupdate += ",Alert Repeat";
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
                if(MinutesTextBox.Text!=""&& MinutesTextBox.Text!=null)
                    returnvalue = VSWebBL.SettingBL.SettingsBL.Ins.UpdateSvalue("Alert Repeat Interval", MinutesTextBox.Text, VSWeb.Constants.Constants.SysString);
                if (returnvalue == false)
                {
                    Notupdate += ",Alert Repeat Interval";
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
                MsgPopupControl.ShowOnPageLoad = true;
                MsgLabel.Text = "Alert settings saved.";
            }
            else
            {
                MsgPopupControl.ShowOnPageLoad = true;
                MsgLabel.Text=Notupdate+" were not updated.";
            }
           
        }

        protected void OKButton_Click(object sender, EventArgs e)
        {
            WriteSettings();

        }

        protected void AlertButton_Click(object sender, EventArgs e)
        {
            OKButton.Enabled = true;
            if ( AlertButton.Text == "Alert ON")
            {
               
                AlertButton.BackColor = System.Drawing.Color.Green;
                AlertButton.Text = "Alert OFF";
            }
            else 
            {
                AlertButton.Text = "Alert ON";
                
            }

        }

        protected void RepeatCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            OKButton.Enabled = true;
            if (RepeatCheckBox.Checked == true)
            {
                MinutesTextBox.Enabled = true;
            }
            else
            {
                MinutesTextBox.Enabled = false;
            }

        }

        protected void MinutesTextBox_TextChanged(object sender, EventArgs e)
        {
            OKButton.Enabled = true;
        }

        protected void MsgButton_Click(object sender, EventArgs e)
        {
           // Response.Redirect("~/Configurator/Default.aspx");
            MsgPopupControl.ShowOnPageLoad = false;
        }

      
    }
}