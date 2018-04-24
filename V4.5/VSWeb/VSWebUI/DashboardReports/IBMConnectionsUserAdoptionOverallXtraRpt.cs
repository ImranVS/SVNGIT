using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using DevExpress.XtraCharts;
using System.Data;

namespace VSWebUI.DashboardReports
{
    public partial class IBMConnectionsUserAdoptionOverallXtraRpt : DevExpress.XtraReports.UI.XtraReport
    {
        public IBMConnectionsUserAdoptionOverallXtraRpt()
        {
            InitializeComponent();
        }

        private void IBMConnectionsUserAdoptionOverallXtraRpt_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            DataTable dt = new DataTable();
            Series series = null;
            string servername = "";
            servername = this.ServerName.Value.ToString();
            dt = VSWebBL.DashboardBL.ConnectionsBL.Ins.GetTop5MostActiveCommunities(servername);
            string seriesname = "";
            string seriesarg = "";
            int seriesval = -1;
            if (dt.Rows.Count > 0)
            {
                xrChart1.Series.Clear();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    seriesarg = dt.Rows[i]["Name"].ToString();
                    seriesname = dt.Rows[i]["Type"].ToString();
                    seriesval = Convert.ToInt32(dt.Rows[i]["Total"].ToString());
                    series = xrChart1.Series[seriesname];
                    if (series == null)
                    {
                        series = new Series(seriesname, ViewType.StackedBar);
                        series.Points.Add(new SeriesPoint(seriesarg, seriesval));
                        series.SeriesPointsSortingKey = SeriesPointKey.Value_1;
                        series.ShowInLegend = true;
                        series.LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;
                        xrChart1.Series.Add(series);
                    }
                    else
                    {
                        series.Points.Add(new SeriesPoint(seriesarg, seriesval));
                        series.SeriesPointsSortingKey = SeriesPointKey.Value_1;
                        series.ShowInLegend = true;
                        series.LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;
                    }
                }
                UI uiobj = new UI();
                uiobj.RecalibrateChartAxes(xrChart1.Diagram, "Y", "int", "int"); 
                xrChart1.SeriesSorting = SortingMode.None;
                xrChart1.DataSource = dt;
            }

            xrChart2.Series.Clear();
            string statname = "COMMUNITY_TYPE_";
            dt = VSWebBL.DashboardBL.ConnectionsBL.Ins.GetStatByName(servername, statname, false);
            series = new Series("StatName", ViewType.Pie);

            series.ArgumentDataMember = dt.Columns["StatName"].ToString();
            series.LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;
            PieSeriesLabel label = (PieSeriesLabel)series.Label;
            label.TextPattern = "{A}: {VP:P0}";
            series.LegendTextPattern = "{A}: {V}";

            ValueDataMemberCollection seriesValueDataMembers = (ValueDataMemberCollection)series.ValueDataMembers;
            seriesValueDataMembers.AddRange(dt.Columns["StatValue"].ToString());
            xrChart2.Series.Add(series);

            xrChart2.DataSource = dt;

            xrChart3.Series.Clear();
            dt = VSWebBL.DashboardBL.ConnectionsBL.Ins.GetCommunitiesMonthly(servername);
            if (dt.Rows.Count > 0)
            {
                xrChart3.SeriesDataMember = "MName";
                xrChart3.SeriesTemplate.ArgumentDataMember = "MName";
                xrChart3.SeriesTemplate.ValueDataMembers.AddRange(new string[] { "Total" });
                XYDiagram d = (XYDiagram)xrChart3.Diagram;
                d.AxisX.Label.TextPattern = "{A:MMMM, yyyy}";
                xrChart3.DataSource = dt;
            }

            xrChart4.Series.Clear();
            dt = VSWebBL.DashboardBL.ConnectionsBL.Ins.GetCommunityItemsMonthly(servername);
            if (dt.Rows.Count > 0)
            {
                xrChart4.SeriesDataMember = "MName";
                xrChart4.SeriesTemplate.ArgumentDataMember = "MName";
                xrChart4.SeriesTemplate.ValueDataMembers.AddRange(new string[] { "Total" });
                ((BarSeriesView)xrChart4.SeriesTemplate.View).BarWidth = 0.5;
                XYDiagram d = (XYDiagram)xrChart4.Diagram;
                xrChart4.DataSource = dt;
                UI uiobj = new UI();
                uiobj.RecalibrateChartAxes(xrChart4.Diagram, "Y", "int", "int");
            }

            dt = new DataTable();
            dt = VSWebBL.DashboardBL.ConnectionsBL.Ins.GetCommunityItemsMonthlyByType(servername);
            series = null;
            xrChart5.Series.Clear();
            if (dt.Rows.Count > 0)
            {
                seriesname = "";
                seriesarg = "";
                seriesval = -1;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    seriesarg = dt.Rows[i]["MonthYear"].ToString();
                    seriesname = dt.Rows[i]["Type"].ToString();
                    seriesval = Convert.ToInt32(dt.Rows[i]["Total"].ToString());
                    series = xrChart5.Series[seriesname];
                    if (series == null)
                    {
                        series = new Series(seriesname, ViewType.StackedBar);
                        series.Points.Add(new SeriesPoint(seriesarg, seriesval));
                        //((StackedBarSeriesView)series.View).BarWidth = 8;
                        series.ShowInLegend = true;
                        series.LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;
                        xrChart5.Series.Add(series);
                    }
                    else
                    {
                        series.Points.Add(new SeriesPoint(seriesarg, seriesval));
                        series.ShowInLegend = true;
                        series.LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;
                    }
                }
                if (xrChart5.Series[0].Points.Count > 1)
                {
                    for (int i = 0; i < xrChart5.Series.Count; i++)
                    {
                        series = xrChart5.Series[i];
                        ((StackedBarSeriesView)series.View).BarWidth = 8;
                    }
                }
                xrChart5.DataSource = dt;
            }
        }

    }
}
