<%@ Page Title="VitalSigns Plus" Language="C#" MasterPageFile="~/DashboardSite.Master" AutoEventWireup="true" CodeBehind="Problems.aspx.cs" Inherits="VSWebUI.Problems" %>

<%@ MasterType virtualpath="~/DashboardSite.Master" %>
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
<table width="100%">
    <tr>
        <td>
            <div class="header" id="servernamelbldisp" runat="server">Database Health Problems</div>
        </td>
        <td>
            &nbsp;
        </td>    
        <td align="right">
            <table>
                <tr>
                    <td>
                    </td>
                </tr>
            </table>
        </td>        
    </tr>
</table>
<table class="style1">
        <tr>
            <td>
                <dx:ASPxGridView ID="ProblemsGridView" runat="server" 
                    AutoGenerateColumns="False" EnableTheming="True" Theme="Office2003Blue" KeyFieldName="ID" 
                    OnPageSizeChanged="ProblemsGridView_PageSizeChanged"
                   Width="100%" OnHtmlDataCellPrepared="ProblemsGridView_HtmlDataCellPrepared">
                    <Columns>
                        <dx:GridViewDataTextColumn Caption="ID" FieldName="ID" 
                            ShowInCustomizationForm="True" Visible="False" VisibleIndex="34">
                             <EditCellStyle CssClass="GridCss"></EditCellStyle><EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle><HeaderStyle CssClass="GridCssHeader" /><CellStyle CssClass="GridCss"></CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Scan Date" FieldName="ScanDate" 
                            ShowInCustomizationForm="True" VisibleIndex="6">
                            <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                             <EditCellStyle CssClass="GridCss"></EditCellStyle><EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle>
                             <HeaderStyle CssClass="GridCssHeader" /><CellStyle CssClass="GridCss"></CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="File Name" FieldName="FileName" 
                            ShowInCustomizationForm="True" VisibleIndex="1" FixedStyle="Left">
                            <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                             <EditCellStyle CssClass="GridCss"></EditCellStyle><EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle>
                             <HeaderStyle CssClass="GridCssHeader" /><CellStyle CssClass="GridCss"></CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Title" FieldName="Title" 
                            ShowInCustomizationForm="True" VisibleIndex="2" FixedStyle="Left">
                             <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                             <EditCellStyle CssClass="GridCss"></EditCellStyle><EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle>
                             <HeaderStyle CssClass="GridCssHeader" /><CellStyle CssClass="GridCss"></CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="File Size" FieldName="FileSize" 
                            ShowInCustomizationForm="True" VisibleIndex="7">
                             <EditCellStyle CssClass="GridCss"></EditCellStyle><EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle>
                             <HeaderStyle CssClass="GridCssHeader" /><CellStyle CssClass="GridCss"></CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Server" FieldName="Server" 
                            ShowInCustomizationForm="True" VisibleIndex="4" FixedStyle="Left" 
                            Width="200px">
                        <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                             <EditCellStyle CssClass="GridCss"></EditCellStyle><EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle>
                             <HeaderStyle CssClass="GridCssHeader" /><CellStyle CssClass="GridCss" 
                                 Wrap="True"></CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Design Template Name" 
                            FieldName="DesignTemplateName" ShowInCustomizationForm="True" 
                            VisibleIndex="9" Width="100px">
                             <EditCellStyle CssClass="GridCss"></EditCellStyle><EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle>
                             <HeaderStyle CssClass="GridCssHeader" Wrap="True" />
                             <CellStyle CssClass="GridCss" Wrap="True"></CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Quota" FieldName="Quota" 
                            ShowInCustomizationForm="True" VisibleIndex="5">
                             <EditCellStyle CssClass="GridCss"></EditCellStyle><EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle>
                             <HeaderStyle CssClass="GridCssHeader" /><CellStyle CssClass="GridCss"></CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="FTIndexed" FieldName="FTIndexed" 
                            ShowInCustomizationForm="True" VisibleIndex="10">
                             <EditCellStyle CssClass="GridCss"></EditCellStyle><EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle>
                             <HeaderStyle CssClass="GridCssHeader" /><CellStyle CssClass="GridCss"></CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Enabled For Cluster Replication" 
                            FieldName="EnabledForClusterReplication" ShowInCustomizationForm="True" 
                            VisibleIndex="11" Width="100px">
                             <EditCellStyle CssClass="GridCss"></EditCellStyle><EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle>
                             <HeaderStyle CssClass="GridCssHeader" Wrap="True" /><CellStyle CssClass="GridCss"></CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="ReplicaID" FieldName="ReplicaID" 
                            ShowInCustomizationForm="True" VisibleIndex="12">
                             <EditCellStyle CssClass="GridCss"></EditCellStyle><EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle>
                             <HeaderStyle CssClass="GridCssHeader" /><CellStyle CssClass="GridCss" 
                                 Wrap="True"></CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="ODS" FieldName="ODS" 
                            ShowInCustomizationForm="True" VisibleIndex="13">
                             <EditCellStyle CssClass="GridCss"></EditCellStyle><EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle>
                             <HeaderStyle CssClass="GridCssHeader" /><CellStyle CssClass="GridCss"></CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Status" FieldName="Status" 
                            ShowInCustomizationForm="True" VisibleIndex="3" FixedStyle="Left">
                        <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                             <EditCellStyle CssClass="GridCss"></EditCellStyle><EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle>
                             <HeaderStyle CssClass="GridCssHeader" /><CellStyle CssClass="GridCss1"></CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Document Count" FieldName="DocumentCount" 
                            ShowInCustomizationForm="True" VisibleIndex="14">
                             <EditCellStyle CssClass="GridCss"></EditCellStyle><EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle>
                             <HeaderStyle CssClass="GridCssHeader" Wrap="True" /><CellStyle CssClass="GridCss"></CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Categories" FieldName="Categories" 
                            ShowInCustomizationForm="True" VisibleIndex="15">
                            <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                             <EditCellStyle CssClass="GridCss"></EditCellStyle><EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle>
                             <HeaderStyle CssClass="GridCssHeader" /><CellStyle CssClass="GridCss"></CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Created" FieldName="Created" 
                            ShowInCustomizationForm="True" VisibleIndex="16">
                             <EditCellStyle CssClass="GridCss"></EditCellStyle><EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle>
                             <HeaderStyle CssClass="GridCssHeader" /><CellStyle CssClass="GridCss"></CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Current Access Level" 
                            FieldName="CurrentAccessLevel" ShowInCustomizationForm="True" 
                            VisibleIndex="17">
                             <EditCellStyle CssClass="GridCss"></EditCellStyle><EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle>
                             <HeaderStyle CssClass="GridCssHeader" Wrap="True" /><CellStyle CssClass="GridCss"></CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="FTIndex Frequency" 
                            FieldName="FTIndexFrequency" ShowInCustomizationForm="True" 
                            VisibleIndex="18">
                             <EditCellStyle CssClass="GridCss"></EditCellStyle><EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle>
                             <HeaderStyle CssClass="GridCssHeader" Wrap="True" /><CellStyle CssClass="GridCss"></CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Is In Service" FieldName="IsInService" 
                            ShowInCustomizationForm="True" VisibleIndex="19">
                             <EditCellStyle CssClass="GridCss"></EditCellStyle><EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle>
                             <HeaderStyle CssClass="GridCssHeader" /><CellStyle CssClass="GridCss"></CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Folder" FieldName="Folder" 
                            ShowInCustomizationForm="True" VisibleIndex="20">
                             <EditCellStyle CssClass="GridCss"></EditCellStyle><EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle>
                             <HeaderStyle CssClass="GridCssHeader" /><CellStyle CssClass="GridCss"></CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="IsPrivate AddressBook" 
                            FieldName="IsPrivateAddressBook" ShowInCustomizationForm="True" 
                            VisibleIndex="21">
                             <EditCellStyle CssClass="GridCss"></EditCellStyle><EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle>
                             <HeaderStyle CssClass="GridCssHeader" Wrap="True" /><CellStyle CssClass="GridCss"></CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="IsPublic AddressBook" 
                            FieldName="IsPublicAddressBook" ShowInCustomizationForm="True" 
                            VisibleIndex="22">
                             <EditCellStyle CssClass="GridCss"></EditCellStyle><EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle>
                             <HeaderStyle CssClass="GridCssHeader" Wrap="True" /><CellStyle CssClass="GridCss"></CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Last Fixup" FieldName="LastFixup" 
                            ShowInCustomizationForm="True" VisibleIndex="23">
                             <EditCellStyle CssClass="GridCss"></EditCellStyle><EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle>
                             <HeaderStyle CssClass="GridCssHeader" /><CellStyle CssClass="GridCss"></CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="LastFTIndexed" FieldName="LastFTIndexed" 
                            ShowInCustomizationForm="True" VisibleIndex="24">
                             <EditCellStyle CssClass="GridCss"></EditCellStyle><EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle>
                             <HeaderStyle CssClass="GridCssHeader" /><CellStyle CssClass="GridCss"></CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Percent Used" FieldName="PercentUsed" 
                            ShowInCustomizationForm="True" VisibleIndex="25">
                             <EditCellStyle CssClass="GridCss"></EditCellStyle><EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle>
                             <HeaderStyle CssClass="GridCssHeader" /><CellStyle CssClass="GridCss"></CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Details" FieldName="Details" 
                            ShowInCustomizationForm="True" VisibleIndex="26">
                             <EditCellStyle CssClass="GridCss"></EditCellStyle><EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle>
                             <HeaderStyle CssClass="GridCssHeader" /><CellStyle CssClass="GridCss" 
                                 Wrap="False"></CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Last Modified" FieldName="LastModified" 
                            ShowInCustomizationForm="True" VisibleIndex="27">
                             <EditCellStyle CssClass="GridCss"></EditCellStyle><EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle>
                             <HeaderStyle CssClass="GridCssHeader" /><CellStyle CssClass="GridCss"></CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Enabled For Replication" 
                            FieldName="EnabledForReplication" ShowInCustomizationForm="True" 
                            VisibleIndex="28">
                             <EditCellStyle CssClass="GridCss"></EditCellStyle><EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle>
                             <HeaderStyle CssClass="GridCssHeader" Wrap="True" /><CellStyle CssClass="GridCss"></CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="IsMail File" FieldName="IsMailFile" 
                            ShowInCustomizationForm="True" VisibleIndex="29">
                             <EditCellStyle CssClass="GridCss"></EditCellStyle><EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle>
                             <HeaderStyle CssClass="GridCssHeader" /><CellStyle CssClass="GridCss"></CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Inbox DocCount" FieldName="InboxDocCount" 
                            ShowInCustomizationForm="True" VisibleIndex="8">
                             <EditCellStyle CssClass="GridCss"></EditCellStyle><EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle>
                             <HeaderStyle CssClass="GridCssHeader" Wrap="True" /><CellStyle CssClass="GridCss"></CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Q_PlaceBot Count" 
                            FieldName="Q_PlaceBotCount" ShowInCustomizationForm="True" VisibleIndex="30">
                             <EditCellStyle CssClass="GridCss"></EditCellStyle><EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle>
                             <HeaderStyle CssClass="GridCssHeader" /><CellStyle CssClass="GridCss"></CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Q_CustomFormCount" 
                            FieldName="Q_CustomFormCount" ShowInCustomizationForm="True" VisibleIndex="31">
                             <EditCellStyle CssClass="GridCss"></EditCellStyle><EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle>
                             <HeaderStyle CssClass="GridCssHeader" Wrap="True" /><CellStyle CssClass="GridCss"></CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="PersonDocID" FieldName="PersonDocID" 
                            ShowInCustomizationForm="True" VisibleIndex="32">
                             <EditCellStyle CssClass="GridCss"></EditCellStyle><EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle>
                             <HeaderStyle CssClass="GridCssHeader" /><CellStyle CssClass="GridCss"></CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="FileNamePath" FieldName="FileNamePath" 
                            ShowInCustomizationForm="True" VisibleIndex="33">
                             <EditCellStyle CssClass="GridCss"></EditCellStyle><EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle>
                             <HeaderStyle CssClass="GridCssHeader" /><CellStyle CssClass="GridCss"></CellStyle>
                        </dx:GridViewDataTextColumn>
                    </Columns>
                    <SettingsBehavior ColumnResizeMode="NextColumn" />
                    <SettingsPager>
                        <PageSizeItemSettings Visible="True">
                        </PageSizeItemSettings>
                    </SettingsPager>
                    <Settings ShowFilterRow="True" ShowHorizontalScrollBar="True" />
                    <Styles>
                        <AlternatingRow CssClass="GridAltRow">
                        </AlternatingRow>
                    </Styles>
                </dx:ASPxGridView>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
        </tr>
    </table>
</asp:Content>
