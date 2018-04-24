<%@ Page Title="VitalSigns Plus - TravelerStatsRpt" Language="C#" MasterPageFile="~/Reports.Master" AutoEventWireup="true" CodeBehind="TravelerStatsRpt.aspx.cs" Inherits="VSWebUI.DashboardReports.TravelerStatsRpt" %>

<%@ Register assembly="DevExpress.XtraReports.v14.2.Web, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.XtraReports.Web" tagprefix="dx" %>

<%@ Register assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>





<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<link href="../css/bootstrap1.min.css" rel="stylesheet" />
   <style type="text/css">
    .tdpadded
    {
        padding-left:20px;
    }
       .style1
       {
           width: 5px;
       }
   </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
    <table>
        <tr>
            <td class="tdpadded" valign="top">
        <table>
                    <tr>
                        <td>
                    <dx:ASPxButton ID="SubmitButton" runat="server" onclick="SubmitButton_Click" 
                        Text="Submit" CssClass="sysButton">
                    </dx:ASPxButton>
                        </td>
                        <td>
                    <dx:ASPxButton ID="ResetButton" runat="server" Text="Reset" onclick="ResetButton_Click" CssClass="sysButton">
                    </dx:ASPxButton>
                        </td>
                        <td>
                <dx:ASPxLabel ID="ASPxLabel3" runat="server" 
                    Text="Click on the star icon to mark this report as favorite" Visible="False">
                </dx:ASPxLabel>
                        </td>
                        <td>
                <dx:ASPxRatingControl ID="ASPxRatingControl1" runat="server" ItemCount="1" 
                    Value="0" FillPrecision="Full" Theme="RedWine" Visible="False">
                    <ClientSideEvents ItemClick="function(s, e) {
	if (e.index == rating-1)
       s.SetValue(0);
    rating = s.GetValue();
}"
                    Init="function(s, e) {
	rating = s.GetValue();
}" />
                </dx:ASPxRatingControl>
                        </td>
                    </tr>
                </table>
                <dx:ASPxButton ID="ReptBtn" runat="server" Text="Reports" 
            onclick="ReptBtn_Click" Theme="Office2010Blue" Visible="False">
        </dx:ASPxButton>
                <div class="input-prepend">&nbsp;</div>
                <table>
                    <tr>
                        <td>
                    <dx:ASPxLabel ID="ASPxLabel1" runat="server" 
                        Text="Select an interval:" CssClass="lblsmallFont">
                    </dx:ASPxLabel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                    <dx:ASPxComboBox ID="IntervalFilterComboBox" runat="server" 
                        EnableTheming="True" Theme="Default">
                    </dx:ASPxComboBox>
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td>
                    <dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="Select a traveler server:" 
                                CssClass="lblsmallFont">
                    </dx:ASPxLabel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                    <dx:ASPxComboBox ID="ServerFilterComboBox" runat="server" EnableTheming="True" 
                        Theme="Default">
                    </dx:ASPxComboBox>
                        </td>
                    </tr>
                </table>
                </td>
                <td valign="top">
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
        <dx:ReportViewer ID="ReportViewer1" runat="server">
                <ClientSideEvents EndCallback="function(s, e) {
	setDivs();
}" />
        </dx:ReportViewer>
                            </td>
                        </tr>
                    </table>
                </td>
        </tr>
    </table>
   </div>
</asp:Content>