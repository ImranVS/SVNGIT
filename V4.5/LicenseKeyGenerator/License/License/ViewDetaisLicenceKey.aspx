<%@ Page Title="VitalSigns Plus - View Details LicenseKey" Language="C#" MasterPageFile="~/Site1.Master"
	AutoEventWireup="true" CodeBehind="ViewDetaisLicenceKey.aspx.cs" Inherits="License.ViewDetaisLicenceKey" %>

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
			$('.alert-danger').delay(10000).fadeOut("slow", function () {
			});
		});
		function HidePopup() {
			var popup = document.getElementById('ContentPlaceHolder1_pnlAreaDtls');
			popup.style.visibility = 'hidden';
		}

		var visibleIndex;
		function OnCustomButtonClick(s, e) {
			visibleIndex = e.visibleIndex;

			if (e.buttonID == "deleteButton")
				UsersGridView.GetRowValues(e.visibleIndex, 'FullName', OnGetRowValues);

			function OnGetRowValues(values) {
				var id = values[0];
				var name = values[1];
				var OK = (confirm('Are you sure you want to delete the  FullName - ' + values + '?'))
				if (OK == true) {
					UsersGridView.DeleteRow(visibleIndex);
									}

				else {
				}

			}
		}
	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
	<table>
	<tr>
			<td>
				<div class="header" id="lblServer" runat="server">
					View License Key Details</div>
			</td>
			</tr>
			</table>
			<table>
			
		
<tr>
<td>
<dx:ASPxLabel ID="userLabel" runat="server" CssClass="lblsmallFont" 
   Text="Users:">
   </dx:ASPxLabel>
</td>
<td>
 <dx:ASPxComboBox ID="UsersComboBox" runat="server" AutoPostBack="true" OnSelectedIndexChanged="UsersComboBox_SelectedIndexChanged"  >
 <ValidationSettings ErrorDisplayMode="ImageWithTooltip" SetFocusOnError="True" 
  ErrorText="Users may not be empty.">
  <RequiredField IsRequired="True" ErrorText="" />
 </ValidationSettings>
 </dx:ASPxComboBox>
</td>
</tr>
</table>
<table>
<tr>
<td>
	<dx:ASPxGridView runat="server" KeyFieldName="ID" Width="95%" AutoGenerateColumns="False" ID="LicenceUsersGridView"
				Theme="Office2003Blue" >
		<%--<ClientSideEvents CustomButtonClick="OnCustomButtonClick" />--%>
		<Columns>
		<dx:GridViewDataTextColumn FieldName="ID" ReadOnly="True" Visible="False" 
											VisibleIndex="1">
												<EditFormSettings Visible="False"></EditFormSettings>
											</dx:GridViewDataTextColumn>
											<dx:GridViewDataTextColumn FieldName="LicenseKey" ReadOnly="True"
											VisibleIndex="7" Width=500px>
											<Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
												<EditFormSettings Visible="False"></EditFormSettings>
											</dx:GridViewDataTextColumn>
											<dx:GridViewDataTextColumn FieldName="Units" ReadOnly="True"
											VisibleIndex="3" Width=50px>
											<Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
												<EditFormSettings Visible="False"></EditFormSettings>
													</dx:GridViewDataTextColumn>
												<dx:GridViewDataTextColumn FieldName="InstallType" ReadOnly="True"
											VisibleIndex="4">
												<EditFormSettings Visible="False"></EditFormSettings>
													</dx:GridViewDataTextColumn>
												<dx:GridViewDataTextColumn FieldName="CompanyName" ReadOnly="True"
											VisibleIndex="2">
											<Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
												<EditFormSettings Visible="False"></EditFormSettings>
													</dx:GridViewDataTextColumn>
												<dx:GridViewDataTextColumn FieldName="LicenseType" ReadOnly="True"
											VisibleIndex="5">
											<Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
												<EditFormSettings Visible="False"></EditFormSettings>
													</dx:GridViewDataTextColumn>
												<dx:GridViewDataTextColumn FieldName="ExpirationDate"  ReadOnly="True"
											VisibleIndex="6">
											<Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
											<PropertiesTextEdit	DisplayFormatString="MM/dd/yyyy"></PropertiesTextEdit>
												<EditFormSettings Visible="False"></EditFormSettings>
											</dx:GridViewDataTextColumn>
											<dx:GridViewDataTextColumn FieldName="CreatedOn" ReadOnly="True"
											VisibleIndex="8">
											<Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
												<EditFormSettings Visible="False"></EditFormSettings>
											</dx:GridViewDataTextColumn>
		</Columns>
		<Settings ShowFilterRow="True" ShowGroupPanel="True" />
	    <SettingsPager PageSize="10" SEOFriendly="Enabled" >
            <PageSizeItemSettings Visible="true" />
<PageSizeItemSettings Visible="True"></PageSizeItemSettings>
        </SettingsPager>
	</dx:ASPxGridView>

	</td>
</tr>
	</table>
	</asp:Content>
