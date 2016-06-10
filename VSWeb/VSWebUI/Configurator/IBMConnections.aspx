<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="IBMConnections.aspx.cs" Inherits="VSWebUI.Configurator.IBMConnections" %>
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
	<table width="100%">
		<tr>
			<td>
				<div class="header" id="lblServer" runat="server">
					IBM Connections</div>
			</td>
			<td valign="top" align="right">
				<table>
					<tr>
						<td>
							<dx:aspxmenu ID="ASPxMenu1" runat="server" EnableTheming="True" HorizontalAlign="Right"
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
							</dx:aspxmenu>
						</td>
					</tr>
				</table>
			</td>
		</tr>
		<tr>
			<td colspan="2">
				<dx:aspxpagecontrol Font-Bold="True" ID="ASPxPageControl1" runat="server" ActiveTabIndex="0"
					CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass" SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css"
					TabSpacing="0px" Width="100%" EnableHierarchyRecreation="False" >
					<TabPages >
						<dx:TabPage Text="Server Attributes">
							<TabImage Url="~/images/icons/information.png" />
							<TabImage Url="~/images/icons/information.png">
							</TabImage>
							<ContentCollection>
								<dx:ContentControl ID="ContentControl1" runat="server" SupportsDisabledAttribute="True">
									<table class="navbarTbl">
										<tr>
											<td>
												<dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
													CssPostfix="Glass" GroupBoxCaptionOffsetY="-24px" HeaderText="Server Properties"
													SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" Width="100%">
													<ContentPaddings PaddingBottom="10px" PaddingLeft="4px" PaddingTop="10px" />
													<ContentPaddings PaddingLeft="4px" PaddingTop="10px" PaddingBottom="10px"></ContentPaddings>
													<HeaderStyle Height="23px">
													</HeaderStyle>
													<PanelCollection>
														<dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">
															<table align="left" class="style3">
																<tr>
																	<td align="left">
																		<dx:ASPxLabel ID="ASPxLabel51" runat="server" CssClass="lblsmallFont" Text="Server Name:">
																		</dx:ASPxLabel>
																	</td>
																	<td align="left">
																		<dx:ASPxTextBox ID="ServerNameTextBox" runat="server" AutoPostBack="True" CssClass="txtsmall"
																			Enabled="False" OnTextChanged="ServerNameTextBox_TextChanged" ReadOnly="True"
																			Width="170px">
																			<ValidationSettings>
																				<RequiredField IsRequired="True" />
																				<RequiredField IsRequired="True"></RequiredField>
																			</ValidationSettings>
																		</dx:ASPxTextBox>
																	</td>
																	<td align="left">
																		&nbsp;
																	</td>
																	<td align="left">
																		<dx:ASPxLabel ID="ASPxLabel68" runat="server" CssClass="lblsmallFont" Text="Category:">
																		</dx:ASPxLabel>
																	</td>
																	<td align="left">
																		<dx:ASPxTextBox ID="CategoryTextBox" runat="server" AutoPostBack="True" CssClass="txtsmall"
																			Width="170px">
																			<ValidationSettings>
																				<RequiredField ErrorText="Enter category" IsRequired="True" />
																				<RequiredField IsRequired="True" ErrorText="Enter category"></RequiredField>
																			</ValidationSettings>
																		</dx:ASPxTextBox>
																	</td>
																	<td align="left" colspan="2">
																		<dx:ASPxCheckBox ID="EnabledForScanningCheckBox" runat="server" CheckState="Unchecked"
																			CssClass="lblsmallFont" Text="Enabled for scanning">
																		</dx:ASPxCheckBox>
																	</td>
																</tr>
																<tr>
																	<td align="left">
																		<dx:ASPxLabel ID="ASPxLabel52" runat="server" CssClass="lblsmallFont" Text="IP Address or Host Name:">
																		</dx:ASPxLabel>
																	</td>
																	<td align="left" colspan="1">
																		<dx:ASPxTextBox ID="IPAddressTextBox" runat="server" CssClass="txtsmall" Enabled="False"
																			Width="170px">
																		</dx:ASPxTextBox>
																	</td>
																	<td align="left">
																		&nbsp;
																	</td>
																	<td align="left">
																		<dx:ASPxLabel ID="ASPxLabel64" runat="server" CssClass="lblsmallFont" Height="16px"
																			Text="Location:">
																		</dx:ASPxLabel>
																	</td>
																	<td align="left">
																		<dx:ASPxTextBox ID="LocationTextBox" runat="server" AutoPostBack="True" CssClass="txtsmall"
																			Enabled="False" OnTextChanged="LocationTextBox_TextChanged" Width="170px">
																			<ValidationSettings ErrorDisplayMode="None">
																				<RequiredField IsRequired="True" />
																				<RequiredField IsRequired="True"></RequiredField>
																			</ValidationSettings>
																		</dx:ASPxTextBox>
																	</td>
																	<td align="left" colspan="2">
																		<dx:ASPxCheckBox ID="ThisserverrequiresSSLCheckBox" runat="server" CheckState="Unchecked"
																			CssClass="lblsmallFont" Text="This server requires SSL">
																		</dx:ASPxCheckBox>
																	</td>
																</tr>
																<tr>
																	<td align="left">
																		<dx:ASPxLabel ID="ASPxLabel53" runat="server" CssClass="lblsmallFont" Text="Description:">
																		</dx:ASPxLabel>
																	</td>
																	<td align="left">
																		<dx:ASPxTextBox ID="DescriptionTextBox" runat="server" AutoPostBack="True" CssClass="txtsmall"
																			Enabled="False" OnTextChanged="DescriptionTextBox_TextChanged" Width="170px">
																			<ValidationSettings ErrorDisplayMode="None">
																				<RequiredField IsRequired="True" />
																				<RequiredField IsRequired="True"></RequiredField>
																			</ValidationSettings>
																		</dx:ASPxTextBox>
																	</td>
																	<%--<td align="left">
                                                                        &nbsp;</td>--%>
																</tr>
																<tr>
																	<td>
																				<dx:ASPxLabel ID="ASPxLabel6" runat="server" CssClass="lblsmallFont" Text="Credentials:">
																				</dx:ASPxLabel>
																			</td>
																			<td>
																				<dx:ASPxComboBox ID="CredentialComboBox" runat="server" AutoPostBack="false">
                                                                                <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ValidationGroup="Creds">
																				
																				<RequiredField IsRequired="True" ErrorText="Please select Credentials."></RequiredField>
																			</ValidationSettings>
																				</dx:ASPxComboBox>
																			</td>
																	<td align="left">
                                                                        &nbsp;</td>
                                                                    <td align="left">
                                                                        &nbsp;
                                                                    </td>
                                                                    <td align="left">
                                                                        &nbsp;
                                                                    </td>
                                                                    <td align="left">
                                                                        &nbsp;
                                                                    </td>
																	<td>
																		<asp:Label ID="lblServerID" runat="server" Visible="False"></asp:Label>
																	</td>
																</tr>
                                                                <tr>
                                                                <td>
                                                                	<dx:ASPxButton ID="ASPxButton1" runat="server" Text="Add Credentials" CssClass="sysButton"
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
										<tr>
											<td>
												<dx:ASPxRoundPanel ID="ASPxRoundPanel5" runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
													CssPostfix="Glass" GroupBoxCaptionOffsetY="-24px" HeaderText="Scan Settings"
													SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" Width="100%">
													<ContentPaddings PaddingBottom="10px" PaddingLeft="4px" PaddingTop="10px" />
													<ContentPaddings PaddingLeft="4px" PaddingTop="10px" PaddingBottom="10px"></ContentPaddings>
													<HeaderStyle Height="23px">
													</HeaderStyle>
													<PanelCollection>
														<dx:PanelContent ID="PanelContent3" runat="server" SupportsDisabledAttribute="True">
															<table>
																<tr>
																	<td>
																		<dx:ASPxLabel ID="ASPxLabel56" runat="server" CssClass="lblsmallFont" Text="Scan Interval:"
																			Wrap="False">
																		</dx:ASPxLabel>
																	</td>
																	<td>
																		<dx:ASPxTextBox ID="ScanIntervalTextBox" runat="server" CssClass="txtsmall" Width="40px">
																			<MaskSettings Mask="&lt;0..9999&gt;" />
																			<MaskSettings Mask="&lt;0..9999&gt;"></MaskSettings>
																			<ValidationSettings CausesValidation="True" ErrorDisplayMode="ImageWithTooltip">
																				<RequiredField IsRequired="True" />
																				<RegularExpression ErrorText="Please enter a numeric value using the numbers only (0-9)."
																					ValidationExpression="^\d+$" />
																				<RequiredField IsRequired="True"></RequiredField>
																				<RegularExpression ErrorText="Please enter a numeric value using the numbers only (0-9)."
																					ValidationExpression="^\d+$"></RegularExpression>
																			</ValidationSettings>
																		</dx:ASPxTextBox>
																	</td>
																	<td>
																		<dx:ASPxLabel ID="ASPxLabel60" runat="server" CssClass="lblsmallFont" Text="minutes">
																		</dx:ASPxLabel>
																	</td>
																	<td>
																		&nbsp;
																	</td>
																	<td>
																		<dx:ASPxLabel ID="ASPxLabel59" runat="server" CssClass="lblsmallFont" Text="Response Threshold:"
																			Wrap="False">
																		</dx:ASPxLabel>
																	</td>
																	<td>
																		<dx:ASPxTextBox ID="ResponseThresholdTextBox" runat="server" CssClass="txtmed">
																			<MaskSettings Mask="&lt;0..99999&gt;" />
																			<MaskSettings Mask="&lt;0..99999&gt;"></MaskSettings>
																			<ValidationSettings ErrorDisplayMode="ImageWithTooltip" SetFocusOnError="True">
																				<RequiredField IsRequired="True" />
																				<RegularExpression ErrorText="Please enter a numeric value using the numbers only (0-9)."
																					ValidationExpression="^\d+$" />
																				<RequiredField IsRequired="True"></RequiredField>
																				<RegularExpression ErrorText="Please enter a numeric value using the numbers only (0-9)."
																					ValidationExpression="^\d+$"></RegularExpression>
																			</ValidationSettings>
																		</dx:ASPxTextBox>
																	</td>
																	<td>
																		<dx:ASPxLabel ID="ASPxLabel63" runat="server" CssClass="lblsmallFont" Text="milliseconds">
																		</dx:ASPxLabel>
																	</td>
																</tr>
																<tr>
																	<td>
																		<dx:ASPxLabel ID="ASPxLabel57" runat="server" CssClass="lblsmallFont" Text="Retry Interval:"
																			Wrap="False">
																		</dx:ASPxLabel>
																	</td>
																	<td>
																		<dx:ASPxTextBox ID="RetryIntervalTextBox" runat="server" CssClass="txtsmall" Width="40px">
																			<MaskSettings Mask="&lt;1..99999&gt;" />
																			
																			<ValidationSettings CausesValidation="True" ErrorDisplayMode="ImageWithTooltip" SetFocusOnError="True">
																				<RequiredField IsRequired="True" />
																				<RegularExpression ErrorText="Please enter a numeric value using the numbers only (1-9)."
																					ValidationExpression="^\d+$" />
																				<RequiredField IsRequired="True"></RequiredField>
																				
																			</ValidationSettings>
																		</dx:ASPxTextBox>
																	</td>
																	<td>
																		<dx:ASPxLabel ID="ASPxLabel61" runat="server" CssClass="lblsmallFont" Text="minutes">
																		</dx:ASPxLabel>
																	</td>
																	<td>
																		&nbsp;
																	</td>
																	<td>
																		<dx:ASPxLabel ID="ASPxLabel66" runat="server" CssClass="lblsmallFont" Text="Failures before Alert:"
																			Wrap="False">
																		</dx:ASPxLabel>
																	</td>
																	<td>
																		<dx:ASPxTextBox ID="SrvAtrFailBefAlertTextBox" runat="server" CssClass="txtsmall"
																			Text="2">
																			<MaskSettings ErrorText="Enter number between 1 to 100" Mask="&lt;0..9999&gt;" ShowHints="True" />
																			<MaskSettings Mask="&lt;0..9999&gt;" ErrorText="Enter number between 1 to 100" ShowHints="True">
																			</MaskSettings>
																			<ValidationSettings CausesValidation="True" ErrorDisplayMode="ImageWithTooltip" SetFocusOnError="True">
																				<RequiredField ErrorText="Please enter the failure threshold.  (How many times can the server be down before an alert is sent.)"
																					IsRequired="True" />
																				<RegularExpression ErrorText="Please enter a numeric value using the numbers only (0-9)."
																					ValidationExpression="^\d+$" />
																				<RequiredField IsRequired="True" ErrorText="Please enter the failure threshold.  (How many times can the server be down before an alert is sent.)">
																				</RequiredField>
																				<RegularExpression ErrorText="Please enter a numeric value using the numbers only (0-9)."
																					ValidationExpression="^\d+$"></RegularExpression>
																			</ValidationSettings>
																		</dx:ASPxTextBox>
																	</td>
																	<td>
																		<dx:ASPxLabel ID="ASPxLabel67" runat="server" CssClass="lblsmallFont" Text="consecutive failures"
																			Wrap="False">
																		</dx:ASPxLabel>
																	</td>
																</tr>
																<tr>
																	<td>
																		<dx:ASPxLabel ID="ASPxLabel58" runat="server" CssClass="lblsmallFont" Text="Off-Hours Scan Interval:">
																		</dx:ASPxLabel>
																	</td>
																	<td>
																		<dx:ASPxTextBox ID="OffHoursScanIntervalTextBox" runat="server" CssClass="txtsmall"
																			Width="40px">
																			<MaskSettings Mask="&lt;0..99999&gt;" />
																			<MaskSettings Mask="&lt;0..99999&gt;"></MaskSettings>
																			<ValidationSettings CausesValidation="True" ErrorDisplayMode="ImageWithTooltip" SetFocusOnError="True">
																				<RequiredField IsRequired="True" />
																				<RegularExpression ErrorText="Please enter a numeric value using the numbers only (0-9)."
																					ValidationExpression="^\d+$" />
																				<RequiredField IsRequired="True"></RequiredField>
																				<RegularExpression ErrorText="Please enter a numeric value using the numbers only (0-9)."
																					ValidationExpression="^\d+$"></RegularExpression>
																			</ValidationSettings>
																		</dx:ASPxTextBox>
																	</td>
																	<td>
																		<dx:ASPxLabel ID="ASPxLabel62" runat="server" CssClass="lblsmallFont" Text="minutes">
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
										<td>
												<dx:ASPxRoundPanel ID="ASPxRoundPanel9" runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
													CssPostfix="Glass" GroupBoxCaptionOffsetY="-24px" HeaderText="Chat Settings"
													SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" Width="100%" Visible="False">
													<ContentPaddings PaddingBottom="10px" PaddingLeft="4px" PaddingTop="10px" />
													<ContentPaddings PaddingLeft="4px" PaddingTop="10px" PaddingBottom="10px"></ContentPaddings>
													<HeaderStyle Height="23px">
													</HeaderStyle>
													<PanelCollection>
														<dx:PanelContent ID="chatPanel" runat="server" SupportsDisabledAttribute="True">
														<table>
													
														

														<tr>
														<td>
																		<dx:ASPxLabel ID="IBMConnectionsURLLabel" runat="server" CssClass="lblsmallFont" Text="IBM Connections URL :">
																		</dx:ASPxLabel>
																	</td>
													    <td>
																		<dx:ASPxTextBox ID="IBMConnectionsURLTextBox" runat="server" AutoPostBack="True" CssClass="txtsmall"
																			 Width="170px">
																			<ValidationSettings ErrorDisplayMode="None">
																				<RequiredField IsRequired="True" />
																				<RequiredField IsRequired="True"></RequiredField>
																			</ValidationSettings>
																		</dx:ASPxTextBox>
																	</td>
																	<td> &nbsp</td>
																	<td> &nbsp</td>
																	<td >
														          <dx:ASPxCheckBox ID="chkChatSimulation" runat="server" 
																			 CssClass="lblsmallFont" 
																			Text="Test Chat Simulation">
																		</dx:ASPxCheckBox>
														</td>
																	</tr>
                                                        <tr>
														<td>
																		<dx:ASPxLabel ID="PortNumberLabel" runat="server" CssClass="lblsmallFont" Text="Port Number :">
																		</dx:ASPxLabel>
																	</td>
													    <td>
																		<dx:ASPxTextBox ID="PortNumberTextBox" runat="server" CssClass="txtsmall"
																			Width="40px">
																			<MaskSettings Mask="&lt;0..99999&gt;" />
																			<MaskSettings Mask="&lt;0..99999&gt;"></MaskSettings>
																			
																		</dx:ASPxTextBox>
																	</td>
																	</tr>
														</table>
														<div class="info">
															Two User Credentials are required to simulate a chat session between two users. 
                                                            IC User1 and IC User2 reflect existing users that were previously configured to 
                                                            gather IBM Connections  Statistics. To edit or view these users, please go to Stored 
                                                            Passwords &amp; Options -&gt; Server credentials.
														</div>

															<table>
																<tr>
																	<td>
																		<dx:ASPxLabel ID="ASPxLabel11" runat="server" CssClass="lblsmallFont" Text="User 1 Credentials:"
																			>
																		</dx:ASPxLabel>
																	</td>
																	<td>
																		<dx:ASPxComboBox ID="cbChatUser1" runat="server" AutoPostBack="false" >
																		</dx:ASPxComboBox>
																	</td>
																	
																</tr>
																<tr>
																	<td>
																		<dx:ASPxLabel ID="ASPxLabel12" runat="server" CssClass="lblsmallFont" Text="User 2 Credentials:"
																			>
																		</dx:ASPxLabel>
																	</td>
																	<td>
																		<dx:ASPxComboBox ID="cbChatUser2" runat="server" AutoPostBack="false" >
																		</dx:ASPxComboBox>
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
						<dx:TabPage Text="Simulation Tests" Visible="true">
							<TabImage Url="~/images/application_view_tile.png" />
							<TabImage Url="~/images/application_view_tile.png">
							</TabImage>
							<ContentCollection>
								<dx:ContentControl ID="ContentControl7" runat="server" SupportsDisabledAttribute="True">
									<table>
										<tr>
											<td>
												<table width="150%" >
													<tr>
														<td>
															<div id="Div1" class="alert alert-success" runat="server" style="display: none">
																Success.
															</div>
														</td>
													</tr>
													
													<tr>
														<td>
															<dx:ASPxCallback ID="cb" runat="server" ClientInstanceName="cb" OnCallback="cb_callback" />
															<dx:ASPxGridView ID="Tests" runat="server" AutoGenerateColumns="False" ClientInstanceName="TestsGridView"
																Width="85%" EnableTheming="True" KeyFieldName="Id" Visible="false" Theme="Office2003Blue" OnPreRender="Tests_PreRender"
																>
																<SettingsBehavior ProcessSelectionChangedOnServer="True" >
																</SettingsBehavior>
															
																<TotalSummary>
																	<dx:ASPxSummaryItem FieldName="Type" SummaryType="Count" ShowInColumn="Type" DisplayFormat="{0} Item(s)" />
																</TotalSummary>
																<Columns>
																	<dx:GridViewDataTextColumn Caption="Id" FieldName="Id" VisibleIndex="0" Visible="false">
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
																	<dx:GridViewDataTextColumn Caption="ServerId" FieldName="ServerId" VisibleIndex="1"
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
																	<dx:GridViewCommandColumn Caption="Enable Simulation Tests" ShowSelectCheckbox="true"
																		SelectAllCheckboxMode="AllPages" VisibleIndex="0" Width="100px">
																		<ClearFilterButton Visible="True">
																		</ClearFilterButton>
																		<HeaderStyle CssClass="GridCssHeader1" />
																	</dx:GridViewCommandColumn>
																	<%-- <dx:GridViewDataColumn Caption="EnableSimulationTests" VisibleIndex="2"  Width="200px"
                                                            CellStyle-HorizontalAlign="Center" >
															<DataItemTemplate>                                                           
															<dx:ASPxCheckBox ID="checkToMonitor" runat="server" OnInit="checkToMonitor_Init" Value='<%# Eval("EnableSimulationTests") %>' />
															</DataItemTemplate>
															<HeaderStyle CssClass="GridCssHeader1" />
															<CellStyle HorizontalAlign="Center" CssClass="GridCss1">
															</CellStyle>
                                                      </dx:GridViewDataColumn>--%>
																	<%--<dx:GridViewDataColumn Caption="Enable" VisibleIndex="0" Width="70px">
            <DataItemTemplate>
                <dx:ASPxCheckBox ID="enablechbx" runat="server"  >
            
                </dx:ASPxCheckBox>
            </DataItemTemplate>
			<HeaderStyle CssClass="GridCssHeader" />
			</dx:GridViewDataColumn>--%>
																	<%--<dx:GridViewDataColumn Caption="Enable" VisibleIndex="0" CellStyle-HorizontalAlign="Center"
																	Width="50px">
																	<DataItemTemplate>
																		<dx:ASPxCheckBox ID="checklatency" runat="server" OnInit="checklatency_Init"
																			 KeyFieldName="ServerId" Value='<%# Eval("EnableLatencyTest") %>' />
																	</DataItemTemplate>
																	<HeaderStyle CssClass="GridCssHeader1" />
																	<CellStyle HorizontalAlign="Center" CssClass="GridCss1">
																	</CellStyle>
																</dx:GridViewDataColumn>--%>
																	<dx:GridViewDataTextColumn Caption="Response Threshold (ms)" FieldName="ResponseThreshold"
																		VisibleIndex="1" Width="200px">
																		<DataItemTemplate>
																			<dx:ASPxTextBox ID="txtResponseThresholdValue" runat="server" KeyFieldName="ResponseThreshold"
																				Width="100px" Value='<%# Eval("ResponseThreshold") %>'>
																			</dx:ASPxTextBox>
																		</DataItemTemplate>
																		<Settings AutoFilterCondition="Contains" />
																		<EditCellStyle CssClass="GridCss">
																		</EditCellStyle>
																		<EditFormCaptionStyle CssClass="GridCss" HorizontalAlign="Left">
																		</EditFormCaptionStyle>
																		<HeaderStyle CssClass="GridCssHeader" />
																		<CellStyle CssClass="GridCss">
																		</CellStyle>
																	</dx:GridViewDataTextColumn>
																	<%--<dx:GridViewDataColumn Caption="Subscribed Alerts" VisibleIndex="3"  Width="200px"
                                                            CellStyle-HorizontalAlign="Center" >
															<DataItemTemplate>                                                           
															<dx:ASPxCheckBox ID="checkSubScribedAlerts" runat="server" OnInit="checkSubScribedAlerts_Init" Value='<%# Eval("SubScribedAlerts") %>' />
															</DataItemTemplate>
															<HeaderStyle CssClass="GridCssHeader1" />
															<CellStyle HorizontalAlign="Center" CssClass="GridCss1">
															</CellStyle>
                                                        </dx:GridViewDataColumn>--%>
																	<%--<dx:GridViewDataCheckColumn Caption="Subscribed Alerts" FieldName="SubScribedAlerts" Width="200px"
                                                        ShowInCustomizationForm="True" VisibleIndex="4">
                                                        <Settings AllowAutoFilter="False" />
                                                        <EditFormSettings Caption="If a task did not load, send &amp;#39;Load&amp;#39; command to the server (i.e., try to start it)" />
                                                        <EditCellStyle CssClass="GridCss1">
                                                        </EditCellStyle>
                                                        <EditFormCaptionStyle CssClass="GridCss1">
                                                        </EditFormCaptionStyle>
                                                        <DataItemTemplate>
                                                            <dx:ASPxCheckBox ID="SubscribedAlerts" runat="server" Checked="false" >
                                                            </dx:ASPxCheckBox>
                                                        </DataItemTemplate>
                                                        <HeaderStyle CssClass="GridCssHeader1" />
                                                        <CellStyle CssClass="GridCss1">
                                                        </CellStyle>
                                                    </dx:GridViewDataCheckColumn>--%>
																	<dx:GridViewDataTextColumn Caption="Tests" FieldName="Tests" Width="200px" VisibleIndex="2">
																		<HeaderStyle CssClass="GridCssHeader" />
																		<CellStyle CssClass="GridCss">
																		</CellStyle>
																	</dx:GridViewDataTextColumn>
																	<dx:GridViewDataTextColumn Caption="Type" FieldName="Type" Width="200px" VisibleIndex="3"
																		ShowInCustomizationForm="True">
																		<Settings AllowAutoFilter="False" AllowDragDrop="True" AutoFilterCondition="Contains" />
																		<EditCellStyle CssClass="GridCss">
																		</EditCellStyle>
																		<EditFormCaptionStyle CssClass="GridCss">
																		</EditFormCaptionStyle>
																		<HeaderStyle CssClass="GridCssHeader" />
																		<CellStyle CssClass="GridCss">
																		</CellStyle>
																	</dx:GridViewDataTextColumn>
																</Columns>
																<SettingsPager PageSize="20" AlwaysShowPager="True">
									                             <PageSizeItemSettings  Visible="True">
									                               </PageSizeItemSettings>
								                                  </SettingsPager>
																<SettingsBehavior AllowSort="false" />
																<Styles CssPostfix="Office2010Silver" CssFilePath="~/App_Themes/Office2010Silver/{0}/styles.css">
																	<AlternatingRow CssClass="GridCssAltRow">
																	</AlternatingRow>
																</Styles>
																
															</dx:ASPxGridView>
														</td>
													</tr>
												</table>
											</td>
										</tr>

										<tr>
														<td>
															<dx:ASPxRoundPanel ID="ASPxRoundPanel3" runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
															CssPostfix="Glass" GroupBoxCaptionOffsetY="-24px" HeaderText="Performance Tests"
															Height="50px" SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" Width="800px">
															<ContentPaddings Padding="2px" PaddingBottom="10px" PaddingLeft="4px" PaddingTop="10px" />
															<ContentPaddings Padding="2px" PaddingLeft="4px" PaddingTop="10px" PaddingBottom="10px">
															</ContentPaddings>
															<HeaderStyle Height="23px">
															</HeaderStyle>
															<PanelCollection>
																<dx:PanelContent ID="PanelContent4" runat="server" SupportsDisabledAttribute="True">
																	<asp:Label ID="Label1" runat="server" Style="color: #0033CC" Text="Check any items you would like to be tested upon scanning this server."></asp:Label>
																	<table>
																			<tr>
																				<td valign="top">
																					<table>
																						<tr>
																							<td valign="top">
																								<dx:ASPxLabel runat="server" Text="Create Activity Threshold (ms):"  ID="ASPxLabel23" CssClass="lblsmallFont"
																									Wrap="False">
																								</dx:ASPxLabel>
																							</td>
																							<td valign="top">
																								<dx:ASPxTextBox runat="server"  Width="50px" CssClass="txtsmall" ID="SrvAtrCreateActivityThTextBox"
																									Text="200">
																									<MaskSettings Mask="&lt;0..99999&gt;"></MaskSettings>
																									<MaskSettings Mask="&lt;0..99999&gt;" />
																									<ValidationSettings CausesValidation="True" ErrorDisplayMode="ImageWithTooltip" SetFocusOnError="True">
																										<RegularExpression ErrorText="Please enter a numeric value using the numbers only (0-9)."
																											ValidationExpression="^\d+$" />
																										<RegularExpression ErrorText="Please enter a numeric value using the numbers only (0-9)."
																											ValidationExpression="^\d+$"></RegularExpression>
																									</ValidationSettings>
																								</dx:ASPxTextBox>
																							</td>
																						</tr>
																						<tr>
																							<td valign="top">
																								<dx:ASPxLabel ID="ASPxLabel25" runat="server" CssClass="lblsmallFont"  Text="Create Blog Threshold (ms):"
																									Wrap="False">
																								</dx:ASPxLabel>
																							</td>
																							<td valign="top">
																								<dx:ASPxTextBox ID="SrvAtrCreateBlogThTextBox" runat="server"  Width="50px" CssClass="txtsmall" Text="500">
																									<MaskSettings Mask="&lt;0..99999&gt;"></MaskSettings>
																									<MaskSettings Mask="&lt;0..99999&gt;" />
																									<ValidationSettings CausesValidation="True" ErrorDisplayMode="ImageWithTooltip">
																										<RegularExpression ErrorText="Please enter a numeric value using the numbers only (0-9)."
																											ValidationExpression="^\d+$" />
																										<RegularExpression ErrorText="Please enter a numeric value using the numbers only (0-9)."
																											ValidationExpression="^\d+$"></RegularExpression>
																									</ValidationSettings>
																								</dx:ASPxTextBox>
																							</td>
																						</tr>
																						<tr>
																							<td valign="top">
																								<dx:ASPxLabel ID="ASPxLabel26" runat="server" CssClass="lblsmallFont"  Text="Create Bookmark Threshold (ms):"
																									Wrap="False">
																								</dx:ASPxLabel>
																							</td>
																							<td valign="top">
																								<dx:ASPxTextBox ID="SrvAtrCreateBookmarkThTextBox" runat="server"  Width="50px" CssClass="txtsmall" Text="200">
																									<MaskSettings Mask="&lt;0..99999&gt;"></MaskSettings>
																									<MaskSettings Mask="&lt;0..99999&gt;" />
																									<ValidationSettings CausesValidation="True" ErrorDisplayMode="ImageWithTooltip" SetFocusOnError="True">
																										<RegularExpression ErrorText="Please enter a numeric value using the numbers only (0-9)."
																											ValidationExpression="^\d+$" />
																										<RegularExpression ErrorText="Please enter a numeric value using the numbers only (0-9)."
																											ValidationExpression="^\d+$"></RegularExpression>
																									</ValidationSettings>
																								</dx:ASPxTextBox>
																							</td>
																						</tr>
																						<tr>
																							<td valign="top">
																								<dx:ASPxLabel ID="ASPxLabel15" runat="server" CssClass="lblsmallFont"  Text="Create Community Threshold (ms):"
																									Wrap="False">
																								</dx:ASPxLabel>
																							</td>
																							<td valign="top">
																								<dx:ASPxTextBox ID="SrvAtrCreateCommunityThTextBox" runat="server"  Width="50px" CssClass="txtsmall"  Text="200">
																									<MaskSettings Mask="&lt;0..99999&gt;"></MaskSettings>
																									<MaskSettings Mask="&lt;0..99999&gt;" />
																									<ValidationSettings CausesValidation="True" ErrorDisplayMode="ImageWithTooltip" SetFocusOnError="True">
																										<RegularExpression ErrorText="Please enter a numeric value using the numbers only (0-9)."
																											ValidationExpression="^\d+$" />
																										<RegularExpression ErrorText="Please enter a numeric value using the numbers only (0-9)."
																											ValidationExpression="^\d+$"></RegularExpression>
																									</ValidationSettings>
																								</dx:ASPxTextBox>
																							</td>
																						</tr>
																						<tr>
																							<td valign="top">
																								<dx:ASPxLabel ID="ASPxLabel2" runat="server"  CssClass="lblsmallFont"  Text="Create File Threshold (ms):"
																									Wrap="False">
																								</dx:ASPxLabel>
																							</td>
																							<td valign="top">
																								<dx:ASPxTextBox ID="SrvAtrCreateFileThTextBox" runat="server"  Width="50px" CssClass="txtsmall"  Text="200">
																									<MaskSettings Mask="&lt;0..99999&gt;"></MaskSettings>
																									<MaskSettings Mask="&lt;0..99999&gt;" />
																									<ValidationSettings CausesValidation="True" ErrorDisplayMode="ImageWithTooltip" SetFocusOnError="True">
																										<RegularExpression ErrorText="Please enter a numeric value using the numbers only (0-9)."
																											ValidationExpression="^\d+$" />
																										<RegularExpression ErrorText="Please enter a numeric value using the numbers only (0-9)."
																											ValidationExpression="^\d+$"></RegularExpression>
																									</ValidationSettings>
																								</dx:ASPxTextBox>
																							</td>
																						</tr>
																						<tr>
																							<td valign="top">
																								<dx:ASPxLabel ID="ASPxLabel5" runat="server" CssClass="lblsmallFont" Text="Create Wiki Threshold (ms):"
																									Wrap="False">
																								</dx:ASPxLabel>
																							</td>
																							<td valign="top">
																								<dx:ASPxTextBox ID="SrvAtrCreateWikiThTextBox" runat="server"  Width="50px" CssClass="txtsmall"  Text="200">
																									<MaskSettings Mask="&lt;0..99999&gt;"></MaskSettings>
																									<MaskSettings Mask="&lt;0..99999&gt;" />
																									<ValidationSettings CausesValidation="True" ErrorDisplayMode="ImageWithTooltip" SetFocusOnError="True">
																										<RegularExpression ErrorText="Please enter a numeric value using the numbers only (0-9)."
																											ValidationExpression="^\d+$" />
																										<RegularExpression ErrorText="Please enter a numeric value using the numbers only (0-9)."
																											ValidationExpression="^\d+$"></RegularExpression>
																									</ValidationSettings>
																								</dx:ASPxTextBox>
																							</td>
																						</tr>
																						<tr>
																							<td valign="top">
																								<dx:ASPxLabel ID="ASPxLabel7" runat="server" CssClass="lblsmallFont"  Text="Search Profile Threshold (ms):"
																									Wrap="False">
																								</dx:ASPxLabel>
																							</td>
																							<td valign="top">
																								<dx:ASPxTextBox ID="SrvAtrSearchProfileThTextBox" runat="server"  Width="50px" CssClass="txtsmall"  Text="200">
																									<MaskSettings Mask="&lt;0..99999&gt;"></MaskSettings>
																									<MaskSettings Mask="&lt;0..99999&gt;" />
																									<ValidationSettings CausesValidation="True" ErrorDisplayMode="ImageWithTooltip" SetFocusOnError="True">
																										<RegularExpression ErrorText="Please enter a numeric value using the numbers only (0-9)."
																											ValidationExpression="^\d+$" />
																										<RegularExpression ErrorText="Please enter a numeric value using the numbers only (0-9)."
																											ValidationExpression="^\d+$"></RegularExpression>
																									</ValidationSettings>
																								</dx:ASPxTextBox>
																							</td>
																						</tr>
																					</table>
																				</td>
																				<td valign="top">
																					<table>
																						<tr>
																							<td colspan="2" valign="top">
																								<dx:ASPxCheckBox ID="chbxCreateActivity" runat="server" CheckState="Unchecked"
																									Text="Create Activity"

																									CssClass="lblsmallFont">
																								</dx:ASPxCheckBox>
																							</td>
																						</tr>
																						<tr>
																							<td valign="top"  colspan="3">
																								<dx:ASPxCheckBox ID="chbxCreateBlog" runat="server" CheckState="Unchecked"
																									CssClass="lblsmallFont" Text="Create Blog" Wrap="False">
																								</dx:ASPxCheckBox>
																							</td>
																						</tr>
																						<tr>
																							<td colspan="3" valign="top">
																								<dx:ASPxCheckBox ID="chbxCreateBookmark" runat="server" CheckState="Unchecked"
																									CssClass="lblsmallFont" Text="Create Bookmark" Wrap="False">
																								</dx:ASPxCheckBox>
																							</td>
																						</tr>
																						<tr>
																							<td colspan="3" valign="top">
																								<dx:ASPxCheckBox ID="chbxCreateCommunity" runat="server" CheckState="Unchecked"
																									CssClass="lblsmallFont" Text="Create Community" Wrap="False">
																								</dx:ASPxCheckBox>
																							</td>
																						</tr>
																						<tr>
																							<td colspan="3" valign="top">
																								<dx:ASPxCheckBox ID="chbxCreateFile" runat="server" CheckState="Unchecked"
																									CssClass="lblsmallFont" Text="Create File" Wrap="False">
																								</dx:ASPxCheckBox>
																							</td>
																						</tr>
																						<tr>
																							<td colspan="3" valign="top">
																								<dx:ASPxCheckBox ID="chbxCreateWiki" runat="server" CheckState="Unchecked"
																									CssClass="lblsmallFont" Text="Create Wiki" Wrap="False">
																								</dx:ASPxCheckBox>
																							</td>
																						</tr>
																						<tr>
																							<td colspan="3" valign="top">
																								<dx:ASPxCheckBox ID="chbxSearchProfile" runat="server" CheckState="Unchecked"
																									CssClass="lblsmallFont" Text="Search Profile" Wrap="False">
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
									</table>
								</dx:ContentControl>
							</ContentCollection>
						</dx:TabPage>

						<dx:TabPage Text="WebSphere Settings"  Visible="False" >
							<TabImage Url="~/images/icons/information.png"  />
							<ContentCollection>
								<dx:ContentControl ID="ContentControl6" runat="server" SupportsDisabledAttribute="True" >
							<dx:PanelContent ID="PanelContentforwebsettings" runat="server" SupportsDisabledAttribute="True" >
									<table>

									<tr>
											<td>
												<dx:ASPxRoundPanel ID="ASPxRoundPanel7" runat="server" Theme="Glass"
													Width="100%" HeaderText="WebSphere Cell"  >
													<PanelCollection>
														<dx:PanelContent ID="PanelContent25" runat="server" SupportsDisabledAttribute="True" >
															<table>
																<tr>
																	<td>
																		<dx:ASPxLabel ID="lblcellname" runat="server" Text="Name:" CssClass="lblsmallFont">
                                                                        </dx:ASPxLabel>
																	</td>
																	<td>
																		

																		<dx:ASPxCallbackPanel ID="ASPxCallbackPanel1" runat="server" ClientInstanceName="callbackpanel"
											Width="200px">
											<PanelCollection>
												<dx:PanelContent ID="PanelContent2" runat="server" SupportsDisabledAttribute="True">
										<dx:ASPxTextBox ID="CellnameTextBox" runat="server" Width="220px"  AutoPostBack="true" 
														ClientInstanceName="CellnameTextBox" OnValidation="CellnameTextBox_Validation">
										<ClientSideEvents LostFocus="function(s, e) {s.Validate();}" Validation="function(s, e) {callbackpanel.PerformCallback();}" />
											<ValidationSettings ErrorDisplayMode="Text">
												<RequiredField ErrorText="Please enter cell name" IsRequired="False" />
											</ValidationSettings>
										</dx:ASPxTextBox>
									</dx:PanelContent>
											</PanelCollection>
										</dx:ASPxCallbackPanel>
																	</td>
																</tr>
																<tr>
																	<td>
																		<dx:ASPxLabel ID="ASPxLabel3" runat="server" Text="Host Name:" CssClass="lblsmallFont">
																		</dx:ASPxLabel>
																	</td>
																	<td>
																		<dx:ASPxCallbackPanel ID="ASPxCallbackPanel2" runat="server" ClientInstanceName="callbackpanel"
											Width="200px">
											<PanelCollection>
												<dx:PanelContent ID="PanelContent8" runat="server" SupportsDisabledAttribute="True">
													<dx:ASPxTextBox ID="HostName" runat="server" Width="220px" 
														 AutoPostBack="true" OnValidation="HostName_Validation">
														<ClientSideEvents LostFocus="function(s, e) {s.Validate();}" Validation="function(s, e) {callbackpanel.PerformCallback();}" />
														<ValidationSettings ErrorDisplayMode="Text">
															<RequiredField ErrorText="Please enter host name" IsRequired="False" />
															<%--<RegularExpression ValidationExpression="^((25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)(\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)){3})$|^((([a-zA-Z0-9]|[a-zA-Z0-9][a-zA-Z0-9\-]*[a-zA-Z0-9])\.)*([A-Za-z0-9]|[A-Za-z0-9][A-Za-z0-9\-]*[A-Za-z0-9]))$" ErrorText="Enter valid IP or Host Nmae" />--%>
														
														<%--<RegularExpression ValidationExpression="([a-zA-Z0-9]|[a-zA-Z0-9][a-zA-Z0-9\-]*[a-zA-Z0-9])\.)*" />--%>
														</ValidationSettings>
													</dx:ASPxTextBox>
												</dx:PanelContent>
											</PanelCollection>
										</dx:ASPxCallbackPanel>

																	</td>
																</tr>
																<tr>
																	<td>
																		<dx:ASPxLabel ID="connlbl" runat="server" Text="Conn.Type:" CssClass="lblsmallFont">
																		</dx:ASPxLabel>
																	</td>
																	<td>
																		<dx:ASPxComboBox ID="ConnectionComboBox" runat="server" Width="220px" OnSelectedIndexChanged="ConnectionComboBox_SelectedIndexChanged"
																			AutoPostBack="true">
																			<Items>
																				<dx:ListEditItem Text="SOAP" Value="0" />
																				<dx:ListEditItem Text="RMI" Value="1" />
																			</Items>
																			<ValidationSettings ErrorDisplayMode="Text" SetFocusOnError="True">
																				<RequiredField ErrorText="Please select connection type" IsRequired="False" />
																				
																			</ValidationSettings>
																		</dx:ASPxComboBox>
																	</td>
																</tr>
																<tr>
																	<td>
																		<dx:ASPxLabel ID="ASPxLabel4" runat="server" Text="Port No:" CssClass="lblsmallFont">
																		</dx:ASPxLabel>
																	</td>
																	<td>
																		<dx:ASPxTextBox ID="txtport" runat="server" Width="220px">
																			<ValidationSettings ErrorDisplayMode="Text" SetFocusOnError="True">
																				<RequiredField ErrorText="Please enter port no" IsRequired="false" />
																			</ValidationSettings>
																		</dx:ASPxTextBox>
																	</td>
																</tr>
																<tr>
																	<td>
																		<dx:ASPxCheckBox ID="chbx" runat="server" Text="Global Security" TextAlign="left"
																			AutoPostBack="true" OnCheckedChanged="chbx_CheckedChanged">
																		</dx:ASPxCheckBox>
																	</td>
																</tr>
																<tr>
																	<td>
																		<dx:ASPxLabel ID="CredentialsLabel" runat="server" CssClass="lblsmallFont" Text="Credentials:"
																			Visible="false">
																		</dx:ASPxLabel>
																	</td>
																	<td>
																		<dx:ASPxComboBox ID="WebCredentialsComboBox" runat="server" AutoPostBack="false" Visible="false">
																		
																		</dx:ASPxComboBox>
																	</td>
																	<td>
																		<dx:ASPxButton ID="Credentialsbtn" runat="server" Text="Add Credentials" CssClass="sysButton"
																			OnClick="btn_clickcopyprofile" CausesValidation="true" Visible="false">
																		</dx:ASPxButton>
																	</td>
																</tr>
																<tr>
																	<td>
																		<dx:ASPxLabel ID="reallbl" runat="server" Text="Realm:" Visible="false" CssClass="lblsmallFont">
																		</dx:ASPxLabel>
																	</td>
																	<td>
																		<dx:ASPxTextBox ID="realmtxtbx" runat="server" Width="220px" Visible="false">
																		</dx:ASPxTextBox>
																	</td>
																</tr>
																
																<tr>
																<td>
																<dx:ASPxButton ID="RefreshButton" runat="server" CssClass="sysButton"
																									Text="Refresh" OnClick="Refresh_Click" Width="60px" ClientInstanceName="goButton" CausesValidation="false">
																								</dx:ASPxButton>
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
													<dx:ASPxRoundPanel ID="ASPxRoundPanel6" runat="server" Theme="Glass"
													Width="100%" HeaderText="List of Nodes/Servers">
													<PanelCollection>
														<dx:PanelContent ID="PanelContentfornodes" runat="server" SupportsDisabledAttribute="True">
											<table>
										<tr>
											<td>
												<dx:ASPxButton ID="CollapseAllButton" ClientInstanceName="collapseAll" runat="server"
													Text="Collapse All" CssClass="sysButton" Wrap="False" EnableClientSideAPI="False"
													OnClick="CollapseAllButton_Click">
													<Image Url="~/images/icons/forbidden.png">
													</Image>
												</dx:ASPxButton>
											</td>
										</tr>
										<tr>
											<td>
												<dx:ASPxTreeList ID="SametimeNodesTreeList" ClientInstanceName="eventsTree" runat="server"
													AutoGenerateColumns="False" CssClass="lblsmallFont" KeyFieldName="Id" ParentFieldName="SrvId"
													Theme="Office2003Blue" >
													<Columns>
														<dx:TreeListTextColumn Caption="NodeName  " FieldName="Name" Name="NodeName" VisibleIndex="0"
															ReadOnly="True">
															<EditFormSettings Visible="True" />
															<HeaderStyle CssClass="lblsmallFont" />
														</dx:TreeListTextColumn>
														<dx:TreeListTextColumn FieldName="actid" Name="actid" Visible="False" VisibleIndex="1">
														</dx:TreeListTextColumn>
														<dx:TreeListTextColumn FieldName="tbl" Name="tbl" Visible="False" VisibleIndex="2">
														</dx:TreeListTextColumn>
														<dx:TreeListTextColumn FieldName="SrvId" Name="SrvId" Visible="False" VisibleIndex="3">
														</dx:TreeListTextColumn>
														<dx:TreeListTextColumn FieldName="Status" Name="Status"  VisibleIndex="4">
													     </dx:TreeListTextColumn>
														<dx:TreeListTextColumn FieldName="HostName" Name="HostName"  VisibleIndex="5">
														</dx:TreeListTextColumn>
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
										
		                                          </dx:PanelContent>
													</PanelCollection>
												</dx:ASPxRoundPanel>


												</td>
											</tr>
											<tr>
												<td>
													<div id="errorDivForImportingWS" runat="server" class="alert alert-danger" style="display: none">
													</div>
												</td>
											</tr>
										</table>
										<table>
							
								
							</table>
											</dx:PanelContent>
										</dx:ContentControl>
							</ContentCollection>
						</dx:TabPage>
					
						<dx:TabPage Text="Advanced Settings" Visible="True">
							<TabImage Url="~/images/icons/information.png" />
							<ContentCollection>
								<dx:ContentControl ID="ContentControl5" runat="server" SupportsDisabledAttribute="True">
									<table class="navbarTbl">
									
										
										<tr>
											<td>
												<dx:ASPxRoundPanel ID="ASPxRoundPanel4" runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
													CssPostfix="Glass" GroupBoxCaptionOffsetY="-24px" HeaderText="Proxy Server Settings"
													SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" Width="100%" Visible="False">
													<ContentPaddings PaddingBottom="10px" PaddingLeft="4px" PaddingTop="10px" />
													<HeaderStyle Height="23px">
													</HeaderStyle>
													<PanelCollection>
														<dx:PanelContent ID="PanelContent7" runat="server" SupportsDisabledAttribute="True">
															<table>
																<tr>
																	<td>
																		<asp:Label ID="Label2" runat="server" Text="Type:" CssClass="lblsmallFont"></asp:Label>
																	</td>
																	<td>
																		<dx:ASPxComboBox ID="ProxyTypeComboBox" runat="server" ClientInstanceName="cmbServerType"
																			ValueType="System.String" AutoPostBack="True">
																			<Items>
																				<dx:ListEditItem Text="No Proxy" Value="NoProxy" />
																				<dx:ListEditItem Text="Proxy" Value="Proxy" />
																			</Items>
																		</dx:ASPxComboBox>
																	</td>
																</tr>
																<tr>
																	<td>
																		<asp:Label ID="Label3" runat="server" Text="Protocol:" CssClass="lblsmallFont"></asp:Label>
																	</td>
																	<td>
																		<dx:ASPxComboBox ID="ProtocolComboBox" runat="server" ClientInstanceName="cmbServerType"
																			ValueType="System.String" AutoPostBack="True">
																			<Items>
																				<dx:ListEditItem Text="Direct Connection Using IBM Connection Protocol" Value="DirectConnectionUsingIBMConnection" />
																				<dx:ListEditItem Text="Direct Connection" Value="DirectConnection" />
																			</Items>
																		</dx:ASPxComboBox>
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
												<dx:ASPxRoundPanel ID="DB2RoundPane" runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
													CssPostfix="Glass" GroupBoxCaptionOffsetY="-24px" HeaderText="DB2 Settings" SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css"
													Width="100%">
													<ContentPaddings PaddingBottom="10px" PaddingLeft="4px" PaddingTop="10px" />
													<HeaderStyle Height="23px">
													</HeaderStyle>
													<PanelCollection>
														<dx:PanelContent ID="PanelContentDB2" runat="server" SupportsDisabledAttribute="True">
															<table>
																<tr>
																	<td>
																		<dx:ASPxLabel ID="HostNameLabel" runat="server" CssClass="lblsmallFont" Text="Host Name:">
																		</dx:ASPxLabel>
																	</td>
																	<td>
																		<dx:ASPxTextBox ID="HostNameTextBox" runat="server" AutoPostBack="True" CssClass="txtsmall"
																			Enabled="True" Width="170px">
																			
                                                                              <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ValidationGroup="Creds">
																				
																				<RequiredField IsRequired="True" ErrorText="Please Enter Host Name."></RequiredField>
																			</ValidationSettings>
																		</dx:ASPxTextBox>
																	</td>
																	<td>&nbsp</td>
																	<td>&nbsp</td>
																	<td>
																		<dx:ASPxLabel ID="ASPxLabel1" runat="server" CssClass="lblsmallFont" Text="Credentials:">
																		</dx:ASPxLabel>
																	</td>
																	<td>
																				
																				<dx:ASPxComboBox ID= "DBCredentialsComboBox" runat="server" AutoPostBack = "false" >
                                                                                <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ValidationGroup="Creds">
<RequiredField IsRequired="True" ErrorText="Please Select Credentials.">
</RequiredField>
	</ValidationSettings>		
                                                                                </dx:ASPxComboBox>
																			</td>
                                                                              <td>
                                                                	<dx:ASPxButton ID="ASPxButton2" runat="server" Text="Add Credentials" CssClass="sysButton"
																						OnClick="btn_clickcopyprofile" CausesValidation="false" Visible="true" UseSubmitBehavior="false">
																					</dx:ASPxButton>
                                                                </td>
																	<td>
																		<dx:ASPxLabel ID="DataBaseNameLabel" Visible="false" runat="server" CssClass="lblsmallFont" Text="Database Name:">
																		</dx:ASPxLabel>
																	</td>
																	<td>
																		<dx:ASPxTextBox ID="DataBaseNameTextBox"  Visible="false" runat="server" AutoPostBack="True" CssClass="txtsmall"
																			Width="170px">
																		</dx:ASPxTextBox>
																	</td>
																</tr>
																<tr>
																	<td>
																		<dx:ASPxCheckBox ID="PortCheckBox" runat="server" CheckState="Unchecked" CssClass="lblsmallFont"
																			Text="Port:" Visible="false" >
																		</dx:ASPxCheckBox>
																		<dx:ASPxLabel ID="port" runat="server" CssClass="lblsmallFont" Text="Port:">
																		</dx:ASPxLabel>
																	</td>
																	<td>
																		<dx:ASPxTextBox ID="DB2PortTextBox" runat="server" Width="40px" AutoPostBack="True" CssClass="txtsmall"
																			>
                                                                            <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ValidationGroup="Creds">
<RequiredField IsRequired="True" ErrorText="Please Enter Port Number.">
</RequiredField>
	</ValidationSettings>	
																			<MaskSettings Mask="&lt;0..99999&gt;" />
																			<MaskSettings Mask="&lt;0..99999&gt;"></MaskSettings>
																		</dx:ASPxTextBox>
																	</td>
																	
																</tr>
																<tr>
																	<td colspan="2">
																		<dx:ASPxButton ID="TestConnectionButton2" runat="server" CssClass="sysButton"
																			Text="Test Connection" Visible="false" >
																		</dx:ASPxButton>
																	</td>
																	<td align="right">
																		&nbsp;
																	</td>
																	<td align="right">
																		&nbsp;
																	</td>
																	<td align="right">
																		&nbsp;
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
						<dx:TabPage Text="Maintenance Windows" Visible="True">
							<TabImage Url="~/images/application_view_tile.png" />
							<ContentCollection>
								<dx:ContentControl ID="ContentControl3" runat="server" SupportsDisabledAttribute="True">
									<table width="100%">
										<tr>
											<td>
												<div id="maintDiv" class="info">
													Maintenance Windows are times when you do not want the server monitored. You can
													define maintenance windows using the Hours & Maintenance\Maintenance menu option.
													Use the Actions column to modify maintenance windows information.</div>
											</td>
										</tr>
										<tr>
											<td>
												<dx:ASPxButton ID="ToggleVeiwButton" runat="server" CssFilePath="~/App_Themes/Office2010Blue/{0}/styles.css"
													CssPostfix="Office2010Blue" SpriteCssFilePath="~/App_Themes/Office2010Blue/{0}/sprite.css"
													Text="Switch to Calendar view" Visible="False" Width="178px">
												</dx:ASPxButton>
												<dx:ASPxGridView ID="MaintWinListGridView" runat="server" AutoGenerateColumns="False"
													CssFilePath="~/App_Themes/Office2010Blue/{0}/styles.css" Width="100%" CssPostfix="Office2010Blue"
													KeyFieldName="ID" OnHtmlRowCreated="MaintWinListGridView_HtmlRowCreated" OnPageSizeChanged="MaintWinListGridView_PageSizeChanged"
													OnSelectionChanged="MaintWinListGridView_SelectionChanged" EnableTheming="True"
													Theme="Office2003Blue" Cursor="pointer">
													<Columns>
														<dx:GridViewCommandColumn ShowInCustomizationForm="True" Visible="False" VisibleIndex="0">
															<ClearFilterButton Visible="True">
															</ClearFilterButton>
														</dx:GridViewCommandColumn>
														<dx:GridViewDataTextColumn FieldName="ID" ReadOnly="True" ShowInCustomizationForm="True"
															Visible="False" VisibleIndex="1">
															<EditFormSettings Visible="False" />
															<EditFormSettings Visible="False"></EditFormSettings>
														</dx:GridViewDataTextColumn>
														<dx:GridViewDataTextColumn FieldName="ServerID" ReadOnly="True" ShowInCustomizationForm="True"
															Visible="False" VisibleIndex="2">
															<EditFormSettings Visible="False" />
															<EditFormSettings Visible="False"></EditFormSettings>
														</dx:GridViewDataTextColumn>
														<dx:GridViewDataTextColumn Caption="Name" FieldName="Name" ShowInCustomizationForm="True"
															VisibleIndex="3">
															<Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
															<Settings AutoFilterCondition="Contains"></Settings>
															<EditCellStyle CssClass="GridCss">
															</EditCellStyle>
															<EditFormCaptionStyle CssClass="GridCss" HorizontalAlign="Left">
															</EditFormCaptionStyle>
															<HeaderStyle CssClass="GridCssHeader">
																<Paddings Padding="5px" />
																<Paddings Padding="5px"></Paddings>
															</HeaderStyle>
															<CellStyle CssClass="GridCss">
															</CellStyle>
														</dx:GridViewDataTextColumn>
														<dx:GridViewDataDateColumn FieldName="StartDate" ShowInCustomizationForm="True" VisibleIndex="4">
															<EditCellStyle CssClass="GridCss">
															</EditCellStyle>
															<EditFormCaptionStyle CssClass="GridCss">
															</EditFormCaptionStyle>
															<HeaderStyle CssClass="GridCssHeader" />
															<CellStyle CssClass="GridCss">
															</CellStyle>
														</dx:GridViewDataDateColumn>
														<dx:GridViewDataDateColumn FieldName="StartTime" ShowInCustomizationForm="True" VisibleIndex="5">
															<EditCellStyle CssClass="GridCss">
															</EditCellStyle>
															<EditFormCaptionStyle CssClass="GridCss">
															</EditFormCaptionStyle>
															<HeaderStyle CssClass="GridCssHeader" />
															<CellStyle CssClass="GridCss">
															</CellStyle>
															<PropertiesDateEdit DisplayFormatString="t">
															</PropertiesDateEdit>
														</dx:GridViewDataDateColumn>
														<dx:GridViewDataTextColumn FieldName="Duration" ShowInCustomizationForm="True" VisibleIndex="6">
															<EditCellStyle CssClass="GridCss">
															</EditCellStyle>
															<EditFormCaptionStyle CssClass="GridCss">
															</EditFormCaptionStyle>
															<HeaderStyle CssClass="GridCssHeader" />
															<CellStyle CssClass="GridCss">
															</CellStyle><Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
														</dx:GridViewDataTextColumn>
														<dx:GridViewDataDateColumn FieldName="EndDate" ShowInCustomizationForm="True" VisibleIndex="7">
															<EditCellStyle CssClass="GridCss">
															</EditCellStyle>
															<EditFormCaptionStyle CssClass="GridCss">
															</EditFormCaptionStyle>
															<HeaderStyle CssClass="GridCssHeader" />
															<CellStyle CssClass="GridCss">
															</CellStyle>
														</dx:GridViewDataDateColumn>
														<dx:GridViewDataTextColumn FieldName="MaintType" ReadOnly="True" ShowInCustomizationForm="True"
															VisibleIndex="8">
															<EditCellStyle CssClass="GridCss">
															</EditCellStyle>
															<EditFormCaptionStyle CssClass="GridCss">
															</EditFormCaptionStyle>
															<HeaderStyle CssClass="GridCssHeader" />
															<CellStyle CssClass="GridCss">
															</CellStyle><Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
														</dx:GridViewDataTextColumn>
													</Columns>
													<SettingsBehavior AllowSelectByRowClick="True" ColumnResizeMode="NextColumn" ProcessSelectionChangedOnServer="True" />
													<SettingsBehavior AllowSelectByRowClick="True" ColumnResizeMode="NextColumn" ProcessSelectionChangedOnServer="True">
													</SettingsBehavior>
													<SettingsPager PageSize="50">
														<PageSizeItemSettings Visible="True">
														</PageSizeItemSettings>
													</SettingsPager>
													<Settings ShowFilterRow="True" ShowGroupPanel="True" />
													<Settings ShowFilterRow="True" ShowGroupPanel="True"></Settings>
													<Images SpriteCssFilePath="~/App_Themes/Office2010Blue/{0}/sprite.css">
														<LoadingPanelOnStatusBar Url="~/App_Themes/Office2010Blue/GridView/Loading.gif">
														</LoadingPanelOnStatusBar>
														<LoadingPanel Url="~/App_Themes/Office2010Blue/GridView/Loading.gif">
														</LoadingPanel>
													</Images>
													<ImagesFilterControl>
														<LoadingPanel Url="~/App_Themes/Office2010Blue/GridView/Loading.gif">
														</LoadingPanel>
													</ImagesFilterControl>
													<Styles CssFilePath="~/App_Themes/Office2010Blue/{0}/styles.css" CssPostfix="Office2010Blue">
														<Header ImageSpacing="5px" SortingImageSpacing="5px">
														</Header>
														<GroupRow Font-Bold="True">
														</GroupRow>
														<AlternatingRow CssClass="GridAltRow">
														</AlternatingRow>
														<LoadingPanel ImageSpacing="5px">
														</LoadingPanel>
													</Styles>
													<StylesPager>
														<PageNumber ForeColor="#3E4846">
														</PageNumber>
														<Summary ForeColor="#1E395B">
														</Summary>
													</StylesPager>
													<StylesEditors ButtonEditCellSpacing="0">
														<ProgressBar Height="21px">
														</ProgressBar>
													</StylesEditors>
												</dx:ASPxGridView>
											</td>
										</tr>
										<tr>
											<td>
												&nbsp;
											</td>
										</tr>
									</table>
									
								</dx:ContentControl>
							</ContentCollection>
						</dx:TabPage>
						<dx:TabPage Text="Alert" Visible="true" >
							<TabImage Url="../images/icons/error.png">
							</TabImage>
							<ContentCollection>
								<dx:ContentControl ID="ContentControl4" runat="server" SupportsDisabledAttribute="True">
									<table class="style3">
										<tr>
											<td>
												<table class="style2">
													<tr>
														<td>
															<div id="alertDiv" class="info">
																The list of available alerts is listed below. In order to add new alerts or configure
																existing alerts, please use the Alerts\Alert Definitions menu.</div>
														</td>
													</tr>
													<tr>
														<td>
															<dx:ASPxGridView ID="AlertGridView" runat="server" AutoGenerateColumns="False" CssFilePath="~/App_Themes/Office2010Blue/{0}/styles.css"
																CssPostfix="Office2010Blue" Cursor="pointer" KeyFieldName="ID" Theme="Office2003Blue"
																Width="100%" EnableTheming="True" OnPageSizeChanged="AlertGridView_PageSizeChanged">
																<Columns>
																	<dx:GridViewDataTextColumn Caption="Name" FieldName="Name" ReadOnly="True" ShowInCustomizationForm="True"
																		VisibleIndex="0">
																		<HeaderStyle CssClass="GridCssHeader" />
																		<CellStyle CssClass="GridCss">
																		</CellStyle><Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
																	</dx:GridViewDataTextColumn>
																	<dx:GridViewDataTextColumn Caption="Start Time" FieldName="StartTime" ReadOnly="True"
																		ShowInCustomizationForm="True" VisibleIndex="1">
																		<HeaderStyle CssClass="GridCssHeader" />
																		<CellStyle CssClass="GridCss">
																		</CellStyle>
																	</dx:GridViewDataTextColumn>
																	<dx:GridViewDataTextColumn Caption="Duration" FieldName="Duration" ShowInCustomizationForm="True"
																		VisibleIndex="2" Width="70px">
																		<Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
																		<EditCellStyle CssClass="GridCss">
																		</EditCellStyle>
																		<EditFormCaptionStyle CssClass="GridCss" HorizontalAlign="Left">
																		</EditFormCaptionStyle>
																		<HeaderStyle CssClass="GridCssHeader">
																			<Paddings Padding="5px" />
																		</HeaderStyle>
																		<CellStyle CssClass="GridCss">
																		</CellStyle>
																	</dx:GridViewDataTextColumn>
																	<dx:GridViewDataTextColumn Caption="To" FieldName="SendTo" ShowInCustomizationForm="True"
																		VisibleIndex="3">
																		<HeaderStyle CssClass="GridCssHeader" />
																		<CellStyle CssClass="GridCss">
																		</CellStyle><Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
																	</dx:GridViewDataTextColumn>
																	<dx:GridViewDataTextColumn Caption="Cc" FieldName="CopyTo" ShowInCustomizationForm="True"
																		VisibleIndex="4">
																		<PropertiesTextEdit DisplayFormatString="t">
																		</PropertiesTextEdit>
																		<EditCellStyle CssClass="GridCss">
																		</EditCellStyle>
																		<EditFormCaptionStyle CssClass="GridCss">
																		</EditFormCaptionStyle>
																		<HeaderStyle CssClass="GridCssHeader" />
																		<CellStyle CssClass="GridCss">
																		</CellStyle><Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
																	</dx:GridViewDataTextColumn>
																	<dx:GridViewDataTextColumn Caption="Bcc" FieldName="BlindCopyTo" ShowInCustomizationForm="True"
																		VisibleIndex="5">
																		<PropertiesTextEdit DisplayFormatString="d">
																		</PropertiesTextEdit>
																		<EditCellStyle CssClass="GridCss">
																		</EditCellStyle>
																		<EditFormCaptionStyle CssClass="GridCss">
																		</EditFormCaptionStyle>
																		<HeaderStyle CssClass="GridCssHeader" />
																		<CellStyle CssClass="GridCss">
																		</CellStyle><Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
																	</dx:GridViewDataTextColumn>
																	<dx:GridViewDataTextColumn Caption="Event Type" FieldName="EventName" ShowInCustomizationForm="True"
																		VisibleIndex="7" Width="200px">
																		<HeaderStyle CssClass="GridCssHeader" />
																		<CellStyle CssClass="GridCss">
																		</CellStyle><Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
																	</dx:GridViewDataTextColumn>
																</Columns>
																<SettingsBehavior ColumnResizeMode="NextColumn" />
																<SettingsPager PageSize="50">
																	<PageSizeItemSettings Visible="True">
																	</PageSizeItemSettings>
																</SettingsPager>
																<Settings ShowFilterRow="True" ShowGroupPanel="True" />
																<Images SpriteCssFilePath="~/App_Themes/Office2010Blue/{0}/sprite.css">
																	<LoadingPanelOnStatusBar Url="~/App_Themes/Office2010Blue/GridView/Loading.gif">
																	</LoadingPanelOnStatusBar>
																	<LoadingPanel Url="~/App_Themes/Office2010Blue/GridView/Loading.gif">
																	</LoadingPanel>
																</Images>
																<ImagesFilterControl>
																	<LoadingPanel Url="~/App_Themes/Office2010Blue/GridView/Loading.gif">
																	</LoadingPanel>
																</ImagesFilterControl>
																<Styles CssFilePath="~/App_Themes/Office2010Blue/{0}/styles.css" CssPostfix="Office2010Blue">
																	<LoadingPanel ImageSpacing="5px">
																	</LoadingPanel>
																	<Header ImageSpacing="5px" SortingImageSpacing="5px">
																	</Header>
																	<GroupRow Font-Bold="True">
																	</GroupRow>
																	<AlternatingRow CssClass="GridAltRow">
																	</AlternatingRow>
																</Styles>
																<StylesPager>
																	<PageNumber ForeColor="#3E4846">
																	</PageNumber>
																	<Summary ForeColor="#1E395B">
																	</Summary>
																</StylesPager>
																<StylesEditors ButtonEditCellSpacing="0">
																	<ProgressBar Height="21px">
																	</ProgressBar>
																</StylesEditors>
															</dx:ASPxGridView>
														</td>
													</tr>
												</table>
											</td>
										</tr>
									</table>
								</dx:ContentControl>
							</ContentCollection>
						</dx:TabPage>

					</TabPages>
					<ContentStyle>
						<Border BorderColor="#4986A2" />
						<Border BorderColor="#4986A2"></Border>
					</ContentStyle>
				</dx:aspxpagecontrol>
			</td>
		</tr>
		<tr>
			<td colspan="2">
				<div id="errorDiv" runat="server" class="alert alert-danger" style="display: none">
					Error attempting to update the status table.
					<button type="button" class="close" data-dismiss="alert">
						<span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
				</div>
				<table>
					<tr>
						<td align="right">
							<dx:aspxbutton ID="okButton" runat="server" CssClass="sysButton"
								Text="OK" OnClick="okButton_Click"  ValidationGroup="Creds">
							</dx:aspxbutton>
						</td>
						<td>
							<dx:aspxbutton ID="CancelButton" runat="server" CssClass="sysButton"
								Text="Cancel" OnClick="CancelButton_Click" CausesValidation="False">
							</dx:aspxbutton>
						</td>
					</tr>
				</table>
			</td>
		</tr>
	</table>
	<dx:aspxpopupcontrol ID="ErrorMessagePopupControl" runat="server" AllowDragging="True"
		ClientInstanceName="pcErrorMessage" CloseAction="CloseButton" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
		CssPostfix="Glass" EnableAnimation="False" EnableViewState="False" HeaderText="Validation Failure"
		Height="150px" Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
		SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" Width="300px">
		<LoadingPanelImage Url="~/App_Themes/Glass/Web/Loading.gif">
		</LoadingPanelImage>
		<HeaderStyle>
			<Paddings PaddingLeft="10px" PaddingRight="6px" PaddingTop="1px" />
		</HeaderStyle>
		<ContentCollection>
			<dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server" SupportsDisabledAttribute="True">
				<dx:ASPxPanel ID="Panel1" runat="server" DefaultButton="btOK">
					<PanelCollection>
						<dx:PanelContent ID="PanelContent5" runat="server" SupportsDisabledAttribute="True">
							<div style="min-height: 70px;">
								<dx:ASPxLabel ID="ErrorMessageLabel1" runat="server">
								</dx:ASPxLabel>
							</div>
							<div>
								<table cellpadding="0" cellspacing="0" width="100%">
									<tr>
										<td align="center">
											<dx:ASPxButton ID="ValidationOkButton" runat="server" AutoPostBack="False" CssFilePath="~/App_Themes/Office2010Blue/{0}/styles.css"
												CssPostfix="Office2010Blue" SpriteCssFilePath="~/App_Themes/Office2010Blue/{0}/sprite.css"
												Text="OK" Width="80px">
												<ClientSideEvents Click="function(s, e) { pcErrorMessage.Hide(); }" />
											</dx:ASPxButton>
											<dx:ASPxButton ID="ValidationUpdatedButton" runat="server" AutoPostBack="False" CssFilePath="~/App_Themes/Office2010Blue/{0}/styles.css"
												CssPostfix="Office2010Blue" OnClick="ValidationUpdatedButton_Click" SpriteCssFilePath="~/App_Themes/Office2010Blue/{0}/sprite.css"
												Text="OK" Visible="False" Width="80px">
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
	</dx:aspxpopupcontrol>
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
                                        <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ValidationGroup="PopUp">
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
											<ValidationSettings ErrorDisplayMode="ImageWithTooltip"  ValidationGroup="PopUp">
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
											
                                            	<ValidationSettings ErrorDisplayMode="ImageWithTooltip"  ValidationGroup="PopUp">
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
											ClientInstanceName="goButton" CausesValidation="true"  ValidationGroup="PopUp">
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
	<div id="successDiv" runat="server" class="alert alert-success" style="display: none">
		Status information updated successfully.
		<button type="button" class="close" data-dismiss="alert">
			<span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
	</div>
</asp:Content>
