using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace VSWebUI.Configurator
{
    public partial class Proxy : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Title = "Proxy Details";
            if (!IsPostBack)
            {
                ReadSettings();
            }
        }

        protected void FormOkButton_Click(object sender, EventArgs e)
        {
            WriteSettings();
        }

        protected void FormCancelButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("URLsGrid.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
            Context.ApplicationInstance.CompleteRequest();
        }

        protected void ProxyCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (ProxyCheckBox.Checked == true)
            {
                ProxyPortTextBox.Enabled = true;
                ProxyServerTextBox.Enabled = true;
                ProxyUserTextBox.Enabled = true;
                ProxyPasswordTextBox.Enabled = true;

            }
            else
            {
                ProxyPasswordTextBox.Enabled = false;
                ProxyPortTextBox.Enabled = false;
                ProxyServerTextBox.Enabled = false;
                ProxyUserTextBox.Enabled = false;
              
            }
        }

        private void ReadSettings()
        {
            try
            {  
               // if(VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("ProxyEnabled")!=null)
                
                ProxyCheckBox.Checked = bool.Parse(VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("ProxyEnabled"));

            }
            catch (Exception ex)
            {
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
               // ProxyCheckBox.Checked =true;
                ProxyCheckBox.Checked = false;
            }

            try
            {
                ProxyPasswordTextBox.Text = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("ProxyPassword");
				if (ProxyPasswordTextBox.Text == "")
				{
					ProxyPasswordTextBox.Visible = true;
				}
				else
				{
					LinkButton1.Visible = true;
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
                
                ProxyPortTextBox.Text = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("ProxyPort");
            }
            catch (Exception ex)
            {
                ProxyPortTextBox.Text = "80";
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
            }

            try
            {
                ProxyServerTextBox.Text = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("ProxyServer");
            }
            catch (Exception ex)
            {
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }

            try
            {
                ProxyUserTextBox.Text = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("ProxyUser");
            }
            catch (Exception ex)
            {
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }

           
        }
        private void WriteSettings()
        {
            bool returnvalue;
            try
            {
                returnvalue = VSWebBL.SettingBL.SettingsBL.Ins.UpdateSvalue("ProxyEnabled", ProxyCheckBox.Checked.ToString(), VSWeb.Constants.Constants.SysString);
            }
            catch (Exception ex)
            {
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }

            try
            {
                if (ProxyPortTextBox.Text != null)
                {
                    returnvalue = VSWebBL.SettingBL.SettingsBL.Ins.UpdateSvalue("ProxyPort", ProxyPortTextBox.Text, VSWeb.Constants.Constants.SysString);
                }
                else
                {
                    VSWebBL.SettingBL.SettingsBL.Ins.UpdateSvalue("ProxyPort", "80", VSWeb.Constants.Constants.SysString);
                     ProxyPortTextBox.Text="80";
                }
            }
            catch (Exception ex)
            {
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
               ProxyPortTextBox.Text="80";
            }
            try
            {
                returnvalue = VSWebBL.SettingBL.SettingsBL.Ins.UpdateSvalue("ProxyUser", ProxyUserTextBox.Text, VSWeb.Constants.Constants.SysString);

            }
            catch (Exception ex)
            {
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
            try
            {
                returnvalue = VSWebBL.SettingBL.SettingsBL.Ins.UpdateSvalue("ProxyServer", ProxyServerTextBox.Text, VSWeb.Constants.Constants.SysString);
            }
            catch (Exception ex)
            {
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
            try
            {
                returnvalue = VSWebBL.SettingBL.SettingsBL.Ins.UpdateSvalue("ProxyPassword", ProxyPasswordTextBox.Text, VSWeb.Constants.Constants.SysString);
            }
            catch (Exception ex)
            {
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }

            if (returnvalue == true)
            {
                Response.Redirect("URLsGrid.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
                Context.ApplicationInstance.CompleteRequest();
            }
        }
		protected void confirmButton_Click(object sender, EventArgs e)
		{
			bool returnvalue;
			ASPxPopupControl2.ShowOnPageLoad = false;
			
			try
			{
				returnvalue = VSWebBL.SettingBL.SettingsBL.Ins.UpdateSvalue("ProxyPassword", cnpsw.Text, VSWeb.Constants.Constants.SysString);
			}
			catch (Exception ex)
			{
				//6/27/2014 NS added for VSPLUS-634
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}

		}

		protected void canclellbt_Click(object sender, EventArgs e)

		{
			
			ASPxPopupControl2.ShowOnPageLoad = false;


		}

    }
}