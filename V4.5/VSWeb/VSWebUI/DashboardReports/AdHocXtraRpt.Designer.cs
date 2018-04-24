namespace VSWebUI.DashboardReports
{
    partial class AdHocXtraRpt
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
			this.Detail = new DevExpress.XtraReports.UI.DetailBand();
			this.xrPivotGrid1 = new DevExpress.XtraReports.UI.XRPivotGrid();
			this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
			this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
			this.PageFooter = new DevExpress.XtraReports.UI.PageFooterBand();
			this.xrSubreport1 = new DevExpress.XtraReports.UI.XRSubreport();
			this.GroupHeader1 = new DevExpress.XtraReports.UI.GroupHeaderBand();
			this.xrPageInfo1 = new DevExpress.XtraReports.UI.XRPageInfo();
			this.xrLine1 = new DevExpress.XtraReports.UI.XRLine();
			this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
			this.StatName = new DevExpress.XtraReports.Parameters.Parameter();
			this.GroupHeader2 = new DevExpress.XtraReports.UI.GroupHeaderBand();
			this.xrLabel3 = new DevExpress.XtraReports.UI.XRLabel();
			this.xrLabel2 = new DevExpress.XtraReports.UI.XRLabel();
			this.StatType = new DevExpress.XtraReports.Parameters.Parameter();
			this.StatSummaryType = new DevExpress.XtraReports.Parameters.Parameter();
			this.formattingRule1 = new DevExpress.XtraReports.UI.FormattingRule();
			this.StartDate = new DevExpress.XtraReports.Parameters.Parameter();
			this.EndDate = new DevExpress.XtraReports.Parameters.Parameter();
			((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
			// 
			// Detail
			// 
			this.Detail.HeightF = 52.08333F;
			this.Detail.KeepTogetherWithDetailReports = true;
			this.Detail.Name = "Detail";
			this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
			this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
			// 
			// xrPivotGrid1
			// 
			this.xrPivotGrid1.Appearance.Cell.Font = new System.Drawing.Font("Tahoma", 8.25F);
			this.xrPivotGrid1.Appearance.FieldHeader.BackColor = System.Drawing.Color.LightSkyBlue;
			this.xrPivotGrid1.Appearance.FieldHeader.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
			this.xrPivotGrid1.Appearance.FieldValue.BackColor = System.Drawing.Color.LightBlue;
			this.xrPivotGrid1.Appearance.FieldValue.Font = new System.Drawing.Font("Tahoma", 8.25F);
			this.xrPivotGrid1.Appearance.FieldValueTotal.BackColor = System.Drawing.Color.LightSkyBlue;
			this.xrPivotGrid1.Appearance.FieldValueTotal.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.xrPivotGrid1.Appearance.TotalCell.BackColor = System.Drawing.Color.Wheat;
			this.xrPivotGrid1.Appearance.TotalCell.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.xrPivotGrid1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 36.45833F);
			this.xrPivotGrid1.Name = "xrPivotGrid1";
			this.xrPivotGrid1.OptionsPrint.FilterSeparatorBarPadding = 3;
			this.xrPivotGrid1.OptionsView.ShowDataHeaders = false;
			this.xrPivotGrid1.OptionsView.ShowGrandTotalsForSingleValues = true;
			this.xrPivotGrid1.OptionsView.ShowRowHeaders = false;
			this.xrPivotGrid1.SizeF = new System.Drawing.SizeF(215F, 50F);
			this.xrPivotGrid1.FieldValueDisplayText += new System.EventHandler<DevExpress.XtraReports.UI.PivotGrid.PivotFieldDisplayTextEventArgs>(this.xrPivotGrid1_FieldValueDisplayText);
			this.xrPivotGrid1.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrPivotGrid1_BeforePrint);
			// 
			// TopMargin
			// 
			this.TopMargin.HeightF = 100F;
			this.TopMargin.Name = "TopMargin";
			this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
			this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
			// 
			// BottomMargin
			// 
			this.BottomMargin.HeightF = 100F;
			this.BottomMargin.Name = "BottomMargin";
			this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
			this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
			// 
			// PageFooter
			// 
			this.PageFooter.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrSubreport1});
			this.PageFooter.HeightF = 23.95833F;
			this.PageFooter.Name = "PageFooter";
			// 
			// xrSubreport1
			// 
			this.xrSubreport1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
			this.xrSubreport1.Name = "xrSubreport1";
			this.xrSubreport1.ReportSource = new VSWebUI.DashboardReports.CopyrightXtraRpt();
			this.xrSubreport1.SizeF = new System.Drawing.SizeF(100F, 23F);
			// 
			// GroupHeader1
			// 
			this.GroupHeader1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPageInfo1,
            this.xrLine1,
            this.xrLabel1});
			this.GroupHeader1.HeightF = 55.99999F;
			this.GroupHeader1.Level = 1;
			this.GroupHeader1.Name = "GroupHeader1";
			// 
			// xrPageInfo1
			// 
			this.xrPageInfo1.Font = new System.Drawing.Font("Tahoma", 8.25F);
			this.xrPageInfo1.LocationFloat = new DevExpress.Utils.PointFloat(655F, 17.00001F);
			this.xrPageInfo1.Name = "xrPageInfo1";
			this.xrPageInfo1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
			this.xrPageInfo1.PageInfo = DevExpress.XtraPrinting.PageInfo.DateTime;
			this.xrPageInfo1.SizeF = new System.Drawing.SizeF(245F, 16F);
			this.xrPageInfo1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
			// 
			// xrLine1
			// 
			this.xrLine1.LineWidth = 2;
			this.xrLine1.LocationFloat = new DevExpress.Utils.PointFloat(4.768372E-05F, 32.99999F);
			this.xrLine1.Name = "xrLine1";
			this.xrLine1.SizeF = new System.Drawing.SizeF(899.9999F, 23F);
			// 
			// xrLabel1
			// 
			this.xrLabel1.CanShrink = true;
			this.xrLabel1.Font = new System.Drawing.Font("Tahoma", 18F);
			this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
			this.xrLabel1.Name = "xrLabel1";
			this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
			this.xrLabel1.SizeF = new System.Drawing.SizeF(655F, 33F);
			this.xrLabel1.StylePriority.UseTextAlignment = false;
			this.xrLabel1.Text = "Report for <Statistic>";
			this.xrLabel1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
			// 
			// StatName
			// 
			this.StatName.Name = "StatName";
			// 
			// GroupHeader2
			// 
			this.GroupHeader2.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel3,
            this.xrLabel2,
            this.xrPivotGrid1});
			this.GroupHeader2.GroupUnion = DevExpress.XtraReports.UI.GroupUnion.WithFirstDetail;
			this.GroupHeader2.HeightF = 102.0833F;
			this.GroupHeader2.KeepTogether = true;
			this.GroupHeader2.Name = "GroupHeader2";
			this.GroupHeader2.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.GroupHeader2_BeforePrint);
			// 
			// xrLabel3
			// 
			this.xrLabel3.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.xrLabel3.LocationFloat = new DevExpress.Utils.PointFloat(4.768372E-05F, 0F);
			this.xrLabel3.Name = "xrLabel3";
			this.xrLabel3.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
			this.xrLabel3.SizeF = new System.Drawing.SizeF(127.0833F, 23F);
			this.xrLabel3.StylePriority.UseFont = false;
			this.xrLabel3.Text = "Week Number";
			// 
			// xrLabel2
			// 
			this.xrLabel2.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.xrLabel2.LocationFloat = new DevExpress.Utils.PointFloat(127.0834F, 0F);
			this.xrLabel2.Name = "xrLabel2";
			this.xrLabel2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
			this.xrLabel2.SizeF = new System.Drawing.SizeF(204.1666F, 23F);
			this.xrLabel2.StylePriority.UseFont = false;
			this.xrLabel2.StylePriority.UseTextAlignment = false;
			this.xrLabel2.Text = "xrLabel2";
			this.xrLabel2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
			// 
			// StatType
			// 
			this.StatType.Name = "StatType";
			// 
			// StatSummaryType
			// 
			this.StatSummaryType.Name = "StatSummaryType";
			// 
			// formattingRule1
			// 
			this.formattingRule1.Name = "formattingRule1";
			// 
			// StartDate
			// 
			this.StartDate.Name = "StartDate";
			// 
			// EndDate
			// 
			this.EndDate.Name = "EndDate";
			// 
			// AdHocXtraRpt
			// 
			this.BackColor = System.Drawing.Color.White;
			this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.PageFooter,
            this.GroupHeader1,
            this.GroupHeader2});
			this.FormattingRuleSheet.AddRange(new DevExpress.XtraReports.UI.FormattingRule[] {
            this.formattingRule1});
			this.Landscape = true;
			this.PageHeight = 850;
			this.PageWidth = 1100;
			this.Parameters.AddRange(new DevExpress.XtraReports.Parameters.Parameter[] {
            this.StatName,
            this.StatType,
            this.StatSummaryType,
            this.StartDate,
            this.EndDate});
			this.Version = "14.2";
			this.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.AdHocXtraRpt_BeforePrint);
			((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.DetailBand Detail;
        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        private DevExpress.XtraReports.UI.PageFooterBand PageFooter;
        private DevExpress.XtraReports.UI.XRSubreport xrSubreport1;
        private DevExpress.XtraReports.UI.GroupHeaderBand GroupHeader1;
        private DevExpress.XtraReports.Parameters.Parameter StatName;
        private DevExpress.XtraReports.UI.GroupHeaderBand GroupHeader2;
        private DevExpress.XtraReports.UI.XRLabel xrLabel1;
        private DevExpress.XtraReports.UI.XRLine xrLine1;
        private DevExpress.XtraReports.UI.XRPageInfo xrPageInfo1;
        private DevExpress.XtraReports.UI.XRPivotGrid xrPivotGrid1;
        private DevExpress.XtraReports.UI.XRLabel xrLabel2;
        private DevExpress.XtraReports.UI.XRLabel xrLabel3;
        private DevExpress.XtraReports.Parameters.Parameter StatType;
        private DevExpress.XtraReports.Parameters.Parameter StatSummaryType;
        private DevExpress.XtraReports.UI.FormattingRule formattingRule1;
        private DevExpress.XtraReports.Parameters.Parameter StartDate;
        private DevExpress.XtraReports.Parameters.Parameter EndDate;
    }
}
