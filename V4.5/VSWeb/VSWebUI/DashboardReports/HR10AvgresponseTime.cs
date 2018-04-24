using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;
using VSWebBL;

namespace VSWebUI.DashboardReports
{
    public partial class HR10AvgresponseTime : DevExpress.XtraReports.UI.XtraReport
    {
        public HR10AvgresponseTime()
        {
            InitializeComponent();
        }

        private void HR10AvgresponseTime_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
          //  hr10AvarageDeliveryTimeinSecondsNotesMailTableAdapters.BlackBerryProbeStatsTableAdapter NB = new hr10AvarageDeliveryTimeinSecondsNotesMailTableAdapters.BlackBerryProbeStatsTableAdapter();
           // NB.Fill(this.hr10AvarageDeliveryTimeinSecondsNotesMail1.BlackBerryProbeStats, this.MyDevice.Value.ToString(), (DateTime)this.StartDate.Value, (DateTime)this.EndDate.Value);
            DataTable dt = new DataTable();
            dt = VSWebBL.ReportsBL.XsdBL.Ins.hr10AvarageDeliveryTimeinSecondsBL(this.MyDevice.Value.ToString(), (DateTime)this.StartDate.Value, (DateTime)this.EndDate.Value);
            xrChart1.DataSource = dt;

        }

    }
}
