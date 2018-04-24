<%@ Page Title="VitalSigns Plus - Daily Memory Used" Language="C#" MasterPageFile="~/Reports.Master" AutoEventWireup="true" CodeBehind="DailyMemoryUsedTab.aspx.cs" Inherits="VSWebUI.DashboardReports.DailyMemoryUsedTab" %>

<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>

<%@ Register assembly="DevExpress.Web.ASPxPivotGrid.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxPivotGrid" tagprefix="dx" %>




<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<link href="../css/bootstrap1.min.css" rel="stylesheet" />  
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<link rel="stylesheet" type="text/css" media="screen" href="../css/jquery-ui-1.10.3.custom.css" />
    <link rel="stylesheet" type="text/css" media="screen" href="../css/jquery-ui-1.10.3.custom.min.css" />
<script type="text/javascript" src="../js/jquery-ui-1.10.3.custom.js"></script>
    <script type="text/javascript" src="../js/jquery-ui-1.10.3.custom.min.js"></script>
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
       button.ui-datepicker-current { display: none; }
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
        <td valign="top">
            <table>
                <tr>
                    <td>
                        <dx:ASPxButton ID="ViewChartButton" runat="server" 
                onclick="ViewChartButton_Click" Text="View Chart" CssClass="sysButton">
            </dx:ASPxButton>
            <div class="input-prepend">&nbsp;</div>
                    </td>
                </tr>
                <tr>
                    <td></td>
                </tr>
            </table>
            <table>
                <tr>
        <td>
             
            <dx:ASPxLabel ID="ASPxLabel1" runat="server" CssClass="lblsmallFont" 
                Text="Month/Year:">
            </dx:ASPxLabel>
             
        </td>
        </tr>
        <tr>
        <td valign="top">
        <input id="startDate" runat="server" class="date-picker" name="startDate" />     
        </td>
    </tr>
            </table>
        </td>
        <td valign="top">
            <div class="header" id="servernamelbldisp" runat="server">Daily Memory Used</div>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <dx:ASPxPivotGrid ID="DailyMemoryPivotGrid" ClientInstanceName="pivot" runat="server" 
                Theme="Office2003Blue" ClientIDMode="AutoID">
                <Fields>
                    <dx:PivotGridField ID="fieldServerName" Area="RowArea" AreaIndex="1" 
                        Caption="Server Name" FieldName="ServerName">
                    </dx:PivotGridField>
                    <dx:PivotGridField ID="fielddate" Area="ColumnArea" AreaIndex="0" 
                        Caption="Date" FieldName="date" ValueFormat-FormatString="d" 
                        ValueFormat-FormatType="DateTime">
                    </dx:PivotGridField>
                    <dx:PivotGridField ID="fieldStatValue" Area="DataArea" AreaIndex="0" 
                        Caption="Memory Used" FieldName="StatValue">
                    </dx:PivotGridField>
                    <dx:PivotGridField ID="fieldServerType" Area="RowArea" AreaIndex="0" 
                        Caption="Server Type" FieldName="ServerType">
                    </dx:PivotGridField>
                </Fields>
                <OptionsView ShowColumnGrandTotalHeader="False" ShowColumnGrandTotals="False" 
                    ShowColumnHeaders="False" ShowColumnTotals="False" ShowDataHeaders="False" 
                    ShowFilterHeaders="False" ShowHorizontalScrollBar="True" 
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
            <dx:ASPxPivotGridExporter ID="ServerGridViewExporter" runat="server">
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