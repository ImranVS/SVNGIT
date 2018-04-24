using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using VSWebBL;
using DevExpress.XtraCharts;
using System.Drawing;
using System.Web.UI.HtmlControls;
using System.Reflection;
//using DevExpress.Web;


namespace VSWebUI
{
    public partial class MailHealth : System.Web.UI.Page
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
            //2/2/2015 NS added for
            DataTable dt = new DataTable();
            bool flag1 = false;
            bool flag2 = false;
            dt = VSWebBL.SecurityBL.MenusBL.Ins.GetSelectedFeatures();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["Name"].ToString() == "Domino")
                    {
                        flag1 = true; 
                    }
                    else if (dt.Rows[i]["Name"].ToString() == "Exchange")
                    {
                        flag2 = true; 
                    }
                }
            }
            if (!flag1)
            {
                ASPxPageControl1.TabPages[0].Visible = false;
            }
            if (!flag2)
            {
                ASPxPageControl1.TabPages[1].Visible = false;
            }
            if (!IsPostBack && !IsCallback)
            {
                HtmlGenericControl body = (HtmlGenericControl)Master.FindControl("Body1");
                body.Attributes.Add("onload", "DoCallback()");
                body.Attributes.Add("onResize", "Resized()");
            }
            if (!IsPostBack)
            {                           
                FillMailServiceGrid();
                FillNotesMailProbeGrid();
                FillDominoServersList();
                //2/29/2015 NS added for VSPLUS-1358
                FillExchangeServersList();
                if (Session["UserPreferences"] != null)
                {
                    DataTable UserPreferences = (DataTable)Session["UserPreferences"];
                    foreach (DataRow dr in UserPreferences.Rows)
                    {
                        if (dr[1].ToString() == "MailHealth|MailGridView")
                        {
                            MailGridView.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                        }
                        if (dr[1].ToString() == "MailHealth|NotesMailProbeGridView")
                        {
                            NotesMailProbeGridView.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                        }
                    }
                }
            }
            else
            {
                FillMailServiceGridfromSesson();
                FillNotesMailProbeGridfromSession();               
            }
            Session["BackURL"] = "MailHealth.aspx";
        }

        public void FillDominoServersList()
        {
            DataTable DominoTab = new DataTable();
            try
            {
                DominoTab = VSWebBL.DashboardBL.MailHealthBL.Ins.GetDominoServersForMailHealth();
                ServerListComboBox.DataSource = DominoTab;
                ServerListComboBox.ValueField = "Name";
                ServerListComboBox.TextField = "Name";
                ServerListComboBox.DataBind();
            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
        }
        public void FillMailServiceGrid()
        {
            DataTable MailTab = new DataTable();
            try
            {
                MailTab = VSWebBL.DashboardBL.MailHealthBL.Ins.GetMailServiceData();
                Session["MailTab"] = MailTab;
                //8/9/2013 NS added
                if (MailTab.Rows.Count == 0)
                {
                    //11/10/2014 NS modified
                    //ASPxRoundPanel1.Visible = false;

                    //MailGridView.Visible = false;
                    //mailServicesDiv.Style.Value = "display: none";
                    mailservicesMainDiv.Style.Value = "display: none";
                }
                else
                {
                    //11/10/2014 NS modified
                    //ASPxRoundPanel1.Visible = true;

                    //MailGridView.Visible = true;
                    //mailServicesDiv.Style.Value = "display: block";
                    mailservicesMainDiv.Style.Value = "display: block";
                    MailGridView.DataSource = MailTab;
                    MailGridView.DataBind();
                }
            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
          
        }

        public void FillNotesMailProbeGrid()
        {
            DataTable NotesMailTab = new DataTable();
            try
            {
                NotesMailTab = VSWebBL.DashboardBL.MailHealthBL.Ins.GetNotesMailProbeData();
                Session["NotesMailTab"] = NotesMailTab;
                //8/9/2013 NS added
                if (NotesMailTab.Rows.Count == 0)
                {
                    //11/10/2014 NS modified
                    //MailProbeRoundPanel.Visible = false;

                    //NotesMailProbeGridView.Visible = false;
                    //notesmailprobeDiv.Style.Value = "display: none";
                    notesmailprobeMainDiv.Style.Value = "display: none";
                }
                else
                {
                    //11/10/2014 NS modified
                    //MailProbeRoundPanel.Visible = true;

                    //NotesMailProbeGridView.Visible = true;
                    //notesmailprobeDiv.Style.Value = "display: block";
                    notesmailprobeMainDiv.Style.Value = "display: block";
                    NotesMailProbeGridView.DataSource = NotesMailTab;
                    NotesMailProbeGridView.DataBind();
                }   
            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
        }


        public void FillMailServiceGridfromSesson()
        {
            DataTable MailTab = new DataTable();
            try
            {
                if (Session["MailTab"] != "" && Session["MailTab"] != null)
                {
                    MailTab = Session["MailTab"] as DataTable;
                    MailGridView.DataSource = MailTab;
                    MailGridView.DataBind();
                }
            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
        }

        public void FillNotesMailProbeGridfromSession()
        {
            DataTable NotesMailTab = new DataTable();
            try
            {
                NotesMailTab = Session["NotesMailTab"] as DataTable;
               
                NotesMailProbeGridView.DataSource = NotesMailTab;
                NotesMailProbeGridView.DataBind();
            }           
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
        }        

      

        protected void MailGridView_HtmlDataCellPrepared(object sender, DevExpress.Web.ASPxGridViewTableDataCellEventArgs e)
        {

            if (e.DataColumn.FieldName == "Status" && (e.CellValue.ToString() == "OK" || e.CellValue.ToString() == "Scanning"))
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
            else if (e.DataColumn.FieldName == "Status" && e.CellValue.ToString() == "disabled")
            {
                e.Cell.BackColor = System.Drawing.Color.Gray;
                // e.Cell.ForeColor = System.Drawing.Color.White;
            }
            else if (e.DataColumn.FieldName == "Status")
            {
                e.Cell.BackColor = System.Drawing.Color.Yellow;
                // e.DataColumn.GroupFooterCellStyle.ForeColor = System.Drawing.Color.Yellow;

            }
        }

        protected void NotesMailProbeGridView_HtmlDataCellPrepared(object sender, DevExpress.Web.ASPxGridViewTableDataCellEventArgs e)
        {
            if (e.DataColumn.FieldName == "Status" && (e.CellValue.ToString() == "OK" || e.CellValue.ToString() == "Scanning"))
            {
                e.Cell.BackColor = System.Drawing.Color.LightGreen;
            }

            else if (e.DataColumn.FieldName == "Status" && (e.CellValue.ToString() == "Not Responding" ||e.CellValue.ToString() == "Error" ))
            {
                e.Cell.BackColor = System.Drawing.Color.Red;
                e.Cell.ForeColor = System.Drawing.Color.White;
            }
            else if (e.DataColumn.FieldName == "Status" && e.CellValue.ToString() == "Not Scanned")
            {
                e.Cell.BackColor = System.Drawing.Color.LightBlue;
                e.Cell.ForeColor = System.Drawing.Color.White;
            }
            else if (e.DataColumn.FieldName == "Status" && e.CellValue.ToString() == "disabled")
            {
                e.Cell.BackColor = System.Drawing.Color.Gray;
                // e.Cell.ForeColor = System.Drawing.Color.White;
            }
            else if (e.DataColumn.FieldName == "Status")
            {
                e.Cell.BackColor = System.Drawing.Color.Yellow;
                // e.DataColumn.GroupFooterCellStyle.ForeColor = System.Drawing.Color.Yellow;
            }
        }

    

        // code for Charts

        public void SetGraphforMailDelivered(string DeviceName)
        {
            MailDeliveredWebChartControl.Series.Clear();
            DataTable dt = VSWebBL.DashboardBL.MailHealthBL.Ins.SetGraphForMailDelivered(DeviceName);

            Series series = null;
            series = new Series("MailDelivered", ViewType.Line);
            series.Visible = true;
            series.DataSource = dt;
            series.ArgumentDataMember = dt.Columns["Date"].ToString();

            ValueDataMemberCollection seriesValueDataMembers = (ValueDataMemberCollection)series.ValueDataMembers;
            seriesValueDataMembers.AddRange(dt.Columns["StatValue"].ToString());
            MailDeliveredWebChartControl.Series.Add(series);

            ((XYDiagram)MailDeliveredWebChartControl.Diagram).PaneLayoutDirection = PaneLayoutDirection.Horizontal;

            XYDiagram seriesXY = (XYDiagram)MailDeliveredWebChartControl.Diagram;
            seriesXY.AxisX.Title.Text = "Time";
            seriesXY.AxisX.Title.Visible = true;
            seriesXY.AxisY.Title.Text = "Count";
            seriesXY.AxisY.Title.Visible = true;

            MailDeliveredWebChartControl.Legend.Visible = false;

            //((LineSeriesView)series.View).LineTensionPercent = 100;
            ((LineSeriesView)series.View).LineMarkerOptions.Size = 8;
            ((LineSeriesView)series.View).LineMarkerOptions.Color = Color.White;

            AxisBase axis = ((XYDiagram)MailDeliveredWebChartControl.Diagram).AxisX;
            //axis.DateTimeGridAlignment = DateTimeMeasurementUnit.Day;
            axis.GridSpacingAuto = true;
            //axis.MinorCount = 15;
            //axis.GridSpacing = 1;
            axis.Range.SideMarginsEnabled = false;
            axis.GridLines.Visible = false;
            //4/18/2014 NS commented out for VSPLUS-312
            //axis.DateTimeGridAlignment = DateTimeMeasurementUnit.Hour;
            axis.DateTimeMeasureUnit = DateTimeMeasurementUnit.Minute;
            axis.DateTimeOptions.Format = DateTimeFormat.ShortTime;
            //5axis.DateTimeOptions.FormatString = "dd/MM HH:mm";
            //((LineSeriesView)series.View).Color = Color.Blue;

            AxisBase axisy = ((XYDiagram)MailDeliveredWebChartControl.Diagram).AxisY;
            axisy.Range.AlwaysShowZeroLevel = false;
            axisy.Range.SideMarginsEnabled = true;
            axisy.GridLines.Visible = true;
        }

        public void SetGraphforMailTransffered(string DeviceName)
        {
           MailTranfferedWebChartControl.Series.Clear();
           DataTable dt = VSWebBL.DashboardBL.MailHealthBL.Ins.SetGraphForMailTransffered(DeviceName);

            Series series = null;
            series = new Series("DominoServer", ViewType.Line);
            series.Visible = true;
            //series.DataSource = dt;
            series.ArgumentDataMember = dt.Columns["Date"].ToString();

            ValueDataMemberCollection seriesValueDataMembers = (ValueDataMemberCollection)series.ValueDataMembers;
            seriesValueDataMembers.AddRange(dt.Columns["StatValue"].ToString());
            MailTranfferedWebChartControl.Series.Add(series);

            ((XYDiagram)MailTranfferedWebChartControl.Diagram).PaneLayoutDirection = PaneLayoutDirection.Horizontal;

            XYDiagram seriesXY = (XYDiagram)MailTranfferedWebChartControl.Diagram;
            seriesXY.AxisX.Title.Text = "Time";
            seriesXY.AxisX.Title.Visible = true;
            seriesXY.AxisY.Title.Text = "Count";
            seriesXY.AxisY.Title.Visible = true;

            MailTranfferedWebChartControl.Legend.Visible = false;

            //((SplineSeriesView)series.View).LineTensionPercent = 100;
            ((LineSeriesView)series.View).LineMarkerOptions.Size = 8;
            ((LineSeriesView)series.View).LineMarkerOptions.Color = Color.White;

            AxisBase axis = ((XYDiagram)MailTranfferedWebChartControl.Diagram).AxisX;
            //axis.DateTimeGridAlignment = DateTimeMeasurementUnit.Day;
            axis.GridSpacingAuto = true;
            //axis.MinorCount = 15;
            //axis.GridSpacing = 1;
            axis.Range.SideMarginsEnabled = false;
            axis.GridLines.Visible = false;
            //4/18/2014 NS commented out for VSPLUS-312
            //axis.DateTimeGridAlignment = DateTimeMeasurementUnit.Hour;
            axis.DateTimeMeasureUnit = DateTimeMeasurementUnit.Minute;
            axis.DateTimeOptions.Format = DateTimeFormat.ShortTime;
            //axis.DateTimeOptions.FormatString = "dd/MM HH:mm";
            //((LineSeriesView)series.View).Color = Color.Blue;
            //12/17/2013 NS commented out
            /*
            double min = Convert.ToDouble(((XYDiagram)MailTranfferedWebChartControl.Diagram).AxisX.Range.MinValue);
            double max = Convert.ToDouble(((XYDiagram)MailTranfferedWebChartControl.Diagram).AxisX.Range.MaxValue);

            int gs = (int)((max - min) / 5);

            if (gs == 0)
            {
                gs = 1;
                seriesXY.AxisX.GridSpacingAuto = false;
                seriesXY.AxisX.GridSpacing = gs;
            }
             */

            AxisBase axisy = ((XYDiagram)MailTranfferedWebChartControl.Diagram).AxisY;
            axisy.Range.AlwaysShowZeroLevel = false;
            axisy.Range.SideMarginsEnabled = true;
            axisy.GridLines.Visible = true;

            MailTranfferedWebChartControl.DataSource = dt;
            MailTranfferedWebChartControl.DataBind();
        }

        public void SetGraphforMailRouted(string DeviceName)
        {
            MailRoutedWebChartControl.Series.Clear();
            DataTable dt = VSWebBL.DashboardBL.MailHealthBL.Ins.SetGraphForMailRouted(DeviceName);

            Series series = null;
            series = new Series("DominoServer", ViewType.Line);
            series.Visible = true;
            series.ArgumentDataMember = dt.Columns["Date"].ToString();

            ValueDataMemberCollection seriesValueDataMembers = (ValueDataMemberCollection)series.ValueDataMembers;
            seriesValueDataMembers.AddRange(dt.Columns["StatValue"].ToString());
            MailRoutedWebChartControl.Series.Add(series);

            ((XYDiagram)MailRoutedWebChartControl.Diagram).PaneLayoutDirection = PaneLayoutDirection.Horizontal;

            XYDiagram seriesXY = (XYDiagram)MailRoutedWebChartControl.Diagram;
            seriesXY.AxisX.Title.Text = "Time";
            seriesXY.AxisX.Title.Visible = true;
            seriesXY.AxisY.Title.Text = "Count";
            seriesXY.AxisY.Title.Visible = true;

            MailRoutedWebChartControl.Legend.Visible = false;

            //((SplineSeriesView)series.View).LineTensionPercent = 100;
            ((LineSeriesView)series.View).LineMarkerOptions.Size = 8;
            ((LineSeriesView)series.View).LineMarkerOptions.Color = Color.White;

            AxisBase axis = ((XYDiagram)MailRoutedWebChartControl.Diagram).AxisX;
            //axis.DateTimeGridAlignment = DateTimeMeasurementUnit.Day;
            axis.GridSpacingAuto = true;
            //axis.MinorCount = 15;
            //axis.GridSpacing = 1;
            axis.Range.SideMarginsEnabled = false;
            axis.GridLines.Visible = false;
            //4/18/2014 NS commented out for VSPLUS-312
            //axis.DateTimeGridAlignment = DateTimeMeasurementUnit.Hour;
            axis.DateTimeMeasureUnit = DateTimeMeasurementUnit.Minute;
            axis.DateTimeOptions.Format = DateTimeFormat.ShortTime;
            //axis.DateTimeOptions.FormatString = "dd/MM HH:mm";
            //((LineSeriesView)series.View).Color = Color.Blue;

            AxisBase axisy = ((XYDiagram)MailRoutedWebChartControl.Diagram).AxisY;
            axisy.Range.AlwaysShowZeroLevel = false;
            axisy.Range.SideMarginsEnabled = true;
            axisy.GridLines.Visible = true;
            MailRoutedWebChartControl.DataSource = dt;
            MailRoutedWebChartControl.DataBind();           
        }

        public void SetGraphForMailTraffic(string serverName)
        {
            MailTraficWebChartControl.Series.Clear();
            DataTable dt = VSWebBL.DashboardBL.DashboardBL.Ins.GetMailStatus(serverName);

            Series series = new Series("Mail", ViewType.Bar);

            series.ArgumentDataMember = dt.Columns["Mail"].ToString();

            ValueDataMemberCollection seriesValueDataMembers = (ValueDataMemberCollection)series.ValueDataMembers;
            seriesValueDataMembers.AddRange(dt.Columns["Value"].ToString());
            MailTraficWebChartControl.Series.Add(series);

            MailTraficWebChartControl.Legend.Visible = false;

            ((SideBySideBarSeriesView)series.View).ColorEach = true;

            //AxisBase axisx = ((XYDiagram)deviceTypeWebChart.Diagram).AxisX;

            XYDiagram seriesXY = (XYDiagram)MailTraficWebChartControl.Diagram;
            seriesXY.AxisX.Title.Text = "Mail Status";
            seriesXY.AxisX.Title.Visible = true;
            seriesXY.AxisX.Label.ResolveOverlappingOptions.AllowRotate = false;
            seriesXY.AxisY.Title.Text = "Count";
            seriesXY.AxisY.Title.Visible = true;

            MailTraficWebChartControl.DataSource = dt;
            MailTraficWebChartControl.DataBind();

            double min = Convert.ToDouble(((XYDiagram)MailTraficWebChartControl.Diagram).AxisY.Range.MinValue);
            double max = Convert.ToDouble(((XYDiagram)MailTraficWebChartControl.Diagram).AxisY.Range.MaxValue);

            int gs = (int)((max - min) / 5);

            if (gs == 0)
            {
                gs = 1;
                seriesXY.AxisY.GridSpacingAuto = false;
                seriesXY.AxisY.GridSpacing = gs;
            }
            else
            {
                seriesXY.AxisY.GridSpacingAuto = true;
                seriesXY.AxisY.Tickmarks.MinorVisible = false;
                seriesXY.AxisY.GridLines.MinorVisible = false;
            }

            AxisBase axisy = ((XYDiagram)MailTraficWebChartControl.Diagram).AxisY;
            axisy.Range.AlwaysShowZeroLevel = false;
            axisy.GridLines.Visible = true;
        }

        protected void ASPxCallbackPanel1_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
        {
            int index = MailGridView.FocusedRowIndex;
            if (index >= 0)
            {
                value = MailGridView.GetRowValues(index, "Name").ToString();
                SetGraphForMailTraffic(value);
                SetGraphforMailDelivered(value);
                SetGraphforMailTransffered(value);
                SetGraphforMailRouted(value);
                //2/13/2015 NS added for VSPLUS-1358
                SetGraphForMailDeliverySuccess(value);

            }
        }

        protected void NotesMailProbeGridView_HtmlRowCreated(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
        {
            e.Row.Attributes.Add("onmouseover", "this.originalcolor=this.style.backgroundColor;" + "this.style.backgroundColor='#C0C0C0';");

            e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalcolor;");
        }

        protected void MailGridView_HtmlRowCreated(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
        {
            int index = MailGridView.FocusedRowIndex;
            if (e.VisibleIndex != index)
            {
                e.Row.Attributes.Add("onmouseover", "this.originalcolor=this.style.backgroundColor;" + "this.style.backgroundColor='#C0C0C0';");

                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalcolor;");
            }
        }

        protected void ServerListComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string value = ServerListComboBox.SelectedItem.Value.ToString();
            SetGraphForMailTraffic(value);
            SetGraphforMailDelivered(value);
            SetGraphforMailTransffered(value);
            SetGraphforMailRouted(value);
            MailTraficWebChartControl.Width = new Unit(Convert.ToInt32(chartWidth.Value));
            MailDeliveredWebChartControl.Width = new Unit(Convert.ToInt32(chartWidth.Value));
            MailTranfferedWebChartControl.Width = new Unit(Convert.ToInt32(chartWidth.Value));
            MailRoutedWebChartControl.Width = new Unit(Convert.ToInt32(chartWidth.Value));
        }

        protected void MailGridView_PageSizeChanged(object sender, EventArgs e)
        {
            //ProfilesGridView.PageIndex;
            VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("MailHealth|MailGridView", MailGridView.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        }
        protected void NotesMailProbeGridView_PageSizeChanged(object sender, EventArgs e)
        {
            //ProfilesGridView.PageIndex;
            VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("MailHealth|NotesMailProbeGridView", NotesMailProbeGridView.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        }

        protected void MailTraficWebChartControl_CustomCallback(object sender, DevExpress.XtraCharts.Web.CustomCallbackEventArgs e)
        {
            MailTraficWebChartControl.Width = new Unit(Convert.ToInt32(chartWidth.Value));
        }

        protected void MailDeliveredWebChartControl_CustomCallback(object sender, DevExpress.XtraCharts.Web.CustomCallbackEventArgs e)
        {
            MailDeliveredWebChartControl.Width = new Unit(Convert.ToInt32(chartWidth.Value));
        }

        protected void MailTranfferedWebChartControl_CustomCallback(object sender, DevExpress.XtraCharts.Web.CustomCallbackEventArgs e)
        {
            MailTranfferedWebChartControl.Width = new Unit(Convert.ToInt32(chartWidth.Value));
        }

        protected void MailRoutedWebChartControl_CustomCallback(object sender, DevExpress.XtraCharts.Web.CustomCallbackEventArgs e)
        {
            MailRoutedWebChartControl.Width = new Unit(Convert.ToInt32(chartWidth.Value));
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (!this.DesignMode)
            {
                this.UpdatePanel1.Unload += new EventHandler(UpdatePanel_Unload);
                this.UpdatePanel2.Unload += new EventHandler(UpdatePanel_Unload);
                this.UpdatePanel3.Unload += new EventHandler(UpdatePanel_Unload);
                this.UpdatePanel4.Unload += new EventHandler(UpdatePanel_Unload);
            }
        }

        protected void UpdatePanel_Unload(object sender, EventArgs e)
        {
            RegisterUpdatePanel((UpdatePanel)sender);
        }
        protected void RegisterUpdatePanel(UpdatePanel panel)
        {
            var sType = typeof(ScriptManager);
            var mInfo = sType.GetMethod("System.Web.UI.IScriptManagerInternal.RegisterUpdatePanel", BindingFlags.NonPublic | BindingFlags.Instance);
            if (mInfo != null)
                mInfo.Invoke(ScriptManager.GetCurrent(Page), new object[] { panel });
        }

        public void SetGraphForMailQueues(string ServerName)
        {
            QueueWebChart.Series.Clear();

            DataTable dt1 = VSWebBL.DashboardBL.ExchangeServerDetailsBL.Ins.SetMailGraph(ServerName);
            DataTable dt2 = dt1.DefaultView.ToTable(true, "SubCategory");


			if (dt2.Rows.Count > 0)
			{
				for (int i = 0; i < dt2.Rows.Count; i++)
				{
					DataTable finaldt = new DataTable();
					finaldt = dt1.Clone();
					//string str = dt2.Rows[i][0].ToString();
					DataRow[] dr = dt1.Select("SubCategory = '" + dt2.Rows[i][0].ToString() + "'");
					for (int c = 0; c < dr.Length; c++)
					{
						finaldt.NewRow();
						finaldt.ImportRow(dr[c]);
					}

					Series series = new Series(dt2.Rows[i][0].ToString(), ViewType.Line);

					series.DataSource = finaldt;

					series.ArgumentDataMember = dt1.Columns["Date"].ToString();

					QueueWebChart.SeriesTemplate.ArgumentScaleType = ScaleType.DateTime;

					ValueDataMemberCollection seriesValueDataMembers = (ValueDataMemberCollection)series.ValueDataMembers;
					seriesValueDataMembers.AddRange(dt1.Columns["StatValue"].ToString());

					QueueWebChart.Series.Add(series);

					XYDiagram seriesXY = (XYDiagram)QueueWebChart.Diagram;
					seriesXY.AxisX.Title.Text = "Time";
					seriesXY.AxisX.Title.Visible = true;
					seriesXY.AxisY.Title.Text = "Count";
					seriesXY.AxisY.Title.Visible = true;

					((LineSeriesView)series.View).LineMarkerOptions.Size = 8;
					((LineSeriesView)series.View).LineMarkerOptions.Color = Color.White;

					AxisBase axis = ((XYDiagram)QueueWebChart.Diagram).AxisX;
					//4/18/2014 NS commented out for VSPLUS-312
					//axis.DateTimeGridAlignment = DateTimeMeasurementUnit.Hour;
					axis.GridSpacing = 1;
					axis.Range.SideMarginsEnabled = false;

					AxisBase axisy = ((XYDiagram)QueueWebChart.Diagram).AxisY;
					axisy.Range.AlwaysShowZeroLevel = false;
					axisy.Range.SideMarginsEnabled = true;
					axisy.GridLines.Visible = true;
					double min = Convert.ToDouble(((XYDiagram)QueueWebChart.Diagram).AxisY.Range.MinValue);
					double max = Convert.ToDouble(((XYDiagram)QueueWebChart.Diagram).AxisY.Range.MaxValue);

					int gs = (int)((max - min) / 5);

					if (gs == 0)
					{
						gs = 1;
						seriesXY.AxisY.GridSpacingAuto = false;
						seriesXY.AxisY.GridSpacing = gs;
					}
					else
					{
						seriesXY.AxisY.GridSpacingAuto = true;
					}
				}
			}
			else {

				DataTable finaldt = new DataTable();
				finaldt = dt1.Clone();
				Series series = new Series("Series", ViewType.Line);

				series.DataSource = finaldt;

				series.ArgumentDataMember = "Date";

				QueueWebChart.SeriesTemplate.ArgumentScaleType = ScaleType.DateTime;

				ValueDataMemberCollection seriesValueDataMembers = (ValueDataMemberCollection)series.ValueDataMembers;
				seriesValueDataMembers.AddRange(dt1.Columns["StatValue"].ToString());

				QueueWebChart.Series.Add(series);

				XYDiagram seriesXY = (XYDiagram)QueueWebChart.Diagram;
				seriesXY.AxisX.Title.Text = "Time";
				seriesXY.AxisX.Title.Visible = true;
				seriesXY.AxisY.Title.Text = "Count";
				seriesXY.AxisY.Title.Visible = true;

				((LineSeriesView)series.View).LineMarkerOptions.Size = 8;
				((LineSeriesView)series.View).LineMarkerOptions.Color = Color.White;

				AxisBase axis = ((XYDiagram)QueueWebChart.Diagram).AxisX;
				
				axis.GridSpacing = 1;
				axis.Range.SideMarginsEnabled = false;

				AxisBase axisy = ((XYDiagram)QueueWebChart.Diagram).AxisY;
				axisy.Range.AlwaysShowZeroLevel = false;
				axisy.Range.SideMarginsEnabled = true;
				axisy.GridLines.Visible = true;
				double min = Convert.ToDouble(((XYDiagram)QueueWebChart.Diagram).AxisY.Range.MinValue);
				double max = Convert.ToDouble(((XYDiagram)QueueWebChart.Diagram).AxisY.Range.MaxValue);

				int gs = (int)((max - min) / 5);

				if (gs == 0)
				{
					gs = 1;
					seriesXY.AxisY.GridSpacingAuto = false;
					seriesXY.AxisY.GridSpacing = gs;
				}
				else
				{
					seriesXY.AxisY.GridSpacingAuto = true;
				}
			
			}
			
			//myText.Antialiasing = true;
			//myText.Text = "There are no visible series to represent in the chart.";
			//myText.TextColor = Color.Black;
        }

        //2/13/2015 NS added for VSPLUS-1358
        public void SetGraphForMailDeliverySuccess(string ServerName)
        {
            DeliveryRateWebChart.Series.Clear();

            DataTable dt = VSWebBL.DashboardBL.ExchangeServerDetailsBL.Ins.SetMailDeliveryRateGraph(ServerName);
            Series series = null;
            series = new Series("MailDelivered", ViewType.Line);
            series.Visible = true;
            series.DataSource = dt;
            series.ArgumentDataMember = dt.Columns["Date"].ToString();

            ValueDataMemberCollection seriesValueDataMembers = (ValueDataMemberCollection)series.ValueDataMembers;
            seriesValueDataMembers.AddRange(dt.Columns["StatValue"].ToString());
            DeliveryRateWebChart.Series.Add(series);

            ((XYDiagram)DeliveryRateWebChart.Diagram).PaneLayoutDirection = PaneLayoutDirection.Horizontal;

            DeliveryRateWebChart.SeriesTemplate.ArgumentScaleType = ScaleType.DateTime;
            XYDiagram seriesXY = (XYDiagram)DeliveryRateWebChart.Diagram;
            seriesXY.AxisX.Title.Text = "Time";
            seriesXY.AxisX.Title.Visible = true;
            seriesXY.AxisY.Title.Text = "Delivery Rate";
            seriesXY.AxisY.Title.Visible = true;

            ((LineSeriesView)series.View).LineMarkerOptions.Size = 8;
            ((LineSeriesView)series.View).LineMarkerOptions.Color = Color.White;

            AxisBase axis = ((XYDiagram)DeliveryRateWebChart.Diagram).AxisX;
            //4/18/2014 NS commented out for VSPLUS-312
            //axis.DateTimeGridAlignment = DateTimeMeasurementUnit.Hour;
            axis.GridSpacing = 1;
            axis.Range.SideMarginsEnabled = false;

            AxisBase axisy = ((XYDiagram)DeliveryRateWebChart.Diagram).AxisY;
            axisy.Range.AlwaysShowZeroLevel = false;
            axisy.Range.SideMarginsEnabled = true;
            axisy.GridLines.Visible = true;
            double min = Convert.ToDouble(((XYDiagram)DeliveryRateWebChart.Diagram).AxisY.Range.MinValue);
            double max = Convert.ToDouble(((XYDiagram)DeliveryRateWebChart.Diagram).AxisY.Range.MaxValue);

            int gs = (int)((max - min) / 5);

            if (gs == 0)
            {
                gs = 1;
                seriesXY.AxisY.GridSpacingAuto = false;
                seriesXY.AxisY.GridSpacing = gs;
            }
            else
            {
                seriesXY.AxisY.GridSpacingAuto = true;
            }
        }

        //2/13/2015 NS added for VSPLUS-1358
        //4/8/2016 Sowjanya modified for VSPLUS-2766
        public void SetGraphForMailSize(string ServerName)
        {
            DataTable dt = VSWebBL.DashboardBL.ExchangeServerDetailsBL.Ins.SetMailSizeGraph(ServerName);
            MailSizeWebChart.DataSource = dt;
            MailSizeWebChart.Series[0].DataSource = dt;
            MailSizeWebChart.Series[0].ArgumentDataMember = dt.Columns["Date"].ToString();
            MailSizeWebChart.Series[0].ValueDataMembers.AddRange(dt.Columns["Mail_ReceivedSizeMB"].ToString());
            MailSizeWebChart.Series[0].Name = "Received";
            MailSizeWebChart.Series[0].Visible = true;
            MailSizeWebChart.Series[1].DataSource = dt;
            MailSizeWebChart.Series[1].ArgumentDataMember = dt.Columns["Date"].ToString();
            MailSizeWebChart.Series[1].ValueDataMembers.AddRange(dt.Columns["Mail_SentSizeMB"].ToString());
            MailSizeWebChart.Series[1].Name = "Sent";
            MailSizeWebChart.Series[1].Visible = true;
            
        }

        //2/13/2015 NS added for VSPLUS-1358
        //4/8/2016 Sowjanya modified for VSPLUS-2766
        public void SetGraphForMailCount(string ServerName)
        {
            DataTable dt = VSWebBL.DashboardBL.ExchangeServerDetailsBL.Ins.SetMailCountGraph(ServerName);
            MailCountWebChart.DataSource = dt;
            MailCountWebChart.Series[0].DataSource = dt;
            MailCountWebChart.Series[0].ArgumentDataMember = dt.Columns["Date"].ToString();
            MailCountWebChart.Series[0].ValueDataMembers.AddRange(dt.Columns["Mail_ReceivedCount"].ToString());
            MailCountWebChart.Series[0].Name = "Received";
            MailCountWebChart.Series[0].Visible = true;
            MailCountWebChart.Series[1].DataSource = dt;
            MailCountWebChart.Series[1].ArgumentDataMember = dt.Columns["Date"].ToString();
            MailCountWebChart.Series[1].ValueDataMembers.AddRange(dt.Columns["Mail_SentCount"].ToString());
            MailCountWebChart.Series[1].Name = "Sent";
            MailCountWebChart.Series[1].Visible = true;
        }

        protected void ServerListExchangeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string value = ServerListExchangeComboBox.SelectedItem.Value.ToString();
            SetGraphForMailQueues(value);
            //2/13/2015 NS added for VSPLUS-1358
            SetGraphForMailDeliverySuccess(value);
            SetGraphForMailSize(value);
            SetGraphForMailCount(value);
            QueueWebChart.Width = new Unit(Convert.ToInt32(chartWidth.Value));
            DeliveryRateWebChart.Width = new Unit(Convert.ToInt32(chartWidth.Value));
            MailSizeWebChart.Width = new Unit(Convert.ToInt32(chartWidth.Value));
            MailCountWebChart.Width = new Unit(Convert.ToInt32(chartWidth.Value));
        }

        protected void QueueWebChart_CustomCallback(object sender, DevExpress.XtraCharts.Web.CustomCallbackEventArgs e)
        {
            QueueWebChart.Width = new Unit(Convert.ToInt32(chartWidth.Value));
        }

        public void FillExchangeServersList()
        {
            DataTable dt = new DataTable();
            try
            {
                dt = VSWebBL.DashboardBL.ExchangeServerDetailsBL.Ins.GetExchangeServersList();
                ServerListExchangeComboBox.DataSource = dt;
                ServerListExchangeComboBox.ValueField = "ServerName";
                ServerListExchangeComboBox.TextField = "ServerName";
                ServerListExchangeComboBox.DataBind();
            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
        }

        protected void DeliveryRateWebChart_CustomCallback(object sender, DevExpress.XtraCharts.Web.CustomCallbackEventArgs e)
        {
            DeliveryRateWebChart.Width = new Unit(Convert.ToInt32(chartWidth.Value));
        }

        protected void MailSizeWebChart_CustomCallback(object sender, DevExpress.XtraCharts.Web.CustomCallbackEventArgs e)
        {
            MailSizeWebChart.Width = new Unit(Convert.ToInt32(chartWidth.Value));
        }

        protected void MailCountWebChart_CustomCallback(object sender, DevExpress.XtraCharts.Web.CustomCallbackEventArgs e)
        {
            MailCountWebChart.Width = new Unit(Convert.ToInt32(chartWidth.Value));
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
        //            serverTypes = serverType;
        //            //ViewState["CheckedListValues"] = serverTypes;
        //            SetGraphForMailTraffic(serverTypes);
        //            SetGraphforMailDelivered(serverTypes);
        //            SetGraphforMailTransffered(serverTypes);
        //            SetGraphforMailRouted(serverTypes);
        //        }
        //    }
        //    else
        //    {
        //        //Response.Write("Atleast select one server.");
        //    }
        //}

		
    }
}