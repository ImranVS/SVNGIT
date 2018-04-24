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
    public partial class DominoSrvCPUUtilXtraRpt : DevExpress.XtraReports.UI.XtraReport
    {
        public DominoSrvCPUUtilXtraRpt()
        {
            InitializeComponent();
        }

        private void DominoSrvCPUUtilXtraRpt_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
          //  DominoSrvCPUUtilRptDSTableAdapters.DominoCPUUtilDTTableAdapter dominocpuAdapter = new DominoSrvCPUUtilRptDSTableAdapters.DominoCPUUtilDTTableAdapter();
           // dominocpuAdapter.Fill(this.dominoSrvCPUUtilRptDS1.DominoCPUUtilDT, this.ServerName.Value.ToString());
           
            DataTable dt=new DataTable();
            dt=VSWebBL.ReportsBL.XsdBL.Ins.DominoSrvCpuutilBL(this.ServerName.Value.ToString());
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
                            DominoCPUChart.Series.Add(series);
                        }
                        series = new Series(dt.Rows[i]["ServerName"].ToString(), ViewType.Line);
                        series.ArgumentDataMember = dt.Columns["Hour"].ToString();
                        series.ArgumentScaleType = ScaleType.Numerical;
                        series.ValueScaleType = ScaleType.Numerical;
                    }
                    srvName = dt.Rows[i]["ServerName"].ToString();
                    if (series != null)
                    {
                        series.Points.Add(new SeriesPoint(dt.Rows[i]["Hour"].ToString(), Convert.ToDouble(dt.Rows[i]["StatValue"].ToString())));
                    }
                }
                if (series != null)
                {
                    DominoCPUChart.Series.Add(series);
                }

                ((XYDiagram)DominoCPUChart.Diagram).PaneLayoutDirection = PaneLayoutDirection.Horizontal;
                XYDiagram seriesXY = (XYDiagram)DominoCPUChart.Diagram;
                seriesXY.AxisX.Label.ResolveOverlappingOptions.AllowRotate = true;
                seriesXY.AxisX.Label.ResolveOverlappingOptions.AllowHide = false;
                seriesXY.AxisX.GridSpacing = 1;
                seriesXY.AxisY.Title.Text = "Percentage";
                seriesXY.AxisX.Title.Text = "Hour";
                seriesXY.AxisY.Title.Visible = true;
                seriesXY.AxisX.Title.Visible = true;

                double min = Convert.ToDouble(((XYDiagram)DominoCPUChart.Diagram).AxisX.Range.MinValue);
                double max = Convert.ToDouble(((XYDiagram)DominoCPUChart.Diagram).AxisX.Range.MaxValue);

                int gs = (int)((max - min) / 5);

                if (gs == 0)
                {
                    gs = 1;
                    seriesXY.AxisX.GridSpacingAuto = false;
                    seriesXY.AxisX.GridSpacing = gs;
                }
            }
            DominoCPUChart.DataSource = dt;
          
        }     
      
    }
}
