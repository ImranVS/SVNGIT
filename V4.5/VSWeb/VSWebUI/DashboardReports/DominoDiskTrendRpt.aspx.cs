using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using DevExpress.XtraCharts;
using DevExpress.XtraReports.UI;

namespace VSWebUI.DashboardReports
{
    public partial class DominoDiskTrendRpt : System.Web.UI.Page
    {
        public bool exactmatch;
        //2/2/2015 NS added for VSPLUS-1370 
        string selectedType = "";
        string selectedTypeSQL = "";
        private static DominoDiskTrendRpt _self = new DominoDiskTrendRpt();

        public static DominoDiskTrendRpt Ins
        {
            get
            {
                return _self;
            }
        }
        DashboardReports.DominoDiskAvgXtraRpt report;

        public DashboardReports.DominoDiskAvgXtraRpt GetRpt()
        {
            string srvName = "";
            bool isSummary = false;
            bool exactmatch = false;
            string srvType = "";
            try
            {
                report = new DashboardReports.DominoDiskAvgXtraRpt();
                report.Parameters["ServerName"].Value = srvName;
                report.Parameters["ServerNameSQL"].Value = srvName;
                report.Parameters["DaysRem"].Value = -1;
                report.Parameters["ExactMatch"].Value = exactmatch;
                report.Parameters["IsSummary"].Value = isSummary;
                report.Parameters["ServerType"].Value = srvType;
                report.Parameters["ServerTypeSQL"].Value = srvType;
                report.SetReport(report);
                //XRChart CurrentDiskSpaceChart = new XRChart();
                //CurrentDiskSpaceChart = (XRChart)report.FindControl("CurrentDiskSpaceChart",true);
                //report.SetGraphForDiskSpace(srvName,report,CurrentDiskSpaceChart);
            }
            catch (Exception ex)
            {
                WriteServiceHistoryEntry(DateTime.Now.ToString() + " The following error has occurred in GetRpt: " + ex.Message);
            }
            return report;
        }

        public void SetGraphForDiskSpace(DashboardReports.DominoDiskAvgXtraRpt report, DataTable dt, bool isSummary, 
            XRChart CurrentDiskSpaceChart)
        {
            //2/24/2014 NS added
            try
            {
                CurrentDiskSpaceChart.Series.Clear();
                CurrentDiskSpaceChart.DataSource = dt;
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
                        //if (diskName == report.FindControl("xrTableCell1", true).Text && srvName == report.FindControl("xrLabel18", true).Text)
                        //{
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
                                //PieSeriesView seriesView = (PieSeriesView)series.View;
                                //seriesView.Titles.Add(new SeriesTitle());
                                //seriesView.Titles[0].Dock = ChartTitleDockStyle.Bottom;
                                //seriesView.Titles[0].Text = series.Name.ToString();
                                //seriesView.Titles[0].Visible = true;
                                //seriesView.Titles[0].WordWrap = true;
                            }
                        //}
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
            catch (Exception ex)
            {
                WriteServiceHistoryEntry(DateTime.Now.ToString() + " The following error has occurred in SetGraph: " + ex.Message);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Report();   
            if (!IsPostBack)
            {
                fillcombo("");
                fillservertypelist();
            }
            else
            {
                fillcombo(selectedTypeSQL);
            }
        }

        public void fillcombo(string sType)
        {
            DataTable dt = new DataTable();
            dt = VSWebBL.ReportsBL.ReportsBL.Ins.fillDominoDiskAvgConsumption(sType);
            /*
            ServerListFilterComboBox.DataSource = dt;
            ServerListFilterComboBox.TextField = "ServerName";
            ServerListFilterComboBox.ValueField = "ServerName";
            ServerListFilterComboBox.DataBind(); 
             */
            ServerListFilterListBox.DataSource = dt;
            ServerListFilterListBox.TextField = "ServerName";
            ServerListFilterListBox.ValueField = "ServerName";
            ServerListFilterListBox.DataBind();
        }

        public void fillservertypelist()
        {
            DataTable dt = new DataTable();
            dt = VSWebBL.ReportsBL.ReportsBL.Ins.fillDominoDiskAvgConsumptionServerTypes();
            ServerTypeFilterListBox.DataSource = dt;
            ServerTypeFilterListBox.TextField = "ServerType";
            ServerTypeFilterListBox.ValueField = "ServerType";
            ServerTypeFilterListBox.DataBind();
        }

        public void Report()
        {
            int daysrem = -1;
            string selectedServer = "";
            exactmatch = false;
            /*
            if (this.ServerListFilterComboBox.SelectedIndex >= 0)
            {
                selectedServer = this.ServerListFilterComboBox.SelectedItem.Value.ToString();
            } */
            if (this.ServerFilterTextBox.Text != "")
            {
                selectedServer = this.ServerFilterTextBox.Text;
            }
            if (this.ServerListFilterListBox.SelectedItems.Count > 0)
            {
                selectedServer = "";
                for (int i = 0; i < this.ServerListFilterListBox.SelectedItems.Count; i++)
                {
                    selectedServer += "'" + this.ServerListFilterListBox.SelectedItems[i].Text + "'" + ",";
                }
                exactmatch = true;
                try
                {
                    selectedServer = selectedServer.Substring(0, selectedServer.Length - 1);
                }
                catch
                {
                    selectedServer = "";     // throw ex; 
                    exactmatch = false;
                }
                finally { }
            }
            //2/2/2015 NS added for VSPLUS-1370
            if (this.ServerTypeFilterListBox.SelectedItems.Count > 0)
            {
                selectedType = "";
                selectedTypeSQL = "";
                for (int i = 0; i < this.ServerTypeFilterListBox.SelectedItems.Count; i++)
                {
                    selectedType += this.ServerTypeFilterListBox.SelectedItems[i].Text + ",";
                    selectedTypeSQL += "'" + this.ServerTypeFilterListBox.SelectedItems[i].Text + "'" + ",";
                }
                try
                {
                    selectedType = selectedType.Substring(0, selectedType.Length - 1);
                    selectedTypeSQL = selectedTypeSQL.Substring(0, selectedTypeSQL.Length - 1);
                }
                catch
                {
                    selectedType = "";     // throw ex; 
                    selectedTypeSQL = "";
                }
                finally { }
            }
            if (this.NumberDaysTextBox.Text.ToString() != "")
            {
                daysrem = Convert.ToInt32(this.NumberDaysTextBox.Value.ToString());
            }
            DashboardReports.DominoDiskAvgXtraRpt report = new DashboardReports.DominoDiskAvgXtraRpt();
            report.Parameters["ServerName"].Value = selectedServer;
            report.Parameters["ServerNameSQL"].Value = selectedServer;
            report.Parameters["DaysRem"].Value = daysrem;
            report.Parameters["ExactMatch"].Value = exactmatch;
            //2/2/2015 NS added for VSPLUS-1370
            report.Parameters["ServerType"].Value = selectedType;
            report.Parameters["ServerTypeSQL"].Value = selectedTypeSQL;
            report.CreateDocument();
            ASPxDocumentViewer1.Report = report;
            ASPxDocumentViewer1.DataBind();
        }

        protected void ResetButton_Click(object sender, EventArgs e)
        {
            //this.ServerListFilterComboBox.SelectedIndex = -1;
            this.ServerFilterTextBox.Text = "";
            this.NumberDaysTextBox.Text = "";
            this.ServerListFilterListBox.UnselectAll();
            //2/2/2015 NS added for VSPLUS-1370 
            this.ServerTypeFilterListBox.UnselectAll();
            fillcombo("");
            DashboardReports.DominoDiskAvgXtraRpt report = new DashboardReports.DominoDiskAvgXtraRpt();
            report.Parameters["ServerName"].Value = "";
            report.Parameters["ServerNameSQL"].Value = "";
            report.Parameters["DaysRem"].Value = "";
            report.Parameters["ServerType"].Value = "";
            report.Parameters["ServerTypeSQL"].Value = "";
            report.CreateDocument();
            ASPxDocumentViewer1.Report = report;
            ASPxDocumentViewer1.DataBind();
        }

        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (Request.QueryString["M"] == "C" && Request.QueryString["M"].ToString() != "")
            {
                this.MasterPageFile = "~/Reports.Master";

            }
            else
            {
                this.MasterPageFile = "~/Reports.Master";

            }
        }

        protected void ReptBtn_Click(object sender, EventArgs e)
        {

            Response.Redirect("~/Configurator/Reports.aspx?M=" + Request.QueryString["M"], false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
            Context.ApplicationInstance.CompleteRequest();
        }

        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            Report();
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