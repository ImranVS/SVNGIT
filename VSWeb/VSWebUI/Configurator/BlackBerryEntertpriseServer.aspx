<%@ Page Title="VitalSigns Plus-BlackBerryEntertpriseServer" Language="C#" MasterPageFile="~/Site1.Master"AutoEventWireup="true" CodeBehind="BlackBerryEntertpriseServer.aspx.cs" Inherits="VSWebUI.WebForm15" %><%@ Register assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>







<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script src="../js/jquery-1.9.1.js" type="text/javascript"></script>
    <script src="../js/bootstrap.min.js" type="text/javascript"></script>
    <style type="text/css">
        .style1
        {
            width: 100%;
        }
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            $('.alert-success').delay(10000).fadeOut("slow", function () {
            });
           });
           function OnItemClick(s, e) {
           	if (e.item.parent == s.GetRootItem())
           		e.processOnServer = false;
           }
        </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<table class="style1">
        <tr>
            <td>
                <div class="header" id="servernamelbldisp" runat="server">BlackBerry Enterprise Server</div>
            </td>
            <td align="right">
                <table align="right">
<tr><td >
                            <dx:ASPxMenu ID="ASPxMenu1" runat="server" EnableTheming="True" 
                    HorizontalAlign="Right"  onitemclick="ASPxMenu1_ItemClick" ShowAsToolbar="True" 
                    Theme="Moderno">
                    <ClientSideEvents ItemClick="OnItemClick" />
                    <Items>
                        <dx:MenuItem Name="MainItem">
                            <Items>
                                
                                <dx:MenuItem Name="ServerDetailsPage" Text="View Server Detailed Page">
                                </dx:MenuItem>
                               
                            </Items>
                            <Image Url="~/images/icons/Gear.png">
                            </Image>
                        </dx:MenuItem>
                    </Items>
                </dx:ASPxMenu>
                        </td></tr>
</table>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <dx:ASPxPageControl Font-Bold="True"  ID="ASPxPageControl1" runat="server" ActiveTabIndex="0"
                 CssFilePath="~/App_Themes/Glass/{0}/styles.css" 
                   CssPostfix="Glass" SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" 
                    TabSpacing="0px" EnableHierarchyRecreation="False">
                    <TabPages>
                        <dx:TabPage Text="General Settings">
                        
                        <TabImage Url="~/images/information.png"/>
<TabImage Url="~/images/information.png"></TabImage>
                            <ContentCollection>
                                <dx:ContentControl ID="ContentControl1" runat="server" SupportsDisabledAttribute="True">
                                    <table class="style1">
                                        <tr>
                                            <td>
                                                <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" Width="800px"
                                                CssFilePath="~/App_Themes/Glass/{0}/styles.css" 
        CssPostfix="Glass" Height="53px"        
        SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" HeaderText="Server Attributes" GroupBoxCaptionOffsetY="-24px">
                                                    <ContentPaddings PaddingBottom="10px" PaddingLeft="4px" PaddingTop="10px" />
<ContentPaddings PaddingLeft="4px" PaddingTop="10px" PaddingBottom="10px"></ContentPaddings>

        <HeaderStyle Height="23px">
                                                    </HeaderStyle>
                                                    <panelcollection>
                                                        <dx:PanelContent ID="PanelContent2" runat="server" SupportsDisabledAttribute="True">
                                                            <table class="style1">
                                                                <tr>
                                                                    <td width="50px">
                                                                        <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="Name:" CssClass="lblsmallFont">
                                                                        </dx:ASPxLabel>
                                                                    </td>
                                                                    <td colspan="2">
                                                                        <dx:ASPxTextBox ID="NameTextBox" runat="server" Width="170px" ReadOnly="True">
                                                                            <ValidationSettings>
                                                                                <RequiredField ErrorText="please enter Name" IsRequired="True" />
<RequiredField IsRequired="True" ErrorText="please enter Name"></RequiredField>
                                                                            </ValidationSettings>
                                                                        </dx:ASPxTextBox>
                                                                    </td>
                                                                    <td>
                                                                        <dx:ASPxCheckBox ID="EnableforScanningCheckBox" runat="server" 
                                                                            CheckState="Unchecked" CssClass="lblsmallFont" Text="Enabled for scanning" 
                                                                            Wrap="False">
                                                                        </dx:ASPxCheckBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="Address:" CssClass="lblsmallFont">
                                                                        </dx:ASPxLabel>
                                                                    </td>
                                                                    <td width="70px">
                                                                        <dx:ASPxTextBox ID="AddressTextBox" runat="server" Width="170px" 
                                                                            ReadOnly="True">
                                                                            <ValidationSettings ErrorDisplayMode="ImageWithTooltip">
                                                                                <RequiredField ErrorText="Enter Address" IsRequired="True" />
<RequiredField IsRequired="True" ErrorText="Enter Address"></RequiredField>
                                                                            </ValidationSettings>
                                                                        </dx:ASPxTextBox>
                                                                    </td>
                                                                    <td>
                                                                        <dx:ASPxLabel ID="ASPxLabel5" runat="server" Text="(IP or HostName)" CssClass="lblsmallFont">
                                                                        </dx:ASPxLabel>
                                                                    </td>
                                                                    <td>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <dx:ASPxLabel ID="ASPxLabel3" runat="server" Text="Description:" 
                                                                            CssClass="lblsmallFont">
                                                                        </dx:ASPxLabel>
                                                                    </td>
                                                                    <td colspan="2">
                                                                        <dx:ASPxTextBox ID="DescriptionTextBox" runat="server" Width="170px" 
                                                                            ReadOnly="True">
                                                                            <ValidationSettings>
                                                                                <RequiredField ErrorText="Enter Description" IsRequired="True" />
<RequiredField IsRequired="True" ErrorText="Enter Description"></RequiredField>
                                                                            </ValidationSettings>
                                                                        </dx:ASPxTextBox>
                                                                    </td>
                                                                    <td>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <dx:ASPxLabel ID="ASPxLabel4" runat="server" Text="Category:" 
                                                                            CssClass="lblsmallFont">
                                                                        </dx:ASPxLabel>
                                                                    </td>
                                                                    <td colspan="2">
                                                                        <dx:ASPxTextBox ID="categoryTextBox" runat="server" Width="170px">
                                                                            <ValidationSettings>
                                                                                <RequiredField ErrorText="Enter category" IsRequired="True" />
<RequiredField IsRequired="True" ErrorText="Enter category"></RequiredField>
                                                                            </ValidationSettings>
                                                                        </dx:ASPxTextBox>
                                                                        
                                                                    </td>
                                                                    <td>
                                                                   <%-- <asp:HiddenField ID="HiddenField1" runat="server" />--%>
                                                                        <asp:Label ID="lblSid" runat="server" Visible="False"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </dx:PanelContent>
                                                    </panelcollection>
                                                </dx:ASPxRoundPanel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" valign="top">
                                                <dx:ASPxRoundPanel ID="ASPxRoundPanel2" runat="server" 
                                                    CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass" 
                                                    GroupBoxCaptionOffsetY="-24px" HeaderText="Alert Threshold" Height="53px" 
                                                    SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" Width="800px">
                                                    <ContentPaddings PaddingBottom="10px" PaddingLeft="4px" PaddingTop="10px" />
<ContentPaddings PaddingLeft="4px" PaddingTop="10px" PaddingBottom="10px"></ContentPaddings>

                                                    <HeaderStyle Height="23px">
                                                    </HeaderStyle>
                                                    <PanelCollection>
                                                        <dx:PanelContent ID="PanelContent3" runat="server" SupportsDisabledAttribute="True">
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <dx:ASPxLabel ID="ASPxLabel6" runat="server" CssClass="lblsmallFont" 
                                                                            Text="Total Pending Messages:">
                                                                        </dx:ASPxLabel>
                                                                    </td>
                                                                    <td>
                                                                        <dx:ASPxTextBox ID="pendingTextBox" runat="server" Width="170px">
<MaskSettings Mask="&lt;0..999999&gt;"></MaskSettings>
                                                                            <MaskSettings Mask="&lt;0..999999&gt;" />
                                                                            <ValidationSettings CausesValidation="True" ErrorDisplayMode="ImageWithTooltip" 
                                                                                SetFocusOnError="True">
                                                                                <RegularExpression ErrorText="Please enter a numeric value using the numbers only (0-9)." 
                                                                                    ValidationExpression="^\d+$" />
<RegularExpression ErrorText="Please enter a numeric value using the numbers only (0-9)." ValidationExpression="^\d+$"></RegularExpression>
                                                                            </ValidationSettings>
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
                                            <td align="left" valign="top">
                                                <dx:ASPxRoundPanel ID="ASPxRoundPanel3" runat="server" 
                                                    CssFilePath="~/App_Themes/Glass/{0}/styles.css" 
                                                    CssPostfix="Glass" HeaderText="Scan Settings" Height="53px" 
                                                    SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" Width="800px" 
                                                    GroupBoxCaptionOffsetY="-24px">
                                                    <ContentPaddings PaddingBottom="10px" PaddingLeft="4px" PaddingTop="10px" />
<ContentPaddings PaddingLeft="4px" PaddingTop="10px" PaddingBottom="10px"></ContentPaddings>

                                                    <HeaderStyle Height="23px" >
                                                    </HeaderStyle>
                                                    <PanelCollection>
                                                        <dx:PanelContent ID="PanelContent4" runat="server" SupportsDisabledAttribute="True">
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <dx:ASPxLabel ID="ASPxLabel7" runat="server" Text="Scan Interval:" CssClass="lblsmallFont">
                                                                        </dx:ASPxLabel>
                                                                    </td>
                                                                    <td>
                                                                        <dx:ASPxTextBox ID="ScanIntervalTextBox" runat="server" Width="40px">
                                                                            <MaskSettings Mask="&lt;0..99999&gt;" />
<MaskSettings Mask="&lt;0..99999&gt;"></MaskSettings>

                                                                            <ValidationSettings ErrorDisplayMode="ImageWithTooltip" CausesValidation="True" 
                                                                                SetFocusOnError="True">
<RequiredField IsRequired="True" ErrorText="Enter aNumber"></RequiredField>
                                                                                <RegularExpression ErrorText="Please enter a numeric value using the numbers only (0-9)." 
                                                                                    ValidationExpression="^\d+$" />
                                                                                <RequiredField ErrorText="Enter aNumber" IsRequired="True" />

<RegularExpression ErrorText="Please enter a numeric value using the numbers only (0-9)." ValidationExpression="^\d+$"></RegularExpression>
                                                                            </ValidationSettings>
                                                                        </dx:ASPxTextBox>
                                                                    </td>
                                                                    <td>
                                                                        <dx:ASPxLabel ID="ASPxLabel10" runat="server" Text="minutes" CssClass="lblsmallFont">
                                                                        </dx:ASPxLabel>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <dx:ASPxLabel ID="ASPxLabel8" runat="server" Text="Retry Interval:" CssClass="lblsmallFont">
                                                                        </dx:ASPxLabel>
                                                                    </td>
                                                                    <td>
                                                                        <dx:ASPxTextBox ID="RetryIntervalTextBox" runat="server" Width="40px" 
                                                                            Height="19px">
                                                                            <MaskSettings Mask="&lt;1..99999&gt;" />
<MaskSettings Mask="&lt;1..99999&gt;"></MaskSettings>

                                                                            <ValidationSettings ErrorDisplayMode="ImageWithTooltip" CausesValidation="True" 
                                                                                SetFocusOnError="True">
<RequiredField IsRequired="True"></RequiredField>
                                                                                <RegularExpression ErrorText="Please enter a numeric value using the numbers only (1-9)." 
                                                                                    ValidationExpression="^\d+$" />
                                                                                <RequiredField IsRequired="True" />

<RegularExpression ErrorText="Please enter a numeric value using the numbers only (0-9)." ValidationExpression="^\d+$"></RegularExpression>
                                                                            </ValidationSettings>
                                                                        </dx:ASPxTextBox>
                                                                    </td>
                                                                    <td>
                                                                        <dx:ASPxLabel ID="ASPxLabel11" runat="server" Text="minutes" CssClass="lblsmallFont">
                                                                        </dx:ASPxLabel>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <dx:ASPxLabel ID="ASPxLabel9" runat="server" Text="Off-Hours Scan Interval:" 
                                                                            CssClass="lblsmallFont">
                                                                        </dx:ASPxLabel>
                                                                    </td>
                                                                    <td>
                                                                        <dx:ASPxTextBox ID="OffHoursScanIntervalTextBox" runat="server" Width="40px">
                                                                            <MaskSettings Mask="&lt;0..99999&gt;" />
<MaskSettings Mask="&lt;0..99999&gt;"></MaskSettings>

                                                                            <ValidationSettings ErrorDisplayMode="ImageWithTooltip" CausesValidation="True" 
                                                                                SetFocusOnError="True">
                                                                                <RequiredField IsRequired="True" />
                                                                                <RegularExpression ErrorText="Please enter a numeric value using the numbers only (0-9)." 
                                                                                    ValidationExpression="^\d+$" />
<RegularExpression ErrorText=""></RegularExpression>

<RequiredField IsRequired="True"></RequiredField>
                                                                            </ValidationSettings>
                                                                        </dx:ASPxTextBox>
                                                                    </td>
                                                                    <td>
                                                                        <dx:ASPxLabel ID="ASPxLabel12" runat="server" Text="minutes" CssClass="lblsmallFont">
                                                                        </dx:ASPxLabel>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </dx:PanelContent>
                                                    </PanelCollection>
                                                </dx:ASPxRoundPanel>
                                            </td>
                                        </tr>
                                    </table>
                                </dx:ContentControl>
                            </ContentCollection>
                        </dx:TabPage>
                        <dx:TabPage Text="Advanced">
                        <TabImage Url="~/images/icons/advanced.png"/>
<TabImage Url="~/images/icons/advanced.png"></TabImage>
                            <ContentCollection>
                                <dx:ContentControl ID="ContentControl2" runat="server" SupportsDisabledAttribute="True">
                                    <table >
                                        <tr>
                                            <td>
                                                <dx:ASPxRoundPanel ID="ASPxRoundPanel6" runat="server" 
                                                    CssFilePath="~/App_Themes/Glass/{0}/styles.css" 
                                                    CssPostfix="Glass" 
                                                    GroupBoxCaptionOffsetY="-24px" 
                                                    SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" 
                                                    Width="600px" HeaderText="SNMP Options">
                                                    <ContentPaddings PaddingBottom="10px" PaddingTop="10px" PaddingLeft="4px" />
<ContentPaddings PaddingLeft="4px" PaddingTop="10px" PaddingBottom="10px"></ContentPaddings>

                                                    <HeaderStyle Height="23px">
                                                    <Paddings PaddingBottom="0px" PaddingLeft="2px" PaddingTop="0px" />
<Paddings PaddingLeft="2px" PaddingTop="0px" PaddingBottom="0px"></Paddings>
                                                    </HeaderStyle>
                                                    <PanelCollection>
                                                        <dx:PanelContent ID="PanelContent5" runat="server" SupportsDisabledAttribute="True">
                                                            <table class="style1">
                                                                <tr>
                                                                    <td colspan="3">
                                                                        <asp:Label ID="Label2" runat="server" 
                                                                            Text="The &lt;b&gt;SNMP Community&lt;/b&gt; name is like a&nbsp; password, and it is case sensitive. The monitoring station must have READ rights to this community. The BES server must be running the SNMP service.&lt;br&gt;&lt;br&gt;The SNMP Service must be enabled on BlackBerry server, and it must be set to allow SNMP Queries from the IP Address or hostname of the monitoring workstation." 
                                                                            CssClass="lblsmallFont"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td width="20%">
                                                                        <dx:ASPxLabel ID="ASPxLabel15" runat="server"  Text="SNMP Community:" 
                                                                            CssClass="lblsmallFont">
                                                                           
                                                                        </dx:ASPxLabel>
                                                                    </td>
                                                                    <td width="30%">
                                                                        <dx:ASPxTextBox ID="SNMPCommunityTextBox" runat="server" Width="170px">
                                                                        </dx:ASPxTextBox>
                                                                    </td>
                                                                    <td>
                                                                        <dx:ASPxButton ID="TestButton" runat="server" 
                                                                            CssFilePath="~/App_Themes/Office2003Blue/{0}/styles.css" 
                                                                            CssPostfix="Office2003Blue" 
                                                                            SpriteCssFilePath="~/App_Themes/Office2003Blue/{0}/sprite.css" Text="Test" 
                                                                            OnClick="TestButton_Click" Visible="False" 
                                                                           >
                                                                        </dx:ASPxButton>
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
                                                <dx:ASPxRoundPanel ID="ASPxRoundPanel81" runat="server" 
                                                                CssFilePath="~/App_Themes/Glass/{0}/styles.css" 
                                                                CssPostfix="Glass" 
                                                                GroupBoxCaptionOffsetY="-24px" HeaderText="Server Mode" 
                                                                SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" Width="600px">
                                                                <ContentPaddings PaddingBottom="10px" PaddingTop="10px" PaddingLeft="4px" />
<ContentPaddings PaddingLeft="4px" PaddingTop="10px" PaddingBottom="10px"></ContentPaddings>

                                                                <HeaderStyle Height="23px">
                                                                <Paddings PaddingBottom="0px" PaddingLeft="2px" PaddingTop="0px" />
<Paddings PaddingLeft="2px" PaddingTop="0px" PaddingBottom="0px"></Paddings>
                                                                </HeaderStyle>
                                                                <PanelCollection>
                                                                    <dx:PanelContent ID="PanelContent31" runat="server" SupportsDisabledAttribute="True">
                                                                        <table class="style1">
                                                                            <tr>
                                                                                <td colspan="2">
                                                                                    <asp:Label ID="Label41" runat="server" CssClass="lblsmallFont" 
                                                                                        Text="Specify if this server is in a High-Availability group or not and which server it is paired with. &lt;/b&gt;"></asp:Label>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                            <td>
                                                                            <dx:ASPxRadioButtonList ID="rbtnservermode" runat="server" OnSelectedIndexChanged="rbtnservermode_SelectedIndexChanged" AutoPostBack="true" >
                                                                            <Items>
                                                                            <dx:ListEditItem  Selected="true" Text="Stand Alone Server" Value="Stand Alone Server" />
                                                                            <dx:ListEditItem Text="HA Server-Typically Active" Value="HA Server-Typically Active" />
                                                                            <dx:ListEditItem Text="HA Server-Typically Standby" Value="HA Server-Typically Standby" />
                                                                            </Items>
                                                                            </dx:ASPxRadioButtonList>                                                                                                                                                  
                                                                            </td>

                                                                            <td> <dx:ASPxLabel ID="lblprdserver" runat="server" Text="Paired Server:" 
                                                                                    CssClass="lblsmallFont" Visible="False"></dx:ASPxLabel>
                                                                                 <dx:ASPxComboBox ID="SrvAtrCategoryComboBox" runat="server" Width="150px" 
                                                                                     Visible="False">
                                                                                <Items>
                                                                             
                                                                           
                                                                            </Items>
                                                                           </dx:ASPxComboBox>
                                                                          </td>                                                                     
                                                                            </tr>
                                                                            <tr>
                                                                                <td colspan="2">
                                                                                    <dx:ASPxButton ID="navgButton11" runat="server" 
                                                                                        CssFilePath="~/App_Themes/Office2003Blue/{0}/styles.css" 
                                                                                        CssPostfix="Office2003Blue" 
                                                                                        SpriteCssFilePath="~/App_Themes/Office2003Blue/{0}/sprite.css" Text="..." 
                                                                                        OnClick="navgButton_Click" Visible="False" 
                                                                                        >
                                                                                    </dx:ASPxButton>
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
                                                <dx:ASPxRoundPanel ID="ASPxRoundPanel8" runat="server" 
                                                                CssFilePath="~/App_Themes/Glass/{0}/styles.css" 
                                                                CssPostfix="Glass" 
                                                                GroupBoxCaptionOffsetY="-24px" HeaderText="Time Issues" 
                                                                SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" Width="600px">
                                                                <ContentPaddings PaddingBottom="10px" PaddingTop="10px" PaddingLeft="4px" />
<ContentPaddings PaddingLeft="4px" PaddingTop="10px" PaddingBottom="10px"></ContentPaddings>

                                                                <HeaderStyle Height="23px">
                                                                <Paddings PaddingBottom="0px" PaddingLeft="2px" PaddingTop="0px" />
<Paddings PaddingLeft="2px" PaddingTop="0px" PaddingBottom="0px"></Paddings>
                                                                </HeaderStyle>
                                                                <PanelCollection>
                                                                    <dx:PanelContent ID="PanelContent6" runat="server" SupportsDisabledAttribute="True">
                                                                        <table class="style1">
                                                                            <tr>
                                                                                <td colspan="3">
                                                                                    <asp:Label ID="Label4" runat="server" CssClass="lblsmallFont" 
                                                                                        Text=" If the BES server is located in a &lt;b&gt;different time zone&lt;/b&gt; as VitalSigns, please enter the Offset here. Also, confirm the date format of OS on the BES server."></asp:Label>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td width="20%">
                                                                                                <dx:ASPxLabel ID="ASPxLabel17" runat="server" Text="Time Zone Adjustment:" 
                                                                                                    CssClass="lblsmallFont" Wrap="False">
                                                                                                </dx:ASPxLabel>
                                                                                            </td>
                                                                                            <td width="10%">
                                                                                                <dx:ASPxTextBox ID="TimeZoneAdjestmentTextBox" runat="server" Width="40px">
<MaskSettings Mask="&lt;0..99999&gt;"></MaskSettings>
                                                                                                    <MaskSettings Mask="&lt;0..99999&gt;" />
                                                                                                    <ValidationSettings ErrorDisplayMode="ImageWithTooltip" CausesValidation="True" 
                                                                                                        SetFocusOnError="True">
                                                                                                        <RegularExpression ErrorText="Please enter a numeric value using the numbers only (0-9)." 
                                                                                                            ValidationExpression="^\d+$" />
<RegularExpression ErrorText="Please enter a numeric value using the numbers only (0-9)." ValidationExpression="^\d+$"></RegularExpression>
                                                                                                    </ValidationSettings>
                                                                                                </dx:ASPxTextBox>
                                                                                            </td>
                                                                                            <td>
                                                                                                <dx:ASPxLabel ID="ASPxLabel18" runat="server" Text="hours" CssClass="lblsmallFont">
                                                                                                </dx:ASPxLabel>
                                                                                            </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td colspan="3">
                                                                                    <dx:ASPxCheckBox ID="UsdateformateCheckBox" runat="server" CheckState="Unchecked" 
                                                                                        Text="MM/DD/YY Date Format" CssClass="lblsmallFont">
                                                                                    </dx:ASPxCheckBox>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </dx:PanelContent>
                                                                </PanelCollection>
                                                            </dx:ASPxRoundPanel>                                            
                                            </td>
                                        </tr>
                                    </table>
                                </dx:ContentControl>
                            </ContentCollection>
                        </dx:TabPage>
                        <dx:TabPage Text="BES Modules">
                        <TabImage Url="~/images/icons/BBDevice.gif"/>
<TabImage Url="~/images/icons/BBDevice.gif"></TabImage>
                            <ContentCollection>
                                <dx:ContentControl ID="ContentControl3" runat="server" SupportsDisabledAttribute="True">
                                    <table class="style1">
                                        <tr>
                                            <td>
                                                <table class="style1">
                                                    <tr>
                                                        <td style="width: 100px">
                                                            <dx:ASPxLabel ID="ASPxLabel13" runat="server" Text="BES Version:" 
                                                                CssClass="lblsmallFont">
                                                            </dx:ASPxLabel>
                                                        </td>
                                                        <td>
                                                            <dx:ASPxComboBox ID="BSEversionComboBox" runat="server" CssClass="lblsmallFont" 
                                                                OnSelectedIndexChanged="BSEversionComboBox_SelectedIndexChanged" AutoPostBack="True" 
                                                                >
                                                            </dx:ASPxComboBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <dx:ASPxRoundPanel ID="ASPxRoundPanel4" runat="server"  CssFilePath="~/App_Themes/Glass/{0}/styles.css" 
                                                    CssPostfix="Glass" HeaderText="Local BES Services" Height="53px" 
                                                    SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" Width="600px" 
                                                    GroupBoxCaptionOffsetY="-24px">
                                                    <ContentPaddings PaddingBottom="10px" PaddingLeft="4px" PaddingTop="10px" />
<ContentPaddings PaddingLeft="4px" PaddingTop="10px" PaddingBottom="10px"></ContentPaddings>

                                                    <HeaderStyle Height="23px" >                                                    
                                                    <Paddings PaddingBottom="0px" PaddingLeft="2px" PaddingTop="0px" />
<Paddings PaddingLeft="2px" PaddingTop="0px" PaddingBottom="0px"></Paddings>
                                                    </HeaderStyle>
                                                    <PanelCollection>
                                                        <dx:PanelContent ID="PanelContent7" runat="server" SupportsDisabledAttribute="True">
                                                            <table class="style1">
                                                                <tr>
                                                                    <td>
                                                                        <table class="style1">
                                                                            <tr>
                                                                                <td>
                                                                                    <dx:ASPxCheckBox ID="BlackBerryMessagingAgentCheckBox" runat="server" CheckState="Unchecked" 
                                                                                        Text="BlackBerry Messaging Agent" CssClass="lblsmallFont">
                                                                                    </dx:ASPxCheckBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    <dx:ASPxCheckBox ID="BlackBerryControllerServiceCheckBox" runat="server" CheckState="Unchecked" 
                                                                                        Text="BlackBerry Controller Service" CssClass="lblsmallFont">
                                                                                    </dx:ASPxCheckBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    <dx:ASPxCheckBox ID="BlackBerryDispacherServiceCheckBox" runat="server" CheckState="Unchecked" 
                                                                                        Text="BlackBerry Dispatcher Service" CssClass="lblsmallFont">
                                                                                    </dx:ASPxCheckBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    <dx:ASPxCheckBox ID="BlackBerrySychronizationServiceCheckBox" runat="server" CheckState="Unchecked" 
                                                                                        Text="BlackBerry Sychronization Service" CssClass="lblsmallFont">
                                                                                    </dx:ASPxCheckBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    <dx:ASPxCheckBox ID="BlackBerryPolicyServiceCheckBox" runat="server" CheckState="Unchecked" 
                                                                                        Text="BlackBerry Policy Service" CssClass="lblsmallFont">
                                                                                    </dx:ASPxCheckBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    <dx:ASPxCheckBox ID="BlackBerryMobileDataServiceCheckBox" runat="server" CheckState="Unchecked" 
                                                                                        Text="BlackBerry Mobile Data Service(4.0)" CssClass="lblsmallFont">
                                                                                    </dx:ASPxCheckBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    <dx:ASPxCheckBox ID="BlackBerryMDSConnectionServiceCheckBox" runat="server" CheckState="Unchecked" 
                                                                                        Text="BlackBerry MDS Connection Service(4.1)" CssClass="lblsmallFont">
                                                                                    </dx:ASPxCheckBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    <dx:ASPxCheckBox ID="BlackBerryMDSServicesCheckBox" runat="server" CheckState="Unchecked" 
                                                                                        Text="BlackBerry MDS Services(4.1)" CssClass="lblsmallFont">
                                                                                    </dx:ASPxCheckBox>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                    <td>
                                                                        <table class="style1">
                                                                            <tr>
                                                                                <td>
                                                                                    <dx:ASPxLabel ID="ASPxLabel14" runat="server" Text="Other Services To Monitor" CssClass="lblsmallFont">
                                                                                    </dx:ASPxLabel>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    <dx:ASPxMemo ID="OtherServicesToMonitorTextBox" runat="server" Height="171px" 
                                                                                        Width="170px">
                                                                                    </dx:ASPxMemo>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
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
                                                <dx:ASPxRoundPanel ID="ASPxRoundPanel5" runat="server" 
                                                    CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass" 
                                                    GroupBoxCaptionOffsetY="-24px" HeaderText="Remote Services" Height="53px" 
                                                    SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" Width="600px">
                                                    <ContentPaddings PaddingBottom="10px" PaddingLeft="4px" PaddingTop="10px" />
<ContentPaddings PaddingLeft="4px" PaddingTop="10px" PaddingBottom="10px"></ContentPaddings>

                                                    <HeaderStyle Height="23px">
                                                    <Paddings PaddingBottom="0px" PaddingLeft="2px" PaddingTop="0px" />
<Paddings PaddingLeft="2px" PaddingTop="0px" PaddingBottom="0px"></Paddings>
                                                    </HeaderStyle>
                                                    <PanelCollection>
                                                        <dx:PanelContent ID="PanelContent8" runat="server" SupportsDisabledAttribute="True">
                                                            <table class="style1">
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="Label1" runat="server" CssClass="lblsmallFont" 
                                                                            Text="The following services may be run on separate machine. If this is the case, please enter the IP or Hostname as appropriate. Leave blank if running on the same machine as the services above."></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <table class="style1">
                                                                            <tr>
                                                                                <td>
                                                                                    <dx:ASPxCheckBox ID="BlackBerryAttachmentServiceCheckBox" runat="server" 
                                                                                        CheckState="Unchecked" CssClass="lblsmallFont" 
                                                                                        Text="BlackBerry Attachment Service">
                                                                                    </dx:ASPxCheckBox>
                                                                                </td>
                                                                                <td>
                                                                                    <dx:ASPxTextBox ID="BlackBerryAttachmTextBox" runat="server" Width="170px">
                                                                                    </dx:ASPxTextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    <dx:ASPxCheckBox ID="BlackBerryAlertServiceCheckBox" runat="server" 
                                                                                        CheckState="Unchecked" CssClass="lblsmallFont" Text="BlackBerry Alert Service">
                                                                                    </dx:ASPxCheckBox>
                                                                                </td>
                                                                                <td>
                                                                                    <dx:ASPxTextBox ID="BlackBerryAlertServiceTextBox" runat="server" Width="170px">
                                                                                     
                                                                                    </dx:ASPxTextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    <dx:ASPxCheckBox ID="BlackBerryRouterServiceCheckBox" runat="server" 
                                                                                        CheckState="Unchecked" CssClass="lblsmallFont" Text="BlackBerry Router Service">
                                                                                    </dx:ASPxCheckBox>
                                                                                </td>
                                                                                <td>
                                                                                    <dx:ASPxTextBox ID="BlackBerryRouterserviceTextBox" runat="server" 
                                                                                        Width="170px">
                                                                                    </dx:ASPxTextBox>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </dx:PanelContent>
                                                    </PanelCollection>
                                                </dx:ASPxRoundPanel>
                                            </td>
                                        </tr>
                                    </table>
                                </dx:ContentControl>
                            </ContentCollection>
                        </dx:TabPage>
                        <dx:TabPage Name="Maintenance Windows" Text="Maintenance Windows">
                        <TabImage Url="~/images/application_view_tile.png"/>
<TabImage Url="~/images/application_view_tile.png"></TabImage>
                            <ContentCollection>
                                <dx:ContentControl ID="ContentControl4" runat="server" SupportsDisabledAttribute="True">
                               <%-- <dx:ASPxRoundPanel ID="MaintRoundPanel" runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
                                            CssPostfix="Glass" GroupBoxCaptionOffsetY="-24px" HeaderText="Maintenance Windows"
                                            Height="50px" SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" 
                                            Width="100%">
                                            <ContentPaddings PaddingBottom="10px" PaddingTop="10px" PaddingLeft="4px" />
                                            <ContentPaddings Padding="2px" PaddingTop="10px" PaddingBottom="10px"></ContentPaddings>
                                            <HeaderStyle Height="23px">
                                                <Paddings PaddingBottom="0px" PaddingLeft="2px" PaddingTop="0px" />
                                                <Paddings Padding="0px" PaddingLeft="2px" PaddingRight="2px" PaddingBottom="7px">
                                                </Paddings>
                                            </HeaderStyle>
                                            <PanelCollection>
                                                <dx:PanelContent ID="PanelContent2" runat="server" SupportsDisabledAttribute="True">--%>
                                                    <table width="100%">
                                                        <tr>
                                                            <td>
                                                                <dx:ASPxLabel ID="ASPxLabel29" runat="server" CssClass="lblsmallFont" 
                                                                    Text="Maintenance Windows are times when you do not want the server monitored. You can define maintenance windows using the Hours &amp; Maintenance\Maintenance menu option. Use the Actions column to modify maintenance windows information.">
                                                                </dx:ASPxLabel>
                                                                <dx:ASPxButton ID="ToggleVeiwButton" runat="server" 
                                                                    CssFilePath="~/App_Themes/Office2003Blue/{0}/styles.css" 
                                                                    CssPostfix="Office2003Blue" 
                                                                    SpriteCssFilePath="~/App_Themes/Office2003Blue/{0}/sprite.css" 
                                                                    Text="Switch to Calendar view" Visible="False" Width="178px">
                                                                </dx:ASPxButton>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <dx:ASPxGridView ID="MaintWinListGridView" runat="server" AutoGenerateColumns="False"
                            CssFilePath="~/App_Themes/Office2010Blue/{0}/styles.css" Width="100%" CssPostfix="Office2010Blue"
                            KeyFieldName="ID" Cursor="pointer" EnableTheming="True" OnHtmlRowCreated="MaintWinListGridView_HtmlRowCreated" 
                            OnSelectionChanged="MaintWinListGridView_SelectionChanged" OnPageSizeChanged="MaintWinListGridView_PageSizeChanged"
                            Theme="Office2003Blue" >
                            <Columns>
                                <dx:GridViewCommandColumn ShowInCustomizationForm="True" Visible="False" 
                                    VisibleIndex="0">
                                    <ClearFilterButton Visible="True">
                                    </ClearFilterButton>
                                </dx:GridViewCommandColumn>
                                <dx:GridViewDataTextColumn FieldName="ID" ReadOnly="True" ShowInCustomizationForm="True"
                                    Visible="False" VisibleIndex="1">
                                    <EditFormSettings Visible="False" />
<EditFormSettings Visible="False"></EditFormSettings>
                                    </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn FieldName="ServerID" ReadOnly="True" ShowInCustomizationForm="True"
                                    Visible="False" VisibleIndex="2">
                                    <EditFormSettings Visible="False" />
<EditFormSettings Visible="False"></EditFormSettings>
                                    </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="Name" FieldName="Name" 
                                    ShowInCustomizationForm="True" VisibleIndex="3">
                                    <Settings AutoFilterCondition="Contains" />
<Settings AutoFilterCondition="Contains"></Settings>

                                    <EditCellStyle CssClass="GridCss">
                                    </EditCellStyle>
                                    <EditFormCaptionStyle CssClass="GridCss" HorizontalAlign="Left">
                                    </EditFormCaptionStyle>
                                    <HeaderStyle CssClass="GridCssHeader">
                                    <Paddings Padding="5px" />
<Paddings Padding="5px"></Paddings>
                                    </HeaderStyle>
                                    <CellStyle CssClass="GridCss">
                                    </CellStyle>
                                </dx:GridViewDataTextColumn>

                               
                                <dx:GridViewDataDateColumn FieldName="StartDate" ShowInCustomizationForm="True" 
                                    VisibleIndex="4">
                                    <EditCellStyle CssClass="GridCss">
                                    </EditCellStyle>
                                    <EditFormCaptionStyle CssClass="GridCss">
                                    </EditFormCaptionStyle>
                                    <HeaderStyle CssClass="GridCssHeader" />
                                    <CellStyle CssClass="GridCss">
                                    </CellStyle>
                                </dx:GridViewDataDateColumn>
                                <dx:GridViewDataDateColumn FieldName="StartTime" ShowInCustomizationForm="True" 
                                    VisibleIndex="5">
                                    <EditCellStyle CssClass="GridCss">
                                    </EditCellStyle>
                                    <EditFormCaptionStyle CssClass="GridCss">
                                    </EditFormCaptionStyle>
                                    <HeaderStyle CssClass="GridCssHeader" />
                                    <CellStyle CssClass="GridCss">
                                    </CellStyle>
                                    <PropertiesDateEdit DisplayFormatString="t">
                                    </PropertiesDateEdit>
                                </dx:GridViewDataDateColumn>
                                <dx:GridViewDataTextColumn FieldName="Duration" ShowInCustomizationForm="True" 
                                    VisibleIndex="6">
                                    <EditCellStyle CssClass="GridCss">
                                    </EditCellStyle>
                                    <EditFormCaptionStyle CssClass="GridCss">
                                    </EditFormCaptionStyle>
                                    <HeaderStyle CssClass="GridCssHeader" />
                                    <CellStyle CssClass="GridCss">
                                    </CellStyle>
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataDateColumn FieldName="EndDate" ShowInCustomizationForm="True" 
                                    VisibleIndex="7">
                                    <EditCellStyle CssClass="GridCss">
                                    </EditCellStyle>
                                    <EditFormCaptionStyle CssClass="GridCss">
                                    </EditFormCaptionStyle>
                                    <HeaderStyle CssClass="GridCssHeader" />
                                    <CellStyle CssClass="GridCss">
                                    </CellStyle>
                                </dx:GridViewDataDateColumn>
                                <dx:GridViewDataTextColumn FieldName="MaintType" ReadOnly="True" ShowInCustomizationForm="True"
                                    VisibleIndex="8">
                                    <EditCellStyle CssClass="GridCss">
                                    </EditCellStyle>
                                    <EditFormCaptionStyle CssClass="GridCss">
                                    </EditFormCaptionStyle>
                                    <HeaderStyle CssClass="GridCssHeader" />
                                    <CellStyle CssClass="GridCss">
                                    </CellStyle>
                                </dx:GridViewDataTextColumn>
                            </Columns>
                                                                    <SettingsBehavior AllowSelectByRowClick="True" ColumnResizeMode="NextColumn" 
                                                                        ProcessSelectionChangedOnServer="True" />

<SettingsBehavior AllowSelectByRowClick="True" ColumnResizeMode="NextColumn" ProcessSelectionChangedOnServer="True"></SettingsBehavior>

                                                                    <SettingsPager PageSize="50">
                                                                        <PageSizeItemSettings Visible="True">
                                                                        </PageSizeItemSettings>
                                                                    </SettingsPager>
                                                                    <Settings ShowFilterRow="True" ShowGroupPanel="True" />

<Settings ShowFilterRow="True" ShowGroupPanel="True"></Settings>

                            <Images SpriteCssFilePath="~/App_Themes/Office2010Blue/{0}/sprite.css">
                                <LoadingPanelOnStatusBar Url="~/App_Themes/Office2010Blue/GridView/Loading.gif">
                                </LoadingPanelOnStatusBar>
                                <LoadingPanel Url="~/App_Themes/Office2010Blue/GridView/Loading.gif">
                                </LoadingPanel>
                            </Images>
                            <ImagesFilterControl>
                                <LoadingPanel Url="~/App_Themes/Office2010Blue/GridView/Loading.gif">
                                </LoadingPanel>
                            </ImagesFilterControl>
                            <Styles CssFilePath="~/App_Themes/Office2010Blue/{0}/styles.css" CssPostfix="Office2010Blue">
                                <Header ImageSpacing="5px" SortingImageSpacing="5px">
                                </Header>
                                <GroupRow Font-Bold="True">
                                </GroupRow>
                                <AlternatingRow CssClass="GridAltRow">
                                </AlternatingRow>
                                <LoadingPanel ImageSpacing="5px">
                                </LoadingPanel>
                            </Styles>
                            <StylesPager>
                                <PageNumber ForeColor="#3E4846">
                                </PageNumber>
                                <Summary ForeColor="#1E395B">
                                </Summary>
                            </StylesPager>
                            <StylesEditors ButtonEditCellSpacing="0">
                                <ProgressBar Height="21px">
                                </ProgressBar>
                            </StylesEditors>
                        </dx:ASPxGridView>
                                                                </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                &nbsp;
                                                            </td>
                                                        </tr>
                                                    </table>
                                               <%-- </dx:PanelContent>
                                            </PanelCollection>
                                        </dx:ASPxRoundPanel>--%>
                                    <%--<table class="style1">
                                        <tr>
                                            <td>
                                                &nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>
                                                &nbsp;</td>
                                        </tr>
                                    </table>--%>
                                </dx:ContentControl>
                            </ContentCollection>
                        </dx:TabPage>
                        <dx:TabPage Text="Alert">
                        <TabImage Url="../images/icons/error.png"></TabImage>
                            <ContentCollection>
                                <dx:ContentControl ID="ContentControl5" runat="server" SupportsDisabledAttribute="True">
                                    <table class="style1">
                                        <tr>
                                            <td>
                                                <dx:ASPxLabel ID="ASPxLabel30" runat="server" 
                                                    Text="The list of available alerts is listed below. In order to add new alerts or configure existing alerts, please use the Alerts\Alert Definitions menu.">
                                                </dx:ASPxLabel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                            <dx:ASPxGridView ID="AlertGridView" runat="server" AutoGenerateColumns="False" 
                                                                                CssFilePath="~/App_Themes/Office2010Blue/{0}/styles.css" 
                                                                                CssPostfix="Office2010Blue" KeyFieldName="ID" 
                                                                                Theme="Office2003Blue" Width="100%" 
                                                    EnableTheming="True" OnPageSizeChanged="AlertGridView_PageSizeChanged">
                                                                                <Columns>
                                                                                    <dx:GridViewDataTextColumn Caption="Name" FieldName="Name" ReadOnly="True" 
                                                                                        ShowInCustomizationForm="True" VisibleIndex="0">
                                                                                        <HeaderStyle CssClass="GridCssHeader" />
                                                                                        <CellStyle CssClass="GridCss">
                                                                                        </CellStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn Caption="Start Time" FieldName="StartTime" 
                                                                                        ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="1">
                                                                                        <HeaderStyle CssClass="GridCssHeader" />
                                                                                        <CellStyle CssClass="GridCss">
                                                                                        </CellStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn Caption="Duration" FieldName="Duration" 
                                                                                        ShowInCustomizationForm="True" VisibleIndex="2" Width="70px">
                                                                                        <Settings AutoFilterCondition="Contains" />
<Settings AutoFilterCondition="Contains"></Settings>

                                                                                        <EditCellStyle CssClass="GridCss">
                                                                                        </EditCellStyle>
                                                                                        <EditFormCaptionStyle CssClass="GridCss" HorizontalAlign="Left">
                                                                                        </EditFormCaptionStyle>
                                                                                        <HeaderStyle CssClass="GridCssHeader">
                                                                                        <Paddings Padding="5px" />
<Paddings Padding="5px"></Paddings>
                                                                                        </HeaderStyle>
                                                                                        <CellStyle CssClass="GridCss">
                                                                                        </CellStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn Caption="To" FieldName="SendTo" 
                                                                                        ShowInCustomizationForm="True" VisibleIndex="3">
                                                                                        <HeaderStyle CssClass="GridCssHeader" />
                                                                                        <CellStyle CssClass="GridCss">
                                                                                        </CellStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn Caption="Cc" FieldName="CopyTo" 
                                                                                        ShowInCustomizationForm="True" VisibleIndex="4">
                                                                                        <PropertiesTextEdit DisplayFormatString="t">
                                                                                        </PropertiesTextEdit>
                                                                                        <EditCellStyle CssClass="GridCss">
                                                                                        </EditCellStyle>
                                                                                        <EditFormCaptionStyle CssClass="GridCss">
                                                                                        </EditFormCaptionStyle>
                                                                                        <HeaderStyle CssClass="GridCssHeader" />
                                                                                        <CellStyle CssClass="GridCss">
                                                                                        </CellStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn Caption="Bcc" FieldName="BlindCopyTo" 
                                                                                        ShowInCustomizationForm="True" VisibleIndex="5">
                                                                                        <PropertiesTextEdit DisplayFormatString="d">
                                                                                        </PropertiesTextEdit>
                                                                                        <EditCellStyle CssClass="GridCss">
                                                                                        </EditCellStyle>
                                                                                        <EditFormCaptionStyle CssClass="GridCss">
                                                                                        </EditFormCaptionStyle>
                                                                                        <HeaderStyle CssClass="GridCssHeader"></HeaderStyle>
                                                                                        <CellStyle CssClass="GridCss">
                                                                                        </CellStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn Caption="Event Type" FieldName="EventName" 
                                                                                        ShowInCustomizationForm="True" VisibleIndex="7" Width="200px">
                                                                                        <HeaderStyle CssClass="GridCssHeader" />
                                                                                        <CellStyle CssClass="GridCss">
                                                                                        </CellStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                </Columns>
                                                                                <SettingsBehavior AllowSelectByRowClick="True" ColumnResizeMode="NextColumn" 
                                                                                    ProcessSelectionChangedOnServer="True" />

<SettingsBehavior AllowSelectByRowClick="True" ColumnResizeMode="NextColumn" ProcessSelectionChangedOnServer="True"></SettingsBehavior>

                                                                                <SettingsPager PageSize="50">
                                                                                    <PageSizeItemSettings Visible="True">
                                                                                    </PageSizeItemSettings>
                                                                                </SettingsPager>
                                                                                <Settings ShowFilterRow="True" ShowGroupPanel="True" />

<Settings ShowFilterRow="True" ShowGroupPanel="True"></Settings>

                                                                                <Images SpriteCssFilePath="~/App_Themes/Office2010Blue/{0}/sprite.css">
                                                                                    <LoadingPanelOnStatusBar Url="~/App_Themes/Office2010Blue/GridView/Loading.gif">
                                                                                    </LoadingPanelOnStatusBar>
                                                                                    <LoadingPanel Url="~/App_Themes/Office2010Blue/GridView/Loading.gif">
                                                                                    </LoadingPanel>
                                                                                </Images>
                                                                                <ImagesFilterControl>
                                                                                    <LoadingPanel Url="~/App_Themes/Office2010Blue/GridView/Loading.gif">
                                                                                    </LoadingPanel>
                                                                                </ImagesFilterControl>
                                                                                <Styles CssFilePath="~/App_Themes/Office2010Blue/{0}/styles.css" 
                                                                                    CssPostfix="Office2010Blue">
                                                                                    <Header ImageSpacing="5px" SortingImageSpacing="5px">
                                                                                    </Header>
                                                                                    <GroupRow Font-Bold="True">
                                                                                    </GroupRow>
                                                                                    <AlternatingRow CssClass="GridAltRow">
                                                                                    </AlternatingRow>
                                                                                    <LoadingPanel ImageSpacing="5px">
                                                                                    </LoadingPanel>
                                                                                </Styles>
                                                                                <StylesPager>
                                                                                    <PageNumber ForeColor="#3E4846">
                                                                                    </PageNumber>
                                                                                    <Summary ForeColor="#1E395B">
                                                                                    </Summary>
                                                                                </StylesPager>
                                                                                <StylesEditors ButtonEditCellSpacing="0">
                                                                                    <ProgressBar Height="21px">
                                                                                    </ProgressBar>
                                                                                </StylesEditors>
                                                                            </dx:ASPxGridView>
                                               </td>
                                        </tr>
                                    </table>
                                </dx:ContentControl>
                            </ContentCollection>
                        </dx:TabPage>
                    </TabPages>
                    <Paddings PaddingLeft="0px" />

<Paddings PaddingLeft="0px"></Paddings>

                    <ContentStyle>
                        <Border BorderColor="#4986A2" />
<Border BorderColor="#4986A2"></Border>
                    </ContentStyle>
                </dx:ASPxPageControl>
            </td>
        </tr>
        <tr>
            <td>
                <div id="errorDiv" runat="server" class="alert alert-danger" style="display: none">Error attempting to update the status table.
                </div>
                <table >
                    <tr>
                        <td>
                            <dx:ASPxButton ID="OKButton" runat="server" Text="OK" CssClass="sysButton"
                                onclick="OKButton_Click"  >
                            </dx:ASPxButton>
                        </td>
                        <td>
                            <dx:ASPxButton ID="CancelButton" runat="server" Text="Cancel" CssClass="sysButton"
                            CausesValidation="False" 
                                OnClick="CancelButton_Click" >
                            </dx:ASPxButton>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
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
                                        <dx:PanelContent ID="PanelContent9" runat="server">
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
    
</asp:Content>
