<%@ Page Title="VitalSigns Plus" Language="C#" MasterPageFile="~/DashboardSite.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="VSWebUI.Dashboard.Default" %>
<%@ MasterType virtualpath="~/DashboardSite.Master" %>
<%@ Register assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <table style="width: 100%;" >
        <tr>
            <td>
                &nbsp;</td>
            <td align=left valign=top>
                <dx:ASPxLabel ID="ASPxLabel47" runat="server" Font-Size="Large" 
                    ForeColor="#182A50" Text="VitalSigns Dashboard" Width="250px" 
                    Font-Bold="True">
                </dx:ASPxLabel>
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
            <td>
                <br />
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
            <td>
                <dx:ASPxLabel ID="ASPxLabel2" runat="server" ForeColor="Black" 
                    Text="Welcome to VS Dashboard. Check the below FAQs.">
                </dx:ASPxLabel>
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;&nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
            <td>
                <asp:HyperLink ID="HyperLink1" runat="server" CssClass="styleone1" 
                    NavigateUrl="~/Configurator/">Go to Configurator</asp:HyperLink>
            </td>
            <td>
                &nbsp;</td>
        </tr>
    </table>
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />

</asp:Content>
