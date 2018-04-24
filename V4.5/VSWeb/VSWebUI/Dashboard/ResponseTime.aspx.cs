using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DevExpress.XtraCharts;
using DevExpress.Web;
using System.Drawing;
using VSWebBL;
using System.Web.UI.HtmlControls;

namespace VSWebUI
{
    public partial class WebForm7 : System.Web.UI.Page
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
           
            this.ResponseWebChartControl.Width = new Unit(Convert.ToInt32(chartWidth.Value));

            if (!IsPostBack && !IsCallback)
            {
                FillCombobox();
                //2/13/2015 NS added for VSPLUS-1346
                FillLocationCombobox();
                GraphforResponseTime("Server", "", "All","All");

                HtmlGenericControl body = (HtmlGenericControl)Master.FindControl("Body1");
                body.Attributes.Add("onload", "DoCallback()");
                body.Attributes.Add("onResize", "Resized()");

            }
            GraphforResponseTime(SortRadioButtonList1.SelectedItem.Value.ToString(), SearchTextBox.Text, 
                TypeComboBox.SelectedItem.Value.ToString(),LocationComboBox.SelectedItem.Value.ToString());
        }

        public void GraphforResponseTime(string param,string sname,string stype, string location)
        {
            ResponseWebChartControl.Series.Clear();
            DataTable dt = VSWebBL.DashboardBL.KeyMetricsBL.Ins.GetResponseTime(param,sname,stype,location);
            
            Series series = new Series("usercount", ViewType.Bar);

            series.ArgumentDataMember = dt.Columns["Server"].ToString();
            //10/30/2013 NS added - point labels on series
            series.LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;

            ValueDataMemberCollection seriesValueDataMembers = (ValueDataMemberCollection)series.ValueDataMembers;
            seriesValueDataMembers.AddRange(dt.Columns["ResponseTime"].ToString());
            ResponseWebChartControl.Series.Add(series);

            ResponseWebChartControl.Legend.Visible = false;
           
            ((SideBySideBarSeriesView)series.View).ColorEach = true;

            //AxisBase axisx = ((XYDiagram)deviceTypeWebChart.Diagram).AxisX;

            XYDiagram seriesXY = (XYDiagram)ResponseWebChartControl.Diagram;
           // seriesXY.AxisX.Title.Text = "Milliseconds";
           // seriesXY.AxisX.Title.Visible = true;
            ((DevExpress.XtraCharts.XYDiagram)ResponseWebChartControl.Diagram).Rotated = true;           
            //11/16/2013 NS modified the AllowRotate option to false, otherwise the server names were unreadable
            seriesXY.AxisX.Label.ResolveOverlappingOptions.AllowRotate = false;
            seriesXY.AxisX.Label.ResolveOverlappingOptions.AllowHide = false;
            seriesXY.AxisY.Title.Text = "Milliseconds";
           seriesXY.AxisY.Title.Visible = true;
           
           AxisBase axisy = ((XYDiagram)ResponseWebChartControl.Diagram).AxisY;
           //4/18/2014 NS modified for VSPLUS-312
           axisy.Range.AlwaysShowZeroLevel = true;
           double min = Convert.ToDouble(((XYDiagram)ResponseWebChartControl.Diagram).AxisY.Range.MinValue);
           double max = Convert.ToDouble(((XYDiagram)ResponseWebChartControl.Diagram).AxisY.Range.MaxValue);

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
                    ResponseWebChartControl.Height = 200;
                }
                else
                {
                    if (dt.Rows.Count >= 4 && dt.Rows.Count < 10)
                    {
                        ResponseWebChartControl.Height = ((dt.Rows.Count) * 50) + 20;
                    }
                    else
                    {
                        if (dt.Rows.Count >= 10 && dt.Rows.Count < 100)
                        {
                            ResponseWebChartControl.Height = ((dt.Rows.Count) * 40) + 20;
                        }
                        else
                        {
                            if (dt.Rows.Count >= 100)
                            {
                                ResponseWebChartControl.Height = ((dt.Rows.Count) * 20) + 20;
                            }
                        }
                    }
                }
            }
            //ResponseWebChartControl.Width = new Unit(1000);
            ResponseWebChartControl.DataSource = dt;
            ResponseWebChartControl.DataBind();

        }

        protected void SortRadioButtonList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            GraphforResponseTime(SortRadioButtonList1.SelectedItem.Value.ToString(),SearchTextBox.Text,
                TypeComboBox.Text,LocationComboBox.SelectedItem.Value.ToString());
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
            try
            {
                TypeComboBox.SelectedItem = TypeComboBox.Items.FindByText("Domino");
            }
            catch
            {
                TypeComboBox.SelectedIndex = 0;
            }
        }

        public void FillLocationCombobox()
        {
            DataTable dt = VSWebBL.DashboardBL.KeyMetricsBL.Ins.GetLocation();
            LocationComboBox.DataSource = dt;
            LocationComboBox.TextField = "Location";
            LocationComboBox.DataBind();
            LocationComboBox.SelectedIndex = 0;
        }



        protected void GoButton_Click(object sender, EventArgs e)
        {
            GraphforResponseTime(SortRadioButtonList1.SelectedItem.Value.ToString(), SearchTextBox.Text, 
                TypeComboBox.Text,LocationComboBox.SelectedItem.Value.ToString());
        }

        protected void ResponseWebChartControl_CustomCallback(object sender, DevExpress.XtraCharts.Web.CustomCallbackEventArgs e)
        {
            //2/13/2013 NS commented out
            ResponseWebChartControl.Width = new Unit(Convert.ToInt32(chartWidth.Value));
            GraphforResponseTime(SortRadioButtonList1.SelectedItem.Value.ToString(), SearchTextBox.Text, TypeComboBox.Text,LocationComboBox.SelectedItem.Value.ToString());
        }

      
    }
}