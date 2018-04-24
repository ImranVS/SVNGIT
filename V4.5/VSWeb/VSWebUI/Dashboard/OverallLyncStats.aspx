<%@ Page Title="VitalSigns Plus - Overall Skype for Business Stats" Language="C#" MasterPageFile="~/DashboardSite.Master" AutoEventWireup="true" CodeBehind="OverallLyncStats.aspx.cs" Inherits="VSWebUI.Dashboard.OverallLyncStats" %>
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
            <div class="header" id="servernamelbldisp" runat="server">Overall Skype for Business Statistics</div>
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
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            <dx:ASPxButton ID="ExportXlsButton" runat="server" Text="Export to XLS" 
                                Theme="Office2010Blue" onclick="ExportXlsButton_Click" Visible="False">
                            </dx:ASPxButton>
                        </td>
                        <td>
                            <dx:ASPxButton ID="ExportXlsxButton" runat="server" Text="Export to XLSX" 
                                Theme="Office2010Blue" onclick="ExportXlsxButton_Click" Visible="False">
                            </dx:ASPxButton>
                        </td>
                        <td>
                            <dx:ASPxButton ID="ExportPdfButton" runat="server" Text="Export to PDF" 
                                Theme="Office2010Blue" onclick="ExportPdfButton_Click" Visible="False">
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
                        <dx:ASPxGridView ID="LyncStatsGridView" runat="server" 
                            ClientInstanceName="grid" AutoGenerateColumns="False" EnableTheming="True" 
                            Theme="Office2003Blue" Width="100%" OnPageSizeChanged="LyncStatsGridView_OnPageSizeChanged">
                            <Columns>
                                <dx:GridViewDataTextColumn Caption="Server Name" FieldName="ServerName" 
                                    VisibleIndex="1">
									<Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="Users" FieldName="Users" VisibleIndex="2">
                                    <Settings AllowAutoFilter="False" />
                                    <HeaderStyle CssClass="GridCssHeader2" />
                                    <CellStyle CssClass="GridCss2">
                                    </CellStyle>
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="Chat Sessions" FieldName="ChatSessions" 
                                    VisibleIndex="3">
                                    <Settings AllowAutoFilter="False" />
                                    <HeaderStyle CssClass="GridCssHeader2" />
                                    <CellStyle CssClass="GridCss2">
                                    </CellStyle>
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="n-Way Chat Sessions" FieldName="nWayChat" 
                                    VisibleIndex="4">
                                    <Settings AllowAutoFilter="False" />
                                    <HeaderStyle CssClass="GridCssHeader2" />
                                    <CellStyle CssClass="GridCss2">
                                    </CellStyle>
                                </dx:GridViewDataTextColumn>
                            </Columns>
                            <SettingsPager AlwaysShowPager="True" PageSize="20">
                                <PageSizeItemSettings Visible="True">
                                </PageSizeItemSettings>
                            </SettingsPager>
                            <Settings ShowFilterRow="True" />
                            <Styles>
                                <Header CssClass="GridCssHeader">
                                </Header>
                                <AlternatingRow CssClass="GridAltRow">
                                </AlternatingRow>
                                <Cell CssClass="GridCss">
                                </Cell>
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
                <dx:ASPxGridViewExporter ID="ServerGridViewExporter" GridViewID="LyncStatsGridView" runat="server">
                </dx:ASPxGridViewExporter>
            </td>
        </tr>
    </table>
</asp:Content>
