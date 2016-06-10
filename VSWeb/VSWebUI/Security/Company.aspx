<%@ Page Title="VitalSigns Plus - Company Settings" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Company.aspx.cs" Inherits="VSWebUI.Security.Company" %>
<%@ MasterType virtualpath="~/Site1.Master" %>
<%@ Register assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <br />
    <br />
<br />
<table style="width:100%;">
    <tr>
        <td width="15%">
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
                GroupBoxCaptionOffsetY="-24px" HeaderText="Company Master" 
                SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" Width="316px">
                <ContentPaddings PaddingBottom="10px" PaddingLeft="4px" PaddingTop="10px" />
<ContentPaddings PaddingLeft="4px" PaddingTop="10px" PaddingBottom="10px"></ContentPaddings>

                <HeaderStyle Height="23px">
                <Paddings PaddingBottom="0px" PaddingLeft="2px" PaddingTop="0px" />
<Paddings PaddingLeft="2px" PaddingTop="0px" PaddingBottom="0px"></Paddings>
                </HeaderStyle>
                <PanelCollection>
<dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">
    <table style="width:100%;">
        <tr>
            <td colspan="3">
                &nbsp;</td>
        </tr>
        <tr>
            <td colspan="3">
                <dx:ASPxLabel ID="ASPxLabel3" runat="server" CssClass="lblsmallFont" 
                    Text="(Select PNG image, size- width:250px, height:45px)">
                </dx:ASPxLabel>
            </td>
        </tr>
        <tr>
            <td>
                <dx:ASPxLabel ID="ASPxLabel2" runat="server" CssClass="lblsmallFont" 
                    Text="Logo">
                </dx:ASPxLabel>
                 
            </td>
            <td>
                <dx:ASPxUploadControl ID="FileUploadControl" runat="server" Width="280px">
                </dx:ASPxUploadControl>
            </td>
            <td>
             
            </td>
        </tr>
        <tr>
            <td class="style1">
                
            </td>
            <td class="style1">
                <dx:ASPxHyperLink ID="FilePathHyperLink" runat="server" Target="_blank" 
                    NavigateUrl="~/images/hsbc.png" Text="Company Logo">
                </dx:ASPxHyperLink>
            </td>
            <td style="margin-left: 40px" class="style1">
            
            </td>
        </tr>

        <tr>
            <td class="style1">
                &nbsp;</td>
            <td class="style1">
                &nbsp;</td>
            <td class="style1" style="margin-left: 40px">
                &nbsp;</td>
        </tr>

        <tr>
            <td colspan="3">
               
                <dx:ASPxLabel ID="ErrorLabel" runat="server" ForeColor="Red">
                </dx:ASPxLabel>
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <table style="width:100%;">
                    <tr>
                        <td align="right">
                            
                            <dx:ASPxButton ID="OkButton" runat="server" 
                                CssFilePath="~/App_Themes/Office2010Blue/{0}/styles.css" 
                                CssPostfix="Office2010Blue" OnClick="LoginButton_Click" 
                                SpriteCssFilePath="~/App_Themes/Office2010Blue/{0}/sprite.css" Text="Ok">
                            </dx:ASPxButton>
                            
                        </td>
                        <td align="left">
                            
                            <dx:ASPxButton ID="CancelButton" runat="server" 
                                CssFilePath="~/App_Themes/Office2010Blue/{0}/styles.css" 
                                CssPostfix="Office2010Blue" 
                                SpriteCssFilePath="~/App_Themes/Office2010Blue/{0}/sprite.css" 
                                Text="Cancel" CausesValidation="False">
                            </dx:ASPxButton>
                            
                        </td>
                        <td>
                            &nbsp;</td>
                    </tr>
                </table>
            </td>
        </tr>
                <tr>
            <td>
                &nbsp;</td>
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
