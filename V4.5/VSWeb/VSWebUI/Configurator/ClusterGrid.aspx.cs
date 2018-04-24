using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data;
using System.Web.UI.WebControls;
using VSWebBL;
using DevExpress.Web;

namespace VSWebUI.Configurator
{
    public partial class ClusterGrid : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //10/9/2013 NS modified in order to avoid an exception when Cluster grid is empty
           
            if (!IsPostBack)
            {

                if (Session["UserPreferences"] != null)
                {
                    DataTable UserPreferences = (DataTable)Session["UserPreferences"];
                    foreach (DataRow dr in UserPreferences.Rows)
                    {
                        if (dr[1].ToString() == "ClusterGrid|DominoClusterGridView")
                        {
                            DominoClusterGridView.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                        }
                    }
                }
            }
            /*else
            {

                FillDominoClusterGridfromSession();

            }
            */
			successDiv.Style.Value = "display:none";
            FillDominoClusterGrid();			
			string strMessage = string.Empty;
            //1/21/2014 NS added for
            if (Session["ClusterUpdateStatus"] != null)
            {
                if (Session["ClusterUpdateStatus"].ToString() != "")
                {
                    //10/3/2014 NS modified for VSPLUS-990
					strMessage = "Notes Database Replica information for <b>" + Session["ClusterUpdateStatus"].ToString() +
						"</b> updated successfully.<br>";
                   
                    Session["ClusterUpdateStatus"] = "";
                }
            }

			//if (!string.IsNullOrEmpty(Session["Status"].ToString()))
			if (Session["Status"] != null)
			{
				if (!string.IsNullOrEmpty(Session["Status"].ToString()))
				{
					{
						//10/3/2014 NS modified for VSPLUS-990
						strMessage = strMessage + "Status information for <b>" + Session["Status"].ToString() +
							"</b> updated successfully.";

						Session["Status"] = "";
					}
				}
			}
			if (!string.IsNullOrEmpty(strMessage))
			{
				successDiv.InnerHtml = strMessage+"<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
				successDiv.Style.Value = "display: block";
			}
			
        }



        private void FillDominoClusterGrid()
        {
            try
            {
                
                DataTable DCTaskSettingsDataTable = new DataTable();

                DCTaskSettingsDataTable = VSWebBL.ConfiguratorBL.DominoClusterBL.Ins.GetAllData();
                if (DCTaskSettingsDataTable.Rows.Count >= 0)
                {
                    
                    Session["DominoCluster"] = DCTaskSettingsDataTable;
                    DominoClusterGridView.DataSource = DCTaskSettingsDataTable;
                    DominoClusterGridView.DataBind();
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

        private void FillDominoClusterGridfromSession()
        {
            try
            {

                DataTable DCTaskSettingsDataTable = new DataTable();
                if(Session["DominoCluster"]!=""&& Session["DominoCluster"]!=null)
                DCTaskSettingsDataTable = (DataTable)Session["DominoCluster"];//VSWebBL.ConfiguratorBL.DominoPropertiesBL.Ins.GetAllData();
                if (DCTaskSettingsDataTable.Rows.Count > 0)
                {
                    DominoClusterGridView.DataSource = DCTaskSettingsDataTable;
                    DominoClusterGridView.DataBind();
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


        protected void DominoClusterGridView_HtmlRowCreated(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
        {
          
            if (e.RowType == GridViewRowType.EditForm)
            {
                //Mukund: VSPLUS-844, Page redirect on callback
                try
                {
                    if (e.GetValue("ID") != " " && e.GetValue("ID") != null)
                    {
                        ASPxWebControl.RedirectOnCallback("LotusDominoCluster.aspx?ID=" + e.GetValue("ID"));
                        Context.ApplicationInstance.CompleteRequest();//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback

                    }
                    else
                    {
                        ASPxWebControl.RedirectOnCallback("LotusDominoCluster.aspx");
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

        protected void DominoClusterGridView_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            DominoCluster DCObject = new DominoCluster();
            DCObject.ID = Convert.ToInt32(e.Keys[0]);
            //Delete row from DB
            Object ReturnValue = VSWebBL.ConfiguratorBL.DominoClusterBL.Ins.DeleteData(DCObject);

            //Update Grid after inserting new row, refresh grid as in page load
            ASPxGridView gridView = (ASPxGridView)sender;
            gridView.CancelEdit();
            e.Cancel = true;
            FillDominoClusterGrid();
        }

        protected void DominoClusterGridView_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            e.Cell.ToolTip = string.Format("{0}", e.CellValue);
        }

        protected void DominoClusterGridView_PageSizeChanged(object sender, EventArgs e)
        {
            //ProfilesGridView.PageIndex;
            VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("ClusterGrid|DominoClusterGridView", DominoClusterGridView.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        }

        protected void NewButton_Click(object sender, EventArgs e)
        {
            //3/2/2015 NS added for VSPLUS-1432
            Response.Redirect("~/Configurator/LotusDominoCluster.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
            Context.ApplicationInstance.CompleteRequest();
        }
    }
}