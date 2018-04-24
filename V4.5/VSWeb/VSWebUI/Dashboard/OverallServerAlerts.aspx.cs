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

namespace VSWebUI.Dashboard
{
    public partial class OverallServerAlerts : System.Web.UI.Page
    {
		protected void Page_PreInit(object sender, EventArgs e)
		{

			this.Master.contentCallEvent += new EventHandler(Master_ButtonClick);
		}

		private void Master_ButtonClick(object sender, EventArgs e)
		{

		}
        public bool exactmatch;
        string date;
        DateTime dtval;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["UserPreferences"] != null)
                {
                    DataTable UserPreferences = (DataTable)Session["UserPreferences"];
                    foreach (DataRow dr in UserPreferences.Rows)
                    {
                        if (dr[1].ToString() == "OverallServerAlerts|AlertsHistory")
                        {
							AlertsHistory.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                        }
                    }
                }

                //5/6/2015 NS added for VSPLUS-1622
                fillcombo();
            }
            //13/4/2016 Sowmya added for VSPLUS-2749
          
                ReloadGrid();
            
        }

        protected void DateParamEdit_DateChanged(object sender, EventArgs e)
        {
            ReloadGrid();
        }

        public void ReloadGrid()
        {
           
            exactmatch = false;
            //12/4/2013 NS modified - added jQuery month/year control
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
            if (startDate.Value.ToString() == "")
            {
                date = DateTime.Now.ToString();
            }
            else
            {
                date = startDate.Value.ToString();
            }
             dtval = Convert.ToDateTime(date);
            DataTable dt = new DataTable();
            //6/12/2015 NS modified for VSPLUS-1622
            if (AlertDefComboBox.SelectedIndex != -1)
            {
                // 3/14/2016 Durga Addded for VSPLUS-2704
				if (AlertDefComboBox.Text == "All Open Alerts")
                {

                    dt = VSWebBL.ConfiguratorBL.AlertsBL.Ins.GetAllOpenAlers(dtval.Month.ToString(), dtval.Year.ToString());
                    AlertsHistory.DataSource = dt;
                    AlertsHistory.DataBind();
                }
                else
                {
                dt = VSWebBL.ConfiguratorBL.AlertsBL.Ins.GetAlertHistoryByAlertID(AlertDefComboBox.SelectedItem.Value.ToString());
                }
            }
            else
            {
                dt = VSWebBL.ConfiguratorBL.AlertsBL.Ins.GetAlertHistory(dtval.Month.ToString(), dtval.Year.ToString());
            }
			AlertsHistory.DataSource = dt;
			AlertsHistory.DataBind();
           
            AlertsHistory.SortBy(AlertsHistory.Columns["DateTimeOfAlert"] as GridViewDataColumn, DevExpress.Data.ColumnSortOrder.Descending);
       
        }

        protected void ExportXlsButton_Click(object sender, EventArgs e)
        {
		
			//AlertsHistory.SettingsText.Title = DateTime.Now.ToString();
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

        protected void AlertSettings_ItemClick(object source, DevExpress.Web.MenuItemEventArgs e)
        {
            if (e.Item.Name == "AlertSettingsItem")
            {
                Response.Redirect("~/Configurator/Alert_Settings.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
            }
            else if (e.Item.Name == "ExportXLSItem")
            {
				if (startDate.Value != "")
				{
					ServerGridViewExporter.FileName = "AlertsHistory" + "_" + startDate.Value;
					ServerGridViewExporter.WriteXlsToResponse();
				}
				else
				{
					ServerGridViewExporter.FileName = "AlertsHistory" + "_" + DateTime.Now.ToString();
					ServerGridViewExporter.WriteXlsToResponse();
				}
            }
			else if (e.Item.Name == "ExportXLSXItem")
			{
				if (startDate.Value != "")
				{
					ServerGridViewExporter.FileName = "AlertsHistory" + "_" + startDate.Value;
					ServerGridViewExporter.WriteXlsxToResponse();
				}
				else
				{
					ServerGridViewExporter.FileName = "AlertsHistory" + "_" + DateTime.Now.ToString();
					ServerGridViewExporter.WriteXlsxToResponse();
				}
			}
			else if (e.Item.Name == "ExportPDFItem")
			{
				if (startDate.Value != "")
				{
					ServerGridViewExporter.FileName = "AlertsHistory" + "_" + startDate.Value;
					//ServerGridViewExporter.WriteXlsxToResponse();
				}
				else
				{
					ServerGridViewExporter.FileName = "AlertsHistory" + "_" + DateTime.Now.ToString();
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



        protected void AlertsGridView_PageSizeChanged(object sender, EventArgs e)
        {
            //ProfilesGridView.PageIndex;
			VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("OverallServerAlerts|AlertsHistory", AlertsHistory.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
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
            contentDisposition.FileName = "AlertsHistory.pdf";
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

        //5/6/2015 NS added for VSPLUS-1622
        public void fillcombo()
        {
            DataTable dt = new DataTable();
            DataRow dr, dr1;
            dt = VSWebBL.ConfiguratorBL.AlertsBL.Ins.GetAlertNames();
            dr = dt.NewRow();
            dr["AlertKey"] = 0;
            dr["AlertName"] = "All Alerts";
            dt.Rows.InsertAt(dr, 0);
            dr1 = dt.NewRow();
           
            // 27/7/2016 Durga Modified  for VSPLUS-3125
            if (dt.Rows.Count > 0)
            {
                var maxValue = dt.AsEnumerable()
             .Select(x => Convert.ToInt32(x.Field<int>("AlertKey")))
             .DefaultIfEmpty(0)
             .Max(x => x);

                dr1["AlertKey"] = maxValue + 1;
             
            }
            else
            {
                dr1["AlertKey"] = 1;
               
            }
                dr1["AlertName"] = "All Open Alerts";
                 dt.Rows.InsertAt(dr1, 1);
                AlertDefComboBox.DataSource = dt;
                AlertDefComboBox.TextField = "AlertName";
                AlertDefComboBox.ValueField = "AlertKey";
                AlertDefComboBox.DataBind();
                AlertDefComboBox.SelectedIndex = 0;
           
        }

        protected void AlertDefComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            if (AlertDefComboBox.SelectedIndex != -1)
            {// 3/14/2016 Durga Addded for VSPLUS-2704
                if (AlertDefComboBox.Text == "All Open Alerts")
                {
                    dt = VSWebBL.ConfiguratorBL.AlertsBL.Ins.GetAllOpenAlers(dtval.Month.ToString(), dtval.Year.ToString());
                    AlertsHistory.DataSource = dt;
                    AlertsHistory.DataBind();
                }
                else
                {
               
                dt = VSWebBL.ConfiguratorBL.AlertsBL.Ins.GetAlertHistoryByAlertID(AlertDefComboBox.SelectedItem.Value.ToString());
                
                AlertsHistory.DataSource = dt;
                AlertsHistory.DataBind();
                }
            }
        }
    }
}