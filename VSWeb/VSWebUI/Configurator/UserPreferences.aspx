<%@ Page Title="VitalSigns Plus - User Preferences" Language="C#" MasterPageFile="~/Site1.Master"
	AutoEventWireup="true" CodeBehind="UserPreferences.aspx.cs" Inherits="VSWebUI.CompanyLogo" %>

<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.Web" TagPrefix="dx1" %>
<%@ Register Assembly="DevExpress.Web.ASPxScheduler.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.Web.ASPxScheduler" TagPrefix="dxwschs" %>
<%@ Register Assembly="DevExpress.XtraScheduler.v14.2.Core, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.XtraScheduler" TagPrefix="cc1" %>
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
	<table>
		<tr>
			<td>
				<div class="header" id="servernamelbldisp" runat="server">
					General Preferences</div>
			</td>
		</tr>
        <tr><td>
        
        </td></tr>
      <tr>
			<td>
				<div class="error" id='divErrorMessage' runat="server" visible="false">
					User is not authorised to access this page.
				</div>
			</td>
		</tr>
		<tr>
			<td>
				<div class="info" id='divInfo' runat="server">
					The User Preferences page allows you to change the default values of the Server
					Attributes as well as modify general VitalSigns preferences. In order to set the
					default Server Attributes, first select the attributes you wish to change, then
					specify the value, and finally, click the 'Save' button.
				</div>
			</td>
		</tr>
          </table>
        <table runat="server" id="tblPreferences">
		<tr>
			<td>
				<dx:ASPxPageControl Font-Bold="True" ID="ASPxPageControl1" runat="server" ActiveTabIndex="0"
					CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass" SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css"
					TabSpacing="0px" Width="100%" EnableHierarchyRecreation="False">
					<TabPages>
						<dx:TabPage Text="Logo">
							<TabImage Url="~/images/information.png">
							</TabImage>
							<ContentCollection>
								<dx:ContentControl ID="LogoPreferencesContent" runat="server" SupportsDisabledAttribute="True">
									<asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
										<ContentTemplate>
											<table>
												<tr>
													<td>
														<dx:ASPxLabel ID="ASPxLabel3" runat="server" CssClass="lblsmallFont" Text="Company Name:">
														</dx:ASPxLabel>
													</td>
													<td>
														<dx:ASPxTextBox ID="CompanyNameTextBox" runat="server" Width="170px">
															<ValidationSettings ErrorDisplayMode="ImageWithTooltip">
																<RequiredField IsRequired="True" />
															</ValidationSettings>
														</dx:ASPxTextBox>
													</td>
												</tr>
												<tr>
													<td>
														<dx:ASPxLabel ID="ASPxLabel2" runat="server" CssClass="lblsmallFont" Text="Company Logo:">
														</dx:ASPxLabel>
													</td>
													<td>
														<dx:ASPxUploadControl ID="FileUploadControl" runat="server" Theme="Office2010Blue"
															Width="280px" OnFileUploadComplete="FileUploadControl_FileUploadComplete" ClientInstanceName="uploader">
															<%-- <ClientSideEvents FileUploadComplete="function(s, e) { Uploader_OnFileUploadComplete(e); }" />
                                                        <ValidationSettings MaxFileSize="30000" AllowedFileExtensions="Gif,.jpg,.png"
                                                            MaxFileSizeErrorText="The size of the uploaded file exceeds max size 30KB." />--%>
															<ClientSideEvents FileUploadComplete="function(s, e) { Uploader_OnFileUploadComplete(e); }"
																FilesUploadComplete="function(s, e) { Uploader_OnFilesUploadComplete(e); }" FileUploadStart="function(s, e) { Uploader_OnUploadStart(); }"
																TextChanged="function(s, e) { UpdateUploadButton(); }"></ClientSideEvents>
															<%--  <ValidationSettings MaxFileSize="30000" AllowedFileExtensions=".jpg,.jpeg,.jpe,.gif,.png"  MaxFileSizeErrorText="The size of the uploaded file exceeds max size 30KB.">
                                                                    </ValidationSettings>--%>
															<ValidationSettings AllowedFileExtensions=".jpg,.jpeg,.jpe,.gif,.png" MaxFileSize="30000"
																MaxFileSizeErrorText="The size of the uploaded file exceeds the maximum supported size of 30 KB." NotAllowedFileExtensionErrorText="Only .jpg, .jpeg, .jpe, .gif, and .png files may be uploaded." />
														</dx:ASPxUploadControl>
													</td>
												</tr>
												<tr>
													<td>
													</td>
													<td>
													</td>
												</tr>
												<tr>
													<td valign="middle">
														<dx:ASPxLabel ID="ASPxLabel4" runat="server" CssClass="lblsmallFont" Text="Currently Set Logo:">
														</dx:ASPxLabel>
													</td>
													<td valign="middle">
														<dx:ASPxHyperLink ID="FilePathHyperLink" runat="server" NavigateUrl="~/images/hsbc.png"
															Target="_blank" Text="Company Logo">
														</dx:ASPxHyperLink>
													</td>
												</tr>
											</table>
											<table>
												<tr>
													<td>
														<dx:ASPxButton runat="server" Text="Save" ID="OkButton" CssClass="sysButton" OnClick="LoginButton_Click">
															<%--<ClientSideEvents Click="function(s, e) { uploader.Upload(); }" />
															<ClientSideEvents Click="function(s, e) { uploader.Upload(); }"></ClientSideEvents>--%>
														</dx:ASPxButton>
													</td>
													<td>
														<dx:ASPxButton runat="server" CausesValidation="False" Text="Cancel" CssClass="sysButton"
															ID="CancelButton" OnClick="CancelButton_Click">
														</dx:ASPxButton>
													</td>
												</tr>
											</table>
										</ContentTemplate>
										<Triggers>
											<asp:PostBackTrigger ControlID="OkButton" />
										</Triggers>
									</asp:UpdatePanel>
								</dx:ContentControl>
							</ContentCollection>
						</dx:TabPage>
					<%--	<dx:TabPage Text="User Preferences">
							<TabImage Url="~/images/information.png">
							</TabImage>
							<ContentCollection>
								<dx:ContentControl ID="GeneralPreferencesContent" runat="server" SupportsDisabledAttribute="True">
									<asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional">
										<ContentTemplate>
											<table>
												<tr>
													<td colspan="3">
														<div id="infoRefreshDiv" class="info">
															Set your preferences below:
															<br />
															- Refresh Time - preferred page refresh time is seconds.
															<br />
															- Starting Page - a page from the drop down list that you would like to open each
															time you enter VitalSigns.
															<br />
															- Disk Space Alert Threshold - preferred alert threshold for Disk Space (Yellow
															area).
														</div>
													</td>
												</tr>
											</table>
											<table>
												<tr>
													<td>
														<dx1:ASPxLabel ID="RefreshLabel" runat="server" CssClass="lblsmallFont" Text="Refresh Time:">
														</dx1:ASPxLabel>
													</td>
													<td>
														<dx1:ASPxTextBox ID="RefreshTimeTextBox" runat="server" Width="170px">
															<MaskSettings Mask="&lt;1..999999999&gt;" />
															<ValidationSettings ErrorDisplayMode="ImageWithTooltip" SetFocusOnError="True">
																<RequiredField ErrorText="Enter Refresh Time" IsRequired="True" />
																<RegularExpression ErrorText="Please enter a numeric value using the numbers only (0-9)."
																	ValidationExpression="^\d+$" />
															</ValidationSettings>
														</dx1:ASPxTextBox>
													</td>
													<td>
														<dx1:ASPxLabel ID="ASPxLabel6" runat="server" CssClass="lblsmallFont" Text="seconds">
														</dx1:ASPxLabel>
													</td>
												</tr>
												<tr>
													<td>
														<dx:ASPxLabel ID="StartUpPageLabel" runat="server" CssClass="lblsmallFont" Text="Starting Page:">
														</dx:ASPxLabel>
													</td>
													<td>
														<dx:ASPxComboBox ID="StartupURLCombobox" runat="server" DropDownStyle="DropDown"
															Spacing="0" TextField="Name" ValueField="URL">
															<LoadingPanelImage Url="~/App_Themes/Office2010Blue/Editors/Loading.gif">
															</LoadingPanelImage>
															<LoadingPanelStyle ImageSpacing="5px">
															</LoadingPanelStyle>
															<ButtonStyle Width="13px">
															</ButtonStyle>
															<ValidationSettings ErrorDisplayMode="ImageWithTooltip">
																<RequiredField ErrorText="It's required" IsRequired="True" />
															</ValidationSettings>
														</dx:ASPxComboBox>
													</td>
													<td>
														&nbsp;
													</td>
												</tr>
												
												<tr>
													<td colspan="2">
														<div id="UserPreferenceError" class="alert alert-danger" runat="server" style="display: none">
															Account settings were NOT updated.
															<button type="button" class="close" data-dismiss="alert">
																<span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
														</div>
														<div id="UserPreferenceSuccess" class="alert alert-success" runat="server" style="display: none">
															Account settings were successully updated.
															<button type="button" class="close" data-dismiss="alert">
																<span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
														</div>
													</td>
													<td>
														&nbsp;
													</td>
												</tr>
											</table>
											<table>
												<tr>
													<td>
														<dx:ASPxButton ID="UserPreferenceApply" runat="server" Text="Save" Visible="true"
															CssClass="sysButton" OnClick="UserPreferenceApply_Click">
														</dx:ASPxButton>
													</td>
													<td>
														<dx:ASPxButton ID="ASPxButton5" runat="server" Text="Cancel" Visible="true" CssClass="sysButton"
															OnClick="CancelButton_Click">
														</dx:ASPxButton>
													</td>
												</tr>
											</table>
										</ContentTemplate>
										<Triggers>
											<asp:AsyncPostBackTrigger ControlID="UserPreferenceApply" />
										</Triggers>
									</asp:UpdatePanel>
								</dx:ContentControl>
							</ContentCollection>
						</dx:TabPage>
					--%>	<dx:TabPage Text="General Settings">
							<TabImage Url="~/images/information.png">
							</TabImage>
							<ContentCollection>
								<dx:ContentControl ID="ContentControl3" runat="server" SupportsDisabledAttribute="True">
									<asp:UpdatePanel ID="UpdatePanel6" runat="server" UpdateMode="Conditional">
										<ContentTemplate>
											<table>
												<tr>
													<td valign="top" colspan="3">
														<div id="Div1" class="info">
															Set your preferences below:
															<br />
															- Status Changed Threshold - time in minutes you would like to keep track of changed
															statuses. Default is 15 minutes.
															<br />
															- Monitoring Delay - time delay in minutes between scans. When the interval between
															the previous scan and the next scan exceeds the specified value, VitalSigns will
															trigger a delay notification. I.e., if the last VitalSigns scan occurred at 7:30
															AM, the monitoring delay parameter is set to 10 minutes, and the current time is
															7:45 AM, VitalSigns will trigger a notification since the target scan time was 7:40
															AM.
                                                            <br />
															- Disk Space Alert Threshold - preferred alert threshold for Disk Space (Yellow
															area).
														</div>
													</td>
												</tr>
											</table>
											<table>
												<tr>
													<td>
														<dx1:ASPxLabel ID="ASPxLabel1" runat="server" CssClass="lblsmallFont" Text="Status Changed Threshold:"
															Wrap="False">
														</dx1:ASPxLabel>
													</td>
													<td>
														<dx1:ASPxTextBox ID="StatusChagnedTextBox" runat="server" Width="170px">
															<MaskSettings Mask="&lt;1..999999999&gt;" />
															<ValidationSettings ErrorDisplayMode="ImageWithTooltip" SetFocusOnError="True">
																<RequiredField ErrorText="Enter Status Changed Threshold" IsRequired="True" />
																<RegularExpression ErrorText="Please enter a numeric value using the numbers only (0-9)."
																	ValidationExpression="^\d+$" />
															</ValidationSettings>
														</dx1:ASPxTextBox>
													</td>
													<td>
														<dx1:ASPxLabel ID="ASPxLabel7" runat="server" CssClass="lblsmallFont" Text="minutes">
														</dx1:ASPxLabel>
													</td>
												</tr>
												<tr>
													<td>
														<dx:ASPxLabel ID="MonitoringDelayLabel" runat="server" CssClass="lblsmallFont" Text="Monitoring Delay:"
															Wrap="False">
														</dx:ASPxLabel>
													</td>
													<td>
														<dx:ASPxTextBox ID="MonitoringDelayTextBox" runat="server" Width="170px">
															<%--<MaskSettings Mask="&lt;1..999999999&gt;" />--%>
															<ValidationSettings ErrorDisplayMode="ImageWithTooltip" SetFocusOnError="True">
																<RequiredField ErrorText="Enter Monitoring Delay" IsRequired="True" />
																<RegularExpression ErrorText="Please enter a numeric value using the numbers only (0-9)."
																	ValidationExpression="^\d+$" />
															</ValidationSettings>
														</dx:ASPxTextBox>
													</td>
													<td>
														<dx1:ASPxLabel ID="ASPxLabel8" runat="server" CssClass="lblsmallFont" Text="minutes">
														</dx1:ASPxLabel>
													</td>
                                                    </tr>
                                                <tr>
                                                    <td>
                                                        <dx:ASPxLabel ID="StartUpPageLabel0" runat="server" CssClass="lblsmallFont" Text="Disk Space Alert Threshold:">
                                                        </dx:ASPxLabel>
                                                    </td>
                                                    <td>
                                                        <dx:ASPxTextBox ID="DiskSpaceAlertThreshold" runat="server" Width="170px">
                                                            <MaskSettings Mask="&lt;1..999999999&gt;" />
                                                            <ValidationSettings ErrorDisplayMode="ImageWithTooltip" SetFocusOnError="True">
                                                                <RequiredField ErrorText="Enter Disk Space Alert Threshold" IsRequired="True" />
                                                                <RegularExpression ErrorText="Please enter a numeric value using the numbers only (0-9)."
                                                                    ValidationExpression="^\d+$" />
                                                            </ValidationSettings>
                                                        </dx:ASPxTextBox>
                                                    </td>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                                   <tr>
                                                    <td>
                                                        <dx:ASPxLabel ID="ASPxLabel6" runat="server" CssClass="lblsmallFont" Text="Currency Symbol:">
                                                        </dx:ASPxLabel>
                                                    </td>
                                                    <td>
                                                        <dx:ASPxTextBox ID="CurrencySymbolTextBox" runat="server" Width="170px" Text="$">
                                                            
                                                        </dx:ASPxTextBox>
                                                    </td>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                                    	<tr>
																	<td>
																		<dx:ASPxLabel ID="ASPxLabel9" runat="server" CssClass="lblsmallFont" Height="16px"
																			Text="Dashboard only/Exec Summary Buttons:">
																		</dx:ASPxLabel>
																	</td>
																	<td>
																		<dx:ASPxRadioButtonList ID="SortRadioButtonList1" runat="server" 
                                                                            AutoPostBack="True" SelectedIndex="0"
																			 RepeatDirection="Horizontal">
																			<Items>
																				<dx:ListEditItem Text="Show" Value="1" />
																				<dx:ListEditItem Text="Hide" Value="2" />
																			</Items>
																		</dx:ASPxRadioButtonList>
																	</td>
																
																</tr>
												
												<tr>
													<td colspan="3">
														<div id="GeneralErrorDiv" class="alert alert-danger" runat="server" style="display: none">
															Settings were NOT updated.
															<button type="button" class="close" data-dismiss="alert">
																<span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
														</div>
														<div id="GeneralSuccessDiv" class="alert alert-success" runat="server" style="display: none">
															Settings were successully updated.
															<button type="button" class="close" data-dismiss="alert">
																<span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
														</div>
													</td>
												</tr>
											</table>
											<table>
												<tr>
													<td>
														<dx:ASPxButton ID="GeneralApply" runat="server" Text="Save" Visible="true" CssClass="sysButton"
															OnClick="GeneralApply_Click">
														</dx:ASPxButton>
													</td>
													<td>
														<dx:ASPxButton ID="GeneralCancel" runat="server" Text="Cancel" Visible="true" CssClass="sysButton"
															OnClick="CancelButton_Click">
														</dx:ASPxButton>
													</td>
												</tr>
											</table>
										</ContentTemplate>
									</asp:UpdatePanel>
								</dx:ContentControl>
							</ContentCollection>
						</dx:TabPage>
						<dx:TabPage Text="Bing Key">
							<TabImage Url="~/images/information.png">
							</TabImage>
							<ContentCollection>
								<dx:ContentControl ID="ContentControl1" runat="server" SupportsDisabledAttribute="True">
									<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
										<ContentTemplate>
											<table>
												<tr>
													<td valign="top" colspan="3">
														<div id="Div3" class="info">
															This key is issued by Bing to use their services. Please go <a href="http://www.microsoft.com/maps/create-a-bing-maps-key.aspx"
																style="color: White" target="_blank">here</a> for more information.
														</div>
													</td>
												</tr>
											</table>
											<table>
												<tr>
													<td>
														<dx1:ASPxLabel ID="ASPxLabel5" runat="server" CssClass="lblsmallFont" Text="Bing API Key:"
															Wrap="False">
														</dx1:ASPxLabel>
													</td>
													<td>
														<dx1:ASPxTextBox ID="BingKeyTextBox" runat="server" Width="170px">
                                                        <ValidationSettings ErrorDisplayMode="ImageWithTooltip" >
												<RequiredField IsRequired="True"  ErrorText="Please enter Bing API Key." />
												
											</ValidationSettings>
														</dx1:ASPxTextBox>
													</td>
													<td>
													</td>
												</tr>
												<tr>
													<td colspan="3">
														<div id="BingErrorDiv" class="alert alert-danger" runat="server" style="display: none">
															Settings were NOT updated.
															<button type="button" class="close" data-dismiss="alert">
																<span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
														</div>
														<div id="BingSuccessDiv" class="alert alert-success" runat="server" style="display: none">
															Settings were successully updated.
															<button type="button" class="close" data-dismiss="alert">
																<span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
														</div>
													</td>
												</tr>
											</table>
											<table>
												<tr>
													<td>
														<dx:ASPxButton ID="BingApply" runat="server" Text="Save" Visible="true" CssClass="sysButton"
															OnClick="BingApply_Click">
														</dx:ASPxButton>
													</td>
													<td>
														<dx:ASPxButton ID="ASPxButton2" runat="server" Text="Cancel" Visible="true" CssClass="sysButton"
															OnClick="CancelButton_Click">
														</dx:ASPxButton>
													</td>
												</tr>
											</table>
										</ContentTemplate>
									</asp:UpdatePanel>
								</dx:ContentControl>
							</ContentCollection>
						</dx:TabPage>
					</TabPages>
					<LoadingPanelImage Url="~/App_Themes/Glass/Web/Loading.gif">
					</LoadingPanelImage>
					<Paddings PaddingLeft="0px"></Paddings>
					<ActiveTabStyle Font-Bold="True">
					</ActiveTabStyle>
					<ContentStyle>
						<Border BorderColor="#4986A2" />
						<Border BorderColor="#4986A2"></Border>
					</ContentStyle>
				</dx:ASPxPageControl>
			</td>
		</tr>
		<tr>
			<td>
				<div id="logoerrordiv" runat="server" class="alert alert-danger" style="display: none">
					The Logo was not updated.
					<button type="button" class="close" data-dismiss="alert">
						<span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
				</div>
				<div id="logosuccessdiv" runat="server" class="alert alert-success" style="display: none">
					The Logo was successfully updated.
					<button type="button" class="close" data-dismiss="alert">
						<span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
				</div>
			</td>
		</tr>
		<tr>
			<td>
				<div id="errorDiv" runat="server" class="alert alert-danger" style="display: none">
					The preferences were NOT updated.
					<button type="button" class="close" data-dismiss="alert">
						<span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
				</div>
				<div id="successDiv" runat="server" class="alert alert-success" style="display: none">
					The preferences were successfully updated.
					<button type="button" class="close" data-dismiss="alert">
						<span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
				</div>
			</td>
		</tr>
	</table>
</asp:Content>
