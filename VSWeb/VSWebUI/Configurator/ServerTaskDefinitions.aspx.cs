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
    public partial class ServerTasksDef : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
           if (!IsPostBack)
            {
                FillDominoServerTasksGrid();
                if (Session["UserPreferences"] != null)
                {
                    DataTable UserPreferences = (DataTable)Session["UserPreferences"];
                    foreach (DataRow dr in UserPreferences.Rows)
                    {
                        if (dr[1].ToString() == "ServerTaskDefinitions|ServerTaskDefGridView")
                        {
                            ServerTaskDefGridView.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                        }
                    }
                }
            }
            else
            {
                FillDominoServerTasksGridfromSession();
            }
           //1/21/2014 NS added for
           if (Session["ServerTaskUpdateStatus"] != null)
           {
               if (Session["ServerTaskUpdateStatus"].ToString() != "")
               {
                   //10/6/2014 NS modified for VSPLUS-990
                   successDiv.InnerHtml = "Server task information for <b>" + Session["ServerTaskUpdateStatus"].ToString() +
                       "</b> updated successfully."+
                       "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                   successDiv.Style.Value = "display: block";
                   Session["ServerTaskUpdateStatus"] = "";
               }
           }
        }


        private void FillDominoServerTasksGrid()
        {
            try
            {

                DataTable DSTasksDataTable = new DataTable();

                DSTasksDataTable = VSWebBL.ConfiguratorBL.DominoServerTasksBL.Ins.GetAllData();
                if (DSTasksDataTable.Rows.Count >= 0)
                {

                    Session["DominoCluster"] = DSTasksDataTable;
                    ServerTaskDefGridView.DataSource = DSTasksDataTable;
                    ServerTaskDefGridView.DataBind();
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
        private void FillDominoServerTasksGridfromSession()
        {
            try
            {

                DataTable DSTasksDataTable = new DataTable();
                if(Session["DominoCluster"]!=""&&Session["DominoCluster"]!=null)
                DSTasksDataTable = (DataTable)Session["DominoCluster"];
                if (DSTasksDataTable.Rows.Count > 0)
                {
                    ServerTaskDefGridView.DataSource = DSTasksDataTable;
                    ServerTaskDefGridView.DataBind();
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



        protected void ServerTaskDefGridView_HtmlRowCreated(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType == GridViewRowType.EditForm)
            {
                //Mukund: VSPLUS-844, Page redirect on callback
                try
                {

                    if (e.GetValue("TaskID") != " ")
                    {
                        ASPxWebControl.RedirectOnCallback("EditServerTask.aspx?TaskID=" + e.GetValue("TaskID"));
                        Context.ApplicationInstance.CompleteRequest();//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback

                    }
                    else
                    {
                        ASPxWebControl.RedirectOnCallback("EditServerTask.aspx");
                    }
                }
                catch (Exception ex)
                {
                    Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                    //throw ex;
                }
            }
        }

        protected void ServerTaskDefGridView_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            DominoServerTasks DSTObject = new DominoServerTasks();
            DSTObject.TaskID = Convert.ToInt32(e.Keys[0]);
            //Delete row from DB
            Object ReturnValue = VSWebBL.ConfiguratorBL.DominoServerTasksBL.Ins.DeleteData(DSTObject);

            //Update Grid after inserting new row, refresh grid as in page load
            ASPxGridView gridView = (ASPxGridView)sender;
            gridView.CancelEdit();
            e.Cancel = true;
            FillDominoServerTasksGrid();
        }

        protected void ServerTaskDefGridView_PageSizeChanged(object sender, EventArgs e)
        {
            //ProfilesGridView.PageIndex;
            VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("ServerTaskDefinitions|ServerTaskDefGridView", ServerTaskDefGridView.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        }

        protected void NewButton_Click(object sender, EventArgs e)
        {
            //3/2/2015 NS added for VSPLUS-1432
            Response.Redirect("~/Configurator/EditServerTask.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
            Context.ApplicationInstance.CompleteRequest();
        }
    }
}