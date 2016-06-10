using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//using VSWebBL;
using System.Drawing;
using System.Text;
using System.Data;

namespace VSWebUI.Configurator
{
    public partial class LicenseInformation : System.Web.UI.Page
    {
        //public string MyProductName = "VitalSigns";
        //public string MonitorServiceName = "VitalSigns Monitoring Service";
        //public string DailyServiceName = "VitalSigns Daily Tasks";
        //public string MasterServiceName = "VitalSigns Master Service";
        //public string DBHealthServiceName = "VitalSigns Database Health";
        string myLicenseInfo;
        string myName;
        string myExpiration;
        int myCount;
        bool Licensed;
        bool Subscription;
		byte[] Mylicensekey;
		string[] MyEnkey;

		VSFramework.TripleDES mytestenkey = new VSFramework.TripleDES();
      //  string myKey;
        protected void Page_Load(object sender, EventArgs e)
        {
            //Page.Title = "License Information";
			if (!IsPostBack)
			{
				FillLicenseUsagegrid();
				ReadLicense();
				//LoadEnabled();
			}
			else
			{
				FillLicenseUsagegridfromSession();

			}
            successDiv.Style.Value = "display: none;";
        }
		private void ReadLicense()
		{
			string LicenseType = "";
			string Expiration = "";
            string InstallationType = "";

			DataTable licensedt = new DataTable();
			DataTable dt = VSWebBL.ConfiguratorBL.LicenseBL.Ins.Getlicenseunitsinfo();
			DataTable unitsused = VSWebBL.ConfiguratorBL.LicenseBL.Ins.Gettotalunitsused();

			if (unitsused.Rows.Count > 0)
			{
				//Totalunitspurchased.Text = dt.Rows[0]["UnitsPurchased"].ToString();
				Totalunitsused.Text = unitsused.Rows[0]["totalunitsused"].ToString();
				
				
			}
			licensedt = VSWebBL.ConfiguratorBL.LicenseBL.Ins.GetLicensedetails();
			if (licensedt.Rows.Count > 0)
			{
					LicenseType = licensedt.Rows[0]["LicenseType"].ToString();
					Expiration = licensedt.Rows[0]["ExpirationDate"].ToString();
                //4/23/2015 NS added for VSPLUS-1685
                    InstallationType = licensedt.Rows[0]["InstallType"].ToString();
				myCount = int.Parse(licensedt.Rows[0]["Units"].ToString());
				Totalunitspurchased.Text = licensedt.Rows[0]["Units"].ToString();
				if (licensedt.Rows[0]["Units"].ToString() != "" && unitsused.Rows[0]["totalunitsused"].ToString() != "")
				{
                    TotalUnitsRemaining.Text = (Convert.ToDouble(licensedt.Rows[0]["Units"].ToString()) - Convert.ToDouble(unitsused.Rows[0]["totalunitsused"].ToString())).ToString();
				} 
				DateTime currendate = DateTime.Now;
				if (Expiration != null && Expiration != "")
				{
					DateTime ExpirationDt = Convert.ToDateTime(Expiration);
					int result = DateTime.Compare(currendate, ExpirationDt);
					if (result <= 0)
					{
						LicenseCodeLabel.ForeColor = Color.Green;
					}
					else
					{
						LicenseCodeLabel.ForeColor = Color.Red;
					}
				}

				try
				{
					// LicenseCodeLabel.Text = myLicenseInfo; 
					//LicenseCodeLabel.Text = LicenseType + Expiration + strExpiryOn;
                    //4/23/2015 NS added for VSPLUS-1685
                    LicenseTypeLabel.Text = LicenseType + " " + InstallationType;
                    DateTime dateonlystr = Convert.ToDateTime(Expiration);
					LicenseCodeLabel.Text = dateonlystr.Date.ToShortDateString();
				}
				catch (Exception ex)
				{
                    //4/23/2015 NS modified for
                    LicenseTypeLabel.Text = LicenseType + " Version";
					//LicenseCodeLabel.Text = LicenseType + " Version";
					//6/27/2014 NS added for VSPLUS-634
					Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				}
			}
			
			try
			{
				// LicenseCodeLabel.Text = myLicenseInfo;
                //4/23/2015 NS added for VSPLUS-1685
				LicenseTypeLabel.Text = LicenseType + " " + InstallationType;
				LicenseExirationLabel.Text = "Expiration: ";
                if (Expiration != "" && !(LicenseType.Contains("Perpetual")))
                {
                    DateTime datetimestr = Convert.ToDateTime(Expiration);
                    LicenseCodeLabel.Text = datetimestr.Date.ToShortDateString();
                }
				else if(LicenseType.Contains("Perpetual")){
					LicenseCodeLabel.Text = "Never";
				}
                else
                {
                    LicenseCodeLabel.Text = "NA";
                }
                //LicenseCountValueLabel.Text = myCount.ToString();
				//LicenseCodeLabel.Text = LicenseType + " Valid Until: " + "  " + Expiration + " License Count: " + myCount;
			}
			catch (Exception ex)
			{
				//6/27/2014 NS added for VSPLUS-634
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}
		}
		//protected void SaveButton_Click(object sender, EventArgs e)
		//{
		//    WriteLicense();
		//}

		//private void LoadEnabled()
		//{
		//    bool enabledservice;
		//    try
		//    {
		//        enabledservice = bool.Parse(VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("MonitorDomino"));
		//        DominoServerCheckBox.Checked = enabledservice;
		//        enabledservice = bool.Parse(VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("MonitorNotesMail"));
		//        NotesMailCheckBox.Checked = enabledservice;
		//        enabledservice = bool.Parse(VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("MonitorNotesDatabases"));
		//        NotesDatabasesCheckBox.Checked = enabledservice;
		//        enabledservice = bool.Parse(VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("MonitorClusters"));
		//        DominoClusterCheckBox.Checked = enabledservice;
		//        enabledservice = bool.Parse(VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("MonitorConsoleCommands"));
		//        SheduledDominoCheckBox.Checked = enabledservice;
		//        enabledservice = bool.Parse(VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("MonitorBlackBerry"));
		//        BBDeviceProbeCheckBox.Checked = enabledservice;
		//        enabledservice = bool.Parse(VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("MonitorBlackBerryQueues"));
		//        BBMsgQCheckBox.Checked = enabledservice;
		//        enabledservice = bool.Parse(VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("MonitorBES"));
		//        BBEnterprizeServerCheckBox.Checked = enabledservice;
		//        enabledservice = bool.Parse(VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("MonitorBlackBerryUsers"));
		//        BBUsersCheckBox.Checked = enabledservice;
		//        enabledservice = bool.Parse(VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("MonitorURLs"));
		//        URLsCheckBox.Checked = enabledservice;
		//        enabledservice = bool.Parse(VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("MonitorMailServices"));
		//        MailServicesCheckBox.Checked = enabledservice;
		//        enabledservice = bool.Parse(VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("MonitorNetworkDevices"));
		//        NetworkDevicesCheckBox.Checked = enabledservice;
		//        enabledservice = bool.Parse(VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("MonitorSametime"));
		//        SameTimeCheckBox.Checked = enabledservice;
		//    }
		//    catch (Exception ex)
		//    {
		//        //Licensed = false;
		//        //6/27/2014 NS added for VSPLUS-634
		//        Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
		//    }
		//}

		////private void ReadLicense()
		////{
		////    DataTable licensedt = new DataTable();
		////    licensedt = VSWebBL.ConfiguratorBL.LicenseBL.Ins.GetLicensedetails();
		////    string LicenseType = licensedt.Rows[0]["LicenseType"].ToString();
		////    string Expiration = licensedt.Rows[0]["ExpirationDate"].ToString();
		////    myCount = int.Parse(licensedt.Rows[0]["Units"].ToString());
		////    LicenseCodeLabel.Text = LicenseType + Expiration;
		////    LicenseCodeLabel.ForeColor = Color.Green;
                
		////    myLicenseInfo = LicenseType + " Version - The VS service will run in '" + LicenseType + " Mode' with " + myCount.ToString() + " licenses.";

		////     myExpiration = Expiration;
               
		////   LicenseCodeLabel.Text = LicenseType + " Valid Until : " + "  " + Expiration + " &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;" + " License Count : " + myCount;

		////}
	


	   //private void  WriteLicense()
	   // {   string NotUpdatedvalues="";
	   //     bool returnval = false;
	   //     try
	   //     {
	   //         returnval = VSWebBL.SettingBL.SettingsBL.Ins.UpdateSvalue("MonitorBlackBerryUsers", BBUsersCheckBox.Checked.ToString(), VSWeb.Constants.Constants.SysString);
	   //         if(returnval==false) 
	   //             NotUpdatedvalues+="MonitorBlackBerryUsers,";
	   //     }
	   //     catch (Exception ex)
	   //     {
	   //         //6/27/2014 NS added for VSPLUS-634
	   //         Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
	   //         throw ex;
	   //     }
	   //     try
	   //     {
	   //         returnval = VSWebBL.SettingBL.SettingsBL.Ins.UpdateSvalue("MonitorBES", BBEnterprizeServerCheckBox.Checked.ToString(), VSWeb.Constants.Constants.SysString);
	   //         if (returnval == false)
	   //             NotUpdatedvalues += "MonitorBES,";
	   //     }
	   //     catch (Exception ex)
	   //     {
	   //         //6/27/2014 NS added for VSPLUS-634
	   //         Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
	   //         throw ex;
	   //     }
	   //     try
	   //     {
	   //         returnval = VSWebBL.SettingBL.SettingsBL.Ins.UpdateSvalue("MonitorBlackBerry", BBDeviceProbeCheckBox.Checked.ToString(), VSWeb.Constants.Constants.SysString);
	   //         if (returnval == false) NotUpdatedvalues += "MonitorBlackBerry,";
	   //     }
	   //     catch (Exception ex)
	   //     {
	   //         //6/27/2014 NS added for VSPLUS-634
	   //         Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
	   //         throw ex;
	   //     }
	   //     try
	   //     {
	   //         returnval = VSWebBL.SettingBL.SettingsBL.Ins.UpdateSvalue("MonitorDomino", DominoServerCheckBox.Checked.ToString(), VSWeb.Constants.Constants.SysString);
	   //         if (returnval == false) NotUpdatedvalues += "MonitorDomino,";
	   //     }
	   //     catch (Exception ex)
	   //     {
	   //         //6/27/2014 NS added for VSPLUS-634
	   //         Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
	   //         throw ex;
	   //     }
	   //     try
	   //     {
	   //         returnval = VSWebBL.SettingBL.SettingsBL.Ins.UpdateSvalue("MonitorNotesMail", NotesMailCheckBox.Checked.ToString(), VSWeb.Constants.Constants.SysString);
	   //         if (returnval == false) NotUpdatedvalues += "MonitorNotesMail,";
	   //     }
	   //     catch (Exception ex)
	   //     {
	   //         //6/27/2014 NS added for VSPLUS-634
	   //         Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
	   //         throw ex;
	   //     }

	   //     try
	   //     {
	   //         returnval = VSWebBL.SettingBL.SettingsBL.Ins.UpdateSvalue("MonitorURLs", URLsCheckBox.Checked.ToString(), VSWeb.Constants.Constants.SysString);
	   //         if (returnval == false) NotUpdatedvalues += "MonitorURLs,";
	   //     }
	   //     catch (Exception ex)
	   //     {
	   //         //6/27/2014 NS added for VSPLUS-634
	   //         Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
	   //         throw ex;
	   //     }
	   //     try
	   //     {
	   //         returnval = VSWebBL.SettingBL.SettingsBL.Ins.UpdateSvalue("MonitorSametime", SameTimeCheckBox.Checked.ToString(), VSWeb.Constants.Constants.SysString);
	   //         if (returnval == false) NotUpdatedvalues += "MonitorSametime,";
	   //     }
	   //     catch (Exception ex)
	   //     {
	   //         //6/27/2014 NS added for VSPLUS-634
	   //         Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
	   //         throw ex;
	   //     }

	   //     try
	   //     {
	   //         returnval = VSWebBL.SettingBL.SettingsBL.Ins.UpdateSvalue("MonitorNetworkDevices", NetworkDevicesCheckBox.Checked.ToString(), VSWeb.Constants.Constants.SysString);
	   //         if (returnval == false) NotUpdatedvalues += "MonitorNetworkDevices,";
	   //     }
	   //     catch (Exception ex)
	   //     {
	   //         //6/27/2014 NS added for VSPLUS-634
	   //         Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
	   //         throw ex;
	   //     }
	   //     try
	   //     {
	   //         returnval = VSWebBL.SettingBL.SettingsBL.Ins.UpdateSvalue("MonitorMailServices", MailServicesCheckBox.Checked.ToString(), VSWeb.Constants.Constants.SysString);
	   //         if (returnval == false) NotUpdatedvalues += "MonitorMailServices,";
	   //     }
	   //     catch (Exception ex)
	   //     {
	   //         //6/27/2014 NS added for VSPLUS-634
	   //         Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
	   //         throw ex;
	   //     }
	   //     try
	   //     {
	   //         returnval = VSWebBL.SettingBL.SettingsBL.Ins.UpdateSvalue("MonitorBlackBerryQueues", BBMsgQCheckBox.Checked.ToString(), VSWeb.Constants.Constants.SysString);
	   //         if (returnval == false) NotUpdatedvalues += "MonitorBlackBerryQueues,";
	   //     }
	   //     catch (Exception ex)
	   //     {
	   //         //6/27/2014 NS added for VSPLUS-634
	   //         Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
	   //         throw ex; 
	   //     }

	   //     try
	   //     {

	   //         returnval = VSWebBL.SettingBL.SettingsBL.Ins.UpdateSvalue("MonitorNotesDatabases", NotesDatabasesCheckBox.Checked.ToString(), VSWeb.Constants.Constants.SysString);
	   //         if (returnval == false) NotUpdatedvalues += "MonitorNotesDatabases,";
	   //     }
	   //     catch (Exception ex)
	   //     {
	   //         //6/27/2014 NS added for VSPLUS-634
	   //         Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
	   //         throw ex;
	   //     }
	   //     try
	   //     {
	   //         returnval = VSWebBL.SettingBL.SettingsBL.Ins.UpdateSvalue("MonitorConsoleCommands", SheduledDominoCheckBox.Checked.ToString(), VSWeb.Constants.Constants.SysString);
	   //         if (returnval == false) NotUpdatedvalues += "MonitorConsoleCommands,";
	   //     }
	   //     catch (Exception ex)
	   //     {
	   //         //6/27/2014 NS added for VSPLUS-634
	   //         Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
	   //         throw ex;
	   //     }
	   //     try
	   //     {
	   //         returnval = VSWebBL.SettingBL.SettingsBL.Ins.UpdateSvalue("MonitorClusters", DominoClusterCheckBox.Checked.ToString(), VSWeb.Constants.Constants.SysString);
	   //         if (returnval == false) NotUpdatedvalues += "MonitorClusters,";
	   //     }

	   //     catch (Exception ex)
	   //     {
	   //         //6/27/2014 NS added for VSPLUS-634
	   //         Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
	   //         throw ex;
	   //     }
	   //     finally
	   //     {
	   //         if (NotUpdatedvalues == "")
	   //         {
	   //             //12/24/2013 NS modified
	   //            // SaveButton.Enabled = false;
	   //             //LicensePopupControl.ShowOnPageLoad = true;
	   //             //5/23/2013 NS modified
	   //             //passwordLabel.Text = "Data updated successfully.";
	   //             //KeyTextBox.Visible = false;
	   //             //KeyOK.Visible = false;
	   //             //KeyOKSave.Visible = true;
	   //             errorDiv.Style.Value = "display: none";
	   //             successDiv.Style.Value = "display: block";
	   //         }
	   //         else
	   //         {
	   //             //12/24/2013 NS modified
	   //             //LicensePopupControl.ShowOnPageLoad = true;
	   //             //passwordLabel.Text = NotUpdatedvalues + " were not updated.";
	   //             //KeyTextBox.Visible = false;
	   //             //KeyOK.Visible = false;
	   //             //KeyOKSave.Visible = true;
	   //             //SaveButton.Enabled = true;
	   //             successDiv.Style.Value = "display: none";
	   //             //10/3/2014 NS modified for VSPLUS-990
	   //             errorDiv.InnerHtml = "The following fields were not updated: " + NotUpdatedvalues+
	   //                 "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
	   //             errorDiv.Style.Value = "display: block";
	   //         }
	   //     }
	   //}

       protected void KeyOK_Click(object sender, EventArgs e)
       {
          // myKey = KeyTextBox.Text;
           string mykey;
		   string[] words;
           mykey = KeyTextBox.Text;
		   string Decriptkey = "";
		   //Byte[] inputInBytes;
		
		  

           try
		   {
			   
			   //byte[] array = Encoding.ASCII.GetBytes(mykey);
			   MyEnkey = mykey.Split(',');
			   Mylicensekey = new byte[MyEnkey.Length];
			   for (int j = 0; j < MyEnkey.Length; j++)
			   {
				   Mylicensekey[j] = Byte.Parse(MyEnkey[j]);
			   }
			  // Decriptkey = VSWebBL.SettingBL.TripleDES.Ins.Decrypt(Mylicensekey);
			   Decriptkey = mytestenkey.Decrypt(Mylicensekey);

			   if (Decriptkey != null)

               {

				   if (Decriptkey.Contains("#"))
				   {
					   words = Decriptkey.Split(new[] { "#" }, StringSplitOptions.RemoveEmptyEntries);

				   }
				   else
				   {
					   words = null;
				   }

                   //4/28/2015 NS added to handle invalid licenses
                   if (words != null)
                   {
                       License lic = new License();
                       //lic.LicenseKey=mykey;
                       lic.LicenseKey = mykey;
                       lic.Units = Convert.ToInt32(words[0]);
                       lic.InstallType = words[1];
                       lic.CompanyName = words[2];

                       lic.LicenseType = words[3];
                       lic.ExpirationDate = words[4];

                       VSWebBL.ConfiguratorBL.LicenseBL.Ins.InsertLicense(lic);

                       //4/23/2015 NS modified for VSPLUS-1685
                       /*
                       if (ServiceStatus() == true)
                       {
                           LicenseCodeLabel.Text = "You must restart the service for the changes to take affect.";
                           //Thread threadStopService = new Thread(myCaller.StopService);
                           //threadStopService.Start();
                       }
                       else
                       {
                           LicenseCodeLabel.Text = "The new license code will go into effect when the service starts.";
                       }
                        */
                       successDiv.InnerHtml = "License information updated.";
                       successDiv.Style.Value = "display: block";
                       errorDiv.Style.Value = "display: none";
                   }
                   else
                   {
                       errorDiv.InnerHtml = "Lincense key is invalid." + 
                           "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                       errorDiv.Style.Value = "display: block";
                       successDiv.Style.Value = "display: none";
                   }
               }
			   LicensePopupControl.ShowOnPageLoad = false;
           }
           catch (Exception ex)
           {
			   erordivinpopup.InnerHtml = "Please enter valid License Key." +
						  "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
			   erordivinpopup.Style.Value = "display: block";
			 //  erordivinpopup.Style.Value = "display: none";
               //6/27/2014 NS added for VSPLUS-634
           //    Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
             //  throw ex;
           }
         
       }

       protected void LicenseCodeButton2_Click(object sender, EventArgs e)
       {
           LicensePopupControl.ShowOnPageLoad = true;
           KeyTextBox.Visible = true;
           KeyOK.Visible = true;
           KeyOKSave.Visible = false;
       }

        private bool ServiceStatus()
        {
           // here add service Code
            bool x=true;
            return x;
        }

        protected void CancelButton_Click(object sender, EventArgs e)
        {
            //Clears CheckBox Selection
            //DNSCheckBox.Checked = false;
            //DominoServerCheckBox.Checked = false;
            //NotesMailCheckBox.Checked = false;
            //NotesDatabasesCheckBox.Checked = false;
            //DominoClusterCheckBox.Checked = false;
            //SheduledDominoCheckBox.Checked = false;
            //BBDeviceProbeCheckBox.Checked = false;
            //BBMsgQCheckBox.Checked = false;
            //BBEnterprizeServerCheckBox.Checked = false;
            //BBUsersCheckBox.Checked = false;
            //URLsCheckBox.Checked = false;
            //MailServicesCheckBox.Checked = false;
            //NetworkDevicesCheckBox.Checked = false;
            //SameTimeCheckBox.Checked = false;
            Response.Redirect("~/configurator/Default.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
            Context.ApplicationInstance.CompleteRequest();
            
            //SaveButton.Enabled = true;
        }

        protected void KeyOKSave_Click(object sender, EventArgs e)
        {
            LicensePopupControl.ShowOnPageLoad = false;
        }
        
        // foreach (Control control in controls)
        //{
        //   if (control.GetType().Equals(CheckBox))
        //   {
        //       //----------
        //   }
        //}    

		private void FillLicenseUsagegrid()
		{
			try
			{

				DataTable dt = VSWebBL.ConfiguratorBL.LicenseBL.Ins.Getlicenseusage();
				LicenseUsage.DataSource = dt;
				Session["Licence"] = dt;
				LicenseUsage.DataBind();


			}
			catch (Exception ex)
			{
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}
			finally { }
		}

	
		private void FillLicenseUsagegridfromSession()

		{
			try
			{

				DataTable Licenceinformation = new DataTable();
				if (Session["Licence"] != null && Session["Licence"] != "")
					Licenceinformation = (DataTable)Session["Licence"];//VSWebBL.ConfiguratorBL.WindowsPropertiesBL.Ins.GetAllData();
				if (Licenceinformation.Rows.Count > 0)
				{
					LicenseUsage.DataSource = Licenceinformation;
					LicenseUsage.DataBind();
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
    }
}


//try
//            {
//                Licensed =bool.Parse( VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("Registered"));
//                Subscription=bool.Parse(VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("Subscription"));
//            }
//            catch 
//            {
//                Licensed = false;
//            }
//            if (Licensed == true)
//            {
//                try
//                {
//                    myCount = int.Parse(VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("License_Count"));
//                }
//                catch
//                {

//                    myCount = 2;
//                }

//                try
//                {
//                    myName = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("Licensed To");

//                }
//                catch
//                {

//                    myName = "";
//                }
//                LicenseCodeLabel.ForeColor = Color.Green;
//                try
//                {
//                    if (myName != "")
//                    {

//                        myLicenseInfo = "Full License  with" + myCount.ToString() + "Licenses. Company Name:" + myName;
//                    }
//                    else
//                    {
//                        myLicenseInfo = "Full License  with" + myCount.ToString() + "Licenses.";

//                    }
//                }
//                catch 
//                {

//                    myLicenseInfo = "License information not found.";
//                }

             
//                try
//                {
//                    LicenseCodeLabel.Text = myLicenseInfo;
//                }
//                catch 
//                {
//                    LicenseCodeLabel.Text = "Evaluation Version";
                   
//                }
               
//            }
//            else if (Subscription == true)
//            {
//                try
//                {
//                    myCount = int.Parse(VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("License_Count"));
//                }
//                catch
//                {

//                    myCount = 2;
//                }

//                try
//                {
//                    myName = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("Licensed To");

//                }
//                catch
//                {

//                    myName = "";
//                }
//                LicenseCodeLabel.ForeColor = Color.Green;
//                try
//                {
//                    myExpiration = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("Evaluation Expiration");

//                }
//                catch
//                {

//                    myExpiration = "";
//                }
//                try
//                {
//                    if (myName != "")
//                    {

//                        myLicenseInfo = "Subscription License  with" + myCount.ToString() + "Licenses. Company Name:" + myName;
//                    }
//                    else
//                    {
//                        myLicenseInfo = "Subscription License  with" + myCount.ToString() + "Licenses. valid until :" + myExpiration;

//                    }
//                }
//                catch
//                {

//                    myLicenseInfo = "License information not found.";
//                }


//                try
//                {
//                    LicenseCodeLabel.Text = myLicenseInfo;
//                }
//                catch
//                {
//                    LicenseCodeLabel.Text = "Evaluation Version";

//                }
//            }
//            //Not Registered

//            else
//            {

//                try
//                {
//                    myCount = int.Parse(VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("License_Count"));
//                }
//                catch
//                {

//                    myCount = 2;
//                }

//                if(myCount<2)
//                {
//                myCount=2;
//                }

//                myLicenseInfo = "Evaluation Version - The VS service will run in 'Evaluation Mode' with " + myCount.ToString() + " licenses.";
//                try
//                {
//                    myExpiration = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("Evaluation Expiration");
//                }
//                catch 
//                {
//                    myExpiration = "";
//                }
//                try
//                {
//                    if(myExpiration!=""&& myExpiration!=null)
//                    {
//                       myLicenseInfo+="Expiration date: " +myExpiration;
                    
//                    }
//                }
//                catch (Exception)
//                {                    
//                    throw;
//                }

//                try
//                {
//                    string myKey;
//                    myKey = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("License Key");
//                    if (myKey == ""||myKey==null)
//                    {
//                        myLicenseInfo += " Please contact your authorized reseller for a license key.";
//                    }
                    
//                }
//                catch (Exception)
//                {
                    
//                    throw;
//                }

//            }
