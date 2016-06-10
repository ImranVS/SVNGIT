using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DevExpress.XtraCharts;
using System.Drawing;
using DevExpress.Web;
using System.Web.UI.HtmlControls;
using DevExpress.Utils;

namespace VSWebUI.Dashboard
{
    public partial class ClusterHealth : System.Web.UI.Page
    {
		protected void Page_PreInit(object sender, EventArgs e)
		{

			this.Master.contentCallEvent += new EventHandler(Master_ButtonClick);
		}

		private void Master_ButtonClick(object sender, EventArgs e)
		{


		}
        string fromdate;
        string todate;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            this.WebChartControl1.Width = new Unit(Convert.ToInt32(chartWidth.Value));
            //Mukund 22Dec14, commented below line, which was giving the WebChartControl1 a small width on Page load
            //this.WebChartControl1.Width = new Unit(Convert.ToInt32(chartWidth.Value));
            //11/25/2014 NS modified
            /*
            if (txtfromdate.Text == "" && txttodate.Text == "")
            {
                txtfromdate.Text = System.DateTime.Now.AddDays(-1).ToShortDateString();
                txttodate.Text = System.DateTime.Now.ToShortDateString();
            }
            //10/9/2013 NS added
            fromdate = txtfromdate.Text + " " + "00:00:00.000";
            todate = txttodate.Text + " " + "23:59:59.000";
             */
            if (dtPick.FromDate == "" && dtPick.ToDate == "")
            {
                fromdate = System.DateTime.Now.AddDays(-6).ToShortDateString() + " " + "00:00:00.000";
                todate = System.DateTime.Now.ToShortDateString() + " " + "23:59:59.000";
            }
            else
            {
                fromdate = dtPick.FromDate;
                todate = dtPick.ToDate;
            }
            if (!IsPostBack && !IsCallback)
            {
                Fillgrid();
                HtmlGenericControl body = (HtmlGenericControl)Master.FindControl("Body1");
                body.Attributes.Add("onResize", "Resized()");
                body.Attributes.Add("onload", "DoCallback()");
                if (Session["UserPreferences"] != null)
                {
                    DataTable UserPreferences = (DataTable)Session["UserPreferences"];
                    foreach (DataRow dr in UserPreferences.Rows)
                    {
                        if (dr[1].ToString() == "ClusterHealth|ClusterHealthGrid")
                        {
                            ClusterHealthGrid.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                        }
                    }
                }
            }
            else
            {
                Fillgridfromsession();
            }
            //9/15/2014 NS added for VSPLUS-921
            
            //10/9/2013 NS added
            int index = ClusterHealthGrid.FocusedRowIndex;
            if (index != -1)
            {
                string Type = ClusterHealthGrid.GetRowValues(index, "ServerName").ToString();
                if (Session["Type"] != "" && Session["Type"] != null)
                {
                    if (Session["Type"].ToString() != Type)
                    {
                        Session["Type"] = Type;
                    }
                }
                else
                {
                    Session["Type"] = Type;
                }
            }
            if (fromdate != "" && todate != "" && Session["Type"] != "" && Session["Type"] != null)
            {
                //WebChartControl1.Visible = true;
                SetGraphforCluster(Session["Type"].ToString(), fromdate, todate);
            }
            //11/25/2014 NS commented out
            /*
            if (Convert.ToDateTime(txtfromdate.Text) > Convert.ToDateTime(txttodate.Text))
            {
                MsgPopupControl.ShowOnPageLoad = true;
                ErrmsgLabel.Text = "From Date value should be less than To Date.";
            }
             */
        }

        public void Fillgrid()
        {
            DataTable dtClusterHealth = VSWebBL.DashboardBL.ClusterHealthBL.Ins.GetData();
            dtClusterHealth.PrimaryKey = new DataColumn[] { dtClusterHealth.Columns["ID"] };
            ClusterHealthGrid.DataSource = dtClusterHealth;
            ClusterHealthGrid.DataBind();
            ((GridViewDataColumn)ClusterHealthGrid.Columns["ClusterName"]).GroupBy();
            Session["ClusterHealth"] = dtClusterHealth;
        }

        public void Fillgridfromsession()
        {
            if (Session["ClusterHealth"] != "" && Session["ClusterHealth"] != null)
            {
                ClusterHealthGrid.DataSource = (DataTable)Session["ClusterHealth"];
                ClusterHealthGrid.DataBind();
            }
        }

        protected void ClusterHealthGrid_HtmlDataCellPrepared(object sender, DevExpress.Web.ASPxGridViewTableDataCellEventArgs e)
        {
            if (e.DataColumn.FieldName == "Analysis" )
            {
                if (e.DataColumn.FieldName == "Analysis" && (e.CellValue.ToString() == "Approaching Threshold" || e.CellValue.ToString() == "Approaching Threshold"))
                {
                    e.Cell.BackColor = System.Drawing.Color.Yellow;
                    e.Cell.ForeColor = System.Drawing.Color.Black;
                }
                else if ( e.DataColumn.FieldName == "Analysis"&& e.CellValue.ToString() == "OK")
                {
                    e.Cell.BackColor = System.Drawing.Color.LightGreen;
                    e.Cell.ForeColor = System.Drawing.Color.Black;
                }
                else if (e.DataColumn.FieldName == "Analysis" && e.CellValue.ToString() == "Not Configured for Load Balancing")
                {
                    e.Cell.BackColor = System.Drawing.Color.LightGray;
                    e.Cell.ForeColor = System.Drawing.Color.Black;
                }
            }
            if (e.DataColumn.FieldName == "SecondsOnQueue")
            {
                int val = Convert.ToInt32(e.CellValue.ToString());
                
                if (val<=30 && val>15)
                {
                    e.Cell.BackColor = System.Drawing.Color.Yellow;
                    e.Cell.ForeColor = System.Drawing.Color.Black;
                }
                else if (val<=15)
                {
                    e.Cell.BackColor = System.Drawing.Color.LightGreen;
                    e.Cell.ForeColor = System.Drawing.Color.Black;
                }
                else if (val>30)
                {
                    e.Cell.BackColor = System.Drawing.Color.Red;
                    e.Cell.ForeColor = System.Drawing.Color.White;
                    
                }
            }

            if (e.DataColumn.FieldName == "WorkQueueDepth")
            {
                int val = Convert.ToInt32(e.CellValue.ToString());

                if (val >=30)
                {
                    e.Cell.BackColor = System.Drawing.Color.Red;
                    e.Cell.ForeColor = System.Drawing.Color.White;
                }
                else if (val >= 15 && val<30)
                {
                    e.Cell.BackColor = System.Drawing.Color.Yellow;
                    e.Cell.ForeColor = System.Drawing.Color.Black;
                }
                else if (val < 15)
                {
                    e.Cell.BackColor = System.Drawing.Color.LightGreen;
                    e.Cell.ForeColor = System.Drawing.Color.Black;

                }
            }

            if (e.DataColumn.FieldName == "SecondsOnQueueAvg")
            {
                int val = Convert.ToInt32(e.CellValue.ToString());

                if (val <= 30 && val>15)
                {
                    e.Cell.BackColor = System.Drawing.Color.Yellow;
                    e.Cell.ForeColor = System.Drawing.Color.Black;
                }
                else if (val <=15)
                {
                    e.Cell.BackColor = System.Drawing.Color.LightGreen;
                    e.Cell.ForeColor = System.Drawing.Color.Black;
                }
                else if (val >30)
                {
                    e.Cell.BackColor = System.Drawing.Color.Red;
                    e.Cell.ForeColor = System.Drawing.Color.White;

                }
            }
            if (e.DataColumn.FieldName == "SecondsOnQueueMax")
            {
                int val = Convert.ToInt32(e.CellValue.ToString());

                if (val <= 30 && val>15)
                {
                    e.Cell.BackColor = System.Drawing.Color.Yellow;
                    e.Cell.ForeColor = System.Drawing.Color.Black;
                }
                else if (val <= 15)
                {
                    e.Cell.BackColor = System.Drawing.Color.LightGreen;
                    e.Cell.ForeColor = System.Drawing.Color.Black;
                }
                else if (val > 30)
                {
                    e.Cell.BackColor = System.Drawing.Color.Red;
                    e.Cell.ForeColor = System.Drawing.Color.White;

                }
            }

            if (e.DataColumn.FieldName == "WorkQueueDepthAvg")
            {
                int val = Convert.ToInt32(e.CellValue.ToString());

                if (val >= 30)
                {
                    e.Cell.BackColor = System.Drawing.Color.Red;
                    e.Cell.ForeColor = System.Drawing.Color.White;
                }
                else if (val >= 15)
                {
                    e.Cell.BackColor = System.Drawing.Color.Yellow;
                    e.Cell.ForeColor = System.Drawing.Color.Black;
                }
                else if (val < 15)
                {
                    e.Cell.BackColor = System.Drawing.Color.LightGreen;
                    e.Cell.ForeColor = System.Drawing.Color.Black;

                }
            }
            if (e.DataColumn.FieldName == "WorkQueueDepthMax")
            {
                int val = Convert.ToInt32(e.CellValue.ToString());

                if (val >= 30)
                {
                    e.Cell.BackColor = System.Drawing.Color.Red;
                    e.Cell.ForeColor = System.Drawing.Color.White;
                }
                else if (val >= 15)
                {
                    e.Cell.BackColor = System.Drawing.Color.Yellow;
                    e.Cell.ForeColor = System.Drawing.Color.Black;
                }
                else if (val < 15)
                {
                    e.Cell.BackColor = System.Drawing.Color.LightGreen;
                    e.Cell.ForeColor = System.Drawing.Color.Black;

                }
            }
            int val1=0;
            if (e.DataColumn.FieldName == "Availability")
            {
                val1 = Convert.ToInt32(e.CellValue.ToString());

                if (val1 < 50 )
                {
                    e.Cell.BackColor = System.Drawing.Color.Yellow;
                    e.Cell.ForeColor = System.Drawing.Color.Black;
                }
                else if (val1 < 25)
                {
                    e.Cell.BackColor = System.Drawing.Color.Red;
                    e.Cell.ForeColor = System.Drawing.Color.White;

                }
                else if (val1 > 50)
                {
                    e.Cell.BackColor = System.Drawing.Color.LightGreen;
                    e.Cell.ForeColor = System.Drawing.Color.Black;

                }
            }
           
            if (e.DataColumn.FieldName == "AvailabilityThreshold")
            {
              int val = Convert.ToInt32(e.CellValue.ToString());

                if (val == 0)
                {
                    e.Cell.BackColor = System.Drawing.Color.LightGray;
                    e.Cell.ForeColor = System.Drawing.Color.Black;
                }
                else if (val > val1)
                {
                    e.Cell.BackColor = System.Drawing.Color.LightGreen;
                    e.Cell.ForeColor = System.Drawing.Color.Black;

                }
                else if (val < val1)
                {
                    e.Cell.BackColor = System.Drawing.Color.Red;
                    e.Cell.ForeColor = System.Drawing.Color.White;

                }
            }
            
            

            
            
        }

        protected void ASPxCallbackPanel1_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
        {
            int index = ClusterHealthGrid.FocusedRowIndex;
            string Type = ClusterHealthGrid.GetRowValues(index, "ServerName").ToString();
            Session["Type"] = Type;
            //11/25/2014 NS modified
            /*
            if (txtfromdate.Text != "" && txttodate.Text != "")
            {
                fromdate = txtfromdate.Text + " " + "00:00:00.000";
                todate = txttodate.Text + " " + "23:59:59.000";

                if (Convert.ToDateTime(txtfromdate.Text) > Convert.ToDateTime(txttodate.Text))
                {
                    MsgPopupControl.ShowOnPageLoad = true;
                    ErrmsgLabel.Text = "From Date value should be less than To Date.";
                }

                else if (Session["Type"].ToString() != "" && Session["Type"] != null)
                {
                    SetGraphforCluster(Session["Type"].ToString(), fromdate, todate);
                }


            }
             */
            if (Session["Type"].ToString() != "" && Session["Type"] != null)
            {
                SetGraphforCluster(Session["Type"].ToString(), fromdate, todate);
            }
            //else
            //{
                
            //    fromdate = txtfromdate.Text + " " + "00:00:00.000";
            //    todate = txttodate.Text + " " + "23:59:59.000";
            //    if (Session["Type"].ToString() != "" && Session["Type"] != null)
            //    {
            //        SetGraphforCluster(Session["Type"].ToString(), fromdate, todate);
            //    }
            //}
        }
        
        public void SetGraphforCluster(string Name,string fromdate,string todate)
        {
            WebChartControl1.Series.Clear();
             DataTable dtClusterHealth = VSWebBL.DashboardBL.ClusterHealthBL.Ins.Getgraphdata(Name, fromdate, todate);
             if (dtClusterHealth.Rows.Count > 0)
             {
                 WebChartControl1.SeriesDataMember = "ServerName";
                 WebChartControl1.SeriesTemplate.ArgumentDataMember = "Date";
                 WebChartControl1.SeriesTemplate.ValueDataMembers.AddRange(new string[] { "StatValue" });
                 WebChartControl1.SeriesTemplate.View = new SideBySideBarSeriesView();
                 WebChartControl1.SeriesTemplate.ChangeView(ViewType.Line);
                 LineSeriesView view = (LineSeriesView)WebChartControl1.SeriesTemplate.View;
                 // WebChartControl1.LoadingPanelText = Name;
                 ChartTitle ct = new ChartTitle();
                 ct.Text = Name;
                 if (WebChartControl1.Titles.Count > 0)
                 {
                     WebChartControl1.Titles.Clear();
                 }
                 //10/9/2013 NS modified
                 WebChartControl1.Titles.Add(ct);
                 /*
                  Series series = null;
                  series = new Series("Cluster Server", ViewType.Line);
                  series.Visible = true;
                  series.DataSource = dtClusterHealth;
                  series.ArgumentDataMember = dtClusterHealth.Columns["Date"].ToString();

                  ValueDataMemberCollection seriesValueDataMembers = (ValueDataMemberCollection)series.ValueDataMembers;
                  seriesValueDataMembers.AddRange(dtClusterHealth.Columns["StatValue"].ToString());
                  WebChartControl1.Series.Add(series);
                 */
                 //((XYDiagram)WebChartControl1.Diagram).PaneLayoutDirection = PaneLayoutDirection.Horizontal;

                 XYDiagram seriesXY = (XYDiagram)WebChartControl1.Diagram;
                 seriesXY.PaneLayoutDirection = PaneLayoutDirection.Horizontal;
                 seriesXY.AxisY.Title.Text = "Replica.Cluster.SecondsOnQueue";
                 seriesXY.AxisY.Title.Visible = true;
                 seriesXY.AxisX.Title.Text = "Date/Time";
                 seriesXY.AxisX.Title.Visible = true;
                 seriesXY.AxisX.Label.ResolveOverlappingOptions.AllowRotate = false;
                 WebChartControl1.Legend.Visible = false;

                 //((LineSeriesView)series.View).AxisX = 100;
                 //((SplineSeriesView)series.View).LineMarkerOptions.Size = 4;
                 //((SplineSeriesView)series.View).LineMarkerOptions.Color = Color.White;

                 view.LineMarkerOptions.Size = 4;
                 view.LineMarkerOptions.Color = Color.White;
                 view.Color = Color.Blue;
                 //view.MarkerVisibility = DefaultBoolean.False;

                 AxisBase axis = ((XYDiagram)WebChartControl1.Diagram).AxisX;
                 //4/18/2014 NS commented out for VSPLUS-312
                 //axis.DateTimeGridAlignment = DateTimeMeasurementUnit.Hour;
                 axis.DateTimeMeasureUnit = DateTimeMeasurementUnit.Minute;
                 axis.DateTimeOptions.Format = DateTimeFormat.General;
                 //10/9/2013 NS modified
                 axis.GridSpacingAuto = true;
                 //axis.MinorCount = 15;
                 //axis.GridSpacing = 50;
                 axis.Range.SideMarginsEnabled = false;
                 axis.GridLines.Visible = false;
                 //axis.DateTimeOptions.Format = DateTimeFormat.Custom;
                 //axis.DateTimeOptions.FormatString = "HH:mm";

                 AxisBase axisy = ((XYDiagram)WebChartControl1.Diagram).AxisY;
                 axisy.Range.AlwaysShowZeroLevel = false;
                 axisy.Range.SideMarginsEnabled = true;
                 axisy.GridLines.Visible = true;
                 //10/9/2013 NS uncommented lines below
                 WebChartControl1.DataSource = dtClusterHealth;
                 WebChartControl1.DataBind();
             }
            //DevExpress.Web.ASPxWebControl.RedirectOnCallback("DominoServerDetailsPage2.aspx?Name=" + Name + "");

                    
        }

        protected void OKButton_Click(object sender, EventArgs e)
        {
            MsgPopupControl.ShowOnPageLoad = false;
        }

        protected void GoButton_Click(object sender, EventArgs e)
        {
            //11/25/2014 NS modified
            /*
            if (Session["Type"] != "" && Session["Type"].ToString() != null && txtfromdate.Text != "" && txttodate.Text != "")
            {
                if (Convert.ToDateTime(txtfromdate.Text) > Convert.ToDateTime(txttodate.Text))
                {
                    MsgPopupControl.ShowOnPageLoad = true;
                    ErrmsgLabel.Text = "From Date value should be less than To Date.";
                }
                else
                {
                    fromdate = txtfromdate.Text + " " + "00:00:00.000";
                    todate = txttodate.Text + " " + "23:59:59.000";

                    SetGraphforCluster(Session["Type"].ToString(), fromdate, todate);
                }
            }
             */
            if (Session["Type"] != "" && Session["Type"].ToString() != null)
            {
                fromdate = dtPick.FromDate;
                todate = dtPick.ToDate;

                SetGraphforCluster(Session["Type"].ToString(), fromdate, todate);
            }
        }

        protected void ClusterHealthGrid_HtmlRowCreated(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
        {
            e.Row.Attributes.Add("onmouseover", "this.originalcolor=this.style.backgroundColor;" + "this.style.backgroundColor='#C0C0C0';");

            e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalcolor;");
        }

        protected void WebChartControl1_CustomCallback(object sender, DevExpress.XtraCharts.Web.CustomCallbackEventArgs e)
        {
            //12/16/2013 NS uncommented the line below
		WebChartControl1.Width = new Unit(Convert.ToInt32(chartWidth.Value));
			//WebChartControl1.Width = new Unit(Convert.ToInt32(chartWidth.Value));
            //int index = ClusterHealthGrid.FocusedRowIndex;
            //string Type = ClusterHealthGrid.GetRowValues(index, "ServerName").ToString();
            //Session["Type"] = Type;
            //if (txtfromdate.Text != "" && txttodate.Text != "")
            //{
            //    fromdate = txtfromdate.Text + " " + "00:00:00.000";
            //    todate = txttodate.Text + " " + "23:59:59.000";

            //    if (Convert.ToDateTime(txtfromdate.Text) > Convert.ToDateTime(txttodate.Text))
            //    {
            //        MsgPopupControl.ShowOnPageLoad = true;
            //        ErrmsgLabel.Text = "From Date Should be less than To Date";
            //    }

            //    else if (Session["Type"].ToString() != "" && Session["Type"] != null)
            //    {
            //        SetGraphforCluster(Session["Type"].ToString(), fromdate, todate);
            //    }


            //}
        }

        protected void ClusterHealthGrid_PageSizeChanged(object sender, EventArgs e)
        {
            //ProfilesGridView.PageIndex;
            VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("ClusterHealth|ClusterHealthGrid", ClusterHealthGrid.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        }
    }       
}

