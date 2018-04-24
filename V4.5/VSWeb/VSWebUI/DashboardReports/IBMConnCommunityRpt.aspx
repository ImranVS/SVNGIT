<%@ Page Title="VitalSigns Plus - IBM Community" Language="C#" AutoEventWireup="true"
    MasterPageFile="~/Reports.Master" CodeBehind="IBMConnCommunityRpt.aspx.cs" Inherits="VSWebUI.DashboardReports.IBMConnCommunityRpt" %>

<%@ Register Assembly="DevExpress.XtraReports.v14.2.Web, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.XtraReports.Web" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>
<asp:content id="Content1" contentplaceholderid="head" runat="server">
<link href="../css/bootstrap1.min.css" rel="stylesheet" />
    <style type="text/css">
    .tdpadded
    {
        padding-left:20px;
    }
</style>
</asp:content>
<asp:content id="Content2" contentplaceholderid="ContentPlaceHolder1" runat="server">
    <div>
    <table>
        <tr>
            <td class="tdpadded" valign="top">
                <table>
                    <tr>
                    <td>
                                <dx:ASPxButton ID="SubmitButton" runat="server" Text="Submit" CssClass="sysButton"  onclick="SubmitButton_Click">
                                </dx:ASPxButton>
                            </td>
                        <td>
                            <dx:ASPxButton ID="UserResetButton" runat="server" CssClass="sysButton"
                      style="margin-left: 0px" Text="Reset" onclick="UserResetButton_Click" >
                    </dx:ASPxButton>
                              
                        </td>
                    </tr>
                    </table>
                    <table>
                     <tr>
                            <td>
                                <dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="Servers:" CssClass="lblsmallFont">
                                </dx:ASPxLabel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                              
                                    <dx:ASPxComboBox ID="ServerComboBox" runat="server"  OnSelectedIndexChanged = "ServerComboBox_SelectedIndexChanged"
                                     EnableTheming="True" 
                                    
                                     Theme="Default" 
                                    AutoPostBack="True">
                                   </dx:ASPxComboBox>
                                     
                                   
                            </td>       
                        </tr>
                    <tr>
                        <td>
                            <dx:ASPxLabel ID="ASPxLabel1" runat="server" 
            Text="Select a value from the list to filter by community:" CssClass="lblsmallFont">
        </dx:ASPxLabel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <dx:ASPxComboBox ID="UserFilterComboBox" runat="server" 
            DropDownStyle="DropDown" 
             Theme="Default" >
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
                        <asp:AsyncPostBackTrigger ControlID="UserResetButton" />
                        <asp:AsyncPostBackTrigger ControlID="UserFilterComboBox" />
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
   </asp:content>
