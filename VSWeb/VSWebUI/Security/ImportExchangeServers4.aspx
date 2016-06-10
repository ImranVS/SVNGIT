<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site1.Master" CodeBehind="ImportExchangeServers4.aspx.cs" Inherits="VSWebUI.Security.ImportExchangeServers4" %>
<%@ MasterType virtualpath="~/Site1.Master" %>
<%@ Register assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<link href='http://fonts.googleapis.com/css?family=Francois One' rel='stylesheet' type='text/css' />
<link rel="stylesheet" type="text/css" href="../css/vswebforms.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<table>
    <tr>
        <td>
        <div class="header" id="servernamelbldisp" runat="server">Import Servers</div>
        </td>
    </tr>
    <tr>
        <td>
        </td>
    </tr>
    <tr>
        <td>
            
            <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" 
                HeaderText="Step 4 - Complete Import or Import Additional Servers" 
                Theme="Glass" Width="700px">
                <PanelCollection>
<dx:PanelContent runat="server" SupportsDisabledAttribute="True">
    <table>
        <tr>
            <td colspan="2">
                <dx:ASPxLabel ID="ASPxLabel1" runat="server" 
                    Text="The following server(s) have been imported:" CssClass="lblsmallFont">
                </dx:ASPxLabel>
            </td>
        </tr>
        <tr>
            <td colspan="2">
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <dx:ASPxLabel ID="SrvLabel" runat="server" ForeColor="Black">
                </dx:ASPxLabel>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <dx:ASPxLabel ID="ASPxLabel2" runat="server" CssClass="lblsmallFont" 
                    Text="The number of server(s) have been imported:">
                </dx:ASPxLabel>
                <dx:ASPxLabel ID="SrvCountLabel" runat="server" ForeColor="Black">
                </dx:ASPxLabel>
            </td>
        </tr>
        <tr>
            <td colspan="2">
            </td>
        </tr>
        <tr>
            <td>
                <table>
                    <tr>
                        <td>
                            <dx:ASPxButton ID="ImportAddlButton" runat="server" 
                    OnClick="ImportAddlButton_Click" Text="Import Additional Servers" 
                    Theme="Office2010Blue">
                </dx:ASPxButton>
                        </td>
                        <td>
                            <dx:ASPxButton ID="DoneButton" runat="server" OnClick="DoneButton_Click" 
                    Text="Done" Theme="Office2010Blue">
                </dx:ASPxButton>
                        </td>
                    </tr>
                </table>
                
            </td>
        </tr>
    </table>
                    </dx:PanelContent>
</PanelCollection>
            </dx:ASPxRoundPanel>
            
        </td>
    </tr>
</table>
</asp:Content>