using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DO;
using BL;
using DAL;
using System.Data;
using DevExpress.Web.ASPxThemes;

using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Security.Principal;
using System.Net;
using DevExpress.Web;



namespace License
{
	public partial class licensekey : System.Web.UI.Page
	{
		int LicenseID;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (Request.QueryString["ID"] != null && Request.QueryString["ID"] != "")
			{
				

				LicenseID = int.Parse(Request.QueryString["ID"]);
				CompanyNameComboBox.Enabled = false;
			
				if (!IsPostBack)
				{
					FillLicenseGrid(LicenseID);
					FillCompanyNamesComboBox();
					//ExpirationDate.Text = "";

				}
		}
			else
			{
				
				if (!IsPostBack)
				{
					FillCompanyNamesComboBox();

				}
			}
			

		}

		protected void OK_Click(object sender, EventArgs e)
		{   
			bool check = false;
			LicenseKey lic = new LicenseKey();
			if (UnitsTextBox.Text != "0")
			{
				lic.Units = Convert.ToInt32(UnitsTextBox.Text);
				lic.InstallType = InstallTypeList1.SelectedItem.Text;
				lic.CompanyName = CompanyNameComboBox.Text;
				DataTable CompanysIDDataTable = BL.UsersBL.Ins.GetCompanyID(CompanyNameComboBox.Text);
				int CompanyID = int.Parse(CompanysIDDataTable.Rows[0]["ID"].ToString());
				lic.LicenseType = LicenseType.SelectedItem.Text;
				if (ExpirationDate.Date != null)
				{
					if (LicenseType.SelectedItem.Text == "Perpetual")
					{
						lic.ExpirationDate = "12/31/9999";
					}
					else
					{
						DateTime exp = ExpirationDate.Date;
						lic.ExpirationDate = ExpirationDate.Date.ToString("MM/dd/yyyy");
						//lic.ExpirationDate = ExpirationDate.Date.ToShortTimeString();
					}
				}
				else
				{
					lic.ExpirationDate = null;
				}
			 

				VSFramework.TripleDES tripleDes = new VSFramework.TripleDES();

				string rawPass = lic.Units + "#" + lic.InstallType + "#" + lic.CompanyName + "#" + LicenseType.SelectedItem.Text + "#" + lic.ExpirationDate;
				byte[] encryptedPass = tripleDes.Encrypt(rawPass);
				string encryptedPassAsString = string.Join(", ", encryptedPass.Select(s => s.ToString()).ToArray());

				string encrypt = encryptedPassAsString;
				Session["Key"] = encrypt;
				string CreatedBy = Convert.ToString(Session["UserID"]);
				string CreatedOn = DateTime.Now.ToString();
				//int CompanyID = Convert.ToInt32(CompanyNameComboBox.SelectedItem.Value);

				check = BL.LicenseKeyBL.Ins.insert(encrypt, lic.Units, lic.InstallType, lic.LicenseType, lic.ExpirationDate, CreatedBy, CreatedOn, CompanyID);
				Session["LicenseUpdateStatus"] = CompanyNameComboBox.Text;
				Response.Redirect("LicenseGrid.aspx", false);
				Context.ApplicationInstance.CompleteRequest();
			}
			else
			{
				string script = "alert('Units should be Greater than 0');";
				ScriptManager.RegisterStartupScript(this, typeof(Page), "alert", script, true);
			}
			


		}
		private void FillCompanyNamesComboBox()
		{
			DataTable CompanysDataTable = BL.UsersBL.Ins.GetCompanyNames();
			//int id =int.Parse( CompanysDataTable.Rows[0]["ID"].ToString());
			CompanyNameComboBox.DataSource = CompanysDataTable;
			CompanyNameComboBox.TextField = "CompanyName";
			
			CompanyNameComboBox.ValueField = "ID";
			CompanyNameComboBox.DataBind();
			if (Session["li"] != null && Session["li"] != "")

				CompanyNameComboBox.SelectedIndex = Convert.ToInt32(Session["li"].ToString());
		}
		private void FillLicenseGrid(int ID)
		{
			LicenseKey Licensekeyobject = new LicenseKey();
			LicenseKey ReturnObject = new LicenseKey();

			Licensekeyobject.ID = ID;
			int id = Convert.ToInt32(Session["UserID"].ToString());
			ReturnObject = BL.UsersBL.Ins.GetData(Licensekeyobject, id);
			CompanyNameComboBox.Text = ReturnObject.CompanyName;
			//InstallTypeList1.SelectedItem.Text = ReturnObject.InstallType;
			if (ReturnObject.InstallType == "HA")
			{
				InstallTypeList1.SelectedIndex = 0;
			}
			else
			{
				InstallTypeList1.SelectedIndex = 1;
			}

			if (ReturnObject.LicenseType == "Evaluation")
			{
				LicenseType.SelectedIndex = 0;
			}
			else if (ReturnObject.LicenseType == "Subscription") 
			{

				LicenseType.SelectedIndex = 1;
		   }
			
			else
			{
				LicenseType.SelectedIndex = 2;

			}
			

			
			//LicenseType.SelectedItem.Text = ReturnObject.LicenseType;
			//string abc = (ReturnObject.ExpirationDate)
				
			ExpirationDate.Date = Convert.ToDateTime(ReturnObject.ExpirationDate);
			
			UnitsTextBox.Text = ReturnObject.Units.ToString();
		}

		protected void CancelButton_Click(object sender, EventArgs e)
		{
			Response.Redirect("~/LicenseGrid.aspx",false);
		}

		protected void CompanyNameComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			
		}
		protected void InstallTypeRadioButton_SelectedIndexChanged(object sender, EventArgs e)
		{
			//ExpirationDate.Date.Add(DateTime.Now.Month);
			
			//**************************************************************************\\
			//		AutoPostBack is set to false since it was not being used at all		\\
			//**************************************************************************\\
		}

		protected void LicenseTypeRadioButton_SelectedIndexChanged(object sender, EventArgs e)
		{

			ExpirationDate.EnableClientSideAPI = true;
			MonthButton1.EnableClientSideAPI = true;
			YearButton1.ClientEnabled = true;

			if (LicenseType.SelectedIndex == 0)
			{
				ExpirationDate.Date = DateTime.Now.AddMonths(1);
				MonthButton1.ClientEnabled = true;
				YearButton1.ClientEnabled = true;
				ExpirationDate.ClientEnabled = true;
			}

			else if (LicenseType.SelectedIndex == 1)
			{
				ExpirationDate.Date = DateTime.Now.AddYears(1);
				MonthButton1.ClientEnabled = true;
				YearButton1.ClientEnabled = true;
				ExpirationDate.ClientEnabled = true;
			}
			else
			{
				ExpirationDate.Text = "";
				ExpirationDate.ClientEnabled = false;
				MonthButton1.ClientEnabled = false;
				YearButton1.ClientEnabled = false;
			}

		}
		//protected void ExpirationDateRadioButton_SelectedIndexChanged(object sender, EventArgs e)
		//{
		//    if (ExpirationDateList.SelectedIndex == 0)
		//    {
		//        ExpirationDate.Date = DateTime.Now.AddMonths(1);
		//    }
		//    else
		//    {
		//        ExpirationDate.Date = DateTime.Now.AddYears(1);

		//    }
	
		//}

		protected void MonthButton_Click(object sender, EventArgs e)
		{
			ExpirationDate.Date = DateTime.Now.AddMonths(1);
		}

		protected void YearButton_Click(object sender, EventArgs e)
		{
			ExpirationDate.Date = DateTime.Now.AddYears(1);
		}
		

	}
}