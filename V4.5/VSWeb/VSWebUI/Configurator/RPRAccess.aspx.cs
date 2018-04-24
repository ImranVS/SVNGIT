using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VSWebBL;
using System.Data;
using VSWebDO;
using System.Web.Security;
using DevExpress.Web;

//VSPLUS-206: Mukund 28-Jan-14
namespace VSWebUI.Security
{
    public partial class RPRAccess : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            rprAccess.Visible = false;
            Pwd.Visible = true;
            if (Session["rprAccess"] != null)
            {
                if (Session["rprAccess"] != "")
                {
                    rprAccess.Visible = true;
                    Pwd.Visible = false;
                }
            }

            if (!IsPostBack)
            {
                FillGrid();
                if (Session["UserPreferences"] != null)
                {
                    DataTable UserPreferences = (DataTable)Session["UserPreferences"];
                    foreach (DataRow dr in UserPreferences.Rows)
                    {
                        if (dr[1].ToString() == "RPRAccess|RPRAccessGridView")
                        {
                            RPRAccessGridView.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                        }
                    }
                }
            }
            else
            {
                FillGridfromSession();
            }

        }

        protected void CancelButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Login.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
            Context.ApplicationInstance.CompleteRequest();
        }

        protected void SaveButton_Click(object sender, EventArgs e)
        {
            rprAccess.Visible = false;
            Pwd.Visible = true;
            string rpraccess = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("RPR Access Pwd");
            if (rpraccess == PwdTextBox.Text)
            {
                Session["rprAccess"] = "RPR Access Pwd";
                rprAccess.Visible = true;
                Pwd.Visible = false;
                
            }
        }

        public void FillGrid()
        {
            DataTable RPRAccessTable = new DataTable();
            try
            {
                    RPRAccessTable = VSWebBL.SecurityBL.AdminTabBL.Ins.GetRPRAccessData();
                
                Session["RPRAccessTable"] = RPRAccessTable;
                RPRAccessGridView.DataSource = RPRAccessTable;
                RPRAccessGridView.DataBind();

                ((GridViewDataColumn)RPRAccessGridView.Columns["Category"]).GroupBy();
                // ReportsDataView.DataSource = RPRAccessTable;
                //  ReportsDataView.DataBind();

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
        public void FillGridfromSession()
        {
            try
            {
                DataTable RPRAccessTable = new DataTable();
                RPRAccessTable = Session["RPRAccessTable"] as DataTable;
                RPRAccessGridView.DataSource = RPRAccessTable;
                RPRAccessGridView.DataBind();

                ((GridViewDataColumn)RPRAccessGridView.Columns["Category"]).GroupBy();

                //  ReportsDataView.DataSource = RPRAccessTable;
                // ReportsDataView.DataBind();

            }
            catch (Exception ex)
            {
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }



        }
        protected void RPRAccessGridView_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
        {
            e.Row.Attributes.Add("onmouseover", "this.originalcolor=this.style.backgroundColor;" + "this.style.backgroundColor='#C0C0C0';");

            e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalcolor;");
        }
        protected void RPRAccessGridView_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            //if (e.DataColumn.FieldName == "PageURL")
            //    e.Cell.Text = e.CellValue.ToString() +"?M="+ Request.QueryString["M"];            

        }
        protected void RPRAccessGridView_SelectionChanged(object sender, EventArgs e)
        {
            if (RPRAccessGridView.Selection.Count > 0)
            {
                System.Collections.Generic.List<object> Type = RPRAccessGridView.GetSelectedFieldValues("PageURL");

                if (Type.Count > 0)
                {
                    string URL = Type[0].ToString();
                    // Session["Type"] = Type[0];
                    //Mukund: VSPLUS-844, Page redirect on callback
                    try
                    {
                        DevExpress.Web.ASPxWebControl.RedirectOnCallback(URL);
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
        protected void RPRAccessGridView_PageSizeChanged(object sender, EventArgs e)
        {
            //ProfilesGridView.PageIndex;
            VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("RPRAccess|RPRAccessGridView", RPRAccessGridView.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        }
   
    }
}