using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;

namespace VSWebUI.DashboardReports
{
    public partial class AdHocXtraRpt : DevExpress.XtraReports.UI.XtraReport
    {
        DataTable dt;
        public AdHocXtraRpt()
        {
            InitializeComponent();
        }

        private void AdHocXtraRpt_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (this.StatName.Value.ToString() == "")
            {
                xrLabel1.Text = "Report for <Statistic>";
            }
            else
            {
                xrLabel1.Text = "Report for " + this.StatName.Value.ToString();
            }
            dt = VSWebBL.ReportsBL.ReportsBL.Ins.GetDominoStatValues(this.StatName.Value.ToString(),this.StatType.Value.ToString(),
                this.StartDate.Value.ToString(),this.EndDate.Value.ToString());
            if (dt.Rows.Count > 0)
            {
                this.DataSource = dt;
                xrPivotGrid1.Visible = true;
                xrPivotGrid1.DataSource = dt;
                xrLabel2.Visible = true;
                xrLabel3.Visible = true;
                xrLabel2.DataBindings.Add("Text", dt, "WeekNumber");
                xrPivotGrid1.Fields.Add("ServerName", DevExpress.XtraPivotGrid.PivotArea.RowArea);
                xrPivotGrid1.Fields.Add("StatValue", DevExpress.XtraPivotGrid.PivotArea.DataArea);
                //11/4/2014 NS added - it is critical to have WeekNumber added to the fields in the column area
                //for the Prefilter code below to work.
                xrPivotGrid1.Fields.Add("WeekNumber", DevExpress.XtraPivotGrid.PivotArea.ColumnArea);
                xrPivotGrid1.Fields.Add("MonthDay", DevExpress.XtraPivotGrid.PivotArea.ColumnArea);
                xrPivotGrid1.Fields["MonthDay"].Caption = "Month/Day/Year";
                xrPivotGrid1.Fields["MonthDay"].ValueFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
                xrPivotGrid1.Fields["MonthDay"].ValueFormat.FormatString = "MM/dd/yyyy";
                xrPivotGrid1.Fields["ServerName"].Caption = "Server";
			
                xrPivotGrid1.Fields["WeekNumber"].Visible = false;
                if (this.StatSummaryType.Value.ToString() == "0")
                {
                    xrPivotGrid1.Fields["StatValue"].SummaryType = DevExpress.Data.PivotGrid.PivotSummaryType.Sum;
                    xrPivotGrid1.Fields["ServerName"].SummaryType = DevExpress.Data.PivotGrid.PivotSummaryType.Sum;
                }
                else
                {
                    xrPivotGrid1.Fields["StatValue"].SummaryType = DevExpress.Data.PivotGrid.PivotSummaryType.Average;
                    xrPivotGrid1.Fields["ServerName"].SummaryType = DevExpress.Data.PivotGrid.PivotSummaryType.Average;
                    xrPivotGrid1.Fields["StatValue"].GrandTotalCellFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                    xrPivotGrid1.Fields["StatValue"].GrandTotalCellFormat.FormatString = "f2";
                }
            }
            else
            {
                xrPivotGrid1.Visible = false;
                xrLabel2.Visible = false;
                xrLabel3.Visible = false;
            }
        }

        private void xrPivotGrid1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            //11/4/2014 NS added - the Prefilter call below is the key to displaying pivot grid values in a 
            //grouped by Week Number manner. It is also critical to have WeekNumber field added to the grid
            //xrPivotGrid1.Fields.Add("WeekNumber", DevExpress.XtraPivotGrid.PivotArea.ColumnArea);
            //in order to be able to filter by that value.
            if (GetCurrentColumnValue("WeekNumber") != null)
            {
                xrPivotGrid1.Prefilter.CriteriaString = "[WeekNumber]==" + GetCurrentColumnValue("WeekNumber").ToString();
            }
            xrPivotGrid1.BestFit();
        }

        private void GroupHeader2_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            DevExpress.XtraReports.UI.GroupField objGroupField = new DevExpress.XtraReports.UI.GroupField();
            objGroupField.FieldName = "WeekNumber";
            GroupHeader2.GroupFields.Add(objGroupField);
        }

        private void xrPivotGrid1_FieldValueDisplayText(object sender, DevExpress.XtraReports.UI.PivotGrid.PivotFieldDisplayTextEventArgs e)
        {
            if (e.ValueType == DevExpress.XtraPivotGrid.PivotGridValueType.Total || e.ValueType == DevExpress.XtraPivotGrid.PivotGridValueType.GrandTotal)
            {
                if (this.StatSummaryType.Value.ToString() == "0")
                {
                    e.DisplayText = "Total";
                }
                else
                {
                    e.DisplayText = "Average";
                }
            }
        }
    }
}
