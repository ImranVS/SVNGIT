using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.HtmlControls;
using DevExpress.XtraCharts;
using DevExpress.XtraCharts.Web;
using DevExpress.Web;
using DevExpress.Web.ASPxTreeList;

namespace VSWebUI.Dashboard
{
    public partial class ConnectionsHealth : System.Web.UI.Page
    {
        string selectedServer = "";
        int ID;
		protected void Page_PreInit(object sender, EventArgs e)
		{

			this.Master.contentCallEvent += new EventHandler(Master_ButtonClick);
		}

		private void Master_ButtonClick(object sender, EventArgs e)
		{

		}
        protected void Page_Load(object sender, EventArgs e)
        {
            //1/06/2016 sowmya added for VSPLUS-2934
            if (Session["UserPreferences"] != null)
            {
                DataTable UserPreferences = (DataTable)Session["UserPreferences"];
                foreach (DataRow dr in UserPreferences.Rows)
                {

                    if (dr[1].ToString() == "ConnectionsHealth|CommunitiesGrid")
                    {
                        CommunitiesGrid.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                    }
                }
            }
            if (!IsPostBack)
            {
                HtmlGenericControl body = (HtmlGenericControl)Master.FindControl("Body1");
                body.Attributes.Add("onload", "DoCallback()");
                body.Attributes.Add("onResize", "Resized()");
            }
            else
            {
                if (Session["CompareUsersGrid"] != null)
                {
                    CompareUsersGrid.DataSource = (DataTable)Session["CompareUsersGrid"];
                    CompareUsersGrid.DataBind();
                }
            }
            FillConnectionsGridView();
            if (ConnectionsGridView.VisibleRowCount > 0)
            {
                int index = ConnectionsGridView.FocusedRowIndex;
                ConnectionsGridView.Selection.UnselectAll();
                if (index > -1)
                {
                    //servernamelbldisp.InnerHtml = "";
                    ID = Convert.ToInt32(ConnectionsGridView.GetRowValues(index, "Id").ToString());
                    selectedServer = ConnectionsGridView.GetRowValues(index, "ServerName").ToString();
                }
            }
            FillActivitiesGridView();
            FillActivitiesHeader();
            FillUsersCombo();
            FillBlogsGridView();
            FillBookmarksGridView();
            FillBookmarkHeader();
            FillFilesGridView();
            FillForumsGridView();
            FillWikisGridView();
            FillDailyGridView();
            SetGraphForUsersDaily();
            SetGraphForTop5Tags();
            SetGraphForCommunityTypes();
            SetGraphForMostActiveCommunity();
            SetGraphForManagers();
            SetGraphForPictures();
            SetGraphForJobHierarchy();
            SetGraphForBlogs();
            SetGraphForActivities();
            SetGraphForFiles();
            SetGraphForForums();
            SetGraphForWikis();
            SetGraphForBookmarks();
            SetGraphForTop5Objects();
            SetGraphForTop5MostActiveCommunities();
            FillCommunitiesGrid();
        }

        private void FillConnectionsGridView()
        {
            try
            {
                DataTable dt = new DataTable();
                dt = VSWebBL.DashboardBL.ConnectionsBL.Ins.GetConnectionsData();
                if (dt.Rows.Count > 0)
                {
                    Session["ConnectionsTests"] = dt;
                    ConnectionsGridView.DataSource = dt;
                    ConnectionsGridView.Columns.Clear();
                    ConnectionsGridView.AutoGenerateColumns = true;
                    ConnectionsGridView.KeyFieldName = "Id";
                    ConnectionsGridView.DataBind();
                }
            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
            finally { }
        }

        private void FillDailyGridView()
        {
            try
            {
                DataTable dt = new DataTable();
                dt = VSWebBL.DashboardBL.ConnectionsBL.Ins.GetDailyActivities();
                if (dt.Rows.Count > 0)
                {
                    Session["ConnectionsDailyActivitiesGrid"] = dt;
                    DailyGridView.DataSource = dt;
                    DailyGridView.Columns.Clear();
                    DailyGridView.AutoGenerateColumns = true;
                    DailyGridView.KeyFieldName = "ID";
                    DailyGridView.DataBind();
                }
            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
            finally { }
        }

        private void FillActivitiesGridView()
        {
            try
            {
                DataTable dt = new DataTable();
                dt = VSWebBL.DashboardBL.ConnectionsBL.Ins.GetActivities("NUM_OF_ACTIVITIES");
                if (dt.Rows.Count > 0)
                {
                    Session["ConnectionsActivitiesGrid"] = dt;
                    ActivitiesGridView.DataSource = dt;
                    ActivitiesGridView.Columns.Clear();
                    ActivitiesGridView.AutoGenerateColumns = true;
                    ActivitiesGridView.KeyFieldName = "ID";
                    ActivitiesGridView.DataBind();
                }
            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
            finally { }
        }

        private void FillBlogsGridView()
        {
            try
            {
                DataTable dt = new DataTable();
                dt = VSWebBL.DashboardBL.ConnectionsBL.Ins.GetActivities("NUM_OF_BLOGS");
                if (dt.Rows.Count > 0)
                {
                    Session["ConnectionsBlogsGrid"] = dt;
                    BlogsGridView.DataSource = dt;
                    BlogsGridView.Columns.Clear();
                    BlogsGridView.AutoGenerateColumns = true;
                    BlogsGridView.KeyFieldName = "ID";
                    BlogsGridView.DataBind();
                }
            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
            finally { }
        }

        private void FillBookmarksGridView()
        {
            try
            {
                DataTable dt = new DataTable();
                dt = VSWebBL.DashboardBL.ConnectionsBL.Ins.GetActivities("NUM_OF_BOOKMARKS");
                if (dt.Rows.Count > 0)
                {
                    Session["ConnectionsBookmarksGrid"] = dt;
                    BookmarksGridView.DataSource = dt;
                    BookmarksGridView.Columns.Clear();
                    BookmarksGridView.AutoGenerateColumns = true;
                    BookmarksGridView.KeyFieldName = "ID";
                    BookmarksGridView.DataBind();
                }
            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
            finally { }
        }

        private void FillFilesGridView()
        {
            try
            {
                DataTable dt = new DataTable();
                dt = VSWebBL.DashboardBL.ConnectionsBL.Ins.GetActivities("NUM_OF_FILES");
                if (dt.Rows.Count > 0)
                {
                    Session["ConnectionsFilesGrid"] = dt;
                    FilesGridView.DataSource = dt;
                    FilesGridView.Columns.Clear();
                    FilesGridView.AutoGenerateColumns = true;
                    FilesGridView.KeyFieldName = "ID";
                    FilesGridView.DataBind();
                }
            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
            finally { }
        }

        private void FillForumsGridView()
        {
            try
            {
                DataTable dt = new DataTable();
                dt = VSWebBL.DashboardBL.ConnectionsBL.Ins.GetActivities("NUM_OF_FORUMS");
                if (dt.Rows.Count > 0)
                {
                    Session["ConnectionsForumsGrid"] = dt;
                    ForumsGridView.DataSource = dt;
                    ForumsGridView.Columns.Clear();
                    ForumsGridView.AutoGenerateColumns = true;
                    ForumsGridView.KeyFieldName = "ID";
                    ForumsGridView.DataBind();
                }
            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
            finally { }
        }

        private void FillWikisGridView()
        {
            try
            {
                DataTable dt = new DataTable();
                dt = VSWebBL.DashboardBL.ConnectionsBL.Ins.GetActivities("NUM_OF_WIKIS");
                if (dt.Rows.Count > 0)
                {
                    Session["ConnectionsWikisGrid"] = dt;
                    WikisGridView.DataSource = dt;
                    WikisGridView.Columns.Clear();
                    WikisGridView.AutoGenerateColumns = true;
                    WikisGridView.KeyFieldName = "ID";
                    WikisGridView.DataBind();
                }
            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
            finally { }
        }

        protected void ConnectionsGridView_HtmlDataCellPrepared(object sender, DevExpress.Web.ASPxGridViewTableDataCellEventArgs e)
        {
            //4/8/2016 NS modified for VSPLUS-2834
            if (e.CellValue.ToString() == "Pass" || e.CellValue.ToString() == "Passed" || e.CellValue.ToString() == "OK")
            {
                e.Cell.BackColor = System.Drawing.Color.LightGreen;
            }
            else if (e.CellValue.ToString().ToUpper().Contains("FAIL") || e.CellValue.ToString() == "Not Responding")
            {
                e.Cell.BackColor = System.Drawing.Color.Red;
                e.Cell.ForeColor = System.Drawing.Color.White;
            }
            else if (e.CellValue.ToString().ToUpper().Contains("ISSUE"))
            {
                e.Cell.BackColor = System.Drawing.Color.Yellow;
            }
        }

        protected void ConnectionsGridView_DataBound(object sender, EventArgs e)
        {
            for (int i = 0; i < ConnectionsGridView.Columns.Count; i++)
            {
                GridViewColumn c = ConnectionsGridView.Columns[i];
                GridViewDataColumn dataColumn = c as GridViewDataColumn;
                if (dataColumn.FieldName == "Id")
                {
                    c.Visible = false;
                }
                dataColumn.HeaderStyle.Wrap = DevExpress.Utils.DefaultBoolean.False;
                GridViewDataDateColumn dateColumn = c as GridViewDataDateColumn;
                if (dateColumn != null)
                {
                    if (dateColumn.FieldName == "LastUpdate")
                    {
                        dateColumn.PropertiesDateEdit.DisplayFormatString = "G";
                        dateColumn.Width = new Unit("140px");
                        //29-04-2016 Durga Modified for VSPLUS-2908
                        dateColumn.VisibleIndex = ConnectionsGridView.Columns.Count - 1;
                    }
                }
            }
            ConnectionsGridView.Width = System.Web.UI.WebControls.Unit.Percentage(100);
        }

        public DataTable SetGraphForUsersDaily()
        {
            //string fromdate = "";
            //string todate = "";
            //fromdate = dtPick.FromDate;
            //todate = dtPick.ToDate;
            UsersDailyWebChart.Series.Clear();
            DataTable dt = VSWebBL.DashboardBL.ConnectionsBL.Ins.GetUsersDailyCount("%created_last_day%", selectedServer);
            if (dt.Rows.Count > 0)
            {
                UsersDailyWebChart.SeriesDataMember = "StatName";
                UsersDailyWebChart.SeriesTemplate.ArgumentDataMember = "Date";
                UsersDailyWebChart.SeriesTemplate.ValueDataMembers.AddRange(new string[] { "StatValue" });
                ((LineSeriesView)UsersDailyWebChart.SeriesTemplate.View).LineMarkerOptions.Size = 7;
                ChartTitle title = new ChartTitle();

                title.Text = "Daily Activities";
                System.Drawing.Font font = new System.Drawing.Font(title.Font.FontFamily.Name, 12);
                title.Font = font;

                UsersDailyWebChart.Titles.Clear();
                UsersDailyWebChart.Titles.Add(title);
                UsersDailyWebChart.DataSource = dt;
                UsersDailyWebChart.DataBind();
                UI uiobj = new UI();
                uiobj.RecalibrateChartAxes(UsersDailyWebChart.Diagram, "Y", "int", "int");
                
            }
            return dt;
        }

        public DataTable SetGraphForBlogs()
        {
            BlogsWebChart.Series.Clear();
            DataTable dt = VSWebBL.DashboardBL.ConnectionsBL.Ins.GetUserStatsCommon("NUM_OF_BLOGS_", "_CREATED_YESTERDAY", selectedServer);
            if (dt.Rows.Count > 0)
            {
                BlogsWebChart.SeriesDataMember = "StatName";
                BlogsWebChart.SeriesTemplate.ArgumentDataMember = "Date";
                BlogsWebChart.SeriesTemplate.ValueDataMembers.AddRange(new string[] { "StatValue" });
                ((LineSeriesView)BlogsWebChart.SeriesTemplate.View).LineMarkerOptions.Size = 7;
                BlogsWebChart.DataSource = dt;
                BlogsWebChart.DataBind();
            }
            return dt;
        }

        public DataTable SetGraphForActivities()
        {
            //string fromdate = "";
            //string todate = "";
            //fromdate = dtPick.FromDate;
            //todate = dtPick.ToDate;
            ActivitiesWebChart.Series.Clear();
            //6/5/2016 Durga Added for VSPLUS-2925
            DataTable dt = VSWebBL.DashboardBL.ConnectionsBL.Ins.GetUserStatsForActivities("NUM_OF_ACTIVITIES_", "_YESTERDAY", selectedServer);
            if (dt.Rows.Count > 0)
            {
                ActivitiesWebChart.SeriesDataMember = "StatName";
                ActivitiesWebChart.SeriesTemplate.ArgumentDataMember = "Date";
                ActivitiesWebChart.SeriesTemplate.ValueDataMembers.AddRange(new string[] { "StatValue" });
                ((LineSeriesView)ActivitiesWebChart.SeriesTemplate.View).LineMarkerOptions.Size = 7;
                ActivitiesWebChart.DataSource = dt;
                ActivitiesWebChart.DataBind();
            }
            return dt;
        }

        public DataTable SetGraphForFiles()
        {
            //string fromdate = "";
            //string todate = "";
            //fromdate = dtPick.FromDate;
            //todate = dtPick.ToDate;
            FilesWebChart.Series.Clear();
            DataTable dt = VSWebBL.DashboardBL.ConnectionsBL.Ins.GetUserStatsCommon("NUM_OF_FILES_", "_YESTERDAY", selectedServer);
            if (dt.Rows.Count > 0)
            {
                FilesWebChart.SeriesDataMember = "StatName";
                FilesWebChart.SeriesTemplate.ArgumentDataMember = "Date";
                FilesWebChart.SeriesTemplate.ValueDataMembers.AddRange(new string[] { "StatValue" });
                ((LineSeriesView)FilesWebChart.SeriesTemplate.View).LineMarkerOptions.Size = 7;
                FilesWebChart.DataSource = dt;
                FilesWebChart.DataBind();
            }
            return dt;
        }

        public DataTable SetGraphForForums()
        {
            //string fromdate = "";
            //string todate = "";
            //fromdate = dtPick.FromDate;
            //todate = dtPick.ToDate;
            ForumsWebChart.Series.Clear();
            DataTable dt = VSWebBL.DashboardBL.ConnectionsBL.Ins.GetUserStatsCommon("NUM_OF_FORUMS_", "_CREATED_YESTERDAY", selectedServer);
            if (dt.Rows.Count > 0)
            {
                ForumsWebChart.SeriesDataMember = "StatName";
                ForumsWebChart.SeriesTemplate.ArgumentDataMember = "Date";
                ForumsWebChart.SeriesTemplate.ValueDataMembers.AddRange(new string[] { "StatValue" });
                ((LineSeriesView)ForumsWebChart.SeriesTemplate.View).LineMarkerOptions.Size = 7;
                ForumsWebChart.DataSource = dt;
                ForumsWebChart.DataBind();
            }
            return dt;
        }

        public DataTable SetGraphForWikis()
        {
            //string fromdate = "";
            //string todate = "";
            //fromdate = dtPick.FromDate;
            //todate = dtPick.ToDate;
            WikisWebChart.Series.Clear();
            DataTable dt = VSWebBL.DashboardBL.ConnectionsBL.Ins.GetUserStatsCommon("NUM_OF_WIKIS_", "_CREATED_YESTERDAY", selectedServer);
            if (dt.Rows.Count > 0)
            {
                WikisWebChart.SeriesDataMember = "StatName";
                WikisWebChart.SeriesTemplate.ArgumentDataMember = "Date";
                WikisWebChart.SeriesTemplate.ValueDataMembers.AddRange(new string[] { "StatValue" });
                ((LineSeriesView)WikisWebChart.SeriesTemplate.View).LineMarkerOptions.Size = 7;
                WikisWebChart.DataSource = dt;
                WikisWebChart.DataBind();
            }
            return dt;
        }

        public DataTable SetGraphForBookmarks()
        {
            //string fromdate = "";
            //string todate = "";
            //fromdate = dtPick.FromDate;
            //todate = dtPick.ToDate;
            BookmarksWebChart.Series.Clear();
            DataTable dt = VSWebBL.DashboardBL.ConnectionsBL.Ins.GetUserStatsCommon("NUM_OF_BOOKMARKS_", "_CREATED_YESTERDAY", selectedServer);
            if (dt.Rows.Count > 0)
            {
                BookmarksWebChart.SeriesDataMember = "StatName";
                BookmarksWebChart.SeriesTemplate.ArgumentDataMember = "Date";
                BookmarksWebChart.SeriesTemplate.ValueDataMembers.AddRange(new string[] { "StatValue" });
                ((LineSeriesView)BookmarksWebChart.SeriesTemplate.View).LineMarkerOptions.Size = 7;
                UI uiobj = new UI();
                uiobj.RecalibrateChartAxes(BookmarksWebChart.Diagram, "Y", "int", "int");
                BookmarksWebChart.DataSource = dt;
                BookmarksWebChart.DataBind();
            }
            return dt;
        }

        public DataTable SetGraphForManagers()
        {
            ManagersWebChart.Series.Clear();
            DataTable dt = VSWebBL.DashboardBL.ConnectionsBL.Ins.GetProfileStats("NUM_OF_PROFILES_MANAGERS", "NUM_OF_PROFILES_PROFILES", selectedServer,"Managers","Non Managers",false);
            Series series = new Series("StatName", ViewType.Doughnut);

            series.ArgumentDataMember = dt.Columns["StatName"].ToString();
            series.LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;
            DoughnutSeriesLabel label = (DoughnutSeriesLabel)series.Label;
            label.TextPattern = "{A}: {VP:P0}";
            series.LegendTextPattern = "{A}: {V}";

            ValueDataMemberCollection seriesValueDataMembers = (ValueDataMemberCollection)series.ValueDataMembers;
            seriesValueDataMembers.AddRange(dt.Columns["StatValue"].ToString());
            ManagersWebChart.Series.Add(series);

            ManagersWebChart.DataSource = dt;
            ManagersWebChart.DataBind();
            return dt;
        }

        public DataTable SetGraphForJobHierarchy()
        {
            JobHierarchyWebChart.Series.Clear();
            DataTable dt = VSWebBL.DashboardBL.ConnectionsBL.Ins.GetProfileStats("NUM_OF_PROFILES_WITH_JOB_HIERARCHY", "NUM_OF_PROFILES_WITH_NO_JOB_HIERARCHY", selectedServer, "Job Hierarchy", "No Job Hierarchy", false);
            Series series = new Series("StatName", ViewType.Doughnut);

            series.ArgumentDataMember = dt.Columns["StatName"].ToString();
            series.LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;
            DoughnutSeriesLabel label = (DoughnutSeriesLabel)series.Label;
            label.TextPattern = "{A}: {VP:P0}";
            series.LegendTextPattern = "{A}: {V}";

            ValueDataMemberCollection seriesValueDataMembers = (ValueDataMemberCollection)series.ValueDataMembers;
            seriesValueDataMembers.AddRange(dt.Columns["StatValue"].ToString());
            JobHierarchyWebChart.Series.Add(series);

            JobHierarchyWebChart.DataSource = dt;
            JobHierarchyWebChart.DataBind();
            return dt;
        }

        public DataTable SetGraphForPictures()
        {
            PicturesWebChart.Series.Clear();
            DataTable dt = VSWebBL.DashboardBL.ConnectionsBL.Ins.GetProfileStats("NUM_OF_PROFILES_WITH_NO_PICTURE", "NUM_OF_PROFILES_PROFILES", selectedServer, "No Picture", "Picture", false);
            Series series = new Series("StatName", ViewType.Doughnut);

            series.ArgumentDataMember = dt.Columns["StatName"].ToString();
            series.LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;
            DoughnutSeriesLabel label = (DoughnutSeriesLabel)series.Label;
            label.TextPattern = "{A}: {VP:P0}";
            series.LegendTextPattern = "{A}: {V}";

            ValueDataMemberCollection seriesValueDataMembers = (ValueDataMemberCollection)series.ValueDataMembers;
            seriesValueDataMembers.AddRange(dt.Columns["StatValue"].ToString());
            PicturesWebChart.Series.Add(series);

            PicturesWebChart.DataSource = dt;
            PicturesWebChart.DataBind();
            return dt;
        }

        public DataTable SetGraphForTop5Tags()
        {
            Top5TagsWebChart.Series.Clear();
            DataTable dt = VSWebBL.DashboardBL.ConnectionsBL.Ins.GetTopTags("");
            if (dt.Rows.Count > 0)
            {
                Top5TagsWebChart.SeriesDataMember = "StatName";
                Top5TagsWebChart.SeriesTemplate.ArgumentDataMember = "StatName";
                Top5TagsWebChart.SeriesTemplate.ValueDataMembers.AddRange(new string[] { "StatValue" });
                XYDiagram d = (XYDiagram)Top5TagsWebChart.Diagram;
                d.AxisX.Reverse = true;
                ((BarSeriesView)Top5TagsWebChart.SeriesTemplate.View).BarWidth = 1;
                Top5TagsWebChart.DataSource = dt;
                Top5TagsWebChart.DataBind();
            }
            return dt;
        }

        protected void ActivitiesGridView_DataBound(object sender, EventArgs e)
        {
            for (int i = 0; i < ActivitiesGridView.Columns.Count; i++)
            {
                GridViewColumn c = ActivitiesGridView.Columns[i];
                GridViewDataColumn dataColumn = c as GridViewDataColumn;
                if (dataColumn.FieldName == "ID")
                {
                    c.Visible = false;
                }
                else if (dataColumn.FieldName == "StatValue")
                {
                    c.CellStyle.CssClass = "GridCss2";
                    c.HeaderStyle.CssClass = "GridCssHeader2";
                }
            }
        }

        protected void BlogsGridView_DataBound(object sender, EventArgs e)
        {
            for (int i = 0; i < BlogsGridView.Columns.Count; i++)
            {
                GridViewColumn c = BlogsGridView.Columns[i];
                GridViewDataColumn dataColumn = c as GridViewDataColumn;
                if (dataColumn.FieldName == "ID")
                {
                    c.Visible = false;
                }
                else if (dataColumn.FieldName == "StatValue")
                {
                    c.CellStyle.CssClass = "GridCss2";
                    c.HeaderStyle.CssClass = "GridCssHeader2";
                }
            }
        }

        protected void BookmarksGridView_DataBound(object sender, EventArgs e)
        {
            for (int i = 0; i < BookmarksGridView.Columns.Count; i++)
            {
                GridViewColumn c = BookmarksGridView.Columns[i];
                GridViewDataColumn dataColumn = c as GridViewDataColumn;
                if (dataColumn.FieldName == "ID")
                {
                    c.Visible = false;
                }
                else if (dataColumn.FieldName == "StatValue")
                {
                    c.CellStyle.CssClass = "GridCss2";
                    c.HeaderStyle.CssClass = "GridCssHeader2";
                }
            }
        }

        protected void FilesGridView_DataBound(object sender, EventArgs e)
        {
            for (int i = 0; i < FilesGridView.Columns.Count; i++)
            {
                GridViewColumn c = FilesGridView.Columns[i];
                GridViewDataColumn dataColumn = c as GridViewDataColumn;
                if (dataColumn.FieldName == "ID")
                {
                    c.Visible = false;
                }
                else if (dataColumn.FieldName == "StatValue")
                {
                    c.CellStyle.CssClass = "GridCss2";
                    c.HeaderStyle.CssClass = "GridCssHeader2";
                }
            }
        }

        protected void ForumsGridView_DataBound(object sender, EventArgs e)
        {
            for (int i = 0; i < ForumsGridView.Columns.Count; i++)
            {
                GridViewColumn c = ForumsGridView.Columns[i];
                GridViewDataColumn dataColumn = c as GridViewDataColumn;
                if (dataColumn.FieldName == "ID")
                {
                    c.Visible = false;
                }
                else if (dataColumn.FieldName == "StatValue")
                {
                    c.CellStyle.CssClass = "GridCss2";
                    c.HeaderStyle.CssClass = "GridCssHeader2";
                }
            }
        }

        protected void WikisGridView_DataBound(object sender, EventArgs e)
        {
            for (int i = 0; i < WikisGridView.Columns.Count; i++)
            {
                GridViewColumn c = WikisGridView.Columns[i];
                GridViewDataColumn dataColumn = c as GridViewDataColumn;
                if (dataColumn.FieldName == "ID")
                {
                    c.Visible = false;
                }
                else if (dataColumn.FieldName == "StatValue")
                {
                    c.CellStyle.CssClass = "GridCss2";
                    c.HeaderStyle.CssClass = "GridCssHeader2";
                }
            }
        }

        protected void DailyGridView_DataBound(object sender, EventArgs e)
        {
            for (int i = 0; i < DailyGridView.Columns.Count; i++)
            {
                GridViewColumn c = DailyGridView.Columns[i];
                GridViewDataColumn dataColumn = c as GridViewDataColumn;
                if (dataColumn.FieldName == "ID")
                {
                    c.Visible = false;
                }
                else if (dataColumn.FieldName == "StatValue")
                {
                    c.CellStyle.CssClass = "GridCss2";
                    c.HeaderStyle.CssClass = "GridCssHeader2";
                }
            }
        }

        private void FillBookmarkHeader()
        {
            DataTable dt = new DataTable();
            try
            {
                dt = VSWebBL.DashboardBL.ConnectionsBL.Ins.GetStatByName("NUM_OF_BOOKMARKS_BOOKMARKS");
                if (dt.Rows.Count > 0)
                {
                    BookmarksLabel.Text = dt.Rows[0]["StatValue"].ToString();
                }
            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
            finally { }
            try
            {
                dt = VSWebBL.DashboardBL.ConnectionsBL.Ins.GetStatByName("NUM_OF_DISTINCT_BOOKMARK_URLS");
                if (dt.Rows.Count > 0)
                {
                    BookmarkURLsLabel.Text = dt.Rows[0]["StatValue"].ToString();
                }
            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
            finally { }
        }

        private void FillActivitiesHeader()
        {
            DataTable dt = new DataTable();
            try
            {
                dt = VSWebBL.DashboardBL.ConnectionsBL.Ins.GetStatByName("NUM_OF_USERS_FOLLOWING_ACTIVITY");
                if (dt.Rows.Count > 0)
                {
                    UsersActivityLabel.Text = dt.Rows[0]["StatValue"].ToString();
                }
            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
            finally { }
            try
            {
                dt = VSWebBL.DashboardBL.ConnectionsBL.Ins.GetStatByName("NUM_OF_ACTIVITY_OWNERS");
                if (dt.Rows.Count > 0)
                {
                    ActivityOwnersLabel.Text = dt.Rows[0]["StatValue"].ToString();
                }
            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
            finally { }
        }

        private void SetGraphForCommunityTypes()
        {
            CommunitiesByTypeChart.Series.Clear();
            string statname = "COMMUNITY_TYPE_";
            DataTable dt = VSWebBL.DashboardBL.ConnectionsBL.Ins.GetStatByName(statname, false);
            Series series = new Series("StatName", ViewType.Pie);

            series.ArgumentDataMember = dt.Columns["StatName"].ToString();
            series.LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;
            PieSeriesLabel label = (PieSeriesLabel)series.Label;
            label.TextPattern = "{A}: {VP:P0}";
            series.LegendTextPattern = "{A}: {V}";

            ValueDataMemberCollection seriesValueDataMembers = (ValueDataMemberCollection)series.ValueDataMembers;
            seriesValueDataMembers.AddRange(dt.Columns["StatValue"].ToString());
            CommunitiesByTypeChart.Series.Add(series);

            CommunitiesByTypeChart.DataSource = dt;
            CommunitiesByTypeChart.DataBind();
        }

        //6/1/2016 NS added for VSPLUS-3015
        private void FillUsersCombo()
        {
            DataTable dt = new DataTable();
            try
            {
                dt = VSWebBL.DashboardBL.ConnectionsBL.Ins.GetConnectionsUsers();
                if (dt.Rows.Count > 0)
                {
                    User1ComboBox.DataSource = dt;
                    User1ComboBox.ValueField = "DisplayName";
                    User1ComboBox.TextField = "DisplayName";
                    User1ComboBox.DataBind();
                    User2ComboBox.DataSource = dt;
                    User2ComboBox.ValueField = "DisplayName";
                    User2ComboBox.TextField = "DisplayName";
                    User2ComboBox.DataBind();
                }
            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
            finally { }
            try
            {
                dt = VSWebBL.DashboardBL.ConnectionsBL.Ins.GetStatByName("NUM_OF_ACTIVITY_OWNERS");
                if (dt.Rows.Count > 0)
                {
                    ActivityOwnersLabel.Text = dt.Rows[0]["StatValue"].ToString();
                }
            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
            finally { }
        }

        protected void CompareUsersButton_Click(object sender, EventArgs e)
        {
            string user1 = "";
            string user2 = "";
            bool proceed = true;
            DataTable dt = new DataTable();
            if (User1ComboBox.SelectedIndex != -1)
            {
                user1 = User1ComboBox.SelectedItem.Text;
            }
            else
            {
                CompareUsersGrid.ClientVisible = false;
                errorDiv.Style.Value = "display: block;";
                errorDiv.InnerHtml = "You must select User 1 and User 2 in order to perform a comparison." +
                    "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                proceed = false;
            }
            if (User2ComboBox.SelectedIndex != -1)
            {
                user2 = User2ComboBox.SelectedItem.Text;
            }
            else
            {
                CompareUsersGrid.ClientVisible = false;
                errorDiv.Style.Value = "display: block;";
                errorDiv.InnerHtml = "You must select User 1 and User 2 in order to perform a comparison." +
                    "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                proceed = false;
            }
            if (user1 != "" && user2 != "" && user1 == user2)
            {
                CompareUsersGrid.ClientVisible = false;
                errorDiv.Style.Value = "display: block;";
                errorDiv.InnerHtml = "User 1 and User 2 must be different in order to perform a comparison." +
                    "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                proceed = false;
            }
            if (proceed)
            {
                errorDiv.Style.Value = "display: none;";
                dt = VSWebBL.DashboardBL.ConnectionsBL.Ins.GetCommunitiesForUsers(user1, user2);
                CompareUsersGrid.ClientVisible = true;
                Session["CompareUsersGrid"] = dt;
                CompareUsersGrid.DataSource = dt;
                CompareUsersGrid.DataBind();
                ((GridViewDataColumn)CompareUsersGrid.Columns["Category"]).GroupBy();
            }
        }

        //6/2/2016 NS added for VSPLUS-3011
        private void SetGraphForMostActiveCommunity()
        {
            MostActiveCommunityChart.Series.Clear();
            DataTable dt = VSWebBL.DashboardBL.ConnectionsBL.Ins.GetMostActiveCommunity();
            if (dt.Rows.Count > 0)
            {
                Series series = new Series("Type", ViewType.Doughnut);

                series.ArgumentDataMember = dt.Columns["Type"].ToString();
                series.LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;
                DoughnutSeriesLabel label = (DoughnutSeriesLabel)series.Label;
                label.TextPattern = "{A}: {VP:P0}";
                series.LegendTextPattern = "{A}: {V}";

                ValueDataMemberCollection seriesValueDataMembers = (ValueDataMemberCollection)series.ValueDataMembers;
                seriesValueDataMembers.AddRange(dt.Columns["Total"].ToString());
                MostActiveCommunityChart.Series.Add(series);

                ChartTitle title = new ChartTitle();

                title.Text = "Most Active Community is \"" + dt.Rows[0]["Name"].ToString() + "\"";
                System.Drawing.Font font = new System.Drawing.Font(title.Font.FontFamily.Name, 12);
                title.Font = font;

                MostActiveCommunityChart.Titles.Clear();
                MostActiveCommunityChart.Titles.Add(title);
                MostActiveCommunityChart.DataSource = dt;
                MostActiveCommunityChart.DataBind();
            }
        }

        //6/2/2016 NS added for VSPLUS-3016
        private void SetGraphForTop5Objects()
        {
            SetGraphForTop5("Bookmark", Top5BookmarksChart);
            SetGraphForTop5("Activity", Top5ActivitiesChart);
            SetGraphForTop5("Blog", Top5BlogsChart);
            SetGraphForTop5("Wiki", Top5WikisChart);
        }

        private void SetGraphForTop5(string objtype,object objchart)
        {
            ((WebChartControl)objchart).Series.Clear();
            DataTable dt = VSWebBL.DashboardBL.ConnectionsBL.Ins.GetSourceCommunity(objtype);
            if (dt.Rows.Count > 0)
            {
                ((WebChartControl)objchart).SeriesDataMember = "Name";
                ((WebChartControl)objchart).SeriesTemplate.ArgumentDataMember = "Name";
                ((WebChartControl)objchart).SeriesTemplate.ValueDataMembers.AddRange(new string[] { "Total" });
                XYDiagram d = (XYDiagram)((WebChartControl)objchart).Diagram;
                //((BarSeriesView)((WebChartControl)objchart).SeriesTemplate.View).BarWidth = 1;
                UI uiobj = new UI();
                uiobj.RecalibrateChartAxes(((WebChartControl)objchart).Diagram, "Y", "int", "int"); 
                ((WebChartControl)objchart).DataSource = dt;
                ((WebChartControl)objchart).DataBind();
            }
        }
        
        //1/06/2016 sowmya added for VSPLUS-2934
        private void FillCommunitiesGrid()
        {
            try
            {
                DataTable dt = new DataTable();
                dt = dt = VSWebBL.ReportsBL.ReportsBL.Ins.GetCommList("");
                if (dt.Rows.Count > 0)
                {
                    
                    CommunitiesGrid.DataSource = dt;
                    CommunitiesGrid.Columns.Clear();
                    CommunitiesGrid.AutoGenerateColumns = true;
                    
                    CommunitiesGrid.DataBind();
                    (CommunitiesGrid.Columns["Name"] as GridViewDataColumn).GroupBy();
                }
            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
            finally { }
        }
        protected void CommunitiesGrid_PageSizeChanged(object sender, EventArgs e)
        {
            VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("ConnectionsHealth|CommunitiesGrid", CommunitiesGrid.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        }

        private void SetGraphForTop5MostActiveCommunities()
        {
            DataTable dt = new DataTable();
            dt = VSWebBL.DashboardBL.ConnectionsBL.Ins.GetTop5MostActiveCommunities();
            if (dt.Rows.Count > 0)
            {
                Top5CommunitiesChart.Series.Clear();
                Series series = null;
                string seriesname = "";
                string seriesarg = "";
                int seriesval = -1;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    seriesarg = dt.Rows[i]["Name"].ToString();
                    seriesname = dt.Rows[i]["Type"].ToString();
                    seriesval = Convert.ToInt32(dt.Rows[i]["Total"].ToString());
                    series = Top5CommunitiesChart.Series[seriesname];
                    if (series == null)
                    {
                        series = new Series(seriesname, ViewType.StackedBar);
                        series.Points.Add(new SeriesPoint(seriesarg, seriesval));
                        series.ShowInLegend = true;
                        series.LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;
                        Top5CommunitiesChart.Series.Add(series);
                        Top5CommunitiesChart.DataBind();
                    }
                    else
                    {
                        series.Points.Add(new SeriesPoint(seriesarg, seriesval));
                        series.ShowInLegend = true;
                        series.LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;
                        Top5CommunitiesChart.DataBind();
                    }
                }
                UI uiobj = new UI();
                uiobj.RecalibrateChartAxes(Top5CommunitiesChart.Diagram, "Y", "int", "int");
            }
        }
    }
}