<%@ Page Title="VitalSigns Plus - Manage Server Settings" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="AssignServerToNode.aspx.cs" Inherits="VSWebUI.Security.AssignServerToNode" %>

<%@ MasterType virtualpath="~/Site1.Master" %>
<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>




        



<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.5/jquery.min.js"></script>
<link href='http://fonts.googleapis.com/css?family=Francois One' rel='stylesheet' type='text/css' />
<link rel="stylesheet" type="text/css" href="../css/vswebforms.css" />

    <script type="text/javascript">
    	$(document).ready(function () {
    		$('.alert-success').delay(10000).fadeOut("slow", function () {
    		});
    	});
    	function ProcessSelection(grid, visibleIndex, checkbox, keyValue, hiddenField) {

    		if (checkbox.GetChecked()) {
    			//grid.SelectRowOnPage(visibleIndex);
    			if (!hiddenField.Contains("key" + keyValue.toString())) {
    				hiddenField.Add("key" + keyValue.toString(), true);
    			}
    		}
    		else {
    			//grid.UnselectRowOnPage(visibleIndex);
    			if (hiddenField.Contains("key" + keyValue)) {
    				hiddenField.Remove("key" + keyValue.toString());
    			}
    		}
    	}
    </script>
<script type="text/javascript" language="javascript">
	var visibleIndex;
	function OnCustomButtonClick(s, e) {
		visibleIndex = e.visibleIndex;

		if (e.buttonID == "deleteButton")
			NodesGrid.GetRowValues(e.visibleIndex, 'Name', OnGetRowValues);

		function OnGetRowValues(values) {
			var id = values[0];
			var name = values[1];
			var OK = (confirm('Are you sure you want to delete the node - ' + values + '?'))
			if (OK == true) {
				NodesGrid.DeleteRow(visibleIndex);

				//alert('The Server ' + values + ' was Successfully Deleted');
				//ScriptManager.RegisterClientScriptBlock(base.Page, this.GetType(), "FooterRequired", "alert('Notification : Record deleted successfully');", true);
			}

			else {
				detailsGrid.PerformCallback(e.visibleIndex);
			}

		}
	}
</script>
	<style type="text/css">
		.style1
		{
			width: 391px;
		}
	</style>
	<script type="text/javascript">

		function OnRowClick(s, e) {
			NodesGrid.SetFocusedRowIndex(e.visibleIndex);
			detailsGrid.PerformCallback(e.visibleIndex); //NodesGrid.GetFocusedRowIndex());
		}
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
	<table>
		<tr>
			<td>
				
				<table>
					<tr>
						<td>
							<div class="header" id="Div1" runat="server">Node Health</div>
						</td>
					</tr>
																																																																																																																																																									<tr>
						<td>
							<dx:ASPxGridView ID="NodesGrid"  runat="server" KeyFieldName="ID" 
                                ClientInstanceName="NodesGrid"  OnPageSizeChanged="NodesGrid_PageSizeChanged"
								Width="100%" OnHtmlDataCellPrepared="NodesGrid_HtmlDataCellPrepared" OnHtmlRowCreated="NodesGridView_HtmlRowCreated"
								Theme="Office2003Blue" OnRowDeleting="NodesGridView_RowDeleting" AutoGenerateColumns="False" >
                                <ClientSideEvents CustomButtonClick="OnCustomButtonClick" 
									RowClick="OnRowClick" />
								<Columns>
									<dx:GridViewCommandColumn ButtonType="Image" Caption="Actions"
										VisibleIndex="0" Width="50px">
										<EditButton Visible="True">
											<Image Url="../images/edit.png">
											</Image>
										</EditButton>
										<CellStyle CssClass="GridCss1">
											<Paddings Padding="3px" />
											<Paddings Padding="3px"></Paddings>
										</CellStyle>
										<ClearFilterButton Visible="True">
											<Image Url="~/images/clear.png">
											</Image>
										</ClearFilterButton>
										<HeaderStyle CssClass="GridCssHeader1" />
									</dx:GridViewCommandColumn>

									<dx:GridViewCommandColumn Caption="Delete" ButtonType="Image" VisibleIndex="1" 
										Width="50px">
										<CustomButtons>
											<dx:GridViewCommandColumnCustomButton ID="deleteButton" 
												Image-Url="../images/delete.png" Text="Delete" >
												<Image Url="../images/delete.png"></Image>
											</dx:GridViewCommandColumnCustomButton>
										</CustomButtons>
										<HeaderStyle CssClass="GridCssHeader1" />
									</dx:GridViewCommandColumn>

									<dx:GridViewDataTextColumn ShowInCustomizationForm="True" VisibleIndex="2" 
                                        Caption="Primary Node">
										<PropertiesTextEdit>
											<FocusedStyle HorizontalAlign="Center">
											</FocusedStyle>
										</PropertiesTextEdit>
										<DataItemTemplate>
											<asp:Label ID="lblIcon" runat="server" Text='<%# Eval("IsPrimaryNode")%>' 
												Visible="false"></asp:Label>
											<asp:Image ID="IconImage" runat="server" ImageAlign=Middle />
											<dx:ASPxHyperLink ID="ASPxHyperLink2" runat="server" 
												NavigateUrl="<%# SetIcon(Container) %>" Visible="false">
											</dx:ASPxHyperLink>
										</DataItemTemplate>
										<HeaderStyle CssClass="GridCssHeader" Wrap="True" />
										<CellStyle HorizontalAlign=Center />
									</dx:GridViewDataTextColumn>
				
									<dx:GridViewDataTextColumn Caption="Name" FieldName="Name" 
										ShowInCustomizationForm="True" VisibleIndex="3">
										<PropertiesTextEdit><FocusedStyle HorizontalAlign="Center"></FocusedStyle></PropertiesTextEdit>
										<Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
										<HeaderStyle CssClass="GridCssHeader" />
										<CellStyle CssClass="GridCss" HorizontalAlign="Center">
										</CellStyle>
									</dx:GridViewDataTextColumn>
					
									<dx:GridViewDataTextColumn Caption="Host Name" FieldName="HostName" 
										ShowInCustomizationForm="True" VisibleIndex="4">
										<PropertiesTextEdit><FocusedStyle HorizontalAlign="Center"></FocusedStyle></PropertiesTextEdit>
										<Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
										<HeaderStyle CssClass="GridCssHeader" />
										<CellStyle CssClass="GridCss" HorizontalAlign="Center">
										</CellStyle>
									</dx:GridViewDataTextColumn>

									




									<dx:GridViewDataTextColumn Caption="Version" FieldName="Version"
											VisibleIndex="6">
											<Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
										<EditCellStyle CssClass="GridCss">
										</EditCellStyle>
										<EditFormCaptionStyle CssClass="GridCss">
										</EditFormCaptionStyle>
										<HeaderStyle CssClass="GridCssHeader" />
										<CellStyle CssClass="GridCss">
										</CellStyle>
									</dx:GridViewDataTextColumn>

									<dx:GridViewDataTextColumn Caption="Node Type" FieldName="NodeType"
											VisibleIndex="7">
											<Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
										<EditCellStyle CssClass="GridCss">
										</EditCellStyle>
										<EditFormCaptionStyle CssClass="GridCss">
										</EditFormCaptionStyle>
										<HeaderStyle CssClass="GridCssHeader" />
										<CellStyle CssClass="GridCss">
										</CellStyle>
									</dx:GridViewDataTextColumn>
									<dx:GridViewDataTextColumn Caption="Load Factor" FieldName="LoadFactor"
											VisibleIndex="8">
											<Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
										<EditCellStyle CssClass="GridCss">
										</EditCellStyle>
										<EditFormCaptionStyle CssClass="GridCss">
										</EditFormCaptionStyle>
										<HeaderStyle CssClass="GridCssHeader" />
										<CellStyle CssClass="GridCss">
										</CellStyle>
									</dx:GridViewDataTextColumn>
									<dx:GridViewDataTextColumn Caption="Configured Primary Node" FieldName="IsConfiguredPrimaryNode"
											VisibleIndex="10">
											<Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
										<EditCellStyle CssClass="GridCss">
										</EditCellStyle>
										<EditFormCaptionStyle CssClass="GridCss">
										</EditFormCaptionStyle>
										<HeaderStyle CssClass="GridCssHeader" Wrap="True" />
										<CellStyle CssClass="GridCss">
										</CellStyle>
									</dx:GridViewDataTextColumn>
									<dx:GridViewDataTextColumn Caption="Location" FieldName="Location"
											VisibleIndex="11">
											<Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
										<EditCellStyle CssClass="GridCss">
										</EditCellStyle>
										<EditFormCaptionStyle CssClass="GridCss">
										</EditFormCaptionStyle>
										<HeaderStyle CssClass="GridCssHeader" />
										<CellStyle CssClass="GridCss">
										</CellStyle>
									</dx:GridViewDataTextColumn>


									<dx:GridViewDataTextColumn Caption="Alive" FieldName="Alive" 
										ShowInCustomizationForm="True" VisibleIndex="19" Width=50>
										<PropertiesTextEdit><FocusedStyle HorizontalAlign="Center"></FocusedStyle></PropertiesTextEdit>
								<Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
										<HeaderStyle CssClass="GridCssHeader" />
										<CellStyle CssClass="GridCss" HorizontalAlign="Center">
										</CellStyle>
									</dx:GridViewDataTextColumn>

									<dx:GridViewDataTextColumn Caption="Disabled" FieldName="isDisabled" 
										ShowInCustomizationForm="True" VisibleIndex="20" Width=50 Visible="false">
										<PropertiesTextEdit><FocusedStyle HorizontalAlign="Center"></FocusedStyle></PropertiesTextEdit>
										<Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
										<HeaderStyle CssClass="GridCssHeader" />
										<CellStyle CssClass="GridCss" HorizontalAlign="Center">
										</CellStyle>
									</dx:GridViewDataTextColumn>


									<dx:GridViewDataTextColumn Caption="Last Update" FieldName="Pulse" 
										ShowInCustomizationForm="True" VisibleIndex="21" Width=160px>
										<PropertiesTextEdit><FocusedStyle HorizontalAlign="Center"></FocusedStyle></PropertiesTextEdit>
										<Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
										<HeaderStyle CssClass="GridCssHeader" />
										<CellStyle CssClass="GridCss" HorizontalAlign="Center">
										</CellStyle>
									</dx:GridViewDataTextColumn>
									<dx:GridViewDataTextColumn FieldName="ID" Visible="false" />

								</Columns>
                                
								<SettingsBehavior ConfirmDelete="True"></SettingsBehavior>
								<Settings ShowFilterRow="True"  ShowGroupPanel="false"/>
								<SettingsText ConfirmDelete="Are you sure you want to delete this server?"></SettingsText>
								
								<SettingsBehavior AllowDragDrop="true" AllowSelectByRowClick="True" AllowFocusedRow="true" ProcessSelectionChangedOnServer="true" ConfirmDelete="True"  />
								
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
							<div class="header" id="Div2" runat="server">Services Health</div>
						</td>
					</tr>
													<tr>
						<td>
							<div id="infoDiv" runat="server" class="info" style="display: block">
								Only the "Primary Node", the node marked with the star, will have the "Alerting Service" and the "DB Health" running on it.  Also, any service not colored is due to it
								running only when needed, so it is expected to be in the "Stopped" state.
							</div>
						</td>
					</tr>
																																																																																																											<tr>
						<td>
							<dx:ASPxGridView ID="servicesGrid"  runat="server" ClientInstanceName="detailsGrid" OnPageSizeChanged="servicesGrid_PageSizeChanged"
								Width="100%" OnDataBound="NodesGrid_OnDataBound" OnHtmlDataCellPrepared="servicesGrid_HtmlDataCellPrepared"
								Theme="Office2003Blue" EnableCallBacks="false" OnCustomCallback="DetailsGrid_CustomCallback">

								<Columns>
			
									<dx:GridViewDataTextColumn Caption="Alerting" FieldName="Alerting"  
										ShowInCustomizationForm="True" VisibleIndex="4">
										<PropertiesTextEdit><FocusedStyle HorizontalAlign="Center"></FocusedStyle></PropertiesTextEdit>
										<Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
										<HeaderStyle CssClass="GridCssHeader" />
										<CellStyle CssClass="GridCss" HorizontalAlign="Center">
										</CellStyle>
									</dx:GridViewDataTextColumn>
									<%--1/28/2016 Durga Modified for VSPLUS-2544--%>
									<dx:GridViewDataTextColumn Caption="Cluster Health" FieldName="Cluster Health"  Visible="false"
										ShowInCustomizationForm="True" VisibleIndex="5" >
										<PropertiesTextEdit><FocusedStyle HorizontalAlign="Center"></FocusedStyle></PropertiesTextEdit>
										<Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
										<HeaderStyle CssClass="GridCssHeader" />
										<CellStyle CssClass="GridCss" HorizontalAlign="Center">
										</CellStyle>
									</dx:GridViewDataTextColumn>

									<dx:GridViewDataTextColumn Caption="Console Commands" FieldName="Console Commands" 
										ShowInCustomizationForm="True" VisibleIndex="6">
										<PropertiesTextEdit><FocusedStyle HorizontalAlign="Center"></FocusedStyle></PropertiesTextEdit>
										<Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
										<HeaderStyle CssClass="GridCssHeader" />
										<CellStyle CssClass="GridCss" HorizontalAlign="Center">
										</CellStyle>
									</dx:GridViewDataTextColumn>

									<dx:GridViewDataTextColumn Caption="Core" FieldName="Core" 
										ShowInCustomizationForm="True" VisibleIndex="7">
										<PropertiesTextEdit><FocusedStyle HorizontalAlign="Center"></FocusedStyle></PropertiesTextEdit>
										<Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
										<HeaderStyle CssClass="GridCssHeader" />
										<CellStyle CssClass="GridCss" HorizontalAlign="Center">
										</CellStyle>
									</dx:GridViewDataTextColumn>

									<dx:GridViewDataTextColumn Caption="Core 64-bit" FieldName="Core 64-bit" 
										ShowInCustomizationForm="True" VisibleIndex="8">
										<PropertiesTextEdit><FocusedStyle HorizontalAlign="Center"></FocusedStyle></PropertiesTextEdit>
										<Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
										<HeaderStyle CssClass="GridCssHeader" />
										<CellStyle CssClass="GridCss" HorizontalAlign="Center">
										</CellStyle>
									</dx:GridViewDataTextColumn>

									<dx:GridViewDataTextColumn Caption="DB Health" FieldName="DB Health" 
										ShowInCustomizationForm="True" VisibleIndex="9">
										<PropertiesTextEdit><FocusedStyle HorizontalAlign="Center"></FocusedStyle></PropertiesTextEdit>
										<Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
										<HeaderStyle CssClass="GridCssHeader" />
										<CellStyle CssClass="GridCss" HorizontalAlign="Center">
										</CellStyle>
									</dx:GridViewDataTextColumn>

									<dx:GridViewDataTextColumn Caption="Domino" FieldName="Domino" 
										ShowInCustomizationForm="True" VisibleIndex="10">
										<PropertiesTextEdit><FocusedStyle HorizontalAlign="Center"></FocusedStyle></PropertiesTextEdit>
										<Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
										<HeaderStyle CssClass="GridCssHeader" />
										<CellStyle CssClass="GridCss" HorizontalAlign="Center">
										</CellStyle>
									</dx:GridViewDataTextColumn>

									<dx:GridViewDataTextColumn Caption="EX Journal" FieldName="EX Journal" 
										ShowInCustomizationForm="True" VisibleIndex="11">
										<PropertiesTextEdit><FocusedStyle HorizontalAlign="Center"></FocusedStyle></PropertiesTextEdit>
										<Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
										<HeaderStyle CssClass="GridCssHeader" />
										<CellStyle CssClass="GridCss" HorizontalAlign="Center">
										</CellStyle>
									</dx:GridViewDataTextColumn>

									<dx:GridViewDataTextColumn Caption="Master" FieldName="Master Service" 
										ShowInCustomizationForm="True" VisibleIndex="12">
										<PropertiesTextEdit><FocusedStyle HorizontalAlign="Center"></FocusedStyle></PropertiesTextEdit>
										<Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
										<HeaderStyle CssClass="GridCssHeader" />
										<CellStyle CssClass="GridCss" HorizontalAlign="Center">
										</CellStyle>
									</dx:GridViewDataTextColumn>

									<dx:GridViewDataTextColumn Caption="Microsoft" FieldName="Microsoft" 
										ShowInCustomizationForm="True" VisibleIndex="13">
										<PropertiesTextEdit><FocusedStyle HorizontalAlign="Center"></FocusedStyle></PropertiesTextEdit>
									<Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
										<HeaderStyle CssClass="GridCssHeader" />
										<CellStyle CssClass="GridCss" HorizontalAlign="Center">
										</CellStyle>
									</dx:GridViewDataTextColumn>

									<dx:GridViewDataTextColumn Visible=false FieldName="IsPrimaryNode" />
									<dx:GridViewDataTextColumn Visible=false FieldName="Alive" />

								</Columns>

								<SettingsBehavior AllowDragDrop="False" AllowSelectByRowClick="True" />
								<Settings ShowGroupPanel="False" ShowFilterRow="True" />
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
				</table>

			</td>
		</tr>
		<tr>
			<td>
				<div id="pnlAreaDtls" style="height: 100%; width: 100%;visibility:hidden" runat="server" class="pnlDetails12">
				</div>
				<table>
					<tr>
						<td>
							<div class="header" id="Div3" runat="server">Assign Servers</div>
						</td>
					</tr>   
					<tr>
						<td>
                                       
							<dx:ASPxGridView ID="AssignNodesGridView"  runat="server" KeyFieldName="ID" 
								Width="100%"  OnPageSizeChanged="AssignNodesGridView_OnPageSizeChanged"
								Theme="Office2003Blue" >
								
								<Columns>			

									<dx:GridViewCommandColumn ShowSelectCheckbox="True" 
										ShowClearFilterButton="true" VisibleIndex="0" SelectAllCheckboxMode="Page" 
										Width="20px" />

									<dx:GridViewDataTextColumn Caption="Name" FieldName="Name" Visible="True" VisibleIndex="1"
										Width="100px">
										<Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
										<EditCellStyle CssClass="GridCss" />
										<EditFormCaptionStyle CssClass="GridCss" />
										<HeaderStyle CssClass="GridCssHeader" />
										<CellStyle CssClass="GridCss" />
									</dx:GridViewDataTextColumn>

									<dx:GridViewDataTextColumn Caption="Servers" FieldName="ServerType" Visible="True"
										VisibleIndex="2" Width="100px">
										<Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
										<EditCellStyle CssClass="GridCss" />
										<EditFormCaptionStyle CssClass="GridCss" />
										<HeaderStyle CssClass="GridCssHeader" />
										<CellStyle CssClass="GridCss" />
									</dx:GridViewDataTextColumn>

									<dx:GridViewDataTextColumn Caption="Location" FieldName="Location" Visible="True" VisibleIndex="3"
										Width="100px">
										<Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
										<EditCellStyle CssClass="GridCss" />
										<EditFormCaptionStyle CssClass="GridCss" />
										<HeaderStyle CssClass="GridCssHeader" />
										<CellStyle CssClass="GridCss" />
									</dx:GridViewDataTextColumn>

									<dx:GridViewDataTextColumn Caption="Assigned Node Name" FieldName="AssignedNodeName" 
										Visible="True" VisibleIndex="4"
										Width="100px">
										<Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
										<EditCellStyle CssClass="GridCss" />
										<EditFormCaptionStyle CssClass="GridCss" />
										<HeaderStyle CssClass="GridCssHeader" />
										<CellStyle CssClass="GridCss" />
									</dx:GridViewDataTextColumn>

									<dx:GridViewDataTextColumn Caption="Current Node Name" FieldName="CurrentNodeName" 
										Visible="True" VisibleIndex="4"
										Width="100px">
										<Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
										<EditCellStyle CssClass="GridCss" />
										<EditFormCaptionStyle CssClass="GridCss" />
										<HeaderStyle CssClass="GridCssHeader" />
										<CellStyle CssClass="GridCss" />
									</dx:GridViewDataTextColumn>

									<dx:GridViewDataTextColumn Caption="ID" FieldName="ID" Visible="False" 
									VisibleIndex="5" />

								</Columns>
								
								<SettingsBehavior AllowDragDrop="False" AllowSelectByRowClick="False" ProcessSelectionChangedOnServer="True" />
								<Settings ShowGroupPanel="False" ShowFilterRow="True" />
								
								<SettingsPager PageSize=50>
									<PageSizeItemSettings Visible="True" />
									
								</SettingsPager>

								<Styles>
									<AlternatingRow CssClass="GridAltRow" />
								</Styles>
							</dx:ASPxGridView>                         
                                            
						</td>
					</tr>
				</table>

				<table>
					<tr>
						<td>
							<div id="NodeSuccess" runat="server" class="alert alert-success" style="display: none">Settings for selected servers were successully updated.
							</div>
							<div id="NodeError" class="alert alert-danger" runat="server" style="display: none">Please select at least one Task and one Server in order to proceed.
							</div>
						</td>
					</tr>
					<tr>
						<td>
							<table>
								<tr>
									<td>
										<asp:Label ID="Label4" runat="server" Text="Nodes:" CssClass="lblsmallFont"></asp:Label>
									</td>
									<td>
										<dx:ASPxComboBox ID="ServerNodes" runat="server" 
											ValueType="System.String" AutoPostBack="False">
											<ValidationSettings ErrorDisplayMode="ImageWithTooltip">
											<RequiredField ErrorText="Select Nodes." />
											<RequiredField ErrorText="Select Location."></RequiredField>
											</ValidationSettings>                                                    
										</dx:ASPxComboBox>
									</td>
									<td>
										<dx:ASPxButton ID="NodesApply" runat="server" Text="Assign Nodes" CssClass="sysButton"
											OnClick="NodesApply_Click">
										</dx:ASPxButton>
									</td>
								</tr>
							</table>  
						</td>                                  
					</tr>
				</table>
			</td>
		</tr>
	</table>





	<%--<script language="javascript">
		window.onload = function () {
			cmbServerType = document.getElementById("ContentPlaceHolder1_ASPxPageControl1_ServerTypeComboBox_I").value;
			if (cmbServerType != 'Exchange') {
				cmbRole.SetVisible(false);
				document.getElementById("ContentPlaceHolder1_ASPxPageControl1_ExchangeRolesLabel").style.visibility = 'hidden';
			}
		};
  
    </script>--%>
</asp:Content>


<%--<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
 <div id="pnlAreaDtls" style="height: 100%; width: 100%;visibility:hidden" runat="server" class="pnlDetails12">
 </div>
    <table>
        <tr>
            <td>
                <div class="header" id="servernamelbldisp" runat="server">Assign Severs To Nodes</div>
				 <div id="errorDiv" class="alert alert-danger" runat="server" style="display: none">Error.
                                        </div>
            </td>
        </tr>
   
        <tr>
            <td>
                <dx:ASPxPageControl ID="ASPxPageControl1" runat="server" Font-Bold="True" 
                    Theme="Glass" ActiveTabIndex="0" Width="100%"> 
                    <TabPages>
                        <dx:TabPage Text="Node Assignments">
                            <TabImage Url="~/images/information.png">
                            </TabImage>
                            <ContentCollection>
                            <dx:ContentControl ID="ContentControl3" runat="server" SupportsDisabledAttribute="True">
                                    <table>
                                    
                                    <tr>
                                        <td>
                                       
											 <dx:ASPxGridView ID="AssignNodesGridView"  runat="server" KeyFieldName="ID" 
											 							Width="100%"  OnPageSizeChanged="AssignNodesGridView_OnPageSizeChanged"
							 Theme="Office2003Blue" >
							<%--<Settings ShowTitlePanel="true" ShowFilterRow="true" ShowFilterBar="Auto" />
        <SettingsBehavior AllowGroup="false" AllowDragDrop="false" />
        <ClientSideEvents Init="OnGridViewInit" SelectionChanged="OnGridViewSelectionChanged" EndCallback="OnGridViewEndCallback" />
        <Styles TitlePanel-CssClass="titleContainer" />--%>
						<%--<Columns>			
								<%--<dx:GridViewCommandColumn Caption="Select" ShowSelectCheckbox="True" 
                                                                VisibleIndex="0">
                                                                <ClearFilterButton Visible="True">
                                                                </ClearFilterButton>
                                                                <HeaderStyle CssClass="GridCssHeader1" />
                                                            </dx:GridViewCommandColumn>
															 --%>
										<%--<dx:GridViewCommandColumn ShowSelectCheckbox="True" 
                                    ShowClearFilterButton="true" VisibleIndex="0" SelectAllCheckboxMode="Page" 
                                    Width="20px" />
										  <%--<dx:GridViewDataColumn FieldName="Name" VisibleIndex="1" />
            <dx:GridViewDataColumn Caption="Servers" FieldName="ServerType" VisibleIndex="2" />
            <dx:GridViewDataColumn FieldName="Location" VisibleIndex="3" />
            <dx:GridViewDataColumn FieldName="NodeName" VisibleIndex="4" />
			 <dx:GridViewDataColumn FieldName="ID" VisibleIndex="5" Visible="false" />--%>
               								<%--<dx:GridViewDataTextColumn Caption="Name" FieldName="Name" Visible="True" VisibleIndex="1"
									Width="100px">
									<Settings AllowAutoFilter="True" AllowDragDrop="False" AutoFilterCondition="Contains" />
									<EditCellStyle CssClass="GridCss">
									</EditCellStyle>
									<EditFormCaptionStyle CssClass="GridCss">
									</EditFormCaptionStyle>
									<HeaderStyle CssClass="GridCssHeader" />
									<CellStyle CssClass="GridCss">
									</CellStyle>
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataTextColumn Caption="Servers" FieldName="ServerType" Visible="True"
									VisibleIndex="2" Width="100px">
									<Settings AllowDragDrop="False" AutoFilterCondition="Contains" />
									<EditCellStyle CssClass="GridCss">
									</EditCellStyle>
									<EditFormCaptionStyle CssClass="GridCss">
									</EditFormCaptionStyle>
									<HeaderStyle CssClass="GridCssHeader" />
									<CellStyle CssClass="GridCss">
									</CellStyle>
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataTextColumn Caption="Location" FieldName="Location" Visible="True" VisibleIndex="3"
									Width="100px">
									<Settings AllowAutoFilter="True" AllowDragDrop="False" AutoFilterCondition="Contains" />
									<EditCellStyle CssClass="GridCss">
									</EditCellStyle>
									<EditFormCaptionStyle CssClass="GridCss">
									</EditFormCaptionStyle>
									<HeaderStyle CssClass="GridCssHeader" />
									<CellStyle CssClass="GridCss">
									</CellStyle>
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataTextColumn Caption="Assigned Node Name" FieldName="AssignedNodeName" 
                                    Visible="True" VisibleIndex="4"
									Width="100px">
									<Settings AllowAutoFilter="True" AllowDragDrop="False" AutoFilterCondition="Contains" />
									<EditCellStyle CssClass="GridCss">
									</EditCellStyle>
									<EditFormCaptionStyle CssClass="GridCss">
									</EditFormCaptionStyle>
									<HeaderStyle CssClass="GridCssHeader" />
									<CellStyle CssClass="GridCss">
									</CellStyle>
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataTextColumn Caption="Current Node Name" FieldName="CurrentNodeName" 
                                    Visible="True" VisibleIndex="4"
									Width="100px">
									<Settings AllowAutoFilter="True" AllowDragDrop="False" AutoFilterCondition="Contains" />
									<EditCellStyle CssClass="GridCss">
									</EditCellStyle>
									<EditFormCaptionStyle CssClass="GridCss">
									</EditFormCaptionStyle>
									<HeaderStyle CssClass="GridCssHeader" />
									<CellStyle CssClass="GridCss">
									</CellStyle>
								</dx:GridViewDataTextColumn>
								           <dx:GridViewDataTextColumn Caption="ID" FieldName="ID" Visible="False" 
                                                                VisibleIndex="5">
                                                            </dx:GridViewDataTextColumn>
							</Columns>
						<%--<Templates>
            <TitlePanel>
                <dx:ASPxLabel ID="lblInfo" ClientInstanceName="info" runat="server" />
                <dx:ASPxHyperLink ID="lnkSelectAllRows" ClientInstanceName="lnkSelectAllRows" OnLoad="lnkSelectAllRows_Load"
                    Text="Select all rows" runat="server" Cursor="pointer" ClientSideEvents-Click="OnSelectAllRowsLinkClick" />
                &nbsp;
                <dx:ASPxHyperLink ID="lnkClearSelection" ClientInstanceName="lnkClearSelection" OnLoad="lnkClearSelection_Load"
                    Text="Clear selection" runat="server" Cursor="pointer" ClientVisible="false" ClientSideEvents-Click="OnUnselectAllRowsLinkClick" />
            </TitlePanel>
        </Templates>
					--%>		
					<%--<SettingsBehavior AllowDragDrop="False" AllowSelectByRowClick="False" ProcessSelectionChangedOnServer="True" />
							<Settings ShowGroupPanel="False" ShowFilterRow="True" />
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
                                </table>
                              
                                   <table>
                                    <tr>
                                        <td>
                                            <div id="NodeSuccess" runat="server" class="alert alert-success" style="display: none">Settings for selected servers were successully updated.
                                            </div>
                                            <div id="NodeError" class="alert alert-danger" runat="server" style="display: none">Please select at least one Task and one Server in order to proceed.
                                            </div>
                                            </td>
                                    </tr>
                                    <tr>
                                    <td>
                                    <table>
                                        <tr>
                                           <td>
                                                <asp:Label ID="Label4" runat="server" Text="Nodes:" CssClass="lblsmallFont"></asp:Label>
                                            </td>
                                            <td>
                                                <dx:ASPxComboBox ID="ServerNodes" runat="server" 
                                                    ValueType="System.String" AutoPostBack="False">
                                                    <ValidationSettings ErrorDisplayMode="ImageWithTooltip">
                                                        <RequiredField ErrorText="Select Nodes." />
<RequiredField ErrorText="Select Location."></RequiredField>
                                                    </ValidationSettings>                                                    
                                                </dx:ASPxComboBox>
                                            </td>
                                            <td>
                                            <dx:ASPxButton ID="NodesApply" runat="server" Text="Assign Nodes" CssClass="sysButton"
                                                OnClick="NodesApply_Click">
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

			                      <dx:TabPage Text="Node Settings">
                            <TabImage Url="~/images/information.png">
                            </TabImage>
                            <ContentCollection>
                            <dx:ContentControl ID="ContentControl1" runat="server" SupportsDisabledAttribute="True">
								<%-- <div style="overflow: auto; height: 60px; font-size: 12px; font-weight: normal; font-family: Arial, Helvetica, sans-serif; text-align:left; color:black; width:350px;"  id="divmsg" runat="server"></div>--%>
								<%--
                                    <table>
									<tr>
									<td>
									      <dx:ASPxGridView runat="server" CssFilePath="~/App_Themes/Office2010Silver/{0}/styles.css"
                                CssPostfix="Office2010Silver" KeyFieldName="ID" AutoGenerateColumns="False"
                                Width="100%" ID="NodesGridView"  OnRowDeleting="NodesGridView_RowDeleting" OnPageSizeChanged="NodesGridView_PageSizeChanged"   
                                 OnHtmlRowCreated="NodesGridView_HtmlRowCreated" Cursor="pointer" 
                                   EnableTheming="True" Theme="Office2003Blue" ClientInstanceName="NodesGridView">
								    <ClientSideEvents CustomButtonClick="OnCustomButtonClick" /> 
                                <Columns>
                                        <dx:GridViewCommandColumn ButtonType="Image" Caption="Actions"
                                        VisibleIndex="0" Width="80px">
                                        <EditButton Visible="True">
                                            <Image Url="../images/edit.png">
                                            </Image>
                                        </EditButton>
										<NewButton Visible="True" >
                                        <Image Url="../images/icons/add.png">
                                        </Image>
                                        </NewButton>
										<DeleteButton Visible="False" >
                                            <Image Url="../images/delete.png">
                                            </Image>
											</DeleteButton>
                                        <CellStyle CssClass="GridCss1">
                                            <Paddings Padding="3px" />
                                           <Paddings Padding="3px"></Paddings>
                                        </CellStyle>
                                        <ClearFilterButton Visible="True">
                                            <Image Url="~/images/clear.png">
                                            </Image>
                                        </ClearFilterButton>
                                        <HeaderStyle CssClass="GridCssHeader1" />
                                    </dx:GridViewCommandColumn>


								<%--	 <dx:GridViewCommandColumn Caption="Delete" ButtonType="Image" VisibleIndex="0">
                        <CustomButtons>
                            <dx:GridViewCommandColumnCustomButton ID="deleteButton" Image-Url="../images/delete.png" />
                        </CustomButtons>
						<HeaderStyle CssClass="GridCssHeader" />
                    </dx:GridViewCommandColumn>--%>

					<%--
                    <dx:GridViewCommandColumn Caption="Delete" ButtonType="Image" VisibleIndex="1" 
                            Width="50px">
                                               <CustomButtons>
                                           <dx:GridViewCommandColumnCustomButton ID="deleteButton" 
                                                       Image-Url="../images/delete.png" >
<Image Url="../images/delete.png"></Image>
                                                   </dx:GridViewCommandColumnCustomButton>
                                               </CustomButtons>
						                      <HeaderStyle CssClass="GridCssHeader1" />
                                                </dx:GridViewCommandColumn>
                                    <dx:GridViewDataTextColumn Caption="Name" FieldName="Name" 
                                        VisibleIndex="2">
                                        <PropertiesTextEdit>
					<ValidationSettings>
						<RequiredField IsRequired="True" />
					</ValidationSettings>
				</PropertiesTextEdit>
                                        <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        <HeaderStyle CssClass="GridCssHeader" />
                                        <CellStyle CssClass="GridCss">
                                        </CellStyle>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Host Name" FieldName="HostName"
                                        VisibleIndex="3">
                                        <Settings AutoFilterCondition="Contains" />
                                        <EditCellStyle CssClass="GridCss">
                                        </EditCellStyle>
                                        <EditFormCaptionStyle CssClass="GridCss"  HorizontalAlign="Left">
                                        </EditFormCaptionStyle>
                                        <HeaderStyle CssClass="GridCssHeader" />
                                        <CellStyle CssClass="GridCss">
                                        </CellStyle>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Alive" FieldName="Alive"
                                        VisibleIndex="4">
                                        <Settings AutoFilterCondition="Contains" />
                                        <EditCellStyle CssClass="GridCss">
                                        </EditCellStyle>
                                        <EditFormCaptionStyle CssClass="GridCss"  HorizontalAlign="Left">
                                        </EditFormCaptionStyle>
                                        <HeaderStyle CssClass="GridCssHeader" />
                                        <CellStyle CssClass="GridCss">
                                        </CellStyle>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Version" FieldName="Version"
                                         VisibleIndex="5">
                                        <EditCellStyle CssClass="GridCss">
                                        </EditCellStyle>
                                        <EditFormCaptionStyle CssClass="GridCss">
                                        </EditFormCaptionStyle>
                                        <HeaderStyle CssClass="GridCssHeader" />
                                        <CellStyle CssClass="GridCss">
                                        </CellStyle>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Credential" FieldName="Credential"
                                         VisibleIndex="6" Visible = "false">
                                        <EditCellStyle CssClass="GridCss">
                                        </EditCellStyle>
                                        <EditFormCaptionStyle CssClass="GridCss">
                                        </EditFormCaptionStyle>
                                        <HeaderStyle CssClass="GridCssHeader" />
                                        <CellStyle CssClass="GridCss">
                                        </CellStyle>
                                    </dx:GridViewDataTextColumn>
									<dx:GridViewDataTextColumn Caption="Node Type" FieldName="NodeType"
                                         VisibleIndex="7">
                                        <EditCellStyle CssClass="GridCss">
                                        </EditCellStyle>
                                        <EditFormCaptionStyle CssClass="GridCss">
                                        </EditFormCaptionStyle>
                                        <HeaderStyle CssClass="GridCssHeader" />
                                        <CellStyle CssClass="GridCss">
                                        </CellStyle>
                                    </dx:GridViewDataTextColumn>
									<dx:GridViewDataTextColumn Caption="Load Factor" FieldName="LoadFactor"
                                         VisibleIndex="8">
                                        <EditCellStyle CssClass="GridCss">
                                        </EditCellStyle>
                                        <EditFormCaptionStyle CssClass="GridCss">
                                        </EditFormCaptionStyle>
                                        <HeaderStyle CssClass="GridCssHeader" />
                                        <CellStyle CssClass="GridCss">
                                        </CellStyle>
                                    </dx:GridViewDataTextColumn>
									<dx:GridViewDataTextColumn Caption="Primary Node" FieldName="IsPrimaryNode"
                                         VisibleIndex="10">
                                        <EditCellStyle CssClass="GridCss">
                                        </EditCellStyle>
                                        <EditFormCaptionStyle CssClass="GridCss">
                                        </EditFormCaptionStyle>
                                        <HeaderStyle CssClass="GridCssHeader" />
                                        <CellStyle CssClass="GridCss">
                                        </CellStyle>
                                    </dx:GridViewDataTextColumn>
									<dx:GridViewDataTextColumn Caption="Location" FieldName="Location"
                                         VisibleIndex="11">
                                        <EditCellStyle CssClass="GridCss">
                                        </EditCellStyle>
                                        <EditFormCaptionStyle CssClass="GridCss">
                                        </EditFormCaptionStyle>
                                        <HeaderStyle CssClass="GridCssHeader" />
                                        <CellStyle CssClass="GridCss">
                                        </CellStyle>
                                    </dx:GridViewDataTextColumn>
                                </Columns>
                     <SettingsBehavior ConfirmDelete="True" ColumnResizeMode="NextColumn" 
                                    AllowSelectByRowClick="True" ProcessSelectionChangedOnServer="True" 
                                    AllowSelectSingleRowOnly="True"></SettingsBehavior>
                                <Settings ShowFilterRow="True" ShowGroupPanel="False" />

                            <Settings ShowFilterRow="True" ShowGroupPanel="False"></Settings>

                                <SettingsText ConfirmDelete="Are you sure you want to delete?"></SettingsText>
                                <Styles CssPostfix="Office2010Silver" CssFilePath="~/App_Themes/Office2010Silver/{0}/styles.css">
                                    <Header ImageSpacing="5px" SortingImageSpacing="5px" CssClass="GridCssHeader">
                                    </Header>
                                      <GroupRow Font-Bold="True">
                                    </GroupRow>
                                      <AlternatingRow CssClass="GridAltRow" Enabled="True">
                        </AlternatingRow>
                                    <Cell CssClass="GridCss">
                                    </Cell>
                                    <LoadingPanel ImageSpacing="5px">
                                    </LoadingPanel>
                                </Styles>
                                <StylesEditors ButtonEditCellSpacing="0">
                                    <ProgressBar Height="21px">
                                    </ProgressBar>
                                </StylesEditors>
                                <SettingsPager PageSize="50" SEOFriendly="Enabled" >
            <PageSizeItemSettings Visible="true" />
<PageSizeItemSettings Visible="True"></PageSizeItemSettings>
        </SettingsPager>   
                            </dx:ASPxGridView>
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
</asp:Content>--%>