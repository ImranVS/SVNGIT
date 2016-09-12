using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using DevExpress.XtraPrinting;
using System.Net.Mime;

namespace VSWebUI.DashboardReports
{
    public partial class IBMConnectionsUserAdoptionRpt : System.Web.UI.Page
    {
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
            SetUserAdoptionGrid();
            if (Session["UserPreferences"] != null)
            {
                DataTable UserPreferences = (DataTable)Session["UserPreferences"];
                foreach (DataRow dr in UserPreferences.Rows)
                {
                    if (dr[1].ToString() == "UserAdoption|UserAdoptionPivotGrid")
                    {
                        UserAdoptionPivotGrid.OptionsPager.RowsPerPage = Convert.ToInt32(dr[2]);
                    }
                }
            }
        }

        private void SetUserAdoptionGrid()
        {
            DataTable dt = new DataTable();
            dt = VSWebBL.DashboardBL.ConnectionsBL.Ins.GetUserAdoption();
            UserAdoptionPivotGrid.DataSource = dt;
            UserAdoptionPivotGrid.DataBind();
            UserAdoptionPivotGrid.Fields["Name"].SortBySummaryInfo.Field = UserAdoptionPivotGrid.Fields["Total"];
            UserAdoptionPivotGrid.Fields["Name"].SortOrder = DevExpress.XtraPivotGrid.PivotSortOrder.Descending;
        }

        protected void ASPxMenu1_ItemClick(object source, DevExpress.Web.MenuItemEventArgs e)
        {
            if (e.Item.Name == "ExportXLSItem")
            {
                ServerGridViewExporter.ExportXlsToResponse("UserAdoption.xls");
            }
            else if (e.Item.Name == "ExportXLSXItem")
            {
                ServerGridViewExporter.ExportXlsxToResponse("UserAdoption.xlsx");
            }
            else if (e.Item.Name == "ExportPDFItem")
            {
                //ServerGridViewExporter.Landscape = true;
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

        public static void WriteResponse(HttpResponse response, byte[] filearray, string type)
        {
            response.ClearContent();
            response.Buffer = true;
            response.Cache.SetCacheability(HttpCacheability.Private);
            response.ContentType = "application/pdf";
            ContentDisposition contentDisposition = new ContentDisposition();
            contentDisposition.FileName = "UserAdoption.pdf";
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

        protected void UserAdoptionPivotGrid_Unload(object sender, EventArgs e)
        {
            VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("UserAdoption|UserAdoptionPivotGrid", UserAdoptionPivotGrid.OptionsPager.RowsPerPage.ToString(), Convert.ToInt32(Session["UserID"]));
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        }
    }
}