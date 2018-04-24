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
	public partial class LicenseExpiringReport : System.Web.UI.Page
	{
		
		protected void Page_Load(object sender, EventArgs e)
		{

			if (!IsPostBack)
			{
				FillLicenceReportGrid();
			}

			else
			{

				FillLicenceReportGridfromSession();
			}
		}
		private void FillLicenceReportGrid()
		{
			try
			{
				DataTable Licenceinformation = new DataTable();
				Licenceinformation = BL.LicenseKeyBL.Ins.GetLicenceReportInformation(Convert.ToString(Session["UserID"]));
				Session["Licence"] = Licenceinformation;
				LicenceReportGridView.DataSource = Licenceinformation;
				LicenceReportGridView.DataBind();
			}
			catch (Exception ex)
			{
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}
			finally { }
		}
		private void FillLicenceReportGridfromSession()
		{
			try
			{

				DataTable Licenceinformation = new DataTable();
				if (Session["Licence"] != null && Session["Licence"] != "")
					Licenceinformation = (DataTable)Session["Licence"];//VSWebBL.ConfiguratorBL.WindowsPropertiesBL.Ins.GetAllData();
				if (Licenceinformation.Rows.Count > 0)
				{
					LicenceReportGridView.DataSource = Licenceinformation;
					LicenceReportGridView.DataBind();
				}
			}
			catch (Exception ex)
			{
				//6/27/2014 NS added for VSPLUS-634
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}
			finally { }
		}
		
		
	}
	}