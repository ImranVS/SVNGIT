using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Collections;
using DevExpress.Web;
using System.Web.UI.HtmlControls;

namespace VSWebUI.Configurator
{
    public partial class Reports : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            FillGrid();
            if (Session["SchedRptUpdateStatus"] != null)
            {
                if (Session["SchedRptUpdateStatus"].ToString() != "")
                {
                    successDiv.InnerHtml = "Schedule Report information for <b>" + Session["SchedRptUpdateStatus"].ToString() +
                        "</b> updated successfully." +
                        "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                    successDiv.Style.Value = "display: block";
                    Session["SchedRptUpdateStatus"] = "";
                }
            }
        }

        public void FillGrid()
        {
            DataTable dt = new DataTable();
            dt = VSWebBL.ConfiguratorBL.ReportsBL.Ins.GetReports(0);
            RptGridView.DataSource = dt;
            RptGridView.DataBind();
        }

        protected void RptGridView_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {

            ASPxGridView gridview = (ASPxGridView)sender;
            gridview.CancelEdit();
            e.Cancel = true;
            FillGrid();
        }

        protected void RptGridView_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType == GridViewRowType.EditForm)
            {
                try
                {
                    if (e.GetValue("ID") != "" && e.GetValue("ID") != null)
                    {
                        ASPxWebControl.RedirectOnCallback("ScheduledReports_Edit.aspx?ID=" + e.GetValue("ID"));
                        Context.ApplicationInstance.CompleteRequest();
                    }
                    else
                    {
                        ASPxWebControl.RedirectOnCallback("ScheduledReports_Edit.aspx");
                        Context.ApplicationInstance.CompleteRequest();
                    }
                }
                catch (Exception ex)
                {
                    Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                    //throw ex;
                }
            }
        }

        protected void NewButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Configurator/ScheduledReports_Edit.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }

        protected void RptGridView_PageSizeChanged(object sender, EventArgs e)
        {
            VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("Reports|RptGridView", RptGridView.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
            UI.Ins.ChangeUserPreference("Reports|DominoServerGridView", RptGridView.SettingsPager.PageSize);

        }

        protected void ASPxMenu1_ItemClick(object source, MenuItemEventArgs e)
        {
            Response.Redirect("~/DashboardReports/OverallSrvStatusHealthRpt.aspx?M=c", false);
            Context.ApplicationInstance.CompleteRequest();
        }
    }
}