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
	public partial class Office365Groups : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				FillGroupsGrid();
			}
			else
			{
				FillOffice365GroupsgridFromSession();	
			}
            SetGraphForGroups();
		}
		public void FillGroupsGrid()
		{
			DataTable dt = VSWebBL.DashboardBL.Office365BL.Ins.Groupgrid();
			if (dt.Rows.Count > 0)
			{
				Office365Groupsgrid.DataSource = dt;
				Session["Office365Groupsgrid"] = dt;
				Office365Groupsgrid.DataBind();
				Office365Groupsgrid.GroupSummarySortInfo.Clear();
				((GridViewDataColumn)Office365Groupsgrid.Columns["GroupName"]).GroupBy();
				ASPxGroupSummarySortInfo sortInfo = new ASPxGroupSummarySortInfo();
				sortInfo.SortOrder = DevExpress.Data.ColumnSortOrder.Ascending;
				sortInfo.SummaryItem = Office365Groupsgrid.GroupSummary["GroupName", DevExpress.Data.SummaryItemType.Count];
				sortInfo.GroupColumn = "GroupName";
				Office365Groupsgrid.GroupSummarySortInfo.AddRange(sortInfo);
			}
		}
		public void FillOffice365GroupsgridFromSession()
		{
			//MSoma
			if (Session["Office365Groupsgrid"] != "" && Session["Office365Groupsgrid"] != null)
			{
				Office365Groupsgrid.DataSource = (DataTable)Session["Office365Groupsgrid"];
				Office365Groupsgrid.DataBind();
			}
			
		}
        public void SetGraphForGroups()
        {
            GroupsWebChart.Series.Clear();
            DataTable dt = new DataTable();
            dt = VSWebBL.DashboardBL.Office365BL.Ins.GetOffice365GroupCount();
            GroupsWebChart.DataSource = dt;
            Series series = null;
            List<Series> serieslist = new List<Series>();
            string groupName = "";
            string groupType = "";
            string groupNameNew = "";
            string groupTypeNew = "";
            SeriesTitle title;
            if (dt.Rows.Count > 0)
            {
        
                bool seriesadded = false;
                groupName = dt.Rows[0]["GroupName"].ToString();
                groupType = dt.Rows[0]["GroupType"].ToString();
                series = new Series(groupType, ViewType.Pie3D);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    groupNameNew = dt.Rows[i]["GroupName"].ToString();
                    groupTypeNew = dt.Rows[i]["GroupType"].ToString();
                    //5/31/2016 NS added for VSPLUS-3007
                    title = new SeriesTitle();
                    title.Text = string.Format("{0}", groupType);

                    if (groupType != groupTypeNew)
                    {
                        if (series != null && !seriesadded)
                        {
                            series.LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;
                            Pie3DSeriesLabel label = (Pie3DSeriesLabel)series.Label;
                            label.TextPattern = "{A}: {VP:P0}";
                            label.Position = PieSeriesLabelPosition.TwoColumns;
                            series.LegendTextPattern = "{A}: {V}";
                            //5/31/2016 NS added for VSPLUS-3007
                            ((Pie3DSeriesView)series.View).Titles.Add(title);
                            GroupsWebChart.Series.Add(series);
                            //5/31/2016 NS added for VSPLUS-3007
                            groupType = groupTypeNew;
                        }
                        series = GroupsWebChart.Series[groupTypeNew];

                        if (series == null)
                        {
                            seriesadded = false;
                            series = new Series(groupTypeNew, ViewType.Pie3D);
                        }
                        else
                        {
                            seriesadded = true;
                        }
                        series.ArgumentDataMember = dt.Columns["GroupName"].ToString();
                        series.ArgumentScaleType = ScaleType.Qualitative;
                        series.ValueScaleType = ScaleType.Numerical;
                    }
                    if (series != null)
                    {
                        series.Points.Add(new SeriesPoint(groupNameNew, Convert.ToDouble(dt.Rows[i]["GroupCount"].ToString())));
                    }
                }
                //5/31/2016 NS added for VSPLUS-3007
                groupType = groupTypeNew;
                title = new SeriesTitle();
                title.Text = string.Format("{0}", groupType);
                if (series != null && !seriesadded)
                {
                    series.LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;
                    Pie3DSeriesLabel label = (Pie3DSeriesLabel)series.Label;
                    label.TextPattern = "{A}: {VP:P0}";
                    label.Position = PieSeriesLabelPosition.TwoColumns;
                    series.LegendTextPattern = "{A}: {V}";
                    //5/31/2016 NS added for VSPLUS-3007
                    ((Pie3DSeriesView)series.View).Titles.Add(title);
                    GroupsWebChart.Series.Add(series);
                }
            }
        }
	}
}