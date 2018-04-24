<%@ Page  Title="VitalSigns Plus - ResponseTimeRpt" Language="C#" MasterPageFile="~/Reports.Master" AutoEventWireup="true" CodeBehind="ResponseTimeRpt.aspx.cs" Inherits="VSWebUI.DashboardReports.ResponseTimeRpt" %>

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
                    <tr>
                        <td>
                <dx:ASPxButton ID="SubmitButton" runat="server" onclick="SubmitButton_Click" 
                    Text="Submit" CssClass="sysButton">
                </dx:ASPxButton>
                    <dx:ASPxButton ID="EmailRptButton" runat="server" 
                        onclick="EmailRptButton_Click" Text="Email Report" Theme="Office2010Blue" 
                        Visible="False">
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
                <dx:ASPxLabel ID="ASPxLabel1" runat="server" CssClass="lblsmallFont" 
                    Text="Type:">
                </dx:ASPxLabel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                <dx:ASPxComboBox ID="TypeComboBox" runat="server" ValueType="System.String">
                </dx:ASPxComboBox>
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
                        <asp:AsyncPostBackTrigger ControlID="SubmitButton" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>  
   </asp:Content>
