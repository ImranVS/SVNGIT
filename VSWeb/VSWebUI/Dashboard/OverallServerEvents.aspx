<%@ Page Title="VitalSigns Plus - Overall Event History" Language="C#" AutoEventWireup="true" MasterPageFile="~/DashboardSite.Master" CodeBehind="OverallServerEvents.aspx.cs" Inherits="VSWebUI.Dashboard.OverallServerEvents" %>
<%@ MasterType virtualpath="~/DashboardSite.Master" %>
<%@ Register assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>







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
        	EventsHistory.PerformCallback();
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
            <div class="header" id="servernamelbldisp" runat="server">Windows Event History</div>
        </td>
        <td>
            &nbsp;
        </td>    
        <td align="right">
            <table>
                <tr>
                    <td>
                        <dx:ASPxMenu ID="ASPxMenu1" runat="server" EnableTheming="True" 
                    HorizontalAlign="Right" onitemclick="EventSettings_ItemClick" ShowAsToolbar="True" 
                    Theme="Moderno">
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
        <table>
                <tr>
                    <td>
                        <dx:ASPxLabel ID="ASPxLabel1" runat="server" CssClass="lblsmallFont" 
                            Text="Month/Year:">
                        </dx:ASPxLabel>
                    </td>
                    <td>
                        <input ID="startDate" runat="server" class="date-picker" type="text"></input>
                    </td>
                    <td>
                        &nbsp;</td>
                    <td>
                        <dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="Filter by Event Log:" CssClass="lblsmallFont">
                        </dx:ASPxLabel>
                    </td>
                    <td>
                        <dx:ASPxComboBox ID="EventTypeComboBox" runat="server" ValueType="System.String" 
                            AutoPostBack="True" 
                            onselectedindexchanged="EventTypeComboBox_SelectedIndexChanged">
                        </dx:ASPxComboBox>
                    </td>
                    <td>
                        
                    </td>
                  <%-- <td style="width:450px"></td>
                       
                       
                    
                     <td valign="top" align="right">
                            <dx:ASPxMenu ID="ASPxMenu1" runat="server" EnableTheming="True" 
                    HorizontalAlign="Right" onitemclick="AlertSettings_ItemClick" ShowAsToolbar="True" 
                    Theme="Moderno">
                    <Items>
                        <dx:MenuItem Name="MainItem">
                            <Items>
                                <dx:MenuItem Name="AlertSettingsItem" Text="Alert Settings">
                                </dx:MenuItem>
                                
                            </Items>
                            <Image Url="~/images/icons/Gear.png">
                            </Image>
                        </dx:MenuItem>
                    </Items>
                </dx:ASPxMenu>
                        </td>--%>

                </tr>
            </table>
    </td>
    </tr>
    <tr>
        <td>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
<ContentTemplate>
                <dx:ASPxGridView ID="EventsHistory" runat="server"  AutoGenerateColumns="False" Theme="Office2003Blue" Width="100%" OnHtmlDataCellPrepared="EventsHistory_HtmlDataCellPrepared"  OnPageSizeChanged="EventsHistoryGridView_PageSizeChanged"  ClientInstanceName="EventsHistory">
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
		<dx:GridViewDataTextColumn Caption="Alias Name" FieldName="AliasName" 
                VisibleIndex="0" Width="100px">
                <Settings AllowHeaderFilter="True" AutoFilterCondition="Contains" 	AllowAutoFilterTextInputTimer="False" 
	

                    HeaderFilterMode="CheckedList" />
                <CellStyle CssClass="GridCss">
                </CellStyle>
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataTextColumn Caption="Server Name" FieldName="DeviceName" 
                VisibleIndex="1">
                <Settings AllowHeaderFilter="True" AutoFilterCondition="Contains" 	AllowAutoFilterTextInputTimer="False" 
                    HeaderFilterMode="CheckedList" />
                <CellStyle CssClass="GridCss">
                </CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="DeviceName" FieldName="DeviceName" Visible=false
                VisibleIndex="7">
				<Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                <CellStyle CssClass="GridCss">
                </CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="DeviceType"  Width="100px"
                FieldName="DeviceType" VisibleIndex="8">
				<Settings AllowHeaderFilter="True" AutoFilterCondition="Contains" 	AllowAutoFilterTextInputTimer="False" 
                    HeaderFilterMode="CheckedList" />
                <CellStyle CssClass="GridCss">
                </CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Log Name" FieldName="LogName"  Width="100px"
                VisibleIndex="2">
				<Settings AllowHeaderFilter="True" AutoFilterCondition="Contains" 	AllowAutoFilterTextInputTimer="False" 
                    HeaderFilterMode="CheckedList" />
                <CellStyle CssClass="GridCss">
                </CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Event Time" FieldName="EventTime"
                VisibleIndex="6">
                <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False"/>
                <CellStyle CssClass="GridCss" Wrap="False">
                </CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Source"
                FieldName="Source" VisibleIndex="5">
                <Settings AllowHeaderFilter="True" AutoFilterCondition="Contains" 	AllowAutoFilterTextInputTimer="False" 
                    HeaderFilterMode="CheckedList" />
                <CellStyle CssClass="GridCss" Wrap="False">
                </CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Message" FieldName="MessageDetails"   Width="300px"
                VisibleIndex="4">
                <CellStyle CssClass="GridCss">
                </CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Entry Type" FieldName="EntryType" Width="100px"
                VisibleIndex="3">
				<Settings AllowHeaderFilter="True" AutoFilterCondition="Contains" 	AllowAutoFilterTextInputTimer="False" 
                    HeaderFilterMode="CheckedList" />
                <CellStyle CssClass="GridCss">
                </CellStyle>
            </dx:GridViewDataTextColumn>
        </Columns>
                    <SettingsBehavior ColumnResizeMode="NextColumn" />
        <SettingsPager AlwaysShowPager="True" PageSize="20">
            <PageSizeItemSettings Visible="True">
            </PageSizeItemSettings>
        </SettingsPager>
        <Settings ShowFilterRow="True" ShowFooter="True" 
                        ShowGroupPanel="True" />
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
        <asp:AsyncPostBackTrigger ControlID="EventTypeComboBox" />
    </Triggers>
    </asp:UpdatePanel>
        </td>
    </tr>
    <tr>
        <td>
            <dx:ASPxGridViewExporter ID="ServerGridViewExporter" runat="server" 
            GridViewID="EventsHistory">
        </dx:ASPxGridViewExporter>
        </td>
    </tr>
</table>
</asp:Content>