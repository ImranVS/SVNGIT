﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="VersionInfoEdit.aspx.cs" Inherits="CustomerTracking.VersionInfoEdit" %>
<%@ Register Assembly="DevExpress.Web.v13.2, Version=13.2.9.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>
<%@ Register assembly="DevExpress.Web.v13.2, Version=13.2.9.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxTabControl" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v13.2, Version=13.2.9.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxClasses" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v13.2, Version=13.2.9.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxGridView" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v13.2, Version=13.2.9.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxEditors" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v13.2, Version=13.2.9.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxRoundPanel" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v13.2, Version=13.2.9.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxPanel" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v13.2, Version=13.2.9.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxMenu" tagprefix="dx" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<dx:ASPxRoundPanel ID="CustomerTrackingRoundPanel" runat="server" 
                                        CssFilePath="~/App_Themes/Glass/{0}/styles.css" 
                                        CssPostfix="Glass" 
                                        GroupBoxCaptionOffsetY="-24px" HeaderText="Customer Tracking Information" 
                                        SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" 
                                        Width="100%" Height="250px">
                                        <ContentPaddings PaddingBottom="10px" PaddingTop="10px" PaddingLeft="4px" />
<ContentPaddings PaddingLeft="4px" PaddingTop="10px" PaddingBottom="10px"></ContentPaddings>

                                        <HeaderStyle Height="23px">
                                        <Paddings PaddingBottom="0px" PaddingLeft="2px" PaddingTop="0px" />
<Paddings PaddingLeft="2px" PaddingTop="0px" PaddingBottom="0px"></Paddings>
                                        </HeaderStyle>
                                        <PanelCollection>
                                            <dx:PanelContent ID="PanelContent0" runat="server" SupportsDisabledAttribute="True">
        <dx:ASPxPageControl ID="ASPxPageControl1" runat="server" ActiveTabIndex="1"
            Width="100%" CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass"
            SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" TabSpacing="0px" 
            Font-Bold="True">
            <TabPages>                
                <dx:TabPage Text="Version Info">               
                    <ContentCollection>
                        <dx:ContentControl ID="ContentControl2" runat="server" SupportsDisabledAttribute="True">
                            
                            <table class="style16">
                                <tr>
                                    <td>
                                        <dx:ASPxRoundPanel ID="ASPxRoundPanel4" runat="server" 
                                            CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass" 
                                            GroupBoxCaptionOffsetY="-24px" HeaderText="Contacts" Height="104px" 
                                            SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" Width="100%">
                                            <ContentPaddings Padding="2px" PaddingBottom="10px" PaddingLeft="4px" 
                                                PaddingTop="10px" />
<ContentPaddings Padding="2px" PaddingLeft="4px" PaddingTop="10px" PaddingBottom="10px"></ContentPaddings>

                                            <HeaderStyle Height="23px">
                                            <Paddings Padding="0px" PaddingBottom="7px" PaddingLeft="2px" 
                                                PaddingRight="2px" PaddingTop="0px" />
<Paddings Padding="0px" PaddingLeft="2px" PaddingTop="0px" PaddingRight="2px" PaddingBottom="7px"></Paddings>
                                            </HeaderStyle>
                                            <PanelCollection>
                                                <dx:PanelContent ID="PanelContent4" runat="server" SupportsDisabledAttribute="True">
                                                    <table>
                                                      <tr>
                <td class="style1">
               
                <dx:ASPxLabel ID="CustomerName" runat="server" Text="Customer Name"
                 CssClass="lblsmallFont">
                </dx:ASPxLabel>
                </td>
                <td>
                    <dx:ASPxComboBox ID="CustomerComboBox" runat="server">
                      <ValidationSettings CausesValidation="True" Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                        <RequiredField IsRequired="True" ErrorText="Select Customer"></RequiredField>
                        <ErrorImage ToolTip="Select Customer"></ErrorImage>
                        <RequiredField ErrorText="Select Customer" IsRequired="True" />
                      </ValidationSettings>
                    </dx:ASPxComboBox>
                    </td>
                    </tr>                                                          
                                                <tr>
                                                 <td>
                                               
                                                    <dx:ASPxLabel ID="ASPxLabel13" runat="server" Text="Version Number:" CssClass="lblsmallFont">
                                                    </dx:ASPxLabel>
                                                </td>
                                                <td>
                                                    <dx:ASPxTextBox ID="VersionNumberTextbox" runat="server" Width="170px">
                                                        <ValidationSettings>
                                                            <RequiredField IsRequired="True" />
                                                            <RequiredField IsRequired="True"></RequiredField>
                                                        </ValidationSettings>
                                                    </dx:ASPxTextBox>
                                                </td>
                                                
                                                <td>
                                               
                                                    <dx:ASPxLabel ID="ASPxLabel14" runat="server" Text="Install Date:" CssClass="lblsmallFont">
                                                    </dx:ASPxLabel>
                                                </td>
                                                <td>
                                                    <dx:ASPxDateEdit ID="InstallDateEdit" runat="server" Width="170px">
                                                    <ValidationSettings>
                                                            <RequiredField IsRequired="True" />
                                                            <RequiredField IsRequired="True"></RequiredField>
                                                        </ValidationSettings>
                                                    </dx:ASPxDateEdit>
                                                    <%--<dx:ASPxTextBox ID="InstallDateTextbox" runat="server" Width="170px">
                                                        <ValidationSettings>
                                                            <RequiredField IsRequired="True" />
                                                            <RequiredField IsRequired="True"></RequiredField>
                                                        </ValidationSettings>
                                                    </dx:ASPxTextBox>--%>
                                                </td>
                                                        
                                                        <td>
                                               
                                                    <dx:ASPxLabel ID="ASPxLabel22" runat="server" Text="Details:" CssClass="lblsmallFont">
                                                    </dx:ASPxLabel>
                                                </td>
                                                <td>
                                                    <dx:ASPxMemo ID="VersionDetailsTextbox" runat="server" Height="71px" Width="170px">
                                                    </dx:ASPxMemo>
                                                    <%--<dx:ASPxTextBox ID="VersionDetailsTextbox" runat="server" Width="170px">
                                                        <ValidationSettings>
                                                            <RequiredField IsRequired="True" />
                                                            <RequiredField IsRequired="True"></RequiredField>
                                                        </ValidationSettings>
                                                    </dx:ASPxTextBox>--%>
                                                </td>
                                                        </tr>
                                                        
                                                        <tr>
                                                        <td colspan = "2">
                                                        <div id="successDiv" runat="server" class="success" style="display: none">Contact Details were updated successfully.
                                                        </div>
                                                        <div id="errorDiv1" class="error" runat="server" style="display: none">Contact Details were not updated.
                                                        </div>
                                                        </td>
                                                        </tr>
                                                          
                                                    </table>

                                                </dx:PanelContent>
                                            </PanelCollection>
                                        </dx:ASPxRoundPanel>
                                    </td>
                                    <td>
                                        &nbsp;</td>
                                    <td>
                                        &nbsp;</td>
                                </tr>
                            </table>
                        </dx:ContentControl>
                    </ContentCollection>
                </dx:TabPage>   
            </TabPages>
            <LoadingPanelImage Url="~/App_Themes/Glass/Web/Loading.gif">
            </LoadingPanelImage>
            <Paddings PaddingLeft="0px" />

<Paddings PaddingLeft="0px"></Paddings>

            <ContentStyle>
                <Border BorderColor="#4986A2" />
                <Border BorderColor="#002D96" BorderStyle="Solid" BorderWidth="1px"></Border>
            </ContentStyle>
        </dx:ASPxPageControl>
        <div id="errorDiv" class="error" runat="server" style="display: none">Error.
        </div>
        <table>
            <tr>
                <td>
                    &nbsp;
                </td>
                <caption>
                    <dx:ASPxPopupControl ID="ErrorMessagePopupControl" runat="server"
                    AllowDragging="true" ClientInstanceName="pcErrorMessage"
                    CloseAction="CloseButton" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
                    CssPostfix="Glass" EnableAnimation="False" EnableViewState="False" 
                    HeaderText="Validation Failure" Height="150px" Modal="True" 
                    PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" 
                    SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" Width="300px">
                    <LoadingPanelImage Url="~/App_Themes/Glass/Web/Loading.gif">
                        </LoadingPanelImage>

                        <ContentCollection>
                    <dx:PopupControlContentControl ID = "PopupControlContentControl1" runat="server" SupportsDisabledAttribute="True">
                    <dx:ASPxPanel ID="Panel1" runat="server" DefaultButton="btOK">
                                    <PanelCollection>
                                        <dx:PanelContent ID="PanelContent6" runat="server">
                                            <div style="min-height: 70px;">
                                                <dx:ASPxLabel ID="ErrorMessageLabel" runat="server">
                                                </dx:ASPxLabel>
                                            </div>
                                            <div>
                                                <table cellpadding="0" cellspacing="0" width="100%">
                                                    <tr>
                                                        <td align="center">
                                                            <dx:ASPxButton ID="ValidationOkButton" runat="server" AutoPostBack="False" 
                                                                CssFilePath="~/App_Themes/Office2010Blue/{0}/styles.css" 
                                                                CssPostfix="Office2010Blue" 
                                                                SpriteCssFilePath="~/App_Themes/Office2010Blue/{0}/sprite.css" Text="OK" 
                                                                Width="80px">
                                                                <ClientSideEvents Click="function(s, e) { pcErrorMessage.Hide(); }" />
<ClientSideEvents Click="function(s, e) { pcErrorMessage.Hide(); }"></ClientSideEvents>
                                                            </dx:ASPxButton>
                                                            <dx:ASPxButton ID="ValidationUpdatedButton" runat="server" AutoPostBack="False" 
                                                                CssFilePath="~/App_Themes/Office2010Blue/{0}/styles.css" 
                                                                CssPostfix="Office2010Blue"  
                                                                SpriteCssFilePath="~/App_Themes/Office2010Blue/{0}/sprite.css" Text="OK" 
                                                                Visible="False" Width="80px" >
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
                    <tr>
                        <td>
                            <dx:ASPxButton ID="formOKButton" runat="server" 
                                CssFilePath="~/App_Themes/Office2010Blue/{0}/styles.css" 
                                CssPostfix="Office2010Blue" Height="29px" OnClick="formOKButton_Click" 
                                SpriteCssFilePath="~/App_Themes/Office2010Blue/{0}/sprite.css" Text="OK" 
                                Width="76px">
                            </dx:ASPxButton>
                        </td>
                        <td>
                            <dx:ASPxButton ID="FormCancelButton" runat="server" CausesValidation="False" 
                                CssFilePath="~/App_Themes/Office2010Blue/{0}/styles.css" 
                                CssPostfix="Office2010Blue" Height="29px" OnClick="FormCancelButton_Click" 
                                SpriteCssFilePath="~/App_Themes/Office2010Blue/{0}/sprite.css" Text="Cancel" 
                                Width="76px">
                            </dx:ASPxButton>
                        </td>
                    </tr>
                </caption>
            </tr>
        </table>
      </dx:PanelContent>
                                        </PanelCollection>
                                    </dx:ASPxRoundPanel>
</asp:Content>
