<%@ Page  Title="VitalSigns Plus - Server Utilization" Language="C#" MasterPageFile="~/Reports.Master" AutoEventWireup="true" CodeBehind="ServerUtilization.aspx.cs" Inherits="VSWebUI.DashboardReports.ServerUtilization" %>

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
   
<script type="text/javascript">
     function OnKeyDown(s, e) {
      
        var keyCode = window.event.keyCode;
        if (keyCode == 13)
            SubmitButton.DoClick();
    }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table width="100%">
        <tr>
            <td class="style1" valign="top">
                <table>
                    <tr>
                        <td>
                <dx:ASPxButton ID="SubmitButton" runat="server" onclick="SubmitButton_Click"  ClientInstanceName="SubmitButton" 
                    Text="Submit" CssClass="sysButton">
                </dx:ASPxButton>
                
                        </td>
                    </tr>
                </table>
              
                <div class="input-prepend">&nbsp;</div>
                <table>
                  
                    <tr>
                         <td>
            <dx:ASPxLabel ID="TypeLabel" runat="server" CssClass="lblsmallFont" 
                Text="Type:">
            </dx:ASPxLabel>
        </td>
          </tr>

                    <tr>
        <td colspan="2">
            <dx:ASPxComboBox ID="TypeComboBox" runat="server" onselectedindexchanged="TypeComboBox_SelectedIndexChanged"   AutoPostBack="true">
                <Items>
                    <dx:ListEditItem Text="Select Type" Value="Select Type" />
                </Items>
            </dx:ASPxComboBox>
        </td>
                    </tr>

                    <tr>
                       <td>
            <dx:ASPxLabel ID="UserTypelabel" runat="server" CssClass="lblsmallFont" 
                Text="User Type:" >
            </dx:ASPxLabel>
        </td>
          </tr>
                    <tr>
                <td>
                    <dx:ASPxComboBox ID="UserTypeComboBox" runat="server" ValueType="System.String" >
                    </dx:ASPxComboBox>
        </td>
                    </tr>
                    <tr>
                    <td colspan="2">
                      
                    <dx:ASPxLabel ID="SearchLabel" runat="server" CssClass="lblsmallFont" 
                        Text="Specify a part of the server name to filter by:">
                    </dx:ASPxLabel>
     
                    </td>
                    </tr>
                    <tr>
                     
        <td>
            <dx:ASPxTextBox ID="SearchTextBox" runat="server" 
                ClientInstanceName="searchTxtBox" Width="170px">
                <%--<ClientSideEvents KeyDown="function(s, e) {OnKeyDown(s, e);}" />--%>
            </dx:ASPxTextBox>
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
