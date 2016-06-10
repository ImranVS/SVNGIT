<%@ Page Title="VitalSigns Plus - MailFilesRpt" Language="C#" MasterPageFile="~/Reports.Master" AutoEventWireup="true" CodeBehind="MailFileRpt.aspx.cs" Inherits="VSWebUI.ConfiguratorReports.MailFileRpt" %>

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
           width: 3px;
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
                    <dx:ASPxButton ID="MailFileButton" runat="server" 
                        onclick="MailFileButton_Click" Text="Submit" CssClass="sysButton">
                    </dx:ASPxButton>
                        </td>
                        <td>
                            <dx:ASPxButton ID="ServerListResetButton" runat="server" CssClass="sysButton"
                        onclick="ServerListResetButton_Click" style="margin-left: 0px" Text="Reset">
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
                        <td colspan="2" valign="top">
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" valign="top">
                            <table>
                    <tr>
                        <td>
                    <dx:ASPxLabel ID="ASPxLabel1" runat="server" 
                        
                                Text="Mail file size threshold:" 
                                CssClass="lblsmallFont">
                    </dx:ASPxLabel>
                    <br />
                            <dx:ASPxLabel ID="ASPxLabel2" runat="server" CssClass="lblsmallFont" 
                                Text="(will filter mail file size exceeding the entry)" Wrap="True">
                            </dx:ASPxLabel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table>
                                <tr>
                                    <td>
                                        <dx:ASPxTextBox ID="MailFileTextBox" runat="server" NullText="0" Width="100px">
                                        </dx:ASPxTextBox>
                                    </td>
                                    <td>
                                        <dx:ASPxLabel ID="ASPxLabel3" runat="server" CssClass="lblsmallFont" Text="MB">
                            </dx:ASPxLabel>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                        </td>
                    </tr>
                </table>
            </td>
            <td valign="top">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <dx:ASPxDocumentViewer ID="ASPxDocumentViewer1" runat="server" Width="910px" Theme="Moderno" SettingsSplitter-SidePaneVisible="false">
                        </dx:ASPxDocumentViewer>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="MailFileButton" />
                        <asp:AsyncPostBackTrigger ControlID="ServerListResetButton" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
        </table>
     </div>
   </asp:Content>
