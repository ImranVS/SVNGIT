using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using DevExpress.XtraPrinting;
using System.Net.Mime;
using System.Data;
using DevExpress.Web;
using DevExpress.XtraGrid;
using System.Drawing;
using DevExpress.XtraCharts;
using System.Drawing;
using VSWebBL;
using DevExpress.XtraCharts.Web;
using DevExpress.XtraScheduler;
using DevExpress.XtraScheduler.Xml;
using System.Text;
using System.Windows.Forms;
using DevExpress.Web.ASPxScheduler;

namespace VSWebUI.Dashboard
{
	public partial class GraphExchangeHeatMap : System.Web.UI.Page
	{
		protected void Page_PreInit(object sender, EventArgs e)
		{

			this.Master.contentCallEvent += new EventHandler(Master_ButtonClick);
		}

		private void Master_ButtonClick(object sender, EventArgs e)
		{

		}

		DataTable stdata;
		
		
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{

				DateTime dt1 = System.DateTime.Now;
				string name = Convert.ToString(Request.QueryString["s"]);
				string name1 = Convert.ToString(Request.QueryString["t"]);
				string fullName = name1;
				 var names = fullName.Split(' ');
				   string firstName = names[0];
				   string dname = names[1];
				   lblsruce.Text = name.ToString();
				   lbldstn.Text = dname.ToString();
			
				SetGraphForLatency(dt1, name, dname);

			}
			//else
			//{

			//    DateTime dt1 = Convert.ToDateTime("2014-12-12 18:59:41.000");
			//    string name = Convert.ToString(Request.QueryString["s"]);
			//    string name1 = Convert.ToString(Request.QueryString["t"]);
			//    SetGraphForLatency(dt1, name, name1);
			//}

			
			
				
			//req qeury string !=""
		}
	
		

       
	

		//protected void EXGHeatGridView_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
		//{
		//    ASPxGridView gridView = (ASPxGridView)sender;
		//    string[] data = e.Parameters.Split(new char[] { '|' });
		//    //DateTime dt1 = System.DateTime.Now;
		//    DateTime dt1 = Convert.ToDateTime("2014-12-12 18:59:41.000");
		//    string fullName = data[1];
		//    var names = fullName.Split(' ');
		//    string firstName = names[0];
		//    string lastName = names[1];
		//    //string sname1 = "JNITTECH-EXCHG1";
		//    //string dname1 = "JNITTECH-EXCHG1";
		//    //DateTime startdate = Convert.ToDateTime("2014-12-10 00:50:52.000");

		//    //SetGraphForLatency(dt1, data[0], lastName);
		//    //fillgraph from MailLatencyDailyStats,[sourceserver]      ,[DestinationServer]      ,[Latency]      ,[Date]
		//    //Date last 24hrs
		//    //string sname = "JNITTECH-EXCHG1";
		//    //string dname = "JNITTECH-EXCHG2";
		//}
		public void SetGraphForLatency(DateTime dt1, string sname, string dname)
		{
			
			LatencyWebChartControl.Series.Clear();
			//DataTable dt = VSWebBL.DashboardBL.SharepointDetailsBL.Ins.SetGraphForCPUsharepoint(paramGraph, serverName, ServerTypeId);

			DataTable dt = VSWebBL.DashboardBL.ExchangeHeatBL.Ins.SetGraphForLatency(dt1, sname, dname);
			if (dt.Rows.Count > 0)
			{
				Series series = null;
				series = new Series("LyncServer", ViewType.Line);
				series.Visible = true;
				series.ArgumentDataMember = dt.Columns["dtfrom"].ToString();

				ValueDataMemberCollection seriesValueDataMembers = (ValueDataMemberCollection)series.ValueDataMembers;
				seriesValueDataMembers.AddRange(dt.Columns["maxval"].ToString());
				LatencyWebChartControl.Series.Add(series);

				((XYDiagram)LatencyWebChartControl.Diagram).PaneLayoutDirection = PaneLayoutDirection.Horizontal;

				XYDiagram seriesXY = (XYDiagram)LatencyWebChartControl.Diagram;
				seriesXY.AxisY.Title.Text = "Latency";
				seriesXY.AxisY.Title.Visible = true;
				seriesXY.AxisX.Title.Text = "Date/Time";
				seriesXY.AxisX.Title.Visible = true;
				LatencyWebChartControl.Legend.Visible = false;

				// ((SplineSeriesView)series.View).LineTensionPercent = 100;
				((LineSeriesView)series.View).LineMarkerOptions.Size = 4;
				((LineSeriesView)series.View).LineMarkerOptions.Color = Color.White;

				AxisBase axis = ((XYDiagram)LatencyWebChartControl.Diagram).AxisX;
				//4/18/2014 NS commented out for VSPLUS-312
				//axis.DateTimeGridAlignment = DateTimeMeasurementUnit.Hour;
				//5/13/2014 NS commented out for VSPLUS-621
				//axis.GridSpacingAuto = false;
				//axis.MinorCount = 15;
				//axis.GridSpacing = 0.5;
				axis.Range.SideMarginsEnabled = false;
				axis.GridLines.Visible = false;
				//axis.DateTimeOptions.Format = DateTimeFormat.Custom;
				//axis.DateTimeOptions.FormatString = "dd/MM HH:mm";
				((LineSeriesView)series.View).Color = Color.Blue;

				AxisBase axisy = ((XYDiagram)LatencyWebChartControl.Diagram).AxisY;
				axisy.Range.AlwaysShowZeroLevel = false;
				axisy.Range.SideMarginsEnabled = true;
				axisy.GridLines.Visible = true;
				LatencyWebChartControl.DataSource = dt;
				LatencyWebChartControl.DataBind();
				LatencyWebChartControl.Visible = true;

			}
			//return dt;     
		}

		

		protected void returngrd_Click1(object sender, EventArgs e)
		{
			Response.Redirect("ExchangeHeatMap.aspx");
		}
	}
}