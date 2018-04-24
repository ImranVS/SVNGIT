<%@ Page Language="C#" Title="VitalSigns Plus-Script Definition" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="ScriptDef.aspx.cs" Inherits="VSWebUI.Configurator.ScriptDef" %>

<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>







<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<link href='http://fonts.googleapis.com/css?family=Francois One' rel='stylesheet' type='text/css' />
<link rel="stylesheet" type="text/css" href="../css/vswebforms.css" />
<script src="../js/jquery-1.9.1.js" type="text/javascript"></script>
<script type="text/javascript">
        $(document).ready(function () {
            $('.alert-success').delay(10000).fadeOut("slow", function () {
            });
        });
</script>
<script type="text/javascript">
// <![CDATA[
    function Uploader_OnUploadStart() {
        ASPxButton1.SetEnabled(false);
    }
    function Uploader_OnFilesUploadComplete(args) {
        UpdateUploadButton();
    }
    function UpdateUploadButton() {
        ASPxButton1.SetEnabled(uploader.GetText(0) != "");
    }
// ]]>
</script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<table width="100%">
    <tr>
        <td>
            <div class="header" id="servernamelbldisp" runat="server">Script Definition</div>
        </td>
    </tr>
    <tr>
        <td>
            <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" Width="100%" 
                HeaderText="Script Information" Theme="Glass">
                <PanelCollection>
<dx:PanelContent runat="server">
    <table>
        <tr>
            <td>
                <div id="infoDiv" class="info">
                    When passing parameters to your custom scripts, the following variables can be used.  At run time, the variables will be substituted for their current values. <br /><br />
                    %Name%  - the of the server, device, or application <br />
                    %Type%  - the type of device, i.e., Exchange, Domino, SharePoint, etc.<br />
                    %EventType% - the type of exception, i.e., Dead Mail, Not Responding, Memory, etc.<br />
                    %DTD% - the date and time the alert condition was detected<br />
                    %Details%  - the details of the alert condition<br />
                </div>
            </td>
        </tr>
    </table>
    <table>
        <tr>
            <td>
                <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="Script Name:" 
                    CssClass="lblsmallFont" Wrap="False">
                </dx:ASPxLabel>
            </td>
            <td>
                <dx:ASPxTextBox ID="ScriptNameTextBox" runat="server" Width="280px" 
                    style="margin-bottom: 0px">
                    <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorText="">
                        <RequiredField ErrorText="Enter Script Name" IsRequired="True" />
                    </ValidationSettings>
                </dx:ASPxTextBox>
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                <dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="Command:" 
                    CssClass="lblsmallFont">
                </dx:ASPxLabel>
            </td>
            <td>
                <dx:ASPxTextBox ID="ScriptCommandTextBox" runat="server" Width="280px">
                    <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorText="">
                        <RequiredField ErrorText="Enter Command" IsRequired="True" />
                    </ValidationSettings>
                </dx:ASPxTextBox>
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                <dx:ASPxLabel ID="ASPxLabel3" runat="server" Text="Upload Script:" 
                    CssClass="lblsmallFont" Wrap="False">
                </dx:ASPxLabel>
            </td>
            <td>
                <dx:ASPxUploadControl ID="ScriptUploadControl" runat="server" 
                    OnFileUploadComplete="ScriptUploadControl_FileUploadComplete" 
                    Width="280px" ClientInstanceName="uploader">
                    <ClientSideEvents FilesUploadComplete="function(s, e) { Uploader_OnFilesUploadComplete(e); }" FileUploadStart="function(s, e) { Uploader_OnUploadStart(); }"
                            TextChanged="function(s, e) { UpdateUploadButton(); }"></ClientSideEvents>
                </dx:ASPxUploadControl>
            </td>
            <td>
                <dx:ASPxButton ID="ASPxButton1" runat="server" Text="Upload" CssClass="sysButton" 
                    ClientInstanceName="ASPxButton1" 
                    ClientEnabled="False">
                </dx:ASPxButton>
            </td>
        </tr>
        <tr>
            <td>
                <dx:ASPxLabel ID="UploadLocationDispLabel" runat="server" 
                    CssClass="lblsmallFont" Text="Upload Location:" Visible="False">
                </dx:ASPxLabel>
            </td>
            <td>
                <dx:ASPxLabel ID="UploadLocationLabel" runat="server" CssClass="lblsmallFont" 
                    Visible="False">
                </dx:ASPxLabel>
            </td>
            <td>
                &nbsp;</td>
        </tr>
    </table>
                    </dx:PanelContent>
</PanelCollection>
            </dx:ASPxRoundPanel>
        </td>
    </tr>
    <tr>
        <td>
            <div id="successDiv" class="alert alert-success" runat="server" style="display: none">File uploaded successfully.</div>
        </td>
    </tr>
    <tr>
        <td>
            <div id="errorDiv" runat="server" class="alert alert-danger" style="display: none">Error
            </div>
            <table>
                <tr>
                    <td>
                        <dx:ASPxButton ID="OKButton" runat="server" Text="OK" onclick="OKButton_Click" 
                            CssClass="sysButton">
                        </dx:ASPxButton>
                    </td>
                    <td>
                        <dx:ASPxButton ID="CancelButton" runat="server" Text="Cancel" CssClass="sysButton"
                            onclick="CancelButton_Click" CausesValidation ="false">
                        </dx:ASPxButton>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
</asp:Content>