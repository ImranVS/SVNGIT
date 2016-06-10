using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DevExpress.XtraCharts;
using System.Drawing;

namespace VSWebUI
{
    public partial class WebForm5 : System.Web.UI.Page
    {
        System.DateTime startdate =Convert.ToDateTime("6/22/2012 12:00:00 AM");//System.DateTime.Now;
        System.DateTime Enddate = Convert.ToDateTime("6/22/2012 12:00:00 AM");  //System.DateTime.Today;
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Title = "Notes Database Performance";
            if (StartDateEdit.Text != "" && StartDateEdit.Text != null)
            {
                startdate = Convert.ToDateTime(StartDateEdit.Text);
            }
            else
            {
                StartDateEdit.Text = "6/22/2012";
            }
            if (EndDateEdit.Text != "" && EndDateEdit.Text != null)
            {
                Enddate = Convert.ToDateTime(EndDateEdit.Text);
            }
            else
            {
                EndDateEdit.Text = "6/22/2012";
            }
            if (!IsPostBack)
            {
                fillNameCombobox();
                if (Request.QueryString["Name"] != "" && Request.QueryString["Name"] != null)
                {
                    ResponseLabel.Text = Request.QueryString["Name"];
                    NameComboBox.Text = Request.QueryString["Name"];
                }
                else if (NameComboBox.Text != "" && NameComboBox.Text != null)
                {   
                    ResponseLabel.Text = NameComboBox.Text;
                }
               
                SetGraph(ResponseLabel.Text, startdate, Enddate);

            }
        }

        protected void GraphButton_Click(object sender, EventArgs e)
        {
            if (startdate > Enddate)
            {
                MsgPopupControl.ShowOnPageLoad = true;
                ErrmsgLabel.Text = "Start Date value should be less than End Date.";
            }
            else
            {

                SetGraph(ResponseLabel.Text, startdate, Enddate);
            }

        }


        public void SetDate()
        {
            //string Time; 
            //if (TimeComboBox.Text != "" && TimeComboBox.Text != null)
            //{
            //    Time = TimeComboBox.Text;
            //}
            //else
            //{
            //   Time= TimeComboBox.Text = "Today";
            //}
           
            //if (TimeComboBox.Text == "Today")
            //{
            //    SetGraph(Time, ResponseLabel.Text, startdate, Enddate);
            //}

            //if (TimeComboBox.Text.ToString() == "Two days")
            //{

            //}

            //if (TimeComboBox.Text.ToString() == "Daily")
            //{

            //}
            //if (TimeComboBox.Text.ToString() == "weekly")
            //{

            //}
            //if (TimeComboBox.Text == "Monthly")
            //{

            //}
        
        }
        public void fillNameCombobox()
        {

            DataTable dt = new DataTable();
            try
            {
                dt = VSWebBL.DashboardBL.DatabaseHealthBL.Ins.GetData("");
                NameComboBox.DataSource = dt;
                NameComboBox.TextField = "Name";
                NameComboBox.ValueField = "Name";
                NameComboBox.DataBind();

            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }        
        }

        public void SetGraph( string DeviceName,System.DateTime starttime,System.DateTime endtime)
        {
            try
            {

                DBPerformanceWebChartControl.Series.Clear();
                DataTable dt = VSWebBL.DashboardBL.DatabaseHealthBL.Ins.SetGraph( DeviceName,starttime,endtime);

                Series series = null;
                series = new Series("DominoServer", ViewType.Line);
                series.Visible = true;
                series.ArgumentDataMember = dt.Columns["Date"].ToString();

                ValueDataMemberCollection seriesValueDataMembers = (ValueDataMemberCollection)series.ValueDataMembers;
                seriesValueDataMembers.AddRange(dt.Columns["StatValue"].ToString());
                DBPerformanceWebChartControl.Series.Add(series);

                // Constant Line

                // Cast the chart's diagram to the XYDiagram type, to access its axes.
                XYDiagram diagram = (XYDiagram)DBPerformanceWebChartControl.Diagram;

                // Create a constant line.
                ConstantLine constantLine1 = new ConstantLine("Constant Line 1");
                diagram.AxisY.ConstantLines.Add(constantLine1);

                // Define its axis value.
                constantLine1.AxisValue = 25000;

                // Customize the behavior of the constant line.
               // constantLine1.Visible = true;
                //constantLine1.ShowInLegend = true;
               // constantLine1.LegendText = "Some Threshold";
               constantLine1.ShowBehind = true;

                // Customize the constant line's title.
                constantLine1.Title.Visible = true;
                constantLine1.Title.Text = "Threshold:25000";
                constantLine1.Title.TextColor = Color.Red;
               // constantLine1.Title.Antialiasing = false;
                //constantLine1.Title.Font = new Font("Tahoma", 14, FontStyle.Bold);
                constantLine1.Title.ShowBelowLine = true;
                constantLine1.Title.Alignment = ConstantLineTitleAlignment.Far;

                // Customize the appearance of the constant line.
                constantLine1.Color = Color.Red;
                constantLine1.LineStyle.DashStyle = DashStyle.Solid;
                constantLine1.LineStyle.Thickness = 4;






                ((XYDiagram)DBPerformanceWebChartControl.Diagram).PaneLayoutDirection = PaneLayoutDirection.Horizontal;

                XYDiagram seriesXY = (XYDiagram)DBPerformanceWebChartControl.Diagram;
                seriesXY.AxisY.Title.Text = "ResponseTme";
                seriesXY.AxisY.Title.Visible = true;

                DBPerformanceWebChartControl.Legend.Visible = false;

               // ((SplineSeriesView)series.View).LineTensionPercent = 100;
                ((LineSeriesView)series.View).LineMarkerOptions.Size = 4;
                ((LineSeriesView)series.View).LineMarkerOptions.Color = Color.White;

                AxisBase axis = ((XYDiagram)DBPerformanceWebChartControl.Diagram).AxisX;
                //4/18/2014 NS commented out for VSPLUS-312
                //axis.DateTimeGridAlignment = DateTimeMeasurementUnit.Day;
                axis.GridSpacingAuto = false;
                axis.MinorCount = 15;
                axis.GridSpacing = 0.5;
                axis.Range.SideMarginsEnabled = false;
                axis.GridLines.Visible = true;
                axis.DateTimeOptions.Format = DateTimeFormat.Custom;
                axis.DateTimeOptions.FormatString = "dd/MM HH:mm";
                ((LineSeriesView)series.View).Color = Color.Blue;

                AxisBase axisy = ((XYDiagram)DBPerformanceWebChartControl.Diagram).AxisY;
                axisy.Range.AlwaysShowZeroLevel = false;
                axisy.Range.SideMarginsEnabled = true;

                DBPerformanceWebChartControl.DataSource = dt;
                DBPerformanceWebChartControl.DataBind();

               

            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
          
        }

        protected void OKButton_Click(object sender, EventArgs e)
        {
           
            MsgPopupControl.ShowOnPageLoad = false;
           
        }
       
    }
}