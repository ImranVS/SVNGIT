<%@ Page Title="VitalSigns Plus - Overall Mail Stats" Language="C#" AutoEventWireup="true" MasterPageFile="~/DashboardSite.Master" CodeBehind="OverallMailStats.aspx.cs" Inherits="VSWebUI.Dashboard.OverallMailStats" %>
<%@ MasterType virtualpath="~/DashboardSite.Master" %>
<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>
	







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
            grid.PerformCallback();
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
            <div class="header" id="titleDiv" runat="server">Overall Mail Statistics</div>
        </td>
        <td>
            &nbsp;
        </td>    
        <td align="right">
            <table>
                <tr>
                    <td>
                        <dx:ASPxMenu ID="ASPxMenu1" runat="server" onitemclick="ASPxMenu1_ItemClick" 
                            ShowAsToolbar="True" Theme="Moderno">
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
    <table width="100%">
        <tr>
            <td>
                <table>
                    <tr>
                        <td>
                            <label class="lblsmallFont" for="startDate">
                    Month/Year:</label>
                    </td>
                        <td>
                            <input runat="server" ID="startDate" type="text" class="date-picker" />
                        </td>
                        <td>
                            &nbsp;</td>
                        <td>
                                <dx:ASPxButton ID="ExportXlsButton" runat="server" 
                            onclick="ExportXlsButton_Click" Text="Export to XLS" Theme="Office2010Blue" 
                            Wrap="False" Visible="False">
                        </dx:ASPxButton>
                        </td>
                        <td>
                        <dx:ASPxButton ID="ExportXlsxButton" runat="server" 
                            onclick="ExportXlsxButton_Click" Text="Export to XLSX" Theme="Office2010Blue" 
                            Wrap="False" Visible="False">
                        </dx:ASPxButton>
                    </td>
                    <td>
                        <dx:ASPxButton ID="ExportPdfButton" runat="server" 
                            onclick="ExportPdfButton_Click" Text="Export to PDF" Theme="Office2010Blue" 
                            Wrap="False" Visible="False">
                        </dx:ASPxButton>
                    </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                <dx:ASPxGridView ID="ServerStatsGridView" runat="server" AutoGenerateColumns="False" 
                        ClientInstanceName="grid" EnableTheming="True" Theme="Office2003Blue" 
                        Width="100%" OnPageSizeChanged="ServerStatsGridView_PageSizeChanged">
                    <Columns>
                        <dx:GridViewDataTextColumn Caption="Server Name" FieldName="ServerName" 
                            VisibleIndex="0" Width="200px">
                            <Settings AllowHeaderFilter="True" AutoFilterCondition="Contains"  AllowAutoFilterTextInputTimer="False"
                                HeaderFilterMode="CheckedList" />
                            <HeaderStyle CssClass="GridCssHeader" Wrap="False" />
                            <CellStyle CssClass="GridCss" Wrap="False">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Total Routed" FieldName="TotalRouted" 
                            VisibleIndex="1">
                            <Settings AllowAutoFilter="False" />
                            <HeaderStyle CssClass="GridCssHeader" Wrap="False" />
                            <CellStyle CssClass="GridCss2">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Delivered" FieldName="Delivered" 
                            VisibleIndex="2">
                            <Settings AllowAutoFilter="False" />
                            <HeaderStyle CssClass="GridCssHeader" Wrap="False" />
                            <CellStyle CssClass="GridCss2">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Transfer Failures" VisibleIndex="3" 
                            FieldName="TransferFailures" Width="120px">
                            <Settings AllowAutoFilter="False" />
                            <HeaderStyle CssClass="GridCssHeader" Wrap="False" />
                            <CellStyle CssClass="GridCss2">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Total Pending" VisibleIndex="4" 
                            FieldName="TotalPending">
                            <Settings AllowAutoFilter="False" />
                            <HeaderStyle CssClass="GridCssHeader" Wrap="False" />
                            <CellStyle CssClass="GridCss2">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Avg Delivery Time (seconds)" 
                            VisibleIndex="5" FieldName="AvgDelivTime" Width="120px">
                            <Settings AllowAutoFilter="False" />
                            <HeaderStyle CssClass="GridCssHeader" Wrap="True" />
                            <CellStyle CssClass="GridCss2">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Avg Server Hops" 
                            VisibleIndex="6" FieldName="AvgSrvHops" Width="120px">
                            <Settings AllowAutoFilter="False" />
                            <HeaderStyle CssClass="GridCssHeader" Wrap="False" />
                            <CellStyle CssClass="GridCss2">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Avg Size Delivered" VisibleIndex="7" 
                            FieldName="AvgSizeDeliv" Width="120px">
                            <Settings AllowAutoFilter="False" />
                            <HeaderStyle CssClass="GridCssHeader" Wrap="False" />
                            <CellStyle CssClass="GridCss2">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="SMTP Messages Processed" VisibleIndex="9" 
                            FieldName="SMTPMsgProc" Width="180px">
                            <Settings AllowAutoFilter="False" />
                            <HeaderStyle CssClass="GridCssHeader" Wrap="False" />
                            <CellStyle CssClass="GridCss2">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                    </Columns>
                    <SettingsPager AlwaysShowPager="True" PageSize="20">
                        <PageSizeItemSettings Visible="True">
                        </PageSizeItemSettings>
                    </SettingsPager>
                    <Settings ShowFilterRow="True" ShowFooter="True" />
                    <Styles>
                        <AlternatingRow CssClass="GridCssAltrow">
                        </AlternatingRow>
                    </Styles>
        </dx:ASPxGridView>
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
        <dx:ASPxGridViewExporter ID="ServerGridViewExporter" runat="server" 
            GridViewID="ServerStatsGridView">
        </dx:ASPxGridViewExporter>
        
    </td>
</tr>
</table>
</asp:Content>