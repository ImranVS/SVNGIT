using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;
using DevExpress.XtraCharts;
using VSWebBL;

namespace VSWebUI.DashboardReports
{
    public partial class AvgDailyResponseTimeRpt : DevExpress.XtraReports.UI.XtraReport
    {
        public AvgDailyResponseTimeRpt()
        {
            InitializeComponent();
        }

        private void AvgDailyResponseTimeRpt_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
           // AvgDailyResponseTimeTableAdapters.DeviceDailyStatsTableAdapter Avg = new AvgDailyResponseTimeTableAdapters.DeviceDailyStatsTableAdapter();
           // Avg.Fill(this.avgDailyResponseTime1.DeviceDailyStats, this.MyDevice.Value.ToString(), (DateTime)this.StartDate.Value, (DateTime)this.EndDate.Value);

         
            DataTable dt=new DataTable();
            dt=VSWebBL.ReportsBL.XsdBL.Ins.getAvgDailyResponseBL(this.MyDevice.Value.ToString(),(DateTime)this.StartDate.Value,
                (DateTime)this.EndDate.Value, this.ServerType.Value.ToString());

            Series series = null;
            string srvName = "";

            if (dt.Rows.Count > 0)
            {
                //xrChart1.Series.Clear();
                srvName = dt.Rows[0]["DeviceName"].ToString();
                series = new Series(dt.Rows[0]["DeviceName"].ToString(), ViewType.Line);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (srvName != dt.Rows[i]["DeviceName"].ToString())
                    {
                        if (series != null)
                        {
                            xrChart1.Series.Add(series);
                        }
                        series = new Series(dt.Rows[i]["DeviceName"].ToString(), ViewType.Line);
                        series.ArgumentDataMember = dt.Columns["Date"].ToString();
                        series.ArgumentScaleType = ScaleType.DateTime;
                        series.ValueScaleType = ScaleType.Numerical;
                    }
                    srvName = dt.Rows[i]["DeviceName"].ToString();
                    if (series != null)
                    {
                        series.Points.Add(new SeriesPoint(dt.Rows[i]["Date"].ToString(), Convert.ToDouble(dt.Rows[i]["StatValue"].ToString())));
                    }
                }
                if (series != null)
                {
                    xrChart1.Series.Add(series);
                }
                ((XYDiagram)xrChart1.Diagram).PaneLayoutDirection = PaneLayoutDirection.Horizontal;
                XYDiagram seriesXY = (XYDiagram)xrChart1.Diagram;
                seriesXY.AxisX.Label.ResolveOverlappingOptions.AllowRotate = true;
                seriesXY.AxisX.Label.ResolveOverlappingOptions.AllowHide = false;
                seriesXY.AxisY.Title.Text = "Milliseconds";
                seriesXY.AxisX.Title.Text = "Date";
                seriesXY.AxisY.Title.Visible = true;
                seriesXY.AxisX.Title.Visible = true;
            }

            xrChart1.DataSource = dt;
            MonthYearLabel.Text = "Report for " + ((DateTime)this.StartDate.Value).ToShortDateString() + " - " + ((DateTime)this.EndDate.Value).ToShortDateString();

        }




    }
}
