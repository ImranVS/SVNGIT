using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DevExpress.XtraCharts;
using System.Drawing;
using DevExpress.Web.ASPxScheduler;
using DevExpress.XtraScheduler;
using DevExpress.XtraScheduler.Xml;
using DevExpress.Web;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraPrinting;
using System.IO;
using System.Net.Mime;

namespace VSWebUI.Dashboard
{
    public partial class WindowsServerDetails : System.Web.UI.Page
    {
		protected void Page_PreInit(object sender, EventArgs e)
		{

			this.Master.contentCallEvent += new EventHandler(Master_ButtonClick);
		}

		private void Master_ButtonClick(object sender, EventArgs e)
		{

		}
		int ServerTypeId = 16;
		// 3.5
		public bool exactmatch;
        protected void Page_Load(object sender, EventArgs e)
        {
            FillHealthAssessmentStatusGrid(Request.QueryString["Type"].ToString(), servernamelbl.Text);

            if (!IsPostBack && !IsCallback)
            {


                if (Request.QueryString["Type"] != "" && Request.QueryString["Type"] != null)
                {
                    lblServerType.Text = Request.QueryString["Type"].ToString() + ":  ";
                }
                if (Request.QueryString["Typ"] != "" && Request.QueryString["Typ"] != null)
                {
                    lblServerType.Text = Request.QueryString["Type"].ToString();
                }

                if (Request.QueryString["Type"] == "URL" || Request.QueryString["Type"] == "Network Device")
                {
                    diskspaceASPxRoundPanel.Visible = false;
                    cpuASPxRoundPanel.Visible = false;
                    memASPxRoundPanel.Visible = false;
                    usersASPxRoundPanel.Visible = false;

                    // performanceASPxRoundPanel.Width = 1000;
                    // performanceWebChartControl.Width = 800;
                    ASPxPageControl1.TabPages[1].ClientVisible = false;
                    ASPxPageControl1.TabPages[2].ClientVisible = false;
                }

				if (Request.QueryString["Status"] != "" && Request.QueryString["Status"] != null)
				{
					serverstatus.InnerHtml = Request.QueryString["Status"];
					if (Request.QueryString["Status"] == "OK" || Request.QueryString["Status"] == "Scanning")
					{
						serverstatus.Attributes["class"] = "OK";
					}
					else if (Request.QueryString["Status"] == "Not Responding")
					{
						serverstatus.Attributes["class"] = "NotResponding";
					}
					else if (Request.QueryString["Status"] == "Maintenance")
					{
						serverstatus.Attributes["class"] = "Maintenance";
					}
					else if (Request.QueryString["Status"] == "Not Scanned")
					{
						serverstatus.Attributes["class"] = "NotScanned";
					}
					else
					{
						serverstatus.Attributes["class"] = "Issue";
					}
				}

                if (Request.QueryString["Name"] != "" && Request.QueryString["Name"] != null)
                {
                    servernamelbl.Text = Request.QueryString["Name"];

					servernamelbldisp.InnerHtml = lblServerType.Text + servernamelbl.Text;
                    linkDiskSpaceMore.NavigateUrl = "DiskHealth.aspx?server=" + servernamelbl.Text;
                    CpuHyperLink.NavigateUrl = "DominoServerStatisticsDetail.aspx?name=Cpu&server=" + servernamelbl.Text;
                    MemHyperLink.NavigateUrl = "DominoServerStatisticsDetail.aspx?name=Mem&server=" + servernamelbl.Text;
                    //  PerfrmHyperLink.NavigateUrl = "DominoServerStatisticsDetail.aspx?name=Perfm&server=" + servernamelbl.Text;
                }
                if (Request.QueryString["LastDate"] != "" && Request.QueryString["LastDate"] != null)
                {
					Lastscanned.InnerHtml = "Last scan date: " + Request.QueryString["LastDate"].ToString();
                }
                else
                {
                    DataTable dt = VSWebBL.DashboardBL.QuickrHealthBLL.Ins.getLastScanDate(servernamelbl.Text);
					if (dt.Rows.Count > 0)
					{
						//9/19/2014 NS modified for VSPLUS-934
						//Lastscanned.Text = dt.Rows[0]["LastUpdate"].ToString();
						Lastscanned.InnerHtml = "Last scan date: " + dt.Rows[0]["LastUpdate"].ToString();
					}
					else
					{
						//9/19/2014 NS modified for VSPLUS-934
						//lbltext.Visible = false;
						//Lastscanned.Visible = false;
						//lbltext.Style.Add("display", "none");
						Lastscanned.Style.Add("display", "none");
					}
                }

            }
        
            //01/23/2014 MD changed it for every page load for the graphs to be populated always.
            SetGridForGenericDisk(servernamelbl.Text);
            SetGraphForMemoryGeneric("hh", servernamelbl.Text);
            SetGraphForCPUGeneric("hh", servernamelbl.Text);

            SetGraphForGenericEnabledUsers("hh", servernamelbl.Text);

            //7/14/2014 NS added for VSPLUS-813
            //5/6/2015 NS modified for VSPLUS-1707
            SetGraphForDiskSpace2("'" + servernamelbl.Text + "'");
            DiskSpaceWebChartControl1.ClientVisible = true;

            DiskSpaceLync.ClientVisible = false;
            SetGraphforperformance("hh", servernamelbl.Text);

              // DataBase Code
            if (!IsPostBack && !IsCallback)
            {
				fillcombo();
                Fillmaintenancegrid();
                FillAlertHistory();
                FillOutageTab();
                FillHealthAssessmentStatusGrid(Request.QueryString["Type"].ToString(), servernamelbl.Text);

				FillMonitoredServicesGrid();
				FillUnMonitoredServicesGrid();

                if (Session["UserPreferences"] != null)
                {
                    DataTable UserPreferences = (DataTable)Session["UserPreferences"];
                    foreach (DataRow dr in UserPreferences.Rows)
					{
						if (dr[1].ToString() == "WindowsServerDetailspage|MonitoredServicesGrid")
						{
							MonitoredServicesGrid.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
						}
						if (dr[1].ToString() == "WindowsServerDetailspage|UnMonitoredServicesGrid")
						{
							UnMonitoredServicesGrid.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
						}
                        if (dr[1].ToString() == "WindowsServerDetailspage|maintenancegrid")
                        {
                            maintenancegrid.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                        }
                        if (dr[1].ToString() == "WindowsServerDetailspage|AlertGridView")
                        {
                            AlertGridView.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                        }
                        if (dr[1].ToString() == "WindowsServerDetailspage|OutageGridView")
                        {
                            OutageGridView.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                        }
                    }
                }
            }
            else
            {
				
                SetGridFromSession();
                Fillmaintenancegridfromsession();
                FillAlertHistoryfromSession();
				FillMonitoredServicesGridfromSession();
                FilloutagefromSession();
				FillUnMonitoredServicesGridfromSession();
                FillHealthAssessmentStatusGrid(Request.QueryString["Type"].ToString(), servernamelbl.Text);

            }
        }
		// VSPLUS-2197====version 3.5 soma
		protected void DateParamEdit_DateChanged(object sender, EventArgs e)
		{
			ReloadGrid();
		}
		// VSPLUS-2197====version 3.5 soma
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
				if (EventTypeComboBox.SelectedIndex == 0)
					dt = VSWebBL.ConfiguratorBL.ELSBL.Ins.GetEventHistorydetails(servernamelbl.Text, Request.QueryString["Type"].ToString());
				else
					dt = VSWebBL.ConfiguratorBL.ELSBL.Ins.GetEventHistoryByLogdetails(EventTypeComboBox.SelectedItem.Value.ToString(), servernamelbl.Text, Request.QueryString["Type"].ToString());
			}
			else
			{
				dt = VSWebBL.ConfiguratorBL.ELSBL.Ins.GetEventHistorydetails(dtval.Month.ToString(), dtval.Year.ToString(), servernamelbl.Text);
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
		// VSPLUS-2197====version 3.5 soma
		protected void EventTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (EventTypeComboBox.SelectedIndex != -1)
			{
				DataTable dt = new DataTable();
				if (EventTypeComboBox.SelectedIndex == 0)
					dt = VSWebBL.ConfiguratorBL.ELSBL.Ins.GetEventHistorydetails(servernamelbl.Text, Request.QueryString["Type"].ToString());
				else
					dt = VSWebBL.ConfiguratorBL.ELSBL.Ins.GetEventHistoryByLogdetails(EventTypeComboBox.SelectedItem.Value.ToString(), servernamelbl.Text, Request.QueryString["Type"].ToString());
				EventsHistory.DataSource = dt;
				EventsHistory.DataBind();
			}
		}
		// VSPLUS-2197====version 3.5 soma
		public void fillcombo()
		{
			EventTypeComboBox.Items.Add("All Logs", "All Logs");
			EventTypeComboBox.Items.Add("Application", "Application");
			EventTypeComboBox.Items.Add("Security", "Security");
			EventTypeComboBox.Items.Add("Setup", "Setup");
			EventTypeComboBox.Items.Add("System", "System");

		}
		// VSPLUS-2197====version 3.5 soma
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
		// VSPLUS-2197====version 3.5 soma
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
		// VSPLUS-2197====version 3.5 soma
		protected void EventsHistoryGridView_PageSizeChanged(object sender, EventArgs e)
		{
			//ProfilesGridView.PageIndex;
			VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("OverallServerAlerts|EventsHistory", EventsHistory.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
			Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
		}
		// VSPLUS-2197====version 3.5 soma
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
		// VSPLUS-2197====version 3.5 soma
        public void SetGridForGenericDisk(string ServerName)
        {
            try
            {
                DataTable dt = VSWebBL.DashboardBL.WindowsDetailsBAL.Ins.SetGridForGenericDisk(ServerName);
                Session["GridData"] = dt;
                DiskSpaceLync.DataSource = dt;
                DiskSpaceLync.DataBind();
                //((GridViewDataColumn)DiskHealthGrid.Columns["ServerName"]).GroupBy();
                int rowIndex = DiskSpaceLync.FindVisibleIndexByKeyValue("ID");
                //Session["rowIndex"] = rowIndex;
            }
            catch (Exception ex)
            {
                //Response.Write("Error : " + ex);
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
        }
        public void FillHealthAssessmentFromSession()
        {
            try
            {
                if (Session["GridData"] != null)
                {
                    DataTable dt = Session["HealthAssessment"] as DataTable;
                    HealthAssessmentGrid11.DataSource = dt;
                    HealthAssessmentGrid11.DataBind();

                }
            }
            catch (Exception ex)
            {
                 Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
        }
        private void FillHealthAssessmentStatusGrid(string type, string servername)
        {
            try
            {
                 string TypeAndName = servername + "-" + type;
                DataTable StatusTable12 = VSWebBL.DashboardBL.ExchangeServerDetailsBL.Ins.GetHealthAssessmentStatusDetails(TypeAndName);

                Session["HealthAssessment"] = StatusTable12;
                HealthAssessmentGrid11.DataSource = StatusTable12;
                HealthAssessmentGrid11.DataBind();
            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
            finally
            {
            }

        }
        protected void HealthAssessmentGrid11_HtmlDataCellPrepared(object sender, DevExpress.Web.ASPxGridViewTableDataCellEventArgs e)
        {
           
            if (e.DataColumn.FieldName == "Result" && (e.CellValue.ToString() == "OK" || e.CellValue.ToString() == "Scanning" || e.CellValue.ToString() == "Telnet" ||
                e.CellValue.ToString() == "Running" || e.CellValue.ToString() == "Pass") || e.CellValue.ToString() == "Passed")
            {
                e.Cell.BackColor = System.Drawing.Color.LightGreen;
            }

            else if (e.DataColumn.FieldName == "Result" && (e.CellValue.ToString() == "Not Responding" || e.CellValue.ToString().ToUpper() == "FAIL"))
            {
                e.Cell.BackColor = System.Drawing.Color.Red;
                e.Cell.ForeColor = System.Drawing.Color.White;
            }
            else if (e.DataColumn.FieldName == "Result" && e.CellValue.ToString() == "Not Scanned")
            {
                e.Cell.BackColor = System.Drawing.Color.Blue;
                e.Cell.ForeColor = System.Drawing.Color.White;
            }
            else if (e.DataColumn.FieldName == "Result" && e.CellValue.ToString() == "Disabled")
            {
                e.Cell.BackColor = System.Drawing.Color.FromName("#D0D0D0");
                //e.Cell.BackColor = System.Drawing.Color.Gray;
                // e.Cell.ForeColor = System.Drawing.Color.White;
            }
            else if (e.DataColumn.FieldName == "Result" && e.CellValue.ToString() == "ISSUE")
            {
                e.Cell.BackColor = System.Drawing.Color.Yellow;
                // e.DataColumn.GroupFooterCellStyle.ForeColor = System.Drawing.Color.Yellow;

            }

        }
        

        public void SetGridFromSession()
        {
            try
            {
                if (Session["GridData"] != null)
                {
                    DataTable dt = Session["GridData"] as DataTable;
                    DiskSpaceLync.DataSource = dt;
                    DiskSpaceLync.DataBind();
                    int rowIndex = DiskSpaceLync.FindVisibleIndexByKeyValue("ID");
                    Session["rowIndex"] = rowIndex;
                }
            }
            catch (Exception ex)
            {
                //Response.Write("Error : " + ex);
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
        }
        protected void DiskSpaceLync_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            string thresholdType = e.GetValue("ThresholdType").ToString();
            //if (thresholdType == "")
            //    thresholdType = "Percent";
            double valThreshold;
            if (e.GetValue("Threshold").ToString() != "")
            {
				string t = e.GetValue("Threshold").ToString();
				valThreshold = Convert.ToDouble(e.GetValue("Threshold").ToString());

                //if the threshold type is "%" then divide the value by 100
                if (thresholdType == "Percent" && valThreshold > 0)
                    valThreshold = (valThreshold / 100);
            }
            else
            {
                valThreshold = 0.0;
            }
            if (e.DataColumn.FieldName == "DiskName")
            {
                if (thresholdType == "Percent")
                {
                    double valPercentFree;
                    if (e.GetValue("PercentFree").ToString() != "")
                    {
                        valPercentFree = Convert.ToDouble(e.GetValue("PercentFree").ToString());
                    }
                    else
                    {
                        valPercentFree = 0.0;
                    }

                    if (valPercentFree > valThreshold && (valPercentFree <= 1 - (0.8 * (1 - valThreshold))))
                    {
                        e.Cell.BackColor = System.Drawing.Color.Yellow;
                    }
                    else if ((valPercentFree) <= valThreshold)
                    {
                        e.Cell.BackColor = System.Drawing.Color.Red;
                        e.Cell.ForeColor = System.Drawing.Color.White;
                    }
                    else
                    {
                        e.Cell.BackColor = System.Drawing.Color.Green;
                        e.Cell.ForeColor = System.Drawing.Color.White;
                    }
                }
            }
            if (e.DataColumn.FieldName == "DiskName")
            {
                if (thresholdType == "GB")
                {
                    double valGBFree;
                    if (e.GetValue("DiskFree").ToString() != "")
                    {
                        valGBFree = Convert.ToDouble(e.GetValue("DiskFree").ToString());
                    }
                    else
                    {
                        valGBFree = 0.0;
                    }

                    //double valPercentFree = (double)e.GetValue("PercentFree");

                    //if (valGBFree > valThreshold && (valPercentFree <= 1 - (0.8 * (1 - 0))))
                    if (valGBFree > valThreshold && (valGBFree <= (valThreshold + ((20 * valThreshold) / 100))))
                    {
                        e.Cell.BackColor = System.Drawing.Color.Yellow;
                    }
                    else if ((valGBFree) <= valThreshold)
                    {
                        e.Cell.BackColor = System.Drawing.Color.Red;
                        e.Cell.ForeColor = System.Drawing.Color.White;
                    }
                    else
                    {
                        e.Cell.BackColor = System.Drawing.Color.Green;
                        e.Cell.ForeColor = System.Drawing.Color.White;
                    }
                }
            }
        }

        protected void DiskSpaceLync_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            //ASPxGridView grid = sender as ASPxGridView;
            //int index = int.Parse(e.Parameters);
            //int exCount = 0;
            //for (int i = grid.SettingsPager.PageSize * grid.PageIndex; i < index; i++)
            //{
            //    if (grid.IsRowExpanded(i))
            //        exCount += 1;
            //}
            //grid.CollapseAll();
            //grid.FocusedRowIndex = index - exCount / 2;
            //grid.ExpandRow(index - exCount / 2);

        }

        protected void DiskSpaceLync_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
        {
            int index = DiskSpaceLync.FocusedRowIndex;
            if (e.VisibleIndex != index)
            {
                e.Row.Attributes.Add("onmouseover", "this.originalcolor=this.style.backgroundColor;" + "this.style.backgroundColor='#C0C0C0';");

                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalcolor;");

            }
        }
        protected void DiskSpaceLync_PageSizeChanged(object sender, EventArgs e)
        {
            //ProfilesGridView.PageIndex;
            VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("DiskHealth|DiskHealthGrid", DiskSpaceLync.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        }
        public void FillOutageTab()
        {
            DataTable dt = new DataTable();
            dt = VSWebBL.DashboardBL.DominoServerDetailsBL.Ins.GetOutage(servernamelbl.Text, Request.QueryString["Type"].ToString());
            Session["Outage"] = dt;
            OutageGridView.DataSource = dt;
            OutageGridView.DataBind();
        }
        public void FilloutagefromSession()
        {
            DataTable dt = new DataTable();
            dt = Session["Outage"] as DataTable;
            OutageGridView.DataSource = dt;
            OutageGridView.DataBind();
        }






        public DataTable SetGraphforperformance(string paramGraph, string DeviceName)
        {
            try
            {
                performanceWebChartControl.Series.Clear();
				DataTable dt = VSWebBL.DashboardBL.WindowsDetailsBAL.Ins.SetGraphforperformance(paramGraph, DeviceName, ServerTypeId);

                Series series = null;
                series = new Series("DominoServer", ViewType.Line);
                series.Visible = true;
                series.ArgumentDataMember = dt.Columns["dtfrom"].ToString();

                ValueDataMemberCollection seriesValueDataMembers = (ValueDataMemberCollection)series.ValueDataMembers;
                seriesValueDataMembers.AddRange(dt.Columns["maxval"].ToString());
                performanceWebChartControl.Series.Add(series);

                // Constant Line

                // Cast the chart's diagram to the XYDiagram type, to access its axes.
                XYDiagram diagram = (XYDiagram)performanceWebChartControl.Diagram;
                //Mukund 16Jul14, VSPLUS-824- Threshold in graph is not updating
                DataTable dt1 = VSWebBL.DashboardBL.WindowsDetailsBAL.Ins.ResponseThresholdForGeneric(DeviceName);
                if (dt1.Rows.Count > 0)
                {
                    if (int.Parse(dt1.Rows[0]["ResponseThreshold"].ToString()) > 0)
                    {
                        // Create a constant line.
                        ConstantLine constantLine1 = new ConstantLine("Constant Line 1");
                        diagram.AxisY.ConstantLines.Add(constantLine1);

                        // Define its axis value.
                        constantLine1.AxisValue = dt1.Rows[0]["ResponseThreshold"].ToString();

                        // Customize the behavior of the constant line.
                        // constantLine1.Visible = true;
                        //constantLine1.ShowInLegend = true;
                        // constantLine1.LegendText = "Some Threshold";
                        constantLine1.ShowBehind = true;

                        // Customize the constant line's title.
                        constantLine1.Title.Visible = true;
                        constantLine1.Title.Text = "Threshold:" + dt1.Rows[0]["ResponseThreshold"].ToString();
                        constantLine1.Title.TextColor = Color.Red;
                        // constantLine1.Title.Antialiasing = false;
                        //constantLine1.Title.Font = new Font("Tahoma", 14, FontStyle.Bold);
                        constantLine1.Title.ShowBelowLine = true;
                        constantLine1.Title.Alignment = ConstantLineTitleAlignment.Far;

                        // Customize the appearance of the constant line.
                        constantLine1.Color = Color.Red;
                        constantLine1.LineStyle.DashStyle = DashStyle.Solid;
                        constantLine1.LineStyle.Thickness = 2;
                    }
                }
                ((XYDiagram)performanceWebChartControl.Diagram).PaneLayoutDirection = PaneLayoutDirection.Horizontal;

                XYDiagram seriesXY = (XYDiagram)performanceWebChartControl.Diagram;
                seriesXY.AxisY.Title.Text = "Response Time";
                seriesXY.AxisY.Title.Visible = true;
                seriesXY.AxisX.Title.Text = "Date/Time";
                seriesXY.AxisX.Title.Visible = true;
                seriesXY.AxisX.Label.ResolveOverlappingOptions.AllowStagger = true;

                performanceWebChartControl.Legend.Visible = false;

                // ((SplineSeriesView)series.View).LineTensionPercent = 100;
                ((LineSeriesView)series.View).LineMarkerOptions.Size = 4;
                ((LineSeriesView)series.View).LineMarkerOptions.Color = Color.White;

                AxisBase axis = ((XYDiagram)performanceWebChartControl.Diagram).AxisX;
                //4/18/2014 NS commented out for VSPLUS-312
                //axis.DateTimeGridAlignment = DateTimeMeasurementUnit.Hour;
                axis.Range.SideMarginsEnabled = false;
                axis.GridLines.Visible = false;
                //axis.DateTimeOptions.Format = DateTimeFormat.Custom;
                //axis.DateTimeOptions.FormatString = "MM/dd/yyyy HH:mm";
                ((LineSeriesView)series.View).Color = Color.Blue;

                AxisBase axisy = ((XYDiagram)performanceWebChartControl.Diagram).AxisY;
                axisy.Range.AlwaysShowZeroLevel = false;
                axisy.Range.SideMarginsEnabled = true;
                axisy.GridLines.Visible = true;
                performanceWebChartControl.DataSource = dt;
                performanceWebChartControl.DataBind();

                return dt;
            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }

        }



        public void SetGraphForDiskSpace2(string serverName)
        {
            DataTable dt = VSWebBL.DashboardBL.DominoServerDetailsBL.Ins.SetGraphForDiskSpace(serverName, "","");
            DiskSpaceWebChartControl1.DataSource = dt;
            DiskSpaceWebChartControl1.Series[0].DataSource = dt;
            DiskSpaceWebChartControl1.Series[0].ArgumentDataMember = dt.Columns["DiskName"].ToString();
            DiskSpaceWebChartControl1.Series[0].ValueDataMembers.AddRange(dt.Columns["DiskUsed"].ToString());
            DiskSpaceWebChartControl1.Series[0].Visible = true;
            DiskSpaceWebChartControl1.Series[1].DataSource = dt;
            DiskSpaceWebChartControl1.Series[1].ArgumentDataMember = dt.Columns["DiskName"].ToString();
            DiskSpaceWebChartControl1.Series[1].ValueDataMembers.AddRange(dt.Columns["DiskFree"].ToString());
            DiskSpaceWebChartControl1.Series[1].Visible = true;
        }

        public void SetGraphForCPUGeneric(string paramGraph, string serverName)
        {
            cpuWebChartControl.Series.Clear();
			DataTable dt = VSWebBL.DashboardBL.WindowsDetailsBAL.Ins.SetGraphForCPUGeneric(paramGraph, serverName, ServerTypeId);

            Series series = null;
            series = new Series("LyncServer", ViewType.Line);
            series.Visible = true;
            series.ArgumentDataMember = dt.Columns["dtfrom"].ToString();

            ValueDataMemberCollection seriesValueDataMembers = (ValueDataMemberCollection)series.ValueDataMembers;
            seriesValueDataMembers.AddRange(dt.Columns["maxval"].ToString());
            cpuWebChartControl.Series.Add(series);

            ((XYDiagram)cpuWebChartControl.Diagram).PaneLayoutDirection = PaneLayoutDirection.Horizontal;

            XYDiagram seriesXY = (XYDiagram)cpuWebChartControl.Diagram;
            seriesXY.AxisY.Title.Text = "CPU";
            seriesXY.AxisY.Title.Visible = true;
            seriesXY.AxisX.Title.Text = "Date/Time";
            seriesXY.AxisX.Title.Visible = true;
            cpuWebChartControl.Legend.Visible = false;

            // ((SplineSeriesView)series.View).LineTensionPercent = 100;
            ((LineSeriesView)series.View).LineMarkerOptions.Size = 4;
            ((LineSeriesView)series.View).LineMarkerOptions.Color = Color.White;

            AxisBase axis = ((XYDiagram)cpuWebChartControl.Diagram).AxisX;
            //4/18/2014 NS commented out for VSPLUS-312
            //axis.DateTimeGridAlignment = DateTimeMeasurementUnit.Hour;
            //5/13/2014 NS commented out for VSPLUS-621
            //axis.GridSpacingAuto = false;
            //axis.MinorCount = 15;
            //axis.GridSpacing = 0.5;
            axis.Range.SideMarginsEnabled = false;
            axis.GridLines.Visible = false;
            //axis.DateTimeOptions.Format = DateTimeFormat.Custom;
            //axis.DateTimeOptions.FormatString = "dd/MM HH:mm";
            ((LineSeriesView)series.View).Color = Color.Blue;

            AxisBase axisy = ((XYDiagram)cpuWebChartControl.Diagram).AxisY;
            axisy.Range.AlwaysShowZeroLevel = false;
            axisy.Range.SideMarginsEnabled = true;
            axisy.GridLines.Visible = true;
            cpuWebChartControl.DataSource = dt;
            cpuWebChartControl.DataBind();

            //return dt;     
        }

        public void SetGraphForMemoryGeneric(string paramGraph, string serverName)
        {
            memWebChartControl.Series.Clear();
			DataTable dt = VSWebBL.DashboardBL.WindowsDetailsBAL.Ins.SetGraphForMemoryGeneric(paramGraph, serverName, ServerTypeId);

            Series series = null;
            series = new Series("LyncServer", ViewType.Line);
            series.Visible = true;
            series.ArgumentDataMember = dt.Columns["dtfrom"].ToString();

            ValueDataMemberCollection seriesValueDataMembers = (ValueDataMemberCollection)series.ValueDataMembers;
            seriesValueDataMembers.AddRange(dt.Columns["maxval"].ToString());
            memWebChartControl.Series.Add(series);

            ((XYDiagram)memWebChartControl.Diagram).PaneLayoutDirection = PaneLayoutDirection.Horizontal;

            XYDiagram seriesXY = (XYDiagram)memWebChartControl.Diagram;
            seriesXY.AxisY.Title.Text = "Memory";
            seriesXY.AxisY.Title.Visible = true;
            seriesXY.AxisX.Title.Text = "Date/Time";
            seriesXY.AxisX.Title.Visible = true;
            memWebChartControl.Legend.Visible = false;

            //((SplineSeriesView)series.View).LineTensionPercent = 100;
            ((LineSeriesView)series.View).LineMarkerOptions.Size = 4;
            ((LineSeriesView)series.View).LineMarkerOptions.Color = Color.White;

            AxisBase axis = ((XYDiagram)memWebChartControl.Diagram).AxisX;
            //4/18/2014 NS commented out for VSPLUS-312
            //axis.DateTimeGridAlignment = DateTimeMeasurementUnit.Hour;
            //5/13/2014 NS commented out for VSPLUS-621
            //axis.GridSpacingAuto = false;
            //axis.MinorCount = 15;
            //axis.GridSpacing = 0.5;
            axis.Range.SideMarginsEnabled = false;
            axis.GridLines.Visible = false;
            //axis.DateTimeOptions.Format = DateTimeFormat.Custom;
            //axis.DateTimeOptions.FormatString = "dd/MM HH:mm";

            ((LineSeriesView)series.View).Color = Color.Blue;

            AxisBase axisy = ((XYDiagram)memWebChartControl.Diagram).AxisY;
            axisy.Range.AlwaysShowZeroLevel = false;
            axisy.Range.SideMarginsEnabled = true;
            axisy.GridLines.Visible = true;
            memWebChartControl.DataSource = dt;
            memWebChartControl.DataBind();
        }


        protected void MonitoredDBGridView_HtmlDataCellPrepared(object sender, DevExpress.Web.ASPxGridViewTableDataCellEventArgs e)
        {
            if (e.DataColumn.FieldName == "Status" && (e.CellValue.ToString() == "OK" || e.CellValue.ToString() == "Scanning"))
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
                e.Cell.BackColor = System.Drawing.Color.Blue;
                e.Cell.ForeColor = System.Drawing.Color.White;
            }
            else if (e.DataColumn.FieldName == "Status" && e.CellValue.ToString() == "disabled")
            {
                e.Cell.BackColor = System.Drawing.Color.Gray;
                // e.Cell.ForeColor = System.Drawing.Color.White;
            }
            else if (e.DataColumn.FieldName == "Status")
            {
                e.Cell.BackColor = System.Drawing.Color.Yellow;
                // e.DataColumn.GroupFooterCellStyle.ForeColor = System.Drawing.Color.Yellow;
            }

        }


       
        protected void DominoserverTasksgrid_HtmlDataCellPrepared(object sender, DevExpress.Web.ASPxGridViewTableDataCellEventArgs e)
        {
            if (e.DataColumn.FieldName == "StatusSummary" && (e.CellValue.ToString() == "OK" || e.CellValue.ToString() == "Scanning"))
            {
                e.Cell.BackColor = System.Drawing.Color.LightGreen;
            }

            else if (e.DataColumn.FieldName == "StatusSummary" && e.CellValue.ToString() == "Not Responding")
            {
                e.Cell.BackColor = System.Drawing.Color.Red;
                e.Cell.ForeColor = System.Drawing.Color.White;
            }
            else if (e.DataColumn.FieldName == "StatusSummary" && e.CellValue.ToString() == "Not Scanned")
            {
                e.Cell.BackColor = System.Drawing.Color.Blue;
                e.Cell.ForeColor = System.Drawing.Color.White;
            }
            else if (e.DataColumn.FieldName == "StatusSummary" && e.CellValue.ToString() == "disabled")
            {
                e.Cell.BackColor = System.Drawing.Color.Gray;
                // e.Cell.ForeColor = System.Drawing.Color.White;
            }
            else if (e.DataColumn.FieldName == "StatusSummary")
            {
                e.Cell.BackColor = System.Drawing.Color.Yellow;
                // e.DataColumn.GroupFooterCellStyle.ForeColor = System.Drawing.Color.Yellow;
            }
        }






        protected void NonmoniterdGrid_HtmlDataCellPrepared(object sender, DevExpress.Web.ASPxGridViewTableDataCellEventArgs e)
        {
            if (e.DataColumn.FieldName == "StatusSummary" && (e.CellValue.ToString() == "OK" || e.CellValue.ToString() == "Scanning"))
            {
                e.Cell.BackColor = System.Drawing.Color.LightGreen;
            }

            else if (e.DataColumn.FieldName == "StatusSummary" && e.CellValue.ToString() == "Not Responding")
            {
                e.Cell.BackColor = System.Drawing.Color.Red;
                e.Cell.ForeColor = System.Drawing.Color.White;
            }
            else if (e.DataColumn.FieldName == "StatusSummary" && e.CellValue.ToString() == "Not Scanned")
            {
                e.Cell.BackColor = System.Drawing.Color.Blue;
                e.Cell.ForeColor = System.Drawing.Color.White;
            }
            else if (e.DataColumn.FieldName == "StatusSummary" && e.CellValue.ToString() == "disabled")
            {
                e.Cell.BackColor = System.Drawing.Color.Gray;
                // e.Cell.ForeColor = System.Drawing.Color.White;
            }
            else if (e.DataColumn.FieldName == "StatusSummary")
            {
                e.Cell.BackColor = System.Drawing.Color.Yellow;
            }   // e.DataColumn.GroupFooterCellStyle.ForeColor = System.Drawing.Color.Yellow;
        }


        public void Fillmaintenancegrid()
        {
            Session["SLmaitservers"] = "";
            DataTable dt = VSWebBL.ConfiguratorBL.MaintenanceBL.Ins.Getmaintenanceserversbyservername(Request.QueryString["Name"].ToString(), "", "", "", "");
            maintenancegrid.DataSource = dt;
            maintenancegrid.DataBind();
            Session["SLmaitservers"] = dt;
        }
        public void Fillmaintenancegridfromsession()
        {
            FillMaintenance();

        }

        protected void btnsearch_Click(object sender, EventArgs e)
        {
            FillMaintenance();
        }
        public void FillMaintenance()
        {
            string fromdate = txtfromdate.Text;
            string todate = txttodate.Text;
            // string fromtime = //txtfromtime.Text;ASPxTimeEdit1

            string fromtime = ASPxTimeEdit1.Text;
            //  string totime = txttotime.Text;
            string totime = ASPxTimeEdit2.Text;
            if (txtfromdate.Text != "" && txttodate.Text != "")
            {
                if (Convert.ToDateTime(fromdate) > Convert.ToDateTime(todate))
                {
                    MsgPopupControl.ShowOnPageLoad = true;
                    ErrmsgLabel.Text = "From Date value should be less than To Date.";
                }

                else
                {

                    if ((ASPxTimeEdit1.Text != null && ASPxTimeEdit2.Text != null) && (ASPxTimeEdit1.Text != "" && ASPxTimeEdit2.Text != ""))
                    {
                        string fhour = fromtime.Substring(0, fromtime.IndexOf(":"));
                        string thour = totime.Substring(0, totime.IndexOf(":"));
                        int fhourInt = int.Parse(fhour);
                        int thourInt = int.Parse(thour);
                        string fminute = fromtime.Substring(3, 2);
                        string tminute = totime.Substring(3, 2);
                        int fminuteInt = int.Parse(fminute);
                        int tminuteInt = int.Parse(tminute);
                        if (fhourInt >= 24 || thourInt >= 24 || fminuteInt >= 60 || tminuteInt >= 60)
                        {
                            MsgPopupControl.ShowOnPageLoad = true;
                            ErrmsgLabel.Text = "Invalid hour/minute entry.";
                        }

                        else
                        {
                            Session["SLmaitservers"] = "";
                            DataTable dt = VSWebBL.ConfiguratorBL.MaintenanceBL.Ins.Getmaintenanceserversbyservername(Request.QueryString["Name"].ToString(), fromdate, todate, fromtime, totime);
                            Session["SLmaitservers"] = dt;
                            maintenancegrid.DataSource = dt;
                            maintenancegrid.DataBind();
                        }
                    }
                }
            }
            else
            {
                if ((ASPxTimeEdit1.Text != null && ASPxTimeEdit2.Text != null) && (ASPxTimeEdit1.Text != "" && ASPxTimeEdit2.Text != ""))
                {
                    string fhour = fromtime.Substring(0, fromtime.IndexOf(":"));
                    string thour = totime.Substring(0, totime.IndexOf(":"));
                    int fhourInt = int.Parse(fhour);
                    int thourInt = int.Parse(thour);
                    string fminute = fromtime.Substring(3, 2);
                    string tminute = totime.Substring(3, 2);
                    int fminuteInt = int.Parse(fminute);
                    int tminuteInt = int.Parse(tminute);
                    if (fhourInt >= 24 || thourInt >= 24 || fminuteInt >= 60 || tminuteInt >= 60)
                    {
                        MsgPopupControl.ShowOnPageLoad = true;
                        ErrmsgLabel.Text = "Invalid hour/minute entry.";
                    }

                    else
                    {
                        Session["SLmaitservers"] = "";
                        DataTable dt = VSWebBL.ConfiguratorBL.MaintenanceBL.Ins.Getmaintenanceserversbyservername(Request.QueryString["Name"].ToString(), fromdate, todate, fromtime, totime);
                        Session["SLmaitservers"] = dt;
                        maintenancegrid.DataSource = dt;
                        maintenancegrid.DataBind();
                    }
                }
              
                else
                {
                    Session["SLmaitservers"] = "";
                    DataTable dt = VSWebBL.ConfiguratorBL.MaintenanceBL.Ins.Getmaintenanceserversbyservername(Request.QueryString["Name"].ToString(), fromdate, todate, fromtime, totime);
                    Session["SLmaitservers"] = dt;
                    maintenancegrid.DataSource = dt;
                    maintenancegrid.DataBind();
                }
            }

        }
        protected void ClearButton_Click(object sender, EventArgs e)
        {
            txtfromdate.Text = "";
            txttodate.Text = "";
            ASPxTimeEdit1.Text = "";
            ASPxTimeEdit2.Text = "";
            FillMaintenance();
        }


        protected void OKButton_Click(object sender, EventArgs e)
        {

            MsgPopupControl.ShowOnPageLoad = false;

        }





        public void SetGraphForGenericEnabledUsers(string paramGraph, string serverName)
        {
            usersWebChartControl.Series.Clear();
			DataTable dt = VSWebBL.DashboardBL.WindowsDetailsBAL.Ins.SetGraphForGenericEnabledUsers(paramGraph, serverName, ServerTypeId);

            Series series = null;
            series = new Series("DominoServer", ViewType.Line);
            series.Visible = true;
            series.ArgumentDataMember = dt.Columns["dtfrom"].ToString();

            ValueDataMemberCollection seriesValueDataMembers = (ValueDataMemberCollection)series.ValueDataMembers;
            seriesValueDataMembers.AddRange(dt.Columns["maxval"].ToString());
            usersWebChartControl.Series.Add(series);

            ((XYDiagram)usersWebChartControl.Diagram).PaneLayoutDirection = PaneLayoutDirection.Horizontal;

            XYDiagram seriesXY = (XYDiagram)usersWebChartControl.Diagram;
            seriesXY.AxisY.Title.Text = "GenericEnabledUsers";
            seriesXY.AxisY.Title.Visible = true;
            seriesXY.AxisX.Title.Text = "Date/Time";
            seriesXY.AxisX.Title.Visible = true;
            usersWebChartControl.Legend.Visible = false;

            //((SplineSeriesView)series.View).LineTensionPercent = 100;
            ((LineSeriesView)series.View).LineMarkerOptions.Size = 4;
            ((LineSeriesView)series.View).LineMarkerOptions.Color = Color.White;

            AxisBase axis = ((XYDiagram)usersWebChartControl.Diagram).AxisX;
            //4/18/2014 NS commented out for VSPLUS-312
            //axis.DateTimeGridAlignment = DateTimeMeasurementUnit.Hour;
            //axis.MinorCount = 15;
            //axis.GridSpacing = 0.5;
            axis.Range.SideMarginsEnabled = false;
            axis.GridLines.Visible = false;
            //axis.DateTimeOptions.Format = DateTimeFormat.Custom;

            ((LineSeriesView)series.View).Color = Color.Blue;

            //10/9/2013 NS moved the ds assignment prior to setting the grid spacing option
            usersWebChartControl.DataSource = dt;
            usersWebChartControl.DataBind();

            AxisBase axisy = ((XYDiagram)usersWebChartControl.Diagram).AxisY;
            axisy.Range.AlwaysShowZeroLevel = false;
            axisy.Range.SideMarginsEnabled = true;
            axisy.GridLines.Visible = true;

            //10/8/2013 NS added
            seriesXY.AxisY.NumericOptions.Format = NumericFormat.Number;
            seriesXY.AxisY.NumericOptions.Precision = 0;
            seriesXY.AxisY.GridSpacingAuto = true;

            double min = Convert.ToDouble(((XYDiagram)usersWebChartControl.Diagram).AxisY.Range.MinValue);
            double max = Convert.ToDouble(((XYDiagram)usersWebChartControl.Diagram).AxisY.Range.MaxValue);

            int gs = (int)((max - min) / 5);

            if (gs == 0)
            {
                gs = 1;
                seriesXY.AxisY.GridSpacingAuto = false;
                seriesXY.AxisY.GridSpacing = gs;
            }

        }

        protected void usersASPxRadioButtonList_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetGraphForGenericEnabledUsers(usersASPxRadioButtonList.Value.ToString(), servernamelbl.Text);
        }








        public void FillAlertHistory()
        {
            DataTable AlertHistorytab = new DataTable();
            try
            {
                AlertHistorytab = VSWebBL.DashboardBL.DominoServerDetailsBL.Ins.GetAlertHistry(Request.QueryString["Name"].ToString());
                Session["AlertHistorytab"] = AlertHistorytab;
                AlertGridView.DataSource = AlertHistorytab;
                AlertGridView.DataBind();
            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }


        }

        public void FillAlertHistoryfromSession()
        {
            DataTable AlertHistorytab = new DataTable();
            try
            {

                AlertHistorytab = (DataTable)Session["AlertHistorytab"];
                AlertGridView.DataSource = AlertHistorytab;
                AlertGridView.DataBind();
            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }


        }

        protected void AllDBGridView_HtmlDataCellPrepared(object sender, DevExpress.Web.ASPxGridViewTableDataCellEventArgs e)
        {
            if (e.DataColumn.FieldName == "Status" && (e.CellValue.ToString() == "OK" || e.CellValue.ToString() == "Scanning"))
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
                e.Cell.BackColor = System.Drawing.Color.Blue;
                e.Cell.ForeColor = System.Drawing.Color.White;
            }
            else if (e.DataColumn.FieldName == "Status" && e.CellValue.ToString() == "disabled")
            {
                e.Cell.BackColor = System.Drawing.Color.Gray;
                // e.Cell.ForeColor = System.Drawing.Color.White;
            }
            else if (e.DataColumn.FieldName == "Status")
            {
                e.Cell.BackColor = System.Drawing.Color.Yellow;
                // e.DataColumn.GroupFooterCellStyle.ForeColor = System.Drawing.Color.Yellow;
            }

        }

        protected void BackButton_Click(object sender, EventArgs e)
        {
            if (Session["BackURL"] != "" && Session["BackURL"] != null)
            {
                Response.Redirect(Session["BackURL"].ToString());
                Session["BackURL"] = "";

            }
        }






        protected void OpenDBButton_Click(object sender, EventArgs e)
        {

        }








        protected void ScanButton_Click(object sender, EventArgs e)
        {
            try
            {
                Status StatusObj = new Status();
                StatusObj.Name = Request.QueryString["Name"];
                StatusObj.Type = Request.QueryString["Type"];

                bool bl = VSWebBL.StatusBL.StatusTBL.Ins.UpdateforScan(StatusObj);

                if (Request.QueryString["Type"] == "Domino")
                {
					bl = VSWebBL.SettingBL.SettingsBL.Ins.UpdateScanvalue("ScanDominoASAP", StatusObj.Name, VSWeb.Constants.Constants.SysString);
                }
                else if (Request.QueryString["Type"] == "Mail")
                {
					bl = VSWebBL.SettingBL.SettingsBL.Ins.UpdateScanvalue("ScanMailServiceASAP", StatusObj.Name, VSWeb.Constants.Constants.SysString);
                }
                else if (Request.QueryString["Type"] == "Network Device")
                {
					bl = VSWebBL.SettingBL.SettingsBL.Ins.UpdateScanvalue("ScanNetworkDeviceASAP", StatusObj.Name, VSWeb.Constants.Constants.SysString);
                }
                else if (Request.QueryString["Type"] == "Sametime Server")
                {
					bl = VSWebBL.SettingBL.SettingsBL.Ins.UpdateScanvalue("ScanSametimeASAP", StatusObj.Name, VSWeb.Constants.Constants.SysString);
                }
                else if (Request.QueryString["Type"] == "BES")
                {
					bl = VSWebBL.SettingBL.SettingsBL.Ins.UpdateScanvalue("ScanBlackBerryASAP", StatusObj.Name, VSWeb.Constants.Constants.SysString);
                }
                else if (Request.QueryString["Type"] == "URL")
                {
					bl = VSWebBL.SettingBL.SettingsBL.Ins.UpdateScanvalue("ScanURLASAP", StatusObj.Name, VSWeb.Constants.Constants.SysString);
                }
                else if (Request.QueryString["Type"] == "Exchange")
                {
					bl = VSWebBL.SettingBL.SettingsBL.Ins.UpdateScanvalue("ScanExchangeASAP", StatusObj.Name, VSWeb.Constants.Constants.SysString);
                }
                else if (Request.QueryString["Type"] == "SharePoint")
                {
					bl = VSWebBL.SettingBL.SettingsBL.Ins.UpdateScanvalue("ScanSharePointASAP", StatusObj.Name, VSWeb.Constants.Constants.SysString);
                }
				else if (Request.QueryString["Type"] == "Skype for Business")
                {
					bl = VSWebBL.SettingBL.SettingsBL.Ins.UpdateScanvalue("LyncASAP", StatusObj.Name, VSWeb.Constants.Constants.SysString);
                }
                else if (Request.QueryString["Type"] == "Windows")
                {
					bl = VSWebBL.SettingBL.SettingsBL.Ins.UpdateScanvalue("WindowsASAP", StatusObj.Name, VSWeb.Constants.Constants.SysString);
                }
            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
        }

        protected void EditInConfigButton_Click(object sender, EventArgs e)
        {
            
            string id = "";
            string name = "";
            Session["Submenu"] = "Windows";
            if (Request.QueryString["Name"] != "" && Request.QueryString["Name"] != null)
            {
                id = (VSWebBL.SecurityBL.ServersBL.Ins.GetServerIDbyServerName(Request.QueryString["Name"].ToString())).ToString();
                name = Request.QueryString["Name"].ToString();
                DataTable dt = VSWebBL.SecurityBL.ServersBL.Ins.GetServerDetailsByName1(name);
                string Loc = dt.Rows[0]["Location"].ToString();
                string Cat = dt.Rows[0]["ServerType"].ToString();
                //servernamelbl.Text

                Response.Redirect("~/Configurator/WindowsProperties.aspx?ID=" + id + "&Name=" + name + "&Cat=" + dt.Rows[0]["ServerType"].ToString() + "&Loc=" + dt.Rows[0]["Location"].ToString(), false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
                Context.ApplicationInstance.CompleteRequest();
            }
        }

        protected void BtnApply_Click(object sender, EventArgs e)
        {
            try
            {
                List<string> serverIDValues = new List<string>();
                List<string> servertypeIDValues = new List<string>();
                Random random = new Random((int)DateTime.Now.Ticks);
                //RandomString(5, random);
                string Name = servernamelbl.Text + "-Temp-" + DateTime.Now.ToLongDateString();
                string StartDate = DateTime.Now.ToShortDateString();
                string StartTime = DateTime.Now.ToShortTimeString();
                DateTime sdt = Convert.ToDateTime(StartDate);
                string Duration = TbDuration.Text;
                string EndDate = DateTime.Now.ToShortDateString();
                DateTime edt = Convert.ToDateTime(EndDate);
                string MaintType = "1";
                string MaintDaysList = "";
                string altime = DateTime.Now.ToShortTimeString();
                DateTime al = Convert.ToDateTime(altime);
                ASPxScheduler sh = new ASPxScheduler();
                Appointment apt = sh.Storage.CreateAppointment(AppointmentType.Pattern);
                Reminder r = apt.CreateNewReminder();

                //int min = Convert.ToInt32(MaintDurationTextBox.Text);
                int min = Convert.ToInt32(TbDuration.Text);
                r.AlertTime = al.AddMinutes(min);
                //3/24/2015 NS modified for DevExpress upgrade 14.2
                //ReminderXmlPersistenceHelper reminderHelper = new ReminderXmlPersistenceHelper(r, DateSavingType.LocalTime);
                ReminderXmlPersistenceHelper reminderHelper = new ReminderXmlPersistenceHelper(r);
                string rem = reminderHelper.ToXml().ToString();

                RecurrenceInfo reci = new RecurrenceInfo();
                reci.BeginUpdate();
                reci.AllDay = false;
                reci.Periodicity = 10;
                reci.Range = RecurrenceRange.EndByDate;
                reci.Start = sdt;
                reci.End = edt;
                reci.Duration = edt - sdt;
                reci.Type = RecurrenceType.Yearly;


                OccurrenceCalculator calc = OccurrenceCalculator.CreateInstance(reci);
                TimeInterval ttc = new TimeInterval(reci.Start, reci.End + new TimeSpan(1, 0, 0));


                var bcoll = calc.CalcOccurrences(ttc, apt);
                if (bcoll.Count != 0)
                {
                    reci.OccurrenceCount = bcoll.Count;
                }
                else
                {
                    reci.OccurrenceCount = 1;
                }
                reci.Range = RecurrenceRange.OccurrenceCount;
                reci.EndUpdate();
                string s = reci.ToXml();

                string EndDateIndicator = "";
                DataTable dt = VSWebBL.SecurityBL.ServersBL.Ins.GetServerDetailsByName(servernamelbl.Text);
                if (dt != null && dt.Rows.Count > 0)
                {
                    //VSPLUS-833:Suspend temporarily is not working correctly 
                    //19Jul14, Mukund, The below two were reversely assigned so suspend wasnt working. 
                    servertypeIDValues.Add(dt.Rows[0][2].ToString());
                    serverIDValues.Add(dt.Rows[0][0].ToString());
                }
                bool update = false;
                if (servertypeIDValues != null && servertypeIDValues.Count > 0)
                {
                    update = VSWebBL.ConfiguratorBL.MaintenanceBL.Ins.UpdateMaintenanceWindows(null, Name, StartDate, StartTime, Duration,
                          EndDate, MaintType, MaintDaysList, EndDateIndicator, serverIDValues, s, rem, 1, true, servertypeIDValues, "true", "1");
                }
                if (update == true)
                {
                    SuccessMsg.InnerHtml = "Monitoring for " + servernamelbl.Text + " has been temporarily suspended for a duration of " + TbDuration.Text + " minutes.";
                    SuccessMsg.Style.Value = "display: block";

                }
                else
                {
                    ErrorMsg.InnerHtml = "The Settings were NOT updated";
                    ErrorMsg.Style.Value = "display: block";
                }
                ASPxPopupControl1.ShowOnPageLoad = false;
            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
        }


        protected void maintenancegrid_PageSizeChanged(object sender, EventArgs e)
        {
            //ProfilesGridView.PageIndex;
            VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("DominoServerDetailsPage2|maintenancegrid", maintenancegrid.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        }
        protected void AlertGridView_PageSizeChanged(object sender, EventArgs e)
        {
            //ProfilesGridView.PageIndex;
            VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("DominoServerDetailsPage2|AlertGridView", AlertGridView.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        }
        protected void OutageGridView_PageSizeChanged(object sender, EventArgs e)
        {
            //ProfilesGridView.PageIndex;
            VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("DominoServerDetailsPage2|OutageGridView", OutageGridView.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        }


		protected void ASPxMenu1_ItemClick(object source, DevExpress.Web.MenuItemEventArgs e)
		{
			if (e.Item.Name == "BackItem")
			{
				if (Session["BackURL"] != "" && Session["BackURL"] != null)
				{
					Response.Redirect(Session["BackURL"].ToString());
					Session["BackURL"] = "";

				}
			}
			else if (e.Item.Name == "ScanItem")
			{
				try
				{
					Status StatusObj = new Status();
					StatusObj.Name = Request.QueryString["Name"];
					StatusObj.Type = Request.QueryString["Type"];

					bool bl = VSWebBL.StatusBL.StatusTBL.Ins.UpdateforScan(StatusObj);

					if (Request.QueryString["Type"] == "Domino")
					{
						bl = VSWebBL.SettingBL.SettingsBL.Ins.UpdateScanvalue("ScanDominoASAP", StatusObj.Name, VSWeb.Constants.Constants.SysString);
					}
					else if (Request.QueryString["Type"] == "Mail")
					{
						bl = VSWebBL.SettingBL.SettingsBL.Ins.UpdateScanvalue("ScanMailServiceASAP", StatusObj.Name, VSWeb.Constants.Constants.SysString);
					}
					else if (Request.QueryString["Type"] == "Network Device")
					{
						bl = VSWebBL.SettingBL.SettingsBL.Ins.UpdateScanvalue("ScanNetworkDeviceASAP", StatusObj.Name, VSWeb.Constants.Constants.SysString);
					}
					else if (Request.QueryString["Type"] == "Sametime Server")
					{
						bl = VSWebBL.SettingBL.SettingsBL.Ins.UpdateScanvalue("ScanSametimeASAP", StatusObj.Name, VSWeb.Constants.Constants.SysString);
					}
					else if (Request.QueryString["Type"] == "BES")
					{
						bl = VSWebBL.SettingBL.SettingsBL.Ins.UpdateScanvalue("ScanBlackBerryASAP", StatusObj.Name, VSWeb.Constants.Constants.SysString);
					}
					else if (Request.QueryString["Type"] == "URL")
					{
						bl = VSWebBL.SettingBL.SettingsBL.Ins.UpdateScanvalue("ScanURLASAP", StatusObj.Name, VSWeb.Constants.Constants.SysString);
					}
					else if (Request.QueryString["Type"] == "Exchange")
					{
						bl = VSWebBL.SettingBL.SettingsBL.Ins.UpdateScanvalue("ScanExchangeASAP", StatusObj.Name, VSWeb.Constants.Constants.SysString);
					}
					else if (Request.QueryString["Type"] == "SharePoint")
					{
						bl = VSWebBL.SettingBL.SettingsBL.Ins.UpdateScanvalue("ScanSharePointASAP", StatusObj.Name, VSWeb.Constants.Constants.SysString);
					}
					else if (Request.QueryString["Type"] == "Skype for Business")
					{
						bl = VSWebBL.SettingBL.SettingsBL.Ins.UpdateScanvalue("LyncASAP", StatusObj.Name, VSWeb.Constants.Constants.SysString);
					}
					else if (Request.QueryString["Type"] == "Lync")
					{
						bl = VSWebBL.SettingBL.SettingsBL.Ins.UpdateScanvalue("WindowsASAP", StatusObj.Name, VSWeb.Constants.Constants.SysString);
					}
				}
				catch (Exception ex)
				{
					Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
					throw ex;
				}
			}
			else if (e.Item.Name == "EditConfigItem")
			{
				string id = "";
				Session["Submenu"] = "LotusDomino";
				if (Request.QueryString["Name"] != "" && Request.QueryString["Name"] != null)
				{
					//string Name = Request.QueryString["Name"].ToString();
					//id = (VSWebBL.SecurityBL.ServersBL.Ins.GetServerIDbyServerName(Request.QueryString["Name"].ToString())).ToString();
					//Response.Redirect("~/Configurator/WindowsProperties.aspx?Key=" + id + "&Name=" +Name , false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
					//Context.ApplicationInstance.CompleteRequest();
					id = (VSWebBL.SecurityBL.ServersBL.Ins.GetServerIDbyServerName(Request.QueryString["Name"].ToString())).ToString();
					string name = Request.QueryString["Name"].ToString();
					DataTable dt = VSWebBL.SecurityBL.ServersBL.Ins.GetServerDetailsByName1(name);
					string Loc = dt.Rows[0]["Location"].ToString();
					string Cat = dt.Rows[0]["ServerType"].ToString();
					Response.Redirect("~/Configurator/WindowsProperties.aspx?ID=" + id + "&Name=" + name + "&Cat=" + dt.Rows[0]["ServerType"].ToString() + "&Loc=" + dt.Rows[0]["Location"].ToString(), false);
					Context.ApplicationInstance.CompleteRequest();
				}
			}
			else if (e.Item.Name == "SuspendItem")
			{
				ASPxPopupControl1.HeaderText = "Suspend Temporarily";
				ASPxPopupControl1.ShowOnPageLoad = true;
			}
		}
		public void FillUnMonitoredServicesGrid()
		{

			try
			{
				if (VSWebBL.DashboardBL.ExchangeServerDetailsBL.Ins.isResponding(servernamelbl.Text + "-Exchange"))
				{
					DataTable StatusTable1 = VSWebBL.DashboardBL.ExchangeServerDetailsBL.Ins.GetUnmonitoredWindowsServices(servernamelbl.Text);
					Session["UnMonitoredServices"] = StatusTable1;
					UnMonitoredServicesGrid.DataSource = StatusTable1;
					UnMonitoredServicesGrid.DataBind();
				}


			}
			catch (Exception ex)
			{
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}
			finally
			{
			}
		}
		protected void MonitoredServicesGrid_PageSizeChanged(object sender, EventArgs e)
		{
			//ProfilesGridView.PageIndex;
			VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("WindowsServerDetailspage|MonitoredServicesGrid", MonitoredServicesGrid.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
			Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
		}
		protected void MonitoredServicesGrid_HtmlDataCellPrepared(object sender, DevExpress.Web.ASPxGridViewTableDataCellEventArgs e)
		{
			if (e.DataColumn.FieldName == "Result" && (e.CellValue.ToString() == "Running"))
			{
				e.Cell.BackColor = System.Drawing.Color.LightGreen;
			}
			else if (e.DataColumn.FieldName == "Result" && (e.CellValue.ToString() == "Stopped"))
			{
				e.Cell.BackColor = System.Drawing.Color.Red;
				e.Cell.ForeColor = System.Drawing.Color.White;
			}
			else if (e.DataColumn.FieldName == "Result")
			{
				e.Cell.BackColor = System.Drawing.Color.Yellow;
			}
		
		}
		public void FillMonitoredServicesGrid()
		{

			try
			{
				if (VSWebBL.DashboardBL.ExchangeServerDetailsBL.Ins.isResponding(servernamelbl.Text + "-Exchange"))
				{
					DataTable StatusTable1 = VSWebBL.DashboardBL.ExchangeServerDetailsBL.Ins.GetWindowsServices(servernamelbl.Text);
					Session["MonitoredServices"] = StatusTable1;
					MonitoredServicesGrid.DataSource = StatusTable1;
					MonitoredServicesGrid.DataBind();
					(MonitoredServicesGrid.Columns["Type"] as GridViewDataColumn).GroupBy();
				}


			}
			catch (Exception ex)
			{
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}
			finally
			{
			}
		}
		protected void UnMonitoredServicesGrid_PageSizeChanged(object sender, EventArgs e)
		{
			//ProfilesGridView.PageIndex;
			VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("WindowsServerDetailspage|UnMonitoredServicesGrid", UnMonitoredServicesGrid.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
			Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
		}
		public void FillMonitoredServicesGridfromSession()

		{
			DataTable MonitoredServices = new DataTable();
			try
			{

				MonitoredServices = (DataTable)Session["MonitoredServices"];
				MonitoredServicesGrid.DataSource = MonitoredServices;
				MonitoredServicesGrid.DataBind();
			}
			catch (Exception ex)
			{
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}


		}
		public void FillUnMonitoredServicesGridfromSession()
		{
			DataTable MonitoredServices = new DataTable();
			try
			{

				MonitoredServices = (DataTable)Session["UnMonitoredServices"];
				UnMonitoredServicesGrid.DataSource = MonitoredServices;
				UnMonitoredServicesGrid.DataBind();
			}
			catch (Exception ex)
			{
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}


		}
    }
}

