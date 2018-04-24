<%@ Page Language="C#" MasterPageFile="~/Reports.Master" AutoEventWireup="true" CodeBehind="DominoDiskAnnualTrendRpt.aspx.cs" Inherits="VSWebUI.DashboardReports.DominoDiskAnnualTrend" %>

<%@ Register assembly="DevExpress.XtraReports.v14.2.Web, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.XtraReports.Web" tagprefix="dx" %>

<%@ Register assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>

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
            $('.date-picker-year').datepicker({
                changeYear: true,
                showButtonPanel: true,
                dateFormat: 'yy',
                stepMonths: 12,
                onClose: function (dateText, inst) {
                    //var month = $("#ui-datepicker-div .ui-datepicker-month :selected").val();
                    var year = $("#ui-datepicker-div .ui-datepicker-year :selected").val();
                    $(this).datepicker('setDate', new Date(year, 1));
                }
            });
            $(".date-picker-year").focus(function () {
                $(".ui-datepicker-month").hide();
            });
        });
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
    <table>
        <tr>
            <td class="tdpadded" valign="top">
                <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <table>
                    <tr>
                        <td>
                            
                            <dx:ASPxButton ID="SubmitButton" runat="server" onclick="SubmitButton_Click" 
                        Text="Submit" CssClass="sysButton">
                    </dx:ASPxButton>
                        </td>
                        <td>
                            <dx:ASPxButton ID="ServerListResetButton" runat="server" Text="Reset" CssClass="sysButton"
                       onclick="ServerListResetButton_Click">
                    </dx:ASPxButton>        
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                    </tr>
                </table>                    
                    </ContentTemplate>
                </asp:UpdatePanel>
                <dx:ASPxButton ID="ReptBtn" runat="server" Text="Reports" 
            onclick="ReptBtn_Click" Theme="Office2010Blue" Visible="False">
        </dx:ASPxButton>    
                <table>
                    <tr>
            <td>
                    <dx:ASPxLabel ID="ASPxLabel3" runat="server" CssClass="lblsmallFont" 
                        Text="Server Type:">
                    </dx:ASPxLabel>
            </td>
        </tr>
        <tr>
            <td valign="top">
                    <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                        <ContentTemplate>
                            <dx:ASPxListBox ID="ServerTypeFilterListBox" runat="server" AutoPostBack="True" 
                                ValueType="System.String">
                            </dx:ASPxListBox>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="ServerListResetButton" />
                        </Triggers>
                    </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td>
                    <dx:ASPxLabel ID="ASPxLabel1" runat="server" 
                        Text="Server(s):" CssClass="lblsmallFont">
                    </dx:ASPxLabel>
            </td>
        </tr>
        <tr>
            <td>
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                        <dx:ASPxListBox ID="ServerListFilterListBox" runat="server" 
                            SelectionMode="CheckColumn" ValueType="System.String">
                        </dx:ASPxListBox>
                    </ContentTemplate>
                    <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="ServerListResetButton" />
                            <asp:AsyncPostBackTrigger ControlID="ServerTypeFilterListBox" />
                        </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td>
                    <dx:ASPxLabel ID="ASPxLabel2" runat="server" 
                        Text="Disk(s):" CssClass="lblsmallFont">
                    </dx:ASPxLabel>
            </td>
        </tr>
        <tr>
            <td>
                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                        <ContentTemplate>
                            <dx:ASPxListBox ID="DiskListFilterListBox" runat="server" 
                                SelectionMode="CheckColumn" ValueType="System.String">
                            </dx:ASPxListBox>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="ServerListResetButton" />
                            <asp:AsyncPostBackTrigger ControlID="ServerTypeFilterListBox" />
                        </Triggers>
                    </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td>
                <label class="lblsmallFont" for="startDate">
                    Year:</label>
            </td>
        </tr>
        <tr>
            <td>
                <input id="startDate" runat="server" class="date-picker-year" name="startDate" />
                    <div class="input-prepend">&nbsp;</div>
            </td>
        </tr>
                </table>
                </td>
                <td valign="top">
                    <table>
        <tr>
            <td>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <dx:ASPxDocumentViewer ID="ASPxDocumentViewer1" runat="server" Width="910px" Theme="Moderno" SettingsSplitter-SidePaneVisible="False">
                    </dx:ASPxDocumentViewer>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="SubmitButton" />
                    <asp:AsyncPostBackTrigger ControlID="ServerListResetButton" />
                </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>                
                </td>
        </tr>
        </table>
    
</asp:Content>