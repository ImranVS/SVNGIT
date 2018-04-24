<%@ Page Title="VitalSigns Plus - Server Statistics" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="OverallStatisticsRpt.aspx.cs" Inherits="VSWebUI.DashboardReports.OverallStatisticsRpt" %>

<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>

<%@ Register assembly="DevExpress.XtraReports.v14.2.Web, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.XtraReports.Web" tagprefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
    .tdpadded
    {
        padding-left:20px;
    }
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<br />
    <table>
        <tr>
            <td class="tdpadded" colspan="2">
                <dx:ASPxButton ID="ReptBtn" runat="server" Text="Reports" 
            onclick="ReptBtn_Click" Theme="Office2010Blue">
        </dx:ASPxButton>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                </td>
        </tr>
        <tr>
            <td class="tdpadded" colspan="2">

                <dx:ReportToolbar ID="ReportToolbar1" runat="server" ShowDefaultButtons="False" 
                    ReportViewerID="ReportViewer1">
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
            </td>
        </tr>
        <tr>
            <td class="tdpadded" colspan="2">
                <dx:ASPxLabel ID="ASPxLabel2" runat="server" 
                    Text="Select server(s) from the list below:">
                </dx:ASPxLabel>
            </td>
        </tr>
        <tr>
        <td class="tdpadded">
        <table>
            <tr>
                <td rowspan="2">
                <dx:ASPxListBox ID="ServerListFilterListBox" runat="server" 
                        SelectionMode="CheckColumn" ValueType="System.String" 
                        TextField="ServerName" ValueField="ServerName">
                    </dx:ASPxListBox>
                </td>
                <td valign="top">
        <dx:ASPxButton ID="ResetButton" runat="server" OnClick="ResetButton_Click"
                        Text="Reset" Theme="Office2010Blue" Width="80px">
                    </dx:ASPxButton>
        </td>
            </tr>
            <tr>
            <td valign="top">
            <dx:ASPxButton ID="SubmitButton" runat="server" onclick="SubmitButton_Click" 
                        Text="Submit" Theme="Office2010Blue" Width="80px">
                    </dx:ASPxButton>
            </td>
        </tr>
        <tr>
            <td>
                        <dx:ASPxLabel ID="ASPxLabel4" runat="server" CssClass="lblsmallFont" Text="OR" 
                        Font-Bold="True">
                    </dx:ASPxLabel>
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td colspan="2">
                        <dx:ASPxLabel ID="ASPxLabel5" runat="server" CssClass="lblsmallFont" 
                        Text="Specify a part of the server name to filter by:">
                    </dx:ASPxLabel>
            </td>
        </tr>
        <tr>
            <td>
                        <dx:ASPxTextBox ID="ServerFilterTextBox" runat="server" Width="170px">
                    </dx:ASPxTextBox>
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                        <dx:ASPxDateEdit ID="DateParamEdit" runat="server" 
                            DisplayFormatString="MMM yyyy" EditFormat="Custom" EditFormatString="MMM yyyy">
                        </dx:ASPxDateEdit>
            </td>
            <td>
                &nbsp;</td>
        </tr>

        </table>    
        </td>
        </tr>
                <tr>
            <td class="tdpadded" colspan="2">
                        <dx:ReportViewer ID="ReportViewer1" runat="server">
                        <ClientSideEvents EndCallback="function(s, e) {
	setDivs();
}" />
                        </dx:ReportViewer>
                        </td>
        </tr>
    </table>
</asp:Content>

