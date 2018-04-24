using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using VSWebDO;
using DevExpress.Web;
namespace VSWebUI.Configurator
{
    public partial class BlackBerryDeviceProbesgrid : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Page.Title = "BlackBerry Device Probe Details";
            if (!IsPostBack)
            {
                if (Session["UserPreferences"] != null)
                {
                    DataTable UserPreferences = (DataTable)Session["UserPreferences"];
                    foreach (DataRow dr in UserPreferences.Rows)
                    {
                        if (dr[1].ToString() == "BlackBerryDeviceProbesgrid|BlackBerryDeviceProbegrid")
                        {
                            BlackBerryDeviceProbegrid.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                        }
                        if (dr[1].ToString() == "BlackBerryDeviceProbesgrid|ASPxGridView2")
                        {
                            ASPxGridView2.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                        }
                    }
                }
                getdata();

            }
            else
            {
                fillgridbysession();
            }
            //3/19/2014 NS added
            if (Session["BlackberryDeviceUpdateStatus"] != null)
            {
                if (Session["BlackberryDeviceUpdateStatus"].ToString() != "")
                {
                    //10/3/2014 NS modified for VSPLUS-990
                    successDiv.InnerHtml = "BlackBerry device information for <b>" + Session["BlackberryDeviceUpdateStatus"].ToString() +
                        "</b> updated successfully."+
                        "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                    successDiv.Style.Value = "display: block";
                    Session["BlackberryDeviceUpdateStatus"] = "";
                }
            }
        }

        public void getdata()
        {
            DataTable dt = new DataTable();
            try
            {
                dt = VSWebBL.ConfiguratorBL.BlackBerryBL.Ins.gettabledetails(); ;
                if (dt.Rows.Count >= 0)
                {
                    Session["BlackBerryDevicePrbegrid"] = dt;
                    BlackBerryDeviceProbegrid.DataSource = dt;

                    BlackBerryDeviceProbegrid.DataBind();
                }
            }
            catch (Exception ex)
            {
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }

        }

        public void fillgridbysession()
        {
            if (Session["BlackBerryDevicePrbegrid"] != null && Session["BlackBerryDevicePrbegrid"] != "")
            {
                DataTable d = new DataTable();
                try
                {
                    d = (DataTable)Session["BlackBerryDevicePrbegrid"];
                    BlackBerryDeviceProbegrid.DataSource = d;
                    BlackBerryDeviceProbegrid.DataBind();
                }
                catch (Exception ex)
                {
                    //6/27/2014 NS added for VSPLUS-634
                    Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                    throw ex;
                }
            }
        }

        protected void BlackBerryDeviceProdegridGridView_HtmlRowCreated(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType == GridViewRowType.EditForm)
            {
                //Mukund: VSPLUS-844, Page redirect on callback
                try
                {
                    if (e.GetValue("NotesMailAddress") != "" && e.GetValue("NotesMailAddress") != null)
                    {
                        ASPxWebControl.RedirectOnCallback("BlackBerryDeviceProbe.aspx?NotesMailAddress=" + e.GetValue("NotesMailAddress"));
                        Context.ApplicationInstance.CompleteRequest();//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
                    }

                    else
                    {
                        ASPxWebControl.RedirectOnCallback("BlackBerryDeviceProbe.aspx");
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

        protected void BlackBerryDeviceProbegrid_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            e.Cell.ToolTip = string.Format("{0}", e.CellValue);
        }

        protected void BlackBerryDeviceProbegrid_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            BlackBerry blackberryobject = new BlackBerry();
            blackberryobject.NotesMailAddress = Convert.ToString(e.Keys[0]);
            Object retval = VSWebBL.ConfiguratorBL.BlackBerryBL.Ins.delete(blackberryobject);
            ASPxGridView gridView = (ASPxGridView)sender;
            gridView.CancelEdit();
            e.Cancel = true;
            getdata();

        }

        protected void BlackBerryDeviceProbegrid_PageSizeChanged(object sender, EventArgs e)
        {
            //ProfilesGridView.PageIndex;
            VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("BlackBerryDeviceProbesgrid|BlackBerryDeviceProbegrid", BlackBerryDeviceProbegrid.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        }

        protected void ASPxGridView2_PageSizeChanged(object sender, EventArgs e)
        {
            //ProfilesGridView.PageIndex;
            VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("BlackBerryDeviceProbesgrid|ASPxGridView2", ASPxGridView2.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        }

        protected void NewButton_Click(object sender, EventArgs e)
        {
            //3/2/2015 NS added for VSPLUS-1432
            Response.Redirect("~/Configurator/BlackberryDeviceProbe.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
            Context.ApplicationInstance.CompleteRequest();
        }
     }
 }
