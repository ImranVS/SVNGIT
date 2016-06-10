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
namespace VSWebUI.Configurator
{
    public partial class EditServerTask : System.Web.UI.Page
    {
        int ServerKey;
        string Mode;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Request.QueryString["TaskID"] != null && Request.QueryString["TaskID"] != "")
                {
                    Mode = "Update";

                    ServerKey = int.Parse(Request.QueryString["TaskID"]);

                    if (!IsPostBack)
                    {
                        //Fill EditServerTask page
                        FillData(ServerKey);
                        if (EditEnableCheckBox.Checked)
                        {
                            EditMaxRunTimeTextBox.Enabled = true;
                            EditRetryCountTextBox.Enabled = true;
                        }
                        else
                        {
                            EditMaxRunTimeTextBox.Enabled = false;

                        }
                        //11/19/2014 NS modified
                        //ServerTaskRoundPanel.HeaderText = "Server Task Definitions - " + EditTaskNameTextBox.Text;
                        servernamelbldisp.InnerHtml += " - " + EditTaskNameTextBox.Text;
                    }
                }
                else
                {
                    Mode = "Insert";
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

                DominoServerTasks DominoSTaskObject = new DominoServerTasks();
                DominoServerTasks ReturnDSTaskObject = new DominoServerTasks();
                DominoSTaskObject.TaskID = ID;
                ReturnDSTaskObject = VSWebBL.ConfiguratorBL.DominoServerTasksBL.Ins.GetDataForID(DominoSTaskObject);
                //DominoStatistic fields
                EditTaskNameTextBox.Text = ReturnDSTaskObject.TaskName.ToString();
                EditIdleStringTextBox.Text = ReturnDSTaskObject.IdleString.ToString();
                EditConsoleStringTextBox.Text = ReturnDSTaskObject.ConsoleString.ToString();
                EditLoadCmdTextBox.Text = ReturnDSTaskObject.LoadString.ToString();
                EditMaxRunTimeTextBox.Text = ReturnDSTaskObject.MaxBusyTime.ToString();
                EditRetryCountTextBox.Text = ReturnDSTaskObject.RetryCount.ToString();
                if (ReturnDSTaskObject.FreezeDetect == true)
                {
                    EditEnableCheckBox.Checked = true;
                }
                else
                {
                    EditEnableCheckBox.Checked = false;
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

        private DominoServerTasks CollectDataForDominoServerTasks()
        {
            try
            {
                //DominoServerTasks 
                DominoServerTasks DominoSTasksObject = new DominoServerTasks();
                DominoSTasksObject.TaskName = EditTaskNameTextBox.Text;
                DominoSTasksObject.IdleString = EditIdleStringTextBox.Text;
                DominoSTasksObject.ConsoleString = EditConsoleStringTextBox.Text;
                DominoSTasksObject.LoadString = EditLoadCmdTextBox.Text;               
                DominoSTasksObject.RetryCount = int.Parse(EditRetryCountTextBox.Text);
                if (EditEnableCheckBox.Checked == true)
                {
                    DominoSTasksObject.FreezeDetect = true;
                }
                else
                {
                    DominoSTasksObject.FreezeDetect = false;
                }

               if(EditMaxRunTimeTextBox.Text!=""&& EditMaxRunTimeTextBox.Text!=null)
                DominoSTasksObject.MaxBusyTime = int.Parse(EditMaxRunTimeTextBox.Text);          
               
                if (Mode == "Update")
                {
                    DominoSTasksObject.TaskID = ServerKey;
                }

                return DominoSTasksObject;
            }
            catch (Exception ex)
            {
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex; 
            }
            finally { }
        }
        private void UpdateDominoServerTasksData()
        {

            try
            {


                DominoServerTasks NDObj = new DominoServerTasks();

                NDObj.TaskName = EditTaskNameTextBox.Text;
                NDObj.TaskID = ServerKey;
                DataTable returntable = VSWebBL.ConfiguratorBL.DominoServerTasksBL.Ins.GetName(NDObj);

                if (returntable.Rows.Count > 0)
                {
                    ErrorMessageLabel.Text = "This Name is already being monitored. Please enter another  or Name.";
                    ErrorMessagePopupControl.ShowOnPageLoad = true;

                    //IPAddressTextBox.Focus();
                }
                else
                {

                    try
                    {
                        Object ReturnValue = VSWebBL.ConfiguratorBL.DominoServerTasksBL.Ins.UpdateData(CollectDataForDominoServerTasks());
                        SetFocusOnError(ReturnValue);
                        if (ReturnValue.ToString() == "True")
                        {
                            //1/21/2014 NS modified
                            /*
                            ErrorMessageLabel.Text = "Data updated successfully.";
                            ErrorMessagePopupControl.HeaderText = "Information";
                            ErrorMessagePopupControl.ShowCloseButton = false;
                            ValidationUpdatedButton.Visible = true;
                            ValidationOkButton.Visible = false;
                            ErrorMessagePopupControl.ShowOnPageLoad = true;
                            */
                            Session["ServerTaskUpdateStatus"] = EditTaskNameTextBox.Text;
                            Response.Redirect("ServerTaskDefinitions.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
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
            }
            catch (Exception ex)
            {
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
        }

        private void InsertDominoServerTasks()
        {                       
                       
          try
            {


                DominoServerTasks NDObj = new DominoServerTasks();
                
                NDObj.TaskName = EditTaskNameTextBox.Text;
                NDObj.TaskID = ServerKey;
                DataTable returntable = VSWebBL.ConfiguratorBL.DominoServerTasksBL.Ins.GetName(NDObj);

                if (returntable.Rows.Count > 0)
                {
                    ErrorMessageLabel.Text = "This Name is already being monitored. Please enter another Name.";
                    ErrorMessagePopupControl.ShowOnPageLoad = true;
                   
                    //IPAddressTextBox.Focus();
                }
                else
                {

          
           
                try
                {
                    object ReturnValue = VSWebBL.ConfiguratorBL.DominoServerTasksBL.Ins.InsertData(CollectDataForDominoServerTasks());
                    SetFocusOnError(ReturnValue);
                    if (ReturnValue.ToString() == "True")
                    {

                        ErrorMessageLabel.Text = "Data inserted successfully.";
                        ErrorMessagePopupControl.HeaderText = "Information";
                        ErrorMessagePopupControl.ShowCloseButton = false;
                        ValidationUpdatedButton.Visible = true;
                        ValidationOkButton.Visible = false;
                        ErrorMessagePopupControl.ShowOnPageLoad = true;

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
            catch (Exception ex)
            {
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
            }
      
      
        private void SetFocusOnError(Object ReturnValue)
        {
            string ErrorMessage = ReturnValue.ToString();
            if (ErrorMessage.Substring(0, 2) == "ER")
            {
                ErrorMessageLabel.Text = ErrorMessage.Substring(3);
                ErrorMessagePopupControl.ShowOnPageLoad = true;

            }
        }

        

        protected void formOKButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (Mode == "Update")
                {

                    UpdateDominoServerTasksData();
                }
                if (Mode == "Insert")
                {
                    InsertDominoServerTasks();
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

        protected void CancelButton_Click1(object sender, EventArgs e)
        {
            Response.Redirect("ServerTaskDefinitions.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
            Context.ApplicationInstance.CompleteRequest(); ;
        }

        protected void ValidationUpdatedButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("ServerTaskDefinitions.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
            Context.ApplicationInstance.CompleteRequest();
        }

        protected void EditEnableCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (EditEnableCheckBox.Checked == true)
            {
                EditMaxRunTimeTextBox.Enabled = true;
                //EditRetryCountTextBox.Enabled = true;
            }
            else
            {
                EditMaxRunTimeTextBox.Enabled = false;

            }
        }
      }
    }
