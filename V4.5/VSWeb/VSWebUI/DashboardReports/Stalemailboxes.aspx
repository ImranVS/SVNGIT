<%@ Page  Title="VitalSigns Plus - Stale mailboxes" Language="C#" MasterPageFile="~/Reports.Master" AutoEventWireup="true" CodeBehind="Stalemailboxes.aspx.cs" Inherits="VSWebUI.DashboardReports.Stalemailboxes" %>

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
       .style1
       {
           padding-left: 20px;
           height: 31px;
       }
   </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table width="100%">
        <tr>
            <td class="style1" valign="top">
                <table>
               <tr><td>
                <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table>
                        <tr>
                            <td>
            <dx:ASPxButton ID="SubmitButton" runat="server" Text="Submit" 
                onclick="SubmitButton_Click" CssClass="sysButton">
            </dx:ASPxButton> 

                            </td>
                            <td>
        <dx:ASPxButton ID="ResetButton" runat="server" Text="Reset" 
            onclick="ResetButton_Click" CssClass="sysButton">
            </dx:ASPxButton> 
                            </td>
                        </tr>
                    </table>        
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    </td></tr>
             
                </table>
                <dx:ASPxButton ID="ReptBtn" runat="server" Text="Reports" 
            onclick="ReptBtn_Click" Theme="Office2010Blue" Visible="False">
            </dx:ASPxButton>
                <div class="input-prepend">&nbsp;</div>
                <table>
                   
                         <tr>
                            <td valign="top" class="style1">
                                <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                    <table>
                                       <tr>
                                        <td>
                                        <dx:ASPxLabel ID="ServerLabel" runat="server" Text="Servers:"  CssClass="lblsmallFont"/>
                                        </td>
                                        </tr>
                                    <tr>
                                    <td>
                                        <dx:ASPxListBox ID="ServerListBox" runat="server" ValueType="System.String" 
                                            SelectionMode="CheckColumn" Theme="Default">
                                        </dx:ASPxListBox>
                                        </td>
                                        </tr>
                                     
                                        </table>
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
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <dx:ASPxDocumentViewer ID="ASPxDocumentViewer1" runat="server" Width="910" Theme="Moderno" SettingsSplitter-SidePaneVisible="false">
                        </dx:ASPxDocumentViewer>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="SubmitButton" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>  
   </asp:Content>
