<%@ Page Title="VitalSigns Plus-Domino Event Log Definitions" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="DominoELS_Edit.aspx.cs" Inherits="VSWebUI.Configurator.DominoELS_Edit" %>
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
				DELSGridView.GetRowValues(e.visibleIndex, 'Keyword', OnGetRowValues);

			function OnGetRowValues(values) {
				var id = values[0];
				var name = values[1];
				var OK = (confirm('Are you sure you want to delete the keyword - ' + values + '?'))
				if (OK == true) {
					//					alert('Before Delete');
					//					var name1 = ServersGridView.DeleteRow(visibleIndex);
					DELSGridView.DeleteRow(visibleIndex);
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
            <div class="header" id="servernamelbldisp" runat="server">Domino Event Log Scanning</div>
            <div id="successDiv" class="alert alert-success" runat="server" style="display: none">Success.
                </div>
            <asp:Label ID="lblmessage" runat="server" Style="font-weight: bold; color: #f00;"> </asp:Label>
          
			<div id="errormsg" runat="server" class="alert alert-danger" style="display: none">
								Error attempting to update the status table.
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
							<dx:ASPxTextBox ID="EventNameTextBox" runat="server" Width="170px"  >
							
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
                        <ClientSideEvents Click="function() { DELSGridView.AddNewRow(); }" />
                                <Image Url="~/images/icons/add.png">
                                </Image>
                    </dx:ASPxButton>
                </td>
            </tr>
	<tr>
			<td>
            <%--<dx:ASPxGridView runat="server" 
                                KeyFieldName="ID" AutoGenerateColumns="False" 
                                ID="DELSGridView" 
				 ClientInstanceName="DELSGridView"
                                OnRowInserting="DELSGridView_RowInserting" 
                               OnRowUpdating="DELSGridView_RowUpdating"
							   OnRowDeleting="DELSGridView_RowDeleting"
							   oncelleditorinitialize="DELSGridView_CellEditorInitialize" 
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
            <dx:GridViewDataTextColumn FieldName="EventId" VisibleIndex="3" visible="false" Caption="Event ID">
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
			 <dx:GridViewDataTextColumn FieldName="Keyword" VisibleIndex="2" Caption="Keywords">
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
            <dx:GridViewDataTextColumn Caption="Exclude scanning for" FieldName="NotRequiredKeyword" 
                VisibleIndex="3">
                
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

		<dx:GridViewDataCheckColumn Caption="Limit to one alert per day" Width="120px"
                                        VisibleIndex="4" FieldName="RepeatOnce">
										<Settings AllowAutoFilter="False" />
										<EditCellStyle CssClass="GridCss">
										</EditCellStyle>
										<EditFormCaptionStyle CssClass="GridCss">
										</EditFormCaptionStyle>
										<HeaderStyle CssClass="GridCssHeader" Wrap="True">
											<Paddings Padding="5px" />
										</HeaderStyle>
										<CellStyle CssClass="GridCss1">
										</CellStyle>
									</dx:GridViewDataCheckColumn>
		<dx:GridViewDataCheckColumn Caption="Scan log.nsf" VisibleIndex="5" Width="80px"
                                        FieldName="Log">
										<Settings AllowAutoFilter="False" />
										<EditCellStyle CssClass="GridCss">
										</EditCellStyle>
										<EditFormCaptionStyle CssClass="GridCss">
										</EditFormCaptionStyle>
										<HeaderStyle CssClass="GridCssHeader" Wrap="True">
											<Paddings Padding="5px" />
										</HeaderStyle>
										<CellStyle CssClass="GridCss1">
										</CellStyle>
									</dx:GridViewDataCheckColumn>
		<dx:GridViewDataCheckColumn Caption="Scan AgentLog.nsf " VisibleIndex="6" Width="100px"
                                        FieldName="AgentLog">
										<Settings AllowAutoFilter="False" />
										<EditCellStyle CssClass="GridCss">
										</EditCellStyle>
										<EditFormCaptionStyle CssClass="GridCss" >
										</EditFormCaptionStyle>
										<HeaderStyle CssClass="GridCssHeader" Wrap="True">
											<Paddings Padding="5px" />
										</HeaderStyle>
										<CellStyle CssClass="GridCss1">
										</CellStyle>
									</dx:GridViewDataCheckColumn>

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
    </dx:ASPxGridView>--%>
    <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
								<ContentTemplate>
	<dx:ASPxGridView ID="DELSGridView" runat="server" AutoGenerateColumns="False" ClientInstanceName="DELSGridView"
										EnableTheming="True" KeyFieldName="ID" 
										OnHtmlRowCreated="DELSGridView_HtmlRowCreated"
										 OnRowDeleting="DELSGridView_RowDeleting"
										OnRowInserting="DELSGridView_RowInserting" OnRowUpdating="DELSGridView_RowUpdating"
										Theme="Office2003Blue" Width="100%"
										
										
										oncustomerrortext="DELSGridView_CustomErrorText">
									 <ClientSideEvents CustomButtonClick="OnCustomButtonClick" />
										 <Columns>

            <dx:GridViewCommandColumn ButtonType="Image" Caption="Actions" 
                                        FixedStyle="Left" VisibleIndex="0" Width="100px">
                <EditButton Visible="True">
                    <Image Url="../images/edit.png">
                    </Image>
                </EditButton>
                
                <DeleteButton Visible="False">
                    <Image Url="../images/delete.png">
                    </Image>
                </DeleteButton>
				<NewButton Visible="True">
													<Image Url="../images/icons/add.png">
													</Image>
												</NewButton>
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
            <dx:GridViewDataTextColumn FieldName="DominoEventLogId" VisibleIndex="3" visible="false" Caption="DominoEventLogId">
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
			 <dx:GridViewDataTextColumn FieldName="Keyword" VisibleIndex="4" Caption="Keywords">
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
            <dx:GridViewDataTextColumn Caption="Exclude scanning for" FieldName="NotRequiredKeyword" 
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

		<dx:GridViewDataCheckColumn Caption="Limit to one alert per day" Width="120px"
                                        VisibleIndex="6" FieldName="RepeatOnce">
										<Settings AllowAutoFilter="False" />
										<EditCellStyle CssClass="GridCss">
										</EditCellStyle>
										<EditFormCaptionStyle CssClass="GridCss">
										</EditFormCaptionStyle>
										<HeaderStyle CssClass="GridCssHeader" Wrap="True">
											<Paddings Padding="5px" />
										</HeaderStyle>
										<CellStyle CssClass="GridCss1">
										</CellStyle>
									</dx:GridViewDataCheckColumn>
		<dx:GridViewDataCheckColumn Caption="Scan log.nsf" VisibleIndex="7" Width="80px"
                                        FieldName="Log">
										<Settings AllowAutoFilter="False" />
										<EditCellStyle CssClass="GridCss">
										</EditCellStyle>
										<EditFormCaptionStyle CssClass="GridCss">
										</EditFormCaptionStyle>
										<HeaderStyle CssClass="GridCssHeader" Wrap="True">
											<Paddings Padding="5px" />
										</HeaderStyle>
										<CellStyle CssClass="GridCss1">
										</CellStyle>
									</dx:GridViewDataCheckColumn>
		<dx:GridViewDataCheckColumn Caption="Scan AgentLog.nsf " VisibleIndex="8" Width="100px"
                                        FieldName="AgentLog">
										<Settings AllowAutoFilter="False" />
										<EditCellStyle CssClass="GridCss">
										</EditCellStyle>
										<EditFormCaptionStyle CssClass="GridCss" >
										</EditFormCaptionStyle>
										<HeaderStyle CssClass="GridCssHeader" Wrap="True">
											<Paddings Padding="5px" />
										</HeaderStyle>
										<CellStyle CssClass="GridCss1">
										</CellStyle>
									</dx:GridViewDataCheckColumn>

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
												<td>
													&nbsp;
												</td>
												<td colspan="2">
													<%--<asp:HiddenField ID="LogFileIDHiddenField" runat="server" Value='<%# Eval("ID") %>' />--%>
													<asp:Label ID="Label2" runat="server" CssClass="lblsmallFont" Text="VitalSigns will search the selected logs for the word or phrase in the Keywords field. If you would like to be alerted &lt;b&gt;every time&lt;/b&gt; the word/phrase is found, do not check the limit box."></asp:Label>
												</td>
											</tr>
                                            <tr>
                                                <td colspan="3">
                                                    &nbsp;
                                                </td>
                                            </tr>
											<tr>
												<td>
													&nbsp;
												</td>
												<td>
													<dx:ASPxLabel ID="ASPxLabel3" runat="server" Text="Keywords:" CssClass="lblsmallFont">
													</dx:ASPxLabel>
												</td>
												<td>
													<dx:ASPxTextBox ID="LogFileTextBox" runat="server" Width="350px" ValidationSettings-ValidationGroup="<%# Container.ValidationGroup %>"
													 Value='<%#Eval("Keyword")%>'>
													 <ValidationSettings ErrorText="Enter Keywords">
                                                     <RegularExpression ValidationExpression="^.{1,255}$"  ErrorText="Allows upto 255 characters only"  />

													<RequiredField IsRequired="true" ErrorText="Please Enter Keywords" />
													</ValidationSettings>
													</dx:ASPxTextBox>
												</td>
											</tr>
											<tr>
												<td>
												</td>
												<td>
													<dx:ASPxLabel ID="ASPxLabel4" runat="server" Text="Exclude scanning for:" CssClass="lblsmallFont">
													</dx:ASPxLabel>
												</td>
												<td>
													<dx:ASPxTextBox ID="NotLogFileTextBox" runat="server" Width="350px" ValidationSettings-ValidationGroup="<%# Container.ValidationGroup %>"
													Value='<%#Eval("NotRequiredKeyword") %>'>
													<%--<ValidationSettings ErrorText="Enter Exclude scanning ">
																				<RequiredField IsRequired="true" ErrorText="Please Enter Exclude scanning " />
																				</ValidationSettings>--%>
													</dx:ASPxTextBox>
												</td>
											</tr>
											<tr>
												<td colspan="2">
													&nbsp;
												</td>
												<td>
													<table>
														<tr>
															<td>
																<dx:ASPxCheckBox ID="logCheckBox" runat="server" Text=" Scan log.nsf " CheckState="Unchecked"
																	Value='<%#Eval("Log") %>'>
																</dx:ASPxCheckBox>
															</td>
															<td>
																<dx:ASPxCheckBox ID="LogFileCheckBox" runat="server" Text="Limit to one Alert per day"
																	CheckState="Unchecked" Value='<%#Eval("RepeatOnce") %>'>
																</dx:ASPxCheckBox>
															</td>
														</tr>
														<tr>
															<td>
																<dx:ASPxCheckBox ID="AgentlogCheckBox" runat="server" Text=" Scan AgentLog.nsf  "
																	CheckState="Unchecked" Value='<%#Eval("AgentLog") %>'>
																</dx:ASPxCheckBox>
															</td>
															<td>
															</td>
														</tr>
													</table>
												</td>
											</tr>
											<tr>
												<td colspan="3" align="right">
													<dx:ASPxGridViewTemplateReplacement ID="UpdateButton" runat="server" ReplacementType="EditFormUpdateButton" />
													<dx:ASPxGridViewTemplateReplacement ID="CancelButton" runat="server" ReplacementType="EditFormCancelButton" />
												</td>
											</tr>
										</table>
									</EditForm>
										</Templates>
									</dx:ASPxGridView>
                                    	</ContentTemplate>
							</asp:UpdatePanel>
        </td>

    </tr>
	<tr>
	<td>
	<div id="pnlAreaDtls" style="height: 100%; width: 100%;visibility:hidden" runat="server" class="pnlDetails12">
	</div>
	<table>
		<tr>
			<td>
				<div class="header" id="Div3" runat="server"></div>
			</td>
		</tr>
        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
											<ContentTemplate>  
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
        </ContentTemplate>
											<Triggers>
												<asp:AsyncPostBackTrigger ControlID="CollapseAllSrvButton" />
											</Triggers>
										</asp:UpdatePanel>
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
                         <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
											<ContentTemplate>  
                                              <div id="errorDiv" class="alert alert-danger" runat="server" style="display: none">Error.
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

