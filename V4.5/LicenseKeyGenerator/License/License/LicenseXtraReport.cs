using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;

using System.Web;

namespace License
{
	public partial class LicenseXtraReport : DevExpress.XtraReports.UI.XtraReport
	{
		public LicenseXtraReport()
		{
			InitializeComponent();
		}

		private void XtraReport3_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
		{
			DataTable dt = new DataTable();

			//dt = BL.LicenseKeyBL.Ins.GetLicenceReportInformations();
			dt = (DataTable)HttpContext.Current.Session["report"];
			if (dt.Rows.Count > 0)
			{
				this.DataSource = dt;
				xrTableCell1.DataBindings.Add("Text", dt, "CompanyName");
				xrTableCell2.DataBindings.Add("Text", dt, "CreatedOn");
				xrTableCell4.DataBindings.Add("Text", dt, "ExpirationDate");
				xrTableCell5.DataBindings.Add("Text", dt, "InstallType");
				xrTableCell6.DataBindings.Add("Text", dt, "LicenseType");
				xrTableCell7.DataBindings.Add("Text", dt, "Units");
				xrTableCell3.DataBindings.Add("Text", dt, "LicenseKey");

			}
		}

	}
}
