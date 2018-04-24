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
    public partial class CostperUserserved : System.Web.UI.Page
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
           
            this.CostperUserservedWebChartControl.Width = new Unit(Convert.ToInt32(chartWidth.Value));
            fillUserTypeCombo();
            if (!IsPostBack && !IsCallback)
            {
                FillCombobox();
                //2/13/2015 NS added for VSPLUS-1346
               
                GraphforResponseTime("Server", "", "All","");
                //HtmlGenericControl body = (HtmlGenericControl)Master.FindControl("Body1");
                //body.Attributes.Add("onload", "DoCallback()");
                //body.Attributes.Add("onResize", "Resized()");

            }
            GraphforResponseTime(SortRadioButtonList1.SelectedItem.Value.ToString(), SearchTextBox.Text,
                TypeComboBox.SelectedItem.Value.ToString(),UserTypeComboBox.Text);
            GetUserType();
        }

        public void GraphforResponseTime(string param, string sname, string stype, string UserType)
        {
            CostperUserservedWebChartControl.Series.Clear();
            DataTable dt = new DataTable();
            if (TypeComboBox.Text == "Exchange")
            {
                UserType = UserTypeComboBox.Text;

                dt = VSWebBL.DashboardBL.KeyMetricsBL.Ins.GetCostperuserserveddata(param, sname, stype, UserType);
            }
            else
            {
                dt = VSWebBL.DashboardBL.KeyMetricsBL.Ins.GetCostperuserserveddataForDomino(param, sname, stype);

            }


            for (int i = 0; i < dt.Rows.Count; i++)
            {
                decimal avgusers = Convert.ToDecimal(dt.Rows[i]["StatValue"].ToString() == "" ? "0" : dt.Rows[i]["StatValue"].ToString());//dt.Rows[i]["MonthlyOperatingCost"]
                decimal MonthlyopeartingCost = Convert.ToDecimal(dt.Rows[i]["MonthlyOperatingCost"].ToString() == "" ? "0" : dt.Rows[i]["MonthlyOperatingCost"].ToString());
                if (avgusers != 0)
                    dt.Rows[i]["StatValue"] = Math.Round((MonthlyopeartingCost / avgusers), 2);
                else
                    dt.Rows[i]["StatValue"] = "0";

            }
            DataView dv = dt.AsDataView();
            if (SortRadioButtonList1.Value.ToString() == "1")
            {
                dv.Sort = "ServerName DESC";
            }
            else
            {
                dv.Sort = "StatValue ASC";
            }

            DataTable sortedDT = dv.ToTable();
            Series series = new Series("usercount", ViewType.Bar);

            series.ArgumentDataMember = "servername";
            //10/30/2013 NS added - point labels on series
            series.LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;

            ValueDataMemberCollection seriesValueDataMembers = (ValueDataMemberCollection)series.ValueDataMembers;
            seriesValueDataMembers.AddRange("StatValue");
            CostperUserservedWebChartControl.Series.Add(series);

            CostperUserservedWebChartControl.Legend.Visible = false;

            ((SideBySideBarSeriesView)series.View).ColorEach = true;

            //AxisBase axisx = ((XYDiagram)deviceTypeWebChart.Diagram).AxisX;

            XYDiagram seriesXY = (XYDiagram)CostperUserservedWebChartControl.Diagram;
            // seriesXY.AxisX.Title.Text = "Milliseconds";
            // seriesXY.AxisX.Title.Visible = true;
            ((DevExpress.XtraCharts.XYDiagram)CostperUserservedWebChartControl.Diagram).Rotated = true;
            //11/16/2013 NS modified the AllowRotate option to false, otherwise the server names were unreadable
            seriesXY.AxisX.Label.ResolveOverlappingOptions.AllowRotate = false;
            seriesXY.AxisX.Label.ResolveOverlappingOptions.AllowHide = false;
            seriesXY.AxisY.Title.Text = "Cost Per User Served";
            seriesXY.AxisY.Title.Visible = true;

            AxisBase axisy = ((XYDiagram)CostperUserservedWebChartControl.Diagram).AxisY;
            //4/18/2014 NS modified for VSPLUS-312
            axisy.Range.AlwaysShowZeroLevel = true;
            //series.Label.PointOptions.ValueNumericOptions.Format = NumericFormat.Number;
            //series.Label.PointOptions.ValueNumericOptions.Precision = 0;
            axisy.Range.AlwaysShowZeroLevel = true;
            seriesXY.AxisY.GridSpacingAuto = true;
            //double min = Convert.ToDouble(((XYDiagram)CostperUserservedWebChartControl.Diagram).AxisY.Range.MinValue);
            //double max = Convert.ToDouble(((XYDiagram)CostperUserservedWebChartControl.Diagram).AxisY.Range.MaxValue);

            //int gs = (int)((max - min) / 5);

            //if (gs == 0)
            //    gs = 1;

            //((XYDiagram)CostperUserservedWebChartControl.Diagram).AxisY.GridSpacingAuto = false;
            //((XYDiagram)CostperUserservedWebChartControl.Diagram).AxisY.GridSpacing = gs;

            //2/6/2013 NS modified chart height calculations based on the number of rows
            if (sortedDT.Rows.Count > 0)
            {
                if (sortedDT.Rows.Count < 4)
                {
                    CostperUserservedWebChartControl.Height = 200;
                }
                else
                {
                    if (sortedDT.Rows.Count >= 4 && sortedDT.Rows.Count < 10)
                    {
                        CostperUserservedWebChartControl.Height = ((sortedDT.Rows.Count) * 50) + 20;
                    }
                    else
                    {
                        if (sortedDT.Rows.Count >= 10 && sortedDT.Rows.Count < 100)
                        {
                            CostperUserservedWebChartControl.Height = ((sortedDT.Rows.Count) * 40) + 20;
                        }
                        else
                        {
                            if (sortedDT.Rows.Count >= 100)
                            {
                                CostperUserservedWebChartControl.Height = ((sortedDT.Rows.Count) * 20) + 20;
                            }
                        }
                    }
                }
            }


            //10/20/2015 NS modified for VSPLUS-2072
            UI uiobj = new UI();
            uiobj.RecalibrateChartAxes(CostperUserservedWebChartControl.Diagram, "Y", "int", "int");
            CostperUserservedWebChartControl.DataSource = sortedDT;
            CostperUserservedWebChartControl.DataBind();
            
            
        }
  
        protected void SortRadioButtonList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            GraphforResponseTime(SortRadioButtonList1.SelectedItem.Value.ToString(),SearchTextBox.Text,
                TypeComboBox.Text,UserTypeComboBox.Text);
        }

        public void FillCombobox()
        {
            DataTable Typetab = VSWebBL.DashboardBL.KeyMetricsBL.Ins.GetServerTypeForCostperUserserved();            
            TypeComboBox.DataSource = Typetab;
            TypeComboBox.TextField = "ServerType";
            TypeComboBox.DataBind();
            
            try
            {
                TypeComboBox.SelectedItem = TypeComboBox.Items.FindByText("Domino");
            }
            catch
            {
                TypeComboBox.SelectedIndex = 0;
            }
        }

       

        protected void GoButton_Click(object sender, EventArgs e)
        {
            GraphforResponseTime(SortRadioButtonList1.SelectedItem.Value.ToString(), SearchTextBox.Text, 
                TypeComboBox.Text,UserTypeComboBox.Text);
        }

        protected void CostperUserservedWebChartControl_CustomCallback(object sender, DevExpress.XtraCharts.Web.CustomCallbackEventArgs e)
        {
          
            //CostperUserservedWebChartControl.Width = new Unit(Convert.ToInt32(chartWidth.Value));
            GraphforResponseTime(SortRadioButtonList1.SelectedItem.Value.ToString(), SearchTextBox.Text, TypeComboBox.Text,UserTypeComboBox.Text);
        }
        protected void ServerComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            GraphforResponseTime(SortRadioButtonList1.SelectedItem.Value.ToString(), SearchTextBox.Text,
                 TypeComboBox.SelectedItem.Value.ToString(),UserTypeComboBox.Text);
            
        }
        protected void UserTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)

        {
            GraphforResponseTime(SortRadioButtonList1.SelectedItem.Value.ToString(), SearchTextBox.Text,
                TypeComboBox.Text,UserTypeComboBox.Text);
           
        }
        protected void TypeComboBox_SelectedIndexChanged(object sender, EventArgs e)

        {
            GetUserType();
           

        }
        public void GetUserType()
        {
            if (TypeComboBox.SelectedItem.ToString() == "Domino")
            {
                UserTypeComboBox.Visible = false;
                UserTypelabel.Visible = false;

            }
            else
            {
                UserTypeComboBox.Visible = true;
                UserTypelabel.Visible = true;
            }
           
        }
        public void fillUserTypeCombo()
        {
            DataTable dt = new DataTable();
            dt = VSWebBL.ReportsBL.ReportsBL.Ins.GetMSUserCountTypes("AVG");
            if (dt.Rows.Count > 0)
            {
                //UserTypeComboBox.SelectedIndex = 0;
                UserTypeComboBox.TextField = "StatName";
                UserTypeComboBox.ValueField = "StatName";
                UserTypeComboBox.DataSource = dt;
                UserTypeComboBox.DataBind();
            }
        }
        protected void CostperUserservedWebChartControl_BoundDataChanged(object sender, EventArgs e)

        {
            UI uiobj = new UI();
            uiobj.RecalibrateChartAxes(CostperUserservedWebChartControl.Diagram);
        }
    }
}