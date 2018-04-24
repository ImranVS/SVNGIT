using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;
using DevExpress.XtraCharts;
using System.IO;

namespace VSWebUI.DashboardReports
{
    public partial class DominoDiskAvgXtraRpt : DevExpress.XtraReports.UI.XtraReport
    {
        public DataTable dt;
        //public bool isSummary;
        public XRTableCell tblCell;
        public XRLabel lbl;
        public DominoDiskAvgXtraRpt()
        {
            InitializeComponent();
        }

        private void CurrentDiskSpaceChart_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            SetGraphForDiskSpace(this.ServerName.Value.ToString(), this, this.CurrentDiskSpaceChart, this.ServerType.Value.ToString());
        }

        public void SetGraphForDiskSpace(string serverName, DashboardReports.DominoDiskAvgXtraRpt report, XRChart CurrentDiskSpaceChart, string serverType)
        {
            bool isSummary;
            //XRChart CurrentDiskSpaceChart = (XRChart)report.FindControl("CurrentDiskSpaceChart", true);
            CurrentDiskSpaceChart.Series.Clear();
            //DataTable dt = VSWebBL.ReportsBL.ReportsBL.Ins.DominoDiskSpaceBL(serverName);
            dt = (DataTable)report.DataSource;
            CurrentDiskSpaceChart.DataSource = dt;
            isSummary = (bool)report.Parameters["IsSummary"].Value;
            double[] double1 = new double[dt.Rows.Count];
            double[] double2 = new double[dt.Rows.Count];
            string diskName = "";
            string srvName = "";

            Series series = null;
            if (!isSummary)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    diskName = dt.Rows[i]["DiskName"].ToString();
                    srvName = dt.Rows[i]["ServerName"].ToString();
                    //if (diskName == xrTableCell1.Text && srvName == xrLabel18.Text)
                    if (diskName == ((XRTableCell)report.FindControl("xrTableCell1",true)).Text && 
                        srvName == ((XRLabel)report.FindControl("xrLabel18",true)).Text)
                    {
                        if (series != null)
                        {
                            CurrentDiskSpaceChart.Series.Add(series);
                        }


                        series = new Series(dt.Rows[i]["DiskName"].ToString(), ViewType.Pie);

                        string val1 = dt.Rows[i]["PercentFree"].ToString();
                        string val2 = dt.Rows[i]["PercentUsed"].ToString();

                        if (val1 != "" && val2 != "")
                        {
                            double1[i] = Convert.ToDouble(dt.Rows[i]["PercentFree"].ToString());
                            double2[i] = Convert.ToDouble(dt.Rows[i]["PercentUsed"].ToString());

                            series.Points.Add(new SeriesPoint("Percent Free", double1[i]));
                            series.Points.Add(new SeriesPoint("Percent Used", double2[i]));
                            series.ShowInLegend = true;

                            series.LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;
                            PieSeriesLabel seriesLabel = (PieSeriesLabel)series.Label;
                            seriesLabel.Position = PieSeriesLabelPosition.Radial;
                            seriesLabel.BackColor = System.Drawing.Color.Transparent;
                            seriesLabel.TextColor = System.Drawing.Color.Black;
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    srvName = report.Parameters["ServerName"].Value.ToString();
                    if (series != null)
                    {
                        CurrentDiskSpaceChart.Series.Add(series);
                    }

                    series = new Series("All Disks", ViewType.Pie);

                    string val1 = dt.Rows[i]["PercentFree"].ToString();
                    string val2 = dt.Rows[i]["PercentUsed"].ToString();

                    if (val1 != "" && val2 != "")
                    {
                        double1[i] = Convert.ToDouble(dt.Rows[i]["PercentFree"].ToString());
                        double2[i] = Convert.ToDouble(dt.Rows[i]["PercentUsed"].ToString());

                        series.Points.Add(new SeriesPoint("Percent Free", double1[i]));
                        series.Points.Add(new SeriesPoint("Percent Used", double2[i]));
                        series.ShowInLegend = true;

                        series.LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;
                        PieSeriesLabel seriesLabel = (PieSeriesLabel)series.Label;
                        seriesLabel.Position = PieSeriesLabelPosition.Radial;
                        seriesLabel.BackColor = System.Drawing.Color.Transparent;
                        seriesLabel.TextColor = System.Drawing.Color.Black;
                    }
                }
            }
            if (series != null)
            {
                CurrentDiskSpaceChart.Series.Add(series);
            }
            for (int c = 0; c < CurrentDiskSpaceChart.Series.Count; c++)
            {
                if (c == 0)
                {
                    PiePointOptions seriesPointOptions = (PiePointOptions)series.LegendPointOptions;
                    series.LegendPointOptions.PointView = PointView.Argument;
                    CurrentDiskSpaceChart.Series[0].LegendPointOptions.PointView = PointView.Argument;
                    CurrentDiskSpaceChart.Series[0].ShowInLegend = true;
                    CurrentDiskSpaceChart.Legend.Visible = true;
                }
                else
                {
                    CurrentDiskSpaceChart.Series[c].ShowInLegend = false;
                }
            }   
        }

        public void SetReport(DashboardReports.DominoDiskAvgXtraRpt report)
        {
            double[] daysrem;
            double dcon = 0;
            string diskName = "";
            string serverName = "";
            string srvName = "";
            string sName = "";
            bool exactmatch;
            bool isSummary;
            string serverType = "";
            string serverTypeSQL = "";
            try
            {
                // this.ServerName.Value = "'azphxdom1/RPRWyatt','azphxdom2/RPRWyatt'";
                serverName = report.Parameters["ServerNameSQL"].Value.ToString();
                srvName = report.Parameters["ServerName"].Value.ToString();
                sName = srvName;
                exactmatch = (bool)report.Parameters["ExactMatch"].Value;
                if (srvName == "" || exactmatch)
                {
                    sName = serverName;
                }
                isSummary = (bool)report.Parameters["IsSummary"].Value;
                serverType = report.Parameters["ServerType"].Value.ToString();
                serverTypeSQL = report.Parameters["ServerTypeSQL"].Value.ToString();
                dt = VSWebBL.ReportsBL.ReportsBL.Ins.DominoDiskSpaceBL(sName, exactmatch, isSummary, serverTypeSQL);
                dt.Columns.Add("DiskConsumption", typeof(double));
                dt.Columns.Add("DaysRemain", typeof(string));
                dt.Columns.Add("ImgURL", typeof(string));
                dt.Columns.Add("FontColor", typeof(System.Drawing.Color));
                daysrem = new double[dt.Rows.Count];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (!isSummary)
                    {
                        diskName = dt.Rows[i]["DiskName"].ToString();
                        srvName = dt.Rows[i]["ServerName"].ToString();
                    }
                    else
                    {
                        diskName = "";
                        srvName = report.Parameters["ServerName"].Value.ToString();
                    }
                    dcon = VSWebBL.ReportsBL.ReportsBL.Ins.GetDominoDiskConsumption(serverType, srvName, diskName, exactmatch, isSummary);
                    if (dcon <= 0)
                    {
                        daysrem[i] = -1;
                        dt.Rows[i]["DaysRemain"] = "INF";
                        dt.Rows[i]["ImgURL"] = "~/images/up_g.gif";
                        dt.Rows[i]["FontColor"] = System.Drawing.Color.Green;
                    }
                    else
                    {
                        daysrem[i] = Math.Round(Convert.ToDouble(dt.Rows[i]["DiskFree"].ToString()) * 1024 / dcon, 0);
                        dt.Rows[i]["DaysRemain"] = Math.Round(Convert.ToDouble(dt.Rows[i]["DiskFree"].ToString()) * 1024 / dcon, 0).ToString();
                        dt.Rows[i]["ImgURL"] = "~/images/down_r.gif";
                        dt.Rows[i]["FontColor"] = System.Drawing.Color.Red;
                    }
                    dt.Rows[i]["DiskConsumption"] = Math.Abs(dcon);
                }
                //WriteServiceHistoryEntry(DateTime.Now.ToString() + " here #1");
                if (report.Parameters["DaysRem"].Value.ToString() != "")
                {
                    if (Convert.ToInt32(report.Parameters["DaysRem"].Value.ToString()) > -1)
                    {
                        DataRow row;
                        for (int i = dt.Rows.Count - 1; i >= 0; i--)
                        {
                            if (daysrem[i] > Convert.ToInt32(report.Parameters["DaysRem"].Value.ToString()) || daysrem[i] == -1)
                            {
                                row = dt.Rows[i];
                                row.Delete();
                            }
                        }
                    }
                }
                dt.AcceptChanges();
                report.DataSource = dt;
                if (!isSummary)
                {
                    ((XRTableCell)report.FindControl("xrTableCell1", true)).DataBindings.Add("Text", dt, "DiskName");
                    ((XRLabel)report.FindControl("xrLabel2", true)).DataBindings.Add("Text", dt, "DiskName");
                    ((XRLabel)report.FindControl("xrLabel18", true)).DataBindings.Add("Text", dt, "ServerName");
                    //WriteServiceHistoryEntry(DateTime.Now.ToString() + " here #2");
                }
                else
                {
                    ((XRLabel)report.FindControl("xrLabel22", true)).Text += " Summary";
                    if (exactmatch)
                    {
                        ((XRLabel)report.FindControl("xrLabel18", true)).Text = srvName;
                    }
                    else
                    {
                        ((XRLabel)report.FindControl("xrLabel18", true)).Text = "Server name contains '" + srvName + "'";
                    }
                    if (srvName == "" && (serverType == "" || serverType == "''" || serverType == "'All'"))
                    {
                        ((XRLabel)report.FindControl("xrLabel18", true)).Text = "All Servers";
                    }
                    else if (srvName == "" && !(serverType == "" || serverType == "''" || serverType == "'All'"))
                    {
                        ((XRLabel)report.FindControl("xrLabel18", true)).Text = "All " + serverType + " Servers";
                    }
                    ((XRLabel)report.FindControl("xrLabel3", true)).Text = "Overall";
                    ((XRLabel)report.FindControl("xrLabel2", true)).Text = " Summary";
                    ((XRLabel)report.FindControl("xrLabel4", true)).Visible = false;
                }
                ((XRLabel)report.FindControl("xrLabel6", true)).DataBindings.Add("Text", dt, "DiskFree");
                ((XRLabel)report.FindControl("xrLabel8", true)).DataBindings.Add("Text", dt, "DiskSize");
                ((XRLabel)report.FindControl("xrLabel14", true)).DataBindings.Add("Text", dt, "PercentFree");
                ((XRLabel)report.FindControl("xrLabel11", true)).DataBindings.Add("Text", dt, "DiskConsumption");
                ((XRLabel)report.FindControl("xrLabel11", true)).DataBindings.Add("ForeColor", dt, "FontColor");
                ((XRLabel)report.FindControl("xrLabel15", true)).DataBindings.Add("ForeColor", dt, "FontColor");
                ((XRPictureBox)report.FindControl("AvgIndicatorPic", true)).DataBindings.Add("ImageUrl", dt, "ImgURL");
                ((XRLabel)report.FindControl("xrLabel16", true)).DataBindings.Add("Text", dt, "DaysRemain");
                //WriteServiceHistoryEntry(DateTime.Now.ToString() + " here #3");
            }
            catch (Exception ex)
            {
                WriteServiceHistoryEntry(DateTime.Now.ToString() + " The following error has occurred in SetReport: " + ex.Message);
            }
        }

        private void DominoDiskAvgXtraRpt_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            SetReport(this);
            //SetGraphForDiskSpace(this.Parameters["ServerName"].Value.ToString(), this);
        }

        //2/4/2014 NS added for sched reports
        protected void WriteServiceHistoryEntry(string strMsg)
        {
            bool appendMode = true;
            string ServiceLogDestination = AppDomain.CurrentDomain.BaseDirectory.ToString() + "/log_files/ScheduledReports_Service.txt";
            try
            {
                StreamWriter sw = new StreamWriter(ServiceLogDestination, appendMode, System.Text.Encoding.Unicode);
                sw.WriteLine(strMsg);
                sw.Close();
                sw = null;
            }
            catch
            {
            }
            finally
            {
                GC.Collect();
            }
        }
    }
}
