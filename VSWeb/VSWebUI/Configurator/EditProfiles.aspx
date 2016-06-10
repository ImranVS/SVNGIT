<%@ Page Title="VitalSigns Plus-User Preferences" Language="C#" MasterPageFile="~/Site1.Master"
	AutoEventWireup="true" CodeBehind="EditProfiles.aspx.cs" Inherits="VSWebUI.EditProfiles" %>

<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.Web" TagPrefix="dx" %>









<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.Web" TagPrefix="dx1" %>

<%@ Register Assembly="DevExpress.Web.ASPxScheduler.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.Web.ASPxScheduler" TagPrefix="dxwschs" %>
<%@ Register Assembly="DevExpress.XtraScheduler.v14.2.Core, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.XtraScheduler" TagPrefix="cc1" %>
	
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
		function OnItemClick(s, e) {
			if (e.item.parent == s.GetRootItem())
				e.processOnServer = false;
		}
	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
	<table>
		<tr>
			<td>
				<div class="header" id="reglabel" runat="server"></div>
			</td>
			<td>
                <dx:ASPxLabel ID="lblprofilename" runat="server" Text="ASPxLabel" Visible="false">
                </dx:ASPxLabel>
			</td>
		</tr>
		<tr>
			<td style="color: Black" id="tdmsg" runat="server" align="center">
			</td>
		</tr>
	</table>
	<div id="errorDiv" class="alert alert-danger" runat="server" style="display: none">
		Error.
	</div>
	<div id="successDiv" class="success" runat="server" style="display: none">
		Error.
	</div>
	<table>
		<tr>
			<td>
				<asp:Label ID="Label2" runat="server" Text="Select Server Type:" CssClass="lblsmallFont"></asp:Label>
			</td>
			<td>
				<dx:ASPxComboBox ID="ServerTypeComboBox" runat="server" ClientInstanceName="cmbServerType"
					ValueType="System.String" AutoPostBack="True" OnSelectedIndexChanged="ServerTypeComboBox_SelectedIndexChanged">
					<ValidationSettings ErrorDisplayMode="ImageWithTooltip">
						<RequiredField ErrorText="Select server type." IsRequired="True" />
						<RequiredField IsRequired="True" ErrorText="Select server type."></RequiredField>
					</ValidationSettings>
				</dx:ASPxComboBox>
			</td>
		</tr>
	</table>
	<table>
	
		<dx:ASPxGridView ID="ProfilesGridView" runat="server" KeyFieldName="ID" AutoGenerateColumns="False"
			EnableTheming="True" Theme="Office2003Blue" Width="100%" OnPageSizeChanged="ProfilesGridView_OnPageSizeChanged" OnPreRender="ProfilesGridView_PreRender">
			<Columns>
			<dx:GridViewDataTextColumn Caption="isSelected" FieldName="isSelected"
										VisibleIndex="4" Visible="false">
										<Settings AutoFilterCondition="Contains" />
										<Settings AutoFilterCondition="Contains"></Settings>
										<EditCellStyle CssClass="GridCss">
										</EditCellStyle>
										<EditFormCaptionStyle CssClass="GridCss" HorizontalAlign="Left">
										</EditFormCaptionStyle>
										<HeaderStyle CssClass="GridCssHeader" />
										<CellStyle CssClass="GridCss">
										</CellStyle>
									</dx:GridViewDataTextColumn>
				
									<dx:GridViewCommandColumn Caption="Enable" ShowSelectCheckbox="True" VisibleIndex="0">
										<ClearFilterButton Visible="True">
										</ClearFilterButton>
										<HeaderStyle CssClass="GridCssHeader1" />
									</dx:GridViewCommandColumn>
				<dx:GridViewDataColumn Caption="Attribute Name" FieldName="AttributeName" VisibleIndex="1">
					<DataItemTemplate>
						<dx:ASPxLabel ID="lblAttribute" runat="server" Value='<%# Eval("AttributeName") %>'>
						</dx:ASPxLabel>
					</DataItemTemplate>
					<HeaderStyle CssClass="GridCssHeader" />
					<CellStyle CssClass="GridCss">
					</CellStyle>
				</dx:GridViewDataColumn>
				<dx:GridViewDataTextColumn Caption="Default Value" FieldName="DefaultValue" VisibleIndex="2">
					<DataItemTemplate>
						<dx:ASPxTextBox ID="txtDefaultValue" runat="server" Width="170px" Value='<%# Eval("DefaultValue") %>'>
						</dx:ASPxTextBox>
					</DataItemTemplate>
					<HeaderStyle CssClass="GridCssHeader" />
					<CellStyle CssClass="GridCss">
					</CellStyle>
				</dx:GridViewDataTextColumn>
				<dx:GridViewDataColumn Caption="Unit of Measurement" FieldName="UnitOfMeasurement"
					VisibleIndex="3">
					<DataItemTemplate>
						<dx:ASPxLabel ID="lblUnitOfMeasurement" runat="server" Value='<%# Eval("UnitOfMeasurement") %>'>
						</dx:ASPxLabel>
					</DataItemTemplate>
					<HeaderStyle CssClass="GridCssHeader" />
					<CellStyle CssClass="GridCss">
					</CellStyle>
				</dx:GridViewDataColumn>
				<dx:GridViewDataColumn Caption="RelatedTable" FieldName="RelatedTable" Visible="False"
					VisibleIndex="7">
					<DataItemTemplate>
						<dx:ASPxLabel ID="lblRelatedTable" runat="server" Value='<%# Eval("RelatedTable") %>'>
						</dx:ASPxLabel>
					</DataItemTemplate>
					<HeaderStyle CssClass="GridCssHeader" />
					<CellStyle CssClass="GridCss">
					</CellStyle>
				</dx:GridViewDataColumn>
				<dx:GridViewDataColumn Caption="RelatedField" FieldName="RelatedField" Visible="False"
					VisibleIndex="6">
					<DataItemTemplate>
						<dx:ASPxLabel ID="tlblRelatedField" runat="server" Value='<%# Eval("RelatedField") %>'>
						</dx:ASPxLabel>
					</DataItemTemplate>
					<HeaderStyle CssClass="GridCssHeader" />
					<CellStyle CssClass="GridCss">
					</CellStyle>
				</dx:GridViewDataColumn>
					<dx:GridViewDataTextColumn Caption="isChecked" VisibleIndex="1" FieldName="isChecked" Visible="false">
															<HeaderStyle CssClass="GridCssHeader" />
															<Settings AllowAutoFilter="False" />
															<CellStyle CssClass="GridCss1" HorizontalAlign="Center">
															</CellStyle>
														</dx:GridViewDataTextColumn>
				<dx:GridViewDataTextColumn Caption="ID" FieldName="ID" Visible="False" VisibleIndex="5">
				</dx:GridViewDataTextColumn>
				
			</Columns>
			<SettingsPager>
				<PageSizeItemSettings Visible="True">
				</PageSizeItemSettings>
			</SettingsPager>
			<Styles>
				<AlternatingRow CssClass="GridAltRow">
				</AlternatingRow>
			</Styles>
		</dx:ASPxGridView>
		<tr>
			<td>
				<div id="emptyDiv3" runat="server" style="display: none">
					&nbsp;</div>
				<div id="Div3" runat="server" class="alert alert-success" style="display: none">
					Settings for selected servers were successully updated.
				</div>
			</td>
		</tr>
		<tr>
			<td>
				<div id="prfilealert" class="alert alert-danger" runat="server" style="display: none">
					Please select at least one attribute.
				</div>
				<div id="prfilealert1" class="alert alert-danger" runat="server" style="display: none">
					Settings for selected attribute(s) were NOT updated.
				</div>
				<dx:ASPxButton ID="ApplyServersButton" runat="server" Text="Save" CssClass="sysButton" Visible="false"
					OnClick="ApplyServersButton_Click">
				</dx:ASPxButton>
				<dx:ASPxButton ID="canclbuttn" runat="server" Text="Cancel" Visible="false" CssClass="sysButton"
					OnClick="CancelButton_Click">
				</dx:ASPxButton>
			</td>
		</tr>
	</table>
</asp:Content>
