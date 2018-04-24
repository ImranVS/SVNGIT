<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="LocationsEdit.aspx.cs" Inherits="VSWebUI.Security.LocationsEdit" %>
<%@ MasterType virtualpath="~/Site1.Master" %>
<%@ Register assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>




<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script src="../js/jquery-1.9.1.js" type="text/javascript"></script>
    <script src="../js/bootstrap.min.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class="header" id="servernamelbldisp" runat="server">Location</div>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
	<ContentTemplate>
		<dx:ASPxRoundPanel ID="ASPxRoundPanel13" runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
			CssPostfix="Glass" GroupBoxCaptionOffsetY="-24px" HeaderText="Location Info" 
            SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css">
			<PanelCollection>
				<dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">
					<table>
						<tr>
							<td>
								<dx:ASPxLabel ID="ASPxLabel4" runat="server" Text="Location Name:" 
                                    CssClass="lblsmallFont" />
							</td>
							<td>
								<dx:ASPxTextBox ID="LocationTextBox" runat="server">
							      <ValidationSettings CausesValidation="true" SetFocusOnError="True">
                                  <RequiredField ErrorText="Please Enter Location" IsRequired="True" />
								   <RegularExpression ValidationExpression="^\d*[a-zA-Z ][a-zA-Z0-9 ]*$" ErrorText="Please Enter Location" />
                                  </ValidationSettings>
								  </dx:ASPxTextBox>
							</td>
						</tr>
						<tr>
							<td>
								<dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="Country:" 
                                    CssClass="lblsmallFont" />
							</td>
							<td>
								<dx:ASPxComboBox runat="server" ID="CountryCombobox" AutoPostBack="true" OnSelectedIndexChanged="CountryCombobox_SelectedIndexChanged" DropDownStyle="DropDown" IncrementalFilteringMode="StartsWith" />

							</td>
						</tr>
						<tr>
							<td>
								<dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="State/Region:" 
                                    CssClass="lblsmallFont" />
							</td>
							<td>
								<dx:ASPxComboBox runat="server" ID="StateCombobox" OnSelectedIndexChanged="StateCombobox_SelectedIndexChanged" AutoPostBack="true"  DropDownStyle="DropDown" IncrementalFilteringMode="StartsWith" />

							</td>
						</tr>
						<tr>
							<td>
								<dx:ASPxLabel ID="ASPxLabel3" runat="server" Text="City:" 
                                    CssClass="lblsmallFont" />
							</td>
							<td>
								<dx:ASPxComboBox runat="server" ID="CityCombobox"  DropDownStyle="DropDown" IncrementalFilteringMode="Contains" />
								 <%-- <dx:ASPxTextBox ID="Citytextbox" runat="server"/>--%>

							</td>
						</tr>
					</table>
				</dx:PanelContent>
			</PanelCollection>
		</dx:ASPxRoundPanel>
        <table>
            <tr>
							<td>
								<div id="errorDiv" class="alert alert-danger" runat="server" style="display: none" />
							</td>
						</tr>
            <tr>
							<td align="center">
								<dx:ASPxButton ID="ValidationOkButton" runat="server" AutoPostBack="False"
									Text="OK" CssClass="sysButton" OnClick="ValidationUpdatedButton_Click">
								</dx:ASPxButton>
								<dx:ASPxButton ID="ValidationUpdatedButton" runat="server" AutoPostBack="False" OnClick="CancelButton_Click"
									Text="Cancel" CssClass="sysButton" CausesValidation ="false">
								</dx:ASPxButton>
							</td>
						</tr>
        </table>
	</ContentTemplate>
</asp:UpdatePanel>
<%--<dx:ASPxPopupControl ID="ErrorMessagePopupControl" runat="server" AllowDragging="True"
		ClientInstanceName="pcErrorMessage" CloseAction="CloseButton" 
		EnableAnimation="False" EnableViewState="False" HeaderText="Validation Failure"
		Height="150px" Modal="True" PopupHorizontalAlign="WindowCenter" 
		PopupVerticalAlign="WindowCenter" Width="300px" Theme="Glass">
		<ContentStyle BackColor="White">
		</ContentStyle>
		<HeaderStyle BackColor="#CCCCCC">
			<Paddings PaddingLeft="10px" PaddingRight="6px" PaddingTop="1px" />
			<Paddings PaddingLeft="10px" PaddingTop="1px" PaddingRight="6px"></Paddings>
		</HeaderStyle>
		<ContentCollection>
			<dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server" SupportsDisabledAttribute="True">
				<dx:ASPxPanel ID="Panel1" runat="server" DefaultButton="btOK">
					<PanelCollection>
						<dx:PanelContent ID="PanelContent4" runat="server" SupportsDisabledAttribute="True">
							<div style="min-height: 70px;">
								&nbsp;&nbsp;&nbsp;   <dx:ASPxLabel ID="ErrorMessageLabel" runat="server" Text="msglabel">
								</dx:ASPxLabel>
							</div>
							<div>
								<table cellpadding="0" cellspacing="0" width="100%">
									<tr>
										<td align="center">
											<dx:ASPxButton ID="ValidationOkButton" runat="server" AutoPostBack="False"
												Text="OK" Width="80px" Theme="Office2010Blue" OnClick="ValidationUpdatedButton_Click">
											</dx:ASPxButton>
											<dx:ASPxButton ID="ValidationUpdatedButton" runat="server" AutoPostBack="False" OnClick="CancelButton_Click"
												Text="Cancel" Visible="False" Width="80px" Theme="Office2010Blue">
											</dx:ASPxButton>
										</td>
									</tr>
								</table>
							</div>
						</dx:PanelContent>
					</PanelCollection>
				</dx:ASPxPanel>
			</dx:PopupControlContentControl>
		</ContentCollection>
	</dx:ASPxPopupControl>--%>

</asp:Content>
