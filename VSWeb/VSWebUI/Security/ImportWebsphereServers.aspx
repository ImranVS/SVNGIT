<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true"
	CodeBehind="ImportWebsphereServers.aspx.cs" Inherits="VSWebUI.Security.ImportWebsphereServers" %>

<%@ MasterType VirtualPath="~/Site1.Master" %>
<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.Web" TagPrefix="dx1" %>
<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.Web" TagPrefix="dx2" %>
<%@ Register Assembly="DevExpress.Web.ASPxTreeList.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.Web.ASPxTreeList" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
	<link href='http://fonts.googleapis.com/css?family=Francois One' rel='stylesheet'
		type='text/css' />
	<link rel="stylesheet" type="text/css" href="../css/vswebforms.css" />
	<script src="../js/jquery-1.9.1.js" type="text/javascript"></script>
	<script src="../js/bootstrap.min.js" type="text/javascript"></script>
	<script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.7.2/jquery.min.js">
	<script type="text/javascript">
//		$(document).ready(function () {
//			$('.alert-success').delay(10000).fadeOut("slow", function () {
//			});
//			$('.alert-danger').delay(10000).fadeOut("slow", function () {
//			});
//		});
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

		function CollapseTree() {
			alert('in');
			eventsTree.ExpandAll();
		}
		</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
	<table width="90%">
		<tr>
			<td>
				<div class="header" id="servernamelbldisp" runat="server">
					Cell Configuration</div>
			</td>
		</tr>
		<tr>
			<td>
			</td>
		</tr>
		<tr>
			<td>
				<div id="successDiv" runat="server" class="success" style="display: none">
					server datails were successully saved.
				</div>
			</td>
		</tr>
		<tr>
			<td style="color: Black" id="tdmsg1" runat="server" align="left">
				<dx:ASPxRoundPanel ID="ASPxRoundPanel2" runat="server" Theme="Glass" Width="100%"
					HeaderText="WebSphere Cell">
					<PanelCollection>
						<dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">
							<table>
								<tr>
									<td>
										<dx:ASPxLabel ID="lblcellname" runat="server" Text="Name:" CssClass="lblsmallFont">
										</dx:ASPxLabel>
									</td>
									<td>
									<dx:ASPxCallbackPanel ID="ASPxCallbackPanel1" runat="server" ClientInstanceName="callbackpanel"
											Width="200px">
											<PanelCollection>
												<dx:PanelContent ID="PanelContent2" runat="server" SupportsDisabledAttribute="True">
										<dx:ASPxTextBox ID="CellnameTextBox" runat="server" Width="220px"  AutoPostBack="true" 
														ClientInstanceName="CellnameTextBox" OnValidation="CellnameTextBox_Validation">
										<ClientSideEvents LostFocus="function(s, e) {s.Validate();}" Validation="function(s, e) {callbackpanel.PerformCallback();}" />
											<ValidationSettings ErrorDisplayMode="Text">
												<RequiredField ErrorText="Please enter  Cell Name" IsRequired="True" />
											</ValidationSettings>
										</dx:ASPxTextBox>
										</dx:PanelContent>
											</PanelCollection>
										</dx:ASPxCallbackPanel>
									</td>
								</tr>
								<tr>
									<td>
										<dx:ASPxLabel ID="ASPxLabel3" runat="server" Text="Host Name:" CssClass="lblsmallFont">
										</dx:ASPxLabel>
									</td>
									<td>
										<dx:ASPxCallbackPanel ID="ASPxCallbackPanel2" runat="server" ClientInstanceName="callbackpanel"
											Width="200px">
											<PanelCollection>
												<dx:PanelContent ID="PanelContent4" runat="server" SupportsDisabledAttribute="True">
													<dx:ASPxTextBox ID="HostName" runat="server" Width="220px" 
														 AutoPostBack="true" OnValidation="HostName_Validation">
														<ClientSideEvents LostFocus="function(s, e) {s.Validate();}" Validation="function(s, e) {callbackpanel.PerformCallback();}" />
														<ValidationSettings ErrorDisplayMode="Text"  SetFocusOnError="false">
															<RequiredField ErrorText="Please enter  Host Name" IsRequired="True" />
															<%--<RegularExpression ValidationExpression="^((25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)(\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)){3})$|^((([a-zA-Z0-9]|[a-zA-Z0-9][a-zA-Z0-9\-]*[a-zA-Z0-9])\.)*([A-Za-z0-9]|[A-Za-z0-9][A-Za-z0-9\-]*[A-Za-z0-9]))$" ErrorText="Enter valid IP or Host Nmae" />--%>
														
														<%--<RegularExpression ValidationExpression="([a-zA-Z0-9]|[a-zA-Z0-9][a-zA-Z0-9\-]*[a-zA-Z0-9])\.)*" />--%>
														</ValidationSettings>
													</dx:ASPxTextBox>
												</dx:PanelContent>
											</PanelCollection>
										</dx:ASPxCallbackPanel>
									</td>
								</tr>
								<tr>
									<td>
										<dx:ASPxLabel ID="connlbl" runat="server" Text="Conn.Type:" CssClass="lblsmallFont">
										</dx:ASPxLabel>
									</td>
									<td>
										<dx:ASPxComboBox ID="ConnectionComboBox" runat="server" AutoPostBack="true" Width="220px"
											OnSelectedIndexChanged="ConnectionComboBox_SelectedIndexChanged">
											<Items>
												<dx:ListEditItem Text="SOAP" Value="0" />
												<dx:ListEditItem Text="RMI" Value="1" />
											</Items>
											<ValidationSettings ErrorDisplayMode="Text" SetFocusOnError="True">
												<RequiredField ErrorText="Please select connection type" IsRequired="True" />
											</ValidationSettings>
										</dx:ASPxComboBox>
									</td>
								</tr>
								<tr>
									<td>
										<dx:ASPxLabel ID="ASPxLabel4" runat="server" Text="Port No.:" CssClass="lblsmallFont">
										</dx:ASPxLabel>
									</td>
									<td>
										<dx:ASPxTextBox ID="porttextbox" runat="server" Width="220px">
											<ValidationSettings ErrorDisplayMode="Text" SetFocusOnError="True">
												<RequiredField ErrorText="Please enter port no" IsRequired="True" />
											</ValidationSettings>
										</dx:ASPxTextBox>
									</td>
								</tr>
								<tr>
									<td>
										<dx:ASPxCheckBox ID="chbx" runat="server" Text="Global Security" TextAlign="left"
											AutoPostBack="true" OnCheckedChanged="chbx_CheckedChanged">
										</dx:ASPxCheckBox>
									</td>
								</tr>
								<tr>
									<td>
										<dx:ASPxLabel ID="CredentialsLabel" runat="server" CssClass="lblsmallFont" Text="Credentials:"
											Visible="false">
										</dx:ASPxLabel>
									</td>
									<td>
										<dx:ASPxComboBox ID="CredentialsComboBox" runat="server" AutoPostBack="false" Visible="false">
										<ValidationSettings ErrorDisplayMode="Text" SetFocusOnError="True">
												<RequiredField ErrorText="Please select Credentials" IsRequired="True" />
											</ValidationSettings>
										</dx:ASPxComboBox>
									</td>
									<td>
										<dx:ASPxButton ID="ASPxButton1" runat="server" Text="Add Credentials" CssClass="sysButton"
											OnClick="btn_clickcopyprofile" CausesValidation="false" Visible="false">
										</dx:ASPxButton>
									</td>
								</tr>
								<tr>
									<td>
										<dx:ASPxLabel ID="reallbl" runat="server" Text="Realm:" Visible="false" CssClass="lblsmallFont">
										</dx:ASPxLabel>
									</td>
									<td>
										<dx:ASPxTextBox ID="realmtxtbx" runat="server" Width="220px" Visible="false">
										
										</dx:ASPxTextBox>
									</td>
									<%--	<td>
					<dx:ASPxComboBox ID="ServerTypeComboBox" runat="server" AutoPostBack="false">
						<Items>
							<dx:ListEditItem Text="Active Directory" Value="0" />
							<dx:ListEditItem Text="Database Availability Group" Value="1" />
							<dx:ListEditItem Text="Exchange" Value="2" />
							<dx:ListEditItem Text="Lync" Value="3" />
							<dx:ListEditItem Text="SharePoint" Value="4" />
							<dx:ListEditItem Text="Windows" Value="5" />
						</Items>
					
					</dx:ASPxComboBox>
				</td>--%>
								</tr>
								<%--<tr>
									<td>
										<dx:ASPxLabel ID="certifaicatelbl" runat="server" Text="Certificate:" Visible="false"
											CssClass="lblsmallFont">
										</dx:ASPxLabel>
									</td>
									<td id="tdload" runat="server">
										<dx:ASPxUploadControl ID="fileupld" runat="server" Visible="false" ClientInstanceName="uploader"
											ShowProgressPanel="false" NullText="Click here to browse  files." Size="30" OnFileUploadComplete="fileupld_FileUploadComplete"
											CancelButtonHorizontalPosition="Right" ShowUploadButton="false">
											<ClientSideEvents FileUploadComplete="function(s, e) { Uploader_OnFileUploadComplete(e); }"
												FilesUploadComplete="function(s, e) { Uploader_OnFilesUploadComplete(e); }" FileUploadStart="function(s, e) { Uploader_OnUploadStart(); }"
												TextChanged="function(s, e) { UpdateUploadButton(); }"></ClientSideEvents>
											<ValidationSettings MaxFileSize="4194304">
											</ValidationSettings>
											<CancelButton ImagePosition="Right">
											</CancelButton>
										</dx:ASPxUploadControl>
											
									</td>
									<td colspan="2">
								<a id="anchorFilename" runat="server" target="_blank">
                                                        <asp:Label ID="lblfilename" runat="server"  Width="200px"  CssClass="lblsmallFont"></asp:Label></a>
                                                    <asp:Label ID="lblFullFileName" runat="server"  CssClass="lblsmallFont" ></asp:Label>
													 <div>
                                                        <asp:LinkButton ID="lnkchange" runat="server" CausesValidation="false" Text="Change" Visible="false" 
                                                            OnClick="lnkChange_Click"  CssClass="lblsmallFont" Visible="false"></asp:LinkButton>
                                                    </div>
								</td>
									
								</tr>--%>
							</table>
							
						</dx:PanelContent>
					</PanelCollection>
				</dx:ASPxRoundPanel>
			</td>
		</tr>
		<tr>
			<td>
				<div id="errorDiv" runat="server" class="alert alert-danger" style="display: none">
					Error:
				</div>
				<div id="errorinfoDiv" runat="server" class="info" style="display: none">
				</div>
				<div id="infoDiv" runat="server" class="info" style="display: none">
					Any server not on the list has already been imported.
				</div>
			</td>
		</tr>
		<tr>
			<td>
				<table>
					<tr>
						<td colspan="2">
							<div id="errormsg" runat="server" class="alert alert-danger" style="display: none">
								Error attempting to update the status table.
							</div>
						</td>
					</tr>
					<tr>
						<td>
							<dx:ASPxButton ID="savebtn" runat="server" Text="Save" CssClass="sysButton" OnClick="savebtn_Click">
							</dx:ASPxButton>
						</td>
						<td>
							<dx:ASPxButton ID="cancelbtn" runat="server" Text="Cancel" CssClass="sysButton" OnClick="cancelbtn_Click"
								CausesValidation="false">
							</dx:ASPxButton>
						</td>
					</tr>
					
				</table>
			</td>
		</tr>
		<tr>
			<td style="color: Black" id="tdmsg" runat="server" align="left">
				<dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" GroupBoxCaptionOffsetY="-24px"
					HeaderText="Servers" Width="100%" Theme="Glass" Visible="false">
					<ContentPaddings PaddingBottom="10px" PaddingLeft="4px" PaddingTop="10px" />
					<HeaderStyle Height="23px">
						<Paddings PaddingBottom="0px" PaddingLeft="2px" PaddingTop="0px" />
					</HeaderStyle>
					<PanelCollection>
						<dx:PanelContent ID="PanelContent3" runat="server" SupportsDisabledAttribute="True">
							<table class="style1">
								<tr>
									<td>
										<asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
											<ContentTemplate>
												<table>
													<tr>
														<td>
															<dx:ASPxButton ID="CollapseAllButton" ClientInstanceName="collapseAll" runat="server"
																Text="Collapse All" Theme="Office2010Blue" Wrap="False" EnableClientSideAPI="False"
																OnClick="CollapseAllButton_Click">
																<Image Url="~/images/icons/forbidden.png">
																</Image>
															</dx:ASPxButton>
														</td>
													</tr>
													<tr>
														<td valign="top">
															<dx:ASPxTreeList ID="NodesTreeList" ClientInstanceName="eventsTree" runat="server"
																AutoGenerateColumns="False" CssClass="lblsmallFont" KeyFieldName="Id" ParentFieldName="SrvId"
																Theme="Office2003Blue" Width="100%">
																<Columns>
																	<dx:TreeListTextColumn Caption="NodeName  " FieldName="Name" Name="NodeName" VisibleIndex="0"
																		ReadOnly="True">
																		<EditFormSettings Visible="False" />
																		<HeaderStyle CssClass="lblsmallFont" />
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
							</table>
						</dx:PanelContent>
					</PanelCollection>
				</dx:ASPxRoundPanel>
			</td>
		</tr>
		<tr>
			<td>
				<asp:UpdatePanel ID="UpdatePanel45" runat="server" UpdateMode="Conditional">
					<ContentTemplate>
						<div id="Div1" class="alert alert-success" runat="server" style="display: none">
							Success.
						</div>
						<div id="Div2" class="alert alert-danger" runat="server" style="display: none">
							Error.
						</div>
						<table>
							<tr>
								<td>
									<%--<dx:ASPxButton ID="OKButton" runat="server" Text="OK" Width="75px" 
										Theme="Office2010Blue" onclick="OKButton_Click">
									</dx:ASPxButton>--%>
								</td>
								<td>
									<%--<dx:ASPxButton ID="CancelButton" runat="server" CausesValidation="False" Text="Cancel"
										Width="75px" Theme="Office2010Blue" onclick="CancelButton_Click">
									</dx:ASPxButton>--%>
								</td>
							</tr>
						</table>
					</ContentTemplate>
					<%--<Triggers>
						<asp:AsyncPostBackTrigger ControlID="OKButton" />
					</Triggers>--%>
				</asp:UpdatePanel>
			</td>
		</tr>
		<tr>
			<td>
				<dx:ASPxPopupControl ID="CopyProfilePopupControl" runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
					CssPostfix="Glass" HeaderText="Please Enter Credentials" SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css"
					Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
					Theme="MetropolisBlue" EnableHierarchyRecreation="False" Width="320px">
					<LoadingPanelImage Url="~/App_Themes/Glass/Web/Loading.gif">
					</LoadingPanelImage>
					<HeaderStyle>
						<Paddings PaddingLeft="10px" PaddingRight="6px" PaddingTop="1px" />
					</HeaderStyle>
					<ContentCollection>
						<dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server" SupportsDisabledAttribute="True">
							<table class="style1">
								<tr>
									<td>
									</td>
									<td>
									</td>
								</tr>
								<tr>
									<td>
										<dx2:ASPxLabel ID="AliasNameLabel" runat="server" CssClass="lblsmallFont" Text="Alias Name:">
										</dx2:ASPxLabel>
									</td>
									<td>
										<dx2:ASPxTextBox ID="AliasName" runat="server" ClientInstanceName="AliasName" Width="170px">
										</dx2:ASPxTextBox>
									</td>
								</tr>
								<tr>
									<td>
										<dx:ASPxLabel ID="UserIDLabel" runat="server" Text="User ID:" CssClass="lblsmallFont">
										</dx:ASPxLabel>
									</td>
									<td>
										<dx:ASPxTextBox ID="UserID" runat="server" Width="170px" ClientInstanceName="UserID">
											<%--<ClientSideEvents KeyDown="function(s, e) {OnKeyDown(s, e);}" />--%>
										</dx:ASPxTextBox>
									</td>
								</tr>
								<tr>
									<td>
										<dx:ASPxLabel ID="PasswordLabel" runat="server" Text="Password:" CssClass="lblsmallFont">
										</dx:ASPxLabel>
									</td>
									<td>
										<dx:ASPxTextBox ID="Password" runat="server" Width="170px" ClientInstanceName="password"
											Password="True">
											<ClientSideEvents KeyDown="function(s, e) {OnKeyDown(s, e);}" />
										</dx:ASPxTextBox>
									</td>
								</tr>
								<tr>
									<td colspan="2">
										<div id="Div3" runat="server" class="alert alert-danger" style="display: none">
											Error:
										</div>
									</td>
								</tr>
							</table>
							<table>
								<tr>
									<td>
										<dx:ASPxButton ID="OKCopy" runat="server" CssClass="sysButton" Text="OK" OnClick="OKCopy_Click"
											ClientInstanceName="goButton" CausesValidation="false">
										</dx:ASPxButton>
									</td>
									<td>
										<dx:ASPxButton ID="Cancel" runat="server" CssClass="sysButton" Text="Cancel" OnClick="Cancel_Click"
											ClientInstanceName="goButton" CausesValidation="false">
										</dx:ASPxButton>
									</td>
								</tr>
							</table>
						</dx:PopupControlContentControl>
					</ContentCollection>
				</dx:ASPxPopupControl>
			</td>
		</tr>
	</table>
</asp:Content>
