﻿<%@ Page Language="C#" Title="VitalSigns Plus - IBM Connections Community Activity" MasterPageFile="~/Reports.Master"  AutoEventWireup="true" CodeBehind="IBMConnectionsCommunityActivityRpt.aspx.cs" Inherits="VSWebUI.DashboardReports.IBMConnectionsCommunityActivityRpt" %>

<%@ Register Assembly="DevExpress.Web.ASPxPivotGrid.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPivotGrid" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../css/bootstrap1.min.css" rel="stylesheet" />
   <style type="text/css">
    .tdpadded
    {
        padding-left:20px;
    }
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<script type="text/javascript">
    function OnItemClick(s, e) {
        if (e.item.parent == s.GetRootItem())
            e.processOnServer = false;
    }
</script>
    <table>
        <tr>
            <td>
                <div class="header" id="servernamelbldisp" runat="server">Community Activity</div>
            </td>
            <td align="right" valign="top">
                <table>
                <tr>
                    <td>
                        <dx:ASPxMenu ID="ASPxMenu1" runat="server" ShowAsToolbar="True" Theme="Moderno" 
                            onitemclick="ASPxMenu1_ItemClick" ClientInstanceName="menu1" 
                            AutoPostBack="True">
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
        <tr>
             <td colspan="2">
                 <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <dx:ASPxPivotGrid ID="CommunityActivityPivotGrid" runat="server" 
                            ClientIDMode="AutoID" EnableTheming="True" 
                            oncustomfieldsort="CommunityActivityPivotGrid_CustomFieldSort" 
                            Theme="Office2003Blue">
                            <Fields>
                                <dx:PivotGridField ID="fieldTotal" Area="DataArea" AreaIndex="0" 
                                    Caption="Total" FieldName="Total">
                                </dx:PivotGridField>
                                <dx:PivotGridField ID="fieldName" Area="RowArea" AreaIndex="0" Caption="Name" 
                                    FieldName="Name">
                                </dx:PivotGridField>
                                <dx:PivotGridField ID="fieldType" Area="RowArea" AreaIndex="1" Caption="Object" 
                                    FieldName="Type">
                                </dx:PivotGridField>
                                <dx:PivotGridField ID="fieldLastDate" Area="ColumnArea" AreaIndex="0" 
                                    Caption="Date Range" FieldName="LastDate" SortMode="Custom">
                                </dx:PivotGridField>
                            </Fields>
                            <OptionsView ShowColumnGrandTotalHeader="False" ShowColumnGrandTotals="False" 
                                ShowDataHeaders="False" ShowRowGrandTotalHeader="False" 
                                ShowRowGrandTotals="False" />
                        </dx:ASPxPivotGrid>
                        <dx:ASPxPivotGridExporter ID="ServerGridViewExporter" runat="server" 
                            ASPxPivotGridID="CommunityActivityPivotGrid">
                        </dx:ASPxPivotGridExporter>
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="ASPxMenu1" />
                    </Triggers>
                 </asp:UpdatePanel>
             </td>
        </tr>
    </table>
</asp:Content>
