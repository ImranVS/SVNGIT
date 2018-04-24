namespace VSWebUI.ConfiguratorReports
{
    partial class MailThresholdXtraRpt
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
			this.xrTable2 = new DevExpress.XtraReports.UI.XRTable();
			this.xrTableRow2 = new DevExpress.XtraReports.UI.XRTableRow();
			this.mtServerName = new DevExpress.XtraReports.UI.XRTableCell();
			this.mtDeadThreshold = new DevExpress.XtraReports.UI.XRTableCell();
			this.mtPendingThreshold = new DevExpress.XtraReports.UI.XRTableCell();
			this.mtHeldMailThreshold = new DevExpress.XtraReports.UI.XRTableCell();
			this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
			this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
			this.GroupHeader1 = new DevExpress.XtraReports.UI.GroupHeaderBand();
			this.xrLine1 = new DevExpress.XtraReports.UI.XRLine();
			this.xrTable1 = new DevExpress.XtraReports.UI.XRTable();
			this.xrTableRow1 = new DevExpress.XtraReports.UI.XRTableRow();
			this.xrTableCell1 = new DevExpress.XtraReports.UI.XRTableCell();
			this.xrTableCell4 = new DevExpress.XtraReports.UI.XRTableCell();
			this.xrTableCell3 = new DevExpress.XtraReports.UI.XRTableCell();
			this.xrTableCell2 = new DevExpress.XtraReports.UI.XRTableCell();
			this.xrPageInfo1 = new DevExpress.XtraReports.UI.XRPageInfo();
			this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
			this.EvenStyle = new DevExpress.XtraReports.UI.XRControlStyle();
			this.OddStyle = new DevExpress.XtraReports.UI.XRControlStyle();
			this.PageFooter = new DevExpress.XtraReports.UI.PageFooterBand();
			this.xrSubreport1 = new DevExpress.XtraReports.UI.XRSubreport();
			((System.ComponentModel.ISupportInitialize)(this.xrTable2)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.xrTable1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
			// 
			// Detail
			// 
			this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable2});
			this.Detail.HeightF = 27.08333F;
			this.Detail.Name = "Detail";
			this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
			this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
			// 
			// xrTable2
			// 
			this.xrTable2.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
			this.xrTable2.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
			this.xrTable2.Name = "xrTable2";
			this.xrTable2.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow2});
			this.xrTable2.SizeF = new System.Drawing.SizeF(541.046F, 27.08333F);
			this.xrTable2.StylePriority.UseBorders = false;
			// 
			// xrTableRow2
			// 
			this.xrTableRow2.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.mtServerName,
            this.mtDeadThreshold,
            this.mtPendingThreshold,
            this.mtHeldMailThreshold});
			this.xrTableRow2.EvenStyleName = "EvenStyle";
			this.xrTableRow2.Name = "xrTableRow2";
			this.xrTableRow2.OddStyleName = "OddStyle";
			this.xrTableRow2.Weight = 1D;
			// 
			// mtServerName
			// 
			this.mtServerName.Name = "mtServerName";
			this.mtServerName.Padding = new DevExpress.XtraPrinting.PaddingInfo(7, 5, 3, 3, 100F);
			this.mtServerName.StylePriority.UsePadding = false;
			this.mtServerName.Text = "NA";
			this.mtServerName.Weight = 0.64726966271033648D;
			// 
			// mtDeadThreshold
			// 
			this.mtDeadThreshold.Name = "mtDeadThreshold";
			this.mtDeadThreshold.Padding = new DevExpress.XtraPrinting.PaddingInfo(7, 5, 3, 3, 100F);
			this.mtDeadThreshold.StylePriority.UsePadding = false;
			this.mtDeadThreshold.StylePriority.UseTextAlignment = false;
			this.mtDeadThreshold.Text = "NA";
			this.mtDeadThreshold.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
			this.mtDeadThreshold.Weight = 0.58863592087579442D;
			// 
			// mtPendingThreshold
			// 
			this.mtPendingThreshold.Name = "mtPendingThreshold";
			this.mtPendingThreshold.Padding = new DevExpress.XtraPrinting.PaddingInfo(7, 5, 3, 3, 100F);
			this.mtPendingThreshold.StylePriority.UsePadding = false;
			this.mtPendingThreshold.StylePriority.UseTextAlignment = false;
			this.mtPendingThreshold.Text = "NA";
			this.mtPendingThreshold.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
			this.mtPendingThreshold.Weight = 0.63332401442192543D;
			// 
			// mtHeldMailThreshold
			// 
			this.mtHeldMailThreshold.Name = "mtHeldMailThreshold";
			this.mtHeldMailThreshold.Padding = new DevExpress.XtraPrinting.PaddingInfo(7, 5, 3, 3, 100F);
			this.mtHeldMailThreshold.StylePriority.UsePadding = false;
			this.mtHeldMailThreshold.StylePriority.UseTextAlignment = false;
			this.mtHeldMailThreshold.Text = "NA";
			this.mtHeldMailThreshold.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
			this.mtHeldMailThreshold.Weight = 0.62790563396405608D;
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
			// GroupHeader1
			// 
			this.GroupHeader1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLine1,
            this.xrTable1,
            this.xrPageInfo1,
            this.xrLabel1});
			this.GroupHeader1.GroupUnion = DevExpress.XtraReports.UI.GroupUnion.WithFirstDetail;
			this.GroupHeader1.HeightF = 108.375F;
			this.GroupHeader1.KeepTogether = true;
			this.GroupHeader1.Name = "GroupHeader1";
			this.GroupHeader1.RepeatEveryPage = true;
			// 
			// xrLine1
			// 
			this.xrLine1.LineWidth = 2;
			this.xrLine1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 32.99999F);
			this.xrLine1.Name = "xrLine1";
			this.xrLine1.SizeF = new System.Drawing.SizeF(650F, 23F);
			// 
			// xrTable1
			// 
			this.xrTable1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(214)))), ((int)(((byte)(211)))));
			this.xrTable1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(189)))), ((int)(((byte)(177)))), ((int)(((byte)(183)))));
			this.xrTable1.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
			this.xrTable1.BorderWidth = 2F;
			this.xrTable1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 65.37501F);
			this.xrTable1.Name = "xrTable1";
			this.xrTable1.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow1});
			this.xrTable1.SizeF = new System.Drawing.SizeF(541.046F, 43F);
			this.xrTable1.StylePriority.UseBackColor = false;
			this.xrTable1.StylePriority.UseBorderColor = false;
			this.xrTable1.StylePriority.UseBorders = false;
			this.xrTable1.StylePriority.UseBorderWidth = false;
			// 
			// xrTableRow1
			// 
			this.xrTableRow1.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell1,
            this.xrTableCell4,
            this.xrTableCell3,
            this.xrTableCell2});
			this.xrTableRow1.Name = "xrTableRow1";
			this.xrTableRow1.Weight = 1D;
			// 
			// xrTableCell1
			// 
			this.xrTableCell1.BackColor = System.Drawing.Color.SteelBlue;
			this.xrTableCell1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(189)))), ((int)(((byte)(177)))), ((int)(((byte)(183)))));
			this.xrTableCell1.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
			this.xrTableCell1.BorderWidth = 1F;
			this.xrTableCell1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.xrTableCell1.ForeColor = System.Drawing.Color.White;
			this.xrTableCell1.Name = "xrTableCell1";
			this.xrTableCell1.StylePriority.UseBackColor = false;
			this.xrTableCell1.StylePriority.UseBorderColor = false;
			this.xrTableCell1.StylePriority.UseBorders = false;
			this.xrTableCell1.StylePriority.UseBorderWidth = false;
			this.xrTableCell1.StylePriority.UseFont = false;
			this.xrTableCell1.StylePriority.UseForeColor = false;
			this.xrTableCell1.StylePriority.UseTextAlignment = false;
			this.xrTableCell1.Text = " Server Name";
			this.xrTableCell1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
			this.xrTableCell1.Weight = 0.93046339486633345D;
			// 
			// xrTableCell4
			// 
			this.xrTableCell4.BackColor = System.Drawing.Color.SteelBlue;
			this.xrTableCell4.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(189)))), ((int)(((byte)(177)))), ((int)(((byte)(183)))));
			this.xrTableCell4.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
			this.xrTableCell4.BorderWidth = 1F;
			this.xrTableCell4.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.xrTableCell4.ForeColor = System.Drawing.Color.White;
			this.xrTableCell4.Name = "xrTableCell4";
			this.xrTableCell4.StylePriority.UseBackColor = false;
			this.xrTableCell4.StylePriority.UseBorderColor = false;
			this.xrTableCell4.StylePriority.UseBorders = false;
			this.xrTableCell4.StylePriority.UseBorderWidth = false;
			this.xrTableCell4.StylePriority.UseFont = false;
			this.xrTableCell4.StylePriority.UseForeColor = false;
			this.xrTableCell4.StylePriority.UseTextAlignment = false;
			this.xrTableCell4.Text = "  Dead Threshold";
			this.xrTableCell4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
			this.xrTableCell4.Weight = 0.84619845259230908D;
			// 
			// xrTableCell3
			// 
			this.xrTableCell3.BackColor = System.Drawing.Color.SteelBlue;
			this.xrTableCell3.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(189)))), ((int)(((byte)(177)))), ((int)(((byte)(183)))));
			this.xrTableCell3.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
			this.xrTableCell3.BorderWidth = 1F;
			this.xrTableCell3.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.xrTableCell3.ForeColor = System.Drawing.Color.White;
			this.xrTableCell3.Name = "xrTableCell3";
			this.xrTableCell3.StylePriority.UseBackColor = false;
			this.xrTableCell3.StylePriority.UseBorderColor = false;
			this.xrTableCell3.StylePriority.UseBorders = false;
			this.xrTableCell3.StylePriority.UseBorderWidth = false;
			this.xrTableCell3.StylePriority.UseFont = false;
			this.xrTableCell3.StylePriority.UseForeColor = false;
			this.xrTableCell3.StylePriority.UseTextAlignment = false;
			this.xrTableCell3.Text = "  Pending Threshold";
			this.xrTableCell3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
			this.xrTableCell3.Weight = 0.91042796882519483D;
			this.xrTableCell3.WordWrap = false;
			// 
			// xrTableCell2
			// 
			this.xrTableCell2.BackColor = System.Drawing.Color.SteelBlue;
			this.xrTableCell2.BorderWidth = 1F;
			this.xrTableCell2.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
			this.xrTableCell2.ForeColor = System.Drawing.Color.White;
			this.xrTableCell2.Name = "xrTableCell2";
			this.xrTableCell2.StylePriority.UseBackColor = false;
			this.xrTableCell2.StylePriority.UseBorderWidth = false;
			this.xrTableCell2.StylePriority.UseFont = false;
			this.xrTableCell2.StylePriority.UseForeColor = false;
			this.xrTableCell2.StylePriority.UseTextAlignment = false;
			this.xrTableCell2.Text = " Held Mail Threshold";
			this.xrTableCell2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
			this.xrTableCell2.Weight = 0.90263866760707279D;
			this.xrTableCell2.WordWrap = false;
			// 
			// xrPageInfo1
			// 
			this.xrPageInfo1.Font = new System.Drawing.Font("Tahoma", 8.25F);
			this.xrPageInfo1.LocationFloat = new DevExpress.Utils.PointFloat(404.9998F, 16.99999F);
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
			this.xrLabel1.SizeF = new System.Drawing.SizeF(282.6666F, 33F);
			this.xrLabel1.StylePriority.UseTextAlignment = false;
			this.xrLabel1.Text = "Mail Thresholds";
			this.xrLabel1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
			// 
			// EvenStyle
			// 
			this.EvenStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(240)))));
			this.EvenStyle.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(207)))), ((int)(((byte)(189)))));
			this.EvenStyle.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
			this.EvenStyle.BorderWidth = 1F;
			this.EvenStyle.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.EvenStyle.Name = "EvenStyle";
			// 
			// OddStyle
			// 
			this.OddStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(232)))), ((int)(((byte)(220)))));
			this.OddStyle.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(207)))), ((int)(((byte)(189)))));
			this.OddStyle.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
			this.OddStyle.BorderWidth = 1F;
			this.OddStyle.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.OddStyle.Name = "OddStyle";
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
			this.xrSubreport1.ReportSource = new VSWebUI.ConfiguratorReports.CopyrightPortraitXtraRpt();
			this.xrSubreport1.SizeF = new System.Drawing.SizeF(100F, 23F);
			// 
			// MailThresholdXtraRpt
			// 
			this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.GroupHeader1,
            this.PageFooter});
			this.PageColor = System.Drawing.Color.Transparent;
			this.StyleSheet.AddRange(new DevExpress.XtraReports.UI.XRControlStyle[] {
            this.EvenStyle,
            this.OddStyle});
			this.Version = "14.2";
			this.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.MailThresholdXtraRpt_BeforePrint);
			((System.ComponentModel.ISupportInitialize)(this.xrTable2)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.xrTable1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.DetailBand Detail;
        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        private DevExpress.XtraReports.UI.GroupHeaderBand GroupHeader1;
        private DevExpress.XtraReports.UI.XRLabel xrLabel1;
        private DevExpress.XtraReports.UI.XRPageInfo xrPageInfo1;
        private DevExpress.XtraReports.UI.XRTable xrTable1;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow1;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell1;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell4;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell3;
        private DevExpress.XtraReports.UI.XRTable xrTable2;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow2;
        private DevExpress.XtraReports.UI.XRTableCell mtServerName;
        private DevExpress.XtraReports.UI.XRTableCell mtDeadThreshold;
        private DevExpress.XtraReports.UI.XRTableCell mtPendingThreshold;
		private DevExpress.XtraReports.UI.XRTableCell mtHeldMailThreshold;
		private DevExpress.XtraReports.UI.XRTableCell xrTableCell2;
        private DevExpress.XtraReports.UI.XRControlStyle EvenStyle;
        private DevExpress.XtraReports.UI.XRControlStyle OddStyle;
        private DevExpress.XtraReports.UI.XRLine xrLine1;
        private DevExpress.XtraReports.UI.PageFooterBand PageFooter;
        private DevExpress.XtraReports.UI.XRSubreport xrSubreport1;
    }
}
