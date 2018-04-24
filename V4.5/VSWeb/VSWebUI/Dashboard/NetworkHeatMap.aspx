<%@ Page Title="VitalSigns Plus - Overall Skype for Business Stats" Language="C#" MasterPageFile="~/DashboardSite.Master"
	AutoEventWireup="true" CodeBehind="NetworkHeatMap.aspx.cs" Inherits="VSWebUI.Dashboard.NetworkHeatMap" %>
	<%@ MasterType virtualpath="~/DashboardSite.Master" %>
<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.Web" TagPrefix="dx" %>





	<%@ Register Assembly="DevExpress.XtraCharts.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.XtraCharts" TagPrefix="cc1" %>



<%@ Register Assembly="DevExpress.XtraCharts.v14.2.Web, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.XtraCharts.Web" TagPrefix="dxchartsui" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
	<script type="text/javascript" src="../js/jquery-1.9.1.js"></script>
	<script type="text/javascript" src="../js/jquery-ui-1.10.3.custom.js"></script>
	<script type="text/javascript" src="../js/jquery-ui-1.10.3.custom.min.js"></script>
	<link rel="stylesheet" type="text/css" media="screen" href="../css/jquery-ui-1.10.3.custom.css" />
	<link rel="stylesheet" type="text/css" media="screen" href="../css/jquery-ui-1.10.3.custom.min.css" />
	<script type="text/javascript">
		function onCellClick(sourceserver, targetserver) {
			//alert(sourceserver + "," + targetserver);
			//EXGHeatGridView.PerformCallback(sourceserver + "|" + targetserver);
			//alert(window.location + "?s=" + sourceserver + "&t=" + targetserver);
			window.open("GraphExchangeHeatMap.aspx?s=" + sourceserver + "&t=" + targetserver, "_self")
			//document.location = window.location + "?s=" + sourceserver + "&t=" + targetserver;
		}
		
	</script>
	<style type="text/css">
		.verticalHeader
		{
			width: 100px;
			padding-top: 50px;
			padding-bottom: 40px;
			background-color: #dcdcdc;
			-webkit-transform: rotate(90deg);
			-moz-transform: rotate(90deg);
			-ms-transform: rotate(-90deg);
			-o-transform: rotate(90deg);
			transform: rotate(-90deg);
			-webkit-transform-origin: 50% 50%;
			-moz-transform-origin: 50% 50%;
			-ms-transform-origin: 50% 50%;
			-o-transform-origin: 50% 50%;
			transform-origin: 50% 50%;
			filter: progid:DXImageTransform.Microsoft.BasicImage(rotation=1);
		}
	</style>
	<table width="100%">
		<tr>
			<td>
				<div class="header" id="servernamelbldisp" runat="server">
					Exchange Server Mail Latency Heat Map</div>
			</td>
			<td>
				&nbsp;
			</td>
		</tr>
	</table>
	<table width="100%">
		<tr>
			<td>
				<table class="navbarTbl">
					<tr>
						<td>
							<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
								<ContentTemplate>
									<table>
										<tr>
											<td valign="top">
												<asp:UpdatePanel ID="EXGServerListUpdatePanel" runat="server" UpdateMode="Conditional">
													<ContentTemplate>
														<div id="successDiv" class="info1" runat="server">
															<asp:Label ID="Label1" runat="server" BackColor="Red" ForeColor="Red" Text="....."></asp:Label>
															<asp:Label ID="Label2" runat="server" Text="Above Red Threshold"></asp:Label>
															&nbsp;<asp:Label ID="Label3" runat="server" BackColor="Yellow" ForeColor="Yellow"
																Text="....."></asp:Label>
															<asp:Label ID="Label4" runat="server" Text="  Above Yellow Threshold "></asp:Label>
															&nbsp;<asp:Label ID="Label5" runat="server" BackColor="Green" ForeColor="Green" Text="....."></asp:Label>
															<asp:Label ID="Label6" runat="server" Text="Below Yellow Threshold"></asp:Label>
															&nbsp;<asp:Label ID="Label7" runat="server" BackColor="Gray" ForeColor="Gray" Text="....."></asp:Label>
															<asp:Label ID="Label8" runat="server" Text="Not Responding"></asp:Label>
															&nbsp;
															<%--<button type="button" class="close" data-dismiss="alert"><span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
															--%>
														</div>
														<dx:ASPxGridView ID="EXGHeatGridView" runat="server" ClientInstanceName="EXGHeatGridView"
															CssFilePath="~/App_Themes/Office2010Silver/{0}/styles.css" CssPostfix="Office2010Silver"
															Cursor="pointer" SummaryText="m" Theme="Office2010Blue" Width="100%" KeyFieldName="sourceserver"
															OnHtmlDataCellPrepared="EXGHeatGridView_HtmlDataCellPrepared" >
															<Columns>
															</Columns>
															<Templates>
																<%--  <HeaderCaption>
                        <div class="verticalHeader"><%# Container.DataItem %></div>
                    </HeaderCaption>--%>
															</Templates>
															<SettingsBehavior AllowDragDrop="False" AllowFocusedRow="false" AllowSelectByRowClick="True"
																AutoExpandAllGroups="True" ProcessSelectionChangedOnServer="True" />
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
															<Styles>
																<GroupRow Font-Bold="True" Font-Italic="False">
																</GroupRow>
																<Cell HorizontalAlign="Center">
																</Cell>
																<GroupFooter Font-Bold="True">
																</GroupFooter>
																<GroupPanel Font-Bold="False">
																</GroupPanel>
																<LoadingPanel ImageSpacing="5px">
																</LoadingPanel>
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
								</ContentTemplate>
							</asp:UpdatePanel>
						</td>
					</tr>
				</table>
			</td>
		</tr>
	</table>
</asp:Content>
