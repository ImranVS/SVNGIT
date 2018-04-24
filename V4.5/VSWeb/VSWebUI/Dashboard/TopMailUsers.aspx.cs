using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using DevExpress.XtraCharts;
using DevExpress.XtraCharts.Web;
namespace VSWebUI
{
	public partial class TopMailUsers : System.Web.UI.Page
    {
		string maxValue;
		protected void Page_PreInit(object sender, EventArgs e)
        {
           
            this.Master.contentCallEvent += new EventHandler(Master_ButtonClick);
        }

		private void Master_ButtonClick(object sender, EventArgs e)
		{
		}       

		

        protected void Page_Load(object sender, EventArgs e)
        {
			this.TopMailChartControl.Width = new Unit(Convert.ToInt32(chartWidth.Value));

            if (!IsPostBack)
            {
                serverCombo();
				
				
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

		protected void TopMailChartControl_CustomCallback(object sender, DevExpress.XtraCharts.Web.CustomCallbackEventArgs e)
        {
			this.TopMailChartControl.Width = new Unit(Convert.ToInt32(chartWidth.Value));
        }
		
		public void SetBarGraphForTopMail(string servername)
        {
            double minLavel = 0, maxlevel = 0;
			TopMailChartControl.Series.Clear();
			string radiovalue1 = "%"+CountorSizeRadioButtonList.SelectedItem.Value.ToString()+"%";
			string radiovalue2 = "%" + SentorReceivedRadioButtonList.SelectedItem.Value.ToString() + "%";
			DataTable dt = VSWebBL.DashboardBL.DominoServerHealthBLL.Ins.SetGraphForTopMailUsers(servername,radiovalue1,radiovalue2);

            if (dt.Rows.Count > 0)
            {

                minLavel = Convert.ToDouble(dt.Select("StatValue=min(StatValue)")[0][0]);
                maxlevel = Convert.ToDouble(dt.Select("StatValue=max(StatValue)")[0][0]);

            }
            for (int i = dt.Rows.Count - 1; i >= 0; i--)
            {
                DataRow dr = dt.Rows[i];
                string newrow = dt.Rows[i]["StatName"].ToString();
                newrow = newrow.Split('.')[1];
                dt.Rows[i]["StatName"] = newrow;
            }
			Series series1 = new Series("Mail Server", ViewType.Bar);
            series1.ArgumentDataMember = "StatName";
            TopMailChartControl.DataSource = dt;
            TopMailChartControl.DataBind();
			ValueDataMemberCollection seriesValueDataMembers = (ValueDataMemberCollection)series1.ValueDataMembers;
            seriesValueDataMembers.AddRange("StatValue");
			TopMailChartControl.Series.Add(series1);
            series1.LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;
            //series1.CrosshairEnabled = DevExpress.Utils.DefaultBoolean.True;
            //series1.CrosshairLabelVisibility = DevExpress.Utils.DefaultBoolean.True;
            //series1.CrosshairLabelPattern = "{A: V}";
            //series1.CrosshairHighlightPoints = DevExpress.Utils.DefaultBoolean.True;
            
            XYDiagram seriesXY = (XYDiagram)TopMailChartControl.Diagram;
            if (seriesXY != null)
            {
                seriesXY.AxisY.NumericOptions.Format = NumericFormat.Number;
                seriesXY.AxisY.NumericOptions.Precision = 0;

                seriesXY.AxisY.Title.Visible = true;
                ChartTitle title = new ChartTitle();
                if (servername == "All")
                {
                    title.Text = "All Servers - Top 10  Mail Users";
                }
                else
                {
                    title.Text = servername + " - Top 10  Mail Users";
                }
                TopMailChartControl.Titles.Clear();
                TopMailChartControl.Titles.Add(title);
                //X and Y aixs detals
                //2/5/2016 NS modified for VSPLUS-2562
                UI uiobj = new UI();
                if (CountorSizeRadioButtonList.SelectedItem.Value.ToString() == "Count")
                {
                    seriesXY.AxisY.Title.Text = SentorReceivedRadioButtonList.SelectedItem.Value.ToString() + "  Mail  " + CountorSizeRadioButtonList.SelectedItem.Value.ToString();
                    uiobj.RecalibrateChartAxes(seriesXY, "Y", "int", "int");
                }
                else
                {
                    seriesXY.AxisY.Title.Text = SentorReceivedRadioButtonList.SelectedItem.Value.ToString() + "  Mail  " + CountorSizeRadioButtonList.SelectedItem.Value.ToString() + " (MB)";
                    uiobj.RecalibrateChartAxes(seriesXY, "Y", "double", "double");
                }
            }
            //((XYDiagram)TopMailChartControl.Diagram).PaneLayoutDirection = PaneLayoutDirection.Horizontal;
    	}

       

        protected void ServerComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
           
			SetBarGraphForTopMail(ServerComboBox.Text);
        }
		
		
		
        public void serverCombo()
        {
            DataTable dt = new DataTable();
            dt = VSWebBL.DashboardBL.mailFileBL.Ins.GetServerNames();
			if (dt.Rows.Count > 0)
			{
				DataRow dr = dt.NewRow();
				dr[0] = "All Servers";
				dt.Rows.InsertAt(dr, 0);
				dt = dt.DefaultView.ToTable(true, "ServerName");
				ServerComboBox.DataSource = dt;
				ServerComboBox.TextField = "ServerName";
				ServerComboBox.ValueField = "ServerName";
				ServerComboBox.DataBind();
				ServerComboBox.SelectedIndex = 0;
			}
        }
		protected void CountorSizeRadioButtonList_SelectedIndexChanged(object sender, EventArgs e)
		{
			SetBarGraphForTopMail(ServerComboBox.Text);
		
		}
		protected void SentorReceivedRadioButtonList_SelectedIndexChanged(object sender, EventArgs e)
		{
			SetBarGraphForTopMail(ServerComboBox.Text);
		
		}

        protected void TopMailChartControl_BoundDataChanged(object sender, EventArgs e)
        {
            SetBarGraphForTopMail(ServerComboBox.Text);

        }
    }
}