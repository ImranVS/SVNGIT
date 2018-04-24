using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DevExpress.XtraCharts;
using System.Drawing;

namespace VSWebUI.Dashboard
{
    public partial class QuickrHealth : System.Web.UI.Page
    {
		protected void Page_PreInit(object sender, EventArgs e)
		{

			this.Master.contentCallEvent += new EventHandler(Master_ButtonClick);
		}

		private void Master_ButtonClick(object sender, EventArgs e)
		{

		}
        static string value = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["QuickrServersGridData"] != null)
                {
                }
                else
                {
                    Session["QuickrServersGridData"] = SetQuickrServersGrid("Domino");
                }
                if (Session["UserPreferences"] != null)
                {
                    DataTable UserPreferences = (DataTable)Session["UserPreferences"];
                    foreach (DataRow dr in UserPreferences.Rows)
                    {
                        if (dr[1].ToString() == "QuickrHealth|QuickrServersGrid")
                        {
                            QuickrServersGrid.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                        }
                        if (dr[1].ToString() == "QuickrHealth|QuickrPlacesGrid")
                        {
                            QuickrPlacesGrid.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                        }
                    }
                }

            }
            QuickrServersGrid.DataSource = Session["QuickrServersGridData"];
            if (!IsPostBack && !IsCallback)
            {
                //QuickrServersGrid.DataSource = Session["QuickrServersGridData"];
                QuickrServersGrid.DataBind();
            }
            SettQuickrServersGridFromSession();
            Session["BackURL"] = "QuickrHealth.aspx";
            //10/11/2013 NS added
            int index = QuickrServersGrid.FocusedRowIndex;
            if (index > -1)
            {
                value = QuickrServersGrid.GetRowValues(index, "Name").ToString();
                SetGraphForResponseTime("hh", value);
                SetGraphForHttpSessions("hh", value);
                SetGraphForCPU("hh", value);
                SetGraphForMemory("hh", value);
                //10/28/2013 NS commented out per AF's request
                //SetQuickrPlacesGrid(value);
            }
        }

        public DataTable SetQuickrServersGrid(string serverType)
        {
            DataTable dt = new DataTable();
            try
            {
                dt = VSWebBL.DashboardBL.QuickrHealthBLL.Ins.SetQuickrServersGrid(serverType);
            }
            catch (Exception ex)
            {
                Response.Write("Error :" + ex);
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
            return dt;
        }

        public void SettQuickrServersGridFromSession()
        {
            if (Session["QuickrServersGridData"] != null)
            {
                QuickrServersGrid.DataSource = (DataTable)Session["QuickrServersGridData"];
                QuickrServersGrid.DataBind();
            }
        }

        public void SetGraphForResponseTime(string paramVal, string serverName)
        {
            WebChartResponseTime.Series.Clear();

            DataTable dt = VSWebBL.DashboardBL.QuickrHealthBLL.Ins.SetGraphForResponseTime(paramVal, serverName);

            if (dt.Rows.Count <= 0)
            {
            }
            else
            {
                Series series = new Series("ResponseTime", ViewType.Line);

                series.Visible = true;

                series.ArgumentDataMember = dt.Columns["Date"].ToString();

                ValueDataMemberCollection seriesValueDataMembers = (ValueDataMemberCollection)series.ValueDataMembers;
                seriesValueDataMembers.AddRange(dt.Columns["StatValue"].ToString());

                WebChartResponseTime.Series.Add(series);

                XYDiagram seriesXY = (XYDiagram)WebChartResponseTime.Diagram;
                seriesXY.AxisX.Title.Text = "Time";
                seriesXY.AxisX.Title.Visible = true;
                seriesXY.AxisY.Title.Text = "Utilization";
                seriesXY.AxisY.Title.Visible = true;

                ((LineSeriesView)series.View).LineMarkerOptions.Size = 8;
                ((LineSeriesView)series.View).LineMarkerOptions.Color = Color.White;

                AxisBase axisx = ((XYDiagram)WebChartResponseTime.Diagram).AxisX;
                //axisx.DateTimeGridAlignment = DateTimeMeasurementUnit.Day;
                axisx.GridSpacingAuto = false;
                axisx.MinorCount = 15;
                axisx.GridSpacing = 1;
                //axisx.DateTimeOptions.Format = DateTimeFormat.Custom;
                //axisx.DateTimeOptions.FormatString = "dd/MM HH:mm";
                axisx.Range.SideMarginsEnabled = true;
                axisx.GridLines.Visible = false;

                AxisBase axisy = ((XYDiagram)WebChartResponseTime.Diagram).AxisY;
                axisy.Range.AlwaysShowZeroLevel = false;
                axisy.Range.SideMarginsEnabled = true;
                axisy.GridLines.Visible = true;

                ((LineSeriesView)series.View).Color = Color.Blue;

                WebChartResponseTime.Legend.Visible = false;

                WebChartResponseTime.DataSource = dt;
                WebChartResponseTime.DataBind();
            }
        }

        public void SetGraphForHttpSessions(string paramValue, string serverName)
        {
            httpSessionsWebChart.Series.Clear();
            DataTable dt = VSWebBL.DashboardBL.QuickrHealthBLL.Ins.SetGraphForHttpSessions(paramValue, serverName);

            if (dt.Rows.Count <= 0)
            {
            }
            else
            {
                Series series = new Series("HttpSessions", ViewType.Line);

                series.Visible = true;

                series.ArgumentDataMember = dt.Columns["Date"].ToString();

                ValueDataMemberCollection seriesValueDataMembers = (ValueDataMemberCollection)series.ValueDataMembers;
                seriesValueDataMembers.AddRange(dt.Columns["StatValue"].ToString());

                httpSessionsWebChart.Series.Add(series);

                XYDiagram seriesXY = (XYDiagram)httpSessionsWebChart.Diagram;
                seriesXY.AxisX.Title.Text = "Time";
                seriesXY.AxisX.Title.Visible = true;
                seriesXY.AxisY.Title.Text = "Value";
                seriesXY.AxisY.Title.Visible = true;

                //1/9/2013 NS commented out constant line for Quickr server since there is no HTTP_MaxConfiguredConnections
                //value anywhere in the Quickr table
                /*
                ConstantLine constantLine1 = new ConstantLine("Constant Line 1");
                seriesXY.AxisY.ConstantLines.Add(constantLine1);
                constantLine1.AxisValue = dt.Rows[0][2].ToString();
                constantLine1.Visible = true;
                constantLine1.ShowBehind = false;
                constantLine1.Title.Visible = true;
                constantLine1.Title.Text = "Threshold : " + dt.Rows[0][2].ToString();
                constantLine1.Title.TextColor = Color.Red;
                constantLine1.Title.Antialiasing = false;
                constantLine1.Title.Font = new Font("Tahoma", 10, FontStyle.Regular);
                constantLine1.Title.ShowBelowLine = false;
                constantLine1.Title.Alignment = ConstantLineTitleAlignment.Near;
                constantLine1.Color = Color.Red;
                constantLine1.LineStyle.DashStyle = DashStyle.Solid;
                 */
                //constantLine1.LineStyle.Thickness = 2;

                ((LineSeriesView)series.View).LineMarkerOptions.Size = 8;
                ((LineSeriesView)series.View).LineMarkerOptions.Color = Color.White;

                AxisBase axisx = ((XYDiagram)httpSessionsWebChart.Diagram).AxisX;
                //axisx.DateTimeGridAlignment = DateTimeMeasurementUnit.Day;
                axisx.GridSpacingAuto = false;
                axisx.MinorCount = 15;
                axisx.GridSpacing = 1;
                //axisx.DateTimeOptions.Format = DateTimeFormat.Custom;
                //axisx.DateTimeOptions.FormatString = "dd/MM HH:mm";
                if (paramValue == "hh")
                {
                    //4/18/2014 NS commented out for VSPLUS-312
                    //axisx.DateTimeGridAlignment = DateTimeMeasurementUnit.Hour;
                    axisx.DateTimeMeasureUnit = DateTimeMeasurementUnit.Minute;
                    axisx.DateTimeOptions.Format = DateTimeFormat.ShortTime;
                }
                else
                {
                    //4/18/2014 NS commented out for VSPLUS-312
                    //axisx.DateTimeGridAlignment = DateTimeMeasurementUnit.Day;
                    axisx.DateTimeOptions.Format = DateTimeFormat.ShortDate;
                }
                axisx.Range.SideMarginsEnabled = true;
                axisx.GridLines.Visible = false;

                AxisBase axisy = ((XYDiagram)httpSessionsWebChart.Diagram).AxisY;
                axisy.Range.AlwaysShowZeroLevel = false;
                axisy.Range.SideMarginsEnabled = true;
                axisy.GridLines.Visible = true;
                ((LineSeriesView)series.View).Color = Color.Blue;

                httpSessionsWebChart.Legend.Visible = false;

                httpSessionsWebChart.DataSource = dt;
                httpSessionsWebChart.DataBind();
            }
        }

        public void SetGraphForCPU(string paramVal, string serverName)
        {
            webChartCPU.Series.Clear();

            DataTable dt = VSWebBL.DashboardBL.QuickrHealthBLL.Ins.SetGraphForCPU(paramVal, serverName);

            if (dt.Rows.Count <= 0)
            {
            }
            else
            {
                Series series = new Series("CPU", ViewType.Line);

                series.Visible = true;

                series.ArgumentDataMember = dt.Columns["Date"].ToString();

                ValueDataMemberCollection seriesValueDataMembers = (ValueDataMemberCollection)series.ValueDataMembers;
                seriesValueDataMembers.AddRange(dt.Columns["StatValue"].ToString());

                webChartCPU.Series.Add(series);

                XYDiagram seriesXY = (XYDiagram)webChartCPU.Diagram;
                seriesXY.AxisX.Title.Text = "Time";
                seriesXY.AxisX.Title.Visible = true;
                seriesXY.AxisY.Title.Text = "Milliseconds";
                seriesXY.AxisY.Title.Visible = true;

                ((LineSeriesView)series.View).LineMarkerOptions.Size = 8;
                ((LineSeriesView)series.View).LineMarkerOptions.Color = Color.White;

                AxisBase axisx = ((XYDiagram)webChartCPU.Diagram).AxisX;
                //axisx.DateTimeGridAlignment = DateTimeMeasurementUnit.Day;
                axisx.GridSpacingAuto = false;
                axisx.MinorCount = 15;
                axisx.GridSpacing = 1;
                //axisx.DateTimeOptions.Format = DateTimeFormat.Custom;
                //axisx.DateTimeOptions.FormatString = "HH:mm";
                axisx.Range.SideMarginsEnabled = true;
                axisx.GridLines.Visible =false;

                AxisBase axisy = ((XYDiagram)webChartCPU.Diagram).AxisY;
                axisy.Range.AlwaysShowZeroLevel = false;
                axisy.Range.SideMarginsEnabled = true;
                axisy.GridLines.Visible = true;
                ((LineSeriesView)series.View).Color = Color.Blue;

                webChartCPU.Legend.Visible = false;

                webChartCPU.DataSource = dt;
                webChartCPU.DataBind();
            }
        }

        public void SetGraphForMemory(string paramVal, string serverName)
        {
            webChartMemory.Series.Clear();

            DataTable dt = VSWebBL.DashboardBL.QuickrHealthBLL.Ins.SetGraphForMemory(paramVal, serverName);

            if (dt.Rows.Count <= 0)
            {
            }
            else
            {
                Series series = new Series("Memory", ViewType.Line);

                series.Visible = true;

                series.ArgumentDataMember = dt.Columns["Date"].ToString();

                ValueDataMemberCollection seriesValueDataMembers = (ValueDataMemberCollection)series.ValueDataMembers;
                seriesValueDataMembers.AddRange(dt.Columns["StatValue"].ToString());

                webChartMemory.Series.Add(series);

                XYDiagram seriesXY = (XYDiagram)webChartMemory.Diagram;
                seriesXY.AxisX.Title.Text = "Time";
                seriesXY.AxisX.Title.Visible = true;
                seriesXY.AxisY.Title.Text = "Memory Used";
                seriesXY.AxisY.Title.Visible = true;

                ((LineSeriesView)series.View).LineMarkerOptions.Size = 8;
                ((LineSeriesView)series.View).LineMarkerOptions.Color = Color.White;

                AxisBase axisx = ((XYDiagram)webChartMemory.Diagram).AxisX;
                //axisx.DateTimeGridAlignment = DateTimeMeasurementUnit.Day;
                axisx.GridSpacingAuto = false;
                axisx.MinorCount = 15;
                axisx.GridSpacing = 1;
                //4/18/2014 NS commented out for VSPLUS-312
                //axisx.DateTimeGridAlignment = DateTimeMeasurementUnit.Hour;
                axisx.DateTimeMeasureUnit = DateTimeMeasurementUnit.Minute;
                axisx.DateTimeOptions.Format = DateTimeFormat.ShortTime;
                //axisx.DateTimeOptions.Format = DateTimeFormat.Custom;
                //axisx.DateTimeOptions.FormatString = "dd/MM HH:mm";
                axisx.Range.SideMarginsEnabled = true;
                axisx.GridLines.Visible = false;

                AxisBase axisy = ((XYDiagram)webChartMemory.Diagram).AxisY;
                axisy.Range.AlwaysShowZeroLevel = false;
                axisy.Range.SideMarginsEnabled = true;
                axisy.GridLines.Visible = true;
                ((LineSeriesView)series.View).Color = Color.Blue;

                webChartMemory.Legend.Visible = false;

                webChartMemory.DataSource = dt;
                webChartMemory.DataBind();
            }
        }

        public void SetQuickrPlacesGrid(string serverName)
        {
            DataTable dt = VSWebBL.DashboardBL.QuickrHealthBLL.Ins.SetQuickrPlacesGrid(serverName);
            QuickrPlacesGrid.DataSource = dt;

            QuickrPlacesGrid.DataBind();
            Session["QuickrPlacesData"] = dt;
        }

        //protected void bttnSubmit_Click(object sender, EventArgs e)
        //{
        //    if (selList.Items.Count > 0)
        //    {
        //        string[] str = new string[selList.Items.Count];
        //        for (int i = 0; i < selList.Items.Count; i++)
        //        {
        //            str[i] = selList.Items[i].ToString();
        //        }

        //        string serverType = null;

        //        for (int i = 0; i < str.Count(); i++)
        //        {
        //            serverType += str[i];
        //        }

        //        if (serverType != null)
        //        {
        //            serverName = serverType;
        //            SetGraphForResponseTime("hh", serverName);
        //            SetGraphForHttpSessions("hh", serverName);
        //            SetGraphForCPU("hh", serverName);
        //            SetGraphForMemory("hh", serverName);
        //            SetQuickrPlacesGrid(serverName);
        //        }
        //    }
        //    else
        //    {
        //        //Response.Write("Atleast select one server.");
        //    }
        //}

        protected void httpSessionsASPxRadioButtonList_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetGraphForHttpSessions(httpSessionsASPxRadioButtonList.Value.ToString(), value);
        }

        protected void ASPxCallbackPanel2_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
        {
            int index = QuickrServersGrid.FocusedRowIndex;
            //7/8/2013 NS modified - if grid is empty, an error is thrown "Object variable not set"
            if (index > -1)
            {
                value = QuickrServersGrid.GetRowValues(index, "Name").ToString();
                SetGraphForResponseTime("hh", value);
                SetGraphForHttpSessions("hh", value);
                SetGraphForCPU("hh", value);
                SetGraphForMemory("hh", value);
                //10/28/2013 NS commented out per AF's request
                //SetQuickrPlacesGrid(value);
            }
        }

        protected void QuickrPlacesGrid_HtmlRowCreated(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
        {
            e.Row.Attributes.Add("onmouseover", "this.originalcolor=this.style.backgroundColor;" + "this.style.backgroundColor='#C0C0C0';");

            e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalcolor;");

        }

        protected void QuickrServersGrid_HtmlRowCreated(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
        {
            int index = QuickrServersGrid.FocusedRowIndex;
            if (e.VisibleIndex != index)
            {
                e.Row.Attributes.Add("onmouseover", "this.originalcolor=this.style.backgroundColor;" + "this.style.backgroundColor='#C0C0C0';");

                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalcolor;");

            }
        }

        protected void QuickrServersGrid_PageSizeChanged(object sender, EventArgs e)
        {
            //ProfilesGridView.PageIndex;
            VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("QuickrHealth|QuickrServersGrid", QuickrServersGrid.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        }
        protected void QuickrPlacesGrid_PageSizeChanged(object sender, EventArgs e)
        {
            //ProfilesGridView.PageIndex;
            VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("QuickrHealth|QuickrPlacesGrid", QuickrPlacesGrid.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        }
    }
}