using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;
using VSWebBL;
using DevExpress.XtraCharts;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using System.Configuration;
namespace VSWebUI.DashboardReports
{
    //6/3/2016 Sowjanya added for VSPLUS-2895
    public partial class ConnectionstestXtraRpt : DevExpress.XtraReports.UI.XtraReport
    {
        public ConnectionstestXtraRpt()
        {
            InitializeComponent();
        }

        private void ConnectionstestXtraRpt_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {

            DataTable dt = new DataTable();
            dt = VSWebBL.ReportsBL.XsdBL.Ins.ConnectionTags(this.Name.Value.ToString(), this.ServerName.Value.ToString());
            if (dt.Rows.Count > 0)
            {
                this.DataSource = dt;
                xrLabel2.DataBindings.Add("Text", dt, "Name");
                mtDiskName.DataBindings.Add("Text", dt, "Tag");
                mtPercentFree.DataBindings.Add("Text", dt, "Count");
                GroupField groupField = new GroupField("Name");
                GroupHeader2.GroupFields.Add(groupField);
            }
            
          
            DataTable t1 = new DataTable();
            dt = VSWebBL.ReportsBL.XsdBL.Ins.GetTagsCount(this.Name.Value.ToString(),this.ServerName.Value.ToString());
            if (dt.Rows.Count > 0)
            {
            
                     BuildChart(0, dt.Rows.Count, dt, xrChart1);
               
            }
        }

       

        public void BuildChart(int countS, int countE, DataTable dt, XRChart xrChart1)
        {
          
            xrChart1.Series.Clear();
            Series series = null;
            for (int i = countS; i < countE; i++)
            {
                if (series == null)
                {
                    series = new Series("tag", ViewType.Bar);
                    series.ArgumentDataMember = dt.Columns["tag"].ToString();
                    series.ArgumentScaleType = ScaleType.Qualitative;
                    series.ValueScaleType = ScaleType.Numerical;
                }
                if (series != null)
                {
                    series.Points.Add(new SeriesPoint(dt.Rows[i]["tag"].ToString(), Convert.ToInt32(dt.Rows[i]["tagcount"].ToString())));
                    xrChart1.Series.Add(series);
                    series = null;
                }
            }
            ((XYDiagram)xrChart1.Diagram).PaneLayoutDirection = PaneLayoutDirection.Horizontal;
            XYDiagram seriesXY = (XYDiagram)xrChart1.Diagram;
            seriesXY.EnableAxisXScrolling = true;
            seriesXY.EnableAxisYScrolling = true;
            seriesXY.AxisX.Label.ResolveOverlappingOptions.AllowRotate = false;
            seriesXY.AxisX.Label.ResolveOverlappingOptions.AllowHide = false;
            seriesXY.AxisY.Title.Text = "Tag Count";
            seriesXY.AxisY.Title.Visible = true;
            seriesXY.AxisX.Title.Visible = false;
            xrChart1.DataSource = dt;
            UI uiobj = new UI();
            uiobj.RecalibrateChartAxes(xrChart1.Diagram, "Y", "int", "int");
        }
    }
}
