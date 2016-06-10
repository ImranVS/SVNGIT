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
    public partial class IBMConnectionsCommunityActivityRpt : System.Web.UI.Page
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
            SetCommunityActivityGrid();
        }

        private void SetCommunityActivityGrid()
        {
            DataTable dt = new DataTable();
            dt = VSWebBL.DashboardBL.ConnectionsBL.Ins.GetCommunityActivity("");
            CommunityActivityPivotGrid.DataSource = dt;
            CommunityActivityPivotGrid.DataBind();
            CommunityActivityPivotGrid.OptionsView.ShowRowTotals = false;
        }

        protected void CommunityActivityPivotGrid_CustomFieldSort(object sender, DevExpress.Web.ASPxPivotGrid.PivotGridCustomFieldSortEventArgs e)
        {
            if (e.Field.FieldName != "LastDate") return;

            int val1 = Convert.ToInt32(e.GetListSourceColumnValue(e.ListSourceRowIndex1, "OrdNum"));
            int val2 = Convert.ToInt32(e.GetListSourceColumnValue(e.ListSourceRowIndex2, "OrdNum"));

            if (Convert.ToInt32(val1) > Convert.ToInt32(val2))
                e.Result = 1;
            else
                if (Convert.ToInt32(val1) == Convert.ToInt32(val2))
                    e.Result = Comparer<Int32>.Default.Compare(Convert.ToInt32(val1), Convert.ToInt32(val2));
                else
                    e.Result = -1;

            e.Handled = true;
        }

        protected void ASPxMenu1_ItemClick(object source, DevExpress.Web.MenuItemEventArgs e)
        {
            if (e.Item.Name == "ExportXLSItem")
            {
                ServerGridViewExporter.ExportXlsToResponse("CommunityActivity.xls");
            }
            else if (e.Item.Name == "ExportXLSXItem")
            {
                ServerGridViewExporter.ExportXlsxToResponse("CommunityActivity.xlsx");
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
            contentDisposition.FileName = "CommunityActivity.pdf";
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