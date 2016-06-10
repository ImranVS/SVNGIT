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
    public partial class LotusDominoServers : System.Web.UI.Page
    {
		int Userid;
        protected void Page_Load(object sender, EventArgs e)
        {
          
            if (!IsPostBack)
            {

                FillDominoServerGrid();
				UI.Ins.GetUserPreferenceSession("LotusDominoServers|DominoServerGridView", DominoServerGridView);
					//if (Session["UserPreferences"] != null)
					//{
					//    DataTable UserPreferences = (DataTable)Session["UserPreferences"];
					//    //foreach (DataRow dr in UserPreferences.Rows)
					//    DataRow[] UserPreferencesRow = UserPreferences.Select("PreferenceName =  'LotusDominoServers|DominoServerGridView' ");
					//    //{
					//        //if (dr[1].ToString() == "LotusDominoServers|DominoServerGridView")
					//        //{
					//        //    DominoServerGridView.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
					//        //}
					//    if (UserPreferencesRow.Length > 0)
					//    {
					//        DominoServerGridView.SettingsPager.PageSize = (UserPreferencesRow[0]).ItemArray[2];
					//    }
					//    //}
					//}
            }
            else {

                FillDominoServerGridfromSession();
            
            }
            //1/21/2014 NS added for
            if (Session["DominoUpdateStatus"] != null)
            {
                if (Session["DominoUpdateStatus"].ToString() != "")
                {
                    //10/3/2014 NS modified for VSPLUS-990
                    successDiv.InnerHtml = "Domino information for <b>" + Session["DominoUpdateStatus"].ToString() +
                        "</b> updated successfully."+
                        "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                    successDiv.Style.Value = "display: block";
                    Session["DominoUpdateStatus"] = "";
                }
            }
        }

        //protected void Page_Init(object sender, EventArgs e)
        //{
            
        //    //FillDominoServerGrid();
        //}
        /// <summary>
        /// Fill grid DominoServerGridView
        /// </summary>
        /// <param name="ServerKey"></param>
        private void FillDominoServerGrid()
        {
            try
            {

                DataTable DSTaskSettingsDataTable = new DataTable();
				if (Session["UserID"] != null)
				{
					Userid = Convert.ToInt32(Session["UserID"]);
				}
				//DSTaskSettingsDataTable = VSWebBL.ConfiguratorBL.DominoPropertiesBL.Ins.GetAllData();
				DSTaskSettingsDataTable = VSWebBL.ConfiguratorBL.DominoPropertiesBL.Ins.GetAllDataforuserrestrictions(Userid);
				
                if (DSTaskSettingsDataTable.Rows.Count > 0)
                {
                    if (Session["RestrictedServers"] != "" && Session["RestrictedServers"] != null)
                    {
                        List<int> ServerID = new List<int>();
                        List<int> LocationID = new List<int>();
                        DataTable resServers = (DataTable)Session["RestrictedServers"];
                        foreach (DataRow resser in resServers.Rows)
                        {
                            foreach (DataRow dominorow in DSTaskSettingsDataTable.Rows)
                            {

                                if (resser["serverid"].ToString() == dominorow["ID"].ToString())
                                {
                                    ServerID.Add(DSTaskSettingsDataTable.Rows.IndexOf(dominorow));
                                }
                                if (resser["locationID"].ToString() == dominorow["locationid"].ToString())
                                {
                                    LocationID.Add(Convert.ToInt32(dominorow["locationid"].ToString()));
                                    //LocationID.Add(DSTaskSettingsDataTable.Rows.IndexOf(dominorow));
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
                    Session["DominoServer"] = DSTaskSettingsDataTable;
                    DominoServerGridView.DataSource = DSTaskSettingsDataTable;
                    DominoServerGridView.DataBind();
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
        private void FillDominoServerGridfromSession()
        {
            try
            {

                DataTable DSTaskSettingsDataTable = new DataTable();
                if(Session["DominoServer"]!=null && Session["DominoServer"]!="")
                DSTaskSettingsDataTable = (DataTable)Session["DominoServer"];//VSWebBL.ConfiguratorBL.DominoPropertiesBL.Ins.GetAllData();
                if (DSTaskSettingsDataTable.Rows.Count > 0)
                {
                    DominoServerGridView.DataSource = DSTaskSettingsDataTable;
                    DominoServerGridView.DataBind();
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


        protected void DominoServerGridView_HtmlRowCreated(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
        {
            //if (e.RowType == GridViewRowType.EditForm)
            //{
            //    ASPxWebControl.RedirectOnCallback("DominoProperties.aspx?Key=" + e.GetValue("ID"));
            //}
            e.Row.Attributes.Add("onmouseover", "this.originalcolor=this.style.backgroundColor;" + "this.style.backgroundColor='#C0C0C0';");

            e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalcolor;");
            //1/7/2014 NS added
            if (e.RowType == GridViewRowType.EditForm)
            {
                if (e.GetValue("ID").ToString() != " ")
                {
					Session["Key"] = e.GetValue("ID").ToString();
                    //Mukund: VSPLUS-844, Page redirect on callback
                    try
                    {
						ASPxWebControl.RedirectOnCallback("DominoProperties.aspx?Key=" + Session["Key"]);
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
        protected void ImportDominoServers_Click(object sender, EventArgs e)
        {
            DevExpress.Web.ASPxMenu SubMenu = (DevExpress.Web.ASPxMenu)this.Master.FindControl("SubMenu");
            SubMenu.Items.Clear();
            Session["MenuID"] = null;
            Response.Redirect("~/Security/ImportServers.aspx", false);//Mukund,01Oct14
            Context.ApplicationInstance.CompleteRequest();
        }

        protected void DominoServerGridView_SelectionChanged(object sender, EventArgs e)
        {
            if (DominoServerGridView.Selection.Count > 0)
            {
                System.Collections.Generic.List<object> Type = DominoServerGridView.GetSelectedFieldValues("ID");
               
                if (Type.Count > 0)
                {
                    string ID = Type[0].ToString();
					Session["Key"] = ID;
                    //Mukund: VSPLUS-844, Page redirect on callback
                    try
                    {
						DevExpress.Web.ASPxWebControl.RedirectOnCallback("DominoProperties.aspx?Key=" + Session["Key"]);
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

        protected void DominoServerGridView_PageSizeChanged(object sender, EventArgs e)
        {
            //ProfilesGridView.PageIndex;
			//DataTable table = new DataTable()
		  VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("LotusDominoServers|DominoServerGridView", DominoServerGridView.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
		//	Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
		 UI.Ins.ChangeUserPreference("LotusDominoServers|DominoServerGridView", DominoServerGridView.SettingsPager.PageSize);


			

	                                                   
			//VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("LotusDominoServers|DominoServerGridView", DominoServerGridView.SettingsPager.PageSize.ToString(), Session);
			
        }

	
        //protected void DominoServerGridView_CustomColumnSort(object sender, CustomColumnSortEventArgs e)
        //{
        //    e.Handled = true;

        //}
    }

}