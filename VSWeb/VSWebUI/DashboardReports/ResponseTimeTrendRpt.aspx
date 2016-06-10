<%@ Page Language="C#" Title="VitalSigns Plus - Response Time Trend" MasterPageFile="~/Reports.Master" AutoEventWireup="true" CodeBehind="ResponseTimeTrendRpt.aspx.cs" Inherits="VSWebUI.DashboardReports.ResponseTimeTrendRpt" %>

<%@ Register Assembly="DevExpress.XtraReports.v14.2.Web, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.XtraReports.Web" TagPrefix="dx" %>

<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>

<%@ Register Src="~/Controls/DateRange.ascx" TagName="DateRange" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../css/bootstrap1.min.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <script type="text/javascript" src="../js/jquery-ui-1.10.3.custom.js"></script>
        <script type="text/javascript" src="../js/jquery-ui-1.10.3.custom.min.js"></script>
    <link rel="stylesheet" type="text/css" media="screen" href="../css/jquery-ui-1.10.3.custom.css" />
    <link rel="stylesheet" type="text/css" media="screen" href="../css/jquery-ui-1.10.3.custom.min.css" />
    <script type="text/javascript">
//        $(function () {
//            $('.date-picker').datepicker({
//                changeMonth: true,
//                changeYear: true,
//                showButtonPanel: true,
//                dateFormat: 'MM yy',
//                onClose: function (dateText, inst) {
//                    var month = $("#ui-datepicker-div .ui-datepicker-month :selected").val();
//                    var year = $("#ui-datepicker-div .ui-datepicker-year :selected").val();
//                    $(this).datepicker('setDate', new Date(year, month, 1));
//                }
//            });
//        });
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
    <table>
            <tr>
                <td class="tdpadded" valign="top">
                    <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table>
                        <tr>
                            <td>
                                <dx:ASPxButton ID="SubmitButton" runat="server" Text="Submit" CssClass="sysButton">
                                </dx:ASPxButton>
                            </td>
                            <td>
                                <dx:ASPxButton ID="ResetButton" runat="server" Text="Reset" 
                                    CssClass="sysButton" onclick="ResetButton_Click">
                                </dx:ASPxButton>
                            </td>
                        </tr>
                    </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <div class="input-prepend">&nbsp;</div>
                    <table>
                        <tr>
                            <td>
                                <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="Server Type:" CssClass="lblsmallFont">
                                </dx:ASPxLabel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <dx:ASPxListBox ID="ServerTypeFilterListBox" runat="server" ValueType="System.String" AutoPostBack="True">
                                        </dx:ASPxListBox>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="ResetButton" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="Server(s):" CssClass="lblsmallFont">
                                </dx:ASPxLabel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <dx:ASPxListBox ID="ServerListFilterListBox" runat="server" ValueType="System.String" SelectionMode="CheckColumn" ValueField="ServerName" TextField="ServerName">
                                        </dx:ASPxListBox>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="ServerTypeFilterListBox" />
                                        <asp:AsyncPostBackTrigger ControlID="ResetButton" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                        </tr>
                        <tr>
                            <td>
                                <dx:ASPxLabel ID="ASPxLabel3" runat="server" Text="OR" CssClass="lblsmallFont" Font-Bold="true">
                                </dx:ASPxLabel>
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                        </tr>
                        <tr>
                            <td>
                                <dx:ASPxLabel ID="ASPxLabel4" runat="server" Text="Specify a part of the server name to filter by:" CssClass="lblsmallFont">
                                </dx:ASPxLabel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <dx:ASPxTextBox ID="ServerFilterTextBox" runat="server" Width="170px">
                                        </dx:ASPxTextBox>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="ResetButton" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label class="lblsmallFont" for="startDate">&nbsp;Date:</label>
                            </td>
                        </tr>
                        <tr>
                        <td>
                            <uc1:DateRange ID="dtPick" runat="server" Width="100px" Height="100%"></uc1:DateRange>
                        </td>
                    </tr>
                    </table>
                </td>
                <td valign="top">
                    <table> 
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="SubmitButton" />
                                        <asp:AsyncPostBackTrigger ControlID="ResetButton" />
                                    </Triggers>
                                    <ContentTemplate>
                                        <dx:ReportToolbar ID="ReportToolbar1" runat='server' ShowDefaultButtons='False' 
                                            ReportViewerID="ReportViewer1" Theme="Moderno">
                                            <Items>
                                                <dx:ReportToolbarButton ItemKind='Search' />
                                                <dx:ReportToolbarSeparator />
                                                <dx:ReportToolbarButton ItemKind='PrintReport' />
                                                <dx:ReportToolbarButton ItemKind='PrintPage' />
                                                <dx:ReportToolbarSeparator />
                                                <dx:ReportToolbarButton Enabled='False' ItemKind='FirstPage' />
                                                <dx:ReportToolbarButton Enabled='False' ItemKind='PreviousPage' />
                                                <dx:ReportToolbarLabel ItemKind='PageLabel' />
                                                <dx:ReportToolbarComboBox ItemKind='PageNumber' Width='65px'>
                                                </dx:ReportToolbarComboBox>
                                                <dx:ReportToolbarLabel ItemKind='OfLabel' />
                                                <dx:ReportToolbarTextBox IsReadOnly='True' ItemKind='PageCount' />
                                                <dx:ReportToolbarButton ItemKind='NextPage' />
                                                <dx:ReportToolbarButton ItemKind='LastPage' />
                                                <dx:ReportToolbarSeparator />
                                                <dx:ReportToolbarButton ItemKind='SaveToDisk' />
                                                <dx:ReportToolbarButton ItemKind='SaveToWindow' />
                                                <dx:ReportToolbarComboBox ItemKind='SaveFormat' Width='70px'>
                                                    <Elements>
                                                        <dx:ListElement Value='pdf' />
                                                        <dx:ListElement Value='xls' />
                                                        <dx:ListElement Value='xlsx' />
                                                        <dx:ListElement Value='rtf' />
                                                        <dx:ListElement Value='mht' />
                                                        <dx:ListElement Value='html' />
                                                        <dx:ListElement Value='txt' />
                                                        <dx:ListElement Value='csv' />
                                                        <dx:ListElement Value='png' />
                                                    </Elements>
                                                </dx:ReportToolbarComboBox>
                                            </Items>
                                            <Styles>
                                                <LabelStyle>
                                                    <Margins MarginLeft='3px' MarginRight='3px' />
                                                </LabelStyle>
                                            </Styles>
                                        </dx:ReportToolbar>
                                        <dx:ReportViewer ID="ReportViewer1" runat="server">
                                            <ClientSideEvents EndCallback="function(s, e) {
	                                            setDivs();
                                            }" />
                                        </dx:ReportViewer>        
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
    </table>
</asp:Content>