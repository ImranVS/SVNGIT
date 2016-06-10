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
    public partial class BiggestMailFiles : System.Web.UI.Page
    {
		protected void Page_PreInit(object sender, EventArgs e)
        {
           
            this.Master.contentCallEvent += new EventHandler(Master_ButtonClick);
        }

		private void Master_ButtonClick(object sender, EventArgs e)
		{
		}       

		string page = "BiggestMailFiles.aspx", control = "TypeComboBox";

        protected void Page_Load(object sender, EventArgs e)
        {
            this.BigMailChartControl.Width = new Unit(Convert.ToInt32(chartWidth.Value));

            if (!IsPostBack)
            {
                serverCombo();
				FillTypeCombobox(page,control);
                //TypeComboBox.Text = "--Select Server Type--";
                SetBarGraphForTopMail(ServerComboBox.Text);
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
           
        }

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
            DataTable  dt = VSWebBL.DashboardBL.DominoServerHealthBLL.Ins.SetBarGraphForTopMailBox(servername);
            Series series1 = new Series("Mail Server", ViewType.Bar);
            series1.ArgumentDataMember = dt.Columns["Title"].ToString();
            ValueDataMemberCollection seriesValueDataMembers = (ValueDataMemberCollection)series1.ValueDataMembers;
            seriesValueDataMembers.AddRange(dt.Columns["FileSize"].ToString());
            //Addnig series to mailchartbox control
            BigMailChartControl.Series.Add(series1);
            ((XYDiagram)BigMailChartControl.Diagram).PaneLayoutDirection = PaneLayoutDirection.Horizontal;
            XYDiagram seriesXY = (XYDiagram)BigMailChartControl.Diagram;
            //X and Y aixs detals
            seriesXY.AxisY.Title.Text = "File Size (MB)";
            seriesXY.AxisX.Title.Text = "Database Name";
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
		protected void TypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (TypeComboBox.Text == "Exchange")
			{
                //15/2/2016 Sowmya Added for VSPLUS 2610
                if (!Page.IsCallback)
                {
                    Response.Redirect("~/Dashboard/BiggestExchangeMailFiles.aspx?type=" + TypeComboBox.Text, false);
                }
			}
			else
			{
				SetBarGraphForTopMail(TypeComboBox.Text);
			}
		}
		
		//Response.Redirect("~/Configurator/WebSphereSeverGrid.aspx", false)
        public void serverCombo()
        {
            DataTable dt = new DataTable();
            dt = VSWebBL.DashboardBL.mailFileBL.Ins.FillServerCombobox();
            DataRow dr = dt.NewRow();
            dr[0] = "All Servers";
            dt.Rows.InsertAt(dr, 0);
            dt = dt.DefaultView.ToTable(true, "Server");
            ServerComboBox.DataSource = dt;
            ServerComboBox.TextField = "Server";
            ServerComboBox.ValueField ="Server";
            ServerComboBox.DataBind();
            ServerComboBox.SelectedIndex = 0;           
        }
		public void FillTypeCombobox(string page, string control)
		{
			//page = "BiggestMailFiles.aspx";
			//control = "TypeComboBox";
			DataTable Typetab = VSWebBL.DashboardBL.KeyMetricsBL.Ins.GetspecificServerType(page, control);
			TypeComboBox.DataSource = Typetab;
			TypeComboBox.TextField = "ServerType";
			TypeComboBox.DataBind();
			//TypeComboBox.SelectedIndex = 0;
			try
			{
				TypeComboBox.SelectedItem = TypeComboBox.Items.FindByText("Domino");
			}
			catch
			{
				TypeComboBox.SelectedIndex = 0;
			}
		}
    }
}