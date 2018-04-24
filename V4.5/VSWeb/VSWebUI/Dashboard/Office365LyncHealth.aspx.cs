using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Globalization;
using System.Web.UI.HtmlControls;

using DevExpress.Web;
using DevExpress.XtraCharts;
using System.IO;
using DevExpress.XtraPrinting;
using System.Net.Mime;

namespace VSWebUI.Dashboard
{
	public partial class Office365LyncHealth : System.Web.UI.Page
	{
		string accountName;

		protected void Page_PreInit(object sender, EventArgs e)
		{

			this.Master.contentCallEvent += new EventHandler(Master_ButtonClick);
		}

		private void Master_ButtonClick(object sender, EventArgs e)
		{

		}
		DataRow myRow = null;
		protected void Page_Load(object sender, EventArgs e)
		{
			if (Request.QueryString["Name"] != null)
				accountName = Request.QueryString["Name"].ToString();
			if (accountName != "")
			{

				this.deviceTypeWebChart.Width = new Unit(Convert.ToInt32(chartWidth.Value));
				this.OSTypeWebChartControl.Width = new Unit(Convert.ToInt32(chartWidth.Value));

				if (!IsPostBack && !IsCallback)
				{
					HtmlGenericControl body = (HtmlGenericControl)Master.FindControl("Body1");
					body.Attributes.Add("onload", "DoCallback()");
					body.Attributes.Add("onResize", "Resized()");

					fillDeviceTypeServer();
					SetGraphForDeviceType();
					SetGraphConfReport();
					SetGraphP2PSessions();
					//SetBarGraphForOSType(SortRadioButtonList2.SelectedItem.Value.ToString(), OSComboBox.Text);
					SetGraphForAVSessions();
				}
				else
				{
					SetGraphForDeviceType();
					SetGraphP2PSessions();
				}
			}
		}

		public DataTable SetGraphForDeviceType()
		{

			deviceTypeWebChart.Series.Clear();
			DataTable dt = VSWebBL.DashboardBL.Office365BL.Ins.SetGraphDeviceTypes(accountName);

			Series series = new Series("DeviceType", ViewType.Bar);

			series.ArgumentDataMember = dt.Columns["DeviceType"].ToString();
			//10/30/2013 NS added - point labels on series
			series.LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;

			ValueDataMemberCollection seriesValueDataMembers = (ValueDataMemberCollection)series.ValueDataMembers;
			seriesValueDataMembers.AddRange(dt.Columns["No_of_Users"].ToString());
			deviceTypeWebChart.Series.Add(series);

			deviceTypeWebChart.Legend.Visible = false;

			((SideBySideBarSeriesView)series.View).ColorEach = true;

			//AxisBase axisx = ((XYDiagram)deviceTypeWebChart.Diagram).AxisX;

			XYDiagram seriesXY = (XYDiagram)deviceTypeWebChart.Diagram;
			// seriesXY.AxisX.Title.Text = "Milliseconds";
			// seriesXY.AxisX.Title.Visible = true;
			((DevExpress.XtraCharts.XYDiagram)deviceTypeWebChart.Diagram).Rotated = true;
			//11/16/2013 NS modified the AllowRotate option to false, otherwise the server names were unreadable
			seriesXY.AxisX.Label.ResolveOverlappingOptions.AllowRotate = false;
			seriesXY.AxisX.Label.ResolveOverlappingOptions.AllowHide = false;
			seriesXY.AxisY.Title.Text = "Users";
			seriesXY.AxisY.Title.Visible = true;

			AxisBase axisy = ((XYDiagram)deviceTypeWebChart.Diagram).AxisY;
			//4/18/2014 NS modified for VSPLUS-312
			axisy.Range.AlwaysShowZeroLevel = true;
			double min = Convert.ToDouble(((XYDiagram)deviceTypeWebChart.Diagram).AxisY.Range.MinValue);
			double max = Convert.ToDouble(((XYDiagram)deviceTypeWebChart.Diagram).AxisY.Range.MaxValue);

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

			//2/6/2013 NS modified chart height calculations based on the number of rows
			if (dt.Rows.Count > 0)
			{
				if (dt.Rows.Count < 4)
				{
					deviceTypeWebChart.Height = 200;
				}
				else
				{
					if (dt.Rows.Count >= 4 && dt.Rows.Count < 10)
					{
						deviceTypeWebChart.Height = ((dt.Rows.Count) * 50) + 20;
					}
					else
					{
						if (dt.Rows.Count >= 10 && dt.Rows.Count < 100)
						{
							deviceTypeWebChart.Height = ((dt.Rows.Count) * 40) + 20;
						}
						else
						{
							if (dt.Rows.Count >= 100)
							{
								deviceTypeWebChart.Height = ((dt.Rows.Count) * 20) + 20;
							}
						}
					}
				}
			}
			//ResponseWebChartControl.Width = new Unit(1000);
			deviceTypeWebChart.DataSource = dt;
			deviceTypeWebChart.DataBind();
			ChartTitle title = new ChartTitle();
			
				title.Text = "Mobile Devices ";
			deviceTypeWebChart.Titles.Clear();
			deviceTypeWebChart.Titles.Add(title);
			return dt;
		}
		public DataTable SetGraphP2PSessions()
		{

			P2PSessionsChart1.Series.Clear();
			DataTable dt = VSWebBL.DashboardBL.Office365BL.Ins.SetGraphP2PSessions(accountName);

			Series series = new Series("usercount", ViewType.Bar);

			series.ArgumentDataMember = dt.Columns["DeviceType"].ToString();
			//10/30/2013 NS added - point labels on series
			series.LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;

			ValueDataMemberCollection seriesValueDataMembers = (ValueDataMemberCollection)series.ValueDataMembers;
			seriesValueDataMembers.AddRange(dt.Columns["No_of_Users"].ToString());
			P2PSessionsChart1.Series.Add(series);

			P2PSessionsChart1.Legend.Visible = false;

			((SideBySideBarSeriesView)series.View).ColorEach = true;

			//AxisBase axisx = ((XYDiagram)deviceTypeWebChart.Diagram).AxisX;

			XYDiagram seriesXY = (XYDiagram)P2PSessionsChart1.Diagram;
			// seriesXY.AxisX.Title.Text = "Milliseconds";
			// seriesXY.AxisX.Title.Visible = true;
			((DevExpress.XtraCharts.XYDiagram)P2PSessionsChart1.Diagram).Rotated = true;
			//11/16/2013 NS modified the AllowRotate option to false, otherwise the server names were unreadable
			seriesXY.AxisX.Label.ResolveOverlappingOptions.AllowRotate = false;
			seriesXY.AxisX.Label.ResolveOverlappingOptions.AllowHide = false;
			seriesXY.AxisY.Title.Text = "Users";
			seriesXY.AxisY.Title.Visible = true;

			AxisBase axisy = ((XYDiagram)P2PSessionsChart1.Diagram).AxisY;
			//4/18/2014 NS modified for VSPLUS-312
			axisy.Range.AlwaysShowZeroLevel = true;
			double min = Convert.ToDouble(((XYDiagram)P2PSessionsChart1.Diagram).AxisY.Range.MinValue);
			double max = Convert.ToDouble(((XYDiagram)P2PSessionsChart1.Diagram).AxisY.Range.MaxValue);

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

			//2/6/2013 NS modified chart height calculations based on the number of rows
			if (dt.Rows.Count > 0)
			{
				if (dt.Rows.Count < 4)
				{
					P2PSessionsChart1.Height = 200;
				}
				else
				{
					if (dt.Rows.Count >= 4 && dt.Rows.Count < 10)
					{
						P2PSessionsChart1.Height = ((dt.Rows.Count) * 50) + 20;
					}
					else
					{
						if (dt.Rows.Count >= 10 && dt.Rows.Count < 100)
						{
							P2PSessionsChart1.Height = ((dt.Rows.Count) * 40) + 20;
						}
						else
						{
							if (dt.Rows.Count >= 100)
							{
								P2PSessionsChart1.Height = ((dt.Rows.Count) * 20) + 20;
							}
						}
					}
				}
			}
			//ResponseWebChartControl.Width = new Unit(1000);
			P2PSessionsChart1.DataSource = dt;
			P2PSessionsChart1.DataBind();
			ChartTitle title = new ChartTitle();
			
				title.Text = "P2P Sessions";
			P2PSessionsChart1.Titles.Clear();
			P2PSessionsChart1.Titles.Add(title);
			return dt;
		}
		public void fillDeviceTypeServer()
		{
			//DataTable dt = new DataTable();
			//dt = VSWebBL.DashboardBL.LotusTravelerHealthBLL.Ins.SetGrid1();
			//DataRow dr = dt.NewRow();
			//dr[0] = "All";
			//dt.Rows.InsertAt(dr, 0);
			//dt = dt.DefaultView.ToTable(true, "Name");
			//ServerComboBox.DataSource = dt;
			//ServerComboBox.TextField = "Name";
			//ServerComboBox.ValueField = "Name";
			//ServerComboBox.DataBind();
			//ServerComboBox.SelectedIndex = 0;
		}
		public DataTable SetGraphForAVSessions()
		{
			AVSessionsChart.Series.Clear();
			DataTable dt = VSWebBL.DashboardBL.Office365BL.Ins.SetGraphAVSessions(accountName);
			Series series = new Series("OSType", ViewType.Pie);
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				series.Points.Add(new SeriesPoint(dt.Rows[i]["DeviceType"], dt.Rows[i]["No_of_Users"]));
			}
			AVSessionsChart.Series.Add(series);
			series.Label.PointOptions.PointView = PointView.Argument;
			series.ToolTipEnabled = DevExpress.Utils.DefaultBoolean.True;
			//series.Label.PointOptions.ValueNumericOptions.Format = NumericFormat.General;
			// series.Label.PointOptions.ValueNumericOptions.Precision = 0;

			AVSessionsChart.Legend.Visible = false;
			ChartTitle title = new ChartTitle();
			
				title.Text = "Av Sessions";
			AVSessionsChart.Titles.Clear();
			AVSessionsChart.Titles.Add(title);

			AVSessionsChart.DataSource = dt;
			AVSessionsChart.DataBind();
			return dt;
		}
		public DataTable SetGraphConfReport()
		{
			ConfReportChart.Series.Clear();
			DataTable dt = VSWebBL.DashboardBL.Office365BL.Ins.SetGraphConfReport(accountName);
			Series series = new Series("DeviceCount", ViewType.Pie);
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				series.Points.Add(new SeriesPoint(dt.Rows[i]["DeviceType"], dt.Rows[i]["No_Of_users"]));
			}
			ConfReportChart.Series.Add(series);
			series.Label.PointOptions.PointView = PointView.Argument;
			series.ToolTipEnabled = DevExpress.Utils.DefaultBoolean.True;
			//series.Label.PointOptions.ValueNumericOptions.Format = NumericFormat.General;
			// series.Label.PointOptions.ValueNumericOptions.Precision = 0;

			ConfReportChart.Legend.Visible = false;
			ChartTitle title = new ChartTitle();

			title.Text = "Conference Report";

			ConfReportChart.Titles.Clear();
			ConfReportChart.Titles.Add(title);

			ConfReportChart.DataSource = dt;
			ConfReportChart.DataBind();
			return dt;
		}
		protected void ASPxMenu1_Init(object sender, EventArgs e)
		{

			string[][] arrOfMenuItems = {	
			new string[] {"Dashboard", "OverallHealth1.aspx"},
			new string[] {"O365 Stats", "Office365Health.aspx"}
			};

			DevExpress.Web.MenuItem item;
			foreach (string[] arr in arrOfMenuItems)
			{
				item = new DevExpress.Web.MenuItem();
				item.Text = arr[0];
				item.NavigateUrl = arr[1];

				ASPxMenu1.Items[0].Items.Add(item);
			}
		}
	}
}