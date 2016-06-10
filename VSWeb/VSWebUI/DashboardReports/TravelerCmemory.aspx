<%@ Page Title="VitalSigns Plus -Traveler Allocated C memory" Language="C#" MasterPageFile="~/Reports.Master" AutoEventWireup="true" CodeBehind="TravelerCmemory.aspx.cs" Inherits="VSWebUI.TravelerCmemory" %>

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
        .style1
        {
            height: 113px;
        }
    </style>
</asp:Content>
<%--2/1/2016 Durga added for VSPLUS 2174--%>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <table>
            <tr>
                <td class="tdpadded" valign="top">  
        <table>
            <tr>
                <td>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table>
                        <tr>
                            <td>
            <dx:ASPxButton ID="SubmitButton" runat="server" Text="Submit" 
                onclick="SubmitButton_Click" CssClass="sysButton">
            </dx:ASPxButton> 

                            </td>
                            <td>
        <dx:ASPxButton ID="ResetButton" runat="server" Text="Reset" 
            onclick="ResetButton_Click" CssClass="sysButton">
            </dx:ASPxButton> 
                            </td>
                        </tr>
                    </table>        
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <dx:ASPxButton ID="ReptBtn" runat="server" Text="Reports" 
            onclick="ReptBtn_Click" Theme="Office2010Blue" Visible="False">
        </dx:ASPxButton>
                </td>
            </tr>
                        <tr>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td>
        <dx:ASPxLabel ID="ASPxLabel4" runat="server" 
            Text="Server(s):" CssClass="lblsmallFont">
        </dx:ASPxLabel>
    
                            </td>
                        </tr>
                        <tr>
                            <td valign="top" class="style1">
                                <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <dx:ASPxListBox ID="ServerListBox" runat="server" ValueType="System.String" 
                                            SelectionMode="CheckColumn" Theme="Default">
                                        </dx:ASPxListBox>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="ResetButton" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top">
                            </td>
                        </tr>
                        <tr>
                            <td valign="top">
                                <dx:ASPxLabel ID="ASPxLabel6" runat="server" CssClass="lblsmallFont" 
                                    Text="Date Range:">
                                </dx:ASPxLabel>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top">
                                <uc1:DateRange ID="dtPick" runat="server" Width="100px" Height="100%"></uc1:DateRange>    
                            </td>
                        </tr>
                        <tr>
                            <td valign="top">
                                <dx:ASPxLabel ID="ASPxLabel5" runat="server" CssClass="lblsmallFont" 
                                    Text="Threshold:">
                                </dx:ASPxLabel>
                                <br />
                                <dx:ASPxLabel ID="ASPxLabel7" runat="server" CssClass="lblsmallFont" 
                                    Text="(will filter the report by Allocated C memory above the entered value)">
                                </dx:ASPxLabel>
    
                            </td>
                        </tr>
                        <tr>
                            <td valign="top">
                                <dx:ASPxTextBox ID="TCutoffTextBox" runat="server" Width="170px">
                                </dx:ASPxTextBox>
    
                            </td>
                        </tr>
                    </table>
                </td>
                <td valign="top" rowspan="2">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table>
                        <tr>
                            <td>
    <dx:ReportToolbar ID="ReportToolbar1" runat="server" ShowDefaultButtons="False" 
        ReportViewerID="ReportViewer1" Theme="Moderno">
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
                <Elements>
                    <dx:ListElement Value="pdf" />
                    <dx:ListElement Value="xls" />
                    <dx:ListElement Value="xlsx" />
                    <dx:ListElement Value="rtf" />
                    <dx:ListElement Value="mht" />
                    <dx:ListElement Value="html" />
                    <dx:ListElement Value="txt" />
                    <dx:ListElement Value="csv" />
                    <dx:ListElement Value="png" />
                </Elements>
            </dx:ReportToolbarComboBox>
        </Items>
        <Styles>
            <LabelStyle>
            <Margins MarginLeft="3px" MarginRight="3px" />
            </LabelStyle>
        </Styles>
    </dx:ReportToolbar>

                            </td>
                        </tr>
                        <tr>
                            <td>
    <dx:ReportViewer ID="ReportViewer1" runat="server">
        <ClientSideEvents EndCallback="function(s, e) {
	setDivs();
}" />
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
            <tr>
                <td class="tdpadded" valign="top">
                    
                    </td>
            </tr>
            </table>
</asp:Content>
