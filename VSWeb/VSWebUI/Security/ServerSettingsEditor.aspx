<%@ Page Title="VitalSigns Plus - Manage Server Settings" Language="C#" MasterPageFile="~/Site1.Master"
	AutoEventWireup="true" CodeBehind="ServerSettingsEditor.aspx.cs" Inherits="VSWebUI.Security.ServerSettingsEditor" %>

<%@ Register Assembly="DevExpress.Web.ASPxTreeList.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.Web.ASPxTreeList" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.Web" TagPrefix="dx1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
	<script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.5/jquery.min.js"></script>
	<script src="../js/jquery-1.9.1.js" type="text/javascript"></script>
	<script src="../js/bootstrap.min.js" type="text/javascript"></script>
	<link href='http://fonts.googleapis.com/css?family=Francois One' rel='stylesheet'
		type='text/css' />
	<link rel="stylesheet" type="text/css" href="../css/vswebforms.css" />
	<script type="text/javascript">
		$(document).ready(function () {
			$('.alert-success').delay(60000).fadeOut("slow", function () {
			});
		});
		function ProcessSelection(grid, visibleIndex, checkbox, keyValue, hiddenField) {

			if (checkbox.GetChecked()) {

				if (!hiddenField.Contains("key" + keyValue.toString())) {
					hiddenField.Add("key" + keyValue.toString(), true);
				}
			}
			else {

				if (hiddenField.Contains("key" + keyValue)) {
					hiddenField.Remove("key" + keyValue.toString());
				}
			}
		}
	</script>
	<script type="text/javascript">
		function diskSettingsRadioBtnclicked(s) {

			if (s.GetSelectedItem().value == 1) {
				CleardiskSettings.SetVisible(true);
			}
			else {
				CleardiskSettings.SetVisible(false);
			}
		}
		function SetComboBox(s) {
			if (s.GetText() == "Exchange") {
				cmbRole.SetVisible(true);
				document.getElementById("ContentPlaceHolder1_ASPxPageControl1_ExchangeRolesLabel").style.visibility = 'visible';

			}
			else {
				cmbRole.SetSelectedIndex(-1);
				//if (s.GetText()!= "Exchange") 
				cmbRole.SetVisible(false);
				document.getElementById("ContentPlaceHolder1_ASPxPageControl1_ExchangeRolesLabel").style.visibility = 'hidden';

				//clear cmbRole remains

			}
		}
	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table width="100%">
		<tr>
			<td>
				<div class="header" id="servernamelbldisp" runat="server">
					Server Settings Editor</div>
			</td>
		</tr>
		<tr>
			<td>
				<div class="info">
					The server settings editor allows you to change one or more settings across multiple
					servers. First, select a server type from the drop down menu.
					<br />
					Then select the attributes you wish to change and specify the value. Finally, select
					the servers you wish to change and click the 'Apply' button.
				</div>
			</td>
		</tr>
	
		<tr>
			<td>
				<dx:ASPxPageControl ID="ASPxPageControl1" runat="server" Font-Bold="True" Theme="Glass"
					ActiveTabIndex="0" Width="100%" EnableHierarchyRecreation="False" EnableTabScrolling="True">
					<TabPages>
						<dx:TabPage Text="Server Attributes">
							<TabImage Url="~/images/information.png">
							</TabImage>
							<ContentCollection>
								<dx:ContentControl runat="server" SupportsDisabledAttribute="True">
									<%--<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
										<ContentTemplate>--%>
									<table>
										<tr>
											<td>
												<asp:Label ID="Label1" runat="server" Text="Select Profile:" CssClass="lblsmallFont"></asp:Label>
											</td>
											<td>
												<dx:ASPxComboBox ID="ProfileComboBox" runat="server" ClientInstanceName="cmbLocation"
													ValueType="System.String" AutoPostBack="True" OnSelectedIndexChanged="ProfileComboBox_SelectedIndexChanged">
													<ValidationSettings ErrorDisplayMode="ImageWithTooltip">
														<RequiredField ErrorText="Select Profile." IsRequired="True" />
														<RequiredField IsRequired="True" ErrorText="Select Location."></RequiredField>
													</ValidationSettings>
												</dx:ASPxComboBox>
											</td>
											<td>
												<asp:Label ID="Label2" runat="server" Text="Select Server Type:" CssClass="lblsmallFont"></asp:Label>
											</td>
											<td>
												<dx:ASPxComboBox ID="ServerTypeComboBox" runat="server" ClientInstanceName="cmbServerType"
													ValueType="System.String" AutoPostBack="True" Enabled="false" OnSelectedIndexChanged="ServerTypeComboBox_SelectedIndexChanged">
													<%--<ClientSideEvents SelectedIndexChanged="function(s, e) { SetComboBox(s); }"></ClientSideEvents>--%>
													<ValidationSettings ErrorDisplayMode="ImageWithTooltip">
														<RequiredField ErrorText="Select server type." IsRequired="True" />
														<RequiredField IsRequired="True" ErrorText="Select server type."></RequiredField>
													</ValidationSettings>
												</dx:ASPxComboBox>
											</td>
											<td>
												<asp:Label ID="ExchangeRolesLabel" runat="server" Text="Role:" CssClass="lblsmallFont"></asp:Label>
											</td>
											<td>
												<dx:ASPxComboBox ID="ExchangeRolesComboBox" runat="server" ClientInstanceName="cmbRole"
													ValueType="System.String" AutoPostBack="True" OnSelectedIndexChanged="ExchangeRolesComboBox_SelectedIndexChanged">
													<ValidationSettings ErrorDisplayMode="ImageWithTooltip">
														<RequiredField ErrorText="Select role." />
														<RequiredField ErrorText="Select role."></RequiredField>
													</ValidationSettings>
													<Items>
														<dx:ListEditItem Text="General" Value="" />
														<dx:ListEditItem Text="HUB" Value="HUB" />
														<dx:ListEditItem Text="EDGE" Value="EDGE" />
														<dx:ListEditItem Text="MailBox" Value="Mailbox" />
														<dx:ListEditItem Text="CAS" Value="CAS" />
													</Items>
												</dx:ASPxComboBox>
											</td>
										</tr>
									</table>
									<table width="100%">
										<tr>
											<td>
												<dx:ASPxGridView ID="ProfilesGridView" runat="server" KeyFieldName="ID" AutoGenerateColumns="False"
													EnableTheming="True" Theme="Office2003Blue" Width="100%" OnPageSizeChanged="ProfilesGridView_OnPageSizeChanged">
													<Columns>
														<dx:GridViewCommandColumn Caption="Select" ShowSelectCheckbox="True" VisibleIndex="0">
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
														<dx:GridViewDataTextColumn Caption="ID" FieldName="ID" Visible="False" VisibleIndex="5">
														</dx:GridViewDataTextColumn>
														<dx:GridViewDataTextColumn Caption="isSelected" FieldName="isSelected" Visible="False"
															VisibleIndex="4">
														</dx:GridViewDataTextColumn>
													</Columns>
													<SettingsBehavior AllowSort="false" />
													<SettingsPager>
														<PageSizeItemSettings Visible="True">
														</PageSizeItemSettings>
													</SettingsPager>
													<Styles>
														<AlternatingRow CssClass="GridAltRow">
														</AlternatingRow>
													</Styles>
												</dx:ASPxGridView>
											</td>
										</tr>
										<tr>
											<td>
												<div id="emptyDiv3" runat="server" style="display: none">
													&nbsp;</div>
												<div id="successDiv" runat="server" class="alert alert-success" style="display: none">
															Settings for selected servers were successully updated.
														</div>
											</td>
										</tr>
										<tr>
											<td>
												<div id="emptyDiv" runat="server" style="display: none">
													&nbsp;</div>
												<dx:ASPxButton ID="CollapseAllButton" runat="server" Text="Collapse All" Visible="false"
													CssClass="sysButton" Wrap="False" OnClick="CollapseAllButton_Click">
													<Image Url="~/images/icons/forbidden.png">
													</Image>
												</dx:ASPxButton>
											</td>
										</tr>
										<tr>
											<td>
												<div id="tblServer" runat="server" style="display: none">
													<dx:ASPxTreeList ID="ServersTreeList" KeyFieldName="Id" ParentFieldName="LocId" runat="server"
														AutoGenerateColumns="False" EnableTheming="True" Width="100%" Theme="Office2003Blue" OnDataBound="DataBound">
														<Columns>
															<dx:TreeListTextColumn Caption="Server" FieldName="Name" Name="Servers" VisibleIndex="0">
																<HeaderStyle CssClass="GridCssHeader" />
																<CellStyle CssClass="GridCss">
																</CellStyle>
															</dx:TreeListTextColumn>
															<dx:TreeListTextColumn Caption="actid" FieldName="actid" Name="actid" Visible="False"
																VisibleIndex="1">
															</dx:TreeListTextColumn>
															<dx:TreeListTextColumn Caption="tbl" FieldName="tbl" Name="tbl" Visible="False" VisibleIndex="2">
															</dx:TreeListTextColumn>
															<dx:TreeListTextColumn Caption="locid" FieldName="locid" Name="locid" Visible="False"
																VisibleIndex="3">
															</dx:TreeListTextColumn>
															<dx:TreeListTextColumn FieldName="ServerType" VisibleIndex="4">
																<HeaderStyle CssClass="GridCssHeader" />
																<CellStyle CssClass="GridCss">
																</CellStyle>
															</dx:TreeListTextColumn>
															<dx:TreeListTextColumn FieldName="Description" VisibleIndex="5">
																<HeaderStyle CssClass="GridCssHeader" />
																<CellStyle CssClass="GridCss">
																</CellStyle>
															</dx:TreeListTextColumn>
														</Columns>
                                                           <Summary>
                         <dx:TreeListSummaryItem   FieldName="Name" SummaryType="Count" ShowInColumn="Name" DisplayFormat="{0} Item(s)" /></Summary>
														<Settings GridLines="Both" />
														<SettingsBehavior AllowDragDrop="False" AutoExpandAllNodes="True" />
														<SettingsPager AlwaysShowPager="True" Mode="ShowPager" PageSize="20">
															<PageSizeItemSettings Visible="True">
															</PageSizeItemSettings>
														</SettingsPager>
														<SettingsSelection AllowSelectAll="True" Enabled="True" Recursive="True" />
														<Styles>
															<AlternatingNode Enabled="True">
															</AlternatingNode>
														</Styles>
													</dx:ASPxTreeList>
												</div>
											</td>
										</tr>
										<tr>
											<td>
												<div id="Div9" runat="server" style="display: none">
													&nbsp;
												</div>
												<div id="errorDiv" class="alert alert-danger" runat="server" style="display: none">
													Please select at least one Attribute and one Server in order to proceed.
												</div>
												<div id="errorDiv5" class="alert alert-danger" runat="server" style="display: none">
													Please select at least one Attribute to proceed.
												</div>
												<div id="errorDiv6" class="alert alert-danger" runat="server" style="display: none">
													Please select at least one Server to proceed.
												</div>
												<div id="errorDiv7" class="alert alert-danger" runat="server" style="display: none">
													The servers do not exist.
												</div>
												<div id="errorDiv2" class="alert alert-danger" runat="server" style="display: none">
													Settings for selected servers were NOT updated.
												</div>
											</td>
										</tr>
									</table>
									<table>
										<tr>
											<td>
												<dx:ASPxButton ID="ApplyServersButton" runat="server" Text="Apply" Visible="false"
													Width="50px" CssClass="sysButton" OnClick="ApplyServersButton_Click">
												</dx:ASPxButton>
											</td>
										</tr>
									</table>
									<%--</ContentTemplate>--%>
									<%--<Triggers>
											<asp:AsyncPostBackTrigger ControlID="ServerTypeComboBox" />
											<asp:AsyncPostBackTrigger ControlID="ApplyServersButton" />
										</Triggers>--%>
									<%--	</asp:UpdatePanel>--%>
								</dx:ContentControl>
							</ContentCollection>
						</dx:TabPage>
						<dx:TabPage Text="Domino Server Tasks">
							<TabImage Url="~/images/information.png">
							</TabImage>
							<ContentCollection>
								<dx:ContentControl runat="server" SupportsDisabledAttribute="True">
									<asp:UpdatePanel ID="UpdatePanel2" runat="server">
										<ContentTemplate>
											<table width="100%">
												<tr>
													<td>
														<dx:ASPxGridView ID="DominoTasksGridView" KeyFieldName="TaskID" runat="server" AutoGenerateColumns="False"
															EnableTheming="True" Theme="Office2003Blue" ClientInstanceName="masterGrid" Width="100%"
															OnPageSizeChanged="DominoTasksGridView_OnPageSizeChanged">
															<Columns>
																<dx:GridViewCommandColumn Caption="Select" ShowInCustomizationForm="True" ShowSelectCheckbox="True"
																	VisibleIndex="0">
																	<ClearFilterButton Visible="True">
																	</ClearFilterButton>
																	<HeaderStyle CssClass="GridCssHeader1" />
																</dx:GridViewCommandColumn>
																<dx:GridViewDataTextColumn FieldName="TaskID" ShowInCustomizationForm="True" Visible="False"
																	VisibleIndex="3">
																</dx:GridViewDataTextColumn>
																<dx:GridViewDataTextColumn Caption="Task Name" FieldName="TaskName" ShowInCustomizationForm="True"
																	VisibleIndex="1">
																	<DataItemTemplate>
																		<dx:ASPxLabel ID="txtTaskName" runat="server" Value='<%# Eval("TaskName") %>'>
																		</dx:ASPxLabel>
																	</DataItemTemplate>
																	<HeaderStyle CssClass="GridCssHeader" />
																	<CellStyle CssClass="GridCss">
																	</CellStyle>
																</dx:GridViewDataTextColumn>
																<dx:GridViewDataTextColumn Caption="isSelected" FieldName="isSelected" ShowInCustomizationForm="True"
																	Visible="False" VisibleIndex="2">
																</dx:GridViewDataTextColumn>
																<dx:GridViewDataCheckColumn Caption="Load" FieldName="SendLoadCommand" ShowInCustomizationForm="True"
																	VisibleIndex="4">
																	<Settings AllowAutoFilter="False" />
																	<EditFormSettings Caption="If a task did not load, send &amp;#39;Load&amp;#39; command to the server (i.e., try to start it)" />
																	<EditCellStyle CssClass="GridCss1">
																	</EditCellStyle>
																	<EditFormCaptionStyle CssClass="GridCss1">
																	</EditFormCaptionStyle>
																	<DataItemTemplate>
																		<dx:ASPxCheckBox ID="SendLoadCommand" runat="server" Checked="false" OnInit="SelectCheckboxSendLoadCommand_Init">
																		</dx:ASPxCheckBox>
																	</DataItemTemplate>
																	<HeaderStyle CssClass="GridCssHeader1" />
																	<CellStyle CssClass="GridCss1">
																	</CellStyle>
																</dx:GridViewDataCheckColumn>
																<dx:GridViewDataCheckColumn Caption="Restart ASAP" FieldName="SendRestartCommand"
																	ShowInCustomizationForm="True" UnboundType="Boolean" VisibleIndex="5">
																	<Settings AllowAutoFilter="False" />
																	<EditFormSettings Caption="If Task is HUNG, or LOAD TASK fails, send &amp;#39;Tell Server Restart&amp;#39; command, AS SOON AS POSSIBLE" />
																	<EditCellStyle CssClass="GridCss1">
																	</EditCellStyle>
																	<EditFormCaptionStyle CssClass="GridCss1">
																	</EditFormCaptionStyle>
																	<DataItemTemplate>
																		<dx:ASPxCheckBox ID="SendRestartCommand" runat="server" Checked="false" OnInit="SelectCheckboxRestartASAP_Init">
																		</dx:ASPxCheckBox>
																	</DataItemTemplate>
																	<HeaderStyle CssClass="GridCssHeader1" />
																	<CellStyle CssClass="GridCss1">
																	</CellStyle>
																</dx:GridViewDataCheckColumn>
																<dx:GridViewDataCheckColumn Caption="Restart Later" FieldName="RestartOffHours" ShowInCustomizationForm="True"
																	VisibleIndex="6">
																	<Settings AllowAutoFilter="True" />
																	<EditFormSettings Caption="If Task is HUNG, or LOAD TASK fails, send &amp;#39;Tell Server Restart&amp;#39; command, OFF PEAK HOURS ONLY" />
																	<EditCellStyle CssClass="GridCss1">
																	</EditCellStyle>
																	<EditFormCaptionStyle CssClass="GridCss1">
																	</EditFormCaptionStyle>
																	<DataItemTemplate>
																		<dx:ASPxCheckBox ID="RestartOffHours" runat="server" Checked="false" OnInit="SelectCheckboxRestartOffHours_Init">
																		</dx:ASPxCheckBox>
																	</DataItemTemplate>
																	<HeaderStyle CssClass="GridCssHeader1" />
																	<CellStyle CssClass="GridCss1">
																	</CellStyle>
																</dx:GridViewDataCheckColumn>
																<dx:GridViewDataCheckColumn Caption="Disallow" FieldName="SendExitCommand" ShowInCustomizationForm="True"
																	VisibleIndex="8">
																	<PropertiesCheckEdit>
																		<CheckBoxStyle HorizontalAlign="Left" Wrap="True" />
																		<Style HorizontalAlign="Left" Wrap="True"></Style>
																	</PropertiesCheckEdit>
																	<Settings AllowAutoFilter="False" />
																	<EditFormSettings Caption="If Task is running, send &amp;#39;Task exit&amp;#39; command (i.e prohibit this command)" />
																	<EditCellStyle CssClass="GridCss1">
																	</EditCellStyle>
																	<EditFormCaptionStyle CssClass="GridCss1">
																	</EditFormCaptionStyle>
																	<DataItemTemplate>
																		<dx:ASPxCheckBox ID="SendExitCommand" runat="server" Checked="false" OnInit="SelectCheckboxSendExitCommand_Init">
																		</dx:ASPxCheckBox>
																	</DataItemTemplate>
																	<HeaderStyle CssClass="GridCssHeader1" />
																	<CellStyle CssClass="GridCss1">
																	</CellStyle>
																</dx:GridViewDataCheckColumn>
															</Columns>
															<SettingsBehavior ConfirmDelete="True" AllowSort="false" />
															<SettingsPager AlwaysShowPager="True">
																<PageSizeItemSettings Visible="True">
																</PageSizeItemSettings>
															</SettingsPager>
															<SettingsText ConfirmDelete="Are you sure you want to delete this Profile?" />
															<Styles>
																<AlternatingRow CssClass="GridAltRow">
																</AlternatingRow>
															</Styles>
														</dx:ASPxGridView>
														<dx:ASPxHiddenField ID="masterSelection" ClientInstanceName="masterSelection" runat="server">
														</dx:ASPxHiddenField>
														<dx:ASPxHiddenField ID="SendLoadCommandSelection" ClientInstanceName="SendLoadCommandSelection"
															runat="server">
														</dx:ASPxHiddenField>
														<dx:ASPxHiddenField ID="RestartASAPSelection" ClientInstanceName="RestartASAPSelection"
															runat="server">
														</dx:ASPxHiddenField>
														<dx:ASPxHiddenField ID="RestartOffHoursSelection" ClientInstanceName="RestartOffHoursSelection"
															runat="server">
														</dx:ASPxHiddenField>
														<dx:ASPxHiddenField ID="SendExitCommandSelection" ClientInstanceName="SendExitCommandSelection"
															runat="server">
														</dx:ASPxHiddenField>
													</td>
												</tr>
												<tr>
													<td>
														<div id="emptyDiv2" style="display: block">
															&nbsp;</div>
														<dx:ASPxButton ID="CollapseAllButton_Domino" runat="server" Text="Collapse All" CssClass="sysButton"
															Wrap="False" OnClick="CollapseAllButton_Domino_Click">
															<Image Url="~/images/icons/forbidden.png">
															</Image>
														</dx:ASPxButton>
													</td>
												</tr>
												<tr>
													<td>
														<dx:ASPxTreeList ID="DominoServerTreeList" KeyFieldName="Id" ParentFieldName="LocId"
															runat="server" AutoGenerateColumns="False" EnableTheming="True" Theme="Office2003Blue"
															Width="100%">
															<Columns>
																<dx:TreeListTextColumn Caption="Servers" FieldName="Name" Name="Servers" ShowInCustomizationForm="True"
																	VisibleIndex="0">
																	<HeaderStyle CssClass="GridCssHeader" />
																	<CellStyle CssClass="GridCss">
																	</CellStyle>
																</dx:TreeListTextColumn>
																<dx:TreeListTextColumn FieldName="actid" Name="actid" ShowInCustomizationForm="True"
																	Visible="False" VisibleIndex="1">
																</dx:TreeListTextColumn>
																<dx:TreeListTextColumn FieldName="tbl" Name="tbl" ShowInCustomizationForm="True"
																	Visible="False" VisibleIndex="2">
																</dx:TreeListTextColumn>
																<dx:TreeListTextColumn FieldName="LocId" Name="LocId" ShowInCustomizationForm="True"
																	Visible="False" VisibleIndex="3">
																</dx:TreeListTextColumn>
																<dx:TreeListTextColumn FieldName="ServerType" ShowInCustomizationForm="True" VisibleIndex="4">
																	<HeaderStyle CssClass="GridCssHeader" />
																	<CellStyle CssClass="GridCss">
																	</CellStyle>
																</dx:TreeListTextColumn>
																<dx:TreeListTextColumn FieldName="Description" ShowInCustomizationForm="True" VisibleIndex="5">
																	<HeaderStyle CssClass="GridCssHeader" />
																	<CellStyle CssClass="GridCss">
																	</CellStyle>
																</dx:TreeListTextColumn>
															</Columns>
															<Settings GridLines="Both" />
															<SettingsBehavior AllowDragDrop="False" AutoExpandAllNodes="True" />
															<SettingsPager Mode="ShowPager" PageSize="20">
																<PageSizeItemSettings Visible="True">
																</PageSizeItemSettings>
															</SettingsPager>
															<SettingsSelection AllowSelectAll="True" Enabled="True" Recursive="True" />
															<Styles>
																<AlternatingNode Enabled="True">
																</AlternatingNode>
															</Styles>
														</dx:ASPxTreeList>
														<div id="emptyDiv4" runat="server" style="display: none">
															&nbsp;
														</div>
														<div id="successDivDomino" runat="server" class="alert alert-success" style="display: none">
															Settings for selected servers were successully updated.
														</div>
														<div id="errorDiv3" class="alert alert-danger" runat="server" style="display: none">
															Please select at least one Task and one Server in order to proceed.
															<button type="button" class="close" data-dismiss="alert">
																<span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
														</div>
														<div id="errorDiv4" class="alert alert-danger" runat="server" style="display: none">
															Settings for selected servers were NOT updated.
														</div>
													</td>
												</tr>
											</table>
										</ContentTemplate>
										<%--<Triggers>
											<asp:AsyncPostBackTrigger ControlID="DominoServerTasksApply" />
											<asp:AsyncPostBackTrigger ControlID="DominoServerTasksClear" />
										</Triggers>--%>
									</asp:UpdatePanel>
									<table>
										<tr>
											<td>
												<dx:ASPxButton ID="DominoServerTasksApply" runat="server" Text="Add Task(s)" CssClass="sysButton"
													OnClick="DominoServerTasksApply_Click">
												</dx:ASPxButton>
											</td>
											<td>
												<dx:ASPxButton ID="DominoServerTasksClear" runat="server" Text="Remove Task(s)" CssClass="sysButton"
													OnClick="DominoServerTasksClear_Click">
												</dx:ASPxButton>
											</td>
										</tr>
									</table>
								</dx:ContentControl>
							</ContentCollection>
						</dx:TabPage>
						<dx:TabPage Text="Windows Services">
							<TabImage Url="~/images/information.png">
							</TabImage>
							<ContentCollection>
								<dx:ContentControl ID="ContentControl2" runat="server" SupportsDisabledAttribute="True">
									<asp:UpdatePanel ID="UpdatePanel4" runat="server">
										<ContentTemplate>
											<table>
												<tr>
													<td>
														<table>
															<tr width="100%">
																<td>
																	<asp:Label ID="Label3" runat="server" Text="Server Type:" CssClass="lblsmallFont"></asp:Label>
                                                                    </td>
																<td>
																	<dx:ASPxComboBox ID="ServerTypes" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ServerTypes_SelectedIndexChanged">
																		<Items>
																			<dx:ListEditItem Text="Exchange" Value="5" />
																			<dx:ListEditItem Text="SharePoint" Value="4" />
																			<dx:ListEditItem Text="Active Directory" Value="18" />
																		</Items>
																		<ValidationSettings ErrorDisplayMode="ImageWithTooltip">
																			<RequiredField ErrorText="Select Server Type." />
																		</ValidationSettings>
																	</dx:ASPxComboBox>
																</td>
																<td>
																	&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
																</td>
																<td class=info>
																	<div>
																		The Service Display Name will be displayed. If there was an issue fetching the Display Name, the Service Name will be displayed
																	</div>
																</td>
															</tr>
														</table>
													</td>
												</tr>
												<tr>
													<td>
														<dx:ASPxGridView ID="WindowsServices" KeyFieldName="DisplayName" runat="server" AutoGenerateColumns="False"
															EnableTheming="True" Theme="Office2003Blue" Width="100%" OnPageSizeChanged="WindowsServices_OnPageSizeChanged">
															<Columns>
																<dx:GridViewCommandColumn Caption="Select" ShowSelectCheckbox="True" VisibleIndex="0" Width="10%"
																	ShowClearFilterButton="False">
																	<ClearFilterButton Visible="True">
																	</ClearFilterButton>
																	<HeaderStyle CssClass="GridCssHeader1" />
																</dx:GridViewCommandColumn>

																<dx:GridViewDataTextColumn Caption="Service Name" FieldName="VitalSignsDisplayName" VisibleIndex="1">
																	<DataItemTemplate>
																		<dx:ASPxLabel ID="txtServiceName" runat="server" Value='<%# Eval("VitalSignsDisplayName") %>'>
																		</dx:ASPxLabel>
																	</DataItemTemplate>
																	<HeaderStyle CssClass="GridCssHeader" />
																	<Settings AllowDragDrop="True" AutoFilterCondition="Contains" />
																	<CellStyle CssClass="GridCss">
																	</CellStyle>
																</dx:GridViewDataTextColumn>

																<dx:GridViewDataTextColumn Caption="isSelected" FieldName="isSelected" Visible="False"
																	VisibleIndex="7">
																</dx:GridViewDataTextColumn>
																<dx:GridViewDataTextColumn Caption="Display Name" FieldName="DisplayName" VisibleIndex="4"
																	Visible="False">
																	<DataItemTemplate>
																		<dx:ASPxLabel ID="txtDisplayName" runat="server" Value='<%# Eval("DisplayName") %>'>
																		</dx:ASPxLabel>
																	</DataItemTemplate>
																	<HeaderStyle CssClass="GridCssHeader" />
																	<CellStyle CssClass="GridCss">
																	</CellStyle>
																</dx:GridViewDataTextColumn>
																<dx:GridViewDataTextColumn Caption="Required/Optional" FieldName="ServerRequired"
																	Visible="false" VisibleIndex="5">
																	<DataItemTemplate>
																		<dx:ASPxLabel ID="txtServerRequired" runat="server" Value='<%# Eval("ServerRequired") %>'>
																		</dx:ASPxLabel>
																	</DataItemTemplate>
																	<HeaderStyle CssClass="GridCssHeader" />
																	<CellStyle CssClass="GridCss">
																	</CellStyle>
																</dx:GridViewDataTextColumn>

																<dx:GridViewDataCheckColumn Caption="Monitored" FieldName="Monitored" VisibleIndex="6"
																	Visible="false">
																	<Settings AllowAutoFilter="False" />
																	<EditFormSettings Caption="Check the box if monitoring is required" />
																	<DataItemTemplate>
																		<dx:ASPxCheckBox ID="Monitored1" runat="server" Checked='<%# Eval("Monitored").ToString() == "True" ? true : false %>'
																			ReadOnly="true">
																		</dx:ASPxCheckBox>
																	</DataItemTemplate>
																	<Settings AllowAutoFilter="False" />
																	<EditFormSettings Caption="Check the box if monitoring is required" />
																	<EditCellStyle CssClass="GridCss1">
																	</EditCellStyle>
																	<EditFormCaptionStyle CssClass="GridCss1">
																	</EditFormCaptionStyle>
																	<HeaderStyle CssClass="GridCssHeader1" />
																	<CellStyle CssClass="GridCss1">
																	</CellStyle>
																</dx:GridViewDataCheckColumn>

																<dx:GridViewDataTextColumn Caption="True Display Name" FieldName="DisplayName" VisibleIndex="4"
																	Visible="False">
																	<DataItemTemplate>
																		<dx:ASPxLabel ID="txtDisplayName" runat="server" Value='<%# Eval("DisplayName") %>'>
																		</dx:ASPxLabel>
																	</DataItemTemplate>
																	<HeaderStyle CssClass="GridCssHeader" />
																	<CellStyle CssClass="GridCss">
																	</CellStyle>
																</dx:GridViewDataTextColumn>
																<dx:GridViewDataTextColumn Caption="True Service Name" FieldName="Service_Name" VisibleIndex="4"
																	Visible="False">
																	<DataItemTemplate>
																		<dx:ASPxLabel ID="txtDisplayName" runat="server" Value='<%# Eval("Service_Name") %>'>
																		</dx:ASPxLabel>
																	</DataItemTemplate>
																	<HeaderStyle CssClass="GridCssHeader" />
																	<CellStyle CssClass="GridCss">
																	</CellStyle>
																</dx:GridViewDataTextColumn>
																<%--<dx:GridViewDataTextColumn Caption="SVRID" FieldName="SVRId" VisibleIndex="8" 
                                                        Visible = "false">
                                                        <DataItemTemplate>
                                                            <dx:ASPxLabel ID="txtRequired" runat="server" Value='<%# Eval("SVRId") %>'>
                                                            </dx:ASPxLabel>
                                                        </DataItemTemplate>
                                                        <HeaderStyle CssClass="GridCssHeader" />
                                                        <CellStyle CssClass="GridCss">
                                                        </CellStyle>
                                                    </dx:GridViewDataTextColumn>  --%>
															</Columns>
															<SettingsBehavior ConfirmDelete="True" AllowSort="false" />
															<SettingsBehavior ConfirmDelete="True" />
															<SettingsPager AlwaysShowPager="True">
																<PageSizeItemSettings Visible="True">
																</PageSizeItemSettings>
															</SettingsPager>
															<SettingsText ConfirmDelete="Are you sure you want to delete this Profile?" />
															<SettingsText ConfirmDelete="Are you sure you want to delete this Profile?" />
															<Styles>
																<AlternatingRow CssClass="GridAltRow">
																</AlternatingRow>
															</Styles>
														</dx:ASPxGridView>
														<dx:ASPxHiddenField ID="ServiceMonitoredSelection" ClientInstanceName="ServiceMonitoredSelection"
															runat="server">
														</dx:ASPxHiddenField>
													</td>
												</tr>
												<tr>
													<td>
														<div id="Div6" style="display: block">
															&nbsp;</div>
														<dx:ASPxButton ID="WindowsCollapseAll" runat="server" Text="Collapse All" CssClass="sysButton"
															Wrap="False" OnClick="CollapseAllButton_Windows_Click">
															<Image Url="~/images/icons/forbidden.png">
															</Image>
														</dx:ASPxButton>
													</td>
												</tr>
												<tr>
													<td>
														<dx:ASPxTreeList ID="WindowsServerTreeList" KeyFieldName="Id" ParentFieldName="LocId"
															runat="server" AutoGenerateColumns="False" EnableTheming="True" Theme="Office2003Blue"
															Width="100%">
															<Columns>
																<dx:TreeListTextColumn Caption="Servers" FieldName="Name" Name="Servers" VisibleIndex="0">
																	<HeaderStyle CssClass="GridCssHeader" />
																	<CellStyle CssClass="GridCss">
																	</CellStyle>
																</dx:TreeListTextColumn>
																<dx:TreeListTextColumn FieldName="actid" Name="actid" Visible="False" VisibleIndex="1">
																</dx:TreeListTextColumn>
																<dx:TreeListTextColumn FieldName="tbl" Name="tbl" Visible="False" VisibleIndex="2">
																</dx:TreeListTextColumn>
																<dx:TreeListTextColumn FieldName="LocId" Name="LocId" Visible="False" VisibleIndex="3">
																</dx:TreeListTextColumn>
																<dx:TreeListTextColumn FieldName="ServerType" VisibleIndex="4">
																	<HeaderStyle CssClass="GridCssHeader" />
																	<CellStyle CssClass="GridCss">
																	</CellStyle>
																</dx:TreeListTextColumn>
																<dx:TreeListTextColumn FieldName="Description" VisibleIndex="5">
																	<HeaderStyle CssClass="GridCssHeader" />
																	<CellStyle CssClass="GridCss">
																	</CellStyle>
																</dx:TreeListTextColumn>
															</Columns>
															<Settings GridLines="Both" />
															<SettingsBehavior AllowDragDrop="False" AutoExpandAllNodes="True" />
															<SettingsPager Mode="ShowPager" PageSize="20">
																<PageSizeItemSettings Visible="True">
																</PageSizeItemSettings>
															</SettingsPager>
															<SettingsSelection AllowSelectAll="True" Enabled="True" Recursive="True" />
															<Styles>
																<AlternatingNode Enabled="True">
																</AlternatingNode>
															</Styles>
														</dx:ASPxTreeList>
													</td>
												</tr>
												<tr>
													<td>
														<div id="Div7" runat="server" style="display: none">
															&nbsp;
														</div>
														<div id="WindowsSuccess" runat="server" class="alert alert-success" style="display: none">
															Settings for selected servers were successully updated.
														</div>
														<div id="WindowsError" class="alert alert-danger" runat="server" style="display: none">
															Please select at least one Task and one Server in order to proceed.
														</div>
														<div id="Div8" class="alert alert-danger" runat="server" style="display: none">
															Settings for selected servers were NOT updated.
														</div>
														<div id="errorDiv8" class="alert alert-danger" runat="server" style="display: none">
													The servers do not exist.
												</div>
													</td>
												</tr>
											</table>
										</ContentTemplate>
										<%--<Triggers>
											<asp:AsyncPostBackTrigger ControlID="WindowsServerServicesApply" />
											<%--<asp:AsyncPostBackTrigger ControlID="ExchangeServerServicesClear" />--%>
										<%--	</Triggers>--%>
									</asp:UpdatePanel>
									<table>
										<tr>
											<td>
												<%--<dx:ASPxButton ID="WindowsServerServicesApply" runat="server" Text="Apply Setting(s)" 
                                                Theme="Office2010Blue" OnClick="WindowsServerServicesApply_Click">
                                            </dx:ASPxButton>--%>
												<dx:ASPxButton ID="WindowsServerServicesApply" runat="server" Text="Add Service(s)"
													CssClass="sysButton" OnClick="WindowsServerServicesApply_Click">
												</dx:ASPxButton>
											</td>
											<td>
												<dx:ASPxButton ID="ExchangeServerServicesClear" runat="server" Text="Remove Services(s)"
													CssClass="sysButton" OnClick="ExchangeServerServicesClear_Click">
												</dx:ASPxButton>
											</td>
										</tr>
									</table>
								</dx:ContentControl>
							</ContentCollection>
						</dx:TabPage>
						<dx:TabPage Text="Disk Settings">
							<TabImage Url="~/images/information.png">
							</TabImage>
							<ContentCollection>
								<dx:ContentControl ID="ContentControl1" runat="server" SupportsDisabledAttribute="True">
									<asp:UpdatePanel ID="UpdatePanel3" runat="server">
										<ContentTemplate>
											<table>
												<tr>
													<td>
														<table>
															<tr>
																<td>
																	<dx:ASPxRadioButtonList ID="SelCriteriaRadioButtonList" ClientInstanceName="diskSettingsRadioBtn"
																		runat="server" SelectedIndex="0" OnSelectedIndexChanged="SelCriteriaRadioButtonList_SelectedIndexChanged"
																		AutoPostBack="True">
																		<ClientSideEvents SelectedIndexChanged="function(s, e) { diskSettingsRadioBtnclicked(s); }" />
																		<Items>
																			<dx:ListEditItem Selected="True" Text="All Disks - By Percentage" Value="0" />
																			<dx:ListEditItem Text="All Disks - By GB" Value="3" />
																			<dx:ListEditItem Text="Selected Disks" Value="1" />
																			<dx:ListEditItem Text="No Disk Alerts" Value="2" />
																		</Items>
																	</dx:ASPxRadioButtonList>
																</td>
																<td>
																	<dx:ASPxTrackBar ID="AdvDiskSpaceThTrackBar" runat="server" AutoPostBack="True" CssFilePath="~/App_Themes/Office2010Black/{0}/styles.css"
																		CssPostfix="Office2010Black" EnableViewState="false" OnValueChanged="AdvDiskSpaceThTrackBar_ValueChanged"
																		Position="10" PositionStart="10" ScalePosition="LeftOrTop" SmallTickFrequency="5"
																		SpriteCssFilePath="~/App_Themes/Office2010Black/{0}/sprite.css" Step="1" Width="100%"
																		Theme="Office2010Blue">
																	</dx:ASPxTrackBar>
																	<dx:ASPxLabel ID="DiskLabel" runat="server" Visible="True" CssClass="lblsmallFont">
																	</dx:ASPxLabel>
																</td>
															</tr>
															<tr>
																<td>
																	&nbsp;
																</td>
															</tr>
															<tr>
																<td>
																	<dx:ASPxGridView ID="DiskGridView" ClientInstanceName="diskSettingsGrid" runat="server"
																		AutoGenerateColumns="False" Cursor="pointer" KeyFieldName="DiskName" Theme="Office2003Blue"
																		Visible="false">
																		<Columns>
																			<dx:GridViewDataTextColumn Caption="isSelected" FieldName="isSelected" VisibleIndex="9"
																				Visible="false">
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
																			<dx:GridViewDataTextColumn Caption="Disk Name" FieldName="DiskName" ReadOnly="true"
																				VisibleIndex="4" Width="100px">
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
																			<dx:GridViewDataTextColumn Caption="Free Space Threshold" FieldName="Threshold" VisibleIndex="6"
																				Width="100px">
																				<PropertiesTextEdit DisplayFormatString="g">
																					<ValidationSettings>
																						<RegularExpression ValidationExpression="(^(100(?:\.0{1,2})?))|(?!^0*$)(?!^0*\.0*$)^\d{1,2}(\.\d{1,2})?$" />
																					</ValidationSettings>
																				</PropertiesTextEdit>
																				<DataItemTemplate>
																					<dx:ASPxTextBox ID="txtFreeSpaceThresholdValue" runat="server" Width="150px" Value='<%# Eval("Threshold") %>'>
																					</dx:ASPxTextBox>
																				</DataItemTemplate>
																				<Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
																				<HeaderStyle CssClass="GridCssHeader" />
																				<CellStyle CssClass="GridCss2">
																				</CellStyle>
																			</dx:GridViewDataTextColumn>
																			<dx:GridViewDataComboBoxColumn Caption="Threshold Type (% or GB)" FieldName="ThresholdType"
																				VisibleIndex="8">
																				<DataItemTemplate>
																					<dx:ASPxComboBox ID="txtFreeSpaceThresholdType" runat="server" TextField="ThresholdType"
																						Value='<%# Eval("ThresholdType") %>' ValueField="ThresholdType" ValueType="System.String">
																						<Items>
																							<dx:ListEditItem Text="Percent" Value="Percent" />
																							<dx:ListEditItem Text="GB" Value="GB" />
																						</Items>
																					</dx:ASPxComboBox>
																				</DataItemTemplate>
																				<HeaderStyle CssClass="GridCssHeader" />
																				<CellStyle CssClass="GridCss">
																				</CellStyle>
																			</dx:GridViewDataComboBoxColumn>
																			<dx:GridViewCommandColumn Caption="Select" Visible="true" VisibleIndex="3" ShowSelectCheckbox="True">
																				<HeaderStyle CssClass="GridCssHeader" />
																				<CellStyle CssClass="GridCss1">
																				</CellStyle>
																			</dx:GridViewCommandColumn>
																		</Columns>
																		<SettingsPager PageSize="50" SEOFriendly="Enabled" Mode="ShowAllRecords">
																		</SettingsPager>
																		<SettingsEditing Mode="Inline">
																		</SettingsEditing>
																		<Images SpriteCssFilePath="~/App_Themes/Office2010Silver/{0}/sprite.css">
																			<LoadingPanelOnStatusBar Url="~/App_Themes/Office2010Silver/GridView/Loading.gif">
																			</LoadingPanelOnStatusBar>
																			<LoadingPanel Url="~/App_Themes/Office2010Silver/GridView/Loading.gif">
																			</LoadingPanel>
																		</Images>
																		<ImagesFilterControl>
																			<LoadingPanel Url="~/App_Themes/Office2010Silver/GridView/Loading.gif">
																			</LoadingPanel>
																		</ImagesFilterControl>
																		<Styles>
																			<Header ImageSpacing="5px" SortingImageSpacing="5px">
																			</Header>
																			<GroupRow Font-Bold="True">
																			</GroupRow>
																			<AlternatingRow CssClass="GridAltRow" Enabled="True">
																			</AlternatingRow>
																			<LoadingPanel ImageSpacing="5px">
																			</LoadingPanel>
																		</Styles>
																		<StylesEditors ButtonEditCellSpacing="0">
																			<ProgressBar Height="21px">
																			</ProgressBar>
																		</StylesEditors>
																	</dx:ASPxGridView>
																	<table>
																		<tr>
																			<td>
																				<dx:ASPxLabel ID="GBTitle" runat="server" Text="Current threshold:" CssClass="lblsmallFont"
																					Visible="false">
																				</dx:ASPxLabel>
																			</td>
																			<td>
																				<dx:ASPxTextBox ID="GBTextBox" runat="server" Width="170px" Visible="False">
																				</dx:ASPxTextBox>
																			</td>
																			<td>
																				<dx:ASPxLabel ID="GBLabel" runat="server" Text="GB free space" CssClass="lblsmallFont"
																					Visible="False">
																				</dx:ASPxLabel>
																			</td>
																		</tr>
																		<tr>
																			<td colspan="3">
																			</td>
																		</tr>
																	</table>
																</td>
															</tr>
														</table>
													</td>
												</tr>
												<tr>
													<td>
														<div id="Div1" style="display: block">
															&nbsp;</div>
														<dx:ASPxButton ID="DiskSettingsCollapseAll" runat="server" Text="Collapse All" CssClass="sysButton"
															Wrap="False" OnClick="CollapseAllButton_DiskSettings_Click">
															<Image Url="~/images/icons/forbidden.png">
															</Image>
														</dx:ASPxButton>
													</td>
												</tr>
												<tr>
													<td>
														<dx:ASPxTreeList ID="DiskSettingsTreeList" KeyFieldName="Id" ParentFieldName="LocId"
															runat="server" AutoGenerateColumns="False" EnableTheming="True" Theme="Office2003Blue"
															Width="100%">
															<Columns>
																<dx:TreeListTextColumn Caption="Servers" FieldName="Name" Name="Servers" ShowInCustomizationForm="True"
																	VisibleIndex="0">
																	<HeaderStyle CssClass="GridCssHeader" />
																	<CellStyle CssClass="GridCss">
																	</CellStyle>
																</dx:TreeListTextColumn>
																<dx:TreeListTextColumn FieldName="actid" Name="actid" ShowInCustomizationForm="True"
																	Visible="False" VisibleIndex="1">
																</dx:TreeListTextColumn>
																<dx:TreeListTextColumn FieldName="tbl" Name="tbl" ShowInCustomizationForm="True"
																	Visible="False" VisibleIndex="2">
																</dx:TreeListTextColumn>
																<dx:TreeListTextColumn FieldName="LocId" Name="LocId" ShowInCustomizationForm="True"
																	Visible="False" VisibleIndex="3">
																</dx:TreeListTextColumn>
																<dx:TreeListTextColumn FieldName="ServerType" ShowInCustomizationForm="True" VisibleIndex="4">
																	<HeaderStyle CssClass="GridCssHeader" />
																	<CellStyle CssClass="GridCss">
																	</CellStyle>
																</dx:TreeListTextColumn>
																<dx:TreeListTextColumn FieldName="Description" ShowInCustomizationForm="True" VisibleIndex="5">
																	<HeaderStyle CssClass="GridCssHeader" />
																	<CellStyle CssClass="GridCss">
																	</CellStyle>
																</dx:TreeListTextColumn>
															</Columns>
															<Settings GridLines="Both" />
															<SettingsBehavior AllowDragDrop="False" AutoExpandAllNodes="True" />
															<SettingsPager Mode="ShowPager" PageSize="20">
																<PageSizeItemSettings Visible="True">
																</PageSizeItemSettings>
															</SettingsPager>
															<SettingsSelection AllowSelectAll="True" Enabled="True" Recursive="True" />
															<Styles>
																<AlternatingNode Enabled="True">
																</AlternatingNode>
															</Styles>
														</dx:ASPxTreeList>
														<div id="Div2" runat="server" style="display: none">
															&nbsp;
														</div>
														<div id="Div3" runat="server" class="alert alert-success" style="display: none">
															Settings for selected servers were successully updated.
														</div>
														<div id="Div4" class="alert alert-danger" runat="server" style="display: none">
															Please select at least one Task and one Server in order to proceed.
														</div>
														<div id="Div5" class="alert alert-danger" runat="server" style="display: none">
															Settings for selected servers were NOT updated.
														</div>
													</td>
												</tr>
												<tr>
													<td>
														<div id="DiskSuccess" runat="server" class="alert alert-success" style="display: none">
															Settings for selected servers were successully updated.
														</div>
													</td>
												</tr>
												<tr>
													<td>
														<div id="DiskError" class="alert alert-danger" runat="server" style="display: none">
															Please select at least one Task and one Server in order to proceed.
														</div>
													</td>
												</tr>
											</table>
										</ContentTemplate>
										<%--	<Triggers>
											<asp:AsyncPostBackTrigger ControlID="DiskSettingsApply" />
											<asp:AsyncPostBackTrigger ControlID="DiskSettingsClear" />
										</Triggers>--%>
									</asp:UpdatePanel>
									<table>
										<tr>
											<td>
												<dx:ASPxButton ID="DiskSettingsApply" runat="server" Text="Apply Settings(s)" CssClass="sysButton"
													OnClick="DiskSettingsApply_Click">
												</dx:ASPxButton>
											</td>
											<td>
												<dx:ASPxButton ID="DiskSettingsClear" runat="server" Text="Remove Disk Monitoring"
													ClientInstanceName="CleardiskSettings" CssClass="sysButton" ClientVisible="false"
													OnClick="DiskSettingsClear_Click">
												</dx:ASPxButton>
											</td>
										</tr>
									</table>
								</dx:ContentControl>
							</ContentCollection>
						</dx:TabPage>
						<dx:TabPage Text="Server Locations">
							<TabImage Url="~/images/information.png">
							</TabImage>
							<ContentCollection>
								<dx:ContentControl ID="ContentControl3" runat="server" SupportsDisabledAttribute="True">
									<asp:UpdatePanel ID="UpdatePanel5" runat="server">
										<ContentTemplate>
											<table>
												<tr>
													<td>
														<dx:ASPxButton ID="LocationsCollapseAll" runat="server" Text="Collapse All" CssClass="sysButton"
															Wrap="False" OnClick="CollapseAllButton_ServerLocations_Click">
															<Image Url="~/images/icons/forbidden.png">
															</Image>
														</dx:ASPxButton>
													</td>
												</tr>
												<tr>
													<td>
														<dx:ASPxTreeList ID="ServerLocationsTreeList" KeyFieldName="Id" ParentFieldName="LocId"
															runat="server" AutoGenerateColumns="False" EnableTheming="True" Theme="Office2003Blue"
															Width="100%">
															<Columns>
																<dx:TreeListTextColumn Caption="Servers" FieldName="Name" Name="Servers" ShowInCustomizationForm="True"
																	VisibleIndex="0">
																	<HeaderStyle CssClass="GridCssHeader" />
																	<CellStyle CssClass="GridCss">
																	</CellStyle>
																</dx:TreeListTextColumn>
																<dx:TreeListTextColumn FieldName="actid" Name="actid" ShowInCustomizationForm="True"
																	Visible="False" VisibleIndex="1">
																</dx:TreeListTextColumn>
																<dx:TreeListTextColumn FieldName="tbl" Name="tbl" ShowInCustomizationForm="True"
																	Visible="False" VisibleIndex="2">
																</dx:TreeListTextColumn>
																<dx:TreeListTextColumn FieldName="LocId" Name="LocId" ShowInCustomizationForm="True"
																	Visible="False" VisibleIndex="3">
																</dx:TreeListTextColumn>
																<dx:TreeListTextColumn FieldName="ServerType" ShowInCustomizationForm="True" VisibleIndex="4">
																	<HeaderStyle CssClass="GridCssHeader" />
																	<CellStyle CssClass="GridCss">
																	</CellStyle>
																</dx:TreeListTextColumn>
																<dx:TreeListTextColumn FieldName="Description" ShowInCustomizationForm="True" VisibleIndex="5">
																	<HeaderStyle CssClass="GridCssHeader" />
																	<CellStyle CssClass="GridCss">
																	</CellStyle>
																</dx:TreeListTextColumn>
															</Columns>
															<Settings GridLines="Both" />
															<SettingsBehavior AllowDragDrop="False" AutoExpandAllNodes="True" />
															<SettingsPager Mode="ShowPager" PageSize="50">
																<PageSizeItemSettings Visible="True">
																</PageSizeItemSettings>
															</SettingsPager>
															<SettingsSelection AllowSelectAll="True" Enabled="True" Recursive="True" />
															<Styles>
																<AlternatingNode Enabled="True">
																</AlternatingNode>
															</Styles>
														</dx:ASPxTreeList>
													</td>
												</tr>
											</table>
										</ContentTemplate>
									</asp:UpdatePanel>
									<table>
										<tr>
											<td>
												<div id="LocationSuccess" runat="server" class="alert alert-success" style="display: none">
													Settings for selected servers were successully updated.
												</div>
												<div id="LocationError" class="alert alert-danger" runat="server" style="display: none">
													Please select at least one Task and one Server in order to proceed.
												</div>
											</td>
										</tr>
										<tr>
											<td>
												<table>
													<tr>
														<td>
															<asp:Label ID="Label4" runat="server" Text="Location:" CssClass="lblsmallFont"></asp:Label>
														</td>
														<td>
															<dx:ASPxComboBox ID="ServerLocation" runat="server" ValueType="System.String" AutoPostBack="True">
																<ValidationSettings ErrorDisplayMode="ImageWithTooltip">
																	<RequiredField ErrorText="Select Location." />
																	<RequiredField ErrorText="Select Location."></RequiredField>
																</ValidationSettings>
															</dx:ASPxComboBox>
														</td>
														<td>
															<dx:ASPxButton ID="ServerLocationsApply" runat="server" Text="Assign Location" CssClass="sysButton"
																OnClick="ServerLocationsApply_Click">
															</dx:ASPxButton>
														</td>
													</tr>
												</table>
											</td>
										</tr>
									</table>
								</dx:ContentControl>
							</ContentCollection>
						</dx:TabPage>
						<dx:TabPage Text="Credentials">
							<TabImage Url="~/images/information.png">
							</TabImage>
							<ContentCollection>
								<dx:ContentControl ID="ContentControl4" runat="server" SupportsDisabledAttribute="True">
									<asp:UpdatePanel ID="UpdatePanel6" runat="server">
										<ContentTemplate>
											<table>
												<tr>
													<td>
														<dx:ASPxButton ID="CredentialsCollapseAll" runat="server" Text="Collapse All" CssClass="sysButton"
															Wrap="False" OnClick="CollapseAllButton_ServerCredentials_Click">
															<Image Url="~/images/icons/forbidden.png">
															</Image>
														</dx:ASPxButton>
													</td>
												</tr>
												<tr>
													<td>
														<dx:ASPxTreeList ID="ServerCredentialsTreeList" KeyFieldName="Id" ParentFieldName="LocId"
															runat="server" AutoGenerateColumns="False" EnableTheming="True" Theme="Office2003Blue"
															Width="100%">
															<Columns>
																<dx:TreeListTextColumn Caption="Servers" FieldName="Name" Name="Servers" ShowInCustomizationForm="True"
																	VisibleIndex="0">
																	<HeaderStyle CssClass="GridCssHeader" />
																	<CellStyle CssClass="GridCss">
																	</CellStyle>
																</dx:TreeListTextColumn>
																<dx:TreeListTextColumn FieldName="actid" Name="actid" ShowInCustomizationForm="True"
																	Visible="False" VisibleIndex="1">
																</dx:TreeListTextColumn>
																<dx:TreeListTextColumn FieldName="tbl" Name="tbl" ShowInCustomizationForm="True"
																	Visible="False" VisibleIndex="2">
																</dx:TreeListTextColumn>
																<dx:TreeListTextColumn FieldName="LocId" Name="LocId" ShowInCustomizationForm="True"
																	Visible="False" VisibleIndex="3">
																</dx:TreeListTextColumn>
																<dx:TreeListTextColumn FieldName="ServerType" ShowInCustomizationForm="True" VisibleIndex="4">
																	<HeaderStyle CssClass="GridCssHeader" />
																	<CellStyle CssClass="GridCss">
																	</CellStyle>
																</dx:TreeListTextColumn>
																<dx:TreeListTextColumn FieldName="Description" ShowInCustomizationForm="True" VisibleIndex="5">
																	<HeaderStyle CssClass="GridCssHeader" />
																	<CellStyle CssClass="GridCss">
																	</CellStyle>
																</dx:TreeListTextColumn>
															</Columns>
															<Settings GridLines="Both" />
															<SettingsBehavior AllowDragDrop="False" AutoExpandAllNodes="True" />
															<SettingsPager Mode="ShowPager" PageSize="20">
																<PageSizeItemSettings Visible="True">
																</PageSizeItemSettings>
															</SettingsPager>
															<SettingsSelection AllowSelectAll="True" Enabled="True" Recursive="True" />
															<Styles>
																<AlternatingNode Enabled="True">
																</AlternatingNode>
															</Styles>
														</dx:ASPxTreeList>
													</td>
												</tr>
											</table>
										</ContentTemplate>
									</asp:UpdatePanel>
									<table>
										<tr>
											<td>
												<div id="CredentialsSuccess" runat="server" class="alert alert-success" style="display: none">
													Settings for selected servers were successully updated.
												</div>
												<div id="CredentialsError" class="alert alert-danger" runat="server" style="display: none">
													Please select at least one Task and one Server in order to proceed.
												</div>
											</td>
										</tr>
										<tr>
											<td>
												<table>
													<tr>
														<td>
															<asp:Label ID="Label5" runat="server" Text="Credential:" CssClass="lblsmallFont"></asp:Label>
														</td>
														<td>
															<dx:ASPxComboBox ID="ServerCredentials" runat="server" ValueType="System.String"
																AutoPostBack="false">
																<ValidationSettings ErrorDisplayMode="ImageWithTooltip">
																	<RequiredField ErrorText="Select Credentials." />
																	<RequiredField ErrorText="Select Credentials."></RequiredField>
																</ValidationSettings>
															</dx:ASPxComboBox>
														</td>
														<td>
															<dx:ASPxButton ID="ServerCredentialsApply" runat="server" Text="Assign Credentials"
																CssClass="sysButton" OnClick="ServerCredentialsApply_Click">
															</dx:ASPxButton>
														</td>
													</tr>
												</table>
											</td>
										</tr>
									</table>
								</dx:ContentControl>
							</ContentCollection>
						</dx:TabPage>
						<dx:TabPage Text="Business Hours">
							<TabImage Url="~/images/information.png">
							</TabImage>
							<ContentCollection>
								<dx:ContentControl ID="ContentControl5" runat="server" SupportsDisabledAttribute="True">
									<table>
										<tr>
											<td>
												<div id="BusinessHoursSuccess" runat="server" class="alert alert-success" style="display: none">
													Hours for selected servers were successully updated.
												</div>
												<div id="BusinessHoursError" class="alert alert-danger" runat="server" style="display: none">
													Please select at least one Hours Definition and one Server in order to proceed.
												</div>
											</td>
										</tr>
									</table>
									<asp:UpdatePanel ID="UpdatePanel7" runat="server">
										<ContentTemplate>
											<table>
												<tr>
													<td>
														<dx:ASPxButton ID="ASPxButton1" runat="server" Text="Collapse All" CssClass="sysButton"
															Wrap="False" OnClick="CollapseAllButton_ServerLocations_Click">
															<Image Url="~/images/icons/forbidden.png">
															</Image>
														</dx:ASPxButton>
													</td>
												</tr>
												<tr>
													<td>
														<dx:ASPxTreeList ID="BusinessHoursTreeList" KeyFieldName="Id" ParentFieldName="LocId"
															runat="server" AutoGenerateColumns="False" EnableTheming="True" Theme="Office2003Blue">
															<Columns>
																<dx:TreeListTextColumn Caption="Servers" FieldName="Name" Name="Servers" ShowInCustomizationForm="True"
																	VisibleIndex="0">
																	<HeaderStyle CssClass="GridCssHeader" />
																	<CellStyle CssClass="GridCss">
																	</CellStyle>
																</dx:TreeListTextColumn>
																<dx:TreeListTextColumn FieldName="actid" Name="actid" ShowInCustomizationForm="True"
																	Visible="False" VisibleIndex="1">
																</dx:TreeListTextColumn>
																<dx:TreeListTextColumn FieldName="tbl" Name="tbl" ShowInCustomizationForm="True"
																	Visible="False" VisibleIndex="2">
																</dx:TreeListTextColumn>
																<dx:TreeListTextColumn FieldName="LocId" Name="LocId" ShowInCustomizationForm="True"
																	Visible="False" VisibleIndex="3">
																</dx:TreeListTextColumn>
																<dx:TreeListTextColumn FieldName="ServerType" ShowInCustomizationForm="True" VisibleIndex="4">
																	<HeaderStyle CssClass="GridCssHeader" />
																	<CellStyle CssClass="GridCss">
																	</CellStyle>
																</dx:TreeListTextColumn>
																<dx:TreeListTextColumn FieldName="Description" ShowInCustomizationForm="True" VisibleIndex="5">
																	<HeaderStyle CssClass="GridCssHeader" />
																	<CellStyle CssClass="GridCss">
																	</CellStyle>
																</dx:TreeListTextColumn>
																<dx:TreeListTextColumn FieldName="Businesshours" ShowInCustomizationForm="True" VisibleIndex="5"
																	Caption="Business Hours">
																	<HeaderStyle CssClass="GridCssHeader" />
																	<CellStyle CssClass="GridCss">
																	</CellStyle>
																</dx:TreeListTextColumn>
															</Columns>
															<Settings GridLines="Both" />
															<SettingsBehavior AllowDragDrop="False" AutoExpandAllNodes="True" />
															<SettingsPager Mode="ShowPager" PageSize="50">
																<PageSizeItemSettings Visible="True">
																</PageSizeItemSettings>
															</SettingsPager>
															<SettingsSelection AllowSelectAll="True" Enabled="True" Recursive="True" />
															<Styles>
																<AlternatingNode Enabled="True">
																</AlternatingNode>
															</Styles>
														</dx:ASPxTreeList>
													</td>
												</tr>
											</table>
										</ContentTemplate>
									</asp:UpdatePanel>
									<table>
										<tr>
											<td>
												<table>
													<tr>
														<td>
															<asp:Label ID="Label6" runat="server" Text="Hours:" CssClass="lblsmallFont"></asp:Label>
														</td>
														<td>
															<dx:ASPxComboBox ID="BusinessHoursComboBox" runat="server" ValueType="System.String"
																AutoPostBack="false">
																<ValidationSettings ErrorDisplayMode="ImageWithTooltip">
																	<RequiredField ErrorText="Select Hours." />
																	<RequiredField ErrorText="Select Hours."></RequiredField>
																</ValidationSettings>
															</dx:ASPxComboBox>
														</td>
														<td>
															<dx:ASPxButton ID="ASPxButton2" runat="server" Text="Assign Hours" CssClass="sysButton"
																OnClick="BusinessHoursApply_Click">
															</dx:ASPxButton>
														</td>
													</tr>
												</table>
											</td>
										</tr>
									</table>
								</dx:ContentControl>
							</ContentCollection>
						</dx:TabPage>
					</TabPages>
				</dx:ASPxPageControl>
			</td>
		</tr>
	</table>
	<script language="javascript">
		window.onload = function () {
			cmbServerType = document.getElementById("ContentPlaceHolder1_ASPxPageControl1_ServerTypeComboBox_I").value;
			if (cmbServerType != 'Exchange') {
				cmbRole.SetVisible(false);
				document.getElementById("ContentPlaceHolder1_ASPxPageControl1_ExchangeRolesLabel").style.visibility = 'hidden';
			}
		};
  
	</script>
	</asp:Content>
	

