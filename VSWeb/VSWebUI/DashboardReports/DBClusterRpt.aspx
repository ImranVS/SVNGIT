<%@ Page Title="VitalSigns Plus - DBClusterRpt" Language="C#" MasterPageFile="~/Reports.Master" AutoEventWireup="true" CodeBehind="DBClusterRpt.aspx.cs" Inherits="VSWebUI.DashboardReports.DBClusterRpt" %>

<%@ Register Assembly="DevExpress.XtraReports.v14.2.Web, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.XtraReports.Web" TagPrefix="dx" %>

<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>

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
                <table>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div class="input-prepend">&nbsp;</div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            
                <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="Cluster Name:" 
                                CssClass="lblsmallFont">
                            </dx:ASPxLabel>
                        </td>
                    </tr>
                    <tr>
            <td>
                <dx:ASPxComboBox ID="ClusterNameComboBox" runat="server" 
                                ValueType="System.String" 
                                onselectedindexchanged="ClusterNameComboBox_SelectedIndexChanged" 
                                AutoPostBack="True">
                            </dx:ASPxComboBox>
            </td>
        </tr>
                </table>
                <dx:ASPxButton ID="ReptBtn" runat="server" Text="Reports" 
                    onclick="ReptBtn_Click" Theme="Office2010Blue" Visible="False">
                </dx:ASPxButton>
            </td>
            <td valign="top">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <dx:ASPxDocumentViewer ID="ASPxDocumentViewer1" runat="server" Width="910px" Theme="Moderno" SettingsSplitter-SidePaneVisible="false">
                        </dx:ASPxDocumentViewer>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ClusterNameComboBox" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
        
    </table>
</asp:Content>