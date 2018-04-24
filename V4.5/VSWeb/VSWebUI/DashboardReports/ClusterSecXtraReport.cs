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
    public partial class ClusterSecXtraReport : DevExpress.XtraReports.UI.XtraReport
    {
        public ClusterSecXtraReport()
        {
            InitializeComponent();
        }

       

        private void ClusterSecXtraReport_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {  
            DataTable dt = new DataTable();
            dt = VSWebBL.ReportsBL.XsdBL.Ins.getClusterSecOnQBL(this.ServerName.Value.ToString(), this.StartDate.Value.ToString());
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
                        series.ArgumentDataMember = dt.Columns["Hour"].ToString();
                        series.ArgumentScaleType = ScaleType.Numerical;
                        series.ValueScaleType = ScaleType.Numerical;
                    }
                    srvName = dt.Rows[i]["ServerName"].ToString();
                    if (series != null)
                    {
                        series.Points.Add(new SeriesPoint(dt.Rows[i]["Hour"].ToString(), Convert.ToInt32(dt.Rows[i]["StatValue"].ToString())));
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
                seriesXY.AxisY.Title.Text = "Seconds";
                seriesXY.AxisX.Title.Text = "Hour";
                seriesXY.AxisY.Title.Visible = true;
                seriesXY.AxisX.Title.Visible = true;
            }
            xrChart1.DataSource = dt;
            MonthYearLabel.Text = "Report for " + ((DateTime)this.StartDate.Value).ToShortDateString();
        }

        private void xrChart1_BoundDataChanged(object sender, EventArgs e)
        {
            UI uiobj = new UI();
            uiobj.RecalibrateChartAxes(xrChart1.Diagram, "X");
        }
    }
}
