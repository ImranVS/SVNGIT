using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DevExpress.Web;

namespace VSWebUI.Configurator
{
    public partial class ScheduledReportsGrid : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FillScheduledReportsGrid();
                FillReportNames();
            }
            else
            {
                FillScheduledReportsGridFromSession();
            }
        }

        private void FillScheduledReportsGrid()
        {
            try
            {
                DataTable dt = new DataTable();
                dt = VSWebBL.ConfiguratorBL.ReportsBL.Ins.GetReports(0);
                ScheduledReportsGridView.DataSource = dt;
                ScheduledReportsGridView.DataBind();
                if (dt.Rows.Count > 0)
                {
                    Session["ScheduledReports"] = dt;
                }
            }
            catch (Exception ex)
            { throw ex; }
            finally { }
        }

        private void FillScheduledReportsGridFromSession()
        {
            try
            {
                DataTable dt = new DataTable();
                if (Session["ScheduledReports"] != "" && Session["ScheduledReports"] != null)
                {
                    dt = (DataTable)Session["ScheduledReports"];
                }
                if (dt.Rows.Count > 0)
                {
                    ScheduledReportsGridView.DataSource = dt;
                    ScheduledReportsGridView.DataBind();
                }
            }
            catch (Exception ex)
            { throw ex; }
            finally { }
        }

        private void FillReportNames()
        {
            try
            {
                DataTable dt = new DataTable();
                dt = VSWebBL.ConfiguratorBL.ReportsBL.Ins.GetReportNames();
                GridViewDataComboBoxColumn srcb = (GridViewDataComboBoxColumn)ScheduledReportsGridView.Columns["ReportName"];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    srcb.PropertiesComboBox.Items.Add(dt.Rows[i]["Name"].ToString());
                }
            }
            catch (Exception ex)
            { throw ex; }
            finally { }
        }

        protected void ScheduledReportsGridView_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            ASPxGridView gridView = (ASPxGridView)sender;
            string reportName = e.NewValues["ReportName"].ToString();
            string reportTitle = e.NewValues["Title"].ToString();
            string reportID = "0";
            string frequency = e.NewValues["Frequency"].ToString();
            string sendto = e.NewValues["SendTo"].ToString();
            //ScheduledReports SRObject = new ScheduledReports(reportName,reportTitle,Convert.ToInt32(reportID),frequency,sendto);
            ScheduledReports SRObject = null;
            Object ReturnValue = VSWebBL.ConfiguratorBL.ReportsBL.Ins.InsertData(SRObject);
            gridView.CancelEdit();
            e.Cancel = true;
            FillScheduledReportsGrid();
        }

        protected void ScheduledReportsGridView_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            ASPxGridView gridView = (ASPxGridView)sender;
            string reportName = e.NewValues["ReportName"].ToString();
            string reportTitle = e.NewValues["Title"].ToString();
            string reportID = e.Keys[0].ToString();
            string frequency = e.NewValues["Frequency"].ToString();
            string sendto = e.NewValues["SendTo"].ToString();
            //ScheduledReports SRObject = new ScheduledReports(reportName, reportTitle, Convert.ToInt32(reportID), frequency, sendto);
            ScheduledReports SRObject = null;
            Object ReturnValue = VSWebBL.ConfiguratorBL.ReportsBL.Ins.UpdateData(SRObject);
            gridView.CancelEdit();
            e.Cancel = true;
            FillScheduledReportsGrid();
        }

        protected void ScheduledReportsGridView_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            ASPxGridView gridView = (ASPxGridView)sender;
            string reportID = e.Keys[0].ToString();
            //ScheduledReports SRObject = new ScheduledReports("", "", Convert.ToInt32(reportID), "", "");
            ScheduledReports SRObject = null;
            Object ReturnValue = VSWebBL.ConfiguratorBL.ReportsBL.Ins.DeleteData(SRObject);
            gridView.CancelEdit();
            e.Cancel = true;
            FillScheduledReportsGrid();
        }
    }
}