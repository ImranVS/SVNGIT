<%@ Page Title="VitalSigns Plus - HistoricalResponseTime" Language="C#" MasterPageFile="~/DashboardSite.Master" AutoEventWireup="true" CodeBehind="HistoricalResponseTime.aspx.cs" Inherits="VSWebUI.WebForm9" %>
<%@ Register assembly="DevExpress.XtraReports.v14.2.Web, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.XtraReports.Web" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style2
    {
        width: 89px;
    }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <br />
        <dx:ASPxButton ID="ReptBtn" runat="server" Text="Reports" 
            onclick="ReptBtn_Click" Theme="Office2010Blue">
        </dx:ASPxButton><br />

 <dx:ReportToolbar ID="ReportToolbar1" 
        runat="server" ReportViewerID="ReportViewer1" ShowDefaultButtons="False">
        <Items>
            <dx:ReportToolbarButton ItemKind="Search" />
            <dx:ReportToolbarSeparator />
            <dx:ReportToolbarButton ItemKind="PrintReport" />
            <dx:ReportToolbarButton ItemKind="PrintPage" />
            <dx:ReportToolbarSeparator />
            <dx:ReportToolbarButton Enabled="False" ItemKind="FirstPage" />
            <dx:ReportToolbarButton Enabled="False" ItemKind="PreviousPage" />
            <dx:ReportToolbarLabel ItemKind="PageLabel" />
            <dx:ReportToolbarComboBox ItemKind="PageNumber" Width="65px">
            </dx:ReportToolbarComboBox>
            <dx:ReportToolbarLabel ItemKind="OfLabel" />
            <dx:ReportToolbarTextBox IsReadOnly="True" ItemKind="PageCount" />
            <dx:ReportToolbarButton ItemKind="NextPage" />
            <dx:ReportToolbarButton ItemKind="LastPage" />
            <dx:ReportToolbarSeparator />
            <dx:ReportToolbarButton ItemKind="SaveToDisk" />
            <dx:ReportToolbarButton ItemKind="SaveToWindow" />
            <dx:ReportToolbarComboBox ItemKind="SaveFormat" Width="70px">
                <Elements>
                    <dx:ListElement Value="pdf" />
                    <dx:ListElement Value="xls" />
                    <dx:ListElement Value="xlsx" />
                    <dx:ListElement Value="rtf" />
                    <dx:ListElement Value="mht" />
                    <dx:ListElement Value="html" />
                    <dx:ListElement Value="txt" />
                    <dx:ListElement Value="csv" />
                    <dx:ListElement Value="png" />
                </Elements>
            </dx:ReportToolbarComboBox>
        </Items>
        <Styles>
            <LabelStyle>
            <Margins MarginLeft="3px" MarginRight="3px" />
            </LabelStyle>
        </Styles>
    </dx:ReportToolbar>
&nbsp;<table>
        <tr>
            <td>
                <dx:ASPxLabel ID="ASPxLabel5" runat="server" Text="Graph Type:">
                </dx:ASPxLabel>
            </td>
            <td>
                <dx:ASPxComboBox ID="GraphCombobox" runat="server" AutoPostBack="True" 
                    onselectedindexchanged="GraphCombobox_SelectedIndexChanged">
                    <Items>
                        <dx:ListEditItem Text="Today" Value="Today" />
                        <dx:ListEditItem Text="Two Days" Value="Two Days" />
                        <dx:ListEditItem Text="Daily" Value="Daily" />
                        <dx:ListEditItem Text="Weekly" Value="Weekly" />
                        <dx:ListEditItem Text="Monthly" Value="Monthly" />
                    </Items>
                </dx:ASPxComboBox>
            </td>
            <td class="style2">
                <dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="Start Date:">
                </dx:ASPxLabel>
            </td>
            <td>
                <dx:ASPxDateEdit ID="StartDateEdit" runat="server">
                </dx:ASPxDateEdit>
            </td>
        </tr>
        <tr>
            <%-- <Items>
                        <dx:ListEditItem Selected="True" />
                        <dx:ListEditItem Text="NotesMail" Value="NotesMail" />
                        <dx:ListEditItem Text="BlackBerry" Value="BlackBerry" />
                    </Items>--%><%--<td>
                <dx:ASPxLabel ID="ASPxLabel3" runat="server" Text="End Date:">
                </dx:ASPxLabel>
            </td>--%>
            <td class="style2">
                <dx:ASPxLabel ID="ASPxLabel4" runat="server" Text="Device Type:">
                </dx:ASPxLabel>
            </td>
            <td>
                <dx:ASPxComboBox ID="DeviceTypeComboBox" runat="server" AutoPostBack="True" 
                    onselectedindexchanged="DeviceTypeComboBox_SelectedIndexChanged" 
                    SelectedIndex="0">
                </dx:ASPxComboBox>
            </td>
              <td>
                <dx:ASPxLabel ID="ASPxLabel3" runat="server" Text="End Date:">
                </dx:ASPxLabel>
            </td>
            <td>
                <dx:ASPxDateEdit ID="EndDateEdit" runat="server">
                </dx:ASPxDateEdit>
            </td>
        </tr>
        <tr>
            <%--<td>
                <dx:ASPxDateEdit ID="EndDateEdit" runat="server">
                </dx:ASPxDateEdit>
            </td>--%>            <%--<td>
                <dx:ASPxDateEdit ID="EndDateEdit" runat="server">
                </dx:ASPxDateEdit>
            </td>--%>
            
            
             <td>
                <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="Server Name:">
                </dx:ASPxLabel>
            </td>
            <td>
                <dx:ASPxComboBox ID="ServerComboBox" runat="server" 
                   >
                </dx:ASPxComboBox>
            </td>
            <td class="style2">
                <dx:ASPxButton ID="Submitbtn" runat="server" Text="Submit" 
                    Theme="Office2010Blue" onclick="Submitbtn_Click">
                </dx:ASPxButton>
            </td>
            <td>
                <dx:ASPxButton ID="ResetBtn" runat="server" Text="Reset" Theme="Office2010Blue" 
                    onclick="ResetBtn_Click">
                </dx:ASPxButton>
            </td>
        </tr>
    </table>
    <br />
    <%--<asp:SqlDataSource ID="SqlDataSource1" runat="server" 
        ConnectionString="<%$ ConnectionStrings:VSS_StatisticsConnectionString %>" 
        SelectCommand="SELECT DISTINCT [DeviceName] FROM [DeviceDailyStats] ORDER BY [DeviceName]">
    </asp:SqlDataSource>--%>
    <%--<asp:SqlDataSource ID="SqlDataSource3" runat="server" 
        ConnectionString="<%$ ConnectionStrings:VSS_StatisticsConnectionString %>" 
        SelectCommand="SELECT DISTINCT [Name] FROM [BlackBerryProbeStats] ORDER BY [Name]"></asp:SqlDataSource>--%>
    <%--<asp:SqlDataSource ID="SqlDataSource2" runat="server" 
        ConnectionString="<%$ ConnectionStrings:VSS_StatisticsConnectionString %>" 
        
        SelectCommand="SELECT DISTINCT [Name] FROM [NotesMailStats] ORDER BY [Name]">
    </asp:SqlDataSource>--%>
    <%--<asp:SqlDataSource ID="DeviceTypeDS" runat="server" 
    ConnectionString="<%$ ConnectionStrings:VSS_StatisticsConnectionString %>" 
    SelectCommand="SELECT DISTINCT [DeviceType] FROM [DeviceDailyStats]">
</asp:SqlDataSource>--%>
    <dx:ReportViewer ID="ReportViewer1" runat="server">
    </dx:ReportViewer>
    <br />
</asp:Content>
