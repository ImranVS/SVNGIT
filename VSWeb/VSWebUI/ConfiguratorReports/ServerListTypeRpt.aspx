<%@ Page Title="VitalSigns Plus - Server List by Type" Language="C#" MasterPageFile="~/Reports.Master" AutoEventWireup="true" CodeBehind="ServerListTypeRpt.aspx.cs" Inherits="VSWebUI.ConfiguratorReports.ServerListTypeRpt" %>

<%@ Register assembly="DevExpress.XtraReports.v14.2.Web, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.XtraReports.Web" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>
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
                <table>
                    <tr>
                        <td>
                            <table>
                                <tr>
                                    <td>
                                        <dx:ASPxButton ID="ServerListResetButton" runat="server" 
                        onclick="ServerListResetButton_Click" Text="Reset" CssClass="sysButton">
                    </dx:ASPxButton>            
                                    </td>
                                </tr>
                            </table>
                            <div class="input-prepend">&nbsp;</div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <dx:ASPxButton ID="ReptBtn" runat="server" Text="Reports" 
            onclick="ReptBtn_Click" Theme="Office2010Blue" Visible="False">
        </dx:ASPxButton>    
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <dx:ASPxLabel ID="ASPxLabel2" runat="server" 
                        Text="Select a value from the list to filter by type:" 
                        CssClass="lblsmallFont">
                    </dx:ASPxLabel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <dx:ASPxComboBox ID="ServerListFilterComboBox" runat="server" 
                         Theme="Default" 
                         AutoPostBack="True" 
                        onselectedindexchanged="ServerListFilterComboBox_SelectedIndexChanged">
                    </dx:ASPxComboBox>
                        </td>
                    </tr>
                </table>
            </td>
            <td valign="top" rowspan="2">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <dx:ASPxDocumentViewer ID="ASPxDocumentViewer1" runat="server" Width="910px" Theme="Moderno">
                            <SettingsSplitter RightPaneVisible="False" SidePaneVisible="False" />
                        </dx:ASPxDocumentViewer>
                        
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ServerListResetButton" />
                        <asp:AsyncPostBackTrigger ControlID="ServerListFilterComboBox" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td class="tdpadded" valign="top">
            
                    </td>
        </tr>
        </table>
   </asp:Content>