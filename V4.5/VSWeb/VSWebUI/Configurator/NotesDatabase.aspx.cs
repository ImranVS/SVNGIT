using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using VSWebBL;
using VSWebDO;
using DevExpress.Web;

namespace VSWebUI.Configurator
{
    public partial class NotesDatabase : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FillNotesDatabaseGrid();
                if (Session["UserPreferences"] != null)
                {
                    DataTable UserPreferences = (DataTable)Session["UserPreferences"];
                    foreach (DataRow dr in UserPreferences.Rows)
                    {
                        if (dr[1].ToString() == "NotesDatabase|NotesDatabaseGridView")
                        {
                            NotesDatabaseGridView.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                        }
                    }
                }
            }
            else
            {
                FillNotesDatabaseFromSession();
            }
            //1/21/2014 NS added for
            if (Session["NotesDBUpdateStatus"] != null)
            {
                if (Session["NotesDBUpdateStatus"].ToString() != "")
                {
                    //10/6/2014 NS modified for VSPLUS-990
                    successDiv.InnerHtml = "Notes database information for <b>" + Session["NotesDBUpdateStatus"].ToString() +
                        "</b> updated successfully."+
                        "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                    successDiv.Style.Value = "display: block";
                    Session["NotesDBUpdateStatus"] = "";
                }
            }
        }

        private void FillNotesDatabaseFromSession()
        {
            try
            {

                DataTable NotesDatabaseDataTable = new DataTable();
                if(Session["NotesDatabase"]!=null&&Session["NotesDatabase"]!="")
                NotesDatabaseDataTable = (DataTable)Session["NotesDatabase"];
                if (NotesDatabaseDataTable.Rows.Count > 0)
                {  
                    NotesDatabaseGridView.DataSource = NotesDatabaseDataTable;
                    NotesDatabaseGridView.DataBind();
                }
            }
            catch (Exception ex)
            {
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex; }
            finally { }
        }

        private void FillNotesDatabaseGrid()
        {
            try
            {

                DataTable NotesDatabaseDataTable = new DataTable();

                NotesDatabaseDataTable = VSWebBL.ConfiguratorBL.NotesDatabaseBL.Ins.GetAllData();
                if (NotesDatabaseDataTable.Rows.Count >= 0)
                {
                    Session["NotesDatabase"] = NotesDatabaseDataTable;
                    NotesDatabaseGridView.DataSource = NotesDatabaseDataTable;
                    NotesDatabaseGridView.DataBind();
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
        protected void NotesDatabaseGridView_HtmlRowCreated(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType == GridViewRowType.EditForm)
            {
                //Mukund: VSPLUS-844, Page redirect on callback
                try
                {
                    if (e.GetValue("ID") != null)
                    {
                        ASPxWebControl.RedirectOnCallback("EditNotes.aspx?ID=" + e.GetValue("ID"));
                        Context.ApplicationInstance.CompleteRequest();//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
                    }
                    else
                    {
                        ASPxWebControl.RedirectOnCallback("EditNotes.aspx");
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

        protected void NotesDatabaseGridView_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            NotesDatabases NDObject = new NotesDatabases();
            NDObject.ID = Convert.ToInt32(e.Keys[0]);
            //Delete row from DB
            Object ReturnValue = VSWebBL.ConfiguratorBL.NotesDatabaseBL.Ins.DeleteData(NDObject);
            
            
            //Update Grid after inserting new row, refresh grid as in page load
            ASPxGridView gridView = (ASPxGridView)sender;
            gridView.CancelEdit();
            e.Cancel = true;
            FillNotesDatabaseGrid();
        }

        protected void NotesDatabaseGridView_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            e.Cell.ToolTip = string.Format("{0}", e.CellValue);
        }

        protected void NotesDatabaseGridView_PageSizeChanged(object sender, EventArgs e)
        {
            //ProfilesGridView.PageIndex;
            VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("NotesDatabase|NotesDatabaseGridView", NotesDatabaseGridView.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        }

        protected void NewButton_Click(object sender, EventArgs e)
        {
            //3/2/2015 NS added for VSPLUS-1432
            Response.Redirect("~/Configurator/EditNotes.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
            Context.ApplicationInstance.CompleteRequest();
        }

    }
}