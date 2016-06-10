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
    public partial class SrvTransPerMinXtraRpt : DevExpress.XtraReports.UI.XtraReport
    {
        public SrvTransPerMinXtraRpt()
        {
            InitializeComponent();
        }

        private void SrvTransPerMinXtraRpt_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            //SrvTransPerMinRptDSTableAdapters.DominoSummaryStatsTableAdapter dominoAdapter = new SrvTransPerMinRptDSTableAdapters.DominoSummaryStatsTableAdapter();
           // dominoAdapter.Fill(this.srvTransPerMinRptDS1.DominoSummaryStats, (int)this.DateM.Value, (int)this.DateY.Value, this.ServerName.Value.ToString());

            DataTable dt = new DataTable();
            dt = VSWebBL.ReportsBL.XsdBL.Ins.SrvTransPerMinRptDSBL(int.Parse(this.DateM.Value.ToString()), int.Parse(this.DateY.Value.ToString()), this.ServerName.Value.ToString());
            Series series = null;
            string srvName = "";

            if (dt.Rows.Count > 0)
            {
                //xrChart1.Series.Clear();
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
                seriesXY.AxisX.Label.ResolveOverlappingOptions.AllowHide = true;
                seriesXY.AxisY.Title.Text = "Transactions";
                seriesXY.AxisX.Title.Text = "Date";
                seriesXY.AxisY.Title.Visible = true;
                seriesXY.AxisX.Title.Visible = true;
            }
            xrChart1.DataSource = dt;
            MonthYearLabel.Text = "Report for " + this.MonthYear.Value.ToString();
        }

    }
}
