using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace VSWebUI
{
    public partial class WebForm9 : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(ConfigurationSettings.AppSettings["VSS_StatisticsConnectionString"]);
        string selectedServer = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GraphCombobox.Text = "Today";
                FillDeviceTypeCombo();
               // DeviceTypeComboBox.SelectedItem = "All";
            }
                FillGraph();
               
        }
        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (Request.QueryString["M"] == "C" && Request.QueryString["M"].ToString() != "")
            {
                this.MasterPageFile = "~/Site1.Master";

            }
            else
            {
                this.MasterPageFile = "~/DashboardSite.Master";

            }
        }
        protected void ReptBtn_Click(object sender, EventArgs e)
        {

            Response.Redirect("~/Configurator/Reports.aspx?M=" + Request.QueryString["M"]);
        }


        protected void Submitbtn_Click(object sender, EventArgs e)
        {
            FillGraph();
        }

        public void FillServerCombo()
        {
            string SqlQuery = "";
            DataTable dt=new DataTable();
            if (DeviceTypeComboBox.Text != "All" && DeviceTypeComboBox.Text != null)
            {
              //   SqlQuery = "select Distinct DeviceName from DeviceDailyStats where DeviceType='" + DeviceTypeComboBox.Text + "'";

              dt= VSWebBL.ReportsBL.ReportsBL.Ins.DeviceTypeComboBoxBL(DeviceTypeComboBox.Text);
              ServerComboBox.DataSource = dt;
              ServerComboBox.TextField = "DeviceName";
              ServerComboBox.ValueField = "DeviceName";
              ServerComboBox.DataBind();

              
            }
            else
            {
               // SqlQuery = "select Distinct DeviceName from DeviceDailyStats";

              dt= VSWebBL.ReportsBL.ReportsBL.Ins.DeviceTypeComboBoxforhistoricalresponseelsepartBL();
              ServerComboBox.DataSource = dt;
              ServerComboBox.TextField = "DeviceName";
              ServerComboBox.ValueField = "DeviceName";
              ServerComboBox.DataBind();

            }
           
            //dt = ds.Tables[0];
            //ServerComboBox.DataSourceID = "";
            //ServerComboBox.DataSource = dt;
            //ServerComboBox.TextField = "DeviceName";
            //ServerComboBox.ValueField = "DeviceName";
            //ServerComboBox.DataBind();
        }
        public void FillDeviceTypeCombo()
        {
           
            DataTable dt = new DataTable();

            dt = VSWebBL.ReportsBL.ReportsBL.Ins.FillDeviceTypeComboforhistoricalresponseBL();
           
                        
           // SqlQuery = "select Distinct DeviceType from DeviceDailyStats";
          
           // SqlDataAdapter da = new SqlDataAdapter(SqlQuery, con);
           // DataSet ds = new DataSet();
           // da.Fill(ds, "DeviceDailyStats");
           // dt = ds.Tables[0];
            // dt.Rows.Add(dt.NewRow()[0]["All"]);
            DataRow dr1 = dt.NewRow();
            dr1[0] = "BlackBerry";
            dt.Rows.Add(dr1);
            DataRow dr2 = dt.NewRow();
            dr2[0] = "NotesMail";
            dt.Rows.Add(dr2);
            DataRow dr3 = dt.NewRow();
            dr3[0] = "All";
            //dt.Rows.Add(dr3);
            //DataRow r=new DataRow();

            dt.Rows.InsertAt(dr3, 0);

            DeviceTypeComboBox.DataSource = dt;
            DeviceTypeComboBox.TextField = "DeviceType";
            DeviceTypeComboBox.ValueField = "DeviceType";
            DeviceTypeComboBox.DataBind();
            
        }

        public void FillGraph()
        {
          //  DataTable dt = new DataTable();

            if (StartDateEdit.Text == "")
            {
                StartDateEdit.Text = DateTime.Now.ToShortDateString();
            }

            if (EndDateEdit.Text == "")
            {
                EndDateEdit.Text = DateTime.Now.ToShortDateString();
            }
        
            // Today
            try
            {
                if (GraphCombobox.Text == "Today")
                {
                    //DeviceTypeComboBox.SelectedIndex == 0 && 
                    //ServerComboBox.DataSourceID = "SqlDataSource1";
                    //ServerComboBox.TextField = "DeviceName";
                    //ServerComboBox.ValueField = "DeviceName";
                    //ServerComboBox.DataBind();
                    FillServerCombo();
                    if (this.ServerComboBox.SelectedIndex >= 0)
                    {
                        selectedServer = this.ServerComboBox.SelectedItem.Value.ToString();
                    }
                    StartDateEdit.Enabled = false;
                    EndDateEdit.Enabled = false;
                    DashboardReports.HR1rpt report = new DashboardReports.HR1rpt();
                    report.Parameters["MyDevice"].Value = selectedServer;
                    report.Parameters["StartDate"].Value = DateTime.Now.AddDays(-1);//StartDateEdit.Text;
                    report.Parameters["EndDate"].Value = DateTime.Now;//EndDateEdit.Text;
                    this.ReportViewer1.Report = report;
                    this.ReportViewer1.DataBind();
                }
            }
            catch (Exception)
            {
                
                throw;
            }
            try
            {
                if (DeviceTypeComboBox.Text == "NotesMail" && GraphCombobox.Text == "Today")
                {
                  //  ServerComboBox.DataSourceID = "SqlDataSource2";
                    DataTable dt=new DataTable();
                    dt = VSWebBL.ReportsBL.ReportsBL.Ins.fillNotesMailStatsBL();
                    ServerComboBox.DataSource = dt;
                    ServerComboBox.TextField = "Name";
                    ServerComboBox.ValueField = "Name";
                    ServerComboBox.DataBind();
                    if (this.ServerComboBox.SelectedIndex >= 0)
                    {
                        selectedServer = this.ServerComboBox.SelectedItem.Value.ToString();
                    }
                    StartDateEdit.Enabled = false;
                    EndDateEdit.Enabled = false;
                    DashboardReports.Hrs2NotesMail report = new DashboardReports.Hrs2NotesMail();
                    report.Parameters["MyDevice"].Value = selectedServer;
                    report.Parameters["StartDate"].Value = DateTime.Now.AddDays(-1); //StartDateEdit.Text;
                    report.Parameters["EndDate"].Value = DateTime.Now; //EndDateEdit.Text;
                    this.ReportViewer1.Report = report;
                    this.ReportViewer1.DataBind();
                }
            }
            catch (Exception)
            {
                
                throw;
            }

            try
            {
                  if (DeviceTypeComboBox.Text == "BlackBerry" && GraphCombobox.Text == "Today")
            {
               // ServerComboBox.DataSourceID = "SqlDataSource3";
                 DataTable d=new DataTable();  

                d = VSWebBL.ReportsBL.ReportsBL.Ins.fillBlackBerryProbeStatsBL();
                ServerComboBox.DataSource = d;
                
                ServerComboBox.TextField = "Name";
                ServerComboBox.ValueField = "Name";
                ServerComboBox.DataBind();
                if (this.ServerComboBox.SelectedIndex >= 0)
                {
                    selectedServer = this.ServerComboBox.SelectedItem.Value.ToString();
                }

                StartDateEdit.Enabled = false;
                EndDateEdit.Enabled = false;
                DashboardReports.hr3BlackBerryRpt report = new DashboardReports.hr3BlackBerryRpt();
                report.Parameters["MyDevice"].Value = selectedServer;
                report.Parameters["StartDate"].Value = DateTime.Now.AddDays(-1);// StartDateEdit.Text;
                report.Parameters["EndDate"].Value = DateTime.Now;// EndDateEdit.Text;
                this.ReportViewer1.Report = report;
                this.ReportViewer1.DataBind();
            }
            }
            catch (Exception)
            {
                
                throw;
            }

            //Two Days

            try
            {
                if ( GraphCombobox.Text == "Two Days")
                {
                    //ServerComboBox.DataSourceID = "SqlDataSource1";
                    //ServerComboBox.TextField = "DeviceName";
                    //ServerComboBox.ValueField = "DeviceName";
                    //ServerComboBox.DataBind();
                    FillServerCombo();
                    if (this.ServerComboBox.SelectedIndex >= 0)
                    {
                        selectedServer = this.ServerComboBox.SelectedItem.Value.ToString();
                    }

                    StartDateEdit.Enabled = false;
                    EndDateEdit.Enabled = false;

                    DashboardReports.Hr4DeviceDailyStatsRpt report = new DashboardReports.Hr4DeviceDailyStatsRpt();
                    report.Parameters["MyDevice"].Value = selectedServer;
                    report.Parameters["StartDate"].Value = DateTime.Now.AddDays(-2);//StartDateEdit.Text;
                    report.Parameters["EndDate"].Value = DateTime.Now;//EndDateEdit.Text;
                    this.ReportViewer1.Report = report;
                    this.ReportViewer1.DataBind();
                }
            }
            catch (Exception)
            {
                
                throw;
            }
            try
            {
                if ( DeviceTypeComboBox.Text == "NotesMail" && GraphCombobox.Text == "Two Days")
                {
                  //  ServerComboBox.DataSourceID = "SqlDataSource2";
                    DataTable t = new DataTable();
                    t = VSWebBL.ReportsBL.ReportsBL.Ins.fillNotesMailStatsBL();
                    ServerComboBox.DataSource = t;
                    ServerComboBox.TextField = "Name";
                    ServerComboBox.ValueField = "Name";
                    ServerComboBox.DataBind();
                    if (this.ServerComboBox.SelectedIndex >= 0)
                    {
                        selectedServer = this.ServerComboBox.SelectedItem.Value.ToString();
                    }
                    StartDateEdit.Enabled = false;
                    EndDateEdit.Enabled = false;
                    DashboardReports.hr5NotesMailRpt report = new DashboardReports.hr5NotesMailRpt();
                    report.Parameters["MyDevice"].Value = selectedServer;
                    report.Parameters["StartDate"].Value = DateTime.Now.AddDays(-2);//StartDateEdit.Text;
                    report.Parameters["EndDate"].Value = DateTime.Now;//EndDateEdit.Text;
                    this.ReportViewer1.Report = report;
                    this.ReportViewer1.DataBind();
                }
            }
            catch (Exception)
            {
                
                throw;
            }
            try
            {
                if ( DeviceTypeComboBox.Text == "BlackBerry" && GraphCombobox.Text == "Two Days")
                {
                   // ServerComboBox.DataSourceID = "SqlDataSource3";
                    DataTable d = new DataTable();
                    d = VSWebBL.ReportsBL.ReportsBL.Ins.fillBlackBerryProbeStatsBL();
                    ServerComboBox.DataSource = d;

                    ServerComboBox.TextField = "Name";
                    ServerComboBox.ValueField ="Name";
                    ServerComboBox.DataBind();
                    if (this.ServerComboBox.SelectedIndex >= 0)
                    {
                        selectedServer = this.ServerComboBox.SelectedItem.Value.ToString();
                    }
                    StartDateEdit.Enabled = false;
                    EndDateEdit.Enabled = false;
                    DashboardReports.HR10AvgresponseTime report = new DashboardReports.HR10AvgresponseTime();
                  
                    report.Parameters["MyDevice"].Value = selectedServer;
                    report.Parameters["StartDate"].Value = DateTime.Now.AddDays(-2);//StartDateEdit.Text;
                    report.Parameters["EndDate"].Value = DateTime.Now;//EndDateEdit.Text;
                    this.ReportViewer1.Report = report;
                    this.ReportViewer1.DataBind();
                }
            }
            catch (Exception)
            {
                
                throw;
            }
           

            //  ********* Daily

            try
            {
                if ( GraphCombobox.Text == "Daily")
                {
                    //ServerComboBox.DataSourceID = "SqlDataSource1";
                    //ServerComboBox.TextField = "DeviceName";
                    //ServerComboBox.ValueField = "DeviceName";
                    //ServerComboBox.DataBind();
                    FillServerCombo();
                    if (this.ServerComboBox.SelectedIndex >= 0)
                    {
                        selectedServer = this.ServerComboBox.SelectedItem.Value.ToString();
                    }
                    
                    DashboardReports.Hr6DailyDeviceRpt report = new DashboardReports.Hr6DailyDeviceRpt();
                    report.Parameters["MyDevice"].Value = selectedServer;
                    report.Parameters["StartDate"].Value = StartDateEdit.Text;
                    report.Parameters["EndDate"].Value = EndDateEdit.Text;
                    this.ReportViewer1.Report = report;
                    this.ReportViewer1.DataBind();
                }
            }
            catch (Exception)
            {
                
                throw;
            }

            try
            {
                if (GraphCombobox.Text == "Daily" && (DeviceTypeComboBox.Text == "NotesMail" || DeviceTypeComboBox.Text == "BlackBerry"))
                {
                    //ServerComboBox.DataSourceID = "SqlDataSource1";
                    //ServerComboBox.TextField = "DeviceName";
                    //ServerComboBox.ValueField = "DeviceName";
                    //ServerComboBox.DataBind();
                    FillServerCombo();
                    if (this.ServerComboBox.SelectedIndex >= 0)
                    {
                        selectedServer = this.ServerComboBox.SelectedItem.Value.ToString();
                    }

                    DashboardReports.Hr7NotesMailDailyRpt report = new DashboardReports.Hr7NotesMailDailyRpt();
                    report.Parameters["MyDevice"].Value = selectedServer;
                    report.Parameters["StartDate"].Value = StartDateEdit.Text;
                    report.Parameters["EndDate"].Value = EndDateEdit.Text;
                    this.ReportViewer1.Report = report;
                    this.ReportViewer1.DataBind();
                }

            }
            catch (Exception)
            {
                
                throw;
            }
           


            //------------weekly
            try
            {
                if ( GraphCombobox.Text == "Weekly")
                {
                    //ServerComboBox.DataSourceID = "SqlDataSource1";
                    //ServerComboBox.TextField = "DeviceName";
                    //ServerComboBox.ValueField = "DeviceName";
                    //ServerComboBox.DataBind();
                    FillServerCombo();
                    if (this.ServerComboBox.SelectedIndex >= 0)
                    {
                        selectedServer = this.ServerComboBox.SelectedItem.Value.ToString();
                    }
                 

                    DashboardReports.hr8DeviceDailyStatsforweeklyRpt report = new DashboardReports.hr8DeviceDailyStatsforweeklyRpt();
                    report.Parameters["MyDevice"].Value = selectedServer;
                    report.Parameters["StartDate"].Value = StartDateEdit.Text;
                    report.Parameters["EndDate"].Value = EndDateEdit.Text;
                    this.ReportViewer1.Report = report;
                    this.ReportViewer1.DataBind();
                }
            }
            catch (Exception)
            {
                
                throw;
            }

            try
            {
                if ((DeviceTypeComboBox.Text == "BlackBerry" || DeviceTypeComboBox.Text == "NotesMail") && GraphCombobox.Text == "Weekly")
                {
                    //ServerComboBox.DataSourceID = "SqlDataSource1";
                    //ServerComboBox.TextField = "DeviceName";
                    //ServerComboBox.ValueField = "DeviceName";
                    //ServerComboBox.DataBind();
                    FillServerCombo();
                    if (this.ServerComboBox.SelectedIndex >= 0)
                    {
                        selectedServer = this.ServerComboBox.SelectedItem.Value.ToString();
                    }
                 

                    DashboardReports.hr9NotesMailTimeAvarageRpt report = new DashboardReports.hr9NotesMailTimeAvarageRpt();
                    report.Parameters["MyDevice"].Value = selectedServer;
                    report.Parameters["StartDate"].Value = StartDateEdit.Text;
                    report.Parameters["EndDate"].Value = EndDateEdit.Text;
                    this.ReportViewer1.Report = report;
                    this.ReportViewer1.DataBind();
                }
            }
            catch (Exception)
            {
                
                throw;
            }
          

            //----------------monthly
            try
            {
                if (GraphCombobox.Text == "Monthly")
                {
                    //ServerComboBox.DataSourceID = "SqlDataSource1";
                    //ServerComboBox.TextField = "DeviceName";
                    //ServerComboBox.ValueField = "DeviceName";
                    //ServerComboBox.DataBind();
                    FillServerCombo();
                    if (this.ServerComboBox.SelectedIndex >= 0)
                    {
                        selectedServer = this.ServerComboBox.SelectedItem.Value.ToString();
                    }
                  
                    DashboardReports.HR11MonthlyRpt report = new DashboardReports.HR11MonthlyRpt();
                    report.Parameters["MyDevice"].Value = selectedServer;
                    report.Parameters["StartDate"].Value = StartDateEdit.Text;
                    report.Parameters["EndDate"].Value = EndDateEdit.Text;
                    this.ReportViewer1.Report = report;
                    this.ReportViewer1.DataBind();
                }
            }
            catch (Exception)
            {
                
                throw;
            }                 
            
        }

        protected void ResetBtn_Click(object sender, EventArgs e)
        {
            this.ServerComboBox.SelectedIndex = -1;
            //DashboardReports.HR1rpt report = new DashboardReports.HR1rpt();
            //report.Parameters["ServerName"].Value = "";
            //report.Parameters["StartDate"].Value = DateTime.Now.ToString();
            //report.Parameters["EndDate"].Value = DateTime.Now.ToString();
            // ServerComboBox.Text = "";
            GraphCombobox.SelectedIndex = 0;
            DeviceTypeComboBox.SelectedIndex = 0;
            StartDateEdit.Text = DateTime.Now.ToShortDateString();
            EndDateEdit.Text = DateTime.Now.ToShortDateString();
            FillServerCombo();
            //ServerComboBox.DataSourceID = "SqlDataSource1";
            //ServerComboBox.TextField = "DeviceName";
            //ServerComboBox.ValueField = "DeviceName";
            //ServerComboBox.DataBind();

            //this.ReportViewer1.Report =null; //report;
            //this.ReportViewer1.DataBind();
        }

        protected void GraphCombobox_SelectedIndexChanged(object sender, EventArgs e)
        {
            DeviceTypeComboBox.SelectedIndex = 0;
            ServerComboBox.SelectedIndex = -1;
            StartDateEdit.Text = DateTime.Now.ToShortDateString();
            EndDateEdit.Text = DateTime.Now.ToShortDateString();
            if (GraphCombobox.SelectedItem.ToString() == "Today" || GraphCombobox.SelectedItem.ToString() == "Two Days")
            {
                StartDateEdit.Enabled= false;
                EndDateEdit.Enabled = false;
            }

            else
            {
                StartDateEdit.Enabled = true;
                EndDateEdit.Enabled = true;
            }
        }

        protected void DeviceTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ServerComboBox.Text = "";
            DataTable dt = new DataTable();
            if (GraphCombobox.Text == "Today" || GraphCombobox.Text == "Two Days")
            {
                if (DeviceTypeComboBox.SelectedItem.ToString() == "NotesMail")
                {
                   // ServerComboBox.DataSourceID = "SqlDataSource2";
                    dt = VSWebBL.ReportsBL.ReportsBL.Ins.fillNotesMailStatsBL();
                    ServerComboBox.DataSource = dt;
                    ServerComboBox.TextField = "Name";
                    ServerComboBox.ValueField = "Name";
                    ServerComboBox.DataBind();
                }
                else if (DeviceTypeComboBox.SelectedItem.ToString() == "BlackBerry" )
                {
                    dt = VSWebBL.ReportsBL.ReportsBL.Ins.fillBlackBerryProbeStatsBL();
                   // ServerComboBox.DataSourceID = "SqlDataSource3";
                    ServerComboBox.DataSource = dt;

                    ServerComboBox.TextField = "Name";
                    ServerComboBox.ValueField = "Name";
                    ServerComboBox.DataBind();
                }
                else if (DeviceTypeComboBox.Text == "All")
                {
                    //ServerComboBox.DataSourceID = "SqlDataSource1";
                    //ServerComboBox.TextField = "DeviceName";
                    //ServerComboBox.ValueField = "DeviceName";
                    //ServerComboBox.DataBind();

                    FillDeviceTypeCombo();

                }
                else
                {
                    FillServerCombo();
                }
                              
            }
            else
            {
                FillServerCombo();
            }
        }

     
    }
}