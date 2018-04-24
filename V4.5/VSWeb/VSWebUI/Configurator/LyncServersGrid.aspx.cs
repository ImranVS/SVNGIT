using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DevExpress.Web;

namespace VSWebUI.Configurator
{
    public partial class LyncServersGrid : System.Web.UI.Page
    {
		int UserID;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["Submenu"] = "";
                FillLyncServerGrid();
                if (Session["UserPreferences"] != null)
                {
                    DataTable UserPreferences = (DataTable)Session["UserPreferences"];
                    foreach (DataRow dr in UserPreferences.Rows)
                    {
                        if (dr[1].ToString() == "MSLyncGrid|MSLyncGridView")
                        {
                            MSLyncGridView.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                        }
                    }
                }
            }
            else
            {
                FillLyncServerGridfromSession();
            }
            if (Session["LyncUpdateStatus"] != null)
            {
                if (Session["LyncUpdateStatus"].ToString() != "")
                {
                    //10/3/2014 NS modified for VSPLUS-990
					successDiv.InnerHtml = "Skype for Business information for <b>" + Session["LyncUpdateStatus"].ToString() +
                        "</b> updated successfully."+
                        "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                    successDiv.Style.Value = "display: block";
                    Session["LyncUpdateStatus"] = "";
                }
            }
        }

        private void FillLyncServerGrid()
        {
            try
            {
                DataTable dt = new DataTable();
				//DataTable DSTaskSettingsDataTable = new DataTable();
				if (Session["UserID"] != null)
				{
					UserID = Convert.ToInt32(Session["UserID"]);

				}
                //dt = VSWebBL.ConfiguratorBL.LyncBL.Ins.GetAllData();
				dt = VSWebBL.ConfiguratorBL.LyncBL.Ins.GetAllDataByUser(UserID);
                if (dt.Rows.Count > 0)
                {
                    if (Session["RestrictedServers"] != "" && Session["RestrictedServers"] != null)
                    {
                        List<int> ServerID = new List<int>();
                        List<int> LocationID = new List<int>();
                        DataTable resServers = (DataTable)Session["RestrictedServers"];
                        foreach (DataRow resser in resServers.Rows)
                        {
                            foreach (DataRow Exchangerow in dt.Rows)
                            {
                                if (resser["serverid"].ToString() == Exchangerow["ID"].ToString())
                                {
                                    ServerID.Add(dt.Rows.IndexOf(Exchangerow));
                                }
                                if (resser["locationID"].ToString() == Exchangerow["locationid"].ToString())
                                {
                                    LocationID.Add(Convert.ToInt32(Exchangerow["locationid"].ToString()));
                                    //LocationID.Add(DSTaskSettingsDataTable.Rows.IndexOf(Exchangerow));
                                }
                            }

                        }
                        foreach (int Id in ServerID)
                        {
                            dt.Rows[Id].Delete();
                        }
                        dt.AcceptChanges();

                        foreach (int lid in LocationID)
                        {
                            DataRow[] row = dt.Select("locationid=" + lid + "");
                            for (int i = 0; i < row.Count(); i++)
                            {
                                dt.Rows.Remove(row[i]);
                                dt.AcceptChanges();
                            }
                        }
                        dt.AcceptChanges();
                    }
                    Session["LyncServer"] = dt;
                    MSLyncGridView.DataSource = dt;
                    MSLyncGridView.DataBind();
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

        private void FillLyncServerGridfromSession()
        {
            try
            {
                DataTable dt = new DataTable();
                if (Session["LyncServer"] != null && Session["LyncServer"] != "")
                    dt = (DataTable)Session["LyncServer"];//VSWebBL.ConfiguratorBL.ExchangePropertiesBL.Ins.GetAllData();
                if (dt.Rows.Count > 0)
                {
                    MSLyncGridView.DataSource = dt;
                    MSLyncGridView.DataBind();
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

        protected void MSLyncGridView_PageSizeChanged(object sender, EventArgs e)
        {
            VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("MSLyncGrid|MSLyncGridView", MSLyncGridView.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        }

        protected void MSLyncGridView_HtmlRowCreated(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
        {
            e.Row.Attributes.Add("onmouseover", "this.originalcolor=this.style.backgroundColor;" + "this.style.backgroundColor='#C0C0C0';");
            e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalcolor;");
            if (e.RowType == GridViewRowType.EditForm)
            {
                if (e.GetValue("ID").ToString() != " ")
                {
                    try
                    {
                        ASPxWebControl.RedirectOnCallback("LyncServer.aspx?ID=" + e.GetValue("ID") + "&Name=" + e.GetValue("ServerName") + "&Cat=" + e.GetValue("ServerType") + "&Loc=" + e.GetValue("Location"));
                        Context.ApplicationInstance.CompleteRequest();//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
                    }
                    catch (Exception ex)
                    {
                        Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                    }
                }
            }
        }
    }
}