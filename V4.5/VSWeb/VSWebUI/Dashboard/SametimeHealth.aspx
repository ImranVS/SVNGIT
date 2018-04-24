<%@ Page Title="VitalSigns Plus - Sametime Server Health" Language="C#" MasterPageFile="~/DashboardSite.Master"
	AutoEventWireup="true" CodeBehind="SametimeHealth.aspx.cs" Inherits="VSWebUI.Dashboard.SametimeHealth" %>
	<%@ MasterType virtualpath="~/DashboardSite.Master" %>
<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.XtraCharts.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.XtraCharts" TagPrefix="cc1" %>
<%@ Register Assembly="DevExpress.XtraCharts.v14.2.Web, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.XtraCharts.Web" TagPrefix="dxchartsui" %>




<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<%@ Register Assembly="DevExpress.XtraCharts.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.XtraCharts" TagPrefix="cc2" %>

	
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
	<link href='http://fonts.googleapis.com/css?family=Francois One' rel='stylesheet'
		type='text/css' />
	<link rel="stylesheet" type="text/css" href="css/vswebforms.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
	<script type="text/javascript">

		function grid_SelectionChanged(s, e) {
			s.GetSelectedFieldValues("Name", GetSelectedFieldValuesCallback);
		}
		function GetSelectedFieldValuesCallback(values) {
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
		}

		function Resized() {
			var callbackState = document.getElementById('ContentPlaceHolder1_callbackState').value;

			if (callbackState == 0)
				DoCallback();
		}

		function DoCallback() {
			//1/29/2013 NS commented out the line below
			//document.getElementById('ContentPlaceHolder1_chartWidth').value = document.body.offsetWidth - 105;

			//cSelectServerRoundPanel.style.width = document.getElementById('ContentPlaceHolder1__chartWidth').value + "px";
			//
			//cResponseWebChartControl.Performcallback();
			var chartwidth = document.getElementById('ContentPlaceHolder1_chartWidth').value;
			cActiveChartWebChartControl.Performcallback();
			cActiveMeetingWebChartControl.Performcallback();
			cActiveNwayWebChartControl.Performcallback();
		}

		function ResizeChart(s, e) {
			document.getElementById('ContentPlaceHolder1_callbackState').value = 0;
			//1/29/2013 NS commented out the line below
			//s.GetMainElement().style.width = document.getElementById('ContentPlaceHolder1__chartWidth').value + "px";
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
	</script>
	<input id="chartWidth" type="hidden" runat="server" value="550" />
	<input id="callbackState" type="hidden" runat="server" value="0" />
	<table width="100%">
		<tr>
			<td>
				<div class="header" id="servernamelbldisp" runat="server">
					<img alt="" src="../images/icons/sametime.gif" />&nbsp;IBM Sametime Server Health</div>
			</td>
		</tr>
		<tr>
			<td>
			</td>
		</tr>
		<tr>
			<td>
				<table width="100%">
					<tr>
						<td colspan="2">
							<asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional">
								<ContentTemplate>
									<dx:ASPxGridView ID="DomionSametimegrid" runat="server" AutoGenerateColumns="False"
										SummaryText="m" CssFilePath="~/App_Themes/Office2010Silver/{0}/styles.css" OnPageSizeChanged="DomionSametimegrid_PageSizeChanged"
										CssPostfix="Office2010Silver" Width="100%" Theme="Office2003Blue" KeyFieldName="TypeANDName"
										Cursor="pointer" OnHtmlDataCellPrepared="DomionSametimegrid_HtmlDataCellPrepared"
										OnHtmlRowCreated="DomionSametimegrid_HtmlRowCreated" Visible="False">
										<SettingsBehavior AllowSelectByRowClick="True" ProcessSelectionChangedOnServer="True"
											ColumnResizeMode="NextColumn"></SettingsBehavior>
										<SettingsPager PageSize="20">
											<PageSizeItemSettings Visible="True">
											</PageSizeItemSettings>
										</SettingsPager>
										<Columns>
											<dx:GridViewDataTextColumn Caption="Name" VisibleIndex="0" FieldName="Name">
												<Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
										
												<EditCellStyle CssClass="GridCss">
												</EditCellStyle>
												<EditFormCaptionStyle CssClass="GridCss">
												</EditFormCaptionStyle>
												<HeaderStyle CssClass="GridCssHeader" />
												<CellStyle CssClass="GridCss" Wrap="True">
												</CellStyle>
											</dx:GridViewDataTextColumn>
											<dx:GridViewDataTextColumn Caption="Location" VisibleIndex="2" FieldName="Location">
										<Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
												<EditCellStyle CssClass="GridCss">
												</EditCellStyle>
												<EditFormCaptionStyle CssClass="GridCss">
												</EditFormCaptionStyle>
												<HeaderStyle CssClass="GridCssHeader" />
												<CellStyle CssClass="GridCss">
												</CellStyle>
											</dx:GridViewDataTextColumn>
											<dx:GridViewDataTextColumn Caption="Status" FieldName="Status" ShowInCustomizationForm="True"
												Visible="True" VisibleIndex="1">
											<Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
												
												<EditCellStyle CssClass="GridCss">
												</EditCellStyle>
												<EditFormCaptionStyle CssClass="GridCss">
												</EditFormCaptionStyle>
												<HeaderStyle CssClass="GridCssHeader" />
												<CellStyle CssClass="GridCss1">
												</CellStyle>
											</dx:GridViewDataTextColumn>
											<dx:GridViewDataTextColumn Caption="Details" VisibleIndex="3" FieldName="Details">
												<Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
											
												<EditCellStyle CssClass="GridCss">
												</EditCellStyle>
												<EditFormCaptionStyle CssClass="GridCss">
												</EditFormCaptionStyle>
												<HeaderStyle CssClass="GridCssHeader" />
												<CellStyle CssClass="GridCss">
												</CellStyle>
											</dx:GridViewDataTextColumn>
											<dx:GridViewDataTextColumn Caption="CPU" VisibleIndex="4" FieldName="CPU">
												<Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
												<EditCellStyle CssClass="GridCss">
												</EditCellStyle>
												<EditFormCaptionStyle CssClass="GridCss">
												</EditFormCaptionStyle>
												<HeaderStyle CssClass="GridCssHeader" />
												<CellStyle CssClass="GridCss">
												</CellStyle>
											</dx:GridViewDataTextColumn>
											<dx:GridViewDataTextColumn Caption="User Count" VisibleIndex="4" FieldName="UserCount">
											<Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
												<EditCellStyle CssClass="GridCss">
												</EditCellStyle>
												<EditFormCaptionStyle CssClass="GridCss">
												</EditFormCaptionStyle>
												<HeaderStyle CssClass="GridCssHeader" />
												<CellStyle CssClass="GridCss">
												</CellStyle>
											</dx:GridViewDataTextColumn>
										</Columns>
										<SettingsBehavior AllowDragDrop="False" AutoExpandAllGroups="True" AllowFocusedRow="true"
											AllowSelectByRowClick="True" ProcessSelectionChangedOnServer="True" />
										<SettingsPager PageSize="10" SEOFriendly="Enabled">
											<PageSizeItemSettings Visible="true" />
										</SettingsPager>
										<Settings ShowGroupPanel="True" ShowFilterRow="True" />
										<Settings ShowFilterRow="True" ShowGroupPanel="True"></Settings>
										<Images SpriteCssFilePath="~/App_Themes/Office2010Silver/{0}/sprite.css">
											<LoadingPanelOnStatusBar Url="~/App_Themes/Office2010Silver/GridView/Loading.gif">
											</LoadingPanelOnStatusBar>
											<LoadingPanel Url="~/App_Themes/Office2010Silver/GridView/Loading.gif">
											</LoadingPanel>
										</Images>
										<ImagesFilterControl>
											<LoadingPanel Url="~/App_Themes/Office2010Silver/GridView/Loading.gif">
											</LoadingPanel>
										</ImagesFilterControl>
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
											<AlternatingRow CssClass="GridAltrow" Enabled="True">
											</AlternatingRow>
										</Styles>
										<StylesEditors ButtonEditCellSpacing="0">
											<ProgressBar Height="21px">
											</ProgressBar>
										</StylesEditors>
									</dx:ASPxGridView>
								</ContentTemplate>
							</asp:UpdatePanel>
						</td>
					</tr>
				</table>
			</td>
		</tr>
		<tr>
			<td>
				<asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
					<ContentTemplate>
						<dx:ASPxGridView ID="Sametimegrid" runat="server" AutoGenerateColumns="False" SummaryText="m"
							Width="100%" KeyFieldName="TypeANDName" Cursor="pointer" OnHtmlDataCellPrepared="Sametimegrid_HtmlDataCellPrepared"
							OnPageSizeChanged="Sametimegrid_PageSizeChanged" OnHtmlRowCreated="Sametimegrid_HtmlRowCreated"
							Theme="Office2003Blue" ClientInstanceName="Sametimegrid" EnableCallBacks="False">
							<ClientSideEvents SelectionChanged="grid_SelectionChanged"></ClientSideEvents>
							<SettingsBehavior AllowSelectByRowClick="True" ProcessSelectionChangedOnServer="True"
								ColumnResizeMode="NextColumn" AllowFocusedRow="True"></SettingsBehavior>
							<SettingsPager PageSize="20">
								<PageSizeItemSettings Visible="True">
								</PageSizeItemSettings>
							</SettingsPager>
							<Columns>
								<dx:GridViewDataTextColumn Caption="Name" VisibleIndex="0" FieldName="Name">
									<Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
									<EditCellStyle CssClass="GridCss">
									</EditCellStyle>
									<EditFormCaptionStyle CssClass="GridCss">
									</EditFormCaptionStyle>
									<HeaderStyle CssClass="GridCssHeader" />
									<CellStyle CssClass="GridCss">
									</CellStyle>
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataTextColumn Caption="Location" VisibleIndex="2" FieldName="Location"
									Width="100px">
									<Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
									<EditCellStyle CssClass="GridCss">
									</EditCellStyle>
									<EditFormCaptionStyle CssClass="GridCss">
									</EditFormCaptionStyle>
									<HeaderStyle CssClass="GridCssHeader" />
									<CellStyle CssClass="GridCss">
									</CellStyle>
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataTextColumn Caption="Status" FieldName="Status" Visible="True" VisibleIndex="1"
									Width="100px">
								<Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
									<EditCellStyle CssClass="GridCss">
									</EditCellStyle>
									<EditFormCaptionStyle CssClass="GridCss">
									</EditFormCaptionStyle>
									<HeaderStyle CssClass="GridCssHeader" />
									<CellStyle CssClass="GridCss1">
									</CellStyle>
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataTextColumn Caption="Details" VisibleIndex="3" FieldName="Details">
									<Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
									<EditCellStyle CssClass="GridCss">
									</EditCellStyle>
									<EditFormCaptionStyle CssClass="GridCss">
									</EditFormCaptionStyle>
									<HeaderStyle CssClass="GridCssHeader" />
									<CellStyle CssClass="GridCss">
									</CellStyle>
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataTextColumn Caption="CPU" VisibleIndex="5" FieldName="CPU" Width="60px">
									<Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
									<EditCellStyle CssClass="GridCss">
									</EditCellStyle>
									<EditFormCaptionStyle CssClass="GridCss">
									</EditFormCaptionStyle>
									<HeaderStyle CssClass="GridCssHeader" />
									<CellStyle CssClass="GridCss">
									</CellStyle>
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataTextColumn Caption="User Count" VisibleIndex="4" FieldName="UserCount"
									Width="80px">
									<Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
									<EditCellStyle CssClass="GridCss">
									</EditCellStyle>
									<EditFormCaptionStyle CssClass="GridCss">
									</EditFormCaptionStyle>
									<HeaderStyle CssClass="GridCssHeader" />
									<CellStyle CssClass="GridCss">
									</CellStyle>
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataTextColumn Caption="Type" FieldName="SType" VisibleIndex="6" Width="150px">
									<HeaderStyle CssClass="GridCssHeader" />
									<CellStyle CssClass="GridCss">
									</CellStyle>
								</dx:GridViewDataTextColumn>
							</Columns>
							<SettingsBehavior AllowDragDrop="False" AutoExpandAllGroups="True" AllowFocusedRow="True"
								AllowSelectByRowClick="True" ProcessSelectionChangedOnServer="True" />
							<SettingsPager PageSize="10" SEOFriendly="Enabled">
								<PageSizeItemSettings Visible="true" />
							</SettingsPager>
							<Settings ShowGroupPanel="True" ShowFilterRow="True" />
							<Settings ShowFilterRow="True" ShowGroupPanel="True"></Settings>
							<Images SpriteCssFilePath="~/App_Themes/Office2010Silver/{0}/sprite.css">
								<LoadingPanelOnStatusBar Url="~/App_Themes/Office2010Silver/GridView/Loading.gif">
								</LoadingPanelOnStatusBar>
								<LoadingPanel Url="~/App_Themes/Office2010Silver/GridView/Loading.gif">
								</LoadingPanel>
							</Images>
							<ImagesFilterControl>
								<LoadingPanel Url="~/App_Themes/Office2010Silver/GridView/Loading.gif">
								</LoadingPanel>
							</ImagesFilterControl>
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
						</dx:ASPxGridView>
					</ContentTemplate>
				</asp:UpdatePanel>
			</td>
		</tr>
		
		<tr>
			<td>
			
		<asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <dx:ASPxPageControl ID="ASPxPageControl1" ActiveTabIndex="0" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
                CssPostfix="Glass" SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" TabSpacing="0px"
                runat="server" Width="100%" EnableHierarchyRecreation="False">
                <TabPages>
                    <dx:TabPage Text="Overall">
                        <TabImage Url="~/images/icons/information.png">
                        </TabImage>
                        <ContentCollection>
                            <dx:Contentcontrol ID="ContentControl1" runat="server" SupportsDisabledAttribute="True">
                                <table width="100%">
								<tr>
						       <td colspan="2" align="center" style="text-align: center">
							   <dx:ASPxLabel ID="ASPxLabel4" runat="server" Font-Bold="true" Visible="false">
							   </dx:ASPxLabel>
						       </td>
					           </tr>
                                  <tr>
						<td><asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
								<ContentTemplate>
							<dx:ASPxRoundPanel ID="ASPxRoundPanel5" runat="server" HeaderText="Response Times"
								Theme="Glass" Width="100%">
								<PanelCollection>
									<dx:PanelContent ID="PanelContent2" runat="server" SupportsDisabledAttribute="True">
										<dxchartsui:WebChartControl ID="ResponseTimesWebChartControl" runat="server" Height="300px"
											Width="550px" ClientInstanceName="cResponseTimesWebChartControl" 
                                            OnBoundDataChanged="ResponseTimesWebChartControl_BoundDataChanged">
											<fillstyle>
            <optionsserializable><cc2:SolidFillOptions /></optionsserializable>
        </fillstyle>
											<seriestemplate>
            <viewserializable><cc2:SideBySideBarSeriesView></cc2:SideBySideBarSeriesView></viewserializable>
            <labelserializable><cc2:SideBySideBarSeriesLabel LineVisible="True"><fillstyle><optionsserializable><cc2:SolidFillOptions /></optionsserializable></fillstyle><pointoptionsserializable><cc2:PointOptions></cc2:PointOptions></pointoptionsserializable></cc2:SideBySideBarSeriesLabel></labelserializable>
            <legendpointoptionsserializable><cc2:PointOptions></cc2:PointOptions></legendpointoptionsserializable>
        </seriestemplate>
											<crosshairoptions>
            <commonlabelpositionserializable><cc2:CrosshairMousePosition /></commonlabelpositionserializable>
        </crosshairoptions>
											<tooltipoptions>
            <tooltippositionserializable><cc2:ToolTipMousePosition /></tooltippositionserializable>
        </tooltipoptions>
										</dxchartsui:WebChartControl>
									</dx:PanelContent>
								</PanelCollection>
							</dx:ASPxRoundPanel>
							</ContentTemplate>
							<Triggers>
									<asp:AsyncPostBackTrigger ControlID="Sametimegrid" />
								</Triggers>
							</asp:UpdatePanel>
						</td>
						<td>
							<asp:UpdatePanel ID="UpdatePanel18" runat="server" UpdateMode="Conditional">
								<ContentTemplate>
									<dx:ASPxRoundPanel ID="ASPxRoundPanel2" runat="server" HeaderText="Users" Theme="Glass" Width="100%">
										<PanelCollection>
											<dx:PanelContent ID="PanelContent10" runat="server" SupportsDisabledAttribute="True">
												<dxchartsui:WebChartControl ID="ActiveChartWebChartControl" runat="server" ClientInstanceName="cActiveChartWebChartControl"
													Height="300px" Width="550px" OnBoundDataChanged="ActiveChartWebChartControl_BoundDataChanged">
													<fillstyle>
                                                <optionsserializable><cc2:SolidFillOptions /></optionsserializable>
                                            </fillstyle>
													<seriestemplate>
                                                <viewserializable><cc2:LineSeriesView></cc2:LineSeriesView></viewserializable>
                                                <labelserializable><cc2:PointSeriesLabel LineVisible="True"><fillstyle><optionsserializable><cc2:SolidFillOptions /></optionsserializable></fillstyle><pointoptionsserializable><cc2:PointOptions></cc2:PointOptions></pointoptionsserializable></cc2:PointSeriesLabel></labelserializable>
                                                <legendpointoptionsserializable><cc2:PointOptions></cc2:PointOptions></legendpointoptionsserializable>
                                            </seriestemplate>
													<crosshairoptions>
                                                <commonlabelpositionserializable><cc2:CrosshairMousePosition /></commonlabelpositionserializable>
                                            </crosshairoptions>
													<tooltipoptions>
                                                <tooltippositionserializable><cc2:ToolTipMousePosition /></tooltippositionserializable>
                                            </tooltipoptions>
												</dxchartsui:WebChartControl>
											</dx:PanelContent>
										</PanelCollection>
									</dx:ASPxRoundPanel>
								</ContentTemplate>
								<Triggers>
									<asp:AsyncPostBackTrigger ControlID="Sametimegrid" />
								</Triggers>
							</asp:UpdatePanel>
						</td>
					</tr>  
                                </table>
                            </dx:ContentControl>
                        </ContentCollection>
                    </dx:TabPage>
					<dx:TabPage Text="Chats">
                        <TabImage Url="~/images/icons/information.png">
                        </TabImage>
                        <ContentCollection>
                            <dx:Contentcontrol ID="ContentControl2" runat="server" SupportsDisabledAttribute="True">
                                <table width="100%">
								<tr>
						       <td colspan="2" align="center" style="text-align: center">
							   <dx:ASPxLabel ID="ASPxLabel5" runat="server" Font-Bold="true" Visible="false">
							   </dx:ASPxLabel>
						       </td>
					           </tr>
                                     <tr>
                    <td>
							<asp:UpdatePanel ID="UpdatePanel19" runat="server" UpdateMode="Conditional">
								<ContentTemplate>
									<dx:ASPxRoundPanel ID="ASPxRoundPanel4" runat="server" HeaderText="Number of n-way chats " Theme="Glass" Width="100%">
										<PanelCollection>
											<dx:PanelContent ID="PanelContent11" runat="server" SupportsDisabledAttribute="True">
												<dxchartsui:WebChartControl ID="Numberofnwaychats" runat="server" ClientInstanceName="NumberofnwaychatsWebChartControl"
													Height="300px" Width="550px" OnBoundDataChanged="Numberofnwaychats_BoundDataChanged">
													<fillstyle>
                                                <optionsserializable><cc2:SolidFillOptions /></optionsserializable>
                                            </fillstyle>
													<seriestemplate>
                                                <viewserializable><cc2:LineSeriesView></cc2:LineSeriesView></viewserializable>
                                                <labelserializable><cc2:PointSeriesLabel LineVisible="True"><fillstyle><optionsserializable><cc2:SolidFillOptions /></optionsserializable></fillstyle><pointoptionsserializable><cc2:PointOptions></cc2:PointOptions></pointoptionsserializable></cc2:PointSeriesLabel></labelserializable>
                                                <legendpointoptionsserializable><cc2:PointOptions></cc2:PointOptions></legendpointoptionsserializable>
                                            </seriestemplate>
													<crosshairoptions>
                                                <commonlabelpositionserializable><cc2:CrosshairMousePosition /></commonlabelpositionserializable>
                                            </crosshairoptions>
													<tooltipoptions>
                                                <tooltippositionserializable><cc2:ToolTipMousePosition /></tooltippositionserializable>
                                            </tooltipoptions>
												</dxchartsui:WebChartControl>
											</dx:PanelContent>
										</PanelCollection>
									</dx:ASPxRoundPanel>
								</ContentTemplate>
								<Triggers>
									<asp:AsyncPostBackTrigger ControlID="Sametimegrid" />
								</Triggers>
							</asp:UpdatePanel>
						</td>
					<td>
							<asp:UpdatePanel ID="UpdatePanel20" runat="server" UpdateMode="Conditional">
								<ContentTemplate>
									<dx:ASPxRoundPanel ID="ASPxRoundPanel6" runat="server" HeaderText="Number of chat messages " Theme="Glass" Width="100%">
										<PanelCollection>
											<dx:PanelContent ID="PanelContent12" runat="server" SupportsDisabledAttribute="True">
												<dxchartsui:WebChartControl ID="Numberofchatmessages" runat="server" ClientInstanceName="NumberofchatmessagesWebChartControl"
													Height="300px" Width="550px" OnBoundDataChanged="Numberofchatmessages_BoundDataChanged">
													<fillstyle>
                                                <optionsserializable><cc2:SolidFillOptions /></optionsserializable>
                                            </fillstyle>
													<seriestemplate>
                                                <viewserializable><cc2:LineSeriesView></cc2:LineSeriesView></viewserializable>
                                                <labelserializable><cc2:PointSeriesLabel LineVisible="True"><fillstyle><optionsserializable><cc2:SolidFillOptions /></optionsserializable></fillstyle><pointoptionsserializable><cc2:PointOptions></cc2:PointOptions></pointoptionsserializable></cc2:PointSeriesLabel></labelserializable>
                                                <legendpointoptionsserializable><cc2:PointOptions></cc2:PointOptions></legendpointoptionsserializable>
                                            </seriestemplate>
													<crosshairoptions>
                                                <commonlabelpositionserializable><cc2:CrosshairMousePosition /></commonlabelpositionserializable>
                                            </crosshairoptions>
													<tooltipoptions>
                                                <tooltippositionserializable><cc2:ToolTipMousePosition /></tooltippositionserializable>
                                            </tooltipoptions>
												</dxchartsui:WebChartControl>
											</dx:PanelContent>
										</PanelCollection>
									</dx:ASPxRoundPanel>
								</ContentTemplate>
								<Triggers>
									<asp:AsyncPostBackTrigger ControlID="Sametimegrid" />
								</Triggers>
							</asp:UpdatePanel>
						</td>
					</tr>
					<tr>
					<td>
							<asp:UpdatePanel ID="UpdatePanel21" runat="server" UpdateMode="Conditional">
								<ContentTemplate>
									<dx:ASPxRoundPanel ID="ASPxRoundPanel7" runat="server" HeaderText=" Number of open chat sessions" Theme="Glass" Width="100%">
										<PanelCollection>
											<dx:PanelContent ID="PanelContent13" runat="server" SupportsDisabledAttribute="True">
												<dxchartsui:WebChartControl ID="Numberofopenchatsessions" runat="server" ClientInstanceName="NumberofopenchatsessionsWebChartControl"
													Height="300px" Width="550px" OnBoundDataChanged="Numberofopenchatsessions_BoundDataChanged">
													<fillstyle>
                                                <optionsserializable><cc2:SolidFillOptions /></optionsserializable>
                                            </fillstyle>
													<seriestemplate>
                                                <viewserializable><cc2:LineSeriesView></cc2:LineSeriesView></viewserializable>
                                                <labelserializable><cc2:PointSeriesLabel LineVisible="True"><fillstyle><optionsserializable><cc2:SolidFillOptions /></optionsserializable></fillstyle><pointoptionsserializable><cc2:PointOptions></cc2:PointOptions></pointoptionsserializable></cc2:PointSeriesLabel></labelserializable>
                                                <legendpointoptionsserializable><cc2:PointOptions></cc2:PointOptions></legendpointoptionsserializable>
                                            </seriestemplate>
													<crosshairoptions>
                                                <commonlabelpositionserializable><cc2:CrosshairMousePosition /></commonlabelpositionserializable>
                                            </crosshairoptions>
													<tooltipoptions>
                                                <tooltippositionserializable><cc2:ToolTipMousePosition /></tooltippositionserializable>
                                            </tooltipoptions>
												</dxchartsui:WebChartControl>
											</dx:PanelContent>
										</PanelCollection>
									</dx:ASPxRoundPanel>
								</ContentTemplate>
								<Triggers>
									<asp:AsyncPostBackTrigger ControlID="Sametimegrid" />
								</Triggers>
							</asp:UpdatePanel>
						</td>
                    <td>
							<asp:UpdatePanel ID="UpdatePanel22" runat="server" UpdateMode="Conditional">
								<ContentTemplate>
									<dx:ASPxRoundPanel ID="ASPxRoundPanel8" runat="server" HeaderText="Number of active n-way chats " Theme="Glass" Width="100%">
										<PanelCollection>
											<dx:PanelContent ID="PanelContent14" runat="server" SupportsDisabledAttribute="True">
												<dxchartsui:WebChartControl ID="Numberofactivenwaychats1" runat="server" ClientInstanceName="CountOfPSActiveUsersWebChartControl"
													Height="300px" Width="550px" OnBoundDataChanged="Numberofactivenwaychats1_BoundDataChanged">
													<fillstyle>
                                                <optionsserializable><cc2:SolidFillOptions /></optionsserializable>
                                            </fillstyle>
													<seriestemplate>
                                                <viewserializable><cc2:LineSeriesView></cc2:LineSeriesView></viewserializable>
                                                <labelserializable><cc2:PointSeriesLabel LineVisible="True"><fillstyle><optionsserializable><cc2:SolidFillOptions /></optionsserializable></fillstyle><pointoptionsserializable><cc2:PointOptions></cc2:PointOptions></pointoptionsserializable></cc2:PointSeriesLabel></labelserializable>
                                                <legendpointoptionsserializable><cc2:PointOptions></cc2:PointOptions></legendpointoptionsserializable>
                                            </seriestemplate>
													<crosshairoptions>
                                                <commonlabelpositionserializable><cc2:CrosshairMousePosition /></commonlabelpositionserializable>
                                            </crosshairoptions>
													<tooltipoptions>
                                                <tooltippositionserializable><cc2:ToolTipMousePosition /></tooltippositionserializable>
                                            </tooltipoptions>
												</dxchartsui:WebChartControl>
											</dx:PanelContent>
										</PanelCollection>
									</dx:ASPxRoundPanel>
								</ContentTemplate>
								<Triggers>
									<asp:AsyncPostBackTrigger ControlID="Sametimegrid" />
								</Triggers>
							</asp:UpdatePanel>
						</td>
					</tr>

                                </table>
                            </dx:ContentControl>
                        </ContentCollection>
                    </dx:TabPage>
				    <dx:TabPage Text="Conferences">
                        <TabImage Url="~/images/icons/information.png">
                        </TabImage>
                        <ContentCollection>
                            <dx:Contentcontrol ID="ContentControl3" runat="server" SupportsDisabledAttribute="True">
                                <table width="100%">
								<tr>
						       <td colspan="2" align="center" style="text-align: center">
							   <dx:ASPxLabel ID="ASPxLabel6" runat="server" Font-Bold="true" Visible="false">
							   </dx:ASPxLabel>
						       </td>
					           </tr>
                                  <tr>
					<td >
							<asp:UpdatePanel ID="UpdatePanel23" runat="server" UpdateMode="Conditional">
								<ContentTemplate>
									<dx:ASPxRoundPanel ID="ASPxRoundPanel9" runat="server" HeaderText=" Total count of all 1x1 calls " Theme="Glass" Width="100%">
										<PanelCollection>
											<dx:PanelContent ID="PanelContent15" runat="server" SupportsDisabledAttribute="True">
												<dxchartsui:WebChartControl ID="Totalcountofall1x1calls" runat="server" ClientInstanceName="Totalcountofall1x1callsWebChartControl"
													Height="300px" Width="550px" OnBoundDataChanged="Totalcountofall1x1calls_BoundDataChanged">
													<fillstyle>
                                                <optionsserializable><cc2:SolidFillOptions /></optionsserializable>
                                            </fillstyle>
													<seriestemplate>
                                                <viewserializable><cc2:LineSeriesView></cc2:LineSeriesView></viewserializable>
                                                <labelserializable><cc2:PointSeriesLabel LineVisible="True"><fillstyle><optionsserializable><cc2:SolidFillOptions /></optionsserializable></fillstyle><pointoptionsserializable><cc2:PointOptions></cc2:PointOptions></pointoptionsserializable></cc2:PointSeriesLabel></labelserializable>
                                                <legendpointoptionsserializable><cc2:PointOptions></cc2:PointOptions></legendpointoptionsserializable>
                                            </seriestemplate>
													<crosshairoptions>
                                                <commonlabelpositionserializable><cc2:CrosshairMousePosition /></commonlabelpositionserializable>
                                            </crosshairoptions>
													<tooltipoptions>
                                                <tooltippositionserializable><cc2:ToolTipMousePosition /></tooltippositionserializable>
                                            </tooltipoptions>
												</dxchartsui:WebChartControl>
											</dx:PanelContent>
										</PanelCollection>
									</dx:ASPxRoundPanel>
								</ContentTemplate>
								<Triggers>
									<asp:AsyncPostBackTrigger ControlID="Sametimegrid" />
								</Triggers>
							</asp:UpdatePanel>
						</td>
                    <td>
							<asp:UpdatePanel ID="UpdatePanel24" runat="server" UpdateMode="Conditional">
								<ContentTemplate>
									<dx:ASPxRoundPanel ID="ASPxRoundPanel10" runat="server" HeaderText="Total count of all calls" Theme="Glass" Width="100%">
										<PanelCollection>
											<dx:PanelContent ID="PanelContent16" runat="server" SupportsDisabledAttribute="True">
												<dxchartsui:WebChartControl ID="Totalcountofallcalls" runat="server" ClientInstanceName="TotalcountofallcallsWebChartControl"
													Height="300px" Width="550px" OnBoundDataChanged="Totalcountofallcalls_BoundDataChanged">
													<fillstyle>
                                                <optionsserializable><cc2:SolidFillOptions /></optionsserializable>
                                            </fillstyle>
													<seriestemplate>
                                                <viewserializable><cc2:LineSeriesView></cc2:LineSeriesView></viewserializable>
                                                <labelserializable><cc2:PointSeriesLabel LineVisible="True"><fillstyle><optionsserializable><cc2:SolidFillOptions /></optionsserializable></fillstyle><pointoptionsserializable><cc2:PointOptions></cc2:PointOptions></pointoptionsserializable></cc2:PointSeriesLabel></labelserializable>
                                                <legendpointoptionsserializable><cc2:PointOptions></cc2:PointOptions></legendpointoptionsserializable>
                                            </seriestemplate>
													<crosshairoptions>
                                                <commonlabelpositionserializable><cc2:CrosshairMousePosition /></commonlabelpositionserializable>
                                            </crosshairoptions>
													<tooltipoptions>
                                                <tooltippositionserializable><cc2:ToolTipMousePosition /></tooltippositionserializable>
                                            </tooltipoptions>
												</dxchartsui:WebChartControl>
											</dx:PanelContent>
										</PanelCollection>
									</dx:ASPxRoundPanel>
								</ContentTemplate>
								<Triggers>
									<asp:AsyncPostBackTrigger ControlID="Sametimegrid" />
								</Triggers>
							</asp:UpdatePanel>
						</td>
					</tr>
					<tr>
					<td>
							<asp:UpdatePanel ID="UpdatePanel25" runat="server" UpdateMode="Conditional">
								<ContentTemplate>
									<dx:ASPxRoundPanel ID="ASPxRoundPanel11" runat="server" HeaderText="Total count of all multi-user calls" Theme="Glass" Width="100%">
										<PanelCollection>
											<dx:PanelContent ID="PanelContent17" runat="server" SupportsDisabledAttribute="True">
												<dxchartsui:WebChartControl ID="Totalcountofallmultiusercalls" runat="server" ClientInstanceName="totalcountofallmultiusercallsWebChartControl"
													Height="300px" Width="550px" OnBoundDataChanged="Totalcountofallmultiusercalls_BoundDataChanged">
													<fillstyle>
                                                <optionsserializable><cc2:SolidFillOptions /></optionsserializable>
                                            </fillstyle>
													<seriestemplate>
                                                <viewserializable><cc2:LineSeriesView></cc2:LineSeriesView></viewserializable>
                                                <labelserializable><cc2:PointSeriesLabel LineVisible="True"><fillstyle><optionsserializable><cc2:SolidFillOptions /></optionsserializable></fillstyle><pointoptionsserializable><cc2:PointOptions></cc2:PointOptions></pointoptionsserializable></cc2:PointSeriesLabel></labelserializable>
                                                <legendpointoptionsserializable><cc2:PointOptions></cc2:PointOptions></legendpointoptionsserializable>
                                            </seriestemplate>
													<crosshairoptions>
                                                <commonlabelpositionserializable><cc2:CrosshairMousePosition /></commonlabelpositionserializable>
                                            </crosshairoptions>
													<tooltipoptions>
                                                <tooltippositionserializable><cc2:ToolTipMousePosition /></tooltippositionserializable>
                                            </tooltipoptions>
												</dxchartsui:WebChartControl>
											</dx:PanelContent>
										</PanelCollection>
									</dx:ASPxRoundPanel>
								</ContentTemplate>
								<Triggers>
									<asp:AsyncPostBackTrigger ControlID="Sametimegrid" />
								</Triggers>
							</asp:UpdatePanel>
						</td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel26" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <dx:ASPxRoundPanel ID="ASPxRoundPanel12" HeaderText="Count of all calls /Count of all users "  runat="server"
                                                    Theme="Glass" Width="100%">
                                                    <PanelCollection>
                                                        <dx:PanelContent>
                                                            <dxchartsui:WebChartControl ID="Countofallcallsandusers" ClientInstanceName="ctransactionPerMinuteWebChart"
                                                                runat="server" Height="300px" Width="550px" 
                                                                OnCustomCallback="CountofallcallsandUsersChartControl_CustomCallback" 
                                                                OnBoundDataChanged="Countofallcallsandusers_BoundDataChanged">
                                                                <diagramserializable>
                                                            <cc1:XYDiagram LabelsResolveOverlappingMinIndent="1" PaneLayoutDirection="Horizontal">
                                                                <AxisX Title-Text="Date" VisibleInPanesSerializable="-1" 
                                                                    GridSpacingAuto="False" GridSpacing="1.5" MinorCount="15" 
                                                                    DateTimeMeasureUnit="Minute" datetimegridalignment="Hour"><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" />
                                                                    <tickmarks minorvisible="False" />
                                                                    <label><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False"></resolveoverlappingoptions></label><Range SideMarginsEnabled="True"></Range><datetimeoptions format="Custom" /><datetimeoptions format="ShortTime" /><datetimeoptions format="ShortTime" /><datetimeoptions format="ShortTime" /><datetimeoptions format="ShortTime" /><datetimeoptions format="ShortTime" /><datetimeoptions format="Custom" /><datetimeoptions format="ShortTime" /><datetimeoptions format="ShortTime" /><datetimeoptions format="ShortTime" /><datetimeoptions format="ShortTime" /><datetimeoptions format="ShortTime" /><visualrange autosidemargins="True" /><wholerange autosidemargins="True" /><numericscaleoptions autogrid="False" gridspacing="1.5" />
                                                                    <visualrange autosidemargins="True" />
                                                                    <wholerange autosidemargins="True" />
                                                                    <numericscaleoptions autogrid="False" gridspacing="1.5" />
                                                                    <datetimescaleoptions autogrid="False" gridalignment="Hour" gridspacing="1.5" 
                                                                        measureunit="Minute"></datetimescaleoptions></AxisX>
                                                                <AxisY Title-Text="" Title-Visible="True" VisibleInPanesSerializable="-1"><Range AlwaysShowZeroLevel="False" SideMarginsEnabled="True"></Range><visualrange autosidemargins="True" /><wholerange alwaysshowzerolevel="False" autosidemargins="True" />
                                                                    <visualrange autosidemargins="True" />
                                                                    <wholerange alwaysshowzerolevel="False" autosidemargins="True" />
                                                                </AxisY>
                                                            </cc1:XYDiagram>
                                                        </diagramserializable>
                                                                <fillstyle><OptionsSerializable><cc1:SolidFillOptions></cc1:SolidFillOptions></OptionsSerializable>
</fillstyle>
                                                                <seriesserializable>
                                                        <cc1:Series ArgumentScaleType="DateTime" Name="Series 1">
                                                            <viewserializable><cc1:LineSeriesView></cc1:LineSeriesView></viewserializable>
                                                            <labelserializable><cc1:PointSeriesLabel LineVisible="True"><fillstyle><optionsserializable><cc1:SolidFillOptions /></optionsserializable></fillstyle><pointoptionsserializable><cc1:PointOptions></cc1:PointOptions></pointoptionsserializable></cc1:PointSeriesLabel></labelserializable>
                                                            <legendpointoptionsserializable><cc1:PointOptions></cc1:PointOptions></legendpointoptionsserializable>
                                                        </cc1:Series>
                                                        <cc1:Series ArgumentScaleType="DateTime" Name="Series 2">
                                                            <viewserializable><cc1:LineSeriesView></cc1:LineSeriesView></viewserializable>
                                                            <labelserializable><cc1:PointSeriesLabel LineVisible="True"><fillstyle><optionsserializable><cc1:SolidFillOptions /></optionsserializable></fillstyle><pointoptionsserializable><cc1:PointOptions></cc1:PointOptions></pointoptionsserializable></cc1:PointSeriesLabel></labelserializable>
                                                            <legendpointoptionsserializable><cc1:PointOptions></cc1:PointOptions></legendpointoptionsserializable>
                                                        </cc1:Series>
                                                    </seriesserializable>
                                                                <seriestemplate argumentscaletype="DateTime"><ViewSerializable><cc1:LineSeriesView></cc1:LineSeriesView></ViewSerializable>
<LabelSerializable><cc1:PointSeriesLabel LineVisible="True"><fillstyle><optionsserializable><cc1:SolidFillOptions /></optionsserializable></fillstyle><pointoptionsserializable><cc1:PointOptions></cc1:PointOptions></pointoptionsserializable></cc1:PointSeriesLabel></LabelSerializable>
<LegendPointOptionsSerializable><cc1:PointOptions></cc1:PointOptions></LegendPointOptionsSerializable>
</seriestemplate>
                                                                <crosshairoptions><CommonLabelPositionSerializable><cc1:CrosshairMousePosition></cc1:CrosshairMousePosition></CommonLabelPositionSerializable>
</crosshairoptions>
                                                                <tooltipoptions><ToolTipPositionSerializable><cc1:ToolTipMousePosition></cc1:ToolTipMousePosition></ToolTipPositionSerializable>
</tooltipoptions>
                                                            </dxchartsui:WebChartControl>
                                                        </dx:PanelContent>
                                                    </PanelCollection>
                                                </dx:ASPxRoundPanel>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                        </td>
					</tr>
		    		<tr>
					<td>
                                        <asp:UpdatePanel ID="UpdatePanel27" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <dx:ASPxRoundPanel ID="ASPxRoundPanel13" HeaderText="Count of all 1x1 calls  /Count of all 1x1 call users" runat="server"
                                                    Theme="Glass" Width="100%">
                                                    <PanelCollection>
                                                        <dx:PanelContent>
                                                            <dxchartsui:WebChartControl ID="Countofall1x1callsandusers" ClientInstanceName="Countofall1x1callsandusers"
                                                                runat="server" Height="300px" Width="550px" 
                                                                OnCustomCallback="Countofall1x1callsandusers_CustomCallback" 
                                                                OnBoundDataChanged="Countofall1x1callsandusers_BoundDataChanged">
                                                                <diagramserializable>
                                                            <cc1:XYDiagram LabelsResolveOverlappingMinIndent="1" PaneLayoutDirection="Horizontal">
                                                                <AxisX Title-Text="Date" VisibleInPanesSerializable="-1" 
                                                                    GridSpacingAuto="False" GridSpacing="1.5" MinorCount="15" 
                                                                    DateTimeMeasureUnit="Minute" datetimegridalignment="Hour"><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" />
                                                                    <tickmarks minorvisible="False" />
                                                                    <label><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False"></resolveoverlappingoptions></label><Range SideMarginsEnabled="True"></Range><datetimeoptions format="Custom" /><datetimeoptions format="ShortTime" /><datetimeoptions format="ShortTime" /><datetimeoptions format="ShortTime" /><datetimeoptions format="ShortTime" /><datetimeoptions format="ShortTime" /><datetimeoptions format="Custom" /><datetimeoptions format="ShortTime" /><datetimeoptions format="ShortTime" /><datetimeoptions format="ShortTime" /><datetimeoptions format="ShortTime" /><datetimeoptions format="ShortTime" /><visualrange autosidemargins="True" /><wholerange autosidemargins="True" /><numericscaleoptions autogrid="False" gridspacing="1.5" />
                                                                    <visualrange autosidemargins="True" />
                                                                    <wholerange autosidemargins="True" />
                                                                    <numericscaleoptions autogrid="False" gridspacing="1.5" />
                                                                    <datetimescaleoptions autogrid="False" gridalignment="Hour" gridspacing="1.5" 
                                                                        measureunit="Minute"></datetimescaleoptions></AxisX>
                                                                <AxisY Title-Text="" Title-Visible="True" VisibleInPanesSerializable="-1"><Range AlwaysShowZeroLevel="False" SideMarginsEnabled="True"></Range><visualrange autosidemargins="True" /><wholerange alwaysshowzerolevel="False" autosidemargins="True" />
                                                                    <visualrange autosidemargins="True" />
                                                                    <wholerange alwaysshowzerolevel="False" autosidemargins="True" />
                                                                </AxisY>
                                                            </cc1:XYDiagram>
                                                        </diagramserializable>
                                                                <fillstyle><OptionsSerializable><cc1:SolidFillOptions></cc1:SolidFillOptions></OptionsSerializable>
</fillstyle>
                                                                <seriesserializable>
                                                        <cc1:Series ArgumentScaleType="DateTime" Name="Series 1">
                                                            <viewserializable><cc1:LineSeriesView></cc1:LineSeriesView></viewserializable>
                                                            <labelserializable><cc1:PointSeriesLabel LineVisible="True"><fillstyle><optionsserializable><cc1:SolidFillOptions /></optionsserializable></fillstyle><pointoptionsserializable><cc1:PointOptions></cc1:PointOptions></pointoptionsserializable></cc1:PointSeriesLabel></labelserializable>
                                                            <legendpointoptionsserializable><cc1:PointOptions></cc1:PointOptions></legendpointoptionsserializable>
                                                        </cc1:Series>
                                                        <cc1:Series ArgumentScaleType="DateTime" Name="Series 2">
                                                            <viewserializable><cc1:LineSeriesView></cc1:LineSeriesView></viewserializable>
                                                            <labelserializable><cc1:PointSeriesLabel LineVisible="True"><fillstyle><optionsserializable><cc1:SolidFillOptions /></optionsserializable></fillstyle><pointoptionsserializable><cc1:PointOptions></cc1:PointOptions></pointoptionsserializable></cc1:PointSeriesLabel></labelserializable>
                                                            <legendpointoptionsserializable><cc1:PointOptions></cc1:PointOptions></legendpointoptionsserializable>
                                                        </cc1:Series>
                                                    </seriesserializable>
                                                                <seriestemplate argumentscaletype="DateTime"><ViewSerializable><cc1:LineSeriesView></cc1:LineSeriesView></ViewSerializable>
<LabelSerializable><cc1:PointSeriesLabel LineVisible="True"><fillstyle><optionsserializable><cc1:SolidFillOptions /></optionsserializable></fillstyle><pointoptionsserializable><cc1:PointOptions></cc1:PointOptions></pointoptionsserializable></cc1:PointSeriesLabel></LabelSerializable>
<LegendPointOptionsSerializable><cc1:PointOptions></cc1:PointOptions></LegendPointOptionsSerializable>
</seriestemplate>
                                                                <crosshairoptions><CommonLabelPositionSerializable><cc1:CrosshairMousePosition></cc1:CrosshairMousePosition></CommonLabelPositionSerializable>
</crosshairoptions>
                                                                <tooltipoptions><ToolTipPositionSerializable><cc1:ToolTipMousePosition></cc1:ToolTipMousePosition></ToolTipPositionSerializable>
</tooltipoptions>
                                                            </dxchartsui:WebChartControl>
                                                        </dx:PanelContent>
                                                    </PanelCollection>
                                                </dx:ASPxRoundPanel>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="UpdatePanel28" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <dx:ASPxRoundPanel ID="ASPxRoundPanel14" HeaderText="Count of all multi-user calls/count of all multi-user call users " runat="server"
                                                    Theme="Glass" Width="100%">
                                                    <PanelCollection>
                                                        <dx:PanelContent>
                                                            <dxchartsui:WebChartControl ID="countofallmultiusercallsandusers" ClientInstanceName="countofallmultiusercallsandusers"
                                                                runat="server" Height="300px" Width="550px" 
                                                                OnCustomCallback="countofallmultiusercallsandusers_CustomCallback" 
                                                                OnBoundDataChanged="countofallmultiusercallsandusers_BoundDataChanged">
                                                                <diagramserializable>
                                                            <cc1:XYDiagram LabelsResolveOverlappingMinIndent="1" PaneLayoutDirection="Horizontal">
                                                                <AxisX Title-Text="Date" VisibleInPanesSerializable="-1" 
                                                                    GridSpacingAuto="False" GridSpacing="1.5" MinorCount="15" 
                                                                    DateTimeMeasureUnit="Minute" datetimegridalignment="Hour"><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" />
                                                                    <tickmarks minorvisible="False" />
                                                                    <label><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False"></resolveoverlappingoptions></label><Range SideMarginsEnabled="True"></Range><datetimeoptions format="Custom" /><datetimeoptions format="ShortTime" /><datetimeoptions format="ShortTime" /><datetimeoptions format="ShortTime" /><datetimeoptions format="ShortTime" /><datetimeoptions format="ShortTime" /><datetimeoptions format="Custom" /><datetimeoptions format="ShortTime" /><datetimeoptions format="ShortTime" /><datetimeoptions format="ShortTime" /><datetimeoptions format="ShortTime" /><datetimeoptions format="ShortTime" /><visualrange autosidemargins="True" /><wholerange autosidemargins="True" /><numericscaleoptions autogrid="False" gridspacing="1.5" />
                                                                    <visualrange autosidemargins="True" />
                                                                    <wholerange autosidemargins="True" />
                                                                    <numericscaleoptions autogrid="False" gridspacing="1.5" />
                                                                    <datetimescaleoptions autogrid="False" gridalignment="Hour" gridspacing="1.5" 
                                                                        measureunit="Minute"></datetimescaleoptions></AxisX>
                                                                <AxisY Title-Text="" Title-Visible="True" VisibleInPanesSerializable="-1"><Range AlwaysShowZeroLevel="False" SideMarginsEnabled="True"></Range><visualrange autosidemargins="True" /><wholerange alwaysshowzerolevel="False" autosidemargins="True" />
                                                                    <visualrange autosidemargins="True" />
                                                                    <wholerange alwaysshowzerolevel="False" autosidemargins="True" />
                                                                </AxisY>
                                                            </cc1:XYDiagram>
                                                        </diagramserializable>
                                                                <fillstyle><OptionsSerializable><cc1:SolidFillOptions></cc1:SolidFillOptions></OptionsSerializable>
</fillstyle>
                                                                <seriesserializable>
                                                        <cc1:Series ArgumentScaleType="DateTime" Name="Series 1">
                                                            <viewserializable><cc1:LineSeriesView></cc1:LineSeriesView></viewserializable>
                                                            <labelserializable><cc1:PointSeriesLabel LineVisible="True"><fillstyle><optionsserializable><cc1:SolidFillOptions /></optionsserializable></fillstyle><pointoptionsserializable><cc1:PointOptions></cc1:PointOptions></pointoptionsserializable></cc1:PointSeriesLabel></labelserializable>
                                                            <legendpointoptionsserializable><cc1:PointOptions></cc1:PointOptions></legendpointoptionsserializable>
                                                        </cc1:Series>
                                                        <cc1:Series ArgumentScaleType="DateTime" Name="Series 2">
                                                            <viewserializable><cc1:LineSeriesView></cc1:LineSeriesView></viewserializable>
                                                            <labelserializable><cc1:PointSeriesLabel LineVisible="True"><fillstyle><optionsserializable><cc1:SolidFillOptions /></optionsserializable></fillstyle><pointoptionsserializable><cc1:PointOptions></cc1:PointOptions></pointoptionsserializable></cc1:PointSeriesLabel></labelserializable>
                                                            <legendpointoptionsserializable><cc1:PointOptions></cc1:PointOptions></legendpointoptionsserializable>
                                                        </cc1:Series>
                                                    </seriesserializable>
                                                                <seriestemplate argumentscaletype="DateTime"><ViewSerializable><cc1:LineSeriesView></cc1:LineSeriesView></ViewSerializable>
<LabelSerializable><cc1:PointSeriesLabel LineVisible="True"><fillstyle><optionsserializable><cc1:SolidFillOptions /></optionsserializable></fillstyle><pointoptionsserializable><cc1:PointOptions></cc1:PointOptions></pointoptionsserializable></cc1:PointSeriesLabel></LabelSerializable>
<LegendPointOptionsSerializable><cc1:PointOptions></cc1:PointOptions></LegendPointOptionsSerializable>
</seriestemplate>
                                                                <crosshairoptions><CommonLabelPositionSerializable><cc1:CrosshairMousePosition></cc1:CrosshairMousePosition></CommonLabelPositionSerializable>
</crosshairoptions>
                                                                <tooltipoptions><ToolTipPositionSerializable><cc1:ToolTipMousePosition></cc1:ToolTipMousePosition></ToolTipPositionSerializable>
</tooltipoptions>
                                                            </dxchartsui:WebChartControl>
                                                        </dx:PanelContent>
                                                    </PanelCollection>
                                                </dx:ASPxRoundPanel>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
					</tr>
					
                                </table>
                            </dx:ContentControl>
                        </ContentCollection>
                    </dx:TabPage>
					<dx:TabPage Text="Meetings">
                        <TabImage Url="~/images/icons/information.png">
                        </TabImage>
                        <ContentCollection>
                            <dx:Contentcontrol ID="ContentControl4" runat="server" SupportsDisabledAttribute="True">
                                <table width="100%">
								<tr>
						       <td align="center" style="text-align: center">
							   <dx:ASPxLabel ID="ASPxLabel7" runat="server" Font-Bold="true" Visible="false">
							   </dx:ASPxLabel>
						       </td>
					           </tr>
                                 	<tr>
					<td>
                                        <asp:UpdatePanel ID="UpdatePanel29" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <dx:ASPxRoundPanel ID="ASPxRoundPanel15" HeaderText="Number of active meetings /Current number of users inside meetings " runat="server"
                                                    Theme="Glass" Width="100%">
                                                    <PanelCollection>
                                                        <dx:PanelContent>
                                                            <dxchartsui:WebChartControl ID="Numberofactivemeetingsandusersinsidemeetings" ClientInstanceName="Numberofactivemeetingsandusersinsidemeetings"
                                                                runat="server" Height="300px" Width="1030px" 
                                                                OnCustomCallback="Numberofactivemeetingsandusersinsidemeetings_CustomCallback" 
                                                                OnBoundDataChanged="Numberofactivemeetingsandusersinsidemeetings_BoundDataChanged">
                                                                <diagramserializable>
                                                            <cc1:XYDiagram LabelsResolveOverlappingMinIndent="1" PaneLayoutDirection="Horizontal">
                                                                <AxisX Title-Text="Date" VisibleInPanesSerializable="-1" 
                                                                    GridSpacingAuto="False" GridSpacing="1.5" MinorCount="15" 
                                                                    DateTimeMeasureUnit="Minute" datetimegridalignment="Hour"><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><label><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False"></resolveoverlappingoptions></label><Range SideMarginsEnabled="True"></Range><datetimeoptions format="Custom" /><datetimeoptions format="ShortTime" /><datetimeoptions format="ShortTime" /><datetimeoptions format="ShortTime" /><datetimeoptions format="ShortTime" /><datetimeoptions format="ShortTime" /><datetimeoptions format="Custom" /><datetimeoptions format="ShortTime" /><datetimeoptions format="ShortTime" /><datetimeoptions format="ShortTime" /><datetimeoptions format="ShortTime" /><datetimeoptions format="ShortTime" /><visualrange autosidemargins="True" /><wholerange autosidemargins="True" /><numericscaleoptions autogrid="False" gridspacing="1.5" /><datetimescaleoptions autogrid="False" gridalignment="Hour" gridspacing="1.5" 
                                                                        measureunit="Minute"></datetimescaleoptions></AxisX>
                                                                <AxisY Title-Text="" Title-Visible="True" VisibleInPanesSerializable="-1"><Range AlwaysShowZeroLevel="False" SideMarginsEnabled="True"></Range><visualrange autosidemargins="True" /><wholerange alwaysshowzerolevel="False" autosidemargins="True" /></AxisY>
                                                            </cc1:XYDiagram>
                                                        </diagramserializable>
                                                                <fillstyle><OptionsSerializable><cc1:SolidFillOptions></cc1:SolidFillOptions></OptionsSerializable>
</fillstyle>
                                                                <seriesserializable>
                                                        <cc1:Series ArgumentScaleType="DateTime" Name="Series 1">
                                                            <viewserializable><cc1:LineSeriesView></cc1:LineSeriesView></viewserializable>
                                                            <labelserializable><cc1:PointSeriesLabel LineVisible="True"><fillstyle><optionsserializable><cc1:SolidFillOptions /></optionsserializable></fillstyle><pointoptionsserializable><cc1:PointOptions></cc1:PointOptions></pointoptionsserializable></cc1:PointSeriesLabel></labelserializable>
                                                            <legendpointoptionsserializable><cc1:PointOptions></cc1:PointOptions></legendpointoptionsserializable>
                                                        </cc1:Series>
                                                        <cc1:Series ArgumentScaleType="DateTime" Name="Series 2">
                                                            <viewserializable><cc1:LineSeriesView></cc1:LineSeriesView></viewserializable>
                                                            <labelserializable><cc1:PointSeriesLabel LineVisible="True"><fillstyle><optionsserializable><cc1:SolidFillOptions /></optionsserializable></fillstyle><pointoptionsserializable><cc1:PointOptions></cc1:PointOptions></pointoptionsserializable></cc1:PointSeriesLabel></labelserializable>
                                                            <legendpointoptionsserializable><cc1:PointOptions></cc1:PointOptions></legendpointoptionsserializable>
                                                        </cc1:Series>
                                                    </seriesserializable>
                                                                <seriestemplate argumentscaletype="DateTime"><ViewSerializable><cc1:LineSeriesView></cc1:LineSeriesView></ViewSerializable>
<LabelSerializable><cc1:PointSeriesLabel LineVisible="True"><fillstyle><optionsserializable><cc1:SolidFillOptions /></optionsserializable></fillstyle><pointoptionsserializable><cc1:PointOptions></cc1:PointOptions></pointoptionsserializable></cc1:PointSeriesLabel></LabelSerializable>
<LegendPointOptionsSerializable><cc1:PointOptions></cc1:PointOptions></LegendPointOptionsSerializable>
</seriestemplate>
                                                                <crosshairoptions><CommonLabelPositionSerializable><cc1:CrosshairMousePosition></cc1:CrosshairMousePosition></CommonLabelPositionSerializable>
</crosshairoptions>
                                                                <tooltipoptions><ToolTipPositionSerializable><cc1:ToolTipMousePosition></cc1:ToolTipMousePosition></ToolTipPositionSerializable>
</tooltipoptions>
                                                            </dxchartsui:WebChartControl>
                                                        </dx:PanelContent>
                                                    </PanelCollection>
                                                </dx:ASPxRoundPanel>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
					</tr>
                                </table>
                            </dx:ContentControl>
                        </ContentCollection>
                    </dx:TabPage>

					</TabPages>
            </dx:ASPxPageControl>
        </ContentTemplate>
    </asp:UpdatePanel>
	</td>
		</tr>


	</table>
</asp:Content>
