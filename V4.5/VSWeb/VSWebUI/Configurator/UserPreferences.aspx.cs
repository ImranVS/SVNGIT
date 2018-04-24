using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Drawing.Imaging;
using System.Data;
using VSWebDO;
using VSWebBL;
using System.Web.UI.HtmlControls;

using DevExpress.Web;

namespace VSWebUI
{
    public partial class CompanyLogo : System.Web.UI.Page
    {
        private int _MaxUserImageSize = 0;//524288;
        private int _MaxUserImageWidth = 250;
        private int _MaxUserImageHeight = 45;
        private const string _ImageTypeInvalid = "Images are restricted to PNG.";
        private const string _ImageSizeInvalid = "Images are restricted to 512K.";
        private const string _ImageInvalidDimensions = "Images are restricted to 250 width x 45 height.";
        string savedfile;
        string DominoSession = "Domino";
        string SametimeSession = "Sametime";
        string ExchangeSession = "Exchange";
        string URLSession = "URL";
        string ActiveDirectorySession = "Active Directory";
        protected DataTable ProfilesDataTable = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            //5/2/2014 NS modified
            //Page.Title = "Company Logo";
            if (!IsPostBack && !IsCallback)
            {
                Session["Profiles"] = null;
                Session["ProfileId"] = "0";

                bool isadmin = false;
               
                DataTable sa = VSWebBL.SecurityBL.UsersBL.Ins.GetIsAdmin1(Session["UserID"].ToString());
                if (sa.Rows.Count > 0)
                {
                    if (sa.Rows[0]["SuperAdmin"].ToString() == "Y")
                    {
                        isadmin = true;
                    }
                }
                if (!isadmin)
                {
                    tblPreferences.Visible = false;
                    divInfo.Visible = false;
                    divErrorMessage.Visible = true;

                }
                else
                {
                    Company cmpobj = new Company();
                    cmpobj = VSWebBL.ConfiguratorBL.CompanyBL.Ins.GetLogo();
                    CompanyNameTextBox.Text = cmpobj.CompanyName;
                    FilePathHyperLink.Text = cmpobj.LogoPath;
                    FilePathHyperLink.NavigateUrl = cmpobj.LogoPath;
                    Session[DominoSession] = "";
                    Session[SametimeSession] = "";
                    Session[ExchangeSession] = "";
                    Session[URLSession] = "";
                    Session[ActiveDirectorySession] = "";
                    FillGeneralPreferences();
                    FillBingPreferences();
                }
            }
        }
        protected void LoginButton_Click(object sender, EventArgs e)
        {
            Int32 fileLen;
            string fileName, extension, fileExt, fileext;
            if (FileUploadControl.UploadedFiles[0] != null)
            {

                fileName = FileUploadControl.UploadedFiles[0].FileName;
                extension = FileUploadControl.PostedFile.FileName;
                fileext = System.IO.Path.GetExtension(extension);
                fileExt = fileext.ToLower();
                //.jpg,.jpeg,.jpe,.gif,.png
                if ((fileExt == ".jpg") || (fileExt == ".jpeg") || (fileExt == ".jpe") || (fileExt == ".gif") || (fileExt == ".png"))
                {
                    fileLen = FileUploadControl.PostedFile.ContentLength;
                    Byte[] Input = new Byte[fileLen];

                    if (fileLen < 30000)
                    {


                        // Initialize the stream to read the uploaded file.
                        //myStream = FileUpload1.FileContent;
                        //Byte[] filesize;
                        //filesize=FileUploadControl.FileBytes;


                        // return when image is not valid
                        //if (!IsImageValid(FileUploadControl.PostedFile))
                        //    return;
                        //ErrorLabel.Text = "";
                        string uploadFolder = Server.MapPath("~/images/");
                        fileName = FileUploadControl.UploadedFiles[0].FileName;
                        if (fileName != null && fileName != "")
                        {
                            try
                            {
                                FileUploadControl.UploadedFiles[0].SaveAs(uploadFolder + fileName);//Server.MapPath("~/images/logo.png"));

                            }
                            catch (Exception ex)
                            {
                                //6/27/2014 NS added for VSPLUS-634
                                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                                throw ex;
                            }
                            savedfile = "/images/" + fileName;
                        }
                        else
                        {

                            savedfile = FilePathHyperLink.Text;
                        }
                        //Parent.logo.ImageUrl 
                        System.Web.UI.WebControls.Image img = (System.Web.UI.WebControls.Image)this.Master.FindControl("logo");

                        img.ImageUrl = savedfile;
                        FilePathHyperLink.Text = savedfile;
                        FilePathHyperLink.NavigateUrl = savedfile;
                    }
                    if (fileLen < 30000)
                    {
                        Company Cmpobj = new Company();
                        Cmpobj.CompanyName = CompanyNameTextBox.Text;
                        Cmpobj.LogoPath = savedfile;
                        //12/24/2013 NS modified
                        VSWebBL.ConfiguratorBL.CompanyBL.Ins.UpdateLogo(Cmpobj);
                        if (Cmpobj != null)
                        {
                            logoerrordiv.Style.Value = "display: none";
                            logosuccessdiv.Style.Value = "display: block";
                        }
                        else
                        {
                            logosuccessdiv.Style.Value = "display: none";
                            logoerrordiv.Style.Value = "display: block";
                        }
                    }
                    else
                    {
                        logosuccessdiv.Style.Value = "display: none";
                        logoerrordiv.Style.Value = "display: block";

                        //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('The size of the uploaded file exceeds max size 30KB.')", true);
                    }

                }
            }
        }
        //private bool IsImageValid(HttpPostedFile objFile)
        //{
        //    ValidateFiles validateImage = new ValidateFiles();
        //    if (!Get_IsImageValid(objFile, validateImage))
        //        return SetError(_ImageTypeInvalid);
        //    if (!Get_IsImageDimensionsValid(objFile, validateImage))
        //        return SetError(_ImageInvalidDimensions);
        //  //  if (!Get_IsImageSizeValid(objFile,
        //  //validateImage))
        //  //      return SetError(_ImageSizeInvalid);
        //    return true;
        //}

        //private bool SetError(string msg)
        //{
        //    if (!ErrorLabel.Visible)
        //        ErrorLabel.Visible = true;
        //    ErrorLabel.Text = msg;
        //    return false;
        //}

        //private bool Get_IsImageValid(HttpPostedFile
        //objFile, ValidateFiles validateImage)
        //{
        //    return
        //  validateImage.ValidateFileIsImage(objFile.ContentType);
        //}

        //private bool
        //Get_IsImageDimensionsValid(HttpPostedFile objFile, ValidateFiles validateImage)
        //{
        //    return
        //  validateImage.ValidateUserImageDimensions(objFile);
        //}

        //private bool Get_IsImageSizeValid(HttpPostedFile
        //objFile, ValidateFiles validateImage)
        //{
        //    return
        //  validateImage.ValidateUserImageSize(_MaxUserImageSize, objFile.ContentLength);
        //}

        protected void CancelButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Configurator/Default.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
            Context.ApplicationInstance.CompleteRequest();
        }

   


        #region UpdateDefaults

        private ProfilesMaster CollectDataForProfiles(DataRow ProfilesRow)
        {
            try
            {
                ProfilesMaster ProfilesObject = new ProfilesMaster();
                ProfilesObject.ServerType = ProfilesRow["ServerType"].ToString();
                ProfilesObject.RelatedTable = ProfilesRow["RelatedTable"].ToString();
                ProfilesObject.RelatedField = ProfilesRow["RelatedField"].ToString();
                ProfilesObject.DefaultValue = ProfilesRow["DefaultValue"].ToString();
                ProfilesObject.UnitOfMeasurement = ProfilesRow["UnitofMeasurement"].ToString();
                ProfilesObject.AttributeName = ProfilesRow["AttributeName"].ToString();

                return ProfilesObject;
            }
            catch (Exception ex)
            {
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
            finally { }
        }


        protected void UpdateAttributeDefaults(string SessionName, ASPxGridView ProfilesGridView, HtmlGenericControl Error, HtmlGenericControl Success)
        {
            Error.Style.Value = "display: none;";
            Success.Style.Value = "display: none";
            List<object> fieldValues = ProfilesGridView.GetSelectedFieldValues(new string[] { "RelatedField", "DefaultValue", "RelatedTable" });
            bool Update = false;
            string AttributeErrors = "";
            string AppliedAttribute = "";
            if (fieldValues.Count > 0)
            {
                int ProfileMasterID = 0;
                DataTable ProfilesDataTable = new DataTable();
                if (Session[SessionName] != null && Session[SessionName] != "")
                {
                    ProfilesDataTable = (DataTable)Session[SessionName];
                }
                for (int i = 0; i < ProfilesDataTable.Rows.Count; i++)
                {
                    if (ProfilesDataTable.Rows[i]["isSelected"].ToString() == "true")
                    {
                        ProfileMasterID = Convert.ToInt32(ProfilesDataTable.Rows[i]["ID"]);
                        Update = VSWebBL.SecurityBL.ProfilesMasterBL.Ins.UpdateProfiles(CollectDataForProfiles(ProfilesDataTable.Rows[i]), ProfilesDataTable.Rows[i]["ID"].ToString());

                        if (Update == false)
                        {
                            if (AttributeErrors == "")
                            {
                                AttributeErrors += ProfilesDataTable.Rows[i]["AttributeName"].ToString();
                            }
                            else
                            {
                                AttributeErrors += ", " + ProfilesDataTable.Rows[i]["AttributeName"].ToString();
                            }
                        }
                        else
                        {
                            if (AppliedAttribute == "")
                            {
                                AppliedAttribute += ProfilesDataTable.Rows[i]["AttributeName"].ToString();
                            }
                            else
                            {
                                AppliedAttribute += ", " + ProfilesDataTable.Rows[i]["AttributeName"].ToString();
                            }
                        }
                    }
                }
                if (AttributeErrors != "")
                {
                    //2/26/2014 NS modified
                    //Error.InnerHtml = "The default values for " + SessionName + " attributes(s): " + AttributeErrors + " were NOT updated.";
                    //10/6/2014 NS modified for VSPLUS-990
                    Error.InnerHtml = "The following default values for " + SessionName + " were NOT updated: " + AttributeErrors + " ." +
                        "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                    Error.Style.Value = "display: block";
                }
                if (AppliedAttribute != "")
                {
                    //2/26/2014 NS modified
                    //Success.InnerHtml = "The default values for " + SessionName + " attributes(s): " + AppliedAttribute + " were updated.";
                    //10/6/2014 NS modified for VSPLUS-990
                    Success.InnerHtml = "The following default values for " + SessionName + " were successfully updated: " + AppliedAttribute + "." +
                        "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                    Success.Style.Value = "display: block";
                }
            }
            else
            {
                //2/26/2014 NS modified
                //Error.InnerHtml = "Kindly select the required attributes to update their default values";
                //10/6/2014 NS modified for VSPLUS-990
                Error.InnerHtml = "Please select at least one attribute to update its default values." +
                    "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                Error.Style.Value = "display: block";
            }
        }

        #endregion

       

        private Users CollectDataForUsers()
        {
            try
            {
                Users UsersObject = new Users();

                UsersObject.LoginName = Session["UserLogin"].ToString();
                UsersObject.FullName = Session["UserFullName"].ToString();
                UsersObject.Email = Session["UserEmail"].ToString();
                UsersObject.SecurityQuestion1 = Session["UserSecurityQuestion1"].ToString();
                UsersObject.SecurityQuestion1Answer = Session["UserSecurityQuestion1Answer"].ToString();
                UsersObject.SecurityQuestion2 = Session["UserSecurityQuestion2"].ToString();
                UsersObject.SecurityQuestion2Answer = Session["UserSecurityQuestion2Answer"].ToString();
                UsersObject.Refreshtime = Convert.ToInt32(Session["Refreshtime"]);
                UsersObject.StartupURL = Session["StartupURL"].ToString();
                UsersObject.ID = int.Parse(Session["UserID"].ToString());
                return UsersObject;
            }
            catch (Exception ex)
            {
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
            finally { }
        }
        private void FillStartupURLS(ASPxComboBox StartupURLCombobox)
        {
            Users UsersObject = new Users();
            UsersObject.IsConfigurator = Convert.ToBoolean(Session["Isconfigurator"].ToString());
            UsersObject.Isdashboard = Convert.ToBoolean(Session["IsDashboard"].ToString());
            UsersObject.Isconsolecomm = Convert.ToBoolean(Session["Isconsolecomm"].ToString());

            DataTable StartupURLDataTable = VSWebBL.SecurityBL.UserStartupURLBL.Ins.GetAllData(UsersObject);
            StartupURLCombobox.DataSource = StartupURLDataTable;
            StartupURLCombobox.TextField = "Name";
            StartupURLCombobox.ValueField = "URL";
            StartupURLCombobox.DataBind();
        }

        protected void FillGeneralPreferences()
        {
            string statusThreshold = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("StatusChangedThreshold");
            if (statusThreshold.Trim() == "")
                statusThreshold = "15";
            StatusChagnedTextBox.Text = statusThreshold;

            string MonitoringDelay = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("MonitoringDelay");
            if (MonitoringDelay.Trim() == "")
                MonitoringDelay = "10";
            MonitoringDelayTextBox.Text = MonitoringDelay;

            //5/20/2016 Sowjanya modified for VSPLUS-2985

            string CurrencySymbol = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("CurrencySymbol");
            if (CurrencySymbol.Trim() == "")
                CurrencySymbol = "$";
            CurrencySymbolTextBox.Text = CurrencySymbol;

            string showhide = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("Show Dashboard only/Exec Summary Buttons");
            string strDiskYellowTh = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("DiskYellowThreshold");
            DiskSpaceAlertThreshold.Text = (strDiskYellowTh == "" ? "20" : strDiskYellowTh);
            if (showhide == "True")
            {
                SortRadioButtonList1.SelectedIndex = 0;
            }
            else
            {
                SortRadioButtonList1.SelectedIndex = 1;
            }
           
        }

        protected void GeneralApply_Click(object sender, EventArgs e)
        {
            int show = Convert.ToInt32(SortRadioButtonList1.SelectedItem.Value);

            Object ReturnValue = false;
          
            try
            {
                ReturnValue = VSWebBL.SettingBL.SettingsBL.Ins.UpdateSvalue("StatusChangedThreshold", StatusChagnedTextBox.Text, VSWeb.Constants.Constants.SysString);
                ReturnValue = VSWebBL.SettingBL.SettingsBL.Ins.UpdateSvalue("MonitoringDelay", MonitoringDelayTextBox.Text, VSWeb.Constants.Constants.SysInt32);
                ReturnValue = VSWebBL.SettingBL.SettingsBL.Ins.UpdateSvalue("DiskYellowThreshold",  DiskSpaceAlertThreshold.Text, VSWeb.Constants.Constants.SysString);
                //5/20/2016 Sowjanya modified for VSPLUS-2985
                ReturnValue = VSWebBL.SettingBL.SettingsBL.Ins.UpdateSvalue("CurrencySymbol", CurrencySymbolTextBox.Text, VSWeb.Constants.Constants.SysString);
                if (show == 1)
                {
                    ReturnValue = VSWebBL.SettingBL.SettingsBL.Ins.UpdateSvalue("Show Dashboard only/Exec Summary Buttons", "True", VSWeb.Constants.Constants.SysString);
                }
                else
                {
                    ReturnValue = VSWebBL.SettingBL.SettingsBL.Ins.UpdateSvalue("Show Dashboard only/Exec Summary Buttons", "False", VSWeb.Constants.Constants.SysString);
                }

            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
            }

            if ((bool)ReturnValue)
            {
                GeneralSuccessDiv.Style.Value = "display: block";
            }
            else
            {
                GeneralErrorDiv.Style.Value = "display: block";
            }
        }

        protected void FillBingPreferences()
        {
            string Key = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("BingApiKey");
            if (Key == null)
                Key = "";
            BingKeyTextBox.Text = Key;
        }

        protected void BingApply_Click(object sender, EventArgs e)
        {
            Object ReturnValue = false;

            try
            {
                ReturnValue = VSWebBL.SettingBL.SettingsBL.Ins.UpdateSvalue("BingApiKey", BingKeyTextBox.Text, VSWeb.Constants.Constants.SysString);
            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
            }

            if ((bool)ReturnValue)
            {
                BingSuccessDiv.Style.Value = "display: block";
            }
            else
            {
                BingErrorDiv.Style.Value = "display: block";
            }
        }

        protected void FileUploadControl_FileUploadComplete(object sender, FileUploadCompleteEventArgs e)
        {
            if (FileUploadControl.UploadedFiles[0] != null)
            {
                Int32 fileLen;
                fileLen = FileUploadControl.PostedFile.ContentLength;
                Byte[] Input = new Byte[fileLen];
                int size = FileUploadControl.Size;
                if (fileLen > 30000)
                {
                    //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('The size of the uploaded file exceeds max size 30KB.')", true);
                }

            }

        }

 

        //public class ValidateFiles
        //{
        //    private bool _IsImage = false;
        //    private bool _IsOfficeDocument = false;
        //    private bool _ValidFileSize = false;
        //    private bool _ValidImageDimension = false;
        //    private string _FileType = string.Empty;
        //    private int _FileSize = 0;
        //    private int _MaxWidth = 250;
        //    private int _MaxHeight = 45;

        //    public bool IsImage
        //    {
        //        get
        //        {
        //            return _IsImage;
        //        }
        //        set
        //        {
        //            _IsImage = value;
        //        }
        //    }

        //    public bool ValidFileSize
        //    {
        //        get
        //        {
        //            return _ValidFileSize;
        //        }
        //        set
        //        {
        //            _ValidFileSize = value;
        //        }
        //    }

        //    public string FileType
        //    {
        //        get
        //        {
        //            return _FileType;
        //        }
        //        set
        //        {
        //            _FileType = value;
        //        }
        //    }

        //    public bool ValidImageDimension
        //    {
        //        get
        //        {
        //            return _ValidImageDimension;
        //        }
        //        set
        //        {
        //            _ValidImageDimension = value;
        //        }
        //    }

        //    public ValidateFiles()
        //    {
        //        // Default construcor
        //    }

        //    public bool ValidateFileIsImage(string fileType)
        //    {
        //        this._FileType = fileType;

        //        switch (fileType)
        //        {
        //            case "image/png":
        //            //case "image/jpeg":
        //            //case "image/pjpeg":
        //                IsImage = true;
        //                break;
        //            default:
        //                IsImage = false;
        //                break;
        //        }
        //        return IsImage;
        //    }

        //    public bool ValidateUserImageSize(int maxFileSize, int fileSize)
        //    {
        //        this._FileSize = fileSize;

        //        if (maxFileSize > fileSize)
        //            return ValidFileSize = false;

        //        return ValidFileSize;
        //    }

        //    public bool ValidateUserImageDimensions(HttpPostedFile file)
        //    {
        //        using (Bitmap bitmap = new Bitmap(file.InputStream, false))
        //        {
        //            if (bitmap.Width == _MaxWidth && bitmap.Height == _MaxHeight)
        //                _ValidImageDimension = true;

        //            return ValidImageDimension;
        //        }
        //    }
        //}

    }
}