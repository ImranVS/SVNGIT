using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DevExpress.XtraCharts;
using System.Drawing;
using DevExpress.Web;
using System.Web.UI.HtmlControls;

namespace VSWebUI.Configurator
{
    public partial class LotusTravellerHealth : System.Web.UI.Page
    {
        static string value = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            this.mailFileOpensCumulativeWebChart.Width = new Unit(Convert.ToInt32(chartWidth.Value));
            this.mailFileOpensWebChart.Width = new Unit(Convert.ToInt32(chartWidth.Value));
            this.httpSessionsWebChart.Width = new Unit(Convert.ToInt32(chartWidth.Value));
            //SelectServerRoundPanel.Width = new Unit(Convert.ToInt32(chartWidth.Value));
            Page.Title = "Lotus Traveler Health";
            if (!IsPostBack)
            {
                Session["GridData"] = SetGrid1();
            }
            grid.DataSource = Session["GridData"];
            if (!IsPostBack && !IsCallback)
            {

                HtmlGenericControl body = (HtmlGenericControl)Master.FindControl("Body1");
                body.Attributes.Add("onload", "DoCallback()");
                body.Attributes.Add("onResize", "Resized()");

                grid.DataBind();
                fillservernamecombo();
                fillintervalcombo();

            }
            // FillUserGridFromSession();  
        }

        private void fillservernamecombo()
        {
            DataTable dt = new DataTable();
            dt = VSWebBL.ReportsBL.ReportsBL.Ins.fillMailServer();
            mailServerListComboBox.DataSource = dt;
            mailServerListComboBox.TextField = "MailServerName";
            mailServerListComboBox.ValueField = "MailServerName";
            mailServerListComboBox.DataBind();
            mailServerListComboBox.SelectedIndex = 0;
        }

        private void fillintervalcombo()
        {
            DataTable dt = new DataTable();
            dt = VSWebBL.ReportsBL.ReportsBL.Ins.fillTravelerInterval();
            travelerIntervalComboBox.DataSource = dt;
            travelerIntervalComboBox.TextField = "Interval";
            travelerIntervalComboBox.ValueField = "Interval";
            travelerIntervalComboBox.DataBind();
            if (travelerIntervalComboBox.Items.Count > 0)
            {
                travelerIntervalComboBox.SelectedIndex = 0;
            }
        }

        public DataTable SetGrid1()
        {
            DataTable dt = new DataTable();
            try
            {
                dt = VSWebBL.DashboardBL.LotusTravelerHealthBLL.Ins.SetGrid1();
            }
            catch (Exception ex)
            {
                Response.Write("Error :" + ex);
            }
            return dt;
        }

        //public void FillUserGridFromSession()
        //{
        //    if (Session["Fillgid"] != "" && Session["Fillgid"] != null)
        //    {
        //        UsersGrid.DataSource = (DataTable)Session["Fillgid"];
        //        UsersGrid.DataBind();
        //    }
        //}

        public DataTable SetGraphForHttpSessions(string paramval, string servername)
        {
            httpSessionsWebChart.Series.Clear();
            DataTable dt = VSWebBL.DashboardBL.LotusTravelerHealthBLL.Ins.SetGraphForHttpSessions(paramval, servername);

            if (dt.Rows.Count <= 0)
            {
            }
            else
            {
                Series series = new Series("HttpSessions", ViewType.Spline);

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

                ((SplineSeriesView)series.View).LineTensionPercent = 100;
                ((SplineSeriesView)series.View).LineMarkerOptions.Size = 4;
                ((SplineSeriesView)series.View).LineMarkerOptions.Color = Color.White;

                AxisBase axisx = ((XYDiagram)httpSessionsWebChart.Diagram).AxisX;
                axisx.DateTimeGridAlignment = DateTimeMeasurementUnit.Day;
                axisx.GridSpacingAuto = false;
                axisx.MinorCount = 15;
                axisx.GridSpacing = 0.5;
                axisx.DateTimeOptions.Format = DateTimeFormat.Custom;
                axisx.DateTimeOptions.FormatString = "dd/MM HH:mm";
                axisx.Range.SideMarginsEnabled = true;
                axisx.GridLines.Visible = true;

                AxisBase axisy = ((XYDiagram)httpSessionsWebChart.Diagram).AxisY;
                axisy.Range.AlwaysShowZeroLevel = false;
                axisy.Range.SideMarginsEnabled = true;

                ((SplineSeriesView)series.View).Color = Color.Blue;

                httpSessionsWebChart.Legend.Visible = false;

                httpSessionsWebChart.DataSource = dt;
                httpSessionsWebChart.DataBind();
            }
            return dt;
        }

        //public DataTable SetGraphForDeviceType(string servername)
        //{
        //    deviceTypeWebChart.Series.Clear();
        //    DataTable dt = VSWebBL.DashboardBL.LotusTravelerHealthBLL.Ins.SetGraphForDeviceType(servername);


        //    Series series = new Series("DeviceType", ViewType.Bar);

        //    series.ArgumentDataMember = dt.Columns["DeviceName"].ToString();

        //    ValueDataMemberCollection seriesValueDataMembers = (ValueDataMemberCollection)series.ValueDataMembers;
        //    seriesValueDataMembers.AddRange(dt.Columns["No_of_Users"].ToString());
        //    deviceTypeWebChart.Series.Add(series);

        //    deviceTypeWebChart.Legend.Visible = false;

        //    ((SideBySideBarSeriesView)series.View).ColorEach = true;            

        //    XYDiagram seriesXY = (XYDiagram)deviceTypeWebChart.Diagram;
        //    seriesXY.AxisX.Title.Text = "Device Type";
        //    seriesXY.AxisX.Title.Visible = true;
        //    seriesXY.AxisX.Label.ResolveOverlappingOptions.AllowRotate = true;
        //    seriesXY.AxisX.Label.ResolveOverlappingOptions.AllowHide = false;
        //    seriesXY.AxisY.Title.Text = "Users";
        //    seriesXY.AxisY.Title.Visible = true;

        //    AxisBase axisy = ((XYDiagram)deviceTypeWebChart.Diagram).AxisY;
        //    axisy.Range.AlwaysShowZeroLevel = false;

        //    deviceTypeWebChart.DataSource = dt;
        //    deviceTypeWebChart.DataBind();

        //    return dt;
        //}

        //public DataTable SetGrid(string servername)
        //{
        //    DataTable dt = VSWebBL.DashboardBL.LotusTravelerHealthBLL.Ins.SetGrid(servername);
        //    UsersGrid.DataSource = dt;

        //    UsersGrid.DataBind();
        //    Session["Fillgid"] = dt;            

        //    return dt;
        //}

        protected void httpSessionsASPxRadioButtonList_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetGraphForHttpSessions(httpSessionsASPxRadioButtonList.Value.ToString(), value);
        }

        protected void ASPxCallbackPanel1_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
        {
            string mailservername = "";
            string interval = "";
            int index = grid.FocusedRowIndex;
            value = grid.GetRowValues(index, "Name").ToString();
            if (mailServerListComboBox.SelectedIndex == -1)
            {
                mailservername = mailServerListComboBox.Items[0].Value.ToString();
            }
            else
            {
                mailservername = mailServerListComboBox.SelectedItem.Value.ToString();
            }
            if (travelerIntervalComboBox.SelectedIndex == -1)
            {
                if (travelerIntervalComboBox.Items.Count > 0)
                {
                    interval = travelerIntervalComboBox.Items[travelerIntervalComboBox.Items.Count - 1].Value.ToString();
                }
            }
            else
            {
                interval = travelerIntervalComboBox.SelectedItem.Value.ToString();
            }
            mailFileOpensRoundPanel.HeaderText = "Cumulative Mail File Open Times - " + mailservername;
            mailFileOpensDeltaRoundPanel.HeaderText = "Mail File Open Times Delta - " + mailservername;
            SetGraphForHttpSessions("hh", value);
            SetGraphForMailFileOpensCumulative(value, mailservername, interval);
            SetGraphForMailFileOpens(value, mailservername, interval);

            // SetGraphForDeviceType(value);
            // SetGrid(value);
        }


        //protected void UsersGrid_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
        //{
        //    e.Row.Attributes.Add("onmouseover", "this.style.backgroundColor='#F2F9FF';");
        //    e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='white';");
        //}

        protected void grid_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
        {
            int index = grid.FocusedRowIndex;
            if (e.VisibleIndex != index)
            {
                e.Row.Attributes.Add("onmouseover", "this.style.backgroundColor='#F2F9FF';");
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='white';");
            }
        }



        public DataTable SetGraphForMailFileOpens(string servername, string mailservername, string interval)
        {
            mailFileOpensWebChart.Series.Clear();
            DataTable dt = VSWebBL.DashboardBL.LotusTravelerHealthBLL.Ins.SetGraphForMailFileOpens(servername, mailservername, interval);

            mailFileOpensWebChart.SeriesDataMember = "Interval";
            mailFileOpensWebChart.SeriesTemplate.ArgumentDataMember = "DateUpdated";
            mailFileOpensWebChart.SeriesTemplate.ValueDataMembers.AddRange(new string[] { "Delta" });
            mailFileOpensWebChart.SeriesTemplate.View = new SideBySideBarSeriesView();
            mailFileOpensWebChart.SeriesTemplate.ChangeView(ViewType.Line);
            LineSeriesView view = (LineSeriesView)mailFileOpensWebChart.SeriesTemplate.View;
            view.LineMarkerOptions.Visible = true;
            view.LineMarkerOptions.Size = 8;
            mailFileOpensWebChart.Legend.Visible = true;

            XYDiagram seriesXY = (XYDiagram)mailFileOpensWebChart.Diagram;
            seriesXY.AxisX.Title.Text = "Time of Day";
            seriesXY.AxisX.Title.Visible = true;
            seriesXY.AxisX.Label.ResolveOverlappingOptions.AllowRotate = true;
            seriesXY.AxisX.Label.ResolveOverlappingOptions.AllowHide = false;
            seriesXY.AxisX.DateTimeGridAlignment = DateTimeMeasurementUnit.Minute;
            seriesXY.AxisX.DateTimeMeasureUnit = DateTimeMeasurementUnit.Minute;
            seriesXY.AxisX.DateTimeOptions.Format = DateTimeFormat.ShortTime;

            seriesXY.AxisY.Title.Text = "Open Times Delta";
            seriesXY.AxisY.Title.Visible = true;

            AxisBase axisy = ((XYDiagram)mailFileOpensWebChart.Diagram).AxisY;
            axisy.Range.AlwaysShowZeroLevel = false;

            mailFileOpensWebChart.DataSource = dt;
            mailFileOpensWebChart.DataBind();

            //seriesXY.AxisX.GridSpacingAuto = false;
            double min = Convert.ToDouble(((XYDiagram)mailFileOpensWebChart.Diagram).AxisY.Range.MinValue);
            double max = Convert.ToDouble(((XYDiagram)mailFileOpensWebChart.Diagram).AxisY.Range.MaxValue);

            int gs = (int)((max - min) / 5);

            if (gs == 0)
            {
                gs = 1;
                seriesXY.AxisY.GridSpacingAuto = false;
                seriesXY.AxisY.GridSpacing = gs;
            }
            return dt;
        }

        public DataTable SetGraphForMailFileOpensCumulative(string servername, string mailservername, string interval)
        {
            mailFileOpensCumulativeWebChart.Series.Clear();
            DataTable dt = VSWebBL.DashboardBL.LotusTravelerHealthBLL.Ins.SetGraphForMailFileOpensCumulative(servername, mailservername, interval);

            mailFileOpensCumulativeWebChart.SeriesDataMember = "Interval";
            mailFileOpensCumulativeWebChart.SeriesTemplate.ArgumentDataMember = "DateUpdated";
            mailFileOpensCumulativeWebChart.SeriesTemplate.ValueDataMembers.AddRange(new string[] { "OpenTimes" });
            mailFileOpensCumulativeWebChart.SeriesTemplate.View = new SideBySideBarSeriesView();
            mailFileOpensCumulativeWebChart.SeriesTemplate.ChangeView(ViewType.Line);
            LineSeriesView view = (LineSeriesView)mailFileOpensCumulativeWebChart.SeriesTemplate.View;
            view.LineMarkerOptions.Visible = true;
            view.LineMarkerOptions.Size = 8;
            mailFileOpensCumulativeWebChart.Legend.Visible = true;

            XYDiagram seriesXY = (XYDiagram)mailFileOpensCumulativeWebChart.Diagram;
            seriesXY.AxisX.Title.Text = "Time of Day";
            seriesXY.AxisX.Title.Visible = true;
            seriesXY.AxisX.Label.ResolveOverlappingOptions.AllowRotate = true;
            seriesXY.AxisX.Label.ResolveOverlappingOptions.AllowHide = false;
            seriesXY.AxisX.DateTimeGridAlignment = DateTimeMeasurementUnit.Minute;
            seriesXY.AxisX.DateTimeMeasureUnit = DateTimeMeasurementUnit.Minute;
            seriesXY.AxisX.DateTimeOptions.Format = DateTimeFormat.ShortTime;

            seriesXY.AxisY.Title.Text = "Open Times";
            seriesXY.AxisY.Title.Visible = true;

            AxisBase axisy = ((XYDiagram)mailFileOpensCumulativeWebChart.Diagram).AxisY;
            axisy.Range.AlwaysShowZeroLevel = false;

            mailFileOpensCumulativeWebChart.DataSource = dt;
            mailFileOpensCumulativeWebChart.DataBind();

            //seriesXY.AxisX.GridSpacingAuto = false;
            double min = Convert.ToDouble(((XYDiagram)mailFileOpensCumulativeWebChart.Diagram).AxisY.Range.MinValue);
            double max = Convert.ToDouble(((XYDiagram)mailFileOpensCumulativeWebChart.Diagram).AxisY.Range.MaxValue);

            int gs = (int)((max - min) / 5);

            if (gs == 0)
            {
                gs = 1;
                seriesXY.AxisY.GridSpacingAuto = false;
                seriesXY.AxisY.GridSpacing = gs;
            }
            return dt;
        }

        protected void mailFileOpensCumulativeWebChart_CustomCallback(object sender, DevExpress.XtraCharts.Web.CustomCallbackEventArgs e)
        {
            this.mailFileOpensCumulativeWebChart.Width = new Unit(Convert.ToInt32(chartWidth.Value));

        }

        protected void mailFileOpensWebChart_CustomCallback(object sender, DevExpress.XtraCharts.Web.CustomCallbackEventArgs e)
        {
            this.mailFileOpensWebChart.Width = new Unit(Convert.ToInt32(chartWidth.Value));

        }

        protected void httpSessionsWebChart_CustomCallback(object sender, DevExpress.XtraCharts.Web.CustomCallbackEventArgs e)
        {
            this.httpSessionsWebChart.Width = new Unit(Convert.ToInt32(chartWidth.Value));

        }
        //public DataTable SetGraphForMailFileOpens(string servername)
        //{
        //    mailFileOpensWebChart.Series.Clear();
        //    DataTable dt = VSWebBL.DashboardBL.LotusTravelerHealthBLL.Ins.SetGraphForMailFileOpens(servername);

        //    mailFileOpensWebChart.SeriesDataMember = "AllIntervals";
        //    mailFileOpensWebChart.SeriesTemplate.ArgumentDataMember = "DateUpdated";
        //    mailFileOpensWebChart.SeriesTemplate.ValueDataMembers.AddRange(new string[] {"Delta"});
        //    mailFileOpensWebChart.SeriesTemplate.View = new SideBySideBarSeriesView();
        //    mailFileOpensWebChart.SeriesTemplate.ChangeView(ViewType.Line);
        //    LineSeriesView view = (LineSeriesView)mailFileOpensWebChart.SeriesTemplate.View;
        //    view.LineMarkerOptions.Visible = true;
        //    mailFileOpensWebChart.Legend.Visible = true;

        //    XYDiagram seriesXY = (XYDiagram)mailFileOpensWebChart.Diagram;
        //    seriesXY.AxisX.Title.Text = "Time of Day";
        //    seriesXY.AxisX.Title.Visible = true;
        //    seriesXY.AxisX.Label.ResolveOverlappingOptions.AllowRotate = true;
        //    seriesXY.AxisX.Label.ResolveOverlappingOptions.AllowHide = false;
        //    seriesXY.AxisX.DateTimeGridAlignment = DateTimeMeasurementUnit.Minute;
        //    seriesXY.AxisX.DateTimeMeasureUnit = DateTimeMeasurementUnit.Minute;
        //    seriesXY.AxisX.DateTimeOptions.Format = DateTimeFormat.ShortTime;

        //    seriesXY.AxisY.Title.Text = "Open Times Delta";
        //    seriesXY.AxisY.Title.Visible = true;

        //    AxisBase axisy = ((XYDiagram)mailFileOpensWebChart.Diagram).AxisY;
        //    axisy.Range.AlwaysShowZeroLevel = false;

        //    mailFileOpensWebChart.DataSource = dt;
        //    mailFileOpensWebChart.DataBind();

        //    //seriesXY.AxisX.GridSpacingAuto = false;
        //    double min = Convert.ToDouble(((XYDiagram)mailFileOpensWebChart.Diagram).AxisY.Range.MinValue);
        //    double max = Convert.ToDouble(((XYDiagram)mailFileOpensWebChart.Diagram).AxisY.Range.MaxValue);

        //    int gs = (int)((max - min) / 5);

        //    if (gs == 0)
        //    {
        //        gs = 1;
        //        seriesXY.AxisY.GridSpacingAuto = false;
        //        seriesXY.AxisY.GridSpacing = gs;
        //    }
        //    return dt;
        //}
    }
}