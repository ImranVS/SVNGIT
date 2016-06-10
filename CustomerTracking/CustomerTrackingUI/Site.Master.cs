using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxNavBar;
using System.Web.Security;
using System.Data;
using System.Text;
using DevExpress.Web.ASPxEditors;
using CustomerTrackingDO;

namespace CustomerTracking
{
    public partial class SiteMaster : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            Company cmpobj = new Company();

            cmpobj = CustomerServiceBL.CompanyBL.Ins.GetLogo();
                
            logo1.ImageUrl = cmpobj.LogoPath;
            CompanyLabel1.Text = cmpobj.CompanyName;

            // logo1.ImageUrl = "/images/logo.png";// logoPath;

            if (!IsPostBack)
                return;
            
        }

        protected void CustomerMenu_ItemClick(object source, NavBarItemEventArgs e)
        {
            Session["GroupIndex"] = e.Item.Group.Index;
            Session["ItemIndex"] = e.Item.Index;
            e.Item.Selected = true;
            Session["SubMenu"] = "";
            Session["SubItemIndex"] = null;
            if (e.Item.Name == "Customer")
            {
                Session["Customer"] = "";
                Session["Contacts"] = "";
                Session["Notes"] = "";
                Session["Tickets"] = "";
                Session["VersionInfo"] = "";
                Response.Redirect("Customer.aspx");
            }

            if (e.Item.Name == "Contacts")
            {
                Response.Redirect("ContactSearch.aspx");
            }

            if (e.Item.Name == "Notes")
            {
                Response.Redirect("NotesSearch.aspx");
            }
            if (e.Item.Name == "Tickets")
            {
                Response.Redirect("TicketsSearch.aspx");
            }
            if (e.Item.Name == "VersionInfo")
            {
                Response.Redirect("VersionInfoSearch.aspx");
            }
        }

        protected void CustomerMenu_HeaderClick(object source, NavBarGroupCancelEventArgs e)
        {
            if (e.Group.Name == "CustomerInfo")
            {
                if (Session["GroupIndex"] != null)
                    CustomerMenu.Groups[Convert.ToInt32(Session["GroupIndex"])].Expanded = false;
                Session["GroupIndex"] = null;
                Session["ItemIndex"] = null;
                Session["Submenu"] = null;

                Response.Redirect("CustomerInfo.aspx");
            }
        }


    }
}