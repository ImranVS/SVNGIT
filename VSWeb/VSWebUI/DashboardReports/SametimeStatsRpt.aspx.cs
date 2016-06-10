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
    public partial class SametimeStatsRpt : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            FillStatsPivotGrid();
            // 17/05/2016 Durga Addded for VSPLUS-2969
            if (Session["UserPreferences"] != null)
            {
                DataTable UserPreferences = (DataTable)Session["UserPreferences"];
                foreach (DataRow dr in UserPreferences.Rows)
                {
                    if (dr[1].ToString() == "SametimeStatsRpt|SametimeStatsPivotGrid")
                    {
                        SametimeStatsPivotGrid.OptionsPager.RowsPerPage = Convert.ToInt32(dr[2]);
                    }
                }
            }
        }

        protected void ASPxMenu1_ItemClick(object source, DevExpress.Web.MenuItemEventArgs e)
        {
            if (e.Item.Name == "ExportXLSItem")
            {
                ServerGridViewExporter.ExportXlsToResponse("SametimeStats.xls");
            }
            else if (e.Item.Name == "ExportXLSXItem")
            {
                ServerGridViewExporter.ExportXlsxToResponse("SametimeStats.xlsx");
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
            contentDisposition.FileName = "SametimeStats.pdf";
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

        public void FillStatsPivotGrid()
        {
            string fromdate = dtPick.FromDate;
            string todate = dtPick.ToDate;
            DataTable dt = new DataTable();
            dt = VSWebBL.DashboardBL.SametimeHealthBL.Ins.GetSametimeSummaryStats(fromdate, todate,"");
            SametimeStatsPivotGrid.DataSource = dt;
            SametimeStatsPivotGrid.DataBind();
        }

        protected void ASPxButton1_Click(object sender, EventArgs e)
        {
            FillStatsPivotGrid();
        }
        // 17/05/2016 Durga Addded for VSPLUS-2969
        protected void SametimeStatsPivotGrid_Unload(object sender, EventArgs e)
        {
            VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("SametimeStatsRpt|SametimeStatsPivotGrid", SametimeStatsPivotGrid.OptionsPager.RowsPerPage.ToString(), Convert.ToInt32(Session["UserID"]));
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        }
    }
}