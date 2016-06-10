<%@ Page Title="VitalSigns Plus- NodeSettings Properties" Language="C#" MasterPageFile="~/Site1.Master" 
AutoEventWireup="true" CodeBehind="NodeSettingsProperties.aspx.cs" Inherits="VSWebUI.Configurator.NodeSettingsProperties" %>

<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>

<%@ Register Assembly="DevExpress.Web.ASPxTreeList.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxTreeList" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<link href='http://fonts.googleapis.com/css?family=Francois One' rel='stylesheet' type='text/css' />
<link rel="stylesheet" type="text/css" href="../css/vswebforms.css" />
<script src="../js/jquery-1.9.1.js" type="text/javascript"></script>
    <script src="../js/bootstrap.min.js" type="text/javascript"></script>
<style type="text/css">

.dxeBase
{
	font: 12px Tahoma;
}
.tab {border-collapse:collapse;}
.tab .first {border-left:1px solid #CCC;border-top:1px solid #CCC;border-right:1px solid #CCC;border-bottom:1px solid #CCC;}
.tab .second {border-left:1px solid #CCC;border-top:1px solid #CCC;border-right:1px solid #CCC;border-bottom:1px solid #CCC;}​
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<table width="100%">
        <tr>
            <td>
                <div class="header" id="lblServer" runat="server"> Node Settings</div>
    <asp:Label ID="lblServerId" runat="server" Font-Bold="False" Visible="False"></asp:Label>
            </td>
        </tr>

        <tr>
            <td>
            <dx:ASPxRoundPanel ID="LocationsRoundPanel" runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
                                CssPostfix="Glass" GroupBoxCaptionOffsetY="-24px" HeaderText="Node Settings Info"
                                SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" Width="630px">
                                <ContentPaddings PaddingBottom="10px" PaddingTop="10px" PaddingLeft="4px" />
                                <HeaderStyle Height="23px">
                                </HeaderStyle>
                                <PanelCollection>
                                    <dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">
                                        <table>
                                            <tr>
                                                <td align="left">
                                                    <table class="style1">
													 <tr>
                                                                    <td>
                                                                        <dx:ASPxLabel ID="ASPxLabel1" runat="server" CssClass="lblsmallFont" 
                                                                            Text="Name:">
                                                                        </dx:ASPxLabel>
                                                                    </td>
                                                                    <td>
                                                                        <dx:ASPxTextBox ID="NameTextBox" runat="server" CssClass="txtsmall" 
                                                                            Width="150px">
                                                                            <ValidationSettings CausesValidation="True" ErrorDisplayMode="ImageWithTooltip" 
                                                                                SetFocusOnError="True">
                                                                            </ValidationSettings>
                                                                        </dx:ASPxTextBox>
                                                                    </td>
                                                                   <td></td>
                                                                    <td colspan="2">
                                                                        <dx:ASPxCheckBox ID="IsPrimaryNodeCheckBox" runat="server" 
                                                                            CheckState="Unchecked" CssClass="lblsmallFont" Text="Primary Node" Wrap="False">
                                                                        </dx:ASPxCheckBox>
                                                                    </td>
 
                                                                    <td>
                                                                        &nbsp;</td>
                                                                    <td>
                                                                        &nbsp;</td>
                                                                    <td>
                                                                        &nbsp;</td>
 
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <dx:ASPxLabel ID="ASPxLabel7" runat="server" CssClass="lblsmallFont" 
                                                                            Text="Host Name:">
                                                                        </dx:ASPxLabel>
                                                                    </td>
                                                                    <td>
                                                                        <dx:ASPxTextBox ID="HostNameTextBox" runat="server" CssClass="txtsmall" 
                                                                            Width="150px">
                                                                        </dx:ASPxTextBox>
                                                                    </td>
                                                                    <td valign="middle">
                                                                    </td>
                                                                    <td>
                                                                        <dx:ASPxLabel ID="AliveTitle" runat="server" CssClass="lblsmallFont" 
                                                                            Text="Alive:">
                                                                        </dx:ASPxLabel>
                                                                    </td>
                                                                    <td>
                                                                        <dx:ASPxLabel ID="AliveTextBox" runat="server" CssClass="lblsmallFont" 
                                                                            Width="150px">
                                                                        </dx:ASPxLabel>
                                                                    </td>
                                                                    <td>
                                                                        &nbsp;</td>
                                                                    <td>
                                                                        <dx:ASPxLabel ID="ASPxLabel2" runat="server" CssClass="lblsmallFont" 
                                                                            Text="Credentials ID:" Visible="False">
                                                                        </dx:ASPxLabel>
                                                                    </td>
                                                                    <td>
                                                                        <dx:ASPxComboBox ID="CredentialsComboBox" runat="server" Visible="False">
                                                                            <ValidationSettings ErrorDisplayMode="ImageWithTooltip" 
                                                                                ErrorText="Credentials may not be empty." SetFocusOnError="True">
                                                                                <RequiredField ErrorText="" IsRequired="True" />
                                                                            </ValidationSettings>
                                                                        </dx:ASPxComboBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <dx:ASPxLabel ID="ASPxLabel3" runat="server" CssClass="lblsmallFont" 
                                                                            Text="Node Type:">
                                                                        </dx:ASPxLabel>
                                                                    </td>
																	<td>
                                                                        <dx:ASPxTextBox ID="NodeTypeTextBox" runat="server" CssClass="txtsmall" 
                                                                            Width="150px">
                                                                           
                                                                        </dx:ASPxTextBox>
                                                                    </td>
																	<td></td>
																	<td>
                                                                        <dx:ASPxLabel ID="VersionTitle" runat="server" CssClass="lblsmallFont" 
                                                                            Text="Version:">
                                                                        </dx:ASPxLabel>
                                                                    </td>
																	<td>
                                                                        <dx:ASPxLabel ID="VersionTextBox" runat="server" CssClass="lblsmallFont" 
                                                                            Width="150px">
                                                                        </dx:ASPxLabel>
                                                                    </td>
                                                                    
                                                                    <td>
                                                                        &nbsp;</td>
                                                                    <td>
                                                                        &nbsp;</td>
                                                                    <td>
                                                                        &nbsp;</td>
                                                                    
                                                                    </tr>
															    <tr>
                                                                    
                                                                    
																	<td>
                                                                        <dx:ASPxLabel ID="ASPxLabel4" runat="server" CssClass="lblsmallFont" 
                                                                            Text="Load Factor:">
                                                                        </dx:ASPxLabel>
                                                                    </td>
                                                                    <td>
                                                                        <dx:ASPxTextBox ID="LoadFactorTextBox" runat="server" CssClass="txtsmall" 
                                                                            Width="150px">
                                                                        </dx:ASPxTextBox>
                                                                    </td>
                                                                   <td></td>
																   <td>
																		<dx:ASPxLabel ID="LocationsLabel" runat="server" Text="Location:" CssClass="lblsmallFont"  />
																   </td>
																   <td>
																		<dx:ASPxComboBox ID="LocationsComboBox" runat="server" AutoPostBack="false" />
																   </td>
																	<td>
                                                                        &nbsp;</td>
                                                                    <td>
                                                                        &nbsp;</td>
                                                                    <td>
                                                                        &nbsp;</td>
                                                                </tr>
															   
													 </table>
													 </tr>
													 </table>
                                    </dx:PanelContent>
                                </PanelCollection>
                            </dx:ASPxRoundPanel>

            </td>
        </tr>
        <tr>
            <td>
            <div id="errorDiv" class="alert alert-danger" runat="server" style="display: none">Error.
            <button type="button" class="close" data-dismiss="alert"><span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                                        </div>
                <table>
                    <tr>
                        <td>
                        
                            <dx:ASPxButton ID="FormOkButton" runat="server" Text="OK" CssClass="sysButton"
                                onclick="FormOkButton_Click" >
                            </dx:ASPxButton>
                        </td>
                        <td>
                            <dx:ASPxButton ID="FormCancelButton" runat="server" Text="Cancel" CssClass="sysButton"
                                onclick="FormCancelButton_Click"  CausesValidation ="false">
                            </dx:ASPxButton>
                        </td>
						<td>
                            <dx:ASPxButton ID="FormDisableButton" runat="server" Text="Disable" CssClass="sysButton"
                                onclick="FormDisableButton_Click"  CausesValidation ="false">
                            </dx:ASPxButton>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
</table>
</asp:Content>
