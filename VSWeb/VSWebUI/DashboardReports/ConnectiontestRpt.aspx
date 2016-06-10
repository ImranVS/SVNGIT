<%@ Page Title="VitalSigns plus - DominoDiskHealthRpt" Language="C#" MasterPageFile="~/Reports.Master" AutoEventWireup="true" CodeBehind="ConnectiontestRpt.aspx.cs" Inherits="VSWebUI.DashboardReports.ConnectiontestRpt" %>

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
           width: 10px;
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
                    <dx:ASPxButton ID="ServerListResetButton" runat="server" CssClass="sysButton" 
                        onclick="ServerListResetButton_Click" Text="Reset">
                    </dx:ASPxButton>
                            </td>
                        </tr>
                    </table>
                    <dx:ASPxButton ID="ReptBtn" runat="server" Text="Reports" 
            onclick="ReptBtn_Click" Theme="Office2010Blue" Visible="False">
        </dx:ASPxButton>
                    <table>
                        <tr>
                            <td>
                    <dx:ASPxLabel ID="ASPxLabel1" runat="server" 
                        Text="Select a value from the list to filter by Community Name:" CssClass="lblsmallFont">
                    </dx:ASPxLabel>
                            </td>
                        </tr>
                        <tr>
                            <td>
        <dx:ASPxComboBox ID="ServerListFilterComboBox" runat="server" 
             EnableTheming="True" 
            onselectedindexchanged="ServerListFilterComboBox_SelectedIndexChanged" 
            Theme="Default" 
            AutoPostBack="True">
        </dx:ASPxComboBox>
                            </td>
                        </tr>
                    </table>
                </td>
                <td valign="top">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <dx:ASPxDocumentViewer ID="ASPxDocumentViewer1" runat="server" Width="720px" Theme="Moderno"  SettingsSplitter-SidePaneVisible="False">
                    </dx:ASPxDocumentViewer>
        </ContentTemplate>
        <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="ServerListFilterComboBox" />
                    <asp:AsyncPostBackTrigger ControlID="ServerListResetButton" />
                </Triggers>
                
        </asp:UpdatePanel>
                </td>
            </tr>
        </table>    
    </div>
    </asp:Content>
