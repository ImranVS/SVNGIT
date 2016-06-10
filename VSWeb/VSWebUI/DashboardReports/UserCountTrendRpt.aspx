<%@ Page Title="VitalSigns Plus - User Count Trend" Language="C#" MasterPageFile="~/Reports.Master" AutoEventWireup="true" CodeBehind="UserCountTrendRpt.aspx.cs" Inherits="VSWebUI.DashboardReports.UserCountTrendRpt" %>

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
     .dxsplS { display: block;}
   </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table width="100%">
        <tr>
            <td valign="top">
                <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <table>
                    <tr>
                        <td>
                            <dx:ASPxButton ID="SubmitButton" runat="server" Text="Submit" 
                                CssClass="sysButton" onclick="SubmitButton_Click">
                            </dx:ASPxButton>
                        </td>
                        <td>
                    <dx:ASPxButton ID="ServerListResetButton" runat="server" 
                        onclick="ServerListResetButton_Click" Text="Reset" CssClass="sysButton">
                    </dx:ASPxButton>
                        </td>
                    </tr>
                </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <table>
                    <tr>
                        <td>
                            <dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="Date Range:" CssClass="lblsmallFont">
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
                    <dx:ASPxLabel ID="ASPxLabel3" runat="server" CssClass="lblsmallFont" 
                        Text="Server(s):">
                    </dx:ASPxLabel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <dx:ASPxListBox ID="ServerListFilterListBox" runat="server" 
                        SelectionMode="CheckColumn" ValueType="System.String">
                    </dx:ASPxListBox>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="ServerListResetButton" />
                                </Triggers>
                            </asp:UpdatePanel>
                            
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="Server type:" 
                                CssClass="lblsmallFont" Visible="False">
                            </dx:ASPxLabel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <dx:ASPxComboBox ID="TypeComboBox" runat="server" SelectedIndex="0" 
                                Visible="False">
                                <Items>
                                    <dx:ListEditItem Selected="True" Text="All" Value="0" />
                                    <dx:ListEditItem Text="Domino" Value="1" />
                                    <dx:ListEditItem Text="Exchange" Value="2" />
                                </Items>
                            </dx:ASPxComboBox>
                        </td>
                    </tr>
                </table>
            </td>
            <td valign="top">
                <table>
                    <tr>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
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
            </td>
        </tr>
    </table>
</asp:Content>