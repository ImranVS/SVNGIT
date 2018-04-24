using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DevExpress.Web;
using DevExpress.XtraCharts;
using System.Web.UI.HtmlControls;
using System.Drawing;
namespace VSWebUI.Dashboard
{
    public partial class NotesMailProbeDetailsPage : System.Web.UI.Page
    {
		protected void Page_PreInit(object sender, EventArgs e)
		{

			this.Master.contentCallEvent += new EventHandler(Master_ButtonClick);
		}

		private void Master_ButtonClick(object sender, EventArgs e)
		{

		}
        protected void Page_Load(object sender, EventArgs e)
        {
            //11/7/2014 NS modified for VSPLUS-1153
            performanceWebChartControl.Width = new Unit(Convert.ToInt32(chartWidth.Value));
            SetGraph("hh", Request.QueryString["Name"].ToString());
            if (!IsPostBack)
            {
                HtmlGenericControl body = (HtmlGenericControl)Master.FindControl("Body1");
                body.Attributes.Add("onload", "DoCallback()");
                body.Attributes.Add("onResize", "Resized()");
                //11/7/2014 NS added for VSPLUS-1153
                if (Request.QueryString["Type"] != "" && Request.QueryString["Type"] != null)
                {
                    lblServerType.Text = Request.QueryString["Type"].ToString() + ":  ";
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
                //performanceASPxRoundPanel.Width = 1000;
                if (Request.QueryString["Server"] != "" && Request.QueryString["Server"] != null)
                {
                    SetGridFromQString(Request.QueryString["Server"]);
                    
                }
                else
                {
                    servernamelbl.Text = Request.QueryString["Name"];
                    //11/7/2014 NS modified for VSPLUS-1153
                    //Lastscanned.Text = Request.QueryString["LastDate"].ToString();
                    servernamelbldisp.InnerHtml = lblServerType.Text + servernamelbl.Text;
                    Lastscanned.InnerHtml = "Last scan date: " + Request.QueryString["LastDate"].ToString();
                    NotesMailProbe MailObj=new NotesMailProbe();
                    MailObj.NotesMailAddress = "";
                    MailObj.Name = servernamelbl.Text;

                    DataTable dtNotes = VSWebBL.ConfiguratorBL.NotesMailProbeBL.Ins.GetAllDataByName(MailObj);
                    if (dtNotes.Rows.Count > 0)
                    {
                        lblSource.Text = dtNotes.Rows[0]["SourceServer"].ToString();
                        lblDestination.Text = dtNotes.Rows[0]["DestinationServer"].ToString();
                        lblDestination0.Text = lblDestination.Text;
                        lblDestination1.Text = lblDestination.Text;
                        lblNotesMailAddress.Text = dtNotes.Rows[0]["NotesMailAddress"].ToString();
                        lblDeliveryThreshold.Text = dtNotes.Rows[0]["DeliveryThreshold"].ToString();
                        lblScanInterval.Text = dtNotes.Rows[0]["ScanInterval"].ToString();
                        lblOffHoursScanInterval.Text = dtNotes.Rows[0]["OffHoursScanInterval"].ToString();
                        lblDestinationDatabase.Text = dtNotes.Rows[0]["DestinationDatabase"].ToString();
                    }
                    else
                    {
                        NotesInfo.Visible = false;
                    }
                    SetGrid();                    
                }
                if (Session["UserPreferences"] != null)
                {
                    DataTable UserPreferences = (DataTable)Session["UserPreferences"];
                    foreach (DataRow dr in UserPreferences.Rows)
                    {
                        if (dr[1].ToString() == "NotesMailProbeDetailsPage|DiskHealthGrid")
                        {
                            DiskHealthGrid.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                        }
                    }
                }
            }
            else
            {                
                if (Request.QueryString["Server"] != "" && Request.QueryString["Server"] != null)
                {
                    SetGridFromQString(Request.QueryString["Server"]);                    
                }
                else
                {
                    SetGridFromSession();                    
                }
            }
        }

        public void SetGrid()
        {
            try
            {
                DataTable dt = VSWebBL.DashboardBL.DiskHealthBLL.Ins.SetGridforNotes(servernamelbl.Text);
                Session["GridData"] = dt;
                DiskHealthGrid.DataSource = dt;
                DiskHealthGrid.DataBind();
                //((GridViewDataColumn)DiskHealthGrid.Columns["DeviceName"]).GroupBy();
                int rowIndex = DiskHealthGrid.FindVisibleIndexByKeyValue("ProbeID");
                //Session["rowIndex"] = rowIndex;
            }
            catch (Exception ex)
            {
                Response.Write("Error : " + ex);
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
        }



        public void SetGridFromSession()
        {
            try
            {
                if (Session["GridData"] != null)
                {
                    DataTable dt = Session["GridData"] as DataTable;
                    DiskHealthGrid.DataSource = dt;
                    DiskHealthGrid.DataBind();
                    int rowIndex = DiskHealthGrid.FindVisibleIndexByKeyValue("ProbeID");
                    Session["rowIndex"] = rowIndex;
                }
            }
            catch (Exception ex)
            {
                Response.Write("Error : " + ex);
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
        }

        public void SetGridFromQString(string serverName )
        {
            try
            {
                DataTable dt = VSWebBL.DashboardBL.DiskHealthBLL.Ins.SetGridFromQString(serverName);
                DiskHealthGrid.DataSource = dt;
                DiskHealthGrid.DataBind();
                ((GridViewDataColumn)DiskHealthGrid.Columns["DeviceName"]).GroupBy();                
            }
            catch (Exception ex)
            {
                Response.Write("Error : " + ex);
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
        }


        public DataTable SetGraph(string paramGraph, string serverName)
        {
            try
            {
                performanceWebChartControl.Series.Clear();
                DataTable dt = VSWebBL.DashboardBL.DominoServerDetailsBL.Ins.SetGraphforNotes(paramGraph, serverName);

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

                // Create a constant line.
                ConstantLine constantLine1 = new ConstantLine("Constant Line 1");
                diagram.AxisY.ConstantLines.Add(constantLine1);

                // Define its axis value.
                constantLine1.AxisValue = 2;

                // Customize the behavior of the constant line.
                // constantLine1.Visible = true;
                //constantLine1.ShowInLegend = true;
                // constantLine1.LegendText = "Some Threshold";
                constantLine1.ShowBehind = true;

                // Customize the constant line's title.
                constantLine1.Title.Visible = true;
                constantLine1.Title.Text = "Threshold:2";
                constantLine1.Title.TextColor = Color.Red;
                // constantLine1.Title.Antialiasing = false;
                //constantLine1.Title.Font = new Font("Tahoma", 14, FontStyle.Bold);
                constantLine1.Title.ShowBelowLine = true;
                constantLine1.Title.Alignment = ConstantLineTitleAlignment.Far;

                // Customize the appearance of the constant line.
                constantLine1.Color = Color.Red;
                constantLine1.LineStyle.DashStyle = DashStyle.Solid;
                constantLine1.LineStyle.Thickness = 2;

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

        protected void BackButton_Click(object sender, EventArgs e)
        {
            if (Session["BackURL"] != "" && Session["BackURL"] != null)
            {
                Response.Redirect(Session["BackURL"].ToString());
                Session["BackURL"] = "";

            }
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
                else if (Request.QueryString["Type"] == "NotesMail Probe")
                {
					bl = VSWebBL.SettingBL.SettingsBL.Ins.UpdateScanvalue("ScanNotesMailProbeASAP", StatusObj.Name, VSWeb.Constants.Constants.SysString);
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
            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
        }

        protected void EditInConfigButton_Click(object sender, EventArgs e)
        {
            //1/2/2014 NS added
            string id = "";
            Session["Submenu"] = "NotesMailProbe";
            if (Request.QueryString["Name"] != "" && Request.QueryString["Name"] != null)
            {
                id = (VSWebBL.SecurityBL.ServersBL.Ins.GetServerIDbyServerName(Request.QueryString["Name"].ToString())).ToString();
                Response.Redirect("~/Configurator/NotesMailprobeGrid.aspx?Key=" + id, false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
                Context.ApplicationInstance.CompleteRequest();
            }
        }

        //public void SetGraph(string serverName)
        //{
        //    try
        //    {
        //        DataTable dt = new DataTable();

        //        webChartDiskHealth.Series.Clear();
        //        dt = VSWebBL.DashboardBL.DiskHealthBLL.Ins.SetGraph(serverName);
        //        webChartDiskHealth.DataSource = dt;

        //        double[] double1 = new double[dt.Rows.Count];
        //        double[] double2 = new double[dt.Rows.Count];

        //        Series series = null;

        //        for (int i = 0; i < dt.Rows.Count; i++)
        //        {
        //            if (series != null)
        //            {
        //                webChartDiskHealth.Series.Add(series);
        //                webChartDiskHealth.DataBind();
        //            }

        //            series = new Series(dt.Rows[i]["DiskName"].ToString(), ViewType.Pie);

        //            string val1 = dt.Rows[i]["DiskFree"].ToString();
        //            string val2 = dt.Rows[i]["DiskUsed"].ToString();

        //            if (val1 != "" && val2 != "")
        //            {
        //                double1[i] = Convert.ToDouble(dt.Rows[i]["DiskFree"].ToString());
        //                double2[i] = Convert.ToDouble(dt.Rows[i]["DiskUsed"].ToString());

        //                series.Points.Add(new SeriesPoint("DiskFree", double1[i]));
        //                series.Points.Add(new SeriesPoint("DiskUsed", double2[i]));
        //                series.ShowInLegend = true;

        //                series.LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;
        //                PieSeriesLabel seriesLabel = (PieSeriesLabel)series.Label;
        //                seriesLabel.Position = PieSeriesLabelPosition.Radial;
        //                seriesLabel.BackColor = System.Drawing.Color.Transparent;
        //                seriesLabel.TextColor = System.Drawing.Color.Black;

        //                PieSeriesView seriesView = (PieSeriesView)series.View;
        //                seriesView.Titles.Add(new SeriesTitle());
        //                seriesView.Titles[0].Dock = ChartTitleDockStyle.Bottom;

        //                seriesView.Titles[0].Text = series.Name.Substring(5);
        //                seriesView.Titles[0].Visible = true;
        //                seriesView.Titles[0].WordWrap = true;
        //            }
        //        }
        //        if (series != null)
        //        {
        //            webChartDiskHealth.Series.Add(series);
        //            webChartDiskHealth.DataBind();
        //        }

        //        for (int c = 0; c < webChartDiskHealth.Series.Count; c++)
        //        {
        //            if (c == 0)
        //            {
        //                PiePointOptions seriesPointOptions = (PiePointOptions)series.LegendPointOptions;
        //                series.LegendPointOptions.PointView = PointView.Argument;
        //                webChartDiskHealth.Series[0].ShowInLegend = true;
        //                webChartDiskHealth.Series[0].LegendPointOptions.PointView = PointView.Argument;
        //                webChartDiskHealth.Series[0].ShowInLegend = true;
        //                webChartDiskHealth.Legend.Visible = true;
        //            }
        //            else
        //            {
        //                webChartDiskHealth.Series[c].ShowInLegend = false;
        //            }
        //        }

        //        ChartTitle title = new ChartTitle();
        //        title.Text = serverName;
        //        webChartDiskHealth.Titles.Clear();
        //        webChartDiskHealth.Titles.Add(title);
        //    }
        //    catch (Exception ex)
        //    {
        //        Response.Write("Error : " + ex);
        //    }
        //}

        protected void ASPxCallbackPanel1_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
        {
            if (Request.QueryString["Server"] != "" && Request.QueryString["Server"] != null)
            {
                SetGridFromQString(Request.QueryString["Server"]);
                lblServerName.Text = Request.QueryString["Server"];
                SetGraph("hh",lblServerName.Text);
            }
            else
            {
                //lblServerName.Text = "";
                int index = DiskHealthGrid.FocusedRowIndex;
                //8/9/2013 NS modified - when no rows were found, object reference not set error appeared
                if (index > -1)
                {
                    webChartDiskHealth.Visible = true;
                    string value = DiskHealthGrid.GetRowValues(index, "DeviceName").ToString();
                    lblServerName.Text = value;
                    SetGraph("hh",value);
                }
                else
                {
                    webChartDiskHealth.Visible = false;
                }
            }
        }

        //protected void btnCollapse_Click(object sender, EventArgs e)
        //{
        //     if (btnCollapse.Text == "Collapse All Rows")
        //    {
        //        DiskHealthGrid.CollapseAll();
        //        btnCollapse.Image.Url = "~/images/icons/add.png"; 
        //        btnCollapse.Text = "Expand All Rows";
        //    }
        //    else
        //    {
        //        DiskHealthGrid.ExpandAll();
        //        btnCollapse.Image.Url = "~/images/icons/forbidden.png";
        //        btnCollapse.Text = "Collapse All Rows";

        //    }
               
        //    if (Request.QueryString["Server"] != "" && Request.QueryString["Server"] != null)
        //    {
        //        SetGridFromQString(Request.QueryString["Server"]);
        //        lblServerName.Text = Request.QueryString["Server"];
        //        SetGraph(lblServerName.Text);
        //    }
        //    else
        //    {
        //        if (Session["ServerName"] != null)
        //        {
        //            int index = DiskHealthGrid.FocusedRowIndex;
        //            string value = DiskHealthGrid.GetRowValues(index, "ServerName").ToString();
        //            lblServerName.Text = value;
        //            SetGraph(lblServerName.Text);
        //        }
        //        else
        //        {
        //            lblServerName.Text = "";
        //           // int index = DiskHealthGrid.FocusedRowIndex;
        //            string value = DiskHealthGrid.GetRowValues(0, "ServerName").ToString();
        //            lblServerName.Text = value;
        //            SetGraph(lblServerName.Text);
        //        }
        //    }

        //    //int index = Convert.ToInt32(DiskHealthGrid.FocusedRowIndex);
        //    //int exCount = 0;
        //    //for (int i = DiskHealthGrid.SettingsPager.PageSize * DiskHealthGrid.PageIndex; i < index; i++)
        //    //{
        //    //    if (DiskHealthGrid.IsRowExpanded(i))
        //    //        exCount += 1;
        //    //}
        //    //DiskHealthGrid.CollapseAll();
        //    //DiskHealthGrid.FocusedRowIndex = index - exCount / 2;    

            
        //}

        //protected void btnExpand_Click(object sender, EventArgs e)
        //{
        //    //DiskHealthGrid.ExpandAll();

        //    //if (Request.QueryString["Server"] != "" && Request.QueryString["Server"] != null)
        //    //{
        //    //    SetGridFromQString(Request.QueryString["Server"]);
        //    //    lblServerName.Text = Request.QueryString["Server"];
        //    //    SetGraph(lblServerName.Text);
        //    //}
        //    //else
        //    //{
        //    //    lblServerName.Text = "";
        //    //    int index = DiskHealthGrid.FocusedRowIndex;
        //    //    string value = DiskHealthGrid.GetRowValues(index, "ServerName").ToString();
        //    //    lblServerName.Text = value;
        //    //    SetGraph(lblServerName.Text);
        //    //    //Session["Index"] = index;
        //    //}            
        //}

        protected void DiskHealthGrid_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            if (e.DataColumn.FieldName == "Status" && (e.CellValue.ToString() == "OK" || e.CellValue.ToString() == "Scanning" || e.CellValue.ToString() == "Telnet"))
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
            else if (e.DataColumn.FieldName == "Status" && e.CellValue.ToString() == "Disabled")
            {
                e.Cell.BackColor = System.Drawing.Color.FromName("#D0D0D0");
                //e.Cell.BackColor = System.Drawing.Color.Gray;
                // e.Cell.ForeColor = System.Drawing.Color.White;
            }
            else if (e.DataColumn.FieldName == "Status")
            {
                e.Cell.BackColor = System.Drawing.Color.Yellow;
                // e.DataColumn.GroupFooterCellStyle.ForeColor = System.Drawing.Color.Yellow;

            }
        }

        protected void DiskHealthGrid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
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

        protected void DiskHealthGrid_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
        {
            int index = DiskHealthGrid.FocusedRowIndex;
            if (e.VisibleIndex != index)
            {
                e.Row.Attributes.Add("onmouseover", "this.originalcolor=this.style.backgroundColor;" + "this.style.backgroundColor='#C0C0C0';");

                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalcolor;");

            }
        }

        protected void webChartDiskHealth_CustomCallback(object sender, DevExpress.XtraCharts.Web.CustomCallbackEventArgs e)
        {
            webChartDiskHealth.Width = new Unit(Convert.ToInt32(chartWidth.Value));
        }

        protected void DiskHealthGrid_PageSizeChanged(object sender, EventArgs e)
        {
            //ProfilesGridView.PageIndex;
            VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("NotesMailProbeDetailsPage|DiskHealthGrid", DiskHealthGrid.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
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
                    else if (Request.QueryString["Type"] == "NotesMail Probe")
                    {
						bl = VSWebBL.SettingBL.SettingsBL.Ins.UpdateScanvalue("ScanNotesMailProbeASAP", StatusObj.Name, VSWeb.Constants.Constants.SysString);
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
                Session["Submenu"] = "NotesMailProbe";
                if (Request.QueryString["Name"] != "" && Request.QueryString["Name"] != null)
                {
                    id = (VSWebBL.SecurityBL.ServersBL.Ins.GetServerIDbyServerName(Request.QueryString["Name"].ToString())).ToString();
                    //11/7/2014 NS modified for VSPLUS-1153
                    //Response.Redirect("~/Configurator/NotesMailprobeGrid.aspx?Key=" + id, false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
                    Response.Redirect("~/Configurator/EditNotesMailProbe.aspx?Name=" + Request.QueryString["Name"].ToString(), false);
                    Context.ApplicationInstance.CompleteRequest();
                }
            }
        }

    }
}