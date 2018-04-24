using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DevExpress.XtraReports.UI;

namespace VSWebUI.DashboardReports
{
    public partial class DailyMailVolumeRpt : System.Web.UI.Page
    {
        private static DailyMailVolumeRpt _self = new DailyMailVolumeRpt();

        public static DailyMailVolumeRpt Ins
        {
            get
            {
                return _self;
            }
        }
        DashboardReports.DailyMailVolumeXtraRpt report;

        public DashboardReports.DailyMailVolumeXtraRpt GetRpt()
        {
            report = new DashboardReports.DailyMailVolumeXtraRpt();
            DataTable dt = new DataTable();
            dt = VSWebBL.ReportsBL.XsdBL.Ins.getDailyMailVolumeBL("", "Domino");
            ((XRPivotGrid)report.FindControl("xrPivotGrid1", true)).DataSource = dt;
            return report;
        }

        public DataTable GetDT()
        {
            DataTable dt = new DataTable();
            dt = VSWebBL.ReportsBL.XsdBL.Ins.getDailyMailVolumeBL("", "Domino");
            return dt;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            FillReport();
            if (!IsPostBack)
            {   //26/4/2016 Durga Modified for VSPLUS-2883
                FillCombobox();
                FillServerslist(TypeComboBox.Text);
            }
        }

        public void FillReport()
        {
            string selectedServer = "";
         
            if (this.ServerListFilterListBox.SelectedItems.Count > 0)
            {
                selectedServer = "";
                for (int i = 0; i < this.ServerListFilterListBox.SelectedItems.Count; i++)
                {
                    selectedServer += "'" + this.ServerListFilterListBox.SelectedItems[i].Text + "'" + ",";
                }
                try
                {
                    selectedServer = selectedServer.Substring(0, selectedServer.Length - 1);
                }
                catch
                {
                    selectedServer = "";     // throw ex; 
                }
                finally { }
            }
            DashboardReports.DailyMailVolumeXtraRpt report = new DashboardReports.DailyMailVolumeXtraRpt();
            report.Parameters["ServerName"].Value = selectedServer;
            //12/18/2015 NS added for VSPLUS-2291
            report.Parameters["RptType"].Value = DailyMonthlyRadioButtonList.SelectedItem.Value.ToString();
            //26/4/2016 Durga Modified for VSPLUS-2883
            report.Parameters["ServerType"].Value = TypeComboBox.Text;
            report.CreateDocument();
            ASPxDocumentViewer1.Report = report;
            ASPxDocumentViewer1.DataBind();

        }
        //26/4/2016 Durga Modified for VSPLUS-2883
        public void FillServerslist(string ServerType)
        {
            DataTable d = new DataTable();
            d = VSWebBL.ReportsBL.ReportsBL.Ins.getDailyMailVolume(ServerType);
         
            ServerListFilterListBox.DataSource = d;
            ServerListFilterListBox.TextField = "ServerName";
            ServerListFilterListBox.ValueField = "ServerName";
            ServerListFilterListBox.DataBind();
        }


        protected void ServerListResetButton_Click(object sender, EventArgs e)
        {
            //this.ServerListFilterComboBox.SelectedIndex = -1;
            this.ServerListFilterListBox.UnselectAll();
            DashboardReports.DailyMailVolumeXtraRpt report = new DashboardReports.DailyMailVolumeXtraRpt();
            report.Parameters["ServerName"].Value = "";
            report.CreateDocument();
            ASPxDocumentViewer1.Report = report;
            ASPxDocumentViewer1.DataBind();
        }

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
        protected void ReptBtn_Click(object sender, EventArgs e)
        {

            Response.Redirect("~/Configurator/Reports.aspx?M=" + Request.QueryString["M"], false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
            Context.ApplicationInstance.CompleteRequest();
        }

        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            FillReport();
        }
        //12/18/2015 NS added for VSPLUS-2291
        protected void DailyMonthlyRadioButtonList_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillReport();
        }
        //3-5-2016 Durga Added for VSPLUS-2883
        public void FillCombobox()
        {
            DataTable Typetab = VSWebBL.DashboardBL.KeyMetricsBL.Ins.GetServerTypeForDailyMailVolume();
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
        //26/4/2016 Durga Modified for VSPLUS-2883
        protected void TypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillReport();

            FillServerslist(TypeComboBox.Text);

        }
    }
}