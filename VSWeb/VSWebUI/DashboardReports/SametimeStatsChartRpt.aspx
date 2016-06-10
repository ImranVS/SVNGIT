<%@ Page Title="VitalSigns Plus - Sametime Chart Report" Language="C#" MasterPageFile="~/Reports.Master" AutoEventWireup="true" CodeBehind="SametimeStatsChartRpt.aspx.cs" Inherits="VSWebUI.DashboardReports.SametimeStatsChartRpt" %>

<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register assembly="DevExpress.XtraReports.v14.2.Web, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.XtraReports.Web" tagprefix="dx" %>
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
    <table>
        <tr>
            <td class="tdpadded" valign="top">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <table>
                    <tr>
                        <td>
                            <dx:ASPxButton ID="SubmitBtn" runat="server" Text="Submit" 
                                CssClass="sysButton">
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
                                Theme="Office2010Blue" onclick="ReptBtn_Click" Visible="False">
                            </dx:ASPxButton>
                <table>
                    <tr>
                        <td>
                            <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="Date range:" CssClass="lblsmallFont">
                            </dx:ASPxLabel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <uc1:DateRange ID="dtPick" runat="server" Width="100px" Height="100%"></uc1:DateRange>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="Statistic:" CssClass="lblsmallFont">
                            </dx:ASPxLabel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <dx:ASPxComboBox ID="StatComboBox" runat="server" ValueType="System.String">
                            </dx:ASPxComboBox>
                        </td>
                    </tr>
                </table>
            </td>
            <td valign="top">
                <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <table>
                    <tr>
                        <td>
                            <dx:ReportToolbar ID="ReportToolbar1" runat="server" 
            ReportViewerID="ReportViewer1" ShowDefaultButtons="False" Theme="Moderno">
            <Items>
                <dx:ReportToolbarButton ItemKind="Search" />
                <dx:ReportToolbarSeparator />
                <dx:ReportToolbarButton ItemKind="PrintReport" />
                <dx:ReportToolbarButton ItemKind="PrintPage" />
                <dx:ReportToolbarSeparator />
                <dx:ReportToolbarButton Enabled="False" ItemKind="FirstPage" />
                <dx:ReportToolbarButton Enabled="False" ItemKind="PreviousPage" />
                <dx:ReportToolbarLabel ItemKind="PageLabel" />
                <dx:ReportToolbarComboBox ItemKind="PageNumber" Width="65px">
                </dx:ReportToolbarComboBox>
                <dx:ReportToolbarLabel ItemKind="OfLabel" />
                <dx:ReportToolbarTextBox IsReadOnly="True" ItemKind="PageCount" />
                <dx:ReportToolbarButton ItemKind="NextPage" />
                <dx:ReportToolbarButton ItemKind="LastPage" />
                <dx:ReportToolbarSeparator />
                <dx:ReportToolbarButton ItemKind="SaveToDisk" />
                <dx:ReportToolbarButton ItemKind="SaveToWindow" />
                <dx:ReportToolbarComboBox ItemKind="SaveFormat" Width="70px">
                    <elements>
                        <dx:ListElement Value="pdf" />
                        <dx:ListElement Value="xls" />
                        <dx:ListElement Value="xlsx" />
                        <dx:ListElement Value="rtf" />
                        <dx:ListElement Value="mht" />
                        <dx:ListElement Value="html" />
                        <dx:ListElement Value="txt" />
                        <dx:ListElement Value="csv" />
                        <dx:ListElement Value="png" />
                    </elements>
                </dx:ReportToolbarComboBox>
            </Items>
            <styles>
                <LabelStyle><Margins MarginLeft='3px' MarginRight='3px' /></LabelStyle>
            </styles>
                </dx:ReportToolbar>
                <dx:ReportViewer ID="ReportViewer1" runat="server" Height="56px">
                <ClientSideEvents EndCallback="function(s, e) {
	setDivs();
}" />
        </dx:ReportViewer>
                        </td>
                    </tr>
                </table>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="SubmitBtn" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
</asp:Content>
