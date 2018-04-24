using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using DevExpress.XtraCharts;
using System.Data;
// 3/29/2016 Durga Addded for VSPLUS-2698
namespace VSWebUI.DashboardReports
{
    public partial class ServerUtilizationRpt : DevExpress.XtraReports.UI.XtraReport
    {
        public ServerUtilizationRpt()
        {
            InitializeComponent();
        }

        private void ResponseTimeXtraRpt_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            int maxC = 50;
            DataTable dt = new DataTable();
            string servertype = "";
            double IdealUserCount, StatValue;
            if (this.ServerType.Value.ToString() == "Domino")

                dt = VSWebBL.ReportsBL.ReportsBL.Ins.GetServerUtiliZationOfDomino(this.ServerName.Value.ToString());
            else
                dt = VSWebBL.ReportsBL.ReportsBL.Ins.GetServerUtiliZationOfExchange(this.ServerName.Value.ToString(), this.UserType.Value.ToString());
            dt.Columns.Add("PercentUtilization", typeof(string));
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["IdealUserCount"].ToString() != "0")
                {
                    
                    IdealUserCount = Convert.ToDouble(dt.Rows[i]["IdealUserCount"].ToString() == "" ? "0" : dt.Rows[i]["IdealUserCount"].ToString());
                    StatValue = Convert.ToDouble(dt.Rows[i]["StatValue"].ToString() == "" ? "0" : dt.Rows[i]["StatValue"].ToString());
                    dt.Rows[i]["PercentUtilization"] = Math.Round(((StatValue / IdealUserCount) * 100),2);
                }
                //else
                //    dt.Rows[i]["PercentUtilization"] = "100";
            }

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
                    series = new Series("servername", ViewType.Bar);
                    series.ArgumentDataMember = dt.Columns["servername"].ToString();
                    series.ArgumentScaleType = ScaleType.Qualitative;
                    series.ValueScaleType = ScaleType.Numerical;
                   
                }
                if (series != null)
                {
                    //5/5/2016 Sowjanya modified for VSPLUS-2919
                    series.Points.Add(new SeriesPoint(dt.Rows[i]["servername"].ToString(), Convert.ToDouble(dt.Rows[i]["PercentUtilization"].ToString() == "" ? "0" : dt.Rows[i]["PercentUtilization"].ToString())));
                    xrChart.Series.Add(series);
                    series = null;
                }
            }
            if (countE != 0)
            {

                ((XYDiagram)xrChart1.Diagram).PaneLayoutDirection = PaneLayoutDirection.Horizontal;
                XYDiagram seriesXY = (XYDiagram)xrChart.Diagram;
                seriesXY.EnableAxisXScrolling = true;
                seriesXY.EnableAxisYScrolling = true;
                seriesXY.AxisX.Label.ResolveOverlappingOptions.AllowRotate = false;
                seriesXY.AxisX.Label.ResolveOverlappingOptions.AllowHide = false;

                seriesXY.AxisY.Title.Text = "Percentage Of Server Utilization";



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
