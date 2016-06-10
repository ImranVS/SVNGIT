<%@ Page Title="VitalSigns Plus - Forgot Password" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="ForgotPassword.aspx.cs" Inherits="VSWebUI.Security.ForgotPassword" %>


<%@ Register assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>







<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
	<style type="text/css">
        .style1
        {
            width: 102%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
	<table width="100%">
	 <tr>
                        <td>
                            <div id="successDiv" runat="server" class="alert alert-success" style="display: none">Password was reset successully.
                            <button type="button" class="close" data-dismiss="alert"><span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                </div>
                        </td>
						<td>
        <div id="errorDiv" runat="server" class="alert alert-danger" style="display: none">Passwords do not match. Please make sure you enter the same password in the new and confirm password fields.
        <button type="button" class="close" data-dismiss="alert"><span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
        </div>
                                                            <dx:ASPxLabel runat="server" 
                ForeColor="Red" ID="ErrorMsg"></dx:ASPxLabel>

        </td>
                    </tr>
        <tr>
            <td align="center">
                <dx:ASPxRoundPanel ID="LocationsRoundPanel" runat="server" 
                                        CssFilePath="~/App_Themes/Glass/{0}/styles.css" 
                                        CssPostfix="Glass" 
                                        GroupBoxCaptionOffsetY="-24px" HeaderText="Forgot Password" 
                                        SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" 
                                        Width="630px">
                                        <HeaderStyle Height="23px">
                                        </HeaderStyle>
                                        <PanelCollection>
                                            <dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">
                            <table width="100%" border="0">
                                <tr>
                                    <td colspan="3" align="left">
                                        <table width="100%">
                                            <tr>
                                                <td width="200px"> 
                                                    <dx:ASPxLabel ID="ForgotPwdLoginNameLabel" runat="server" 
                                                        CssClass="lblsmallFont" CssFilePath="~/css/vswebforms.css" Text="Login Name:">
                                                    </dx:ASPxLabel>
                                                </td>
                                                <td>
                                                    <dx:ASPxTextBox ID="ForgotPwdLoginNameTextBox" runat="server" Width="190px">
                                                    </dx:ASPxTextBox>
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <dx:ASPxLabel ID="ASPxLabel1" runat="server" CssClass="lblsmallFont" 
                                                        CssFilePath="~/css/vswebforms.css" Text="Email:">
                                                    </dx:ASPxLabel>
                                                </td>
                                                <td>
                                                    <dx:ASPxTextBox ID="ForgotPwdEmailTextBox" runat="server" Width="190px">
                                                    </dx:ASPxTextBox>
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <dx:ASPxButton ID="ForgotPwdVerifyAccountButton" runat="server" OnClick="ForgotPwdVerifyAccountButton_Click" 
                                                        SpriteCssFilePath="~/App_Themes/Office2010Blue/{0}/sprite.css" 
                                                        Text="Verify Account" Wrap="False" CssClass="sysButton">
                                                    </dx:ASPxButton>
                                                </td>
                                                <td>
                                                    <dx:ASPxLabel ID="ForgotPwdVerifyAcctLabel" runat="server" ForeColor="Red" 
                                                        Text="User account could not be verified." Visible="False">
                                                    </dx:ASPxLabel>
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
								<tr><td><dx:ASPxRadioButtonList ID="SortRadioButtonList1" runat="server" AutoPostBack="True" SelectedIndex="0"
																			OnSelectedIndexChanged="SortRadioButtonList1_SelectedIndexChanged" RepeatDirection="Horizontal"  Visible="false">
																			<Items>
																				<dx:ListEditItem Text="By Security Questions" Value="1" />
																				<dx:ListEditItem Text="By Email" Value="2" />
																			</Items>
																		</dx:ASPxRadioButtonList></td></tr>
                                <tr>
                                    <td colspan="3" align="left">
                                        <dx:ASPxPanel ID="ForgotPwdPanel" runat="server" Width="100%"  Visible="false">
                                            <PanelCollection>
                                                <dx:PanelContent runat="server" SupportsDisabledAttribute="True">
                                                    <table width="100%" border="0">
                                                        <tr>
                                                           <td width="200px">
                                                               &nbsp;</td>
                                    <td>
                                        &nbsp;</td>
                                                            <td>
                                                                &nbsp;</td>
                                </tr>
								<tr>
								<td colspan="3">
								 <div id="Passwordlabel" runat="server" class="info">Password requires at least one lower case letter, one upper case letter, one digit, 6-13 characters, and no spaces.</div>
								</td>
								</tr>
                                                        <tr>
                                                            <td>
                                                                <dx:ASPxLabel ID="ForgotPwdSecQuestion1Label" runat="server" 
                                                                    CssClass="lblsmallFont" CssFilePath="~/css/vswebforms.css" 
                                                                    Text="Security Question 1:" Width="118px">
                                                                </dx:ASPxLabel>
                                                            </td>
                                                            <td>
                                        <dx:ASPxTextBox ID="ForgotPwdSecQuestion1TextBox" runat="server" 
                                            Width="190px" Enabled="false">
                                        </dx:ASPxTextBox>
                                    </td>
                                                            <td>
                                                                &nbsp;</td>
                                                        </tr>
                               
								
								<tr>
                                    <td>
                                        <dx:ASPxLabel ID="ForgotPwdSecQuestion1AnsLabel" runat="server" 
                                            CssClass="lblsmallFont" CssFilePath="~/css/vswebforms.css" 
                                            Text="Security Question 1 Answer:">
                                        </dx:ASPxLabel>
                                    </td>
                                    <td>
                                        <dx:ASPxTextBox ID="ForgotPwdSecQuestion1AnsTextBox" runat="server" 
                                            Width="190px">
                                        </dx:ASPxTextBox>
                                    </td>
                                    <td>
                                        &nbsp;</td>
                                </tr>
                                <tr>
                                    <td>
                                        <dx:ASPxLabel ID="ForgotPwdSecQuestion2AnsLabel" runat="server" 
                                            CssClass="lblsmallFont" CssFilePath="~/css/vswebforms.css" 
                                            Text="Security Question 2:">
                                        </dx:ASPxLabel>
                                    </td>
                                    <td>
                                        <dx:ASPxTextBox ID="ForgotPwdSecQuestion2TextBox" runat="server" Width="190px" 
                                            Enabled="False">
                                        </dx:ASPxTextBox>
                                    </td>
                                    <td>
                                        &nbsp;</td>
                                </tr>
                                <tr>
                                    <td>
                                        <dx:ASPxLabel ID="ASPxLabel2" runat="server" CssClass="lblsmallFont" 
                                            CssFilePath="~/css/vswebforms.css" Text="Security Question 2 Answer:">
                                        </dx:ASPxLabel>
                                    </td>
                                    <td>
                                        <dx:ASPxTextBox ID="ForgotPwdSecQuestion2AnsTextBox" runat="server" 
                                            Width="190px">
                                        </dx:ASPxTextBox>
                                    </td>
                                    <td>
                                        &nbsp;</td>
                                </tr>
                                <tr>
                                    <td>
                                        <dx:ASPxLabel ID="ForgotPwdNewPasswordLabel" runat="server" 
                                            CssClass="lblsmallFont" CssFilePath="~/css/vswebforms.css" Text="New Password:">
                                        </dx:ASPxLabel>
                                    </td>
                                    <td>
                                        <dx:ASPxTextBox ID="ForgotPwdNewPasswordTextBox" runat="server" Width="190px"  ClientInstanceName="ForgotPwdNewPasswordTextBox"
                                            Password="True">
											<%--	<clientsideevents Validation="function(s, e) {e.isValid = (s.GetText())}" />--%>
											<ValidationSettings ErrorDisplayMode="ImageWithTooltip">
											  <RequiredField ErrorText="Please enter new password." IsRequired="True" />
											</ValidationSettings>
                                            <ValidationSettings ErrorDisplayMode="ImageWithTooltip" >
                                              
                                                <RegularExpression ErrorText="Password requires at least one lower case letter, one upper case letter, one digit, 6-13 characters, and no spaces." ValidationExpression="^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?!.*\s).{6,13}$"></RegularExpression>

<RequiredField IsRequired="True" ErrorText="Please enter new password."></RequiredField>
                                            </ValidationSettings>
                                        </dx:ASPxTextBox>
                                    </td>
                                    <td>&nbsp;
                            </td>
                                </tr>
								<%--<tr><td>           <asp:RequiredFieldValidator id="RequiredFieldValidator2" runat="server"
  ControlToValidate="ForgotPwdNewPasswordTextBox"
  ErrorMessage="Please enter new password."
  ForeColor="Red">
</asp:RequiredFieldValidator></td></tr>--%>

									 
                                <tr>
                                    <td>
                                        <dx:ASPxLabel ID="ForgotPwdConfirmPasswordLabel" runat="server" 
                                            CssClass="lblsmallFont" CssFilePath="~/css/vswebforms.css" 
                                            Text="Confirm Password:">
                                        </dx:ASPxLabel>
                                    </td>
                                    <td>
                                        <dx:ASPxTextBox ID="ForgotPwdConfirmPasswordTextBox" runat="server" 
                                            Width="190px" Password="True"  ClientInstanceName="ForgotPwdConfirmPasswordTextBox">
											<clientsideevents Validation="function(s, e) {e.isValid = (s.GetText() == ForgotPwdNewPasswordTextBox.GetText());}" />
                                           <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorText="Passwords do not match. Please make sure you enter the same password in the new and confirm password fields.">
                                                                   <%-- <RequiredField IsRequired="True" ErrorText="Passwords do not match. Please make sure you enter the same password in the new and confirm password fields." />--%>
																
                                                             </ValidationSettings>
                                        </dx:ASPxTextBox>
                                    </td>
									<td>
									&nbsp;
									</td>
									 </tr>
								
                                                        <tr>
                                                            <td colspan="3">
                                                                &nbsp;</td>
                                                        </tr>
                                                    </table>
                                                </dx:PanelContent>
                                            </PanelCollection>
                                        </dx:ASPxPanel>
                                    </td>
                                </tr>
								<tr>
                <td>
                    <dx:ASPxPanel ID="mailpanel" runat="server"
                        Width="100%" HeaderText="Details" Theme="Glass" ClientVisible="false">
                        <PanelCollection>
<dx:PanelContent ID="PanelContent2" runat="server">
    <table class="navbarTbl">
        <tr>
            <td>
                <dx:ASPxLabel ID="unamedispLbl" runat="server" Visible="false" CssClass="lblsmallFont" 
                    Text="User Name:" Wrap="False">
                </dx:ASPxLabel>
            </td>
            <td>
                <dx:ASPxLabel ID="unameLbl" runat="server" Visible="false" CssClass="lblsmallFont" Wrap="False">
                </dx:ASPxLabel>
            </td>
        </tr>
        <%--<tr>
            <td colspan="2">
            </td>
        </tr>--%>
       <%-- <tr>
            <td>
                <dx:ASPxLabel ID="ASPxLabel3" runat="server" CssClass="lblsmallFont" 
                    Text="Send e-mail notification:">
                </dx:ASPxLabel>
            </td>
            <td>
                <dx:ASPxCheckBox ID="chkSendMail" runat="server" Text="Yes">
                </dx:ASPxCheckBox>
            </td>
        </tr>--%>
    </table>
                            </dx:PanelContent>
</PanelCollection>
                    </dx:ASPxPanel>
                </td>
            </tr>
                                <tr>
                                    <td colspan="3">
                                        <dx:ASPxLabel ID="ErrorMsgLabel" runat="server" ForeColor="#FF3300">
                                        </dx:ASPxLabel>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        &nbsp;</td>
                                </tr>
                                <tr>
                                    <td colspan="3" align="left">
                                        <table>
                                            <tr>
                                                <td>
                                                    <dx:ASPxButton ID="ForgotPwdResetPasswordButton" runat="server" OnClick="ForgotPwdResetPasswordButton_Click" 
                                                        SpriteCssFilePath="~/App_Themes/Office2010Blue/{0}/sprite.css" 
                                                        Text="Reset Password" Wrap="False" CssClass="sysButton">
                                                    </dx:ASPxButton>
                                                </td>
                                                <td>
                                                    <dx:ASPxButton ID="ForgotPwdCancelButton" runat="server" 
                                                        CausesValidation="False" OnClick="ForgotPwdCancelButton_Click" 
                                                        SpriteCssFilePath="~/App_Themes/Office2010Blue/{0}/sprite.css" Text="Cancel" 
                                                        Wrap="False" CssClass="sysButton">
                                                    </dx:ASPxButton>
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
</asp:Content>
