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
    public partial class O365ServerGrid : System.Web.UI.Page
    {
		string mode;
        protected void Page_Load(object sender, EventArgs e)
        {
            //Page.Title = "O365Server Details";
            if (!IsPostBack)
            {
                FillO365ServerGrid();
                if (Session["UserPreferences"] != null)
                {
                    DataTable UserPreferences = (DataTable)Session["UserPreferences"];
                    foreach (DataRow dr in UserPreferences.Rows)
                    {
                        if (dr[1].ToString() == "O365ServerGrid|O365ServerGridView")
                        {
                            O365ServerGridView.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                        }
                    }
                }
            }
            else
            {

                FillO365ServerGridfromSession();

            }
            //3/19/2014 NS added
			if ((Session["O365ServerUpdateStatus"] != null))
            {
				if (Session["O365ServerUpdateStatus"].ToString() != "")
				{
					//10/9/2014 NS modified for VSPLUS-990
					if ((Request.QueryString["modemes"] == "Update"))
					{
						successDiv.InnerHtml = "Office 365 Server information for <b>" + Session["O365ServerUpdateStatus"].ToString() +
							"</b> updated successfully." +
							"<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
						successDiv.Style.Value = "display: block";
						Session["O365ServerUpdateStatus"] = "";
					}
					else if ((Request.QueryString["modemes"] == "Insert"))
					{
						successDiv.InnerHtml = "Office 365 Server information for <b>" + Session["O365ServerUpdateStatus"].ToString() +
						"</b> saved successfully." +
						"<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
						successDiv.Style.Value = "display: block";
						Session["O365ServerUpdateStatus"] = "";
					}
				}
            }
        }

  private void FillO365ServerGrid()

        {
            try
            {

                DataTable DSTaskSettingsDataTable = new DataTable();

                DSTaskSettingsDataTable = VSWebBL.ConfiguratorBL.O365ServerBL.Ins.GetAllData();
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
                    Session["O365Server"] = DSTaskSettingsDataTable;
					O365ServerGridView.DataSource = DSTaskSettingsDataTable;
					O365ServerGridView.DataBind();
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



        private void FillO365ServerGridfromSession()
        {
            try
            {

                DataTable O365ServerDataTable = new DataTable();
                if (Session["O365Server"] != "" && Session["O365Server"] != null)
                    O365ServerDataTable = (DataTable)Session["O365Server"];//VSWebBL.ConfiguratorBL.DominoPropertiesBL.Ins.GetAllData();
                if (O365ServerDataTable.Rows.Count >= 0)
                {
                    O365ServerGridView.DataSource = O365ServerDataTable;
                    O365ServerGridView.DataBind();
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



        protected void O365ServerGridView_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
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
						mode="Update";
						//Response.Redirect("AlertEventTemplate_Edit.aspx?Key=" + e.GetValue("ID") + "&Mode=" + mode + "&Name=" + e.GetValue("Name"), false);
                        ASPxWebControl.RedirectOnCallback("O365ServerProperties.aspx?ID=" + e.GetValue("ID")+"&Mode=" + mode);
                       // Context.ApplicationInstance.CompleteRequest();//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback

                    }
                    else
                    {
						mode = "Insert";
                        ASPxWebControl.RedirectOnCallback("O365ServerProperties.aspx?Mode=" + mode);
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

        protected void O365ServerGridView_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            O365Server O365ServerObject = new O365Server();
            //11/19/2013 NS modified
            //URLObject.TheURL = (e.Keys[0]).ToString();
            O365ServerObject.ID = (e.Keys[0]).ToString();
			DataTable o365data = VSWebBL.ConfiguratorBL.O365ServerBL.Ins.Get((e.Keys[0]).ToString());
			string name = o365data.Rows[0]["Name"].ToString();
			O365ServerObject.Name = name;
            //Delete row from DB
            Object ReturnValue = VSWebBL.ConfiguratorBL.O365ServerBL.Ins.DeleteData(O365ServerObject);

            //Update Grid after inserting new row, refresh grid as in page load
            ASPxGridView gridView = (ASPxGridView)sender;
            gridView.CancelEdit();
            e.Cancel = true;
            FillO365ServerGrid();
        }

        protected void ProxySettingsButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("Proxy.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
            Context.ApplicationInstance.CompleteRequest();
        }

        protected void O365ServerGridView_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            e.Cell.ToolTip = string.Format("{0}", e.CellValue);
        }

        protected void O365ServerGridView_PageSizeChanged(object sender, EventArgs e)
        {
            //ProfilesGridView.PageIndex;
            VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("O365ServerGrid|O365ServerGridView", O365ServerGridView.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        }

        protected void NewButton_Click(object sender, EventArgs e)
        {
            //3/2/2015 NS added for VSPLUS-1432
			mode = "Insert";
			Response.Redirect("~/Configurator/O365ServerProperties.aspx?Mode=" + mode,false);
			Context.ApplicationInstance.CompleteRequest();
        }
       
    }
}