<%@ Page Title="VitalSigns Plus - Log Settings" Language="C#" MasterPageFile="~/Site1.Master"
	AutoEventWireup="true" CodeBehind="LogSettings.aspx.cs" Inherits="VSWebUI.Configurator.WebForm1" %>

<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.Web" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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
					Log Settings</div>
			</td>
		</tr>
		<tr>
			<td>
				<div id="successDiv" runat="server" class="alert alert-success" style="display: none">
					Log settings were successully updated.
					<button type="button" class="close" data-dismiss="alert">
						<span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
				</div>
				<div id="errorDiv" runat="server" class="alert alert-danger" style="display: none">
					<button type="button" class="close" data-dismiss="alert">
						<span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
				</div>
				<div class="info">
					Set the Log Levels and the Log Files Location here, or leave the Log Files Location
					blank to use the default location.
				</div>
			</td>
		</tr>
		<tr>
			<td>
				<dx:ASPxRoundPanel ID="LogSettingsRoundPanel" runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
					CssPostfix="Glass" GroupBoxCaptionOffsetY="-24px" HeaderText="Log Settings Info"
					SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" Width="630px">
					<ContentPaddings PaddingBottom="10px" PaddingTop="10px" PaddingLeft="4px" />
					<HeaderStyle Height="23px"></HeaderStyle>
					<PanelCollection>
						<dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">
							<table class="style1">
								<tr>
									<td>
										<dx:ASPxLabel ID="DefaultLogLevelLabel" runat="server" CssClass="lblsmallFont" CssFilePath="~/css/vswebforms.css"
											Text="Default Log Level:">
										</dx:ASPxLabel>
									</td>
									<td>
										<dx:ASPxComboBox ID="DefaultLogLevelComboBox" runat="server" ValueType="System.String"
											DropDownStyle="DropDownList" Spacing="0">
											<LoadingPanelImage Url="~/App_Themes/Office2010Blue/Editors/Loading.gif">
											</LoadingPanelImage>
											<LoadingPanelStyle ImageSpacing="5px">
											</LoadingPanelStyle>
											<ButtonStyle Width="13px">
											</ButtonStyle>
											<ValidationSettings ErrorDisplayMode="ImageWithTooltip">
												<RequiredField IsRequired="True" ErrorText="It's required" />
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
										<dx:ASPxLabel ID="VSAdapterLogLevelLabel" runat="server" CssClass="lblsmallFont"
											CssFilePath="~/css/vswebforms.css" Text="VSAdapter Log Level:">
										</dx:ASPxLabel>
									</td>
									<td>
										<dx:ASPxComboBox ID="VSAdapterLogLevelComboBox" runat="server" ValueType="System.String"
											DropDownStyle="DropDownList" Spacing="0">
											<LoadingPanelImage Url="~/App_Themes/Office2010Blue/Editors/Loading.gif">
											</LoadingPanelImage>
											<LoadingPanelStyle ImageSpacing="5px">
											</LoadingPanelStyle>
											<ButtonStyle Width="13px">
											</ButtonStyle>
											<ValidationSettings ErrorDisplayMode="ImageWithTooltip">
												<RequiredField IsRequired="True" ErrorText="It's required" />
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
										<dx:ASPxLabel ID="LogFilesLbl" runat="server" CssClass="lblsmallFont" CssFilePath="~/css/vswebforms.css"
											Text="Log Files Location:">
										</dx:ASPxLabel>
									</td>
									<td>
										<dx:ASPxTextBox ID="LogFilestb" runat="server" CssClass="txtsmall" Width="86%">
										</dx:ASPxTextBox>
									</td>
								</tr>
								<tr>
									<td>
										<dx:ASPxLabel ID="LogFileSizelbl" runat="server" CssClass="lblsmallFont" CssFilePath="~/css/vswebforms.css"
											Text="Log File Size:">
										</dx:ASPxLabel>
									</td>
									<td>
										<dx:ASPxTextBox ID="LogfileSizetb" runat="server" CssClass="txtsmall" Width="86%">
										</dx:ASPxTextBox>
									</td>
									<td>
										<dx:ASPxLabel ID="MBLbl" runat="server" CssClass="lblsmallFont" CssFilePath="~/css/vswebforms.css"
											Text="MB">
										</dx:ASPxLabel>
									</td>
								</tr>
								<tr>
									<td>
										<dx:ASPxLabel ID="LogFileRotationlbl" runat="server" CssClass="lblsmallFont" CssFilePath="~/css/vswebforms.css"
											Text="Log File Rotation:">
										</dx:ASPxLabel>
									</td>
									<td>
										<dx:ASPxTextBox ID="LogfileRotationtb" runat="server" CssClass="txtsmall" Width="86%">
										</dx:ASPxTextBox>
									</td>
									<td>
										<dx:ASPxLabel ID="LgFileslbl" runat="server" CssClass="lblsmallFont" CssFilePath="~/css/vswebforms.css"
											Text="Log Files">
										</dx:ASPxLabel>
									</td>
								</tr>
								<tr>
									<td>
										<dx:ASPxButton ID="logfilesdeletebt" runat="server" CssClass="sysButton" AutoPostBack="false"
											Text="Delete Logfiles" UseSubmitBehavior="false" OnClick="logfilesdeletebt_Click">
										</dx:ASPxButton>
									</td>
								</tr>
							</table>
						</dx:PanelContent>
					</PanelCollection>
				</dx:ASPxRoundPanel>
			</td>
		</tr>
        <tr><td><div class="info" id="success" style="display: none" runat="server">
					Verbose setting will be automatically reset to Normal at midnight.
				</div></td></tr>
		<tr>
			<td>
				<table>
					<tr>
						<td>
							<dx:ASPxButton ID="SaveButton" runat="server" CssClass="sysButton" Text="Save" OnClick="SaveButton_Click">
							</dx:ASPxButton>
						</td>
						<td>
							<dx:ASPxButton ID="CancelButton" runat="server" CssClass="sysButton" OnClick="CancelButton_Click"
								Text="Cancel" CausesValidation="False">
							</dx:ASPxButton>
						</td>
					</tr>
				</table>
			</td>
		</tr>
		<tr>
			<td>
				<dx:ASPxPopupControl ID="DeletePopupControl" runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
					CssPostfix="Glass" HeaderText="Confirm" SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css"
					Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
					Theme="MetropolisBlue" EnableHierarchyRecreation="False" Width="270px">
					<%--<HeaderStyle>
						<Paddings PaddingLeft="10px" PaddingRight="6px" PaddingTop="1px" />
					</HeaderStyle>--%>
					<ContentCollection>
						<dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server" SupportsDisabledAttribute="True"
							Width="270px">
							<table class="style1">
								<tr>
									<td width="270px">
										<p>
											Are you sure you want to delete the Log Files</p>
									</td>
								</tr>

							</table>
							<table><tr><td></td></tr></table>
							<table><tr><td></td></tr></table>
							<table class="style1">
								<tr>
									<td>
										<dx:ASPxButton ID="OKCopy" runat="server" CssClass="sysButton" Text="OK" OnClick="OKCopy_Click"
											ClientInstanceName="goButton" CausesValidation="false">
										</dx:ASPxButton>
									</td>
									<td>
										<dx:ASPxButton ID="Cancel" runat="server" CssClass="sysButton" Text="Cancel" OnClick="Cancel_Click"
											ClientInstanceName="goButton" CausesValidation="false">
										</dx:ASPxButton>
									</td>
								</tr>
							</table>
						</dx:PopupControlContentControl>
					</ContentCollection>
				</dx:ASPxPopupControl>
			</td>
		</tr>
	</table>
</asp:Content>
