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
    public partial class WebForm13 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Title = "Power Shell Script";
            if (!IsPostBack)
            {

                FillPowerGrid();
                if (Session["UserPreferences"] != null)
                {
                    DataTable UserPreferences = (DataTable)Session["UserPreferences"];
                    foreach (DataRow dr in UserPreferences.Rows)
                    {
                        if (dr[1].ToString() == "PowerShellScripts|PwrShelGridView")
                        {
                            PwrShelGridView.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                        }
                    }
                }
            }
            else
            {

                FillPowerGridfromSession();

            }   
        }

        private void FillPowerGrid()
        {
            try
            {

                DataTable pwrshltable = new DataTable();

                pwrshltable = VSWebBL.ConfiguratorBL.PwrshelBL.Ins.GetPwrData();
                if (pwrshltable.Rows.Count > 0)
                {
                    Session["PwrShel"] = pwrshltable;
                    PwrShelGridView.DataSource = pwrshltable;
                    PwrShelGridView.DataBind();
                    ((GridViewDataColumn)PwrShelGridView.Columns["Category"]).GroupBy();
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

        private void FillPowerGridfromSession()
        {
            try
            {

                DataTable pwrshltable = new DataTable();
                if (Session["PwrShel"] != "" && Session["PwrShel"] != null)
                    pwrshltable = (DataTable)Session["PwrShel"];//VSWebBL.ConfiguratorBL.DominoPropertiesBL.Ins.GetAllData();
                if (pwrshltable.Rows.Count > 0)
                {
                    PwrShelGridView.DataSource = pwrshltable;
                    PwrShelGridView.DataBind();
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

        protected void PwrShelGridView_HtmlRowCreated(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType == GridViewRowType.EditForm)
            {
                e.Row.Attributes.Add("onmouseover", "this.originalcolor=this.style.backgroundColor;" + "this.style.backgroundColor='#C0C0C0';");

                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalcolor;");
                //Mukund: VSPLUS-844, Page redirect on callback
                try
                {
               
                if (e.GetValue("ScriptName") != " " && e.GetValue("ScriptName") != null)
                {
                    ASPxWebControl.RedirectOnCallback("PwrShellEdit.aspx?ID=" + e.GetValue("ScriptName"));
                    Context.ApplicationInstance.CompleteRequest();//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback

                }
                else
                {
                    ASPxWebControl.RedirectOnCallback("PwrShellEdit.aspx");
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

        protected void PwrShelGridView_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            PowershellScripts PwrObject = new PowershellScripts();
            PwrObject.ScriptName = e.Keys[0].ToString();
            //Delete row from DB
            Object ReturnValue = VSWebBL.ConfiguratorBL.PwrshelBL.Ins.DeleteData(PwrObject);

            //Update Grid after inserting new row, refresh grid as in page load
            ASPxGridView gridView = (ASPxGridView)sender;
            gridView.CancelEdit();
            e.Cancel = true;
            FillPowerGrid();
        }

        protected void PwrShelGridView_PageSizeChanged(object sender, EventArgs e)
        {
            //ProfilesGridView.PageIndex;
            VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("PowerShellScripts|PwrShelGridView", PwrShelGridView.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        }
    }
}