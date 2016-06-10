using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using DevExpress.XtraCharts;
using System.Data;

namespace VSWebUI.DashboardReports
{
    public partial class MAXCpuUtilXtraRpt : DevExpress.XtraReports.UI.XtraReport
    {
        public MAXCpuUtilXtraRpt()
        {
            InitializeComponent();
        }

        private void MAXCpuUtilXtraRpt_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            DataTable dt = new DataTable();
            //27/05/2016 sowmya added for vaplus-2971
            dt = VSWebBL.ReportsBL.ReportsBL.Ins.GetMAXCpuUtil(this.ServerName.Value.ToString(), (DateTime)this.StartDate.Value, (DateTime)this.EndDate.Value,this.Threshold.Value.ToString());
            Series series = null;
            string srvName = "";

            if (dt.Rows.Count > 0)
            {
                xrChart1.Series.Clear();
                srvName = dt.Rows[0]["ServerName"].ToString();
                series = new Series(dt.Rows[0]["ServerName"].ToString(), ViewType.Area);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (srvName != dt.Rows[i]["ServerName"].ToString())
                    {
                        if (series != null)
                        {
                            xrChart1.Series.Add(series);
                        }
                        series = new Series(dt.Rows[i]["ServerName"].ToString(), ViewType.Area);
                        series.ArgumentDataMember = dt.Columns["Date"].ToString();
                        series.ArgumentScaleType = ScaleType.DateTime;
                        series.ValueScaleType = ScaleType.Numerical;
                    }
                    srvName = dt.Rows[i]["ServerName"].ToString();
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
                seriesXY.AxisY.Title.Text = "CPU";
                seriesXY.AxisX.Title.Text = "Date";
                seriesXY.AxisY.Title.Visible = true;
                seriesXY.AxisX.Title.Visible = true;
            }
            xrChart1.DataSource = dt;
            MonthYearLabel.Text = "Report for " + ((DateTime)this.StartDate.Value).ToShortDateString() + " - " + ((DateTime)this.EndDate.Value).ToShortDateString();
        }

    }
}
