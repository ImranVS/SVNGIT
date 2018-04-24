<%@ Page Title="vitalSigns Plus - SrvAvailabilityRpt" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="SrvAvailabilityRpt.aspx.cs" Inherits="VSWebUI.DashboardReports.SrvAvailabilityRpt" %>

<%@ Register assembly="DevExpress.XtraReports.v14.2.Web, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.XtraReports.Web" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>


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
        <br />
        <table>
        <tr>
            <td class="tdpadded">
                <dx:ASPxButton ID="ReptBtn" runat="server" Text="Reports" 
            onclick="ReptBtn_Click" Theme="Office2010Blue">
        </dx:ASPxButton>
            </td>
        </tr>
        <tr>
            <td>&nbsp;
            </td>
        </tr>
        <tr>
            <td class="tdpadded">
                <dx:ReportToolbar ID="ReportToolbar1" runat="server" 
            ReportViewerID="ReportViewer1" ShowDefaultButtons="False">
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
            <td class="tdpadded">
                <table class="style1">
            <tr>
                <td colspan="2">
                    <dx:ASPxLabel ID="ASPxLabel1" runat="server" 
                        Text="Select a value from the list to filter by server:">
                    </dx:ASPxLabel>
                </td>
            </tr>
            <tr>
                <td class="style2">
                    <dx:ASPxComboBox ID="ServerListFilterComboBox" runat="server" 
                          Theme="Default">
                    </dx:ASPxComboBox>
                </td>
                <td>
                    <dx:ASPxButton ID="ServerListResetButton" runat="server" 
                        onclick="ServerListResetButton_Click" Text="Reset" Theme="Office2010Blue" 
                        Width="80px">
                    </dx:ASPxButton>
                </td>
            </tr>
            <tr>
                <td class="style2">
                    <dx:ASPxDateEdit ID="DateParamEdit" runat="server" 
                        DisplayFormatString="MMM yyyy" EditFormat="Custom" EditFormatString="MMM yyyy" 
                        Theme="Default">
                    </dx:ASPxDateEdit>
                </td>
                <td>
                    <dx:ASPxButton ID="SubmitButton" runat="server" onclick="SubmitButton_Click" 
                        Text="Submit" Theme="Office2010Blue" Width="80px">
                    </dx:ASPxButton>
                </td>
            </tr>
        </table>
            </td>
        </tr>
        <tr>
            <td class="tdpadded">
                <dx:ReportViewer ID="ReportViewer1" runat="server">
                <ClientSideEvents EndCallback="function(s, e) {
	setDivs();
}" />
    </dx:ReportViewer>
            </td>
        </tr>
    </table>
     </div>
    
    </asp:Content>
