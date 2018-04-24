<%@ Page Language="C#" Title="VitalSigns Plus - Domino Access via Browser Report" AutoEventWireup="true" CodeBehind="DominoAccessBrowserRpt.aspx.cs" MasterPageFile="~/Reports.Master" Inherits="VSWebUI.DashboardReports.DominoAccessBrowserRpt" %>
<%@ Register Assembly="DevExpress.XtraReports.v14.2.Web, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.XtraReports.Web" TagPrefix="dx" %>

<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>
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
    <table width="100%">
        <tr>
            <td class="tdpadded" valign="top">
                <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <table>
                    <tr>
                        <td>
                            <dx:ASPxButton ID="SubmitButton" runat="server" Text="Submit" 
                                onclick="SubmitButton_Click" CssClass="sysButton">
                            </dx:ASPxButton>
                        </td>
                        <td>
                            <dx:ASPxButton ID="ServerListResetButton" runat="server" Text="Reset" 
                                onclick="ServerListResetButton_Click" CssClass="sysButton">
                            </dx:ASPxButton>
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                    </tr>
                </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <table>
                    <tr>
                                    <td>
                                        <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="Server(s):" 
                                            CssClass="lblsmallFont">
                                        </dx:ASPxLabel>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                        <dx:ASPxListBox ID="ServerListFilterListBox" runat="server" 
                                            ValueType="System.String" SelectionMode="CheckColumn">
                                        </dx:ASPxListBox>        
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="ServerListResetButton" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                        <td>&nbsp;</td>
                    </tr>
                    
                                <tr>
                        <td>
                            <dx:ASPxLabel ID="ASPxLabel3" runat="server" 
                                Text="Statistic:" CssClass="lblsmallFont">
                            </dx:ASPxLabel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <dx:ASPxComboBox ID="StatComboBox" runat="server" ValueType="System.String" 
                                        Theme="Default">
                                    </dx:ASPxComboBox>
                        </td>
                    </tr>
                                <tr>
                        <td>&nbsp;</td>
                    </tr>
                                <tr>
                                    <td>
                                        <dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="Date Range:" 
                                            CssClass="lblsmallFont">
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
                        <dx:ASPxDocumentViewer ID="ASPxDocumentViewer1" runat="server"
                        Theme="Moderno" Width="910px">
                            <SettingsSplitter RightPaneVisible="False" SidePaneVisible="False" />
                        </dx:ASPxDocumentViewer>
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