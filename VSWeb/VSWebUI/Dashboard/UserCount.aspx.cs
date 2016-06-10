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
using VSWebBL;

namespace VSWebUI
{
    public partial class UserCount : System.Web.UI.Page
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
            this.UserCountWebChartControl.Width = new Unit(Convert.ToInt32(chartWidth.Value));
            Page.Title = "User Count";
            if (!IsPostBack && !IsCallback)
            {
                //2/4/2015 NS added
                FillCombobox();
                GraphforUsercount("Server", TypeComboBox.SelectedItem.Value.ToString());
                HtmlGenericControl body = (HtmlGenericControl)Master.FindControl("Body1");
                body.Attributes.Add("onload", "DoCallback()");
                body.Attributes.Add("onResize", "Resized()");

            }
            else
            {
                GraphforUsercount(SortRadioButtonList1.SelectedItem.Value.ToString(), TypeComboBox.SelectedItem.Value.ToString());
            }
        }

        //2/4/2015 NS added for
        public void FillCombobox()
        {
            //10/20/2015 NS modified for VSPLUS-2072
            DataTable Typetab = VSWebBL.DashboardBL.KeyMetricsBL.Ins.GetType("'Domino','Exchange','Sharepoint'");
            TypeComboBox.DataSource = Typetab;
            TypeComboBox.TextField = "Type";
            TypeComboBox.DataBind();
            //9/26/2013 NS commented out the option All in order to display a manageable amount of data per page
            //TypeComboBox.Items.Insert(0, new DevExpress.Web.ListEditItem("All", 0));
            //TypeComboBox.Items[0].Selected = true;
            try
            {
                TypeComboBox.SelectedItem = TypeComboBox.Items.FindByText("Domino");
            }
            catch
            {
                TypeComboBox.SelectedIndex = 0;
            }
        }

        public void GraphforUsercount(string param, string stype)
        {
            UserCountWebChartControl.Series.Clear();
            DataTable dt = VSWebBL.DashboardBL.KeyMetricsBL.Ins.GetUserCount(param,stype);

            Series series = new Series("usercount", ViewType.Bar);

            series.ArgumentDataMember = dt.Columns["Server"].ToString();
            series.Label.PointOptions.ValueNumericOptions.Format = NumericFormat.Number;
            series.Label.PointOptions.ValueNumericOptions.Precision = 0;
            //10/30/2013 NS added - point labels on series
            series.LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;

            ((SideBySideBarSeriesView)series.View).ColorEach = true;
            ((SideBySideBarSeriesLabel)series.Label).LineVisible = true;
            ((SideBySideBarSeriesLabel)series.Label).TextOrientation = TextOrientation.Horizontal;
            ((SideBySideBarSeriesLabel)series.Label).PointOptions.PointView = PointView.Values;
            ValueDataMemberCollection seriesValueDataMembers = (ValueDataMemberCollection)series.ValueDataMembers;
            seriesValueDataMembers.AddRange(dt.Columns["UserCount"].ToString());
          
            UserCountWebChartControl.Series.Add(series);
            UserCountWebChartControl.SeriesTemplate.Label.PointOptions.PointView = PointView.Values;
            UserCountWebChartControl.SeriesTemplate.LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;
            UserCountWebChartControl.Legend.Visible = false;

            

            //AxisBase axisx = ((XYDiagram)deviceTypeWebChart.Diagram).AxisX;

            XYDiagram seriesXY = (XYDiagram)UserCountWebChartControl.Diagram;
            // seriesXY.AxisX.Title.Text = "Date and Time";
            //seriesXY.AxisX.Title.Visible = true;
            ((DevExpress.XtraCharts.XYDiagram)UserCountWebChartControl.Diagram).Rotated = true;

            seriesXY.AxisX.Label.ResolveOverlappingOptions.AllowRotate = true;
            seriesXY.AxisX.Label.ResolveOverlappingOptions.AllowHide = false;
           
            // seriesXY.AxisY.Title.Text = "count";
            //seriesXY.AxisY.Title.Visible = true;

            AxisBase axisy = ((XYDiagram)UserCountWebChartControl.Diagram).AxisY;
            //4/18/2014 NS modified for VSPLUS-312
            axisy.Range.AlwaysShowZeroLevel = true;
            seriesXY.AxisY.GridSpacingAuto = false;
            //10/20/2015 NS modified for VSPLUS-2072
            UI uiobj = new UI();
            uiobj.RecalibrateChartAxes(UserCountWebChartControl.Diagram);
            //2/6/2013 NS modified chart height calculations based on the number of rows
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows.Count < 10)
                {
                    if (dt.Rows.Count == 1)
                    {
                        UserCountWebChartControl.Height = 100;
                    }
                    else
                    {
                        UserCountWebChartControl.Height = ((dt.Rows.Count) * 60) + 20;
                    }
                }
                else
                {
                    if (dt.Rows.Count >= 10 && dt.Rows.Count < 100)
                    {
                        UserCountWebChartControl.Height = ((dt.Rows.Count) * 40) + 20;
                    }
                    else
                    {
                        if (dt.Rows.Count >= 100)
                        {
                            UserCountWebChartControl.Height = ((dt.Rows.Count) * 10) + 20;
                        }
                    }
                }
            }
            UserCountWebChartControl.DataSource = dt;
            UserCountWebChartControl.DataBind();       
        
        }

        protected void SortRadioButtonList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            GraphforUsercount(SortRadioButtonList1.SelectedItem.Value.ToString(), TypeComboBox.SelectedItem.Value.ToString());
        }

        protected void UserCountWebChartControl_CustomCallback(object sender, DevExpress.XtraCharts.Web.CustomCallbackEventArgs e)
        {
            this.UserCountWebChartControl.Width = new Unit(Convert.ToInt32(chartWidth.Value));
        }

        protected void GoButton_Click(object sender, EventArgs e)
        {
            GraphforUsercount(SortRadioButtonList1.SelectedItem.Value.ToString(), TypeComboBox.SelectedItem.Value.ToString());
        }
        //7/9/2015 NS added for VSPLUS-1973
        protected void UserCountWebChartControl_BoundDataChanged(object sender, EventArgs e)
        {
            UI uiobj = new UI();
            uiobj.RecalibrateChartAxes(UserCountWebChartControl.Diagram);
        }



    }
}