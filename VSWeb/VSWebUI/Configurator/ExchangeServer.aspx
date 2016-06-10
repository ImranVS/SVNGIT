<%@ Page Title="VitalSigns Plus-Exchange Server" Language="C#" MasterPageFile="~/Site1.Master"
	AutoEventWireup="true" CodeBehind="ExchangeServer.aspx.cs" Inherits="VSWebUI.ExchangeServer" %>

<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.Web" TagPrefix="dx" %>






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
		});

		function OnItemClick(s, e) {
			if (e.item.parent == s.GetRootItem())
				e.processOnServer = false;
		}
	</script>
	<style type="text/css">
		.dxeBase
		{
			font: 12px Tahoma;
		}
		.tab
		{
			border-collapse: collapse;
		}
		.tab .first
		{
			border-left: 1px solid #CCC;
			border-top: 1px solid #CCC;
			border-right: 1px solid #CCC;
			border-bottom: 1px solid #CCC;
		}
		.tab .second
		{
			border-left: 1px solid #CCC;
			border-top: 1px solid #CCC;
			border-right: 1px solid #CCC;
			border-bottom: 1px solid #CCC;
		}
		​</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
	<table width="100%">
		<tr>
			<td>
				<table width="100%">
					<tr>
						<td valign="top">
							<div class="header" id="lblServer" runat="server">
								Microsoft Exchange Servers</div>
							<asp:Label ID="lblServerId" runat="server" Font-Bold="False" Visible="False"></asp:Label>
						</td>
						<td valign="top" align="right">
							<table>
								<tr>
									<td>
										<dx:ASPxMenu ID="ASPxMenu1" runat="server" EnableTheming="True" HorizontalAlign="Right"
											OnItemClick="ASPxMenu1_ItemClick" ShowAsToolbar="True" Theme="Moderno">
											<ClientSideEvents ItemClick="OnItemClick" />
											<Items>
												<dx:MenuItem Name="MainItem">
													<Items>
														<dx:MenuItem Name="ServerDetailsPage" Text="View Details in Dashboard">
														</dx:MenuItem>
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
				</table>
			</td>
		</tr>
		<tr>
			<td>
				<%--<asp:UpdatePanel ID="PanelControl2" runat="server">
					<ContentTemplate>--%>
						<dx:ASPxPageControl Font-Bold="True" ID="ASPxPageControlWindow" runat="server" ActiveTabIndex="0"
							CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass" SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css"
							TabSpacing="0px" Width="100%" EnableHierarchyRecreation="False">
							<TabPages>
								<dx:TabPage Text="Server Attributes">
									<TabImage Url="~/images/information.png" />
									<TabImage Url="~/images/information.png">
									</TabImage>
									<ContentCollection>
										<dx:ContentControl runat="server" SupportsDisabledAttribute="True">
											<table class="style1" width="100%">
												<tr>
													<td colspan="2">
														<dx:ASPxRoundPanel ID="ASPxRoundPanel2" runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
															CssPostfix="Glass" GroupBoxCaptionOffsetY="-24px" HeaderText="Server Attributes"
															SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" Width="100%">
															<ContentPaddings Padding="14px" PaddingBottom="10px" PaddingLeft="4px" PaddingTop="10px" />
															<HeaderStyle Height="23px">
															</HeaderStyle>
															<PanelCollection>
																<dx:PanelContent runat="server" SupportsDisabledAttribute="True">
																	<table class="style1">
																		<tr>
																			<td>
																				<dx:ASPxLabel ID="ASPxLabel12" runat="server" CssClass="lblsmallFont" Text="Name:">
																				</dx:ASPxLabel>
																			</td>
																			<td>
																				<dx:ASPxTextBox ID="NameTextBox" runat="server" CssClass="txtsmall" Width="170px">
																				</dx:ASPxTextBox>
																			</td>
																			<td>
																				&nbsp;
																			</td>
																			<td>
																				<dx:ASPxLabel ID="ASPxLabel36" runat="server" CssClass="lblsmallFont" Text="Description:">
																				</dx:ASPxLabel>
																			</td>
																			<td>
																				<dx:ASPxTextBox ID="DescTextBox" runat="server" CssClass="txtsmall" Width="170px">
																				</dx:ASPxTextBox>
																			</td>
																			<td>
																				&nbsp;
																			</td>
																			<td>
																				<dx:ASPxCheckBox ID="EnabledCheckBox" runat="server" CheckState="Unchecked" CssClass="lblsmallFont"
																					Text="Enabled for scanning" Wrap="False">
																				</dx:ASPxCheckBox>
																			</td>
																		</tr>
																		<tr>
																			<td>
																				<dx:ASPxLabel ID="ASPxLabel14" runat="server" CssClass="lblsmallFont" Text="Location:">
																				</dx:ASPxLabel>
																			</td>
																			<td>
																				<dx:ASPxTextBox ID="LocationTextBox" runat="server" CssClass="txtsmall" Width="170px">
																				</dx:ASPxTextBox>
																			</td>
																			<td>
																				&nbsp;
																			</td>
																			<td>
																				<dx:ASPxLabel ID="ASPxLabel13" runat="server" CssClass="lblsmallFont" Text="Category:">
																				</dx:ASPxLabel>
																			</td>
																			<td>
																				<dx:ASPxTextBox ID="CategoryTextBox" runat="server" CssClass="txtsmall" Width="170px">
																				</dx:ASPxTextBox>
																			</td>
																			<td>
																				&nbsp;
																			</td>
																			<td>
																				<dx:ASPxCheckBox ID="ASPxCheckBoxDAG" runat="server" CheckState="Unchecked" CssClass="lblsmallFont"
																					Text="Scan DAG Health with the server" Wrap="False" Visible="False">
																				</dx:ASPxCheckBox>
																			</td>
																		</tr>
																		<tr>
																			<td>
																				<dx:ASPxLabel ID="ASPxLabel6" runat="server" CssClass="lblsmallFont" Text="Credentials:">
																				</dx:ASPxLabel>
																			</td>
																			<td>
																				<dx:ASPxComboBox ID="CredentialsComboBox" runat="server" AutoPostBack="false">
																				</dx:ASPxComboBox>
																			</td>
																			<td>
																				&nbsp;
																			</td>
																			<td>
																				<dx:ASPxLabel ID="ASPxLabel20" runat="server" CssClass="lblsmallFont" Text="Authentication Type:">
																				</dx:ASPxLabel>
																			</td>
																			<td>
																				<dx:ASPxComboBox ID="AuthenticationTypeComboBox" runat="server" AutoPostBack="false">
																					<Items>
																						<dx:ListEditItem Text="Default" Value="0" Selected="true" />
																						<dx:ListEditItem Text="Kerberos" Value="1" />
																					</Items>
																				</dx:ASPxComboBox>
																			</td>
																		</tr>
                                                                         <tr>
                                                                
																				<td>
																					<dx:ASPxButton ID="ASPxButton1" runat="server" Text="Add Credentials" CssClass="sysButton"
																						OnClick="btn_clickcopyprofile" CausesValidation="false" Visible="true" UseSubmitBehavior="false">
																					</dx:ASPxButton>
																				</td>
                                                                </tr>
                                                                <tr>
                                                                <td>
                                                                		
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
														<dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
															CssPostfix="Glass" GroupBoxCaptionOffsetY="-24px" HeaderText="Scan Settings"
															SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" Width="100%">
															<ContentPaddings Padding="14px" PaddingBottom="10px" PaddingLeft="4px" PaddingTop="10px" />
															<HeaderStyle Height="23px">
															</HeaderStyle>
															<PanelCollection>
																<dx:PanelContent runat="server" SupportsDisabledAttribute="True">
																	<table>
																		<tr>
																			<td>
																				<dx:ASPxLabel ID="ASPxLabel1" runat="server" CssClass="lblsmallFont" Text="Scan Interval:">
																				</dx:ASPxLabel>
																			</td>
																			<td>
																				<dx:ASPxTextBox ID="ScanIntvlTextBox" runat="server" CssClass="txtsmall" Width="40px">
																					<MaskSettings Mask="&lt;0..9999&gt;" />
																					<ValidationSettings CausesValidation="True" ErrorDisplayMode="ImageWithTooltip" SetFocusOnError="True">
																						<RegularExpression ErrorText="Please enter a numeric value using the numbers only (0-9)."
																							ValidationExpression="^\d+$" />
																					</ValidationSettings>
																				</dx:ASPxTextBox>
																			</td>
																			<td valign="middle">
																				<dx:ASPxLabel ID="ASPxLabel3" runat="server" CssClass="lblsmallFont" Text="minutes">
																				</dx:ASPxLabel>
																			</td>
																			<td>
																				<dx:ASPxLabel ID="ASPxLabel7" runat="server" CssClass="lblsmallFont" Text="Response Threshold:">
																				</dx:ASPxLabel>
																			</td>
																			<td>
																				<dx:ASPxTextBox ID="ResponseTextBox" runat="server" CssClass="txtsmall" Width="40px">
																					<MaskSettings Mask="&lt;0..99999&gt;" />
																					<ValidationSettings CausesValidation="True" ErrorDisplayMode="ImageWithTooltip" SetFocusOnError="True">
																						<RegularExpression ErrorText="Please enter a numeric value using the numbers only (0-9)."
																							ValidationExpression="^\d+$" />
																					</ValidationSettings>
																				</dx:ASPxTextBox>
																			</td>
																			<td>
																				<dx:ASPxLabel ID="ASPxLabel34" runat="server" CssClass="lblsmallFont" Height="16px"
																					Text="milliseconds">
																				</dx:ASPxLabel>
																			</td>
																		</tr>
																		<tr>
																			<td>
																				<dx:ASPxLabel ID="ASPxLabel5" runat="server" CssClass="lblsmallFont" Text="Retry Interval:">
																				</dx:ASPxLabel>
																			</td>
																			<td>
																				<dx:ASPxTextBox ID="RetryTextBox" runat="server" CssClass="txtsmall" Width="40px">
																				<MaskSettings Mask="&lt;1..99999&gt;"></MaskSettings>
                                                                                        <ValidationSettings ErrorDisplayMode="ImageWithTooltip" CausesValidation="True" SetFocusOnError="True">
                                                                                            <RegularExpression ErrorText="Please enter a numeric value using the numbers only (1-9)."
                                                                                                ValidationExpression="^\d+$"></RegularExpression>
                                                                                            <RequiredField IsRequired="True"></RequiredField>
                                                                                           
                                                                                        </ValidationSettings>
																				</dx:ASPxTextBox>
																			</td>
																			<td valign="middle">
																				<dx:ASPxLabel ID="ASPxLabel30" runat="server" CssClass="lblsmallFont" Text="minutes">
																				</dx:ASPxLabel>
																			</td>
																			<td>
																				<dx:ASPxLabel ID="ASPxLabel35" runat="server" CssClass="lblsmallFont" Text="Failures before Alert:">
																				</dx:ASPxLabel>
																			</td>
																			<td>
																				<dx:ASPxTextBox ID="ConsFailuresBefAlertTextBox" runat="server" CssClass="txtsmall"
																					Width="40px">
																					<MaskSettings ErrorText="Enter number between 1 to 100" Mask="&lt;0..9999&gt;" ShowHints="True" />
																					<ValidationSettings CausesValidation="True" ErrorDisplayMode="ImageWithTooltip" SetFocusOnError="True">
																						<RegularExpression ErrorText="Please enter a numeric value using the numbers only (0-9)."
																							ValidationExpression="^\d+$" />
																						<RequiredField ErrorText="Please enter the failure threshold.  How many times can the server be down before an alert is sent?"
																							IsRequired="True" />
																					</ValidationSettings>
																				</dx:ASPxTextBox>
																			</td>
																			<td>
																				&nbsp;
																			</td>
																		</tr>
																		<tr>
																			<td>
																				<dx:ASPxLabel ID="ASPxLabel2" runat="server" CssClass="lblsmallFont" Text="Off-Hours Scan Interval:">
																				</dx:ASPxLabel>
																			</td>
																			<td>
																				<dx:ASPxTextBox ID="OffscanTextBox" runat="server" CssClass="txtsmall" Width="40px">
																					<MaskSettings Mask="&lt;0..9999&gt;" />
																					<ValidationSettings CausesValidation="True" ErrorDisplayMode="ImageWithTooltip" SetFocusOnError="True">
																						<RegularExpression ErrorText="Please enter a numeric value using the numbers only (0-9)."
																							ValidationExpression="^\d+$" />
																					</ValidationSettings>
																				</dx:ASPxTextBox>
																			</td>
																			<td class="style2">
																				<dx:ASPxLabel ID="ASPxLabel4" runat="server" CssClass="lblsmallFont" Text="minutes">
																				</dx:ASPxLabel>
																			</td>
																			<td>
																				&nbsp;
																			</td>
																			<td>
																				&nbsp;
																			</td>
																			<td>
																				&nbsp;
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
														<dx:ASPxRoundPanel ID="ASPxRoundPanel5" runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
															CssPostfix="Glass" GroupBoxCaptionOffsetY="-24px" HeaderText="Server Roles" SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css"
															Width="100%" Visible="false">
															<ContentPaddings Padding="14px" PaddingBottom="10px" PaddingLeft="4px" PaddingTop="10px" />
															<HeaderStyle Height="23px">
															</HeaderStyle>
															<PanelCollection>
																<dx:PanelContent ID="PanelContent6" runat="server" SupportsDisabledAttribute="True">
																	<table>
																		<tr>
																			<td>
																				<dx:ASPxLabel ID="ASPxLabel41" runat="server" CssClass="lblsmallFont" Height="16px"
																					Text="Exchange Version:">
																				</dx:ASPxLabel>
																			</td>
																			<td>
																				<dx:ASPxComboBox ID="VersionCombobox" runat="server" AutoPostBack="true" OnSelectedIndexChanged="VersionCombobox_SelectedIndexChanged">
																				</dx:ASPxComboBox>
																			</td>
																			<td>
																				&nbsp;
																			</td>
																			<td>
																				<dx:ASPxLabel ID="ASPxLabel71" runat="server" CssClass="lblsmallFont" Height="16px"
																					Text="Roles:">
																				</dx:ASPxLabel>
																			</td>
																			<td>
																				<table>
																					<tr>
																						<td>
																							<dx:ASPxCheckBox ID="RoleHub" runat="server" CheckState="Unchecked" Text="Hub" OnCheckedChanged="RolesCheckBox_CheckedChanged"
																								AutoPostBack="true">
																							</dx:ASPxCheckBox>
																						</td>
																						<td>
																							<dx:ASPxCheckBox ID="RoleMailBox" runat="server" CheckState="Unchecked" Text="Mailbox"
																								OnCheckedChanged="RolesCheckBox_CheckedChanged" AutoPostBack="true">
																							</dx:ASPxCheckBox>
																						</td>
																						<td>
																							<dx:ASPxCheckBox ID="RoleCAS" runat="server" CheckState="Unchecked" Text="CAS" OnCheckedChanged="RolesCheckBox_CheckedChanged"
																								AutoPostBack="true">
																							</dx:ASPxCheckBox>
																						</td>
																						<td>
																							<dx:ASPxCheckBox ID="RoleEdge" runat="server" CheckState="Unchecked" Text="Edge"
																								OnCheckedChanged="RolesCheckBox_CheckedChanged" AutoPostBack="true">
																							</dx:ASPxCheckBox>
																						</td>
																						<td>
																							<dx:ASPxCheckBox ID="RoleUnified" runat="server" CheckState="Unchecked" Text="Unified Messaging"
																								OnCheckedChanged="RolesCheckBox_CheckedChanged" AutoPostBack="true" Visible="false"
																								Checked="false">
																							</dx:ASPxCheckBox>
																						</td>
																					</tr>
																				</table>
																			</td>
																		</tr>
																	</table>
																</dx:PanelContent>
															</PanelCollection>
														</dx:ASPxRoundPanel>
													</td>
												</tr>
												<tr>
													<td align="left" valign="top">
                                                        <dx:ASPxRoundPanel ID="ASPxRoundPanel3" runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
																		CssPostfix="Glass" GroupBoxCaptionOffsetY="-24px" HeaderText="Queue Thresholds"
																		SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" Width="100%">
																		<ContentPaddings Padding="14px" PaddingBottom="10px" PaddingLeft="4px" PaddingTop="10px" />
																		<HeaderStyle Height="23px">
																		</HeaderStyle>
																		<PanelCollection>
																			<dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">
																				<table>
																					<tr>
																						<td>
																							<dx:ASPxLabel ID="ASPxLabel44" runat="server" CssClass="lblsmallFont" Text="Submission Queue Threshold:">
																							</dx:ASPxLabel>
																						</td>
																						<td>
																							<dx:ASPxTextBox ID="SubQThreshold" runat="server" CssClass="txtsmall" Text="100"
																								Width="40px">
																								<MaskSettings Mask="&lt;0..99999&gt;" />
																								<ValidationSettings CausesValidation="True" ErrorDisplayMode="ImageWithTooltip" SetFocusOnError="True">
																									<RegularExpression ErrorText="Please enter a numeric value using the numbers only (0-9)."
																										ValidationExpression="^\d+$" />
																								</ValidationSettings>
																							</dx:ASPxTextBox>
																						</td>
																						<td class="style2">
																							<dx:ASPxLabel ID="ASPxLabel45" runat="server" CssClass="lblsmallFont" Height="16px"
																								Text="messages">
																							</dx:ASPxLabel>
																						</td>
																					</tr>
																					<tr>
																						<td>
																							<dx:ASPxLabel ID="ASPxLabel48" runat="server" CssClass="lblsmallFont" Text="Poison Queue Threshold:">
																							</dx:ASPxLabel>
																						</td>
																						<td>
																							<dx:ASPxTextBox ID="PoisonQThreshold" runat="server" CssClass="txtsmall" Text="100"
																								Width="40px">
																								<MaskSettings ErrorText="Enter number between 1 to 100" Mask="&lt;0..9999&gt;" ShowHints="True" />
																								<ValidationSettings CausesValidation="True" ErrorDisplayMode="ImageWithTooltip" SetFocusOnError="True">
																									<RequiredField ErrorText="Please enter the failure threshold.  How many times can the server be down before an alert is sent?"
																										IsRequired="True" />
																									<RegularExpression ErrorText="Please enter a numeric value using the numbers only (0-9)."
																										ValidationExpression="^\d+$" />
																								</ValidationSettings>
																							</dx:ASPxTextBox>
																						</td>
																						<td class="style2">
																							<dx:ASPxLabel ID="ASPxLabel52" runat="server" CssClass="lblsmallFont" Height="16px"
																								Text="messages">
																							</dx:ASPxLabel>
																						</td>
																					</tr>
																					<tr>
																						<td>
																							<dx:ASPxLabel ID="ASPxLabel49" runat="server" CssClass="lblsmallFont" Text="Unreachable Queue Threshold:">
																							</dx:ASPxLabel>
																						</td>
																						<td>
																							<dx:ASPxTextBox ID="UnReachableQThreshold" runat="server" CssClass="txtsmall" Text="100"
																								Width="40px">
																								<MaskSettings Mask="&lt;0..9999&gt;" />
																								<ValidationSettings CausesValidation="True" ErrorDisplayMode="ImageWithTooltip" SetFocusOnError="True">
																									<RegularExpression ErrorText="Please enter a numeric value using the numbers only (0-9)."
																										ValidationExpression="^\d+$" />
																								</ValidationSettings>
																							</dx:ASPxTextBox>
																						</td>
																						<td class="style2">
																							<dx:ASPxLabel ID="ASPxLabel53" runat="server" CssClass="lblsmallFont" Height="16px"
																								Text="messages">
																							</dx:ASPxLabel>
																						</td>
																					</tr>
																					<tr>
																						<td>
																							<dx:ASPxLabel ID="ASPxLabel18" runat="server" CssClass="lblsmallFont" Text="Shadow Queue Threshold:">
																							</dx:ASPxLabel>
																						</td>
																						<td>
																							<dx:ASPxTextBox ID="ShadowQThreshold" runat="server" CssClass="txtsmall" Text="100"
																								Width="40px">
																								<MaskSettings Mask="&lt;0..9999&gt;" />
																								<ValidationSettings CausesValidation="True" ErrorDisplayMode="ImageWithTooltip" SetFocusOnError="True">
																									<RegularExpression ErrorText="Please enter a numeric value using the numbers only (0-9)."
																										ValidationExpression="^\d+$" />
																								</ValidationSettings>
																							</dx:ASPxTextBox>
																						</td>
																						<td class="style2">
																							<dx:ASPxLabel ID="ASPxLabel19" runat="server" CssClass="lblsmallFont" Height="16px"
																								Text="messages">
																							</dx:ASPxLabel>
																						</td>
																					</tr>
																					<tr>
																						<td>
																							<dx:ASPxLabel ID="ASPxLabel51" runat="server" CssClass="lblsmallFont" Text="Total Queue Threshold:"
																								Visible="false">
																							</dx:ASPxLabel>
																						</td>
																						<td>
																							<dx:ASPxTextBox ID="TotalQThreshold" runat="server" CssClass="txtsmall" Text="15"
																								Width="40px" Visible="false">
																								<MaskSettings Mask="&lt;0..9999&gt;" />
																								<%--<ValidationSettings CausesValidation="True" ErrorDisplayMode="ImageWithTooltip" 
                                                                    SetFocusOnError="True">
                                                                    <RegularExpression ErrorText="Please enter a numeric value using the numbers only (0-9)." 
                                                                        ValidationExpression="^\d+$" />
                                                                </ValidationSettings>--%>
																							</dx:ASPxTextBox>
																						</td>
																						<td class="style2">
																							<dx:ASPxLabel ID="ASPxLabel54" runat="server" CssClass="lblsmallFont" Height="16px"
																								Text="messages" Visible="false">
																							</dx:ASPxLabel>
																						</td>
																					</tr>
																				</table>
																			</dx:PanelContent>
																		</PanelCollection>
																	</dx:ASPxRoundPanel>
													</td>
												    <td align="left" valign="top">
                                                        <dx:ASPxRoundPanel ID="ASPxRoundPanel7" runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
																		CssPostfix="Glass" GroupBoxCaptionOffsetY="-24px" HeaderText="Mail Latency Test Settings"
																		SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" Width="100%">
																		<ContentPaddings Padding="14px" PaddingBottom="10px" PaddingLeft="0px" PaddingTop="10px" />
																		<HeaderStyle Height="23px">
																		</HeaderStyle>
																		<PanelCollection>
																			<dx:PanelContent ID="PanelContent9" runat="server" SupportsDisabledAttribute="True">
																				<table>
																					<tr>
																						<td colspan="2">
																							<dx:ASPxCheckBox ID="Latencycheck" runat="server" CheckState="Unchecked" CssClass="lblsmallFont" AutoPostBack=false
																								Text="Enabled for Latency Test" Wrap="False">
																							</dx:ASPxCheckBox>
																						</td>
																					</tr>
																					<tr>
																						<td>
																							<dx:ASPxLabel ID="ASPxLabel11" runat="server" CssClass="lblsmallFont" Text="Yellow (warning) Threshold:">
																							</dx:ASPxLabel>
																						</td>
																						<td>
																							<dx:ASPxTextBox ID="ltencyyellow" runat="server" CssClass="txtsmall" Width="50px">
																							</dx:ASPxTextBox>
																						</td>
																						<td>
																							<dx:ASPxLabel ID="ASPxLabel16" runat="server" CssClass="lblsmallFont" Text="milliseconds">
																							</dx:ASPxLabel>
																						</td>
																					</tr>
																					<tr>
																						<td>
																							<dx:ASPxLabel ID="ASPxLabel15" runat="server" CssClass="lblsmallFont" Text="Red (alert) Threshold:">
																							</dx:ASPxLabel>
																						</td>
																						<td>
																							<dx:ASPxTextBox ID="ltencyred" runat="server" CssClass="txtsmall" Width="50px">
																							</dx:ASPxTextBox>
																						</td>
																						<td>
																						<dx:ASPxLabel ID="ASPxLabel17" runat="server" CssClass="lblsmallFont" Height="16px"
																							Text="milliseconds">
																						</dx:ASPxLabel>
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
														<dx:ASPxRoundPanel ID="ASPxRoundPanel9" runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
															CssPostfix="Glass" GroupBoxCaptionOffsetY="-24px" HeaderText="Other Settings"
															SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" Width="100%" Visible="false">
															<ContentPaddings Padding="14px" PaddingBottom="10px" PaddingLeft="4px" PaddingTop="10px" />
															<HeaderStyle Height="23px">
															</HeaderStyle>
															<PanelCollection>
																<dx:PanelContent ID="PanelContent5" runat="server" SupportsDisabledAttribute="True">
																	<table class="style1">
																		<tr>
																			<td>
																				<dx:ASPxLabel ID="ASPxLabel40" runat="server" CssClass="lblsmallFont" Text="Consecutive over threshold hits before alert:"
																					Wrap="True">
																				</dx:ASPxLabel>
																			</td>
																			<td>
																				<dx:ASPxTextBox ID="ConsOvrThresholdBefAlertTextBox" runat="server" CssClass="txtsmall"
																					Width="40px">
																					<MaskSettings Mask="&lt;0..9999&gt;" />
																					<ValidationSettings CausesValidation="True" ErrorDisplayMode="ImageWithTooltip" SetFocusOnError="True">
																						<RegularExpression ErrorText="Please enter a numeric value using the numbers only (0-9)."
																							ValidationExpression="^\d+$" />
																					</ValidationSettings>
																				</dx:ASPxTextBox>
																			</td>
																		</tr>
																	</table>
																</dx:PanelContent>
															</PanelCollection>
														</dx:ASPxRoundPanel>
													</td>
												</tr>
											</table>
										</dx:ContentControl>
									</ContentCollection>
								</dx:TabPage>
								<dx:TabPage Text="Windows Services">
									<TabImage Url="~/images/icons/windows.gif">
									</TabImage>
									<ContentCollection>
										<dx:ContentControl ID="ContentControl3" runat="server" SupportsDisabledAttribute="True">
											<table width="100%">
												<tr>
													<td>
														<dx:ASPxCallback ID="cb" runat="server" ClientInstanceName="cb" OnCallback="cb_callback" />
														<dx:ASPxGridView ID="ServicesGrid" runat="server" AutoGenerateColumns="False" ClientInstanceName="WindowsGridView"
															EnableTheming="True" KeyFieldName="ID" Theme="Office2003Blue" Width="100%" OnHtmlDataCellPrepared="ServicesGrid_HtmlDataCellPrepared" OnPageSizeChanged="ServicesGrid_PageSizeChanged">
															<Columns>
																<dx:GridViewDataColumn Caption="Select" VisibleIndex="0" CellStyle-HorizontalAlign="Center"
																	Width="50px">
																	<DataItemTemplate>
																		<dx:ASPxCheckBox ID="checkToMonitor" runat="server" OnInit="checkToMonitor_Init"
																			Value='<%# Eval("isSelected") %>' />
																	</DataItemTemplate>
																	<HeaderStyle CssClass="GridCssHeader1" />
																	<CellStyle HorizontalAlign="Center" CssClass="GridCss1">
																	</CellStyle>
																</dx:GridViewDataColumn>
																<dx:GridViewDataTextColumn Caption="Service Name" VisibleIndex="0" FieldName="ServiceName">
																	<Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
																	<EditCellStyle CssClass="GridCss">
																	</EditCellStyle>
																	<EditFormCaptionStyle CssClass="GridCss">
																	</EditFormCaptionStyle>
																	<HeaderStyle CssClass="GridCssHeader" />
																	<CellStyle CssClass="GridCss">
																	</CellStyle>
                                                                    <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
																</dx:GridViewDataTextColumn>
																<dx:GridViewDataTextColumn Caption="Startup Mode" VisibleIndex="2" FieldName="StartMode"
																	Width="180px">
																	<Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
																	<EditCellStyle CssClass="GridCss">
																	</EditCellStyle>
																	<EditFormCaptionStyle CssClass="GridCss">
																	</EditFormCaptionStyle>
																	<HeaderStyle CssClass="GridCssHeader" />
																	<CellStyle CssClass="GridCss">
																	</CellStyle><Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
																</dx:GridViewDataTextColumn>
																<dx:GridViewDataTextColumn Caption="Status" VisibleIndex="2" FieldName="Result" Width="180px">
																	<Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
																	<EditCellStyle CssClass="GridCss">
																	</EditCellStyle>
																	<EditFormCaptionStyle CssClass="GridCss">
																	</EditFormCaptionStyle>
																	<HeaderStyle CssClass="GridCssHeader" />
																	<CellStyle CssClass="GridCss">
																	</CellStyle><Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
																</dx:GridViewDataTextColumn>
																<dx:GridViewDataTextColumn Caption="Currently Monitored" VisibleIndex="1" FieldName="Monitored"
																	Visible="false">
																	<HeaderStyle CssClass="GridCssHeader" />
																	<Settings AllowAutoFilter="False" />
																	<CellStyle CssClass="GridCss1" HorizontalAlign="Center">
																	</CellStyle>
																</dx:GridViewDataTextColumn>
																<dx:GridViewDataTextColumn Caption="ID" VisibleIndex="1" FieldName="ID" Visible="false">
																	<HeaderStyle CssClass="GridCssHeader" />
																	<Settings AllowAutoFilter="False" />
																	<CellStyle CssClass="GridCss1" HorizontalAlign="Center">
																	</CellStyle>
																</dx:GridViewDataTextColumn>
																<dx:GridViewDataTextColumn Caption="isSelected" FieldName="isSelected" VisibleIndex="4"
																	Visible="false">
																</dx:GridViewDataTextColumn>
																<dx:GridViewDataTextColumn Caption="Type" FieldName="Type" VisibleIndex="4" Visible="false">
																</dx:GridViewDataTextColumn>
															</Columns>
															<Templates>
																<GroupRowContent>
																	<%# Container.GroupText %>
																</GroupRowContent>
															</Templates>
															<SettingsBehavior AllowDragDrop="False" AutoExpandAllGroups="True" AllowSort="true" />
															<Settings ShowFilterRow="True" />
															<SettingsPager PageSize="20">
																<PageSizeItemSettings Visible="True">
																</PageSizeItemSettings>
															</SettingsPager>
															<Styles>
																<AlternatingRow CssClass="GridAltRow">
																</AlternatingRow>
																<GroupRow Font-Bold="true" />
															</Styles>
														</dx:ASPxGridView>
													</td>
												</tr>
											</table>
										</dx:ContentControl>
									</ContentCollection>
								</dx:TabPage>
								<dx:TabPage Text="Advanced">
									<TabImage Url="~/images/package_green.png">
									</TabImage>
									<ContentCollection>
										<dx:ContentControl ID="ContentControl2" runat="server" SupportsDisabledAttribute="True">
											<table>
												<tr>
													<td valign="top">
														<asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
															<ContentTemplate>
																<dx:ASPxRoundPanel ID="MemRoundPanel9" runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
																	CssPostfix="Glass" GroupBoxCaptionOffsetY="-24px" HeaderText="Memory Usage Alert"
																	Height="50px" SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" Width="400px">
																	<ContentPaddings Padding="2px" PaddingBottom="10px" PaddingLeft="4px" PaddingTop="10px" />
																	<HeaderStyle Height="23px">
																	</HeaderStyle>
																	<PanelCollection>
																		<dx:PanelContent ID="PanelContent2" runat="server" SupportsDisabledAttribute="True">
																			<table cellspacing="3px" width="100%">
																				<tr>
																					<td>
																						<dx:ASPxLabel ID="MemLabel" runat="server" Visible="False">
																						</dx:ASPxLabel>
																						<dx:ASPxTrackBar ID="AdvMemoryThTrackBar" runat="server" AutoPostBack="True" CssFilePath="~/App_Themes/Office2010Black/{0}/styles.css"
																							CssPostfix="Office2010Black" EnableViewState="False" OnPositionChanged="AdvMemoryThTrackBar_PositionChanged"
																							Position="95" PositionStart="95" ScalePosition="LeftOrTop" SpriteCssFilePath="~/App_Themes/Office2010Black/{0}/sprite.css"
																							Step="1" Width="95%" Theme="Office2010Blue">
																							<ValueToolTipStyle>
																							</ValueToolTipStyle>
																						</dx:ASPxTrackBar>
																					</td>
																				</tr>
																				<tr>
																					<td align="left">
																						<asp:Label ID="Label5" runat="server" Style="color: #0033CC" Text="&lt;b&gt;Memory Utilization&lt;/b&gt; alerts will trigger if the percentage of memory in use on the server exceeeds this threshold.  &lt;br/&gt;&lt;br/&gt; If you don't want to get memory alerts, set the threshold to zero."></asp:Label>
																					</td>
																				</tr>
																			</table>
																		</dx:PanelContent>
																	</PanelCollection>
																</dx:ASPxRoundPanel>
															</ContentTemplate>
														</asp:UpdatePanel>
													</td>
													<td valign="top">
														<asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
															<ContentTemplate>
																<dx:ASPxRoundPanel ID="CPURoundPanel" runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
																	CssPostfix="Glass" GroupBoxCaptionOffsetY="-24px" HeaderText="CPU Utilization Alert"
																	Height="59px" SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" Width="396px">
																	<ContentPaddings Padding="2px" PaddingBottom="10px" PaddingLeft="4px" PaddingTop="10px" />
																	<HeaderStyle Height="23px">
																	</HeaderStyle>
																	<PanelCollection>
																		<dx:PanelContent ID="PanelContent7" runat="server" SupportsDisabledAttribute="True">
																			<table cellspacing="3px" width="100%">
																				<tr>
																					<td>
																						<dx:ASPxLabel ID="CpuLabel" runat="server" Visible="False">
																						</dx:ASPxLabel>
																						<dx:ASPxTrackBar ID="AdvCPUThTrackBar" runat="server" AutoPostBack="True" CssFilePath="~/App_Themes/Office2010Black/{0}/styles.css"
																							CssPostfix="Office2010Black" EnableViewState="False" OnValueChanged="AdvCPUThTrackBar_ValueChanged"
																							Position="90" PositionStart="90" ScalePosition="LeftOrTop" SpriteCssFilePath="~/App_Themes/Office2010Black/{0}/sprite.css"
																							Step="1" Width="95%" Theme="Office2010Blue">
																						</dx:ASPxTrackBar>
																					</td>
																				</tr>
																				<tr>
																					<td align="left">
																						<asp:Label ID="Label6" runat="server" Style="color: #0033CC" Text="&lt;b&gt;CPU Utilization&lt;/b&gt; alerts will trigger if the CPU utilization rate exceeds this threshold. &lt;br/&gt;&lt;br/&gt;  If you don't want to get CPU alerts, set the threshold to zero. "></asp:Label>
																					</td>
																				</tr>
																			</table>
																		</dx:PanelContent>
																	</PanelCollection>
																</dx:ASPxRoundPanel>
															</ContentTemplate>
														</asp:UpdatePanel>
													</td>
												</tr>
												<tr>
													<td colspan="2">
														<dx:ASPxRoundPanel ID="ASPxRoundPanel6" runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
															CssPostfix="Glass" GroupBoxCaptionOffsetY="-24px" HeaderText="Server Running Days Alert"
															Height="50px" SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" Width="800px">
															<ContentPaddings Padding="2px" PaddingBottom="10px" PaddingLeft="4px" PaddingTop="10px" />
															<ContentPaddings Padding="2px" PaddingLeft="4px" PaddingTop="10px" PaddingBottom="10px">
															</ContentPaddings>
															<HeaderStyle Height="23px">
															</HeaderStyle>
															<PanelCollection>
																<dx:PanelContent ID="PanelContent8" runat="server" SupportsDisabledAttribute="True">
																	<asp:Label ID="Label3" runat="server" Style="color: #0033CC" Text="Some companies have a practice to reboot Exchange servers after a set number of days. Enter a value here if you would like to be notified if a server is running beyond this limit."></asp:Label>
																	<table width="480px">
																		<tr>
																			<td align="right">
																				<dx:ASPxLabel ID="ASPxLabel8" runat="server" Text="Alert if total elapsed running time exceeds:"
																					CssClass="lblsmallFont">
																				</dx:ASPxLabel>
																			</td>
																			<td align="left" width="50px">
																				<dx:ASPxTextBox ID="ServerDaysAlert" runat="server" Width="50px">
																				</dx:ASPxTextBox>
																			</td>
																			<td width="150px">
																				<dx:ASPxLabel ID="ASPxLabel9" runat="server" Text=" consecutive days" CssClass="lblsmallFont">
																				</dx:ASPxLabel>
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
														<dx:ASPxRoundPanel ID="ASPxRoundPanel10" runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
															CssPostfix="Glass" GroupBoxCaptionOffsetY="-24px" HeaderText="CAS Server Tests"
															Height="50px" SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" Width="800px">
															<ContentPaddings Padding="2px" PaddingBottom="10px" PaddingLeft="4px" PaddingTop="10px" />
															<ContentPaddings Padding="2px" PaddingLeft="4px" PaddingTop="10px" PaddingBottom="10px">
															</ContentPaddings>
															<HeaderStyle Height="23px">
															</HeaderStyle>
															<PanelCollection>
																<dx:PanelContent ID="PanelContent3" runat="server" SupportsDisabledAttribute="True">
																	<asp:Label ID="Label8" runat="server" Style="color: #0033CC" Text="Check any items you would like to be tested upon scanning this server."></asp:Label>
																	<table>
																		<tr>
																			<td>
																				<dx:ASPxCheckBox ID="CASSmtp" runat="server" CheckState="Unchecked" Text="SMTP">
																				</dx:ASPxCheckBox>
																			</td>
																			<td>
																				<dx:ASPxCheckBox ID="CASEWS" runat="server" CheckState="Unchecked" Text="Outlook Anywhere">
																				</dx:ASPxCheckBox>
																			</td>
																			<td>
																				<dx:ASPxCheckBox ID="CASOWA" runat="server" CheckState="Unchecked" Text="OWA (Outlook Web App)">
																				</dx:ASPxCheckBox>
																			</td>
																		</tr>
																		<tr>
																			<td>
																				<dx:ASPxCheckBox ID="CASPop3" runat="server" CheckState="Unchecked" Text="POP3">
																				</dx:ASPxCheckBox>
																			</td>
																			<td>
																				<dx:ASPxCheckBox ID="CASAutoDiscovery" runat="server" CheckState="Unchecked" Text="Auto Discovery">
																				</dx:ASPxCheckBox>
																			</td>
																			<td>
																				<dx:ASPxCheckBox ID="CASOARPC" runat="server" CheckState="Unchecked" Text="Outlook Native RPC">
																				</dx:ASPxCheckBox>
																			</td>
																		</tr>
																		<tr>
																			<td>
																				<dx:ASPxCheckBox ID="CASImap" runat="server" CheckState="Unchecked" Text="IMAP">
																				</dx:ASPxCheckBox>
																			</td>
																			<%--<td>
																				<dx:ASPxCheckBox ID="CASECP" runat="server" CheckState="Unchecked" Text="ECP" Visible="false">
																				</dx:ASPxCheckBox>
																			</td>
																			<td>
																				<dx:ASPxCheckBox ID="CASOAB" runat="server" CheckState="Unchecked" Text="Offline Address Book" Visible ="false">
																				</dx:ASPxCheckBox>
																			</td>--%>
                                                                            <td>
																				<dx:ASPxCheckBox ID="CASActiveSync" runat="server" CheckState="Unchecked" Text="Active Sync">
																				</dx:ASPxCheckBox>
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
																		<tr>
																			<td>
																				<dx:ASPxLabel ID="ASPxLabel10" runat="server" CssClass="lblsmallFont" Text="Active Sync Credentials:">
																				</dx:ASPxLabel>
																			</td>
																			<td>
																				<dx:ASPxComboBox ID="ActiveSyncCredentialsComboBox" runat="server" AutoPostBack="false">
																				</dx:ASPxComboBox>
																			</td>
                                                                            <td>
																					<dx:ASPxButton ID="ASPxButton2" runat="server" Text="Add Credentials" CssClass="sysButton"
																						OnClick="btn_clickcopyprofile" CausesValidation="false" Visible="true" UseSubmitBehavior="false">
																					</dx:ASPxButton>
																				</td>
																		</tr>
																	</table>
																</dx:PanelContent>
															</PanelCollection>
														</dx:ASPxRoundPanel>
													</td>
												</tr>
											</table>
										</dx:ContentControl>
									</ContentCollection>
								</dx:TabPage>
								<dx:TabPage Text="CAS" Visible="false">
									<ContentCollection>
										<dx:ContentControl runat="server" SupportsDisabledAttribute="True">
										</dx:ContentControl>
									</ContentCollection>
								</dx:TabPage>
								<dx:TabPage Text="Disk Settings">
									<TabImage Url="~/images/drive.png">
									</TabImage>
									<ContentCollection>
										<dx:ContentControl ID="ContentControl1" runat="server" SupportsDisabledAttribute="True">
											<table width="100%">
												<tr>
													<td>
														<dx:ASPxRoundPanel ID="DiskSettingsRoundPanel2" runat="server" Width="100%" HeaderText="Disk Settings"
															Theme="Glass">
															<PanelCollection>
																<dx:PanelContent ID="PanelContent11" runat="server" SupportsDisabledAttribute="True">
																	<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" OnUnload="UpdatePanel_Unload">
																		<Triggers>
																			<asp:AsyncPostBackTrigger ControlID="SelCriteriaRadioButtonList" />
																		</Triggers>
																		<ContentTemplate>
																			<table width="100%">
																				<tr>
																					<td valign="top">
																						<dx:ASPxRadioButtonList ID="SelCriteriaRadioButtonList" ClientInstanceName="diskSettingsRadioBtn"
																							runat="server" SelectedIndex="0" OnSelectedIndexChanged="SelCriteriaRadioButtonList_SelectedIndexChanged"
																							AutoPostBack="True" TextWrap="False">
																							<Items>
																								<dx:ListEditItem Selected="True" Text="All Disks - By Percentage" Value="0" />
																								<dx:ListEditItem Text="All Disks - By GB" Value="3" />
																								<dx:ListEditItem Text="Selected Disks" Value="1" />
																								<dx:ListEditItem Text="No Disk Alerts" Value="2" />
																							</Items>
																						</dx:ASPxRadioButtonList>
																					</td>
																					<td>
																						&nbsp;
																					</td>
																					<td valign="top">
																						<table>
																							<tr>
																								<td>
																									<div id="infoDiskDiv" class="info" runat="server" style="display: none">
																										The column 'Free Space Threshold' should be set to a value in GB or Percent (1 to
																										100) of the remaining free space when an alert should be generated. The column 'Threshold
																										Type' should be set accordingly in either percent or GB.
																									</div>
																									<dx:ASPxLabel ID="DiskGridInfo" runat="server" Text="Please select disks from the grid below:"
																										Visible="False" CssClass="lblsmallFont">
																									</dx:ASPxLabel>
																									<dx:ASPxLabel ID="DiskLabel" runat="server" Visible="True" CssClass="lblsmallFont">
																									</dx:ASPxLabel>
																								</td>
																							</tr>
																							<tr>
																								<td>
																									<dx:ASPxGridView ID="DiskGridView" ClientInstanceName="diskSettingsGrid" runat="server"
																										AutoGenerateColumns="False" Cursor="pointer" KeyFieldName="DiskName" Theme="Office2003Blue"
																										OnPageSizeChanged="DiskGridView_PageSizeChanged" OnPreRender="DiskGridView_PreRender"
																										OnRowUpdating="DiskGridView_RowUpdating" Visible="False" Width="100%">
																										<Columns>
																											<dx:GridViewDataTextColumn Caption="isSelected" FieldName="isSelected" VisibleIndex="12"
																												Visible="false">
																												<Settings AutoFilterCondition="Contains" />
																												<Settings AutoFilterCondition="Contains"></Settings>
																												<EditCellStyle CssClass="GridCss">
																												</EditCellStyle>
																												<EditFormCaptionStyle CssClass="GridCss" HorizontalAlign="Left">
																												</EditFormCaptionStyle>
																												<HeaderStyle CssClass="GridCssHeader" />
																												<CellStyle CssClass="GridCss">
																												</CellStyle>
																											</dx:GridViewDataTextColumn>
																											<dx:GridViewDataTextColumn Caption="Disk Name" FieldName="DiskName" ReadOnly="true"
																												VisibleIndex="4" Width="100px">
																												<Settings AutoFilterCondition="Contains" />
																												<Settings AutoFilterCondition="Contains"></Settings>
																												<EditCellStyle CssClass="GridCss">
																												</EditCellStyle>
																												<EditFormCaptionStyle CssClass="GridCss" HorizontalAlign="Left">
																												</EditFormCaptionStyle>
																												<HeaderStyle CssClass="GridCssHeader" />
																												<CellStyle CssClass="GridCss">
																												</CellStyle>
																											</dx:GridViewDataTextColumn>
																											<dx:GridViewDataTextColumn Caption="Free Space Threshold" FieldName="Threshold" VisibleIndex="9">
																												<PropertiesTextEdit DisplayFormatString="g" Width="50px">
																													<ValidationSettings>
																														<RegularExpression ValidationExpression="(^(100(?:\.0{1,2})?))|(?!^0*$)(?!^0*\.0*$)^\d{1,2}(\.\d{1,2})?$" />
																													</ValidationSettings>
																												</PropertiesTextEdit>
																												<DataItemTemplate>
																													<dx:ASPxTextBox ID="txtFreeSpaceThresholdValue" runat="server" Width="150px" Value='<%# Eval("Threshold") %>'>
																													</dx:ASPxTextBox>
																												</DataItemTemplate>
																												<Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
																												<HeaderStyle CssClass="GridCssHeader" />
																												<CellStyle CssClass="GridCss2">
																												</CellStyle>
																											</dx:GridViewDataTextColumn>
																											<dx:GridViewDataComboBoxColumn Caption="Threshold Type (% or GB)" FieldName="ThresholdType"
																												VisibleIndex="11">
																												<PropertiesComboBox Width="50px">
																												</PropertiesComboBox>
																												<DataItemTemplate>
																													<dx:ASPxComboBox ID="txtFreeSpaceThresholdType" runat="server" TextField="ThresholdType"
																														Value='<%# Eval("ThresholdType") %>' ValueField="ThresholdType" ValueType="System.String">
																														<Items>
																															<dx:ListEditItem Text="Percent" Value="Percent" />
																															<dx:ListEditItem Text="GB" Value="GB" />
																														</Items>
																													</dx:ASPxComboBox>
																												</DataItemTemplate>
																												<HeaderStyle CssClass="GridCssHeader" />
																												<CellStyle CssClass="GridCss">
																												</CellStyle>
																											</dx:GridViewDataComboBoxColumn>
																											<dx:GridViewCommandColumn Caption="Select" Visible="true" VisibleIndex="3" ShowSelectCheckbox="True">
																												<HeaderStyle CssClass="GridCssHeader" />
																												<CellStyle CssClass="GridCss1">
																												</CellStyle>
																											</dx:GridViewCommandColumn>
																											<dx:GridViewDataTextColumn Caption="Disk Size" FieldName="DiskSize" VisibleIndex="5">
																												<HeaderStyle CssClass="GridCssHeader" />
																												<CellStyle CssClass="GridCss2">
																												</CellStyle>
																											</dx:GridViewDataTextColumn>
																											<dx:GridViewDataTextColumn Caption="Disk Free Space" FieldName="DiskFree" VisibleIndex="6">
																												<HeaderStyle CssClass="GridCssHeader" />
																												<CellStyle CssClass="GridCss2">
																												</CellStyle>
																											</dx:GridViewDataTextColumn>
																											<dx:GridViewDataTextColumn Caption="Percent Free" FieldName="PercentFree" VisibleIndex="7">
																												<HeaderStyle CssClass="GridCssHeader" />
																												<CellStyle CssClass="GridCss2">
																												</CellStyle>
																											</dx:GridViewDataTextColumn>
																										</Columns>
																										<SettingsPager PageSize="50" SEOFriendly="Enabled" Mode="ShowAllRecords">
																										</SettingsPager>
																										<SettingsEditing Mode="Inline">
																										</SettingsEditing>
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
																											<Header ImageSpacing="5px" SortingImageSpacing="5px">
																											</Header>
																											<GroupRow Font-Bold="True">
																											</GroupRow>
																											<AlternatingRow CssClass="GridAltRow" Enabled="True">
																											</AlternatingRow>
																											<LoadingPanel ImageSpacing="5px">
																											</LoadingPanel>
																										</Styles>
																										<StylesEditors ButtonEditCellSpacing="0">
																											<ProgressBar Height="21px">
																											</ProgressBar>
																										</StylesEditors>
																									</dx:ASPxGridView>
																									<dx:ASPxTrackBar ID="AdvDiskSpaceThTrackBar" runat="server" AutoPostBack="True" CssFilePath="~/App_Themes/Office2010Black/{0}/styles.css"
																										CssPostfix="Office2010Black" EnableViewState="false" OnValueChanged="AdvDiskSpaceThTrackBar_ValueChanged"
																										Position="10" PositionStart="10" ScalePosition="LeftOrTop" SmallTickFrequency="5"
																										SpriteCssFilePath="~/App_Themes/Office2010Black/{0}/sprite.css" Step="1" Width="100%"
																										Theme="Office2010Blue">
																									</dx:ASPxTrackBar>
																									<table>
																										<tr>
																											<td>
																												<dx:ASPxLabel ID="GBTitle" runat="server" Text="Current threshold:" CssClass="lblsmallFont"
																													Visible="false">
																												</dx:ASPxLabel>
																											</td>
																											<td>
																												<dx:ASPxTextBox ID="GBTextBox" runat="server" Width="170px" Visible="False">
																												</dx:ASPxTextBox>
																											</td>
																											<td>
																												<dx:ASPxLabel ID="GBLabel" runat="server" Text="GB free space" CssClass="lblsmallFont"
																													Visible="False">
																												</dx:ASPxLabel>
																											</td>
																										</tr>
																										<tr>
																											<td colspan="3">
																												&nbsp;
																											</td>
																										</tr>
																									</table>
																									<asp:Label ID="Label4" runat="server" ForeColor="#0033CC" CssClass="lblsmallFont"
																										Text="&nbsp;&lt;b&gt;Disk Space &lt;/b&gt;alerts will trigger if any of the drives on the Exchange server falls below the threshold."></asp:Label>
																								</td>
																							</tr>
																						</table>
																					</td>
																				</tr>
																			</table>
																		</ContentTemplate>
																	</asp:UpdatePanel>
																</dx:PanelContent>
															</PanelCollection>
														</dx:ASPxRoundPanel>
													</td>
												</tr>
											</table>
										</dx:ContentControl>
									</ContentCollection>
								</dx:TabPage>
								<dx:TabPage Text="Database Settings">
									<TabImage Url="~/images/drive.png">
									</TabImage>
									<ContentCollection>
										<dx:ContentControl ID="ContentControl4" runat="server" SupportsDisabledAttribute="True">
											<table width="100%">
												<tr>
													<td>
														<dx:ASPxRoundPanel ID="ASPxRoundPanel4" runat="server" Width="100%" HeaderText="Database Settings"
															Theme="Glass">
															<PanelCollection>
																<dx:PanelContent ID="PanelContent4" runat="server" SupportsDisabledAttribute="True">
																	<asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional" OnUnload="UpdatePanel_Unload">
																		<Triggers>
																			<asp:AsyncPostBackTrigger ControlID="SelCriteriaDBRadioButtonList" />
																		</Triggers>
																		<ContentTemplate>
																			<table width="100%">
																				<tr>
																					<td valign="top">
																						<dx:ASPxRadioButtonList ID="SelCriteriaDBRadioButtonList" ClientInstanceName="databaseSettingsRadioBtn"
																							runat="server" SelectedIndex="0" OnSelectedIndexChanged="SelCriteriaDBRadioButtonList_SelectedIndexChanged"
																							AutoPostBack="True" TextWrap="False">
																							<Items>
																								<dx:ListEditItem Text="All Databases - By MB" Value="0" />
																								<dx:ListEditItem Text="Selected Databases" Value="1" />
																								<dx:ListEditItem Text="No Database Checking" Value="2" />
																							</Items>
																						</dx:ASPxRadioButtonList>
																					</td>
																					<td>
																						&nbsp;
																					</td>
																					<td valign="top">
																						<table>
																							<tr>
																								<td>
																									<div id="infoDatabaseDiv" class="info" runat="server" style="display: none">
																										The column 'Free Space Threshold' should be set to a value in MB of the remaining
																										free space when an alert should be generated.
																									</div>
																									<dx:ASPxLabel ID="DatabaseGridInfo" runat="server" Text="Please select databases from the grid below:"
																										Visible="False" CssClass="lblsmallFont">
																									</dx:ASPxLabel>
																									<dx:ASPxLabel ID="DatabaseLabel" runat="server" Visible="True" CssClass="lblsmallFont">
																									</dx:ASPxLabel>
																								</td>
																							</tr>
																							<tr>
																								<td>
																									<dx:ASPxGridView ID="DatabaseGridView" ClientInstanceName="DatabaseSettingsGrid"
																										runat="server" AutoGenerateColumns="False" Cursor="pointer" KeyFieldName="DatabaseName"
																										Theme="Office2003Blue" OnPreRender="DatabaseGridView_PreRender" OnRowUpdating="DatabaseGridView_RowUpdating"
																										Visible="False" Width="100%">
																										<Columns>
																											<dx:GridViewDataTextColumn Caption="isSelected" FieldName="isSelected" VisibleIndex="12"
																												Visible="false">
																												<Settings AutoFilterCondition="Contains" />
																												<Settings AutoFilterCondition="Contains"></Settings>
																												<EditCellStyle CssClass="GridCss">
																												</EditCellStyle>
																												<EditFormCaptionStyle CssClass="GridCss" HorizontalAlign="Left">
																												</EditFormCaptionStyle>
																												<HeaderStyle CssClass="GridCssHeader" />
																												<CellStyle CssClass="GridCss">
																												</CellStyle>
																											</dx:GridViewDataTextColumn>
																											<dx:GridViewDataTextColumn Caption="Server" FieldName="ServerName" ReadOnly="true"
																												VisibleIndex="4" Width="100px">
																												<Settings AutoFilterCondition="Contains" />
																												<Settings AutoFilterCondition="Contains"></Settings>
																												<EditCellStyle CssClass="GridCss">
																												</EditCellStyle>
																												<EditFormCaptionStyle CssClass="GridCss" HorizontalAlign="Left">
																												</EditFormCaptionStyle>
																												<HeaderStyle CssClass="GridCssHeader" />
																												<CellStyle CssClass="GridCss">
																												</CellStyle>
																											</dx:GridViewDataTextColumn>
																											<dx:GridViewDataTextColumn Caption="Database Name" FieldName="DatabaseName" ReadOnly="true"
																												VisibleIndex="5" Width="100px">
																												<Settings AutoFilterCondition="Contains" />
																												<Settings AutoFilterCondition="Contains"></Settings>
																												<EditCellStyle CssClass="GridCss">
																												</EditCellStyle>
																												<EditFormCaptionStyle CssClass="GridCss" HorizontalAlign="Left">
																												</EditFormCaptionStyle>
																												<HeaderStyle CssClass="GridCssHeader" />
																												<CellStyle CssClass="GridCss">
																												</CellStyle>
																											</dx:GridViewDataTextColumn>
																											<dx:GridViewDataTextColumn Caption="Database Size (MB)" FieldName="SizeMB" ReadOnly="true"
																												VisibleIndex="6" Width="100px">
																												<Settings AutoFilterCondition="Contains" />
																												<Settings AutoFilterCondition="Contains"></Settings>
																												<EditCellStyle CssClass="GridCss2">
																												</EditCellStyle>
																												<EditFormCaptionStyle CssClass="GridCss2" HorizontalAlign="Left">
																												</EditFormCaptionStyle>
																												<HeaderStyle CssClass="GridCssHeader2" />
																												<CellStyle CssClass="GridCss2">
																												</CellStyle>
																											</dx:GridViewDataTextColumn>
																											<dx:GridViewDataTextColumn Caption="Whitespace Size (MB)" FieldName="WhiteSpaceMB"
																												ReadOnly="true" VisibleIndex="7" Width="100px">
																												<Settings AutoFilterCondition="Contains" />
																												<Settings AutoFilterCondition="Contains"></Settings>
																												<EditCellStyle CssClass="GridCss2">
																												</EditCellStyle>
																												<EditFormCaptionStyle CssClass="GridCss2" HorizontalAlign="Left">
																												</EditFormCaptionStyle>
																												<HeaderStyle CssClass="GridCssHeader2" />
																												<CellStyle CssClass="GridCss2">
																												</CellStyle>
																											</dx:GridViewDataTextColumn>
																											<dx:GridViewDataTextColumn Caption="Database Size Threshold (MB)" FieldName="SizeMB"
																												VisibleIndex="9" Width="100px">
																												<PropertiesTextEdit DisplayFormatString="g" Width="50px">
																													<ValidationSettings>
																														<RegularExpression ValidationExpression="(^(100(?:\.0{1,2})?))|(?!^0*$)(?!^0*\.0*$)^\d{1,2}(\.\d{1,2})?$" />
																													</ValidationSettings>
																												</PropertiesTextEdit>
																												<DataItemTemplate>
																													<dx:ASPxTextBox ID="txtDatabaseSizeThreshold" runat="server" Width="150px" Value='<%# Eval("DatabaseSizeThreshold") %>'>
																													</dx:ASPxTextBox>
																												</DataItemTemplate>
																												<Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
																												<HeaderStyle CssClass="GridCssHeader" Wrap="True" />
																												<CellStyle CssClass="GridCss2">
																												</CellStyle>
																											</dx:GridViewDataTextColumn>
																											<dx:GridViewDataTextColumn Caption="White Space Size Threshold (MB)" FieldName="WhiteSpaceMB"
																												VisibleIndex="10" Width="100px">
																												<PropertiesTextEdit DisplayFormatString="g" Width="50px">
																													<ValidationSettings>
																														<RegularExpression ValidationExpression="(^(100(?:\.0{1,2})?))|(?!^0*$)(?!^0*\.0*$)^\d{1,2}(\.\d{1,2})?$" />
																													</ValidationSettings>
																												</PropertiesTextEdit>
																												<DataItemTemplate>
																													<dx:ASPxTextBox ID="txtWhiteSpaceThreshold" runat="server" Width="150px" Value='<%# Eval("WhiteSpaceThreshold") %>'>
																													</dx:ASPxTextBox>
																												</DataItemTemplate>
																												<Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
																												<HeaderStyle CssClass="GridCssHeader" Wrap="True" />
																												<CellStyle CssClass="GridCss2">
																												</CellStyle>
																											</dx:GridViewDataTextColumn>
																											<dx:GridViewCommandColumn Caption="Select" Visible="true" VisibleIndex="3" ShowSelectCheckbox="True">
																												<HeaderStyle CssClass="GridCssHeader" />
																												<CellStyle CssClass="GridCss1">
																												</CellStyle>
																											</dx:GridViewCommandColumn>
																											<%--
																	<dx:GridViewDataTextColumn Caption="Disk Size" FieldName="DiskSize" VisibleIndex="5">
																		<HeaderStyle CssClass="GridCssHeader" />
																		<CellStyle CssClass="GridCss2">
																		</CellStyle>
																	</dx:GridViewDataTextColumn>
																	<dx:GridViewDataTextColumn Caption="Disk Free Space" FieldName="DiskFree" VisibleIndex="6">
																		<HeaderStyle CssClass="GridCssHeader" />
																		<CellStyle CssClass="GridCss2">
																		</CellStyle>
																	</dx:GridViewDataTextColumn>
																	<dx:GridViewDataTextColumn Caption="Percent Free" FieldName="PercentFree" VisibleIndex="7">
																		<HeaderStyle CssClass="GridCssHeader" />
																		<CellStyle CssClass="GridCss2">
																		</CellStyle>
																	</dx:GridViewDataTextColumn>--%>
																										</Columns>
																										<SettingsPager PageSize="50" SEOFriendly="Enabled" Mode="ShowAllRecords">
																										</SettingsPager>
																										<SettingsEditing Mode="Inline">
																										</SettingsEditing>
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
																											<Header ImageSpacing="5px" SortingImageSpacing="5px">
																											</Header>
																											<GroupRow Font-Bold="True">
																											</GroupRow>
																											<AlternatingRow CssClass="GridAltRow" Enabled="True">
																											</AlternatingRow>
																											<LoadingPanel ImageSpacing="5px">
																											</LoadingPanel>
																										</Styles>
																										<StylesEditors ButtonEditCellSpacing="0">
																											<ProgressBar Height="21px">
																											</ProgressBar>
																										</StylesEditors>
																									</dx:ASPxGridView>
																									<table>
																										<tr>
																											<td>
																												<dx:ASPxLabel ID="GBDatabaseTitle" runat="server" Text="Current Database threshold:"
																													CssClass="lblsmallFont" Visible="false">
																												</dx:ASPxLabel>
																											</td>
																											<td>
																												<dx:ASPxTextBox ID="GBDatabaseTextBox" runat="server" Width="170px" Visible="False">
																												</dx:ASPxTextBox>
																											</td>
																										</tr>
																										<tr>
																											<td>
																												<dx:ASPxLabel ID="GBWhiteSpaceTitle" runat="server" Text="Current White Space threshold:"
																													CssClass="lblsmallFont" Visible="false">
																												</dx:ASPxLabel>
																											</td>
																											<td>
																												<dx:ASPxTextBox ID="GBWhiteSpaceTextBox" runat="server" Width="170px" Visible="False">
																												</dx:ASPxTextBox>
																											</td>
																										</tr>
																										<tr>
																											<td colspan="3">
																												&nbsp;
																											</td>
																										</tr>
																									</table>
																									<asp:Label ID="Label1" runat="server" ForeColor="#0033CC" CssClass="lblsmallFont"
																										Text="&nbsp;&lt;b&gt;Database Space &lt;/b&gt;alerts will trigger if any of the databases on the Exchange server fall below the threshold.  Set to 0 for no alerts."></asp:Label>
																								</td>
																							</tr>
																						</table>
																					</td>
																				</tr>
																			</table>
																		</ContentTemplate>
																	</asp:UpdatePanel>
																</dx:PanelContent>
															</PanelCollection>
														</dx:ASPxRoundPanel>
													</td>
												</tr>
											</table>
										</dx:ContentControl>
									</ContentCollection>
								</dx:TabPage>
							</TabPages>
							<LoadingPanelImage Url="~/App_Themes/Glass/Web/Loading.gif">
							</LoadingPanelImage>
							<Paddings PaddingLeft="0px" />
							<Paddings PaddingLeft="0px"></Paddings>
							<ContentStyle>
								<Border BorderColor="#4986A2" />
								<Border BorderColor="#002D96" BorderStyle="Solid" BorderWidth="1px"></Border>
							</ContentStyle>
						</dx:ASPxPageControl>
					<%--</ContentTemplate>
				</asp:UpdatePanel>--%>
			</td>
		</tr>

			<tr>
				<td>
						<div id="errorDiv" class="alert alert-danger" runat="server" style="display: none">
								Error.
						</div>
                </td>
            </tr>
                    <tr>
                     <td>
                      <table>
                       <tr>
                        <td>
							<dx:ASPxButton ID="FormOkButton" runat="server" Text="OK" CssClass="sysButton" OnClick="FormOkButton_Click">
							</dx:ASPxButton>
				        </td>
						<td>
							<dx:ASPxButton ID="FormCancelButton" runat="server" Text="Cancel" CssClass="sysButton" OnClick="FormCancelButton_Click">
							</dx:ASPxButton>
						</td>
                        </tr>
                        </table>
                      </td>
                    </tr>				
	</table>
    <dx:ASPxPopupControl ID="CopyProfilePopupControl" runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
					CssPostfix="Glass" HeaderText="Please Enter Credentials" SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css"
					Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
					Theme="MetropolisBlue" EnableHierarchyRecreation="False" Width="300px">
            <ClientSideEvents Closing="function(s, e) {
        ASPxClientEdit.ClearEditorsInContainerById('addcontentdiv');
    }" />
     <ClientSideEvents PopUp="function(s, e) {
        ASPxClientEdit.ClearEditorsInContainerById('addcontentdiv');
    }" />
					<HeaderStyle>
						<Paddings PaddingLeft="10px" PaddingRight="6px" PaddingTop="1px" />
					</HeaderStyle>
					<ContentCollection>
						<dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server" SupportsDisabledAttribute="True">
                        <div id="addcontentdiv">
							<table>
								<tr>
									<td>
										<dx:ASPxLabel ID="AliasNameLabel" runat="server" CssClass="lblsmallFont" 
                                            Text="Alias Name:" Wrap="False">
										</dx:ASPxLabel>
									</td>
									<td>
										<dx:ASPxTextBox ID="AliasName" runat="server" ClientInstanceName="AliasName" Width="170px">
                                        <ValidationSettings ErrorDisplayMode="ImageWithTooltip">
												<RequiredField ErrorText="Please enter Alias Name" IsRequired="True" />
											</ValidationSettings>
										</dx:ASPxTextBox>
									</td>
								</tr>
								<tr>
									<td>
										<dx:ASPxLabel ID="UserIDLabel" runat="server" Text="User ID:" 
                                            CssClass="lblsmallFont" Wrap="False">
										</dx:ASPxLabel>
									</td>
									<td>
										<dx:ASPxTextBox ID="UserID" runat="server" Width="170px" ClientInstanceName="UserID">
											<ValidationSettings ErrorDisplayMode="ImageWithTooltip">
												<RequiredField ErrorText="Please enter User ID" IsRequired="True" />
											</ValidationSettings>
										</dx:ASPxTextBox>
									</td>
								</tr>
								<tr>
									<td>
										<dx:ASPxLabel ID="PasswordLabel" runat="server" Text="Password:" 
                                            CssClass="lblsmallFont" Wrap="False">
                                            
										</dx:ASPxLabel>
									</td>
									<td>
										<dx:ASPxTextBox ID="Password" runat="server" Width="170px" ClientInstanceName="password"
											Password="True">
											
                                            	<ValidationSettings ErrorDisplayMode="ImageWithTooltip">
												<RequiredField ErrorText="Please enter Password" IsRequired="True" />
											</ValidationSettings>
										</dx:ASPxTextBox>
									</td>
								</tr>
								<tr>
									<td colspan="2">
										<div id="Div3" runat="server" class="alert alert-danger" style="display: none">
											Error:
										</div>
									</td>
								</tr>
							</table>
							<table>
								<tr>
									<td>
										<dx:ASPxButton ID="OKCopy" runat="server" CssClass="sysButton" Text="OK" OnClick="OKCopy_Click"
											ClientInstanceName="goButton" CausesValidation="true">
										</dx:ASPxButton>
									</td>
									<td>
										<dx:ASPxButton ID="Cancel" runat="server" CssClass="sysButton" Text="Cancel" OnClick="Cancel_Click"
											ClientInstanceName="goButton" CausesValidation="false">
										</dx:ASPxButton>
									</td>
								</tr>
							</table>
                        </div>
						</dx:PopupControlContentControl>
					</ContentCollection>
				</dx:ASPxPopupControl>
</asp:Content>
