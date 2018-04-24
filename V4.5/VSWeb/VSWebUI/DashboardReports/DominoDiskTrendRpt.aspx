<%@ Page Language="C#" MasterPageFile="~/Reports.Master" AutoEventWireup="true" CodeBehind="DominoDiskTrendRpt.aspx.cs" Inherits="VSWebUI.DashboardReports.DominoDiskTrendRpt" %>

<%@ Register assembly="DevExpress.XtraReports.v14.2.Web, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.XtraReports.Web" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>


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
    <div>
        <table width="100%">
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
                    <dx:ASPxButton ID="ResetButton" runat="server" OnClick="ResetButton_Click"
                        Text="Reset" CssClass="sysButton">
                    </dx:ASPxButton>
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                        </tr>
                    </table>
                    <dx:ASPxButton ID="ReptBtn" runat="server" Text="Reports" 
            onclick="ReptBtn_Click" Theme="Office2010Blue" Visible="False">
            </dx:ASPxButton>
                    <table>
                        <tr>
                            <td valign="top">
                                <dx:ASPxLabel ID="ASPxLabel6" runat="server" CssClass="lblsmallFont" 
                                    Text="Server Type:">
                                </dx:ASPxLabel>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top">
                                <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <dx:ASPxListBox ID="ServerTypeFilterListBox" runat="server" AutoPostBack="True" 
                                    ValueType="System.String">
                                </dx:ASPxListBox>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="ResetButton" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                        </tr>
                        <tr>
                            <td valign="top">
                                <dx:ASPxLabel ID="ASPxLabel1" runat="server" CssClass="lblsmallFont" 
                                    Text="Server(s):">
                                </dx:ASPxLabel>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top">
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <dx:ASPxListBox ID="ServerListFilterListBox" runat="server" 
                                    SelectionMode="CheckColumn" TextField="ServerName" ValueField="ServerName" 
                                    ValueType="System.String">
                                </dx:ASPxListBox>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="ServerTypeFilterListBox" />
                                        <asp:AsyncPostBackTrigger ControlID="ResetButton" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                        </tr>
                        <tr>
                            <td valign="top">
                    <dx:ASPxLabel ID="ASPxLabel4" runat="server" CssClass="lblsmallFont" Text="OR" 
                        Font-Bold="True">
                    </dx:ASPxLabel>
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                        </tr>
                        <tr>
                            <td valign="top">
                    <dx:ASPxLabel ID="ASPxLabel5" runat="server" CssClass="lblsmallFont" 
                        Text="Specify a part of the server name to filter by:">
                    </dx:ASPxLabel>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top">
                                <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                    <dx:ASPxTextBox ID="ServerFilterTextBox" runat="server" Width="170px">
                    </dx:ASPxTextBox>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="ResetButton" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                        </tr>
                        <tr>
                            <td valign="top">
                    <dx:ASPxLabel ID="ASPxLabel2" runat="server" CssClass="lblsmallFont" 
                        
                        Text="Enter a number of days below to filter the report by remaining space. " 
                                    Width="200px" Wrap="True">
                    </dx:ASPxLabel><br /><br />
                    <dx:ASPxLabel ID="ASPxLabel3" runat="server" CssClass="lblsmallFont" 
                        
                                    Text="I.e., if you enter 100, the report will display all servers that have less than 100 days of space remaining." 
                                    Width="200px" Wrap="True">
                    </dx:ASPxLabel>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top">
                                <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <dx:ASPxTextBox ID="NumberDaysTextBox" runat="server" Width="170px">
                    </dx:ASPxTextBox>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="ResetButton" />
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
        <ContentTemplate>
            <dx:ASPxDocumentViewer ID="ASPxDocumentViewer1" runat="server" Width="850px" Theme="Moderno" SettingsSplitter-SidePaneVisible="False">
            </dx:ASPxDocumentViewer>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="SubmitButton" />
            <asp:AsyncPostBackTrigger ControlID="ResetButton" />
        </Triggers>
        </asp:UpdatePanel>
                </td>
                <td class="tdpadded">
                    &nbsp;</td>
            </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td class="tdpadded" valign="top">
                    
                </td>
                <td class="tdpadded">
                    &nbsp;</td>
            </tr>
            
            <tr>
                <td class="tdpadded">
                    &nbsp;</td>
                <td class="tdpadded">
                    &nbsp;</td>
            </tr>
        </table>
          
    </div>
    </asp:Content> 