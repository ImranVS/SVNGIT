using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ServiceProcess;
using System.Data;
namespace VSWebUI
{
    public partial class WebForm27 : System.Web.UI.Page
    {
        //1/24/2013 NS modified - VitalSignsPlusDomino replaced the VitalSigns Monitoring Service
        //ServiceController svcController = new ServiceController("VitalSigns Monitoring Service");
        //1/8/2014 NS modified
        ServiceController VSDominoController;
        ServiceController VSDailyTasksController;
        ServiceController VSDBHealthController;
        ServiceController VSMasterSvcController;
        ServiceController VSCoreFeaturesController;
        ServiceController VSConsoleCmdsController;
        ServiceController VSAlertingSvcController;
        ServiceController VSMicrosoftController;
		//Durga 1493
		ServiceController VSCoreServiceController;

        string Startime1, Endtime1;
        string Startime2, Endtime2;
        string Startime3, Endtime3;
        string Startime4, Endtime4;
        string Startime5, Endtime5;
        string Startime6, Endtime6;
        string Startime7, Endtime7;

        bool DominoEnabled = false;
        bool MicrosoftEnabled = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack){
                DataTable ConfigDt = VSWebBL.SecurityBL.MenusBL.Ins.GetSelectedFeatures();

                for (int i = 0; i < ConfigDt.Rows.Count; i++)
                {
                    string s = ConfigDt.Rows[i]["Name"].ToString();
                    if (ConfigDt.Rows[i]["Name"].ToString() == "Domino")
                    {
                        DominoEnabled = true;
                        trDomino.Visible = true;
                        trRemote.Visible = true;
                        trDatabase.Visible = true;

                    }
					if (ConfigDt.Rows[i]["Name"].ToString() == "Exchange" || ConfigDt.Rows[i]["Name"].ToString() == "Active Directory" || ConfigDt.Rows[i]["Name"].ToString() == "Sharepoint" || ConfigDt.Rows[i]["Name"].ToString() == "Windows" || ConfigDt.Rows[i]["Name"].ToString() == "Lync")
                    {
                        MicrosoftEnabled = true;
                        trMicrosoft.Visible = true;
                    }
                    
                }


            }
            //1/8/2014 NS added
            string ControllerName = "";
            ControllerName = "VitalSignsPlusDomino";
            try
            {
                VSDominoController = new ServiceController(ControllerName);
            }
            catch (Exception ex)
            {
                errorDiv.Style.Value = "display: block";
                //10/6/2014 NS modified for VSPLUS-990
                errorDiv.InnerHtml = ControllerName + " could not be loaded. The following error has occurred: " + ex.Message+
                    "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
            }
            ControllerName = "VitalSigns Daily Tasks";
            try
            {
                VSDailyTasksController = new ServiceController(ControllerName);
            }
            catch (Exception ex)
            {
                errorDiv.Style.Value = "display: block";
                //10/6/2014 NS modified for VSPLUS-990
                errorDiv.InnerHtml = ControllerName + " could not be loaded. The following error has occurred: " + ex.Message+
                    "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
            }
            ControllerName = "VitalSigns Database Health";
            try
            {
                VSDBHealthController = new ServiceController(ControllerName);
            }
            catch (Exception ex)
            {
                errorDiv.Style.Value = "display: block";
                //10/6/2014 NS modified for VSPLUS-990
                errorDiv.InnerHtml = ControllerName + " could not be loaded. The following error has occurred: " + ex.Message+
                    "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
            }
            ControllerName = "VitalSigns Plus Master Service";
            try
            {
                VSMasterSvcController = new ServiceController(ControllerName);
            }
            catch (Exception ex)
            {
                errorDiv.Style.Value = "display: block";
                //10/6/2014 NS modified for VSPLUS-990
                errorDiv.InnerHtml = ControllerName + " could not be loaded. The following error has occurred: " + ex.Message+
                    "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
            }
            ControllerName = "VitalSignsPlusCore";
            try
            {
                VSCoreFeaturesController = new ServiceController(ControllerName);
            }
            catch (Exception ex)
            {
                errorDiv.Style.Value = "display: block";
                //10/6/2014 NS modified for VSPLUS-990
                errorDiv.InnerHtml = ControllerName + " could not be loaded. The following error has occurred: " + ex.Message+
                    "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
            }
			//Durga 1493
			ControllerName = "VitalSignsCore64";
			try
			{
				VSCoreServiceController = new ServiceController(ControllerName);
			}
			catch (Exception ex)
			{
				errorDiv.Style.Value = "display: block";
				//10/6/2014 NS modified for VSPLUS-990
				errorDiv.InnerHtml = ControllerName + " could not be loaded. The following error has occurred: " + ex.Message +
					"<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
				//6/27/2014 NS added for VSPLUS-634
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
			}
            ControllerName = "VSConsoleCommand";
            try
            {
                VSConsoleCmdsController = new ServiceController(ControllerName);
            }
            catch (Exception ex)
            {
                errorDiv.Style.Value = "display: block";
                //10/6/2014 NS modified for VSPLUS-990
                errorDiv.InnerHtml = ControllerName + " could not be loaded. The following error has occurred: " + ex.Message+
                    "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";

                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
            }
            ControllerName = "VitalSignsAlerts";
            try
            {
                VSAlertingSvcController = new ServiceController(ControllerName);
            }
            catch (Exception ex)
            {
                errorDiv.Style.Value = "display: block";
                //10/6/2014 NS modified for VSPLUS-990
                errorDiv.InnerHtml = ControllerName + " could not be loaded. The following error has occurred: " + ex.Message+
                    "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
            }
            ControllerName = "VitalSignsMicrosoft";
            try
            {
                VSMicrosoftController = new ServiceController(ControllerName);
            }
            catch (Exception ex)
            {
                errorDiv.Style.Value = "display: block";
                //10/6/2014 NS modified for VSPLUS-990
                errorDiv.InnerHtml = ControllerName + " could not be loaded. The following error has occurred: " + ex.Message+
                    "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
            }
           
            try
            {
                if (!IsPostBack)
                {
                    //1/8/2014 NS added
                    if (VSDominoController != null && DominoEnabled)
                    {
                        //ControllerName = "VitalSigns Plus for Domino";
                        try
                        {
                            lblS1.Text = VSDominoController.Status.ToString();
                            Session["ServiceStatus"] = VSDominoController.Status.ToString();
                            lblStarted1.Text = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("VS Domino Service Start");
                        }
                        catch (Exception ex)
                        {
                            lblS1.Text = "Not Started";
                            errorDiv.Style.Value = "display: block";
                            //10/6/2014 NS modified for VSPLUS-990
                            errorDiv.InnerHtml = "The following error has occurred: " + ex.Message+
                                "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                            //6/27/2014 NS added for VSPLUS-634
                            Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                        }
                        //1/8/2014 NS commented out
                        //lblB1.Text = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("VS Domino Build Number");
                    }
                    if (VSDailyTasksController != null)
                    {
                        //ControllerName = "VitalSigns Daily Tasks";
                        try{
                            lbls2.Text = VSDailyTasksController.Status.ToString();
                            lblStarted2.Text = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("Daily Tasks Start");
                        }
                        catch (Exception ex)
                        {
                            lbls2.Text = "Not Started";
                            errorDiv.Style.Value = "display: block";
                            //10/6/2014 NS modified for VSPLUS-990
                            errorDiv.InnerHtml = "The following error has occurred: " + ex.Message+
                                "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                            //6/27/2014 NS added for VSPLUS-634
                            Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                        }
                        //1/8/2014 NS commented out
                        //lblB2.Text = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("Daily Tasks Build");
                    }
                    if (VSDBHealthController != null && DominoEnabled)
                    {
                        //ControllerName = "VitalSigns Database Health";
                        try
                        {
                            lblS3.Text = VSDBHealthController.Status.ToString();
                            lblStarted3.Text = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("Database Health Start");
                        }
                        catch (Exception ex)
                        {
                            lblS3.Text = "Not Started";
                            errorDiv.Style.Value = "display: block";
                            //10/6/2014 NS modified for VSPLUS-990
                            errorDiv.InnerHtml = "The following error has occurred: " + ex.Message+
                                "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                            //6/27/2014 NS added for VSPLUS-634
                            Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                        }
                        //lblB3.Text = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("");
                    }
                    if (VSMasterSvcController != null)
                    {
                        // ControllerName = "VitalSigns Plus Master Service";
                        try
                        {
                            lblS4.Text = VSMasterSvcController.Status.ToString();
                            lblStarted4.Text = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("Master Service Start");
                        }
                        catch (Exception ex)
                        {
                            lblS4.Text = "Not Started";
                            errorDiv.Style.Value = "display: block";
                            //10/6/2014 NS modified for VSPLUS-990
                            errorDiv.InnerHtml = "The following error has occurred: " + ex.Message+
                                "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                            //6/27/2014 NS added for VSPLUS-634
                            Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                        }
                        //1/8/2014 NS commented out
                        //lblB4.Text = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("Service Build Number");
                    }
                    if (VSCoreFeaturesController != null)
                    {
                        // ControllerName = "VitalSigns Plus Core Features";
                        try
                        {
                            lblS6.Text = VSCoreFeaturesController.Status.ToString();
							lblStarted6.Text = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("Core Features Start");
                        }
                        catch (Exception ex)
                        {
                            lblS6.Text = "Not Started";
                            errorDiv.Style.Value = "display: block";
                            //10/6/2014 NS modified for VSPLUS-990
                            errorDiv.InnerHtml = "The following error has occurred: " + ex.Message+
                                "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                            //6/27/2014 NS added for VSPLUS-634
                            Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                        }
                        //1/8/2014 NS commented out
                        //lblB6.Text = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("VS Other Build Number");
                    }
					//Durga 1493
					if (VSCoreServiceController != null)
					{
						// ControllerName = "VitalSigns Plus Core Features";
						try
						{
							CoreServicesStatus.Text = VSCoreServiceController.Status.ToString();
							//CoreServicesStatus.Text = "Start";
							CoreServiceslaststart.Text = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("Core Service (64 bit) Start");//Sowjanya 1558
						}
						catch (Exception ex)
						{
							CoreServicesStatus.Text = "Not Started";
							errorDiv.Style.Value = "display: block";
							//10/6/2014 NS modified for VSPLUS-990
							errorDiv.InnerHtml = "The following error has occurred: " + ex.Message +
								"<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
							//6/27/2014 NS added for VSPLUS-634
							Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
						}
						//1/8/2014 NS commented out
						//lblB6.Text = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("VS Other Build Number");
					}
                    if (VSAlertingSvcController != null)
                    {
                        //ControllerName = "VitalSigns Alerting Service";
                        try
                        {
                            lblS7.Text = VSAlertingSvcController.Status.ToString();
                            lblStarted7.Text = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("Alert Service Start");
                        }
                        catch (Exception ex)
                        {
                            lblS7.Text = "Not Started";
                            errorDiv.Style.Value = "display: block";
                            //10/6/2014 NS modified for VSPLUS-990
                            errorDiv.InnerHtml = "The following error has occurred: " + ex.Message+
                                "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                            //6/27/2014 NS added for VSPLUS-634
                            Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                        }
                        //1/8/2014 NS commented out
                        //lblB7.Text = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("Alert Service Build");
                    }
                    if (VSConsoleCmdsController != null && DominoEnabled)
                    {
                        // ControllerName = "VitalSigns Console Commands";
                        try
                        {
                            lblS8.Text = VSConsoleCmdsController.Status.ToString();
                            lblStarted8.Text = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("Console Commands Start");
                        }
                        catch (Exception ex)
                        {
                            lblS8.Text = "Not Started";
                            errorDiv.Style.Value = "display: block";
                            //10/6/2014 NS modified for VSPLUS-990
                            errorDiv.InnerHtml = "The following error has occurred: " + ex.Message+
                                "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                            //6/27/2014 NS added for VSPLUS-634
                            Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                        }
                        //1/8/2014 NS commented out
                        //lblB8.Text = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("Console Commands Build");
                    }
                    if (VSMicrosoftController != null && MicrosoftEnabled)
                    {
                        // ControllerName = "VitalSigns Console Commands";
                        try
                        {
                            lblS9.Text = VSMicrosoftController.Status.ToString();
                            lblStarted9.Text = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("VS Microsoft Service Start");
                        }
                        catch (Exception ex)
                        {
                            lblS9.Text = "Not Started";
                            errorDiv.Style.Value = "display: block";
                            //10/6/2014 NS modified for VSPLUS-990
                            errorDiv.InnerHtml = "The following error has occurred: " + ex.Message+
                                "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                            //6/27/2014 NS added for VSPLUS-634
                            Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                        }
                        //1/8/2014 NS commented out
                        //lblB8.Text = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("Console Commands Build");
                    }
                    
                }
                
                //2/12/2013 NS modified
                //1/8/2014 NS modified
                if (VSMasterSvcController != null)
                {
                    if (VSMasterSvcController.Status == ServiceControllerStatus.Stopped)
                    {
                        //lblend1.Visible = true;
                        //Stopbtn.Enabled = false;
                        //lblf1.Visible = true;
                        //lblf1.Text = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("Service End");
                    }
                    if (VSMasterSvcController.Status == ServiceControllerStatus.Running)
                    {
                        //StartBtn.Enabled = false;
                        //lblend1.Visible = false;
                        //lblf1.Visible = false;
                    }
                }
                //2/12/2013 NS commented out
                /*
                if (svcController1.Status == ServiceControllerStatus.Stopped)
                {
                    lblend2.Visible = true;
                    lblf2.Visible = true;
                    lblf2.Text = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("Daily Tasks End");
                }
                if (svcController2.Status == ServiceControllerStatus.Stopped)
                {
                    lblend3.Visible = true;
                    lblf3.Visible = true;
                    lblf3.Text = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("Database Health End");
                }
                if (svcController3.Status == ServiceControllerStatus.Stopped)
                {
                    lblend4.Visible = true;
                    lblf4.Visible = true;
                    lblf4.Text = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("Master Service End");
                }*/

                if (lblS1.Text == "Running")
                {
                    lblS1.ForeColor = System.Drawing.Color.Green;
                }
                else
                {
                    lblS1.ForeColor = System.Drawing.Color.Red;
                }
                if (lbls2.Text == "Running")
                {
                    lbls2.ForeColor = System.Drawing.Color.Green;
                }
                else
                {
                    lbls2.ForeColor = System.Drawing.Color.Black;
                }
                if (lblS3.Text == "Running")
                {
                    lblS3.ForeColor = System.Drawing.Color.Green;
                }
                else
                {
                    lblS3.ForeColor = System.Drawing.Color.Black;
                }
                if (lblS4.Text == "Running")
                {
                    lblS4.ForeColor = System.Drawing.Color.Green;
                }
                else
                {
                    lblS4.ForeColor = System.Drawing.Color.Red;
                }
                //2/12/2013 NS added
                if (lblS6.Text == "Running")
                {
                    lblS6.ForeColor = System.Drawing.Color.Green;
                }
                else
                {
                    lblS6.ForeColor = System.Drawing.Color.Red;
                }
				if (CoreServicesStatus.Text == "Running")
				{
					CoreServicesStatus.ForeColor = System.Drawing.Color.Green;
				}
				else
				{
					CoreServicesStatus.ForeColor = System.Drawing.Color.Red;
				}
                if (lblS7.Text == "Running")
                {
                    lblS7.ForeColor = System.Drawing.Color.Green;
                }
                else
                {
                    lblS7.ForeColor = System.Drawing.Color.Red;
                }
                if (lblS8.Text == "Running")
                {
                    lblS8.ForeColor = System.Drawing.Color.Green;
                }
                else
                {
                    lblS8.ForeColor = System.Drawing.Color.Red;
                }
                if (lblS9.Text == "Running")
                {
                    lblS9.ForeColor = System.Drawing.Color.Green;
                }
                else
                {
                    lblS9.ForeColor = System.Drawing.Color.Red;
                }
                if (lblS10.Text == "Running")
                {
                    lblS10.ForeColor = System.Drawing.Color.Green;
                }
                else
                {
                    lblS10.ForeColor = System.Drawing.Color.Red;
                }
            }
            catch (Exception ex)
            {
                errorDiv.Style.Value = "display: block";
                //10/6/2014 NS modified for VSPLUS-990
                errorDiv.InnerHtml = "The following error has occurred: " + ex.Message+
                    "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
            }
        }

        protected void StartBtn_Click(object sender, EventArgs e)
        {
            //7/8/2013 NS modified to handle an exception if the service is null
            if (VSMasterSvcController.Container != null)
            {
                if (VSMasterSvcController.Status == ServiceControllerStatus.Stopped)
                {
                    VSMasterSvcController.Start();
                    //StartBtn.Enabled = false;
                    //Stopbtn.Enabled = true;
                    lblS4.Text = ServiceControllerStatus.Running.ToString();
                    lblS4.ForeColor = System.Drawing.Color.Green;
                    Startime4 = DateTime.Now.ToString();
                    lblStarted4.Text = DateTime.Now.ToString();
                    //1/8/2014 NS commented out
                    //lblend4.Visible = false;
                    //lblf4.Visible = false;
                    VSWebBL.SettingBL.SettingsBL.Ins.UpdateSvalue("Master Service Start", Startime4, VSWeb.Constants.Constants.SysString);
                }
            }
            
            /*
            if (svcController.Status == ServiceControllerStatus.Stopped)
            {
                svcController.Start();
                lblS1.Text = ServiceControllerStatus.Running.ToString();
                lblS1.ForeColor = System.Drawing.Color.Green;
                Startime1 = DateTime.Now.ToString();
                lblStarted1.Text = DateTime.Now.ToString();
                lblend1.Visible = false;
                lblf1.Visible = false;
                VSWebBL.SettingBL.SettingsBL.Ins.UpdateSvalue("VS Domino Service Start", Startime1);
            }
            if (svcController1.Status == ServiceControllerStatus.Stopped)
            {
                svcController1.Start();
                lbls2.Text = ServiceControllerStatus.Running.ToString();
                lbls2.ForeColor = System.Drawing.Color.Green;
                Startime2 = DateTime.Now.ToString();
                lblStarted2.Text = DateTime.Now.ToString();
                lblend2.Visible = false;
                lblf2.Visible = false;
                VSWebBL.SettingBL.SettingsBL.Ins.UpdateSvalue("Daily Tasks Start", Startime2);
            }
            if (svcController2.Status == ServiceControllerStatus.Stopped)
            {
                svcController2.Start();
                lblS3.Text = ServiceControllerStatus.Running.ToString();
                lblS3.ForeColor = System.Drawing.Color.Green;
                Startime3 = DateTime.Now.ToString();
                lblStarted3.Text = DateTime.Now.ToString();
                lblend3.Visible = false;
                lblf3.Visible = false;
                VSWebBL.SettingBL.SettingsBL.Ins.UpdateSvalue("Database Health Start", Startime3);
            }
            //2/12/2013 NS added
            if (svcController4.Status == ServiceControllerStatus.Stopped)
            {
                svcController4.Start();
                lblS6.Text = ServiceControllerStatus.Running.ToString();
                lblS6.ForeColor = System.Drawing.Color.Green;
                Startime5 = DateTime.Now.ToString();
                lblStarted6.Text = DateTime.Now.ToString();
                VSWebBL.SettingBL.SettingsBL.Ins.UpdateSvalue("VS Other Start", Startime5);
            }
            if (svcController5.Status == ServiceControllerStatus.Stopped)
            {
                svcController5.Start();
                lblS8.Text = ServiceControllerStatus.Running.ToString();
                lblS8.ForeColor = System.Drawing.Color.Green;
                Startime6 = DateTime.Now.ToString();
                lblStarted8.Text = DateTime.Now.ToString();
                VSWebBL.SettingBL.SettingsBL.Ins.UpdateSvalue("", Startime6);
            }
            if (svcController6.Status == ServiceControllerStatus.Stopped)
            {
                svcController6.Start();
                lblS7.Text = ServiceControllerStatus.Running.ToString();
                lblS7.ForeColor = System.Drawing.Color.Green;
                Startime7 = DateTime.Now.ToString();
                lblStarted4.Text = DateTime.Now.ToString();
                lblend4.Visible = false;
                lblf4.Visible = false;
                VSWebBL.SettingBL.SettingsBL.Ins.UpdateSvalue("", Startime7);
            }*/
        }

        protected void Stopbtn_Click(object sender, EventArgs e)
        {
            //10/9/2013 NS added
            try
            {
                //7/8/2013 NS modified to handle an exception if the service is null
                if (VSMasterSvcController.Container != null)
                {

                    if (VSMasterSvcController.Status == ServiceControllerStatus.Running)
                    {
                        VSMasterSvcController.Stop();
                        //Stopbtn.Enabled = false;
                        //StartBtn.Enabled = true;
                        lblS4.Text = ServiceControllerStatus.StopPending.ToString();
                        lblS4.ForeColor = System.Drawing.Color.Red;
                        Endtime4 = DateTime.Now.ToString();
                        //1/8/2014 NS commented out
                        //lblend4.Visible = true;
                        //lblf4.Visible = true;
                        //lblf4.Text = DateTime.Now.ToString();
                        VSWebBL.SettingBL.SettingsBL.Ins.UpdateSvalue("Master Service End", Endtime4, VSWeb.Constants.Constants.SysString);
                    }
                }
            }
            catch (Exception ex)
            {
                using (System.IO.StreamWriter _testData = new System.IO.StreamWriter(System.Web.HttpContext.Current.Server.MapPath("~/SvcControllerLog.txt"), true))
                {
                    _testData.WriteLine(ex.Message); // Write the file.
                }
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
            }
            /*
            if (svcController.Status == ServiceControllerStatus.Running)
            {
                svcController.Stop();
                lblS1.Text = ServiceControllerStatus.StopPending.ToString();
                lblS1.ForeColor = System.Drawing.Color.Red;
                Endtime1 = DateTime.Now.ToString();
                lblend1.Visible = true;
                lblf1.Visible = true;
                lblf1.Text = DateTime.Now.ToString();
                VSWebBL.SettingBL.SettingsBL.Ins.UpdateSvalue("Service End", Endtime1);
            }
            if (svcController1.Status == ServiceControllerStatus.Running)
            {
                svcController1.Stop();
                lbls2.Text = ServiceControllerStatus.StopPending.ToString();
                //2/13/2013 NS modified
                //lbls2.ForeColor = System.Drawing.Color.Red;
                lbls2.ForeColor = System.Drawing.Color.Black;
                Endtime2 = DateTime.Now.ToString();
                lblend2.Visible = true;
                lblf2.Visible = true;
                lblf2.Text = DateTime.Now.ToString();
                VSWebBL.SettingBL.SettingsBL.Ins.UpdateSvalue("Daily Tasks End", Endtime2);
            }
            if (svcController2.Status == ServiceControllerStatus.Running)
            {
                svcController2.Stop();
                lblS3.Text = ServiceControllerStatus.StopPending.ToString();
                //2/13/2013 NS modified
                //lblS3.ForeColor = System.Drawing.Color.Red;
                lblS3.ForeColor = System.Drawing.Color.Black;
                Endtime3 = DateTime.Now.ToString();
                lblend3.Visible = true;
                lblf3.Visible = true;
                lblf3.Text = DateTime.Now.ToString();
                VSWebBL.SettingBL.SettingsBL.Ins.UpdateSvalue("Database Health End", Endtime3);
            }
            //2/12/2013 NS added
            if (svcController4.Status == ServiceControllerStatus.Running)
            {
                svcController4.Stop();
                lblS6.Text = ServiceControllerStatus.StopPending.ToString();
                lblS6.ForeColor = System.Drawing.Color.Red;
                Endtime5 = DateTime.Now.ToString();
                VSWebBL.SettingBL.SettingsBL.Ins.UpdateSvalue("", Endtime5);
            }
            if (svcController5.Status == ServiceControllerStatus.Running)
            {
                svcController5.Stop();
                lblS8.Text = ServiceControllerStatus.StopPending.ToString();
                lblS8.ForeColor = System.Drawing.Color.Red;
                Endtime6 = DateTime.Now.ToString();
                VSWebBL.SettingBL.SettingsBL.Ins.UpdateSvalue("", Endtime6);
            }
            if (svcController6.Status == ServiceControllerStatus.Running)
            {
                svcController6.Stop();
                lblS7.Text = ServiceControllerStatus.StopPending.ToString();
                lblS7.ForeColor = System.Drawing.Color.Red;
                Endtime7 = DateTime.Now.ToString();
                VSWebBL.SettingBL.SettingsBL.Ins.UpdateSvalue("", Endtime7);
            }*/
        }
    }
}