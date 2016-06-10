using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;
using VSWebBL;

namespace VSWebUI.DashboardReports
{
    public partial class DailyMailVolumeXtraRpt : DevExpress.XtraReports.UI.XtraReport
    {
        public DailyMailVolumeXtraRpt()
        {
            InitializeComponent();
        }

        private void DailyMailVolumeXtraRpt_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
           // DailyMailVolumeRptDSTableAdapters.DominoDailyStatsTableAdapter dominoAdapter = new DailyMailVolumeRptDSTableAdapters.DominoDailyStatsTableAdapter();
            //dominoAdapter.Fill(this.dailyMailVolumeRptDS1.DominoDailyStats, this.ServerName.Value.ToString());

            DataTable dt = new DataTable();
            //12/18/2015 NS modified for VSPLUS-2291
            //26/4/2016 Durga Modified for VSPLUS-2883
            if (this.RptType.Value.ToString() == "Daily")
            {
                dt = VSWebBL.ReportsBL.XsdBL.Ins.getDailyMailVolumeBL(this.ServerName.Value.ToString(),this.ServerType.Value.ToString());
                xrPivotGrid1.DataSource = dt;
                xrPivotGrid1.Fields["Date"].Visible = true;
                xrPivotGrid1.Fields["MonthYear"].Visible = false;
                xrLabel1.Text = "Daily Mail Volume";
            }
            else
            {
                dt = VSWebBL.ReportsBL.XsdBL.Ins.MonthlyMailVolumeRptDS(this.ServerName.Value.ToString(),this.ServerType.Value.ToString());
                xrPivotGrid1.DataSource = dt;
                xrPivotGrid1.Fields["Date"].Visible = false;
                xrPivotGrid1.Fields["MonthYear"].Visible = true;
                xrPivotGrid1.Fields["MonthYear"].ValueFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
                xrPivotGrid1.Fields["MonthYear"].ValueFormat.FormatString = "MM/yyyy";
                xrLabel1.Text = "Monthly Mail Volume";
            }
        }

    }
}
