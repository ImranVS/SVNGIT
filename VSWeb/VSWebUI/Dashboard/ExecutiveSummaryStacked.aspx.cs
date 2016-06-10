using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace VSWebUI.Dashboard
{
    public partial class ExecutiveSummaryStacked : System.Web.UI.Page
    {
        //To sync with Master page refresh below 2 events are used Page_PreInit, Master_ButtonClick
        protected void Page_PreInit(object sender, EventArgs e)
        {
            //Mukund 26Apr15, Create an event handler for the master page's contentCallEvent event
            this.Master.contentCallEvent += new EventHandler(Master_ButtonClick);
        }
        private void Master_ButtonClick(object sender, EventArgs e)
        {
            //Mukund 26Apr15, This Method will be Called from Timer Click from Master page


        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetStatus();
            }
        }

        protected void GetStatus()
        {
            double widthval = 0;
            DataRow[] foundRows;
            System.Web.UI.HtmlControls.HtmlGenericControl ahref = new System.Web.UI.HtmlControls.HtmlGenericControl("a");
            System.Web.UI.HtmlControls.HtmlGenericControl dynDiv = new System.Web.UI.HtmlControls.HtmlGenericControl("div");
            System.Web.UI.HtmlControls.HtmlGenericControl dynDivAll = new System.Web.UI.HtmlControls.HtmlGenericControl("div");
            System.Web.UI.HtmlControls.HtmlGenericControl dynDivHeader = new System.Web.UI.HtmlControls.HtmlGenericControl("div");
            try
            {
                DataTable dt = VSWebBL.DashboardBL.DashboardBL.Ins.GetStatusCountByType();
                DataTable dt2 = VSWebBL.DashboardBL.DashboardBL.Ins.GetStatusCountTotalByType();
                if (dt.Rows.Count > 0)
                {
                    string currtype = dt.Rows[0]["Type"].ToString();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (currtype != dt.Rows[i]["Type"].ToString() || i==0)
                        {
                            currtype = dt.Rows[i]["Type"].ToString();
                            dynDivHeader = new System.Web.UI.HtmlControls.HtmlGenericControl("div");
                            dynDivHeader.ID = dt.Rows[i]["Type"].ToString().Trim() + "Div";
                            dynDivHeader.Attributes["class"] = "subheader";
                            dynDivHeader.Attributes["runat"] = "server";
                            dynDivHeader.InnerHtml = currtype;
                            ASPxPanel1.Controls.Add(dynDivHeader);
                            dynDivAll = new System.Web.UI.HtmlControls.HtmlGenericControl("div");
                            dynDivAll.ID = dt.Rows[i]["Type"].ToString().Trim() + "progressDiv";
                            dynDivAll.Attributes["class"] = "progress";
                            ASPxPanel1.Controls.Add(dynDivAll);
                        }
                        dynDiv = new System.Web.UI.HtmlControls.HtmlGenericControl("div");
                        dynDiv.ID = dt.Rows[i]["Type"].ToString().Trim() + "Div" + (i+1).ToString();
                        ahref = new System.Web.UI.HtmlControls.HtmlGenericControl("a");
                        switch (dt.Rows[i]["OrderNum"].ToString())
                        {
                            case "1":
                                dynDiv.Attributes["class"] = "progress-bar progress-bar-success";
                                ahref.Attributes["href"] = "DeviceTypeList.aspx?status=OK&typeloc=" + dt.Rows[i]["Type"].ToString().Trim();
                                dynDiv.InnerHtml = "OK";
                                break;
                            case "2":
                                dynDiv.Attributes["class"] = "progress-bar progress-bar-warning";
                                ahref.Attributes["href"] = "DeviceTypeList.aspx?status=Issue&typeloc=" + dt.Rows[i]["Type"].ToString().Trim();
                                dynDiv.InnerHtml = "Issue";
                                break;
                            case "3":
                                dynDiv.Attributes["class"] = "progress-bar progress-bar-danger";
                                ahref.Attributes["href"] = "DeviceTypeList.aspx?status=Not%20Responding&typeloc=" + dt.Rows[i]["Type"].ToString().Trim();
                                dynDiv.InnerHtml = "Down";
                                break;
                            case "4":
                                dynDiv.Attributes["class"] = "progress-bar progress-bar-info";
                                ahref.Attributes["href"] = "DeviceTypeList.aspx?status=Maintenance&typeloc=" + dt.Rows[i]["Type"].ToString().Trim();
                                dynDiv.InnerHtml = "Maintenance";
                                break;
                        }
                        foundRows = dt2.Select("Type='" + currtype + "'");
                        if (foundRows[0] != null)
                        {
                            widthval = (Math.Round(Convert.ToDouble(dt.Rows[i]["StatusCount"].ToString()) / Convert.ToDouble(foundRows[0]["TotalStatusCount"].ToString()),2)) * 100;
                        }
                        dynDiv.Style.Add(HtmlTextWriterStyle.Width, widthval.ToString() + "%");
                        ahref.Controls.Add(dynDiv); 
                        dynDivAll.Controls.Add(ahref);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
        }
    }
}