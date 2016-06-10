using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using DevExpress.XtraCharts;
namespace VSWebUI
{
    public partial class BiggestExchangeMailFiles : System.Web.UI.Page
    {
		protected void Page_PreInit(object sender, EventArgs e)
		{
			//Mukund 05Nov13, Create an event handler for the master page's contentCallEvent event
			this.Master.contentCallEvent += new EventHandler(Master_ButtonClick);
		}

		private void Master_ButtonClick(object sender, EventArgs e)
		{
			//Mukund 05Nov13, This Method will be Called from Timer Click from Master page


		}
		string page = "BiggestExchangeMailFiles.aspx", control = "TypeComboBox";

        protected void Page_Load(object sender, EventArgs e)
        {
			
            this.BigMailChartControl.Width = new Unit(Convert.ToInt32(chartWidth.Value));
			this.chartItemSizeServer.Width = new Unit(Convert.ToInt32(chartWidth.Value));
			this.ChartMBSizeDatabase.Width = new Unit(Convert.ToInt32(chartWidth.Value));
			this.chartItemSizeDatabase.Width = new Unit(Convert.ToInt32(chartWidth.Value));

			
            if (!IsPostBack)
            {
				serverCombo();
				FillTypeCombobox(page, control);
				//if (Request.QueryString["name"] == "Exchange")
				//{
				//    MessageLabel.Text = "Mail Files For Exchange";
				//}

                TypeComboBox.Text = TypeComboBox.SelectedItem.ToString();
                SetBarGraphForTopMail(ServerComboBox.Text);
				SetBarGraphForItemSizeDatabase(ItemSizeDatabaseComboBox.Text);
				SetBarGraphForItemSizeServer(ItemSizeServercombobox.Text);
				SetBarGraphForMBSizeDatabase(MBSizeDatabasecombobox.Text);
                HtmlGenericControl body = (HtmlGenericControl)Master.FindControl("Body1");
                body.Attributes.Add("onResize", "Resized()");
                body.Attributes.Add("onload", "DoCallback()");
            }
            if (ServerComboBox.Text == "All Servers")
            {
                SetBarGraphForTopMail("All Servers");
            }
            else
            {
                SetBarGraphForTopMail(ServerComboBox.Text);
            }

			if (ItemSizeDatabaseComboBox.Text == "All Databases")
			{
				SetBarGraphForItemSizeDatabase("All Databases");
			}
			else
			{
				SetBarGraphForItemSizeDatabase(ItemSizeDatabaseComboBox.Text);
			}

			if (ItemSizeServercombobox.Text == "All Servers")
			{
				SetBarGraphForItemSizeServer("All Servers");
			}
			else
			{
				SetBarGraphForItemSizeServer(ItemSizeServercombobox.Text);
			}

			if (MBSizeDatabasecombobox.Text == "All Databases")
			{
				SetBarGraphForMBSizeDatabase("All Databases");
			}
			else
			{
				SetBarGraphForMBSizeDatabase(MBSizeDatabasecombobox.Text);
			}
        }
		public void serverCombo()
		{

			DataTable dt = new DataTable();
			dt = VSWebBL.DashboardBL.ExchangeMailFileBL.Ins.FillServerCombobox();
			DataRow dr = dt.NewRow();
			dr[0] = "All Servers";
			dt.Rows.InsertAt(dr, 0);
			dt = dt.DefaultView.ToTable(true, "Server");
			ServerComboBox.DataSource = dt;
			ServerComboBox.TextField = "Server";
			ServerComboBox.ValueField = "Server";
			ServerComboBox.DataBind();
			ServerComboBox.SelectedIndex = 0;


			ItemSizeServercombobox.DataSource = dt;
			ItemSizeServercombobox.TextField = "Server";
			ItemSizeServercombobox.ValueField = "Server";
			ItemSizeServercombobox.DataBind();
			ItemSizeServercombobox.SelectedIndex = 0;

			DataTable dt2 = new DataTable();
			dt2 = VSWebBL.DashboardBL.ExchangeMailFileBL.Ins.FillDBCombobox();
			DataRow dr2 = dt2.NewRow();
			dr2[0] = "All Databases";
			dt2.Rows.InsertAt(dr2, 0);
			dt2 = dt2.DefaultView.ToTable(true, "Database");
			MBSizeDatabasecombobox.DataSource = dt2;
			MBSizeDatabasecombobox.TextField = "Database";
			MBSizeDatabasecombobox.ValueField = "Database";
			MBSizeDatabasecombobox.DataBind();
			MBSizeDatabasecombobox.SelectedIndex = 0;


			ItemSizeDatabaseComboBox.DataSource = dt2;
			ItemSizeDatabaseComboBox.TextField = "Database";
			ItemSizeDatabaseComboBox.ValueField = "Database";
			ItemSizeDatabaseComboBox.DataBind();
			ItemSizeDatabaseComboBox.SelectedIndex = 0;

		}

		//Mailbox size by server
        protected void BigMailChartControl_CustomCallback(object sender, DevExpress.XtraCharts.Web.CustomCallbackEventArgs e)
        {
            this.BigMailChartControl.Width = new Unit(Convert.ToInt32(chartWidth.Value));
            if (ServerComboBox.Text == "All Servers")
            {
                SetBarGraphForTopMail("All Servers");
            }
            else
            {
                SetBarGraphForTopMail(ServerComboBox.Text);
            }
        }
        public void SetBarGraphForTopMail(string servername)
        {
            BigMailChartControl.Series.Clear();
			DataTable dt = VSWebBL.DashboardBL.ExchangeServerDetailsBL.Ins.SetBarGraphForTopMailBox(servername,"");
            Series series1 = new Series("Mail Server", ViewType.Bar);
            series1.ArgumentDataMember = dt.Columns["Title"].ToString();
            ValueDataMemberCollection seriesValueDataMembers = (ValueDataMemberCollection)series1.ValueDataMembers;
			seriesValueDataMembers.AddRange(dt.Columns["TotalItemSizeInMB"].ToString());
            //Addnig series to mailchartbox control
            BigMailChartControl.Series.Add(series1);
            ((XYDiagram)BigMailChartControl.Diagram).PaneLayoutDirection = PaneLayoutDirection.Horizontal;
            XYDiagram seriesXY = (XYDiagram)BigMailChartControl.Diagram;
            //X and Y aixs detals
            seriesXY.AxisY.Title.Text = "Mailbox Size (MB)";
            seriesXY.AxisX.Title.Text = "Mail User";
            seriesXY.AxisY.Title.Visible = true;
            seriesXY.AxisX.Title.Visible = true;
            seriesXY.AxisX.Label.ResolveOverlappingOptions.AllowHide = false;
            seriesXY.AxisX.Label.ResolveOverlappingOptions.AllowRotate = true;
            seriesXY.AxisX.Label.ResolveOverlappingOptions.AllowStagger = true;
            //12/11/2013 NS added
            seriesXY.AxisY.Label.ResolveOverlappingOptions.AllowRotate = false;

            //Enabling the series 
            BigMailChartControl.Legend.Visible = false;
            ChartTitle title = new ChartTitle();
            if (servername == "All")
            {
                title.Text = "All Servers Top 20 Biggest Mail Files";
            }
            else
            {
                title.Text = servername + " Top 20 Biggest Mail Files";
            }

            BigMailChartControl.Titles.Clear();
            BigMailChartControl.Titles.Add(title);
            AxisBase axis = ((XYDiagram)BigMailChartControl.Diagram).AxisX;
            ((BarSeriesView)series1.View).ColorEach = false;
            BigMailChartControl.DataSource = dt;
            BigMailChartControl.DataBind();
        }
        protected void ServerComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetBarGraphForTopMail(ServerComboBox.Text);
        }

		//Item count count by server
		protected void chartItemSizeServer_CustomCallback(object sender, DevExpress.XtraCharts.Web.CustomCallbackEventArgs e)
		{
			this.chartItemSizeServer.Width = new Unit(Convert.ToInt32(chartWidth.Value));
			if (ItemSizeServercombobox.Text == "All Servers")
			{
				SetBarGraphForItemSizeServer("All Servers");
			}
			else
			{
				SetBarGraphForItemSizeServer(ItemSizeServercombobox.Text);
			}
		}
		protected void ItemSizeServercombobox_SelectedIndexChanged(object sender, EventArgs e)
		{
			SetBarGraphForItemSizeServer(ItemSizeServercombobox.Text);
		}
		public void SetBarGraphForItemSizeServer(string servername)
		{
			chartItemSizeServer.Series.Clear();
			DataTable dt = VSWebBL.DashboardBL.ExchangeServerDetailsBL.Ins.SetBarGraphForTopMailBox(servername, "FileSize");
			Series series1 = new Series("Mail Server", ViewType.Bar);
			series1.ArgumentDataMember = dt.Columns["Title"].ToString();
			ValueDataMemberCollection seriesValueDataMembers = (ValueDataMemberCollection)series1.ValueDataMembers;
			seriesValueDataMembers.AddRange(dt.Columns["ItemCount"].ToString());
			//Addnig series to mailchartbox control
			chartItemSizeServer.Series.Add(series1);
			((XYDiagram)chartItemSizeServer.Diagram).PaneLayoutDirection = PaneLayoutDirection.Horizontal;
			XYDiagram seriesXY = (XYDiagram)chartItemSizeServer.Diagram;
			//X and Y aixs detals
			seriesXY.AxisY.Title.Text = "Message Count";
			seriesXY.AxisX.Title.Text = "Mail User";
			seriesXY.AxisY.Title.Visible = true;
			seriesXY.AxisX.Title.Visible = true;
			seriesXY.AxisX.Label.ResolveOverlappingOptions.AllowHide = false;
			seriesXY.AxisX.Label.ResolveOverlappingOptions.AllowRotate = true;
			seriesXY.AxisX.Label.ResolveOverlappingOptions.AllowStagger = true;
			//12/11/2013 NS added
			seriesXY.AxisY.Label.ResolveOverlappingOptions.AllowRotate = false;

			//Enabling the series 
			chartItemSizeServer.Legend.Visible = false;
			ChartTitle title = new ChartTitle();
			if (servername == "All")
			{
				title.Text = "All Servers Top 20 Biggest Message Count";
			}
			else
			{
				title.Text = servername + " Top 20 Biggest Message Count";
			}

			chartItemSizeServer.Titles.Clear();
			chartItemSizeServer.Titles.Add(title);
			AxisBase axis = ((XYDiagram)chartItemSizeServer.Diagram).AxisX;
			((BarSeriesView)series1.View).ColorEach = false;
			chartItemSizeServer.DataSource = dt;
			chartItemSizeServer.DataBind();
		}

		//Mailbox size by database
		protected void ChartMBSizeDatabase_CustomCallback(object sender, DevExpress.XtraCharts.Web.CustomCallbackEventArgs e)
		{
			this.ChartMBSizeDatabase.Width = new Unit(Convert.ToInt32(chartWidth.Value));
			if (MBSizeDatabasecombobox.Text == "All Databases")
			{
				SetBarGraphForMBSizeDatabase("All Databases");
			}
			else
			{
				SetBarGraphForMBSizeDatabase(MBSizeDatabasecombobox.Text);
			}
		}

		protected void MBSizeDatabasecombobox_SelectedIndexChanged(object sender, EventArgs e)
		{
			SetBarGraphForMBSizeDatabase(MBSizeDatabasecombobox.Text);
		}

		public void SetBarGraphForMBSizeDatabase(string servername)
		{
			ChartMBSizeDatabase.Series.Clear();
			DataTable dt = VSWebBL.DashboardBL.ExchangeServerDetailsBL.Ins.SetBarGraphForTopMailDatabase(servername, "");
			Series series1 = new Series("Mail Server", ViewType.Bar);
			series1.ArgumentDataMember = dt.Columns["Title"].ToString();
			ValueDataMemberCollection seriesValueDataMembers = (ValueDataMemberCollection)series1.ValueDataMembers;
			seriesValueDataMembers.AddRange(dt.Columns["TotalItemSizeInMB"].ToString());
			//Addnig series to mailchartbox control
			ChartMBSizeDatabase.Series.Add(series1);
			((XYDiagram)ChartMBSizeDatabase.Diagram).PaneLayoutDirection = PaneLayoutDirection.Horizontal;
			XYDiagram seriesXY = (XYDiagram)ChartMBSizeDatabase.Diagram;
			//X and Y aixs detals
			seriesXY.AxisY.Title.Text = "Mailbox Size (MB)";
			seriesXY.AxisX.Title.Text = "Mail User";
			seriesXY.AxisY.Title.Visible = true;
			seriesXY.AxisX.Title.Visible = true;
			seriesXY.AxisX.Label.ResolveOverlappingOptions.AllowHide = false;
			seriesXY.AxisX.Label.ResolveOverlappingOptions.AllowRotate = true;
			seriesXY.AxisX.Label.ResolveOverlappingOptions.AllowStagger = true;
			//12/11/2013 NS added
			seriesXY.AxisY.Label.ResolveOverlappingOptions.AllowRotate = false;

			//Enabling the series 
			ChartMBSizeDatabase.Legend.Visible = false;
			ChartTitle title = new ChartTitle();
			if (servername == "All")
			{
				title.Text = "All Servers Top 20 Biggest Mail Files";
			}
			else
			{
				title.Text = servername + " Top 20 Biggest Mail Files";
			}

			ChartMBSizeDatabase.Titles.Clear();
			ChartMBSizeDatabase.Titles.Add(title);
			AxisBase axis = ((XYDiagram)ChartMBSizeDatabase.Diagram).AxisX;
			((BarSeriesView)series1.View).ColorEach = false;
			ChartMBSizeDatabase.DataSource = dt;
			ChartMBSizeDatabase.DataBind();
		}

		//Item count by database
		protected void chartItemSizeDatabase_CustomCallback(object sender, DevExpress.XtraCharts.Web.CustomCallbackEventArgs e)
		{
			this.chartItemSizeDatabase.Width = new Unit(Convert.ToInt32(chartWidth.Value));
			if (ItemSizeDatabaseComboBox.Text == "All Databases")
			{
				SetBarGraphForItemSizeDatabase("All Databases");
			}
			else
			{
				SetBarGraphForItemSizeDatabase(ItemSizeDatabaseComboBox.Text);
			}
		}

		protected void ItemSizeDatabaseComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			SetBarGraphForItemSizeDatabase(ItemSizeDatabaseComboBox.Text);
		}

		public void SetBarGraphForItemSizeDatabase(string servername)
		{
			chartItemSizeDatabase.Series.Clear();
			DataTable dt = VSWebBL.DashboardBL.ExchangeServerDetailsBL.Ins.SetBarGraphForTopMailDatabase(servername, "FileSize");
			Series series1 = new Series("Mail Server", ViewType.Bar);
			series1.ArgumentDataMember = dt.Columns["Title"].ToString();
			ValueDataMemberCollection seriesValueDataMembers = (ValueDataMemberCollection)series1.ValueDataMembers;
			seriesValueDataMembers.AddRange(dt.Columns["ItemCount"].ToString());
			//Addnig series to mailchartbox control
			chartItemSizeDatabase.Series.Add(series1);
			((XYDiagram)chartItemSizeDatabase.Diagram).PaneLayoutDirection = PaneLayoutDirection.Horizontal;
			XYDiagram seriesXY = (XYDiagram)chartItemSizeDatabase.Diagram;
			//X and Y aixs detals
			seriesXY.AxisY.Title.Text = "Message Count";
			seriesXY.AxisX.Title.Text = "Mail User";
			seriesXY.AxisY.Title.Visible = true;
			seriesXY.AxisX.Title.Visible = true;
			seriesXY.AxisX.Label.ResolveOverlappingOptions.AllowHide = false;
			seriesXY.AxisX.Label.ResolveOverlappingOptions.AllowRotate = true;
			seriesXY.AxisX.Label.ResolveOverlappingOptions.AllowStagger = true;
			//12/11/2013 NS added
			seriesXY.AxisY.Label.ResolveOverlappingOptions.AllowRotate = false;

			//Enabling the series 
			chartItemSizeDatabase.Legend.Visible = false;
			ChartTitle title = new ChartTitle();
			if (servername == "All")
			{
				title.Text = "All Servers Top 20 Biggest Message Count";
			}
			else
			{
				title.Text = servername + " Top 20 Biggest Message Count";
			}

			chartItemSizeDatabase.Titles.Clear();
			chartItemSizeDatabase.Titles.Add(title);
			AxisBase axis = ((XYDiagram)chartItemSizeDatabase.Diagram).AxisX;
			((BarSeriesView)series1.View).ColorEach = false;
			chartItemSizeDatabase.DataSource = dt;
			chartItemSizeDatabase.DataBind();
		}

		public void FillTypeCombobox(string page, string control)
		{

			DataTable Typetab = VSWebBL.DashboardBL.KeyMetricsBL.Ins.GetspecificServerType(page, control);
			TypeComboBox.DataSource = Typetab;
			TypeComboBox.TextField = "ServerType";
			TypeComboBox.DataBind();
			//TypeComboBox.SelectedIndex = 0;
            //try
            //{
            //    TypeComboBox.SelectedItem = TypeComboBox.Items.FindByText("Domino");
            //}
            //catch
            //{
            //    TypeComboBox.SelectedIndex = 0;
            //}
            //15/2/2016 Sowmya Added for VSPLUS 2610
            if (Request.QueryString["type"] != null)
            {
                TypeComboBox.SelectedItem = TypeComboBox.Items.FindByText(Request.QueryString["type"]);
            }

		}
		protected void TypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (TypeComboBox.Text == "Domino")
			{
                //5/21/2015 NS modified for VSPLUS-1773
                if (!Page.IsCallback)
                {
                    Response.Redirect("~/Dashboard/BiggestMailFiles.aspx", false);
                }
			}
			else
			{
				SetBarGraphForTopMail(TypeComboBox.Text);
			}
		}
    }
}