<%@ Page Title="VitalSigns Plus" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="VitalSignsBlog1.aspx.cs" Inherits="VSWebUI.Configurator.VitalSignsBlog1" %>

<%@ Register assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>




<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
      <div>
    
        <table class="style1">
            <tr>
                <td colspan="2">
    <dx:ASPxLabel runat="server" Text="VitalSigns Blog and User Community" Font-Size="XX-Large" 
                        ID="ASPxLabel3"></dx:ASPxLabel>

                        </td>
            </tr>
            <tr>
                <td valign="top">
                    <dx:ASPxRoundPanel ID="ASPxRoundPanel3" runat="server" 
                        CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass" 
                        GroupBoxCaptionOffsetY="-24px" 
                        SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" Width="345px" 
                        HeaderText="Fine Tuning Alerts" Height="34px">
                        <ContentPaddings PaddingBottom="10px" PaddingLeft="4px" PaddingTop="10px" />
                        <HeaderStyle Height="23px">
                        <Paddings PaddingBottom="0px" PaddingLeft="2px" PaddingTop="0px" />
                        </HeaderStyle>
                        <PanelCollection>
<dx:PanelContent ID="PanelContent4" runat="server" SupportsDisabledAttribute="True"></dx:PanelContent>
</PanelCollection>
                    </dx:ASPxRoundPanel>
                </td>
                <td rowspan="4">
                    <dx:ASPxNavBar ID="ASPxNavBar2" runat="server" 
                        CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua" 
                        SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css">
                        <Groups>
                            <dx:NavBarGroup Name="Recent Posts" Text="Recent Posts">
                                <Items>
                                    <dx:NavBarItem Text="Fine Tuning Alerts">
                                    </dx:NavBarItem>
                                    <dx:NavBarItem Text=" Sever room meltdown prevented">
                                    </dx:NavBarItem>
                                    <dx:NavBarItem Text="Anouncing VitalSigns 11">
                                    </dx:NavBarItem>
                                    <dx:NavBarItem Text="See you at BLOG?">
                                    </dx:NavBarItem>
                                    <dx:NavBarItem Text="Major enhancements to IBM Traveler Monitoring">
                                    </dx:NavBarItem>
                                </Items>
                            </dx:NavBarGroup>
                            <dx:NavBarGroup Text="Pages">
                                <Items>
                                    <dx:NavBarItem Text="5 Key Areas of IBM Domino  Server Health">
                                    </dx:NavBarItem>
                                    <dx:NavBarItem Text="About This Site(contact/Welcome)">
                                    </dx:NavBarItem>
                                    <dx:NavBarItem Text="About VitalSigns">
                                    </dx:NavBarItem>
                                    <dx:NavBarItem Text="Current Version">
                                    </dx:NavBarItem>
                                    <dx:NavBarItem Text="How to Buy VitalSigns">
                                    </dx:NavBarItem>
                                </Items>
                            </dx:NavBarGroup>
                            <dx:NavBarGroup Text="Blogroll" Expanded="False">
                                <Items>
                                    <dx:NavBarItem Text="End Brill">
                                    </dx:NavBarItem>
                                </Items>
                            </dx:NavBarGroup>
                        </Groups>
                        <LoadingPanelImage Url="~/App_Themes/Aqua/Web/nbLoading.gif">
                        </LoadingPanelImage>
                    </dx:ASPxNavBar>
                </td>
            </tr>
            <tr>
                <td valign="top">
                    <dx:ASPxLabel ID="ASPxLabel2" runat="server" 
                        Text=" At the request of one of our larger customers,We have added a new option to the Alert Definitions. Now we can specify whitch servers an alert to apply to. For example, you can Define an alert for West Coast Admins then check of all the servers that group is Responsible for. If one of those servers has an issue, the alert will go to them. If a server not on the list has the issue, that perticular alert will be skipped." 
                          CssClass="lblsmallFont" Width="400px" >
                    </dx:ASPxLabel>
                </td>
            </tr>
            <tr>
                <td valign="top">
                    <dx:ASPxRoundPanel ID="ASPxRoundPanel4" runat="server" 
                        CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass" 
                        GroupBoxCaptionOffsetY="-24px" 
                        SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" Width="339px" 
                        HeaderText="Server Room meltdown prevented">
                        <ContentPaddings PaddingBottom="10px" PaddingLeft="4px" PaddingTop="10px" />
                        <HeaderStyle Height="23px">
                        <Paddings PaddingBottom="0px" PaddingLeft="2px" PaddingTop="0px" />
                        </HeaderStyle>
                        <PanelCollection>
<dx:PanelContent ID="PanelContent5" runat="server" SupportsDisabledAttribute="True"></dx:PanelContent>
</PanelCollection>
                    </dx:ASPxRoundPanel>
                </td>
            </tr>
            <tr>
                <td valign="top">
                    <dx:ASPxLabel ID="ASPxLabel4" runat="server" CssClass="lblsmallFont" 
                        Text="One of our customer had a near meltdown this weekend. Apparently the AC in the server rtoom malfunctioned on Saturday afternoon resulting in hot air being Vented into the server room. Shortly thereafter the servers got so hot they began automatically shutting Down.">
                    </dx:ASPxLabel>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    &nbsp;</td>
            </tr>
        </table>
    
    </div>
</asp:Content>