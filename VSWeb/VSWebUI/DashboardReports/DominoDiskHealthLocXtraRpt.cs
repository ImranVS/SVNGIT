using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;
using System.Web.UI.WebControls;
using DevExpress.XtraCharts;

namespace VSWebUI.DashboardReports
{
    public partial class DominoDiskHealthLocXtraRpt : DevExpress.XtraReports.UI.XtraReport
    {
        public DominoDiskHealthLocXtraRpt()
        {
            InitializeComponent();
        }

        private void DominoDiskHealthLocXtraRpt_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            /*
            DominoDiskHealthRptDSTableAdapters.DominoDiskSpaceLocTableAdapter dominodiskAdapter = new DominoDiskHealthRptDSTableAdapters.DominoDiskSpaceLocTableAdapter();
            dominodiskAdapter.Fill(this.dominoDiskHealthRptDS1.DominoDiskSpaceLoc, this.ServerName.Value.ToString());
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
            dt = VSWebBL.ReportsBL.XsdBL.Ins.DominoDiskSpaceLocBL(this.LocationName.Value.ToString());
            if (dt.Rows.Count > 0)
            {
                this.DataSource = dt;
                xrLabel2.DataBindings.Add("Text", dt, "Location");
                xrLabel3.DataBindings.Add("Text", dt, "ServerName");
                mtDiskName.DataBindings.Add("Text", dt, "DiskName");
                mtPercentFree.DataBindings.Add("Text", dt, "PercentFree");
                mtDiskSize.DataBindings.Add("Text", dt, "DiskSize");
                mtDiskFree.DataBindings.Add("Text", dt, "DiskFree");
                //6/17/2014 NS commented out for VSPLUS-731
                //mtPercentUtil.DataBindings.Add("Text", dt, "PercentUtilization");
                //mtAvgQueueLength.DataBindings.Add("Text", dt, "AverageQueueLength");
                mtUpdated.DataBindings.Add("Text", dt, "Updated");
                mtThreshold.DataBindings.Add("Text", dt, "Threshold");
                if (this.Parameters["LocationName"].Value.ToString() != "")
                {
                    //xrChart1.Visible = true;
                }
            }

        }

        private void xrChart1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (this.Parameters["LocationName"].Value.ToString() != "")
            {
               // DominoDiskHealthRptDSTableAdapters.DominoDiskSpaceSrvTableAdapter dominodisksrvAdapter = new DominoDiskHealthRptDSTableAdapters.DominoDiskSpaceSrvTableAdapter();
               
                DataTable dt = new DataTable();
                dt = VSWebBL.ReportsBL.XsdBL.Ins.DominoDiskSpaceLocBL(this.LocationName.Value.ToString());

                Series series = new Series("s1",ViewType.Spline);
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
    }
}
