using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DevExpress.Web;

namespace VSWebUI.Dashboard
{
    public partial class SummaryEXJournal : System.Web.UI.Page
    {
		protected void Page_PreInit(object sender, EventArgs e)
		{

			this.Master.contentCallEvent += new EventHandler(Master_ButtonClick);
            if (Session["showsummary"] != null)
            {
                if (Session["showsummary"].ToString() == "False")
                {
                    if (Session["UserLogin"] == null || Session["UserLogin"] == "")
                    {
                        Response.Redirect("~/login.aspx", false);
                        Context.ApplicationInstance.CompleteRequest();
                    }

                }
            }
		}

		private void Master_ButtonClick(object sender, EventArgs e)
		{
            //8/26/2015 NS added
            FillGrid();
		}
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["UserPreferences"] != null)
                {
                    DataTable UserPreferences = (DataTable)Session["UserPreferences"];
                    foreach (DataRow dr in UserPreferences.Rows)
                    {
                        if (dr[1].ToString() == "SummaryEXJournal|EXJournalGridView")
                        {
                            EXJournalGridView.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                        }
                    }
                }
            }
            //10/3/2013 NS commented out
            //if (!IsPostBack)
            //{
            FillGrid();
            //}
            //else
            //{
            //    FillGridFromSession();
            //}
            //10/3/2013 NS added
            //if (Session["Refreshtime"] != "" && Session["Refreshtime"] != null)
            //    timerupdate.Interval = Convert.ToInt32(Session["Refreshtime"]) * 1000;
        }

        public void FillGrid()
        {
            try
            {
                DataTable StatusTable = VSWebBL.DashboardBL.DashboardBL.Ins.GetEXJournalData();
                EXJournalGridView.DataSource = StatusTable;
                EXJournalGridView.DataBind();
                Session["StatusTable"] = StatusTable;
                Session["SummaryEXJournal"] = "SummaryEXJournal"; //MD 30Dec2013   
            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
            finally
            {
            }
        }

        public void FillGridFromSession()
        {
            if (Session["StatusTable"] != "" && Session["StatusTable"] != null)
            {
                DataTable StatusTable = Session["StatusTable"] as DataTable;
                try
                {
                    EXJournalGridView.DataSource = StatusTable;
                    EXJournalGridView.DataBind();
                }
                catch (Exception ex)
                {
                    Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                    throw ex;
                }
                finally
                {
                }
            }
        }

        protected void EXJournalGridView_HtmlDataCellPrepared(object sender, DevExpress.Web.ASPxGridViewTableDataCellEventArgs e)
        {
            ASPxGridView gv = sender as ASPxGridView;
            Label hfStatus = gv.FindRowCellTemplateControl(e.VisibleIndex, e.DataColumn, "hfNameLabel") as Label;
            //10/4/2013 NS added
            Label hfExTotal = gv.FindRowCellTemplateControl(e.VisibleIndex, e.DataColumn, "hfExTotalLabel") as Label;
            string ExJournalThreshold = "1000";
            try
            {
                ExJournalThreshold = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("ExJournal Threshold");
            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }

            if (e.DataColumn.FieldName == "EXTotal" && ExJournalThreshold != "")
            {
                if (Convert.ToInt32(hfExTotal.Text.ToString()) > Convert.ToInt32(ExJournalThreshold))
                {
                    e.Cell.ForeColor = System.Drawing.Color.Red;
                }
            }

            if (e.DataColumn.FieldName == "Status" && (hfStatus.Text.ToString() == "OK" || hfStatus.Text.ToString() == "Scanning" || hfStatus.Text.ToString() == "Telnet"))
            {
                e.Cell.BackColor = System.Drawing.Color.LightGreen;
            }

            else if (e.DataColumn.FieldName == "Status" && hfStatus.Text.ToString() == "Not Responding")
            {
                e.Cell.BackColor = System.Drawing.Color.Red;
                e.Cell.ForeColor = System.Drawing.Color.White;
            }
            else if (e.DataColumn.FieldName == "Status" && hfStatus.Text.ToString() == "Not Scanned")
            {

                e.Cell.BackColor = System.Drawing.Color.FromName("#87CEEB");
                e.Cell.ForeColor = System.Drawing.Color.Black;
            }
            else if (e.DataColumn.FieldName == "Status" && hfStatus.Text.ToString() == "Disabled")
            {
                e.Cell.BackColor = System.Drawing.Color.FromName("#D0D0D0");
            }
            else if (e.DataColumn.FieldName == "Status" && hfStatus.Text.ToString() == "Maintenance")
            {
                e.Cell.BackColor = System.Drawing.Color.LightBlue;
            }
            else if (e.DataColumn.FieldName == "Status")
            {
                e.Cell.BackColor = System.Drawing.Color.Yellow;
            }
        }

        protected void EXJournalGridView_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
        {
            e.Row.Attributes.Add("onmouseover", "this.originalcolor=this.style.backgroundColor;" + "this.style.backgroundColor='#C0C0C0';");
            e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalcolor;");
        }

        protected void EXJournalGridView_PageSizeChanged(object sender, EventArgs e)
        {
            //ProfilesGridView.PageIndex;
            VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("SummaryEXJournal|EXJournalGridView", EXJournalGridView.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        }
    }
}