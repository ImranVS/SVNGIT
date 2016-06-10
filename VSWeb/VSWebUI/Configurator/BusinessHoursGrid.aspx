<%@ Page Title="VitalSigns Plus-MaintenanceWin" Language="C#" MasterPageFile="~/Site1.Master"
	AutoEventWireup="true" CodeBehind="BusinessHoursGrid.aspx.cs" Inherits="VSWebUI.Configurator.BusinessHoursGrid" %>

<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.Web" TagPrefix="dx" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href='http://fonts.googleapis.com/css?family=Francois One' rel='stylesheet'
		type='text/css' />
	<link rel="stylesheet" type="text/css" href="../css/vswebforms.css" />
	<script type="text/javascript">


		var visibleIndex;
		function OnCustomButtonClick(s, e) {
			visibleIndex = e.visibleIndex;

			if (e.buttonID == "deleteButton") {
			    BusinessrGridView.GetRowValues(e.visibleIndex, 'Type', OnGetRowValues);
			}

			function OnGetRowValues(values) {
				var id = values[0];
				var name = values[1];
                //5/21/2015 NS modified for VSPLUS-1771
				var OK = (confirm('Are you sure you want to delete the hours definition - ' + values + '?'))
				if (OK == true) {
					BusinessrGridView.DeleteRow(visibleIndex);

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
    <div id="pnlAreaDtls" style="height: 100%; width: 100%;visibility:hidden" runat="server" class="pnlDetails12">
     <table align="center" width="30%" style="height: 100%">
                    <tr>
                        <td align="center" valign="middle" style="height: auto;">
                            <table border="1" cellspacing="0" class="csline" cellpadding="2px" id="table_txt_edit" style="border-width:1px; border-style: solid;  border-collapse: collapse;  border-color: silver; background-color: #F8F8FF"  
                                width="100%">
                                <tr style="background-color:White">
                                  
                                    <td align="left">
                                      <div class="subheading">Delete Bussiness Hours</div>
                                    </td>
                                </tr>
                                <tr>
                                
                                    <td align="center">
                                    <table><tr><td valign="top">
                                
                                    </td><td align="center">
                                        <div style="overflow: auto; height: 60px; font-size: 12px; font-weight: normal; font-family: Arial, Helvetica, sans-serif; text-align:left; color:black; width:350px;"  id="divmsg" runat="server"></div>
                                              <asp:Button ID="btnok1" runat="server" OnClick="btn_OkClick" OnClientClick="hidepopup()" Text="OK" Width="50px" Font-Names="Arial" Font-Size="Small" />
                                              <asp:Button ID="btncancel1" runat="server" OnClick="btn_CancelClick" OnClientClick="hidepopup()" Text="Cancel"  Width="70px" Font-Names="Arial" Font-Size="Small" />
                                           
                                        </td>
                                        
                                        </tr></table>
                                        
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
					Hours Definitions</div>
                <div class="info">Hours definitions below may be used when configuring server scanning and maintenance as well as alerting. <b>Note:</b> the default definition for Business Hours may not be renamed or deleted.</div>
                <dx:ASPxButton ID="NewButton" runat="server" Text="New" CssClass="sysButton" 
                    onclick="NewButton_Click">
                    <Image Url="~/images/icons/add.png">
                                            </Image>
                </dx:ASPxButton>
			</td>
		</tr>
	</table>
	<asp:UpdatePanel ID="UpdatePanel1" runat="server">
		<ContentTemplate>
			<table>
				<tr>
					<td>
						<dx:ASPxGridView runat="server" 
							CssFilePath="~/App_Themes/Office2010Silver/{0}/styles.css"  ClientInstanceName="BusinessrGridView"
							CssPostfix="Office2010Silver" KeyFieldName="ID" AutoGenerateColumns="False"  OnPageSizeChanged="BusinessrGridView_PageSizeChanged"
							ID="BusinessrGridView" OnHtmlRowCreated="BusinessrGridView_HtmlRowCreated" Cursor="pointer"
							OnRowDeleting="BusinessrGridView_RowDeleting" Theme="Office2003Blue" 
							oncustombuttoninitialize="BusinessrGridView_CustomButtonInitialize" EnableCallBacks=false>
							<ClientSideEvents CustomButtonClick="OnCustomButtonClick" />
							<Columns>
								<dx:GridViewCommandColumn ButtonType="Image" Caption="Actions" VisibleIndex="0" Width="60px">
									<EditButton Visible="True">
										<Image Url="../images/edit.png">
										</Image>
									</EditButton>
									<NewButton Visible="True">
										<Image Url="../images/icons/add.png">
										</Image>
									</NewButton>
									<DeleteButton Visible="False">
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
									<CellStyle CssClass="GridCss1">
										<Paddings Padding="3px" />
										<Paddings Padding="3px"></Paddings>
									</CellStyle>
									<ClearFilterButton Visible="True">
									</ClearFilterButton>
									<HeaderStyle CssClass="GridCssHeader1" />
								</dx:GridViewCommandColumn>
						
				<%--    <dx:GridViewDataTextColumn Caption="Delete" VisibleIndex="1" CellStyle-HorizontalAlign="Center" Width="30px">
					<DataItemTemplate>
            
					 <asp:Label ID="lblservername" runat="server" Text='<%#Eval("Type") %>' Visible="false"></asp:Label>
					  <asp:ImageButton ID="btndele" runat="server" ImageUrl="../images/delete.png" Width="15px" Height="15px" CommandName="Delete" CommandArgument='<%#Eval("ID") %>' AlternateText='<%#Eval("Type") %>' ToolTip="Delete" OnClick="btn_Click" />
					</DataItemTemplate>
					<EditFormSettings Visible="False"/>
					<EditCellStyle CssClass="GridCss"></EditCellStyle><EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle><HeaderStyle CssClass="GridCssHeader" />
												<CellStyle CssClass="GridCss1"></CellStyle>
		<CellStyle HorizontalAlign="Center"></CellStyle>
				 </dx:GridViewDataTextColumn>--%>
				  <dx:GridViewCommandColumn Caption="Delete" ButtonType="Image" VisibleIndex="1" Width="50px" 
									ToolTip="Delete">
								<CustomButtons>
									<dx:GridViewCommandColumnCustomButton ID="deleteButton" Text="Delete" Image-ToolTip="Delete"
										Image-Url="../images/delete.png" >
		<Image Url="../images/delete.png"></Image>
									</dx:GridViewCommandColumnCustomButton>
								</CustomButtons>
								<HeaderStyle CssClass="GridCssHeader1" />
								<CellStyle CssClass="GridCss1">
								</CellStyle>
							</dx:GridViewCommandColumn>
								<dx:GridViewDataTextColumn FieldName="Type" Caption="Name">
									<Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
									<Settings AllowAutoFilter="True" AutoFilterCondition="Contains"></Settings>
									<HeaderStyle CssClass="GridCssHeader"></HeaderStyle>
									<CellStyle CssClass="GridCss">
									</CellStyle>
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataTextColumn FieldName="Starttime" Caption="Start Time" 
									Width="70px">
									<Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
									<Settings AllowAutoFilter="True" AutoFilterCondition="Contains"></Settings>
									<HeaderStyle CssClass="GridCssHeader"></HeaderStyle>
									<CellStyle CssClass="GridCss">
									</CellStyle>
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataTextColumn FieldName="Duration" Caption="Duration" 
									VisibleIndex="4" Width="60px">
									<Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
									<Settings AllowAutoFilter="True" AutoFilterCondition="Contains"></Settings>
									<HeaderStyle CssClass="GridCssHeader2"></HeaderStyle>
									<CellStyle CssClass="GridCss2">
									</CellStyle>
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataCheckColumn Caption="Sunday" FieldName="Issunday" 
									Width="80px">
									<Settings AllowAutoFilter="False" />
									<Settings AllowAutoFilter="False"></Settings>
									<EditCellStyle CssClass="GridCss">
									</EditCellStyle>
									<EditFormCaptionStyle CssClass="GridCss">
									</EditFormCaptionStyle>
									<HeaderStyle CssClass="GridCssHeader1" Wrap="False">
										<Paddings Padding="5px" />
										<Paddings Padding="5px"></Paddings>
									</HeaderStyle>
									<CellStyle CssClass="GridCss1">
										<Paddings Padding="5px" />
										<Paddings Padding="5px"></Paddings>
									</CellStyle>
								</dx:GridViewDataCheckColumn>
								<dx:GridViewDataCheckColumn Caption="Monday" FieldName="IsMonday" 
									Width="80px">
									<Settings AllowAutoFilter="False" />
									<Settings AllowAutoFilter="False"></Settings>
									<EditCellStyle CssClass="GridCss">
									</EditCellStyle>
									<EditFormCaptionStyle CssClass="GridCss">
									</EditFormCaptionStyle>
									<HeaderStyle CssClass="GridCssHeader1" Wrap="False">
										<Paddings Padding="5px" />
										<Paddings Padding="5px"></Paddings>
									</HeaderStyle>
									<CellStyle CssClass="GridCss1">
										<Paddings Padding="5px" />
										<Paddings Padding="5px"></Paddings>
									</CellStyle>
								</dx:GridViewDataCheckColumn>
								<dx:GridViewDataCheckColumn Caption="Tuesday" FieldName="IsTuesday" 
									Width="80px">
									<Settings AllowAutoFilter="False" />
									<Settings AllowAutoFilter="False"></Settings>
									<EditCellStyle CssClass="GridCss">
									</EditCellStyle>
									<EditFormCaptionStyle CssClass="GridCss">
									</EditFormCaptionStyle>
									<HeaderStyle CssClass="GridCssHeader1" Wrap="False">
										<Paddings Padding="5px" />
										<Paddings Padding="5px"></Paddings>
									</HeaderStyle>
									<CellStyle CssClass="GridCss1">
										<Paddings Padding="5px" />
										<Paddings Padding="5px"></Paddings>
									</CellStyle>
								</dx:GridViewDataCheckColumn>
								<dx:GridViewDataCheckColumn Caption="Wednesday" FieldName="IsWednesday" 
									Width="80px">
									<Settings AllowAutoFilter="False" />
									<Settings AllowAutoFilter="False"></Settings>
									<EditCellStyle CssClass="GridCss">
									</EditCellStyle>
									<EditFormCaptionStyle CssClass="GridCss">
									</EditFormCaptionStyle>
									<HeaderStyle CssClass="GridCssHeader1" Wrap="False">
										<Paddings Padding="5px"></Paddings>
									</HeaderStyle>
									<CellStyle CssClass="GridCss1">
										<Paddings Padding="5px" />
										<Paddings Padding="5px"></Paddings>
									</CellStyle>
								</dx:GridViewDataCheckColumn>
								<dx:GridViewDataCheckColumn Caption="Thursday" FieldName="IsThursday" 
									Width="80px">
									<Settings AllowAutoFilter="False" />
									<Settings AllowAutoFilter="False"></Settings>
									<EditCellStyle CssClass="GridCss">
									</EditCellStyle>
									<EditFormCaptionStyle CssClass="GridCss">
									</EditFormCaptionStyle>
									<HeaderStyle CssClass="GridCssHeader1" Wrap="False">
										<Paddings Padding="5px" />
										<Paddings Padding="5px"></Paddings>
									</HeaderStyle>
									<CellStyle CssClass="GridCss1">
										<Paddings Padding="5px" />
										<Paddings Padding="5px"></Paddings>
									</CellStyle>
								</dx:GridViewDataCheckColumn>
								<dx:GridViewDataCheckColumn Caption="Friday" FieldName="IsFriday" 
									Width="80px">
									<Settings AllowAutoFilter="False" />
									<Settings AllowAutoFilter="False"></Settings>
									<EditCellStyle CssClass="GridCss">
									</EditCellStyle>
									<EditFormCaptionStyle CssClass="GridCss">
									</EditFormCaptionStyle>
									<HeaderStyle CssClass="GridCssHeader1" Wrap="False">
										<Paddings Padding="5px" />
										<Paddings Padding="5px"></Paddings>
									</HeaderStyle>
									<CellStyle CssClass="GridCss1">
										<Paddings Padding="5px" />
										<Paddings Padding="5px"></Paddings>
									</CellStyle>
								</dx:GridViewDataCheckColumn>
								<dx:GridViewDataCheckColumn Caption="Saturday" FieldName="Issaturday" 
									Width="80px">
									<Settings AllowAutoFilter="False" />
									<Settings AllowAutoFilter="False"></Settings>
									<EditCellStyle CssClass="GridCss">
									</EditCellStyle>
									<EditFormCaptionStyle CssClass="GridCss">
									</EditFormCaptionStyle>
									<HeaderStyle CssClass="GridCssHeader1" Wrap="False">
										<Paddings Padding="5px" />
										<Paddings Padding="5px"></Paddings>
									</HeaderStyle>
									<CellStyle CssClass="GridCss1">
										<Paddings Padding="5px" />
										<Paddings Padding="5px"></Paddings>
									</CellStyle>
								</dx:GridViewDataCheckColumn>
							</Columns>
							<SettingsBehavior ConfirmDelete="True" ColumnResizeMode="NextColumn" AllowSelectByRowClick="True"
								ProcessSelectionChangedOnServer="True" AllowSelectSingleRowOnly="True"></SettingsBehavior>
							<Settings ShowFilterRow="True" ShowGroupPanel="True" />
							<Settings ShowFilterRow="True" ShowGroupPanel="True"></Settings>
							<SettingsText ConfirmDelete="Are you sure you want to delete?"></SettingsText>
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
							<SettingsPager PageSize="50" SEOFriendly="Enabled">
								<PageSizeItemSettings Visible="true" />
								<PageSizeItemSettings Visible="True">
								</PageSizeItemSettings>
							</SettingsPager>
						</dx:ASPxGridView>
					</td>
				</tr>
		
				<tr>
				<td>
					<dx:ASPxPopupControl ID="NavigatorPopupControl" runat="server" ClientInstanceName="popup1" 
												HeaderText="Information" Modal="True" PopupHorizontalAlign="WindowCenter" 
												PopupVerticalAlign="WindowCenter" Theme="MetropolisBlue" Width=400px>
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
																	<dx:ASPxButton ID="OKButton" runat="server" OnClick="OKButton_Click" Text="OK" CssClass="sysButton">
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
		</ContentTemplate>
		
	</asp:UpdatePanel>
	
</asp:Content>
