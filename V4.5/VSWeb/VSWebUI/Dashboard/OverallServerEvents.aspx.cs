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
using DevExpress.Web;
using DevExpress.XtraCharts;

namespace VSWebUI.Dashboard
{
    public partial class OverallServerEvents : System.Web.UI.Page
    {
		protected void Page_PreInit(object sender, EventArgs e)
		{

			this.Master.contentCallEvent += new EventHandler(Master_ButtonClick);
		}

		private void Master_ButtonClick(object sender, EventArgs e)
		{

		}
        public bool exactmatch;

        protected void Page_Load(object sender, EventArgs e)
        {

           
            if (!IsPostBack)
            {
				fillcombo();

                if (Session["UserPreferences"] != null)
                {
                    DataTable UserPreferences = (DataTable)Session["UserPreferences"];
                    foreach (DataRow dr in UserPreferences.Rows)
                    {
                        if (dr[1].ToString() == "OverallServerAlerts|EventsHistory")
                        {
							EventsHistory.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                        }
                    }
                }
                //5/6/2015 NS added for VSPLUS-1622
            }
			ReloadGrid();
        }

        protected void DateParamEdit_DateChanged(object sender, EventArgs e)
        {
            ReloadGrid();
        }

        public void ReloadGrid()
        {
            string date;
            exactmatch = false;
           
            if (startDate.Value.ToString() == "")
            {
                date = DateTime.Now.ToString();
            }
            else
            {
                date = startDate.Value.ToString();
            }
            DateTime dtval = Convert.ToDateTime(date);
            DataTable dt = new DataTable();
			if (EventTypeComboBox.SelectedIndex != -1)
			{
				if (EventTypeComboBox.SelectedIndex ==0)
					dt = VSWebBL.ConfiguratorBL.ELSBL.Ins.GetEventHistory();
				else
					dt = VSWebBL.ConfiguratorBL.ELSBL.Ins.GetEventHistoryByLog(EventTypeComboBox.SelectedItem.Value.ToString());
			}
			else
			{
				dt = VSWebBL.ConfiguratorBL.ELSBL.Ins.GetEventHistory(dtval.Month.ToString(), dtval.Year.ToString());
			}
				EventsHistory.DataSource = dt;
				EventsHistory.DataBind();
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

        protected void EventSettings_ItemClick(object source, DevExpress.Web.MenuItemEventArgs e)
        {
           if (e.Item.Name == "ExportXLSItem")
            {
				if (startDate.Value != "")
				{
					ServerGridViewExporter.FileName = "EventsHistory" + "_" + startDate.Value;
					ServerGridViewExporter.WriteXlsToResponse();
				}
				else
				{
					ServerGridViewExporter.FileName = "EventsHistory" + "_" + DateTime.Now.ToString();
					ServerGridViewExporter.WriteXlsToResponse();
				}
            }
			else if (e.Item.Name == "ExportXLSXItem")
			{
				if (startDate.Value != "")
				{
					ServerGridViewExporter.FileName = "EventsHistory" + "_" + startDate.Value;
					ServerGridViewExporter.WriteXlsxToResponse();
				}
				else
				{
					ServerGridViewExporter.FileName = "EventsHistory" + "_" + DateTime.Now.ToString();
					ServerGridViewExporter.WriteXlsxToResponse();
				}
			}
			else if (e.Item.Name == "ExportPDFItem")
			{
				if (startDate.Value != "")
				{
					ServerGridViewExporter.FileName = "EventsHistory" + "_" + startDate.Value;
					//ServerGridViewExporter.WriteXlsxToResponse();
				}
				else
				{
					ServerGridViewExporter.FileName = "EventsHistory" + "_" + DateTime.Now.ToString();
					//ServerGridViewExporter.WriteXlsxToResponse();
				}

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



		protected void EventsHistoryGridView_PageSizeChanged(object sender, EventArgs e)
        {
            //ProfilesGridView.PageIndex;
			VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("OverallServerAlerts|EventsHistory", EventsHistory.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        }

        //11/7/2014 NS added - the function below will format the table to fit on one page width wise
        public static void WriteResponse(HttpResponse response, byte[] filearray, string type)
        {
            response.ClearContent();
            response.Buffer = true;
            response.Cache.SetCacheability(HttpCacheability.Private);
            response.ContentType = "application/pdf";
            ContentDisposition contentDisposition = new ContentDisposition();
            contentDisposition.FileName = "EventsHistory.pdf";
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
		protected void EventsHistory_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
		{
			if (e.DataColumn.FieldName == "EntryType" && e.CellValue.ToString() == "Error")
			{
				e.Cell.BackColor = System.Drawing.Color.Red;
				e.Cell.ForeColor = System.Drawing.Color.White;
			}
			else if (e.DataColumn.FieldName == "EntryType" && e.CellValue.ToString() == "Warning")
			{
				e.Cell.BackColor = System.Drawing.Color.Yellow;
				e.Cell.ForeColor = System.Drawing.Color.Black;
			}
		}
        public void fillcombo()
        {
			EventTypeComboBox.Items.Add("All Logs", "All Logs");
			EventTypeComboBox.Items.Add("Application", "Application");
			EventTypeComboBox.Items.Add("Security", "Security");
			EventTypeComboBox.Items.Add("Setup", "Setup");
			EventTypeComboBox.Items.Add("System", "System");

        }

		protected void EventTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
			if (EventTypeComboBox.SelectedIndex != -1)
            {
                DataTable dt = new DataTable();
				if (EventTypeComboBox.SelectedIndex == 0)
					dt = VSWebBL.ConfiguratorBL.ELSBL.Ins.GetEventHistory();
				else
				dt = VSWebBL.ConfiguratorBL.ELSBL.Ins.GetEventHistoryByLog(EventTypeComboBox.SelectedItem.Value.ToString());
				EventsHistory.DataSource = dt;
				EventsHistory.DataBind();
            }
        }
    }
}