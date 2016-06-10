<%@ Page Title="VitalSigns Plus-AlertSettings" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true"
    CodeBehind="AlertSettings.aspx.cs" Inherits="VSWebUI.AlertSettings" %>

<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>


<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style1
        {
            width: 100%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table class="style1">
        <tr>
            <td>
                <dx:ASPxRoundPanel ID="AlertSettingsRoundPanel1" runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
                    CssPostfix="Glass" GroupBoxCaptionOffsetY="-24px" HeaderText="Alert Settings"
                    SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" Width="600px">
                    <ContentPaddings PaddingBottom="10px" PaddingLeft="4px" PaddingTop="10px" />
                    <HeaderStyle Height="23px">
                        <Paddings PaddingBottom="0px" PaddingLeft="2px" PaddingTop="0px" />
                    </HeaderStyle>
                    <PanelCollection>
                        <dx:PanelContent runat="server" SupportsDisabledAttribute="True">
                            <table class="style1">
                                <tr>
                                    <td>
                                        <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
                                            CssPostfix="Glass" GroupBoxCaptionOffsetY="-24px" SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css"
                                            Width="400px" HeaderText="Alert Options">
                                            <ContentPaddings PaddingBottom="10px" PaddingLeft="4px" PaddingTop="10px" />
                                            <HeaderStyle Height="23px">
                                                <Paddings PaddingBottom="0px" PaddingLeft="2px" PaddingTop="0px" />
                                            </HeaderStyle>
                                            <PanelCollection>
                                                <dx:PanelContent runat="server" SupportsDisabledAttribute="True">
                                                    <table class="style1">
                                                        <tr>
                                                            <td colspan="3">
                                                                <dx:ASPxButton ID="AlertButton" runat="server" CssFilePath="~/App_Themes/Office2010Blue/{0}/styles.css"
                                                                    CssPostfix="Office2010Blue" SpriteCssFilePath="~/App_Themes/Office2010Blue/{0}/sprite.css"
                                                                    Text="Alerts ON" OnClick="AlertButton_Click" Theme="Default">
                                                                </dx:ASPxButton>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <dx:ASPxCheckBox ID="RepeatCheckBox" runat="server" CheckState="Unchecked" CssClass="lblsmallFont"
                                                                    Text="Repeat Down Server alerts every " AutoPostBack="True" OnCheckedChanged="RepeatCheckBox_CheckedChanged">
                                                                </dx:ASPxCheckBox>
                                                            </td>
                                                            <td>
                                                                <dx:ASPxTextBox ID="MinutesTextBox" runat="server" Width="70px" AutoPostBack="True"
                                                                    OnTextChanged="MinutesTextBox_TextChanged">
                                                                    <MaskSettings Mask="&lt;0..9999&gt;" />
                                                                </dx:ASPxTextBox>
                                                            </td>
                                                            <td>
                                                                <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="Minutes" CssClass="lblsmallFont">
                                                                </dx:ASPxLabel>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </dx:PanelContent>
                                            </PanelCollection>
                                        </dx:ASPxRoundPanel>
                                    </td>
                                    <td>
                                        <dx:ASPxRoundPanel ID="ASPxRoundPanel2" runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
                                            CssPostfix="Glass" GroupBoxCaptionOffsetY="-24px" SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css"
                                            Width="300px" HeaderText="Alert Routing">
                                            <ContentPaddings PaddingBottom="10px" PaddingLeft="4px" PaddingTop="10px" />
                                            <HeaderStyle Height="23px">
                                                <Paddings PaddingBottom="0px" PaddingLeft="2px" PaddingTop="0px" />
                                            </HeaderStyle>
                                            <PanelCollection>
                                                <dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">
                                                    <table class="style1">
                                                        <tr>
                                                            <td>
                                                                <dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="Primary Server" CssClass="lblsmallFont">
                                                                </dx:ASPxLabel>
                                                            </td>
                                                            <td>
                                                                <dx:ASPxComboBox ID="PrimaryComboBox" runat="server" Theme="Office2010Blue">
                                                                </dx:ASPxComboBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <dx:ASPxLabel ID="ASPxLabel3" runat="server" Text="Backup Server" CssClass="lblsmallFont">
                                                                </dx:ASPxLabel>
                                                            </td>
                                                            <td>
                                                                <dx:ASPxComboBox ID="BackupComboBox" runat="server" Theme="Office2010Blue">
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
                                    <td colspan="2">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <table>
                                            <tr>
                                                <td>
                                                    <dx:ASPxButton ID="OKButton" runat="server" CssFilePath="~/App_Themes/Office2010Blue/{0}/styles.css"
                                                        CssPostfix="Office2010Blue" Font-Bold="False" OnClick="OKButton_Click" SpriteCssFilePath="~/App_Themes/Office2010Blue/{0}/sprite.css"
                                                        Text="OK" Width="75px">
                                                    </dx:ASPxButton>
                                                </td>
                                                <td>
                                                    <dx:ASPxButton ID="FormCancelButton" runat="server" CssFilePath="~/App_Themes/Office2010Blue/{0}/styles.css"
                                                        CssPostfix="Office2010Blue" Font-Bold="False" OnClick="FormCancelButton_Click"
                                                        SpriteCssFilePath="~/App_Themes/Office2010Blue/{0}/sprite.css" Text="Cancel"
                                                        Width="75px">
                                                    </dx:ASPxButton>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td>
                                        <dx:ASPxPopupControl ID="MsgPopupControl" runat="server" HeaderText="Information"
                                            Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
                                            Theme="Glass" Width="300px">
                                            <ContentCollection>
                                                <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server" SupportsDisabledAttribute="True">
                                                    <table class="style1">
                                                        <tr>
                                                            <td>
                                                                <dx:ASPxLabel ID="MsgLabel" runat="server" Text="ASPxLabel">
                                                                </dx:ASPxLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <dx:ASPxButton ID="MsgButton" runat="server" OnClick="MsgButton_Click" Text="OK"
                                                                    Theme="Office2010Blue">
                                                                </dx:ASPxButton>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </dx:PopupControlContentControl>
                                            </ContentCollection>
                                        </dx:ASPxPopupControl>
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
</asp:Content>
