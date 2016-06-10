using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DevExpress.XtraCharts;
using System.Drawing;
using System.Web.UI.HtmlControls;

namespace VSWebUI.Dashboard
{
    public partial class SametimeHealth : System.Web.UI.Page
    {
		protected void Page_PreInit(object sender, EventArgs e)
		{

			this.Master.contentCallEvent += new EventHandler(Master_ButtonClick);
		}

		private void Master_ButtonClick(object sender, EventArgs e)
		{

		}
        protected void Page_Load(object sender, EventArgs e)
        {
            //1/29/2013 NS commented out
            //ActiveChartWebChartControl.Width = new Unit(Convert.ToInt32(chartWidth.Value));
            //ActiveMeetingWebChartControl.Width = new Unit(Convert.ToInt32(chartWidth.Value));
            //ActiveNwayWebChartControl.Width = new Unit(Convert.ToInt32(chartWidth.Value));
			//ChatnWayChatSessionsWebChartControl.Width = new Unit(Convert.ToInt32(chartWidth.Value));
            if (!IsPostBack)
            {
				
				
                DomionSametimegrid.FocusedRowIndex = -1;
                //12/11/2013 NS
                //Sametimegrid.FocusedRowIndex = -1;
                FillDominoSametimegrid();
                FillSametimeServergrid();
                if (Session["UserPreferences"] != null)
				{ 
//SetResponseTimeGraph(value);
                    DataTable UserPreferences = (DataTable)Session["UserPreferences"];
                    foreach (DataRow dr in UserPreferences.Rows)
                    {
                        if (dr[1].ToString() == "SametimeHealth|DomionSametimegrid")
                        {
                            DomionSametimegrid.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                        }
                        if (dr[1].ToString() == "SametimeHealth|Sametimegrid")
                        {
                            Sametimegrid.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                        }
                    }
                }
				
					//10/31/2013 NS modified - in order to enable grid filtering, callback needs to be enabled too.
					//Since grid is reloaded from session, no row has focus and index returns -1
					
				
            }
            else
            {
                FillDominoSametimeGridfromsession();
                FillSametimeserverGridfromsession();
            }
			if (Sametimegrid.VisibleRowCount > 0)
				{
					int index = Sametimegrid.FocusedRowIndex;
			if (index > -1)
			{
				string value = Sametimegrid.GetRowValues(index, "Name").ToString();
				Session["Type"] = value;
				SetActiveChartGraph(value);
				ASPxRoundPanel2.HeaderText = "Users - "+value;
				//SetActiveMeetingGraph(value);
				//SetGraphForChatnWayChatSessionsWebChartControl(value);
				//ASPxRoundPanel3.HeaderText = "Chat Sessions/n-Way Chat Sessions" + " - " + Session["Type"].ToString();
				//SetActiveNWayGraph(value);
				//ASPxRoundPanel4.HeaderText = "n-Way Chat Sessions" + " - " + Session["Type"].ToString(); 
				//12/12/2013 NS added
				SetResponseTimeGraph(value);
				ASPxRoundPanel5.HeaderText = " Response Times" + " - " + Session["Type"].ToString();
				//SetCountOfAllActiveUsers(value);
				//ASPxRoundPanel1.HeaderText = "All ActiveUsers" + " - " + Session["Type"].ToString();
				SetNumberofnwaychats(value);
				ASPxRoundPanel4.HeaderText = "Number of n-way chats" + " - " + Session["Type"].ToString();
				SetNumberofchatmessages(value);
				ASPxRoundPanel6.HeaderText = "Number of chat messages" + " - " + Session["Type"].ToString();
				SetNumberofopenchatsessions(value);
				ASPxRoundPanel7.HeaderText = "Number of open chat sessions" + " - " + Session["Type"].ToString();
				SetNumberofactivenwaychats1(value);
				ASPxRoundPanel8.HeaderText = "Number of active n-way chats" + " - " + Session["Type"].ToString();
				SetTotalcountofall1x1calls(value);
				ASPxRoundPanel9.HeaderText = "Total count of all 1x1 calls" + " - " + Session["Type"].ToString();
				SetTotalcountofallcalls(value);
				ASPxRoundPanel10.HeaderText = "Total count of all calls" + " - " + Session["Type"].ToString();
				SetTotalcountofallmultiusercalls(value);
				ASPxRoundPanel11.HeaderText = "Total count of all multi-user calls" + " - " + Session["Type"].ToString();
				SetCountofallcallsandusers(value);
				ASPxRoundPanel12.HeaderText = "Count of all calls/Count of all users" + " - " + Session["Type"].ToString();
				SetCountofall1x1callsandusers(value);
				ASPxRoundPanel13.HeaderText = "Count of all 1x1 calls/Count of all 1x1 call users" + " - " + Session["Type"].ToString();
				Setcountofallmultiusercallsandusers(value);
				ASPxRoundPanel14.HeaderText = "Count of all multi-user calls/Count of all multi-user call users" + " - " + Session["Type"].ToString();
				SetNumberofactivemeetingsandusersinsidemeetings(value);
				ASPxRoundPanel15.HeaderText = "Number of active meetings/Number of users in meetings" + " - " + Session["Type"].ToString();
			}
				}
        }
        public void FillDominoSametimegrid()
        {
            DataTable dtdominserverdata = new DataTable();
            dtdominserverdata = VSWebBL.DashboardBL.SametimeHealthBL.Ins.GetDominosametimeData();
            DomionSametimegrid.DataSource = dtdominserverdata;
            DomionSametimegrid.DataBind();
            Session["dominosametimegrid"] = dtdominserverdata;
        }
        public void FillDominoSametimeGridfromsession()
        {
            if (Session["dominosametimegrid"] != "" && Session["dominosametimegrid"] != null)
            {
                DomionSametimegrid.DataSource = (DataTable)Session["dominosametimegrid"];
                DomionSametimegrid.DataBind();
            }

        }
        public void FillSametimeServergrid()
        {
            DataTable dtSametimeserverdata = new DataTable();
            dtSametimeserverdata = VSWebBL.DashboardBL.SametimeHealthBL.Ins.GetSametimeData();
            Sametimegrid.DataSource = dtSametimeserverdata;
            Sametimegrid.DataBind();
            Session["sametimeservergrid"] = dtSametimeserverdata;
        }
        public void FillSametimeserverGridfromsession()
        {
            if (Session["sametimeservergrid"] != "" && Session["sametimeservergrid"] != null)
            {
                Sametimegrid.DataSource = (DataTable)Session["sametimeservergrid"];
                Sametimegrid.DataBind();
            }

        }

        protected void DomionSametimegrid_HtmlDataCellPrepared(object sender, DevExpress.Web.ASPxGridViewTableDataCellEventArgs e)
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

                e.Cell.BackColor = System.Drawing.Color.LightBlue;
                e.Cell.ForeColor = System.Drawing.Color.Black;
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

        protected void Sametimegrid_HtmlDataCellPrepared(object sender, DevExpress.Web.ASPxGridViewTableDataCellEventArgs e)
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

                e.Cell.BackColor = System.Drawing.Color.LightBlue;
                e.Cell.ForeColor = System.Drawing.Color.Black;
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

        protected void ASPxCallbackPanel1_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
        {
            if (IsPostBack)
            {                
                int index = DomionSametimegrid.FocusedRowIndex;
                int index1 = Sametimegrid.FocusedRowIndex;
                 string Type="";
                string Type1="";
                if(index>-1)
                 Type = DomionSametimegrid.GetRowValues(index, "Name").ToString();
                if(index1>-1)
                Type1 = Sametimegrid.GetRowValues(index1, "Name").ToString();
                //if (index == index1)
                //{
                //    if (Type != "")
                //    {
                //        string Name = Type;
                //        Session["Type"] = Type;

                //        SetActiveChartGraph(Name);
                //        SetActiveMeetingGraph(Name);
                //        SetActiveNWayGraph(Name);
                        
                //    }
                //}
                //else
                //{
                    //if (index != 0)
                    //{
                        if (Type != "")
                        {
                            string Name = Type;
                            Session["Type"] = Type;

                            SetActiveChartGraph(Name);
							//SetCountOfAllActiveUsers(Name);
							SetNumberofnwaychats(Name);
							SetNumberofchatmessages(Name);
							SetNumberofopenchatsessions(Name);
							SetNumberofactivenwaychats1(Name);
							SetTotalcountofall1x1calls(Name);
							SetTotalcountofallcalls(Name);
							SetTotalcountofallmultiusercalls(Name);
							//SetGraphForChatnWayChatSessionsWebChartControl(Name);
							SetCountofallcallsandusers(Name);
							SetCountofall1x1callsandusers(Name);
							Setcountofallmultiusercallsandusers(Name);
							SetNumberofactivemeetingsandusersinsidemeetings(Name);
							//SetActiveMeetingGraph(Name);
							
							//SetActiveNWayGraph(Name);
                        }
                       
                    //}                 
                                                 
                    //else if(index != 0)
                    //{
                        else if (Type1 != "")
                        {
                        string Name = Type1;
                        Session["Type"] = Type1;

                        SetActiveChartGraph(Name);
						//SetCountOfAllActiveUsers(Name);
						SetNumberofnwaychats(Name);
						SetNumberofchatmessages(Name);
						SetNumberofopenchatsessions(Name);
						SetNumberofactivenwaychats1(Name);
						SetTotalcountofall1x1calls(Name);
						SetTotalcountofallcalls(Name);
						SetTotalcountofallmultiusercalls(Name);
						//SetGraphForChatnWayChatSessionsWebChartControl(Name);
						SetCountofallcallsandusers(Name);
						SetCountofall1x1callsandusers(Name);
						Setcountofallmultiusercallsandusers(Name);
						SetNumberofactivemeetingsandusersinsidemeetings(Name);

						//SetActiveMeetingGraph(Name);
						//SetActiveNWayGraph(Name);
                        }
                    //}
              //  }             
            }
        }

        protected void Sametimegrid_HtmlRowCreated(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
        {
          e.Row.Attributes.Add("onmouseover", "this.originalcolor=this.style.backgroundColor;" + "this.style.backgroundColor='#C0C0C0';"); 
               
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalcolor;");

  
        }

        protected void DomionSametimegrid_HtmlRowCreated(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
        {
           e.Row.Attributes.Add("onmouseover", "this.originalcolor=this.style.backgroundColor;" + "this.style.backgroundColor='#C0C0C0';"); 
               
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalcolor;");

  
        }

        public void SetActiveChartGraph(string Server)
        {
            ActiveChartWebChartControl.Series.Clear();
			DataTable dtActiveChat = VSWebBL.DashboardBL.SametimeHealthBL.Ins.GetActivechatGraph(Server);

           
                //Series series = null;
                
                Series series = new Series("Users", ViewType.Line);
                series.Visible = true;
                series.DataSource = dtActiveChat;
                series.ArgumentDataMember = dtActiveChat.Columns["Date"].ToString();

                ValueDataMemberCollection seriesValueDataMembers = (ValueDataMemberCollection)series.ValueDataMembers;
                seriesValueDataMembers.AddRange(dtActiveChat.Columns["StatValue"].ToString());
                ActiveChartWebChartControl.Series.Add(series);
				//ASPxRoundPanel2.HeaderText = null;
				//string header = ASPxRoundPanel2.HeaderText;
				//ASPxRoundPanel2.HeaderText = ASPxRoundPanel2.HeaderText + " - " + Server;

				//lblservername.Text = Server;
                //((XYDiagram)ActiveChartWebChartControl.Diagram).PaneLayoutDirection = PaneLayoutDirection.Horizontal;

                XYDiagram seriesXY = (XYDiagram)ActiveChartWebChartControl.Diagram;
                seriesXY.AxisY.Title.Text = "Users";
                seriesXY.AxisY.Title.Visible = true;
                seriesXY.AxisX.Title.Text = "Time";
                seriesXY.AxisX.Title.Visible = true;
               
                ((LineSeriesView)series.View).LineStyle.Thickness = 2;
                ((LineSeriesView)series.View).LineMarkerOptions.Size = 4;
                ((LineSeriesView)series.View).LineMarkerOptions.Color = Color.White;
                ((LineSeriesView)series.View).Color = Color.Blue;

                AxisBase axis = ((XYDiagram)ActiveChartWebChartControl.Diagram).AxisX;
                //axis.GridSpacingAuto = true;
                //4/18/2014 NS commented out for VSPLUS-312
                //axis.DateTimeGridAlignment = DateTimeMeasurementUnit.Minute;
                axis.DateTimeMeasureUnit = DateTimeMeasurementUnit.Minute;
                axis.DateTimeOptions.Format = DateTimeFormat.ShortTime;
                // axis.NumericOptions.Format=DevExpress.XtraCharts.NumericFormat.Number;

                //axis.MinorCount = 15;
                //axis.GridSpacing = 5;
                axis.Range.SideMarginsEnabled = true;
                axis.GridLines.Visible = false;

                //axis.DateTimeOptions.Format = DateTimeFormat.Custom;
                //axis.DateTimeOptions.FormatString = "MM/dd";

                AxisBase axisy = ((XYDiagram)ActiveChartWebChartControl.Diagram).AxisY;
                axisy.Range.AlwaysShowZeroLevel = false;
                axisy.Range.SideMarginsEnabled = true;
                axisy.GridLines.Visible = true;
                
                ActiveChartWebChartControl.Legend.Visible = false;
                ActiveChartWebChartControl.DataSource = dtActiveChat;
                ActiveChartWebChartControl.DataBind();

            //12/11/2015 NS added for VSPLUS-2298
                UI uiobj = new UI();
                uiobj.RecalibrateChartAxes(ActiveChartWebChartControl.Diagram);
        }

		//public void SetActiveMeetingGraph(string Name)
		//{
		//    ActiveMeetingWebChartControl.Series.Clear();
		//    DataTable dtActiveMeeting = VSWebBL.DashboardBL.SametimeHealthBL.Ins.GetActiveMeetingGraph(Name);
           
		//    //string header = ASPxRoundPanel3.HeaderText;
		//    //ASPxRoundPanel3.HeaderText = ASPxRoundPanel3.HeaderText + " - " + Name;
            
		//    lblservername.Text = Name;
		//    Series series = null;
		//    series = new Series("Chat Sessions", ViewType.Line);
		//    series.Visible = true;
		//    series.DataSource = dtActiveMeeting;
		//    series.ArgumentDataMember = dtActiveMeeting.Columns["Date"].ToString();

		//    ValueDataMemberCollection seriesValueDataMembers = (ValueDataMemberCollection)series.ValueDataMembers;
		//    seriesValueDataMembers.AddRange(dtActiveMeeting.Columns["StatValue"].ToString());
		//    ActiveMeetingWebChartControl.Series.Add(series);
		//    //ASPxRoundPanel3.HeaderText = ASPxRoundPanel2.HeaderText + " - " + Name;

		//    lblservername.Text = Name;
		//    ((XYDiagram)ActiveMeetingWebChartControl.Diagram).PaneLayoutDirection = PaneLayoutDirection.Horizontal;

		//    XYDiagram seriesXY = (XYDiagram)ActiveMeetingWebChartControl.Diagram;
		//    seriesXY.AxisY.Title.Text = "Chat Sessions";
		//    seriesXY.AxisY.Title.Visible = true;
		//    seriesXY.AxisX.Title.Text = "Time";
		//    seriesXY.AxisX.Title.Visible = true;
		//    ActiveMeetingWebChartControl.Legend.Visible = false;

		//    //((LineSeriesView)series.View).AxisX = 100;
		//    //((SplineSeriesView)series.View).LineMarkerOptions.Size = 4;
		//    //((SplineSeriesView)series.View).LineMarkerOptions.Color = Color.White;

		//    ((LineSeriesView)series.View).LineStyle.Thickness = 2;
		//    ((LineSeriesView)series.View).LineMarkerOptions.Size = 10;
		//    ((LineSeriesView)series.View).LineMarkerOptions.Color = Color.White;
		//    AxisBase axis = ((XYDiagram)ActiveMeetingWebChartControl.Diagram).AxisX;
		//    axis.GridSpacingAuto = true;
		//    //axis.DateTimeGridAlignment = DateTimeMeasurementUnit.Minute;
		//    //axis.DateTimeMeasureUnit = DateTimeMeasurementUnit.Hour;
		//    // axis.NumericOptions.Format=DevExpress.XtraCharts.NumericFormat.Number;
            
		//    //axis.MinorCount = 15;
		//    //axis.GridSpacing = 5;
		//    axis.Range.SideMarginsEnabled = true;
		//    axis.GridLines.Visible = false;

		//    //axis.DateTimeOptions.Format = DateTimeFormat.Custom;
		//    //axis.DateTimeOptions.FormatString = "MM/dd";

		//    ((LineSeriesView)series.View).Color = Color.Blue;

		//    AxisBase axisy = ((XYDiagram)ActiveMeetingWebChartControl.Diagram).AxisY;
		//    axisy.Range.AlwaysShowZeroLevel = false;
		//    axisy.Range.SideMarginsEnabled = true;
		//    axisy.GridLines.Visible = true;

		//    ActiveMeetingWebChartControl.DataSource = dtActiveMeeting;
		//    ActiveMeetingWebChartControl.DataBind();

		//    double min = Convert.ToDouble(((XYDiagram)ActiveMeetingWebChartControl.Diagram).AxisY.Range.MinValue);
		//    double max = Convert.ToDouble(((XYDiagram)ActiveMeetingWebChartControl.Diagram).AxisY.Range.MaxValue);

		//    int gs = (int)((max - min) / 5);

		//    if (gs == 0)
		//    {
		//        gs = 1;
		//        seriesXY.AxisY.GridSpacingAuto = false;
		//        seriesXY.AxisY.GridSpacing = gs;
		//    }
		//}

		//public void SetActiveNWayGraph(string Name)
		//{
		//    ActiveNwayWebChartControl.Series.Clear();
		//    DataTable dtActivenway = VSWebBL.DashboardBL.SametimeHealthBL.Ins.GetActiveNWayGraph(Name);
			
           
		//    //string header = ASPxRoundPanel4.HeaderText;
		////	ASPxRoundPanel4.HeaderText = ASPxRoundPanel4.HeaderText + " - " + Session["Type"].ToString();
		//    //ASPxRoundPanel4.HeaderText += " - " + lblservername.Text;
		//    //lblservername.Text = Name;
		//    Series series = null;
		//    series = new Series("NWayChats", ViewType.Line);
		//    series.Visible = true;
		//    series.DataSource = dtActivenway;
		//    series.ArgumentDataMember = dtActivenway.Columns["Date"].ToString();

		//    ValueDataMemberCollection seriesValueDataMembers = (ValueDataMemberCollection)series.ValueDataMembers;
		//    seriesValueDataMembers.AddRange(dtActivenway.Columns["StatValue"].ToString());
		//    ActiveNwayWebChartControl.Series.Add(series);

		//    ((XYDiagram)ActiveNwayWebChartControl.Diagram).PaneLayoutDirection = PaneLayoutDirection.Horizontal;

		//    XYDiagram seriesXY = (XYDiagram)ActiveNwayWebChartControl.Diagram;
		//    seriesXY.AxisY.Title.Text = "n-Way Chat Sessions";
		//    seriesXY.AxisY.Title.Visible = true;
		//    seriesXY.AxisX.Title.Text = "Time";
		//    seriesXY.AxisX.Title.Visible = true;
		//    ActiveNwayWebChartControl.Legend.Visible = false;

		//    //((LineSeriesView)series.View).AxisX = 100;
		//    //((SplineSeriesView)series.View).LineMarkerOptions.Size = 4;
		//    //((SplineSeriesView)series.View).LineMarkerOptions.Color = Color.White;

		//    ((LineSeriesView)series.View).LineStyle.Thickness = 2;
		//    ((LineSeriesView)series.View).LineMarkerOptions.Size = 10;
		//    ((LineSeriesView)series.View).LineMarkerOptions.Color = Color.White;
		//    AxisBase axis = ((XYDiagram)ActiveNwayWebChartControl.Diagram).AxisX;
		//    axis.GridSpacingAuto = true;
		//    //axis.DateTimeGridAlignment = DateTimeMeasurementUnit.Minute;
		//    //axis.DateTimeMeasureUnit = DateTimeMeasurementUnit.Hour;
		//    // axis.NumericOptions.Format=DevExpress.XtraCharts.NumericFormat.Number;
            
		//    //axis.MinorCount = 15;
		//    //axis.GridSpacing = 5;
		//    axis.Range.SideMarginsEnabled = true;
		//    axis.GridLines.Visible = false;

		//    //axis.DateTimeOptions.Format = DateTimeFormat.Custom;
		//    //axis.DateTimeOptions.FormatString = "MM/dd";
            
		//    ((LineSeriesView)series.View).Color = Color.Blue;

		//    AxisBase axisy = ((XYDiagram)ActiveNwayWebChartControl.Diagram).AxisY;
		//    axisy.Range.AlwaysShowZeroLevel = false;
		//    axisy.Range.SideMarginsEnabled = true;
		//    axisy.GridLines.Visible = true;

		//    ActiveNwayWebChartControl.DataSource = dtActivenway;
		//    ActiveNwayWebChartControl.DataBind();

		//    double min = Convert.ToDouble(((XYDiagram)ActiveNwayWebChartControl.Diagram).AxisY.Range.MinValue);
		//    double max = Convert.ToDouble(((XYDiagram)ActiveNwayWebChartControl.Diagram).AxisY.Range.MaxValue);

		//    int gs = (int)((max - min) / 5);

		//    if (gs == 0)
		//    {
		//        gs = 1;
		//        seriesXY.AxisY.GridSpacingAuto = false;
		//        seriesXY.AxisY.GridSpacing = gs;
		//    }
		//}

        //12/12/2013 NS added
        public void SetResponseTimeGraph(string Name)
        {
			ResponseTimesWebChartControl.Series.Clear();
            DataTable dt = VSWebBL.DashboardBL.SametimeHealthBL.Ins.GetResponseTimesGraph(Name);
           
			//string header = ASPxRoundPanel5.HeaderText;
		//	ASPxRoundPanel5.HeaderText = ASPxRoundPanel5.HeaderText + " - " + Name;
			//ASPxRoundPanel5.HeaderText = ASPxRoundPanel5.HeaderText + " - " + Name;
			//PxRoundPanel5.HeaderText = Name;
			//lblservername.Text = Name;
            Series series = null;
            series = new Series("ResponseTimes", ViewType.Line);
            series.Visible = true;
            series.DataSource = dt;
            series.ArgumentDataMember = dt.Columns["Date"].ToString();

            ValueDataMemberCollection seriesValueDataMembers = (ValueDataMemberCollection)series.ValueDataMembers;
            seriesValueDataMembers.AddRange(dt.Columns["StatValue"].ToString());
            ResponseTimesWebChartControl.Series.Add(series);

            ((XYDiagram)ResponseTimesWebChartControl.Diagram).PaneLayoutDirection = PaneLayoutDirection.Horizontal;

            XYDiagram seriesXY = (XYDiagram)ResponseTimesWebChartControl.Diagram;
            seriesXY.AxisY.Title.Text = "Response Time";
            seriesXY.AxisY.Title.Visible = true;
            seriesXY.AxisX.Title.Text = "Time";
            seriesXY.AxisX.Title.Visible = true;
            ResponseTimesWebChartControl.Legend.Visible = false;

            //((LineSeriesView)series.View).AxisX = 100;
            //((SplineSeriesView)series.View).LineMarkerOptions.Size = 4;
            //((SplineSeriesView)series.View).LineMarkerOptions.Color = Color.White;

            ((LineSeriesView)series.View).LineStyle.Thickness = 2;
            ((LineSeriesView)series.View).LineMarkerOptions.Size = 4;
            ((LineSeriesView)series.View).LineMarkerOptions.Color = Color.White;
            AxisBase axis = ((XYDiagram)ResponseTimesWebChartControl.Diagram).AxisX;
            //4/18/2014 NS commented out for VSPLUS-312
            //axis.DateTimeGridAlignment = DateTimeMeasurementUnit.Hour;
            axis.DateTimeMeasureUnit = DateTimeMeasurementUnit.Minute;
            axis.DateTimeOptions.Format = DateTimeFormat.ShortTime;
            // axis.NumericOptions.Format=DevExpress.XtraCharts.NumericFormat.Number;

            //axis.MinorCount = 15;
            //axis.GridSpacing = 5;
            axis.Range.SideMarginsEnabled = true;
            axis.GridLines.Visible = false;

            //axis.DateTimeOptions.Format = DateTimeFormat.Custom;
            //axis.DateTimeOptions.FormatString = "MM/dd";

            ((LineSeriesView)series.View).Color = Color.Blue;

            AxisBase axisy = ((XYDiagram)ResponseTimesWebChartControl.Diagram).AxisY;
            axisy.Range.AlwaysShowZeroLevel = false;
            axisy.Range.SideMarginsEnabled = true;
            axisy.GridLines.Visible = true;
            ResponseTimesWebChartControl.DataSource = dt;
            ResponseTimesWebChartControl.DataBind();
            //10/22/2015 NS added for VSPLUS-2298
            UI uiobj = new UI();
            uiobj.RecalibrateChartAxes(ResponseTimesWebChartControl.Diagram);
        }
		//public void SetCountOfAllActiveUsers(string Name)
		//{
		//    CountOfAllActiveUsers.Series.Clear();
		//    DataTable dtActiveUsers = VSWebBL.DashboardBL.SametimeHealthBL.Ins.GetCountOfAllActiveUsers(Name);


		//    //Series series = null;

		//    Series series = new Series("AllActiveUsers", ViewType.Line);
		//    series.Visible = true;
		//    series.DataSource = dtActiveUsers;
		//    series.ArgumentDataMember = dtActiveUsers.Columns["Date"].ToString();

		//    ValueDataMemberCollection seriesValueDataMembers = (ValueDataMemberCollection)series.ValueDataMembers;
		//    seriesValueDataMembers.AddRange(dtActiveUsers.Columns["StatValue"].ToString());
		//    CountOfAllActiveUsers.Series.Add(series);
		//    //ASPxRoundPanel2.HeaderText = null;
		//    //string header = ASPxRoundPanel2.HeaderText;
		//    //ASPxRoundPanel2.HeaderText = ASPxRoundPanel2.HeaderText + " - " + Server;

		//    //lblservername.Text = Server;
		//    ((XYDiagram)ActiveChartWebChartControl.Diagram).PaneLayoutDirection = PaneLayoutDirection.Horizontal;

		//    XYDiagram seriesXY = (XYDiagram)CountOfAllActiveUsers.Diagram;
		//    seriesXY.AxisY.Title.Text = "Count";
		//    seriesXY.AxisY.Title.Visible = true;
		//    seriesXY.AxisX.Title.Text = "Time";
		//    seriesXY.AxisX.Title.Visible = true;

		//    ((LineSeriesView)series.View).LineStyle.Thickness = 2;
		//    ((LineSeriesView)series.View).LineMarkerOptions.Size = 10;
		//    ((LineSeriesView)series.View).LineMarkerOptions.Color = Color.White;
		//    ((LineSeriesView)series.View).Color = Color.Blue;

		//    AxisBase axis = ((XYDiagram)CountOfAllActiveUsers.Diagram).AxisX;
		//    axis.GridSpacingAuto = true;
		//    //4/18/2014 NS commented out for VSPLUS-312
		//    //axis.DateTimeGridAlignment = DateTimeMeasurementUnit.Minute;
		//    axis.DateTimeMeasureUnit = DateTimeMeasurementUnit.Hour;
		//    axis.DateTimeOptions.Format = DateTimeFormat.ShortTime;
		//    // axis.NumericOptions.Format=DevExpress.XtraCharts.NumericFormat.Number;

		//    //axis.MinorCount = 15;
		//    //axis.GridSpacing = 5;
		//    axis.Range.SideMarginsEnabled = true;
		//    axis.GridLines.Visible = false;

		//    //axis.DateTimeOptions.Format = DateTimeFormat.Custom;
		//    //axis.DateTimeOptions.FormatString = "MM/dd";

		//    AxisBase axisy = ((XYDiagram)CountOfAllActiveUsers.Diagram).AxisY;
		//    axisy.Range.AlwaysShowZeroLevel = false;
		//    axisy.Range.SideMarginsEnabled = true;
		//    axisy.GridLines.Visible = true;


		//    double min = Convert.ToDouble(((XYDiagram)CountOfAllActiveUsers.Diagram).AxisY.Range.MinValue);
		//    double max = Convert.ToDouble(((XYDiagram)CountOfAllActiveUsers.Diagram).AxisY.Range.MaxValue);

		//    int gs = (int)((max - min) / 5);

		//    if (gs == 0)
		//    {
		//        gs = 1;
		//        seriesXY.AxisY.GridSpacingAuto = false;
		//        seriesXY.AxisY.GridSpacing = gs;
		//    }
		//    CountOfAllActiveUsers.Legend.Visible = false;
		//    CountOfAllActiveUsers.DataSource = dtActiveUsers;
		//    CountOfAllActiveUsers.DataBind();

		//}
		//public void SetCountOfAllActiveUsers(string Name)
		//{
		//    CountOfAllActiveUsers.Series.Clear();
		//    DataTable dt = VSWebBL.DashboardBL.SametimeHealthBL.Ins.GetCountOfAllActiveUsers(Name);

		//    //string header = ASPxRoundPanel5.HeaderText;
		//    //	ASPxRoundPanel5.HeaderText = ASPxRoundPanel5.HeaderText + " - " + Name;
		//    //ASPxRoundPanel5.HeaderText = ASPxRoundPanel5.HeaderText + " - " + Name;
		//    //PxRoundPanel5.HeaderText = Name;
		//    //lblservername.Text = Name;
		//    Series series = null;
		//    series = new Series("AllActiveUsers", ViewType.Line);
		//    series.Visible = true;
		//    series.DataSource = dt;
		//    series.ArgumentDataMember = dt.Columns["Date"].ToString();

		//    ValueDataMemberCollection seriesValueDataMembers = (ValueDataMemberCollection)series.ValueDataMembers;
		//    seriesValueDataMembers.AddRange(dt.Columns["StatValue"].ToString());
		//    CountOfAllActiveUsers.Series.Add(series);

		//    ((XYDiagram)CountOfAllActiveUsers.Diagram).PaneLayoutDirection = PaneLayoutDirection.Horizontal;

		//    XYDiagram seriesXY = (XYDiagram)CountOfAllActiveUsers.Diagram;
		//    seriesXY.AxisY.Title.Text = "Count";
		//    seriesXY.AxisY.Title.Visible = true;
		//    seriesXY.AxisX.Title.Text = "Time";
		//    seriesXY.AxisX.Title.Visible = true;
		//    CountOfAllActiveUsers.Legend.Visible = false;

		//    //((LineSeriesView)series.View).AxisX = 100;
		//    //((SplineSeriesView)series.View).LineMarkerOptions.Size = 4;
		//    //((SplineSeriesView)series.View).LineMarkerOptions.Color = Color.White;

		//    ((LineSeriesView)series.View).LineStyle.Thickness = 2;
		//    ((LineSeriesView)series.View).LineMarkerOptions.Size = 10;
		//    ((LineSeriesView)series.View).LineMarkerOptions.Color = Color.White;
		//    AxisBase axis = ((XYDiagram)CountOfAllActiveUsers.Diagram).AxisX;
		//    axis.GridSpacingAuto = true;
		//    //4/18/2014 NS commented out for VSPLUS-312
		//    //axis.DateTimeGridAlignment = DateTimeMeasurementUnit.Hour;
		//    axis.DateTimeMeasureUnit = DateTimeMeasurementUnit.Minute;
		//    axis.DateTimeOptions.Format = DateTimeFormat.ShortTime;
		//    // axis.NumericOptions.Format=DevExpress.XtraCharts.NumericFormat.Number;

		//    //axis.MinorCount = 15;
		//    //axis.GridSpacing = 5;
		//    axis.Range.SideMarginsEnabled = true;
		//    axis.GridLines.Visible = false;

		//    //axis.DateTimeOptions.Format = DateTimeFormat.Custom;
		//    //axis.DateTimeOptions.FormatString = "MM/dd";

		//    ((LineSeriesView)series.View).Color = Color.Blue;

		//    AxisBase axisy = ((XYDiagram)CountOfAllActiveUsers.Diagram).AxisY;
		//    axisy.Range.AlwaysShowZeroLevel = false;
		//    axisy.Range.SideMarginsEnabled = true;
		//    axisy.GridLines.Visible = true;

		//    CountOfAllActiveUsers.DataSource = dt;
		//    CountOfAllActiveUsers.DataBind();

		//    double min = Convert.ToDouble(((XYDiagram)CountOfAllActiveUsers.Diagram).AxisY.Range.MinValue);
		//    double max = Convert.ToDouble(((XYDiagram)CountOfAllActiveUsers.Diagram).AxisY.Range.MaxValue);

		//    int gs = (int)((max - min) / 5);

		//    if (gs == 0)
		//    {
		//        gs = 1;
		//        seriesXY.AxisY.GridSpacingAuto = false;
		//        seriesXY.AxisY.GridSpacing = gs;
		//    }
		//}
		public void SetNumberofchatmessages(string Name)
		{
			Numberofchatmessages.Series.Clear();
			DataTable dt = VSWebBL.DashboardBL.SametimeHealthBL.Ins.GetNumberofchatmessages(Name);

			//string header = ASPxRoundPanel5.HeaderText;
			//	ASPxRoundPanel5.HeaderText = ASPxRoundPanel5.HeaderText + " - " + Name;
			//ASPxRoundPanel5.HeaderText = ASPxRoundPanel5.HeaderText + " - " + Name;
			//PxRoundPanel5.HeaderText = Name;
			//lblservername.Text = Name;
			Series series = null;
			series = new Series("P2PActiveUsers", ViewType.Line);
			series.Visible = true;
			series.DataSource = dt;
			series.ArgumentDataMember = dt.Columns["Date"].ToString();

			ValueDataMemberCollection seriesValueDataMembers = (ValueDataMemberCollection)series.ValueDataMembers;
			seriesValueDataMembers.AddRange(dt.Columns["StatValue"].ToString());
			Numberofchatmessages.Series.Add(series);

			((XYDiagram)Numberofchatmessages.Diagram).PaneLayoutDirection = PaneLayoutDirection.Horizontal;

			XYDiagram seriesXY = (XYDiagram)Numberofchatmessages.Diagram;
			seriesXY.AxisY.Title.Text = "Count";
			seriesXY.AxisY.Title.Visible = true;
			seriesXY.AxisX.Title.Text = "Time";
			seriesXY.AxisX.Title.Visible = true;
			Numberofchatmessages.Legend.Visible = false;

			//((LineSeriesView)series.View).AxisX = 100;
			//((SplineSeriesView)series.View).LineMarkerOptions.Size = 4;
			//((SplineSeriesView)series.View).LineMarkerOptions.Color = Color.White;

			((LineSeriesView)series.View).LineStyle.Thickness = 2;
			((LineSeriesView)series.View).LineMarkerOptions.Size = 4;
			((LineSeriesView)series.View).LineMarkerOptions.Color = Color.White;
			AxisBase axis = ((XYDiagram)Numberofchatmessages.Diagram).AxisX;
			axis.GridSpacingAuto = true;
			//4/18/2014 NS commented out for VSPLUS-312
			//axis.DateTimeGridAlignment = DateTimeMeasurementUnit.Hour;
			axis.DateTimeMeasureUnit = DateTimeMeasurementUnit.Minute;
			axis.DateTimeOptions.Format = DateTimeFormat.ShortTime;
			// axis.NumericOptions.Format=DevExpress.XtraCharts.NumericFormat.Number;

			//axis.MinorCount = 15;
			//axis.GridSpacing = 5;
			axis.Range.SideMarginsEnabled = true;
			axis.GridLines.Visible = false;

			//axis.DateTimeOptions.Format = DateTimeFormat.Custom;
			//axis.DateTimeOptions.FormatString = "MM/dd";

			((LineSeriesView)series.View).Color = Color.Blue;

			AxisBase axisy = ((XYDiagram)Numberofchatmessages.Diagram).AxisY;
			axisy.Range.AlwaysShowZeroLevel = false;
			axisy.Range.SideMarginsEnabled = true;
			axisy.GridLines.Visible = true;

			Numberofnwaychats.DataSource = dt;
			Numberofnwaychats.DataBind();

			double min = Convert.ToDouble(((XYDiagram)Numberofchatmessages.Diagram).AxisY.Range.MinValue);
			double max = Convert.ToDouble(((XYDiagram)Numberofchatmessages.Diagram).AxisY.Range.MaxValue);

			int gs = (int)((max - min) / 5);

			if (gs == 0)
			{
				gs = 1;
				seriesXY.AxisY.GridSpacingAuto = false;
				seriesXY.AxisY.GridSpacing = gs;
			}
		}
		public void SetNumberofnwaychats(string Name)
		{
			Numberofnwaychats.Series.Clear();
			DataTable dt = VSWebBL.DashboardBL.SametimeHealthBL.Ins.GetNumberofnwaychats(Name);

			//string header = ASPxRoundPanel5.HeaderText;
			//	ASPxRoundPanel5.HeaderText = ASPxRoundPanel5.HeaderText + " - " + Name;
			//ASPxRoundPanel5.HeaderText = ASPxRoundPanel5.HeaderText + " - " + Name;
			//PxRoundPanel5.HeaderText = Name;
			//lblservername.Text = Name;
			Series series = null;
			series = new Series("MCUActiveUsers", ViewType.Line);
			series.Visible = true;
			series.DataSource = dt;
			series.ArgumentDataMember = dt.Columns["Date"].ToString();

			ValueDataMemberCollection seriesValueDataMembers = (ValueDataMemberCollection)series.ValueDataMembers;
			seriesValueDataMembers.AddRange(dt.Columns["StatValue"].ToString());
			Numberofnwaychats.Series.Add(series);

			((XYDiagram)Numberofnwaychats.Diagram).PaneLayoutDirection = PaneLayoutDirection.Horizontal;

			XYDiagram seriesXY = (XYDiagram)Numberofnwaychats.Diagram;
			seriesXY.AxisY.Title.Text = "Count";
			seriesXY.AxisY.Title.Visible = true;
			seriesXY.AxisX.Title.Text = "Time";
			seriesXY.AxisX.Title.Visible = true;
			Numberofnwaychats.Legend.Visible = false;

			//((LineSeriesView)series.View).AxisX = 100;
			//((SplineSeriesView)series.View).LineMarkerOptions.Size = 4;
			//((SplineSeriesView)series.View).LineMarkerOptions.Color = Color.White;

			((LineSeriesView)series.View).LineStyle.Thickness = 2;
			((LineSeriesView)series.View).LineMarkerOptions.Size = 4;
			((LineSeriesView)series.View).LineMarkerOptions.Color = Color.White;
			AxisBase axis = ((XYDiagram)Numberofnwaychats.Diagram).AxisX;
			axis.GridSpacingAuto = true;
			//4/18/2014 NS commented out for VSPLUS-312
			//axis.DateTimeGridAlignment = DateTimeMeasurementUnit.Hour;
			axis.DateTimeMeasureUnit = DateTimeMeasurementUnit.Minute;
			axis.DateTimeOptions.Format = DateTimeFormat.ShortTime;
			// axis.NumericOptions.Format=DevExpress.XtraCharts.NumericFormat.Number;

			//axis.MinorCount = 15;
			//axis.GridSpacing = 5;
			axis.Range.SideMarginsEnabled = true;
			axis.GridLines.Visible = false;

			//axis.DateTimeOptions.Format = DateTimeFormat.Custom;
			//axis.DateTimeOptions.FormatString = "MM/dd";

			((LineSeriesView)series.View).Color = Color.Blue;

			AxisBase axisy = ((XYDiagram)Numberofnwaychats.Diagram).AxisY;
			axisy.Range.AlwaysShowZeroLevel = false;
			axisy.Range.SideMarginsEnabled = true;
			axisy.GridLines.Visible = true;

			Numberofnwaychats.DataSource = dt;
			Numberofnwaychats.DataBind();

			double min = Convert.ToDouble(((XYDiagram)Numberofnwaychats.Diagram).AxisY.Range.MinValue);
			double max = Convert.ToDouble(((XYDiagram)Numberofnwaychats.Diagram).AxisY.Range.MaxValue);

			int gs = (int)((max - min) / 5);

			if (gs == 0)
			{
				gs = 1;
				seriesXY.AxisY.GridSpacingAuto = false;
				seriesXY.AxisY.GridSpacing = gs;
			}
		}
		public void SetNumberofopenchatsessions(string Name)
		{
			Numberofopenchatsessions.Series.Clear();
			DataTable dt = VSWebBL.DashboardBL.SametimeHealthBL.Ins.GetNumberofopenchatsessions(Name);

			//string header = ASPxRoundPanel5.HeaderText;
			//	ASPxRoundPanel5.HeaderText = ASPxRoundPanel5.HeaderText + " - " + Name;
			//ASPxRoundPanel5.HeaderText = ASPxRoundPanel5.HeaderText + " - " + Name;
			//PxRoundPanel5.HeaderText = Name;
			//lblservername.Text = Name;
			Series series = null;
			series = new Series("PSActiveUsers", ViewType.Line);
			series.Visible = true;
			series.DataSource = dt;
			series.ArgumentDataMember = dt.Columns["Date"].ToString();

			ValueDataMemberCollection seriesValueDataMembers = (ValueDataMemberCollection)series.ValueDataMembers;
			seriesValueDataMembers.AddRange(dt.Columns["StatValue"].ToString());
			Numberofopenchatsessions.Series.Add(series);

			((XYDiagram)Numberofopenchatsessions.Diagram).PaneLayoutDirection = PaneLayoutDirection.Horizontal;

			XYDiagram seriesXY = (XYDiagram)Numberofopenchatsessions.Diagram;
			seriesXY.AxisY.Title.Text = "Count";
			seriesXY.AxisY.Title.Visible = true;
			seriesXY.AxisX.Title.Text = "Time";
			seriesXY.AxisX.Title.Visible = true;
			Numberofopenchatsessions.Legend.Visible = false;

			//((LineSeriesView)series.View).AxisX = 100;
			//((SplineSeriesView)series.View).LineMarkerOptions.Size = 4;
			//((SplineSeriesView)series.View).LineMarkerOptions.Color = Color.White;

			((LineSeriesView)series.View).LineStyle.Thickness = 2;
			((LineSeriesView)series.View).LineMarkerOptions.Size = 4;
			((LineSeriesView)series.View).LineMarkerOptions.Color = Color.White;
			AxisBase axis = ((XYDiagram)Numberofopenchatsessions.Diagram).AxisX;
			axis.GridSpacingAuto = true;
			//4/18/2014 NS commented out for VSPLUS-312
			//axis.DateTimeGridAlignment = DateTimeMeasurementUnit.Hour;
			axis.DateTimeMeasureUnit = DateTimeMeasurementUnit.Minute;
			axis.DateTimeOptions.Format = DateTimeFormat.ShortTime;
			// axis.NumericOptions.Format=DevExpress.XtraCharts.NumericFormat.Number;

			//axis.MinorCount = 15;
			//axis.GridSpacing = 5;
			axis.Range.SideMarginsEnabled = true;
			axis.GridLines.Visible = false;

			//axis.DateTimeOptions.Format = DateTimeFormat.Custom;
			//axis.DateTimeOptions.FormatString = "MM/dd";

			((LineSeriesView)series.View).Color = Color.Blue;

			AxisBase axisy = ((XYDiagram)Numberofopenchatsessions.Diagram).AxisY;
			axisy.Range.AlwaysShowZeroLevel = false;
			axisy.Range.SideMarginsEnabled = true;
			axisy.GridLines.Visible = true;

			Numberofopenchatsessions.DataSource = dt;
			Numberofopenchatsessions.DataBind();

			double min = Convert.ToDouble(((XYDiagram)Numberofopenchatsessions.Diagram).AxisY.Range.MinValue);
			double max = Convert.ToDouble(((XYDiagram)Numberofopenchatsessions.Diagram).AxisY.Range.MaxValue);

			int gs = (int)((max - min) / 5);

			if (gs == 0)
			{
				gs = 1;
				seriesXY.AxisY.GridSpacingAuto = false;
				seriesXY.AxisY.GridSpacing = gs;
			}
		}
		public void SetNumberofactivenwaychats1(string Name)
		{
			Numberofactivenwaychats1.Series.Clear();
			DataTable dt = VSWebBL.DashboardBL.SametimeHealthBL.Ins.GetNumberofactivenwaychats1(Name);

			//string header = ASPxRoundPanel5.HeaderText;
			//	ASPxRoundPanel5.HeaderText = ASPxRoundPanel5.HeaderText + " - " + Name;
			//ASPxRoundPanel5.HeaderText = ASPxRoundPanel5.HeaderText + " - " + Name;
			//PxRoundPanel5.HeaderText = Name;
			//lblservername.Text = Name;
			Series series = null;
			series = new Series("PSActiveUsers", ViewType.Line);
			series.Visible = true;
			series.DataSource = dt;
			series.ArgumentDataMember = dt.Columns["Date"].ToString();

			ValueDataMemberCollection seriesValueDataMembers = (ValueDataMemberCollection)series.ValueDataMembers;
			seriesValueDataMembers.AddRange(dt.Columns["StatValue"].ToString());
			Numberofactivenwaychats1.Series.Add(series);

			((XYDiagram)Numberofactivenwaychats1.Diagram).PaneLayoutDirection = PaneLayoutDirection.Horizontal;

			XYDiagram seriesXY = (XYDiagram)Numberofactivenwaychats1.Diagram;
			seriesXY.AxisY.Title.Text = "Count";
			seriesXY.AxisY.Title.Visible = true;
			seriesXY.AxisX.Title.Text = "Time";
			seriesXY.AxisX.Title.Visible = true;
			Numberofactivenwaychats1.Legend.Visible = false;

			//((LineSeriesView)series.View).AxisX = 100;
			//((SplineSeriesView)series.View).LineMarkerOptions.Size = 4;
			//((SplineSeriesView)series.View).LineMarkerOptions.Color = Color.White;

			((LineSeriesView)series.View).LineStyle.Thickness = 2;
			((LineSeriesView)series.View).LineMarkerOptions.Size = 4;
			((LineSeriesView)series.View).LineMarkerOptions.Color = Color.White;
			AxisBase axis = ((XYDiagram)Numberofactivenwaychats1.Diagram).AxisX;
			axis.GridSpacingAuto = true;
			//4/18/2014 NS commented out for VSPLUS-312
			//axis.DateTimeGridAlignment = DateTimeMeasurementUnit.Hour;
			axis.DateTimeMeasureUnit = DateTimeMeasurementUnit.Minute;
			axis.DateTimeOptions.Format = DateTimeFormat.ShortTime;
			// axis.NumericOptions.Format=DevExpress.XtraCharts.NumericFormat.Number;

			//axis.MinorCount = 15;
			//axis.GridSpacing = 5;
			axis.Range.SideMarginsEnabled = true;
			axis.GridLines.Visible = false;

			//axis.DateTimeOptions.Format = DateTimeFormat.Custom;
			//axis.DateTimeOptions.FormatString = "MM/dd";

			((LineSeriesView)series.View).Color = Color.Blue;

			AxisBase axisy = ((XYDiagram)Numberofactivenwaychats1.Diagram).AxisY;
			axisy.Range.AlwaysShowZeroLevel = false;
			axisy.Range.SideMarginsEnabled = true;
			axisy.GridLines.Visible = true;

			Numberofactivenwaychats1.DataSource = dt;
			Numberofactivenwaychats1.DataBind();

			double min = Convert.ToDouble(((XYDiagram)Numberofactivenwaychats1.Diagram).AxisY.Range.MinValue);
			double max = Convert.ToDouble(((XYDiagram)Numberofactivenwaychats1.Diagram).AxisY.Range.MaxValue);

			int gs = (int)((max - min) / 5);

			if (gs == 0)
			{
				gs = 1;
				seriesXY.AxisY.GridSpacingAuto = false;
				seriesXY.AxisY.GridSpacing = gs;
			}
		}
		public void SetTotalcountofall1x1calls(string Name)
		{
			Totalcountofall1x1calls.Series.Clear();
			DataTable dt = VSWebBL.DashboardBL.SametimeHealthBL.Ins.GetTotalcountofall1x1calls(Name);

			//string header = ASPxRoundPanel5.HeaderText;
			//	ASPxRoundPanel5.HeaderText = ASPxRoundPanel5.HeaderText + " - " + Name;
			//ASPxRoundPanel5.HeaderText = ASPxRoundPanel5.HeaderText + " - " + Name;
			//PxRoundPanel5.HeaderText = Name;
			//lblservername.Text = Name;
			Series series = null;
			series = new Series("PSActiveUsers", ViewType.Line);
			series.Visible = true;
			series.DataSource = dt;
			series.ArgumentDataMember = dt.Columns["Date"].ToString();

			ValueDataMemberCollection seriesValueDataMembers = (ValueDataMemberCollection)series.ValueDataMembers;
			seriesValueDataMembers.AddRange(dt.Columns["StatValue"].ToString());
			Totalcountofall1x1calls.Series.Add(series);

			((XYDiagram)Totalcountofall1x1calls.Diagram).PaneLayoutDirection = PaneLayoutDirection.Horizontal;

			XYDiagram seriesXY = (XYDiagram)Totalcountofall1x1calls.Diagram;
			seriesXY.AxisY.Title.Text = "Count";
			seriesXY.AxisY.Title.Visible = true;
			seriesXY.AxisX.Title.Text = "Time";
			seriesXY.AxisX.Title.Visible = true;
			Totalcountofall1x1calls.Legend.Visible = false;

			//((LineSeriesView)series.View).AxisX = 100;
			//((SplineSeriesView)series.View).LineMarkerOptions.Size = 4;
			//((SplineSeriesView)series.View).LineMarkerOptions.Color = Color.White;

			((LineSeriesView)series.View).LineStyle.Thickness = 2;
			((LineSeriesView)series.View).LineMarkerOptions.Size = 4;
			((LineSeriesView)series.View).LineMarkerOptions.Color = Color.White;
			AxisBase axis = ((XYDiagram)Totalcountofall1x1calls.Diagram).AxisX;
			axis.GridSpacingAuto = true;
			//4/18/2014 NS commented out for VSPLUS-312
			//axis.DateTimeGridAlignment = DateTimeMeasurementUnit.Hour;
			axis.DateTimeMeasureUnit = DateTimeMeasurementUnit.Minute;
			axis.DateTimeOptions.Format = DateTimeFormat.ShortTime;
			// axis.NumericOptions.Format=DevExpress.XtraCharts.NumericFormat.Number;

			//axis.MinorCount = 15;
			//axis.GridSpacing = 5;
			axis.Range.SideMarginsEnabled = true;
			axis.GridLines.Visible = false;

			//axis.DateTimeOptions.Format = DateTimeFormat.Custom;
			//axis.DateTimeOptions.FormatString = "MM/dd";

			((LineSeriesView)series.View).Color = Color.Blue;

			AxisBase axisy = ((XYDiagram)Totalcountofall1x1calls.Diagram).AxisY;
			axisy.Range.AlwaysShowZeroLevel = false;
			axisy.Range.SideMarginsEnabled = true;
			axisy.GridLines.Visible = true;

			Totalcountofall1x1calls.DataSource = dt;
			Totalcountofall1x1calls.DataBind();

			double min = Convert.ToDouble(((XYDiagram)Totalcountofall1x1calls.Diagram).AxisY.Range.MinValue);
			double max = Convert.ToDouble(((XYDiagram)Totalcountofall1x1calls.Diagram).AxisY.Range.MaxValue);

			int gs = (int)((max - min) / 5);

			if (gs == 0)
			{
				gs = 1;
				seriesXY.AxisY.GridSpacingAuto = false;
				seriesXY.AxisY.GridSpacing = gs;
			}
		}
		public void SetTotalcountofallcalls(string Name)
		{
			Totalcountofallcalls.Series.Clear();
			DataTable dt = VSWebBL.DashboardBL.SametimeHealthBL.Ins.GetTotalcountofallcalls(Name);

			//string header = ASPxRoundPanel5.HeaderText;
			//	ASPxRoundPanel5.HeaderText = ASPxRoundPanel5.HeaderText + " - " + Name;
			//ASPxRoundPanel5.HeaderText = ASPxRoundPanel5.HeaderText + " - " + Name;
			//PxRoundPanel5.HeaderText = Name;
			//lblservername.Text = Name;
			Series series = null;
			series = new Series("PSActiveUsers", ViewType.Line);
			series.Visible = true;
			series.DataSource = dt;
			series.ArgumentDataMember = dt.Columns["Date"].ToString();

			ValueDataMemberCollection seriesValueDataMembers = (ValueDataMemberCollection)series.ValueDataMembers;
			seriesValueDataMembers.AddRange(dt.Columns["StatValue"].ToString());
			Totalcountofallcalls.Series.Add(series);

			((XYDiagram)Totalcountofallcalls.Diagram).PaneLayoutDirection = PaneLayoutDirection.Horizontal;

			XYDiagram seriesXY = (XYDiagram)Totalcountofallcalls.Diagram;
			seriesXY.AxisY.Title.Text = "Count";
			seriesXY.AxisY.Title.Visible = true;
			seriesXY.AxisX.Title.Text = "Time";
			seriesXY.AxisX.Title.Visible = true;
			Totalcountofallcalls.Legend.Visible = false;

			//((LineSeriesView)series.View).AxisX = 100;
			//((SplineSeriesView)series.View).LineMarkerOptions.Size = 4;
			//((SplineSeriesView)series.View).LineMarkerOptions.Color = Color.White;

			((LineSeriesView)series.View).LineStyle.Thickness = 2;
			((LineSeriesView)series.View).LineMarkerOptions.Size = 4;
			((LineSeriesView)series.View).LineMarkerOptions.Color = Color.White;
			AxisBase axis = ((XYDiagram)Totalcountofallcalls.Diagram).AxisX;
			axis.GridSpacingAuto = true;
			//4/18/2014 NS commented out for VSPLUS-312
			//axis.DateTimeGridAlignment = DateTimeMeasurementUnit.Hour;
			axis.DateTimeMeasureUnit = DateTimeMeasurementUnit.Minute;
			axis.DateTimeOptions.Format = DateTimeFormat.ShortTime;
			// axis.NumericOptions.Format=DevExpress.XtraCharts.NumericFormat.Number;

			//axis.MinorCount = 15;
			//axis.GridSpacing = 5;
			axis.Range.SideMarginsEnabled = true;
			axis.GridLines.Visible = false;

			//axis.DateTimeOptions.Format = DateTimeFormat.Custom;
			//axis.DateTimeOptions.FormatString = "MM/dd";

			((LineSeriesView)series.View).Color = Color.Blue;

			AxisBase axisy = ((XYDiagram)Totalcountofallcalls.Diagram).AxisY;
			axisy.Range.AlwaysShowZeroLevel = false;
			axisy.Range.SideMarginsEnabled = true;
			axisy.GridLines.Visible = true;

			Totalcountofallcalls.DataSource = dt;
			Totalcountofallcalls.DataBind();

			double min = Convert.ToDouble(((XYDiagram)Totalcountofallcalls.Diagram).AxisY.Range.MinValue);
			double max = Convert.ToDouble(((XYDiagram)Totalcountofallcalls.Diagram).AxisY.Range.MaxValue);

			int gs = (int)((max - min) / 5);

			if (gs == 0)
			{
				gs = 1;
				seriesXY.AxisY.GridSpacingAuto = false;
				seriesXY.AxisY.GridSpacing = gs;
			}
		}
		public void SetTotalcountofallmultiusercalls(string Name)
		{
			Totalcountofallmultiusercalls.Series.Clear();
			DataTable dt = VSWebBL.DashboardBL.SametimeHealthBL.Ins.GetTotalcountofallmultiusercalls(Name);

			//string header = ASPxRoundPanel5.HeaderText;
			//	ASPxRoundPanel5.HeaderText = ASPxRoundPanel5.HeaderText + " - " + Name;
			//ASPxRoundPanel5.HeaderText = ASPxRoundPanel5.HeaderText + " - " + Name;
			//PxRoundPanel5.HeaderText = Name;
			//lblservername.Text = Name;
			Series series = null;
			series = new Series("PSActiveUsers", ViewType.Line);
			series.Visible = true;
			series.DataSource = dt;
			series.ArgumentDataMember = dt.Columns["Date"].ToString();

			ValueDataMemberCollection seriesValueDataMembers = (ValueDataMemberCollection)series.ValueDataMembers;
			seriesValueDataMembers.AddRange(dt.Columns["StatValue"].ToString());
			Totalcountofallmultiusercalls.Series.Add(series);

			((XYDiagram)Totalcountofallmultiusercalls.Diagram).PaneLayoutDirection = PaneLayoutDirection.Horizontal;

			XYDiagram seriesXY = (XYDiagram)Totalcountofallmultiusercalls.Diagram;
			seriesXY.AxisY.Title.Text = "Count";
			seriesXY.AxisY.Title.Visible = true;
			seriesXY.AxisX.Title.Text = "Time";
			seriesXY.AxisX.Title.Visible = true;
			Totalcountofallmultiusercalls.Legend.Visible = false;

			//((LineSeriesView)series.View).AxisX = 100;
			//((SplineSeriesView)series.View).LineMarkerOptions.Size = 4;
			//((SplineSeriesView)series.View).LineMarkerOptions.Color = Color.White;

			((LineSeriesView)series.View).LineStyle.Thickness = 2;
			((LineSeriesView)series.View).LineMarkerOptions.Size = 4;
			((LineSeriesView)series.View).LineMarkerOptions.Color = Color.White;
			AxisBase axis = ((XYDiagram)Totalcountofallmultiusercalls.Diagram).AxisX;
			axis.GridSpacingAuto = true;
			//4/18/2014 NS commented out for VSPLUS-312
			//axis.DateTimeGridAlignment = DateTimeMeasurementUnit.Hour;
			axis.DateTimeMeasureUnit = DateTimeMeasurementUnit.Minute;
			axis.DateTimeOptions.Format = DateTimeFormat.ShortTime;
			// axis.NumericOptions.Format=DevExpress.XtraCharts.NumericFormat.Number;

			//axis.MinorCount = 15;
			//axis.GridSpacing = 5;
			axis.Range.SideMarginsEnabled = true;
			axis.GridLines.Visible = false;

			//axis.DateTimeOptions.Format = DateTimeFormat.Custom;
			//axis.DateTimeOptions.FormatString = "MM/dd";

			((LineSeriesView)series.View).Color = Color.Blue;

			AxisBase axisy = ((XYDiagram)Totalcountofallcalls.Diagram).AxisY;
			axisy.Range.AlwaysShowZeroLevel = false;
			axisy.Range.SideMarginsEnabled = true;
			axisy.GridLines.Visible = true;

			Totalcountofallmultiusercalls.DataSource = dt;
			Totalcountofallmultiusercalls.DataBind();

			double min = Convert.ToDouble(((XYDiagram)Totalcountofallmultiusercalls.Diagram).AxisY.Range.MinValue);
			double max = Convert.ToDouble(((XYDiagram)Totalcountofallmultiusercalls.Diagram).AxisY.Range.MaxValue);

			int gs = (int)((max - min) / 5);

			if (gs == 0)
			{
				gs = 1;
				seriesXY.AxisY.GridSpacingAuto = false;
				seriesXY.AxisY.GridSpacing = gs;
			}
		}
        protected void ActiveChartWebChartControl_CustomCallback(object sender, DevExpress.XtraCharts.Web.CustomCallbackEventArgs e)
        {
            ActiveChartWebChartControl.Width = new Unit(Convert.ToInt32(chartWidth.Value));
        }
		//public void SetGraphForChatnWayChatSessionsWebChartControl(string Name)
		//{
		//    ChatnWayChatSessionsWebChartControl.Series.Clear();

		//     DataTable dt1 = VSWebBL.DashboardBL.SametimeHealthBL.Ins.SetGraphForChatnWayChatSessionsWebChartControl(Name);
		//     DataTable dt2 = dt1.DefaultView.ToTable(true, "StatName");
			 
		//    //DataSet ds = new DataSet();
		//    //DataTable finaldt1 = dt1.Clone();
		//    for (int i = 0; i < dt2.Rows.Count; i++)
		//    {
		//        DataTable finaldt = new DataTable();
		//        finaldt = dt1.Clone();
		//        //string str = dt2.Rows[i][0].ToString();
		//        DataRow[] dr = dt1.Select("StatName = '" + dt2.Rows[i][0].ToString() + "'");
		//        for (int c = 0; c < dr.Length; c++)
		//        {
		//            finaldt.NewRow();
		//            finaldt.ImportRow(dr[c]);
		//        }

		//        Series series = new Series(dt2.Rows[i][0].ToString(), ViewType.Line);

		//        series.DataSource = finaldt;

		//        series.ArgumentDataMember = dt1.Columns["Date"].ToString();

		//        ValueDataMemberCollection seriesValueDataMembers = (ValueDataMemberCollection)series.ValueDataMembers;
		//        seriesValueDataMembers.AddRange(dt1.Columns["StatValue"].ToString());

		//        ChatnWayChatSessionsWebChartControl.Series.Add(series);

		//        XYDiagram seriesXY = (XYDiagram)ChatnWayChatSessionsWebChartControl.Diagram;
		//        seriesXY.AxisX.Title.Text = "Time";
		//        seriesXY.AxisX.Title.Visible = true;
		//        seriesXY.AxisY.Title.Text = "Count";
		//        seriesXY.AxisY.Title.Visible = true;

		//        //transactionPerMinuteWebChart.Legend.Visible = false;

		//        // ((SplineSeriesView)series.View).LineTensionPercent = 100;
		//        ((LineSeriesView)series.View).LineMarkerOptions.Size = 8;
		//        ((LineSeriesView)series.View).LineMarkerOptions.Color = Color.White;

		//        //series.CrosshairLabelPattern = "{A} : {V}";

		//        AxisBase axis = ((XYDiagram)ChatnWayChatSessionsWebChartControl.Diagram).AxisX;
		//        //4/18/2014 NS commented out for VSPLUS-312
		//        //axis.DateTimeGridAlignment = DateTimeMeasurementUnit.Hour;
		//        axis.GridSpacingAuto = false;
		//        axis.MinorCount = 15;
		//        axis.GridSpacing = 1;
		//        axis.Range.SideMarginsEnabled = false;
		//        axis.GridLines.Visible = false;
		//        //axis.DateTimeOptions.Format = DateTimeFormat.Custom;
		//        //axis.DateTimeOptions.FormatString = "MM/dd/yyyy HH:mm";

		//        AxisBase axisy = ((XYDiagram)ChatnWayChatSessionsWebChartControl.Diagram).AxisY;
		//        axisy.Range.AlwaysShowZeroLevel = false;
		//        axisy.Range.SideMarginsEnabled = true;
		//        axisy.GridLines.Visible = true;
		//    }
		//}
		public void SetCountofallcallsandusers(string Name)
		{
			Countofallcallsandusers.Series.Clear();

			DataTable dt1 = VSWebBL.DashboardBL.SametimeHealthBL.Ins.GetCountofallcallsandusers(Name);
			DataTable dt2 = dt1.DefaultView.ToTable(true, "StatName");

			//DataSet ds = new DataSet();
			//DataTable finaldt1 = dt1.Clone();
			for (int i = 0; i < dt2.Rows.Count; i++)
			{
				DataTable finaldt = new DataTable();
				finaldt = dt1.Clone();
				//string str = dt2.Rows[i][0].ToString();
				DataRow[] dr = dt1.Select("StatName = '" + dt2.Rows[i][0].ToString() + "'");
				for (int c = 0; c < dr.Length; c++)
				{
					finaldt.NewRow();
					finaldt.ImportRow(dr[c]);
				}

				Series series = new Series(dt2.Rows[i][0].ToString(), ViewType.Line);

				series.DataSource = finaldt;

				series.ArgumentDataMember = dt1.Columns["Date"].ToString();

				ValueDataMemberCollection seriesValueDataMembers = (ValueDataMemberCollection)series.ValueDataMembers;
				seriesValueDataMembers.AddRange(dt1.Columns["StatValue"].ToString());

				Countofallcallsandusers.Series.Add(series);

				XYDiagram seriesXY = (XYDiagram)Countofallcallsandusers.Diagram;
				seriesXY.AxisX.Title.Text = "Time";
				seriesXY.AxisX.Title.Visible = true;
				seriesXY.AxisY.Title.Text = "Count";
				seriesXY.AxisY.Title.Visible = true;

				//transactionPerMinuteWebChart.Legend.Visible = false;

				// ((SplineSeriesView)series.View).LineTensionPercent = 100;
				((LineSeriesView)series.View).LineMarkerOptions.Size = 4;
				((LineSeriesView)series.View).LineMarkerOptions.Color = Color.White;

				//series.CrosshairLabelPattern = "{A} : {V}";

				AxisBase axis = ((XYDiagram)Countofallcallsandusers.Diagram).AxisX;
				//4/18/2014 NS commented out for VSPLUS-312
				//axis.DateTimeGridAlignment = DateTimeMeasurementUnit.Hour;
				axis.GridSpacingAuto = false;
				axis.MinorCount = 15;
				axis.GridSpacing = 1;
				axis.Range.SideMarginsEnabled = false;
				axis.GridLines.Visible = false;
				//axis.DateTimeOptions.Format = DateTimeFormat.Custom;
				//axis.DateTimeOptions.FormatString = "MM/dd/yyyy HH:mm";

				AxisBase axisy = ((XYDiagram)Countofallcallsandusers.Diagram).AxisY;
				axisy.Range.AlwaysShowZeroLevel = false;
				axisy.Range.SideMarginsEnabled = true;
				axisy.GridLines.Visible = true;
			}
		}
		public void SetCountofall1x1callsandusers(string Name)
		{
			Countofall1x1callsandusers.Series.Clear();

			DataTable dt1 = VSWebBL.DashboardBL.SametimeHealthBL.Ins.GetCountofall1x1callsandusers(Name);
			DataTable dt2 = dt1.DefaultView.ToTable(true, "StatName");

			//DataSet ds = new DataSet();
			//DataTable finaldt1 = dt1.Clone();
			for (int i = 0; i < dt2.Rows.Count; i++)
			{
				DataTable finaldt = new DataTable();
				finaldt = dt1.Clone();
				//string str = dt2.Rows[i][0].ToString();
				DataRow[] dr = dt1.Select("StatName = '" + dt2.Rows[i][0].ToString() + "'");
				for (int c = 0; c < dr.Length; c++)
				{
					finaldt.NewRow();
					finaldt.ImportRow(dr[c]);
				}

				Series series = new Series(dt2.Rows[i][0].ToString(), ViewType.Line);

				series.DataSource = finaldt;

				series.ArgumentDataMember = dt1.Columns["Date"].ToString();

				ValueDataMemberCollection seriesValueDataMembers = (ValueDataMemberCollection)series.ValueDataMembers;
				seriesValueDataMembers.AddRange(dt1.Columns["StatValue"].ToString());

				Countofall1x1callsandusers.Series.Add(series);

				XYDiagram seriesXY = (XYDiagram)Countofall1x1callsandusers.Diagram;
				seriesXY.AxisX.Title.Text = "Time";
				seriesXY.AxisX.Title.Visible = true;
				seriesXY.AxisY.Title.Text = "Count";
				seriesXY.AxisY.Title.Visible = true;

				//transactionPerMinuteWebChart.Legend.Visible = false;

				// ((SplineSeriesView)series.View).LineTensionPercent = 100;
				((LineSeriesView)series.View).LineMarkerOptions.Size = 4;
				((LineSeriesView)series.View).LineMarkerOptions.Color = Color.White;

				//series.CrosshairLabelPattern = "{A} : {V}";

				AxisBase axis = ((XYDiagram)Countofall1x1callsandusers.Diagram).AxisX;
				//4/18/2014 NS commented out for VSPLUS-312
				//axis.DateTimeGridAlignment = DateTimeMeasurementUnit.Hour;
				axis.GridSpacingAuto = false;
				axis.MinorCount = 15;
				axis.GridSpacing = 1;
				axis.Range.SideMarginsEnabled = false;
				axis.GridLines.Visible = false;
				//axis.DateTimeOptions.Format = DateTimeFormat.Custom;
				//axis.DateTimeOptions.FormatString = "MM/dd/yyyy HH:mm";

				AxisBase axisy = ((XYDiagram)Countofall1x1callsandusers.Diagram).AxisY;
				axisy.Range.AlwaysShowZeroLevel = false;
				axisy.Range.SideMarginsEnabled = true;
				axisy.GridLines.Visible = true;
			}
		}
		public void Setcountofallmultiusercallsandusers(string Name)
		{
			countofallmultiusercallsandusers.Series.Clear();

			DataTable dt1 = VSWebBL.DashboardBL.SametimeHealthBL.Ins.Getcountofallmultiusercallsandusers(Name);
			DataTable dt2 = dt1.DefaultView.ToTable(true, "StatName");

			//DataSet ds = new DataSet();
			//DataTable finaldt1 = dt1.Clone();
			for (int i = 0; i < dt2.Rows.Count; i++)
			{
				DataTable finaldt = new DataTable();
				finaldt = dt1.Clone();
				//string str = dt2.Rows[i][0].ToString();
				DataRow[] dr = dt1.Select("StatName = '" + dt2.Rows[i][0].ToString() + "'");
				for (int c = 0; c < dr.Length; c++)
				{
					finaldt.NewRow();
					finaldt.ImportRow(dr[c]);
				}

				Series series = new Series(dt2.Rows[i][0].ToString(), ViewType.Line);

				series.DataSource = finaldt;

				series.ArgumentDataMember = dt1.Columns["Date"].ToString();

				ValueDataMemberCollection seriesValueDataMembers = (ValueDataMemberCollection)series.ValueDataMembers;
				seriesValueDataMembers.AddRange(dt1.Columns["StatValue"].ToString());

				countofallmultiusercallsandusers.Series.Add(series);

				XYDiagram seriesXY = (XYDiagram)countofallmultiusercallsandusers.Diagram;
				seriesXY.AxisX.Title.Text = "Time";
				seriesXY.AxisX.Title.Visible = true;
				seriesXY.AxisY.Title.Text = "Count";
				seriesXY.AxisY.Title.Visible = true;

				//transactionPerMinuteWebChart.Legend.Visible = false;

				// ((SplineSeriesView)series.View).LineTensionPercent = 100;
				((LineSeriesView)series.View).LineMarkerOptions.Size = 4;
				((LineSeriesView)series.View).LineMarkerOptions.Color = Color.White;

				//series.CrosshairLabelPattern = "{A} : {V}";

				AxisBase axis = ((XYDiagram)countofallmultiusercallsandusers.Diagram).AxisX;
				//4/18/2014 NS commented out for VSPLUS-312
				//axis.DateTimeGridAlignment = DateTimeMeasurementUnit.Hour;
				axis.GridSpacingAuto = false;
				axis.MinorCount = 15;
				axis.GridSpacing = 1;
				axis.Range.SideMarginsEnabled = false;
				axis.GridLines.Visible = false;
				//axis.DateTimeOptions.Format = DateTimeFormat.Custom;
				//axis.DateTimeOptions.FormatString = "MM/dd/yyyy HH:mm";

				AxisBase axisy = ((XYDiagram)countofallmultiusercallsandusers.Diagram).AxisY;
				axisy.Range.AlwaysShowZeroLevel = false;
				axisy.Range.SideMarginsEnabled = true;
				axisy.GridLines.Visible = true;
			}
		}
		public void SetNumberofactivemeetingsandusersinsidemeetings(string Name)
		{
			Numberofactivemeetingsandusersinsidemeetings.Series.Clear();

			DataTable dt1 = VSWebBL.DashboardBL.SametimeHealthBL.Ins.GetNumberofactivemeetingsandusersinsidemeetings(Name);
			DataTable dt2 = dt1.DefaultView.ToTable(true, "StatName");

			//DataSet ds = new DataSet();
			//DataTable finaldt1 = dt1.Clone();
			for (int i = 0; i < dt2.Rows.Count; i++)
			{
				DataTable finaldt = new DataTable();
				finaldt = dt1.Clone();
				//string str = dt2.Rows[i][0].ToString();
				DataRow[] dr = dt1.Select("StatName = '" + dt2.Rows[i][0].ToString() + "'");
				for (int c = 0; c < dr.Length; c++)
				{
					finaldt.NewRow();
					finaldt.ImportRow(dr[c]);
				}

				Series series = new Series(dt2.Rows[i][0].ToString(), ViewType.Line);

				series.DataSource = finaldt;

				series.ArgumentDataMember = dt1.Columns["Date"].ToString();

				ValueDataMemberCollection seriesValueDataMembers = (ValueDataMemberCollection)series.ValueDataMembers;
				seriesValueDataMembers.AddRange(dt1.Columns["StatValue"].ToString());

				Numberofactivemeetingsandusersinsidemeetings.Series.Add(series);

				XYDiagram seriesXY = (XYDiagram)Numberofactivemeetingsandusersinsidemeetings.Diagram;
				seriesXY.AxisX.Title.Text = "Time";
				seriesXY.AxisX.Title.Visible = true;
				seriesXY.AxisY.Title.Text = "Count";
				seriesXY.AxisY.Title.Visible = true;

				//transactionPerMinuteWebChart.Legend.Visible = false;

				// ((SplineSeriesView)series.View).LineTensionPercent = 100;
				((LineSeriesView)series.View).LineMarkerOptions.Size = 4;
				((LineSeriesView)series.View).LineMarkerOptions.Color = Color.White;

				//series.CrosshairLabelPattern = "{A} : {V}";

				AxisBase axis = ((XYDiagram)Numberofactivemeetingsandusersinsidemeetings.Diagram).AxisX;
				//4/18/2014 NS commented out for VSPLUS-312
				//axis.DateTimeGridAlignment = DateTimeMeasurementUnit.Hour;
				axis.GridSpacingAuto = false;
				axis.MinorCount = 15;
				axis.GridSpacing = 1;
				axis.Range.SideMarginsEnabled = false;
				axis.GridLines.Visible = false;
				//axis.DateTimeOptions.Format = DateTimeFormat.Custom;
				//axis.DateTimeOptions.FormatString = "MM/dd/yyyy HH:mm";

				AxisBase axisy = ((XYDiagram)Numberofactivemeetingsandusersinsidemeetings.Diagram).AxisY;
				axisy.Range.AlwaysShowZeroLevel = false;
				axisy.Range.SideMarginsEnabled = true;
				axisy.GridLines.Visible = true;
			}
		}

		//protected void ASPxCallbackPanel1_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
		//{
		//    if (selList.Items.Count > 1)
		//    {
		//        //MailBoxID.Visible = true;
		//        //ASPxRoundPanel2.Visible = true;
		//        //mailASPxRoundPanel.Visible = true;
		//        //RoundControlCPU.Visible = true;
		//        //ASPxRoundPanel5.Visible = true;
		//        string[] str = new string[selList.Items.Count];
		//        for (int i = 0; i < selList.Items.Count; i++)
		//        {
		//            str[i] = selList.Items[i].ToString();
		//        }

		//        string serverType = null;

		//        for (int i = 0; i < str.Count(); i++)
		//        {
		//            serverType += "'" + str[i] + "', ";
		//        }

		//        string serverTypes = null;
		//        if (serverType != null)
		//        {
		//            serverTypes = serverType.Remove(serverType.LastIndexOf(","));
		//            ViewState["CheckedListValues"] = serverTypes;

		//            if (rblist1.SelectedIndex == 0)
		//            {
		//                SetGraphForMailBox(serverTypes, rblist1.Value.ToString());
		//            }
		//            else
		//            {
		//                SetGraphForMailBox(serverTypes, rblist1.Value.ToString());
		//            }

		//            ChatnWayChatSessionsWebChartControl(serverTypes);
					
		//        }
		//    }
		//    else
		//    {

		//        ChatnWayChatSessionsWebChartControl.Series.Clear();
				
		//        //1/11/2013 NS need to modify the prompt
		//        //Response.Write("<script>alert('Please select at least two servers to compare')</script>");
		//    }
		//}
		//protected void ChatnWayChatSessionsWebChartControl_CustomCallback(object sender, DevExpress.XtraCharts.Web.CustomCallbackEventArgs e)
		//{
		//    ChatnWayChatSessionsWebChartControl.Width = new Unit(Convert.ToInt32(chartWidth.Value));
		//}
		protected void CountofallcallsandUsersChartControl_CustomCallback(object sender, DevExpress.XtraCharts.Web.CustomCallbackEventArgs e)
		{
			Countofallcallsandusers.Width = new Unit(Convert.ToInt32(chartWidth.Value));
		}
		protected void Countofall1x1callsandusers_CustomCallback(object sender, DevExpress.XtraCharts.Web.CustomCallbackEventArgs e)
		{
			Countofall1x1callsandusers.Width = new Unit(Convert.ToInt32(chartWidth.Value));
		}
		protected void countofallmultiusercallsandusers_CustomCallback(object sender, DevExpress.XtraCharts.Web.CustomCallbackEventArgs e)
		{
			countofallmultiusercallsandusers.Width = new Unit(Convert.ToInt32(chartWidth.Value));
		}
		protected void Numberofactivemeetingsandusersinsidemeetings_CustomCallback(object sender, DevExpress.XtraCharts.Web.CustomCallbackEventArgs e)
		{
			Numberofactivemeetingsandusersinsidemeetings.Width = new Unit(Convert.ToInt32(chartWidth.Value));
		}
		//protected void ActiveMeetingWebChartControl_CustomCallback(object sender, DevExpress.XtraCharts.Web.CustomCallbackEventArgs e)
		//{
		//    ActiveMeetingWebChartControl.Width = new Unit(Convert.ToInt32(chartWidth.Value));
		//}

		//protected void ActiveNwayWebChartControl_CustomCallback(object sender, DevExpress.XtraCharts.Web.CustomCallbackEventArgs e)
		//{
		//    ActiveNwayWebChartControl.Width = new Unit(Convert.ToInt32(chartWidth.Value));
		//}

        protected void DomionSametimegrid_PageSizeChanged(object sender, EventArgs e)
        {
            //ProfilesGridView.PageIndex;
            VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("SametimeHealth|DomionSametimegrid", DomionSametimegrid.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        }
        protected void Sametimegrid_PageSizeChanged(object sender, EventArgs e)
        {
            //ProfilesGridView.PageIndex;
            VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("SametimeHealth|Sametimegrid", Sametimegrid.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        }

        //7/9/2015 NS added for VSPLUS-1973
        protected void ResponseTimesWebChartControl_BoundDataChanged(object sender, EventArgs e)
        {
            UI uiobj = new UI();
            uiobj.RecalibrateChartAxes(ResponseTimesWebChartControl.Diagram);
        }
        //7/9/2015 NS added for VSPLUS-1973
        protected void ActiveChartWebChartControl_BoundDataChanged(object sender, EventArgs e)
        {
            UI uiobj = new UI();
            uiobj.RecalibrateChartAxes(ActiveChartWebChartControl.Diagram);
        }
        //7/9/2015 NS added for VSPLUS-1973
        protected void Numberofnwaychats_BoundDataChanged(object sender, EventArgs e)
        {
            UI uiobj = new UI();
            uiobj.RecalibrateChartAxes(Numberofnwaychats.Diagram);
        }
        //7/9/2015 NS added for VSPLUS-1973
        protected void Numberofchatmessages_BoundDataChanged(object sender, EventArgs e)
        {
            UI uiobj = new UI();
            uiobj.RecalibrateChartAxes(Numberofchatmessages.Diagram);
        }
        //7/9/2015 NS added for VSPLUS-1973
        protected void Numberofopenchatsessions_BoundDataChanged(object sender, EventArgs e)
        {
            UI uiobj = new UI();
            uiobj.RecalibrateChartAxes(Numberofopenchatsessions.Diagram);
        }
        //7/9/2015 NS added for VSPLUS-1973
        protected void Numberofactivenwaychats1_BoundDataChanged(object sender, EventArgs e)
        {
            UI uiobj = new UI();
            uiobj.RecalibrateChartAxes(Numberofactivenwaychats1.Diagram);
        }
        //7/9/2015 NS added for VSPLUS-1973
        protected void Totalcountofall1x1calls_BoundDataChanged(object sender, EventArgs e)
        {
            UI uiobj = new UI();
            uiobj.RecalibrateChartAxes(Totalcountofall1x1calls.Diagram);
        }
        //7/9/2015 NS added for VSPLUS-1973
        protected void Totalcountofallcalls_BoundDataChanged(object sender, EventArgs e)
        {
            UI uiobj = new UI();
            uiobj.RecalibrateChartAxes(Totalcountofallcalls.Diagram);
        }
        //7/9/2015 NS added for VSPLUS-1973
        protected void Totalcountofallmultiusercalls_BoundDataChanged(object sender, EventArgs e)
        {
            UI uiobj = new UI();
            uiobj.RecalibrateChartAxes(Totalcountofallmultiusercalls.Diagram);
        }
        //7/9/2015 NS added for VSPLUS-1973
        protected void Countofallcallsandusers_BoundDataChanged(object sender, EventArgs e)
        {
            UI uiobj = new UI();
            uiobj.RecalibrateChartAxes(Countofallcallsandusers.Diagram);
        }
        //7/9/2015 NS added for VSPLUS-1973
        protected void Countofall1x1callsandusers_BoundDataChanged(object sender, EventArgs e)
        {
            UI uiobj = new UI();
            uiobj.RecalibrateChartAxes(Countofall1x1callsandusers.Diagram);
        }
        //7/9/2015 NS added for VSPLUS-1973
        protected void countofallmultiusercallsandusers_BoundDataChanged(object sender, EventArgs e)
        {
            UI uiobj = new UI();
            uiobj.RecalibrateChartAxes(countofallmultiusercallsandusers.Diagram);
        }
        //7/9/2015 NS added for VSPLUS-1973
        protected void Numberofactivemeetingsandusersinsidemeetings_BoundDataChanged(object sender, EventArgs e)
        {
            UI uiobj = new UI();
            uiobj.RecalibrateChartAxes(Numberofactivemeetingsandusersinsidemeetings.Diagram);
        }
		
    }
}