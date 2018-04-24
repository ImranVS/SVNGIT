using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using System.IO;
using DevExpress.XtraPrinting;
using System.Net.Mime;

using DevExpress.XtraExport;
using VSFramework;
using VSWebBL;
using VSWebDO;

namespace VSWebUI.Configurator
{
    public partial class DominoServerThreshold : System.Web.UI.Page
    {
        public bool exactmatch;

        //4/9/2014 NS added for proper master page calculation
        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (Request.QueryString["M"] == "C" && Request.QueryString["M"].ToString() != "")
            {
                this.MasterPageFile = "~/Reports.Master";

            }
            else
            {
                this.MasterPageFile = "~/Reports.Master";

            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ReloadGrid();
            if (!IsPostBack)
            {

            if (Session["UserPreferences"] != null)
                {
                DataTable UserPreferences = (DataTable)Session["UserPreferences"];
                foreach (DataRow dr in UserPreferences.Rows)
                    {
                        if (dr[1].ToString() == "DominoServerThreshold|DominoServerReportsGridView")
                        {
                            DominoServerReportsGridView.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                        }
                    }
                }
            }
        }
        protected void DateParamEdit_DateChanged(object sender, EventArgs e)
        {
            ReloadGrid();
        }

        public void ReloadGrid()
        {
            //string date;
            exactmatch = false;
            //10/23/2013 NS modified - added jQuery month/year control
            /*
            if (this.DateParamEdit.Text == "")
            {
                date = DateTime.Now.ToString();
                this.DateParamEdit.Date = Convert.ToDateTime(date);
            }
            else
            {
                date = this.DateParamEdit.Value.ToString();
            }
             */
            /*if (startDate.Value.ToString() == "")
            {
                date = DateTime.Now.ToString();
            }
            else
            {
                date = startDate.Value.ToString();
            }
            DateTime dtval = Convert.ToDateTime(date);*/
            DataTable dt = new DataTable();
            dt = VSWebBL.DashboardBL.DominoServerStatisticsDetailBLL.Ins.GetDominoServerThreshold();
            DominoServerReportsGridView.DataSource = dt;
            DominoServerReportsGridView.DataBind();
        }
        
        protected void ExportXlsButton_Click(object sender, EventArgs e)
        {
            ServerGridViewExporter.WriteXlsToResponse();
        }

        protected void ExportXlsxButton_Click(object sender, EventArgs e)
        {
            
            ServerGridViewExporter.WriteXlsxToResponse();
        }

        protected void ExportPdfButton_Click(object sender, EventArgs e)
        {
            ServerGridViewExporter.WritePdfToResponse();
        }

        protected void DominoServerReportsGridView_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            ReloadGrid();
        }

        protected void DominoServerReportsGridView_PageSizeChanged(object sender, EventArgs e)
        {
            //ProfilesGridView.PageIndex;
            VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("DominoServerThreshold|DominoServerReportsGridView", DominoServerReportsGridView.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        }

        protected void ASPxMenu1_ItemClick(object source, DevExpress.Web.MenuItemEventArgs e)
        {
            if (e.Item.Name == "ExportXLSItem")
            {
                ServerGridViewExporter.WriteXlsToResponse();
            }
            else if (e.Item.Name == "ExportXLSXItem")
            {
                ServerGridViewExporter.WriteXlsxToResponse();
            }
            else if (e.Item.Name == "ExportPDFItem")
            {
                ServerGridViewExporter.Landscape = true;
                using (MemoryStream ms = new MemoryStream())
                {
                    PrintableComponentLink pcl = new PrintableComponentLink(new PrintingSystem());
                    pcl.Component = ServerGridViewExporter;
                    pcl.Margins.Left = pcl.Margins.Right = 50;
                    pcl.Landscape = true;
                    pcl.CreateDocument(false);
                    pcl.PrintingSystem.Document.AutoFitToPagesWidth = 1;
                    pcl.ExportToPdf(ms);
                    WriteResponse(this.Response, ms.ToArray(), System.Net.Mime.DispositionTypeNames.Attachment.ToString());
                    //ServerGridViewExporter.WritePdfToResponse();
                }
            }
        }

        //11/10/2014 NS added - the function below will format the table to fit on one page width wise
        public static void WriteResponse(HttpResponse response, byte[] filearray, string type)
        {
            response.ClearContent();
            response.Buffer = true;
            response.Cache.SetCacheability(HttpCacheability.Private);
            response.ContentType = "application/pdf";
            ContentDisposition contentDisposition = new ContentDisposition();
            contentDisposition.FileName = "DominoServerConfigSettings.pdf";
            contentDisposition.DispositionType = type;
            response.AddHeader("Content-Disposition", contentDisposition.ToString());
            response.BinaryWrite(filearray);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
            try
            {
                response.End();
            }
            catch (System.Threading.ThreadAbortException)
            {
            }

        }
    }
}
     