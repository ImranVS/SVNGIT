<%@ Page Title="VitalSigns Plus-Favorites" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="ReportFavorites.aspx.cs" Inherits="VSWebUI.Configurator.ReportFavorites" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
         
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <table class="tableWidth100Percent">
        <tr>
            <td>
                <asp:Label ID="Label1" runat="server" Text="Favorites" Style="color: #000000;
                    font-weight: 700; font-size: medium"></asp:Label></td>
            <td>
                <asp:Label ID="Label2" runat="server" Text="Usage Popularity" Style="color: #000000;
                    font-weight: 700; font-size: medium"></asp:Label></td>
            <td>
                <asp:Label ID="Label3" runat="server" Text="Top Rated" Style="color: #000000;
                    font-weight: 700; font-size: medium"></asp:Label></td>
        </tr>
        <tr>
            <td valign="top">
                <ul runat="server" id="favoriteReports" style="list-style-type:none;">
                <li>link1</li>
            </ul></td>
            <td valign="top">
                <ul runat="server" id="popularReports" style="list-style-type:none;">
                <li>link1</li>
            </ul></td>
            <td valign="top">
                <ul runat="server" id="topReports" style="list-style-type:none;">
                <li>link1</li>
            </ul></td>
        </tr>
    </table>

</asp:Content>