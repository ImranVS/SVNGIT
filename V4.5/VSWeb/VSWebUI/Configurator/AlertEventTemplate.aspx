<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="AlertEventTemplate.aspx.cs" Inherits="VSWebUI.Configurator.AlertEventTemplate" %>
<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.Web" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

	<script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.5/jquery.min.js"></script>
	<script src="../js/jquery-1.9.1.js" type="text/javascript"></script>
	<script src="../js/bootstrap.min.js" type="text/javascript"></script>
	<script type="text/javascript">
		$(document).ready(function () {
			$('.alert-success').delay(10000).fadeOut("slow", function () {
			});
		});
		
		</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<script language="javascript" type="text/javascript">
	function OnItemClick(s, e) {
		if (e.item.parent == s.GetRootItem())
			e.processOnServer = false;
	}
	var visibleIndex;
	function OnCustomButtonClick(s, e) {
		visibleIndex = e.visibleIndex;

		if (e.buttonID == "deleteButton")
			AlertEventTEmplategrid.GetRowValues(e.visibleIndex, 'Name', OnGetRowValues);

		function OnGetRowValues(values) {
			var id = values[0];
			var name = values[1];
			
			var OK = (confirm('Are you sure you want to delete  Event Template - ' + values + '?'))
			if (OK == true) {

				AlertEventTEmplategrid.DeleteRow(visibleIndex);
				
			}


		}
	}
</script>
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
<table width="100px" align="right">
	<tr>
	<td align="right">
	<table id ="menutable" runat="server">
	<tr>
	
	<td align="right">
	
	<dx:ASPxMenu ID="ASPxMenu1" runat="server" EnableTheming="True" 
                                HorizontalAlign="Right"  onitemclick="ASPxMenu1_ItemClick" ShowAsToolbar="True" 
                                Theme="Moderno">
                                <ClientSideEvents ItemClick="OnItemClick" />
                                <Items>
                                    <dx:MenuItem Name="MainItem">
                                        <Items>
                                
                                            <dx:MenuItem Name="Myaccountdetails" Text="Alert Definitions">
                                            </dx:MenuItem>
                               
                                        </Items>
                                        <Image Url="~/images/icons/Gear.png">
                                        </Image>
                                    </dx:MenuItem>
                                </Items>
                            </dx:ASPxMenu>
	</td>
	</tr>
	</table>
	</td>
	</tr>
	</table>
<table>
<tr>
            <td>
                <div class="header" id="servernamelbldisp" runat="server">Alert Event Template</div>
            </td>
        </tr>
        <tr>
            <td>
			  <div id="successDiv" class="alert alert-success" runat="server" style="display: none">Success.
                </div>
                <dx:ASPxButton ID="NewButton" runat="server" Text="New"  CssClass="sysButton" 
                    onclick="NewButton_Click">
                    <Image Url="~/images/icons/add.png">
                                            </Image>
                </dx:ASPxButton>
            </td>
        </tr>
<tr>
<td>
<dx:ASPxGridView runat="server" 
							CssFilePath="~/App_Themes/Office2010Silver/{0}/styles.css"  ClientInstanceName="AlertEventTEmplategrid"
							CssPostfix="Office2010Silver" KeyFieldName="ID" AutoGenerateColumns="False"  
							ID="AlertEventTEmplategrid"  Cursor="pointer"
							 Theme="Office2003Blue" 
							 EnableCallBacks=false 
		onhtmlrowcreated="AlertEventTEmplategrid_HtmlRowCreated" 
		onrowdeleting="AlertEventTEmplategrid_RowDeleting" >
						<ClientSideEvents CustomButtonClick="OnCustomButtonClick" />
					
							<Columns>
								<dx:GridViewCommandColumn ButtonType="Image" Caption="Actions" VisibleIndex="0"  Width="70px">
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
									<CellStyle CssClass="GridCss1">
										<Paddings Padding="3px" />
										<Paddings Padding="3px"></Paddings>
									</CellStyle>
									<ClearFilterButton Visible="True">
									</ClearFilterButton>
									<HeaderStyle CssClass="GridCssHeader1" />
								</dx:GridViewCommandColumn>
							 <dx:GridViewCommandColumn Caption="Delete" ButtonType="Image"  Width="50px">
                        <CustomButtons>
                            <dx:GridViewCommandColumnCustomButton ID="deleteButton" Text="Delete" 
                                Image-Url="../images/delete.png" >
<Image Url="../images/delete.png"></Image>
                            </dx:GridViewCommandColumnCustomButton>
                        </CustomButtons>
						<HeaderStyle CssClass="GridCssHeader" />
                    </dx:GridViewCommandColumn>
		
	
								<dx:GridViewDataTextColumn FieldName="ID" Visible="false"
									>
									<Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
									<Settings AllowAutoFilter="True" AutoFilterCondition="Contains"></Settings>
									<HeaderStyle CssClass="GridCssHeader"></HeaderStyle>
									<CellStyle CssClass="GridCss">
									</CellStyle>
								</dx:GridViewDataTextColumn>
							
				
								<dx:GridViewDataTextColumn FieldName="Name" Caption="Alert Event Template Name" 
									Width="100px">
									<Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
									<Settings AllowAutoFilter="True" AutoFilterCondition="Contains"></Settings>
									<HeaderStyle CssClass="GridCssHeader"></HeaderStyle>
									<CellStyle CssClass="GridCss">
									</CellStyle>
								</dx:GridViewDataTextColumn>
							
							
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
