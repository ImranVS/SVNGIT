using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DevExpress.Web;
using System.Web.UI.HtmlControls;
using System.Drawing;
using DevExpress.XtraCharts;

namespace VSWebUI.Dashboard
{
    public partial class ServerDownTime : System.Web.UI.Page
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
            this.SrvDownTimeWebChart.Width = new Unit(Convert.ToInt32(chartWidth.Value));

            if (!IsPostBack && !IsCallback)
            {
                //FillCombobox();
                HtmlGenericControl body = (HtmlGenericControl)Master.FindControl("Body1");
                body.Attributes.Add("onload", "DoCallback()");
                body.Attributes.Add("onResize", "Resized()");
            }
            GraphForDownTime();
        }

        public void GraphForDownTime()
        {
            SrvDownTimeWebChart.Series.Clear();
            DataTable dt = VSWebBL.DashboardBL.KeyMetricsBL.Ins.GetServerDownTime();

            Series series = new Series("DownMin", ViewType.Bar);

            series.ArgumentDataMember = dt.Columns["Name"].ToString();
            series.LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;

            ValueDataMemberCollection seriesValueDataMembers = (ValueDataMemberCollection)series.ValueDataMembers;
            seriesValueDataMembers.AddRange(dt.Columns["DownMinutes"].ToString());
            SrvDownTimeWebChart.Series.Add(series);

            SrvDownTimeWebChart.Legend.Visible = false;

            ((SideBySideBarSeriesView)series.View).ColorEach = true;

            XYDiagram seriesXY = (XYDiagram)SrvDownTimeWebChart.Diagram;
            ((DevExpress.XtraCharts.XYDiagram)SrvDownTimeWebChart.Diagram).Rotated = true;
            seriesXY.AxisX.Label.ResolveOverlappingOptions.AllowRotate = false;
            seriesXY.AxisX.Label.ResolveOverlappingOptions.AllowHide = false;
            seriesXY.AxisY.Title.Text = "Minutes";
            seriesXY.AxisY.Title.Visible = true;
            AxisBase axisy = ((XYDiagram)SrvDownTimeWebChart.Diagram).AxisY;
            double min = Convert.ToDouble(((XYDiagram)SrvDownTimeWebChart.Diagram).AxisY.Range.MinValue);
            double max = Convert.ToDouble(((XYDiagram)SrvDownTimeWebChart.Diagram).AxisY.Range.MaxValue);

            int gs = (int)((max - min) / 5);

            if (gs == 0)
            {
                gs = 1;
                seriesXY.AxisY.GridSpacingAuto = false;
                seriesXY.AxisY.GridSpacing = gs;
                axisy.Range.AlwaysShowZeroLevel = false;
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
                    SrvDownTimeWebChart.Height = 200;
                }
                else
                {
                    if (dt.Rows.Count >= 4 && dt.Rows.Count < 10)
                    {
                        SrvDownTimeWebChart.Height = ((dt.Rows.Count) * 50) + 20;
                    }
                    else
                    {
                        if (dt.Rows.Count >= 10 && dt.Rows.Count < 100)
                        {
                            SrvDownTimeWebChart.Height = ((dt.Rows.Count) * 40) + 20;
                        }
                        else
                        {
                            if (dt.Rows.Count >= 100)
                            {
                                SrvDownTimeWebChart.Height = ((dt.Rows.Count) * 20) + 20;
                            }
                        }
                    }
                }
            }
            SrvDownTimeWebChart.DataSource = dt;
            SrvDownTimeWebChart.DataBind();
        }
        
    }
}