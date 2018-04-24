using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using VSFramework;
using VSWebBL;
using VSWebDO;
using System.Data;
using DevExpress.Web;

namespace VSWebUI
{
    public partial class WebForm17 : System.Web.UI.Page
    {
        string ServerKey;
        string Mode;
      
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Title = "PowerShell Edit";                

            try
            {
                if (Request.QueryString["ID"] != null && Request.QueryString["ID"] != "")
                {
                    Mode = "Update";
                    txtScriptName.Enabled = false;
                    ServerKey = Request.QueryString["ID"].ToString();
                  
                    if (!IsPostBack)
                    {                       
                        FillData(ServerKey);
                        fillcomboCategory();
                    }
                   
                }
                else
                {
                    Mode = "Insert";
                    fillcomboCategory();
                 
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


        public void fillcomboCategory()
        {
            DataTable dt = new DataTable();
            dt = VSWebBL.ConfiguratorBL.PwrshelBL.Ins.fillComboBL();
            category.DataSource = dt;
            category.TextField = "Category";
            category.ValueField = "Category";
            category.DataBind();
        }



        private void FillData(string ScriptName)
        {
            try
            {
                PowershellScripts Pwrobj = new PowershellScripts();

                PowershellScripts ReturnpwrObject = new PowershellScripts();
                Pwrobj.ScriptName = ScriptName;
                ReturnpwrObject = VSWebBL.ConfiguratorBL.PwrshelBL.Ins.GetPwrData(Pwrobj);
                txtScriptName.Text = ReturnpwrObject.ScriptName.Replace("&#39;", "'");
                ScriptDetailsMemo.Text = ReturnpwrObject.ScriptDetails.Replace("&#39;", "'");
                category.Text = ReturnpwrObject.Category.Replace("&#39;", "'");
                txtDescripation.Text = ReturnpwrObject.Description.Replace("&#39;", "'");

            }
            catch (Exception ex)
            {
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex; 
            }
            finally { }
        }

        private PowershellScripts CollectDataForPowershellScripts()
        {
            try
            {
                // SetFocusOnControl();
                //Cluster Settings
                PowershellScripts pwrObject = new PowershellScripts();
                pwrObject.ScriptName = txtScriptName.Text.Replace("'","&#39;");
                pwrObject.ScriptDetails = ScriptDetailsMemo.Text.Replace("'", "&#39;");
                pwrObject.Category = category.Text.Replace("'", "&#39;");
                pwrObject.Description = txtDescripation.Text.Replace("'", "&#39;");
              
                return pwrObject;
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
                
            }
        }
        private void UpdatePwrscriptData()
        {
            try
            {
                Object ReturnValue = VSWebBL.ConfiguratorBL.PwrshelBL.Ins.UpdateScript(CollectDataForPowershellScripts());
                SetFocusOnError(ReturnValue);
                if (ReturnValue.ToString() == "True")
                {

                    ErrorMessageLabel.Text = "Data updated successfully.";
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

        private void InsertDominoCluster()
        {

            //PowershellScripts pwrObj = new PowershellScripts();
            //pwrObj.ScriptName = txtScriptName.Text;
            DataTable returntable = VSWebBL.ConfiguratorBL.PwrshelBL.Ins.GetScriptName(txtScriptName.Text);

            if (returntable.Rows.Count > 0)
            {
                ErrorMessageLabel.Text = "This Script name is already in use.  Please enter a different name.";
                ErrorMessagePopupControl.ShowOnPageLoad = true;
              
            }
            else
            {
                try
                {
                    object ReturnValue = VSWebBL.ConfiguratorBL.PwrshelBL.Ins.InsertScript(CollectDataForPowershellScripts());
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

        protected void OKButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (Mode == "Update")
                {

                    UpdatePwrscriptData();
                }
                if (Mode == "Insert")
                {
                    InsertDominoCluster();
                
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

        protected void ValidationUpdatedButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Configurator/PowerShellScripts.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
            Context.ApplicationInstance.CompleteRequest();
        }

        protected void CancelButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Configurator/PowerShellScripts.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
            Context.ApplicationInstance.CompleteRequest();
        }

        protected void category_SelectedIndexChanged(object sender, EventArgs e)
        {
            PowershellScripts pwrObject = new PowershellScripts();
             if(category.Text!="")
            {
                pwrObject.Category = category.Text;
            }

        }


      

    }
}