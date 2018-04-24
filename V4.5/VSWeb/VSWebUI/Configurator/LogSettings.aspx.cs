using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


using DevExpress.Web;

using VSWebDO;
using VSWebUI;
using System.Data;
namespace VSWebUI.Configurator
{
    public partial class WebForm1 : System.Web.UI.Page
	{
		string log="";
        string logSize = "";
        string logRotation = "";
		
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FillComboBoxes();
				Filltextbox("Log Files Path-New");
                FillLogFileSizeTextBox("Log File Size");
                FillLogFileRotationTextBox("Log File Rotation");
            }
			
        }

        protected void FillComboBoxes()
        {
            FillGivenComboBox(DefaultLogLevelComboBox,"Log Level");
            
            FillGivenComboBox(VSAdapterLogLevelComboBox,"Log Level VSAdapter");
			
        }

        protected void FillGivenComboBox(ASPxComboBox cb, string settingsTableName)
        {
            cb.Items.Clear();
            cb.Items.Add("Normal");
            cb.Items.Add("Debug");
            cb.Items.Add("Verbose");

            string value = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue(settingsTableName);
            cb.SelectedIndex = cb.Items.IndexOfText(ConvertLogNumberToString(value));
        }
		protected void Filltextbox( string log)
		{


			string value = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue(log);
			LogFilestb.Text = value;
		}
        protected String ConvertLogNumberToString(string s)
        {
            switch (s)
            {
                case "0":
                    return "Verbose";
                case "1":
                    return "Debug";
                case "2":
                    return "Normal";
                default:
                    return "";
            }

        }

        protected String ConvertLogStringToNumber(string s)
        {
            switch (s)
            {
                case "Verbose":
                    return "0";
                case "Debug":
                    return "1";
                case "Normal":
                    return "2";
                default:
                    return "Debug";
            }

        }

        protected void CancelButton_Click(object sender, EventArgs e)
        {
            RedirectToPage();
        }

        private void RedirectToPage()
        {
            if (Request.QueryString["dboard"] != null && Request.QueryString["dboard"].ToString() != "")
            {
                Response.Redirect("~/Dashboard/OverallHealth1.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
                Context.ApplicationInstance.CompleteRequest();
            }
            else
            {
                Response.Redirect("~/Configurator/Default.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
                Context.ApplicationInstance.CompleteRequest();
            }
        }

        protected void SaveButton_Click(object sender, EventArgs e)
        {

			if (!System.IO.Directory.Exists(LogFilestb.Text) && LogFilestb.Text != "")
			{
				successDiv.Style.Value = "display: none";
				errorDiv.Style.Value = "display: block";
				errorDiv.InnerText = "The directory " + LogFilestb.Text + " does not exist.  Please enter a valid directory";
				return;
			}
			else
			{
				errorDiv.Style.Value = "display: none";
			}
            SaveComboBoxes();

            successDiv.Style.Value = "display: block";
        }

        protected void SaveComboBoxes()
        {
            SaveGivenComboBox(DefaultLogLevelComboBox, "Log Level");
            SaveGivenComboBox(VSAdapterLogLevelComboBox, "Log Level VSAdapter");
			savelogfiles(log);
            saveLogFileSize(logSize);
            saveLogFileRotation(logRotation);
            if (DefaultLogLevelComboBox.SelectedItem.Text == "Verbose" || VSAdapterLogLevelComboBox.SelectedItem.Text == "Verbose")
            {
                success.Style.Value = "display: block";
            }
            else
            {
                success.Style.Value = "display: none";
            }

        }

        protected void SaveGivenComboBox(ASPxComboBox cb, string settingsTableName)
        {
            Settings st = new Settings();
		
            st.svalue = ConvertLogStringToNumber(cb.Text);
            st.stype = "System.Int32";

            VSWebBL.SettingBL.SettingsBL.Ins.UpdateSettings(st, settingsTableName);
        }
		protected void savelogfiles(string log)
		{
			Settings st = new Settings();

			st.svalue = LogFilestb.Text;
			st.stype = "System.String";
			log = "Log Files Path-New";
			st.sname = log;
			VSWebBL.SettingBL.SettingsBL.Ins.UpdateSettings(st, log);
		}
        protected void FillLogFileSizeTextBox(string logSize)
        {
            string value = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue(logSize);
            LogfileSizetb.Text = value;
        }

        protected void FillLogFileRotationTextBox(string logRotation)
        {
            string value = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue(logRotation);
            LogfileRotationtb.Text = value;
        }

        protected void saveLogFileSize(string logSize)
        {
            Settings st = new Settings();
            st.svalue = LogfileSizetb.Text;
            st.stype = "System.Int32";
            logSize = "Log File Size";
            st.sname = logSize;
            VSWebBL.SettingBL.SettingsBL.Ins.UpdateSettings(st, logSize);
        }
        private void saveLogFileRotation(string logRotation)
        {
            Settings st = new Settings();
            st.svalue = LogfileRotationtb.Text;
            st.stype = "System.Int32";
            logRotation = "Log File Rotation";
            st.sname = logRotation;
            VSWebBL.SettingBL.SettingsBL.Ins.UpdateSettings(st, logRotation);
        }

		//protected void logfilesdeletebt_Click(object sender, EventArgs e)
		//{
		//    //DataTable dt = (DataTable)Session["LogFiles"];

		//    string logPath = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("Log Files Path");
		//    string[] filePaths = System.IO.Directory.GetFiles(logPath);

		//    foreach (string file in filePaths)
		//    {
		//        //if (System.IO.File.Exists(@"C:\Users\Public\DeleteTest\test.txt"))
		//        if (System.IO.File.Exists(@file))
		//        {


		//            System.IO.File.Delete(@file);


		//        }
		//    }
		//}

		protected void logfilesdeletebt_Click(object sender, EventArgs e)
		{

			DeletePopupControl.ShowOnPageLoad = true;
			OKCopy.Visible = true;
			Cancel.Visible = true;

		}
		protected void OKCopy_Click(object sender, EventArgs e)
		{
			try
			{
				string logPath = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("Log Files Path-New");
				if (logPath == "")
				{
					logPath = "C:\\Program Files (x86)\\VitalSignsPlus\\log_files";
				}
				string[] filePaths = System.IO.Directory.GetFiles(logPath);

				foreach (string file in filePaths)
				{

					if (System.IO.File.Exists(@file))
					{


						System.IO.File.Delete(@file);


					}
				}

				Response.Redirect("~/Configurator/LogSettings.aspx");
			}
			catch (Exception ex)
			{
				DeletePopupControl.ShowOnPageLoad = false;
				
				//Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Error zipping log files. Exception - " + ex.Message);
				//errorDiv.InnerText = "error in sending log files";
				errorDiv.Style.Value = "display: block";
				errorDiv.InnerHtml = "Cannot delete log files. Pl give Full Access permission on the folder to IIS user.";
			//	Response.Redirect("~/Configurator/LogSettings.aspx");
			}

		}
		protected void Cancel_Click(object sender, EventArgs e)
		{
			Response.Redirect("~/Configurator/LogSettings.aspx");

		}
    }
}