using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

using VSWebBL;
using DevExpress.Web;
using VSWebDO;

namespace VSWebUI.Configurator
{
    public partial class ExchangeMailProbe : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FillExchangeMailProbeGrid();
                FillExchangeMailProbeHistoryGrid();
                if (Session["UserPreferences"] != null)
                {
                    DataTable UserPreferences = (DataTable)Session["UserPreferences"];
                    foreach (DataRow dr in UserPreferences.Rows)
                    {
                        if (dr[1].ToString() == "ExchangeMailprobeGrid|ExchangeMailProbeGridView")
                        {
                            ExchangeMailProbeGridView.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                        }
                        if (dr[1].ToString() == "ExchangeMailprobeGrid|ExchangeMailProbeHistoryGridView")
                        {
                            ExchangeMailProbeHistoryGridView.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                        }
                    }
                }
            }
            else
            {

                FillExchangeMailProbeGridfromSession();
                FillExchangeMailProbeGridHistoryfromSession();

            }
            //2/20/2014 NS added
            if (Session["ExchangeMailProbeUpdateStatus"] != null)
            {
                if (Session["ExchangeMailProbeUpdateStatus"].ToString() != "")
                {
                    //10/3/2014 NS modified for VSPLUS-990
                    successDiv.InnerHtml = "ExchangeMail Probe information for <b>" + Session["ExchangeMailProbeUpdateStatus"].ToString() +
                        "</b> updated successfully."+
                        "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                    successDiv.Style.Value = "display: block";
                    Session["ExchangeMailProbeUpdateStatus"] = "";
                }
            }
        }


        private void FillExchangeMailProbeGrid()
        {
            try
            {

                DataTable ProbSettingsDataTable = new DataTable();

                ProbSettingsDataTable = VSWebBL.ConfiguratorBL.ExchangeMailProbeBL.Ins.GetAllData();
                if (ProbSettingsDataTable.Rows.Count >= 0)
                {

                    Session["ExchangeMailProbe"] = ProbSettingsDataTable;
                    ExchangeMailProbeGridView.DataSource = ProbSettingsDataTable;
                    ExchangeMailProbeGridView.DataBind();
                }
            }
            catch (Exception ex)
            {
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
            finally { }
        }


        private void FillExchangeMailProbeGridfromSession()
        {
            try
            {

                DataTable DCTaskSettingsDataTable = new DataTable();
                if (Session["ExchangeMailProbe"] != "" && Session["ExchangeMailProbe"] != null)
                    DCTaskSettingsDataTable = (DataTable)Session["ExchangeMailProbe"];//VSWebBL.ConfiguratorBL.DominoPropertiesBL.Ins.GetAllData();
                if (DCTaskSettingsDataTable.Rows.Count > 0)
                {
                    ExchangeMailProbeGridView.DataSource = DCTaskSettingsDataTable;
                    ExchangeMailProbeGridView.DataBind();
                }
            }
            catch (Exception ex)
            {
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
            finally { }
        }





        protected void ExchangeMailProbeGridView_HtmlRowCreated(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType == GridViewRowType.EditForm)
            {
             
                //Mukund: VSPLUS-844, Page redirect on callback
                try
                {
                    if (e.GetValue("Name") != null && e.GetValue("Name") != "")
                    {
                        ASPxWebControl.RedirectOnCallback("EditExchangeMailProbe.aspx?Name=" + e.GetValue("Name"));
                        Context.ApplicationInstance.CompleteRequest();//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
                    }
                    else
                    {
                        ASPxWebControl.RedirectOnCallback("EditExchangeMailProbe.aspx");
                        Context.ApplicationInstance.CompleteRequest();//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
                    }
                }
                catch (Exception ex)
                {
                    Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                    //throw ex;
                }
            }

        }

        protected void ExchangeMailProbeGridView_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            try
            {
                ExchangeMailProbeClass MailProbObject = new ExchangeMailProbeClass();
                MailProbObject.Name = e.Keys[0].ToString();
                //Delete row from DB
                Object ReturnValue = VSWebBL.ConfiguratorBL.ExchangeMailProbeBL.Ins.DeleteData(MailProbObject);

                //Update Grid after inserting new row, refresh grid as in page load
                ASPxGridView gridView = (ASPxGridView)sender;
                gridView.CancelEdit();
                e.Cancel = true;
                FillExchangeMailProbeGrid();
            }
            catch (Exception ex)
            {
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
        }

        private void FillExchangeMailProbeHistoryGrid()
        {
            try
            {

                DataTable ProbHistoryDataTable = new DataTable();

                ProbHistoryDataTable = VSWebBL.ConfiguratorBL.ExchangeMailProbeBL.Ins.GetAllHistoryData();
                if (ProbHistoryDataTable.Rows.Count > 0)
                {

                    Session["ExchangeMailProbeHistory"] = ProbHistoryDataTable;
                    ExchangeMailProbeHistoryGridView.DataSource = ProbHistoryDataTable;
                    ExchangeMailProbeHistoryGridView.DataBind();
                }
            }
            catch (Exception ex)
            {
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
            finally { }
        }

        private void FillExchangeMailProbeGridHistoryfromSession()
        {
            try
            {

                DataTable HistoryDataTable = new DataTable();
                if (Session["ExchangeMailProbeHistory"] != null && Session["ExchangeMailProbeHistory"] != "")
                    HistoryDataTable = (DataTable)Session["ExchangeMailProbeHistory"];//VSWebBL.ConfiguratorBL.DominoPropertiesBL.Ins.GetAllData();
                if (HistoryDataTable.Rows.Count > 0)
                {
                    ExchangeMailProbeHistoryGridView.DataSource = HistoryDataTable;
                    ExchangeMailProbeHistoryGridView.DataBind();
                }
            }
            catch (Exception ex)
            {
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
            finally { }
        }
        protected void ExchangeMailProbeGridView_PageSizeChanged(object sender, EventArgs e)
        {
            //ProfilesGridView.PageIndex;
            VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("ExchangeMailprobeGrid|ExchangeMailProbeGridView", ExchangeMailProbeGridView.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        }
        protected void ExchangeMailProbeHistoryGridView_PageSizeChanged(object sender, EventArgs e)
        {
            //ProfilesGridView.PageIndex;
            VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("ExchangeMailprobeGrid|ExchangeMailProbeHistoryGridView", ExchangeMailProbeHistoryGridView.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        }

        protected void NewButton_Click(object sender, EventArgs e)
        {
            //3/2/2015 NS added for VSPLUS-1432
            Response.Redirect("~/Configurator/EditExchangeMailProbe.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
            Context.ApplicationInstance.CompleteRequest();
        }


    }
}