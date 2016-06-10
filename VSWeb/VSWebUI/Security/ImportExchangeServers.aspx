<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site1.Master" CodeBehind="ImportExchangeServers.aspx.cs" Inherits="VSWebUI.Security.ImportExchangeServers" EnableViewState="true" %>
<%@ MasterType virtualpath="~/Site1.Master" %>
<%@ Register assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>




<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<link href='http://fonts.googleapis.com/css?family=Francois One' rel='stylesheet' type='text/css' />
<link rel="stylesheet" type="text/css" href="../css/vswebforms.css" />
<script src="../js/jquery-1.9.1.js" type="text/javascript"></script>
    <script src="../js/bootstrap.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('.alert-success').delay(10000).fadeOut("slow", function () {
            });
            $('.alert-danger').delay(10000).fadeOut("slow", function () {
            });
        });
        </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table width="90%">
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
                    <td style="color:Black" id="tdmsg1" runat="server" align="left">
                        <dx:ASPxRoundPanel ID="ASPxRoundPanel2" runat="server" Theme="Glass" 
                            Width="100%" HeaderText="Specify Exchange Directory Server">
                            <PanelCollection>
<dx:PanelContent runat="server" SupportsDisabledAttribute="True">
    <table>
        <tr>
            <td>
                <dx:ASPxLabel ID="ASPxLabel3" runat="server" Text="Exchange Server Address:" 
                    CssClass="lblsmallFont">
                </dx:ASPxLabel>
            </td>
            <td>
                <dx:ASPxTextBox ID="ExchangeServerTextBox" runat="server" Width="220px">
                    <ValidationSettings ErrorDisplayMode="Text" SetFocusOnError="True">
                        <RequiredField ErrorText="Please enter server Address" IsRequired="True" />
                    </ValidationSettings>
                </dx:ASPxTextBox>
            </td>
			</tr>
			<tr>
				<td>
                    <dx:ASPxLabel ID="ASPxLabel12" runat="server" Text="Select Credentials:" 
                                                                    CssClass="lblsmallFont">
                                                                </dx:ASPxLabel>
                </td>
                <td>
                    <dx:ASPxComboBox ID="CredentialsComboBox" runat="server" AutoPostBack="true"  OnSelectedIndexChanged="CredentialsComboBox_SelectedIndexChanged"></dx:ASPxComboBox>
                </td>
			</tr>
			
			<tr>
			<td></td>
			<td><dx:ASPxLabel ID="ASPxLabel7" runat="server" Text="OR" 
                    CssClass="lblsmallFont">
                </dx:ASPxLabel></td>
			</tr>
			<tr>
			<td>
                <dx:ASPxLabel ID="ASPxLabel4" runat="server" Text="Enter User Id:" 
                    CssClass="lblsmallFont">
                </dx:ASPxLabel>
            </td>
            <td>
                <dx:ASPxTextBox ID="UserIdtextBox" runat="server" Width="220px">
                    <%--<ValidationSettings ErrorDisplayMode="Text" SetFocusOnError="True">
                        <RequiredField ErrorText="Please enter User Id" IsRequired="True" />
                    </ValidationSettings>--%>
                </dx:ASPxTextBox>
            </td>
			</tr>
			<tr>
			<td>
                <dx:ASPxLabel ID="ASPxLabel5" runat="server" Text="Enter Password:" 
                    CssClass="lblsmallFont">
                </dx:ASPxLabel>
            </td>
            <td>
                <dx:ASPxTextBox ID="PasswordTextbox" runat="server" Width="220px" Password="True">
                    <%--<ValidationSettings ErrorDisplayMode="Text" SetFocusOnError="True">
                        <RequiredField ErrorText="Please enter Password" IsRequired="True" />
                    </ValidationSettings>--%>
                </dx:ASPxTextBox>
            </td>
			
			</tr>
			<tr>
			
            <td>
                <dx:ASPxButton ID="LoadServersButton" runat="server" Text="Load Exchange Servers" 
                    Theme="Office2010Blue" Wrap="False" OnClick="LoadServersButton_Click" Width="150px">
                </dx:ASPxButton>
            </td>
			<td>
                <dx:ASPxButton ID="ASPxButton2" runat="server" Text="Load DAG Servers" 
                    Theme="Office2010Blue" Wrap="False" OnClick="LoadDAGButton_Click" Width="150px">
                </dx:ASPxButton>
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
                        <div id="errorDiv" runat="server" class="alert alert-danger" style="display: none">Error:
                    </div>
                    <div id="errorinfoDiv" runat="server" class="info" style="display: none">
                        </div>
                    <div id="infoDiv" runat="server" class="info" style="display: none">Any server not on the list has already been imported.
                    </div>
                    </td>
                </tr>
                <tr>
                    <td style="color:Black" id="tdmsg" runat="server" align="left">
                        <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" Width="100%" 
                            HeaderText="Step 1 - Import Servers and Assign Locations" Theme="Glass" 
                            Visible="False">
                            <PanelCollection>
<dx:PanelContent runat="server" SupportsDisabledAttribute="True">
<table align="left">
                    <tr>
                        <td>
                            <dx:ASPxLabel ID="ASPxLabel1" runat="server" CssClass="lblsmallFont" 
                                Text="Select Location:">
                            </dx:ASPxLabel>
                        </td>
                        <td>
                            <dx:ASPxComboBox ID="LocComboBox" runat="server">
                            </dx:ASPxComboBox>
                        </td>

						<td>
						<dx:ASPxCheckBox ID="SSLCheckBox" runat="server" CheckState="checked" 
                                            Text="Requires SSL">
                                        </dx:ASPxCheckBox>
						</td>
						<td></td>
						<td>
						   <asp:Label ID="Label2" runat="server" Text="Select Profile:" CssClass="lblsmallFont"></asp:Label>
						</td>
						<td>
						    <dx:ASPxComboBox ID="ProfileComboBox" runat="server" ClientInstanceName="cmbLocation" 
                                                    ValueType="System.String" AutoPostBack="True">
                                                   
                                                    <ValidationSettings ErrorDisplayMode="ImageWithTooltip">
                                                    <RequiredField ErrorText="Select Profile." IsRequired="True" />
                                                    <RequiredField IsRequired="True" ErrorText="Select Profule."></RequiredField>
                                                    </ValidationSettings>                                                    
                                                </dx:ASPxComboBox>
						</td>
                    </tr>
                    <tr>
                        <td>
                            <dx:ASPxComboBox ID="LocIDComboBox" runat="server" ValueType="System.String" 
                                Visible="False">
                        </dx:ASPxComboBox>   
                        </td>
                    </tr>
                </table>
                <br />
    <table align="left">
        <tr>
            <td>
                <dx:ASPxLabel ID="ASPxLabel2" runat="server" 
                    Text="Select servers for the above Location:" CssClass="lblsmallFont">
                </dx:ASPxLabel>
            </td>
            <td colspan="2">
                <table>
                    <tr>
                        <td>
                            <dx:ASPxButton ID="SelectAllButton" runat="server" 
                    OnClick="SelectAllButton_Click" Text="Select All" Theme="Office2010Blue">
                </dx:ASPxButton>
                        </td>
                        <td>
                            <dx:ASPxButton ID="DeselectAllButton" runat="server" Text="Deselect All" 
                    Theme="Office2010Blue" OnClick="DeselectAllButton_Click">
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
                                Theme="Office2010Blue" Width="60px" OnClick="ImportButton_Click">
                            </dx:ASPxButton>
				 <dx:ASPxButton ID="ASPxButton1" runat="server" Text="Save Credentials and Move Next" 
                                Theme="Office2010Blue" Width="260px" OnClick="CreateCredentialsImportButton_Click">
                            </dx:ASPxButton>
							<dx:ASPxLabel ID="ASPxLabel6" runat="server" 
                    Text="(Credential Name will be Saved as:  'Exchange-<Selected Location Name>' )" CssClass="lblsmallFont">
                </dx:ASPxLabel>
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