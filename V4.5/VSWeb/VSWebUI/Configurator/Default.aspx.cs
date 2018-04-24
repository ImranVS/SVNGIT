using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace VSWebUI
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //Mukund 04Jun14 :: VSPLUS-676
                Session["Submenu"] = null;

                //Mukund 24Oct13
				DataTable licensedt = new DataTable();
				licensedt = VSWebBL.ConfiguratorBL.LicenseBL.Ins.GetLicensedetails();
				if (licensedt.Rows.Count > 0)
				{
					//LicenseTypeLabel.Text = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("License Type");
					//ExpirationLabel.Text = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("License Expiration");
					//string strExpiryOn = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("License Warning Days");
                    LicenseTypeLabel.Text = licensedt.Rows[0]["LicenseType"].ToString() + " " + licensedt.Rows[0]["InstallType"].ToString();
                    //4/23/2015 NS modified for VSPLUS-1685
                    DateTime datetimeonly = Convert.ToDateTime(licensedt.Rows[0]["ExpirationDate"].ToString());
                    ExpirationLabel.Text = datetimeonly.Date.ToShortDateString();
				}

                //4/23/2015 NS modified for VSPLUS-1685
                DateTime currendate = DateTime.Now;
				if (licensedt.Rows[0]["ExpirationDate"].ToString() != null && licensedt.Rows[0]["ExpirationDate"].ToString() != "" && !(LicenseTypeLabel.Text.Contains("Perpetual")))
				{
                    DateTime ExpirationDt = Convert.ToDateTime(licensedt.Rows[0]["ExpirationDate"].ToString());
                    ExpiresonLabel.Visible = true;
                    ExpirationLabel.Visible = true;
                    int result = DateTime.Compare(currendate, ExpirationDt);
                    if (result <= 0)
                    {
                        ExpirationLabel.ForeColor = System.Drawing.Color.Green;
                        ExpirationLabel.Font.Bold = true;
                    }
                    else
                    {
                        if ((Convert.ToDateTime(ExpirationLabel.Text) - DateTime.Now).TotalDays <= Convert.ToInt32(licensedt.Rows[0]["ExpirationDate"].ToString()))
                        {
                            ExpirationLabel.ForeColor = System.Drawing.Color.Red;
                            ExpirationLabel.Font.Bold = true;
                        }
                    }
                }
                else
                {
                    ExpiresonLabel.Visible = false;
                    ExpirationLabel.Visible = false;
                }
                /*
				string strExpiryOn = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("License Warning Days");

                if (Convert.ToInt32(strExpiryOn) == 0) //Perpetual License
                {
                    ExpiresonLabel.Visible = false;
                    ExpirationLabel.Visible = false;
                }
                else
                {
                    ExpiresonLabel.Visible = true;
                    ExpirationLabel.Visible = true;
					if (ExpirationLabel.Text != null && ExpirationLabel.Text!="")
					{
                    if ((Convert.ToDateTime(ExpirationLabel.Text) - DateTime.Now).TotalDays <= Convert.ToInt32(strExpiryOn))
                    {
                        ExpirationLabel.ForeColor = System.Drawing.Color.Red;
                        ExpirationLabel.Font.Bold = true;
                    }
					}
                }
                 */
                //1/6/2014 NS added
                bool alertson = false;
                alertson = bool.Parse(VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("AlertsOn"));
                if (alertson)
                {
                    AlertsOnLabel.InnerHtml = "ON";
                    AlertsOnLabel.Style.Value = "color: green; font-weight: bold";
                }
                else
                {
                    AlertsOnLabel.InnerHtml = "OFF";
                    AlertsOnLabel.Style.Value = "color: red; font-weight: bold";
                }
            }
            catch (Exception ex)
            {
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                //throw;
            }
        }
    }
}