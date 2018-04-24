<%@ Page Title="Domino Server Configuration" Language="C#" MasterPageFile="~/Reports.Master" AutoEventWireup="true" CodeBehind="DominoServerThreshold.aspx.cs" Inherits="VSWebUI.Configurator.DominoServerThreshold" %>
<%@ Register assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>
<%@ MasterType virtualpath="~/Reports.Master" %>







<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   <link href="../css/bootstrap1.min.css" rel="stylesheet" />
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
        <td class="tdpadded">
            <div class="input-prepend">&nbsp;</div>
            <table width="100%">
    <tr>
        <td>
            <div class="header" id="titleDiv" runat="server">Domino Server Configuration Settings</div>
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
            
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
<ContentTemplate>
                <dx:ASPxGridView ID="DominoServerReportsGridView" runat="server" ClientInstanceName="grid"
        AutoGenerateColumns="False" Theme="Office2003Blue" Width="100%" KeyFieldName = "ServerID"
                    oncustomcallback="DominoServerReportsGridView_CustomCallback" OnPageSizeChanged="DominoServerReportsGridView_PageSizeChanged">
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
            <dx:GridViewDataTextColumn Caption="ID" FieldName="ServerID" 
                VisibleIndex="0" Width="200px" Visible="False">
                <Settings AllowHeaderFilter="True" AutoFilterCondition="Contains" 
                     />
                <CellStyle CssClass="GridCss">
                </CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Server Name" FieldName="ServerName" 
                VisibleIndex="1" Width="200px">
                <Settings AllowHeaderFilter="True" AutoFilterCondition="Contains" 
                    HeaderFilterMode="CheckedList" />
                <CellStyle CssClass="GridCss">
                </CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Pending Threshold" FieldName="PendingThreshold" 
                VisibleIndex="3">
                <Settings AllowAutoFilter="False" />
                <HeaderStyle Wrap="True" CssClass="GridCssHeader2" />
                <CellStyle CssClass="GridCss2" HorizontalAlign="Right">
                </CellStyle>
                <FooterCellStyle Wrap="True">
                </FooterCellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Dead Mail Threshold" 
                FieldName="DeadThreshold" VisibleIndex="6">
                <Settings AllowAutoFilter="False" />
                <HeaderStyle CssClass="GridCssHeader2" Wrap="True" />
                <CellStyle CssClass="GridCss2">
                </CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Enabled" FieldName="Enabled" 
                VisibleIndex="2">
                <Settings AllowAutoFilter="False" />
                <HeaderStyle Wrap="True" CssClass="GridCssHeader2" />
                <CellStyle CssClass="GridCss2" HorizontalAlign="Right">
                </CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Scan Interval" 
                FieldName="Scan Interval" VisibleIndex="4">
                <Settings AllowAutoFilter="False" />
                <HeaderStyle Wrap="True" CssClass="GridCssHeader2" />
                <CellStyle CssClass="GridCss2">
                </CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Off-Hours Scan Interval" 
                FieldName="OffHoursScanInterval" VisibleIndex="5">
                <Settings AllowAutoFilter="False" />
                <HeaderStyle Wrap="True" CssClass="GridCssHeader2" />
                <CellStyle CssClass="GridCss2">
                </CellStyle>
            </dx:GridViewDataTextColumn>
            
            <dx:GridViewDataTextColumn Caption="Retry Interval" FieldName="RetryInterval" 
                VisibleIndex="8">
                <Settings AllowAutoFilter="False" />
                <HeaderStyle CssClass="GridCssHeader2" Wrap="True" />
                <CellStyle CssClass="GridCss2">
                </CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Response Threshold" FieldName="ResponseThreshold" 
                VisibleIndex="9">
                <Settings AllowAutoFilter="False" />
                <HeaderStyle CssClass="GridCssHeader2" Wrap="True" />
                <CellStyle CssClass="GridCss2">
                </CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="BES Server" FieldName="BES_Server" 
                VisibleIndex="10">
                <Settings AllowAutoFilter="False" />
                <HeaderStyle CssClass="GridCssHeader2" Wrap="True" />
                <CellStyle CssClass="GridCss2">
                </CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="BES Threshold" FieldName="BES_Threshold" 
                VisibleIndex="11">
                <Settings AllowAutoFilter="False" />
                <HeaderStyle CssClass="GridCssHeader2" Wrap="True" />
                <CellStyle CssClass="GridCss2">
                </CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Failure Threshold" FieldName="FailureThreshold" 
                VisibleIndex="12">
                <Settings AllowAutoFilter="False" />
                <HeaderStyle CssClass="GridCssHeader2" Wrap="True" />
                <CellStyle CssClass="GridCss2">
                </CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Search String" FieldName="SearchString" 
                VisibleIndex="13" Visible="False">
                <Settings AllowAutoFilter="False" />
                <HeaderStyle CssClass="GridCssHeader2" Wrap="True" />
                <CellStyle CssClass="GridCss2">
                </CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Advanced Mail Scan" FieldName="AdvancedMailScan" 
                VisibleIndex="14" Visible="False">
                <Settings AllowAutoFilter="False" />
                <HeaderStyle CssClass="GridCssHeader2" Wrap="True" />
                <CellStyle CssClass="GridCss2">
                </CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Dead Mail Delete Threshold" FieldName="DeadMailDeleteThreshold"
                VisibleIndex="15">
                <Settings AllowAutoFilter="False" />
                <HeaderStyle CssClass="GridCssHeader2" Wrap="True" />
                <CellStyle CssClass="GridCss2">
                </CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Disk Space Threshold" FieldName="DiskSpaceThreshold"
                VisibleIndex="16" 
                UnboundType="Decimal" Visible="False">
                <PropertiesTextEdit EncodeHtml="False" DisplayFormatString="{0:#0.##%}" 
                    EnableClientSideAPI="True">
                </PropertiesTextEdit>
                <Settings AllowAutoFilter="False" />
                <HeaderStyle CssClass="GridCssHeader2" Wrap="True" />
                <CellStyle CssClass="GridCss2">
                </CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Held Threshold" FieldName="HeldThreshold" 
                VisibleIndex="17">
                <Settings AllowAutoFilter="False" />
                <HeaderStyle CssClass="GridCssHeader2" Wrap="True" />
                <CellStyle CssClass="GridCss2">
                </CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Scan DB Health" FieldName="ScanDBHealth" 
                VisibleIndex="18">
                <Settings AllowAutoFilter="False" />
                <HeaderStyle CssClass="GridCssHeader2" Wrap="True" />
                <CellStyle CssClass="GridCss2">
                </CellStyle>
            </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Memory Threshold" FieldName="Memory_Threshold" 
                VisibleIndex="20" UnboundType="Decimal" >
                            <PropertiesTextEdit DisplayFormatString="{0:#0.##%}" EnableClientSideAPI="True" 
                                EncodeHtml="False">
                            </PropertiesTextEdit>
                <Settings AllowAutoFilter="False" />
                <HeaderStyle CssClass="GridCssHeader2" Wrap="True" />
                <CellStyle CssClass="GridCss2">
                </CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="CPU Threshold" FieldName="CPU_Threshold" 
                VisibleIndex="21" UnboundType="Decimal">
                <PropertiesTextEdit DisplayFormatString="{0:#0.##%}" EnableClientSideAPI="True" 
                    EncodeHtml="False">
                </PropertiesTextEdit>
                <Settings AllowAutoFilter="False" />
                <HeaderStyle CssClass="GridCssHeader2" Wrap="True" />
                <CellStyle CssClass="GridCss2">
                </CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Cluster Rep Delays Threshold" FieldName="Cluster_Rep_Delays_Threshold" 
                VisibleIndex="22">
                <Settings AllowAutoFilter="False" />
                <HeaderStyle CssClass="GridCssHeader2" Wrap="True" />
                <CellStyle CssClass="GridCss2">
                </CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Modified By" FieldName="Modified_By" 
                VisibleIndex="23" Visible="False">
                <Settings AllowAutoFilter="False" />
                <HeaderStyle CssClass="GridCssHeader2" Wrap="True" />
                <CellStyle CssClass="GridCss2">
                </CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Modified On" FieldName="Modified_On" 
                VisibleIndex="24" Visible="False">
                <Settings AllowAutoFilter="False" />
                <HeaderStyle CssClass="GridCssHeader2" Wrap="True" />
                <CellStyle CssClass="GridCss2">
                </CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Server Days Alert" FieldName="ServerDaysAlert" 
                VisibleIndex="25">
                <Settings AllowAutoFilter="False" />
                <HeaderStyle CssClass="GridCssHeader2" Wrap="True" />
                <CellStyle CssClass="GridCss2">
                </CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Monitored By" FieldName="MonitoredBy" 
                VisibleIndex="26" Visible="False">
                <Settings AllowAutoFilter="False" />
                <HeaderStyle CssClass="GridCssHeader2" Wrap="True" />
                <CellStyle CssClass="GridCss2">
                </CellStyle>
            </dx:GridViewDataTextColumn>

           

        </Columns>
        <SettingsBehavior AllowFocusedRow="True" AllowDragDrop="True" 
                                                AutoExpandAllGroups="True" AllowSelectByRowClick="True" 
                                                ProcessSelectionChangedOnServer="True" ColumnResizeMode="NextColumn" />
                                            <Settings ShowHorizontalScrollBar="False" />

<Settings ShowFilterRow="True" ShowHorizontalScrollBar="True"></Settings>
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
        <asp:PostBackTrigger ControlID="ASPxMenu1" />
    </Triggers>
    </asp:UpdatePanel>
            </td>
    </tr>
<tr>
    <td>     
        <dx:ASPxGridViewExporter ID="ServerGridViewExporter" runat="server" 
            GridViewID="DominoServerReportsGridView">
        </dx:ASPxGridViewExporter>
        
    </td>
</tr>
</table>            
        </td>
    </tr>
</table>
</asp:Content>