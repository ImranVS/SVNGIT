using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;
using VSWebBL;
using System.Web.UI.WebControls;



namespace VSWebUI.DashboardReports
{
    public partial class HR1rpt : DevExpress.XtraReports.UI.XtraReport
    {
        public HR1rpt()
        {
            InitializeComponent();
        }

        private void HR1rpt_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
           // HRDeviceDailyStatusTableAdapters.DeviceDailyStatsTableAdapter HR1 = new HRDeviceDailyStatusTableAdapters.DeviceDailyStatsTableAdapter();
           
          //  HR1.Fill(this.hrDeviceDailyStatus1.DeviceDailyStats, this.MyDevice.Value.ToString(), (DateTime)this.StartDate.Value, (DateTime)this.EndDate.Value);


            DataSet ds = new DataSet();
            SqlDataSource datasourse = new SqlDataSource();
            
            
            datasourse = VSWebBL.ReportsBL.XsdBL.Ins.Hr1RptBL(this.MyDevice.Value.ToString(), (DateTime)this.StartDate.Value, (DateTime)this.EndDate.Value);
            xrChart1.DataSource = datasourse;
           // xrChart1.DataMember = dt;
            ((SqlDataSource)this.xrChart1.DataSource).DataBind();
                              
        }



    }
}
