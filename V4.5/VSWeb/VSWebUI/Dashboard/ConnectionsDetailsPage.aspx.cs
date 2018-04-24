using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DevExpress.XtraCharts;

namespace VSWebUI.Dashboard
{
    public partial class ConnectionsDetailsPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
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
                if (Request.QueryString["Name"] != "" && Request.QueryString["Name"] != null)
                {
                    servernamelbl.Text = Request.QueryString["Name"];
                    servernamelbldisp.InnerHtml = lblServerType.Text + servernamelbl.Text;
                }
                //4/11/2016 Sowjanya modified for VSPLUS-2813
                if (Request.QueryString["LastDate"] != "" && Request.QueryString["LastDate"] != null)
                {
                     Lastscanned.InnerHtml = "Last scan date: " + Request.QueryString["LastDate"].ToString();
                }
                else
                {
                    DataTable dt = VSWebBL.DashboardBL.QuickrHealthBLL.Ins.getLastScanDate(servernamelbl.Text);
                    if (dt.Rows.Count > 0)
                    {
                          Lastscanned.InnerHtml = "Last scan date: " + dt.Rows[0]["LastUpdate"].ToString();
                    }
                    else
                    {
                         Lastscanned.Style.Add("display", "none");
                    }


                }



            }
            else
            {
                
            }
            if (!IsPostBack && !IsCallback)
            {
                Fillmaintenancegrid();
                FillAlertHistory();
                FillOutageTab();
                FillHealthAssessmentStatusGrid(Request.QueryString["Type"].ToString(), servernamelbl.Text);
                if (Session["UserPreferences"] != null)
                {
                    DataTable UserPreferences = (DataTable)Session["UserPreferences"];
                    foreach (DataRow dr in UserPreferences.Rows)
                    {
                        if (dr[1].ToString() == "ConnectionsDetailsPage|maintenancegrid")
                        {
                            maintenancegrid.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                        }
                        if (dr[1].ToString() == "ConnectionsDetailsPage|AlertGridView")
                        {
                            AlertGridView.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                        }

                        if (dr[1].ToString() == "ConnectionsDetailsPage|OutageGridView")
                        {
                            OutageGridView.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                        }
                    }
                }
            }
            else
            {
                Fillmaintenancegridfromsession();
                FillAlertHistoryfromSession();
                FilloutagefromSession();
                FillHealthAssessmentFromSession();
            }
            SetGraphForUsersDaily();
            SetGraphForActivities();
            SetGraphForBlogs();
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
                    else if (Request.QueryString["Type"] == "IBM Connections")
                    {
                        bl = VSWebBL.SettingBL.SettingsBL.Ins.UpdateScanvalue("ScanIBMConnectionsASAP", StatusObj.Name, VSWeb.Constants.Constants.SysString);
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
                Session["Submenu"] = "IBMConnections";
                if (Request.QueryString["Name"] != "" && Request.QueryString["Name"] != null)
                {
                    id = (VSWebBL.SecurityBL.ServersBL.Ins.GetServerIDbyServerName(Request.QueryString["Name"].ToString())).ToString();
                    Response.Redirect("~/Configurator/IBMConnections.aspx?ID=" + id, false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
                    Context.ApplicationInstance.CompleteRequest();
                }
            }
        }

        public DataTable SetGraphForUsersDaily()
        {
            //string fromdate = "";
            //string todate = "";
            //fromdate = dtPick.FromDate;
            //todate = dtPick.ToDate;
            UsersDailyWebChart.Series.Clear();
            DataTable dt = VSWebBL.DashboardBL.ConnectionsBL.Ins.GetUsersDailyCount("%created_last_day%", Request.QueryString["Name"]);
            UsersDailyWebChart.SeriesDataMember = "StatName";
            UsersDailyWebChart.SeriesTemplate.ArgumentDataMember = "Date";
            UsersDailyWebChart.SeriesTemplate.ValueDataMembers.AddRange(new string[] { "StatValue" });
            ((LineSeriesView)UsersDailyWebChart.SeriesTemplate.View).LineMarkerOptions.Size = 7;
            ChartTitle title = new ChartTitle();

            title.Text = "Daily Activities";
            System.Drawing.Font font = new System.Drawing.Font(title.Font.FontFamily.Name, 12);
            title.Font = font;

            UsersDailyWebChart.Titles.Clear();
            UsersDailyWebChart.Titles.Add(title);
            UI uiobj = new UI();
            uiobj.RecalibrateChartAxes(UsersDailyWebChart.Diagram, "Y", "int", "int");
            UsersDailyWebChart.DataSource = dt;
            UsersDailyWebChart.DataBind();

            return dt;
        }

        public DataTable SetGraphForActivities()
        {
            //string fromdate = "";
            //string todate = "";
            //fromdate = dtPick.FromDate;
            //todate = dtPick.ToDate;
            ActivitiesWebChart.Series.Clear();
            DataTable dt = VSWebBL.DashboardBL.ConnectionsBL.Ins.GetUserStatsCommon("NUM_OF_ACTIVITIES_", "_CREATED_YESTERDAY", Request.QueryString["Name"]);
            ActivitiesWebChart.SeriesDataMember = "StatName";
            ActivitiesWebChart.SeriesTemplate.ArgumentDataMember = "Date";
            ActivitiesWebChart.SeriesTemplate.ValueDataMembers.AddRange(new string[] { "StatValue" });
            ((LineSeriesView)ActivitiesWebChart.SeriesTemplate.View).LineMarkerOptions.Size = 7;
            ActivitiesWebChart.DataSource = dt;
            ActivitiesWebChart.DataBind();
            return dt;
        }

        public DataTable SetGraphForBlogs()
        {
            BlogsWebChart.Series.Clear();
            DataTable dt = VSWebBL.DashboardBL.ConnectionsBL.Ins.GetUserStatsCommon("NUM_OF_BLOGS_", "_CREATED_YESTERDAY", Request.QueryString["Name"]);
            BlogsWebChart.SeriesDataMember = "StatName";
            BlogsWebChart.SeriesTemplate.ArgumentDataMember = "Date";
            BlogsWebChart.SeriesTemplate.ValueDataMembers.AddRange(new string[] { "StatValue" });
            ((LineSeriesView)BlogsWebChart.SeriesTemplate.View).LineMarkerOptions.Size = 7;
            BlogsWebChart.DataSource = dt;
            BlogsWebChart.DataBind();
            return dt;
        }

        protected void ClearButton_Click(object sender, EventArgs e)
        {
            txtfromdate.Text = "";
            txttodate.Text = "";
            ASPxTimeEdit1.Text = "";
            ASPxTimeEdit2.Text = "";
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

        protected void OKButton_Click(object sender, EventArgs e)
        {
            MsgPopupControl.ShowOnPageLoad = false;
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
    }
}