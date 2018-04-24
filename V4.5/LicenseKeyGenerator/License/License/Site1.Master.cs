using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using System.Web.Security;



namespace License
{
	public partial class Site1 : System.Web.UI.MasterPage
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				if (Session["UserFullName"] != null)
				{
					UserFullName.Text = Session["UserFullName"].ToString();
				}

				if (Session["UserType"] != null)
				{
					if (Session["UserType"].ToString() == "Admin")
					{

						ASPxMenu1.Visible = true;
					}
					else
					{

						ASPxMenu1.Visible = false;
					}

				}
			}
		}
		protected void ASPxMenu1_ItemClick(object source, DevExpress.Web.MenuItemEventArgs e)
		{
			if (e.Item.Name == "Company")
			{
				Response.Redirect("~/CompanyGrid.aspx", false);
				Context.ApplicationInstance.CompleteRequest();
			}
			if (e.Item.Name == "MaintainUserspage")
			{
				Response.Redirect("~/MaintainUsers.aspx",false);
			   Context.ApplicationInstance.CompleteRequest();
			}
			if (e.Item.Name == "LicenseKeyGeneratorpage")
			{
				Response.Redirect("~/LicenseGrid.aspx", false);
				Context.ApplicationInstance.CompleteRequest();

			}
			if (e.Item.Name == "ViewDetailsLicenseKey")
			{
				Response.Redirect("~/ViewDetaisLicenceKey.aspx", false);
				Context.ApplicationInstance.CompleteRequest();

			}
			if (e.Item.Name == "LicenseExpiryReport")
			{
				Response.Redirect("~/LicenseExpiryReport.aspx", false);
				Context.ApplicationInstance.CompleteRequest();

			}
			if (e.Item.Name == "EstimateLicense")
			{
				Response.Redirect("~/EstimateLicenses.aspx", false);
				Context.ApplicationInstance.CompleteRequest();

			}
		
		}
		protected void LogoutLinkButton_Click(object sender, EventArgs e)
		{
			SignOut();
		}
		protected void SignOut()
		{
			try
			{
				string message = "<script language=JavaScript>window.history.forward(1);</script>";

				if (!Page.ClientScript.IsStartupScriptRegistered("clientscript"))
				{
					Page.ClientScript.RegisterStartupScript(Page.GetType(), "clientscript", message, true);
				}

				FormsAuthentication.SignOut();
				Session.Clear();
			}
			catch (Exception ex)
			{
				throw ex;
			}
			
			Response.Redirect("~/Login.aspx", false);
			Context.ApplicationInstance.CompleteRequest();
		}
	
	}
}