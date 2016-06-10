using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;
using DevExpress.XtraCharts;

namespace VSWebUI.DashboardReports
{
    public partial class SrvAvailabilityXtraRpt : DevExpress.XtraReports.UI.XtraReport
    {
        public SrvAvailabilityXtraRpt()
        {
            InitializeComponent();
        }

        private void SrvAvailabilityXtraRpt_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            DataTable dt = new DataTable();
            dt = VSWebBL.ReportsBL.XsdBL.Ins.SrvAvailabilityRptBL(int.Parse(this.DateM.Value.ToString()), int.Parse(this.DateY.Value.ToString()), this.ServerName.Value.ToString());
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
                        series.LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;
                        series.Label.ResolveOverlappingMode = ResolveOverlappingMode.HideOverlapped;
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
                seriesXY.AxisY.Title.Text = "Availability";
                seriesXY.AxisX.Title.Text = "Date";
                seriesXY.AxisY.Title.Visible = true;
                seriesXY.AxisX.Title.Visible = true;
            }
            xrChart1.DataSource = dt;
        }

    }
}
