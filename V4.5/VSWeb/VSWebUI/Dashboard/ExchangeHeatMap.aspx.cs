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
	public partial class ExchangeHeatMap : System.Web.UI.Page
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
				fillgrid();


			}
			
		}
	
		public void fillgrid()
		{
			DataTable stdata = VSWebBL.DashboardBL.ExchangeHeatBL.Ins.getexchangeheatmap();
			Session["sessiondata"] = stdata;
			EXGHeatGridView.DataSource = stdata;
			EXGHeatGridView.DataBind();
			

			for (int i = 0; i < EXGHeatGridView.Columns.Count; i++)
			{
                if (EXGHeatGridView.Columns[i].ToString().Contains("Latency Red Threshold"))
				{
					EXGHeatGridView.Columns[i].Visible = false;
				}
                else if (EXGHeatGridView.Columns[i].ToString().Contains("Latency Yellow Threshold"))
				{
					EXGHeatGridView.Columns[i].Visible = false;
				}
				else if (EXGHeatGridView.Columns[i].ToString().Contains("Date"))
				{
					EXGHeatGridView.Columns[i].Visible = false;
				}
			}
            //int start = EXGHeatGridView.PageIndex * EXGHeatGridView.SettingsPager.PageSize;
            //int end = (EXGHeatGridView.PageIndex + 1) * EXGHeatGridView.SettingsPager.PageSize;
			
            ////GridViewDataColumn column1 = ASPxGridView1.Columns["CategoryName"] as GridViewDataColumn;
            ////GridViewDataColumn column2 = ASPxGridView1.Columns["Description"] as GridViewDataColumn;
            //for (int i = start; i < end; i++)
            //{
            //    for (int j = 1;j < EXGHeatGridView.Columns.Count;j++)
            //    {

            //       string destval = (EXGHeatGridView.GetRowValues(i, EXGHeatGridView.Columns[j].ToString()) == null ? "" : EXGHeatGridView.GetRowValues(i, EXGHeatGridView.Columns[j].ToString()).ToString());
            //        string destsrv = EXGHeatGridView.Columns[j].ToString();
            //        for (int k = 1; k < EXGHeatGridView.Columns.Count; k++)
            //        {
            //            string r = "";
            //            string y = "";
            //            if (destsrv + "#@RED" == EXGHeatGridView.Columns[k].ToString())
            //            {
            //                r = (EXGHeatGridView.GetRowValues(i, EXGHeatGridView.Columns[k].ToString()) == null ? "" : EXGHeatGridView.GetRowValues(i, EXGHeatGridView.Columns[k].ToString()).ToString());
            //            }
            //            if (destsrv + "#@YELLOW" == EXGHeatGridView.Columns[k].ToString())
            //            {
            //                y = (EXGHeatGridView.GetRowValues(i, EXGHeatGridView.Columns[k].ToString()) == null ? "" : EXGHeatGridView.GetRowValues(i, EXGHeatGridView.Columns[k].ToString()).ToString());
            //            }
            //            if (y != "")
            //            {
            //                if (double.Parse(destval) < double.Parse(y))
            //                {
            //                    EXGHeatGridView.Columns[j].CellStyle.BackColor = Color.Green;
            //                    EXGHeatGridView.Columns[j].CellStyle.ForeColor= Color.White;
            //                    EXGHeatGridView.Columns[j].CellStyle.HorizontalAlign = HorizontalAlign.Center;
            //                }
            //                if (r != "")
            //                {
            //                    if (double.Parse(destval) >= double.Parse(y) && double.Parse(destval) < double.Parse(r))
            //                    {
            //                        EXGHeatGridView.Columns[j].CellStyle.BackColor = Color.Yellow;
            //                        EXGHeatGridView.Columns[j].CellStyle.ForeColor = Color.Black;
            //                        EXGHeatGridView.Columns[j].CellStyle.HorizontalAlign = HorizontalAlign.Center;
									
            //                    }
            //                }

            //            }
            //            if (r != "")
            //            {
            //                if (double.Parse(destval) >= double.Parse(r))
            //                {
            //                    EXGHeatGridView.Columns[j].CellStyle.BackColor = Color.Red;
            //                    EXGHeatGridView.Columns[j].CellStyle.ForeColor = Color.White;
            //                    EXGHeatGridView.Columns[j].CellStyle.HorizontalAlign = HorizontalAlign.Center;
            //                }
            //            }
            //        }
            //    }			
            //}
		}
	

        protected void EXGHeatGridView_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {

			if (e.DataColumn.FieldName != "From Server" && e.DataColumn.FieldName != "Date")
            {
                ASPxGridView request = sender as ASPxGridView;
                string sourceserver = request.GetRowValues(e.VisibleIndex, "From Server").ToString();
                e.Cell.Attributes.Add("onclick", "onCellClick('" + sourceserver + "', '" + e.DataColumn.FieldName + "')");
				//DateTime dt1 = Convert.ToDateTime("2014-12-12 18:59:41.000");
				//SetGraphForLatency(dt1, sourceserver, e.DataColumn.FieldName);
                string destval = e.CellValue.ToString();
                //string YellowTh = int.Parse(request.GetRowValues(e.VisibleIndex, "LatencyYellowThreshold").ToString());
                string YellowTh = request.GetRowValues(e.VisibleIndex, "LatencyYellowThreshold").ToString();
                string RedTh = request.GetRowValues(e.VisibleIndex, "LatencyRedThreshold").ToString();
				string dt = request.GetRowValues(e.VisibleIndex, "Date").ToString();
				dt = "Last Scan Time: " + dt;

				e.Cell.Attributes.Add("title", dt);
                e.Cell.HorizontalAlign = HorizontalAlign.Center;
                if (destval == "--")
                {
                    e.Cell.BackColor = Color.Gray;
                    e.Cell.ForeColor = Color.White;
                }
                else
                {
                    if (YellowTh.ToString() != "")
                    {
                        if (double.Parse(destval) < double.Parse(YellowTh))
                        {
                            e.Cell.BackColor = Color.Green;
                            e.Cell.ForeColor = Color.White;
                        }
                        if (RedTh.ToString() != "")
                        {
                            if (double.Parse(destval) >= double.Parse(YellowTh) && double.Parse(destval) < double.Parse(RedTh))
                            {
                                e.Cell.BackColor = Color.Yellow;
                                e.Cell.ForeColor = Color.Black;
                            }
                        }

                    }
                    if (RedTh.ToString() != "")
                    {
                        if (double.Parse(destval) >= double.Parse(RedTh))
                        {
                            e.Cell.BackColor = Color.Red;
                            e.Cell.ForeColor = Color.White;
                        }
                    }
                }
            }
        }

		protected void ASPxMenu1_ItemClick(object source, DevExpress.Web.MenuItemEventArgs e)
		{



			if (e.Item.Name == "MessageLatencyTest")
			{
				Response.Redirect("~/Configurator/MessageLatencyTest.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
				Context.ApplicationInstance.CompleteRequest();
			}
			else
			{
			}


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
		//public void SetGraphForLatency(DateTime dt1, string sname, string dname)
		//{
			
		//    LatencyWebChartControl.Series.Clear();
		//    //DataTable dt = VSWebBL.DashboardBL.SharepointDetailsBL.Ins.SetGraphForCPUsharepoint(paramGraph, serverName, ServerTypeId);

		//    DataTable dt = VSWebBL.DashboardBL.ExchangeHeatBL.Ins.SetGraphForLatency(dt1, sname, dname);
		//    if (dt.Rows.Count > 0)
		//    {
		//        Series series = null;
		//        series = new Series("LyncServer", ViewType.Line);
		//        series.Visible = true;
		//        series.ArgumentDataMember = dt.Columns["dtfrom"].ToString();

		//        ValueDataMemberCollection seriesValueDataMembers = (ValueDataMemberCollection)series.ValueDataMembers;
		//        seriesValueDataMembers.AddRange(dt.Columns["maxval"].ToString());
		//        LatencyWebChartControl.Series.Add(series);

		//        ((XYDiagram)LatencyWebChartControl.Diagram).PaneLayoutDirection = PaneLayoutDirection.Horizontal;

		//        XYDiagram seriesXY = (XYDiagram)LatencyWebChartControl.Diagram;
		//        seriesXY.AxisY.Title.Text = "Latency";
		//        seriesXY.AxisY.Title.Visible = true;
		//        seriesXY.AxisX.Title.Text = "Date/Time";
		//        seriesXY.AxisX.Title.Visible = true;
		//        LatencyWebChartControl.Legend.Visible = false;

		//        // ((SplineSeriesView)series.View).LineTensionPercent = 100;
		//        ((LineSeriesView)series.View).LineMarkerOptions.Size = 4;
		//        ((LineSeriesView)series.View).LineMarkerOptions.Color = Color.White;

		//        AxisBase axis = ((XYDiagram)LatencyWebChartControl.Diagram).AxisX;
		//        //4/18/2014 NS commented out for VSPLUS-312
		//        //axis.DateTimeGridAlignment = DateTimeMeasurementUnit.Hour;
		//        //5/13/2014 NS commented out for VSPLUS-621
		//        //axis.GridSpacingAuto = false;
		//        //axis.MinorCount = 15;
		//        //axis.GridSpacing = 0.5;
		//        axis.Range.SideMarginsEnabled = false;
		//        axis.GridLines.Visible = false;
		//        //axis.DateTimeOptions.Format = DateTimeFormat.Custom;
		//        //axis.DateTimeOptions.FormatString = "dd/MM HH:mm";
		//        ((LineSeriesView)series.View).Color = Color.Blue;

		//        AxisBase axisy = ((XYDiagram)LatencyWebChartControl.Diagram).AxisY;
		//        axisy.Range.AlwaysShowZeroLevel = false;
		//        axisy.Range.SideMarginsEnabled = true;
		//        axisy.GridLines.Visible = true;
		//        LatencyWebChartControl.DataSource = dt;
		//        LatencyWebChartControl.DataBind();
		//        LatencyWebChartControl.Visible = true;

		//    }
		//    //return dt;     
		//}
	}
}