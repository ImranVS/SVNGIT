using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using VSWebBL;
using DevExpress.Web;
using System.Collections;



namespace VSWebUI.Configurator
{
    public partial class NetworkLatencyServers : System.Web.UI.Page
    {

        DataTable nlDataTable = null;
        static int netid = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
          
            if (!IsPostBack)
            {

                FillnetworknlGrid();
              
                    if (Session["UserPreferences"] != null)
                    {
                        DataTable UserPreferences = (DataTable)Session["UserPreferences"];
                        foreach (DataRow dr in UserPreferences.Rows)
                        {
                            if (dr[1].ToString() == "NetworkLatencyServers|nlGridView1")
                            {
                                nlGridView1.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                            }
                        }
                    }
            }
            else {

                FillnwlatencyfromSession();
            
            }
            //1/21/2014 NS added for
            if (Session["nwlatencyUpdateStatus"] != null)
            {
                if (Session["nwlatencyUpdateStatus"].ToString() != "")
                {
                    //10/3/2014 NS modified for VSPLUS-990
                    successDiv.InnerHtml = "Network Latency information for <b>" + Session["nwlatencyUpdateStatus"].ToString() +
                        "</b> updated successfully."+
                        "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                    successDiv.Style.Value = "display: block";
                    Session["nwlatencyUpdateStatus"] = "";
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
        private void FillnetworknlGrid()
        {
            try
            {

                DataTable DSTaskSettingsDataTable = new DataTable();


                DSTaskSettingsDataTable = VSWebBL.ConfiguratorBL.NetworkLatencyBL.Ins.Getvalue();
               
                   
                    Session["networklatencytest"] = DSTaskSettingsDataTable;
                    nlGridView1.DataSource = DSTaskSettingsDataTable;
                    nlGridView1.DataBind();
                
            }
            catch (Exception ex)
            {
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex; 
            }
            finally { }
        }

    

        private void FillnwlatencyfromSession()
        {
            try
            {

                DataTable DSTaskSettingsDataTable = new DataTable();
                if (Session["networklatencytest"] != null && Session["networklatencytest"] != "")
                    DSTaskSettingsDataTable = (DataTable)Session["networklatencytest"];//VSWebBL.ConfiguratorBL.DominoPropertiesBL.Ins.GetAllData();
                if (DSTaskSettingsDataTable.Rows.Count > 0)
                {
                    nlGridView1.DataSource = DSTaskSettingsDataTable;
                    nlGridView1.DataBind();
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
        protected DataRow GetRow(DataTable ServerObject, IDictionaryEnumerator enumerator, int Keys)
        {
            //4/24/2013 NS added a fix for cases when there are no entries in the Locations table yet
            DataTable dataTable = ServerObject;
            DataRow DRRow = dataTable.NewRow();
            if (Keys == 0)
                DRRow = dataTable.NewRow();
            else
                DRRow = dataTable.Rows.Find(Keys);
            //IDictionaryEnumerator enumerator = e.NewValues.GetEnumerator();
            enumerator.Reset();
            while (enumerator.MoveNext())
                DRRow[enumerator.Key.ToString()] = (enumerator.Value == null ? "False" : enumerator.Value);
            return DRRow;
        }


        protected void nlGridView1_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            NetworkLatency LocObject = new NetworkLatency();
            LocObject.NetworkLatencyId = Convert.ToInt32(e.Keys[0]);

            //Delete row from DB
            Object ReturnValue = VSWebBL.ConfiguratorBL.NetworkLatencyBL.Ins.DeleteData(LocObject.NetworkLatencyId.ToString());

            //Update Grid after inserting new row, refresh grid as in page load
            ASPxGridView gridView = (ASPxGridView)sender;
            gridView.CancelEdit();
            e.Cancel = true;
            FillnetworknlGrid();
        }




        protected void nlGridView1_HtmlRowCreated(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
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

                //Mukund: VSPLUS-844, Page redirect on callback
                try
                {
                    if (e.GetValue("NetworkLatencyId") != " " && e.GetValue("NetworkLatencyId") != null)
                    {
                        ASPxWebControl.RedirectOnCallback("NetworkLatencyTestServers.aspx?Key=" + e.GetValue("NetworkLatencyId"));
                        Context.ApplicationInstance.CompleteRequest();//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
                    }
                    else
                    {
                        ASPxWebControl.RedirectOnCallback("NetworkLatencyTestServers.aspx");
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
        //protected void btn_Click(object sender, EventArgs e)
        //{
        //    ImageButton bttnDel = (ImageButton)sender;
        //    NetworkLatency locObj = new NetworkLatency();
        //    locObj.NetworkLatencyId = Convert.ToInt32(bttnDel.CommandArgument);
        //    netid = Convert.ToInt32(bttnDel.CommandArgument);
        //    string locName = bttnDel.AlternateText;
        //    pnlAreaDtls.Style.Add("visibility", "visible");
        //    //5/14/2014 NS added for VSPLUS-627
        //    DataTable dt = VSWebBL.ConfiguratorBL.NetworkLatencyBL.Ins.getname(locName);
        //    if (dt.Rows.Count == 0)
        //    {
        //        divmsg.InnerHtml = "Are you sure you want to delete the testserver " + locName + "?";
        //        bttnOK.Visible = true;
        //    }
        //    else
        //    {
        //        bttnOK.Visible = false;
        //        divmsg.InnerHtml = "You may not delete the testserver " + locName + " because there are servers assigned to it. " +
        //            "Please re-assign servers to other locations or delete all servers for this testserver before deleting " + locName + ".";
        //    }
        //}



        protected void ValidationUpdatedButton_Click(object sender, EventArgs e)
        {
            try
            {


                Response.Redirect("NetworkLatencyServers.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
                Context.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
            finally { }
        }
        protected void bttnOK_Click(object sender, EventArgs e)
        {
            NetworkLatency locObj = new NetworkLatency();
            locObj.NetworkLatencyId = netid;
            Object returnValue = VSWebBL.ConfiguratorBL.NetworkLatencyBL.Ins.DeleteData(locObj.NetworkLatencyId.ToString());
            
           
        }
        protected void bttnCancel_Click(object sender, EventArgs e)
        {
            FillnetworknlGrid();
        }


        protected void nlGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (nlGridView1.Selection.Count > 0)
            {
                System.Collections.Generic.List<object> Type = nlGridView1.GetSelectedFieldValues("NetworkLatencyId");
               
                if (Type.Count > 0)
                {
                    string NetworkLatencyId = Type[0].ToString();                   
                   
                    //Mukund: VSPLUS-844, Page redirect on callback
                    try
                    {
                        DevExpress.Web.ASPxWebControl.RedirectOnCallback("NetworkLatencyTestServers.aspx?Key=" + NetworkLatencyId + "");
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

        protected void nlGridView1_PageSizeChanged(object sender, EventArgs e)
        {
            //ProfilesGridView.PageIndex;
            VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("NetworkLatencyServers|nlGridView1", nlGridView1.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        }

        protected void NewButton_Click(object sender, EventArgs e)
        {
            //3/2/2015 NS added for VSPLUS-1432
            Response.Redirect("~/Configurator/NetworkLatencyTestServers.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
            Context.ApplicationInstance.CompleteRequest();
        }

        //protected void DominoServerGridView_CustomColumnSort(object sender, CustomColumnSortEventArgs e)
        //{
        //    e.Handled = true;

        //}
    }

}