<%@ Page title ="VitalSigns Plus - DominoResponseTimesMonthlyRpt" Language="C#" MasterPageFile="~/Reports.Master" AutoEventWireup="true" CodeBehind="DominoResponseTimesMonthlyRpt.aspx.cs" Inherits="VSWebUI.DashboardReports.DominoResponseTimesMonthlyRpt" %>

<%@ Register assembly="DevExpress.XtraReports.v14.2.Web, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.XtraReports.Web" tagprefix="dx" %>

<%@ Register assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<link href="../css/bootstrap1.min.css" rel="stylesheet" />   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<script type="text/javascript" src="../js/jquery-1.9.1.js"></script>
    <script type="text/javascript" src="../js/jquery-ui-1.10.3.custom.js"></script>
    <script type="text/javascript" src="../js/jquery-ui-1.10.3.custom.min.js"></script>
    <link rel="stylesheet" type="text/css" media="screen" href="../css/jquery-ui-1.10.3.custom.css" />
    <link rel="stylesheet" type="text/css" media="screen" href="../css/jquery-ui-1.10.3.custom.min.css" />
    <script type="text/javascript">
        $(function () {
            $('.date-picker').datepicker({
                changeMonth: true,
                changeYear: true,
                showButtonPanel: true,
                dateFormat: 'MM yy',
                onClose: function (dateText, inst) {
                    var month = $("#ui-datepicker-div .ui-datepicker-month :selected").val();
                    var year = $("#ui-datepicker-div .ui-datepicker-year :selected").val();
                    $(this).datepicker('setDate', new Date(year, month, 1));
                }
            });
        });
</script>
   <style type="text/css">
     button.ui-datepicker-current { display: none; }
    .tdpadded
    {
        padding-left:20px;
    }
    .ui-datepicker-calendar {
    display: none;
    }
</style>

    <div>
        <table>
            <tr>
                <td class="tdpadded" valign="top">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table>
                        <tr>
                            <td>
                    <dx:ASPxButton ID="SubmitButton" runat="server" onclick="SubmitButton_Click" 
                        Text="Submit" CssClass="sysButton">
                    </dx:ASPxButton>
                            </td>
                            <td>
                    <dx:ASPxButton ID="ServerListResetButton" runat="server" Text="Reset" 
                        onclick="ServerListResetButton_Click" CssClass="sysButton">
                    </dx:ASPxButton>
                            </td>
                        </tr>
                    </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <dx:ASPxButton ID="ReptBtn" runat="server" Text="Reports" 
            onclick="ReptBtn_Click" Theme="Office2010Blue" Visible="False">
        </dx:ASPxButton>
                    <div class="input-prepend">&nbsp;</div>
                    <table>
                        <tr>
                            <td>
                    <dx:ASPxLabel ID="ASPxLabel1" runat="server" 
                        Text="Server(s):" CssClass="lblsmallFont">
                    </dx:ASPxLabel>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top">
                                <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <dx:ASPxListBox ID="ServerListFilterListBox" runat="server" 
                        SelectionMode="CheckColumn" ValueType="System.String">
                    </dx:ASPxListBox>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="ServerListResetButton" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top">
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td valign="top">
                                <label class="lblsmallFont" for="startDate">
                    Month/Year:</label>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top">
                                <input id="startDate" runat="server" class="date-picker" name="startDate" />
                            </td>
                        </tr>
                    </table>
                </td>
                <td valign="top">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                    <table class="style1">
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
                            <LabelStyle>
                            <Margins MarginLeft="3px" MarginRight="3px" />
                            </LabelStyle>
                        </styles>
                    </dx:ReportToolbar>
                    <dx:ReportViewer ID="ReportViewer1" runat="server">
                    <ClientSideEvents EndCallback="function(s, e) {
	setDivs();
}" />
        </dx:ReportViewer>
                </td>
            </tr>

        </table>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="SubmitButton" />
            <asp:AsyncPostBackTrigger ControlID="ServerListResetButton" />
        </Triggers>
        </asp:UpdatePanel>
                </td>
            </tr>
                    </table>  
    </div>
    </asp:Content>