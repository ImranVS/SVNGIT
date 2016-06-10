using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace VSWebUI
{
    public partial class ExchangeMailboxstoragegrowth : System.Web.UI.Page
    {
        // 6/6/2016 Durga Addded for  VSPLUS-2993
      
        protected void Page_Load(object sender, EventArgs e)
        {



            if (!IsPostBack)
            {
                FillDocumentscombo(DatabasesServersRadioButton.SelectedItem.Value.ToString());
            }

            
            FillReport();
        }


     

        public void FillReport()
        {
            DashboardReports.ExchangeMailboxstoragegrowthRpt rpt = new DashboardReports.ExchangeMailboxstoragegrowthRpt();



            rpt.Parameters["DocumentName"].Value = DocumentComboBox.Text;
            rpt.Parameters["Type"].Value = DatabasesServersRadioButton.Value;

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

        protected void DatabasesServersRadioButton_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            FillDocumentscombo(DatabasesServersRadioButton.SelectedItem.Value.ToString());
          
        }

        public void FillDocumentscombo(string Type)
        {
            DataTable dt = new DataTable();

            dt = VSWebBL.ReportsBL.XsdBL.Ins.getExchangeMailboxNames(Type);
            if (dt.Rows.Count > 0 && Type=="Databases")
            {
                DocumentComboBox.DataSource = dt;
                DocumentComboBox.TextField = "DocumentName";
                DocumentComboBox.ValueField = "statname";
                DocumentComboBox.DataBind();
                DocumentComboBox.SelectedIndex = 0;
            }
            else if (Type == "Servers")
            {
                DocumentComboBox.Items.Clear();
                
                    DocumentComboBox.DataSource = dt;
                   
                    DocumentComboBox.TextField = "ServerName";
                    DocumentComboBox.ValueField = "ServerName";
                    DocumentComboBox.DataBind();
                    if (dt.Rows.Count > 0)
                    {
                        DocumentComboBox.SelectedIndex = 0;
                    }
                    else if(dt.Rows.Count == 0)
                    {
                        DocumentComboBox.Text = "";
                    }
             
            }
            UpdatePanel3.Update();
        }
       
    }
}   