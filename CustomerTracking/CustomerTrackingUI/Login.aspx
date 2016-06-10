<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="CustomerTracking.Login" %>
<%@ Register assembly="DevExpress.Web.v13.2, Version=13.2.9.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxRoundPanel" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v13.2, Version=13.2.9.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxPanel" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v13.2, Version=13.2.9.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxEditors" tagprefix="dx" %>

<%@ Register assembly="DevExpress.Web.v13.2, Version=13.2.9.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxMenu" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v13.2, Version=13.2.9.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxNavBar" tagprefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <dx:aspxroundpanel ID="ASPxRoundPanel1" runat="server" 
                CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass" 
                GroupBoxCaptionOffsetY="-24px" HeaderText="User Login" 
                SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" 
        Width="316px">
                <ContentPaddings PaddingBottom="10px" PaddingLeft="4px" PaddingTop="10px" />
                <HeaderStyle Height="23px">
                <Paddings PaddingBottom="0px" PaddingLeft="2px" PaddingTop="0px" />
                </HeaderStyle>
                <PanelCollection>
<dx:PanelContent runat="server" SupportsDisabledAttribute="True">
    <table style="width:100%;">
        <tr>
            <td colspan="2">
                <div id="infoDiv" class="info">Enter your Login name and Password below or use the 'Dashboard Only'
                button to access the site without authenticating.
                </div>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <dx:ASPxLabel ID="ASPxLabel1" runat="server" CssClass="lblsmallFont" 
                    Font-Bold="False" Text="Enter your Login name and Password." 
                    Visible="False">
                </dx:ASPxLabel>
            </td>
        </tr>
        <tr>
            <td>
                <dx:ASPxLabel ID="ASPxLabel2" runat="server" CssClass="lblsmallFont" 
                    Text="Login name: ">
                </dx:ASPxLabel>
            </td>
            <td>
                <dx:ASPxTextBox ID="LoginTextBox" runat="server" 
                    NullText="Enter your Login Name" Width="170px">
                    <ValidationSettings CausesValidation="True" Display="Dynamic" 
                        ErrorDisplayMode="ImageWithTooltip" SetFocusOnError="True">
                        <RequiredField ErrorText="Enter your Login Name" IsRequired="True" />
                    </ValidationSettings>
                </dx:ASPxTextBox>
            </td>
        </tr>
        <tr>
            <td>
                <dx:ASPxLabel ID="ASPxLabel3" runat="server" CssClass="lblsmallFont" 
                    Text="Password: ">
                </dx:ASPxLabel>
            </td>
            <td style="margin-left: 40px">
                <dx:ASPxTextBox ID="PasswordTextBox" runat="server" NullText="Enter your Password" 
                    Password="True" Width="170px">
                    <ValidationSettings CausesValidation="True" ErrorDisplayMode="ImageWithTooltip">
                        <RequiredField ErrorText="Enter your Password" />
                    </ValidationSettings>
                </dx:ASPxTextBox>
            </td>
        </tr>

        <tr>
            <td>
                &nbsp;</td>
            <td>
                <dx:ASPxCheckBox ID="RememberCheckBox" runat="server" CheckState="Unchecked" 
                    Text="Remember me">
                </dx:ASPxCheckBox>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <dx:ASPxLabel ID="ErrorLabel" runat="server" ForeColor="Red">
                </dx:ASPxLabel>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <dx:ASPxCaptcha ID="ValidateCaptcha" runat="server" CssClass="lblsmallfont" 
                    CssFilePath="~/App_Themes/PlasticBlue/{0}/styles.css" CssPostfix="PlasticBlue" 
                    Font-Overline="False" 
                    SpriteCssFilePath="~/App_Themes/PlasticBlue/{0}/sprite.css" Visible="False">
                    <LoadingPanelImage Url="~/App_Themes/PlasticBlue/Editors/Loading.gif">
                    </LoadingPanelImage>
                    <TextBoxStyle CssClass="lblsmallfont" Width="80px" />
                    <ValidationSettings>
                        <RequiredField ErrorText="Enter code" IsRequired="True" />
                    </ValidationSettings>
                    <ChallengeImage BackgroundColor="WhiteSmoke" BorderColor="#CCCCCC" 
                        ForegroundColor="#485C9F" Height="40" Width="130">
                    </ChallengeImage>
                </dx:ASPxCaptcha>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <table style="width:100%;">
                    <tr>
                        <td align="right">
                            <dx:ASPxButton ID="LoginButton" runat="server" 
                                CssFilePath="~/App_Themes/Office2010Blue/{0}/styles.css" 
                                CssPostfix="Office2010Blue" OnClick="LoginButton_Click" 
                                SpriteCssFilePath="~/App_Themes/Office2010Blue/{0}/sprite.css" Text="Login">
                            </dx:ASPxButton>
                        </td>
                        <td align="left">
                            <dx:ASPxButton ID="ResetButton" runat="server" CausesValidation="False" 
                                CssFilePath="~/App_Themes/Office2010Blue/{0}/styles.css" 
                                CssPostfix="Office2010Blue" OnClick="ResetButton_Click" 
                                SpriteCssFilePath="~/App_Themes/Office2010Blue/{0}/sprite.css" 
                                Text="Reset" AutoPostBack="False">
                            </dx:ASPxButton>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
                <tr>
            <td>
                &nbsp;</td>
            <td>
                <asp:LinkButton ID="ForgotPwdLink" runat = "server" Text="Forgot Password"
                    OnClick="ForgotPwdLink_Click"></asp:LinkButton>
                <%--<dx:ASPxHyperLink ID="ForgotPwdHyperLink" runat="server" Cursor="pointer" 
                    Text="Forgot Password" NavigateUrl="~/Security/ForgotPassword.aspx">
                </dx:ASPxHyperLink>--%>
                    </td>
        </tr>
    </table>
    <table width="100%">
        <tr>
            <td>
               <hr /></td>
        </tr>
        <tr>
            <td align="center">
                <dx:ASPxButton ID="DashOnlyButton" runat="server" Text="Dashboard Only" 
                    Theme="Office2010Blue" Wrap="False" OnClick="DashOnlyButton_Click" 
                    CausesValidation="False">
                </dx:ASPxButton>
            </td>
        </tr>
    </table>
                    </dx:PanelContent>
</PanelCollection>
            </dx:aspxroundpanel>
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


