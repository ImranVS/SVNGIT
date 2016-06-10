<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true"
    CodeBehind="TestReport.aspx.cs" Inherits="VSWebUI.ConfiguratorReports.TestReport" %>

<%@ Register Src="~/Controls/DateRange.ascx" TagName="DateRange" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:DateRange ID="dtPick" runat="server" Width="100px" Height="100%"></uc1:DateRange>
    <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Button" /> 
    <br />
    <asp:Label ID="Label1" runat="server" Text="" Style="color: Black"></asp:Label>
</asp:Content>
