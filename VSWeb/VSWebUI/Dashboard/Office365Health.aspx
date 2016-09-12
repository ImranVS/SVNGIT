<%@ Page Title="VitalSigns Plus - Office 365 Health" Language="C#" MasterPageFile="~/DashboardSite.Master"
	AutoEventWireup="true" CodeBehind="Office365Health.aspx.cs" Inherits="VSWebUI.Dashboard.Office365Health" %>

<%@ MasterType VirtualPath="~/DashboardSite.Master" %>
<%@ Register Assembly="DevExpress.XtraCharts.v14.2.Web, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.XtraCharts.Web" TagPrefix="dxchartsui" %>
<%@ Register Assembly="DevExpress.XtraCharts.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.XtraCharts" TagPrefix="cc2" %>
<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.XtraCharts.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.XtraCharts" TagPrefix="cc1" %>
	<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="DevExpress.XtraCharts.v14.2.Web, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.XtraCharts.Web" TagPrefix="dxchartsui" %>
<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.Web.ASPxPivotGrid" TagPrefix="dx" %>
<%@ Register Src="~/Controls/DateRange.ascx" TagName="DateRange" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
	<style type="text/css">
		
	</style>
	<script type="text/javascript">



		function officehealthgrid_ContextMenu(s, e) {
			if (e.objectType == "row") {
				s.SetFocusedRowIndex(e.index);
				StatusListPopup.ShowAtPos(ASPxClientUtils.GetEventX(e.htmlEvent), ASPxClientUtils.GetEventY(e.htmlEvent));
			}
		}
		function officehealthgrid_FocusedRowChanged(s, e) {

			if (e.objectType != "row") return;
			//alert(e.index);
			s.SetFocusedRowIndex(e.index);
		}

		function InitPopupMenuHandler(s, e) {

			var gridCell = document.getElementById('gridCell');
			ASPxClientUtils.AttachEventToElement(gridCell, 'contextmenu', OnGridContextMenu);
		}

		function OnGridContextMenu(evt) {
			DeviceGridView.SetFocusedRowIndex(e.index);
			var SortPopupMenu = StatusListPopup;
			SortPopupMenu.ShowAtPos(evt.clientX + ASPxClientUtils.GetDocumentScrollLeft(), evt.clientY + ASPxClientUtils.GetDocumentScrollTop());
			return OnPreventContextMenu(evt);
		}
		function OnPreventContextMenu(evt) {
			return ASPxClientUtils.PreventEventAndBubble(evt);
		}


		function DeviceGridView_ContextMenu(s, e) {
			if (e.objectType == "row") {
				s.SetFocusedRowIndex(e.index);
				StatusListPopup.ShowAtPos(ASPxClientUtils.GetEventX(e.htmlEvent), ASPxClientUtils.GetEventY(e.htmlEvent));
			}
		}
		function DeviceGridView_FocusedRowChanged(s, e) {
			if (e.objectType != "row") return;
			s.SetFocusedRowIndex(e.index);
		}
		function OnItemClick(s, e) {
			if (e.item.parent == s.GetRootItem())
				e.processOnServer = false;
		}

		function Resized() {
			var callbackState = document.getElementById('ContentPlaceHolder1_callbackState').value;

			if (callbackState == 0)
				DoCallback();
		}

		function DoCallback() {
			/*
			document.getElementById('ContentPlaceHolder1_chartWidth').value = Math.round(document.body.offsetWidth / 2) - 50;
			var chartwidth = document.getElementById('ContentPlaceHolder1_chartWidth').value;
			cDevicesWebChart.PerformCallback();
			cMailTestsWebChart.PerformCallback();
			//			cDirSyncWebChart.PerformCallback();
			cUserScenarioWebChart.PerformCallback();
			

			cOneDriveWebChart.PerformCallback();
			cSiteTestsWebChart.PerformCallback();
			cTaskFolderTestsWebChart.PerformCallback();
			DeviceTypeChart.PerformCallback();
			DeviceTypeChart0.PerformCallback();
			ConfReportChart.PerformCallback();
			P2PSessionsChart1.PerformCallback();
			SyncTypeChart.PerformCallback();
			*/
		}
		sessionStorage.setItem("Force refresh", "True");
	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
	<input id="chartWidth" type="hidden" runat="server" value="200" />
	<input id="callbackState" type="hidden" runat="server" value="0" />
	<table width="100%">
		<tr>
			<td valign="top">
				<table width="100%">
					<tr>
						<td valign="top">
							<div id="NameHolder">
								<%--<table width="100%"><tr><td>
										<asp:ListView ID="DevicesListView" CssFilePath="~/App_Themes/Office2010Silver/{0}/styles.css"
											CssPostfix="Office2010Silver" Theme="Office2003Blue" runat="server">
											<ItemTemplate>
												<tr id="Tr2" runat="server">
													<td>
														<asp:Label ID="FirstNameLabel" runat="Server" Text='<%#Eval("Stat") %>' />
													</td>
													<td valign="top">
														<asp:Label ID="LastNameLabel" runat="Server" Text='<%#Eval("Value") %>' />
													</td>
												</tr>
												<tr height="1" bgcolor="black">
													<td colspan=2>
														<div style="background-color: #000000"></div>
													</td>
												</tr>
											</ItemTemplate>
											<AlternatingItemTemplate>
												<tr id="Tr2" runat="server">
													<td>
														<asp:Label ID="FirstNameLabel" runat="Server" Text='<%#Eval("Stat") %>' />
													</td>
													<td valign="top">
														<asp:Label ID="LastNameLabel" runat="Server" Text='<%#Eval("Value") %>' />
													</td>
												</tr>
												<tr height="1" bgcolor="black">
													<td colspan=2>
														<div style="background-color: #000000"></div>
													</td>
												</tr>
											</AlternatingItemTemplate>
										</asp:ListView>
										</td></tr></table>--%>
								<asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
									<ContentTemplate>
										<dx:ASPxLabel ID="servernamelbldisp" runat="server" CssClass="header" Text="Microsoft Office 365 Health">
										</dx:ASPxLabel>
									</ContentTemplate>
									<Triggers>
										<asp:AsyncPostBackTrigger ControlID="officehealthgrid" />
									</Triggers>
								</asp:UpdatePanel>
							</div>
						</td>
					</tr>
				</table>
			</td>
			<td>
				&nbsp;&nbsp;
			</td>
			<td valign="top" align="right">
				<table>
					<tr>
						<td align="right">
							<dx:ASPxMenu ID="ASPxMenu1" runat="server" ShowAsToolbar="True" OnInit="ASPxMenu1_Init"
								Theme="Moderno" HorizontalAlign="Right">
								<ClientSideEvents ItemClick="OnItemClick" />
								<Items>
									<dx:MenuItem BeginGroup="True" Name="EditConfigItem2" Text="">
										<Items>
										</Items>
										<Image Url="~/images/icons/Gear.png">
										</Image>
									</dx:MenuItem>
								</Items>
							</dx:ASPxMenu>
						</td>
					</tr>
				</table>
			</td>
		</tr>
		<tr>
			<td colspan="3">
				<dx:ASPxGridView ID="officehealthgrid" runat="server" AutoGenerateColumns="False"
					EnableTheming="True" Theme="Office2003Blue" OnHtmlRowCreated="officehealthgrid_HtmlRowCreated"
					OnHtmlDataCellPrepared="officehealthgrid_HtmlDataCellPrepared" Width="100%" OnPageSizeChanged="officehealthgrid_OnPageSizeChanged"
					OnDataBound="officehealthgrid_DataBound" EnableCallBacks="False">
					<Columns>
						<dx:GridViewDataTextColumn Caption="Id" FieldName="Id" Visible="false" VisibleIndex="0">
						</dx:GridViewDataTextColumn>
						<dx:GridViewDataTextColumn Caption="Account Name" FieldName="AccountName" VisibleIndex="1">
							<HeaderStyle CssClass="GridCssHeader" />
							<CellStyle CssClass="GridCss">
							</CellStyle>
						</dx:GridViewDataTextColumn>
						<dx:GridViewDataTextColumn Caption="Node Name" FieldName="NodeName" VisibleIndex="2">
							<HeaderStyle CssClass="GridCssHeader" />
							<CellStyle CssClass="GridCss">
							</CellStyle>
						</dx:GridViewDataTextColumn>
						<dx:GridViewDataTextColumn Caption="Status" FieldName="Status" VisibleIndex="3">
							<HeaderStyle CssClass="GridCssHeader" />
							<CellStyle CssClass="GridCss" Wrap="False">
							</CellStyle>
						</dx:GridViewDataTextColumn>
						<dx:GridViewDataTextColumn Caption="Location" FieldName="Location" VisibleIndex="4">
							<HeaderStyle CssClass="GridCssHeader" />
							<CellStyle CssClass="GridCss">
							</CellStyle>
						</dx:GridViewDataTextColumn>
						<dx:GridViewDataDateColumn Caption="Last Scan" FieldName="LastUpdate" VisibleIndex="6">
						</dx:GridViewDataDateColumn>
					</Columns>
					<SettingsBehavior AllowFocusedRow="True" AllowSelectByRowClick="True" ProcessSelectionChangedOnServer="True"
						AllowSelectSingleRowOnly="True" />
					<SettingsPager AlwaysShowPager="True" SEOFriendly="Enabled">
						<PageSizeItemSettings Visible="True">
						</PageSizeItemSettings>
					</SettingsPager>
					<Settings HorizontalScrollBarMode="Auto" />
					<Styles>
						<Header CssClass="GridCssHeader">
						</Header>
						<AlternatingRow CssClass="GridAltRow">
						</AlternatingRow>
						<Cell CssClass="GridCss">
						</Cell>
					</Styles>
				</dx:ASPxGridView>
			</td>
		</tr>
		<tr>
			<td valign="top" colspan="3" width="100%">
				<table>
					<tr>
						<td colspan="2">
						</td>
					</tr>
					<tr>
						<td>
							<uc1:DateRange ID="dtPick" runat="server" Height="100%" Width="100px" Visible="false" />
						</td>
						<td>
							<dx:ASPxButton ID="SubmitButton" runat="server" Text="Submit" CssClass="sysButton"
								Visible="false">
							</dx:ASPxButton>
						</td>
					</tr>
				</table>
				<dx:ASPxPageControl ID="ASPxPageControl1" runat="server" ActiveTabIndex="0" Theme="Glass"
					Width="100%" EnableHierarchyRecreation="False">
					<TabPages>
						<dx:TabPage Text="Overall">
							<ContentCollection>
								<dx:ContentControl runat="server">
									<asp:UpdatePanel ID="UpdatePanel2" runat="server" width="100%" UpdateMode="Conditional">
										<ContentTemplate>
											<table class="navbarTbl" width="100%">
												<tr>
													<td valign="top">
														<dxchartsui:WebChartControl ID="MailTestsWebChart" runat="server" ClientInstanceName="cMailTestsWebChart"
															CrosshairEnabled="True" Height="200px" OnCustomCallback="MailTestsWebChart_CustomCallback"
															PaletteName="Oriel" Width="400px">
															<diagramserializable>
                                                        <cc1:XYDiagram>
                                                            <axisx visibleinpanesserializable="-1" title-text="Time" title-visible="True" 
                                                                title-visibility="Default">
                                                                <label>
                                                                    <datetimeoptions autoformat="False" format="ShortTime" /><datetimeoptions autoformat="False" format="ShortTime" /><datetimeoptions autoformat="False" format="ShortTime" /><datetimeoptions autoformat="False" format="ShortTime" /><resolveoverlappingoptions allowrotate="False" /><datetimeoptions autoformat="False" format="ShortTime" /><resolveoverlappingoptions allowrotate="False" /><datetimeoptions autoformat="False" format="ShortTime" /><resolveoverlappingoptions allowrotate="False" /><datetimeoptions autoformat="False" format="ShortTime" /><resolveoverlappingoptions allowrotate="False" /><datetimeoptions autoformat="False" format="ShortTime" /><resolveoverlappingoptions allowrotate="False" /><datetimeoptions autoformat="False" format="ShortTime" /><resolveoverlappingoptions allowrotate="False" /><datetimeoptions autoformat="False" format="ShortTime" /><resolveoverlappingoptions allowrotate="False" /><datetimeoptions autoformat="False" format="ShortTime" /><resolveoverlappingoptions allowrotate="False" /><datetimeoptions autoformat="False" format="ShortTime" /><resolveoverlappingoptions allowrotate="False" /><datetimeoptions autoformat="False" format="ShortTime" /><resolveoverlappingoptions allowrotate="False" /><datetimeoptions autoformat="False" format="ShortTime" /><resolveoverlappingoptions allowrotate="False" /><datetimeoptions autoformat="False" format="ShortTime" /><resolveoverlappingoptions allowrotate="False" /><datetimeoptions autoformat="False" format="ShortTime" /><resolveoverlappingoptions allowrotate="False" /><datetimeoptions autoformat="False" format="ShortTime" /><resolveoverlappingoptions allowrotate="False" /><datetimeoptions autoformat="False" format="ShortTime" /><resolveoverlappingoptions allowrotate="False" /><datetimeoptions autoformat="False" format="ShortTime" /><resolveoverlappingoptions allowrotate="False" /><datetimeoptions autoformat="False" format="ShortTime" /><resolveoverlappingoptions allowrotate="False" /><datetimeoptions autoformat="False" format="ShortTime" /><resolveoverlappingoptions allowrotate="False" /><datetimeoptions autoformat="False" format="ShortTime" /><resolveoverlappingoptions allowrotate="False" /><datetimeoptions autoformat="False" format="ShortTime" /><resolveoverlappingoptions allowrotate="False" /><datetimeoptions autoformat="False" format="ShortTime" /><resolveoverlappingoptions allowrotate="False" /><datetimeoptions autoformat="False" format="ShortTime" /><resolveoverlappingoptions allowrotate="False" /><datetimeoptions autoformat="False" format="ShortTime" /><resolveoverlappingoptions allowrotate="False" /><datetimeoptions autoformat="False" format="ShortTime" /><resolveoverlappingoptions allowrotate="False" /><datetimeoptions autoformat="False" format="ShortTime" /><resolveoverlappingoptions allowrotate="False" /><datetimeoptions autoformat="False" format="ShortTime" /><resolveoverlappingoptions allowrotate="False" /><datetimeoptions autoformat="False" format="ShortTime" /><resolveoverlappingoptions allowrotate="False" /><datetimeoptions autoformat="False" format="ShortTime" /><resolveoverlappingoptions allowrotate="False" /><datetimeoptions autoformat="False" format="ShortTime" /><resolveoverlappingoptions allowrotate="False" /><datetimeoptions autoformat="False" format="ShortTime" /><resolveoverlappingoptions allowrotate="False" /><datetimeoptions autoformat="False" format="ShortTime" /><resolveoverlappingoptions allowrotate="False" /><datetimeoptions autoformat="False" format="ShortTime" /><resolveoverlappingoptions allowrotate="False" /><datetimeoptions autoformat="False" format="ShortTime" /><resolveoverlappingoptions allowrotate="False" /><datetimeoptions autoformat="False" format="ShortTime" /><resolveoverlappingoptions allowrotate="False" /><datetimeoptions autoformat="False" format="ShortTime" /><resolveoverlappingoptions allowrotate="False" /><datetimeoptions autoformat="False" format="ShortTime" /><resolveoverlappingoptions allowrotate="False" /><datetimeoptions autoformat="False" format="ShortTime" /><resolveoverlappingoptions allowrotate="False" /><datetimeoptions autoformat="False" format="ShortTime" /><resolveoverlappingoptions allowrotate="False" /><datetimeoptions autoformat="False" format="ShortTime" /><resolveoverlappingoptions allowrotate="False" /><datetimeoptions autoformat="False" format="ShortTime" /><resolveoverlappingoptions allowrotate="False" />
                                                                    <datetimeoptions autoformat="False" format="ShortTime" />
<ResolveOverlappingOptions AllowRotate="False"></ResolveOverlappingOptions>

<DateTimeOptions Format="ShortTime" AutoFormat="False"></DateTimeOptions>
                                                                <resolveoverlappingoptions allowrotate="False" /><datetimeoptions autoformat="False" format="ShortTime" /><resolveoverlappingoptions allowrotate="False" /><datetimeoptions autoformat="False" format="ShortTime" /><resolveoverlappingoptions allowrotate="False" /><datetimeoptions autoformat="False" format="ShortTime" /><resolveoverlappingoptions allowrotate="False" /><datetimeoptions autoformat="False" format="ShortTime" /><resolveoverlappingoptions allowrotate="False" /><datetimeoptions autoformat="False" format="ShortTime" /><resolveoverlappingoptions allowrotate="False" /><datetimeoptions autoformat="False" format="ShortTime" /><resolveoverlappingoptions allowrotate="False" /><datetimeoptions autoformat="False" format="ShortTime" /><resolveoverlappingoptions allowrotate="False" /><datetimeoptions autoformat="False" format="ShortTime" /><resolveoverlappingoptions allowrotate="False" /><datetimeoptions autoformat="False" format="ShortTime" /><resolveoverlappingoptions allowrotate="False" /><datetimeoptions autoformat="False" format="ShortTime" /><resolveoverlappingoptions allowrotate="False" /><datetimeoptions autoformat="False" format="ShortTime" /><resolveoverlappingoptions allowrotate="False" /><datetimeoptions autoformat="False" format="ShortTime" /><resolveoverlappingoptions allowrotate="False" /><datetimeoptions autoformat="False" format="ShortTime" /><resolveoverlappingoptions allowrotate="False" /><datetimeoptions autoformat="False" format="ShortTime" /><resolveoverlappingoptions allowrotate="False" /><datetimeoptions autoformat="False" format="ShortTime" /><resolveoverlappingoptions allowrotate="False" /><datetimeoptions autoformat="False" format="ShortTime" /><resolveoverlappingoptions allowrotate="False" /><datetimeoptions autoformat="False" format="ShortTime" /><resolveoverlappingoptions allowrotate="False" /><datetimeoptions autoformat="False" format="ShortTime" /><resolveoverlappingoptions allowrotate="False" /><datetimeoptions autoformat="False" format="ShortTime" /><resolveoverlappingoptions allowrotate="False" /><datetimeoptions autoformat="False" format="ShortTime" /><resolveoverlappingoptions allowrotate="False" /><datetimeoptions autoformat="False" format="ShortTime" /><resolveoverlappingoptions allowrotate="False" /><datetimeoptions autoformat="False" format="ShortTime" /><resolveoverlappingoptions allowrotate="False" /><datetimeoptions autoformat="False" format="ShortTime" /><resolveoverlappingoptions allowrotate="False" /><datetimeoptions autoformat="False" format="ShortTime" /><resolveoverlappingoptions allowrotate="False" /><datetimeoptions autoformat="False" format="ShortTime" /><resolveoverlappingoptions allowrotate="False" /><datetimeoptions autoformat="False" format="ShortTime" /><resolveoverlappingoptions allowrotate="False" /><datetimeoptions autoformat="False" format="ShortTime" /><resolveoverlappingoptions allowrotate="False" /><datetimeoptions autoformat="False" format="ShortTime" /><resolveoverlappingoptions allowrotate="False" /><datetimeoptions autoformat="False" format="ShortTime" /><resolveoverlappingoptions allowrotate="False" /><datetimeoptions autoformat="False" format="ShortTime" /><resolveoverlappingoptions allowrotate="False" /><datetimeoptions autoformat="False" format="ShortTime" /><resolveoverlappingoptions allowrotate="False" /><datetimeoptions autoformat="False" format="ShortTime" /><resolveoverlappingoptions allowrotate="False" /><datetimeoptions autoformat="False" format="ShortTime" /><resolveoverlappingoptions allowrotate="False" /><datetimeoptions autoformat="False" format="ShortTime" /><resolveoverlappingoptions allowrotate="False" /><datetimeoptions autoformat="False" format="ShortTime" /><resolveoverlappingoptions allowrotate="False" /><datetimeoptions autoformat="False" format="ShortTime" /><resolveoverlappingoptions allowrotate="False" /><datetimeoptions autoformat="False" format="ShortTime" /><resolveoverlappingoptions allowrotate="False" /><datetimeoptions autoformat="False" format="ShortTime" /><resolveoverlappingoptions allowrotate="False" /><datetimeoptions autoformat="False" format="ShortTime" /><resolveoverlappingoptions allowrotate="False" /><datetimeoptions autoformat="False" format="ShortTime" /><resolveoverlappingoptions allowrotate="False" /><datetimeoptions autoformat="False" format="ShortTime" /><resolveoverlappingoptions allowrotate="False" /><datetimeoptions autoformat="False" format="ShortTime" /><resolveoverlappingoptions allowrotate="False" /><datetimeoptions autoformat="False" format="ShortTime" /><resolveoverlappingoptions allowrotate="False" /><datetimeoptions autoformat="False" format="ShortTime" /><resolveoverlappingoptions allowrotate="False" /><datetimeoptions autoformat="False" format="ShortTime" /><resolveoverlappingoptions allowrotate="False" /><datetimeoptions autoformat="False" format="ShortTime" /><resolveoverlappingoptions allowrotate="False" /><datetimeoptions autoformat="False" format="ShortTime" /><resolveoverlappingoptions allowrotate="False" /><datetimeoptions autoformat="False" format="ShortTime" /><resolveoverlappingoptions allowrotate="False" /><datetimeoptions autoformat="False" format="ShortTime" /><resolveoverlappingoptions allowrotate="False" /><datetimeoptions autoformat="False" format="ShortTime" /><resolveoverlappingoptions allowrotate="False" /><datetimeoptions autoformat="False" format="ShortTime" /><resolveoverlappingoptions allowrotate="False" /><datetimeoptions autoformat="False" format="ShortTime" /></label>
                                                                <datetimescaleoptions measureunit="Hour">
                                                                </datetimescaleoptions>
                                                            </axisx>
                                                            <axisy visibleinpanesserializable="-1" title-text="Response Time (ms)" 
                                                                title-visible="True" title-font="Tahoma, 9pt">
                                                            </axisy>
                                                        </cc1:XYDiagram>
                                                    </diagramserializable>
															<legend alignmenthorizontal="Center" alignmentvertical="BottomOutside" maxverticalpercentage="30"
																markersize="13, 16"></legend>
															<seriesserializable>
                                                        <cc1:Series ArgumentScaleType="DateTime" Name="Series 1" 
															CrosshairLabelPattern="{A}: {V:n1}">
                                                            <viewserializable>
                                                                <cc1:LineSeriesView MarkerVisibility="True">
                                                                    <linemarkeroptions size="7">
                                                                    </linemarkeroptions>
                                                                </cc1:LineSeriesView>
                                                            </viewserializable>
                                                            <labelserializable>
                                                                <cc1:PointSeriesLabel>
                                                                    <pointoptionsserializable>
                                                                        <cc1:PointOptions>
                                                                            <valuenumericoptions format="Number" precision="0" />
                                                                        	<valuenumericoptions format="Number" precision="0" />
                                                                        <valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /></cc1:PointOptions>
                                                                    </pointoptionsserializable>
                                                                </cc1:PointSeriesLabel>
                                                            </labelserializable>
                                                            <legendpointoptionsserializable>
                                                                <cc1:PointOptions>
                                                                    <valuenumericoptions format="Number" precision="0" />
                                                                	<valuenumericoptions format="Number" precision="0" />
                                                                <valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /></cc1:PointOptions>
                                                            </legendpointoptionsserializable>
                                                        </cc1:Series>
                                                        <cc1:Series ArgumentScaleType="DateTime" Name="Series 2" CrosshairLabelPattern="{A}: {V:n1}">
                                                            <viewserializable>
                                                                <cc1:LineSeriesView MarkerVisibility="True">
                                                                    <linemarkeroptions size="7">
                                                                    </linemarkeroptions>
                                                                </cc1:LineSeriesView>
                                                            </viewserializable>
                                                            <labelserializable>
                                                                <cc1:PointSeriesLabel>
                                                                    <pointoptionsserializable>
                                                                        <cc1:PointOptions>
                                                                            <valuenumericoptions format="Number" precision="0" />
                                                                        	<valuenumericoptions format="Number" precision="0" />
                                                                        <valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /></cc1:PointOptions>
                                                                    </pointoptionsserializable>
                                                                </cc1:PointSeriesLabel>
                                                            </labelserializable>
                                                            <legendpointoptionsserializable>
                                                                <cc1:PointOptions>
                                                                    <valuenumericoptions format="Number" precision="0" />
                                                                	<valuenumericoptions format="Number" precision="0" />
                                                                <valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /></cc1:PointOptions>
                                                            </legendpointoptionsserializable>
                                                        </cc1:Series>
                                                    </seriesserializable>
															<seriestemplate>
                                                        <viewserializable>
                                                            <cc1:LineSeriesView>
                                                            </cc1:LineSeriesView>
                                                        </viewserializable>
                                                        <labelserializable>
                                                            <cc1:PointSeriesLabel>
                                                                <pointoptionsserializable>
                                                                    <cc1:PointOptions>
                                                                        <valuenumericoptions format="Number" precision="0" />
                                                                    	<valuenumericoptions format="Number" precision="0" />
                                                                    <valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /></cc1:PointOptions>
                                                                </pointoptionsserializable>
                                                            </cc1:PointSeriesLabel>
                                                        </labelserializable>
                                                        <legendpointoptionsserializable>
                                                            <cc1:PointOptions>
                                                                <valuenumericoptions format="Number" precision="0" />
                                                            	<valuenumericoptions format="Number" precision="0" />
                                                            <valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /></cc1:PointOptions>
                                                        </legendpointoptionsserializable>
                                                    </seriestemplate>
														</dxchartsui:WebChartControl>
													</td>
													<td valign="top">
														<dxchartsui:WebChartControl ID="UsersDailyWebChart" runat="server" CrosshairEnabled="True"
															Height="200px" PaletteName="Oriel" Width="400px">
															<diagramserializable>
                                                                <cc1:XYDiagram>
                                                                    <axisx visibleinpanesserializable="-1">
                                                                    </axisx>
                                                                    <axisy visibleinpanesserializable="-1">
                                                                    </axisy>
                                                                </cc1:XYDiagram>
                                                            </diagramserializable>
															<legend visibility="False"></legend>
															<seriesserializable>
                                                                <cc1:Series ArgumentScaleType="DateTime" Name="Series 1">
                                                                    <viewserializable>
                                                                        <cc1:LineSeriesView MarkerVisibility="True">
                                                                            <linemarkeroptions size="7">
                                                                            </linemarkeroptions>
                                                                        </cc1:LineSeriesView>
                                                                    </viewserializable>
                                                                </cc1:Series>
                                                                <cc1:Series ArgumentScaleType="DateTime" Name="Series 2">
                                                                    <viewserializable>
                                                                        <cc1:LineSeriesView MarkerVisibility="True">
                                                                            <linemarkeroptions size="7">
                                                                            </linemarkeroptions>
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
														</dxchartsui:WebChartControl>
													</td>
													<td valign="top">
														<dxchartsui:WebChartControl ID="DevicesWebChart" runat="server" BackColor="White"
															ClientInstanceName="cDevicesWebChart" CrosshairEnabled="True" Height="200px"
															OnCustomCallback="DevicesWebChart_CustomCallback" PaletteName="Oriel" Width="400px">
															<diagramserializable>
                                                                <cc1:SimpleDiagram EqualPieSize="False">
                                                                </cc1:SimpleDiagram>
                                                            </diagramserializable>
															<seriesserializable>
                                                                <cc1:Series Name="Series 1">
                                                                    <viewserializable>
                                                                        <cc1:PieSeriesView RuntimeExploding="False">
                                                                        </cc1:PieSeriesView>
                                                                    </viewserializable>
                                                                    <labelserializable>
                                                                        <cc1:PieSeriesLabel>
                                                                            <pointoptionsserializable>
                                                                                <cc1:PiePointOptions>
                                                                                    <valuenumericoptions format="Percent" />
                                                                                    <valuenumericoptions format="Percent" />
                                                                                <valuenumericoptions format="Percent" /><valuenumericoptions format="Percent" /><valuenumericoptions format="Percent" /><valuenumericoptions format="Percent" /><valuenumericoptions format="Percent" /><valuenumericoptions format="Percent" /></cc1:PiePointOptions>
                                                                            </pointoptionsserializable>
                                                                        </cc1:PieSeriesLabel>
                                                                    </labelserializable>
                                                                    <legendpointoptionsserializable>
                                                                        <cc1:PiePointOptions>
                                                                            <valuenumericoptions format="Percent" />
                                                                            <valuenumericoptions format="Percent" />
                                                                        <valuenumericoptions format="Percent" /><valuenumericoptions format="Percent" /><valuenumericoptions format="Percent" /><valuenumericoptions format="Percent" /><valuenumericoptions format="Percent" /><valuenumericoptions format="Percent" /></cc1:PiePointOptions>
                                                                    </legendpointoptionsserializable>
                                                                </cc1:Series>
                                                            </seriesserializable>
															<seriestemplate>
                                                                <viewserializable>
                                                                    <cc1:PieSeriesView RuntimeExploding="False">
                                                                    </cc1:PieSeriesView>
                                                                </viewserializable>
                                                                <labelserializable>
                                                                    <cc1:PieSeriesLabel>
                                                                        <pointoptionsserializable>
                                                                            <cc1:PiePointOptions PercentOptions-ValueAsPercent="False">
                                                                                <valuenumericoptions format="Percent" />
                                                                                <valuenumericoptions format="Percent" />
                                                                            <valuenumericoptions format="Percent" /><valuenumericoptions format="Percent" /><valuenumericoptions format="Percent" /><valuenumericoptions format="Percent" /><valuenumericoptions format="Percent" /><valuenumericoptions format="Percent" /></cc1:PiePointOptions>
                                                                        </pointoptionsserializable>
                                                                    </cc1:PieSeriesLabel>
                                                                </labelserializable>
                                                                <legendpointoptionsserializable>
                                                                    <cc1:PiePointOptions PercentOptions-ValueAsPercent="False">
                                                                        <valuenumericoptions format="Percent" />
                                                                        <valuenumericoptions format="Percent" />
                                                                    <valuenumericoptions format="Percent" /><valuenumericoptions format="Percent" /><valuenumericoptions format="Percent" /><valuenumericoptions format="Percent" /><valuenumericoptions format="Percent" /><valuenumericoptions format="Percent" /></cc1:PiePointOptions>
                                                                </legendpointoptionsserializable>
                                                            </seriestemplate>
														</dxchartsui:WebChartControl>
													</td>
												</tr>

											</table>
                                            <table>
                                                <tr>
                                                    <td>
                                                    <dx:ASPxLabel ID="TotalLicenseslb" runat="server" CssClass="lblsmallFont" Text="Total Licenses:">
																					</dx:ASPxLabel>

                                                    </td>
                                                     <td>
                                                    <dx:ASPxLabel ID="MonthlyLicenseCostlb" runat="server" CssClass="lblsmallFont" Text="Monthly License Cost:">
																					</dx:ASPxLabel>

                                                    </td>
                                                </tr>
                                                 <tr>
                                                    <td>
                                                    <dx:ASPxLabel ID="UnassignedLicenseslb" runat="server" CssClass="lblsmallFont" Text="Unassigned Licenses:">
																					</dx:ASPxLabel>

                                                    </td>
                                                     <td>
                                                    <dx:ASPxLabel ID="costofUnassignedlicenseslb" runat="server" CssClass="lblsmallFont" Text="Monthly Cost of Unassigned Licenses:">
																					</dx:ASPxLabel>

                                                    </td>
                                                </tr>
                                                 <tr>
                                                    <td>
                                                    <dx:ASPxLabel ID="InactiveUserslb" runat="server" CssClass="lblsmallFont" Text="Inactive Users:">
																					</dx:ASPxLabel>

                                                    </td>
                                                     <td>
                                                    <dx:ASPxLabel ID="costofinactiveuserslb" runat="server" CssClass="lblsmallFont" Text="Monthly Cost of Inactive Users:">
																					</dx:ASPxLabel>

                                                    </td>
                                                </tr>
                                            </table>
										</ContentTemplate>
										<Triggers>
											<asp:AsyncPostBackTrigger ControlID="officehealthgrid" />
										</Triggers>
									</asp:UpdatePanel>
								</dx:ContentControl>
							</ContentCollection>
						</dx:TabPage>
						<dx:TabPage Text="Mail Stats">
							<ContentCollection>
								<dx:ContentControl runat="server">
									<asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
										<Triggers>
											<asp:AsyncPostBackTrigger ControlID="officehealthgrid" />
										</Triggers>
										<ContentTemplate>
											<table class="navbarTbl" width="100%">
												<tr>
													<td>
														<dxchartsui:WebChartControl ID="deviceTypeWebChart" runat="server" ClientInstanceName="DeviceTypeChart"
															CrosshairEnabled="True" Height="200px" OnCustomCallback="deviceTypeWebChart_CustomCallback"
															Width="400px" PaletteName="Oriel">
															<diagramserializable>
                                                        <cc1:XYDiagram>
                                                            <axisx visibleinpanesserializable="-1" title-font="Tahoma, 10pt" 
                                                                title-text="" title-visibility="Default">
                                                                
                                                                
                                                                
<Tickmarks MinorVisible="False"></Tickmarks>

                                                                
<VisualRange AutoSideMargins="True"></VisualRange>

<WholeRange AutoSideMargins="True"></WholeRange>
                                                            <visualrange autosidemargins="True" /><wholerange autosidemargins="True" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><visualrange autosidemargins="True" /><wholerange autosidemargins="True" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><visualrange autosidemargins="True" /><wholerange autosidemargins="True" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><visualrange autosidemargins="True" /><wholerange autosidemargins="True" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><label>
                                                                    
<ResolveOverlappingOptions AllowRotate="False"></ResolveOverlappingOptions>
                                                                <resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" /><resolveoverlappingoptions allowrotate="False" /></label><visualrange autosidemargins="True" /><wholerange autosidemargins="True" /><visualrange autosidemargins="True" /><wholerange autosidemargins="True" /><visualrange autosidemargins="True" /><wholerange autosidemargins="True" /><visualrange autosidemargins="True" /><wholerange autosidemargins="True" /></axisx>
                                                            <axisy visibleinpanesserializable="-1" title-font="Tahoma, 10pt" 
                                                                title-text="Mailbox Size (GB)" title-visibility="True">
                                                                
<Tickmarks MinorVisible="False"></Tickmarks>

                                                                
                                                                
                                                                

<VisualRange AutoSideMargins="True"></VisualRange>

<WholeRange AutoSideMargins="True"></WholeRange>
                                                            <tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><label>
                                                                    
                                                                
<ResolveOverlappingOptions AllowStagger="False" AllowRotate="False"></ResolveOverlappingOptions>

<NumericOptions Format="Number" Precision="0"></NumericOptions>
                                                                <numericoptions format="Number" precision="0" /><resolveoverlappingoptions allowrotate="False" allowstagger="False" /><resolveoverlappingoptions allowrotate="False" allowstagger="False" /><numericoptions format="Number" precision="0" /><numericoptions format="Number" precision="0" /><resolveoverlappingoptions allowrotate="False" allowstagger="False" /><resolveoverlappingoptions allowrotate="False" allowstagger="False" /><numericoptions format="Number" precision="0" /><numericoptions format="Number" precision="0" /><resolveoverlappingoptions allowrotate="False" allowstagger="False" /><resolveoverlappingoptions allowrotate="False" allowstagger="False" /><numericoptions format="Number" precision="0" /><numericoptions format="Number" precision="0" /><resolveoverlappingoptions allowrotate="False" allowstagger="False" /><resolveoverlappingoptions allowrotate="False" allowstagger="False" /><numericoptions format="Number" precision="0" /></label><visualrange autosidemargins="True" /><wholerange autosidemargins="True" /><visualrange autosidemargins="True" /><wholerange autosidemargins="True" /><visualrange autosidemargins="True" /><wholerange autosidemargins="True" /><visualrange autosidemargins="True" /><wholerange autosidemargins="True" /><visualrange autosidemargins="True" /><wholerange autosidemargins="True" /><visualrange autosidemargins="True" /><wholerange autosidemargins="True" /><visualrange autosidemargins="True" /><wholerange autosidemargins="True" /><visualrange autosidemargins="True" /><wholerange autosidemargins="True" /></axisy>
                                                        </cc1:XYDiagram>
                                                    </diagramserializable>
															<seriesserializable>
                                                        <cc1:Series Name="Series 1" CrosshairLabelPattern="{A:g}">
                                                        </cc1:Series>
                                                        <cc1:Series Name="Series 2" CrosshairLabelPattern="{A:g}">
                                                        </cc1:Series>
                                                    </seriesserializable>
														</dxchartsui:WebChartControl>
													</td>
													<td>
														<dxchartsui:WebChartControl ID="InactiveMailboxesWebChart" runat="server" CrosshairEnabled="True"
															Height="200px" PaletteName="Oriel" Width="400px">
															<diagramserializable>
                                                        <cc1:XYDiagram Rotated="True">
                                                            <axisx visibleinpanesserializable="-1">
                                                                <tickmarks minorvisible="False" />
                                                                <tickmarks minorvisible="False" />
                                                            <tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /></axisx>
                                                            <axisy title-font="Tahoma, 10pt" title-text="Days of Inactivity" 
                                                                title-visibility="True" visibleinpanesserializable="-1">
                                                                <tickmarks minorvisible="False" />
                                                                <tickmarks minorvisible="False" />
                                                                <tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><tickmarks minorvisible="False" /><label>
                                                                <resolveoverlappingoptions allowstagger="False" allowrotate="False" /><resolveoverlappingoptions allowstagger="False" allowrotate="False" /><resolveoverlappingoptions allowstagger="False" allowrotate="False" /><resolveoverlappingoptions allowstagger="False" allowrotate="False" />
                                                                <resolveoverlappingoptions allowrotate="False" allowstagger="False"></resolveoverlappingoptions>
                                                                </label>
                                                            </axisy>
                                                        </cc1:XYDiagram>
                                                    </diagramserializable>
															<legend visibility="False"></legend>
															<seriesserializable>
                                                        <cc1:Series Name="Series 1">
                                                        </cc1:Series>
                                                        <cc1:Series Name="Series 2">
                                                        </cc1:Series>
                                                    </seriesserializable>
															<titles>
                                                        <cc1:ChartTitle Font="Tahoma, 12pt" Text="Top 5 Inactive Mailboxes" />
                                                    </titles>
														</dxchartsui:WebChartControl>
													</td>
													<td>
														<dxchartsui:WebChartControl ID="MailBoxTypeWebChart" runat="server" Height="200px"
															PaletteName="Oriel" Width="400px" CrosshairEnabled="True">
															<diagramserializable>
                                                        <cc1:SimpleDiagram EqualPieSize="False">
                                                        </cc1:SimpleDiagram>
                                                    </diagramserializable>
															<legend visibility="True" alignmenthorizontal="Center" alignmentvertical="BottomOutside"
																maxverticalpercentage="30"></legend>
															<seriesserializable>
                                                        <cc1:Series ArgumentScaleType="Qualitative" Name="Series 1">
                                                            <viewserializable>
                                                                
                                                            <cc1:DoughnutSeriesView>
                                                                </cc1:DoughnutSeriesView></viewserializable>
                                                            <labelserializable>
                                                                
                                                            <cc1:DoughnutSeriesLabel TextPattern="{A}: {VP}">
                                                                </cc1:DoughnutSeriesLabel></labelserializable>
                                                        </cc1:Series>
                                                    </seriesserializable>
															<seriestemplate>
                                                        <viewserializable>
                                                            
                                                        <cc1:DoughnutSeriesView>
                                                            </cc1:DoughnutSeriesView></viewserializable>
                                                    </seriestemplate>
															<titles>
                                                        <cc1:ChartTitle Font="Tahoma, 12pt" Text="Mailbox Types" />
                                                    </titles>
														</dxchartsui:WebChartControl>
													</td>
												</tr>
												<tr>
													<td>
													</td>
												</tr>
												<tr>
													<td>
													</td>
												</tr>
												<tr>
													<td>
														<dxchartsui:WebChartControl ID="InactiveUsersWebChart" runat="server" CrosshairEnabled="True"
															Height="200px" PaletteName="Oriel" Width="400px">
															<diagramserializable>
                                                        <cc1:SimpleDiagram EqualPieSize="False">
                                                        </cc1:SimpleDiagram>
                                                    </diagramserializable>
															<legend alignmenthorizontal="Center" alignmentvertical="BottomOutside" maxverticalpercentage="30">
															</legend>
															<seriesserializable>
                                                        <cc1:Series Name="Series 1">
                                                            <viewserializable>
                                                                <cc1:DoughnutSeriesView>
                                                                </cc1:DoughnutSeriesView>
                                                            </viewserializable>
                                                            <labelserializable>
                                                                <cc1:DoughnutSeriesLabel TextPattern="{A}:{VP:P0} ">
                                                                </cc1:DoughnutSeriesLabel>
                                                            </labelserializable>
                                                        </cc1:Series>
                                                    </seriesserializable>
															<seriestemplate argumentscaletype="Qualitative">
                                                        <viewserializable>
                                                            <cc1:DoughnutSeriesView>
                                                            </cc1:DoughnutSeriesView>
                                                        </viewserializable>
                                                    </seriestemplate>
															<titles>
                                                        <cc1:ChartTitle Font="Tahoma, 12pt" Text="Active/Inactive Users" />
                                                    </titles>
														</dxchartsui:WebChartControl>
													</td>
													<td>
													</td>
													<td>
													</td>
												</tr>
												<tr>
													<td colspan="3">
														<div style="display: none">
															<table>
																<tr style="border-bottom-style: solid; border-bottom-width: 1px; border-bottom-color: Gray">
																	<td style="padding-right: 100px; border-bottom-style: solid; border-bottom-width: 1px;
																		border-bottom-color: Gray">
																		<asp:Label ID="lblNoOfMailBoxes" runat="Server" Text="No of Mailboxes" />
																	</td>
																	<td align="right" style="border-bottom-style: solid; border-bottom-width: 1px; border-bottom-color: Gray">
																		<asp:Label ID="lblNoOfMailBoxesValue" runat="Server" />
																	</td>
																</tr>
																<tr style="border-bottom-style: solid; border-bottom-width: 1px; border-bottom-color: Gray">
																	<td style="padding-right: 100px; border-bottom-style: solid; border-bottom-width: 1px;
																		border-bottom-color: Gray">
																		<asp:Label ID="lblSizeOfMailBoxes" runat="Server" Text="Size of Mailboxes" />
																	</td>
																	<td align="right" style="border-bottom-style: solid; border-bottom-width: 1px; border-bottom-color: Gray">
																		<asp:Label ID="lblSizeOfMailBoxesValue" runat="Server" />
																	</td>
																</tr>
																<tr style="border-bottom-style: solid; border-bottom-width: 1px; border-bottom-color: Gray">
																	<td style="padding-right: 100px; border-bottom-style: solid; border-bottom-width: 1px;
																		border-bottom-color: Gray">
																		<asp:Label ID="lblTotalNoOfItems" runat="Server" Text="Total No of Items" />
																	</td>
																	<td align="right" style="border-bottom-style: solid; border-bottom-width: 1px; border-bottom-color: Gray">
																		<asp:Label ID="lblTotalNoOfItemsValue" runat="Server" />
																	</td>
																</tr>
																<tr style="border-bottom-style: solid; border-bottom-width: 1px; border-bottom-color: Gray">
																	<td style="padding-right: 100px; border-bottom-style: solid; border-bottom-width: 1px;
																		border-bottom-color: Gray">
																		<asp:Label ID="lblAvgSizeOfMailBoxes" runat="Server" Text="Avg Size of Mailboxes" />
																	</td>
																	<td align="right" style="border-bottom-style: solid; border-bottom-width: 1px; border-bottom-color: Gray">
																		<asp:Label ID="lblAvgSizeOfMailBoxesValue" runat="Server" />
																	</td>
																</tr>
																<tr style="border-bottom-style: solid; border-bottom-width: 1px; border-bottom-color: Gray">
																	<td style="padding-right: 100px; border-bottom-style: solid; border-bottom-width: 1px;
																		border-bottom-color: Gray">
																		<asp:Label ID="lblAvgCountOfItems" runat="Server" Text="Avg Count of Items" />
																	</td>
																	<td align="right" style="border-bottom-style: solid; border-bottom-width: 1px; border-bottom-color: Gray">
																		<asp:Label ID="lblAvgCountOfItemsValue" runat="Server" />
																	</td>
																</tr>
															</table>
														</div>
													</td>
													<td>
													</td>
												</tr>
											</table>
										</ContentTemplate>
									</asp:UpdatePanel>
								</dx:ContentControl>
							</ContentCollection>
						</dx:TabPage>
						<dx:TabPage Text="Health Assessment">
							<ContentCollection>
								<dx:ContentControl runat="server">
									<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
										<ContentTemplate>
											<dx:ASPxGridView ID="HealthAssessmentGrid" runat="server" AutoGenerateColumns="False"
												EnableTheming="True" Theme="Office2003Blue" Width="100%" OnHtmlDataCellPrepared="HealthAssessmentGrid_HtmlDataCellPrepared"
												OnPageSizeChanged="HealthAssessmentGrid_PageSizeChanged">
												<Columns>
													<dx:GridViewDataTextColumn FieldName="ID" Name="ID" Visible="False" VisibleIndex="0">
													</dx:GridViewDataTextColumn>
													<dx:GridViewDataTextColumn FieldName="Category" Name="Category" VisibleIndex="1">
													</dx:GridViewDataTextColumn>
													<dx:GridViewDataTextColumn FieldName="TestName" Name="Test Name" VisibleIndex="2">
													</dx:GridViewDataTextColumn>
													<dx:GridViewDataTextColumn FieldName="Result" Name="Result" VisibleIndex="3" Width="80px">
														<HeaderStyle CssClass="GridCssHeader1" />
														<CellStyle CssClass="GridCss1">
														</CellStyle>
													</dx:GridViewDataTextColumn>
													<dx:GridViewDataTextColumn FieldName="Details" Name="Details" VisibleIndex="4">
													</dx:GridViewDataTextColumn>
													<dx:GridViewDataTextColumn FieldName="LastUpdate" Name="Last Update" VisibleIndex="5"
														Width="200px">
													</dx:GridViewDataTextColumn>
												</Columns>
												<SettingsPager AlwaysShowPager="True">
													<PageSizeItemSettings Visible="True">
													</PageSizeItemSettings>
												</SettingsPager>
												<Styles>
													<Header CssClass="GridCssHeader">
													</Header>
													<AlternatingRow CssClass="GridAltRow">
													</AlternatingRow>
													<Cell CssClass="GridCss">
													</Cell>
												</Styles>
											</dx:ASPxGridView>
										</ContentTemplate>
									</asp:UpdatePanel>
								</dx:ContentControl>
							</ContentCollection>
						</dx:TabPage>
						<dx:TabPage Text="User Scenario Tests">
							<ContentCollection>
								<dx:ContentControl runat="server">
									<table class="navbarTbl" width="100%">
										<tr>
											<td>
												<dxchartsui:WebChartControl ID="MailTestScenarioWebChart" runat="server" CrosshairEnabled="True"
													Height="200px" Width="400px" ClientInstanceName="cUserScenarioWebChart" PaletteName="Oriel"
													OnCustomCallback="MailTestScenarioWebChart_CustomCallback">
													<diagramserializable>
                                                        <cc1:XYDiagram>
                                                            <axisx visibleinpanesserializable="-1" title-text="Time" title-visible="True" 
                                                                title-visibility="Default">
                                                                <label>
                                                                    <resolveoverlappingoptions allowrotate="False" />
                                                                    <datetimeoptions autoformat="False" format="ShortTime" />
<ResolveOverlappingOptions AllowRotate="False"></ResolveOverlappingOptions>

<DateTimeOptions Format="ShortTime" AutoFormat="False"></DateTimeOptions>
                                                                </label>
                                                                <datetimescaleoptions measureunit="Hour">
                                                                </datetimescaleoptions>
                                                            </axisx>
                                                            <axisy visibleinpanesserializable="-1" title-text="Response Time (ms)" 
                                                                title-visible="True" title-font="Tahoma, 10pt">
                                                            </axisy>
                                                        </cc1:XYDiagram>
                                                    </diagramserializable>
													<legend markersize="13, 16" alignmenthorizontal="Center" alignmentvertical="BottomOutside"
														maxverticalpercentage="30"></legend>
													<seriesserializable>
                                                        <cc1:Series ArgumentScaleType="DateTime" Name="Series 1" CrosshairLabelPattern="{A:g}">
                                                            <viewserializable>
                                                                <cc1:LineSeriesView MarkerVisibility="True">
                                                                    <linemarkeroptions size="7">
                                                                    </linemarkeroptions>
                                                                </cc1:LineSeriesView>
                                                            </viewserializable>
                                                        </cc1:Series>
                                                        <cc1:Series ArgumentScaleType="DateTime" Name="Series 2" CrosshairLabelPattern="{A:g}">
                                                            <viewserializable>
                                                                <cc1:LineSeriesView MarkerVisibility="True">
                                                                    <linemarkeroptions size="7">
                                                                    </linemarkeroptions>
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
												</dxchartsui:WebChartControl>
											</td>
											<td>
												<dxchartsui:WebChartControl ID="SiteTestsWebChart" runat="server" ClientInstanceName="cSiteTestsWebChart"
													CrosshairEnabled="True" Height="200px" OnCustomCallback="SiteTestsWebChart_CustomCallback"
													Width="400px" PaletteName="Oriel">
													<diagramserializable>
                                                        <cc1:XYDiagram>
                                                            <axisx visibleinpanesserializable="-1" title-text="Time" title-visible="True" 
                                                                title-visibility="Default">
                                                                <label>
                                                                    <resolveoverlappingoptions allowrotate="False" />
                                                                    <datetimeoptions autoformat="False" format="ShortTime" />
<ResolveOverlappingOptions AllowRotate="False"></ResolveOverlappingOptions>

<DateTimeOptions Format="ShortTime" AutoFormat="False"></DateTimeOptions>
                                                                </label>
                                                                <datetimescaleoptions measureunit="Hour">
                                                                </datetimescaleoptions>
                                                            </axisx>
                                                            <axisy visibleinpanesserializable="-1" title-text="Response Time (ms)" 
                                                                title-visible="True" title-font="Tahoma, 10pt">
                                                            </axisy>
                                                        </cc1:XYDiagram>
                                                    </diagramserializable>
													<legend markersize="13, 16" alignmenthorizontal="Center" alignmentvertical="BottomOutside"
														maxverticalpercentage="30"></legend>
													<seriesserializable>
                                                        <cc1:Series ArgumentScaleType="DateTime" Name="Series 1" 
															CrosshairLabelPattern="{A:g}">
															
                                                            <viewserializable>
                                                                <cc1:LineSeriesView MarkerVisibility="True">
                                                                    <linemarkeroptions size="7">
                                                                    </linemarkeroptions>
                                                                </cc1:LineSeriesView>
                                                            </viewserializable>
                                                        </cc1:Series>
                                                        <cc1:Series ArgumentScaleType="DateTime" Name="Series 2" 
															 CrosshairLabelPattern="{A:g}">
                                                            <viewserializable>
                                                                <cc1:LineSeriesView MarkerVisibility="True">
                                                                    <linemarkeroptions size="7">
                                                                    </linemarkeroptions>
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
												</dxchartsui:WebChartControl>
											</td>
											<td>
												<dxchartsui:WebChartControl ID="TaskFolderTestsWebChart" runat="server" ClientInstanceName="cTaskFolderTestsWebChart"
													CrosshairEnabled="True" Height="200px" OnCustomCallback="TaskFolderTestsWebChart_CustomCallback"
													Width="400px" PaletteName="Oriel">
													<diagramserializable>
                                                        <cc1:XYDiagram>
                                                            <axisx visibleinpanesserializable="-1" title-text="Time" title-visible="True" 
                                                                title-visibility="Default">
                                                                <label>
                                                                    <resolveoverlappingoptions allowrotate="False" />
                                                                    <datetimeoptions autoformat="False" format="ShortTime" />
<ResolveOverlappingOptions AllowRotate="False"></ResolveOverlappingOptions>

<DateTimeOptions Format="ShortTime" AutoFormat="False"></DateTimeOptions>
                                                                </label>
                                                                <datetimescaleoptions measureunit="Hour">
                                                                </datetimescaleoptions>
                                                            </axisx>
                                                            <axisy visibleinpanesserializable="-1" title-text="Response Time (ms)" 
                                                                title-visible="True" title-font="Tahoma, 10pt">
                                                            </axisy>
                                                        </cc1:XYDiagram>
                                                    </diagramserializable>
													<legend markersize="13, 16" alignmenthorizontal="Center" alignmentvertical="BottomOutside"
														maxverticalpercentage="30"></legend>
													<seriesserializable>
                                                        <cc1:Series ArgumentScaleType="DateTime" Name="Series 1" CrosshairLabelPattern="{A:g}">
                                                            <viewserializable>
                                                                <cc1:LineSeriesView MarkerVisibility="True">
                                                                    <linemarkeroptions size="7">
                                                                    </linemarkeroptions>
                                                                </cc1:LineSeriesView>
                                                            </viewserializable>
                                                        </cc1:Series>
                                                        <cc1:Series ArgumentScaleType="DateTime" Name="Series 2" CrosshairLabelPattern="{A:g}">
                                                            <viewserializable>
                                                                <cc1:LineSeriesView MarkerVisibility="True">
                                                                    <linemarkeroptions size="7">
                                                                    </linemarkeroptions>
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
												</dxchartsui:WebChartControl>
											</td>
										</tr>
										<tr>
											<td>
											</td>
										</tr>
										<tr>
											<td>
											</td>
										</tr>
										<tr>
											<td>
												<dxchartsui:WebChartControl ID="OneDriveWebChart" runat="server" ClientInstanceName="cOneDriveWebChart"
													CrosshairEnabled="True" Height="200px" PaletteName="Oriel" Width="400px" OnCustomCallback="OneDriveWebChart_CustomCallback">
													<diagramserializable>
                                                        <cc1:XYDiagram>
                                                            <axisx visibleinpanesserializable="-1" title-text="Time" title-visible="True" 
                                                                title-visibility="Default">
                                                                <tickmarks minorvisible="False" />
                                                                <tickmarks minorvisible="False" />
<Tickmarks MinorVisible="False"></Tickmarks>

                                                                <label>
                                                                    <resolveoverlappingoptions allowrotate="False" />
                                                                    <datetimeoptions autoformat="False" format="ShortTime" />
                                                                <resolveoverlappingoptions allowrotate="False"></resolveoverlappingoptions>
                                                                <datetimeoptions autoformat="False" format="ShortTime" />

<DateTimeOptions Format="ShortTime" AutoFormat="False"></DateTimeOptions>
                                                                </label>
                                                                <datetimescaleoptions measureunit="Hour">
                                                                </datetimescaleoptions>
                                                            </axisx>
                                                            <axisy visibleinpanesserializable="-1" title-text="Response Time (ms)" 
                                                                title-visible="True" title-font="Tahoma, 10pt">
                                                            </axisy>
                                                        </cc1:XYDiagram>
                                                    </diagramserializable>
													<legend markersize="13, 16" alignmenthorizontal="Center" alignmentvertical="BottomOutside"
														maxverticalpercentage="30"></legend>
													<seriesserializable>
                                                        <cc1:Series Name="Series 1" ArgumentScaleType="DateTime" CrosshairLabelPattern="{A:g}">
                                                            <viewserializable>
                                                                <cc1:LineSeriesView MarkerVisibility="True">
                                                                    <linemarkeroptions size="7">
                                                                    </linemarkeroptions>
                                                                </cc1:LineSeriesView>
                                                            </viewserializable>
                                                        </cc1:Series>
                                                        <cc1:Series Name="Series 2" ArgumentScaleType="DateTime" CrosshairLabelPattern="{A:g}">
                                                            <viewserializable>
                                                                <cc1:LineSeriesView MarkerVisibility="True">
                                                                    <linemarkeroptions size="7">
                                                                    </linemarkeroptions>
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
												</dxchartsui:WebChartControl>
											</td>
											<td>
											</td>
											<td>
											</td>
										</tr>
										<%--<tr>
										 <td valign="top">
                                                <dxchartsui:WebChartControl ID="DirSyncWebChart" runat="server" 
                                                    ClientInstanceName="cDirSyncWebChart" CrosshairEnabled="True" Height="300px" 
                                                    OnCustomCallback="DirSyncWebChart_CustomCallback" PaletteName="Paper" 
                                                    Width="400px">
                                                    <diagramserializable>
                                                        <cc1:XYDiagram>
                                                            <axisx visibleinpanesserializable="-1" title-text="Time" title-visible="True">
                                                                <label>
                                                                    <resolveoverlappingoptions allowrotate="False" /><datetimeoptions autoformat="False" format="ShortTime" /><resolveoverlappingoptions allowrotate="False" />
                                                                    <datetimeoptions autoformat="False" format="ShortTime" />
<ResolveOverlappingOptions AllowRotate="False"></ResolveOverlappingOptions>

<DateTimeOptions Format="ShortTime" AutoFormat="False"></DateTimeOptions>
                                                                </label>
                                                                <datetimescaleoptions measureunit="Hour">
                                                                </datetimescaleoptions>
                                                            </axisx>
                                                            <axisy visibleinpanesserializable="-1" title-text="Response Time (ms)" 
                                                                title-visible="True">
                                                            </axisy>
                                                        </cc1:XYDiagram>
                                                    </diagramserializable>
                                                    <seriesserializable>
                                                        <cc1:Series ArgumentScaleType="DateTime" Name="Series 1" 
															CrosshairLabelPattern="{A:g}">
                                                            <viewserializable>
                                                                <cc1:LineSeriesView MarkerVisibility="True">
                                                                </cc1:LineSeriesView>
                                                            </viewserializable>
                                                            <labelserializable>
                                                                <cc1:PointSeriesLabel>
                                                                    <pointoptionsserializable>
                                                                        <cc1:PointOptions>
                                                                            <valuenumericoptions format="Number" precision="0" />
                                                                        	<valuenumericoptions format="Number" precision="0" />
                                                                        <valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /></cc1:PointOptions>
                                                                    </pointoptionsserializable>
                                                                </cc1:PointSeriesLabel>
                                                            </labelserializable>
                                                            <legendpointoptionsserializable>
                                                                <cc1:PointOptions>
                                                                    <valuenumericoptions format="Number" precision="0" />
                                                                	<valuenumericoptions format="Number" precision="0" />
                                                                <valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /></cc1:PointOptions>
                                                            </legendpointoptionsserializable>
                                                        </cc1:Series>
                                                        <cc1:Series ArgumentScaleType="DateTime" Name="Series 2" CrosshairLabelPattern="{A:g}">
                                                            <viewserializable>
                                                                <cc1:LineSeriesView>
                                                                </cc1:LineSeriesView>
                                                            </viewserializable>
                                                            <labelserializable>
                                                                <cc1:PointSeriesLabel>
                                                                    <pointoptionsserializable>
                                                                        <cc1:PointOptions>
                                                                            <valuenumericoptions format="Number" precision="0" />
                                                                        	<valuenumericoptions format="Number" precision="0" />
                                                                        <valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /></cc1:PointOptions>
                                                                    </pointoptionsserializable>
                                                                </cc1:PointSeriesLabel>
                                                            </labelserializable>
                                                            <legendpointoptionsserializable>
                                                                <cc1:PointOptions>
                                                                    <valuenumericoptions format="Number" precision="0" />
                                                                	<valuenumericoptions format="Number" precision="0" />
                                                                <valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /></cc1:PointOptions>
                                                            </legendpointoptionsserializable>
                                                        </cc1:Series>
                                                    </seriesserializable>
                                                    <seriestemplate>
                                                        <viewserializable>
                                                            <cc1:LineSeriesView>
                                                            </cc1:LineSeriesView>
                                                        </viewserializable>
                                                        <labelserializable>
                                                            <cc1:PointSeriesLabel>
                                                                <pointoptionsserializable>
                                                                    <cc1:PointOptions>
                                                                        <valuenumericoptions format="Number" precision="0" />
                                                                    	<valuenumericoptions format="Number" precision="0" />
                                                                    <valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /></cc1:PointOptions>
                                                                </pointoptionsserializable>
                                                            </cc1:PointSeriesLabel>
                                                        </labelserializable>
                                                        <legendpointoptionsserializable>
                                                            <cc1:PointOptions>
                                                                <valuenumericoptions format="Number" precision="0" />
                                                            	<valuenumericoptions format="Number" precision="0" />
                                                            <valuenumericoptions format="Number" precision="0" /><valuenumericoptions format="Number" precision="0" /></cc1:PointOptions>
                                                        </legendpointoptionsserializable>
                                                    </seriestemplate>
                                                </dxchartsui:WebChartControl>
                                            </td>
										</tr>--%>
									</table>
								</dx:ContentControl>
							</ContentCollection>
						</dx:TabPage>
						<dx:TabPage Text="Password Settings">
							<ContentCollection>
								<dx:ContentControl runat="server">
									<table class="navbarTbl">
										<tr>
											<td valign="top">
												<dxchartsui:WebChartControl ID="Passwordneverexpires" runat="server" ClientInstanceName="Passwordneverexpires"
													CrosshairEnabled="True" Height="200px" PaletteName="Oriel" Width="400px">
													<diagramserializable>
                                                        <cc1:SimpleDiagram EqualPieSize="False">
                                                        </cc1:SimpleDiagram>
                                                    </diagramserializable>
													<legend alignmenthorizontal="Center" alignmentvertical="BottomOutside" maxverticalpercentage="30">
													</legend>
													<seriesserializable>
                                                        <cc1:Series LegendTextPattern="{VP:P2}" Name="Pie1">
                                                            <viewserializable>
                                                                <cc1:PieSeriesView>
                                                                </cc1:PieSeriesView>
                                                            </viewserializable>
                                                            <labelserializable>
                                                                <cc1:PieSeriesLabel LineVisibility="True">
                                                                </cc1:PieSeriesLabel>
                                                            </labelserializable>
                                                        </cc1:Series>
                                                    </seriesserializable>
													<seriestemplate legendtextpattern="{V:P2}">
                                                        <viewserializable>
                                                            <cc1:PieSeriesView>
                                                            </cc1:PieSeriesView>
                                                        </viewserializable>
                                                        <labelserializable>
                                                            <cc1:PieSeriesLabel LineVisibility="True">
                                                            </cc1:PieSeriesLabel>
                                                        </labelserializable>
                                                    </seriestemplate>
													<titles>
                                                        <cc1:ChartTitle Font="Tahoma, 12pt" Text="Password Expires" />
                                                    </titles>
												</dxchartsui:WebChartControl>
											</td>
											<td valign="top">
												<dxchartsui:WebChartControl ID="strongpasswordWebChart" runat="server" ClientInstanceName="strongpasswordWebChart"
													CrosshairEnabled="True" Height="200px" PaletteName="Oriel" Width="400px">
													<diagramserializable>
                                                        <cc1:SimpleDiagram EqualPieSize="False">
                                                        </cc1:SimpleDiagram>
                                                    </diagramserializable>
													<legend alignmenthorizontal="Center" alignmentvertical="BottomOutside" maxverticalpercentage="30">
													</legend>
													<seriesserializable>
                                                        <cc1:Series LegendTextPattern="{VP:P2}" Name="Pie1">
                                                            <viewserializable>
                                                                <cc1:PieSeriesView>
                                                                </cc1:PieSeriesView>
                                                            </viewserializable>
                                                            <labelserializable>
                                                                <cc1:PieSeriesLabel LineVisibility="True">
                                                                </cc1:PieSeriesLabel>
                                                            </labelserializable>
                                                        </cc1:Series>
                                                    </seriesserializable>
													<seriestemplate legendtextpattern="{V:P2}">
                                                        <viewserializable>
                                                            <cc1:PieSeriesView>
                                                            </cc1:PieSeriesView>
                                                        </viewserializable>
                                                        <labelserializable>
                                                            <cc1:PieSeriesLabel LineVisibility="True">
                                                            </cc1:PieSeriesLabel>
                                                        </labelserializable>
                                                    </seriestemplate>
													<titles>
                                                        <cc1:ChartTitle Font="Tahoma, 12pt" Text="Strong Password" />
                                                    </titles>
												</dxchartsui:WebChartControl>
											</td>
											<td>
												&nbsp;
											</td>
										</tr>
										<tr>
											<td colspan="3">
												&nbsp;
											</td>
										</tr>
										<tr>
											<td colspan="3">
												<dx:ASPxGridView ID="O365Usersettinggrid" runat="server" AutoGenerateColumns="False"
													Cursor="pointer" EnableCallBacks="False" EnableTheming="True" KeyFieldName="ServerId"
													Theme="Office2003Blue" Width="100%">
													<Columns>
														<dx:GridViewDataTextColumn Caption="Member Name" FieldName="DisplayName" FixedStyle="Left"
															ShowInCustomizationForm="True" VisibleIndex="1">
															<PropertiesTextEdit>
																<FocusedStyle HorizontalAlign="Center">
																</FocusedStyle>
															</PropertiesTextEdit>
															<Settings AllowAutoFilter="True" AllowAutoFilterTextInputTimer="False" AutoFilterCondition="Contains" />
															<EditCellStyle CssClass="GridCss">
															</EditCellStyle>
															<EditFormCaptionStyle CssClass="GridCss">
															</EditFormCaptionStyle>
															<HeaderStyle CssClass="GridCssHeader" />
															<CellStyle CssClass="GridCss" HorizontalAlign="Center">
															</CellStyle>
														</dx:GridViewDataTextColumn>
														<dx:GridViewDataTextColumn Caption="User Principal Name" FieldName="UserPrincipalName"
															ShowInCustomizationForm="True" VisibleIndex="2">
															<PropertiesTextEdit>
																<FocusedStyle HorizontalAlign="Center">
																</FocusedStyle>
															</PropertiesTextEdit>
															<Settings AllowAutoFilterTextInputTimer="False" AutoFilterCondition="Contains" />
															<EditCellStyle CssClass="GridCss">
															</EditCellStyle>
															<EditFormCaptionStyle CssClass="GridCss">
															</EditFormCaptionStyle>
															<HeaderStyle CssClass="GridCssHeader" />
															<CellStyle CssClass="GridCss" HorizontalAlign="Center">
															</CellStyle>
														</dx:GridViewDataTextColumn>
														<dx:GridViewDataTextColumn Caption="Strong Password Required" FieldName="StrongPasswordRequired"
															ShowInCustomizationForm="True" VisibleIndex="3">
															<PropertiesTextEdit>
																<FocusedStyle HorizontalAlign="Center">
																</FocusedStyle>
															</PropertiesTextEdit>
															<Settings AllowAutoFilterTextInputTimer="False" AutoFilterCondition="Contains" />
															<EditCellStyle CssClass="GridCss">
															</EditCellStyle>
															<EditFormCaptionStyle CssClass="GridCss">
															</EditFormCaptionStyle>
															<HeaderStyle CssClass="GridCssHeader" />
															<CellStyle CssClass="GridCss" HorizontalAlign="Center">
															</CellStyle>
														</dx:GridViewDataTextColumn>
														<dx:GridViewDataTextColumn Caption="Password Never Expires" FieldName="PasswordNeverExpires"
															ShowInCustomizationForm="True" VisibleIndex="4">
															<PropertiesTextEdit>
																<FocusedStyle HorizontalAlign="Center">
																</FocusedStyle>
															</PropertiesTextEdit>
															<Settings AllowAutoFilterTextInputTimer="False" AutoFilterCondition="Contains" />
															<EditCellStyle CssClass="GridCss">
															</EditCellStyle>
															<EditFormCaptionStyle CssClass="GridCss">
															</EditFormCaptionStyle>
															<HeaderStyle CssClass="GridCssHeader" />
															<CellStyle CssClass="GridCss" HorizontalAlign="Center">
															</CellStyle>
														</dx:GridViewDataTextColumn>
														<dx:GridViewDataTextColumn Caption="ServerId" FieldName="ServerId" ShowInCustomizationForm="True"
															Visible="False" VisibleIndex="5">
															<PropertiesTextEdit>
																<FocusedStyle HorizontalAlign="Center">
																</FocusedStyle>
															</PropertiesTextEdit>
															<Settings AllowAutoFilterTextInputTimer="False" AutoFilterCondition="Contains" />
															<EditCellStyle CssClass="GridCss">
															</EditCellStyle>
															<EditFormCaptionStyle CssClass="GridCss">
															</EditFormCaptionStyle>
															<HeaderStyle CssClass="GridCssHeader" />
															<CellStyle CssClass="GridCss" HorizontalAlign="Center">
															</CellStyle>
														</dx:GridViewDataTextColumn>
													</Columns>
													<SettingsBehavior AllowFocusedRow="True" AllowSelectByRowClick="True" ColumnResizeMode="NextColumn"
														ProcessSelectionChangedOnServer="True" />
													<SettingsPager AlwaysShowPager="True" NumericButtonCount="30">
														<PageSizeItemSettings Visible="True">
														</PageSizeItemSettings>
													</SettingsPager>
													<Settings ShowFilterRow="True" ShowGroupPanel="True" />
													<Styles>
														<AlternatingRow CssClass="GridAltRow" Enabled="True">
														</AlternatingRow>
														<Header VerticalAlign="Middle">
														</Header>
													</Styles>
												</dx:ASPxGridView>
											</td>
										</tr>
									</table>
								</dx:ContentControl>
							</ContentCollection>
						</dx:TabPage>
						<dx:TabPage Text="Skype for Business Stats">
							<ContentCollection>
								<dx:ContentControl runat="server">
									<table class="navbarTbl" width="100%">
										<tr>
											<td valign="top">
												<dxchartsui:WebChartControl ID="deviceTypeWebChart0" runat="server" ClientInstanceName="DeviceTypeChart0"
													CrosshairEnabled="True" Height="200px" Width="400px" OnCustomCallback="deviceTypeWebChart0_CustomCallback"
													PaletteName="Oriel">
													<diagramserializable>
                                                        <cc1:XYDiagram>
                                                            <axisx visibleinpanesserializable="-1">
                                                                <visualrange autosidemargins="True" />
                                                                <wholerange autosidemargins="True" />
                                                                <tickmarks minorvisible="False" />
<Tickmarks MinorVisible="False"></Tickmarks>

<VisualRange AutoSideMargins="True"></VisualRange>

<WholeRange AutoSideMargins="True"></WholeRange>
                                                            </axisx>
                                                            <axisy visibleinpanesserializable="-1">
                                                                <tickmarks minorvisible="False" />
<Tickmarks MinorVisible="False"></Tickmarks>

                                                                <label>
                                                                    <numericoptions format="Number" precision="0" />
<NumericOptions Format="Number" Precision="0"></NumericOptions>
                                                                </label>
                                                                <visualrange autosidemargins="True" />
                                                                <wholerange autosidemargins="True" />

<VisualRange AutoSideMargins="True"></VisualRange>

<WholeRange AutoSideMargins="True"></WholeRange>
                                                            </axisy>
                                                        </cc1:XYDiagram>
                                                    </diagramserializable>
													<seriesserializable>
                                                        <cc1:Series Name="Series 1">
                                                        </cc1:Series>
                                                        <cc1:Series Name="Series 2">
                                                        </cc1:Series>
                                                    </seriesserializable>
												</dxchartsui:WebChartControl>
											</td>
											<td valign="top">
												<dxchartsui:WebChartControl ID="P2PSessionsChart1" runat="server" ClientInstanceName="P2PSessionsChart1"
													CrosshairEnabled="True" Height="200px" Width="400px" OnCustomCallback="P2PSessionsChart1_CustomCallback"
													PaletteName="Oriel">
													<diagramserializable>
                                                        <cc1:XYDiagram>
                                                            <axisx visibleinpanesserializable="-1">
                                                                <visualrange autosidemargins="True" />
                                                                <wholerange autosidemargins="True" />
                                                                <tickmarks minorvisible="False" />
<Tickmarks MinorVisible="False"></Tickmarks>

<VisualRange AutoSideMargins="True"></VisualRange>

<WholeRange AutoSideMargins="True"></WholeRange>
                                                            </axisx>
                                                            <axisy visibleinpanesserializable="-1">
                                                                <tickmarks minorvisible="False" />
<Tickmarks MinorVisible="False"></Tickmarks>

                                                                <label>
                                                                    <numericoptions format="Number" precision="0" />
<NumericOptions Format="Number" Precision="0"></NumericOptions>
                                                                </label>
                                                                <visualrange autosidemargins="True" />
                                                                <wholerange autosidemargins="True" />

<VisualRange AutoSideMargins="True"></VisualRange>

<WholeRange AutoSideMargins="True"></WholeRange>
                                                            </axisy>
                                                        </cc1:XYDiagram>
                                                    </diagramserializable>
													<seriesserializable>
                                                        <cc1:Series Name="Series 1">
                                                        </cc1:Series>
                                                        <cc1:Series Name="Series 2">
                                                        </cc1:Series>
                                                    </seriesserializable>
												</dxchartsui:WebChartControl>
											</td>
											<td valign="top">
												<dxchartsui:WebChartControl ID="AVSessionsChart" runat="server" ClientInstanceName="SyncTypeChart"
													CrosshairEnabled="True" Height="200px" Width="400px" OnCustomCallback="AVSessionsChart_CustomCallback"
													PaletteName="Oriel">
													<diagramserializable>
                                                        <cc1:SimpleDiagram EqualPieSize="False">
                                                        </cc1:SimpleDiagram>
                                                    </diagramserializable>
													<seriesserializable>
                                                        <cc1:Series Name="Series 1">
                                                            <viewserializable>
                                                                <cc1:PieSeriesView RuntimeExploding="False">
                                                                </cc1:PieSeriesView>
                                                            </viewserializable>
                                                            <labelserializable>
                                                                <cc1:PieSeriesLabel>
                                                                    <pointoptionsserializable>
                                                                        <cc1:PiePointOptions>
                                                                            <valuenumericoptions format="Percent" />
<ValueNumericOptions Format="Percent"></ValueNumericOptions>
                                                                        </cc1:PiePointOptions>
                                                                    </pointoptionsserializable>
                                                                </cc1:PieSeriesLabel>
                                                            </labelserializable>
                                                            <legendpointoptionsserializable>
                                                                <cc1:PiePointOptions>
                                                                    <valuenumericoptions format="Percent" />
<ValueNumericOptions Format="Percent"></ValueNumericOptions>
                                                                </cc1:PiePointOptions>
                                                            </legendpointoptionsserializable>
                                                        </cc1:Series>
                                                    </seriesserializable>
													<seriestemplate>
                                                        <viewserializable>
                                                            <cc1:PieSeriesView RuntimeExploding="False">
                                                            </cc1:PieSeriesView>
                                                        </viewserializable>
                                                        <labelserializable>
                                                            <cc1:PieSeriesLabel>
                                                                <pointoptionsserializable>
                                                                    <cc1:PiePointOptions PercentOptions-ValueAsPercent="False">
                                                                        <valuenumericoptions format="Percent" />
<ValueNumericOptions Format="Percent"></ValueNumericOptions>
                                                                    </cc1:PiePointOptions>
                                                                </pointoptionsserializable>
                                                            </cc1:PieSeriesLabel>
                                                        </labelserializable>
                                                        <legendpointoptionsserializable>
                                                            <cc1:PiePointOptions PercentOptions-ValueAsPercent="False">
                                                                <valuenumericoptions format="Percent" />
<ValueNumericOptions Format="Percent"></ValueNumericOptions>
                                                            </cc1:PiePointOptions>
                                                        </legendpointoptionsserializable>
                                                    </seriestemplate>
												</dxchartsui:WebChartControl>
											</td>
										</tr>
										<tr>
											<td>
											</td>
										</tr>
										<tr>
											<td>
											</td>
										</tr>
										<tr>
											<td>
												<dxchartsui:WebChartControl ID="ConfReportChart" runat="server" ClientInstanceName="ConfReportChart"
													CrosshairEnabled="True" Height="200px" Width="400px" OnCustomCallback="ConfReportChart_CustomCallback"
													PaletteName="Oriel">
													<diagramserializable>
                                                        <cc1:SimpleDiagram EqualPieSize="False">
                                                        </cc1:SimpleDiagram>
                                                    </diagramserializable>
													<seriesserializable>
                                                        <cc1:Series Name="Series 1">
                                                            <viewserializable>
                                                                <cc1:PieSeriesView RuntimeExploding="False">
                                                                </cc1:PieSeriesView>
                                                            </viewserializable>
                                                            <labelserializable>
                                                                <cc1:PieSeriesLabel>
                                                                    <pointoptionsserializable>
                                                                        <cc1:PiePointOptions>
                                                                            <valuenumericoptions format="Percent" />
<ValueNumericOptions Format="Percent"></ValueNumericOptions>
                                                                        </cc1:PiePointOptions>
                                                                    </pointoptionsserializable>
                                                                </cc1:PieSeriesLabel>
                                                            </labelserializable>
                                                            <legendpointoptionsserializable>
                                                                <cc1:PiePointOptions>
                                                                    <valuenumericoptions format="Percent" />
<ValueNumericOptions Format="Percent"></ValueNumericOptions>
                                                                </cc1:PiePointOptions>
                                                            </legendpointoptionsserializable>
                                                        </cc1:Series>
                                                    </seriesserializable>
													<seriestemplate>
                                                        <viewserializable>
                                                            <cc1:PieSeriesView RuntimeExploding="False">
                                                            </cc1:PieSeriesView>
                                                        </viewserializable>
                                                        <labelserializable>
                                                            <cc1:PieSeriesLabel>
                                                                <pointoptionsserializable>
                                                                    <cc1:PiePointOptions PercentOptions-ValueAsPercent="False">
                                                                        <valuenumericoptions format="Percent" />
<ValueNumericOptions Format="Percent"></ValueNumericOptions>
                                                                    </cc1:PiePointOptions>
                                                                </pointoptionsserializable>
                                                            </cc1:PieSeriesLabel>
                                                        </labelserializable>
                                                        <legendpointoptionsserializable>
                                                            <cc1:PiePointOptions PercentOptions-ValueAsPercent="False">
                                                                <valuenumericoptions format="Percent" />
<ValueNumericOptions Format="Percent"></ValueNumericOptions>
                                                            </cc1:PiePointOptions>
                                                        </legendpointoptionsserializable>
                                                    </seriestemplate>
												</dxchartsui:WebChartControl>
											</td>
											<td>
											</td>
											<td>
											</td>
										</tr>
									</table>
								</dx:ContentControl>
							</ContentCollection>
						</dx:TabPage>
						<dx:TabPage Text="OneDrive Stats">
							<ContentCollection>
								<dx:ContentControl runat="server">
									<table class="navbarTbl" width="100%">
										<tr>
											<td>
											</td>
											<td>
												&nbsp;
											</td>
										</tr>
										<tr>
											<td>
												&nbsp;
											</td>
											<td>
												&nbsp;
											</td>
										</tr>
									</table>
								</dx:ContentControl>
							</ContentCollection>
						</dx:TabPage>
						<dx:TabPage Text="Service Details">
							<ContentCollection>
								<dx:ContentControl ID="ContentControl1" runat="server">
									<table class="navbarTbl" width="100%">
										<tr>
											<td>
												<dx:ASPxCheckBox ID="chkSingleExpanded" runat="server" Text="Keep a single expanded row at a time"
													AutoPostBack="true" OnCheckedChanged="chkSingleExpanded_CheckedChanged" />
												<br />
												<dx:ASPxGridView ID="servicedetailsgrid" ClientInstanceName="servicedetailsgrid"
													EnableTheming="True" Theme="Office2003Blue" AutoGenerateColumns="false" runat="server"
													KeyFieldName="ServiceID" Width="100%">
													<Columns>
														<dx:GridViewDataColumn FieldName="ServiceID" VisibleIndex="0" Caption="Id" HeaderStyle-CssClass="GridCssHeader"
															CellStyle-CssClass="GridCss" />
														<dx:GridViewDataColumn FieldName="ServiceName" VisibleIndex="1" Caption="ServiceName"
															HeaderStyle-CssClass="GridCssHeader" CellStyle-CssClass="GridCss" />
														<dx:GridViewDataColumn FieldName="Status" VisibleIndex="2" Caption="Status" HeaderStyle-CssClass="GridCssHeader"
															CellStyle-CssClass="GridCss" />
														<dx:GridViewDataColumn FieldName="StartTime" VisibleIndex="3" Caption="StartTime"
															HeaderStyle-CssClass="GridCssHeader" CellStyle-CssClass="GridCss" />
													</Columns>
													<%--							<SettingsBehavior AllowFocusedRow="True" AllowSelectByRowClick="True" ProcessSelectionChangedOnServer="True"
						AllowSelectSingleRowOnly="True" />--%>
													<%--<SettingsPager AlwaysShowPager="True" SEOFriendly="Enabled">
						<PageSizeItemSettings Visible="True">
						</PageSizeItemSettings>
					</SettingsPager>--%>
													<%--<Settings HorizontalScrollBarMode="Auto" />--%>
													<Styles>
														<Header CssClass="GridCssHeader">
														</Header>
														<AlternatingRow CssClass="GridAltRow">
														</AlternatingRow>
														<Cell CssClass="GridCss">
														</Cell>
													</Styles>
													<Templates>
														<DetailRow>
															<div style="color: Blue">
																<b>Message:</b>
																<%--<b>--%>
																<%# Eval("Message")%>
																<%--</b>--%>
																<br />
																<br />
															</div>
														</DetailRow>
													</Templates>
													<SettingsDetail ShowDetailRow="true" />
												</dx:ASPxGridView>
											</td>
										</tr>
									</table>
								</dx:ContentControl>
							</ContentCollection>
						</dx:TabPage>
					 <dx:TabPage Text="Maintenance">
                        <TabImage Url="~/images/icons/wrench.png">
                        </TabImage>
                        <ContentCollection>
                            <dx:ContentControl ID="ContentControl4" runat="server" SupportsDisabledAttribute="True">
                                <asp:UpdatePanel ID="UpdatePanel8" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    <table>
                                                        <tr>
                                                            <td colspan="8">
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td valign="top">
                                                                <dx:ASPxLabel ID="ASPxLabel1" runat="server" CssClass="lblsmallFont" Text="From Date:"
                                                                    Wrap="False">
                                                                </dx:ASPxLabel>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtfromdate" runat="server" Font-Size="12px"></asp:TextBox>
                                                                <cc1:CalendarExtender ID="calextender" runat="server" Format="MM/dd/yyyy" TargetControlID="txtfromdate">
                                                                </cc1:CalendarExtender>
                                                            </td>
                                                            <td valign="top">
                                                                <asp:RequiredFieldValidator ID="rfvtxtfromdate" runat="server" ControlToValidate="txtfromdate"
                                                                    ErrorMessage="Enter From Date" Font-Size="12px" ForeColor="#FF3300"></asp:RequiredFieldValidator>
                                                            </td>
                                                            <td valign="top">
                                                                &nbsp;
                                                            </td>
                                                            <td valign="top">
                                                                <dx:ASPxLabel ID="ASPxLabel2" runat="server" CssClass="lblsmallFont" Text="From Time:"
                                                                    Wrap="False">
                                                                </dx:ASPxLabel>
                                                            </td>
                                                            <td valign="top">
                                                                <dx:ASPxTimeEdit ID="ASPxTimeEdit1" runat="server">
                                                                </dx:ASPxTimeEdit>
                                                            </td>
                                                            <td valign="top">
                                                                &nbsp;
                                                            </td>
                                                            <td valign="top">
                                                                <dx:ASPxButton ID="ClearButton" runat="server" OnClick="ClearButton_Click" Text="Clear"
                                                                    Theme="Office2010Blue" Width="80px">
                                                                </dx:ASPxButton>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td valign="top">
                                                                <dx:ASPxLabel ID="ASPxLabel3" runat="server" CssClass="lblsmallFont" Text="To Date:"
                                                                    Wrap="False">
                                                                </dx:ASPxLabel>
                                                            </td>
                                                            <td valign="top">
                                                                <asp:TextBox ID="txttodate" runat="server" Font-Size="12px"></asp:TextBox>
                                                                <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Format="MM/dd/yyyy" TargetControlID="txttodate">
                                                                </cc1:CalendarExtender>
                                                            </td>
                                                            <td valign="top">
                                                                <asp:RequiredFieldValidator ID="RFvtxttodate" runat="server" ControlToValidate="txttodate"
                                                                    ErrorMessage="Enter To Date" Font-Size="12px" ForeColor="Red"></asp:RequiredFieldValidator>
                                                            </td>
                                                            <td valign="top">
                                                                &nbsp;
                                                            </td>
                                                            <td valign="top">
                                                                <dx:ASPxLabel ID="ASPxLabel4" runat="server" CssClass="lblsmallFont" Text="To Time:"
                                                                    Wrap="False">
                                                                </dx:ASPxLabel>
                                                            </td>
                                                            <td valign="top">
                                                                <dx:ASPxTimeEdit ID="ASPxTimeEdit2" runat="server">
                                                                </dx:ASPxTimeEdit>
                                                            </td>
                                                            <td>
                                                                &nbsp;
                                                            </td>
                                                            <td>
                                                                <dx:ASPxButton ID="btnsearch" runat="server" OnClick="btnsearch_Click" Text="Search"
                                                                    Theme="Office2010Blue" Width="80px">
                                                                </dx:ASPxButton>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="5">
                                                    <dx:ASPxGridView ID="maintenancegrid" runat="server" AutoGenerateColumns="False"
                                                        CssFilePath="~/App_Themes/Office2010Silver/{0}/styles.css" CssPostfix="Office2010Silver"
                                                        Width="100%" KeyFieldName="TypeandName" Theme="Office2003Blue" OnPageSizeChanged="maintenancegrid_PageSizeChanged">
                                                        <SettingsBehavior ColumnResizeMode="NextColumn"></SettingsBehavior>
                                                        <SettingsPager PageSize="50">
                                                            <PageSizeItemSettings Visible="True">
                                                            </PageSizeItemSettings>
                                                        </SettingsPager>
                                                        <Columns>
                                                            <dx:GridViewDataTextColumn Caption="Server Name" VisibleIndex="0" FieldName="servername">
                                                                <Settings AllowDragDrop="True" AutoFilterCondition="Contains" />
                                                                <Settings AllowDragDrop="True" AutoFilterCondition="Contains"></Settings>
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
                                                            <dx:GridViewDataTextColumn Caption="Server Type" FieldName="ServerType" Visible="True"
                                                                VisibleIndex="1">
                                                                <Settings AllowAutoFilter="True" AllowDragDrop="True" AutoFilterCondition="Contains" />
                                                                <Settings AllowDragDrop="True" AllowAutoFilter="True" AutoFilterCondition="Contains">
                                                                </Settings>
                                                                <EditCellStyle CssClass="GridCss">
                                                                </EditCellStyle>
                                                                <EditFormCaptionStyle CssClass="GridCss">
                                                                </EditFormCaptionStyle>
                                                                <HeaderStyle CssClass="GridCssHeader" />
                                                                <CellStyle CssClass="GridCss">
                                                                </CellStyle>
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn Caption="Start Date" VisibleIndex="2" FieldName="StartDate">
                                                                <Settings AllowAutoFilter="False" AllowDragDrop="True" AutoFilterCondition="Contains" />
                                                                <Settings AllowDragDrop="True" AllowAutoFilter="False" AutoFilterCondition="Contains">
                                                                </Settings>
                                                                <EditCellStyle CssClass="GridCss">
                                                                </EditCellStyle>
                                                                <EditFormCaptionStyle CssClass="GridCss">
                                                                </EditFormCaptionStyle>
                                                                <HeaderStyle CssClass="GridCssHeader" />
                                                                <CellStyle CssClass="GridCss">
                                                                </CellStyle>
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn Caption="Start Time" VisibleIndex="3" FieldName="StartTime">
                                                                <Settings AllowAutoFilter="False" AllowDragDrop="True" AutoFilterCondition="Contains" />
                                                                <Settings AllowDragDrop="True" AllowAutoFilter="False" AutoFilterCondition="Contains">
                                                                </Settings>
                                                                <EditCellStyle CssClass="GridCss">
                                                                </EditCellStyle>
                                                                <EditFormCaptionStyle CssClass="GridCss">
                                                                </EditFormCaptionStyle>
                                                                <HeaderStyle CssClass="GridCssHeader" />
                                                                <CellStyle CssClass="GridCss">
                                                                </CellStyle>
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn Caption="Duration" VisibleIndex="4" FieldName="Duration">
                                                                <Settings AllowAutoFilter="False" AllowDragDrop="True" />
                                                                <Settings AllowDragDrop="True" AllowAutoFilter="False"></Settings>
                                                                <EditCellStyle CssClass="GridCss">
                                                                </EditCellStyle>
                                                                <EditFormCaptionStyle CssClass="GridCss">
                                                                </EditFormCaptionStyle>
                                                                <HeaderStyle CssClass="GridCssHeader" />
                                                                <CellStyle CssClass="GridCss">
                                                                </CellStyle>
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn Caption="End Date" VisibleIndex="5" FieldName="EndDate">
                                                                <Settings AllowAutoFilter="False" AllowDragDrop="True" />
                                                                <Settings AllowDragDrop="True" AllowAutoFilter="False"></Settings>
                                                                <EditCellStyle CssClass="GridCss">
                                                                </EditCellStyle>
                                                                <EditFormCaptionStyle CssClass="GridCss">
                                                                </EditFormCaptionStyle>
                                                                <HeaderStyle CssClass="GridCssHeader" />
                                                                <CellStyle CssClass="GridCss">
                                                                </CellStyle>
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn Caption="Maintenance Type" VisibleIndex="6" FieldName="MaintType">
                                                                <Settings AllowAutoFilter="False" AllowDragDrop="True" />
                                                                <Settings AllowDragDrop="True" AllowAutoFilter="False"></Settings>
                                                                <EditCellStyle CssClass="GridCss">
                                                                </EditCellStyle>
                                                                <EditFormCaptionStyle CssClass="GridCss">
                                                                </EditFormCaptionStyle>
                                                                <HeaderStyle CssClass="GridCssHeader" />
                                                                <CellStyle CssClass="GridCss">
                                                                </CellStyle>
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn Caption="Maintenance Days List" VisibleIndex="7" FieldName="MaintDaysList">
                                                                <Settings AllowAutoFilter="False" AllowDragDrop="True" />
                                                                <Settings AllowDragDrop="True" AllowAutoFilter="False"></Settings>
                                                                <EditCellStyle CssClass="GridCss">
                                                                </EditCellStyle>
                                                                <EditFormCaptionStyle CssClass="GridCss">
                                                                </EditFormCaptionStyle>
                                                                <HeaderStyle CssClass="GridCssHeader" />
                                                                <CellStyle CssClass="GridCss">
                                                                </CellStyle>
                                                            </dx:GridViewDataTextColumn>
                                                        </Columns>
                                                        <SettingsBehavior AllowDragDrop="False" AutoExpandAllGroups="True" AllowSelectByRowClick="True"
                                                            ProcessSelectionChangedOnServer="True" />
                                                        <SettingsPager PageSize="50" SEOFriendly="Enabled">
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
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <dx:ASPxPopupControl ID="MsgPopupControl" runat="server" HeaderText="Information"
                                                        Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
                                                        Theme="Glass">
                                                        <ContentCollection>
                                                            <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server" SupportsDisabledAttribute="True">
                                                                <table class="style1">
                                                                    <tr>
                                                                        <td>
                                                                            <dx:ASPxLabel ID="ErrmsgLabel" runat="server">
                                                                            </dx:ASPxLabel>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <dx:ASPxButton ID="OKButton" runat="server" OnClick="OKButton_Click" Text="OK" Theme="Office2010Blue">
                                                                            </dx:ASPxButton>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </dx:PopupControlContentControl>
                                                        </ContentCollection>
                                                    </dx:ASPxPopupControl>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </dx:ContentControl>
                        </ContentCollection>
                    </dx:TabPage>
                    <dx:TabPage Text="Alerts History">
                        <TabImage Url="~/images/icons/sounds.gif">
                        </TabImage>
                        <ContentCollection>
                            <dx:ContentControl ID="ContentControl5" runat="server" SupportsDisabledAttribute="True">
                                <table class="style1" width="100%">
                                    <tr>
                                        <td>
                                            <asp:UpdatePanel ID="UpdatePanel7" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <table width="100%">
                                                        <tr>
                                                            <td>
                                                                <dx:ASPxLabel ID="ASPxLabel9" runat="server" CssClass="lblsmallFont" Text="The list of configured alerts that apply to the server are listed below. The list includes the last 7 days worth of information.">
                                                                </dx:ASPxLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <dx:ASPxGridView ID="AlertGridView" runat="server" AutoGenerateColumns="False" EnableTheming="True"
                                                                    Theme="Office2003Blue" Width="100%" OnPageSizeChanged="AlertGridView_PageSizeChanged">
                                                                    <Columns>
                                                                        <dx:GridViewCommandColumn ShowInCustomizationForm="True" Visible="False" VisibleIndex="0">
                                                                            <ClearFilterButton Visible="True">
                                                                            </ClearFilterButton>
                                                                        </dx:GridViewCommandColumn>
                                                                        <dx:GridViewDataTextColumn Caption="Device Name" FieldName="DeviceName" ShowInCustomizationForm="True"
                                                                            VisibleIndex="1">
                                                                            <Settings AutoFilterCondition="Contains" />
                                                                            <Settings AutoFilterCondition="Contains"></Settings>
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
                                                                        <dx:GridViewDataTextColumn Caption="Device Type" FieldName="DeviceType" ShowInCustomizationForm="True"
                                                                            VisibleIndex="2">
                                                                            <EditCellStyle CssClass="GridCss">
                                                                            </EditCellStyle>
                                                                            <EditFormCaptionStyle CssClass="GridCss">
                                                                            </EditFormCaptionStyle>
                                                                            <HeaderStyle CssClass="GridCssHeader" />
                                                                            <CellStyle CssClass="GridCss">
                                                                            </CellStyle>
                                                                        </dx:GridViewDataTextColumn>
                                                                        <dx:GridViewDataTextColumn Caption="Alert Type" FieldName="AlertType" ShowInCustomizationForm="True"
                                                                            VisibleIndex="3">
                                                                            <EditCellStyle CssClass="GridCss">
                                                                            </EditCellStyle>
                                                                            <EditFormCaptionStyle CssClass="GridCss">
                                                                            </EditFormCaptionStyle>
                                                                            <HeaderStyle CssClass="GridCssHeader" />
                                                                            <CellStyle CssClass="GridCss">
                                                                            </CellStyle>
                                                                        </dx:GridViewDataTextColumn>
                                                                        <dx:GridViewDataTextColumn Caption="Date/Time of Alert" FieldName="DateTimeOfAlert"
                                                                            ShowInCustomizationForm="True" VisibleIndex="4">
                                                                            <EditCellStyle CssClass="GridCss">
                                                                            </EditCellStyle>
                                                                            <EditFormCaptionStyle CssClass="GridCss">
                                                                            </EditFormCaptionStyle>
                                                                            <HeaderStyle CssClass="GridCssHeader" />
                                                                            <CellStyle CssClass="GridCss">
                                                                            </CellStyle>
                                                                        </dx:GridViewDataTextColumn>
                                                                        <%-- <dx:GridViewDataTextColumn Caption="Date/Time Sent" FieldName="DateTimeSent" ShowInCustomizationForm="True"
                                                                            VisibleIndex="5">
                                                                            <EditCellStyle CssClass="GridCss">
                                                                            </EditCellStyle>
                                                                            <EditFormCaptionStyle CssClass="GridCss">
                                                                            </EditFormCaptionStyle>
                                                                            <HeaderStyle CssClass="GridCssHeader" />
                                                                            <CellStyle CssClass="GridCss">
                                                                            </CellStyle>
                                                                        </dx:GridViewDataTextColumn>--%>
                                                                        <dx:GridViewDataTextColumn Caption="Date/Time Alert Cleared" FieldName="DateTimeAlertCleared"
                                                                            ShowInCustomizationForm="True" VisibleIndex="6">
                                                                            <EditCellStyle CssClass="GridCss">
                                                                            </EditCellStyle>
                                                                            <EditFormCaptionStyle CssClass="GridCss">
                                                                            </EditFormCaptionStyle>
                                                                            <HeaderStyle CssClass="GridCssHeader" />
                                                                            <CellStyle CssClass="GridCss">
                                                                            </CellStyle>
                                                                        </dx:GridViewDataTextColumn>
                                                                    </Columns>
                                                                    <SettingsBehavior ColumnResizeMode="NextColumn" />
                                                                    <SettingsBehavior ColumnResizeMode="NextColumn"></SettingsBehavior>
                                                                    <SettingsPager PageSize="50">
                                                                        <PageSizeItemSettings Visible="True">
                                                                        </PageSizeItemSettings>
                                                                    </SettingsPager>
                                                                    <Settings ShowFilterRow="True" ShowGroupPanel="True" />
                                                                    <Settings ShowFilterRow="True" ShowGroupPanel="True"></Settings>
                                                                    <Styles>
                                                                        <AlternatingRow CssClass="GridAltRow">
                                                                        </AlternatingRow>
                                                                    </Styles>
                                                                </dx:ASPxGridView>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                </table>
                            </dx:ContentControl>
                        </ContentCollection>
                    </dx:TabPage>
                    <dx:TabPage Text="Outages">
                        <TabImage Url="../images/icons/exclamation.png">
                        </TabImage>
                        <ContentCollection>
                            <dx:ContentControl ID="ContentControl6" runat="server" SupportsDisabledAttribute="True">
                                <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table class="tableWidth100Percent">
                                            <tr>
                                                <td>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <dx:ASPxGridView ID="OutageGridView" runat="server" AutoGenerateColumns="False" EnableTheming="True"
                                                        Theme="Office2003Blue" Width="100%" OnPageSizeChanged="OutageGridView_PageSizeChanged">
                                                        <Columns>
                                                            <dx:GridViewDataTextColumn Caption="Server Name" FieldName="ServerName" ShowInCustomizationForm="True"
                                                                VisibleIndex="1">
                                                                <Settings AllowAutoFilter="False" AutoFilterCondition="Contains" />
                                                                <Settings AllowAutoFilter="False" AutoFilterCondition="Contains" />
                                                                <EditCellStyle CssClass="GridCss">
                                                                </EditCellStyle>
                                                                <EditFormCaptionStyle CssClass="GridCss">
                                                                </EditFormCaptionStyle>
                                                                <HeaderStyle CssClass="GridCssHeader">
                                                                    <Paddings Padding="5px" />
                                                                    <Paddings Padding="5px" />
                                                                </HeaderStyle>
                                                                <CellStyle CssClass="GridCss">
                                                                </CellStyle>
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn Caption="Date/Time Down" FieldName="DateTimeDown" ShowInCustomizationForm="True"
                                                                VisibleIndex="2">
                                                                <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                                <EditCellStyle CssClass="GridCss">
                                                                </EditCellStyle>
                                                                <EditFormCaptionStyle CssClass="GridCss">
                                                                </EditFormCaptionStyle>
                                                                <HeaderStyle CssClass="GridCssHeader" />
                                                                <CellStyle CssClass="GridCss">
                                                                </CellStyle>
                                                                <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn Caption="Date/Time Up" FieldName="DateTimeUp" ShowInCustomizationForm="True"
                                                                VisibleIndex="3">
                                                                <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                                <EditCellStyle CssClass="GridCss">
                                                                </EditCellStyle>
                                                                <EditFormCaptionStyle CssClass="GridCss">
                                                                </EditFormCaptionStyle>
                                                                <HeaderStyle CssClass="GridCssHeader" />
                                                                <CellStyle CssClass="GridCss">
                                                                </CellStyle>
                                                                <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn Caption="Duration (mins)" FieldName="Description" ShowInCustomizationForm="True"
                                                                VisibleIndex="4">
                                                                <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                                <EditCellStyle CssClass="GridCss">
                                                                </EditCellStyle>
                                                                <EditFormCaptionStyle CssClass="GridCss">
                                                                </EditFormCaptionStyle>
                                                                <HeaderStyle CssClass="GridCssHeader" />
                                                                <CellStyle CssClass="GridCss">
                                                                </CellStyle>
                                                                <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                            </dx:GridViewDataTextColumn>
                                                        </Columns>
                                                        <SettingsBehavior ColumnResizeMode="NextColumn" />
                                                        <SettingsPager PageSize="50">
                                                            <PageSizeItemSettings Visible="True">
                                                            </PageSizeItemSettings>
                                                        </SettingsPager>
                                                        <Settings ShowFilterRow="True" ShowGroupPanel="True" />
                                                        <SettingsBehavior ColumnResizeMode="NextColumn" />
                                                        <Settings ShowFilterRow="True" ShowGroupPanel="True" />
                                                        <Styles>
                                                            <AlternatingRow CssClass="GridAltRow">
                                                            </AlternatingRow>
                                                        </Styles>
                                                    </dx:ASPxGridView>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </dx:ContentControl>
                        </ContentCollection>
                    </dx:TabPage>
					</TabPages>
				</dx:ASPxPageControl>
			</td>
		</tr>
	</table>
</asp:Content>
