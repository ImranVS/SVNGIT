using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using DevExpress.XtraCharts;
using System.Data;

namespace VSWebUI.DashboardReports
{
    public partial class ResponseTimeXtraRpt : DevExpress.XtraReports.UI.XtraReport
    {
        public ResponseTimeXtraRpt()
        {
            InitializeComponent();
        }

        private void ResponseTimeXtraRpt_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            int maxC = 50;
            DataTable dt = new DataTable();
            string typeval = "";
            typeval = this.TypeVal.Value.ToString();
            dt = VSWebBL.ReportsBL.ReportsBL.Ins.GetResponseTimes(typeval);
            //6/25/2014 NS modified for VSPLUS-277
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows.Count > maxC && dt.Rows.Count <= maxC * 2)
                {
                    BuildChart(0, maxC, dt, xrChart1);
                    xrChart2.Visible = true;
                    BuildChart(maxC, dt.Rows.Count, dt, xrChart2);
                }
                else if (dt.Rows.Count > maxC * 2 && dt.Rows.Count <= maxC * 3)
                {
                    BuildChart(0, maxC, dt, xrChart1);
                    xrChart2.Visible = true;
                    BuildChart(maxC, maxC * 2, dt, xrChart2);
                    xrChart3.Visible = true;
                    BuildChart(maxC * 2, dt.Rows.Count, dt, xrChart3);
                }
                else if (dt.Rows.Count > maxC * 3 && dt.Rows.Count <= maxC * 4)
                {
                    BuildChart(0, maxC, dt, xrChart1);
                    xrChart2.Visible = true;
                    BuildChart(maxC, maxC * 2, dt, xrChart2);
                    xrChart3.Visible = true;
                    BuildChart(maxC * 2, maxC * 3, dt, xrChart3);
                    xrChart4.Visible = true;
                    BuildChart(maxC * 3, dt.Rows.Count, dt, xrChart4);
                }
                else if (dt.Rows.Count > maxC * 4 && dt.Rows.Count <= maxC * 5)
                {
                    BuildChart(0, maxC, dt, xrChart1);
                    xrChart2.Visible = true;
                    BuildChart(maxC, maxC * 2, dt, xrChart2);
                    xrChart3.Visible = true;
                    BuildChart(maxC * 2, maxC * 3, dt, xrChart3);
                    xrChart4.Visible = true;
                    BuildChart(maxC * 3, maxC * 4, dt, xrChart4);
                    xrChart5.Visible = true;
                    BuildChart(maxC * 4, dt.Rows.Count, dt, xrChart5);
                }
                else
                {
                    BuildChart(0, dt.Rows.Count, dt, xrChart1);
                }
            }
        }

        public void BuildChart(int countS, int countE, DataTable dt, XRChart xrChart)
        {
            //6/25/2014 NS modified for VSPLUS-277
            xrChart.Series.Clear();
            Series series = null;
            for (int i = countS; i < countE; i++)
            {
                if (series == null)
                {
                    series = new Series("Server", ViewType.Bar);
                    series.ArgumentDataMember = dt.Columns["Server"].ToString();
                    series.ArgumentScaleType = ScaleType.Qualitative;
                    series.ValueScaleType = ScaleType.Numerical;
                }
                if (series != null)
                {
                    series.Points.Add(new SeriesPoint(dt.Rows[i]["Server"].ToString(), Convert.ToDouble(dt.Rows[i]["ResponseTime"].ToString())));
                    xrChart.Series.Add(series);
                    series = null;
                }
            }
            ((XYDiagram)xrChart1.Diagram).PaneLayoutDirection = PaneLayoutDirection.Horizontal;
            XYDiagram seriesXY = (XYDiagram)xrChart.Diagram;
            seriesXY.EnableAxisXScrolling = true;
            seriesXY.EnableAxisYScrolling = true;
            seriesXY.AxisX.Label.ResolveOverlappingOptions.AllowRotate = false;
            seriesXY.AxisX.Label.ResolveOverlappingOptions.AllowHide = false;
            seriesXY.AxisY.Title.Text = "Milliseconds";
            seriesXY.AxisY.Title.Visible = true;
            seriesXY.AxisX.Title.Visible = false;
            xrChart.DataSource = dt;
        }
    }
}
