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
    public partial class DeviceUptimeXtraRpt : DevExpress.XtraReports.UI.XtraReport
    {
        public DeviceUptimeXtraRpt()
        {
            InitializeComponent();
        }

        private void DeviceUptimeXtraRpt_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            //2/4/2015 NS modified for VSPLUS-1370
           // DeviceUptimeRptDSTableAdapters.StatusTableAdapter dominoAdapter = new DeviceUptimeRptDSTableAdapters.StatusTableAdapter();
           // dominoAdapter.Fill(this.deviceUptimeRptDS1.Status, (System.DateTime)this.DateVal.Value, this.ServerName.Value.ToString());
            DataTable dt = new DataTable();
            dt = VSWebBL.ReportsBL.XsdBL.Ins.DeviceuptimeBL(this.ServerName.Value.ToString(), (DateTime)this.DateVal.Value,
                this.ServerType.Value.ToString());
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
                        series.ArgumentDataMember = dt.Columns["Hour"].ToString();
                        series.ArgumentScaleType = ScaleType.Numerical;
                        series.ValueScaleType = ScaleType.Numerical;
                    }
                    srvName = dt.Rows[i]["DeviceName"].ToString();
                    if (series != null)
                    {
                        series.Points.Add(new SeriesPoint(Convert.ToInt32(dt.Rows[i]["Hour"].ToString()), Convert.ToDouble(dt.Rows[i]["UpPercent"].ToString())));
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
                seriesXY.AxisX.Title.Text = "Hour";
                seriesXY.AxisY.Title.Visible = true;
                seriesXY.AxisX.Title.Visible = true;
            }
            xrChart1.DataSource = dt;
            MonthYearLabel.Text = "Report for " + ((DateTime)this.DateVal.Value).ToShortDateString();
        }

    }
}
