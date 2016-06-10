using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using DevExpress.XtraCharts;
using System.Data;
using System.Globalization;

namespace VSWebUI.DashboardReports
{
    public partial class DominoDiskAnnualTrendXtraRpt : DevExpress.XtraReports.UI.XtraReport
    {
        public DataTable dt;

        public DominoDiskAnnualTrendXtraRpt()
        {
            InitializeComponent();
        }

        private void DominoDiskAnnualTrendXtraRpt_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            bool seriesadded = false;
            string monthName = "";
            string srvdiskName = "";
            string year = "";
            string srvName = "";
            string diskName = "";
            string srvType = "";
            year = this.DateY.Value.ToString();
            srvName = this.ServerName.Value.ToString();
            diskName = this.DiskName.Value.ToString();
            srvType = this.ServerType.Value.ToString();
            dt = VSWebBL.ReportsBL.ReportsBL.Ins.GetDominoDiskMonthlyConsumption(srvName, diskName, year, srvType);
            Series series = null;
            if (dt.Rows.Count > 0)
            {
                xrChart1.Series.Clear();
                srvName = dt.Rows[0]["ServerName"].ToString();
                diskName = dt.Rows[0]["DiskName"].ToString();
                series = new Series(srvName + " - " + diskName, ViewType.StackedArea);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    srvdiskName = dt.Rows[i]["ServerName"].ToString() + " - " + dt.Rows[i]["DiskName"].ToString();
                    if ((srvName + " - " + diskName) != srvdiskName)
                    {
                        if (series != null && !seriesadded)
                        {
                            (series.View as StackedAreaSeriesView).Transparency = 180;
                            (series.View as StackedAreaSeriesView).FillStyle.FillMode = FillMode.Gradient;
                            ((PolygonGradientFillOptions)(series.View as StackedAreaSeriesView).FillStyle.Options).GradientMode = PolygonGradientMode.TopToBottom;
                            xrChart1.Series.Add(series);
                        }
                        srvName = dt.Rows[i]["ServerName"].ToString();
                        diskName = dt.Rows[i]["DiskName"].ToString();
                        series = xrChart1.Series[srvName + " - " + diskName];
                        if (series == null)
                        {
                            seriesadded = false;
                            series = new Series(srvName + " - " + diskName, ViewType.StackedArea);
                        }
                        else
                        {
                            seriesadded = true;
                        }
                        series.ArgumentDataMember = dt.Columns["MonthYear"].ToString();
                        series.ArgumentScaleType = ScaleType.Qualitative;
                        series.ValueScaleType = ScaleType.Numerical;
                        series.Label.ResolveOverlappingMode = ResolveOverlappingMode.JustifyAroundPoint;
                    }
                    srvName = dt.Rows[i]["ServerName"].ToString();
                    diskName = dt.Rows[i]["DiskName"].ToString();
                    if (series != null)
                    {
                        monthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToInt32(dt.Rows[i]["MonthYear"].ToString()));
                        series.Points.Add(new SeriesPoint(monthName, Convert.ToDouble(dt.Rows[i]["StatValue"].ToString())));
                    }
                }
                if (series != null && !seriesadded)
                {
                    (series.View as StackedAreaSeriesView).Transparency = 180;
                    (series.View as StackedAreaSeriesView).FillStyle.FillMode = FillMode.Gradient;
                    ((PolygonGradientFillOptions)(series.View as StackedAreaSeriesView).FillStyle.Options).GradientMode = PolygonGradientMode.TopToBottom;
                    xrChart1.Series.Add(series);
                }
                ((XYDiagram)xrChart1.Diagram).PaneLayoutDirection = PaneLayoutDirection.Horizontal;
                XYDiagram seriesXY = (XYDiagram)xrChart1.Diagram;
                /*
                double min = Convert.ToDouble(((XYDiagram)xrChart1.Diagram).AxisX.Range.MinValue);
                double max = Convert.ToDouble(((XYDiagram)xrChart1.Diagram).AxisX.Range.MaxValue);

                int gs = (int)((max - min) / 11);

                if (gs == 0)
                {
                    gs = 1;
                    seriesXY.AxisX.GridSpacingAuto = false;
                    seriesXY.AxisX.GridSpacing = gs;
                }
                 */
                seriesXY.AxisX.Label.ResolveOverlappingOptions.AllowRotate = true;
                seriesXY.AxisX.Label.ResolveOverlappingOptions.AllowHide = true;
                seriesXY.AxisX.Label.ResolveOverlappingOptions.AllowStagger = true;
                seriesXY.AxisY.Title.Text = "Size (GB)";
                seriesXY.AxisX.Title.Text = "Month";
                seriesXY.AxisY.Title.Visible = true;
                seriesXY.AxisX.Title.Visible = true;
            }
            xrChart1.DataSource = dt;
            MonthYearLabel.Text = "Report for " + this.DateY.Value.ToString();
            this.DataSource = dt;
            this.DataMember = "Table";
            GroupField groupField = new GroupField("MonthYear");
            this.GroupHeader2.GroupFields.Add(groupField);

            xrTableCell7.DataBindings.Add("Text", dt, "MonthName");
            xrTableCell1.DataBindings.Add("Text", dt, "ServerName");
            xrTableCell2.DataBindings.Add("Text", dt, "DiskName");
            xrTableCell3.DataBindings.Add("Text", dt, "StatValue");
        }
    }
}
