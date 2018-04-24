using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VSWebBL;
using System.Data;
using DevExpress.Web;


namespace VSWebUI.Configurator
{
    public partial class SNMPDevicesGrid : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Title = "SNMP Devices";
            if (!IsPostBack)
            {
                Session["Submenu"] = "";
                FillSNMPDevicesGrid();
                if (Session["UserPreferences"] != null)
                {
                    DataTable UserPreferences = (DataTable)Session["UserPreferences"];
                    foreach (DataRow dr in UserPreferences.Rows)
                    {
                        if (dr[1].ToString() == "SNMPDevicesGrid|SNMPDevicesGridView")
                        {
                            SNMPDevicesGridView.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                        }
                    }
                }
            }
            else
            {

                FillSNMPDevicesGridfromSession();

            }
            //3/19/2014 NS added
            if (Session["SNMPDeviceUpdateStatus"] != null)
            {
                if (Session["SNMPDeviceUpdateStatus"].ToString() != "")
                {
                    //10/6/2014 NS modified for VSPLUS-990
                    successDiv.InnerHtml = "SNMP device information for <b>" + Session["SNMPDeviceUpdateStatus"].ToString() +
                        "</b> updated successfully."+
                        "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                    successDiv.Style.Value = "display: block";
                    Session["SNMPDeviceUpdateStatus"] = "";
                }
            }
        }


        private void FillSNMPDevicesGrid()
        {
            try
            {

                DataTable DCTaskSettingsDataTable = new DataTable();

                DCTaskSettingsDataTable = VSWebBL.ConfiguratorBL.SNMPDevicesBL.Ins.GetAllData();
                if (DCTaskSettingsDataTable.Rows.Count >= 0)
                {

                    Session["SNMPDevices"] = DCTaskSettingsDataTable;
                    SNMPDevicesGridView.DataSource = DCTaskSettingsDataTable;
                    SNMPDevicesGridView.DataBind();
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

        private void FillSNMPDevicesGridfromSession()
        {
            try
            {

                DataTable DCTaskSettingsDataTable = new DataTable();
                if ((Session["SNMPDevices"] != null) && (Session["SNMPDevices"] != ""))
                {
                    DCTaskSettingsDataTable = (DataTable)Session["SNMPDevices"];//VSWebBL.ConfiguratorBL.DominoPropertiesBL.Ins.GetAllData();
                    if (DCTaskSettingsDataTable.Rows.Count > 0)
                    {
                        SNMPDevicesGridView.DataSource = DCTaskSettingsDataTable;
                        SNMPDevicesGridView.DataBind();
                    }
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

        protected void SNMPDevicesGridView_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
        {
            e.Row.Attributes.Add("onmouseover", "this.originalcolor=this.style.backgroundColor;" + "this.style.backgroundColor='#C0C0C0';");

            e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalcolor;");

            if (e.RowType == GridViewRowType.EditForm)
            {
                //Mukund: VSPLUS-844, Page redirect on callback
                try
                {
                    if (e.GetValue("ID") != " " && e.GetValue("ID") != null)
                    {
                        ASPxWebControl.RedirectOnCallback("SNMPDeviceProperties.aspx?ID=" + e.GetValue("ID"));
                        Context.ApplicationInstance.CompleteRequest();//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback

                    }
                    else
                    {
                        ASPxWebControl.RedirectOnCallback("SNMPDeviceProperties.aspx");
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

        protected void SNMPDevicesGridView_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {

            SNMPDevices DCObject = new SNMPDevices();
            DCObject.ID = Convert.ToInt32(e.Keys[0]);
            //Delete row from DB
            Object ReturnValue = VSWebBL.ConfiguratorBL.SNMPDevicesBL.Ins.DeleteData(DCObject);

            //Update Grid after inserting new row, refresh grid as in page load
            ASPxGridView gridView = (ASPxGridView)sender;
            gridView.CancelEdit();
            e.Cancel = true;
            FillSNMPDevicesGrid();

        }

        protected void SNMPDevicesGridView_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            e.Cell.ToolTip = string.Format("{0}", e.CellValue);
        }

        protected void SNMPDevicesGridView_PageSizeChanged(object sender, EventArgs e)
        {
            //ProfilesGridView.PageIndex;
            VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("SNMPDevicesGrid|SNMPDevicesGridView", SNMPDevicesGridView.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        }

    }
}