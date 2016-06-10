<%@ Page Title="VitalSigns Plus-Alert Definitions" Language="C#" MasterPageFile="~/Site1.Master"
	AutoEventWireup="true" CodeBehind="AlertDef_Edit.aspx.cs" Inherits="VSWebUI.Configurator.AlertDef_Edit" %>

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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
	<table width="100%">
		<tr>
			<td>
				<div class="header" id="servernamelbldisp" runat="server">
					Alert Definition</div>
				<table class="style1">
					<tr>
						<td colspan="2">
							<div id="infoDiv" class="info">
								You need to select at least one Event and one Server to receive alerts. Please refer
								to the Alert Settings page to change the &#39;Alert on Event Recurrence&#39; column
								in the Events grid.
							</div>
                            <dx:ASPxLoadingPanel ID="ASPxLoadingPanel1" runat="server" 
                                ClientInstanceName="LoadingPanel" Theme="Moderno"
                             Modal="true" ContainerElementID="Content2" 
                                Text="Update in progress. Please wait&amp;hellip;" VerticalAlign="Bottom">
                            </dx:ASPxLoadingPanel>
						</td>
					</tr>
					<tr>
						<td colspan="2">
							<table>
								<tr>
									<td>
										<dx:ASPxLabel ID="ASPxLabel1" runat="server" CssClass="lblsmallFont" Text="Alert Name:">
										</dx:ASPxLabel>
									</td>
									<td>
										<dx:ASPxTextBox ID="AlertNameTextBox" runat="server" Width="170px">
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
						<td colspan="2">
							<div class="subheader" id="Div1" runat="server">
								Hours & Destinations</div>
							<asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
								<ContentTemplate>
									<script type='text/javascript' src='//ajax.googleapis.com/ajax/libs/jquery/1.9.1/jquery.min.js'></script>
									<script type='text/javascript' src='../js/intlTelInput.min.js'></script>
									<link rel="stylesheet" href="../css/intlTelInput.css">
									<dx:ASPxGridView ID="HoursGridView" runat="server" AutoGenerateColumns="False" ClientInstanceName="grid"
										EnableTheming="True" KeyFieldName="ID" OnCellEditorInitialize="HoursGridView_CellEditorInitialize"
										OnCustomCallback="HoursGridView_CustomCallback" OnHtmlRowCreated="HoursGridView_HtmlRowCreated"
										OnPageSizeChanged="HoursGridView_PageSizeChanged" OnRowDeleting="HoursGridView_RowDeleting"
										OnRowInserting="HoursGridView_RowInserting" OnRowUpdating="HoursGridView_RowUpdating"
										Theme="Office2003Blue" Width="100%" OnCustomColumnDisplayText="HoursGridView_CustomColumnDisplayText"
										OnCustomUnboundColumnData="HoursGridView_CustomUnboundColumnData" 
										OnCustomJSProperties="HoursGridView_CustomJSProperties" 
										oncustomerrortext="HoursGridView_CustomErrorText">
										<ClientSideEvents EndCallback="function(s, e) {
	                                                                    if(s.cpIsEdit)
                                                                          {

                                                                                var telInput = $('#phone'),
                                                                                errorMsg = $('#error-msg'),
                                                                                validMsg = $('#valid-msg');                                                                        
                                                                                telInput.intlTelInput({  utilsScript: '../js/utils.js'    });   
                                                                                            
                                                                          if(telInput.val()!='')
                                                                          { 
                                                                              document.getElementById('divSendto').style.value = 'display: block';
                                                                              document.getElementById('divSendto').style.height=0;
                                                                              
                                                                          }
                                                                               
                                                                                //var mJSVariable =telInput.intlTelInput('getNumber');                                                                              
                                                                                var mJSVariable = s.cpSMSTxtTo;
                                                                                var smsconfig = s.cpSMSConfig;
                                                                                var hiddenPhone = $('#hidden_phone'); 
                                                                                if (mJSVariable == ''){                                                               
                                                                                    telInput.intlTelInput('setNumber', '');
                                                                                    hiddenPhone.val(''); 
                                                                                }
                                                                                else{
                                                                                    telInput.intlTelInput('setNumber', '+' + mJSVariable);
                                                                                    hiddenPhone.val('+' +  mJSVariable); 
                                                                                }
                                                                                var invalid = 1;
                                                                                telInput.blur(function () {
                                                                    if ($.trim(telInput.val())) {
                                                                        if (telInput.intlTelInput('isValidNumber')) {
                                                                            validMsg.removeClass('hide');
                                                                            validMsg.addClass('dispValid');
                                                                            errorMsg.addClass('hide');
                                                                            invalid = 0;
                                                                        } else {
                                                                            errorMsg.removeClass('hide');
                                                                            errorMsg.addClass('dispError');
                                                                            validMsg.addClass('hide');
                                                                        }
                                                                    }
                                                                    var num = telInput.intlTelInput('getNumber');
                                                                    if (invalid == 1){
                                                                        document.getElementById('hidden_phone').value = '';
                                                                        hidden_phone1.Set('smsto', 'invalid');
                                                                    }
                                                                    else {
                                                                        document.getElementById('hidden_phone').value = num;
                                                                        hidden_phone1.Set('smsto', num);
                                                                    }
                                                                    hidden_smsconfig.Set('smsconfig',smsconfig);
                                                                });

                                                                
                                                                telInput.keydown(function () {
                                                                    errorMsg.addClass('hide');
                                                                    validMsg.addClass('hide');
                                                                });                                                                        
                                                                          }
                                                                        else
                                                                        {
                                                                           
                                                                        }
                                                                        var num = telInput.intlTelInput('getNumber');

                                                                    document.getElementById('hidden_phone').value = num;
                                                                    hidden_phone1.Set('smsto', num);
                                                                    hidden_smsconfig.Set('smsconfig',smsconfig);
                                                                    }" />
										<Columns>
											<dx:GridViewCommandColumn ButtonType="Image" Caption="Actions" FixedStyle="Left"
												VisibleIndex="0" Width="70px">
												<EditButton Visible="True">
													<Image Url="../images/edit.png">
													</Image>
												</EditButton>
												<NewButton Visible="True">
													<Image Url="../images/icons/add.png">
													</Image>
												</NewButton>
												<DeleteButton Visible="True">
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
												<HeaderStyle CssClass="GridCssHeader1" Wrap="False" />
												<CellStyle CssClass="GridCss1">
													<Paddings Padding="3px" />
												</CellStyle>
											</dx:GridViewCommandColumn>
											<dx:GridViewDataTextColumn Caption="Start Time" FieldName="StartTime" VisibleIndex="2"
												Width="70px">
												<PropertiesTextEdit>
													<MaskSettings Mask="hh:mm tt" />
												</PropertiesTextEdit>
												<EditFormSettings Visible="True" />
												<EditCellStyle CssClass="GridCss">
												</EditCellStyle>
												<EditFormCaptionStyle CssClass="GridCss">
												</EditFormCaptionStyle>
												<HeaderStyle CssClass="GridCssHeader" Wrap="True" />
												<CellStyle CssClass="GridCss">
												</CellStyle>
											</dx:GridViewDataTextColumn>
											<dx:GridViewDataTextColumn Caption="Duration (min)" FieldName="Duration" VisibleIndex="3"
												Width="60px">
												<PropertiesTextEdit>
													<MaskSettings Mask="hh:mm tt" ShowHints="True" />
												</PropertiesTextEdit>
												<EditFormSettings Visible="True" />
												<EditCellStyle CssClass="GridCss">
												</EditCellStyle>
												<EditFormCaptionStyle CssClass="GridCss">
												</EditFormCaptionStyle>
												<HeaderStyle CssClass="GridCssHeader2" Wrap="True" />
												<CellStyle CssClass="GridCss2">
												</CellStyle>
											</dx:GridViewDataTextColumn>
											<dx:GridViewDataTextColumn Caption="Send To" FieldName="SendTo" VisibleIndex="5"
												Visible="False">
												<PropertiesTextEdit>
													<ValidationSettings>
														<RegularExpression ErrorText="Enter Valid Email ID" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" />
													</ValidationSettings>
												</PropertiesTextEdit>
												<EditFormSettings Visible="True" />
												<EditCellStyle CssClass="GridCss">
												</EditCellStyle>
												<EditFormCaptionStyle CssClass="GridCss">
												</EditFormCaptionStyle>
												<HeaderStyle CssClass="GridCssHeader" Wrap="True" />
												<CellStyle CssClass="GridCss">
												</CellStyle>
											</dx:GridViewDataTextColumn>
											<dx:GridViewDataTextColumn Caption="Copy To" FieldName="CopyTo" VisibleIndex="6">
												<PropertiesTextEdit>
													<ValidationSettings>
														<RegularExpression ErrorText="Enter Valid Email ID" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" />
													</ValidationSettings>
												</PropertiesTextEdit>
												<EditFormSettings Visible="True" />
												<EditCellStyle CssClass="GridCss">
												</EditCellStyle>
												<EditFormCaptionStyle CssClass="GridCss">
												</EditFormCaptionStyle>
												<HeaderStyle CssClass="GridCssHeader" Wrap="True" />
												<CellStyle CssClass="GridCss">
												</CellStyle>
											</dx:GridViewDataTextColumn>
											<dx:GridViewDataTextColumn Caption="Blind Copy To" FieldName="BlindCopyTo" VisibleIndex="7">
												<EditFormSettings Visible="True" />
												<EditCellStyle CssClass="GridCss">
												</EditCellStyle>
												<EditFormCaptionStyle CssClass="GridCss">
												</EditFormCaptionStyle>
												<HeaderStyle CssClass="GridCssHeader" Wrap="True" />
												<CellStyle CssClass="GridCss">
												</CellStyle>
											</dx:GridViewDataTextColumn>
											<dx:GridViewDataComboBoxColumn Caption="Type" FieldName="HoursIndicator" VisibleIndex="1"
												Width="70px">
												<EditFormSettings Visible="True" />
												<EditCellStyle CssClass="GridCss">
												</EditCellStyle>
												<EditFormCaptionStyle CssClass="GridCss">
												</EditFormCaptionStyle>
												<HeaderStyle CssClass="GridCssHeader" Wrap="True" />
												<CellStyle CssClass="GridCss">
												</CellStyle>
											</dx:GridViewDataComboBoxColumn>
											<dx:GridViewDataTextColumn Caption="Day(s)" FieldName="Day" VisibleIndex="10" Width="120px">
												<PropertiesTextEdit>
													<ValidationSettings>
														<RegularExpression ErrorText="Enter Valid Email ID" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" />
													</ValidationSettings>
												</PropertiesTextEdit>
												<EditFormSettings Visible="True" />
												<EditCellStyle CssClass="GridCss">
												</EditCellStyle>
												<EditFormCaptionStyle CssClass="GridCss">
												</EditFormCaptionStyle>
												<HeaderStyle CssClass="GridCssHeader" Wrap="True" />
												<CellStyle CssClass="GridCss">
												</CellStyle>
											</dx:GridViewDataTextColumn>
											<dx:GridViewDataCheckColumn Caption="Send SNMP Trap" FieldName="SendSNMPTrap" VisibleIndex="11"
												Width="70px">
												<Settings AllowAutoFilter="False" />
												<EditFormSettings Visible="True" />
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
											<dx:GridViewDataCheckColumn Caption="Persistent Alerting" FieldName="EnablePersistentAlert"
												VisibleIndex="12" Width="70px">
												<HeaderStyle CssClass="GridCssHeader1" Wrap="True" />
											</dx:GridViewDataCheckColumn>
											<dx:GridViewDataTextColumn Caption="SMS To" FieldName="SMSTo" VisibleIndex="8" Visible="False">
												<PropertiesTextEdit>
													<ValidationSettings ErrorDisplayMode="ImageWithText" />
												</PropertiesTextEdit>
												<EditFormSettings Visible="True" />
												<HeaderStyle CssClass="GridCssHeader" />
												<CellStyle CssClass="GridCss">
												</CellStyle>
											</dx:GridViewDataTextColumn>
											<dx:GridViewDataTextColumn Caption="Script" FieldName="ScriptName" VisibleIndex="9"
												Visible="False">
												<HeaderStyle CssClass="GridCssHeader" />
												<CellStyle CssClass="GridCss">
												</CellStyle>
											</dx:GridViewDataTextColumn>
											<dx:GridViewDataTextColumn Caption="ScriptID" FieldName="ScriptID" Visible="False"
												VisibleIndex="13">
											</dx:GridViewDataTextColumn>
											<dx:GridViewDataTextColumn Caption="Send To" FieldName="SendToAll" UnboundType="String"
												VisibleIndex="4" Width="200px">
												<HeaderStyle CssClass="GridCssHeader" />
												<CellStyle CssClass="GridCss">
												</CellStyle>
											</dx:GridViewDataTextColumn>
										</Columns>
										<SettingsBehavior ColumnResizeMode="NextColumn" ConfirmDelete="True" />
										<SettingsText ConfirmDelete="Are you sure you want to delete this record?" />
										<Styles GroupButtonWidth="20">
											<AlternatingRow CssClass="GridAltRow" Enabled="True">
											</AlternatingRow>
											<Header ImageSpacing="5px" SortingImageSpacing="5px">
											</Header>
											<GroupRow Font-Bold="True">
											</GroupRow>
											<LoadingPanel ImageSpacing="5px">
											</LoadingPanel>
											<EditForm BackColor="White">
											</EditForm>
										</Styles>
										<StylesEditors ButtonEditCellSpacing="0">
											<ProgressBar Height="21px">
											</ProgressBar>
										</StylesEditors>
										<Templates>
											<EditForm>
												<table>
													<tr>
														<td colspan="3">
															<div id="divSMSToMsg" runat="server" class="alert alert-danger" clientidmode="Static"
																style="display: none">
																The SMS settings are incomplete or not configured on the Alert Settings page.
																<br />
																You will not be able to send SMS until the settings are properly configured.
															</div>
															<dx:ASPxHiddenField ID="hidden_smsconfig" runat="server" ClientInstanceName="hidden_smsconfig">
															</dx:ASPxHiddenField>
														</td>
													</tr>
													<tr>
														<td valign="top">
															<table>
																<tr>
																	<td>
																		<dx:ASPxLabel ID="lblType" runat="server" CssClass="lblsmallFont" Text="Hours: ">
																		</dx:ASPxLabel>
																	</td>
																	<td>
																		<dx:ASPxComboBox ID="cmbType" runat="server" AutoPostBack="False" ClientInstanceName="cmbType"
																			ValueType="System.String">
																			<ClientSideEvents SelectedIndexChanged="function(s,e){OnSelectedIndexChanged(s,e)}" />
																			<ValidationSettings CausesValidation="True" ErrorDisplayMode="ImageWithTooltip" ErrorText="Hours field is required.">
																				<RequiredField ErrorText="Hours field is required." IsRequired="True" />
																			</ValidationSettings>
																			<%--<ClientSideEvents Init="function(s,e) { s.Validate(); }" />         --%>
																		</dx:ASPxComboBox>
																	</td>
																</tr>
																<tr>
																	<td>
																		<dx:ASPxLabel ID="lblStartTime" runat="server" ClientInstanceName="clblStartTime"
																			CssClass="lblsmallFont" Text="Start Time: " ClientVisible="false">
																		</dx:ASPxLabel>
																	</td>
																	<td>
																		<dx:ASPxTimeEdit ID="timeEdtStartTime" runat="server" ClientInstanceName="timeEdtStartTime"
																			Spacing="0" ClientVisible="false">
																			<ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorText="Start Time is required">
																				<RequiredField ErrorText="" IsRequired="true" />
																			</ValidationSettings>
																		</dx:ASPxTimeEdit>
																	</td>
																</tr>
																<tr>
																	<td>
																		<dx:ASPxLabel ID="lblDuration" runat="server" ClientInstanceName="clblDuration" CssClass="lblsmallFont"
																			Text="Duration (minutes): " ClientVisible="false">
																		</dx:ASPxLabel>
																	</td>
																	<td>
																		<dx:ASPxTextBox ID="txtDuration" runat="server" ClientInstanceName="txtDuration"
																			ValidationSettings-ValidationGroup="<%# Container.ValidationGroup %>" Width="60px"
																			ClientVisible="false">
																			<%--<MaskSettings Mask="&lt;0..9999&gt;" />--%>
																			<ValidationSettings ErrorText="enter duratiion>0">
																				<RequiredField IsRequired="true" ErrorText="Please Enter duratiion>0" />
																				<RegularExpression ValidationExpression="^[1-9][0-9]*(\.[0-9]+)?|0+\.[0-9]*[1-9][0-9]*$"
																					ErrorText="Please Enter Duration>0" />
																			</ValidationSettings>
																		</dx:ASPxTextBox>
																	</td>
																</tr>
																<tr>
																	<td>
																		<dx:ASPxLabel ID="lblMechanism" runat="server" Text="Alerting Mechanism:" CssClass="lblsmallFont">
																		</dx:ASPxLabel>
																	</td>
																	<td>
																		<dx:ASPxComboBox ID="cmbMechanism" runat="server" ValueType="System.String" ClientInstanceName="cmbMechanism">
																			<Items>
																				<dx:ListEditItem Text="E-mail" Value="0" Selected="true" />
																				<dx:ListEditItem Text="SMS" Value="1" />
																				<dx:ListEditItem Text="Script" Value="2" />
																				<dx:ListEditItem Text="SNMP Trap" Value="4" />
																				<dx:ListEditItem Text="Windows Log" Value="3" />
																			</Items>
																			<ClientSideEvents SelectedIndexChanged="function(s,e){OnSelectedIndexChanged2(s,e)}" />
																			<ValidationSettings CausesValidation="True" ErrorDisplayMode="ImageWithTooltip" ErrorText="Alerting Mechanism field is required.">
																				<RequiredField ErrorText="Alerting Mechanism field is required." IsRequired="True" />
																			</ValidationSettings>
																		</dx:ASPxComboBox>
																	</td>
																</tr>
																<tr>
																	<td valign="top">
																		<dx:ASPxLabel ID="lblSendTo" ClientInstanceName="lblSendTo" runat="server" CssClass="lblsmallFont"
																			Text="Send To: ">
																		</dx:ASPxLabel>
																		<dx:ASPxLabel ID="lblSMSTo" ClientVisible="false" ClientInstanceName="lblSMSTo" runat="server"
																			Text="SMS To:" CssClass="lblsmallFont">
																		</dx:ASPxLabel>
																		<dx:ASPxLabel ID="lblScript" ClientVisible="false" ClientInstanceName="lblScript"
																			runat="server" Text="Script:" CssClass="lblsmallFont">
																		</dx:ASPxLabel>
																	</td>
																	<td valign="top">
																		<dx:ASPxTextBox ID="txtSendTo" ClientInstanceName="txtSendTo" runat="server" Spacing="0"
																			ValidationSettings-ValidationGroup="<%# Container.ValidationGroup %>" Width="170px">
																			<ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorText="">
																				<RegularExpression ErrorText="Not a valid email address" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" />
																			</ValidationSettings>
																			<ClientSideEvents Validation="function(s, e) { 
                                                                                    if (FieldsAreEmptySendTo()){
                                                                                        e.isValid = false;
                                                                                        e.errorText = 'Send to field may not be empty.';
                                                                                    }
                                                                                }" />
																			<%--<ClientSideEvents Init="function(s,e) { s.Validate(); }" />--%>
																		</dx:ASPxTextBox>
																		<div id="divSendto" runat="server" clientidmode="Static" style="display: none">
																		</div>
																		<dx:ASPxHiddenField ID="hidden_phone1" runat="server" ClientInstanceName="hidden_phone1">
																		</dx:ASPxHiddenField>
																		<dx:ASPxTextBox ID="txtSMSTo" ClientInstanceName="txtSMSTo" runat="server" ClientVisible="false"
																			ValidationSettings-ValidationGroup="<%# Container.ValidationGroup %>" Width="170px"
																			ClientIDMode="Static">
																		</dx:ASPxTextBox>
																		<dx:ASPxComboBox ID="cmbScript" ClientVisible="false" ClientInstanceName="cmbScript"
																			runat="server" ValidationSettings-ValidationGroup="<%# Container.ValidationGroup %>"
																			ValueType="System.String">
																			<ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorText="">
																			</ValidationSettings>
																			<ClientSideEvents Validation="function(s, e) { 
                                                                                        if (FieldsAreEmptyScript()){
                                                                                            e.isValid = false;
                                                                                            e.errorText = 'Script field may not be empty.';
                                                                                        }
                                                                                    }" />
																		</dx:ASPxComboBox>
																	</td>
																</tr>
																<tr>
																	<td>
																		<dx:ASPxLabel ID="lblCopyTo" ClientInstanceName="lblCopyTo" runat="server" CssClass="lblsmallFont"
																			Text="Copy To: " Width="100px">
																		</dx:ASPxLabel>
																	</td>
																	<td colspan="2">
																		<dx:ASPxTextBox ID="txtCopyTo" ClientInstanceName="txtCopyTo" runat="server" ValidationSettings-ValidationGroup="<%# Container.ValidationGroup %>"
																			Width="170px">
																			<ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorText="">
																				<%--<RequiredField IsRequired="True" ErrorText=""></RequiredField>--%>
																				<RegularExpression ErrorText="Not a valid email address" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" />
																			</ValidationSettings>
																			<%--<ClientSideEvents Init="function(s,e) { s.Validate(); }" />--%>
																		</dx:ASPxTextBox>
																	</td>
																</tr>
																<tr>
																	<td>
																		<dx:ASPxLabel ID="lblBlindCopyTo" ClientInstanceName="lblBlindCopyTo" runat="server"
																			CssClass="lblsmallFont" Text="Blind Copy To: " Width="100px">
																		</dx:ASPxLabel>
																	</td>
																	<td colspan="2">
																		<dx:ASPxTextBox ID="txtBlindCopyTo" ClientInstanceName="txtBlindCopyTo" runat="server"
																			ValidationSettings-ValidationGroup="<%# Container.ValidationGroup %>" Width="170px">
																			<ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorText="">
																				<%--<RequiredField IsRequired="True" ErrorText=""></RequiredField>--%>
																				<RegularExpression ErrorText="Not a valid email address" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" />
																			</ValidationSettings>
																			<%--<ClientSideEvents Init="function(s,e) { s.Validate(); }" />--%>
																		</dx:ASPxTextBox>
																	</td>
																</tr>
															</table>
														</td>
														<td valign="top">
															<table>
																<tr>
																	<td>
																		<dx:ASPxLabel ID="lblDays" runat="server" ClientInstanceName="clblDays" CssClass="lblsmallFont"
																			Text="Days: ">
																		</dx:ASPxLabel>
																	</td>
																	<td>
																		<dx:ASPxListBox ID="lstBoxDays" runat="server" ClientInstanceName="lstBoxDays" Height="130px"
																			ValueType="System.String" SelectionMode="CheckColumn">
																			<ValidationSettings CausesValidation="True" ErrorDisplayMode="ImageWithTooltip" ErrorFrameStyle-VerticalAlign="Top"
																				ErrorText="Select at least one day.">
																				<RequiredField ErrorText="Select at least one day." IsRequired="True" />
																			</ValidationSettings>
																			<Items>
																				<dx:ListEditItem Text="Monday" Value="Monday" />
																				<dx:ListEditItem Text="Tuesday" Value="Tuesday" Selected="false" />
																				<dx:ListEditItem Text="Wednesday" Value="Wednesday" Selected="false" />
																				<dx:ListEditItem Text="Thursday" Value="Thursday" Selected="false" />
																				<dx:ListEditItem Text="Friday" Value="Friday" Selected="false" />
																				<dx:ListEditItem Text="Saturday" Value="Saturday" Selected="false" />
																				<dx:ListEditItem Text="Sunday" Value="Sunday" Selected="false" />
																			</Items>
																			<%--<ValidationSettings ErrorDisplayMode="ImageWithTooltip"  CausesValidation="true">
																							<RequiredField IsRequired="true" ErrorText="Select at least one day."/>
																							</ValidationSettings>--%>
																		</dx:ASPxListBox>
																	</td>
																</tr>
															</table>
														</td>
														<td valign="top">
															<table>
																<tr>
																	<td colspan="2">
																		<dx:ASPxCheckBox ID="chkboxPersistent" ClientInstanceName="chkboxPersistent" runat="server"
																			Checked='<%# Convert.ToBoolean(Eval("EnablePersistentAlert")) %>' Text="Enable Persistent Alerting">
																		</dx:ASPxCheckBox>
																	</td>
																</tr>
																<tr>
																	<td colspan="2">
																		<dx:ASPxCheckBox ID="chkBxSendSNMPTrap" ClientInstanceName="chkBxSendSNMPTrap" runat="server"
																			Checked='<%# Convert.ToBoolean(Eval("SendSNMPTrap")) %>' Text="Send SNMP Trap"
																			ClientVisible="false">
																		</dx:ASPxCheckBox>
																	</td>
																</tr>
																<tr>
																	<td colspan="2">
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
																	</td>
																	<td>
																	</td>
																</tr>
															</table>
														</td>
													</tr>
													<tr>
														<td>
															&nbsp;
														</td>
														<td>
															&nbsp;
														</td>
														<td align="right">
															<dx:ASPxGridViewTemplateReplacement ID="bttnUpdate" runat="server" ReplacementType="EditFormUpdateButton" />
															<dx:ASPxGridViewTemplateReplacement ID="bttCancel" runat="server" ReplacementType="EditFormCancelButton" />
														</td>
													</tr>
												</table>
											</EditForm>
										</Templates>
									</dx:ASPxGridView>
									<div id="bussinesserror" runat="server" class="alert alert-danger" style="display: none">
										Error attempting to update the status table.
									</div>
								</ContentTemplate>
							</asp:UpdatePanel>
						</td>
					</tr>
					<tr>
						<td colspan="2">
							<div class="subheader" id="Div4" runat="server">
								Escalation</div>
							<script type='text/javascript' src='//ajax.googleapis.com/ajax/libs/jquery/1.9.1/jquery.min.js'></script>
							<script type='text/javascript' src='../js/intlTelInput.min.js'></script>
							<link rel="stylesheet" href="../css/intlTelInput.css">
							<dx:ASPxGridView ID="EscalationGridView" runat="server" Theme="Office2003Blue" Width="100%"
								AutoGenerateColumns="False" OnRowInserting="EscalationGridView_RowInserting"
								KeyFieldName="ID" OnCustomUnboundColumnData="EscalationGridView_CustomUnboundColumnData"
								OnHtmlRowCreated="EscalationGridView_HtmlRowCreated" OnRowDeleting="EscalationGridView_RowDeleting"
								OnRowUpdating="EscalationGridView_RowUpdating" 
								OnCustomJSProperties="EscalationGridView_CustomJSProperties" 
								oncustomerrortext="EscalationGridView_CustomErrorText">
								<ClientSideEvents EndCallback="function(s, e) {
	                                                                    if(s.cpIsEdit)
                                                                          {
                                                                                var telInput = $('#ephone'),
                                                                                errorMsg = $('#e-error-msg'),
                                                                                validMsg = $('#e-valid-msg');                                                                        
                                                                                telInput.intlTelInput({  utilsScript: '../js/utils.js'    });   
                                                                                            
                                                                          if(telInput.val()!='')
                                                                          { 
                                                                              document.getElementById('divESendto').style.value = 'display: block';
                                                                              document.getElementById('divESendto').style.height=0;
                                                                              
                                                                          }
                                                                               
                                                                                //var mJSVariable =telInput.intlTelInput('getNumber');                                                                              
                                                                                var mJSVariable = s.cpSMSTxtTo;
                                                                                var smsconfig = s.cpSMSConfig;
                                                                                var ehiddenPhone = $('#ehidden_phone'); 
                                                                                if (mJSVariable == ''){                                                               
                                                                                    telInput.intlTelInput('setNumber', '');
                                                                                    ehiddenPhone.val(''); 
                                                                                }
                                                                                else{
                                                                                    telInput.intlTelInput('setNumber', '+' + mJSVariable);
                                                                                    ehiddenPhone.val('+' +  mJSVariable); 
                                                                                }
                                                                                var invalid = 1;
                                                                                telInput.blur(function () {
                                                                    
                                                                    if ($.trim(telInput.val())) {
                                                                        if (telInput.intlTelInput('isValidNumber')) {
                                                                            validMsg.removeClass('hide');
                                                                            validMsg.addClass('dispValid');
                                                                            errorMsg.addClass('hide');
                                                                            invalid = 0;
                                                                        } else {
                                                                            errorMsg.removeClass('hide');
                                                                            errorMsg.addClass('dispError');
                                                                            validMsg.addClass('hide');
                                                                        }
                                                                    }
                                                                    var num = telInput.intlTelInput('getNumber');
                                                                    if (invalid == 0){
                                                                        document.getElementById('ehidden_phone').value = num;
                                                                        ehidden_phone1.Set('esmsto', num);
                                                                    }
                                                                    else {
                                                                        document.getElementById('ehidden_phone').value = '';
                                                                        ehidden_phone1.Set('esmsto', 'invalid');
                                                                    }
                                                                    ehidden_smsconfig.Set('smsconfig', smsconfig);
                                                                });

                                                                
                                                                telInput.keydown(function () {
                                                                    errorMsg.addClass('hide');
                                                                    validMsg.addClass('hide');
                                                                });                                                                        
                                                                          }
                                                                        else
                                                                        {
                                                                           
                                                                        }
                                                                        var num = telInput.intlTelInput('getNumber');
                                                                    document.getElementById('ehidden_phone').value = num;
                                                                    ehidden_phone1.Set('esmsto', num);
                                                                    ehidden_smsconfig.Set('smsconfig', smsconfig);
                                                                    //alert(document.getElementById('ehidden_phone').value);
                                                                    }" />
								<Columns>
									<dx:GridViewCommandColumn ButtonType="Image" Caption="Actions" VisibleIndex="0" Width="80px" FixedStyle="Left">
										<HeaderStyle CssClass="GridCssHeader1" />
										<EditButton Visible="True">
											<Image Url="../images/edit.png">
											</Image>
										</EditButton>
										<NewButton Visible="True">
											<Image Url="../images/icons/add.png">
											</Image>
										</NewButton>
										<DeleteButton Visible="True">
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
									</dx:GridViewCommandColumn>
									<dx:GridViewDataTextColumn Caption="Escalate To" FieldName="EscalateTo" VisibleIndex="3"
										Visible="False">
									</dx:GridViewDataTextColumn>
									<dx:GridViewDataTextColumn Caption="SMS To" FieldName="SMSTo" VisibleIndex="4" Visible="False">
									</dx:GridViewDataTextColumn>
									<dx:GridViewDataTextColumn Caption="Script" FieldName="ScriptName" VisibleIndex="5"
										Visible="False">
									</dx:GridViewDataTextColumn>
									<dx:GridViewDataTextColumn Caption="Script ID" FieldName="ScriptID" Visible="False"
										VisibleIndex="6">
									</dx:GridViewDataTextColumn>
									<dx:GridViewDataTextColumn Caption="Escalation Interval (min)" FieldName="EscalationInterval"
										VisibleIndex="1" Width="150px">
										<HeaderStyle CssClass="GridCssHeader2" />
										<CellStyle CssClass="GridCss2">
										</CellStyle>
									</dx:GridViewDataTextColumn>
									<dx:GridViewDataTextColumn Caption="Escalate To" FieldName="EscalateToAll" UnboundType="String"
										VisibleIndex="2">
									</dx:GridViewDataTextColumn>
								</Columns>
                               <SettingsBehavior ColumnResizeMode="NextColumn" ConfirmDelete="true" />
                               <SettingsText ConfirmDelete="Are you sure you want to delete this record?" />
								<Styles>
									<AlternatingRow CssClass="GridAltRow">
									</AlternatingRow>
									<Header CssClass="GridCssHeader">
									</Header>
									<Cell CssClass="GridCss">
									</Cell>
									<EditForm BackColor="White">
									</EditForm>
								</Styles>
                                
								<Templates>
									<EditForm>
										<table>
											<tr>
												<td colspan="4">
													<div id="divESMSToMsg" runat="server" class="alert alert-danger" clientidmode="Static"
														style="display: none">
														The SMS settings are incomplete or not configured on the Alert Settings page.
														<br />
														You will not be able to send SMS until the settings are properly configured.
													</div>
													<dx:ASPxHiddenField ID="ehidden_smsconfig" runat="server" ClientInstanceName="ehidden_smsconfig">
													</dx:ASPxHiddenField>
												</td>
											</tr>
											<tr>
												<td>
													<dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="Alerting Mechanism:" CssClass="lblsmallFont">
													</dx:ASPxLabel>
												</td>
												<td>
													<dx:ASPxComboBox ID="cmbEMechanism" runat="server" ValueType="System.String" ClientInstanceName="cmbEMechanism">
														<Items>
															<dx:ListEditItem Text="E-mail" Value="0" Selected="true" />
															<dx:ListEditItem Text="SMS" Value="1" />
															<dx:ListEditItem Text="Script" Value="2" />
														</Items>
														<ClientSideEvents SelectedIndexChanged="function(s,e){OnSelectedIndexChangedEscalate(s,e)}" />
														<ValidationSettings CausesValidation="True" ErrorDisplayMode="ImageWithTooltip" ErrorText="Alerting Mechanism field is required.">
															<RequiredField ErrorText="Alerting Mechanism field is required." IsRequired="True" />
														</ValidationSettings>
													</dx:ASPxComboBox>
												</td>
												<td>
													<dx:ASPxLabel ID="ASPxLabel3" runat="server" Text="Escalation Interval (min):" CssClass="lblsmallFont">
													</dx:ASPxLabel>
												</td>
												<td>
													<dx:ASPxTextBox ID="txtEDuration" runat="server" ClientInstanceName="txtEDuration"
														ValidationSettings-ValidationGroup="<%# Container.ValidationGroup %>" Width="60px">
														<MaskSettings Mask="&lt;0..9999&gt;" />
														<ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorText="Escalation interval is required">
															<RequiredField ErrorText="" />
															<RegularExpression ErrorText="" />
														</ValidationSettings>
														<ClientSideEvents Validation="function(s, e) { 
                                                                                    if (FieldIsZeroInterval()){
                                                                                        e.isValid = false;
                                                                                        e.errorText = 'Escalation Interval field must be greater than 0.';
                                                                                    }
                                                                                }" />
													</dx:ASPxTextBox>
												</td>
											</tr>
											<tr>
												<td valign="top">
													<dx:ASPxLabel ID="lblESendTo" ClientInstanceName="lblESendTo" runat="server" CssClass="lblsmallFont"
														Text="Escalate To: ">
													</dx:ASPxLabel>
													<dx:ASPxLabel ID="lblESMSTo" ClientVisible="false" ClientInstanceName="lblESMSTo"
														runat="server" Text="SMS To:" CssClass="lblsmallFont">
													</dx:ASPxLabel>
													<dx:ASPxLabel ID="lblEScript" ClientVisible="false" ClientInstanceName="lblEScript"
														runat="server" Text="Script:" CssClass="lblsmallFont">
													</dx:ASPxLabel>
												</td>
												<td valign="top" colspan="4">
													<dx:ASPxTextBox ID="txtESendTo" ClientInstanceName="txtESendTo" runat="server" Spacing="0"
														ValidationSettings-ValidationGroup="<%# Container.ValidationGroup %>" Width="170px">
														<ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorText="">
															<RegularExpression ErrorText="Not a valid email address" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" />
														</ValidationSettings>
														<ClientSideEvents Validation="function(s, e) { 
                                                                                    if (FieldsAreEmptyEscalateTo()){
                                                                                        e.isValid = false;
                                                                                        e.errorText = 'Escalate To field may not be empty.';
                                                                                    }
                                                                                }" />
														<%--<ClientSideEvents Init="function(s,e) { s.Validate(); }" />--%>
													</dx:ASPxTextBox>
													<div id="divESendto" runat="server" clientidmode="Static" style="display: none">
													</div>
													<dx:ASPxHiddenField ID="ehidden_phone1" runat="server" ClientInstanceName="ehidden_phone1">
													</dx:ASPxHiddenField>
													<dx:ASPxTextBox ID="txtESMSTo" ClientVisible="false" ClientInstanceName="txtESMSTo"
														runat="server" ValidationSettings-ValidationGroup="<%# Container.ValidationGroup %>"
														Width="170px">
													</dx:ASPxTextBox>
													<dx:ASPxComboBox ID="cmbEScript" ClientVisible="false" ClientInstanceName="cmbEScript"
														runat="server" ValidationSettings-ValidationGroup="<%# Container.ValidationGroup %>"
														ValueType="System.String">
														<ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorText="">
														</ValidationSettings>
														<ClientSideEvents Validation="function(s, e) { 
                                                                                        if (FieldsAreEmptyEScript()){
                                                                                            e.isValid = false;
                                                                                            e.errorText = 'Script field may not be empty.';
                                                                                        }
                                                                                    }" />
													</dx:ASPxComboBox>
												</td>
											</tr>
											<tr>
												<td>
												</td>
												<td>
												</td>
												<td>
												</td>
												<td align="right">
													<dx:ASPxGridViewTemplateReplacement ID="bttnEUpdate" runat="server" ReplacementType="EditFormUpdateButton" />
													<dx:ASPxGridViewTemplateReplacement ID="bttECancel" runat="server" ReplacementType="EditFormCancelButton" />
												</td>
											</tr>
										</table>
									</EditForm>
								</Templates>
							</dx:ASPxGridView>
						</td>
					</tr>
					<tr>
						<td valign="top">
							<table class="style1">
								<tr>
									<td>
										<div class="subheader" id="Div2" runat="server">
											Events</div>
									</td>
								</tr>
								<tr>
									<td>
										<table>
											<tr>
												<td>
													<asp:Label ID="Alerteventslabel" runat="server" Text="Alert Event Template :" CssClass="lblsmallFont"></asp:Label>
												</td>
												<td>
													<dx:ASPxComboBox ID="AlerteventsComboBox" runat="server" ClientInstanceName="cmbLocation"
														ValueType="System.String" AutoPostBack="True" OnSelectedIndexChanged="AlerteventsComboBox_SelectedIndexChanged">
														<ValidationSettings ErrorDisplayMode="ImageWithTooltip">
															<RequiredField ErrorText="Select Profile." IsRequired="True" />
															<RequiredField IsRequired="True" ErrorText="Select Location."></RequiredField>
														</ValidationSettings>
													</dx:ASPxComboBox>
												</td>
											</tr>
										</table>
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
																OnClick="CollapseAllButton_Click">
																<Image Url="~/images/icons/forbidden.png">
																</Image>
															</dx:ASPxButton>
														</td>
													</tr>
													<%--	<tr>
														<td>
															<dx:ASPxButton ID="NewEventTemplate" runat="server" Text="New Event Template" CssClass="sysButton"
																Wrap="False" OnClick="NewEventTemplate_Click" CausesValidation="false">
																<Image Url="~/images/icons/forbidden.png">
																</Image>
															</dx:ASPxButton>
														</td>
													</tr>--%>
													<tr id="evet1" runat="server" visible="false">
														<td valign="top">
															<dx:ASPxTreeList ID="EventsTreeList" ClientInstanceName="eventsTree" runat="server"
																AutoGenerateColumns="False" OnPageSizeChanged="EventsTreeList_PageSizeChanged"
																CssClass="lblsmallFont" KeyFieldName="Id" ParentFieldName="SrvId" Theme="Office2003Blue"
																Width="100%">
																<Columns>
																	<dx:TreeListTextColumn Caption="Events  " FieldName="Name" Name="Events" VisibleIndex="0"
																		ReadOnly="True">
																		<EditFormSettings Visible="False" />
																		<HeaderStyle CssClass="lblsmallFont" />
																	</dx:TreeListTextColumn>
																	<dx:TreeListTextColumn FieldName="actid" Name="actid" Visible="False" VisibleIndex="2">
																	</dx:TreeListTextColumn>
																	<dx:TreeListTextColumn FieldName="tbl" Name="tbl" Visible="False" VisibleIndex="3">
																	</dx:TreeListTextColumn>
																	<dx:TreeListTextColumn FieldName="SrvId" Name="SrvId" Visible="False" VisibleIndex="4">
																	</dx:TreeListTextColumn>
																	<dx:TreeListCheckColumn Caption="Alert on Event Recurrence" Name="ConsecutiveFailures"
																		VisibleIndex="1" FieldName="AlertOnRepeat">
																		<PropertiesCheckEdit>
																			<CheckBoxStyle CssClass="lblsmallFont" />
																		</PropertiesCheckEdit>
																		<HeaderStyle CssClass="lblsmallFont" />
																		<CellStyle CssClass="GridCss1">
																		</CellStyle>
																	</dx:TreeListCheckColumn>
																</Columns>
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
						</td>
						<td valign="top">
							<table class="style1">
								<tr>
									<td>
										<div class="subheader" id="Div3" runat="server">
											Servers</div>
									</td>
								</tr>
								<tr>
									<td>
										<asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
											<ContentTemplate>
												<table>
													<tr>
														<td valign="top" style="height: 22px">
															&nbsp
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
														<td valign="top">
															<dx:ASPxTreeList ID="ServersTreeList" runat="server" AutoGenerateColumns="False"
																CssClass="lblsmallFont" KeyFieldName="Id" OnPageSizeChanged="ServersTreeList_PageSizeChanged"
																ParentFieldName="LocId" Theme="Office2003Blue" Width="100%" OnDataBound="DataBound">
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
                                                                 <Summary>
                                                        <dx:TreeListSummaryItem   FieldName="Name" SummaryType="Count" ShowInColumn="Name" DisplayFormat="{0} Item(s)" /></Summary>
																<Settings GridLines="Both" SuppressOuterGridLines="True" />
																<SettingsBehavior AllowDragDrop="False" AutoExpandAllNodes="True" />
																<Settings GridLines="Both" SuppressOuterGridLines="True" />
																<SettingsBehavior AllowDragDrop="False" AutoExpandAllNodes="True" />
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
												</table>
											</ContentTemplate>
											<Triggers>
												<asp:AsyncPostBackTrigger ControlID="CollapseAllSrvButton" />
											</Triggers>
										</asp:UpdatePanel>
									</td>
								</tr>
							</table>
						</td>
					</tr>
				</table>
			</td>
		</tr>
		<tr>
			<td colspan="2">
				<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
					<ContentTemplate>
						<div id="successDiv" class="alert alert-success" runat="server" style="display: none">
							Success.
						</div>
						<div id="errorDiv" class="alert alert-danger" runat="server" style="display: none">
							Error.
						</div>
						<table>
							<tr>
								<td>
									<dx:ASPxButton ID="OKButton" runat="server" OnClick="OKButton_Click" Text="OK" CssClass="sysButton"
                                     ClientEnabled="true" ClientInstanceName="OKButton">
                                        <ClientSideEvents Click="function(s, e) {
														LoadingPanel.Show();
														}" />
									</dx:ASPxButton>
								</td>
								<td>
									<dx:ASPxButton ID="CancelButton" runat="server" CausesValidation="False" OnClick="CancelButton_Click"
										Text="Cancel" CssClass="sysButton">
									</dx:ASPxButton>
								</td>
							</tr>
						</table>
					</ContentTemplate>
					<Triggers>
						<asp:AsyncPostBackTrigger ControlID="OKButton" />
					</Triggers>
				</asp:UpdatePanel>
			</td>
		</tr>
	</table>
	<dx:ASPxPopupControl ID="ErrorMessagePopupControl" runat="server" AllowDragging="True"
		ClientInstanceName="pcErrorMessage" CloseAction="CloseButton" EnableAnimation="False"
		EnableViewState="False" HeaderText="Validation Failure" Height="150px" Modal="True"
		PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" Width="300px"
		Theme="Glass">
		<ContentStyle BackColor="White">
		</ContentStyle>
		<HeaderStyle BackColor="#CCCCCC">
			<Paddings PaddingLeft="10px" PaddingRight="6px" PaddingTop="1px" />
			<Paddings PaddingLeft="10px" PaddingTop="1px" PaddingRight="6px"></Paddings>
		</HeaderStyle>
		<ContentCollection>
			<dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server" SupportsDisabledAttribute="True">
				<dx:ASPxPanel ID="Panel1" runat="server" DefaultButton="btOK">
					<PanelCollection>
						<dx:PanelContent ID="PanelContent4" runat="server" SupportsDisabledAttribute="True">
							<div style="min-height: 70px;">
								&nbsp;&nbsp;&nbsp;
								<dx:ASPxLabel ID="ErrorMessageLabel" runat="server" Text="msglabel">
								</dx:ASPxLabel>
							</div>
							<div>
								<table cellpadding="0" cellspacing="0" width="100%">
									<tr>
										<td align="center">
											<dx:ASPxButton ID="ValidationOkButton" runat="server" AutoPostBack="False" Text="OK"
												Width="80px" Theme="Office2010Blue">
												<ClientSideEvents Click="function(s, e) { pcErrorMessage.Hide(); }" />
												<ClientSideEvents Click="function(s, e) { pcErrorMessage.Hide(); }"></ClientSideEvents>
											</dx:ASPxButton>
											<dx:ASPxButton ID="ValidationUpdatedButton" runat="server" AutoPostBack="False" OnClick="ValidationUpdatedButton_Click"
												Text="OK" Visible="False" Width="80px" Theme="Office2010Blue">
												<ClientSideEvents Click="function(s, e) { pcErrorMessage.Hide(); }" />
												<ClientSideEvents Click="function(s, e) { pcErrorMessage.Hide(); }"></ClientSideEvents>
											</dx:ASPxButton>
										</td>
									</tr>
								</table>
							</div>
						</dx:PanelContent>
					</PanelCollection>
				</dx:ASPxPanel>
			</dx:PopupControlContentControl>
		</ContentCollection>
	</dx:ASPxPopupControl>
</asp:Content>
