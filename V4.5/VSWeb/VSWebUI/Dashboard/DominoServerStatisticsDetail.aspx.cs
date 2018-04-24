using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DevExpress.XtraCharts;
using System.Collections;
using System.Drawing;

namespace VSWebUI.Configurator
{
    public partial class DominoServerStatisticsDetail : System.Web.UI.Page
    {
		protected void Page_PreInit(object sender, EventArgs e)
		{

			this.Master.contentCallEvent += new EventHandler(Master_ButtonClick);
		}

		private void Master_ButtonClick(object sender, EventArgs e)
		{

		}
        string name = "";
        string sType = "";
        Series series = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            
            Page.Title = "Domino Server Statistic Details";
            if (!IsPostBack)
            {
                if (Request.QueryString["T"] != "" && Request.QueryString["T"] != null)
                {
                    sType = Request.QueryString["T"].ToString();
                }
                if (Request.QueryString["server"] != "" && Request.QueryString["server"] != null)
                {
                    ServerLabel.Text = Request.QueryString["server"].ToString();
                }
                else
                {
                    ServerLabel.Text="azphxdom1/RPRWyatt";
                }          
                Populate_hrASPxCombo();
                Populate_dayASPxCombo();
                Populate_monthASPxCombo();
            }
               // SetGraph("hh", ServerLabel.Text);
                if (Request.QueryString["name"] != "" && Request.QueryString["name"] != null)
                {
                    name = Request.QueryString["name"].ToString();
                }
                if (name == "Cpu")
                {
                    statisticroundpanel.HeaderText = "CPU";
                }
                if (name == "Mem")
                {
                    statisticroundpanel.HeaderText = "Memory";
                }
                if (name == "Perfm")
                {
                    statisticroundpanel.HeaderText = "Performance";
                }
            
        }

        public void Populate_hrASPxCombo()
        {
            hrASPxCombo.Items.Add("--Compare to--");
            hrASPxCombo.Items.Add("00:00 - 01:00", "0");
            hrASPxCombo.Items.Add("01:00 - 02:00", "01");
            hrASPxCombo.Items.Add("02:00 - 03:00", "02");
            hrASPxCombo.Items.Add("03:00 - 04:00", "03");
            hrASPxCombo.Items.Add("04:00 - 05:00", "04");
            hrASPxCombo.Items.Add("05:00 - 06:00", "05");
            hrASPxCombo.Items.Add("06:00 - 07:00", "06");
            hrASPxCombo.Items.Add("07:00 - 08:00", "07");
            hrASPxCombo.Items.Add("08:00 - 09:00", "08");
            hrASPxCombo.Items.Add("09:00 - 10:00", "09");
            hrASPxCombo.Items.Add("10:00 - 11:00", "10");
            hrASPxCombo.Items.Add("11:00 - 12:00", "11");
            hrASPxCombo.Items.Add("12:00 - 13:00", "12");
            hrASPxCombo.Items.Add("13:00 - 14:00", "13");
            hrASPxCombo.Items.Add("14:00 - 15:00", "14");
            hrASPxCombo.Items.Add("15:00 - 16:00", "15");
            hrASPxCombo.Items.Add("16:00 - 17:00", "16");
            hrASPxCombo.Items.Add("17:00 - 18:00", "17");
            hrASPxCombo.Items.Add("18:00 - 19:00", "18");
            hrASPxCombo.Items.Add("19:00 - 20:00", "19");
            hrASPxCombo.Items.Add("20:00 - 21:00", "20");
            hrASPxCombo.Items.Add("21:00 - 22:00", "21");
            hrASPxCombo.Items.Add("22:00 - 23:00", "22");
            hrASPxCombo.Items.Add("23:00 - 24:00", "23");

            hrASPxCombo.SelectedIndex = 0;
        }

        public void Populate_dayASPxCombo()
        {
            dayASPxCombo.Items.Add("--Compare With--");
            dayASPxCombo.Items.Add("Yesterday");
            dayASPxCombo.Items.Add("Last Week/Same Day");
            dayASPxCombo.Items.Add("2 Days Ago");
            dayASPxCombo.Items.Add("3 Days Ago");
            dayASPxCombo.Items.Add("4 Days Ago");
            dayASPxCombo.Items.Add("5 Days Ago");
            dayASPxCombo.Items.Add("6 Days Ago");
            dayASPxCombo.Items.Add("7 Days Ago");
            dayASPxCombo.Items.Add("8 Days Ago");
            dayASPxCombo.Items.Add("9 Days Ago");
            dayASPxCombo.Items.Add("10 Days Ago");
            dayASPxCombo.Items.Add("11 Days Ago");
            dayASPxCombo.Items.Add("12 Days Ago");
            dayASPxCombo.Items.Add("13 Days Ago");
            dayASPxCombo.Items.Add("14 Days Ago");
            dayASPxCombo.Items.Add("15 Days Ago");
            dayASPxCombo.Items.Add("16 Days Ago");
            dayASPxCombo.Items.Add("17 Days Ago");
            dayASPxCombo.Items.Add("18 Days Ago");
            dayASPxCombo.Items.Add("19 Days Ago");
            dayASPxCombo.Items.Add("20 Days Ago");
            dayASPxCombo.Items.Add("21 Days Ago");
            dayASPxCombo.Items.Add("22 Days Ago");
            dayASPxCombo.Items.Add("23 Days Ago");
            dayASPxCombo.Items.Add("24 Days Ago");
            dayASPxCombo.Items.Add("25 Days Ago");
            dayASPxCombo.Items.Add("26 Days Ago");
            dayASPxCombo.Items.Add("27 Days Ago");
            dayASPxCombo.Items.Add("28 Days Ago");
            dayASPxCombo.Items.Add("29 Days Ago");
            dayASPxCombo.Items.Add("30 Days Ago");
                     
            hrASPxCombo.SelectedIndex = 0;
            dayASPxCombo.SelectedIndex = 0;
        }

        public void Populate_monthASPxCombo()
        {
            monthASPxCombo.Items.Add("--Compare With--");
            monthASPxCombo.Items.Add("Last Month", "1");
            monthASPxCombo.Items.Add("2 Months Ago", "02");
            monthASPxCombo.Items.Add("3 Months Ago", "03");
            monthASPxCombo.Items.Add("4 Months Ago", "04");
            monthASPxCombo.Items.Add("5 Months Ago", "05");
            monthASPxCombo.SelectedIndex = 0;
        }

        public DataTable SetGraph(string paramValue, string servername)
        {
            DataTable dt = VSWebBL.DashboardBL.DominoServerStatisticsDetailBLL.Ins.SetGraph(paramValue, servername);       
                

            if (series == null)
            {
                series = new Series("DominoServer", ViewType.Line);
                series.Visible = true;
                series.ArgumentDataMember = dt.Columns["Date"].ToString();

                ValueDataMemberCollection seriesValueDataMembers = (ValueDataMemberCollection)series.ValueDataMembers;
                seriesValueDataMembers.AddRange(dt.Columns["StatValue"].ToString());
                dominoServerWebChart.Series.Add(series);

                ((XYDiagram)dominoServerWebChart.Diagram).PaneLayoutDirection = PaneLayoutDirection.Horizontal;

                XYDiagram seriesXY = (XYDiagram)dominoServerWebChart.Diagram;
                seriesXY.AxisX.Title.Text = "Time";
                seriesXY.AxisX.Title.Visible = true;
                seriesXY.AxisY.Title.Text = "Performance";
                seriesXY.AxisY.Title.Visible = true;

                dominoServerWebChart.Legend.Visible = false;

                //((SplineSeriesView)series.View).LineTensionPercent = 100;
                ((LineSeriesView)series.View).LineMarkerOptions.Size = 4;
                ((LineSeriesView)series.View).LineMarkerOptions.Color = Color.White;

                AxisBase axis = ((XYDiagram)dominoServerWebChart.Diagram).AxisX;
                //4/18/2014 NS commented out for VSPLUS-312
                //axis.DateTimeGridAlignment = DateTimeMeasurementUnit.Hour;
                axis.GridSpacingAuto = false;
                axis.MinorCount = 50;
                axis.GridSpacing = 5.0;
                axis.Range.SideMarginsEnabled = false;
                axis.GridLines.Visible = false;
                //axis.DateTimeOptions.Format = DateTimeFormat.Custom;
                //axis.DateTimeOptions.FormatString = "dd/MM HH:mm";

                ((LineSeriesView)series.View).Color = Color.Blue;

                AxisBase axisy = ((XYDiagram)dominoServerWebChart.Diagram).AxisY;
                axisy.Range.AlwaysShowZeroLevel = false;
                axisy.Range.SideMarginsEnabled = true;
                axisy.GridLines.Visible = false;
                dominoServerWebChart.DataSource = dt;
                dominoServerWebChart.DataBind();
            }
            return dt;
        }
        
        protected void hrASPxCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            //10/9/2013 NS added
            //dominoServerWebChart.Series.Clear();

            dayASPxCombo.SelectedIndex = 0;
            monthASPxCombo.SelectedIndex = 0;
             DataTable dt1=new DataTable();
             if (name == "Perfm")
             {
                 dt1 = VSWebBL.DashboardBL.DominoServerStatisticsDetailBLL.Ins.SetGraphForHrCombo("hh", ServerLabel.Text, hrASPxCombo.SelectedItem.Text.Substring(0, 2), hrASPxCombo.SelectedItem.Text.Substring(8, 2),"ResponseTime");
             }
             else if (name == "Cpu")
             {
                 dt1 = VSWebBL.DashboardBL.DominoServerStatisticsDetailBLL.Ins.SetGraphForHrCombo("hh", ServerLabel.Text, hrASPxCombo.SelectedItem.Text.Substring(0, 2), hrASPxCombo.SelectedItem.Text.Substring(8, 2), "Platform.System.PctCombinedCpuUtil");
             }
             else if (name == "Mem")
             {
                 dt1 = VSWebBL.DashboardBL.DominoServerStatisticsDetailBLL.Ins.SetGraphForHrCombo("hh", ServerLabel.Text, hrASPxCombo.SelectedItem.Text.Substring(0, 2), hrASPxCombo.SelectedItem.Text.Substring(8, 2), "Mem.PercentUsed");
             }

            dominoServerWebChart.SeriesDataMember = "Date";
            dominoServerWebChart.SeriesTemplate.ArgumentDataMember = "Time";
            dominoServerWebChart.SeriesTemplate.ValueDataMembers.AddRange(new string[] { "StatValue" });
            dominoServerWebChart.SeriesTemplate.View = new LineSeriesView();
            //10/9/2013 NS added
            LineSeriesView view = (LineSeriesView)dominoServerWebChart.SeriesTemplate.View;
            view.LineMarkerOptions.Visible = true;
            view.LineMarkerOptions.Size = 8;
            XYDiagram seriesXY = (XYDiagram)dominoServerWebChart.Diagram;
            seriesXY.AxisX.Title.Text = "Time";
            seriesXY.AxisX.Title.Visible = true;
            seriesXY.AxisX.Label.ResolveOverlappingOptions.AllowRotate = true;
            seriesXY.AxisX.Label.ResolveOverlappingOptions.AllowHide = false;
            //4/18/2014 NS commented out for VSPLUS-312
            //seriesXY.AxisX.DateTimeGridAlignment = DateTimeMeasurementUnit.Minute;
            seriesXY.AxisX.DateTimeMeasureUnit = DateTimeMeasurementUnit.Minute;
            seriesXY.AxisX.DateTimeOptions.Format = DateTimeFormat.ShortTime;

            if (name == "Perfm")
            {
                seriesXY.AxisY.Title.Text = "Response Time";
            }
            
            seriesXY.AxisY.Title.Visible = true;
            //10/7/2013 NS added
            seriesXY.AxisY.Label.ResolveOverlappingOptions.AllowRotate = false;
            seriesXY.AxisY.NumericOptions.Format = NumericFormat.Number;
            seriesXY.AxisY.NumericOptions.Precision = 0;
            seriesXY.AxisY.GridSpacingAuto = true;

            AxisBase axis = ((XYDiagram)dominoServerWebChart.Diagram).AxisX;
            //4/18/2014 NS commented out for VSPLUS-312
            //axis.DateTimeGridAlignment = DateTimeMeasurementUnit.Day;
            axis.GridSpacingAuto = true;
            /*
            axis.MinorCount = 15;
            axis.GridSpacing = 0.5;
             */
            axis.Range.SideMarginsEnabled = true;
            axis.GridLines.Visible = false;
            //axis.DateTimeOptions.Format = DateTimeFormat.Custom;
            //axis.DateTimeOptions.FormatString = "HH:mm";
            //4/18/2014 NS commented out for VSPLUS-312
            //axis.DateTimeGridAlignment = DateTimeMeasurementUnit.Minute;
            axis.DateTimeMeasureUnit = DateTimeMeasurementUnit.Minute;


            AxisBase axisy = ((XYDiagram)dominoServerWebChart.Diagram).AxisY;
            axisy.Range.AlwaysShowZeroLevel = false;
            axisy.Range.SideMarginsEnabled = true;
            axisy.GridLines.Visible = true;
            dominoServerWebChart.Legend.Visible = true;
            
            dominoServerWebChart.DataSource = dt1;
            dominoServerWebChart.DataBind();   
                    
                   //Series series1 = new Series(DateTime.Now.ToString("MM/dd/yyyy"), ViewType.Line);
                   //series1.Visible = true;
                   //series1.DataSource = dt2;
                   //series1.CrosshairEnabled = DevExpress.Utils.DefaultBoolean.True;
                   //series1.View.Color = Color.DarkRed;
                   //series1.ArgumentDataMember = dt2.Columns["Time"].ToString();

                   //ValueDataMemberCollection seriesValueDataMembers = (ValueDataMemberCollection)series1.ValueDataMembers;
                   // seriesValueDataMembers.AddRange(dt2.Columns["StatValue"].ToString());
                   // dominoServerWebChart.Series.Add(series1);                  
                     
                 
                   // ((LineSeriesView)series1.View).LineMarkerOptions.Size = 4;
                   // ((LineSeriesView)series1.View).LineMarkerOptions.Color = Color.White;

                   // AxisBase axis = ((XYDiagram)dominoServerWebChart.Diagram).AxisX;
                   // axis.DateTimeGridAlignment = DateTimeMeasurementUnit.Day;
                   // axis.GridSpacingAuto = false;
                   // axis.MinorCount = 15;
                   // axis.GridSpacing = 0.5;
                   // axis.Range.SideMarginsEnabled = false;
                   // axis.GridLines.Visible = false;
                   // axis.DateTimeOptions.Format = DateTimeFormat.Custom;

                   // AxisBase axisy = ((XYDiagram)dominoServerWebChart.Diagram).AxisY;
                   // axisy.Range.AlwaysShowZeroLevel = false;
                   // axisy.Range.SideMarginsEnabled = true;
                   // axisy.GridLines.Visible = false;
                   // dominoServerWebChart.Legend.Visible = true;
                   // dominoServerWebChart.DataBind();
        }

        protected void dayASPxCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            //dominoServerWebChart.Series.Clear();
            DataTable dt1 = new DataTable();

            if (name == "Perfm")
            {
                if (dayASPxCombo.SelectedIndex == 1)
                {
                    dt1 = VSWebBL.DashboardBL.DominoServerStatisticsDetailBLL.Ins.SetGraphForDayCombo("dd", ServerLabel.Text, "1", hrASPxCombo.SelectedItem.Text.Substring(0, 2), hrASPxCombo.SelectedItem.Text.Substring(8, 2),"ResponseTime");
                }
                else if (dayASPxCombo.SelectedIndex == 2)
                {
                    dt1 = VSWebBL.DashboardBL.DominoServerStatisticsDetailBLL.Ins.SetGraphForDayCombo("dd", ServerLabel.Text, "7", hrASPxCombo.SelectedItem.Text.Substring(0, 2), hrASPxCombo.SelectedItem.Text.Substring(8, 2), "ResponseTime");
                }
                else
                {
                    dt1 = VSWebBL.DashboardBL.DominoServerStatisticsDetailBLL.Ins.SetGraphForDayCombo("dd", ServerLabel.Text, dayASPxCombo.SelectedItem.Text.Substring(0, 2), hrASPxCombo.SelectedItem.Text.Substring(0, 2), hrASPxCombo.SelectedItem.Text.Substring(8, 2), "ResponseTime");
                }
            }
            else if (name == "Cpu")
            {
                if (dayASPxCombo.SelectedIndex == 1)
                {
                    dt1 = VSWebBL.DashboardBL.DominoServerStatisticsDetailBLL.Ins.SetGraphForDayCombo("dd", ServerLabel.Text, "1", hrASPxCombo.SelectedItem.Text.Substring(0, 2), hrASPxCombo.SelectedItem.Text.Substring(8, 2), "Platform.System.PctCombinedCpuUtil");
                }
                else if (dayASPxCombo.SelectedIndex == 2)
                {
                    dt1 = VSWebBL.DashboardBL.DominoServerStatisticsDetailBLL.Ins.SetGraphForDayCombo("dd", ServerLabel.Text, "7", hrASPxCombo.SelectedItem.Text.Substring(0, 2), hrASPxCombo.SelectedItem.Text.Substring(8, 2), "Platform.System.PctCombinedCpuUtil");
                }
                else
                {
                    dt1 = VSWebBL.DashboardBL.DominoServerStatisticsDetailBLL.Ins.SetGraphForDayCombo("dd", ServerLabel.Text, dayASPxCombo.SelectedItem.Text.Substring(0, 2), hrASPxCombo.SelectedItem.Text.Substring(0, 2), hrASPxCombo.SelectedItem.Text.Substring(8, 2), "Platform.System.PctCombinedCpuUtil");
                }
            }
            else if (name == "Mem")
            {
                if (dayASPxCombo.SelectedIndex == 1)
                {
                    dt1 = VSWebBL.DashboardBL.DominoServerStatisticsDetailBLL.Ins.SetGraphForDayCombo("dd", ServerLabel.Text, "1", hrASPxCombo.SelectedItem.Text.Substring(0, 2), hrASPxCombo.SelectedItem.Text.Substring(8, 2), "Mem.PercentUsed");
                }
                else if (dayASPxCombo.SelectedIndex == 2)
                {
                    dt1 = VSWebBL.DashboardBL.DominoServerStatisticsDetailBLL.Ins.SetGraphForDayCombo("dd", ServerLabel.Text, "7", hrASPxCombo.SelectedItem.Text.Substring(0, 2), hrASPxCombo.SelectedItem.Text.Substring(8, 2), "Mem.PercentUsed");
                }
                else
                {
                    dt1 = VSWebBL.DashboardBL.DominoServerStatisticsDetailBLL.Ins.SetGraphForDayCombo("dd", ServerLabel.Text, dayASPxCombo.SelectedItem.Text.Substring(0, 2), hrASPxCombo.SelectedItem.Text.Substring(0, 2), hrASPxCombo.SelectedItem.Text.Substring(8, 2), "Mem.PercentUsed");
                }
            }

          
         
            //dominoServerWebChart



            dominoServerWebChart.SeriesDataMember = "Date";
            dominoServerWebChart.SeriesTemplate.ArgumentDataMember = "Time";
            
            /*
            Series series = new Series(dt1.Rows[0]["Date"].ToString(), ViewType.Line);
            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                if (series != null)
                {
                    series.Points.Add(new SeriesPoint(dt1.Rows[i]["Date"].ToString(), Convert.ToDouble(dt1.Rows[i]["StatValue"].ToString())));
                }
            }
            dominoServerWebChart.Series.Add(series);
             */
            dominoServerWebChart.SeriesTemplate.ValueDataMembers.AddRange(new string[] { "StatValue" });
            dominoServerWebChart.SeriesTemplate.View = new LineSeriesView();
            //10/9/2013 NS added
            
            LineSeriesView view = (LineSeriesView)dominoServerWebChart.SeriesTemplate.View;
            view.LineMarkerOptions.Visible = true;
            view.LineMarkerOptions.Size = 8;
            XYDiagram seriesXY = (XYDiagram)dominoServerWebChart.Diagram;
            seriesXY.AxisX.Title.Text = "Time";
            seriesXY.AxisX.Title.Visible = true;
            seriesXY.AxisX.Label.ResolveOverlappingOptions.AllowRotate = true;
            seriesXY.AxisX.Label.ResolveOverlappingOptions.AllowHide = false;
            //4/18/2014 NS commented out for VSPLUS-312
            //seriesXY.AxisX.DateTimeGridAlignment = DateTimeMeasurementUnit.Minute;
            seriesXY.AxisX.DateTimeMeasureUnit = DateTimeMeasurementUnit.Minute;
            seriesXY.AxisX.DateTimeOptions.Format = DateTimeFormat.ShortTime;

            //10/7/2013 NS added
            seriesXY.AxisY.Label.ResolveOverlappingOptions.AllowRotate = false;
            seriesXY.AxisY.NumericOptions.Format = NumericFormat.Number;
            seriesXY.AxisY.NumericOptions.Precision = 0;
            seriesXY.AxisY.GridSpacingAuto = true;

            AxisBase axis = ((XYDiagram)dominoServerWebChart.Diagram).AxisX;
            //4/18/2014 NS commented out for VSPLUS-312
            //axis.DateTimeGridAlignment = DateTimeMeasurementUnit.Day;
            axis.GridSpacingAuto = true;
           
            axis.Range.SideMarginsEnabled = true;
            axis.GridLines.Visible = false;
            //axis.DateTimeOptions.Format = DateTimeFormat.Custom;
            //axis.DateTimeOptions.FormatString = "HH:mm";
            //4/18/2014 NS commented out for VSPLUS-312
            //axis.DateTimeGridAlignment = DateTimeMeasurementUnit.Minute;
            axis.DateTimeMeasureUnit = DateTimeMeasurementUnit.Minute;


            AxisBase axisy = ((XYDiagram)dominoServerWebChart.Diagram).AxisY;
            axisy.Range.AlwaysShowZeroLevel = false;
            axisy.Range.SideMarginsEnabled = true;
            axisy.GridLines.Visible = true;
            dominoServerWebChart.Legend.Visible = true;


            dominoServerWebChart.DataSource = dt1;
            dominoServerWebChart.DataBind();   


            //dominoServerWebChart.Series.Clear();
            //DataTable dt1= new DataTable();
            //if (dayASPxCombo.SelectedIndex == 1)
            //{
            //    dt1 = VSWebBL.DashboardBL.DominoServerStatisticsDetailBLL.Ins.SetGraphForDayCombo("dd", ServerLabel.Text, "1", hrASPxCombo.SelectedItem.Text.Substring(0, 2), hrASPxCombo.SelectedItem.Text.Substring(8, 2));
            //}
            //else if (dayASPxCombo.SelectedIndex == 2)
            //{
            //    dt1 = VSWebBL.DashboardBL.DominoServerStatisticsDetailBLL.Ins.SetGraphForDayCombo("dd", ServerLabel.Text, "7", hrASPxCombo.SelectedItem.Text.Substring(0, 2), hrASPxCombo.SelectedItem.Text.Substring(8, 2));
            //}
            //else
            //{
            //    dt1 = VSWebBL.DashboardBL.DominoServerStatisticsDetailBLL.Ins.SetGraphForDayCombo("dd", ServerLabel.Text, dayASPxCombo.SelectedItem.Text.Substring(0, 2), hrASPxCombo.SelectedItem.Text.Substring(0, 2), hrASPxCombo.SelectedItem.Text.Substring(8, 2));
            //}
          
            //DataTable dt2 = dt1.DefaultView.ToTable(true, "Date");

            ////DataSet ds = new DataSet();
            ////DataTable finaldt1 = dt1.Clone();
            //for (int i = 0; i < dt2.Rows.Count; i++)
            //{
            //    DataTable finaldt = new DataTable();
            //    finaldt = dt1.Clone();
            //    //string str = dt2.Rows[i][0].ToString();
            //    DataRow[] dr = dt1.Select("Date= '" + dt2.Rows[i][0].ToString() + "'");
            //    for (int c = 0; c < dr.Length; c++)
            //    {
            //        finaldt.NewRow();
            //        finaldt.ImportRow(dr[c]);
            //    }

            //    Series series = new Series(dt2.Rows[i][0].ToString(), ViewType.Line);

            //    series.DataSource = finaldt;

            //    series.ArgumentDataMember = dt1.Columns["Time"].ToString();

            //    ValueDataMemberCollection seriesValueDataMembers = (ValueDataMemberCollection)series.ValueDataMembers;
            //    seriesValueDataMembers.AddRange(dt1.Columns["StatValue"].ToString());

            //    dominoServerWebChart.Series.Add(series);

            //    XYDiagram seriesXY = (XYDiagram)dominoServerWebChart.Diagram;
            //    seriesXY.AxisX.Title.Text = "Time";
            //    seriesXY.AxisX.Title.Visible = true;
            //    seriesXY.AxisY.Title.Text = "Transactions";
            //    seriesXY.AxisY.Title.Visible = true;

            //    //transactionPerMinuteWebChart.Legend.Visible = false;

            //    // ((SplineSeriesView)series.View).LineTensionPercent = 100;
            //    ((LineSeriesView)series.View).LineMarkerOptions.Size = 4;
            //    ((LineSeriesView)series.View).LineMarkerOptions.Color = Color.White;

            //    //series.CrosshairLabelPattern = "{A} : {V}";

            //    AxisBase axis = ((XYDiagram)dominoServerWebChart.Diagram).AxisX;
            //    axis.DateTimeGridAlignment = DateTimeMeasurementUnit.Hour;
            //    axis.GridSpacingAuto = false;
            //    axis.MinorCount = 15;
            //    axis.GridSpacing = 0.5;
            //    axis.Range.SideMarginsEnabled = false;
            //    axis.GridLines.Visible = false;
            //    axis.DateTimeOptions.Format = DateTimeFormat.Custom;
            //    axis.DateTimeOptions.FormatString = "dd/MM HH:mm";

            //    AxisBase axisy = ((XYDiagram)dominoServerWebChart.Diagram).AxisY;
            //    axisy.Range.AlwaysShowZeroLevel = false;
            //    axisy.Range.SideMarginsEnabled = true;
            //    axisy.GridLines.Visible = false;
            //}



            

        }

        protected void monthASPxCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            hrASPxCombo.SelectedIndex = 0;
            dayASPxCombo.SelectedIndex = 0;

            DataTable dt1 = VSWebBL.DashboardBL.DominoServerStatisticsDetailBLL.Ins.SetGraph("mm", "azphxdom1/RPRWyatt");
            DataTable dt2 = null;

            if (monthASPxCombo.SelectedIndex == 1)
            {
                dt2 = VSWebBL.DashboardBL.DominoServerStatisticsDetailBLL.Ins.SetGraphForMonthCombo("azphxdom1/RPRWyatt", "mm", monthASPxCombo.Value.ToString());
            }
            else if(monthASPxCombo.SelectedIndex == 2)
            {
                dt2 = VSWebBL.DashboardBL.DominoServerStatisticsDetailBLL.Ins.SetGraphForMonthCombo("azphxdom1/RPRWyatt", "mm", monthASPxCombo.Value.ToString());
            }
            else if (monthASPxCombo.SelectedIndex == 3)
            {
                dt2 = VSWebBL.DashboardBL.DominoServerStatisticsDetailBLL.Ins.SetGraphForMonthCombo("azphxdom1/RPRWyatt", "mm", monthASPxCombo.Value.ToString());
            }
            else if (monthASPxCombo.SelectedIndex == 4)
            {
                dt2 = VSWebBL.DashboardBL.DominoServerStatisticsDetailBLL.Ins.SetGraphForMonthCombo("azphxdom1/RPRWyatt", "mm", monthASPxCombo.Value.ToString());
            }
            else
            {
                dt2 = VSWebBL.DashboardBL.DominoServerStatisticsDetailBLL.Ins.SetGraphForMonthCombo("azphxdom1/RPRWyatt", "mm", monthASPxCombo.Value.ToString());
            }

            bool flag = true;

            Series series = null;

            while (flag)
            {
                if (series == null)
                {
                    series = new Series("series1", ViewType.Line);
                    series.Visible = true;
                    series.DataSource = dt1;

                    series.ArgumentDataMember = dt1.Columns["Date"].ToString();

                    ValueDataMemberCollection seriesValueDataMembers = (ValueDataMemberCollection)series.ValueDataMembers;
                    seriesValueDataMembers.AddRange(dt1.Columns["StatValue"].ToString());
                    series.CrosshairEnabled = DevExpress.Utils.DefaultBoolean.True;
                    series.View.Color = Color.Blue;
                    dominoServerWebChart.Series.Add(series);
                    ((XYDiagram)dominoServerWebChart.Diagram).PaneLayoutDirection = PaneLayoutDirection.Horizontal;

                    XYDiagram seriesXY = (XYDiagram)dominoServerWebChart.Diagram;
                    seriesXY.AxisX.Title.Text = "Time";
                    seriesXY.AxisX.Title.Visible = true;
                    seriesXY.AxisY.Title.Text = "Performance";
                    seriesXY.AxisY.Title.Visible = true;

                    dominoServerWebChart.Legend.Visible = true;

                   // ((SplineSeriesView)series.View).LineTensionPercent = 100;
                    ((LineSeriesView)series.View).LineMarkerOptions.Size = 4;
                    ((LineSeriesView)series.View).LineMarkerOptions.Color = Color.White;

                    //PointSeriesLabel label = (PointSeriesLabel)series.Label;
                    //label.LineLength = 12;
                    //label.LineVisible = true;
                    //label.ResolveOverlappingMode = ResolveOverlappingMode.JustifyAllAroundPoint;

                    AxisBase axis2 = ((XYDiagram)dominoServerWebChart.Diagram).AxisX;
                    //4/18/2014 NS commented out for VSPLUS-312
                    //axis2.DateTimeGridAlignment = DateTimeMeasurementUnit.Month;
                    axis2.GridSpacingAuto = false;
                    axis2.MinorCount = 15;

                    //axis.GridSpacing = 12;
                    axis2.GridSpacing = 0.5;
                    axis2.Range.SideMarginsEnabled = false;
                    axis2.GridLines.Visible = true;
                    axis2.DateTimeOptions.Format = DateTimeFormat.Custom;
                    axis2.DateTimeOptions.FormatString = "dd/MM HH:mm";

                    //((SplineSeriesView)series.View).Color = Color.Blue;

                    AxisBase axisy = ((XYDiagram)dominoServerWebChart.Diagram).AxisY;
                    axisy.Range.AlwaysShowZeroLevel = false;
                    axisy.Range.SideMarginsEnabled = true;
                }
                else
                {
                    Series series1 = new Series("series2", ViewType.Line);
                    //Series series2 = new Series("s2", ViewType.Spline);
                    series1.Visible = true;
                    series1.DataSource = dt2;
                    series1.ArgumentDataMember = dt2.Columns["Date"].ToString();

                    ValueDataMemberCollection seriesValueDataMembers = (ValueDataMemberCollection)series1.ValueDataMembers;
                    seriesValueDataMembers.AddRange(dt2.Columns["StatValue"].ToString());
                    series1.CrosshairEnabled = DevExpress.Utils.DefaultBoolean.True;
                    series1.View.Color = Color.SeaGreen;
                    dominoServerWebChart.Series.Add(series1);
                    ((XYDiagram)dominoServerWebChart.Diagram).PaneLayoutDirection = PaneLayoutDirection.Horizontal;

                    XYDiagram seriesXY = (XYDiagram)dominoServerWebChart.Diagram;
                    //seriesXY.AxisX.Title.Text = "Time";
                    //seriesXY.AxisX.Title.Visible = true;
                    seriesXY.AxisY.Title.Text = "Performance";
                    seriesXY.AxisY.Title.Visible = true;

                    dominoServerWebChart.Legend.Visible = false;

                    //((SplineSeriesView)series1.View).LineTensionPercent = 100;
                    ((LineSeriesView)series1.View).LineMarkerOptions.Size = 4;
                    ((LineSeriesView)series1.View).LineMarkerOptions.Color = Color.White;

                    // ((SplineSeriesView)series1.View).Color = Color.SeaGreen;
                    //PointSeriesLabel label = (PointSeriesLabel)series.Label;
                    //label.LineLength = 12;
                    //label.LineVisible = true;
                    //label.ResolveOverlappingMode = ResolveOverlappingMode.JustifyAllAroundPoint;

                    AxisBase axis2 = ((XYDiagram)dominoServerWebChart.Diagram).AxisX;
                    //4/18/2014 NS commented out for VSPLUS-312
                    //axis2.DateTimeGridAlignment = DateTimeMeasurementUnit.Month;
                    axis2.GridSpacingAuto = false;
                    axis2.MinorCount = 15;
                    //axis.GridSpacing = 12;
                    axis2.GridSpacing = 0.5;
                    axis2.Range.SideMarginsEnabled = false;
                    axis2.GridLines.Visible = true;
                    axis2.DateTimeOptions.Format = DateTimeFormat.Custom;
                    axis2.DateTimeOptions.FormatString = "dd/MM HH:mm";

                    AxisBase axisy = ((XYDiagram)dominoServerWebChart.Diagram).AxisY;
                    axisy.Range.AlwaysShowZeroLevel = false;
                    axisy.Range.SideMarginsEnabled = true;

                    //((SplineSeriesView)series.View).Color = Color.;

                    flag = false;
                }
            }
        }

        protected void BackBtn_Click(object sender, EventArgs e)
        {

            Response.Redirect("DominoServerDetailsPage2.aspx?Name=" + ServerLabel.Text + "&Typ=" + sType, false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
            Context.ApplicationInstance.CompleteRequest();
        }        
    }
}