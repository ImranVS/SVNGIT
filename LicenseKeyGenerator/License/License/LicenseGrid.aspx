<%@ Page Title="VitalSigns Plus - LicenseKey Generator" Language="C#" MasterPageFile="~/Site1.Master"
	AutoEventWireup="true" CodeBehind="LicenseGrid.aspx.cs" Inherits="License.LicenseGrid" %>

<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.Web" TagPrefix="dx" %>
<%--<%@ Register Src="~/Controls/DateRange.ascx" TagName="DateRange" TagPrefix="uc1" %>--%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
	<link href='http://fonts.googleapis.com/css?family=Francois One' rel='stylesheet'
		type='text/css' />
	<link rel="stylesheet" type="text/css" href="../css/vswebforms.css" />
	<script src="../js/jquery-1.9.1.js" type="text/javascript"></script>
	<script src="../js/bootstrap.min.js" type="text/javascript"></script>
	<%--<script type="text/javascript">
		$(document).ready(function () {
			$('.alert-success').delay(10000).fadeOut("slow", function () {
			});
		});--%>
	<script type="text/javascript">
        $(document).ready(function () {
            $('.alert-success').delay(100000000).fadeOut("slow", function () {
            });
        });
	</script>
	
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
	<table width="100%">
		<tr>
			<td>
				<div class="header" id="servernamelbldisp" runat="server">
					License Key Generator
				</div>
			</td>
		</tr>
		<tr>
			<td>
				<div id="successDiv" class="alert alert-success" runat="server" style="display: none">
					Success.
				</div>
			</td>
		</tr>
		<tr>
			<td>
				<table>
					<tr>
						<td>
							<dx:ASPxButton ID="NewButton" runat="server" Text="Generate Key" CssClass="sysButton"
								OnClick="NewButton_Click">
								<Image Url="~/images/icons/add.png">
								</Image>
							</dx:ASPxButton>
						</td>
						<td>
							<dx:ASPxButton ID="ASPxButton3" runat="server" Text="licenses expiring in next 30 days"
								CssClass="sysButton" OnClick="ExpiryButton_Click" Visible="false">
							</dx:ASPxButton>
						</td>
						<td>
							<dx:ASPxButton ID="ASPxButton1" runat="server" Text="Audit" CssClass="sysButton"
								OnClick="Audit_Click" Visible="false">
							</dx:ASPxButton>
						</td>
						<td>
							<dx:ASPxButton ID="ASPxButton2" runat="server" Text="Generate Report" CssClass="sysButton"
								OnClick="LicenseReport_Click">
							</dx:ASPxButton>
						</td>
					</tr>
				</table>
			</td>
		</tr>
	</table>
	<table>
		<tr>
			<td>
				<dx:ASPxLabel ID="ASPxLabel68" runat="server" align="left" CssClass="lblsmallFont"
					Text="Licenses expiring in next">
				</dx:ASPxLabel>
			</td>
			<td>
				<dx:ASPxSpinEdit ID="ExpiryTextBox" Width="50px" runat="server">

					<SpinButtons ShowIncrementButtons="False" ShowLargeIncrementButtons="false" />
				</dx:ASPxSpinEdit>
			</td>
			<td>
				<dx:ASPxLabel ID="ASPxLabel1" runat="server" align="left" CssClass="lblsmallFont"
					Text="days">
				</dx:ASPxLabel>
			</td>
			<td>
				<dx:ASPxButton ID="Submit" runat="server" OnClick="Submit_Click" Text="Go" AutoPostBack="True"
					CssClass="sysButton" Width="50px">
				</dx:ASPxButton>
			</td>
		</tr>
	</table>
	<table>
		<tr>
			<td>
				<dx:ASPxLabel ID="ASPxLabel5" runat="server" Text="Date range:" Visible="false" CssClass="lblsmallFont">
				</dx:ASPxLabel>
			</td>
			<%--<td>
                            <uc1:DateRange ID="dtPick" runat="server" Width="100px" Height="100%" Visible="false"></uc1:DateRange>    
                        </td>--%>
			<%--<td>
                <dx:ASPxButton ID="GoButton" runat="server" OnClick="GoButton_Click" Text="Submit" CssClass="sysButton" Visible="false" 
                   Width="50px">
                </dx:ASPxButton>
            </td>--%>
		</tr>
	</table>
	<table>
		<tr>
			<td>
				<dx:ASPxRoundPanel ID="fp" runat="server" HeaderText="Filter" Visible="false" Width="40px"
					CssFilePath="~/App_Themes/Glass/{0}/styles.css" SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css"
					GroupBoxCaptionOffsetY="-25px" Height="10%" CssPostfix="Glass">
					<PanelCollection>
						<dx:PanelContent ID="PanelContent1" runat="server">
							<dx:ASPxFilterControl ID="filter" runat="server" ClientInstanceName="filter">
								<Columns>
									<dx:FilterControlColumn PropertyName="ExpirationDate" ColumnType="DateTime" />
								</Columns>
								<ClientSideEvents Applied="function(s, e) { LicenseGridView.ApplyFilter(e.filterExpression);}" />
							</dx:ASPxFilterControl>
							<dx:ASPxButton runat="server" ID="btnApply" Text="Apply" AutoPostBack="false" UseSubmitBehavior="false"
								Width="40px" Style="margin: 12px auto 0; display: block;">
								<ClientSideEvents Click="function() { filter.Apply(); }" />
							</dx:ASPxButton>
						</dx:PanelContent>
					</PanelCollection>
				</dx:ASPxRoundPanel>
			</td>
		</tr>
	</table>
	<table>
		<tr>
			<td>
				<dx:ASPxGridView ID="LicenseGridView" ClientInstanceName="LicenseGridView" runat="server"
					AutoGenerateColumns="False" Width="95%" KeyFieldName="ID" OnHtmlRowCreated="LicenseGridView_HtmlRowCreated"
					OnHtmlDataCellPrepared="LicenseGridView_HtmlDataCellPrepared" EnableTheming="True" OnPageSizeChanged="LicenseGridView_PageSizeChanged"
					Theme="Office2003Blue">
					<Columns>
						<dx:GridViewCommandColumn ButtonType="Image" Caption="Actions" ShowInCustomizationForm="True"
							FixedStyle="Left" VisibleIndex="0" Width="5%" Visible="True">
							<EditButton Visible="True">
								<Image Url="../images/edit.png">
								</Image>
							</EditButton>
							<NewButton Visible="false">
								<Image Url="../images/icons/add.png">
								</Image>
							</NewButton>
							<DeleteButton Visible="false">
								<Image Url="../images/delete.png">
								</Image>
							</DeleteButton>
							<CancelButton Visible="True">
								<Image Url="~/images/cancel.gif">
								</Image>
							</CancelButton>
							<UpdateButton Visible="True">
								<Image Url="~/images/update.gif">
								</Image>
							</UpdateButton>
							<CellStyle CssClass="GridCss">
								<Paddings Padding="3px" />
							</CellStyle>
							<ClearFilterButton Visible="True">
								<Image Url="~/images/clear.png">
								</Image>
							</ClearFilterButton>
							<HeaderStyle CssClass="GridCssHeader" />
						</dx:GridViewCommandColumn>
						<dx:GridViewDataTextColumn Caption="Users" FixedStyle="Left" ShowInCustomizationForm="True"
							Width="10%" VisibleIndex="1" FieldName="LoginName">
							<Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
							<EditCellStyle CssClass="GridCss">
							</EditCellStyle>
							<EditFormCaptionStyle CssClass="GridCss">
							</EditFormCaptionStyle>
							<HeaderStyle CssClass="GridCssHeader" />
							<CellStyle CssClass="GridCss">
							</CellStyle>
						</dx:GridViewDataTextColumn>
						<dx:GridViewDataTextColumn Caption="Company Name" FixedStyle="Left" ShowInCustomizationForm="True"
							Width="10%" VisibleIndex="1" FieldName="CompanyName">
							<Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
							<EditCellStyle CssClass="GridCss">
							</EditCellStyle>
							<EditFormCaptionStyle CssClass="GridCss">
							</EditFormCaptionStyle>
							<HeaderStyle CssClass="GridCssHeader" />
							<CellStyle CssClass="GridCss">
							</CellStyle>
						</dx:GridViewDataTextColumn>
						<dx:GridViewDataTextColumn Caption="Units" VisibleIndex="4" FieldName="Units" ShowInCustomizationForm="True"
							Width="5%">
							<Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
							<EditCellStyle CssClass="GridCss">
							</EditCellStyle>
							<EditFormCaptionStyle CssClass="GridCss">
							</EditFormCaptionStyle>
							<HeaderStyle CssClass="GridCssHeader" Wrap="True" />
							<CellStyle CssClass="GridCss">
							</CellStyle>
						</dx:GridViewDataTextColumn>
						<dx:GridViewDataTextColumn Caption="Install Type" VisibleIndex="2" FieldName="InstallType"
							Width="10%">
							<Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
							<EditCellStyle CssClass="GridCss">
							</EditCellStyle>
							<EditFormCaptionStyle CssClass="GridCss">
							</EditFormCaptionStyle>
							<HeaderStyle CssClass="GridCssHeader" Wrap="True" />
							<CellStyle CssClass="GridCss">
							</CellStyle>
						</dx:GridViewDataTextColumn>
						<dx:GridViewDataTextColumn Caption="License Type" VisibleIndex="3" Width="10%" FieldName="LicenseType">
							<Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
							<EditCellStyle CssClass="GridCss">
							</EditCellStyle>
							<EditFormCaptionStyle CssClass="GridCss">
							</EditFormCaptionStyle>
							<HeaderStyle CssClass="GridCssHeader" />
							<CellStyle CssClass="GridCss">
							</CellStyle>
						</dx:GridViewDataTextColumn>
						<dx:GridViewDataTextColumn Caption="CreatedOn" VisibleIndex="5" Width="10%" FieldName="CreatedOn">
							<Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
							<PropertiesTextEdit DisplayFormatString="MM/dd/yyyy">
							</PropertiesTextEdit>
							<EditCellStyle CssClass="GridCss">
							</EditCellStyle>
							<EditFormCaptionStyle CssClass="GridCss">
							</EditFormCaptionStyle>
							<HeaderStyle CssClass="GridCssHeader" />
							<CellStyle CssClass="GridCss">
							</CellStyle>
						</dx:GridViewDataTextColumn>
						<dx:GridViewDataTextColumn Caption="Expiration Date" VisibleIndex="6" Width="10%"
							FieldName="ExpirationDate">
							<Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
							<PropertiesTextEdit DisplayFormatString="MM/dd/yyyy">
							</PropertiesTextEdit>
							<EditCellStyle CssClass="GridCss">
							</EditCellStyle>
							<EditFormCaptionStyle CssClass="GridCss">
							</EditFormCaptionStyle>
							<HeaderStyle CssClass="GridCssHeader" />
							<CellStyle CssClass="GridCss">
							</CellStyle>
						</dx:GridViewDataTextColumn>
						<%--	<dx:GridViewDataDateColumn FieldName="ExpirationDate" Caption="Expiration Date" Width="10%"
							VisibleIndex="3">
							<Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
							<EditCellStyle CssClass="GridCss">
							</EditCellStyle>
							<EditFormCaptionStyle CssClass="GridCss">
							</EditFormCaptionStyle>
							<HeaderStyle CssClass="GridCssHeader" />
							<CellStyle CssClass="GridCss">
							</CellStyle>
						</dx:GridViewDataDateColumn>--%>
						<dx:GridViewDataTextColumn Caption="CreatedOn" VisibleIndex="8" FieldName="CreatedOn"
							Width="90px" Visible="false">
							<Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
							<EditCellStyle CssClass="GridCss">
							</EditCellStyle>
							<EditFormCaptionStyle CssClass="GridCss">
							</EditFormCaptionStyle>
							<HeaderStyle CssClass="GridCssHeader2" Wrap="True" />
							<CellStyle CssClass="GridCss2">
							</CellStyle>
						</dx:GridViewDataTextColumn>
						<dx:GridViewDataTextColumn Caption="License Key" VisibleIndex="7" FieldName="LicenseKey">
							<Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
							<EditCellStyle CssClass="GridCss">
							</EditCellStyle>
							<EditFormCaptionStyle CssClass="GridCss">
							</EditFormCaptionStyle>
							<HeaderStyle CssClass="GridCssHeader" />
							<CellStyle CssClass="GridCss">
							</CellStyle>
						</dx:GridViewDataTextColumn>
					</Columns>
					<SettingsBehavior ConfirmDelete="True" ColumnResizeMode="NextColumn" />
					<Settings ShowGroupPanel="false" ShowFilterRow="True" />
					<SettingsText ConfirmDelete="Are you sure you want to delete this record?" />
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
					<%--<Settings ShowFilterBar="Visible" />--%>
					<SettingsPager PageSize="50" SEOFriendly="Enabled">
						<PageSizeItemSettings Visible="true" />
					</SettingsPager>
				</dx:ASPxGridView>
				&nbsp;
			</td>
		</tr>
	</table>
</asp:Content>
