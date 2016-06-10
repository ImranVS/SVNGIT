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
    public partial class DAGserverGrid : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                Session["Submenu"] = "";
                FillExchangeServerGrid();
                if (Session["UserPreferences"] != null)
                {
                    DataTable UserPreferences = (DataTable)Session["UserPreferences"];
                    foreach (DataRow dr in UserPreferences.Rows)
                    {
                        if (dr[1].ToString() == "MSServersGrid|MSServerGridView")
                        {
                            MSServerGridView.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                        }
                    }
                }
            }
            else
            {

                FillExchangeServerGridfromSession();

            }
            //3/27/2014 MD added for
            if (Session["DAGUpdateStatus"] != null)
            {
				if (Session["DAGUpdateStatus"].ToString() != "")
                {
					successDiv.InnerHtml = "DAG information for <b>" + Session["DAGUpdateStatus"].ToString() +
                        "</b> updated successfully.";
                    successDiv.Style.Value = "display: block";
					Session["DAGUpdateStatus"] = "";
                }
            }
        }
        private void FillExchangeServerGrid()
        {
            try
            {

                DataTable DSTaskSettingsDataTable = new DataTable();

                DSTaskSettingsDataTable = VSWebBL.ConfiguratorBL.DAGBL.Ins.GetAllData();
                if (DSTaskSettingsDataTable.Rows.Count > 0)
                {
                    if (Session["RestrictedServers"] != "" && Session["RestrictedServers"] != null)
                    {
                        List<int> ServerID = new List<int>();
                        List<int> LocationID = new List<int>();
                        DataTable resServers = (DataTable)Session["RestrictedServers"];
                        foreach (DataRow resser in resServers.Rows)
                        {
                            foreach (DataRow Exchangerow in DSTaskSettingsDataTable.Rows)
                            {

                                if (resser["serverid"].ToString() == Exchangerow["ID"].ToString())
                                {
                                    ServerID.Add(DSTaskSettingsDataTable.Rows.IndexOf(Exchangerow));
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
                    Session["ExchangeServer"] = DSTaskSettingsDataTable;
                    MSServerGridView.DataSource = DSTaskSettingsDataTable;
                    MSServerGridView.DataBind();
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
        private void FillExchangeServerGridfromSession()
        {
            try
            {

                DataTable DSTaskSettingsDataTable = new DataTable();
                if (Session["ExchangeServer"] != null && Session["ExchangeServer"] != "")
                    DSTaskSettingsDataTable = (DataTable)Session["ExchangeServer"];//VSWebBL.ConfiguratorBL.ExchangePropertiesBL.Ins.GetAllData();
                if (DSTaskSettingsDataTable.Rows.Count > 0)
                {
                    MSServerGridView.DataSource = DSTaskSettingsDataTable;
                    MSServerGridView.DataBind();
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
        protected void ExchangeServerGridView_HtmlRowCreated(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
        {
            e.Row.Attributes.Add("onmouseover", "this.originalcolor=this.style.backgroundColor;" + "this.style.backgroundColor='#C0C0C0';");

            e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalcolor;");

            if (e.RowType == GridViewRowType.EditForm)
            {
                if (e.GetValue("ID").ToString() != " ")
                {
                    //"DominoProperties.aspx?Key=" + e.GetValue("ID")
                    //Mukund: VSPLUS-844, Page redirect on callback
                    try
                    {
                        ASPxWebControl.RedirectOnCallback("MicrosoftExchangeDAG.aspx?ID=" + e.GetValue("ID") + "&name=" + e.GetValue("ServerName") + "&Cat=" + e.GetValue("ServerType") + "&Loc=" + e.GetValue("Location") + "&ipaddr=" + e.GetValue("ipaddress"));
                        Context.ApplicationInstance.CompleteRequest();//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
                    }
                    catch (Exception ex)
                    {
                        Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                        //throw ex;
                    }

                }
            }
        }
    }
}