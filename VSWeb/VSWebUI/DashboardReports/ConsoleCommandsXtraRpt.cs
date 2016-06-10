using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;

namespace VSWebUI.DashboardReports
{
    public partial class ConsoleCommandsXtraRpt : DevExpress.XtraReports.UI.XtraReport
    {
        public ConsoleCommandsXtraRpt()
        {
            InitializeComponent();
        }

        private void ConsoleCommandsXtraRpt_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            DataTable dt = new DataTable();
            dt = VSWebBL.ReportsBL.ReportsBL.Ins.GetConsoleCmdList();
            if (dt.Rows.Count > 0)
            {
                this.DataSource = dt;
                xrLabel2.DataBindings.Add("Text", dt, "Submitter");
                xrTableCell5.DataBindings.Add("Text", dt, "ServerName");
                xrTableCell6.DataBindings.Add("Text", dt, "Command");
                xrTableCell7.DataBindings.Add("Text", dt, "DateTimeSubmitted");
                xrTableCell9.DataBindings.Add("Text", dt, "DateTimeProcessed");
                //11/20/2014 NS added for VSPLUS-1188
                xrTableCell11.DataBindings.Add("Text", dt, "Result");
                //12/11/2014 NS added for VSPLUS-1213
                xrTableCell13.DataBindings.Add("Text", dt, "Comments");
            }
        }

        //11/4/2014 NS added for VSPLUS-1148
        private void GroupHeader2_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            DevExpress.XtraReports.UI.GroupField objGroupField = new DevExpress.XtraReports.UI.GroupField();
            objGroupField.FieldName = "Submitter";
            GroupHeader2.GroupFields.Add(objGroupField);
        }

    }
}
