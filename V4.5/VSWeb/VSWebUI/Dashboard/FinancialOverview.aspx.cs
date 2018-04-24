using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.HtmlControls;
using DevExpress.XtraCharts;
using DevExpress.Web;
using System.Globalization;

namespace VSWebUI.Dashboard
{
    public partial class FinancialOverview : System.Web.UI.Page
    {
        string selectedServer = "";
        int ID;
        //18-04-2016 Durga Modified for VSPLUS-2866
		protected void Page_PreInit(object sender, EventArgs e)
		{

			this.Master.contentCallEvent += new EventHandler(Master_ButtonClick);
		}

		private void Master_ButtonClick(object sender, EventArgs e)
		{

		}
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                HtmlGenericControl body = (HtmlGenericControl)Master.FindControl("Body1");
                body.Attributes.Add("onload", "DoCallback()");
                body.Attributes.Add("onResize", "Resized()");
            }

            SetGraphForMonthlyExpenditurebyType();
            SetGraphForMonthlyExpenditurebyLocation();
            SetGraphForMonthlyExpenditurebyCategory();
            SetGraphForMostUtilizedServers();
            SetGraphForLeastUtilizedServers();
            SetGraphForCostPerUserServed();
        }




        public DataTable SetGraphForMonthlyExpenditurebyType()
        {
            try
            {
                // CurrencySymbol;
                MonthlyExpenditurebyTypeChart.Series.Clear();
                DataTable dt = VSWebBL.DashboardBL.KeyMetricsBL.Ins.GetMonthlyExpenditureDetails("ServerType");
                //6/3/2016 Sowjanya modified for VSPLUS-2999
                DataTable CurrencySymboldatatable = VSWebBL.DashboardBL.KeyMetricsBL.Ins.GetCurrencySymbol("CurrencySymbol");
                if (CurrencySymboldatatable.Rows.Count > 0)
                {
                    String CurrencySymbol = CurrencySymboldatatable.Rows[0]["svalue"].ToString();
                    Session["CurrencySymbol"] = CurrencySymbol;
                }
                    if (dt.Rows.Count > 0)
                    {
                        MonthlyExpenditurebyTypeChart.SeriesDataMember = "ServerType";
                        MonthlyExpenditurebyTypeChart.SeriesTemplate.ArgumentDataMember = "ServerType";
                        MonthlyExpenditurebyTypeChart.SeriesTemplate.ValueDataMembers.AddRange(new string[] { "MonthlyExpenditure" });
                        XYDiagram d = (XYDiagram)MonthlyExpenditurebyTypeChart.Diagram;
                        d.AxisX.Reverse = true;
                        if (Session["CurrencySymbol"] != null && Session["CurrencySymbol"] != "")
                        {
                            d.AxisY.Title.Text = Session["CurrencySymbol"].ToString();
                        }
                        ((BarSeriesView)MonthlyExpenditurebyTypeChart.SeriesTemplate.View).BarWidth = 1;
                        MonthlyExpenditurebyTypeChart.DataSource = dt;
                        MonthlyExpenditurebyTypeChart.DataBind();
                    }
                
                return dt;
            }
            catch (Exception ex)
            {
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
        }

        public DataTable SetGraphForMonthlyExpenditurebyLocation()
        {
              try
            {
            MonthlyExpenditurebyLocationChart.Series.Clear();
            DataTable dt = VSWebBL.DashboardBL.KeyMetricsBL.Ins.GetMonthlyExpenditureDetails("Location");
            if (dt.Rows.Count > 0)
            {
                MonthlyExpenditurebyLocationChart.SeriesDataMember = "Location";
                MonthlyExpenditurebyLocationChart.SeriesTemplate.ArgumentDataMember = "Location";
                MonthlyExpenditurebyLocationChart.SeriesTemplate.ValueDataMembers.AddRange(new string[] { "MonthlyExpenditure" });
                XYDiagram d = (XYDiagram)MonthlyExpenditurebyLocationChart.Diagram;
                d.AxisX.Reverse = true;
                //6/3/2016 Sowjanya modified for VSPLUS-2999
                if (Session["CurrencySymbol"] != null && Session["CurrencySymbol"] != "")
                {
                    d.AxisY.Title.Text = Session["CurrencySymbol"].ToString();
                }
                ((BarSeriesView)MonthlyExpenditurebyLocationChart.SeriesTemplate.View).BarWidth = 1;
                MonthlyExpenditurebyLocationChart.DataSource = dt;
                MonthlyExpenditurebyLocationChart.DataBind();
            }
            return dt;
            }
              catch (Exception ex)
              {
                  //6/27/2014 NS added for VSPLUS-634
                  Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                  throw ex;
              }
        }
      
        public DataTable SetGraphForMonthlyExpenditurebyCategory()
        {
            try
            {
            MonthlyExpenditurebyCategoryChart.Series.Clear();
            DataTable dt = VSWebBL.DashboardBL.KeyMetricsBL.Ins.GetMonthlyExpenditureDetails("Category");
            if (dt.Rows.Count > 0)
            {
                MonthlyExpenditurebyCategoryChart.SeriesDataMember = "Description";
                MonthlyExpenditurebyCategoryChart.SeriesTemplate.ArgumentDataMember = "Description";
                MonthlyExpenditurebyCategoryChart.SeriesTemplate.ValueDataMembers.AddRange(new string[] { "MonthlyExpenditure" });
                XYDiagram d = (XYDiagram)MonthlyExpenditurebyCategoryChart.Diagram;
                d.AxisX.Reverse = true;
                //6/3/2016 Sowjanya modified for VSPLUS-2999
                if (Session["CurrencySymbol"] != null && Session["CurrencySymbol"] != "")
                {
                    d.AxisY.Title.Text = Session["CurrencySymbol"].ToString();
                }
                ((BarSeriesView)MonthlyExpenditurebyCategoryChart.SeriesTemplate.View).BarWidth = 1;
                MonthlyExpenditurebyCategoryChart.DataSource = dt;
                MonthlyExpenditurebyCategoryChart.DataBind();
            }
            return dt;
              }
              catch (Exception ex)
              {
                  //6/27/2014 NS added for VSPLUS-634
                  Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                  throw ex;
              }
        }
        public DataTable SetGraphForMostUtilizedServers()
        {
             try
            {
            MostUtilizedServersChart.Series.Clear();
            DataTable dt = VSWebBL.DashboardBL.KeyMetricsBL.Ins.GetMostUtilizedServers();
            Session["MostUtilizedServers"] = null;
            if (dt.Rows.Count > 0)
            {
                double IdealUserCount, StatValue;
             
               dt.Columns.Add("PercentUtilization", typeof(double));
              
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["IdealUserCount"].ToString() != "0")
                    {
                        IdealUserCount = Convert.ToDouble(dt.Rows[i]["IdealUserCount"].ToString() == "" ? "0" : dt.Rows[i]["IdealUserCount"].ToString());
                        StatValue = Convert.ToDouble(dt.Rows[i]["StatValue"].ToString() == "" ? "0" : dt.Rows[i]["StatValue"].ToString());
                        double Percentage = Math.Round(((StatValue / IdealUserCount) * 100), 2);
                        dt.Rows[i]["PercentUtilization"] = Percentage;
                    }

                }
                Session["MostUtilizedServers"] = dt;

                dt.DefaultView.Sort = "PercentUtilization desc";

                DataTable Sortdt = dt.DefaultView.ToTable();
                if (Sortdt.Rows.Count > 0)
                {
                    DataTable Top5records = Sortdt.Rows.Cast<System.Data.DataRow>().Take(5).CopyToDataTable();
                    if (Top5records.Rows.Count > 0)
                    {
                        MostUtilizedServersChart.SeriesDataMember = "ServerName";
                        MostUtilizedServersChart.SeriesTemplate.ArgumentDataMember = "ServerName";
                        MostUtilizedServersChart.SeriesTemplate.ValueDataMembers.AddRange( "PercentUtilization" );
                        XYDiagram d = (XYDiagram)MostUtilizedServersChart.Diagram;
                        d.AxisX.Reverse = true;
                        d.AxisX.Title.Text = "Percentage";
                        

                        //XYDiagram axs = (XYDiagram)MostUtilizedServersChart.Diagram;
                     
                        ((BarSeriesView)MostUtilizedServersChart.SeriesTemplate.View).BarWidth = 1;
                        MostUtilizedServersChart.DataSource = Top5records;
                        MostUtilizedServersChart.DataBind();
                    }
                }
            }
            return dt;
             }
              catch (Exception ex)
              {
                  //6/27/2014 NS added for VSPLUS-634
                  Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                  throw ex;
              }
        }
        public void SetGraphForLeastUtilizedServers()
        {
            DataTable dt;
           
             try
            {
               
            LeastUtilizedServersChart.Series.Clear();
            if (Session["MostUtilizedServers"] != null)
            {
                dt= (DataTable)Session["MostUtilizedServers"];
            
                dt.DefaultView.Sort = "PercentUtilization ASC";
                DataTable Sortdt = dt.DefaultView.ToTable();
                if (Sortdt.Rows.Count > 0)
                {
                    DataTable Top5records = Sortdt.Rows.Cast<System.Data.DataRow>().Take(5).CopyToDataTable();
                    if (Top5records.Rows.Count > 0)
                    {
                        LeastUtilizedServersChart.SeriesDataMember = "ServerName";
                        LeastUtilizedServersChart.SeriesTemplate.ArgumentDataMember = "ServerName";
                        LeastUtilizedServersChart.SeriesTemplate.ValueDataMembers.AddRange("PercentUtilization");
                        XYDiagram d = (XYDiagram)LeastUtilizedServersChart.Diagram;
                        d.AxisX.Reverse = true;
                        d.AxisY.Title.Text = "Percentage";
                        ((BarSeriesView)LeastUtilizedServersChart.SeriesTemplate.View).BarWidth = 1;
                        LeastUtilizedServersChart.DataSource = Top5records;
                        LeastUtilizedServersChart.DataBind();
                    }
                }
            }
           
           }

              catch (Exception ex)
              {
                  //6/27/2014 NS added for VSPLUS-634
                  Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                  throw ex;
              }
        }
        public DataTable SetGraphForCostPerUserServed()

        {
            try
            {
            CostPerUserServedChart.Series.Clear();
            DataTable dt = VSWebBL.DashboardBL.KeyMetricsBL.Ins.GetCostPerUserServedDetails();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    decimal avgusers = Convert.ToDecimal(dt.Rows[i]["StatValue"].ToString() == "" ? "0" : dt.Rows[i]["StatValue"].ToString());//dt.Rows[i]["MonthlyOperatingCost"]
                    decimal MonthlyopeartingCost = Convert.ToDecimal(dt.Rows[i]["MonthlyOperatingCost"].ToString() == "" ? "0" : dt.Rows[i]["MonthlyOperatingCost"].ToString());
                    if (avgusers != 0)
                        dt.Rows[i]["StatValue"] = Math.Round((MonthlyopeartingCost / avgusers), 2);
                    else
                        dt.Rows[i]["StatValue"] = "0";
                }
                dt.DefaultView.Sort = "StatValue desc";
                DataTable Sortdt = dt.DefaultView.ToTable();
                if (Sortdt.Rows.Count > 0)
                {
                    DataTable Top5records = Sortdt.Rows.Cast<System.Data.DataRow>().Take(5).CopyToDataTable();
                    if (Top5records.Rows.Count > 0)
                    {
                        CostPerUserServedChart.SeriesDataMember = "servername";
                        CostPerUserServedChart.SeriesTemplate.ArgumentDataMember = "servername";
                        CostPerUserServedChart.SeriesTemplate.ValueDataMembers.AddRange(new string[] { "StatValue" });
                        XYDiagram d = (XYDiagram)CostPerUserServedChart.Diagram;
                        d.AxisX.Reverse = true;
                        //6/3/2016 Sowjanya modified for VSPLUS-2999
                        if (Session["CurrencySymbol"] != null && Session["CurrencySymbol"] != "")
                        {
                            d.AxisY.Title.Text = Session["CurrencySymbol"].ToString();
                        }
                        ((BarSeriesView)CostPerUserServedChart.SeriesTemplate.View).BarWidth = 1;
                        CostPerUserServedChart.DataSource = Top5records;
                        CostPerUserServedChart.DataBind();
                    }
                }
            }
            return dt;
 }
              catch (Exception ex)
              {
                  //6/27/2014 NS added for VSPLUS-634
                  Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                  throw ex;
              }
        }
    }
}