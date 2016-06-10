using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace VSWebUI
{
    public partial class O365Mailboxstoragegrowth : System.Web.UI.Page
    {
        // 6/6/2016 Durga Addded for  VSPLUS-2993
        protected void Page_Load(object sender, EventArgs e)
        {



             
          
            FillReport();
        }


     

        public void FillReport()
        {
            DashboardReports.O365MailboxstoragegrowthRpt rpt = new DashboardReports.O365MailboxstoragegrowthRpt();



           

            rpt.Parameters["StartDate"].Value = dtPick.FromDate; 
            rpt.Parameters["EndDate"].Value = dtPick.ToDate; 
         
            rpt.Parameters["Threshold"].Value = TCutoffTextBox.Text;
            
            ReportViewer1.Report = rpt;
            ReportViewer1.DataBind();
        }

        protected void ResetButton_Click(object sender, EventArgs e)
        {

            if (Request.UrlReferrer != null)
                Response.Redirect(Request.RawUrl); 
        }

        protected void SubmitButton_Click(object sender, EventArgs e)
        {
          
            FillReport();
        }

        protected void Page_PreInit(object sender, EventArgs e)
        {
          
                this.MasterPageFile = "~/Reports.Master";

           
        }

       
       
       
    }
}   