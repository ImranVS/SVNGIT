using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DevExpress.Web;
using DevExpress.XtraCharts;

namespace VSWebUI.Dashboard
{
	public partial class UserPasswordSettings : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				FillUsersPasswordGrid();
				SetGraphForStrongPasswordCount();
				SetGraphForPasswordNeverExpiresCount();
			}
			else
			{
				FillUserPasswordgridFromSession();
			}
		}
		public void FillUsersPasswordGrid()
		{
			DataTable dt = VSWebBL.DashboardBL.Office365BL.Ins.Usersettingsgrid();
			if (dt.Rows.Count > 0)
			{
				O365Usersettinggrid.DataSource = dt;
				Session["Office365Usersettingsgrid"] = dt;
				O365Usersettinggrid.DataBind();
				
			}
		}
		public void FillUserPasswordgridFromSession()
		{
			//MSoma
			if (Session["Office365Usersettingsgrid"] != "" && Session["Office365Usersettingsgrid"] != null)
			{
				O365Usersettinggrid.DataSource = (DataTable)Session["Office365Usersettingsgrid"];
				O365Usersettinggrid.DataBind();
			}

		}
		public DataTable SetGraphForStrongPasswordCount()
		{
			strongpasswordWebChart.Series.Clear();
			DataTable dt = VSWebBL.DashboardBL.Office365BL.Ins.PasswordsChart();
			Series series = new Series("DeviceCount", ViewType.Pie);
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				series.Points.Add(new SeriesPoint("Strong Password Required: "+ dt.Rows[i]["StrongPasswordRequired"], dt.Rows[i]["NumberOfPasswords"]));
			}
			strongpasswordWebChart.Series.Add(series);
			series.Label.PointOptions.PointView = PointView.Argument;
			series.ToolTipEnabled = DevExpress.Utils.DefaultBoolean.True;
			
			
			
			//series.Label.PointOptions.ValueNumericOptions.Format = NumericFormat.General;
			// series.Label.PointOptions.ValueNumericOptions.Precision = 0;

			strongpasswordWebChart.Legend.Visible = false;
			ChartTitle title = new ChartTitle();

			title.Text = "Strong Password Required";

			strongpasswordWebChart.Titles.Clear();
			strongpasswordWebChart.Titles.Add(title);

			strongpasswordWebChart.DataSource = dt;
			strongpasswordWebChart.DataBind();
			return dt;
		}
		protected void strongpasswordWebChart_CustomDrawSeriesPoint(object sender, DevExpress.XtraCharts.CustomDrawSeriesPointEventArgs e)
		{
			//Pie3DDrawOptions drawOptions = e.SeriesDrawOptions as Pie3DDrawOptions;
			//Pie3DDrawOptions legendOptions = e.LegendDrawOptions as Pie3DDrawOptions;
			//drawOptions.FillStyle.FillMode = FillMode3D.Solid;
			//legendOptions.FillStyle.FillMode = FillMode3D.Solid;
			//((Pie3DDrawOptions)e.SeriesDrawOptions).FillStyle.FillMode = FillMode3D.Solid; 
			if (e.SeriesPoint.Argument == "Strong Password Required: True")
			{
				
				e.SeriesDrawOptions.Color = System.Drawing.Color.FromArgb(0, 255, 0);//green
				e.SeriesDrawOptions.Color = System.Drawing.Color.Lime;//green
				//legendOptions.Color = System.Drawing.Color.FromArgb(0, 128, 0);
			}

			else if (e.SeriesPoint.Argument == "Strong Password Required: False")
			{
				//e.SeriesDrawOptions.Color = System.Drawing.Color.FromArgb(253, 0, 0);red
				
				e.SeriesDrawOptions.Color = System.Drawing.Color.FromArgb(255, 0, 0);//red
				e.SeriesDrawOptions.Color = System.Drawing.Color.Red;
				//legendOptions.Color = System.Drawing.Color.FromArgb(253, 0, 0);
			}
		
		}
		public DataTable SetGraphForPasswordNeverExpiresCount()
		{
			Passwordneverexpires.Series.Clear();
			DataTable dt = VSWebBL.DashboardBL.Office365BL.Ins.PasswordsNeverexpiresChart();
			Series series = new Series("DeviceCount", ViewType.Pie);
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				series.Points.Add(new SeriesPoint("Passwords Never Expire: " + dt.Rows[i]["PasswordNeverExpires"], dt.Rows[i]["NumberOfPasswords"]));
			}
			Passwordneverexpires.Series.Add(series);
			series.Label.PointOptions.PointView = PointView.Argument;
			series.ToolTipEnabled = DevExpress.Utils.DefaultBoolean.True;
			//series.Label.PointOptions.ValueNumericOptions.Format = NumericFormat.General;
			// series.Label.PointOptions.ValueNumericOptions.Precision = 0;

			Passwordneverexpires.Legend.Visible = false;
			ChartTitle title = new ChartTitle();

			title.Text = "Password Never Expire";
			
			Passwordneverexpires.Titles.Clear();
			Passwordneverexpires.Titles.Add(title);

			Passwordneverexpires.DataSource = dt;
			Passwordneverexpires.DataBind();
			return dt;
		}
		protected void Passwordneverexpires_CustomDrawSeriesPoint(object sender, DevExpress.XtraCharts.CustomDrawSeriesPointEventArgs e)
		{
			//Pie3DDrawOptions drawOptions = e.SeriesDrawOptions as Pie3DDrawOptions;
			//Pie3DDrawOptions legendOptions = e.LegendDrawOptions as Pie3DDrawOptions;
			//drawOptions.FillStyle.FillMode = FillMode3D.Solid;
			//legendOptions.FillStyle.FillMode = FillMode3D.Solid;
			//((Pie3DDrawOptions)e.SeriesDrawOptions).FillStyle.FillMode = FillMode3D.Solid; 
			if (e.SeriesPoint.Argument == "Passwords Never Expire: True")
			{
				//e.SeriesDrawOptions.Color = System.Drawing.Color.FromArgb(0, 128, 0);red
				e.SeriesDrawOptions.Color = System.Drawing.Color.FromArgb(255, 0, 0); //red
				e.SeriesDrawOptions.Color = System.Drawing.Color.Red; //red
				//legendOptions.Color = System.Drawing.Color.FromArgb(0, 128, 0);
			}

			else if (e.SeriesPoint.Argument == "Passwords Never Expire: False")
			{
				//e.SeriesDrawOptions.Color = System.Drawing.Color.FromArgb(253, 0, 0);
				e.SeriesDrawOptions.Color = System.Drawing.Color.FromArgb(0, 255, 0);//green
				e.SeriesDrawOptions.Color = System.Drawing.Color.Lime;//green
				//legendOptions.Color = System.Drawing.Color.FromArgb(253, 0, 0);
			}

		}

	}
}