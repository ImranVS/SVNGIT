using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VSWebBL;
using VSWebDO;
using System.Data;

using DevExpress.Web;
using System.Runtime.InteropServices;

namespace VSWebUI
{
    public partial class ELSDefinitions_Grid : System.Web.UI.Page
    {
        //[DllImport("AlertLibrary.dll",EntryPoint="QueueAlert")]
        //public static extern void QueueAlert(string DeviceType, string DeviceName, string AlertType, string Details, string Location);
        protected void Page_Load(object sender, EventArgs e)
        {
            //Page.Title = "Alert Definitions";
            if (!IsPostBack)
            {              
                fillGrid();
                if (Session["UserPreferences"] != null)
                {
                    DataTable UserPreferences = (DataTable)Session["UserPreferences"];
                    foreach (DataRow dr in UserPreferences.Rows)
                    {
                        if (dr[1].ToString() == "ELSDefinitions_Grid|ELSDefGridView")
                        {
                            ELSDefGridView.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                        }
                    }
                }
            }
            else
            {
                fillGridfromSession();
               
                
            }
        }

        

        


        public void fillGrid()
        {
            DataTable AlertsTable = new DataTable();
            AlertsTable = VSWebBL.ConfiguratorBL.ELSBL.Ins.GetELSNames();
            try
            {
                 Session["ELSNameTable"] = AlertsTable;
                 ELSDefGridView.DataSource = AlertsTable;
                 ELSDefGridView.DataBind();
            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw;
            }
        }

        public void fillGridfromSession()
        {
            DataTable AlertsTable = new DataTable();
			if (Session["ELSNameTable"] != "" && Session["ELSNameTable"] != null)
				AlertsTable = (DataTable)Session["ELSNameTable"];

            try
            {
                ELSDefGridView.DataSource = AlertsTable;
                ELSDefGridView.DataBind();
            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw;
            }          
        
        }

        protected void ELSDefGridView_HtmlRowCreated(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
        {
            e.Row.Attributes.Add("onmouseover", "this.originalcolor=this.style.backgroundColor;" + "this.style.backgroundColor='#C0C0C0';");

            e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalcolor;");


            if (e.RowType == GridViewRowType.EditForm)
            {
				string mode;
                try
                {
                    if (e.GetValue("ID") != " " && e.GetValue("ID") != null)
                    {
						mode = "update";
						DevExpress.Web.ASPxWebControl.RedirectOnCallback("ELS_Edit.aspx?EventKey=" + e.GetValue("ID") + "&Mode=" + mode + "&Name=" + e.GetValue("Name") + "");
                        Context.ApplicationInstance.CompleteRequest();//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
                    }
                    else
                    {
						mode = "insert";
						//Response.Redirect("ELS_Edit.aspx?Mode=" + mode, false);
						DevExpress.Web.ASPxWebControl.RedirectOnCallback("ELS_Edit.aspx?Mode=" + mode + "");
                        Context.ApplicationInstance.CompleteRequest();//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
                    }
                }
                catch (Exception ex)
                {
                    Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                }
               
            }
        }

        protected void ELSDefGridView_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            e.Cell.ToolTip = string.Format("{0}", e.CellValue);
        }

        protected void ELSDefGridView_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            int id = Convert.ToInt32(e.Keys[0]);
            try
            {
				VSWebBL.ConfiguratorBL.ELSBL.Ins.DeleteELSDef(id);
            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
            }
            ASPxGridView gridview = (ASPxGridView)sender;
            gridview.CancelEdit();
            e.Cancel = true;
            fillGrid();

          }


        public static Control GetPostBackControl(Page page)
        {
            Control control = null;
            string ctrlname = page.Request.Params.Get("__EVENTTARGET");
            if (ctrlname != null && ctrlname != string.Empty)
            {
                control = page.FindControl(ctrlname);
            }
            else
            {
                foreach (string ctl in page.Request.Form.AllKeys)
                {
                    Control c = page.FindControl(ctl) as Control;
                    if (c is DevExpress.Web.ASPxButton)
                    {
                        control = c;
                        break;
                    }
                }
            }
            return control;
        }

        protected void ELSDefGridView_PageSizeChanged(object sender, EventArgs e)
        {
            //ProfilesGridView.PageIndex;
			VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("ELSDefinitions_Grid|ELSDefGridView", ELSDefGridView.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        
        }

        protected void NewButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Configurator/ELS_Edit.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
            Context.ApplicationInstance.CompleteRequest();
        }
    }
}