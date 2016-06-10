<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true"
	CodeBehind="TravelerRegisterPassword.aspx.cs" Inherits="VSWebUI.Configurator.TravelerRegisterPassword" %>

<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.Web" TagPrefix="dx" %>






<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.Web" TagPrefix="dx1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
	<style type="text/css">
		.style1
		{
			width: 187px;
		}
		.style4
		{
			width: 137px;
		}
		.style6
		{
			width: 179px;
		}
	</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
	<table width="80%">
		<tr>
			<td class="style1">
				<div class="header" id="servernamelbldisp" runat="server">
					Register Traveler Password</div>
			</td>
		</tr>
	</table>
	<dx:ASPxRoundPanel ID="RegpwdRoundPanel" runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
		CssPostfix="Glass" GroupBoxCaptionOffsetY="-24px" HeaderText="Register Traveler Password "
		SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" Width="294px">
		<ContentPaddings PaddingBottom="10px" PaddingTop="10px" PaddingLeft="4px" />
		<HeaderStyle Height="23px">
			<Paddings PaddingBottom="0px" PaddingLeft="2px" PaddingTop="0px" />
		</HeaderStyle>
		<PanelCollection>
			<dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">
				<table style="width: 184px">
					<tr>
						
						<td class="style6">
							<dx:ASPxLabel runat="server" Text="Please enter the password" Wrap="False" CssClass="lblsmallFont"
								ID="reglabel">
							</dx:ASPxLabel>
						</td>
						
						
						
					</tr>
					<tr>
						<td class="style6">
							<dx:ASPxTextBox runat="server" Width="179px" ID="TravelPwdTextBox" Height="16px"
								Password="True">
								<ValidationSettings>
									<RequiredField IsRequired="True"></RequiredField>
								</ValidationSettings>
							</dx:ASPxTextBox>
						</td>
						
					</tr>
					<tr><td>
					</td>
					</tr>
					<tr>
						<td class="style6">
							<dx:ASPxButton ID="OkButton" runat="server" CssFilePath="~/App_Themes/Office2010Blue/{0}/styles.css"
								CssPostfix="Office2010Blue" OnClick="OKButton_Click" SpriteCssFilePath="~/App_Themes/Office2010Blue/{0}/sprite.css"
								Text="OK" Wrap="False">
							</dx:ASPxButton>
							&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
							<dx:ASPxButton ID="CancelButton" runat="server" CssFilePath="~/App_Themes/Office2010Blue/{0}/styles.css"
								CssPostfix="Office2010Blue" OnClick="CancelButton_Click" SpriteCssFilePath="~/App_Themes/Office2010Blue/{0}/sprite.css"
								Text="Cancel" Wrap="False">
							</dx:ASPxButton>
						</td>
						
					</tr>
				</table>
			</dx:PanelContent>
		</PanelCollection>
	</dx:ASPxRoundPanel>
</asp:Content>
