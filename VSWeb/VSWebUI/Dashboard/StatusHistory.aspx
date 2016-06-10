<%@ Page Title="Status Changes" Language="C#" MasterPageFile="~/DashboardSite.Master"
	AutoEventWireup="true" CodeBehind="StatusHistory.aspx.cs" Inherits="VSWebUI.Dashboard.StatusHistory" %>

<%@ MasterType VirtualPath="~/DashboardSite.master" %>
<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.Web" TagPrefix="dx" %>
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





<%@ Register Src="../Controls/MailStatus.ascx" TagName="MailStatus" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href='http://fonts.googleapis.com/css?family=Francois One' rel='stylesheet' type='text/css' />
    <link rel="stylesheet" type="text/css" href="css/vswebforms.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
	<table width="100%">
		<tr>
			<td>
                <div class="header" id="servernamelbldisp" runat="server">Status Changes</div>
			</td>
		</tr>

		<tr>
            <td>
                <div class="info">This page shows the most recent changes to the status of a given server.  The servers with status changes within the threshold value 
				set in the User Preferences page will be designated by the red exclamation icon - <img alt="" id="img" runat="server" src='~/images/icons/exclamation.png' />.
                </div>
            </td>
        </tr>

		<tr>
			<td id="gridCell" colspan="5">
				<asp:UpdatePanel ID="updatepan1" runat="server" UpdateMode="Conditional">
					<ContentTemplate>
						<dx:ASPxGridView ID="HistoryGridView" runat="server" AutoGenerateColumns="False"
							CssFilePath="~/App_Themes/Office2010Silver/{0}/styles.css" CssPostfix="Office2010Silver"
							Width="100%" KeyFieldName="TypeANDName" Cursor="pointer" OnHtmlDataCellPrepared="HistoryGridView_HtmlDataCellPrepared" OnPageSizeChanged="HistoryGridView_OnPageSizeChanged"
							EnableTheming="True" Theme="Office2003Blue">
							<SettingsBehavior AllowSelectByRowClick="True" ProcessSelectionChangedOnServer="True"
								ColumnResizeMode="NextColumn"></SettingsBehavior>
							<SettingsBehavior AllowFocusedRow="true" />
							<SettingsPager PageSize="20">
								<PageSizeItemSettings Visible="True">
								</PageSizeItemSettings>
							</SettingsPager>
							<Columns>
								<dx:GridViewDataColumn Caption="" VisibleIndex="0" Width="30px">
									<DataItemTemplate>
										<img id="img" runat="server" src='<%# GetImage(Eval("LastStatusChange")) %>' />
									</DataItemTemplate>
								</dx:GridViewDataColumn>
								<dx:GridViewDataTextColumn Caption="Name" VisibleIndex="1" FieldName="Name" Width="300px">
									<PropertiesTextEdit>
										<Style HorizontalAlign="Center">
											
										</Style>
									</PropertiesTextEdit>
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
									<FooterCellStyle ForeColor="#FF3300">
									</FooterCellStyle>
									<GroupFooterCellStyle Font-Names="Arial Black">
									</GroupFooterCellStyle>
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataTextColumn Caption="Current Status" VisibleIndex="3" FieldName="NewStatus" Width="200px">
									<DataItemTemplate>
										<asp:Label ID="hfNameLabel" Text='<%# Eval("NewStatus") %>' runat="server" />
										<asp:Label ID="hfNameLabel2" Text='<%# Eval("NewStatusCode") %>' runat="server" Visible="false" />
									</DataItemTemplate>
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
									<CellStyle CssClass="GridCss1">
									</CellStyle>
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataTextColumn Caption="Current Details" VisibleIndex="4" FieldName="Details">
									<Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
									<EditCellStyle CssClass="GridCss">
									</EditCellStyle>
									<EditFormCaptionStyle CssClass="GridCss">
									</EditFormCaptionStyle>
									<HeaderStyle CssClass="GridCssHeader" />
									<CellStyle CssClass="GridCss" Wrap="False">
									</CellStyle>
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataTextColumn Caption="Previous Status" VisibleIndex="5" FieldName="OldStatus" Width="200px">
									<DataItemTemplate>
										<asp:Label ID="hfNameLabel" Text='<%# Eval("OldStatus") %>' runat="server" />
										<asp:Label ID="hfNameLabel2" Text='<%# Eval("OldStatusCode") %>' runat="server" Visible="false" />
									</DataItemTemplate>
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
									<CellStyle CssClass="GridCss1">
									</CellStyle>
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataTextColumn Caption="Type" FieldName="Type" Visible="True" VisibleIndex="6"
									Width="100px">
									<Settings AutoFilterCondition="Contains"  AllowAutoFilter="True" AllowAutoFilterTextInputTimer="False"  />
									<EditCellStyle CssClass="GridCss">
									</EditCellStyle>
									<EditFormCaptionStyle CssClass="GridCss">
									</EditFormCaptionStyle>
									<HeaderStyle CssClass="GridCssHeader" />
									<CellStyle CssClass="GridCss">
									</CellStyle>
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataTextColumn Caption="Location" FieldName="Location" Visible="True"
									VisibleIndex="7" Width="100px">
									<Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
									<EditCellStyle CssClass="GridCss">
									</EditCellStyle>
									<EditFormCaptionStyle CssClass="GridCss">
									</EditFormCaptionStyle>
									<HeaderStyle CssClass="GridCssHeader" />
									<CellStyle CssClass="GridCss">
									</CellStyle>
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataTextColumn Caption="" VisibleIndex="10" FieldName="NewStatusCode" Width="0px">
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataTextColumn Caption="" VisibleIndex="10" FieldName="OldStatusCode" Width="0px">
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataTextColumn Caption="Last Status Update" FieldName="LastStatusChange" Width="10%"
									VisibleIndex=2>
									<Settings AllowAutoFilter="False" AllowDragDrop="False" />
									<EditCellStyle CssClass="GridCss">
									</EditCellStyle>
									<EditFormCaptionStyle CssClass="GridCss">
									</EditFormCaptionStyle>
									<HeaderStyle CssClass="GridCssHeader" />
									<CellStyle CssClass="GridCss" Wrap="False">
									</CellStyle>
								</dx:GridViewDataTextColumn>
							</Columns>
							<SettingsBehavior AllowDragDrop="False" AllowSelectByRowClick="True" ProcessSelectionChangedOnServer="True" />
							<SettingsPager PageSize="20" SEOFriendly="Enabled">
								<PageSizeItemSettings Visible="true" />
							</SettingsPager>
							<Settings ShowGroupPanel="False" ShowFilterRow="True" />
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
								<AlternatingRow Enabled="True">
								</AlternatingRow>
								<Cell CssClass="GridCss">
								</Cell>
								<LoadingPanel ImageSpacing="5px">
								</LoadingPanel>
							</Styles>
							<StylesEditors ButtonEditCellSpacing="0">
								<ProgressBar Height="21px">
								</ProgressBar>
							</StylesEditors>
						</dx:ASPxGridView>
					</ContentTemplate>
					<Triggers>
						<asp:AsyncPostBackTrigger ControlID="timer1" />
					</Triggers>
				</asp:UpdatePanel>
			</td>
		</tr>
	</table>
</asp:Content>
