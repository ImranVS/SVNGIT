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
	public partial class WebSphereGrid : System.Web.UI.Page
    {
		string UserName;
		int UserID;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                Session["Submenu"] = "";
                FillWindowsServerGrid();
                if (Session["UserPreferences"] != null)
                {
                    DataTable UserPreferences = (DataTable)Session["UserPreferences"];
                    foreach (DataRow dr in UserPreferences.Rows)
                    {
						if (dr[1].ToString() == "WebsphereGridView|WebsphereGridView")
                        {
							WebsphereGridView.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                        }
                    }
                }
            }
            else
            {

                FillWebSphereserverGridfromSession();

            }
            //3/27/2014 MD added for
            if (Session["WebSphereUpdateStatus"] != null)
            {
				if (Session["WebSphereUpdateStatus"].ToString() != "")
                {
                    //10/21/2014 NS modified for VSPLUS-990
					successDiv.InnerHtml = "WebSphere Server information for <b>" + Session["WebSphereUpdateStatus"].ToString() +
                        "</b> updated successfully."+
                        "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                    successDiv.Style.Value = "display: block";
					Session["WebSphereUpdateStatus"] = "";
                }
            }
        }


        private void FillWindowsServerGrid()
        {
            try
            {

                DataTable DSTaskSettingsDataTable = new DataTable();
				//Session["CellName"]=DSTaskSettingsDataTable.Rows[0]["cellName"].ToString();
				//Session["NodeName"] = DSTaskSettingsDataTable.Rows[0]["NodeName"].ToString();
				//DSTaskSettingsDataTable = VSWebBL.ConfiguratorBL.WebSphereServerGridBAL.Ins.GetAllData();
				if (Session["UserID"] != null)
				{
					UserID = Convert.ToInt32(Session["UserID"]);
					DSTaskSettingsDataTable = VSWebBL.ConfiguratorBL.WebSphereServerGridBAL.Ins.GetAllDatabyUserrestrictions(UserID);
					
				}
		
                if (DSTaskSettingsDataTable.Rows.Count > 0)
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
                    Session["WebSphereServer"] = DSTaskSettingsDataTable;
					WebsphereGridView.DataSource = DSTaskSettingsDataTable;
					WebsphereGridView.DataBind();
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
		private void FillWebSphereserverGridfromSession()
        {
            try
            {

                DataTable DSTaskSettingsDataTable = new DataTable();
				if (Session["WebSphereServer"] != null && Session["WebSphereServer"] != "")
					DSTaskSettingsDataTable = (DataTable)Session["WebSphereServer"];//VSWebBL.ConfiguratorBL.WindowsPropertiesBL.Ins.GetAllData();
                if (DSTaskSettingsDataTable.Rows.Count > 0)
                {
					WebsphereGridView.DataSource = DSTaskSettingsDataTable;
					WebsphereGridView.DataBind();
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


		protected void WebSphereGridView_HtmlRowCreated(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
        {
            e.Row.Attributes.Add("onmouseover", "this.originalcolor=this.style.backgroundColor;" + "this.style.backgroundColor='#C0C0C0';");

            e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalcolor;");

            if (e.RowType == GridViewRowType.EditForm)
            {
				//if (e.GetValue("ID").ToString() != " ")
				//{
                    //"DominoProperties.aspx?Key=" + e.GetValue("ID")
                    //Mukund: VSPLUS-844, Page redirect on callback
                    try
                    {
						if (e.GetValue("ID") != " " && e.GetValue("ID") != null)
						{
                            ASPxWebControl.RedirectOnCallback("WebSphereProperties.aspx?ID=" + e.GetValue("ID") + "&name=" + e.GetValue("ServerName") + "&Cat=" + e.GetValue("ServerType") + "&Loc=" + e.GetValue("Location") + "&ipaddr=" + e.GetValue("ipaddress") + "&CellName=" + e.GetValue("CellName") + "&NodeName=" + e.GetValue("NodeName"));
							Context.ApplicationInstance.CompleteRequest();//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
						}
						else 
						{
							ASPxWebControl.RedirectOnCallback("WebSphereProperties.aspx");
							Context.ApplicationInstance.CompleteRequest();
						}
                    }
                    catch (Exception ex)
                    {
                        Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                        //throw ex;
                    }

				//}
            }
        }

		protected void WebSphereGridView_SelectionChanged(object sender, EventArgs e)
        {
			if (WebsphereGridView.Selection.Count > 0)
            {
				System.Collections.Generic.List<object> Type = WebsphereGridView.GetSelectedFieldValues("ID");
				System.Collections.Generic.List<object> Name = WebsphereGridView.GetSelectedFieldValues("ServerName");
				System.Collections.Generic.List<object> Cat = WebsphereGridView.GetSelectedFieldValues("ServerType");
				System.Collections.Generic.List<object> Location = WebsphereGridView.GetSelectedFieldValues("Location");
				System.Collections.Generic.List<object> ipaddress = WebsphereGridView.GetSelectedFieldValues("ipaddress");
                if (Type.Count > 0)
                {
                    string ID = Type[0].ToString();
                    string name = Name[0].ToString();
                    string category = Cat[0].ToString();
                    string Loc = Location[0].ToString();
                    string ipaddr = ipaddress[0].ToString();

                    //Mukund: VSPLUS-844, Page redirect on callback
                    try
                    {
						DevExpress.Web.ASPxWebControl.RedirectOnCallback("WebSphereProperties.aspx?ID=" + ID + "&name=" + name + "&Cat=" + category + "&Loc=" + Loc + "&ipaddr=" + ipaddr);
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

		protected void WebsphereGridView_PageSizeChanged(object sender, EventArgs e)
        {
            //ProfilesGridView.PageIndex;
			VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("WebsphereGridView|WebsphereGridView", WebsphereGridView.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        }

        protected void NewButton_Click(object sender, EventArgs e)
        {
            //3/2/2015 NS added for VSPLUS-1432
            Response.Redirect("~/Configurator/WebSphereProperties.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
            Context.ApplicationInstance.CompleteRequest();
        }
    }
}
