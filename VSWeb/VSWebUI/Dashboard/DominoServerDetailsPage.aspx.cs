using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DevExpress.XtraCharts;

namespace VSWebUI.Configurator
{
    public partial class DominoServerDetailsPage : System.Web.UI.Page
    {
        //string strDeviceName = "azphxweb1/RPRWyatt";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //DataTable dt = VSWebBL.ConfiguratorBL.DominoServerDetails_BL.Ins.GetPerHourDetails();
                //WebChartControl1.DataSource = dt;

                //WebChartControl1.DataBind();
                servernameASPxLabel.Text = Request.QueryString["Name"];

                SetGraph("hh", servernameASPxLabel.Text);
                SetGraphForCPU("hh", servernameASPxLabel.Text);
                SetGraphForMemory("hh", servernameASPxLabel.Text);
                SetGraphForUsers("hh", servernameASPxLabel.Text);
                SetGraphForDiskSpace(servernameASPxLabel.Text);
            }
        }

        protected void performanceASPxRadioBttLst_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (performanceASPxRadioBttLst.SelectedItem.Text == "Per Day")
            //{
            //    DataTable dt = VSWebBL.ConfiguratorBL.DominoServerDetails_BL.Ins.GetPerDayDetails();
            //    WebChartControl1.DataSource = dt;

            //    WebChartControl1.DataBind();
            //}
            //else
            //{
            //    DataTable dt = VSWebBL.ConfiguratorBL.DominoServerDetails_BL.Ins.GetPerHourDetails();
            //    WebChartControl1.DataSource = dt;

            //    WebChartControl1.DataBind();
            //}
            SetGraph(performanceASPxRadioBttLst.Value.ToString(), servernameASPxLabel.Text);
        }

        protected void cpuASPxRadioButtonList_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (performanceASPxRadioBttLst.SelectedItem.Text == "Per Day")
            //{
            //    DataTable dt = VSWebBL.ConfiguratorBL.DominoServerDetails_BL.Ins.GetPerDayDetails();
            //    WebChartControl1.DataSource = dt;

            //    WebChartControl1.DataBind();
            //}
            //else
            //{
            //    DataTable dt = VSWebBL.ConfiguratorBL.DominoServerDetails_BL.Ins.GetPerHourDetails();
            //    WebChartControl1.DataSource = dt;

            //    WebChartControl1.DataBind();
            //}
            SetGraphForCPU(cpuASPxRadioButtonList.Value.ToString(), servernameASPxLabel.Text);
        }       

        public void SetGraph(string paramGraph, string DeviceName)
        {
            DataTable dt = VSWebBL.ConfiguratorBL.DominoServerDetails_BL.Ins.SetGraph(paramGraph, DeviceName);

            Series series = null;

            if (series == null)
            {
                series = new Series("DominoServer", ViewType.Spline);
                series.Visible = true;
                series.ArgumentDataMember = dt.Columns["Date"].ToString();

                ValueDataMemberCollection seriesValueDataMembers = (ValueDataMemberCollection)series.ValueDataMembers;
                seriesValueDataMembers.AddRange(dt.Columns["StatValue"].ToString());
                WebChartControl1.Series.Add(series);

                XYDiagram seriesXY = (XYDiagram)WebChartControl1.Diagram;
                seriesXY.AxisX.Title.Text = "Time";
                seriesXY.AxisX.Title.Visible = true;
                seriesXY.AxisY.Title.Text = "Performance";
                seriesXY.AxisY.Title.Visible = true;

                WebChartControl1.Legend.Visible = false;
                //WebChartControl1.Series[0].ValueDataMembers = dt.Columns["StatValue"].ToString();
                WebChartControl1.DataSource = dt;
                WebChartControl1.DataBind();
            }
        }

        public void SetGraphForCPU(string paramGraph, string serverName)
        {
            DataTable dt = VSWebBL.ConfiguratorBL.DominoServerDetails_BL.Ins.SetGraphForCPU(paramGraph, serverName);
            Series series = null;

            if (series == null)
            {
                series = new Series("DominoServer", ViewType.Spline);
                series.Visible = true;
                series.ArgumentDataMember = dt.Columns["Date"].ToString();

                ValueDataMemberCollection seriesValueDataMembers = (ValueDataMemberCollection)series.ValueDataMembers;
                seriesValueDataMembers.AddRange(dt.Columns["StatValue"].ToString());
                cpuWebChartControl.Series.Add(series);

                XYDiagram seriesXY = (XYDiagram)cpuWebChartControl.Diagram;
                seriesXY.AxisX.Title.Text = "Time";
                seriesXY.AxisX.Title.Visible = true;
                seriesXY.AxisY.Title.Text = "StatValue";
                seriesXY.AxisY.Title.Visible = true;

                cpuWebChartControl.Legend.Visible = false;
                //WebChartControl1.Series[0].ValueDataMembers = dt.Columns["StatValue"].ToString();
                cpuWebChartControl.DataSource = dt;
                cpuWebChartControl.DataBind();
            }
            //cpuWebChartControl.DataSource = dt;
            //cpuWebChartControl.DataBind();
        }

        public void SetGraphForMemory(string paramGraph, string serverName)
        {
            DataTable dt = VSWebBL.ConfiguratorBL.DominoServerDetails_BL.Ins.SetGraphForMemory(paramGraph, serverName);
            Series series = null;

            if (series == null)
            {
                series = new Series("DominoServer", ViewType.Spline);
                series.Visible = true;
                series.ArgumentDataMember = dt.Columns["Date"].ToString();

                ValueDataMemberCollection seriesValueDataMembers = (ValueDataMemberCollection)series.ValueDataMembers;
                seriesValueDataMembers.AddRange(dt.Columns["StatValue"].ToString());
                WebChartControl4.Series.Add(series);

                XYDiagram seriesXY = (XYDiagram)WebChartControl4.Diagram;
                seriesXY.AxisX.Title.Text = "Time";
                seriesXY.AxisX.Title.Visible = true;
                seriesXY.AxisY.Title.Text = "Memory Use";
                seriesXY.AxisY.Title.Visible = true;

                WebChartControl4.Legend.Visible = false;
                //WebChartControl1.Series[0].ValueDataMembers = dt.Columns["StatValue"].ToString();
                WebChartControl4.DataSource = dt;
                WebChartControl4.DataBind();
            }
            //WebChartControl4.DataSource = dt;
            //WebChartControl4.DataBind();
        }

        public void SetGraphForUsers(string paramGraph, string serverName)
        {
            DataTable dt = VSWebBL.ConfiguratorBL.DominoServerDetails_BL.Ins.SetGraphForUsers(paramGraph, serverName);
            Series series = null;

            if (series == null)
            {
                series = new Series("DominoServer", ViewType.Line);
                series.Visible = true;
                series.ArgumentDataMember = dt.Columns["Date"].ToString();

                ValueDataMemberCollection seriesValueDataMembers = (ValueDataMemberCollection)series.ValueDataMembers;
                seriesValueDataMembers.AddRange(dt.Columns["StatValue"].ToString());

                //XYDiagram seriesXY = (XYDiagram)seriesaMembers;
                //seriesXY.AxisX.Title.Text = "Time";

                usersWebChartControl.Series.Add(series);

                XYDiagram seriesXY = (XYDiagram)usersWebChartControl.Diagram;
                seriesXY.AxisX.Title.Text = "Time";
                seriesXY.AxisX.Title.Visible = true;
                seriesXY.AxisY.Title.Text = "No. of Users";
                seriesXY.AxisY.Title.Visible = true;

                usersWebChartControl.Legend.Visible = false;
                //WebChartControl1.Series[0].ValueDataMembers = dt.Columns["StatValue"].ToString();
                usersWebChartControl.DataSource = dt;
                usersWebChartControl.DataBind();
            }
            //usersWebChartControl.DataSource = dt;
            //usersWebChartControl.DataBind();
        }

        public void SetGraphForDiskSpace(string serverName)
        {
            DataTable dt = VSWebBL.ConfiguratorBL.DominoServerDetails_BL.Ins.SetGraphForDiskSpace(serverName);
            DiskSpaceWebChartControl.DataSource = dt;

            double[] double1 = new double[dt.Rows.Count];
            double[] double2 = new double[dt.Rows.Count];            
            
            Series series = null;

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (series != null)
                    {
                            DiskSpaceWebChartControl.Series.Add(series);                            
                            DiskSpaceWebChartControl.DataBind();
                    }
                    series = new Series(dt.Rows[i]["DiskName"].ToString(), ViewType.Pie);
                    if(dt.Rows[i]["DiskFree"]!=""&&dt.Rows[i]["DiskFree"]!=null)
                    double1[i] = Convert.ToDouble(dt.Rows[i]["DiskFree"].ToString());
                    if (dt.Rows[i]["DiskUsed"] != "" && dt.Rows[i]["DiskUsed"] != null)
                    double2[i] = Convert.ToDouble(dt.Rows[i]["DiskUsed"].ToString());
                    series.Points.Add(new SeriesPoint("DiskFree", double1[i]));
                    series.Points.Add(new SeriesPoint("DiskUsed", double2[i]));

                    //series.Label.Visible = true;
                    series.LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;
                    PieSeriesLabel seriesLabel = (PieSeriesLabel)series.Label;
                    seriesLabel.Position = PieSeriesLabelPosition.Radial;
                    seriesLabel.BackColor = System.Drawing.Color.Transparent;
                    seriesLabel.TextColor = System.Drawing.Color.Black;

                    PieSeriesView seriesView = (PieSeriesView)series.View;
                    seriesView.Titles.Add(new SeriesTitle());
                    seriesView.Titles[0].Dock = ChartTitleDockStyle.Bottom;

                    if (i == 0)
                    {
                        PiePointOptions seriesPointOptions = (PiePointOptions)series.LegendPointOptions;
                        series.LegendPointOptions.PointView = PointView.Argument;
                        DiskSpaceWebChartControl.Series[i].ShowInLegend = true;
                        DiskSpaceWebChartControl.Series[i].LegendPointOptions.PointView = PointView.Argument;
                        DiskSpaceWebChartControl.Series[i].ShowInLegend = true;
                        DiskSpaceWebChartControl.Legend.Visible = true;
                    }
                    else
                    {
                        DiskSpaceWebChartControl.Series[i].LegendPointOptions.PointView = PointView.Argument;
                        DiskSpaceWebChartControl.Series[i].ShowInLegend = false;
                    }
                    
                    seriesView.Titles[0].Text = series.Name;
                    seriesView.Titles[0].Visible = true;
                    seriesView.Titles[0].WordWrap = true;                    
                }                
            }
            if (series != null)
            {
                DiskSpaceWebChartControl.Series.Add(series);
                series.LegendPointOptions.PointView = PointView.Argument;
                DiskSpaceWebChartControl.DataBind();
                //DiskSpaceWebChartControl.Series[i].LegendPointOptions.PointView = PointView.Argument;
                //DiskSpaceWebChartControl.Series[i].ShowInLegend = false;
            }            
        }

        protected void memASPxRadioButtonList_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetGraphForMemory(memASPxRadioButtonList.Value.ToString(), servernameASPxLabel.Text);
        }

        protected void usersASPxRadioButtonList_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetGraphForUsers(usersASPxRadioButtonList.Value.ToString(), servernameASPxLabel.Text);
        }

        protected void servernameASPxComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        
    }
}