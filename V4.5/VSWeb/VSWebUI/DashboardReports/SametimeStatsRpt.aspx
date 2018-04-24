<%@ Page Title="VitalSigns Plus - Sametime Stats Report" Language="C#" MasterPageFile="~/Reports.Master" AutoEventWireup="true" CodeBehind="SametimeStatsRpt.aspx.cs" Inherits="VSWebUI.DashboardReports.SametimeStatsRpt" %>

<%@ Register Assembly="DevExpress.Web.ASPxPivotGrid.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPivotGrid" TagPrefix="dx" %>

<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>



<%@ Register Src="~/Controls/DateRange.ascx" TagName="DateRange" TagPrefix="uc1" %>
<%@ Register assembly="DevExpress.Web.ASPxPivotGrid.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxPivotGrid" tagprefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<script type="text/javascript">
function OnItemClick(s, e) {
    if (e.item.parent == s.GetRootItem())
        e.processOnServer = false;
}
</script>
<table width="100%">
    <tr>
        <td valign="top">
            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <table>
                <tr>
                    <td>
                        <dx:ASPxButton ID="ASPxButton1" runat="server" Text="Submit" 
                            onclick="ASPxButton1_Click" CssClass="sysButton">
                        </dx:ASPxButton>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                </tr>
            </table>
                </ContentTemplate>
            </asp:UpdatePanel>
            <table>
                <tr>
                    <td>
                        <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="Date Range:" 
                            CssClass="lblsmallFont" Wrap="False">
                        </dx:ASPxLabel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <uc1:DateRange ID="dtPick" runat="server" Width="100px" Height="100%"></uc1:DateRange>
                    </td>
                    
                </tr>            
            </table>
                        
                    </td>
        <td valign="top">
            <div class="header" id="servernamelbldisp" runat="server">Sametime Statistics</div>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <dx:ASPxPivotGrid ID="SametimeStatsPivotGrid" runat="server" 
                        ClientIDMode="AutoID" EnableTheming="True" Theme="Office2003Blue" OnUnload="SametimeStatsPivotGrid_Unload">
                        <Fields>
                            <dx:PivotGridField ID="fieldServerName" Area="RowArea" AreaIndex="0" 
                                Caption="Server Name" FieldName="ServerName" TotalsVisibility="None">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fieldDate" Area="ColumnArea" AreaIndex="0" 
                                Caption="Date" FieldName="Date" TotalsVisibility="None" 
                                ValueFormat-FormatType="DateTime" ValueFormat-FormatString="d">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fieldStatName" Area="RowArea" AreaIndex="1" 
                                Caption="Stat Name" FieldName="StatName" TotalsVisibility="None">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fieldStatValue" Area="DataArea" AreaIndex="0" 
                                Caption="Stat Value" FieldName="StatValue" TotalsVisibility="None">
                            </dx:PivotGridField>
                        </Fields>
                        <OptionsView ShowColumnGrandTotalHeader="False" ShowColumnGrandTotals="False" 
                            ShowColumnHeaders="False" ShowColumnTotals="False" ShowDataHeaders="False" 
                            ShowFilterHeaders="False" ShowRowGrandTotalHeader="False" 
                            ShowRowGrandTotals="False" ShowRowTotals="False" 
                            ShowHorizontalScrollBar="True" />
                        <OptionsPager AlwaysShowPager="True">
                            <PageSizeItemSettings Visible="True">
                            </PageSizeItemSettings>
                        </OptionsPager>
                        <Styles>
                            <DataAreaStyle CssClass="GridAltRow">
                            </DataAreaStyle>
                        </Styles>
                    </dx:ASPxPivotGrid>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="ASPxButton1" />
                    <asp:PostBackTrigger ControlID="ASPxMenu1" />
                </Triggers>
            </asp:UpdatePanel>
            <dx:ASPxPivotGridExporter ID="ServerGridViewExporter" ASPxPivotGridID="SametimeStatsPivotGrid" runat="server">
        </dx:ASPxPivotGridExporter>
        </td>
        <td align="right" valign="top">
            <table>
                <tr>
                    <td>
                        <dx:ASPxMenu ID="ASPxMenu1" runat="server" ShowAsToolbar="True" Theme="Moderno" 
                            onitemclick="ASPxMenu1_ItemClick">
                        <ClientSideEvents ItemClick="OnItemClick" />
                    <Items>
                        <dx:MenuItem Name="MainItem">
                            <Items>
                                <dx:MenuItem Name="ExportXLSItem" Text="Export to XLS">
                                </dx:MenuItem>
                                <dx:MenuItem Name="ExportXLSXItem" Text="Export to XLSX">
                                </dx:MenuItem>
                                <dx:MenuItem Name="ExportPDFItem" Text="Export to PDF">
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
</table>
</asp:Content>
