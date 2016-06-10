using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using VSWebBL;
using DevExpress.Web;

namespace VSWebUI.Configurator
{
    public partial class DominoCustomStat : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FillDominoCustomStatGrid();
                if (Session["UserPreferences"] != null)
                {
                    DataTable UserPreferences = (DataTable)Session["UserPreferences"];
                    foreach (DataRow dr in UserPreferences.Rows)
                    {
                        if (dr[1].ToString() == "DominoCustomStat|DominoCustomGridView")
                        {
                            DominoCustomGridView.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                        }
                    }
                }
            }
            else
            {
                FillDominoCustomGridfromSession();
            }
            //1/21/2014 NS added for
            if (Session["DominoStatUpdateStatus"] != null)
            {
                if (Session["DominoStatUpdateStatus"].ToString() != "")
                {
                    //10/3/2014 NS modified for VSPLUS-990
                    successDiv.InnerHtml = "Custom statistics information for <b>" + Session["DominoStatUpdateStatus"].ToString() +
                        "</b> updated successfully."+
                        "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                    successDiv.Style.Value = "display: block";
                    Session["DominoStatUpdateStatus"] = "";
                }
            }
            //1/21/2014 NS added
            Session["Submenu"] = "LotusDomino";
        }

        private void FillDominoCustomStatGrid()
        {
            try
            {

                DataTable DCustomStatDataTable = new DataTable();

                DCustomStatDataTable = VSWebBL.ConfiguratorBL.DominoCustomStatBL.Ins.GetAllData();
                if (DCustomStatDataTable.Rows.Count >= 0)
                {

                    Session["DominoCustom"] = DCustomStatDataTable;
                    DominoCustomGridView.DataSource = DCustomStatDataTable;
                    DominoCustomGridView.DataBind();
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


        private void FillDominoCustomGridfromSession()
        {
            try
            {

                DataTable DCustomStatDataTable = new DataTable();
                if(Session["DominoCustom"]!=""&&Session["DominoCustom"]!=null)
                DCustomStatDataTable = (DataTable)Session["DominoCustom"];
                if (DCustomStatDataTable.Rows.Count > 0)
                {
                    DominoCustomGridView.DataSource = DCustomStatDataTable;
                    DominoCustomGridView.DataBind();
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
       

        protected void DominoCustomGridView_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType == GridViewRowType.EditForm)
            {
             
                //Mukund: VSPLUS-844, Page redirect on callback
                try
                {
                    if (e.GetValue("ID") != " " && e.GetValue("ID") != null)
                    {
                        ASPxWebControl.RedirectOnCallback("DominoStatistic.aspx?ID=" + e.GetValue("ID"));
                        Context.ApplicationInstance.CompleteRequest();//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
                        //1/21/2014 NS added
                        Session["Submenu"] = null;
                    }
                    else
                    {
                        ASPxWebControl.RedirectOnCallback("DominoStatistic.aspx");
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
       

        protected void DominoCustomGridView_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            DominoCustomStatValues DCustomObject = new DominoCustomStatValues();
            DCustomObject.ID = Convert.ToInt32(e.Keys[0]);
            //Delete row from DB
            Object ReturnValue = VSWebBL.ConfiguratorBL.DominoCustomStatBL.Ins.DeleteData(DCustomObject);

            //Update Grid after inserting new row, refresh grid as in page load
            ASPxGridView gridView = (ASPxGridView)sender;
            gridView.CancelEdit();
            e.Cancel = true;
            FillDominoCustomStatGrid();
        }

        protected void DominoCustomGridView_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            e.Cell.ToolTip = string.Format("{0}", e.CellValue);
        }

        protected void DominoCustomGridView_PageSizeChanged(object sender, EventArgs e)
        {
            //ProfilesGridView.PageIndex;
            VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("DominoCustomStat|DominoCustomGridView", DominoCustomGridView.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        }

        protected void NewButton_Click(object sender, EventArgs e)
        {
            //3/2/2015 NS added for VSPLUS-1432
            Response.Redirect("~/Configurator/DominoStatistic.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
            Context.ApplicationInstance.CompleteRequest();
        }

    }

}