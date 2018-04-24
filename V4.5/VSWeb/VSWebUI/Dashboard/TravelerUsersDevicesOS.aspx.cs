using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using System.Data;
using System.Web.UI.HtmlControls;
using DevExpress.XtraCharts;
using System.Drawing;
using System.Globalization;
using DevExpress.Utils;

namespace VSWebUI.Dashboard
{
    public partial class TravelerUsersDevicesOS_NEW : System.Web.UI.Page
    {
		protected void Page_PreInit(object sender, EventArgs e)
		{

			this.Master.contentCallEvent += new EventHandler(Master_ButtonClick);
		}

		private void Master_ButtonClick(object sender, EventArgs e)
		{

		}

        static string value = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            //errorDiv.Style.Value = "display: none;";
            grid.Settings.ShowFilterRow = false;
            int index = 0;
            //8/19/2014 NS added for VSPLUS-884
            //7/31/2015 NS modified
            //this.DeviceSyncsWebChart.Width = new Unit(Convert.ToInt32(chartWidth.Value));
            //8/25/2015 NS modified for VSPLUS-2096
            //this.DeviceSyncsWebChart.Width = new Unit(Convert.ToInt32(chartWidth.Value)/2);
            //this.mailFileOpensCumulativeWebChart.Width = new Unit(Convert.ToInt32(chartWidth.Value));
            //this.mailFileOpensCumulativeWebChart.Width = new Unit(Convert.ToInt32(chartWidth.Value)/2);
            //this.mailFileOpensWebChart.Width = new Unit(Convert.ToInt32(chartWidth.Value));
            //this.mailFileOpensWebChart.Width = new Unit(Convert.ToInt32(chartWidth.Value)/2);
            //this.httpSessionsWebChart.Width = new Unit(Convert.ToInt32(chartWidth.Value));
            //8/25/2015 NS modified for VSPLUS-2096
            //this.httpSessionsWebChart.Width = new Unit(Convert.ToInt32(chartWidth.Value)/2);
            //3/28/2014 NS commented out for
            //this.deviceTypeWebChart.Width = new Unit(Convert.ToInt32(chartWidth.Value));
            //this.OSTypeWebChartControl.Width = new Unit(Convert.ToInt32(chartWidth.Value));
            //SelectServerRoundPanel.Width = new Unit(Convert.ToInt32(chartWidth.Value));

            if (!IsPostBack)
            {
               
                //2/7/2013 NS added
                //2/5/2014 NS commented out the line below for now (1.2.3) until properly tested 
                //UpdateMenuVisibility();
                if (Session["UserPreferences"] != null)
                {
                    DataTable UserPreferences = (DataTable)Session["UserPreferences"];
                    foreach (DataRow dr in UserPreferences.Rows)
                    {
                        if (dr[1].ToString() == "TravelerUsersDevicesOS|grid")
                        {
                            grid.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                        }
                        if (dr[1].ToString() == "TravelerUsersDevicesOS|TravelerGrid")
                        {
                            //8/25/2015 NS modified for VSPLUS-2096
                            //TravelerGrid.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                        }
                        //3/28/2014 NS commented out for
                        /*
                        if (dr[1].ToString() == "TravelerUsersDevicesOS|UsersGrid")
                        {
                            UsersGrid.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                        }
                         */
                    }
                }
            }
            Session["GridData"] = SetGrid1();
            grid.DataSource = Session["GridData"];

            DataTable dt2 = new DataTable();
            
            dt2 = (DataTable)Session["GridData"];
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
            if (!IsPostBack && !IsCallback)
            {
                //10/7/2013 NS moved the code to the end
                HtmlGenericControl body = (HtmlGenericControl)Master.FindControl("Body1");
                //8/25/2015 NS modified for VSPLUS-2096
                //body.Attributes.Add("onload", "DoCallback()");
                //body.Attributes.Add("onResize", "Resized()");

                grid.DataBind();
                //if (grid.DetailRows.VisibleCount > 0)
                if (grid.VisibleRowCount > 0)
                {
                    //8/26/2015 NS modified
                    //value = grid.GetRowValues(index, "Name").ToString();
                    System.Collections.Generic.List<object> Name = grid.GetSelectedFieldValues("Name");
                    if (Name.Count > 0)
                    {
                        value = Name[0].ToString();
                        fillservernamecombo(value);
                    }
                }
                fillintervalcombo();
                //3/28/2014 NS commented out for
                /*
                fillDeviceTypeServer();
                fillOSTypeCombo();
                UsersGrid.FocusedRowIndex = -1;
                SetGrid(0, 0);
                SetGraphForDeviceType(ServerComboBox.Text);
                TravelerTrackBar.Enabled = false;
                TravelerTrackBar.Value = 15;
                SetGraphForDevice_OSType(OSComboBox.Text);
                */
            }

            else
            {
                if (mailServerListComboBox.SelectedIndex != -1 && travelerIntervalComboBox.SelectedIndex != -1)
                {
                    Session["CurrentMailSrvInd"] = mailServerListComboBox.SelectedIndex;
                }
                //3/28/2014 NS commented out for
                //FillUserGridFromSession();
                index = grid.FocusedRowIndex;
                //if (grid.DetailRows.VisibleCount > 0)
                //{
                if (grid.VisibleRowCount > 0)
                {
                    //8/26/2015 NS modified
                    //value = grid.GetRowValues(index, "Name").ToString();
                    System.Collections.Generic.List<object> Name = grid.GetSelectedFieldValues("Name");
                    if (Name.Count > 0)
                    {
                        value = Name[0].ToString();
                        fillservernamecombo(value);
                    }
                    if (Session["CurrentMailSrvInd"] != null && Session["CurrentMailSrvInd"] != "")
                    {
                        mailServerListComboBox.SelectedIndex = Convert.ToInt32(Session["CurrentMailSrvInd"].ToString());
                    }
                }
                //}
                //2/6/2013 NS moved the call below from the If block above so that the graph is set every time,
                //otherwise, when a radio button changes values on the Users tab, the chart is emptied
                //3/28/2014 NS commented out for 
                //SetGraphForDeviceType(ServerComboBox.Text);
            }

            //8/19/2014 NS added for VSPLUS-884
            //8/25/2015 NS modified for VSPLUS-2096
            //DeviceSyncsRoundPanel.HeaderText = "Successful Device Syncs - " + value;
            //httpSessionsASPxRoundPanel.HeaderText = "HTTP Sessions - " + value;
            //11/19/2014 NS modified
            //MailServerlbl.Text = value + "'" + "s access to mail servers";
            //8/25/2015 NS modified for VSPLUS-2096
            //MailServerlbl.InnerHtml = value + "'" + "s access to mail servers";
            //TravelerGrid.DataSource = SetGridForTravelerInterval(value);
            //TravelerGrid.DataBind();
            // if(!IsPostBack)
            //10/7/2013 NS added
            if (grid.VisibleRowCount > 0)
            {
                if (mailServerListComboBox.SelectedIndex != -1 && travelerIntervalComboBox.SelectedIndex != -1)
                {
                    //8/25/2015 NS modified for VSPLUS-2096
                    //RecalulateGraphs();
                }
                //8/19/2014 NS added for VSPLUS-884
                //8/25/2015 NS modified for VSPLUS-2096
                //RecalculateDeviceSyncChart(value);
                //SetGraphForHttpSessions(httpSessionsASPxRadioButtonList.Value.ToString(), value);
            }

        }
        public string SetIcon(GridViewDataItemTemplateContainer Container)
        {
            bool imgset = false;
            System.Web.UI.WebControls.Image img = (System.Web.UI.WebControls.Image)Container.FindControl("IconImage");
            Label lbl = (Label)Container.FindControl("lblIcon");
            string lblOS = lbl.Text;
            CultureInfo culture = new CultureInfo("");
            //8/16/2013 NS modified
            //if (lblOS.Contains("Android") == true)
            if (culture.CompareInfo.IndexOf(lblOS, "Android", CompareOptions.IgnoreCase) >= 0)
            {
                img.ImageUrl = "~/images/icons/android-icon.png";
                imgset = true;
            }

            //if (lblOS.Contains("Apple") == true)
            if (culture.CompareInfo.IndexOf(lblOS, "Apple", CompareOptions.IgnoreCase) >= 0)
            {
                img.ImageUrl = "~/images/icons/os_icon_mac.png";
                imgset = true;
            }

            //8/15/2013 NS added
            //if (lblOS.Contains("RIM") == true)
            if (culture.CompareInfo.IndexOf(lblOS, "RIM", CompareOptions.IgnoreCase) >= 0)
            {
                img.ImageUrl = "~/images/icons/rim.png";
                imgset = true;
            }
            //if (lblOS.Contains("Win") == true)
            if (culture.CompareInfo.IndexOf(lblOS, "Win", CompareOptions.IgnoreCase) >= 0)
            {
                img.ImageUrl = "~/images/icons/winphone.png";
                imgset = true;
            }

            if (!imgset)
            {
                img.ImageUrl = "~/images/icons/phone.png";
                imgset = true;
            }
            return "";
        }
        private void fillservernamecombo(string servername)
        {
            DataTable dt = new DataTable();
            dt = VSWebBL.ReportsBL.ReportsBL.Ins.fillMailbyServerName(servername);
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

        //3/28/2014 NS commented out for 
        /*
        public void fillDeviceTypeServer()
        {
            DataTable dt = new DataTable();
            dt = VSWebBL.DashboardBL.LotusTravelerHealthBLL.Ins.SetGrid1();
            DataRow dr = dt.NewRow();
            dr[0] = "All";
            dt.Rows.InsertAt(dr, 0);
            dt = dt.DefaultView.ToTable(true, "Name");
            ServerComboBox.DataSource = dt;
            ServerComboBox.TextField = "Name";
            ServerComboBox.ValueField = "Name";
            ServerComboBox.DataBind();
            ServerComboBox.SelectedIndex = 0;

        }
        
        public void fillOSTypeCombo()
        {
            DataTable dt = new DataTable();
            dt = VSWebBL.DashboardBL.LotusTravelerHealthBLL.Ins.SetGrid1();
            DataRow dr = dt.NewRow();
            dr[0] = "All";
            dt.Rows.InsertAt(dr, 0);
            dt = dt.DefaultView.ToTable(true, "Name");
            OSComboBox.DataSource = dt;
            OSComboBox.TextField = "Name";
            OSComboBox.ValueField = "Name";
            OSComboBox.DataBind();
            OSComboBox.SelectedIndex = 0;
        }
        */


        //8/25/2015 NS modified for VSPLUS-2096
        /*
        public DataTable SetGraphForHttpSessions(string paramval, string servername)
        {
            httpSessionsWebChart.Series.Clear();
            DataTable dt = VSWebBL.DashboardBL.LotusTravelerHealthBLL.Ins.SetGraphForHttpSessions(paramval, servername);

            if (dt.Rows.Count <= 0)
            {
            }
            else
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
                    axisx.DateTimeMeasureUnit = DateTimeMeasurementUnit.Minute;
                    axisx.DateTimeOptions.Format = DateTimeFormat.ShortTime;
                    axisx.GridSpacingAuto = true;
                }
                else
                {
                    //4/18/2014 NS commented out for VSPLUS-312
                    //axisx.DateTimeGridAlignment = DateTimeMeasurementUnit.Day;
                    axisx.DateTimeOptions.Format = DateTimeFormat.ShortDate;
                    axisx.GridSpacingAuto = true;
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
            }
            return dt;
        }
         */

        protected void httpSessionsASPxRadioButtonList_SelectedIndexChanged(object sender, EventArgs e)
        {
            //8/25/2015 NS modified for VSPLUS-2096
            //SetGraphForHttpSessions(httpSessionsASPxRadioButtonList.Value.ToString(), value);
        }

        protected void ASPxCallbackPanel1_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
        {
            string mailservername = "";
            string interval = "";
            //if (grid.DetailRows.VisibleCount > 0)
            //{
            int index = grid.FocusedRowIndex;
            //7/8/2013 NS modified - if grid is empty, an error is thrown "Object variable not set"
            if (index > -1)
            {
                value = grid.GetRowValues(index, "Name").ToString();
                //}

                if (mailServerListComboBox.SelectedIndex == -1)
                {
                    if (mailServerListComboBox.Items.Count > 0)
                    {
                        mailservername = mailServerListComboBox.Items[0].Value.ToString();
                    }
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
                //8/25/2015 NS modified for VSPLUS-2096
                //SetGraphForHttpSessions("hh", value);
                SetGraphForMailFileOpensCumulative(value, mailservername, interval);
                SetGraphForMailFileOpens(value, mailservername, interval);
                //SetGridForTravelerInterval(value);       
            }
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
        protected void grid_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
        {
            int index = grid.FocusedRowIndex;
            if (e.VisibleIndex != index)
            {

                e.Row.Attributes.Add("onmouseover", "this.originalcolor=this.style.backgroundColor;" + "this.style.backgroundColor='#C0C0C0';"); //'#C0C0C0'

                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalcolor;");

            }
        }

        public DataTable SetGraphForMailFileOpens(string servername, string mailservername, string interval)
        {
            //mailFileOpensWebChart.Series.Clear();
            DataTable dt = VSWebBL.DashboardBL.LotusTravelerHealthBLL.Ins.SetGraphForMailFileOpens(servername, mailservername, interval);
            mailFileOpensWebChart.SeriesDataMember = "Interval";
            mailFileOpensWebChart.SeriesTemplate.ArgumentDataMember = "DateUpdated";
            mailFileOpensWebChart.SeriesTemplate.ValueDataMembers.AddRange(new string[] { "Delta" });
            mailFileOpensWebChart.SeriesTemplate.View = new SideBySideBarSeriesView();
            mailFileOpensWebChart.SeriesTemplate.ChangeView(ViewType.Line);
            LineSeriesView view = (LineSeriesView)mailFileOpensWebChart.SeriesTemplate.View;
            //view.LineMarkerOptions.Visible = true;
            //view.LineMarkerOptions.Size = 8;
            //7/31/2015 NS added
            view.MarkerVisibility = DefaultBoolean.False;

            mailFileOpensWebChart.Legend.Visible = true;

            XYDiagram seriesXY = (XYDiagram)mailFileOpensWebChart.Diagram;
            seriesXY.AxisX.Title.Text = "Time";
            seriesXY.AxisX.Title.Visible = true;
            seriesXY.AxisX.Label.ResolveOverlappingOptions.AllowRotate = true;
            seriesXY.AxisX.Label.ResolveOverlappingOptions.AllowHide = false;
            //4/18/2014 NS commented out for VSPLUS-312
            //seriesXY.AxisX.DateTimeGridAlignment = DateTimeMeasurementUnit.Hour;
            seriesXY.AxisX.DateTimeMeasureUnit = DateTimeMeasurementUnit.Minute;
            seriesXY.AxisX.DateTimeOptions.Format = DateTimeFormat.ShortTime;

            seriesXY.AxisY.Title.Text = "Open Times Delta";
            seriesXY.AxisY.Title.Visible = true;
            //10/7/2013 NS added
            seriesXY.AxisY.Label.ResolveOverlappingOptions.AllowRotate = false;
            seriesXY.AxisY.NumericOptions.Format = NumericFormat.Number;
            seriesXY.AxisY.NumericOptions.Precision = 0;
            //seriesXY.AxisY.GridSpacingAuto = true;

            AxisBase axisy = ((XYDiagram)mailFileOpensWebChart.Diagram).AxisY;
            axisy.Range.AlwaysShowZeroLevel = false;
            mailFileOpensWebChart.DataSource = dt;
            mailFileOpensWebChart.DataBind();
  
            return dt;
        }

        public DataTable SetGraphForMailFileOpensCumulative(string servername, string mailservername, string interval)
        {
            //mailFileOpensCumulativeWebChart.Series.Clear();
            DataTable dt = VSWebBL.DashboardBL.LotusTravelerHealthBLL.Ins.SetGraphForMailFileOpensCumulative(servername, mailservername, interval);

            mailFileOpensCumulativeWebChart.SeriesDataMember = "Interval";
            mailFileOpensCumulativeWebChart.SeriesTemplate.ArgumentDataMember = "DateUpdated";
            mailFileOpensCumulativeWebChart.SeriesTemplate.ValueDataMembers.AddRange(new string[] { "OpenTimes" });
            mailFileOpensCumulativeWebChart.SeriesTemplate.View = new LineSeriesView();
            //mailFileOpensCumulativeWebChart.SeriesTemplate.ChangeView(ViewType.Line);
            LineSeriesView view = (LineSeriesView)mailFileOpensCumulativeWebChart.SeriesTemplate.View;
            //view.LineMarkerOptions.Visible = true;
            //view.LineMarkerOptions.Size = 8;
            //7/31/2015 NS added
            view.MarkerVisibility = DefaultBoolean.False;

            mailFileOpensCumulativeWebChart.Legend.Visible = true;

            XYDiagram seriesXY = (XYDiagram)mailFileOpensCumulativeWebChart.Diagram;
            seriesXY.AxisX.Title.Text = "Time";
            seriesXY.AxisX.Title.Visible = true;
            seriesXY.AxisX.Label.ResolveOverlappingOptions.AllowRotate = true;
            seriesXY.AxisX.Label.ResolveOverlappingOptions.AllowHide = false;
            //4/18/2014 NS commented out for VSPLUS-312
            //seriesXY.AxisX.DateTimeGridAlignment = DateTimeMeasurementUnit.Hour;
            seriesXY.AxisX.DateTimeMeasureUnit = DateTimeMeasurementUnit.Minute;
            seriesXY.AxisX.DateTimeOptions.Format = DateTimeFormat.ShortTime;

            seriesXY.AxisY.Title.Text = "Open Times";
            seriesXY.AxisY.Title.Visible = true;
            //10/7/2013 NS added
            seriesXY.AxisY.Label.ResolveOverlappingOptions.AllowRotate = false;
            seriesXY.AxisY.NumericOptions.Format = NumericFormat.Number;
            seriesXY.AxisY.NumericOptions.Precision = 0;

            mailFileOpensCumulativeWebChart.DataSource = dt;
            mailFileOpensCumulativeWebChart.DataBind();

            AxisBase axisy = ((XYDiagram)mailFileOpensCumulativeWebChart.Diagram).AxisY;
            axisy.Range.AlwaysShowZeroLevel = false;
            return dt;
        }

        protected void mailFileOpensCumulativeWebChart_CustomCallback(object sender, DevExpress.XtraCharts.Web.CustomCallbackEventArgs e)
        {
            this.mailFileOpensCumulativeWebChart.Width = new Unit(Convert.ToInt32(chartWidth.Value)/2);
        }

        protected void mailFileOpensWebChart_CustomCallback(object sender, DevExpress.XtraCharts.Web.CustomCallbackEventArgs e)
        {
            this.mailFileOpensWebChart.Width = new Unit(Convert.ToInt32(chartWidth.Value)/2);
        }

        protected void httpSessionsWebChart_CustomCallback(object sender, DevExpress.XtraCharts.Web.CustomCallbackEventArgs e)
        {
            //8/25/2015 NS modified for VSPLUS-2096
            //this.httpSessionsWebChart.Width = new Unit(Convert.ToInt32(chartWidth.Value)/2);

        }

        //3/28/2014 NS commented out for
        /*
        public DataTable SetGrid(int lastmin, int agomin)
        {
            DataTable dt = VSWebBL.DashboardBL.LotusTravelerHealthBLL.Ins.SetGrid(lastmin, agomin);
            UsersGrid.DataSource = dt;

            UsersGrid.DataBind();

            Session["Fillgid"] = dt;

            return dt;
        }
         
        public void FillUserGridFromSession()
        {
            if (Session["Fillgid"] != "" && Session["Fillgid"] != null)
            {
                UsersGrid.DataSource = (DataTable)Session["Fillgid"];
                UsersGrid.DataBind();
            }
        }
        
        public DataTable SetGraphForDeviceType(string servername)
        {
            deviceTypeWebChart.Series.Clear();
            DataTable dt = VSWebBL.DashboardBL.LotusTravelerHealthBLL.Ins.SetGraphForDeviceType(servername);


            Series series = new Series("DeviceType", ViewType.Bar);

            series.ArgumentDataMember = dt.Columns["DeviceName"].ToString();

            ValueDataMemberCollection seriesValueDataMembers = (ValueDataMemberCollection)series.ValueDataMembers;
            seriesValueDataMembers.AddRange(dt.Columns["No_of_Users"].ToString());
            deviceTypeWebChart.Series.Add(series);

            deviceTypeWebChart.Legend.Visible = false;

            ((SideBySideBarSeriesView)series.View).ColorEach = true;

            XYDiagram seriesXY = (XYDiagram)deviceTypeWebChart.Diagram;
            seriesXY.AxisX.Title.Text = "Device Type";
            seriesXY.AxisX.Title.Visible = true;
            seriesXY.AxisX.Label.ResolveOverlappingOptions.AllowRotate = true;
            seriesXY.AxisX.Label.ResolveOverlappingOptions.AllowHide = false;
            seriesXY.AxisY.Label.ResolveOverlappingOptions.AllowRotate = false;
            seriesXY.AxisY.Title.Text = "Users";
            seriesXY.AxisY.Title.Visible = true;

            //1/8/2013 NS added recalculations to display whole numbers on the y axis
            double min = Convert.ToDouble(((XYDiagram)deviceTypeWebChart.Diagram).AxisY.Range.MinValue);
            double max = Convert.ToDouble(((XYDiagram)deviceTypeWebChart.Diagram).AxisY.Range.MaxValue);

            int gs = (int)((max - min) / 5);

            if (gs == 0)
            {
                gs = 1;
                seriesXY.AxisY.GridSpacingAuto = false;
                seriesXY.AxisY.GridSpacing = gs;
            }
            else
            {
                seriesXY.AxisY.GridSpacingAuto = true;
            }

            AxisBase axisy = ((XYDiagram)deviceTypeWebChart.Diagram).AxisY;
            axisy.Range.AlwaysShowZeroLevel = false;
            ChartTitle title = new ChartTitle();
            if (servername == "All")
            {
                title.Text = "Traveler Devices for All Servers";
            }
            else
            {
                title.Text = "Traveler Devices for " + servername;
            }
            deviceTypeWebChart.Titles.Clear();
            deviceTypeWebChart.Titles.Add(title);
            deviceTypeWebChart.DataSource = dt;
            deviceTypeWebChart.DataBind();

            return dt;
        }
        
        public DataTable SetGraphForDevice_OSType(string servername)
        {
            OSTypeWebChartControl.Series.Clear();
            DataTable dt = VSWebBL.DashboardBL.LotusTravelerHealthBLL.Ins.SetGraphForDevice_OSType(servername);
            Series series = new Series("OSType", ViewType.Pie);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                series.Points.Add(new SeriesPoint(dt.Rows[i]["OS_Type"], dt.Rows[i]["No_of_Users"]));
            }

            OSTypeWebChartControl.Series.Add(series);
            series.Label.PointOptions.PointView = PointView.Argument;

            //series.Label.PointOptions.ValueNumericOptions.Format = NumericFormat.General;
            // series.Label.PointOptions.ValueNumericOptions.Precision = 0;

            OSTypeWebChartControl.Legend.Visible = false;
            ChartTitle title = new ChartTitle();
            if (servername == "All")
            {
                title.Text = "Traveler Devices OS for All Servers";
            }
            else
            {
                title.Text = "Traveler Devices OS for " + servername;
            }
            OSTypeWebChartControl.Titles.Clear();
            OSTypeWebChartControl.Titles.Add(title);



            OSTypeWebChartControl.DataSource = dt;
            OSTypeWebChartControl.DataBind();

            return dt;
        }
        
        protected void UsersGrid_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
        {
            int index = UsersGrid.FocusedRowIndex;
            if (e.VisibleIndex != index)
            {

                e.Row.Attributes.Add("onmouseover", "this.originalcolor=this.style.backgroundColor;" + "this.style.backgroundColor='#C0C0C0';"); //'#C0C0C0'


                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalcolor;");
            }
        } 
        
        protected void travelerButtonList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (travelerButtonList.Items[0].Selected == true)
            {
                SetGrid(0, 0);
                TravelerTrackBar.Enabled = false;
                TravelerTrackBar.Value = 15;
            }
            else if (travelerButtonList.Items[1].Selected == true)
            {
                SetGrid(Convert.ToInt32(TravelerTrackBar.Value), 0);
                TravelerTrackBar.Enabled = true;
            }
            else if (travelerButtonList.Items[2].Selected == true)
            {
                SetGrid(0, Convert.ToInt32(TravelerTrackBar.Value));
                TravelerTrackBar.Enabled = true;
            }

        }
        
        protected void TravelerusersPopupMenu_ItemClick(object source, DevExpress.Web.MenuItemEventArgs e)
        {

            if (e.Item.Name == "DenyAccess")
            {
                if (UsersGrid.FocusedRowIndex > -1)
                {
                    YesButton.Visible = true;
                    NOButton.Visible = true;
                    CancelButton.Visible = false;
                    // CancelButton.Text = "Cancel";
                    Session["MenuItem"] = "Deny";
                    DenyAccess();
                }
                else
                {
                    msgPopupControl.HeaderText = "Deny Access";
                    msgPopupControl.ShowOnPageLoad = true;
                    msgLabel.Text = "Please select a device in the grid.";
                    YesButton.Visible = false;
                    NOButton.Visible = false;
                    CancelButton.Visible = true;
                    CancelButton.Text = "OK";
                }
            }
            if (e.Item.Name == "WipeDevice")
            {
                if (UsersGrid.FocusedRowIndex > -1)
                {
                    YesButton.Visible = true;
                    NOButton.Visible = true;
                    CancelButton.Text = "Cancel";
                    Session["MenuItem"] = "wipe";

                    //  WipeDevice Form Code 
                    WipeDevice();

                }
                else
                {
                    msgPopupControl.HeaderText = "Wipe Device";
                    msgLabel.Text = "Please select a device in the grid.";
                    msgPopupControl.ShowOnPageLoad = true;
                    YesButton.Visible = false;
                    NOButton.Visible = false;
                    CancelButton.Visible = true;
                    CancelButton.Text = "OK";
                }


            }
            if (e.Item.Name == "ClearWipeRequest")
            {
                if (UsersGrid.FocusedRowIndex > -1)
                {
                    YesButton.Visible = true;
                    NOButton.Visible = true;
                    // CancelButton.Text = "Cancel";
                    CancelButton.Visible = false;
                    Session["MenuItem"] = "Clearwipe";
                    ClearWipe();
                }
                else
                {
                    msgPopupControl.HeaderText = "Clear Wipe Request";
                    msgLabel.Text = "Please select a device in the grid.";
                    msgPopupControl.ShowOnPageLoad = true;
                    YesButton.Visible = false;
                    NOButton.Visible = false;
                    CancelButton.Visible = true;
                    CancelButton.Text = "OK";
                }

            }
            if (e.Item.Name == "ChangeApproval-Deny")
            {
                if (UsersGrid.FocusedRowIndex > -1)
                {
                    YesButton.Visible = true;
                    NOButton.Visible = true;
                    CancelButton.Visible = false;
                    //CancelButton.Text = "Cancel";
                    Session["MenuItem"] = "changeDeny";
                    changeDeny();
                }
                else
                {
                    msgPopupControl.HeaderText = "Change Approval - Deny";
                    msgLabel.Text = "Please select a device in the grid.";
                    msgPopupControl.ShowOnPageLoad = true;
                    YesButton.Visible = false;
                    NOButton.Visible = false;
                    CancelButton.Visible = true;
                    CancelButton.Text = "OK";
                }
            }
            if (e.Item.Name == "ChangeApproval-Approve")
            {
                if (UsersGrid.FocusedRowIndex > -1)
                {
                    YesButton.Visible = true;
                    NOButton.Visible = true;
                    CancelButton.Visible = false;
                    //CancelButton.Text = "Cancel";
                    Session["MenuItem"] = "ChangeApprove";
                    ChangeApprove();
                }
                else
                {
                    msgPopupControl.HeaderText = "Change Approval - Approve";
                    msgLabel.Text = "Please select a device in the grid.";
                    msgPopupControl.ShowOnPageLoad = true;
                    YesButton.Visible = false;
                    NOButton.Visible = false;
                    CancelButton.Visible = true;
                    CancelButton.Text = "OK";
                }
            }
            if (e.Item.Name == "LogLevel-DisableFinest")
            {
                if (UsersGrid.FocusedRowIndex > -1)
                {
                    YesButton.Visible = true;
                    NOButton.Visible = true;
                    CancelButton.Visible = false;
                    // CancelButton.Text = "Cancel";
                    Session["MenuItem"] = "LogDisable";
                    LogDisable();
                }
                else
                {
                    msgPopupControl.HeaderText = "Log Level - Disable Finest";
                    msgLabel.Text = "Please select a device in the grid.";
                    msgPopupControl.ShowOnPageLoad = true;
                    YesButton.Visible = false;
                    NOButton.Visible = false;
                    CancelButton.Visible = true;
                    CancelButton.Text = "OK";
                }

            }
            if (e.Item.Name == "LogLevel-EnableFinest")
            {
                if (UsersGrid.FocusedRowIndex > -1)
                {
                    YesButton.Visible = true;
                    NOButton.Visible = true;
                    CancelButton.Visible = false;
                    //CancelButton.Text = "Cancel";
                    Session["MenuItem"] = "LogEnable";
                    LogEnable();
                }
                else
                {
                    msgPopupControl.HeaderText = "Log Level - Enable Finest";
                    msgLabel.Text = "Please select a device in the grid.";
                    msgPopupControl.ShowOnPageLoad = true;
                    YesButton.Visible = false;
                    NOButton.Visible = false;
                    CancelButton.Visible = true;
                    CancelButton.Text = "OK";
                }

            }
            if (e.Item.Name == "LogLevel-CreateDumpFile")
            {
                if (UsersGrid.FocusedRowIndex > -1)
                {
                    YesButton.Visible = true;
                    NOButton.Visible = true;
                    CancelButton.Visible = false;
                    // CancelButton.Text = "Cancel";
                    Session["MenuItem"] = "LogCreate";
                    LogCreate();
                }
                else
                {
                    msgPopupControl.HeaderText = "Log Level - Create Dump File";
                    msgLabel.Text = "Please select a device in the grid.";
                    msgPopupControl.ShowOnPageLoad = true;
                    YesButton.Visible = false;
                    NOButton.Visible = false;
                    CancelButton.Visible = true;
                    CancelButton.Text = "OK";
                }

            }

        }
        public void WipeDevice()
        {
            string myServerName = "";
            string wipeSupported = "";
            string myUserName = "";
            string myDeviceName = "";
            string myDeviceID = "";

            HardCheckBox.Enabled = true;
            TravelerAppCheckBox.Enabled = true;
            StorageCadrCheckBox.Enabled = true;
            DataRow myRow = UsersGrid.GetDataRow(UsersGrid.FocusedRowIndex);
            try
            {
                wipeSupported = myRow["wipeSupported"].ToString();
                myServerName = myRow["ServerName"].ToString();
                myUserName = myRow["UserName"].ToString();
                // Session["myUserName"] = myUserName;
                //  myServerName = myRow["ServerName"].ToString();
                Session["myServerName"] = myServerName;
                myDeviceName = myRow["DeviceName"].ToString();
                // Session["myDeviceName"] = myDeviceName;
                myDeviceID = myRow["DeviceID"].ToString();

                UserNameLabel.Text = myUserName;
                DeviceNameLabel.Text = myDeviceName;
                DeviceIDLabel.Text = myDeviceID;

            }
            catch (Exception)
            {
                wipeSupported = "1";
            }
            switch (wipeSupported)
            {
                case "1":
                    break;
                case "2":
                    HardCheckBox.Enabled = false;
                    break;
                case "3":
                    HardCheckBox.Enabled = true;
                    TravelerAppCheckBox.Enabled = false;
                    StorageCadrCheckBox.Enabled = false;
                    break;
                case "4":
                    HardCheckBox.Enabled = false;
                    TravelerAppCheckBox.Enabled = true;
                    StorageCadrCheckBox.Enabled = false;
                    break;
                case "5":
                    HardCheckBox.Enabled = true;
                    TravelerAppCheckBox.Enabled = true;
                    StorageCadrCheckBox.Enabled = false;
                    break;
                default:
                    HardCheckBox.Enabled = true;
                    TravelerAppCheckBox.Enabled = true;
                    StorageCadrCheckBox.Enabled = true;
                    break;
            }

            WipePopupControl.ShowOnPageLoad = true;
        }
        
        public void DenyAccess()
        {
            string TellCommand = "";
            string myUserName = "";
            string myDeviceName = "";
            string myServerName = "";

            DataRow myRow = UsersGrid.GetDataRow(UsersGrid.FocusedRowIndex);
            try
            {

                myUserName = myRow["UserName"].ToString();
                Session["myUserName"] = myUserName;
                myServerName = myRow["ServerName"].ToString();
                Session["myServerName"] = myServerName;
                myDeviceName = myRow["DeviceID"].ToString();
                Session["myDeviceName"] = myDeviceName;
                TellCommand = "Tell Traveler Security flagsAdd lock " + myDeviceName + " " + myUserName;
                Session["TellCommand"] = TellCommand;
            }
            catch (Exception)
            {
                myUserName = "";
            }

            msgPopupControl.ShowOnPageLoad = true;
            msgLabel.Text = "Are you sure you want to deny access to " + myUserName + " on device " + myDeviceName + "?";

        }
        public void ClearWipe()
        {
            string TellCommand = "";
            string myUserName = "";
            string myDeviceName = "";
            string myServerName = "";
            string myDeviceTitle = "";
            DataRow myRow = UsersGrid.GetDataRow(UsersGrid.FocusedRowIndex);
            try
            {

                myUserName = myRow["UserName"].ToString();
                Session["myUserName"] = myUserName;
                myServerName = myRow["ServerName"].ToString();
                Session["myServerName"] = myServerName;
                myDeviceName = myRow["DeviceID"].ToString();
                //  Session["myDeviceName"] = myDeviceName;
                myDeviceTitle = myRow["DeviceID"].ToString();
                // Session["myDeviceTitle"] = myDeviceName;
                TellCommand = "Tell Traveler Security flagsRemove all " + myDeviceName + " " + myUserName;
                Session["TellCommand"] = TellCommand;
            }
            catch (Exception)
            {
                myUserName = "";
            }
            msgPopupControl.ShowOnPageLoad = true;
            msgLabel.Text = "Would you like to try to restore access for " + myUserName + " on device " + myDeviceTitle + "?" + "\n" + "Note that this will ONLY work if the device has not already been wiped.";
        }
        public void changeDeny()
        {

            string TellCommand = "";
            string myUserName = "";
            string myDeviceName = "";
            string myServerName = "";

            DataRow myRow = UsersGrid.GetDataRow(UsersGrid.FocusedRowIndex);
            try
            {

                myUserName = myRow["UserName"].ToString();
                // Session["myUserName"] = myUserName;
                myServerName = myRow["ServerName"].ToString();
                // Session["myServerName"] = myServerName;
                myDeviceName = myRow["DeviceID"].ToString();
                // Session["myDeviceName"] = myDeviceName;
                TellCommand = "Tell Traveler Security approval deny " + myDeviceName + " " + myUserName;
                // Session["TellCommand"] = TellCommand;
                VSWebBL.DashboardBL.LotusTravelerHealthBLL.Ins.SENDTravelerConsoleCommand(myServerName, TellCommand, myUserName);
            }
            catch (Exception)
            {
                myUserName = "";
            }


        }
        public void ChangeApprove()
        {

            string TellCommand = "";
            string myUserName = "";
            string myDeviceName = "";
            string myServerName = "";

            DataRow myRow = UsersGrid.GetDataRow(UsersGrid.FocusedRowIndex);
            try
            {

                myUserName = myRow["UserName"].ToString();
                // Session["myUserName"] = myUserName;
                myServerName = myRow["ServerName"].ToString();
                // Session["myServerName"] = myServerName;
                myDeviceName = myRow["DeviceID"].ToString();
                // Session["myDeviceName"] = myDeviceName;
                TellCommand = "Tell Traveler Security approval approve " + myDeviceName + " " + myUserName;
                // Session["TellCommand"] = TellCommand;
                VSWebBL.DashboardBL.LotusTravelerHealthBLL.Ins.SENDTravelerConsoleCommand(myServerName, TellCommand, myUserName);
            }
            catch (Exception)
            {
                myUserName = "";
            }


        }
        public void LogDisable()
        {
            string TellCommand = "";
            string myUserName = "";
            string myDeviceName = "";
            string myServerName = "";

            DataRow myRow = UsersGrid.GetDataRow(UsersGrid.FocusedRowIndex);
            try
            {

                myUserName = myRow["UserName"].ToString();
                Session["myUserName"] = myUserName;
                myServerName = myRow["ServerName"].ToString();
                Session["myServerName"] = myServerName;
                myDeviceName = myRow["DeviceID"].ToString();
                // Session["myDeviceName"] = myDeviceName;
                TellCommand = "Tell Traveler AddUser Finest " + myUserName;
                Session["TellCommand"] = TellCommand;
            }
            catch (Exception)
            {
                myUserName = "";
            }

            msgPopupControl.ShowOnPageLoad = true;
            msgLabel.Text = "Would you like to disable finest logging level for " + myUserName + "?";


        }

        public void LogEnable()
        {
            string TellCommand = "";
            string myUserName = "";
            string myDeviceName = "";
            string myServerName = "";

            DataRow myRow = UsersGrid.GetDataRow(UsersGrid.FocusedRowIndex);
            try
            {

                myUserName = myRow["UserName"].ToString();
                Session["myUserName"] = myUserName;
                myServerName = myRow["ServerName"].ToString();
                Session["myServerName"] = myServerName;
                myDeviceName = myRow["DeviceID"].ToString();
                // Session["myDeviceName"] = myDeviceName;
                TellCommand = "Tell Traveler Log RemoveUser  " + myUserName;
                Session["TellCommand"] = TellCommand;
            }
            catch (Exception)
            {
                myUserName = "";
            }

            msgPopupControl.ShowOnPageLoad = true;
            msgLabel.Text = "Would you like to enable finest logging level for " + myUserName + "?";
        }

        public void LogCreate()
        {
            string TellCommand = "";
            string myUserName = "";
            string myDeviceName = "";
            string myServerName = "";

            DataRow myRow = UsersGrid.GetDataRow(UsersGrid.FocusedRowIndex);
            try
            {

                myUserName = myRow["UserName"].ToString();
                Session["myUserName"] = myUserName;
                myServerName = myRow["ServerName"].ToString();
                Session["myServerName"] = myServerName;
                myDeviceName = myRow["DeviceID"].ToString();
                Session["myDeviceName"] = myDeviceName;
                TellCommand = "Tell Traveler Dump  " + myUserName;
                Session["TellCommand"] = TellCommand;
            }
            catch (Exception)
            {
                myUserName = "";
            }

            msgPopupControl.ShowOnPageLoad = true;
            msgLabel.Text = "Would you like to create a dump file for " + myUserName + "?";


        }
 
        protected void YesButton_Click(object sender, EventArgs e)
        {
            // StatusText.Text = "Denying access to " & myUserName
            if (Session["MenuItem"] == "Deny")
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
            if (Session["MenuItem"] == "Clearwipe")
            {
                if (Session["myUserName"] != "" && Session["myUserName"] != null)
                {
                    msgLabel.Text = "Restoring access for " + Session["myUserName"].ToString() + ".";
                    if (Session["myServerName"] != "" && Session["TellCommand"] != "")
                        VSWebBL.DashboardBL.LotusTravelerHealthBLL.Ins.SENDTravelerConsoleCommand(Session["myServerName"].ToString(), Session["TellCommand"].ToString(), Session["myUserName"].ToString());
                }
                YesButton.Visible = false;
                NOButton.Visible = false;
                CancelButton.Visible = true;
                CancelButton.Text = "OK";
                Session["myUserName"] = "";
                Session["myServerName"] = "";
                Session["myDeviceName"] = "";
                Session["TellCommand"] = "";

            }
            if (Session["MenuItem"] == "LogDisable" || Session["MenuItem"] == "LogEnable")
            {
                if (Session["myUserName"] != "" && Session["myUserName"] != null)
                {
                    msgLabel.Text = "Updating Log Level for " + Session["myUserName"].ToString() + ".";
                    if (Session["myServerName"] != "" && Session["TellCommand"] != "")
                        VSWebBL.DashboardBL.LotusTravelerHealthBLL.Ins.SENDTravelerConsoleCommand(Session["myServerName"].ToString(), Session["TellCommand"].ToString(), Session["myUserName"].ToString());
                    YesButton.Visible = false;
                    NOButton.Visible = false;
                    CancelButton.Visible = true;
                    CancelButton.Text = "OK";
                }
                Session["myUserName"] = "";
                Session["myServerName"] = "";
                Session["myDeviceName"] = "";
                Session["TellCommand"] = "";

            }
            if (Session["MenuItem"] == "LogCreate")
            {
                if (Session["myServerName"] != "" && Session["myServerName"] != null)
                    msgLabel.Text = "Created a text file on the server in the \\data\\ibm_technical_support\traveler\\logs\\dumps\\" + Session["myUserName"].ToString() + ".timedate.log file.";
                VSWebBL.DashboardBL.LotusTravelerHealthBLL.Ins.SENDTravelerConsoleCommand(Session["myServerName"].ToString(), Session["TellCommand"].ToString(), Session["myUserName"].ToString());
                YesButton.Visible = false;
                NOButton.Visible = false;
                CancelButton.Visible = true;
                CancelButton.Text = "OK";
                Session["myUserName"] = "";
                Session["myServerName"] = "";
                Session["myDeviceName"] = "";
                Session["TellCommand"] = "";

            }

        }
        
        protected void NOButton_Click(object sender, EventArgs e)
        {
            if (Session["myUserName"] != "" && Session["myUserName"] != null)
                msgLabel.Text = "No changes made to the user account " + Session["myUserName"].ToString() + ".";
            YesButton.Visible = false;
            NOButton.Visible = false;
            CancelButton.Visible = true;
            CancelButton.Text = "OK";
            Session["myUserName"] = "";
            Session["myServerName"] = "";
            Session["myDeviceName"] = "";
            Session["TellCommand"] = "";

        }
        
        protected void CancelButton_Click(object sender, EventArgs e)
        {
            msgPopupControl.ShowOnPageLoad = false;
        }
        
        protected void WipeCancelButton_Click(object sender, EventArgs e)
        {
            WipePopupControl.ShowOnPageLoad = false;
        }
        
        protected void WipeButton_Click(object sender, EventArgs e)
        {
            string tellCommand = "";
            string flags = "";
            if (HardCheckBox.Checked == false && TravelerAppCheckBox.Checked == false && StorageCadrCheckBox.Checked == false)
            {
                msgPopupControl.ShowOnPageLoad = true;
                msgLabel.Text = "Please select a wipe option:";
                YesButton.Visible = false;
                NOButton.Visible = false;
                CancelButton.Visible = true;
                CancelButton.Text = "OK";
            }
            if (TravelerAppCheckBox.Checked == true && HardCheckBox.Checked == false)
            {
                flags = "wipeApps ";
            }
            if (TravelerAppCheckBox.Checked == false && HardCheckBox.Checked == true && StorageCadrCheckBox.Checked == false)
            {
                flags = "wipeDevice ";
            }
            if (TravelerAppCheckBox.Checked == true && HardCheckBox.Checked == false && StorageCadrCheckBox.Checked == true)
            {
                flags = "wipeApps/wipeStorageCard";
            }
            if (TravelerAppCheckBox.Checked == true && HardCheckBox.Checked == true)
            {
                flags = "wipeDevice/wipeApps";
            }
            if (StorageCadrCheckBox.Checked == true && HardCheckBox.Checked == true)
            {
                flags = "wipeDevice/wipeStorageCard";
            }
            if (TravelerAppCheckBox.Checked == false && HardCheckBox.Checked == false && StorageCadrCheckBox.Checked == true)
            {
                flags = "wipeStorageCard ";
            }
            if (TravelerAppCheckBox.Checked == true && HardCheckBox.Checked == true && StorageCadrCheckBox.Checked == true)
            {
                flags = "wipeDevice/wipeApps/wipeStorageCard";
            }
            tellCommand += flags + DeviceIDLabel.Text + " " + UserNameLabel.Text;
            if (Session["myServerName"] != "" && Session["myServerName"] != null)
                VSWebBL.DashboardBL.LotusTravelerHealthBLL.Ins.SENDTravelerConsoleCommand(Session["myServerName"].ToString(), tellCommand, UserNameLabel.Text);
            WipePopupControl.ShowOnPageLoad = false;
            Session["myServerName"] = "";
        }
         
        protected void TravelerTrackBar_PositionChanged(object sender, EventArgs e)
        {
            if (travelerButtonList.Items[1].Selected == true)
            {
                travelerButtonList.Items[1].Text = "Show Devices that HAVE synchronized within the last " + TravelerTrackBar.Value + " minutes";
                travelerButtonList.Items[2].Text = "Show Devices that have NOT synchronized within the last " + TravelerTrackBar.Value + " minutes";
                trackLabel.Text = TravelerTrackBar.Value + " minutes";
                SetGrid(Convert.ToInt32(TravelerTrackBar.Value), 0);
            }
            if (travelerButtonList.Items[2].Selected == true)
            {
                travelerButtonList.Items[1].Text = "Show Devices that HAVE synchronized within the last " + TravelerTrackBar.Value + " minutes";
                travelerButtonList.Items[2].Text = "Show Devices that have NOT synchronized within the last " + TravelerTrackBar.Value + " minutes";
                trackLabel.Text = TravelerTrackBar.Value + " minutes";
                SetGrid(0, Convert.ToInt32(TravelerTrackBar.Value));
            }
        } 
         */












        protected void UsersGrid_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            if (e.DataColumn.FieldName == "Security_Policy" && e.CellValue.ToString() == "No policy")
            {
                e.Cell.BackColor = System.Drawing.Color.Yellow;
                e.Cell.ForeColor = System.Drawing.Color.Black;
            }
            else if (e.DataColumn.FieldName == "Security_Policy" && e.CellValue.ToString() == "Compliant")
            {
                e.Cell.BackColor = System.Drawing.Color.LightGreen;
                e.Cell.ForeColor = System.Drawing.Color.Black;
            }
            else if (e.DataColumn.FieldName == "Security_Policy" && e.CellValue.ToString() == "Not Compliant")
            {
                e.Cell.BackColor = System.Drawing.Color.Red;
                e.Cell.ForeColor = System.Drawing.Color.White;
            }
            else if (e.DataColumn.FieldName == "Security_Policy")
            {
                e.Cell.BackColor = System.Drawing.Color.LightGreen;
                e.Cell.ForeColor = System.Drawing.Color.Black;
            }



            if (e.DataColumn.FieldName == "Access" && e.CellValue.ToString() == "Allow")
            {
                e.Cell.BackColor = System.Drawing.Color.LightGreen;
                e.Cell.ForeColor = System.Drawing.Color.Black;
            }
            else if (e.DataColumn.FieldName == "Access")
            {
                e.Cell.BackColor = System.Drawing.Color.Red;
                e.Cell.ForeColor = System.Drawing.Color.White;
            }

            if (e.DataColumn.FieldName == "UserName")
            {
                string Uname = e.CellValue.ToString();
                Uname = Uname.Replace("CN=", " ");
                Uname = Uname.Replace("O=", " ");
                Uname = Uname.Replace("OU=", " ");
                e.Cell.Text = Uname;
            }

        }

        //3/28/2014 NS commented out for
        /*
        protected void ServerComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetGraphForDeviceType(ServerComboBox.Text);
        }
        protected void OSComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetGraphForDevice_OSType(OSComboBox.Text);
        }
        */


        protected void TravelGrid_HtmlDataCellPrepared(object sender, DevExpress.Web.ASPxGridViewTableDataCellEventArgs e)
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

        protected void TravelerGrid_CustomColumnDisplayText(object sender, DevExpress.Web.ASPxGridViewColumnDisplayTextEventArgs e)
        {
            if (e.Column.FieldName == "MailServerName")
            {
                //3/26/2015 NS added for DevEx upgrade to 14.2
                e.EncodeHtml = false;
                e.DisplayText = e.Value + "<br />" + "<span style=\"font-size:10px; font-weight:normal; color:black\">Location: " + e.GetFieldValue("Location") + "</span>";
            }

        }

        //3/28/2014 NS commented out for
        /*
        protected void UserDetailsMenu_ItemClick(object source, DevExpress.Web.MenuItemEventArgs e)
        {
            if (e.Item.Name == "DenyAccess")
            {
                if (UsersGrid.FocusedRowIndex > -1)
                {
                    YesButton.Visible = true;
                    NOButton.Visible = true;
                    CancelButton.Visible = false;
                    // CancelButton.Text = "Cancel";
                    Session["MenuItem"] = "Deny";
                    DenyAccess();
                }
                else
                {
                    msgPopupControl.HeaderText = "Deny Access";
                    msgPopupControl.ShowOnPageLoad = true;
                    msgLabel.Text = "Please select a device in the grid.";
                    YesButton.Visible = false;
                    NOButton.Visible = false;
                    CancelButton.Visible = true;
                    CancelButton.Text = "OK";
                }
            }
            if (e.Item.Name == "WipeDevice")
            {
                if (UsersGrid.FocusedRowIndex > -1)
                {
                    YesButton.Visible = true;
                    NOButton.Visible = true;
                    CancelButton.Text = "Cancel";
                    Session["MenuItem"] = "wipe";

                    //  WipeDevice Form Code 
                    WipeDevice();

                }
                else
                {
                    msgPopupControl.HeaderText = "Wipe Device";
                    msgLabel.Text = "Please select a device in the grid.";
                    msgPopupControl.ShowOnPageLoad = true;
                    YesButton.Visible = false;
                    NOButton.Visible = false;
                    CancelButton.Visible = true;
                    CancelButton.Text = "OK";
                }


            }
            if (e.Item.Name == "ClearWipeRequest")
            {
                if (UsersGrid.FocusedRowIndex > -1)
                {
                    YesButton.Visible = true;
                    NOButton.Visible = true;
                    // CancelButton.Text = "Cancel";
                    CancelButton.Visible = false;
                    Session["MenuItem"] = "Clearwipe";
                    ClearWipe();
                }
                else
                {
                    msgPopupControl.HeaderText = "Clear Wipe Request";
                    msgLabel.Text = "Please select a device in the grid.";
                    msgPopupControl.ShowOnPageLoad = true;
                    YesButton.Visible = false;
                    NOButton.Visible = false;
                    CancelButton.Visible = true;
                    CancelButton.Text = "OK";
                }

            }
            if (e.Item.Name == "ChangeApproval-Deny")
            {
                if (UsersGrid.FocusedRowIndex > -1)
                {
                    YesButton.Visible = true;
                    NOButton.Visible = true;
                    CancelButton.Visible = false;
                    //CancelButton.Text = "Cancel";
                    Session["MenuItem"] = "changeDeny";
                    changeDeny();
                }
                else
                {
                    msgPopupControl.HeaderText = "Change Approval - Deny";
                    msgLabel.Text = "Please select a device in the grid.";
                    msgPopupControl.ShowOnPageLoad = true;
                    YesButton.Visible = false;
                    NOButton.Visible = false;
                    CancelButton.Visible = true;
                    CancelButton.Text = "OK";
                }


            }
            if (e.Item.Name == "ChangeApproval-Approve")
            {
                if (UsersGrid.FocusedRowIndex > -1)
                {
                    YesButton.Visible = true;
                    NOButton.Visible = true;
                    CancelButton.Visible = false;
                    //CancelButton.Text = "Cancel";
                    Session["MenuItem"] = "ChangeApprove";
                    ChangeApprove();
                }
                else
                {
                    msgPopupControl.HeaderText = "Change Approval - Approve";
                    msgLabel.Text = "Please select a device in the grid.";
                    msgPopupControl.ShowOnPageLoad = true;
                    YesButton.Visible = false;
                    NOButton.Visible = false;
                    CancelButton.Visible = true;
                    CancelButton.Text = "OK";
                }


            }
            if (e.Item.Name == "LogLevel-DisableFinest")
            {
                if (UsersGrid.FocusedRowIndex > -1)
                {
                    YesButton.Visible = true;
                    NOButton.Visible = true;
                    CancelButton.Visible = false;
                    // CancelButton.Text = "Cancel";
                    Session["MenuItem"] = "LogDisable";
                    LogDisable();
                }
                else
                {
                    msgPopupControl.HeaderText = "Log Level - Disable Finest";
                    msgLabel.Text = "Please select a device in the grid.";
                    msgPopupControl.ShowOnPageLoad = true;
                    YesButton.Visible = false;
                    NOButton.Visible = false;
                    CancelButton.Visible = true;
                    CancelButton.Text = "OK";
                }

            }
            if (e.Item.Name == "LogLevel-EnableFinest")
            {
                if (UsersGrid.FocusedRowIndex > -1)
                {
                    YesButton.Visible = true;
                    NOButton.Visible = true;
                    CancelButton.Visible = false;
                    //CancelButton.Text = "Cancel";
                    Session["MenuItem"] = "LogEnable";
                    LogEnable();
                }
                else
                {
                    msgPopupControl.HeaderText = "Log Level - Enable Finest";
                    msgLabel.Text = "Please select a device in the grid.";
                    msgPopupControl.ShowOnPageLoad = true;
                    YesButton.Visible = false;
                    NOButton.Visible = false;
                    CancelButton.Visible = true;
                    CancelButton.Text = "OK";
                }

            }
            if (e.Item.Name == "LogLevel-CreateDumpFile")
            {
                if (UsersGrid.FocusedRowIndex > -1)
                {
                    YesButton.Visible = true;
                    NOButton.Visible = true;
                    CancelButton.Visible = false;
                    // CancelButton.Text = "Cancel";
                    Session["MenuItem"] = "LogCreate";
                    LogCreate();
                }
                else
                {
                    msgPopupControl.HeaderText = "Log Level - Create Dump File";
                    msgLabel.Text = "Please select a device in the grid.";
                    msgPopupControl.ShowOnPageLoad = true;
                    YesButton.Visible = false;
                    NOButton.Visible = false;
                    CancelButton.Visible = true;
                    CancelButton.Text = "OK";
                }

            }
        } 
         */


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
						else if (e.CellValue.ToString()=="")
                        {
							e.Cell.BackColor = System.Drawing.Color.Empty;
							e.Cell.ForeColor = System.Drawing.Color.Empty;
                        }
                        //6/9/2016 NS added for VSPLUS-2973
                        else if (e.CellValue.ToString().ToUpper() == "FAIL")
                        {
                            e.Cell.BackColor = System.Drawing.Color.Red;
                            e.Cell.ForeColor = System.Drawing.Color.White;
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
                //5/20/2016 NS modified for VSPLUS-2973
                else if (e.CellValue.ToString() == "Not Responding")
                {
                    e.Cell.BackColor = System.Drawing.Color.Red;
                    e.Cell.ForeColor = System.Drawing.Color.White;
                }
				else if (e.DataColumn.FieldName == "TravelerServlet" && e.CellValue.ToString()=="")
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

        //3/28/2014 NS commented out for
        /*
        protected void deviceTypeWebChart_CustomCallback(object sender, DevExpress.XtraCharts.Web.CustomCallbackEventArgs e)
        {
            this.deviceTypeWebChart.Width = new Unit(Convert.ToInt32(chartWidth.Value));
        }

        protected void OSTypeWebChartControl_CustomCallback(object sender, DevExpress.XtraCharts.Web.CustomCallbackEventArgs e)
        {
            this.OSTypeWebChartControl.Width = new Unit(Convert.ToInt32(chartWidth.Value));
        }
        */

        protected void Timer1_Tick(object sender, EventArgs e)
        {
            //8/25/2015 NS modified for VSPLUS-2096
            //TravelerGrid.DataSource = SetGridForTravelerInterval(value);
            //TravelerGrid.DataBind();
        }

        //3/28/2014 NS commented out for
        /*
        public void UpdateMenuVisibility()
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
                UserDetailsMenu.Visible = true;
            }
        }
        */

        protected void ASPxCallbackPanel2_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
        {
            //11/19/2014 NS modified
            //MailServerlbl.Text = value + "'" + "s access to mail servers";
            //8/25/2015 NS modified for VSPLUS-2096
            //MailServerlbl.InnerHtml = value + "'" + "s access to mail servers";
            SetGridForTravelerInterval(value);
        }

        protected void mailServerListComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            //8/25/2015 NS modified for VSPLUS-2096
            //RecalulateGraphs();
        }

        protected void travelerIntervalComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            //8/25/2015 NS modified for VSPLUS-2096
            //RecalulateGraphs();
        }
        //8/25/2015 NS modified for VSPLUS-2096
        /*
        public void RecalulateGraphs()
        {
            object servername;
            string mailservername = "";
            string interval = "";

            servername = grid.GetRowValues(grid.FocusedRowIndex, new string[] { "Name" });
            mailservername = mailServerListComboBox.SelectedItem.Value.ToString();
            interval = travelerIntervalComboBox.SelectedItem.Value.ToString();
            mailFileOpensRoundPanel.HeaderText = "Cumulative Mail File Open Times - " + mailservername;
            mailFileOpensDeltaRoundPanel.HeaderText = "Mail File Open Times Delta - " + mailservername;
            //8/19/2014 NS commented out for VSPLUS-884
            //httpSessionsASPxRoundPanel.HeaderText = "HTTP Sessions - " + mailservername;
            SetGraphForMailFileOpens(servername.ToString(), mailservername, interval);
            SetGraphForMailFileOpensCumulative(servername.ToString(), mailservername, interval);
            //8/19/2014 NS commented out for VSPLUS-884
            //SetGraphForHttpSessions(httpSessionsASPxRadioButtonList.Value.ToString(), servername.ToString());
        }
         */
        protected void grid_PageSizeChanged(object sender, EventArgs e)
        {
            //ProfilesGridView.PageIndex;
            VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("TravelerUsersDevicesOS|grid", grid.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        }

        //8/25/2015 NS modified for VSPLUS-2096
        /*
        protected void TravelerGrid_PageSizeChanged(object sender, EventArgs e)
        {
            //ProfilesGridView.PageIndex;
            VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("TravelerUsersDevicesOS|TravelerGrid", TravelerGrid.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        }
         */

        //3/28/2014 NS commented out
        /*
        protected void UsersGrid_PageSizeChanged(object sender, EventArgs e)
        {
            //ProfilesGridView.PageIndex;
            VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("TravelerUsersDevicesOS|UsersGrid", UsersGrid.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        }
         */
        protected void ServerPopupMenu_ItemClick(object source, DevExpress.Web.MenuItemEventArgs e)
        {
            //08/3/2016 Sowmya comment for VSPLUS 2692
            ////4/21/2014 MD added
            //string servername = "";
            //string servertype = "";
            //errorDiv.Style.Value = "display: none;";
            //if (e.Item.Name == "ScanNow")
            //{
            //    if (grid.FocusedRowIndex > -1)
            //    {
            //        try
            //        {
            //            Status StatusObj = new Status();
            //            DataRow myRow = grid.GetDataRow(grid.FocusedRowIndex);
            //            try
            //            {
            //                servername = myRow["Name"].ToString();
            //                if (servername != "" && servername != null)
            //                {
            //                    StatusObj.Name = servername;
            //                }
            //                servertype = myRow["type"].ToString();
            //                if (servertype != "" && servertype != null)
            //                {
            //                    StatusObj.Type = servertype;
            //                }
            //                bool bl = VSWebBL.StatusBL.StatusTBL.Ins.UpdateforScan(StatusObj);
            //                //7/7/2015 NS modified for VSPLUS-1949
            //                //bl = VSWebBL.SettingBL.SettingsBL.Ins.UpdateSvalue("ScanDominoASAP", StatusObj.Name, VSWeb.Constants.Constants.SysString);
            //                bl = VSWebBL.SettingBL.SettingsBL.Ins.UpdateScanvalue("ScanDominoASAP", StatusObj.Name, VSWeb.Constants.Constants.SysString);
            //            }
            //            catch (Exception ex)
            //            {
            //                errorDiv.Style.Value = "display: block;";
            //                //10/3/2014 NS modified for VSPLUS-990
            //                errorDiv.InnerHtml = "The following error has occurred while trying to scan: " + ex.Message +
            //                    "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
            //                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
            //                throw ex;
            //            }
            //        }
            //        catch (Exception ex)
            //        {
            //            errorDiv.Style.Value = "display: block;";
            //            //10/3/2014 NS modified for VSPLUS-990
            //            errorDiv.InnerHtml = "The following error has occurred while trying to scan: " + ex.Message +
            //                "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
            //            Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
            //            throw ex;
            //            //throw;
            //        }
            //    }
            //    else
            //    {
            //        errorDiv.Style.Value = "display: block;";
            //    }
            //}
            //else if (e.Item.Name == "EditInConfig")
            //{
            //    if (grid.FocusedRowIndex > -1)
            //    {
            //        string id = "";
            //        Session["Submenu"] = "LotusDomino";
            //        DataRow myRow = grid.GetDataRow(grid.FocusedRowIndex);
            //        try
            //        {
            //            servername = myRow["Name"].ToString();
            //            if (servername != "" && servername != null)
            //            {
            //                id = (VSWebBL.SecurityBL.ServersBL.Ins.GetServerIDbyServerName(servername)).ToString();
            //                Response.Redirect("~/Configurator/DominoProperties.aspx?Key=" + id, false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
            //                Context.ApplicationInstance.CompleteRequest();
            //            }
            //        }
            //        catch (Exception ex)
            //        {
            //            errorDiv.Style.Value = "display: block;";
            //            //10/3/2014 NS modified for VSPLUS-990
            //            errorDiv.InnerHtml = "The following error has occurred while trying to scan: " + ex.Message+
            //                "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
            //            Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
            //            throw ex;
            //        }
            //    }
            //    else
            //    {
            //        errorDiv.Style.Value = "display: block;";
            //    }
            //}
        }

        //8/19/2014 NS added for VSPLUS-884
        protected void DeviceSyncWebchart_CustomCallback(object sender, DevExpress.XtraCharts.Web.CustomCallbackEventArgs e)
        {
            //8/25/2015 NS modified for VSPLUS-2096
            //this.DeviceSyncsWebChart.Width = new Unit(Convert.ToInt32(chartWidth.Value)/2);
        }

        //8/25/2015 NS modified for VSPLUS-2096
        /*
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
            seriesXY.AxisX.DateTimeMeasureUnit = DateTimeMeasurementUnit.Minute;
            seriesXY.AxisX.DateTimeOptions.Format = DateTimeFormat.ShortTime;

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
        }
         */

		protected void hfNameLabel_Load(object sender, EventArgs e)
		{
			Label StatusLabel = (Label)sender;

			if (StatusLabel.Text == "Red")
				StatusLabel.ForeColor = System.Drawing.Color.White;
			else
				StatusLabel.ForeColor = System.Drawing.Color.Black;
		}

        protected void mailFileOpensWebChart_BoundDataChanged(object sender, EventArgs e)
        {
            UI uiobj = new UI();
            uiobj.RecalibrateChartAxes(mailFileOpensWebChart.Diagram);
        }

        protected void mailFileOpensCumulativeWebChart_BoundDataChanged(object sender, EventArgs e)
        {
            UI uiobj = new UI();
            uiobj.RecalibrateChartAxes(mailFileOpensCumulativeWebChart.Diagram);
        }

        //8/25/2015 NS added for VSPLUS-2096
        protected void grid_SelectionChanged(object sender, EventArgs e)
        {
            if (grid.Selection.Count > 0)
            {
                System.Collections.Generic.List<object> Name = grid.GetSelectedFieldValues("Name");
                System.Collections.Generic.List<object> Status = grid.GetSelectedFieldValues("Status");
                System.Collections.Generic.List<object> HeartBeat = grid.GetSelectedFieldValues("HeartBeat");
                if (Name.Count > 0)
                {
                    //DevExpress.Web.ASPxWebControl.RedirectOnCallback("DominoServerDetailsPage2.aspx?Name=" + Name[0].ToString() + "&Type=Traveler" + "&Status=" + Status[0].ToString() + "&LastDate=" + HeartBeat[0].ToString() + "");
                    Response.Redirect("~/Dashboard/DominoServerDetailsPage2.aspx?Name=" + Name[0].ToString() + "&Type=Traveler" + "&Status=" + Status[0].ToString() + "&LastDate=" + HeartBeat[0].ToString() + "", false);
                    Context.ApplicationInstance.CompleteRequest();
                }
            }
        }

        //8/25/2015 NS modified for VSPLUS-2096
        /*
        protected void httpSessionsWebChart_BoundDataChanged(object sender, EventArgs e)
        {
            UI uiobj = new UI();
            uiobj.RecalibrateChartAxes(httpSessionsWebChart.Diagram);
        }
		*/
    
    }
}