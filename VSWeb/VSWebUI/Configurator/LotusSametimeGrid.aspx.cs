using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using VSWebBL;
using VSWebDO;
using DevExpress.Web;


namespace VSWebUI.Configurator
{
    public partial class LotusSametimeGrid : System.Web.UI.Page
    {
		int UserID;
        protected void Page_Load(object sender, EventArgs e)
        {
          
            if (!IsPostBack)
            {
              
                filllotussametimegrid();
               
                if (Session["UserPreferences"] != null)
                {
                    DataTable UserPreferences = (DataTable)Session["UserPreferences"];
                    foreach (DataRow dr in UserPreferences.Rows)
                    {
                        if (dr[1].ToString() == "LotusSametimeGrid|LotusSametimeGridview")
                        {
                            LotusSametimeGridview.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                        }
                    }
                }
            }
            else
            {
                filllotussametimegridbysession();
            }
            //2/20/2014 NS added
            if (Session["SametimeUpdateStatus"] != null)
            {
                if (Session["SametimeUpdateStatus"].ToString() != "")
                {
                    //10/3/2014 NS modified for VSPLUS-990
                    successDiv.InnerHtml = "Sametime information for <b>" + Session["SametimeUpdateStatus"].ToString() +
                        "</b> updated successfully."+
                        "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                    successDiv.Style.Value = "display: block";
                    Session["SametimeUpdateStatus"] = "";
                }
            }

        }

        public void filllotussametimegrid()
        {
            try
            {
                DataTable sametime = new DataTable();
                LotusSametimeGrid objectlotussametime = new LotusSametimeGrid();
                //sametime = VSWebBL.ConfiguratorBL.SametimeServerBL.Ins.getdata();
				if (Session["UserID"] != null)
				{
					UserID = Convert.ToInt32(Session["UserID"]);
				}
				sametime = VSWebBL.ConfiguratorBL.SametimeServerBL.Ins.getdataforlotussametimegridbyuser(UserID);

                if (sametime.Rows.Count > 0)
                {
                    if (Session["RestrictedServers"] != "" && Session["RestrictedServers"] != null)
                    {
                        List<int> ServerID = new List<int>();
                        List<int> LocationID = new List<int>();
                        DataTable resServers = (DataTable)Session["RestrictedServers"];
                        foreach (DataRow dominorow in sametime.Rows)
                        {
                            foreach (DataRow resser in resServers.Rows)
                            {
                                if (resser["serverid"].ToString() == dominorow["SID"].ToString())
                                {
                                    ServerID.Add(sametime.Rows.IndexOf(dominorow));
                                }
                                if (resser["locationID"].ToString() == dominorow["LocationID"].ToString())
                                {
                                    LocationID.Add(Convert.ToInt32(dominorow["LocationID"].ToString()));
                                   // LocationID.Add(sametime.Rows.IndexOf(dominorow));
                                }
                            }

                        }
                        foreach (int Id in ServerID)
                        {
                            sametime.Rows[Id].Delete();
                        }
                        sametime.AcceptChanges();
                        //foreach (int Lid in LocationID)
                        //{
                        //    sametime.Rows[Lid].Delete();
                        //}
                        foreach (int lid in LocationID)
                        {
                            DataRow[] row = sametime.Select("LocationID=" + lid + "");
                            for (int i = 0; i < row.Count(); i++)
                            {
                                sametime.Rows.Remove(row[i]);
                                sametime.AcceptChanges();
                            }
                        }
                        sametime.AcceptChanges();

                    }
                    Session["sametime"] = sametime;
                    LotusSametimeGridview.DataSource = sametime;
                    LotusSametimeGridview.DataBind();
                }
            }
            catch (Exception ex)
            {
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
            finally
            {
            }
        }

        public void filllotussametimegridbysession()
        {
            try
            {
                DataTable sametimebysession = new DataTable();
                if(Session["sametime"]!=null&&Session["sametime"]!="")
                sametimebysession = (DataTable)Session["sametime"];
                if (sametimebysession.Rows.Count > 0)
                {
                    LotusSametimeGridview.DataSource = sametimebysession;
                    LotusSametimeGridview.DataBind();
                }
            }
            catch (Exception ex)
            {
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
        }


        protected void LotusSametimeGridview_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            SametimeServers SametimeObject = new SametimeServers();

            SametimeObject.ID = Convert.ToInt32(e.Keys[0]);
            //delete  a Row
            Object Stsobject = VSWebBL.ConfiguratorBL.SametimeServerBL.Ins.DeleteSametime(SametimeObject);
            ASPxGridView griedview = (ASPxGridView)sender;
            griedview.CancelEdit();
            e.Cancel = true;
            filllotussametimegrid();
        }


        protected void LotusSametimeGridview_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
        {
            //if (e.RowType == GridViewRowType.EditForm)
            //{
            //    if (e.GetValue("ID") != "")
            //    {

            //        ASPxWebControl.RedirectOnCallback("SametimeServer.aspx?ID=" + e.GetValue("ID"));
            //    }
            //    //else
            //    //{
            //    //    ASPxWebControl.RedirectOnCallback("SametimeServer.aspx");
            //    //}



            //}
            e.Row.Attributes.Add("onmouseover", "this.originalcolor=this.style.backgroundColor;" + "this.style.backgroundColor='#C0C0C0';");

            e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalcolor;");
            //1/7/2014 NS added
            if (e.RowType == GridViewRowType.EditForm)
            {
                if (e.GetValue("SID").ToString() != " ")
                {
                    //Mukund: VSPLUS-844, Page redirect on callback
                    try
                    {
                        ASPxWebControl.RedirectOnCallback("SametimeServer.aspx?ID=" + e.GetValue("SID"));
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

        protected void LotusSametimeGridview_SelectionChanged(object sender, EventArgs e)
        {
            if (LotusSametimeGridview.Selection.Count > 0)
            {
                System.Collections.Generic.List<object> Type = LotusSametimeGridview.GetSelectedFieldValues("SID");

                if (Type.Count > 0)
                {
                    string ID = Type[0].ToString();

                    //Mukund: VSPLUS-844, Page redirect on callback
                    try
                    {
                        DevExpress.Web.ASPxWebControl.RedirectOnCallback("SametimeServer.aspx?ID=" + ID + "");
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

        protected void LotusSametimeGridview_PageSizeChanged(object sender, EventArgs e)
        {
            //ProfilesGridView.PageIndex;
            VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("LotusSametimeGrid|LotusSametimeGridview", LotusSametimeGridview.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        }
    }
}