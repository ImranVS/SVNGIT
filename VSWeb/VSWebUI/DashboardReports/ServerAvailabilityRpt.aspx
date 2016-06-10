<%@ Page Title="VitalSigns Plus - Server Down Time" Language="C#" MasterPageFile="~/Reports.Master" AutoEventWireup="true" CodeBehind="ServerAvailabilityRpt.aspx.cs" Inherits="VSWebUI.DashboardReports.ServerAvailabilityRpt" %>

<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>


<%@ Register assembly="DevExpress.XtraReports.v14.2.Web, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.XtraReports.Web" tagprefix="dx" %>


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
        <table>
        <tr>
            <td class="tdpadded" valign="top">
                <asp:UpdatePanel ID="UpdatePanel6" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <table>
                    <tr>
                        <td>
                        <dx:ASPxButton ID="SubmitButton" runat="server" onclick="SubmitButton_Click" 
                            Text="Submit" CssClass="sysButton">
                        </dx:ASPxButton>
                            </td>
                        <td>
                                <dx:ASPxButton ID="ResetButton" runat="server" OnClick="ResetButton_Click" 
                                    Text="Reset" CssClass="sysButton">
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
                        <td valign="top">
                                <dx:ASPxLabel ID="ASPxLabel8" runat="server" CssClass="lblsmallFont" 
                                    Text="Server Type:">
                                </dx:ASPxLabel>
                            </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <dx:ASPxListBox ID="ServerTypeFilterListBox" runat="server" AutoPostBack="True" 
                                    ValueType="System.String">
                                </dx:ASPxListBox>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="ResetButton" />
                                </Triggers>
                            </asp:UpdatePanel>
                            </td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td valign="top">
                                <dx:ASPxLabel ID="ASPxLabel2" runat="server" CssClass="lblsmallFont" 
                                    Text="Server(s):">
                                </dx:ASPxLabel>
                            </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <dx:ASPxListBox ID="ServerListFilterListBox" runat="server" 
                            SelectionMode="CheckColumn" TextField="ServerName" ValueField="ServerName" 
                            ValueType="System.String">
                        </dx:ASPxListBox>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="ServerTypeFilterListBox" />
                                    <asp:AsyncPostBackTrigger ControlID="ResetButton" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                    </tr>
                    <tr>
                        <td valign="top">
                                <dx:ASPxLabel ID="ASPxLabel4" runat="server" CssClass="lblsmallFont" 
                                    Font-Bold="True" Text="OR">
                                </dx:ASPxLabel>
                            </td>
                    </tr>
                    <tr>
                        <td></td>
                    </tr>
                    <tr>
                        <td valign="top">
                                <dx:ASPxLabel ID="ASPxLabel5" runat="server" CssClass="lblsmallFont" 
                                    Text="Specify a part of the server name to filter by:">
                                </dx:ASPxLabel>
                            </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <dx:ASPxTextBox ID="ServerFilterTextBox" runat="server" Width="170px">
                                    </dx:ASPxTextBox>                                    
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="ResetButton" />
                                </Triggers>
                            </asp:UpdatePanel>
                            </td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td valign="top">
                                <dx:ASPxLabel ID="ASPxLabel7" runat="server" CssClass="lblsmallFont" 
                                    
                                Text="Specify down time in minutes (filter by down time greater than X):">
                                </dx:ASPxLabel>
                            </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <dx:ASPxTextBox ID="DownFilterTextBox" runat="server" Width="170px">
                                        <MaskSettings Mask="&lt;0..99999&gt;" />
                                        <ValidationSettings CausesValidation="True" ErrorDisplayMode="ImageWithTooltip" SetFocusOnError="True">
                                            <RegularExpression ErrorText="Please enter a numeric value using the numbers only (0-9)." 
                                                ValidationExpression="^\d+$" />
                                        </ValidationSettings>

                                    </dx:ASPxTextBox>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="ResetButton" />
                                </Triggers>
                            </asp:UpdatePanel>
                            </td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <label class="lblsmallFont" for="startDate">
                            Month/Year:</label>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <input id="startDate" runat="server" class="date-picker" name="startDate"  />
                        </td>
                    </tr>
                </table>
            </td>
            <td valign="top">
                <table>
                    <tr>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="SubmitButton" />
                    <asp:AsyncPostBackTrigger ControlID="ResetButton" />
                </Triggers>
                <ContentTemplate>
                    <dx:ASPxDocumentViewer ID="ASPxDocumentViewer1" runat="server" Width="910px" Theme="Moderno" SettingsSplitter-SidePaneVisible="false">
                    </dx:ASPxDocumentViewer>
                </ContentTemplate>
                </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>    
            </td>
        </tr>
        </table>
</asp:Content>