<%@ Page Title="VitalSigns Plus - Import Microsoft Servers" Language="C#" AutoEventWireup="true" MasterPageFile="~/Site1.Master" CodeBehind="ImportMicrosoftServers.aspx.cs" Inherits="VSWebUI.Security.ImportMicrosoftServers" EnableViewState="true" %>
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
    	});

    	function ServerTypeComboBox_IndexChanged(s, e) {
    		if (ServerTypeComboBoxClient.GetSelectedItem() != null && ServerTypeComboBoxClient.GetSelectedItem().value == 2) {
    			AuthenticationTypeComboBoxClient.SetVisible(true);
    			AuthenticationTypeLabelClient.SetVisible(true);
    		}
    		else {
    			AuthenticationTypeComboBoxClient.SetVisible(false);
    			AuthenticationTypeLabelClient.SetVisible(false);
    		}
    	}
    	
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
                            Width="100%" HeaderText="Specify Microsoft Directory Server">
                            <PanelCollection>
<dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">
    <div id="infoForServers" runat="server" class="info" style="display: block" visible="false">
                </div>
				 <dx:ASPxLoadingPanel ID="LoadingPanel" runat="server" ClientInstanceName="LoadingPanel"
                                        Modal="True" ContainerElementID="ASPxRoundPanel2" Theme="Moderno">
                                    </dx:ASPxLoadingPanel>
    <table>
	<tr>
				<td>
					<dx:ASPxLabel ID="ASPxLabel8" runat="server" Text="Server Type:" 
						CssClass="lblsmallFont">
					</dx:ASPxLabel>
				</td>
				<td>
					<dx:ASPxComboBox ID="ServerTypeComboBox" runat="server" AutoPostBack="true" 
                        ClientInstanceName="ServerTypeComboBoxClient" 
                        OnSelectedIndexChanged="ServerTypeComboBox_SelectedIndexChanged1">
						<Items>
							<dx:ListEditItem Text="Active Directory" Value="0" />
							<dx:ListEditItem Text="Database Availability Group" Value="1" />
							<dx:ListEditItem Text="Exchange" Value="2" />
							<dx:ListEditItem Text="Skype for Business" Value="3" />
							<dx:ListEditItem Text="SharePoint" Value="4" />
							<dx:ListEditItem Text="Windows" Value="5" />
						</Items>
						<ValidationSettings>
							<RequiredField IsRequired="True"  ErrorText="Please select a Server Type"/>
						</ValidationSettings>
						<ClientSideEvents 
							SelectedIndexChanged="function(s, e) { ServerTypeComboBox_IndexChanged(s,e); }"
							Init="function(s, e) { ServerTypeComboBox_IndexChanged(s,e); }"
						/>
						
					
					</dx:ASPxComboBox>
				</td>
				<td>
					<dx:ASPxLabel ID="AuthenticationTypeLabel" runat="server" Text="Authentication Type:" ClientInstanceName="AuthenticationTypeLabelClient" ClientVisible="false"
						CssClass="lblsmallFont">
					</dx:ASPxLabel>
				</td>
				<td>
					<dx:ASPxComboBox ID="AuthenticationTypeComboBox" runat="server" AutoPostBack="false" ClientInstanceName="AuthenticationTypeComboBoxClient" ClientVisible="false" >
						<Items>
							<dx:ListEditItem Text="Default" Value="0" Selected="true" />
							<dx:ListEditItem Text="Kerberos" Value="1" />
						</Items>
						<%-- <ClientSideEvents Init="function(s, e) { ServerTypeComboBox_IndexChanged(s,e); }" /> --%>
					</dx:ASPxComboBox>
				</td>
			</tr>
        <tr>
            <td>
                <dx:ASPxLabel ID="ASPxLabel3" runat="server" Text="Server Address:" 
                    CssClass="lblsmallFont">
                </dx:ASPxLabel>
            </td>
            <td>
                <dx:ASPxTextBox ID="ExchangeServerTextBox" runat="server" Width="220px">
                    <ValidationSettings ErrorDisplayMode="ImageWithTooltip" SetFocusOnError="True">
                        <RequiredField ErrorText="Please enter server Address" IsRequired="True" />
                    </ValidationSettings>
                </dx:ASPxTextBox>
            </td>
			
			</tr>
			<tr><td></td></tr>
			<tr><td></td></tr>
		     <tr>
				<td>
                    <dx:ASPxLabel ID="ASPxLabel12" runat="server" Text="Credentials:" 
                                                                    CssClass="lblsmallFont">
                                                                </dx:ASPxLabel>
                </td>
                <td>
                    <dx:ASPxComboBox ID="CredentialsComboBox" runat="server" AutoPostBack="true"  OnSelectedIndexChanged="CredentialsComboBox_SelectedIndexChanged"></dx:ASPxComboBox>
                </td>
			</tr>
			
			<tr>
			<td></td>
			<td></td>
			</tr>
			<tr>
                <td>
                    <dx:ASPxLabel ID="ASPxLabel7" runat="server" CssClass="lblsmallFont" Text="OR">
                    </dx:ASPxLabel>
                </td>
                <td>
                </td>
        </tr>
        <tr>
            <td>
                </td>
            <td>
                </td>
        </tr>
			<tr>
			<td>
                <dx:ASPxLabel ID="ASPxLabel4" runat="server" Text="User ID:" 
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
                <dx:ASPxLabel ID="ASPxLabel5" runat="server" Text="Password:" 
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
			<td colspan="1">
			
                <dx:ASPxButton ID="LoadServersButton" runat="server" CssClass="sysButton"   ClientInstanceName="LoadServersButton" ClientEnabled="true"
                    OnClick="LoadServersButton_Click" Text="Load Servers"   CausesValidation="true" 
                    Wrap="False">
					 <ClientSideEvents Click="function(s, e) {
						if(ServerTypeComboBoxClient.GetText() != '')
                                            LoadingPanel.Show();
                                        }" />
                </dx:ASPxButton>
            </td>
			<td>
			
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
                        <%--<ValidationSettings ErrorDisplayMode="Text" SetFocusOnError="True">
                        <RequiredField ErrorText="Please enter User Id" IsRequired="True" />
                    </ValidationSettings>--%>
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
<dx:PanelContent ID="PanelContent2" runat="server" SupportsDisabledAttribute="True">
<table align="left">
                    <tr>
                        <td>
                            <dx:ASPxLabel ID="ASPxLabel1" runat="server" CssClass="lblsmallFont" 
                                Text="Select Location:" Wrap="False">
                            </dx:ASPxLabel>
                        </td>
                        <td>
                            <dx:ASPxComboBox ID="LocComboBox" runat="server">
							 <ValidationSettings ErrorDisplayMode= "ImageWithTooltip"  SetFocusOnError="True" 
                                    ErrorText="">
                        <RequiredField ErrorText="Please enter location"   IsRequired="True" />
                    </ValidationSettings>
                            </dx:ASPxComboBox>
                            <dx:ASPxComboBox ID="LocIDComboBox" runat="server" Visible="False">
                            </dx:ASPxComboBox>
                        </td>

						<td>
						<dx:ASPxCheckBox ID="SSLCheckBox" runat="server" CheckState="checked" 
                                            Text="Requires SSL" Visible="False">
                                        </dx:ASPxCheckBox>
						</td>
				        <td></td>
						<td>
                            <dx:ASPxLabel ID="ASPxLabel10" runat="server" Text="Select Profile:" 
                                CssClass="lblsmallFont" Wrap="False">
                            </dx:ASPxLabel>
						</td>
						<td>
						    <dx:ASPxComboBox ID="ProfileComboBox" runat="server" ClientInstanceName="cmbLocation" 
                                                    ValueType="System.String" AutoPostBack="True"  SelectedIndex="0">
                                                   
                                                    <ValidationSettings ErrorDisplayMode="ImageWithTooltip">
                                                    <RequiredField ErrorText="Please select profile" IsRequired="True" />
                                                    <RequiredField IsRequired="True" ErrorText="Select Profule."></RequiredField>
                                                    </ValidationSettings>                                                    
                                                </dx:ASPxComboBox>
						</td>
                        <td>
                            &nbsp;</td>
                        <td>
                            <dx:ASPxLabel ID="ASPxLabel13" runat="server" CssClass="lblsmallFont" 
                                Text="Requires SSL:">
                            </dx:ASPxLabel>
                        </td>
                        <td>
                            <dx:ASPxRadioButtonList ID="RequireSSLRadioButtonList" runat="server" 
                                RepeatDirection="Horizontal">
                                <Items>
                                    <dx:ListEditItem Text="Yes" Value="1" />
                                    <dx:ListEditItem Text="No" Value="0" />
                                </Items>
                                <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorText="">
                                    <RequiredField ErrorText=" Please select Yes or No" IsRequired="True" />
                                </ValidationSettings>
                            </dx:ASPxRadioButtonList>
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
                            <dx:ASPxButton ID="SelectAllButton" runat="server" CssClass="sysButton"
                    OnClick="SelectAllButton_Click" Text="Select All" CausesValidation="false">
                </dx:ASPxButton>
                        </td>
                        <td>
                            <dx:ASPxButton ID="DeselectAllButton" runat="server" Text="Deselect All" CssClass="sysButton"
                    OnClick="DeselectAllButton_Click" CausesValidation="false">
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
                <dx:ASPxButton ID="ImportButton" runat="server" Text="Next" CssClass="sysButton"
                                OnClick="ImportButton_Click">
                            </dx:ASPxButton>
				 <dx:ASPxButton ID="ASPxButton1" runat="server" Text="Save Credentials and Move Next" CssClass="sysButton"
                                 Wrap="False"
					OnClick="CreateCredentialsImportButton_Click">
                            </dx:ASPxButton>
							<dx:ASPxLabel ID="ASPxLabel6" runat="server" 
                    Text="(Credential Name will be Saved as:  '<Server Type>-<Selected Location Name>' )" CssClass="lblsmallFont">
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