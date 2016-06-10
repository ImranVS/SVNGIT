<%@ Page Title="VitalSigns Plus - Network Latency Tests" Language="C#" MasterPageFile="~/DashboardSite.Master"
	AutoEventWireup="true" CodeBehind="LatencyTest.aspx.cs" Inherits="VSWebUI.Dashboard.LatencyTest" %>
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
			//window.open("GraphExchangeHeatMap.aspx?s=" + sourceserver + "&t=" + targetserver, "_self")
			//document.location = window.location + "?s=" + sourceserver + "&t=" + targetserver;
		}
		function OnItemClick(s, e) {
			if (e.item.parent == s.GetRootItem())
				e.processOnServer = false;
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
					Network Latency Tests</div>
			</td>
			<td valign="top" align="right">
                            <dx:ASPxMenu ID="ASPxMenu1" runat="server" EnableTheming="True"  Visible="false"
                    HorizontalAlign="Right" onitemclick="ASPxMenu1_ItemClick" ShowAsToolbar="True"  
                    Theme="Moderno">
                    <ClientSideEvents ItemClick="OnItemClick" />
                    <Items>
                        <dx:MenuItem Name="MainItem">
                            <Items>
                                <dx:MenuItem Name="Edit in Configurator" Text="Edit in Configurator">
                                </dx:MenuItem>
                                
                            </Items>
                            <Image Url="~/images/icons/Gear.png">
                            </Image>
                        </dx:MenuItem>
                    </Items>
                </dx:ASPxMenu>
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
											<td>
												<dx:ASPxComboBox ID="TestNameComboBox" runat="server" OnSelectedIndexChanged='TestnameCombobox_SelectedIndexChanged' AutoPostBack="true" />
											</td>
										</tr>
										<tr>
											<td valign="top">
												<asp:UpdatePanel ID="LatencyServerListUpdatePanel" runat="server" UpdateMode="Conditional">
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
															<p style="line-height: .5em">
															<br />
															<div align="center">
																<asp:Label ID="Label9" runat="server" Text="All measurements are in milliseconds (ms)."></asp:Label>
															</div>
															    <p>
                                                                </p>
                                                                <%--<button type="button" class="close" data-dismiss="alert"><span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
															--%>
															</p>
														</div>
														<dx:ASPxGridView ID="LatencyHeatGridView" runat="server" ClientInstanceName="EXGHeatGridView"
															CssFilePath="~/App_Themes/Office2010Silver/{0}/styles.css" CssPostfix="Office2010Silver"
															Cursor="pointer" SummaryText="m" Theme="Office2003Blue" Width="100%" KeyFieldName="sourceserver"
															OnHtmlDataCellPrepared="LatencyHeatGridView_HtmlDataCellPrepared" >
															<Columns>
															</Columns>
															<Templates>
																<%--  <HeaderCaption>
                        <div class="verticalHeader"><%# Container.DataItem %></div>
                    </HeaderCaption>--%>
															</Templates>
															<SettingsPager Mode=ShowAllRecords />
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
																<Header CssClass="GridCssHeader">
                                                                </Header>
																<GroupRow Font-Bold="True" Font-Italic="False">
																</GroupRow>
																<AlternatingRow CssClass="GridAltRow">
                                                                </AlternatingRow>
																<Cell HorizontalAlign="Center" CssClass="GridCss">
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
