<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master"AutoEventWireup="true" CodeBehind="MailServices.aspx.cs" Inherits="VSWebUI.WebForm13" %><%@ Register assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>




<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style1
        {
            width: 100%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <br />
    <br />
    <table class="style1">
        <tr>
            <td>
                <dx:ASPxPanel ID="ASPxPanel1" runat="server" style="background-color: #3399FF; color: #FFFFFF;" 
                    Width="600px">
                    <PanelCollection>
<dx:PanelContent runat="server" SupportsDisabledAttribute="True">
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; New Mail Service 
    Profile</dx:PanelContent>
</PanelCollection>
                </dx:ASPxPanel>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                <dx:ASPxPageControl Font-Bold="True"  ID="ASPxPageControl1" runat="server" ActiveTabIndex="0"
                 CssFilePath="~/App_Themes/Glass/{0}/styles.css" 
        CssPostfix="Glass" 
        SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" TabSpacing="0px">
                    <TabPages>
                        <dx:TabPage Text="General">
                            <ContentCollection>
                                <dx:ContentControl runat="server" SupportsDisabledAttribute="True">
                                    <table class="style1">
                                        <tr>
                                            <td colspan="2">
                                                <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" Width="600px"
                                                 CssFilePath="~/App_Themes/Glass/{0}/styles.css" 
                                         CssPostfix="Glass" 
                                       SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" 
                                                    HeaderText="Service Attributes" GroupBoxCaptionOffsetY="-24px">
                                                     <ContentPaddings PaddingBottom="10px" PaddingLeft="4px" PaddingTop="10px" />
                                                     <HeaderStyle Height="23px">
                                                                <Paddings PaddingBottom="0px" PaddingLeft="2px" PaddingTop="0px" />
                                           <Paddings Padding="0px" PaddingLeft="2px" PaddingRight="2px" PaddingBottom="7px"></Paddings>
                                                                </HeaderStyle>             
                                                                   <panelcollection>
                                                        <dx:PanelContent runat="server" SupportsDisabledAttribute="True">
                                                            <table class="style1">
                                                                <tr>
                                                                    <td>
                                                                        <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="Name:" CssClass="lblsmallFont">
                                                                        </dx:ASPxLabel>
                                                                    </td>
                                                                    <td colspan="3">
                                                                        <dx:ASPxTextBox ID="ASPxTextBox1" runat="server" Width="170px" CssClass="txtsmall">
                                                                        </dx:ASPxTextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="Address:" CssClass="lblsmallFont">
                                                                        </dx:ASPxLabel>
                                                                    </td>
                                                                    <td colspan="3">
                                                                        <dx:ASPxTextBox ID="ASPxTextBox2" runat="server" Width="170px" CssClass="txtsmall">
                                                                        </dx:ASPxTextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <dx:ASPxLabel ID="ASPxLabel3" runat="server" Text="Description:" CssClass="lblsmallFont">
                                                                        </dx:ASPxLabel>
                                                                    </td>
                                                                    <td colspan="3">
                                                                        <dx:ASPxTextBox ID="ASPxTextBox3" runat="server" Width="170px" CssClass="txtsmall">
                                                                        </dx:ASPxTextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <dx:ASPxLabel ID="ASPxLabel4" runat="server" Text="Protocol:" CssClass="lblsmallFont">
                                                                        </dx:ASPxLabel>
                                                                    </td>
                                                                    <td>
                                                                        <dx:ASPxComboBox ID="ASPxComboBox1" runat="server" CssClass="lblsmallFont">
                                                                        </dx:ASPxComboBox>
                                                                    </td>
                                                                    <td>
                                                                        <dx:ASPxLabel ID="ASPxLabel5" runat="server" Text="Port:" CssClass="lblsmallFont">
                                                                        </dx:ASPxLabel>
                                                                    </td>
                                                                    <td>
                                                                        <dx:ASPxTextBox ID="ASPxTextBox4" runat="server" Width="170px" CssClass="txtsmall">
                                                                        </dx:ASPxTextBox>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </dx:PanelContent>
                                                    </panelcollection>
                                                </dx:ASPxRoundPanel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                &nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <dx:ASPxRoundPanel ID="ASPxRoundPanel2" runat="server" 
                                                    CssFilePath="~/App_Themes/Glass/{0}/styles.css" 
                                                    CssPostfix="Glass" 
                                                    SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" 
                                                    Width="300px" HeaderText="ScanSettings" GroupBoxCaptionOffsetY="-24px">
                                                     <ContentPaddings PaddingBottom="10px" PaddingLeft="4px" PaddingTop="10px" />
                                                     <HeaderStyle Height="23px">
                                                                <Paddings PaddingBottom="0px" PaddingLeft="2px" PaddingTop="0px" />
                                           <Paddings Padding="0px" PaddingLeft="2px" PaddingRight="2px" PaddingBottom="7px"></Paddings>
                                                                </HeaderStyle>             
                                                    <panelcollection>
                                                        <dx:PanelContent runat="server" SupportsDisabledAttribute="True">
                                                            <table class="style1">
                                                                <tr>
                                                                    <td>
                                                                        <dx:ASPxCheckBox ID="ASPxCheckBox1" runat="server" CheckState="Unchecked" 
                                                                            Text="Enabled  for Scanning" CssClass="lblsmallFont">
                                                                        </dx:ASPxCheckBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <table class="style1">
                                                                            <tr>
                                                                                <td>
                                                                                    <dx:ASPxLabel ID="ASPxLabel6" runat="server" Text="Scan Interval" CssClass="lblsmallFont">
                                                                                    </dx:ASPxLabel>
                                                                                </td>
                                                                                <td>
                                                                                    <dx:ASPxTextBox ID="ASPxTextBox5" runat="server" Width="40px" CssClass="txtsmall">
                                                                                    </dx:ASPxTextBox>
                                                                                </td>
                                                                                <td>
                                                                                    <dx:ASPxLabel ID="ASPxLabel9" runat="server" Text="minutes" CssClass="lblsmallFont">
                                                                                    </dx:ASPxLabel>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    <dx:ASPxLabel ID="ASPxLabel7" runat="server" Text="Off Hours Scan Interval" CssClass="lblsmallFont">
                                                                                    </dx:ASPxLabel>
                                                                                </td>
                                                                                <td>
                                                                                    <dx:ASPxTextBox ID="ASPxTextBox6" runat="server" Width="40px" CssClass="txtsmall">
                                                                                    </dx:ASPxTextBox>
                                                                                </td>
                                                                                <td>
                                                                                    <dx:ASPxLabel ID="ASPxLabel10" runat="server" Text="minutes" CssClass="lblsmallFont">
                                                                                    </dx:ASPxLabel>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    <dx:ASPxLabel ID="ASPxLabel8" runat="server" Text="Retry Interval" CssClass="lblsmallFont">
                                                                                    </dx:ASPxLabel>
                                                                                </td>
                                                                                <td>
                                                                                    <dx:ASPxTextBox ID="ASPxTextBox7" runat="server" Width="40px" CssClass="txtsmall">
                                                                                    </dx:ASPxTextBox>
                                                                                </td>
                                                                                <td>
                                                                                    <dx:ASPxLabel ID="ASPxLabel11" runat="server" Text="minutes" CssClass="lblsmallFont">
                                                                                    </dx:ASPxLabel>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </dx:PanelContent>
                                                    </panelcollection>
                                                </dx:ASPxRoundPanel>
                                            </td>
                                            <td>
                                            <dx:ASPxRoundPanel ID="ASPxRoundPanel3" runat="server" 
                                                    CssFilePath="~/App_Themes/Glass/{0}/styles.css" 
                                                    CssPostfix="Glass" 
                                                    SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" 
                                                    Width="300px" HeaderText="Alert Settings" GroupBoxCaptionOffsetY="-24px">
                                                     <ContentPaddings PaddingBottom="10px" PaddingLeft="4px" PaddingTop="10px" />
                                                     <HeaderStyle Height="23px">
                                                                <Paddings PaddingBottom="0px" PaddingLeft="2px" PaddingTop="0px" />
                                           <Paddings Padding="0px" PaddingLeft="2px" PaddingRight="2px" PaddingBottom="7px"></Paddings>
                                                                </HeaderStyle>             
                                                    <panelcollection>
                                                        <dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">
                                                            <table class="style1">
                                                                <tr>
                                                                    <td>
                                                                        <dx:ASPxLabel ID="ASPxLabel12" runat="server" Text="Failures before Alert:" CssClass="lblsmallFont">
                                                                        </dx:ASPxLabel>
                                                                    </td>
                                                                    <td>
                                                                        <dx:ASPxTextBox ID="ASPxTextBox8" runat="server" Width="40px" CssClass="txtsmall">
                                                                        </dx:ASPxTextBox>
                                                                    </td>
                                                                    <td>
                                                                        <dx:ASPxLabel ID="ASPxLabel14" runat="server" Text="consecutive failures" CssClass="lblsmallFont">
                                                                        </dx:ASPxLabel>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        &nbsp;</td>
                                                                    <td>
                                                                        &nbsp;</td>
                                                                    <td>
                                                                        &nbsp;</td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <dx:ASPxLabel ID="ASPxLabel13" runat="server" Text="Response Threshold" CssClass="lblsmallFont">
                                                                        </dx:ASPxLabel>
                                                                    </td>
                                                                    <td>
                                                                        <dx:ASPxTextBox ID="ASPxTextBox9" runat="server" Width="40px" CssClass="txtsmall">
                                                                        </dx:ASPxTextBox>
                                                                    </td>
                                                                    <td>
                                                                        <dx:ASPxLabel ID="ASPxLabel15" runat="server" Text="milliseconds" CssClass="lblsmallFont">
                                                                        </dx:ASPxLabel>
                                                                    </td>
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
                                                        </dx:PanelContent>
                                                    </panelcollection>
                                                </dx:ASPxRoundPanel>
                                               </td>
                                        </tr>
                                    </table>
                                </dx:ContentControl>
                            </ContentCollection>
                        </dx:TabPage>
                        <dx:TabPage Text="Maitenance Window">
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
                <table class="style1">
                    <tr>
                        <td>
                            <dx:ASPxButton ID="ASPxButton1" runat="server" 
                                CssFilePath="~/App_Themes/Office2010Blue/{0}/styles.css" CssPostfix="Office2010Blue" 
                                SpriteCssFilePath="~/App_Themes/Office2010Blue/{0}/sprite.css" Text="ok" 
                                Height="29px" Width="76px">
                            </dx:ASPxButton>
                        </td>
                        <td>
                            <dx:ASPxButton ID="ASPxButton2" runat="server" 
                                CssFilePath="~/App_Themes/Office2010Blue/{0}/styles.css" CssPostfix="Office2010Blue" 
                                SpriteCssFilePath="~/App_Themes/Office2010Blue/{0}/sprite.css" 
                                Text="Cancel" Height="29px" Width="76px">
                            </dx:ASPxButton>
                        </td>
                    </tr>
                </table>
                &nbsp;</td>
        </tr>
    </table>
</asp:Content>
