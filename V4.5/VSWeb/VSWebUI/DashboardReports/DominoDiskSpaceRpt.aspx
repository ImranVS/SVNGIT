<%@ Page Language="C#" Title="VitalSigns Plus - Server Disk Total Space" MasterPageFile="~/Reports.Master" AutoEventWireup="true" CodeBehind="DominoDiskSpaceRpt.aspx.cs" Inherits="VSWebUI.DashboardReports.DominoDiskSpaceRpt" %>

<%@ Register Assembly="DevExpress.XtraReports.v14.2.Web, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.XtraReports.Web" TagPrefix="dx" %>

<%@ Register assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<link href="../css/bootstrap1.min.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style type="text/css">
        .tdpadded
        {
            padding-left:20px;
        }
        .ui-datepicker-calendar {
        display: none;
        }
    </style>
    <table width="100%">
        <tr>
            <td class="tdpadded" valign="top">
                <table>
        <tr>
            <td>
                <dx:ASPxButton ID="SubmitButton" runat="server" Text="Submit" CssClass="sysButton">
                </dx:ASPxButton>
            </td>
            <td>
                <dx:ASPxButton ID="ServerListResetButton" runat="server" CssClass="sysButton" 
                    Text="Reset" onclick="ServerListResetButton_Click">
                </dx:ASPxButton>
            </td>
        </tr>
        <tr>
            <td></td>
            <td>&nbsp;</td>
        </tr>
        </table>
        <dx:ASPxButton ID="ReptBtn" runat="server" Text="Reports" 
                    Theme="Office2010Blue" onclick="ReptBtn_Click" Visible="False">
                </dx:ASPxButton>
        <table>
        <tr>
            <td>
                <dx:ASPxLabel ID="ASPxLabel2" runat="server" CssClass="lblsmallFont" 
                    Text="Server Type:">
                </dx:ASPxLabel>
            </td>
        </tr>
        <tr>
            <td>
                <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <dx:ASPxListBox ID="ServerTypeFilterListBox" runat="server" 
                            ValueType="System.String" AutoPostBack="True">
                        </dx:ASPxListBox>
                        <dx:ASPxLabel ID="ASPxLabel1" runat="server" 
                            Text="Specify a part of the server name to filter by:" CssClass="lblsmallFont" 
                            Visible="False">
                        </dx:ASPxLabel>
                        <dx:ASPxTextBox ID="ServerFilterTextBox" runat="server" Width="170px" 
                            Visible="False">
                        </dx:ASPxTextBox>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ServerListResetButton" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
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
                <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <dx:ASPxListBox ID="ServerListFilterListBox" runat="server" 
                            SelectionMode="CheckColumn" ValueType="System.String">
                        </dx:ASPxListBox>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ServerTypeFilterListBox" />
                        <asp:AsyncPostBackTrigger ControlID="ServerListResetButton" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
            </td>
            <td valign="top">
                <table>
                    <tr>
            <td>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="SubmitButton" />
                        <asp:AsyncPostBackTrigger ControlID="ServerListResetButton" />
                    </Triggers>
                    <ContentTemplate>
                        <dx:ASPxDocumentViewer ID="ASPxDocumentViewer1" runat="server" Width="850px" Theme="Moderno" SettingsSplitter-SidePaneVisible="False">
                        </dx:ASPxDocumentViewer>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>