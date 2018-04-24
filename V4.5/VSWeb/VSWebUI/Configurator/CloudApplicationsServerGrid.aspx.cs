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
    public partial class CloudApplicationsServerGrid : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Title = "CloudApplicationsServer Details";
            if (!IsPostBack)
            {
                FillCloudApplicationsServerGrid();
                if (Session["UserPreferences"] != null)
                {
                    DataTable UserPreferences = (DataTable)Session["UserPreferences"];
                    foreach (DataRow dr in UserPreferences.Rows)
                    {
                        if (dr[1].ToString() == "CloudApplicationsServerGrid|CloudApplicationsServerGridView")
                        {
                            CloudApplicationsServerGridView.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                        }
                    }
                }
            }
            else
            {

                FillCloudApplicationsServerGridfromSession();

            }
            //3/19/2014 NS added
            if (Session["CloudApplicationsServerUpdateStatus"] != null)
            {
                if (Session["CloudApplicationsServerUpdateStatus"].ToString() != "")
                {
                    //10/9/2014 NS modified for VSPLUS-990
                    successDiv.InnerHtml = "Cloud Applications Server information for <b>" + Session["CloudApplicationsServerUpdateStatus"].ToString() +
                        "</b> updated successfully." +
                        "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                    successDiv.Style.Value = "display: block";
                    Session["CloudApplicationsServerUpdateStatus"] = "";
                }
            }
        }

        private void FillCloudApplicationsServerGrid()
        {
            try
            {

                DataTable DSTaskSettingsDataTable = new DataTable();

                DSTaskSettingsDataTable = VSWebBL.ConfiguratorBL.CloudApplicationsServerBL.Ins.GetAllData();
                if (DSTaskSettingsDataTable.Rows.Count >= 0)
                {
                    if (Session["RestrictedServers"] != "" && Session["RestrictedServers"] != null)
                    {
                        List<int> ServerID = new List<int>();
                        List<int> LocationID = new List<int>();
                        DataTable resServers = (DataTable)Session["RestrictedServers"];
                        foreach (DataRow resser in resServers.Rows)
                        {
                            foreach (DataRow Windowsrow in DSTaskSettingsDataTable.Rows)
                            {

                                if (resser["serverid"].ToString() == Windowsrow["ID"].ToString())
                                {
                                    ServerID.Add(DSTaskSettingsDataTable.Rows.IndexOf(Windowsrow));
                                }
                                if (resser["locationID"].ToString() == Windowsrow["locationid"].ToString())
                                {
                                    LocationID.Add(Convert.ToInt32(Windowsrow["locationid"].ToString()));
                                    //LocationID.Add(DSTaskSettingsDataTable.Rows.IndexOf(Windowsrow));
                                }
                            }

                        }
                        foreach (int Id in ServerID)
                        {
                            DSTaskSettingsDataTable.Rows[Id].Delete();
                        }
                        DSTaskSettingsDataTable.AcceptChanges();

                        //foreach (int Lid in LocationID)
                        //{
                        //    DSTaskSettingsDataTable.Rows[Lid].Delete();
                        //}
                        foreach (int lid in LocationID)
                        {
                            DataRow[] row = DSTaskSettingsDataTable.Select("locationid=" + lid + "");
                            for (int i = 0; i < row.Count(); i++)
                            {
                                DSTaskSettingsDataTable.Rows.Remove(row[i]);
                                DSTaskSettingsDataTable.AcceptChanges();
                            }
                        }
                        DSTaskSettingsDataTable.AcceptChanges();
                    }
                    Session["CloudApplicationsServer"] = DSTaskSettingsDataTable;
                    CloudApplicationsServerGridView.DataSource = DSTaskSettingsDataTable;
                    CloudApplicationsServerGridView.DataBind();
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



        private void FillCloudApplicationsServerGridfromSession()
        {
            try
            {

                DataTable CloudApplicationsServerDataTable = new DataTable();
                //Session["CloudApplicationsServer"] = CloudApplicationsServerDataTable;
                if (Session["CloudApplicationsServer"] != "" && Session["CloudApplicationsServer"] != null)
                {
                    CloudApplicationsServerDataTable = (DataTable)Session["CloudApplicationsServer"];//VSWebBL.ConfiguratorBL.DominoPropertiesBL.Ins.GetAllData();
                }
                else
                {
                    CloudApplicationsServerDataTable = VSWebBL.ConfiguratorBL.CloudApplicationsServerBL.Ins.GetAllData();
                }
                if (CloudApplicationsServerDataTable.Rows.Count >= 0)
                {
                    CloudApplicationsServerGridView.DataSource = CloudApplicationsServerDataTable;
                    CloudApplicationsServerGridView.DataBind();
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



        protected void CloudApplicationsServerGridView_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType == GridViewRowType.EditForm)
            {
                //11/19/2013 NS modified - can't use TheURL column value as it may not be unique
                //if (e.GetValue("TheURL") != " " && e.GetValue("TheURL")!=null)
                //{
                //    ASPxWebControl.RedirectOnCallback("URLProperties.aspx?TheURL=" + e.GetValue("TheURL"));
                //}
                //Mukund: VSPLUS-844, Page redirect on callback
                try
                {

                    if (e.GetValue("ID") != "" && e.GetValue("ID") != null)
                    {
                        ASPxWebControl.RedirectOnCallback("CloudApplicationsServerProperties.aspx?ID=" + e.GetValue("ID"));
                        Context.ApplicationInstance.CompleteRequest();//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback

                    }
                    else
                    {
                        ASPxWebControl.RedirectOnCallback("CloudApplicationsServerProperties.aspx");
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

        protected void CloudApplicationsServerGridView_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            CloudApplicationsServer CloudApplicationsServerObject = new CloudApplicationsServer();
            //11/19/2013 NS modified
            //URLObject.TheURL = (e.Keys[0]).ToString();
            CloudApplicationsServerObject.ID = (e.Keys[0]).ToString();
            //Delete row from DB
            Object ReturnValue = VSWebBL.ConfiguratorBL.CloudApplicationsServerBL.Ins.DeleteData(CloudApplicationsServerObject);

            //Update Grid after inserting new row, refresh grid as in page load
            ASPxGridView gridView = (ASPxGridView)sender;
            gridView.CancelEdit();
            e.Cancel = true;
            FillCloudApplicationsServerGrid();
        }

        protected void ProxySettingsButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("Proxy.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
            Context.ApplicationInstance.CompleteRequest();
        }

        protected void CloudApplicationsServerGridView_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            e.Cell.ToolTip = string.Format("{0}", e.CellValue);
        }

        protected void CloudApplicationsServerGridView_PageSizeChanged(object sender, EventArgs e)
        {
            //ProfilesGridView.PageIndex;
            VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("CloudApplicationsServerGrid|CloudApplicationsServerGridView", CloudApplicationsServerGridView.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        }

        protected void NewButton_Click(object sender, EventArgs e)
        {
            //3/2/2015 NS added for VSPLUS-1432
            Response.Redirect("~/Configurator/CloudApplicationsServerProperties.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
            Context.ApplicationInstance.CompleteRequest();
        }
    }
}