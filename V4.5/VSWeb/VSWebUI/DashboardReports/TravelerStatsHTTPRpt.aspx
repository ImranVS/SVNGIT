<%@ Page Title ="VitalSigns Plus - Traveler HTTP Sessions Report" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="TravelerStatsHTTPRpt.aspx.cs" Inherits="VSWebUI.DashboardReports.TravelerStatsHTTPRpt" %>

<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>

<%@ Register Assembly="DevExpress.XtraReports.v14.2.Web, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.XtraReports.Web" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   <style type="text/css">
    .tdpadded
    {
        padding-left:20px;
    }
       .style1
       {
           padding-left: 20px;
           height: 31px;
       }
   </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
    <table>
        <tr>
            <td class="style1">
                <table>
                    <tr>
                        <td>
                            <dx:ASPxButton ID="ReptBtn" runat="server" Text="Reports" 
                                onclick="ReptBtn_Click" Theme="Office2010Blue">
                            </dx:ASPxButton>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            <dx:ASPxButton ID="SubmitButton" runat="server" Text="Submit" 
                                onclick="SubmitButton_Click" Theme="Office2010Blue">
                            </dx:ASPxButton>
                        </td>
                        <td>
                            <dx:ASPxButton ID="ResetButton" runat="server" Text="Reset" 
                                onclick="ResetButton_Click" Theme="Office2010Blue">
                            </dx:ASPxButton>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td class="tdpadded">
                <table>
                    <tr>
                        <td>
                            <dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="Select a traveler server:" 
                                CssClass="lblsmallFont">
                            </dx:ASPxLabel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <dx:ASPxComboBox ID="TravelerFilterComboBox" runat="server" ValueType="System.String">
                            </dx:ASPxComboBox>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td class="tdpadded">
                <dx:ReportToolbar ID="ReportToolbar1" runat='server' ShowDefaultButtons='False' 
                    ReportViewerID="ReportViewer1">
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
            </td>
        </tr>
        <tr>
            <td class="tdpadded">
                <dx:ReportViewer ID="ReportViewer1" runat="server">
                    <ClientSideEvents EndCallback="function(s, e) { setDivs();}" />
                </dx:ReportViewer>
            </td>
        </tr>
    </table>
    </div>
</asp:Content>
