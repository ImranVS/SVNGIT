﻿using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using DevExpress.XtraCharts;
using System.Data;

namespace VSWebUI.DashboardReports
{
    public partial class ExchangeMailDBTrendXtraRpt : DevExpress.XtraReports.UI.XtraReport
    {
        public ExchangeMailDBTrendXtraRpt()
        {
            InitializeComponent();
        }

        private void ExchangeMailDBTrendXtraRpt_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            DataTable dt = new DataTable();
            string servername = "";
            string startdt = "";
            string enddt = "";
            servername = this.ServerName.Value.ToString();
            startdt = this.StartDate.Value.ToString();
            enddt = this.EndDate.Value.ToString();
            dt = VSWebBL.ReportsBL.ReportsBL.Ins.GetUserCountTrend("", startdt, enddt, servername, "MicrosoftSummaryStats", "ExDatabaseSizeMb",false);
            if (dt.Rows.Count > 0)
            {
                BuildChart(0, dt.Rows.Count, dt, xrChart1);
            }
        }

        public void BuildChart(int countS, int countE, DataTable dt, XRChart xrChart)
        {
            xrChart.Series.Clear();
            Series series = null;
            string srvName = "";
            srvName = dt.Rows[0]["ServerName"].ToString();
            series = new Series(dt.Rows[0]["ServerName"].ToString(), ViewType.Line);
            for (int i = countS; i < countE; i++)
            {
                if (srvName != dt.Rows[i]["ServerName"].ToString())
                {
                    if (series != null)
                    {
                        xrChart.Series.Add(series);
                    }
                    series = new Series(dt.Rows[i]["ServerName"].ToString(), ViewType.Line);
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
            seriesXY.AxisY.Title.Text = "Database Size (MB)";
            seriesXY.AxisX.Title.Text = "Date";
            seriesXY.AxisY.Title.Visible = true;
            seriesXY.AxisX.Title.Visible = true;

            xrChart.DataSource = dt;
        }
    }
}
