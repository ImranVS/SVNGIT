using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using DevExpress.XtraCharts;
using System.Data;

namespace VSWebUI.DashboardReports
{
    public partial class IBMConnectionForumsRptXtraRpt : DevExpress.XtraReports.UI.XtraReport
    {
        public IBMConnectionForumsRptXtraRpt()
        {
            InitializeComponent();
        }

        private void IBMConnectionForumsRptXtraRpt_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {

          
            DataTable dt = new DataTable();
            dt = VSWebBL.ReportsBL.ReportsBL.Ins.GetIBMConnectionstatsInfo(this.ServerName.Value.ToString(), this.ForumType.Value.ToString(),
                this.DateFrom.Value.ToString(),this.DateTo.Value.ToString());
            Series series = null;
            string srvName = "";

            if (dt.Rows.Count > 0)
            {
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
                            series.ArgumentDataMember = dt.Columns["Hour"].ToString();
                            series.ArgumentScaleType = ScaleType.Numerical;
                        }
                        else
                        {
                            series.ArgumentDataMember = dt.Columns["Date"].ToString();
                            series.ArgumentScaleType = ScaleType.DateTime;
                        }
                        series.ValueScaleType = ScaleType.Numerical;
                    }
                    srvName = dt.Rows[i]["ServerName"].ToString();
                    if (series != null)
                    {
                        if (this.RptType.Value.ToString() == "0")
                        {
                            series.Points.Add(new SeriesPoint(Convert.ToInt32(dt.Rows[i]["Hour"].ToString()), Convert.ToDouble(dt.Rows[i]["StatValue"].ToString())));
                        }
                        else
                        {
                            series.Points.Add(new SeriesPoint(Convert.ToDateTime(dt.Rows[i]["Date"].ToString()), Convert.ToDouble(dt.Rows[i]["StatValue"].ToString())));
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
                seriesXY.AxisY.Title.Text = "Count";
                if (this.RptType.Value.ToString() == "0")
                {
                    seriesXY.AxisX.Title.Text = "Hour";
                }
                else
                {
                    seriesXY.AxisX.Title.Text = "Date";
                }
                seriesXY.AxisY.Title.Visible = true;
                seriesXY.AxisX.Title.Visible = true;
            }
            xrChart1.DataSource = dt;
        }

    }
}
