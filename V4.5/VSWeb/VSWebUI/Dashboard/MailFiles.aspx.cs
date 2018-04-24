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
    public partial class MailFiles : System.Web.UI.Page
    {
		protected void Page_PreInit(object sender, EventArgs e)
		{

			this.Master.contentCallEvent += new EventHandler(Master_ButtonClick);
		}

		private void Master_ButtonClick(object sender, EventArgs e)
		{

		}
		string tab = "",  control = "TypeComboBox";
        protected void Page_Load(object sender, EventArgs e)
        {

            //Mukund 11Apr14
            tab = Request.QueryString["MItem"].ToString();
			string page = "MailFiles.aspx?MItem="+ tab;
            if (!IsPostBack)
            {
                //1/15/2014 NS added
                serverCombo();
				FillTypeCombobox(page, control);
				
                FillMailFiles(tab);
                //1/2/2014 NS added
                HtmlGenericControl body = (HtmlGenericControl)Master.FindControl("Body1");
                body.Attributes.Add("onResize", "Resized()");
                body.Attributes.Add("onload", "DoCallback()");

                if (Session["UserPreferences"] != null)
                {
                    DataTable UserPreferences = (DataTable)Session["UserPreferences"];
                    foreach (DataRow dr in UserPreferences.Rows)
                    {
                        if (dr[1].ToString() == "MailFiles|MailFileGridView")
                        {
                            MailFileGridView.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                        }
                    }
                }
            }
            //4/8/2014 NS moved the line below out of the !IsPostback block for VSPLUS-485
         //   tab = Request.QueryString["MItem"].ToString(); //Commented by Mukund 11Apr14, this should be first line in pageload to fill data first time too
            FillMailFilesfromSession();
            if (tab == "1")
            {
                //11/10/2014 NS modified
                //MailFileRoundPanel.HeaderText = "Mail Files Sorted By File Name";
                titleDiv.InnerHtml = "Mail Files Sorted By File Name for Domino";
                MailFileGridView.Columns["File Name"].VisibleIndex = 0;
                //1/3/2014 NS added
                BiggestQuotaWebChart.Visible = false;
                //1/15/2014 NS added
                MailTemplateWebChart.Visible = false;
                ServerComboBox.Visible = false;
            }
            if (tab == "2")
            {
                //11/10/2014 NS modified
                //MailFileRoundPanel.HeaderText = "Mail Files Sorted By Inbox Count";
                titleDiv.InnerHtml = "Mail Files Sorted By Inbox Count";
                MailFileGridView.Columns["Inbox DocCount"].VisibleIndex = 0;
                //1/3/2014 NS added
                BiggestQuotaWebChart.Visible = false;
                //1/15/2014 NS added
                MailTemplateWebChart.Visible = false;
                ServerComboBox.Visible = false;
				TypeComboBox.Visible = false;
				TypeLabel.Visible = false;
            }
            if (tab == "3")
            {
                //1/23/2014 NS modified - hide grid for the % of Quota report
                //11/10/2014 NS modified
                //MailFileRoundPanel.Visible = false;
                MailFileGridView.Visible = false;
                btnCollapse.Visible = false;
                //MailFileRoundPanel.HeaderText = "Mail Files Sorted By Percent of Quota";
                //12/16/2013 NS modified
                //MailFileGridView.Columns["Quota"].VisibleIndex = 0;
                //MailFileGridView.Columns["PercentQuota"].VisibleIndex = 0;
                //1/2/2014 NS added
                SetBarGraphForTopQuota("");
                //1/15/2014 NS added
                MailTemplateWebChart.Visible = false;
                //4/8/2014 NS modified for VSPLUS-485
                ServerComboBox.Visible = true;
				TypeComboBox.Visible = false;
				TypeLabel.Visible = false;
				ExportMenu.Visible = false;
            }
            //1/15/2014 NS added - Mail Template report
            if (tab == "4")
            {
                BiggestQuotaWebChart.Visible = false;
                //11/10/2014 NS modified
                //MailFileRoundPanel.Visible = false;
                btnCollapse.Visible = false;
                MailFileGridView.Visible = false;
                SetPieChartForMailTemplates("");
				TypeComboBox.Visible = false;
				TypeLabel.Visible = false;
				ExportMenu.Visible = false;
            }
            //2/25/2016 Durga Modified for  VSPLUS-2611
            DropDownSelectionMethod();
            if (TypeComboBox.SelectedItem == null)
            {
                MailFileGridView.Visible = false;
            }
            else
            {
                MailFileGridView.Visible = true;

            }
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
        public void FillMailFiles(string tab)
        {
            
            DataTable dtMailFiles = VSWebBL.DashboardBL.mailFileBL.Ins.GetMails(tab);
            dtMailFiles.PrimaryKey = new DataColumn[] { dtMailFiles.Columns["ID"] };
            MailFileGridView.DataSource = dtMailFiles;
            MailFileGridView.DataBind();
            ((GridViewDataColumn)MailFileGridView.Columns["Server"]).GroupBy();
            Session["MailFiles"] = dtMailFiles;
                  
        }
        public void FillMailFilesfromSession()
        {

            DataTable dtMailFiles = new DataTable();
            if(Session["MailFiles"]!="" && Session["MailFiles"]!=null)
            dtMailFiles=(DataTable)Session["MailFiles"];
           
            MailFileGridView.DataSource = dtMailFiles;
            MailFileGridView.DataBind();
        

        }

        protected void MailFileGridView_HtmlDataCellPrepared(object sender, DevExpress.Web.ASPxGridViewTableDataCellEventArgs e)
        {
             if (e.DataColumn.FieldName == "Status" &&(e.CellValue.ToString() == "OK"|| e.CellValue.ToString()=="Scanning"))
            {
                e.Cell.BackColor = System.Drawing.Color.LightGreen;
            }           

            else if (e.DataColumn.FieldName == "Status" && e.CellValue.ToString() == "Not Responding")
            {
                e.Cell.BackColor = System.Drawing.Color.Red;
                e.Cell.ForeColor = System.Drawing.Color.White;
            }
            else if (e.DataColumn.FieldName == "Status" && e.CellValue.ToString() == "Not Scanned")
            {
               
                e.Cell.BackColor =  System.Drawing.Color.FromName("#87CEEB");
                e.Cell.ForeColor = System.Drawing.Color.Black;
            }
            else if (e.DataColumn.FieldName == "Status" && e.CellValue.ToString() == "disabled")
            {
                e.Cell.BackColor = System.Drawing.Color.Gray;
               // e.Cell.ForeColor = System.Drawing.Color.White;
            }
             else if (e.DataColumn.FieldName == "Status")
             {
                 e.Cell.BackColor = System.Drawing.Color.Yellow;
                 // e.DataColumn.GroupFooterCellStyle.ForeColor =
             }
        }

        protected void MailFileGridView_HtmlRowCreated(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
        {
            //e.Row.Attributes.Add("onmouseover", "this.originalcolor=this.style.backgroundColor;" + "this.style.backgroundColor='#C0C0C0';");

            //e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalcolor;");
        }

        //1/2/2014 NS added
        public void SetBarGraphForTopQuota(string servername)
        {
            BiggestQuotaWebChart.Series.Clear();
            DataTable dt = VSWebBL.DashboardBL.DominoServerHealthBLL.Ins.SetBarGraphForTopQuota(servername);
            Series series1 = new Series("Mail File", ViewType.Bar);
            series1.ArgumentDataMember = dt.Columns["Title"].ToString();
            ValueDataMemberCollection seriesValueDataMembers = (ValueDataMemberCollection)series1.ValueDataMembers;
            seriesValueDataMembers.AddRange(dt.Columns["PercentQuota"].ToString());
            //Addnig series to mailchartbox control
            BiggestQuotaWebChart.Series.Add(series1);
            ((XYDiagram)BiggestQuotaWebChart.Diagram).PaneLayoutDirection = PaneLayoutDirection.Horizontal;
            XYDiagram seriesXY = (XYDiagram)BiggestQuotaWebChart.Diagram;
            //X and Y aixs detals
            seriesXY.AxisY.Title.Text = "Percent of Quota";
            seriesXY.AxisX.Title.Text = "Database Name";
            seriesXY.AxisY.Title.Visible = true;
            seriesXY.AxisX.Title.Visible = true;
            seriesXY.AxisX.Label.ResolveOverlappingOptions.AllowHide = false;
            seriesXY.AxisX.Label.ResolveOverlappingOptions.AllowRotate = true;
            seriesXY.AxisX.Label.ResolveOverlappingOptions.AllowStagger = true;
            //12/11/2013 NS added
            seriesXY.AxisY.Label.ResolveOverlappingOptions.AllowRotate = false;

            //Enabling the series 
            BiggestQuotaWebChart.Legend.Visible = false;
            ChartTitle title = new ChartTitle();
            if (servername == "All" || servername == "")
            {
                title.Text = "All Servers - Top 20 Largest Mail Files as a % of Quota";
            }
            else
            {
                title.Text = servername + " - Top 20 Largest Mail Files as a % of Quota";
            }

            BiggestQuotaWebChart.Titles.Clear();
            BiggestQuotaWebChart.Titles.Add(title);
            AxisBase axis = ((XYDiagram)BiggestQuotaWebChart.Diagram).AxisX;
            ((BarSeriesView)series1.View).ColorEach = false;
            BiggestQuotaWebChart.DataSource = dt;
            BiggestQuotaWebChart.DataBind();
        }

        protected void BiggestQuotaWebChart_CustomCallback(object sender, DevExpress.XtraCharts.Web.CustomCallbackEventArgs e)
        {
            this.BiggestQuotaWebChart.Width = new Unit(Convert.ToInt32(chartWidth.Value));
            SetBarGraphForTopQuota("");
            /*
            if (ServerComboBox.Text == "All Servers")
            {
                SetBarGraphForTopMail("All Servers");
            }
            else
            {
                SetBarGraphForTopMail(ServerComboBox.Text);
            }
             */
        }

        //1/15/2014 NS added
        public void SetPieChartForMailTemplates(string servername)
        {
            MailTemplateWebChart.Series.Clear();
            DataTable dt = VSWebBL.DashboardBL.DominoServerHealthBLL.Ins.SetPieChartForMailTemplates(servername);
            Series series = new Series("MailTemplate", ViewType.Pie);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                series.Points.Add(new SeriesPoint(dt.Rows[i]["DesignTemplateName"],dt.Rows[i]["MailDBCount"]));
            }
            MailTemplateWebChart.Series.Add(series);
            series.Label.PointOptions.PointView = PointView.Argument;
            MailTemplateWebChart.Legend.Visible = true;
            ChartTitle title = new ChartTitle();
            if (servername == "All" || servername == "")
            {
                title.Text = "Mail Templates for All Servers";
            }
            else
            {
                title.Text = "Mail Templates for " + servername;
            }
            MailTemplateWebChart.Titles.Clear();
            MailTemplateWebChart.Titles.Add(title);
            MailTemplateWebChart.DataSource = dt;
            MailTemplateWebChart.DataBind();
        }

        public void serverCombo()
        {
            DataTable dt = new DataTable();
            dt = VSWebBL.DashboardBL.mailFileBL.Ins.FillServerCombobox();
            DataRow dr = dt.NewRow();
            dr[0] = "All Servers";
            dt.Rows.InsertAt(dr, 0);
            dt = dt.DefaultView.ToTable(true, "Server");
            ServerComboBox.DataSource = dt;
            ServerComboBox.TextField = "Server";
            ServerComboBox.ValueField = "Server";
            ServerComboBox.DataBind();
            ServerComboBox.SelectedIndex = 0;
        }
		public void FillTypeCombobox(string page, string control)
		{
			//page = "tab";
			//control = "TypeComboBox";
			DataTable Typetab = VSWebBL.DashboardBL.KeyMetricsBL.Ins.GetspecificServerType(page, control);
			TypeComboBox.DataSource = Typetab;
			TypeComboBox.TextField = "ServerType";
			TypeComboBox.DataBind();
            //2/25/2016 Durga Modified for  VSPLUS-2611
			try
			{if(Typetab.Rows.Count>0)
                TypeComboBox.SelectedItem = TypeComboBox.Items.FindByText(Typetab.Rows[0]["ServerType"].ToString());
			}
			catch
			{
				TypeComboBox.SelectedIndex = 0;
			}
            if (Request.QueryString["ServerType"] != null)
            {
                TypeComboBox.SelectedItem = TypeComboBox.Items.FindByText(Request.QueryString["ServerType"].ToString());
            }
		}
		

        protected void MailTemplateWebChart_CustomCallback(object sender, DevExpress.XtraCharts.Web.CustomCallbackEventArgs e)
        {
            this.MailTemplateWebChart.Width = new Unit(Convert.ToInt32(chartWidth.Value));
            if (ServerComboBox.Text == "All Servers")
            {
                SetPieChartForMailTemplates("");
            }
            else
            {
                SetPieChartForMailTemplates(ServerComboBox.Text);
            }
        }

        protected void ServerComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            //4/8/2014 NS modified for VSPLUS-485
            if (tab == "3")
            {
                SetBarGraphForTopQuota(ServerComboBox.Text);
            }
            else if (tab == "4")
            {
                SetPieChartForMailTemplates(ServerComboBox.Text);
            }
        }

		protected void TypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {//2/25/2016 Durga Modified for  VSPLUS-2611
            DropDownSelectionMethod();
		}
        protected void MailFileGridView_PageSizeChanged(object sender, EventArgs e)
        {
            //ProfilesGridView.PageIndex;
            VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("MailFiles|MailFileGridView", MailFileGridView.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        }

        protected void btnCollapse_Click(object sender, EventArgs e)
        {
            if (btnCollapse.Text == "Collapse All Rows")
            {
                MailFileGridView.CollapseAll();
                btnCollapse.Image.Url = "~/images/icons/add.png";
                btnCollapse.Text = "Expand All Rows";
            }
            else
            {
                MailFileGridView.ExpandAll();
                btnCollapse.Image.Url = "~/images/icons/forbidden.png";
                btnCollapse.Text = "Collapse All Rows";
            }   
        }
        //2/25/2016 Durga Modified for  VSPLUS-2611
        public void DropDownSelectionMethod()
        {
            if (TypeComboBox.Text == "Exchange")
            {
                Response.Redirect("~/Dashboard/ExchangeMailFiles.aspx?ServerType=" + TypeComboBox.Text, false);
            }
            else if (TypeComboBox.Text == "Office365")
            {
                Response.Redirect("~/Dashboard/O365MailFiles.aspx?ServerType=" + TypeComboBox.Text, false);
            }
            else
            {
                SetBarGraphForTopQuota(TypeComboBox.Text);
            }
        }
        //2/25/2016 Durga Modified for  VSPLUS-2611
        protected void DataBound(object sender, EventArgs e)
        {
            SetItemCount();
        }
        void SetItemCount()
        {
            int itemCount = (int)MailFileGridView.GetTotalSummaryValue(MailFileGridView.TotalSummary["Server"]);
            MailFileGridView.SettingsPager.Summary.Text = "Page {0} of {1} (" + itemCount.ToString() + " items)";
        }
    }
}