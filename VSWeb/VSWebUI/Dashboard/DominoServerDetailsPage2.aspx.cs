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
using System.Globalization;
using DevExpress.Utils;
using System.Web.UI.HtmlControls;
//using VSWebBL;

namespace VSWebUI.Configurator
{
    public partial class DominoServerDetailsPage2 : System.Web.UI.Page
    {
        protected void Page_PreInit(object sender, EventArgs e)
        {

            this.Master.contentCallEvent += new EventHandler(Master_ButtonClick);
            //9/14/2015 NS added for VSPLUS-2148
            if (Request.QueryString["Type"] == "Traveler")
            {
                ASPxPageControl1.TabPages[2].Visible = true;
            }
            else
            {
                ASPxPageControl1.TabPages[2].Visible = false;
            }
                
        }

        private void Master_ButtonClick(object sender, EventArgs e)
        {

        }
        string url = "";
        string cellname;
        protected void Page_Load(object sender, EventArgs e)
        {
            //sowmya
            HtmlGenericControl body = (HtmlGenericControl)Master.FindControl("Body1");
            this.DeviceSyncsWebChart.Width = new Unit(Convert.ToInt32(chartWidth.Value) / 2);
            this.httpSessionsWebChart.Width = new Unit(Convert.ToInt32(chartWidth.Value) / 2);

            this.JavaMemWebChart.Width = new Unit(Convert.ToInt32(chartWidth.Value) / 2);
            this.CMemWebChart.Width = new Unit(Convert.ToInt32(chartWidth.Value) / 2);

            DataTable Traverdt = VSWebBL.DashboardBL.DominoServerDetailsBL.Ins.GetdataForTraveler(Request.QueryString["Name"]);
            if (Traverdt.Rows.Count>0)
            {
                grid.Visible = true;
            }
               
                if (Session["UserPreferences"] != null)
                {
                    DataTable UserPreferences = (DataTable)Session["UserPreferences"];
                    foreach (DataRow dr in UserPreferences.Rows)
                    {
                        if (dr[1].ToString() == "DominoServerDetailsPage2|MonitoredDBGridView")
                        {
                            MonitoredDBGridView.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                        }
                        if (dr[1].ToString() == "AllDBGridView_PageSizeChanged")
                        {
                            AllDBGridView.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                        }
                        if (dr[1].ToString() == "DominoServerDetailsPage2|DominoserverTasksgrid")
                        {
                            DominoserverTasksgrid.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                        }
                        if (dr[1].ToString() == "DominoServerDetailsPage2|NonmoniterdGrid")
                        {
                            NonmoniterdGrid.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                        }
                        if (dr[1].ToString() == "DominoServerDetailsPage2|maintenancegrid")
                        {
                            maintenancegrid.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                        }
                        if (dr[1].ToString() == "DominoServerDetailsPage2|AlertGridView")
                        {
                            AlertGridView.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                        }
                        if (dr[1].ToString() == "DominoServerDetailsPage2|OutageGridView")
                        {
                            OutageGridView.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                        }
                    }
                }

           
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
                    mailASPxRoundPanel.Visible = false;
                    performanceASPxRoundPanel.Width = 1000;
                    performanceWebChartControl.Width = 800;
                    ASPxPageControl1.TabPages[1].ClientVisible = false;
                    ASPxPageControl1.TabPages[2].ClientVisible = false;
                }
                //9/19/2014 NS added for VSPLUS-934
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
                    //9/19/2014 NS added for VSPLUS-934
                    servernamelbldisp.InnerHtml = lblServerType.Text + servernamelbl.Text;
                    linkDiskSpaceMore.NavigateUrl = "DiskHealth.aspx?server=" + servernamelbl.Text;
                    CpuHyperLink.NavigateUrl = "DominoServerStatisticsDetail.aspx?name=Cpu&server=" + servernamelbl.Text;
                    MemHyperLink.NavigateUrl = "DominoServerStatisticsDetail.aspx?name=Mem&server=" + servernamelbl.Text;
                    PerfrmHyperLink.NavigateUrl = "DominoServerStatisticsDetail.aspx?name=Perfm&server=" + servernamelbl.Text;
                }
                if (Request.QueryString["LastDate"] != "" && Request.QueryString["LastDate"] != null)
                {
                    //9/19/2014 NS modified for VSPLUS-934
                    //Lastscanned.Text = Request.QueryString["LastDate"].ToString();
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

                //10/6/2015 NS added for VSPLUS-2252
                try
                {
                    string type = "";
                    if (Request.QueryString["Type"].ToString() == "Traveler")
                    {
                        type = "Domino";
                    }
                    else
                    {
                        type = Request.QueryString["Type"].ToString();
                    }
                    DataRow row = VSWebBL.DashboardBL.DominoServerDetailsBL.Ins.GetSysInfoData(Request.QueryString["Name"].ToString(), type.ToString()).Rows[0];
                    PlatformValueLabel1.Text = row["OperatingSystem"].ToString();
                    DomVersionValueLabel1.Text = row["DominoVersion"].ToString();
                    
                    DataTable dt = VSWebBL.DashboardBL.DominoServerDetailsBL.Ins.GetServerDetailsData(Request.QueryString["Name"].ToString());
                    if (dt.Rows.Count > 0)
                    {
                        TimeSpan t = TimeSpan.FromSeconds(int.Parse(dt.Rows[0]["ElapsedTimeSeconds"].ToString()));

                        string answer = string.Format("{0:D1} days {1:D2}:{2:D2}:{3:D2}",
                                        t.Days,
                                        t.Hours,
                                        t.Minutes,
                                        t.Seconds
                                        );
                        if (answer != "")
                        {
                            ElapsedTimeValueLabel1.Text = answer;
                            elapsedtimeDiv.Style.Value = "display: block";
                        }
                        CPUCountValueLabel1.Text = dt.Rows[0]["CPUCount"].ToString() + " CPU";
                        ArchitectureValueLabel1.Text = dt.Rows[0]["VersionArchitecture"].ToString();
                        if (ArchitectureValueLabel1.Text != "")
                        {
                            ArchitectureValueLabel1.Text += " ";
                        }
                        if (PlatformValueLabel1.Text != "" || DomVersionValueLabel1.Text != "" || CPUCountValueLabel1.Text != "" || ArchitectureValueLabel1.Text != "")
                        {
                            sysinfoDiv.Style.Value = "display: block";
                        }
                    }
                }
                catch (Exception ex)
                {
                    ErrorMsg.InnerHtml = "The following error has occurred when trying to get Domino data from SQL: " + ex.Message +
                        "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                    ErrorMsg.Style.Value = "display: block";
                }
            }
            // FillCombobox();
          
            //01/23/2014 MD changed it for every page load for the graphs to be populated always.
             SetGrid1(servernamelbl.Text);
            SetGraph("hh", servernamelbl.Text);
            //SetGraphForDiskSpace(servernamelbl.Text,"Disk.C");
            SetGrid(servernamelbl.Text);
            SetGraphForCPU("hh", servernamelbl.Text);
          SetGraphForMemory("hh", servernamelbl.Text);
            SetGraphForUsers("hh", servernamelbl.Text);
            SetGraphForMail(servernamelbl.Text);
            //7/14/2014 NS added for VSPLUS-813
            //5/6/2015 NS modified for VSPLUS-1707
            SetGraphForDiskSpace2("'" + servernamelbl.Text + "'");
            DiskSpaceWebChartControl1.ClientVisible = true;
            DiskHealthGrid.ClientVisible = false;
            //12/4/2013 NS commented out - the page should only load on tab click
            //2/12/2014 NS uncommented out - tab click event takes too long, we will use a link to open Web Admin in a new window instead
            SetupWebAdmin();
            Session["GridData1"] = SetGrid1(servernamelbl.Text);
           grid.DataSource = (DataTable)Session["GridData1"];
            grid.DataBind();
            //if (grid.VisibleRowCount > 0)
            //{
            //    int index = grid.FocusedRowIndex;
            //    if (index > -1)
            //    {
            //        cellname = grid.GetRowValues(index, "Name").ToString();
            //    }
            //}

            DataTable dt2 = new DataTable();

            dt2 = (DataTable)Session["GridData1"];
            for (int i = 0; i < dt2.Rows.Count; i++)
            {
                if (dt2.Rows[i]["Status"].ToString() == "Yellow" || dt2.Rows[i]["Status"].ToString() == "Red")
                {
                    if (dt2.Rows[i]["Reasons"].ToString() == "" || dt2.Rows[i]["Reasons"].ToString() == null)
                    {
                        dt2.Rows[i]["Reasons"] = "Status detail is available for Traveler 9.0.1.4 and higher";
                    }
                }
                else if (dt2.Rows[i]["Status"].ToString() == "Fail")
                {
                    dt2.Rows[i]["Reasons"] = dt2.Rows[i]["Details"].ToString();
                }
                else
                {
                    if (dt2.Rows[i]["Reasons"].ToString() == "" || dt2.Rows[i]["Reasons"].ToString() == null)
                    {
                        dt2.Rows[i]["Reasons"] = "No Traveler-specific issues detected";
                    }
                }
            }
            // DataBase Code

            // performanceASPxRadioButtonList.Items.RemoveAt(0);
            // performanceASPxRadioButtonList.Items[0].Selected = true;

            if (!IsPostBack && !IsCallback)
            {
                //8/26/2015 NS modified
               // HtmlGenericControl body = (HtmlGenericControl)Master.FindControl("Body1");
                body.Attributes.Add("onload", "DoCallback()");
                body.Attributes.Add("onResize", "Resized()");

                FillGrid();
                FillALLGrid();
                FillMonitoredGrid();
                FillMonMoniteredGrid();
                Fillmaintenancegrid();
                FillAlertHistory();
                FillOutageTab();
               
                FillHealthAssessmentStatusGrid(Request.QueryString["Type"].ToString(), servernamelbl.Text);

                if (Session["UserPreferences"] != null)
                {
                    DataTable UserPreferences = (DataTable)Session["UserPreferences"];
                    foreach (DataRow dr in UserPreferences.Rows)
                    {
                        if (dr[1].ToString() == "DominoServerDetailsPage2|MonitoredDBGridView")
                        {
                            MonitoredDBGridView.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                        }
                        if (dr[1].ToString() == "DominoServerDetailsPage2|AllDBGridView")
                        {
                            AllDBGridView.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                        }
                        if (dr[1].ToString() == "DominoServerDetailsPage2|DominoserverTasksgrid")
                        {
                            DominoserverTasksgrid.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                        }
                        if (dr[1].ToString() == "DominoServerDetailsPage2|NonmoniterdGrid")
                        {
                            NonmoniterdGrid.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                        }
                        if (dr[1].ToString() == "DominoServerDetailsPage2|maintenancegrid")
                        {
                            maintenancegrid.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                        }
                        if (dr[1].ToString() == "DominoServerDetailsPage2|AlertGridView")
                        {
                            AlertGridView.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                        }

                        if (dr[1].ToString() == "DominoServerDetailsPage2|OutageGridView")
                        {
                            OutageGridView.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                        }
                        //8/25/2015 NS added
                        if (dr[1].ToString() == "TravelerUsersDevicesOS|TravelerGrid")
                        {
                            TravelerGrid.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                        }
                    }
                }
            }


            else
            {
                //2/6/2014 NS commented out the calls below - will move them into a tab click event and load only the ones
                //relevant to the tab
                //2/12/2014 NS uncommented out - the tab click event takes too long, tab loading looks like it's not happening
                FillGridfromSession();
                SetGridFromSession();
                FillALLGridfromSession();
                FillgridfromSession();
                FillHealthAssessmentFromSession();
                FillNonMongridfromSession();
                Fillmaintenancegridfromsession();
                FillAlertHistoryfromSession();
                FilloutagefromSession();
            }
            //8/25/2015 NS added
            //9/14/2015 NS modified for VSPLUS-2148
            if (Request.QueryString["Type"] == "Traveler")
            {
                MailServerlbl.InnerHtml = servernamelbl.Text + "'" + "s access to mail servers";
                TravelerGrid.DataSource = SetGridForTravelerInterval(servernamelbl.Text);
                TravelerGrid.DataBind();

                RecalculateDeviceSyncChart(servernamelbl.Text);
                SetGraphForHttpSessions(httpSessionsASPxRadioButtonList.Value.ToString(), servernamelbl.Text);
                //10/8/2015 NS added for VSPLUS-2208
                SetGraphForJavaMemory(httpSessionsASPxRadioButtonList.Value.ToString(), servernamelbl.Text);
                SetGraphForCMemory(httpSessionsASPxRadioButtonList.Value.ToString(), servernamelbl.Text);
            }
            //2/6/2013 NS added
            UpdateButtonVisibility();
            //9/30/2014 NS added for VSPLUS-934
            ASPxPopupControl1.ShowOnPageLoad = false;
        }
        public void SetGrid(string ServerName)
        {
            try
            {
                DataTable dt = VSWebBL.DashboardBL.DiskHealthBLL.Ins.SetGrid(ServerName);
                Session["GridData"] = dt;
                DiskHealthGrid.DataSource = dt;
                DiskHealthGrid.DataBind();
                //((GridViewDataColumn)DiskHealthGrid.Columns["ServerName"]).GroupBy();
                int rowIndex = DiskHealthGrid.FindVisibleIndexByKeyValue("ID");
                //Session["rowIndex"] = rowIndex;
            }
            catch (Exception ex)
            {
                //Response.Write("Error : " + ex);
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
                    int rowIndex = DiskHealthGrid.FindVisibleIndexByKeyValue("ID");
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
        protected void DiskHealthGrid_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            string thresholdType = e.GetValue("ThresholdType").ToString();
            //if (thresholdType == "")
            //    thresholdType = "Percent";
            double valThreshold;
            if (e.GetValue("Threshold").ToString() != "")
            {
                valThreshold = (double)e.GetValue("Threshold");

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
                        valPercentFree = (double)e.GetValue("PercentFree");
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
                        valGBFree = (double)e.GetValue("DiskFree");
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
            //if (e.DataColumn.FieldName == "AverageQueueLength")
            //{
            //    double valThreshold;
            //    if (e.GetValue("Threshold").ToString() != "")
            //    {
            //        valThreshold = (double)e.GetValue("Threshold");
            //    }
            //    else
            //    {
            //        valThreshold = 0.0;
            //    }

            //    if ((valThreshold) >= 0.3 && valThreshold < 1.0)
            //    {
            //        e.Cell.BackColor = System.Drawing.Color.Yellow;
            //    }
            //    else if ((valThreshold) >= 1.0)
            //    {
            //        e.Cell.BackColor = System.Drawing.Color.Red;
            //    }
            //    else
            //    {
            //        e.Cell.BackColor = System.Drawing.Color.Green;
            //        e.Cell.ForeColor = System.Drawing.Color.White;
            //    }
            //}
            //if (e.DataColumn.FieldName == "PercentUtilization")
            //{
            //    double valThreshold;
            //    if (e.GetValue("Threshold").ToString() != "")
            //    {
            //        valThreshold = (double)e.GetValue("Threshold");
            //    }
            //    else
            //    {
            //        valThreshold = 0.0;
            //    }

            //    if ((valThreshold) >= 0.8 && valThreshold < 1.0)
            //    {
            //        e.Cell.BackColor = System.Drawing.Color.Yellow;
            //    }
            //    else if ((valThreshold) >= 1.0)
            //    {
            //        e.Cell.BackColor = System.Drawing.Color.Red;
            //    }
            //    else
            //    {
            //        e.Cell.BackColor = System.Drawing.Color.Green;
            //        e.Cell.ForeColor = System.Drawing.Color.White;
            //    }
            //}
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
        protected void DiskHealthGrid_PageSizeChanged(object sender, EventArgs e)
        {
            //ProfilesGridView.PageIndex;
            VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("DiskHealth|DiskHealthGrid", DiskHealthGrid.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        }
        public void FillOutageTab()
        {
            DataTable dt = new DataTable();
            //9/29/2015 NS modified for VSPLUS-2212
            string type = "";
            if (Request.QueryString["Type"].ToString() == "Traveler")
            {
                type = "Domino";
            }
            else
            {
                type = Request.QueryString["Type"].ToString();
            }
            dt = VSWebBL.DashboardBL.DominoServerDetailsBL.Ins.GetOutage(servernamelbl.Text, type);
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

        public DataTable SetGraph(string paramGraph, string DeviceName)
        {
            try
            {
                performanceWebChartControl.Series.Clear();
                DataTable dt = VSWebBL.DashboardBL.DominoServerDetailsBL.Ins.SetGraph(paramGraph, DeviceName);

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
                DataTable dt1 = VSWebBL.DashboardBL.DominoServerDetailsBL.Ins.ResponseThreshold(DeviceName);
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
                UI uiobj = new UI();
                uiobj.RecalibrateChartAxes(performanceWebChartControl.Diagram);
                return dt;
            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }

        }

        //7/16/2014 NS commented out for VSPLUS-813
        //DiskHealthGrid_SelectionChanged, ASPxCallbackPanel1_Callback, SetGraphForDiskSpace
        /*
		protected void DiskHealthGrid_SelectionChanged(object sender, EventArgs e)
		{
			if (DiskHealthGrid.Selection.Count > 0)
			{
				System.Collections.Generic.List<object> Type = DiskHealthGrid.GetSelectedFieldValues("DiskName");
				//3/22/2014 NS added
				//System.Collections.Generic.List<object> ID = DeviceGridView.GetSelectedFieldValues("ID");
				if (Type.Count > 0)
				{
					string Name = Type[0].ToString();
					SetGraphForDiskSpace(servernamelbl.Text, Name);
					//string SType = ServerType[0].ToString();
					//string LastUpdate = LastDate[0].ToString();
					// Session["Type"] = Type[0];
					//MD Notes 20Feb14
					//if (!(Session["UserFullName"] != null && Session["UserFullName"].ToString() == "Anonymous"))
					//{
					//    if (SType == "NotesMail Probe")
					//    {
					//        DevExpress.Web.ASPxWebControl.RedirectOnCallback("NotesMailProbeDetailsPage.aspx?Name=" + Name + "&Type=" + SType + "&LastDate=" + LastUpdate + "");
					//    }
					//    else if (SType == "Exchange") //MD exchange Dec13
					//    {
					//        DevExpress.Web.ASPxWebControl.RedirectOnCallback("ExchangeServerDetailsPage3.aspx?Name=" + Name + "&Type=" + SType + "&LastDate=" + LastUpdate + "");
					//    }
					//    //3/22/2014 NS added
					//    //else if (SType == "Exchange DAG")
					//    //{
					//    //    DevExpress.Web.ASPxWebControl.RedirectOnCallback("DAGDetail.aspx?Name=" + Name + "&Type=" + SType + "&LastDate=" + LastUpdate + "&ID=" + ID + "");
					//    //}
					//    //19/06/2014
					//    else if (SType == "BES")
					//    {
					//        DevExpress.Web.ASPxWebControl.RedirectOnCallback("BlackBerryServerDetailsPage2.aspx?Name=" + Name + "&Type=" + SType + "&LastDate=" + LastUpdate + "");
					//    }
					//    else
					//    {
					//        DevExpress.Web.ASPxWebControl.RedirectOnCallback("DominoServerDetailsPage2.aspx?Name=" + Name + "&Type=" + SType + "&LastDate=" + LastUpdate + "");
					//    }
					//}
				}


			}
		}
        

		protected void ASPxCallbackPanel1_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
		{
			
				//lblServerName.Text = "";
			int index = DiskHealthGrid.FocusedRowIndex;
				//8/9/2013 NS modified - when no rows were found, object reference not set error appeared
				if (index > -1)
				{
					string value = DiskHealthGrid.GetRowValues(index, "DiskName").ToString();
					SetGraphForDiskSpace(servernamelbl.Text, value);
				}
				
			
		}

		public void SetGraphForDiskSpace(string serverName, string DiskName )
		{
			//if (!IsPostBack)
			//{
				//int cnt = 0;
				diskspaceWebChartControl.Series.Clear();
				DataTable dt = VSWebBL.DashboardBL.DominoServerDetailsBL.Ins.SetGraphForDiskSpace(serverName,DiskName);
				diskspaceWebChartControl.DataSource = dt;

				double[] double1 = new double[dt.Rows.Count];
				double[] double2 = new double[dt.Rows.Count];

				Series series = null;

				//diskspaceWebChartControl.Series.Clear();

				for (int i = 0; i < dt.Rows.Count; i++)
				{
					if (series != null)
					{
						diskspaceWebChartControl.Series.Add(series);
						diskspaceWebChartControl.DataBind();
					}

					series = new Series(dt.Rows[i]["DiskName"].ToString(), ViewType.Pie);

					string val1 = dt.Rows[i]["DiskFree"].ToString();
					string val2 = dt.Rows[i]["DiskUsed"].ToString();

					if (val1 != "" && val2 != "")
					{
						double1[i] = Convert.ToDouble(dt.Rows[i]["DiskFree"].ToString());
						double2[i] = Convert.ToDouble(dt.Rows[i]["DiskUsed"].ToString());

						//if (dt.Rows[i]["DiskFree"] != "" && dt.Rows[i]["DiskFree"] != null)
						//    double1[i] = Convert.ToDouble(dt.Rows[i]["DiskFree"].ToString());
						//if (dt.Rows[i]["DiskUsed"] != "" && dt.Rows[i]["DiskUsed"] != null)
						//    double2[i] = Convert.ToDouble(dt.Rows[i]["DiskUsed"].ToString());

						series.Points.Add(new SeriesPoint("Disk Free", double1[i]));
						series.Points.Add(new SeriesPoint("Disk Used", double2[i]));
						series.ShowInLegend = true;

						series.LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;
						PieSeriesLabel seriesLabel = (PieSeriesLabel)series.Label;
						seriesLabel.Position = PieSeriesLabelPosition.Radial;
						seriesLabel.BackColor = System.Drawing.Color.Transparent;
						seriesLabel.TextColor = System.Drawing.Color.Black;

                        PieSeriesView seriesView = (PieSeriesView)series.View;
						seriesView.Titles.Add(new SeriesTitle());
						seriesView.Titles[0].Dock = ChartTitleDockStyle.Bottom;

						seriesView.Titles[0].Text = series.Name.Substring(5);
						seriesView.Titles[0].Visible = true;
						seriesView.Titles[0].WordWrap = true;
					}
				}

				if (series != null)
				{
					diskspaceWebChartControl.Series.Add(series);
					diskspaceWebChartControl.DataBind();
				}

				for (int c = 0; c < diskspaceWebChartControl.Series.Count; c++)
				{
					if (c == 0)
					{
						PiePointOptions seriesPointOptions = (PiePointOptions)series.LegendPointOptions;
						series.LegendPointOptions.PointView = PointView.Argument;
						diskspaceWebChartControl.Series[0].ShowInLegend = true;
						diskspaceWebChartControl.Series[0].LegendPointOptions.PointView = PointView.Argument;
						diskspaceWebChartControl.Series[0].ShowInLegend = true;
						diskspaceWebChartControl.Legend.Visible = true;
					}
					else
					{

						diskspaceWebChartControl.Series[c].ShowInLegend = false;
					}


				}


			//}
		}
        */
        public void SetGraphForDiskSpace2(string serverName)
        {
            DataTable dt = VSWebBL.DashboardBL.DominoServerDetailsBL.Ins.SetGraphForDiskSpace(serverName, "", "");
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

        public void SetGraphForCPU(string paramGraph, string serverName)
        {
            cpuWebChartControl.Series.Clear();
            DataTable dt = VSWebBL.DashboardBL.DominoServerDetailsBL.Ins.SetGraphForCPU(paramGraph, serverName);

            Series series = null;
            series = new Series("DominoServer", ViewType.Line);
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
            UI uiobj = new UI();
            uiobj.RecalibrateChartAxes(cpuWebChartControl.Diagram);
            //return dt;     
        }

        //protected void cpuASPxRadioButtonList_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    SetGraphForCPU(cpuASPxRadioButtonList.Value.ToString(), servernamelbl.Text);
        //}

        public void SetGraphForMemory(string paramGraph, string serverName)
        {
            memWebChartControl.Series.Clear();
            DataTable dt = VSWebBL.DashboardBL.DominoServerDetailsBL.Ins.SetGraphForMemory(paramGraph, serverName);

            Series series = null;
            series = new Series("DominoServer", ViewType.Line);
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
            UI uiobj = new UI();
            uiobj.RecalibrateChartAxes(memWebChartControl.Diagram);
        }

        //protected void memASPxRadioButtonList_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    SetGraphForMemory(memASPxRadioButtonList.Value.ToString(), servernamelbl.Text);
        //}

        public void SetGraphForUsers(string paramGraph, string serverName)
        {
            usersWebChartControl.Series.Clear();
            DataTable dt = VSWebBL.DashboardBL.DominoServerDetailsBL.Ins.SetGraphForUsers(paramGraph, serverName);

            Series series = null;
            series = new Series("DominoServer", ViewType.Line);
            series.Visible = true;
            series.ArgumentDataMember = dt.Columns["dtfrom"].ToString();

            ValueDataMemberCollection seriesValueDataMembers = (ValueDataMemberCollection)series.ValueDataMembers;
            seriesValueDataMembers.AddRange(dt.Columns["maxval"].ToString());
            usersWebChartControl.Series.Add(series);

            ((XYDiagram)usersWebChartControl.Diagram).PaneLayoutDirection = PaneLayoutDirection.Horizontal;

            XYDiagram seriesXY = (XYDiagram)usersWebChartControl.Diagram;
            seriesXY.AxisY.Title.Text = "User Count";
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
            
            UI uiobj = new UI();
            uiobj.RecalibrateChartAxes(usersWebChartControl.Diagram);

        }

        protected void usersASPxRadioButtonList_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetGraphForUsers(usersASPxRadioButtonList.Value.ToString(), servernamelbl.Text);
        }

        //// public void FillCombobox()
        // {
        //    DataTable trafictab = VSWebBL.DashboardBL.DashboardBL.Ins.GetCombobox();
        //   MailComboBox.DataSource = trafictab;
        //   MailComboBox.TextField = "NameandType";
        //   MailComboBox.DataBind();
        // }



		//public void SetGraphForMail(string servername)
		//{
		//    MailWebChartControl.Series.Clear();
		//    DataTable dt = VSWebBL.DashboardBL.DashboardBL.Ins.GetMailStatus(servername);
		//    Series series1 = new Series("DeviceType", ViewType.Bar);
		//    series1.ArgumentDataMember = dt.Columns["Mail"].ToString();
		//    ValueDataMemberCollection seriesValueDataMembers = (ValueDataMemberCollection)series1.ValueDataMembers;
		//    seriesValueDataMembers.AddRange(dt.Columns["Value"].ToString());
		//    //Addnig series to mailchartbox control
		//    MailWebChartControl.Series.Add(series1);
		//    ((XYDiagram)MailWebChartControl.Diagram).PaneLayoutDirection = PaneLayoutDirection.Horizontal;
		//    XYDiagram seriesXY = (XYDiagram)MailWebChartControl.Diagram;
		//    //X and Y aixs detals
		//    seriesXY.AxisY.Title.Text = "Count";
		//    seriesXY.AxisX.Title.Text = "Mail Status";
		//    seriesXY.AxisY.Title.Visible = true;
		//    seriesXY.AxisX.Title.Visible = true;
		//    seriesXY.AxisX.Label.ResolveOverlappingOptions.AllowHide = false;
		//    seriesXY.AxisX.Label.ResolveOverlappingOptions.AllowRotate = true;
		//    seriesXY.AxisX.Label.ResolveOverlappingOptions.AllowStagger = true;
		//    //12/11/2013 NS added
		//    seriesXY.AxisY.Label.ResolveOverlappingOptions.AllowRotate = false;

		//    //Enabling the series 
		//    MailWebChartControl.Legend.Visible = false;


		//    AxisBase axis = ((XYDiagram)MailWebChartControl.Diagram).AxisX;
		//    ((BarSeriesView)series1.View).ColorEach = false;
		//    MailWebChartControl.DataSource = dt;
		//    MailWebChartControl.DataBind();
		//}
		public void SetGraphForMail(string serverName)
		{
			bool setgraph=true;
			MailWebChartControl.Series.Clear();
			DataTable dt = VSWebBL.DashboardBL.DashboardBL.Ins.GetMailStatus(serverName);
			if (dt.Rows.Count > 0)
			{
				//for (int i = 0; i <= 2; i++)
				//{
				//    if (Convert.ToInt32(dt.Rows[i]["Value"].ToString()) == 0)
				//    {
				//        setgraph = false;
				//    }
				//}
				if (Convert.ToInt32(dt.Rows[0]["Value"].ToString()) == 0 && Convert.ToInt32(dt.Rows[1]["Value"].ToString()) == 0 && Convert.ToInt32(dt.Rows[2]["Value"].ToString()) == 0)
				{
					setgraph = false;
				}
				Series series = null;
				series = new Series("DeviceType", ViewType.Bar);

				series.ArgumentDataMember = dt.Columns["Mail"].ToString();

				ValueDataMemberCollection seriesValueDataMembers = (ValueDataMemberCollection)series.ValueDataMembers;
				seriesValueDataMembers.AddRange(dt.Columns["Value"].ToString());
				MailWebChartControl.Series.Add(series);

				MailWebChartControl.Legend.Visible = false;

				((SideBySideBarSeriesView)series.View).ColorEach = true;

				AxisBase axisx = ((XYDiagram)MailWebChartControl.Diagram).AxisX;
				axisx.GridLines.Visible = false;
				XYDiagram seriesXY = (XYDiagram)MailWebChartControl.Diagram;
				seriesXY.AxisX.Title.Text = "Mail Status";
				seriesXY.AxisX.Title.Visible = true;
				seriesXY.AxisX.Label.ResolveOverlappingOptions.AllowRotate = false;
				seriesXY.AxisX.Label.ResolveOverlappingOptions.AllowStagger = true;
				seriesXY.AxisY.Title.Text = "Count";
				seriesXY.AxisY.Title.Visible = true;

				if (setgraph == true)
				{
					seriesXY.AxisY.WholeRange.Auto = true;

				}
				else
				{
					seriesXY.AxisY.WholeRange.Auto = false;
				}
				seriesXY.AxisY.NumericOptions.Format = NumericFormat.Number;
				seriesXY.AxisY.NumericOptions.Precision = 0;
				seriesXY.AxisY.Label.ResolveOverlappingOptions.AllowRotate = false;
				seriesXY.AxisY.VisualRange.MinValue = 0;
                AxisBase axisy = ((XYDiagram)MailWebChartControl.Diagram).AxisY;
				axisy.Range.AlwaysShowZeroLevel = false;
				axisy.GridLines.Visible = false;
				MailWebChartControl.DataSource = dt;
				MailWebChartControl.DataBind();
                UI uiobj = new UI();
                uiobj.RecalibrateChartAxes(MailWebChartControl.Diagram);
			}
		}

        //protected void mailASPxRadioButtonList_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    SetGraphForMail(mailASPxRadioButtonList.Value.ToString(), servernamelbl.Text);
        //}


        //DataBase Code


        public void FillGrid()
        {
            DataTable Statustab = new DataTable();
            try
            {
                Statustab = VSWebBL.DashboardBL.DatabaseHealthBL.Ins.GetData(servernamelbl.Text);

                if (Statustab.Rows.Count > 0)
                {

                }
                Session["Statustab"] = Statustab;
                MonitoredDBGridView.DataSource = Statustab;
                MonitoredDBGridView.DataBind();


            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }

        }

        public void FillGridfromSession()
        {
            DataTable dt = new DataTable();
            if (Session["Statustab"] != "" && Session["Statustab"] != null)
                dt = Session["Statustab"] as DataTable;
            if (dt.Rows.Count > 0)
            {
                MonitoredDBGridView.DataSource = dt;
                MonitoredDBGridView.DataBind();
            }
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

        protected void MonitoredDBGridView_SelectionChanged(object sender, EventArgs e)
        {

            if (MonitoredDBGridView.Selection.Count > 0)
            {
                System.Collections.Generic.List<object> Type = MonitoredDBGridView.GetSelectedFieldValues("Name");
                System.Collections.Generic.List<object> c = MonitoredDBGridView.GetSelectedFieldValues("Category");
                if (Type.Count > 0 && c.Count > 0)
                {

                    string Name = Type[0].ToString();
                    string RT = c[0].ToString();
                    if (RT == "Database Response Time")
                        //Mukund: VSPLUS-844, Page redirect on callback
                        try
                        {
                            DevExpress.Web.ASPxWebControl.RedirectOnCallback("Performance.aspx?Name=" + Name + "");
                            Context.ApplicationInstance.CompleteRequest();//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
                        }
                        catch (Exception ex)
                        {
                            Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                            //throw ex;
                        }
                }


            }
        }

        //protected void AllDBGridView_SelectionChanged(object sender, EventArgs e)
        //{
        //    if (AllDBGridView.Selection.Count > 0)
        //    {
        //        System.Collections.Generic.List<object> Type = AllDBGridView.GetSelectedFieldValues("Server");

        //        if (Type.Count > 0)
        //        {
        //            string Name = Type[0].ToString();
        //            Session["Type"] = Type[0];
        //            DevExpress.Web.ASPxWebControl.RedirectOnCallback("Performance.aspx?Name=" + Server + "");
        //            //Response.Redirect("DeviceChart.aspx");
        //        }


        //    }
        //}

        public void FillALLGrid()
        {
            DataTable Dailytab = new DataTable();
            try
            {
                Dailytab = VSWebBL.DashboardBL.DatabaseHealthBL.Ins.GetAllData(servernamelbl.Text);

                if (Dailytab.Rows.Count > 0)
                {

                }
                Session["Dailytab"] = Dailytab;
                AllDBGridView.DataSource = Dailytab;
                AllDBGridView.DataBind();
            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }

        }

        public void FillALLGridfromSession()
        {
            DataTable dt = new DataTable();
            if (Session["Dailytab"] != "" && Session["Dailytab"] != null)
                dt = Session["Dailytab"] as DataTable;
            if (dt.Rows.Count > 0)
            {
                AllDBGridView.DataSource = dt;
                AllDBGridView.DataBind();
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

        //protected void DominoserverTasksgrid_SelectionChanged(object sender, EventArgs e)
        //{
        //    if (DominoserverTasksgrid.Selection.Count > 0)
        //    {
        //        System.Collections.Generic.List<object> Type = DominoserverTasksgrid.GetSelectedFieldValues("Name");

        //        if (Type.Count > 0)
        //        {
        //            string Name = Type[0].ToString();
        //            Session["Type"] = Type[0];
        //            DevExpress.Web.ASPxWebControl.RedirectOnCallback("DominoServerDetailsPage2.aspx?Name=" + Name + "");
        //            //Response.Redirect("DeviceChart.aspx");
        //        }


        //    }
        //}

        public void FillMonitoredGrid()
        {

            try
            {
                DataTable StatusTable = VSWebBL.DashboardBL.DashboardBL.Ins.GetMoniteredServerTasks(servernamelbl.Text);

                if (StatusTable.Rows.Count <= 0)
                {
                    MonitoredDBGridView.Visible = false;
                    // MonitorDBRoundPanel1.Visible = false;
                    Label5.Visible = false;
                }
                Session["MoniterdeServerTasks"] = StatusTable;
                DominoserverTasksgrid.DataSource = StatusTable;
                DominoserverTasksgrid.DataBind();


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

        public void FillgridfromSession()
        {
            if (Session["MoniterdeServerTasks"] != "" && Session["MoniterdeServerTasks"] != null)
            {
                DataTable StatusTable = Session["MoniterdeServerTasks"] as DataTable;
                try
                {
                    DominoserverTasksgrid.DataSource = StatusTable;
                    DominoserverTasksgrid.DataBind();

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
            else
            {
                MonitoredDBGridView.Visible = false;
                //  MonitorDBRoundPanel1.Visible = false;
                Label5.Visible = false;
            }
        }

        public void FillMonMoniteredGrid()
        {

            try
            {
                DataTable StatusTable1 = VSWebBL.DashboardBL.DashboardBL.Ins.GetNonMoniteredServerTasks(servernamelbl.Text);

                Session["NonMoniterdeServerTasks"] = StatusTable1;
                NonmoniterdGrid.DataSource = StatusTable1;
                NonmoniterdGrid.DataBind();


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

        public void FillNonMongridfromSession()
        {
            if (Session["NonMoniterdeServerTasks"] != "" && Session["NonMoniterdeServerTasks"] != null)
            {
                DataTable StatusTable1 = Session["NonMoniterdeServerTasks"] as DataTable;
                try
                {
                    NonmoniterdGrid.DataSource = StatusTable1;
                    NonmoniterdGrid.DataBind();

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
        }

        //protected void NonmoniterdGrid_SelectionChanged(object sender, EventArgs e)
        //{
        //    if (NonmoniterdGrid.Selection.Count > 0)
        //    {
        //        System.Collections.Generic.List<object> Type = NonmoniterdGrid.GetSelectedFieldValues("Name");

        //        if (Type.Count > 0)
        //        {
        //            string Name = Type[0].ToString();

        //            DevExpress.Web.ASPxWebControl.RedirectOnCallback("DominoServerDetailsPage2.aspx?Name=" + Name + "");

        //        }


        //    }
        //}

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

        //protected void MailComboBox_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    SetGraphForMail(MailComboBox.SelectedItem.Text);
        //}

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
            //if (Session["SLmaitservers"] != "" && Session["SLmaitservers"] != null)
            //{
            //    //DataTable dt = VSWebBL.ConfiguratorBL.MaintenanceBL.Ins.servermaintenance(System.DateTime.Now, 0, 0);
            //    maintenancegrid.DataSource = (DataTable)Session["SLmaitservers"];
            //    maintenancegrid.DataBind();
            //    //Session["maitservers"] = dt;
            //}
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
                //1/4/2013 NS - when Clear button is clicked, the grid needs to return to its original state
                //which means return all records regardless of the dates/times
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



        //protected void maintenancegrid_HtmlDataCellPrepared(object sender, DevExpress.Web.ASPxGridViewTableDataCellEventArgs e)
        //{
        //    if (e.DataColumn.FieldName == "MaintType")
        //    {

        //        if (e.CellValue.ToString() == "1")
        //        {
        //            e.Cell.Text = "One time";
        //        }
        //        else if (e.CellValue.ToString() == "2")
        //        {
        //            e.Cell.Text = "Daily";
        //        }
        //        else if (e.CellValue.ToString() == "3")
        //        {
        //            e.Cell.Text = "Weekly";
        //        }
        //        else
        //        {
        //            e.Cell.Text = "Monthly";
        //        }

        //    }


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

        protected void OpenButton1_Click(object sender, EventArgs e)
        {
            if (AllDBGridView.FocusedRowIndex > -1)
            {
                DataTable dt = new DataTable();
                int index;
                if (AllDBGridView.FocusedRowIndex > -1)
                {
                    index = AllDBGridView.FocusedRowIndex;
                }
                else
                {
                    index = 0;
                }
                string HostName = "Nohost";
                string Server = AllDBGridView.GetRowValues(index, "Server").ToString();
                DataTable servertab = VSWebBL.DashboardBL.DatabaseHealthBL.Ins.GetIPfromServers(Server);
                if (servertab.Rows.Count > 0)
                    HostName = servertab.Rows[0]["IPAddress"].ToString();
                string filepath = AllDBGridView.GetRowValues(index, "Folder").ToString();
                string fileName = AllDBGridView.GetRowValues(index, "FileName").ToString();
                filepath = filepath.Replace("\\", "/");
                //7/23/2015 NS modified
                string message = ""; if (filepath == "")
                {
                    message = "PopupCenter('http://" + HostName + "/" + fileName + "?OpenDatabase');";
                    //Page.ClientScript.RegisterStartupScript(this.GetType(), "Script", "PopupCenter('http://" + HostName + "/" + fileName + "?OpenDatabase');", true);
                }
                else
                {
                    message = "PopupCenter('http://" + HostName + "/" + filepath + fileName + "?OpenDatabase');";
                    //Page.ClientScript.RegisterStartupScript(this.GetType(), "Script", "PopupCenter('http://" + HostName + "/" + filepath + "/" + fileName + "?OpenDatabase');", true);
                }

                ScriptManager.RegisterStartupScript(Page, this.GetType(), "clientscript", message, true);


                //if (filepath == "")
                //{
                //    Response.Write("<script>window.open('http://" + HostName + "/" + fileName + "?OpenDatabase','Open Database','width=500px height=100px',1)</script>");
                //}
                //else
                //{
                //    Response.Write("<script>window.open('http://" + HostName + "/" + filepath + "/" + fileName + "?OpenDatabase','Open Database','width=500px height=100px',1)</script>");
                //}
            }
            else
            {
                Dominomsglbl.Text = "Please select a database from the All Databases grid.";
                DominomsgPopupControl.ShowOnPageLoad = true;
            }
        }

        protected void CompactButton_Click(object sender, EventArgs e)
        {
            popuptextBox.Text = "";
            DBPopupControl.HeaderText = "Compact Options";
            ASPxLabel5.Text = "Please enter any desired COMPACT options.";
            ASPxLabel6.Text = "Use -c for corrupt databases and -B for in place compaction with file size reduction.";
            ASPxLabel8.Text = "Google 'IBM Domino Compact switches' for a full list of options.";
            ASPxLabel7.Visible = false;
            DBPopupControl.ShowOnPageLoad = true;
            Session["ActBtn"] = "CompactButton";
        }

        protected void FixupButton_Click(object sender, EventArgs e)
        {
            popuptextBox.Text = "";
            DBPopupControl.HeaderText = "Fixup Options";
            ASPxLabel5.Text = "Please enter any desired FIXUP options.";
            ASPxLabel6.Text = "For example, use -Q to check more quickly but less thoroughly.";
            ASPxLabel8.Text = "Google 'IBM Domino Fixup switches' for a full list of options.";
            ASPxLabel7.Visible = true;
            ASPxLabel7.Text = "Use -V to prevents Fixup from running on views. This option reduces the time it takes Fixup to run.";
            DBPopupControl.ShowOnPageLoad = true;
            Session["ActBtn"] = "Fixup";
        }

        protected void UpdallButton_Click(object sender, EventArgs e)
        {
            popuptextBox.Text = "";
            DBPopupControl.HeaderText = "Updall Options";
            ASPxLabel5.Text = "Please enter any desired UPDALL options.";
            ASPxLabel6.Text = "For example, use -R to rebuild all used views (resource intensive).";
            ASPxLabel8.Text = "Use -X to rebuild the full text index.";
            ASPxLabel7.Visible = false;
            DBPopupControl.ShowOnPageLoad = true;
            Session["ActBtn"] = "Updall";
        }

        protected void DominoOKButton_Click(object sender, EventArgs e)
        {
            if (Session["ActBtn"] != "" && Session["ActBtn"] != null)
            {
                if (AllDBGridView.FocusedRowIndex > -1)
                {
                    if (Session["ActBtn"] == "CompactButton")
                    {

                        //            NotesSession  session =new NotesSession();
                        //// NotesUIWorkspace workspace = New NotesUIWorkspace();
                        //    // NotesUIDocument  uidoc= new NotesUIDocument();
                        ////Set uidoc = workspace.CurrentDocument
                        //        NotesDocument doc =new NotesDocument();
                        //    //doc = uidoc.Document
                        //// Variant StatValue;
                        //    string dbPath;
                        //if(doc.FolderReferences.Folder(0) != "" )
                        //{

                        //    dbPath = doc.FolderReferences.Folder(0) &doc.FolderReferences.Filename(0);
                        //}

                        //else
                        //                {
                        //    dbPath = doc.FolderReferences.Filename(0);
                        //}
                        ////serverName$ =doc.Server(0);
                        //   
                        //// Config$ ="lo compact "+ dbPath + " " + Options;
                        ////consoleReturn$ = session.SendConsoleCommand(serverName$, Config$)
                        ////Print  "Sent the command " &Config$  & ".  Note that this will only work if YOU have the appropriate remote console and admin rights."
                        string Server = "";
                        string Config = "";
                        string Folder = "";
                        string FileName = "";
                        DataRow myRow = AllDBGridView.GetDataRow(AllDBGridView.FocusedRowIndex);
                        Server = myRow["Server"].ToString();
                        Folder = myRow["Folder"].ToString();
                        FileName = myRow["Filename"].ToString();
                        string Options;
                        Options = popuptextBox.Text;
                        Config = "lo compact " + Folder + FileName + " "  +" "+  Options;
                        string NotesIDfile = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("Notes User ID");
                        var pos = NotesIDfile.LastIndexOf('/');
                       
                        var value = NotesIDfile.Substring(0, pos);
                        var activeid =  NotesIDfile.Substring(pos + 1);
                        bool returnval = VSWebBL.DashboardBL.LotusTravelerHealthBLL.Ins.SENDTravelerConsoleCommand(Server, Config, Session["UserFullName"].ToString());
                        if (returnval == true)
                        {
                            Dominomsglbl.Text = "Sent the command " + Config + ". Note that this will only work if Notes ID  " + activeid + " have the appropriate remote console and admin rights.";
                            DominomsgPopupControl.ShowOnPageLoad = true;
                        }

                    }

                    if (Session["ActBtn"] == "Fixup")
                    {
                        string Server = "";
                        string Config = "";
                        string Folder = "";
                        string FileName = "";
                        DataRow myRow = AllDBGridView.GetDataRow(AllDBGridView.FocusedRowIndex);
                        Server = myRow["Server"].ToString();
                        Folder = myRow["Folder"].ToString();
                        FileName = myRow["Filename"].ToString();
                        string Options;
                        Options = popuptextBox.Text;
                        Config = "Load fixupdbpath options " + Folder + FileName + "" + "" + Options;
                        string NotesIDfile = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("Notes User ID");
                        var pos = NotesIDfile.LastIndexOf('/');
                        var value = NotesIDfile.Substring(0, pos);
                        var activeid = NotesIDfile.Substring(pos + 1);
                        bool returnval = VSWebBL.DashboardBL.LotusTravelerHealthBLL.Ins.SENDTravelerConsoleCommand(Server, Config, Session["UserFullName"].ToString());
                        if (returnval == true)
                        {
                            Dominomsglbl.Text = "Sent the command " + Config + ". Note that this will only work if  Notes ID " + activeid + " have the appropriate remote console and admin rights.";
                            DominomsgPopupControl.ShowOnPageLoad = true;
                        }
                        // Config ="Load fixupdbpath options "+ dbPath + " " + Options;


                    }
                    if (Session["ActBtn"] == "Updall")
                    {
                        string Server = "";
                        string Config = "";
                        string Folder = "";
                        string FileName = "";
                        DataRow myRow = AllDBGridView.GetDataRow(AllDBGridView.FocusedRowIndex);
                        Server = myRow["Server"].ToString();
                        Folder = myRow["Folder"].ToString();
                        FileName = myRow["Filename"].ToString();
                        string Options;
                        Options = popuptextBox.Text;
                        Config = "Load updalldbpath options " + Folder + FileName + "" + "" + Options;
                        string NotesIDfile = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("Notes User ID");
                        var pos = NotesIDfile.LastIndexOf('/');
                        var value = NotesIDfile.Substring(0, pos);
                        var activeid = NotesIDfile.Substring(pos + 1);
                        bool returnval = VSWebBL.DashboardBL.LotusTravelerHealthBLL.Ins.SENDTravelerConsoleCommand(Server, Config, Session["UserFullName"].ToString());
                        if (returnval == true)
                        {
                            Dominomsglbl.Text = "Sent the command " + Config + ". Note that this will only work if  Notes ID  " + activeid + " have the appropriate remote console and admin rights.";
                            DominomsgPopupControl.ShowOnPageLoad = true;
                        }


                        // Config$ ="Load updalldbpath options"+ dbPath + " " + Options;

                    }
                    DBPopupControl.ShowOnPageLoad = false;

                }

                else
                {
                    Dominomsglbl.Text = "Please Select Server";
                    DominomsgPopupControl.ShowOnPageLoad = true;
                    DBPopupControl.ShowOnPageLoad = false;
                }
            }
        }

        protected void CancelButton_Click(object sender, EventArgs e)
        {
            DBPopupControl.ShowOnPageLoad = false;
        }

        protected void msgbtn_Click(object sender, EventArgs e)
        {
            DominomsgPopupControl.ShowOnPageLoad = false;
        }

        protected void OpenDBButton_Click(object sender, EventArgs e)
        {

        }

        protected void ASPxPageControl1_TabClick(object source, DevExpress.Web.TabControlCancelEventArgs e)
        {
            if (e.Tab.Text == "Web Admin")
            {
                //12/4/2013 NS modified
                SetupWebAdmin();
                /*
                string serverip = "";
                string url = "";
                string servername = Request.QueryString["Name"];
                string dbpath = "webadmin.nsf";
                DataTable dt = new DataTable();
                dt = VSWebBL.ConfiguratorBL.AlertsBL.Ins.GetServerIP(servername);
                if (dt.Rows.Count > 0)
                {
                    serverip = dt.Rows[0]["IPAddress"].ToString();
                }
                if (serverip != "")
                {
                    url = "http://" + serverip + "/" + dbpath + "?OpenDatabase";
                    fraHtml.Attributes.Add("src", url);
                }
                 */
            }
            //2/6/2014 NS added all else statements for each tab
            else
            {
                if (e.Tab.Text == "Overall")
                {
                    //12/4/2013 NS copied the code from the !IsPostback event so that when a tab is clicked
                    //and the code executes, the graphs would not lose data
                    SetGraph("hh", servernamelbl.Text);
                    // SetGraphForDiskSpace(servernamelbl.Text);
                    SetGraphForCPU("hh", servernamelbl.Text);
                    SetGraphForMemory("hh", servernamelbl.Text);
                    SetGraphForUsers("hh", servernamelbl.Text);
                    SetGraphForMail(servernamelbl.Text);
                }
                else
                {
                    if (e.Tab.Text == "Databases")
                    {
                        FillgridfromSession();
                        FillALLGridfromSession();
                    }
                    else
                    {
                        if (e.Tab.Text == "Server Tasks")
                        {
                            FillgridfromSession();
                            FillNonMongridfromSession();
                        }
                        else
                        {
                            if (e.Tab.Text == "Maintenance")
                            {
                                Fillmaintenancegridfromsession();
                            }
                            else
                            {
                                if (e.Tab.Text == "Alerts History")
                                {
                                    FillAlertHistoryfromSession();
                                }
                                else
                                {
                                    if (e.Tab.Text == "Outages")
                                    {
                                        FilloutagefromSession();
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public void SetupWebAdmin()
        {
            string serverip = "";
            string servername = Request.QueryString["Name"];
            string dbpath = "webadmin.nsf";
            DataTable dt = new DataTable();
            dt = VSWebBL.ConfiguratorBL.AlertsBL.Ins.GetServerIP(servername);
            if (dt.Rows.Count > 0)
            {
                serverip = dt.Rows[0]["IPAddress"].ToString();
            }
            //3/21/2014 NS modified - hide WebAdmin tab for non-Domino pages - the code below doesn't work because it sits inside the UpdatePanel
            //if (Request.QueryString["Type"] != "Domino")
            //{
            //    ASPxPageControl1.TabPages[6].Visible = false;
            //}
            if (serverip != "")
            {
                url = "http://" + serverip + "/" + dbpath + "?OpenDatabase";
                //2/12/2014 NS modified
                //fraHtml.Attributes.Add("src", url);
                WebAdminButton.Text = "Click here to open the Web Admin page for " + servernamelbl.Text;
                webAdminLabel.Text = url;
            }
            else
            {
                WebAdminButton.Text = "No IP address found - you will not be able to open WebAdmin";
                webAdminLabel.Text = "";
            }
        }

        public void UpdateButtonVisibility()
        {
            bool isadmin = false;
            DataTable sa = VSWebBL.SecurityBL.UsersBL.Ins.GetIsConsoleComm(Session["UserID"].ToString());
            if (sa.Rows.Count > 0)
            {
                if (sa.Rows[0]["IsConsoleComm"].ToString() == "True")
                {
                    isadmin = true;
                }
            }
            if (isadmin)
            {
                CompactButton.Visible = true;
                FixupButton.Visible = true;
                UpdallButton.Visible = true;
            }
            //1/2/2014 NS added
            bool isconfig = false;
            DataTable dsconfig = VSWebBL.SecurityBL.UsersBL.Ins.GetIsConfig(Session["UserID"].ToString());
            if (dsconfig.Rows.Count > 0)
            {
                if (dsconfig.Rows[0]["IsConfigurator"].ToString() == "True")
                {
                    isconfig = true;
                }
            }
            //11/3/2014 NS modified for VSPLUS-1142
            if (isconfig)
            {
                //EditInConfigButton.Visible = true;
                ASPxMenu1.Items[0].Items[2].Visible = true;
                ASPxMenu1.Items[0].Items[3].Visible = true;
            }
            else
            {
                ASPxMenu1.Items[0].Items[2].Visible = false;
                ASPxMenu1.Items[0].Items[3].Visible = false;
            }
        }

        protected void ServerTasksPopupMenu_ItemClick(object source, DevExpress.Web.MenuItemEventArgs e)
        {
            if (e.Item.Name == "ServerTaskStart")
            {
                if (DominoserverTasksgrid.FocusedRowIndex > -1)
                {
                    YesButton.Visible = true;
                    NOButton.Visible = true;
                    CancelButton.Visible = false;
                    // CancelButton.Text = "Cancel";
                    Session["MenuItem"] = "ServerTaskStart";
                    ServerTaskStart();
                }
                else
                {
                    msgPopupControl1.HeaderText = "Server Tasks Start";
                    msgPopupControl1.ShowOnPageLoad = true;
                    msgLabel.Text = "Please select a task in the grid.";
                    YesButton.Visible = false;
                    NOButton.Visible = false;
                    CancelButton.Visible = true;
                    CancelButton.Text = "OK";
                }
            }
            if (e.Item.Name == "ServerTaskStop")
            {
                if (DominoserverTasksgrid.FocusedRowIndex > -1)
                {
                    YesButton.Visible = true;
                    NOButton.Visible = true;
                    CancelButton.Visible = false;
                    // CancelButton.Text = "Cancel";
                    Session["MenuItem"] = "ServerTaskStop";
                    ServerTaskStop();
                }
                else
                {
                    msgPopupControl1.HeaderText = "Server Task Stop";
                    msgPopupControl1.ShowOnPageLoad = true;
                    msgLabel.Text = "Please select a task in the grid.";
                    YesButton.Visible = false;
                    NOButton.Visible = false;
                    CancelButton.Visible = true;
                    CancelButton.Text = "OK";
                }
            }
            if (e.Item.Name == "ServerTaskRestart")
            {
                if (DominoserverTasksgrid.FocusedRowIndex > -1)
                {
                    YesButton.Visible = true;
                    NOButton.Visible = true;
                    CancelButton.Visible = false;
                    // CancelButton.Text = "Cancel";
                    Session["MenuItem"] = "ServerTaskRestart";
                    ServerTaskRestart();
                }
                else
                {
                    msgPopupControl1.HeaderText = "Server Task Restart";
                    msgPopupControl1.ShowOnPageLoad = true;
                    msgLabel.Text = "Please select a task in the grid.";
                    YesButton.Visible = false;
                    NOButton.Visible = false;
                    CancelButton.Visible = true;
                    CancelButton.Text = "OK";
                }
            }
        }

        private void ServerTaskRestart()
        {
            string TellCommand = "";
            string myUserName = "";
            string myDeviceName = "";
            string myServerName = "";

            DataRow myRow = DominoserverTasksgrid.GetDataRow(DominoserverTasksgrid.FocusedRowIndex);
            try
            {

                Session["TaskName"] = myRow["TaskName"].ToString();
                Session["MenuItem"] = "ServerTaskRestart";

                myUserName = Session["UserLogin"].ToString();
                Session["myUserName"] = myUserName;
                myServerName = servernamelbl.Text;
                Session["myServerName"] = myServerName;

                VSWebDO.DominoServerTasks DSTasksObject = new VSWebDO.DominoServerTasks();
                DSTasksObject.TaskName = myRow["TaskName"].ToString();
                VSWebDO.DominoServerTasks ReturnValue = VSWebBL.ConfiguratorBL.DominoServerTasksBL.Ins.GetDataForTaskName(DSTasksObject);
                TellCommand = ReturnValue.LoadString;

                TellCommand = "tell " + TellCommand.ToString().Substring(3) + " restart";
                Session["TellCommand"] = TellCommand;




                VSWebBL.DashboardBL.LotusTravelerHealthBLL.Ins.SENDTravelerConsoleCommand(myServerName, TellCommand, myUserName);

                msgPopupControl1.HeaderText = "Server Tasks Restart";
                msgPopupControl1.ShowOnPageLoad = true;
                msgLabel.Text = "The request to restart the task '" + myRow["TaskName"].ToString() + "' is sent.";
                YesButton.Visible = false;
                NOButton.Visible = false;
                CancelButton.Visible = true;
                CancelButton.Text = "OK";
            }
            catch (Exception ex)
            {
                myUserName = "";
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
            //msgPopupControl1.ShowOnPageLoad = true;
            //msgLabel.Text = "Are you sure you want to Restart " + myRow["TaskName"].ToString() + " on server " + myServerName + "?";                      

        }

        private void ServerTaskStop()
        {
            string TellCommand = "";
            string myUserName = "";
            string myDeviceName = "";
            string myServerName = "";

            DataRow myRow = DominoserverTasksgrid.GetDataRow(DominoserverTasksgrid.FocusedRowIndex);
            try
            {
                Session["TaskName"] = myRow["TaskName"].ToString();
                Session["MenuItem"] = "ServerTaskStop";

                myUserName = Session["UserLogin"].ToString();
                Session["myUserName"] = myUserName;
                myServerName = servernamelbl.Text;
                Session["myServerName"] = myServerName;

                VSWebDO.DominoServerTasks DSTasksObject = new VSWebDO.DominoServerTasks();
                DSTasksObject.TaskName = myRow["TaskName"].ToString();
                VSWebDO.DominoServerTasks ReturnValue = VSWebBL.ConfiguratorBL.DominoServerTasksBL.Ins.GetDataForTaskName(DSTasksObject);
                TellCommand = ReturnValue.LoadString;

                TellCommand = "tell " + TellCommand.Substring(3) + " exit";
                Session["TellCommand"] = TellCommand;



                VSWebBL.DashboardBL.LotusTravelerHealthBLL.Ins.SENDTravelerConsoleCommand(myServerName, TellCommand, myUserName);

                msgPopupControl1.HeaderText = "Server Tasks Stop";
                msgPopupControl1.ShowOnPageLoad = true;
                msgLabel.Text = "The request to stop the task '" + myRow["TaskName"].ToString() + "' is sent.";
                YesButton.Visible = false;
                NOButton.Visible = false;
                CancelButton.Visible = true;
                CancelButton.Text = "OK";
            }
            catch (Exception ex)
            {
                myUserName = "";
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
            //msgPopupControl1.ShowOnPageLoad = true;
            //msgLabel.Text = "Are you sure you want to Stop " + myRow["TaskName"].ToString() + " on server " + myServerName + "?";                      

        }

        private void ServerTaskStart()
        {
            string TellCommand = "";
            string myUserName = "";
            string myDeviceName = "";
            string myServerName = "";

            DataRow myRow = DominoserverTasksgrid.GetDataRow(DominoserverTasksgrid.FocusedRowIndex);
            try
            {

                myUserName = Session["UserLogin"].ToString();
                myServerName = servernamelbl.Text;

                VSWebDO.DominoServerTasks DSTasksObject = new VSWebDO.DominoServerTasks();
                DSTasksObject.TaskName = myRow["TaskName"].ToString();
                VSWebDO.DominoServerTasks ReturnValue = VSWebBL.ConfiguratorBL.DominoServerTasksBL.Ins.GetDataForTaskName(DSTasksObject);
                TellCommand = ReturnValue.LoadString;

                VSWebBL.DashboardBL.LotusTravelerHealthBLL.Ins.SENDTravelerConsoleCommand(myServerName, TellCommand, myUserName);

                msgPopupControl1.HeaderText = "Server Tasks Start";
                msgPopupControl1.ShowOnPageLoad = true;
                msgLabel.Text = "The request to start the task '" + myRow["TaskName"].ToString() + "' is sent.";
                YesButton.Visible = false;
                NOButton.Visible = false;
                CancelButton.Visible = true;
                CancelButton.Text = "OK";
            }
            catch (Exception ex)
            {
                myUserName = "";
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }

        }


        protected void DominoserverTasksgrid_HtmlRowCreated(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
        {
            int index = DominoserverTasksgrid.FocusedRowIndex;
            if (e.VisibleIndex != index)
            {

                e.Row.Attributes.Add("onmouseover", "this.originalcolor=this.style.backgroundColor;" + "this.style.backgroundColor='#C0C0C0';"); //'#C0C0C0'


                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalcolor;");
            }
        }

        protected void YesButton_Click(object sender, EventArgs e)
        {

            if (Session["MenuItem"] == "ServerTaskRestart" || Session["MenuItem"] == "ServerTaskExit")
            {
                if (Session["TellCommand"] != "" && Session["myServerName"] != "" && Session["myUserName"] != "")
                {
                    msgLabel.Text = Session["TellCommand"].ToString() + "," + Session["myServerName"].ToString();
                    VSWebBL.DashboardBL.LotusTravelerHealthBLL.Ins.SENDTravelerConsoleCommand(Session["myServerName"].ToString(), Session["TellCommand"].ToString(), Session["myUserName"].ToString());

                } YesButton.Visible = false;
                NOButton.Visible = false;
                CancelButton.Visible = true;
                CancelButton.Text = "OK";
                Session["myUserName"] = "";
                Session["myServerName"] = "";
                Session["myDeviceName"] = "";
                Session["TellCommand"] = "";


            }
        }
        protected void CancelButton1_Click(object sender, EventArgs e)
        {
            msgPopupControl1.ShowOnPageLoad = false;
        }

        protected void NOButton_Click(object sender, EventArgs e)
        {
            if (Session["myUserName"] != "" && Session["myUserName"] != null)
                msgLabel.Text = "No changes made to the task " + Session["TaskName"].ToString() + ".";
            YesButton.Visible = false;
            NOButton.Visible = false;
            CancelButton.Visible = true;
            CancelButton.Text = "OK";
            Session["myUserName"] = "";
            Session["myServerName"] = "";
            Session["myDeviceName"] = "";
            Session["TellCommand"] = "";
            Session["TaskName"] = "";
        }

        protected void ScanButton_Click(object sender, EventArgs e)
        {
            try
            {
                Status StatusObj = new Status();
                StatusObj.Name = Request.QueryString["Name"];
                //9/29/2015 NS modified for VSPLUS-2212
                string type = "";
                if (Request.QueryString["Type"].ToString() == "Traveler")
                {
                    type = "Domino";
                }
                else
                {
                    type = Request.QueryString["Type"].ToString();
                }
                StatusObj.Type = type;

                bool bl = VSWebBL.StatusBL.StatusTBL.Ins.UpdateforScan(StatusObj);
                //9/29/2015 NS modified for VSPLUS-2212
                if (Request.QueryString["Type"] == "Domino" || Request.QueryString["Type"] == "Traveler")
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
            Session["Submenu"] = "LotusDomino";
            if (Request.QueryString["Name"] != "" && Request.QueryString["Name"] != null)
            {
                id = (VSWebBL.SecurityBL.ServersBL.Ins.GetServerIDbyServerName(Request.QueryString["Name"].ToString())).ToString();
                Response.Redirect("~/Configurator/DominoProperties.aspx?Key=" + id, false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
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
                    //10/3/2014 NS modified for VSPLUS-990
                    SuccessMsg.InnerHtml = "Monitoring for " + servernamelbl.Text + " has been temporarily suspended for a duration of " + TbDuration.Text + " minutes." +
                    "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                    SuccessMsg.Style.Value = "display: block";

                }
                else
                {
                    //10/3/2014 NS modified for VSPLUS-990
                    ErrorMsg.InnerHtml = "The Settings were NOT updated." +
                        "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
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



        protected void MonitoredDBGridView_PageSizeChanged(object sender, EventArgs e)
        {
            //ProfilesGridView.PageIndex;
            VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("DominoServerDetailsPage2|MonitoredDBGridView", MonitoredDBGridView.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        }
        protected void AllDBGridView_PageSizeChanged(object sender, EventArgs e)
        {
            //ProfilesGridView.PageIndex;
            VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("DominoServerDetailsPage2|AllDBGridView", AllDBGridView.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        }
        protected void DominoserverTasksgrid_PageSizeChanged(object sender, EventArgs e)
        {
            //ProfilesGridView.PageIndex;
            VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("DominoServerDetailsPage2|DominoserverTasksgrid", DominoserverTasksgrid.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        }

        protected void NonmoniterdGrid_PageSizeChanged(object sender, EventArgs e)
        {
            //ProfilesGridView.PageIndex;
            VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("DominoServerDetailsPage2|NonmoniterdGrid", NonmoniterdGrid.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        }
        protected void Maintenancegrid_PageSizeChanged(object sender, EventArgs e)
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



        public void FillHealthAssessmentFromSession()
        {
            try
            {
                if (Session["GridData"] != null)
                {
                    DataTable dt = Session["HealthAssessment"] as DataTable;
                    HealthAssessmentGrid1.DataSource = dt;
                    HealthAssessmentGrid1.DataBind();

                }
            }
            catch (Exception ex)
            {
                //Response.Write("Error : " + ex);
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
        }
        private void FillHealthAssessmentStatusGrid(string type, string servername)
        {
            try
            {
                //DataTable StatusTable1 = VSWebBL.DashboardBL.ExchangeServerDetailsBL.Ins.GetCASStatus(servernamelbl.Text);
                //3/25/2014 NS modified - we only need CAS to pass as a category
                //DataTable StatusTable1 = VSWebBL.DashboardBL.ExchangeServerDetailsBL.Ins.GetStatusDetails(servernamelbl.Text,"CAS Status");
                //9/29/2015 NS modified for VSPLUS-2212
                string typeFinal = "";
                string TypeAndName = "";
                if (Request.QueryString["Type"] != "" && Request.QueryString["Type"] != null)
                {
                    if (Request.QueryString["Type"].ToString() == "Traveler")
                    {
                        typeFinal = "Domino";
                    }
                    else
                    {
                        typeFinal = type;
                    }
                    TypeAndName = servername + "-" + typeFinal;
                }
                DataTable StatusTable12 = VSWebBL.DashboardBL.ExchangeServerDetailsBL.Ins.GetHealthAssessmentStatusDetails(TypeAndName);

                Session["HealthAssessment"] = StatusTable12;
                HealthAssessmentGrid1.DataSource = StatusTable12;
                HealthAssessmentGrid1.DataBind();
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
        protected void HealthAssessmentGrid1_HtmlDataCellPrepared(object sender, DevExpress.Web.ASPxGridViewTableDataCellEventArgs e)
        {
            //3/25/2014 NS modified
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
                    //9/29/2015 NS modified for VSPLUS-2212
                    string type = "";
                    if (Request.QueryString["Type"] == "Traveler")
                    {
                        type = "Domino";
                    }
                    else
                    {
                        type = Request.QueryString["Type"];
                    }
                    StatusObj.Type = type;

                    bool bl = VSWebBL.StatusBL.StatusTBL.Ins.UpdateforScan(StatusObj);
                    //9/29/2015 NS modified for VSPLUS-2212
                    if (Request.QueryString["Type"] == "Domino" || Request.QueryString["Type"] == "Traveler")
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
                    id = (VSWebBL.SecurityBL.ServersBL.Ins.GetServerIDbyServerName(Request.QueryString["Name"].ToString())).ToString();
                    Response.Redirect("~/Configurator/DominoProperties.aspx?Key=" + id, false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
                    Context.ApplicationInstance.CompleteRequest();
                }
            }
            else if (e.Item.Name == "SuspendItem")
            {
                ASPxPopupControl1.HeaderText = "Suspend Temporarily";
                ASPxPopupControl1.ShowOnPageLoad = true;
            }
        }
        //8/25/2015 NS added
        protected void TravelGrid_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            const int zerolimit = 0;
            const int lowerlimit = 5;
            const int upperlimit = 10;
            int interval000 = 0;
            int interval001 = 0;
            int interval002 = 0;
            int interval005 = 0;
            int interval010 = 0;
            int interval030 = 0;
            int interval060 = 0;
            int interval120 = 0;
            bool setred = false;
            bool setyellow = false;
            bool setredsrv = false;
            //Get the value of each interval in the selected row
            if (Convert.ToString(e.GetValue("000-001")) != "")
                interval000 = Convert.ToInt32(e.GetValue("000-001"));
            if (Convert.ToString(e.GetValue("001-002")) != "")
                interval001 = Convert.ToInt32(e.GetValue("001-002"));
            if (Convert.ToString(e.GetValue("002-005")) != "")
                interval002 = Convert.ToInt32(e.GetValue("002-005"));
            if (Convert.ToString(e.GetValue("005-010")) != "")
                interval005 = Convert.ToInt32(e.GetValue("005-010"));
            if (Convert.ToString(e.GetValue("010-030")) != "")
                interval010 = Convert.ToInt32(e.GetValue("010-030"));
            if (Convert.ToString(e.GetValue("030-060")) != "")
                interval030 = Convert.ToInt32(e.GetValue("030-060"));
            if (Convert.ToString(e.GetValue("060-120")) != "")
                interval060 = Convert.ToInt32(e.GetValue("060-120"));
            if (Convert.ToString(e.GetValue("120-INF")) != "")
                interval120 = Convert.ToInt32(e.GetValue("120-INF"));
            e.Cell.ForeColor = System.Drawing.Color.Black;
            //Set the back color for all cells to green by default
            switch (e.DataColumn.FieldName)
            {
                case "MailServerName":
                    //Compute color coding for each interval based on the interval values
                    if ((interval000 == zerolimit & (interval002 > zerolimit |
                        interval005 > zerolimit | interval010 > zerolimit | interval030 > zerolimit |
                        interval060 > zerolimit | interval120 > zerolimit)) | interval002 > upperlimit |
                        interval005 > upperlimit | interval010 > upperlimit | interval030 > upperlimit |
                        interval060 > lowerlimit | interval120 > lowerlimit)
                        setred = true;
                    else if (interval000 == zerolimit & interval001 == zerolimit & (interval002 > zerolimit |
                        interval005 > zerolimit | interval010 > zerolimit | interval030 > zerolimit |
                        interval060 > zerolimit | interval120 > zerolimit))
                        setred = true;
                    if (setred)
                    {
                        e.Cell.ForeColor = System.Drawing.Color.Red;
                        e.Cell.Font.Bold = true;
                    }
                    e.Cell.Wrap = Convert.ToBoolean(DefaultBoolean.True);
                    break;
                case "000-001":
                    e.Cell.BackColor = System.Drawing.Color.LightGreen;
                    //Compute color coding for each interval based on the interval values
                    if (interval000 == zerolimit & (interval002 > zerolimit |
                        interval005 > zerolimit | interval010 > zerolimit |
                        interval030 > zerolimit | interval060 > zerolimit | interval120 > zerolimit))
                        setred = true;
                    else if (interval000 == zerolimit & interval001 == zerolimit & interval002 == zerolimit &
                        interval005 == zerolimit & interval010 == zerolimit &
                        interval030 == zerolimit & interval060 == zerolimit & interval120 == zerolimit)
                        setyellow = true;
                    else if (interval000 == zerolimit & interval001 > zerolimit & interval002 == zerolimit &
                        interval005 == zerolimit & interval010 == zerolimit & interval030 == zerolimit &
                        interval060 == zerolimit & interval120 == zerolimit)
                        setyellow = true;
                    else if (interval000 == zerolimit & interval001 == zerolimit & (interval002 > zerolimit |
                        interval005 > zerolimit | interval010 > zerolimit | interval030 > zerolimit |
                        interval060 > zerolimit | interval120 > zerolimit))
                        setred = true;
                    if (Convert.ToString(e.CellValue) != "")
                    {
                        if (setyellow)
                            e.Cell.BackColor = System.Drawing.Color.Yellow;
                        else if (setred)
                        {
                            e.Cell.BackColor = System.Drawing.Color.Red;
                            e.Cell.ForeColor = System.Drawing.Color.White;
                        }
                    }
                    break;
                case "001-002":
                    e.Cell.BackColor = System.Drawing.Color.LightGreen;
                    if (Convert.ToString(e.CellValue) != "")
                    {

                    }
                    break;
                case "002-005":
                    e.Cell.BackColor = System.Drawing.Color.LightGreen;
                    if (Convert.ToString(e.CellValue) != "")
                    {
                        if (Convert.ToInt32(e.CellValue) > upperlimit)
                            setred = true;
                        else if (Convert.ToInt32(e.CellValue) > lowerlimit & Convert.ToInt32(e.CellValue) <= upperlimit)
                            setyellow = true;
                        //Reset cell color
                        if (setyellow)
                            e.Cell.BackColor = System.Drawing.Color.Yellow;
                        else if (setred)
                        {
                            e.Cell.BackColor = System.Drawing.Color.Red;
                            e.Cell.ForeColor = System.Drawing.Color.White;
                        }
                    }
                    break;
                case "005-010":
                    e.Cell.BackColor = System.Drawing.Color.LightGreen;
                    if (Convert.ToString(e.CellValue) != "")
                    {
                        if (Convert.ToInt32(e.CellValue) > upperlimit)
                            setred = true;
                        else if (Convert.ToInt32(e.CellValue) > lowerlimit & Convert.ToInt32(e.CellValue) <= upperlimit)
                            setyellow = true;
                        //Reset cell color
                        if (setyellow)
                            e.Cell.BackColor = System.Drawing.Color.Yellow;
                        else if (setred)
                        {
                            e.Cell.BackColor = System.Drawing.Color.Red;
                            e.Cell.ForeColor = System.Drawing.Color.White;
                        }
                    }
                    break;
                case "010-030":
                    e.Cell.BackColor = System.Drawing.Color.LightGreen;
                    if (Convert.ToString(e.CellValue) != "")
                    {
                        if (Convert.ToInt32(e.CellValue) > upperlimit)
                            setred = true;
                        else if (Convert.ToInt32(e.CellValue) > lowerlimit & Convert.ToInt32(e.CellValue) <= upperlimit)
                            setyellow = true;
                        //Reset cell color
                        if (setyellow)
                            e.Cell.BackColor = System.Drawing.Color.Yellow;
                        else if (setred)
                        {
                            e.Cell.BackColor = System.Drawing.Color.Red;
                            e.Cell.ForeColor = System.Drawing.Color.White;
                        }
                    }
                    break;
                case "030-060":
                    e.Cell.BackColor = System.Drawing.Color.LightGreen;
                    if (Convert.ToString(e.CellValue) != "")
                    {
                        if (Convert.ToInt32(e.CellValue) > upperlimit)
                            setred = true;
                        else if (Convert.ToInt32(e.CellValue) > lowerlimit & Convert.ToInt32(e.CellValue) <= upperlimit)
                            setyellow = true;
                        //Reset cell color
                        if (setyellow)
                            e.Cell.BackColor = System.Drawing.Color.Yellow;
                        else if (setred)
                        {
                            e.Cell.BackColor = System.Drawing.Color.Red;
                            e.Cell.ForeColor = System.Drawing.Color.White;
                        }
                    }
                    break;
                case "060-120":
                    e.Cell.BackColor = System.Drawing.Color.LightGreen;
                    if (Convert.ToString(e.CellValue) != "")
                    {
                        if (Convert.ToInt32(e.CellValue) > lowerlimit)
                            setred = true;
                        else if (Convert.ToInt32(e.CellValue) > zerolimit & Convert.ToInt32(e.CellValue) <= lowerlimit)
                            setyellow = true;
                        //Reset cell color
                        if (setyellow)
                            e.Cell.BackColor = System.Drawing.Color.Yellow;
                        else if (setred)
                        {
                            e.Cell.BackColor = System.Drawing.Color.Red;
                            e.Cell.ForeColor = System.Drawing.Color.White;
                        }
                    }
                    break;
                case "120-INF":
                    e.Cell.BackColor = System.Drawing.Color.LightGreen;
                    if (Convert.ToString(e.CellValue) != "")
                    {
                        if (Convert.ToInt32(e.CellValue) > lowerlimit)
                            setred = true;
                        else if (Convert.ToInt32(e.CellValue) > zerolimit & Convert.ToInt32(e.CellValue) <= lowerlimit)
                            setyellow = true;
                        //Reset cell color
                        if (setyellow)
                            e.Cell.BackColor = System.Drawing.Color.Yellow;
                        else if (setred)
                        {
                            e.Cell.BackColor = System.Drawing.Color.Red;
                            e.Cell.ForeColor = System.Drawing.Color.White;
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        protected void TravelerGrid_CustomColumnDisplayText(object sender, ASPxGridViewColumnDisplayTextEventArgs e)
        {
            if (e.Column.FieldName == "MailServerName")
            {
                //3/26/2015 NS added for DevEx upgrade to 14.2
                e.EncodeHtml = false;
                e.DisplayText = e.Value + "<br />" + "<span style=\"font-size:10px; font-weight:normal; color:black\">Location: " + e.GetFieldValue("Location") + "</span>";
            }
        }

        protected void TravelerGrid_PageSizeChanged(object sender, EventArgs e)
        {
            //ProfilesGridView.PageIndex;
            VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("TravelerUsersDevicesOS|TravelerGrid", TravelerGrid.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        }

        protected void DeviceSyncWebchart_CustomCallback(object sender, DevExpress.XtraCharts.Web.CustomCallbackEventArgs e)
        {
            this.DeviceSyncsWebChart.Width = new Unit(Convert.ToInt32(chartWidth.Value) / 2 - 25);
        }

        protected void httpSessionsWebChart_BoundDataChanged(object sender, EventArgs e)
        {
            UI uiobj = new UI();
            uiobj.RecalibrateChartAxes(httpSessionsWebChart.Diagram);
        }

        protected void httpSessionsWebChart_CustomCallback(object sender, DevExpress.XtraCharts.Web.CustomCallbackEventArgs e)
        {
            this.httpSessionsWebChart.Width = new Unit(Convert.ToInt32(chartWidth.Value) / 2 - 25);
        }

        public DataTable SetGridForTravelerInterval(string servername)
        {
            DataTable dt = new DataTable();
            try
            {
                dt = VSWebBL.DashboardBL.LotusTravelerHealthBLL.Ins.SetGridForTravelerInterval(servername);
            }
            catch (Exception ex)
            {
                Response.Write("Error :" + ex);
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
            return dt;
        }

        public void RecalculateDeviceSyncChart(string servername)
        {
            DeviceSyncsWebChart.Series.Clear();
            DataTable dt = VSWebBL.DashboardBL.LotusTravelerHealthBLL.Ins.SetGraphForDeviceSyncs(servername);

            Series series = new Series("DeviceSync", ViewType.Line);
            series.Visible = true;
            series.ArgumentDataMember = dt.Columns["Date"].ToString();

            ValueDataMemberCollection seriesValueDataMembers = (ValueDataMemberCollection)series.ValueDataMembers;
            seriesValueDataMembers.AddRange(dt.Columns["StatValue"].ToString());

            DeviceSyncsWebChart.Series.Add(series);
            DeviceSyncsWebChart.SeriesTemplate.View = new SideBySideBarSeriesView();
            DeviceSyncsWebChart.SeriesTemplate.ChangeView(ViewType.Line);
            LineSeriesView view = (LineSeriesView)DeviceSyncsWebChart.SeriesTemplate.View;
            //7/31/2015 NS modified
            //view.LineMarkerOptions.Visible = true;
            //view.LineMarkerOptions.Size = 8;
            ((LineSeriesView)series.View).MarkerVisibility = DefaultBoolean.False;
            //((LineSeriesView)series.View).LineMarkerOptions.Color = Color.White;
            DeviceSyncsWebChart.Legend.Visible = false;

            XYDiagram seriesXY = (XYDiagram)DeviceSyncsWebChart.Diagram;
            seriesXY.AxisX.Title.Text = "Time";
            seriesXY.AxisX.Title.Visible = true;
            seriesXY.AxisX.Label.ResolveOverlappingOptions.AllowRotate = true;
            seriesXY.AxisX.Label.ResolveOverlappingOptions.AllowHide = false;
            //4/18/2014 NS commented out for VSPLUS-312
            //seriesXY.AxisX.DateTimeGridAlignment = DateTimeMeasurementUnit.Hour;
            seriesXY.AxisX.DateTimeMeasureUnit = DateTimeMeasurementUnit.Hour;
            //seriesXY.AxisX.DateTimeOptions.Format = DateTimeFormat.ShortTime;

            seriesXY.AxisY.Title.Text = "Number of Syncs";
            seriesXY.AxisY.Title.Visible = true;
            //10/7/2013 NS added
            seriesXY.AxisY.Label.ResolveOverlappingOptions.AllowRotate = false;
            seriesXY.AxisY.NumericOptions.Format = NumericFormat.Number;
            seriesXY.AxisY.NumericOptions.Precision = 0;

            //7/31/2015 NS added
            ((LineSeriesView)DeviceSyncsWebChart.Series[0].View).LineStyle.Thickness = 2;

            AxisBase axisy = ((XYDiagram)DeviceSyncsWebChart.Diagram).AxisY;
            axisy.Range.AlwaysShowZeroLevel = false;

            DeviceSyncsWebChart.DataSource = dt;
            DeviceSyncsWebChart.DataBind();

            UI uiobj = new UI();
            uiobj.RecalibrateChartAxes(DeviceSyncsWebChart.Diagram,null,"int","int");
        }

        protected void httpSessionsASPxRadioButtonList_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetGraphForHttpSessions(httpSessionsASPxRadioButtonList.Value.ToString(), servernamelbl.Text);
            //10/8/2015 NS added for VSPLUS-2208
            SetGraphForJavaMemory(httpSessionsASPxRadioButtonList.Value.ToString(), servernamelbl.Text);
            SetGraphForCMemory(httpSessionsASPxRadioButtonList.Value.ToString(), servernamelbl.Text);
        }

        public DataTable SetGraphForHttpSessions(string paramval, string servername)
        {
            httpSessionsWebChart.Series.Clear();
            DataTable dt = VSWebBL.DashboardBL.LotusTravelerHealthBLL.Ins.SetGraphForHttpSessions(paramval, servername);

            if (dt.Rows.Count > 0)
            {
                Series series = new Series("HttpSessions", ViewType.Line);

                series.Visible = true;

                series.ArgumentDataMember = dt.Columns["Date"].ToString();

                ValueDataMemberCollection seriesValueDataMembers = (ValueDataMemberCollection)series.ValueDataMembers;
                seriesValueDataMembers.AddRange(dt.Columns["StatValue"].ToString());

                httpSessionsWebChart.Series.Add(series);

                XYDiagram seriesXY = (XYDiagram)httpSessionsWebChart.Diagram;
                seriesXY.AxisX.Title.Text = "Time";
                seriesXY.AxisX.Title.Visible = true;
                seriesXY.AxisY.Title.Text = "Sessions";
                seriesXY.AxisY.Title.Visible = true;
                seriesXY.AxisY.Label.ResolveOverlappingOptions.AllowRotate = false;
                //9/2/2015 NS added
                seriesXY.AxisY.NumericOptions.Format = NumericFormat.Number;
                seriesXY.AxisY.NumericOptions.Precision = 0;
                seriesXY.AxisY.GridSpacingAuto = false;
                ConstantLine constantLine1 = new ConstantLine("Constant Line 1");
                seriesXY.AxisY.ConstantLines.Add(constantLine1);
                constantLine1.AxisValue = dt.Rows[0][2].ToString();
                constantLine1.Visible = true;
                constantLine1.ShowBehind = false;
                constantLine1.Title.Visible = true;
                constantLine1.Title.Text = "Threshold : " + dt.Rows[0][2].ToString();
                constantLine1.Title.TextColor = Color.Red;
                constantLine1.Title.Antialiasing = false;
                constantLine1.Title.Font = new Font("Tahoma", 10, FontStyle.Regular);
                constantLine1.Title.ShowBelowLine = false;
                constantLine1.Title.Alignment = ConstantLineTitleAlignment.Near;
                constantLine1.Color = Color.Red;
                constantLine1.LineStyle.DashStyle = DashStyle.Solid;
                //constantLine1.LineStyle.Thickness = 2;
                //7/31/2015 NS modified
                //((LineSeriesView)series.View).LineMarkerOptions.Size = 8;
                //((LineSeriesView)series.View).LineMarkerOptions.Color = Color.White;
                ((LineSeriesView)series.View).MarkerVisibility = DefaultBoolean.False;
                AxisBase axisx = ((XYDiagram)httpSessionsWebChart.Diagram).AxisX;
                if (paramval == "hh")
                {
                    //4/18/2014 NS commented out for VSPLUS-312
                    //axisx.DateTimeGridAlignment = DateTimeMeasurementUnit.Hour;
                    axisx.DateTimeMeasureUnit = DateTimeMeasurementUnit.Hour;
                    //axisx.DateTimeOptions.Format = DateTimeFormat.ShortTime;
                    //9/2/2015 NS commented out
                    //axisx.GridSpacingAuto = true;
                }
                else
                {
                    //4/18/2014 NS commented out for VSPLUS-312
                    //axisx.DateTimeGridAlignment = DateTimeMeasurementUnit.Day;
                    //axisx.DateTimeOptions.Format = DateTimeFormat.ShortDate;
                    //9/2/2015 NS commented out
                    //axisx.GridSpacingAuto = true;
                }
                //axisx.DateTimeOptions.FormatString = "dd/MM HH:mm";
                axisx.Range.SideMarginsEnabled = true;
                axisx.GridLines.Visible = true;

                AxisBase axisy = ((XYDiagram)httpSessionsWebChart.Diagram).AxisY;
                axisy.Range.AlwaysShowZeroLevel = false;
                axisy.Range.SideMarginsEnabled = true;

                //((LineSeriesView)series.View).Color = Color.Blue;

                httpSessionsWebChart.Legend.Visible = false;

                httpSessionsWebChart.DataSource = dt;
                httpSessionsWebChart.DataBind();
                UI uiobj = new UI();
                uiobj.RecalibrateChartAxes(httpSessionsWebChart.Diagram);
            }
            return dt;
        }

        protected void UpdatePanel_Unload(object sender, EventArgs e)
        {
            System.Reflection.MethodInfo methodInfo = typeof(ScriptManager).GetMethods(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                .Where(i => i.Name.Equals("System.Web.UI.IScriptManagerInternal.RegisterUpdatePanel")).First();
            methodInfo.Invoke(ScriptManager.GetCurrent(Page), new object[] { sender as UpdatePanel });
        }

        //10/8/2015 NS added for VSPLUs-2208
        protected void JavaMemWebChart_BoundDataChanged(object sender, EventArgs e)
        {
            UI uiobj = new UI();
            uiobj.RecalibrateChartAxes(JavaMemWebChart.Diagram);
        }

        protected void JavaMemWebChart_CustomCallback(object sender, DevExpress.XtraCharts.Web.CustomCallbackEventArgs e)
        {
            this.JavaMemWebChart.Width = new Unit(Convert.ToInt32(chartWidth.Value) / 2 - 25);
        }

        public DataTable SetGraphForJavaMemory(string paramval, string servername)
        {
            JavaMemWebChart.Series.Clear();
            DataTable dt = VSWebBL.DashboardBL.LotusTravelerHealthBLL.Ins.SetGraphForJavaMemory(paramval, servername);

            if (dt.Rows.Count > 0)
            {
                Series series = new Series("JavaMem", ViewType.Line);

                series.Visible = true;

                series.ArgumentDataMember = dt.Columns["Date"].ToString();

                ValueDataMemberCollection seriesValueDataMembers = (ValueDataMemberCollection)series.ValueDataMembers;
                seriesValueDataMembers.AddRange(dt.Columns["StatValue"].ToString());

                JavaMemWebChart.Series.Add(series);

                XYDiagram seriesXY = (XYDiagram)JavaMemWebChart.Diagram;
                seriesXY.AxisX.Title.Text = "Time";
                seriesXY.AxisX.Title.Visible = true;
                seriesXY.AxisY.Title.Text = "Memory (MB)";
                seriesXY.AxisY.Title.Visible = true;
                seriesXY.AxisY.Label.ResolveOverlappingOptions.AllowRotate = false;
                //9/2/2015 NS added
                seriesXY.AxisY.NumericOptions.Format = NumericFormat.Number;
                seriesXY.AxisY.NumericOptions.Precision = 0;
                //seriesXY.AxisY.GridSpacingAuto = false;
                ((LineSeriesView)series.View).MarkerVisibility = DefaultBoolean.False;
                AxisBase axisx = ((XYDiagram)JavaMemWebChart.Diagram).AxisX;
                axisx.DateTimeMeasureUnit = DateTimeMeasurementUnit.Hour;
                //axisx.DateTimeOptions.Format = DateTimeFormat.ShortTime;
                axisx.Range.SideMarginsEnabled = true;
                axisx.GridLines.Visible = true;

                AxisBase axisy = ((XYDiagram)JavaMemWebChart.Diagram).AxisY;
                axisy.Range.AlwaysShowZeroLevel = false;
                axisy.Range.SideMarginsEnabled = true;

                JavaMemWebChart.Legend.Visible = false;

                JavaMemWebChart.DataSource = dt;
                JavaMemWebChart.DataBind();
                UI uiobj = new UI();
                uiobj.RecalibrateChartAxes(JavaMemWebChart.Diagram);
            }
            return dt;
        }

        protected void CMemWebChart_BoundDataChanged(object sender, EventArgs e)
        {
            UI uiobj = new UI();
            uiobj.RecalibrateChartAxes(CMemWebChart.Diagram);
        }

        protected void CMemWebChart_CustomCallback(object sender, DevExpress.XtraCharts.Web.CustomCallbackEventArgs e)
        {
            this.CMemWebChart.Width = new Unit(Convert.ToInt32(chartWidth.Value) / 2 - 25);
        }

        public DataTable SetGraphForCMemory(string paramval, string servername)
        {
            CMemWebChart.Series.Clear();
            DataTable dt = VSWebBL.DashboardBL.LotusTravelerHealthBLL.Ins.SetGraphForCMemory(paramval, servername);

            if (dt.Rows.Count > 0)
            {
                Series series = new Series("CMem", ViewType.Line);

                series.Visible = true;

                series.ArgumentDataMember = dt.Columns["Date"].ToString();

                ValueDataMemberCollection seriesValueDataMembers = (ValueDataMemberCollection)series.ValueDataMembers;
                seriesValueDataMembers.AddRange(dt.Columns["StatValue"].ToString());

                CMemWebChart.Series.Add(series);

                XYDiagram seriesXY = (XYDiagram)CMemWebChart.Diagram;
                seriesXY.AxisX.Title.Text = "Time";
                seriesXY.AxisX.Title.Visible = true;
                seriesXY.AxisY.Title.Text = "Memory (MB)";
                seriesXY.AxisY.Title.Visible = true;
                seriesXY.AxisY.Label.ResolveOverlappingOptions.AllowRotate = false;
                //9/2/2015 NS added
                seriesXY.AxisY.NumericOptions.Format = NumericFormat.Number;
                seriesXY.AxisY.NumericOptions.Precision = 0;
                //seriesXY.AxisY.GridSpacingAuto = false;
                ((LineSeriesView)series.View).MarkerVisibility = DefaultBoolean.False;
                AxisBase axisx = ((XYDiagram)CMemWebChart.Diagram).AxisX;
                axisx.DateTimeMeasureUnit = DateTimeMeasurementUnit.Hour;
                //axisx.DateTimeOptions.Format = DateTimeFormat.ShortDate;
                axisx.Range.SideMarginsEnabled = true;
                axisx.GridLines.Visible = true;

                AxisBase axisy = ((XYDiagram)CMemWebChart.Diagram).AxisY;
                axisy.Range.AlwaysShowZeroLevel = false;
                axisy.Range.SideMarginsEnabled = true;

                CMemWebChart.Legend.Visible = false;
                CMemWebChart.DataSource = dt;
                CMemWebChart.DataBind();
                UI uiobj = new UI();
                uiobj.RecalibrateChartAxes(CMemWebChart.Diagram);
            }
            return dt;
        }



        public DataTable SetGrid1(string servername)
        {
            DataTable dt = new DataTable();
            try
            {
                dt = VSWebBL.DashboardBL.LotusTravelerHealthBLL.Ins.SetGrid1(servername);

                //Combines all the reasoning into one long string seperated by new lines
                for (int i = 0; i < dt.Rows.Count - 1; i++)
                {
                    if (dt.Rows[i]["Name"].ToString() == dt.Rows[i + 1]["Name"].ToString())
                    {
                        dt.Rows[i]["Reasons"] += "<br /><br />" + dt.Rows[i + 1]["Reasons"].ToString();
                        dt.Rows.Remove(dt.Rows[i + 1]);
                        i--;
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write("Error :" + ex);
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
            return dt;
        }

        protected void hfNameLabel_Load(object sender, EventArgs e)
        {
            System.Web.UI.WebControls.Label StatusLabel = (System.Web.UI.WebControls.Label)sender;

            if (StatusLabel.Text == "Red")
                StatusLabel.ForeColor = System.Drawing.Color.White;
            else
                StatusLabel.ForeColor = System.Drawing.Color.Black;
        }

        protected void grid_PageSizeChanged(object sender, EventArgs e)
        {
            //ProfilesGridView.PageIndex;
            VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("TravelerUsersDevicesOS|grid", grid.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        }


        protected void grid_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            if (e.DataColumn.FieldName == "Status" && e.CellValue.ToString() == "Running")
            {
                e.Cell.BackColor = System.Drawing.Color.LightGreen;
                e.Cell.ForeColor = System.Drawing.Color.Black;
            }
            else if (e.DataColumn.FieldName == "Status")
            {
                //2/8/2013 NS added per AF's request
                if (e.CellValue.ToString() == "Green")
                {
                    e.Cell.BackColor = System.Drawing.Color.LightGreen;
                    e.Cell.ForeColor = System.Drawing.Color.Black;
                }
                else
                {
                    if (e.CellValue.ToString() == "Yellow")
                    {
                        e.Cell.BackColor = System.Drawing.Color.Yellow;
                        e.Cell.ForeColor = System.Drawing.Color.Black;
                    }
                    else
                    {
                        if (e.CellValue.ToString() == "Red")
                        {
                            e.Cell.BackColor = System.Drawing.Color.Red;
                            e.Cell.ForeColor = System.Drawing.Color.White;
                        }
                        else if (e.CellValue.ToString() == "")
                        {
                            e.Cell.BackColor = System.Drawing.Color.Empty;
                            e.Cell.ForeColor = System.Drawing.Color.Empty;
                        }
                    }
                }
            }

            if (e.DataColumn.FieldName == "HTTP_Status" && e.CellValue.ToString().Trim() == "OK")
            {
                e.Cell.BackColor = System.Drawing.Color.LightGreen;
                e.Cell.ForeColor = System.Drawing.Color.Black;
            }
            else if (e.DataColumn.FieldName == "HTTP_Status")
            {
                e.Cell.BackColor = System.Drawing.Color.Red;
                e.Cell.ForeColor = System.Drawing.Color.White;
            }
            else if (e.DataColumn.FieldName == "HTTP_Status" && e.CellValue.ToString() == "")
            {
                e.Cell.BackColor = System.Drawing.Color.Empty;
                e.Cell.ForeColor = System.Drawing.Color.Empty;
            }

            //1/21/2014 NS added
            if (e.DataColumn.FieldName == "HA_Datastore_Status")
            {
                if (e.CellValue.ToString() == "Pass")
                {
                    e.Cell.BackColor = System.Drawing.Color.LightGreen;
                    e.Cell.ForeColor = System.Drawing.Color.Black;
                }
                else if (e.CellValue.ToString() == "Fail")
                {
                    e.Cell.BackColor = System.Drawing.Color.Red;
                    e.Cell.ForeColor = System.Drawing.Color.White;
                }
            }
            //2/5/2014 NS added for VSPLUS-328
            if (e.DataColumn.FieldName == "TravelerServlet")
            {
                if (e.CellValue.ToString() == "OK")
                {
                    e.Cell.BackColor = System.Drawing.Color.LightGreen;
                    e.Cell.ForeColor = System.Drawing.Color.Black;
                }
                else if (e.CellValue.ToString() == "Not Checked")
                {
                    e.Cell.BackColor = System.Drawing.Color.White;
                    e.Cell.ForeColor = System.Drawing.Color.Black;
                }
                else if (e.DataColumn.FieldName == "TravelerServlet" && e.CellValue.ToString() == "")
                {
                    e.Cell.BackColor = System.Drawing.Color.Empty;
                    e.Cell.ForeColor = System.Drawing.Color.Empty;
                }
                //else
                //{
                //    e.Cell.BackColor = System.Drawing.Color.Red;
                //    e.Cell.ForeColor = System.Drawing.Color.White;
                //}
            }
            if (e.DataColumn.FieldName == "DevicesAPIStatus")
            {
                if (e.CellValue.ToString() == "")
                {
                    e.Cell.BackColor = System.Drawing.Color.Empty;
                    e.Cell.ForeColor = System.Drawing.Color.Empty;
                }
                else if (e.CellValue.ToString() == "Pass")
                {
                    e.Cell.BackColor = System.Drawing.Color.LightGreen;
                    e.Cell.ForeColor = System.Drawing.Color.Black;
                }

                else
                {
                    e.Cell.BackColor = System.Drawing.Color.Red;
                    e.Cell.ForeColor = System.Drawing.Color.White;
                }
            }
            //5/15/2015 NS added for VSPLUS-1754
            if (e.DataColumn.FieldName == "ResourceConstraint")
            {
                if (e.CellValue.ToString() == "Pass")
                {
                    e.Cell.BackColor = System.Drawing.Color.LightGreen;
                    e.Cell.ForeColor = System.Drawing.Color.Black;
                }
                else if (e.CellValue.ToString() == "Fail")
                {
                    e.Cell.BackColor = System.Drawing.Color.Red;
                    e.Cell.ForeColor = System.Drawing.Color.White;
                }
                else
                {
                    e.Cell.BackColor = System.Drawing.Color.Empty;
                    e.Cell.ForeColor = System.Drawing.Color.Empty;
                }
            }
            //9/1/2015 NS added for VSPLUS-2096
            if (e.DataColumn.FieldName == "HTTP_Details")
            {
                if (e.CellValue.ToString() == "")
                {
                    e.Cell.BackColor = System.Drawing.Color.Empty;
                    e.Cell.ForeColor = System.Drawing.Color.Empty;
                }
                else if (e.CellValue.ToString().Trim() == "OK")
                {
                    e.Cell.BackColor = System.Drawing.Color.LightGreen;
                    e.Cell.ForeColor = System.Drawing.Color.Black;
                }
                else if (e.CellValue.ToString().Trim() == "Warning")
                {
                    e.Cell.BackColor = System.Drawing.Color.Yellow;
                    e.Cell.ForeColor = System.Drawing.Color.Black;
                }
                else if (e.CellValue.ToString().Trim() == "Insufficient")
                {
                    e.Cell.BackColor = System.Drawing.Color.Red;
                    e.Cell.ForeColor = System.Drawing.Color.White;
                }

            }
        }

        protected void grid_SelectionChanged(object sender, EventArgs e)
        {
            if (grid.Selection.Count > 0)
            {
                System.Collections.Generic.List<object> Name = grid.GetSelectedFieldValues("Name");
                System.Collections.Generic.List<object> Status = grid.GetSelectedFieldValues("Status");
                System.Collections.Generic.List<object> HeartBeat = grid.GetSelectedFieldValues("HeartBeat");
                if (Name.Count > 0)
                {
                    ASPxPageControl1.ActiveTabIndex = 2;
                    //DevExpress.Web.ASPxWebControl.RedirectOnCallback("DominoServerDetailsPage2.aspx?Name=" + Name[0].ToString() + "&Type=Traveler" + "&Status=" + Status[0].ToString() + "&LastDate=" + HeartBeat[0].ToString() + "");
                    //Response.Redirect("~/Dashboard/DominoServerDetailsPage2.aspx?Name=" + Name[0].ToString() + "&Type=Traveler" + "&Status=" + Status[0].ToString() + "&LastDate=" + HeartBeat[0].ToString() + "", false);
                    //Context.ApplicationInstance.CompleteRequest();
                }
            }
        }

    }

}






