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
    public partial class MailDeliveryStatus : System.Web.UI.Page
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

		}
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FillGrid();
                //2/9/2015 NS added for VSPLUS-1446
                FillQueueGrid();
                if (Session["UserPreferences"] != null)
                {
                    DataTable UserPreferences = (DataTable)Session["UserPreferences"];
                    foreach (DataRow dr in UserPreferences.Rows)
                    {
                        if (dr[1].ToString() == "MailDeliveryStatus|EXJournalGridView")
                        {
                            EXJournalGridView.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                        }
                        else if (dr[1].ToString() == "ExMailDeliveryStatus|ExchangeMailGridView")
                        {
                            ExchangeMailGridView.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                        }
                    }
                }
            }
            else
            {
                FillGridFromSession();
                //2/9/2015 NS added for VSPLUS-1446
                FillQueueGridFromSession();
            }
        }

        public void FillGrid()
        {
            try
            {
                //11/20/2013 NS modified
                //DataTable StatusTable = VSWebBL.DashboardBL.DashboardBL.Ins.GetEXJournalData();
                DataTable StatusTable = VSWebBL.DashboardBL.MailHealthBL.Ins.GetMailDelivData("Domino");
                EXJournalGridView.DataSource = StatusTable;
                EXJournalGridView.DataBind();
                Session["StatusTable"] = StatusTable;
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

        protected void FilterButton_Click(object sender, EventArgs e)
        {
            FillGrid();
        }

        protected void EXJournalGridView_PageSizeChanged(object sender, EventArgs e)
        {
            //ProfilesGridView.PageIndex;
            VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("MailDeliveryStatus|EXJournalGridView", EXJournalGridView.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        }

        public void FillQueueGrid()
        {
            try
            {
                DataTable StatusTable = VSWebBL.DashboardBL.MailHealthBL.Ins.GetMailDelivData("Exchange");
                ExchangeMailGridView.DataSource = StatusTable;
                ExchangeMailGridView.DataBind();
                Session["ExStatusTable"] = StatusTable;
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

        public void FillQueueGridFromSession()
        {
            if (Session["ExStatusTable"] != "" && Session["ExStatusTable"] != null)
            {
                DataTable StatusTable = Session["ExStatusTable"] as DataTable;
                try
                {
                    ExchangeMailGridView.DataSource = StatusTable;
                    ExchangeMailGridView.DataBind();
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

        protected void ExchangeMailGridView_PageSizeChanged(object sender, EventArgs e)
        {
            VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("ExMailDeliveryStatus|ExchangeMailGridView", ExchangeMailGridView.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        }

        protected void ExchangeMailGridView_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            ASPxGridView gv = sender as ASPxGridView;
            Label hfStatus = gv.FindRowCellTemplateControl(e.VisibleIndex, e.DataColumn, "hfExNameLabel") as Label;


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
    }
}