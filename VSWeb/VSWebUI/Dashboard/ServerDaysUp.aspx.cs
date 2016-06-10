using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Drawing;
using System.Data;
using DevExpress.XtraCharts;

namespace VSWebUI.Dashboard
{
    public partial class ServerDaysUp : System.Web.UI.Page
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
            this.ServerUpWebChartControl.Width = new Unit(Convert.ToInt32(chartWidth.Value));
            Page.Title = "Server Days Up";
            if (!IsPostBack && !IsCallback)
            {
                FillCombobox();
                GraphForDaysUp("Server","Domino");
                HtmlGenericControl body = (HtmlGenericControl)Master.FindControl("Body1");
                body.Attributes.Add("onload", "DoCallback()");
                body.Attributes.Add("onResize", "Resized()");
            }
			if (TypeComboBox.Value != null && SortRadioButtonList.Value!=null)
			{
				GraphForDaysUp(SortRadioButtonList.Value.ToString(), TypeComboBox.Value.ToString());
			}
        }
        public void FillCombobox()
        {
            DataTable Typetab = VSWebBL.DashboardBL.KeyMetricsBL.Ins.GetType();
            TypeComboBox.DataSource = Typetab;
            TypeComboBox.TextField = "Type";
            TypeComboBox.DataBind();
            //9/26/2013 NS commented out the option All in order to display a manageable amount of data per page
            //TypeComboBox.Items.Insert(0, new DevExpress.Web.ListEditItem("All", 0));
            //TypeComboBox.Items[0].Selected = true;
            //TypeComboBox.SelectedItem = TypeComboBox.Items.FindByText("Domino");
            if (TypeComboBox.Items.Count > 1)
            {
                TypeComboBox.Items[1].Selected = true;
            }
            else if (TypeComboBox.Items.Count > 0)
            {
                TypeComboBox.Items[0].Selected = true;
            }
        }
        public void GraphForDaysUp(string sortby,string stype)
        {
            ServerUpWebChartControl.Series.Clear();
            DataTable dt = VSWebBL.DashboardBL.KeyMetricsBL.Ins.GetServerDaysUp(sortby,stype);

            Series series = new Series("Duration", ViewType.Bar);

            series.ArgumentDataMember = dt.Columns["Server"].ToString();
            series.Label.PointOptions.ValueNumericOptions.Format = NumericFormat.General;

            ((SideBySideBarSeriesView)series.View).ColorEach = true;
            ((SideBySideBarSeriesLabel)series.Label).LineVisible = true;
            ((SideBySideBarSeriesLabel)series.Label).TextOrientation = TextOrientation.Horizontal;
            ((SideBySideBarSeriesLabel)series.Label).PointOptions.PointView = PointView.Values;
            ValueDataMemberCollection seriesValueDataMembers = (ValueDataMemberCollection)series.ValueDataMembers;
            seriesValueDataMembers.AddRange(dt.Columns["Duration"].ToString());

            series.LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;

            ServerUpWebChartControl.Series.Add(series);
            ServerUpWebChartControl.SeriesTemplate.Label.PointOptions.PointView = PointView.Values;
            ServerUpWebChartControl.SeriesTemplate.LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;
            ServerUpWebChartControl.Legend.Visible = false;

            XYDiagram seriesXY = (XYDiagram)ServerUpWebChartControl.Diagram;
            ((DevExpress.XtraCharts.XYDiagram)ServerUpWebChartControl.Diagram).Rotated = true;
            //11/16/2013 NS modified the AllowRotate option to false, otherwise the server names were unreadable
            seriesXY.AxisX.Label.ResolveOverlappingOptions.AllowRotate = false;
            seriesXY.AxisX.Label.ResolveOverlappingOptions.AllowHide = false;
            
            AxisBase axisy = ((XYDiagram)ServerUpWebChartControl.Diagram).AxisY;
            //4/18/2014 NS modified for VSPLUS-312
            axisy.Range.AlwaysShowZeroLevel = true;

            //2/6/2013 NS modified chart height calculations based on the number of rows
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows.Count < 10)
                {
                    if (dt.Rows.Count == 1)
                    {
                        ServerUpWebChartControl.Height = 100;
                    }
                    else
                    {
                        ServerUpWebChartControl.Height = ((dt.Rows.Count) * 60) + 20;
                    }
                }
                else
                {
                    if (dt.Rows.Count >= 10 && dt.Rows.Count < 100)
                    {
                        ServerUpWebChartControl.Height = ((dt.Rows.Count) * 40) + 20;
                    }
                    else
                    {
                        if (dt.Rows.Count >= 100)
                        {
                            //11/18/2013 NS modified - the height needs to be greater than *10
                            //ServerUpWebChartControl.Height = ((dt.Rows.Count) * 10) + 20;
                            ServerUpWebChartControl.Height = ((dt.Rows.Count) * 20) + 20;
                        }
                    }
                }
            }
            ServerUpWebChartControl.DataSource = dt;
            ServerUpWebChartControl.DataBind();       
        }

        protected void ServerUpWebChartControl_CustomCallback(object sender, DevExpress.XtraCharts.Web.CustomCallbackEventArgs e)
        {
            this.ServerUpWebChartControl.Width = new Unit(Convert.ToInt32(chartWidth.Value));
        }

		protected void SortRadioButtonList_SelectedIndexChanged(object sender, EventArgs e)
		{
			
				GraphForDaysUp(SortRadioButtonList.SelectedItem.Value.ToString(), TypeComboBox.SelectedItem.Value.ToString());
			
		}
        protected void GoButton_Click(object sender, EventArgs e)
        {
            GraphForDaysUp(SortRadioButtonList.SelectedItem.Value.ToString(), TypeComboBox.SelectedItem.Value.ToString());
        }
        //7/9/2015 NS added for VSPLUS-1973
        protected void ServerUpWebChartControl_BoundDataChanged(object sender, EventArgs e)
        {
            UI uiobj = new UI();
            uiobj.RecalibrateChartAxes(ServerUpWebChartControl.Diagram);
        }
		//protected void TypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
		//{
		//    if (SortRadioButtonList.SelectedItem.Value != null && SortRadioButtonList.SelectedItem.Value != "")
		//    {
		//        GraphForDaysUp(SortRadioButtonList.SelectedItem.Value.ToString(), TypeComboBox.SelectedItem.Value.ToString());
		//    }
		//    else
		//    {
		//        GraphForDaysUp(SortRadioButtonList.Value.ToString(), TypeComboBox.SelectedItem.Value.ToString());
		//    }
			
		//}
    }
}