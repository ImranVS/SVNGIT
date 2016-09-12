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
    public partial class IBMConnectionsUserActivityRpt : System.Web.UI.Page
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
            SetUserActivityGrid();
            if (Session["UserPreferences"] != null)
            {
                DataTable UserPreferences = (DataTable)Session["UserPreferences"];
                foreach (DataRow dr in UserPreferences.Rows)
                {
                    if (dr[1].ToString() == "UserActivity|UserActivityPivotGrid")
                    {
                        UserActivityPivotGrid.OptionsPager.RowsPerPage = Convert.ToInt32(dr[2]);
                    }
                }
            }
        }

        private void SetUserActivityGrid()
        {
            DataTable dt = new DataTable();
            dt = VSWebBL.DashboardBL.ConnectionsBL.Ins.GetUserActivity("");
            UserActivityPivotGrid.DataSource = dt;
            UserActivityPivotGrid.DataBind();
            UserActivityPivotGrid.OptionsView.ShowRowTotals = false;
        }

        protected void ASPxMenu1_ItemClick(object source, DevExpress.Web.MenuItemEventArgs e)
        {
            if (e.Item.Name == "ExportXLSItem")
            {
                ServerGridViewExporter.ExportXlsToResponse("UserActivity.xls");
            }
            else if (e.Item.Name == "ExportXLSXItem")
            {
                ServerGridViewExporter.ExportXlsxToResponse("UserActivity.xlsx");
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
            contentDisposition.FileName = "UserActivity.pdf";
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

        protected void UserActivityPivotGrid_CustomFieldSort(object sender, DevExpress.Web.ASPxPivotGrid.PivotGridCustomFieldSortEventArgs e)
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

        protected void UserActivityPivotGrid_Unload(object sender, EventArgs e)
        {
            VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("UserActivity|UserActivityPivotGrid", UserActivityPivotGrid.OptionsPager.RowsPerPage.ToString(), Convert.ToInt32(Session["UserID"]));
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        }
    }
}