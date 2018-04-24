<%@ Page Title="VitalSigns Plus - Overall Server Stats" Language="C#" AutoEventWireup="true" MasterPageFile="~/DashboardSite.Master" CodeBehind="OverallServerAlerts.aspx.cs" Inherits="VSWebUI.Dashboard.OverallServerAlerts" %>
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
        	AlertsHistory.PerformCallback();
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
            <div class="header" id="servernamelbldisp" runat="server">Alerts History</div>
        </td>
        <td>
            &nbsp;
        </td>    
        <td align="right">
            <table>
                <tr>
                    <td>
                        <dx:ASPxMenu ID="ASPxMenu1" runat="server" EnableTheming="True" 
                    HorizontalAlign="Right" onitemclick="AlertSettings_ItemClick" ShowAsToolbar="True" 
                    Theme="Moderno">
                    <ClientSideEvents ItemClick="OnItemClick" />
                    <Items>
                        <dx:MenuItem Name="MainItem">
                            <Items>
                                <dx:MenuItem Name="AlertSettingsItem" Text="Alert Settings">
                                </dx:MenuItem>
                                
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
                        <dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="Filter by alert definition:" CssClass="lblsmallFont">
                        </dx:ASPxLabel>
                    </td>
                    <td>
                        <dx:ASPxComboBox ID="AlertDefComboBox" runat="server" ValueType="System.String" 
                            AutoPostBack="True" 
                            onselectedindexchanged="AlertDefComboBox_SelectedIndexChanged">
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
                <dx:ASPxGridView ID="AlertsHistory" runat="server"  AutoGenerateColumns="False" Theme="Office2003Blue" Width="100%" OnPageSizeChanged="AlertsGridView_PageSizeChanged"  ClientInstanceName="AlertsHistory">
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
            <dx:GridViewDataTextColumn Caption="Server Name" FieldName="DeviceName" 
                VisibleIndex="0" Width="200px">
                <Settings AllowHeaderFilter="True" AutoFilterCondition="Contains" 	AllowAutoFilterTextInputTimer="False" 
	

                    HeaderFilterMode="CheckedList" />
                <CellStyle CssClass="GridCss">
                </CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="DeviceName" FieldName="DeviceName" Visible=false
                VisibleIndex="1">
				<Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                <CellStyle CssClass="GridCss">
                </CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="DeviceType" 
                FieldName="DeviceType" VisibleIndex="2">
				<Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                <CellStyle CssClass="GridCss">
                </CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="AlertType" FieldName="AlertType" 
                VisibleIndex="3">
				<Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                <CellStyle CssClass="GridCss">
                </CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Alerted Date" FieldName="DateTimeOfAlert" 
                VisibleIndex="5">
                <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False"/>
                <CellStyle CssClass="GridCss" Wrap="False">
                </CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Alert Cleared On" 
                FieldName="DateTimeAlertCleared" VisibleIndex="6">
                <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                <CellStyle CssClass="GridCss" Wrap="False">
                </CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Location" FieldName="Location" 
                VisibleIndex="4">
                <CellStyle CssClass="GridCss">
                </CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Details" FieldName="Details" Width="300px"
                VisibleIndex="8">
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
        <asp:AsyncPostBackTrigger ControlID="AlertDefComboBox" />
    </Triggers>
    </asp:UpdatePanel>
        </td>
    </tr>
    <tr>
        <td>
            <dx:ASPxGridViewExporter ID="ServerGridViewExporter" runat="server" 
            GridViewID="AlertsHistory">
        </dx:ASPxGridViewExporter>
        </td>
    </tr>
</table>
</asp:Content>