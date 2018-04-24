<%@ Page Title="VitalSigns Plus-Edit NotesMailProbe" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true"
    CodeBehind="EditNotesMailProbe.aspx.cs" Inherits="VSWebUI.Configurator.EditNotesMailProbe" %>

<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>


<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx1" %>




	
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<link href='http://fonts.googleapis.com/css?family=Francois One' rel='stylesheet' type='text/css' />
<link rel="stylesheet" type="text/css" href="../css/vswebforms.css" />
<script src="../js/jquery-1.9.1.js" type="text/javascript"></script>
    <script src="../js/bootstrap.min.js" type="text/javascript"></script>
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
    <style type="text/css">
        .style1
        {
            width: 100%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<table width="100%" >
<tr>
    <td>
        <div class="header" id="lblServer" runat="server">NotesMail Probe</div>
    </td>
    <td valign="top" align="right">
                <table id="gearmenu" runat="server" visible="false">
                    <tr>
                        <td>
                            <dx:ASPxMenu ID="ASPxMenu1" runat="server" EnableTheming="True" 
                                HorizontalAlign="Right"  onitemclick="ASPxMenu1_ItemClick" ShowAsToolbar="True" 
                                Theme="Moderno">
                                <ClientSideEvents ItemClick="OnItemClick" />
                                <Items>
                                    <dx:MenuItem Name="MainItem">
                                        <Items>
                                
                                            <dx:MenuItem Name="ServerDetailsPage" Text="View Details in Dashboard">
                                            </dx:MenuItem>
                               
                                        </Items>
                                        <Image Url="~/images/icons/Gear.png">
                                        </Image>
                                    </dx:MenuItem>
                                </Items>
                            </dx:ASPxMenu>
                        </td>
                    </tr>
                </table>
    </td>
</tr>
<tr>
    <td colspan="2">
        <dx1:ASPxPageControl ID="ASPxPageControl1" runat="server" ActiveTabIndex="0" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
                                CssPostfix="Glass" SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" 
                                TabSpacing="0px" Width="100%" 
                                EnableHierarchyRecreation="False">
                                <TabPages>
                                    <dx1:TabPage Text="General">
<TabImage Url="~/images/information.png"></TabImage>
                                    <TabImage Url="~/images/information.png"/>
                                        <TabStyle Font-Bold="True">
                                        </TabStyle>
                                        <ContentCollection>
                                            <dx:ContentControl ID="ContentControl1" runat="server" SupportsDisabledAttribute="True">
                                                <table width="100%">
                                                    <tr>
                                                        <td colspan="2">
                                                            <dx:ASPxRoundPanel ID="GeneralAttrRoundPanel" runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
                                                                CssPostfix="Glass" GroupBoxCaptionOffsetY="-24px" HeaderText="General Attributes"
                                                                SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" Width="100%">
                                                                <ContentPaddings PaddingBottom="10px" PaddingLeft="4px" PaddingTop="10px" />
<ContentPaddings PaddingLeft="4px" PaddingTop="10px" PaddingBottom="10px"></ContentPaddings>

                                                                <HeaderStyle Height="23px">
                                                                </HeaderStyle>
                                                                <PanelCollection>
                                                                    <dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">
                                                                        <table width="100%">
                                                                            <tr>
                                                                                <td>
                                                                                    <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="Name:" 
                                                                                        CssClass="lblsmallFont">
                                                                                    </dx:ASPxLabel>
                                                                                </td>
                                                                                <td>
                                                                                    <dx:ASPxTextBox ID="NameTextBox" runat="server" 
                                                                                        OnTextChanged="NameTextBox_TextChanged">
                                                                                        <ValidationSettings ErrorDisplayMode="ImageWithTooltip">
                                                                                            <RequiredField IsRequired="True" />
<RequiredField IsRequired="True"></RequiredField>
                                                                                        </ValidationSettings>
                                                                                    </dx:ASPxTextBox>
                                                                                </td>
                                                                                <td>
                                                                                    <dx:ASPxCheckBox ID="EnableCheckBox" runat="server" CheckState="Unchecked" 
                                                                                        Text="Enabled for scanning" CssClass="lblsmallFont">
                                                                                    </dx:ASPxCheckBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    <dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="Category:" 
                                                                                        CssClass="lblsmallFont">
                                                                                    </dx:ASPxLabel>
                                                                                </td>
                                                                                <td>
                                                                                    <dx:ASPxComboBox ID="CategoryComboBox" runat="server">
                                                                                        <Items>
                                                                                            <dx:ListEditItem Text="Domino" Value="Domino" />
                                                                                            <dx:ListEditItem Text="Inbound" Value="Inbound" />
                                                                                            <dx:ListEditItem Text="International" Value="International" />
                                                                                            <dx:ListEditItem Text="Internet Round Trip" Value="Internet Round Trip" />
                                                                                            <dx:ListEditItem Text="Domestic" Value="Domestic" />
                                                                                            <dx:ListEditItem Text="Between Hubs" Value="Between Hubs" />
                                                                                        </Items>
                                                                                        <LoadingPanelStyle ImageSpacing="5px">
                                                                                        </LoadingPanelStyle>
                                                                                        <ButtonStyle Width="13px">
                                                                                        </ButtonStyle>
                                                                                        <ValidationSettings SetFocusOnError="True" ErrorDisplayMode="ImageWithTooltip">
                                                                                            <RequiredField ErrorText="Select Category" IsRequired="True" />
<RequiredField IsRequired="True" ErrorText="Select Category"></RequiredField>
                                                                                        </ValidationSettings>
                                                                                    </dx:ASPxComboBox>
                                                                                </td>
                                                                                <td>
                                                                                    <dx:ASPxCheckBox ID="EchoServiceCheckBox" runat="server" CheckState="Unchecked" 
                                                                                        Text="Send to an Echo Service" 
                                                                                        OnCheckedChanged="EchoServiceCheckBox_CheckedChanged">
                                                                                    </dx:ASPxCheckBox>
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
                                                            <dx:ASPxRoundPanel ID="MsgStartRoundPanel" runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
                                                                CssPostfix="Glass" GroupBoxCaptionOffsetY="-24px" HeaderText="Messsage Start"
                                                                SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" Width="100%" 
                                                                Height="130px">
                                                                <ContentPaddings PaddingBottom="10px" PaddingLeft="4px" PaddingTop="10px" />
<ContentPaddings PaddingLeft="4px" PaddingTop="10px" PaddingBottom="10px"></ContentPaddings>

                                                                <HeaderStyle Height="23px">
                                                                </HeaderStyle>
                                                                <PanelCollection>
                                                                    <dx:PanelContent ID="PanelContent2" runat="server" SupportsDisabledAttribute="True">
                                                                        <table class="style1">
                                                                            <tr>
                                                                                <td>
                                                                                    <dx:ASPxLabel ID="ASPxLabel3" runat="server" Text="Source Server:" 
                                                                                        CssClass="lblsmallFont" Wrap="False">
                                                                                    </dx:ASPxLabel>
                                                                                </td>
                                                                                <td>
                                                                                    <dx:ASPxComboBox ID="SourceServerComboBox" runat="server">
                                                                                        <ValidationSettings SetFocusOnError="True" ErrorDisplayMode="ImageWithTooltip">
                                                                                            <RequiredField ErrorText="Select Server" IsRequired="True" />
<RequiredField IsRequired="True" ErrorText="Select Server"></RequiredField>
                                                                                        </ValidationSettings>
                                                                                    </dx:ASPxComboBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    <dx:ASPxLabel ID="ASPxLabel4" runat="server" Text="Send To Address:" 
                                                                                        CssClass="lblsmallFont" Wrap="False">
                                                                                    </dx:ASPxLabel>
                                                                                </td>
                                                                                <td>
                                                                                    <dx:ASPxTextBox ID="SendToTextBox" runat="server" Width="170px">
                                                                                        <ValidationSettings ErrorDisplayMode="ImageWithTooltip">
                                                                                            <RegularExpression ErrorText="Please Enter Valid Email" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" />
<RegularExpression ErrorText="Please Enter Valid Email" ValidationExpression="\w+([-+.&#39;]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></RegularExpression>
                                                                                        </ValidationSettings>
                                                                                    </dx:ASPxTextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    <dx:ASPxLabel ID="ASPxLabel5" runat="server" Text="Reply To Address:" 
                                                                                        CssClass="lblsmallFont" Wrap="False">
                                                                                    </dx:ASPxLabel>
                                                                                </td>
                                                                                <td>
                                                                                    <dx:ASPxTextBox ID="ReplyToTextBox" runat="server" Width="170px">
                                                                                        <ValidationSettings ErrorDisplayMode="ImageWithTooltip">
                                                                                            <RegularExpression ErrorText="Please Enter Valid Email" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" />
<RegularExpression ErrorText="Please Enter Valid Email" ValidationExpression="\w+([-+.&#39;]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></RegularExpression>
                                                                                        </ValidationSettings>
                                                                                    </dx:ASPxTextBox>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </dx:PanelContent>
                                                                </PanelCollection>
                                                            </dx:ASPxRoundPanel>
                                                            
                                                        </td>
                                                        <td align="left" valign="top">
                                                            <dx:ASPxRoundPanel ID="MsgDestinationRoundPanel" runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
                                                                CssPostfix="Glass" GroupBoxCaptionOffsetY="-24px" HeaderText="Message Destination"
                                                                SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" Width="100%" 
                                                                Height="130px">
                                                                <ContentPaddings PaddingBottom="10px" PaddingLeft="4px" PaddingTop="10px" />
<ContentPaddings PaddingLeft="4px" PaddingTop="10px" PaddingBottom="10px"></ContentPaddings>

                                                                <HeaderStyle Height="23px">
                                                                </HeaderStyle>
                                                                <PanelCollection>
                                                                    <dx:PanelContent ID="PanelContent3" runat="server" SupportsDisabledAttribute="True">
                                                                        <table>
                                                                            <tr>
                                                                                <td>
                                                                                    <dx:ASPxLabel ID="ASPxLabel7" runat="server" Text="Target Server:" 
                                                                                        CssClass="lblsmallFont" Wrap="False">
                                                                                    </dx:ASPxLabel>
                                                                                </td>
                                                                                <td>
                                                                                    <dx:ASPxComboBox ID="TargetComboBox" runat="server">
                                                                                        <LoadingPanelStyle ImageSpacing="5px">
                                                                                        </LoadingPanelStyle>
                                                                                        <ButtonStyle Width="13px">
                                                                                        </ButtonStyle>
                                                                                        <ValidationSettings SetFocusOnError="True" ErrorDisplayMode="ImageWithTooltip">
                                                                                            <RequiredField ErrorText="Select Server" IsRequired="True" />
<RequiredField IsRequired="True" ErrorText="Select Server"></RequiredField>
                                                                                        </ValidationSettings>
                                                                                    </dx:ASPxComboBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    <dx:ASPxLabel ID="ASPxLabel8" runat="server" Text="Target Database Filename:" 
                                                                                        CssClass="lblsmallFont" Wrap="False">
                                                                                    </dx:ASPxLabel>
                                                                                </td>
                                                                                <td>
                                                                                    <dx:ASPxTextBox ID="DestinationTextBox" runat="server" Width="100%">
                                                                                        <ValidationSettings ErrorDisplayMode="ImageWithTooltip">
                                                                                            <RequiredField IsRequired="True" />
<RequiredField IsRequired="True"></RequiredField>
                                                                                        </ValidationSettings>
                                                                                    </dx:ASPxTextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
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
                                                    <tr>
                                                        <td colspan="2">
                                                            <dx:ASPxRoundPanel ID="ScanSettingsRoundPanel" runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
                                                                CssPostfix="Glass" GroupBoxCaptionOffsetY="-24px" HeaderText="Scan Settings"
                                                                SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" Width="100%">
                                                                <ContentPaddings PaddingBottom="10px" PaddingLeft="4px" PaddingTop="10px" />
<ContentPaddings PaddingLeft="4px" PaddingTop="10px" PaddingBottom="10px"></ContentPaddings>

                                                                <HeaderStyle Height="23px">
                                                                </HeaderStyle>
                                                                <PanelCollection>
                                                                    <dx:PanelContent ID="PanelContent4" runat="server" SupportsDisabledAttribute="True">
                                                                        <table>
                                                                            <tr>
                                                                                <td>
                                                                                    <dx:ASPxLabel ID="ASPxLabel9" runat="server" Text="Scan Interval:" 
                                                                                        CssClass="lblsmallFont">
                                                                                    </dx:ASPxLabel>
                                                                                </td>
                                                                                <td>
                                                                                    <dx:ASPxTextBox ID="ScanIntervalTextBox" runat="server" CssClass="txtsmall">
                                                                                        <MaskSettings Mask="&lt;0..99999&gt;" />
<MaskSettings Mask="&lt;0..99999&gt;"></MaskSettings>

                                                                                        <ValidationSettings ErrorDisplayMode="ImageWithTooltip">
                                                                                            <RequiredField IsRequired="True" />
<RequiredField IsRequired="True"></RequiredField>
                                                                                        </ValidationSettings>
                                                                                    </dx:ASPxTextBox>
                                                                                </td>
                                                                                <td>
                                                                                    <dx:ASPxLabel ID="ASPxLabel11" runat="server" Text="minutes" 
                                                                                        CssClass="lblsmallFont">
                                                                                    </dx:ASPxLabel>
                                                                                </td>
                                                                                <td>
                                                                                    &nbsp;
                                                                                </td>
                                                                                <td>
                                                                                    <dx:ASPxLabel ID="ASPxLabel16" runat="server" CssClass="lblsmallFont" 
                                                                                        Text="Delivery Threshold:">
                                                                                    </dx:ASPxLabel>
                                                                                </td>
                                                                                <td>
                                                                                    <dx:ASPxTextBox ID="DeliveryThresholdTextBox" runat="server" 
                                                                                        CssClass="txtsmall">
                                                                                        <MaskSettings Mask="&lt;0..99999&gt;" />
                                                                                        <ValidationSettings ErrorDisplayMode="ImageWithTooltip">
                                                                                            <RequiredField IsRequired="True" />
                                                                                        </ValidationSettings>
                                                                                    </dx:ASPxTextBox>
                                                                                </td>
                                                                                <td>
                                                                                    <dx:ASPxLabel ID="ASPxLabel14" runat="server" CssClass="lblsmallFont" 
                                                                                        Text="minutes">
                                                                                    </dx:ASPxLabel>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    <dx:ASPxLabel ID="ASPxLabel15" runat="server" Text="Off-Hours Scan Interval:" 
                                                                                        CssClass="lblsmallFont">
                                                                                    </dx:ASPxLabel>
                                                                                </td>
                                                                                <td>
                                                                                    <dx:ASPxTextBox ID="offScanTextBox" runat="server" CssClass="txtsmall">
                                                                                        <MaskSettings Mask="&lt;0..9999&gt;" />
                                                                                        <ValidationSettings ErrorDisplayMode="ImageWithTooltip">
                                                                                            <RequiredField IsRequired="True" />
                                                                                        </ValidationSettings>
                                                                                    </dx:ASPxTextBox>
                                                                                </td>
                                                                                <td>
                                                                                    <dx:ASPxLabel ID="ASPxLabel13" runat="server" CssClass="lblsmallFont" 
                                                                                        Text="minutes">
                                                                                    </dx:ASPxLabel>
                                                                                </td>
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
                                                                                    <dx:ASPxLabel ID="ASPxLabel10" runat="server" CssClass="lblsmallFont" 
                                                                                        Text="Retry Interval:">
                                                                                    </dx:ASPxLabel>
                                                                                </td>
                                                                                <td>
                                                                                    <dx:ASPxTextBox ID="RetryTextBox" runat="server" CssClass="txtsmall">
                                                                                        <MaskSettings Mask="&lt;1..99999&gt;" />
                                                                                        <ValidationSettings ErrorDisplayMode="ImageWithTooltip">
                                                                                            <RequiredField IsRequired="True" />
                                                                                        </ValidationSettings>
                                                                                    </dx:ASPxTextBox>
                                                                                </td>
                                                                                <td>
                                                                                    <dx:ASPxLabel ID="ASPxLabel12" runat="server" CssClass="lblsmallFont" 
                                                                                        Text="minutes">
                                                                                    </dx:ASPxLabel>
                                                                                </td>
                                                                                <td>
                                                                                    &nbsp;
                                                                                </td>
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
                                            </dx:ContentControl>
                                        </ContentCollection>
                                    </dx1:TabPage>
                                    <dx1:TabPage Text="Maintenance Windows">
<TabImage Url="~/images/application_view_tile.png"></TabImage>
                                     <TabImage Url="~/images/application_view_tile.png"/>
                                        <TabStyle Font-Bold="True">
                                        </TabStyle>
                                        <ContentCollection>
                                            <dx:ContentControl ID="ContentControl2" runat="server" SupportsDisabledAttribute="True">
                                              
                                                            <table width="100%">
                                                                <tr>
                                                                    <td>
                                                                        <div id="maintDiv" class="info">Maintenance Windows are times when you do not want the server monitored. You can define maintenance windows using the Hours & Maintenance\Maintenance menu option. Use the Actions column to modify maintenance windows information.</div>
                                                                        <dx:ASPxButton ID="ToggleVeiwButton" runat="server" 
                                                                            CssFilePath="~/App_Themes/Office2010Silver/{0}/styles.css" 
                                                                            CssPostfix="Office2010Blue" 
                                                                            SpriteCssFilePath="~/App_Themes/Office2010Silver/{0}/sprite.css" 
                                                                            Text="Switch to Calendar view" Visible="False" Width="178px">
                                                                        </dx:ASPxButton>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <dx:ASPxGridView ID="MaintWinListGridView" runat="server" AutoGenerateColumns="False"
                                                                            CssFilePath="~/App_Themes/Office2010Blue/{0}/styles.css" Width="100%" CssPostfix="Office2010Blue"
                                                                            KeyFieldName="ID" OnHtmlRowCreated="MaintWinListGridView_HtmlRowCreated" 
                                                                            OnSelectionChanged="MaintWinListGridView_SelectionChanged" 
                                                                            EnableTheming="True" Theme="Office2003Blue" Cursor="pointer" OnPageSizeChanged="MaintWinListGridView_PageSizeChanged">
                                                                            <Columns>
                                                                                <dx:GridViewCommandColumn ShowInCustomizationForm="True" Visible="False" 
                                                                                    VisibleIndex="0">
                                                                                    <ClearFilterButton Visible="True">
                                                                                        <Image Url="~/images/clear.png">
                                                                                        </Image>
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
                                                                                    <HeaderStyle CssClass="GridCssHeader" >
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
																					<Settings AllowAutoFilter="False" />

<Settings AllowAutoFilter="False"></Settings>

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
																					<Settings AllowAutoFilter="False" />
<Settings AllowAutoFilter="False"></Settings>

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
                                                                             <SettingsBehavior AllowSelectByRowClick="True" ColumnResizeMode="NextColumn" ProcessSelectionChangedOnServer="True" />
                                                                           

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
                                                     
                                            </dx:ContentControl>
                                        </ContentCollection>
                                    </dx1:TabPage>
                                    <dx1:TabPage Text="Alert">
                                    <TabImage Url="../images/icons/error.png"></TabImage>
                                        <TabStyle Font-Bold="True">
                                        </TabStyle>
                                        <ContentCollection>
                                            <dx:ContentControl ID="ContentControl3" runat="server" SupportsDisabledAttribute="True">
                                                <table class="style1">
                                                    <tr>
                                                        <td>
                                                            <div id="alertDiv" class="info">The list of available alerts is listed below. In order to add new alerts or configure existing alerts, please use the Alerts\Alert Definitions menu.</div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                        <dx:ASPxGridView ID="AlertGridView" runat="server" AutoGenerateColumns="False" 
                                                                                CssFilePath="~/App_Themes/Office2010Blue/{0}/styles.css" 
                                                                                CssPostfix="Office2010Blue" KeyFieldName="ID" 
                                                                                Theme="Office2003Blue" Width="100%" OnPageIndexChanged="AlertGridView_PageSizeChanged">
                                                                                <Columns>
                                                                                    <dx:GridViewDataTextColumn Caption="Name" FieldName="Name" ReadOnly="True" 
                                                                                        ShowInCustomizationForm="True" VisibleIndex="0">
                                                                                        <HeaderStyle CssClass="GridCssHeader" />
                                                                                        <CellStyle CssClass="GridCss">
                                                                                        </CellStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn Caption="Duration" FieldName="Duration" 
                                                                                        ShowInCustomizationForm="True" VisibleIndex="2">
																						<Settings AllowAutoFilter="False" />
<Settings AllowAutoFilter="False"></Settings>

                                                                                        <HeaderStyle CssClass="GridCssHeader" />
                                                                                        <CellStyle CssClass="GridCss">
                                                                                        </CellStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn Caption="Start Time" FieldName="StartTime" 
                                                                                        ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="1">
																						<Settings AllowAutoFilter="False" />
<Settings AllowAutoFilter="False"></Settings>

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
                                                                                    <dx:GridViewDataTextColumn Caption="cc" FieldName="CopyTo" 
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
                                                                                    <dx:GridViewDataTextColumn Caption="Event Type" FieldName="EventName" Width="40%"
                                                                                        ShowInCustomizationForm="True" VisibleIndex="6">
                                                                                        <HeaderStyle CssClass="GridCssHeader" />
                                                                                        <CellStyle CssClass="GridCss">
                                                                                        </CellStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn Caption="Day(s)" FieldName="Day" 
                                                                                        ShowInCustomizationForm="True" VisibleIndex="8">
                                                                                        <HeaderStyle CssClass="GridCssHeader" />
                                                                                        <CellStyle CssClass="GridCss">
                                                                                        </CellStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                </Columns>
                                                                                <SettingsBehavior ColumnResizeMode="NextColumn" />

<SettingsBehavior AllowSelectByRowClick="false" ColumnResizeMode="NextColumn" ProcessSelectionChangedOnServer="false"></SettingsBehavior>

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
                                                                                    <LoadingPanel ImageSpacing="5px">
                                                                                    </LoadingPanel>
                                                                                    <AlternatingRow CssClass="GridAltRow">
                                                                                    </AlternatingRow>
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
                                    </dx1:TabPage>
                                </TabPages>
                                <LoadingPanelImage Url="~/App_Themes/Glass/Web/Loading.gif">
                                </LoadingPanelImage>
                                <Paddings PaddingLeft="0px" />

<Paddings PaddingLeft="0px"></Paddings>

                                <ContentStyle>
                                    <Border BorderColor="#4986A2" />
<Border BorderColor="#4986A2"></Border>
                                </ContentStyle>
                            </dx1:ASPxPageControl>
    </td>
</tr>
<tr>
    <td colspan="2">
                            <div id="errorDiv" class="alert alert-danger" runat="server" style="display: none">Error.
                            </div>
                            <table>
                                <tr>
                                 <td>
                                                    <dx:ASPxButton ID="formOKButton" runat="server" CssClass="sysButton"
                                                        Text="OK" OnClick="formOKButton_Click">
                                                    </dx:ASPxButton>
                                                </td>
                                                <td>
                                                    <dx:ASPxButton ID="formCancelButton" runat="server" CssClass="sysButton"
                                                        Text="Cancel" OnClick="formCancelButton_Click" CausesValidation="False">
                                                    </dx:ASPxButton>
                                                </td>
                                    
                                </tr>
                            </table>
                            
                        </td>
</tr>
</table>
    <dx:ASPxPopupControl ID="ErrorMessagePopupControl" runat="server" AllowDragging="True"
                                ClientInstanceName="pcErrorMessage" CloseAction="CloseButton" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
                                CssPostfix="Glass" EnableAnimation="False" EnableViewState="False" HeaderText="Validation Failure"
                                Height="150px" Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
                                SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" Width="300px">
                                <LoadingPanelImage Url="~/App_Themes/Glass/Web/Loading.gif">
                                </LoadingPanelImage>
                                <HeaderStyle>
                                    <Paddings PaddingLeft="10px" PaddingRight="6px" PaddingTop="1px" />
<Paddings PaddingLeft="10px" PaddingTop="1px" PaddingRight="6px"></Paddings>
                                </HeaderStyle>
                                <ContentCollection>
                                    <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server" SupportsDisabledAttribute="True">
                                        <dx:ASPxPanel ID="Panel1" runat="server" DefaultButton="btOK">
                                            <PanelCollection>
                                                <dx:PanelContent ID="PanelContent5" runat="server" SupportsDisabledAttribute="True">
                                                    <div style="min-height: 70px;">
                                                        <dx:ASPxLabel ID="ErrorMessageLabel" runat="server">
                                                        </dx:ASPxLabel>
                                                    </div>
                                                    <div>
                                                        <table cellpadding="0" cellspacing="0" width="100%">
                                                            <tr>
                                                                <td align="center">
                                                                    <dx:ASPxButton ID="ValidationOkButton" runat="server" AutoPostBack="False" CssFilePath="~/App_Themes/Office2010Blue/{0}/styles.css"
                                                                        CssPostfix="Office2010Blue" SpriteCssFilePath="~/App_Themes/Office2010Blue/{0}/sprite.css"
                                                                        Text="OK" Width="80px">
                                                                        <ClientSideEvents Click="function(s, e) { pcErrorMessage.Hide(); }" />
<ClientSideEvents Click="function(s, e) { pcErrorMessage.Hide(); }"></ClientSideEvents>
                                                                    </dx:ASPxButton>
                                                                    <dx:ASPxButton ID="ValidationUpdatedButton" runat="server" AutoPostBack="False" CssFilePath="~/App_Themes/Office2010Blue/{0}/styles.css"
                                                                        CssPostfix="Office2010Blue" OnClick="ValidationUpdatedButton_Click" SpriteCssFilePath="~/App_Themes/Office2010Blue/{0}/sprite.css"
                                                                        Text="OK" Visible="False" Width="80px">
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
                                                                                    <dx:ASPxLabel runat="server" Text="Attach File:" CssClass="lblsmallFont" 
        ID="ASPxLabel6" Visible="False"></dx:ASPxLabel>


                                                                                    <dx:ASPxUploadControl runat="server" 
        SpriteCssFilePath="~/App_Themes/Office2010Blue/{0}/sprite.css" Width="280px" 
        CssPostfix="Office2010Blue" 
        CssFilePath="~/App_Themes/Office2010Blue/{0}/styles.css" ID="FileUploadControl" 
        Visible="False"></dx:ASPxUploadControl>


                                                                                    <dx:ASPxLabel runat="server" Text="Attachment:" ID="ASPxLabel30" 
        Visible="False"></dx:ASPxLabel>


                                                                                    <dx:ASPxHyperLink runat="server" Text="FilePath" Target="_blank" 
        ID="FilePathHyperLink" Visible="False"></dx:ASPxHyperLink>


                                                                                </asp:Content>
