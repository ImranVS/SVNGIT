using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using DevExpress.XtraCharts;
using System.Data;

namespace VSWebUI.DashboardReports
{
    public partial class StalemailboxeRpt : DevExpress.XtraReports.UI.XtraReport
    {// 3/17/2016 Durga Addded for VSPLUS-2702
        public StalemailboxeRpt()
        {
            InitializeComponent();
        }

        private void ResponseTimeXtraRpt_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            int maxC = 50;
            XRChart xrChart = new XRChart();
            xrChart.Series.Clear();
            DataTable dt = new DataTable();

            string typeval = "";
            typeval = this.TypeVal.Value.ToString();

          
            dt = VSWebBL.ReportsBL.ReportsBL.Ins.GetStalemailboxesInfo(typeval);
            BuildChart(0, dt.Rows.Count, dt, xrChart1);
           
        }

        public void BuildChart(int countS, int countE, DataTable dt, XRChart xrChart)
        {

            xrChart.Series.Clear();
            Series series = null;
            for (int i = countS; i < countE; i++)
            {
                if (series == null)
                {
                    series = new Series("Server", ViewType.Bar);
                    series.ArgumentDataMember = "DisplayName";
                    series.ArgumentScaleType = ScaleType.Qualitative;
                    series.ValueScaleType = ScaleType.Numerical;
                }
                if (series != null)
                {
                    series.Points.Add(new SeriesPoint(dt.Rows[i]["DisplayName"].ToString(), Convert.ToDouble(dt.Rows[i]["InActiveDaysCount"].ToString())));
                    xrChart.Series.Add(series);
                    series = null;
                }
            }
            // 3/24/2016 Durga Addded for VSPLUS-2702
            if (countE != 0)
            {
                ((XYDiagram)xrChart1.Diagram).PaneLayoutDirection = PaneLayoutDirection.Horizontal;
                XYDiagram seriesXY = (XYDiagram)xrChart.Diagram;
                seriesXY.EnableAxisXScrolling = true;
                seriesXY.EnableAxisYScrolling = true;
                seriesXY.AxisX.Label.ResolveOverlappingOptions.AllowRotate = false;
                seriesXY.AxisX.Label.ResolveOverlappingOptions.AllowHide = false;
                seriesXY.AxisY.Title.Text = "Days of Inactivity";
                seriesXY.AxisY.Title.Visible = true;
                seriesXY.AxisX.Title.Visible = false;
            }
            else
            {
               
                EmptyChartText myText = xrChart.EmptyChartText;

                myText.Antialiasing = true;
                myText.Text = "There is no data to dispaly.";
                myText.TextColor = Color.Black;
            }

           
            xrChart.DataSource = dt;
        }


    

    }
}
