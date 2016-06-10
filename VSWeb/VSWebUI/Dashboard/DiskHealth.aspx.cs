using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DevExpress.Web;
using DevExpress.XtraCharts;
using DevExpress.XtraCharts.Web;
using System.Web.UI.HtmlControls;
using System.Drawing;
namespace VSWebUI.Dashboard
{
    public partial class DiskHealth : System.Web.UI.Page
    {
		protected void Page_PreInit(object sender, EventArgs e)
		{

			this.Master.contentCallEvent += new EventHandler(Master_ButtonClick);
		}

		private void Master_ButtonClick(object sender, EventArgs e)
		{

		}
        string strDiskYellowTh;
        protected void Page_Load(object sender, EventArgs e)
        {
            webChartDiskHealth.Width = new Unit(Convert.ToInt32(chartWidth.Value));

            strDiskYellowTh = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("DiskYellowThreshold");
            strDiskYellowTh = (strDiskYellowTh == "" ? "20" : strDiskYellowTh);

            if (!IsPostBack)
            {
                
                HtmlGenericControl body = (HtmlGenericControl)Master.FindControl("Body1");
                body.Attributes.Add("onload", "DoCallback()");
                body.Attributes.Add("onResize", "Resized()");
                
                if (Request.QueryString["Server"] != "" && Request.QueryString["Server"] != null)
                {
                    SetGridFromQString(Request.QueryString["Server"]);                    
                }
                else
                {
                    SetGrid();                    
                }
                if (Session["UserPreferences"] != null)
                {
                    DataTable UserPreferences = (DataTable)Session["UserPreferences"];
                    foreach (DataRow dr in UserPreferences.Rows)
                    {
                        if (dr[1].ToString() == "DiskHealth|DiskHealthGrid")
                        {
                            DiskHealthGrid.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                        }
                    }
                }
            }
            else
            {                
                if (Request.QueryString["Server"] != "" && Request.QueryString["Server"] != null)
                {
                    SetGridFromQString(Request.QueryString["Server"]);                    
                }
                else
                {
                    SetGridFromSession();                    
                }
            }
        }

        public void SetGrid()
        {
            try
            {
                DataTable dt = VSWebBL.DashboardBL.DiskHealthBLL.Ins.SetGrid();
                Session["GridData"] = dt;
                DiskHealthGrid.DataSource = dt;
                DiskHealthGrid.DataBind();
                ((GridViewDataColumn)DiskHealthGrid.Columns["ServerName"]).GroupBy();
                int rowIndex = DiskHealthGrid.FindVisibleIndexByKeyValue("ID");
                //Session["rowIndex"] = rowIndex;
            }
            catch (Exception ex)
            {
                //Response.Write("Error : " + ex);
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
        }

        public void SetGridFromSession()
        {
            try
            {
                if (Session["GridData"] != null)
                {
                    DataTable dt = Session["GridData"] as DataTable;
                    DiskHealthGrid.DataSource = dt;
                    DiskHealthGrid.DataBind();
                    int rowIndex = DiskHealthGrid.FindVisibleIndexByKeyValue("ID");
                    Session["rowIndex"] = rowIndex;
                }
            }
            catch (Exception ex)
            {
                //Response.Write("Error : " + ex);
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
        }

        public void SetGridFromQString(string serverName)
        {
            try
            {
                DataTable dt = VSWebBL.DashboardBL.DiskHealthBLL.Ins.SetGridFromQString(serverName);
                DiskHealthGrid.DataSource = dt;
                DiskHealthGrid.DataBind();
                ((GridViewDataColumn)DiskHealthGrid.Columns["ServerName"]).GroupBy();                
            }
            catch (Exception ex)
            {
                //Response.Write("Error : " + ex);
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
        }

        public void SetGraph(string serverName)
        {
            try
            {
                DataTable dt = new DataTable();
                //webChartDiskHealth.Series.Clear();
                dt = VSWebBL.DashboardBL.DiskHealthBLL.Ins.SetGraph(serverName,"");
                webChartDiskHealth.DataSource = dt;
                webChartDiskHealth.SeriesTemplate.ChangeView(ViewType.StackedBar);
                ChartTitle chartTitle1 = new ChartTitle();
                chartTitle1.Text = serverName;
                webChartDiskHealth.Titles.Add(chartTitle1);
                webChartDiskHealth.Series[0].DataSource = dt;
                webChartDiskHealth.Series[0].ArgumentDataMember = dt.Columns["DiskName"].ToString();
                webChartDiskHealth.Series[0].ValueDataMembers.AddRange(dt.Columns["DiskUsed"].ToString());
                webChartDiskHealth.Series[0].Visible = true;
                webChartDiskHealth.Series[1].DataSource = dt;
                webChartDiskHealth.Series[1].ArgumentDataMember = dt.Columns["DiskName"].ToString();
                webChartDiskHealth.Series[1].ValueDataMembers.AddRange(dt.Columns["DiskFree"].ToString());
                webChartDiskHealth.Series[1].Visible = true;

            }
            catch (Exception ex)
            {
                //Response.Write("Error : " + ex);
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
        }

        protected void ASPxCallbackPanel1_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
        {
            if (Request.QueryString["Server"] != "" && Request.QueryString["Server"] != null)
            {
                SetGridFromQString(Request.QueryString["Server"]);
                lblServerName.Text = Request.QueryString["Server"];
                SetGraph(lblServerName.Text);
            }
            else
            {
                //lblServerName.Text = "";
                int index = DiskHealthGrid.FocusedRowIndex;
                //8/9/2013 NS modified - when no rows were found, object reference not set error appeared
                if (index > -1)
                {
                    //3/2/2015 NS commented out
                    //webChartDiskHealth.Visible = true;
                    string value = DiskHealthGrid.GetRowValues(index, "ServerName").ToString();
                    lblServerName.Text = value;
                    SetGraph(value);
                }
                else
                {
                    webChartDiskHealth.Visible = false;
                }
            }
        }

        protected void btnCollapse_Click(object sender, EventArgs e)
        {
             if (btnCollapse.Text == "Collapse All Rows")
            {
                DiskHealthGrid.CollapseAll();
                btnCollapse.Image.Url = "~/images/icons/add.png"; 
                btnCollapse.Text = "Expand All Rows";
            }
            else
            {
                DiskHealthGrid.ExpandAll();
                btnCollapse.Image.Url = "~/images/icons/forbidden.png";
                btnCollapse.Text = "Collapse All Rows";

            }
               
            if (Request.QueryString["Server"] != "" && Request.QueryString["Server"] != null)
            {
                SetGridFromQString(Request.QueryString["Server"]);
                lblServerName.Text = Request.QueryString["Server"];
                SetGraph(lblServerName.Text);
            }
            else
            {
                if (Session["ServerName"] != null)
                {
                    int index = DiskHealthGrid.FocusedRowIndex;
                    string value = DiskHealthGrid.GetRowValues(index, "ServerName").ToString();
                    lblServerName.Text = value;
                    SetGraph(lblServerName.Text);
                }
                else
                {
                    lblServerName.Text = "";
                   // int index = DiskHealthGrid.FocusedRowIndex;
                    string value = DiskHealthGrid.GetRowValues(0, "ServerName").ToString();
                    lblServerName.Text = value;
                    SetGraph(lblServerName.Text);
                }
            }

            //int index = Convert.ToInt32(DiskHealthGrid.FocusedRowIndex);
            //int exCount = 0;
            //for (int i = DiskHealthGrid.SettingsPager.PageSize * DiskHealthGrid.PageIndex; i < index; i++)
            //{
            //    if (DiskHealthGrid.IsRowExpanded(i))
            //        exCount += 1;
            //}
            //DiskHealthGrid.CollapseAll();
            //DiskHealthGrid.FocusedRowIndex = index - exCount / 2;    

            
        }

        protected void btnExpand_Click(object sender, EventArgs e)
        {
            //DiskHealthGrid.ExpandAll();

            //if (Request.QueryString["Server"] != "" && Request.QueryString["Server"] != null)
            //{
            //    SetGridFromQString(Request.QueryString["Server"]);
            //    lblServerName.Text = Request.QueryString["Server"];
            //    SetGraph(lblServerName.Text);
            //}
            //else
            //{
            //    lblServerName.Text = "";
            //    int index = DiskHealthGrid.FocusedRowIndex;
            //    string value = DiskHealthGrid.GetRowValues(index, "ServerName").ToString();
            //    lblServerName.Text = value;
            //    SetGraph(lblServerName.Text);
            //    //Session["Index"] = index;
            //}            
        }

        protected void DiskHealthGrid_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
			string thresholdType = e.GetValue("ThresholdType").ToString();
			//if (thresholdType == "")
			//    thresholdType = "Percent";
            double valThreshold;
            if (e.GetValue("Threshold").ToString() != "")
            {
                valThreshold = (double)e.GetValue("Threshold");

				//if the threshold type is "%" then divide the value by 100
				if (thresholdType == "Percent" && valThreshold>0)
					valThreshold = (valThreshold / 100);
            }
            else
            {
                valThreshold = 0.0;
            }
            //3/9/2015 NS modified for VSPLUS-1464
			//if (e.DataColumn.FieldName == "DiskName")
            if (e.DataColumn.FieldName == "PercentFree")
			{
				if (thresholdType == "Percent")
				{
					double valPercentFree;
					if (e.GetValue("PercentFree").ToString() != "")
					{
						valPercentFree = (double)e.GetValue("PercentFree");
					}
					else
					{
						valPercentFree = 0.0;
					}

                    //if (valPercentFree > valThreshold && (valPercentFree <= 1 - (0.8 * (1 - valThreshold))))
                    float topValue = (1 - (float.Parse(strDiskYellowTh) / 100));
                    if (valPercentFree > valThreshold && (valPercentFree <= 1 - (topValue * (1 - valThreshold))))
					{
						e.Cell.BackColor = System.Drawing.Color.Yellow;
					}
					else if ((valPercentFree) <= valThreshold)
					{
						e.Cell.BackColor = System.Drawing.Color.Red;
						e.Cell.ForeColor = System.Drawing.Color.White;
					}
					else
					{
						e.Cell.BackColor = System.Drawing.Color.Green;
						e.Cell.ForeColor = System.Drawing.Color.White;
					}
				}
			}
            //3/9/2015 NS modified for VSPLUS-1464
            //if (e.DataColumn.FieldName == "DiskName")
            if (e.DataColumn.FieldName == "PercentFree")
			{
				if (thresholdType == "GB")
				{
					double valGBFree;
					if (e.GetValue("DiskFree").ToString() != "")
					{
						valGBFree = (double)e.GetValue("DiskFree");
					}
					else
					{
						valGBFree = 0.0;
					}

					//double valPercentFree = (double)e.GetValue("PercentFree");

					//if (valGBFree > valThreshold && (valPercentFree <= 1 - (0.8 * (1 - 0))))
                    //if (valGBFree > valThreshold && (valGBFree <= (valThreshold + ((20 * valThreshold) / 100))))
                    if (valGBFree > valThreshold && (valGBFree <= (valThreshold + ((float.Parse(strDiskYellowTh) * valThreshold) / 100))))
					{
						e.Cell.BackColor = System.Drawing.Color.Yellow;
					}
					else if ((valGBFree) <= valThreshold)
					{
						e.Cell.BackColor = System.Drawing.Color.Red;
						e.Cell.ForeColor = System.Drawing.Color.White;
					}
					else
					{
						e.Cell.BackColor = System.Drawing.Color.Green;
						e.Cell.ForeColor = System.Drawing.Color.White;
					}
				}
			}
            //if (e.DataColumn.FieldName == "AverageQueueLength")
            //{
            //    double valThreshold;
            //    if (e.GetValue("Threshold").ToString() != "")
            //    {
            //        valThreshold = (double)e.GetValue("Threshold");
            //    }
            //    else
            //    {
            //        valThreshold = 0.0;
            //    }

            //    if ((valThreshold) >= 0.3 && valThreshold < 1.0)
            //    {
            //        e.Cell.BackColor = System.Drawing.Color.Yellow;
            //    }
            //    else if ((valThreshold) >= 1.0)
            //    {
            //        e.Cell.BackColor = System.Drawing.Color.Red;
            //    }
            //    else
            //    {
            //        e.Cell.BackColor = System.Drawing.Color.Green;
            //        e.Cell.ForeColor = System.Drawing.Color.White;
            //    }
            //}
            //if (e.DataColumn.FieldName == "PercentUtilization")
            //{
            //    double valThreshold;
            //    if (e.GetValue("Threshold").ToString() != "")
            //    {
            //        valThreshold = (double)e.GetValue("Threshold");
            //    }
            //    else
            //    {
            //        valThreshold = 0.0;
            //    }

            //    if ((valThreshold) >= 0.8 && valThreshold < 1.0)
            //    {
            //        e.Cell.BackColor = System.Drawing.Color.Yellow;
            //    }
            //    else if ((valThreshold) >= 1.0)
            //    {
            //        e.Cell.BackColor = System.Drawing.Color.Red;
            //    }
            //    else
            //    {
            //        e.Cell.BackColor = System.Drawing.Color.Green;
            //        e.Cell.ForeColor = System.Drawing.Color.White;
            //    }
            //}
        }

        protected void DiskHealthGrid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            //ASPxGridView grid = sender as ASPxGridView;
            //int index = int.Parse(e.Parameters);
            //int exCount = 0;
            //for (int i = grid.SettingsPager.PageSize * grid.PageIndex; i < index; i++)
            //{
            //    if (grid.IsRowExpanded(i))
            //        exCount += 1;
            //}
            //grid.CollapseAll();
            //grid.FocusedRowIndex = index - exCount / 2;
            //grid.ExpandRow(index - exCount / 2);
            
        }

        protected void DiskHealthGrid_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
        {
            int index = DiskHealthGrid.FocusedRowIndex;
            if (e.VisibleIndex != index)
            {
                e.Row.Attributes.Add("onmouseover", "this.originalcolor=this.style.backgroundColor;" + "this.style.backgroundColor='#C0C0C0';");

                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalcolor;");

            }
        }

        protected void webChartDiskHealth_CustomCallback(object sender, DevExpress.XtraCharts.Web.CustomCallbackEventArgs e)
        {
            webChartDiskHealth.Width = new Unit(Convert.ToInt32(chartWidth.Value));
        }

        protected void webChartDiskHealth_CustomDrawSeriesPoint(object sender, CustomDrawSeriesPointEventArgs e)
        {
            //e.SeriesDrawOptions.Color = Color.Yellow;
        }

        protected void DiskHealthGrid_PageSizeChanged(object sender, EventArgs e)
        {
            //ProfilesGridView.PageIndex;
            VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("DiskHealth|DiskHealthGrid", DiskHealthGrid.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        }

		protected void DiskWebChart_Load(object sender, EventArgs e)
		{
			DataTable dt = new DataTable();
			WebChartControl chartControl = (WebChartControl)sender;
			DevExpress.Web.GridViewDataItemTemplateContainer gridc = (DevExpress.Web.GridViewDataItemTemplateContainer)chartControl.Parent;
			string srvname = DataBinder.Eval(gridc.DataItem, "ServerName").ToString();
			string diskname = DataBinder.Eval(gridc.DataItem, "DiskName").ToString();
			dt = VSWebBL.DashboardBL.DiskHealthBLL.Ins.SetGraph(srvname, diskname);
			chartControl.DataSource = dt;
			chartControl.Series[0].DataSource = dt;
			chartControl.Series[0].ArgumentDataMember = dt.Columns["DiskName"].ToString();
			chartControl.Series[0].ValueDataMembers.AddRange(dt.Columns["DiskUsed"].ToString());
			chartControl.Series[0].Visible = true;
			chartControl.Series[1].DataSource = dt;
			chartControl.Series[1].ArgumentDataMember = dt.Columns["DiskName"].ToString();
			chartControl.Series[1].ValueDataMembers.AddRange(dt.Columns["DiskFree"].ToString());
			chartControl.Series[1].Visible = true;
			chartControl.DataBind();
		}
		public string GetDiskInfo(GridViewDataItemTemplateContainer Container)

		{
			
			Label diskname = (Label)Container.FindControl("hfNameLabel");

			Label comments = (Label)Container.FindControl("lblReasons");
			Label detailsspan = (Label)Container.FindControl("detailsspan");
		//System.Web.UI.WebControls.Image Imgforinfo = (System.Web.UI.WebControls.Image)Container.FindControl("Imgforinfo");
		//	System.Drawing.Image Imgforinfo = (System.Drawing.Image)Container.FindControl("Imgforinfo");
			//Image Imgforinfo = (Image)Container.FindControl("");
			System.Web.UI.HtmlControls.HtmlImage Imgforinfo = (System.Web.UI.HtmlControls.HtmlImage)Container.FindControl("Imgforinfo");
			if (comments == null || comments.Text == "")
			{
				detailsspan.Visible = false;
				Imgforinfo.Visible = false;
			}
              return "";
		}
    }
}