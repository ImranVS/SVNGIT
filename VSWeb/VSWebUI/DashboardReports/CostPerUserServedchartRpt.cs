using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using DevExpress.XtraCharts;
using System.Data;

namespace VSWebUI.DashboardReports
{
    public partial class CostPerUserServedchartRpt : DevExpress.XtraReports.UI.XtraReport
    {
        public CostPerUserServedchartRpt()
        {
            InitializeComponent();
        }

        private void ResponseTimeXtraRpt_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            int maxC = 50;
            DataTable dt = new DataTable();
            string servertype = "";
            servertype = this.ServerType.Value.ToString();
            if (this.ServerType.Value.ToString()=="Domino")
            {
                dt = VSWebBL.DashboardBL.KeyMetricsBL.Ins.GetCostperuserserveddataForDomino("2", this.ServerName.Value.ToString(), this.ServerType.Value.ToString());
             
            }
            else
            {
                dt = VSWebBL.DashboardBL.KeyMetricsBL.Ins.GetCostperuserserveddata("2", this.ServerName.Value.ToString(), this.ServerType.Value.ToString(), this.UserType.Value.ToString());
               
            }

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                decimal avgusers = Convert.ToDecimal(dt.Rows[i]["StatValue"].ToString() == "" ? "0" : dt.Rows[i]["StatValue"].ToString());//dt.Rows[i]["MonthlyOperatingCost"]
                decimal MonthlyopeartingCost = Convert.ToDecimal(dt.Rows[i]["MonthlyOperatingCost"].ToString() == "" ? "0" : dt.Rows[i]["MonthlyOperatingCost"].ToString());
                if (avgusers != 0)
                    dt.Rows[i]["StatValue"] = Math.Round((MonthlyopeartingCost / avgusers), 2);
                else
                    dt.Rows[i]["StatValue"] = "0";
            }
            DataView dv = dt.AsDataView();

            dv.Sort = "StatValue desc";

            DataTable sortedDT = dv.ToTable();
            BuildChart(0, sortedDT.Rows.Count, sortedDT, xrChart1);

          
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
                    series.Points.Add(new SeriesPoint(dt.Rows[i]["servername"].ToString(), Convert.ToDouble(dt.Rows[i]["StatValue"].ToString())));
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

                seriesXY.AxisY.Title.Text = "Cost Per User Served";
                seriesXY.AxisY.Title.Visible = true;
                seriesXY.AxisX.Title.Visible = false;
            }
            else
            {
                EmptyChartText myText = xrChart.EmptyChartText;

                myText.Antialiasing = true;
                myText.Text = "There is no data to display.";
                myText.TextColor = Color.Black;
            }
            UI uiobj = new UI();
            uiobj.RecalibrateChartAxes(xrChart1.Diagram);
            xrChart.DataSource = dt;
            
        }

        private void xrChart1_BoundDataChanged(object sender, EventArgs e)
        {
            //UI uiobj = new UI();
            //uiobj.RecalibrateChartAxes(xrChart1.Diagram);
        }
    }
}
