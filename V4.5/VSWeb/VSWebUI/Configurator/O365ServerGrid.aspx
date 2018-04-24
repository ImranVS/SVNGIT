<%@ Page Title="VitalSigns Plus - Office 365 Servers" Language="C#" MasterPageFile="~/Site1.Master"
	AutoEventWireup="true" CodeBehind="O365ServerGrid.aspx.cs" Inherits="VSWebUI.Configurator.O365ServerGrid" %>

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
		function OnCustomButtonClick(s, e) {
			visibleIndex = e.visibleIndex;

			if (e.buttonID == "deleteButton")
				O365ServerGridView.GetRowValues(e.visibleIndex, 'Name', OnGetRowValues);

			function OnGetRowValues(values) {
				var id = values[0];
				var name = values[1];
				var OK = (confirm('Are you sure you want to delete the user - ' + values + '?'))
				if (OK == true) {
					O365ServerGridView.DeleteRow(visibleIndex);

					//alert('The Server ' + values + ' was Successfully Deleted');
					//ScriptManager.RegisterClientScriptBlock(base.Page, this.GetType(), "FooterRequired", "alert('Notification : Record deleted successfully');", true);
				}

				else {
				}

			}
		}

	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
	<table width="100%">
		<tr>
			<td>
				<div class="header" id="servernamelbldisp" runat="server">
					Microsoft Office 365 Tenants</div>
				<div class="info">
					VitalSigns will attempt to scan any valid Office 365 tenant.
				</div>
				<div id="successDiv" class="alert alert-success" runat="server" style="display: none">
					Success.
				</div>
				<dx:ASPxButton ID="NewButton" runat="server" Text="New" CssClass="sysButton" OnClick="NewButton_Click">
					<Image Url="~/images/icons/add.png">
					</Image>
				</dx:ASPxButton>
				<dx:ASPxButton ID="ProxySettingsButton0" runat="server" CssFilePath="~/App_Themes/Office2010Blue/{0}/styles.css"
					CssPostfix="Office2010Blue" OnClick="ProxySettingsButton_Click" SpriteCssFilePath="~/App_Themes/Office2010Blue/{0}/sprite.css"
					Text="Proxy Settings" Wrap="False" Visible="False">
				</dx:ASPxButton>
			</td>
		</tr>
		<tr>
			<td>
				<dx:ASPxGridView ID="O365ServerGridView" runat="server" AutoGenerateColumns="False"
					Width="100%" KeyFieldName="ID" OnHtmlRowCreated="O365ServerGridView_HtmlRowCreated"
					ClientInstanceName="O365ServerGridView" OnRowDeleting="O365ServerGridView_RowDeleting"
					OnPageSizeChanged="O365ServerGridView_PageSizeChanged" OnHtmlDataCellPrepared="O365ServerGridView_HtmlDataCellPrepared"
					Theme="Office2003Blue">
					<ClientSideEvents CustomButtonClick="OnCustomButtonClick" />
					<Columns>
						<dx:GridViewCommandColumn ButtonType="Image" Caption="Actions" FixedStyle="Left"
							VisibleIndex="0" Width="70px">
							<NewButton Visible="True">
								<Image Url="../images/icons/add.png">
								</Image>
							</NewButton>
							<EditButton Visible="True">
								<Image Url="../images/edit.png">
								</Image>
							</EditButton>
							<DeleteButton Visible="false">
								<Image Url="../images/delete.png">
								</Image>
							</DeleteButton>
							<EditButton Visible="True">
								<Image Url="../images/edit.png">
								</Image>
							</EditButton>
							<NewButton Visible="True">
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
							<ClearFilterButton Visible="True">
								<Image Url="~/images/clear.png">
								</Image>
							</ClearFilterButton>
							<HeaderStyle CssClass="GridCssHeader" />
							<CellStyle CssClass="GridCss1">
								<Paddings Padding="3px" />
							</CellStyle>
						</dx:GridViewCommandColumn>
							<dx:GridViewCommandColumn Caption="Delete" ButtonType="Image" VisibleIndex="1" Width="50px" FixedStyle="Left">
							<CustomButtons>
								<dx:GridViewCommandColumnCustomButton ID="deleteButton" Image-Url="../images/delete.png"
									Text="Delete">
									<Image Url="../images/delete.png">
									</Image>
								</dx:GridViewCommandColumnCustomButton>
							</CustomButtons>
							<HeaderStyle CssClass="GridCssHeader1" />
						</dx:GridViewCommandColumn>
						<dx:GridViewDataCheckColumn Caption="Enabled" VisibleIndex="2" FixedStyle="Left"
							FieldName="Enabled" Width="60px">
							<Settings AllowAutoFilter="False" />
							<EditCellStyle CssClass="GridCss">
							</EditCellStyle>
							<EditFormCaptionStyle CssClass="GridCss">
							</EditFormCaptionStyle>
							<HeaderStyle CssClass="GridCssHeader">
								<Paddings Padding="5px" />
							</HeaderStyle>
							<CellStyle CssClass="GridCss1">
							</CellStyle>
						</dx:GridViewDataCheckColumn>
						<dx:GridViewDataTextColumn Caption="Name" VisibleIndex="3" FixedStyle="Left" FieldName="Name"
							Width="200px">
							<Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
							<EditCellStyle CssClass="GridCss">
							</EditCellStyle>
							<EditFormCaptionStyle CssClass="GridCss">
							</EditFormCaptionStyle>
							<HeaderStyle CssClass="GridCssHeader" />
							<CellStyle CssClass="GridCss">
							</CellStyle>
						</dx:GridViewDataTextColumn>
						<dx:GridViewDataTextColumn Caption="Scan Interval" VisibleIndex="4" FieldName="ScanInterval"
							Width="70px">
							<Settings AllowAutoFilter="False" AutoFilterCondition="Contains" />
							<EditCellStyle CssClass="GridCss">
							</EditCellStyle>
							<EditFormCaptionStyle CssClass="GridCss">
							</EditFormCaptionStyle>
							<HeaderStyle CssClass="GridCssHeader2" Wrap="True" />
							<CellStyle CssClass="GridCss2">
							</CellStyle>
						</dx:GridViewDataTextColumn>
						<dx:GridViewDataTextColumn Caption="Off-Hours Scan Interval" VisibleIndex="5" FieldName="OffHoursScanInterval"
							Width="90px">
							<Settings AllowAutoFilter="False" AutoFilterCondition="Contains" />
							<EditCellStyle CssClass="GridCss">
							</EditCellStyle>
							<EditFormCaptionStyle CssClass="GridCss">
							</EditFormCaptionStyle>
							<HeaderStyle CssClass="GridCssHeader2" Wrap="True" />
							<CellStyle CssClass="GridCss2">
							</CellStyle>
						</dx:GridViewDataTextColumn>
						<dx:GridViewDataTextColumn Caption="Retry Interval" VisibleIndex="6" FieldName="RetryInterval"
							Width="70px">
							<Settings AllowAutoFilter="False" AutoFilterCondition="Contains" />
							<EditCellStyle CssClass="GridCss">
							</EditCellStyle>
							<EditFormCaptionStyle CssClass="GridCss">
							</EditFormCaptionStyle>
							<HeaderStyle CssClass="GridCssHeader2" Wrap="True" />
							<CellStyle CssClass="GridCss2">
							</CellStyle>
						</dx:GridViewDataTextColumn>
						<dx:GridViewDataTextColumn Caption="Search String" VisibleIndex="8" FieldName="SearchString" Visible="false">
							<Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
							<EditCellStyle CssClass="GridCss">
							</EditCellStyle>
							<EditFormCaptionStyle CssClass="GridCss">
							</EditFormCaptionStyle>
							<HeaderStyle CssClass="GridCssHeader" />
							<CellStyle CssClass="GridCss">
							</CellStyle>
						</dx:GridViewDataTextColumn>
						<dx:GridViewDataTextColumn Caption="Response Threshold" VisibleIndex="7" FieldName="ResponseThreshold"
							Width="70px">
							<Settings AllowAutoFilter="False" AutoFilterCondition="Contains" />
							<EditCellStyle CssClass="GridCss">
							</EditCellStyle>
							<EditFormCaptionStyle CssClass="GridCss">
							</EditFormCaptionStyle>
							<HeaderStyle CssClass="GridCssHeader2" Wrap="True" />
							<CellStyle CssClass="GridCss2">
							</CellStyle>
						</dx:GridViewDataTextColumn>
						<dx:GridViewDataTextColumn Caption="User Name" VisibleIndex="9" FieldName="UserName">
							<Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
							<EditCellStyle CssClass="GridCss">
							</EditCellStyle>
							<EditFormCaptionStyle CssClass="GridCss">
							</EditFormCaptionStyle>
							<HeaderStyle CssClass="GridCssHeader" />
							<CellStyle CssClass="GridCss">
							</CellStyle>
						</dx:GridViewDataTextColumn>
						<dx:GridViewDataTextColumn Caption="Pwd" VisibleIndex="10" FieldName="PW" Visible="false">
							<EditCellStyle CssClass="GridCss">
							</EditCellStyle>
							<EditFormCaptionStyle CssClass="GridCss">
							</EditFormCaptionStyle>
							<HeaderStyle CssClass="GridCssHeader" />
							<CellStyle CssClass="GridCss">
							</CellStyle>
						</dx:GridViewDataTextColumn>
						<dx:GridViewDataTextColumn Caption="ID" FieldName="ID" Visible="False" VisibleIndex="11">
						</dx:GridViewDataTextColumn>
					</Columns>
					<SettingsBehavior ConfirmDelete="True" ColumnResizeMode="NextColumn" />
					<%--<Settings ShowGroupPanel="True" ShowFilterRow="True" />
					<SettingsText ConfirmDelete=" 'Are you sure you want to delete?'" />--%>
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
		</tr>
	</table>
</asp:Content>
