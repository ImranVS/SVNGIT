using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using VSWebBL;
using DevExpress.Web;

namespace VSWebUI
{
    public partial class WebForm14 : System.Web.UI.Page
    {
		int UserID;
        protected void Page_Load(object sender, EventArgs e)
        {
            //Page.Title = "BlackBerry Details";
            if (!IsPostBack)
            {
              
                fillgrid();
                if (Session["UserPreferences"] != null)
                {
                    DataTable UserPreferences = (DataTable)Session["UserPreferences"];
                    foreach (DataRow dr in UserPreferences.Rows)
                    {
                        if (dr[1].ToString() == "BlackBerry|BlackBerryGridView")
                        {
                            BlackBerryGridView.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                        }
                    }
                }
            }
            else
            {
                fillgridbySession();
            }
            //3/19/2014 NS added
            if (Session["BlackberryUpdateStatus"] != null)
            {
                if (Session["BlackberryUpdateStatus"].ToString() != "")
                {
                    //10/3/2014 NS modified for VSPLUS-990
                    successDiv.InnerHtml = "BlackBerry information for <b>" + Session["BlackberryUpdateStatus"].ToString() +
                        "</b> updated successfully."+
                        "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                    successDiv.Style.Value = "display: block";
                    Session["BlackberryUpdateStatus"] = "";
                }
            }
        }


        public void fillgrid()
        {
           // DataTable dt = VSWebBL.ConfiguratorBL.BlackBerryServersBL.Ins.fillgrid();
            try
            {
				if (Session["UserID"] != null)
				{
					UserID = Convert.ToInt32(Session["UserID"]);
				
				}
				DataTable dt = VSWebBL.ConfiguratorBL.BlackBerryServersBL.Ins.getfillgridbyUser(UserID);
                if (dt.Rows.Count > 0)
                {
                    if (Session["RestrictedServers"] != "" && Session["RestrictedServers"] != null)
                    {
                        List<int> ServerID = new List<int>();
                        List<int> LocationID = new List<int>();
                        DataTable resServers = (DataTable)Session["RestrictedServers"];
                        foreach (DataRow dominorow in dt.Rows)
                        {
                            foreach (DataRow resser in resServers.Rows)
                            {
                                if (resser["serverid"].ToString() == dominorow["SID"].ToString())
                                {
                                    ServerID.Add(dt.Rows.IndexOf(dominorow));
                                }
                                if (resser["locationID"].ToString() == dominorow["LocationID"].ToString())
                                {
                                    LocationID.Add(Convert.ToInt32(dominorow["LocationID"].ToString()));
                                    //LocationID.Add(dt.Rows.IndexOf(dominorow));
                                }
                            }

                        }
                        foreach (int Id in ServerID)
                        {
                            dt.Rows[Id].Delete();
                        }
                        dt.AcceptChanges();
                        //foreach (int Lid in LocationID)
                        //{
                        //    dt.Rows[Lid].Delete();
                        //}
                        foreach (int lid in LocationID)
                        {
                            DataRow[] row = dt.Select("LocationID=" + lid + "");
                            for (int i = 0; i < row.Count(); i++)
                            {
                                dt.Rows.Remove(row[i]);
                                dt.AcceptChanges();
                            }
                        }
                        dt.AcceptChanges();

                    }
                    Session["BlackBerryServers"] = dt;
                    BlackBerryGridView.DataSource = dt;
                    BlackBerryGridView.DataBind();
                }
            }
            catch (Exception ex)
            {
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
        }

        public void fillgridbySession()
        {
            DataTable d = new DataTable();
            try
            {
                if ((Session["BlackBerryServers"] != null) && (Session["BlackBerryServers"] != ""))
                {
                    d = (DataTable)Session["BlackBerryServers"];
                    if (d.Rows.Count > 0)
                    {
                        BlackBerryGridView.DataSource = d;
                        BlackBerryGridView.DataBind();
                    }
                }

            }
            catch (Exception ex)
            {
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
        }

        protected void BlackBerryGridView_HtmlRowCreated(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
        {
            e.Row.Attributes.Add("onmouseover", "this.originalcolor=this.style.backgroundColor;" + "this.style.backgroundColor='#C0C0C0';");

            e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalcolor;");

          
            if(e.RowType==GridViewRowType.EditForm)
            {
                //Mukund: VSPLUS-844, Page redirect on callback
                try
                {
                    if (e.GetValue("SID").ToString() != "")
                    {
                        ASPxWebControl.RedirectOnCallback("BlackBerryEntertpriseServer.aspx?Key=" + e.GetValue("SID"));
                        Context.ApplicationInstance.CompleteRequest();//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback

                    }
                    else
                    {
                        ASPxWebControl.RedirectOnCallback("BlackBerryEntertpriseServer.aspx");
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

        protected void BlackBerryGridView_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            BlackBerryServers BlackBerryServerObject = new BlackBerryServers();
            BlackBerryServerObject.key = Convert.ToInt32(e.Keys[0]);
            Object Blackberry = VSWebBL.ConfiguratorBL.BlackBerryServersBL.Ins.deleteBlackBerryServer(BlackBerryServerObject);
            ASPxGridView griedview = (ASPxGridView)sender;
            griedview.CancelEdit();
            e.Cancel = true;
            fillgrid();
        }

        protected void BlackBerryGridView_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            e.Cell.ToolTip = string.Format("{0}", e.CellValue);
        }

        protected void BlackBerryGridView_SelectionChanged(object sender, EventArgs e)
        {
            if (BlackBerryGridView.Selection.Count > 0)
            {
                System.Collections.Generic.List<object> Type = BlackBerryGridView.GetSelectedFieldValues("SID");

                if (Type.Count > 0)
                {
                    string ID = Type[0].ToString();

                    //Mukund: VSPLUS-844, Page redirect on callback
                    try
                    {
                        DevExpress.Web.ASPxWebControl.RedirectOnCallback("BlackBerryEntertpriseServer.aspx?Key=" + ID + "");
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

        protected void BlackBerryGridView_PageSizeChanged(object sender, EventArgs e)
        {
            //ProfilesGridView.PageIndex;
            VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("BlackBerry|BlackBerryGridView", BlackBerryGridView.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));

        }

    }
}
