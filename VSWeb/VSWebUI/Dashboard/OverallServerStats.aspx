<%@ Page Title="VitalSigns Plus - Overall Server Stats" Language="C#" AutoEventWireup="true" MasterPageFile="~/DashboardSite.Master" CodeBehind="OverallServerStats.aspx.cs" Inherits="VSWebUI.Dashboard.OverallServerStats" %>
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
            <div class="header" id="servernamelbldisp" runat="server">Overall Domino Statistics</div>
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
                <dx:ASPxGridView ID="ServerStatsGridView" runat="server" ClientInstanceName="grid"
        AutoGenerateColumns="False" Theme="Office2003Blue" Width="100%" 
                    oncustomcallback="ServerStatsGridView_CustomCallback" OnPageSizeChanged="ServerStatsGridView_PageSizeChanged">
                    <TotalSummary>
                        <dx:ASPxSummaryItem FieldName="MailDeliv" ShowInColumn="Total Mail Delivered" 
                            SummaryType="Sum" DisplayFormat="{0:n0}" />
                        <dx:ASPxSummaryItem DisplayFormat="{0:n0}" FieldName="DownTime" 
                            ShowInColumn="Down Time (min)" SummaryType="Sum" />
                        <dx:ASPxSummaryItem DisplayFormat="{0:n0}" FieldName="WebOpenDoc" 
                            ShowInColumn="Web Databases Opened" SummaryType="Sum" />
                        <dx:ASPxSummaryItem DisplayFormat="{0:n0}" FieldName="WebCreateDoc" 
                            ShowInColumn="Web Documents Created" 
                            ShowInGroupFooterColumn="Web Documents Created" SummaryType="Sum" />
                        <dx:ASPxSummaryItem DisplayFormat="{0:n0}" FieldName="WebOpenView" 
                            ShowInColumn="Web Views Opened" SummaryType="Sum" />
                        <dx:ASPxSummaryItem DisplayFormat="{0:n0}" FieldName="WebComTotal" 
                            ShowInColumn="Web Commands Total" SummaryType="Sum" />
                    </TotalSummary>
        <Columns>
            <dx:GridViewDataTextColumn Caption="Server Name" FieldName="ServerName" 
                VisibleIndex="0" Width="200px">
                <Settings AllowHeaderFilter="True" AutoFilterCondition="Contains"  AllowAutoFilterTextInputTimer="False"
                    HeaderFilterMode="CheckedList" />
                <CellStyle CssClass="GridCss">
                </CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Total Mail Delivered" FieldName="MailDeliv" 
                VisibleIndex="1">
                <Settings AllowAutoFilter="False" />
                <HeaderStyle Wrap="True" CssClass="GridCssHeader2" />
                <CellStyle CssClass="GridCss2" HorizontalAlign="Right">
                </CellStyle>
                <FooterCellStyle Wrap="True">
                </FooterCellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Avg Memory % Available" 
                FieldName="MemAvgAvail" VisibleIndex="5">
                <Settings AllowAutoFilter="False" />
                <HeaderStyle CssClass="GridCssHeader2" Wrap="True" />
                <CellStyle CssClass="GridCss2">
                </CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Down Time (min)" FieldName="DownTime" 
                VisibleIndex="4">
                <Settings AllowAutoFilter="False" />
                <HeaderStyle Wrap="True" CssClass="GridCssHeader2" />
                <CellStyle CssClass="GridCss2" HorizontalAlign="Right">
                </CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Avg Mail Delivery Time (ms)" 
                FieldName="MailAvgDeliv" VisibleIndex="2">
                <Settings AllowAutoFilter="False" />
                <HeaderStyle Wrap="True" CssClass="GridCssHeader2" />
                <CellStyle CssClass="GridCss2">
                </CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Avg Server Availability Index" 
                FieldName="SrvAvailInd" VisibleIndex="3">
                <Settings AllowAutoFilter="False" />
                <HeaderStyle Wrap="True" CssClass="GridCssHeader2" />
                <CellStyle CssClass="GridCss2">
                </CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Web Documents Opened" FieldName="WebOpenDoc" 
                VisibleIndex="6">
                <Settings AllowAutoFilter="False" />
                <HeaderStyle CssClass="GridCssHeader2" Wrap="True" />
                <CellStyle CssClass="GridCss2">
                </CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Web Documents Created" FieldName="WebCreateDoc" 
                VisibleIndex="7">
                <Settings AllowAutoFilter="False" />
                <HeaderStyle CssClass="GridCssHeader2" Wrap="True" />
                <CellStyle CssClass="GridCss2">
                </CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Web Databases Opened" FieldName="WebOpenDb" 
                VisibleIndex="8">
                <Settings AllowAutoFilter="False" />
                <HeaderStyle CssClass="GridCssHeader2" Wrap="True" />
                <CellStyle CssClass="GridCss2">
                </CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Web Views Opened" FieldName="WebOpenView" 
                VisibleIndex="9">
                <Settings AllowAutoFilter="False" />
                <HeaderStyle CssClass="GridCssHeader2" Wrap="True" />
                <CellStyle CssClass="GridCss2">
                </CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Web Commands Total" FieldName="WebComTotal" 
                VisibleIndex="10">
                <Settings AllowAutoFilter="False" />
                <HeaderStyle CssClass="GridCssHeader2" Wrap="True" />
                <CellStyle CssClass="GridCss2">
                </CellStyle>
            </dx:GridViewDataTextColumn>
        </Columns>
                    <SettingsBehavior ColumnResizeMode="NextColumn" />
        <SettingsPager AlwaysShowPager="True" PageSize="20">
            <PageSizeItemSettings Visible="True">
            </PageSizeItemSettings>
        </SettingsPager>
        <Settings ShowFilterRow="True" ShowFooter="True" />
        <Styles>
            <Header CssClass="GridCssHeader">
            </Header>
            <AlternatingRow CssClass="GridAltRow" Enabled="True">
            </AlternatingRow>
            <Footer CssClass="GridCss2">
            </Footer>
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