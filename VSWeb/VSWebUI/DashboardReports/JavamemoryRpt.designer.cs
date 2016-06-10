namespace VSWebUI.DashboardReports
{
	partial class JavamemoryRpt
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
			DevExpress.XtraCharts.XYDiagram xyDiagram1 = new DevExpress.XtraCharts.XYDiagram();
			DevExpress.XtraCharts.AreaSeriesView areaSeriesView1 = new DevExpress.XtraCharts.AreaSeriesView();
			this.Detail = new DevExpress.XtraReports.UI.DetailBand();
			this.xrChart1 = new DevExpress.XtraReports.UI.XRChart();
			this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
			this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
			this.GroupHeader1 = new DevExpress.XtraReports.UI.GroupHeaderBand();
			this.MonthYearLabel = new DevExpress.XtraReports.UI.XRLabel();
			this.xrLine1 = new DevExpress.XtraReports.UI.XRLine();
			this.xrPageInfo1 = new DevExpress.XtraReports.UI.XRPageInfo();
			this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
			this.PageFooter = new DevExpress.XtraReports.UI.PageFooterBand();
			this.ServerName = new DevExpress.XtraReports.Parameters.Parameter();
			this.StartDate = new DevExpress.XtraReports.Parameters.Parameter();
			this.EndDate = new DevExpress.XtraReports.Parameters.Parameter();
			this.Threshold = new DevExpress.XtraReports.Parameters.Parameter();
			this.ServerType = new DevExpress.XtraReports.Parameters.Parameter();
			this.xrSubreport1 = new DevExpress.XtraReports.UI.XRSubreport();
			((System.ComponentModel.ISupportInitialize)(this.xrChart1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(xyDiagram1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(areaSeriesView1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
			// 
			// Detail
			// 
			this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrChart1});
			this.Detail.HeightF = 450.7083F;
			this.Detail.Name = "Detail";
			this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
			this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
			// 
			// xrChart1
			// 
			this.xrChart1.BackColor = System.Drawing.Color.Transparent;
			this.xrChart1.BorderColor = System.Drawing.Color.Black;
			this.xrChart1.Borders = DevExpress.XtraPrinting.BorderSide.None;
			xyDiagram1.AxisX.Title.Text = "Date";
			xyDiagram1.AxisX.Title.Visibility = DevExpress.Utils.DefaultBoolean.True;
			xyDiagram1.AxisX.VisibleInPanesSerializable = "-1";
			xyDiagram1.AxisX.WholeRange.AutoSideMargins = false;
			xyDiagram1.AxisX.WholeRange.SideMarginsValue = 0D;
			xyDiagram1.AxisY.Title.Text = "Allocated Java memory (MB)";
			xyDiagram1.AxisY.Title.Visibility = DevExpress.Utils.DefaultBoolean.True;
			xyDiagram1.AxisY.VisibleInPanesSerializable = "-1";
			xyDiagram1.AxisY.WholeRange.AutoSideMargins = true;
			xyDiagram1.DefaultPane.EnableAxisXScrolling = DevExpress.Utils.DefaultBoolean.False;
			xyDiagram1.DefaultPane.EnableAxisXZooming = DevExpress.Utils.DefaultBoolean.False;
			xyDiagram1.DefaultPane.EnableAxisYScrolling = DevExpress.Utils.DefaultBoolean.False;
			xyDiagram1.DefaultPane.EnableAxisYZooming = DevExpress.Utils.DefaultBoolean.False;
			this.xrChart1.Diagram = xyDiagram1;
			this.xrChart1.Legend.AlignmentHorizontal = DevExpress.XtraCharts.LegendAlignmentHorizontal.Center;
			this.xrChart1.Legend.AlignmentVertical = DevExpress.XtraCharts.LegendAlignmentVertical.BottomOutside;
			this.xrChart1.Legend.MaxVerticalPercentage = 30D;
			this.xrChart1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
			this.xrChart1.Name = "xrChart1";
			this.xrChart1.PaletteName = "Oriel";
			this.xrChart1.SeriesDataMember = "ServerName";
			this.xrChart1.SeriesSerializable = new DevExpress.XtraCharts.Series[0];
			this.xrChart1.SeriesTemplate.ArgumentDataMember = "Date";
			this.xrChart1.SeriesTemplate.ArgumentScaleType = DevExpress.XtraCharts.ScaleType.DateTime;
			this.xrChart1.SeriesTemplate.ValueDataMembersSerializable = "StatValue";
			this.xrChart1.SeriesTemplate.View = areaSeriesView1;
			this.xrChart1.SizeF = new System.Drawing.SizeF(900.7083F, 450.7083F);
			this.xrChart1.StylePriority.UseBackColor = false;
			// 
			// TopMargin
			// 
			this.TopMargin.HeightF = 4F;
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
			// GroupHeader1
			// 
			this.GroupHeader1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.MonthYearLabel,
            this.xrLine1,
            this.xrPageInfo1,
            this.xrLabel1});
			this.GroupHeader1.HeightF = 79F;
			this.GroupHeader1.Name = "GroupHeader1";
			// 
			// MonthYearLabel
			// 
			this.MonthYearLabel.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.MonthYearLabel.LocationFloat = new DevExpress.Utils.PointFloat(0F, 56F);
			this.MonthYearLabel.Name = "MonthYearLabel";
			this.MonthYearLabel.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
			this.MonthYearLabel.SizeF = new System.Drawing.SizeF(327.0833F, 23F);
			this.MonthYearLabel.StylePriority.UseFont = false;
			// 
			// xrLine1
			// 
			this.xrLine1.LineWidth = 2;
			this.xrLine1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 32.99999F);
			this.xrLine1.Name = "xrLine1";
			this.xrLine1.SizeF = new System.Drawing.SizeF(900.7083F, 23F);
			// 
			// xrPageInfo1
			// 
			this.xrPageInfo1.Font = new System.Drawing.Font("Tahoma", 8.25F);
			this.xrPageInfo1.LocationFloat = new DevExpress.Utils.PointFloat(655.7083F, 10.00001F);
			this.xrPageInfo1.Name = "xrPageInfo1";
			this.xrPageInfo1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
			this.xrPageInfo1.PageInfo = DevExpress.XtraPrinting.PageInfo.DateTime;
			this.xrPageInfo1.SizeF = new System.Drawing.SizeF(245F, 16F);
			this.xrPageInfo1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
			// 
			// xrLabel1
			// 
			this.xrLabel1.CanShrink = true;
			this.xrLabel1.Font = new System.Drawing.Font("Tahoma", 18F);
			this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
			this.xrLabel1.Name = "xrLabel1";
			this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
			this.xrLabel1.SizeF = new System.Drawing.SizeF(376.4166F, 33F);
			this.xrLabel1.StylePriority.UseTextAlignment = false;
			this.xrLabel1.Text = "Taveler Allocated Java Memory";
			this.xrLabel1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
			// 
			// PageFooter
			// 
			this.PageFooter.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrSubreport1});
			this.PageFooter.HeightF = 23F;
			this.PageFooter.Name = "PageFooter";
			// 
			// ServerName
			// 
			this.ServerName.Name = "ServerName";
			// 
			// StartDate
			// 
			this.StartDate.Name = "StartDate";
			this.StartDate.Type = typeof(System.DateTime);
			// 
			// EndDate
			// 
			this.EndDate.Name = "EndDate";
			this.EndDate.Type = typeof(System.DateTime);
			// 
			// Threshold
			// 
			this.Threshold.Name = "Threshold";
			// 
			// ServerType
			// 
			this.ServerType.Name = "ServerType";
			// 
			// xrSubreport1
			// 
			this.xrSubreport1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
			this.xrSubreport1.Name = "xrSubreport1";
			this.xrSubreport1.ReportSource = new VSWebUI.DashboardReports.CopyrightXtraRpt();
			this.xrSubreport1.SizeF = new System.Drawing.SizeF(100F, 23F);
			// 
			// JavamemoryRpt
			// 
			this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.GroupHeader1,
            this.PageFooter});
			this.Landscape = true;
			this.Margins = new System.Drawing.Printing.Margins(100, 99, 4, 100);
			this.PageColor = System.Drawing.Color.Transparent;
			this.PageHeight = 850;
			this.PageWidth = 1100;
			this.Parameters.AddRange(new DevExpress.XtraReports.Parameters.Parameter[] {
            this.ServerName,
            this.StartDate,
            this.EndDate,
            this.Threshold,
            this.ServerType});
			this.Version = "14.2";
			this.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.AVGCPURpt_BeforePrint);
			((System.ComponentModel.ISupportInitialize)(xyDiagram1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(areaSeriesView1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.xrChart1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.DetailBand Detail;
        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        private DevExpress.XtraReports.UI.XRChart xrChart1;
        //private AvgCpuUtil avgCpuUtil1;
        private DevExpress.XtraReports.UI.GroupHeaderBand GroupHeader1;
        private DevExpress.XtraReports.UI.XRLabel xrLabel1;
        private DevExpress.XtraReports.UI.PageFooterBand PageFooter;
        //private AvgCpuUtilTableAdapters.DominoSummaryStatsTableAdapter dominoSummaryStatsTableAdapter;
        private DevExpress.XtraReports.Parameters.Parameter ServerName;
        private DevExpress.XtraReports.Parameters.Parameter StartDate;
        private DevExpress.XtraReports.Parameters.Parameter EndDate;
        private DevExpress.XtraReports.UI.XRPageInfo xrPageInfo1;
        private DevExpress.XtraReports.UI.XRLine xrLine1;
        private DevExpress.XtraReports.UI.XRSubreport xrSubreport1;
        private DevExpress.XtraReports.UI.XRLabel MonthYearLabel;
        private DevExpress.XtraReports.Parameters.Parameter Threshold;
        private DevExpress.XtraReports.Parameters.Parameter ServerType;
    }
}
