using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace VSWebUI.Dashboard
{
	public partial class O365UserLicenseswithServices : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				FillUserLicenseswithservices();
			}
			if (!IsPostBack)
			{

				//23/2/2016 Sowmya Added for VSPLUS 2637
				if (Session["UserPreferences"] != null)
				{
					DataTable UserPreferences = (DataTable)Session["UserPreferences"];
					foreach (DataRow dr in UserPreferences.Rows)
					{
						if (dr[1].ToString() == "Office365UserLicense|UserLicensesgrid")
						{
							UserLicensesgrid.SettingsPager.PageSize = Convert.ToInt32(dr[2]);

						}
						if (dr[1].ToString() == "Office365UserLicense|UserLicensesgrid")
						{
							UserLicensesgrid.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
						}
					}
				}
				//servernamelbldisp.InnerHtml += " - " + selectedAccName;
			}

		}
		public void FillUserLicenseswithservices()
		{
			DataTable dt = new DataTable();
			dt = VSWebBL.DashboardBL.Office365BL.Ins.GetOffice365Userlicensesstatus();

			try
			{
				if (dt.Rows.Count > 0)
				{
					Session["0365UserLicenses"] = dt;
					UserLicensesgrid.DataSource = dt;
					UserLicensesgrid.DataBind();
					//UserLicensesgrid.Columns[0].Visible = false;

					UserLicensesgrid.Columns["ServerId"].Visible = false;
					UserLicensesgrid.Columns["DisplayName"].VisibleIndex = 0;
					UserLicensesgrid.Columns["Licenses"].VisibleIndex = 1;
					UserLicensesgrid.Columns[1].Width = 200;
					UserLicensesgrid.Columns[0].Width = 200;
					//UserLicensesgrid.Columns[2].Width = 180;
					UserLicensesgrid.Columns[3].Width = 180;
					UserLicensesgrid.Columns[4].Width = 180;
					UserLicensesgrid.Columns[5].Width = 180;
					UserLicensesgrid.Columns[6].Width = 180;
					UserLicensesgrid.Columns[7].Width = 180;
					UserLicensesgrid.Columns[8].Width = 180;
					UserLicensesgrid.Columns[9].Width = 180;
					UserLicensesgrid.Width = 1500;
				}
				
			}
			catch (Exception ex)
			{
				//6/27/2014 NS added for VSPLUS-634
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw;
			}
		}

		protected void UserLicensesgrid_HtmlDataCellPrepared(object sender, DevExpress.Web.ASPxGridViewTableDataCellEventArgs e)
		{
			if ( e.CellValue.ToString() == "Success")
			{
				e.Cell.BackColor = System.Drawing.Color.LightGreen;
			}
			else if (e.CellValue.ToString() == "PendingActivation")
			{
				e.Cell.BackColor = System.Drawing.Color.Orange;
				e.Cell.ForeColor = System.Drawing.Color.White;
			}
		}

		protected void UserLicensesgrid_PageSizeChanged(object sender, EventArgs e)
		{
			VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("Office365UserLicense|UserLicensesgrid", UserLicensesgrid.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
			Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
		}
	}
}