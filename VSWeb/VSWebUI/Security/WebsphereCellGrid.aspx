<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true"
	CodeBehind="WebsphereCellGrid.aspx.cs" Inherits="VSWebUI.Configurator.WebsphererCellGrid" %>
	<%@ MasterType virtualpath="~/Site1.Master" %>
<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.Web" TagPrefix="dx" %>

<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.Web" TagPrefix="dx1" %>
    <%@ Register Assembly="DevExpress.Web.ASPxTreeList.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.Web.ASPxTreeList" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
	<link href='http://fonts.googleapis.com/css?family=Francois One' rel='stylesheet'
		type='text/css' />
	<link rel="stylesheet" type="text/css" href="../css/vswebforms.css" />
	<script src="../js/jquery-1.9.1.js" type="text/javascript"></script>
	<script src="../js/bootstrap.min.js" type="text/javascript"></script>
	<script type="text/javascript">
		function DoCallback() {
			eventsTree.PerformCallback();
			webspherecellgrid.PerformCallback();
		}
		$(document).ready(function () {
			$('.alert-success').delay(10000).fadeOut("slow", function () {
			});
		});
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
		//10/30/2013 NS added - fix for when an Enter key is pressed within the text box on the page - redirect the
		//submit function to the actual Go button on the page instead of performing a whole page submit
		function OnKeyDown(s, e) {
			//alert(window.event.keyCode);
			//var keyCode = (window.event) ? e.which : e.keyCode;
			//alert(keyCode);
			var keyCode = window.event.keyCode;
			if (keyCode == 13)
				goButton.DoClick();
		}



		function OnItemClick(s, e) {
			if (e.item.parent == s.GetRootItem())
				e.processOnServer = false;
		}
		var visibleIndex;
		function OnCustomButtonClick(s, e) {

			visibleIndex = e.visibleIndex;
			if (e.buttonID == "deleteButton")


				webspherecellgrid.GetRowValues(e.visibleIndex, 'Name', OnGetRowValues);
			function OnGetRowValues(values) {
				var id = values[0];
				var name = values[1];

				var OK = (confirm('Are you sure you want to delete WebSphere Cell- ' + values + '?'))
				if (OK == true) {

					webspherecellgrid.DeleteRow(visibleIndex);

				}


			}
		}
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
	<div id="pnlAreaDtls" style="height: 100%; width: 100%; visibility: hidden" runat="server"
		class="pnlDetails12">
		<table align="center" width="30%" style="height: 100%">
			<tr>
				<td align="center" valign="middle" style="height: auto;">
					<table border="1" cellspacing="0" class="csline" cellpadding="2px" id="table_txt_edit"
						style="border-width: 1px; border-style: solid; border-collapse: collapse; border-color: silver;
						background-color: #F8F8FF" width="100%">
						<tr style="background-color: White">
							<td align="left">
								<div class="subheading">
									Delete Server</div>
							</td>
						</tr>
						<tr>
							<td align="center">
								<table>
									<tr>
										<td valign="top">
										</td>
										<td align="center">
											<div style="overflow: auto; height: 60px; font-size: 12px; font-weight: normal; font-family: Arial, Helvetica, sans-serif;
												text-align: left; color: black; width: 350px;" id="divmsg" runat="server">
											</div>
											<asp:Button ID="btnok1" runat="server" OnClick="btn_OkClick" OnClientClick="hidepopup()"
												Text="OK" Width="50px" Font-Names="Arial" Font-Size="Small" UseSubmitBehavior="false" />
											<asp:Button ID="btncancel1" runat="server" OnClick="btn_CancelClick" OnClientClick="hidepopup()"
												Text="Cancel" Width="70px" Font-Names="Arial" Font-Size="Small"  UseSubmitBehavior="false"/>
										</td>
									</tr>
								</table>
							</td>
						</tr>
					</table>
				</td>
			</tr>
		</table>
	</div>
	<table>
		<tr>
			<td>
				<div class="header" id="servernamelbldisp" runat="server">
					WebSphere Cell Configuration</div>
			</td>
		</tr>
        <tr>
            <td>
                <div class="info" id="importDiv">Create a WebSphere cell using the 'New' action button. After the cell shows up in the grid, 
                click the 'Refresh Nodes' icon in the corresponding row in order to load the available WebSphere Nodes for the server.
                Select the desired nodes from the grid and click 'Next' to configure the monitoring defaults. Once the defaults are entered,
                click 'Done' to complete the import process.</div>
            </td>
        </tr>
		<tr>
			<td>
				<table>
					<%--<tr>
						<td>
							<dx:ASPxButton ID="SendTestAlertButton" runat="server" Text="Send Test Alert" Theme="Office2010Blue"
								Wrap="False" OnClick="SendTestAlertButton_Click">
							</dx:ASPxButton>
							<dx:ASPxLabel ID="TestAlertSubmittedLabel" runat="server" Text="ASPxLabel" ClientInstanceName="TestAlertSubmittedLabel"
								Visible="False" Wrap="False">
							</dx:ASPxLabel>
						</td>
					</tr>--%>
					<tr>
						<td>
							<div id="successDiv" runat="server" class="alert alert-success" style="display: none">
								Success.
							</div>
							<div id="errorDiv" runat="server" class="alert alert-danger" style="display: none">
								Error.
							</div>
                            <dx:ASPxButton ID="NewButton" runat="server" Text="New" CssClass="sysButton" 
                                onclick="NewButton_Click">
                                <Image Url="~/images/icons/add.png">
                                            </Image>
                            </dx:ASPxButton>
							<%--<div class="info">
								The list of available alerts is displayed below. In order to add new alerts or configure
								existing alerts, please use the buttons in the 'Actions' column.
							</div>--%>
						</td>
					</tr>
					<tr>
						<td>
							<%--	<dx:ASPxButton ID="ImportDominoServers" runat="server" 
                    CssFilePath="~/App_Themes/Office2010Blue/{0}/styles.css" 
                    CssPostfix="Office2010Blue" 
                    SpriteCssFilePath="~/App_Themes/Office2010Blue/{0}/sprite.css" 
                    Text="Import Websphere Cell" Wrap="False">
                </dx:ASPxButton>

                            <br />
                            <br />--%>
							<dx:ASPxGridView ID="webspherecellgrid" runat="server" AutoGenerateColumns="False"
								KeyFieldName="CellID" OnHtmlDataCellPrepared="webspherecellgrid_HtmlDataCellPrepared"
								OnHtmlRowCreated="webspherecellgrid_HtmlRowCreated" OnRowDeleting="webspherecellgrid_RowDeleting"
								Width="100%" OnPageSizeChanged="webspherecellgrid_PageSizeChanged" EnableTheming="True" 
								Theme="Office2003Blue" ClientInstanceName="webspherecellgrid" onfocusedrowchanged="webspherecellgrid_FocusedRowChanged" 
								
								 EnableCallBacks="true" >
								<ClientSideEvents CustomButtonClick="OnCustomButtonClick" />
								<Columns>
									<dx:GridViewCommandColumn ButtonType="Image" Caption="Actions" VisibleIndex="0" Width="70px">
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
										<CancelButton Visible="true">
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
										<HeaderStyle CssClass="GridCssHeader">
											<Paddings Padding="5px" />
										</HeaderStyle>
									</dx:GridViewCommandColumn>
								    <dx:GridViewCommandColumn Caption="Delete" ButtonType="Image"  Width="50px" VisibleIndex="1">
                        <CustomButtons>
                            <dx:GridViewCommandColumnCustomButton ID="deleteButton" Text="Delete" 
                                Image-Url="../images/delete.png" >
<Image Url="../images/delete.png"></Image>
                            </dx:GridViewCommandColumnCustomButton>
                        </CustomButtons>
						<HeaderStyle CssClass="GridCssHeader" />
                    </dx:GridViewCommandColumn>
                                    <dx:GridViewDataTextColumn Caption="Refresh Nodes" VisibleIndex="2" CellStyle-HorizontalAlign="Center" Width="100px">	
				<DataItemTemplate>
					
					<asp:ImageButton ID="btnserverattributes" runat="server" ImageUrl="~/Images/icons/reset.png"
						Width="15px" Height="15px" CommandName="Delete" CommandArgument='<%#Eval("CellID") %>' 
						 ToolTip="refresh" OnClick="btn_Clickeditserver"  />
				</DataItemTemplate>
				<EditFormSettings Visible="False" />
				<EditCellStyle CssClass="GridCss">
				</EditCellStyle>
				<EditFormCaptionStyle CssClass="GridCss">
				</EditFormCaptionStyle>
				<HeaderStyle CssClass="GridCssHeader" />
				<CellStyle CssClass="GridCss1">
				</CellStyle>
				<CellStyle HorizontalAlign="Center">
				</CellStyle>
			</dx:GridViewDataTextColumn>
									<dx:GridViewDataTextColumn Caption="Cell Name" VisibleIndex="3" 
                                        FieldName="CellName">
										<Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
										<EditCellStyle CssClass="GridCss">
										</EditCellStyle>
										<EditFormCaptionStyle CssClass="GridCss">
										</EditFormCaptionStyle>
										<HeaderStyle CssClass="GridCssHeader" Wrap="True">
											<Paddings Padding="5px" />
										</HeaderStyle>
										<CellStyle CssClass="GridCss">
										</CellStyle>
									</dx:GridViewDataTextColumn>
									<dx:GridViewDataTextColumn Caption="Name" VisibleIndex="4" 
                                        FieldName="Name">
										<Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
										<EditCellStyle CssClass="GridCss">
										</EditCellStyle>
										<EditFormCaptionStyle CssClass="GridCss">
										</EditFormCaptionStyle>
										<HeaderStyle CssClass="GridCssHeader" Wrap="True">
											<Paddings Padding="5px" />
										</HeaderStyle>
										<CellStyle CssClass="GridCss">
										</CellStyle>
									</dx:GridViewDataTextColumn>
									<dx:GridViewDataTextColumn Caption="Host Name" VisibleIndex="5" 
                                        FieldName="HostName">
										<Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
										<EditCellStyle CssClass="GridCss">
										</EditCellStyle>
										<EditFormCaptionStyle CssClass="GridCss">
										</EditFormCaptionStyle>
										<HeaderStyle CssClass="GridCssHeader" Wrap="True">
											<Paddings Padding="5px" />
										</HeaderStyle>
										<CellStyle CssClass="GridCss">
										</CellStyle>
									</dx:GridViewDataTextColumn>
									<dx:GridViewDataTextColumn Caption="Connection Type" VisibleIndex="6" 
                                        FieldName="ConnectionType">
										<Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
										<EditCellStyle CssClass="GridCss">
										</EditCellStyle>
										<EditFormCaptionStyle CssClass="GridCss">
										</EditFormCaptionStyle>
										<HeaderStyle CssClass="GridCssHeader" Wrap="True">
											<Paddings Padding="5px" />
										</HeaderStyle>
										<CellStyle CssClass="GridCss">
										</CellStyle>
									</dx:GridViewDataTextColumn>
									<dx:GridViewDataTextColumn Caption="Port No." VisibleIndex="7" 
                                        FieldName="PortNo">
										<Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
										<EditCellStyle CssClass="GridCss">
										</EditCellStyle>
										<EditFormCaptionStyle CssClass="GridCss">
										</EditFormCaptionStyle>
										<HeaderStyle CssClass="GridCssHeader" Wrap="True">
											<Paddings Padding="5px" />
										</HeaderStyle>
										<CellStyle CssClass="GridCss">
										</CellStyle>
									</dx:GridViewDataTextColumn>
									<dx:GridViewDataCheckColumn Caption="Global Security" VisibleIndex="8" Width="60px"
										FieldName="GlobalSecurity">
										<Settings AllowAutoFilter="False" />
										<EditCellStyle CssClass="GridCss">
										</EditCellStyle>
										<EditFormCaptionStyle CssClass="GridCss">
										</EditFormCaptionStyle>
										<HeaderStyle CssClass="GridCssHeader" Wrap="True">
											<Paddings Padding="5px" />
										</HeaderStyle>
										<CellStyle CssClass="GridCss1">
											<Paddings Padding="5px" />
										</CellStyle>
									</dx:GridViewDataCheckColumn>
									<dx:GridViewDataTextColumn Caption="Credentials" VisibleIndex="9" FieldName="AliasName">
										<Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
										<EditCellStyle CssClass="GridCss">
										</EditCellStyle>
										<EditFormCaptionStyle CssClass="GridCss">
										</EditFormCaptionStyle>
										<HeaderStyle CssClass="GridCssHeader" Wrap="True">
											<Paddings Padding="5px" />
										</HeaderStyle>
										<CellStyle CssClass="GridCss">
										</CellStyle>
									</dx:GridViewDataTextColumn>
									<%--<dx:GridViewDataComboBoxColumn FieldName="CredentialsID" VisibleIndex="4" UnboundType="String">
										<PropertiesComboBox TextField="AliasName" ValueField="ID">
											
										</PropertiesComboBox>
										<Settings AllowAutoFilter="True" AllowHeaderFilter="False" />
										<EditCellStyle CssClass="GridCss">
										</EditCellStyle>
										<EditFormCaptionStyle CssClass="GridCss">
										</EditFormCaptionStyle>
										<HeaderStyle CssClass="GridCssHeader" />
										<CellStyle CssClass="GridCss">
										</CellStyle>
									</dx:GridViewDataComboBoxColumn>--%>
									<dx:GridViewDataTextColumn Caption="Realm" VisibleIndex="10" FieldName="Realm">
										<Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
										<EditCellStyle CssClass="GridCss">
										</EditCellStyle>
										<EditFormCaptionStyle CssClass="GridCss">
										</EditFormCaptionStyle>
										<HeaderStyle CssClass="GridCssHeader" Wrap="True">
											<Paddings Padding="5px" />
										</HeaderStyle>
										<CellStyle CssClass="GridCss">
										</CellStyle>
									</dx:GridViewDataTextColumn>
									
								</Columns>
								<SettingsBehavior ConfirmDelete="True" ColumnResizeMode="NextColumn" />
								<SettingsBehavior AllowFocusedRow="True" AllowSelectByRowClick="True" ProcessSelectionChangedOnServer="True"  />
								<Settings ShowFilterRow="True" />
								<SettingsText ConfirmDelete="Are you sure you want to delete this cell?  All of the servers associated with this cell will also be deleted." />
								<Styles>
									<AlternatingRow CssClass="GridAltRow" Enabled="True">
									</AlternatingRow>
									<LoadingPanel ImageSpacing="5px">
									</LoadingPanel>
									<Header ImageSpacing="5px" SortingImageSpacing="5px">
									</Header>
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
                    <tr>
									<td>
										<asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
											<ContentTemplate>
												<table>
													<tr>
														<td>
															<dx:ASPxButton ID="CollapseAllButton" ClientInstanceName="collapseAll" runat="server"
																Text="Collapse All" CssClass="sysButton" Wrap="False" EnableClientSideAPI="False"
																OnClick="CollapseAllButton_Click" Visible="false">
																<Image Url="~/images/icons/forbidden.png">
																</Image>
															</dx:ASPxButton>
														</td>
													</tr>
													<tr>
														<td valign="top">
															<dx:ASPxTreeList ID="NodesTreeList" ClientInstanceName="eventsTree" runat="server"
																AutoGenerateColumns="False" CssClass="lblsmallFont" KeyFieldName="Id" ParentFieldName="SrvId"
																Theme="Office2003Blue" Width="100%" Visible="false">
																<Columns>
																	<dx:TreeListTextColumn Caption="NodeName  " FieldName="Name" Name="NodeName" VisibleIndex="0"
																		ReadOnly="True">
																		<EditFormSettings Visible="False" />
																		<HeaderStyle CssClass="lblsmallFont" />
																	</dx:TreeListTextColumn>
																	 <dx:TreeListTextColumn FieldName="actid" Name="actid" Visible="False" 
                                                                            VisibleIndex="1">
                                                                        </dx:TreeListTextColumn>
                                                                        <dx:TreeListTextColumn FieldName="tbl" Name="tbl" Visible="False" 
                                                                            VisibleIndex="2">
                                                                        </dx:TreeListTextColumn>
                                                                        <dx:TreeListTextColumn FieldName="SrvId" Name="SrvId" Visible="False" 
                                                                            VisibleIndex="3">
                                                                        </dx:TreeListTextColumn>
                                                                        <dx:TreeListTextColumn FieldName="Status" Name="Status"  VisibleIndex="4">
													     </dx:TreeListTextColumn>
														<dx:TreeListTextColumn FieldName="HostName" Name="HostName"  VisibleIndex="5">
														</dx:TreeListTextColumn>
																	<%--<dx:TreeListTextColumn FieldName="NodeName" Name="NodeName" Visible="false" VisibleIndex="2">
																	</dx:TreeListTextColumn>
																	<dx:TreeListTextColumn FieldName="serverName" Name="serverName" Visible="false" VisibleIndex="3">
																	</dx:TreeListTextColumn>--%>
																</Columns>
																<Styles CssFilePath="~/App_Themes/Office2010Blue/{0}/styles.css" CssPostfix="Office2010Blue">
																	<Header CssClass="GridCssHeader">
																	</Header>
																	<Cell CssClass="GridCss">
																	</Cell>
																	<LoadingPanel ImageSpacing="5px">
																	</LoadingPanel>
																	<AlternatingNode CssClass="GridAltRow" Enabled="True">
																	</AlternatingNode>
																</Styles>
																<Settings GridLines="Both" />
																<SettingsBehavior AllowDragDrop="False" AutoExpandAllNodes="True" />
																<SettingsPager AlwaysShowPager="True" Mode="ShowPager" PageSize="20">
																	<PageSizeItemSettings Visible="True">
																	</PageSizeItemSettings>
																</SettingsPager>
																<SettingsSelection AllowSelectAll="True" Recursive="True" Enabled="True" />
																<SettingsEditing Mode="Inline"></SettingsEditing>
																<Styles>
																	<LoadingPanel ImageSpacing="5px">
																	</LoadingPanel>
																	<AlternatingNode Enabled="True">
																	</AlternatingNode>
																</Styles>
																<StylesPager>
																	<PageNumber ForeColor="#3E4846">
																	</PageNumber>
																	<Summary ForeColor="#1E395B">
																	</Summary>
																</StylesPager>
																<StylesEditors ButtonEditCellSpacing="0">
																</StylesEditors>
															</dx:ASPxTreeList>
														</td>
													</tr>
												</table>
											</ContentTemplate>
											<Triggers>
												<asp:AsyncPostBackTrigger ControlID="CollapseAllButton" />
											</Triggers>
										</asp:UpdatePanel>
									</td>
								</tr>
                                <tr><td><dx:ASPxButton ID="ImportButton" runat="server" Text="Next" CssClass="sysButton"
                                OnClick="ImportButton_Click" Visible="false">
                                        </dx:ASPxButton>
                                </td></tr>
                    <%--<tr><td><dx:ASPxGridView ID="WebsphereNodesgridview" runat="server" 
                                        AutoGenerateColumns="False" EnableCallBacks="False" EnableTheming="True" 
                                        KeyFieldName="NodeID" visible="true"
                                        
                                        Theme="Office2003Blue" Width="100%">
                                        <Columns>
                                            <dx:GridViewDataTextColumn Caption="Node Name" FieldName="NodeName" 
                                                ShowInCustomizationForm="True" VisibleIndex="0">
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="NodeID" FieldName="NodeID" 
                                                ShowInCustomizationForm="True" Visible="False" VisibleIndex="1">
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="JVM's" FieldName="JVMs" 
                                                ShowInCustomizationForm="True" VisibleIndex="3">
                                                <HeaderStyle CssClass="GridCssHeader2" />
                                                <CellStyle CssClass="GridCss2">
                                                </CellStyle>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="Status" FieldName="Status" 
                                                ShowInCustomizationForm="True" VisibleIndex="2">
                                                <HeaderStyle CssClass="GridCssHeader1" />
                                                <CellStyle CssClass="GridCss1">
                                                </CellStyle>
                                            </dx:GridViewDataTextColumn>
                                        </Columns>
                                        <SettingsPager AlwaysShowPager="True" SEOFriendly="Enabled">
                                            <PageSizeItemSettings Visible="True">
                                            </PageSizeItemSettings>
                                        </SettingsPager>
                                        <Styles>
                                            <Header CssClass="GridCssHeader">
                                            </Header>
                                            <AlternatingRow CssClass="GridAltRow">
                                            </AlternatingRow>
                                            <Cell CssClass="GridCss">
                                            </Cell>
                                        </Styles>
                                    </dx:ASPxGridView></td></tr>--%>
					<%--  <tr>
            <td>
                <dx:ASPxPopupControl ID="CreateTestAlertPopupControl" runat="server" ClientInstanceName="CreateTestAlertPopupControl"
                    HeaderText="Create Test Alert" PopupHorizontalAlign="WindowCenter" 
                    PopupVerticalAlign="WindowCenter" Theme="MetropolisBlue" Height="200px" 
                    AllowDragging="True" Modal="True">
                    <ClientSideEvents CloseUp="function(s, e) {
     CreateTestAlertPopupControl.Hide();
}" />
                    <ContentCollection>
<dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server" SupportsDisabledAttribute="True">
<asp:UpdatePanel ID="updatepan1" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<table>
        <tr>
            <td>
                <dx:ASPxLabel ID="ASPxLabel2" runat="server" CssClass="lblsmallFont" 
                    Text="Location:">
                </dx:ASPxLabel>
            </td>
            <td>
                <dx:ASPxComboBox ID="LocationComboBox" runat="server" AutoPostBack="True" 
                    ClientInstanceName="LocationComboBox" 
                    OnSelectedIndexChanged="LocationComboBox_SelectedIndexChanged">
                </dx:ASPxComboBox>
            </td>
        </tr>
        <tr>
            <td>
                <dx:ASPxLabel ID="ASPxLabel3" runat="server" CssClass="lblsmallFont" 
                    Text="Device Type:" Wrap="False">
                </dx:ASPxLabel>
            </td>
            <td>
                <dx:ASPxComboBox ID="DeviceTypeComboBox" runat="server" AutoPostBack="True" 
                    OnSelectedIndexChanged="DeviceTypeComboBox_SelectedIndexChanged">
                </dx:ASPxComboBox>
            </td>
        </tr>
        <tr>
            <td>
                <dx:ASPxLabel ID="ASPxLabel4" runat="server" CssClass="lblsmallFont" 
                    Text="Event Type:">
                </dx:ASPxLabel>
            </td>
            <td>
                <dx:ASPxComboBox ID="EventTypeComboBox" runat="server" Enabled="False">
                </dx:ASPxComboBox>
            </td>
        </tr>
        <tr>
            <td>
                <dx:ASPxLabel ID="ASPxLabel5" runat="server" CssClass="lblsmallFont" 
                    Text="Device Name:" Wrap="False">
                </dx:ASPxLabel>
            </td>
            <td>
                <dx:ASPxComboBox ID="ServerNameComboBox" runat="server" Enabled="False">
                </dx:ASPxComboBox>
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
                <dx:ASPxButton ID="TestAlertButton" runat="server"
                    OnClick="TestAlertButton_Click" Text="Create" Theme="Office2010Blue" 
                    ClientInstanceName="goButton">
                    <ClientSideEvents Click="function(s, e) {
	CreateTestAlertPopupControl.Hide();
}" />
                </dx:ASPxButton>
            </td>
            <td>
                <dx:ASPxButton ID="CancelButton" runat="server" CausesValidation="False" 
                    CssFilePath="~/App_Themes/Office2010Blue/{0}/styles.css" 
                    CssPostfix="Office2010Blue" EnableClientSideAPI="true" Font-Bold="False" 
                    SpriteCssFilePath="~/App_Themes/Office2010Blue/{0}/sprite.css" Text="Cancel" 
                    Width="75px" onclick="CancelButton_Click">
                    <ClientSideEvents Click="function(s, e) {
	CreateTestAlertPopupControl.Hide();
}" />
                </dx:ASPxButton>
            </td>
        </tr>
    </table>
</ContentTemplate>
</asp:UpdatePanel>
    
                        </dx:PopupControlContentControl>
</ContentCollection>
                </dx:ASPxPopupControl>
            </td>
        </tr>--%><tr>
												<td>
													<div id="errorDivForImportingWS" runat="server" class="alert alert-danger" style="display: none">
													</div>
												</td>
											</tr>
				</table>
			</td>
		</tr>
	</table>
	<dx:ASPxPopupControl ID="NavigatorPopupControl" runat="server" ClientInstanceName="popup1"
		HeaderText="Information" Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
		Theme="Glass">
		<ContentCollection>
			<dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server" SupportsDisabledAttribute="True">
				<table class="style1">
					<tr>
						<td>
							<dx:ASPxLabel ID="MsgLabel" runat="server" ClientInstanceName="poplbl">
							</dx:ASPxLabel>
						</td>
					</tr>
					<tr>
						<td>
						</td>
					</tr>
					<tr>
						<td>
							<dx:ASPxButton ID="OKButton" runat="server" OnClick="OKButton_Click" Text="OK" Theme="Office2010Blue">
							</dx:ASPxButton>
						</td>
					</tr>
				</table>
			</dx:PopupControlContentControl>
		</ContentCollection>
	</dx:ASPxPopupControl>
</asp:Content>
