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
    public partial class DominoDiskHealthXtraRpt : DevExpress.XtraReports.UI.XtraReport
    {
        public DominoDiskHealthXtraRpt()
        {
            InitializeComponent();
        }

        private void DominoDiskHealthXtraRpt_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {

            /*
            DominoDiskHealthRptDSTableAdapters.DominoDiskSpaceTableAdapter dominodiskAdapter = new DominoDiskHealthRptDSTableAdapters.DominoDiskSpaceTableAdapter();
            dominodiskAdapter.Fill(this.dominoDiskHealthRptDS1.DominoDiskSpace, this.ServerName.Value.ToString());
            if (this.Parameters["ServerName"].Value.ToString() != "")
            {
                DominoDiskHealthRptDSTableAdapters.DominoDiskSpaceSrvTableAdapter dominodisksrvAdapter = new DominoDiskHealthRptDSTableAdapters.DominoDiskSpaceSrvTableAdapter();
                dominodisksrvAdapter.Fill(this.dominoDiskHealthRptDS1.DominoDiskSpaceSrv, this.ServerName.Value.ToString());
                //DataTable d = new DataTable();

                //d = VSWebBL.ReportsBL.XsdBL.Ins.DominoDiskSpaceSrcBL(this.ServerName.Value.ToString());

                xrChart1.Visible = true;
            }
             */

            DataTable dt = new DataTable();
            dt = VSWebBL.ReportsBL.XsdBL.Ins.DominoDiskSpaceBL(this.ServerName.Value.ToString());
            if (dt.Rows.Count > 0)
            {
                this.DataSource = dt;
                xrLabel2.DataBindings.Add("Text", dt, "ServerName");
                mtDiskName.DataBindings.Add("Text", dt, "DiskName");
                mtPercentFree.DataBindings.Add("Text", dt, "PercentFree");
                mtDiskSize.DataBindings.Add("Text", dt, "DiskSize");
                mtDiskFree.DataBindings.Add("Text", dt, "DiskFree");
                //6/17/2014 NS commented out
                //mtPercentUtil.DataBindings.Add("Text", dt, "PercentUtilization");
                //mtAvgQueueLength.DataBindings.Add("Text", dt, "AverageQueueLength");
                mtUpdated.DataBindings.Add("Text", dt, "Updated");
                mtThreshold.DataBindings.Add("Text", dt, "Threshold");
                if (this.Parameters["ServerName"].Value.ToString() != "")
                {
                    xrChart1.Visible = true;
                }
            }
            //6/3/2015 NS added for VSPLUS-1828
            int pageSize = 25;
            DataTable t1 = new DataTable();
            dt = VSWebBL.DashboardBL.DominoServerDetailsBL.Ins.SetGraphForDiskSpace(this.ServerNameSQL.Value.ToString(), "", "");
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows.Count > pageSize && dt.Rows.Count <= pageSize * 2)
                {
                    t1 = dt.Clone();
                    for (int i = 0; i <= pageSize; i++)
                    {
                        t1.ImportRow(dt.Rows[i]);
                    }
                    BuildChart(t1, xrChart2);
                    t1 = dt.Clone();
                    for (int i = pageSize + 1; i < dt.Rows.Count; i++)
                    {
                        t1.ImportRow(dt.Rows[i]);
                    }
                    xrChart3.Visible = true;
                    BuildChart(t1, xrChart3);
                }
                else if (dt.Rows.Count > pageSize * 2 && dt.Rows.Count <= pageSize * 3)
                {
                    t1 = dt.Clone();
                    for (int i = 0; i <= pageSize; i++)
                    {
                        t1.ImportRow(dt.Rows[i]);
                    }
                    BuildChart(t1, xrChart2);
                    t1 = dt.Clone();
                    for (int i = pageSize + 1; i <= pageSize * 2; i++)
                    {
                        t1.ImportRow(dt.Rows[i]);
                    }
                    xrChart3.Visible = true;
                    BuildChart(t1, xrChart3);
                    t1 = dt.Clone();
                    for (int i = pageSize * 2 + 1; i < dt.Rows.Count; i++)
                    {
                        t1.ImportRow(dt.Rows[i]);
                    }
                    xrChart4.Visible = true;
                    BuildChart(t1, xrChart4);
                }
                else if (dt.Rows.Count > pageSize * 3 && dt.Rows.Count <= pageSize * 4)
                {
                    t1 = dt.Clone();
                    for (int i = 0; i <= pageSize; i++)
                    {
                        t1.ImportRow(dt.Rows[i]);
                    }
                    BuildChart(t1, xrChart2);
                    t1 = dt.Clone();
                    for (int i = pageSize + 1; i <= pageSize * 2; i++)
                    {
                        t1.ImportRow(dt.Rows[i]);
                    }
                    xrChart3.Visible = true;
                    BuildChart(t1, xrChart3);
                    t1 = dt.Clone();
                    for (int i = pageSize * 2 + 1; i <= pageSize * 3; i++)
                    {
                        t1.ImportRow(dt.Rows[i]);
                    }
                    xrChart4.Visible = true;
                    BuildChart(t1, xrChart4); 
                    t1 = dt.Clone();
                    for (int i = pageSize * 3 + 1; i < dt.Rows.Count; i++)
                    {
                        t1.ImportRow(dt.Rows[i]);
                    }
                    xrChart5.Visible = true;
                    BuildChart(t1, xrChart5);
                }
                else if (dt.Rows.Count > pageSize * 4 && dt.Rows.Count <= pageSize * 5)
                {
                    t1 = dt.Clone();
                    for (int i = 0; i <= pageSize; i++)
                    {
                        t1.ImportRow(dt.Rows[i]);
                    }
                    BuildChart(t1, xrChart2);
                    t1 = dt.Clone();
                    for (int i = pageSize + 1; i <= pageSize * 2; i++)
                    {
                        t1.ImportRow(dt.Rows[i]);
                    }
                    xrChart3.Visible = true;
                    BuildChart(t1, xrChart3);
                    t1 = dt.Clone();
                    for (int i = pageSize * 2 + 1; i <= pageSize * 3; i++)
                    {
                        t1.ImportRow(dt.Rows[i]);
                    }
                    xrChart4.Visible = true;
                    BuildChart(t1, xrChart4);
                    t1 = dt.Clone();
                    for (int i = pageSize * 3 + 1; i < pageSize * 4; i++)
                    {
                        t1.ImportRow(dt.Rows[i]);
                    }
                    xrChart5.Visible = true;
                    BuildChart(t1, xrChart5);
                    t1 = dt.Clone();
                    for (int i = pageSize * 4 + 1; i < dt.Rows.Count; i++)
                    {
                        t1.ImportRow(dt.Rows[i]);
                    }
                    xrChart6.Visible = true;
                    BuildChart(t1, xrChart6);
                }
                else if (dt.Rows.Count > pageSize * 5 && dt.Rows.Count <= pageSize * 6)
                {
                    t1 = dt.Clone();
                    for (int i = 0; i <= pageSize; i++)
                    {
                        t1.ImportRow(dt.Rows[i]);
                    }
                    BuildChart(t1, xrChart2);
                    t1 = dt.Clone();
                    for (int i = pageSize + 1; i <= pageSize * 2; i++)
                    {
                        t1.ImportRow(dt.Rows[i]);
                    }
                    xrChart3.Visible = true;
                    BuildChart(t1, xrChart3);
                    t1 = dt.Clone();
                    for (int i = pageSize * 2 + 1; i <= pageSize * 3; i++)
                    {
                        t1.ImportRow(dt.Rows[i]);
                    }
                    xrChart4.Visible = true;
                    BuildChart(t1, xrChart4);
                    t1 = dt.Clone();
                    for (int i = pageSize * 3 + 1; i < pageSize * 4; i++)
                    {
                        t1.ImportRow(dt.Rows[i]);
                    }
                    xrChart5.Visible = true;
                    BuildChart(t1, xrChart5);
                    t1 = dt.Clone();
                    for (int i = pageSize * 4 + 1; i < pageSize * 5; i++)
                    {
                        t1.ImportRow(dt.Rows[i]);
                    }
                    xrChart6.Visible = true;
                    BuildChart(t1, xrChart6);
                    t1 = dt.Clone();
                    for (int i = pageSize * 5 + 1; i < dt.Rows.Count; i++)
                    {
                        t1.ImportRow(dt.Rows[i]);
                    }
                    xrChart7.Visible = true;
                    BuildChart(t1, xrChart7);
                }
                else
                {
                    BuildChart(dt, xrChart2);
                }
            }
        }

        private void xrChart1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            //DominoDiskHealthRptDSTableAdapters.DominoDiskSpaceSrvTableAdapter dominodisksrvAdapter = new DominoDiskHealthRptDSTableAdapters.DominoDiskSpaceSrvTableAdapter();
            //dominodisksrvAdapter.Fill(this.dominoDiskHealthRptDS1.DominoDiskSpaceSrv, this.ServerName.Value.ToString());
            if (this.Parameters["ServerName"].Value.ToString() != "")
            {
                DataTable dt = new DataTable();
                dt = VSWebBL.ReportsBL.XsdBL.Ins.DominoDiskSpaceSrcBL(this.ServerName.Value.ToString());

                Series series = new Series("s1", ViewType.Spline);
                series.Visible = true;
                //4/28/2015 NS commented out for VSPLUS-1697
                //series.ArgumentScaleType = ScaleType.Auto;
                //series.ValueScaleType = ScaleType.Auto;

                xrChart1.Series[0].DataSource = dt;
                xrChart1.Series[0].ArgumentDataMember = dt.Columns["DiskName"].ToString();
                xrChart1.Series[0].ValueDataMembers.AddRange(dt.Columns["DiskUsed"].ToString());
                xrChart1.Series[0].Visible = true;
                xrChart1.Series[1].DataSource = dt;
                xrChart1.Series[1].ArgumentDataMember = dt.Columns["DiskName"].ToString();
                xrChart1.Series[1].ValueDataMembers.AddRange(dt.Columns["DiskFree"].ToString());
                xrChart1.Series[1].Visible = true;
            }
        }

        private void xrChart2_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            DataTable dt = VSWebBL.DashboardBL.DominoServerDetailsBL.Ins.SetGraphForDiskSpace(this.ServerNameSQL.Value.ToString(), "", "");

            xrChart2.Series[0].DataSource = dt;
            xrChart2.Series[0].ArgumentDataMember = dt.Columns["ServerDiskName"].ToString();
            xrChart2.Series[0].ValueDataMembers.AddRange(dt.Columns["DiskUsed"].ToString());
            xrChart2.Series[0].Visible = true;
            xrChart2.Series[1].DataSource = dt;
            xrChart2.Series[1].ArgumentDataMember = dt.Columns["ServerDiskName"].ToString();
            xrChart2.Series[1].ValueDataMembers.AddRange(dt.Columns["DiskFree"].ToString());
            xrChart2.Series[1].Visible = true;
        }

        //6/3/2015 NS added for VSPLUS-1828
        public void BuildChart(DataTable dt, XRChart xrChart)
        {
            xrChart.Series[0].DataSource = dt;
            xrChart.Series[0].ArgumentDataMember = dt.Columns["ServerDiskName"].ToString();
            xrChart.Series[0].ValueDataMembers.AddRange(dt.Columns["DiskUsed"].ToString());
            xrChart.Series[0].Visible = true;
            xrChart.Series[1].DataSource = dt;
            xrChart.Series[1].ArgumentDataMember = dt.Columns["ServerDiskName"].ToString();
            xrChart.Series[1].ValueDataMembers.AddRange(dt.Columns["DiskFree"].ToString());
            xrChart.Series[1].Visible = true;
        }

    }
}
