using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Configuration;
using System.IO;
using System.Net.Mail;
using System.Data;
using DevExpress.XtraReports.UI;

namespace VSWebUI.DashboardReports
{
    // 3/29/2016 Durga Addded for VSPLUS-2698
    public partial class ServerUtilization : System.Web.UI.Page
    {
        private static ServerUtilization _self = new ServerUtilization();

        public static ServerUtilization Ins
        {
            get
            {
                return _self;
            }
        }
        DashboardReports.ServerUtilizationRpt report;
        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (Request.QueryString["M"] == "C" && Request.QueryString["M"].ToString() != "")
            {
                this.MasterPageFile = "~/Reports.Master";

            }
            else
            {
                this.MasterPageFile = "~/Reports.Master";

            }
        }
     

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FillCombobox();
                fillUserTypeCombo();
            }
            FillReport(); 
         
            GetUserType();
        }

        public void FillCombobox()
        {
            DataTable Typetab = VSWebBL.DashboardBL.KeyMetricsBL.Ins.GetServerTypeForCostperUserserved();
            TypeComboBox.DataSource = Typetab;
            TypeComboBox.TextField = "ServerType";
            TypeComboBox.DataBind();

            try
            {
                TypeComboBox.SelectedItem = TypeComboBox.Items.FindByText("Domino");
            }
            catch
            {
                TypeComboBox.SelectedIndex = 0;
            }
        }
        protected void TypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetUserType();
            FillReport();
           
         

        }
        public void GetUserType()
        {
            if (TypeComboBox.Text == "Domino")
            {
                UserTypeComboBox.Visible = false;
                UserTypelabel.Visible = false;

            }
            else
            {
                UserTypeComboBox.Visible = true;
                UserTypelabel.Visible = true;
            }

        }
    
       
        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            FillReport();
        }

      
  
        public void fillUserTypeCombo()
        {
            DataTable dt = new DataTable();
            dt = VSWebBL.ReportsBL.ReportsBL.Ins.GetMSUserCountTypes("AVG");
            if (dt.Rows.Count > 0)
            {
                UserTypeComboBox.SelectedIndex = 0;
                UserTypeComboBox.TextField = "StatName";
                UserTypeComboBox.ValueField = "StatName";
                UserTypeComboBox.DataSource = dt;
                UserTypeComboBox.DataBind();
            }
        }

        public void FillReport()
        {
            DashboardReports.ServerUtilizationRpt rpt = new DashboardReports.ServerUtilizationRpt();
            string selectedServer = "";
            if (this.TypeComboBox.SelectedIndex >= 0)
            {
                selectedServer = this.TypeComboBox.SelectedItem.Value.ToString();
            }

            rpt.Parameters["ServerType"].Value = selectedServer;


            rpt.Parameters["ServerName"].Value = SearchTextBox.Text;
            rpt.Parameters["UserType"].Value = UserTypeComboBox.Text;

            rpt.CreateDocument();
            ASPxDocumentViewer1.Report = rpt;
            ASPxDocumentViewer1.DataBind();
        }
    }
}