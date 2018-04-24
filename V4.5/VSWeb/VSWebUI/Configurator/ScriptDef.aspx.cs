using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using DevExpress.Web;
using VSWebDO;

namespace VSWebUI.Configurator
{
    public partial class ScriptDef : System.Web.UI.Page
    {
        string fileNameUpload = "";
        string flag = "Insert";
        protected void Page_Load(object sender, EventArgs e)
        {
            int ID;
            if (!IsPostBack)
            {
                if (Request.QueryString["ID"] != "" && Request.QueryString["ID"] != null)
                {
                    flag = "Update";
                    ID = int.Parse(Request.QueryString["ID"]);
                    fillData(ID);
                }
            }
        }

        public void fillData(int Key)
        {
            try
            {
                CustomScript CScript = new CustomScript();
                CScript.ID = Key;
                CustomScript Returnobj = VSWebBL.ConfiguratorBL.AlertsBL.Ins.GetScriptByKey(CScript);
                ScriptNameTextBox.Text = Returnobj.ScriptName;
                ScriptCommandTextBox.Text = Returnobj.ScriptCommand;
                UploadLocationLabel.Text = Returnobj.ScriptLocation;
                UploadLocationLabel.Visible = true;
                UploadLocationDispLabel.Visible = true;
            }
            catch (Exception ex)
            {
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
            }
        }

        protected void ScriptUploadControl_FileUploadComplete(object sender, DevExpress.Web.FileUploadCompleteEventArgs e)
        {
            try
            {
                errorDiv.Style.Value = "display: none";
                e.CallbackData = fileNameUpload = SavePostedFile(e.UploadedFile);
                if (fileNameUpload != "")
                {
                    UploadLocationDispLabel.Visible = true;
                    UploadLocationLabel.Visible = true;
                    UploadLocationLabel.Text = fileNameUpload;
                }
                successDiv.Style.Value = "display: block";
                successDiv.InnerHtml = "The file has been uploaded to " + fileNameUpload + "." +
                    "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
            }
            catch (Exception ex)
            {
                e.IsValid = false;
                e.ErrorText = ex.Message;
            }
        }

        public string SavePostedFile(UploadedFile uploadedFile)
        {
            ScriptUploadControl.SaveAs(Server.MapPath("~/LogFiles/" + uploadedFile.FileName));
            string fileName1 = Server.MapPath("~/LogFiles/" + uploadedFile.FileName);
            return fileName1;
        }

        protected void CancelButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("ScriptDefGrid.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }

        protected void OKButton_Click(object sender, EventArgs e)
        {
            if (UploadLocationLabel.Text != "")
            {
                InsertScriptData();
                Response.Redirect("ScriptDefGrid.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
            }
            else
            {
                errorDiv.Style.Value = "display: block";
                errorDiv.InnerHtml = "You must upload a script in order to save the script definition. Locate the script file using the Browse button above and upload it using the Upload button in order to proceed." + 
                    "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
            }
        }

        public void InsertScriptData()
        {
            bool res = false;
            CustomScript CScript = new CustomScript();
            if (Request.QueryString["ID"] != "" && Request.QueryString["ID"] != null)
            {
                CScript.ID = int.Parse(Request.QueryString["ID"]);
                flag = "Update";
            }
            CScript.ScriptName = ScriptNameTextBox.Text;
            CScript.ScriptCommand = ScriptCommandTextBox.Text;
            CScript.ScriptLocation = UploadLocationLabel.Text;
            if (flag == "Insert")
            {
                res = VSWebBL.ConfiguratorBL.AlertsBL.Ins.InsertScriptDetails(CScript);
            }
            else
            {
                res = VSWebBL.ConfiguratorBL.AlertsBL.Ins.UpdateScriptDetails(CScript);
            }
        }

    }
}