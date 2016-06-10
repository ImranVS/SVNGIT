<%@ Page Title="VitalSigns Plus- Edit Notes database " Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true"
    CodeBehind="EditNotes.aspx.cs" Inherits="VSWebUI.Configurator.EditNotes" %>

<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>




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
<table width="70%">
    <tr>
        <td valign="top">    
            <div class="header" id="servernamelbldisp" runat="server">Notes Database</div>
        </td>
    </tr>
    <tr>
        <td>
            <table>
                    <tr>
                        <td width="30%" align="left" valign="top">
                            <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" 
                                CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass" 
                                GroupBoxCaptionOffsetY="-24px" HeaderText="Notes Database Properties" 
                                SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" Width="100%">
                                <ContentPaddings PaddingBottom="10px" PaddingLeft="4px" PaddingTop="10px" />
<ContentPaddings PaddingLeft="4px" PaddingTop="10px" PaddingBottom="10px"></ContentPaddings>

                                <HeaderStyle Height="23px">
                                </HeaderStyle>
                                <PanelCollection>
                                    <dx:PanelContent ID="PanelContent2" runat="server" SupportsDisabledAttribute="True">
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    <dx:ASPxLabel ID="notesLabel" runat="server" CssClass="lblsmallFont" 
                                                        Text="Name:">
                                                    </dx:ASPxLabel>
                                                </td>
                                                <td>
                                                    <dx:ASPxTextBox ID="NameTextBox" runat="server" CssClass="txtsmall" 
                                                        OnTextChanged="NameTextBox_TextChanged" Width="170px">
                                                        <ValidationSettings ErrorDisplayMode="ImageWithTooltip">
                                                            <RequiredField IsRequired="True" />
<RequiredField IsRequired="True"></RequiredField>
                                                        </ValidationSettings>
                                                    </dx:ASPxTextBox>
                                                </td>
                                                <td>
                                                    <dx:ASPxButton ID="NotesBrowseButton" runat="server" CausesValidation="False" 
                                                        CssClass="sysButton"
                                                        Text="Browse.." OnClick="NotesBrowseButton_Click">
                                                    </dx:ASPxButton>
                                                </td>
                                                <td>
                                                    &nbsp;</td>
                                                <td align="right">
                                                    <dx:ASPxCheckBox ID="EnabledCheckBox" runat="server" CheckState="Unchecked" 
                                                        CssClass="lblsmallFont" Text="Enabled for scanning" Wrap="False">
                                                    </dx:ASPxCheckBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <dx:ASPxLabel ID="ASPxLabel2" runat="server" CssClass="lblsmallFont" 
                                                        Text="Domino Server:">
                                                    </dx:ASPxLabel>
                                                    &nbsp;</td>
                                                <td>
                                                    <dx:ASPxComboBox ID="DominoServerComboBox" runat="server">
                                                        <ValidationSettings ErrorDisplayMode="ImageWithTooltip">
                                                        </ValidationSettings>
                                                    </dx:ASPxComboBox>
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    &nbsp;</td>
                                                <td>
                                                    &nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <dx:ASPxLabel ID="ASPxLabel3" runat="server" CssClass="lblsmallFont" 
                                                        Text="Database Filename:">
                                                    </dx:ASPxLabel>
                                                </td>
                                                <td>
                                                    <dx:ASPxTextBox ID="DBFileNameTextBox" runat="server" Width="170px">
                                                        <ValidationSettings ErrorDisplayMode="ImageWithTooltip">
                                                            <RequiredField IsRequired="True" />
<RequiredField IsRequired="True"></RequiredField>
                                                        </ValidationSettings>
                                                    </dx:ASPxTextBox>
                                                </td>
                                                <td>
                                                    <dx:ASPxLabel ID="NDsizeLabel" runat="server" CssClass="lblsmallFont">
                                                    </dx:ASPxLabel>
                                                </td>
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
                        <td>
                            <dx:ASPxRoundPanel ID="ASPxRoundPanel2" runat="server" 
                                CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass" 
                                GroupBoxCaptionOffsetY="-24px" HeaderText="Scan Settings" 
                                SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" Width="100%">
                                <ContentPaddings PaddingBottom="10px" PaddingLeft="4px" PaddingTop="10px" />
<ContentPaddings PaddingLeft="4px" PaddingTop="10px" PaddingBottom="10px"></ContentPaddings>

                                <HeaderStyle Height="23px">
                                </HeaderStyle>
                                <PanelCollection>
                                    <dx:PanelContent ID="PanelContent3" runat="server" SupportsDisabledAttribute="True">
                                        <table>
                                            <tr>
                                                <td>
                                                    <dx:ASPxLabel ID="ASPxLabel4" runat="server" CssClass="lblsmallFont" 
                                                        Text="Scan Interval:" Wrap="False">
                                                    </dx:ASPxLabel>
                                                </td>
                                                <td>
                                                    <dx:ASPxTextBox ID="ScanIntervalTextBox" runat="server" CssClass="txtsmall" 
                                                        Width="40px">
                                                        <MaskSettings Mask="&lt;0..99999&gt;" />
<MaskSettings Mask="&lt;0..99999&gt;"></MaskSettings>

                                                        <ValidationSettings CausesValidation="True" ErrorDisplayMode="ImageWithTooltip" 
                                                            SetFocusOnError="True">
                                                            <RegularExpression ErrorText="Please enter a numeric value using the numbers only (0-9)." 
                                                                ValidationExpression="^\d+$" />
                                                            <RequiredField IsRequired="True" />
<RegularExpression ErrorText="Please enter a numeric value using the numbers only (0-9)." ValidationExpression="^\d+$"></RegularExpression>

<RequiredField IsRequired="True"></RequiredField>
                                                        </ValidationSettings>
                                                    </dx:ASPxTextBox>
                                                </td>
                                                <td>
                                                    <dx:ASPxLabel ID="ASPxLabel7" runat="server" CssClass="lblsmallFont" 
                                                        Text="minutes">
                                                    </dx:ASPxLabel>
                                                </td>
                                                <td>
                                                    &nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <dx:ASPxLabel ID="ASPxLabel5" runat="server" CssClass="lblsmallFont" 
                                                        Text="Retry Interval:" Wrap="False">
                                                    </dx:ASPxLabel>
                                                </td>
                                                <td>
                                                    <dx:ASPxTextBox ID="RetryIntervalTextBox" runat="server" CssClass="txtsmall" 
                                                        Width="40px">
                                                        <MaskSettings Mask="&lt;1..99999&gt;" />

                                                        <ValidationSettings CausesValidation="True" ErrorDisplayMode="ImageWithTooltip" 
                                                            SetFocusOnError="True">
                                                            <RegularExpression ErrorText="Please enter a numeric value using the numbers only (1-9)." 
                                                                ValidationExpression="^\d+$" />
                                                            <RequiredField IsRequired="True" />
<RegularExpression ErrorText="Please enter a numeric value using the numbers only (0-9)." ValidationExpression="^\d+$"></RegularExpression>

<RequiredField IsRequired="True"></RequiredField>
                                                        </ValidationSettings>
                                                    </dx:ASPxTextBox>
                                                </td>
                                                <td>
                                                    <dx:ASPxLabel ID="ASPxLabel8" runat="server" CssClass="lblsmallFont" 
                                                        Text="minutes">
                                                    </dx:ASPxLabel>
                                                </td>
                                                <td>
                                                    &nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <dx:ASPxLabel ID="ASPxLabel6" runat="server" CssClass="lblsmallFont" 
                                                        Text="Off-Hours Retry Interval:" Wrap="False">
                                                    </dx:ASPxLabel>
                                                </td>
                                                <td>
                                                    <dx:ASPxTextBox ID="OffHoursTextBox" runat="server" CssClass="txtsmall" 
                                                        Width="40px">
                                                        <MaskSettings Mask="&lt;0..99999&gt;" />
<MaskSettings Mask="&lt;0..99999&gt;"></MaskSettings>

                                                        <ValidationSettings CausesValidation="True" ErrorDisplayMode="ImageWithTooltip" 
                                                            SetFocusOnError="True">
                                                            <RegularExpression ErrorText="Please enter a numeric value using the numbers only (0-9)." 
                                                                ValidationExpression="^\d+$" />
                                                            <RequiredField IsRequired="True" />
<RegularExpression ErrorText="Please enter a numeric value using the numbers only (0-9)." ValidationExpression="^\d+$"></RegularExpression>

<RequiredField IsRequired="True"></RequiredField>
                                                        </ValidationSettings>
                                                    </dx:ASPxTextBox>
                                                </td>
                                                <td>
                                                    <dx:ASPxLabel ID="ASPxLabel9" runat="server" CssClass="lblsmallFont" 
                                                        Text="minutes">
                                                    </dx:ASPxLabel>
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
                            <dx:ASPxRoundPanel ID="ASPxRoundPanel3" runat="server" 
                                CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass" 
                                GroupBoxCaptionOffsetY="-24px" HeaderText="Type" Height="153px" 
                                SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" Width="100%" EnableHierarchyRecreation="False">
                                <ContentPaddings PaddingBottom="10px" PaddingLeft="4px" PaddingTop="10px" />
<ContentPaddings PaddingLeft="4px" PaddingTop="10px" PaddingBottom="10px"></ContentPaddings>

                                <HeaderStyle Height="23px">
                                </HeaderStyle>
                                <PanelCollection>
                                    <dx:PanelContent ID="PanelContent4" runat="server" SupportsDisabledAttribute="True">
                                        <div class="info">Selecting <b>'Refresh All Views' </b>will detect many types of database corruption. 
                                                                </div>
                                        <table>
                                            <tr>
                                            <td>
                                                                <dx:ASPxComboBox ID="AlertTypeComboBox" runat="server" AutoPostBack="True" 
                                                                    OnSelectedIndexChanged="AlertTypeComboBox_SelectedIndexChanged">
                                                                    <Items>
                                                                        <dx:ListEditItem Text="Document Count" Value="Document Count" />
                                                                        <dx:ListEditItem Text="Database Size" Value="Database Size" />
                                                                        <dx:ListEditItem Text="Replication" Value="Replication" />
                                                                        <dx:ListEditItem Text="Refresh All Views" Value="Refresh All Views" />
                                                                        <dx:ListEditItem Text="Database Response Time" Value="Database Response Time" />
                                                                        <dx:ListEditItem Text="Database Disappearance" Value="Database Disappearance" />
                                                                    </Items>
                                                                    <ValidationSettings>
                                                                        <RequiredField IsRequired="True" />
<RequiredField IsRequired="True"></RequiredField>
                                                                    </ValidationSettings>
                                                                </dx:ASPxComboBox>
                                                            </td>
                             
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                           <table>
                                                                            <tr>
                                                                                <td colspan="2">
                                                                                    <dx:ASPxCheckBox ID="InitiateRepCheckBox" runat="server" CheckState="Unchecked" 
                                                                                        CssClass="lblsmallFont" Text="Initiate Replication" Wrap="False">
                                                                                    </dx:ASPxCheckBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    <dx:ASPxLabel ID="ServerLabel" runat="server" CssClass="lblsmallFont" 
                                                                                        Text="Select target servers:">
                                                                                    </dx:ASPxLabel>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    <dx:ASPxListBox ID="ServerListBox" runat="server" SelectionMode="CheckColumn">
                                                                                    </dx:ASPxListBox>
                                                                                    <dx:ASPxTextBox ID="TriggerValueTextBox" runat="server" CssClass="txtsmall" 
                                                                                        OnTextChanged="TriggerValueTextBox_TextChanged" Visible="False" Width="100px">
                                                                                        <ValidationSettings CausesValidation="True" ErrorDisplayMode="ImageWithTooltip" 
                                                                                            SetFocusOnError="True">
                                                                                            <RegularExpression ErrorText="Please enter a numeric value using the numbers only (0-9)." 
                                                                                                ValidationExpression="^\d+$" />
                                                                                        </ValidationSettings>
                                                                                    </dx:ASPxTextBox>
                                                                                </td>
                                                                                <td>
                                                                                    <dx:ASPxLabel ID="TriggerLabel" runat="server" CssClass="lblsmallFont">
                                                                                    </dx:ASPxLabel>
                                                                                    <dx:ASPxLabel ID="GBLabel" runat="server" CssClass="lblsmallFont">
                                                                                    </dx:ASPxLabel>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="AlertTypeComboBox" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                        </table>
                                    </dx:PanelContent>
                                </PanelCollection>
                            </dx:ASPxRoundPanel>

                            <dx:ASPxPopupControl runat="server" PopupHorizontalAlign="WindowCenter" 
                                PopupVerticalAlign="WindowCenter" 
                                SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" Modal="True" 
                                AllowDragging="True" HeaderText="Select a Notes Database" CssPostfix="Glass" 
                                CssFilePath="~/App_Themes/Glass/{0}/styles.css" Width="500px" 
                                ID="SelectNotesPopupControl" Theme="MetropolisBlue" 
                                EnableHierarchyRecreation="False">
<HeaderStyle>
<Paddings PaddingLeft="10px" PaddingTop="1px" PaddingRight="6px"></Paddings>
</HeaderStyle>
<ContentCollection>
<dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server" SupportsDisabledAttribute="True">
                                        <table>
                                            <tr>
                                                <td>
                                                    <dx:ASPxComboBox ID="NDNameComboBox" runat="server" AutoPostBack="True" 
                                                        OnSelectedIndexChanged="NDNameComboBox_SelectedIndexChanged">
                                                    </dx:ASPxComboBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <dx:ASPxGridView ID="SelectNDGridView" runat="server" 
                                                                AutoGenerateColumns="False" 
                                                                CssFilePath="~/App_Themes/Office2010Silver/{0}/styles.css" 
                                                                CssPostfix="Office2010Silver" 
                                                                OnSelectionChanged="SelectNDGridView_SelectionChanged" Theme="Office2003Blue" 
                                                                Width="100%" OnPageSizeChanged="SelectNDGridView_PageSizeChanged">
                                                                <Columns>
                                                                    <dx:GridViewDataTextColumn Caption="DB Title" Name="DBTitle" 
                                                                        ShowInCustomizationForm="True" VisibleIndex="1">
                                                                        <Settings AutoFilterCondition="Contains" />
                                                                    </dx:GridViewDataTextColumn>
                                                                    <dx:GridViewDataTextColumn Caption="DB File Path" Name="DBFile" 
                                                                        ShowInCustomizationForm="True" VisibleIndex="2">
                                                                        <Settings AutoFilterCondition="Contains" />
                                                                    </dx:GridViewDataTextColumn>
                                                                    <dx:GridViewDataTextColumn Caption="Replica ID" Name="ReplicaID" 
                                                                        ShowInCustomizationForm="True" Visible="False" VisibleIndex="3">
                                                                    </dx:GridViewDataTextColumn>
                                                                </Columns>
                                                                <SettingsBehavior AllowSelectByRowClick="True" AllowSelectSingleRowOnly="True" 
                                                                    ProcessSelectionChangedOnServer="True" />
                                                                <Settings ShowFilterRow="True" />
                                                                <Styles CssFilePath="~/App_Themes/Office2010Silver/{0}/styles.css" 
                                                                    CssPostfix="Office2010Silver">
                                                                    <LoadingPanel ImageSpacing="5px">
                                                                    </LoadingPanel>
                                                                    <Header ImageSpacing="5px" SortingImageSpacing="5px">
                                                                    </Header>
                                                                </Styles>
                                                                <StylesEditors ButtonEditCellSpacing="0">
                                                                    <ProgressBar Height="21px">
                                                                    </ProgressBar>
                                                                </StylesEditors>
                                                            </dx:ASPxGridView><br />
                                                            <div ID="errorDiv" runat="server" class="alert alert-danger" style="display: none">Error:
                                                            </div>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="NDNameComboBox" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <dx:ASPxButton ID="selectButton" runat="server" CausesValidation="False" 
                                                                    OnClick="selectButton_Click" 
                                                                    CssClass="sysButton" Text="Select" 
                                                                    Width="75px">
                                                                </dx:ASPxButton>
                                                            </td>
                                                            <td>
                                                                <dx:ASPxButton ID="CancelButton" runat="server" CausesValidation="False" 
                                                                    OnClick="CancelButton_Click" 
                                                                    CssClass="sysButton" Text="Cancel" 
                                                                    Width="75px">
                                                                </dx:ASPxButton>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>

                                    </dx:PopupControlContentControl>
</ContentCollection>
</dx:ASPxPopupControl>

                            <dx:ASPxPopupControl ID="ErrorMessagePopupControl" runat="server" 
                                AllowDragging="True" ClientInstanceName="pcErrorMessage" 
                                CloseAction="CloseButton" CssFilePath="~/App_Themes/Glass/{0}/styles.css" 
                                CssPostfix="Glass" EnableAnimation="False" EnableViewState="False" 
                                HeaderText="Validation Failure" Height="150px" Modal="True" 
                                PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" 
                                SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" Width="300px" EnableHierarchyRecreation="False">
                                <HeaderStyle>
                                <Paddings PaddingLeft="10px" PaddingRight="6px" PaddingTop="1px" />
<Paddings PaddingLeft="10px" PaddingTop="1px" PaddingRight="6px"></Paddings>
                                </HeaderStyle>
                                <ContentCollection>
                                    <dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server" SupportsDisabledAttribute="True">
                                        <dx:ASPxPanel ID="Panel1" runat="server" DefaultButton="btOK">
                                            <PanelCollection>
                                                <dx:PanelContent ID="PanelContent5" runat="server" SupportsDisabledAttribute="True">
                                                    <div style="min-height: 70px;">
                                                        <dx:ASPxLabel ID="ErrorMessageLabel" runat="server" Text="Username:">
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
                                                                        CssPostfix="Office2010Blue" OnClick="ValidationUpdatedButton_Click" 
                                                                        SpriteCssFilePath="~/App_Themes/Office2010Blue/{0}/sprite.css" Text="OK" 
                                                                        Visible="False" Width="80px">
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
                    <tr>
                        <td>
                            <div id="errorDiv2" class="alert alert-danger" runat="server" style="display: none">Error.
                            </div>
                            <table>
                                <tr>
                                    <td>
                                        <dx:ASPxButton ID="FormOkButton" runat="server" CssClass="sysButton"
                                            Text="OK" OnClick="FormOkButton_Click">
                                        </dx:ASPxButton>
                                    </td>
                                    <td>
                                        <dx:ASPxButton ID="FormCancelButton" runat="server" CssClass="sysButton"
                                        CausesValidation="False" 
                                            OnClick="FormCancelButton_Click"
                                            Text="Cancel">
                                        </dx:ASPxButton>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
        </td>
    </tr>
</table>
    </asp:Content>
