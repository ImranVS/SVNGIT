<%@ Page Language="C#" Title="VitalSigns Plus - Office 365 Groups" MasterPageFile="~/DashboardSite.Master" AutoEventWireup="true"
	CodeBehind="Office365Groups.aspx.cs" Inherits="VSWebUI.Dashboard.Office365Groups" %>

<%@ MasterType VirtualPath="~/DashboardSite.Master" %>
<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.XtraCharts.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.XtraCharts" TagPrefix="cc1" %>
<%@ Register Assembly="DevExpress.XtraCharts.v14.2.Web, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.XtraCharts.Web" TagPrefix="dxchartsui" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
	<table>
		<tr>
        <td>
							<img alt="" src="../images/icons/O365.png" />
						</td>
                        <td>
							<div class="header" id="servernamelbldisp" runat="server">
								Office 365 Groups</div>
						</td>
		</tr>
	</table>
	<table width="100%">
        <tr>
            <td>
                <dxchartsui:WebChartControl ID="GroupsWebChart" runat="server" 
                    CrosshairEnabled="True" Height="300px" PaletteName="Module" Width="1300px">
                    <diagramserializable>
                        <cc1:SimpleDiagram3D RotationMatrixSerializable="1;0;0;0;0;0.5;-0.866025403784439;0;0;0.866025403784439;0.5;0;0;0;0;1">
                        </cc1:SimpleDiagram3D>
                    </diagramserializable>
                    <legend alignmenthorizontal="Center" alignmentvertical="BottomOutside" 
                        maxverticalpercentage="30" Direction="LeftToRight" 
                        EquallySpacedItems="False">
                        <border visibility="False" />
                    </legend>
                    <seriesserializable>
                        <cc1:Series Name="Series 1">
                            <viewserializable>
                                <cc1:Pie3DSeriesView>
                                    <titles>
                                        <cc1:SeriesTitle />
                                    </titles>
                                </cc1:Pie3DSeriesView>
                            </viewserializable>
                            <labelserializable>
                                <cc1:Pie3DSeriesLabel TextPattern="{A}: {V}" Position="TwoColumns">
                                </cc1:Pie3DSeriesLabel>
                            </labelserializable>
                        </cc1:Series>
                    </seriesserializable>
                    <seriestemplate argumentscaletype="Qualitative">
                        <viewserializable>
                            <cc1:Pie3DSeriesView>
                            </cc1:Pie3DSeriesView>
                        </viewserializable>
                        <labelserializable>
                            <cc1:Pie3DSeriesLabel Position="Inside">
                            </cc1:Pie3DSeriesLabel>
                        </labelserializable>
                    </seriestemplate>
                    <titles>
                        <cc1:ChartTitle Text="Groups" />
                    </titles>
                </dxchartsui:WebChartControl>
            </td>
        </tr>
		<tr>
			<td>
				<dx:ASPxGridView ID="Office365Groupsgrid" runat="server" AutoGenerateColumns="False"
					Cursor="pointer" EnableTheming="True" KeyFieldName="GroupId" Theme="Office2003Blue"
					Width="100%" EnableCallBacks="False">
					<%--	<ClientSideEvents ContextMenu="UsersGrid_ContextMenu"></ClientSideEvents>--%>
					<SettingsBehavior AllowFocusedRow="true" />
					<SettingsBehavior AllowFocusedRow="True" AllowSelectByRowClick="True" ColumnResizeMode="NextColumn" />
					<GroupSummary>
						<dx:ASPxSummaryItem FieldName="GroupName" SummaryType="Count" />
					</GroupSummary>
					<Columns>
						<dx:GridViewDataTextColumn Caption="Group Name" FieldName="GroupName" FixedStyle="Left"
							ShowInCustomizationForm="True" VisibleIndex="1">
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
						<dx:GridViewDataTextColumn Caption="Member Name" FieldName="DisplayName" ShowInCustomizationForm="True"
							VisibleIndex="2">
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
						<dx:GridViewDataTextColumn Caption="User Principal Name" FieldName="UserPrincipalName"
							ShowInCustomizationForm="True" VisibleIndex="3">
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
						<dx:GridViewDataTextColumn Caption="Group Type" FieldName="GroupType" 
							ShowInCustomizationForm="True" VisibleIndex="4">
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
						<dx:GridViewDataTextColumn Caption="Account Name" FieldName="AccountName" ShowInCustomizationForm="True"
							VisibleIndex="5">
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
						<dx:GridViewDataTextColumn Caption="GroupId" FieldName="GroupId" ShowInCustomizationForm="True"
							Visible="False" VisibleIndex="6">
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
					<SettingsBehavior AllowSelectByRowClick="True" ProcessSelectionChangedOnServer="True"
						ColumnResizeMode="NextColumn"></SettingsBehavior>
					<SettingsPager AlwaysShowPager="True" NumericButtonCount="30" PageSize="50">
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
</asp:Content>
