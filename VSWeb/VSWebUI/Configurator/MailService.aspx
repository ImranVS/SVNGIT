<%@ Page Title="VitalSigns Plus-MailService" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true"
    CodeBehind="MailService.aspx.cs" Inherits="VSWebUI.Configurator.MailService" %>

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
        });
        </script>
    </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<table width="100%">
        <tr>
            <td>
                <div class="header" id="lblServer" runat="server">Mail Service</div>
    <asp:Label ID="lblServerId" runat="server" Font-Bold="False" Visible="False"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <dx:ASPxPageControl Font-Bold="True" ID="ASPxPageControl1" runat="server" ActiveTabIndex="0"
                    CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass" SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css"
                    TabSpacing="0px" Width="100%" EnableHierarchyRecreation="False">
                    <TabPages>
                        <dx:TabPage Text="General">
                         <%--<dx:TabPage Text="Configure">--%>
<TabImage Url="~/images/information.png"></TabImage>
                        <TabImage Url="~/images/information.png"/>
                            <ContentCollection>
                                <dx:ContentControl ID="ContentControl1" runat="server" SupportsDisabledAttribute="True">
                                    <table width="100%">
                                        <tr>
                                            <td colspan="2">
                                                <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" Width="100%" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
                                                    CssPostfix="Glass" SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" HeaderText="Service Attributes"
                                                    GroupBoxCaptionOffsetY="-24px">
                                                    <ContentPaddings PaddingBottom="10px" PaddingLeft="4px" PaddingTop="10px" />
<ContentPaddings PaddingBottom="10px" PaddingLeft="4px" PaddingTop="10px"></ContentPaddings>

                                                    <HeaderStyle Height="23px">
                                                    </HeaderStyle>
                                                    <PanelCollection>
                                                        <dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <dx:ASPxLabel ID="ASPxLabel1" runat="server" CssClass="lblsmallFont" 
                                                                            Text="Name:">
                                                                        </dx:ASPxLabel>
                                                                    </td>
                                                                    <td>
                                                                        <dx:ASPxTextBox ID="MSNameTextBox" runat="server" AutoPostBack="True" 
                                                                            OnTextChanged="MSNameTextBox_TextChanged">
                                                                            <validationsettings>
                                                                                <requiredfield isrequired="True" />
<RequiredField IsRequired="True"></RequiredField>
                                                                            </validationsettings>
                                                                        </dx:ASPxTextBox>
                                                                    </td>
                                                                    <td>
                                                                        <dx:ASPxLabel ID="ASPxLabel4" runat="server" CssClass="lblsmallFont" 
                                                                            Text="Protocol:">
                                                                        </dx:ASPxLabel>
                                                                    </td>
                                                                    <td>
                                                                        <dx:ASPxComboBox ID="ProtocolComboBox" runat="server" AutoPostBack="True" 
                                                                            CssClass="lblsmallFont" 
                                                                            OnSelectedIndexChanged="ProtocolComboBox_SelectedIndexChanged">
                                                                            <Items>
                                                                                <dx:ListEditItem Text="SMTP" Value="SMTP" />
                                                                                <dx:ListEditItem Text="POP3" Value="POP3" />
                                                                                <dx:ListEditItem Text="LDAP" Value="LDAP" />
                                                                                <dx:ListEditItem Text="IMAP" Value="IMAP" />
                                                                                <dx:ListEditItem Text="Other" Value="Other" />
                                                                            </Items>
                                                                            <ValidationSettings ErrorDisplayMode="ImageWithTooltip">
                                                                                <RequiredField IsRequired="True" />
                                                                            </ValidationSettings>
                                                                        </dx:ASPxComboBox>
                                                                    </td>
                                                                    <td>
                                                                        <dx:ASPxCheckBox ID="EnableCheckBox" runat="server" CheckState="Unchecked" 
                                                                            CssClass="lblsmallFont" Text="Enabled  for scanning" Wrap="False">
                                                                        </dx:ASPxCheckBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <dx:ASPxLabel ID="ASPxLabel2" runat="server" CssClass="lblsmallFont" 
                                                                            Text="Address:">
                                                                        </dx:ASPxLabel>
                                                                    </td>
                                                                    <td>
                                                                        <dx:ASPxTextBox ID="AddressTextBox" runat="server">
                                                                            <validationsettings>
                                                                                <requiredfield isrequired="True" />
<RequiredField IsRequired="True"></RequiredField>
                                                                            </validationsettings>
                                                                        </dx:ASPxTextBox>
                                                                    </td>
                                                                    <td>
                                                                        <dx:ASPxLabel ID="ASPxLabel5" runat="server" CssClass="lblsmallFont" 
                                                                            Text="Port:">
                                                                        </dx:ASPxLabel>
                                                                    </td>
                                                                    <td>
                                                                        <dx:ASPxTextBox ID="PortTextBox" runat="server" CssClass="txtsmall">
                                                                            <ValidationSettings CausesValidation="True" ErrorDisplayMode="ImageWithTooltip" 
                                                                                SetFocusOnError="True">
                                                                                <RequiredField IsRequired="true" />
                                                                                <RegularExpression ErrorText="Please enter a numeric value using the numbers only (0-9)." 
                                                                                    ValidationExpression="^\d+$"  />
                                                                            </ValidationSettings>
                                                                        </dx:ASPxTextBox>
                                                                    </td>
                                                                    <td>
                                                                        &nbsp;</td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <dx:ASPxLabel ID="ASPxLabel3" runat="server" CssClass="lblsmallFont" 
                                                                            Text="Description:">
                                                                        </dx:ASPxLabel>
                                                                    </td>
                                                                    <td>
                                                                        <dx:ASPxTextBox ID="DescriptionTextBox" runat="server" AutoPostBack="True" 
                                                                            OnTextChanged="DescriptionTextBox_TextChanged">
                                                                            <validationsettings>
                                                                                <requiredfield isrequired="True" />
<RequiredField IsRequired="True"></RequiredField>
                                                                            </validationsettings>
                                                                        </dx:ASPxTextBox>
                                                                    </td>
                                                                    <td>
                                                                        <dx:ASPxLabel ID="ASPxLabel32" runat="server" CssClass="lblsmallFont" 
                                                                            Text="Location:">
                                                                        </dx:ASPxLabel>
                                                                    </td>
                                                                    <td>
                                                                        <dx:ASPxComboBox ID="LocComboBox" runat="server">
                                                                            <ValidationSettings CausesValidation="True" Display="Dynamic" 
                                                                                ErrorDisplayMode="ImageWithTooltip">
                                                                                <RequiredField ErrorText="Select Location" IsRequired="True" />
                                                                                <ErrorImage ToolTip="Select Location">
                                                                                </ErrorImage>
                                                                            </ValidationSettings>
                                                                        </dx:ASPxComboBox>
                                                                    </td>
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
                                            <td>
                                                <dx:ASPxRoundPanel ID="ASPxRoundPanel2" runat="server" 
                                                    CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass" 
                                                    GroupBoxCaptionOffsetY="-24px" HeaderText="Scan Settings" 
                                                    SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" Width="100%" >
                                                    <ContentPaddings PaddingBottom="10px" PaddingLeft="4px" PaddingTop="10px" />
<ContentPaddings PaddingLeft="4px" PaddingTop="10px" PaddingBottom="10px"></ContentPaddings>

                                                    <HeaderStyle Height="23px">
                                                    </HeaderStyle>
                                                    <PanelCollection>
                                                        <dx:PanelContent ID="PanelContent4" runat="server" SupportsDisabledAttribute="True">
                                                            <table>
                                                                            <tr>
                                                                                <td>
                                                                                    <dx:ASPxLabel ID="ASPxLabel6" runat="server" CssClass="lblsmallFont" 
                                                                                        Text="Scan Interval:">
                                                                                    </dx:ASPxLabel>
                                                                                </td>
                                                                                <td>
                                                                                    <dx:ASPxTextBox ID="ScanIntervalTextBox" runat="server" CssClass="txtsmall" 
                                                                                                    Width="40px">
                                                                                                    <MaskSettings Mask="&lt;0..99999&gt;" />
<MaskSettings Mask="&lt;0..99999&gt;"></MaskSettings>

                                                                                                    <ValidationSettings ErrorDisplayMode="ImageWithTooltip" CausesValidation="True" 
                                                                                                        SetFocusOnError="True">
                                                                                                        <RegularExpression ErrorText="Please enter a numeric value using the numbers only (0-9)." 
                                                                                                            ValidationExpression="^\d+$" />
                                                                                                    </ValidationSettings>
                                                                                                </dx:ASPxTextBox>
                                                  
                                                                                </td>
                                                                                <td>
                                                                                    <dx:ASPxLabel ID="ASPxLabel9" runat="server" CssClass="lblsmallFont" 
                                                                                                    Text="minutes">
                                                                                                </dx:ASPxLabel>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    <dx:ASPxLabel ID="ASPxLabel7" runat="server" CssClass="lblsmallFont" 
                                                                                        Text="Off-Hours Scan Interval:">
                                                                                    </dx:ASPxLabel>
                                                                                </td>
                                                                                <td>
                                                                                    <dx:ASPxTextBox ID="OffScanIntervalTextBox" runat="server" CssClass="txtsmall" 
                                                                                                    Width="40px">
                                                                                                    <MaskSettings Mask="&lt;0..99999&gt;" />
<MaskSettings Mask="&lt;0..99999&gt;"></MaskSettings>

                                                                                                    <ValidationSettings ErrorDisplayMode="ImageWithTooltip" CausesValidation="True" 
                                                                                                        SetFocusOnError="True">
<RequiredField IsRequired="True"></RequiredField>
                                                                                                        <RegularExpression ErrorText="Please enter a numeric value using the numbers only (0-9)." 
                                                                                                            ValidationExpression="^\d+$" />
                                                                                                        <RequiredField IsRequired="True" />
                                                                                                    </ValidationSettings>
                                                                                                </dx:ASPxTextBox>
                     
                                                                                </td>
                                                                                <td>
                                                                                    <dx:ASPxLabel ID="ASPxLabel10" runat="server" CssClass="lblsmallFont" 
                                                                                                    Text="minutes">
                                                                                                </dx:ASPxLabel>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    <dx:ASPxLabel ID="ASPxLabel8" runat="server" CssClass="lblsmallFont" 
                                                                                        Text="Retry Interval:">
                                                                                    </dx:ASPxLabel>
                                                                                </td>
                                                                                <td>
                                                                                    <dx:ASPxTextBox ID="RetryIntervalTextBox" runat="server" CssClass="txtsmall" 
                                                                                                    Width="40px">
                                                                                                    <MaskSettings Mask="&lt;1..99999&gt;" />
                                                                                                     <MaskSettings Mask="&lt;1..99999&gt;"></MaskSettings>

                                                                                                    <ValidationSettings ErrorDisplayMode="ImageWithTooltip" CausesValidation="True" 
                                                                                                        SetFocusOnError="True">
<RequiredField IsRequired="True"></RequiredField>
                                                                                                        <RegularExpression ErrorText="Please enter a numeric value using the numbers only (1-9)." 
                                                                                                            ValidationExpression="^\d+$" />
                                                                                                        <RequiredField IsRequired="True" />
                                                                                                    </ValidationSettings>
                                                                                                </dx:ASPxTextBox>
                                                   
                                                                                </td>
                                                                                <td>
                                                                                    <dx:ASPxLabel ID="ASPxLabel11" runat="server" CssClass="lblsmallFont" 
                                                                                                    Text="minutes">
                                                                                                </dx:ASPxLabel>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                        </dx:PanelContent>
                                                    </PanelCollection>
                                                </dx:ASPxRoundPanel>
                                            </td>
                                            <td valign="top">
                                                <dx:ASPxRoundPanel ID="ASPxRoundPanel3" runat="server" 
                                                    CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass" 
                                                    GroupBoxCaptionOffsetY="-24px" HeaderText="Alert Settings" 
                                                    SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" Width="100%" 
                                                    >
                                                    <ContentPaddings PaddingBottom="10px" PaddingLeft="4px" PaddingTop="10px" />
<ContentPaddings PaddingLeft="4px" PaddingTop="10px" PaddingBottom="10px"></ContentPaddings>

                                                    <HeaderStyle Height="23px">
                                                    </HeaderStyle>
                                                    <PanelCollection>
                                                        <dx:PanelContent ID="PanelContent5" runat="server" SupportsDisabledAttribute="True">
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <dx:ASPxLabel ID="ASPxLabel12" runat="server" CssClass="lblsmallFont" 
                                                                            Text="Failures before Alert:">
                                                                        </dx:ASPxLabel>
                                                                    </td>
                                                                    <td>
                                                                        <dx:ASPxTextBox ID="FailureTextBox" runat="server" CssClass="txtsmall" 
                                                                            Width="40px">
<MaskSettings Mask="&lt;0..99999&gt;"></MaskSettings>
                                                                            <MaskSettings Mask="&lt;0..99999&gt;" />
                                                                            <ValidationSettings CausesValidation="True" ErrorDisplayMode="ImageWithTooltip" 
                                                                                SetFocusOnError="True">
                                                                                <RegularExpression ErrorText="Please enter a numeric value using the numbers only (0-9)." 
                                                                                    ValidationExpression="^\d+$" />
                                                                            </ValidationSettings>
                                                                        </dx:ASPxTextBox>
                                                                    </td>
                                                                    <td>
                                                                        <dx:ASPxLabel ID="ASPxLabel14" runat="server" CssClass="lblsmallFont" 
                                                                            Text="consecutive failures">
                                                                        </dx:ASPxLabel>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <dx:ASPxLabel ID="ASPxLabel13" runat="server" CssClass="lblsmallFont" 
                                                                            Text="Response Threshold:">
                                                                        </dx:ASPxLabel>
                                                                    </td>
                                                                    <td>
                                                                        <dx:ASPxTextBox ID="MSRespThresholdTextBox" runat="server" CssClass="txtsmall" 
                                                                            Width="40px">
<MaskSettings Mask="&lt;0..99999&gt;"></MaskSettings>
                                                                            <MaskSettings Mask="&lt;0..99999&gt;" />
                                                                            <ValidationSettings CausesValidation="True" ErrorDisplayMode="ImageWithTooltip" 
                                                                                SetFocusOnError="True">
                                                                                <RegularExpression ErrorText="Please enter a numeric value using the numbers only (0-9)." 
                                                                                    ValidationExpression="^\d+$" />
                                                                            </ValidationSettings>
                                                                        </dx:ASPxTextBox>
                                                                    </td>
                                                                    <td>
                                                                        <dx:ASPxLabel ID="ASPxLabel15" runat="server" CssClass="lblsmallFont" 
                                                                            Text="milliseconds">
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
                                                    </PanelCollection>
                                                </dx:ASPxRoundPanel>
                                            </td>
                                        </tr>
                                    </table>
                                </dx:ContentControl>
                            </ContentCollection>
                        </dx:TabPage>
                        <dx:TabPage Text="Maintenance Window">
                         <TabImage Url="~/images/application_view_tile.png"/>
<TabImage Url="~/images/application_view_tile.png"></TabImage>
                            <ContentCollection>
                                <dx:ContentControl ID="ContentControl2" runat="server" SupportsDisabledAttribute="True">
                                <table width="100%">
                                                        <tr>
                                                            <td>
                                                               
                                                                <dx:ASPxButton ID="ToggleVeiwButton" runat="server" 
                                                                    CssFilePath="~/App_Themes/Office2010Blue/{0}/styles.css" 
                                                                    CssPostfix="Office2010Blue" 
                                                                    SpriteCssFilePath="~/App_Themes/Office2010Blue/{0}/sprite.css" 
                                                                    Text="Switch to Calendar view" Width="178px" Visible="False">
                                                                </dx:ASPxButton>
                                                               
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <div id="maintDiv" class="info">Maintenance Windows are times when you do not want the server monitored. You can define maintenance windows using the Hours & Maintenance\Maintenance menu option. Use the Actions column to modify maintenance windows information.</div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <dx:ASPxGridView ID="MaintWinListGridView" runat="server" AutoGenerateColumns="False"
                            CssFilePath="~/App_Themes/Office2010Blue/{0}/styles.css" Width="100%" CssPostfix="Office2010Blue"
                            KeyFieldName="ID" Cursor="pointer" OnHtmlRowCreated="MaintWinListGridView_HtmlRowCreated" 
                                                                    OnSelectionChanged="MaintWinListGridView_SelectionChanged" 
                                                                    Theme="Office2003Blue" OnPageSizeChanged="MaintWinListGridView_PageSizeChanged">
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
                                                                    <Settings ShowFilterRow="True" ShowGroupPanel="True" />

<SettingsBehavior AllowSelectByRowClick="True" ColumnResizeMode="NextColumn" ProcessSelectionChangedOnServer="True"></SettingsBehavior>

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
                        </dx:TabPage>
                        <dx:TabPage Text="Alert">
                        <TabImage Url="../images/icons/error.png"></TabImage>
                            <ContentCollection>
                                <dx:ContentControl ID="ContentControl3" runat="server" SupportsDisabledAttribute="True">
                                    <table width="100%">
                                        <tr>
                                            <td>
                                                <div id="alertDiv" class="info">The list of available alerts is listed below. In order to add new alerts or configure existing alerts, please use the Alerts\Alert Definitions menu.</div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                            <dx:ASPxGridView ID="AlertGridView" runat="server" AutoGenerateColumns="False" 
                                                                                CssFilePath="~/App_Themes/Office2010Blue/{0}/styles.css" 
                                                                                CssPostfix="Office2010Blue" Cursor="pointer" KeyFieldName="ID" 
                                                                                Theme="Office2003Blue" Width="100%" OnPageSizeChanged="AlertGridView_PageSizeChanged">
                                                                                <Columns>
                                                                                    <dx:GridViewDataTextColumn Caption="Name" FieldName="Name" ReadOnly="True" 
                                                                                        ShowInCustomizationForm="True" VisibleIndex="0">
                                                                                        <Settings AutoFilterCondition="Contains" />
<Settings AutoFilterCondition="Contains"></Settings>

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
                                                                                        ShowInCustomizationForm="True" VisibleIndex="2" Width="60px">
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
                                                                                        ShowInCustomizationForm="True" VisibleIndex="6" Width="200px">
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
<Border BorderColor="#4986A2"></Border>
                    </ContentStyle>
                </dx:ASPxPageControl>
            </td>
            </tr>
            <tr>
                <td>
                <div id="errorDiv" runat="server" class="alert alert-danger" style="display: none">Error attempting to update mail services.
                </div>
                <table>
                    <tr>
                        <td>
                            <dx:ASPxButton ID="formOKButton" runat="server" CssClass="sysButton"
                                Text="OK" OnClick="formOKButton_Click" 
                                AutoPostBack="False">
                            </dx:ASPxButton>
                        </td>
                        <td>
                            <dx:ASPxButton ID="formCancelButton" runat="server" CssClass="sysButton"
                                Text="Cancel" OnClick="formCancelButton_Click" 
                                CausesValidation="False">
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
                    </HeaderStyle>
                    <ContentCollection>
                        <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server" SupportsDisabledAttribute="True">
                            <dx:ASPxPanel ID="Panel1" runat="server" DefaultButton="btOK">
                                <PanelCollection>
                                    <dx:PanelContent ID="PanelContent3" runat="server" SupportsDisabledAttribute="True">
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
                                                        </dx:ASPxButton>
                                                        <dx:ASPxButton ID="ValidationUpdatedButton" runat="server" AutoPostBack="False" 
                                                            CssFilePath="~/App_Themes/Office2010Blue/{0}/styles.css" 
                                                            CssPostfix="Office2010Blue" OnClick="ValidationUpdatedButton_Click" 
                                                            SpriteCssFilePath="~/App_Themes/Office2010Blue/{0}/sprite.css" Text="OK" 
                                                            Visible="False" Width="80px">
                                                            <ClientSideEvents Click="function(s, e) { pcErrorMessage.Hide(); }" />
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
