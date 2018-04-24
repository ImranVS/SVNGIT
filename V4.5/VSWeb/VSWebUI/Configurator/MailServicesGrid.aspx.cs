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
    public partial class MailServicesGrid : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
           
            if (!IsPostBack)
            {
               
                FillMailServiceGrid();
                if (Session["UserPreferences"] != null)
                {
                    DataTable UserPreferences = (DataTable)Session["UserPreferences"];
                    foreach (DataRow dr in UserPreferences.Rows)
                    {
                        if (dr[1].ToString() == "MailServicesGrid|MailServicesGridView")
                        {
                            MailServicesGridView.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                        }
                    }
                }
            }
            else
            {

                FillMailServicesGridfromSession();

            }
            //3/19/2014 NS added
            if (Session["MailServiceUpdateStatus"] != null)
            {
                if (Session["MailServiceUpdateStatus"].ToString() != "")
                {
                    //10/3/2014 NS modified for VSPLUS-990
                    successDiv.InnerHtml = "Mail Service information for <b>" + Session["MailServiceUpdateStatus"].ToString() +
                        "</b> updated successfully."+
                        "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                    successDiv.Style.Value = "display: block";
                    Session["MailServiceUpdateStatus"] = "";
                }
            }
        }

        private void FillMailServiceGrid()
        {
            try
            {

                DataTable MailServicesDataTable = new DataTable();

                MailServicesDataTable = VSWebBL.ConfiguratorBL.MailServicesBL.Ins.GetAllData();
                if (MailServicesDataTable.Rows.Count >= 0)
                {

                    Session["MailServices"] = MailServicesDataTable;
                    MailServicesGridView.DataSource = MailServicesDataTable;
                    MailServicesGridView.DataBind();
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
        private void FillMailServicesGridfromSession()
        {
            try
            {

                DataTable MailServicesDataTable = new DataTable();
                if (Session["MailServices"] != "" && Session["MailServices"]!=null)
                MailServicesDataTable = (DataTable)Session["MailServices"];//VSWebBL.ConfiguratorBL.DominoPropertiesBL.Ins.GetAllData();
                if (MailServicesDataTable.Rows.Count > 0)
                {
                    MailServicesGridView.DataSource = MailServicesDataTable;
                    MailServicesGridView.DataBind();
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

        protected void MailServicesGridView_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType == GridViewRowType.EditForm)
            {
             
                //Mukund: VSPLUS-844, Page redirect on callback
                try
                {
                    if (e.GetValue("key") != " ")
                    {
                        ASPxWebControl.RedirectOnCallback("MailService.aspx?Key=" + e.GetValue("key"));
                        Context.ApplicationInstance.CompleteRequest();//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback

                    }
                    else
                    {
                        ASPxWebControl.RedirectOnCallback("MailService.aspx");
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

        protected void MailServicesGridView_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            MailServices MSObject = new MailServices();
            if(e.Values[9].ToString()!="")
            MSObject.key = int.Parse(e.Values[9].ToString());
            //Delete row from DB
            Object ReturnValue = VSWebBL.ConfiguratorBL.MailServicesBL.Ins.DeleteData(MSObject);

            //Update Grid after inserting new row, refresh grid as in page load
            ASPxGridView gridView = (ASPxGridView)sender;
            gridView.CancelEdit();
            e.Cancel = true;
            FillMailServiceGrid();

        }

        protected void MailServicesGridView_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            e.Cell.ToolTip = string.Format("{0}", e.CellValue);
        }

        protected void MailServicesGridView_PageSizeChanged(object sender, EventArgs e)
        {
            //ProfilesGridView.PageIndex;
            VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("MailServicesGrid|MailServicesGridView", MailServicesGridView.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        }

        protected void NewButton_Click(object sender, EventArgs e)
        {
            //3/2/2015 NS added for VSPLUS-1432
            Response.Redirect("~/Configurator/MailService.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
            Context.ApplicationInstance.CompleteRequest();
        }

    }
}