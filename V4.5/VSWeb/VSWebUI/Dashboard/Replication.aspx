<%@ Page Title="VitalSigns Plus - Replication Disabled" Language="C#" MasterPageFile="~/DashboardSite.Master" AutoEventWireup="true" CodeBehind="Replication.aspx.cs" Inherits="VSWebUI.Replication" %>

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
    <table class="style1">
        <tr>
            <td>
              <dx:ASPxRoundPanel ID="ReplicationRoundPanel" runat="server" 
                    HeaderText="Replication Disabled" Theme="Glass" Width="100%">
                    <PanelCollection>
<dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">
    <table class="style1">
        <tr>
            <td>
                <dx:ASPxGridView ID="ReplicationGridView" runat="server" OnPageSizeChanged="ReplicationGridView_PageSizeChanged" 
                    AutoGenerateColumns="False" EnableTheming="True" Theme="Office2003Blue" KeyFieldName="ID" 
                   
                   Width="100%" 
                    OnHtmlDataCellPrepared="ReplicationGridView_HtmlDataCellPrepared">
                    <Columns>
                        <dx:GridViewDataTextColumn Caption="ID" FieldName="ID" 
                            ShowInCustomizationForm="True" Visible="False" VisibleIndex="34">
                             <EditCellStyle CssClass="GridCss"></EditCellStyle><EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle><HeaderStyle CssClass="GridCssHeader" /><CellStyle CssClass="GridCss"></CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Scan Date" FieldName="ScanDate" 
                            ShowInCustomizationForm="True" VisibleIndex="6">
                             <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                             <EditCellStyle CssClass="GridCss"></EditCellStyle><EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle>
                             <HeaderStyle CssClass="GridCssHeader" /><CellStyle CssClass="GridCss" 
                                 Wrap="True"></CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="File Name" FieldName="FileName" 
                            ShowInCustomizationForm="True" VisibleIndex="1" FixedStyle="Left">
                             <Settings AutoFilterCondition="Contains" />
                             <EditCellStyle CssClass="GridCss"></EditCellStyle><EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle>
                             <HeaderStyle CssClass="GridCssHeader" /><CellStyle CssClass="GridCss"></CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Title" FieldName="Title" 
                            ShowInCustomizationForm="True" VisibleIndex="2" FixedStyle="Left">
                             <Settings AutoFilterCondition="Contains" />
                             <EditCellStyle CssClass="GridCss"></EditCellStyle><EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle>
                             <HeaderStyle CssClass="GridCssHeader" /><CellStyle CssClass="GridCss" 
                                 Wrap="True"></CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="File Size" FieldName="FileSize" 
                            ShowInCustomizationForm="True" VisibleIndex="7">
                             <EditCellStyle CssClass="GridCss"></EditCellStyle><EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle>
                             <HeaderStyle CssClass="GridCssHeader" /><CellStyle CssClass="GridCss"></CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Server" FieldName="Server" 
                            ShowInCustomizationForm="True" VisibleIndex="4" FixedStyle="Left" 
                            Width="200px">
                             <Settings AutoFilterCondition="Contains" />
                             <EditCellStyle CssClass="GridCss"></EditCellStyle><EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle>
                             <HeaderStyle CssClass="GridCssHeader" Wrap="True" /><CellStyle CssClass="GridCss" 
                                 Wrap="True"></CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Design Template Name" 
                            FieldName="DesignTemplateName" ShowInCustomizationForm="True" 
                            VisibleIndex="9">
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
                            VisibleIndex="11">
                             <EditCellStyle CssClass="GridCss"></EditCellStyle><EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle>
                             <HeaderStyle CssClass="GridCssHeader" Wrap="True" />
                             <CellStyle CssClass="GridCss" Wrap="True"></CellStyle>
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
                             <Settings AutoFilterCondition="Contains" />
                             <EditCellStyle CssClass="GridCss"></EditCellStyle><EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle>
                             <HeaderStyle CssClass="GridCssHeader" /><CellStyle CssClass="GridCss1" 
                                 Wrap="True"></CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Document Count" FieldName="DocumentCount" 
                            ShowInCustomizationForm="True" VisibleIndex="14">
                             <EditCellStyle CssClass="GridCss"></EditCellStyle><EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle>
                             <HeaderStyle CssClass="GridCssHeader" Wrap="True" /><CellStyle CssClass="GridCss"></CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Categories" FieldName="Categories" 
                            ShowInCustomizationForm="True" VisibleIndex="15">
                             <Settings AutoFilterCondition="Contains" />
                             <EditCellStyle CssClass="GridCss"></EditCellStyle><EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle>
                             <HeaderStyle CssClass="GridCssHeader" /><CellStyle CssClass="GridCss" 
                                 Wrap="True"></CellStyle>
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
                             <HeaderStyle CssClass="GridCssHeader" Wrap="True" />
                             <CellStyle CssClass="GridCss" Wrap="True"></CellStyle>
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
                        <dx:GridViewDataTextColumn Caption="IsPrivateAddressBook" 
                            FieldName="IsPrivateAddressBook" ShowInCustomizationForm="True" 
                            VisibleIndex="21">
                             <EditCellStyle CssClass="GridCss"></EditCellStyle><EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle>
                             <HeaderStyle CssClass="GridCssHeader" Wrap="True" /><CellStyle CssClass="GridCss"></CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="IsPublicAddressBook" 
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
                             <HeaderStyle CssClass="GridCssHeader" /><CellStyle CssClass="GridCss"></CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Last Modified" FieldName="LastModified" 
                            ShowInCustomizationForm="True" VisibleIndex="27">
                             <EditCellStyle CssClass="GridCss"></EditCellStyle><EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle>
                             <HeaderStyle CssClass="GridCssHeader" /><CellStyle CssClass="GridCss"></CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="EnabledForReplication" 
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
                             <HeaderStyle CssClass="GridCssHeader" Wrap="True" /><CellStyle CssClass="GridCss"></CellStyle>
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
                             <HeaderStyle CssClass="GridCssHeader" Wrap="True" /><CellStyle CssClass="GridCss"></CellStyle>
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
                        </dx:PanelContent>
</PanelCollection>
                </dx:ASPxRoundPanel>  
                
                </td>
        </tr>
    </table>
</asp:Content>
