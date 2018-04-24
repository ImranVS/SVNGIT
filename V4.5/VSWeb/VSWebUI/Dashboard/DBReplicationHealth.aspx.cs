using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DevExpress.XtraCharts;
using System.Web.UI.HtmlControls;
using DevExpress.Web;
using System.Drawing;

namespace VSWebUI.Dashboard
{
    public partial class DBReplicationHealth : System.Web.UI.Page
    {
        //9/15/2014 NS added for VSPLUS-921
        public int threshold = 0;
        int value;
        string ClustName;
        int Clustervalue;
        int Thresholdval;
        protected void Page_Load(object sender, EventArgs e)
        {
            //9/15/2014 NS added for VSPLUS-921
            bool ShowAll = false;
           
            this.ClusterRepWebChartControl1.Width = new Unit(Convert.ToInt32(chartWidth.Value));
            this.ClusterRepWebChartControl2.Width = new Unit(Convert.ToInt32(chartWidth.Value));
            //17/3/2016 sowmya added for VSPLUS 2455
            Fillclusterinfogrid();
            if (ClusterInfoGrid.VisibleRowCount > 0)
            {
                int index = ClusterInfoGrid.FocusedRowIndex;
                if (index > -1)
                {
                    Clustervalue = Convert.ToInt32(ClusterInfoGrid.GetRowValues(index, "ID"));
                    ClustName = ClusterInfoGrid.GetRowValues(index, "Name").ToString();
                    Thresholdval=Convert.ToInt32(ClusterInfoGrid.GetRowValues (index ,"First_Alert_Threshold"));
                    Session["ClustName"] = ClustName;
                    Session["Thresholdvalue"] = Thresholdval;
                }
            }
         
            if (!IsPostBack && !IsCallback)
            {
                //9/15/2014 NS added for VSPLUS-921
                //FillCombobox();
               
                HtmlGenericControl body = (HtmlGenericControl)Master.FindControl("Body1");
                body.Attributes.Add("onResize", "Resized()");
                body.Attributes.Add("onload", "DoCallback()");
                if (Session["UserPreferences"] != null)
                {
                    DataTable UserPreferences = (DataTable)Session["UserPreferences"];
                    foreach (DataRow dr in UserPreferences.Rows)
                    {
                        //9/16/2014 NS added for VSPLUS-921
                        if (dr[1].ToString() == "PotentialClusterIssues|PotentialIssuesGrid")
                        {
                            PotentialIssuesGrid.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                        }
                        //17/3/2016 sowmya added for VSPLUS 2455
                        if (dr[1].ToString() == "ClusterDB|ClusterInfoGrid")
                        {
                            ClusterInfoGrid.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                        }
                    }
                }
            }
            //10/17/2014 NS modified for VSPLUS-1024
            //if (ClusterNameComboBox1.Items.Count > 0)
            //{
            //    ClusterName = ClusterNameComboBox1.SelectedItem.Value.ToString();
            //}
            if (ShowProblemsRadioButtonList.SelectedItem.Value.ToString() == "0")
            {
                ShowAll = true;
            }
            //10/17/2014 NS modified for VSPLUS-1024
            if (ClustName != "")
            {
                FillPotentialIssuesGrid(ClustName, ShowAll);
                GraphForClusterRepDocCountProblems(ClustName);
                GraphForClusterRepDBSizeProblems(ClustName);
            }
        }

        protected void ShowProblemsRadioButtonList_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool ShowAll = false;
              if (ShowProblemsRadioButtonList.SelectedItem.Value.ToString() == "0") //show all
              {
                   ShowAll = true;
               }
              FillPotentialIssuesGrid(ClustName, ShowAll);
           
        }

        //9/15/2014 NS added for VSPLUS-921
        public void FillPotentialIssuesGrid(string ClustName, bool ShowAll)
        {
            DataTable dt2 = VSWebBL.ReportsBL.ReportsBL.Ins.GetDBClusterServers(ClustName);
            if (dt2.Rows.Count > 0)
            {
              threshold =Convert .ToInt32 (Session["Thresholdvalue"]);
                //threshold = int.Parse(dt2.Rows[0]["Threshold"].ToString());
            //    lblFailureThreshold.Text = threshold.ToString() + "%";
            }
            DataTable dtraw = VSWebBL.ReportsBL.ReportsBL.Ins.GetDBClusterInfo(ClustName);
            DataTable dt = new DataTable();
            //5/9/2016 Sowjanya modified for VSPLUS-2931
            DataTable DBClusterCDatatable = VSWebBL.ReportsBL.ReportsBL.Ins.GetDBClusterC(ClustName);

            //5/11/2016 NS modified for VSPLUS-2966
            if (DBClusterCDatatable.Rows.Count > 0)
            {
                Session["ServerCName"] = DBClusterCDatatable.Rows[0]["ServerCName"].ToString();
            }

           
            dt = GetThresholdList(dtraw, "DocCountA", "DocCountB", "DocCountC", "DBSizeA", "DBSizeB", "DBSizeC", ShowAll);
          
            if (dt.Rows.Count > 0)
            {
                //9/11/2015 NS modified for VSPLUS-2126
                dt.PrimaryKey = new DataColumn[] { dt.Columns["ID"] };
                PotentialIssuesGrid.DataSource = dt;
                PotentialIssuesGrid.DataBind();
                if (dt2.Rows.Count > 0)
                {
                    PotentialIssuesGrid.Columns["DocCountA"].Caption = dt2.Rows[0]["ServerNameA"].ToString() + "<br />" + "Document Count";
                    PotentialIssuesGrid.Columns["DocCountB"].Caption = dt2.Rows[0]["ServerNameB"].ToString() + "<br />" + "Document Count";
                    if (dt2.Rows[0]["ServerNameC"].ToString() == "")
                    {
                        PotentialIssuesGrid.Columns[5].Visible = false;
                        PotentialIssuesGrid.Columns[6].Visible = false;
                    }
                    else
                    {
                        PotentialIssuesGrid.Columns[5].Visible = true;
                        PotentialIssuesGrid.Columns[6].Visible = true;
                        PotentialIssuesGrid.Columns["DocCountC"].Caption = dt2.Rows[0]["ServerNameC"].ToString() + "<br />" + "Document Count";
                        PotentialIssuesGrid.Columns["DBSizeC"].Caption = dt2.Rows[0]["ServerNameC"].ToString() + "<br />" + "Database Size";
                    }
                    PotentialIssuesGrid.Columns["DBSizeA"].Caption = dt2.Rows[0]["ServerNameA"].ToString() + "<br />" + "Database Size";
                    PotentialIssuesGrid.Columns["DBSizeB"].Caption = dt2.Rows[0]["ServerNameB"].ToString() + "<br />" + "Database Size";
                    
                }
                Session["PotentialClusterIssues"] = dt;
            }
            else
            {
                //5/9/2016 Sowjanya modified for VSPLUS-2931
                if (DBClusterCDatatable.Rows.Count > 0)
                {

                    if (DBClusterCDatatable.Rows[0]["ServerCName"].ToString() == "")
                    {
                        PotentialIssuesGrid.Columns[5].Visible = false;
                        PotentialIssuesGrid.Columns[6].Visible = false;
                    }
                    else
                    {
                        PotentialIssuesGrid.Columns[5].Visible = true;
                        PotentialIssuesGrid.Columns[6].Visible = true;
                        PotentialIssuesGrid.Columns["DocCountC"].Caption = "Document Count C";
                        PotentialIssuesGrid.Columns["DBSizeC"].Caption = "DB Size C";
                    }
                }
                
                PotentialIssuesGrid.Columns["DocCountA"].Caption = "Document Count A";
                PotentialIssuesGrid.Columns["DocCountB"].Caption = "Document Count B";
                PotentialIssuesGrid.Columns["DBSizeA"].Caption = "DB Size A";
                PotentialIssuesGrid.Columns["DBSizeB"].Caption = "DB Size B";
                PotentialIssuesGrid.DataSource = dt;
                PotentialIssuesGrid.DataBind();
            }
        }

        public void GraphForClusterRepDocCountProblems(string ClusterName)
        {
            DataTable dt2 = VSWebBL.ReportsBL.ReportsBL.Ins.GetDBClusterServers(ClusterName);
            ClusterRepWebChartControl1.Series.Clear();
            DataTable dtraw = VSWebBL.ReportsBL.ReportsBL.Ins.GetDBClusterInfo(ClusterName);
            DataTable dt = GetThresholdList(dtraw, "DocCountA", "DocCountB", "DocCountC", "DBSizeA", "DBSizeB", "DBSizeC", false);
            Series seriesA = new Series("ServerNameA", ViewType.Bar);
            Series seriesB = new Series("ServerNameB", ViewType.Bar);
            Series seriesC = new Series("ServerNameC", ViewType.Bar);
            if (dt.Rows.Count > 0)
            {
                ClusterRepWebChartControl1.DataSource = dt;
                seriesA.DataSource = dt;
                seriesA.ArgumentDataMember = "DatabaseName";
                ValueDataMemberCollection seriesValueDataMembersA = (ValueDataMemberCollection)seriesA.ValueDataMembers;
                seriesValueDataMembersA.AddRange(dt.Columns["DocCountA"].ToString());
                seriesA.Visible = true;
                seriesB.DataSource = dt;
                seriesB.ArgumentDataMember = "DatabaseName";
                ValueDataMemberCollection seriesValueDataMembersB = (ValueDataMemberCollection)seriesB.ValueDataMembers;
                seriesValueDataMembersB.AddRange(dt.Columns["DocCountB"].ToString());
                seriesB.Visible = true;
                if (Session["ServerCName"] != "")
                {
                    seriesC.DataSource = dt;
                    seriesC.ArgumentDataMember = "DatabaseName";
                    ValueDataMemberCollection seriesValueDataMembersC = (ValueDataMemberCollection)seriesC.ValueDataMembers;
                    seriesValueDataMembersC.AddRange(dt.Columns["DocCountC"].ToString());
                    seriesC.Visible = true;
                }
            }
           
            if (dt2.Rows.Count > 0)
            {
                seriesA.Name = dt2.Rows[0]["ServerNameA"].ToString();
            }
            ClusterRepWebChartControl1.Series.Add(seriesA);
            if (dt2.Rows.Count > 0)
            {
                seriesB.Name = dt2.Rows[0]["ServerNameB"].ToString();
            }
            ClusterRepWebChartControl1.Series.Add(seriesB);
            if (Session["ServerCName"] != "")
            {
                if (dt2.Rows.Count > 0)
                {
                    seriesC.Name = dt2.Rows[0]["ServerNameC"].ToString();
                }
                ClusterRepWebChartControl1.Series.Add(seriesC);
            }
            //10/30/2013 NS added - point labels on series
            //series.LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;

            ClusterRepWebChartControl1.Legend.Visible = true;

            //((SideBySideBarSeriesView)series.View).ColorEach = true;

            XYDiagram seriesXY = (XYDiagram)ClusterRepWebChartControl1.Diagram;
            // seriesXY.AxisX.Title.Text = "Milliseconds";
            // seriesXY.AxisX.Title.Visible = true;
            ((DevExpress.XtraCharts.XYDiagram)ClusterRepWebChartControl1.Diagram).Rotated = true;
            //11/16/2013 NS modified the AllowRotate option to false, otherwise the server names were unreadable
            seriesXY.AxisX.Label.ResolveOverlappingOptions.AllowRotate = false;
            seriesXY.AxisX.Label.ResolveOverlappingOptions.AllowHide = false;
            seriesXY.AxisY.Title.Text = "Documents";
            seriesXY.AxisY.Title.Visible = true;

            AxisBase axisy = ((XYDiagram)ClusterRepWebChartControl1.Diagram).AxisY;
            //4/18/2014 NS modified for VSPLUS-312
            axisy.Range.AlwaysShowZeroLevel = true;
            //9/2/2015 NS commented out for VSPLUS-2117
            /*
            double min = Convert.ToDouble(((XYDiagram)ClusterRepWebChartControl1.Diagram).AxisY.Range.MinValue);
            double max = Convert.ToDouble(((XYDiagram)ClusterRepWebChartControl1.Diagram).AxisY.Range.MaxValue);

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
            */
            //2/6/2013 NS modified chart height calculations based on the number of rows
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows.Count < 4)
                {
                    ClusterRepWebChartControl1.Height = 200;
                }
                else
                {
                    if (dt.Rows.Count >= 4 && dt.Rows.Count < 10)
                    {
                        ClusterRepWebChartControl1.Height = ((dt.Rows.Count) * 50) + 20;
                    }
                    else
                    {
                        if (dt.Rows.Count >= 10 && dt.Rows.Count < 100)
                        {
                            ClusterRepWebChartControl1.Height = ((dt.Rows.Count) * 40) + 20;
                        }
                        else
                        {
                            if (dt.Rows.Count >= 100)
                            {
                                //10/13/2015 NS modified
                                //ClusterRepWebChartControl1.Height = ((dt.Rows.Count) * 20) + 20;
                                ClusterRepWebChartControl1.Height = 10000;
                            }
                        }
                    }
                }
            }
            ClusterRepWebChartControl1.DataSource = dt;
            ClusterRepWebChartControl1.DataBind();
        }

        public void GraphForClusterRepDBSizeProblems(string ClusterName)
        {
            DataTable dt2 = VSWebBL.ReportsBL.ReportsBL.Ins.GetDBClusterServers(ClusterName);
            ClusterRepWebChartControl2.Series.Clear();
            DataTable dtraw = VSWebBL.ReportsBL.ReportsBL.Ins.GetDBClusterInfo(ClusterName);
            DataTable dt = GetThresholdList(dtraw, "DocCountA", "DocCountB", "DocCountC", "DBSizeA", "DBSizeB", "DBSizeC", false);
            if (dt.Rows.Count > 0)
            {
                ClusterRepWebChartControl2.DataSource = dt;
            }
            //ClusterRepWebChartControl.SeriesDataMember = dt.Columns["ServerNameA"].ToString();
            //ClusterRepWebChartControl.SeriesTemplate.ArgumentDataMember = "DatabaseName";
            //ClusterRepWebChartControl.SeriesTemplate.ValueDataMembers.AddRange(new string[] { dt.Columns["DocCountA"].ToString() });

            Series seriesA = new Series("ServerNameA", ViewType.Bar);
            seriesA.DataSource = dt;
            seriesA.ArgumentDataMember = "DatabaseName";
            ValueDataMemberCollection seriesValueDataMembersA = (ValueDataMemberCollection)seriesA.ValueDataMembers;
            seriesValueDataMembersA.AddRange(dt.Columns["DBSizeA"].ToString());
            seriesA.Visible = true;
            if (dt2.Rows.Count > 0)
            {
                seriesA.Name = dt2.Rows[0]["ServerNameA"].ToString();
            }
            ClusterRepWebChartControl2.Series.Add(seriesA);

            Series seriesB = new Series("ServerNameB", ViewType.Bar);
            seriesB.DataSource = dt;
            seriesB.ArgumentDataMember = "DatabaseName";
            ValueDataMemberCollection seriesValueDataMembersB = (ValueDataMemberCollection)seriesB.ValueDataMembers;
            seriesValueDataMembersB.AddRange(dt.Columns["DBSizeB"].ToString());
            seriesB.Visible = true;
            if (dt2.Rows.Count > 0)
            {
                seriesB.Name = dt2.Rows[0]["ServerNameB"].ToString();
            }
            ClusterRepWebChartControl2.Series.Add(seriesB);
            if (Session["ServerCName"] != "")
            {
                Series seriesC = new Series("ServerNameC", ViewType.Bar);
                seriesC.DataSource = dt;
                seriesC.ArgumentDataMember = "DatabaseName";
                ValueDataMemberCollection seriesValueDataMembersC = (ValueDataMemberCollection)seriesC.ValueDataMembers;
                seriesValueDataMembersC.AddRange(dt.Columns["DBSizeC"].ToString());
                seriesC.Visible = true;

                if (dt2.Rows.Count > 0)
                {
                    seriesC.Name = dt2.Rows[0]["ServerNameC"].ToString();
                }
                ClusterRepWebChartControl2.Series.Add(seriesC);
            }
            //10/30/2013 NS added - point labels on series
            //series.LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;

            ClusterRepWebChartControl2.Legend.Visible = true;

            //((SideBySideBarSeriesView)series.View).ColorEach = true;

            XYDiagram seriesXY = (XYDiagram)ClusterRepWebChartControl2.Diagram;
            // seriesXY.AxisX.Title.Text = "Milliseconds";
            // seriesXY.AxisX.Title.Visible = true;
            ((DevExpress.XtraCharts.XYDiagram)ClusterRepWebChartControl2.Diagram).Rotated = true;
            //11/16/2013 NS modified the AllowRotate option to false, otherwise the server names were unreadable
            seriesXY.AxisX.Label.ResolveOverlappingOptions.AllowRotate = false;
            seriesXY.AxisX.Label.ResolveOverlappingOptions.AllowHide = false;
            seriesXY.AxisY.Title.Text = "DB Size (MB)";
            seriesXY.AxisY.Title.Visible = true;

            AxisBase axisy = ((XYDiagram)ClusterRepWebChartControl2.Diagram).AxisY;
            //4/18/2014 NS modified for VSPLUS-312
            axisy.Range.AlwaysShowZeroLevel = true;
            //9/2/2015 NS commented out for VSPLUS-2117
            /*
            double min = Convert.ToDouble(((XYDiagram)ClusterRepWebChartControl2.Diagram).AxisY.Range.MinValue);
            double max = Convert.ToDouble(((XYDiagram)ClusterRepWebChartControl2.Diagram).AxisY.Range.MaxValue);

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
            */
            //2/6/2013 NS modified chart height calculations based on the number of rows
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows.Count < 4)
                {
                    ClusterRepWebChartControl2.Height = 200;
                }
                else
                {
                    if (dt.Rows.Count >= 4 && dt.Rows.Count < 10)
                    {
                        ClusterRepWebChartControl2.Height = ((dt.Rows.Count) * 50) + 20;
                    }
                    else
                    {
                        if (dt.Rows.Count >= 10 && dt.Rows.Count < 100)
                        {
                            ClusterRepWebChartControl2.Height = ((dt.Rows.Count) * 40) + 20;
                        }
                        else
                        {
                            if (dt.Rows.Count >= 100)
                            {
                                //10/13/2015 NS modified
                                //ClusterRepWebChartControl2.Height = ((dt.Rows.Count) * 20) + 20;
                                ClusterRepWebChartControl2.Height = 10000;
                            }
                        }
                    }
                }
            }
            ClusterRepWebChartControl2.DataSource = dt;
            ClusterRepWebChartControl2.DataBind();
        }

        protected void ClusterRepWebChartControl1_CustomCallback(object sender, DevExpress.XtraCharts.Web.CustomCallbackEventArgs e)
        {
            ClusterRepWebChartControl1.Width = new Unit(Convert.ToInt32(chartWidth.Value));
        }

        protected void ClusterRepWebChartControl2_CustomCallback(object sender, DevExpress.XtraCharts.Web.CustomCallbackEventArgs e)
        {
            ClusterRepWebChartControl2.Width = new Unit(Convert.ToInt32(chartWidth.Value));
        }

        //17/3/2016 sowmya added for VSPLUS 2455
        public void Fillclusterinfogrid()
        {
            DataTable dt = VSWebBL.ReportsBL.ReportsBL.Ins.GetclusterinfoBL();
            ClusterInfoGrid.DataSource = dt;
            ClusterInfoGrid.DataBind();

        }

        //9/15/2014 NS added for VSPLUS-921
        //public void FillCombobox()
        //{
        //    DataTable dt = VSWebBL.ReportsBL.ReportsBL.Ins.GetDBClusterNames();
        //    ClusterNameComboBox1.DataSource = dt;
        //    ClusterNameComboBox1.TextField = "ClusterName";
        //    ClusterNameComboBox1.DataBind();
        //    //10/17/2014 NS modified for VSPLUS-1024
        //    if (dt.Rows.Count > 0)
        //    {
        //        ClusterNameComboBox1.SelectedItem = ClusterNameComboBox1.Items[0];
        //    }
        //}

      //  protected void ClusterNameComboBox1_SelectedIndexChanged(object sender, EventArgs e)
       // {
            //bool ShowAll = false;

            //ClusterName = ClusterNameComboBox1.SelectedItem.ToString();
            //if (ShowProblemsRadioButtonList.SelectedItem.Value.ToString() == "0")
            //{
            //    ShowAll = true;
            //}
            //FillPotentialIssuesGrid(ClusterName, ShowAll);
            //GraphForClusterRepDocCountProblems(ClusterName);
            //GraphForClusterRepDBSizeProblems(ClusterName);
      //  }

        public DataTable GetThresholdList(DataTable dt, string paramA1, string paramB1, string paramC1, string paramA2, string paramB2, string paramC2, bool ShowAll)
        {
            double doccountA = 0;
            double doccountB = 0;
            double doccountC = 0;
            double dbsizeA = 0;
            double dbsizeB = 0;
            double dbsizeC = 0;
            double diffcount = 0;
            double diffsize = 0;
            double maxdoccount = 0;
            double mindoccount = 0;
            double maxdbsize = 0;
            double mindbsize = 0;

            //   for (int i = dt.Rows.Count - 1; i >= 0; i--)
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                doccountA = double.Parse(dt.Rows[i][paramA1].ToString());
                doccountB = double.Parse(dt.Rows[i][paramB1].ToString());
                doccountC = double.Parse(dt.Rows[i][paramC1].ToString());
                dbsizeA = double.Parse(dt.Rows[i][paramA2].ToString());
                dbsizeB = double.Parse(dt.Rows[i][paramB2].ToString());
                dbsizeC = double.Parse(dt.Rows[i][paramC2].ToString());
                if (dt.Rows[i]["ServerNameC"].ToString() != "")
                {
                    //Find the max value of the three values
                    maxdoccount = Math.Max(doccountC, Math.Max(doccountA, doccountB));
                    //Find the min value of the three values
                    mindoccount = Math.Min(doccountC, Math.Min(doccountA, doccountB));

                    maxdbsize = Math.Max(dbsizeC, Math.Max(dbsizeA, dbsizeB));
                    mindbsize = Math.Min(dbsizeC, Math.Min(dbsizeA, dbsizeB));
                }
                else
                {
                    //Find the max value of the two values
                    maxdoccount = Math.Max(doccountA, doccountB);
                    //Find the min value of the two values
                    mindoccount = Math.Min(doccountA, doccountB);

                    maxdbsize = Math.Max(dbsizeA, dbsizeB);
                    mindbsize = Math.Min(dbsizeA, dbsizeB);
                }
                //9/9/2015 NS modified
                if (maxdoccount != 0)
                {
                    diffcount = Convert.ToDouble((maxdoccount - mindoccount) / maxdoccount * 100);
                }
                if (maxdbsize != 0)
                {
                    diffsize = Convert.ToDouble((maxdbsize - mindbsize) / maxdbsize * 100);
                }
                //int threshold = int.Parse(lblFailureThreshold.Text.Substring(0, lblFailureThreshold.Text.Length - 1));
                double ythreshold = (0.8 * threshold);
                double myPercent = 0;
                if (maxdoccount > 0)
                {
                    myPercent = 100 - (mindoccount * 100 / maxdoccount);
                }

                   //8/3/2016 sowmya modified for VSPLUS 2455

                //if (myPercent >= threshold || maxdoccount == 0 && mindoccount == 0)
                //{
                //    dt.Rows[i]["color"] = "Red/White";
                //}
                //else if (myPercent >= ythreshold && myPercent < threshold)
                //{
                //    dt.Rows[i]["color"] = "Yellow/Black";
                //}
                //else
                //{
                //    dt.Rows[i]["color"] = "Green/White";
                //}

                if (!ShowAll)
                {
                    //5/25/2016 NS modified - removing DB size from consideration
                    if (diffcount <= threshold && threshold != 0)
                    {
                        dt.Rows[i].Delete();
                    }
                }
            }
            dt.AcceptChanges();
            return dt;
        }

        protected void PotentialIssuesGrid_PageSizeChanged(object sender, EventArgs e)
        {
            VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("PotentialClusterIssues|PotentialIssuesGrid", PotentialIssuesGrid.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        }
        //17/3/2016 sowmya added for VSPLUS 2455
        protected void ClusterInfoGrid_PageSizeChanged(object sender, EventArgs e)
        {
            VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("ClusterDB|ClusterInfoGrid", ClusterInfoGrid.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        }

        protected void PotentialIssuesGrid_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
        {
           
        }
        //9/2/2015 NS added for VSPLUS-2117
        protected void ClusterRepWebChartControl1_BoundDataChanged(object sender, EventArgs e)
        {
            UI uiobj = new UI();
            uiobj.RecalibrateChartAxes(ClusterRepWebChartControl1.Diagram);
        }
        //9/2/2015 NS added for VSPLUS-2117
        protected void ClusterRepWebChartControl2_BoundDataChanged(object sender, EventArgs e)
        {
            UI uiobj = new UI();
            uiobj.RecalibrateChartAxes(ClusterRepWebChartControl2.Diagram);
        }

        //10/2/2016 Sowmya Added for VSPLUS 2455
        protected void ASPxMenu1_ItemClick(object source, DevExpress.Web.MenuItemEventArgs e)
        {
            if (e.Item.Name == "ScanItem")
            {
                try
                {
                    DominoCluster ClusterObj = new DominoCluster();
                    ClusterObj.Name = Session["ClustName"] == null ? "" : Session["ClustName"].ToString();
                        if (ClusterObj.Name != "")
                        {
                            ClusterObj.ClusterScan = "Ready";
                            //2/22/2016 NS modified for VSPLUS-2629
                            bool returnvalue = VSWebBL.SettingBL.SettingsBL.Ins.UpdateScanvalue("ScanDominoClusterASAP", ClusterObj.Name, VSWeb.Constants.Constants.SysString);
                            successDiv.Style.Value = "display: block";
                            successDiv.InnerHtml = "Cluster scan is initiated, please allow a few minutes for the data to refresh." +
                               "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                            ErrorDiv.Style.Value = "display: none";
                        }

                        else
                        {
                            successDiv.Style.Value = "display:none";
                            ErrorDiv.Style.Value = "display: block";
                            ErrorDiv.InnerHtml = "There are no clusters to scan." +
                                "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                        }
                }
                catch (Exception ex)
                {
                    Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                    throw ex;
                }
            }

        }
        //17/3/2016 sowmya added for VSPLUS 2455
        //5/9/2016 Sowjanya modified for VSPLUS-2931
        protected void PotentialIssuesGrid_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            int docCount1 = Convert.ToInt32(e.GetValue("DocCountA"));
            int docCount2 = Convert.ToInt32(e.GetValue("DocCountB"));
            int docCount3 = Convert.ToInt32(e.GetValue("DocCountC"));
            int dbsize1 = Convert.ToInt32(e.GetValue("DBSizeA"));
            int dbsize2 = Convert.ToInt32(e.GetValue("DBSizeB"));
            int Threshold = Convert.ToInt32(Session["Thresholdvalue"].ToString());

            

            bool isMatched = (docCount1 != docCount2 && docCount3 != docCount2) ? ((docCount1 != docCount3) ? true : false) : false;

           
            if (Session["ServerCName"] != "")
                {
                    //if (isMatched)
                    //{
                        if (docCount1 == 0 || docCount2 == 0 || docCount3 == 0)
                        {
                            if (e.DataColumn.FieldName == "DocCountA"  || e.DataColumn.FieldName == "DocCountB"  || e.DataColumn.FieldName == "DocCountC")
                            {
                                if (!(docCount1 == 0 && docCount2 == 0 && docCount3 == 0))
                                {
                                    e.Cell.BackColor = System.Drawing.Color.Red;
                                    e.Cell.ForeColor = System.Drawing.Color.Black;
                                }
                            }
                           
                        }

                        if (docCount1 > 0 && docCount2 > 0 && docCount3 > 0)
                        {
                            
                                if (Math.Abs(docCount1 - docCount2) < Threshold && Math.Abs(docCount2 - docCount3) < Threshold &&  Math.Abs(docCount3 - docCount1) < Threshold 
                                    && (docCount1 != docCount2 || docCount1 != docCount3 || docCount2 != docCount3))
                                {
                                    if (e.DataColumn.FieldName == "DocCountA" || e.DataColumn.FieldName == "DocCountB" || e.DataColumn.FieldName == "DocCountC")
                                    {
                                        e.Cell.BackColor = System.Drawing.Color.Yellow;
                                        e.Cell.ForeColor = System.Drawing.Color.Black;
                                    }
                                    

                                }
                                if (Math.Abs(docCount1 - docCount2) >= Threshold &&  Math.Abs(docCount2 - docCount3) >= Threshold && Math.Abs(docCount3 - docCount1) >= Threshold)
                                {
                                    if (e.DataColumn.FieldName == "DocCountA" || e.DataColumn.FieldName == "DocCountB" || e.DataColumn.FieldName == "DocCountC")
                                    {
                                        e.Cell.BackColor = System.Drawing.Color.Red;
                                        e.Cell.ForeColor = System.Drawing.Color.Black;
                                    }
                                   
                            }
                           
                           
                        }
                       
                      
                }

                else
                {
                   

                        if (docCount1 == 0 || docCount2 == 0 )
                        {
                            if (e.DataColumn.FieldName == "DocCountA" || e.DataColumn.FieldName == "DocCountB")
                            {
                                if (!(docCount1 == 0 && docCount2 == 0))
                                {
                                    e.Cell.BackColor = System.Drawing.Color.Red;
                                    e.Cell.ForeColor = System.Drawing.Color.Black;
                                }
                            }
                           
                           
                        }
                          
                if(docCount1 > 0 && docCount2 > 0)
                 {
                   
                        if (Math.Abs(docCount1 - docCount2) < Threshold && docCount1 != docCount2)
                        {
                            if (e.DataColumn.FieldName == "DocCountA" || e.DataColumn.FieldName == "DocCountB")
                            {
                                e.Cell.BackColor = System.Drawing.Color.Yellow;
                                e.Cell.ForeColor = System.Drawing.Color.Black;
                            }
                           
                        }
                        if (Math.Abs(docCount1 - docCount2) >= Threshold)
                        {
                            if (e.DataColumn.FieldName == "DocCountA" || e.DataColumn.FieldName == "DocCountB")
                            {
                                e.Cell.BackColor = System.Drawing.Color.Red;
                                e.Cell.ForeColor = System.Drawing.Color.Black;
                            }
                            
                           
                        }

                }

                 
                }
           
        }
           

        //4/11/2016 NS added for VSPLUS-2840
        protected void ClusterInfoGrid_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            if (e.CellValue.ToString() == "OK" || e.CellValue.ToString() == "Scanning")
            {
                e.Cell.BackColor = System.Drawing.Color.LightGreen;
            }
            else if (e.CellValue.ToString().ToUpper().Contains("FAIL") || e.CellValue.ToString().ToUpper().Contains("FAILURE"))
            {
                e.Cell.BackColor = System.Drawing.Color.Red;
                e.Cell.ForeColor = System.Drawing.Color.White;
            }
            else if (e.CellValue.ToString().ToUpper().Contains("ISSUE"))
            {
                e.Cell.BackColor = System.Drawing.Color.Yellow;
            }
            else if (e.CellValue.ToString() == "Not Scanned")
            {
                e.Cell.BackColor = System.Drawing.Color.FromName("#87CEEB");
                e.Cell.ForeColor = System.Drawing.Color.Black;
            }
        }
    }
}













           

        

        
   
   