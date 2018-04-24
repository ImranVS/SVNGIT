<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true"
	CodeBehind="AlertEventTemplate_Edit.aspx.cs" Inherits="VSWebUI.Configurator.AlertEventTemplate_Edit" %>

<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxTreeList.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.Web.ASPxTreeList" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
	<script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.5/jquery.min.js"></script>
	<script src="../js/jquery-1.9.1.js" type="text/javascript"></script>
	<script src="../js/bootstrap.min.js" type="text/javascript"></script>
	<script type="text/javascript">
		$(document).ready(function () {
			$('.alert-success').delay(10000).fadeOut("slow", function () {
			});
		});
		
	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
	<table>
		<tr>
			<td>
				<div class="header" id="servernamelbldisp" runat="server">
					Alert Event Template</div>
			</td>
		</tr>
		<tr>
			<td>
				<table>
					<tr>
						<td>
							<dx:ASPxLabel ID="AlertEventlb" runat="server" CssClass="lblsmallFont" Text="Alert Event Template Name:">
							</dx:ASPxLabel>
						</td>
						<td>
							<dx:ASPxTextBox ID="AlertEventtb" runat="server" Width="170px">
								<ValidationSettings ErrorDisplayMode="ImageWithTooltip" SetFocusOnError="True">
									<RequiredField ErrorText="Enter Alert Name" IsRequired="True" />
									<RequiredField IsRequired="True" ErrorText="Enter Alert Name"></RequiredField>
								</ValidationSettings>
							</dx:ASPxTextBox>
						</td>
					</tr>
				</table>
			</td>
		</tr>
		<tr>
			<td>
				<table class="style1">
					<tr>
						<td>
							<div class="subheader" id="Div2" runat="server">
								Events</div>
						</td>
					</tr>
					<tr>
						<td>
							<asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
								<ContentTemplate>
									<table>
										<tr>
											<td>
												<dx:ASPxButton ID="CollapseAllButton" ClientInstanceName="collapseAll" runat="server"
													Text="Collapse All" CssClass="sysButton" Wrap="False" EnableClientSideAPI="False"
													OnClick="CollapseAllButton_Click" CausesValidation="false">
													<Image Url="~/images/icons/forbidden.png">
													</Image>
												</dx:ASPxButton>
											</td>
										</tr>
										<tr>
											<td valign="top">
												<dx:ASPxTreeList ID="EventsTreeList" ClientInstanceName="eventsTree" runat="server"
													AutoGenerateColumns="False" OnPageSizeChanged="EventsTreeList_PageSizeChanged"
													CssClass="lblsmallFont" KeyFieldName="Id" ParentFieldName="SrvId" Theme="Office2003Blue"
													Width="50%">
													<Columns>
														<dx:TreeListTextColumn Caption="Events  " FieldName="Name" Name="Events" VisibleIndex="0"
															ReadOnly="True">
															<EditFormSettings Visible="False" />
															<HeaderStyle CssClass="lblsmallFont" />
														</dx:TreeListTextColumn>
														<dx:TreeListTextColumn FieldName="actid" Name="actid" Visible="False" VisibleIndex="2">
														</dx:TreeListTextColumn>
														<dx:TreeListTextColumn FieldName="tbl" Name="tbl" Visible="False" VisibleIndex="3">
														</dx:TreeListTextColumn>
														<dx:TreeListTextColumn FieldName="SrvId" Name="SrvId" Visible="False" VisibleIndex="4">
														</dx:TreeListTextColumn>
														<dx:TreeListCheckColumn Caption="Alert on Event Recurrence" Name="ConsecutiveFailures"
															VisibleIndex="1" FieldName="AlertOnRepeat" Visible="false">
															<PropertiesCheckEdit>
																<CheckBoxStyle CssClass="lblsmallFont" />
															</PropertiesCheckEdit>
															<HeaderStyle CssClass="lblsmallFont" />
															<CellStyle CssClass="GridCss1">
															</CellStyle>
														</dx:TreeListCheckColumn>
													</Columns>
													<Settings GridLines="Both" />
													<SettingsBehavior AllowDragDrop="False" AutoExpandAllNodes="True" />
													<SettingsPager AlwaysShowPager="True" Mode="ShowPager" PageSize="20">
														<PageSizeItemSettings Visible="True">
														</PageSizeItemSettings>
													</SettingsPager>
													<SettingsSelection AllowSelectAll="True" Recursive="True" Enabled="True" />
													<SettingsEditing Mode="Inline"></SettingsEditing>
													<Styles>
														<LoadingPanel ImageSpacing="5px">
														</LoadingPanel>
														<AlternatingNode Enabled="True">
														</AlternatingNode>
													</Styles>
													<StylesPager>
														<PageNumber ForeColor="#3E4846">
														</PageNumber>
														<Summary ForeColor="#1E395B">
														</Summary>
													</StylesPager>
													<StylesEditors ButtonEditCellSpacing="0">
													</StylesEditors>
												</dx:ASPxTreeList>
											</td>
										</tr>
									</table>
								</ContentTemplate>
								<Triggers>
									<asp:AsyncPostBackTrigger ControlID="CollapseAllButton" />
								</Triggers>
							</asp:UpdatePanel>
						</td>
					</tr>
				</table>
			</td>
		</tr>
		<tr>
			<td>
				<table>
					<tr>
						<td>
							<dx:ASPxButton ID="OKButton" runat="server" OnClick="OKButton_Click" Text="OK" CssClass="sysButton"
								ClientIDMode="Static">
							</dx:ASPxButton>
						</td>
						<td>
							<dx:ASPxButton ID="CancelButton" runat="server" OnClick="CancelButton_Click" Text="Cancel"
								CssClass="sysButton" CausesValidation="false">
							</dx:ASPxButton>
						</td>
						  <td valign="top">
                <dx:ASPxButton ID="MaintResetButton" runat="server" CssClass="sysButton"
                    OnClick="MaintResetButton_Click"
                    Text="Reset" CausesValidation="False">
                </dx:ASPxButton>
            </td>
					</tr>

				</table>
			</td>
		</tr>
		<tr>
			<td>
				<div id="errorDiv4" class="alert alert-danger" runat="server" style="display: none">
					Select at least one event.
				</div>
			</td>
		</tr>
	</table>
</asp:Content>
