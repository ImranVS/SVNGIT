<%@ Page Language="C#" MasterPageFile="~/DashboardSite.Master" AutoEventWireup="true" CodeBehind="MyCustomPages.aspx.cs" Inherits="VSWebUI.Dashboard.MyCustomPages" %>

<%@ MasterType virtualpath="~/DashboardSite.Master" %>
<%@ Register assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table width="60%">
    <tr>

            <td> <ul runat="server" id="manageCustomPage" style="list-style-type:none; font-family: Verdana;">
                <dx:ASPxButton ID="ASPxButton1" runat="server" Text="Manage your custom pages" 
                                                    onclick="ASPxButton1_Click" CssClass="sysButton">
                                                    
                                                </dx:ASPxButton>
            </ul></td>
    </tr>
     <tr>
        <td>
            <asp:Label ID="Label2" runat="server" Text="My " Style="color: #000000; font-family: Verdana;
                    font-weight: 700; font-size: medium; display: none"></asp:Label>
            <asp:Label ID="Label1" runat="server" Text="custom pages" Style="color: #000000; font-family: Verdana;
                    font-weight: 700; font-size: medium; display: none"></asp:Label>
            <div class="header" id="titleDiv" runat="server">My custom pages</div>
        </td>
        <td>
            <asp:Label ID="Label3" runat="server" Text="Company " Style="color: #000000; font-family: Verdana;
                    font-weight: 700; font-size: medium; display: none"></asp:Label>
            <asp:Label ID="Label4" runat="server" Text="custom pages" Style="color: #000000; font-family: Verdana;
                    font-weight: 700; font-size: medium; display: none"></asp:Label>
            <div class="header" id="title2Div" runat="server">Company custom pages</div>
        </td>
    </tr>
    <tr>
        <td valign="top">
        
            <ul runat="server" id="privatePages" style="list-style-type:none; font-family: Verdana;">
                <li></li>
            </ul>
            <br />
           
        </td>
        <td valign="top">
            <ul runat="server" id="publicPages" style="list-style-type:none; font-family: Verdana;">
                <li></li>
            </ul>
        </td>
    </tr>
    </table>
</asp:Content>
