using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Drawing.Imaging;

namespace VSWebUI.Security
{
    public partial class Company : System.Web.UI.Page
    {
		protected void Page_PreInit(object sender, EventArgs e)
		{

			this.Master.contentCallEvent += new EventHandler(Master_ButtonClick);
		}

		private void Master_ButtonClick(object sender, EventArgs e)
		{

		}

        private int _MaxUserImageSize = 0;//524288;
        private int _MaxUserImageWidth = 250;
        private int _MaxUserImageHeight = 45;
        private const string _ImageTypeInvalid = "Images are restricted to PNG";
        private const string _ImageSizeInvalid = "Images are restricted to 512k";
        private const string _ImageInvalidDimensions = "Images are restricted to 250 width x 45 height.";
        
        
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void LoginButton_Click(object sender, EventArgs e)
        {
            if (FileUploadControl.UploadedFiles[0] != null)
            {
                
                // return when image is not valid
                if (!IsImageValid(FileUploadControl.PostedFile))
                    return;
                ErrorLabel.Text = "";
                string uploadFolder = Server.MapPath("~/images/");
                string fileName = FileUploadControl.UploadedFiles[0].FileName;
                FileUploadControl.UploadedFiles[0].SaveAs(Server.MapPath("~/images/hsbc.png"));
                //savedfile = "~/files/hsbc.png";
            }

        }
        private bool IsImageValid(HttpPostedFile objFile)
        {
            ValidateFiles validateImage = new ValidateFiles();
            if (!Get_IsImageValid(objFile, validateImage))
                return SetError(_ImageTypeInvalid);
            if (!Get_IsImageDimensionsValid(objFile, validateImage))
                return SetError(_ImageInvalidDimensions);
          //  if (!Get_IsImageSizeValid(objFile,
          //validateImage))
          //      return SetError(_ImageSizeInvalid);
            return true;
        }

        private bool SetError(string msg)
        {
            if (!ErrorLabel.Visible)
                ErrorLabel.Visible = true;
            ErrorLabel.Text = msg;
            return false;
        }

        private bool Get_IsImageValid(HttpPostedFile
        objFile, ValidateFiles validateImage)
        {
            return
          validateImage.ValidateFileIsImage(objFile.ContentType);
        }

        private bool
        Get_IsImageDimensionsValid(HttpPostedFile objFile, ValidateFiles validateImage)
        {
            return
          validateImage.ValidateUserImageDimensions(objFile);
        }

        private bool Get_IsImageSizeValid(HttpPostedFile
        objFile, ValidateFiles validateImage)
        {
            return
          validateImage.ValidateUserImageSize(_MaxUserImageSize, objFile.ContentLength);
        }
    }

    public class ValidateFiles
    {
        private bool _IsImage = false;
        private bool _IsOfficeDocument = false;
        private bool _ValidFileSize = false;
        private bool _ValidImageDimension = false;
        private string _FileType = string.Empty;
        private int _FileSize = 0;
        private int _MaxWidth = 250;
        private int _MaxHeight = 45;

        public bool IsImage
        {
            get
            {
                return _IsImage;
            }
            set
            {
                _IsImage = value;
            }
        }

        public bool ValidFileSize
        {
            get
            {
                return _ValidFileSize;
            }
            set
            {
                _ValidFileSize = value;
            }
        }

        public string FileType
        {
            get
            {
                return _FileType;
            }
            set
            {
                _FileType = value;
            }
        }

        public bool ValidImageDimension
        {
            get
            {
                return _ValidImageDimension;
            }
            set
            {
                _ValidImageDimension = value;
            }
        }

        public ValidateFiles()
        {
            // Default construcor
        }

        public bool ValidateFileIsImage(string fileType)
        {
            this._FileType = fileType;

            switch (fileType)
            {
                case "image/png":
                //case "image/jpeg":
                //case "image/pjpeg":
                    IsImage = true;
                    break;
                default:
                    IsImage = false;
                    break;
            }
            return IsImage;
        }

        public bool ValidateUserImageSize(int maxFileSize, int fileSize)
        {
            this._FileSize = fileSize;

            if (maxFileSize > fileSize)
                return ValidFileSize = false;

            return ValidFileSize;
        }

        public bool ValidateUserImageDimensions(HttpPostedFile file)
        {
            using (Bitmap bitmap = new Bitmap(file.InputStream, false))
            {
                if (bitmap.Width == _MaxWidth && bitmap.Height == _MaxHeight)
                    _ValidImageDimension = true;

                return ValidImageDimension;
            }
        }
    }
}