<%@ Page Title="VitalSigns Plus - ServerListLocRpt" Language="C#" MasterPageFile="~/Reports.Master" AutoEventWireup="true" CodeBehind="ServerListLocRpt.aspx.cs" Inherits="VSWebUI.ServerListRpt" %>

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
    <table>
        <tr>
            <td class="tdpadded" valign="top">
                <table>
                    <tr>
                        <td>
                            <dx:ASPxButton ID="ServerListResetButton" runat="server" CssClass="sysButton"
                        onclick="ServerListResetButton_Click" style="margin-left: 0px" Text="Reset">
                    </dx:ASPxButton>
                                <dx:ASPxButton ID="ReptBtn" runat="server" Text="Reports" 
            onclick="ReptBtn_Click" Theme="Office2010Blue" Visible="False">
        </dx:ASPxButton>
        <div class="input-prepend">&nbsp;</div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <dx:ASPxLabel ID="ASPxLabel1" runat="server" 
            Text="Select a value from the list to filter by location:" CssClass="lblsmallFont">
        </dx:ASPxLabel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <dx:ASPxComboBox ID="ServerListFilterComboBox" runat="server" 
            AutoPostBack="True"  DropDownStyle="DropDown" 
            onselectedindexchanged="ServerListFilterComboBox_SelectedIndexChanged" 
             Theme="Default">
        </dx:ASPxComboBox>
                        </td>
                    </tr>
                </table>
            </td>
            <td rowspan="4" valign="top">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <dx:ASPxDocumentViewer ID="ASPxDocumentViewer1" runat="server" Width="910px" Theme="Moderno" SettingsSplitter-SidePaneVisible="False">
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
            <td class="tdpadded">
                <table class="style1">
            <tr>
                <td class="style2" colspan="2">
        
                </td>
            </tr>
            <tr>
                <td class="style3">
        
                </td>
                <td>
                    &nbsp;</td>
            </tr>
        </table>
            </td>
        </tr>
        <tr>
            <td class="tdpadded">
                
            </td>
        </tr>
        <tr>
            <td class="tdpadded">
                
            </td>

        </tr>
    </table>
    </div>
   </asp:Content>
