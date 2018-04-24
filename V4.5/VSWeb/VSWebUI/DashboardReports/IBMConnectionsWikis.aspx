<%@ Page Language="C#" Title="VitalSigns Plus - IBM Connections Wikis" MasterPageFile="~/Reports.Master" AutoEventWireup="true" CodeBehind="IBMConnectionsWikis.aspx.cs" Inherits="VSWebUI.DashboardReports.IBMConnectionsWikis" %>

<%@ Register Assembly="DevExpress.XtraReports.v14.2.Web, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.XtraReports.Web" TagPrefix="dx" %>

<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>

<%@ Register Src="~/Controls/DateRange.ascx" TagName="DateRange" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../css/bootstrap1.min.css" rel="stylesheet" />
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
                                <dx:ASPxButton ID="SubmitButton" runat="server" Text="Submit" CssClass="sysButton"  onclick="SubmitButton_Click">
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
                                <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="Servers:" CssClass="lblsmallFont">
                                </dx:ASPxLabel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                       <%-- <dx:ASPxListBox ID="ServerListFilterListBox" runat="server" SelectionMode="CheckColumn" 
                                    ValueType="System.String">
                                </dx:ASPxListBox>--%>
                                 <dx:ASPxComboBox ID="ServerComboBox" runat="server"  
                                     EnableTheming="True" 
                                    
                                     Theme="Default" 
                                    AutoPostBack="True">
                                   </dx:ASPxComboBox>
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
                                <dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="Date range:" CssClass="lblsmallFont">
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
                                <dx:ASPxLabel ID="ASPxLabel3" runat="server" Text="Wikis:" CssClass="lblsmallFont">
                                </dx:ASPxLabel>
                            </td>
                            </tr>

                     <tr>
                     <td>
                              <dx:ASPxComboBox ID="WikiTypeCombobox" runat="server" onselectedindexchanged="WikiTypeCombobox_SelectedIndexChanged"   AutoPostBack="true">
                <%--<Items>
                    <dx:ListEditItem Text="Select Type" Value="Select Type" />
                </Items>--%>
            </dx:ASPxComboBox>  
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
    </table>
</asp:Content>
