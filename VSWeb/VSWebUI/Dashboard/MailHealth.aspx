<%@ Page Title="VitalSigns Plus - Mail Health" Language="C#" MasterPageFile="~/DashboardSite.Master"
	AutoEventWireup="true" CodeBehind="MailHealth.aspx.cs" Inherits="VSWebUI.MailHealth" %>

<%@ MasterType VirtualPath="~/DashboardSite.Master" %>
<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.XtraCharts.v14.2.Web, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.XtraCharts.Web" TagPrefix="dxchartsui" %>
<%@ Register Assembly="DevExpress.XtraCharts.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.XtraCharts" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc11" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
	<style type="text/css">
		.style1
		{
			height: 18px;
		}
	</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
	<script type="text/javascript">

		function grid_SelectionChanged(s, e) {
			s.GetSelectedFieldValues("Name", GetSelectedFieldValuesCallback);
		}
		function GetSelectedFieldValuesCallback(values) {
			//1/9/2013 NS commented out - was throwing an exception in IE selList is undefined
			/*
			selList.BeginUpdate();
			try {
			selList.ClearItems();
			for (var i = 0; i < values.length; i++) {
			selList.AddItem(values[i]);
			}
			} finally {
			selList.EndUpdate();
			}
			document.getElementById("selCount").innerHTML = grid.GetSelectedRowCount();
			*/
		}
		function Resized() {
			var callbackState = document.getElementById('ContentPlaceHolder1_callbackState').value;

			if (callbackState == 0)
				DoCallback();
		}

		function DoCallback() {
			document.getElementById('ContentPlaceHolder1_chartWidth').value = Math.round(document.body.offsetWidth / 2) - 50;
			var chartwidth = document.getElementById('ContentPlaceHolder1_chartWidth').value;
			cMailTraficWebChartControl.PerformCallback();
			cMailDeliveredWebChartControl.PerformCallback();
			cMailTranfferedWebChartControl.PerformCallback();
			cMailRoutedWebChartControl.PerformCallback();
			cQueueWebChart.PerformCallback();
			cDeliveryRateWebChart.PerformCallback();
			cMailSizeWebChart.PerformCallback();
			cMailCountWebChart.PerformCallback();
		}  
	</script>
	<input id="chartWidth" type="hidden" runat="server" value="200" />
	<input id="callbackState" type="hidden" runat="server" value="0" />
	<table width="100%">
		<tr>
			<td>
				<div class="header" id="servernamelbldisp" runat="server">
					Mail Health</div>
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
			<td>
				<div id="mailservicesMainDiv" runat="server">
					<div class="subheader" id="mailServicesDiv" runat="server">
						Mail Services
					</div>
					<table width="100%">
						<tr>
							<td colspan="2">
								<dx:ASPxGridView ID="MailGridView" ClientInstanceName="grid" runat="server" AutoGenerateColumns="False"
									EnableTheming="True" Theme="Office2003Blue" OnHtmlDataCellPrepared="MailGridView_HtmlDataCellPrepared"
									Width="100%" KeyFieldName="TypeANDName" OnHtmlRowCreated="MailGridView_HtmlRowCreated"
									OnPageSizeChanged="MailGridView_PageSizeChanged">
									<ClientSideEvents FocusedRowChanged="function(s, e) { edit_panel.PerformCallback(); }" />
									<ClientSideEvents SelectionChanged="grid_SelectionChanged" />
									<Columns>
										<dx:GridViewCommandColumn Visible="False" VisibleIndex="0">
											<ClearFilterButton Visible="True">
											</ClearFilterButton>
										</dx:GridViewCommandColumn>
										<dx:GridViewDataTextColumn Caption="Name" FieldName="Name" FixedStyle="Left" VisibleIndex="1">
											<Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
											<EditCellStyle CssClass="GridCss">
											</EditCellStyle>
											<EditFormCaptionStyle CssClass="GridCss">
											</EditFormCaptionStyle>
											<HeaderStyle CssClass="GridCssHeader" />
											<CellStyle CssClass="GridCss">
											</CellStyle>
										</dx:GridViewDataTextColumn>
										<dx:GridViewDataTextColumn Caption="Status" FieldName="Status" VisibleIndex="2" Width="150px">
											<EditCellStyle CssClass="GridCss">
											</EditCellStyle>
											<EditFormCaptionStyle CssClass="GridCss">
											</EditFormCaptionStyle>
											<HeaderStyle CssClass="GridCssHeader1">
												<Paddings Padding="5px" />
											</HeaderStyle>
											<CellStyle CssClass="GridCss1" HorizontalAlign="Center">
											</CellStyle>
										</dx:GridViewDataTextColumn>
										<dx:GridViewDataTextColumn Caption="Location" FieldName="Location" VisibleIndex="4">
											<EditCellStyle CssClass="GridCss">
											</EditCellStyle>
											<EditFormCaptionStyle CssClass="GridCss">
											</EditFormCaptionStyle>
											<HeaderStyle CssClass="GridCssHeader" />
											<CellStyle CssClass="GridCss">
											</CellStyle>
										</dx:GridViewDataTextColumn>
										<dx:GridViewDataTextColumn Caption="Protocol" VisibleIndex="5" FieldName="Category">
											<EditCellStyle CssClass="GridCss">
											</EditCellStyle>
											<EditFormCaptionStyle CssClass="GridCss">
											</EditFormCaptionStyle>
											<HeaderStyle CssClass="GridCssHeader" />
											<CellStyle CssClass="GridCss">
											</CellStyle>
										</dx:GridViewDataTextColumn>
										<dx:GridViewDataTextColumn Caption="TypeANDName" FieldName="TypeANDName" Visible="False"
											VisibleIndex="6">
										</dx:GridViewDataTextColumn>
										<dx:GridViewDataTextColumn Caption="Details" FieldName="Details" VisibleIndex="3"
											Width="400px">
											<HeaderStyle CssClass="GridCssHeader" />
											<CellStyle CssClass="GridCss">
											</CellStyle>
										</dx:GridViewDataTextColumn>
									</Columns>
									<SettingsBehavior AllowFocusedRow="True" AutoExpandAllGroups="True" AllowSelectByRowClick="True"
										ColumnResizeMode="NextColumn" />
									<SettingsPager PageSize="10">
										<PageSizeItemSettings Visible="True">
										</PageSizeItemSettings>
									</SettingsPager>
									<Settings ShowGroupPanel="True" ShowFilterRow="True" />
									<Styles>
										<AlternatingRow CssClass="GridAltRow">
										</AlternatingRow>
									</Styles>
								</dx:ASPxGridView>
							</td>
						</tr>
						<tr>
							<td>
							</td>
						</tr>
					</table>
				</div>
			</td>
		</tr>
		<tr>
			<td>
				<dx:ASPxPageControl ID="ASPxPageControl1" runat="server" Theme="Glass" Width="100%"
					ActiveTabIndex="0" EnableHierarchyRecreation="False">
					<TabPages>
						<dx:TabPage Text="Domino">
							<ContentCollection>
								<dx:ContentControl ID="ContentControl1" runat="server">
									<table width="100%">
										<tr>
											<td>
												<div id="notesmailprobeMainDiv" runat="server">
													<div class="subheader" id="notesmailprobeDiv" runat="server">
														NotesMail Probe</div>
													<table width="100%">
														<tr>
															<td>
																<dx:ASPxGridView ID="NotesMailProbeGridView" runat="server" AutoGenerateColumns="False"
																	EnableTheming="True" Theme="Office2003Blue" OnHtmlDataCellPrepared="NotesMailProbeGridView_HtmlDataCellPrepared"
																	Width="100%" KeyFieldName="TypeANDName" OnPageSizeChanged="NotesMailProbeGridView_PageSizeChanged"
																	OnHtmlRowCreated="NotesMailProbeGridView_HtmlRowCreated">
																	<SettingsBehavior AllowSelectByRowClick="True" ColumnResizeMode="NextColumn" />
																	<SettingsPager PageSize="10">
																		<PageSizeItemSettings Visible="True">
																		</PageSizeItemSettings>
																	</SettingsPager>
																	<Columns>
																		<dx:GridViewCommandColumn ShowInCustomizationForm="True" Visible="False" VisibleIndex="0">
																			<ClearFilterButton Visible="True">
																			</ClearFilterButton>
																		</dx:GridViewCommandColumn>
																		<dx:GridViewDataTextColumn Caption="Name" FieldName="Name" ShowInCustomizationForm="True"
																			VisibleIndex="1">
																			<Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
																			<EditCellStyle CssClass="GridCss">
																			</EditCellStyle>
																			<EditFormCaptionStyle CssClass="GridCss">
																			</EditFormCaptionStyle>
																			<HeaderStyle CssClass="GridCssHeader">
																				<Paddings Padding="5px" />
																			</HeaderStyle>
																			<CellStyle CssClass="GridCss">
																			</CellStyle>
																		</dx:GridViewDataTextColumn>
																		<dx:GridViewDataTextColumn Caption="Status" FieldName="Status" ShowInCustomizationForm="True"
																			VisibleIndex="2">
																			<Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
																			<EditCellStyle CssClass="GridCss">
																			</EditCellStyle>
																			<EditFormCaptionStyle CssClass="GridCss">
																			</EditFormCaptionStyle>
																			<HeaderStyle CssClass="GridCssHeader" />
																			<CellStyle CssClass="GridCss1" HorizontalAlign="Center">
																			</CellStyle>
																		</dx:GridViewDataTextColumn>
																		<dx:GridViewDataTextColumn Caption="Details" FieldName="Details" ShowInCustomizationForm="True"
																			VisibleIndex="3">
																			<Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
																			<EditCellStyle CssClass="GridCss">
																			</EditCellStyle>
																			<EditFormCaptionStyle CssClass="GridCss">
																			</EditFormCaptionStyle>
																			<HeaderStyle CssClass="GridCssHeader" />
																			<CellStyle CssClass="GridCss">
																			</CellStyle>
																		</dx:GridViewDataTextColumn>
																		<dx:GridViewDataTextColumn Caption="Last Update" ShowInCustomizationForm="True" VisibleIndex="4"
																			FieldName="LastUpdate">
																			<Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
																			<EditCellStyle CssClass="GridCss">
																			</EditCellStyle>
																			<EditFormCaptionStyle CssClass="GridCss">
																			</EditFormCaptionStyle>
																			<HeaderStyle CssClass="GridCssHeader" />
																			<CellStyle CssClass="GridCss">
																			</CellStyle>
																		</dx:GridViewDataTextColumn>
																		<dx:GridViewDataTextColumn Caption="TypeANDName" FieldName="TypeANDName" ShowInCustomizationForm="True"
																			Visible="False" VisibleIndex="5">
																			<Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
																		</dx:GridViewDataTextColumn>
																	</Columns>
																	<Settings ShowGroupPanel="True" ShowFilterRow="True" />
																	<Styles>
																		<AlternatingRow CssClass="GridAltRow" Enabled="True">
																		</AlternatingRow>
																	</Styles>
																</dx:ASPxGridView>
															</td>
														</tr>
													</table>
												</div>
											</td>
										</tr>
										<tr>
											<td align="left">
												<table width="100%">
													<tr>
														<td colspan="2">
															<table>
																<tr>
																	<td class="style1">
																		<dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="Select a server:" CssClass="lblsmallFont">
																		</dx:ASPxLabel>
																	</td>
																	<td>
																		<dx:ASPxComboBox ID="ServerListComboBox" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ServerListComboBox_SelectedIndexChanged">
																		</dx:ASPxComboBox>
																	</td>
																</tr>
															</table>
														</td>
													</tr>
													<tr valign="top">
														<td>
															<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
																<ContentTemplate>
																	<dxchartsui:WebChartControl ID="MailRoutedWebChartControl" ClientInstanceName="cMailRoutedWebChartControl"
																		runat="server" Height="300px" Width="500px" CrosshairEnabled="True" OnCustomCallback="MailRoutedWebChartControl_CustomCallback">
																		<diagramserializable>
                                                                                            <cc1:XYDiagram>
                                                                                                <axisx datetimegridalignment="Hour" datetimemeasureunit="Hour" 
                                                                                                    visibleinpanesserializable="-1"  title-text="Time" title-visible="True" >
                                                                                                    <label>
                                                                                                    <resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" />
                                                                                                    <resolveoverlappingoptions allowrotate="False" />
                                                                                                    <resolveoverlappingoptions allowrotate="False"></resolveoverlappingoptions>
                                                                                                    <datetimeoptions autoformat="False" format="ShortTime" />
                                                                                                    <datetimeoptions autoformat="False" format="ShortTime" /></label>
                                                                                                    <range sidemarginsenabled="True" />
                                                                                                    <datetimeoptions format="ShortTime" />
                                                                                                <range sidemarginsenabled="True" /><datetimeoptions format="ShortTime" />
                                                                                                    <visualrange autosidemargins="True" />
                                                                                                    <wholerange autosidemargins="True" />
                                                                                                    <range sidemarginsenabled="True" /><datetimeoptions format="ShortTime" /><range sidemarginsenabled="True" /><datetimeoptions format="ShortTime" /><visualrange autosidemargins="True" /><wholerange autosidemargins="True" /><datetimescaleoptions autogrid="False" gridalignment="Hour" measureunit="Hour">
                                                                                                    </datetimescaleoptions>
                                                                                                </axisx>
                                                                                                <axisy visibleinpanesserializable="-1"  title-text="Count" title-visible="True">
                                                                                                    <range sidemarginsenabled="True" />
                                                                                                <range sidemarginsenabled="True" />
                                                                                                    <visualrange autosidemargins="True" />
                                                                                                    <wholerange autosidemargins="True" />
                                                                                                <range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><visualrange autosidemargins="True" /><wholerange autosidemargins="True" /></axisy>
                                                                                            </cc1:XYDiagram>
                                                                                        </diagramserializable>
																		<fillstyle>
                                                                    <optionsserializable>
                                                                        <cc1:SolidFillOptions />
                                                                    </optionsserializable>
                                                                </fillstyle>
																		<seriesserializable>
                                                                                            <cc1:Series ArgumentScaleType="DateTime" Name="Series 1">
                                                                                                <viewserializable>
                                                                                                    <cc1:SideBySideBarSeriesView>
                                                                                                    </cc1:SideBySideBarSeriesView>
                                                                                                </viewserializable>
                                                                                                <labelserializable>
                                                                                                    <cc1:SideBySideBarSeriesLabel LineVisible="True">
                                                                                                        <fillstyle>
                                                                                                            <optionsserializable>
                                                                                                                <cc1:SolidFillOptions />
                                                                                                            </optionsserializable>
                                                                                                        </fillstyle>
                                                                                                        <pointoptionsserializable>
                                                                                                            <cc1:PointOptions>
                                                                                                            </cc1:PointOptions>
                                                                                                        </pointoptionsserializable>
                                                                                                    </cc1:SideBySideBarSeriesLabel>
                                                                                                </labelserializable>
                                                                                                <legendpointoptionsserializable>
                                                                                                    <cc1:PointOptions>
                                                                                                    </cc1:PointOptions>
                                                                                                </legendpointoptionsserializable>
                                                                                            </cc1:Series>
                                                                                            <cc1:Series ArgumentScaleType="DateTime" Name="Series 2">
                                                                                                <viewserializable>
                                                                                                    <cc1:SideBySideBarSeriesView>
                                                                                                    </cc1:SideBySideBarSeriesView>
                                                                                                </viewserializable>
                                                                                                <labelserializable>
                                                                                                    <cc1:SideBySideBarSeriesLabel LineVisible="True">
                                                                                                        <fillstyle>
                                                                                                            <optionsserializable>
                                                                                                                <cc1:SolidFillOptions />
                                                                                                            </optionsserializable>
                                                                                                        </fillstyle>
                                                                                                        <pointoptionsserializable>
                                                                                                            <cc1:PointOptions>
                                                                                                            </cc1:PointOptions>
                                                                                                        </pointoptionsserializable>
                                                                                                    </cc1:SideBySideBarSeriesLabel>
                                                                                                </labelserializable>
                                                                                                <legendpointoptionsserializable>
                                                                                                    <cc1:PointOptions>
                                                                                                    </cc1:PointOptions>
                                                                                                </legendpointoptionsserializable>
                                                                                            </cc1:Series>
                                                                                        </seriesserializable>
																		<seriestemplate>
                                                                    <viewserializable>
                                                                        <cc1:SideBySideBarSeriesView>
                                                                        </cc1:SideBySideBarSeriesView>
                                                                    </viewserializable>
                                                                    <labelserializable>
                                                                        <cc1:SideBySideBarSeriesLabel LineVisible="True">
                                                                            <fillstyle>
                                                                                <optionsserializable>
                                                                                    <cc1:SolidFillOptions />
                                                                                </optionsserializable>
                                                                            </fillstyle>
                                                                            <pointoptionsserializable>
                                                                                <cc1:PointOptions>
                                                                                </cc1:PointOptions>
                                                                            </pointoptionsserializable>
                                                                        </cc1:SideBySideBarSeriesLabel>
                                                                    </labelserializable>
                                                                    <legendpointoptionsserializable>
                                                                        <cc1:PointOptions>
                                                                        </cc1:PointOptions>
                                                                    </legendpointoptionsserializable>
                                                                </seriestemplate>
																		<crosshairoptions>
                                                                    <commonlabelpositionserializable>
                                                                        <cc1:CrosshairMousePosition />
                                                                    </commonlabelpositionserializable>
                                                                </crosshairoptions>
																		<tooltipoptions>
                                                                    <tooltippositionserializable>
                                                                        <cc1:ToolTipMousePosition />
                                                                    </tooltippositionserializable>
                                                                </tooltipoptions>
																		<titles>
                                                                                            <cc1:ChartTitle Text="Mail Routed" />
                                                                                        </titles>
																	</dxchartsui:WebChartControl>
																</ContentTemplate>
																<Triggers>
																	<asp:AsyncPostBackTrigger ControlID="ServerListComboBox" />
																</Triggers>
															</asp:UpdatePanel>
														</td>
														<td>
															<asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
																<ContentTemplate>
																	<dxchartsui:WebChartControl ID="MailDeliveredWebChartControl" ClientInstanceName="cMailDeliveredWebChartControl"
																		runat="server" Height="300px" Width="500px" CrosshairEnabled="True" OnCustomCallback="MailDeliveredWebChartControl_CustomCallback">
																		<diagramserializable>
                                                            <cc1:XYDiagram>
                                                                <AxisX VisibleInPanesSerializable="-1" DateTimeMeasureUnit="Hour" 
                                                                    datetimegridalignment="Hour"  title-text="Time" title-visible="True">
                                                                    <label>
                                                                    <resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" />
                                                                    <resolveoverlappingoptions allowrotate="False" />
                                                                    <resolveoverlappingoptions allowrotate="False"></resolveoverlappingoptions>
                                                                    <datetimeoptions autoformat="False" format="ShortTime" />
                                                                    <datetimeoptions autoformat="False" format="ShortTime" /></label>
                                                                    <Range SideMarginsEnabled="True"></Range>
                                                                    <datetimeoptions format="ShortTime" />
                                                                <datetimeoptions format="ShortTime" />
                                                                    <visualrange autosidemargins="True" />
                                                                    <wholerange autosidemargins="True" />
                                                                    <datetimeoptions format="ShortTime" /><datetimeoptions format="ShortTime" /><visualrange autosidemargins="True" /><wholerange autosidemargins="True" /><datetimescaleoptions autogrid="False" gridalignment="Hour" measureunit="Hour">
                                                                    </datetimescaleoptions>
                                                                </AxisX>
                                                                <AxisY VisibleInPanesSerializable="-1" title-text="Count" title-visible="True"><Range SideMarginsEnabled="True"  ></Range>
                                                                    <visualrange autosidemargins="True" />
                                                                    <wholerange autosidemargins="True" />
                                                                <visualrange autosidemargins="True" /><wholerange autosidemargins="True" /></AxisY>
                                                            </cc1:XYDiagram>
                                                        </diagramserializable>
																		<fillstyle><OptionsSerializable><cc1:SolidFillOptions></cc1:SolidFillOptions></OptionsSerializable>
</fillstyle>
																		<seriesserializable>
                    <cc1:Series Name="Series 1" ArgumentScaleType="DateTime">
                        <viewserializable><cc1:LineSeriesView></cc1:LineSeriesView></viewserializable>
                        <labelserializable><cc1:PointSeriesLabel LineVisible="True"><fillstyle><optionsserializable><cc1:SolidFillOptions /></optionsserializable></fillstyle><pointoptionsserializable><cc1:PointOptions></cc1:PointOptions></pointoptionsserializable></cc1:PointSeriesLabel></labelserializable>
                        <legendpointoptionsserializable><cc1:PointOptions></cc1:PointOptions></legendpointoptionsserializable>
                    </cc1:Series>
                    <cc1:Series Name="Series 2" ArgumentScaleType="DateTime">
                        <viewserializable><cc1:LineSeriesView></cc1:LineSeriesView></viewserializable>
                        <labelserializable><cc1:PointSeriesLabel LineVisible="True"><fillstyle><optionsserializable><cc1:SolidFillOptions /></optionsserializable></fillstyle><pointoptionsserializable><cc1:PointOptions></cc1:PointOptions></pointoptionsserializable></cc1:PointSeriesLabel></labelserializable>
                        <legendpointoptionsserializable><cc1:PointOptions></cc1:PointOptions></legendpointoptionsserializable>
                    </cc1:Series>
                </seriesserializable>
																		<seriestemplate>
                    <viewserializable><cc1:LineSeriesView></cc1:LineSeriesView></viewserializable>
                    <labelserializable><cc1:PointSeriesLabel LineVisible="True"><fillstyle><optionsserializable><cc1:SolidFillOptions /></optionsserializable></fillstyle><pointoptionsserializable><cc1:PointOptions></cc1:PointOptions></pointoptionsserializable></cc1:PointSeriesLabel></labelserializable>
                    <legendpointoptionsserializable><cc1:PointOptions></cc1:PointOptions></legendpointoptionsserializable>
                </seriestemplate>
																		<crosshairoptions><CommonLabelPositionSerializable><cc1:CrosshairMousePosition></cc1:CrosshairMousePosition></CommonLabelPositionSerializable>
</crosshairoptions>
																		<tooltipoptions><ToolTipPositionSerializable><cc1:ToolTipMousePosition></cc1:ToolTipMousePosition></ToolTipPositionSerializable>
</tooltipoptions>
																		<titles>
                                                                                            <cc1:ChartTitle Text="Mail Delivered" />
                                                                                        </titles>
																	</dxchartsui:WebChartControl>
																</ContentTemplate>
																<Triggers>
																	<asp:AsyncPostBackTrigger ControlID="ServerListComboBox" />
																</Triggers>
															</asp:UpdatePanel>
														</td>
													</tr>
													<tr>
														<td valign="top">
															<asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
																<ContentTemplate>
																	<dxchartsui:WebChartControl ID="MailTranfferedWebChartControl" ClientInstanceName="cMailTranfferedWebChartControl"
																		runat="server" Height="300px" Width="500px" CrosshairEnabled="True" OnCustomCallback="MailTranfferedWebChartControl_CustomCallback">
																		<diagramserializable>
                                                                                            <cc1:XYDiagram>
                                                                                                <axisx datetimegridalignment="Hour" datetimemeasureunit="Hour" 
                                                                                                    visibleinpanesserializable="-1"  title-text="Time" title-visible="True">
                                                                                                    <label>
                                                                                                    <resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" />
                                                                                                    <resolveoverlappingoptions allowrotate="False" />
                                                                                                    <resolveoverlappingoptions allowrotate="False"></resolveoverlappingoptions>
                                                                                                    <datetimeoptions autoformat="False" format="ShortTime" />
                                                                                                    <datetimeoptions autoformat="False" format="ShortTime" /></label>
                                                                                                    <range sidemarginsenabled="True" />
                                                                                                    <datetimeoptions format="ShortTime" />
                                                                                                <range sidemarginsenabled="True" /><datetimeoptions format="ShortTime" />
                                                                                                    <visualrange autosidemargins="True" />
                                                                                                    <wholerange autosidemargins="True" />
                                                                                                    <range sidemarginsenabled="True" /><datetimeoptions format="ShortTime" /><range sidemarginsenabled="True" /><datetimeoptions format="ShortTime" /><visualrange autosidemargins="True" /><wholerange autosidemargins="True" /><datetimescaleoptions autogrid="False" gridalignment="Hour" measureunit="Hour">
                                                                                                    </datetimescaleoptions>
                                                                                                </axisx>
                                                                                                <axisy visibleinpanesserializable="-1"  title-text="Count" title-visible="True">
                                                                                                    <range sidemarginsenabled="True" />
                                                                                                <range sidemarginsenabled="True" />
                                                                                                    <visualrange autosidemargins="True" />
                                                                                                    <wholerange autosidemargins="True" />
                                                                                                <range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><visualrange autosidemargins="True" /><wholerange autosidemargins="True" /></axisy>
                                                                                            </cc1:XYDiagram>
                                                                                        </diagramserializable>
																		<fillstyle>
                                                                    <optionsserializable>
                                                                        <cc1:SolidFillOptions />
                                                                    </optionsserializable>
                                                                </fillstyle>
																		<seriesserializable>
                                                                                            <cc1:Series ArgumentScaleType="DateTime" Name="Series 1">
                                                                                                <viewserializable>
                                                                                                    <cc1:LineSeriesView>
                                                                                                    </cc1:LineSeriesView>
                                                                                                </viewserializable>
                                                                                                <labelserializable>
                                                                                                    <cc1:PointSeriesLabel LineVisible="True">
                                                                                                        <fillstyle>
                                                                                                            <optionsserializable>
                                                                                                                <cc1:SolidFillOptions />
                                                                                                            </optionsserializable>
                                                                                                        </fillstyle>
                                                                                                        <pointoptionsserializable>
                                                                                                            <cc1:PointOptions>
                                                                                                            </cc1:PointOptions>
                                                                                                        </pointoptionsserializable>
                                                                                                    </cc1:PointSeriesLabel>
                                                                                                </labelserializable>
                                                                                                <legendpointoptionsserializable>
                                                                                                    <cc1:PointOptions>
                                                                                                    </cc1:PointOptions>
                                                                                                </legendpointoptionsserializable>
                                                                                            </cc1:Series>
                                                                                            <cc1:Series ArgumentScaleType="DateTime" Name="Series 2">
                                                                                                <viewserializable>
                                                                                                    <cc1:LineSeriesView>
                                                                                                    </cc1:LineSeriesView>
                                                                                                </viewserializable>
                                                                                                <labelserializable>
                                                                                                    <cc1:PointSeriesLabel LineVisible="True">
                                                                                                        <fillstyle>
                                                                                                            <optionsserializable>
                                                                                                                <cc1:SolidFillOptions />
                                                                                                            </optionsserializable>
                                                                                                        </fillstyle>
                                                                                                        <pointoptionsserializable>
                                                                                                            <cc1:PointOptions>
                                                                                                            </cc1:PointOptions>
                                                                                                        </pointoptionsserializable>
                                                                                                    </cc1:PointSeriesLabel>
                                                                                                </labelserializable>
                                                                                                <legendpointoptionsserializable>
                                                                                                    <cc1:PointOptions>
                                                                                                    </cc1:PointOptions>
                                                                                                </legendpointoptionsserializable>
                                                                                            </cc1:Series>
                                                                                        </seriesserializable>
																		<seriestemplate argumentscaletype="DateTime">
                                                                    <viewserializable>
                                                                        <cc1:LineSeriesView>
                                                                        </cc1:LineSeriesView>
                                                                    </viewserializable>
                                                                    <labelserializable>
                                                                        <cc1:PointSeriesLabel LineVisible="True">
                                                                            <fillstyle>
                                                                                <optionsserializable>
                                                                                    <cc1:SolidFillOptions />
                                                                                </optionsserializable>
                                                                            </fillstyle>
                                                                            <pointoptionsserializable>
                                                                                <cc1:PointOptions>
                                                                                </cc1:PointOptions>
                                                                            </pointoptionsserializable>
                                                                        </cc1:PointSeriesLabel>
                                                                    </labelserializable>
                                                                    <legendpointoptionsserializable>
                                                                        <cc1:PointOptions>
                                                                        </cc1:PointOptions>
                                                                    </legendpointoptionsserializable>
                                                                </seriestemplate>
																		<crosshairoptions>
                                                                    <commonlabelpositionserializable>
                                                                        <cc1:CrosshairMousePosition />
                                                                    </commonlabelpositionserializable>
                                                                </crosshairoptions>
																		<tooltipoptions>
                                                                    <tooltippositionserializable>
                                                                        <cc1:ToolTipMousePosition />
                                                                    </tooltippositionserializable>
                                                                </tooltipoptions>
																		<titles>
                                                                                            <cc1:ChartTitle Text="Mail Transferred" />
                                                                                        </titles>
																	</dxchartsui:WebChartControl>
																</ContentTemplate>
																<Triggers>
																	<asp:AsyncPostBackTrigger ControlID="ServerListComboBox" />
																</Triggers>
															</asp:UpdatePanel>
														</td>
														<td>
															<asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
																<ContentTemplate>
																	<dxchartsui:WebChartControl ID="MailTraficWebChartControl" ClientInstanceName="cMailTraficWebChartControl"
																		runat="server" Height="300px" Width="500px" CrosshairEnabled="True" OnCustomCallback="MailTraficWebChartControl_CustomCallback">
																		<diagramserializable>
                                                                                            <cc1:XYDiagram>
                                                                                                <axisx visibleinpanesserializable="-1"  title-text="Mail Status" title-visible="True"><range sidemarginsenabled="True" /><range sidemarginsenabled="True" /><tickmarks minorvisible="False" /><range sidemarginsenabled="True" />
                                                                                                    <tickmarks minorvisible="False" />
                                                                                                    <visualrange autosidemargins="True" />
                                                                                                    <wholerange autosidemargins="True" />
                                                                                                <range sidemarginsenabled="True"   /><range sidemarginsenabled="True" /><tickmarks minorvisible="False" /><range sidemarginsenabled="True" /><tickmarks minorvisible="False" /><visualrange autosidemargins="True" /><wholerange autosidemargins="True" /></axisx>
                                                                                                <axisy visibleinpanesserializable="-1" title-text="Count" title-visible="True">
                                                                                                    <range sidemarginsenabled="True" />
																									 <visualrange autosidemargins="True" />
                                                                                                    <wholerange autosidemargins="True" />
                                                                                                </axisy>
                                                                                            </cc1:XYDiagram>
                                                                                        </diagramserializable>
																		<fillstyle>
                                                                    <optionsserializable><cc1:SolidFillOptions /></optionsserializable>
                                                                </fillstyle>
																		<seriesserializable>
                                                                                            <cc1:Series Name="Series 1">
                                                                                                <viewserializable><cc1:SideBySideBarSeriesView></cc1:SideBySideBarSeriesView></viewserializable>
                                                                                                <labelserializable><cc1:SideBySideBarSeriesLabel LineVisible="True"><fillstyle><optionsserializable><cc1:SolidFillOptions /></optionsserializable></fillstyle><pointoptionsserializable><cc1:PointOptions></cc1:PointOptions></pointoptionsserializable></cc1:SideBySideBarSeriesLabel></labelserializable>
                                                                                                <legendpointoptionsserializable><cc1:PointOptions></cc1:PointOptions></legendpointoptionsserializable>
                                                                                            </cc1:Series>
                                                                                            <cc1:Series Name="Series 2">
                                                                                                <viewserializable><cc1:SideBySideBarSeriesView></cc1:SideBySideBarSeriesView></viewserializable>
                                                                                                <labelserializable><cc1:SideBySideBarSeriesLabel LineVisible="True"><fillstyle><optionsserializable><cc1:SolidFillOptions /></optionsserializable></fillstyle><pointoptionsserializable><cc1:PointOptions></cc1:PointOptions></pointoptionsserializable></cc1:SideBySideBarSeriesLabel></labelserializable>
                                                                                                <legendpointoptionsserializable><cc1:PointOptions></cc1:PointOptions></legendpointoptionsserializable>
                                                                                            </cc1:Series>
                                                                                        </seriesserializable>
																		<seriestemplate>
                                                                    <viewserializable><cc1:SideBySideBarSeriesView></cc1:SideBySideBarSeriesView></viewserializable>
                                                                    <labelserializable><cc1:SideBySideBarSeriesLabel LineVisible="True"><fillstyle><optionsserializable><cc1:SolidFillOptions /></optionsserializable></fillstyle><pointoptionsserializable><cc1:PointOptions></cc1:PointOptions></pointoptionsserializable></cc1:SideBySideBarSeriesLabel></labelserializable>
                                                                    <legendpointoptionsserializable><cc1:PointOptions></cc1:PointOptions></legendpointoptionsserializable>
                                                                </seriestemplate>
																		<crosshairoptions>
                                                                    <commonlabelpositionserializable><cc1:CrosshairMousePosition /></commonlabelpositionserializable>
                                                                </crosshairoptions>
																		<tooltipoptions>
                                                                    <tooltippositionserializable><cc1:ToolTipMousePosition /></tooltippositionserializable>
                                                                </tooltipoptions>
																		<titles>
                                                                                            <cc1:ChartTitle Text="Mail Traffic" />
                                                                                        </titles>
																	</dxchartsui:WebChartControl>
																</ContentTemplate>
																<Triggers>
																	<asp:AsyncPostBackTrigger ControlID="ServerListComboBox" />
																</Triggers>
															</asp:UpdatePanel>
														</td>
													</tr>
												</table>
											</td>
										</tr>
										<tr>
											<td>
											</td>
										</tr>
									</table>
								</dx:ContentControl>
							</ContentCollection>
						</dx:TabPage>
						<dx:TabPage Text="Exchange">
							<ContentCollection>
								<dx:ContentControl ID="ContentControl2" runat="server">
									<table class="navbarTbl">
										<tr>
											<td>
												<table>
													<tr>
														<td>
															<dx:ASPxLabel ID="ASPxLabel2" runat="server" CssClass="lblsmallFont" Text="Select a server:">
															</dx:ASPxLabel>
														</td>
														<td>
															<dx:ASPxComboBox ID="ServerListExchangeComboBox" runat="server" OnSelectedIndexChanged="ServerListExchangeComboBox_SelectedIndexChanged"
																AutoPostBack="True">
															</dx:ASPxComboBox>
														</td>
													</tr>
												</table>
											</td>
										</tr>
										<tr>
											<td valign="top">
												<asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional">
													<ContentTemplate>
														<dxchartsui:WebChartControl ID="QueueWebChart" runat="server" ClientInstanceName="cQueueWebChart"
															CrosshairEnabled="True" Height="300px" Width="500px" OnCustomCallback="QueueWebChart_CustomCallback">
															<diagramserializable>
                                                    <cc1:XYDiagram>
                                                        <axisx visibleinpanesserializable="-1" title-text="Time" title-visible="True">
                                                            <tickmarks minorvisible="False" />
                                                            <label>
                                                            <resolveoverlappingoptions allowrotate="False" />
                                                            <datetimeoptions autoformat="False" format="ShortTime" />
                                                            <datetimeoptions autoformat="False" format="Custom" />
                                                            <datetimeoptions autoformat="False" format="ShortTime" />
                                                            <resolveoverlappingoptions allowrotate="False" allowstagger="False"></resolveoverlappingoptions>
                                                            <datetimeoptions autoformat="False" format="ShortTime" />
                                                            </label>
                                                            <datetimescaleoptions autogrid="False" gridalignment="Hour" measureunit="Hour">
                                                            </datetimescaleoptions>
                                                        </axisx>
                                                        <axisy visibleinpanesserializable="-1" title-text="Count" title-visible="True">
                                                          <range sidemarginsenabled="True" />
                                                          <visualrange autosidemargins="True" />
                                                          <wholerange autosidemargins="True" />
                                                        </axisy>
                                                    </cc1:XYDiagram>
                                                </diagramserializable>
															<seriesserializable>
                                                    <cc1:Series ArgumentScaleType="DateTime" Name="Series 1">
                                                        <viewserializable>
                                                            <cc1:LineSeriesView MarkerVisibility="True">
                                                            </cc1:LineSeriesView>
                                                        </viewserializable>
                                                    </cc1:Series>
                                                    <cc1:Series ArgumentScaleType="DateTime" Name="Series 2">
                                                        <viewserializable>
                                                            <cc1:LineSeriesView>
                                                            </cc1:LineSeriesView>
                                                        </viewserializable>
                                                    </cc1:Series>
                                                </seriesserializable>
															<seriestemplate>
                                                    <viewserializable>
                                                        <cc1:LineSeriesView>
                                                        </cc1:LineSeriesView>
                                                    </viewserializable>
                                                </seriestemplate>
															<titles>
                                                    <cc1:ChartTitle Text="Mail Queues" />
                                                </titles>
														</dxchartsui:WebChartControl>
													</ContentTemplate>
													<Triggers>
														<asp:AsyncPostBackTrigger ControlID="ServerListExchangeComboBox" />
													</Triggers>
												</asp:UpdatePanel>
											</td>
											<td valign="top">
												<asp:UpdatePanel ID="UpdatePanel6" runat="server" UpdateMode="Conditional">
													<ContentTemplate>
														<dxchartsui:WebChartControl ID="DeliveryRateWebChart" runat="server" ClientInstanceName="cDeliveryRateWebChart"
															CrosshairEnabled="True" Height="300px" Width="500px" OnCustomCallback="DeliveryRateWebChart_CustomCallback">
															<diagramserializable>
                                                    <cc1:XYDiagram>
                                                        <axisx visibleinpanesserializable="-1" title-text="Time" title-visible="True" >
                                                            <tickmarks minorvisible="False" />
                                                            <label>
                                                            <resolveoverlappingoptions allowrotate="False" />
                                                            <datetimeoptions autoformat="False" format="ShortTime" />
                                                            </label>
                                                            <datetimescaleoptions measureunit="Hour">
                                                            </datetimescaleoptions>
                                                        </axisx>
                                                         <axisy visibleinpanesserializable="-1" title-text="Delivery Rate" title-visible="True" >
                                                            <range sidemarginsenabled="True" />
                                                            <visualrange autosidemargins="True" />
                                                            <wholerange autosidemargins="True" />
                                                            </axisy>
                                                    </cc1:XYDiagram>
                                                </diagramserializable>
															<seriesserializable>
                                                    <cc1:Series ArgumentScaleType="DateTime" Name="Series 1">
                                                        <viewserializable>
                                                            <cc1:LineSeriesView MarkerVisibility="True">
                                                            </cc1:LineSeriesView>
                                                        </viewserializable>
                                                    </cc1:Series>
                                                    <cc1:Series ArgumentScaleType="DateTime" Name="Series 2">
                                                        <viewserializable>
                                                            <cc1:LineSeriesView>
                                                            </cc1:LineSeriesView>
                                                        </viewserializable>
                                                    </cc1:Series>
                                                </seriesserializable>
															<seriestemplate>
                                                    <viewserializable>
                                                        <cc1:LineSeriesView>
                                                        </cc1:LineSeriesView>
                                                    </viewserializable>
                                                </seriestemplate>
															<titles>
                                                    <cc1:ChartTitle Text="Mail Delivery Success Rate" />
                                                </titles>
														</dxchartsui:WebChartControl>
													</ContentTemplate>
													<Triggers>
														<asp:AsyncPostBackTrigger ControlID="ServerListExchangeComboBox" />
													</Triggers>
												</asp:UpdatePanel>
											</td>
										</tr>
										<tr>
											<td>
												<asp:UpdatePanel ID="UpdatePanel7" runat="server" UpdateMode="Conditional">
													<ContentTemplate>
														<dxchartsui:WebChartControl ID="MailSizeWebChart" runat="server" ClientInstanceName="cMailSizeWebChart"
															CrosshairEnabled="True" Height="300px" Width="500px" OnCustomCallback="MailSizeWebChart_CustomCallback">
															<diagramserializable>
                                                    <cc1:XYDiagram>
                                                        <axisx visibleinpanesserializable="-1" title-text="Time" title-visible="True">
                                                            <tickmarks minorvisible="False" />
                                                            <tickmarks minorvisible="False" />
                                                            <tickmarks minorvisible="False" />
                                                            <label>
                                                            <resolveoverlappingoptions allowrotate="False" />
                                                            <datetimeoptions autoformat="False" format="ShortTime" />
                                                            <resolveoverlappingoptions allowrotate="False"></resolveoverlappingoptions>
                                                            <datetimeoptions autoformat="False" format="ShortTime" />
                                                            <datetimeoptions autoformat="False" format="ShortTime" />
                                                            </label>
                                                            <datetimescaleoptions measureunit="Hour">
                                                            </datetimescaleoptions>
                                                        </axisx>
                                                         <axisy visibleinpanesserializable="-1" title-text="Mail Received/Sent Size(MB)" title-visible="True" >
                                                          <range sidemarginsenabled="True" />
                                                           <visualrange autosidemargins="True" />
                                                           <wholerange autosidemargins="True" />
                                                           </axisy>
                                                    </cc1:XYDiagram>
                                                </diagramserializable>
															<seriesserializable>
                                                    <cc1:Series ArgumentScaleType="DateTime" Name="Series1">
                                                        <viewserializable>
                                                            <cc1:LineSeriesView MarkerVisibility="True">
                                                            </cc1:LineSeriesView>
                                                        </viewserializable>
                                                    </cc1:Series>
                                                    <cc1:Series ArgumentScaleType="DateTime" Name="Series2">
                                                        <viewserializable>
                                                            <cc1:LineSeriesView>
                                                            </cc1:LineSeriesView>
                                                        </viewserializable>
                                                    </cc1:Series>
                                                </seriesserializable>
															<seriestemplate>
                                                    <viewserializable>
                                                        <cc1:LineSeriesView>
                                                            <linemarkeroptions color="White">
                                                            </linemarkeroptions>
                                                        </cc1:LineSeriesView>
                                                    </viewserializable>
                                                </seriestemplate>
															<titles>
                                                    <cc1:ChartTitle Text="Mail Received/Sent Size" />
                                                </titles>
														</dxchartsui:WebChartControl>
													</ContentTemplate>
													<Triggers>
														<asp:AsyncPostBackTrigger ControlID="ServerListExchangeComboBox" />
													</Triggers>
												</asp:UpdatePanel>
											</td>
											<td>
												<asp:UpdatePanel ID="UpdatePanel8" runat="server" UpdateMode="Conditional">
													<ContentTemplate>
														<dxchartsui:WebChartControl ID="MailCountWebChart" runat="server" ClientInstanceName="cMailCountWebChart"
															CrosshairEnabled="True" Height="300px" Width="500px" OnCustomCallback="MailCountWebChart_CustomCallback">
															<diagramserializable>
                                                    <cc1:XYDiagram>
                                                        <axisx visibleinpanesserializable="-1" title-text="Time" title-visible="True">
                                                            <tickmarks minorvisible="False" />
                                                            <tickmarks minorvisible="False" />
                                                            <tickmarks minorvisible="False" />
                                                            <label>
                                                            <resolveoverlappingoptions allowrotate="False" />
                                                            <datetimeoptions autoformat="False" format="ShortTime" />
                                                            <resolveoverlappingoptions allowrotate="False"></resolveoverlappingoptions>
                                                            <datetimeoptions autoformat="False" format="ShortTime" />
                                                            <datetimeoptions autoformat="False" format="ShortTime" />
                                                            </label>
                                                            <datetimescaleoptions measureunit="Hour">
                                                            </datetimescaleoptions>
                                                        </axisx>
                                                        <axisy visibleinpanesserializable="-1" title-text="Mail Received/Sent Count" title-visible="True">
                                                         <range sidemarginsenabled="True" />
                                                          <visualrange autosidemargins="True" />
                                                          <wholerange autosidemargins="True" />
                                                          </axisy>
                                                    </cc1:XYDiagram>
                                                </diagramserializable>
															<seriesserializable>
                                                    <cc1:Series ArgumentScaleType="DateTime" Name="Series 1">
                                                        <viewserializable>
                                                            <cc1:LineSeriesView MarkerVisibility="True">
                                                            </cc1:LineSeriesView>
                                                        </viewserializable>
                                                    </cc1:Series>
                                                    <cc1:Series ArgumentScaleType="DateTime" Name="Series 2">
                                                        <viewserializable>
                                                            <cc1:LineSeriesView>
                                                            </cc1:LineSeriesView>
                                                        </viewserializable>
                                                    </cc1:Series>
                                                </seriesserializable>
															<seriestemplate>
                                                    <viewserializable>
                                                        <cc1:LineSeriesView>
                                                            <linemarkeroptions color="White">
                                                            </linemarkeroptions>
                                                        </cc1:LineSeriesView>
                                                    </viewserializable>
                                                </seriestemplate>
															<titles>
                                                    <cc1:ChartTitle Text="Mail Received/Sent Count" />
                                                </titles>
														</dxchartsui:WebChartControl>
													</ContentTemplate>
													<Triggers>
														<asp:AsyncPostBackTrigger ControlID="ServerListExchangeComboBox" />
													</Triggers>
												</asp:UpdatePanel>
											</td>
										</tr>
									</table>
								</dx:ContentControl>
							</ContentCollection>
						</dx:TabPage>
					</TabPages>
				</dx:ASPxPageControl>
			</td>
		</tr>
	</table>
</asp:Content>
