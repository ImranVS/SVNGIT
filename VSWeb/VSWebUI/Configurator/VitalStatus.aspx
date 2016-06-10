<%@ Page Title="VitalSigns Plus" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true"
    CodeBehind="VitalStatus.aspx.cs" Inherits="VSWebUI.WebForm26" %>

<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>



<%@ Register assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx1" %>




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
    <style type="text/css">
        .style1
        {
            text-align: justify;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table width="60%">
        <tr>
            <td>
                <asp:Label ID="Label2" runat="server" Text="VitalStatus Database" 
                                                    Font-Bold="True" Font-Size="Large" style="color: #000000; font-family: Verdana"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <div class="info">The <b>VitalStatus Database</b> is a database used to share status information of all monitored servers with the stakeholders. <br />The Database needs to be created on the server using the supplied template.
                           </div>
                <div id="successDiv" runat="server" class="alert alert-success" style="display: none">Success
                </div>
                </td>
        </tr>
        <tr>
            <td>
                                        <dx:ASPxRoundPanel runat="server" 
                    GroupBoxCaptionOffsetY="-24px" HeaderText="Notes Database Output" Width="100%" 
                    SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" CssPostfix="Glass" 
                    CssFilePath="~/App_Themes/Glass/{0}/styles.css" ID="VitalStatusRoundPanel">
<ContentPaddings PaddingLeft="4px" PaddingTop="10px" PaddingBottom="10px"></ContentPaddings>

<HeaderStyle Height="23px">
<Paddings PaddingLeft="2px" PaddingTop="0px" PaddingBottom="0px"></Paddings>
</HeaderStyle>
<PanelCollection>
<dx:PanelContent runat="server" SupportsDisabledAttribute="True">
                                                    <table class="style1">
                                                        <tr>
                                                            <td colspan="2">
                                                                <dx:ASPxCheckBox runat="server" CheckState="Unchecked" 
                                                                    Text="Connect to the VitalStatus database" AutoPostBack="True" 
                                                                    CssClass="lblsmallFont" ID="CnctDBCheckBox" 
                                                                    OnCheckedChanged="CnctDBCheckBox_CheckedChanged"></dx:ASPxCheckBox>

                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <dx:ASPxLabel runat="server" Text="Server:" CssClass="lblsmallFont" 
                                                                    ID="ASPxLabel1"></dx:ASPxLabel>

                                                            </td>
                                                            <td>
                                                                <dx:ASPxComboBox runat="server" Spacing="0" AutoPostBack="True" 
                                                                    SpriteCssFilePath="~/App_Themes/Office2010Blue/{0}/sprite.css" 
                                                                    ID="DServerComboBox" 
                                                                    OnSelectedIndexChanged="DServerComboBox_SelectedIndexChanged">
<LoadingPanelImage Url="~/App_Themes/Office2010Blue/Editors/Loading.gif"></LoadingPanelImage>

<LoadingPanelStyle ImageSpacing="5px"></LoadingPanelStyle>

<ButtonStyle Width="13px"></ButtonStyle>
</dx:ASPxComboBox>

                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <dx:ASPxLabel runat="server" Text="Database:" Height="16px" 
                                                                    CssClass="lblsmallFont" ID="ASPxLabel2"></dx:ASPxLabel>

                                                            </td>
                                                            <td>
                                                                <dx:ASPxTextBox runat="server" Width="170px" AutoPostBack="True" ID="DBTextBox" 
                                                                    OnTextChanged="DBTextBox_TextChanged"></dx:ASPxTextBox>

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
                <div id="errorDiv" runat="server" class="alert alert-danger" style="display: none">Error
                </div>                            
            </td>
        </tr>
        <tr>
            <td>
                                        
                                        <dx:ASPxPopupControl runat="server" 
                    PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" 
                    SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" Modal="True" 
                    CloseAction="CloseButton" AllowDragging="True" 
                    ClientInstanceName="pcErrorMessage" EnableAnimation="False" 
                    HeaderText="Validation Failure" CssPostfix="Glass" 
                    CssFilePath="~/App_Themes/Glass/{0}/styles.css" Width="300px" Height="150px" 
                    ID="ErrorMessagePopupControl" EnableViewState="False">
<LoadingPanelImage Url="~/App_Themes/Glass/Web/Loading.gif"></LoadingPanelImage>

<HeaderStyle>
<Paddings PaddingLeft="10px" PaddingTop="1px" PaddingRight="6px"></Paddings>
</HeaderStyle>
<ContentCollection>
<dx:PopupControlContentControl runat="server" SupportsDisabledAttribute="True">
                                                    <dx:ASPxPanel runat="server" DefaultButton="btOK" ID="Panel1"><PanelCollection>
<dx:PanelContent runat="server" SupportsDisabledAttribute="True">
                                                                <div style="min-height: 70px;">
                                                                    <dx:ASPxLabel runat="server" ID="ErrorMessageLabel"></dx:ASPxLabel>

                                                                </div>
                                                                <div>
                                                                    <table cellpadding="0" cellspacing="0" width="100%">
                                                                        <tr>
                                                                            <td align="center">
                                                                                <dx:ASPxButton runat="server" AutoPostBack="False" 
                                                                                    SpriteCssFilePath="~/App_Themes/Office2010Blue/{0}/sprite.css" Text="OK" 
                                                                                    CssPostfix="Office2010Blue" 
                                                                                    CssFilePath="~/App_Themes/Office2010Blue/{0}/styles.css" Width="80px" 
                                                                                    ID="ValidationOkButton">
<ClientSideEvents Click="function(s, e) { pcErrorMessage.Hide(); }"></ClientSideEvents>
</dx:ASPxButton>

                                                                                <dx:ASPxButton runat="server" AutoPostBack="False" 
                                                                                    SpriteCssFilePath="~/App_Themes/Office2010Blue/{0}/sprite.css" Text="OK" 
                                                                                    CssPostfix="Office2010Blue" 
                                                                                    CssFilePath="~/App_Themes/Office2010Blue/{0}/styles.css" Width="80px" 
                                                                                    ID="ValidationUpdatedButton" Visible="False" 
                                                                                    OnClick="ValidationUpdatedButton_Click">
<ClientSideEvents Click="function(s, e) { pcErrorMessage.Hide(); }"></ClientSideEvents>
</dx:ASPxButton>

                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </div>
                                                            </dx:PanelContent>
</PanelCollection>
</dx:ASPxPanel>

                                                </dx:PopupControlContentControl>
</ContentCollection>
</dx:ASPxPopupControl>

                                        
                                    </td>
        </tr>
        <tr>
            <td>
                <table>
                                            <tr>
                                                <td align="right">
                                                    <dx:ASPxButton ID="ApplyButton" runat="server" CssFilePath="~/App_Themes/Office2010Blue/{0}/styles.css"
                                                        CssPostfix="Office2010Blue" SpriteCssFilePath="~/App_Themes/Office2010Blue/{0}/sprite.css"
                                                        Text="Apply" OnClick="ApplyButton_Click" AutoPostBack="False">
                                                    </dx:ASPxButton>
                                                </td>
                                                <td>
                                                    <dx:ASPxButton ID="CancelButton" runat="server" CssFilePath="~/App_Themes/Office2010Blue/{0}/styles.css"
                                                        CssPostfix="Office2010Blue" SpriteCssFilePath="~/App_Themes/Office2010Blue/{0}/sprite.css"
                                                        Text="Cancel" OnClick="CancelButton_Click">
                                                    </dx:ASPxButton>
                                                </td>
                                            </tr>
                                        </table>
            </td>
        </tr>
    </table>
</asp:Content>
