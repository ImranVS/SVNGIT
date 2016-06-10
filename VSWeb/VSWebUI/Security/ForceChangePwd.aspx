<%@ Page Title="VitalSigns Plus - Change Password" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="ForceChangePwd.aspx.cs" Inherits="VSWebUI.Security.ForceChangePwd" %>

<%@ Register assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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
	<table style="width:100%;">
    <tr>
        <td width="35%">
            &nbsp;</td>
        <td>
            &nbsp;</td>
        <td>
            &nbsp;</td>
    </tr>
    <tr>

        <td>
            &nbsp;</td>
        <td>
            <table>
			<tr>
<td>
 <div class="info">Password requires at least one lower case letter, one upper case letter, one digit, 6-13 characters, and no spaces.</div>
</td>
</tr>
                <tr>
                    <td>
                        <div id="successDiv" runat="server" class="alert alert-success" style="display: none">
                            Password was changed successully.
							
                            <button type="button" class="close" data-dismiss="alert"><span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                       </div>
                    </td>
                </tr>
				<tr>
			<%--	<td>
				 <div  id="labelDiv" class="info" runat = "server">
        </div>
				</td>--%>
				<td>
				<div id="infoDivLoad" class="info">
                                           
                                        Your password has been reset. Please change the temporary password.
                                        </div>
										</td>

				</tr>
                <tr>
                    <td>
                        <dx:ASPxRoundPanel ID="ChangepwdRoundPanel" runat="server" 
                            CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass" 
                            GroupBoxCaptionOffsetY="-24px" HeaderText="Change Password" 
                            SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" Width="400px">
                            <ContentPaddings PaddingBottom="10px" PaddingLeft="4px" PaddingTop="10px" />
                            <HeaderStyle Height="23px">
                            <Paddings PaddingBottom="0px" PaddingLeft="2px" PaddingTop="0px" />
                            </HeaderStyle>
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent1" runat="server" 
                                    SupportsDisabledAttribute="True">
                                    <table class="style1">
                                        <tr>
                                            <td>
                                                <dx:ASPxLabel ID="ASPxLabel1" runat="server" CssClass="lblsmallFont" 
                                                    Text="Current Password:">
                                                </dx:ASPxLabel>
                                            </td>
                                            <td>
                                                <dx:ASPxTextBox ID="CurrentpwdTextBox" runat="server" Password="True" 
                                                    Width="170px">
                                                    <ValidationSettings ErrorDisplayMode="ImageWithTooltip">
                                                        <RequiredField ErrorText="Enter Current Password" IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                &nbsp;</td>
                                            <td>
                                                <dx:ASPxLabel ID="pwdLabel" runat="server" ForeColor="#FF3300">
                                                </dx:ASPxLabel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <dx:ASPxLabel ID="ASPxLabel2" runat="server" CssClass="lblsmallFont" 
                                                    Text="New Password:">
                                                </dx:ASPxLabel>
                                            </td>
                                            <td>
                                                            <dx:ASPxTextBox ID="NewpwdTextBox" runat="server" Width="170px" Password="True" ClientInstanceName="NewpwdTextBox">
															<%--<clientsideevents Validation="function(s, e) {e.isValid = (s.GetText())}" />--%>
                                                                <ValidationSettings ErrorDisplayMode="ImageWithTooltip">
                                                                    <RequiredField IsRequired="True" ErrorText="Enter New Password." />
																	<RegularExpression ErrorText="Password requires at least one lower case letter, one upper case letter, one digit, 6-13 characters, and no spaces." ValidationExpression="^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?!.*\s).{6,13}$"></RegularExpression>
				
                                                                </ValidationSettings>
                                                            </dx:ASPxTextBox>
                                                        </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <dx:ASPxLabel ID="ASPxLabel3" runat="server" CssClass="lblsmallFont" 
                                                    Text="Confirm Password:">
                                                </dx:ASPxLabel>
                                            </td>
                                            <td>
                                                 <dx:ASPxTextBox ID="ConfirmpwdTextBox" runat="server" Width="170px"  ClientInstanceName="ConfirmpwdTextBox"
                                                                Password="True">
																  <clientsideevents Validation="function(s, e) {e.isValid = (s.GetText() == NewpwdTextBox.GetText());}" />

                                                                <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorText="Passwords do not match. Please make sure you enter the same password in the new and confirm password fields.">
                                                                 
                                                               </ValidationSettings>
                                                            </dx:ASPxTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <dx:ASPxLabel ID="ErrorMsg" runat="server" ForeColor="Red">
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
                        <div id="errorDiv" runat="server" class="alert alert-danger" style="display: none">
                            Passwords don&#39;t match. Please make sure you enter the same password in the New 
                            and Confirm Password fields.
                            <button type="button" class="close" data-dismiss="alert"><span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table>
                            <tr>
                                <td>
                                    <dx:ASPxButton ID="SaveButton" runat="server" 
                                        CssFilePath="~/App_Themes/Office2010Blue/{0}/styles.css" 
                                        CssPostfix="Office2010Blue" OnClick="SaveButton_Click" 
                                        SpriteCssFilePath="~/App_Themes/Office2010Blue/{0}/sprite.css" Text="Save">
                                    </dx:ASPxButton>
                                </td>
                                <td>
                                    <dx:ASPxButton ID="CancelButton" runat="server" CausesValidation="False" 
                                        CssFilePath="~/App_Themes/Office2010Blue/{0}/styles.css" 
                                        CssPostfix="Office2010Blue" OnClick="CancelButton_Click" 
                                        SpriteCssFilePath="~/App_Themes/Office2010Blue/{0}/sprite.css" Text="Cancel">
                                    </dx:ASPxButton>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </td>
        <td>
            &nbsp;</td>
    </tr>
    <tr>
        <td>
            &nbsp;</td>
        <td>
            &nbsp;</td>
        <td>
            &nbsp;</td>
    </tr>
</table>
</asp:Content>
