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
    public partial class DailyMemoryUsedXtraRpt : DevExpress.XtraReports.UI.XtraReport
    {
        public DailyMemoryUsedXtraRpt()
        {
            InitializeComponent();
        }

        private void DailyMemoryUsedXtraRpt_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
           // DailyMemoryUsedRptDSTableAdapters.DominoSummaryStatsTableAdapter dominoAdapter = new DailyMemoryUsedRptDSTableAdapters.DominoSummaryStatsTableAdapter();
           // dominoAdapter.Fill(this.dailyMemoryUsedRptDS1.DominoSummaryStats, (int)this.DateM.Value, (int)this.DateY.Value, this.ServerName.Value.ToString());
            
            DataTable dt=new DataTable();
            dt=VSWebBL.ReportsBL.XsdBL.Ins.getDailyMemoryUsedBL(this.ServerName.Value.ToString(), int.Parse(this.DateY.Value.ToString()), 
                int.Parse(this.DateM.Value.ToString()),this.ServerType.Value.ToString());
            Series series = null;
            string srvName = "";

            if (dt.Rows.Count > 0)
            {
                xrChart1.Series.Clear();
                srvName = dt.Rows[0]["ServerName"].ToString();
                series = new Series(dt.Rows[0]["ServerName"].ToString(), ViewType.Line);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (srvName != dt.Rows[i]["ServerName"].ToString())
                    {
                        if (series != null)
                        {
                            xrChart1.Series.Add(series);
                        }
                        series = new Series(dt.Rows[i]["ServerName"].ToString(), ViewType.Line);
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
                seriesXY.AxisY.Title.Text = "Percentage";
                seriesXY.AxisX.Title.Text = "Date";
                seriesXY.AxisY.Title.Visible = true;
                seriesXY.AxisX.Title.Visible = true;
            }
            this.DataSource = dt;
            xrChart1.DataSource = dt;
            //xrTableCell1.DataBindings.Add("Text", dt, "ServerName");
            //xrTableCell2.DataBindings.Add("Text", dt, "StatValue");
            //xrTableCell6.DataBindings.Add("Text", dt, "Date","{0:MM/dd/yyyy}");
            MonthYearLabel.Text = "Report for " + this.MonthYear.Value.ToString();
        }

    }
}
