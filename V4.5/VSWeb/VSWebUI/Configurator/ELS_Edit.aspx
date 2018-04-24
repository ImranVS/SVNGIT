<%@ Page Title="VitalSigns Plus-Event Log Definitions" Language="C#" MasterPageFile="~/Site1.Master"
	AutoEventWireup="true" CodeBehind="ELS_Edit.aspx.cs" Inherits="VSWebUI.Configurator.ELS_Edit" %>

<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxTreeList.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.Web.ASPxTreeList" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
	<link href='http://fonts.googleapis.com/css?family=Francois One' rel='stylesheet'
		type='text/css' />
	<script src="../js/jquery-1.9.1.js" type="text/javascript"></script>
	<script src="../js/bootstrap.min.js" type="text/javascript"></script>
	<style type="text/css">
		.hide
		{
			display: none;
		}
		.dispError
		{
			color: Red;
		}
		.dispValid
		{
			color: Green;
		}
		.iti-flag
		{
			background-image: url("../images/flags.png");
		}
		.style1
		{
			width: 100%;
		}
		.dxm-horizontal
		{
			float: right !important;
		}
	</style>
	<style type="text/css">
		.style1
		{
			width: 100%;
		}
		.dxtlHeader_Office2010Silver
		{
			border: 1px solid #868b91;
			padding: 5px 6px;
			font-weight: normal;
			color: #3b3b3b;
		}
		.dxtl__B2
		{
			border-top-style: none !important;
			border-right-style: none !important;
			border-left-style: none !important;
		}
		.dxtl__I
		{
			width: 0.1%;
		}
		
		.dxtl__I, .dxtl__IM, .dxtl__I8
		{
			text-align: center;
			font-size: 2px !important;
			line-height: 0 !important;
		}
		.dxtl__B3
		{
			border-top-style: none !important;
			border-right-style: none !important;
		}
		.dxtlNode_Office2010Silver
		{
			background: white none;
		}
		
		.dxtlIndent_Office2010Silver, .dxtlIndentWithButton_Office2010Silver
		{
			background-color: White;
		}
		
		.dxtlLineRoot_Office2010Silver, .dxtlLineFirst_Office2010Silver, .dxtlLineMiddle_Office2010Silver, .dxtlLineLast_Office2010Silver, .dxtlLineFirstRtl_Office2010Silver, .dxtlLineMiddleRtl_Office2010Silver, .dxtlLineLastRtl_Office2010Silver
		{
			background-color: Transparent;
		}
		
		.dxtlIndent_Office2010Silver
		{
			padding: 0 11px;
		}
		.dxtlIndent_Office2010Silver, .dxtlIndentWithButton_Office2010Silver
		{
			vertical-align: top;
			background: white none no-repeat top center;
		}
		.dxtlSelectionCell_Office2010Silver
		{
			border-width: 0;
		}
		
		.dxtl__B0
		{
			border-style: none !important;
		}
		.dxtlAltNode_Office2010Silver
		{
			background: #f7f7f8 none;
		}
		.dxtlAltNode_Office2010Silver .dxtlIndent_Office2010Silver, .dxtlSelectedNode_Office2010Silver .dxtlIndent_Office2010Silver, .dxtlFocusedNode_Office2010Silver .dxtlIndent_Office2010Silver
		{
			background: White none;
		}
		.style2
		{
			height: 2px;
		}
	</style>
	<script type="text/javascript">
		//        window.onload = function () {
		//            if (!window.location.hash) {
		//                alert('load');
		//                window.location = window.location + '#loaded';
		//                window.location.reload();
		//            }
		//        }
		$(document).ready(function () {
			$('.alert-success').delay(10000).fadeOut("slow", function () {
			});
		});
		function OnTypeChanged(cmbType) {
			//alert(cmbType.GetValue().toString());
			//alert(lstBoxDays.visibility);
			grid.PerformCallback(cmbType.GetValue().toString());
		}
		//10/16/2013 NS added
		function OnSelectedIndexChanged(s, e) {

			var visible = false;
			if (s.GetText().toString() == "Specific Hours") {
				visible = true;
			}
			if (visible) {

				clblStartTime.SetVisible(true);
				timeEdtStartTime.SetVisible(true);
				clblDuration.SetVisible(true);
				txtDuration.SetVisible(true);
				//clblMin.SetVisible(true);
				clblDays.SetVisible(true);
				lstBoxDays.SetVisible(true);
				if (timeEdtStartTime.Text == null || timeEdtStartTime.Text == "") {

					timeEdtStartTime.Text = "12:00 AM";
					//                	alert(timeEdtStartTime.Text);
				}
			}
			else {
				clblStartTime.SetVisible(false);
				timeEdtStartTime.SetVisible(false);
				clblDuration.SetVisible(false);
				txtDuration.SetVisible(false);
				//clblMin.SetVisible(false);
				clblDays.SetVisible(false);
				lstBoxDays.SetVisible(false);
			}
		}
		function CollapseTree() {
			//alert('in');
			eventsTree.ExpandAll();
		}
		function FieldsAreEmpty() {
			return txtSendTo.GetText() == '' && phone.GetText() == '';
		}
		function FieldsAreEmptySendTo() {
			return txtSendTo.GetText() == '';
		}
		function FieldsAreEmptySMS() {
			return phone.GetText() == '';
		}
		function FieldsAreEmptyScript() {
			return cmbScript.GetSelectedItem() == null;
		}
		function FieldIsZeroInterval() {
			return parseInt(txtEDuration.GetText()) <= 0;
		}
		function OnSelectedIndexChanged2(s, e) {
			//document.getElementById('ContentPlaceHolder1_HoursGridView_efnew_txtSMSTo_I').style.height = 0;

			//7/1/2015 NS added for VSPLUS-1894
			var smsconfig = hidden_smsconfig.Get('smsconfig');
			document.getElementById("divSMSToMsg").style.display = "none";

			var visible = true;
			if (s.GetText().toString() == "E-mail") {
				lblSendTo.SetVisible(visible);
				txtSendTo.SetVisible(visible);
				lblCopyTo.SetVisible(visible);
				txtCopyTo.SetVisible(visible);
				lblBlindCopyTo.SetVisible(visible);
				txtBlindCopyTo.SetVisible(visible);
				chkboxPersistent.SetVisible(visible);
				lblSMSTo.SetVisible(!visible);
				document.getElementById("divSendto").style.display = "none";
				//7/1/2015 NS added for VSPLUS-1894
				lblScript.SetVisible(!visible);
				cmbScript.SetVisible(!visible);
				cmbScript.SetSelectedIndex(-1);
				chkBxSendSNMPTrap.SetChecked(!visible);
			}
			else if (s.GetText().toString() == "SMS") {
				//document.getElementById('ContentPlaceHolder1_HoursGridView_efnew_txtSMSTo_I').style.height = 10;

				lblSendTo.SetVisible(!visible);
				txtSendTo.SetVisible(!visible);
				txtSendTo.SetText("");
				lblCopyTo.SetVisible(!visible);
				txtCopyTo.SetVisible(!visible);
				txtCopyTo.SetText("");
				lblBlindCopyTo.SetVisible(!visible);
				txtBlindCopyTo.SetVisible(!visible);
				txtBlindCopyTo.SetText("");
				chkboxPersistent.SetVisible(!visible);
				lblSMSTo.SetVisible(visible);
				document.getElementById("divSendto").style.display = "block";
				//7/1/2015 NS added for VSPLUS-1894
				if (!smsconfig) {
					document.getElementById("divSMSToMsg").style.display = "block";
				}
				lblScript.SetVisible(!visible);
				cmbScript.SetVisible(!visible);
				cmbScript.SetSelectedIndex(-1);
				chkBxSendSNMPTrap.SetChecked(!visible);
			}
			else if (s.GetText().toString() == "Script") {
				lblSendTo.SetVisible(!visible);
				txtSendTo.SetVisible(!visible);
				txtSendTo.SetText("");
				lblCopyTo.SetVisible(!visible);
				txtCopyTo.SetVisible(!visible);
				txtCopyTo.SetText("");
				lblBlindCopyTo.SetVisible(!visible);
				txtBlindCopyTo.SetVisible(!visible);
				txtBlindCopyTo.SetText("");
				chkboxPersistent.SetVisible(!visible);
				lblSMSTo.SetVisible(!visible);
				document.getElementById("divSendto").style.display = "none";
				lblScript.SetVisible(visible);
				cmbScript.SetVisible(visible);
				chkBxSendSNMPTrap.SetChecked(!visible);
			}
			else if (s.GetText().toString() == "Windows Log") {
				lblSendTo.SetVisible(!visible);
				txtSendTo.SetVisible(!visible);
				txtSendTo.SetText("");
				lblCopyTo.SetVisible(!visible);
				txtCopyTo.SetVisible(!visible);
				txtCopyTo.SetText("");
				lblBlindCopyTo.SetVisible(!visible);
				txtBlindCopyTo.SetVisible(!visible);
				txtBlindCopyTo.SetText("");
				chkboxPersistent.SetVisible(!visible);
				lblSMSTo.SetVisible(!visible);
				document.getElementById("divSendto").style.display = "none";
				lblScript.SetVisible(!visible);
				cmbScript.SetVisible(!visible);
				chkBxSendSNMPTrap.SetChecked(!visible);
			}
			else if (s.GetText().toString() == "SNMP Trap") {
				lblSendTo.SetVisible(!visible);
				txtSendTo.SetVisible(!visible);
				txtSendTo.SetText("");
				lblCopyTo.SetVisible(!visible);
				txtCopyTo.SetVisible(!visible);
				txtCopyTo.SetText("");
				lblBlindCopyTo.SetVisible(!visible);
				txtBlindCopyTo.SetVisible(!visible);
				txtBlindCopyTo.SetText("");
				chkboxPersistent.SetVisible(!visible);
				lblSMSTo.SetVisible(!visible);
				document.getElementById("divSendto").style.display = "none";
				lblScript.SetVisible(!visible);
				cmbScript.SetVisible(!visible);
				chkBxSendSNMPTrap.SetChecked(visible);
			}
		}

		//4/2/2015 NS added for VSPLUS-219
		function FieldsAreEmptyEscalateTo() {
			return txtESendTo.GetText() == '' && txtESMSTo.GetText() == '';
		}
		function FieldsAreEmptyESendTo() {
			return txtESendTo.GetText() == '';
		}
		function FieldsAreEmptyESMS() {
			return txtESMSTo.GetText() == '';
		}
		function FieldsAreEmptyEScript() {
			return cmbEScript.GetSelectedItem() == null;
		}
		function OnSelectedIndexChangedEscalate(s, e) {
			//7/1/2015 NS added for VSPLUS-1894
			var smsconfig = ehidden_smsconfig.Get('smsconfig');
			document.getElementById("divESMSToMsg").style.display = "none";
			var visible = false;
			if (s.GetText().toString() == "E-mail") {
				visible = true;
				lblESendTo.SetVisible(visible);
				txtESendTo.SetVisible(visible);
				lblESMSTo.SetVisible(!visible);
				//txtESMSTo.SetVisible(!visible);
				//txtESMSTo.SetText("");
				lblEScript.SetVisible(!visible);
				cmbEScript.SetVisible(!visible);
				cmbEScript.SetSelectedIndex(-1);
				document.getElementById("divESendto").style.display = "none";
			}
			else if (s.GetText().toString() == "SMS") {
				visible = true;
				lblESendTo.SetVisible(!visible);
				txtESendTo.SetVisible(!visible);
				txtESendTo.SetText("");
				lblESMSTo.SetVisible(visible);
				//txtESMSTo.SetVisible(visible);
				lblEScript.SetVisible(!visible);
				cmbEScript.SetVisible(!visible);
				cmbEScript.SetSelectedIndex(-1);
				document.getElementById("divESendto").style.display = "block";
				//7/1/2015 NS added for VSPLUS-1894
				if (!smsconfig) {
					document.getElementById("divESMSToMsg").style.display = "block";
				}
			}
			else if (s.GetText().toString() == "Script") {
				visible = true;
				lblESendTo.SetVisible(!visible);
				txtESendTo.SetVisible(!visible);
				txtESendTo.SetText("");
				lblESMSTo.SetVisible(!visible);
				//txtESMSTo.SetVisible(!visible);
				//txtESMSTo.SetText("");
				lblEScript.SetVisible(visible);
				cmbEScript.SetVisible(visible);
				document.getElementById("divESendto").style.display = "none";
			}
		}

	</script>
	<script type="text/javascript">
		//    	function OnTextBoxKeyPress(s, e) {
		//    		//alert("hi");
		//    		if (e.htmlEvent.keyCode == 13) {
		//    			ASPxClientUtils.PreventEventAndBubble(e.htmlEvent);
		//    		}
		//    	}
		var visibleIndex;

		function OnCustomButtonClick(s, e) {
			visibleIndex = e.visibleIndex;

			if (e.buttonID == "deleteButton")
				ELSGridView.GetRowValues(e.visibleIndex, 'AliasName', OnGetRowValues);

			function OnGetRowValues(values) {
				var id = values[0];
				var name = values[1];
				var OK = (confirm('Are sure you want to delete these credentials - ' + values + '?'))
				if (OK == true) {
					//					alert('Before Delete');
					//					var name1 = ServersGridView.DeleteRow(visibleIndex);
					ELSGridView.DeleteRow(visibleIndex);
					//alert('The Server ' + values + ' was Successfully Deleted');
					//					alert(name1);
					//ScriptManager.RegisterClientScriptBlock(base.Page, this.GetType(), "FooterRequired", "alert('Notification : Record deleted successfully');", true);
				}

				else {
				}

			}
		}

    	
					
        </script>
    <style type="text/css">
.dxeTextBoxSys, .dxeMemoSys
{
    border-collapse:separate!important;
}

        .style1
        {
            width: 100%;
            font-size: 0;
        }
        
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<table>
	<tr>
        <td>
            <div class="header" id="servernamelbldisp" runat="server">Windows Event Log Scanning</div>
            <div id="successDiv" class="alert alert-success" runat="server" style="display: none">Success.
                </div>
            <asp:Label ID="lblmessage" runat="server" Style="font-weight: bold; color: #f00;"> </asp:Label>
            <div id="errorDiv" class="alert alert-danger" runat="server" style="display: none">Error.
            </div>
        </td>
    </tr>
	<tr>
			<td colspan="2">
				<table>
					<tr>
						<td>
							<dx:ASPxLabel ID="ASPxLabel1" runat="server" CssClass="lblsmallFont" Text="Event Definition:">
							</dx:ASPxLabel>
						</td>
						<td>
							<dx:ASPxTextBox ID="EventNameTextBox" runat="server" Width="170px">
								<ValidationSettings ErrorDisplayMode="ImageWithTooltip" SetFocusOnError="True">
									<RequiredField ErrorText="Enter Alert Name" IsRequired="True" />
									<RequiredField IsRequired="True" ErrorText="Enter Alert Name"></RequiredField>
								</ValidationSettings>
							</dx:ASPxTextBox>
						</td>
					</tr>
				</table>
			</td>
	</tr>

    <tr>
                <td>
                    <dx:ASPxButton ID="NewButton" runat="server" Text="New" CssClass="sysButton" AutoPostBack="False">
                        <ClientSideEvents Click="function() { ELSGridView.AddNewRow(); }" />
                                <Image Url="~/images/icons/add.png">
                                </Image>
                    </dx:ASPxButton>
                </td>
            </tr>
	<tr>
			<td>
            <dx:ASPxGridView runat="server" 
                                KeyFieldName="ID" AutoGenerateColumns="False" 
                                ID="ELSGridView" 
				 ClientInstanceName="ELSGridView"
                                OnRowInserting="ELSGridView_RowInserting" 
                               OnRowUpdating="ELSGridView_RowUpdating"
							   OnRowDeleting="ELSGridView_RowDeleting"
							   oncelleditorinitialize="ELSGridView_CellEditorInitialize" 
        Theme="Office2003Blue" EnableTheming="True" 
		
				CssFilePath="~/App_Themes/Office2010Blue/{0}/styles.css" 
				CssPostfix="Office2010Blue"  >
		<ClientSideEvents CustomButtonClick="OnCustomButtonClick" />
        <Columns>
            <dx:GridViewCommandColumn ButtonType="Image" Caption="Actions" ShowSelectCheckbox="True"  SelectAllCheckboxMode="Page"
                                        FixedStyle="Left" VisibleIndex="0" Width="100px">
                <EditButton Visible="True">
                    <Image Url="../images/edit.png">
                    </Image>
                </EditButton>
                
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
                    <Image Url="~/images/clear.png">
                    </Image>
                </ClearFilterButton>
                <HeaderStyle CssClass="GridCssHeader1" />
            </dx:GridViewCommandColumn>
            <%--<dx:GridViewDataTextColumn Caption="Delete" VisibleIndex="1" CellStyle-HorizontalAlign="Center" Width="30px" CellStyle-VerticalAlign="Middle">
                <DataItemTemplate>
                    <asp:Label ID="lblLocation" runat="server" Text='<%#Eval("AliasName") %>' Visible="False"></asp:Label>
                    <asp:ImageButton ID="deleteButton" runat="server" ImageUrl="../images/delete.png" Width="15px" Height="15px" 
                    CommandName="Delete" CommandArgument='<%#Eval("ID") %>' AlternateText='<%#Eval("AliasName") %>' ToolTip="Delete" 
                    OnClick="bttnDelete_Click"/>
                </DataItemTemplate>
				<HeaderStyle CssClass="GridCssHeader" />
                <EditFormSettings Visible="False"/>
<EditFormSettings Visible="False"></EditFormSettings>

                <EditCellStyle CssClass="GridCss">
                </EditCellStyle>
                <EditFormCaptionStyle CssClass="GridCss">
                </EditFormCaptionStyle>
                <HeaderStyle CssClass="GridCssHeader" />
                <CellStyle CssClass="GridCss1">
                </CellStyle>
                <CellStyle HorizontalAlign="Center">
                </CellStyle>
            </dx:GridViewDataTextColumn>--%>
			 <dx:GridViewCommandColumn   Caption="Delete" ButtonType="Image" 
				VisibleIndex="1">
                        <CustomButtons>
                            <dx:GridViewCommandColumnCustomButton ID="deleteButton" 
								Image-Url="../images/delete.png" Text="Delete" >
<Image Url="../images/delete.png"></Image>
							</dx:GridViewCommandColumnCustomButton>
                        </CustomButtons>
						<HeaderStyle CssClass="GridCssHeader1" />
                        <CellStyle CssClass="GridCss1">
                        </CellStyle>
                    </dx:GridViewCommandColumn>

            <dx:GridViewDataTextColumn FieldName="ID"  Visible="False" 
                                    VisibleIndex="2">
                <EditFormSettings Visible="False">
                </EditFormSettings>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="EventId" VisibleIndex="3" Caption="Event ID">
               <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />

<Settings AutoFilterCondition="Contains"></Settings>

                <EditCellStyle CssClass="GridCss">
                </EditCellStyle>
                <EditFormCaptionStyle CssClass="GridCss">
                </EditFormCaptionStyle>
                <HeaderStyle CssClass="GridCssHeader" />
                <CellStyle CssClass="GridCss">
                </CellStyle>
            </dx:GridViewDataTextColumn>
			 <dx:GridViewDataTextColumn FieldName="AliasName" VisibleIndex="1" Caption="Alias Name">
                <PropertiesTextEdit>
                    <ValidationSettings CausesValidation="True" SetFocusOnError="True">
                        <RequiredField ErrorText="Enter Alias" IsRequired="True" />
                    </ValidationSettings>
                </PropertiesTextEdit>
               <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />

<Settings AutoFilterCondition="Contains"></Settings>

                <EditCellStyle CssClass="GridCss">
                </EditCellStyle>
                <EditFormCaptionStyle CssClass="GridCss">
                </EditFormCaptionStyle>
                <HeaderStyle CssClass="GridCssHeader" />
                <CellStyle CssClass="GridCss">
                </CellStyle>
            </dx:GridViewDataTextColumn>

			 <%--<dx:GridViewDataTextColumn FieldName="EventName" VisibleIndex="3" Caption="Event Name">
                <PropertiesTextEdit>
                    <ValidationSettings CausesValidation="True" SetFocusOnError="True">
                        <RequiredField ErrorText="Enter Alias" IsRequired="True" />
                    </ValidationSettings>
                </PropertiesTextEdit>
               <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />

<Settings AutoFilterCondition="Contains"></Settings>

                <EditCellStyle CssClass="GridCss">
                </EditCellStyle>
                <EditFormCaptionStyle CssClass="GridCss">
                </EditFormCaptionStyle>
                <HeaderStyle CssClass="GridCssHeader" />
                <CellStyle CssClass="GridCss">
                </CellStyle>
            </dx:GridViewDataTextColumn>--%>
			 <dx:GridViewDataComboBoxColumn FieldName="EventName" VisibleIndex="5" 
                UnboundType="String">
                <PropertiesComboBox  TextField="EventName" 
                    ValueField="EventName">
                    <ValidationSettings>
                        <RequiredField IsRequired="True" />
                    </ValidationSettings>
                    
                </PropertiesComboBox>
                <Settings AllowAutoFilter="True" AllowHeaderFilter="False" />
<EditCellStyle CssClass="GridCss"></EditCellStyle><EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle><HeaderStyle CssClass="GridCssHeader" /><CellStyle CssClass="GridCss"></CellStyle>
            </dx:GridViewDataComboBoxColumn>
            <dx:GridViewDataTextColumn Caption="Event Key" FieldName="EventKey" 
                VisibleIndex="2">
                
                <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />

<Settings AutoFilterCondition="Contains"></Settings>

                <EditCellStyle CssClass="GridCss">
                </EditCellStyle>
                <EditFormCaptionStyle CssClass="GridCss">
                </EditFormCaptionStyle>
                <HeaderStyle CssClass="GridCssHeader" />
                <CellStyle CssClass="GridCss">
                </CellStyle>
            </dx:GridViewDataTextColumn>

			<dx:GridViewDataComboBoxColumn FieldName="EventLevel" VisibleIndex="5" 
                UnboundType="String">
                
                <Settings AllowAutoFilter="True" AllowHeaderFilter="False" />
<EditCellStyle CssClass="GridCss"></EditCellStyle><EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle><HeaderStyle CssClass="GridCssHeader" /><CellStyle CssClass="GridCss"></CellStyle>
            </dx:GridViewDataComboBoxColumn>

			 <dx:GridViewDataTextColumn FieldName="Source" VisibleIndex="4">
				 
				<Settings AutoFilterCondition="Contains"></Settings>
				<EditCellStyle CssClass="GridCss">
                </EditCellStyle>
			
                <EditFormCaptionStyle CssClass="GridCss">
                </EditFormCaptionStyle>
			  	<HeaderStyle CssClass="GridCssHeader" />
                <CellStyle CssClass="GridCss">
                </CellStyle>
            </dx:GridViewDataTextColumn>

			 <dx:GridViewDataTextColumn Caption="Task Category" FieldName="TaskCategory" 
                VisibleIndex="5">
               
                <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />

<Settings AutoFilterCondition="Contains"></Settings>

                <EditCellStyle CssClass="GridCss">
                </EditCellStyle>
                <EditFormCaptionStyle CssClass="GridCss">
                </EditFormCaptionStyle>
                <HeaderStyle CssClass="GridCssHeader" />
                <CellStyle CssClass="GridCss">
                </CellStyle>
            </dx:GridViewDataTextColumn>
			       

        </Columns>
        <SettingsBehavior ConfirmDelete="True">
        </SettingsBehavior>
        <Settings ShowFilterRow="True" />
        <SettingsText ConfirmDelete="Are you sure you want to delete this Event?">
        </SettingsText>
        <Styles CssPostfix="Office2010Blue" CssFilePath="~/App_Themes/Office2010Blue/{0}/styles.css">
            <Header SortingImageSpacing="5px" ImageSpacing="5px">
            </Header>
            <LoadingPanel ImageSpacing="5px">
            </LoadingPanel>
            <AlternatingRow CssClass="GridAltRow" Enabled="True">
            </AlternatingRow>
            <EditForm CssClass="GridCssEditForm">
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
        <SettingsPager PageSize="10" SEOFriendly="Enabled" >
            <PageSizeItemSettings Visible="true" />
<PageSizeItemSettings Visible="True"></PageSizeItemSettings>
        </SettingsPager>
    </dx:ASPxGridView>
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
														<td valign="top" style="height: 30px">
															<dx:ASPxButton ID="CollapseAllSrvButton" runat="server" Text="Collapse All" CssClass="sysButton"
																Wrap="False" OnClick="CollapseAllSrvButton_Click">
																<Image Url="~/images/icons/forbidden.png">
																</Image>
															</dx:ASPxButton>
														</td>
													</tr> 
		<tr>
			<td>
                                       
				<dx:ASPxTreeList ID="ServersTreeList" runat="server" AutoGenerateColumns="False"
																CssClass="lblsmallFont" KeyFieldName="Id" OnPageSizeChanged="ServersTreeList_PageSizeChanged"
																ParentFieldName="LocId" Theme="Office2003Blue" Width="100%">
																<Columns>
																	<dx:TreeListTextColumn Caption="Servers  " FieldName="Name" Name="Servers" ShowInCustomizationForm="True"
																		VisibleIndex="0">
																		<HeaderStyle CssClass="lblsmallFont" />
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
																		<HeaderStyle CssClass="lblsmallFont" />
																	</dx:TreeListTextColumn>
																	<dx:TreeListTextColumn FieldName="Description" ShowInCustomizationForm="True" VisibleIndex="5">
																		<HeaderStyle CssClass="lblsmallFont" />
																	</dx:TreeListTextColumn>
																	<dx:TreeListTextColumn FieldName="srvtypeid" Name="srvtypeid" ShowInCustomizationForm="True"
																		Visible="False" VisibleIndex="6">
																	</dx:TreeListTextColumn>
																</Columns>
																<Settings GridLines="Both" SuppressOuterGridLines="True" />
																<SettingsBehavior AllowDragDrop="False" AutoExpandAllNodes="False" />
																<Settings GridLines="Both" SuppressOuterGridLines="True" />
																<SettingsBehavior AllowDragDrop="False" AutoExpandAllNodes="False" />
																<SettingsPager AlwaysShowPager="True" Mode="ShowPager" PageSize="20">
																	<PageSizeItemSettings Visible="True">
																	</PageSizeItemSettings>
																</SettingsPager>
																<SettingsSelection AllowSelectAll="True" Enabled="True" Recursive="True" />
																<SettingsSelection AllowSelectAll="True" Enabled="True" Recursive="True" />
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
	</td>
	</tr>
	<tr>
			<td colspan="2">
				
						<div id="Div1" class="alert alert-success" runat="server" style="display: none">
							Success.
						</div>
						<div id="Div2" class="alert alert-danger" runat="server" style="display: none">
							Error.
						</div>
						<table>
							<tr>
								<td>
									<dx:ASPxButton ID="OKButton" runat="server" OnClick="OKButton_Click" Text="OK" CssClass="sysButton">
									</dx:ASPxButton>
								</td>
								<td>
									<dx:ASPxButton ID="CancelButton" runat="server" CausesValidation="False" OnClick="CancelButton_Click"
										Text="Cancel" CssClass="sysButton">
									</dx:ASPxButton>
								</td>
							</tr>
						</table>
					
			</td>
		</tr>
</table>
</asp:Content>
