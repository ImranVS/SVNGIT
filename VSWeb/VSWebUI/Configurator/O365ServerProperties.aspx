<%@ Page Title="VitalSigns Plus - Office 365 Server Properties" Language="C#" MasterPageFile="~/Site1.Master"
	AutoEventWireup="true" CodeBehind="O365ServerProperties.aspx.cs" Inherits="VSWebUI.Configurator.O365ServerProperties" %>

<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.Web" TagPrefix="dx1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
	<script src="../js/jquery-1.9.1.js" type="text/javascript"></script>
	<script src="../js/bootstrap.min.js" type="text/javascript"></script>
	<link href='http://fonts.googleapis.com/css?family=Francois One' rel='stylesheet'
		type='text/css' />
	<link rel="stylesheet" type="text/css" href="../css/vswebforms.css" />
	<style type="text/css">
		.style1
		{
			width: 100%;
		}
		.style2
		{
			height: 23px;
			width: 4px;
		}
		.style3
		{
			width: 4px;
		}
	</style>
	<script type="text/javascript">
		$(document).ready(function () {
			//			var v = document.getElementById("ContentPlaceHolder1_ASPxPageControl1_ASPxRoundPanel4_PasswordTextBox_I")
			//			v.type = 'password';
		});
		$(document).ready(function () {
			$('.alert-success').delay(10000).fadeOut("slow", function () {
			});
		});
		function OnNameValidation(s, e) {
			var name = e.value;
			var n = name;
			if (name == null || name == "")
				return;
			while (n.indexOf('\'') >= 0) {
				name = n.replace('\'', '');
				n = name;
			}
			while (n.indexOf('\"') >= 0) {
				name = n.replace('\"', '~');
				n = name;
			}
			while (n.indexOf(',') >= 0) {
				name = n.replace(',', ' ');
				n = name;
			}
			e.value = n;
		}
		//           function InitPopupMenuHandler(s, e) {
		//           	//var menu1 = document.getElementById('ContentPlaceHolder1_ASPxPageControl1_UserDetailsMenu');
		//           	//alert(menu1.style.visibility);
		//           	//if (menu1.style.visibility == "visible") {
		//           	var gridCell = document.getElementById('gridCell');
		//           	ASPxClientUtils.AttachEventToElement(gridCell, 'contextmenu', OnGridContextMenu);
		//           	//        var imgButton = document.getElementById('popupButton');
		//           	//        ASPxClientUtils.AttachEventToElement(imgButton, 'contextmenu', OnPreventContextMenu);
		//           	//}
		//           }

		//           function OnGridContextMenu(evt) {
		//           	DeviceGridView.SetFocusedRowIndex(e.index);
		//           	var SortPopupMenu = StatusListPopup;
		//           	SortPopupMenu.ShowAtPos(evt.clientX + ASPxClientUtils.GetDocumentScrollLeft(), evt.clientY + ASPxClientUtils.GetDocumentScrollTop());
		//           	return OnPreventContextMenu(evt);
		//           }

		//           function OnPreventContextMenu(evt) {
		//           	return ASPxClientUtils.PreventEventAndBubble(evt);
		//           }
		//           //10/14/2014 NS modified for VSPLUS-1022
		//           function Tests_ContextMenu(s, e) {
		//           	if (e.objectType == "row") {
		//           		s.GetRowValues(e.index, 'Type', OnGetRowValues);
		//           		s.SetFocusedRowIndex(e.index);
		//           		StatusListPopup.ShowAtPos(ASPxClientUtils.GetEventX(e.htmlEvent), ASPxClientUtils.GetEventY(e.htmlEvent));
		//           	}
		//           }

       
    
	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
	
	<script type="text/javascript">
		$(document).ready(function () {
			var v = document.getElementById("ContentPlaceHolder1_ASPxPageControl1_ASPxRoundPanel4_PasswordTextBox_I")
			//var v = document.getElementById("PasswordTextBox")
			v.type = 'password';
			//			$(":password").val;   
			//$('#PasswordTextBox').autocomplete('disable');
			//===========
			//$(document).ready(function () { $("#PasswordTextBox").attr("autocomplete", "off"); });
			//$(document).ready(function () { $("#UserNameTextBox").attr("autocomplete", "off"); }); 
			//================	

		});

  
	</script>
	<asp:HiddenField ID="hdnPwd" runat="server" />
	<table width="100%">
		<tr>
			<td>
				<div class="header" id="servernamelbldisp" runat="server">
					Microsoft Office 365 Tenant</div>
				<div id="successDiv1" class="alert alert-success" runat="server" style="display: none">
					Success.
				</div>
			</td>
		</tr>
		<tr>
			<td>
				<dx:ASPxPageControl Font-Bold="True" ID="ASPxPageControl1" runat="server" ActiveTabIndex="0"
					CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass" SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css"
					TabSpacing="0px" Width="100%" EnableHierarchyRecreation="False" 
					OnActiveTabChanged="ASPxPageControl1_ActiveTabChanged">
					<ClientSideEvents ActiveTabChanging="function(s,e){e.processOnServer = true}" />
					<TabPages>
						<dx:TabPage Text="Account Properties">
							<TabImage Url="~/images/information.png" />
							<TabImage Url="~/images/information.png">
							</TabImage>
							<ContentCollection>
								<dx:ContentControl ID="ContentControl3" runat="server" SupportsDisabledAttribute="True">
									<table class="style1">
										<tr>
											<td>
												<dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
													CssPostfix="Glass" GroupBoxCaptionOffsetY="-24px" SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css"
													Width="100%" HeaderText="Office 365 Server Attributes">
													<ContentPaddings PaddingBottom="10px" PaddingTop="10px" PaddingLeft="4px" />
													<ContentPaddings PaddingLeft="4px" PaddingTop="10px" PaddingBottom="10px"></ContentPaddings>
													<HeaderStyle Height="23px"></HeaderStyle>
													<PanelCollection>
														<dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">
															<table>
																<tr>
																	<td style="width: 100px" valign="top">
																		<asp:Image ID="Image1" Width="100px" runat="server" ImageUrl="~/images/Cloud_Apps/O365.png" />
																	</td>
																	<td>
																		&nbsp;
																	</td>
																	<td valign="top">
																		<table width="100%">
																			<%--we do not use URL file dnay more, thi setting is stored in settings table and hardly ever change until MS decides to.--%>
																			<tr>
																				<td>
																					<dx1:ASPxLabel ID="ASPxLabel2" runat="server" CssClass="lblsmallFont" Text="Office 365 URL:"
																						Wrap="False" Visible="false">
																					</dx1:ASPxLabel>
																				</td>
																				<%-- <td>
                                                                                    <dx:ASPxComboBox ID="CredentialsComboBox" runat="server" AutoPostBack="true" TextField="Name"
                                                                                        OnSelectedIndexChanged="CredentialsComboBox_SelectedIndexChanged">
                                                                                    </dx:ASPxComboBox>
                                                                                </td>--%>
																				<td>
																					<dx1:ASPxTextBox ID="IPAddressTextBox" runat="server" Width="170px" Visible="false"
																						autocomplete="off">
																						<ValidationSettings ErrorDisplayMode="ImageWithTooltip" SetFocusOnError="True">
																							<RegularExpression ErrorText="Address validation failed" ValidationExpression="(http|ftp|https):\/\/[\w\-_]+(\.[\w\-_]+)+([\w\-\.,@?^=%&amp;amp;:/~\+#]*[\w\-\@?^=%&amp;amp;/~\+#])?" />
																							<RequiredField ErrorText="Enter IP Address" IsRequired="True" />
																							<RegularExpression ErrorText="Address validation failed" ValidationExpression="(http|ftp|https):\/\/[\w\-_]+(\.[\w\-_]+)+([\w\-\.,@?^=%&amp;amp;:/~\+#]*[\w\-\@?^=%&amp;amp;/~\+#])?">
																							</RegularExpression>
																							<RequiredField IsRequired="True" ErrorText="Enter IP Address"></RequiredField>
																						</ValidationSettings>
																					</dx1:ASPxTextBox>
																				</td>
																			</tr>
																			<tr>
																				<td>
																					<dx1:ASPxLabel ID="ASPxLabel1" runat="server" CssClass="lblsmallFont" Text="Name:"
																						Visible="True">
																					</dx1:ASPxLabel>
																				</td>
																				<td>
																					<dx1:ASPxTextBox ID="NameTextBox" runat="server" Visible="True" Width="170px">
																						<ClientSideEvents Validation="OnNameValidation" />
																						<ClientSideEvents Validation="OnNameValidation"></ClientSideEvents>
																						<ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorText="Category may not be empty."
																							SetFocusOnError="True">
																							<RequiredField ErrorText="" IsRequired="True" />
																							<RequiredField IsRequired="True" ErrorText=""></RequiredField>
																						</ValidationSettings>
																					</dx1:ASPxTextBox>
																				</td>

                                                                               
																			
																			</tr>
																			<tr>
																				<td>
																					<dx:ASPxLabel ID="ASPxLabel3" runat="server" CssClass="lblsmallFont" Text="Category:">
																					</dx:ASPxLabel>
																				</td>
																				<td>
																					<dx:ASPxTextBox ID="CategoryTextBox" runat="server" Width="170px">
																						<ClientSideEvents Validation="OnNameValidation" />
																						<ValidationSettings ErrorDisplayMode="ImageWithTooltip" SetFocusOnError="True" ErrorText="Category may not be empty.">
																							<RequiredField IsRequired="True" ErrorText="" />
																						</ValidationSettings>
																					</dx:ASPxTextBox>
																				</td>

                                                                                	<td>
																					<dx:ASPxCheckBox ID="EnabledCheckBox" runat="server" CheckState="Unchecked" CssClass="lblsmallFont"
																						Text="Enabled for scanning" Wrap="False" Checked="false" ClientInstanceName="enablecheckbox"
																						Enabled="false">
																						<ValidationSettings ErrorDisplayMode="None">
																						</ValidationSettings>
																					</dx:ASPxCheckBox>
																				</td>
																				<td>
																					<table width="215px">
																						<tr>
																							<td>
																								<dx1:ASPxLabel ID="enablemsg" runat="server" CssClass="lblsmallFont" Text="(To enable scanning, please select at least one node on the Nodes tab)"
																									Width="215px" Visible="False">
																								</dx1:ASPxLabel>
																							</td>
																						</tr>
																					</table>
																				</td>
																				
																			</tr>

                                                                            <tr>
                                                                             <td>
																					<dx1:ASPxLabel ID="CostperuserLabel" runat="server" CssClass="lblsmallFont" Text="Cost per user :"
																						Visible="True">
																					</dx1:ASPxLabel>
																				</td>
																				<td>
																					<dx1:ASPxTextBox ID="CostperuserTextBox" runat="server" Visible="True" Width="170px">
																						<%--<ClientSideEvents Validation="OnNameValidation" />
																						<ClientSideEvents Validation="OnNameValidation"></ClientSideEvents>
																						<ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorText="Category may not be empty."
																							SetFocusOnError="True">
																							<RequiredField ErrorText="" IsRequired="True" />
																							<RequiredField IsRequired="True" ErrorText=""></RequiredField>
																						</ValidationSettings>--%>
																					</dx1:ASPxTextBox>
																				</td>
                                                                            </tr>
																			<tr>
																				<td>
																					<dx:ASPxLabel ID="ASPxLabel12" runat="server" CssClass="lblsmallFont" Height="16px"
																						Text="Mode:">
																					</dx:ASPxLabel>
																				</td>
																				<td>
																					<dx:ASPxRadioButtonList ID="SortRadioButtonList1" runat="server" AutoPostBack="True"
																						SelectedIndex="0" OnSelectedIndexChanged="SortRadioButtonList1_SelectedIndexChanged"
																						RepeatDirection="Horizontal" TextWrap="False">
																						<Items>
																							<dx:ListEditItem Text="ADFS" Value="1" />
																							<dx:ListEditItem Text="Cloud Only" Value="2" />
																							<dx:ListEditItem Text="Dir Sync" Value="3" />
																						</Items>
																					</dx:ASPxRadioButtonList>
																				</td>
																				
																				<td>
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
											<td>
												<dx:ASPxRoundPanel ID="CloudASPxRoundPanel2" runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
													CssPostfix="Glass" GroupBoxCaptionOffsetY="-24px" HeaderText="Scan Settings"
													SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" Width="100%">
													<ContentPaddings PaddingBottom="10px" PaddingTop="10px" PaddingLeft="4px" />
													<ContentPaddings PaddingLeft="4px" PaddingTop="10px" PaddingBottom="10px"></ContentPaddings>
													<HeaderStyle Height="23px"></HeaderStyle>
													<PanelCollection>
														<dx:PanelContent ID="PanelContent2" runat="server" SupportsDisabledAttribute="True">
															<table>
																<tr>
																	<td>
																		<dx:ASPxLabel ID="ASPxLabel4" runat="server" CssClass="lblsmallFont" Text="Scan Interval:"
																			Wrap="False">
																		</dx:ASPxLabel>
																	</td>
																	<td>
																		<dx:ASPxTextBox ID="ScanTextBox" runat="server" Style="margin-top: 0px" Width="40px">
																			<MaskSettings Mask="&lt;0..99999&gt;" />
																			<MaskSettings Mask="&lt;0..99999&gt;"></MaskSettings>
																			<ValidationSettings ErrorDisplayMode="ImageWithTooltip" CausesValidation="True" SetFocusOnError="True">
																				<RegularExpression ErrorText="Please enter a numeric value using the numbers only (0-9)."
																					ValidationExpression="^\d+$"></RegularExpression>
																				<RequiredField IsRequired="True"></RequiredField>
																				<RegularExpression ErrorText="Please enter a numeric value using the numbers only (0-9)."
																					ValidationExpression="^\d+$" />
																				<RequiredField IsRequired="True" />
																			</ValidationSettings>
																		</dx:ASPxTextBox>
																	</td>
																	<td>
																		<dx:ASPxLabel ID="ASPxLabel6" runat="server" CssClass="lblsmallFont" Text="minutes">
																		</dx:ASPxLabel>
																	</td>
																	<td>
																		&nbsp;
																	</td>
																	<td>
																		<dx:ASPxLabel ID="ASPxLabel8" runat="server" CssClass="lblsmallFont" Text="Response Threshold:"
																			Wrap="False">
																		</dx:ASPxLabel>
																	</td>
																	<td>
																		<dx:ASPxTextBox ID="RespThrTextBox" runat="server" CssClass="txtmed">
																			<MaskSettings Mask="&lt;0..99999&gt;" />
																			<MaskSettings Mask="&lt;0..99999&gt;"></MaskSettings>
																			<ValidationSettings CausesValidation="True" ErrorDisplayMode="ImageWithTooltip" SetFocusOnError="True">
																				<RegularExpression ErrorText="Please enter a numeric value using the numbers only (0-9)."
																					ValidationExpression="^\d+$" />
																				<RegularExpression ErrorText="Please enter a numeric value using the numbers only (0-9)."
																					ValidationExpression="^\d+$"></RegularExpression>
																			</ValidationSettings>
																		</dx:ASPxTextBox>
																	</td>
																	<td>
																		<dx:ASPxLabel ID="ASPxLabel9" runat="server" CssClass="lblsmallFont" Text="milliseconds">
																		</dx:ASPxLabel>
																	</td>
																</tr>
																<tr>
																	<td>
																		<dx:ASPxLabel ID="ASPxLabel5" runat="server" CssClass="lblsmallFont" Text="Retry Interval:"
																			Wrap="False">
																		</dx:ASPxLabel>
																	</td>
																	<td>
																		<dx:ASPxTextBox ID="RetryTextBox" runat="server" Width="40px">
																			<MaskSettings Mask="&lt;1..99999&gt;"></MaskSettings>
																			<ValidationSettings ErrorDisplayMode="ImageWithTooltip" CausesValidation="True" SetFocusOnError="True">
																				<RegularExpression ErrorText="Please enter a numeric value using the numbers only (1-9)."
																					ValidationExpression="^\d+$"></RegularExpression>
																				<RequiredField IsRequired="True"></RequiredField>
																			</ValidationSettings>
																		</dx:ASPxTextBox>
																	</td>
																	<td>
																		<dx:ASPxLabel ID="ASPxLabel10" runat="server" CssClass="lblsmallFont" Text="minutes">
																		</dx:ASPxLabel>
																	</td>
																	<td>
																		&nbsp;
																	</td>
																	<td>
																		<dx:ASPxLabel ID="ASPxLabel31" runat="server" CssClass="lblsmallFont" Text="Failures before Alert:"
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
																				<RequiredField ErrorText="Please enter the failure threshold.  How many times can the server be down before an alert is sent?"
																					IsRequired="True" />
																				<RegularExpression ErrorText="Please enter a numeric value using the numbers only (0-9)."
																					ValidationExpression="^\d+$" />
																				<RequiredField IsRequired="True" ErrorText="Please enter the failure threshold.  How many times can the server be down before an alert is sent?">
																				</RequiredField>
																				<RegularExpression ErrorText="Please enter a numeric value using the numbers only (0-9)."
																					ValidationExpression="^\d+$"></RegularExpression>
																			</ValidationSettings>
																		</dx:ASPxTextBox>
																	</td>
																	<td>
																		<dx:ASPxLabel ID="ASPxLabel32" runat="server" CssClass="lblsmallFont" Text="consecutive failures"
																			Wrap="False">
																		</dx:ASPxLabel>
																	</td>
																</tr>
																<tr>
																	<td>
																		<dx:ASPxLabel ID="ASPxLabel7" runat="server" CssClass="lblsmallFont" Text="Off-Hours Scan Interval:"
																			Wrap="False">
																		</dx:ASPxLabel>
																	</td>
																	<td>
																		<dx:ASPxTextBox ID="OffScanTextBox" runat="server" Width="40px">
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
																		<dx:ASPxLabel ID="ASPxLabel11" runat="server" CssClass="lblsmallFont" Text="minutes">
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
												<dx:ASPxRoundPanel ID="DirSyncPanel" runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
													CssPostfix="Glass" GroupBoxCaptionOffsetY="-24px" HeaderText="Directory Sync Settings"
													SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" Width="100%" Visible="false">
													<ContentPaddings PaddingBottom="10px" PaddingTop="10px" PaddingLeft="4px" />
													<ContentPaddings PaddingLeft="4px" PaddingTop="10px" PaddingBottom="10px"></ContentPaddings>
													<HeaderStyle Height="23px"></HeaderStyle>
													<PanelCollection>
														<dx:PanelContent ID="PanelContent3" runat="server" SupportsDisabledAttribute="True">
															<table>
															<tr>
															<td>
																
																					
																	<dx:ASPxLabel ID="lblservername" runat="server" CssClass="lblsmallFont" Text="Server Name:"
																		>
																	</dx:ASPxLabel>
																</td>
																<td>
																	<dx:ASPxTextBox ID="txtservername" runat="server" Width="170px" >
																		<ClientSideEvents Validation="OnNameValidation" />
																		<ValidationSettings ErrorDisplayMode="ImageWithTooltip" SetFocusOnError="True" ErrorText="ServerName may not be empty.">
																			<RequiredField IsRequired="True" ErrorText="" />
																		</ValidationSettings>
																	</dx:ASPxTextBox>
																</td>
																						
															</tr>
															<tr>
															<td>
																					
																	<dx:ASPxLabel ID="lblcrecombo" runat="server" CssClass="lblsmallFont" Text="Credentials Id:"
																		 Wrap="False">
																	</dx:ASPxLabel>
																</td>
																<td>
																	<dx:ASPxComboBox ID="creComboBox" runat="server" >
																	</dx:ASPxComboBox>
																</td>
																<td>
																	<dx:ASPxButton ID="ASPxButton1" runat="server" Text="Add Credentials" CssClass="sysButton"
																		OnClick="btn_clickcopyprofile" CausesValidation="false"  UseSubmitBehavior="false">
																	</dx:ASPxButton>
																</td>
																						
															</tr>
															</table>
														</dx:PanelContent>
													</PanelCollection>
												</dx:ASPxRoundPanel>
											</td>
										</tr>
										<%--   <tr>
                                                        <td>
                                                            <dx:ASPxRoundPanel ID="ASPxRoundPanel3" runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
                                                                CssPostfix="Glass" GroupBoxCaptionOffsetY="-24px" SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css"
                                                                Width="100%" HeaderText="Optional: Search Text">
                                                                <ContentPaddings PaddingBottom="10px" PaddingTop="10px" PaddingLeft="4px" />
                                                                <ContentPaddings PaddingLeft="4px" PaddingTop="10px" PaddingBottom="10px"></ContentPaddings>
                                                                <HeaderStyle Height="23px">
                                                                    <Paddings PaddingBottom="0px" PaddingLeft="2px" PaddingTop="0px" />
                                                                    <Paddings PaddingLeft="2px" PaddingTop="0px" PaddingBottom="0px"></Paddings>
                                                                </HeaderStyle>
                                                                <PanelCollection>
                                                                    <dx:PanelContent ID="PanelContent3" runat="server" SupportsDisabledAttribute="True">
                                                                        <table>
                                                                            <tr>
                                                                                <td>
                                                                                    <dx:ASPxLabel ID="ASPxLabel12" runat="server" Text="Search Text  (optional):" CssClass="lblsmallFont">
                                                                                    </dx:ASPxLabel>
                                                                                </td>
                                                                                <td rowspan="2" valign="top">
                                                                                    <asp:Label ID="Label1" runat="server" CssClass="lblsmallFont" Text="If you entered a value into the Search Text box, then VitalSigns will try to locate this text on the O365Server page and if it can’t find it, an alert will be generated."></asp:Label>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    <dx:ASPxTextBox ID="RequiredTextBox" runat="server" Width="170px">
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
                                                    </tr>--%>
										<tr>
											<td>
												<dx:ASPxRoundPanel ID="ASPxRoundPanel4" runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
													CssPostfix="Glass" GroupBoxCaptionOffsetY="-24px" SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css"
													Width="100%" HeaderText="Username/Password">
													<ContentPaddings PaddingBottom="10px" PaddingTop="10px" PaddingLeft="4px" />
													<ContentPaddings PaddingLeft="4px" PaddingTop="10px" PaddingBottom="10px"></ContentPaddings>
													<HeaderStyle Height="23px"></HeaderStyle>
													<PanelCollection>
														<dx:PanelContent ID="PanelContent5" runat="server" SupportsDisabledAttribute="True">
															<table>
																<tr>
																	<td>
																		<table>
																		<tr>
                                                                        <td>
																		<dx:ASPxLabel ID="ASPxLabel13" runat="server" Text="Username:" CssClass="lblsmallFont">
																		</dx:ASPxLabel>
																	</td>
																	<td>
																		<dx:ASPxTextBox ID="UserNameTextBox" runat="server" Width="170px" AutoCompleteType="Disabled"
																			autocomplete="off">
																			<ClientSideEvents Validation="OnNameValidation" />
																			<ValidationSettings ErrorDisplayMode="ImageWithTooltip" SetFocusOnError="True" ErrorText="Category may not be empty.">
																				<RequiredField IsRequired="True" ErrorText="" />
																			</ValidationSettings>
																		</dx:ASPxTextBox>
                                                                        </td>
                                                                        </tr>
                                                                        </table>
																	</td>
																	<td rowspan="2" valign="top">
																		<asp:Label ID="Label2" runat="server" CssClass="lblsmallFont" Text="VitalSigns will use the Username and Password provided here. If the provided credentials fail, an alert will be generated."></asp:Label>
																	</td>
																</tr>
																<tr>
																	<td style="margin-left: 40px">
                                                                    <table>
																		<tr>
                                                                        <td>
																		<dx:ASPxLabel ID="ASPxLabel14" runat="server" Text="Password:" CssClass="lblsmallFont">
																		</dx:ASPxLabel>
																	</td>
																	<td>
																		<dx:ASPxTextBox ID="PasswordTextBox" Paddings-PaddingRight="50px" runat="server" Width="170px" AutoCompleteType="Disabled"
																			autocomplete="off" >
																			<ClientSideEvents Validation="OnNameValidation" />
																			<ValidationSettings ErrorDisplayMode="None">
																			</ValidationSettings>
																		</dx:ASPxTextBox>
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
						<dx:TabPage Text="Tests/Options">
							<TabImage Url="~/images/application_view_tile.png" />
							<TabImage Url="~/images/application_view_tile.png">
							</TabImage>
							<ContentCollection>
								<dx:ContentControl ID="ContentControl4" runat="server" SupportsDisabledAttribute="True">
									<table>
										<tr>
											<td>
												<table width="150%">
													<tr>
														<td>
															<div id="successDiv" class="alert alert-success" runat="server" style="display: none">
																Success.
															</div>
														</td>
													</tr>
													
													<tr>
														<td>
															<dx:ASPxRoundPanel ID="ASPxRoundPanel2" runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
															CssPostfix="Glass" GroupBoxCaptionOffsetY="-24px" HeaderText="Overall Pass/Fail Tests"
															Height="50px" SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" Width="800px">
															<ContentPaddings Padding="2px" PaddingBottom="10px" PaddingLeft="4px" PaddingTop="10px" />
															<ContentPaddings Padding="2px" PaddingLeft="4px" PaddingTop="10px" PaddingBottom="10px">
															</ContentPaddings>
															<HeaderStyle Height="23px">
															</HeaderStyle>
															<PanelCollection>
																<dx:PanelContent ID="PanelContent6" runat="server" SupportsDisabledAttribute="True">
																	<asp:Label ID="Label1" runat="server" Style="color: #0033CC" Text="Check any items you would like to be tested upon scanning this server."></asp:Label>
																	<table>
																		<tr>
																			<td>
																				<dx:ASPxCheckBox ID="chbxsmtp" runat="server" CheckState="Unchecked" Text="SMTP">
																				</dx:ASPxCheckBox>
																			</td>
																			<td>
																				<dx:ASPxCheckBox ID="chbxpop" runat="server" CheckState="Unchecked" Text="POP">
																				</dx:ASPxCheckBox>
																			</td>
																			<td>
																				<dx:ASPxCheckBox ID="chbxowa" runat="server" CheckState="Unchecked" Text="OWA">
																				</dx:ASPxCheckBox>
																			</td>
																			
																		</tr>
																		<tr>
																			<td>
																				<dx:ASPxCheckBox ID="chbxautodiscovery" runat="server" CheckState="Unchecked" Text="Auto Discovery">
																				</dx:ASPxCheckBox>
																			</td>
																			<td>
																				<dx:ASPxCheckBox ID="chbxmapi" runat="server" CheckState="Unchecked" Text="MAPI Connectivity">
																				</dx:ASPxCheckBox>
																			</td>
																		</tr>
																		<tr>
																			<td>
																				<dx:ASPxCheckBox ID="chbxcalender" runat="server" CheckState="Unchecked" Text="Create Calendar">
																				</dx:ASPxCheckBox>
																			</td>
																			<td>
																				<dx:ASPxCheckBox ID="chbxinbox" runat="server" CheckState="Unchecked" Text="Inbox">
																				</dx:ASPxCheckBox>
																			</td>
																		</tr>
																		<tr>
																			<td>
																				<dx:ASPxCheckBox ID="chbxtask" runat="server" CheckState="Unchecked" Text="Create Task">
																				</dx:ASPxCheckBox>
																			</td>
																			<td>
																				<dx:ASPxCheckBox ID="chbximap" runat="server" CheckState="Unchecked" Text="IMAP">
																				</dx:ASPxCheckBox>
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
															<dx:ASPxRoundPanel ID="ASPxRoundPanel3" runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
															CssPostfix="Glass" GroupBoxCaptionOffsetY="-24px" HeaderText="Performance Tests"
															Height="50px" SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" Width="800px">
															<ContentPaddings Padding="2px" PaddingBottom="10px" PaddingLeft="4px" PaddingTop="10px" />
															<ContentPaddings Padding="2px" PaddingLeft="4px" PaddingTop="10px" PaddingBottom="10px">
															</ContentPaddings>
															<HeaderStyle Height="23px">
															</HeaderStyle>
															<PanelCollection>
																<dx:PanelContent ID="PanelContent7" runat="server" SupportsDisabledAttribute="True">
																	<asp:Label ID="Label3" runat="server" Style="color: #0033CC" Text="Check any items you would like to be tested upon scanning this server."></asp:Label>
																	<table>
																			<tr>
																				<td valign="top">
																					<table>
																						<tr>
																							<td valign="top">
																								<dx:ASPxLabel runat="server" Text="Mail Flow Threshold (ms):" ID="ASPxLabel23" CssClass="lblsmallFont"
																									Wrap="False">
																								</dx:ASPxLabel>
																							</td>
																							<td valign="top">
																								<dx:ASPxTextBox Width="50px" runat="server" CssClass="txtsmall" ID="SrvAtrMailFlowThTextBox"
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
																								<dx:ASPxLabel ID="ASPxLabel25" runat="server" CssClass="lblsmallFont" Text="Create Folder Threshold (ms):"
																									Wrap="False">
																								</dx:ASPxLabel>
																							</td>
																							<td valign="top">
																								<dx:ASPxTextBox ID="SrvAtrCreateFolderThTextBox" Width="50px" runat="server" CssClass="txtsmall" Text="500">
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
																								<dx:ASPxLabel ID="ASPxLabel17" runat="server" CssClass="lblsmallFont" Text="Create Site Threshold (ms):"
																									Wrap="False">
																								</dx:ASPxLabel>
																							</td>
																							<td valign="top">
																								<dx:ASPxTextBox ID="srvAtrCreateSiteThTextBox" runat="server" Width="50px" CssClass="txtsmall" Text="500">
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
																								<dx:ASPxLabel ID="ASPxLabel26" runat="server" CssClass="lblsmallFont" Text="OneDrive Upload Threshold (ms):"
																									Wrap="False">
																								</dx:ASPxLabel>
																							</td>
																							<td valign="top">
																								<dx:ASPxTextBox ID="SrvAtrODUploadThTextBox" runat="server" Width="50px" CssClass="txtsmall" Text="200">
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
																								<dx:ASPxLabel ID="ASPxLabel15" runat="server" CssClass="lblsmallFont" Text="OneDrive Download Threshold (ms):"
																									Wrap="False">
																								</dx:ASPxLabel>
																							</td>
																							<td valign="top">
																								<dx:ASPxTextBox ID="SrvAtrODDownloadThTextBox" runat="server"  Width="50px" CssClass="txtsmall" Text="200">
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
																							<td colspan="3" valign="top">
																								<dx:ASPxCheckBox ID="chbxmailflow" runat="server" CheckState="Unchecked"
																									Text="Mail Flow Test"
																									CssClass="lblsmallFont">
																								</dx:ASPxCheckBox>
																							</td>
																						</tr>
																						<tr>
																							<td valign="top">
																								<dx:ASPxCheckBox ID="chbxcreatefolder" runat="server" CheckState="Unchecked"
																									CssClass="lblsmallFont" Text="Create Folder Test" Wrap="False">
																								</dx:ASPxCheckBox>
																							</td>
																						</tr>
																						<tr>
																							<td valign="top">
																								<dx:ASPxCheckBox ID="chbxcreatesite" runat="server" CheckState="Unchecked"
																									CssClass="lblsmallFont" Text="Create Site Test" Wrap="False">
																								</dx:ASPxCheckBox>
																							</td>
																						</tr>
																						<tr>
																							<td colspan="3" valign="top">
																								<dx:ASPxCheckBox ID="chbxonedriveupload" runat="server" CheckState="Unchecked"
																									CssClass="lblsmallFont" Text="OneDrive Upload Test" Wrap="False">
																								</dx:ASPxCheckBox>
																							</td>
																						</tr>
																						<tr>
																							<td colspan="3" valign="top">
																								<dx:ASPxCheckBox ID="chbxonedrivedownload" runat="server" CheckState="Unchecked"
																									CssClass="lblsmallFont" Text="OneDrive Download Test" Wrap="False">
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
														<td>
															<dx:ASPxRoundPanel ID="ASPxRoundPanel5" runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
															CssPostfix="Glass" GroupBoxCaptionOffsetY="-24px" HeaderText="Directory Syncronization Tests"
															Height="50px" SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" Width="800px">
															<ContentPaddings Padding="2px" PaddingBottom="10px" PaddingLeft="4px" PaddingTop="10px" />
															<ContentPaddings Padding="2px" PaddingLeft="4px" PaddingTop="10px" PaddingBottom="10px">
															</ContentPaddings>
															<HeaderStyle Height="23px">
															</HeaderStyle>
															<PanelCollection>
																<dx:PanelContent ID="PanelContent4" runat="server" SupportsDisabledAttribute="True">
																	<asp:Label ID="Label4" runat="server" Style="color: #0033CC" Text="Check any items you would like to be tested upon scanning this server."></asp:Label>
																	<table>
																			<tr>
																				<td valign="top">
																					<table>
																						<tr>
																							<td valign="top">
																								<dx:ASPxLabel runat="server" Text="Dir Sync Imp/Exp Threshold (Minutes):" ID="ASPxLabel16" CssClass="lblsmallFont"
																									Wrap="False">
																								</dx:ASPxLabel>
																							</td>
																							<td valign="top">
																								<dx:ASPxTextBox runat="server" Width="50px" CssClass="txtsmall" ID="dirsynctxt"
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
																					</table>
																				</td>
																				<td valign="top">
																					<table>
																						<tr>
																							<td colspan="3" valign="top">
																								<dx:ASPxCheckBox ID="chbxdirsync" runat="server" CheckState="Unchecked"
																									Text="Dir Sync Imp/Export Test"
																									CssClass="lblsmallFont">
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
											</td>
										</tr>
									</table>
								</dx:ContentControl>
							</ContentCollection>
						</dx:TabPage>
						<dx:TabPage Text="Nodes">
							<TabImage Url="~/images/application_view_tile.png" />
							<TabImage Url="~/images/application_view_tile.png">
							</TabImage>
							<ContentCollection>
								<dx:ContentControl ID="ContentControl5" runat="server" SupportsDisabledAttribute="True">
									<table>
										<tr>
											<td>
												<table width="150%">
													<tr>
														<td>
															<div id="Div1" class="alert alert-danger" runat="server" style="display: none">
																
															</div>
														</td>
													</tr>
													<tr>
														<td>
															<dx:ASPxCallback ID="cb1" runat="server" ClientInstanceName="cb1" OnCallback="cb1_callback" />
															<dx:ASPxGridView ID="Nodes" runat="server" AutoGenerateColumns="False" ClientInstanceName="NodesGridView"
																EnableTheming="True" KeyFieldName="Id" Theme="Office2003Blue" OnPreRender="Nodes_PreRender"
																Width="60%" OnCommandButtonInitialize="Nodes_CommandButtonInitialize" OnSelectionChanged="Nodes_SelectionChanged"
																EnableCallBacks="false" OnPageSizeChanged="NodesGridView_OnPageSizeChanged">
																<Settings ShowGroupPanel="false" ShowFilterRow="false" />
																<%-- <ClientSideEvents SelectionChanged="Nodes_SelectionChanged"/>--%>
																<SettingsBehavior ProcessSelectionChangedOnServer="true" ColumnResizeMode="NextColumn">
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
																	<dx:GridViewDataTextColumn Caption="NodeId" FieldName="NodeId" VisibleIndex="1" Visible="false">
																		<Settings AutoFilterCondition="Contains" />
																		<EditCellStyle CssClass="GridCss">
																		</EditCellStyle>
																		<EditFormCaptionStyle CssClass="GridCss" HorizontalAlign="Left">
																		</EditFormCaptionStyle>
																		<HeaderStyle CssClass="GridCssHeader" />
																		<CellStyle CssClass="GridCss">
																		</CellStyle>
																	</dx:GridViewDataTextColumn>
																	<dx:GridViewDataTextColumn Caption="O365ServerId" FieldName="O365ServerId" VisibleIndex="3"
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
																	<dx:GridViewCommandColumn Caption="Select Nodes" ShowSelectCheckbox="true" VisibleIndex="2"
																		Width="200px">
																		<ClearFilterButton Visible="True">
																		</ClearFilterButton>
																		<HeaderStyle CssClass="GridCssHeader1" />
																	</dx:GridViewCommandColumn>
																	<dx:GridViewDataTextColumn Caption="Node Name" FieldName="Name" VisibleIndex="4"
																		Width="200px">
																		<Settings AutoFilterCondition="Contains" />
																		<Settings AllowSort="False"></Settings>
																		<EditCellStyle CssClass="GridCss">
																		</EditCellStyle>
																		<EditFormCaptionStyle CssClass="GridCss" HorizontalAlign="Left">
																		</EditFormCaptionStyle>
																		<HeaderStyle CssClass="GridCssHeader" />
																		<CellStyle CssClass="GridCss">
																		</CellStyle>
																	</dx:GridViewDataTextColumn>
																
																	<dx:GridViewDataTextColumn Caption="Host Name" FieldName="HostName" Width="200px"
																		VisibleIndex="5">
																		<HeaderStyle CssClass="GridCssHeader" />
																		<CellStyle CssClass="GridCss">
																		</CellStyle>
																	</dx:GridViewDataTextColumn>
																	<dx:GridViewDataTextColumn Caption="Location" FieldName="Location" Width="200px"
																		VisibleIndex="6">
																		<HeaderStyle CssClass="GridCssHeader" />
																		<CellStyle CssClass="GridCss">
																		</CellStyle>
																	</dx:GridViewDataTextColumn>
																	
																
																</Columns>
																
																<SettingsPager PageSize="20" AlwaysShowPager="True">
									                             <PageSizeItemSettings  Visible="True">
									                               </PageSizeItemSettings>
								                                  </SettingsPager>
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
									</table>
								</dx:ContentControl>
							</ContentCollection>
						</dx:TabPage>
						<dx:TabPage Text="Maintenance Windows">
							<TabImage Url="~/images/application_view_tile.png" />
							<TabImage Url="~/images/application_view_tile.png">
							</TabImage>
							<ContentCollection>
								<dx:ContentControl ID="ContentControl2" runat="server" SupportsDisabledAttribute="True">
									<%-- <dx:ASPxRoundPanel ID="MaintRoundPanel" runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
                                            CssPostfix="Glass" GroupBoxCaptionOffsetY="-24px" HeaderText="Maintenance Windows"
                                            Height="50px" SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" 
                                            Width="100%">
                                            <ContentPaddings PaddingBottom="10px" PaddingTop="10px" PaddingLeft="4px" />
                                            <ContentPaddings Padding="2px" PaddingTop="10px" PaddingBottom="10px"></ContentPaddings>
                                            <HeaderStyle Height="23px">
                                                <Paddings PaddingBottom="0px" PaddingLeft="2px" PaddingTop="0px" />
                                                <Paddings Padding="0px" PaddingLeft="2px" PaddingRight="2px" PaddingBottom="7px">
                                                </Paddings>
                                            </HeaderStyle>
                                            <PanelCollection>
                                                <dx:PanelContent ID="PanelContent7" runat="server" SupportsDisabledAttribute="True">--%>
									<table class="style1">
										<tr>
											<td>
												<div id="infoDivMaint" class="info">
													Maintenance Windows are times when you do not want the server monitored. You can
													define maintenance windows using the Hours &amp; Maintenance\Maintenance menu option.
													Use the Actions column to modify maintenance windows information.
												</div>
												<dx:ASPxButton ID="ToggleVeiwButton" runat="server" CssFilePath="~/App_Themes/Office2010Blue/{0}/styles.css"
													CssPostfix="Office2010Blue" SpriteCssFilePath="~/App_Themes/Office2010Blue/{0}/sprite.css"
													Text="Switch to Calendar view" Visible="False" Width="178px">
												</dx:ASPxButton>
											</td>
										</tr>
										<tr>
											<td>
												<dx:ASPxGridView ID="MaintWinListGridView" runat="server" AutoGenerateColumns="False"
													CssFilePath="~/App_Themes/Office2010Blue/{0}/styles.css" Width="100%" CssPostfix="Office2010Blue"
													KeyFieldName="ID" Cursor="pointer" EnableTheming="True" OnHtmlRowCreated="MaintWinListGridView_HtmlRowCreated"
													OnSelectionChanged="MaintWinListGridView_SelectionChanged" Theme="Office2003Blue"
													OnPageSizeChanged="MaintWinListGridView_PageSizeChanged">
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
															<Settings AllowAutoFilter="False" />
															<Settings AllowAutoFilter="False"></Settings>
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
															</CellStyle>
															<Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
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
															</CellStyle>
															<Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
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
									<%--   </dx:PanelContent>
                                            </PanelCollection>
                                        </dx:ASPxRoundPanel>--%>
								</dx:ContentControl>
							</ContentCollection>
						</dx:TabPage>
						<dx:TabPage Text="Alert">
							<TabImage Url="../images/icons/error.png">
							</TabImage>
							<ContentCollection>
								<dx:ContentControl ID="ContentControl1" runat="server" SupportsDisabledAttribute="True">
									<table class="style1">
										<tr>
											<td>
												<div id="infoDivAlerts" class="info">
													The list of available alerts is listed below. In order to add new alerts or configure
													existing alerts, please use the Alerts\Alert Definitions menu.
												</div>
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
															</CellStyle>
															<Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
														</dx:GridViewDataTextColumn>
														<dx:GridViewDataTextColumn Caption="Start Time" FieldName="StartTime" ReadOnly="True"
															ShowInCustomizationForm="True" VisibleIndex="1">
															<HeaderStyle CssClass="GridCssHeader" />
															<CellStyle CssClass="GridCss">
															</CellStyle>
														</dx:GridViewDataTextColumn>
														<dx:GridViewDataTextColumn Caption="To" FieldName="SendTo" ShowInCustomizationForm="True"
															VisibleIndex="3">
															<HeaderStyle CssClass="GridCssHeader" />
															<CellStyle CssClass="GridCss">
															</CellStyle>
															<Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
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
															</CellStyle>
															<Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
														</dx:GridViewDataTextColumn>
														<dx:GridViewDataTextColumn Caption="Bcc" FieldName="BlindCopyTo" ShowInCustomizationForm="True"
															VisibleIndex="5">
															<PropertiesTextEdit DisplayFormatString="d">
															</PropertiesTextEdit>
															<EditCellStyle CssClass="GridCss">
															</EditCellStyle>
															<EditFormCaptionStyle CssClass="GridCss">
															</EditFormCaptionStyle>
															<HeaderStyle CssClass="GridCssHeader"></HeaderStyle>
															<CellStyle CssClass="GridCss">
															</CellStyle>
															<Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
														</dx:GridViewDataTextColumn>
														<dx:GridViewDataTextColumn Caption="Event Type" FieldName="EventName" ShowInCustomizationForm="True"
															VisibleIndex="7">
															<HeaderStyle CssClass="GridCssHeader" />
															<CellStyle CssClass="GridCss">
															</CellStyle>
															<Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
														</dx:GridViewDataTextColumn>
														<dx:GridViewDataTextColumn Caption="Day(s)" FieldName="Day" ShowInCustomizationForm="True"
															VisibleIndex="9">
															<HeaderStyle CssClass="GridCssHeader" />
															<CellStyle CssClass="GridCss">
															</CellStyle>
														</dx:GridViewDataTextColumn>
														<dx:GridViewDataTextColumn Caption="Duration" FieldName="Duration" ShowInCustomizationForm="True"
															VisibleIndex="2">
															<HeaderStyle CssClass="GridCssHeader" />
															<CellStyle CssClass="GridCss">
															</CellStyle>
															<Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
														</dx:GridViewDataTextColumn>
													</Columns>
													<SettingsBehavior ColumnResizeMode="NextColumn" />
													<SettingsBehavior ColumnResizeMode="NextColumn"></SettingsBehavior>
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
						<Border BorderColor="#4986A2"></Border>
					</ContentStyle>
				</dx:ASPxPageControl>
			</td>
		</tr>
		<tr>
			<td>
				<dx:ASPxPopupControl runat="server" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
					SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" Modal="True" EnableHierarchyRecreation="False"
					HeaderText="Please Enter Credentials" CssPostfix="Glass" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
					Theme="MetropolisBlue" Width="300px" ID="CopyProfilePopupControl">
					<HeaderStyle>
						<Paddings PaddingTop="1px" PaddingLeft="10px" PaddingRight="6px"></Paddings>
					</HeaderStyle>
					<ContentCollection>
						<dx:PopupControlContentControl runat="server">
							<table>
								<tr>
									<td>
										<dx:ASPxLabel runat="server" Text="Alias Name:" Wrap="False" CssClass="lblsmallFont"
											ID="AliasNameLabel">
										</dx:ASPxLabel>
									</td>
									<td>
										<dx:ASPxTextBox runat="server" Width="170px" ClientInstanceName="AliasName" ID="AliasName">
											<ValidationSettings ErrorDisplayMode="ImageWithTooltip">
												<RequiredField IsRequired="True" ErrorText="Enter Alias Name"></RequiredField>
											</ValidationSettings>
										</dx:ASPxTextBox>
									</td>
								</tr>
								<tr>
									<td>
										<dx:ASPxLabel runat="server" Text="User ID:" Wrap="False" CssClass="lblsmallFont"
											ID="UserIDLabel">
										</dx:ASPxLabel>
									</td>
									<td>
										<dx:ASPxTextBox runat="server" Width="170px" ClientInstanceName="UserID" ID="UserID">
											<ValidationSettings ErrorDisplayMode="ImageWithTooltip">
												<RequiredField IsRequired="True" ErrorText="Enter UserID"></RequiredField>
											</ValidationSettings>
										</dx:ASPxTextBox>
									</td>
								</tr>
								<tr>
									<td>
										<dx:ASPxLabel runat="server" Text="Password:" Wrap="False" CssClass="lblsmallFont"
											ID="PasswordLabel">
										</dx:ASPxLabel>
									</td>
									<td>
										<dx:ASPxTextBox runat="server" Width="170px" Password="True" ClientInstanceName="password"
											ID="Password">
											<ClientSideEvents KeyDown="function(s, e) {OnKeyDown(s, e);}"></ClientSideEvents>
											<ValidationSettings ErrorDisplayMode="ImageWithTooltip">
												<RequiredField IsRequired="True" ErrorText="Enter Password"></RequiredField>
											</ValidationSettings>
										</dx:ASPxTextBox>
									</td>
								</tr>
								<tr>
									<td colspan="2">
										<div runat="server" id="Div3" class="alert alert-danger" style="display: none">
											Error:
										</div>
									</td>
								</tr>
							</table>
							<table>
								<tr>
									<td>
										<dx:ASPxButton runat="server" ClientInstanceName="goButton" CausesValidation="False"
											Text="OK" CssClass="sysButton" ID="OKCopy" OnClick="OKCopy_Click">
										</dx:ASPxButton>
									</td>
									<td>
										<dx:ASPxButton runat="server" ClientInstanceName="goButton" CausesValidation="False"
											Text="Cancel" CssClass="sysButton" ID="Cancel" OnClick="Cancel_Click">
										</dx:ASPxButton>
									</td>
								</tr>
							</table>
						</dx:PopupControlContentControl>
					</ContentCollection>
				</dx:ASPxPopupControl>
			</td>
		</tr>
		<tr>
			<td>
				<div id="errorDiv" runat="server" class="alert alert-danger" style="display: none">
					Error attempting to update the status table.
					<button type="button" class="close" data-dismiss="alert">
						<span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
				</div>
				<table>
					<tr>
						<td>
							<dx:ASPxButton ID="FormOkButton" runat="server" Text="OK" CssClass="sysButton" OnClick="FormOkButton_Click"
								AutoPostBack="False">
							</dx:ASPxButton>
						</td>
						<td>
							<dx:ASPxButton ID="FormCancelButton" runat="server" Text="Cancel" CssClass="sysButton"
								OnClick="FormCancelButton_Click" CausesValidation="False" AutoPostBack="False">
							</dx:ASPxButton>
						</td>
						<td>
							<dx:ASPxButton ID="Resetoffice365Button" runat="server" CssClass="sysButton" Visible="false"
								Text="Reset" OnClick="Resetoffice365Button_Click">
							</dx:ASPxButton>
						</td>
					</tr>
				</table>
			</td>
		</tr>
	</table>

</asp:Content>
