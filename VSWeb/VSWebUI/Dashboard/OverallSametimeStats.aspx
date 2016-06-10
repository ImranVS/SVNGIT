<%@ Page Title="VitalSigns Plus - Overall Sametime Stats" Language="C#" AutoEventWireup="true" MasterPageFile="~/DashboardSite.Master" CodeBehind="OverallSametimeStats.aspx.cs" Inherits="VSWebUI.Dashboard.OverallSametimeStats" %>
<%@ MasterType virtualpath="~/DashboardSite.Master" %>
<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>









<%@ Register assembly="DevExpress.Web.ASPxPivotGrid.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxPivotGrid" tagprefix="dx" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<script type="text/javascript" src="../js/jquery-1.9.1.js"></script>
    <script type="text/javascript" src="../js/jquery-ui-1.10.3.custom.js"></script>
    <script type="text/javascript" src="../js/jquery-ui-1.10.3.custom.min.js"></script>
    <link rel="stylesheet" type="text/css" media="screen" href="../css/jquery-ui-1.10.3.custom.css" />
    <link rel="stylesheet" type="text/css" media="screen" href="../css/jquery-ui-1.10.3.custom.min.css" />
    <script type="text/javascript">
        $(function () {
            $('.date-picker').datepicker({
                changeMonth: true,
                changeYear: true,
                showButtonPanel: true,
                dateFormat: 'MM yy',
                onClose: function (dateText, inst) {
                    var month = $("#ui-datepicker-div .ui-datepicker-month :selected").val();
                    var year = $("#ui-datepicker-div .ui-datepicker-year :selected").val();
                    $(this).datepicker('setDate', new Date(year, month, 1));
                    DoCallback();
                }
            });
        });
        function DoCallback() {
            pivot.PerformCallback();
        }
        function OnItemClick(s, e) {
            if (e.item.parent == s.GetRootItem())
                e.processOnServer = false;
        }
</script>
   <style type="text/css">
    .tdpadded
    {
        padding-left:20px;
    }
    .ui-datepicker-calendar {
    display: none;
    }
</style>
<table width="100%">
    <tr>
        <td>
            <div class="header" id="servernamelbldisp" runat="server">Overall Sametime Statistics</div>
        </td>
        <td>
            &nbsp;
        </td>    
        <td align="right">
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
<table width="100%">
    <tr>
        <td>
            <table class="navbarTbl">
        <tr>
            <td>
                <table>
                    <tr>
                        <td>
                            <label class="lblsmallFont" for="startDate">
                            Month/Year:</label>
                        </td>
                        <td>
                            <input runat="server" ID="startDate" type="text" class="date-picker"></input>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <dx:ASPxPivotGrid ID="SametimeStatsPivotGrid" runat="server" 
                            ClientIDMode="AutoID" ClientInstanceName="pivot" EnableTheming="True" Theme="Office2003Blue">
                            <Fields>
                                <dx:PivotGridField ID="fieldServerName" Area="RowArea" AreaIndex="0" 
                                    Caption="Server Name" FieldName="ServerName">
                                </dx:PivotGridField>
                                <dx:PivotGridField ID="fieldStatValue" Area="DataArea" AreaIndex="0" 
                                    Caption="Stat Value" FieldName="StatValue">
                                </dx:PivotGridField>
                                <dx:PivotGridField ID="fieldStatName" Area="ColumnArea" AreaIndex="0" 
                                    Caption="Stat Name" FieldName="StatName">
                                </dx:PivotGridField>
                            </Fields>
                            <OptionsView ShowColumnGrandTotalHeader="False" ShowColumnGrandTotals="False" 
                                ShowColumnTotals="False" ShowDataHeaders="False" ShowFilterHeaders="False" 
                                ShowFilterSeparatorBar="False" ShowHorizontalScrollBar="True" 
                                ShowRowGrandTotalHeader="False" ShowRowGrandTotals="False" 
                                ShowRowTotals="False" />
                            <OptionsPager AlwaysShowPager="True">
                                <PageSizeItemSettings Visible="True">
                                </PageSizeItemSettings>
                            </OptionsPager>
                        </dx:ASPxPivotGrid>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="startDate" />
                    </Triggers>
                </asp:UpdatePanel>
                
            </td>
        </tr>
    </table>
        </td>
    </tr>
    <tr>
        <td>
            <dx:ASPxPivotGridExporter ID="ServerPivotGridExporter" runat="server">
            </dx:ASPxPivotGridExporter>
        </td>
    </tr>
</table>
</asp:Content>
