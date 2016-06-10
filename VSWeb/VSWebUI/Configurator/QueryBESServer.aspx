<%@ Page Title="VitalSigns Plus-QueryBESServer" Language="C#" MasterPageFile="~/Site1.Master"AutoEventWireup="true" CodeBehind="QueryBESServer.aspx.cs" Inherits="VSWebUI.WebForm16" %>
<%@ Register assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>




<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style1
        {
            
        }
        .style2
        {
            width: 100%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table class="style1">
        <tr>
            <td>
                <dx:ASPxPageControl Font-Bold="True"  ID="ASPxPageControl1" runat="server" ActiveTabIndex="0" 
                    CssFilePath="~/App_Themes/Glass/{0}/styles.css" 
                    CssPostfix="Glass" 
                    SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" Height="705px" 
                    TabSpacing="0px" EnableHierarchyRecreation="False">
                    <TabPages>
                        <dx:TabPage Text="BlackBerryServerInformation">
                        <TabImage Url="~/images/icons/BBDevice.gif" />
                            <ContentCollection>
                                <dx:ContentControl runat="server" SupportsDisabledAttribute="True">
                                    <table class="style1">
                                        <tr>
                                            <td>
                                                <table class="style1">
                                                    <tr>
                                                        <td>
                                                            &nbsp;</td>
                                                        <td align="center">
                                                            &nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <dx:ASPxLabel ID="ASPxLabel38" runat="server" CssClass="lblsmallFont" 
                                                                Text=" BlackBerry Alert Service">
                                                            </dx:ASPxLabel>
                                                        </td>
                                                        <td align="center">
                                                            <dx:ASPxLabel ID="ASPxLabel13" runat="server" style="background-color: #CC6699" 
                                                                Text=" N/A" Width="80px">
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
                                                            <dx:ASPxLabel ID="ASPxLabel24" runat="server" 
                                                                Text="BlackBerry Attachment Service" CssClass="lblsmallFont">
                                                            </dx:ASPxLabel>
                                                        </td>
                                                        <td align="center">
                                                            <dx:ASPxLabel ID="ASPxLabel14" runat="server" style="background-color: #CC6699" 
                                                                Text=" N/A" Width="80px">
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
                                                            <dx:ASPxLabel ID="ASPxLabel3" runat="server" Text="BlackBerry Control Service" CssClass="lblsmallFont">
                                                            </dx:ASPxLabel>
                                                        </td>
                                                        <td align="center">
                                                            <dx:ASPxLabel ID="ASPxLabel15" runat="server" style="background-color: #CC6699" 
                                                                Text=" N/A" Width="80px">
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
                                                            <dx:ASPxLabel ID="ASPxLabel4" runat="server" Text="BlackBerry Dispacher Service" CssClass="lblsmallFont">
                                                            </dx:ASPxLabel>
                                                        </td>
                                                        <td align="center">
                                                            <dx:ASPxLabel ID="ASPxLabel16" runat="server" style="background-color: #CC6699" 
                                                                Text=" N/A" Width="80px">
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
                                                            <dx:ASPxLabel ID="ASPxLabel5" runat="server" 
                                                                Text="BlackBerryMessaging Agent(4.0 only)" CssClass="lblsmallFont">
                                                            </dx:ASPxLabel>
                                                        </td>
                                                        <td align="center">
                                                            <dx:ASPxLabel ID="ASPxLabel17" runat="server" style="background-color: #CC6699" 
                                                                Text=" N/A" Width="80px">
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
                                                            <dx:ASPxLabel ID="ASPxLabel6" runat="server" 
                                                                Text="BlackBerryMobile Services(4.0)" CssClass="lblsmallFont">
                                                            </dx:ASPxLabel>
                                                        </td>
                                                        <td align="center">
                                                            <dx:ASPxLabel ID="ASPxLabel18" runat="server" style="background-color: #CC6699" 
                                                                Text=" N/A" Width="80px">
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
                                                            <dx:ASPxLabel ID="ASPxLabel7" runat="server" 
                                                                Text="BlackBerry Policy Service(4.0)" CssClass="lblsmallFont">
                                                            </dx:ASPxLabel>
                                                        </td>
                                                        <td align="center">
                                                            <dx:ASPxLabel ID="ASPxLabel19" runat="server" style="background-color: #CC6699" 
                                                                Text=" N/A" Width="80px">
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
                                                            <dx:ASPxLabel ID="ASPxLabel8" runat="server" Text="BlackBerry Router Service" CssClass="lblsmallFont">
                                                            </dx:ASPxLabel>
                                                        </td>
                                                        <td align="center">
                                                            <dx:ASPxLabel ID="ASPxLabel20" runat="server" style="background-color: #CC6699" 
                                                                Text=" N/A" Width="80px">
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
                                                            <dx:ASPxLabel ID="ASPxLabel9" runat="server" 
                                                                Text="BlackBerry Synchronization Service" CssClass="lblsmallFont">
                                                            </dx:ASPxLabel>
                                                        </td>
                                                        <td align="center">
                                                            <dx:ASPxLabel ID="ASPxLabel21" runat="server" style="background-color: #CC6699" 
                                                                Text=" N/A" Width="80px">
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
                                                            <dx:ASPxLabel ID="ASPxLabel10" runat="server" Text="BlackBerry MDS Service" CssClass="lblsmallFont">
                                                            </dx:ASPxLabel>
                                                        </td>
                                                        <td align="center">
                                                            <dx:ASPxLabel ID="ASPxLabel22" runat="server" style="background-color: #CC6699" 
                                                                Text=" N/A" Width="80px">
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
                                                            <dx:ASPxLabel ID="ASPxLabel11" runat="server" Text="MDS Connection Service" CssClass="lblsmallFont">
                                                            </dx:ASPxLabel>
                                                        </td>
                                                        <td align="center">
                                                            <dx:ASPxLabel ID="ASPxLabel23" runat="server" style="background-color: #CC6699" 
                                                                Text=" N/A" Width="80px">
                                                            </dx:ASPxLabel>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td valign="top">
                                                <table class="style1" >
                                                    <tr>
                                                        <td>
                                                            &nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" 
                                                                CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass" 
                                                                GroupBoxCaptionOffsetY="-24px" HeaderText="CurrentStatus" 
                                                                SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" Width="310px">
                                                                <ContentPaddings PaddingBottom="10px" PaddingLeft="4px" PaddingTop="10px" />
                                                                <HeaderStyle Height="23px">
                                                                <Paddings PaddingBottom="0px" PaddingLeft="2px" PaddingTop="0px" />
                                                                </HeaderStyle>
                                                                <PanelCollection>
                                                                    <dx:PanelContent runat="server" SupportsDisabledAttribute="True">
                                                                        <table class="style1">
                                                                            <tr>
                                                                                <td>
                                                                                    <dx:ASPxLabel ID="ASPxLabel25" runat="server" CssClass="lblsmallFont" 
                                                                                        Text="Connection Status:">
                                                                                    </dx:ASPxLabel>
                                                                                </td>
                                                                                <td>
                                                                                    <dx:ASPxLabel ID="ASPxLabel27" runat="server" CssClass="lblsmallFont">
                                                                                    </dx:ASPxLabel>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    <dx:ASPxLabel ID="ASPxLabel26" runat="server" CssClass="lblsmallFont" 
                                                                                        Text="PendingMessages:">
                                                                                    </dx:ASPxLabel>
                                                                                </td>
                                                                                <td>
                                                                                    <dx:ASPxTextBox ID="ASPxTextBox1" runat="server" Width="50px">
                                                                                    </dx:ASPxTextBox>
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
                                                            &nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <dx:ASPxRoundPanel ID="ASPxRoundPanel2" runat="server" 
                                                                CssFilePath="~/App_Themes/Glass/{0}/styles.css" 
                                                                CssPostfix="Glass" 
                                                                GroupBoxCaptionOffsetY="-24px" HeaderText="Server Information" 
                                                                SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" 
                                                                Width="310px">
                                                                <ContentPaddings PaddingBottom="10px" PaddingTop="10px" PaddingLeft="4px" />
                                                                <HeaderStyle Height="23px">
                                                                <Paddings PaddingBottom="0px" PaddingLeft="2px" PaddingTop="0px" />
                                                                </HeaderStyle>
                                                                <PanelCollection>
                                                                    <dx:PanelContent runat="server" SupportsDisabledAttribute="True">
                                                                        <table class="style1">
                                                                            <tr>
                                                                                <td>
                                                                                    <dx:ASPxLabel ID="ASPxLabel28" runat="server" Text="ServerName:" CssClass="lblsmallFont">
                                                                                    </dx:ASPxLabel>
                                                                                </td>
                                                                                <td>
                                                                                    <dx:ASPxLabel ID="ASPxLabel31" runat="server" CssClass="lblsmallFont">
                                                                                    </dx:ASPxLabel>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    <dx:ASPxLabel ID="ASPxLabel29" runat="server" Text="Server Version:" CssClass="lblsmallFont">
                                                                                    </dx:ASPxLabel>
                                                                                </td>
                                                                                <td>
                                                                                    <dx:ASPxLabel ID="ASPxLabel32" runat="server" CssClass="lblsmallFont">
                                                                                    </dx:ASPxLabel>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    <dx:ASPxLabel ID="ASPxLabel30" runat="server" Text="Licenses Used:" CssClass="lblsmallFont">
                                                                                    </dx:ASPxLabel>
                                                                                </td>
                                                                                <td>
                                                                                    <dx:ASPxLabel ID="ASPxLabel33" runat="server" CssClass="lblsmallFont">
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
                                                            &nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <dx:ASPxRoundPanel ID="ASPxRoundPanel3" runat="server" 
                                                                CssFilePath="~/App_Themes/Glass/{0}/styles.css" 
                                                                CssPostfix="Glass" 
                                                                GroupBoxCaptionOffsetY="-24px" HeaderText="Message History" 
                                                                SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" 
                                                                Width="300px">
                                                                <ContentPaddings PaddingBottom="10px" PaddingTop="10px" PaddingLeft="4px" />
                                                                <HeaderStyle Height="23px">
                                                                <Paddings PaddingBottom="0px" PaddingLeft="2px" PaddingTop="0px" />
                                                                </HeaderStyle>
                                                                <PanelCollection>
                                                                    <dx:PanelContent runat="server" SupportsDisabledAttribute="True">
                                                                        <table class="style1">
                                                                            <tr>
                                                                                <td>
                                                                                    <dx:ASPxLabel ID="ASPxLabel34" runat="server" Text="Total Sent" CssClass="lblsmallFont">
                                                                                    </dx:ASPxLabel>
                                                                                </td>
                                                                                <td>
                                                                                    <dx:ASPxTextBox ID="ASPxTextBox2" runat="server" Width="70px" CssClass="lblsmallFont">
                                                                                    </dx:ASPxTextBox>
                                                                                </td>
                                                                                <td>
                                                                                    &nbsp;</td>
                                                                                <td>
                                                                                    <dx:ASPxLabel ID="ASPxLabel36" runat="server" Text="Total Experied" CssClass="lblsmallFont">
                                                                                    </dx:ASPxLabel>
                                                                                </td>
                                                                                <td>
                                                                                    <dx:ASPxTextBox ID="ASPxTextBox4" runat="server" Width="70px" CssClass="lblsmallFont">
                                                                                    </dx:ASPxTextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    &nbsp;</td>
                                                                                <td>
                                                                                    &nbsp;</td>
                                                                                <td>
                                                                                    &nbsp;</td>
                                                                                <td>
                                                                                    &nbsp;</td>
                                                                                <td>
                                                                                    &nbsp;</td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    <dx:ASPxLabel ID="ASPxLabel35" runat="server" Text="Total Received" CssClass="lblsmallFont">
                                                                                    </dx:ASPxLabel>
                                                                                </td>
                                                                                <td>
                                                                                    <dx:ASPxTextBox ID="ASPxTextBox3" runat="server" Width="70px" CssClass="lblsmallFont">
                                                                                    </dx:ASPxTextBox>
                                                                                </td>
                                                                                <td>
                                                                                    &nbsp;</td>
                                                                                <td>
                                                                                    <dx:ASPxLabel ID="ASPxLabel37" runat="server" Text="Total Filtered" CssClass="lblsmallFont">
                                                                                    </dx:ASPxLabel>
                                                                                </td>
                                                                                <td>
                                                                                    <dx:ASPxTextBox ID="ASPxTextBox5" runat="server" Width="70px">
                                                                                    </dx:ASPxTextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    &nbsp;</td>
                                                                                <td>
                                                                                    &nbsp;</td>
                                                                                <td>
                                                                                    &nbsp;</td>
                                                                                <td>
                                                                                    &nbsp;</td>
                                                                                <td>
                                                                                    &nbsp;</td>
                                                                            </tr>
                                                                        </table>
                                                                    </dx:PanelContent>
                                                                </PanelCollection>
                                                            </dx:ASPxRoundPanel>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                &nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" style="background-color: #FFFFFF; color:#15428B;">
                                                <strong>Quick Trouble Shooting Tipes:</strong> The BES server must have the SNMP 
                                                servicve enabled. it must allow the SNMP request from this IP Address. And you 
                                                must provide the proper community Name in the BlackBerry Server Properties Box.</td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                &nbsp;</td>
                                        </tr>
                                    </table>
                                </dx:ContentControl>
                            </ContentCollection>
                        </dx:TabPage>
                        <dx:TabPage Text="Running Services">
                     <TabImage Url="~/images/icons/information.png"/>
                            <ContentCollection>
                                <dx:ContentControl runat="server" SupportsDisabledAttribute="True">
                                </dx:ContentControl>
                            </ContentCollection>
                        </dx:TabPage>
                    </TabPages>
                    <LoadingPanelImage Url="~/App_Themes/Glass/Web/Loading.gif">
                    </LoadingPanelImage>
                    <Paddings PaddingLeft="0px" />
                    <ContentStyle>
                        <Border BorderColor="#4986A2" />
                    </ContentStyle>
                </dx:ASPxPageControl>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                <table>
                    <tr>
                        <td>
                            <dx:ASPxButton ID="ASPxButton2" runat="server" 
                                CssFilePath="~/App_Themes/Office2010Blue/{0}/styles.css" 
                                CssPostfix="Office2010Blue" 
                                SpriteCssFilePath="~/App_Themes/Office2010Blue/{0}/sprite.css" 
                                Text="Ping Now" Height="29px" Width="100px">
                            </dx:ASPxButton>
                        </td>
                        <td>
                            <dx:ASPxButton ID="ASPxButton3" runat="server" 
                                CssFilePath="~/App_Themes/Office2010Blue/{0}/styles.css" 
                                CssPostfix="Office2010Blue" 
                                SpriteCssFilePath="~/App_Themes/Office2010Blue/{0}/sprite.css" Text="OK" 
                                Height="29px" Width="100px">
                            </dx:ASPxButton>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
