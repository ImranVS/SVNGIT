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
    <script src="../js/jquery-1.9.1.js" type="text/javascript"></script>

	<script src="../js/bootstrap.min.js" type="text/javascript"></script>

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
	<table >
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
					Width="100%" EnableCallBacks="False" 
                    onpagesizechanged="Office365Groupsgrid_PageSizeChanged">
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
        <tr>
        <td>
          <dx:ASPxRoundPanel ID="DiskSettingsRoundPanel" runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
																CssPostfix="Glass" GroupBoxCaptionOffsetY="-24px" HeaderText="Compare Two Users"
																Height="50px" SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" Width="100%">
																<ContentPaddings Padding="2px" PaddingBottom="10px" PaddingLeft="4px" PaddingTop="10px" />
																<ContentPaddings Padding="2px" PaddingLeft="4px" PaddingTop="10px" PaddingBottom="10px">
																</ContentPaddings>
																<HeaderStyle Height="23px"></HeaderStyle>
																<PanelCollection>
																	<dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">
      <table>
                                                    <tr>
                                                        <td colspan="2">
                                                            <div id="infoDiv" class="info">To compare users' community memberships, select two distinct users below and click Compare.</div>
                                                            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <div id="errorDiv" class="alert alert-danger" style="display:none" runat="server"></div>
                                                                </ContentTemplate>
                                                                <Triggers>
                                                                    <asp:AsyncPostBackTrigger ControlID="CompareUsersButton" />
                                                                </Triggers>
                                                            </asp:UpdatePanel>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td valign="top">
                                                            <table>
                                                                <tr>
                                                        <td>
                                                            <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="User 1:" CssClass="lblsmallFont">
                                                            </dx:ASPxLabel>
                                                        </td>
                                                    </tr>
                                                                <tr>
                                                        <td>
                                                            <dx:ASPxComboBox ID="User1ComboBox" ClientInstanceName="User1ComboBox" runat="server"
                                                                ValueType="System.String">
                                                                <ValidationSettings ErrorDisplayMode="ImageWithTooltip"  >
                                                                    <RequiredField IsRequired="True" ErrorText="Please Select User 1" />
                                                                </ValidationSettings>
                                                            </dx:ASPxComboBox>
                                                        </td>
                                                    </tr>
                                                                <tr>
                                                        <td>
                                                            <dx:ASPxLabel ID="ASPxLabel5" runat="server" Text="User 2:" CssClass="lblsmallFont">
                                                            </dx:ASPxLabel>
                                                        </td>
                                                    </tr>
                                                                <tr>
                                                        <td>
                                                            <dx:ASPxComboBox ID="User2ComboBox" ClientInstanceName="User2ComboBox" runat="server" ValueType="System.String">
                                                              <ValidationSettings ErrorDisplayMode="ImageWithTooltip" >
                                                                    <RequiredField IsRequired="True" ErrorText="Please Select User 2" />
                                                                </ValidationSettings>
                                                            </dx:ASPxComboBox>
                                                        </td>
                                                    </tr>
                                                                <tr>
                                                        <td>
                                                            <dx:ASPxButton ID="CompareUsersButton" runat="server" Text="Compare" 
                                                                CssClass="sysButton" OnClick="CompareUsersButton_Click">
                                                            </dx:ASPxButton>
                                                        </td>
                                                    </tr>
                                                            </table>
                                                        </td>
                                                        <td valign="top">
                                                           <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <dx:ASPxGridView ID="CompareUsersGrid" runat="server" 
                                                                        AutoGenerateColumns="False" ClientVisible="False" EnableTheming="True" 
                                                                        Theme="Office2003Blue" KeyFieldName="ID" Width="100px"  OnPageSizeChanged = "CompareUsersGrid_OnPageSizeChanged">
                                                                        <Columns>
                                                                            <dx:GridViewDataTextColumn Caption="Category" FieldName="Category" 
                                                                                ShowInCustomizationForm="True" VisibleIndex="0">
                                                                            </dx:GridViewDataTextColumn>
                                                                            <dx:GridViewDataTextColumn Caption="Group Name" FieldName="GroupName" 
                                                                                ShowInCustomizationForm="True" VisibleIndex="1">
                                                                                    </dx:GridViewDataTextColumn>
                                                                                  <dx:GridViewDataTextColumn Caption="Group Type" FieldName="GroupType" 
                                                                                ShowInCustomizationForm="True" VisibleIndex="2">
                                                                            </dx:GridViewDataTextColumn>
                                                                            <dx:GridViewDataTextColumn Caption="ID" FieldName="ID" 
                                                                                ShowInCustomizationForm="True" Visible="False" VisibleIndex="2">
                                                                            </dx:GridViewDataTextColumn>
                                                                            <dx:GridViewDataTextColumn Caption="ParentID" FieldName="ParentID" 
                                                                                ShowInCustomizationForm="True" Visible="False" VisibleIndex="3">
                                                                            </dx:GridViewDataTextColumn>
                                                                        </Columns>
                                                                        <SettingsBehavior AutoExpandAllGroups="True"  />
                                                                        <SettingsBehavior AllowSelectByRowClick="True" ProcessSelectionChangedOnServer="True"
                                                                            ColumnResizeMode="NextColumn"></SettingsBehavior>
                                                                        <SettingsPager AlwaysShowPager="True">
                                                                            <PageSizeItemSettings Visible="True">
                                                                            </PageSizeItemSettings>
                                                                        </SettingsPager>
                                                                        <Settings ShowFilterRow="True"  ShowGroupPanel="True"  />
                                                                        <Settings GroupFormat="{1}" ShowColumnHeaders="true" />
                                                                        <Styles>
                                                                            <AlternatingRow CssClass="GridAltRow">
                                                                            </AlternatingRow>
                                                                            <Header CssClass="GridCssHeader">
                                                                            </Header>
                                                                            <Cell CssClass="GridCss">
                                                                            </Cell>
                                                                        </Styles>
                                                                    </dx:ASPxGridView>
                                                                </ContentTemplate>
                                                                <Triggers>
                                                                    <asp:AsyncPostBackTrigger ControlID="CompareUsersButton" />
                                                                </Triggers>
                                                            </asp:UpdatePanel>
                                                        </td>
                                                    </tr>   
                                                </table>
                                                	</dx:PanelContent>
																</PanelCollection>
															</dx:ASPxRoundPanel>
        </td>
        </tr>
	</table>
  
</asp:Content>
