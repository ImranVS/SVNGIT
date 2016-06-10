using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DevExpress.Web;

namespace VSWebUI.Configurator
{
    public partial class ScriptDefGrid : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                fillGrid();
            }
            else
            {
                fillGridFromSession();
            }
            //ASPxPopupControl1.ShowOnPageLoad = false;
        }

        public void fillGrid()
        {
            DataTable ScriptsTable = new DataTable();
            ScriptsTable = VSWebBL.ConfiguratorBL.AlertsBL.Ins.GetCustomScripts();
            try
            {
                Session["CustomScriptsTable"] = ScriptsTable;
                ScriptDefGridView.DataSource = ScriptsTable;
                ScriptDefGridView.DataBind();
            }
            catch (Exception ex)
            {
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw;
            }
        }

        public void fillGridFromSession()
        {
            DataTable ScriptsTable = new DataTable();
            if (Session["CustomScriptsTable"] != "" && Session["CustomScriptsTable"] != null)
                ScriptsTable = (DataTable)Session["CustomScriptsTable"];
            try
            {
                ScriptDefGridView.DataSource = ScriptsTable;
                ScriptDefGridView.DataBind();
            }
            catch (Exception ex)
            {
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw;
            }
        }

        protected void ScriptDefGridView_HtmlRowCreated(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
        {
            e.Row.Attributes.Add("onmouseover", "this.originalcolor=this.style.backgroundColor;" + "this.style.backgroundColor='#C0C0C0';");
            e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalcolor;");
            if (e.RowType == GridViewRowType.EditForm)
            {
                try
                {
                    if (e.GetValue("ID") != " " && e.GetValue("ID") != null)
                    {
                        ASPxWebControl.RedirectOnCallback("ScriptDef.aspx?ID=" + e.GetValue("ID"));
                        Context.ApplicationInstance.CompleteRequest();
                    }
                    else
                    {
                        ASPxWebControl.RedirectOnCallback("ScriptDef.aspx");
                        Context.ApplicationInstance.CompleteRequest();
                    }
                }
                catch (Exception ex)
                {
                    Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                }
            }
        }

        protected void ScriptDefGridView_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            DataTable dt = new DataTable();
            try
            {
                e.Cancel = true;
                dt = VSWebBL.ConfiguratorBL.AlertsBL.Ins.GetAlertsByScriptID(e.Keys[0].ToString());
                if (dt.Rows.Count > 0)
                {
                    ASPxPopupControl1.ShowOnPageLoad = true;
                }
                else
                {
                    try
                    {
                        VSWebBL.ConfiguratorBL.AlertsBL.Ins.DeleteScript(e.Keys[0].ToString());
                    }
                    catch (Exception ex)
                    {
                        //6/27/2014 NS added for VSPLUS-634
                        Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                    }
                    ASPxGridView gridview = (ASPxGridView)sender;
                    gridview.CancelEdit();
                    fillGrid();
                }
            }
            catch (Exception ex)
            {
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
            }
        }

        protected void ASPxButton1_Click(object sender, EventArgs e)
        {
            ASPxPopupControl1.ShowOnPageLoad = false;
        }

        protected void bttnDelete_Click(object sender, EventArgs e)
        {
            ImageButton bttnDel = (ImageButton)sender;
            DataTable dt = new DataTable();
            dt = VSWebBL.ConfiguratorBL.AlertsBL.Ins.GetAlertsByScriptID(bttnDel.CommandArgument);
            if (dt.Rows.Count > 0)
            {
                ASPxPopupControl1.ShowOnPageLoad = true;
            }
            else
            {
                try
                {
                    VSWebBL.ConfiguratorBL.AlertsBL.Ins.DeleteScript(bttnDel.CommandArgument);
                }
                catch (Exception ex)
                {
                    //6/27/2014 NS added for VSPLUS-634
                    Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                }
                fillGrid();
            }
        }

        protected void NewButton_Click(object sender, EventArgs e)
        {
            //3/2/2015 NS added for VSPLUS-1432
            Response.Redirect("~/Configurator/ScriptDef.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
            Context.ApplicationInstance.CompleteRequest();
        }
    }
}