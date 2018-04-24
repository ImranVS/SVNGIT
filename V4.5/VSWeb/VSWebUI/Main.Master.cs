using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VSWebDO;

namespace VSWebUI
{
    public partial class Main : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["Masterpage"] = "~/Main.Master";
           
            Company cmpobj = new Company();

            cmpobj = VSWebBL.ConfiguratorBL.CompanyBL.Ins.GetLogo();
            logo1.ImageUrl = cmpobj.LogoPath;
            CompanyLabel1.Text = cmpobj.CompanyName;

           // logo1.ImageUrl = "/images/logo.png";// logoPath;
        }
       
    }
}