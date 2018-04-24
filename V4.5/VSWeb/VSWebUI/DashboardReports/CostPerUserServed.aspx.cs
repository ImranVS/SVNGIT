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
    public partial class CostPerUserServed : System.Web.UI.Page
    // 3/28/2016 Durga Addded for VSPLUS-2695
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
                        if (dr[1].ToString() == "CostPerUserServed|CostPerUserPivotGrid")
                        {
                            CostPerUserPivotGrid.OptionsPager.RowsPerPage = Convert.ToInt32(dr[2]);
                        }
                    }
                }
           
        }

        protected void ASPxMenu1_ItemClick(object source, DevExpress.Web.MenuItemEventArgs e)
        {
            if (e.Item.Name == "ExportXLSItem")
            {
                ServerGridViewExporter.ExportXlsToResponse("CostPerUserServedGrid.xls");
            }
            else if (e.Item.Name == "ExportXLSXItem")
            {
                ServerGridViewExporter.ExportXlsxToResponse("CostPerUserServedGrid.xlsx");
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
            contentDisposition.FileName = "CostPerUserServedGrid.pdf";
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
            DataRow row;
            dt = VSWebBL.DashboardBL.KeyMetricsBL.Ins.GetCostPerUserServersGridForDomino(fromdate, todate);
           
            DataTable dttotal = new DataTable();
          
            DataTable exchangedt = new DataTable();
             DataTable Exchangeinfo  = new DataTable();
            exchangedt = VSWebBL.DashboardBL.KeyMetricsBL.Ins.GetCostPerUserServerGrid(fromdate, todate, "");
            exchangedt.Columns.Add("StatName", typeof(string));
            exchangedt.Columns.Add("costperday", typeof(decimal));
            exchangedt.Columns.Add("CostPerUser", typeof(double));
            exchangedt.Columns.Add("MonthlyOperatingCost", typeof(decimal));
            for (int j = 0; j < exchangedt.Rows.Count; j++)
            {
               exchangedt.Rows[j]["StatName"]="User Count";
               Exchangeinfo = VSWebBL.DashboardBL.KeyMetricsBL.Ins.GetCostPerdayforExchange(exchangedt.Rows[j]["ServerName"].ToString(), exchangedt.Rows[j]["Date"].ToString(), Convert.ToInt32(exchangedt.Rows[j]["StatValue"].ToString()));
               if (Exchangeinfo.Rows.Count > 0)
               {
                   exchangedt.Rows[j]["CostPerUser"] = Exchangeinfo.Rows[0]["CostPerUser"].ToString();
                   exchangedt.Rows[j]["costperday"] = Exchangeinfo.Rows[0]["costperday"].ToString();
               }
             }
            dttotal = exchangedt.Copy();
            dttotal.Merge(dt);

            int count = dttotal.Rows.Count;
            for (int i = 0; i < count; i++)
            {

                row = dttotal.NewRow();
                row["StatValue"] = Math.Round(Convert.ToDecimal(dttotal.Rows[i]["costperday"].ToString() == "" ? "0" : dttotal.Rows[i]["costperday"].ToString()), 2);
                row["ServerName"] = dttotal.Rows[i]["ServerName"].ToString();
                row["Date"] = dttotal.Rows[i]["Date"].ToString();
                row["StatName"] = "Cost Per Day";

                dttotal.Rows.Add(row);
                row = dttotal.NewRow();

                row["StatValue"] = Math.Round(Convert.ToDecimal(dttotal.Rows[i]["CostPerUser"].ToString() == "" ? "0" : dttotal.Rows[i]["CostPerUser"].ToString()), 2);
                row["ServerName"] = dttotal.Rows[i]["ServerName"].ToString();
                row["Date"] = dttotal.Rows[i]["Date"].ToString();
                row["StatName"] = "Cost Per User";
                dttotal.Rows.Add(row);

            }
            CostPerUserPivotGrid.DataSource = dttotal;
            CostPerUserPivotGrid.DataBind();
        }

        protected void ASPxButton1_Click(object sender, EventArgs e)
        {
            FillStatsPivotGrid();
        }

        // 17/05/2016 Durga Addded for VSPLUS-2969
        protected void CostPerUserPivotGrid_Unload(object sender, EventArgs e)
        {
            VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("CostPerUserServed|CostPerUserPivotGrid", CostPerUserPivotGrid.OptionsPager.RowsPerPage.ToString(), Convert.ToInt32(Session["UserID"]));
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        }

      
    }
}