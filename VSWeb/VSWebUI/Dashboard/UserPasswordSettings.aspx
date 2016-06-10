<%@ Page Title="" Language="C#" MasterPageFile="~/DashboardSite.Master" AutoEventWireup="true" CodeBehind="UserPasswordSettings.aspx.cs" Inherits="VSWebUI.Dashboard.UserPasswordSettings" %>
<%@ MasterType VirtualPath="~/DashboardSite.Master" %>
<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.XtraCharts.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.XtraCharts" TagPrefix="cc1" %>
<%@ Register Assembly="DevExpress.XtraCharts.v14.2.Web, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.XtraCharts.Web" TagPrefix="dxchartsui" %>
	<%@ Register Assembly="DevExpress.XtraCharts.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.XtraCharts" TagPrefix="cc1" %>
<%@ Register Assembly="DevExpress.XtraCharts.v14.2.Web, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.XtraCharts.Web" TagPrefix="dxchartsui" %>




<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<%@ Register Assembly="DevExpress.XtraCharts.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.XtraCharts" TagPrefix="cc2" %>




<%@ Register Assembly="DevExpress.Web.ASPxGauges.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxGauges" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxGauges.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxGauges.Gauges" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxGauges.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxGauges.Gauges.Linear" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxGauges.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxGauges.Gauges.Circular" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxGauges.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxGauges.Gauges.State" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxGauges.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxGauges.Gauges.Digital" TagPrefix="dx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
	<table>
					<tr>
						<td>
							<img alt="" src="../images/icons/group.png" />
						</td>
						<td>
							<div class="header" id="servernamelbldisp" runat="server">
								User Password Settings </div>
						</td>
					</tr>
					</table>
					<table>
					<tr>
					 <td valign="top">
                <dx:ASPxPanel ID="ASPxPanel1" runat="server" Theme="MetropolisBlue" Width="100%"
                    BackColor="White">
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <table width="99%">
                                <tr>
                                    <td align="center" width="33%">
									<dxchartsui:WebChartControl ID="strongpasswordWebChart" runat="server" ClientInstanceName="strongpasswordWebChart" OnCustomDrawSeriesPoint="strongpasswordWebChart_CustomDrawSeriesPoint"
											Height="300px" Width="400px" CrosshairEnabled="True">
											<diagramserializable>
                                                    <cc1:SimpleDiagram>
                                                    </cc1:SimpleDiagram>
                                                </diagramserializable>
											<fillstyle>
                                                    <optionsserializable>
                                                        <cc1:SolidFillOptions />
                                                    </optionsserializable></fillstyle>
											<seriesserializable>
                                                    <cc1:Series Name="Series 1">
                                                        <viewserializable>
                                                            <cc1:PieSeriesView RuntimeExploding="False">
                                                            </cc1:PieSeriesView>
                                                        </viewserializable><labelserializable>
                                                            <cc1:PieSeriesLabel LineVisible="True">
                                                                <fillstyle>
                                                                    <optionsserializable>
                                                                        <cc1:SolidFillOptions />
                                                                    </optionsserializable>
                                                                </fillstyle>
                                                                <pointoptionsserializable>
                                                                    <cc1:PiePointOptions>
                                                                    </cc1:PiePointOptions>
                                                                </pointoptionsserializable>
                                                            </cc1:PieSeriesLabel>
                                                        </labelserializable><legendpointoptionsserializable>
                                                            <cc1:PiePointOptions>
                                                            </cc1:PiePointOptions>
                                                        </legendpointoptionsserializable></cc1:Series></seriesserializable>
											<seriestemplate>
                                                    <viewserializable>
                                                        <cc1:PieSeriesView RuntimeExploding="False">
                                                        </cc1:PieSeriesView>
                                                    </viewserializable><labelserializable>
                                                        <cc1:PieSeriesLabel LineVisible="True">
                                                            <fillstyle>
                                                                <optionsserializable>
                                                                    <cc1:SolidFillOptions />
                                                                </optionsserializable>
                                                            </fillstyle>
                                                            <pointoptionsserializable>
                                                                <cc1:PiePointOptions PercentOptions-ValueAsPercent="False">
                                                                </cc1:PiePointOptions>
                                                            </pointoptionsserializable>
                                                        </cc1:PieSeriesLabel>
                                                    </labelserializable><legendpointoptionsserializable>
                                                        <cc1:PiePointOptions PercentOptions-ValueAsPercent="False">
                                                        </cc1:PiePointOptions>
                                                    </legendpointoptionsserializable></seriestemplate>
											<crosshairoptions>
                                                    <commonlabelpositionserializable>
                                                        <cc1:CrosshairMousePosition />
                                                    </commonlabelpositionserializable></crosshairoptions>
											<tooltipoptions>
                                                    <tooltippositionserializable>
                                                        <cc1:ToolTipMousePosition />
                                                    </tooltippositionserializable></tooltipoptions>
										</dxchartsui:WebChartControl>
                                     
                                    </td>
                                    <td align="center" width="33%">
                                      <dxchartsui:WebChartControl ID="Passwordneverexpires" runat="server" ClientInstanceName="Passwordneverexpires" OnCustomDrawSeriesPoint="Passwordneverexpires_CustomDrawSeriesPoint"
											Height="300px" Width="400px" CrosshairEnabled="True">
											<diagramserializable>
                                                    <cc1:SimpleDiagram>
                                                    </cc1:SimpleDiagram>
                                                </diagramserializable>
											<fillstyle>
                                                    <optionsserializable>
                                                        <cc1:SolidFillOptions />
                                                    </optionsserializable></fillstyle>
											<seriesserializable>
                                                    <cc1:Series Name="Series 1">
                                                        <viewserializable>
                                                            <cc1:PieSeriesView RuntimeExploding="False">
                                                            </cc1:PieSeriesView>
                                                        </viewserializable><labelserializable>
                                                            <cc1:PieSeriesLabel LineVisible="True">
                                                                <fillstyle>
                                                                    <optionsserializable>
                                                                        <cc1:SolidFillOptions />
                                                                    </optionsserializable>
                                                                </fillstyle>
                                                                <pointoptionsserializable>
                                                                    <cc1:PiePointOptions>
                                                                    </cc1:PiePointOptions>
                                                                </pointoptionsserializable>
                                                            </cc1:PieSeriesLabel>
                                                        </labelserializable><legendpointoptionsserializable>
                                                            <cc1:PiePointOptions>
                                                            </cc1:PiePointOptions>
                                                        </legendpointoptionsserializable></cc1:Series></seriesserializable>
											<seriestemplate>
                                                    <viewserializable>
                                                        <cc1:PieSeriesView RuntimeExploding="False">
                                                        </cc1:PieSeriesView>
                                                    </viewserializable><labelserializable>
                                                        <cc1:PieSeriesLabel LineVisible="True">
                                                            <fillstyle>
                                                                <optionsserializable>
                                                                    <cc1:SolidFillOptions />
                                                                </optionsserializable>
                                                            </fillstyle>
                                                            <pointoptionsserializable>
                                                                <cc1:PiePointOptions PercentOptions-ValueAsPercent="False">
                                                                </cc1:PiePointOptions>
                                                            </pointoptionsserializable>
                                                        </cc1:PieSeriesLabel>
                                                    </labelserializable><legendpointoptionsserializable>
                                                        <cc1:PiePointOptions PercentOptions-ValueAsPercent="False">
                                                        </cc1:PiePointOptions>
                                                    </legendpointoptionsserializable></seriestemplate>
											<crosshairoptions>
                                                    <commonlabelpositionserializable>
                                                        <cc1:CrosshairMousePosition />
                                                    </commonlabelpositionserializable></crosshairoptions>
											<tooltipoptions>
                                                    <tooltippositionserializable>
                                                        <cc1:ToolTipMousePosition />
                                                    </tooltippositionserializable></tooltipoptions>
										</dxchartsui:WebChartControl>
                                    </td>
                                </tr>
                            </table>
                        </dx:PanelContent>
                    </PanelCollection>
                    <Border BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" />
                </dx:ASPxPanel>
            </td>
			</tr>
				</table>
				<table>
				<tr>
				<td>
				<table width="53%">
				<tr>
				<td>
				<dx:ASPxGridView ID="O365Usersettinggrid" runat="server" AutoGenerateColumns="False"
					Cursor="pointer" EnableTheming="True" KeyFieldName="ServerId" Theme="Office2003Blue" 
					Width="100%" EnableCallBacks="False">
					<%--	<ClientSideEvents ContextMenu="UsersGrid_ContextMenu"></ClientSideEvents>--%>
					<SettingsBehavior AllowSelectByRowClick="True" ProcessSelectionChangedOnServer="True"
						ColumnResizeMode="NextColumn"></SettingsBehavior>
					<SettingsBehavior AllowFocusedRow="true" />
					<Columns>
						<dx:GridViewDataTextColumn Caption="Member Name" FieldName="DisplayName" FixedStyle="Left"
							ShowInCustomizationForm="True" VisibleIndex="1" Width="190px">
							<PropertiesTextEdit>
								<FocusedStyle HorizontalAlign="Center">
								</FocusedStyle>
							</PropertiesTextEdit>
							<Settings AllowAutoFilter="True" AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
							<EditCellStyle CssClass="GridCss">
							</EditCellStyle>
							<EditFormCaptionStyle CssClass="GridCss">
							</EditFormCaptionStyle>
							<HeaderStyle CssClass="GridCssHeader" />
							<CellStyle CssClass="GridCss" HorizontalAlign="Center">
							</CellStyle>
						</dx:GridViewDataTextColumn>
						<dx:GridViewDataTextColumn Caption="UserPrincipalName" FieldName="UserPrincipalName"
							ShowInCustomizationForm="True" VisibleIndex="2" Width="200px">
							<PropertiesTextEdit>
								<FocusedStyle HorizontalAlign="Center">
								</FocusedStyle>
							</PropertiesTextEdit>
							<Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
							<EditCellStyle CssClass="GridCss">
							</EditCellStyle>
							<EditFormCaptionStyle CssClass="GridCss">
							</EditFormCaptionStyle>
							<HeaderStyle CssClass="GridCssHeader" />
							<CellStyle CssClass="GridCss" HorizontalAlign="Center">
							</CellStyle>
						</dx:GridViewDataTextColumn>
						<dx:GridViewDataTextColumn Caption="Strong Password Required" FieldName="StrongPasswordRequired" 
							ShowInCustomizationForm="True" VisibleIndex="3" Width="140px">
							<PropertiesTextEdit>
								<FocusedStyle HorizontalAlign="Center">
								</FocusedStyle>
							</PropertiesTextEdit>
							<Settings  AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
							<EditCellStyle CssClass="GridCss">
							</EditCellStyle>
							<EditFormCaptionStyle CssClass="GridCss">
							</EditFormCaptionStyle>
							<HeaderStyle CssClass="GridCssHeader" />
							<CellStyle CssClass="GridCss" HorizontalAlign="Center">
							</CellStyle>
						</dx:GridViewDataTextColumn>
						<dx:GridViewDataTextColumn Caption="Password Never Expires" FieldName="PasswordNeverExpires" ShowInCustomizationForm="True"
							VisibleIndex="4" Width="140px">
							<PropertiesTextEdit>
								<FocusedStyle HorizontalAlign="Center">
								</FocusedStyle>
							</PropertiesTextEdit>
							<Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
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
							<Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
							<EditCellStyle CssClass="GridCss">
							</EditCellStyle>
							<EditFormCaptionStyle CssClass="GridCss">
							</EditFormCaptionStyle>
							<HeaderStyle CssClass="GridCssHeader" />
							<CellStyle CssClass="GridCss" HorizontalAlign="Center">
							</CellStyle>
						</dx:GridViewDataTextColumn>
					</Columns>
					<SettingsBehavior AllowFocusedRow="True" AllowSelectByRowClick="True" ColumnResizeMode="NextColumn" />
					<SettingsPager AlwaysShowPager="True" NumericButtonCount="30" PageSize="10">
						<PageSizeItemSettings Items="10,20,50, 100, 200" Visible="True">
						</PageSizeItemSettings>
					</SettingsPager>
					<Settings ShowFilterRow="True" ShowHorizontalScrollBar="false" ShowGroupPanel="True" />
					<Styles>
						<Header VerticalAlign="Middle">
						</Header>
						<AlternatingRow CssClass="GridAltRow" Enabled="True">
						</AlternatingRow>
					</Styles>
				
				</dx:ASPxGridView>
				</td>
				</tr>
				</table>
				
			</td>
		</tr>
	</table>
</asp:Content>
