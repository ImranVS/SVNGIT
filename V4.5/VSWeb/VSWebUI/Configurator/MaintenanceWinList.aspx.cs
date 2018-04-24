using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using System.Data;
using VSWebDO;
using VSWebBL;

namespace VSWebUI.Configurator
{
    public partial class MaintenanceWinList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Page.Title = "Maintenance Window";
            Session["ReturnUrl"] = null;
            if (!IsPostBack)
            {
                FillMaintenanceGrid();
                if (Session["UserPreferences"] != null)
                {
                    DataTable UserPreferences = (DataTable)Session["UserPreferences"];
                    foreach (DataRow dr in UserPreferences.Rows)
                    {
                        if (dr[1].ToString() == "MaintenanceWinList|MaintWinListGridView")
                        {
                            MaintWinListGridView.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                        }
                    }
                }
            }
            else
            {

                FillMaintServersGridfromSession();

            }
			if(Session["MaintenanceWinList"] != null)
			{
			if(!string.IsNullOrEmpty(Session["MaintenanceWinList"].ToString()))
			//if (Session["MaintenanceWinList"] != null)
			//{
			//    if (Session["MaintenanceWinList"].ToString() != "")
				{
					//10/6/2014 NS modified for VSPLUS-990
					successDiv.InnerHtml = "Maintenance Window information for <b>" + Session["MaintenanceWinList"].ToString() +
						"</b> updated successfully." +
						"<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
					successDiv.Style.Value = "display: block";
					Session["MaintenanceWinList"] = "";
				}
			}
        }

        private void FillMaintenanceGrid()
        {
            try
            {

             DataTable MaintDataTable = new DataTable();
                DataSet ServersDataSet = new DataSet();
                MaintDataTable = VSWebBL.ConfiguratorBL.MaintenanceBL.Ins.GetAllMaintenData();
                if (MaintDataTable.Rows.Count >= 0)
                {
                    DataTable dtcopy = MaintDataTable.Copy();
                    dtcopy.PrimaryKey = new DataColumn[] { dtcopy.Columns["ID"] };
                    Session["MaintServers"] = dtcopy;
                    MaintWinListGridView.DataSource = MaintDataTable;
                    MaintWinListGridView.DataBind();
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

        private void FillMaintServersGridfromSession()
        {
            try
            {

              DataTable ServersDataTable = new DataTable();
                if(Session["MaintServers"]!=null && Session["MaintServers"]!="")
                ServersDataTable = (DataTable)Session["MaintServers"];//VSWebBL.ConfiguratorBL.DominoPropertiesBL.Ins.GetAllData();
                if (ServersDataTable.Rows.Count >= 0)
                {
                    MaintWinListGridView.DataSource = ServersDataTable;
                    MaintWinListGridView.DataBind();
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

        
        

        protected void MaintWinListGridView_HtmlRowCreated(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType == GridViewRowType.EditForm)
            {
                //Mukund: VSPLUS-844, Page redirect on callback
                try
                {
                    ASPxWebControl.RedirectOnCallback("MaintenanceWin.aspx?ID=" + e.GetValue("ID"));
                    Context.ApplicationInstance.CompleteRequest();//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
                }
                catch (Exception ex)
                {
                    Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                    //throw ex;
                }
            }
        }
        protected void btn_click(object sender, EventArgs e)
        {
            ImageButton btn = (ImageButton)sender;

            int Id = Convert.ToInt32(btn.CommandArgument);
            string URL = "MaintenanceWin.aspx?ID=" + Id.ToString() + "&Copy=True";
            Response.Redirect("MaintenanceWin.aspx?ID=" + Id.ToString() + "&Copy=True", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
            Context.ApplicationInstance.CompleteRequest();
           // ASPxWebControl.RedirectOnCallback(URL);
                        
        }

        protected void MaintWinListGridView_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            Maintenance maintobj = new Maintenance();
          maintobj.ID = int.Parse(e.Keys[0].ToString());

            //delete row from DB
            bool returnval = VSWebBL.ConfiguratorBL.MaintenanceBL.Ins.delete(maintobj);
            ASPxGridView gridView = (ASPxGridView)sender;
            gridView.CancelEdit();
            e.Cancel = true;
            FillMaintenanceGrid();
    
        }

        protected void MaintWinListGridView_PageSizeChanged(object sender, EventArgs e)
        {
            //ProfilesGridView.PageIndex;
            VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("MaintenanceWinList|MaintWinListGridView", MaintWinListGridView.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        }

        protected void DeletePastButton_Click(object sender, EventArgs e)
        {
			successDiv.Style.Value = "display :none";
            DeletePopupControl.ShowOnPageLoad = true;
        }

        protected void CancelButton_Click(object sender, EventArgs e)
        {
			successDiv.Style.Value = "display :none";
            DeletePopupControl.ShowOnPageLoad = false;
        }

        protected void OKButton_Click(object sender, EventArgs e)
        {
            VSWebBL.ConfiguratorBL.MaintenanceWindowsBL.Ins.DeletePastMaintWin();
            FillMaintenanceGrid();
            DeletePopupControl.ShowOnPageLoad = false;
        }

        protected void NewButton_Click(object sender, EventArgs e)
        {
            //3/2/2015 NS added for VSPLUS-1432
            Response.Redirect("~/Configurator/MaintenanceWin.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
            Context.ApplicationInstance.CompleteRequest();
        }
    }
}