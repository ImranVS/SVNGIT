using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;

namespace VSWebUI.ConfiguratorReports
{
    public partial class ServerListLocXtraRpt : DevExpress.XtraReports.UI.XtraReport
    {
        public ServerListLocXtraRpt()
        {
            InitializeComponent();
        }

        private void ServerListXtraRpt_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            //ServerListLocRptDSTableAdapters.ServersTableAdapter serverAdapter = new ServerListLocRptDSTableAdapters.ServersTableAdapter();
            //serverAdapter.Fill(this.serverListRptDS1.Servers, this.Location.Value.ToString());
            DataTable dt = new DataTable();
            dt = VSWebBL.ReportsBL.ReportsBL.Ins.GetServerList(this.Location.Value.ToString(),"Location");
            if (dt.Rows.Count > 0)
            {
                this.DataSource = dt;
                xrLabel2.DataBindings.Add("Text", dt, "Location");
                slServerName.DataBindings.Add("Text", dt, "ServerName");
                slIP.DataBindings.Add("Text", dt, "IPAddress");
                slType.DataBindings.Add("Text", dt, "ServerType");
                //8/12/2014 NS added for VSPLUS-428
                slOS.DataBindings.Add("Text", dt, "OperatingSystem");
                slRelease.DataBindings.Add("Text", dt, "Release");
            }
        }

        private void xrTableCell1_PreviewClick(object sender, PreviewMouseEventArgs e)
        {
            // Turn off sorting.
            this.Detail.SortFields.Clear();
            xrTableCell1.Text = xrTableCell1.Text.Remove(xrTableCell1.Text.Length - 1, 1);

            // Create a new field to sort.
            GroupField grField = new GroupField();
            grField.FieldName = ((XRControl)sender).Tag.ToString();
            grField.SortOrder = XRColumnSortOrder.Ascending;

            // Add sorting.
            this.Detail.SortFields.Add(grField);
            ((XRLabel)sender).Text = ((XRLabel)sender).Text + "*";
            xrTableCell1 = (XRTableCell)sender;

            // Recreate the report document.
            this.CreateDocument();
        }

    }
}
