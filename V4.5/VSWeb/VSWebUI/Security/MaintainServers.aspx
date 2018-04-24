<%@ Page Title="VitalSigns Plus - Maintain Servers" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="MaintainServers.aspx.cs" Inherits="VSWebUI.Security.MaintainServers" %>
<%@ MasterType virtualpath="~/Site1.Master" %>
<%@ Register assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>

<%@ Register assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx1" %>
<%@ Register assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx11" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
	<link href='http://fonts.googleapis.com/css?family=Francois One' rel='stylesheet'
		type='text/css' />
	<link rel="stylesheet" type="text/css" href="../css/vswebforms.css" />
      <script src="../js/jquery-1.9.1.js" type="text/javascript"></script>
    <script src="../js/bootstrap.min.js" type="text/javascript"></script>
	<script type="text/javascript">
	        $(document).on('keypress', '#ContentPlaceHolder1_ServersGridView_DXEditor3_I,#ContentPlaceHolder1_ServersGridView_DXEditor5_I', function (e) {
	            if (e.which == 39) {
	                alert('No Single Quotes, Please!');
	                return false;
	            }
	        });


		var visibleIndex;
		function OnCustomButtonClick(s, e) {
	   
	
			visibleIndex = e.visibleIndex;

			if (e.buttonID == "deleteButton")
				ServersGridView.GetRowValues(e.visibleIndex, 'ServerName', OnGetRowValues);

			function OnGetRowValues(values) {
				var id = values[0];
				var name = values[1];
				//5/21/2015 NS modified for VSPLUS-1771
				var OK = (confirm('Are you sure you want to delete the server - ' + values + '?'))
				if (OK == true) {
//					alert('Before Delete');
//					var name1 = ServersGridView.DeleteRow(visibleIndex);
					ServersGridView.DeleteRow(visibleIndex);
					//alert('The Server ' + values + ' was Successfully Deleted');
//					alert(name1);
					//ScriptManager.RegisterClientScriptBlock(base.Page, this.GetType(), "FooterRequired", "alert('Notification : Record deleted successfully');", true);
				}

				else {
				}

			}
		}

		

		
		//            function OnGetRowValues(values) {

		//            	window.location.href = 'MaintainServers.aspx?ID=' + values;

		//            }
		//            function OnClickYes(s, e) {
		//            	//if (confirm('Are you want to delete this server ' + id + '?'))
		//            	ServersGridView.DeleteRow(visibleIndex);
		//                popup.Hide();
		//            }
		//            function OnClickNo(s, e) {
		//            	popup.Hide();
		//            }
		//		function hidepopup() {

		//			var popup = document.getElementById('ContentPlaceHolder1_pnlAreaDtls');
		//			popup.style.visibility = 'hidden';

		//		}
		//		
		//    function OnLinkClick(id, visibleIndex) {
		//        if(confirm('Are you want to delete this server ' + id + '?'))
		//        	ServersGridView.DeleteRow(visibleIndex);
		//    }


		//		function ServersGridView_EndCallback(s, e) {
		//			if (s.cpShowDeleteConfirmBox)
		//				pcConfirm.Show();
		//		}

		//		function Yes_Click() {
		//			ServersGridView.DeleteRow(ServersGridView.cpRowIndex);
		//			pcConfirm.Hide()
		//		}

		//		function No_Click() {
		//			pcConfirm.Hide()
		//		}


		function OnSelectedIndexChanged(s, e) {
			// alert(s.GetValue());
			// servergrid.GetEditor("ServerType").PerformCallback(s.GetValue());
			if (s.GetText().toString() == "Database Availability Group") {

				$("label[for='ContentPlaceHolder1_ServersGridView_DXEditor7_I']").parent('td').parent('tr').hide();
				//            $("label[for='ContentPlaceHolder1_ServersGridView_DXEditor7_EI']").parent('td').parent('tr').hide();
				//            $("label[for='ContentPlaceHolder1_ServersGridView_DXEditor7_ET']").hide();

			}
			else {
				$("label[for='ContentPlaceHolder1_ServersGridView_DXEditor7_I']").parent('td').parent('tr').show();
			}

}


	</script>
	 
	
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server" >
	<asp:Button ID="btnDisable" runat="server" OnClientClick="return false;" style="display:none;" UseSubmitBehavior="true"/>
           <asp:Label ID="Label1" runat="server" Style="font-weight: bold; color: #f00;"> </asp:Label>

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
					Maintain Servers</div>
			</td>
		</tr>
	</table>
	<div id="successDiv" runat="server" class="success" style="display: none">
                                Account settings were successully updated.
                            </div>
	<table>
        <tr>
            <td>
                <dx:ASPxButton ID="NewButton" runat="server" Text="New" CssClass="sysButton" 
                    AutoPostBack="False">
                    <ClientSideEvents Click="function() { ServersGridView.AddNewRow(); }" />
                    <Image Url="~/images/icons/add.png">
                    </Image>
                </dx:ASPxButton>
            </td>
        </tr>
    </table>
    <table width="100%">
        <tr>
            <td>
                <dx:ASPxGridView runat="server" KeyFieldName="ID" AutoGenerateColumns="False" 
        ID="ServersGridView" ClientInstanceName="ServersGridView"
		 OnRowDeleting="ServersGridView_RowDeleting" OnRowInserting="ServersGridView_RowInserting"
		OnRowUpdating="ServersGridView_RowUpdating" OnPageSizeChanged="ServersGridView_PageSizeChanged"
		OnCellEditorInitialize="ServersGridView_CellEditorInitialize" Width="100%" OnAutoFilterCellEditorInitialize="ServersGridView_AutoFilterCellEditorInitialize"
		Theme="Office2003Blue" EnableTheming="True" 
        oncustomerrortext="ServersGridView_CustomErrorText"  >
<ClientSideEvents CustomButtonClick="OnCustomButtonClick" />

		<Columns>
			<dx:GridViewCommandColumn ButtonType="Image" Caption="Actions" FixedStyle="Left"
				VisibleIndex="0" Width="40px">
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
				</CellStyle>
				<ClearFilterButton Visible="True">
					<Image Url="~/images/clear.png">
					</Image>
				</ClearFilterButton>
				<HeaderStyle CssClass="GridCssHeader" />
			</dx:GridViewCommandColumn>
			 <%--<dx:GridViewDataTextColumn Name="Delete" VisibleIndex="0" Visible="true" Caption="Delete">
            <DataItemTemplate>
                <a ID="DeleteButton"  href="javascript:OnLinkClick(<%# Container.KeyValue.ToString()%>, <%# Container.VisibleIndex.ToString()%> );">
				<dx:ASPxImage ID="delete" runat="server" ImageUrl="../images/delete.png"></dx:ASPxImage>
				 </a>
            </DataItemTemplate>
			<HeaderStyle CssClass="GridCssHeader" />
         </dx:GridViewDataTextColumn>--%>
		 <dx:GridViewCommandColumn Caption="Delete" ButtonType="Image" VisibleIndex="1">
                        <CustomButtons>
                            <dx:GridViewCommandColumnCustomButton ID="deleteButton" Text="Delete" 
                                Image-Url="../images/delete.png" >
<Image Url="../images/delete.png"></Image>
                            </dx:GridViewCommandColumnCustomButton>
                        </CustomButtons>
						<HeaderStyle CssClass="GridCssHeader" />
                    </dx:GridViewCommandColumn>
			<%--<dx:GridViewCommandColumn ButtonType="Image" VisibleIndex="0" Caption="Delete">
				<DeleteButton Visible="true">
					<Image Url="../images/delete.png">
					</Image>
					
				</DeleteButton>
				
				<HeaderStyle CssClass="GridCssHeader" 
			</dx:GridViewCommandColumn>--%>
			<dx:GridViewDataTextColumn FieldName="ID" ReadOnly="True" Visible="False" 
                VisibleIndex="2">
				<EditFormSettings Visible="False"></EditFormSettings>
				<EditCellStyle CssClass="GridCss">
				</EditCellStyle>
				<EditFormCaptionStyle CssClass="GridCss">
				</EditFormCaptionStyle>
				<HeaderStyle CssClass="GridCssHeader" />
				<CellStyle CssClass="GridCss">
				</CellStyle>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn FieldName="ServerName" VisibleIndex="3">
				<PropertiesTextEdit>
               <ValidationSettings>
                 <%-- <RegularExpression ValidationExpression ="^[a-zA-Z0-9 _&,.-]+$" ErrorText="Single Quotes are not allowed" />--%>
						<RequiredField IsRequired="True" />
					</ValidationSettings>
                     </PropertiesTextEdit>
				<Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
				<EditFormSettings VisibleIndex="0"></EditFormSettings>
				<EditCellStyle CssClass="GridCss">
				</EditCellStyle>
				<EditFormCaptionStyle CssClass="GridCss">
				</EditFormCaptionStyle>
				<HeaderStyle CssClass="GridCssHeader" />
				<CellStyle CssClass="GridCss">
				</CellStyle>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataComboBoxColumn FieldName="ServerType" VisibleIndex="4" 
                UnboundType="String">
				<PropertiesComboBox TextField="ServerType" ValueField="ServerType">
					<ValidationSettings>
						<RequiredField IsRequired="True" />
					</ValidationSettings>
					<ClientSideEvents SelectedIndexChanged="OnSelectedIndexChanged" />
				</PropertiesComboBox>
				<Settings AllowAutoFilter="True" AllowHeaderFilter="False" />
				<EditCellStyle CssClass="GridCss">
				</EditCellStyle>
				<EditFormCaptionStyle CssClass="GridCss">
				</EditFormCaptionStyle>
				<HeaderStyle CssClass="GridCssHeader" />
				<CellStyle CssClass="GridCss">
				</CellStyle>
			</dx:GridViewDataComboBoxColumn>
			<dx:GridViewDataTextColumn FieldName="Description" VisibleIndex="5">
				<PropertiesTextEdit>
					<ValidationSettings>
						<RequiredField IsRequired="True" />
					</ValidationSettings>
				</PropertiesTextEdit>
				<Settings AllowAutoFilter="False" />
				<EditCellStyle CssClass="GridCss">
				</EditCellStyle>
				<EditFormCaptionStyle CssClass="GridCss">
				</EditFormCaptionStyle>
				<HeaderStyle CssClass="GridCssHeader" />
				<CellStyle CssClass="GridCss">
				</CellStyle>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataComboBoxColumn FieldName="Location" VisibleIndex="6">
				<PropertiesComboBox TextField="Location" ValueField="Location">
					<ValidationSettings>
						<RequiredField IsRequired="True" />
					</ValidationSettings>
				</PropertiesComboBox>
				<Settings AllowAutoFilter="True" />
				<EditCellStyle CssClass="GridCss">
				</EditCellStyle>
				<EditFormCaptionStyle CssClass="GridCss">
				</EditFormCaptionStyle>
				<HeaderStyle CssClass="GridCssHeader" />
				<CellStyle CssClass="GridCss">
				</CellStyle>
			</dx:GridViewDataComboBoxColumn>
			<dx:GridViewDataTextColumn FieldName="IPAddress" VisibleIndex="7" Caption=" IP/Host Name">
				<Settings AllowAutoFilter="True" />
				<EditCellStyle CssClass="GridCss">
				</EditCellStyle>
				<EditFormCaptionStyle CssClass="GridCss">
				</EditFormCaptionStyle>
				<HeaderStyle CssClass="GridCssHeader" />
				<CellStyle CssClass="GridCss">
				</CellStyle>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataComboBoxColumn FieldName="ProfileName" VisibleIndex="8">
				<PropertiesComboBox TextField="ProfileName" ValueField="ProfileName">
					<ValidationSettings>
						<RequiredField IsRequired="True" />
					</ValidationSettings>
				</PropertiesComboBox>
				<Settings AllowAutoFilter="True" />
				<EditCellStyle CssClass="GridCss">
				</EditCellStyle>
				<EditFormCaptionStyle CssClass="GridCss">
				</EditFormCaptionStyle>
				<HeaderStyle CssClass="GridCssHeader" />
				<CellStyle CssClass="GridCss">
				</CellStyle>
			</dx:GridViewDataComboBoxColumn>
      <dx:GridViewDataComboBoxColumn FieldName="Type" VisibleIndex="9" 
                Caption="Business Hours">
				<PropertiesComboBox TextField="Type" ValueField="Type">
					<ValidationSettings>
						<RequiredField IsRequired="True" />
					</ValidationSettings>
				</PropertiesComboBox>
				<Settings AllowAutoFilter="True" />
				<EditCellStyle CssClass="GridCss">
				</EditCellStyle>
				<EditFormCaptionStyle CssClass="GridCss">
				</EditFormCaptionStyle>
				<HeaderStyle CssClass="GridCssHeader" />
				<CellStyle CssClass="GridCss">
				</CellStyle>
			</dx:GridViewDataComboBoxColumn>
      <dx:GridViewDataTextColumn FieldName="MonthlyOperatingCost" VisibleIndex="10" 
                Caption="Monthly Operating Cost" Width="60px">
				<Settings AllowAutoFilter="True" />
				  <PropertiesTextEdit>
                <ValidationSettings>
                    <RegularExpression ValidationExpression="^\d*[0-9](|.\d*[0-9]|,\d*[0-9])?$" ErrorText="Numbers Only"/>
                </ValidationSettings>
            </PropertiesTextEdit>
				<EditCellStyle CssClass="GridCss">
				</EditCellStyle>
				<EditFormCaptionStyle CssClass="GridCss">
				</EditFormCaptionStyle>
				<HeaderStyle CssClass="GridCssHeader2" Wrap="True" />
				<CellStyle CssClass="GridCss2">
				</CellStyle>
			</dx:GridViewDataTextColumn>
			 <dx:GridViewDataTextColumn FieldName="IdealUserCount" VisibleIndex="11" 
                Caption="Ideal User Count" Width="60px">
				<Settings AllowAutoFilter="True" />
				  <PropertiesTextEdit>
                <ValidationSettings>
                    <RegularExpression ValidationExpression="(^([0-9]*\d*\d{1}?\d*)$)" ErrorText="Numbers Only"/>
                </ValidationSettings>
            </PropertiesTextEdit>
				<EditCellStyle CssClass="GridCss">
				</EditCellStyle>
				<EditFormCaptionStyle CssClass="GridCss">
				</EditFormCaptionStyle>
				<HeaderStyle CssClass="GridCssHeader2" Wrap="True" />
				<CellStyle CssClass="GridCss2">
				</CellStyle>
			</dx:GridViewDataTextColumn>
		</Columns>
		
		<SettingsBehavior ConfirmDelete="True"></SettingsBehavior>
		<Settings ShowFilterRow="True" />
		<SettingsText ConfirmDelete="Are you sure you want to delete this server?"></SettingsText>
		<Styles>
			<Header SortingImageSpacing="5px" ImageSpacing="5px">
			</Header>
			<LoadingPanel ImageSpacing="5px">
			</LoadingPanel>
			<AlternatingRow CssClass="GridAltRow" Enabled="True">
			</AlternatingRow>
		    <EditForm BackColor="White">
            </EditForm>
		</Styles>
		<StylesPager>
			<PageNumber ForeColor="#3E4846">
			</PageNumber>
			<Summary ForeColor="#1E395B">
			</Summary>
		</StylesPager>
		<StylesEditors ButtonEditCellSpacing="0">
			<ProgressBar Height="21px">
			</ProgressBar>
		</StylesEditors>
		<SettingsPager PageSize="10" SEOFriendly="Enabled">
			<PageSizeItemSettings Visible="true" />
		</SettingsPager>
	</dx:ASPxGridView>
            </td>
        </tr>
    </table>
	<dx:ASPxPopupControl ID="ASPxPopupControl1" runat="server" Text="Are you sure?" ClientInstanceName="popup">
                <ContentCollection>
                    <dx:PopupControlContentControl>
                        <dx:ASPxButton ID="yesButton" runat="server" Text="Yes" AutoPostBack="false">
                           <%-- <ClientSideEvents Click="OnClickYes" />--%>
                        </dx:ASPxButton>
                        <dx:ASPxButton ID="noButton" runat="server" Text="No" AutoPostBack="false">
                           <%-- <ClientSideEvents Click="OnClickNo" />--%>
                        </dx:ASPxButton>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>
<%--	<dx:ASPxPopupControl ID="pcConfirm" runat="server" ClientInstanceName="pcConfirm"
		Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter">
		<ContentCollection>
			<dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server" SupportsDisabledAttribute="True">
				<table width="100%">
					<tr>
						<td colspan="2" align="center">
							Delete Row?
						</td>
					</tr>
					<tr>
						<td align="center">
							<a href="javascript:Yes_Click()">Yes</a>
						</td>
						<td align="center">
							<a href="javascript:No_Click()">No</a>
						</td>
					</tr>
				</table>
			</dx:PopupControlContentControl>
		</ContentCollection>
	</dx:ASPxPopupControl>--%>
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
