using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;
using System.Configuration;
using DevExpress.Web;
using System.Web.UI.HtmlControls;
using System.Drawing;
using DevExpress.XtraCharts;

namespace VSWebUI.Dashboard
{
	public partial class WebsphereServerHealth : System.Web.UI.Page
	{
		protected void Page_PreInit(object sender, EventArgs e)
		{

			this.Master.contentCallEvent += new EventHandler(Master_ButtonClick);
		}

		private void Master_ButtonClick(object sender, EventArgs e)
		{

		}
		int cellvalue;
		int nodevalue;
        DataTable srvroles = new DataTable();
        //10/15/2014 NS added for VE-133
        DataTable oslist = new DataTable();
        DataTable statuslist = new DataTable();
        protected void Page_Load(object sender, EventArgs e)
        {
			//if (!IsPostBack)
			//{
				FillWebsphereCellStatusGrid();
				if (WebsphereCellGridview.VisibleRowCount > 0)
				{
					int index = WebsphereCellGridview.FocusedRowIndex;
					if (index > -1)
					{
						cellvalue = Convert.ToInt32(WebsphereCellGridview.GetRowValues(index, "CellID").ToString());
					}
				}
				
				FillWebsphereNodeStatusGrid();
				{
					int index = WebsphereNodesgridview.FocusedRowIndex;
					if (index > -1)
					{
						nodevalue = Convert.ToInt32(WebsphereNodesgridview.GetRowValues(index, "NodeID").ToString());
					}
				}
				FillWebsphereServersStatusGrid();
                DataColumn statuscol = new DataColumn("Status", typeof(string));
                statuslist.Columns.Add(statuscol);
                statuscol = new DataColumn("StatusCount", typeof(int));
                statuslist.Columns.Add(statuscol);
                SetStatusChart();


                if (Session["UserPreferences"] != null)
                {
                    DataTable UserPreferences = (DataTable)Session["UserPreferences"];
                    foreach (DataRow dr in UserPreferences.Rows)
                    {
                        if (dr[1].ToString() == "WebsphereServerHealth|WebsphereCellGridview")
                        {
                            WebsphereCellGridview.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                        }
                        if (dr[1].ToString() == "WebsphereServerHealth|WebsphereNodesgridview")
                        {
                            WebsphereNodesgridview.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                        }
                        if (dr[1].ToString() == "WebsphereServerHealth|Websphereservergridview")
                        {
                            Websphereservergridview.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                        }
                    }
                }
			}
		//}


		public void FillWebsphereCellStatusGrid()
		{
			
			DataTable dt = new DataTable();

			dt = VSWebBL.DashboardBL.WebsphereBL.Ins.GetWebsphereCellStatus();
				WebsphereCellGridview.DataSource = dt;
				WebsphereCellGridview.DataBind();
			
			
			
		}

		public void FillWebsphereNodeStatusGrid()
		{

			DataTable dt = new DataTable();

			dt = VSWebBL.DashboardBL.WebsphereBL.Ins.GetWebsphereNodeStatus(cellvalue);
			WebsphereNodesgridview.DataSource = dt;
			WebsphereNodesgridview.DataBind();



		}
		public void FillWebsphereServersStatusGrid()
		{

			DataTable dt = new DataTable();

			dt = VSWebBL.DashboardBL.WebsphereBL.Ins.GetWebsphereseversStatus(cellvalue);
			Websphereservergridview.DataSource = dt;
			Websphereservergridview.DataBind();



		}
		protected void WebsphereCellGridview_HtmlRowCreated(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)

{
			e.Row.Attributes.Add("onmouseover", "this.originalcolor=this.style.backgroundColor;" + "this.style.backgroundColor='#C0C0C0';");

			e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalcolor;");


		}

		protected void WebsphereNodesgridview_HtmlRowCreated(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
		
		{
			e.Row.Attributes.Add("onmouseover", "this.originalcolor=this.style.backgroundColor;" + "this.style.backgroundColor='#C0C0C0';");

			e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalcolor;");


		}
		//protected void Websphereservergridview_SelectionChanged(object sender, EventArgs e)

		//{
		//    if (Websphereservergridview.Selection.Count > 0)
		//    {

		//        List<object> fieldValues = Websphereservergridview.GetSelectedFieldValues(new string[] { "ServerID" });
					
		//            try
		//            {
		//                DevExpress.Web.ASPxWebControl.RedirectOnCallback(fieldValues[0].ToString());
		//                Context.ApplicationInstance.CompleteRequest();
		//            }
		//            catch (Exception ex)
		//            {
		//                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
						
		//            }
				
		//    }
		//}
        protected void WebsphereCellGridview_OnPageSizeChanged(object sender, EventArgs e)
        {
            //ProfilesGridView.PageIndex;
            VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("WebsphereServerHealth|WebsphereCellGridview", WebsphereCellGridview.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        }
        protected void WebsphereNodesgridview_OnPageSizeChanged(object sender, EventArgs e)
        {
            //ProfilesGridView.PageIndex;
            VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("WebsphereServerHealth|WebsphereNodesgridview", WebsphereNodesgridview.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        }
        protected void Websphereservergridview_OnPageSizeChanged(object sender, EventArgs e)
        {
            //ProfilesGridView.PageIndex;
            VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("WebsphereServerHealth|Websphereservergridview", Websphereservergridview.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        }
        protected void Websphereservergridview_SelectionChanged(object sender, EventArgs e)
        {


            if (Websphereservergridview.Selection.Count > 0)
            {
                if (Session["UserFullName"] != null)
                {

                    List<object> fieldValues = Websphereservergridview.GetSelectedFieldValues(new string[] { "redirectto" });
                    //7/22/2014 NS commented the line below, uncommented the following line - the page would auto redirect on each refresh
                    //Response.Redirect(fieldValues[0].ToString());

                    //Mukund: VSPLUS-844, Page redirect on callback
                    try
                    {
                        Response.Redirect(fieldValues[0].ToString());
                        //DevExpress.Web.ASPxWebControl.RedirectOnCallback(fieldValues[0].ToString());
                        //Context.ApplicationInstance.CompleteRequest();//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
                    }
                    catch (Exception ex)
                    {
                        Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                        //throw ex;
                    }
                }
                //    if (Websphereservergridview.Selection.Count > 0)
                //    {
                //        System.Collections.Generic.List<object> ServerID = Websphereservergridview.GetSelectedFieldValues("ServerID");
                //        System.Collections.Generic.List<object> ServerName = Websphereservergridview.GetSelectedFieldValues("ServerName");


                //            int srID = Convert.ToInt32(ServerID[0].ToString());
                //            string srName = ServerName[0].ToString();
                //            //DevExpress.Web.ASPxWebControl.RedirectOnCallback("WebSphereServerDetailsPage.aspx.aspx?ServerID=" + srID + "&ServerName=" + srName + "");
                //            //Context.ApplicationInstance.CompleteRequest();	
                //            Response.Redirect("WebSphereServerDetailsPage.aspx?ServerName=" + srName + "");


                //}
            }
        }
		protected void WebsphereCellGridview_HtmlDataCellPrepared(object sender, DevExpress.Web.ASPxGridViewTableDataCellEventArgs e)
		
		{
			string status = "";
			status = e.GetValue("Status").ToString();

			if (e.DataColumn.FieldName == "Status" && (e.CellValue.ToString() == "OK" || e.CellValue.ToString() == "Scanning" || e.CellValue.ToString() == "Telnet"))
			{
				e.Cell.BackColor = System.Drawing.Color.LightGreen;
			}

			else if (e.DataColumn.FieldName == "Status" && e.CellValue.ToString() == "Not Responding")
			{
				e.Cell.BackColor = System.Drawing.Color.Red;
				e.Cell.ForeColor = System.Drawing.Color.White;
			}
			else if (e.DataColumn.FieldName == "Status" && e.CellValue.ToString() == "Not Scanned")
			{
				e.Cell.BackColor = System.Drawing.Color.Blue;
				e.Cell.ForeColor = System.Drawing.Color.White;
			}
			else if (e.DataColumn.FieldName == "Status" && e.CellValue.ToString() == "Disabled")
			{
				e.Cell.BackColor = System.Drawing.Color.FromName("#D0D0D0");
				//e.Cell.BackColor = System.Drawing.Color.Gray;
				// e.Cell.ForeColor = System.Drawing.Color.White;
			}
			else if (e.DataColumn.FieldName == "Status" && e.CellValue.ToString() == "Issue")
			{
				e.Cell.BackColor = System.Drawing.Color.Yellow;
			}
			else if (e.DataColumn.FieldName == "Status")
			{
				e.Cell.BackColor = System.Drawing.Color.Yellow;
				// e.DataColumn.GroupFooterCellStyle.ForeColor = System.Drawing.Color.Yellow;

			}

		}
		protected void WebsphereNodesgridview_HtmlDataCellPrepared(object sender, DevExpress.Web.ASPxGridViewTableDataCellEventArgs e)
		
		{
			string status = "";
			status = e.GetValue("Status").ToString();

			if (e.DataColumn.FieldName == "Status" && (e.CellValue.ToString() == "OK" || e.CellValue.ToString() == "Scanning" || e.CellValue.ToString() == "Telnet"))
			{
				e.Cell.BackColor = System.Drawing.Color.LightGreen;
			}

			else if (e.DataColumn.FieldName == "Status" && e.CellValue.ToString() == "Not Responding")
			{
				e.Cell.BackColor = System.Drawing.Color.Red;
				e.Cell.ForeColor = System.Drawing.Color.White;
			}
			else if (e.DataColumn.FieldName == "Status" && e.CellValue.ToString() == "Not Scanned")
			{
				e.Cell.BackColor = System.Drawing.Color.Blue;
				e.Cell.ForeColor = System.Drawing.Color.White;
			}
			else if (e.DataColumn.FieldName == "Status" && e.CellValue.ToString() == "Disabled")
			{
				e.Cell.BackColor = System.Drawing.Color.FromName("#D0D0D0");
				//e.Cell.BackColor = System.Drawing.Color.Gray;
				// e.Cell.ForeColor = System.Drawing.Color.White;
			}
			else if (e.DataColumn.FieldName == "Status" && e.CellValue.ToString() == "Issue")
			{
				e.Cell.BackColor = System.Drawing.Color.Yellow;
			}
			else if (e.DataColumn.FieldName == "Status")
			{
				e.Cell.BackColor = System.Drawing.Color.Yellow;
				// e.DataColumn.GroupFooterCellStyle.ForeColor = System.Drawing.Color.Yellow;

			}

		}
		protected void Websphereservergridview_HtmlDataCellPrepared(object sender, DevExpress.Web.ASPxGridViewTableDataCellEventArgs e)
		{
			string status = "";
			status = e.GetValue("Status").ToString();

			if (e.DataColumn.FieldName == "Status" && (e.CellValue.ToString() == "OK" || e.CellValue.ToString() == "Scanning" || e.CellValue.ToString() == "Telnet"))
			{
				e.Cell.BackColor = System.Drawing.Color.LightGreen;
			}

			else if (e.DataColumn.FieldName == "Status" && e.CellValue.ToString() == "Not Responding")
			{
				e.Cell.BackColor = System.Drawing.Color.Red;
				e.Cell.ForeColor = System.Drawing.Color.White;
			}
			else if (e.DataColumn.FieldName == "Status" && e.CellValue.ToString() == "Not Scanned")
			{
				e.Cell.BackColor = System.Drawing.Color.Blue;
				e.Cell.ForeColor = System.Drawing.Color.White;
			}
			else if (e.DataColumn.FieldName == "Status" && e.CellValue.ToString() == "Disabled")
			{
				e.Cell.BackColor = System.Drawing.Color.FromName("#D0D0D0");
				//e.Cell.BackColor = System.Drawing.Color.Gray;
				// e.Cell.ForeColor = System.Drawing.Color.White;
			}
			else if (e.DataColumn.FieldName == "Status" && e.CellValue.ToString() == "Issue")
			{
				e.Cell.BackColor = System.Drawing.Color.Yellow;
			}
			else if (e.DataColumn.FieldName == "Status")
			{
				e.Cell.BackColor = System.Drawing.Color.Yellow;
				// e.DataColumn.GroupFooterCellStyle.ForeColor = System.Drawing.Color.Yellow;

			}

		}

        public void SetStatusChart()
        {
            statuslist = VSWebBL.DashboardBL.WebsphereBL.Ins.GetWebsphereStatusAll();
            StatusWebChart.DataSource = statuslist;
            StatusWebChart.Series[0].DataSource = statuslist;
            StatusWebChart.Series[0].ArgumentDataMember = statuslist.Columns[1].ToString();
            StatusWebChart.Series[0].ValueDataMembers.AddRange(statuslist.Columns[0].ToString());
            StatusWebChart.Series[0].Visible = true;
            StatusWebChart.DataBind();
        }

        protected void StatusWebChart_CustomDrawSeriesPoint(object sender, CustomDrawSeriesPointEventArgs e)
        {
            Pie3DDrawOptions drawOptions = e.SeriesDrawOptions as Pie3DDrawOptions;
            Pie3DDrawOptions legendOptions = e.LegendDrawOptions as Pie3DDrawOptions;
            drawOptions.FillStyle.FillMode = FillMode3D.Solid;
            legendOptions.FillStyle.FillMode = FillMode3D.Solid;
            if (e.SeriesPoint.Argument == "OK")
            {
                drawOptions.Color = System.Drawing.Color.FromArgb(0, 128, 0);
                legendOptions.Color = System.Drawing.Color.FromArgb(0, 128, 0);
            }
            else if (e.SeriesPoint.Argument == "Issue")
            {
                drawOptions.Color = System.Drawing.Color.FromArgb(242, 242, 0);
                legendOptions.Color = System.Drawing.Color.FromArgb(242, 242, 0);
            }
            else if (e.SeriesPoint.Argument == "Not Responding")
            {
                drawOptions.Color = System.Drawing.Color.FromArgb(253, 0, 0);
                legendOptions.Color = System.Drawing.Color.FromArgb(253, 0, 0);
            }
            else if (e.SeriesPoint.Argument == "Maintenance")
            {
                drawOptions.Color = System.Drawing.Color.FromArgb(80, 80, 80);
                legendOptions.Color = System.Drawing.Color.FromArgb(80, 80, 80);
            }
            else if (e.SeriesPoint.Argument == "Scanning")
            {
                drawOptions.Color = System.Drawing.Color.FromArgb(144, 238, 144);
                legendOptions.Color = System.Drawing.Color.FromArgb(144, 238, 144);
            }
        }
	}
}



			
		
	