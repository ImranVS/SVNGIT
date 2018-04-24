<%@ Page Title="VitalSigns Plus - Password Settings" Language="C#" MasterPageFile="~/Reports.Master" AutoEventWireup="true" CodeBehind="O365PasswordSettingsRpt.aspx.cs" Inherits="VSWebUI.DashboardReports.O365PasswordSettingsRpt" %>

<%@ Register Assembly="DevExpress.XtraReports.v14.2.Web, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.XtraReports.Web" TagPrefix="dx" %>

<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>

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
            <table>
                <tr>
                    <td valign="top">
                        <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="Server:" CssClass="lblsmallFont">
                        </dx:ASPxLabel>
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        <dx:ASPxComboBox ID="ServerListComboBox" runat="server" ValueType="System.String">
                        </dx:ASPxComboBox>
                    </td>
                </tr>
            </table>
            <div class="input-prepend">&nbsp;</div>
        </td>
        <td valign="top"> 
            <table>
                <tr>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <dx:ASPxDocumentViewer ID="ASPxDocumentViewer1" runat="server" Theme="Moderno" Width="910px">
                                     <SettingsSplitter RightPaneVisible="False" SidePaneVisible="False" />
                                </dx:ASPxDocumentViewer>
                            </ContentTemplate>
                            <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="ServerListComboBox" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
</asp:Content>