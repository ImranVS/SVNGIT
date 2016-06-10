using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;
using VSWebBL;

namespace VSWebUI.ConfiguratorReports
{
    public partial class MailFileXtraRpt : DevExpress.XtraReports.UI.XtraReport
    {
        public MailFileXtraRpt()
        {
            InitializeComponent();
        }

        private void MailFileXtraRpt_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            string filesize = "";
            double Num;
            filesize = this.FileSize.Value.ToString();
            bool isNum = double.TryParse(filesize, out Num);
            if (isNum || filesize == "")
            {
                DataTable dt = new DataTable();
                dt = VSWebBL.ReportsBL.ReportsBL.Ins.GetMailFileStats(filesize);
                this.DataSource = dt;
                mfTitle.DataBindings.Add("Text", dt, "Title");
                mfFileName.DataBindings.Add("Text", dt, "FileName");
                mfFileSize.DataBindings.Add("Text", dt, "FileSize");
                mfServer.DataBindings.Add("Text", dt, "Server");
            }
        }
    }
}
