<%@ Page Title="VitalSigns Plus - Estimate Licenses" Language="C#" MasterPageFile="~/Site1.Master"
	AutoEventWireup="true" CodeBehind="EstimateLicenses.aspx.cs" Inherits="License.EstimateLicenses" %>

<%--	<%@ Register Assembly="DevExpress.Web.ASPxGridView.v11.1.Export, Version=14.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxGridView.Export" TagPrefix="dx" %>--%>
<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.Web" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
	<link href='http://fonts.googleapis.com/css?family=Francois One' rel='stylesheet'
		type='text/css' />
	<link rel="stylesheet" type="text/css" href="../css/vswebforms.css" />
	<script type="text/javascript">



		function multiplication(UnitCost, Noofservers, totalunits, endIndex) {
			
			var noofservers = Noofservers.id;

			var sum;
			var licenesum = 0;
			var totalunitsid = noofservers.replace("3_noofservers", "4_totalunits");
			var totalunitsvalue = document.getElementById(Noofservers.id +'_I').value;
			document.getElementById(totalunitsid + '_I').value = totalunitsvalue * UnitCost;
			for (i = 0; i < endIndex; i++) {

				var total = "ContentPlaceHolder1_EstimateLicensesGrid_cell" + i + "_4_totalunits_" + i + "_I";
				sum = document.getElementById(total).value;
				
				if (sum === "") {
					sum = 0;
					         
				}

				else {
					sum = parseInt(document.getElementById(total).value);
				}
				licenesum = licenesum + sum;
			
			}
			var licenselbl = document.getElementById('ContentPlaceHolder1_licensesum');
			//licenselbl.innerHTML = licenselbl.innerHTML + licenesum;
			licenselbl.innerHTML = "Total License Count: " + licenesum;
			}
	
	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
	<table>
		<tr>
			<td>
				<div class="header" id="EstimateLicensesdiv" runat="server">
					Estimate Licenses
				</div>
			</td>
			
		</tr>
		<tr>
		<td>
				<div class="info" id="licensesum" runat="server" align="left"  style="width:580px">
					Total License Count :
				</div>
			</td>
		</tr>
		<tr>
			<td>
				<dx:ASPxGridView ID="EstimateLicensesGrid" runat="server" AutoGenerateColumns="False"
					KeyFieldName="ID" EnableTheming="True" Theme="Office2003Blue" Width="50%" OnPageSizeChanged="EstimateLicensesGrid_PageSizeChanged" 
					OnHtmlRowCreated="EstimateLicensesGrid_HtmlRowCreated" >
					<Columns>
						<dx:GridViewDataTextColumn Caption="ID" FieldName="ID" Visible="false" VisibleIndex="0">
							<Settings AutoFilterCondition="Contains" />
							<EditCellStyle CssClass="GridCss">
							</EditCellStyle>
							<EditFormCaptionStyle CssClass="GridCss" HorizontalAlign="Left">
							</EditFormCaptionStyle>
							<HeaderStyle CssClass="GridCssHeader" />
							<CellStyle CssClass="GridCss">
							</CellStyle>
						</dx:GridViewDataTextColumn>
						<dx:GridViewDataTextColumn Caption="Server Type" FieldName="ServerType" VisibleIndex="1">
							<Settings AutoFilterCondition="Contains" AllowAutoFilter="False" />
							<EditCellStyle CssClass="GridCss">
							</EditCellStyle>
							<EditFormCaptionStyle CssClass="GridCss" HorizontalAlign="Left">
							</EditFormCaptionStyle>
							<HeaderStyle CssClass="GridCssHeader" />
							<CellStyle CssClass="GridCss">
							</CellStyle>
						</dx:GridViewDataTextColumn>
						<dx:GridViewDataTextColumn FieldName="UnitCost" Caption="UnitCost" VisibleIndex="2">
							<Settings AutoFilterCondition="Contains" AllowAutoFilter="False" />
							<HeaderStyle CssClass="GridCssHeader"></HeaderStyle>
							<CellStyle CssClass="GridCss">
							</CellStyle>
						</dx:GridViewDataTextColumn>
						<dx:GridViewDataTextColumn Caption="No of servers" VisibleIndex="3" FieldName="noofservers">
							<DataItemTemplate>
							
								<dx:ASPxTextBox ID="noofservers" runat="server" KeyFieldName="ServerId" Width="50px"
									AutoPostBack="false"  Value='<%# Eval("noofservers") %>' >
									<%-- <ClientSideEvents LostFocus="function(s, e) {s.Validate();}" 
                        Validation="function(s, e) {callbackpanel.PerformCallback();}" />--%>
									<%--<ValidationSettings ErrorText="Please Enter Numbers Only">
										<RequiredField IsRequired="true" ErrorText="Please Enter Numbers Only" />
										<RegularExpression ValidationExpression="^\d+" ErrorText="Please Enter Numbers Only" />
									</ValidationSettings>--%>
								</dx:ASPxTextBox>
							</DataItemTemplate>
							<HeaderStyle CssClass="GridCssHeader" />
							<CellStyle CssClass="GridCss">
							</CellStyle>
							<Settings AutoFilterCondition="Contains" AllowAutoFilter="False" />
						</dx:GridViewDataTextColumn>
						<dx:GridViewDataTextColumn Caption="Licenses" VisibleIndex="4" FieldName="totalunits">
							<DataItemTemplate>
								<dx:ASPxTextBox ID="totalunits" runat="server" KeyFieldName="ServerId" Width="50px"  Value='<%# Eval("totalunits") %>'
									>
								</dx:ASPxTextBox>
								<%--<dx:ASPxLabel ID="totalunits" runat="server" KeyFieldName="ServerId" Width="50px">
								</dx:ASPxLabel>--%>
							</DataItemTemplate>
							<HeaderStyle CssClass="GridCssHeader" />
							<CellStyle CssClass="GridCss"></CellStyle>
							<Settings AutoFilterCondition="Contains" AllowAutoFilter="False" />
						</dx:GridViewDataTextColumn>
					</Columns>
					<SettingsBehavior ConfirmDelete="True" ColumnResizeMode="NextColumn" />
					<SettingsBehavior AllowSort="false" AllowGroup="false" />
					<Settings ShowGroupPanel="false" ShowFilterRow="True" />
					<SettingsText ConfirmDelete="Are you sure you want to delete this record?" />
					<Settings ShowFooter="True" />
					<TotalSummary>
						<dx:ASPxSummaryItem SummaryType="Sum" />
					</TotalSummary>
					<Styles>
						<Header ImageSpacing="5px" SortingImageSpacing="5px">
						</Header>
						<LoadingPanel ImageSpacing="5px">
						</LoadingPanel>
						<GroupRow Font-Bold="True">
						</GroupRow>
						<AlternatingRow CssClass="GridAltRow" Enabled="True">
						</AlternatingRow>
					</Styles>
					<StylesEditors ButtonEditCellSpacing="0">
						<ProgressBar Height="21px">
						</ProgressBar>
					</StylesEditors>
					<SettingsPager PageSize="50" SEOFriendly="Enabled">
						<PageSizeItemSettings Visible="true" />
					</SettingsPager>
				</dx:ASPxGridView>
			</td>
		<%--	<td>
		<dx:ASPxLabel ID="totalunitssum" runat="server"  Text="Total License Count" CssClass="lblsmallFont"> 
								</dx:ASPxLabel>
		</td>
		<td>
		<dx:ASPxLabel ID="totalunitssumValue" runat="server" CssClass="lblsmallFont" >
								</dx:ASPxLabel>
		</td>--%>
		</tr>
		
	</table>
</asp:Content>
