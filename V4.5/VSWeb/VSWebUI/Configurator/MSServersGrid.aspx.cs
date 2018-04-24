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
    public partial class MSServersGrid : System.Web.UI.Page
    {
		int UserID;
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
            else {

                FillExchangeServerGridfromSession();
            
            }
            //3/27/2014 MD added for
            if (Session["ExchangeUpdateStatus"] != null)
            {
                if (Session["ExchangeUpdateStatus"].ToString() != "")
                {
                    successDiv.InnerHtml = "Exchange information for <b>" + Session["ExchangeUpdateStatus"].ToString() +
                        "</b> updated successfully." +
                        "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                    successDiv.Style.Value = "display: block";
                    Session["ExchangeUpdateStatus"] = "";
                }
            }
        }

      
        private void FillExchangeServerGrid()
        {
            try
            {

                DataTable DSTaskSettingsDataTable = new DataTable();
				if (Session["UserID"] != null)
				{
					UserID = Convert.ToInt32(Session["UserID"]);
					DSTaskSettingsDataTable = VSWebBL.ExchangeBAL.Ins.GetAllDatabyUser(UserID); ;
				}
              //  DSTaskSettingsDataTable = VSWebBL.ExchangeBAL.Ins.GetAllData();
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
                if(Session["ExchangeServer"]!=null && Session["ExchangeServer"]!="")
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
                        ASPxWebControl.RedirectOnCallback("ExchangeServer.aspx?ID=" + e.GetValue("ID") + "&name=" + e.GetValue("ServerName") + "&Cat=" + e.GetValue("ServerType") + "&Loc=" + e.GetValue("Location") + "&ipaddr=" + e.GetValue("ipaddress"));
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

        protected void ExchangeServerGridView_SelectionChanged(object sender, EventArgs e)
        {
            if (MSServerGridView.Selection.Count > 0)
            {
                System.Collections.Generic.List<object> Type = MSServerGridView.GetSelectedFieldValues("ID");
                System.Collections.Generic.List<object> Name = MSServerGridView.GetSelectedFieldValues("ServerName");
                System.Collections.Generic.List<object> Cat = MSServerGridView.GetSelectedFieldValues("ServerType");
                System.Collections.Generic.List<object> Location = MSServerGridView.GetSelectedFieldValues("Location");
                System.Collections.Generic.List<object> ipaddress = MSServerGridView.GetSelectedFieldValues("ipaddress");
                if (Type.Count > 0)
                {
                    string ID = Type[0].ToString();
                    string name = Name[0].ToString();
                    string category=Cat[0].ToString();
                    string Loc = Location[0].ToString();
                    string ipaddr = ipaddress[0].ToString();
                  
                    //Mukund: VSPLUS-844, Page redirect on callback
                    try
                    {
                        DevExpress.Web.ASPxWebControl.RedirectOnCallback("ExchangeServer.aspx?ID=" + ID + "&name=" + name + "&Cat=" + category + "&Loc=" + Loc + "&ipaddr=" + ipaddr);
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

        protected void MSServerGridView_PageSizeChanged(object sender, EventArgs e)
        {
            //ProfilesGridView.PageIndex;
            VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("MSServersGrid|MSServerGridView", MSServerGridView.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        }
    }

}