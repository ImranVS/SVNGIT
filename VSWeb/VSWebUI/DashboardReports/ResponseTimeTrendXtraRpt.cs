using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using DevExpress.XtraCharts;
using System.Data;

namespace VSWebUI.DashboardReports
{
    public partial class ResponseTimeTrendXtraRpt : DevExpress.XtraReports.UI.XtraReport
    {
        public ResponseTimeTrendXtraRpt()
        {
            InitializeComponent();
        }

        private void ResponseTimeTrendXtraRpt_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            string serverName = "";
            string srvName = "";
            string sName = "";
            bool exactmatch;
            DateTime monthyear;
            Series series = null;
            
            serverName = this.ServerNameSQL.Value.ToString();
            srvName = this.ServerName.Value.ToString();
            exactmatch = (bool)this.ExactMatch.Value;
            sName = srvName;
            if (srvName == "" || exactmatch)
            {
                sName = serverName;
            }
            xrChart1.Series.Clear();
            DataTable dt = new DataTable();
            dt = VSWebBL.ReportsBL.ReportsBL.Ins.ResponseTimeTrendRpt(this.DateFrom.Value.ToString(), this.DateTo.Value.ToString(),
                sName, exactmatch, this.ServerTypeSQL.Value.ToString(),this.RptType.Value.ToString());
            if (dt.Rows.Count > 0)
            {
                dt.Columns.Add("MonthYear", typeof(DateTime));
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
                        if (this.RptType.Value.ToString() == "0")
                        {
                            series.ArgumentDataMember = dt.Columns["Date"].ToString();
                            series.ArgumentScaleType = ScaleType.DateTime;
                        }
                        else
                        {
                            monthyear = new DateTime(Convert.ToInt32(dt.Rows[i]["YearNumber"].ToString()), Convert.ToInt32(dt.Rows[i]["MonthNumber"].ToString()), 1);
                            dt.Rows[i]["MonthYear"] = monthyear;
                            series.ArgumentDataMember = dt.Columns["MonthYear"].ToString();
                            series.ArgumentScaleType = ScaleType.DateTime;
                        }
                        series.ValueScaleType = ScaleType.Numerical;
                    }
                    srvName = dt.Rows[i]["ServerName"].ToString();
                    if (series != null)
                    {
                        if (this.RptType.Value.ToString() == "0")
                        {
                            series.Points.Add(new SeriesPoint(Convert.ToDateTime(dt.Rows[i]["Date"].ToString()), Convert.ToDouble(dt.Rows[i]["ResponseTime"].ToString())));
                        }
                        else
                        {
                            monthyear = new DateTime(Convert.ToInt32(dt.Rows[i]["YearNumber"].ToString()), Convert.ToInt32(dt.Rows[i]["MonthNumber"].ToString()), 1);
                            series.Points.Add(new SeriesPoint(monthyear, Convert.ToDouble(dt.Rows[i]["ResponseTime"].ToString())));
                        }
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
                seriesXY.AxisY.Title.Text = "Average Response Time (ms)";
                if (this.RptType.Value.ToString() == "0")
                {
                    seriesXY.AxisX.Label.DateTimeOptions.Format = DateTimeFormat.ShortDate;
                    seriesXY.AxisX.Title.Text = "Date";
                }
                else
                {
                    seriesXY.AxisX.Label.DateTimeOptions.Format = DateTimeFormat.MonthAndYear;
                    seriesXY.AxisX.DateTimeScaleOptions.MeasureUnit = DateTimeMeasureUnit.Month;
                    seriesXY.AxisX.Title.Text = "Month/Year";
                }
                seriesXY.AxisY.Title.Visible = true;
                seriesXY.AxisX.Title.Visible = true;
            }
            xrChart1.DataSource = dt;
            //MonthYearLabel.Text = "Report for " + this.MonthYear.Value.ToString();
        }

    }
}
