<%@ Page Title="VitalSigns Plus - DominoDiskRpt" Language="C#" AutoEventWireup="true" CodeBehind="DominoDiskRpt.aspx.cs" Inherits="VSWebUI.DashboardReports.DominoDiskRpt" %>

<%@ Register assembly="DevExpress.XtraReports.v14.2.Web, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.XtraReports.Web" tagprefix="dx" %>

<%@ Register assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .style1
        {
            width: 100%;
        }
        .style2
        {
        }
        .style4
        {
            width: 213px;
        }
        .style5
        {
            width: 251px;
        }
        .style6
        {
            width: 87px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <dx:ReportToolbar ID="ReportToolbar1" runat="server" 
            ReportViewerID="ReportViewer1" ShowDefaultButtons="False">
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
                    <elements>
                        <dx:ListElement Value="pdf" />
                        <dx:ListElement Value="xls" />
                        <dx:ListElement Value="xlsx" />
                        <dx:ListElement Value="rtf" />
                        <dx:ListElement Value="mht" />
                        <dx:ListElement Value="html" />
                        <dx:ListElement Value="txt" />
                        <dx:ListElement Value="csv" />
                        <dx:ListElement Value="png" />
                    </elements>
                </dx:ReportToolbarComboBox>
            </Items>
            <styles>
                <LabelStyle><Margins MarginLeft='3px' MarginRight='3px' /></LabelStyle>
            </styles>
        </dx:ReportToolbar>
        <table class="style1">
            <tr>
                <td class="style5">
                    <dx:ASPxLabel ID="ASPxLabel1" runat="server" 
                        Text="Select a value from the list to filter by server:" Theme="Aqua">
                    </dx:ASPxLabel>
                </td>
                <td class="style4">
                    <dx:ASPxLabel ID="ASPxLabel2" runat="server" 
                        Text="Select month/year from the calendar:">
                    </dx:ASPxLabel>
                </td>
                <td class="style6">
                    &nbsp;</td>
                <td class="style2">
                    &nbsp;</td>
            </tr>
            <tr>
                <td class="style5">
                    <dx:ASPxComboBox ID="ServerListFilterComboBox" runat="server" 
                        DataSourceID="SqlDataSource1" EnableTheming="True" 
                        onselectedindexchanged="ServerListFilterComboBox_SelectedIndexChanged" 
                        TextField="ServerName" Theme="Aqua" ValueField="ServerName">
                    </dx:ASPxComboBox>
                </td>
                <td class="style4">
                    <dx:ASPxDateEdit ID="DateParamEdit" runat="server" Date="10/25/2012 09:30:51" 
                        DisplayFormatString="MM/yyyy" EditFormat="Custom" EditFormatString="MM/yyyy" 
                        Theme="Aqua">
                    </dx:ASPxDateEdit>
                </td>
                <td class="style6">
                    <dx:ASPxButton ID="SubmitButton" runat="server" onclick="SubmitButton_Click" 
                        Text="Submit" Theme="Office2010Blue" Width="80px">
                    </dx:ASPxButton>
                </td>
                <td>
                    <dx:ASPxButton ID="ServerListResetButton" runat="server" 
                        onclick="ServerListResetButton_Click" Text="Reset" Theme="Office2010Blue" 
                        Width="80px">
                    </dx:ASPxButton>
                </td>
            </tr>
        </table>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
            ConnectionString="<%$ ConnectionStrings:VSS_StatisticsConnectionString %>" SelectCommand="SELECT DISTINCT ServerName FROM DominoDailyStats WHERE StatName = 'Disk.C.Free'
ORDER BY ServerName"></asp:SqlDataSource>
        <dx:ReportViewer ID="ReportViewer1" runat="server">
        </dx:ReportViewer>
    
    </div>
    </form>
</body>
</html>
