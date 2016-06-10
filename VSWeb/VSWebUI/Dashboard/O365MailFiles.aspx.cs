using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DevExpress.Web;
using System.Web.UI.HtmlControls;
using DevExpress.XtraCharts;
using System.Net.Mime;
using System.IO;
using DevExpress.XtraPrinting;

namespace VSWebUI
{
    public partial class O365MailFiles : System.Web.UI.Page
    {//2/25/2016 Durga Added for  VSPLUS-2611
		protected void Page_PreInit(object sender, EventArgs e)
        {
           
            this.Master.contentCallEvent += new EventHandler(Master_ButtonClick);
        }

		private void Master_ButtonClick(object sender, EventArgs e)
		{
		}
        string tab = "", page = "O365MailFiles.aspx", control = "TypeComboBox";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                
				FillTypeCombobox(page, control);
		
                HtmlGenericControl body = (HtmlGenericControl)Master.FindControl("Body1");
                body.Attributes.Add("onResize", "Resized()");
                body.Attributes.Add("onload", "DoCallback()");

                if (Session["UserPreferences"] != null)
                {
                    DataTable UserPreferences = (DataTable)Session["UserPreferences"];
                    foreach (DataRow dr in UserPreferences.Rows)
                    {
                        if (dr[1].ToString() == "O365MailFiles|Office365Grid")
                        {
                            Office365Grid.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                        }
                    }
                }
            }
          
            FillMailFilesfromSession();

            titleDiv.InnerHtml = "Mail Files Sorted By Display Name for Office365";

            DropDownSelectionMethod();
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
		public static void WriteResponse(HttpResponse response, byte[] filearray, string type)
		{
			response.ClearContent();
			response.Buffer = true;
			response.Cache.SetCacheability(HttpCacheability.Private);
			response.ContentType = "application/pdf";
			ContentDisposition contentDisposition = new ContentDisposition();
			contentDisposition.FileName = "LyncServerStats.pdf";
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
        public void FillMailFiles()
        {

            DataTable dtMailFiles = VSWebBL.DashboardBL.Office365BL.Ins.GetOffice365Mails();
            dtMailFiles.PrimaryKey = new DataColumn[] { dtMailFiles.Columns["ID"] };
            Office365Grid.DataSource = dtMailFiles;
            Office365Grid.DataBind();
            ((GridViewDataColumn)Office365Grid.Columns["Server"]).GroupBy();
            Session["Office365MailFiles"] = dtMailFiles;
                  
        }
        public void FillMailFilesfromSession()
        {

            DataTable dtMailFiles = new DataTable();
            if (Session["Office365MailFiles"] != "" && Session["Office365MailFiles"] != null)
                dtMailFiles = (DataTable)Session["Office365MailFiles"];

            Office365Grid.DataSource = dtMailFiles;
            Office365Grid.DataBind();
        

        }

	
        protected void MailFileGridView_PageSizeChanged(object sender, EventArgs e)
        {
            //ProfilesGridView.PageIndex;
            VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("MailFiles|MailFileGridView", Office365Grid.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        }

        protected void btnCollapse_Click(object sender, EventArgs e)
        {
            if (btnCollapse.Text == "Collapse All Rows")
            {
                Office365Grid.CollapseAll();
                btnCollapse.Image.Url = "~/images/icons/add.png";
                btnCollapse.Text = "Expand All Rows";
            }
            else
            {
                Office365Grid.ExpandAll();
                btnCollapse.Image.Url = "~/images/icons/forbidden.png";
                btnCollapse.Text = "Collapse All Rows";
            }   
        }
		public void FillTypeCombobox(string page, string control)
		{

            DataTable Typetab = VSWebBL.DashboardBL.KeyMetricsBL.Ins.GetspecificServerType(page, control);
			TypeComboBox.DataSource = Typetab;
			TypeComboBox.TextField = "ServerType";
			TypeComboBox.DataBind();
			//2/16/1016 Durga modified for VSPLUS-2611
			if (Request.QueryString["ServerType"] != null)
			{
				TypeComboBox.SelectedItem = TypeComboBox.Items.FindByText(Request.QueryString["ServerType"].ToString());
			}
		}
		protected void TypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
            DropDownSelectionMethod();
		}

        public void FillOffice365Grid()
        {

            DataTable dtMailFiles = VSWebBL.DashboardBL.Office365BL.Ins.GetOffice365Mails();
            dtMailFiles.PrimaryKey = new DataColumn[] { dtMailFiles.Columns["ID"] };
            Office365Grid.DataSource = dtMailFiles;
            Office365Grid.DataBind();
            ((GridViewDataColumn)Office365Grid.Columns["Server"]).GroupBy();


        }
        protected void Office365Grid_PageSizeChanged(object sender, EventArgs e)
        {
            //ProfilesGridView.PageIndex;
            VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("O365MailFiles|Office365Grid", Office365Grid.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        }
        public void DropDownSelectionMethod()
        {
            if (TypeComboBox.Text == "Domino")
            {
                Response.Redirect("~/Dashboard/MailFiles.aspx?MItem=1&ServerType=" + TypeComboBox.Text, false);
            }
            else if (TypeComboBox.Text == "Exchange")
            {
                //  Response.Redirect("~/Dashboard/O365MailFiles.aspx", false);
                Response.Redirect("~/Dashboard/ExchangeMailFiles.aspx?ServerType=" + TypeComboBox.Text, false);
            }
            else
            {
                FillOffice365Grid();
            }
        }
        void SetItemCount()
        {
            int itemCount = (int)Office365Grid.GetTotalSummaryValue(Office365Grid.TotalSummary["Server"]);
            Office365Grid.SettingsPager.Summary.Text = "Page {0} of {1} (" + itemCount.ToString() + " items)";
        }
        protected void DataBound(object sender, EventArgs e)
        {
            SetItemCount();
        }
    }
}