<%@ Page Title="VitalSigns Plus - Server Availability Index Report" Language="C#" AutoEventWireup="true" MasterPageFile="~/Reports.Master" CodeBehind="SrvAvailabilityIndexRpt.aspx.cs" Inherits="VSWebUI.DashboardReports.SrvAvailabilityIndexRpt" %>

<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>

<%@ Register Assembly="DevExpress.XtraReports.v14.2.Web, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.XtraReports.Web" TagPrefix="dx" %>
<%@ Register Src="~/Controls/DateRange.ascx" TagName="DateRange" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../css/bootstrap1.min.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table>
        <tr>
            <td class="tdpadded" valign="top">
                <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <table>
                            <tr>
                                <td>
                                    <dx:ASPxButton ID="SubmitButton" runat="server" CssClass="sysButton" 
                                        Text="Submit" onclick="SubmitButton_Click">
                                    </dx:ASPxButton>
                                </td>
                                <td>
                                    <dx:ASPxButton ID="ServerListResetButton" runat="server" CssClass="sysButton" 
                                        Text="Reset" onclick="ServerListResetButton_Click">
                                    </dx:ASPxButton>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <table>
                    <tr>
                        <td>
                            <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="Date Range:" CssClass="lblsmallFont">
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
                            <dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="Server(s):" CssClass="lblsmallFont">
                            </dx:ASPxLabel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <dx:ASPxListBox ID="ServerListFilterListBox" runat="server" SelectionMode="CheckColumn" ValueType="System.String">
                                    </dx:ASPxListBox>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="ServerListResetButton" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
            </td>
            <td valign="top">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <table>
                            <tr>
                                <td>
                                    <dx:ASPxDocumentViewer ID="ASPxDocumentViewer1" runat="server" Theme="Moderno" Width="910px">
                                        <SettingsSplitter RightPaneVisible="False" SidePaneVisible="False" />
                                    </dx:ASPxDocumentViewer>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="SubmitButton" />
                        <asp:AsyncPostBackTrigger ControlID="ServerListResetButton" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
</asp:Content>
