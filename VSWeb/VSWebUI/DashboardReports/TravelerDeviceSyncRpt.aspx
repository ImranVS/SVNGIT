<%@ Page Title ="VitalSigns Plus - Traveler Device Sync Volume Report" Language="C#" MasterPageFile="~/Reports.Master" AutoEventWireup="true" CodeBehind="TravelerDeviceSyncRpt.aspx.cs" Inherits="VSWebUI.DashboardReports.TravelerDeviceSyncRpt" %>

<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>

<%@ Register Assembly="DevExpress.XtraReports.v14.2.Web, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.XtraReports.Web" TagPrefix="dx" %>
<%@ Register Src="~/Controls/DateRange.ascx" TagName="DateRange" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   <style type="text/css">
    .tdpadded
    {
        padding-left:20px;
    }
   </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
    <table>
        <tr>
            <td class="tdpadded" valign="top">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <table>
                    <tr>
                        <td>
                            <dx:ASPxButton ID="SubmitButton" runat="server" onclick="SubmitButton_Click" 
                                Text="Submit" CssClass="sysButton">
                            </dx:ASPxButton>
                        </td>
                        <td>
                            <dx:ASPxButton ID="ResetButton" runat="server" onclick="ResetButton_Click" 
                                Text="Reset" CssClass="sysButton">
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
                            </td>
                    </tr>
                    <tr>
                        <td>
                            <dx:ASPxLabel ID="ASPxLabel3" runat="server" 
                                Text="Select server(s) from the list below:" CssClass="lblsmallFont">
                            </dx:ASPxLabel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <dx:ASPxListBox ID="ServerListFilterListBox" runat="server" 
                                        ValueType="System.String" SelectionMode="CheckColumn">
                                    </dx:ASPxListBox>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="ResetButton" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td>
                            <dx:ASPxLabel ID="ASPxLabel4" runat="server" CssClass="lblsmallFont" 
                                Text="Date Range:">
                            </dx:ASPxLabel>
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
                <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <table>
                    <tr>
                        <td>
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
                    <ClientSideEvents EndCallback="function(s, e) { setDivs();}" />
                </dx:ReportViewer>
                        </td>
                    </tr>
                </table>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="SubmitButton" />
                        <asp:AsyncPostBackTrigger ControlID="ResetButton" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
    </div>
</asp:Content>
