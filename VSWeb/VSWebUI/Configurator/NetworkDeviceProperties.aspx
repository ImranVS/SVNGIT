<%@ Page Title="VitalSigns Plus-NetworkDeviceProperties" Language="C#" MasterPageFile="~/Site1.Master"
	AutoEventWireup="true" CodeBehind="NetworkDeviceProperties.aspx.cs" Inherits="VSWebUI.Configurator.NetworkDeviceProperties" %>

<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.Web" TagPrefix="dx" %>




<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.Web" TagPrefix="dx1" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
	<link href='http://fonts.googleapis.com/css?family=Francois One' rel='stylesheet'
		type='text/css' />
	<link rel="stylesheet" type="text/css" href="../css/vswebforms.css" />
	<script src="../js/jquery-1.9.1.js" type="text/javascript"></script>
	<script src="../js/bootstrap.min.js" type="text/javascript"></script>
	<script type="text/javascript">
		$(document).ready(function () {
			var v = document.getElementById("ContentPlaceHolder1_ASPxPageControl1_ASPxRoundPanel4_PasswordTextBox_I")
			v.type = 'password';	
			});
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
		.style1
		{
			width: 100%;
		}
	</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

	
	<table width="100%">
		<tr>
			<td>
				<div class="header" id="lblServer" runat="server">
					Network Device</div>
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
		<tr>
			<td colspan="2">
				<dx:ASPxPageControl ID="ASPxPageControl1" runat="server" ActiveTabIndex="0" Theme="Glass"
					Width="100%" EnableHierarchyRecreation="False">
					<TabPages>
						<dx:TabPage Text="Device Attributes">
							<ContentCollection>
								<dx:ContentControl runat="server">
									<table width="100%">
										<tr>
											<td>
												<dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
													CssPostfix="Glass" GroupBoxCaptionOffsetY="-24px" HeaderText="Device Attributes"
													SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" Width="100%">
													<ContentPaddings PaddingBottom="10px" PaddingLeft="4px" PaddingTop="10px" />
													<HeaderStyle Height="23px">
													</HeaderStyle>
													<PanelCollection>
														<dx:PanelContent runat="server">
															<table width="100%">
																<tr>
																	<td rowspan="4" valign="top">
                                                                        <img id="Img1" runat="server" height="150" width="100" />
                                                                    </td>
																	<td>
                                                                        <dx:ASPxLabel ID="ASPxLabel2" runat="server" CssClass="lblsmallFont" 
                                                                            Text="Name:">
                                                                        </dx:ASPxLabel>
                                                                    </td>
																	<td>
																		<dx:ASPxTextBox ID="NameTextBox" runat="server" 
																			Width="170px">
																			<ValidationSettings ErrorDisplayMode="None" ValidationGroup="OK">
																				<RequiredField IsRequired="True" />
																			</ValidationSettings>
																		</dx:ASPxTextBox>
																	</td>
																	<td>
																		<dx:ASPxLabel ID="ASPxLabel5" runat="server" CssClass="lblsmallFont" Text="Category:">
																		</dx:ASPxLabel>
																	</td>
																	<td>
																		<dx:ASPxTextBox ID="CategoryTextBox" runat="server" Width="170px">
																			<ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorText="Category may not be empty."
																				SetFocusOnError="True">
																				<RequiredField ErrorText="" IsRequired="True" />
																			</ValidationSettings>
																		</dx:ASPxTextBox>
																	</td>
																	<td colspan="2">
																		<dx:ASPxCheckBox ID="EnabledCheckBox" runat="server" CheckState="checked" CssClass="lblsmallFont"
																			Text="Enabled for scanning" Wrap="False">
																			<ValidationSettings ErrorDisplayMode="None">
																			</ValidationSettings>
																		</dx:ASPxCheckBox>
																	</td>
																</tr>
																<tr>
                                                                 
																	<td>
                                                                        <dx:ASPxLabel ID="ASPxLabel3" runat="server" CssClass="lblsmallFont" 
                                                                            Text="IP or Host Name:" Wrap="False">
                                                                        </dx:ASPxLabel>
                                                                    </td>
                                                                    <td>
                                                                    <table><tr><td>
                                                                        <dx:ASPxTextBox ID="IPTextBox" runat="server" Width="170px" >
																			<ValidationSettings CausesValidation="True" ErrorDisplayMode="ImageWithTooltip" ErrorText="Please enter valid IP or Host Name.">
																			
																				<RequiredField IsRequired="True"  ErrorText="Please enter IP or Host Name."/>
																			</ValidationSettings>
																		</dx:ASPxTextBox>
                                                                        </td></tr></table>
                                                                    </td>
																	<td>
																		<dx:ASPxLabel ID="ASPxLabel6" runat="server" CssClass="lblsmallFont" Text="Location:">
																		</dx:ASPxLabel>
																	</td>
																	<td>
																		<dx:ASPxComboBox ID="LocComboBox" runat="server">
																			<ValidationSettings CausesValidation="True" Display="Dynamic" ErrorDisplayMode="ImageWithTooltip" >
																				<ErrorImage ToolTip="Select Location">
																				</ErrorImage>
																				<RequiredField ErrorText="Select Location" IsRequired="True" />
																			</ValidationSettings>
																		</dx:ASPxComboBox>
																	</td>
																	<td  colspan="2">
																		&nbsp;</td>
																</tr>
																<tr>
																	<td>
                                                                        <dx:ASPxLabel ID="ASPxLabel4" runat="server" CssClass="lblsmallFont" 
                                                                            Text="Description:">
                                                                        </dx:ASPxLabel>
                                                                    </td>
																	<td>
																		<dx:ASPxTextBox ID="DescriptionTextBox" runat="server" 
																			Width="170px">
																			<ValidationSettings ErrorDisplayMode="None"  >
																				<RequiredField IsRequired="True" />
																			</ValidationSettings>
																		</dx:ASPxTextBox>
																	</td>
																	<td>
																		<dx:ASPxLabel ID="ASPxLabel1" runat="server" CssClass="lblsmallFont" 
                                                                            Text="Network Type:" Wrap="False">
																		</dx:ASPxLabel>
																	</td>
																	<td>
																		<dx:ASPxComboBox ID="NetworkTypeCombobox" runat="server">
																			
																			<Items>
																				<dx:ListEditItem Text="Cisco" Value=1 />
																				<dx:ListEditItem Text="Juniper ScreenOS" Value=2 />
																				<dx:ListEditItem Text="Juniper Junos" Value=3 />
																			</Items>
                                                                        
                                                                            <ValidationSettings CausesValidation="True" Display="Dynamic" ErrorDisplayMode ="ImageWithTooltip" ErrorText = "Select Network Type" >
																				
<RequiredField  ErrorText="Select Network Type" IsRequired="True" />
																			</ValidationSettings>
																		</dx:ASPxComboBox>
																	</td>
																	<td>
																		&nbsp;
																	</td>
																	<td>
																		&nbsp;
																	</td>
																</tr>
															    <tr>
                                                                    <td>
                                                                        <dx:ASPxLabel ID="lblSelectImage" runat="server" CssClass="lblsmallFont" 
                                                                            Text="Image for Display:"  Wrap="False">
                                                                        </dx:ASPxLabel>
                                                                    </td>
																	
                                                                    <td style="padding:0px 0px 0px 4px">
                                                                        <dx:ASPxComboBox ID="CredentialsComboBox" Width="170px" Height="21px" runat="server" AutoPostBack="True" 
                                                                            OnSelectedIndexChanged="CredentialsComboBox_SelectedIndexChanged" 
                                                                            TextField="Name"  ValidationGroup="OK"><ValidationSettings>
                        <RequiredField IsRequired="True"  ErrorText="Please select Image"/>
                    </ValidationSettings>
                                                                        </dx:ASPxComboBox>
                                                                    </td>
																	
                                                                    <td colspan="2">
                                                                        <dx1:ASPxCheckBox ID="IncludeOnDashBoardCheckBox" runat="server" 
                                                                            CheckState="Unchecked" CssClass="lblsmallFont" 
                                                                            Text="Display on Dashboard as 'Network Infrastructure'" Wrap="False">
                                                                            <ValidationSettings ErrorDisplayMode="None">
                                                                            </ValidationSettings>
                                                                        </dx1:ASPxCheckBox>
                                                                    </td>
                                                                    <td>
                                                                        &nbsp;</td>
                                                                    <td>
                                                                        &nbsp;</td>
                                                                </tr>
															</table>
														</dx:PanelContent>
													</PanelCollection>
												</dx:ASPxRoundPanel>
											</td>
										</tr>
										<tr>
											<td>
												<dx:ASPxRoundPanel ID="ASPxRoundPanel2" runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
													CssPostfix="Glass" GroupBoxCaptionOffsetY="-24px" HeaderText="Scan Settings"
													SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" Width="100%">
													<ContentPaddings PaddingBottom="10px" PaddingLeft="4px" PaddingTop="10px" />
													<HeaderStyle Height="23px">
													</HeaderStyle>
													<PanelCollection>
														<dx:PanelContent runat="server">
															<table>
																<tr>
																	<td>
																		<dx:ASPxLabel ID="ASPxLabel7" runat="server" CssClass="lblsmallFont" Text="Scan Interval:"
																			Width="80px" Wrap="False">
																		</dx:ASPxLabel>
																	</td>
																	<td>
																		<dx:ASPxTextBox ID="ScanIntervalTextBox" runat="server" Width="40px">
																		<%--	<MaskSettings Mask="&lt;8..99999&gt;" />--%>
                                                                          <MaskSettings Mask="&lt;0..99999&gt;" />
																			<ValidationSettings CausesValidation="True" ErrorDisplayMode="ImageWithTooltip" SetFocusOnError="True">
																				<RegularExpression ErrorText="Please enter a numeric value using the numbers only (0-9)."
																					ValidationExpression="^\d+$" />
																				<RequiredField IsRequired="True" />
																			</ValidationSettings>
																		</dx:ASPxTextBox>
																	</td>
																	<td>
																		<dx:ASPxLabel ID="ASPxLabel8" runat="server" CssClass="lblsmallFont" Height="16px"
																			Text="minutes">
																		</dx:ASPxLabel>
																	</td>
																	<td>
																		&nbsp;
																	</td>
																	<td>
																		<dx:ASPxLabel ID="ASPxLabel9" runat="server" CssClass="lblsmallFont" Text="Off-Hours Scan Interval:"
																			Wrap="False">
																		</dx:ASPxLabel>
																	</td>
																	<td>
																		<dx:ASPxTextBox ID="OffScanTextBox" runat="server" Width="40px">
																			<%--<MaskSettings Mask="&lt;30..9999&gt;" />--%>
                                                                              <MaskSettings Mask="&lt;0..99999&gt;" />
																			<ValidationSettings CausesValidation="True" ErrorDisplayMode="ImageWithTooltip" SetFocusOnError="True">
																				<RegularExpression ErrorText="Please enter a numeric value using the numbers only (0-9)."
																					ValidationExpression="^\d+$" />
																				<RequiredField IsRequired="True" />
																			</ValidationSettings>
																		</dx:ASPxTextBox>
																	</td>
																	<td>
																		<dx:ASPxLabel ID="ASPxLabel11" runat="server" CssClass="lblsmallFont" Text="minutes">
																		</dx:ASPxLabel>
																	</td>
																</tr>
																<tr>
																	<td>
																		<dx:ASPxLabel ID="ASPxLabel10" runat="server" CssClass="lblsmallFont" Text="Retry Interval:"
																			Wrap="False">
																		</dx:ASPxLabel>
																	</td>
																	<td width="50px">
																		<dx:ASPxTextBox ID="RetryIntervalTextBox" runat="server" Width="40px">
																			<%--<MaskSettings Mask="&lt;2..1000&gt;" />--%>
                                                                              <MaskSettings Mask="&lt;1..99999&gt;" />
																			<ValidationSettings CausesValidation="True" ErrorDisplayMode="ImageWithTooltip" SetFocusOnError="True">
																				<RegularExpression ErrorText="Please enter a numeric value using the numbers only (1-9)."
																					ValidationExpression="^\d+$" />
																				<RequiredField IsRequired="True" />
																			</ValidationSettings>
																		</dx:ASPxTextBox>
																	</td>
																	<td>
																		<dx:ASPxLabel ID="ASPxLabel15" runat="server" CssClass="lblsmallFont" Text="minutes">
																		</dx:ASPxLabel>
																	</td>
																	<td>
																		&nbsp;
																	</td>
																	<td>
																		<dx:ASPxLabel ID="ASPxLabel12" runat="server" CssClass="lblsmallFont" Text="Response Threshold:"
																			Wrap="False">
																		</dx:ASPxLabel>
																	</td>
																	<td>
																		<dx:ASPxTextBox ID="ResponseThrTextBox" runat="server" Width="40px">
																			<%--<MaskSettings Mask="&lt;250..99999&gt;" />--%>
                                                                              <MaskSettings Mask="&lt;0..99999&gt;" />
																			<ValidationSettings CausesValidation="True" ErrorDisplayMode="ImageWithTooltip" SetFocusOnError="True">
																				<RegularExpression ErrorText="Please enter a numeric value using the numbers only (0-9)."
																					ValidationExpression="^\d+$" />
																			</ValidationSettings>
																		</dx:ASPxTextBox>
																	</td>
																	<td>
																		<dx:ASPxLabel ID="ASPxLabel13" runat="server" CssClass="lblsmallFont" Text="milliseconds">
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
                                            <td>
                                                <dx:ASPxRoundPanel ID="ASPxRoundPanel4" runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
                                                    CssPostfix="Glass" GroupBoxCaptionOffsetY="-24px" SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css"
                                                    Width="100%" HeaderText="Username/Password">
                                                    <ContentPaddings PaddingBottom="10px" PaddingTop="10px" PaddingLeft="4px" />
                                                    <ContentPaddings PaddingLeft="4px" PaddingTop="10px" PaddingBottom="10px"></ContentPaddings>
                                                    <HeaderStyle Height="23px">
                                                    </HeaderStyle>
                                                    <PanelCollection>
                                                        <dx:PanelContent ID="PanelContent5" runat="server" SupportsDisabledAttribute="True">
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <table>
                                                                        </table>
                                                                        <dx:ASPxLabel ID="ASPxLabel14" runat="server" Text="Username:" CssClass="lblsmallFont">
                                                                        </dx:ASPxLabel>
                                                                    </td>
                                                                    <td>
                                                                        <dx:ASPxTextBox ID="UserNameTextBox" runat="server" Width="170px" AutoCompleteType="Disabled">
                                                                            <ValidationSettings ErrorDisplayMode="None">
                                                                            </ValidationSettings>
                                                                        </dx:ASPxTextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="margin-left: 40px">
                                                                        <dx:ASPxLabel ID="ASPxLabel16" runat="server" Text="Password:" CssClass="lblsmallFont">
                                                                        </dx:ASPxLabel>
                                                                    </td>
                                                                    <td>
                                                                        <dx:ASPxTextBox ID="PasswordTextBox" runat="server" Width="170px" MaskSettings-PromptChar="#"
																			 CssClass="header-login-input" ValidationGroup="A"
                                                                            AutoCompleteType="Disabled">
<MaskSettings PromptChar="#"></MaskSettings>

                                                                            <ValidationSettings ErrorDisplayMode="None">
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
					</TabPages>
					<TabStyle Font-Bold="True">
					</TabStyle>
				</dx:ASPxPageControl>
			</td>
		</tr>
	</table>
	<table width="100%">
	<tr>
		<td valign="top" colspan="3">
			<div id="errorDiv" runat="server" class="alert alert-danger" style="display: none">
				Error attempting to update the status table.
			</div>
            <div id="successDiv" runat="server" class="alert alert-success" style="display: none">
                Success.
            </div>
			 <div id="Div1" runat="server" class="alert alert-success" style="display: none">
                Success.
            </div>
		</td>
	</tr>
	
	<tr>
		<td>
			<table>
				<tr>
					<td valign="top">
						<dx:ASPxButton ID="PingButton" runat="server"  CausesValidation="true" CssClass="sysButton"   ValidationGroup="Ip"
                        OnClick="PingButton_Click" 
							Text="Ping">
						</dx:ASPxButton>
					</td>
					<td>
						&nbsp;
					</td>
					<td valign="top">
						<dx:ASPxButton ID="FormOKButton" runat="server" AutoPostBack="False" CssClass="sysButton"
                        OnClick="FormOKButton_Click" 
							Text="OK">
							  <ClientSideEvents Click="function(s, e) { if (ASPxClientEdit.ValidateGroup('OK')  &  ASPxClientEdit.ValidateGroup('Ip'))  { debugger; } }"></ClientSideEvents>
						</dx:ASPxButton>
					</td>
					<td>
						<dx:ASPxButton ID="FormCancelButton" runat="server" CausesValidation="False" CssClass="sysButton"
                        OnClick="FormCancelButton_Click" 
							Text="Cancel">
						</dx:ASPxButton>
					</td>
				</tr>
			</table>
			<dx:ASPxPopupControl ID="ErrorMessagePopupControl" runat="server" AllowDragging="True"
				ClientInstanceName="pcErrorMessage" CloseAction="CloseButton" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
				CssPostfix="Glass" EnableAnimation="False" EnableViewState="False" HeaderText="Validation Failure"
				Height="150px" Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
				SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" Width="300px" 
                Theme="MetropolisBlue">
				<LoadingPanelImage Url="~/App_Themes/Glass/Web/Loading.gif">
				</LoadingPanelImage>
				<HeaderStyle>
					<Paddings PaddingLeft="10px" PaddingRight="6px" PaddingTop="1px" />
				</HeaderStyle>
				<ContentCollection>
					<dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server" SupportsDisabledAttribute="True">
						<dx:ASPxPanel ID="Panel2" runat="server" DefaultButton="btOK">
							<PanelCollection>
								<dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">
									<div style="min-height: 70px;">
										<dx:ASPxLabel ID="ErrorMessageLabel" runat="server">
										</dx:ASPxLabel>
									</div>
									<div>
										<table cellpadding="0" cellspacing="0" width="100%">
											<tr>
												<td align="center">
													<dx:ASPxButton ID="ValidationOkButton" runat="server" AutoPostBack="False" CssClass="sysButton"
														Text="OK">
														<ClientSideEvents Click="function(s, e) { pcErrorMessage.Hide(); }" />
													</dx:ASPxButton>
													<dx:ASPxButton ID="ValidationUpdatedButton" runat="server" AutoPostBack="False" CssClass="sysButton" CausesValidation="False"
														OnClick="ValidationUpdatedButton_Click"
														Text="OK" Visible="False">
														<ClientSideEvents Click="function(s, e) { pcErrorMessage.Hide(); }" />
													</dx:ASPxButton>
												</td>
											</tr>
										</table>
									</div>
								</dx:PanelContent>
							</PanelCollection>
						</dx:ASPxPanel>
					</dx:PopupControlContentControl>
				</ContentCollection>
			</dx:ASPxPopupControl>
		</td>
	</tr>
	</table>
</asp:Content>
