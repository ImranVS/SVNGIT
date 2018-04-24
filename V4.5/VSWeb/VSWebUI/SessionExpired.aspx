<%@ Page Title="VitalSigns Plus - Session Expired" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="SessionExpired.aspx.cs" Inherits="VSWebUI.SessionExpired" %>
<%@ Register assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <br />
<br />
<br />
<br />
<br />
    &nbsp;&nbsp;&nbsp;
    <br />
    <br />
    <br />
<br />
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
            <dx:aspxroundpanel ID="ASPxRoundPanel1" runat="server" 
                CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass" 
                GroupBoxCaptionOffsetY="-24px" HeaderText="Session Expired" 
                SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" Width="316px">
                <ContentPaddings PaddingBottom="10px" PaddingLeft="4px" PaddingTop="10px" />
                <HeaderStyle Height="23px">
                <Paddings PaddingBottom="0px" PaddingLeft="2px" PaddingTop="0px" />
                </HeaderStyle>
                <PanelCollection>
<dx:PanelContent runat="server" SupportsDisabledAttribute="True">
    <table style="width:100%;">
        <tr>
            <td colspan="2">
                <dx:ASPxLabel ID="ASPxLabel1" runat="server" CssClass="lblsmallFont" 
                    Text=" Your session has expired due to inactivity.">
                </dx:ASPxLabel>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                <table style="width:100%;">
                    <tr>
                        <td align="right">
                        </td>
                        <td align="left">
                            <dx:ASPxButton ID="LoginButton" runat="server" 
                                CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass" 
                                OnClick="LoginButton_Click" 
                                SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" 
                                Text="Click here to login again" Width="200px">
                            </dx:ASPxButton>
                        </td>
                        <td>
                            &nbsp;</td>
                    </tr>
                </table>
                 
            </td>
            <td style="margin-left: 40px">
             
            </td>
        </tr>

        <tr>
            <td colspan="2">
               
            </td>
        </tr>
        <tr>
            <td colspan="2">
                &nbsp;</td>
        </tr>
                <tr>
            <td>
                &nbsp;</td>
            <td>
                
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
<br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
<br />
<br />
</asp:Content>
