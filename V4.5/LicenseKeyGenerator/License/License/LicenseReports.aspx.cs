using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using DevExpress.Web;
using System.Configuration;
using BL;
using DO;


namespace License
{
	public partial class LicenseReports : System.Web.UI.Page
	{
		
protected void Page_Load(object sender, EventArgs e)
		{
			
			
			License.LicenseXtraReport reporting = new License.LicenseXtraReport();
			this.ReportViewer1.Report = reporting;
			this.ReportViewer1.DataBind();
			}

		}
}