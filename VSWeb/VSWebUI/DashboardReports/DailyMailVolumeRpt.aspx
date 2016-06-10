<%@ Page Title="VitalSigns Plus - DailyMailVolumeRpt" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="DailyMailVolumeRpt.aspx.cs" Inherits="VSWebUI.DashboardReports.DailyMailVolumeRpt" %>

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
                    <dx:ASPxButton ID="SubmitButton" runat="server" onclick="SubmitButton_Click" 
                        Text="Submit" CssClass="sysButton">
                    </dx:ASPxButton>
                            </td>
                            <td>
                    <dx:ASPxButton ID="ServerListResetButton" runat="server" Text="Reset" 
                        onclick="ServerListResetButton_Click" CssClass="sysButton">
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
        <td colspan="2">
            <dx:ASPxComboBox ID="TypeComboBox" runat="server" onselectedindexchanged="TypeComboBox_SelectedIndexChanged"   AutoPostBack="">
                <Items>
                    <dx:ListEditItem Text="Select Type" Value="Select Type" />
                </Items>
            </dx:ASPxComboBox>
        </td>
                    </tr>
                        <tr>
                            <td>
                    <dx:ASPxLabel ID="ASPxLabel1" runat="server" 
                        Text="Select server(s) from the list below:" CssClass="lblsmallFont">
                    </dx:ASPxLabel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                    <dx:ASPxListBox ID="ServerListFilterListBox" runat="server" 
                        SelectionMode="CheckColumn" ValueType="System.String">
                    </dx:ASPxListBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <dx:ASPxRadioButtonList ID="DailyMonthlyRadioButtonList" runat="server" 
                                    AutoPostBack="True" RepeatDirection="Horizontal" SelectedIndex="0" 
                                    onselectedindexchanged="DailyMonthlyRadioButtonList_SelectedIndexChanged">
                                    <Items>
                                        <dx:ListEditItem Selected="True" Text="Daily" Value="Daily" />
                                        <dx:ListEditItem Text="Monthly" Value="Monthly" />
                                    </Items>
                                </dx:ASPxRadioButtonList>
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
                                        <dx:ASPxDocumentViewer ID="ASPxDocumentViewer1" runat="server" Width="910px"  Theme="Moderno" SettingsSplitter-SidePaneVisible="false">
                                        </dx:ASPxDocumentViewer>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="DailyMonthlyRadioButtonList" />                                    
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    </asp:Content>
