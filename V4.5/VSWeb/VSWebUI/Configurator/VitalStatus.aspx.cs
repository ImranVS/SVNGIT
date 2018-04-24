using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using VSWebDO;
using VSWebBL;


namespace VSWebUI
{
    public partial class WebForm26 : System.Web.UI.Page
    {
        Domino.NotesSession session;

        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Title = "VitalStatus";
            if (!IsPostBack)
            {
                FillServersCombobox();
                ReadSettings();
            }
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

        public void FillServersCombobox()
        {
            DataTable DCStatDataTable = RestrServers();
            DServerComboBox.DataSource = DCStatDataTable;   //DataSource = DCStatDataTable;
            DServerComboBox.TextField = "ServerName";
            DServerComboBox.ValueField = "ServerName";
            DServerComboBox.DataBind();
        
        }


        private void ReadSettings()
        {
            try
            {
                // if(VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("ProxyEnabled")!=null)

                CnctDBCheckBox.Checked = bool.Parse(VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("Notes Output"));

            }
            catch (Exception ex)
            {
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                CnctDBCheckBox.Checked = false;
            }

            try
            {
                DServerComboBox.Text = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("Notes Output Server");
            }
            catch (Exception ex)
            {
                DBTextBox.Text = "";
                CnctDBCheckBox.Checked = false;
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
            }

            try
            {

                DBTextBox.Text = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("Notes Output Database");
            }
            catch (Exception ex)
            {
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;

            }

            if (CnctDBCheckBox.Checked == true)
            {
                DBTextBox.Enabled = true;
                DServerComboBox.Enabled = true;
            }
            else
            {
                DBTextBox.Enabled = false;
                DServerComboBox.Enabled = false;
            }

            ApplyButton.Enabled = false;
        }


        protected void ApplyButton_Click(object sender, EventArgs e)
        {
            WriteSettings();
        }

        private void WriteSettings()
        {
            bool returnvalue=false;
            string NotUpdate = "";
            try
            {
                if (CnctDBCheckBox.Checked == true)
                {
                    try
                    {
                        returnvalue = VSWebBL.SettingBL.SettingsBL.Ins.UpdateSvalue("Notes Output", CnctDBCheckBox.Checked.ToString(), VSWeb.Constants.Constants.SysString);
                       if (returnvalue == false)
                       {
                           NotUpdate += "Notes Output";
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
                        VSWebBL.SettingBL.SettingsBL.Ins.UpdateSvalue("Notes Output Server", DServerComboBox.Text, VSWeb.Constants.Constants.SysString);
                        if (returnvalue == false)
                        {
                            NotUpdate += ",Notes Output Server";
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
                        VSWebBL.SettingBL.SettingsBL.Ins.UpdateSvalue("Notes Output Database", DBTextBox.Text, VSWeb.Constants.Constants.SysString);
                        if (returnvalue == false)
                        {
                            NotUpdate += ",Notes Output Database";
                        }
                    }
                    catch (Exception ex)
                    {
                        //6/27/2014 NS added for VSPLUS-634
                        Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                        throw ex;
                    }
                  

                   
                }
                else
                {
                    VSWebBL.SettingBL.SettingsBL.Ins.UpdateSvalue("Notes Output", "False", VSWeb.Constants.Constants.SysString);
                   
                }

               // returnvalue = true;
            }
            catch (Exception ex)
            {
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                returnvalue = false;
            }
            if (NotUpdate == "" || NotUpdate == null)
            {
                //1/10/2014 NS modified
                /*
                ErrorMessagePopupControl.ShowOnPageLoad = true;
                ErrorMessageLabel.Text = "Data updated successfully.";
                ErrorMessagePopupControl.HeaderText = "Information";
                ErrorMessagePopupControl.ShowCloseButton = false;
                ValidationUpdatedButton.Visible = true;
                ValidationOkButton.Visible = false;
                 */
                //10/6/2014 NS modified for VSPLUS-990
                successDiv.InnerHtml = "Data updated successfully."+
                    "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                successDiv.Style.Value = "display: block";
                errorDiv.Style.Value = "display: none";
            }
            else
            {
                //1/10/2014 NS modified
                /*
                ErrorMessagePopupControl.ShowOnPageLoad = true;
                ErrorMessageLabel.Text = NotUpdate+"Data was not updated.";
                ErrorMessagePopupControl.HeaderText = "Information";
                ErrorMessagePopupControl.ShowCloseButton = false;
                ValidationUpdatedButton.Visible = true;
                ValidationOkButton.Visible = false;
                 */
                //10/6/2014 NS modified for VSPLUS-990
                errorDiv.InnerHtml = "Data was not updated."+
                    "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                errorDiv.Style.Value = "display: block";
                successDiv.Style.Value = "display: none";
            }
        }

        protected void CancelButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("Default.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
            Context.ApplicationInstance.CompleteRequest();
        }

        protected void CnctDBCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (CnctDBCheckBox.Checked == true)
            {
                DBTextBox.Enabled = true;
                DServerComboBox.Enabled = true;
                ApplyButton.Enabled = true;
            }
            else
            {
                DBTextBox.Text = "";
                DServerComboBox.Text = "";
                DBTextBox.Enabled = false;
                DServerComboBox.Enabled = false;
            }

           
        }

        protected void ValidationUpdatedButton_Click(object sender, EventArgs e)
        {
            //Response.Redirect("Default.aspx");
            ErrorMessagePopupControl.ShowOnPageLoad = false;
        }

        protected void DServerComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyButton.Enabled = true;
        }

        protected void DBTextBox_TextChanged(object sender, EventArgs e)
        {
            ApplyButton.Enabled = true;
        }

        protected void CopyTemplateButton_Click(object sender, EventArgs e)
        {
            //Object myEncryptedPassword;

            //string MyDominoPassword = "";
            //try
            //{
            //    myEncryptedPassword = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("Password");
            //}
            //catch (Exception)
            //{

            //    myEncryptedPassword = null;
            //    MyDominoPassword = null;
            //}
            //if (myEncryptedPassword != null)
            //{
            //    Byte[] Mypass;
            //    Mypass[] =  // VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("Password");

            //}
           
            //    MyPass = myRegistry.ReadFromRegistry("Password")  'password as encrypted byte stream
            //    Dim mySecrets As New TripleDES
            //    'Get the password from the registry
            //    MyDominoPassword = mySecrets.Decrypt(myEncryptedPassword) 'password in clear text
            //End If
           
        }

     




    }
}