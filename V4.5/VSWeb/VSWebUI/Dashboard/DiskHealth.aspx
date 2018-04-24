<%@ Page Title="VitalSigns Plus - Disk Health" Language="C#" MasterPageFile="~/DashboardSite.Master"
	AutoEventWireup="true" CodeBehind="DiskHealth.aspx.cs" Inherits="VSWebUI.Dashboard.DiskHealth" %>

<%@ MasterType VirtualPath="~/DashboardSite.Master" %>
<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.XtraCharts.v14.2.Web, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.XtraCharts.Web" TagPrefix="dxchartsui" %>
<%@ Register Assembly="DevExpress.XtraCharts.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.XtraCharts" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
	<script type="text/javascript">
		function Resized() {
			var callbackState = document.getElementById('ContentPlaceHolder1_callbackState').value;

			if (callbackState == 0)
				DoCallback();
		}

		function DoCallback() {
			document.getElementById('ContentPlaceHolder1_chartWidth').value = Math.round(document.body.offsetWidth / 2) - 50;
			//cSelectServerRoundPanel.style.width = document.getElementById('ContentPlaceHolder1__chartWidth').value + "px";
			//        FileOpensCumulativeWebChart.PerformCallback();
			//        FileOpensWebChart.PerformCallback();
			//        chttpSessionsWebChart.PerformCallback();
			//        DeviceTypeChart.PerformCallback();
			//        OSTypeChart.PerformCallback();

			cwebChartDiskHealth.PerformCallback();


		}

		function ResizeChart(s, e) {
			document.getElementById('ContentPlaceHolder1_callbackState').value = 0;
			s.GetMainElement().style.width = document.getElementById('ContentPlaceHolder1__chartWidth').value + "px";
			//cSelectServerRoundPanel.style.width = document.getElementById('ContentPlaceHolder1__chartWidth').value + "px";
		}

		function ResetCallbackState() {
			window.document.form1.elements['ContentPlaceHolder1_callbackState'].value = 1;
		}
		function InitPopupMenuHandler(s, e) {
			var gridCell = document.getElementById('gridCell');
			ASPxClientUtils.AttachEventToElement(gridCell, 'contextmenu', OnGridContextMenu);
			//        var imgButton = document.getElementById('popupButton');
			//        ASPxClientUtils.AttachEventToElement(imgButton, 'contextmenu', OnPreventContextMenu);
		}
		function OnGridContextMenu(evt) {
			var SortPopupMenu = popupmenu;
			SortPopupMenu.ShowAtPos(evt.clientX + ASPxClientUtils.GetDocumentScrollLeft(), evt.clientY + ASPxClientUtils.GetDocumentScrollTop());
			return OnPreventContextMenu(evt);
		}
		function OnPreventContextMenu(evt) {
			return ASPxClientUtils.PreventEventAndBubble(evt);
		}
		function findPos(obj, event, replacestring, replacewith,comments) {
		    var ispan = obj.id;
			ispan = ispan.replace(replacestring, replacewith);
			var ispanCtl = document.getElementById(ispan);

			var xOffset = Math.max(document.documentElement.scrollLeft, document.body.scrollLeft);
			var yOffset = Math.max(document.documentElement.scrollTop, document.body.scrollTop);

			ispanCtl.style.left = (event.clientX + xOffset + 25) + "px"; //obj.offsetParent.offsetLeft + "px";
			ispanCtl.style.top = (event.clientY + yOffset + -40) + "px";
		}

	</script>
	<style id="styles" type="text/css">
      
        a.tooltip2
        {text-decoration: none;
            outline: none;
            /* 4/21/2014 NS commented out */
            /* display: inline-block; */
            width: 100%;
            height: 100%;
            /* 4/21/2014 NS commented out */
            /*color: Black;*/
            top: -5px;
            
        }
        a.tooltip2 strong
        {
            line-height: 30px;
        }
        a.tooltip2:hover
        {
            text-decoration: none;
            /* 4/21/2014 NS commented out */
            /*color: Black;*/
        }
        a.tooltip2 .span2
        {
            text-decoration: none;
            z-index: 10;
            display: none;
            padding: 14px 20px;
            margin-top: -30px;
            float: left;
            width: 240px;
            line-height: 16px;
        }
        a.tooltip2:hover .span2
        {
            text-decoration: none;
            text-align: left;
            float: left;
            margin: 0px;
            left: -240px;
            display: inline-block;
            position: absolute;
            color: #111;
            white-space: normal;
            border: 1px solid #DCA;
            background: #fffAF0;
        }
        
        .callout2
        {
            z-index: 20;
            position: absolute;
            top: 30px;
            border: 0;
            left: -12px;
        }
        
        
        
        
        
       
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
	<input id="chartWidth" type="hidden" runat="server" value="400" />
	<input id="callbackState" type="hidden" runat="server" value="0" />
	<table width="100%">
		<tr>
			<td>
				<div class="header" id="servernamelbldisp" runat="server">
					Disk Health</div>
			</td>
			<td>
				&nbsp;
			</td>
			<td align="right">
				<table>
					<tr>
						<td>
						</td>
					</tr>
				</table>
			</td>
		</tr>
	</table>
	<table width="100%">
		<tr>
			<td colspan="2">
				<div class="info">
					Drives that are not color coded are not monitored by VitalSigns.
				</div>
				<table>
					<tr>
						<td align="left">
							<dx:ASPxButton runat="server" ID="btnCollapse" Text="Collapse All Rows" OnClick="btnCollapse_Click" CssClass="sysButton">
								<Image Url="~/images/icons/forbidden.png">
								</Image>
							</dx:ASPxButton>
						</td>
						<td align="left">
							<dx:ASPxButton runat="server" ID="btnExpand" Text="Expand All Rows" OnClick="btnExpand_Click"
								Theme="Office2010Blue" Visible="False">
								<Image Url="~/images/icons/add.png">
								</Image>
							</dx:ASPxButton>
						</td>
					</tr>
				</table>
			</td>
		</tr>
		<tr>
			<td valign="top">
				<dx:ASPxGridView ID="DiskHealthGrid" ClientInstanceName="grid" runat="server" AutoGenerateColumns="False"
					CssFilePath="~/App_Themes/Office2010Silver/{0}/styles.css" CssPostfix="Office2010Silver"
					Width="100%" KeyFieldName="ID" Cursor="pointer" OnHtmlDataCellPrepared="DiskHealthGrid_HtmlDataCellPrepared"
					OnCustomCallback="DiskHealthGrid_CustomCallback" OnHtmlRowCreated="DiskHealthGrid_HtmlRowCreated"
					EnableTheming="True" Theme="Office2003Blue" OnPageSizeChanged="DiskHealthGrid_PageSizeChanged">
					<%--<ClientSideEvents RowClick="function(s, e) { s.PerformCallback(e.visibleIndex); }" />--%>
					<ClientSideEvents FocusedRowChanged="function(s, e) { edit_panel.PerformCallback(); }">
					</ClientSideEvents>
					<SettingsBehavior AllowDragDrop="False" AllowFocusedRow="false" AllowSelectByRowClick="false"
						ColumnResizeMode="NextColumn" ProcessSelectionChangedOnServer="True" AutoExpandAllGroups="True">
					</SettingsBehavior>
					<Settings ShowGroupPanel="True"></Settings>
					<ClientSideEvents FocusedRowChanged="function(s, e) { edit_panel.PerformCallback(); }" />
					<Columns>
						<dx:GridViewDataTextColumn Caption="ID" VisibleIndex="12" FieldName="ID" Visible="False">
							<Settings AllowAutoFilter="False" />
						</dx:GridViewDataTextColumn>
						<dx:GridViewDataTextColumn Caption="Server Name" VisibleIndex="1" FieldName="ServerName" Width="160px">
							<Settings AllowAutoFilter="False" AllowDragDrop="True" AutoFilterCondition="Contains"
								AllowHeaderFilter="True" HeaderFilterMode="CheckedList" ShowFilterRowMenu="True"
								ShowInFilterControl="True" />
							<Settings AllowDragDrop="True" AllowAutoFilter="True" AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False">
							</Settings>
							<EditCellStyle CssClass="GridCss">
							</EditCellStyle>
							<EditFormCaptionStyle CssClass="GridCss">
							</EditFormCaptionStyle>
							<HeaderStyle CssClass="GridCssHeader">
								<Paddings Padding="5px" />
								<Paddings Padding="5px"></Paddings>
							</HeaderStyle>
							<CellStyle CssClass="GridCss">
							</CellStyle>
						</dx:GridViewDataTextColumn>
					<%--	<dx:GridViewDataTextColumn Caption="Disk Name" VisibleIndex="2" FieldName="DiskName"
							Width="100px">
							<Settings AllowDragDrop="True" AutoFilterCondition="Contains" AllowAutoFilter="False" />
							<Settings AllowDragDrop="True" AutoFilterCondition="Contains"></Settings>
							<EditCellStyle CssClass="GridCss">
							</EditCellStyle>
							<EditFormCaptionStyle CssClass="GridCss">
							</EditFormCaptionStyle>
							<HeaderStyle CssClass="GridCssHeader" />
							<CellStyle CssClass="GridCss">
							</CellStyle>
						</dx:GridViewDataTextColumn>--%>
							 <dx:GridViewDataTextColumn Caption="Disk Name" VisibleIndex="2" 
                                                    Width="100px">
													<DataItemTemplate>
                                                <a class='tooltip2'>
													<img id="Imgforinfo" runat="server" src="~/images/icons/information.png" />
                                            <asp:Label ID="hfNameLabel" Text='<%# Eval("DiskName") %>' runat="server" onmousemove="findPos(this,event,'hfNameLabel', 'detailsspan','lblReasons');" />
											   
                                            <asp:Label ID="detailsspan" class="span2" runat="server">
                                                <img class='callout2' src='../images/callout.gif' />
                                                <font style="color: Blue; font-size: 16px;">&nbsp<b><asp:Label ID="NameLabel" Text='<%# Eval("DiskName") %>' runat="server"></asp:Label>: Comments</b></font><br>&nbsp
												<asp:Label ID="lblReasons" Text='<%# Eval("DiskInfo") %>' runat="server" ></asp:Label>
                                                </asp:Label>
												<asp:Label  ID="test" runat="server" Text="<%# GetDiskInfo(Container) %>" Visible="false">
												</asp:Label>
                                                </a>
                                               
                                        </DataItemTemplate>
 
                                                    <Settings AllowDragDrop="True" AutoFilterCondition="Contains" 
														AllowAutoFilter="False" />
<Settings AllowDragDrop="True" AutoFilterCondition="Contains"></Settings>

                                                    <EditCellStyle CssClass="GridCss"></EditCellStyle>
                                                    <EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle>
                                                    <HeaderStyle CssClass="GridCssHeader" /><CellStyle CssClass="GridCss"></CellStyle>
                                                </dx:GridViewDataTextColumn>
						<dx:GridViewDataTextColumn Caption="Percent Free" VisibleIndex="6" FieldName="PercentFree">
							<Settings AllowAutoFilter="False" AllowDragDrop="True" />
							<PropertiesTextEdit DisplayFormatString="{0:P}">
							</PropertiesTextEdit>
							<Settings AllowDragDrop="True" AllowAutoFilter="False"></Settings>
							<EditCellStyle CssClass="GridCss">
							</EditCellStyle>
							<EditFormCaptionStyle CssClass="GridCss">
							</EditFormCaptionStyle>
							<HeaderStyle CssClass="GridCssHeader2" />
							<CellStyle CssClass="GridCss2">
							</CellStyle>
							<PropertiesTextEdit DisplayFormatString="{0:P}" />
						</dx:GridViewDataTextColumn>
						<dx:GridViewDataTextColumn Caption="Disk Free (GB)" FieldName="DiskFree" Visible="True"
							VisibleIndex="5">
							<Settings AllowDragDrop="True" AutoFilterCondition="Contains" AllowAutoFilter="False" />
							<Settings AllowDragDrop="True" AutoFilterCondition="Contains"></Settings>
							<EditCellStyle CssClass="GridCss">
							</EditCellStyle>
							<EditFormCaptionStyle CssClass="GridCss">
							</EditFormCaptionStyle>
							<HeaderStyle CssClass="GridCssHeader2" />
							<CellStyle CssClass="GridCss2" HorizontalAlign="Center">
							</CellStyle>
							<PropertiesTextEdit DisplayFormatString="0.00">
							</PropertiesTextEdit>
						</dx:GridViewDataTextColumn>
						<dx:GridViewDataTextColumn Caption="Disk Size (GB)" FieldName="DiskSize" Visible="True"
							VisibleIndex="4">
							<PropertiesTextEdit DisplayFormatString="0.00">
							</PropertiesTextEdit>
							<Settings AllowAutoFilter="False" AllowDragDrop="True" AutoFilterCondition="Contains" />
							<Settings AllowDragDrop="True" AllowAutoFilter="False" AutoFilterCondition="Contains">
							</Settings>
							<EditCellStyle CssClass="GridCss">
							</EditCellStyle>
							<EditFormCaptionStyle CssClass="GridCss">
							</EditFormCaptionStyle>
							<HeaderStyle CssClass="GridCssHeader2" />
							<CellStyle CssClass="GridCss2">
							</CellStyle>
						</dx:GridViewDataTextColumn>
						<dx:GridViewDataTextColumn Caption="Disk Utilization" VisibleIndex="9" FieldName="PercentUtilization">
							<Settings AllowAutoFilter="False" AllowDragDrop="True" AutoFilterCondition="Contains" />
							<Settings AllowDragDrop="True" AllowAutoFilter="False" AutoFilterCondition="Contains">
							</Settings>
							<EditCellStyle CssClass="GridCss">
							</EditCellStyle>
							<EditFormCaptionStyle CssClass="GridCss">
							</EditFormCaptionStyle>
							<HeaderStyle CssClass="GridCssHeader2" />
							<CellStyle CssClass="GridCss2">
							</CellStyle>
						</dx:GridViewDataTextColumn>
						<dx:GridViewDataTextColumn Caption="Average Queue Length" FieldName="AverageQueueLength"
							VisibleIndex="10">
							<Settings AllowDragDrop="True" AutoFilterCondition="Contains" AllowAutoFilter="False" />
							<Settings AllowDragDrop="True" AutoFilterCondition="Contains"></Settings>
							<EditCellStyle CssClass="GridCss">
							</EditCellStyle>
							<EditFormCaptionStyle CssClass="GridCss">
							</EditFormCaptionStyle>
							<HeaderStyle CssClass="GridCssHeader2" Wrap="True" />
							<CellStyle CssClass="GridCss2">
							</CellStyle>
						</dx:GridViewDataTextColumn>
						<dx:GridViewDataTextColumn Caption="Updated" FieldName="Updated" VisibleIndex="11"
							Width="150px">
							<Settings AutoFilterCondition="Contains" AllowAutoFilter="False" />
							<EditCellStyle CssClass="GridCss">
							</EditCellStyle>
							<EditFormCaptionStyle CssClass="GridCss">
							</EditFormCaptionStyle>
							<HeaderStyle CssClass="GridCssHeader1" />
							<CellStyle CssClass="GridCss1">
							</CellStyle>
						</dx:GridViewDataTextColumn>
						<dx:GridViewDataTextColumn Caption="Threshold" FieldName="Threshold" VisibleIndex="7">
							<PropertiesTextEdit />
							<Settings AllowAutoFilter="False" AutoFilterCondition="Contains" />
							<EditCellStyle CssClass="GridCss">
							</EditCellStyle>
							<EditFormCaptionStyle CssClass="GridCss">
							</EditFormCaptionStyle>
							<HeaderStyle CssClass="GridCssHeader2" />
							<CellStyle CssClass="GridCss2">
							</CellStyle>
						</dx:GridViewDataTextColumn>
						<dx:GridViewDataTextColumn Caption="Unit" FieldName="ThresholdType" VisibleIndex="8">
							<PropertiesTextEdit />
							<Settings AllowAutoFilter="False" AutoFilterCondition="Contains" />
							<EditCellStyle CssClass="GridCss">
							</EditCellStyle>
							<EditFormCaptionStyle CssClass="GridCss">
							</EditFormCaptionStyle>
							<HeaderStyle CssClass="GridCssHeader" />
							<CellStyle CssClass="GridCss">
							</CellStyle>
						</dx:GridViewDataTextColumn>
						<dx:GridViewDataTextColumn VisibleIndex="3" Width="310px" Caption="Disk Used/Disk Free">
							<DataItemTemplate>
								<dxchartsui:WebChartControl ID="DiskWebChart" runat="server" OnLoad="DiskWebChart_Load"
									CrosshairEnabled="True" Height="25px" PaletteName="Palette 1" Width="300px" BackColor="Transparent">
									<borderoptions visible="False" />
									<borderoptions visible="False"></borderoptions>
									<diagramserializable>
                                            <cc1:XYDiagram Rotated="True">
                                                <axisx reverse="True" visibleinpanesserializable="-1" visible="False">
                                                    <tickmarks minorvisible="False" visible="False" />
<Tickmarks Visible="False" MinorVisible="False"></Tickmarks>

                                                    <label visible="False">
                                                    </label>
                                                </axisx>
                                                <axisy title-text="Total Disk Size (GB)" title-visible="False" 
                                                    visibleinpanesserializable="-1" visible="False">
                                                    <tickmarks minorvisible="False" visible="False" />
<Tickmarks Visible="False" MinorVisible="False"></Tickmarks>

                                                    <label visible="False">
                                                    </label>
                                                    <gridlines visible="False">
                                                    </gridlines>
                                                </axisy>
                                                <margins bottom="0" left="0" right="0" top="0" />

<Margins Left="0" Top="0" Right="0" Bottom="0"></Margins>
                                                <defaultpane backcolor="Transparent" bordervisible="False">
                                                </defaultpane>
                                            </cc1:XYDiagram>
                                        </diagramserializable>
									<legend visible="False"></legend>
									<seriesserializable>
                                            <cc1:Series ArgumentScaleType="Qualitative" LabelsVisibility="True" 
                                                Name="Disk Used">
                                                <viewserializable>
                                                    <cc1:StackedBarSeriesView>
                                                    </cc1:StackedBarSeriesView>
                                                </viewserializable>
                                            </cc1:Series>
                                            <cc1:Series ArgumentScaleType="Qualitative" LabelsVisibility="True" 
                                                Name="Disk Free">
                                                <viewserializable>
                                                    <cc1:StackedBarSeriesView>
                                                    </cc1:StackedBarSeriesView>
                                                </viewserializable>
                                            </cc1:Series>
                                        </seriesserializable>
									<seriestemplate>
                                            <viewserializable>
                                                <cc1:StackedBarSeriesView>
                                                </cc1:StackedBarSeriesView>
                                            </viewserializable>
                                        </seriestemplate>
									<palettewrappers>
                                            <dxchartsui:PaletteWrapper Name="Palette 1" ScaleMode="Repeat">
                                                <palette>
                                                    <cc1:PaletteEntry Color="Red" Color2="Red" />
                                                    <cc1:PaletteEntry Color="Green" Color2="Green" />
                                                </palette>
                                            </dxchartsui:PaletteWrapper>
                                        </palettewrappers>
								</dxchartsui:WebChartControl>
							</DataItemTemplate>
							<HeaderStyle CssClass="GridCssHeader" />
						</dx:GridViewDataTextColumn>
					</Columns>
					<SettingsBehavior AllowDragDrop="False" AutoExpandAllGroups="True" ColumnResizeMode="NextColumn" />
					<SettingsPager PageSize="10" SEOFriendly="Enabled" >
						<PageSizeItemSettings Visible="true"  Items="10, 20, 50, 100, 200, 500"/>
						<PageSizeItemSettings Visible="True">
						</PageSizeItemSettings>
					</SettingsPager>
					<Settings ShowGroupPanel="True" ShowFilterRow="True" />
					<Styles CssFilePath="~/App_Themes/Office2010Silver/{0}/styles.css" CssPostfix="Office2010Silver">
						<Header ImageSpacing="5px" SortingImageSpacing="5px">
						</Header>
						<GroupRow Font-Bold="True" Font-Italic="False">
						</GroupRow>
						<GroupFooter Font-Bold="True">
						</GroupFooter>
						<GroupPanel Font-Bold="False">
						</GroupPanel>
						<LoadingPanel ImageSpacing="5px">
						</LoadingPanel>
						<AlternatingRow CssClass="GridAltRow" Enabled="True">
						</AlternatingRow>
					</Styles>
					<StylesEditors ButtonEditCellSpacing="0">
						<ProgressBar Height="21px">
						</ProgressBar>
					</StylesEditors>
					<Templates>
						<GroupRowContent>
							<%# Container.GroupText%>
						</GroupRowContent>
					</Templates>
				</dx:ASPxGridView>
			</td>
		</tr>
		<tr>
			<td>
				<dx:ASPxCallbackPanel ID="ASPxCallbackPanel1" ClientInstanceName="edit_panel" OnCallback="ASPxCallbackPanel1_Callback"
					runat="server">
					<PanelCollection>
						<dx:PanelContent ID="PanelContent2" runat="server">
							<table>
								<tr>
									<td>
										<dx:ASPxLabel ID="lblServerName" runat="server" Visible="False">
										</dx:ASPxLabel>
									</td>
								</tr>
								<tr>
									<td align="left" colspan="2">
										<dxchartsui:WebChartControl ID="webChartDiskHealth" runat="server" Height="300px"
											Width="500px" EnableDefaultAppearance="False" PaletteName="Palette 1" OnCustomCallback="webChartDiskHealth_CustomCallback"
											ClientInstanceName="cwebChartDiskHealth" OnCustomDrawSeriesPoint="webChartDiskHealth_CustomDrawSeriesPoint"
											Visible="False">
											<fillstyle>
                                                                        <optionsserializable><cc1:SolidFillOptions /></optionsserializable>
                                                                    </fillstyle>
											<diagramserializable>
                                                                        <cc1:XYDiagram Rotated="True">
                                                                            <axisx reverse="True" visibleinpanesserializable="-1">
                                                                                <label>
                                                                                    <resolveoverlappingoptions allowrotate="False" />
<ResolveOverlappingOptions AllowRotate="False"></ResolveOverlappingOptions>
                                                                                </label>
                                                                            </axisx>
                                                                            <axisy title-text="Total Disk Size (GB)" title-visible="True" 
                                                                                visibleinpanesserializable="-1">
                                                                                <label>
                                                                                    <resolveoverlappingoptions allowrotate="False" allowstagger="False" />
                                                                                    <numericoptions format="Number" precision="0" />
<ResolveOverlappingOptions AllowStagger="False" AllowRotate="False"></ResolveOverlappingOptions>

<NumericOptions Format="Number" Precision="0"></NumericOptions>
                                                                                </label>
                                                                                <visualrange autosidemargins="True" />
                                                                                <wholerange autosidemargins="True" />

<VisualRange AutoSideMargins="True"></VisualRange>

<WholeRange AutoSideMargins="True"></WholeRange>
                                                                            </axisy>
                                                                        </cc1:XYDiagram>
                                                                    </diagramserializable>
											<legend visible="False"></legend>
											<seriesserializable>
                                                                        <cc1:Series ArgumentScaleType="Qualitative" LabelsVisibility="True" 
                                                                            Name="Disk Used">
                                                                            <viewserializable>
                                                                                <cc1:StackedBarSeriesView>
                                                                                </cc1:StackedBarSeriesView>
                                                                            </viewserializable>
                                                                            <labelserializable>
                                                                                <cc1:StackedBarSeriesLabel ResolveOverlappingMode="HideOverlapped">
                                                                                </cc1:StackedBarSeriesLabel>
                                                                            </labelserializable>
                                                                        </cc1:Series>
                                                                        <cc1:Series ArgumentScaleType="Qualitative" LabelsVisibility="True" 
                                                                            Name="Disk Free">
                                                                            <viewserializable>
                                                                                <cc1:StackedBarSeriesView>
                                                                                </cc1:StackedBarSeriesView>
                                                                            </viewserializable>
                                                                            <labelserializable>
                                                                                <cc1:StackedBarSeriesLabel ResolveOverlappingMode="HideOverlapped">
                                                                                </cc1:StackedBarSeriesLabel>
                                                                            </labelserializable>
                                                                        </cc1:Series>
                                                                    </seriesserializable>
											<seriestemplate argumentscaletype="Qualitative">
                                                                        <viewserializable>
                                                                            <cc1:StackedBarSeriesView>
                                                                            </cc1:StackedBarSeriesView>
                                                                        </viewserializable>
                                                                        <labelserializable>
                                                                            <cc1:StackedBarSeriesLabel>
                                                                                <pointoptionsserializable>
                                                                                    <cc1:PointOptions>
                                                                                        <valuenumericoptions format="Percent" />
<ValueNumericOptions Format="Percent"></ValueNumericOptions>
                                                                                    </cc1:PointOptions>
                                                                                </pointoptionsserializable>
                                                                            </cc1:StackedBarSeriesLabel>
                                                                        </labelserializable>
                                                                        <legendpointoptionsserializable>
                                                                            <cc1:PointOptions>
                                                                                <valuenumericoptions format="Percent" />
<ValueNumericOptions Format="Percent"></ValueNumericOptions>
                                                                            </cc1:PointOptions>
                                                                        </legendpointoptionsserializable>
                                                                    </seriestemplate>
											<palettewrappers>
                                                                        <dxchartsui:PaletteWrapper Name="Palette 1" ScaleMode="Repeat">
                                                                            <palette><cc1:PaletteEntry Color="Red" Color2="Red" />
                                                                                <cc1:PaletteEntry Color="Green" Color2="Green" /></palette>
                                                                        </dxchartsui:PaletteWrapper>
                                                                    </palettewrappers>
											<crosshairoptions><CommonLabelPositionSerializable><cc1:CrosshairMousePosition></cc1:CrosshairMousePosition></CommonLabelPositionSerializable>
                                                                    </crosshairoptions>
											<tooltipoptions><ToolTipPositionSerializable><cc1:ToolTipMousePosition></cc1:ToolTipMousePosition></ToolTipPositionSerializable>
                                                                    </tooltipoptions>
										</dxchartsui:WebChartControl>
									</td>
								</tr>
							</table>
						</dx:PanelContent>
					</PanelCollection>
				</dx:ASPxCallbackPanel>
			</td>
		</tr>
	</table>
</asp:Content>
