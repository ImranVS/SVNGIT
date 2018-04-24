<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="WebsphereNodesandServers.aspx.cs" Inherits="VSWebUI.Configurator.WebsphereNodesandServers" %>

<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.Web" TagPrefix="dx" %>

		
<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.Web" TagPrefix="dx1" %>
<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.Web" TagPrefix="dx2" %>
<%@ Register Assembly="DevExpress.Web.ASPxTreeList.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.Web.ASPxTreeList" TagPrefix="dx" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<link href='http://fonts.googleapis.com/css?family=Francois One' rel='stylesheet'
		type='text/css' />
	<link rel="stylesheet" type="text/css" href="../css/vswebforms.css" />
	<script src="../js/jquery-1.9.1.js" type="text/javascript"></script>
	<script src="../js/bootstrap.min.js" type="text/javascript"></script>
	<script type="text/javascript">
		$(document).ready(function () {
			$('.alert-success').delay(10000).fadeOut("slow", function () {
			});
			$('.alert-danger').delay(10000).fadeOut("slow", function () {
			});
		});
		</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<table width="90%">
		<tr>
			<td>
				<div class="header" id="servernamelbldisp" runat="server">
					Add Nodes/Servers</div>
			</td>
		</tr>
	
		<tr>
			<td>
				<div id="successDiv" runat="server" class="success" style="display: none">
					server datails were successully saved.
				</div>
			</td>
		</tr>
		
								
		<tr>
			<td>
				<div id="errorDiv" runat="server" class="alert alert-danger" style="display: none">
					Error:
				</div>
				<div id="errorinfoDiv" runat="server" class="info" style="display: none">
				</div>
				<div id="infoDiv" runat="server" class="info" style="display: none">
					Any server not on the list has already been imported.
				</div>
			</td>
		</tr>
		<tr>
			<td style="color: Black" id="tdmsg" runat="server" align="left">
				<dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" GroupBoxCaptionOffsetY="-24px"
					HeaderText="Select Servers to enable for scanning" Width="100%" Theme="Glass" >
					<ContentPaddings PaddingBottom="10px" PaddingLeft="4px" PaddingTop="10px" />
					<HeaderStyle Height="23px">
						<Paddings PaddingBottom="0px" PaddingLeft="2px" PaddingTop="0px" />
					</HeaderStyle>
					<PanelCollection>
						<dx:PanelContent ID="PanelContent3" runat="server" SupportsDisabledAttribute="True">
							<table class="style1">
							<tr>
							<td>
							<table>
								<tr>
									<td>
										<dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="Cell Name:" CssClass="lblsmallFont">
										</dx:ASPxLabel>
									</td>
									<td>
										<dx:ASPxComboBox ID="ASPxComboBox2" runat="server" AutoPostBack="true" 
											Width="220px" OnSelectedIndexChanged="ASPxComboBox2_SelectedIndexChanged" ValueField="CellID" TextField="CellName">
											
											<ValidationSettings ErrorDisplayMode="Text" SetFocusOnError="True">
												<RequiredField ErrorText="Select Cell Name" IsRequired="True" />
											</ValidationSettings>
										</dx:ASPxComboBox>
									</td>
								</tr>
							</table>
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
																Text="Collapse All" Theme="Office2010Blue" Wrap="False" EnableClientSideAPI="False"
																OnClick="CollapseAllButton_Click">
																<Image Url="~/images/icons/forbidden.png">
																</Image>
															</dx:ASPxButton>
														</td>
													</tr>
													<tr>
														<td valign="top">
															<dx:ASPxTreeList ID="NodesTreeList" ClientInstanceName="eventsTree" runat="server"
																AutoGenerateColumns="False" CssClass="lblsmallFont" KeyFieldName="Id" ParentFieldName="SrvId"
																Theme="Office2003Blue" Width="100%" onfocusednodechanged="NodesTreeList_FocusedNodeChanged">
																<Columns>
																	<dx:TreeListTextColumn Caption="NodeName  " FieldName="Name" Name="NodeName" VisibleIndex="0"
																		ReadOnly="True">
																		<EditFormSettings Visible="False" />
																		<HeaderStyle CssClass="lblsmallFont" />
																	</dx:TreeListTextColumn>
																	 <dx:TreeListTextColumn FieldName="actid" Name="actid" Visible="False" 
                                                                            VisibleIndex="1">
                                                                        </dx:TreeListTextColumn>
                                                                        <dx:TreeListTextColumn FieldName="tbl" Name="tbl" Visible="False" 
                                                                            VisibleIndex="2">
                                                                        </dx:TreeListTextColumn>
                                                                        <dx:TreeListTextColumn FieldName="SrvId" Name="SrvId" Visible="False" 
                                                                            VisibleIndex="3">
                                                                        </dx:TreeListTextColumn>
																	<%--<dx:TreeListTextColumn FieldName="NodeName" Name="NodeName" Visible="false" VisibleIndex="2">
																	</dx:TreeListTextColumn>
																	<dx:TreeListTextColumn FieldName="serverName" Name="serverName" Visible="false" VisibleIndex="3">
																	</dx:TreeListTextColumn>--%>
																</Columns>
																<Styles CssFilePath="~/App_Themes/Office2010Blue/{0}/styles.css" CssPostfix="Office2010Blue">
																	<Header CssClass="GridCssHeader">
																	</Header>
																	<Cell CssClass="GridCss">
																	</Cell>
																	<LoadingPanel ImageSpacing="5px">
																	</LoadingPanel>
																	<AlternatingNode CssClass="GridAltRow" Enabled="True">
																	</AlternatingNode>
																</Styles>
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
						</dx:PanelContent>
					</PanelCollection>
				</dx:ASPxRoundPanel>
			</td>
		</tr>
		<tr>
			<td colspan="2">
				<asp:UpdatePanel ID="UpdatePanel45" runat="server" UpdateMode="Conditional">
					<ContentTemplate>
						<div id="Div1" class="alert alert-success" runat="server" style="display: none">
							Success.
						</div>
						<div id="Div2" class="alert alert-danger" runat="server" style="display: none">
							Error.
						</div>
						<table>
							<tr>
								<td>
									<dx:ASPxButton ID="OKButton" runat="server" Text="OK" Width="75px" 
										Theme="Office2010Blue" onclick="OKButton_Click" >
									</dx:ASPxButton>
								</td>
								<td>
									<dx:ASPxButton ID="CancelButton" runat="server" CausesValidation="False" Text="Cancel"
										Width="75px" Theme="Office2010Blue" onclick="CancelButton_Click" >
									</dx:ASPxButton>
								</td>
							</tr>
						</table>
					</ContentTemplate>
					<Triggers>
						<asp:AsyncPostBackTrigger ControlID="OKButton" />
					</Triggers>
				</asp:UpdatePanel>
			</td>
		</tr>
	</table>
</asp:Content>
