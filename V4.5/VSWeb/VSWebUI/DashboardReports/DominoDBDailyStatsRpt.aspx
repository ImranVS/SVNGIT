<%@ Page Title="VitalSigns Plus - DominoDBDailyStatsRpt" Language="C#" MasterPageFile="~/Reports.Master" AutoEventWireup="true" CodeBehind="DominoDBDailyStatsRpt.aspx.cs" Inherits="VSWebUI.DashboardReports.DominoDailyStatsRpt" %>

<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>

<%@ Register assembly="DevExpress.XtraReports.v14.2.Web, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.XtraReports.Web" tagprefix="dx" %>

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
        <table>
            <tr>
                <td class="tdpadded" valign="top">
                    <table>
                        <tr>
                            <td>
                                <dx:ASPxButton ID="SubmitButton" runat="server" onclick="SubmitButton_Click" 
            Text="Submit" Visible="False" CssClass="sysButton">
        </dx:ASPxButton>
                            </td>
                            <td>
        <dx:ASPxButton ID="ResetButton" runat="server" Text="Reset" 
            onclick="ResetButton_Click" CssClass="sysButton">
            </dx:ASPxButton> 
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
        <dx:ASPxLabel ID="ASPxLabel4" runat="server"
                                    Text="Select a server from the list below:" 
                                    CssClass="lblsmallFont">
        </dx:ASPxLabel>
                            </td>
                        </tr>
                        <tr>
                            <td>
        <dx:ASPxComboBox ID="ServerComboBox" runat="server" Theme="Default" 
            AutoPostBack="True" 
            onselectedindexchanged="ServerComboBox_SelectedIndexChanged">
        </dx:ASPxComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="Filter the list by folder name by selecting the folder and clicking 'Submit'." CssClass="lblsmallFont">
                            </dx:ASPxLabel>
                            </td>
                        </tr>
                        <tr>
                            <dx:ASPxComboBox ID="FolderComboBox" runat="server" ValueType="System.String" 
            Visible="False">
        </dx:ASPxComboBox>
                        </tr>
                    </table>
                </td>
                <td valign="top">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <dx:ASPxDocumentViewer ID="ASPxDocumentViewer1" runat="server" Width="910px" Theme="Moderno" SettingsSplitter-SidePaneVisible="False">
                            </dx:ASPxDocumentViewer>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="ServerComboBox" />
                            <asp:AsyncPostBackTrigger ControlID="SubmitButton" />
                            <asp:AsyncPostBackTrigger ControlID="ResetButton" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
      </div>
    </asp:Content>
