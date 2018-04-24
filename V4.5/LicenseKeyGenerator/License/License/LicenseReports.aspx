<%@ Page Title="VitalSigns Plus - LicenseReports" Language="C#" MasterPageFile="~/Site1.Master"
	AutoEventWireup="true" CodeBehind="LicenseReports.aspx.cs" Inherits="License.LicenseReports" %>

<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.Web" TagPrefix="dx" %>
	<%@ Register assembly="DevExpress.XtraReports.v14.2.Web, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.XtraReports.Web" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
	<%--<link href="../css/bootstrap1.min.css" rel="stylesheet" />--%>
   <%--<style type="text/css">
    .tdpadded
    {
        padding-left:20px;
    }
</style>--%>
<link href='http://fonts.googleapis.com/css?family=Francois One' rel='stylesheet'
		type='text/css' />
	<link rel="stylesheet" type="text/css" href="../css/vswebforms.css" />
	<script src="../js/jquery-1.9.1.js" type="text/javascript"></script>
	<script src="../js/bootstrap.min.js" type="text/javascript"></script>
	<script type="text/javascript">
		$(document).ready(function () {
			$('.alert-success').delay(10000).fadeOut("slow", function () {
			});
		});
	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
	<table>
        <tr>
            <td class="tdpadded" valign="top">
                <table>
                    <tr>
                        <td>
                            <table>
                                <%--<tr>
                                    <td>
                                        <dx:ASPxButton ID="LicenseResetButton" runat="server" 
                        onclick="LicenseResetButton_Click" Text="Reset" CssClass="sysButton">
                    </dx:ASPxButton>            
                                    </td>
                                </tr>--%>
                            </table>
                            <div class="input-prepend">&nbsp;</div>
                        </td>
                    </tr>
                  <%--  <tr>
                        <td>
                            <dx:ASPxButton ID="ReptBtn" runat="server" Text="Reports" 
            Theme="Office2010Blue" Visible="False">
        </dx:ASPxButton>    
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <dx:ASPxLabel ID="ASPxLabel2" runat="server" 
                        Text="Select a value from the list to filter by users:" 
                        ForeColor="SteelBlue">
                    </dx:ASPxLabel>
                        </td>
                    </tr>--%>
                    <%--<tr>
                        <td>
                            <dx:ASPxComboBox ID="UsersComboBox" runat="server" 
                         Theme="Default" 
                         AutoPostBack="True" 
                        onselectedindexchanged="UsersComboBox_SelectedIndexChanged">
                    </dx:ASPxComboBox>
                        </td>
                    </tr>--%>
                </table>
            </td>
            <td valign="top" rowspan="2">
                <table>
                    <tr>
                        <td>
                            <dx:ReportToolbar ID="ReportToolbar1" runat="server" 
            ReportViewerID="ReportViewer1" ShowDefaultButtons="False" Theme="Moderno">
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
                        </td>
                    </tr>
                    <tr>
                        <td>
                           

		<dx:ReportViewer ID="ReportViewer1" runat="server" ReportName="License.LicenseXtraReport">
			
		</dx:ReportViewer>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td class="tdpadded" valign="top">
            
                    </td>
        </tr>
        </table>
   </asp:Content>