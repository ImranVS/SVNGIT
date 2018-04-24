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
    public partial class AdHocRpt : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            string selectedStat = "";
            string selectedStatType = "";
            string selectedDateInd = "";
            string selectedSumType = "";
            if (this.StatComboBox.SelectedIndex >= 0)
            {
                selectedStat = this.StatComboBox.SelectedItem.Value.ToString();
            }
            if (this.StatTypeComboBox.SelectedIndex >= 0)
            {
                selectedStatType = this.StatTypeComboBox.SelectedItem.Value.ToString();
            }
            //11/21/2014 NS commented out
            /*
            if (this.DateRangeComboBox.SelectedIndex >= 0)
            {
                selectedDateInd = this.DateRangeComboBox.SelectedItem.Value.ToString();
            }
            */
            if (this.SummaryTypeComboBox.SelectedIndex >= 0)
            {
                selectedSumType = this.SummaryTypeComboBox.SelectedItem.Value.ToString();
            }
            //6/16/2015 NS modified for VSPLUS-1656
            if (OptimizedRadioButtonList.SelectedIndex == 0)
            {
                ASPxPivotGrid1.Visible = true;
                titledisp.Style.Value = "display: block";
                if (selectedStat == "")
                {
                    titledisp.InnerHtml = "Report for &lt;Statistic&gt;";
                }
                else
                {
                    titledisp.InnerHtml = "Report for " + selectedStat;
                }
                ASPxMenu1.ClientVisible = true;
                ASPxDocumentViewer1.Visible = false;
                dt = VSWebBL.ReportsBL.ReportsBL.Ins.GetDominoStatValues(selectedStat, selectedStatType, dtPick.FromDate, dtPick.ToDate);
                if (dt.Rows.Count > 0)
                {
					
                    ASPxPivotGrid1.Fields.Clear();
                    ASPxPivotGrid1.DataSource = dt;
					
					ASPxPivotGrid1.Fields.Add("ServerName", DevExpress.XtraPivotGrid.PivotArea.RowArea);
                    ASPxPivotGrid1.Fields.Add("StatValue", DevExpress.XtraPivotGrid.PivotArea.DataArea);
                    ASPxPivotGrid1.Fields.Add("MonthDay", DevExpress.XtraPivotGrid.PivotArea.ColumnArea);
                    ASPxPivotGrid1.Fields["MonthDay"].Caption = "Month/Day/Year";
                    ASPxPivotGrid1.Fields["MonthDay"].ValueFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
                    ASPxPivotGrid1.Fields["MonthDay"].ValueFormat.FormatString = "MM/dd/yyyy";
                    ASPxPivotGrid1.Fields["ServerName"].Caption = "Server";
					

					

                    ASPxPivotGrid1.OptionsView.HorizontalScrollBarMode = DevExpress.Web.ScrollBarMode.Auto;
					
                    if (selectedSumType == "0")
                    {
                        ASPxPivotGrid1.Fields["StatValue"].SummaryType = DevExpress.Data.PivotGrid.PivotSummaryType.Sum;
                        ASPxPivotGrid1.Fields["ServerName"].SummaryType = DevExpress.Data.PivotGrid.PivotSummaryType.Sum;
                    }
                    else
                    {
                        ASPxPivotGrid1.Fields["StatValue"].SummaryType = DevExpress.Data.PivotGrid.PivotSummaryType.Average;
                        ASPxPivotGrid1.Fields["ServerName"].SummaryType = DevExpress.Data.PivotGrid.PivotSummaryType.Average;
                        ASPxPivotGrid1.Fields["StatValue"].GrandTotalCellFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                        ASPxPivotGrid1.Fields["StatValue"].GrandTotalCellFormat.FormatString = "f2";
                    }
                }
            }
            else
            {
                ASPxPivotGrid1.Visible = false;
                titledisp.Style.Value = "display: none";
                ASPxMenu1.ClientVisible = false;
                ASPxDocumentViewer1.Visible = true;
                DashboardReports.AdHocXtraRpt report = new DashboardReports.AdHocXtraRpt();
                report.Parameters["StatName"].Value = selectedStat;
                report.Parameters["StatType"].Value = selectedStatType;
                //11/21/2014 NS modified
                //report.Parameters["DateInd"].Value = selectedDateInd;
                report.Parameters["StartDate"].Value = dtPick.FromDate;
                report.Parameters["EndDate"].Value = dtPick.ToDate;
                report.Parameters["StatSummaryType"].Value = selectedSumType;
                report.CreateDocument();
                ASPxDocumentViewer1.Report = report;
                ASPxDocumentViewer1.DataBind();
            }
            if (!IsPostBack)
            {
                fillcombo(selectedStatType);
            }
        }

        public void fillcombo(string stattype)
        {
            DataTable dt = new DataTable();
            dt = VSWebBL.ReportsBL.ReportsBL.Ins.GetDominoStatList(stattype);
            StatComboBox.DataSource = dt;
            StatComboBox.TextField = "StatName";
            StatComboBox.ValueField = "StatName";
            StatComboBox.DataBind();
        }

        protected void StatTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            fillcombo(StatTypeComboBox.SelectedItem.Value.ToString());
            StatComboBox.SelectedIndex = -1;
        }

        protected void ReptBtn_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Configurator/Reports.aspx?M=" + Request.QueryString["M"], false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
            Context.ApplicationInstance.CompleteRequest();
        }

        protected void ASPxMenu1_ItemClick(object source, DevExpress.Web.MenuItemEventArgs e)
        {
            if (e.Item.Name == "ExportXLSItem")
            {
                //9/30/2015 NS modified for VSPLUS-2218
                if (ASPxPivotGrid1.DataSource != null)
                {
                    ServerPivotGridExporter.ExportXlsToResponse("AdHocRpt.xls");
                }
            }
            else if (e.Item.Name == "ExportXLSXItem")
            {
                //9/30/2015 NS modified for VSPLUS-2218
                if (ASPxPivotGrid1.DataSource != null)
                {
                    ServerPivotGridExporter.ExportXlsxToResponse("AdHocRpt.xlsx");
                }
            }
            else if (e.Item.Name == "ExportPDFItem")
            {
                //ServerGridViewExporter.Landscape = true;
                //9/30/2015 NS modified for VSPLUS-2218
                if (ASPxPivotGrid1.DataSource != null)
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        PrintableComponentLink pcl = new PrintableComponentLink(new PrintingSystem());
                        pcl.Component = ServerPivotGridExporter;
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
        }

        public static void WriteResponse(HttpResponse response, byte[] filearray, string type)
        {
            response.ClearContent();
            response.Buffer = true;
            response.Cache.SetCacheability(HttpCacheability.Private);
            response.ContentType = "application/pdf";
            ContentDisposition contentDisposition = new ContentDisposition();
            contentDisposition.FileName = "AdHocRpt.pdf";
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