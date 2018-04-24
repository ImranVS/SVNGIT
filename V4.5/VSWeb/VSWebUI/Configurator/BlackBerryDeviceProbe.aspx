<%@ Page Title="VitalSigns Plus-BlackBerryDeviceProbe" Language="C#" MasterPageFile="~/Site1.Master"AutoEventWireup="true" CodeBehind="BlackBerryDeviceProbe.aspx.cs" Inherits="VSWebUI.WebForm18" %>
<%@ Register assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>






<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style1
        {
            width: 100%;
        }
        </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <dx:ASPxRoundPanel ID="BBRoundPanel" runat="server" 
                                        CssFilePath="~/App_Themes/Glass/{0}/styles.css" 
                                        CssPostfix="Glass" 
                                        GroupBoxCaptionOffsetY="-24px" HeaderText="BlackBerry Device Probe" 
                                        SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" 
                                        Width="700px" Height="250px">
                                        <ContentPaddings PaddingBottom="10px" PaddingTop="10px" PaddingLeft="4px" />
<ContentPaddings PaddingLeft="4px" PaddingTop="10px" PaddingBottom="10px"></ContentPaddings>

                                        <HeaderStyle Height="23px">
                                        <Paddings PaddingBottom="0px" PaddingLeft="2px" PaddingTop="0px" />
<Paddings PaddingLeft="2px" PaddingTop="0px" PaddingBottom="0px"></Paddings>
                                        </HeaderStyle>
                                        <PanelCollection>
                                            <dx:PanelContent ID="PanelContent2" runat="server" SupportsDisabledAttribute="True">
    <table class="style1">
        <tr>
            <td colspan="6">
                <dx:ASPxPageControl Font-Bold="True"  ID="ASPxPageControl1" runat="server" ActiveTabIndex="0" 
                    CssFilePath="~/App_Themes/Glass/{0}/styles.css" 
                    CssPostfix="Glass" 
                    SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" TabSpacing="0px" OnActiveTabChanged="ASPxPageControl1_ActiveTabChanged" 
                   EnableHierarchyRecreation="False">
                    <TabPages>
                        <dx:TabPage Text="Probe Properties ">
                        <TabImage Url="~/images/information.png"/>
<TabImage Url="~/images/information.png"></TabImage>
                            <ContentCollection>
                                <dx:ContentControl runat="server" SupportsDisabledAttribute="True">
                                    <table class="style1">
                                        <tr>
                                            <td>
                                                <table class="style1">
                                                    <tr>
                                                        <td width="40%" valign="top">
                                                            <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" 
                                                                CssFilePath="~/App_Themes/Glass/{0}/styles.css" 
                                                                CssPostfix="Glass" 
                                                                GroupBoxCaptionOffsetY="-24px" 
                                                                SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" 
                                                                 HeaderText="Basics" Width="100%">
                                                                <ContentPaddings PaddingBottom="10px" PaddingTop="10px" PaddingLeft="4px" />
<ContentPaddings PaddingLeft="4px" PaddingTop="10px" PaddingBottom="10px"></ContentPaddings>

                                                                <HeaderStyle Height="23px">
                                                                <Paddings PaddingBottom="0px" PaddingLeft="2px" PaddingTop="0px" />
<Paddings PaddingLeft="2px" PaddingTop="0px" PaddingBottom="0px"></Paddings>
                                                                </HeaderStyle>
                                                                <PanelCollection>
                                                                    <dx:PanelContent runat="server" SupportsDisabledAttribute="True">
                                                                        <table class="style1">
                                                                            <tr>
                                                                                <td>
                                                                                    <dx:ASPxLabel ID="ASPxLabel1" runat="server" CssClass="lblsmallFont" 
                                                                                        Text="Name:" Wrap="False">
                                                                                    </dx:ASPxLabel>
                                                                                </td>
                                                                                <td>
                                                                                    <dx:ASPxTextBox ID="NameTextBox" runat="server">
                                                                                        <ValidationSettings ErrorDisplayMode="None" SetFocusOnError="True">
                                                                                            <RequiredField ErrorText="please enter Name" IsRequired="True" />
                                                                                        </ValidationSettings>
                                                                                    </dx:ASPxTextBox>
                                                                                </td>
                                                                                <td>
                                                                                    <dx:ASPxCheckBox ID="EnabledforScanningCheckBox" runat="server" 
                                                                                        CheckState="Unchecked" CssClass="lblsmallFont" Text="Enabled for Scanning" 
                                                                                        Wrap="False">
                                                                                    </dx:ASPxCheckBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    <dx:ASPxLabel ID="ASPxLabel2" runat="server" CssClass="lblsmallFont" 
                                                                                        Text="Category:" Wrap="False">
                                                                                    </dx:ASPxLabel>
                                                                                </td>
                                                                                <td>
                                                                                    <dx:ASPxTextBox ID="CategoryTextBox" runat="server">
                                                                                        <ValidationSettings ErrorDisplayMode="None" SetFocusOnError="True">
                                                                                            <RequiredField ErrorText="please enter category" IsRequired="True" />
                                                                                        </ValidationSettings>
                                                                                    </dx:ASPxTextBox>
                                                                                </td>
                                                                                <td>
                                                                                    &nbsp;
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </dx:PanelContent>
                                                                </PanelCollection>
                                                            </dx:ASPxRoundPanel>
                                                        </td>
                                                        <td width="60%" valign="top">
                                                            <dx:ASPxRoundPanel ID="ASPxRoundPanel2" runat="server" 
                                                                CssFilePath="~/App_Themes/Glass/{0}/styles.css" 
                                                                CssPostfix="Glass" 
                                                                GroupBoxCaptionOffsetY="-24px" HeaderText="Scan Settings" 
                                                                SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" Width="100%" 
                                                                >
                                                                <ContentPaddings PaddingBottom="10px" PaddingTop="10px" PaddingLeft="4px" />
<ContentPaddings PaddingLeft="4px" PaddingTop="10px" PaddingBottom="10px"></ContentPaddings>

                                                                <HeaderStyle Height="23px">
                                                                <Paddings PaddingBottom="0px" PaddingLeft="2px" PaddingTop="0px" />
<Paddings PaddingLeft="2px" PaddingTop="0px" PaddingBottom="0px"></Paddings>
                                                                </HeaderStyle>
                                                                <PanelCollection>
                                                                    <dx:PanelContent runat="server" SupportsDisabledAttribute="True">
                                                                        <table class="style1">
                                                                            <tr>
                                                                                <td>
                                                                                    <dx:ASPxLabel ID="ASPxLabel3" runat="server" Text="Scan Interval:" 
                                                                                        CssClass="lblsmallFont" Wrap="False">
                                                                                    </dx:ASPxLabel>
                                                                                </td>
                                                                                <td>
                                                                                    <dx:ASPxTextBox ID="ScanIntervalTextBox" runat="server" Width="40px" 
                                                                                        Height="19px">
                                                                                        <MaskSettings Mask="&lt;0..99999&gt;" />
<MaskSettings Mask="&lt;0..99999&gt;"></MaskSettings>

                                                                                        <ValidationSettings SetFocusOnError="True" errordisplaymode="ImageWithTooltip" 
                                                                                            CausesValidation="True">
<RequiredField IsRequired="True" ErrorText="please  Enter ScanInterval "></RequiredField>
                                                                                            <RegularExpression ErrorText="Please enter a numeric value using the numbers only (0-9)." 
                                                                                                ValidationExpression="^\d+$" />
                                                                                            <RequiredField IsRequired="True" ErrorText="please  Enter ScanInterval " />
                                                                                        </ValidationSettings>
                                                                                    </dx:ASPxTextBox>
                                                                                </td>
                                                                                <td>
                                                                                    <dx:ASPxLabel ID="ASPxLabel10" runat="server" Text="minutes" CssClass="lblsmallFont">
                                                                                    </dx:ASPxLabel>
                                                                                </td>
                                                                                <td>
                                                                                    <dx:ASPxLabel ID="ASPxLabel5" runat="server" Text="Off-Hours Scan Interval:" 
                                                                                        CssClass="lblsmallFont" Wrap="False">
                                                                                    </dx:ASPxLabel>
                                                                                </td>
                                                                                <td>
                                                                                    <dx:ASPxTextBox ID="offHoursscanIntervalTextBox" runat="server" Width="40px">
                                                                                        <MaskSettings Mask="&lt;0..99999&gt;" />
<MaskSettings Mask="&lt;0..99999&gt;"></MaskSettings>

                                                                                        <ValidationSettings SetFocusOnError="True" errordisplaymode="ImageWithTooltip" 
                                                                                            CausesValidation="True">
<RequiredField IsRequired="True" ErrorText="please Enter offhoursscaninterval"></RequiredField>
                                                                                            <RegularExpression ErrorText="Please enter a numeric value using the numbers only (0-9)." 
                                                                                                ValidationExpression="^\d+$" />
                                                                                            <RequiredField IsRequired="True" 
                                                                                                ErrorText="please Enter offhoursscaninterval" />
                                                                                        </ValidationSettings>
                                                                                    </dx:ASPxTextBox>
                                                                                </td>
                                                                                <td>
                                                                                    <dx:ASPxLabel ID="ASPxLabel7" runat="server" Text="minutes" CssClass="lblsmallFont">
                                                                                    </dx:ASPxLabel>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    <dx:ASPxLabel ID="ASPxLabel4" runat="server" Text="Retry Interval:" 
                                                                                        CssClass="lblsmallFont" Wrap="False">
                                                                                    </dx:ASPxLabel>
                                                                                </td>
                                                                                <td>
                                                                                    <dx:ASPxTextBox ID="RetryIntervalTextBox" runat="server" Width="40px">
                                                                                        <MaskSettings Mask="&lt;0..99999&gt;" />
<MaskSettings Mask="&lt;0..99999&gt;"></MaskSettings>

                                                                                        <ValidationSettings SetFocusOnError="True" errordisplaymode="ImageWithTooltip" 
                                                                                            CausesValidation="True">
<RequiredField IsRequired="True" ErrorText="Please Enter RetryInterval  "></RequiredField>
                                                                                            <RegularExpression ErrorText="Please enter a numeric value using the numbers only (0-9)." 
                                                                                                ValidationExpression="^\d+$" />
                                                                                            <RequiredField IsRequired="True" ErrorText="Please Enter RetryInterval  " />
                                                                                        </ValidationSettings>
                                                                                    </dx:ASPxTextBox>
                                                                                </td>
                                                                                <td>
                                                                                    <dx:ASPxLabel ID="ASPxLabel9" runat="server" Text="minutes" CssClass="lblsmallFont">
                                                                                    </dx:ASPxLabel>
                                                                                </td>
                                                                                <td>
                                                                                    <dx:ASPxLabel ID="ASPxLabel6" runat="server" Text="Delivery Threshold:" 
                                                                                        CssClass="lblsmallFont">
                                                                                    </dx:ASPxLabel>
                                                                                </td>
                                                                                <td>
                                                                                    <dx:ASPxTextBox ID="DeliveryThresholdTextBox" runat="server" Width="40px">
                                                                                        <MaskSettings Mask="&lt;0..99999&gt;" />
<MaskSettings Mask="&lt;0..99999&gt;"></MaskSettings>

                                                                                        <ValidationSettings SetFocusOnError="True" errordisplaymode="ImageWithTooltip" 
                                                                                            CausesValidation="True">
<RequiredField IsRequired="True" ErrorText="please enter Deliverythershold"></RequiredField>
                                                                                            <RegularExpression ErrorText="Please enter a numeric value using the numbers only (0-9)." 
                                                                                                ValidationExpression="^\d+$" />
                                                                                            <RequiredField IsRequired="True" ErrorText="please enter Deliverythershold" />
                                                                                        </ValidationSettings>
                                                                                    </dx:ASPxTextBox>
                                                                                </td>
                                                                                <td>
                                                                                    <dx:ASPxLabel ID="ASPxLabel8" runat="server" Text="minutes" CssClass="lblsmallFont">
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
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <table class="style1">
                                                    <tr>
                                                        <td width="33%" valign="top">
                                                            <dx:ASPxRoundPanel ID="ASPxRoundPanel3" runat="server" 
                                                                CssFilePath="~/App_Themes/Glass/{0}/styles.css" 
                                                                CssPostfix="Glass" 
                                                                GroupBoxCaptionOffsetY="-24px" 
                                                                SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" 
                                                                HeaderText="Step 1: Send message to BlackBerry Device">
                                                                <ContentPaddings PaddingBottom="10px" PaddingTop="10px" PaddingLeft="4px" />
<ContentPaddings PaddingLeft="4px" PaddingTop="10px" PaddingBottom="10px"></ContentPaddings>

                                                                <HeaderStyle Height="23px">
                                                                <Paddings PaddingBottom="0px" PaddingLeft="2px" PaddingTop="0px" />
<Paddings PaddingLeft="2px" PaddingTop="0px" PaddingBottom="0px"></Paddings>
                                                                </HeaderStyle>
                                                                <PanelCollection>
                                                                    <dx:PanelContent runat="server" SupportsDisabledAttribute="True">
                                                                        <table class="style1">
                                                                            <tr>
                                                                                <td>
                                                                                    
                                                                                    <asp:Label ID="Label3" runat="server" 
                                                                                        Text="Test messages will be sent to this NotesMail address, which must be associated with an &lt;b&gt;actual&lt;/b&gt; BlackBerry device." 
                                                                                        CssClass="lblsmallFont"></asp:Label>
                                                                                    
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    &nbsp;</td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    <dx:ASPxLabel ID="ASPxLabel11" runat="server" 
                                                                                        Text="Devices Notes Mail Address:" CssClass="lblsmallFont">
                                                                                    </dx:ASPxLabel>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    <dx:ASPxTextBox ID="DevicesNotesMailAddressTextBox" runat="server" 
                                                                                        Width="170px">
                                                                                        <ValidationSettings>
                                                                                        <RequiredField IsRequired="True" ErrorText="Enter Notes mail Address"></RequiredField>
                                                                                        </ValidationSettings>
                                                                                    </dx:ASPxTextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    <dx:ASPxLabel ID="ASPxLabel12" runat="server" 
                                                                                        Text="Devices Internet Mail Address:" CssClass="lblsmallFont">
                                                                                    </dx:ASPxLabel>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    <dx:ASPxTextBox ID="InternetMailAddress" runat="server" Width="170px">
                                                                                        <ValidationSettings>
                                                                                            <RequiredField ErrorText="Enter mail Address" IsRequired="True" />
                                                                                            <RegularExpression ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" />
<RegularExpression ValidationExpression="\w+([-+.&#39;]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></RegularExpression>

<RequiredField IsRequired="True" ErrorText="Enter mail Address"></RequiredField>
                                                                                        </ValidationSettings>
                                                                                    </dx:ASPxTextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    <dx:ASPxLabel ID="ASPxLabel19" runat="server" Text="Source Server:">
                                                                                    </dx:ASPxLabel>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    <dx:ASPxComboBox ID="SourceComboBox" runat="server">
                                                                                        <ValidationSettings SetFocusOnError="true">
                                                                                            <RequiredField IsRequired="True" ErrorText="Select Source Server" />
<RequiredField IsRequired="True" ErrorText="Select Source Server"></RequiredField>
                                                                                        </ValidationSettings>
                                                                                    </dx:ASPxComboBox>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </dx:PanelContent>
                                                                </PanelCollection>
                                                            </dx:ASPxRoundPanel>
                                                        </td>
                                                        <td width="33%" valign="top">
                                                            <dx:ASPxRoundPanel ID="ASPxRoundPanel7" runat="server" 
                                                                CssFilePath="~/App_Themes/Glass/{0}/styles.css" 
                                                                CssPostfix="Glass" 
                                                                GroupBoxCaptionOffsetY="-24px" HeaderText="Step 2: Check for NotesMail Message" 
                                                                Height="192px" SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" 
                                                               >
                                                                <ContentPaddings PaddingBottom="10px" PaddingTop="10px" PaddingLeft="4px" />
<ContentPaddings PaddingLeft="4px" PaddingTop="10px" PaddingBottom="10px"></ContentPaddings>

                                                                <HeaderStyle Height="23px">
                                                                <Paddings PaddingBottom="0px" PaddingLeft="2px" PaddingTop="0px" />
<Paddings PaddingLeft="2px" PaddingTop="0px" PaddingBottom="0px"></Paddings>
                                                                </HeaderStyle>
                                                                <PanelCollection>
                                                                    <dx:PanelContent runat="server" SupportsDisabledAttribute="True">
                                                                        <table class="style1">
                                                                            <tr>
                                                                                <td>
                                                                                    
                                                                                    <asp:Label ID="Label2" runat="server" 
                                                                                        
                                                                                        Text="Test messages should show up in this mail file, and then get forwarded by the BES server to the device." 
                                                                                        CssClass="lblsmallFont"></asp:Label>
                                                                                    
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    &nbsp;</td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    <dx:ASPxLabel ID="ASPxLabel13" runat="server" 
                                                                                        Text="Look for the Message on this  server: " CssClass="lblsmallFont">
                                                                                    </dx:ASPxLabel>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    <dx:ASPxComboBox ID="TargetserverComboBox" runat="server" >
                                                                                       
                                                                                        <ValidationSettings SetFocusOnError="true">
                                                                                            <RequiredField IsRequired="True" ErrorText="Select Server" />
<RequiredField IsRequired="True" ErrorText="Select Server"></RequiredField>
                                                                                        </ValidationSettings>
                                                                                    </dx:ASPxComboBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    <dx:ASPxLabel ID="ASPxLabel14" runat="server" 
                                                                                        Text="Look for the Message in this Database:" CssClass="lblsmallFont">
                                                                                    </dx:ASPxLabel>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    <dx:ASPxTextBox ID="LookfortheDatabaseTextBox" runat="server" Width="170px">
                                                                                    </dx:ASPxTextBox>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </dx:PanelContent>
                                                                </PanelCollection>
                                                            </dx:ASPxRoundPanel>
                                                        </td>
                                                        <td width="33%" valign="top">
                                                            <dx:ASPxRoundPanel ID="ASPxRoundPanel8" runat="server" 
                                                                CssFilePath="~/App_Themes/Glass/{0}/styles.css" 
                                                                CssPostfix="Glass" 
                                                                GroupBoxCaptionOffsetY="-24px" 
                                                                HeaderText="Step 3: Check for RIM Delivery Confirmation" 
                                                                SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" >
                                                                <ContentPaddings PaddingBottom="10px" PaddingTop="10px" PaddingLeft="4px" />
<ContentPaddings PaddingLeft="4px" PaddingTop="10px" PaddingBottom="10px"></ContentPaddings>

                                                                <HeaderStyle Height="23px">
                                                                <Paddings PaddingBottom="0px" PaddingLeft="2px" PaddingTop="0px" />
<Paddings PaddingLeft="2px" PaddingTop="0px" PaddingBottom="0px"></Paddings>
                                                                </HeaderStyle>
                                                                <PanelCollection>
                                                                    <dx:PanelContent runat="server" SupportsDisabledAttribute="True">
                                                                        <table class="style1">
                                                                            <tr>
                                                                                <td>
                                                                                    
                                                                                    <asp:Label ID="Label1" runat="server" 
                                                                                        
                                                                                        Text="Once the message delivered to the device, the RIM Network will send a 'Delivery Confirmation' message back to the sender. The sender is the Notes ID used by VitalSigns." 
                                                                                        CssClass="lblsmallFont"></asp:Label>
                                                                                    
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    &nbsp;</td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    <dx:ASPxLabel ID="ASPxLabel17" runat="server" CssClass="lblsmallFont" 
                                                                                        Text="Look for the Message on this  server:">
                                                                                    </dx:ASPxLabel>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    <dx:ASPxComboBox ID="confserverComboBox" runat="server">
                                                                                        <ValidationSettings ErrorText="Select">
                                                                                            <RequiredField IsRequired="True" />
<RequiredField IsRequired="True" ErrorText="Select Server"></RequiredField>
                                                                                        </ValidationSettings>
                                                                                    </dx:ASPxComboBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    <dx:ASPxLabel ID="ASPxLabel18" runat="server" 
                                                                                        Text="Look for the Message in this Database:" CssClass="lblsmallFont">
                                                                                    </dx:ASPxLabel>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    <dx:ASPxTextBox ID="LookfortheMessageDatabaseTextBox" runat="server" 
                                                                                        Width="170px">
                                                                                    </dx:ASPxTextBox>
                                                                                </td>
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
                                    </table>
                                </dx:ContentControl>
                            </ContentCollection>
                        </dx:TabPage>
                        <dx:TabPage Text="Maintenance Window">
                         <TabImage Url="~/images/application_view_tile.png" />
<TabImage Url="~/images/application_view_tile.png"></TabImage>
                            <ContentCollection>
                                <dx:ContentControl runat="server" SupportsDisabledAttribute="True">
                                <table>
                                                        <tr>
                                                            <td>
                                                                <dx:ASPxLabel ID="ASPxLabel29" runat="server" CssClass="lblsmallFont" 
                                                                    Text="Maintenance Windows are times when you do not want the server monitored. You can define maintenance windows using the Hours &amp; Maintenance\Maintenance menu option. Use the Actions column to modify maintenance windows information.">
                                                                </dx:ASPxLabel>
                                                                <dx:ASPxButton ID="ToggleVeiwButton" runat="server" 
                                                                    CssFilePath="~/App_Themes/Office2010Blue/{0}/styles.css" 
                                                                    CssPostfix="Office2010Blue" 
                                                                    SpriteCssFilePath="~/App_Themes/Office2010Blue/{0}/sprite.css" 
                                                                    Text="Switch to Calendar view" Visible="False" Width="178px">
                                                                </dx:ASPxButton>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <dx:ASPxGridView ID="MaintWinListGridView" runat="server" AutoGenerateColumns="False"
                            CssFilePath="~/App_Themes/Office2010Blue/{0}/styles.css" Width="100%" CssPostfix="Office2010Blue"
                            KeyFieldName="ID" Cursor="pointer" EnableTheming="True" OnHtmlRowCreated="MaintWinListGridView_HtmlRowCreated" 
                                                                    OnSelectionChanged="MaintWinListGridView_SelectionChanged" 
                                                                    OnPageSizeChanged ="MaintWinListGridView_PageSizeChanged" Theme="Office2003Blue" >
                            <Columns>
                                <dx:GridViewDataTextColumn FieldName="ID" ReadOnly="True" ShowInCustomizationForm="True"
                                    Visible="False" VisibleIndex="0">
                                    <EditFormSettings Visible="False" />
<EditFormSettings Visible="False"></EditFormSettings>
                                    </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn FieldName="ServerID" ReadOnly="True" ShowInCustomizationForm="True"
                                    Visible="False" VisibleIndex="1">
                                    <EditFormSettings Visible="False" />
<EditFormSettings Visible="False"></EditFormSettings>
                                    </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="Name" FieldName="Name" 
                                    ShowInCustomizationForm="True" VisibleIndex="2">
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

                               
                                <dx:GridViewDataDateColumn FieldName="StartDate" ShowInCustomizationForm="True" VisibleIndex="3">
                                    <EditCellStyle CssClass="GridCss">
                                    </EditCellStyle>
                                    <EditFormCaptionStyle CssClass="GridCss">
                                    </EditFormCaptionStyle>
                                    <HeaderStyle CssClass="GridCssHeader" />
                                    <CellStyle CssClass="GridCss">
                                    </CellStyle>
                                </dx:GridViewDataDateColumn>
                                <dx:GridViewDataDateColumn FieldName="StartTime" ShowInCustomizationForm="True" VisibleIndex="4">
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
                                <dx:GridViewDataTextColumn FieldName="Duration" ShowInCustomizationForm="True" VisibleIndex="5">
                                    <EditCellStyle CssClass="GridCss">
                                    </EditCellStyle>
                                    <EditFormCaptionStyle CssClass="GridCss">
                                    </EditFormCaptionStyle>
                                    <HeaderStyle CssClass="GridCssHeader" />
                                    <CellStyle CssClass="GridCss">
                                    </CellStyle>
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataDateColumn FieldName="EndDate" ShowInCustomizationForm="True" VisibleIndex="6">
                                    <EditCellStyle CssClass="GridCss">
                                    </EditCellStyle>
                                    <EditFormCaptionStyle CssClass="GridCss">
                                    </EditFormCaptionStyle>
                                    <HeaderStyle CssClass="GridCssHeader" />
                                    <CellStyle CssClass="GridCss">
                                    </CellStyle>
                                </dx:GridViewDataDateColumn>
                                <dx:GridViewDataTextColumn FieldName="MaintType" ReadOnly="True" ShowInCustomizationForm="True"
                                    VisibleIndex="7">
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

                                                                    <SettingsPager>
                                                                        <PageSizeItemSettings Visible="True">
                                                                        </PageSizeItemSettings>
                                                                    </SettingsPager>
                                                                    <Settings ShowGroupPanel="True" />

<Settings ShowGroupPanel="True"></Settings>

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
                                <AlternatingRow CssClass="GridAltRow">
                                </AlternatingRow>
                                <LoadingPanel ImageSpacing="5px">
                                </LoadingPanel>
                                <GroupRow Font-Bold="True">
                                </GroupRow>
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

                                <!--
                                <dx:ASPxRoundPanel ID="MaintRoundPanel" runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
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
                                                <dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">
                                                    
                                                </dx:PanelContent>
                                            </PanelCollection>
                                        </dx:ASPxRoundPanel>
                                        -->
                                </dx:ContentControl>
                            </ContentCollection>
                        </dx:TabPage>
                        <dx:TabPage Text="Alert">
                        <TabImage Url="../images/icons/error.png"></TabImage>
                            <ContentCollection>
                                <dx:ContentControl runat="server" SupportsDisabledAttribute="True">
                                    <table>
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
                                                                                Theme="Office2003Blue" Width="100%" OnPageSizeChanged="AlertGridView_PageSizeChanged"
                                                    EnableTheming="True">
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
                                                                                    <dx:GridViewDataTextColumn Caption="Day(s)" FieldName="Day" 
                                                                                        ShowInCustomizationForm="True" VisibleIndex="9">
                                                                                        <HeaderStyle CssClass="GridCssHeader" />
                                                                                        <CellStyle CssClass="GridCss">
                                                                                        </CellStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn Caption="Duration" FieldName="Duration" 
                                                                                        ShowInCustomizationForm="True" VisibleIndex="2" Width="70px">
                                                                                        <HeaderStyle CssClass="GridCssHeader" />
                                                                                        <CellStyle CssClass="GridCss">
                                                                                        </CellStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                </Columns>
                                                                                <SettingsBehavior AllowSelectByRowClick="True" ColumnResizeMode="NextColumn" 
                                                                                    ProcessSelectionChangedOnServer="True" />

<SettingsBehavior AllowSelectByRowClick="True" ColumnResizeMode="NextColumn" ProcessSelectionChangedOnServer="True"></SettingsBehavior>

                                                                                <SettingsPager>
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
                    <LoadingPanelImage Url="~/App_Themes/Glass/Web/Loading.gif">
                    </LoadingPanelImage>
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
                 <table>
                        <tr>
                            <td>
                                <dx:ASPxButton ID="FormOkButton" runat="server" Text="OK" CssFilePath="~/App_Themes/Office2010Blue/{0}/styles.css"
                                    CssPostfix="Office2010Blue" SpriteCssFilePath="~/App_Themes/Office2010Blue/{0}/sprite.css"
                                    Width="76px" Height="29px" onclick="FormOkButton_Click">
                                </dx:ASPxButton>
                            </td>
                            <td>
                                <dx:ASPxButton ID="FormCancelButton" runat="server" Text="Cancel" CssFilePath="~/App_Themes/Office2010Blue/{0}/styles.css"
                                    CssPostfix="Office2010Blue" SpriteCssFilePath="~/App_Themes/Office2010Blue/{0}/sprite.css"
                                    Width="76px" Height="29px" onclick="FormCancelButton_Click" 
                                    CausesValidation="False">
                                </dx:ASPxButton>
                            </td>
                        </tr>
                    </table>
                    <asp:HiddenField ID="NotesMailAddressID" runat="server" />
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
                    </td>        
        </tr>
    </table>

    </dx:PanelContent>
                                        </PanelCollection>
                                    </dx:ASPxRoundPanel>
</asp:Content>
