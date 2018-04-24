<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site1.Master" CodeBehind="ImportServers3.aspx.cs" Inherits="VSWebUI.Security.ImportServers3" %>
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
                    HeaderText="Step 3 - Assign Server Tasks" Theme="Glass" Width="700px">
                    <PanelCollection>
<dx:PanelContent runat="server" SupportsDisabledAttribute="True">
    <table>
        <tr>
            <td>
                <dx:ASPxLabel ID="ASPxLabel3" runat="server" Text="Select server tasks:" 
                    CssClass="lblsmallFont">
                </dx:ASPxLabel>
            </td>
            <td>
                <table>
                    <tr>
                        <td>
                            <dx:ASPxButton ID="SelectAllTasksButton" runat="server" 
                    OnClick="SelectAllTasksButton_Click" Text="Select All" CssClass="sysButton">
                </dx:ASPxButton>
                        </td>
                        <td>
                            <dx:ASPxButton ID="DeselectAllTasksButton" runat="server" Text="Deselect All" CssClass="sysButton" 
                    OnClick="DeselectAllTasksButton_Click">
                </dx:ASPxButton>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <dx:ASPxCheckBoxList ID="SrvTaskCheckBoxList" runat="server" RepeatColumns="3">
                </dx:ASPxCheckBoxList>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <dx:ASPxCheckBoxList ID="SrvTaskIDCheckBoxList" runat="server" 
                    RepeatColumns="3" Visible="False">
                </dx:ASPxCheckBoxList>
            </td>
        </tr>
    </table>
    <br />
    <table>
        <tr>
            <td>
                <dx:ASPxLabel ID="ASPxLabel2" runat="server" 
                    Text="The following server(s) will be assigned tasks selected above:" 
                    CssClass="lblsmallFont">
                </dx:ASPxLabel>
            </td>
        </tr>
        <tr>
            <td>
            </td>
        </tr>
        <tr>
            <td>
                <dx:ASPxLabel ID="SrvLabel" runat="server" ForeColor="Black">
                </dx:ASPxLabel>
            </td>
        </tr>
        <tr>
            <td>
                <dx:ASPxCheckBoxList runat="server" ID="SrvCheckBoxList" RepeatColumns="5" 
                    Visible="False"></dx:ASPxCheckBoxList>

            </td>
        </tr>
        <tr>
            <td>
            </td>
        </tr>
        <tr>
            <td>
                <dx:ASPxButton ID="AssignButton" runat="server" OnClick="AssignButton_Click" 
                                Text="Next" CssClass="sysButton">
                            </dx:ASPxButton>
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