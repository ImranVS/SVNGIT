<%@ Page Title="VitalSigns Plus-PwrShellEdit" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="PwrShellEdit.aspx.cs" Inherits="VSWebUI.WebForm17" %>
<%@ Register assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <dx:ASPxRoundPanel ID="RoundPanelPwrShel" runat="server" 
        HeaderText="Power Shell Script " Theme="Glass" Width="100%">
        <PanelCollection>
            <dx:PanelContent runat="server" SupportsDisabledAttribute="True">
                <table width="100%">
                <tr>
                <td width="100px">
                <dx:ASPxLabel ID="AspxLabelcategory" runat="server" Text="Category:" 
                        CssClass="lblsmallFont">
                </dx:ASPxLabel>
                </td>
                <td>
                <dx:ASPxComboBox ID="category" runat="server" AutoPostBack="True" TextFormatString="{0}({1})" IncrementalFilteringMode="Contains" ValueType="System.String" DropDownStyle="DropDown" 
                        OnSelectedIndexChanged="category_SelectedIndexChanged" ></dx:ASPxComboBox>
                </td>
                </tr>
                <tr>
                <td>
                <dx:ASPxLabel ID="AspxlabelDescripation" runat="server" Text="Description:" 
                        Width="60%" CssClass="lblsmallFont"></dx:ASPxLabel>
                </td>
                <td>
                <dx:ASPxMemo ID="txtDescripation" Height="71px" Width="80%" runat="server"></dx:ASPxMemo>
                </td>
                
                

                
                
                </tr>

                    <tr>
                        <td>
                            <dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="Script  Name:" 
                                CssClass="lblsmallFont">
                            </dx:ASPxLabel>
                        </td>
                        <td>
                            <dx:ASPxTextBox ID="txtScriptName" runat="server" Width="80%">
                            </dx:ASPxTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <dx:ASPxLabel ID="ASPxLabel1" runat="server" Height="16px" 
                                Text="Script Details:" CssClass="lblsmallFont">
                            </dx:ASPxLabel>
                        </td>
                        <td>
                            <dx:ASPxMemo ID="ScriptDetailsMemo" runat="server" Height="250px" Width="80%">
                            </dx:ASPxMemo>
                        </td>
                    </tr>
                    <tr>
                    <td></td>
                        <td >
                            <table>
                                <tr>
                                    <td>
                                        <dx:ASPxButton ID="OKButton" runat="server" OnClick="OKButton_Click" Text="OK" 
                                            Theme="Office2010Blue" Width="80px">
                                        </dx:ASPxButton>
                                    </td>
                                    <td>
                                        <dx:ASPxButton ID="CancelButton" runat="server" Text="Cancel" 
                                            Theme="Office2010Blue" Width="80px" OnClick="CancelButton_Click">
                                        </dx:ASPxButton>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                        <dx:ASPxPopupControl ID="ErrorMessagePopupControl" runat="server" 
                        AllowDragging="True" ClientInstanceName="pcErrorMessage" 
                        CloseAction="CloseButton" CssFilePath="~/App_Themes/Glass/{0}/styles.css" 
                        CssPostfix="Glass" EnableAnimation="False" EnableViewState="False" 
                        HeaderText="Validation Failure" Height="150px" Modal="True" 
                        PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" 
                        SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" Width="300px">
                        <LoadingPanelImage Url="~/App_Themes/Glass/Web/Loading.gif">
                        </LoadingPanelImage>
                        <HeaderStyle>
                        <Paddings PaddingLeft="10px" PaddingRight="6px" PaddingTop="1px" />
<Paddings PaddingLeft="10px" PaddingTop="1px" PaddingRight="6px"></Paddings>
                        </HeaderStyle>
                        <ContentCollection>
                            <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                                <dx:ASPxPanel ID="Panel1" runat="server" DefaultButton="btOK">
                                    <PanelCollection>
                                        <dx:PanelContent ID="PanelContent5" runat="server">
                                            <div style="min-height: 70px;">
                                                <dx:ASPxLabel ID="ErrorMessageLabel" runat="server">
                                                </dx:ASPxLabel>
                                                &nbsp;</div>
                                            <div>
                                                <table cellpadding="0" cellspacing="0" width="100%">
                                                    <tr>
                                                        <td align="center">
                                                            <dx:ASPxButton ID="ValidationOkButton" runat="server" AutoPostBack="False" 
                                                                CssFilePath="~/App_Themes/Office2010Blue/{0}/styles.css" 
                                                                CssPostfix="Office2010Blue" 
                                                                SpriteCssFilePath="~/App_Themes/Office2010Blue/{0}/sprite.css" Text="Ok" 
                                                                Width="80px">
                                                                <ClientSideEvents Click="function(s, e) { pcErrorMessage.Hide(); }" />
<ClientSideEvents Click="function(s, e) { pcErrorMessage.Hide(); }"></ClientSideEvents>
                                                            </dx:ASPxButton>
                                                            <dx:ASPxButton ID="ValidationUpdatedButton" runat="server" AutoPostBack="False" 
                                                                CssFilePath="~/App_Themes/Office2010Blue/{0}/styles.css" 
                                                                CssPostfix="Office2010Blue"  
                                                                SpriteCssFilePath="~/App_Themes/Office2010Blue/{0}/sprite.css" Text="Ok" 
                                                                Visible="False" Width="80px" OnClick="ValidationUpdatedButton_Click">
                                                                <ClientSideEvents Click="function(s, e) { pcErrorMessage.Hide(); }" />
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
                </table>
            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxRoundPanel>
</asp:Content>
