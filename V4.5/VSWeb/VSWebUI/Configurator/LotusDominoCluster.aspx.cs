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



namespace VSWeb
{
    public partial class LotusDominoCluster : System.Web.UI.Page
    {
        int ServerKey;
        string Mode;
        bool flag=false;
		
        protected void Page_Load(object sender, EventArgs e)
        {
            //Page.Title = "Lotus Domino Cluster";
            if (Request.QueryString["tab"] != null)
                ASPxPageControl1.ActiveTabIndex = 2;
            if (!IsPostBack)
            {
                
                ServerAComboBox.Items.Clear();
                ServerBComboBox3.Items.Clear();
                ServerCComboBox.Items.Clear();
                //FillClusterCategoryComboBox();
                FillClusterServerAComboBox();
                FillClusterServerBComboBox();
                FillClusterServerCComboBox();
                if (Session["UserPreferences"] != null)
                {
                    DataTable UserPreferences = (DataTable)Session["UserPreferences"];
                    foreach (DataRow dr in UserPreferences.Rows)
                    {
                        if (dr[1].ToString() == "LotusDominoCluster|MaintWinListGridView")
                        {
                            MaintWinListGridView.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                        }
                        if (dr[1].ToString() == "LotusDominoCluster|AlertGridView")
                        {
                            AlertGridView.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                        }
                    }
                }
                
            }
            
            try
            {
                if (Request.QueryString["ID"] != null && Request.QueryString["ID"] != "")
                {
                    Mode = "Update";
					
                    ServerKey = int.Parse(Request.QueryString["ID"]);
                    
                    //For Validation Summary
                    ////ApplyValidationSummarySettings();
                    ////ApplyEditorsSettings();
                    if (!IsPostBack)
                    {
                        //Fill Server Attributes Tab & Advanced Tab
                        FillData(ServerKey);
                        FillMaintenanceGrid();
                        FillAlertGridView();
                        //11/19/2014 NS modified
                        //DominoClusterRoundPanel.HeaderText = "Domino Cluster - " + ClsAttNameTextBox.Text;
                        servernamelbldisp.InnerHtml = "Domino Cluster - " + ClsAttNameTextBox.Text;
                    }
                    else
                    {
                        FillMaintServersGridfromSession();
                        FillAlertGridViewfromSession();
                    }


                }
                else
                {
					errorDiv.Style.Value = "display: none;";
                    Mode = "Insert";
                    if (!IsPostBack)
                    {
                       
                        OHScanTextBox.Text = "120";
                        ScanIntervalTextBox.Text = "60";
                        ClusterCategoryComboBox.Text = "Mail";
                        scanEnableCheckBox.Checked = true;
						//ServerAComboBox.Text = "None";
						//ServerATextBox.Enabled = false;
						//ServerBComboBox3.Text = "None";
						//ServerBTextBox.Enabled = false;
						ServerCComboBox.Text = "None";
                        if (ServerCComboBox.Text == "None")
                        {
                            ServerCTextBox.Enabled = false;

                        }
                        else
                        {
                            ServerCTextBox.Enabled = true;
                        }
						
                        ClsAttNameTextBox.Focus();
                        
                    }
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

                DominoCluster DominoClusterObject = new DominoCluster();
                DominoCluster ReturnDCObject = new DominoCluster();
                DominoClusterObject.ID = ID;
                ReturnDCObject = VSWebBL.ConfiguratorBL.DominoClusterBL.Ins.GetData(DominoClusterObject);       
                //Cluster Setting fields
                ClsAttNameTextBox.Text = ReturnDCObject.Name.ToString();
                ClusterCategoryComboBox.Text = ReturnDCObject.Category.ToString();
                ScanIntervalTextBox.Text = ReturnDCObject.ScanInterval.ToString();
                OHScanTextBox.Text = ReturnDCObject.OffHoursScanInterval.ToString();
                scanEnableCheckBox.Checked = ReturnDCObject.Enabled;//(ScanTextBox.Text != null ? true : false);
                AlertTextBox.Text = ReturnDCObject.First_Alert_Threshold.ToString();
                //AlertCheckBox.Checked = ReturnDCObject.Enabled;
                //Servers fields
                //12/13/2012 NS modified - Server fields should display server name, not ID
                //ServerAComboBox.Text = ReturnDCObject.ServerID_A.ToString();
                ServerAComboBox.Text = ReturnDCObject.ServerAName;
                ServerAComboBox.Value = ReturnDCObject.ServerID_A.ToString();
                ServerATextBox.Text = ReturnDCObject.Server_A_Directory;
                //ServerBComboBox3.Text = ReturnDCObject.ServerID_B.ToString();
                ServerBComboBox3.Text = ReturnDCObject.ServerBName;
                ServerBComboBox3.Value = ReturnDCObject.ServerID_B.ToString();
                ServerBTextBox.Text = ReturnDCObject.Server_B_Directory;
               
                if (ReturnDCObject.ServerID_C.ToString() != "0")
                {
                    //ServerCComboBox.Text = ReturnDCObject.ServerID_C.ToString();
                    ServerCComboBox.Text = ReturnDCObject.ServerCName;
                    ServerCComboBox.Value = ReturnDCObject.ServerID_C.ToString();
                }
                else
                {
                    ServerCComboBox.Text = "None";
                }
                if (ServerCComboBox.Text == "" || ServerCComboBox.Text == "None")
                {
                    ServerCTextBox.Enabled = false;

                }
                else
                {
                    ServerCTextBox.Enabled = true;
                }
                
                ServerCTextBox.Text = ReturnDCObject.Server_C_Directory;
                //4/20/2016 NS added for VSPLUS-2724
                ExcludeATextBox.Text = ReturnDCObject.Server_A_ExcludeList;
                ExcludeBTextBox.Text = ReturnDCObject.Server_B_ExcludeList;
                ExcludeCTextBox.Text = ReturnDCObject.Server_C_ExcludeList;
                Session["ReturnURL"] = "LotusDominoCluster.aspx?ID=" + ID+"&tab=2";


				//if (ReturnDCObject.ServerID_B.ToString() != "0")
				//{
				//    //ServerCComboBox.Text = ReturnDCObject.ServerID_C.ToString();
				//     ServerBComboBox3.Text = ReturnDCObject.ServerBName;
				//ServerBComboBox3.Value = ReturnDCObject.ServerID_B.ToString();
				//}
				//else
				//{
				//    ServerBComboBox3.Text = "None";
				//}
				//ServerBComboBox3.Text =  ReturnDCObject.Server_B_Directory;
				//Session["ReturnURL"] = "LotusDominoCluster.aspx?ID=" + ID+"&tab=2";
                
			}
            
			// 

            catch (Exception ex)
            {
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex; 
            }
            finally { }
        }

        private DominoCluster CollectDataForDominoCluster()
        {
            try
            {
               // SetFocusOnControl();
                //Cluster Settings
                DominoCluster DominoClusterObject = new DominoCluster();
                DominoClusterObject.Name = ClsAttNameTextBox.Text;
                DominoClusterObject.Enabled = scanEnableCheckBox.Checked;
                DominoClusterObject.Category = ClusterCategoryComboBox.Text;
                if(ScanIntervalTextBox.Text!=""&&ScanIntervalTextBox.Text!=null)
                DominoClusterObject.ScanInterval = int.Parse(ScanIntervalTextBox.Text);
                if(OHScanTextBox.Text!=""&&OHScanTextBox.Text!=null)
                DominoClusterObject.OffHoursScanInterval = int.Parse(OHScanTextBox.Text);
                if(AlertTextBox.Text!="" && AlertTextBox.Text!=null)
                DominoClusterObject.First_Alert_Threshold = float.Parse(AlertTextBox.Text);
                //DominoClusterObject.Missing_Replica_Alert = AlertCheckBox.Checked;
                //servers Fields
                //12/13/2012 NS modified
                if(ServerAComboBox.Text!=""&&ServerAComboBox.Text!=null)
                DominoClusterObject.ServerID_A=int.Parse(ServerAComboBox.Value.ToString());
                DominoClusterObject.Server_A_Directory = ServerATextBox.Text;
                DominoClusterObject.ServerAName = ServerAComboBox.Text.ToString();
                if(ServerBComboBox3.Text!=""&&ServerBComboBox3.Text!=null)
                DominoClusterObject.ServerID_B= int.Parse(ServerBComboBox3.Value.ToString());
                DominoClusterObject.Server_B_Directory = ServerBTextBox.Text;
                DominoClusterObject.ServerBName = ServerBComboBox3.Text.ToString();
				if (ServerCComboBox.Value != null)
				{
					if (ServerCComboBox.Value.ToString() != "None" && ServerCComboBox.Value.ToString() != "" && ServerCComboBox.Value.ToString() != null)
					{

						DominoClusterObject.ServerID_C = int.Parse(ServerCComboBox.Value.ToString());
						DominoClusterObject.ServerCName = ServerCComboBox.Text.ToString();
					}
				}
                DominoClusterObject.Server_C_Directory = ServerCTextBox.Text;
				//4/20/2016 NS added for VSPLUS-2724
                DominoClusterObject.Server_A_ExcludeList = ExcludeATextBox.Text;
                DominoClusterObject.Server_B_ExcludeList = ExcludeBTextBox.Text;
                DominoClusterObject.Server_C_ExcludeList = ExcludeCTextBox.Text;

                if(Mode=="Update")
                DominoClusterObject.ID = ServerKey;
                return DominoClusterObject;
            }
            catch (Exception ex)
            {
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex; 
            }
            finally { }
        }

        private void UpdateDominoClusterData()
        {

			DominoCluster ClsObj = new DominoCluster();
			ClsObj.Name = ClsAttNameTextBox.Text;
			ClsObj.ID = ServerKey;
			DataTable returntable = VSWebBL.ConfiguratorBL.DominoClusterBL.Ins.GetIPAddress(ClsObj);

			if (returntable.Rows.Count > 0)
			{
				//4/29/2015 NS modified
				/*
				ErrorMessageLabel.Text = "This cluster name is already in use.  Please enter a different server name.";
				ErrorMessagePopupControl.ShowOnPageLoad = true;
				 */
				errorDiv.InnerHtml = "This cluster name is already in use.  Please enter a different cluster name." +
					"<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
				errorDiv.Style.Value = "display: block";
				flag = true;
				ClsAttNameTextBox.Focus();
			}
			else
			{
				try
				{
					Object ReturnValue = VSWebBL.ConfiguratorBL.DominoClusterBL.Ins.UpdateData(CollectDataForDominoCluster());
					//SetFocusOnError(ReturnValue);

					if (ReturnValue.ToString().Equals("True"))
					{
						//1/21/2014 NS modified
						/*
						ErrorMessageLabel.Text = "Cluster record updated successfully.";
						ErrorMessagePopupControl.HeaderText = "Information";
						ErrorMessagePopupControl.ShowCloseButton = false;
						ValidationUpdatedButton.Visible = true;
						ValidationOkButton.Visible = false;
						ErrorMessagePopupControl.ShowOnPageLoad = true;
						*/
						Session["ClusterUpdateStatus"] = ClsAttNameTextBox.Text;
						Response.Redirect("ClusterGrid.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
						Context.ApplicationInstance.CompleteRequest();
					}
					//9/3/2015 NS modified for VSPLUS-2116
					else if (ReturnValue.ToString().Equals("false"))
					{
						errorDiv.InnerHtml = "The following error has occurred: could not insert a record into the SQL table." +
						"<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
						errorDiv.Style.Value = "display: block";
						//6/27/2014 NS added for VSPLUS-634
						Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Error while trying to insert to SQL.");
					}
					else if (!string.IsNullOrEmpty(ReturnValue.ToString()))
					{

						errorDiv.InnerHtml = ReturnValue.ToString() +
							"<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
						errorDiv.Style.Value = "display: block";

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
        }

        private void InsertDominoCluster()
        {

            DominoCluster ClsObj = new DominoCluster();
            ClsObj.Name = ClsAttNameTextBox.Text;
            DataTable returntable = VSWebBL.ConfiguratorBL.DominoClusterBL.Ins.GetIPAddress(ClsObj);

            if (returntable.Rows.Count > 0)
            {
                //4/29/2015 NS modified
                /*
                ErrorMessageLabel.Text = "This cluster name is already in use.  Please enter a different server name.";
                ErrorMessagePopupControl.ShowOnPageLoad = true;
                 */
				errorDiv.InnerHtml = "This cluster name is already in use.  Please enter a different cluster name." +
					"<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
				errorDiv.Style.Value = "display: block";
                flag = true;
                ClsAttNameTextBox.Focus();
            }
            else
            {
				try
				{
					errorDiv.Style.Value = "display: none";
					object ReturnValue = VSWebBL.ConfiguratorBL.DominoClusterBL.Ins.InsertData(CollectDataForDominoCluster());
					//SetFocusOnError(ReturnValue);
					
					

						if (ReturnValue.ToString().Equals("True"))
						{
							//4/29/2015 NS modified
							/*
							ErrorMessageLabel.Text = "Cluster record created successfully.";
							ErrorMessagePopupControl.HeaderText = "Information";
							ErrorMessagePopupControl.ShowCloseButton = false;
							ValidationUpdatedButton.Visible = true;
							ValidationOkButton.Visible = false;
							ErrorMessagePopupControl.ShowOnPageLoad = true;
							*/
							Session["ClusterUpdateStatus"] = ClsAttNameTextBox.Text;
							Response.Redirect("ClusterGrid.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
							Context.ApplicationInstance.CompleteRequest();
						}
						else if (ReturnValue.ToString().Equals("false"))
						{
							flag = true;
							//9/3/2015 NS modified for VSPLUS-2116
							errorDiv.InnerHtml = "The following error has occurred: could not insert a record into the SQL table." +
								"<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
							errorDiv.Style.Value = "display: block";
							//6/27/2014 NS added for VSPLUS-634
							Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Error while trying to insert to SQL.");
						}
						else if (!string.IsNullOrEmpty(ReturnValue.ToString()))
						{
							flag = true;
							errorDiv.InnerHtml =  ReturnValue.ToString() +
								"<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
							errorDiv.Style.Value = "display: block";
						}
				}
				catch (Exception ex)
				{
					//4/29/2015 NS modified for VSPLUS-990
					errorDiv.InnerHtml = "The following error has occurred: " + ex.Message +
						"<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
					errorDiv.Style.Value = "display: block";
					//6/27/2014 NS added for VSPLUS-634
					Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
					throw ex;
				}
                finally { }
            }
        }

        protected void formOKButton_Click(object sender, EventArgs e)
        {
            //Write to Registry
            if (ServerCComboBox.Text == "None")
            {
                ServerCComboBox.Text = "";
                ServerCTextBox.Text = "";
            }
            //else
            //{
				VSWebBL.SettingBL.SettingsBL.Ins.UpdateSvalue("Domino Cluster Update", "True", VSWeb.Constants.Constants.SysString);

				try
				{
					if (Mode == "Update")
					{

						UpdateDominoClusterData();
					}
					if (Mode == "Insert")
					{
						InsertDominoCluster();
						if (flag == false)
							InsertStatus();

					}
				}
				catch (Exception ex)
				{
					//6/27/2014 NS added for VSPLUS-634
					Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
					throw ex;
				}
				finally { }
            //}
        }
         private void SetFocusOnError(Object ReturnValue)
        {
            string ErrorMessage = ReturnValue.ToString();
            if (ErrorMessage.Substring(0, 2) == "ER")
            {
                ErrorMessageLabel.Text = ErrorMessage.Substring(3);
                ErrorMessagePopupControl.ShowOnPageLoad = true;
                flag = true;
            }
        }

         //private void SetFocusOnControl()
         //{

         //    try
         //    {
         //        int.Parse(OHScanTextBox.Text);
         //    }
         //    catch
         //    {

         //        OHScanTextBox.Focus();
         //    }

         //}

         protected void FormCancelButton_Click(object sender, EventArgs e)
         {
             Response.Redirect("ClusterGrid.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
             Context.ApplicationInstance.CompleteRequest();
         }

         protected void ValidationUpdatedButton_Click(object sender, EventArgs e)
         {
             Response.Redirect("ClusterGrid.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
             Context.ApplicationInstance.CompleteRequest();
         }

         //protected void ClusterCategoryComboBox_Callback(object sender, CallbackEventArgsBase e)
         //{
         //    FillClusterCategoryComboBox(sender as ASPxComboBox);
         //}
         //private void FillClusterCategoryComboBox()
         //{
         //    DataTable DCTasksDataTable = VSWebBL.ConfiguratorBL.DominoClusterBL.Ins.GetAllData();
         //    ClusterCategoryComboBox.DataSource = DCTasksDataTable;
         //    ClusterCategoryComboBox.TextField = "Category";
         //    ClusterCategoryComboBox.ValueField = "Category";
         //    ClusterCategoryComboBox.DataBind();
         //}



         private void FillClusterServerAComboBox()
         {
             DataTable DCTasksDataTable = RestrServers();
             ServerAComboBox.DataSource = DCTasksDataTable;
             ServerAComboBox.TextField = "ServerName";
             ServerAComboBox.ValueField = "ID";
             ServerAComboBox.DataBind();
         }
         private void FillClusterServerBComboBox()
         {
             DataTable DCTasksDataTable = RestrServers();
             ServerBComboBox3.DataSource = DCTasksDataTable;
             ServerBComboBox3.TextField = "ServerName";
             ServerBComboBox3.ValueField = "ID";
             ServerBComboBox3.DataBind();
         }

         private void FillClusterServerCComboBox()
         {
             DataTable DCTasksDataTable = RestrServers();
			            
             ServerCComboBox.DataSource = DCTasksDataTable;
             ServerCComboBox.TextField = "ServerName";
             ServerCComboBox.ValueField = "ID";
			
             ServerCComboBox.DataBind();
			 ServerCComboBox.Items.Insert(0, new DevExpress.Web.ListEditItem("None", "0"));
			 //ServerCComboBox.Items.Insert(new ListItem(0,"None"));
			// ServerCComboBox.SetText('Text #0');
		            
         }

         public DataTable RestrServers()
         {
             DataTable DCTasksDataTable = VSWebBL.ConfiguratorBL.DominoClusterBL.Ins.GetServer();
             //12/14/2012 NS commented out the section below per discussion with Alan. 
             //The issue at hand is that if a user
             //attempts to modify a cluster document which has serves that are restricted to the user,
             //the values in the Server A,B,C drop down lists are displayed as server IDs instead of names.
             /*
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
              */
             return DCTasksDataTable;
         }

         protected void ServerCComboBox_SelectedIndexChanged(object sender, EventArgs e)
         {
			 errorDiv.Style.Value = "display: none;";

             if (ServerCComboBox.Text == "None")
             {
                 ServerCTextBox.Enabled = false;

             }
             else
             {
                 ServerCTextBox.Enabled = true;
             }
         }
		 //protected void ServerBComboBox3_SelectedIndexChanged(object sender, EventArgs e)
		 //{
		 //    if (ServerBComboBox3.Text == "None")
		 //    {
		 //        ServerBTextBox.Enabled = false;

		 //    }
		 //    else
		 //    {
		 //        ServerBTextBox.Enabled = true;
		 //    }
		 //}
		 //protected void ServerAComboBox_SelectedIndexChanged(object sender, EventArgs e)
		 //{

		 //    //if (ServerAComboBox.SelectedIndex == 1)
		 //    //{

		 //    //    ServerAComboBox.IsEnabled = true;
		 //    //}
		 //    //else {
		 //    //    ServerAComboBox.EnableClientSideAPI = false;
		 //    //}
		 //    if (ServerAComboBox.Text.ToString() == "None")
		 //    {
		 //        ServerATextBox.Enabled = false;

		 //    }
		 //    else
		 //    {
		 //        ServerATextBox.Enabled = true;
		 //    }
		 //}

         private Status CollectDataforStatus()
         {
             Status St = new Status();
             try
             {
                 //5/5/2016 NS modified
                 St.Category = ClusterCategoryComboBox.Text;
                 St.DeadMail = 0;
                 St.Description = "A cluster of Domino servers configured for failover.";
                 St.Details = "";
                 St.DownCount = 0;
                 St.Location = "";
                 St.Name = ClsAttNameTextBox.Text;
                 St.MailDetails = "Mail Details";
                 St.PendingMail = 0;
                 St.sStatus = "Not Scanned";
                 St.Type = "Domino Cluster database";
                 St.Upcount = 0;
                 St.UpPercent = 1;
                 St.LastUpdate = System.DateTime.Now;
                 St.ResponseTime = 0;
                 St.TypeANDName = ClsAttNameTextBox.Text + "-Domino Cluster database";
                 St.Icon = 0;
                 // St.NextScan = System.DateTime.Now;

                 return St;
             }
             catch (Exception ex)
             {
                 //6/27/2014 NS added for VSPLUS-634
                 Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                 throw ex;
             }


         }


         private void InsertStatus()
         {
			 errorDiv.Style.Value = "display: none";
			 DominoCluster ClsObj = new DominoCluster();
			 ClsObj.Name = ClsAttNameTextBox.Text;
			 DataTable returntable = VSWebBL.ConfiguratorBL.DominoClusterBL.Ins.GetNameforStatus(ClsObj);

			 if (returntable.Rows.Count > 0)
			 {
				 //4/29/2015 NS modified
				 /*
				 ErrorMessageLabel.Text = "This cluster name is already in use.  Please enter a different server name.";
				 ErrorMessagePopupControl.ShowOnPageLoad = true;
				  */
				 errorDiv.InnerHtml = "This cluster name is already in use.  Please enter a different cluster name." +
					 "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
				 errorDiv.Style.Value = "display: block";
				 flag = true;
				 ClsAttNameTextBox.Focus();
			 }
			 else
			 {
				 try
				 {
					 object ReturnValue = VSWebBL.StatusBL.StatusTBL.Ins.InsertData(CollectDataforStatus());
					 // SetFocusOnError(ReturnValue);
					 if (ReturnValue.ToString().Equals("True"))
					 {

						 Session["Status"] = ClsAttNameTextBox.Text;
						 Response.Redirect("ClusterGrid.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
						 Context.ApplicationInstance.CompleteRequest();
						 //ErrorMessageLabel.Text = "Cluster record created successfully.";
						 //ErrorMessagePopupControl.HeaderText = "Information";
						 //ErrorMessagePopupControl.ShowCloseButton = false;
						 //ValidationUpdatedButton.Visible = true;
						 //ValidationOkButton.Visible = false;
						 //ErrorMessagePopupControl.ShowOnPageLoad = true;

					 }
					 else if (ReturnValue.ToString().Equals("false"))
					 {
						 errorDiv.InnerHtml = "The following error has occurred: could not insert a record into the SQL table." +
								  "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
						 errorDiv.Style.Value = "display: block";
						 //6/27/2014 NS added for VSPLUS-634
						 Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Error while trying to insert to SQL.");

					 }

				 }
				 catch (Exception ex)
				 {
					 ErrorMessageLabel.Text = "Error attempting to update the status table :" + ex.Message;
					 ErrorMessagePopupControl.ShowOnPageLoad = true;
					 //6/27/2014 NS added for VSPLUS-634
					 Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				 }
				 finally { }
			 }
         }
        
       //june 27

         private void FillMaintenanceGrid()
         {
             try
             {

                 DataTable MaintDataTable = new DataTable();
                 DataSet ServersDataSet = new DataSet();
                 MaintDataTable = VSWebBL.ConfiguratorBL.MaintenanceBL.Ins.GetMaintenDataOnServerIDs(ServerAComboBox.Text,ServerBComboBox3.Text,ServerCComboBox.Text);
                 if (MaintDataTable.Rows.Count > 0)
                 {
                     DataTable dtcopy = MaintDataTable.Copy();
                     dtcopy.PrimaryKey = new DataColumn[] { dtcopy.Columns["ID"] };
                     //4/3/2014 NS modified for VSPLUS-138
                     //Session["MaintServers"] = dtcopy;
                     ViewState["MaintServers"] = dtcopy;
                     MaintWinListGridView.DataSource = MaintDataTable;
                     MaintWinListGridView.DataBind();
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

         private void FillMaintServersGridfromSession()
         {
             try
             {

                 DataTable ServersDataTable = new DataTable();
                 //4/3/2014 NS modified for VSPLUS-138
                 //if(Session["MaintServers"]!=""&&Session["MaintServers"]!=null)
                 //    ServersDataTable = (DataTable)Session["MaintServers"];
                 if (ViewState["MaintServers"] != "" && ViewState["MaintServers"] != null)
                     ServersDataTable = (DataTable)ViewState["MaintServers"];
                 if (ServersDataTable.Rows.Count > 0)
                 {
                     MaintWinListGridView.DataSource = ServersDataTable;
                     MaintWinListGridView.DataBind();
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

         protected void MaintWinListGridView_SelectionChanged(object sender, EventArgs e)
         {
             if (MaintWinListGridView.Selection.Count > 0)
             {
                 System.Collections.Generic.List<object> Type = MaintWinListGridView.GetSelectedFieldValues("ID");

                 if (Type.Count > 0)
                 {
                     string ID = Type[0].ToString();

                     //Mukund: VSPLUS-844, Page redirect on callback
                     try
                     {
                         DevExpress.Web.ASPxWebControl.RedirectOnCallback("MaintenanceWin.aspx?ID=" + ID + "");
                         Context.ApplicationInstance.CompleteRequest();//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
                     }
                     catch (Exception ex)
                     {
                         Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                         //throw ex;
                     }
                 }

             }
         }

         protected void MaintWinListGridView_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
         {
             e.Row.Attributes.Add("onmouseover", "this.style.backgroundColor='#C0C0C0';");

             e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='white';");

         }

        ///=====================Alerts
        ///

         private void FillAlertGridView()
         {
             try
             {

                 DataTable AlertDataTable = new DataTable();
                 DataSet AlertDataSet = new DataSet();
                 AlertDataTable = VSWebBL.ConfiguratorBL.AlertsBL.Ins.GetAlertTabbyDiffServers(ServerAComboBox.Text, ServerBComboBox3.Text, ServerCComboBox.Text, "Domino");
                 if (AlertDataTable.Rows.Count > 0)
                 {
                     DataTable dtcopy = AlertDataTable.Copy();
                     dtcopy.PrimaryKey = new DataColumn[] { dtcopy.Columns["ID"] };
                     //4/3/2014 NS modified for VSPLUS-138
                     //Session["AlertDataTable"] = dtcopy;
                     ViewState["AlertDataTable"] = dtcopy;
                     AlertGridView.DataSource = AlertDataTable;
                     AlertGridView.DataBind();
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
         private void FillAlertGridViewfromSession()
         {
             try
             {
                 DataTable AlertDataTable = new DataTable();
                 //4/3/2014 NS modified for VSPLUS-138
                 //if (Session["AlertDataTable"] != "" && Session["AlertDataTable"] != null)
                 //    AlertDataTable = (DataTable)Session["AlertDataTable"];//VSWebBL.ConfiguratorBL.DominoPropertiesBL.Ins.GetAllData();
                 if (ViewState["AlertDataTable"] != "" && ViewState["AlertDataTable"] != null)
                     AlertDataTable = (DataTable)ViewState["AlertDataTable"];//VSWebBL.ConfiguratorBL.DominoPropertiesBL.Ins.GetAllData();
                 if (AlertDataTable.Rows.Count > 0)
                 {
                     AlertGridView.DataSource = AlertDataTable;
                     AlertGridView.DataBind();
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

         protected void MaintWinListGridView_PageSizeChanged(object sender, EventArgs e)
         {
             //ProfilesGridView.PageIndex;
             VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("LotusDominoCluster|MaintWinListGridView", MaintWinListGridView.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
             Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
         }
         protected void AlertGridView_PageSizeChanged(object sender, EventArgs e)
         {
             //ProfilesGridView.PageIndex;
             VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("LotusDominoCluster|AlertGridView", AlertGridView.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
             Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
         }
		 //protected void ResetButtonA_Click(object sender, EventArgs e)
		 //{
		 //    ServerAComboBox.SelectedIndex = -1; 
			  
		 //}
		 //protected void ResetButtonB_Click(object sender, EventArgs e)
		 //{
		 //    ServerBComboBox3.SelectedIndex = -1;

		 //}
		 //protected void ResetButtonC_Click(object sender, EventArgs e)
		 //{
		 //    ServerCComboBox.SelectedIndex = -1;

		 //}
      }
    }



