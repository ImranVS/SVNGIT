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
    public partial class NetworkLatencyedit : System.Web.UI.Page
    {
        int ServerKey;
        string Mode;
        bool flag=false;
        protected void Page_Load(object sender, EventArgs e)
        {
            //Page.Title = "Lotus Domino Cluster";
            if (Request.QueryString["tab"] != null)
               
            if (!IsPostBack)
            {
                
              
                //FillClusterCategoryComboBox();
             
                if (Session["UserPreferences"] != null)
                {
                    DataTable UserPreferences = (DataTable)Session["UserPreferences"];
                   
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
                       
                      
                        //11/19/2014 NS modified
                        //DominoClusterRoundPanel.HeaderText = "Domino Cluster - " + ClsAttNameTextBox.Text;
                        servernamelbldisp.InnerHtml = "Network Latency - " + ClsAttNameTextBox.Text;
                    }
                   


                }
                else
                {
                    Mode = "Insert";
                    if (!IsPostBack)
                    {
                       
                        testduration.Text = "5";
                        ScanIntervalTextBox.Text = "60";
                       
                        scanEnableCheckBox.Checked = true;
                       
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

                NetworkLatency NetworkLatencyObject = new NetworkLatency();
                NetworkLatency ReturnNLObject = new NetworkLatency();
                NetworkLatencyObject.ID = ID;
                ReturnNLObject = VSWebBL.ConfiguratorBL.NetworkLatencyBL.Ins.GetData(NetworkLatencyObject);       
                //Cluster Setting fields
                ClsAttNameTextBox.Text = ReturnNLObject.Name.ToString();
              
                ScanIntervalTextBox.Text = ReturnNLObject.ScanInterval.ToString();
                testduration.Text = ReturnNLObject.TestDuration.ToString();
                scanEnableCheckBox.Checked = ReturnNLObject.Enabled;//(ScanTextBox.Text != null ? true : false);
             
                //Servers fields
                //12/13/2012 NS modified - Server fields should display server name, not ID
                //ServerAComboBox.Text = ReturnDCObject.ServerID_A.ToString();

                Session["ReturnURL"] = "NetworkLatencyTest.aspx?ID=" + ID + "&tab=2";
                
            }
            catch (Exception ex)
            {
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex; 
            }
            finally { }
        }

        private NetworkLatency CollectDataForNetworkLatency()
        {
            try
            {
               // SetFocusOnControl();
                //Cluster Settings
                NetworkLatency NetworkLatencyObject = new NetworkLatency();
                NetworkLatencyObject.Name = ClsAttNameTextBox.Text;
                NetworkLatencyObject.Enabled = scanEnableCheckBox.Checked;
              
                if(ScanIntervalTextBox.Text!=""&&ScanIntervalTextBox.Text!=null)
                    NetworkLatencyObject.ScanInterval = int.Parse(ScanIntervalTextBox.Text);
                if (testduration.Text != "" && testduration.Text != null)
                    NetworkLatencyObject.TestDuration = int.Parse(testduration.Text);
               
                if(Mode=="Update")
                    NetworkLatencyObject.ID = ServerKey;
                return NetworkLatencyObject;
            }
            catch (Exception ex)
            {
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex; 
            }
            finally { }
        }

        private void UpdateNetworkLatencyData()
        {
            try
            {
                Object ReturnValue = VSWebBL.ConfiguratorBL.NetworkLatencyBL.Ins.UpdateData(CollectDataForNetworkLatency());
                SetFocusOnError(ReturnValue);
                if (ReturnValue.ToString() == "True")
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
                    Session["LatencyUpdateStatus"] = ClsAttNameTextBox.Text;
                    Response.Redirect("NetworkLatencyTest.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
                    Context.ApplicationInstance.CompleteRequest();
                }

            }
            catch (Exception ex)
            {
                //10/3/2014 NS modified for VSPLUS-990
                errorDiv.InnerHtml = "The following error has occurred: " + ex.Message+
                    "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                errorDiv.Style.Value = "display: block";
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
            }
        }

        private void InsertNetworkLatency()
        {

            NetworkLatency ClsObj = new NetworkLatency();
            ClsObj.Name = ClsAttNameTextBox.Text;
            DataTable returntable = VSWebBL.ConfiguratorBL.NetworkLatencyBL.Ins.GetIPAddress(ClsObj);

            if (returntable.Rows.Count > 0)
            {
                ErrorMessageLabel.Text = "This Latency name is already in use.  Please enter a different server name.";
                ErrorMessagePopupControl.ShowOnPageLoad = true;
                flag = true;
                ClsAttNameTextBox.Focus();
            }
            else
            {
                try
                {
                    object ReturnValue = VSWebBL.ConfiguratorBL.NetworkLatencyBL.Ins.InsertData(CollectDataForNetworkLatency());
                    SetFocusOnError(ReturnValue);
                    if (ReturnValue.ToString() == "True")
                    {

                        ErrorMessageLabel.Text = "Latency record created successfully.";
                        ErrorMessagePopupControl.HeaderText = "Information";
                        ErrorMessagePopupControl.ShowCloseButton = false;
                        ValidationUpdatedButton.Visible = true;
                        ValidationOkButton.Visible = false;
                        ErrorMessagePopupControl.ShowOnPageLoad = true;

                    }
                    else
                    {
                        flag = true;
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

        protected void formOKButton_Click(object sender, EventArgs e)
        {
            //Write to Registry
            VSWebBL.SettingBL.SettingsBL.Ins.UpdateSvalue("Network Latency Update", "True", VSWeb.Constants.Constants.SysString);

            try
            {
                if (Mode == "Update")
                {

                    UpdateNetworkLatencyData();
                }
                if(Mode=="Insert")
                {
                    InsertNetworkLatency();
                  

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
             Response.Redirect("NetworkLatencyTest.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
             Context.ApplicationInstance.CompleteRequest();
         }

         protected void ValidationUpdatedButton_Click(object sender, EventArgs e)
         {
             Response.Redirect("NetworkLatencyTest.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
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



       

        

        


       
        ///=====================Alerts
        ///

       
      }
    }



