using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;
using DevExpress.XtraCharts;

namespace VSWebUI.DashboardReports
{
    public partial class DominoAccessBrowserXtraRpt : DevExpress.XtraReports.UI.XtraReport
    {
        public DominoAccessBrowserXtraRpt()
        {
            InitializeComponent();
        }

        private void DominoAccessBrowserXtraRpt_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            DataTable dt = new DataTable();
            int maxC = 50;
            string startdt = "";
            string enddt = "";
            string servername = "";
            string statname = "";
            servername = this.ServerName.Value.ToString();
            statname = this.StatName.Value.ToString();
            startdt = this.StartDate.Value.ToString();
            enddt = this.EndDate.Value.ToString();
            dt = VSWebBL.ReportsBL.ReportsBL.Ins.GetDominoStatValues(statname, "Domino", startdt, enddt, servername);
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows.Count > maxC && dt.Rows.Count <= maxC * 2)
                {
                    BuildChart(0, maxC, dt, xrChart1);
                }
                else
                {
                    BuildChart(0, dt.Rows.Count, dt, xrChart1);
                }
            }
        }

        public void BuildChart(int countS, int countE, DataTable dt, XRChart xrChart)
        {
            xrChart.Series.Clear();
            Series series = null;
            string srvName = "";
            srvName = dt.Rows[0]["ServerName"].ToString();
            series = new Series(dt.Rows[0]["ServerName"].ToString(), ViewType.Bar);
            for (int i = countS; i < countE; i++)
            {
                if (srvName != dt.Rows[i]["ServerName"].ToString())
                {
                    if (series != null)
                    {
                        xrChart.Series.Add(series);
                    }
                    series = new Series(dt.Rows[i]["ServerName"].ToString(), ViewType.Bar);
                    series.ArgumentDataMember = dt.Columns["Date"].ToString();
                    series.ArgumentScaleType = ScaleType.DateTime;
                    series.ValueScaleType = ScaleType.Numerical;
                }
                srvName = dt.Rows[i]["ServerName"].ToString();
                if (series != null)
                {
                    series.Points.Add(new SeriesPoint(dt.Rows[i]["Date"].ToString(), Convert.ToInt32(dt.Rows[i]["StatValue"].ToString())));
                }
            }
            if (series != null)
            {
                xrChart.Series.Add(series);
            }
            ((XYDiagram)xrChart.Diagram).PaneLayoutDirection = PaneLayoutDirection.Horizontal;
            XYDiagram seriesXY = (XYDiagram)xrChart.Diagram;
            seriesXY.AxisX.Label.ResolveOverlappingOptions.AllowRotate = true;
            seriesXY.AxisX.Label.ResolveOverlappingOptions.AllowHide = false;
            seriesXY.AxisY.Title.Text = "Value";
            seriesXY.AxisX.Title.Text = "Date";
            seriesXY.AxisY.Title.Visible = true;
            seriesXY.AxisX.Title.Visible = true;
            xrChart.DataSource = dt;
            UI uiobj = new UI();
            uiobj.RecalibrateChartAxes(xrChart1.Diagram, "Y", "int", "int");
            StatLabel.Text = "Report for " + this.StatName.Value.ToString();
        }

        private void xrChart1_BoundDataChanged(object sender, EventArgs e)
        {
            UI uiobj = new UI();
            uiobj.RecalibrateChartAxes(xrChart1.Diagram, "Y", "int", "int");
            
        }
    }
}
