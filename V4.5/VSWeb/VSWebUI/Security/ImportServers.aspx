<%@ Page Title="VitalSigns Plus - Import Servers" Language="C#" AutoEventWireup="true" MasterPageFile="~/Site1.Master" CodeBehind="ImportServers.aspx.cs"
    Inherits="VSWebUI.Security.ImportServers" %>
<%@ MasterType virtualpath="~/Site1.Master" %>
<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>




<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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
    <script type="text/javascript">
    // <![CDATA[
        function Uploader_OnUploadStart() {
            ASPxButton1.SetEnabled(false);
        }
        function Uploader_OnFileUploadComplete(args) {
            var imgSrc = aspxPreviewImgSrc;
            if (args.isValid) {
                var date = new Date();
                imgSrc = "log_files/" + args.callbackData + "?dx=" + date.getTime();
            }
            getPreviewImageElement().src = imgSrc;
        }
        function Uploader_OnFilesUploadComplete(args) {
            UpdateUploadButton();
        }
        function UpdateUploadButton() {
            ASPxButton1.SetEnabled(uploader.GetText(0) != "");
        }
        function getPreviewImageElement() {
            return document.getElementById("ASPxLabel6");
        }


    // ]]> 
    </script>
    <%--<script type ="text/javascript">

    var validFilesTypes = ["csv"];

    function ValidateFile() {

        var file = document.getElementById("<%=fileupld.ClientID%>");

        var label = document.getElementById("<%=ASPxLabel6.ClientID%>");

        var path = file.value;

        var ext = path.substring(path.lastIndexOf(".") + 1, path.length).toLowerCase();

        var isValidFile = false;

        for (var i = 0; i < validFilesTypes.length; i++) {

            if (ext == validFilesTypes[i]) {

                isValidFile = true;

                break;

            }

        }

        if (!isValidFile) {

            label.style.color = "red";

            label.innerHTML = "Invalid File.Please upload a File with" +

         " extension:\n\n" + validFilesTypes.join(", ");

        }

        return isValidFile;

    }

</script>--%>
  <%--<script type="text/javascript">
    // <![CDATA[
      function Uploader_OnUploadStart() {
          ASPxButton1.SetEnabled(false);
      }
      function Uploader_OnFileUploadComplete(args) {
          var imgSrc = aspxPreviewImgSrc;
          if (args.isValid) {
              var date = new Date();
              imgSrc = "Log_Files/" + args.callbackData + "?dx=" + date.getTime();
          }
          getPreviewImageElement().src = imgSrc;
      }
      function Uploader_OnFilesUploadComplete(args) {
          UpdateUploadButton();
      }
      function UpdateUploadButton() {
          ASPxButton1.SetEnabled(uploader.GetText(0) != "");
      }
      function getPreviewImageElement() {
          return document.getElementById("previewImage");
      }
    // ]]> 
    </script>--%>

<%--<script type ="text/javascript">

    var validFilesTypes = ["csv"];

    function ValidateFile() {

        var file = document.getElementById("<%=fileupld.ClientID%>");

        var label = document.getElementById("<%=ASPxLabel6.ClientID%>");

        var path = file.value;

        var ext = path.substring(path.lastIndexOf(".") + 1, path.length).toLowerCase();

        var isValidFile = false;

        for (var i = 0; i < validFilesTypes.length; i++) {

            if (ext == validFilesTypes[i]) {

                isValidFile = true;

                break;

            }

        }

        if (!isValidFile) {

            label.style.color = "red";

            label.innerHTML = "Invalid File.Please upload a File with" +

         " extension:\n\n" + validFilesTypes.join(", ");

        }

        return isValidFile;

    }

</script>--%>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table width="90%">
        <tr>
            <td>
                <div class="header" id="servernamelbldisp" runat="server">
                    Import Servers</div>
            </td>
        </tr>
        <tr>
            <td>
    <table align="left">

        <tr>
            <td style="color: Black" id="tdmsg1" runat="server" align="left">
                <dx:ASPxRoundPanel ID="ASPxRoundPanel2" runat="server" Theme="Glass" Width="100%"
                    HeaderText="Specify Domino Directory Server or Load Servers from a CSV File">
                    <PanelCollection>
                        <dx:PanelContent runat="server" SupportsDisabledAttribute="True">
                            <table>
                                <tr>
                                    <td>
                                        <div id="infoDivLoad" class="info">
                                            Enter the Domino Directory Server value and click 'Load Servers' or load a CSV file
                                            by clicking the 'Browse...' button and click 'Load from CSV' to get a list of servers to import.
                                        </div>
										 <dx:ASPxLoadingPanel ID="LoadingPanel" runat="server" ClientInstanceName="LoadingPanel"
                                        Modal="True" ContainerElementID="ASPxRoundPanel2" Theme="Moderno">
                                    </dx:ASPxLoadingPanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <table>
                                            <tr>
                                                <td>
                                                    <dx:ASPxLabel ID="ASPxLabel3" runat="server" Text="Domino Directory Server:" CssClass="lblsmallFont">
                                                    </dx:ASPxLabel>
                                                </td>
                                                <td>
                                                    <dx:ASPxTextBox ID="DomServerTextBox" runat="server" Width="220px">
                                                        <ValidationSettings ErrorDisplayMode="Text" SetFocusOnError="True">
                                                            <RequiredField ErrorText="Please enter server name" IsRequired="True" />
                                                        </ValidationSettings>
                                                    </dx:ASPxTextBox>
                                                </td>
                                                <td>
                                                    <dx:ASPxButton ID="LoadServersButton" runat="server" Text="Load Servers" CssClass="sysButton"  ClientInstanceName="LoadServersButton" ClientEnabled="true"
                                                        Wrap="False" OnClick="LoadServersButton_Click">
														<ClientSideEvents Click="function(s, e) {
														LoadingPanel.Show();
														}" />
                                                    </dx:ASPxButton>
                                                </td>
                                                <td>
                                                    &nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td colspan="3">
                                                    <dx:ASPxLabel ID="ASPxLabel4" runat="server" CssClass="lblsmallFont" Text="OR">
                                                    </dx:ASPxLabel>
                                                </td>
                                                <td>
                                                    &nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td colspan="">
                                                    <dx:ASPxLabel ID="ASPxLabel5" runat="server" CssClass="lblsmallFont" Text="Load from a CSV file ">
                                                    </dx:ASPxLabel>
                                                </td>
                                                <td>
                                                    <dx:ASPxUploadControl ID="fileupld" runat="server" ClientInstanceName="uploader"
                                                        ShowProgressPanel="false" NullText="Click here to browse .csv files..." Size="30"
                                                        OnFileUploadComplete="fileupld_FileUploadComplete" CancelButtonHorizontalPosition="Right"
                                                        ShowUploadButton="false">
                                                        <ClientSideEvents FileUploadComplete="function(s, e) { Uploader_OnFileUploadComplete(e); }"
                                                            FilesUploadComplete="function(s, e) { Uploader_OnFilesUploadComplete(e); }" FileUploadStart="function(s, e) { Uploader_OnUploadStart(); }"
                                                            TextChanged="function(s, e) { UpdateUploadButton(); }"></ClientSideEvents>
                                                        <ValidationSettings MaxFileSize="4194304" AllowedFileExtensions=".csv">
                                                        </ValidationSettings>
                                                        <CancelButton ImagePosition="Right">
                                                        </CancelButton>
                                                    </dx:ASPxUploadControl>
                                                </td>
                                                <td>
                                                    <%-- <asp:Button ID="ASPxButton1" runat="server" OnClick="ASPxButton1_Click" Text="Load from CSV" Theme="Office2010Blue" OnClientClick = "return ValidateFile()" />--%>
                                                    <dx:ASPxButton ID="ASPxButton1" runat="server" AutoPostBack="true" Text="Load from CSV" CssClass="sysButton"
                                                        ClientInstanceName="ASPxButton1" Width="100px" ClientEnabled="False">
                                                    </dx:ASPxButton>
                                                    <%--  <ClientSideEvents Click="function(s, e) { uploader.Upload(); }" /> <dx:ASPxButton ID="btnUpload" runat="server" AutoPostBack="False" Text="Upload" ClientInstanceName="btnUpload"
                                            Width="100px" ClientEnabled="False" style="margin: 0 auto;">
                                            <ClientSideEvents Click="function(s, e) { uploader.Upload(); }" />
                                        </dx:ASPxButton>--%>
                                                </td>
                                                <td>
                                                    <a class="viewby" href="../files/ServerImportSample.csv">&nbsp;(Sample CSV&nbsp;<img alt="" src="../images/icons/xls.png" />)</a>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <dx:ASPxLabel ID="ASPxLabel6" runat="server" CssClass="lblsmallFont" Text="Allowed File Extension Type:"
                                                        Visible="true">
                                                    </dx:ASPxLabel>
                                                </td>
                                                <td>
                                                    <dx:ASPxLabel ID="ASPxLabel8" runat="server" CssClass="lblsmallFont" Text="*.csv">
                                                    </dx:ASPxLabel>
                                                </td>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                <td>
                                                    &nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td colspan="3">
                                                    <dx:ASPxLabel ID="ASPxLabel7" runat="server" CssClass="lblsmallFont" 
                                                        Visible="False">
                                                    </dx:ASPxLabel>
                                                    <div id="successDiv" runat="server" class="alert alert-success" style="display: none">Success</div>
                                                    <div id="errorDiv2" class="alert alert-danger" runat="server" style="display: none">Error</div>
                                                </td>
                                                <td>
                                                    &nbsp;</td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </dx:PanelContent>
                    </PanelCollection>
                </dx:ASPxRoundPanel>
            </td>
        </tr>
        <tr>
            <td>
                <div id="errorDiv" runat="server" class="alert alert-danger" style="display: none">
                    Error:
                </div>
                <div id="errorinfoDiv" runat="server" class="info" style="display: none">
                    Please check the following possible issues to ensure the Import Wizard is able to
                    function correctly:<br />
                    <br />
                    1. VitalSigns cannot access the Notes ID. Check the values in the Path environment
                    variable to make sure it has a reference to the Notes program directory and the
                    Notes.ini location directory.<br />
                    2. A Notes process is running and locking up the Notes ID. Run Kill Notes as an
                    administrator or end all currently running Notes processes.<br />
                    3. The IIS_USRS user does not have access rights to the Notes directory folder (must
                    have at least read access).<br />
                    4. Incorrect value in the KeyFileName parameter in Notes.ini. Make sure the path
                    and file name of the Notes ID file are correct.<br />
                    5. The Notes.ini file specifies an ID file located on a mapped drive, rather than
                    on a local drive. Move the ID to the local drive and correct Notes.ini.<br />
                    6. Incorrectly entered Notes password. Try logging into the Notes client with the
                    same ID and password, make sure you can access the names.nsf DB on the server you
                    are trying to use in the Import Wizard.<br />
                    7. The server connection document for the server above is missing or incorrect.
                    Check the Domino connection documents to ensure the server document exists and is
                    configured correctly.
                </div>
            </td>
        </tr>
        <tr>
            <td style="color: Black" id="tdmsg" runat="server" align="left">
                <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" Width="100%" HeaderText="Step 1 - Import Servers and Assign Locations"
                    Theme="Glass" Visible="false" >
                    <PanelCollection>
                        <dx:PanelContent runat="server" SupportsDisabledAttribute="True">
                            <table align="left">
                                <tr>
                                    <td>
                                        <dx:ASPxLabel ID="ASPxLabel1" runat="server" CssClass="lblsmallFont" Text="Select Location:">
                                        </dx:ASPxLabel>
                                    </td>
                                    <td>
                                        <dx:ASPxComboBox ID="LocComboBox" runat="server" > 
                                        <ValidationSettings ErrorDisplayMode="ImageWithTooltip"  ValidationGroup="Next">
                                                    <RequiredField ErrorText="Select Location" IsRequired="True" />
                                                    <RequiredField IsRequired="True" ErrorText="Select Location"></RequiredField>
                                                    </ValidationSettings>
                                        </dx:ASPxComboBox>
                                    </td>
								 <td></td>
									<td>
                                                <asp:Label ID="Label2" runat="server" Text="Select Profile:" CssClass="lblsmallFont"></asp:Label>
                                            </td>
                                            <td>
                                                <dx:ASPxComboBox ID="ProfileComboBox" runat="server" ClientInstanceName="cmbLocation" 
                                                    ValueType="System.String" AutoPostBack="True" >
                                                    <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ValidationGroup="Next">
                                                    <RequiredField ErrorText="Select Profile" IsRequired="True" />
                                                    <RequiredField IsRequired="True" ErrorText="Select Profile"></RequiredField>
                                                    </ValidationSettings>                                                    
                                                </dx:ASPxComboBox>
                                            </td>
                                </tr>
                                <tr>
                                    <td>
                                        <dx:ASPxComboBox ID="LocIDComboBox" runat="server" ValueType="System.String" Visible="False">
                                        </dx:ASPxComboBox>
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <table align="left">
                                <tr>
                                    <td colspan="3">
                                        <div id="infoDiv" runat="server" class="info" style="display: none">
                    Any server not on the list has already been imported.
                </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="Select servers for the above Location:"
                                            CssClass="lblsmallFont">
                                        </dx:ASPxLabel>
                                    </td>
                                    <td colspan="2">
                                        <table>
                                            <tr>
                                                <td>
                                                    <dx:ASPxButton ID="SelectAllButton" runat="server" OnClick="SelectAllButton_Click"
                                                        Text="Select All" CssClass="sysButton">
                                                    </dx:ASPxButton>
                                                </td>
                                                <td>
                                                    <dx:ASPxButton ID="DeselectAllButton" runat="server" Text="Deselect All"
                                                        OnClick="DeselectAllButton_Click" CssClass="sysButton">
                                                    </dx:ASPxButton>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" colspan="3">
                                        <dx:ASPxCheckBoxList ID="SrvCheckBoxList" runat="server" RepeatColumns="5">
                                        </dx:ASPxCheckBoxList>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" colspan="3">
                                        <dx:ASPxCheckBoxList ID="IPCheckBoxList" runat="server" Visible="False">
                                        </dx:ASPxCheckBoxList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" colspan="3">
                                        <dx:ASPxButton ID="ImportButton" runat="server" Text="Next"
                                            Width="60px" OnClick="ImportButton_Click" CssClass="sysButton" ValidationGroup="Next">
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
    </td>
    </tr>
    </table>

   <%-- <script type="text/javascript">
    // <![CDATA[
        var aspxPreviewImgSrc = getPreviewImageElement().src;
    // ]]> 
    </script>--%>
</asp:Content>

