<%@ Page Title="Vital Signs - High Availability" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="HighAvailability.aspx.cs" Inherits="VSWebUI.Security.HighAvailability" %>
<%@ MasterType virtualpath="~/Site1.Master" %>
<%@ Register assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>







<%@ Register assembly="DevExpress.Web.ASPxTreeList.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxTreeList" tagprefix="dx" %>












<%@ Register assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script src="../js/jquery-1.9.1.js" type="text/javascript"></script>
    <script src="../js/bootstrap.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('.alert-success').delay(10000).fadeOut("slow", function () {
            });
            $('.alert-danger').delay(10000).fadeOut("slow", function () {
            });
        });
        </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table>
    <tr>
            <td>
                <div class="info">VitalSigns monitoring stations are called 'nodes'. A monitoring node can run in either StandAlone, FailOver or LoadBalancing mode. </div>
                
            </td>
        </tr>
     <tr>
	<td colspan="2">
		<dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" 
			CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass" 
			GroupBoxCaptionOffsetY="-24px" HeaderText="High Availability Options" 
			SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" Width="600px">
			<ContentPaddings Padding="2px" PaddingBottom="10px" PaddingLeft="4px" PaddingTop="10px" />		
			<ContentPaddings Padding="2px" PaddingLeft="4px" PaddingTop="10px" PaddingBottom="10px"></ContentPaddings>

			<HeaderStyle Height="23px">
				<Paddings PaddingBottom="0px" PaddingLeft="2px" PaddingTop="0px" />
				<Paddings PaddingLeft="2px" PaddingTop="0px" PaddingBottom="0px"></Paddings>
			</HeaderStyle>
			<PanelCollection>
				<dx:PanelContent ID="PrimaryNodePanel" runat="server" SupportsDisabledAttribute="True">
					<table cellspacing="3px">
						<tr>
							<td colspan="2">      
								<dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="Run Mode" CssClass="lblsmallFont"></dx:ASPxLabel>
							</td>
						</tr>
						<tr>
							<td colspan="2">
								<dx:ASPxRadioButtonList ID="SelCriteriaRadioButtonList" runat="server" 
									SelectedIndex="2" OnSelectedIndexChanged="SelCriteriaRadioButtonList_SelectedIndexChanged" 
									AutoPostBack="True">
									<Items>
										<dx:ListEditItem Text="Stand Alone" Value="0" />
										<dx:ListEditItem Text="FailOver" Value="1" />
										<dx:ListEditItem Selected="True" Text="Load Balancing" Value="2" />
									</Items>
								</dx:ASPxRadioButtonList>             
							</td>
							<td></td>
							<td>
							<dx:ASPxPanel ID="ServerNode1" runat="server" Width="100%">
								<PanelCollection>
									<dx:PanelContent ID="ServerNodes1" runat="server" SupportsDisabledAttribute="True">
										<table id="PrimaryTable" runat="server" width="100%" border="0">
											<tr>
												<td>
													<dx:ASPxLabel ID="PrimaryNode" runat="server" 
														CssClass="lblsmallFont"  
														Text="Primary Node:" Width="118px">
													</dx:ASPxLabel>
												</td>
											</tr>
											<tr>
												<td>
													<dx:ASPxLabel ID="Label1" runat="server" 
														CssClass="lblsmallFont"  
														Text="Host Name:" Width="118px">
													</dx:ASPxLabel>
                                                </td>
                                                <td>
													<dx:ASPxTextBox ID="PrimaryNodeHostName" runat="server" 
														Enabled="True" Width="170px">
													</dx:ASPxTextBox>
												</td>                                                        
                                            </tr> 
											<tr>
												<td>
													<dx:ASPxLabel ID="ASPxLabel2" runat="server" 
														CssClass="lblsmallFont"  
														Text="Ip Address:" Width="118px">
													</dx:ASPxLabel>
												</td>
												<td>
													<dx:ASPxTextBox ID="PrimaryNodeIPAddress" runat="server" 
														Enabled="True" Width="170px">
													</dx:ASPxTextBox>
												</td>                                                        
											</tr> 
											<tr>
												<td>
													<dx:ASPxLabel ID="ASPxLabel3" runat="server" 
														CssClass="lblsmallFont"  
														Text="Description:" Width="118px">
													</dx:ASPxLabel>
												</td>
												<td>
													<dx:ASPxTextBox ID="PrimaryNodeDescription" runat="server" 
														Enabled="True" Width="170px">
													</dx:ASPxTextBox>
												</td>                                                        
											</tr>                                                         
                                        </table>
                                                
                                    </dx:PanelContent>
                                            
                                </PanelCollection>
                            </dx:ASPxPanel>
							</td>
						</tr>
						<tr>
						<td colspan="2">
						</td>
						<td></td>
						<td>
						<dx:ASPxPanel ID="SecondaryNodePanel" runat="server" Width="100%" Visible=true>
						<PanelCollection>
							<dx:PanelContent ID="ServerPanelContent2" runat="server" SupportsDisabledAttribute="True">
								<table id="SecondaryTable" runat="server" width="100%" border="0" >
									<tr>
										<td>
											<dx:ASPxLabel ID="ASPxLabel4" runat="server" 
												CssClass="lblsmallFont"  
												Text="Secondary Node:" Width="118px">
											</dx:ASPxLabel>
										</td>
									</tr>
									<tr>
										<td>
											<dx:ASPxLabel ID="ASPxLabel5" runat="server" 
												CssClass="lblsmallFont"  
												Text="Host Name:" Width="118px">
											</dx:ASPxLabel>
										</td>
										<td>
											<dx:ASPxTextBox ID="SecondaryNodeHostName" runat="server" 
												Enabled="True" Width="170px">
											</dx:ASPxTextBox>
										</td>                                                        
									</tr> 
									<tr>
										<td>
											<dx:ASPxLabel ID="ASPxLabel6" runat="server" 
												CssClass="lblsmallFont"  
												Text="Ip Address:" Width="118px">
											</dx:ASPxLabel>
										</td>
										<td>
											<dx:ASPxTextBox ID="SecondaryNodeIPAddress" runat="server" 
												Enabled="True" Width="170px">
											</dx:ASPxTextBox>
										</td>                                                        
									</tr> 
									<tr>
										<td>
											<dx:ASPxLabel ID="ASPxLabel7" runat="server" 
												CssClass="lblsmallFont"  
												Text="Description:" Width="118px">
											</dx:ASPxLabel>
										</td>
										<td>
											<dx:ASPxTextBox ID="SecondaryNodeDescription" runat="server" 
												Enabled="True" Width="170px">
											</dx:ASPxTextBox>
										</td>                                                        
									</tr> 
								</table>
                                                
                                </dx:PanelContent>
                                            
							</PanelCollection>
							</dx:ASPxPanel>
							</td>
						</tr>
						<tr>
						   <td colspan="2">
								<div id="errorDiv" runat="server" class="alert alert-danger" style="display: none">Settings for selected servers were NOT updated.
                                <button type="button" class="close" data-dismiss="alert"><span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
								</div>
						   </td>
						</tr>
					    <tr>
						   <td colspan="2">
								<div id="successDiv" runat="server" class="alert alert-success" style="display: none">Settings for selected servers were successully updated.
                                <button type="button" class="close" data-dismiss="alert"><span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
								</div>
						   </td>
						   
					    </tr>
                        <tr>
							 <td>
								<dx:ASPxButton runat="server" Text="Save Settings" Theme="Office2010Blue"
									 Width="120px" ID="ASPxButton1" OnClick="SaveSettings_Click">
								</dx:ASPxButton>
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
        <dx:ASPxRoundPanel ID="ServerRoundPanel" runat="server" 
				CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass" 
				GroupBoxCaptionOffsetY="-24px" HeaderText="Server" Height="50px" 
				SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" Width="600px">
				<ContentPaddings Padding="2px" PaddingBottom="10px" PaddingLeft="4px" 
					PaddingTop="10px" />
				<ContentPaddings Padding="2px" PaddingLeft="4px" PaddingTop="10px" PaddingBottom="10px"></ContentPaddings>

				<HeaderStyle Height="23px">
					<Paddings PaddingBottom="0px" PaddingLeft="2px" PaddingTop="0px" />
					<Paddings PaddingLeft="2px" PaddingTop="0px" PaddingBottom="0px"></Paddings>
				</HeaderStyle>
				<PanelCollection>
					<dx:PanelContent ID="PanelContent2" runat="server" SupportsDisabledAttribute="True">    
						<table>
							<tr>
								<td>
									<dx:ASPxLabel ID="Label2" runat="server" Text="Server Node:" CssClass="lblsmallFont"></dx:ASPxLabel>
								</td>
								<td>
									<dx:ASPxComboBox ID="ServerNodeComboBox" runat="server" AutoPostBack="true"
														ValueType="System.String">
									 </dx:ASPxComboBox>
								</td>
							</tr>
							<tr>
							   <td colspan=2>
								  <ContentCollection>
									   <dx:ContentControl runat="server" SupportsDisabledAttribute="True">
											<asp:UpdatePanel ID="ServersList" runat="server" UpdateMode="Conditional">
												<ContentTemplate>
												   <table id="tblServer" runat="server">
														<tr>
															<td></td>
														</tr>
														<tr>
														   <td id="Td2"   runat="server" align="left">
															  <dx:ASPxButton ID="CollapseAllButton" runat="server" ClientInstanceName="collapseAll" EnableClientSideAPI="False" 
																 onclick="CollapseAllButton_Click" Text="Collapse All" Theme="Office2010Blue" Wrap="False">
																<Image Url="~/images/icons/forbidden.png">
																</Image>
															  </dx:ASPxButton>
														   </td>
														</tr>
														<tr>
														   <td>
															  <dx:ASPxTreeList runat="server" KeyFieldName="Id" ParentFieldName="LocId" AutoGenerateColumns="False"
																Theme="Office2003Blue" Width="100%" ID="ServersTreeList" CssClass="lblsmallFont" style="margin-top: 0px">
																<Columns>
																	<dx:TreeListTextColumn FieldName="Name" Name="Servers"
																		Caption="Servers  " VisibleIndex="0">
																		<HeaderStyle CssClass="GridCssHeader"></HeaderStyle>
																		<CellStyle CssClass="GridCss">
																		</CellStyle>
																	</dx:TreeListTextColumn>
																	<dx:TreeListTextColumn FieldName="actid" Name="actid"
																		Visible="False" VisibleIndex="1">
																	</dx:TreeListTextColumn>
																	<dx:TreeListTextColumn FieldName="tbl" Name="tbl"
																		Visible="False" VisibleIndex="2">
																	</dx:TreeListTextColumn>
																	<dx:TreeListTextColumn FieldName="LocId" Name="LocId"
																		Visible="False" VisibleIndex="3">
																	</dx:TreeListTextColumn>
																	<dx:TreeListTextColumn FieldName="ServerType" VisibleIndex="4">
																		<HeaderStyle CssClass="GridCssHeader"></HeaderStyle>
																		<CellStyle CssClass="GridCss">
																		</CellStyle>
																	</dx:TreeListTextColumn>
																	<dx:TreeListTextColumn FieldName="Description" VisibleIndex="5">
																		<HeaderStyle CssClass="GridCssHeader"></HeaderStyle>
																		<CellStyle CssClass="GridCss">
																		</CellStyle>
																	</dx:TreeListTextColumn>
                                                                    <dx:TreeListTextColumn FieldName="MonitoredBy" VisibleIndex="6">
																		<HeaderStyle CssClass="GridCssHeader"></HeaderStyle>
																		<CellStyle CssClass="GridCss">
																		</CellStyle>
																	</dx:TreeListTextColumn>
																</Columns>
																<Settings GridLines="Both"></Settings>
																<SettingsBehavior AutoExpandAllNodes="True" AllowDragDrop="False"></SettingsBehavior>
																<SettingsPager Mode="ShowPager" AlwaysShowPager="True" PageSize="20">
																	<PageSizeItemSettings Visible="True">
																	</PageSizeItemSettings>
																</SettingsPager>
																<SettingsSelection Enabled="True" AllowSelectAll="True" Recursive="True"></SettingsSelection>
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
													<asp:AsyncPostBackTrigger ControlID="ApplyServersButton" />
												</Triggers>
											</asp:UpdatePanel>
									    </dx:ContentControl>
									</ContentCollection>
								</td>
							</tr>
                            <tr>
						   <td colspan="2">
								<div id="ErrorUpdateServers" runat="server" class="alert alert-danger" style="display: none">Settings for selected servers were NOT updated.
                                <button type="button" class="close" data-dismiss="alert"><span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
								</div>
						   </td>
						</tr>
					    <tr>
						   <td colspan="2">
								<div id="SuccessUpdateServers" runat="server" class="alert alert-success" style="display: none">Settings for selected servers were successully updated.
                                <button type="button" class="close" data-dismiss="alert"><span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
								</div>
						   </td>
						   
					    </tr>
							<tr>
								 <td>
									<dx:ASPxButton runat="server" Text="Apply" Theme="Office2010Blue"
										 Width="50px" ID="ApplyServersButton" OnClick="ApplyToServers_Click">
									</dx:ASPxButton>
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
