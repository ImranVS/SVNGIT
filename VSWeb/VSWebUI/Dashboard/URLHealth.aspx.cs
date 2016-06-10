using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DevExpress.XtraCharts;
using System.Drawing;
using System.Web.UI.HtmlControls;

namespace VSWebUI.Dashboard
{
    public partial class URLHealth : System.Web.UI.Page
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
            Page.Title = "URL Health";
            WebChartControl2.Width = new Unit(Convert.ToInt32(chartWidth.Value));
            WebChartControl1.Width = new Unit(Convert.ToInt32(chartWidth.Value));
        
            if (!IsPostBack)
            {
                Session["Type"] = null;
                FillGrid();
                HtmlGenericControl body = (HtmlGenericControl)Master.FindControl("Body1");
                body.Attributes.Add("onload", "DoCallback()");
                body.Attributes.Add("onResize", "Resized()");
                if (Session["UserPreferences"] != null)
                {
                    DataTable UserPreferences = (DataTable)Session["UserPreferences"];
                    foreach (DataRow dr in UserPreferences.Rows)
                    {
                        if (dr[1].ToString() == "URLHealth|urlhealthgrid")
                        {
                            urlhealthgrid.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                        }
                    }
                }
            }
            else
            {
                FillGridfromSession();
            }
            //10/10/2013 NS added
            int index = urlhealthgrid.FocusedRowIndex;
            if (index != -1)
            {
                string Type = urlhealthgrid.GetRowValues(index, "Name").ToString();
                if (Session["Type"] != "" && Session["Type"] != null)
                {
                    if (Session["Type"].ToString() != Type)
                    {
                        Session["Type"] = Type;
                    }
                }
                else
                {
                    Session["Type"] = Type;
                }
            }
            if (Session["Type"] != null)
            {
                PopulateCharts();
            }
        }

        public void FillGrid()
        {
            DataTable dturl = VSWebBL.DashboardBL.URLHealthBL.Ins.Getdata();
            urlhealthgrid.DataSource = dturl;
            urlhealthgrid.DataBind();
            Session["URLGrid"] = dturl;
        }
        public void FillGridfromSession()
        {
            if (Session["URLGrid"] != "" && Session["URLGrid"] != null)
            {
                urlhealthgrid.DataSource = (DataTable)Session["URLGrid"];
                urlhealthgrid.DataBind();
               // Session["URLGrid"] = dturl;
            }
        }

        protected void urlhealthgrid_HtmlDataCellPrepared(object sender, DevExpress.Web.ASPxGridViewTableDataCellEventArgs e)
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
                e.Cell.BackColor = System.Drawing.Color.FromName("#87CEEB");
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

        //protected void ASPxCallbackPanel1_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
        //{
        //    if (IsPostBack)
        //    {


        //        int index = urlhealthgrid.FocusedRowIndex;
        //        string Type = urlhealthgrid.GetRowValues(index, "Name").ToString();

        //        if (Type != "")
        //        {                  

        //            string Name = Type;
        //            Session["Type"] = Type;

        //            DataTable dtUrlResponsetime = VSWebBL.DashboardBL.URLHealthBL.Ins.GetResponseTimeGraphdata(Name);
        //            DataTable dtUrlAvailability = VSWebBL.DashboardBL.URLHealthBL.Ins.GetAvailabilityGraphdata(Name);
        //            WebChartControl1.Series.Clear();
        //            WebChartControl2.Series.Clear();
        //            ChartTitle ct = new ChartTitle();
        //            ct.Text = Name;
        //            WebChartControl1.Titles.Add(ct);
        //            WebChartControl2.Titles.Add(ct);
        //            Series series = null;
        //            series = new Series("URL RP Server", ViewType.Line);
        //            //series = new Series("URL Avail", ViewType.Bar);
        //            series.Visible = true;
        //            series.DataSource = dtUrlResponsetime;
        //            series.ArgumentDataMember = dtUrlResponsetime.Columns["Date"].ToString();

        //            ValueDataMemberCollection seriesValueDataMembers = (ValueDataMemberCollection)series.ValueDataMembers;
        //            seriesValueDataMembers.AddRange(dtUrlResponsetime.Columns["StatValue"].ToString());
        //            WebChartControl1.Series.Add(series);

        //            ((XYDiagram)WebChartControl1.Diagram).PaneLayoutDirection = PaneLayoutDirection.Horizontal;

        //            XYDiagram seriesXY = (XYDiagram)WebChartControl1.Diagram;
        //            seriesXY.AxisY.Title.Text = "Response Time";
        //            seriesXY.AxisY.Title.Visible = true;
        //            seriesXY.AxisX.Title.Text = "Time";
        //            seriesXY.AxisX.Title.Visible = true;
        //            WebChartControl1.Legend.Visible = false;

        //            //((LineSeriesView)series.View).AxisX = 100;
        //            //((SplineSeriesView)series.View).LineMarkerOptions.Size = 4;
        //            //((SplineSeriesView)series.View).LineMarkerOptions.Color = Color.White;

        //            //((LineSeriesView)series.View).LineStyle.Thickness = 1;
        //            ((LineSeriesView)series.View).LineMarkerOptions.Size = 8;
        //            ((LineSeriesView)series.View).LineMarkerOptions.Color = Color.White;
        //            AxisBase axis = ((XYDiagram)WebChartControl1.Diagram).AxisX;
        //            //axis.DateTimeGridAlignment = DateTimeMeasurementUnit.Day;
        //            // axis.NumericOptions.Format=DevExpress.XtraCharts.NumericFormat.Number;
        //            axis.GridSpacingAuto = false;
        //            axis.MinorCount = 15;
        //            //axis.GridSpacing = 5;
        //            axis.Range.SideMarginsEnabled = false;
        //            axis.GridLines.Visible =false;

        //            //axis.DateTimeOptions.Format = DateTimeFormat.Custom;
        //            //axis.DateTimeOptions.FormatString = "MM/dd HH:MM";

        //            ((LineSeriesView)series.View).Color = Color.Blue;

        //            AxisBase axisy = ((XYDiagram)WebChartControl1.Diagram).AxisY;
        //            axisy.Range.AlwaysShowZeroLevel = false;
        //            axisy.Range.SideMarginsEnabled = true;
        //            axisy.GridLines.Visible = true;

        //            Series series1 = null;
        //            series1 = new Series("URL Avail", ViewType.Line);
        //            series1.DataSource = dtUrlAvailability;
        //            series1.ArgumentDataMember = dtUrlAvailability.Columns["Date"].ToString();
        //            series1.Visible = true;
        //            ValueDataMemberCollection seriesValueDataMembers1 = (ValueDataMemberCollection)series1.ValueDataMembers;
        //            seriesValueDataMembers1.AddRange(dtUrlAvailability.Columns["StatValue"].ToString());
        //            WebChartControl2.Series.Add(series1);

        //            WebChartControl2.Legend.Visible = false;

        //           // ((SideBySideBarSeriesView)series1.View).ColorEach = true;

        //            //AxisBase axisx = ((XYDiagram)deviceTypeWebChart.Diagram).AxisX;

        //            XYDiagram seriesXY1 = (XYDiagram)WebChartControl2.Diagram;
        //            // seriesXY.AxisX.Title.Text = "Date and Time";
        //            //seriesXY.AxisX.Title.Visible = true;
        //            seriesXY1.AxisX.Label.ResolveOverlappingOptions.AllowRotate = false;
        //            seriesXY1.AxisY.Title.Text = "Hourly Up Percent";
        //            seriesXY1.AxisY.Title.Visible = true;
        //            seriesXY1.AxisX.Title.Text = "Time";
        //            seriesXY1.AxisX.Title.Visible = true;

        //            //((SideBySideBarSeriesView)series1.View).BarWidth = 5;
        //            //((SideBySideBarSeriesView)series1.View).BarDistanceFixed = 10;
        //            AxisBase axisy1 = ((XYDiagram)WebChartControl2.Diagram).AxisY;
        //            axisy1.GridLines.Visible = false;
        //            axisy1.Range.AlwaysShowZeroLevel = false;
        //            AxisBase axis1 = ((XYDiagram)WebChartControl2.Diagram).AxisX;
        //            axis1.GridSpacingAuto = false;
        //            axis1.MinorCount = 15;
        //            axis1.GridSpacing = 5;
        //            //axis1.DateTimeOptions.FormatString = "MM/dd HH:MM";
        //            axis1.GridLines.Visible = false;
        //            //WebChartControl1.DataSource = dtClusterHealth;
        //            //WebChartControl1.DataBind();
        //            WebChartControl1.Visible = true;
        //            WebChartControl2.Visible = true;
        //            //DevExpress.Web.ASPxWebControl.RedirectOnCallback("DominoServerDetailsPage2.aspx?Name=" + Name + "");

        //        }
        //    }
        //    }

        protected void urlhealthgrid_HtmlRowCreated(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
        {
            e.Row.Attributes.Add("onmouseover", "this.originalcolor=this.style.backgroundColor;" + "this.style.backgroundColor='#C0C0C0';");

            e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalcolor;");

        }

        protected void WebChartControl2_CustomCallback(object sender, DevExpress.XtraCharts.Web.CustomCallbackEventArgs e)
        {
            WebChartControl2.Width = new Unit(Convert.ToInt32(chartWidth.Value));
        }

        protected void WebChartControl1_CustomCallback(object sender, DevExpress.XtraCharts.Web.CustomCallbackEventArgs e)
        {
            WebChartControl1.Width = new Unit(Convert.ToInt32(chartWidth.Value));
        }

        public void PopulateCharts()
        {
            string Name = Session["Type"].ToString();
            DataTable dtUrlResponsetime = VSWebBL.DashboardBL.URLHealthBL.Ins.GetResponseTimeGraphdata(Name);
                DataTable dtUrlAvailability = VSWebBL.DashboardBL.URLHealthBL.Ins.GetAvailabilityGraphdata(Name);
                WebChartControl1.Series.Clear();
                WebChartControl2.Series.Clear();
                ChartTitle ct = new ChartTitle();
                ct.Text = Name;
                if (WebChartControl1.Titles.Count > 0)
                {
                    WebChartControl1.Titles.Clear();
                }
                WebChartControl1.Titles.Add(ct);
                if (WebChartControl2.Titles.Count > 0)
                {
                    WebChartControl2.Titles.Clear();
                }
                WebChartControl2.Titles.Add(ct);
                Series series = null;
                series = new Series("URL RP Server", ViewType.Line);
                //series = new Series("URL Avail", ViewType.Bar);
                series.Visible = true;
                series.DataSource = dtUrlResponsetime;
                series.ArgumentDataMember = dtUrlResponsetime.Columns["Date"].ToString();

                ValueDataMemberCollection seriesValueDataMembers = (ValueDataMemberCollection)series.ValueDataMembers;
                seriesValueDataMembers.AddRange(dtUrlResponsetime.Columns["StatValue"].ToString());
                WebChartControl1.Series.Add(series);

                ((XYDiagram)WebChartControl1.Diagram).PaneLayoutDirection = PaneLayoutDirection.Horizontal;

                XYDiagram seriesXY = (XYDiagram)WebChartControl1.Diagram;
                seriesXY.AxisY.Title.Text = "Response Time (ms)";
                seriesXY.AxisY.Title.Visible = true;
                seriesXY.AxisX.Title.Text = "Time";
                seriesXY.AxisX.Title.Visible = true;
                WebChartControl1.Legend.Visible = false;

                //((LineSeriesView)series.View).AxisX = 100;
                //((SplineSeriesView)series.View).LineMarkerOptions.Size = 4;
                //((SplineSeriesView)series.View).LineMarkerOptions.Color = Color.White;

                //((LineSeriesView)series.View).LineStyle.Thickness = 1;
                ((LineSeriesView)series.View).LineMarkerOptions.Size = 8;
                ((LineSeriesView)series.View).LineMarkerOptions.Color = Color.White;
                AxisBase axis = ((XYDiagram)WebChartControl1.Diagram).AxisX;
                //axis.DateTimeGridAlignment = DateTimeMeasurementUnit.Day;
                // axis.NumericOptions.Format=DevExpress.XtraCharts.NumericFormat.Number;
                axis.GridSpacingAuto = false;
                axis.MinorCount = 15;
                //axis.GridSpacing = 5;
                axis.Range.SideMarginsEnabled = false;
                axis.GridLines.Visible = false;

                //axis.DateTimeOptions.Format = DateTimeFormat.Custom;
                //axis.DateTimeOptions.FormatString = "MM/dd HH:MM";

                ((LineSeriesView)series.View).Color = Color.Blue;

                AxisBase axisy = ((XYDiagram)WebChartControl1.Diagram).AxisY;
                axisy.Range.AlwaysShowZeroLevel = false;
                axisy.Range.SideMarginsEnabled = true;
                axisy.GridLines.Visible = true;

                Series series1 = null;
                series1 = new Series("URL Avail", ViewType.Line);
                series1.DataSource = dtUrlAvailability;
                series1.ArgumentDataMember = dtUrlAvailability.Columns["Date"].ToString();
                series1.Visible = true;
                ((LineSeriesView)series1.View).LineMarkerOptions.Size = 8;
                ((LineSeriesView)series1.View).LineMarkerOptions.Color = Color.White;
                ((LineSeriesView)series1.View).Color = Color.Blue;
                ValueDataMemberCollection seriesValueDataMembers1 = (ValueDataMemberCollection)series1.ValueDataMembers;
                seriesValueDataMembers1.AddRange(dtUrlAvailability.Columns["StatValue"].ToString());
                WebChartControl2.Series.Add(series1);

                WebChartControl2.Legend.Visible = false;

                // ((SideBySideBarSeriesView)series1.View).ColorEach = true;

                //AxisBase axisx = ((XYDiagram)deviceTypeWebChart.Diagram).AxisX;

                XYDiagram seriesXY1 = (XYDiagram)WebChartControl2.Diagram;
                // seriesXY.AxisX.Title.Text = "Date and Time";
                //seriesXY.AxisX.Title.Visible = true;
                seriesXY1.AxisX.Label.ResolveOverlappingOptions.AllowRotate = false;
                seriesXY1.AxisY.Title.Text = "Hourly Up Percent";
                seriesXY1.AxisY.Title.Visible = true;
                seriesXY1.AxisX.Title.Text = "Time";
                seriesXY1.AxisX.Title.Visible = true;

                //((SideBySideBarSeriesView)series1.View).BarWidth = 5;
                //((SideBySideBarSeriesView)series1.View).BarDistanceFixed = 10;
                AxisBase axisy1 = ((XYDiagram)WebChartControl2.Diagram).AxisY;
                axisy1.GridLines.Visible = true;
                axisy1.Range.AlwaysShowZeroLevel = false;
                AxisBase axis1 = ((XYDiagram)WebChartControl2.Diagram).AxisX;
                axis1.GridSpacingAuto = false;
                axis1.MinorCount = 15;
               // axis1.GridSpacing = 5;
                //axis1.DateTimeOptions.FormatString = "MM/dd HH:MM";
                axis1.GridLines.Visible = false;
                WebChartControl1.DataSource = dtUrlResponsetime;
                WebChartControl1.DataBind();
                WebChartControl2.DataSource = dtUrlAvailability;
                WebChartControl2.DataBind();
                WebChartControl1.Visible = true;
                WebChartControl2.Visible = true;
        }

        protected void urlhealthgrid_PageSizeChanged(object sender, EventArgs e)
        {
            //ProfilesGridView.PageIndex;
            VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("URLHealth|urlhealthgrid", urlhealthgrid.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        }

        }
    }
